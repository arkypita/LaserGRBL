namespace CsPotrace
{
    /// <summary>
    /// Kind of Curve : Line or Bezier
    /// </summary>
    public enum CurveKind
	{
		Line,
		Bezier
	}
	public enum TurnPolicy
	{
		minority,
		majority,
		right,
		black,
		white
	}
}
