using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassicNetwork
{
    public delegate void OnServerMessage(object sender, OnServerMessageArgs e);

    public class OnServerMessageArgs : EventArgs
    {
        public byte ID;
        public string Message;
        public OnServerMessageArgs(byte id, string msg)
        {
            this.ID = id;
            this.Message = msg;
        }
    }
}
