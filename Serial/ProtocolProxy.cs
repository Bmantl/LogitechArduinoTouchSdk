using System.Collections.Generic;
using System.Linq;
using System.Text;
using XSockets.Core.Common.Globals;
using XSockets.Core.Common.Protocol;
using XSockets.Core.Common.Socket.Event.Arguments;
using XSockets.Core.Common.Socket.Event.Interface;
using XSockets.Core.Common.Utility.Serialization;
using XSockets.Core.XSocket.Model;
using XSockets.Plugin.Framework;

namespace Photon.Modules.Protocols
{
    public class PhotonProtocolProxy : IProtocolProxy
    {
        private IXSocketJsonSerializer JsonSerializer { get; set; }

        public PhotonProtocolProxy()
        {
            JsonSerializer = Composable.GetExport<IXSocketJsonSerializer>();
        }

        // A incoming frame is ready to be converted into an IMessage
        public IMessage In(IEnumerable<byte> payload, MessageType messageType)
        {
            var data = Encoding.UTF8.GetString(payload.ToArray());
            //Sanity checks...
            if (data.Length == 0) return null;
            var d = data.Split('|');
            if (d.Length != 3) return null;

            return new Message(d[2], d[1], d[0]);
        }

        // A outgoing frame (IMessage) is going to be formated into the format the client expect
        public byte[] Out(IMessage message)
        {
            var result = new List<byte>();
            result.Add(0x00);
            result.AddRange(Encoding.UTF8.GetBytes(string.Format("{0}|{1}|{2}", message.Controller, message.Topic, message.Data)));
            result.Add(0xff);
            return result.ToArray();
        }
    }
}