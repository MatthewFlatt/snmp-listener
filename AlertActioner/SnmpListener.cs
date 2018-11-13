using SnmpSharpNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace AlertActioner
{
    class SnmpListener
    {
        public Int32 Port { get; private set; }
        public Socket Socket { get; private set; }
        public EndPoint EndPoint { get; private set; }

        public SnmpListener(Int32 port = 162) {
            Port = port;
            Socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            IPEndPoint ipep = new IPEndPoint(IPAddress.Any, port);
            EndPoint = (EndPoint)ipep;
            
            Socket.Bind(EndPoint);
            // Disable timeout processing. Just block until packet is received 
            Socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 0);
        }

        public SnmpData Listen() {
            int inlen = -1;
            byte[] indata = new byte[16 * 1024];
            // 16KB receive buffer int inlen = 0;
            IPEndPoint peer = new IPEndPoint(IPAddress.Any, 0);
            EndPoint inep = (EndPoint)peer;
            try
            {
                inlen = Socket.ReceiveFrom(indata, ref inep);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            if (inlen > 0)
            {
                // Parse SNMP Version 2 TRAP packet 
                SnmpV2Packet pkt = new SnmpV2Packet();
                pkt.decode(indata, inlen);
                return new SnmpData(pkt, inep);
            }
            return null;
        }
    }
}
