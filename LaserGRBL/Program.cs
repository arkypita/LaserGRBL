//Copyright (c) 2016-2021 Diego Settimi - https://github.com/arkypita/

// This program is free software; you can redistribute it and/or modify  it under the terms of the GPLv3 General Public License as published by  the Free Software Foundation; either version 3 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GPLv3  General Public License for more details.
// You should have received a copy of the GPLv3 General Public License  along with this program; if not, write to the Free Software  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307,  USA. using System;

using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;

[assembly:InternalsVisibleTo("LaserGRBL.Tests")]
namespace LaserGRBL
{
	static class Program
	{
        public static Version CurrentVersion { get; private set; }

        /// <summary>
        /// Punto di ingresso principale dell'applicazione.
        /// </summary>
        [STAThread]
		static void Main(string[] args)
		{
			try { CurrentVersion = typeof(GitHub).Assembly.GetName().Version; }
			catch { CurrentVersion = new Version(0, 0, 0); }

			ExceptionManager.RegisterHandler();
			Tools.TimingBase.TimeFromApplicationStartup();

			FixTLS();

			Logger.Start();
			GitHub.InitUpdate();
			UsageStats.LoadFile();
			CustomButtons.LoadFile();
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			System.Globalization.CultureInfo ci = Settings.GetObject<System.Globalization.CultureInfo>("User Language", null);
			if (ci != null) Thread.CurrentThread.CurrentUICulture = ci;
			Tools.TaskScheduler.SetClockResolution(1); //use a fast clock

			Application.Run(new MainForm(args));

			GrblEmulator.WebSocketEmulator.Stop();
			Autotrace.CleanupTmpFolder();

			if (ComWrapper.ComLogger.Enabled)
			{
				string message = ComWrapper.ComLogger.StopLog();
				if (message != null) Logger.LogMessage("ComLog", message);
			}

			Logger.Stop();

		}

		private static void FixTLS()
		{
			//public enum SecurityProtocolType
			//{
			//	Ssl3 = 48,
			//	Tls = 192,
			//	Tls11 = 768,
			//	Tls12 = 3072,
			//}
			try
			{
				//https://developer.github.com/changes/2018-02-01-weak-crypto-removal-notice/
				try { System.Net.ServicePointManager.SecurityProtocol = (System.Net.SecurityProtocolType)3072; } //CONFIGURE SYSTEM FOR TLS 1.2 (Required since 22-02-2018) May work only if .net 4.5 is installed?
				catch { System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls; } //fallback, but not working with new github API!
				System.Net.ServicePointManager.ServerCertificateValidationCallback += new System.Net.Security.RemoteCertificateValidationCallback(bypassAllCertificateStuff);
			}
			catch { }
		}

		private static bool bypassAllCertificateStuff(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
		{ return true; }
	}
}
