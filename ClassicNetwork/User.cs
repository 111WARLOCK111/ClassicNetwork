using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net.Sockets;
using System.Net;
using System.IO.Compression;

namespace ClassicNetwork
{
    public class User
    {
        List<byte> received = new List<byte>();
        public string Username;
        public event OnBlockChange OnBlockChange;
        public event OnKick OnKick;
        public event OnLevelChunk OnLevelChunk;
        public event OnLevelFinalize OnLevelFinalize;
        public event OnLevelInitialize OnLevelInitialize;
        public event OnPing OnPing;
        public event OnPlayerAdd OnPlayerAdd;
        public event OnPlayerOrienationUpdate OnPlayerOrienationUpdate;
        public event OnPlayerPositionOrienationUpdate OnPlayerPositionOrienationUpdate;
        public event OnPlayerPositionUpdate OnPlayerPositionUpdate;
        public event OnPlayerRemove OnPlayerRemove;
        public event OnPlayerTeleport OnPlayerTeleport;
        public event OnServerIdentify OnServerIdentify;
        public event OnServerMessage OnServerMessage;
        public event OnUserTypeChange OnUserTypeChange;
        public Boolean ServerIdentified = false;
        public Byte ProtocolVersion;
        public String ServerName;
        public String ServerMotd;
        public Byte UserType;
        public MemoryStream receivedMapStream;
        public Byte MapLoadingPercentComplete;
        public Int32 MapX;
        public Int32 MapY;
        public Int32 MapZ;
        public Int32 MaxX;
        private Int32 MaxY;
        private Int32 MaxZ;

        public User(string username)
        {
            this.Username = username;
        }

        public void Connect(ClassicServer serv)
        {
            Server.ConnectionSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Server.iep = new IPEndPoint(IPAddress.Any, serv.Port);
            Server.ConnectionSocket.Connect(serv.IP, serv.Port);
            byte[] n = CreateLoginPacket(this.Username, serv.Salt);
            Server.ConnectionSocket.Send(n);
            this.Process();
        }

        private byte[] CreateLoginPacket(string username, string verificationKey)
        {
            MemoryStream n = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(n);
            bw.Write((byte)0);
            bw.Write((byte)0x07);
            bw.Write(NetworkHelper.StringToBytes(username));
            bw.Write(NetworkHelper.StringToBytes(verificationKey));
            bw.Write((byte)0);
            return n.ToArray();
        }

        public enum MinecraftClientPacketId
        {
            PlayerIdentification = 0,
            SetBlock = 5,
            PositionandOrientation = 8,
            Message = 0x0d,

            ExtendedPacketCommand = 100,
        }

        public enum MinecraftServerPacketId
        {
            ServerIdentification = 0,
            Ping = 1,
            LevelInitialize = 2,
            LevelDataChunk = 3,
            LevelFinalize = 4,
            SetBlock = 6,
            SpawnPlayer = 7,
            PlayerTeleport = 8,
            PositionandOrientationUpdate = 9,
            PositionUpdate = 10,
            OrientationUpdate = 11,
            DespawnPlayer = 12,
            Message = 13,
            DisconnectPlayer = 14,
            UpdateUserType = 15
        }

