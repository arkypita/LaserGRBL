using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Tools
{
    public class OSHelper
	{
		public static bool Is64BitProcess = OSVersionInfo.ProgramBits == OSVersionInfo.SoftwareArchitecture.Bit64;
		public static bool Is64BitOperatingSystem = OSVersionInfo.OSBits == OSVersionInfo.SoftwareArchitecture.Bit64;

		public static string GetClrInfo()
		{
			try
			{
				Type type = typeof(String);
				String uri = type.Assembly.CodeBase;
				FileVersionInfo info = FileVersionInfo.GetVersionInfo(new Uri(uri).LocalPath);

				return $"{info.FileName} {info.FileVersion}";
			}
			catch
			{
				return Environment.Version.ToString();
			}
		}

		public static string GetOSInfo()
		{
			string rv;

			try
			{
				if (OSVersionInfo.ServicePack != string.Empty)
					rv = $"{OSVersionInfo.Name}|{OSVersionInfo.Edition}|{OSVersionInfo.VersionString}|{OSVersionInfo.ServicePack}";
				else
					rv = $"{OSVersionInfo.Name}|{OSVersionInfo.Edition}|{OSVersionInfo.VersionString}";
			}
			catch { rv = ""; }

			return rv;
		}

		internal static byte GetBitFlag()
		{
			byte rv = 0;

			try
			{
				if (OSVersionInfo.ProgramBits == OSVersionInfo.SoftwareArchitecture.Bit64)
					rv = SetBit(rv, 0);
				if (OSVersionInfo.OSBits == OSVersionInfo.SoftwareArchitecture.Bit64)
					rv = SetBit(rv, 1);
				if (OSVersionInfo.ProcessorBits == OSVersionInfo.ProcessorArchitecture.Bit64 || OSVersionInfo.ProcessorBits == OSVersionInfo.ProcessorArchitecture.Itanium64)
					rv = SetBit(rv, 2);
			}
			catch { rv = 99; }

			return rv;
		}

		public static byte SetBit(byte value, int position)
		{
			if (position < 0 || position > 7)
				throw new ArgumentOutOfRangeException("position", "position must be in the range 0 - 7");

			return (byte)(value | (1 << position));
		}

	}
}