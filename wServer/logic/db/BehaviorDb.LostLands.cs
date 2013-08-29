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
        static _ LostLands = Behav()
            .Init(0x0d50, Behaves("Lord of the Lost Lands",
                    SmoothWandering.Instance(0.4f, 1),
                    new RunBehaviors(
                        new State("idle",
                        StateOnce.Instance(UnsetConditionEffect.Instance(ConditionEffectIndex.Invulnerable)),
                        Cooldown.Instance(10000, MultiAttack.Instance(25, 10 * (float)Math.PI / 180, 6, 0, projectileIndex: 0)),
                        //Cooldown.Instance(10000, MultiAttack.Instance(25, 10 * (float)Math.PI / 180, 6, 5, projectileIndex: 0)),
                        Once.Instance(new SimpleTaunt("Do you really want to fight me, {PLAYER}?")),
                        IfNot.Instance(
                            IsEntityPresent.Instance(20000, 0x0d51),
                            SetState.Instance("battle")
                        ))
                    ),
                    new RunBehaviors(
                        new State("battle",
                        new QueuedBehavior(
                            Once.Instance(SpawnMinionImmediate.Instance(0x0d53, 7, 1, 3)),
                            Once.Instance(SpawnMinionImmediate.Instance(0x0d52, 7, 1, 3)),
                            Once.Instance(new SimpleTaunt("{PLAYER}, you seem to have a death wish!")),
                            Once.Instance(new SimpleTaunt("My minions will make meager work of you, {PLAYER}!"))
                        ),
                        Cooldown.Instance(1000, MultiAttack.Instance(10, 10 * (float)Math.PI / 180, 6)),
                        //Cooldown.Instance(1000, MultiAttack.Instance(10, 10 * (float)Math.PI / 180, 6, 5)),

                        HpLesserPercent.Instance(0.2500f,
                            new RunBehaviors(
                                Chasing.Instance(3, 25, 2, null),
                                Once.Instance(SetConditionEffect.Instance(ConditionEffectIndex.Invulnerable)),
                                Once.Instance(SpawnMinionImmediate.Instance(0x0d51, 6, 8, 8)),
                                Once.Instance(RingAttack.Instance(8, 0, 2, projectileIndex: 1)),
                                If.Instance(
                                    IsEntityPresent.Instance(20000, 0x0d51),
                                    SetState.Instance("heal")                                  
                                )
                                
                            )                            
                        )),
                        new State("heal",
                        Cooldown.Instance(1000, MultiAttack.Instance(10, 10 * (float)Math.PI / 180, 6)),
                        //Cooldown.Instance(1000, MultiAttack.Instance(10, 10 * (float)Math.PI / 180, 6, 99)),
                            new RunBehaviors(
                                Once.Instance(SetConditionEffect.Instance(ConditionEffectIndex.Invulnerable)),
                                Heal.Instance(100, 100000, 0x0d50),
                              
                                IfNot.Instance(
                                    IsEntityPresent.Instance(20000, 0x0d51),
                                    SetState.Instance("idle")                               
                                )
                                
                            )                            
                        )

                    ),
                    loot: new LootBehavior(LootDef.Empty,
                            Tuple.Create(100, new LootDef(0, 5, 0, 8,
                            Tuple.Create(0.001, (ILoot)new TierLoot(4, ItemType.Ability)),
                            Tuple.Create(0.005, (ILoot)new TierLoot(5, ItemType.Ability)),

                            Tuple.Create(0.005, (ILoot)new ItemLoot("Shield of Ogmur")),
                            Tuple.Create(0.001, (ILoot)new ItemLoot("Orb of the Chaos Walker")),

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

                            Tuple.Create(0.5, (ILoot)new StatPotionLoot(StatPotion.Att)),
                            Tuple.Create(0.5, (ILoot)new StatPotionLoot(StatPotion.Wis)),
                            Tuple.Create(0.5, (ILoot)new StatPotionLoot(StatPotion.Vit)),
                            Tuple.Create(0.5, (ILoot)new StatPotionLoot(StatPotion.Spd))
                           ))),
                           condBehaviors: new ConditionalBehavior[] {
                    new OnDeath(new SimpleTaunt("NOOOOO")),
                    new OnDeath(Once.Instance(RingAttack.Instance(8, 0, 2, projectileIndex: 1)))
                   
                }

                ))
               .Init(0x0d52, Behaves("Knight of the Lost Lands",
                    Chasing.Instance(3,25,2,null),
                    new RunBehaviors(
                        new State("idle",
                        Cooldown.Instance(1000, AngleAttack.Instance(45f,projectileIndex:0))
                    ),
                    new RunBehaviors(
                        Cooldown.Instance(1000, MultiAttack.Instance(10, 6 * (float)Math.PI / 180, 3)),

                        HpLesserPercent.Instance(0.2f,
                            new RunBehaviors(
                                Chasing.Instance(3, 25, 2, null),
                                
                                Cooldown.Instance(8000, Once.Instance(RingAttack.Instance(5, 0, 2, projectileIndex: 0)))
                      ))))
                    )
                ).Init(0x0d53, Behaves("Guardian of the Lost Lands",
                    Circling.Instance(8, 8, 4, 0x0d50),
                    new RunBehaviors(
                        new State("idle",
                        Cooldown.Instance(1000, AngleAttack.Instance(45,projectileIndex:0)),
                        Cooldown.Instance(1500, AngleAttack.Instance(20,projectileIndex:1))
                    ),
                    new RunBehaviors(
                        Cooldown.Instance(1000, MultiAttack.Instance(10, 6 * (float)Math.PI / 180, 3)),
                        HpLesserPercent.Instance(0.2f,
                            new RunBehaviors(
                                Chasing.Instance(3, 25, 2, null),
                                Cooldown.Instance(8000, Once.Instance(RingAttack.Instance(5, 0, 2, projectileIndex: 0)))
                      ))))
                    )
                ).Init(0x0d51, Behaves("Protection Crystal",
                    Circling.Instance(5, 5, 2, 0x0d50),                        
                    new RunBehaviors(
                        new QueuedBehavior(
                            Cooldown.Instance(5000, RingAttack.Instance(8,4, 2,projectileIndex:0))
                        ),
                        Cooldown.Instance(1000, MultiAttack.Instance(10, 6 * (float)Math.PI / 180, 3)),
                        HpLesserPercent.Instance(0.2f,
                            new RunBehaviors(
                               Cooldown.Instance(8000, Once.Instance(RingAttack.Instance(5, 0, 2, projectileIndex: 0)))
                      )))
                    )
                );
     }
}
