//Copyright (c) 2016-2021 Diego Settimi - https://github.com/arkypita/

// This program is free software; you can redistribute it and/or modify  it under the terms of the GPLv3 General Public License as published by  the Free Software Foundation; either version 3 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GPLv3  General Public License for more details.
// You should have received a copy of the GPLv3 General Public License  along with this program; if not, write to the Free Software  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307,  USA. using System;

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

namespace LaserGRBL
{
	class GitHub
	{
		

		public class OnlineVersion
		{
			public string html_url = null;
			public string tag_name = null;
			public string name = null;
			public bool prerelease = false;
			public List<VersionAsset> assets = null;

			public class VersionAsset
			{
				public string browser_download_url = null;
				[IgnoreDataMember] public string Url => browser_download_url;
			}

			[IgnoreDataMember] public bool IsValid => Version != null && DownloadUrl != null;
			[IgnoreDataMember] public bool IsPreRelease => prerelease;

			[IgnoreDataMember] public string VersionName => tag_name;

			[IgnoreDataMember]
			public Version Version
			{
				get
				{
					try { return new Version(tag_name.TrimStart(new char[] { 'v' })); }
					catch { return null; }
				}
			}

			[IgnoreDataMember]
			public string DownloadUrl
			{
				get
				{
					return assets != null && assets.Count > 0 ? assets[0].Url : null;
				}
			}

			[IgnoreDataMember]
			public string HtmlUrl
			{
				get
				{
					return html_url;
				}
			}

			public override string ToString()
			{
				return String.Format("{0}{1}{2}", Version, IsPreRelease ? "-pre" : "", IsValid ? "" : " (not available)");
			}
		}

		public delegate void NewVersionDlg(Version current, OnlineVersion available, Exception error);
		public static event NewVersionDlg NewVersion;

		public static bool Updating = false;

		public static void CheckVersion(bool manual)
		{
			if (UrlManager.UpdateMain != null || UrlManager.UpdateMirror != null)
			{
				System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(GitHub.AsyncCheckVersion), manual);
				System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(GitHub.AsyncDownloadDB), manual);
			}
		}

		private static void AsyncCheckVersion(object foo)
		{
			Exception error = null;
			bool manual = (bool)foo;
			if (UrlManager.UpdateMain != null)
			{
				error = null;
				try { CheckVersionFromSite(UrlManager.UpdateMain, manual); } //official https 
				catch (Exception ex1)
				{
					error = ex1;
					if (UrlManager.UpdateMirror != null)
					{
						error = null;
						try { CheckVersionFromSite(UrlManager.UpdateMirror, manual); }	//http mirror
						catch (Exception ex2) { error = ex2; }
					}
				}
			}
			else if (UrlManager.UpdateMirror != null) //only mirror configured
			{
				error = null;
				try { CheckVersionFromSite(UrlManager.UpdateMirror, manual); } //http mirror
				catch (Exception ex3) { error = ex3; }
			}

			if (error != null && manual)
				NewVersion?.Invoke(null, null, error);
		}

		private static void AsyncDownloadDB(object foo)
		{
			bool manual = (bool)foo;
			if (UrlManager.UpdateMaterialDB != null)
			{
				try
				{
					DateTime curdate = DateTime.UtcNow;
					DateTime filedate = System.IO.File.Exists(PSHelper.MaterialDB.ServerFile) ? System.IO.File.GetLastWriteTimeUtc(PSHelper.MaterialDB.ServerFile) : new DateTime(2015,1,1, 0,0,0, DateTimeKind.Utc);

					if (curdate.Subtract(filedate).TotalHours > 24) //non effettuare aggiornamenti se sono passate meno di 24 ore dall'ultima verifica
					{
						using (System.Net.WebClient wc = new System.Net.WebClient())
						{
							try
							{
								wc.Headers.Add("User-Agent: .Net WebClient");
								string json = wc.DownloadString(UrlManager.UpdateMaterialDB);

								dynamic commits = Tools.JSONParser.FromJson<dynamic>(json);
								dynamic latest = commits[0];
								dynamic commit = latest["commit"];
								dynamic author = commit["author"];
								string sha = latest["sha"] as string;
								DateTime gitdate = DateTime.Parse(author["date"] as string).ToUniversalTime();

								if (gitdate > filedate) //è uscito un file più aggiornato
								{
									//download and overwrite

									using (System.Net.WebClient dc = new System.Net.WebClient())
									{
										string tmp = System.IO.Path.Combine(System.IO.Path.GetTempPath(), System.IO.Path.GetTempFileName());
										try
										{
											dc.DownloadFile(new Uri(UrlManager.DownloadMaterialDB), tmp);
											if (System.IO.File.Exists(PSHelper.MaterialDB.ServerFile)) System.IO.File.Delete(PSHelper.MaterialDB.ServerFile);
											System.IO.File.Move(tmp, PSHelper.MaterialDB.ServerFile);
											System.IO.File.SetLastWriteTimeUtc(PSHelper.MaterialDB.ServerFile, curdate);
											Logger.LogMessage("Update DB", $"Material DB Updated to {gitdate.ToShortDateString()} [{sha.Substring(0, 7)}]");
										}
										catch (Exception ex) { Logger.LogException("Update DB", ex); }
										finally
										{
											try
											{
												if (System.IO.File.Exists(tmp))
													System.IO.File.Delete(tmp);
											}
											catch (Exception ex) { Logger.LogException("Update DB", ex); }
										}
									}

								}
								else //segna che il file è aggiornato
								{
									System.IO.File.SetLastWriteTimeUtc(PSHelper.MaterialDB.ServerFile, curdate);
									Logger.LogMessage("Update DB", $"Material DB is up to date");
								}
							}
							catch (Exception ex) { Logger.LogException("Update DB", ex); }
						}
					}
				}
				catch (Exception ex) { Logger.LogException("Update DB", ex); }
			}

		}

		private static void CheckVersionFromSite(string site, bool manual)
		{
			using (System.Net.WebClient wc = new System.Net.WebClient())
			{
				wc.Headers.Add("User-Agent: .Net WebClient");
				string json = wc.DownloadString(site);

				List<OnlineVersion> versions = Tools.JSONParser.FromJson<List<OnlineVersion>>(json);

				bool build = Settings.GetObject("Auto Update Build", false) || manual;
				bool pre = Settings.GetObject("Auto Update Pre", false);

				Version current = Program.CurrentVersion;
				OnlineVersion found = null;
				foreach (OnlineVersion ov in versions) //version start with higher first
				{
					if (!ov.IsValid) //skip invalid
						continue;
					if (!pre && ov.IsPreRelease) //skip pre-release
						continue;
					if (!build && ov.Version.Build != 0) //skip build version (only update to minor number change)
						continue;
					if (found != null && ov.Version < found.Version) //already found a better match
						continue;

					if (ov.Version > current)
						found = ov;
				}

				if (found != null)
					NewVersion?.Invoke(current, found, null);
				else if (manual) //notify "no new version"
					NewVersion?.Invoke(current, null, null);
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
