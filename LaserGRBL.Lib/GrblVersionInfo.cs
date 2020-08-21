using System;

namespace LaserGRBL
{

    [Serializable]
	public class GrblVersionInfo : IComparable, ICloneable
	{
		int mMajor;
		int mMinor;
		char mBuild;
		bool mOrtur;

		public GrblVersionInfo(int major, int minor, char build, string orturwelcome, string orturversion)
		{
			mMajor = major; mMinor = minor; mBuild = build;
			mOrtur = orturwelcome != null;
		}

		public GrblVersionInfo(int major, int minor, char build)
		{ mMajor = major; mMinor = minor; mBuild = build; }

		public GrblVersionInfo(int major, int minor)
		{ mMajor = major; mMinor = minor; mBuild = (char)0; }

		public static bool operator !=(GrblVersionInfo a, GrblVersionInfo b)
		{ return !(a == b); }

		public static bool operator ==(GrblVersionInfo a, GrblVersionInfo b)
		{
			if (Object.ReferenceEquals(a, null))
				return Object.ReferenceEquals(b, null);
			else
				return a.Equals(b);
		}

		public static bool operator <(GrblVersionInfo a, GrblVersionInfo b)
		{
			if ((Object)a == null)
				throw new ArgumentNullException("a");
			return (a.CompareTo(b) < 0);
		}

		public static bool operator <=(GrblVersionInfo a, GrblVersionInfo b)
		{
			if ((Object)a == null)
				throw new ArgumentNullException("a");
			return (a.CompareTo(b) <= 0);
		}

		public static bool operator >(GrblVersionInfo a, GrblVersionInfo b)
		{ return (b < a); }

		public static bool operator >=(GrblVersionInfo a, GrblVersionInfo b)
		{ return (b <= a); }

		public override string ToString()
		{
			if (mBuild == (char)0)
				return string.Format("{0}.{1}", mMajor, mMinor);
			else
				return string.Format("{0}.{1}{2}", mMajor, mMinor, mBuild);
		}

		public override bool Equals(object obj)
		{
			GrblVersionInfo v = obj as GrblVersionInfo;
			return v != null && this.mMajor == v.mMajor && this.mMinor == v.mMinor && this.mBuild == v.mBuild && this.mOrtur == v.mOrtur;
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int hash = 17;
				// Maybe nullity checks, if these are objects not primitives!
				hash = hash * 23 + mMajor.GetHashCode();
				hash = hash * 23 + mMinor.GetHashCode();
				hash = hash * 23 + mBuild.GetHashCode();
				return hash;
			}
		}

		public int CompareTo(Object version)
		{
			if (version == null)
				return 1;

			GrblVersionInfo v = version as GrblVersionInfo;
			if (v == null)
				throw new ArgumentException("Argument must be GrblVersionInfo");

			if (this.mMajor != v.mMajor)
				if (this.mMajor > v.mMajor)
					return 1;
				else
					return -1;

			if (this.mMinor != v.mMinor)
				if (this.mMinor > v.mMinor)
					return 1;
				else
					return -1;

			if (this.mBuild != v.mBuild)
				if (this.mBuild > v.mBuild)
					return 1;
				else
					return -1;

			return 0;
		}

		public object Clone()
		{ return this.MemberwiseClone(); }

		public int Major { get { return mMajor; } }

		public int Minor { get { return mMinor; } }

		public bool IsOrtur { get => mOrtur; internal set => mOrtur = value; }
	}

}
