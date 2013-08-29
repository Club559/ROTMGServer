using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using terrain;

namespace wServer.realm.worlds
{
    public class Test : World
    {
        public string js = null;

        public Test()
        {
            Id = TEST_ID;
            Name = "Test";
            Background = 0;
            //Mining = true;
        }

        public void LoadJson(string json)
        {
            js = json;
            FromWorldMap(new MemoryStream(Json2Wmap.Convert(json)));
        }                      
    }
}
