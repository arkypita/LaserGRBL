/* Even more algorithms for dithering images using C#
 * http://www.cyotek.com/blog/even-more-algorithms-for-dithering-images-using-csharp
 *
 * Copyright © 2015 Cyotek Ltd.
 *
 * Licensed under the MIT License. See LICENSE.txt for the full text.
 */

using System;
using System.ComponentModel;

namespace Cyotek.Drawing.Imaging.ColorReduction
{
  [Description("Random")]
  public sealed class RandomDithering : IErrorDiffusion
  {
    #region Constants

    private readonly ArgbColor _black;

    private static readonly ArgbColor _blackDefault = new ArgbColor(255, 0, 0, 0);

    private readonly Random _random;

    private readonly ArgbColor _white;

    private static readonly ArgbColor _whiteDefault = new ArgbColor(255, 255, 255, 255);

    #endregion

    #region Constructors

    public RandomDithering()
      : this(_whiteDefault, _blackDefault)
    { }

    public RandomDithering(ArgbColor white, ArgbColor black)
      : this(Environment.TickCount, white, black)
    { }

    public RandomDithering(int seed, ArgbColor white, ArgbColor black)
    {
      _random = new Random(seed);
      _white = white;
      _black = black;
    }

    public RandomDithering(int seed)
      : this(seed, _whiteDefault, _blackDefault)
    { }

    #endregion

    #region IErrorDiffusion Interface

    void IErrorDiffusion.Diffuse(ArgbColor[] data, ArgbColor original, ArgbColor transformed, int x, int y, int width, int height)
    {
      byte gray;

      gray = (byte)(0.299 * original.R + 0.587 * original.G + 0.114 * original.B);

      if (gray > _random.Next(0, 255))
      {
        data[y * width + x] = _white;
      }
      else
      {
        data[y * width + x] = _black;
      }
    }

    #endregion
  }
}
