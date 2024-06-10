//Copyright (c) 2016-2021 Diego Settimi - https://github.com/arkypita/

// This program is free software; you can redistribute it and/or modify  it under the terms of the GPLv3 General Public License as published by  the Free Software Foundation; either version 3 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GPLv3  General Public License for more details.
// You should have received a copy of the GPLv3 General Public License  along with this program; if not, write to the Free Software  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307,  USA. using System;

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Net;
using System.Runtime.Serialization;
using System.Linq;
using LaserGRBL.UserControls;

namespace LaserGRBL
{
    // this class is used to collect anonymous usage statistics
    // statistics will be used to provide better versions
    // focusing on the development of the most used features
    // and translation for most used languages

    [Serializable]
    public class UsageStats
    {
#pragma warning disable 0169
		[Obsolete()] private Firmware Firmware; //non rimuovere, serve per compatibilità!
#pragma warning restore 0169


		[Serializable]
        public class UsageCounters
        {
            public int GCodeFile;
            public int RasterFile;
            public int Vectorization;
            public int Centerline;
            public int Dithering;
            public int Line2Line;
            public int SvgFile;
			public int Passthrough;


			internal void Update(UsageCounters c)
            {
                GCodeFile += c.GCodeFile;
                RasterFile += c.RasterFile;
                Vectorization += c.Vectorization;
                Centerline += c.Centerline;
                Dithering += c.Dithering;
                Line2Line += c.Line2Line;
                SvgFile += c.SvgFile;
				Passthrough += c.Passthrough;
			}
        }

        private Guid InstallationID = Guid.NewGuid();
        private DateTime LastSent = DateTime.MinValue;
        private DateTime InstalledDate = System.IO.Directory.GetCreationTimeUtc(".");
        private Version Version = new Version(0, 0, 0);
        private GrblCore.GrblVersionInfo GrblVersion = new GrblCore.GrblVersionInfo(0, 0);
        private int Locale = 0;
        private int UiLang = 0;
        private int UsageCount = 0;
        private TimeSpan UsageTime = TimeSpan.Zero;

        private ComWrapper.WrapperType Wrapper;
		private String FirmwareString;
		private String VendorString;

		private UsageCounters Counters;
		
		private static MessageManager mManager;
        private static UsageStats data;
        private static string datafilename = System.IO.Path.Combine(GrblCore.DataPath, "UsageStats.bin");
		private static string messagefilename = System.IO.Path.Combine(GrblCore.DataPath, "Message.bin");

		public static MessageManager Messages { get => mManager;  }
        public static bool DoNotSendNow = false; // used to delay sending statistics (i.e. when updating)
		public static void LoadFile() //in ingresso
        {
			bool exist = File.Exists(datafilename);
            data = (UsageStats)Tools.Serializer.ObjFromFile(datafilename);

			if (exist && data == null) //esiste ma non sono riuscito a caricarlo (perché è una versione 3.6.0 con campo Firmware stringa)
                FixFile();

            if (data == null) data = new UsageStats();
            data.UsageCount++;

			if (File.Exists(messagefilename))
				mManager = (MessageManager)Tools.Serializer.ObjFromFile(messagefilename, "lasergrbl");
			if (mManager == null)
				mManager = new MessageManager();
		}

		private static void FixFile()
        {
            FixFile360();
        }

        private static void FixFile360()
        {
            try
            {
                string token = Path.Combine(GrblCore.DataPath, "360fix");
                if (File.Exists(token))
                    return;

                File.Create(token).Dispose();


                DateTime startissue = new DateTime(2020, 07, 09).ToUniversalTime();

                if (File.Exists(datafilename)) //posso cancellarlo per certo, perché tanto non sono riuscito a caricarlo!
                    File.Delete(datafilename);

                DirectoryInfo folder = new DirectoryInfo(GrblCore.DataPath);
                FileInfo[] files = folder.GetFiles("*UsageStats.bin.dam");
                FileInfo older = null;
                foreach (FileInfo info in files)
                {
                    if (info.LastWriteTimeUtc > startissue && info.LastWriteTimeUtc < startissue.AddMonths(2)) //considera di recuperare solo file corrotti dalla versione 3.6.0 rilasciata l' 11/07/2020
                    {
                        if (older == null || info.LastWriteTimeUtc < older.LastWriteTimeUtc)
                            older = info;
                    }
                }

                if (older != null)
                {
                    File.Move(older.FullName, datafilename); //carica il file recuperato

                    foreach (FileInfo info in files) //cancella tutto il resto
                    {
                        if (File.Exists(info.FullName))
                            File.Delete(info.FullName);
                    }
                }
            }
            catch { }

            data = (UsageStats)Tools.Serializer.ObjFromFile(datafilename); //tenta il caricamento
        }

		public static void SaveFile(GrblCore Core) //in uscita
        {
            if (GitHub.Updating) //if updating: delay stat processing - skip this session
                return;

            if (UrlManager.Statistics != null)
                data.UpdateAndSend(Core); //manda solo se serve

            Tools.Serializer.ObjToFile(data, datafilename); //salva
			Tools.Serializer.ObjToFile(mManager, messagefilename, "lasergrbl"); //salva
		}

