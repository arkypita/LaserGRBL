using System;
using Cyotek.Drawing;
using Cyotek.Drawing.Imaging.ColorReduction;

namespace LaserGRBL.RasterConverter
{
	internal sealed class BlueNoiseDithering : IErrorDiffusion
	{
		private const int TileSize = 64;
		private const int TileMask = TileSize - 1;
		private const int PixelCount = TileSize * TileSize;
		private const int FilterRadius = 5;
		private static readonly Lazy<ushort[]> ThresholdRanks = new Lazy<ushort[]>(CreateRanks);
		private readonly ushort[] ranks = ThresholdRanks.Value;

		void IErrorDiffusion.Diffuse(ArgbColor[] data, ArgbColor original, ArgbColor transformed,
			int x, int y, int width, int height)
		{
			int luminance = (299 * original.R + 587 * original.G + 114 * original.B + 500) / 1000;
			int rank = ranks[(y & TileMask) * TileSize + (x & TileMask)];
			byte value = 2 * luminance * PixelCount > 255 * (2 * rank + 1) ? (byte)255 : (byte)0;
			data[y * width + x] = new ArgbColor(original.A, value, value, value);
		}

		// Ulichney's void-and-cluster algorithm (doi.org/10.1117/12.152707).
		private static ushort[] CreateRanks()
		{
			var weights = new int[2 * FilterRadius + 1, 2 * FilterRadius + 1];
			const double sigma = 1.5;
			for (int y = -FilterRadius; y <= FilterRadius; y++)
				for (int x = -FilterRadius; x <= FilterRadius; x++)
					weights[y + FilterRadius, x + FilterRadius] = (int)Math.Round(1000000 * Math.Exp(-(x * x + y * y) / (2 * sigma * sigma)));

			var prototype = new Pattern(weights);
			const int seedCount = PixelCount / 10;
			var candidates = new int[PixelCount];
			for (int i = 0; i < candidates.Length; i++)
				candidates[i] = i;
			var random = new Random(916119);
			for (int i = 0; i < seedCount; i++)
			{
				int next = random.Next(i, candidates.Length);
				prototype.Set(candidates[next], true);
				candidates[next] = candidates[i];
			}

			// Relax the seed pattern by moving points from dense clusters into sparse voids.
			for (int iteration = 0; iteration < PixelCount; iteration++)
			{
				int cluster = prototype.FindCluster();
				prototype.Set(cluster, false);
				int voidPixel = prototype.FindVoid();
				if (prototype.DensityAt(voidPixel) >= prototype.DensityAt(cluster))
				{
					prototype.Set(cluster, true);
					break;
				}
				prototype.Set(voidPixel, true);
			}

			var ranks = new ushort[PixelCount];
			var sparse = new Pattern(prototype);
			for (int rank = seedCount - 1; rank >= 0; rank--)
			{
				int cluster = sparse.FindCluster();
				ranks[cluster] = (ushort)rank;
				sparse.Set(cluster, false);
			}

			for (int rank = seedCount; rank < PixelCount / 2; rank++)
			{
				int voidPixel = prototype.FindVoid();
				ranks[voidPixel] = (ushort)rank;
				prototype.Set(voidPixel, true);
			}

			// Above half coverage, disperse the remaining dark points by optimizing their complement.
			var complement = new Pattern(weights);
			for (int i = 0; i < PixelCount; i++)
				if (!prototype.HasPoint(i))
					complement.Set(i, true);
			for (int rank = PixelCount / 2; rank < PixelCount; rank++)
			{
				int cluster = complement.FindCluster();
				ranks[cluster] = (ushort)rank;
				complement.Set(cluster, false);
			}
			return ranks;
		}

		private sealed class Pattern
		{
			private readonly int[,] weights;
			private readonly bool[] points;
			private readonly int[] densities;

			internal Pattern(int[,] weights)
			{
				this.weights = weights;
				points = new bool[PixelCount];
				densities = new int[PixelCount];
			}

			internal Pattern(Pattern source)
			{
				weights = source.weights;
				points = (bool[])source.points.Clone();
				densities = (int[])source.densities.Clone();
			}

			internal bool HasPoint(int index) { return points[index]; }
			internal int DensityAt(int index) { return densities[index]; }

			internal void Set(int index, bool occupied)
			{
				points[index] = occupied;
				int sign = occupied ? 1 : -1;
				int centerX = index % TileSize;
				int centerY = index / TileSize;
				for (int y = -FilterRadius; y <= FilterRadius; y++)
				{
					int row = ((centerY + y) & TileMask) * TileSize;
					for (int x = -FilterRadius; x <= FilterRadius; x++)
					{
						int column = (centerX + x) & TileMask;
						densities[row + column] += sign * weights[y + FilterRadius, x + FilterRadius];
					}
				}
			}

			internal int FindCluster()
			{
				int best = -1;
				int greatest = int.MinValue;
				for (int i = 0; i < PixelCount; i++)
				{
					if (points[i] && densities[i] > greatest)
					{
						best = i;
						greatest = densities[i];
					}
				}
				return best;
			}

			internal int FindVoid()
			{
				int best = -1;
				int least = int.MaxValue;
				for (int i = 0; i < PixelCount; i++)
				{
					if (!points[i] && densities[i] < least)
					{
						best = i;
						least = densities[i];
					}
				}
				return best;
			}
		}
	}
}
