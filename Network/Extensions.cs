using Engine.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Network
{
    public static class Extensions
    {
        public static void Flush(this TcpClientm tc)
        {
            byte[] data = tc.buffer.ToArray();
            int pos = (int)tc.buffer.Position;
            tc.buffer.Position = 0;
            try
            {
                tc.NetStream.Write(data, 0, pos);
            }
            catch
            {
                Log.Error("Ошибка записи пакета пользователю {0}", tc.id);
            }
            tc.Write = false;
        }
    }
}
