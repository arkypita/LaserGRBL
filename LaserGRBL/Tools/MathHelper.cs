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
}
