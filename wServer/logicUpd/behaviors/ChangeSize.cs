using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wServer.realm;

namespace wServer.logic.behaviors
{
    class ChangeSize : Behavior
    {
        //State storage: cooldown timer

        int rate;
        int target;

        public ChangeSize(int rate, int target)
        {
            this.rate = rate;
            this.target = target;
        }

        protected override void OnStateEntry(Entity host, RealmTime time, ref object state)
        {
            state = 0;
        }

        protected override void TickCore(Entity host, RealmTime time, ref object state)
        {
            int cool = (int)state;

            if (cool <= 0)
            {
                if (host.Size != target)
                {
                    host.Size += rate;
                    if ((rate > 0 && host.Size > target) ||
                        (rate < 0 && host.Size < target))
                        host.Size = target;
                    host.UpdateCount++;
                }
                cool = 150;
            }
            else
                cool -= time.thisTickTimes;

            state = cool;
        }
    }
}
