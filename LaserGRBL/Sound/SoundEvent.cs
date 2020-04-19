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
                    player = new SoundPlayer(@"Sound\success.wav");
                    player.Play();
                    break;
                case 1:
                    player = new SoundPlayer(@"Sound\non-fatal.wav");
                    player.Play();
                    break;
                case 2:
                    player = new SoundPlayer(@"Sound\fatal.wav");
                    player.Play();
                    break;
                case 3:
                    player = new SoundPlayer(@"Sound\connect.wav");
                    player.Play();
                    break;
                case 4:
                    player = new SoundPlayer(@"Sound\disconnect.wav");
                    player.Play();
                    break;
                default:
                    break;
            }
        }
    }
}
