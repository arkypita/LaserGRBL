using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

using Svg.Pathing;

namespace Svg
{
    public static class PointFExtensions
    {
        public static string ToSvgString(this PointF p)
        {
            return p.X.ToString() + " " + p.Y.ToString();
        }
    }

    public class SvgPathBuilder : TypeConverter
    {
        /// <summary>
        /// Parses the specified string into a collection of path segments.
        /// </summary>
        /// <param name="path">A <see cref="string"/> containing path data.</param>
        public static SvgPathSegmentList Parse(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException("path");
            }

            var segments = new SvgPathSegmentList();

            try
            {
                char command;
                bool isRelative;

                foreach (var commandSet in SplitCommands(path.TrimEnd(null)))
                {
                    command = commandSet[0];
                    isRelative = char.IsLower(command);
                    // http://www.w3.org/TR/SVG11/paths.html#PathDataGeneralInformation

                    CreatePathSegment(command, segments, new CoordinateParser(commandSet.Trim()), isRelative);
                }
            }
            catch (Exception exc)
            {
                Trace.TraceError("Error parsing path \"{0}\": {1}", path, exc.Message);
            }

            return segments;
        }

        private static void CreatePathSegment(char command, SvgPathSegmentList segments, CoordinateParser parser, bool isRelative)
        {

            var coords = new float[6];

            switch (command)
            {
                case 'm': // relative moveto
                case 'M': // moveto
                    if (parser.TryGetFloat(out coords[0]) && parser.TryGetFloat(out coords[1]))
                    {
                        segments.Add(new SvgMoveToSegment(ToAbsolute(coords[0], coords[1], segments, isRelative)));
                    }

                    while (parser.TryGetFloat(out coords[0]) && parser.TryGetFloat(out coords[1]))
                    {
                        segments.Add(new SvgLineSegment(segments.Last.End,
                            ToAbsolute(coords[0], coords[1], segments, isRelative)));
                    }
                    break;
                case 'a':
                case 'A':
                    bool size;
                    bool sweep;

                    while (parser.TryGetFloat(out coords[0]) && parser.TryGetFloat(out coords[1]) &&
                           parser.TryGetFloat(out coords[2]) && parser.TryGetBool(out size) &&
                           parser.TryGetBool(out sweep) && parser.TryGetFloat(out coords[3]) &&
                           parser.TryGetFloat(out coords[4]))
                    {
                        // A|a rx ry x-axis-rotation large-arc-flag sweep-flag x y
                        segments.Add(new SvgArcSegment(segments.Last.End, coords[0], coords[1], coords[2],
                            (size ? SvgArcSize.Large : SvgArcSize.Small), 
                            (sweep ? SvgArcSweep.Positive : SvgArcSweep.Negative), 
                            ToAbsolute(coords[3], coords[4], segments, isRelative)));
                    }
                    break;
                case 'l': // relative lineto
                case 'L': // lineto
                    while (parser.TryGetFloat(out coords[0]) && parser.TryGetFloat(out coords[1]))
                    {
                        segments.Add(new SvgLineSegment(segments.Last.End,
                            ToAbsolute(coords[0], coords[1], segments, isRelative)));
                    }
                    break;
                case 'H': // horizontal lineto
                case 'h': // relative horizontal lineto
                    while (parser.TryGetFloat(out coords[0]))
                    {
                        segments.Add(new SvgLineSegment(segments.Last.End,
                            ToAbsolute(coords[0], segments.Last.End.Y, segments, isRelative, false)));
                    }
                    break;
                case 'V': // vertical lineto
                case 'v': // relative vertical lineto
                    while (parser.TryGetFloat(out coords[0]))
                    {
                        segments.Add(new SvgLineSegment(segments.Last.End,
                            ToAbsolute(segments.Last.End.X, coords[0], segments, false, isRelative)));
                    }
                    break;
                case 'Q': // curveto
                case 'q': // relative curveto
                    while (parser.TryGetFloat(out coords[0]) && parser.TryGetFloat(out coords[1]) &&
                           parser.TryGetFloat(out coords[2]) && parser.TryGetFloat(out coords[3]))
                    {
                        segments.Add(new SvgQuadraticCurveSegment(segments.Last.End,
                            ToAbsolute(coords[0], coords[1], segments, isRelative),
                            ToAbsolute(coords[2], coords[3], segments, isRelative)));
                    }
                    break;
                case 'T': // shorthand/smooth curveto
                case 't': // relative shorthand/smooth curveto
                    while (parser.TryGetFloat(out coords[0]) && parser.TryGetFloat(out coords[1]))
                    {
                        var lastQuadCurve = segments.Last as SvgQuadraticCurveSegment;

                        var controlPoint = lastQuadCurve != null
                            ? Reflect(lastQuadCurve.ControlPoint, segments.Last.End)
                            : segments.Last.End;

                        segments.Add(new SvgQuadraticCurveSegment(segments.Last.End, controlPoint,
                            ToAbsolute(coords[0], coords[1], segments, isRelative)));
                    }
                    break;
                case 'C': // curveto
                case 'c': // relative curveto
                    while (parser.TryGetFloat(out coords[0]) && parser.TryGetFloat(out coords[1]) &&
                           parser.TryGetFloat(out coords[2]) && parser.TryGetFloat(out coords[3]) &&
                           parser.TryGetFloat(out coords[4]) && parser.TryGetFloat(out coords[5]))
                    {
                        segments.Add(new SvgCubicCurveSegment(segments.Last.End,
                            ToAbsolute(coords[0], coords[1], segments, isRelative),
                            ToAbsolute(coords[2], coords[3], segments, isRelative),
                            ToAbsolute(coords[4], coords[5], segments, isRelative)));
                    }
                    break;
                case 'S': // shorthand/smooth curveto
                case 's': // relative shorthand/smooth curveto
                    while (parser.TryGetFloat(out coords[0]) && parser.TryGetFloat(out coords[1]) &&
                           parser.TryGetFloat(out coords[2]) && parser.TryGetFloat(out coords[3]))
                    {
                        var lastCubicCurve = segments.Last as SvgCubicCurveSegment;

                        var controlPoint = lastCubicCurve != null
                            ? Reflect(lastCubicCurve.SecondControlPoint, segments.Last.End)
                            : segments.Last.End;

                        segments.Add(new SvgCubicCurveSegment(segments.Last.End, controlPoint,
                            ToAbsolute(coords[0], coords[1], segments, isRelative),
                            ToAbsolute(coords[2], coords[3], segments, isRelative)));
                    }
                    break;
                case 'Z': // closepath
                case 'z': // relative closepath
                    segments.Add(new SvgClosePathSegment());
                    break;
            }
        }

