using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassicNetwork
{
    public delegate void OnPlayerRemove(object sender, OnPlayerRemoveArgs e);

    public class OnPlayerRemoveArgs : EventArgs
    {
        public byte ID;
        public OnPlayerRemoveArgs(byte id)
        {
            this.ID = id;
        }
    }
}
