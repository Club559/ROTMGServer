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
        static _ Davy = Behav()
            .Init(0x0e32, Behaves("Davy Jones",
            new RunBehaviors(
                SimpleWandering.Instance(2, 5),
                        Cooldown.Instance(2000, MultiAttack.Instance(25, 10 * (float)Math.PI / 180, 5, 0, projectileIndex: 0)),
                        Cooldown.Instance(4000, MultiAttack.Instance(25, 10 * (float)Math.PI / 180, 1, 0, projectileIndex: 1))
                        ),
                        loot: new LootBehavior(LootDef.Empty,
                       Tuple.Create(100, new LootDef(0, 5, 0, 10,
                            Tuple.Create(0.01, (ILoot)new ItemLoot("Spirit Dagger")),
                            Tuple.Create(0.01, (ILoot)new ItemLoot("Spectral Cloth Armor")),
                            Tuple.Create(0.01, (ILoot)new ItemLoot("Ghostly Prism")),
                            Tuple.Create(0.99, (ILoot)new ItemLoot("Potion of Wisdom")),
                            Tuple.Create(0.01, (ILoot)new ItemLoot("Captain's Ring"))
                            ))
                        ))

        );
    }
}
