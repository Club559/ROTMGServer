using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using wServer.cliPackets;
using wServer.svrPackets;
using wServer.realm.setpieces;
using wServer.realm.worlds;
using wServer.realm.entities;
using wServer.realm.entities.player;
using wServer.logic.attack;
using terrain;
using db;

namespace wServer.realm.commands
{
    class SpawnCommand : ICommand
    {
        public string Command { get { return "spawn"; } }
        public int RequiredRank { get { return 3; } }

        public void Execute(Player player, string[] args)
        {
            int num;
            if (args.Length > 0 && int.TryParse(args[0], out num)) //multi
            {
                string name = string.Join(" ", args.Skip(1).ToArray());
                short objType;
                //creates a new case insensitive dictionary based on the XmlDatas
                Dictionary<string, short> icdatas = new Dictionary<string, short>(XmlDatas.IdToType, StringComparer.OrdinalIgnoreCase);
                if (!icdatas.TryGetValue(name, out objType) ||
                    !XmlDatas.ObjectDescs.ContainsKey(objType))
                {
                    player.SendInfo("Unknown entity!");
                }
                else
                {
                    int c = int.Parse(args[0]);
                    if (!(player.Client.Account.Rank > 2) && c > 200)
                    {
                        player.SendError("Maximum spawn count is set to 200!");
                        return;
                    }
                    else if (player.Client.Account.Rank > 2 && c > 200)
                    {
                        player.SendInfo("Bypass made!");
                    }
                    for (int i = 0; i < num; i++)
                    {
                        var entity = Entity.Resolve(objType);
                        entity.Move(player.X, player.Y);
                        player.Owner.EnterWorld(entity);
                    }
                    player.SendInfo("Success!");
                }
            }
            else
            {
                string name = string.Join(" ", args);
                short objType;
                //creates a new case insensitive dictionary based on the XmlDatas
                Dictionary<string, short> icdatas = new Dictionary<string, short>(XmlDatas.IdToType, StringComparer.OrdinalIgnoreCase);
                if (!icdatas.TryGetValue(name, out objType) ||
                    !XmlDatas.ObjectDescs.ContainsKey(objType))
                {
                    player.SendHelp("Usage: /spawn <entityname>");
                }
                else
                {
                    var entity = Entity.Resolve(objType);
                    entity.Move(player.X, player.Y);
                    player.Owner.EnterWorld(entity);
                }
            }
        }
    }

    class ArenaCommand : ICommand
    {
        public string Command { get { return "arena"; } }
        public int RequiredRank { get { return 3; } }

        public void Execute(Player player, string[] args)
        {
            var prtal = Portal.Resolve(0x1900);
            prtal.Move(player.X, player.Y);
            player.Owner.EnterWorld(prtal);
            World w = RealmManager.GetWorld(player.Owner.Id);
            w.Timers.Add(new WorldTimer(30 * 1000, (world, t) => //default portal close time * 1000
                
            {
                try
                {
                    w.LeaveWorld(prtal);
                }
                catch //couldn't remove portal, Owner became null. Should be fixed with RealmManager implementation
                {
                    Console.Out.WriteLine("Couldn't despawn portal.");
                }
            }));
            foreach (var i in RealmManager.Clients.Values)
                i.SendPacket(new TextPacket()
                {
                    BubbleTime = 0,
                    Stars = -1,
                    Name = "",
                    Text = "Arena Opened by:" + " " + player.nName
                });
            foreach (var i in RealmManager.Clients.Values)
            i.SendPacket(new NotificationPacket()
            {
                Color = new ARGB(0xff00ff00),
                ObjectId = player.Id,
                Text = "Arena Opened by " + player.nName
            });
        }
    }

    class AddEffCommand : ICommand
    {
        public string Command { get { return "addeff"; } }
        public int RequiredRank { get { return 3; } }

        public void Execute(Player player, string[] args)
        {
            if (args.Length == 0)
            {
                player.SendHelp("Usage: /addeff <Effectname or Effectnumber>");
            }
            else
            {
                try
                {
                    player.ApplyConditionEffect(new ConditionEffect()
                    {
                        Effect = (ConditionEffectIndex)Enum.Parse(typeof(ConditionEffectIndex), args[0].Trim(), true),
                        DurationMS = -1
                    });
                    {
                        player.SendInfo("Success!");
                    }
                }
                catch
                {
                    player.SendError("Invalid effect!");
                }
            }
        }
    }

    class RemoveEffCommand : ICommand
    {
        public string Command { get { return "remeff"; } }
        public int RequiredRank { get { return 3; } }

