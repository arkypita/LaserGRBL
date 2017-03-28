using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace LaserGRBL
{
	class GitHub
	{
		public delegate void NewVersionDlg(Version current, Version latest, string name, string url);
		public static event NewVersionDlg NewVersion;

		public delegate void UpdateResultDlg(bool result);
		public static event UpdateResultDlg UpdateResult;

		public static void CheckVersion()
		{
			Cleanup();

			if ((bool)Settings.GetObject("Auto Update", true))
				System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(GitHub.AsyncCheckVersion));
		}

		private static void AsyncCheckVersion(object foo)
		{
			try
			{
				using (System.Net.WebClient wc = new System.Net.WebClient())
				{
					wc.Headers.Add("User-Agent: .Net WebClient");
					string json = wc.DownloadString(@"https://api.github.com/repos/arkypita/LaserGRBL/releases/latest");

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
			catch (Exception ex) 
			{
 
			}
		}

		public static void Update(string url)
		{
			System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(GitHub.AsyncUpdate), url);
		}

		static string zipfile = "lasergrblupdate.package";
		static string mainpath = @"LaserGRBL/";
		static string delext = ".todelete";

		private static void AsyncUpdate(object foo)
		{
			string url = foo as string;
			bool done = false;
			try
			{
				using (var client = new System.Net.WebClient())
				{
					if (System.IO.File.Exists(zipfile))
						System.IO.File.Delete(zipfile);

					client.DownloadFile(url, zipfile);
					System.IO.Compression.ZipStorer zs = System.IO.Compression.ZipStorer.Open(zipfile, System.IO.FileAccess.Read);

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
					done = true;
				}
			}
			catch (Exception ex)
			{
				
			}
			finally
			{
				try
				{
					if (System.IO.File.Exists(zipfile))
						System.IO.File.Delete(zipfile);
				}
				catch { }
			}

			if (UpdateResult != null)
				UpdateResult(done);
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
