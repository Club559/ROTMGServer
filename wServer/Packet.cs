using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using wServer.cliPackets;
using wServer.svrPackets;
using System.Net.Sockets;
using System.Net;

namespace wServer
{
    public abstract class Packet
    {
        public static Dictionary<PacketID, Packet> Packets = new Dictionary<PacketID, Packet>();
        static Packet()
        {
            foreach (var i in typeof(Packet).Assembly.GetTypes())
                if (typeof(Packet).IsAssignableFrom(i) && !i.IsAbstract)
                {
                    Packet pkt = (Packet)Activator.CreateInstance(i);
                    if (!(pkt is ServerPacket))
                        Packets.Add(pkt.ID, pkt);
                }
        }
        public abstract PacketID ID { get; }
        public abstract Packet CreateInstance();

        public abstract byte[] Crypt(ClientProcessor psr, byte[] dat, int len);

        public void Read(ClientProcessor psr, byte[] body, int len)
        {
            Read(psr, new NReader(new MemoryStream(Crypt(psr, body, len))));
        }
        public byte[] Write(ClientProcessor psr)
        {
            MemoryStream s = new MemoryStream();
            this.Write(psr, new NWriter(s));

            byte[] content = s.ToArray();
            byte[] ret = new byte[5 + content.Length];
            content = this.Crypt(psr, content, content.Length);
            Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder(ret.Length)), 0, ret, 0, 4);
            ret[4] = (byte)this.ID;
            Buffer.BlockCopy(content, 0, ret, 5, content.Length);
            return ret;
        }

        protected abstract void Read(ClientProcessor psr, NReader rdr);
        protected abstract void Write(ClientProcessor psr, NWriter wtr);

        public override string ToString()
        {
            StringBuilder ret = new StringBuilder("{");
            var arr = GetType().GetProperties();
            for (var i = 0; i < arr.Length; i++)
            {
                if (i != 0) ret.Append(", ");
                ret.AppendFormat("{0}: {1}", arr[i].Name, arr[i].GetValue(this, null));
            }
            ret.Append("}");
            return ret.ToString();
        }
    }

    public class NopPacket : Packet
    {
        public override PacketID ID { get { return PacketID.UpdateAck; } }
        public override Packet CreateInstance() { return new NopPacket(); }
        public override byte[] Crypt(ClientProcessor psr, byte[] dat, int len) { return dat; }
        protected override void Read(ClientProcessor psr, NReader rdr) { }
        protected override void Write(ClientProcessor psr, NWriter wtr) { }
    }
}
