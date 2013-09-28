using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace ClassicNetwork
{
    public class Network
    {
        
        public static void SendPacket(byte[] packet)
        {
            int sent = Server.ConnectionSocket.Send(packet);
            if (sent != packet.Length)
            {
                throw new Exception();
            }
        }

        public static void SendChat(string s)
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);
            bw.Write((byte)0x0d);
            bw.Write((byte)255);
            NetworkHelper.WriteString64(bw, s);
            SendPacket(ms.ToArray());
        }

        public static void SendSetBlock(short x, short y, short z, byte mode, int type)
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);
            bw.Write((byte)0x05);
            NetworkHelper.WriteInt16(bw, (short)((x - 16) / 32));
            NetworkHelper.WriteInt16(bw, (short)((z - 16) / 32));
            NetworkHelper.WriteInt16(bw, (short)((y - 16) / 32));
            bw.Write(mode);
            bw.Write((byte)type);
            SendPacket(ms.ToArray());
        }
    }
}
