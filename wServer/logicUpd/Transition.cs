using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wServer.realm;

namespace wServer.logic
{
    public abstract class Transition : IStateChildren
    {
        string targetStateName;
        public Transition(string targetState)
        {
            targetStateName = targetState;
        }

        public State TargetState { get; private set; }
        public bool Tick(Entity host, RealmTime time)
        {
            object state;
            if (!host.StateStorage.TryGetValue(this, out state))
                state = null;

            var ret = TickCore(host, time, ref state);
            if (ret)
                host.SwitchTo(TargetState);

            if (state == null)
                host.StateStorage.Remove(this);
            else
                host.StateStorage[this] = state;
            return ret;
        }
        protected abstract bool TickCore(Entity host, RealmTime time, ref object state);

        internal void Resolve(IDictionary<string, State> states)
        {
            TargetState = states[targetStateName];
        }

        [ThreadStatic]
        private static Random rand;
        protected static Random Random
        {
            get
            {
                if (rand == null) rand = new Random();
                return rand;
            }
        }
    }
}
