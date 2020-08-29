namespace CsPotrace
{
    public class Quad
	{
		public double[] data = new double[9];

		public double at(int x, int y)
		{
			return this.data[x * 3 + y];
		}
	}

}