using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wServer.realm;
using wServer.realm.entities;
using wServer.networking.svrPackets;

namespace wServer.logic.behaviors
{
    class Shoot : CycleBehavior
    {
        //State storage: cooldown timer

        double radius;
        int count;
        double shootAngle;
        double? fixedAngle;
        double angleOffset;
        double? defaultAngle;
        double predictive;
        int projectileIndex;
        int coolDownOffset;
        Cooldown coolDown;

        public Shoot(double radius, int count = 1, double? shootAngle = null,
            int projectileIndex = 0, double? fixedAngle = null,
            double angleOffset = 0, double? defaultAngle = null,
            double predictive = 0, int coolDownOffset = 0, 
            Cooldown coolDown = new Cooldown())
        {
            this.radius = radius;
            this.count = count;
            this.shootAngle = count == 1 ? 0 : (shootAngle ?? 360.0 / count) * Math.PI / 180;
            this.fixedAngle = fixedAngle * Math.PI / 180;
            this.angleOffset = angleOffset * Math.PI / 180;
            this.defaultAngle = defaultAngle * Math.PI / 180;
            this.projectileIndex = projectileIndex;
            this.predictive = predictive;
            this.coolDownOffset = coolDownOffset;
            this.coolDown = coolDown.Normalize();
        }

        protected override void OnStateEntry(Entity host, RealmTime time, ref object state)
        {
            state = coolDownOffset;
        }

        static double Predict(Entity host, Entity target, ProjectileDesc desc)
        {
            Position? history = target.TryGetHistory(100);
            if (history == null)
                return 0;

            var originalAngle = Math.Atan2(history.Value.Y - host.Y, history.Value.X - host.X);
            var newAngle = Math.Atan2(target.Y - host.Y, target.X - host.X);


            var bulletSpeed = desc.Speed / 100f;
            var dist = target.Dist(host);
            var angularVelo = (newAngle - originalAngle) / (100 / 1000f);
            return angularVelo * bulletSpeed;
        }

        protected override void TickCore(Entity host, RealmTime time, ref object state)
        {
            int cool = (int)state;
            Status = CycleStatus.NotStarted;

            if (cool <= 0)
            {
                if (host.HasConditionEffect(ConditionEffects.Stunned)) return;

                Entity player = host.GetNearestEntity(radius, null);
                if (player != null || defaultAngle != null || fixedAngle != null)
                {
                    var desc = host.ObjectDesc.Projectiles[projectileIndex];

                    var a = fixedAngle ?? (player == null ? defaultAngle.Value : Math.Atan2(player.Y - host.Y, player.X - host.X));
                    a += angleOffset;
                    if (predictive != 0 && player != null)
                        a += Predict(host, player, desc) * predictive;

                    int dmg;
                    if (host is Character)
                        dmg = (host as Character).Random.Next(desc.MinDamage, desc.MaxDamage);
                    else
                        dmg = Random.Next(desc.MinDamage, desc.MaxDamage);

                    var startAngle = a - shootAngle * (count - 1) / 2;
                    byte prjId = 0;
                    Position prjPos = new Position() { X = host.X, Y = host.Y };
                    for (int i = 0; i < count; i++)
                    {
                        var prj = host.CreateProjectile(
                            desc, host.ObjectType, dmg, time.tickTimes,
                            prjPos, (float)(startAngle + shootAngle * i));
                        host.Owner.EnterWorld(prj);
                        if (i == 0)
                            prjId = prj.ProjectileId;
                    }

                    host.Owner.BroadcastPacket(new MultiShootPacket()
                    {
                        BulletId = prjId,
                        OwnerId = host.Id,
                        Position = prjPos,
                        Angle = (float)startAngle,
                        Damage = (short)dmg,
                        BulletType = (byte)(desc.BulletType),
                        AngleIncrement = (float)shootAngle,
                        NumShots = (byte)count,
                    }, null);
                }
                cool = coolDown.Next(Random);
                Status = CycleStatus.Completed;
            }
            else
            {
                cool -= time.thisTickTimes;
                Status = CycleStatus.InProgress;
            }

            state = cool;
        }
    }
}
