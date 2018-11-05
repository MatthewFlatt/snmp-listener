using SnmpSharpNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ActionLobster
{
    class SnmpData
    {
        public EndPoint Sender { get; private set; }
        public string Community { get; private set; }
        public AlertData AlertData { get; }

        public SnmpData(SnmpV2Packet packetData, EndPoint sender) {
            Sender = sender;
            Community = packetData.Community.ToString();
            AlertData = new AlertData(packetData.Pdu);
        }
    }
}
