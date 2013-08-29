using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace dataserver
{
    public class Database : IDisposable
    {
        MySqlConnection con;
        public Database() //new schema named "empire_world" (without quotes): contains tables with data about the launcher news for example.
        {
            con = new MySqlConnection("Server=80.241.222.17;Database=rotmg;uid=empireworld;password=YTzxPS6WjuR3Lx3b;Allow Zero Datetime=true"); //later will read from a registry value maybe
            con.Open();
        }

        public void Dispose()
        {
            con.Dispose();
        }

        public MySqlCommand CreateQuery()
        {
            return con.CreateCommand();
        }

        public static int DateTimeToUnixTimestamp(DateTime dateTime)
        {
            return (int)(dateTime - new DateTime(1970, 1, 1).ToLocalTime()).TotalSeconds;
        }
    }
}
