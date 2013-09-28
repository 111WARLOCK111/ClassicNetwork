using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassicNetwork
{
    public delegate void OnPlayerAdd(object sender, OnPlayerAddArgs e);

    public class OnPlayerAddArgs : EventArgs
    {
        public byte ID;
        public string Name;
        public OnPlayerAddArgs(byte id, string nick)
        {
            this.ID = id;
            this.Name = nick;
        }
    }
}
