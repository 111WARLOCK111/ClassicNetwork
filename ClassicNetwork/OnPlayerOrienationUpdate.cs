using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassicNetwork
{
    public delegate void OnPlayerOrienationUpdate(object sender, OnPlayerOrienationUpdateArgs e);

    public class OnPlayerOrienationUpdateArgs : EventArgs
    {
        public byte ID;
        public byte Heading;
        public byte Pitch;
        public OnPlayerOrienationUpdateArgs(byte id, byte heading, byte pitch)
        {
            this.ID = id;
            this.Heading = heading;
            this.Pitch = pitch;
        }
    }
}
