using Svg.DataTypes;
using System.ComponentModel;

namespace Svg
{
	/// <summary>
	/// Description of SvgAspectRatio.
	/// </summary>
	[TypeConverter(typeof(SvgPreserveAspectRatioConverter))]
	public class SvgAspectRatio
	{
		public SvgAspectRatio() : this(SvgPreserveAspectRatio.none)
		{
		}
		
		public SvgAspectRatio(SvgPreserveAspectRatio align)
			: this(align, false)
		{
		}
		
		public SvgAspectRatio(SvgPreserveAspectRatio align, bool slice)
		{
			this.Align = align;
			this.Slice = slice;
			this.Defer = false;
		}
		
		public SvgPreserveAspectRatio Align
		{
			get;
			set;
		}
		
		public bool Slice
		{
			get;
			set;
		}

		public bool Defer
		{
			get;
			set;
		}

		public override string ToString()
		{
			return TypeDescriptor.GetConverter(typeof(SvgPreserveAspectRatio)).ConvertToString(this.Align) + (Slice ? " slice" : "");
		}

	}
	
    [TypeConverter(typeof(SvgPreserveAspectRatioConverter))]
	public enum SvgPreserveAspectRatio
	{
		xMidYMid, //default
		none,
		xMinYMin,
		xMidYMin,
		xMaxYMin,
		xMinYMid,
		xMaxYMid,
		xMinYMax,
		xMidYMax,
		xMaxYMax
	}
}
