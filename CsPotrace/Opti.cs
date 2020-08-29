namespace CsPotrace
{
    /* a private type for the result of opti_penalty */
    public class Opti
	{
		public double pen = 0;
		public dPoint[] c = new dPoint[2];// [new dPoint()    , new dPoint()];
		public double t = 0;
		public double s = 0;
		public double alpha = 0;
	}
}
