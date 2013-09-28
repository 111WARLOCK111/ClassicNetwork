using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassicNetwork
{
    public delegate void OnPlayerTeleport(object sender, OnPlayerTeleportArgs e);

    public class OnPlayerTeleportArgs : EventArgs
    {
        public byte ID;
        public float X;
        public float Y;
        public float Z;
        public byte Heading;
        public byte Pitch;
        public OnPlayerTeleportArgs(byte id, float x, float y, float z, byte heading, byte pitch)
        {
            this.ID = id;
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.Heading = heading;
            this.Pitch = pitch;
        }
    }
}
