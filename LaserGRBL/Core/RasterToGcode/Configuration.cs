using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LaserGRBL.Core.RasterToGcode
{
	public class Configuration
	{
		public System.Drawing.Drawing2D.InterpolationMode InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
		public ColorToGrayscale ColorToGrayscale = new ColorToGrayscale();
		public ConversionTool SelectedTool = new LineToLine();
	}


	public class ColorToGrayscale
	{
		public LaserGRBL.RasterConverter.ImageTransform.Formula Formula = LaserGRBL.RasterConverter.ImageTransform.Formula.SimpleAverage;

		public byte Red = 100;
		public byte Green = 100;
		public byte Blue = 100;

		public byte Brightness = 100;
		public byte Contrast = 100;
		public byte WhitePoint = 5;

		public EnabledByte Threshold = new EnabledByte {Enabled = false, Value = 50};
	}



	public class LineToLine : ConversionTool
	{
 
	}

	public class Dithering : ConversionTool
	{
		public LaserGRBL.RasterConverter.ImageTransform.DitheringMode Mode = RasterConverter.ImageTransform.DitheringMode.FloydSteinberg;
	}

	public class Vectorization : ConversionTool
	{
		public EnabledDecimal SpotRemoval = new EnabledDecimal {Enabled = false, Value = 2.0m };
		public EnabledDecimal Optimize = new EnabledDecimal { Enabled = false, Value = 0.2m };
		public EnabledDecimal Smoothing = new EnabledDecimal { Enabled = false, Value = 1.0m };
		public EnabledDecimal DownSampling = new EnabledDecimal { Enabled = false, Value = 2.0m };
	}

	public abstract class ConversionTool 
	{
		public LaserGRBL.RasterConverter.ImageProcessor.Direction Direction = RasterConverter.ImageProcessor.Direction.Diagonal;
		public double Quality = 5.0;
		public bool LinePreview = true;
	}

	public struct EnabledDecimal
	{
		public bool Enabled;
		public Decimal Value;
	}

	public struct EnabledByte
	{
		public bool Enabled;
		public byte Value;
	}
	
}

