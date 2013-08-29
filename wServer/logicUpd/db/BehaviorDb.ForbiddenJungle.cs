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
        _ ForbiddenJungle = () => Behav()
            .Init("Great Coil Snake",
                    new State(
                        new Prioritize(
                            new StayCloseToSpawn(0.8, 5),
                            new Wander(0.4)
                        ),
                        new State("Waiting",
                            new PlayerWithinTransition(15, "Attacking")
                        ),
                        new State("Attacking",
                            new Shoot(10, projectileIndex: 0, coolDown: 700, coolDownOffset: 600),
                            new Shoot(10, count: 10, shootAngle: 36, projectileIndex: 1, coolDown: 2000),
                            new TossObject("Great Snake Egg", range: 4, angle: 0, coolDown: 5000, coolDownOffset: 0),
                            new TossObject("Great Snake Egg", range: 4, angle: 90, coolDown: 5000, coolDownOffset: 600),
                            new TossObject("Great Snake Egg", range: 4, angle: 180, coolDown: 5000, coolDownOffset: 1200),
                            new TossObject("Great Snake Egg", range: 4, angle: 270, coolDown: 5000, coolDownOffset: 1800),
                            new NoPlayerWithinTransition(30, "Waiting")
                        )
                    )
                )
            .Init("Great Snake Egg",
                    new State(
                        new TransformOnDeath("Great Temple Snake", min: 1, max: 2),
                        new State("Wait",
                            new TimedTransition(2500, "Explode"),
                            new PlayerWithinTransition(2, "Explode")
                        ),
                        new State("Explode",
                            new Suicide()
                        )
                    )
                )
            .Init("Great Temple Snake",
                    new State(
                        new Prioritize(
                            new Follow(0.6),
                            new Wander(0.4)
                        ),
                        new Shoot(10, count: 2, shootAngle: 7, projectileIndex: 0, coolDown: 1000, coolDownOffset: 0),
                        new Shoot(10, count: 6, shootAngle: 60, projectileIndex: 1, coolDown: 2000, coolDownOffset: 600)
                    )
                )
                ;
    }
}
