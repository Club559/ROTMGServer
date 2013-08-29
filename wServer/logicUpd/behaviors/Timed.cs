using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wServer.realm;

namespace wServer.logic.behaviors
{
    //replacement for simple timed transition in sequence
    class Timed : CycleBehavior
    {
        //State storage: time

        Behavior behavior;
        int period;
        public Timed(int period, Behavior behavior)
        {
            this.behavior = behavior;
            this.period = period;
        }

        protected override void OnStateEntry(Entity host, RealmTime time, ref object state)
        {
            behavior.OnStateEntry(host, time);
            state = period;
        }

        protected override void TickCore(Entity host, RealmTime time, ref object state)
        {
            int period = (int)state;

            behavior.Tick(host, time);
            Status = CycleStatus.InProgress;

            period -= time.thisTickTimes;
            if (period <= 0)
            {
                period = this.period;
                Status = CycleStatus.Completed;
                //......- -
                if (behavior is Prioritize)
                    host.StateStorage[behavior] = -1;
            }

            state = period;
        }
    }
}