        public void Execute(Player player, string[] args)
        {
            if (args.Length == 0)
            {
                player.SendHelp("Usage: /remeff <Effectname or Effectnumber>");
            }
            else
            {
                try
                {
                    player.ApplyConditionEffect(new ConditionEffect()
                    {
                        Effect = (ConditionEffectIndex)Enum.Parse(typeof(ConditionEffectIndex), args[0].Trim(), true),
                        DurationMS = 0
                    });
                    player.SendInfo("Success!");
                }
                catch
                {
                    player.SendError("Invalid effect!");
                }
            }
        }
    }

    class GiveCommand : ICommand
    {
        public string Command { get { return "give"; } }
        public int RequiredRank { get { return 2; } }

        public void Execute(Player player, string[] args)
        {
            if (args.Length == 0)
            {
                player.SendHelp("Usage: /give <Itemname>");
            }
            else
            {
                string name = string.Join(" ", args.ToArray()).Trim();
                short objType;
                //creates a new case insensitive dictionary based on the XmlDatas
                Dictionary<string, short> icdatas = new Dictionary<string, short>(XmlDatas.IdToType, StringComparer.OrdinalIgnoreCase);
                if (!icdatas.TryGetValue(name, out objType))
                {
                    player.SendError("Unknown type!");
                    return;
                }
                if (!XmlDatas.ItemDescs[objType].Secret || player.Client.Account.Rank >= 4)
                {
                    for (int i = 0; i < player.Inventory.Length; i++)
                        if (player.Inventory[i] == null)
                        {
                            player.Inventory[i] = XmlDatas.ItemDescs[objType];
                            player.UpdateCount++;

                            player.SendInfo("Success!");
                            return;
                        }
                }
                else
                {
                    player.SendError("Item cannot be given!");
                }

            }
        }
    }

    class TpCommand : ICommand
    {
        public string Command { get { return "tp"; } }
        public int RequiredRank { get { return 2; } }

        public void Execute(Player player, string[] args)
        {
            if (args.Length == 0 || args.Length == 1) 
            {
                player.SendHelp("Usage: /tp <X coordinate> <Y coordinate>"); 
            }
            else
            {
                int x, y;
                try
                {
                    x = int.Parse(args[0]);
                    y = int.Parse(args[1]);
                }
                catch
                {
                    player.SendError("Invalid coordinates!");
                    return; 
                }
                player.Move(x + 0.5f, y + 0.5f);
                player.SetNewbiePeriod();
                player.UpdateCount++;
                player.Owner.BroadcastPacket(new GotoPacket()
                {
                    ObjectId = player.Id,
                    Position = new Position()
                    {
                        X = player.X,
                        Y = player.Y
                    }
                }, null); 
            }
        }
    }

    class SetpieceCommand : ICommand
    {
        public string Command { get { return "setpiece"; } }
        public int RequiredRank { get { return 4; } }

        public void Execute(Player player, string[] args)
        {
            if (args.Length == 0)
            {
                player.SendHelp("Usage: /setpiece <setpieceID>");
            }
            else
            {
                try
                {
                    ISetPiece piece = (ISetPiece)Activator.CreateInstance(Type.GetType(
                        "wServer.realm.setpieces." + args[0]));
                    piece.RenderSetPiece(player.Owner, new IntPoint((int)player.X + 1, (int)player.Y + 1));

                    player.SendInfo("Success!");
                }
                catch
                {
                    player.SendError("Cannot apply setpiece!");
                }
            }
        }
    }

    class DebugCommand : ICommand
    {
        public string Command { get { return "debug"; } }
        public int RequiredRank { get { return 4; } }
        
        public void Execute(Player player, string[] args)
        {
            
        }
    }

    class KillAll : ICommand
    {
        public string Command { get { return "killall"; } }
        public int RequiredRank { get { return 3; } }

        public void Execute(Player player, string[] args)
        {
            if (args.Length == 0)
            {
                player.SendHelp("Usage: /killall <entityname>");
            }
            else
            {
                foreach (var i in player.Owner.Enemies)
                {
                    if ((i.Value.ObjectDesc != null) &&
                        (i.Value.ObjectDesc.ObjectId != null) &&
                        (i.Value.ObjectDesc.ObjectId.Contains(args[0])))
                    {
                        i.Value.Damage(player, new RealmTime(), 1000 * 10000, true); //may not work for ents/liches
                        //i.Value.Owner.LeaveWorld(i.Value);
                    }
                }
                player.SendInfo("Success!");
            }
        }
    }

