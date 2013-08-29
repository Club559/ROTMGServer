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
        static _ OceanTrench = Behav()
            .Init(0x1706, Behaves("Thessal the Mermaid Goddess",
                attack: new RunBehaviors(
                    new QueuedBehavior(
                        Cooldown.Instance(500, RingAttack.Instance(12, 20, projectileIndex: 2)),
                        Cooldown.Instance(500, RingAttack.Instance(6, 20, offset: 60, projectileIndex: 0))
                    ),
                    Cooldown.Instance(1500, RingAttack.Instance(20, 20, projectileIndex: 3)),
                    new State("idle",
                        CooldownExact.Instance(8000, SetState.Instance("dying"))
                    ),
                    new State("dying",
                        Timed.Instance(4000, False.Instance(Flashing.Instance(200, 0xff00ff00))),
                        CooldownExact.Instance(4000, SetState.Instance("idle"))
                    )
                ),
                loot: new LootBehavior(LootDef.Empty,
                        Tuple.Create(800, new LootDef(0, 5, 0, 3,
                            Tuple.Create(0.005, (ILoot)new ItemLoot("Coral Bow")),
                            Tuple.Create(0.01, (ILoot)new ItemLoot("Coral Silk Armor")),
                            Tuple.Create(0.05, (ILoot)new ItemLoot("Coral Ring")),
                            Tuple.Create(0.05, (ILoot)new ItemLoot("Coral Venom Trap")),
                            Tuple.Create(0.1, (ILoot)new ItemLoot("Coral Juice")),
                            Tuple.Create(1.0, (ILoot)new StatPotionLoot(StatPotion.Mana))
                        ))
                ),
                condBehaviors: new ConditionalBehavior[]
                {
                    new ConditionalState(BehaviorCondition.OnDeath, "dying",
                        new Transmute(0x1704)
                    )
                })
            )
            .Init(0x1704, Behaves("Thessal the Mermaid Goddess", //Dying Thessal
                attack: new RunBehaviors(
                    SetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),
                    Once.Instance(new SimpleTaunt("Is King Alexander alive?")),
                    new State("idle",
                        CooldownExact.Instance(12000, new RunBehaviors(
                            SetState.Instance("dungoofed")
                            )
                        )
                    ),
                    new State("dungoofed",
                        Once.Instance(new RunBehaviors(
                            new SimpleTaunt("You speak LIES!!"),
                            RingAttack.Instance(32)
                        )),
                        Timed.Instance(2000, False.Instance(Flashing.Instance(200, 0xff00ff00))),
                        CooldownExact.Instance(2000, new Transmute(0x1706))
                    ),
                    new State("dead",
                        CooldownExact.Instance(2000, Die.Instance)
                    )
                ),
                /*loot: new LootBehavior(LootDef.Empty,
                        Tuple.Create(800, new LootDef(0, 5, 0, 3,
                            Tuple.Create(0.005, (ILoot)new ItemLoot("Coral Bow")),
                            Tuple.Create(0.01, (ILoot)new ItemLoot("Coral Silk Armor")),
                            Tuple.Create(0.05, (ILoot)new ItemLoot("Coral Ring")),
                            Tuple.Create(0.05, (ILoot)new ItemLoot("Coral Venom Trap")),
                            Tuple.Create(0.1, (ILoot)new ItemLoot("Coral Juice")),
                            Tuple.Create(0.001, (ILoot)new ItemLoot("Dagger of Lost Souls")),
                            Tuple.Create(1.0, (ILoot)new StatPotionLoot(StatPotion.Mana))
                        ))
                ),*/
                condBehaviors: new ConditionalBehavior[] {
                    new ChatEvent(
                        new State("idle",
                            new SimpleTaunt("Thank you, kind sailor."),
                            TossEnemy.Instance(60, 2, 0x1705),
                            TossEnemy.Instance(120, 2, 0x1705),
                            TossEnemy.Instance(180, 2, 0x1705),
                            TossEnemy.Instance(240, 2, 0x1705),
                            TossEnemy.Instance(300, 2, 0x1705),
                            TossEnemy.Instance(360, 2, 0x1705),
                            SetState.Instance("dead")
                        )
                    ).FalseEvent(
                        new State("idle",
                            SetState.Instance("dungoofed")
                        )
                    ).SetChats(
                        "He lives and reigns and conquers the world.",
                        "He lives and reigns and conquers the world"
                    )
                })
            )
            .Init(0x1705, Behaves("Coral Gift",
                loot: new LootBehavior(LootDef.Empty,
                        Tuple.Create(800, new LootDef(0, 5, 0, 3,
                            Tuple.Create(0.005, (ILoot)new ItemLoot("Coral Bow")),
                            Tuple.Create(0.01, (ILoot)new ItemLoot("Coral Silk Armor")),
                            Tuple.Create(0.05, (ILoot)new ItemLoot("Coral Ring")),
                            Tuple.Create(0.05, (ILoot)new ItemLoot("Coral Venom Trap")),
                            Tuple.Create(0.1, (ILoot)new ItemLoot("Coral Juice")),
                            Tuple.Create(1.0, (ILoot)new StatPotionLoot(StatPotion.Mana))
                        ))
                )
            ))
            .Init(0x1708, Behaves("Fishman",
                IfNot.Instance(
                  Chasing.Instance(12, 9, 6, 0x1700),
                    IfNot.Instance(
                      Circling.Instance(3, 10, 6, null),
                      SimpleWandering.Instance(4)
                      )
                  ),
                  Cooldown.Instance(800, MultiAttack.Instance(10, 1 * (float)Math.PI / 30, 3, 0, projectileIndex: 0)),
                  Cooldown.Instance(1000, SimpleAttack.Instance(10, projectileIndex: 1))
            ))
            .Init(0x1700, Behaves("Fishman Warrior",
                IfNot.Instance(
                  Chasing.Instance(12, 9, 3, null),
                  IfNot.Instance(
                    Circling.Instance(3, 10, 6, null),
                    SimpleWandering.Instance(4)
                    )
                ),
                new RunBehaviors(
                  Cooldown.Instance(800, MultiAttack.Instance(100, 1 * (float)Math.PI / 30, 3, 0, projectileIndex: 0)),
                  Cooldown.Instance(1000, SimpleAttack.Instance(3, projectileIndex: 1)),
                  Cooldown.Instance(2000, RingAttack.Instance(5, 10, 0, projectileIndex: 2))
                  )
             ))
            .Init(0x170a, Behaves("Sea Mare",
                IfNot.Instance(
                  Charge.Instance(10, 10, null),
                  SimpleWandering.Instance(4)
                  ),
                  Cooldown.Instance(500, MultiAttack.Instance(25, 10 * (float)Math.PI / 180, 5, 0, projectileIndex: 0)),
                  Cooldown.Instance(1500, SimpleAttack.Instance(3, projectileIndex: 1))
            ))
            .Init(0x170c, Behaves("Giant Squid",
                new RunBehaviors(
                    If.Instance(
                        IsEntityPresent.Instance(5, null),
                        Cooldown.Instance(1000, TossEnemy.Instance(new Random().Next() * (float)Math.PI / 360, new Random().Next(10) * (float)Math.PI / 180, 0x170b))
                    ),
                    If.Instance(
                        IsEntityNotPresent.Instance(7, null),
                        SimpleWandering.Instance(1)
                    ),
                    If.Instance(
                        IsEntityPresent.Instance(7, null),
                        new RunBehaviors(
                            IfNot.Instance(
                                Chasing.Instance(10, 9, 0, null),
                                SimpleWandering.Instance(1)
                                )
                            )
                        )
                    ),
                    new RunBehaviors(
                        Cooldown.Instance(100, SimpleAttack.Instance(6, projectileIndex: 0)),
                        Cooldown.Instance(500, MultiAttack.Instance(6, 10 * (float)Math.PI / 180, 5, 0, projectileIndex: 1))
                    )
            ))
            .Init(0x170b, Behaves("Ink Bubble",
                new RunBehaviors(
                    Cooldown.Instance(100, RingAttack.Instance(1, 1, 0, projectileIndex: 0))
                ),
                If.Instance(
                    IsEntityNotPresent.Instance(7, null),
                    CooldownExact.Instance(10000, Die.Instance)
                )
            ))
            .Init(0x1707, Behaves("Deep Sea Beast",
                SetSize.Instance(100),
                  new QueuedBehavior(
                    Cooldown.Instance(50, SimpleAttack.Instance(3, projectileIndex: 0)),
                    Cooldown.Instance(100, SimpleAttack.Instance(3, projectileIndex: 1)),
                    Cooldown.Instance(150, SimpleAttack.Instance(3, projectileIndex: 2)),
                    Cooldown.Instance(200, SimpleAttack.Instance(3, projectileIndex: 3)),
                    CooldownExact.Instance(300)
                    )
            ))
            .Init(0x1709, Behaves("Sea Horse",
                IfNot.Instance(
                  Chasing.Instance(12, 9, 1, 0x170a),
                  SimpleWandering.Instance(4)
                  ),
                  Cooldown.Instance(660, MultiAttack.Instance(25, 10 * (float)Math.PI / 180, 5, 0, projectileIndex: 0))
            ))
            .Init(0x170e, Behaves("Grey Sea Slurp",
                IfNot.Instance(
                  Chasing.Instance(12, 9, 2, 0x170d),
                  SimpleWandering.Instance(4)
                  ),
                  Cooldown.Instance(500, SimpleAttack.Instance(8, projectileIndex: 0)),
                  Cooldown.Instance(500, RingAttack.Instance(8, 4, 0, projectileIndex: 1))
            ))
            .Init(0x170d, Behaves("Sea Slurp Home",
                new QueuedBehavior(
                    Cooldown.Instance(500, RingAttack.Instance(8, 4, 0, projectileIndex: 0)),
                    Cooldown.Instance(500, RingAttack.Instance(8, 2, 0, projectileIndex: 1))
                    )
            ));
    }
}
