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
			Cleanup();

			if ((bool)Settings.GetObject("Auto Update", true))
				System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(GitHub.AsyncCheckVersion));
		}

		private static void AsyncCheckVersion(object foo)
		{
			//try
			//{ CheckSite(@"https://api.github.com/repos/arkypita/LaserGRBL/releases/latest"); } //official https
			//catch
			{ CheckSite(@"http://lasergrbl.com/latest.php"); } //http mirror
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

		public static string zipfile = "lasergrblupdate.package";
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

		public static bool ApplyUpdate()
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
							{
								if (System.IO.File.Exists(fname + delext))
									System.IO.File.Delete(fname + delext);

								System.IO.File.Move(fname, fname + delext);
								System.IO.File.SetAttributes(fname + delext, System.IO.FileAttributes.Hidden);
							}

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


		public static void Cleanup()
		{
			System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(GitHub.AsyncCleanup));
		}

		private static void AsyncCleanup(object foo)
		{
			try
			{
				foreach (string filePath in System.IO.Directory.GetFiles("./", "*" + delext, System.IO.SearchOption.AllDirectories))
					System.IO.File.Delete(filePath);
				if (System.IO.File.Exists(zipfile))
					System.IO.File.Delete(zipfile);
			}
			catch { }
		}
	}
}
