using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wServer.realm;
using wServer.realm.entities;
using wServer.svrPackets;
using Mono.Game;

namespace wServer.logic.movement
{
    class Chasing : Behavior
    {
        float speed;
        float radius;
        float targetRadius;
        short? objType;
        private Chasing(float speed, float radius, float targetRadius, short? objType)
        {
            this.speed = speed;
            this.radius = radius;
            this.targetRadius = targetRadius;
            this.objType = objType;
        }
        static readonly Dictionary<Tuple<float, float, float, short?>, Chasing> instances = new Dictionary<Tuple<float, float, float, short?>, Chasing>();
        public static Chasing Instance(float speed, float radius, float targetRadius, short? objType)
        {
            var key = new Tuple<float, float, float, short?>(speed, radius, targetRadius, objType);
            Chasing ret;
            if (!instances.TryGetValue(key, out ret))
                ret = instances[key] = new Chasing(speed, radius, targetRadius, objType);
            return ret;
        }

        Random rand = new Random();
        protected override bool TickCore(RealmTime time)
        {
            if (Host.Self.HasConditionEffect(ConditionEffects.Paralyzed)) return true;
            var speed = this.speed * GetSpeedMultiplier(Host.Self);

            float dist = radius;
            Entity entity = GetNearestEntity(ref dist, objType);
            if (entity != null && dist > targetRadius)
            {
                var tx = entity.X + rand.Next(-2, 2) / 2f;
                var ty = entity.Y + rand.Next(-2, 2) / 2f;
                if (tx != Host.Self.X || ty != Host.Self.Y)
                {
                    var x = Host.Self.X;
                    var y = Host.Self.Y;
                    Vector2 vect = new Vector2(tx, ty) - new Vector2(Host.Self.X, Host.Self.Y);
                    vect.Normalize();
                    vect *= (speed / 1.5f) * (time.thisTickTimes / 1000f);
                    ValidateAndMove(Host.Self.X + vect.X, Host.Self.Y + vect.Y);
                    Host.Self.UpdateCount++;
                }
                return true;
            }
            else return false;
        }
    }

    class PetChasing : Behavior
    {
        float speed;
        float radius;
        float targetRadius;
        private PetChasing(float speed, float radius, float targetRadius)
        {
            this.speed = speed;
            this.radius = radius;
            this.targetRadius = targetRadius;
        }
        static readonly Dictionary<Tuple<float, float, float>, PetChasing> instances = new Dictionary<Tuple<float, float, float>, PetChasing>();
        public static PetChasing Instance(float speed, float radius, float targetRadius)
        {
            var key = new Tuple<float, float, float>(speed, radius, targetRadius);
            PetChasing ret;
            if (!instances.TryGetValue(key, out ret))
                ret = instances[key] = new PetChasing(speed, radius, targetRadius);
            return ret;
        }

        Random rand = new Random();
        protected override bool TickCore(RealmTime time)
        {
            if (Host.Self.HasConditionEffect(ConditionEffects.Paralyzed)) return true;
            var speed = this.speed * GetSpeedMultiplier(Host.Self);

            float dist = radius;
            Entity entity = Host.Self.PlayerOwner;
            if (entity != null && dist > targetRadius)
            {
                var tx = entity.X + rand.Next(-2, 2) / 2f;
                var ty = entity.Y + rand.Next(-2, 2) / 2f;
                if (tx != Host.Self.X || ty != Host.Self.Y)
                {
                    var x = Host.Self.X;
                    var y = Host.Self.Y;
                    Vector2 vect = new Vector2(tx, ty) - new Vector2(Host.Self.X, Host.Self.Y);
                    vect.Normalize();
                    vect *= (speed / 1.5f) * (time.thisTickTimes / 1000f);
                    ValidateAndMove(Host.Self.X + vect.X, Host.Self.Y + vect.Y);
                    Host.Self.UpdateCount++;
                }
                return true;
            }
            else return false;
        }
    }

    class PetBehaves : Behavior
    {
        public PetBehaves()
        {
        }

        Random rand = new Random();
        protected override bool TickCore(RealmTime time)
        {
            if (Host.Self.PlayerOwner != null)
            {
                if (Host.Self.PlayerOwner.Owner != null)
                {
                    var distance = Vector2.Distance(new Vector2(Host.Self.X, Host.Self.Y), new Vector2(Host.Self.PlayerOwner.X, Host.Self.PlayerOwner.Y));
                    if (distance > 12)
                    {
                        Host.Self.Move(Host.Self.PlayerOwner.X, Host.Self.PlayerOwner.Y);
                    }
                    if (distance >= 1)
                        return true;
                    else
                        return false;
                }
            }
            var enemy = Host as Enemy;
            enemy.DamageCounter.Death();
            foreach (var i in enemy.CondBehaviors)
                if ((i.Condition & BehaviorCondition.OnDeath) != 0)
                    i.Behave(BehaviorCondition.OnDeath, Host, null, enemy.DamageCounter);
            try
            {
                enemy.Owner.LeaveWorld(enemy);
            }
            catch
            {
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.Out.WriteLine("Crash halted! - Nonexistent entity tried to die!");
                Console.ForegroundColor = ConsoleColor.White;
            }
            return false;
        }
    }
}
