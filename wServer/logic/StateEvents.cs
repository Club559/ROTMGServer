using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wServer.realm;
using wServer.realm.entities;

namespace wServer.logic
{

    class State : Behavior
    {
        string name;
        Behavior[] behave;
        public State(string name, params Behavior[] behave)
        {
            this.name = name;
            this.behave = behave;
        }
        static readonly Dictionary<string, State> instances = new Dictionary<string, State>();

        Random rand = new Random();
        protected override bool TickCore(RealmTime time)
        {
            if (Host.Self.State == name)
            {
                foreach (var i in behave)
                {
                    i.Tick(Host, time);
                }
                return true;
            }
            return false;
        }
    }

    class NotState : Behavior
    {
        string name;
        Behavior[] behave;
        public NotState(string name, params Behavior[] behave)
        {
            this.name = name;
            this.behave = behave;
        }
        static readonly Dictionary<string, NotState> instances = new Dictionary<string, NotState>();

        Random rand = new Random();
        protected override bool TickCore(RealmTime time)
        {
            if (Host.Self.State != name)
            {
                foreach (var i in behave)
                {
                    i.Tick(Host, time);
                }
                return true;
            }
            return false;
        }
    }

    class SubState : Behavior
    {
        string name;
        Behavior[] behave;
        public SubState(string name, params Behavior[] behave)
        {
            this.name = name;
            this.behave = behave;
        }
        static readonly Dictionary<string, SubState> instances = new Dictionary<string, SubState>();

        Random rand = new Random();
        protected override bool TickCore(RealmTime time)
        {
            if (Host.Self.SubState == name)
            {
                foreach (var i in behave)
                {
                    i.Tick(Host, time);
                }
                return true;
            }
            return false;
        }
    }

    class SetState : Behavior
    {
        string state;
        private SetState(string state)
        {
            this.state = state;
        }
        static readonly Dictionary<string, SetState> instances = new Dictionary<string, SetState>();
        public static SetState Instance(string state)
        {
            SetState ret;
            if (!instances.TryGetValue(state, out ret))
                ret = instances[state] = new SetState(state);
            return ret;
        }

        Random rand = new Random();
        protected override bool TickCore(RealmTime time)
        {
            string[] states = state.Split('.');
            if (states[0] != null || states[0] != "")
            {
                Host.Self.State = states[0];
            }
            else
            {
                if (states.Length == 1)
                {
                    Host.Self.State = "idle";
                }
            }
            if (states.Length > 1)
            {
                if (states[1] != null || states[1] != "")
                {
                    Host.Self.SubState = states[1];
                }
                else
                {
                    Host.Self.SubState = "1";
                }
            }
            Host.StateTable.Clear();
            return true;
        }
    }

    class StateOnce : Behavior
    {
        Behavior[] x;

        private StateOnce(params Behavior[] x)
        {
            this.x = x;
        }
        static readonly Dictionary<Behavior[], StateOnce> instances = new Dictionary<Behavior[], StateOnce>();
        public static StateOnce Instance(params Behavior[] x)
        {
            StateOnce ret;
            if (!instances.TryGetValue(x, out ret))
                ret = instances[x] = new StateOnce(x);
            return ret;
        }

        protected override bool TickCore(RealmTime time)
        {
            if (!Host.StateTable.ContainsKey(Key))
            {
                foreach (var i in x)
                {
                    i.Tick(Host, time);
                }
                Host.StateTable.Add(Key, true);
                return true;
            }
            else
                return false;
        }
    }

    class ConditionalState : ConditionalBehavior
    {
        BehaviorCondition cond;
        string name;
        Behavior[] behave;
        public ConditionalState(BehaviorCondition cond, string name, params Behavior[] behave)
        {
            this.cond = cond;
            this.name = name;
            this.behave = behave;
        }

        public override BehaviorCondition Condition
        {
            get { return cond; }
        }

        Random rand = new Random();
        protected override void BehaveCore(BehaviorCondition cond, RealmTime? time, object state)
        {
            if (Host.Self.State == name)
            {
                foreach (var i in behave)
                {
                    i.Tick(Host, (RealmTime)time);
                }
            }
        }
    }

    class Uniqueify : Behavior
    {
        string unname;
        public Uniqueify(string unname)
        {
            this.unname = unname;
        }
        //static readonly Dictionary<string, SubState> instances = new Dictionary<string, SubState>();

        Random rand = new Random();
        protected override bool TickCore(RealmTime time)
        {
            return true;
        }
    }
}
