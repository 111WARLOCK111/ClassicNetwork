using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassicNetwork
{
    public delegate void OnLevelChunk(object sender, OnLevelChunkArgs e);

    public class OnLevelChunkArgs : EventArgs
    {
        public int ChunkLength;
        public byte[] ChunkData;
        public byte[] ChunkWithoutPadding;
        public byte LoadingPrecent;
        public System.IO.MemoryStream Map;
        public OnLevelChunkArgs(int len, byte[] data, byte[] padata, byte loading, System.IO.MemoryStream map)
        {
            this.ChunkLength = len;
            this.ChunkData = data;
            this.ChunkWithoutPadding = padata;
            this.LoadingPrecent = loading;
            this.Map = map;
        }
    }
}
