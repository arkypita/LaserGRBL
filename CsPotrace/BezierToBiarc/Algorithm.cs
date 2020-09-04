﻿using System;
using System.Collections.Generic;
using System.Numerics;

//Copyright (C) 2001-2016 Peter Selinger
//Copyright (C) 2009-2016 Wolfgang Nagl

// This program is free software; you can redistribute it and/or modify  it under the terms of the GNU General Public License as published by  the Free Software Foundation; either version 2 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU  General Public License for more details.
// You should have received a copy of the GNU General Public License  along with this program; if not, write to the Free Software  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307,  USA. 

namespace CsPotrace.BezierToBiarc
{
    public class Algorithm
    {

        private static bool IsRealInflexionPoint(Complex t)
        {
            return t.Imaginary == 0 && t.Real > 0 && t.Real < 1;
        }

        /// <summary>
        /// Algorithm to approximate a bezier curve with biarcs
        /// </summary>
        /// <param name="bezier">The bezier curve to be approximated.</param>
        /// <param name="samplingStep">The sampling step used for calculating the approximation error. The length of the arc is divided by this number to get the number of sampling points.</param>
        /// <param name="tolerance">The approximation is accepted if the maximum devation at the sampling points is smaller than this number.</param>
        /// <returns></returns>
        public static List<BiArc> ApproxCubicBezier(CubicBezier bezier, float samplingStep, float tolerance)
        {
            // The result will be put here
            List<BiArc> biarcs = new List<BiArc>();

            // The bezier curves to approximate
            Stack<CubicBezier> curves = new Stack<CubicBezier>();
            curves.Push(bezier);

            // ---------------------------------------------------------------------------
            // First, calculate the inflexion points and split the bezier at them (if any)

            CubicBezier toSplit = curves.Pop();
            Tuple<Complex, Complex> inflex = toSplit.InflexionPoints;

            bool i1 = IsRealInflexionPoint(inflex.Item1);
            bool i2 = IsRealInflexionPoint(inflex.Item2);

            if (i1 && !i2)
            {
                Tuple<CubicBezier, CubicBezier> splited = toSplit.Split((float)inflex.Item1.Real);
                curves.Push(splited.Item2);
                curves.Push(splited.Item1);
            }
            else if (!i1 && i2)
            {
                Tuple<CubicBezier, CubicBezier> splited = toSplit.Split((float)inflex.Item2.Real);
                curves.Push(splited.Item2);
                curves.Push(splited.Item1);
            }
            else if (i1 && i2)
            {
                float t1 = (float)inflex.Item1.Real;
                float t2 = (float)inflex.Item2.Real;

                // I'm not sure if I need, but it does not hurt to order them
                if (t1 > t2)
                {
                    float tmp = t1;
                    t1 = t2;
                    t2 = tmp;
                }

                // Make the first split and save the first new curve. The second one has to be splitted again
                // at the recalculated t2 (it is on a new curve)

                Tuple<CubicBezier, CubicBezier> splited1 = toSplit.Split(t1);

                t2 = (1 - t1) * t2;

                toSplit = splited1.Item2;
                Tuple<CubicBezier, CubicBezier> splited2 = toSplit.Split(t2);

                curves.Push(splited2.Item2);
                curves.Push(splited2.Item1);
                curves.Push(splited1.Item1);
            }
            else
            {
                curves.Push(toSplit);
            }

            // ---------------------------------------------------------------------------
            // Second, approximate the curves until we run out of them

            while (curves.Count > 0)
            {
                bezier = curves.Pop();

                // ---------------------------------------------------------------------------
                // Calculate the transition point for the BiArc 

                // V: Intersection point of tangent lines
                Line T1 = new Line(bezier.P1, bezier.C1);
                Line T2 = new Line(bezier.P2, bezier.C2);
                Vector2 V = T1.Intersection(T2);

                // G: incenter point of the triangle (P1, V, P2)
                // http://www.mathopenref.com/coordincenter.html
                float dP2V = Vector2.Distance(bezier.P2, V);
                float dP1V = Vector2.Distance(bezier.P1, V);
                float dP1P2 = Vector2.Distance(bezier.P1, bezier.P2);
                Vector2 G = (dP2V * bezier.P1 + dP1V * bezier.P2 + dP1P2 * V) / (dP2V + dP1V + dP1P2);

                // ---------------------------------------------------------------------------
                // Calculate the BiArc

                BiArc biarc = new BiArc(bezier.P1, (bezier.P1 - bezier.C1), bezier.P2, (bezier.P2 - bezier.C2), G);

                // ---------------------------------------------------------------------------
                // Calculate the maximum error

                float maxDistance = 0f;
                float maxDistanceAt = 0f;

                float nrPointsToCheck = biarc.Length / samplingStep;
                float parameterStep = 1f / nrPointsToCheck;

                if (nrPointsToCheck > 1000)
                    throw new InvalidOperationException("Too many points to process");

                for (int i = 0; i <= nrPointsToCheck; i++)
                {
                    float t = parameterStep * i;
                    Vector2 u1 = biarc.PointAt(t);
                    Vector2 u2 = bezier.PointAt(t);
                    float distance = (u1 - u2).Length();

                    if (distance > maxDistance)
                    {
                        maxDistance = distance;
                        maxDistanceAt = t;
                    }
                }

                // Check if the two curves are close enough
                if (maxDistance > tolerance)
                {
                    // If not, split the bezier curve the point where the distance is the maximum
                    // and try again with the two halfs
                    Tuple<CubicBezier, CubicBezier> bs = bezier.Split(maxDistanceAt);
                    curves.Push(bs.Item2);
                    curves.Push(bs.Item1);
                }
                else
                {
                    // Otherwise we are done with the current bezier
                    biarcs.Add(biarc);
                }
            }

            return biarcs;
        }

    }
}
