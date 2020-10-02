using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Pong
{
    class Utilities
    {
        public static byte[] StringToByteArray(string str)
        {
            return Encoding.ASCII.GetBytes(str);
        }
        public static string ByteArrayToString(byte[] arr)
        {
            return Encoding.ASCII.GetString(arr);
        }
    }

    class Constants
    {
        public const int PORT = 5000;
        public const int RACKET_HEIGHT = 230;
        public const int RACKET_WIDTH = 30;
        public const int PLAYGROUNDBOTTOM = 1080;
        public const int PLAYGROUNDTOP = 0;
        public const int PLAYGROUNDRIGHT = 1920;
        public const int PLAYGROUNDLEFT = 0;
        public const int BALLHEIGHT = 30;
        public const int BALLWIDTH = 30;
        public const int FIRSTPLAYERRACKETTOP = 425;
        public const int FIRSTPLAYERRACKETLEFT = 1850;
        public const int FIRSTPLAYERRACKETRIGHT = 1880;
        public const int SECONDPLAYERRACKETLEFT = 40;
        public const int SECONDPLAYERRACKETRIGHT = 70;
        public const int SECONDPLAYERRACKETTOP = 425;
        public const int BALLTOP = 525;
        public const int BALLLEFT = 945;
        public const int SLEEPTIME = 7;
    }

    public enum WhoWins
    {
        noOne = 0,
        you = 1,
        enemy = 2
    }
}
