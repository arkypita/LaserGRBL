/*
 * Created by SharpDevelop.
 * User: Diego
 * Date: 11/01/2017
 * Time: 00:02
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace CsPotrace.BezierToBiarc
{
	/// <summary>
	/// Description of Tuple.
	/// </summary>
	public class Tuple<T1, T2> 
	{
 
        private readonly T1 m_Item1;
        private readonly T2 m_Item2;
 
        public T1 Item1 { get { return m_Item1; } }
        public T2 Item2 { get { return m_Item2; } }
 
        public Tuple(T1 item1, T2 item2) {
            m_Item1 = item1;
            m_Item2 = item2;
        }
 
//        Boolean IStructuralEquatable.Equals(Object other, IEqualityComparer comparer) {
//            if (other == null) return false;
// 
//            Tuple<T1, T2> objTuple = other as Tuple<T1, T2>;
// 
//            if (objTuple == null) {
//                return false;
//            }
// 
//            return comparer.Equals(m_Item1, objTuple.m_Item1) && comparer.Equals(m_Item2, objTuple.m_Item2);
//        }
// 
//        Int32 IComparable.CompareTo(Object obj) {
//            return ((IStructuralComparable) this).CompareTo(obj, Comparer<Object>.Default);
//        }
// 
//        Int32 IStructuralComparable.CompareTo(Object other, IComparer comparer) {
//            if (other == null) return 1;
// 
//            Tuple<T1, T2> objTuple = other as Tuple<T1, T2>;
// 
//            if (objTuple == null) {
//                throw new ArgumentException(Environment.GetResourceString("ArgumentException_TupleIncorrectType", this.GetType().ToString()), "other");
//            }
// 
//            int c = 0;
// 
//            c = comparer.Compare(m_Item1, objTuple.m_Item1);
// 
//            if (c != 0) return c;
// 
//            return comparer.Compare(m_Item2, objTuple.m_Item2);
//        }
// 
//        public override int GetHashCode() {
//            return ((IStructuralEquatable) this).GetHashCode(EqualityComparer<Object>.Default);
//        }
// 
//        Int32 IStructuralEquatable.GetHashCode(IEqualityComparer comparer) {
//            return Tuple.CombineHashCodes(comparer.GetHashCode(m_Item1), comparer.GetHashCode(m_Item2));
//        }
// 
//        Int32 ITuple.GetHashCode(IEqualityComparer comparer) {
//            return ((IStructuralEquatable) this).GetHashCode(comparer);
//        }
//        public override string ToString() {
//            StringBuilder sb = new StringBuilder();
//            sb.Append("(");
//            return ((ITuple)this).ToString(sb);
//        }
// 
//        string ITuple.ToString(StringBuilder sb) {
//            sb.Append(m_Item1);
//            sb.Append(", ");
//            sb.Append(m_Item2);
//            sb.Append(")");
//            return sb.ToString();
//        }
// 
//        int ITuple.Size {
//            get {
//                return 2;
//            }
//        }
    }
}