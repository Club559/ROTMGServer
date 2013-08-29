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
        static _ Misc = Behav()
            .Init(0x01c7, Behaves("White Fountain",
                    MagicEye.Instance,
                    Cooldown.Instance(600, new NexusHealHp())
                ))
            .Init(0x6d2, Behaves("Red Satellite",
                    StrictCirclingGroup.Instance(3, 5, "Golems"),
                    GolemSatellites.Instance
                ))
            .Init(0x6d3, Behaves("Green Satellite",
                    StrictCirclingGroup.Instance(3, 5, "Golems"),
                    GolemSatellites.Instance
                ))
            .Init(0x6d4, Behaves("Blue Satellite",
                    StrictCirclingGroup.Instance(3, 5, "Golems"),
                    GolemSatellites.Instance
                ))

            .Init(0x6d5, Behaves("Gray Satellite 1",
                    StrictCirclingGroup.Instance(1f, 10, "Golem Satellites"),
                    new RunBehaviors(
                        Cooldown.Instance(500, SimpleAttack.Instance(5)),
                        GolemSatellites.Instance
                    )
                ))
            .Init(0x6d6, Behaves("Gray Satellite 2",
                    StrictCirclingGroup.Instance(1f, 10, "Golem Satellites"),
                    new RunBehaviors(
                        Cooldown.Instance(500, SimpleAttack.Instance(5)),
                        GolemSatellites.Instance
                    )
                ))
            .Init(0x6d7, Behaves("Gray Satellite 3",
                    StrictCirclingGroup.Instance(1f, 10, "Golem Satellites"),
                    new RunBehaviors(
                        Cooldown.Instance(500, SimpleAttack.Instance(5)),
                        GolemSatellites.Instance
                    )
                ))
            .Init(0x01ff, Behaves("Sheep",
                    new QueuedBehavior(
                        RandomDelay.Instance(2500, 7500),
                        SimpleWandering.Instance(2, 2)
                    ),
                    Cooldown.Instance(1000,
                        Rand.Instance(
                            new RandomTaunt(0.001, "baa"),
                            new RandomTaunt(0.001, "baa baa")
                        )
                    ),
                    condBehaviors: new ConditionalBehavior[] {
                        new ChatEvent(new SimpleTaunt("Yes, master?"))
                        .SetChats(new string[]{ "baa baa baa" })
                    }
                ))
            .Init(0x2002, Behaves("HeartZ",
                new QueuedBehavior(
                    CooldownExact.Instance(500, SetSize.Instance(220)),
                    CooldownExact.Instance(500, SetSize.Instance(200))
                ),
                new RunBehaviors(
                    CooldownExact.Instance(12500, new SimpleTaunt("Ouch! Please spare my other {HP} hitpoints!"))
                ),
                loot: new LootBehavior(LootDef.Empty,
                        Tuple.Create(100, new LootDef(0, 5, 0, 8,
                            Tuple.Create(0.25, (ILoot)new TierLoot(3, ItemType.Armor)),

                            Tuple.Create(0.005, (ILoot)new ItemLoot("Tome of Nothing"))
                )))
            ))
            .Init(0x5e49, Behaves("Candy Gnome",
                    new RunBehaviors(
                        MaintainDist.Instance(9, 10, 7, null)
                        )
                ))
                .Init(0x193b, Behaves("Mega Sheep",
                    new RunBehaviors(
                                Timed.Instance(2500, Flashing.Instance(200, 0xf389E13)),
                                Cooldown.Instance(450, MultiAttack.Instance(12, 10 * (float)Math.PI / 180, 5, 6f, projectileIndex: 0)),
                                Once.Instance(TossEnemy.Instance(24 * (float)Math.PI / 180, 9, 0x195c)),
                                Once.Instance(TossEnemy.Instance(48 * (float)Math.PI / 180, 9, 0x195c)),
                                Once.Instance(TossEnemy.Instance(72 * (float)Math.PI / 180, 9, 0x195c)),
                                Once.Instance(TossEnemy.Instance(96 * (float)Math.PI / 180, 9, 0x195c)),
                                Once.Instance(TossEnemy.Instance(120 * (float)Math.PI / 180, 9, 0x195c)),
                                Once.Instance(TossEnemy.Instance(144 * (float)Math.PI / 180, 9, 0x195c)),
                                Once.Instance(TossEnemy.Instance(168 * (float)Math.PI / 180, 9, 0x195c)),
                                Once.Instance(TossEnemy.Instance(192 * (float)Math.PI / 180, 9, 0x195c)),
                                Once.Instance(TossEnemy.Instance(216 * (float)Math.PI / 180, 9, 0x195c)),
                                Once.Instance(TossEnemy.Instance(240 * (float)Math.PI / 180, 9, 0x195c)),
                                Once.Instance(TossEnemy.Instance(264 * (float)Math.PI / 180, 9, 0x195c)),
                                Once.Instance(TossEnemy.Instance(288 * (float)Math.PI / 180, 9, 0x195c)),
                                Once.Instance(TossEnemy.Instance(312 * (float)Math.PI / 180, 9, 0x195c)),
                                Once.Instance(TossEnemy.Instance(336 * (float)Math.PI / 180, 9, 0x195c)),
                                Once.Instance(TossEnemy.Instance(360 * (float)Math.PI / 180, 9, 0x195c)),
                                CooldownExact.Instance(10000, SetState.Instance("WildWildWest"))
                    ),
                    new State("WildWildWest",
                        new QueuedBehavior(
                        Once.Instance(new SimpleTaunt("BAA BAA!")),
                        Cooldown.Instance(450, MultiAttack.Instance(12, 10 * (float)Math.PI / 180, 5, 6f, projectileIndex: 0)),
                        Flashing.Instance(10000, 0xFF000000),
                                    Cooldown.Instance(250, new RunBehaviors(RingAttack.Instance(3, offset: 0 * (float)Math.PI / 180, projectileIndex: 0))),
                                    Cooldown.Instance(250, new RunBehaviors(RingAttack.Instance(3, offset: 18 * (float)Math.PI / 180, projectileIndex: 0))),
                                    Cooldown.Instance(250, new RunBehaviors(RingAttack.Instance(3, offset: 36 * (float)Math.PI / 180, projectileIndex: 0))),
                                    Cooldown.Instance(250, new RunBehaviors(RingAttack.Instance(3, offset: 54 * (float)Math.PI / 180, projectileIndex: 0))),
                                    Cooldown.Instance(250, new RunBehaviors(RingAttack.Instance(3, offset: 72 * (float)Math.PI / 180, projectileIndex: 0))),
                                    Cooldown.Instance(250, new RunBehaviors(RingAttack.Instance(3, offset: 90 * (float)Math.PI / 180, projectileIndex: 0))),
                                    Cooldown.Instance(250, new RunBehaviors(RingAttack.Instance(3, offset: 108 * (float)Math.PI / 180, projectileIndex: 0))),
                                    Cooldown.Instance(250, new RunBehaviors(RingAttack.Instance(3, offset: 126 * (float)Math.PI / 180, projectileIndex: 0))),
                                    Cooldown.Instance(250, new RunBehaviors(RingAttack.Instance(3, offset: 144 * (float)Math.PI / 180, projectileIndex: 0))),
                                    Cooldown.Instance(250, new RunBehaviors(RingAttack.Instance(3, offset: 162 * (float)Math.PI / 180, projectileIndex: 0))),
                                    Cooldown.Instance(250, new RunBehaviors(RingAttack.Instance(3, offset: 180 * (float)Math.PI / 180, projectileIndex: 0))),
                                    Cooldown.Instance(250, new RunBehaviors(RingAttack.Instance(3, offset: 198 * (float)Math.PI / 180, projectileIndex: 0))),
                                    Cooldown.Instance(250, new RunBehaviors(RingAttack.Instance(3, offset: 216 * (float)Math.PI / 180, projectileIndex: 0))),
                                    Cooldown.Instance(250, new RunBehaviors(RingAttack.Instance(3, offset: 234 * (float)Math.PI / 180, projectileIndex: 0))),
                                    Cooldown.Instance(250, new RunBehaviors(RingAttack.Instance(3, offset: 252 * (float)Math.PI / 180, projectileIndex: 0))),
                                    Cooldown.Instance(250, new RunBehaviors(RingAttack.Instance(3, offset: 270 * (float)Math.PI / 180, projectileIndex: 0))),
                                    Cooldown.Instance(250, new RunBehaviors(RingAttack.Instance(3, offset: 288 * (float)Math.PI / 180, projectileIndex: 0))),
                                    Cooldown.Instance(250, new RunBehaviors(RingAttack.Instance(3, offset: 306 * (float)Math.PI / 180, projectileIndex: 0))),
                                    Cooldown.Instance(250, new RunBehaviors(RingAttack.Instance(3, offset: 324 * (float)Math.PI / 180, projectileIndex: 0))),
                                    Cooldown.Instance(250, new RunBehaviors(RingAttack.Instance(3, offset: 342 * (float)Math.PI / 180, projectileIndex: 0)))
                        )
                    ),
                    Cooldown.Instance(1000,
                        Rand.Instance(
                            new RandomTaunt(0.001, "baa"),
                            new RandomTaunt(0.001, "baa baa")
                        )
                    )
                ))

                .Init(0x195c, Behaves("Mega Sheep Minion",
                    new RunBehaviors(
                        SetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),
                        If.Instance(IsEntityNotPresent.Instance(30, 0x193b),
                        Chasing.Instance(8, 10, 0, null)),
                        If.Instance(IsEntityNotPresent.Instance(30, 0x193b),
                        UnsetConditionEffect.Instance(ConditionEffectIndex.Invulnerable)),
                        If.Instance(IsEntityNotPresent.Instance(30, 0x193b),
                        Once.Instance(new SimpleTaunt("You Killed the Master!"))),
                        If.Instance(IsEntityNotPresent.Instance(30, 0x193b),
                        Cooldown.Instance(300,
                        Once.Instance(new SimpleTaunt("CHARGE!"))))
                    ),
                    Cooldown.Instance(1000,
                        Rand.Instance(
                            new RandomTaunt(0.001, "baa"),
                            new RandomTaunt(0.001, "baa baa")
                        )
                    ),
                    condBehaviors: new ConditionalBehavior[] {
                        new ChatEvent(new SimpleTaunt("I dont know who you are \n But I will find you \n And I will kill you"))
                        .SetChats(new string[]{ "baa baa baa" })
                    }
                ));
    }
}
