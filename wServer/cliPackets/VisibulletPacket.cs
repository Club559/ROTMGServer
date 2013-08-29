using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wServer.cliPackets
{
    public class VisibulletPacket : ClientPacket
    {
        public int Damage { get; set; }
        public int EnemyId { get; set; }
        public byte BulletId { get; set; }

        public override PacketID ID { get { return PacketID.Visibullet; } }
        public override Packet CreateInstance() { return new VisibulletPacket(); }

        protected override void Read(ClientProcessor psr, NReader rdr)
        {
            Damage = rdr.ReadInt32();
            EnemyId = rdr.ReadInt32();
            BulletId = rdr.ReadByte();
        }
        protected override void Write(ClientProcessor psr, NWriter wtr)
        {
            wtr.Write(Damage);
            wtr.Write(EnemyId);
            wtr.Write(BulletId);
        }
    }
}
