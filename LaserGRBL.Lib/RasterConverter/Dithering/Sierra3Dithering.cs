/* Even more algorithms for dithering images using C#
 * http://www.cyotek.com/blog/even-more-algorithms-for-dithering-images-using-csharp
 *
 * Copyright © 2015 Cyotek Ltd.
 *
 * Licensed under the MIT License. See LICENSE.txt for the full text.
 */

/*
 * Sierra Dithering
 * http://www.efg2.com/Lab/Library/ImageProcessing/DHALF.TXT
 *
 *                  *  5/32 3/32
 *      2/32 4/32 5/32 4/32 2/32
 *           2/32 3/32 2/32
 */

using System.ComponentModel;

namespace Cyotek.Drawing.Imaging.ColorReduction
{
  [Description("Sierra")]
  public sealed class Sierra3Dithering : ErrorDiffusionDithering
  {
    #region Constructors

    public Sierra3Dithering()
      : base(new byte[,]
             {
               {
                 0, 0, 0, 5, 3
               },
               {
                 2, 4, 5, 4, 2
               },
               {
                 0, 2, 3, 2, 0
               }
             }, 5, true)
    { }

    #endregion
  }
}
