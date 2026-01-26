using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Custom
{
    public static class BossLootSystem
    {
        public static void AwardBossSpecial(BaseCreature boss, List<Type> dropList, int chance)
        {
            if (boss == null || boss.Corpse == null)
                return;

            if (dropList == null || dropList.Count == 0)
                return;

            if (chance < 1)
                chance = 1;
                
            if (chance > 100)
                chance = 100;

            int roll = Utility.Random(1, 101);

            if (roll <= chance)
            {
                Type itemType = dropList[Utility.Random(dropList.Count)];

                try
                {
                    Item item = (Item)Activator.CreateInstance(itemType);

                    if (item != null)
                    {
                        boss.Corpse.DropItem(item);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error creating boss special item: " + ex.Message);
                }
            }
        }

        public static void BossEnchant(
            Mobile from,
            Container c,
            int power,
            int chance,
            int amount,
            string type
        )
        {
            if (from == null || c == null || c.Deleted)
                return;

            if (chance < 1)
                chance = 1;
            if (chance > 100)
                chance = 100;

            if (Utility.Random(100) >= chance)
                return;

            if (amount < 1)
                amount = 1;

            for (int i = 0; i < amount; i++)
            {
                Item item = GenerateItemByType(type);
                
                if (item != null)
                {
                    item.Hue = Utility.RandomDrowHue();
                    ItemEnchant(from, power, item);
                    /* when this system is expanded, add type check to cycle between different enchant types */
                    ApplyDrowEnhancements(item);
                    c.DropItem(item);
                }
            }
        }

        private static Item GenerateItemByType(string type)
        {
            Item item = null;

            switch (type)
            {
                case "DrowBard":
                    item = GenerateDrowBardItem();
                    break;
                case "DrowBlackGuard":
                    item = GenerateDrowBlackGuardItem();
                    break;
                case "DrowMage":
                    item = GenerateDrowMageItem();
                    break;
                case "DrowPriestess":
                    item = GenerateDrowPriestessItem();
                    break;
                default:
                    Console.WriteLine("BossEnchant: Unknown type '" + type + "'");
                    break;
            }

            return item;
        }

        private static Item GenerateDrowBardItem()
        {
            switch (Utility.Random(19))
            {
                case 0: return new FeatheredHat();
                case 1: return new FloppyHat();
                case 2: return new StrawHat();
                case 3: return new SkullCap();
                case 4: return new BambooFlute();
                case 5: return new Drums();
                case 6: return new Tambourine();
                case 7: return new LapHarp();
                case 8: return new Lute();
                case 9: return new Trumpet();
                case 10: return new ShortPants();
                case 11: return new Crossbow();
                case 12: return new HeavyCrossbow();
                case 13: return new RepeatingCrossbow();
                case 14: return new Boots();
                case 15: return new FancyShirt();
                case 16: return new JewelryRing();
                case 17: return new JewelryBracelet();
                case 18: return new JewelryEarrings();
                default: return null;
            }
        }

        private static Item GenerateDrowBlackGuardItem()
        {
            switch (Utility.Random(28))
            {
                case 0: return new RingmailArms();
                case 1: return new RingmailChest();
                case 2: return new RingmailGloves();
                case 3: return new RingmailLegs();
                case 4: return new RoyalGorget();
                case 5: return new ScaledChest();
                case 6: return new ScaledGloves();
                case 7: return new ScaledGorget();
                case 8: return new ScaledLegs();
                case 9: return new ScaledArms();
                case 10: return new PlateArms();
                case 11: return new PlateChest();
                case 12: return new PlateGloves();
                case 13: return new PlateGorget();
                case 14: return new PlateHelm();
                case 15: return new PlateLegs();
                case 16: return new Longsword();
                case 17: return new Broadsword();
                case 18: return new ElvenSpellblade();
                case 19: return new BronzeShield();
                case 20: return new HeaterShield();
                case 21: return new JeweledShield();
                case 22: return new MetalShield();
                case 23: return new RoyalShield();
                case 24: return new WoodenKiteShield();
                case 25: return new WoodenKiteShield();
                case 26: return new JewelryRing();
                case 27: return new JewelryBracelet();
                default: return null;
            }
        }

        private static Item GenerateDrowMageItem()
        {
            switch (Utility.Random(38))
            {
                case 0: return new VagabondRobe();
                case 1: return new ShinobiRobe();
                case 2: return new ProphetRobe();
                case 3: return new ScholarRobe();
                case 4: return new Robe();
                case 5: return new AssassinRobe();
                case 6: return new FancyRobe();
                case 7: return new GildedRobe();
                case 8: return new OrnateRobe();
                case 9: return new MagistrateRobe();
                case 10: return new RoyalRobe();
                case 11: return new NecromancerRobe();
                case 12: return new SpiderRobe();
                case 13: return new VagabondRobe();
                case 14: return new ExquisiteRobe();
                case 15: return new ProphetRobe();
                case 16: return new ElegantRobe();
                case 17: return new FormalRobe();
                case 18: return new ArchmageRobe();
                case 19: return new PriestRobe();
                case 20: return new CultistRobe();
                case 21: return new GildedDarkRobe();
                case 22: return new GildedLightRobe();
                case 23: return new SageRobe();
                case 24: return new ScholarRobe();
                case 25: return new SorcererRobe();
                case 26: return new ClothCowl();
                case 27: return new ClothHood();
                case 28: return new FancyHood();
                case 29: return new WizardHood();
                case 30: return new HoodedMantle();
                case 31: return new WizardsHat();
                case 32: return new ThighBoots();
                case 33: return new QuarterStaff();
                case 34: return new WizardStaff();
                case 35: return new JewelryCirclet();
                case 36: return new JewelryNecklace();
                case 37: return new JewelryEarrings();
                default: return null;
            }
        }

        private static Item GenerateDrowPriestessItem()
        {
            switch (Utility.Random(50))
            {
                case 0: return new FemaleLeatherChest();
                case 1: return new LeatherBustierArms();
                case 2: return new LeatherGloves();
                case 3: return new LeatherLegs();
                case 4: return new LeatherGorget();
                case 5: return new RingmailArms();
                case 6: return new RingmailChest();
                case 7: return new RingmailGloves();
                case 8: return new RingmailLegs();
                case 9: return new RoyalGorget();
                case 10: return new ScaledChest();
                case 11: return new ScaledGloves();
                case 12: return new ScaledGorget();
                case 13: return new ScaledLegs();
                case 14: return new ScaledArms();
                case 15: return new FemaleStuddedChest();
                case 16: return new StuddedBustierArms();
                case 17: return new StuddedGloves();
                case 18: return new StuddedGorget();
                case 19: return new StuddedSkirt();
                case 20: return new Whip();
                case 21: return new DiamondMace();
                case 22: return new WarMace();
                case 23: return new SpikedClub();
                case 24: return new Cloak();
                case 25: return new RoyalSkirt();
                case 26: return new RoyalLongSkirt();
                case 27: return new AssassinRobe();
                case 28: return new ChaosRobe();
                case 29: return new SpiderRobe();
                case 30: return new LeatherThighBoots();
                case 31: return new BronzeShield();
                case 32: return new ChaosShield();
                case 33: return new DarkShield();
                case 34: return new HeaterShield();
                case 35: return new JeweledShield();
                case 36: return new MetalShield();
                case 37: return new RoyalShield();
                case 38: return new WoodenKiteShield();
                case 39: return new BronzeShield();
                case 40: return new ChaosShield();
                case 41: return new DarkShield();
                case 42: return new HeaterShield();
                case 43: return new JeweledShield();
                case 44: return new MetalShield();
                case 45: return new JewelryRing();
                case 46: return new JewelryCirclet();
                case 47: return new JewelryBracelet();
                case 48: return new JewelryNecklace();
                case 49: return new JewelryEarrings();
                default: return null;
            }
        }

        private static void ApplyDrowEnhancements(Item item)
        {
            if (item == null || item.Deleted)
                return;

            if (item is BaseArmor)
            {
                BaseArmor armor = (BaseArmor)item;
                armor.Attributes.Luck += 100;
                AddMagicResistBonus(armor.SkillBonuses);
            }
            else if (item is BaseWeapon)
            {
                BaseWeapon weapon = (BaseWeapon)item;
                weapon.Attributes.Luck += 100;
                AddMagicResistBonus(weapon.SkillBonuses);
            }
            else if (item is BaseTrinket)
            {
                BaseTrinket jewel = (BaseTrinket)item;
                jewel.Attributes.Luck += 100;
                AddMagicResistBonus(jewel.SkillBonuses);
            }
            else if (item is BaseClothing)
            {
                BaseClothing clothing = (BaseClothing)item;
                clothing.Attributes.Luck += 100;
                AddMagicResistBonus(clothing.SkillBonuses);
            }
            else if (item is BaseInstrument)
            {
                BaseInstrument instrument = (BaseInstrument)item;
                instrument.Attributes.Luck += 100;
                AddMagicResistBonus(instrument.SkillBonuses);
            }
        }

        private static void AddMagicResistBonus(AosSkillBonuses skillBonuses)
        {
            SkillName skill0, skill1, skill2, skill3, skill4;
            double bonus0, bonus1, bonus2, bonus3, bonus4;

            skillBonuses.GetValues(0, out skill0, out bonus0);
            skillBonuses.GetValues(1, out skill1, out bonus1);
            skillBonuses.GetValues(2, out skill2, out bonus2);
            skillBonuses.GetValues(3, out skill3, out bonus3);
            skillBonuses.GetValues(4, out skill4, out bonus4);

            if (skill0 == SkillName.MagicResist)
            {
                skillBonuses.SetValues(0, SkillName.MagicResist, bonus0 + 10.0);
            }
            else if (skill1 == SkillName.MagicResist)
            {
                skillBonuses.SetValues(1, SkillName.MagicResist, bonus1 + 10.0);
            }
            else if (skill2 == SkillName.MagicResist)
            {
                skillBonuses.SetValues(2, SkillName.MagicResist, bonus2 + 10.0);
            }
            else if (skill3 == SkillName.MagicResist)
            {
                skillBonuses.SetValues(3, SkillName.MagicResist, bonus3 + 10.0);
            }
            else if (skill4 == SkillName.MagicResist)
            {
                skillBonuses.SetValues(4, SkillName.MagicResist, bonus4 + 10.0);
            }
            else
            {
                if (skill0 == SkillName.Alchemy && bonus0 == 0.0)
                {
                    skillBonuses.SetValues(0, SkillName.MagicResist, 10.0);
                }
                else if (skill1 == SkillName.Alchemy && bonus1 == 0.0)
                {
                    skillBonuses.SetValues(1, SkillName.MagicResist, 10.0);
                }
                else if (skill2 == SkillName.Alchemy && bonus2 == 0.0)
                {
                    skillBonuses.SetValues(2, SkillName.MagicResist, 10.0);
                }
                else if (skill3 == SkillName.Alchemy && bonus3 == 0.0)
                {
                    skillBonuses.SetValues(3, SkillName.MagicResist, 10.0);
                }
                else if (skill4 == SkillName.Alchemy && bonus4 == 0.0)
                {
                    skillBonuses.SetValues(4, SkillName.MagicResist, 10.0);
                }
            }
        }

        public static void ItemEnchant(
            Mobile from,
            int enchantLevel,
            Item item
        )
        {
            if (from == null || item == null || item.Deleted)
                return;

            try
            {
                LootPackEntry.Enchant(from, enchantLevel, item);
            }
            catch (Exception ex)
            {
                Console.WriteLine("BossEnchant error: " + ex.Message);
            }
        }


        public static void AwardBossSpecial(BaseCreature boss, int chance, params Type[] dropList)
        {
            AwardBossSpecial(boss, new List<Type>(dropList), chance);
        }
    }
}
