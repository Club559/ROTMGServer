using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wServer.svrPackets;
using wServer.cliPackets;
using System.Collections.Concurrent;
using wServer.realm.worlds;
using wServer.logic;
using wServer.realm.entities.player;
using db;

namespace wServer.realm.entities
{
    public partial class Player
    {
        public void CreateGuild(RealmTime t, CreateGuildPacket pkt)
        {
            bool GuildsActive = true;
            if (GuildsActive == false)
            {
                psr.SendPacket(new CreateGuildResultPacket()
                {
                    Success = false,
                    ResultMessage = "Guilds currently disabled!"
                });
                return;
            }
            else
            {
                try
                {
                    string name = pkt.Name.ToString();
                    if (psr.Account.Stats.Fame >= 1000 || Guild != "")
                    {
                        if (name != "")
                        {
                            if (new Database().GetGuild(name) != null)
                            {
                                psr.SendPacket(new CreateGuildResultPacket()
                                {
                                    Success = false,
                                    ResultMessage = "Guild already exists!"
                                });
                                return;
                            }
                            using (Database db1 = new Database())
                            {
                                try
                                {
                                    if (Guild != "")
                                    {
                                        string oldname = psr.Account.Guild.Name;
                                        Guild g = db1.ChangeGuild(psr.Account, db1.GetGuildId(psr.Account.Guild.Name), 0, true);
                                        psr.Account.Guild.Name = g.Name;
                                        psr.Account.Guild.Rank = 0;
                                        Guild = g.Name;
                                        GuildRank = 0;
                                        UpdateCount++;
                                        psr.SendPacket(new NotificationPacket()
                                        {
                                            Text = "Left guild " + oldname,
                                            Color = new ARGB(0xFF008800),
                                            ObjectId = Id
                                        });
                                        psr.SendPacket(new CreateGuildResultPacket()
                                        {
                                            Success = true
                                        });
                                        foreach (var i in RealmManager.Worlds)
                                        {
                                            if (i.Key != 0)
                                            {
                                                foreach (var e in i.Value.Players)
                                                {
                                                    if (e.Value.Client.Account.Guild.Name == oldname)
                                                    {
                                                        e.Value.Client.SendPacket(new TextPacket()
                                                        {
                                                            BubbleTime = 0,
                                                            Stars = -1,
                                                            Name = "",
                                                            Recipient = "*Guild*",
                                                            Text = psr.Account.Name + " has left the guild!"
                                                        });
                                                    }
                                                }
                                            }
                                        }
                                        return;
                                    }
                                    else
                                    {
                                        if (pkt.Name != "")
                                        {
                                            Guild g = db1.CreateGuild(psr.Account, pkt.Name);
                                            psr.Account.Guild.Name = g.Name;
                                            psr.Account.Guild.Rank = g.Rank;
                                            Guild = g.Name;
                                            GuildRank = g.Rank;
                                            psr.SendPacket(new NotificationPacket()
                                            {
                                                Text = "Created guild " + g.Name,
                                                Color = new ARGB(0xFF008800),
                                                ObjectId = Id
                                            });
                                            psr.SendPacket(new CreateGuildResultPacket()
                                            {
                                                Success = true,
                                                ResultMessage = "Success!"
                                            });
                                            CurrentFame = psr.Account.Stats.Fame = psr.Database.UpdateFame(psr.Account, -1000);
                                            UpdateCount++;
                                            return;
                                        }
                                        else
                                        {
                                            psr.SendPacket(new CreateGuildResultPacket()
                                            {
                                                Success = false,
                                                ResultMessage = "Guild name cannot be blank!"
                                            });
                                            return;
                                        }
                                    }
                                }
                                catch (Exception e)
                                {
                                    psr.SendPacket(new CreateGuildResultPacket()
                                    {
                                        Success = false,
                                        ResultMessage = e.Message
                                    });
                                    return;
                                }
                            }
                        }
                        else
                        {
                            psr.SendPacket(new CreateGuildResultPacket()
                            {
                                Success = false,
                                ResultMessage = "Name cannot be empty!"
                            });
                        }
                    }
                    else
                    {
                        psr.SendPacket(new CreateGuildResultPacket()
                        {
                            Success = false,
                            ResultMessage = "Not enough fame!"
                        });
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Error at line 755 of Player.cs");
                    psr.SendPacket(new TextPacket()
                    {
                        Name = "",
                        Stars = -1,
                        BubbleTime = 0,
                        Text = "Error creating guild!"
                    });
                }
            }
        }
        public void JoinGuild(RealmTime t, JoinGuildPacket pkt)
        {
            Database db = new Database();
            GuildStruct gStruct = db.GetGuild(pkt.GuildName);
            if (psr.Player.Invited == false)
            {
                SendInfo("You need to be invited to join a guild!");
            }
            if (gStruct != null)
            {
                Guild g = db.ChangeGuild(psr.Account, gStruct.Id, 0, false);
                if (g != null)
                {
                    psr.Account.Guild = g;
                    Guild = g.Name;
                    GuildRank = g.Rank;
                    UpdateCount++;
                    foreach (var p in RealmManager.GuildMembersOf(g.Name))
                    {
                        p.Client.SendPacket(new TextPacket()
                        {
                            BubbleTime = 0,
                            Stars = -1,
                            //Name = "@" + psr.Account.Name + " has joined the guild!"
                            Name = "",
                            Recipient = "*Guild*",
                            Text = psr.Account.Name + " has joined the guild!"
                        });
                    }
                }
            }
        }
        public void InviteToGuild(RealmTime t, GuildInvitePacket pkt)
        {
            if (GuildRank >= 20)
            {
                foreach (var i in RealmManager.Clients.Values)
                {
                    foreach (var l in RealmManager.Worlds)
                    {
                        if (l.Key != 0)
                        {
                            foreach (var e in l.Value.Players)
                            {
                                if (e.Value.Name == pkt.Name)
                                {
                                    if (e.Value.Guild == "")
                                    {
                                        e.Value.Client.SendPacket(new InvitedToGuildPacket()
                                        {
                                            Name = psr.Account.Name,
                                            Guild = psr.Account.Guild.Name
                                        });
                                        i.Player.Invited = true;
                                        return;
                                    }
                                    else
                                    {
                                        SendError("Player is already in a guild!");
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                psr.SendPacket(new TextPacket()
                {
                    BubbleTime = 0,
                    Stars = -1,
                    Name = "",
                    Text = "Members and initiates cannot invite!"
                });
            }

        }
        public string ResolveGuildChatName()
        {
            return Name;
        }
        public string ResolveRankName(int rank)
        {
            string name;
            switch (rank)
            {
                case 0:
                    name = "Initiate"; break;
                case 10:
                    name = "Member"; break;
                case 20:
                    name = "Officer"; break;
                case 30:
                    name = "Leader"; break;
                case 40:
                    name = "Founder"; break;
                default:
                    name = ""; break;
            }
            return name;
        }

        public void ChangeGuildRank(RealmTime t, ChangeGuildRankPacket pkt)
        {
            string pname = pkt.Name;
            int rank = pkt.GuildRank;
            if (GuildRank > 20)
            {
                Player other = RealmManager.FindPlayer(pname);
                if (other != null && other.Guild == Guild)
                {
                    string rankname = ResolveRankName(other.GuildRank);
                    string rankname2 = ResolveRankName(rank);
                    other.GuildRank = rank;
                    other.Client.Account.Guild.Rank = rank;
                    new Database().ChangeGuild(other.Client.Account, other.Client.Account.Guild.Id, other.GuildRank, false);
                    other.UpdateCount++;
                    foreach (var p in RealmManager.GuildMembersOf(Guild))
                    {
                        p.Client.SendPacket(new TextPacket()
                        {
                            BubbleTime = 0,
                            Stars = -1,
                            Name = "",
                            Recipient = "*Guild*",
                            Text = other.Client.Account.Name + "'s rank has been changed to " + rankname2 + "."
                        });
                    }
                }
                else
                {
                    try
                    {
                        Database db = new Database();
                        Account acc = db.GetAccount(pname);
                        if (acc.Guild.Name == Guild)
                        {
                            string rankname = ResolveRankName(acc.Guild.Rank);
                            string rankname2 = ResolveRankName(rank);
                            db.ChangeGuild(acc, acc.Guild.Id, rank, false);
                            foreach (var p in RealmManager.GuildMembersOf(Guild))
                            {
                                p.Client.SendPacket(new TextPacket()
                                {
                                    BubbleTime = 0,
                                    Stars = -1,
                                    Name = "",
                                    Recipient = "*Guild*",
                                    Text = acc.Name + "'s rank has been changed to " + rankname2 + "."
                                });
                            }
                        }
                        else
                        {
                            psr.SendPacket(new TextPacket()
                            {
                                BubbleTime = 0,
                                Stars = -1,
                                Name = "*Error*",
                                Text = "You can only change a player in your guild."
                            });
                        }
                    }
                    catch (Exception e)
                    {
                        psr.SendPacket(new TextPacket()
                        {
                            BubbleTime = 0,
                            Stars = -1,
                            Name = "*Error*",
                            Text = e.Message
                        });
                    }
                }
            }
            else
            {
                psr.SendPacket(new TextPacket()
                {
                    BubbleTime = 0,
                    Stars = -1,
                    Name = "",
                    Text = "Members and initiates cannot promote!"
                });
            }
        }

        public void GuildRemove(RealmTime t, GuildRemovePacket pkt)
        {
            string pname = pkt.Name;
            try
            {
                Player p = RealmManager.FindPlayer(pname);
                if (p != null && p.Guild == Guild)
                {
                    Database db = new Database();
                    Guild g = db.ChangeGuild(p.Client.Account, p.Client.Account.Guild.Id, p.GuildRank, true);
                    p.Guild = "";
                    p.GuildRank = 0;
                    p.Client.Account.Guild = g;
                    p.UpdateCount++;
                    p.SendGuild("You have been kicked from the guild.");
                    foreach (var pl in RealmManager.GuildMembersOf(Guild))
                        pl.SendGuild(p.nName + " has been kicked from the guild by " + nName + ".");
                }
                else
                {
                    try
                    {
                        Database db = new Database();
                        Account other = db.GetAccount(pname);
                        if (other.Guild.Name == Guild)
                        {
                            db.ChangeGuild(other, other.Guild.Id, other.Guild.Rank, true);
                            foreach (var pl in RealmManager.GuildMembersOf(Guild))
                                pl.SendGuild(pname + " has been kicked from the guild by " + nName + ".");
                        }
                    }
                    catch (Exception e)
                    {
                        psr.SendPacket(new TextPacket()
                        {
                            BubbleTime = 0,
                            Stars = -1,
                            Name = "*Error*",
                            Text = e.Message
                        });
                    }
                }

            }
            catch (Exception e)
            {
                psr.SendPacket(new TextPacket()
                {
                    BubbleTime = 0,
                    Stars = -1,
                    Name = "*Error*",
                    Text = e.Message
                });
            }
        }
    }
}
