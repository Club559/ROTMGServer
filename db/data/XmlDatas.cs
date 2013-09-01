using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using System.Net;
using db.data;

public class XmlDatas
{
    const int XML_COUNT = 36;

    public static bool behaviors = true;

    static string ds = Path.DirectorySeparatorChar.ToString();
    
    static XmlDatas()
    {
        ReadXmls();
        Mods.InitMods(behaviors);
    }

    public static void DoSomething()
    { }

    public static void ReadXmls()
    {
        TypeToId = new Dictionary<short, string>();
        IdToType = new Dictionary<string, short>();
        IdToDungeon = new Dictionary<short, string>();
        KeyPrices = new Dictionary<short, int>();
        TypeToElement = new Dictionary<short, XElement>();
        TileDescs = new Dictionary<short, TileDesc>();
        ItemDescs = new Dictionary<short, Item>();
        ObjectDescs = new Dictionary<short, ObjectDesc>();
        PortalDescs = new Dictionary<short, PortalDesc>();
        DungeonDescs = new Dictionary<string, DungeonDesc>();

        ModTextures = new Dictionary<string, byte[]>();
        ItemIds = new Dictionary<string,short>();
        UsedIds = new List<short>();

        ItemPrices = new Dictionary<short, int>();
        ItemShops = new Dictionary<int, string>();

        Keys = new List<short>();

        Stream stream;
        for (int i = 0; i < XML_COUNT; i++)
        {
            stream = typeof(XmlDatas).Assembly.GetManifestResourceStream("db.data.dat" + i + ".xml");
            ProcessXml(stream);
        }

        stream = typeof(XmlDatas).Assembly.GetManifestResourceStream("db.data.item.xml");
        ProcessXml(stream);
        stream.Position = 0;
        using (StreamReader rdr = new StreamReader(stream))
            ExtraXml.Add(rdr.ReadToEnd());

        stream = typeof(XmlDatas).Assembly.GetManifestResourceStream("db.data.addition2.xml");
        ProcessXml(stream);
        stream.Position = 0;
        using (StreamReader rdr = new StreamReader(stream))
            ExtraXml.Add(rdr.ReadToEnd());

        stream = typeof(XmlDatas).Assembly.GetManifestResourceStream("db.data.addition.xml");
        ProcessXml(stream);
        stream.Position = 0;
        using (StreamReader rdr = new StreamReader(stream))
            ExtraXml.Add(rdr.ReadToEnd());
    }

    public static string ProcessModXml(Stream stream, string dir)
    {
        XElement root = XElement.Load(stream);
        foreach (var elem in root.Elements("Ground"))
        {
            short type = 0x3000;
            string id = elem.Attribute("id").Value;

            if (!ItemIds.ContainsKey(id))
                for (var i = 0x3001; i < 0xffff; i++)
                    if (!UsedIds.Contains((short)i))
                    {
                        ItemIds.Add(id, (short)i);
                        UsedIds.Add((short)i);
                    }
            type = ItemIds[id];

            elem.SetAttributeValue("type", type);

            TypeToId[type] = id;
            IdToType[id] = type;
            TypeToElement[type] = elem;

            TileDescs[type] = new TileDesc(elem);
        }
        foreach (var elem in root.Elements("Object"))
        {
            if (elem.Element("Class") == null) continue;
            string cls = elem.Element("Class").Value;
            short type = 0x4000;
            string id = elem.Attribute("id").Value;

            if (!ItemIds.ContainsKey(id))
                for (var i = 0x4001; i < 0xffff; i++)
                    if (!UsedIds.Contains((short)i))
                    {
                        ItemIds.Add(id, (short)i);
                        UsedIds.Add((short)i);
                        break;
                    }
            type = ItemIds[id];

            Console.Out.WriteLine("(" + new DirectoryInfo(dir).Name + ") Adding mod object: " + id + " (" + type.ToString() + ")");
            if (File.Exists(dir + ds + id + ".png"))
            {
                Console.Out.WriteLine("(" + new DirectoryInfo(dir).Name + ") Adding mod texture: " + id);

                if (elem.Element("RemoteTexture") != null)
                    elem.Element("RemoteTexture").Remove();
                if (elem.Element("Texture") != null)
                    elem.Element("Texture").Remove();

                XElement texElem = new XElement("RemoteTexture",
                    new XElement("Instance",
                        new XText("production")
                    ),
                    new XElement("Id",
                        new XText("mod:" + id)
                    )
                );

                elem.Add(texElem);

                try
                {
                    ModTextures.Add(id, File.ReadAllBytes(dir + ds + id + ".png"));
                }
                catch { Console.Out.WriteLine("(" + new DirectoryInfo(dir).Name + ") Error adding texture: " + id); }
            }

            elem.SetAttributeValue("type", type);

            TypeToId[type] = id;
            IdToType[id] = type;
            TypeToElement[type] = elem;

            if (cls == "Equipment" || cls == "Dye" || cls == "Pet")
            {
                ItemDescs[type] = new Item(elem);
                if (elem.Element("Shop") != null)
                {
                    XElement shop = elem.Element("Shop");
                    ItemShops[type] = shop.Element("Name").Value;
                    ItemPrices[type] = Utils.FromString(shop.Element("Price").Value);
                }
            }
            if (cls == "Character" || cls == "GameObject" || cls == "Wall" ||
                cls == "ConnectedWall" || cls == "CaveWall" || cls == "Portal")
                ObjectDescs[type] = new ObjectDesc(elem);
            if (cls == "Portal")
            {
                try
                {
                    PortalDescs[type] = new PortalDesc(elem);
                }
                catch (Exception e)
                { 
                    Console.WriteLine("Error for portal: " + type + " id: " + id);
                    /*3392,1792,1795,1796,1805,1806,1810,1825 -- no location, assume nexus?* 
*  Tomb Portal of Cowardice,  Dungeon Portal,  Portal of Cowardice,  Realm Portal,  Glowing Portal of Cowardice,  Glowing Realm Portal,  Nexus Portal,  Locked Wine Cellar Portal*/
                }
            }

            XElement key = elem.Element("Key");
            if (key != null)
            {
                Keys.Add(type);
                KeyPrices[type] = Utils.FromString(key.Value);
            }
        }
        foreach (var elem in root.Elements("Dungeon"))
        {
            string name = elem.Attribute("name").Value;
            short portalid = (short)Utils.FromString(elem.Attribute("type").Value);

            IdToDungeon[portalid] = name;
            DungeonDescs[name] = new DungeonDesc(elem);
        }
        using (StringWriter sw = new StringWriter())
        {
            root.Save(sw);
            return sw.ToString();
        }
    }

