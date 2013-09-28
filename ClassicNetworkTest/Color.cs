using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassicNetworkTest
{
    class Color
    {
        public static void WriteLine(string msg)
        {
            if (!String.IsNullOrEmpty(msg))
            {
                string[] subs = msg.Split(new char[] { '&' });
                if (subs[0].Length > 0) { Console.Write(subs[0]); }
                for (int i = 1; i < subs.Length; i++)
                {
                    if (subs[i].Length > 0)
                    {
                        setcolor(subs[i][0]);
                        if (subs[i].Length > 1)
                        {
                            Console.Write(subs[i].Substring(1, subs[i].Length - 1));
                        }
                    }
                }
                Console.Write('\n');
            }
            Console.ForegroundColor = ConsoleColor.White;
        }

        private static void setcolor(char c)
        {
            switch (c)
            {
                case '0': Console.ForegroundColor = ConsoleColor.Gray; break; //Should be Black but Black is non-readable on a black background
                case '1': Console.ForegroundColor = ConsoleColor.DarkBlue; break;
                case '2': Console.ForegroundColor = ConsoleColor.DarkGreen; break;
                case '3': Console.ForegroundColor = ConsoleColor.DarkCyan; break;
                case '4': Console.ForegroundColor = ConsoleColor.DarkRed; break;
                case '5': Console.ForegroundColor = ConsoleColor.DarkMagenta; break;
                case '6': Console.ForegroundColor = ConsoleColor.DarkYellow; break;
                case '7': Console.ForegroundColor = ConsoleColor.Gray; break;
                case '8': Console.ForegroundColor = ConsoleColor.DarkGray; break;
                case '9': Console.ForegroundColor = ConsoleColor.Blue; break;
                case 'a': Console.ForegroundColor = ConsoleColor.Green; break;
                case 'b': Console.ForegroundColor = ConsoleColor.Cyan; break;
                case 'c': Console.ForegroundColor = ConsoleColor.Red; break;
                case 'd': Console.ForegroundColor = ConsoleColor.Magenta; break;
                case 'e': Console.ForegroundColor = ConsoleColor.Yellow; break;
                case 'f': Console.ForegroundColor = ConsoleColor.White; break;
                case 'r': Console.ForegroundColor = ConsoleColor.White; break;
            }
        }
    }
}
