using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace wServer.realm.worlds
{
    public class ShopMap : World
    {
        public ShopMap(string extra)
        {
            Id = SHOP;
            Name = "Shop";
            Background = 0;
            AllowTeleport = true;
            if (extra != "")
                ExtraVar = extra;
            else
                ExtraVar = "Default";
            base.FromWorldMap(typeof(RealmManager).Assembly.GetManifestResourceStream("wServer.realm.worlds.shop.wmap"));            
        }

        public override World GetInstance(ClientProcessor psr)
        {
            return RealmManager.AddWorld(new WineCellarMap());
        }
    }
}
