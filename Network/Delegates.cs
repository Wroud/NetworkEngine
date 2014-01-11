using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Network
{
    public delegate bool OnJoin(TcpClient sender);
    public delegate void OnLeft(TcpClientm sender);
    public delegate void GetData(TcpClientm sender);
}
