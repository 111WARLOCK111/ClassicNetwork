using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassicNetwork
{
    public delegate void OnLevelFinalize(object sender, OnLevelFinalizeArgs e);

    public class OnLevelFinalizeArgs : EventArgs
    {
        public int MapX;
        public int MapY;
        public int MapZ;
        public System.IO.MemoryStream Map;
        public byte[, ,] MapBytes;
        public OnLevelFinalizeArgs(int MaxX, int MaxY, int MaxZ, System.IO.MemoryStream map, byte[, ,] mapbytes)
        {
            this.MapX = MaxX;
            this.MapY = MaxY;
            this.MapZ = MaxZ;
            this.Map = map;
            this.MapBytes = mapbytes;
        }
    }
}
