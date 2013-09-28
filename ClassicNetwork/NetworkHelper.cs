// This class was borrowed from 800Craft Client... o_o

using System;
using System.IO;
using System.Text;

namespace ClassicNetwork
{
    public class NetworkHelper
    {
        public static byte[] StringToBytes(string s)
        {
            byte[] b = Encoding.ASCII.GetBytes(s);
            byte[] bb = new byte[64];
            for (int i = 0; i < bb.Length; i++)
            {
                bb[i] = 32;
            }
            for (int i = 0; i < b.Length; i++)
            {
                bb[i] = b[i];
            }
            return bb;
        }
        private static string BytesToString(byte[] s)
        {
            string b = Encoding.ASCII.GetString(s).Trim();
            return b;
        }
        public static int ReadInt32(BinaryReader br)
        {
            byte[] array = br.ReadBytes(4);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(array);
            }
            return BitConverter.ToInt32(array, 0);
        }
        public static int ReadInt16(BinaryReader br)
        {
            byte[] array = br.ReadBytes(2);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(array);
            }
            return BitConverter.ToInt16(array, 0);
        }
        public static void WriteInt16(BinaryWriter bw, short v)
        {
            byte[] array = BitConverter.GetBytes((short)v);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(array);
            }
            bw.Write(array);
        }
        public static void WriteInt32(BinaryWriter bw, int v)
        {
            byte[] array = BitConverter.GetBytes((int)v);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(array);
            }
            bw.Write(array);
        }
        public static string ReadString64(BinaryReader br)
        {
            return BytesToString(br.ReadBytes(64));
        }
        public static void WriteString64(BinaryWriter bw, string s)
        {
            bw.Write(StringToBytes(s));
        }
        public static int StringLength = 64;
    }
}
