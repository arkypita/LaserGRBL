using System;
using System.IO;

namespace LaserGRBL.Tests
{
	/// <summary>
	/// Keeps tests away from the user's real LaserGRBL data folder.
	/// </summary>
	/// <remarks>
	/// The first use of <see cref="GrblCore"/> or <see cref="Settings"/> (for example by the SVG converter)
	/// resolves <see cref="GrblCore.DataPath"/> (%AppData%\LaserGRBL) and reads, and may rewrite,
	/// <c>LaserGRBL.Settings.bin</c> there. Call <see cref="Isolate"/> before any such code.
	/// </remarks>
	internal static class TestAppData
	{
		/// <summary>Environment variable read by <see cref="GrblCore.DataPath"/>.</summary>
		public const string Variable = "LASERGRBL_DATA_PATH";

		private static readonly object Sync = new object();
		private static string mPath;

		/// <summary>
		/// Points <see cref="GrblCore.DataPath"/> to a temporary folder. Safe to call many times.
		/// </summary>
		/// <exception cref="InvalidOperationException">
		/// <see cref="GrblCore.DataPath"/> was resolved before this call, so the real user folder may already be in use.
		/// </exception>
		public static void Isolate()
		{
			lock (Sync)
			{
				if (mPath != null)
					return;

				string path = Path.Combine(Path.GetTempPath(), "LaserGRBL.Tests", Guid.NewGuid().ToString("N"));
				Directory.CreateDirectory(path);
				Environment.SetEnvironmentVariable(Variable, path);

				string resolved = GrblCore.DataPath;
				if (!string.Equals(resolved, path, StringComparison.OrdinalIgnoreCase))
					throw new InvalidOperationException("GrblCore.DataPath was already resolved to '" + resolved + "' before the tests isolated it.");

				mPath = path;
			}
		}
	}
}
