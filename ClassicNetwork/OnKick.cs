using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassicNetwork
{
    public delegate void OnKick(object sender, OnKickArgs e);

    public class OnKickArgs : EventArgs
    {
        public string Reason;
        public OnKickArgs(string reason)
        {
            this.Reason = reason;
        }
    }
}
