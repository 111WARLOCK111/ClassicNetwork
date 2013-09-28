using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassicNetwork
{
    public delegate void OnBlockChange(object sender, OnBlockChangeArgs e);

    public class OnBlockChangeArgs : EventArgs
    {
        public int x;
        public int y;
        public int z;
        public byte block;
        public OnBlockChangeArgs(int X, int Y, int Z, byte Block)
        {
            this.x = X;
            this.y = Y;
            this.z = Z;
            this.block = Block;
        }
    }
}
