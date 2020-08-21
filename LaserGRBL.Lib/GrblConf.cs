//Copyright (c) 2016-2020 Diego Settimi - https://github.com/arkypita/

// This program is free software; you can redistribute it and/or modify  it under the terms of the GPLv3 General Public License as published by  the Free Software Foundation; either version 3 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GPLv3  General Public License for more details.
// You should have received a copy of the GPLv3 General Public License  along with this program; if not, write to the Free Software  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307,  USA. using System;

using System;
using System.Collections.Generic;

namespace LaserGRBL
{
	[Serializable]
	public class GrblConf : IEnumerable<KeyValuePair<int, decimal>>
	{
		public class GrblConfParam : ICloneable
		{
			public GrblConfParam(int number, decimal value)
			{
				Number = number;
				Value = value;
			}

			public int Number { get; }

			public string DollarNumber
			{ get { return "$" + Number.ToString(); } }

			public string Parameter
			{ get { return CSVD.Settings.GetItem(Number.ToString(), 0); } }

			public decimal Value { get; set; }

			public string Unit
			{ get { return CSVD.Settings.GetItem(Number.ToString(), 1); } }

			public string Description
			{ get { return CSVD.Settings.GetItem(Number.ToString(), 2); } }

			public object Clone()
			{ return this.MemberwiseClone(); }

		}

		private Dictionary<int, decimal> mData;
		private GrblVersionInfo mVersion;

		public GrblConf(GrblVersionInfo GrblVersion)
			: this()
		{
			mVersion = GrblVersion;
		}

		public GrblConf(GrblVersionInfo GrblVersion, Dictionary<int, decimal> configTable)
			: this(GrblVersion)
		{
			foreach (KeyValuePair<int, decimal> kvp in configTable)
				mData.Add(kvp.Key, kvp.Value);
		}

		public GrblConf()
		{ mData = new Dictionary<int, decimal>(); }

		public GrblVersionInfo GrblVersion
		{ get { return mVersion; } }

		private bool Version11
		{ get { return mVersion != null && mVersion >= new GrblVersionInfo(1, 1); } }

		private bool Version9
		{ get { return mVersion != null && mVersion >= new GrblVersionInfo(0, 9); } }

		private bool NoVersionInfo
		{ get { return mVersion == null; } }

		public int ExpectedCount
		{ get { return Version11 ? 34 : Version9 ? 31 : 23; } }

		public bool HomingEnabled
		{ get { return ReadWithDefault(Version9 ? 22 : 17, 1) != 0; } }

		public decimal MaxRateX
		{ get { return ReadWithDefault(Version9 ? 110 : 4, 4000); } }

		public decimal MaxRateY
		{ get { return ReadWithDefault(Version9 ? 111 : 5, 4000); } }

		public bool LaserMode
		{
			get
			{
				if (NoVersionInfo)
					return true;
				else
					return ReadWithDefault(Version11 ? 32 : -1, 0) != 0;
			}
		}

		public decimal MinPWM
		{ get { return ReadWithDefault(Version11 ? 31 : -1, 0); } }

		public decimal MaxPWM
		{ get { return ReadWithDefault(Version11 ? 30 : -1, 1000); } }

		public decimal ResolutionX
		{ get { return ReadWithDefault(Version9 ? 100 : 0, 250); } }

		public decimal ResolutionY
		{ get { return ReadWithDefault(Version9 ? 101 : 1, 250); } }

		public decimal TableWidth
		{ get { return ReadWithDefault(Version9 ? 130 : -1, 3000); } }

		public decimal TableHeight
		{ get { return ReadWithDefault(Version9 ? 131 : -1, 2000); } }


		private decimal ReadWithDefault(int number, decimal defval)
		{
			if (mVersion == null)
				return defval;
			else if (!mData.ContainsKey(number))
				return defval;
			else
				return mData[number];
		}

		public List<GrblConfParam> ToList()
		{
			List<GrblConfParam> rv = new List<GrblConfParam>();
			foreach (KeyValuePair<int, decimal> kvp in mData)
				rv.Add(new GrblConfParam(kvp.Key, kvp.Value));
			return rv;
		}

		private void Add(int num, decimal val)
		{
			mData.Add(num, val);
		}

		public int Count { get { return mData.Count; } }

		public bool HasChanges(GrblConfParam p)
		{
			if (!mData.ContainsKey(p.Number))
				return true;
			else if (mData[p.Number] != p.Value)
				return true;
			else
				return false;
		}

		private bool ContainsKey(int key)
		{
			return mData.ContainsKey(key);
		}

		private void SetValue(int key, decimal value)
		{
			mData[key] = value;
		}

		public IEnumerator<KeyValuePair<int, decimal>> GetEnumerator()
		{
			return mData.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return mData.GetEnumerator();
		}


		private static System.Text.RegularExpressions.Regex ConfRegEX = new System.Text.RegularExpressions.Regex(@"^[$](\d+) *= *(\d+\.?\d*)");

		public static bool IsSetConf(string p)
		{ return ConfRegEX.IsMatch(p); }

		public void AddOrUpdate(string p)
		{
			try
			{
				if (IsSetConf(p))
				{
					System.Text.RegularExpressions.MatchCollection matches = ConfRegEX.Matches(p);
					int key = int.Parse(matches[0].Groups[1].Value);
					decimal val = decimal.Parse(matches[0].Groups[2].Value, System.Globalization.CultureInfo.InvariantCulture);

					if (ContainsKey(key))
						SetValue(key, val);
					else
						Add(key, val);
				}
			}
			catch (Exception)
			{

			}
		}

		public bool SetValueIfKeyExist(string p)
		{
			try
			{
				if (IsSetConf(p))
				{
					System.Text.RegularExpressions.MatchCollection matches = ConfRegEX.Matches(p);
					int key = int.Parse(matches[0].Groups[1].Value);
					decimal val = decimal.Parse(matches[0].Groups[2].Value, System.Globalization.CultureInfo.InvariantCulture);

					if (!ContainsKey(key))
						return false;

					SetValue(key, val);
					return true;
				}
			}
			catch (Exception)
			{

			}

			return false;
		}

		public string ValidateConfig(int parid, object value)
		{
			if (parid == 33 && mVersion != null && mVersion.IsOrtur)
				return "This param control an Ortur safety feature. Please do not change this value!";

			return null;
		}
	}
}
