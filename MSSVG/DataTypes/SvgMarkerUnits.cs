using System.ComponentModel;

namespace Svg.DataTypes
{
    /// <summary>Defines the coordinate system for attributes ‘markerWidth’, ‘markerHeight’ and the contents of the ‘marker’.</summary>
	[TypeConverter(typeof(SvgMarkerUnitsConverter))]
	public enum SvgMarkerUnits
	{
        /// <summary>If markerUnits="strokeWidth", ‘markerWidth’, ‘markerHeight’ and the contents of the ‘marker’ represent values in a coordinate system which has a single unit equal the size in user units of the current stroke width (see the ‘stroke-width’ property) in place for the graphic object referencing the marker.</summary>
		StrokeWidth,

        /// <summary>If markerUnits="userSpaceOnUse", ‘markerWidth’, ‘markerHeight’ and the contents of the ‘marker’ represent values in the current user coordinate system in place for the graphic object referencing the marker (i.e., the user coordinate system for the element referencing the ‘marker’ element via a ‘marker’, ‘marker-start’, ‘marker-mid’ or ‘marker-end’ property).</summary>
		UserSpaceOnUse
	}
}
