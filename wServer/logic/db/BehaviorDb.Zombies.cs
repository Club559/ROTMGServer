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
        static _ Zombies = Behav()
                .Init(0x1920, Behaves("Zombie",
                    IfNot.Instance(
                        Chasing.Instance(8.5f, 200, 0, null), SimpleWandering.Instance(4)),
                    Cooldown.Instance(2500, SimpleAttack.Instance(3))
                ))
                .Init(0x1921, Behaves("Crawler",
                    IfNot.Instance(
                        Chasing.Instance(5, 200, 0, null), SimpleWandering.Instance(4)),
                    Cooldown.Instance(2500, SimpleAttack.Instance(3))
                ))
                .Init(0x1922, Behaves("Hell Hound",
                    IfNot.Instance(
                        Chasing.Instance(11, 200, 0, null), SimpleWandering.Instance(4)),
                    Cooldown.Instance(2500, SimpleAttack.Instance(3))
                ))

                .Init(0x1938, Behaves("Zombie of Draconis",
            new RunBehaviors(
                SimpleWandering.Instance(2, 5),
                        Cooldown.Instance(2000, RingAttack.Instance(20, 25, projectileIndex: 0)),
                        Cooldown.Instance(3000, RingAttack.Instance(8, 25, projectileIndex: 1))
                        ),
                        loot: new LootBehavior(LootDef.Empty,
                       Tuple.Create(100, new LootDef(0, 5, 0, 10,
                            Tuple.Create(0.01, (ILoot)new ItemLoot("Spirit Dagger"))
                            ))
                        ))

        );
    }
}
