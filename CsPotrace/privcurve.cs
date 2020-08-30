namespace CsPotrace
{
    public class privcurve
	{
		public int n;                   /* number of segments */
		public int[] tag;                /* tag[n]: POTRACE_CORNER or POTRACE_CURVETO */
		//     public dPoint[,] ControlPoints;  /* c[n][i]: control points. 
		//                      c[n][0] is unused for tag[n]=POTRACE_CORNER */
		public dPoint[] vertex;          /* for POTRACE_CORNER, this equals c[1] */
		/* (*c)[3]; /* c[n][i]: control points. 
		c[n][0] is unused for tag[n]=POTRACE_CORNER */
		public dPoint[] c = null;        // HelpPoint
		public double[] alpha;           /* only for POTRACE_CURVETO */
		public double[] alpha0;          /* "uncropped" alpha parameter - for debug output only */
		public double[] beta;
		public int alphacurve = 0;
		public privcurve(int Count)
		{
			n = Count;
			tag = new int[n];
			// ControlPoints = new dPoint[n, 3];
			vertex = new dPoint[n];
			alpha = new double[n];
			alpha0 = new double[n];
			beta = new double[n];
			c = new dPoint[n * 3];
		}
	}
}