        private static PointF Reflect(PointF point, PointF mirror)
        {
            float x, y, dx, dy;
            dx = Math.Abs(mirror.X - point.X);
            dy = Math.Abs(mirror.Y - point.Y);

            if (mirror.X >= point.X)
            {
                x = mirror.X + dx;
            }
            else
            {
                x = mirror.X - dx;
            }
            if (mirror.Y >= point.Y)
            {
                y = mirror.Y + dy;
            }
            else
            {
                y = mirror.Y - dy;
            }

            return new PointF(x, y);
        }

        /// <summary>
        /// Creates point with absolute coorindates.
        /// </summary>
        /// <param name="x">Raw X-coordinate value.</param>
        /// <param name="y">Raw Y-coordinate value.</param>
        /// <param name="segments">Current path segments.</param>
        /// <param name="isRelativeBoth"><b>true</b> if <paramref name="x"/> and <paramref name="y"/> contains relative coordinate values, otherwise <b>false</b>.</param>
        /// <returns><see cref="PointF"/> that contains absolute coordinates.</returns>
        private static PointF ToAbsolute(float x, float y, SvgPathSegmentList segments, bool isRelativeBoth)
        {
            return ToAbsolute(x, y, segments, isRelativeBoth, isRelativeBoth);
        }

        /// <summary>
        /// Creates point with absolute coorindates.
        /// </summary>
        /// <param name="x">Raw X-coordinate value.</param>
        /// <param name="y">Raw Y-coordinate value.</param>
        /// <param name="segments">Current path segments.</param>
        /// <param name="isRelativeX"><b>true</b> if <paramref name="x"/> contains relative coordinate value, otherwise <b>false</b>.</param>
        /// <param name="isRelativeY"><b>true</b> if <paramref name="y"/> contains relative coordinate value, otherwise <b>false</b>.</param>
        /// <returns><see cref="PointF"/> that contains absolute coordinates.</returns>
        private static PointF ToAbsolute(float x, float y, SvgPathSegmentList segments, bool isRelativeX, bool isRelativeY)
        {
            var point = new PointF(x, y);

            if ((isRelativeX || isRelativeY) && segments.Count > 0)
            {
                var lastSegment = segments.Last;

                // if the last element is a SvgClosePathSegment the position of the previous element should be used because the position of SvgClosePathSegment is 0,0
                if (lastSegment is SvgClosePathSegment) lastSegment = segments.Reverse().OfType<SvgMoveToSegment>().First();

                if (isRelativeX)
                {
                    point.X += lastSegment.End.X;
                }

                if (isRelativeY)
                {
                    point.Y += lastSegment.End.Y;
                }
            }

            return point;
        }

