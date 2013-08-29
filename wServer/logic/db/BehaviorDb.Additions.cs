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
        static _ Additions = Behav()
            .Init(0x2000, Behaves("Reaper of Doom",
                new RunBehaviors(
                    new State("idle",
                        SetConditionEffect.Instance(ConditionEffectIndex.Invulnerable)
                    ),
                    new State("begin", new QueuedBehavior(
                        new SimpleTaunt("Who has awoken me?!"),
                        CooldownExact.Instance(3000, NullBehavior.Instance),
                        new SimpleTaunt("Well, time to have some fun!"),
                        Timed.Instance(2000, False.Instance(Flashing.Instance(250, 0xffffff00))),
                        SetState.Instance("grow")
                    )),
                    new State("grow", SimpleWandering.Instance(5), new QueuedBehavior(
                        CooldownExact.Instance(1000, SetSize.Instance(120)),
                        CooldownExact.Instance(1000, SetSize.Instance(140)),
                        CooldownExact.Instance(1000, SetSize.Instance(160)),
                        CooldownExact.Instance(1000, SetSize.Instance(180)),
                        CooldownExact.Instance(1000, SetSize.Instance(200)),
                        new SimpleTaunt("Fear my strongest form!"),
                        CooldownExact.Instance(1000, SetState.Instance("attack"))
                    )),
                    new State("attack",
                        StateOnce.Instance(UnsetConditionEffect.Instance(ConditionEffectIndex.Invulnerable)),
                        Chasing.Instance(5, 12, 1, null),
                        Cooldown.Instance(500, SimpleAttack.Instance(12, projectileIndex: 1)),
                        CooldownExact.Instance(1000, MultiAttack.Instance(8, 10 * (float)Math.PI / 180, 6)),
                        HpLesserPercent.Instance((float)0.1, new RunBehaviors(
                            SetConditionEffect.Instance(ConditionEffectIndex.Invulnerable),
                            SetState.Instance("dying")))
                    ),
                    new State("dying",
                        StateOnce.Instance(new SimpleTaunt("I CANNOT DIE!")),
                        new QueuedBehavior(
                            CooldownExact.Instance(1500, UnsetConditionEffect.Instance(ConditionEffectIndex.Invulnerable)),
                            CooldownExact.Instance(500, SetConditionEffect.Instance(ConditionEffectIndex.Invulnerable))
                        ),
                        Flashing.Instance(250, 0xffff0000),
                        CooldownExact.Instance(200, RingAttack.Instance(8))
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
                            Tuple.Create(0.05, (ILoot)new TierLoot(5, ItemType.Ring)),

                            Tuple.Create(0.005, (ILoot)new ItemLoot("Potion of Oryx")),

                            Tuple.Create(0.1, (ILoot)new StatPotionLoot(StatPotion.Att)),
                            Tuple.Create(0.1, (ILoot)new StatPotionLoot(StatPotion.Wis)),
                            Tuple.Create(0.1, (ILoot)new StatPotionLoot(StatPotion.Vit)),
                            Tuple.Create(0.1, (ILoot)new StatPotionLoot(StatPotion.Spd))
                    ))),
                condBehaviors: new ConditionalBehavior[] {
                    new OnHit(new State("idle", SetState.Instance("begin")))
                }
            ));
    }
}
