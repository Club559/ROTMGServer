using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using wServer.svrPackets;
using wServer.cliPackets;
using System.Xml;
using db;
using wServer.realm;
using wServer.realm.entities;
using wServer.realm.entities.player;

namespace wServer
{
    public enum ProtocalStage
    {
        Connected,
        Handshaked,
        Ready,
        Disconnected
    }
    public class ClientProcessor
    {
        Socket skt;
        Thread wkrThread;
        public RC4 ReceiveKey { get; private set; }
        public RC4 SendKey { get; private set; }

        public string clientVer = "0.5.2"; //Might want this

        public RealmManager Manager { get; private set; }
        public Socket Socket { get { return skt; } }

        public ClientProcessor(Socket skt)
        {
            this.skt = skt;
            ReceiveKey = new RC4(new byte[] { 0x31, 0x1f, 0x80, 0x69, 0x14, 0x51, 0xc7, 0x1b, 0x09, 0xa1, 0x3a, 0x2a, 0x6e });
            SendKey = new RC4(new byte[] { 0x72, 0xc5, 0x58, 0x3c, 0xaf, 0xb6, 0x81, 0x89, 0x95, 0xcb, 0xd7, 0x4b, 0x80 });
        }

        NetworkHandler handler;
        public void BeginProcess()
        {
            handler = new NetworkHandler(this, skt);
            handler.BeginHandling();
        }

        public void SendPacket(Packet pkt)
        {
            handler.SendPacket(pkt);
        }
        public void SendPackets(IEnumerable<Packet> pkts)
        {
            handler.SendPackets(pkts);
        }

        //internal bool ProcessPacket(Packet pkt)
        public bool IsReady()
        {
            if (stage == ProtocalStage.Disconnected)
                return false;
            if (stage == ProtocalStage.Ready && (entity == null || entity != null && entity.Owner == null))
                return false;
            return true;
        }
        
