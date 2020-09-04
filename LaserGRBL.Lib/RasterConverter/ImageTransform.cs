﻿//Copyright (c) 2016-2020 Diego Settimi - https://github.com/arkypita/

// This program is free software; you can redistribute it and/or modify  it under the terms of the GPLv3 General Public License as published by  the Free Software Foundation; either version 3 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GPLv3  General Public License for more details.
// You should have received a copy of the GPLv3 General Public License  along with this program; if not, write to the Free Software  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307,  USA. using System;

using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;

namespace LaserGRBL.RasterConverter
{
	public class ImageTransform
	{
		public static Bitmap ResizeImage(Image image, Size size, bool killalfa, InterpolationMode interpolation)
		{
			if (image.Size == size)
				return new Bitmap((Image)image.Clone());

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
				g.InterpolationMode = interpolation;


				g.PixelOffsetMode = PixelOffsetMode.HighQuality;

				using (ImageAttributes wrapMode = new System.Drawing.Imaging.ImageAttributes())
				{
					wrapMode.SetWrapMode(WrapMode.TileFlipXY);
					g.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
				}
			}

			return destImage;
		}

		public static Bitmap Threshold(Image img, float threshold, bool apply)
		{
			Bitmap bmp = new Bitmap(img.Width, img.Height);

			using (Graphics g = Graphics.FromImage(bmp))
			{
				g.Clear(Color.White); //Threshold is the final transformation 
				g.DrawImage(img, 0, 0); //so clear any transparent color and apply threshold

				// Create an ImageAttributes object, and set its color threshold.
				ImageAttributes imageAttr = new ImageAttributes();
				imageAttr.SetThreshold(threshold);

				if (apply)
					g.DrawImage(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height), 0, 0, bmp.Width, bmp.Height, GraphicsUnit.Pixel, imageAttr);
			}
			return bmp;
		}

		private static Bitmap draw_adjusted_image(Image img, ColorMatrix cm)
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

		public static Bitmap InvertingImage(Image img)
		{
			//create a blank bitmap the same size as original
			Bitmap newBitmap = new Bitmap(img.Width, img.Height);

			//get a graphics object from the new image
			Graphics g = Graphics.FromImage(newBitmap);

			// create the negative color matrix
			ColorMatrix colorMatrix = new ColorMatrix(new float[][]
			{
				new float[] {-1, 0, 0, 0, 0},
				new float[] {0, -1, 0, 0, 0},
				new float[] {0, 0, -1, 0, 0},
				new float[] {0, 0, 0, 1, 0},
				new float[] {1, 1, 1, 0, 1}
			});

			// create some image attributes
			ImageAttributes attributes = new ImageAttributes();

			attributes.SetColorMatrix(colorMatrix);

			g.DrawImage(img, new Rectangle(0, 0, img.Width, img.Height), 0, 0, img.Width, img.Height, GraphicsUnit.Pixel, attributes);

			//dispose the Graphics object
			g.Dispose();

			return newBitmap;
		}

		//**************************

		public enum Formula
		{
			SimpleAverage = 0,
			WeightAverage = 1,
			OpticalCorrect = 2,
			Custom = 3
		}

		public enum DitheringMode
		{
			Atkinson,
			FloydSteinberg,
			Burks,
			Jarvis,
			Random,
			Sierra2,
			Sierra3,
			SierraLight,
			Stucki
		}

		private static Cyotek.Drawing.Imaging.ColorReduction.IErrorDiffusion GetDitheringMode(DitheringMode mode)
		{
			if (mode == DitheringMode.FloydSteinberg)
				return new Cyotek.Drawing.Imaging.ColorReduction.FloydSteinbergDithering();
			else if (mode == DitheringMode.Atkinson)
				return new Cyotek.Drawing.Imaging.ColorReduction.AtkinsonDithering();
			else if (mode == DitheringMode.Burks)
				return new Cyotek.Drawing.Imaging.ColorReduction.BurksDithering();
			else if (mode == DitheringMode.Jarvis)
				return new Cyotek.Drawing.Imaging.ColorReduction.JarvisJudiceNinkeDithering();
			else if (mode == DitheringMode.Random)
				return new Cyotek.Drawing.Imaging.ColorReduction.RandomDithering();
			else if (mode == DitheringMode.Sierra2)
				return new Cyotek.Drawing.Imaging.ColorReduction.Sierra2Dithering();
			else if (mode == DitheringMode.Sierra3)
				return new Cyotek.Drawing.Imaging.ColorReduction.Sierra3Dithering();
			else if (mode == DitheringMode.SierraLight)
				return new Cyotek.Drawing.Imaging.ColorReduction.SierraLiteDithering();
			else if (mode == DitheringMode.Stucki)
				return new Cyotek.Drawing.Imaging.ColorReduction.StuckiDithering();
			else
				return new Cyotek.Drawing.Imaging.ColorReduction.FloydSteinbergDithering();
		}

		public static Bitmap DitherImage(Bitmap img, DitheringMode dithering)
		{
			Bitmap image;
			Cyotek.Drawing.ArgbColor[] originalData;
			Size size;
			Cyotek.Drawing.Imaging.ColorReduction.IErrorDiffusion dither;

			image = img;
			size = image.Size;

			originalData = Cyotek.DitheringTest.Helpers.ImageUtilities.GetPixelsFrom32BitArgbImage(image);

			dither = GetDitheringMode(dithering);// new Cyotek.Drawing.Imaging.ColorReduction.FloydSteinbergDithering();

			for (int row = 0; row < size.Height; row++)
			{
				for (int col = 0; col < size.Width; col++)
				{
					int index;
					Cyotek.Drawing.ArgbColor current;
					Cyotek.Drawing.ArgbColor transformed;

					index = row * size.Width + col;

					current = originalData[index];

					// transform the pixel - normally this would be some form of color
					// reduction. For this sample it's simple threshold based
					// monochrome conversion
					transformed = TransformPixel(current);
					originalData[index] = transformed;

					// apply a dither algorithm to this pixel
					if (dither != null)
					{
						dither.Diffuse(originalData, current, transformed, col, row, size.Width, size.Height);
					}
				}
			}

			return Cyotek.DitheringTest.Helpers.ImageUtilities.ToBitmap(originalData, size);
		}

		private static Cyotek.Drawing.ArgbColor TransformPixel(Cyotek.Drawing.ArgbColor pixel)
		{
			byte gray = (byte)(0.299 * pixel.R + 0.587 * pixel.G + 0.114 * pixel.B);

			/*
			 * I'm leaving the alpha channel untouched instead of making it fully opaque
			 * otherwise the transparent areas become fully black, and I was getting annoyed
			 * by this when testing images with large swathes of transparency!
			 */

			if (gray < 128)
				return new Cyotek.Drawing.ArgbColor(pixel.A, 0, 0, 0);
			else
				return new Cyotek.Drawing.ArgbColor(pixel.A, 255, 255, 255);
		}


		public static Bitmap GrayScale(Image img, float R, float G, float B, float brightness, float contrast, Formula formula)
		{

			// Apply selected grayscale formula

			float RedFactor = 0;
			float GreenFactor = 0;
			float BlueFactor = 0;

			if (formula == Formula.SimpleAverage)
			{
				RedFactor = 0.333F;
				GreenFactor = 0.333F;
				BlueFactor = 0.333F;
			}
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
			else if (formula == Formula.Custom)
			{
				RedFactor = 0.333F * R;
				GreenFactor = 0.333F * G;
				BlueFactor = 0.333F * B;
			}

			RedFactor *= contrast;
			GreenFactor *= contrast;
			BlueFactor *= contrast;

			ColorMatrix cm = new ColorMatrix(new float[][] {
				new float[] {RedFactor,RedFactor,RedFactor,0F,0F},
				new float[] {GreenFactor,GreenFactor,GreenFactor,0F,0F},
				new float[] {BlueFactor,BlueFactor,BlueFactor,0F,0F},
				new float[] {0F,0F,0F,1F,0F},
				new float[] {brightness,brightness,brightness,0F,1F}
			});

			return draw_adjusted_image(img, cm);
		}


		public static Bitmap Whitenize(Bitmap src, int threshold, bool demo)
		{
			ColorSubstitutionFilter f = new ColorSubstitutionFilter();
			f.ThresholdValue = threshold;
			f.SourceColor = Color.White;

			f.NewColor = demo ? Color.LightPink : Color.Transparent;
			return ColorSubstitution(src, f);
		}

		private static Bitmap ColorSubstitution(Bitmap sourceBitmap, ColorSubstitutionFilter filterData)
		{
			Bitmap resultBitmap = new Bitmap(sourceBitmap.Width, sourceBitmap.Height, PixelFormat.Format32bppArgb);

			BitmapData sourceData = sourceBitmap.LockBits(new Rectangle(0, 0, sourceBitmap.Width, sourceBitmap.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
			BitmapData resultData = resultBitmap.LockBits(new Rectangle(0, 0, resultBitmap.Width, resultBitmap.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

			byte[] resultBuffer = new byte[resultData.Stride * resultData.Height];
			Marshal.Copy(sourceData.Scan0, resultBuffer, 0, resultBuffer.Length);

			sourceBitmap.UnlockBits(sourceData);
			byte newRedValue = filterData.NewColor.R;
			byte newGreenValue = filterData.NewColor.G;
			byte newBlueValue = filterData.NewColor.B;
			byte newAlphaValue = filterData.NewColor.A;

			byte redFilter = filterData.SourceColor.R;
			byte greenFilter = filterData.SourceColor.G;
			byte blueFilter = filterData.SourceColor.B;

			byte minValue = 0;
			byte maxValue = 255;

			for (int k = 0; k < resultBuffer.Length; k += 4)
			{
				byte sourceAlpha = resultBuffer[k + 3];

				if (sourceAlpha != 0)
				{
					byte sourceBlue = resultBuffer[k];
					byte sourceGreen = resultBuffer[k + 1];
					byte sourceRed = resultBuffer[k + 2];

					if ((sourceBlue < blueFilter + filterData.ThresholdValue &&
							sourceBlue > blueFilter - filterData.ThresholdValue) &&

						(sourceGreen < greenFilter + filterData.ThresholdValue &&
							sourceGreen > greenFilter - filterData.ThresholdValue) &&

						(sourceRed < redFilter + filterData.ThresholdValue &&
							sourceRed > redFilter - filterData.ThresholdValue))
					{
						int resultBlue = blueFilter - sourceBlue + newBlueValue;

						if (resultBlue > maxValue)
						{ resultBlue = maxValue; }
						else if (resultBlue < minValue)
						{ resultBlue = minValue; }

						int resultGreen = greenFilter - sourceGreen + newGreenValue;

						if (resultGreen > maxValue)
						{ resultGreen = maxValue; }
						else if (resultGreen < minValue)
						{ resultGreen = minValue; }

						int resultRed = redFilter - sourceRed + newRedValue;

						if (resultRed > maxValue)
						{ resultRed = maxValue; }
						else if (resultRed < minValue)
						{ resultRed = minValue; }

						resultBuffer[k] = (byte)resultBlue;
						resultBuffer[k + 1] = (byte)resultGreen;
						resultBuffer[k + 2] = (byte)resultRed;
						resultBuffer[k + 3] = newAlphaValue;
					}
				}
			}

			Marshal.Copy(resultBuffer, 0, resultData.Scan0, resultBuffer.Length);
			resultBitmap.UnlockBits(resultData);

			return resultBitmap;
		}

		private static Bitmap Format32bppArgbCopy(Bitmap sourceBitmap)
		{
			Bitmap copyBitmap = new Bitmap(sourceBitmap.Width, sourceBitmap.Height, PixelFormat.Format32bppArgb);

			using (Graphics graphicsObject = Graphics.FromImage(copyBitmap))
			{
				graphicsObject.CompositingQuality = CompositingQuality.HighQuality;
				graphicsObject.InterpolationMode = InterpolationMode.HighQualityBicubic;
				graphicsObject.PixelOffsetMode = PixelOffsetMode.HighQuality;
				graphicsObject.SmoothingMode = SmoothingMode.HighQuality;

				graphicsObject.DrawImage(sourceBitmap, new Rectangle(0, 0, sourceBitmap.Width, sourceBitmap.Height), new Rectangle(0, 0, sourceBitmap.Width, sourceBitmap.Height), GraphicsUnit.Pixel);
			}

			return copyBitmap;
		}

		private class ColorSubstitutionFilter
		{
			public int ThresholdValue { get; set; } = 10;
			public Color SourceColor { get; set; } = Color.White;
			public Color NewColor { get; set; } = Color.White;
		}
	}
}
