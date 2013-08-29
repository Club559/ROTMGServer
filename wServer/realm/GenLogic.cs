using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wServer.realm.entities;

namespace wServer.realm
{
    public static class GenLogic
    {

        public static bool GenRandomRoom(World world, float x, float y, Wall theWall)
        {
            try
            {
                Random rand = new Random();
                if (rand.Next(1, 60) != 1)
                    return false;
                //Console.Out.WriteLine("Generating room...");
                List<string> dirs = new List<string>();
                for (int tx = -1; tx <= 1; tx++)
                    for (int ty = -1; ty <= 1; ty++)
                    {
                        WmapTile targetTile = world.Map[(int)x + tx, (int)y + ty];
                        WmapTile thisTile = world.Map[(int)x, (int)y];
                        if (targetTile.TileId == 0xff)
                        {
                            if (tx == -1 && ty == 0)
                                dirs.Add("left");
                            else if (tx == 1 && ty == 0)
                                dirs.Add("right");
                            else if (tx == 0 && ty == 1)
                                dirs.Add("down");
                            else if (tx == 0 && ty == -1)
                                dirs.Add("up");
                        }
                    }
                if(dirs.Count < 1)
                    return false;
                dirs.Shuffle();
                //Console.Out.WriteLine("Room direction: " + dirs.First());
                float mainX = x;
                float mainY = y;
                float entranceX = x;
                float entranceY = y;
                switch (dirs.First())
                {
                    case "up":
                        mainX = x - 6; mainY = y - 8;
                        entranceY = y - 1; break;
                    case "down":
                        mainX = x - 6; mainY = y + 1;
                        entranceY = y + 1; break;
                    case "left":
                        mainX = x - 12; mainY = y - 3;
                        entranceX = x - 1; break;
                    case "right":
                        mainX = x + 1; mainY = y - 3;
                        entranceX = x + 1; break;
                }
                List<WmapTile> addedTiles = new List<WmapTile>();
                for(int ty = (int)mainY; ty <= mainY + 7; ty++)
                    for (int tx = (int)mainX; tx <= mainX + 11; tx++)
                    {
                        WmapTile tTile = world.Map[tx, ty];
                        if (tTile.TileId != 0xff || tTile.ObjType != 0)
                        {
                            //Console.Out.WriteLine("Found collision while generating room!");
                            return false;
                        }
                        tTile.TileId = world.Map[(int)x, (int)y].TileId;
                        addedTiles.Add(tTile);
                    }
                //Console.Out.WriteLine("Generated tiles, placing...");
                int tileNum = 0;
                for(int ty = (int)mainY; ty <= mainY + 7; ty++)
                    for (int tx = (int)mainX; tx <= mainX + 11; tx++)
                    {
                        WmapTile ctile = addedTiles[tileNum];
                        if ((tx == (int)mainX || tx == (int)mainX + 11 || ty == (int)mainY || ty == (int)mainY + 7) && !(tx == entranceX && ty == entranceY))
                        {
                            //Console.Out.WriteLine("Placed wall");
                            Wall e = new Wall(theWall.ObjectType, XmlDatas.TypeToElement[theWall.ObjectType]);
                            e.Move(tx, ty);
                            world.EnterWorld(e);
                            ctile.ObjType = theWall.ObjectType;
                        }
                        else
                        {
                            //Console.Out.WriteLine("Placed treasure");
                            if (rand.Next(1, 30) == 1)
                            {
                                Entity e = Entity.Resolve(XmlDatas.IdToType["Coral Gift"]);
                                e.Move(tx + 0.5f, ty + 0.5f);
                                world.EnterWorld(e);
                                ctile.ObjType = XmlDatas.IdToType["Coral Gift"];
                            }
                        }
                        world.Map[tx, ty] = ctile;
                    }
                //Console.Out.WriteLine("Placed tiles!");
                return true;
            }
            catch(Exception e)
            {
                return false;
            }
        }
    }
}