        private static IEnumerable<string> SplitCommands(string path)
        {
            var commandStart = 0;

            for (var i = 0; i < path.Length; i++)
            {
                string command;
                if (char.IsLetter(path[i]) && path[i] != 'e') //e is used in scientific notiation. but not svg path
                {
                    command = path.Substring(commandStart, i - commandStart).Trim();
                    commandStart = i;

                    if (!string.IsNullOrEmpty(command))
                    {
                        yield return command;
                    }

                    if (path.Length == i + 1)
                    {
                        yield return path[i].ToString();
                    }
                }
                else if (path.Length == i + 1)
                {
                    command = path.Substring(commandStart, i - commandStart + 1).Trim();

                    if (!string.IsNullOrEmpty(command))
                    {
                        yield return command;
                    }
                }
            }
        }

        

        //private static IEnumerable<float> ParseCoordinates(string coords)
        //{
        //    if (string.IsNullOrEmpty(coords) || coords.Length < 2) yield break;

        //    var pos = 0;
        //    var currState = NumState.separator;
        //    var newState = NumState.separator;

        //    for (int i = 1; i < coords.Length; i++)
        //    {
        //        switch (currState)
        //        {
        //            case NumState.separator:
        //                if (char.IsNumber(coords[i]))
        //                {
        //                    newState = NumState.integer;
        //                }
        //                else if (IsCoordSeparator(coords[i]))
        //                {
        //                    newState = NumState.separator;
        //                }
        //                else
        //                {
        //                    switch (coords[i])
        //                    {
        //                        case '.':
        //                            newState = NumState.decPlace;
        //                            break;
        //                        case '+':
        //                        case '-':
        //                            newState = NumState.prefix;
        //                            break;
        //                        default:
        //                            newState = NumState.invalid;
        //                            break;
        //                    }
        //                }
        //                break;
        //            case NumState.prefix:
        //                if (char.IsNumber(coords[i]))
        //                {
        //                    newState = NumState.integer;
        //                }
        //                else if (coords[i] == '.')
        //                {
        //                    newState = NumState.decPlace;
        //                }
        //                else
        //                {
        //                    newState = NumState.invalid;
        //                }
        //                break;
        //            case NumState.integer:
        //                if (char.IsNumber(coords[i]))
        //                {
        //                    newState = NumState.integer;
        //                }
        //                else if (IsCoordSeparator(coords[i]))
        //                {
        //                    newState = NumState.separator;
        //                }
        //                else
        //                {
        //                    switch (coords[i])
        //                    {
        //                        case '.':
        //                            newState = NumState.decPlace;
        //                            break;
        //                        case 'e':
        //                            newState = NumState.exponent;
        //                            break;
        //                        case '+':
        //                        case '-':
        //                            newState = NumState.prefix;
        //                            break;
        //                        default:
        //                            newState = NumState.invalid;
        //                            break;
        //                    }
        //                }
        //                break;
        //            case NumState.decPlace:
        //                if (char.IsNumber(coords[i]))
        //                {
        //                    newState = NumState.fraction;
        //                }
        //                else if (IsCoordSeparator(coords[i]))
        //                {
        //                    newState = NumState.separator;
        //                }
        //                else
        //                {
        //                    switch (coords[i])
        //                    {
        //                        case 'e':
        //                            newState = NumState.exponent;
        //                            break;
        //                        case '+':
        //                        case '-':
        //                            newState = NumState.prefix;
        //                            break;
        //                        default:
        //                            newState = NumState.invalid;
        //                            break;
        //                    }
        //                }
        //                break;
        //            case NumState.fraction:
        //                if (char.IsNumber(coords[i]))
        //                {
        //                    newState = NumState.fraction;
        //                }
        //                else if (IsCoordSeparator(coords[i]))
        //                {
        //                    newState = NumState.separator;
        //                }
        //                else
        //                {
        //                    switch (coords[i])
        //                    {
        //                        case '.':
        //                            newState = NumState.decPlace;
        //                            break;
        //                        case 'e':
        //                            newState = NumState.exponent;
        //                            break;
        //                        case '+':
        //                        case '-':
        //                            newState = NumState.prefix;
        //                            break;
        //                        default:
        //                            newState = NumState.invalid;
        //                            break;
        //                    }
        //                }
        //                break;
        //            case NumState.exponent:
        //                if (char.IsNumber(coords[i]))
        //                {
        //                    newState = NumState.expValue;
        //                }
        //                else if (IsCoordSeparator(coords[i]))
        //                {
        //                    newState = NumState.invalid;
        //                }
        //                else
        //                {
        //                    switch (coords[i])
        //                    {
        //                        case '+':
        //                        case '-':
        //                            newState = NumState.expPrefix;
        //                            break;
        //                        default:
        //                            newState = NumState.invalid;
        //                            break;
        //                    }
        //                }
        //                break;
        //            case NumState.expPrefix:
        //                if (char.IsNumber(coords[i]))
        //                {
        //                    newState = NumState.expValue;
        //                }
        //                else
        //                {
        //                    newState = NumState.invalid;
        //                }
        //                break;
        //            case NumState.expValue:
        //                if (char.IsNumber(coords[i]))
        //                {
        //                    newState = NumState.expValue;
        //                }
        //                else if (IsCoordSeparator(coords[i]))
        //                {
        //                    newState = NumState.separator;
        //                }
        //                else
        //                {
        //                    switch (coords[i])
        //                    {
        //                        case '.':
        //                            newState = NumState.decPlace;
        //                            break;
        //                        case '+':
        //                        case '-':
        //                            newState = NumState.prefix;
        //                            break;
        //                        default:
        //                            newState = NumState.invalid;
        //                            break;
        //                    }
        //                }
        //                break;
        //        }

        //        if (newState < currState)
        //        {
        //            yield return float.Parse(coords.Substring(pos, i - pos), NumberStyles.Float, CultureInfo.InvariantCulture);
        //            pos = i;
        //        }
        //        else if (newState != currState && currState == NumState.separator)
        //        {
        //            pos = i;
        //        }

        //        if (newState == NumState.invalid) yield break;
        //        currState = newState;
        //    }

        //    if (currState != NumState.separator)
        //    {
        //        yield return float.Parse(coords.Substring(pos, coords.Length - pos), NumberStyles.Float, CultureInfo.InvariantCulture);
        //    }
        //}

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string)
            {
                return Parse((string)value);
            }

            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                var paths = value as SvgPathSegmentList;

                if (paths != null)
                {
                    var curretCulture = CultureInfo.CurrentCulture;
                    try {
                        Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
                        var s = string.Join(" ", paths.Select(p => p.ToString()).ToArray());
                        return s;
                    }
                    finally
                    {
                        // Make sure to set back the old culture even an error occurred.
                        Thread.CurrentThread.CurrentCulture = curretCulture;
                    }
                }
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                return true;
            }

            return base.CanConvertTo(context, destinationType);
        }
    }
}