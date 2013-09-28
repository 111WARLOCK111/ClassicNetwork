using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using ClassicNetwork;

namespace ClassicNetworkTest
{
    class Program
    {
        static bool OP;
        static bool First = false;
        static void Main(string[] args)
        {
            User war = new User("username");
            ClassicServer serv = new ClassicServer("ip", 25565, "MPPASS Here");
            Thread Config = new Thread(new ThreadStart(delegate
            {
                war.OnServerIdentify += new OnServerIdentify(JoinMessage);
                war.OnServerMessage += new OnServerMessage(ServMsg);
                war.OnLevelFinalize += new OnLevelFinalize(OnInit);
            }));
            Config.Start();
            war.Connect(serv);
        }

        private static void OnInit(object sender, OnLevelFinalizeArgs e)
        {
            Color.WriteLine("&c[CLIENT]: &fReceived a map!");
            Color.WriteLine("&c[CLIENT]: &f" + e.MapZ + ", " + e.MapY + ", " + e.MapX + " (Z, Y, X)");
        }

        private static void ServMsg(object sender, OnServerMessageArgs e)
        {
            Color.WriteLine(e.Message);
        }

        private static void JoinMessage(object sender, OnServerIdentifyArgs e)
        {
            if (e.UserType == (byte)100)
            {
                OP = true;
                Network.SendChat("I have OP Permission, I can use adminium and hax!");
            }
            else
            {
                OP = false;
                Network.SendChat("I have no OP Permission, I can't use adminium and hax!");
            }
        }
    }
}
