using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LaserGRBL.RasterConverter
{
	public enum Tool
	{
		Line2Line,
		Dithering,
		Vectorize,
		Centerline
	}

	public enum Direction
	{
		Horizontal,
		Vertical,
		Diagonal,
		None
	}

}
