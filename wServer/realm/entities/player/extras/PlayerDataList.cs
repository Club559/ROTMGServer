using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;

namespace wServer.realm.entities.player
{
    public class PlayerDataList
    {
        public static ConcurrentDictionary<string, GlobalPlayerData> Datas = new ConcurrentDictionary<string, GlobalPlayerData>();

        public static GlobalPlayerData GetData(string name)
        {
            if (!PlayerDataList.Datas.IsEmpty)
            {
                foreach (var i in PlayerDataList.Datas)
                {
                    if (i.Key == name)
                    {
                        return i.Value;
                    }
                }
            }
            GlobalPlayerData n = new GlobalPlayerData();
            Datas.TryAdd(name, n);
            return n;
        }
    }
}