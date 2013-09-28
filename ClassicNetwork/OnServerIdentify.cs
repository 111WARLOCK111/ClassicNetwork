using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassicNetwork
{
    public delegate void OnServerIdentify(object source, OnServerIdentifyArgs e);

    public class OnServerIdentifyArgs : EventArgs
    {
        public string ServerName;
        public string ServerMotd;
        public byte UserType;
        public bool Identified;
        public OnServerIdentifyArgs(string Name, string Motd, byte User, bool Ident)
        {
            this.ServerMotd = Motd;
            this.ServerName = Name;
            this.UserType = User;
            this.Identified = Ident;
        }
    }
}
