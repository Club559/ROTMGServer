using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Collections.Concurrent;
using wServer.realm.entities;
using System.Threading;

namespace wServer.realm.commands
{
    interface ICommand
    {
        string Command { get; }
        int RequiredRank { get; }

        void Execute(Player player, string[] args);
    }
}
