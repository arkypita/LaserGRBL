//Copyright (c) 2016-2020 Diego Settimi - https://github.com/arkypita/

// This program is free software; you can redistribute it and/or modify  it under the terms of the GPLv3 General Public License as published by  the Free Software Foundation; either version 3 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GPLv3  General Public License for more details.
// You should have received a copy of the GPLv3 General Public License  along with this program; if not, write to the Free Software  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307,  USA. using System;

using System;
using System.Reflection;
using System.IO;
using System.Collections.Generic;

namespace LaserGRBL.CSV
{
	/// <summary>
	/// Description of CsvDictionary.
	/// </summary>
	public class CsvList 
	{
		protected List<List<string>> mD = new List<List<string>>();
		private int mLen;
		
		public CsvList(string filename, int len)
		{
      		StreamReader sr = new StreamReader(filename);
      		mLen = len;
      		string rline = null;
      		while ((rline = sr.ReadLine()) != null)
      		{
      			FormattedMessage fm = FormattedMessage.FromStringList(StringList.FromMessage(rline, ";"));
      			List<string> data = new List<string>();
      			
      			for (int i = 0; i < mLen; i++)
      				data.Add(fm.ReadString().Trim());
      			
				mD.Add(data);
      		}
		}
		
		//public string GetItem(string key, int item)
		//{
		//	if (mD.ContainsKey(key))
		//		return mD[key][item];
		//	else
		//		return null;
		//}
	}
}
