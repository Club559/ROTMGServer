using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wServer.realm.worlds;
using wServer.svrPackets;
using System.Collections.Concurrent;
using System.Xml.Linq;
using db;

namespace wServer.realm.entities
{
    public class Merchants : SellableObject
    {
        private Random r;

        public bool custom { get; set; }

        public int mType { get; set; }
        public int mRemaining { get; set; }
        public int mTime { get; set; }

        const int BUY_NO_GOLD = 3;
        const int BUY_NO_FAME = 6;

        public Dictionary<int, Tuple<int, CurrencyType>> prices = MerchantLists.prices;
        public int[] store9List = MerchantLists.store9List;
        public int[] store8List = MerchantLists.store8List;
        public int[] store7List = MerchantLists.store7List;
        public int[] store6List = MerchantLists.store6List;
        public int[] store5List = MerchantLists.store5List;
        public int[] store4List = MerchantLists.store4List;
        public int[] store3List = MerchantLists.store3List;
        public int[] store2List = MerchantLists.store2List;
        public int[] store1List = MerchantLists.store1List;


        public Dictionary<string, int[]> shopLists = MerchantLists.shopLists;

        public int trueMType;
        public Tuple<int,CurrencyType> truePrice;

        public Merchants(short objType)
            : base(objType)
        {
            if (objType == 0x01ca)  //Merchant
            {
                r = new Random(Id*100);
                custom = false;
                Price = 0;
                Currency = CurrencyType.Fame;
                RankReq = 0;
                mRemaining = 10;
                mTime = 10;
                mType = 2354;
            }
        }

        protected override void ExportStats(IDictionary<StatsType, object> stats)
        {
            int[] thelist;
            if (Owner.Map[(int)this.X, (int)this.Y].Region == TileRegion.Store_1)
                thelist = store1List;
            else if (Owner.Map[(int)this.X, (int)this.Y].Region == TileRegion.Store_2)
                thelist = store2List;
            else if (Owner.Map[(int)this.X, (int)this.Y].Region == TileRegion.Store_3)
                thelist = store3List;
            else if (Owner.Map[(int)this.X, (int)this.Y].Region == TileRegion.Store_4)
                thelist = store4List;
            else if (Owner.Map[(int)this.X, (int)this.Y].Region == TileRegion.Store_5)
                thelist = store5List;
            else if (Owner.Map[(int)this.X, (int)this.Y].Region == TileRegion.Store_6)
                thelist = store6List;
            else if (Owner.Map[(int)this.X, (int)this.Y].Region == TileRegion.Store_7)
                thelist = store7List;
            else if (Owner.Map[(int)this.X, (int)this.Y].Region == TileRegion.Store_8)
                thelist = store8List;
            else if (Owner.Map[(int)this.X, (int)this.Y].Region == TileRegion.Store_9)
                thelist = store9List;

            else if (Owner.Name == "Shop" && !custom)
                if (shopLists.ContainsKey(Owner.ExtraVar))
                {
                    shopLists.TryGetValue(Owner.ExtraVar, out thelist);
                }
                else
                {
                    thelist = new int[] { 0xa04, 0xa14, 0xa00, 0xa97, 0xa1a };
                }
            else
                thelist = new int[]{ mType };
            trueMType = thelist[new Random(Id).Next(0, thelist.Length)];
            stats[StatsType.MerchantMerchandiseType] = trueMType;
            stats[StatsType.MerchantRemainingCount] = mRemaining;
            stats[StatsType.MerchantRemainingMinute] = mTime;
            base.ExportStats(stats);
            if (prices.TryGetValue(trueMType, out truePrice) && !custom)
            {
                stats[StatsType.SellablePrice] = truePrice.Item1;
                stats[StatsType.SellablePriceCurrency] = truePrice.Item2;
            }
            else
            {
                stats[StatsType.SellablePrice] = Price;
                stats[StatsType.SellablePriceCurrency] = Currency;
                truePrice = new Tuple<int, CurrencyType>(Price, Currency);
            }
        }

        protected override void ImportStats(StatsType stats, object val)
        {
            if (stats == StatsType.MerchantMerchandiseType) mType = (int)val;
            else if (stats == StatsType.MerchantRemainingCount) mRemaining = (int)val;
            else if (stats == StatsType.MerchantRemainingMinute) mTime = (int)val;
            base.ImportStats(stats, val);
        }

        protected bool TryDeduct(Player player)
        {
            var acc = player.Client.Account;
            new Database().ReadStats(acc);
            if (!player.NameChosen) return false;

            if (truePrice.Item2 == CurrencyType.Fame)
            {
                if (acc.Stats.Fame < truePrice.Item1) return false;
                player.CurrentFame = acc.Stats.Fame = new Database().UpdateFame(acc, -truePrice.Item1);
                player.UpdateCount++;
                return true;
            }
            else if (truePrice.Item2 == CurrencyType.Gold)
            {
                if (acc.Credits < truePrice.Item1) return false;
                player.Credits = acc.Credits = new Database().UpdateCredit(acc, -truePrice.Item1);
                player.UpdateCount++;
                return true;
            }
            else if (truePrice.Item2 == CurrencyType.AccId)
            {
                if (truePrice.Item1 != player.Client.Account.AccountId) return false;
                return true;
            }
            return true;
        }

        public override void Buy(Player player)
        {
            if (ObjectType == 0x01ca)   //Merchant
            {
                if (TryDeduct(player))
                {
                    var Inventory = player.Inventory;
                    for (int i = 0; i < player.Inventory.Length; i++)
                    {
                        XElement ist;
                        XmlDatas.TypeToElement.TryGetValue((short)trueMType, out ist);
                        if (player.Inventory[i] == null && (player.SlotTypes[i] == 0 || player.SlotTypes[i] == Convert.ToInt16(ist.Element("SlotType").Value))) // Exploit fix - No more mnovas as weapons!
                        {
                            player.Inventory[i] = XmlDatas.ItemDescs[(short)trueMType];
                            player.UpdateCount++;
                            break;
                        }
                    }
                    player.Client.SendPacket(new BuyResultPacket()
                    {
                        Result = 0,
                        Message = "Purchase Successful!"
                    });
                    var ItemName = XmlDatas.ItemDescs[(short)trueMType].ObjectId;
                    var dir = @"logs";
                    if (!System.IO.Directory.Exists(dir))
                    System.IO.Directory.CreateDirectory(dir);

                    Console.WriteLine("[{3}]User {0} have bought {1} for {2}", player.nName, ItemName, truePrice, DateTime.Now);
                    using (System.IO.StreamWriter writer = new System.IO.StreamWriter(@"logs\PurchaseLog.txt", true))
                    {
                        writer.WriteLine("[{3}]User {0} have bought {1} for {2}", player.nName, ItemName, truePrice, DateTime.Now);
                    } 
                }
                else
                    switch (truePrice.Item2)
                    {
                        case CurrencyType.Gold:
                            player.Client.SendPacket(new BuyResultPacket()
                            {
                                Result = BUY_NO_GOLD,
                                Message = "Not enough gold!"
                            }); break;
                        case CurrencyType.Fame:
                            player.Client.SendPacket(new BuyResultPacket()
                            {
                                Result = BUY_NO_FAME,
                                Message = "Not enough fame!"
                            }); break;
                        case CurrencyType.AccId:
                            player.Client.SendPacket(new BuyResultPacket()
                            {
                                Result = BUY_NO_GOLD,
                                Message = "You are not ID #" + truePrice.Item1.ToString() + "!"
                            }); break;
                    }
            }
            base.Buy(player);
        }
    }
}
