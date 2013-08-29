using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wServer.realm.entities.player
{
    public class Combinations
    {
        public Dictionary<string[], Tuple<string, int>> combos = new Dictionary<string[], Tuple<string, int>>();
        public Dictionary<string[], Tuple<string, int, bool>> advCombos = new Dictionary<string[], Tuple<string, int, bool>>();

        public List<int> SlotList = new List<int>();

        public Tuple<string, int> Combo = new Tuple<string, int>("Short Sword", 0);

        public Combinations()
        {
            // ---- ADVANCED COMBOS ---- //
            AddAdvCombo("Robe of the Sphinx", 12000, "Robe of the Ancient Intellect", "Robe of the Star Mother", "Robe of the Grand Sorcerer");
            AddAdvCombo("Armor of the Nile", 12000, "Annihilation Armor", "Dominion Armor", "Acropolis Armor");
            AddAdvCombo("Pyramid Leather Armor", 12000, "Hydra Skin Armor", "Wyrmhide Armor", "Leviathan Armor");

            // ---- NORMAL COMBOS ---- //
            AddCombo("Anatis Staff", 1000, "Energy Staff", "Wand of the Bulwark");
            AddCombo("Chicken Leg of Doom", 1000, "Anatis Staff", "Dagger of Dire Hatred");
            AddCombo("Potion of Oryx", 10, "Potion of Dexterity", "Potion of Speed", "Potion of Wisdom", "Potion of Vitality", "Potion of Defense", "Potion of Attack", "Potion of Life", "Potion of Mana");
            AddCombo("Dagger of Dire Hatred", 200, "Dagger of Foul Malevolence", "Dagger of Sinister Deeds");
            AddCombo("Sword of Majesty", 200, "Sword of Acclaim", "Sword of Splendor");
            AddCombo("Wand of Retribution", 200, "Wand of Recompense", "Wand of Evocation");
            AddCombo("Staff of the Fundamental Core", 200, "Staff of the Vital Unity", "Staff of the Cosmic Whole");
            AddCombo("Thousand Shot", 1000, "Doom Bow", "Coral Bow");
            //Abilities (T7, T6)
            AddCombo("Shield of Death", 500, "Magma Shield", "Colossus Shield");
            AddCombo("Skull of the Undead King", 500, "Souleater Skull", "Bloodsucker Skull");
            AddCombo("Azure Trap", 500, "Mobstopper Trap", "Giantcatcher Trap");
            AddCombo("Cloak of the Phantom", 500, "Cloak of Dimensions", "Cloak of Ghostly Concealment");
            AddCombo("Quiver of Artemis", 500, "Bloodorc Quiver", "Quiver of Elvish Mastery");
            AddCombo("Spell of the Eternal Void", 500, "Space Deterioration Spell", "Elemental Detonation Spell");
            AddCombo("Tome of the Ancients", 500, "Tome of the Fountains", "Tome of Holy Guidance");
            AddCombo("Helm of the Leviathan General", 500, "Helm of the Holy Warrior", "Helm of the Great General");
            AddCombo("Fury Orb", 500, "1940s Orb", "Planefetter Orb");
            AddCombo("Bile Poison", 500, "Toothpaste Poison", "Baneserphent Poison");
            AddCombo("Seal of War", 500, "Seal of Mass Power", "Seal of the Blessed Champion");
            AddCombo("Prism of the Bloody Surprise", 500, "Prism of the Soul Stealer", "Prism of Apparitions");
            AddCombo("Scepter of Vorv", 500, "Scepter of the Grand Sorcerer", "Scepter of Storms");
        }

        public bool SetCombo(string[] items)
        {
            foreach (var i in combos)
            {
                if (i.Key.Length == items.Length)
                {
                    bool pass = true;
                    foreach (var e in items)
                    {
                        if (!i.Key.Contains(e))
                        {
                            pass = false;
                        }
                    }
                    if (pass)
                    {
                        Combo = i.Value;
                        return true;
                    }
                }
            }
            return false;
        }

        public bool SetComboAdv(string[] items, bool forgeAmmy)
        {
            foreach (var i in advCombos)
            {
                if (i.Key.Length == items.Length)
                {
                    bool pass = true;
                    foreach (var e in items)
                    {
                        if (!i.Key.Contains(e))
                        {
                            pass = false;
                        }
                        else
                        {
                            if (i.Value.Item3 && !forgeAmmy)
                                pass = false;
                        }
                    }
                    if (pass)
                    {
                        Combo = new Tuple<string, int>(i.Value.Item1, i.Value.Item2);
                        return true;
                    }
                }
            }
            return false;
        }

        public void AddCombo(string result, int price, params string[] items)
        {
            combos.Add(items, new Tuple<string, int>(result, price));
            advCombos.Add(items, new Tuple<string, int, bool>(result, price, false));
        }

        public void AddAdvCombo(string result, int price, params string[] items)
        {
            advCombos.Add(items, new Tuple<string, int, bool>(result, price, true));
        }
    }
}
