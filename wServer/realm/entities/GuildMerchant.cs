using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wServer.realm.entities
{
    public class GuildMerchant : SellableObject
    {
        public const int UP1 = 0x0736;
        public const int UP1C = 10000;
        public const int UP2 = 0x0737;
        public const int UP2C = 100000;
        public const int UP3 = 0x0738;
        public const int UP3C = 250000;
        public bool UseFame = true;
        public int nextLevel = 0;
        public GuildMerchant(short objType)
            : base(objType)
        {
            RankReq = 0;
            if (UseFame)
                Currency = CurrencyType.GuildFame;
            else
                Currency = CurrencyType.Gold;
            switch (objType)
            {
                case UP1:
                    Price = UP1C;
                    nextLevel = 1;
                    break;
                case UP2:
                    Price = UP2C;
                    nextLevel = 2;
                    break;
                case UP3:
                    Price = UP3C;
                    nextLevel = 3;
                    break;
            }
        }
        public override void Buy(Player player)
        {
            using (db.Database dbx = new db.Database())
            {
                if (dbx.GetGuild(dbx.GetGuildId(player.Guild)).GuildFame >= this.Price)
                {
                    dbx.DetractGuildFame(dbx.GetGuildId(player.Guild), this.Price);
                    dbx.ChangeGuildLevel(dbx.GetGuildId(player.Guild), nextLevel);
                    player.Client.SendPacket(new svrPackets.BuyResultPacket()
                    {
                        Message = "Upgrade successful! Please leave the Guild Hall to have it upgraded",
                        Result = 0
                    });
                }
                else
                    player.Client.SendPacket(new wServer.svrPackets.BuyResultPacket()
                    {
                        Message = "Not enough Guild Fame!",
                        Result = 9
                    });
            }
        }
    }
}
