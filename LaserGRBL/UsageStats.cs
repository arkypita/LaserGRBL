using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net; 

namespace LaserGRBL
{
	// this class is used to collect anonymous usage statistics
	// statistics will be used to provide better versions
	// focusing on the development of the most used features
	// and translation for most used languages

	[Serializable]
	public class UsageStats
	{
		[Serializable]
		public class UsageCounters
		{
			public int GCodeFile;
			public int RasterFile;
			public int Vectorization;
			public int Dithering;
			public int Line2Line;

			internal void Update(UsageCounters c)
			{
				GCodeFile += c.GCodeFile;
				RasterFile += c.RasterFile;
				Vectorization += c.Vectorization;
				Dithering += c.Dithering;
				Line2Line += c.Line2Line;
			}
		}

		private Guid InstallationID = Guid.NewGuid();
		private DateTime LastSent = DateTime.MinValue;
		private DateTime InstalledDate = System.IO.Directory.GetCreationTimeUtc(".");
		private Version Version = new Version(0,0,0);
		private GrblCore.GrblVersionInfo GrblVersion = new GrblCore.GrblVersionInfo(0,0);
		private int Locale = 0;
		private int UiLang = 0;
		private int UsageCount = 0;
		private TimeSpan UsageTime = TimeSpan.Zero;

		private ComWrapper.WrapperType Wrapper;
		private UsageCounters Counters;

		[NonSerialized()]
		private TimeSpan hUsageTime = TimeSpan.Zero;

		private static UsageStats data;
		private static string filename = System.IO.Path.Combine(GrblCore.DataPath, "UsageStats.bin");

		public static void LoadFile() //in ingresso
		{
			data = (UsageStats)Tools.Serializer.ObjFromFile(filename);
			if (data == null) data = new UsageStats();
			data.UsageCount++;
		}

		public static void SaveFile(GrblCore Core) //in uscita
		{
			if (GitHub.Updating) //if updating: delay stat processing - skip this session
				return;

			data.UpdateAndSend(Core); //manda solo se serve
			Tools.Serializer.ObjToFile(data, filename); //salva
		}

		private void UpdateAndSend(GrblCore Core)
		{
			//invia i dati solo almeno ad un giorno di distanza o al cambio version/grblversion
			Version current = typeof(GitHub).Assembly.GetName().Version;
			bool mustsend = DateTime.UtcNow.Subtract(LastSent).TotalDays > 1 || Version != current || (Core.Configuration.GrblVersion != null && GrblVersion != Core.Configuration.GrblVersion);
			Version = current;
			GrblVersion = Core.Configuration.GrblVersion != null ? Core.Configuration.GrblVersion : GrblVersion;
			Locale = System.Threading.Thread.CurrentThread.CurrentCulture.LCID;
			UiLang = System.Threading.Thread.CurrentThread.CurrentUICulture.LCID;

			TimeSpan tfas = Tools.TimingBase.TimeFromApplicationStartup();
			TimeSpan elaps = tfas - hUsageTime;
 			hUsageTime = tfas;
			UsageTime = UsageTime.Add(elaps);

			Wrapper = (ComWrapper.WrapperType)Settings.GetObject("ComWrapper Protocol", ComWrapper.WrapperType.UsbSerial);

			if (Counters == null) Counters = new UsageCounters();
			Counters.Update(Core.UsageCounters);

			if (mustsend)
			{
				try
				{
					if (TrueSend())
						LastSent = DateTime.UtcNow;
				}
				catch { }
			}
		}

		private bool TrueSend()
		{
			string urlAddress = "http://stats.lasergrbl.com/handler.php";
			using (MyWebClient client = new MyWebClient())
			{
				NameValueCollection postData = new NameValueCollection()
                {
                    { "guid", InstallationID.ToString("N") },
                    { "installed", InstalledDate.ToString("yyyy-MM-dd HH:mm:ss") },
                    { "version", Version.ToString(3) },
                    { "grblVersion", GrblVersion.ToString() },
                    { "locale", Locale.ToString() },
                    { "uiLang", UiLang.ToString() },
                    { "usageCount", UsageCount.ToString() },
					{ "usageTime", ((int)(UsageTime.TotalMinutes)).ToString() },
					{ "wrapperType", Wrapper.ToString() },
					{ "fGCodeFile", Counters.GCodeFile.ToString() },
					{ "fRasterFile", Counters.RasterFile.ToString() },
					{ "fVectorization", Counters.Vectorization.ToString() },
					{ "fDithering", Counters.Dithering.ToString() },
					{ "fLine2Line", Counters.Line2Line.ToString() },
                };

				// client.UploadValues returns page's source as byte array (byte[]) so it must be transformed into a string
				string rv = System.Text.Encoding.UTF8.GetString(client.UploadValues(urlAddress, postData));
				return (rv == "Success!");
			}
		}

		private class MyWebClient : WebClient
		{
			protected override WebRequest GetWebRequest(Uri uri)
			{
				WebRequest w = base.GetWebRequest(uri);
				w.Timeout = 10000; //milliseconds
				return w;
			}
		}

	}
}
