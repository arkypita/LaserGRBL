using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

namespace Tools
{
	public class OSHelper
	{
		public static bool Is64BitProcess = (IntPtr.Size == 8);
		public static bool Is64BitOperatingSystem = Is64BitProcess || InternalCheckIsWow64();

		[DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool IsWow64Process([In] IntPtr hProcess, [Out] out bool wow64Process);

		private static bool InternalCheckIsWow64()
		{
			if ((Environment.OSVersion.Version.Major == 5 && Environment.OSVersion.Version.Minor >= 1) || Environment.OSVersion.Version.Major >= 6)
			{
				try
				{
					using (Process p = Process.GetCurrentProcess())
					{
						bool retVal;
						if (!IsWow64Process(p.Handle, out retVal))
							return false;
						return retVal;
					}
				}
				catch { return false; }
			}
			else
			{
				return false;
			}
		}

		public static string GetOSInfo()
		{
			try
			{
				System.Management.ManagementScope scope = new System.Management.ManagementScope(@"\\.\root\cimv2");
				scope.Connect();
				System.Management.ObjectQuery oq = new System.Management.ObjectQuery("SELECT * FROM Win32_OperatingSystem");
				using (System.Management.ManagementObjectSearcher os = new System.Management.ManagementObjectSearcher(scope, oq))
				{
					foreach (System.Management.ManagementObject mo in os.Get())
					{
						try
						{
							string a = $"{Convert.ToString(mo["Caption"])} {Convert.ToString(mo["OSArchitecture"])} {Convert.ToString(mo["Version"])}";
							a = Regex.Replace(a, "Microsoft", "", RegexOptions.IgnoreCase);
							a = Regex.Replace(a, "Windows", "Win", RegexOptions.IgnoreCase);
							a = Regex.Replace(a, " bit", "bit", RegexOptions.IgnoreCase);

							return a.Trim(); ;
						}
						catch { }
					}
				}
			}
			catch { }

			return Environment.OSVersion.ToString(); //fallback option
		}

		internal static int GetBitFlag()
		{
			if (Is64BitProcess)
				return 1;   //ok!
			if (!Is64BitOperatingSystem)
				return 0;   //ok... (the os does not support 64bit)
			else
				return 2;   //error! something wrong if os support 64bit but the app run as 32bit
		}
	}
}
