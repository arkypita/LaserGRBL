using System;
using System.IO;
using System.Drawing.Drawing2D;
using System.Xml.Serialization;


namespace LaserGRBL.Core.RasterToGcode
{
	public class Configuration
	{
		public ColorToGrayscale ColorToGrayscale = new ColorToGrayscale();
		public ConversionTool SelectedTool = new LineToLine();
		public LaserSetting LaserSetting = new LaserSetting();
		public OutputConfiguration OutputConfiguration = new OutputConfiguration();

		public void Save(string path)
		{
			XmlSerializer serializer = new XmlSerializer(typeof(Configuration));
			using (FileStream stream = new FileStream(path, FileMode.Create))
				serializer.Serialize(stream, this);
		}

		public static Configuration Load(string path)
		{
			XmlSerializer serializer = new XmlSerializer(typeof(Configuration));
			using (FileStream stream = new FileStream(path, FileMode.Open))
				return serializer.Deserialize(stream) as Configuration;
		}

		public object Clone() { return MemberwiseClone(); }
	}

	public class OutputConfiguration
	{
		public System.Drawing.Size Size = new System.Drawing.Size(100, 100);
		public System.Drawing.Point Offset = new System.Drawing.Point(0, 0);
		public bool CenterToOffset = false;

		public object Clone() { return MemberwiseClone(); }
	}

	public class ColorToGrayscale : ICloneable
	{
		public InterpolationMode InterpolationMode = InterpolationMode.HighQualityBicubic;
		public ImageTransform.Formula Formula = ImageTransform.Formula.SimpleAverage;

		public byte Red = 100;
		public byte Green = 100;
		public byte Blue = 100;

		public byte Brightness = 100;
		public byte Contrast = 100;
		public byte WhitePoint = 5;

		public EnabledByte Threshold = new EnabledByte(false, 50);

		public object Clone() { return MemberwiseClone(); }
	}


	public class LineToLine : ConversionTool, ICloneable
	{
		public object Clone() { return MemberwiseClone(); }
	}


	public class Dithering : ConversionTool, ICloneable
	{
		public ImageTransform.DitheringMode Mode = ImageTransform.DitheringMode.FloydSteinberg;

		public object Clone() { return MemberwiseClone(); }
	}

	public class Vectorization : ConversionTool, ICloneable
	{
		public EnabledDecimal SpotRemoval = new EnabledDecimal(false, 2.0m);
		public EnabledDecimal Optimize = new EnabledDecimal (false, 0.2m);
		public EnabledDecimal Smoothing = new EnabledDecimal (false, 1.0m);
		public EnabledDecimal DownSampling = new EnabledDecimal(false, 2.0m);

		public object Clone() {return MemberwiseClone();}
	}

	[XmlInclude(typeof(Vectorization))]
	[XmlInclude(typeof(Dithering))]
	[XmlInclude(typeof(LineToLine))]
	public abstract class ConversionTool : ICloneable
	{
		public enum AvailableTools
		{ Line2Line, Dithering, Vectorize }

		public enum EngravingDirection
		{ Horizontal, Vertical, Diagonal, None }

		public EngravingDirection Direction = EngravingDirection.Diagonal;
		public bool OneWayEngraving = false;
		public double Quality = 5.0;

		public object Clone() { return MemberwiseClone(); }
	}

	public class LaserSetting
	{
		public enum ModulationType {Power, Speed};
		public ModulationType WhatModulate = ModulationType.Power;

		public int JumpSpeed = 2000;
		public int MarkSpeed = 1000;
		public int TraceSpeed = 1000;

		public ModulationRange SpeedRange = new ModulationRange(1000, 4000);
		public ModulationRange PowerRange = new ModulationRange(0, 255);
	}

	public struct EnabledDecimal
	{
		public bool Enabled;
		public Decimal Value;

		public EnabledDecimal(bool enabled, decimal value)
		{
			Enabled = enabled;
			Value = value;
		}
	}

	public struct EnabledByte
	{
		public bool Enabled;
		public byte Value;

		public EnabledByte(bool enabled, byte value)
		{
			Enabled = enabled;
			Value = value;
		}
	}

	public struct ModulationRange
	{
		public int Min;
		public int Max;

		public ModulationRange(int min, int max)
		{
			Min = min;
			Max = max;
		}
	}
	
}

