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
        static _ Unknown = Behav()
            .Init(0x0f02, Behaves("Unknown Giant Golem",
                new RunBehaviors(
                    SimpleWandering.Instance(2, 5)
                )
            ));
    }
}
