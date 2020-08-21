/*
 * Created by SharpDevelop.
 * User: Diego
 * Date: 11/01/2017
 * Time: 00:02
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

namespace CsPotrace.BezierToBiarc
{
	/// <summary>
	/// Description of Tuple.
	/// </summary>
	public class Tuple<T1, T2> 
	{
        public T1 Item1 { get; }
        public T2 Item2 { get; }

        public Tuple(T1 item1, T2 item2) {
            Item1 = item1;
            Item2 = item2;
        }

    }
}