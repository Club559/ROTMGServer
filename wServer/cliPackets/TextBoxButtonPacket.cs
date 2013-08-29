using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wServer.cliPackets
{
    public class TextBoxButtonPacket : ClientPacket
    {
        public int Button { get; set; }
        public string Type { get; set; }

        public override PacketID ID { get { return PacketID.TextBoxButton; } }
        public override Packet CreateInstance() { return new TextBoxButtonPacket(); }

        protected override void Read(ClientProcessor psr, NReader rdr)
        {
            Button = rdr.ReadInt32();
            Type = rdr.ReadUTF();
        }
        protected override void Write(ClientProcessor psr, NWriter wtr)
        {
            wtr.Write(Button);
            wtr.WriteUTF(Type);
        }
    }
}
