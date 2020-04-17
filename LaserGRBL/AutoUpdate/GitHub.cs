//Copyright (c) 2016-2020 Diego Settimi - https://github.com/arkypita/

// This program is free software; you can redistribute it and/or modify  it under the terms of the GPLv3 General Public License as published by  the Free Software Foundation; either version 3 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GPLv3  General Public License for more details.
// You should have received a copy of the GPLv3 General Public License  along with this program; if not, write to the Free Software  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307,  USA. using System;

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace LaserGRBL
{
	class GitHub
	{
		public delegate void NewVersionDlg(Version current, Version latest, string name, string url);
		public static event NewVersionDlg NewVersion;

		public static bool Updating = false;

		public static void CheckVersion()
		{
			//public enum SecurityProtocolType
			//{
			//	Ssl3 = 48,
			//	Tls = 192,
			//	Tls11 = 768,
			//	Tls12 = 3072,
			//}

			if ((UrlManager.UpdateMain != null || UrlManager.UpdateMirror != null) && Settings.GetObject("Auto Update", true))
			{
				//https://developer.github.com/changes/2018-02-01-weak-crypto-removal-notice/
				try { System.Net.ServicePointManager.SecurityProtocol = (System.Net.SecurityProtocolType)3072; } //CONFIGURE SYSTEM FOR TLS 1.2 (Required since 22-02-2018) May work only if .net 4.5 is installed?
				catch { System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls; } //fallback, but not working with new github API!
				System.Net.ServicePointManager.ServerCertificateValidationCallback += new System.Net.Security.RemoteCertificateValidationCallback(bypassAllCertificateStuff);

				System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(GitHub.AsyncCheckVersion));
			}
		}

		private static bool bypassAllCertificateStuff(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
		{return true;}

		private static void AsyncCheckVersion(object foo)
		{
			if (UrlManager.UpdateMain != null)
			{
				try { CheckSite(UrlManager.UpdateMain); } //official https 
				catch
				{
					if (UrlManager.UpdateMirror != null)
					{
						try { CheckSite(UrlManager.UpdateMirror); }	//http mirror
						catch { }
					}
				}
			}
			else if (UrlManager.UpdateMirror != null) //only mirror configured
			{
				try { CheckSite(UrlManager.UpdateMirror); } //http mirror
				catch { }
			}
		}

		private static void CheckSite(string site)
		{
			using (System.Net.WebClient wc = new System.Net.WebClient())
			{
				wc.Headers.Add("User-Agent: .Net WebClient");
				string json = wc.DownloadString(site);

				string url = null;
				string versionstr = null;
				string name = null;

				foreach (Match m in Regex.Matches(json, @"""browser_download_url"":""([^""]+)"""))
					if (url == null)
						url = m.Groups[1].Value;
				foreach (Match m in Regex.Matches(json, @"""tag_name"":""v([^""]+)"""))
					if (versionstr == null)
						versionstr = m.Groups[1].Value;
				foreach (Match m in Regex.Matches(json, @"""name"":""([^""]+)"""))
					if (name == null)
						name = m.Groups[1].Value;

				Version current = typeof(GitHub).Assembly.GetName().Version;
				Version latest = new Version(versionstr);

				if (current < latest)
				{
					bool minor = Settings.GetObject("Auto Update", false);
					bool build = Settings.GetObject("Auto Update Build", false);

					if ((current.Major != latest.Major) || (current.Minor != latest.Minor && minor) || (current.Build != latest.Build && build))
						NewVersion?.Invoke(current, latest, name, url);
				}

			}
		}

		public static string installer { get { return System.IO.Path.Combine(GrblCore.TempPath, "LaserGRBL Updater.exe"); } }
		//public static string mainpath = @"LaserGRBL/";
		//public static string delext = ".todelete";

		private static System.Net.WebClient client;
		public static void DownloadUpdateA(string url, System.Net.DownloadProgressChangedEventHandler onprogr, System.ComponentModel.AsyncCompletedEventHandler oncomplete)
		{
			try
			{
				if (client == null)
				{
					if (System.IO.File.Exists(installer))
						System.IO.File.Delete(installer);

					client = new System.Net.WebClient();
					client.DownloadProgressChanged += onprogr;
					client.DownloadFileCompleted += disposeclient;
					client.DownloadFileCompleted += oncomplete;

					client.DownloadFileAsync(new System.Uri(url), installer);
				}
				else
				{
					oncomplete(null, new System.ComponentModel.AsyncCompletedEventArgs(new InvalidOperationException("Download already in progress!"), true, null));
				}
			}
			catch (Exception)
			{
				oncomplete(null, new System.ComponentModel.AsyncCompletedEventArgs(new InvalidOperationException("Error downloading!"), true, null));
			}
		}

		private static void disposeclient(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
		{
			if (client != null)
			{
				client.Dispose();
				client = null;
			}
		}

	
		internal static bool ApplyUpdateEXE()
		{
			//run downloaded exe
			if (System.IO.File.Exists(installer))
			{
				System.Diagnostics.ProcessStartInfo p = new System.Diagnostics.ProcessStartInfo { UseShellExecute = true, WorkingDirectory = Environment.CurrentDirectory, FileName = installer, Verb = "runas" };
				try { System.Diagnostics.Process.Start(p); return true; }
				catch { return false; }
			}

			return false;
		}

		public static void InitUpdate()
		{
			InitFlags();
			CleanupOldVersion();
		}

		private static void InitFlags()
		{
			if (!Settings.ExistObject("Auto Update Build"))
			{
				int percentage = 2; //enable "auto update build" on 2% of installation to be used as test platform

				Random rng = new Random();
				Settings.SetObject("Auto Update Build", Settings.GetObject("Auto Update", true) && rng.Next(0, 100) < percentage); 
				Settings.Save();
			}
		}

		private static void CleanupOldVersion()
		{
			try
			{
				foreach (string filePath in System.IO.Directory.GetFiles("./", "*.todelete", System.IO.SearchOption.AllDirectories))
					System.IO.File.Delete(filePath);
				if (System.IO.File.Exists("sessionlog.txt")) //old session log in program file
					System.IO.File.Delete("sessionlog.txt");
				if (System.IO.File.Exists("LaserGRBL.Settings.bin")) //old setting in program file
					System.IO.File.Delete("LaserGRBL.Settings.bin");
			}
			catch { }
		}

	}
}
