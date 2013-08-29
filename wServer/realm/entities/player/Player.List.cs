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
        const int LOCKED_LIST_ID = 0;
        const int IGNORED_LIST_ID = 1;

        public void SendAccountList(List<int> list, int id)
        {
            psr.SendPacket(new AccountListPacket()
            {
                AccountListId = id,
                AccountIds = list.ToArray()
            });
        }

        public void EditAccountList(RealmTime time, EditAccountListPacket pkt)
        {
            List<int> list;
            if (pkt.AccountListId == LOCKED_LIST_ID)
                list = Locked;
            else if (pkt.AccountListId == IGNORED_LIST_ID)
                list = Ignored;
            else return;
            if (list == null)
                list = new List<int>();
            Player player = Owner.GetEntity(pkt.ObjectId) as Player;
            if (player == null) return;
            int accId = player.psr.Account.AccountId;
            db.Database dbx = new db.Database();
            //if (pkt.Add && list.Count < 6)
            //    list.Add(accId);
            //else
            //    list.Remove(accId);
            
            if (pkt.Add)
            {
                list.Add(accId);
                if (pkt.AccountListId == LOCKED_LIST_ID)
                    dbx.AddLock(psr.Account.AccountId, accId);
                if (pkt.AccountListId == IGNORED_LIST_ID)
                    dbx.AddIgnore(psr.Account.AccountId, accId);
            }
            else
            {
                list.Remove(accId);
                if (pkt.AccountListId == LOCKED_LIST_ID)
                    dbx.RemoveLock(psr.Account.AccountId, accId);
                if (pkt.AccountListId == IGNORED_LIST_ID)
                    dbx.RemoveIgnore(psr.Account.AccountId, accId);
            }

            SendAccountList(list, pkt.AccountListId);
        }
    }
}
