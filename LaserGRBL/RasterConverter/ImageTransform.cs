using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;


namespace LaserGRBL.RasterConverter
{
	public class ImageTransform
	{

		public static Bitmap ResizeImage(Image image, Size size, bool killalfa)
		{
			if (image.Size == size)
				return new Bitmap(image);

			Rectangle destRect = new Rectangle(0, 0, size.Width, size.Height);
			Bitmap destImage = new Bitmap(size.Width, size.Height);

			destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

			using (Graphics g = Graphics.FromImage(destImage))
			{
				if (killalfa)
					g.Clear(Color.White);
				
				
				if (killalfa)
					g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;
				else
					g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
				
				g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
				g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
				g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
				g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

				using (var wrapMode = new System.Drawing.Imaging.ImageAttributes())
				{
					wrapMode.SetWrapMode(System.Drawing.Drawing2D.WrapMode.TileFlipXY);
					g.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
				}
			}

			return destImage;
		}

//		public static Bitmap KillAlfa(Image image)
//		{
//			Bitmap destImage = new Bitmap(image.Width, image.Height);
//			destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);
//
//			using (Graphics g = Graphics.FromImage(destImage))
//			{
//				g.Clear(Color.White);
//				g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;
//				g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
//				g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
//				g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
//				g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
//				g.DrawImage(image, 0, 0);
//			}
//
//			return destImage;
//		}

//		public static Bitmap Negative(Image img)
//		{
//
//			ColorMatrix cm = new ColorMatrix(new float[][] {
//				new float[] {
//					-1,
//					0,
//					0,
//					0,
//					0
//				},
//				new float[] {
//					0,
//					-1,
//					0,
//					0,
//					0
//				},
//				new float[] {
//					0,
//					0,
//					-1,
//					0,
//					0
//				},
//				new float[] {
//					0,
//					0,
//					0,
//					1,
//					0
//				},
//				new float[] {
//					1,
//					1,
//					1,
//					0,
//					1
//				}
//			});
//
//			return draw_adjusted_image(img, cm);
//
//		}
//
//		public static Bitmap Brightness(Image img, float brightness)
//		{
//			ColorMatrix cm = new ColorMatrix(new float[][] {
//				new float[] {
//					1,
//					0,
//					0,
//					0,
//					0
//				},
//				new float[] {
//					0,
//					1,
//					0,
//					0,
//					0
//				},
//				new float[] {
//					0,
//					0,
//					1,
//					0,
//					0
//				},
//				new float[] {
//					0,
//					0,
//					0,
//					1,
//					0
//				},
//				new float[] {
//					brightness,
//					brightness,
//					brightness,
//					0,
//					1
//				}
//			});
//
//			return draw_adjusted_image(img, cm);
//
//		}
//
//		public static Bitmap Contrast(Image img, float contrast)
//		{
//			ColorMatrix cm = new ColorMatrix(new float[][] {
//				new float[] {
//					contrast,
//					0,
//					0,
//					0,
//					0
//				},
//				new float[] {
//					0,
//					contrast,
//					0,
//					0,
//					0
//				},
//				new float[] {
//					0,
//					0,
//					contrast,
//					0,
//					0
//				},
//				new float[] {
//					0,
//					0,
//					0,
//					1,
//					0
//				},
//				new float[] {
//					0,
//					0,
//					0,
//					0,
//					1
//				}
//			});
//
//			return draw_adjusted_image(img, cm);
//
//		}
//
//		public static Bitmap BrightnessContrast(Image img, float brightness, float contrast)
//		{
//			ColorMatrix cm = new ColorMatrix(new float[][] {
//				new float[] {
//					contrast,
//					0,
//					0,
//					0,
//					0
//				},
//				new float[] {
//					0,
//					contrast,
//					0,
//					0,
//					0
//				},
//				new float[] {
//					0,
//					0,
//					contrast,
//					0,
//					0
//				},
//				new float[] {
//					0,
//					0,
//					0,
//					1,
//					0
//				},
//				new float[] {
//					brightness,
//					brightness,
//					brightness,
//					0,
//					1
//				}
//			});
//
//			return draw_adjusted_image(img, cm);
//
//		}

		public static Bitmap Threshold(Image img, float threshold, bool apply)
		{
			Bitmap bmp = new Bitmap(img);

			using (Graphics g = Graphics.FromImage(bmp))
			{
				// Create an ImageAttributes object, and set its color threshold.
				ImageAttributes imageAttr = new ImageAttributes();
				imageAttr.SetThreshold(threshold);

				if (apply)
					g.DrawImage(img, new Rectangle(0,0, bmp.Width, bmp.Height), 0,0, bmp.Width, bmp.Height,	GraphicsUnit.Pixel, imageAttr);
				else
					g.DrawImage(img, 0,0);
			}
			return bmp;
		}

		private static Bitmap draw_adjusted_image(Image img, ColorMatrix cm)
		{

			try {
				Bitmap tmp = new Bitmap(img.Width, img.Height);
				// create a copy of the source image 
				using (Graphics g = Graphics.FromImage(tmp)) {
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
			} catch {
				return null;
			}

		}




		//**************************

		public enum Formula
		{
			SimpleAverage = 0,
			WeightAverage = 1
		}


		public static Bitmap GrayScale(Image img, float R, float G, float B, float brightness, float contrast, Formula formula)
		{
			ColorMatrix cm = default(ColorMatrix);

			// Apply selected grayscale formula
			
			if (formula == Formula.SimpleAverage)
			{
					cm = new ColorMatrix(new float[][] {
						new float[] {
							0.333F * R * contrast,
							0.333F * R * contrast,
							0.333F * R * contrast,
							0F,
							0F
						},
						new float[] {
							0.333F * G * contrast,
							0.333F * G * contrast,
							0.333F * G * contrast,
							0F,
							0F
						},
						new float[] {
							0.333F * B * contrast,
							0.333F * B * contrast,
							0.333F * B * contrast,
							0F,
							0F
						},
						new float[] {
							0F,
							0F,
							0F,
							1F,
							0F
						},
						new float[] {
							brightness,
							brightness,
							brightness,
							0F,
							1F
						}
					});
			}
			else if (formula == Formula.WeightAverage)
			{
					cm = new ColorMatrix(new float[][] {
						new float[] {
							0.333F * R * contrast,
							0.333F * R * contrast,
							0.333F * R * contrast,
							0F,
							0F
						},
						new float[] {
							0.444F * G * contrast,
							0.444F * G * contrast,
							0.444F * G * contrast,
							0F,
							0F
						},
						new float[] {
							0.222F * B * contrast,
							0.222F * B * contrast,
							0.222F * B * contrast,
							0F,
							0F
						},
						new float[] {
							0F,
							0F,
							0F,
							1F,
							0F
						},
						new float[] {
							brightness,
							brightness,
							brightness,
							0F,
							1F
						}
					});
			}	

			return draw_adjusted_image(img, cm);

		}
	}

}