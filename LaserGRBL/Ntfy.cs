using System;
using System.Net;
using System.Text;

namespace LaserGRBL
{
    public class Ntfy
    {
        private const string NTFY_SERVER = "https://ntfy.sh";

        public static bool Enabled => Settings.GetObject("Ntfy.Enabled", false);
        public static int Threshold => Settings.GetObject("Ntfy.Threshold", 1);
        public static bool SecondPassEnabled => Settings.GetObject("Ntfy.SecondPass", false);

        /// <summary>
        /// Send a job completion notification if enabled and threshold is met
        /// </summary>
        public static void NotifyJobComplete(TimeSpan jobTime, string message)
        {
            if (!Enabled || jobTime.TotalMinutes < Threshold)
                return;

            NotifyEvent(message);
        }

        /// <summary>
        /// Send a job error/issue notification (always sent if enabled, ignores threshold)
        /// </summary>
        public static void NotifyJobError(string message)
        {
            if (!Enabled)
                return;

            NotifyEvent(message);
        }

        /// <summary>
        /// Send a second pass notification if enabled, second pass notifications are enabled, and threshold is met
        /// </summary>
        public static void NotifySecondPass(TimeSpan projectedTotalTime, string message)
        {
            if (!Enabled || !SecondPassEnabled || projectedTotalTime.TotalMinutes < Threshold)
                return;

            string topic = (string)Settings.GetObject("Ntfy.Topic", string.Empty);
            if (!string.IsNullOrWhiteSpace(topic))
            {
                NotifyEvent(topic, message, "LaserGRBL Pass 2", "default", "arrows_counterclockwise,hourglass");
            }
        }

        public static void NotifyEvent(string message)
        {
            string topic = (string)Settings.GetObject("Ntfy.Topic", string.Empty);
            if (string.IsNullOrWhiteSpace(topic))
                return;

            NotifyEvent(topic, message);
        }

        public static void NotifyEvent(string topic, string message)
        {
            if (string.IsNullOrWhiteSpace(topic))
                return;

            bool isError = message.Contains("Job Issue") || message.Contains("Issue") || message.Contains("Alarm") || message.Contains("Error");

            var data = new NtfyData
            {
                Topic = topic.Trim(),
                Message = message,
                Title = isError ? "⚠️ LaserGRBL Job FAILED" : "✅ LaserGRBL Job Complete",
                Priority = isError ? "urgent" : "high",
                Tags = isError ? "warning,rotating_light,x" : "white_check_mark,fire"
            };

            System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(InternalNotifyEvent), data);
        }

        public static void NotifyEvent(string topic, string message, string title, string priority, string tags)
        {
            if (string.IsNullOrWhiteSpace(topic))
                return;

            var data = new NtfyData
            {
                Topic = topic.Trim(),
                Message = message,
                Title = title,
                Priority = priority,
                Tags = tags
            };

            System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(InternalNotifyEvent), data);
        }

        private static void InternalNotifyEvent(object data)
        {
            try
            {
                NtfyData ntfyData = data as NtfyData;
                using (MyWebClient client = new MyWebClient())
                {
                    client.Headers["Title"] = ntfyData.Title;
                    client.Headers["Priority"] = ntfyData.Priority;
                    client.Headers["Tags"] = ntfyData.Tags;

                    string url = $"{NTFY_SERVER}/{ntfyData.Topic}";
                    byte[] response = client.UploadData(url, "POST", Encoding.UTF8.GetBytes(ntfyData.Message));
                }
            }
            catch (Exception ex)
            {
                // Silently fail - notification is not critical
                Logger.LogMessage("Ntfy", "Notification failed: {0}", ex.Message);
            }
        }

        private class NtfyData
        {
            public string Topic { get; set; }
            public string Message { get; set; }
            public string Title { get; set; }
            public string Priority { get; set; }
            public string Tags { get; set; }
        }

        private class MyWebClient : WebClient
        {
            protected override WebRequest GetWebRequest(Uri uri)
            {
                WebRequest w = base.GetWebRequest(uri);
                w.Timeout = 5000; // milliseconds
                return w;
            }
        }
    }
}
