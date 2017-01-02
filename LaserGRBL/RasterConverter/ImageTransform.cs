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

			bool scaleDown = (size.Width * size.Height) < (image.Size.Width * image.Size.Height);
			
			Rectangle destRect = new Rectangle(0, 0, size.Width, size.Height);
			Bitmap destImage = new Bitmap(size.Width, size.Height);

			destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

			using (Graphics g = Graphics.FromImage(destImage))
			{
				if (killalfa)
					g.Clear(Color.White);
				
				
				if (killalfa)
					g.CompositingMode = CompositingMode.SourceOver;
				else
					g.CompositingMode = CompositingMode.SourceCopy;
				
				g.CompositingQuality = CompositingQuality.HighQuality;
				g.SmoothingMode = SmoothingMode.HighQuality;
				
				if (scaleDown)
					g.InterpolationMode = InterpolationMode.HighQualityBicubic;
				else //scale UP
					g.InterpolationMode = InterpolationMode.NearestNeighbor;
				

				
				g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

				using (var wrapMode = new System.Drawing.Imaging.ImageAttributes())
				{
					wrapMode.SetWrapMode(System.Drawing.Drawing2D.WrapMode.TileFlipXY);
					g.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
				}
			}

			return destImage;
		}

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
			WeightAverage = 1,
			OpticalCorrect = 2
		}


		public static Bitmap GrayScale(Image img, float R, float G, float B, float brightness, float contrast, Formula formula)
		{
			ColorMatrix cm = default(ColorMatrix);

			// Apply selected grayscale formula
			
			float RedFactor = 0.333F; //Formula.SimpleAverage
			float GreenFactor = 0.333F; //Formula.SimpleAverage
			float BlueFactor = 0.333F; //Formula.SimpleAverage
			
			if (formula == Formula.WeightAverage)
			{
				RedFactor = 0.333F;
				GreenFactor = 0.444F;
				BlueFactor = 0.222F;
			}
			else if (formula == Formula.OpticalCorrect) // Reference: http://www.had2know.com/technology/rgb-to-gray-scale-converter.html
            {
				RedFactor = 0.299F;
				GreenFactor = 0.587F;
				BlueFactor = 0.114F;				
            }
			
			RedFactor =	 RedFactor * R * contrast;
			GreenFactor = GreenFactor * G * contrast;
			BlueFactor = BlueFactor * B * contrast;
			
			cm = new ColorMatrix(new float[][] {
				new float[] {RedFactor,RedFactor,RedFactor,0F,0F},
				new float[] {GreenFactor,GreenFactor,GreenFactor,0F,0F},
				new float[] {BlueFactor,BlueFactor,BlueFactor,0F,0F},
				new float[] {0F,0F,0F,1F,0F},
				new float[] {brightness,brightness,brightness,0F,1F}
			});
		
            
			return draw_adjusted_image(img, cm);

		}
	}

}