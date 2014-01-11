using Engine.Tools;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Engine.Network
{
    public class Network
    {

        private static TcpListener Socket;
        public static Dictionary<ushort, TcpClientm> connects;
        public static GetData _gd;
        private static OnJoin _join;
        private static OnLeft _left;

        public static void Create(int port, GetData gd, OnJoin join, OnLeft left)
        {
            connects = new Dictionary<ushort, TcpClientm>();
            Socket = new TcpListener(IPAddress.Any, port);
            _gd = gd;
            _join = join;
            _left = left;
        }

        public static void Start()
        {
            Socket.Start();
            Socket.BeginAcceptTcpClient(AcceptTcpClient, null);
        }

        private static void AcceptTcpClient(IAsyncResult result)
        {
            TcpClient tcpClient = Socket.EndAcceptTcpClient(result);

            if (_join(tcpClient))
            {
                ushort id = ID;
                connects.Add(id, new TcpClientm(tcpClient, id));
            }
            Socket.BeginAcceptTcpClient(AcceptTcpClient, null);
        }

        public static void closeTcpClient(ushort id)
        {
            if (connects.ContainsKey(id))
            {
                //Utils.TimeOut(ref net.connects[id].Write, 2000);
                _left(connects[id]);
                connects[id].Close();
                connects.Remove(id);
            }
        }

        private static ushort ID
        {
            get
            {
                ushort[] ids = connects.Keys.ToArray<ushort>();
                for (ushort i = 0; i < 65535; i++)
                    if (!ids.Contains(i))
                        return i;
                return 0;
            }
        }
    }
}
