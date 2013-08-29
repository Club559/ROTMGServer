using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wServer.realm;
using wServer.realm.entities;
using wServer.networking.svrPackets;

namespace wServer.logic.behaviors
{
    class Order : Behavior
    {
        //State storage: none

        double range;
        ushort children;
        string targetStateName;
        State targetState;

        public Order(double range, string children, string targetState)
        {
            this.range = range;
            this.children = BehaviorDb.InitGameData.IdToObjectType[children];
            this.targetStateName = targetState;
        }

        static State FindState(State state, string name)
        {
            if (state.Name == name) return state;
            State ret;
            foreach (var i in state.States)
            {
                if ((ret = FindState(i, name)) != null)
                    return ret;
            }
            return null;
        }


        protected override void TickCore(Entity host, RealmTime time, ref object state)
        {
            if (targetState == null)
                this.targetState = FindState(host.Manager.Behaviors.Definitions[this.children].Item1, targetStateName);
            foreach (var i in host.GetNearestEntities(range, children))
                if (!i.CurrentState.Is(targetState))
                    i.SwitchTo(targetState);
        }
    }
}
