using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Ionic.Zlib;
using System.IO;

namespace terrain
{
    public class Json2Wmap
    {
        private struct obj
        {
            public string name;
            public string id;
        }
        private struct loc
        {
            public string ground;
            public obj[] objs;
            public obj[] regions;
        }
        private struct json_dat
        {
            public byte[] data;
            public int width;
            public int height;
            public loc[] dict;
        }

        public static void Convert(string from, string to)
        {
            var x = Convert(File.ReadAllText(from));
            File.WriteAllBytes(to, x);
        }
        public static byte[] Convert(string json)
        {
            var obj = JsonConvert.DeserializeObject<json_dat>(json);
            var dat = ZlibStream.UncompressBuffer(obj.data);

            Dictionary<short, TerrainTile> tileDict = new Dictionary<short, TerrainTile>();
            for (int i = 0; i < obj.dict.Length; i++)
            {
                var o = obj.dict[i];
                tileDict[(short)i] = new TerrainTile()
                {
                    TileId = o.ground == null ? (short)0xff : XmlDatas.IdToType[o.ground],
                    TileObj = o.objs == null ? null : o.objs[0].id,
                    Name = o.objs == null ? "" : o.objs[0].name ?? "",
                    Terrain = TerrainType.None,
                    Region = o.regions == null ? TileRegion.None : (TileRegion)Enum.Parse(typeof(TileRegion), o.regions[0].id.Replace(' ', '_'))
                };
            }

            var tiles = new TerrainTile[obj.width, obj.height];
            using (NReader rdr = new NReader(new MemoryStream(dat)))
                for (int y = 0; y < obj.height; y++)
                    for (int x = 0; x < obj.width; x++)
                    {
                        tiles[x, y] = tileDict[rdr.ReadInt16()];
                    }
            return WorldMapExporter.Export(tiles);
        }

        public static byte[] ConvertMakeWalls(string json)
        {
            var obj = JsonConvert.DeserializeObject<json_dat>(json);
            var dat = ZlibStream.UncompressBuffer(obj.data);

            Dictionary<short, TerrainTile> tileDict = new Dictionary<short, TerrainTile>();
            for (int i = 0; i < obj.dict.Length; i++)
            {
                var o = obj.dict[i];
                tileDict[(short)i] = new TerrainTile()
                {
                    TileId = o.ground == null ? (short)0xff : XmlDatas.IdToType[o.ground],
                    TileObj = o.objs == null ? null : o.objs[0].id,
                    Name = o.objs == null ? "" : o.objs[0].name ?? "",
                    Terrain = TerrainType.None,
                    Region = o.regions == null ? TileRegion.None : (TileRegion)Enum.Parse(typeof(TileRegion), o.regions[0].id.Replace(' ', '_'))
                };
            }

            var tiles = new TerrainTile[obj.width, obj.height];
            using (NReader rdr = new NReader(new MemoryStream(dat)))
                for (int y = 0; y < obj.height; y++)
                    for (int x = 0; x < obj.width; x++)
                    {
                        tiles[x, y] = tileDict[rdr.ReadInt16()];
                        tiles[x, y].X = x;
                        tiles[x, y].Y = y;
                    }

            foreach (var i in tiles)
            {
                if (i.TileId == 0xff && i.TileObj == null)
                {
                    bool createWall = false;
                    for (int ty = -1; ty <= 1; ty++)
                        for (int tx = -1; tx <= 1; tx++)
                            try
                            {
                                if (tiles[i.X + tx, i.Y + ty].TileId != 0xff)
                                    createWall = true;
                            }
                            catch { }
                    if (createWall)
                        tiles[i.X, i.Y].TileObj = "Grey Wall";
                }
            }

            return WorldMapExporter.Export(tiles);
        }
    }
}
