using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassicNetwork
{
    public class ClassicServer
    {
        public string IP;
        public int Port;
        public string Salt;
        public ClassicServer(string ip, int port, string salt)
        {
            this.IP = ip;
            this.Port = port;
            this.Salt = salt;
        }
    }
}
