using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wServer.svrPackets;
using wServer.cliPackets;
using wServer.logic.loot;
using System.Xml.Linq;
using db;

namespace wServer.realm.entities
{
    public partial class Player
    {
        bool AuditItem(IContainer container, Item item, int slot)
        {
            if (Decision == 1 || Decision == 2)
                return false;
            return item == null || container.SlotTypes[slot] == 0 || item.SlotType == container.SlotTypes[slot];
        }

        public bool HasSlot(int slot)
        {
            bool ret = false;
            try
            {
                ret = Inventory[3] != null;
            }
            catch { }
            return ret;
        }

        Random invRand = new Random();
        public void InventorySwap(RealmTime time, InvSwapPacket pkt)
        {
            Entity en1 = Owner.GetEntity(pkt.Obj1.ObjectId);
            Entity en2 = Owner.GetEntity(pkt.Obj2.ObjectId);
            IContainer con1 = en1 as IContainer;
            IContainer con2 = en2 as IContainer;

            //TODO: locker
            Item item1 = con1.Inventory[pkt.Obj1.SlotId];
            Item item2 = con2.Inventory[pkt.Obj2.SlotId];
            if (!AuditItem(con2, item1, pkt.Obj2.SlotId) ||
                !AuditItem(con1, item2, pkt.Obj1.SlotId))
                (en1 as Player).Client.SendPacket(new InvResultPacket() { Result = 1 });
            else
            {
                List<short> publicbags = new List<short>() {
                    0x0500,
                    0xffd
                };

                con1.Inventory[pkt.Obj1.SlotId] = item2;
                con2.Inventory[pkt.Obj2.SlotId] = item1;

                if (publicbags.Contains(en1.ObjectType) && (item2.Soulbound || item2.Undead || item2.SUndead))
                {
                    DropBag(item2);
                    con1.Inventory[pkt.Obj1.SlotId] = null;
                }
                if (publicbags.Contains(en2.ObjectType) && (item1.Soulbound || item1.Undead || item1.SUndead))
                {
                    DropBag(item1);
                    con2.Inventory[pkt.Obj2.SlotId] = null;
                }

                en1.UpdateCount++;
                en2.UpdateCount++;

                if (en1 is Player)
                {
                    if (en1.Owner.Name == "Vault")
                        (en1 as Player).Client.Save();
                    (en1 as Player).CalcBoost();
                    (en1 as Player).Client.SendPacket(new InvResultPacket() { Result = 0 });
                }
                if (en2 is Player)
                {
                    if (en2.Owner.Name == "Vault")
                        (en2 as Player).Client.Save();
                    (en2 as Player).CalcBoost();
                    (en2 as Player).Client.SendPacket(new InvResultPacket() { Result = 0 });
                }
            }
        }
        public void InventoryDrop(RealmTime time, InvDropPacket pkt)
        {
            //TODO: locker again
            const short NORM_BAG = 0x0500;
            const short SOUL_BAG = 0x0503;
            const short PDEM_BAG = 0xffd;
            const short DEM_BAG = 0xffe;
            const short SDEM_BAG = 0xfff;

            Entity entity = Owner.GetEntity(pkt.Slot.ObjectId);
            IContainer con = entity as IContainer;
            if (con.Inventory[pkt.Slot.SlotId] == null) return;

            if ((entity is Player) && (entity as Player).Decision == 1)
            {
                (entity as Player).Client.SendPacket(new InvResultPacket() { Result = 1 });
                return;
            }

            Item item = con.Inventory[pkt.Slot.SlotId];
            con.Inventory[pkt.Slot.SlotId] = null;
            entity.UpdateCount++;

            Container container;
            if (item.Soulbound)
            {
                container = new Container(SOUL_BAG, 1000 * 60, true);
                container.BagOwner = AccountId;
            }
            else if (item.Undead)
            {
                container = new Container(DEM_BAG, 1000 * 60, true);
                container.BagOwner = AccountId;
            }
            else if (item.PUndead)
            {
                container = new Container(PDEM_BAG, 1000 * 60, true);
            }
            else if (item.SUndead)
            {
                container = new Container(SDEM_BAG, 1000 * 60, true);
                container.BagOwner = AccountId;
            }
            else
            {
                container = new Container(NORM_BAG, 1000 * 60, true);
            }
            float bagx = entity.X + (float)((invRand.NextDouble() * 2 - 1) * 0.5);
            float bagy = entity.Y + (float)((invRand.NextDouble() * 2 - 1) * 0.5);
            try
            {
                container.Inventory[0] = item;
                container.Move(bagx, bagy);
                container.Size = 75;
                Owner.EnterWorld(container);

                if (entity is Player)
                {
                    (entity as Player).CalcBoost();
                    (entity as Player).Client.SendPacket(new InvResultPacket()
                    {
                        Result = 0
                    });
                    (entity as Player).Client.Save();
                }
            }
            catch
            {
                Console.Out.WriteLine(Name + " just attempted to dupe.");
            }
        }
        public void DropBag(Item i)
        {
            short bagId = 0x0500;
            bool soulbound = false;
            if (i.Soulbound) {
                bagId = 0x0503; soulbound = true; }
            else if (i.Undead) {
                bagId = 0xffe; soulbound = true; }
            else if (i.SUndead) {
                bagId = 0xfff; soulbound = true; }
            else if (i.PUndead) {
                bagId = 0xffd; }

            Container container = new Container(bagId, 1000 * 60, true);
            if (soulbound)
                container.BagOwner = AccountId;
            container.Inventory[0] = i;
            container.Move(X + (float)((invRand.NextDouble() * 2 - 1) * 0.5), Y + (float)((invRand.NextDouble() * 2 - 1) * 0.5));
            container.Size = 75;
            Owner.EnterWorld(container);

        }
    }
}