        internal void ProcessPacket(Packet pkt)
        {
            try
            {
                if (pkt.ID == PacketID.Hello)
                    ProcessHelloPacket(pkt as HelloPacket);
                else if (pkt.ID == PacketID.Create)
                    ProcessCreatePacket(pkt as CreatePacket);
                else if (pkt.ID == PacketID.Load)
                    ProcessLoadPacket(pkt as LoadPacket);
                else if (pkt.ID == PacketID.Pong)
                    entity.Pong(pkt as PongPacket);
                else if (pkt.ID == PacketID.Move)
                    RealmManager.Logic.AddPendingAction(t => entity.Move(t, pkt as MovePacket), PendingPriority.Networking);
                else if (pkt.ID == PacketID.PlayerShoot)
                    RealmManager.Logic.AddPendingAction(t => entity.PlayerShoot(t, pkt as PlayerShootPacket), PendingPriority.Networking);
                else if (pkt.ID == PacketID.EnemyHit)
                    RealmManager.Logic.AddPendingAction(t => entity.EnemyHit(t, pkt as EnemyHitPacket), PendingPriority.Networking);
                else if (pkt.ID == PacketID.OtherHit)
                    RealmManager.Logic.AddPendingAction(t => entity.OtherHit(t, pkt as OtherHitPacket), PendingPriority.Networking);
                else if (pkt.ID == PacketID.SquareHit)
                    RealmManager.Logic.AddPendingAction(t => entity.SquareHit(t, pkt as SquareHitPacket), PendingPriority.Networking);
                else if (pkt.ID == PacketID.PlayerHit)
                    RealmManager.Logic.AddPendingAction(t => entity.PlayerHit(t, pkt as PlayerHitPacket), PendingPriority.Networking);
                else if (pkt.ID == PacketID.ShootAck) //Spam
                    RealmManager.Logic.AddPendingAction(t => entity.ShootAck(t, pkt as ShootAckPacket), PendingPriority.Networking);
                else if (pkt.ID == PacketID.InvSwap)
                    RealmManager.Logic.AddPendingAction(t => entity.InventorySwap(t, pkt as InvSwapPacket), PendingPriority.Networking);
                else if (pkt.ID == PacketID.InvDrop)
                    RealmManager.Logic.AddPendingAction(t => entity.InventoryDrop(t, pkt as InvDropPacket), PendingPriority.Networking);
                else if (pkt.ID == PacketID.UseItem)
                    RealmManager.Logic.AddPendingAction(t => entity.UseItem(t, pkt as UseItemPacket), PendingPriority.Networking);
                else if (pkt.ID == PacketID.UsePortal)
                    RealmManager.Logic.AddPendingAction(t => entity.UsePortal(t, pkt as UsePortalPacket), PendingPriority.Networking);
                else if (pkt.ID == PacketID.PlayerText)
                    RealmManager.Logic.AddPendingAction(t => entity.PlayerText(t, pkt as PlayerTextPacket), PendingPriority.Networking);
                else if (pkt.ID == PacketID.ChooseName)
                    ProcessChooseNamePacket(pkt as ChooseNamePacket);
                else if (pkt.ID == PacketID.Escape)
                    ProcessEscapePacket(pkt as EscapePacket);
                else if (pkt.ID == PacketID.Teleport)
                    RealmManager.Logic.AddPendingAction(t => entity.Teleport(t, pkt as TeleportPacket), PendingPriority.Networking);
                else if (pkt.ID == PacketID.GotoAck)
                    RealmManager.Logic.AddPendingAction(t => entity.GotoAck(t, pkt as GotoAckPacket), PendingPriority.Networking);
                else if (pkt.ID == PacketID.EditAccountList)
                    RealmManager.Logic.AddPendingAction(t => entity.EditAccountList(t, pkt as EditAccountListPacket), PendingPriority.Networking);
                else if (pkt.ID == PacketID.Buy)
                    RealmManager.Logic.AddPendingAction(t => entity.Buy(t, pkt as BuyPacket), PendingPriority.Networking);
                else if (pkt.ID == PacketID.RequestTrade)
                    RealmManager.Logic.AddPendingAction(t => entity.RequestTrade(t, pkt as RequestTradePacket), PendingPriority.Networking);
                else if (pkt.ID == PacketID.ChangeTrade)
                    RealmManager.Logic.AddPendingAction(t => entity.ChangeTrade(t, pkt as ChangeTradePacket), PendingPriority.Networking);
                else if (pkt.ID == PacketID.AcceptTrade)
                    RealmManager.Logic.AddPendingAction(t => entity.AcceptTrade(t, pkt as AcceptTradePacket), PendingPriority.Networking);
                else if (pkt.ID == PacketID.CancelTrade)
                    RealmManager.Logic.AddPendingAction(t => entity.CancelTrade(t, pkt as CancelTradePacket), PendingPriority.Networking);
                else if (pkt.ID == PacketID.AOEAck)
                    RealmManager.Logic.AddPendingAction(t => entity.AOEAck(t, pkt as AOEAckPacket), PendingPriority.Networking);
                else if (pkt.ID == PacketID.GroundDamage)
                    RealmManager.Logic.AddPendingAction(t => entity.GroundDamage(t, pkt as GroundDamagePacket), PendingPriority.Networking);
                else if (pkt.ID == PacketID.CheckCredits)
                    RealmManager.Logic.AddPendingAction(t => entity.CheckCredits(t, pkt as CheckCreditsPacket), PendingPriority.Networking);
                else if (pkt.ID == PacketID.GuildRemove)
                    RealmManager.Logic.AddPendingAction(t => entity.GuildRemove(t, pkt as GuildRemovePacket), PendingPriority.Networking);
                else if (pkt.ID == PacketID.CreateGuild)
                    RealmManager.Logic.AddPendingAction(t => entity.CreateGuild(t, pkt as CreateGuildPacket), PendingPriority.Networking);
                else if (pkt.ID == PacketID.JoinGuild)
                    RealmManager.Logic.AddPendingAction(t => entity.JoinGuild(t, pkt as JoinGuildPacket), PendingPriority.Networking);
                else if (pkt.ID == PacketID.GuildInvite)
                    RealmManager.Logic.AddPendingAction(t => entity.InviteToGuild(t, pkt as GuildInvitePacket), PendingPriority.Networking);
                else if (pkt.ID == PacketID.ChangeGuildRank)
                    RealmManager.Logic.AddPendingAction(t => entity.ChangeGuildRank(t, pkt as ChangeGuildRankPacket), PendingPriority.Networking);
                else if (pkt.ID == PacketID.Visibullet)
                    RealmManager.Logic.AddPendingAction(t => entity.VisibulletHit(pkt as VisibulletPacket), PendingPriority.Networking);
                else if (pkt.ID == PacketID.TextBoxButton)
                    RealmManager.Logic.AddPendingAction(t => entity.TextBoxButton(pkt as TextBoxButtonPacket), PendingPriority.Networking);
                else if (pkt.ID == PacketID.UpdateAck)
                    RealmManager.Logic.AddPendingAction(t => HandleUpdateAck(), PendingPriority.Networking);
                else
                {
                    Console.WriteLine("Unhandled packet: " + pkt.ToString());
                }
            }
            catch (Exception e)
            {
                Console.Out.WriteLine(pkt.ToString());
                Console.Out.WriteLine(e);
                Disconnect();
            }
        }

