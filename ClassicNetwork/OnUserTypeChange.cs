using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassicNetwork
{
    public delegate void OnUserTypeChange(object sender, OnUserTypeChangeArgs e);

    public class OnUserTypeChangeArgs : EventArgs
    {
        public byte ID;
        public OnUserTypeChangeArgs(byte id)
        {
            this.ID = id;
        }
    }
}
