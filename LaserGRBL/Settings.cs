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

		static Settings()
		{
			try 
			{
				if (System.IO.File.Exists("LaserGRBL.Settings.bin"))
				{
					System.Runtime.Serialization.Formatters.Binary.BinaryFormatter f = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
					using (System.IO.FileStream fs = new System.IO.FileStream("LaserGRBL.Settings.bin", System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.None))
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
			return dic.ContainsKey(key) ? dic[key] : defval;
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
			System.Runtime.Serialization.Formatters.Binary.BinaryFormatter f = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
			using (System.IO.FileStream fs = new System.IO.FileStream("LaserGRBL.Settings.bin", System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.None))
			{
				f.Serialize(fs, dic);
				fs.Close();
			}
		}
	}
}
