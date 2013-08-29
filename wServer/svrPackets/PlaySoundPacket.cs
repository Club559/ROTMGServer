using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wServer.svrPackets
{
    public class PlaySoundPacket : ServerPacket
    {
        public int OwnerId { get; set; }
        public byte SoundId { get; set; }

        public override PacketID ID { get { return PacketID.PlaySound; } }
        public override Packet CreateInstance() { return new PlaySoundPacket(); }

        protected override void Read(ClientProcessor psr, NReader rdr)
        {
            OwnerId = rdr.ReadByte();
            SoundId = rdr.ReadByte();
        }

        protected override void Write(ClientProcessor psr, NWriter wtr)
        {
            wtr.Write(OwnerId);
            wtr.Write(SoundId);
        }
    }
}
