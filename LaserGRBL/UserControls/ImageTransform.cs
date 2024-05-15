//Copyright (c) 2016-2021 Diego Settimi - https://github.com/arkypita/

// This program is free software; you can redistribute it and/or modify  it under the terms of the GPLv3 General Public License as published by  the Free Software Foundation; either version 3 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GPLv3  General Public License for more details.
// You should have received a copy of the GPLv3 General Public License  along with this program; if not, write to the Free Software  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307,  USA. using System;

using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;


namespace Base.Drawing
{
	public static class ImageTransform
	{


		public static Image Negative(Image img)
		{

			ColorMatrix cm = new ColorMatrix(new float[][] {
				new float[] {
					-1,
					0,
					0,
					0,
					0
				},
				new float[] {
					0,
					-1,
					0,
					0,
					0
				},
				new float[] {
					0,
					0,
					-1,
					0,
					0
				},
				new float[] {
					0,
					0,
					0,
					1,
					0
				},
				new float[] {
					1,
					1,
					1,
					0,
					1
				}
			});

			return draw_adjusted_image(img, cm);

		}


		public static Image Brightness(Image img, float Val)
		{

			ColorMatrix cm = new ColorMatrix(new float[][] {
				new float[] {
					1,
					0,
					0,
					0,
					0
				},
				new float[] {
					0,
					1,
					0,
					0,
					0
				},
				new float[] {
					0,
					0,
					1,
					0,
					0
				},
				new float[] {
					0,
					0,
					0,
					1,
					0
				},
				new float[] {
					Val,
					Val,
					Val,
					0,
					1
				}
			});

			return draw_adjusted_image(img, cm);

		}

		public static Image Coloration(Image img, Color DestColor, bool greyscalize, float Bright)
		{
			float r = 0;
			float g = 0;
			float b = 0;
			Image rv = (Image)img.Clone();

			if (greyscalize)
			{
				rv = GrayScale(rv, Formula.CCIRRec709);
			}

			// noramlize the color components to 1
			r = Convert.ToSingle(DestColor.R / 255f);
			g = Convert.ToSingle(DestColor.G / 255f);
			b = Convert.ToSingle(DestColor.B / 255f);


			// create the color matrix
			ColorMatrix cm = new ColorMatrix(new float[][] {
				new float[] {
					r,
					0,
					0,
					0,
					0
				},
				new float[] {
					0,
					g,
					0,
					0,
					0
				},
				new float[] {
					0,
					0,
					b,
					0,
					0
				},
				new float[] {
					0,
					0,
					0,
					1,
					0
				},
				new float[] {
					0,
					0,
					0,
					0,
					1
				}
			});

			// apply the matrix to the image
			rv = draw_adjusted_image(rv, cm);

			if (!(Bright == 0))
			{
				rv = Brightness(rv, Bright);
			}

			return rv;

		}


		public static Image Translate(Image img, Color Color, byte Alpha)
		{
			return Translate(img, Color.R, Color.G, Color.B, Alpha);
		}

		public static Image Translate(Image img, float red, float green, float blue, byte alpha)
		{

			float sr = 0;
			float sg = 0;
			float sb = 0;
			float sa = 0;

			// noramlize the color components to 1
			sr = red / 255;
			sg = green / 255;
			sb = blue / 255;
			sa = Convert.ToSingle(alpha / 255);

			// create the color matrix
			ColorMatrix cm = new ColorMatrix(new float[][] {
				new float[] {
					1,
					0,
					0,
					0,
					0
				},
				new float[] {
					0,
					1,
					0,
					0,
					0
				},
				new float[] {
					0,
					0,
					1,
					0,
					0
				},
				new float[] {
					0,
					0,
					0,
					1,
					0
				},
				new float[] {
					sr,
					sg,
					sb,
					sa,
					1
				}
			});

			// apply the matrix to the image
			return draw_adjusted_image(img, cm);

		}


        private static Image MakeWhite(Image img)
        {
            // create the color matrix
            ColorMatrix cm = new ColorMatrix(new float[][] {
                new float[] {
                    1,
                    0,
                    0,
                    0,
                    0
                },
                new float[] {
                    0,
                    1,
                    0,
                    0,
                    0
                },
                new float[] {
                    0,
                    0,
                    1,
                    0,
                    0
                },
                new float[] {
                    0,
                    0,
                    0,
                    1,
                    0
                },
                new float[] {
                    1,
                    1,
                    1,
                    0,
                    1
                }
            });

            // apply the matrix to the image
            return draw_adjusted_image(img, cm);

        }

        public static Image SetColor(Image img, Color color)
        {
            // noramlize the color components to 1
            float sr = color.R / 255f;
            float sg = color.G / 255f;
            float sb = color.B / 255f;

            // create the color matrix
            ColorMatrix cm = new ColorMatrix(new float[][] {
                new float[] {
                    sr,
                    0,
                    0,
                    0,
                    0
                },
                new float[] {
                    0,
                    sg,
                    0,
                    0,
                    0
                },
                new float[] {
                    0,
                    0,
                    sb,
                    0,
                    0
                },
                new float[] {
                    0,
                    0,
                    0,
                    1,
                    0
                },
                new float[] {
                    0,
                    0,
                    0,
                    0,
                    1
                }
            });

            // apply the matrix to the image
            return draw_adjusted_image(MakeWhite(img), cm);

        }