    class KillAllX : ICommand //this version gives XP points, but does not work for enemies with evaluation/inv periods
    {
        public string Command { get { return "killallx"; } }
        public int RequiredRank { get { return 3; } }

        public void Execute(Player player, string[] args)
        {
            if (args.Length == 0)
            {
                player.SendHelp("Usage: /killallx <entityname>");
            }
            else
            {
                foreach (var i in player.Owner.Enemies)
                {
                    if ((i.Value.ObjectDesc != null) &&
                        (i.Value.ObjectDesc.ObjectId != null) &&
                        (i.Value.ObjectDesc.ObjectId.Contains(args[0])))
                    {
                        i.Value.Damage(player, new RealmTime(), 1000 * 10000, true); //may not work for ents/liches, 
                        //i.Value.Owner.LeaveWorld(i.Value);
                    }
                }
                player.SendInfo("Success!");
            }
        }
    }

    class Kick : ICommand
    {
        public string Command { get { return "kick"; } }
        public int RequiredRank { get { return 4; } }

        public void Execute(Player player, string[] args)
        {
            if (args.Length == 0)
            {
                player.SendHelp("Usage: /kick <playername>");
            }
            else
            {
                try
                {
                    foreach (var i in player.Owner.Players)
                    {
                        if (i.Value.nName.ToLower() == args[0].ToLower().Trim())
                        {
                            player.SendInfo("Player Disconnected");
                            i.Value.Client.Disconnect();
                        }
                    }
                }
                catch
                {
                    player.SendError("Cannot kick!");
                }
            }
        }
    }

    class GetQuest : ICommand
    {
        public string Command { get { return "getquest"; } }
        public int RequiredRank { get { return 3; } }

        public void Execute(Player player, string[] args)
        {
            player.SendInfo("Loc: " + player.Quest.X + " " + player.Quest.Y);
        }
    }

    class OryxSay : ICommand
    {
        public string Command { get { return "osay"; } }
        public int RequiredRank { get { return 4; } }

        public void Execute(Player player, string[] args)
        {
            if (args.Length == 0)
            {
                player.SendHelp("Usage: /oryxsay <saytext>");
            }
            else
            {
                string saytext = string.Join(" ", args);
                player.SendEnemy("Oryx the Mad God", saytext);
            }
        }
    }

    class SWhoCommand : ICommand //get all players from all worlds (this may become too large!)
    {
        public string Command { get { return "swho"; } }
        public int RequiredRank { get { return 0; } }

        public void Execute(Player player, string[] args)
        {
            StringBuilder sb = new StringBuilder("All conplayers: ");

            foreach (var w in RealmManager.Worlds)
            {
                World world = w.Value;
                if (w.Key != 0)
                {
                    var copy = world.Players.Values.ToArray();
                    if (copy.Length != 0)
                    {
                        for (int i = 0; i < copy.Length; i++)
                        {
                            sb.Append(copy[i].Name);
                            sb.Append(", ");
                        }
                    }
                }
            }
            string fixedString = sb.ToString().TrimEnd(',', ' '); //clean up trailing ", "s

            player.SendInfo(fixedString);
        }
    }

    class Announcement : ICommand
    {
        public string Command { get { return "announce"; } } //msg all players in all worlds
        public int RequiredRank { get { return 3; } }

        public void Execute(Player player, string[] args)
        {
            if (args.Length == 0)
            {
                player.SendHelp("Usage: /announce <saytext>");
            }
            else
            {
                string saytext = string.Join(" ", args);

                foreach (var i in RealmManager.Clients.Values)
                    i.SendPacket(new TextPacket()
                    {
                        BubbleTime = 0,
                        Stars = -1,
                        Name = "#Announcement",
                        Text = " "+saytext
                    }); 
            }
        }
    }

    class Summon : ICommand
    {
        public string Command { get { return "summon"; } }
        public int RequiredRank { get { return 4; } }

        public void Execute(Player player, string[] args)
        {
            if (args.Length == 0)
            {
                player.SendHelp("Usage: /summon <playername>");
            }
            else
            {
                foreach (var i in player.Owner.Players)
                {
                    if (i.Value.nName.ToLower() == args[0].ToLower().Trim())
                    {
                        i.Value.Teleport(new RealmTime(), new cliPackets.TeleportPacket()
                        {
                            ObjectId = player.Id
                        });
                        return;
                    }
                }
                player.SendInfo(string.Format("Cannot summon, {0} not found!", args[0].Trim()));
            }
        }
    }

