using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wServer.realm.entities;
using wServer.realm;

namespace wServer.logic.loot
{
    public interface ILootDef
    {
        void Populate(RealmManager manager, Enemy enemy, Tuple<Player, int> playerDat,
                      Random rand, IList<LootDef> lootDefs);
    }

    public class ItemLoot : ILootDef
    {
        string item;
        double probability;
        public ItemLoot(string item, double probability)
        {
            this.item = item;
            this.probability = probability;
        }

        public void Populate(RealmManager manager, Enemy enemy, Tuple<Player, int> playerDat,
                             Random rand, IList<LootDef> lootDefs)
        {
            if (playerDat != null) return;
            var dat = manager.GameData;
            lootDefs.Add(new LootDef(dat.Items[dat.IdToObjectType[item]], probability));
        }
    }

    public enum ItemType
    {
        Weapon,
        Ability,
        Armor,
        Ring,
        Potion
    }
    public class TierLoot : ILootDef
    {
        public static readonly int[] WeaponT = new int[] { 1, 2, 3, 8, 17, };
        public static readonly int[] AbilityT = new int[] { 4, 5, 11, 12, 13, 15, 16, 18, 19, 20, 21, 22, 23, };
        public static readonly int[] ArmorT = new int[] { 6, 7, 14, };
        public static readonly int[] RingT = new int[] { 9 };
        public static readonly int[] PotionT = new int[] { 10 };

        byte tier;
        int[] types;
        double probability;
        public TierLoot(byte tier, ItemType type, double probability)
        {
            this.tier = tier;
            switch (type)
            {
                case ItemType.Weapon:
                    types = WeaponT; break;
                case ItemType.Ability:
                    types = AbilityT; break;
                case ItemType.Armor:
                    types = ArmorT; break;
                case ItemType.Ring:
                    types = RingT; break;
                case ItemType.Potion:
                    types = PotionT; break;
                default:
                    throw new NotSupportedException(type.ToString());
            }
            this.probability = probability;
        }

        public void Populate(RealmManager manager, Enemy enemy, Tuple<Player, int> playerDat,
                             Random rand, IList<LootDef> lootDefs)
        {
            if (playerDat != null) return;
            Item[] candidates = manager.GameData.Items
                .Where(item => Array.IndexOf(types, item.Value.SlotType) != -1)
                .Where(item => item.Value.Tier == tier)
                .Select(item => item.Value)
                .ToArray();
            foreach (var i in candidates)
                lootDefs.Add(new LootDef(i, probability / candidates.Length));
        }
    }

    public class Threshold : ILootDef
    {
        double threshold;
        ILootDef[] children;
        public Threshold(double threshold, params ILootDef[] children)
        {
            this.threshold = threshold;
            this.children = children;
        }

        public void Populate(RealmManager manager, Enemy enemy, Tuple<Player, int> playerDat,
                             Random rand, IList<LootDef> lootDefs)
        {
            if (playerDat != null && playerDat.Item2 / (double)enemy.ObjectDesc.MaxHP >= threshold)
            {
                foreach (var i in children)
                    i.Populate(manager, enemy, null, rand, lootDefs);
            }
        }
    }
}