        public void Disconnect()
        {
            try
            {
                if (stage == ProtocalStage.Disconnected) return;
                var original = stage;
                stage = ProtocalStage.Disconnected;
                if (account != null)
                    DisconnectFromRealm();
                if (db != null && original != ProtocalStage.Ready)
                {
                    db.Dispose();
                    db = null;
                }
                skt.Close();
            }
            catch
            {
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.WriteLine("Error disconnecting client, check ClientProcessor.cs");
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
        public void Save()
        {
            try
            {
                if (db != null)
                {
                    if (character != null)
                    {
                        entity.SaveToCharacter();
                        if (entity.Owner.Id != -6)
                            db.SaveCharacter(account, character);
                    }
                    db.Dispose();
                    db = null;
                }
                else
                {
                    db = new Database();
                    if (character != null)
                    {
                        entity.SaveToCharacter();
                        if (entity.Owner.Id != -6)
                            db.SaveCharacter(account, character);
                    }
                    db.Dispose();
                    db = null;
                }
            }
            catch { }
        }

        Database db;
        Account account;
        Char character;
        Player entity;
        bool isGuest = false;
        ProtocalStage stage;
        public uint UpdateAckCount = 0;

        ClientProcessor psr;

        public Database Database { get { return db; } }
        public Char Character { get { return character; } }
        public Account Account { get { return account; } }
        public ProtocalStage Stage { get { return stage; } }
        public Player Player { get { return entity; } }
        public wRandom Random { get; private set; }
        public string ConnectedBuild { get; private set; }

        int targetWorld = -1;
        void ProcessHelloPacket(HelloPacket pkt)
        {
            if (isGuest)
                Disconnect();
            db = new Database();
            if ((account = db.Verify(pkt.GUID, pkt.Password)) == null)
            {
                Console.WriteLine("Account not verified.");
                account = Database.CreateGuestAccount(pkt.GUID);

                if (account == null)
                {
                    Console.WriteLine("Account is null!");
                    SendPacket(new svrPackets.FailurePacket()
                    {
                        Message = "Invalid account."
                    });
                    Disconnect();
                    return;
                }
            }
            Console.WriteLine("Client trying to connect!");
            ConnectedBuild = pkt.BuildVersion;
            if (!RealmManager.TryConnect(this))
            {
                if (CheckAccountInUse(account.AccountId) != false)
                {
                    Console.WriteLine("Account in use: " + account.AccountId + " " + account.Name);
                    account = null;
                    SendPacket(new svrPackets.FailurePacket()
                    {
                        Message = "Account in use! Retrying..."
                    });
                    Disconnect();
                    return;
                }
                account = null;
                SendPacket(new svrPackets.FailurePacket()
                {
                    Message = "Failed to connect."
                });
                Disconnect();
                Console.WriteLine("Failed to connect.");
                return;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Client loading world");
                Console.ForegroundColor = ConsoleColor.White;
                World world = RealmManager.GetWorld(pkt.GameId);
                if (world == null)
                {
                    SendPacket(new svrPackets.FailurePacket()
                    {
                        Message = "Invalid world."
                    });
                    Disconnect();
                    Console.WriteLine("Invalid world");
                }
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Client joined world " + world.Id.ToString());
                Console.ForegroundColor = ConsoleColor.White;
                if (world.Id == -6) //Test World
                    (world as realm.worlds.Test).LoadJson(pkt.MapInfo);
                    
                else if (world.IsLimbo)
                    world = world.GetInstance(this);
                var seed = (uint)((long)Environment.TickCount * pkt.GUID.GetHashCode()) % uint.MaxValue;
                Random = new wRandom(seed);
                targetWorld = world.Id;
                SendPacket(new MapInfoPacket()
                {
                    Width = world.Map.Width,
                    Height = world.Map.Height,
                    Name = world.Name,
                    Seed = seed,
                    Background = world.Background,
                    AllowTeleport = world.AllowTeleport,
                    ShowDisplays = world.ShowDisplays,
                    ClientXML = world.ClientXML,
                    ExtraXML = world.ExtraXML
                });
                stage = ProtocalStage.Handshaked;
            }
        }

        void ProcessCreatePacket(CreatePacket pkt)
        {
            int nextCharId = 1;
            nextCharId = db.GetNextCharID(account);
            var cmd = db.CreateQuery();
            cmd.CommandText = "SELECT maxCharSlot FROM accounts WHERE id=@accId;";
            cmd.Parameters.AddWithValue("@accId", account.AccountId);
            int maxChar = (int)cmd.ExecuteScalar();

            cmd = db.CreateQuery();
            cmd.CommandText = "SELECT COUNT(id) FROM characters WHERE accId=@accId AND dead = FALSE;";
            cmd.Parameters.AddWithValue("@accId", account.AccountId);
            int currChar = (int)(long)cmd.ExecuteScalar();

            if (currChar >= maxChar)
            {
                Disconnect();
                return;
            }
            if (CheckAccountInUse(account.AccountId) != false)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("Account in use: " + account.AccountId);
                Console.ForegroundColor = ConsoleColor.White;
                SendPacket(new svrPackets.FailurePacket()
                {
                    Message = "Account in use! Retrying..."
                });
                return;
            }

            character = Database.CreateCharacter(pkt.ObjectType, nextCharId);

            int[] stats = new int[]
            {
                character.MaxHitPoints,
                character.MaxMagicPoints,
                character.Attack,
                character.Defense,
                character.Speed,
                character.Dexterity,
                character.HpRegen,
                character.MpRegen,
            };

            bool ok = true;
            cmd = db.CreateQuery();
            cmd.Parameters.AddWithValue("@accId", account.AccountId);
            cmd.Parameters.AddWithValue("@charId", nextCharId);
            cmd.Parameters.AddWithValue("@charType", pkt.ObjectType);
            cmd.Parameters.AddWithValue("@items", character._Equipment);
            cmd.Parameters.AddWithValue("@stats", Utils.GetCommaSepString(stats));
            cmd.Parameters.AddWithValue("@fameStats", character.FameStats.ToString());
            cmd.CommandText = "INSERT INTO characters (accId, charId, charType, level, exp, fame, items, hp, mp, stats, dead, pet, fameStats) VALUES (@accId, @charId, @charType, 1, 0, 0, @items, 100, 100, @stats, FALSE, -1, @fameStats);";
            int v = cmd.ExecuteNonQuery();
            ok = v > 0;

            if (ok)
            {
                var target = RealmManager.Worlds[targetWorld];
                //Delay to let client load remote texture
                target.Timers.Add(new WorldTimer(2000, (w, t) =>
                {
                    SendPacket(new CreateResultPacket()
                    {
                        CharacterID = character.CharacterId,
                        ObjectID = RealmManager.Worlds[targetWorld].EnterWorld(entity = new Player(this))
                    });
                }));
                stage = ProtocalStage.Ready;
            }
            else
                SendPacket(new svrPackets.FailurePacket()
                {
                    Message = "Failed to Load character."
                });
        }

        void ProcessLoadPacket(LoadPacket pkt)
        {
            character = db.LoadCharacter(account, pkt.CharacterId);
            if (character != null)
            {
                if (character.Dead)
                    SendPacket(new svrPackets.FailurePacket()
                    {
                        Message = "Character is dead."
                    });
                else
                {
                    var target = RealmManager.Worlds[targetWorld];
                    //Delay to let client load remote texture
                    target.Timers.Add(new WorldTimer(500, (w, t) =>
                    {
                        SendPacket(new CreateResultPacket()
                        {
                            CharacterID = character.CharacterId,
                            ObjectID = RealmManager.Worlds[targetWorld].EnterWorld(entity = new Player(this))
                        });
                    }));
                    stage = ProtocalStage.Ready;
                }
            }
            else
            {
                Player.SendInfo("Failed to Load character.");
                Disconnect();
            }
        }

        void ProcessChooseNamePacket(ChooseNamePacket pkt)
        {
            bool namechosen = false;
            var cmdx = db.CreateQuery();
            cmdx.CommandText = "SELECT namechosen FROM accounts WHERE id=@accId";
            cmdx.Parameters.AddWithValue("@accId", account.AccountId);
            object execx = cmdx.ExecuteScalar();
            namechosen = bool.Parse(execx.ToString());

            if (string.IsNullOrEmpty(pkt.Name) ||
                pkt.Name.Length > 10)
            {
                SendPacket(new NameResultPacket()
                {
                    Success = false,
                    Message = "Invalid name"
                });
                return;
            }
            else
            {
                var cmd = db.CreateQuery();
                cmd.CommandText = "SELECT COUNT(name) FROM accounts WHERE name=@name;";
                cmd.Parameters.AddWithValue("@name", pkt.Name);
                object x = cmd.ExecuteScalar();
                if (int.Parse(x.ToString()) > 0)
                    SendPacket(new NameResultPacket()
                    {
                        Success = false,
                        Message = "Duplicated name"
                    });
                else
                {
                    db.ReadStats(account);
                    if (account.Credits < 1000 && namechosen == true)
                        SendPacket(new NameResultPacket()
                        {
                            Success = false,
                            Message = "Not enough credits"
                        });
                    else
                    {
                        if (account.NameChosen == false)
                        {
                            cmd = db.CreateQuery();
                            cmd.CommandText = "UPDATE accounts SET name=@name, namechosen=TRUE WHERE id=@accId;";
                            cmd.Parameters.AddWithValue("@accId", account.AccountId);
                            cmd.Parameters.AddWithValue("@name", pkt.Name);
                            if (cmd.ExecuteNonQuery() > 0)
                            {
                                entity.Name = pkt.Name;
                                entity.NameChosen = true;
                                entity.UpdateCount++;
                                SendPacket(new NameResultPacket()
                                {
                                    Success = true,
                                    Message = "Success!"
                                });
                            }
                            else
                                SendPacket(new NameResultPacket()
                                {
                                    Success = false,
                                    Message = "Internal Error"
                                });
                        }
                        else
                        {
                            cmd = db.CreateQuery();
                            cmd.CommandText = "UPDATE accounts SET name=@name, namechosen=TRUE WHERE id=@accId;";
                            cmd.Parameters.AddWithValue("@accId", account.AccountId);
                            cmd.Parameters.AddWithValue("@name", pkt.Name);
                            if (cmd.ExecuteNonQuery() > 0)
                            {
                                entity.Credits = db.UpdateCredit(account, -1000);
                                entity.Name = pkt.Name;
                                entity.NameChosen = true;
                                entity.UpdateCount++;
                                SendPacket(new NameResultPacket()
                                {
                                    Success = true,
                                    Message = "Success!"
                                });
                            }
                            else
                                SendPacket(new NameResultPacket()
                                {
                                    Success = false,
                                    Message = "Internal Error"
                                });
                        }
                    }
                }
            }
        }

        void ProcessEscapePacket(EscapePacket pkt)
        {
            try
            {
                World world = RealmManager.GetWorld(Player.Owner.Id);
                if (world.Id == World.NEXUS_ID)
                {
                    SendPacket(new TextPacket()
                    {
                        Stars = -1,
                        BubbleTime = 0,
                        Name = "",
                        Text = "You are already at the Nexus!"
                    });
                    return;
                }
                Reconnect(new ReconnectPacket()
                {
                    Host = "",
                    Port = 2050,
                    GameId = World.NEXUS_ID,
                    Name = "Nexus",
                    Key = Empty<byte>.Array,
                });
            }
            catch
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Out.WriteLine("Error while sending EscapePacket, Check ClientProcessor.cs");
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        //Following must execute, network loop will discard disconnected client, so logic loop
        void DisconnectFromRealm()
        {
            RealmManager.Logic.AddPendingAction(t =>
            {
                if (Player != null)
                    Player.SaveToCharacter();
                Save();
                RealmManager.Disconnect(this);
            }, PendingPriority.Destruction);
        }

        public void Reconnect(ReconnectPacket pkt)
        {
            RealmManager.Logic.AddPendingAction(t =>
            {
                if (Player != null)
                    Player.SaveToCharacter();
                Save();
                RealmManager.Disconnect(this);
                SendPacket(pkt);
            }, PendingPriority.Destruction);
        }

        public bool CheckAccountInUse(int accId)
        {
            try
            {
                int count = 0;
                for (int i = 0; i < RealmManager.Worlds.Values.Count; i++)
                {
                    World w = RealmManager.GetWorld(i);
                    foreach (var plr in w.Players.Values)
                    {
                        if (plr.AccountId == accId)
                        {
                            return true;
                        }
                        else
                        {
                            count = count + 1;
                        }
                    }
                    if (count == w.Players.Values.ToArray().Length)
                    {
                        return false;
                    }
                }
                return true;
            }
            catch
            {
                Console.WriteLine("Error checking if account " + accId + " is in use, check ClientProcessor.cs");
                return false;
            }
        }

        public void HandleUpdateAck() //each time the server sends an UpdatePacket, the client sends back an UpdateAckPacket: if they're out of sync, we can disconnect the client
        {
            /*this.UpdateAckCount++;
            if ((this.Player.UpdateCount - (int)this.UpdateAckCount) > 5)
            {
                Console.WriteLine("Possible custom client (UpdateAck count lesser than UpdateCount): " + this.Account.Name);
            }
            if ((this.Player.UpdateCount - (int)this.UpdateAckCount) > 10)
            {
                Console.WriteLine("Probable custom client (UpdateAck count lesser than UpdateCount): " + this.Account.Name+", disconnecting it");
                psr.SendPacket(new svrPackets.FailurePacket() { Message = "Probable hacked client detected, disconnecting..." });
                psr.Disconnect();
            }*/
        }
    }
}
