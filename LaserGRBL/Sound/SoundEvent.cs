using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Media;
using System.Windows.Forms;
using System.IO;

namespace Sound
{
	public class SoundEvent
	{
		/*  EVENT IDs
         *  0       Job Finished        https://freesound.org/people/grunz/sounds/109662/
         *  1       Non-fatal error     https://freesound.org/people/kwahmah_02/sounds/254174/
         *  2       Fatal error         https://freesound.org/people/fisch12345/sounds/325113/
         *  3       Connected           https://freesound.org/people/Timbre/sounds/232210/
         *  4       Disconnected        https://freesound.org/people/Timbre/sounds/232210/
         *  
         */

		public enum EventId
		{ Success = 0, Warning = 1, Fatal = 2, Connect = 3, Disconnect = 4 }


		private static bool mBusy = false;
		private static string mLock = "PLAY LOCK TOKEN";
		public static void PlaySound(EventId id)
		{
			try
			{
				string name = id.ToString();
				if (LaserGRBL.Settings.GetObject($"Sound.{name}.Enabled", true))
				{
					string filename = LaserGRBL.Settings.GetObject($"Sound.{name}", $"Sound\\{name}.wav");
					if (System.IO.File.Exists(filename))
					{
						lock (mLock)
						{
							if (!mBusy)
							{
								mBusy = true;
								System.Threading.ThreadPool.QueueUserWorkItem(PlayAsync, filename);
							}
						}
					}
				}
			}
			catch { }
		}

		private static void PlayAsync(object filename)
		{
			try
			{
				Uri uri = new Uri(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), $"{filename}"));
                SoundPlayer player = new SoundPlayer(uri.AbsoluteUri);
				player.PlaySync();
				player.Dispose();
			}
			catch {
			}
			mBusy = false;
		}
	}


}
