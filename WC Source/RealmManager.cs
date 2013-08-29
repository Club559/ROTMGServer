using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using db;
using System.Threading;
using System.Diagnostics;
using System.IO;
using wServer.realm.worlds;
using System.Collections.Concurrent;

namespace wServer.realm
{
    public struct RealmTime
    {
        public long tickCount;
        public long tickTimes;
        public int thisTickCounts;
        public int thisTickTimes;
    }

    public enum PendingPriority
    {
        Emergent,
        Destruction,
        Normal,
        Creation,
    }

    public static class RealmManager
    {
        static RealmManager()
        {
            Worlds[World.TUT_ID] = new Tutorial(true);
            Worlds[World.NEXUS_ID] = Worlds[0] = new Nexus();
            Worlds[World.NEXUS_LIMBO] = new NexusLimbo();
            Worlds[World.VAULT_ID] = new Vault(true);
            Worlds[World.TEST_ID] = new Test();
            Worlds[World.RAND_REALM] = new RandomRealm();
            Worlds[World.GAUNTLET] = new GauntletMap();
            Worlds[World.WC] = new WineCellarMap();

            Monitor = new RealmPortalMonitor(Worlds[World.NEXUS_ID] as Nexus);

            AddWorld(new GameWorld(1, "Medusa", true));
        }

        public const int MAX_CLIENT = 100;

        static int nextWorldId = 0;
        public static readonly ConcurrentDictionary<int, World> Worlds = new ConcurrentDictionary<int, World>();
        public static readonly ConcurrentDictionary<int, ClientProcessor> Clients = new ConcurrentDictionary<int, ClientProcessor>();
        public static ConcurrentDictionary<int, World> PlayerWorldMapping = new ConcurrentDictionary<int, World>();

        public static RealmPortalMonitor Monitor { get; private set; }

        public static bool TryConnect(ClientProcessor psr)
        {
            if (Clients.Count >= MAX_CLIENT)
                return false;
            else
                return Clients.TryAdd(psr.Account.AccountId, psr);
        }
        public static void Disconnect(ClientProcessor psr)
        {
            Clients.TryRemove(psr.Account.AccountId, out psr);
        }

        public static World AddWorld(World world)
        {
            world.Id = Interlocked.Increment(ref nextWorldId);
            Worlds[world.Id] = world;
            if (world is GameWorld)
                Monitor.WorldAdded(world);
            return world;
        }
        public static World GetWorld(int id)
        {
            World ret;
            if (!Worlds.TryGetValue(id, out ret)) return null;
            if (ret.Id == 0) return null;
            return ret;
        }

        static Thread network;
        public static NetworkTicker Network { get; private set; }

        static Thread logic;
        public static LogicTicker Logic { get; private set; }

        public static void CoreTickLoop()
        {
            Network = new NetworkTicker();
            Logic = new LogicTicker();
            network = new Thread(Network.TickLoop);
            logic = new Thread(Logic.TickLoop);
            //Start logic loop first
            logic.Start();
            network.Start();
            Thread.CurrentThread.Join();
        }
    }
}