    class KillCommand : ICommand
    {
        public string Command { get { return "kill"; } }
        public int RequiredRank { get { return 4; } }

        public void Execute(Player player, string[] args)
        {
            if (args.Length == 0)
            {
                player.SendHelp("Usage: /kill <playername>");
            }
            else
            {
                foreach (var w in RealmManager.Worlds)
                {
                    //string death = string.Join(" ", args);
                    World world = w.Value;
                    if (w.Key != 0) // 0 is limbo??
                    {
                        foreach (var i in world.Players)
                        {
                            //Unnamed becomes a problem: skip them
                            if (i.Value.nName.ToLower() == args[0].ToLower().Trim() && i.Value.NameChosen)
                            {
                                i.Value.Death("Server Admin");
                                
                                return;
                            }
                        }
                    }
                }
                player.SendInfo(string.Format("Cannot kill, {0} not found!", args[0].Trim()));
            }
        }
    }

    class RestartCommand : ICommand
    {
        public string Command { get { return "restart"; } }
        public int RequiredRank { get { return 4; } }

        public void Execute(Player player, string[] args)
        {
            try
            {
                foreach (var w in RealmManager.Worlds)
                {
                    World world = w.Value;
                    if (w.Key != 0)
                    {
                        world.BroadcastPacket(new TextPacket()
                        {
                            Name = "#Announcement",
                            Stars = -1,
                            BubbleTime = 0,
                            Text = "Server restarting soon. Please be ready to disconnect. Estimated server down time: 30 Seconds - 1 Minute"
                        }, null);
                    }
                }

            }
            catch
            {
                player.SendError("Cannot say that in announcement!");
            }
        }
    }

    class VitalityCommand : ICommand
    {
        public string Command { get { return "vit"; } }
        public int RequiredRank { get { return 3; } }

        public void Execute(Player player, string[] args)
        {
            try
            {
                if (args.Length == 0)
                {
                    player.SendHelp("Use /vit <ammount>");
                }
                else if (args.Length == 1)
                {
                    player.Client.Player.Stats[5] = int.Parse(args[0]);
                    player.UpdateCount++;
                    player.SendInfo("Success!");
                }
            }
            catch
            {
                player.SendError("Error!");
            }
        }
    }

    class DefenseCommand : ICommand
    {
        public string Command { get { return "def"; } }
        public int RequiredRank { get { return 3; } }

        public void Execute(Player player, string[] args)
        {
            try
            {
                if (args.Length == 0)
                {
                    player.SendHelp("Use /def <ammount>");
                }
                else if (args.Length == 1)
                {
                    player.Client.Player.Stats[3] = int.Parse(args[0]);
                    player.UpdateCount++;
                    player.SendInfo("Success!");
                }
            }
            catch
            {
                player.SendError("Error!");
            }
        }
    }

    class AttackCommand : ICommand
    {
        public string Command { get { return "att"; } }
        public int RequiredRank { get { return 3; } }

        public void Execute(Player player, string[] args)
        {
            try
            {
                if (args.Length == 0)
                {
                    player.SendHelp("Use /att <ammount>");
                }
                else if (args.Length == 1)
                {
                    player.Client.Player.Stats[2] = int.Parse(args[0]);
                    player.UpdateCount++;
                    player.SendInfo("Success!");
                }
            }
            catch
            {
                player.SendError("Error!");
            }
        }
    }

    class DexterityCommand : ICommand
    {
        public string Command { get { return "dex"; } }
        public int RequiredRank { get { return 3; } }

        public void Execute(Player player, string[] args)
        {
            try
            {
                if (args.Length == 0)
                {
                    player.SendHelp("Use /dex <ammount>");
                }
                else if (args.Length == 1)
                {
                    player.Client.Player.Stats[7] = int.Parse(args[0]);
                    player.UpdateCount++;
                    player.SendInfo("Success!");
                }
            }
            catch
            {
                player.SendError("Error!");
            }
        }
    }

    class LifeCommand : ICommand
    {
        public string Command { get { return "hp"; } }
        public int RequiredRank { get { return 3; } }

        public void Execute(Player player, string[] args)
        {
            try
            {
                if (args.Length == 0)
                {
                    player.SendHelp("Use /hp <ammount>");
                }
                else if (args.Length == 1)
                {
                    player.Client.Player.Stats[0] = int.Parse(args[0]);
                    player.UpdateCount++;
                    player.SendInfo("Success!");
                }
            }
            catch
            {
                player.SendError("Error!");
            }
        }
    }