        private void UpdateAndSend(GrblCore Core)
        {
            //invia i dati solo almeno ad un giorno di distanza o al cambio version/grblversion
            Version current = Program.CurrentVersion;
            bool mustsend = !DoNotSendNow && (DateTime.UtcNow.Subtract(LastSent).TotalDays > 5 || Version != current || (GrblCore.Configuration.GrblVersion != null && GrblVersion != GrblCore.Configuration.GrblVersion));
            Version = current;
            GrblVersion = GrblCore.Configuration.GrblVersion != null ? GrblCore.Configuration.GrblVersion : GrblVersion;
            Locale = System.Threading.Thread.CurrentThread.CurrentCulture.LCID;
            UiLang = System.Threading.Thread.CurrentThread.CurrentUICulture.LCID;

            if (UsageTime < TimeSpan.Zero)
                UsageTime = TimeSpan.Zero; //fix wrong values

            if (Tools.TimingBase.TimeFromApplicationStartup() > TimeSpan.Zero) //prevent wrong values
                UsageTime = UsageTime.Add(Tools.TimingBase.TimeFromApplicationStartup());

            Wrapper = Settings.GetObject("ComWrapper Protocol", ComWrapper.WrapperType.UsbSerial);

			LaserGRBL.Firmware fw = Settings.GetObject("Firmware Type", LaserGRBL.Firmware.Grbl);
			FirmwareString = fw.ToString();
			VendorString = Core?.GrblVersion?.VendorName != null ? Core.GrblVersion.VendorName : "Unknown";

			if (Counters == null) Counters = new UsageCounters();
            Counters.Update(Core.UsageCounters);

            if (mustsend)
            {
                try
                {
                    if (TrueSend())
                        LastSent = DateTime.UtcNow;
                }
                catch (Exception)
                {
                }
            }
        }

        private bool TrueSend()
        {
            if (UrlManager.Statistics == null)
                return false;

            string urlAddress = UrlManager.Statistics;
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
                    { "fSvgFile", Counters.SvgFile.ToString() },
                    { "fCenterline", Counters.Centerline.ToString() },
                    { "firmware", FirmwareString },
                    { "osinfo", Tools.OSHelper.GetOSInfo() },
                    { "bitflag", Tools.OSHelper.GetBitFlag().ToString() },
					{ "vendor", VendorString },
					{ "fPassthrough", Counters.Passthrough.ToString() },
					{ "RenderType", $"{(int)Settings.RequestedGraphicMode}|{(int)Settings.CurrentGraphicMode}" },
					{ "RenderVendor", GrblPanel3D.CurrentVendor },
					{ "RenderName", GrblPanel3D.CurrentRenderer },
					{ "RenderGLVersion", GrblPanel3D.CurrentGLVersion },
					{ "RenderError", GrblPanel3D.GlDiagnosticMessage },
				};

                // client.UploadValues returns page's source as byte array (byte[]) so it must be transformed into a string
                string json = System.Text.Encoding.UTF8.GetString(client.UploadValues(urlAddress, postData));

				UsageStatsRV RV = Tools.JSONParser.FromJson<UsageStatsRV>(json);
				mManager.SetMessages(RV.Messages);

				return (RV.Success);
            }
        }

		public class UsageStatsRV
		{
			public int UpdateResult = -1;
			public List<MessageData> Messages = null;

			[IgnoreDataMember] public bool Success => UpdateResult == 1;
		}

		[Serializable]
		public class MessageManager
		{
			private List<MessageData> Messages = new List<MessageData>();
			private List<int> ClearedIDs = new List<int>();

			public IEnumerable<MessageData> GetMessages(MessageData.MessageTypes type)
			{return Messages.Where(M => M.Type == type && !ClearedIDs.Contains(M.ID));}

			public void ClearMessage(MessageData message)
			{
				if (!ClearedIDs.Contains(message.ID))
					ClearedIDs.Add(message.ID);
			}

			internal void SetMessages(List<MessageData> messages)
			{
				Messages = messages != null ? messages : new List<MessageData>();
			}

			internal MessageData GetMessage(MessageData.MessageTypes type)
			{ return Messages.FirstOrDefault(M => M.Type == type && !ClearedIDs.Contains(M.ID)); }
		}

		[Serializable]
		public class MessageData
		{

			public enum MessageTypes
			{ Unknown = -1, ToolbarLink = 0, AutoLink = 1  }

			public string id;
			public string date;         //2020-10-08 09:59:47
			public string type;
			public string title;
			public string content;
			public string date_from;    //2020-10-01
			public string date_to;
			public string clearable;

			[IgnoreDataMember] public int ID { get => string.IsNullOrEmpty(id) ? -1 : int.Parse(id); }
			[IgnoreDataMember] public DateTime Date { get => string.IsNullOrEmpty(date) ? DateTime.Today : DateTime.ParseExact(date, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture); }
			[IgnoreDataMember] public MessageTypes Type { get => (MessageTypes)iType; }
			[IgnoreDataMember] public string Title { get => title; }
			[IgnoreDataMember] public string Content { get => content; }
			[IgnoreDataMember] public DateTime DateFrom { get => string.IsNullOrEmpty(date_from) ? DateTime.MinValue : DateTime.ParseExact(date_from, "yyyy-MM-dd", CultureInfo.InvariantCulture); }
			[IgnoreDataMember] public DateTime DateTo { get => string.IsNullOrEmpty(date_to) ? DateTime.MaxValue : DateTime.ParseExact(date_to, "yyyy-MM-dd", CultureInfo.InvariantCulture); }
			[IgnoreDataMember] public bool Clearable { get => clearable == "1" ? true : false; }


			[IgnoreDataMember] private int iType { get => string.IsNullOrEmpty(type) ? -1 : int.Parse(type); }
		}

		public class MyWebClient : WebClient
        {
            protected override WebRequest GetWebRequest(Uri uri)
            {
                WebRequest w = base.GetWebRequest(uri);
                w.Timeout = 5000; //milliseconds
                return w;
            }
        }

		internal static string GetID()
		{
			return data?.InstallationID.ToString("N");
		}
	}
}
