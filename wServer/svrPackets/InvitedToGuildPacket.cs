using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wServer.svrPackets
{
    public class InvitedToGuildPacket : ServerPacket
    {
        public string Name;
        public string Guild;

        public override PacketID ID { get { return PacketID.InvitedToGuild; } }
        public override Packet CreateInstance() { return new InvitedToGuildPacket(); }

        protected override void Read(ClientProcessor psr, NReader rdr)
        {
            Name = rdr.ReadUTF();
            Guild = rdr.ReadUTF();
        }

        protected override void Write(ClientProcessor psr, NWriter wtr)
        {
            wtr.WriteUTF(Name);
            wtr.WriteUTF(Guild);
        } 
    }
}
