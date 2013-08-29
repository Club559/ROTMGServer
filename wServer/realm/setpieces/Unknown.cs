using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wServer.realm.entities;
using wServer.logic.loot;

namespace wServer.realm.setpieces
{
    class Unknown : ISetPiece
    {
        public int Size
        {
            get { return 81; }
        }

        static readonly byte[,] Center = new byte[,]
        {
            { 0, 0, 1, 1, 1, 0, 0, 0, 1, 0, 0, 0, 1, 1, 1, 0, 0 },
            { 0, 1, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 1, 0 },
            { 1, 1, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 1, 1 },
            { 1, 0, 0, 1, 1, 1, 0, 1, 0, 1, 0, 1, 1, 1, 0, 0, 1 },
            { 1, 0, 0, 1, 1, 0, 0, 1, 0, 1, 0, 0, 1, 1, 0, 0, 1 },
            { 0, 0, 1, 1, 0, 0, 1, 1, 1, 1, 1, 0, 0, 1, 1, 0, 0 },
            { 0, 0, 1, 0, 0, 1, 1, 1, 0, 1, 1, 1, 0, 0, 1, 0, 0 },
            { 0, 0, 1, 1, 1, 1, 1, 0, 0, 0, 1, 1, 1, 1, 1, 0, 0 },
            { 1, 1, 1, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 1, 1, 1 },
            { 0, 0, 1, 1, 1, 1, 1, 0, 0, 0, 1, 1, 1, 1, 1, 0, 0 },
            { 0, 0, 1, 0, 0, 1, 1, 1, 0, 1, 1, 1, 0, 0, 1, 0, 0 },
            { 0, 0, 1, 1, 0, 0, 1, 1, 1, 1, 1, 0, 0, 1, 1, 0, 0 },
            { 1, 0, 0, 1, 1, 0, 0, 1, 0, 1, 0, 0, 1, 1, 0, 0, 1 },
            { 1, 0, 0, 1, 1, 1, 0, 1, 0, 1, 0, 1, 1, 1, 0, 0, 1 },
            { 1, 1, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 1, 1 },
            { 0, 1, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 1, 0 },
            { 0, 0, 1, 1, 1, 0, 0, 0, 1, 0, 0, 0, 1, 1, 1, 0, 0 },
        };

        static readonly byte Floor = (byte)XmlDatas.IdToType["Dark Cobblestone"];
        static readonly byte Central = (byte)XmlDatas.IdToType["Light Cobblestone"];
        static readonly short Pillar = XmlDatas.IdToType["Unknown Cryptic Wall"];

        Random rand = new Random();
        public void RenderSetPiece(World world, IntPoint pos)
        {
            int[,] t = new int[81, 81];
            for (int x = 0; x < Size; x++)
                for (int y = 0; y < Size; y++)
                {
                    double dx = x - (Size / 2.0);
                    double dy = y - (Size / 2.0);
                    double r = Math.Sqrt(dx * dx + dy * dy) + rand.NextDouble() * 4 - 2;
                    if (r <= 35)
                        t[x, y] = 1;
                }
            
            for (int x = 0; x < 17; x++)
                for (int y = 0; y < 17; y++)
                {
                    if (Center[x, y] != 0)
                        t[32 + x, 32 + y] = 2;
                }

            t[36, 36] = t[44, 36] = t[36, 44] = t[44, 44] = 3; //Pillars (Solo Standing)

            t[30, 30] = t[50, 30] = t[30, 50] = t[50, 50] = 5; //Pillars (Outer Shape)
            t[29, 33] = t[47, 29] = t[51, 47] = t[33, 51] = 5;
            t[32, 29] = t[48, 51] = t[51, 32] = t[29, 48] = 5; // messy
            t[29, 31] = t[49, 29] = t[51, 49] = t[31, 51] = 5;
            t[30, 31] = t[49, 30] = t[50, 49] = t[31, 50] = 5;
            t[33, 29] = t[51, 33] = t[47, 51] = t[29, 47] = 5;
            t[29, 32] = t[51, 48] = t[48, 29] = t[32, 51] = 5; // messy
            t[31, 29] = t[51, 31] = t[49, 51] = t[29, 49] = 5;
            t[31, 30] = t[50, 31] = t[49, 50] = t[30, 49] = 5;

            t[40, 26] = t[40, 27] = t[39, 27] = t[41, 27] = 4; //Pillars (T Shape)
            t[40, 54] = t[40, 53] = t[39, 53] = t[41, 53] = 4;
            t[26, 40] = t[27, 40] = t[27, 39] = t[27, 41] = 4;
            t[54, 40] = t[53, 40] = t[53, 39] = t[53, 41] = 4;

            for (int x = 0; x < Size; x++)                      //Rendering
                for (int y = 0; y < Size; y++)
                    if (t[x, y] == 1)
                    {
                        var tile = world.Map[x + pos.X, y + pos.Y].Clone();
                        tile.TileId = Floor; tile.ObjType = 0;
                        world.Obstacles[x + pos.X, y + pos.Y] = 0;
                        world.Map[x + pos.X, y + pos.Y] = tile;
                    }
                    else if (t[x, y] == 2)
                    {
                        var tile = world.Map[x + pos.X, y + pos.Y].Clone();
                        tile.TileId = Central; tile.ObjType = 0;
                        world.Obstacles[x + pos.X, y + pos.Y] = 0;
                        world.Map[x + pos.X, y + pos.Y] = tile;
                    }
                    else if (t[x, y] == 3)
                    {
                        var tile = world.Map[x + pos.X, y + pos.Y].Clone();
                        tile.TileId = Central;
                        tile.ObjType = Pillar;
                        tile.Name = ConnectionComputer.GetConnString((_x, _y) => t[x + _x, y + _y] == 3);
                        if (tile.ObjId == 0) tile.ObjId = world.GetNextEntityId();
                        world.Obstacles[x + pos.X, y + pos.Y] = 2;
                        world.Map[x + pos.X, y + pos.Y] = tile;
                    }
                    else if (t[x, y] == 4)
                    {
                        var tile = world.Map[x + pos.X, y + pos.Y].Clone();
                        tile.TileId = Floor;
                        tile.ObjType = Pillar;
                        tile.Name = ConnectionComputer.GetConnString((_x, _y) => t[x + _x, y + _y] == 4);
                        if (tile.ObjId == 0) tile.ObjId = world.GetNextEntityId();
                        world.Obstacles[x + pos.X, y + pos.Y] = 2;
                        world.Map[x + pos.X, y + pos.Y] = tile;
                    }
                    else if (t[x, y] == 5)
                    {
                        var tile = world.Map[x + pos.X, y + pos.Y].Clone();
                        tile.TileId = Floor;
                        tile.ObjType = Pillar;
                        tile.Name = ConnectionComputer.GetConnString((_x, _y) => t[x + _x, y + _y] == 5);
                        if (tile.ObjId == 0) tile.ObjId = world.GetNextEntityId();
                        world.Obstacles[x + pos.X, y + pos.Y] = 2;
                        world.Map[x + pos.X, y + pos.Y] = tile;
                    }

            Entity unknown = Entity.Resolve(0x0f02);
            unknown.Move(pos.X + 40.5f, pos.Y + 40.5f);
            world.EnterWorld(unknown);
        }
    }
}
