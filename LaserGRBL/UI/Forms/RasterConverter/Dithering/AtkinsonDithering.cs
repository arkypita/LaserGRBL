/* Even more algorithms for dithering images using C#
 * http://www.cyotek.com/blog/even-more-algorithms-for-dithering-images-using-csharp
 *
 * Copyright © 2015 Cyotek Ltd.
 *
 * Licensed under the MIT License. See LICENSE.txt for the full text.
 */

/*
 * Atkinson Dithering
 * http://www.tannerhelland.com/4660/dithering-eleven-algorithms-source-code/
 *
 *           *  1/8 1/8
 *      1/8 1/8 1/8
 *          1/8
 */

using System.ComponentModel;

namespace Cyotek.Drawing.Imaging.ColorReduction
{
  [Description("Atkinson")]
  public sealed class AtkinsonDithering : ErrorDiffusionDithering
  {
    #region Constructors

    public AtkinsonDithering()
      : base(new byte[,]
             {
               {
                 0, 0, 1, 1
               },
               {
                 1, 1, 1, 0
               },
               {
                 0, 1, 0, 0
               }
             }, 3, true)
    { }

    #endregion
  }
}
