using Xunit;

namespace LaserGRBL.Tests.PrintAndCut
{
	/// <summary>
	/// Tests that run the SVG converter: it keeps state in static fields (the <c>gcode</c> helper),
	/// so they must not run in parallel.
	/// </summary>
	[CollectionDefinition(Name)]
	public class SvgConverterCollection
	{
		/// <summary>Collection name.</summary>
		public const string Name = "SVG converter (static state)";
	}
}
