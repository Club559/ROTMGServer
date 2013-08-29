using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wServer.realm.entities;
using wServer.realm;
using Mono.Game;

namespace wServer.logic.behaviors
{
    class Orbit : CycleBehavior
    {
        //State storage: orbit state
        class OrbitState
        {
            public float Speed;
            public float Radius;
        }

        float speed;
        float acquireRange;
        float radius;
        ushort? target;
        float speedVariance;
        float radiusVariance;
        public Orbit(double speed, double radius, double acquireRange = 10,
            string target = null, double? speedVariance = null, double? radiusVariance = null)
        {
            this.speed = (float)speed;
            this.radius = (float)radius;
            this.acquireRange = (float)acquireRange;
            this.target = target == null ? null : (ushort?)BehaviorDb.InitGameData.IdToObjectType[target];
            this.speedVariance = (float)(speedVariance ?? speed * 0.1);
            this.radiusVariance = (float)(radiusVariance ?? speed * 0.1);
        }

        protected override void OnStateEntry(realm.Entity host, realm.RealmTime time, ref object state)
        {
            state = new OrbitState()
            {
                Speed = speed + speedVariance * (float)(Random.NextDouble() * 2 - 1),
                Radius = radius + radiusVariance * (float)(Random.NextDouble() * 2 - 1)
            };
        }

        protected override void TickCore(Entity host, RealmTime time, ref object state)
        {
            OrbitState s = (OrbitState)state;

            Status = CycleStatus.NotStarted;

            if (host.HasConditionEffect(ConditionEffects.Paralyzed)) return;

            var entity = host.GetNearestEntity(acquireRange, target);
            if (entity != null)
            {
                double angle;
                if (host.Y == entity.Y && host.X == entity.X)//small offset
                    angle = Math.Atan2(host.Y - entity.Y + (Random.NextDouble() * 2 - 1), host.X - entity.X + (Random.NextDouble() * 2 - 1));
                else
                    angle = Math.Atan2(host.Y - entity.Y, host.X - entity.X);
                var angularSpd = host.GetSpeed(s.Speed) / s.Radius;
                angle += angularSpd * (time.thisTickTimes / 1000f);

                double x = entity.X + Math.Cos(angle) * radius;
                double y = entity.Y + Math.Sin(angle) * radius;
                Vector2 vect = new Vector2((float)x, (float)y) - new Vector2(host.X, host.Y);
                vect.Normalize();
                vect *= host.GetSpeed(s.Speed) * (time.thisTickTimes / 1000f);

                host.ValidateAndMove(host.X + vect.X, host.Y + vect.Y);
                host.UpdateCount++;

                Status = CycleStatus.InProgress;
            }

            state = s;
        }
    }
}
