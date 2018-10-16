using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Svg.Transforms
{
    public abstract class SvgTransform : ICloneable
    {
        public abstract Matrix Matrix { get; }
        public abstract string WriteToString();

    	public abstract object Clone();
    	
    	#region Equals implementation
    	public override bool Equals(object obj)
		{
			SvgTransform other = obj as SvgTransform;
			if (other == null)
				return false;
			
			var thisMatrix = this.Matrix.Elements;
			var otherMatrix = other.Matrix.Elements;
			
			for (int i = 0; i < 6; i++) 
			{
				if(thisMatrix[i] != otherMatrix[i])
					return false;
			}
			
			return true;
		}
    	
    	public override int GetHashCode()
		{
    		int hashCode = this.Matrix.GetHashCode();
			return hashCode;
		}

    	
		public static bool operator ==(SvgTransform lhs, SvgTransform rhs)
		{
			if (ReferenceEquals(lhs, rhs))
				return true;
			if (ReferenceEquals(lhs, null) || ReferenceEquals(rhs, null))
				return false;
			return lhs.Equals(rhs);
		}
    	
		public static bool operator !=(SvgTransform lhs, SvgTransform rhs)
		{
			return !(lhs == rhs);
		}
    	#endregion

        public override string ToString()
        {
            return WriteToString();
        }
    }
}