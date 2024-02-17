using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaserGRBL.Resources
{
	public static class ResourceHelper
	{
		public static bool IsEmbeddedResource(string strName)
		{
			return strName.StartsWith("LaserGRBL.");
		}

		public static Stream GetFileAsStream(string strName)
		{
			return System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(strName);
		}

		internal static string GetFileAsText(string file)
		{
			using (StreamReader reader = new StreamReader(GetFileAsStream(file)))
			{
				return reader.ReadToEnd();
			}
		}
	}
}
