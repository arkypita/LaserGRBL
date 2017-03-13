using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace LaserGRBL
{
	class GitHub
	{

		public delegate void NewVersionDlg(Version current, Version latest, string name, string url);
		public static event NewVersionDlg NewVersion;

	//	function GetLatestReleaseInfo() {
	//	$.getJSON("https://api.github.com/repos/ShareX/ShareX/releases/latest").done(function (release) {
	//		var asset = release.assets[0];
	//		var downloadCount = 0;
	//		for (var i = 0; i < release.assets.length; i++) {
	//			downloadCount += release.assets[i].download_count;
	//		}
	//		var oneHour = 60 * 60 * 1000;
	//		var oneDay = 24 * oneHour;
	//		var dateDiff = new Date() - new Date(asset.updated_at);
	//		var timeAgo;
	//		if (dateDiff < oneDay)
	//		{
	//			timeAgo = (dateDiff / oneHour).toFixed(1) + " hours ago";
	//		}
	//		else
	//		{
	//			timeAgo = (dateDiff / oneDay).toFixed(1) + " days ago";
	//		}
	//		var releaseInfo = release.name + " was updated " + timeAgo + " and downloaded " + downloadCount.toLocaleString() + " times.";
	//		$(".sharex-download").attr("href", asset.browser_download_url);
	//		$(".release-info").text(releaseInfo);
	//		$(".release-info").fadeIn("slow");
	//	});
	//}

		public static void CheckVersion()
		{
			if ((bool)Settings.GetObject("Auto Update", true))
				System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(GitHub.AsyncCheckVersion));
		}

		public static void AsyncCheckVersion(object foo)
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

					if (current.Major < latest.Major || current.Minor < latest.Minor || current.Revision < latest.Revision)
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



	}
}