    class ManaCommand : ICommand
    {
        public string Command { get { return "mp"; } }
        public int RequiredRank { get { return 3; } }

        public void Execute(Player player, string[] args)
        {
            try
            {
                if (args.Length == 0)
                {
                    player.SendHelp("Use /mp <ammount>");
                }
                else if (args.Length == 1)
                {
                    player.Client.Player.Stats[1] = int.Parse(args[0]);
                    player.UpdateCount++;
                    player.SendInfo("Success!");
                }
            }
            catch
            {
                player.SendError("Error!");
            }
        }
    }

    class SpeedCommand : ICommand
    {
        public string Command { get { return "spd"; } }
        public int RequiredRank { get { return 3; } }

        public void Execute(Player player, string[] args)
        {
            try
            {
                if (args.Length == 0)
                {
                    player.SendHelp("Use /spd <ammount>");
                }
                else if (args.Length == 1)
                {
                    player.Client.Player.Stats[4] = int.Parse(args[0]);
                    player.UpdateCount++;
                    player.SendInfo("Success!");
                }
            }
            catch
            {
                player.SendError("Error!");
            }
        }
    }

    class WisdomCommand : ICommand
    {
        public string Command { get { return "wis"; } }
        public int RequiredRank { get { return 3; } }

        public void Execute(Player player, string[] args)
        {
            try
            {
                if (args.Length == 0)
                {
                    player.SendHelp("Use /spd <ammount>");
                }
                else if (args.Length == 1)
                {
                    player.Client.Player.Stats[6] = int.Parse(args[0]);
                    player.UpdateCount++;
                    player.SendInfo("Success!");
                }
            }
            catch
            {
                player.SendError("Error!");
            }
        }
    }

    class Whitelist : ICommand
    {
        public string Command { get { return "whitelist"; } }
        public int RequiredRank { get { return 4; } }

        public void Execute(Player player, string[] args)
        {
            if (args.Length == 0)
            {
                player.SendHelp("Usage: /whitelist <username>");
            }
            try
            {
                using (Database dbx = new Database())
                {
                    var cmd = dbx.CreateQuery();
                    cmd.CommandText = "UPDATE accounts SET rank=1 WHERE name=@name";
                    cmd.Parameters.AddWithValue("@name", args[0]);
                    if (cmd.ExecuteNonQuery() == 0)
                    {
                        player.SendInfo("Could not whitelist!");
                    }
                    else
                    {
                        player.SendInfo("Account successfully whitelisted!");
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Out.WriteLine(player.nName + " Has Whitelisted " + args[0]);
                        Console.ForegroundColor = ConsoleColor.White;

                        var dir = @"logs";
                        if (!System.IO.Directory.Exists(dir))
                            System.IO.Directory.CreateDirectory(dir);
                        using (System.IO.StreamWriter writer = new System.IO.StreamWriter(@"logs\WhitelistLog.txt", true))
                        {
                            writer.WriteLine("[" + DateTime.Now + "]" + player.nName + " Has Whitelisted " + args[0]);
                        } 
                    }
                }
            }
            catch
            {
                player.SendInfo("Could not whitelist!");
            }
        }
    }

    class Ban : ICommand
    {
        public string Command { get { return "ban"; } }
        public int RequiredRank { get { return 4; } }

        public void Execute(Player player, string[] args)
        {
            if (args.Length == 0)
            {
                player.SendHelp("Usage: /ban <username>");
            }
            try
            {
                using (Database dbx = new Database())
                {
                    var cmd = dbx.CreateQuery();
                    cmd.CommandText = "UPDATE accounts SET banned=1, rank=0 WHERE name=@name";
                    cmd.Parameters.AddWithValue("@name", args[0]);
                    if (cmd.ExecuteNonQuery() == 0)
                    {
                        player.SendInfo("Could not ban");
                    }
                    else
                    {
                        foreach (var i in player.Owner.Players)
                        {
                            if (i.Value.nName.ToLower() == args[0].ToLower().Trim())
                            {
                                i.Value.Client.Disconnect();
                                player.SendInfo("Account successfully Banned");
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.Out.WriteLine(args[0] + " was Banned.");
                                Console.ForegroundColor = ConsoleColor.White;
                            }
                        }
                    }
                }
            }
            catch
            {
                player.SendInfo("Could not ban");
            }
        }
    }

    class UnBan : ICommand
    {
        public string Command { get { return "unban"; } }
        public int RequiredRank { get { return 4; } }

