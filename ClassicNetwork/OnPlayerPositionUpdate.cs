using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassicNetwork
{
    public delegate void OnPlayerPositionUpdate(object sender, OnPlayerPositionUpdateArgs e);

    public class OnPlayerPositionUpdateArgs : EventArgs
    {
        public byte ID;
        public float X;
        public float Y;
        public float Z;
        public OnPlayerPositionUpdateArgs(byte id, float x, float y, float z)
        {
            this.ID = id;
            this.X = x;
            this.Y = y;
            this.Z = z;
        }
    }
}
