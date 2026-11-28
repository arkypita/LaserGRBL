using System;
using System.Collections.Generic;
using System.Globalization;

namespace LaserGRBL.DeviceProfiles
{
    internal static class JobPatterns
    {
        internal const string Ruler = "100 mm joonlaud";
        internal const string Matrix = "Võimsuse ja kiiruse testmaatriks";
        internal const string LineWidths = "Peenikesed ja jämedad jooned";
        internal const string Squares = "Kalibreerimisruudud";

        internal static readonly string[] Names = { Ruler, Matrix, LineWidths, Squares };

        private static readonly string[] DigitSegments =
        {
            "abcdef", "bc", "abged", "abgcd", "fgbc", "afgcd", "afgecd", "abc", "abcdefg", "abfgcd"
        };

        private static readonly decimal[,] SegmentCoordinates =
        {
            { 0, 3, 1.5m, 3 },
            { 1.5m, 3, 1.5m, 1.5m },
            { 1.5m, 1.5m, 1.5m, 0 },
            { 1.5m, 0, 0, 0 },
            { 0, 0, 0, 1.5m },
            { 0, 1.5m, 0, 3 },
            { 0, 1.5m, 1.5m, 1.5m }
        };

        internal static IEnumerable<string> Generate(string pattern, decimal width, decimal height, int sourceMax, decimal feed)
        {
            if (sourceMax <= 0)
                throw new ArgumentOutOfRangeException(nameof(sourceMax), "Testvõimsus peab olema positiivne.");
            if (feed <= 0)
                throw new ArgumentOutOfRangeException(nameof(feed), "Testkiirus peab olema positiivne.");

            var job = new PatternJob(sourceMax, feed);
            switch (pattern)
            {
                case Ruler:
                    RequireArea(width, height, 104, 12);
                    AddRuler(job);
                    break;
                case Matrix:
                    RequireArea(width, height, 44, 28);
                    AddMatrix(job, width);
                    break;
                case LineWidths:
                    RequireArea(width, height, 14, 18);
                    AddLineWidths(job, width);
                    break;
                case Squares:
                    RequireArea(width, height, 9, 9);
                    AddSquares(job, Math.Min(width, height));
                    break;
                default:
                    throw new ArgumentException("Vali testmuster.", nameof(pattern));
            }

            job.Commands.Add("M5");
            return job.Commands;
        }

        private static void AddRuler(PatternJob job)
        {
            const decimal startX = 2;
            const decimal baseline = 2;
            job.Line(startX, baseline, startX + 100, baseline);

            for (int millimeters = 0; millimeters <= 100; millimeters++)
            {
                decimal x = startX + millimeters;
                decimal length = 1;
                if (millimeters % 5 == 0)
                    length = 2;
                if (millimeters % 10 == 0)
                {
                    length = 3;
                    AddNumber(job, millimeters, Math.Min(x, 101), 7);
                }
                job.Line(x, baseline, x, baseline + length);
            }
        }

        private static void AddNumber(PatternJob job, int number, decimal centerX, decimal y)
        {
            string digits = number.ToString(CultureInfo.InvariantCulture);
            const decimal digitAdvance = 2;
            decimal x = centerX - ((digits.Length - 1) * digitAdvance + 1.5m) / 2;
            foreach (char digit in digits)
            {
                foreach (char element in DigitSegments[digit - '0'])
                {
                    int segment = element - 'a';
                    job.Line(x + SegmentCoordinates[segment, 0], y + SegmentCoordinates[segment, 1],
                        x + SegmentCoordinates[segment, 2], y + SegmentCoordinates[segment, 3]);
                }
                x += digitAdvance;
            }
        }

        private static void AddMatrix(PatternJob job, decimal width)
        {
            const decimal left = 2;
            const decimal bottom = 2;
            const decimal gap = 2;
            const decimal rowSpacing = 6;
            int powerSteps = Math.Min(job.Power, 5);
            decimal lineLength = Math.Min(12, (width - 2 * left - (powerSteps - 1) * gap) / powerSteps);

            for (int row = 1; row <= 5; row++)
            {
                decimal speed = job.Feed * row / 5;
                decimal y = bottom + (row - 1) * rowSpacing;
                for (int column = 1; column <= powerSteps; column++)
                {
                    decimal x = left + (column - 1) * (lineLength + gap);
                    int power = (int)Math.Round((decimal)job.Power * column / powerSteps, MidpointRounding.AwayFromZero);
                    job.Line(x, y, x + lineLength, y, power, speed);
                }
            }
        }

        private static void AddLineWidths(PatternJob job, decimal width)
        {
            decimal endX = Math.Min(32, width - 2);
            int[] linesPerBand = { 1, 3, 5, 9 };
            for (int band = 0; band < linesPerBand.Length; band++)
            {
                for (int line = 0; line < linesPerBand[band]; line++)
                {
                    decimal y = 2 + 4 * band + 0.25m * line;
                    job.Line(2, y, endX, y);
                }
            }
        }

        private static void AddSquares(PatternJob job, decimal available)
        {
            int[] sizes = { 5, 10, 20, 30 };
            decimal center = Math.Min(34, available) / 2;
            foreach (int size in sizes)
            {
                if (size + 4 > available)
                    break;

                decimal low = center - (decimal)size / 2;
                decimal high = center + (decimal)size / 2;
                job.Travel(low, low);
                job.Move(high, low, job.Power, job.Feed);
                job.Move(high, high, job.Power, job.Feed);
                job.Move(low, high, job.Power, job.Feed);
                job.Move(low, low, job.Power, job.Feed);
            }
        }

        private static void RequireArea(decimal width, decimal height, int requiredWidth, int requiredHeight)
        {
            if (width < requiredWidth || height < requiredHeight)
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                    "See test vajab vähemalt {0} × {1} mm kasutatavat tööala.", requiredWidth, requiredHeight));
        }

        private sealed class PatternJob
        {
            internal readonly List<string> Commands = new List<string> { "M4 S0" };
            internal readonly int Power;
            internal readonly decimal Feed;

            internal PatternJob(int power, decimal feed)
            {
                Power = power;
                Feed = feed;
            }

            internal void Line(decimal startX, decimal startY, decimal endX, decimal endY)
            {
                Line(startX, startY, endX, endY, Power, Feed);
            }

            internal void Line(decimal startX, decimal startY, decimal endX, decimal endY, int power, decimal feed)
            {
                Travel(startX, startY);
                Move(endX, endY, power, feed);
            }

            internal void Travel(decimal x, decimal y)
            {
                Commands.Add(string.Format(CultureInfo.InvariantCulture, "G0 X{0} Y{1} S0", x, y));
            }

            internal void Move(decimal x, decimal y, int power, decimal feed)
            {
                Commands.Add(string.Format(CultureInfo.InvariantCulture, "G1 X{0} Y{1} F{2} S{3}", x, y, feed, power));
            }
        }
    }
}
