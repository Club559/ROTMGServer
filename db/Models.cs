using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Xml;
using System.Collections.Concurrent;

[Serializable, XmlRoot()]
public class Chars
{
    public Chars()
    {
        this._namespaces = new XmlSerializerNamespaces(new XmlQualifiedName[] {
                new XmlQualifiedName(string.Empty, "rotmg") 
            });
    }

    [XmlElement("Char")]
    public List<Char> Characters { get; set; }

    [XmlAttribute("nextCharId")]
    public int NextCharId { get; set; }
    [XmlAttribute("maxNumChars")]
    public int MaxNumChars { get; set; }

    public Account Account { get; set; }

    [XmlArray("News")]
    [XmlArrayItem("Item")]
    public List<NewsItem> News { get; set; }

    [XmlArray("Servers")]
    [XmlArrayItem("Server")]
    public List<ServerItem> Servers { get; set; }

    public double? Lat { get; set; }
    public double? Long { get; set; }

    [XmlNamespaceDeclarations]
    public XmlSerializerNamespaces Namespaces
    {
        get { return this._namespaces; }
    }
    private XmlSerializerNamespaces _namespaces;
}

[Serializable, XmlRoot()]
public class Pics
{
    public Pics()
    {
        this._namespaces = new XmlSerializerNamespaces(new XmlQualifiedName[] {
                new XmlQualifiedName(string.Empty, "rotmg")
            });
    }

    [XmlAttribute("offset")]
    public int SearchOffset { get; set; }

    [XmlArray()]
    [XmlArrayItem("Pic")]
    public List<Pic> Pictures { get; set; }

    [XmlNamespaceDeclarations]
    public XmlSerializerNamespaces Namespaces
    {
        get { return this._namespaces; }
    }
    private XmlSerializerNamespaces _namespaces;
}

[Serializable, XmlRoot()]
public class Account
{
    public Account()
    {
        this._namespaces = new XmlSerializerNamespaces(new XmlQualifiedName[] {
                new XmlQualifiedName(string.Empty, "rotmg") 
            });
    }

    public int AccountId { get; set; }
    public string Name { get; set; }

    [XmlElement("NameChosen")]
    public string _NameChosen { get; set; }
    [XmlIgnore]
    public bool NameChosen { get { return this._NameChosen != null; } set { this._NameChosen = value ? "True" : null; } }

    [XmlElement("Converted")]
    public string _Converted { get; set; }
    [XmlIgnore]
    public bool Converted { get { return this._Converted != null; } set { this._Converted = value ? "True" : null; } }

    [XmlElement("Admin")]
    public string _Admin { get; set; }
    [XmlIgnore]
    public bool Admin { get { return this._Admin != null; } set { this._Admin = value ? "True" : null; } }

    [XmlElement("Banned")]
    public string _Banned { get; set; }
    [XmlIgnore]
    public bool Banned { get { return this._Banned != null; } set { this._Banned = value ? "True" : null; } }

    [XmlElement("VerifiedEmail")]
    public string _VerifiedEmail { get; set; }
    [XmlIgnore]
    public bool VerifiedEmail { get { return this._VerifiedEmail != null; } set { this._VerifiedEmail = value ? "True" : null; } }

    [XmlElement("StarredAccounts")]
    public string _StarredAccounts { get; set; }
    [XmlIgnore]
    public List<int> Locked { get { return (this._StarredAccounts != null ? Utils.StringListToIntList(this._StarredAccounts.Split(',').ToList()) : new List<int>()); } set { this._StarredAccounts = string.Join(",", value); } }

    [XmlElement("IgnoredAccounts")]
    public string _IgnoredAccounts { get; set; }
    [XmlIgnore]
    public List<int> Ignored { get { return (this._IgnoredAccounts != null ? Utils.StringListToIntList(this._IgnoredAccounts.Split(',').ToList()) : new List<int>()); } set { this._IgnoredAccounts = string.Join(",", value); } }

    public int Rank;
    public int Credits { get; set; }
    public int NextCharSlotPrice { get; set; }
    public int? BeginnerPackageTimeLeft { get; set; }

