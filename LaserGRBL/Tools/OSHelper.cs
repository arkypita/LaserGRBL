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

		public static Byte SetBit(Byte value, int position)
		{
			if (position < 0 || position > 7)
				throw new ArgumentOutOfRangeException("position", "position must be in the range 0 - 7");

			return (Byte)(value | (1 << position));
		}


		public static class OSVersionInfo
		{
			#region ENUMS
			public enum SoftwareArchitecture
			{
				Unknown = 0,
				Bit32 = 1,
				Bit64 = 2
			}

			public enum ProcessorArchitecture
			{
				Unknown = 0,
				Bit32 = 1,
				Bit64 = 2,
				Itanium64 = 3
			}
			#endregion ENUMS

			#region DELEGATE DECLARATION
			private delegate bool IsWow64ProcessDelegate([In] IntPtr handle, [Out] out bool isWow64Process);
			#endregion DELEGATE DECLARATION

			#region BITS
			/// <summary>
			/// Determines if the current application is 32 or 64-bit.
			/// </summary>
			static public SoftwareArchitecture ProgramBits
			{
				get
				{
					SoftwareArchitecture pbits = SoftwareArchitecture.Unknown;

					try
					{

						switch (IntPtr.Size * 8)
						{
							case 64:
								pbits = SoftwareArchitecture.Bit64;
								break;

							case 32:
								pbits = SoftwareArchitecture.Bit32;
								break;

							default:
								pbits = SoftwareArchitecture.Unknown;
								break;
						}
					}
					catch { }
					return pbits;
				}
			}

			static public SoftwareArchitecture OSBits
			{
				get
				{
					SoftwareArchitecture osbits = SoftwareArchitecture.Unknown;
					try
					{
						switch (IntPtr.Size * 8)
						{
							case 64:
								osbits = SoftwareArchitecture.Bit64;
								break;

							case 32:
								if (Is32BitProcessOn64BitProcessor())
									osbits = SoftwareArchitecture.Bit64;
								else
									osbits = SoftwareArchitecture.Bit32;
								break;

							default:
								osbits = SoftwareArchitecture.Unknown;
								break;
						}
					}
					catch { }

					return osbits;
				}
			}

			/// <summary>
			/// Determines if the current processor is 32 or 64-bit.
			/// </summary>
			static public ProcessorArchitecture ProcessorBits
			{
				get
				{
					ProcessorArchitecture pbits = ProcessorArchitecture.Unknown;

					try
					{
						SYSTEM_INFO l_System_Info = new SYSTEM_INFO();
						GetNativeSystemInfo(ref l_System_Info);

						switch (l_System_Info.uProcessorInfo.wProcessorArchitecture)
						{
							case 9: // PROCESSOR_ARCHITECTURE_AMD64
								pbits = ProcessorArchitecture.Bit64;
								break;
							case 6: // PROCESSOR_ARCHITECTURE_IA64
								pbits = ProcessorArchitecture.Itanium64;
								break;
							case 0: // PROCESSOR_ARCHITECTURE_INTEL
								pbits = ProcessorArchitecture.Bit32;
								break;
							default: // PROCESSOR_ARCHITECTURE_UNKNOWN
								pbits = ProcessorArchitecture.Unknown;
								break;
						}
					}
					catch { }
					return pbits;
				}
			}
			#endregion BITS

			#region EDITION
			static private string s_Edition;
			/// <summary>
			/// Gets the edition of the operating system running on this computer.
			/// </summary>
			static public string Edition
			{
				get
				{
					if (s_Edition != null)
						return s_Edition;  //***** RETURN *****//

					try
					{

						string edition = String.Empty;

						OperatingSystem osVersion = Environment.OSVersion;
						OSVERSIONINFOEX osVersionInfo = new OSVERSIONINFOEX();
						osVersionInfo.dwOSVersionInfoSize = Marshal.SizeOf(typeof(OSVERSIONINFOEX));

						if (GetVersionEx(ref osVersionInfo))
						{
							int majorVersion = osVersion.Version.Major;
							int minorVersion = osVersion.Version.Minor;
							byte productType = osVersionInfo.wProductType;
							short suiteMask = osVersionInfo.wSuiteMask;

							#region VERSION 4
							if (majorVersion == 4)
							{
								if (productType == VER_NT_WORKSTATION)
								{
									// Windows NT 4.0 Workstation
									edition = "Workstation";
								}
								else if (productType == VER_NT_SERVER)
								{
									if ((suiteMask & VER_SUITE_ENTERPRISE) != 0)
									{
										// Windows NT 4.0 Server Enterprise
										edition = "Enterprise Server";
									}
									else
									{
										// Windows NT 4.0 Server
										edition = "Standard Server";
									}
								}
							}
							#endregion VERSION 4

							#region VERSION 5
							else if (majorVersion == 5)
							{
								if (productType == VER_NT_WORKSTATION)
								{
									if ((suiteMask & VER_SUITE_PERSONAL) != 0)
									{
										edition = "Home";
									}
									else
									{
										if (GetSystemMetrics(86) == 0) // 86 == SM_TABLETPC
											edition = "Professional";
										else
											edition = "Tablet Edition";
									}
								}
								else if (productType == VER_NT_SERVER)
								{
									if (minorVersion == 0)
									{
										if ((suiteMask & VER_SUITE_DATACENTER) != 0)
										{
											// Windows 2000 Datacenter Server
											edition = "Datacenter Server";
										}
										else if ((suiteMask & VER_SUITE_ENTERPRISE) != 0)
										{
											// Windows 2000 Advanced Server
											edition = "Advanced Server";
										}
										else
										{
											// Windows 2000 Server
											edition = "Server";
										}
									}
									else
									{
										if ((suiteMask & VER_SUITE_DATACENTER) != 0)
										{
											// Windows Server 2003 Datacenter Edition
											edition = "Datacenter";
										}
										else if ((suiteMask & VER_SUITE_ENTERPRISE) != 0)
										{
											// Windows Server 2003 Enterprise Edition
											edition = "Enterprise";
										}
										else if ((suiteMask & VER_SUITE_BLADE) != 0)
										{
											// Windows Server 2003 Web Edition
											edition = "Web Edition";
										}
										else
										{
											// Windows Server 2003 Standard Edition
											edition = "Standard";
										}
									}
								}
							}
							#endregion VERSION 5

							#region VERSION 6
							else if (majorVersion == 6)
							{
								int ed;
								if (GetProductInfo(majorVersion, minorVersion, osVersionInfo.wServicePackMajor, osVersionInfo.wServicePackMinor, out ed))
								{
									if (pdic.ContainsKey(ed))
										edition = pdic[ed];
									else
										edition = ed.ToString();
								}
							}
							#endregion VERSION 6
						}

						s_Edition = edition;
					}
					catch { }
					return s_Edition;
				}
			}
			#endregion EDITION

			#region NAME
			static private string s_Name;
			/// <summary>
			/// Gets the name of the operating system running on this computer.
			/// </summary>
			static public string Name
			{
				get
				{
					if (s_Name != null)
						return s_Name;  //***** RETURN *****//
					try
					{
						string name = "unknown";

						OperatingSystem osVersion = Environment.OSVersion;
						OSVERSIONINFOEX osVersionInfo = new OSVERSIONINFOEX();
						osVersionInfo.dwOSVersionInfoSize = Marshal.SizeOf(typeof(OSVERSIONINFOEX));

						if (GetVersionEx(ref osVersionInfo))
						{
							int majorVersion = osVersion.Version.Major;
							int minorVersion = osVersion.Version.Minor;

							if (majorVersion == 6 && minorVersion == 2)
							{
								//The registry read workaround is by Scott Vickery. Thanks a lot for the help!

								//http://msdn.microsoft.com/en-us/library/windows/desktop/ms724832(v=vs.85).aspx

								// For applications that have been manifested for Windows 8.1 & Windows 10. Applications not manifested for 8.1 or 10 will return the Windows 8 OS version value (6.2). 
								// By reading the registry, we'll get the exact version - meaning we can even compare against  Win 8 and Win 8.1.
								string exactVersion = RegistryRead(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion", "CurrentVersion", "");
								if (!string.IsNullOrEmpty(exactVersion))
								{
									string[] splitResult = exactVersion.Split('.');
									majorVersion = Convert.ToInt32(splitResult[0]);
									minorVersion = Convert.ToInt32(splitResult[1]);
								}
								if (IsWindows10())
								{
									majorVersion = 10;
									minorVersion = 0;
								}
							}

							switch (osVersion.Platform)
							{
								case PlatformID.Win32S:
									name = "Win 3.1";
									break;
								case PlatformID.WinCE:
									name = "Win CE";
									break;
								case PlatformID.Win32Windows:
									{
										if (majorVersion == 4)
										{
											string csdVersion = osVersionInfo.szCSDVersion;
											switch (minorVersion)
											{
												case 0:
													if (csdVersion == "B" || csdVersion == "C")
														name = "Win 95 OSR2";
													else
														name = "Win 95";
													break;
												case 10:
													if (csdVersion == "A")
														name = "Win 98 SE";
													else
														name = "Win 98";
													break;
												case 90:
													name = "Win Me";
													break;
											}
										}
										break;
									}
								case PlatformID.Win32NT:
									{
										byte productType = osVersionInfo.wProductType;

										switch (majorVersion)
										{
											case 3:
												name = "Win NT 3.51";
												break;
											case 4:
												switch (productType)
												{
													case 1:
														name = "Win NT 4.0";
														break;
													case 3:
														name = "Win NT 4.0 Server";
														break;
												}
												break;
											case 5:
												switch (minorVersion)
												{
													case 0:
														name = "Win 2000";
														break;
													case 1:
														name = "Win XP";
														break;
													case 2:
														name = "Win Server 2003";
														break;
												}
												break;
											case 6:
												switch (minorVersion)
												{
													case 0:
														switch (productType)
														{
															case 1:
																name = "Win Vista";
																break;
															case 3:
																name = "Win Server 2008";
																break;
														}
														break;

													case 1:
														switch (productType)
														{
															case 1:
																name = "Win 7";
																break;
															case 3:
																name = "Win Server 2008 R2";
																break;
														}
														break;
													case 2:
														switch (productType)
														{
															case 1:
																name = "Win 8";
																break;
															case 3:
																name = "Win Server 2012";
																break;
														}
														break;
													case 3:
														switch (productType)
														{
															case 1:
																name = "Win 8.1";
																break;
															case 3:
																name = "Win Server 2012 R2";
																break;
														}
														break;
												}
												break;
											case 10:
												switch (minorVersion)
												{
													case 0:
														switch (productType)
														{
															case 1:
																{
																	if (IsWindows11())
																		name = "Win 11";
																	else
																		name = "Win 10";
																}
																break;
															case 3:
																name = "Win Server 2016";
																break;
														}
														break;
												}
												break;
										}
										break;
									}
							}
						}

						s_Name = name;
					}
					catch { }
					return s_Name;
				}
			}

			public static bool IsWindows11()
			{
				try
				{
					string currentBuildStr = RegistryRead(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion", "CurrentBuild", "");
					int currentBuild = int.Parse(currentBuildStr);

					return currentBuild >= 22000;
				}
				catch { return false; }
			}

			#endregion NAME

			#region PINVOKE

			#region GET
			#region PRODUCT INFO
			[DllImport("Kernel32.dll")]
			internal static extern bool GetProductInfo(
				int osMajorVersion,
				int osMinorVersion,
				int spMajorVersion,
				int spMinorVersion,
				out int edition);
			#endregion PRODUCT INFO

			#region VERSION
			[DllImport("kernel32.dll")]
			private static extern bool GetVersionEx(ref OSVERSIONINFOEX osVersionInfo);
			#endregion VERSION

			#region SYSTEMMETRICS
			[DllImport("user32")]
			public static extern int GetSystemMetrics(int nIndex);
			#endregion SYSTEMMETRICS

			#region SYSTEMINFO
			[DllImport("kernel32.dll")]
			public static extern void GetSystemInfo([MarshalAs(UnmanagedType.Struct)] ref SYSTEM_INFO lpSystemInfo);

			[DllImport("kernel32.dll")]
			public static extern void GetNativeSystemInfo([MarshalAs(UnmanagedType.Struct)] ref SYSTEM_INFO lpSystemInfo);
			#endregion SYSTEMINFO

			#endregion GET

			#region OSVERSIONINFOEX
			[StructLayout(LayoutKind.Sequential)]
			private struct OSVERSIONINFOEX
			{
				public int dwOSVersionInfoSize;
				public int dwMajorVersion;
				public int dwMinorVersion;
				public int dwBuildNumber;
				public int dwPlatformId;
				[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
				public string szCSDVersion;
				public short wServicePackMajor;
				public short wServicePackMinor;
				public short wSuiteMask;
				public byte wProductType;
				public byte wReserved;
			}
			#endregion OSVERSIONINFOEX

			#region SYSTEM_INFO
			[StructLayout(LayoutKind.Sequential)]
			public struct SYSTEM_INFO
			{
				internal _PROCESSOR_INFO_UNION uProcessorInfo;
				public uint dwPageSize;
				public IntPtr lpMinimumApplicationAddress;
				public IntPtr lpMaximumApplicationAddress;
				public IntPtr dwActiveProcessorMask;
				public uint dwNumberOfProcessors;
				public uint dwProcessorType;
				public uint dwAllocationGranularity;
				public ushort dwProcessorLevel;
				public ushort dwProcessorRevision;
			}
			#endregion SYSTEM_INFO

			#region _PROCESSOR_INFO_UNION
			[StructLayout(LayoutKind.Explicit)]
			public struct _PROCESSOR_INFO_UNION
			{
				[FieldOffset(0)]
				internal uint dwOemId;
				[FieldOffset(0)]
				internal ushort wProcessorArchitecture;
				[FieldOffset(2)]
				internal ushort wReserved;
			}
			#endregion _PROCESSOR_INFO_UNION

			#region 64 BIT OS DETECTION
			[DllImport("kernel32", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
			public extern static IntPtr LoadLibrary(string libraryName);

			[DllImport("kernel32", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
			public extern static IntPtr GetProcAddress(IntPtr hwnd, string procedureName);
			#endregion 64 BIT OS DETECTION

			#region PRODUCT

			private static Dictionary<int, string> pdic = new Dictionary<int, string>
			{
				{0x00000006,"Business"},
				{0x00000010,"Business"},
				{0x00000012,"HPC Edition"},
				{0x00000040,"Server Hyper Core V"},
				{0x00000065,"Home"},
				{0x00000063,"Home"},
				{0x00000062,"Home"},
				{0x00000064,"Home"},
				{0x00000050,"Server Datacenter"},
				{0x00000091,"Server Datacenter"},
				{0x00000092,"Server Standard"},
				{0x00000008,"Server Datacenter"},
				{0x0000000C,"Server Datacenter"},
				{0x00000027,"Server Datacenter"},
				{0x00000025,"Server Datacenter"},
				{0x00000079,"Education"},
				{0x0000007A,"Education"},
				{0x00000004,"Enterprise"},
				{0x00000046,"Enterprise"},
				{0x00000048,"Enterprise"},
				{0x0000001B,"Enterprise"},
				{0x00000054,"Enterprise"},
				{0x0000007D,"Enterprise"},
				{0x00000081,"Enterprise"},
				{0x0000007E,"Enterprise"},
				{0x00000082,"Enterprise"},
				{0x0000000A,"Server Enterprise"},
				{0x0000000E,"Server Enterprise"},
				{0x00000029,"Server Enterprise"},
				{0x0000000F,"Server Enterprise"},
				{0x00000026,"Server Enterprise"},
				{0x0000003C,"Essential Server Solution"},
				{0x0000003E,"Essential Server Solution"},
				{0x0000003B,"Essential Server Solution"},
				{0x0000003D,"Essential Server Solution"},
				{0x00000002,"Home Basic"},
				{0x00000043,"Home Basic"},
				{0x00000005,"Home Basic"},
				{0x00000003,"Home Premium"},
				{0x00000044,"Home Premium"},
				{0x0000001A,"Home Premium"},
				{0x00000022,"Home Server"},
				{0x00000013,"Storage Server"},
				{0x0000002A,"Hyper-V Server"},
				{0x0000007B,"IoT"},
				{0x00000083,"IoT"},
				{0x0000001E,"Essential Business Server"},
				{0x00000020,"Essential Business Server"},
				{0x0000001F,"Essential Business Server"},
				{0x00000068,"Mobile"},
				{0x00000085,"Mobile Enterprise"},
				{0x0000004D,"MultiPoint Server Premium"},
				{0x0000004C,"MultiPoint Server Standard"},
				{0x000000A1,"Workstations"},
				{0x000000A2,"Workstations"},
				{0x00000030,"Professional"},
				{0x00000045,"Professional"},
				{0x00000031,"Professional"},
				{0x00000067,"Professional with Media Center"},
				{0x00000032,"Small Business Server"},
				{0x00000036,"Server For SB Solutions"},
				{0x00000033,"Server For SB Solutions"},
				{0x00000037,"Server For SB Solutions"},
				{0x00000018,"Essential Server Solutions"},
				{0x00000023,"Essential Server Solutions"},
				{0x00000021,"Server Foundation"},
				{0x00000009,"Small Business Server"},
				{0x00000019,"Small Business Server Premium"},
				{0x0000003F,"Small Business Server Premium"},
				{0x00000038,"Windows MultiPoint Server"},
				{0x0000004F,"Server Standard"},
				{0x00000007,"Server Standard"},
				{0x0000000D,"Server Standard"},
				{0x00000028,"Server Standard"},
				{0x00000024,"Server Standard"},
				{0x00000034,"Server Solutions Premium"},
				{0x00000035,"Server Solutions Premium"},
				{0x0000000B,"Starter"},
				{0x00000042,"Starter"},
				{0x0000002F,"Starter"},
				{0x00000017,"Storage Server Enterprise"},
				{0x0000002E,"Storage Server Enterprise"},
				{0x00000014,"Storage Server Express"},
				{0x0000002B,"Storage Server Express"},
				{0x00000060,"Storage Server Standard"},
				{0x00000015,"Storage Server Standard"},
				{0x0000002C,"Storage Server Standard"},
				{0x0000005F,"Storage Server Workgroup"},
				{0x00000016,"Storage Server Workgroup"},
				{0x0000002D,"Storage Server Workgroup"},
				{0x00000001,"Ultimate"},
				{0x00000047,"Ultimate"},
				{0x0000001C,"Ultimate"},
				{0x00000000,"An unknown product"},
				{0x00000011,"Web Server"},
				{0x0000001D,"Web Server"},

			};

			#endregion PRODUCT

			#region VERSIONS
			private const int VER_NT_WORKSTATION = 1;
			private const int VER_NT_DOMAIN_CONTROLLER = 2;
			private const int VER_NT_SERVER = 3;
			private const int VER_SUITE_SMALLBUSINESS = 1;
			private const int VER_SUITE_ENTERPRISE = 2;
			private const int VER_SUITE_TERMINAL = 16;
			private const int VER_SUITE_DATACENTER = 128;
			private const int VER_SUITE_SINGLEUSERTS = 256;
			private const int VER_SUITE_PERSONAL = 512;
			private const int VER_SUITE_BLADE = 1024;
			#endregion VERSIONS

			#endregion PINVOKE

			#region SERVICE PACK
			/// <summary>
			/// Gets the service pack information of the operating system running on this computer.
			/// </summary>
			static public string ServicePack
			{
				get
				{
					try
					{
						string servicePack = String.Empty;
						OSVERSIONINFOEX osVersionInfo = new OSVERSIONINFOEX();

						osVersionInfo.dwOSVersionInfoSize = Marshal.SizeOf(typeof(OSVERSIONINFOEX));

						if (GetVersionEx(ref osVersionInfo))
						{
							servicePack = osVersionInfo.szCSDVersion;
						}

						return servicePack;
					}
					catch { return ""; }
				}
			}
			#endregion SERVICE PACK

			#region VERSION
			#region BUILD
			/// <summary>
			/// Gets the build version number of the operating system running on this computer.
			/// </summary>
			static public int BuildVersion
			{
				get
				{
					return int.Parse(RegistryRead(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion", "CurrentBuildNumber", "0"));
				}
			}
			#endregion BUILD

			#region FULL
			#region STRING
			/// <summary>
			/// Gets the full version string of the operating system running on this computer.
			/// </summary>
			static public string VersionString
			{
				get
				{
					try
					{
						return Version.ToString();
					}
					catch { return ""; }
				}
			}
			#endregion STRING

			#region VERSION
			/// <summary>
			/// Gets the full version of the operating system running on this computer.
			/// </summary>
			static public Version Version
			{
				get
				{
					return new Version(MajorVersion, MinorVersion, BuildVersion, RevisionVersion);
				}
			}
			#endregion VERSION
			#endregion FULL

			#region MAJOR
			/// <summary>
			/// Gets the major version number of the operating system running on this computer.
			/// </summary>
			static public int MajorVersion
			{
				get
				{
					if (IsWindows10())
					{
						return 10;
					}
					string exactVersion = RegistryRead(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion", "CurrentVersion", "");
					if (!string.IsNullOrEmpty(exactVersion))
					{
						string[] splitVersion = exactVersion.Split('.');
						return int.Parse(splitVersion[0]);
					}
					return Environment.OSVersion.Version.Major;
				}
			}
			#endregion MAJOR

			#region MINOR
			/// <summary>
			/// Gets the minor version number of the operating system running on this computer.
			/// </summary>
			static public int MinorVersion
			{
				get
				{
					if (IsWindows10())
					{
						return 0;
					}
					string exactVersion = RegistryRead(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion", "CurrentVersion", "");
					if (!string.IsNullOrEmpty(exactVersion))
					{
						string[] splitVersion = exactVersion.Split('.');
						return int.Parse(splitVersion[1]);
					}
					return Environment.OSVersion.Version.Minor;
				}
			}
			#endregion MINOR

			#region REVISION
			/// <summary>
			/// Gets the revision version number of the operating system running on this computer.
			/// </summary>
			static public int RevisionVersion
			{
				get
				{
					if (IsWindows10())
					{
						return 0;
					}
					return Environment.OSVersion.Version.Revision;
				}
			}
			#endregion REVISION
			#endregion VERSION

			#region 64 BIT OS DETECTION
			private static IsWow64ProcessDelegate GetIsWow64ProcessDelegate()
			{
				IntPtr handle = LoadLibrary("kernel32");

				if (handle != IntPtr.Zero)
				{
					IntPtr fnPtr = GetProcAddress(handle, "IsWow64Process");

					if (fnPtr != IntPtr.Zero)
					{
						return (IsWow64ProcessDelegate)Marshal.GetDelegateForFunctionPointer((IntPtr)fnPtr, typeof(IsWow64ProcessDelegate));
					}
				}

				return null;
			}

			private static bool Is32BitProcessOn64BitProcessor()
			{
				IsWow64ProcessDelegate fnDelegate = GetIsWow64ProcessDelegate();

				if (fnDelegate == null)
				{
					return false;
				}

				bool isWow64;
				bool retVal = fnDelegate.Invoke(Process.GetCurrentProcess().Handle, out isWow64);

				if (retVal == false)
				{
					return false;
				}

				return isWow64;
			}
			#endregion 64 BIT OS DETECTION

			#region Windows 10 Detection

			private static bool IsWindows10()
			{
				string productName = RegistryRead(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion", "ProductName", "");
				if (productName.StartsWith("Windows 10", StringComparison.OrdinalIgnoreCase))
				{
					return true;
				}
				return false;
			}

			#endregion

			#region Registry Methods

			private static string RegistryRead(string RegistryPath, string Field, string DefaultValue)
			{
				string rtn = "";
				string backSlash = "";
				string newRegistryPath = "";

				try
				{
					RegistryKey OurKey = null;
					string[] split_result = RegistryPath.Split('\\');

					if (split_result.Length > 0)
					{
						split_result[0] = split_result[0].ToUpper();        // Make the first entry uppercase...

						if (split_result[0] == "HKEY_CLASSES_ROOT") OurKey = Registry.ClassesRoot;
						else if (split_result[0] == "HKEY_CURRENT_USER") OurKey = Registry.CurrentUser;
						else if (split_result[0] == "HKEY_LOCAL_MACHINE") OurKey = Registry.LocalMachine;
						else if (split_result[0] == "HKEY_USERS") OurKey = Registry.Users;
						else if (split_result[0] == "HKEY_CURRENT_CONFIG") OurKey = Registry.CurrentConfig;

						if (OurKey != null)
						{
							for (int i = 1; i < split_result.Length; i++)
							{
								newRegistryPath += backSlash + split_result[i];
								backSlash = "\\";
							}

							if (newRegistryPath != "")
							{
								//rtn = (string)Registry.GetValue(RegistryPath, "CurrentVersion", DefaultValue);

								OurKey = OurKey.OpenSubKey(newRegistryPath);
								rtn = (string)OurKey.GetValue(Field, DefaultValue);
								OurKey.Close();
							}
						}
					}
				}
				catch { }

				return rtn;
			}

			#endregion Registry Methods
		}

	}
}
