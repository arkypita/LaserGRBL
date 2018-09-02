/*
 * Created by SharpDevelop.
 * User: Diego
 * Date: 19/11/2016
 * Time: 10:37
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace Tools
{
	/// <summary>
	/// Description of MathHelper.
	/// </summary>
	public static class MathHelper
	{

		public static decimal LinearDistance(decimal curX, decimal curY,decimal newX, decimal newY)
		{
			decimal dX = newX - curX;
			decimal dY = newY - curY;
			return (decimal)Math.Sqrt((double)(dX * dX + dY * dY));
		}
		
		public static decimal ArcDistance(decimal curX, decimal curY, decimal newX, decimal newY, double radius)
		{
			try
			{
			decimal dCX = newX - curX;
			decimal dCY = newY - curY;
			double chordLenght = Math.Sqrt((double)(dCX * dCX + dCY * dCY));
			return (decimal)ArcLengthFromChord(radius, chordLenght);
			}
			catch{return 0;}
		}
		
		public static double CalculateAngle(double x1, double y1, double x2, double y2)
		{
			// returns Angle of line between 2 points and X axis (according to quadrants)
			double Angle = 0;

			if (x1 == x2 && y1 == y2) // same points
				return 0;
			else if (x1 == x2) // 90 or 270
			{
				Angle = Math.PI / 2;
				if (y1 > y2) Angle += Math.PI;
			}
			else if (y1 == y2) // 0 or 180
			{
				Angle = 0;
				if (x1 > x2) Angle += Math.PI;
			}
			else
			{
				Angle = Math.Atan(Math.Abs((y2 - y1) / (x2 - x1))); // 1. quadrant
				if (x1 > x2 && y1 < y2) // 2. quadrant
					Angle = Math.PI - Angle;
				else if (x1 > x2 && y1 > y2) // 3. quadrant
					Angle += Math.PI;
				else if (x1 < x2 && y1 > y2) // 4. quadrant
					Angle = 2 * Math.PI - Angle;
			}
			return Angle * 180 / Math.PI;
		}
		
		public static double AngularDistance(double aA, double bA, bool cw)
		{
			if (cw)
				return bA > aA ? (bA - 360 - aA) : bA - aA;
			else
				return - (aA > bA ? (aA - 360 - bA) : aA - bA);
		}
		
		private static double ArcLengthFromChord(double radius, double chordLength) //Get the arc length from the radius and chordLength
		{ return radius == 0.0 ? chordLength : radius * AngleFromChord(radius, chordLength); } //radius * radians

		private static double AngleFromChord(double radius, double chordLength) //  Get the angle from the chordLength (only works for angles <= 180)
		{return radius != 0.0 ? 2.0 * Math.Asin(chordLength / (radius * 2.0)) : 0.0;}
		
	}
}
