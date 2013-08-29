using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wServer.realm
{
    public class EWorld
    {
        public EWorld()
        {
            AdminOnly = false;
            DisplayName = "EWorld";
        }

        public bool AdminOnly { get; set; }
        public string DisplayName { get; set; }
    }
}
