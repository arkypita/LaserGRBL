//Copyright (c) 2016-2020 Diego Settimi - https://github.com/arkypita/

// This program is free software; you can redistribute it and/or modify  it under the terms of the GPLv3 General Public License as published by  the Free Software Foundation; either version 3 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GPLv3  General Public License for more details.
// You should have received a copy of the GPLv3 General Public License  along with this program; if not, write to the Free Software  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307,  USA. using System;

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
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
            public int Centerline;
            public int Dithering;
            public int Line2Line;
            public int SvgFile;

            internal void Update(UsageCounters c)
            {
                GCodeFile += c.GCodeFile;
                RasterFile += c.RasterFile;
                Vectorization += c.Vectorization;
                Centerline += c.Centerline;
                Dithering += c.Dithering;
                Line2Line += c.Line2Line;
                SvgFile += c.SvgFile;
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
        [Obsolete()] private Firmware Firmware; //non rimuovere, serve per compatibilità!
        private String FirmwareString;
        private UsageCounters Counters;

        private static UsageStats data;
        private static string filename = System.IO.Path.Combine(GrblCore.DataPath, "UsageStats.bin");

        public static void LoadFile() //in ingresso
        {
            bool exist = File.Exists(filename);
            data = (UsageStats)Tools.Serializer.ObjFromFile(filename);

            if (exist && data == null) //esiste ma non sono riuscito a caricarlo (perché è una versione 3.6.0 con campo Firmware stringa)
                FixFile();

            if (data == null) data = new UsageStats();
            data.UsageCount++;
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

                if (File.Exists(filename)) //posso cancellarlo per certo, perché tanto non sono riuscito a caricarlo!
                    File.Delete(filename);

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
                    File.Move(older.FullName, filename); //carica il file recuperato

                    foreach (FileInfo info in files) //cancella tutto il resto
                    {
                        if (File.Exists(info.FullName))
                            File.Delete(info.FullName);
                    }
                }
            }
            catch { }

            data = (UsageStats)Tools.Serializer.ObjFromFile(filename); //tenta il caricamento
        }

        public static void SaveFile(GrblCore Core) //in uscita
        {
            if (GitHub.Updating) //if updating: delay stat processing - skip this session
                return;

            if (UrlManager.Statistics != null)
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

            if (UsageTime < TimeSpan.Zero)
                UsageTime = TimeSpan.Zero; //fix wrong values

            if (Tools.TimingBase.TimeFromApplicationStartup() > TimeSpan.Zero) //prevent wrong values
                UsageTime = UsageTime.Add(Tools.TimingBase.TimeFromApplicationStartup());

            Wrapper = Settings.GetObject("ComWrapper Protocol", ComWrapper.WrapperType.UsbSerial);

			LaserGRBL.Firmware fw = Settings.GetObject("Firmware Type", LaserGRBL.Firmware.Grbl);
			if (fw == LaserGRBL.Firmware.Grbl && Core.IsOrturBoard)
				FirmwareString = "Ortur";
			else
				FirmwareString = fw.ToString();

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
                w.Timeout = 5000; //milliseconds
                return w;
            }
        }

    }
}
