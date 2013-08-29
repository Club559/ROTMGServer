using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wServer.realm;
using wServer.logic.behaviors;
using wServer.logic.loot;
using wServer.logic.transitions;

namespace wServer.logic
{
    partial class BehaviorDb
    {
        _ Misc = () => Behav()
            .Init("Forgotten Archmage of Flame",
                    new State(
                        new Prioritize(
                            new Wander(0.1)
                        ),
                        new Shoot(10, count: 6, shootAngle: 60, fixedAngle: 0, coolDown: 1200, coolDownOffset: 0),
                        new Shoot(10, count: 6, shootAngle: 60, fixedAngle: 10, coolDown: 1200, coolDownOffset: 200),
                        new Shoot(10, count: 6, shootAngle: 60, fixedAngle: 20, coolDown: 1200, coolDownOffset: 400),
                        new Shoot(10, count: 6, shootAngle: 60, fixedAngle: 30, coolDown: 1200, coolDownOffset: 600),
                        new Shoot(10, count: 6, shootAngle: 60, fixedAngle: 40, coolDown: 1200, coolDownOffset: 800),
                        new Shoot(10, count: 6, shootAngle: 60, fixedAngle: 50, coolDown: 1200, coolDownOffset: 1000)
                    ),
                    new ItemLoot("Staff of the Abyss", 0.8)
                )
                ;
    }
}
