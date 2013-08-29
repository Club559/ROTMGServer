using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace db
{
    partial class Database
    {
        public string GetGuildName(int id)
        {
            var cmd = CreateQuery();
            cmd.CommandText = "SELECT name FROM guilds WHERE id=@gid;";
            cmd.Parameters.AddWithValue("@gid", id);
            using (var rdr = cmd.ExecuteReader())
            {
                if (!rdr.HasRows) return "";
                rdr.Read();
                return rdr.GetString("name");
            }
        }

        public int GetGuildId(string name)
        {
            var cmd = CreateQuery();
            cmd.CommandText = "SELECT id FROM guilds WHERE name=@name;";
            cmd.Parameters.AddWithValue("@name", name);
            using (var rdr = cmd.ExecuteReader())
            {
                if (!rdr.HasRows) return 0;
                rdr.Read();
                return rdr.GetInt32("id");
            }
        }

        public List<GuildStruct> GetGuilds()
        {
            var cmd = CreateQuery();
            cmd.CommandText = "SELECT * FROM guilds";
            List<GuildStruct> guilds = new List<GuildStruct>();
            using (var rdr = cmd.ExecuteReader())
            {
                if (!rdr.HasRows) return null;
                while (rdr.Read())
                {
                    guilds.Add(new GuildStruct()
                    {
                        Id = rdr.GetInt32("id"),
                        Name = rdr.GetString("name"),
                        Level = rdr.GetInt32("level"),
                        Members = rdr.GetString("members").Split(','),
                        GuildFame = rdr.GetInt32("guildFame"),
                        TotalGuildFame = rdr.GetInt32("totalGuildFame")
                    });
                }
            }
            return guilds;
        }

        public GuildStruct GetGuild(string name)
        {
            var cmd = CreateQuery();
            cmd.CommandText = "SELECT * FROM guilds WHERE name=@n";
            cmd.Parameters.AddWithValue("@n", name);
            GuildStruct guild = null;
            using (var rdr = cmd.ExecuteReader())
            {
                if (!rdr.HasRows) return null;
                rdr.Read();
                guild = new GuildStruct()
                {
                    Id = rdr.GetInt32("id"),
                    Name = rdr.GetString("name"),
                    Level = rdr.GetInt32("level"),
                    Members = rdr.GetString("members").Split(','),
                    GuildFame = rdr.GetInt32("guildFame"),
                    TotalGuildFame = rdr.GetInt32("totalGuildFame")
                };
            }
            return guild;
        }

        public GuildStruct GetGuild(int guildid)
        {
            var cmd = CreateQuery();
            cmd.CommandText = "SELECT * FROM guilds WHERE id=@gid";
            cmd.Parameters.AddWithValue("@gid", guildid);
            GuildStruct guild = null;
            using (var rdr = cmd.ExecuteReader())
            {
                if (!rdr.HasRows) return null;
                rdr.Read();
                guild = new GuildStruct()
                {
                    Id = rdr.GetInt32("id"),
                    Name = rdr.GetString("name"),
                    Level = rdr.GetInt32("level"),
                    Members = rdr.GetString("members").Split(','),
                    GuildFame = rdr.GetInt32("guildFame"),
                    TotalGuildFame = rdr.GetInt32("totalGuildFame")
                };
            }
            return guild;
        }

        public Guild CreateGuild(Account acc, string name)
        {
            var cmd = CreateQuery();
            cmd.CommandText = "INSERT INTO guilds (name, members, guildFame, totalGuildFame, level) VALUES (@name,@empty,0,0,0)";
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@empty", "");
            if (cmd.ExecuteNonQuery() == 0)
            {
                throw new Exception("Could not add guild to SQL database!");
            }
            int id = GetGuildId(name);

            cmd = CreateQuery();
            cmd.CommandText = "INSERT INTO boards (guildId, text) VALUES (@id,@empty)";
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@empty", "");
            if (cmd.ExecuteNonQuery() == 0)
            {
                throw new Exception("Could not add guild board to SQL database!");
            }

            return ChangeGuild(acc, GetGuildId(name), 40, false);
        }

        public Guild ChangeGuild(Account acc, int guildid, int rank, bool renounce)
        {
            Guild guild;
            if (renounce)
            {
                guild = new Guild()
                {
                    Name = "",
                    Id = 0,
                    Rank = 0
                };

                var cmd = CreateQuery();
                cmd.CommandText = "UPDATE accounts SET guild=0, guildRank=0 WHERE id=@aid";
                cmd.Parameters.AddWithValue("@aid", acc.AccountId);
                if (cmd.ExecuteNonQuery() == 0)
                {
                    throw new Exception("Could not change player's guild in the SQL!");
                }

                UpdateGuild(guildid);

                return guild;
            }
            else
            {
                guild = new Guild()
                {
                    Id = guildid,
                    Name = GetGuildName(guildid),
                    Rank = rank
                };
                if(guild.Name == "")
                {
                    throw new Exception("Guild not found!");
                }
                var cmd = CreateQuery();
                cmd.CommandText = "UPDATE accounts SET guild=@gid, guildRank=@gr WHERE id=@aid";
                cmd.Parameters.AddWithValue("@gid", guildid);
                cmd.Parameters.AddWithValue("@gr", rank);
                cmd.Parameters.AddWithValue("@aid", acc.AccountId);
                if (cmd.ExecuteNonQuery() == 0)
                {
                    throw new Exception("Could not change player's guild in the SQL!");
                }

                UpdateGuild(guildid);

                return guild;
            }
        }

        public Guild EditGuild(string name, int guildid, int rank, bool renounce)
        {
            Guild guild;
            if (renounce)
            {
                guild = new Guild()
                {
                    Name = "",
                    Id = 0,
                    Rank = 0
                };

                var cmd = CreateQuery();
                cmd.CommandText = "UPDATE accounts SET guild=0, guildRank=0 WHERE name=@name";
                cmd.Parameters.AddWithValue("@name", name);
                if (cmd.ExecuteNonQuery() == 0)
                {
                    throw new Exception("Player not found!");
                }

                UpdateGuild(guildid);

                return guild;
            }
            else
            {
                guild = new Guild()
                {
                    Id = guildid,
                    Name = GetGuildName(guildid),
                    Rank = rank
                };
                if (guild.Name == "")
                {
                    throw new Exception("Guild not found!");
                }
                var cmd = CreateQuery();
                cmd.CommandText = "UPDATE accounts SET guild=@gid, guildRank=@gr WHERE name=@name";
                cmd.Parameters.AddWithValue("@gid", guildid);
                cmd.Parameters.AddWithValue("@gr", rank);
                cmd.Parameters.AddWithValue("@name", name);
                if (cmd.ExecuteNonQuery() == 0)
                {
                    throw new Exception("Player not found!");
                }

                UpdateGuild(guildid);

                return guild;
            }
        }

        public void UpdateGuild(int id)
        {
            GuildStruct guild = GetGuild(id);
            if (guild == null)
                throw new Exception("Guild not found!");
            var cmd = CreateQuery();
            cmd.CommandText = "SELECT * FROM accounts WHERE guild=@gid";
            cmd.Parameters.AddWithValue("@gid", id);
            string members = "";
            using (var rdr = cmd.ExecuteReader())
            {
                while (rdr.Read())
                {
                    members = members + rdr.GetInt32("id").ToString() + ",";
                }
            }

            if (members != "")
            {
                cmd = CreateQuery();
                cmd.CommandText = "UPDATE guilds SET members=@mem WHERE id=@gid";
                cmd.Parameters.AddWithValue("@gid", id);
                cmd.Parameters.AddWithValue("@mem", members);
                if (cmd.ExecuteNonQuery() == 0)
                {
                    throw new Exception("Failed to edit members column!");
                }
            }
            else
            {
                cmd = CreateQuery();
                cmd.CommandText = "DELETE FROM guilds WHERE id=@gid";
                cmd.Parameters.AddWithValue("@gid", id);
                if (cmd.ExecuteNonQuery() == 0)
                {
                    throw new Exception("Failed to delete empty guild!");
                }
            }
        }

        public string GetGuildBoard(Account acc)
        {
            if (acc != null)
            {
                int gid = acc.Guild.Id;
                var cmd = CreateQuery();
                cmd.CommandText = "SELECT * FROM boards WHERE guildId=@gid";
                cmd.Parameters.AddWithValue("@gid", gid);
                using (var rdr = cmd.ExecuteReader())
                {
                    if (!rdr.HasRows) return "Board error!";
                    rdr.Read();
                    return rdr.GetString("text");
                }
            }
            else
                throw new Exception("Invalid account.");
        }

        public string SetGuildBoard(string text, Account acc)
        {
            if (acc != null)
            {
                int gid = acc.Guild.Id;
                var cmd = CreateQuery();
                cmd.CommandText = "UPDATE boards SET text=@txt WHERE guildId=@gid";
                cmd.Parameters.AddWithValue("@gid", gid);
                cmd.Parameters.AddWithValue("@txt", text);
                if (cmd.ExecuteNonQuery() == 0)
                {
                    return "Error";
                }
                return text;
            }
            else
                return "Error";
        }

        public string HTTPGetGuildMembers(int num, int offset, Account acc)
        {
            GuildStruct guild = GetGuild(acc.Guild.Id);
            string ret = "<Guild name=\"" + guild.Name + "\" id=\"" + guild.Id + "\"><TotalFame>" + guild.TotalGuildFame + "</TotalFame><CurrentFame>" + guild.GuildFame + "</CurrentFame><HallType>Guild Hall " + guild.Level.ToString() + "</HallType>";
            var cmd = CreateQuery();
            cmd.CommandText = "SELECT * FROM accounts WHERE guild = @gid";
            cmd.Parameters.AddWithValue("@gid", guild.Id);
            List<string> Founders = new List<string>();
            List<string> Leaders = new List<string>();
            List<string> Officers = new List<string>();
            List<string> Members = new List<string>();
            List<string> Initiates = new List<string>();
            using (var rdr = cmd.ExecuteReader())
            {
                int countLeft = num;
                int offsleft = offset;
                while (rdr.Read())
                {
                    if (offsleft == 0)
                    {
                        if (countLeft != 0)
                        {
                            string add = "<Member>";

                            add += "<Name>" + rdr.GetString("name") + "</Name>";
                            add += "<Rank>" + rdr.GetInt32("guildRank").ToString() + "</Rank>";
                            add += "<Fame>0</Fame>";

                            add += "</Member>";

                            switch (rdr.GetInt32("guildRank"))
                            {
                                case 40:
                                    Founders.Add(add); break;
                                case 30:
                                    Leaders.Add(add); break;
                                case 20:
                                    Officers.Add(add); break;
                                case 10:
                                    Members.Add(add); break;
                                case 0:
                                    Initiates.Add(add); break;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        offsleft--;
                    }
                    countLeft--;
                }
            }
            Members.AddRange(Initiates);
            Officers.AddRange(Members);
            Leaders.AddRange(Officers);
            Founders.AddRange(Leaders);
            foreach (string i in Founders)
            {
                ret += i;
            }
            ret += "</Guild>";
            return ret;
        }

        public int GetGuildFame(int id)
        {
            var cmd = CreateQuery();
            cmd.CommandText = "SELECT guildFame FROM guilds WHERE id=@id";
            cmd.Parameters.AddWithValue("@id", id);
            return int.Parse(cmd.ExecuteScalar().ToString());
        }

        public void DetractGuildFame(int id, int quantity)
        {
            var cmd = CreateQuery();
            cmd.CommandText = "UPDATE guilds SET guildFame=@fame WHERE id=@id";
            cmd.Parameters.AddWithValue("@fame", GetGuildFame(id) - quantity);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
        }

        public void ChangeGuildLevel(int id, int newLevel)
        {
            var cmd = CreateQuery();
            cmd.CommandText = "UPDATE guilds SET level=@level WHERE id=@id";
            cmd.Parameters.AddWithValue("@level", newLevel);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
        }

        public int GetGuildLevel(int id)
        {
            var cmd = CreateQuery();
            cmd.CommandText = "SELECT level FROM guilds WHERE id=@id";
            cmd.Parameters.AddWithValue("@id", id);
            return int.Parse(cmd.ExecuteScalar().ToString());
        }
    }
}