    public VaultData Vault { get; set; }
    public Stats Stats { get; set; }
    public Guild Guild { get; set; }


    [XmlNamespaceDeclarations]
    public XmlSerializerNamespaces Namespaces
    {
        get { return this._namespaces; }
    }
    private XmlSerializerNamespaces _namespaces;
}
[Serializable, XmlRoot()]
public class VaultData
{
    [XmlElement("Chest")]
    public List<VaultChest> Chests { get; set; }
}
public class VaultChest
{
    [XmlIgnore]
    public int ChestId { get; set; }
    [XmlText]
    public string _Items { get; set; }
    [XmlIgnore]
    public int[] Items
    {
        get { return Utils.FromCommaSepString32(_Items); }
        set { _Items = Utils.GetCommaSepString(value); }
    }
}
[Serializable, XmlRoot()]
public class Stats
{
    public int BestCharFame { get; set; }
    public int TotalFame { get; set; }
    public int Fame { get; set; }
    [XmlElement("ClassStats")]
    public List<ClassStats> ClassStates { get; set; }
}
[Serializable, XmlRoot()]
public class Guild
{
    [XmlAttribute("id")]
    public int Id { get; set; }
    public string Name { get; set; }
    public int Rank { get; set; }
}
[Serializable, XmlRoot()]
public class GuildStruct
{
    [XmlAttribute("id")]
    public int Id { get; set; }
    public string Name { get; set; }
    public int Level { get; set; }
    public string[] Members { get; set; }
    public int GuildFame { get; set; }
    public int TotalGuildFame { get; set; }
}
[Serializable, XmlRoot()]
public class ClassStats
{
    [XmlAttribute("objectType")]
    public int ObjectType { get; set; }
    public int BestLevel { get; set; }
    public int BestFame { get; set; }
}

[Serializable, XmlRoot("Item")]
public class NewsItem
{
    public string Icon { get; set; }
    public string Title { get; set; }
    public string TagLine { get; set; }
    public string Link { get; set; }
    public int Date { get; set; }
}

[Serializable, XmlRoot("Server")]
public class ServerItem
{
    public string Name { get; set; }
    public string DNS { get; set; }
    public double Lat { get; set; }
    public double Long { get; set; }
    public double Usage { get; set; }
    public int RankRequired { get; set; }

    [XmlElement("AdminOnly")]
    string _AdminOnly { get; set; }
    [XmlIgnore]
    public bool AdminOnly { get { return this._AdminOnly != null; } set { this._AdminOnly = value ? "True" : null; } }
}

[Serializable, XmlRoot("Char")]
public class Char
{
    [XmlAttribute("id")]
    public int CharacterId { get; set; }
    public int ObjectType { get; set; }
    public int Level { get; set; }
    public int Exp { get; set; }
    public int CurrentFame { get; set; }
    [XmlElement("Equipment")]
    public string _Equipment { get; set; }
    [XmlIgnore]
    public short[] Equipment
    {
        get { return Utils.FromCommaSepString16(_Equipment); }
        set { _Equipment = Utils.GetCommaSepString(value); }
    }
    public int MaxHitPoints { get; set; }
    public int HitPoints { get; set; }
    public int MaxMagicPoints { get; set; }
    public int MagicPoints { get; set; }
    public int Attack { get; set; }
    public int Defense { get; set; }
    public int Speed { get; set; }
    public int Dexterity { get; set; }
    public int HpRegen { get; set; }
    public int MpRegen { get; set; }
    public int Tex1 { get; set; }
    public int Tex2 { get; set; }
    public string PCStats { get; set; }
    [XmlIgnore]
    public FameStats FameStats { get; set; }
    public bool Dead { get; set; }
    public int Pet { get; set; }
}

[Serializable, XmlRoot("Pic")]
public class Pic
{
    [XmlAttribute("id")]
    public string PictureId { get; set; }
    public string PicName { get; set; }
    public int DataType { get; set; }
    [XmlElement("Tags")]
    public string _Tags { get; set; }
    [XmlIgnore]
    public List<string> Tags
    {
        get { return _Tags.Split(',').ToList(); }
        set { _Tags = string.Join(",", value); }
    }
}