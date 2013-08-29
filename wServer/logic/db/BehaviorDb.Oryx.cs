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
        static _ Oryx = Behav()
            .Init(0x0932, Behaves("Oryx the Mad God 2",
                    new RunBehaviors(
                        new State("idle",
                        Cooldown.Instance(1000, MultiAttack.Instance(25, 10 * (float)Math.PI / 180, 3, 0, projectileIndex: 1)),
                        Cooldown.Instance(1000, MultiAttack.Instance(25, 10 * (float)Math.PI / 180, 3, 0, projectileIndex: 2)),
                        Cooldown.Instance(1000, MultiAttack.Instance(25, 10 * (float)Math.PI / 180, 2, 0, projectileIndex: 3)),
                        //Cooldown.Instance(1000, MultiAttack.Instance(25, 10 * (float)Math.PI / 180, 3, 0, projectileIndex: 4)),
                        Cooldown.Instance(1000, MultiAttack.Instance(25, 10 * (float)Math.PI / 180, 2, 0, projectileIndex: 5)),
                        Cooldown.Instance(1000, MultiAttack.Instance(25, 10 * (float)Math.PI / 180, 3, 0, projectileIndex: 6)),
                        Cooldown.Instance(6000, new SimpleTaunt("Puny mortals! My {HP} HP will annihilate you!"))
                        )
                    ),
                    new RunBehaviors(
                        new State("idle",
                        HpGreaterEqual.Instance(15000,
                            new RunBehaviors(
                                MaintainDist.Instance(1, 5, 15, null),
                                Cooldown.Instance(3600, MultiAttack.Instance(25, 45 * (float)Math.PI / 180, 8, 0, projectileIndex: 0)),
                                SpawnMinion.Instance(0x0944, 2, 4, 12000, 12000)
                            )
                        ),
                        HpLesserPercent.Instance(0.2f,
                            new RunBehaviors(
                                Chasing.Instance(3, 25, 2, null),
                                Cooldown.Instance(2200, MultiAttack.Instance(25, 45 * (float)Math.PI / 180, 8, 0, projectileIndex: 0)),
                                Once.Instance(SetConditionEffect.Instance(ConditionEffectIndex.Invulnerable)),
                                Once.Instance(new SimpleTaunt("Can't... keep... henchmen... alive... anymore! ARGHHH!!!")),
                                Cooldown.Instance(8000, Once.Instance(UnsetConditionEffect.Instance(ConditionEffectIndex.Invulnerable))),
                                Cooldown.Instance(8000, Once.Instance(RingAttack.Instance(30, 0, 2, projectileIndex: 7))),
                                Cooldown.Instance(8000, Once.Instance(RingAttack.Instance(30, projectileIndex: 8))),
                                Cooldown.Instance(1000, TossEnemy.Instance(new Random().Next(0, 179) * (float)Math.PI / 180, 7, 0x0de2))
                            )
                        )
                        )
                    ),
                    new RunBehaviors(
                        new State("dance",
                            Once.Instance(new SimpleTaunt("I will dance for you!")),
                            Cooldown.Instance(500, PredictiveMultiAttack.Instance(25, 10 * (float)Math.PI / 180, 3, 5, projectileIndex: 1)),
                            Cooldown.Instance(500, PredictiveMultiAttack.Instance(25, 10 * (float)Math.PI / 180, 3, 5, projectileIndex: 2)),
                            Cooldown.Instance(500, PredictiveMultiAttack.Instance(25, 10 * (float)Math.PI / 180, 2, 5, projectileIndex: 3)),
                            Cooldown.Instance(500, PredictiveMultiAttack.Instance(25, 10 * (float)Math.PI / 180, 3, 5, projectileIndex: 4)),
                            Cooldown.Instance(500, PredictiveMultiAttack.Instance(25, 10 * (float)Math.PI / 180, 2, 5, projectileIndex: 5)),
                            Cooldown.Instance(500, PredictiveMultiAttack.Instance(25, 10 * (float)Math.PI / 180, 3, 5, projectileIndex: 6))
                        )
                    ),
                    loot: new LootBehavior(LootDef.Empty,
                        Tuple.Create(100, new LootDef(0, 5, 0, 8,
                            Tuple.Create(0.001, (ILoot)new TierLoot(8, ItemType.Ability)),
                            Tuple.Create(0.005, (ILoot)new TierLoot(7, ItemType.Ability)),
                            Tuple.Create(0.05, (ILoot)new TierLoot(6, ItemType.Ability)),
                            Tuple.Create(0.001, (ILoot)new TierLoot(15, ItemType.Armor)),
                            Tuple.Create(0.005, (ILoot)new TierLoot(14, ItemType.Armor)),
                            Tuple.Create(0.05, (ILoot)new TierLoot(13, ItemType.Armor)),
                            Tuple.Create(0.001, (ILoot)new TierLoot(14, ItemType.Weapon)),
                            Tuple.Create(0.005, (ILoot)new TierLoot(13, ItemType.Weapon)),
                            Tuple.Create(0.05, (ILoot)new TierLoot(12, ItemType.Weapon)),
                            Tuple.Create(0.2, (ILoot)new TierLoot(5, ItemType.Ring)),

                            Tuple.Create(0.005, (ILoot)new ItemLoot("Potion of Oryx")),

                            Tuple.Create(0.5, (ILoot)new StatPotionLoot(StatPotion.Att)),
                            Tuple.Create(0.5, (ILoot)new StatPotionLoot(StatPotion.Wis)),
                            Tuple.Create(0.5, (ILoot)new StatPotionLoot(StatPotion.Vit)),
                            Tuple.Create(0.5, (ILoot)new StatPotionLoot(StatPotion.Spd))
                    ))),
                    condBehaviors: new ConditionalBehavior[] {
                        new ChatEvent(
                            SetState.Instance("dance")
                        ).SetChats("Dance, Oryx!").AdminOnly(),

                        new DeathPortal(0x1901, 100)
                    }
            ))
            .Init(0x0944, Behaves("Henchman of Oryx",
                new QueuedBehavior(
                    Timed.Instance(2500, Circling.Instance(2, 20, 3, 0x0932)),
                    Timed.Instance(2000, Chasing.Instance(2, 11, 4, null))
                ),
                new RunBehaviors(
                    Cooldown.Instance(1500, PredictiveAttack.Instance(12, 1, projectileIndex: 0)),
                    Cooldown.Instance(1500, MultiAttack.Instance(12, 20 * (float)Math.PI / 180, 3, projectileIndex: 1))
                ),
                new RunBehaviors(
                    SpawnMinion.Instance(0x0de3, 20, 1, 7000, 7000),
                    SpawnMinion.Instance(0x0de1, 20, 1, 7000, 7000)
                )
            ))
            .Init(0x0de3, Behaves("Aberrant of Oryx",
                new RunBehaviors(
                    SimpleWandering.Instance(3),
                    Chasing.Instance(7, 9, 12, null),
                    Circling.Instance(4, 15, 5, 0x0944)
                ),
                reproduce: new RunBehaviors(
                    Cooldown.Instance(1500, TossEnemy.Instance(new Random().Next(0, 179) * (float)Math.PI / 180, 7, 0x0de4))
                )
            ))
            .Init(0x0de4, Behaves("Aberrant Blaster",
                attack: new RunBehaviors(
                    If.Instance(
                        IsEntityPresent.Instance(5, null),
                        new QueuedBehavior(
                            MultiAttack.Instance(8, 10 * (float)Math.PI / 180, 6),
                            Die.Instance
                        )
                    ),
                    CooldownExact.Instance(1500, Die.Instance)
                )
            ))
            .Init(0x0de1, Behaves("Monstrosity of Oryx",
                new RunBehaviors(
                    SimpleWandering.Instance(3),
                    Chasing.Instance(7, 9, 12, null),
                    Circling.Instance(4, 15, 5, 0x0944)
                ),
                reproduce: new RunBehaviors(
                    If.Instance(
                        IsEntityPresent.Instance(8, null),
                        Cooldown.Instance(1000, SpawnMinionImmediate.Instance(0x0de2, 0, 1, 1))
                    )
                )
            ))
            .Init(0x0de2, Behaves("Monstrosity Scarab",
                attack: new RunBehaviors(
                    If.Instance(
                        IsEntityPresent.Instance(8, null),
                        new RunBehaviors(
                            Chasing.Instance(18, 10, 1, null),
                            If.Instance(
                                IsEntityPresent.Instance(3, null),
                                new QueuedBehavior(
                                    RingAttack.Instance(12),
                                    Die.Instance
                                )
                            )
                        )
                    ),
                    CooldownExact.Instance(1500, Die.Instance)
                )
            ))


            .Init(0x2001, Behaves("Oryx the Mad God 3",
                new RunBehaviors(
                    new State("idle",
                        SetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),
                        IfNot.Instance(
                            IsEntityPresent.Instance(20000, 0x1902),
                            SetState.Instance("flash")
                        )
                    ),
                    new State("flash",
                        SetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),
                        Once.Instance(new RunBehaviors(new SimpleTaunt("Mischevious runts. I will destroy you once and for all."))),
                        Timed.Instance(24000, False.Instance(Flashing.Instance(250, 0xffff0000))),
                        CooldownExact.Instance(5000, Once.Instance(new SimpleTaunt("You have killed me many times."))),
                        CooldownExact.Instance(10000, Once.Instance(new SimpleTaunt("I have been humiliated over and over again."))),
                        CooldownExact.Instance(15000, Once.Instance(new SimpleTaunt("Now, I will slaughter you into a million pieces."))),
                        CooldownExact.Instance(20000, Once.Instance(new SimpleTaunt("Your life is worthless!"))),
                        CooldownExact.Instance(22000, Once.Instance(new SimpleTaunt("Prepare to die!"))),
                        CooldownExact.Instance(24000, UnsetConditionEffect.Instance(ConditionEffectIndex.Invulnerable)),
                        CooldownExact.Instance(24000, SetState.Instance("battle")),
                        new QueuedBehavior(
                            PlaySound.Instance(1),
                            CooldownExact.Instance(5000, PlaySound.Instance()),
                            CooldownExact.Instance(5000, PlaySound.Instance()),
                            CooldownExact.Instance(5000, PlaySound.Instance()),
                            CooldownExact.Instance(5000, PlaySound.Instance()),
                            CooldownExact.Instance(2000, PlaySound.Instance()),
                            CooldownExact.Instance(2000, PlaySound.Instance(2))
                        )
                    ),
                    new State("battle",
                        Chasing.Instance(1, 10, 5, null),
                        Cooldown.Instance(1500, PredictiveMultiAttack.Instance(25, 10 * (float)Math.PI / 180, 3, 1, projectileIndex: 1)),
                        Cooldown.Instance(1500, PredictiveMultiAttack.Instance(25, 10 * (float)Math.PI / 180, 3, 1, projectileIndex: 2)),
                        Cooldown.Instance(1500, PredictiveMultiAttack.Instance(25, 10 * (float)Math.PI / 180, 2, 1, projectileIndex: 3)),
                        Cooldown.Instance(1500, PredictiveMultiAttack.Instance(25, 10 * (float)Math.PI / 180, 3, 1, projectileIndex: 4)),
                        Cooldown.Instance(1500, PredictiveMultiAttack.Instance(25, 10 * (float)Math.PI / 180, 3, 1, projectileIndex: 9)),
                        Cooldown.Instance(1500, PredictiveMultiAttack.Instance(25, 10 * (float)Math.PI / 180, 2, 1, projectileIndex: 5)),
                        Cooldown.Instance(1500, PredictiveMultiAttack.Instance(25, 10 * (float)Math.PI / 180, 3, 1, projectileIndex: 6)),
                        Cooldown.Instance(5000, RingAttack.Instance(16, projectileIndex: 9)),
                        If.Instance(CheckConditionEffects.Instance(new ConditionEffects[] { ConditionEffects.Stunned }),
                                    CooldownExact.Instance(3000, SpawnMinionImmediate.Instance(0x2003, 5, 1, 1))
                        ),
                        HpLesserPercent.Instance(0.1f, SetState.Instance("neardeath"))
                    ),
                    new State("neardeath",
                        Once.Instance(new SimpleTaunt("I must live on!")),
                        Cooldown.Instance(1000, MultiAttack.Instance(25, 50 * (float)Math.PI / 180, 3, 0, projectileIndex: 1)),
                        Cooldown.Instance(1000, MultiAttack.Instance(25, 50 * (float)Math.PI / 180, 3, 0, projectileIndex: 2)),
                        Cooldown.Instance(1000, MultiAttack.Instance(25, 50 * (float)Math.PI / 180, 2, 0, projectileIndex: 3)),
                        Cooldown.Instance(1000, MultiAttack.Instance(25, 50 * (float)Math.PI / 180, 3, 0, projectileIndex: 4)),
                        Cooldown.Instance(1000, MultiAttack.Instance(25, 50 * (float)Math.PI / 180, 3, 0, projectileIndex: 9)),
                        Cooldown.Instance(1000, MultiAttack.Instance(25, 50 * (float)Math.PI / 180, 2, 0, projectileIndex: 5)),
                        Cooldown.Instance(1000, MultiAttack.Instance(25, 50 * (float)Math.PI / 180, 3, 0, projectileIndex: 6)),
                        If.Instance(CheckConditionEffects.Instance(new ConditionEffects[] { ConditionEffects.Stunned }),
                                    CooldownExact.Instance(3000, SpawnMinionImmediate.Instance(0x2003, 5, 1, 1))
                        ),
                        new QueuedBehavior(
                            Cooldown.Instance(2000, RingAttack.Instance(8, projectileIndex: 9)),
                            Cooldown.Instance(2000, RingAttack.Instance(8, offset: 90, projectileIndex: 9))
                        )
                    )
                ),
                loot: new LootBehavior(LootDef.Empty,
                        Tuple.Create(100, new LootDef(0, 5, 0, 8,
                            Tuple.Create(0.1, (ILoot)new TierLoot(8, ItemType.Ability)),
                            Tuple.Create(0.1, (ILoot)new TierLoot(15, ItemType.Armor)),
                            Tuple.Create(0.1, (ILoot)new TierLoot(14, ItemType.Weapon)),
                            Tuple.Create(0.001, (ILoot)new ItemLoot("Tome of Noble Assault")),
                            Tuple.Create(0.01, (ILoot)new ItemLoot("Potion of Oryx"))
                ))),
                condBehaviors: new ConditionalBehavior[] {
                    new OnDeath(new SimpleTaunt("I'm such a weakling...")),
                }
            ))
            .Init(0x1902, Behaves("Magical Tree",
                condBehaviors: new ConditionalBehavior[] {
                    new OnDeath(
                        If.Instance(
                            IsEntityPresent.Instance(20000, 0x1902),
                            Rand.Instance(
                                new SimpleTaunt("We protect the lord!"),
                                new SimpleTaunt("Go into battle if you wish!"),
                                new SimpleTaunt("You are food for his minions!")
                            )
                        )
                    )
                }
            ))
            .Init(0x2003, Behaves("Knight of the Void",
                new RunBehaviors(
                    Chasing.Instance(15, 10, 2, null),
                    new QueuedBehavior(
                        Cooldown.Instance(250, SimpleAttack.Instance(5)),
                        Cooldown.Instance(250, PredictiveAttack.Instance(5, 0.2f))
                    )
                ),
                loot: new LootBehavior(LootDef.Empty,
                        Tuple.Create(100, new LootDef(0, 1, 0, 8,
                            Tuple.Create(0.0001, (ILoot)new ItemLoot("Sword of Dark Enchantment")) //Currently rarest item in the game?
                )))
            ))
            .Init(0x1740, Behaves("Oryx the Mad God 1",
                    new RunBehaviors(
                        IfExist.Instance(-1, NullBehavior.Instance,
                            new RunBehaviors(
                                new QueuedBehavior(
                                    CooldownExact.Instance(400)
                                ),
                                Once.Instance(new SimpleTaunt("Fools! I still have {HP} hitpoints!")),
                                new QueuedBehavior(new SetKey(-1, 1))

                            )
                        ),
                        IfEqual.Instance(-1, 1,
                            new RunBehaviors(
                            Once.Instance(SpawnMinionImmediate.Instance(0x1749, 10, 0, 4)),
							Cooldown.Instance(800, MultiAttack.Instance(10, 15 * (float)Math.PI / 180, 4, 0, projectileIndex: 4)),
                            Reproduce.Instance(0x1749, 10, 1500, 4),
								//new QueuedBehavior(HpLesserPercent.Instance(0.90f, new SetKey(-1, 2)))
                            new QueuedBehavior(Cooldown.Instance(20000, new SetKey(-1, 2)))
                            )
                        ),
                        IfEqual.Instance(-1, 2,
                            new RunBehaviors(
                                Once.Instance(new SimpleTaunt("BE SILENT!!!")),
  						        Timed.Instance(2500, Flashing.Instance(200, 0xf389E13)),
                                Once.Instance(TossEnemy.Instance(24 * (float)Math.PI / 180, 9, 0x1748)),
                                Once.Instance(TossEnemy.Instance(48 * (float)Math.PI / 180, 9, 0x1748)),
                                Once.Instance(TossEnemy.Instance(72 * (float)Math.PI / 180, 9, 0x1748)),
								Once.Instance(TossEnemy.Instance(96 * (float)Math.PI / 180, 9, 0x1748)),
                                Once.Instance(TossEnemy.Instance(120 * (float)Math.PI / 180, 9, 0x1748)),
                                Once.Instance(TossEnemy.Instance(144 * (float)Math.PI / 180, 9, 0x1748)),
                                Once.Instance(TossEnemy.Instance(168 * (float)Math.PI / 180, 9, 0x1748)),
                                Once.Instance(TossEnemy.Instance(192 * (float)Math.PI / 180, 9, 0x1748)),
                                Once.Instance(TossEnemy.Instance(216 * (float)Math.PI / 180, 9, 0x1748)),
                                Once.Instance(TossEnemy.Instance(240 * (float)Math.PI / 180, 9, 0x1748)),
                                Once.Instance(TossEnemy.Instance(264 * (float)Math.PI / 180, 9, 0x1748)),
                                Once.Instance(TossEnemy.Instance(288 * (float)Math.PI / 180, 9, 0x1748)),
                                Once.Instance(TossEnemy.Instance(312 * (float)Math.PI / 180, 9, 0x1748)),
                                Once.Instance(TossEnemy.Instance(336 * (float)Math.PI / 180, 9, 0x1748)),
                                Once.Instance(TossEnemy.Instance(360 * (float)Math.PI / 180, 9, 0x1748)),
								Cooldown.Instance(900, RingAttack.Instance(15, 20, 0, projectileIndex: 6)),
								//Cooldown.Instance(1200, Once.Instance(RingAttack.Instance(40, 2, 0, projectileIndex: 14))),
                                //Cooldown.Instance(700, Once.Instance(RingAttack.Instance(18, 2, 0, projectileIndex: 13))),
                                Cooldown.Instance(3000, ThrowAttack.Instance(4, 5, 240)),
								Cooldown.Instance(2500, ThrowAttack.Instance(4, 8, 150)),
								new QueuedBehavior(Cooldown.Instance(45000, new SetKey(-1, 3)))
                            ))
                        ),
                        IfEqual.Instance(-1, 3,
                            new RunBehaviors(
                                Once.Instance(new SimpleTaunt("My Artifacts will protect me!")),
                                Timed.Instance(2500, Flashing.Instance(200, 0xf389E13)),
                                Cooldown.Instance(1500, RingAttack.Instance(3, 10, 0, projectileIndex: 9)),
                                Cooldown.Instance(2000, RingAttack.Instance(10, 10, 0, projectileIndex: 8)),
                                Cooldown.Instance(500, RingAttack.Instance(10, 10, 0, projectileIndex: 7)),
                                //Inner Elements
                                Once.Instance(
                                new RunBehaviors(
                                TossEnemy.Instance(0 * (float)Math.PI / 180, 2, 0x174a),
                                TossEnemy.Instance(90 * (float)Math.PI / 180, 2, 0x174b),
                                TossEnemy.Instance(180 * (float)Math.PI / 180, 2, 0x174c),
                                TossEnemy.Instance(270 * (float)Math.PI / 180, 2, 0x174d),
                                //Outer Elements
                                TossEnemy.Instance(0 * (float)Math.PI / 180, 9, 0x174e),
                                TossEnemy.Instance(90 * (float)Math.PI / 180, 9, 0x174e),
                                TossEnemy.Instance(180 * (float)Math.PI / 180, 9, 0x174e),
                                TossEnemy.Instance(270 * (float)Math.PI / 180, 9, 0x174e)
                                )),
                                new QueuedBehavior(Cooldown.Instance(55000, new SetKey(-1, 4)))
                            )),

                            IfEqual.Instance(-1, 4,
                            new RunBehaviors(
                                Once.Instance(new SimpleTaunt("I am the master of this existence!")),
                                Timed.Instance(2500, Flashing.Instance(200, 0xf389E13)),
                                Timed.Instance(7000, Cooldown.Instance(2000, RingAttack.Instance(10, 20, 0, projectileIndex: 16))),
                                Cooldown.Instance(900, RingAttack.Instance(15, 20, 0, projectileIndex: 6)),
                                Once.Instance(TossEnemy.Instance(360 * (float)Math.PI / 180, 8, 0x1742)),
                                Once.Instance(TossEnemy.Instance(45 * (float)Math.PI / 180, 8, 0x1742)),
                                Once.Instance(TossEnemy.Instance(90 * (float)Math.PI / 180, 8, 0x1742)),
                                Once.Instance(TossEnemy.Instance(135 * (float)Math.PI / 180, 8, 0x1742)),
                                Once.Instance(TossEnemy.Instance(180 * (float)Math.PI / 180, 8, 0x1742)),
                                Once.Instance(TossEnemy.Instance(225 * (float)Math.PI / 180, 8, 0x1742)),
                                Once.Instance(TossEnemy.Instance(315 * (float)Math.PI / 180, 8, 0x1742)),
                                Once.Instance(TossEnemy.Instance(360 * (float)Math.PI / 180, 8, 0x1742))
                                //    Cooldown.Instance(250, new RunBehaviors(RingAttack.Instance(3, offset: 0 * (float)Math.PI / 180, projectileIndex: 0))),
                                //    Cooldown.Instance(250, new RunBehaviors(RingAttack.Instance(3, offset: 18 * (float)Math.PI / 180, projectileIndex: 0))),
                                //    Cooldown.Instance(250, new RunBehaviors(RingAttack.Instance(3, offset: 36 * (float)Math.PI / 180, projectileIndex: 0))),
                                //    Cooldown.Instance(250, new RunBehaviors(RingAttack.Instance(3, offset: 54 * (float)Math.PI / 180, projectileIndex: 0))),
                                //    Cooldown.Instance(250, new RunBehaviors(RingAttack.Instance(3, offset: 72 * (float)Math.PI / 180, projectileIndex: 0))),
                                //    Cooldown.Instance(250, new RunBehaviors(RingAttack.Instance(3, offset: 90 * (float)Math.PI / 180, projectileIndex: 0))),
                                //    Cooldown.Instance(250, new RunBehaviors(RingAttack.Instance(3, offset: 108 * (float)Math.PI / 180, projectileIndex: 0))),
                                //    Cooldown.Instance(250, new RunBehaviors(RingAttack.Instance(3, offset: 126 * (float)Math.PI / 180, projectileIndex: 0))),
                                //    Cooldown.Instance(250, new RunBehaviors(RingAttack.Instance(3, offset: 144 * (float)Math.PI / 180, projectileIndex: 0))),
                                //    Cooldown.Instance(250, new RunBehaviors(RingAttack.Instance(3, offset: 162 * (float)Math.PI / 180, projectileIndex: 0))),
                                //    Cooldown.Instance(250, new RunBehaviors(RingAttack.Instance(3, offset: 180 * (float)Math.PI / 180, projectileIndex: 0))),
                                //    Cooldown.Instance(250, new RunBehaviors(RingAttack.Instance(3, offset: 198 * (float)Math.PI / 180, projectileIndex: 0))),
                                //    Cooldown.Instance(250, new RunBehaviors(RingAttack.Instance(3, offset: 216 * (float)Math.PI / 180, projectileIndex: 0))),
                                //    Cooldown.Instance(250, new RunBehaviors(RingAttack.Instance(3, offset: 234 * (float)Math.PI / 180, projectileIndex: 0))),
                                //    Cooldown.Instance(250, new RunBehaviors(RingAttack.Instance(3, offset: 252 * (float)Math.PI / 180, projectileIndex: 0))),
                                //    Cooldown.Instance(250, new RunBehaviors(RingAttack.Instance(3, offset: 270 * (float)Math.PI / 180, projectileIndex: 0))),
                                //    Cooldown.Instance(250, new RunBehaviors(RingAttack.Instance(3, offset: 288 * (float)Math.PI / 180, projectileIndex: 0))),
                                //    Cooldown.Instance(250, new RunBehaviors(RingAttack.Instance(3, offset: 306 * (float)Math.PI / 180, projectileIndex: 0))),
                                //    Cooldown.Instance(250, new RunBehaviors(RingAttack.Instance(3, offset: 324 * (float)Math.PI / 180, projectileIndex: 0))),
                                //    Cooldown.Instance(250, new RunBehaviors(RingAttack.Instance(3, offset: 342 * (float)Math.PI / 180, projectileIndex: 0)))
                            )),

                    loot: new LootBehavior(LootDef.Empty,
                        Tuple.Create(100, new LootDef(0, 2, 0, 2,
                            Tuple.Create(0.001, (ILoot)new TierLoot(4, ItemType.Ability)),
                            Tuple.Create(0.005, (ILoot)new TierLoot(5, ItemType.Ability)),

                            Tuple.Create(0.05, (ILoot)new TierLoot(8, ItemType.Armor)),
                            Tuple.Create(0.05, (ILoot)new TierLoot(9, ItemType.Armor)),
                            Tuple.Create(0.025, (ILoot)new TierLoot(10, ItemType.Armor)),
                            Tuple.Create(0.005, (ILoot)new TierLoot(11, ItemType.Armor)),
                            Tuple.Create(0.001, (ILoot)new TierLoot(12, ItemType.Armor)),

                            Tuple.Create(0.05, (ILoot)new TierLoot(8, ItemType.Weapon)),
                            Tuple.Create(0.05, (ILoot)new TierLoot(9, ItemType.Weapon)),
                            Tuple.Create(0.005, (ILoot)new TierLoot(10, ItemType.Weapon)),
                            Tuple.Create(0.001, (ILoot)new TierLoot(11, ItemType.Weapon)),

                            Tuple.Create(0.2, (ILoot)new TierLoot(5, ItemType.Ring)),
                            Tuple.Create(0.5, (ILoot)new TierLoot(4, ItemType.Ring)),
                            Tuple.Create(0.7, (ILoot)new TierLoot(3, ItemType.Ring)),
                            Tuple.Create(0.5, (ILoot)new StatPotionLoot(StatPotion.Def)),
                            Tuple.Create(0.5, (ILoot)new StatPotionLoot(StatPotion.Att))
                    )))
                    
            ))
            .Init(0x1748, Behaves("Ring Element",
            new RunBehaviors(
                        Timed.Instance(20000, Cooldown.Instance(700, RingAttack.Instance(8, 100, 0, projectileIndex: 0))),
                        Once.Instance(SetConditionEffect.Instance(ConditionEffectIndex.Invulnerable)),
                        CooldownExact.Instance(19000,
                        new RunBehaviors( 
                            Despawn.Instance
                            ))
                        ))

        )
        .Init(0x1749, Behaves("Minion of Oryx",
            new RunBehaviors(
                SimpleWandering.Instance(3),
                Cooldown.Instance(700, MultiAttack.Instance(10, 15 * (float)Math.PI / 180, 3, 0, projectileIndex: 0)),
                Cooldown.Instance(700, MultiAttack.Instance(10, 15 * (float)Math.PI / 180, 3, 0, projectileIndex: 1))
                        ))
                        )
                        .Init(0x174a, Behaves("Guardian Element 1",
            new RunBehaviors(
                Circling.Instance(2, 10, 20, 0x1740),
                Once.Instance(SetConditionEffect.Instance(ConditionEffectIndex.Invulnerable)),
                Cooldown.Instance(850, MultiAttack.Instance(10, 15 * (float)Math.PI / 180, 3, 0, projectileIndex: 0)),
                Cooldown.Instance(30000, SetSize.Instance(200)),
                CooldownExact.Instance(40000,
                        new RunBehaviors(
                            Despawn.Instance
                            ))
                        ))
                        )
                        .Init(0x174b, Behaves("Guardian Element 2",
            new RunBehaviors(
                Circling.Instance(2, 10, 20, 0x1740),
                Once.Instance(SetConditionEffect.Instance(ConditionEffectIndex.Invulnerable)),
                Cooldown.Instance(850, MultiAttack.Instance(10, 15 * (float)Math.PI / 180, 3, 0, projectileIndex: 0)),
                Cooldown.Instance(30000, SetSize.Instance(200)),
                CooldownExact.Instance(40000,
                        new RunBehaviors(
                            Despawn.Instance
                            ))
                        ))
                        )
                        .Init(0x174c, Behaves("Guardian Element 3",
            new RunBehaviors(
                Circling.Instance(2, 10, 20, 0x1740),
                Once.Instance(SetConditionEffect.Instance(ConditionEffectIndex.Invulnerable)),
                Cooldown.Instance(850, MultiAttack.Instance(10, 15 * (float)Math.PI / 180, 3, 0, projectileIndex: 0)),
                Cooldown.Instance(30000, SetSize.Instance(200)),
                CooldownExact.Instance(40000,
                        new RunBehaviors(
                            Despawn.Instance
                            ))
                        ))
                        )
                        .Init(0x174d, Behaves("Guardian Element 4",
            new RunBehaviors(
                Circling.Instance(2, 10, 20, 0x1740),
                Once.Instance(SetConditionEffect.Instance(ConditionEffectIndex.Invulnerable)),
                Cooldown.Instance(850, MultiAttack.Instance(10, 15 * (float)Math.PI / 180, 3, 0, projectileIndex: 0)),
                Cooldown.Instance(30000, SetSize.Instance(200)),
                CooldownExact.Instance(40000,
                        new RunBehaviors(
                            Despawn.Instance
                            ))
                        ))
                        )
                        .Init(0x174e, Behaves("Outer Guardian Element",
            new RunBehaviors(
                Circling.Instance(12, 10, 15, 0x1740),
                Cooldown.Instance(850, MultiAttack.Instance(10, 15 * (float)Math.PI / 180, 3, 0, projectileIndex: 0)),
                Once.Instance(SetConditionEffect.Instance(ConditionEffectIndex.Invulnerable)),
                CooldownExact.Instance(40000,
                        new RunBehaviors(
                            Despawn.Instance
                            ))
                        ))
                        );
    }
}
