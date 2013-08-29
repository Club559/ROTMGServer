using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using System.Resources;
using System.Text.RegularExpressions;

namespace Empire_World_Launcher
{
    public class Once
    {
        private static List<string> once = new List<string>();

        public static bool Check(string type)
        {
            if (!once.Contains(type))
            {
                once.Add(type);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
