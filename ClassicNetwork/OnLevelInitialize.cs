using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassicNetwork
{
    public delegate void OnLevelInitialize(object sender, OnLevelInitializeArgs e);

    public class OnLevelInitializeArgs : EventArgs
    {
        public System.IO.MemoryStream Map;
        public OnLevelInitializeArgs(System.IO.MemoryStream map)
        {
            this.Map = map;
        }
    }
}
