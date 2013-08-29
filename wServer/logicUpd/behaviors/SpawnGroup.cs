using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wServer.realm;
using wServer.realm.entities;

namespace wServer.logic.behaviors
{
    class SpawnGroup : Behavior
    {
        //State storage: Spawn state
        class SpawnState
        {
            public int CurrentNumber;
            public int RemainingTime;
        }

        int maxChildren;
        int initialSpawn;
        Cooldown coolDown;
        ushort[] children;

        public SpawnGroup(string group, int maxChildren = 5, double initialSpawn = 0.5, Cooldown coolDown = new Cooldown())
        {
            this.children = BehaviorDb.InitGameData.ObjectDescs.Values
                .Where(x => x.Group == group)
                .Select(x => x.ObjectType).ToArray();
            this.maxChildren = maxChildren;
            this.initialSpawn = (int)(maxChildren * initialSpawn);
            this.coolDown = coolDown.Normalize(0);
        }

        protected override void OnStateEntry(Entity host, RealmTime time, ref object state)
        {
            state = new SpawnState()
            {
                CurrentNumber = initialSpawn,
                RemainingTime = coolDown.Next(Random)
            };
            for (int i = 0; i < initialSpawn; i++)
            {
                Entity entity = Entity.Resolve(host.Manager, children[Random.Next(children.Length)]);

                entity.Move(
                    host.X + (float)(Random.NextDouble() * 0.5),
                    host.Y + (float)(Random.NextDouble() * 0.5));
                (entity as Enemy).Terrain = (host as Enemy).Terrain;
                host.Owner.EnterWorld(entity);
            }
        }

        protected override void TickCore(Entity host, RealmTime time, ref object state)
        {
            SpawnState spawn = (SpawnState)state;

            if (spawn.RemainingTime <= 0 && spawn.CurrentNumber < maxChildren)
            {
                Entity entity = Entity.Resolve(host.Manager, children[Random.Next(children.Length)]);

                entity.Move(host.X, host.Y);
                (entity as Enemy).Terrain = (host as Enemy).Terrain;
                host.Owner.EnterWorld(entity);
                spawn.RemainingTime = coolDown.Next(Random);
                spawn.CurrentNumber++;
            }
            else
                spawn.RemainingTime -= time.thisTickTimes;
        }
    }
}
