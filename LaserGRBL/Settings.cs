//Copyright (c) 2016-2020 Diego Settimi - https://github.com/arkypita/

// This program is free software; you can redistribute it and/or modify  it under the terms of the GPLv3 General Public License as published by  the Free Software Foundation; either version 3 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GPLv3  General Public License for more details.
// You should have received a copy of the GPLv3 General Public License  along with this program; if not, write to the Free Software  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307,  USA. using System;

using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;

namespace LaserGRBL
{
	/// <summary>
	/// Description of Settings.
	/// </summary>
	public static class Settings
	{
		private static Dictionary<string, object> dic;
		private static string dataPath = null;
		static string filename
		{ 
			get 
			{
				string basename = "LaserGRBL.Settings.bin";
				string fullname = Path.Combine(dataPath, basename);

				if (!File.Exists(fullname) && File.Exists(basename))
					File.Copy(basename, fullname);

				return fullname;
			} 
		}
		public static void Initialize(string DataPath) 
		{
			dataPath = DataPath;
            try
            {
                if (File.Exists(filename))
                {
                    BinaryFormatter f = new BinaryFormatter();
                    f.AssemblyFormat = FormatterAssemblyStyle.Simple;
                    using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.None))
                    {
                        dic = (Dictionary<string, object>)f.Deserialize(fs);
                        fs.Close();
                    }
                }

            }
            catch { }

			if (dic == null) dic = new Dictionary<string, object>();
        }


		public static T GetObject<T>(string key, T defval)
		{
			try
			{
				if (dic.ContainsKey(key))
				{
					object obj = dic[key];
					if (obj != null && obj.GetType() == typeof(T))
						return (T)obj;
				}
			}
			catch
			{
			}
			return defval;
		}

		public static object GetAndDeleteObject(string key, object defval)
		{
			object rv = dic.ContainsKey(key) && dic[key] != null ? dic[key] : defval;
			DeleteObject(key);
			return rv;
		}
		
		public static void SetObject(string key, object value)
		{
			if (dic.ContainsKey(key))
				dic[key] = value;
			else
				dic.Add(key, value);
		}
		
		public static void Save()
		{
			try
			{
				BinaryFormatter f = new BinaryFormatter();
				f.AssemblyFormat = FormatterAssemblyStyle.Simple;
				using (FileStream fs = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None))
				{
					f.Serialize(fs, dic);
					fs.Close();
				}
			}
			catch { }
		}

		internal static void DeleteObject(string key)
		{
			if (dic.ContainsKey(key))
				dic.Remove(key);
		}

		internal static bool ExistObject(string key)
		{
			return dic.ContainsKey(key);
		}
	}
}
