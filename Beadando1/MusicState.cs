using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Beadando1
{
    public static class MusicState
    {
        public static bool isMuted { get; set; } = false;
        public static MediaPlayer mediaPlayer { get; } = new MediaPlayer();
    }
}
