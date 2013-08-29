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
        static _ Abyss = Behav()
            .Init(0x090a, Behaves("Archemon Malphas",
                new RunBehaviors(
                    new State("idle",
                        SetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),
                        If.Instance(
                            IsEntityPresent.Instance(5, null),
                            new RunBehaviors(
                                SetState.Instance("attack")
                            )
                        )
                    ),
                    new State("attack",
                        Chasing.Instance(2, 8, 4, null),
                        Cooldown.Instance(1000, SimpleAttack.Instance(8)),
                        CooldownExact.Instance(8000, SetSize.Instance(90)),
                        CooldownExact.Instance(8100, SetSize.Instance(80)),
                        CooldownExact.Instance(8200, SetSize.Instance(70)),
                        CooldownExact.Instance(8300, SetSize.Instance(60)),
                        CooldownExact.Instance(8400, SetSize.Instance(50)),
                        CooldownExact.Instance(8500, SetState.Instance("small"))
                    ),
                    new State("small",
                        SimpleWandering.Instance(2),
                        Cooldown.Instance(1000, RingAttack.Instance(4, projectileIndex: 1)),
                        Cooldown.Instance(1000, SimpleAttack.Instance(8)),
                        CooldownExact.Instance(8000, SetSize.Instance(70)),
                        CooldownExact.Instance(8100, SetSize.Instance(90)),
                        CooldownExact.Instance(8200, SetSize.Instance(110)),
                        CooldownExact.Instance(8300, SetSize.Instance(130)),
                        CooldownExact.Instance(8400, SetSize.Instance(150)),
                        CooldownExact.Instance(8500, SetState.Instance("large"))
                    ),
                    new State("large",
                        SimpleWandering.Instance(2),
                        Cooldown.Instance(1000, RingAttack.Instance(4, projectileIndex: 3)),
                        Cooldown.Instance(1000, SimpleAttack.Instance(8, projectileIndex: 2)),
                        CooldownExact.Instance(8000, SetSize.Instance(140)),
                        CooldownExact.Instance(8100, SetSize.Instance(130)),
                        CooldownExact.Instance(8200, SetSize.Instance(120)),
                        CooldownExact.Instance(8300, SetSize.Instance(110)),
                        CooldownExact.Instance(8400, SetSize.Instance(100)),
                        CooldownExact.Instance(8500, SetState.Instance("attack"))
                    )
                ),
                new QueuedBehavior(
                    CooldownExact.Instance(4000, UnsetConditionEffect.Instance(ConditionEffectIndex.Invulnerable)),
                    CooldownExact.Instance(3000, SetConditionEffect.Instance(ConditionEffectIndex.Invulnerable))
                ),
                loot: new LootBehavior(LootDef.Empty,
                        Tuple.Create(800, new LootDef(0, 5, 1, 2,
                            Tuple.Create(0.005, (ILoot)new ItemLoot("Demon Blade")),
                            Tuple.Create(0.003, (ILoot)new ItemLoot("Prism of Inception")),
                            Tuple.Create(PotProbability, (ILoot)new StatPotionLoot(StatPotion.Vit)),
                            Tuple.Create(0.25, (ILoot)new ItemLoot("Potion of Defense"))
                        ))
                )
            ))
            .Init(0x0908, Behaves("Malphas Protector",
                new RunBehaviors(
                    Circling.Instance(15, 50, 50, 0x090a),
                    Cooldown.Instance(2000, MultiAttack.Instance(100, 1 * (float)Math.PI / 30, 3, 0, projectileIndex: 0))
                    )
            ))
            .Init(0x0909, Behaves("Malphas Missile",
                new RunBehaviors(
                    Chasing.Instance(10, 11, 0, null),
                    CooldownExact.Instance(5000,
                        new RunBehaviors(
                            Cooldown.Instance(0, Flashing.Instance(500, 0x01FAEBD7)),
                            Cooldown.Instance(550, Flashing.Instance(500, 0x01FAEBD7)),
                            RingAttack.Instance(6, 360, 0, projectileIndex: 0),
                            Despawn.Instance
                            )))
            ))
            .Init(0x671, Behaves("Brute of the Abyss",
                IfNot.Instance(
                    Chasing.Instance(16, 11, 1, null),
                    SimpleWandering.Instance(4)
                    ),
                    Cooldown.Instance(500, MultiAttack.Instance(100, 1 * (float)Math.PI / 30, 3, 0, projectileIndex: 0)
                    ),
                    loot: new LootBehavior(
                        new LootDef(0, 5, 0, 10,
                            Tuple.Create(0.01, (ILoot)HpPotionLoot.Instance)
                            ),
                            Tuple.Create(100, new LootDef(0, 1, 0, 2,
                            Tuple.Create(0.01, (ILoot)new ItemLoot("Obsidian Dagger")),
                            Tuple.Create(0.01, (ILoot)new ItemLoot("Steel Helm"))
                            )))
            ))
            .Init(0x66d, Behaves("Imp of the Abyss",
                new RunBehaviors(
                    SimpleWandering.Instance(12f),
                    Cooldown.Instance(1000, MultiAttack.Instance(100, 1 * (float)Math.PI / 20, 5, 0, projectileIndex: 0))
                ),
                loot: new LootBehavior(
                    new LootDef(0, 5, 0, 10,
                        Tuple.Create(0.01, (ILoot)PotionLoot.Instance)
                        ),
                        Tuple.Create(100, new LootDef(0, 1, 0, 2,
                        Tuple.Create(0.01, (ILoot)new ItemLoot("Cloak of the Red Agent")),
                        Tuple.Create(0.01, (ILoot)new ItemLoot("Felwasp Toxin"))
                        )))
            ))
            .Init(0x672, Behaves("Brute Warrior of the Abyss",
                IfNot.Instance(
                    Chasing.Instance(16, 11, 1, null),
                    SimpleWandering.Instance(4)
                    ),
                    Cooldown.Instance(500, MultiAttack.Instance(100, 1 * (float)Math.PI / 30, 3, 0, projectileIndex: 0)
                    ),
                    loot: new LootBehavior(
                       new LootDef(0, 5, 0, 10,
                            Tuple.Create(0.01, (ILoot)new ItemLoot("Spirit Salve Tome"))
                            ),
                            Tuple.Create(100, new LootDef(0, 1, 0, 2,
                            Tuple.Create(0.01, (ILoot)new ItemLoot("Glass Sword")),
                            Tuple.Create(0.01, (ILoot)new ItemLoot("Ring of Greater Dexterity")),
                            Tuple.Create(0.01, (ILoot)new ItemLoot("Magesteel Quiver"))
                            )))
            ))
            .Init(0x670, Behaves("Demon Mage of the Abyss",
                IfNot.Instance(
                    Chasing.Instance(16, 11, 5, null),
                    SimpleWandering.Instance(4)
                    ),
                    Cooldown.Instance(1000, MultiAttack.Instance(100, 1 * (float)Math.PI / 20, 3, 0, projectileIndex: 0)
                    ),
                    loot: new LootBehavior(
                       new LootDef(0, 5, 0, 10,
                            Tuple.Create(0.01, (ILoot)new ItemLoot("Fire Nova Spell"))
                            ),
                            Tuple.Create(100, new LootDef(0, 1, 0, 2,
                            Tuple.Create(0.01, (ILoot)new ItemLoot("Wand of Dark Magic")),
                            Tuple.Create(0.01, (ILoot)new ItemLoot("Avenger Staff")),
                            Tuple.Create(0.01, (ILoot)new ItemLoot("Robe of the Invoker")),
                            Tuple.Create(0.01, (ILoot)new ItemLoot("Essence Tap Skull")),
                            Tuple.Create(0.01, (ILoot)new ItemLoot("Demonhunter Trap"))
                            )))
            ))
            .Init(0x66f, Behaves("Demon Warrior of the Abyss",
                IfNot.Instance(
                    Chasing.Instance(16, 11, 5, null),
                    SimpleWandering.Instance(4)
                    ),
                    Cooldown.Instance(1000, MultiAttack.Instance(100, 1 * (float)Math.PI / 20, 3, 0, projectileIndex: 0)
                    ),
                    loot: new LootBehavior(
                        new LootDef(0, 5, 0, 10,
                            Tuple.Create(0.01, (ILoot)new ItemLoot("Fire Sword")),
                            Tuple.Create(0.01, (ILoot)new ItemLoot("Steel Shield"))
                            ))
            ))
            .Init(0x66e, Behaves("Demon of the Abyss",
                IfNot.Instance(
                    Chasing.Instance(16, 11, 5, null),
                    SimpleWandering.Instance(4)
                    ),
                    Cooldown.Instance(1000, PredictiveMultiAttack.Instance(100, 1 * (float)Math.PI / 25, 3, 0, projectileIndex: 0)
                    ),
                    loot: new LootBehavior(
                        new LootDef(0, 5, 0, 10,
                            Tuple.Create(0.01, (ILoot)new ItemLoot("Fire Bow"))
                            ),
                            Tuple.Create(100, new LootDef(0, 1, 0, 2,
                            Tuple.Create(0.01, (ILoot)new ItemLoot("Mithril Armor"))
                            )))
            ))
            .Init(0x0e1d, Behaves("Abyss Idol",
                new RunBehaviors(
                    Cooldown.Instance(250, PredictiveMultiAttack.Instance(25, 10 * (float)Math.PI / 180, 2, 1, projectileIndex: 1))
                ),
                    loot: new LootBehavior(LootDef.Empty,
                       Tuple.Create(100, new LootDef(0, 5, 0, 10,
                            Tuple.Create(0.99, (ILoot)new ItemLoot("Potion of Defense")),
                            Tuple.Create(0.99, (ILoot)new ItemLoot("Potion of Vitality")),
                            Tuple.Create(0.15, (ILoot)new ItemLoot("Wine Cellar Incantation")),
                            Tuple.Create(0.5, (ILoot)new ItemLoot("Demon Blade"))
            )))));
    }
}
