//Copyright (c) 2016-2021 Diego Settimi - https://github.com/arkypita/

// This program is free software; you can redistribute it and/or modify  it under the terms of the GPLv3 General Public License as published by  the Free Software Foundation; either version 3 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GPLv3  General Public License for more details.
// You should have received a copy of the GPLv3 General Public License  along with this program; if not, write to the Free Software  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307,  USA. using System;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.IO;


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


				g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

				using (System.Drawing.Imaging.ImageAttributes wrapMode = new System.Drawing.Imaging.ImageAttributes())
				{
					wrapMode.SetWrapMode(System.Drawing.Drawing2D.WrapMode.TileFlipXY);
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

			return gray < 128 ? new Cyotek.Drawing.ArgbColor(pixel.A, 0, 0, 0) : new Cyotek.Drawing.ArgbColor(pixel.A, 255, 255, 255);
		}


		public static Bitmap GrayScale(Image img, float R, float G, float B, float brightness, float contrast, Formula formula)
		{
			ColorMatrix cm = default(ColorMatrix);

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

			RedFactor = RedFactor * contrast;
			GreenFactor = GreenFactor * contrast;
			BlueFactor = BlueFactor * contrast;

			cm = new ColorMatrix(new float[][] {
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

			byte sourceRed = 0, sourceGreen = 0, sourceBlue = 0, sourceAlpha = 0;
			int resultRed = 0, resultGreen = 0, resultBlue = 0;

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
				sourceAlpha = resultBuffer[k + 3];

				if (sourceAlpha != 0)
				{
					sourceBlue = resultBuffer[k];
					sourceGreen = resultBuffer[k + 1];
					sourceRed = resultBuffer[k + 2];

					if ((sourceBlue < blueFilter + filterData.ThresholdValue &&
							sourceBlue > blueFilter - filterData.ThresholdValue) &&

						(sourceGreen < greenFilter + filterData.ThresholdValue &&
							sourceGreen > greenFilter - filterData.ThresholdValue) &&

						(sourceRed < redFilter + filterData.ThresholdValue &&
							sourceRed > redFilter - filterData.ThresholdValue))
					{
						resultBlue = blueFilter - sourceBlue + newBlueValue;

						if (resultBlue > maxValue)
						{ resultBlue = maxValue; }
						else if (resultBlue < minValue)
						{ resultBlue = minValue; }

						resultGreen = greenFilter - sourceGreen + newGreenValue;

						if (resultGreen > maxValue)
						{ resultGreen = maxValue; }
						else if (resultGreen < minValue)
						{ resultGreen = minValue; }

						resultRed = redFilter - sourceRed + newRedValue;

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
				graphicsObject.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
				graphicsObject.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
				graphicsObject.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
				graphicsObject.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

				graphicsObject.DrawImage(sourceBitmap, new Rectangle(0, 0, sourceBitmap.Width, sourceBitmap.Height), new Rectangle(0, 0, sourceBitmap.Width, sourceBitmap.Height), GraphicsUnit.Pixel);
			}

			return copyBitmap;
		}

		public class DirectBitmap : IDisposable
		{
			public Bitmap Bitmap { get; private set; }
			public Int32[] Bits { get; private set; }
			public bool Disposed { get; private set; }
			public int Height { get; private set; }
			public int Width { get; private set; }

			protected GCHandle BitsHandle { get; private set; }

			public DirectBitmap(int width, int height)
			{
				Width = width;
				Height = height;
				Bits = new Int32[width * height];
				BitsHandle = GCHandle.Alloc(Bits, GCHandleType.Pinned);
				Bitmap = new Bitmap(width, height, width * 4, PixelFormat.Format32bppPArgb, BitsHandle.AddrOfPinnedObject());
			}

			public void SetPixel(int x, int y, Color colour)
			{
				int index = x + (y * Width);
				int col = colour.ToArgb();

				Bits[index] = col;
			}

			public Color GetPixel(int x, int y)
			{
				int index = x + (y * Width);
				int col = Bits[index];
				Color result = Color.FromArgb(col);

				return result;
			}

			public void Dispose()
			{
				if (Disposed) return;
				Disposed = true;
				Bitmap.Dispose();
				BitsHandle.Free();
			}
		}

		internal static Bitmap Fill(Image img, Point location, Color color, int v)
		{
			//create a blank bitmap the same size as original
			DirectBitmap newBitmap = new DirectBitmap(img.Width, img.Height);

			{
				//get a graphics object from the new image
				Graphics g = Graphics.FromImage(newBitmap.Bitmap);

				g.DrawImage(img, new Rectangle(0, 0, img.Width, img.Height), 0, 0, img.Width, img.Height, GraphicsUnit.Pixel);

				Fill4(newBitmap, location, color, v);

				//dispose the Graphics object
				g.Dispose();
			}

			System.GC.Collect();

			return newBitmap.Bitmap;
		}

		public static bool[][] Fill4(DirectBitmap bmp, Point pt, Color c1, int v)
		{
			bool[][] tchd = new bool[bmp.Width][];
			for (int i = 0; i < bmp.Width; i++) tchd[i] = new bool[bmp.Height];

			Color cx = bmp.GetPixel(pt.X, pt.Y);
			Color c0 = cx;
			Rectangle bmpRect = new Rectangle(Point.Empty, bmp.Bitmap.Size);
			Stack<Point> stack = new Stack<Point>();
			int x0 = pt.X;
			int y0 = pt.Y;

			stack.Push(new Point(x0, y0));
			while (stack.Count > 0)
			{
				Point p = stack.Pop();
				if (!bmpRect.Contains(p)) continue;
				tchd[p.X][p.Y] = true;
				cx = bmp.GetPixel(p.X, p.Y);
				if (FillColorCompare(c0, cx, v))
				{
					bmp.SetPixel(p.X, p.Y, c1);
					Point _p;
					_p = new Point(p.X, p.Y + 1); if (FillChkIn(tchd, _p)) stack.Push(_p);
					_p = new Point(p.X, p.Y - 1); if (FillChkIn(tchd, _p)) stack.Push(_p);
					_p = new Point(p.X + 1, p.Y); if (FillChkIn(tchd, _p)) stack.Push(_p);
					_p = new Point(p.X - 1, p.Y); if (FillChkIn(tchd, _p)) stack.Push(_p);
				}
			}
			return tchd;
		}

		private static bool FillColorCompare(Color c0, Color cx, int v)
		{
			return
				   c0.R > cx.R - v && c0.R < cx.R + v
				&& c0.G > cx.G - v && c0.G < cx.G + v
				&& c0.B > cx.B - v && c0.B < cx.B + v
				;
		}
		private static bool FillChkIn(bool[][] tchd, Point p)
        {
			if (p.X < 0 || p.Y < 0) return false;
			if (p.X >= tchd.Length) return false;
			if (p.Y >= tchd[p.X].Length) return false;
			return !tchd[p.X][p.Y];
		}

		internal static Bitmap Outliner(Image img, Point location)
		{
			//create a blank bitmap the same size as original
			DirectBitmap newBitmap = new DirectBitmap(img.Width, img.Height);

			{
				//get a graphics object from the new image
				Graphics g = Graphics.FromImage(newBitmap.Bitmap);

				g.DrawImage(img, new Rectangle(0, 0, img.Width, img.Height), 0, 0, img.Width, img.Height, GraphicsUnit.Pixel);

				Outline(newBitmap, location);

				//dispose the Graphics object
				g.Dispose();
			}

			System.GC.Collect();

			return newBitmap.Bitmap;
		}

		public static void Outline(DirectBitmap bmp, Point pt)
		{
			int v = 127;
			Color cx = bmp.GetPixel(pt.X, pt.Y);
            bool[][] tchd = Fill4(bmp, pt, Color.White, v);
            Color c0 = cx;
			for (int y = 0; y < bmp.Height; y++)
			{
				for (int x = 0; x < bmp.Width; x++)
				{
					Point p = new Point(x, y);
					if (!tchd[p.X][p.Y])
                    {
						bmp.SetPixel(p.X, p.Y, Color.Black);
					}
				}
			}
		}

		private class ColorSubstitutionFilter
		{
			private int thresholdValue = 10;
			public int ThresholdValue
			{
				get { return thresholdValue; }
				set { thresholdValue = value; }
			}

			private Color sourceColor = Color.White;
			public Color SourceColor
			{
				get { return sourceColor; }
				set { sourceColor = value; }
			}

			private Color newColor = Color.White;
			public Color NewColor
			{
				get { return newColor; }
				set { newColor = value; }
			}
		}



	}

}