        private int TryReadPacket()
        {
            BinaryReader br = new BinaryReader(new MemoryStream(received.ToArray()));
            if (received.Count == 0)
            {
                return 0;
            }
            var packetId = (MinecraftServerPacketId)br.ReadByte();
            int totalread = 1;
            switch (packetId)
            {
                case MinecraftServerPacketId.ServerIdentification:
                    {
                        if (!ServerIdentified)
                        {
                            totalread += 1 + NetworkHelper.StringLength + NetworkHelper.StringLength + 1; if (received.Count < totalread) { return 0; }
                            ProtocolVersion = br.ReadByte();
                            ServerName = NetworkHelper.ReadString64(br);
                            ServerMotd = NetworkHelper.ReadString64(br);
                            UserType = br.ReadByte();
                            //UserType = (byte)100;
                            ServerIdentified = true;
                            if (OnServerIdentify != null)
                            {
                                OnServerIdentify(this, new OnServerIdentifyArgs(ServerName, ServerMotd, UserType, ServerIdentified));
                            }
                        }
                    }
                    break;
                case MinecraftServerPacketId.Ping:
                    {
                        if (OnPing != null)
                        {
                            OnPing(this, new EventArgs());
                        }
                    }
                    break;
                case MinecraftServerPacketId.LevelInitialize:
                    {
                        receivedMapStream = new MemoryStream();
                        if (OnLevelInitialize != null)
                        {
                            OnLevelInitialize(this, new OnLevelInitializeArgs(receivedMapStream));
                        }
                    }
                    break;
                case MinecraftServerPacketId.LevelDataChunk:
                    {
                        totalread += 2 + 1024 + 1; if (received.Count < totalread) { return 0; }
                        int chunkLength = NetworkHelper.ReadInt16(br);
                        byte[] chunkData = br.ReadBytes(1024);
                        BinaryWriter bw1 = new BinaryWriter(receivedMapStream);
                        byte[] chunkDataWithoutPadding = new byte[chunkLength];
                        for (int i = 0; i < chunkLength; i++)
                        {
                            chunkDataWithoutPadding[i] = chunkData[i];
                        }
                        bw1.Write(chunkDataWithoutPadding);
                        MapLoadingPercentComplete = br.ReadByte();
                        if (OnLevelChunk != null)
                        {
                            OnLevelChunk(this, new OnLevelChunkArgs(chunkLength, chunkData, chunkDataWithoutPadding, MapLoadingPercentComplete, receivedMapStream));
                        }
                    }
                    break;
                case MinecraftServerPacketId.LevelFinalize:
                    {
                        totalread += 2 + 2 + 2; if (received.Count < totalread) { return 0; }
                        MapX = NetworkHelper.ReadInt16(br);
                        MapZ = NetworkHelper.ReadInt16(br);
                        MapY = NetworkHelper.ReadInt16(br);
                        receivedMapStream.Seek(0, SeekOrigin.Begin);
                        using (MemoryStream decompressed = new MemoryStream(GzipCompression.Decompress(receivedMapStream.ToArray())))
                        {
                            if (decompressed.Length != MapX * MapY * MapZ +
                                (decompressed.Length % 1024))
                            {
                                //throw new Exception();
                            }
                            byte[, ,] receivedmap = new byte[MapX, MapY, MapZ];
                            {
                                BinaryReader br2 = new BinaryReader(decompressed);
                                int size = NetworkHelper.ReadInt32(br2);
                                for (int z = 0; z < MapZ; z++)
                                {
                                    for (int y = 0; y < MapY; y++)
                                    {
                                        for (int x = 0; x < MapX; x++)
                                        {
                                            receivedmap[x, y, z] = br2.ReadByte();
                                        }
                                    }
                                }
                            }
                            MaxX = receivedmap.GetUpperBound(0) + 1;
                            MaxY = receivedmap.GetUpperBound(1) + 1;
                            MaxZ = receivedmap.GetUpperBound(2) + 1;
                            if (OnLevelFinalize != null)
                            {
                                OnLevelFinalize(this, new OnLevelFinalizeArgs(MaxX, MaxY, MaxZ, receivedMapStream, receivedmap));
                            }
                        }
                    }
                    break;
                case MinecraftServerPacketId.SetBlock:
                    {
                        int x;
                        int y;
                        int z;
                        totalread += 2 + 2 + 2 + 1; if (received.Count < totalread) { return 0; }
                        x = NetworkHelper.ReadInt16(br);
                        z = NetworkHelper.ReadInt16(br);
                        y = NetworkHelper.ReadInt16(br);
                        byte type = br.ReadByte();
                        if (OnBlockChange != null)
                        {
                            OnBlockChange(this, new OnBlockChangeArgs(x, y, z, type));
                        }
                    }
                    break;
                case MinecraftServerPacketId.SpawnPlayer:
                    {
                        totalread += 1 + NetworkHelper.StringLength + 2 + 2 + 2 + 1 + 1; if (received.Count < totalread) { return 0; }
                        byte playerid = br.ReadByte();
                        string playername = NetworkHelper.ReadString64(br);
                        if (OnPlayerAdd != null)
                        {
                            OnPlayerAdd(this, new OnPlayerAddArgs(playerid, playername));
                        }
                    }
                    break;
                case MinecraftServerPacketId.PlayerTeleport:
                    {
                        float x;
                        float y;
                        float z;
                        totalread += 1 + (2 + 2 + 2) + 1 + 1; if (received.Count < totalread) { return 0; }
                        byte playerid = br.ReadByte();
                        x = (float)NetworkHelper.ReadInt16(br) / 32;
                        y = (float)NetworkHelper.ReadInt16(br) / 32;
                        z = (float)NetworkHelper.ReadInt16(br) / 32;
                        byte heading = br.ReadByte();
                        byte pitch = br.ReadByte();
                        if (OnPlayerTeleport != null)
                        {
                            OnPlayerTeleport(this, new OnPlayerTeleportArgs(playerid, x, y, z, heading, pitch));
                        }
                    }
                    break;
                case MinecraftServerPacketId.PositionandOrientationUpdate:
                    {
                        totalread += 1 + (1 + 1 + 1) + 1 + 1; if (received.Count < totalread) { return 0; }
                        byte playerid = br.ReadByte();
                        float x = (float)br.ReadSByte() / 32;
                        float y = (float)br.ReadSByte() / 32;
                        float z = (float)br.ReadSByte() / 32;
                        byte heading = br.ReadByte();
                        byte pitch = br.ReadByte();
                        if (OnPlayerPositionOrienationUpdate != null)
                        {
                            OnPlayerPositionOrienationUpdate(this, new OnPlayerPositionOrienationUpdateArgs(playerid, x, y, z, heading, pitch));
                        }
                    }
                    break;
                case MinecraftServerPacketId.PositionUpdate:
                    {
                        totalread += 1 + 1 + 1 + 1; if (received.Count < totalread) { return 0; }
                        byte playerid = br.ReadByte();
                        float x = (float)br.ReadSByte() / 32;
                        float y = (float)br.ReadSByte() / 32;
                        float z = (float)br.ReadSByte() / 32;
                        if (OnPlayerPositionUpdate != null)
                        {
                            OnPlayerPositionUpdate(this, new OnPlayerPositionUpdateArgs(playerid, x, y, z));
                        }
                    }
                    break;
                case MinecraftServerPacketId.OrientationUpdate:
                    {
                        totalread += 1 + 1 + 1; if (received.Count < totalread) { return 0; }
                        byte playerid = br.ReadByte();
                        byte heading = br.ReadByte();
                        byte pitch = br.ReadByte();
                        if (OnPlayerOrienationUpdate != null)
                        {
                            OnPlayerOrienationUpdate(this, new OnPlayerOrienationUpdateArgs(playerid, heading, pitch));
                        }
                    }
                    break;
                case MinecraftServerPacketId.DespawnPlayer:
                    {
                        totalread += 1; if (received.Count < totalread) { return 0; }
                        byte playerid = br.ReadByte();
                        if (OnPlayerRemove != null)
                        {
                            OnPlayerRemove(this, new OnPlayerRemoveArgs(playerid));
                        }
                    }
                    break;
                case MinecraftServerPacketId.Message:
                    {
                        totalread += 1 + NetworkHelper.StringLength; if (received.Count < totalread) { return 0; }
                        byte msgid = br.ReadByte();
                        string message = NetworkHelper.ReadString64(br);
                        //Console.WriteLine("Received Message: " + message);
                        if (OnServerMessage != null)
                        {
                            OnServerMessage(this, new OnServerMessageArgs(msgid, message));
                        }
                    }
                    break;
                case MinecraftServerPacketId.DisconnectPlayer:
                    {
                        totalread += NetworkHelper.StringLength; if (received.Count < totalread) { return 0; }
                        string disconnectReason = NetworkHelper.ReadString64(br);
                        if (OnKick != null)
                        {
                            OnKick(this, new OnKickArgs(disconnectReason));
                        }
                    }
                    break;
                case MinecraftServerPacketId.UpdateUserType:
                    {
                        totalread += 1;
                        byte UserType = br.ReadByte();
                        if (OnUserTypeChange != null)
                        {
                            OnUserTypeChange(this, new OnUserTypeChangeArgs(UserType));
                        }
                    }
                    break;
                default:
                    {
                    }
                    break;
            }
            return totalread;
        }