    static void ProcessXml(Stream stream)
    {
        XElement root = XElement.Load(stream);
        foreach (var elem in root.Elements("Ground"))
        {
            short type = (short)Utils.FromString(elem.Attribute("type").Value);
            string id = elem.Attribute("id").Value;

            UsedIds.Add(type);

            TypeToId[type] = id;
            IdToType[id] = type;
            TypeToElement[type] = elem;

            TileDescs[type] = new TileDesc(elem);
        }
        foreach (var elem in root.Elements("Object"))
        {
            if (elem.Element("Class") == null) continue;
            string cls = elem.Element("Class").Value;
            short type = (short)Utils.FromString(elem.Attribute("type").Value);
            string id = elem.Attribute("id").Value;

            UsedIds.Add(type);

            TypeToId[type] = id;
            IdToType[id] = type;
            TypeToElement[type] = elem;

            if (cls == "Equipment" || cls == "Dye" || cls == "Pet")
            {
                ItemDescs[type] = new Item(elem);
                if (elem.Element("Shop") != null)
                {
                    XElement shop = elem.Element("Shop");
                    ItemShops[type] = shop.Element("Name").Value;
                    ItemPrices[type] = Utils.FromString(shop.Element("Price").Value);
                }
            }
            if (cls == "Character" || cls == "GameObject" || cls == "Wall" ||
                cls == "ConnectedWall" || cls == "CaveWall" || cls == "Portal")
                ObjectDescs[type] = new ObjectDesc(elem);
            if (cls == "Portal")
            {
                try
                {
                    PortalDescs[type] = new PortalDesc(elem);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error for portal: " + type + " id: " + id);
                    /*3392,1792,1795,1796,1805,1806,1810,1825 -- no location, assume nexus?* 
*  Tomb Portal of Cowardice,  Dungeon Portal,  Portal of Cowardice,  Realm Portal,  Glowing Portal of Cowardice,  Glowing Realm Portal,  Nexus Portal,  Locked Wine Cellar Portal*/
                }
            }

            XElement key = elem.Element("Key");
            if (key != null)
            {
                Keys.Add(type);
                KeyPrices[type] = Utils.FromString(key.Value);
            }
        }
        foreach (var elem in root.Elements("Dungeon"))
        {
            string name = elem.Attribute("name").Value;
            short portalid = (short)Utils.FromString(elem.Attribute("type").Value);

            IdToDungeon[portalid] = name;
            DungeonDescs[name] = new DungeonDesc(elem);
        }
    }


    public static Dictionary<short, string> TypeToId { get; private set; }
    public static Dictionary<string, short> IdToType { get; private set; }
    public static Dictionary<short, string> IdToDungeon { get; private set; }
    public static Dictionary<short, int> KeyPrices { get; private set; }
    public static Dictionary<short, XElement> TypeToElement { get; private set; }
    public static Dictionary<short, TileDesc> TileDescs { get; private set; }
    public static Dictionary<short, Item> ItemDescs { get; private set; }
    public static Dictionary<short, ObjectDesc> ObjectDescs { get; private set; }
    public static Dictionary<short, PortalDesc> PortalDescs { get; private set; }
    public static Dictionary<string, DungeonDesc> DungeonDescs { get; private set; }
    public static Dictionary<string, byte[]> ModTextures { get; private set; }

    public static List<short> UsedIds { get; private set; }
    public static Dictionary<string, short> ItemIds { get; private set; }

    public static Dictionary<short, int> ItemPrices;
    public static Dictionary<int, string> ItemShops;

    public static List<short> Keys;

    XElement addition;

    public static List<string> ExtraXml = new List<string>();
}