using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wServer.realm.entities;
using wServer.svrPackets;

namespace wServer.realm
{
    public class ChatManager
    {
        RealmManager manager;
        public ChatManager(RealmManager manager)
        {
            this.manager = manager;
        }

        public void Say(Player src, string text)
        {
            src.Owner.BroadcastPacket(new TextPacket()
            {
                Name = (src.Client.Account.Admin ? "@" : "") + src.Name,
                ObjectId = src.Id,
                Stars = src.Stars,
                BubbleTime = 5,
                Recipient = "",
                Text = text,
                CleanText = text
            }, null);
        }

        public void Announce(string text)
        {
            foreach (var i in RealmManager.Clients.Values)
                i.SendPacket(new TextPacket()
                {
                    BubbleTime = 0,
                    Stars = -1,
                    Name = "#Announcement",
                    Text = text,
                    CleanText = text
                });
        }

        public void Oryx(World world, string text)
        {
            world.BroadcastPacket(new TextPacket()
            {
                BubbleTime = 0,
                Stars = -1,
                Name = "#Oryx the Mad God",
                Text = text
            }, null);
        }
    }
}