        public static class GzipCompression
        {
            public static byte[] Compress(byte[] data)
            {
                MemoryStream input = new MemoryStream(data);
                MemoryStream output = new MemoryStream();
                using (GZipStream compress = new GZipStream(output, CompressionMode.Compress))
                {
                    byte[] buffer = new byte[4096];
                    int numRead;
                    while ((numRead = input.Read(buffer, 0, buffer.Length)) != 0)
                    {
                        compress.Write(buffer, 0, numRead);
                    }
                }
                return output.ToArray();
            }
            public static byte[] Decompress(byte[] gzip)
            {
                // Create a GZIP stream with decompression mode.
                // ... Then create a buffer and write into while reading from the GZIP stream.
                using (GZipStream stream = new GZipStream(new MemoryStream(gzip), CompressionMode.Decompress))
                {
                    const int size = 4096;
                    byte[] buffer = new byte[size];
                    using (MemoryStream memory = new MemoryStream())
                    {
                        int count = 0;
                        do
                        {
                            count = stream.Read(buffer, 0, size);
                            if (count > 0)
                            {
                                memory.Write(buffer, 0, count);
                            }
                        }
                        while (count > 0);
                        return memory.ToArray();
                    }
                }
            }
            public static byte[] Decompress(FileInfo fi)
            {
                MemoryStream ms = new MemoryStream();
                // Get the stream of the source file.
                using (FileStream inFile = fi.OpenRead())
                {
                    // Get original file extension, for example "doc" from report.doc.gz.
                    string curFile = fi.FullName;
                    string origName = curFile.Remove(curFile.Length - fi.Extension.Length);

                    //Create the decompressed file.
                    //using (FileStream outFile = File.Create(origName))
                    {
                        using (GZipStream Decompress = new GZipStream(inFile,
                                CompressionMode.Decompress))
                        {
                            //Copy the decompression stream into the output file.
                            byte[] buffer = new byte[4096];
                            int numRead;
                            while ((numRead = Decompress.Read(buffer, 0, buffer.Length)) != 0)
                            {
                                ms.Write(buffer, 0, numRead);
                            }
                            //Console.WriteLine("Decompressed: {0}", fi.Name);
                        }
                    }
                }
                return ms.ToArray();
            }
        }

        private void Process()
        {
            if (Server.ConnectionSocket == null)
            {
                //Console.WriteLine("Invalid Server!");
                return;
            }
            for (; ; )
            {
                /*if (!Server.ConnectionSocket.Poll(0, SelectMode.SelectRead))
                {
                    Console.WriteLine("Invalid Socket!");
                    break;
                }*/
                byte[] data = new byte[1024];
                int recv;
                try
                {
                    recv = Server.ConnectionSocket.Receive(data);
                }
                catch
                {
                    recv = 0;
                }
                if (recv == 0)
                {
                    //Console.WriteLine("Invalid data!");
                    //disconnected
                    return;
                }
                for (int i = 0; i < recv; i++)
                {
                    received.Add(data[i]);
                }
                for (; ; )
                {
                    if (received.Count < 4)
                    {
                        break;
                    }
                    byte[] packet = new byte[received.Count];
                    int bytesRead;
                    bytesRead = TryReadPacket();
                    if (bytesRead > 0)
                    {
                        received.RemoveRange(0, bytesRead);
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
    }
}
