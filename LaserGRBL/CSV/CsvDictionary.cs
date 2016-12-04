/*
 * Created by SharpDevelop.
 * User: Diego
 * Date: 03/12/2016
 * Time: 12:44
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Reflection;
using System.IO;


namespace LaserGRBL.CSV
{
	/// <summary>
	/// Description of CsvDictionary.
	/// </summary>
	public class CsvDictionary 
	{
		private System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>> mD = new System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>>();
		private int mLen;
		
		public CsvDictionary(string resource, int len)
		{
      		StreamReader sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream(resource));
      		mLen = len;
      		string rline = null;
      		while ((rline = sr.ReadLine()) != null)
      		{
      			FormattedMessage fm = FormattedMessage.FromStringList(StringList.FromMessage(rline, ","));
      			string key = fm.ReadString().Trim();
      			System.Collections.Generic.List<string> data = new System.Collections.Generic.List<string>();
      			
      			for (int i = 0; i < mLen; i++)
      				data.Add(fm.ReadString().Trim());
      			
      			if (!mD.ContainsKey(key))
      				mD.Add(key, data);
      		}
		}
		
		public string GetItem(string key, int item)
		{
			if (mD.ContainsKey(key))
				return mD[key][item];
			else
				return null;
		}
	}
}
