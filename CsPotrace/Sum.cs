namespace CsPotrace
{
    public struct Sum
	{
		public Sum(double x, double y, double xy, double x2, double y2)
		{
			this.x = x;
			this.y = y;
			this.xy = xy;
			this.x2 = x2;
			this.y2 = y2;
		}
		public double x, y, xy, x2, y2;

	}
}
