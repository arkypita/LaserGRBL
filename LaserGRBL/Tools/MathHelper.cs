using System;

namespace Tools
{
	/// <summary>
	/// Description of MathHelper.
	/// </summary>
	public static class MathHelper
	{
		public static decimal LinearDistance(decimal curX, decimal curY, decimal newX, decimal newY)
		{
			decimal dX = newX - curX;
			decimal dY = newY - curY;
			return (decimal)Math.Sqrt((double)(dX * dX + dY * dY));
		}


	}

	public class RulerStepCalculator
	{
		private double[] mtable = new double[] { 1.0, 2.0, 5.0, 10.0 };
		private double[] stable = new double[] { 2.0, 2.0, 5.0, 10.0 };
		private double _bigstep = 0;
		private double _smallstep = 0;
		private double _firstsmall = 0;
		private double _firstbig = 0;
		private double _min = 0;
		private double _max = 0;
		private int _steps = 0;

		public RulerStepCalculator(double min, double max, int steps)
		{
			_min = min;
			_max = max;
			_steps = steps;

			double logar = System.Math.Log10((max - min) / steps);
			double multiplier = System.Math.Pow(10.0, System.Math.Floor(logar));
			int i = 0;

			logar = System.Math.IEEERemainder(logar, 1.0);
			if ((logar < 0.0))
				logar = logar + 1.0;

			while ((logar >= System.Math.Log10(mtable[i + 1])))
				i += 1;

			_bigstep = mtable[i] * multiplier;
			_smallstep = _bigstep / stable[i];
			_firstsmall = System.Math.Ceiling(min / _smallstep) * _smallstep;
			_firstbig = System.Math.Ceiling(min / _bigstep) * _bigstep;
		}

		// void EvaluateSteps(double min, double max)
		// {
		// static double table[] = { 1.0, 1.25, 1.5, 2.0, 2.5, 3.0, 4.0, 5.0, 6.0, 8.0, 10.0 } ;
		// double logar = log10((max - min) / REQ_STEPS) ; // 	REQ_STEPS = quante tacche vogliamo
		// double multiplier = pow(10.0, floor(logar)) ;
		// int i = 0 ;

		// logar = fmod(logar, 1.0) ;
		// if (logar < 0.0)
		// logar = logar + 1.0 ;
		// while (logar >= log10(table[i + 1]))
		// i++ ;
		// BigStep = table[i] * multiplier ;
		// SmallStep = BigStep / 4.0 ;
		// FirstSmall = ceil(min / SmallStep) * SmallStep ;
		// FirstBig = ceil(max / BigStep) * BigStep ;
		// }



		public double ReqMin
		{
			get
			{
				return _min;
			}
		}

		public double ReqMax
		{
			get
			{
				return _max;
			}
		}

		public double ReqSteps
		{
			get
			{
				return _steps;
			}
		}

		public double BigStep
		{
			get
			{
				return _bigstep;
			}
		}

		public double SmallStep
		{
			get
			{
				return _smallstep;
			}
		}

		public double FirstSmall
		{
			get
			{
				return _firstsmall;
			}
		}

		public double FirstBig
		{
			get
			{
				return _firstbig;
			}
		}
	}

}
