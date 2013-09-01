using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using System.Net;
using Microsoft.CSharp;
using System.CodeDom.Compiler;

namespace db.data
{
    class Mods
    {
        static string ServerName = "ROTMGServer";
        static string ds = Path.DirectorySeparatorChar.ToString();
        static string BaseModDir = Environment.ExpandEnvironmentVariables("%USERPROFILE%" + ds + "RotMG Server Mods"); //Located in your user folder
        //For example: BaseModDir, for me, would be:                  C:\Users\Travoos\RotMG Server Mods
        //The folder for this server's mods specifically would be:    C:\Users\Travoos\RotMG Server Mods\ROTMGServer

        public static void InitMods(bool behaviors)
        {
            string mods = CheckFiles();

            FileStream modIdFs = File.OpenRead(mods + ds + "ModIds.data");
            using (var rdr = new BinaryReader(modIdFs))
            {
                try
                {
                    int count = rdr.ReadInt32();
                    for (int i = 0; i < count; i++)
                    {
                        string name = rdr.ReadString();
                        short type = rdr.ReadInt16();
                        XmlDatas.ItemIds.Add(name, type);
                        XmlDatas.UsedIds.Add(type);
                    }
                }
                catch { }
            }

            foreach (var dir in Directory.GetDirectories(mods))
            {
                foreach (var xml in Directory.GetFiles(dir, "*.xml"))
                {
                    FileStream fs = File.OpenRead(xml);
                    XmlDatas.ExtraXml.Add(XmlDatas.ProcessModXml(File.OpenRead(xml), dir));
                    fs.Close();
                }
            }

            File.WriteAllText(mods + ds + "ModIds.data", "");

            modIdFs = File.OpenWrite(mods + ds + "ModIds.data");
            using (var wtr = new BinaryWriter(modIdFs))
            {
                wtr.Write(XmlDatas.ItemIds.Count);
                foreach (var i in XmlDatas.ItemIds)
                {
                    wtr.Write(i.Key);
                    wtr.Write(i.Value);
                }
            }
            modIdFs.Close();
        }

        public static string CheckFiles()
        {
            string modDir = BaseModDir + ds + ServerName;
            if (!Directory.Exists(BaseModDir))
                Directory.CreateDirectory(BaseModDir);
            if (!Directory.Exists(modDir))
                Directory.CreateDirectory(modDir);
            if (!File.Exists(modDir + ds + "ModIds.data"))
                File.WriteAllText(modDir + ds + "ModIds.data", "");
            return modDir;
        }
    }
}
