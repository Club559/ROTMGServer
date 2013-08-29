using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wServer.realm;
using wServer.realm.entities;
using wServer.networking.svrPackets;
using Mono.Game;

namespace wServer.logic.behaviors
{
    class Taunt : Behavior
    {
        //State storage: time

        float probability = 1;
        bool broadcast = false;
        Cooldown cooldown = new Cooldown(0, 0);
        string[] text;

        public Taunt(params string[] text)
        {
            this.text = text;
        }

        public Taunt(double probability, params string[] text)
        {
            this.text = text;
            this.probability = (float)probability;
        }
        public Taunt(bool broadcast, params string[] text)
        {
            this.text = text;
            this.broadcast = broadcast;
        }
        public Taunt(Cooldown cooldown, params string[] text)
        {
            this.text = text;
            this.cooldown = cooldown;
        }

        public Taunt(double probability, bool broadcast, params string[] text)
        {
            this.text = text;
            this.probability = (float)probability;
            this.broadcast = broadcast;
        }
        public Taunt(double probability, Cooldown cooldown, params string[] text)
        {
            this.text = text;
            this.probability = (float)probability;
            this.cooldown = cooldown;
        }
        public Taunt(bool broadcast, Cooldown cooldown, params string[] text)
        {
            this.text = text;
            this.broadcast = broadcast;
            this.cooldown = cooldown;
        }

        public Taunt(double probability, bool broadcast, Cooldown cooldown, params string[] text)
        {
            this.text = text;
            this.probability = (float)probability;
            this.broadcast = broadcast;
            this.cooldown = cooldown;
        }

        protected override void OnStateEntry(Entity host, RealmTime time, ref object state)
        {
            state = null;
        }

        protected override void TickCore(Entity host, RealmTime time, ref object state)
        {
            if (state != null && cooldown.CoolDown == 0) return;    //cooldown = 0 -> once per state entry

            int c;
            if (state == null) c = cooldown.Next(Random);
            else c = (int)state;

            c -= time.thisTickTimes;
            state = c;
            if (c > 0) return;

            c = cooldown.Next(Random);
            state = c;

            if (Random.NextDouble() >= probability) return;

            var taunt = text.Length == 1 ? text[0] : text[Random.Next(text.Length)];
            if (taunt.Contains("{PLAYER}"))
            {
                Entity player = host.GetNearestEntity(10, null);
                if (player == null) return;
                taunt = taunt.Replace("{PLAYER}", player.Name);
            }
            taunt = taunt.Replace("{HP}", (host as Enemy).HP.ToString());

            var packet = new TextPacket()
            {
                Name = "#" + (host.ObjectDesc.DisplayId ?? host.ObjectDesc.ObjectId),
                ObjectId = host.Id,
                Stars = -1,
                BubbleTime = 5,
                Recipient = "",
                Text = taunt,
                CleanText = ""
            };
            if (broadcast)
                host.Owner.BroadcastPacket(packet, null);
            else
                foreach (var i in host.Owner.PlayersCollision.HitTest(host.X, host.Y, 15))
                {
                    if (host.Dist(i) < 15)
                        (i as Player).Client.SendPacket(packet);
                }
        }
    }
}
