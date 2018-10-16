using System;
using System.Diagnostics;

namespace Tools
{
	public class CommandLine
	{
		public static bool ExecuteCommand(string exeDir, string args)
		{
			try
			{
				ProcessStartInfo procStartInfo = new ProcessStartInfo();

				procStartInfo.FileName = exeDir;
				procStartInfo.Arguments = args;
				procStartInfo.RedirectStandardOutput = true;
				procStartInfo.UseShellExecute = false;
				procStartInfo.CreateNoWindow = true;

				using (Process process = new Process())
				{
					process.StartInfo = procStartInfo;
					process.Start();

					process.WaitForExit();

					string result = process.StandardOutput.ReadToEnd();
					Console.WriteLine(result);
				}
				return true;
			}
			catch (Exception ex)
			{
				Console.WriteLine("*** Error occured executing the following commands.");
				Console.WriteLine(exeDir);
				Console.WriteLine(args);
				Console.WriteLine(ex.Message);
				return false;
			}
		}

		//public static bool ExecuteCommand(string exeDir, string args)
		//{
		//	try
		//	{
		//		ProcessStartInfo procStartInfo = new ProcessStartInfo();

		//		procStartInfo.FileName = exeDir;
		//		procStartInfo.Arguments = args;
		//		procStartInfo.RedirectStandardOutput = true;
		//		procStartInfo.RedirectStandardError = true;
		//		procStartInfo.UseShellExecute = false;
		//		procStartInfo.CreateNoWindow = true;

		//		using (Process process = new Process())
		//		{
		//			process.StartInfo = procStartInfo;
		//			process.Start();
		//			process.WaitForExit();

		//			Console.WriteLine(process.StandardOutput.ReadToEnd());
		//			Console.WriteLine(process.StandardError.ReadToEnd());
		//		}
		//		return true;
		//	}
		//	catch (Exception ex)
		//	{
		//		//Console.WriteLine("*** Error occured executing the following commands.");
		//		//Console.WriteLine(exeDir);
		//		//Console.WriteLine(args);
		//		//Console.WriteLine(ex.Message);
		//		return false;
		//	}
		//}
	}
}