        public static Image ChangeAlpha(Image img, byte alpha)
		{

			// noramlize the color components to 1
			float sa = Convert.ToSingle(alpha / 255);

			// create the color matrix
			ColorMatrix cm = new ColorMatrix(new float[][] {
				new float[] {
					1,
					0,
					0,
					0,
					0
				},
				new float[] {
					0,
					1,
					0,
					0,
					0
				},
				new float[] {
					0,
					0,
					1,
					0,
					0
				},
				new float[] {
					0,
					0,
					0,
					sa,
					0
				},
				new float[] {
					0,
					0,
					0,
					0,
					1
				}
			});

			// apply the matrix to the image
			return draw_adjusted_image(img, cm);

		}


		private static Image draw_adjusted_image(Image img, ColorMatrix cm)
		{

			try
			{
				Bitmap tmp = new Bitmap(img.Width, img.Height);
				// create a copy of the source image 
				using (Graphics g = Graphics.FromImage(tmp))
				{
					g.Clear(Color.Transparent);

					ImageAttributes imgattr = new ImageAttributes();
					Rectangle rc = new Rectangle(0, 0, img.Width, img.Height);
					// associate the ColorMatrix object with an ImageAttributes object
					imgattr.SetColorMatrix(cm, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

					// draw the copy of the source image back over the original image, 
					//applying the ColorMatrix

					g.DrawImage(img, rc, 0, 0, img.Width, img.Height, GraphicsUnit.Pixel, imgattr);
				}
				return tmp;
			}
			catch
			{
				return null;
			}

		}




		//**************************

		public enum Formula
		{
			SimpleAverage = 0,
			// Least accurate
			WeightAverage = 1,
			NtscPal = 2,
			// CCIR Recommendation 601-1 (Used in Ntsc/Pal Standards)
			CCIRRec709 = 3
			// CCIR Recommendation 709
		}


		public static Image GrayScale(Image img, Formula formula)
		{
			ColorMatrix cm = default(ColorMatrix);

			// Apply selected grayscale formula
			switch (formula)
			{
				case Formula.CCIRRec709:
					cm = new ColorMatrix(new float[][] {
						new float[] {
							0.213F,
							0.213F,
							0.213F,
							0,
							0
						},
						new float[] {
							0.715F,
							0.715F,
							0.715F,
							0,
							0
						},
						new float[] {
							0.072F,
							0.072F,
							0.072F,
							0,
							0
						},
						new float[] {
							0,
							0,
							0,
							1,
							0
						},
						new float[] {
							0,
							0,
							0,
							0,
							1
						}
					});
					break;
				case Formula.NtscPal:
					cm = new ColorMatrix(new float[][] {
						new float[] {
							0.299F,
							0.299F,
							0.299F,
							0,
							0
						},
						new float[] {
							0.587F,
							0.587F,
							0.587F,
							0,
							0
						},
						new float[] {
							0.114F,
							0.114F,
							0.114F,
							0,
							0
						},
						new float[] {
							0,
							0,
							0,
							1,
							0
						},
						new float[] {
							0,
							0,
							0,
							0,
							1
						}
					});
					break;
				case Formula.SimpleAverage:
					cm = new ColorMatrix(new float[][] {
						new float[] {
							0.333F,
							0.333F,
							0.333F,
							0,
							0
						},
						new float[] {
							0.333F,
							0.333F,
							0.333F,
							0,
							0
						},
						new float[] {
							0.333F,
							0.333F,
							0.333F,
							0,
							0
						},
						new float[] {
							0,
							0,
							0,
							1,
							0
						},
						new float[] {
							0,
							0,
							0,
							0,
							1
						}
					});
					break;
				case Formula.WeightAverage:
					cm = new ColorMatrix(new float[][] {
						new float[] {
							0.333F,
							0.333F,
							0.333F,
							0,
							0
						},
						new float[] {
							0.444F,
							0.444F,
							0.444F,
							0,
							0
						},
						new float[] {
							0.222F,
							0.222F,
							0.222F,
							0,
							0
						},
						new float[] {
							0,
							0,
							0,
							1,
							0
						},
						new float[] {
							0,
							0,
							0,
							0,
							1
						}
					});
					break;
				default:
					// Use CCIR Rec. 709 as catch all
					cm = new ColorMatrix(new float[][] {
						new float[] {
							0.213F,
							0.213F,
							0.213F,
							0,
							0
						},
						new float[] {
							0.715F,
							0.715F,
							0.715F,
							0,
							0
						},
						new float[] {
							0.072F,
							0.072F,
							0.072F,
							0,
							0
						},
						new float[] {
							0,
							0,
							0,
							1,
							0
						},
						new float[] {
							0,
							0,
							0,
							0,
							1
						}
					});
					break;
			}

			return draw_adjusted_image(img, cm);

		}

    }

}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
