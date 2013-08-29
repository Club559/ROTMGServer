using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.IO;
using System.Collections.Concurrent;
using System.Threading;
using System.Net;
using wServer.realm;

namespace wServer
{
    //hackish code
    class NetworkHandler
    {
        enum ReceiveState
        {
            Awaiting,
            ReceivingHdr,
            ReceivingBody,
            Processing
        }
        class ReceiveToken
        {
            public int Length;
            public Packet Packet;
        }
        enum SendState
        {
            Awaiting,
            Ready,
            Sending
        }
        class SendToken
        {
            public Packet Packet;
        }

        public const int BUFFER_SIZE = 0x10000;
        SocketAsyncEventArgs send;
        SendState sendState = SendState.Awaiting;
        ReceiveState receiveState = ReceiveState.Awaiting;
        Socket skt;
        ClientProcessor parent;
        public NetworkHandler(ClientProcessor parent, Socket skt)
        {
            this.parent = parent;
            this.skt = skt;
        }

        public void BeginHandling()
        {
            Console.WriteLine("{0} connected.", skt.RemoteEndPoint);

            skt.NoDelay = true;
            skt.UseOnlyOverlappedIO = true;
            
            send = new SocketAsyncEventArgs();
            send.Completed += IOCompleted;
            send.UserToken = new SendToken();
            send.SetBuffer(new byte[BUFFER_SIZE], 0, BUFFER_SIZE);

            var receive = new SocketAsyncEventArgs();
            receive.Completed += IOCompleted;
            receive.UserToken = new ReceiveToken();
            receive.SetBuffer(new byte[BUFFER_SIZE], 0, BUFFER_SIZE);

            receiveState = ReceiveState.ReceivingHdr;
            receive.SetBuffer(0, 5);
            if (!skt.ReceiveAsync(receive))
                IOCompleted(this, receive);
        }

        void ProcessPolicyFile()
        {
            var s = new NetworkStream(skt);
            NWriter wtr = new NWriter(s);
            wtr.WriteNullTerminatedString(@"<cross-domain-policy>
    <allowed-access-from domain=""*"" to-ports=""*"" />
</cross-domain-policy>");
            wtr.Write((byte)'\r');
            wtr.Write((byte)'\n');
            parent.Disconnect();
        }
        
        void IOCompleted(object sender, SocketAsyncEventArgs e)
        {
            try
            {
                bool repeat;
                do
                {
                    repeat = false;

                    if (e.SocketError != SocketError.Success)
                        throw new SocketException((int)e.SocketError);

                    if (e.LastOperation == SocketAsyncOperation.Receive)
                    {
                        switch (receiveState)
                        {
                            case ReceiveState.ReceivingHdr:
                                if (e.BytesTransferred < 5)
                                {
                                    parent.Disconnect();
                                    return;
                                }

                                if (e.Buffer[0] == 0x3c && e.Buffer[1] == 0x70 &&
                                    e.Buffer[2] == 0x6f && e.Buffer[3] == 0x6c && e.Buffer[4] == 0x69)
                                {
                                    ProcessPolicyFile();
                                    return;
                                } 
                                
                                var len = (e.UserToken as ReceiveToken).Length =
                                    IPAddress.NetworkToHostOrder(BitConverter.ToInt32(e.Buffer, 0)) - 5;
                                if (len < 0 || len > BUFFER_SIZE)
                                    throw new InternalBufferOverflowException();
                                (e.UserToken as ReceiveToken).Packet = Packet.Packets[(PacketID)e.Buffer[4]].CreateInstance();

                                receiveState = ReceiveState.ReceivingBody;
                                e.SetBuffer(0, len);
                                if (!skt.ReceiveAsync(e))
                                {
                                    repeat = true;
                                    continue;
                                }
                                break;
                            case ReceiveState.ReceivingBody:
                                if (e.BytesTransferred < (e.UserToken as ReceiveToken).Length)
                                {
                                    parent.Disconnect();
                                    return;
                                }

                                var pkt = (e.UserToken as ReceiveToken).Packet;
                                pkt.Read(parent, e.Buffer, (e.UserToken as ReceiveToken).Length);

                                receiveState = ReceiveState.Processing;
                                bool cont = OnPacketReceived(pkt);

                                if (cont && skt.Connected)
                                {
                                    receiveState = ReceiveState.ReceivingHdr;
                                    e.SetBuffer(0, 5);
                                    if (!skt.ReceiveAsync(e))
                                    {
                                        repeat = true;
                                        continue;
                                    }
                                }
                                break;
                            default:
                                throw new InvalidOperationException(e.LastOperation.ToString());
                        }
                    }
                    else if (e.LastOperation == SocketAsyncOperation.Send)
                    {
                        switch (sendState)
                        {
                            case SendState.Ready:
                                var dat = (e.UserToken as SendToken).Packet.Write(parent);

                                sendState = SendState.Sending;
                                e.SetBuffer(dat, 0, dat.Length);
                                if (!skt.SendAsync(e))
                                {
                                    repeat = true;
                                    continue;
                                }
                                break;
                            case SendState.Sending:
                                (e.UserToken as SendToken).Packet = null;

                                if (CanSendPacket(e, true))
                                {
                                    repeat = true;
                                    continue;
                                }
                                break;
                            default:
                                throw new InvalidOperationException(e.LastOperation.ToString());
                        }
                    }
                    else
                        throw new InvalidOperationException(e.LastOperation.ToString());
                } while (repeat);
            }
            catch (Exception ex)
            {
                OnError(ex);
            }
        }


        void OnError(Exception ex)
        {
            parent.Disconnect();
        }
        bool OnPacketReceived(Packet pkt)
        {
            //return parent.ProcessPacket(pkt);
            if (parent.IsReady())
            {
                RealmManager.Network.AddPendingPacket(parent, pkt);
                return true;
            }
            else
                return false;
        }
        ConcurrentQueue<Packet> pendingPackets = new ConcurrentQueue<Packet>();
        bool CanSendPacket(SocketAsyncEventArgs e, bool ignoreSending)
        {
            lock (sendLock)
            {
                if (sendState == SendState.Ready ||
                    (!ignoreSending && sendState == SendState.Sending))
                    return false;
                Packet packet;
                if (pendingPackets.TryDequeue(out packet))
                {
                    (e.UserToken as SendToken).Packet = packet;
                    sendState = SendState.Ready;
                    return true;
                }
                else
                {
                    sendState = SendState.Awaiting;
                    return false;
                }
            }
        }

        object sendLock = new object();
        public void SendPacket(Packet pkt)
        {
            try
            {
                pendingPackets.Enqueue(pkt);
                if (CanSendPacket(send, false))
                {
                    var dat = (send.UserToken as SendToken).Packet.Write(parent);

                    sendState = SendState.Sending;
                    send.SetBuffer(dat, 0, dat.Length);
                    if (!skt.SendAsync(send))
                        IOCompleted(this, send);
                }
            }
            catch
            {
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.WriteLine("Error sending packet with ID " + pkt.ID.ToString());
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
        public void SendPackets(IEnumerable<Packet> pkts)
        {
            try
            {
                foreach (var i in pkts)
                    pendingPackets.Enqueue(i);
                if (CanSendPacket(send, false))
                {
                    var dat = (send.UserToken as SendToken).Packet.Write(parent);

                    sendState = SendState.Sending;
                    send.SetBuffer(dat, 0, dat.Length);
                    if (!skt.SendAsync(send))
                        IOCompleted(this, send);
                }
            }
            catch
            {
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.WriteLine("Error sending packets, check ClientProcessor.cs");
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
    }
}
