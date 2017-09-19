/* Even more algorithms for dithering images using C#
 * http://www.cyotek.com/blog/even-more-algorithms-for-dithering-images-using-csharp
 *
 * Copyright © 2015 Cyotek Ltd.
 *
 * Licensed under the MIT License. See LICENSE.txt for the full text.
 */

using System;

namespace Cyotek.Drawing.Imaging.ColorReduction
{
  public abstract class ErrorDiffusionDithering : IErrorDiffusion
  {
    #region Constants

    private readonly byte _divisor;

    private readonly byte[,] _matrix;

    private readonly byte _matrixHeight;

    private readonly byte _matrixWidth;

    private readonly byte _startingOffset;

    private readonly bool _useShifting;

    #endregion

    #region Constructors

    protected ErrorDiffusionDithering(byte[,] matrix, byte divisor, bool useShifting)
    {
      if (matrix == null)
      {
        //throw new ArgumentNullException(nameof(matrix));
      }

      if (matrix.Length == 0)
      {
        //throw new ArgumentException("Matrix is empty.", nameof(matrix));
      }

      _matrix = matrix;
      _matrixWidth = (byte)(matrix.GetUpperBound(1) + 1);
      _matrixHeight = (byte)(matrix.GetUpperBound(0) + 1);
      _divisor = divisor;
      _useShifting = useShifting;

      for (int i = 0; i < _matrixWidth; i++)
      {
        if (matrix[0, i] != 0)
        {
          _startingOffset = (byte)(i - 1);
          break;
        }
      }
    }

    #endregion

    #region IErrorDiffusion Interface

    void IErrorDiffusion.Diffuse(ArgbColor[] data, ArgbColor original, ArgbColor transformed, int x, int y, int width, int height)
    {
      int redError;
      int blueError;
      int greenError;

      redError = original.R - transformed.R;
      blueError = original.G - transformed.G;
      greenError = original.B - transformed.B;

      for (int row = 0; row < _matrixHeight; row++)
      {
        int offsetY;

        offsetY = y + row;

        for (int col = 0; col < _matrixWidth; col++)
        {
          int coefficient;
          int offsetX;

          coefficient = _matrix[row, col];
          offsetX = x + (col - _startingOffset);

          if (coefficient != 0 && offsetX > 0 && offsetX < width && offsetY > 0 && offsetY < height)
          {
            ArgbColor offsetPixel;
            int offsetIndex;
            int newR;
            int newG;
            int newB;
            byte r;
            byte g;
            byte b;

            offsetIndex = offsetY * width + offsetX;
            offsetPixel = data[offsetIndex];

            // if the UseShifting property is set, then bit shift the values by the specified
            // divisor as this is faster than integer division. Otherwise, use integer division

            if (_useShifting)
            {
              newR = (redError * coefficient) >> _divisor;
              newG = (greenError * coefficient) >> _divisor;
              newB = (blueError * coefficient) >> _divisor;
            }
            else
            {
              newR = (redError * coefficient) / _divisor;
              newG = (greenError * coefficient) / _divisor;
              newB = (blueError * coefficient) / _divisor;
            }

			r = ToByte(offsetPixel.R + newR);
			g = ToByte(offsetPixel.G + newG);
			b = ToByte(offsetPixel.B + newB);

            data[offsetIndex] = ArgbColor.FromArgb(offsetPixel.A, r, g, b);
          }
        }
      }
    }

	internal static byte ToByte(int value)
	{
		if (value < 0)
		{
			value = 0;
		}
		else if (value > 255)
		{
			value = 255;
		}

		return (byte)value;
	}

    #endregion
  }
}
