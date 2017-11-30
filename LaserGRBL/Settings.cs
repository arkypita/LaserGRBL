/*
 * Created by SharpDevelop.
 * User: Diego
 * Date: 25/12/2016
 * Time: 19:49
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace LaserGRBL
{
	/// <summary>
	/// Description of Settings.
	/// </summary>
	public static class Settings
	{
		private static System.Collections.Generic.Dictionary<string, object> dic;

		static string filename
		{ 
			get 
			{
				string basename = "LaserGRBL.Settings.bin";
				string fullname = System.IO.Path.Combine(GrblCore.DataPath, basename);

				if (!System.IO.File.Exists(fullname) && System.IO.File.Exists(basename))
					System.IO.File.Copy(basename, fullname);

				return fullname;
			} 
		}

		static Settings()
		{
			try 
			{
				if (System.IO.File.Exists(filename))
				{
					System.Runtime.Serialization.Formatters.Binary.BinaryFormatter f = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
					f.AssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple;
					using (System.IO.FileStream fs = new System.IO.FileStream(filename, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.None))
					{
						dic = (System.Collections.Generic.Dictionary<string, object>)f.Deserialize(fs);
						fs.Close();
					}
				}
				
			}
			catch {}
			
			if (dic == null)
				dic = new System.Collections.Generic.Dictionary<string, object>();
		}

		
		public static object GetObject(string key, object defval)
		{
			return dic.ContainsKey(key) && dic[key] != null ? dic[key] : defval;
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
				System.Runtime.Serialization.Formatters.Binary.BinaryFormatter f = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
				f.AssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple;
				using (System.IO.FileStream fs = new System.IO.FileStream(filename, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.None))
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
