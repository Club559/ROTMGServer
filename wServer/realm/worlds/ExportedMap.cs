using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using terrain;

namespace wServer.realm.worlds
{
    public class ExportedMap : World
    {
        public string js = null;
        public string name = "Test Map";

        public ExportedMap(string json, string n)
        {
            this.js = json;
            this.name = n;
            Name = n;
            Background = 0;
            AllowTeleport = true;
            base.FromWorldMap(new MemoryStream(Json2Wmap.Convert(json)));
        }

        public override World GetInstance(ClientProcessor psr)
        {
            return RealmManager.AddWorld(new ExportedMap(js, name));
        }                 
    }
}