        public void Execute(Player player, string[] args)
        {
            if (args.Length == 0)
            {
                player.SendHelp("Usage: /unban <username>");
            }
            try
            {
                using (Database dbx = new Database())
                {
                    var cmd = dbx.CreateQuery();
                    cmd.CommandText = "UPDATE accounts SET banned=0, rank=1 WHERE name=@name";
                    cmd.Parameters.AddWithValue("@name", args[0]);
                    if (cmd.ExecuteNonQuery() == 0)
                    {
                        player.SendInfo("Could not unban");
                    }
                    else
                    {
                        player.SendInfo("Account successfully Unbanned");
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Out.WriteLine(args[1]+" was Unbanned.");
                        Console.ForegroundColor = ConsoleColor.White;
                        
                    }
                }
            }
            catch
            {
                player.SendInfo("Could not unban, please unban in database");
            }
        }
    }

    class Rank : ICommand
    {
        public string Command { get { return "rank"; } }
        public int RequiredRank { get { return 4; } }

        public void Execute(Player player, string[] args)
        {
            if (args.Length < 2)
            {
                player.SendHelp("Usage: /rank <username> <number>\n0: Player\n1: Donator\n2: Game Master\n3: Developer\n4: Head Developer\n5: Admin");
            }
            else
            {
                try
                {
                    using (Database dbx = new Database())
                    {
                        var cmd = dbx.CreateQuery();
                        cmd.CommandText = "UPDATE accounts SET rank=@rank WHERE name=@name";
                        cmd.Parameters.AddWithValue("@rank", args[1]);
                        cmd.Parameters.AddWithValue("@name", args[0]);
                        if (cmd.ExecuteNonQuery() == 0)
                        {
                            player.SendInfo("Could not change rank");
                        }
                        else
                            player.SendInfo("Account rank successfully changed");
                    }
                }
                catch
                {
                    player.SendInfo("Could not change rank, please change rank in database");
                }
            }
        }
    }
    class GuildRank : ICommand
    {
        public string Command { get { return "grank"; } }
        public int RequiredRank { get { return 4; } }

