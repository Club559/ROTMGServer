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
        static _ Crystal = Behav()
        .Init(0x0935, Behaves("Mysterious Crystal",
               new RunBehaviors(
                   Once.Instance(new SetKey(-1, 1)),
        #region HpLesserPercent

                   /*If.Instance(
                     And.Instance(HpLesser.Instance(999000, NullBehavior.Instance), HpGreaterEqual.Instance(1000000, NullBehavior.Instance)),
                       new SetKey(-1, 2)  //
                   ),
                   If.Instance(
                     And.Instance(HpLesser.Instance(998000, NullBehavior.Instance), HpGreaterEqual.Instance(999000, NullBehavior.Instance)),
                       new SetKey(-1, 3)  //
                   ),
                   If.Instance(
                     And.Instance(HpLesser.Instance(997000, NullBehavior.Instance), HpGreaterEqual.Instance(998000, NullBehavior.Instance)),
                       Once.Instance(new SetKey(-1, 7))   //break
                   ),*/
                   Cooldown.Instance(5000, HpLesserPercent.Instance(0.9f, new SetKey(-1, 2))),
                   Cooldown.Instance(5000, HpLesserPercent.Instance(0.85f, new SetKey(-1, 3))),
                   Cooldown.Instance(5000, HpLesserPercent.Instance(0.8f, new SetKey(-1, 7))),

        #endregion
                IfEqual.Instance(-1, 1,
                  new QueuedBehavior(
                    Cooldown.Instance(100),
                    True.Instance(Rand.Instance(
                      new RandomTaunt(0.1, "Break the crystal for great rewards..."),
                      new RandomTaunt(0.1, "Help me...")
                      )
                    ),
                    CooldownExact.Instance(10000, new SetKey(-1, 1))
                    )
                ),
                IfEqual.Instance(-1, 2,
                  new QueuedBehavior(
                    Cooldown.Instance(0, Flashing.Instance(1000, 0xffffffff)),
                    Cooldown.Instance(1100, Flashing.Instance(1000, 0xffffffff)),
                    True.Instance(Rand.Instance(
                      new RandomTaunt(0.1, "Fire upon this crystal with all your might for 5 seconds"),
                      new RandomTaunt(0.1, "If your attacks are weak, the crystal magically heals"),
                      new RandomTaunt(0.1, "Gather a large group to smash it open")
                      )
                    ),
                    Cooldown.Instance(5000, new SetKey(-1, 1))
                    )
                ),
                IfEqual.Instance(-1, 3,
                  new QueuedBehavior(
                    Cooldown.Instance(100),
                    True.Instance(Rand.Instance(
                      new RandomTaunt(0.1, "Sweet treasure awaits for powerful adventurers!"),
                      new RandomTaunt(0.1, "Yes!  Smash my prison for great rewards!")
                      )
                    ),
                    Cooldown.Instance(5000, new SetKey(-1, 4))
                    )
                ),
                IfEqual.Instance(-1, 4,
                  new QueuedBehavior(
                    Cooldown.Instance(100),
                    True.Instance(Rand.Instance(
                      new RandomTaunt(0.1, "If you are not very strong, this could kill you"),
                      new RandomTaunt(0.1, "If you are not yet powerful, stay away from the Crystal"),
                      new RandomTaunt(0.1, "New adventurers should stay away"),
                      new RandomTaunt(0.1, "That's the spirit. Lay your fire upon me."),
                      new RandomTaunt(0.1, "So close...")
                      )
                    ),
                    Cooldown.Instance(5000, new SetKey(-1, 5))
                    )
                ),
                IfEqual.Instance(-1, 5,
                  new QueuedBehavior(
                    Cooldown.Instance(100),
                    True.Instance(Rand.Instance(
                      new RandomTaunt(0.1, "I think you need more people..."),
                      new RandomTaunt(0.1, "Call all your friends to help you break the crystal!")
                      )
                    ),
                    CooldownExact.Instance(10000, new SetKey(-1, 6))
                    )
                ),
                IfEqual.Instance(-1, 6,
                  new RunBehaviors(
                      new SimpleTaunt("Perhaps you need a bigger group. Ask others to join you!"),
                      Cooldown.Instance(0, Flashing.Instance(1000, 0xffffffff)),
                      Heal.Instance(5000, 1000000, 0x0935),
                      RingAttack.Instance(16, 22, 16, projectileIndex: 0),
                      Cooldown.Instance(1100, Flashing.Instance(1000, 0xffffffff)),
                      CooldownExact.Instance(5000, new SetKey(-1, 1))
                    )
                ),
                IfEqual.Instance(-1, 7,
                  new QueuedBehavior(
                    SetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),
                    Once.Instance(new SimpleTaunt("You cracked the crystal! Soon we shall emerge!")),
                    Cooldown.Instance(1000, SetSize.Instance(95)),
                    Cooldown.Instance(1000, SetSize.Instance(90)),
                    Cooldown.Instance(1000, SetSize.Instance(85)),
                    Cooldown.Instance(1000, SetSize.Instance(80)),
                    Flashing.Instance(1000, 0xffffffff),
                    Flashing.Instance(1000, 0xffffffff),
                    CooldownExact.Instance(4000, new SetKey(-1, 8))
                    )
                ),
                IfEqual.Instance(-1, 8,
                  new QueuedBehavior(
                    Once.Instance(new SimpleTaunt("This your reward! Imagine what evil even Oryx needs to keep locked up!")),
                    Once.Instance(RingAttack.Instance(16, 22, 16, projectileIndex: 0)),
                    Once.Instance(SpawnMinionImmediate.Instance(0x0941, 0, 1, 1)),
                    Once.Instance(Despawn.Instance)
                    ))
                )
            ))
            .Init(0x0941, Behaves("Crystal Prisoner",
                  new RunBehaviors(
                    Once.Instance(new SimpleTaunt("I'm finally free! Yesss!!!")),
                    Once.Instance(SetConditionEffect.Instance(ConditionEffectIndex.Invulnerable)),
                    SmoothWandering.Instance(1f, 1f),
                    Once.Instance(SpawnMinionImmediate.Instance(0x0943, 3, 3, 5)
                    ),
                    If.Instance(
                        EntityLesserThan.Instance(100, 3, 0x0943),  //IsEntityNotPresent.Instance(100, 0x0943),
                        Rand.Instance(
                            Reproduce.Instance(0x0943, 3, 100, 3)
                            )
                    )
                ),
                new QueuedBehavior(//here Is shooting start

        #region Circle Attack 1
                    Cooldown.Instance(4000),
                      new RunBehaviors(
                          UnsetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),
                          SimpleAttack.Instance(5, projectileIndex: 3),
                          RingAttack.Instance(4, offset: 0 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(200),
                      new RunBehaviors(
                          RingAttack.Instance(4, offset: 18 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(200),
                      new RunBehaviors(
                          RingAttack.Instance(4, offset: 36 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(200),
                      new RunBehaviors(
                          SetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),
                          RingAttack.Instance(4, offset: 54 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(200),
                      new RunBehaviors(
                          RingAttack.Instance(4, offset: 72 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(200),
                      new RunBehaviors(
                          SimpleAttack.Instance(5, projectileIndex: 3),
                          RingAttack.Instance(4, offset: 90 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(200),
                      new RunBehaviors(
                          UnsetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),
                          RingAttack.Instance(4, offset: 108 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(200),
                      new RunBehaviors(
                          RingAttack.Instance(4, offset: 126 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(200),
                      new RunBehaviors(
                          RingAttack.Instance(4, offset: 144 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(200),
                      new RunBehaviors(
                          RingAttack.Instance(4, offset: 162 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(200),
                      new RunBehaviors(
                          SetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),
                          SimpleAttack.Instance(5, projectileIndex: 3),
                          RingAttack.Instance(4, offset: 180 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(200),
                      new RunBehaviors(
                          RingAttack.Instance(4, offset: 198 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(200),
                      new RunBehaviors(
                          RingAttack.Instance(4, offset: 216 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(200),
                      new RunBehaviors(
                          RingAttack.Instance(4, offset: 234 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(200),
                      new RunBehaviors(
                          UnsetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),
                          RingAttack.Instance(4, offset: 252 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(200),
                      new RunBehaviors(
                          SimpleAttack.Instance(5, projectileIndex: 3),
                          RingAttack.Instance(4, offset: 270 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(200),
                      new RunBehaviors(
                          RingAttack.Instance(4, offset: 288 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(200),
                      new RunBehaviors(
                          SetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),
                          RingAttack.Instance(4, offset: 306 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(200),
                      new RunBehaviors(
                          RingAttack.Instance(4, offset: 324 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(200),
                      new RunBehaviors(
                          RingAttack.Instance(4, offset: 342 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(250),
                      new RunBehaviors(
                          UnsetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),
                          RingAttack.Instance(4, offset: 360 * (float)Math.PI / 180)
                    ),
                    
        #endregion 

        #region Circle Attack 2

                    Cooldown.Instance(200),
                      new RunBehaviors(
                          UnsetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),
                          SimpleAttack.Instance(5, projectileIndex: 3),
                          RingAttack.Instance(4, offset: 0 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(200),
                      new RunBehaviors(
                          RingAttack.Instance(4, offset: 18 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(200),
                      new RunBehaviors(
                          RingAttack.Instance(4, offset: 36 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(200),
                      new RunBehaviors(
                          SetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),
                          RingAttack.Instance(4, offset: 54 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(200),
                      new RunBehaviors(
                          RingAttack.Instance(4, offset: 72 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(200),
                      new RunBehaviors(
                          SimpleAttack.Instance(5, projectileIndex: 3),
                          RingAttack.Instance(4, offset: 90 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(200),
                      new RunBehaviors(
                          UnsetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),
                          RingAttack.Instance(4, offset: 108 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(200),
                      new RunBehaviors(
                          RingAttack.Instance(4, offset: 126 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(200),
                      new RunBehaviors(
                          RingAttack.Instance(4, offset: 144 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(200),
                      new RunBehaviors(
                          RingAttack.Instance(4, offset: 162 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(200),
                      new RunBehaviors(
                          SetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),
                          SimpleAttack.Instance(5, projectileIndex: 3),
                          RingAttack.Instance(4, offset: 180 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(200),
                      new RunBehaviors(
                          RingAttack.Instance(4, offset: 198 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(200),
                      new RunBehaviors(
                          RingAttack.Instance(4, offset: 216 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(200),
                      new RunBehaviors(
                          RingAttack.Instance(4, offset: 234 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(200),
                      new RunBehaviors(
                          UnsetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),
                          RingAttack.Instance(4, offset: 252 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(200),
                      new RunBehaviors(
                          SimpleAttack.Instance(5, projectileIndex: 3),
                          RingAttack.Instance(4, offset: 270 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(200),
                      new RunBehaviors(
                          RingAttack.Instance(4, offset: 288 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(200),
                      new RunBehaviors(
                          SetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),
                          RingAttack.Instance(4, offset: 306 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(200),
                      new RunBehaviors(
                          RingAttack.Instance(4, offset: 324 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(200),
                      new RunBehaviors(
                          RingAttack.Instance(4, offset: 342 * (float)Math.PI / 180)
                    ),
                    Cooldown.Instance(250),
                      new RunBehaviors(
                          UnsetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),
                          RingAttack.Instance(4, offset: 360 * (float)Math.PI / 180)
                    ),

        #endregion

        #region Flashing + SetCondEff

                    SetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),
                    Flashing.Instance(150, 0x0000FF0C),
                    Flashing.Instance(150, 0x0000FF0C),
                    Flashing.Instance(150, 0x0000FF0C),
                    Flashing.Instance(150, 0x0000FF0C),
                    Flashing.Instance(150, 0x0000FF0C),
                    Flashing.Instance(150, 0x0000FF0C),
                    Flashing.Instance(150, 0x0000FF0C),
                    Flashing.Instance(150, 0x0000FF0C),
                    /*Flashing.Instance(150, 0x0000FF0C),
                    Flashing.Instance(150, 0x0000FF0C),
                    Flashing.Instance(150, 0x0000FF0C),
                    Flashing.Instance(150, 0x0000FF0C),
                    Flashing.Instance(150, 0x0000FF0C),
                    Flashing.Instance(150, 0x0000FF0C),
                    Flashing.Instance(150, 0x0000FF0C),
                    Flashing.Instance(150, 0x0000FF0C),
                    Flashing.Instance(150, 0x0000FF0C),*/
                    Cooldown.Instance(3000),

        #endregion

        #region Spawn Clones

                    SpawnMinionImmediate.Instance(0x0942, 2, 4, 4),
                    TossEnemy.Instance(0, 5, 0x0942),
                    TossEnemy.Instance(60, 7, 0x0942),
                    TossEnemy.Instance(240, 5, 0x0942),
                    TossEnemy.Instance(300, 7, 0x0942),
                    Cooldown.Instance(2000),

        #endregion

        #region Invulnerable

                    SetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),
                    Cooldown.Instance(5000, UnsetConditionEffect.Instance(ConditionEffectIndex.Invulnerable)),

        #endregion

        #region Whoa nelly

                    SetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),
                    Cooldown.Instance(2000),
                      new RunBehaviors(
                        MultiAttack.Instance(10, 15, 1, 40, projectileIndex: 3),
                        MultiAttack.Instance(10, 15, 1, 220, projectileIndex: 3),
                        MultiAttack.Instance(10, 15, 1, 130, projectileIndex: 3),
                        //MultiAttack.Instance(10, 15, 1, 310, projectileIndex: 3),
                        MultiAttack.Instance(10, 15, 3, 40, projectileIndex: 2),
                        MultiAttack.Instance(10, 15, 3, 220, projectileIndex: 2),
                        MultiAttack.Instance(10, 15, 3, 130, projectileIndex: 2)
                        //MultiAttack.Instance(10, 15, 3, 310, projectileIndex: 2)
                    ),
                    Cooldown.Instance(2000),
                      new RunBehaviors(
                        MultiAttack.Instance(10, 15, 1, 40, projectileIndex: 3),
                        MultiAttack.Instance(10, 15, 1, 220, projectileIndex: 3),
                        MultiAttack.Instance(10, 15, 1, 130, projectileIndex: 3),
                        //MultiAttack.Instance(10, 15, 1, 310, projectileIndex: 3),
                        MultiAttack.Instance(10, 15, 3, 40, projectileIndex: 2),
                        MultiAttack.Instance(10, 15, 3, 220, projectileIndex: 2),
                        MultiAttack.Instance(10, 15, 3, 130, projectileIndex: 2)
                        //MultiAttack.Instance(10, 15, 3, 310, projectileIndex: 2)
                    ),
                    UnsetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),
                    Cooldown.Instance(2000),
                      new RunBehaviors(
                        MultiAttack.Instance(10, 15, 1, 40, projectileIndex: 3),
                        MultiAttack.Instance(10, 15, 1, 220, projectileIndex: 3),
                        MultiAttack.Instance(10, 15, 1, 130, projectileIndex: 3),
                        //MultiAttack.Instance(10, 15, 1, 310, projectileIndex: 3),
                        MultiAttack.Instance(10, 15, 3, 40, projectileIndex: 2),
                        MultiAttack.Instance(10, 15, 3, 220, projectileIndex: 2),
                        MultiAttack.Instance(10, 15, 3, 130, projectileIndex: 2)
                        //MultiAttack.Instance(10, 15, 3, 310, projectileIndex: 2)
                    ),
                    Cooldown.Instance(2000),
                      new RunBehaviors(
                        MultiAttack.Instance(10, 15, 1, 40, projectileIndex: 3),
                        MultiAttack.Instance(10, 15, 1, 220, projectileIndex: 3),
                        MultiAttack.Instance(10, 15, 1, 130, projectileIndex: 3),
                        //MultiAttack.Instance(10, 15, 1, 310, projectileIndex: 3),
                        MultiAttack.Instance(10, 15, 3, 40, projectileIndex: 2),
                        MultiAttack.Instance(10, 15, 3, 220, projectileIndex: 2),
                        MultiAttack.Instance(10, 15, 3, 130, projectileIndex: 2)
                        //MultiAttack.Instance(10, 15, 3, 310, projectileIndex: 2)
                    ),
                    Cooldown.Instance(2000),
                      new RunBehaviors(
                        MultiAttack.Instance(10, 15, 1, 40, projectileIndex: 3),
                        MultiAttack.Instance(10, 15, 1, 220, projectileIndex: 3),
                        MultiAttack.Instance(10, 15, 1, 130, projectileIndex: 3),
                        //MultiAttack.Instance(10, 15, 1, 310, projectileIndex: 3),
                        MultiAttack.Instance(10, 15, 3, 40, projectileIndex: 2),
                        MultiAttack.Instance(10, 15, 3, 220, projectileIndex: 2),
                        MultiAttack.Instance(10, 15, 3, 130, projectileIndex: 2)
                        //MultiAttack.Instance(10, 15, 3, 310, projectileIndex: 2)
                    ),
                    Cooldown.Instance(2000),
                      new RunBehaviors(
                        MultiAttack.Instance(10, 15, 1, 40, projectileIndex: 3),
                        MultiAttack.Instance(10, 15, 1, 220, projectileIndex: 3),
                        MultiAttack.Instance(10, 15, 1, 130, projectileIndex: 3),
                        //MultiAttack.Instance(10, 15, 1, 310, projectileIndex: 3),
                        MultiAttack.Instance(10, 15, 3, 40, projectileIndex: 2),
                        MultiAttack.Instance(10, 15, 3, 220, projectileIndex: 2),
                        MultiAttack.Instance(10, 15, 3, 130, projectileIndex: 2)
                        //MultiAttack.Instance(10, 15, 3, 310, projectileIndex: 2)
                    ),

        #endregion 
                    
        #region Confuse Circle

                    SetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),
                    Cooldown.Instance(2000),
                      new RunBehaviors(
                        MultiAttack.Instance(10, 40, 10, 40, projectileIndex: 1)
                    ),
                    Cooldown.Instance(500),
                      new RunBehaviors(
                        MultiAttack.Instance(10, 40, 10, 60, projectileIndex: 1)
                    ),
                    Cooldown.Instance(500),
                      new RunBehaviors(
                        MultiAttack.Instance(10, 40, 10, 50, projectileIndex: 1)
                    ),
                    Cooldown.Instance(500),
                      new RunBehaviors(
                        MultiAttack.Instance(10, 40, 10, 70, projectileIndex: 1)
                    ),
                    Cooldown.Instance(500),
                      new RunBehaviors(
                        MultiAttack.Instance(10, 40, 10, 40, projectileIndex: 1)
                    ),
                    Cooldown.Instance(500),
                      new RunBehaviors(
                        MultiAttack.Instance(10, 40, 10, 60, projectileIndex: 1)
                    ),
                    Cooldown.Instance(500),
                      new RunBehaviors(
                        MultiAttack.Instance(10, 40, 10, 50, projectileIndex: 1)
                    ),
                    Cooldown.Instance(500),
                      new RunBehaviors(
                        MultiAttack.Instance(10, 40, 10, 70, projectileIndex: 1)
                    ),
                    Cooldown.Instance(500),
                      new RunBehaviors(
                        MultiAttack.Instance(10, 40, 10, 40, projectileIndex: 1)
                    ),
                    Cooldown.Instance(500),
                      new RunBehaviors(
                        MultiAttack.Instance(10, 40, 10, 60, projectileIndex: 1)
                    ),
                    UnsetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),
                    Cooldown.Instance(500),
                      new RunBehaviors(
                        MultiAttack.Instance(10, 40, 10, 50, projectileIndex: 1)
                    ),
                    Cooldown.Instance(500),
                      new RunBehaviors(
                        MultiAttack.Instance(10, 40, 10, 70, projectileIndex: 1)
                    ),
                    Cooldown.Instance(500),
                      new RunBehaviors(
                        MultiAttack.Instance(10, 40, 10, 40, projectileIndex: 1)
                    ),
                    Cooldown.Instance(500),
                      new RunBehaviors(
                        MultiAttack.Instance(10, 40, 10, 60, projectileIndex: 1)
                    ),
                    SetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),
                    Cooldown.Instance(500),
                      new RunBehaviors(
                        MultiAttack.Instance(10, 40, 10, 50, projectileIndex: 1)
                    ),
                    Cooldown.Instance(500),
                      new RunBehaviors(
                        MultiAttack.Instance(10, 40, 10, 70, projectileIndex: 1)
                    ),

                    SetSize.Instance(125),

                    Cooldown.Instance(500),
                      new RunBehaviors(
                        MultiAttack.Instance(10, 40, 10, 40, projectileIndex: 1)
                    ),

                    SetSize.Instance(150),

                    Cooldown.Instance(500),
                      new RunBehaviors(
                        MultiAttack.Instance(10, 40, 10, 60, projectileIndex: 1)
                    ),

                    SetSize.Instance(175),

                    Cooldown.Instance(500),
                      new RunBehaviors(
                        MultiAttack.Instance(10, 40, 10, 50, projectileIndex: 1)
                    ),

                    SetSize.Instance(200),

                    Cooldown.Instance(500),
                      new RunBehaviors(
                        MultiAttack.Instance(10, 40, 10, 70, projectileIndex: 1)
                    ),

                    Cooldown.Instance(500),
                      new RunBehaviors(
                        MultiAttack.Instance(10, 40, 10, 40, projectileIndex: 1)
                    ),
                    Cooldown.Instance(500),
                      new RunBehaviors(
                        MultiAttack.Instance(10, 40, 10, 60, projectileIndex: 1)
                    ),
                    UnsetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),
                    Cooldown.Instance(500),
                      new RunBehaviors(
                        MultiAttack.Instance(10, 40, 10, 50, projectileIndex: 1)
                    ),
                    Cooldown.Instance(500),
                      new RunBehaviors(
                        MultiAttack.Instance(10, 40, 10, 70, projectileIndex: 1)
                    ),
                    Cooldown.Instance(500),
                      new RunBehaviors(
                        MultiAttack.Instance(10, 40, 10, 40, projectileIndex: 1)
                    ),
                    Cooldown.Instance(500),
                      new RunBehaviors(
                        MultiAttack.Instance(10, 40, 10, 60, projectileIndex: 1)
                    ),
                    Cooldown.Instance(500),
                      new RunBehaviors(
                        MultiAttack.Instance(10, 40, 10, 50, projectileIndex: 1)
                    ),
                    Cooldown.Instance(500),
                      new RunBehaviors(
                        MultiAttack.Instance(10, 40, 10, 70, projectileIndex: 1)
                    ),
                    Cooldown.Instance(500),
                      new RunBehaviors(
                        MultiAttack.Instance(10, 40, 10, 40, projectileIndex: 1)
                    ),
                    Cooldown.Instance(500),
                      new RunBehaviors(
                        MultiAttack.Instance(10, 40, 10, 60, projectileIndex: 1)
                    ),
                    SetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),
                    Cooldown.Instance(500),
                      new RunBehaviors(
                        MultiAttack.Instance(10, 40, 10, 50, projectileIndex: 1)
                    ),
                    Cooldown.Instance(500),
                      new RunBehaviors(
                        MultiAttack.Instance(10, 40, 10, 70, projectileIndex: 1)
                    ),
                    Cooldown.Instance(500),
                      new RunBehaviors(
                        MultiAttack.Instance(10, 40, 10, 40, projectileIndex: 1)
                    ),
                    Cooldown.Instance(500),
                      new RunBehaviors(
                        MultiAttack.Instance(10, 40, 10, 60, projectileIndex: 1)
                    ),
                    Cooldown.Instance(500),
                      new RunBehaviors(
                        MultiAttack.Instance(10, 40, 10, 50, projectileIndex: 1)
                    ),
                    Cooldown.Instance(500),
                      new RunBehaviors(
                        MultiAttack.Instance(10, 40, 10, 70, projectileIndex: 1)
                    ),
                    SetSize.Instance(175),

                    Cooldown.Instance(500),
                      new RunBehaviors(
                        MultiAttack.Instance(10, 40, 10, 40, projectileIndex: 1)
                    ),

                    SetSize.Instance(150),

                    Cooldown.Instance(500),
                      new RunBehaviors(
                        MultiAttack.Instance(10, 40, 10, 60, projectileIndex: 1)
                    ),

                    SetSize.Instance(125),

                    Cooldown.Instance(500),
                      new RunBehaviors(
                        MultiAttack.Instance(10, 40, 10, 50, projectileIndex: 1)
                    ),

                    SetSize.Instance(100),

                    Cooldown.Instance(500),
                      new RunBehaviors(
                        MultiAttack.Instance(10, 40, 10, 70, projectileIndex: 1)
                    ),
                    UnsetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),
                    Cooldown.Instance(500),
                      new RunBehaviors(
                        MultiAttack.Instance(10, 40, 10, 40, projectileIndex: 1)
                    ),
                    Cooldown.Instance(500),
                      new RunBehaviors(
                        MultiAttack.Instance(10, 40, 10, 60, projectileIndex: 1)
                    ),
                    Cooldown.Instance(500),
                      new RunBehaviors(
                        MultiAttack.Instance(10, 40, 10, 50, projectileIndex: 1)
                    ),
                    SetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),
                    Cooldown.Instance(500),
                      new RunBehaviors(
                        MultiAttack.Instance(10, 40, 10, 70, projectileIndex: 1)
                    )

        #endregion

                ),
                loot: new LootBehavior(LootDef.Empty,
                    Tuple.Create(100, new LootDef(0, 8, 0, 8,
                        Tuple.Create(0.01, (ILoot)new ItemLoot("Crystal Wand")),
                        Tuple.Create(0.01, (ILoot)new ItemLoot("Crystal Sword")),
                        Tuple.Create(0.01, (ILoot)new ItemLoot("Crystal Dagger")),
                        Tuple.Create(0.001, (ILoot)new ItemLoot("Platinum Wand")),
                        Tuple.Create(0.001, (ILoot)new ItemLoot("Platinum Sword")),
                        Tuple.Create(0.5, (ILoot)new StatPotionLoot(StatPotion.Att)),
                        Tuple.Create(0.5, (ILoot)new StatPotionLoot(StatPotion.Def)),
                        Tuple.Create(0.5, (ILoot)new StatPotionLoot(StatPotion.Dex)),
                        Tuple.Create(0.5, (ILoot)new StatPotionLoot(StatPotion.Spd)),
                        Tuple.Create(0.5, (ILoot)new StatPotionLoot(StatPotion.Vit)),
                        Tuple.Create(0.5, (ILoot)new StatPotionLoot(StatPotion.Wis))
                        
                        )))
            ))
            .Init(0x0942, Behaves("Crystal Prisoner Clone",
                new RunBehaviors(
                    Cooldown.Instance(1000, SimpleAttack.Instance(10, projectileIndex: 0)),
                    Cooldown.Instance(7500, Despawn.Instance)
                ),
                IfNot.Instance(
                    Chasing.Instance(2, 10, 3, 0x0941),
                    SimpleWandering.Instance(2f)
                    )
            ))
            .Init(0x0943, Behaves("Crystal Prisoner Steed",
                new RunBehaviors(
                    Cooldown.Instance(1000, SimpleAttack.Instance(10, projectileIndex: 0))
                ),
                IfNot.Instance(
                    Chasing.Instance(2, 10, 5, 0x0941),
                    SimpleWandering.Instance(2f)
                    )
            ));
        /*.Init(0x0935, Behaves("Mysterious Crystal",
                    new State(
                        new State("Idle",
                            new Taunt(0.1, "Break the crystal for great rewards..."),
                            new Taunt(0.1, "Help me..."),
                            new HpLessTransition(0.9999, "Instructions"),
                            new TimedTransition(10000, "Idle")
                        ),
                        new State("Instructions",
                            new Flash(0xffffffff, 2, 100),
    						new Taunt(0.8, "Fire upon this crystal with all your might for 5 seconds"),
                            new Taunt(0.8, "If your attacks are weak, the crystal magically heals"),
                            new Taunt(0.8, "Gather a large group to smash it open"),
                            new HpLessTransition(0.998, "Evaluation")
                        ),
                        new State("Evaluation",
                            new State("Comment1",
                                new Taunt(true, "Sweet treasure awaits for powerful adventurers!"),
                                new Taunt(0.4, "Yes!  Smash my prison for great rewards!"),
                                new TimedTransition(5000, "Comment2")
                            ),
                            new State("Comment2",
                                new Taunt(0.3, "If you are not very strong, this could kill you",
                                               "If you are not yet powerful, stay away from the Crystal",
                                               "New adventurers should stay away",
                                               "That's the spirit. Lay your fire upon me.",
                                               "So close..."
                                ),
                                new TimedTransition(5000, "Comment3")
                            ),
                            new State("Comment3",
                                new Taunt(0.4, "I think you need more people...",
                                               "Call all your friends to help you break the crystal!"
                                ),
                                new TimedTransition(10000, "Comment2")
                            ),
                            new Heal(1, "Crystals", coolDown: 5000),
                            new HpLessTransition(0.95, "StartBreak"),
                            new TimedTransition(60000, "Fail")
							
							
							
							
                        ),
                        new State("Fail",
                            new SimpleTaunt("Perhaps you need a bigger group. Ask others to join you!"),
                            new Flash(0xff000000, 5, 1),
                            new Shoot(10, count: 16, shootAngle: 22.5, fixedAngle: 0, coolDown: 100000),
                            new Heal(1, "Crystals", coolDown: 1000),
                            new TimedTransition(5000, "Idle")
                        ),
                        new State("StartBreak",
                            new Taunt("You cracked the crystal! Soon we shall emerge!"),
                            new ChangeSize(-2, 80),
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new Flash(0xff000000, 2, 10),
                            new TimedTransition(4000, "BreakCrystal")
                        ),
                        new State("BreakCrystal",
                            new Taunt("This your reward! Imagine what evil even Oryx needs to keep locked up!"),
                            new Shoot(0, count: 16, shootAngle: 22.5, fixedAngle: 0, coolDown: 100000),
                            new Spawn("Crystal Prisoner", maxChildren: 1, initialSpawn: 1, coolDown: 100000),
                            new Decay(0)
                        )
                    )
                )
            .Init("Crystal Prisoner",
                    new State(
                        new SpawnMinionImmidiate("Crystal Prisoner Steed", maxChildren: 3, initialSpawn: 0, coolDown: 200),
                        new State("pause",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new TimedTransition(2000, "start_the_fun")
                        ),
                        new State("start_the_fun",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new Taunt("I'm finally free! Yesss!!!"),
                            new TimedTransition(1500, "Daisy_attack")
                        ),

                        new State("Daisy_attack",
                            new Prioritize(
                                new StayCloseToSpawn(0.3, range: 7),
                                new Wander(0.3)
                            ),
                            new State("Quadforce1",
                                new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                                new Shoot(0, projectileIndex: 0, count: 4, shootAngle: 90, fixedAngle: 0, coolDown: 300),
                                new TimedTransition(200, "Quadforce2")
                            ),
                            new State("Quadforce2",
                                new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                                new Shoot(0, projectileIndex: 0, count: 4, shootAngle: 90, fixedAngle: 15, coolDown: 300),
                                new TimedTransition(200, "Quadforce3")
                            ),
                            new State("Quadforce3",
                                new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                                new Shoot(0, projectileIndex: 0, count: 4, shootAngle: 90, fixedAngle: 30, coolDown: 300),
                                new TimedTransition(200, "Quadforce4")
                            ),
                            new State("Quadforce4",
                                new Shoot(10, projectileIndex: 3, coolDown: 1000),
                                new Shoot(0, projectileIndex: 0, count: 4, shootAngle: 90, fixedAngle: 45, coolDown: 300),
                                new TimedTransition(200, "Quadforce5")
                            ),
                            new State("Quadforce5",
                                new Shoot(0, projectileIndex: 0, count: 4, shootAngle: 90, fixedAngle: 60, coolDown: 300),
                                new TimedTransition(200, "Quadforce6")
                            ),
                            new State("Quadforce6",
                                new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                                new Shoot(0, projectileIndex: 0, count: 4, shootAngle: 90, fixedAngle: 75, coolDown: 300),
                                new TimedTransition(200, "Quadforce7")
                            ),
                            new State("Quadforce7",
                                new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                                new Shoot(0, projectileIndex: 0, count: 4, shootAngle: 90, fixedAngle: 90, coolDown: 300),
                                new TimedTransition(200, "Quadforce8")
                            ),
                            new State("Quadforce8",
                                new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                                new Shoot(10, projectileIndex: 3, coolDown: 1000),
                                new Shoot(0, projectileIndex: 0, count: 4, shootAngle: 90, fixedAngle: 105, coolDown: 300),
                                new TimedTransition(200, "Quadforce1")
                            ),
                            new HpLessTransition(0.3, "Whoa_nelly"),
                            new TimedTransition(18000, "Warning")
                        ),

                        new State("Warning",
                            new Prioritize(
                                new StayCloseToSpawn(0.5, range: 7),
                                new Wander(0.5)
                            ),
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new Flash(0xff00ff00, 0.2, 15),
                            new Follow(0.4, acquireRange: 9, range: 2),
                            new TimedTransition(3000, "Summon_the_clones")
                        ),
                        new State("Summon_the_clones",
                            new Prioritize(
                                new StayCloseToSpawn(0.85, range: 7),
                                new Wander(0.85)
                            ),
                            new Shoot(10, projectileIndex: 0, coolDown: 1000),

                            new Spawn("Crystal Prisoner Clone", maxChildren: 4, initialSpawn: 0, coolDown: 200),
                            new TossObject("Crystal Prisoner Clone", range: 5, angle: 0, coolDown: 100000),
                            new TossObject("Crystal Prisoner Clone", range: 5, angle: 240, coolDown: 100000),
                            new TossObject("Crystal Prisoner Clone", range: 7, angle: 60, coolDown: 100000),
                            new TossObject("Crystal Prisoner Clone", range: 7, angle: 300, coolDown: 100000),

                            new State("invulnerable_clone",
                                new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                                new TimedTransition(3000, "vulnerable_clone")
                            ),
                            new State("vulnerable_clone",
                                new TimedTransition(1200, "invulnerable_clone")
                            ),
                            new TimedTransition(16000, "Warning2")
                        ),

                        new State("Warning2",
                            new Prioritize(
                                new StayCloseToSpawn(0.85, range: 7),
                                new Wander(0.85)
                            ),
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new Flash(0xff00ff00, 0.2, 25),
                            new TimedTransition(5000, "Whoa_nelly")
                        ),
                        new State("Whoa_nelly",
                            new Prioritize(
                                new StayCloseToSpawn(0.6, range: 7),
                                new Wander(0.6)
                            ),

                            new Shoot(10, projectileIndex: 3, count: 3, shootAngle: 120, coolDown: 900),
                            new Shoot(10, projectileIndex: 2, count: 3, shootAngle: 15, fixedAngle: 40, coolDown: 1600, coolDownOffset: 0),
                            new Shoot(10, projectileIndex: 2, count: 3, shootAngle: 15, fixedAngle: 220, coolDown: 1600, coolDownOffset: 0),
                            new Shoot(10, projectileIndex: 2, count: 3, shootAngle: 15, fixedAngle: 130, coolDown: 1600, coolDownOffset: 800),
                            new Shoot(10, projectileIndex: 2, count: 3, shootAngle: 15, fixedAngle: 310, coolDown: 1600, coolDownOffset: 800),

                            new State("invulnerable_whoa",
                                new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                                new TimedTransition(2600, "vulnerable_whoa")
                            ),
                            new State("vulnerable_whoa",
                                new TimedTransition(1200, "invulnerable_whoa")
                            ),
                            new TimedTransition(10000, "Absolutely_Massive")
                        ),

                        new State("Absolutely_Massive",
                            new ChangeSize(13, 260),
                            new Prioritize(
                                new StayCloseToSpawn(0.2, range: 7),
                                new Wander(0.2)
                            ),

                            new Shoot(10, projectileIndex: 1, count: 9, shootAngle: 40, fixedAngle: 40, coolDown: 2000, coolDownOffset: 400),
                            new Shoot(10, projectileIndex: 1, count: 9, shootAngle: 40, fixedAngle: 60, coolDown: 2000, coolDownOffset: 800),
                            new Shoot(10, projectileIndex: 1, count: 9, shootAngle: 40, fixedAngle: 50, coolDown: 2000, coolDownOffset: 1200),
                            new Shoot(10, projectileIndex: 1, count: 9, shootAngle: 40, fixedAngle: 70, coolDown: 2000, coolDownOffset: 1600),

                            new State("invulnerable_mass",
                                new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                                new TimedTransition(2600, "vulnerable_mass")
                            ),
                            new State("vulnerable_mass",
                                new TimedTransition(1000, "invulnerable_mass")
                            ),
                            new TimedTransition(14000, "Start_over_again")
                        ),

                        new State("Start_over_again",
                            new ChangeSize(-20, 100),
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new Flash(0xff00ff00, 0.2, 15),
                            new TimedTransition(3000, "Daisy_attack")
                        )
                    ),
                    new ItemLoot("Potion of Vitality", 1),
                    new Threshold(0.015,
                        new TierLoot(2, ItemType.Potion, 0.07)
                    ),
                    new Threshold(0.03,
                        new ItemLoot("Crystal Wand", 0.05),
                        new ItemLoot("Crystal Sword", 0.06)
                    )
                )
            .Init("Crystal Prisoner Clone",
                    new State(
                        new Prioritize(
                            new StayCloseToSpawn(0.85, range: 5),
                            new Wander(0.85)
                        ),
                        new Shoot(10, coolDown: 1400),
                        new State("taunt",
                            new Taunt(0.09, "I am everywhere and nowhere!"),
                            new TimedTransition(1000, "no_taunt")
                        ),
                        new State("no_taunt",
                            new TimedTransition(1000, "taunt")
                        ),
                        new Decay(17000)
                    )
                )
            .Init("Crystal Prisoner Steed",
                    new State(
                        new State("change_position_fast",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new Prioritize(
                                new StayCloseToSpawn(3.6, range: 12),
                                new Wander(3.6)
                            ),
                            new TimedTransition(800, "attack")
                        ),
                        new State("attack",
                            new Shoot(10, predictive: 0.3, coolDown: 500),
                            new State("keep_distance",
                                new Prioritize(
                                    new StayCloseToSpawn(1, range: 12),
                                    new Orbit(1, 9, target: "Crystal Prisoner", radiusVariance: 0)
                                ),
                                new TimedTransition(2000, "go_anywhere")
                            ),
                            new State("go_anywhere",
                                new Prioritize(
                                    new StayCloseToSpawn(1, range: 12),
                                    new Wander(1)
                                ),
                                new TimedTransition(2000, "keep_distance")
                            )
                        )
                    )
                );
        */
        /*
            .Init(0x0935, Behaves("Mysterious Crystal",
                    attack: new RunBehaviors(
                        new State("idle",
                            new RandomTaunt(0.1, "Break the crystal for great rewards..."),
                            new RandomTaunt(0.1, "Help me..."),
                            HpLesserPercent.Instance(0.9f, SetState.Instance("Instructions")),
                            CooldownExact.Instance(10000, SetState.Instance("idle"))
                        ),
                        new State("Instructions",
                            Flashing.Instance(100, 0xffffffff),
                            new RandomTaunt(0.8, "Fire upon this crystal with all your might for 5 seconds"),
                            new RandomTaunt(0.8, "If your attacks are weak, the crystal magically heals"),
                            new RandomTaunt(0.8, "Gather a large group to smash it open"),
                            HpLesserPercent.Instance((float)0.998, SetState.Instance("Evaluation.Comment1"))
                        ),
                        new State("Evaluation",
                            new SubState("Comment1",
                                new SimpleTaunt("Sweet treasure awaits for powerful adventurers!"),
                                new RandomTaunt(0.4, "Yes!  Smash my prison for great rewards!"),
                                CooldownExact.Instance(5000, SetState.Instance(".Comment2"))
                            ),
                            new SubState("Comment2",
                                new RandomTaunt(0.3, "If you are not very strong, this could kill you"),
                                new RandomTaunt(0.3, "If you are not yet powerful, stay away from the Crystal"),
                                new RandomTaunt(0.3, "New adventurers should stay away"),
                                new RandomTaunt(0.3, "That's the spirit. Lay your fire upon me."),
                                new RandomTaunt(0.3, "So close..."),
                                CooldownExact.Instance(5000, SetState.Instance(".Comment3"))
                            ),
                            new SubState("Comment3",
                                new RandomTaunt(0.4, "I think you need more people..."),
                                new RandomTaunt(0.4, "Call all your friends to help you break the crystal!"),
                                CooldownExact.Instance(10000, SetState.Instance(".Comment2"))
                            ),
                            Cooldown.Instance(5000, Heal.Instance(1, 999999, 0x0935)),
                            HpLesserPercent.Instance((float)0.95, SetState.Instance("StartBreak")),
                            CooldownExact.Instance(60000, SetState.Instance("Fail"))
                        ),
                        new State("Fail",
                            new SimpleTaunt("Perhaps you need a bigger group. Ask others to join you!"),
                            Flashing.Instance(1000, 0xff000000),
                            MultiAttack.Instance(10, numShot: 16, angle: (float)22.5)),
                            Cooldown.Instance(1000, Heal.Instance(1, 999999, 0x0935)),
                            CooldownExact.Instance(5000, SetState.Instance("idle"))
                        )
                        )
                        );*/
                        /*new State("StartBreak",
                            new Taunt("You cracked the crystal! Soon we shall emerge!"),
                            new ChangeSize(-2, 80),
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new Flash(0xff000000, 2, 10),
                            new TimedTransition(4000, "BreakCrystal")
                        ),
                        new State("BreakCrystal",
                            new Taunt("This your reward! Imagine what evil even Oryx needs to keep locked up!"),
                            new Shoot(0, count: 16, shootAngle: 22.5, fixedAngle: 0, coolDown: 100000),
                            new Spawn("Crystal Prisoner", maxChildren: 1, initialSpawn: 1, coolDown: 100000),
                            new Decay(0)
                        )
                    )
                );*/
    }
}
