using Engine.Tools;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Engine.Network
{
    public class TcpClientm
    {
        private TcpClient tcp;
        public NetworkStream NetStream;
        private bool _Write = false;
        public bool Write
        {
            get
            {
                return _Write;
            }
            set
            {
                if (value)
                    Tools.Tools.TimeOut(ref _Write, 300);
                _Write = value;
            }
        }
        public BinaryReader read;
        public BinaryWriter write;
        public MemoryStream buffer;
        private Thread rea;
        private byte[] opcode;
        public ushort id;
        public string ip;
        public int port;

        public TcpClientm(TcpClient t, ushort i)
        {
            this.tcp = t;

            IPEndPoint address = this.tcp.Client.RemoteEndPoint as IPEndPoint;

            this.ip = address.Address.ToString();
            this.port = address.Port;

            this.id = i;
            this.NetStream = this.tcp.GetStream();
            this.read = new BinaryReader(this.NetStream);
            this.buffer = new MemoryStream();
            this.write = new BinaryWriter(this.buffer);

            this.opcode = new byte[1];
            rea = new Thread(this.AcceptPacket);
            rea.Start();
        }
        public virtual void Close()
        {
            if (this.tcp != null)
                this.tcp.Close();
            this.tcp = null;
            this.buffer.Dispose();
            this.write = null;
            this.NetStream = null;
            this.opcode = null;
            this.ip = null;
        }
        private void AcceptPacket()
        {
            while (true)
            {
                    try
                    {
                        write.Write((byte)0);
                        if (!this.tcp.Connected)
                        {
                            Network.closeTcpClient(this.id);
                            return;
                        }
                        if (this.NetStream.DataAvailable)
                        Network._gd(this);
                        //else
                        //Network.closeTcpClient(this.id);
                        //this.NetStream.BeginRead(this.opcode, 0, 1, this.AcceptPacket, null);
                    }
                    catch
                    {
                        Network.closeTcpClient(id);
                    }
                Thread.Sleep(5);
            }
        }
    }
}
