using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wServer.realm;
using wServer.realm.entities;

namespace wServer.logic
{
    class DeathTransmute : ConditionalBehavior
    {
        short objType;
        int minCount;
        int maxCount;
        public DeathTransmute(short objType, int minCount = 1, int maxCount = 1)
        {
            this.objType = objType;
            this.minCount = minCount;
            this.maxCount = maxCount;
        }

        public override BehaviorCondition Condition
        {
            get { return BehaviorCondition.OnDeath; }
        }

        Random rand = new Random();
        protected override void BehaveCore(BehaviorCondition cond, realm.RealmTime? time, object state)
        {
            int c = rand.Next(minCount, maxCount + 1);
            for (int i = 0; i < c; i++)
            {
                Entity entity = Entity.Resolve(objType);
                Entity parent = Host as Entity;
                entity.Move(parent.X, parent.Y);
                (entity as Enemy).Terrain = (Host as Enemy).Terrain;
                parent.Owner.EnterWorld(entity);
            }
        }
    }

    class DeathPortal : ConditionalBehavior
    {
        short objType;
        int percent;
        public DeathPortal(short objType, int percent)
        {
            this.objType = objType;
            this.percent = percent;
        }

        public override BehaviorCondition Condition
        {
            get { return BehaviorCondition.OnDeath; }
        }

        protected override void BehaveCore(BehaviorCondition cond, realm.RealmTime? time, object state)
        {
            if (new Random().Next(1, 100) <= percent)
            {
                Portal entity = Entity.Resolve(objType) as Portal;
                Entity parent = Host as Entity;
                entity.Move(parent.X, parent.Y);
                parent.Owner.EnterWorld(entity);
                World w = RealmManager.GetWorld(Host.Self.Owner.Id);
                w.Timers.Add(new WorldTimer(30 * 1000, (world, t) => //default portal close time * 1000
                {
                    try
                    {
                        w.LeaveWorld(entity);
                    }
                    catch //couldn't remove portal, Owner became null. Should be fixed with RealmManager implementation
                    {
                        Console.Out.WriteLine("Couldn't despawn portal.");
                    }
                }));
            }
        }
    }

    class Corpse : ConditionalBehavior
    {
        short objType;
        public Corpse(short objType)
        {
            this.objType = objType;
        }

        public override BehaviorCondition Condition
        {
            get { return BehaviorCondition.OnDeath; }
        }

        protected override void BehaveCore(BehaviorCondition cond, realm.RealmTime? time, object state)
        {
            Enemy entity = Entity.Resolve(objType) as Enemy;
            Enemy parent = Host as Enemy;
            entity.Move(parent.X, parent.Y);
            entity.Terrain = (Host as Enemy).Terrain;
            parent.DamageCounter.Corpse = entity.DamageCounter;
            parent.Owner.EnterWorld(entity);
        }
    }
}
