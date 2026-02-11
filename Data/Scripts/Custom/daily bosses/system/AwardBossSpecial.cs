using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Custom
{
    public static class BossLootSystem
    {
        #region Constants
        private const int MIN_CHANCE = 1;
        private const int MAX_CHANCE = 100;
        private const double MAGIC_RESIST_BONUS = 10.0;
        #endregion

        #region Item Pool Definitions
        private static readonly Type[] DrowBardItems = new Type[]
        {
            typeof(FeatheredHat), typeof(FloppyHat), typeof(StrawHat), typeof(SkullCap),
            typeof(BambooFlute), typeof(Drums), typeof(Tambourine), typeof(LapHarp),
            typeof(Lute), typeof(Trumpet), typeof(ShortPants), typeof(Crossbow),
            typeof(HeavyCrossbow), typeof(RepeatingCrossbow), typeof(Boots),
            typeof(FancyShirt), typeof(JewelryRing), typeof(JewelryBracelet),
            typeof(JewelryEarrings)
        };

        private static readonly Type[] DrowBlackGuardItems = new Type[]
        {
            typeof(RingmailArms), typeof(RingmailChest), typeof(RingmailGloves),
            typeof(RingmailLegs), typeof(RoyalGorget), typeof(ScaledChest),
            typeof(ScaledGloves), typeof(ScaledGorget), typeof(ScaledLegs),
            typeof(ScaledArms), typeof(PlateArms), typeof(PlateChest),
            typeof(PlateGloves), typeof(PlateGorget), typeof(PlateHelm),
            typeof(PlateLegs), typeof(Longsword), typeof(Broadsword),
            typeof(ElvenSpellblade), typeof(BronzeShield), typeof(HeaterShield),
            typeof(JeweledShield), typeof(MetalShield), typeof(RoyalShield),
            typeof(WoodenKiteShield), typeof(WoodenKiteShield), typeof(JewelryRing),
            typeof(JewelryBracelet)
        };

        private static readonly Type[] DrowMageItems = new Type[]
        {
            typeof(VagabondRobe), typeof(ShinobiRobe), typeof(ProphetRobe),
            typeof(ScholarRobe), typeof(Robe), typeof(AssassinRobe),
            typeof(FancyRobe), typeof(GildedRobe), typeof(OrnateRobe),
            typeof(MagistrateRobe), typeof(RoyalRobe), typeof(NecromancerRobe),
            typeof(SpiderRobe), typeof(VagabondRobe), typeof(ExquisiteRobe),
            typeof(ProphetRobe), typeof(ElegantRobe), typeof(FormalRobe),
            typeof(ArchmageRobe), typeof(PriestRobe), typeof(CultistRobe),
            typeof(GildedDarkRobe), typeof(GildedLightRobe), typeof(SageRobe),
            typeof(ScholarRobe), typeof(SorcererRobe), typeof(ClothCowl),
            typeof(ClothHood), typeof(FancyHood), typeof(WizardHood),
            typeof(HoodedMantle), typeof(WizardsHat), typeof(ThighBoots),
            typeof(QuarterStaff), typeof(WizardStaff), typeof(JewelryCirclet),
            typeof(JewelryNecklace), typeof(JewelryEarrings)
        };

        private static readonly Type[] DrowPriestessItems = new Type[]
        {
            typeof(FemaleLeatherChest), typeof(LeatherBustierArms), typeof(LeatherGloves),
            typeof(LeatherLegs), typeof(LeatherGorget), typeof(RingmailArms),
            typeof(RingmailChest), typeof(RingmailGloves), typeof(RingmailLegs),
            typeof(RoyalGorget), typeof(ScaledChest), typeof(ScaledGloves),
            typeof(ScaledGorget), typeof(ScaledLegs), typeof(ScaledArms),
            typeof(FemaleStuddedChest), typeof(StuddedBustierArms), typeof(StuddedGloves),
            typeof(StuddedGorget), typeof(StuddedSkirt), typeof(Whip),
            typeof(DiamondMace), typeof(WarMace), typeof(SpikedClub),
            typeof(Cloak), typeof(RoyalSkirt), typeof(RoyalLongSkirt),
            typeof(AssassinRobe), typeof(ChaosRobe), typeof(SpiderRobe),
            typeof(LeatherThighBoots), typeof(BronzeShield), typeof(ChaosShield),
            typeof(DarkShield), typeof(HeaterShield), typeof(JeweledShield),
            typeof(MetalShield), typeof(RoyalShield), typeof(WoodenKiteShield),
            typeof(BronzeShield), typeof(ChaosShield), typeof(DarkShield),
            typeof(HeaterShield), typeof(JeweledShield), typeof(MetalShield),
            typeof(JewelryRing), typeof(JewelryCirclet), typeof(JewelryBracelet),
            typeof(JewelryNecklace), typeof(JewelryEarrings)
        };

        private static readonly Type[] ScorchedItems = new Type[]
        {
            typeof(Longsword), typeof(Broadsword), typeof(Claymore), typeof(Kryss),
            typeof(RoyalSword), typeof(VikingSword), typeof(WarFork), typeof(Spear),
            typeof(Pike), typeof(BladedStaff), typeof(Bardiche), typeof(Halberd),
            typeof(Scythe), typeof(WarMace), typeof(WarHammer), typeof(SpikedClub),
            typeof(Maul), typeof(Mace), typeof(DiamondMace), typeof(Cleaver),
            typeof(Dagger), typeof(WarCleaver), typeof(HeavyCrossbow), typeof(RepeatingCrossbow),
            typeof(Crossbow), typeof(WarAxe), typeof(TwoHandedAxe), typeof(OrnateAxe),
            typeof(ExecutionersAxe), typeof(DoubleAxe)
        };

        private static readonly Type[] SkyKnightItems = new Type[]
        {
            typeof(RoyalSword), typeof(RoyalArms), typeof(RoyalGloves), typeof(RoyalGorget),
            typeof(RoyalShield), typeof(RoyalsLegs), typeof(RoyalCape), typeof(RoyalBoots),
            typeof(RoyalBoots), typeof(RoyalBoots), typeof(RoyalHelm)
        };

        private static readonly Type[] DeathKnightItems = new Type[]
        {
            typeof(RoyalSword), typeof(RoyalArms), typeof(RoyalGloves), typeof(RoyalGorget),
            typeof(RoyalShield), typeof(RoyalsLegs), typeof(RoyalCape), typeof(RoyalBoots),
            typeof(RoyalBoots), typeof(RoyalBoots), typeof(DreadHelm)
        };

        private static readonly Dictionary<string, Type[]> ItemPools = new Dictionary<string, Type[]>
        {
            { "DrowBard", DrowBardItems },
            { "DrowBlackGuard", DrowBlackGuardItems },
            { "DrowMage", DrowMageItems },
            { "DrowPriestess", DrowPriestessItems },
            { "scorched", ScorchedItems },
            { "skyknight", SkyKnightItems },
            { "deathknight", DeathKnightItems }
        };
        #endregion

        #region Enhancement Configuration
        private class EnhancementConfig
        {
            public int Hue { get; set; }
            public Action<Item> ApplyEnhancements { get; set; }
        }

        private static readonly Dictionary<string, EnhancementConfig> EnhancementConfigs = new Dictionary<string, EnhancementConfig>
        {
            { "scorched", new EnhancementConfig { Hue = 2931, ApplyEnhancements = ApplyScorchedEnhancements } },
            { "skyknight", new EnhancementConfig { Hue = 0x0672, ApplyEnhancements = ApplySkyKnightEnhancements } },
            { "deathknight", new EnhancementConfig { Hue = 0x0AA5, ApplyEnhancements = ApplyDeathKnightEnhancements } }
        };
        #endregion

        #region Public API
        public static void AwardBossSpecial(BaseCreature boss, List<Type> dropList, int chance)
        {
            if (!ValidateBossAndCorpse(boss) || !ValidateDropList(dropList))
                return;

            chance = ClampChance(chance);

            if (!RollChance(chance))
                return;

            Item item = CreateRandomItem(dropList);
            if (item != null)
            {
                boss.Corpse.DropItem(item);
            }
        }

        public static void AwardBossSpecial(BaseCreature boss, int chance, params Type[] dropList)
        {
            AwardBossSpecial(boss, new List<Type>(dropList), chance);
        }

        public static void BossEnchant(Mobile from, Container c, int power, int chance, int amount, string type)
        {
            if (!ValidateBossEnchantParams(from, c))
                return;

            chance = ClampChance(chance);

            if (!RollChance(chance))
                return;

            amount = Math.Max(1, amount);

            for (int i = 0; i < amount; i++)
            {
                CreateAndDropEnchantedItem(from, c, power, type);
            }
        }

        public static void ItemEnchant(Mobile from, int enchantLevel, Item item)
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
        #endregion

        #region Validation Helpers
        private static bool ValidateBossAndCorpse(BaseCreature boss)
        {
            return boss != null && boss.Corpse != null;
        }

        private static bool ValidateDropList(List<Type> dropList)
        {
            return dropList != null && dropList.Count > 0;
        }

        private static bool ValidateBossEnchantParams(Mobile from, Container c)
        {
            return from != null && c != null && !c.Deleted;
        }

        private static int ClampChance(int chance)
        {
            if (chance < MIN_CHANCE) return MIN_CHANCE;
            if (chance > MAX_CHANCE) return MAX_CHANCE;
            return chance;
        }

        private static bool RollChance(int chance)
        {
            return Utility.Random(100) < chance;
        }
        #endregion

        #region Item Creation
        private static Item CreateRandomItem(List<Type> itemTypes)
        {
            Type itemType = itemTypes[Utility.Random(itemTypes.Count)];
            return CreateItem(itemType);
        }

        private static Item CreateItem(Type itemType)
        {
            try
            {
                return (Item)Activator.CreateInstance(itemType);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error creating boss special item: " + ex.Message);
                return null;
            }
        }

        private static Item GenerateItemByType(string type)
        {
            Type[] pool;
            if (ItemPools.TryGetValue(type, out pool))
            {
                Type itemType = pool[Utility.Random(pool.Length)];
                return CreateItem(itemType);
            }

            Console.WriteLine("BossEnchant: Unknown type '" + type + "'");
            return null;
        }

        private static void CreateAndDropEnchantedItem(Mobile from, Container c, int power, string type)
        {
            Item item = GenerateItemByType(type);
            if (item == null)
                return;

            ItemEnchant(from, power, item);
            ApplyTypeSpecificEnhancements(item, type);
            c.DropItem(item);
        }

        private static void ApplyTypeSpecificEnhancements(Item item, string type)
        {
            EnhancementConfig config;
            if (EnhancementConfigs.TryGetValue(type, out config))
            {
                item.Hue = config.Hue;
                config.ApplyEnhancements(item);
            }
            else
            {
                // Default to Drow if no pattern was found
                item.Hue = Utility.RandomDrowHue();
                ApplyDrowEnhancements(item);
            }
        }
        #endregion

        #region Enhancement Methods
        private static void ApplySkyKnightEnhancements(Item item)
        {
            if (item == null || item.Deleted)
                return;

            if (item is BaseWeapon)
            {
                BaseWeapon weapon = (BaseWeapon)item;
                weapon.Attributes.Luck += 75;
                weapon.WeaponAttributes.HitDispel += 25;
                weapon.MinDamage += 4;
                weapon.MaxDamage += 4;
                weapon.WeaponAttributes.ResistPhysicalBonus += Utility.RandomMinMax(8, 12);
            }
            else if (item is BaseArmor)
            {
                BaseArmor armor = (BaseArmor)item;
                armor.Attributes.Luck += 100;
                armor.Attributes.DefendChance += 10;
                armor.Attributes.BonusHits += 10;
            }
            else if (item is BaseClothing)
            {
                BaseClothing clothing = (BaseClothing)item;
                clothing.Attributes.Luck += 100;
                clothing.Attributes.BonusStam += 10;
                clothing.Attributes.BonusDex += 5;
            }
        }

        private static void ApplyDeathKnightEnhancements(Item item)
        {
            if (item == null || item.Deleted)
                return;

            if (item is BaseWeapon)
            {
                BaseWeapon weapon = (BaseWeapon)item;
                weapon.Attributes.Luck += 75;
                weapon.WeaponAttributes.HitHarm += 25;
                weapon.MinDamage += 4;
                weapon.MaxDamage += 4;
                weapon.WeaponAttributes.ResistColdBonus += Utility.RandomMinMax(8, 12);
            }
            else if (item is BaseArmor)
            {
                BaseArmor armor = (BaseArmor)item;
                armor.Attributes.Luck += 100;
                armor.Attributes.AttackChance += 8;
                armor.Attributes.BonusHits += 10;
            }
            else if (item is BaseClothing)
            {
                BaseClothing clothing = (BaseClothing)item;
                clothing.Attributes.Luck += 100;
                clothing.Attributes.BonusStam += 10;
                clothing.Attributes.BonusDex += 5;
            }
        }

        private static void ApplyScorchedEnhancements(Item item)
        {
            if (item == null || item.Deleted)
                return;

            if (item is BaseWeapon)
            {
                BaseWeapon weapon = (BaseWeapon)item;
                weapon.Attributes.Luck += 25;
                weapon.AosElementDamages.Physical = 50;
                weapon.AosElementDamages.Fire = 50;
                weapon.AosElementDamages.Cold = 0;
                weapon.AosElementDamages.Poison = 0;
                weapon.AosElementDamages.Energy = 0;
                weapon.MinDamage += 3;
                weapon.MaxDamage += 3;
                weapon.WeaponAttributes.ResistFireBonus += Utility.RandomMinMax(8, 12);
            }
        }

        private static void ApplyDrowEnhancements(Item item)
        {
            if (item == null || item.Deleted)
                return;

            AosSkillBonuses skillBonuses = null;
            int luckBonus = 100;

            if (item is BaseArmor)
            {
                BaseArmor armor = (BaseArmor)item;
                armor.Attributes.Luck += luckBonus;
                skillBonuses = armor.SkillBonuses;
            }
            else if (item is BaseWeapon)
            {
                BaseWeapon weapon = (BaseWeapon)item;
                weapon.Attributes.Luck += luckBonus;
                skillBonuses = weapon.SkillBonuses;
            }
            else if (item is BaseTrinket)
            {
                BaseTrinket trinket = (BaseTrinket)item;
                trinket.Attributes.Luck += luckBonus;
                skillBonuses = trinket.SkillBonuses;
            }
            else if (item is BaseClothing)
            {
                BaseClothing clothing = (BaseClothing)item;
                clothing.Attributes.Luck += luckBonus;
                skillBonuses = clothing.SkillBonuses;
            }
            else if (item is BaseInstrument)
            {
                BaseInstrument instrument = (BaseInstrument)item;
                instrument.Attributes.Luck += luckBonus;
                skillBonuses = instrument.SkillBonuses;
            }

            if (skillBonuses != null)
            {
                AddMagicResistBonus(skillBonuses);
            }
        }

        private static void AddMagicResistBonus(AosSkillBonuses skillBonuses)
        {
            for (int i = 0; i < 5; i++)
            {
                SkillName skill;
                double bonus;
                skillBonuses.GetValues(i, out skill, out bonus);

                if (skill == SkillName.MagicResist)
                {
                    skillBonuses.SetValues(i, SkillName.MagicResist, bonus + MAGIC_RESIST_BONUS);
                    return;
                }
            }

            for (int i = 0; i < 5; i++)
            {
                SkillName skill;
                double bonus;
                skillBonuses.GetValues(i, out skill, out bonus);

                if (skill == SkillName.Alchemy && bonus == 0.0)
                {
                    skillBonuses.SetValues(i, SkillName.MagicResist, MAGIC_RESIST_BONUS);
                    return;
                }
            }
        }
        #endregion
    }
}