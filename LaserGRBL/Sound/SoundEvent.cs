using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Media;

namespace Sound
{
    class SoundEvent
    {
        private SoundPlayer player;

        /*  EVENT IDs
         *  0       Job Finished        https://freesound.org/people/grunz/sounds/109662/
         *  1       Non-fatal error     https://freesound.org/people/kwahmah_02/sounds/254174/
         *  2       Fatal error         https://freesound.org/people/fisch12345/sounds/325113/
         *  3       Connected           https://freesound.org/people/Timbre/sounds/232210/
         *  4       Disconnected        https://freesound.org/people/Timbre/sounds/232210/
         *  
         */

        public void PlaySound(int eventId)
        {
            switch (eventId)
            {
                case 0:
                    if(!LaserGRBL.Settings.GetObject("Sound.Success.IsMuted", false)){
                        player = new SoundPlayer(LaserGRBL.Settings.GetObject("Sound.Success", "Sound\\success.wav")); //since it is saved as a setting, we can now read it from there
                        player.Play();
                    }
                    break;
                case 1:
                    if (!LaserGRBL.Settings.GetObject("Sound.Warning.IsMuted", false))
                    {
                        player = new SoundPlayer(LaserGRBL.Settings.GetObject("Sound.Warning", "Sound\\non-fatal.wav"));
                        player.Play();
                    }
                    break;
                case 2:
                    if (!LaserGRBL.Settings.GetObject("Sound.Fatal.IsMuted", false))
                    {
                        player = new SoundPlayer(LaserGRBL.Settings.GetObject("Sound.Fatal", "Sound\\fatal.wav"));
                        player.Play();
                    }
                    break;
                case 3:
                    if (!LaserGRBL.Settings.GetObject("Sound.Connect.IsMuted", false))
                    {
                        player = new SoundPlayer(LaserGRBL.Settings.GetObject("Sound.Connect", "Sound\\connect.wav"));
                        player.Play();
                    }
                    break;
                case 4:
                    if (!LaserGRBL.Settings.GetObject("Sound.Disconnect.IsMuted", false))
                    {
                        player = new SoundPlayer(LaserGRBL.Settings.GetObject("Sound.Disconnect", "Sound\\disconnect.wav"));
                        player.Play();
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
