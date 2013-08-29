using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wServer.realm;
using wServer.logic.attack;
using wServer.logic.movement;
using wServer.logic.loot;
using wServer.logic.taunt;
using wServer.logic.cond;

namespace wServer.logic
{
    partial class BehaviorDb
    {
        static _ UndeadLair = Behav()
            .Init(0x0d90, Behaves("Septavius the Ghost God",
                new RunBehaviors(
                  SmoothWandering.Instance(0.5f, 0.5f),
                  Once.Instance(SpawnMinionImmediate.Instance(0x0db0, 0, 4, 6)),
                  Once.Instance(SpawnMinionImmediate.Instance(0x0db1, 0, 4, 6)),
                  Once.Instance(SpawnMinionImmediate.Instance(0x0db2, 0, 4, 6))
                ),
                Once.Instance(IsEntityPresent.Instance(1, null)),
                new QueuedBehavior(
        #region Circle Attack 1

                    Cooldown.Instance(150),
                      new RunBehaviors(
                          SimpleAttack.Instance(5, projectileIndex: 3),
                          RingAttack.Instance(3, offset: 0 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 18 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 36 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 54 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 72 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          SetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),
                          SimpleAttack.Instance(5, projectileIndex: 3),
                          RingAttack.Instance(3, offset: 90 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 108 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 126 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 144 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 162 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          SimpleAttack.Instance(5, projectileIndex: 3),
                          RingAttack.Instance(3, offset: 180 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 198 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 216 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 234 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 252 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          SimpleAttack.Instance(5, projectileIndex: 3),
                          RingAttack.Instance(3, offset: 270 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 288 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 306 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 324 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 342 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(250),
                      new RunBehaviors(
                          UnsetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),
                          RingAttack.Instance(3, offset: 360 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(100),
        #endregion

        #region Circle Attack 2
 Cooldown.Instance(150),
                      new RunBehaviors(
                          SimpleAttack.Instance(5, projectileIndex: 3),
                          RingAttack.Instance(3, offset: 0 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 18 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 36 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 54 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 72 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          SetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),
                          SimpleAttack.Instance(5, projectileIndex: 3),
                          RingAttack.Instance(3, offset: 90 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 108 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 126 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 144 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 162 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          SimpleAttack.Instance(5, projectileIndex: 3),
                          RingAttack.Instance(3, offset: 180 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 198 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 216 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 234 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 252 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          SimpleAttack.Instance(5, projectileIndex: 3),
                          RingAttack.Instance(3, offset: 270 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          UnsetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),
                          RingAttack.Instance(3, offset: 288 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 306 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 324 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 342 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(250),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 360 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(100),
        #endregion
            //not used
        #region Circle Attack 3
            /*Cooldown.Instance(150),
                      new RunBehaviors(
                          SimpleAttack.Instance(5, projectileIndex: 3),
                          RingAttack.Instance(3, offset: 0 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 18 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 36 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 54 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 72 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          SimpleAttack.Instance(5, projectileIndex: 3),
                          RingAttack.Instance(3, offset: 90 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 108 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 126 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 144 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 162 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          SimpleAttack.Instance(5, projectileIndex: 3),
                          RingAttack.Instance(3, offset: 180 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 198 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 216 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 234 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 252 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          SimpleAttack.Instance(5, projectileIndex: 3),
                          RingAttack.Instance(3, offset: 270 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 288 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 306 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 324 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 342 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(250),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 360 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(100),*/
        #endregion

        #region Circle Attack 4
            /*Cooldown.Instance(150),
                      new RunBehaviors(
                          SimpleAttack.Instance(5, projectileIndex: 3),
                          RingAttack.Instance(3, offset: 0 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 18 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 36 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 54 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 72 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          SimpleAttack.Instance(5, projectileIndex: 3),
                          RingAttack.Instance(3, offset: 90 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 108 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 126 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 144 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 162 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          SimpleAttack.Instance(5, projectileIndex: 3),
                          RingAttack.Instance(3, offset: 180 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 198 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 216 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 234 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 252 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          SimpleAttack.Instance(5, projectileIndex: 3),
                          RingAttack.Instance(3, offset: 270 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 288 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 306 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 324 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 342 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(250),
                      new RunBehaviors(
                          SimpleAttack.Instance(5, projectileIndex: 3),
                          RingAttack.Instance(3, offset: 360 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(100),*/
        #endregion
            //end

        #region RingAttack + Flashing 1

                    new QueuedBehavior(
                        SetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),
                        Flashing.Instance(500, 0x0000FF0C),
                        Flashing.Instance(500, 0x0000FF0C),
                        Flashing.Instance(500, 0x0000FF0C),
                        Flashing.Instance(500, 0x0000FF0C),
                        Cooldown.Instance(2500, RingAttack.Instance(12, 10, 12, projectileIndex: 3)),
                        Cooldown.Instance(2500, RingAttack.Instance(12, 10, 12, projectileIndex: 3)),
                        Cooldown.Instance(2500, RingAttack.Instance(12, 10, 12, projectileIndex: 3)),
                        Cooldown.Instance(2500, RingAttack.Instance(12, 10, 12, projectileIndex: 3)),

        #endregion

        #region Flashing 2

 Flashing.Instance(200, 0x0000FF0C),
                        Flashing.Instance(200, 0x0000FF0C),
                        Flashing.Instance(200, 0x0000FF0C),
                        Flashing.Instance(200, 0x0000FF0C),
                        Flashing.Instance(200, 0x0000FF0C),
                        Flashing.Instance(200, 0x0000FF0C),
                        Flashing.Instance(200, 0x0000FF0C),
                        Flashing.Instance(200, 0x0000FF0C),
                        UnsetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),

        #endregion

        #region Quite + Confuse
            //confuse
                        MultiAttack.Instance(10, 1 * (float)Math.PI / 30, 3, 0, projectileIndex: 2),
            //end confuse
            //quite
                        Cooldown.Instance(500, RingAttack.Instance(10, 10, offset: 0 * (float)Math.PI / 180, projectileIndex: 1)),
                        Cooldown.Instance(500, RingAttack.Instance(10, 10, offset: 90 * (float)Math.PI / 180, projectileIndex: 1)),
                        Cooldown.Instance(500, RingAttack.Instance(10, 10, offset: 180 * (float)Math.PI / 180, projectileIndex: 1)),
                        Cooldown.Instance(500, RingAttack.Instance(10, 10, offset: 270 * (float)Math.PI / 180, projectileIndex: 1)),
            //end quite
            //confuse
                        MultiAttack.Instance(10, 1 * (float)Math.PI / 30, 3, 0, projectileIndex: 2),
            //end confuse
            //quite
                        Cooldown.Instance(500, RingAttack.Instance(10, 10, offset: 360 * (float)Math.PI / 180, projectileIndex: 1)),
                        Cooldown.Instance(500, RingAttack.Instance(10, 10, offset: 0 * (float)Math.PI / 180, projectileIndex: 1)),
                        Cooldown.Instance(500, RingAttack.Instance(10, 10, offset: 90 * (float)Math.PI / 180, projectileIndex: 1)),
                        Cooldown.Instance(500, RingAttack.Instance(10, 10, offset: 180 * (float)Math.PI / 180, projectileIndex: 1)),
            //end quite
            //confuse
                        SetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),
                        MultiAttack.Instance(10, 1 * (float)Math.PI / 30, 3, 0, projectileIndex: 2),
            //end confuse
            //quite
                        Cooldown.Instance(500, RingAttack.Instance(10, 10, offset: 270 * (float)Math.PI / 180, projectileIndex: 1)),
                        Cooldown.Instance(500, RingAttack.Instance(10, 10, offset: 360 * (float)Math.PI / 180, projectileIndex: 1)),
                        Cooldown.Instance(500, RingAttack.Instance(10, 10, offset: 0 * (float)Math.PI / 180, projectileIndex: 1)),
                        Cooldown.Instance(500, RingAttack.Instance(10, 10, offset: 90 * (float)Math.PI / 180, projectileIndex: 1)),
            //end quite
            //confuse
                        MultiAttack.Instance(10, 1 * (float)Math.PI / 30, 3, 0, projectileIndex: 2),
            //end confuse
            //quite
                        Cooldown.Instance(500, RingAttack.Instance(10, 10, offset: 180 * (float)Math.PI / 180, projectileIndex: 1)),
                        Cooldown.Instance(500, RingAttack.Instance(10, 10, offset: 270 * (float)Math.PI / 180, projectileIndex: 1)),
                        Cooldown.Instance(500, RingAttack.Instance(10, 10, offset: 360 * (float)Math.PI / 180, projectileIndex: 1)),
                        Cooldown.Instance(500, RingAttack.Instance(10, 10, offset: 0 * (float)Math.PI / 180, projectileIndex: 1)),
            //end quite
            //confuse
                        MultiAttack.Instance(10, 1 * (float)Math.PI / 30, 3, 0, projectileIndex: 2),
                        UnsetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),
            //end confuse
            //quite
                        Cooldown.Instance(500, RingAttack.Instance(10, 10, offset: 90 * (float)Math.PI / 180, projectileIndex: 1)),
                        Cooldown.Instance(500, RingAttack.Instance(10, 10, offset: 180 * (float)Math.PI / 180, projectileIndex: 1)),
                        Cooldown.Instance(500, RingAttack.Instance(10, 10, offset: 270 * (float)Math.PI / 180, projectileIndex: 1)),
                        Cooldown.Instance(500, RingAttack.Instance(10, 10, offset: 360 * (float)Math.PI / 180, projectileIndex: 1)),
            //end quite
            //confuse
                        MultiAttack.Instance(10, 1 * (float)Math.PI / 30, 3, 0, projectileIndex: 2),
            //end confuse
            //quite
                        SetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),
                        Cooldown.Instance(500, RingAttack.Instance(10, 10, offset: 0 * (float)Math.PI / 180, projectileIndex: 1)),
                        Cooldown.Instance(500, RingAttack.Instance(10, 10, offset: 90 * (float)Math.PI / 180, projectileIndex: 1)),
                        Cooldown.Instance(500, RingAttack.Instance(10, 10, offset: 180 * (float)Math.PI / 180, projectileIndex: 1)),
                        Cooldown.Instance(500, RingAttack.Instance(10, 10, offset: 270 * (float)Math.PI / 180, projectileIndex: 1)),
            //end quite
            //confuse
                        MultiAttack.Instance(10, 1 * (float)Math.PI / 30, 3, 0, projectileIndex: 2),
            //end confuse
            //quite
                        Cooldown.Instance(500, RingAttack.Instance(10, 10, offset: 360 * (float)Math.PI / 180, projectileIndex: 1)),
                        Cooldown.Instance(500, RingAttack.Instance(10, 10, offset: 0 * (float)Math.PI / 180, projectileIndex: 1)),
                        Cooldown.Instance(500, RingAttack.Instance(10, 10, offset: 90 * (float)Math.PI / 180, projectileIndex: 1)),
                        Cooldown.Instance(500, RingAttack.Instance(10, 10, offset: 180 * (float)Math.PI / 180, projectileIndex: 1)),
                        Cooldown.Instance(500, RingAttack.Instance(10, 10, offset: 270 * (float)Math.PI / 180, projectileIndex: 1)),
            //end quite

                        Cooldown.Instance(1500),
                        UnsetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),

        #endregion


        #region Spawn Minions + Circleshoot

        #region Spawn Minions

            //SetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),
            //IsEntityNotPresent.Instance(100, 0x0db0),
            //IsEntityNotPresent.Instance(100, 0x0db1),
            //IsEntityNotPresent.Instance(100, 0x0db2),
            //UnsetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),

                        SpawnMinionImmediate.Instance(0x0db0, 0, 4, 6),
                        SpawnMinionImmediate.Instance(0x0db1, 0, 4, 6),
                        SpawnMinionImmediate.Instance(0x0db2, 0, 4, 6),

        #endregion

        #region Circleshoot

        #region Circle Attack 1

 Cooldown.Instance(150),
                      new RunBehaviors(
                          MultiAttack.Instance(5, 1 * (float)Math.PI / 30, 3, 0, projectileIndex: 4),
                          RingAttack.Instance(3, offset: 0 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 18 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 36 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 54 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 72 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          MultiAttack.Instance(5, 1 * (float)Math.PI / 30, 3, 0, projectileIndex: 4),
                          RingAttack.Instance(3, offset: 90 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 108 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 126 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 144 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 162 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          MultiAttack.Instance(5, 1 * (float)Math.PI / 30, 3, 0, projectileIndex: 4),
                          RingAttack.Instance(3, offset: 180 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 198 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 216 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 234 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 252 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          MultiAttack.Instance(5, 1 * (float)Math.PI / 30, 3, 0, projectileIndex: 4),
                          RingAttack.Instance(3, offset: 270 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 288 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 306 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 324 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 342 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(250),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 360 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(100),
        #endregion

        #region Circle Attack 2

 Cooldown.Instance(150),
                      new RunBehaviors(
                          MultiAttack.Instance(5, 1 * (float)Math.PI / 30, 3, 0, projectileIndex: 4),
                          RingAttack.Instance(3, offset: 0 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 18 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 36 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 54 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 72 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          MultiAttack.Instance(5, 1 * (float)Math.PI / 30, 3, 0, projectileIndex: 4),
                          RingAttack.Instance(3, offset: 90 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 108 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 126 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 144 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 162 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          MultiAttack.Instance(5, 1 * (float)Math.PI / 30, 3, 0, projectileIndex: 4),
                          RingAttack.Instance(3, offset: 180 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 198 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 216 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 234 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 252 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          MultiAttack.Instance(5, 1 * (float)Math.PI / 30, 3, 0, projectileIndex: 4),
                          RingAttack.Instance(3, offset: 270 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 288 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 306 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 324 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(150),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 342 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(250),
                      new RunBehaviors(
                          RingAttack.Instance(3, offset: 360 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(1100))
        #endregion

        #endregion

        #endregion
                ),
                loot: new LootBehavior(LootDef.Empty,
                        Tuple.Create(800, new LootDef(0, 5, 1, 2,
                            Tuple.Create(0.01, (ILoot)new ItemLoot("Doom Bow")),
                            Tuple.Create(0.005, (ILoot)new ItemLoot("Wine Cellar Incantation")),
                            Tuple.Create(1.0, (ILoot)new StatPotionLoot(StatPotion.Wis)),

                            Tuple.Create(0.3, (ILoot)new TierLoot(3, ItemType.Ring)),
                            Tuple.Create(0.2, (ILoot)new TierLoot(4, ItemType.Ring)),
                            Tuple.Create(0.3, (ILoot)new TierLoot(7, ItemType.Weapon)),
                            Tuple.Create(0.2, (ILoot)new TierLoot(8, ItemType.Weapon)),
                            Tuple.Create(0.3, (ILoot)new TierLoot(3, ItemType.Ability)),
                            Tuple.Create(0.2, (ILoot)new TierLoot(4, ItemType.Ability)),
                            Tuple.Create(0.05, (ILoot)new TierLoot(5, ItemType.Ability))
                        ))
                )
            ))
            .Init(0x0dab, Behaves("Lair Ghost Warrior",
                new RunBehaviors(
                    Chasing.Instance(8, 10, 1, null),
                    Cooldown.Instance(500, SimpleAttack.Instance(12))
                )
            ))
            .Init(0x0db0, Behaves("Ghost Warrior of Septavius",
                    IfNot.Instance(
                      Chasing.Instance(12, 7, 1, null),
                        IfNot.Instance(
                          Chasing.Instance(10, 7, 1, 0x0d90),
                          SimpleWandering.Instance(4f)
                          )
                    ),
                    Cooldown.Instance(1000, SimpleAttack.Instance(10, projectileIndex: 0)
                    ),
                    loot: new LootBehavior(
                        new LootDef(0, 1, 0, 8,
                            Tuple.Create(0.6, (ILoot)PotionLoot.Instance)
                            ))
                ))
                .Init(0x0db1, Behaves("Ghost Mage of Septavius",
                    IfNot.Instance(
                      Chasing.Instance(12, 7, 1, null),
                        IfNot.Instance(
                          Chasing.Instance(10, 7, 1, 0x0d90),
                          SimpleWandering.Instance(4f)
                          )
                    ),
                    Cooldown.Instance(1000, SimpleAttack.Instance(10, projectileIndex: 0)
                    ),
                    loot: new LootBehavior(
                        new LootDef(0, 1, 0, 8,
                            Tuple.Create(0.6, (ILoot)PotionLoot.Instance)
                            ))
                ))
                .Init(0x0db2, Behaves("Ghost Rogue of Septavius",
                    IfNot.Instance(
                      Chasing.Instance(12, 7, 1, null),
                        IfNot.Instance(
                          Chasing.Instance(10, 7, 1, 0x0d90),
                          SimpleWandering.Instance(4f)
                          )
                    ),
                    Cooldown.Instance(1000, SimpleAttack.Instance(10, projectileIndex: 0)
                    ),
                    loot: new LootBehavior(
                        new LootDef(0, 1, 0, 8,
                            Tuple.Create(0.6, (ILoot)PotionLoot.Instance)
                            ))
                ))
                .Init(0x0d91, Behaves("Lair Skeleton",
                    IfNot.Instance(
                      Chasing.Instance(13, 7, 1, null),
                      SimpleWandering.Instance(4f)
                    ),
                    Cooldown.Instance(1000, SimpleAttack.Instance(10, projectileIndex: 0)
                    ),
                    loot: new LootBehavior(
                        new LootDef(0, 1, 0, 8,
                            Tuple.Create(0.1, (ILoot)PotionLoot.Instance)
                            ))
                ))
                .Init(0x0d95, Behaves("Lair Skeleton King",
                    IfNot.Instance(
                      Chasing.Instance(13, 10, 7, null),
                      SimpleWandering.Instance(4f)
                    ),
                    Cooldown.Instance(1000, MultiAttack.Instance(10, 1 * (float)Math.PI / 30, 3, 0, projectileIndex: 0)
                    ),
                    loot: new LootBehavior(
                        new LootDef(0, 1, 0, 8,
                            Tuple.Create(0.3, (ILoot)new TierLoot(5, ItemType.Armor))
                            ),
                            Tuple.Create(100, new LootDef(0, 1, 0, 2,
                                Tuple.Create(0.2, (ILoot)new TierLoot(6, ItemType.Weapon)),
                                Tuple.Create(0.1, (ILoot)new TierLoot(7, ItemType.Weapon)),
                                Tuple.Create(0.09, (ILoot)new TierLoot(8, ItemType.Weapon)),
                                Tuple.Create(0.3, (ILoot)new TierLoot(6, ItemType.Armor)),
                                Tuple.Create(0.2, (ILoot)new TierLoot(7, ItemType.Armor)),
                                Tuple.Create(0.3, (ILoot)new TierLoot(3, ItemType.Ring)),
                                Tuple.Create(0.2, (ILoot)new TierLoot(3, ItemType.Ability))
                                )
                            ))
                ))
                .Init(0x0d94, Behaves("Lair Skeleton Mage",
                    IfNot.Instance(
                      Chasing.Instance(13, 10, 7, null),
                      SimpleWandering.Instance(4f)
                    ),
                    Cooldown.Instance(1000, MultiAttack.Instance(10, 1 * (float)Math.PI / 30, 3, 0, projectileIndex: 0)
                    ),
                    loot: new LootBehavior(
                        new LootDef(0, 1, 0, 8,
                            Tuple.Create(0.1, (ILoot)PotionLoot.Instance)
                            ))
                ))
                .Init(0x0d93, Behaves("Lair Skeleton Veteran",
                    IfNot.Instance(
                      Chasing.Instance(13, 10, 7, null),
                      SimpleWandering.Instance(4f)
                    ),
                    Cooldown.Instance(1000, SimpleAttack.Instance(10, projectileIndex: 0)
                    ),
                    loot: new LootBehavior(
                        new LootDef(0, 1, 0, 8,
                            Tuple.Create(0.1, (ILoot)PotionLoot.Instance)
                            ))
                ))
                .Init(0x0d92, Behaves("Lair Skeleton Swordsman",
                    IfNot.Instance(
                      Chasing.Instance(13, 10, 1, null),
                      SimpleWandering.Instance(4f)
                    ),
                    Cooldown.Instance(1000, SimpleAttack.Instance(10, projectileIndex: 0)
                    ),
                    loot: new LootBehavior(
                        new LootDef(0, 1, 0, 8,
                            Tuple.Create(0.1, (ILoot)PotionLoot.Instance)
                            ))
                ))
                .Init(0x0d96, Behaves("Lair Mummy",
                    IfNot.Instance(
                      Chasing.Instance(12, 10, 7, null),
                      SimpleWandering.Instance(4f)
                    ),
                    Cooldown.Instance(1000, SimpleAttack.Instance(10, projectileIndex: 0)
                    ),
                    loot: new LootBehavior(
                        new LootDef(0, 1, 0, 8,
                            Tuple.Create(0.1, (ILoot)PotionLoot.Instance)
                            ))
                ))
                .Init(0x0d97, Behaves("Lair Mummy King",
                    IfNot.Instance(
                      Chasing.Instance(12, 10, 7, null),
                      SimpleWandering.Instance(4f)
                    ),
                    Cooldown.Instance(1000, SimpleAttack.Instance(10, projectileIndex: 0)
                    ),
                    loot: new LootBehavior(
                        new LootDef(0, 1, 0, 8,
                            Tuple.Create(0.1, (ILoot)PotionLoot.Instance)
                            ))
                ))
                .Init(0x0d98, Behaves("Lair Mummy Pharaoh",
                    IfNot.Instance(
                      Chasing.Instance(13, 10, 1, null),
                      SimpleWandering.Instance(4f)
                    ),
                    Cooldown.Instance(1000, SimpleAttack.Instance(10, projectileIndex: 0)
                    ),
                    loot: new LootBehavior(
                        new LootDef(0, 1, 0, 8,
                            Tuple.Create(0.3, (ILoot)new TierLoot(5, ItemType.Armor))
                            ),
                            Tuple.Create(100, new LootDef(0, 1, 0, 2,
                                Tuple.Create(0.2, (ILoot)new TierLoot(6, ItemType.Weapon)),
                                Tuple.Create(0.1, (ILoot)new TierLoot(7, ItemType.Weapon)),
                                Tuple.Create(0.09, (ILoot)new TierLoot(8, ItemType.Weapon)),
                                Tuple.Create(0.3, (ILoot)new TierLoot(6, ItemType.Armor)),
                                Tuple.Create(0.2, (ILoot)new TierLoot(7, ItemType.Armor)),
                                Tuple.Create(0.3, (ILoot)new TierLoot(3, ItemType.Ring)),
                                Tuple.Create(0.2, (ILoot)new TierLoot(3, ItemType.Ability))
                                )
                            ))
                ))
                .Init(0x0d9e, Behaves("Lair Big Brown Slime",
                    new RunBehaviors(
                        SimpleWandering.Instance(2f),
                        Cooldown.Instance(500, MultiAttack.Instance(10, 1 * (float)Math.PI / 30, 3, 0, projectileIndex: 0))
                    ),
                    condBehaviors: new ConditionalBehavior[]
                    {
                        new DeathTransmute(0x0d9f, 6, 6)
                    }
                ))
                .Init(0x0d9f, Behaves("Lair Little Brown Slime",
                    IfNot.Instance(
                      Chasing.Instance(12, 10, 1, 0x0d9e),
                      SimpleWandering.Instance(2f)
                    ),
                    Cooldown.Instance(500, MultiAttack.Instance(10, 1 * (float)Math.PI / 30, 3, 0, projectileIndex: 0)
                    ),
                    loot: new LootBehavior(
                        new LootDef(0, 1, 0, 8,
                            Tuple.Create(0.1, (ILoot)PotionLoot.Instance)
                            ))
                ))
                .Init(0x0d9b, Behaves("Lair Big Black Slime",
                    new RunBehaviors(
                        SimpleWandering.Instance(2f),
                        Cooldown.Instance(500, SimpleAttack.Instance(10, projectileIndex: 0))
                    ),
                    condBehaviors: new ConditionalBehavior[]
                    {
                        new DeathTransmute(0x0d9c, 4, 4)
                    }
                ))
                .Init(0x0d9c, Behaves("Lair Medium Black Slime",
                    IfNot.Instance(
                      Chasing.Instance(12, 10, 1, 0x0d9b),
                      SimpleWandering.Instance(2f)
                    ),
                    Cooldown.Instance(500, SimpleAttack.Instance(10, projectileIndex: 0)
                    ),
                    condBehaviors: new ConditionalBehavior[]
                    {
                        new DeathTransmute(0x0d9d, 4, 4)
                    }
                ))
                .Init(0x0d9d, Behaves("Lair Little Black Slime",
                    IfNot.Instance(
                      Chasing.Instance(12, 10, 1, 0x0d9b),
                      IfNot.Instance(
                        Chasing.Instance(12, 10, 1, 0x0d9c),
                        SimpleWandering.Instance(2f)
                        )
                    ),
                    Cooldown.Instance(500, SimpleAttack.Instance(10, projectileIndex: 0)
                    ),
                    loot: new LootBehavior(
                        new LootDef(0, 1, 0, 8,
                            Tuple.Create(0.1, (ILoot)PotionLoot.Instance)
                            ))
                ))
                .Init(0x0d99, Behaves("Lair Construct Giant",
                    new RunBehaviors(
                        IfNot.Instance(
                            Chasing.Instance(13, 10, 7, null),
                            SimpleWandering.Instance(4f)
                            ),
                        Cooldown.Instance(1000, MultiAttack.Instance(10, 1 * (float)Math.PI / 30, 3, 0, projectileIndex: 0)),
                        Cooldown.Instance(1000, MultiAttack.Instance(10, 1 * (float)Math.PI / 30, 3, 0, projectileIndex: 1))
                    ),
                    loot: new LootBehavior(
                        new LootDef(0, 1, 0, 8,
                            Tuple.Create(0.3, (ILoot)new TierLoot(5, ItemType.Armor))
                            ),
                            Tuple.Create(100, new LootDef(0, 1, 0, 2,
                                Tuple.Create(0.2, (ILoot)new TierLoot(6, ItemType.Weapon)),
                                Tuple.Create(0.1, (ILoot)new TierLoot(7, ItemType.Weapon)),
                                Tuple.Create(0.09, (ILoot)new TierLoot(8, ItemType.Weapon)),
                                Tuple.Create(0.3, (ILoot)new TierLoot(6, ItemType.Armor)),
                                Tuple.Create(0.2, (ILoot)new TierLoot(7, ItemType.Armor)),
                                Tuple.Create(0.3, (ILoot)new TierLoot(3, ItemType.Ring)),
                                Tuple.Create(0.2, (ILoot)new TierLoot(3, ItemType.Ability)),
                                Tuple.Create(0.005, (ILoot)new ItemLoot("Purple Drake Egg"))
                                )
                            ))
                ))
                .Init(0x0d9a, Behaves("Lair Construct Titan",
                    new RunBehaviors(
                        IfNot.Instance(
                            Chasing.Instance(13, 10, 7, null),
                            SimpleWandering.Instance(4f)
                            ),
                        Cooldown.Instance(1000, MultiAttack.Instance(10, 1 * (float)Math.PI / 30, 3, 0, projectileIndex: 0)),
                        Cooldown.Instance(1000, MultiAttack.Instance(10, 1 * (float)Math.PI / 30, 3, 0, projectileIndex: 1))
                    ),
                    loot: new LootBehavior(
                        new LootDef(0, 1, 0, 8,
                            Tuple.Create(0.3, (ILoot)new TierLoot(5, ItemType.Armor))
                            ),
                            Tuple.Create(100, new LootDef(0, 1, 0, 2,
                                Tuple.Create(0.2, (ILoot)new TierLoot(6, ItemType.Weapon)),
                                Tuple.Create(0.1, (ILoot)new TierLoot(7, ItemType.Weapon)),
                                Tuple.Create(0.09, (ILoot)new TierLoot(8, ItemType.Weapon)),
                                Tuple.Create(0.3, (ILoot)new TierLoot(6, ItemType.Armor)),
                                Tuple.Create(0.2, (ILoot)new TierLoot(7, ItemType.Armor)),
                                Tuple.Create(0.3, (ILoot)new TierLoot(3, ItemType.Ring)),
                                Tuple.Create(0.2, (ILoot)new TierLoot(3, ItemType.Ability)),
                                Tuple.Create(0.005, (ILoot)new ItemLoot("Purple Drake Egg"))
                                )
                            ))
                ))
                .Init(0x0da0, Behaves("Lair Brown Bat",
                    new RunBehaviors(
                        IfNot.Instance(
                            Cooldown.Instance(2000, Charge.Instance(40, 10, null)),
                            SimpleWandering.Instance(2f)
                            ),
                        Cooldown.Instance(1000, SimpleAttack.Instance(2, projectileIndex: 0))
                    ),
                    loot: new LootBehavior(
                        new LootDef(0, 1, 0, 8,
                            Tuple.Create(0.1, (ILoot)PotionLoot.Instance)
                            ))
                ))
                .Init(0x0da1, Behaves("Lair Ghost Bat",
                    new RunBehaviors(
                        IfNot.Instance(
                            Cooldown.Instance(2000, Charge.Instance(40, 10, null)),
                            SimpleWandering.Instance(2f)
                            ),
                        Cooldown.Instance(1000, SimpleAttack.Instance(2, projectileIndex: 0))
                    ),
                    loot: new LootBehavior(
                        new LootDef(0, 1, 0, 8,
                            Tuple.Create(0.1, (ILoot)PotionLoot.Instance)
                            ))
                ))
                .Init(0x0da4, Behaves("Lair Reaper",
                    IfNot.Instance(
                        Chasing.Instance(15, 10, 1, null),
                        SimpleWandering.Instance(2f)
                        ),
                    Cooldown.Instance(1000, SimpleAttack.Instance(3, projectileIndex: 0)
                    ),
                    loot: new LootBehavior(
                        new LootDef(0, 1, 0, 8,
                            Tuple.Create(0.3, (ILoot)new TierLoot(5, ItemType.Armor))
                            ),
                            Tuple.Create(100, new LootDef(0, 1, 0, 2,
                                Tuple.Create(0.2, (ILoot)new TierLoot(6, ItemType.Weapon)),
                                Tuple.Create(0.1, (ILoot)new TierLoot(7, ItemType.Weapon)),
                                Tuple.Create(0.09, (ILoot)new TierLoot(8, ItemType.Weapon)),
                                Tuple.Create(0.3, (ILoot)new TierLoot(6, ItemType.Armor)),
                                Tuple.Create(0.2, (ILoot)new TierLoot(7, ItemType.Armor)),
                                Tuple.Create(0.3, (ILoot)new TierLoot(3, ItemType.Ring)),
                                Tuple.Create(0.2, (ILoot)new TierLoot(3, ItemType.Ability))
                                )
                            ))
                ))
                .Init(0x0da5, Behaves("Lair Vampire",
                    IfNot.Instance(
                        Chasing.Instance(13, 10, 1, null),
                        SimpleWandering.Instance(2f)
                        ),
                    Cooldown.Instance(500, SimpleAttack.Instance(20, projectileIndex: 0)),
                    Cooldown.Instance(1000, SimpleAttack.Instance(2, projectileIndex: 1)
                    ),
                    loot: new LootBehavior(
                        new LootDef(0, 1, 0, 8,
                            Tuple.Create(0.1, (ILoot)PotionLoot.Instance)
                            ))
                ))
                .Init(0x0da6, Behaves("Lair Vampire King",
                    IfNot.Instance(
                        Chasing.Instance(13, 10, 1, null),
                        SimpleWandering.Instance(2f)
                        ),
                    Cooldown.Instance(500, SimpleAttack.Instance(20, projectileIndex: 0)),
                    Cooldown.Instance(1000, SimpleAttack.Instance(2, projectileIndex: 1)
                    ),
                    loot: new LootBehavior(
                        new LootDef(0, 1, 0, 8,
                            Tuple.Create(0.3, (ILoot)new TierLoot(5, ItemType.Armor))
                            ),
                            Tuple.Create(100, new LootDef(0, 1, 0, 2,
                                Tuple.Create(0.2, (ILoot)new TierLoot(6, ItemType.Weapon)),
                                Tuple.Create(0.1, (ILoot)new TierLoot(7, ItemType.Weapon)),
                                Tuple.Create(0.09, (ILoot)new TierLoot(8, ItemType.Weapon)),
                                Tuple.Create(0.3, (ILoot)new TierLoot(6, ItemType.Armor)),
                                Tuple.Create(0.2, (ILoot)new TierLoot(7, ItemType.Armor)),
                                Tuple.Create(0.3, (ILoot)new TierLoot(3, ItemType.Ring)),
                                Tuple.Create(0.2, (ILoot)new TierLoot(3, ItemType.Ability))
                                )
                            ))
                ))
                .Init(0x0da7, Behaves("Lair Grey Spectre",
                    new RunBehaviors(
                        SimpleWandering.Instance(2f),
                        Cooldown.Instance(1000, SimpleAttack.Instance(14, projectileIndex: 0)),
                        Cooldown.Instance(1000, ThrowAttack.Instance(2, 8, 50))
                    ),
                    loot: new LootBehavior(LootDef.Empty,
                        Tuple.Create(100, new LootDef(0, 1, 0, 2,
                        Tuple.Create(0.2, (ILoot)new TierLoot(4, ItemType.Ability))
                        )))
                ))
                .Init(0x0da8, Behaves("Lair Blue Spectre",
                    new RunBehaviors(
                        SimpleWandering.Instance(2f),
                        Cooldown.Instance(1000, SimpleAttack.Instance(14, projectileIndex: 0)),
                        Cooldown.Instance(1000, ThrowAttack.Instance(2, 8, 90))
                    ),
                    loot: new LootBehavior(LootDef.Empty,
                        Tuple.Create(100, new LootDef(0, 1, 0, 2,
                        Tuple.Create(0.2, (ILoot)new TierLoot(4, ItemType.Ability))
                        )))
                ))
                .Init(0x0da9, Behaves("Lair White Spectre",
                    new RunBehaviors(
                        SimpleWandering.Instance(2f),
                        Cooldown.Instance(1000, SimpleAttack.Instance(14, projectileIndex: 0)),
                        Cooldown.Instance(1000, ThrowAttack.Instance(2, 8, 90))
                    ),
                    loot: new LootBehavior(LootDef.Empty,
                        Tuple.Create(100, new LootDef(0, 1, 0, 2,
                        Tuple.Create(0.2, (ILoot)new TierLoot(4, ItemType.Ability))
                        )))
                ))
                .Init(0x0da3, Behaves("Lair Blast Trap",
                    attack: new RunBehaviors(
                        If.Instance(
                            IsEntityPresent.Instance(5, null),
                            new QueuedBehavior(
                                MultiAttack.Instance(8, 10 * (float)Math.PI / 180, 6),
                                Die.Instance
                            )
                        )
                    )
                ))
                .Init(0x0da2, Behaves("Lair Burst Trap",
                    attack: new RunBehaviors(
                        If.Instance(
                            IsEntityPresent.Instance(5, null),
                            new QueuedBehavior(
                                MultiAttack.Instance(8, 10 * (float)Math.PI / 180, 6),
                                Die.Instance
                            )
                        )
                    )
                ));
    }
}