        public void Execute(Player player, string[] args)
        {
            if (args.Length < 2)
            {
                player.SendHelp("Usage: /grank <username> <number>");
            }
            else
            {
                try
                {
                    using (Database dbx = new Database())
                    {
                        var cmd = dbx.CreateQuery();
                        cmd.CommandText = "UPDATE accounts SET guildRank=@guildRank WHERE name=@name";
                        cmd.Parameters.AddWithValue("@guildRank", args[1]);
                        cmd.Parameters.AddWithValue("@name", args[0]);
                        if (cmd.ExecuteNonQuery() == 0)
                        {
                            player.SendInfo("Could not change guild rank. Use 10, 20, 30, 40, or 50 (invisible)");
                        }
                        else
                            player.SendInfo("Guild rank successfully changed");
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Out.WriteLine(args[1] + "'s guild rank has been changed");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
                catch
                {
                    player.SendInfo("Could not change rank, please change rank in database");
                }
            }
        }
    }
    class ChangeGuild : ICommand
    {
        public string Command { get { return "setguild"; } }
        public int RequiredRank { get { return 4; } }

        public void Execute(Player player, string[] args)
        {
            if (args.Length < 2)
            {
                player.SendHelp("Usage: /setguild <username> <guild id>");
            }
            else
            {
                try
                {
                    using (Database dbx = new Database())
                    {
                        var cmd = dbx.CreateQuery();
                        cmd.CommandText = "UPDATE accounts SET guild=@guild WHERE name=@name";
                        cmd.Parameters.AddWithValue("@guild", args[1]);
                        cmd.Parameters.AddWithValue("@name", args[0]);
                        if (cmd.ExecuteNonQuery() == 0)
                        {
                            player.SendInfo("Could not change guild.");
                        }
                        else
                            player.SendInfo("Guild successfully changed");
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Out.WriteLine(args[1] + "'s guild has been changed");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
                catch
                {
                    player.SendInfo("Could not change guild, please change in database.                                Use /setguild <username> <guild id>");
                }
            }
        }
    }
    
    class TqCommand : ICommand
    {
        public string Command { get { return "tq"; } }
        public int RequiredRank { get { return 3; } }

        public void Execute(Player player, string[] args)
        {

            if (player.Quest == null)
            {
                player.SendError("Player does not have a quest!");
            }
            else
                player.Move(player.X + 0.5f, player.Y + 0.5f);
            //player.SetNewbiePeriod();
            player.UpdateCount++;
            player.Owner.BroadcastPacket(new GotoPacket()
            {
                ObjectId = player.Id,
                Position = new Position()
                {
                    X = player.Quest.X,
                    Y = player.Quest.Y
                }
            }, null);
            player.SendInfo("Success!");
        }
    }

        class GodMode : ICommand
    {
        public string Command { get { return "god"; } }
        public int RequiredRank { get { return 3; } }

        public void Execute(Player player, string[] args)
        {
            if (player.HasConditionEffect(ConditionEffects.Invincible))
            {
                player.ApplyConditionEffect(new ConditionEffect()
                {
                    Effect = ConditionEffectIndex.Invincible,
                    DurationMS = 0
                });
                player.SendInfo("Godmode Off");
            }
            else
            {
               
                player.ApplyConditionEffect(new ConditionEffect()
                {
                    Effect = ConditionEffectIndex.Invincible,
                    DurationMS = -1
                });
                player.SendInfo("Godmode On");
                foreach (var i in RealmManager.Clients.Values)
                    i.SendPacket(new NotificationPacket()
                    {
                        Color = new ARGB(0xff00ff00),
                        ObjectId = player.Id,
                        Text = "Godmode Activated"
                    });
            }
        }
    }
    class GetIPCommand : ICommand
    {
        public string Command { get { return "ip"; } }
        public int RequiredRank { get { return 4; } }

        public void Execute(Player player, string[] args)
        {
            if (args.Length == 0)
            {
                player.SendHelp("Usage: /ip <PLAYER>");
                player.SendError("NOT READY FOR USAGE");
            }
            else
            {
               
            }
        }
    }
    
    class VanishCommand : ICommand
    {
        public string Command { get { return "vanish"; } }
        public int RequiredRank { get { return 3; } }

        public void Execute(Player player, string[] args)
        {
            if (args.Length >0)
            {
                player.SendHelp("Usage: /vanish");
              
            }
            else
            {
               
            }
        }
    }
    class StarCommand : ICommand
    {
        public string Command { get { return "stars"; } }
        public int RequiredRank { get { return 2; } }

        public void Execute(Player player, string[] args)
        {
            try
            {
                if (args.Length == 0)
                {
                    player.SendHelp("Use /stars <ammount>");
                }
                else if (args.Length == 1)
                {
                    player.Client.Player.Stars = int.Parse(args[0]);
                    player.UpdateCount++;
                    player.SendInfo("Success!");
                }
            }
            catch
            {
                player.SendError("Error!");
            }
        }
    }

    class LevelCommand : ICommand
    {
        public string Command { get { return "level"; } }
        public int RequiredRank { get { return 3; } }

        public void Execute(Player player, string[] args)
        {
            try
            {
                if (args.Length == 0)
                {
                    player.SendHelp("Use /level <ammount>");
                }
                else if (args.Length == 1)
                {
                    player.Client.Character.Level = int.Parse(args[0]);
                    player.Client.Player.Level = int.Parse(args[0]);
                    player.UpdateCount++;
                    player.SendInfo("Success!");
                }
            }
            catch
            {
                player.SendError("Error!");
            }
        }
    }

    class NameCommand : ICommand
    {
        public string Command { get { return "name"; } }
        public int RequiredRank { get { return 4; } }

        public void Execute(Player player, string[] args)
        {
            if (args.Length == 0)
            {
                player.SendHelp("Use /name <name>");
            }
            else if (args.Length == 1)
            {
                using (Database db = new Database())
                {
                    var db1 = db.CreateQuery();
                    db1.CommandText = "SELECT COUNT(name) FROM accounts WHERE name=@name;";
                    db1.Parameters.AddWithValue("@name", args[0]);
                    if ((int)(long)db1.ExecuteScalar() > 0)
                    {
                        player.SendError("Name Already In Use.");
                    }
                    else
                    {
                        db1 = db.CreateQuery();
                        db1.CommandText = "UPDATE accounts SET name=@name WHERE id=@accId";
                        db1.Parameters.AddWithValue("@name", args[0].ToString());
                        db1.Parameters.AddWithValue("@accId", player.Client.Account.AccountId.ToString());
                        if (db1.ExecuteNonQuery() > 0)
                        {
                            player.Client.Player.Credits = db.UpdateCredit(player.Client.Account, -0);
                            player.Client.Player.Name = args[0];
                            player.Client.Player.NameChosen = true;
                            player.Client.Player.UpdateCount++;
                            player.SendInfo("Success!");
                        }
                        else
                        {
                            player.SendError("Internal Server Error Occurred.");
                        }
                    }
                }
            }
        }
    }

    class RenameCommand : ICommand
    {
        public string Command { get { return "rename"; } }
        public int RequiredRank { get { return 4; } }

        public void Execute(Player player, string[] args)
        {
            if (args.Length == 0 || args.Length == 1)
            {
                player.SendHelp("Use /rename <OldPlayerName> <NewPlayerName>");
            }
            else if (args.Length == 2)
            {
                using (Database db = new Database())
                {
                    var db1 = db.CreateQuery();
                    db1.CommandText = "SELECT COUNT(name) FROM accounts WHERE name=@name;";
                    db1.Parameters.AddWithValue("@name", args[1]);
                    if ((int)(long)db1.ExecuteScalar() > 0)
                    {
                        player.SendError("Name Already In Use.");
                    }
                    else
                    {
                        db1 = db.CreateQuery();
                        db1.CommandText = "SELECT COUNT(name) FROM accounts WHERE name=@name";
                        db1.Parameters.AddWithValue("@name", args[0]);
                        if ((int)(long)db1.ExecuteScalar() < 1)
                        {
                            player.SendError("Name Not Found.");
                        }
                        else
                        {
                            db1 = db.CreateQuery();
                            db1.CommandText = "UPDATE accounts SET name=@newName, namechosen=TRUE WHERE name=@oldName;";
                            db1.Parameters.AddWithValue("@newName", args[1]);
                            db1.Parameters.AddWithValue("@oldName", args[0]);
                            if (db1.ExecuteNonQuery() > 0)
                            {
                                foreach (var playerX in RealmManager.Worlds)
                                {
                                    if (playerX.Key != 0)
                                    {
                                        World world = playerX.Value;
                                        foreach (var p in world.Players)
                                        {
                                            Player Client = p.Value;
                                            if ((player.Name.ToLower() == args[0].ToLower()) && player.NameChosen)
                                            {
                                                player.Name = args[1];
                                                player.NameChosen = true;
                                                player.UpdateCount++;
                                                break;
                                            }
                                        }
                                    }
                                }
                                player.SendInfo("Success!");
                                //
                            }
                            else
                            {
                                player.SendError("Internal Server Error Occurred.");
                            }
                        }
                    }
                }
            }
        }
    }
    /*
    class giveEffCommand : ICommand
    {
        public string Command { get { return "giveeff"; } }
        public int RequiredRank { get { return 4; } }

        public void Execute(Player player, string[] args)
        {
            if (args.Length == 0)
            {
                player.SendHelp("Usage: /giveeff <Effectname or Effectnumber> <Playername without prefixes>");
            }
            else
            {
                try
                {
                    var n = "";
                    foreach (var i in RealmManager.Clients.Values)
                    {
                        if (i.Account.Name.ToUpper().StartsWith("[gm]"))
                        {
                            n = i.Account.Name.Substring(4);
                        }
                        if (i.Account.Name.ToUpper().StartsWith("[dev]"))
                        {
                            n = i.Account.Name.Substring(5);
                        }
                        if (i.Account.Name.ToUpper().StartsWith("[hdev]"))
                        {
                            n = i.Account.Name.Substring(6);
                        }
                        if (i.Account.Name.ToUpper().StartsWith("[admin]"))
                        {
                            n = i.Account.Name.Substring(7);
                        }

                        if (n.ToUpper() == args[0].ToUpper())
                        {
                            ConditionEffectIndex cond = (ConditionEffectIndex)Enum.Parse(typeof(ConditionEffectIndex), args[1].Trim());
                            //check if the player already has the condition effect
                            i.Player.ApplyConditionEffect(new ConditionEffect()
                            {
                                Effect = cond,
                                DurationMS = -1
                            });
                            {
                                player.SendInfo("Success!");
                            }
                        }
                    }
                }
                catch
                {
                    player.SendError("Invalid effect!");
                }
            }
        }
    }
    */
    class messageCommand : ICommand
    {
        public string Command { get { return "message"; } }
        public int RequiredRank { get { return 4; } }

        public void Execute(Player player, string[] args)
        {
            if (args.Length == 0)
            {
                player.SendHelp("Usage: /message <title> <message>");
            }
            else
            {
                string title = string.Join(" ", args);
                string message = string.Join(" ", args.Skip(1).ToArray());
                foreach (var i in RealmManager.Clients.Values)
                    i.SendPacket(new TextBoxPacket()
                    {
                        Title = args[0],
                        Message = message,
                        Button1 = "Ok",
                        Type = "GlobalMsg"
                    });
            }
        }
    }
}
