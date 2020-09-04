
namespace LaserGRBL
{
    public class GrblElement
	{
		protected char mCommand;
		protected decimal mNumber;

		public static implicit operator GrblElement(string value)
		{ return new GrblElement(value[0], decimal.Parse(value.Substring(1), System.Globalization.CultureInfo.InvariantCulture)); }

		public GrblElement(char Command, decimal Number)
		{
			mCommand = Command;
			mNumber = Number;
		}

		public char Command
		{ get { return mCommand; } }

		public decimal Number
		{ get { return mNumber; } }

		public override string ToString()
		{ return Command + Number.ToString(System.Globalization.CultureInfo.InvariantCulture); }

		public override bool Equals(object obj)
		{
			GrblElement o = obj as GrblElement;
			return o != null && o.mCommand == mCommand && o.mNumber == mNumber;
		}

		public override int GetHashCode()
		{ return mCommand.GetHashCode() ^ mNumber.GetHashCode(); }
	}
}
