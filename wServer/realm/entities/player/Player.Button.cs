using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wServer.cliPackets;
using wServer.svrPackets;

namespace wServer.realm.entities
{
    partial class Player
    {
        public void TextBoxButton(TextBoxButtonPacket pkt)
        {
            int button = pkt.Button;
            string type = pkt.Type;
            if (type == "NewClient")
            {
                psr.Disconnect();
            }
        }
    }
}
