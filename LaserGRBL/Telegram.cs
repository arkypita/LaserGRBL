using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;

namespace LaserGRBL
{
	public class Telegram
	{

		public static void NotifyEvent(string message)
		{
			if (Settings.GetObject("TelegramNotification.Enabled", false))
				NotifyEvent(Tools.Protector.Decrypt(Settings.GetObject("TelegramNotification.Code", ""), ""), message);
		}
		public static void NotifyEvent(string usercode, string message)
		{
			if (UrlManager.TelegramHandler is null)
				return;
			if (UrlManager.TelegramServiceKey is null)
				return;
			if (string.IsNullOrEmpty(usercode) || usercode.Trim().Length != 10)
				return;

			usercode = usercode.Trim();

			NameValueCollection postData = new NameValueCollection()
					{
						{ "servicekey", UrlManager.TelegramServiceKey },
						{ "id", usercode },
						{ "message", message },
						{ "guid", UsageStats.GetID() },
					};

			System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(InternalNotifyEvent), postData);
			
		}

		private static void InternalNotifyEvent(object data)
		{
			try
			{
				NameValueCollection postData = data as NameValueCollection;
				using (MyWebClient client = new MyWebClient())
				{


					// client.UploadValues returns page's source as byte array (byte[]) so it must be transformed into a string
					string json = System.Text.Encoding.UTF8.GetString(client.UploadValues(UrlManager.TelegramHandler, postData));

					//UsageStatsRV RV = Tools.JSONParser.FromJson<UsageStatsRV>(json);
					//mManager.SetMessages(RV.Messages);

					//return (RV.Success);
				}
			}
			catch (Exception ex) { }
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
