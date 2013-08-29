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
        static _ Limon = Behav()
            .Init(0x0d06, Behaves("Limon the Sprite God",
                new RunBehaviors(
                        IfExist.Instance(-1, NullBehavior.Instance,
                            new RunBehaviors(
                                new QueuedBehavior(
                                    CooldownExact.Instance(400)
                                ),
                                new QueuedBehavior(new SetKey(-1, 1))

                            )
                        ),
                        IfEqual.Instance(-1, 1,
                            new RunBehaviors(
                            Timed.Instance(2500, Flashing.Instance(200, 0xf389E13)),
                            Cooldown.Instance(200, Charge.Instance(35, 7, null)),
                            Cooldown.Instance(1000, RingAttack.Instance(15, 6)),
                            new QueuedBehavior(Cooldown.Instance(30000, new SetKey(-1, 2)))
                            )
                        ),
                        IfEqual.Instance(-1, 2,
                            new RunBehaviors(
                                Cooldown.Instance(750, RingAttack.Instance(3, 6)),
                                
                                Once.Instance(
                                    new RunBehaviors(
                                            //Inner Elements
                                            TossEnemy.Instance(45 * (float)Math.PI / 180, 6, 0x0d07),
                                            TossEnemy.Instance(135 * (float)Math.PI / 180, 6, 0x0d08),
                                            TossEnemy.Instance(225 * (float)Math.PI / 180, 6, 0x0d09),
                                            TossEnemy.Instance(315 * (float)Math.PI / 180, 6, 0x0d0a),
                                            //Outer Elements
                                            TossEnemy.Instance(45 * (float)Math.PI / 180, 10, 0x0d07),
                                            TossEnemy.Instance(135 * (float)Math.PI / 180, 10, 0x0d08),
                                            TossEnemy.Instance(225 * (float)Math.PI / 180, 10, 0x0d09),
                                            TossEnemy.Instance(315 * (float)Math.PI / 180, 10, 0x0d0a)
                                ))
                            ))),
                            new QueuedBehavior(Cooldown.Instance(24000, new SetKey(-1, 1))),

                    loot: new LootBehavior(LootDef.Empty,
                        Tuple.Create(800, new LootDef(0, 5, 0, 3,
                            Tuple.Create(0.006, (ILoot)new ItemLoot("Wine Cellar Incantation")),
                            Tuple.Create(0.01, (ILoot)new ItemLoot("Cloak of the Planewalker")),
                            Tuple.Create(0.01, (ILoot)new ItemLoot("Staff of Extreme Prejudice")),
                            Tuple.Create(PotProbability, (ILoot)new StatPotionLoot(StatPotion.Dex)),
                            Tuple.Create(0.1, (ILoot)new ItemLoot("Potion of Defense"))
                        ))
                    )

            ))
            .Init(0x0d07, Behaves("Limon Element 1",
            new RunBehaviors(
                Cooldown.Instance(5000, SetConditionEffect.Instance(ConditionEffectIndex.Invulnerable)),
                Cooldown.Instance(10000, UnsetConditionEffect.Instance(ConditionEffectIndex.Invulnerable)),
                Cooldown.Instance(500, AngleAttack.Instance(270 * (float)Math.PI / 180, projectileIndex: 0)),
                Cooldown.Instance(500, AngleAttack.Instance(180 * (float)Math.PI / 180, projectileIndex: 0)),
                        Once.Instance(SetConditionEffect.Instance(ConditionEffectIndex.Invulnerable)),
                        CooldownExact.Instance(20000, Cooldown.Instance(500, AngleAttack.Instance(225 * (float)Math.PI / 180, projectileIndex: 0))),
                        CooldownExact.Instance(20000,
                        new RunBehaviors( 
                            Despawn.Instance
                            ))
           )))
           .Init(0x0d08, Behaves("Limon Element 2",
            new RunBehaviors(
                Cooldown.Instance(500, AngleAttack.Instance(360 * (float)Math.PI / 180, projectileIndex: 0)),
                Cooldown.Instance(500, AngleAttack.Instance(270 * (float)Math.PI / 180, projectileIndex: 0)),
                        Once.Instance(SetConditionEffect.Instance(ConditionEffectIndex.Invulnerable)),
                        CooldownExact.Instance(20000,
                        new RunBehaviors(
                            Cooldown.Instance(500, AngleAttack.Instance(315 * (float)Math.PI / 180, projectileIndex: 0))
               )),
                        CooldownExact.Instance(20000,
                        new RunBehaviors(
                            Despawn.Instance
                            ))
           )))
           .Init(0x0d09, Behaves("Limon Element 3",
            new RunBehaviors(
                Cooldown.Instance(500, AngleAttack.Instance(90 * (float)Math.PI / 180, projectileIndex: 0)),
                Cooldown.Instance(500, AngleAttack.Instance(360 * (float)Math.PI / 180, projectileIndex: 0)),
                        Once.Instance(SetConditionEffect.Instance(ConditionEffectIndex.Invulnerable)),
                        CooldownExact.Instance(20000,
                        new RunBehaviors(
                            Cooldown.Instance(500, AngleAttack.Instance(45 * (float)Math.PI / 180, projectileIndex: 0))
               )),
                        CooldownExact.Instance(20000,
                        new RunBehaviors(
                            Despawn.Instance
                            ))
           )))
           .Init(0x0d0a, Behaves("Limon Element 4",
            new RunBehaviors(
                Cooldown.Instance(500, AngleAttack.Instance(90 * (float)Math.PI / 180, projectileIndex: 0)),
                Cooldown.Instance(500, AngleAttack.Instance(180 * (float)Math.PI / 180, projectileIndex: 0)),
                        Once.Instance(SetConditionEffect.Instance(ConditionEffectIndex.Invulnerable)),
                        CooldownExact.Instance(20000,
                        new RunBehaviors(
                            Cooldown.Instance(500, AngleAttack.Instance(135 * (float)Math.PI / 180, projectileIndex: 0))
               )),
                        CooldownExact.Instance(20000,
                        new RunBehaviors(
                            Despawn.Instance
                            ))
           )));

    }
}
