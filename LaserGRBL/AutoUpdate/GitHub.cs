using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace LaserGRBL
{
	class GitHub
	{
		public delegate void NewVersionDlg(Version current, Version latest, string name, string url);
		public static event NewVersionDlg NewVersion;

		public static void CheckVersion()
		{
			if ((bool)Settings.GetObject("Auto Update", true))
				System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(GitHub.AsyncCheckVersion));
		}

		private static void AsyncCheckVersion(object foo)
		{
			try
			{ CheckSite(@"https://api.github.com/repos/arkypita/LaserGRBL/releases/latest"); } //official https
			catch
			{
				try { CheckSite(@"http://lasergrbl.com/latest.php"); }//http mirror
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
					if (NewVersion != null)
						NewVersion(current, latest, name, url);
				}

			}
		}

		public static string zipfile {get{return System.IO.Path.Combine(GrblCore.TempPath, "lasergrblupdate.package");}}
		public static string mainpath = @"LaserGRBL/";
		public static string delext = ".todelete";

		private static System.Net.WebClient client;
		public static void DownloadUpdateA(string url, System.Net.DownloadProgressChangedEventHandler onprogr, System.ComponentModel.AsyncCompletedEventHandler oncomplete)
		{
			try
			{
				if (client == null)
				{
					if (System.IO.File.Exists(zipfile))
						System.IO.File.Delete(zipfile);

					client = new System.Net.WebClient();
					client.DownloadProgressChanged += onprogr;
					client.DownloadFileCompleted += disposeclient;
					client.DownloadFileCompleted += oncomplete;

					client.DownloadFileAsync(new System.Uri(url), zipfile);
				}
				else
				{
					oncomplete(null, new System.ComponentModel.AsyncCompletedEventArgs(new InvalidOperationException("Download already in progress!"), true, null));
				}
			}
			catch (Exception ex)
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

		public static string UpdaterExeName = "LaserGRBL Updater.exe";

		public static bool ApplyUpdateS1() //step one, create the updater and run elevated with AU switch
		{
			string lasergrbl = System.Windows.Forms.Application.ExecutablePath;
			string updater = System.IO.Path.Combine(GrblCore.TempPath, UpdaterExeName);
			if (System.IO.File.Exists(updater))
				System.IO.File.Delete(updater);
			System.IO.File.Copy(lasergrbl, updater);

			System.Diagnostics.ProcessStartInfo p = new System.Diagnostics.ProcessStartInfo { UseShellExecute = true, WorkingDirectory = Environment.CurrentDirectory, FileName = updater, Verb = "runas", Arguments = String.Format("AU {0} \"{1}\"", System.Diagnostics.Process.GetCurrentProcess().Id, lasergrbl) };
			try { System.Diagnostics.Process.Start(p); return true; }
			catch { return false; }	
		}

		public static bool ApplyUpdateS2() //step two, real apply update, if elevated
		{
			try
			{
				if (System.IO.File.Exists(zipfile)) //il download ha fatto il suo lavoro
				{
					using (System.IO.Compression.ZipStorer zs = System.IO.Compression.ZipStorer.Open(zipfile, System.IO.FileAccess.Read))
					{
						foreach (System.IO.Compression.ZipStorer.ZipFileEntry ze in zs.ReadCentralDir())
						{
							string fname = ze.FilenameInZip;
							if (fname.StartsWith(mainpath))
								fname = fname.Substring(mainpath.Length);

							if (System.IO.File.Exists(fname))
								System.IO.File.Delete(fname);

							zs.ExtractFile(ze, "./" + fname);
						}

						zs.Close();
						System.IO.File.Delete(zipfile);
						return true;
					}
				}
			}
			catch (Exception ex){}

			return false;
		}


		public static void CleanupOldVersion()
		{
			try
			{
				foreach (string filePath in System.IO.Directory.GetFiles("./", "*" + delext, System.IO.SearchOption.AllDirectories))
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
