using System;
using System.Collections;
using System.Globalization;
using Server;
using Server.Gumps;
using Server.Items;
using Server.Misc;
using Server.Network;

namespace Server.Items
{
    public enum LegendCategory
    {
        OneHandedWeapons = 0,
        TwoHandedWeapons = 1,
        RangedWeapons    = 2,
        JewelryTrinkets  = 3,
        Armor            = 4,
        Clothing         = 5
    }

    public class LegendEntry
    {
        public string TypeName; 
        public string DisplayName; 
        public LegendCategory Category;

        public LegendEntry( string typeName, string displayName, LegendCategory category )
        {
            TypeName    = typeName;
            DisplayName = displayName;
            Category    = category;
        }
    }

    // =========================================================================
    public class LegendsBook : Item
    {
        public static readonly LegendEntry[] AllLegends = new LegendEntry[]
        {
            // ----- One-Handed Weapons -----
            new LegendEntry( "LevelAssassinSpike",      "Assassin Dagger",      LegendCategory.OneHandedWeapons ),
            new LegendEntry( "LevelElvenSpellblade",    "Assassin Sword",       LegendCategory.OneHandedWeapons ),
            new LegendEntry( "LevelBroadsword",         "Broadsword",           LegendCategory.OneHandedWeapons ),
            new LegendEntry( "LevelButcherKnife",       "Butcher Knife",        LegendCategory.OneHandedWeapons ),
            new LegendEntry( "LevelCleaver",            "Cleaver",              LegendCategory.OneHandedWeapons ),
            new LegendEntry( "LevelClub",               "Club",                 LegendCategory.OneHandedWeapons ),
            new LegendEntry( "LevelCutlass",            "Cutlass",              LegendCategory.OneHandedWeapons ),
            new LegendEntry( "LevelDagger",             "Dagger",               LegendCategory.OneHandedWeapons ),
            new LegendEntry( "LevelRadiantScimitar",    "Falchion",             LegendCategory.OneHandedWeapons ),
            new LegendEntry( "LevelHammers",            "Hammer",               LegendCategory.OneHandedWeapons ),
            new LegendEntry( "LevelHatchet",            "Hatchet",              LegendCategory.OneHandedWeapons ),
            new LegendEntry( "LevelKama",               "Kama",                 LegendCategory.OneHandedWeapons ),
            new LegendEntry( "LevelKatana",             "Katana",               LegendCategory.OneHandedWeapons ),
            new LegendEntry( "LevelKryss",              "Kryss",                LegendCategory.OneHandedWeapons ),
            new LegendEntry( "LevelLargeKnife",         "Large Knife",          LegendCategory.OneHandedWeapons ),
            new LegendEntry( "LevelLongsword",          "Longsword",            LegendCategory.OneHandedWeapons ),
            new LegendEntry( "LevelMace",               "Mace",                 LegendCategory.OneHandedWeapons ),
            new LegendEntry( "LevelElvenMachete",       "Machete",              LegendCategory.OneHandedWeapons ),
            new LegendEntry( "LevelPickaxe",            "Pickaxe",              LegendCategory.OneHandedWeapons ),
            new LegendEntry( "LevelShortSpear",         "Rapier",               LegendCategory.OneHandedWeapons ),
            new LegendEntry( "LevelRoyalSword",         "Royal Sword",          LegendCategory.OneHandedWeapons ),
            new LegendEntry( "LevelScepter",            "Scepter",              LegendCategory.OneHandedWeapons ),
            new LegendEntry( "LevelSceptre",            "Sceptre",              LegendCategory.OneHandedWeapons ),
            new LegendEntry( "LevelScimitar",           "Scimitar",             LegendCategory.OneHandedWeapons ),
        	new LegendEntry( "LevelShortSword",         "Short Sword",          LegendCategory.OneHandedWeapons ),
            new LegendEntry( "LevelSkinningKnife",      "Skinning Knife",       LegendCategory.OneHandedWeapons ),
            new LegendEntry( "LevelBoneHarvester",      "Sickle",               LegendCategory.OneHandedWeapons ),
            new LegendEntry( "LevelSpikedClub",         "Spiked Club",          LegendCategory.OneHandedWeapons ),
            new LegendEntry( "LevelThinLongsword",      "Sword",                LegendCategory.OneHandedWeapons ),
            new LegendEntry( "LevelTekagi",             "Tekagi",               LegendCategory.OneHandedWeapons ),
            new LegendEntry( "LevelWarAxe",             "War Axe",              LegendCategory.OneHandedWeapons ),
            new LegendEntry( "LevelRuneBlade",          "War Blades",           LegendCategory.OneHandedWeapons ),
            new LegendEntry( "LevelWarCleaver",         "War Cleaver",          LegendCategory.OneHandedWeapons ),
            new LegendEntry( "LevelLeafblade",          "War Dagger",           LegendCategory.OneHandedWeapons ),
            new LegendEntry( "LevelWarFork",            "War Fork",             LegendCategory.OneHandedWeapons ),
            new LegendEntry( "LevelWarMace",            "War Mace",             LegendCategory.OneHandedWeapons ),
            new LegendEntry( "LevelWhips",              "Whip",                 LegendCategory.OneHandedWeapons ),
            new LegendEntry( "LevelWakizashi",          "Wakizashi",            LegendCategory.OneHandedWeapons ),
			new LegendEntry( "LevelHammerPick",         "Hammer Pick",          LegendCategory.OneHandedWeapons ),
            new LegendEntry( "LevelDiamondMace",        "Battle Mace",          LegendCategory.OneHandedWeapons ),
            new LegendEntry( "LevelOrnateAxe",          "Barbarian Axe",        LegendCategory.OneHandedWeapons ),
            new LegendEntry( "LevelVikingSword",        "Barbarian Sword",      LegendCategory.OneHandedWeapons ),
            
            // ----- Two-Handed Weapons -----
			new LegendEntry( "LevelPugilistGloves",     "Pugilist Gloves",      LegendCategory.TwoHandedWeapons ),
			new LegendEntry( "LevelTessen",             "Tessen",               LegendCategory.TwoHandedWeapons ),
            new LegendEntry( "LevelAxe",                "Axe",                  LegendCategory.TwoHandedWeapons ),
            new LegendEntry( "LevelDaisho",             "Daisho",               LegendCategory.TwoHandedWeapons ),
            new LegendEntry( "LevelSai",                "Sai",                  LegendCategory.TwoHandedWeapons ),
		    new LegendEntry( "LevelShepherdsCrook",     "Shepherds Crook",      LegendCategory.TwoHandedWeapons ),
            new LegendEntry( "LevelBardiche",           "Bardiche",             LegendCategory.TwoHandedWeapons ),
            new LegendEntry( "LevelBattleAxe",         "Battle Axe",            LegendCategory.TwoHandedWeapons ),
            new LegendEntry( "LevelBladedStaff",        "Bladed Staff",         LegendCategory.TwoHandedWeapons ),
            new LegendEntry( "LevelBokuto",             "Bokuto",               LegendCategory.TwoHandedWeapons ),
            new LegendEntry( "LevelClaymore",           "Claymore",             LegendCategory.TwoHandedWeapons ),
            new LegendEntry( "LevelCrescentBlade",      "Crescent Blade",       LegendCategory.TwoHandedWeapons ),
            new LegendEntry( "LevelDoubleAxe",          "Double Axe",           LegendCategory.TwoHandedWeapons ),
            new LegendEntry( "LevelDoubleBladedStaff",  "Double Bladed Staff",  LegendCategory.TwoHandedWeapons ),
            new LegendEntry( "LevelWildStaff",          "Druid Staff",          LegendCategory.TwoHandedWeapons ),
            new LegendEntry( "LevelExecutionersAxe",    "Executioner Axe",      LegendCategory.TwoHandedWeapons ),
            new LegendEntry( "LevelGnarledStaff",       "Gnarled Staff",        LegendCategory.TwoHandedWeapons ),
            new LegendEntry( "LevelHalberd",            "Halberd",              LegendCategory.TwoHandedWeapons ),    
			new LegendEntry( "LevelHarpoon",            "Harpoon",              LegendCategory.TwoHandedWeapons ),
            new LegendEntry( "LevelLajatang",           "Lajatang",             LegendCategory.TwoHandedWeapons ),
            new LegendEntry( "LevelLance",              "Lance",                LegendCategory.TwoHandedWeapons ),
            new LegendEntry( "LevelLargeBattleAxe",     "Large Battle Axe",     LegendCategory.TwoHandedWeapons ),
            new LegendEntry( "LevelMaul",               "Maul",                 LegendCategory.TwoHandedWeapons ),
            new LegendEntry( "LevelNoDachi",            "NoDachi",              LegendCategory.TwoHandedWeapons ),
            new LegendEntry( "LevelNunchaku",           "Nunchaku",             LegendCategory.TwoHandedWeapons ),
            new LegendEntry( "LevelPike",               "Pike",                 LegendCategory.TwoHandedWeapons ),
            new LegendEntry( "LevelQuarterStaff",       "Quarter Staff",        LegendCategory.TwoHandedWeapons ),
            new LegendEntry( "LevelScythe",             "Scythe",               LegendCategory.TwoHandedWeapons ),
            new LegendEntry( "LevelSpear",              "Spear",                LegendCategory.TwoHandedWeapons ),
            new LegendEntry( "LevelStave",              "Stave",                LegendCategory.TwoHandedWeapons ),
            new LegendEntry( "LevelTetsubo",            "Tetsubo",              LegendCategory.TwoHandedWeapons ),
            new LegendEntry( "LevelTribalSpear",        "Tribal Spear",         LegendCategory.TwoHandedWeapons ),
            new LegendEntry( "LevelPitchfork",          "Trident",              LegendCategory.TwoHandedWeapons ),
            new LegendEntry( "LevelTwoHandedAxe",       "Two Handed Axe",       LegendCategory.TwoHandedWeapons ),
            new LegendEntry( "LevelWarHammer",          "War Hammer",           LegendCategory.TwoHandedWeapons ),
            new LegendEntry( "LevelBlackStaff",         "Wizard Staff",         LegendCategory.TwoHandedWeapons ),

            // ----- Ranged Weapons -----
            new LegendEntry( "LevelBow",                    "Bow",               LegendCategory.RangedWeapons ),
            new LegendEntry( "LevelCompositeBow",           "Composite Bow",     LegendCategory.RangedWeapons ),
            new LegendEntry( "LevelCrossbow",               "Crossbow",          LegendCategory.RangedWeapons ),
            new LegendEntry( "LevelHeavyCrossbow",          "Heavy Crossbow",    LegendCategory.RangedWeapons ),
            new LegendEntry( "LevelRepeatingCrossbow",      "Repeating Crossbow",LegendCategory.RangedWeapons ),
            new LegendEntry( "LevelElvenCompositeLongbow",  "Woodland Longbow",  LegendCategory.RangedWeapons ),
            new LegendEntry( "LevelMagicalShortbow",        "Woodland Shortbow", LegendCategory.RangedWeapons ),
            new LegendEntry( "LevelYumi",                   "Yumi",              LegendCategory.RangedWeapons ),
			new LegendEntry( "LevelThrowingGloves",     "Throwing Gloves",       LegendCategory.RangedWeapons ),


            // ----- Jewelry & Trinkets -----
            new LegendEntry( "LevelCandle",             "Candle",               LegendCategory.JewelryTrinkets ),
            new LegendEntry( "LevelGoldBeadNecklace",   "Bead Necklace",        LegendCategory.JewelryTrinkets ),
            new LegendEntry( "LevelGoldBracelet",       "Gold Bracelet",        LegendCategory.JewelryTrinkets ),
            new LegendEntry( "LevelGoldEarrings",       "Gold Earrings",        LegendCategory.JewelryTrinkets ),
            new LegendEntry( "LevelGoldNecklace",       "Gold Amulet",          LegendCategory.JewelryTrinkets ),
            new LegendEntry( "LevelGoldRing",           "Gold Ring",            LegendCategory.JewelryTrinkets ),
            new LegendEntry( "LevelLantern",            "Lantern",              LegendCategory.JewelryTrinkets ),
            new LegendEntry( "LevelNecklace",           "Amulet",               LegendCategory.JewelryTrinkets ),
            new LegendEntry( "LevelSilverBeadNecklace", "Silver Bead Necklace", LegendCategory.JewelryTrinkets ),
            new LegendEntry( "LevelSilverBracelet",     "Silver Bracelet",      LegendCategory.JewelryTrinkets ),
            new LegendEntry( "LevelSilverEarrings",     "Silver Earrings",      LegendCategory.JewelryTrinkets ),
            new LegendEntry( "LevelSilverNecklace",     "Silver Amulet",        LegendCategory.JewelryTrinkets ),
            new LegendEntry( "LevelSilverRing",         "Silver Ring",          LegendCategory.JewelryTrinkets ),
            new LegendEntry( "LevelTalismanLeather",    "Trinket, Talisman",    LegendCategory.JewelryTrinkets ),
            new LegendEntry( "LevelTalismanHoly",       "Trinket, Symbol",      LegendCategory.JewelryTrinkets ),
            new LegendEntry( "LevelTalismanSnake",      "Trinket, Idol",        LegendCategory.JewelryTrinkets ),
            new LegendEntry( "LevelTalismanTotem",      "Trinket, Totem",       LegendCategory.JewelryTrinkets ),
            new LegendEntry( "LevelTorch",              "Torch",                LegendCategory.JewelryTrinkets ),

            // ----- Armor -----
            new LegendEntry( "LevelBascinet",               "Bascinet",                  LegendCategory.Armor ),
            new LegendEntry( "LevelBoneArms",               "Bone Arms",                 LegendCategory.Armor ),
            new LegendEntry( "LevelBoneChest",              "Bone Chest",                LegendCategory.Armor ),
            new LegendEntry( "LevelBoneGloves",             "Bone Gloves",               LegendCategory.Armor ),
            new LegendEntry( "LevelBoneHelm",               "Bone Helm",                 LegendCategory.Armor ),
            new LegendEntry( "LevelBoneLegs",               "Bone Legs",                 LegendCategory.Armor ),
            new LegendEntry( "LevelBuckler",                "Buckler",                   LegendCategory.Armor ),
            new LegendEntry( "LevelChainChest",             "Chain Chest",               LegendCategory.Armor ),
            new LegendEntry( "LevelChainCoif",              "Chain Coif",                LegendCategory.Armor ),
            new LegendEntry( "LevelChainHatsuburi",         "Chain Hatsuburi",           LegendCategory.Armor ),
            new LegendEntry( "LevelChainLegs",              "Chain Legs",                LegendCategory.Armor ),
            new LegendEntry( "LevelChaosShield",            "Chaos Shield",              LegendCategory.Armor ),
           
            new LegendEntry( "LevelCloseHelm",              "Close Helm",                LegendCategory.Armor ),
            new LegendEntry( "LevelDarkShield",             "Dark Shield",               LegendCategory.Armor ),
            new LegendEntry( "LevelDecorativePlateKabuto",  "Decorative Plate Kabuto",   LegendCategory.Armor ),
            new LegendEntry( "LevelDreadHelm",              "Dread Helm",                LegendCategory.Armor ),
            new LegendEntry( "LevelElvenShield",            "Elven Shield",              LegendCategory.Armor ),
            new LegendEntry( "LevelFemaleLeatherChest",     "Female Leather Chest",      LegendCategory.Armor ),
            new LegendEntry( "LevelFemalePlateChest",       "Female Plate Chest",        LegendCategory.Armor ),
            new LegendEntry( "LevelFemaleStuddedChest",     "Female Studded Chest",      LegendCategory.Armor ),
            new LegendEntry( "LevelGuardsmanShield",        "Guardsman Shield",          LegendCategory.Armor ),
            new LegendEntry( "LevelHeaterShield",           "Heater Shield",             LegendCategory.Armor ),
            new LegendEntry( "LevelHeavyPlateJingasa",      "Heavy Plate Jingasa",       LegendCategory.Armor ),
            new LegendEntry( "LevelHelmet",                 "Helmet",                    LegendCategory.Armor ),
            new LegendEntry( "LevelOrcHelm",                "Horned Helm",               LegendCategory.Armor ),
            new LegendEntry( "LevelJeweledShield",          "Jeweled Shield",            LegendCategory.Armor ),
            new LegendEntry( "LevelBronzeShield",           "Large Shield",              LegendCategory.Armor ),
            new LegendEntry( "LevelLeatherArms",            "Leather Arms",              LegendCategory.Armor ),
            new LegendEntry( "LevelLeatherBustierArms",     "Leather Bustier Arms",      LegendCategory.Armor ),
            new LegendEntry( "LevelLeatherCap",             "Leather Cap",               LegendCategory.Armor ),
            new LegendEntry( "LevelLeatherChest",           "Leather Chest",             LegendCategory.Armor ),
            new LegendEntry( "LevelLeatherCloak",           "Leather Cloak",             LegendCategory.Armor ),
            new LegendEntry( "LevelLeatherDo",              "Leather Do",                LegendCategory.Armor ),
            new LegendEntry( "LevelLeatherGloves",          "Leather Gloves",            LegendCategory.Armor ),
            new LegendEntry( "LevelLeatherGorget",          "Leather Gorget",            LegendCategory.Armor ),
            new LegendEntry( "LevelLeatherHaidate",         "Leather Haidate",           LegendCategory.Armor ),
            new LegendEntry( "LevelLeatherHiroSode",        "Leather HiroSode",          LegendCategory.Armor ),
            new LegendEntry( "LevelLeatherJingasa",         "Leather Jingasa",           LegendCategory.Armor ),
            new LegendEntry( "LevelLeatherLegs",            "Leather Legs",              LegendCategory.Armor ),
            new LegendEntry( "LevelLeatherMempo",           "Leather Mempo",             LegendCategory.Armor ),
            new LegendEntry( "LevelLeatherNinjaHood",       "Leather Ninja Hood",        LegendCategory.Armor ),
            new LegendEntry( "LevelLeatherNinjaJacket",     "Leather Ninja Jacket",      LegendCategory.Armor ),
            new LegendEntry( "LevelLeatherNinjaMitts",      "Leather Ninja Mitts",       LegendCategory.Armor ),
            new LegendEntry( "LevelLeatherNinjaPants",      "Leather Ninja Pants",       LegendCategory.Armor ),
            new LegendEntry( "LevelLeatherRobe",            "Leather Robe",              LegendCategory.Armor ),
            new LegendEntry( "LevelLeatherShorts",          "Leather Shorts",            LegendCategory.Armor ),
            new LegendEntry( "LevelLeatherSkirt",           "Leather Skirt",             LegendCategory.Armor ),
            new LegendEntry( "LevelLeatherSuneate",         "Leather Suneate",           LegendCategory.Armor ),
            new LegendEntry( "LevelLightPlateJingasa",      "Light Plate Jingasa",       LegendCategory.Armor ),
            new LegendEntry( "LevelMetalKiteShield",        "Metal Kite Shield",         LegendCategory.Armor ),
            new LegendEntry( "LevelMetalShield",            "Metal Shield",              LegendCategory.Armor ),
            new LegendEntry( "LevelNorseHelm",              "Norse Helm",                LegendCategory.Armor ),
            new LegendEntry( "LevelOniwabanBoots",          "Oniwaban Boots",            LegendCategory.Armor ),
            new LegendEntry( "LevelOniwabanGloves",         "Oniwaban Gloves",           LegendCategory.Armor ),
            new LegendEntry( "LevelOniwabanHood",           "Oniwaban Hood",             LegendCategory.Armor ),
            new LegendEntry( "LevelOniwabanLeggings",       "Oniwaban Leggings",         LegendCategory.Armor ),
            new LegendEntry( "LevelOniwabanTunic",          "Oniwaban Tunic",            LegendCategory.Armor ),
            new LegendEntry( "LevelOrderShield",            "Order Shield",              LegendCategory.Armor ),
            new LegendEntry( "LevelPlateArms",              "Plate Arms",                LegendCategory.Armor ),
            new LegendEntry( "LevelPlateBattleKabuto",      "Plate Battle Kabuto",       LegendCategory.Armor ),
            new LegendEntry( "LevelPlateChest",             "Plate Chest",               LegendCategory.Armor ),
            new LegendEntry( "LevelPlateDo",                "Plate Do",                  LegendCategory.Armor ),
            new LegendEntry( "LevelPlateGloves",            "Plate Gloves",              LegendCategory.Armor ),
            new LegendEntry( "LevelPlateGorget",            "Plate Gorget",              LegendCategory.Armor ),
            new LegendEntry( "LevelPlateHaidate",           "Plate Haidate",             LegendCategory.Armor ),
            new LegendEntry( "LevelPlateHatsuburi",         "Plate Hatsuburi",           LegendCategory.Armor ),
            new LegendEntry( "LevelPlateHelm",              "Plate Helm",                LegendCategory.Armor ),
            new LegendEntry( "LevelPlateHiroSode",          "Plate Hiro Sode",           LegendCategory.Armor ),
            new LegendEntry( "LevelPlateLegs",              "Plate Legs",                LegendCategory.Armor ),
            new LegendEntry( "LevelPlateMempo",             "Plate Mempo",               LegendCategory.Armor ),
            new LegendEntry( "LevelPlateSuneate",           "Plate Suneate",             LegendCategory.Armor ),
            new LegendEntry( "LevelRingmailArms",           "Ringmail Arms",             LegendCategory.Armor ),
            new LegendEntry( "LevelRingmailChest",          "Ringmail Chest",            LegendCategory.Armor ),
            new LegendEntry( "LevelRingmailGloves",         "Ringmail Gloves",           LegendCategory.Armor ),
            new LegendEntry( "LevelRingmailLegs",           "Ringmail Legs",             LegendCategory.Armor ),
            new LegendEntry( "LevelRoyalArms",              "Royal Arms",                LegendCategory.Armor ),
            new LegendEntry( "LevelRoyalBoots",             "Royal Boots",               LegendCategory.Armor ),
            new LegendEntry( "LevelRoyalChest",             "Royal Chest",               LegendCategory.Armor ),
            new LegendEntry( "LevelRoyalGloves",            "Royal Gloves",              LegendCategory.Armor ),
            new LegendEntry( "LevelRoyalGorget",            "Royal Gorget",              LegendCategory.Armor ),
            new LegendEntry( "LevelRoyalHelm",              "Royal Helm",                LegendCategory.Armor ),
            new LegendEntry( "LevelRoyalsLegs",             "Royal Legs",                LegendCategory.Armor ),
            new LegendEntry( "LevelRoyalShield",            "Royal Shield",              LegendCategory.Armor ),
            new LegendEntry( "LevelDragonArms",             "Scalemail Arms",            LegendCategory.Armor ),
            new LegendEntry( "LevelDragonGloves",           "Scalemail Gloves",          LegendCategory.Armor ),
            new LegendEntry( "LevelDragonHelm",             "Scalemail Helm",            LegendCategory.Armor ),
            new LegendEntry( "LevelDragonLegs",             "Scalemail Leggings",        LegendCategory.Armor ),
            new LegendEntry( "LevelScalemailShield",        "Scalemail Shield",          LegendCategory.Armor ),
            new LegendEntry( "LevelDragonChest",            "Scalemail Tunic",           LegendCategory.Armor ),
            new LegendEntry( "LevelShinobiCowl",            "Leather Shinobi Cowl",      LegendCategory.Armor ),
            new LegendEntry( "LevelShinobiHood",            "Leather Shinobi Hood",      LegendCategory.Armor ),
            new LegendEntry( "LevelShinobiMask",            "Leather Shinobi Mask",      LegendCategory.Armor ),
            new LegendEntry( "LevelShinobiRobe",            "Leather Shinobi Robe",      LegendCategory.Armor ),
            new LegendEntry( "LevelSmallPlateJingasa",      "Small Plate Jingasa",       LegendCategory.Armor ),
            new LegendEntry( "LevelStandardPlateKabuto",    "Standard Plate Kabuto",     LegendCategory.Armor ),
            new LegendEntry( "LevelStuddedArms",            "Studded Arms",              LegendCategory.Armor ),
            new LegendEntry( "LevelStuddedBustierArms",     "Studded Bustier Arms",      LegendCategory.Armor ),
            new LegendEntry( "LevelStuddedChest",           "Studded Chest",             LegendCategory.Armor ),
            new LegendEntry( "LevelStuddedDo",              "Studded Do",                LegendCategory.Armor ),
            new LegendEntry( "LevelStuddedGloves",          "Studded Gloves",            LegendCategory.Armor ),
            new LegendEntry( "LevelStuddedGorget",          "Studded Gorget",            LegendCategory.Armor ),
            new LegendEntry( "LevelStuddedHaidate",         "Studded Haidate",           LegendCategory.Armor ),
            new LegendEntry( "LevelStuddedHiroSode",        "Studded Hiro Sode",         LegendCategory.Armor ),
            new LegendEntry( "LevelStuddedLegs",            "Studded Legs",              LegendCategory.Armor ),
            new LegendEntry( "LevelStuddedMempo",           "Studded Mempo",             LegendCategory.Armor ),
            new LegendEntry( "LevelStuddedSuneate",         "Studded Suneate",           LegendCategory.Armor ),
            new LegendEntry( "LevelSunShield",              "Sun Shield",                LegendCategory.Armor ),
            new LegendEntry( "LevelVirtueShield",           "Virtue Shield",             LegendCategory.Armor ),
            new LegendEntry( "LevelWoodenKiteShield",       "Wooden Kite Shield",        LegendCategory.Armor ),
            new LegendEntry( "LevelWoodenPlateArms",        "Wooden Plate Arms",         LegendCategory.Armor ),
            new LegendEntry( "LevelWoodenPlateChest",       "Wooden Plate Chest",        LegendCategory.Armor ),
            new LegendEntry( "LevelWoodenPlateGloves",      "Wooden Plate Gloves",       LegendCategory.Armor ),
            new LegendEntry( "LevelWoodenPlateGorget",      "Wooden Plate Gorget",       LegendCategory.Armor ),
            new LegendEntry( "LevelWoodenPlateHelm",        "Wooden Plate Helm",         LegendCategory.Armor ),
            new LegendEntry( "LevelWoodenPlateLegs",        "Wooden Plate Legs",         LegendCategory.Armor ),
            new LegendEntry( "LevelWoodenShield",           "Wooden Shield",             LegendCategory.Armor ),
            new LegendEntry( "LevelChampionShield",         "Champion Shield",           LegendCategory.Armor ),
            new LegendEntry( "LevelCrestedShield",          "Crested Shield",            LegendCategory.Armor ),

            // ----- Clothing -----
			new LegendEntry( "LevelCirclet",                "Circlet",                   LegendCategory.Clothing ),
            new LegendEntry( "LevelBandana",            "Bandana",              LegendCategory.Clothing ),
            new LegendEntry( "LevelBearMask",           "Bear Mask",            LegendCategory.Clothing ),
            new LegendEntry( "LevelBelt",               "Belt",                 LegendCategory.Clothing ),
            new LegendEntry( "LevelBodySash",           "Body Sash",            LegendCategory.Clothing ),
            new LegendEntry( "LevelBonnet",             "Bonnet",               LegendCategory.Clothing ),
            new LegendEntry( "LevelBoots",              "Boots",                LegendCategory.Clothing ),
            new LegendEntry( "LevelCap",                "Cap",                  LegendCategory.Clothing ),
            new LegendEntry( "LevelCloak",              "Cloak",                LegendCategory.Clothing ),
            new LegendEntry( "LevelClothNinjaHood",     "Cloth Ninja Hood",     LegendCategory.Clothing ),
            new LegendEntry( "LevelClothNinjaJacket",   "Cloth Ninja Jacket",   LegendCategory.Clothing ),
            new LegendEntry( "LevelCowl",               "Cowl",                 LegendCategory.Clothing ),
            new LegendEntry( "LevelDeerMask",           "Deer Mask",            LegendCategory.Clothing ),
            new LegendEntry( "LevelDoublet",            "Doublet",              LegendCategory.Clothing ),
            new LegendEntry( "LevelElvenBoots",         "Fancy Boots",          LegendCategory.Clothing ),
            new LegendEntry( "LevelFancyDress",         "Fancy Dress",          LegendCategory.Clothing ),
            new LegendEntry( "LevelFancyShirt",         "Fancy Shirt",          LegendCategory.Clothing ),
            new LegendEntry( "LevelFeatheredHat",       "Feathered Hat",        LegendCategory.Clothing ),
            new LegendEntry( "LevelFemaleKimono",       "Female Kimono",        LegendCategory.Clothing ),
            new LegendEntry( "LevelFloppyHat",          "Floppy Hat",           LegendCategory.Clothing ),
            new LegendEntry( "LevelFormalShirt",        "Formal Shirt",         LegendCategory.Clothing ),
            new LegendEntry( "LevelFullApron",          "Full Apron",           LegendCategory.Clothing ),
            new LegendEntry( "LevelFurBoots",           "Fur Boots",            LegendCategory.Clothing ),
            new LegendEntry( "LevelFurCape",            "Fur Cape",             LegendCategory.Clothing ),
            new LegendEntry( "LevelFurSarong",          "Fur Sarong",           LegendCategory.Clothing ),
            new LegendEntry( "LevelGildedDress",        "Gilded Dress",         LegendCategory.Clothing ),
            new LegendEntry( "LevelHakama",             "Hakama",               LegendCategory.Clothing ),
            new LegendEntry( "LevelHakamaShita",        "Hakama Shita",         LegendCategory.Clothing ),
            new LegendEntry( "LevelHalfApron",          "Half Apron",           LegendCategory.Clothing ),
            new LegendEntry( "LevelHood",               "Hood",                 LegendCategory.Clothing ),
            new LegendEntry( "LevelHornedTribalMask",   "Horned Tribal Mask",   LegendCategory.Clothing ),
            new LegendEntry( "LevelJesterHat",          "Jester Hat",           LegendCategory.Clothing ),
            new LegendEntry( "LevelJesterSuit",         "Jester Suit",          LegendCategory.Clothing ),
            new LegendEntry( "LevelJinBaori",           "Jin Baori",            LegendCategory.Clothing ),
            new LegendEntry( "LevelKamishimo",          "Kamishimo",            LegendCategory.Clothing ),
            new LegendEntry( "LevelKasa",               "Kasa",                 LegendCategory.Clothing ),
            new LegendEntry( "LevelKilt",               "Kilt",                 LegendCategory.Clothing ),
            new LegendEntry( "LevelLoinCloth",          "Loin Cloth",           LegendCategory.Clothing ),
            new LegendEntry( "LevelLongPants",          "Long Pants",           LegendCategory.Clothing ),
            new LegendEntry( "LevelMaleKimono",         "Male Kimono",          LegendCategory.Clothing ),
            new LegendEntry( "LevelNinjaTabi",          "Ninja Tabi",           LegendCategory.Clothing ),
            new LegendEntry( "LevelObi",                "Obi",                  LegendCategory.Clothing ),
            new LegendEntry( "LevelPlainDress",         "Plain Dress",          LegendCategory.Clothing ),
            new LegendEntry( "LevelPirateHat",          "Pirate Hat",           LegendCategory.Clothing ),
            new LegendEntry( "LevelRobe",               "Robe",                 LegendCategory.Clothing ),
            new LegendEntry( "LevelRoyalCape",          "Royal Cape",           LegendCategory.Clothing ),
            new LegendEntry( "LevelSamuraiTabi",        "Samurai Tabi",         LegendCategory.Clothing ),
            new LegendEntry( "LevelSandals",            "Sandals",              LegendCategory.Clothing ),
            new LegendEntry( "LevelSash",               "Sash",                 LegendCategory.Clothing ),
            new LegendEntry( "LevelShirt",              "Shirt",                LegendCategory.Clothing ),
            new LegendEntry( "LevelShoes",              "Shoes",                LegendCategory.Clothing ),
            new LegendEntry( "LevelShortPants",         "Short Pants",          LegendCategory.Clothing ),
            new LegendEntry( "LevelSkirt",              "Skirt",                LegendCategory.Clothing ),
            new LegendEntry( "LevelSkullCap",           "Skull Cap",            LegendCategory.Clothing ),
            new LegendEntry( "LevelStrawHat",           "Straw Hat",            LegendCategory.Clothing ),
            new LegendEntry( "LevelSurcoat",            "Surcoat",              LegendCategory.Clothing ),
            new LegendEntry( "LevelTallStrawHat",       "Tall Straw Hat",       LegendCategory.Clothing ),
            new LegendEntry( "LevelTattsukeHakama",     "Tattsuke Hakama",      LegendCategory.Clothing ),
            new LegendEntry( "LevelThighBoots",         "Thigh Boots",          LegendCategory.Clothing ),
            new LegendEntry( "LevelTribalMask",         "Tribal Mask",          LegendCategory.Clothing ),
            new LegendEntry( "LevelTricorneHat",        "Tricorne Hat",         LegendCategory.Clothing ),
            new LegendEntry( "LevelTunic",              "Tunic",                LegendCategory.Clothing ),
            new LegendEntry( "LevelWaraji",             "Waraji",               LegendCategory.Clothing ),
            new LegendEntry( "LevelWideBrimHat",        "Wide Brim Hat",        LegendCategory.Clothing ),
            new LegendEntry( "LevelWitchHat",           "Witch Hat",            LegendCategory.Clothing ),
            new LegendEntry( "LevelWizardsHat",         "Wizards Hat",          LegendCategory.Clothing ),
            new LegendEntry( "LevelWolfMask",           "Wolf Mask",            LegendCategory.Clothing ),
            new LegendEntry( "LevelHikingBoots",        "Hiking Boots",         LegendCategory.Clothing ),
        };

        public static ArrayList GetEntriesForCategory( LegendCategory cat )
        {
            ArrayList result = new ArrayList();
            for ( int i = 0; i < AllLegends.Length; i++ )
            {
                if ( AllLegends[i].Category == cat )
                    result.Add( AllLegends[i] );
            }
            return result;
        }

        public static string CategoryLabel( LegendCategory cat )
        {
            switch ( cat )
            {
                case LegendCategory.OneHandedWeapons: return "Armas de Uma Mão";
				case LegendCategory.TwoHandedWeapons: return "Armas de Duas Mãos";
				case LegendCategory.RangedWeapons:    return "Armas de Longo Alcance";
				case LegendCategory.JewelryTrinkets:  return "Joias & Trinkets";
				case LegendCategory.Armor:            return "Armaduras";
				case LegendCategory.Clothing:         return "Vestuário";
				default:                              return "Desconhecido";
            }
        }

        private static string FixDisplayName( string displayName )
        {
            if ( displayName == "Trinket, Symbol" ) return "Talisman";
            if ( displayName == "Trinket, Idol"   ) return "Talisman";
            if ( displayName == "Trinket, Totem"  ) return "Talisman";
            return displayName;
        }

        [Constructable]
        public LegendsBook() : base( 0x22C5 )
        {
            Weight  = 1.0;
            Movable = false;
            Hue     = 0xB93;
            Name    = "Artefatos Lendários";
        }

        public override void OnDoubleClick( Mobile from )
        {
            from.SendSound( 0x55 );
            from.CloseGump( typeof( LegendCategoryGump ) );
            from.CloseGump( typeof( LegendItemsGump ) );
            from.CloseGump( typeof( LegendConfirmGump ) );
            from.SendGump( new LegendCategoryGump( from, this ) );
        }

        public LegendsBook( Serial serial ) : base( serial ) { }

        public override void Serialize( GenericWriter writer )
        {
            base.Serialize( writer );
            writer.Write( (int)1 );
        }

        public override void Deserialize( GenericReader reader )
        {
            base.Deserialize( reader );
            int version = reader.ReadInt();
        }

        public static bool CanChoose( Mobile from )
        {
            int karma = from.Karma;
            if ( karma < 0 ) karma = -karma;
            return from.Fame >= 15000 && karma >= 15000 && from.TotalGold >= 10000;
        }

        public static string ArtyItemName( string item, Mobile from )
        {
            string ownerName = from.Name;
            string sAdjective = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(
                RandomThings.MagicItemAdj( "start",
                    Server.Misc.GetPlayerInfo.OrientalPlay( from ),
                    Server.Misc.GetPlayerInfo.EvilPlay( from ), 0 ) );

            if ( ownerName.EndsWith( "s" ) )
                ownerName = ownerName + "'";
            else
                ownerName = ownerName + "'s";

            if ( Utility.RandomMinMax( 0, 1 ) == 0 )
                return "o " + sAdjective + " " + item + " de " + from.Name;
            else
                return ownerName + " " + sAdjective + " " + item;
        }

        public class LegendCategoryGump : Gump
        {
            private LegendsBook m_Book;

            public LegendCategoryGump( Mobile from, LegendsBook book ) : base( 100, 100 )
            {
                m_Book = book;
                string color = "#81db9f";

                Closable  = true;
                Disposable = true;
                Dragable  = true;
                Resizable = false;

                AddPage( 0 );
                AddBackground( 0, 0, 340, 340, 9270 );

                AddHtml( 20, 15, 300, 24,
                    "<BODY><BASEFONT Color=" + color + "><CENTER>ARTEFATOS LENDÁRIOS</CENTER></BASEFONT></BODY>",
                    false, false );

                bool eligible = LegendsBook.CanChoose( from );
                string hint = eligible
                    ? "Escolha uma categoria abaixo:"
                    : "Requer 15k de Fama, 15k de Karma, 10k de Ouro";
                AddHtml( 20, 38, 300, 20,
                    "<BODY><BASEFONT Color=" + color + "><CENTER>" + hint + "</CENTER></BASEFONT></BODY>",
                    false, false );

            
                int btnX   = 60;
                int labelX = 100;
                int y      = 74;
                int step   = 36;

                AddCategoryRow( btnX, labelX, y, 1, "Armas de uma mão", color ); y += step;
                AddCategoryRow( btnX, labelX, y, 2, "Armas de duas mãos", color ); y += step;
                AddCategoryRow( btnX, labelX, y, 3, "Armas de alcance",     color ); y += step;
                AddCategoryRow( btnX, labelX, y, 4, "Jóias & Trinkets", color ); y += step;
                AddCategoryRow( btnX, labelX, y, 5, "Armadura",              color ); y += step;
                AddCategoryRow( btnX, labelX, y, 6, "Roupas",           color );

                AddButton( 148, 302, 4017, 4019, 0, GumpButtonType.Reply, 0 );
                AddHtml( 183, 304, 60, 20,
                    "<BODY><BASEFONT Color=" + color + ">Fechar</BASEFONT></BODY>",
                    false, false );
            }

            private void AddCategoryRow( int btnX, int labelX, int y, int buttonID, string label, string color )
            {
                AddButton( btnX, y, 4005, 4007, buttonID, GumpButtonType.Reply, 0 );
                AddHtml( labelX, y + 2, 210, 22,
                    "<BODY><BASEFONT Color=" + color + ">" + label + "</BASEFONT></BODY>",
                    false, false );
            }

            public override void OnResponse( NetState state, RelayInfo info )
            {
                Mobile from = state.Mobile;
                from.SendSound( 0x55 );

                LegendCategory cat;
                switch ( info.ButtonID )
                {
                    case 1: cat = LegendCategory.OneHandedWeapons; break;
                    case 2: cat = LegendCategory.TwoHandedWeapons; break;
                    case 3: cat = LegendCategory.RangedWeapons;    break;
                    case 4: cat = LegendCategory.JewelryTrinkets;  break;
                    case 5: cat = LegendCategory.Armor;            break;
                    case 6: cat = LegendCategory.Clothing;         break;
                    default: return;
                }

                from.CloseGump( typeof( LegendCategoryGump ) );
                from.SendGump( new LegendItemsGump( from, m_Book, cat, 0 ) );
            }
        }

        public class LegendItemsGump : Gump
        {
            private LegendsBook    m_Book;
            private LegendCategory m_Category;
            private ArrayList      m_Entries;
            private int            m_Page;

            private const int ItemsPerPage = 16;

            // Button IDs:
            //   0            = back to categories
            //   1            = previous page
            //   2            = next page
            //   1000 + index = select item

            public LegendItemsGump( Mobile from, LegendsBook book, LegendCategory cat, int page )
                : base( 100, 100 )
            {
                m_Book     = book;
                m_Category = cat;
                m_Entries  = LegendsBook.GetEntriesForCategory( cat );
                m_Page     = page;

                string color = "#81db9f";

                Closable  = true;
                Disposable = true;
                Dragable  = true;
                Resizable = false;

                AddPage( 0 );

                AddImage( 0, 0, 7005, 2964 );
                AddImage( 0, 0, 7006 );
                AddImage( 0, 0, 7024, 2736 );
                AddButton( 590, 48, 4017, 4017, 0, GumpButtonType.Reply, 0 );

                AddHtml( 77, 49, 259, 20,
                    "<BODY><BASEFONT Color=" + color + "><CENTER>" + LegendsBook.CategoryLabel( cat ).ToUpper() + "</CENTER></BASEFONT></BODY>",
                    false, false );

                int totalPages = ( m_Entries.Count + ItemsPerPage - 1 ) / ItemsPerPage;
                if ( totalPages < 1 ) totalPages = 1;

                if ( totalPages > 1 )
                {
                    AddButton( 75,  374, 4014, 4014, 1, GumpButtonType.Reply, 0 );
                    AddButton( 590, 375, 4005, 4005, 2, GumpButtonType.Reply, 0 );
                }

                AddButton( 300, 374, 4017, 4019, 0, GumpButtonType.Reply, 0 );
                AddHtml( 336, 376, 120, 18,
                    "<BODY><BASEFONT Color=" + color + ">Categorias</BASEFONT></BODY>",
                    false, false );

                int firstIndex = page * ItemsPerPage;
                int x, y, z, s;

                // ---- Left column buttons ----
                x = 115; s = 64; z = 34;
                y = s + z;
                for ( int slot = 0; slot < 8; slot++ )
                {
                    int idx = firstIndex + slot;
                    if ( idx < m_Entries.Count )
                        AddButton( x, y, 2447, 2447, 1000 + idx, GumpButtonType.Reply, 0 );
                    y += z;
                }

                // ---- Left column labels ----
                y = s - 3 + z;
                for ( int slot = 0; slot < 8; slot++ )
                {
                    int idx = firstIndex + slot;
                    string label = ( idx < m_Entries.Count )
                        ? ( (LegendEntry)m_Entries[idx] ).DisplayName : "";
                    AddHtml( x + 20, y, 155, 20,
                        "<BODY><BASEFONT Color=" + color + ">" + label + "</BASEFONT></BODY>",
                        false, false );
                    y += z;
                }

                // ---- Right column buttons ----
                x = 407;
                y = s + z;
                for ( int slot = 8; slot < 16; slot++ )
                {
                    int idx = firstIndex + slot;
                    if ( idx < m_Entries.Count )
                        AddButton( x, y, 2447, 2447, 1000 + idx, GumpButtonType.Reply, 0 );
                    y += z;
                }

                // ---- Right column labels ----
                y = s - 3 + z;
                for ( int slot = 8; slot < 16; slot++ )
                {
                    int idx = firstIndex + slot;
                    string label = ( idx < m_Entries.Count )
                        ? ( (LegendEntry)m_Entries[idx] ).DisplayName : "";
                    AddHtml( x + 20, y, 155, 20,
                        "<BODY><BASEFONT Color=" + color + ">" + label + "</BASEFONT></BODY>",
                        false, false );
                    y += z;
                }
            }

            public override void OnResponse( NetState state, RelayInfo info )
            {
                Mobile from = state.Mobile;
                from.SendSound( 0x55 );

                int totalPages = ( m_Entries.Count + ItemsPerPage - 1 ) / ItemsPerPage;
                if ( totalPages < 1 ) totalPages = 1;

                if ( info.ButtonID == 0 )
                {
                    from.CloseGump( typeof( LegendItemsGump ) );
                    from.SendGump( new LegendCategoryGump( from, m_Book ) );
                }
                else if ( info.ButtonID == 1 )
                {
                    int prev = m_Page - 1;
                    if ( prev < 0 ) prev = totalPages - 1;
                    from.CloseGump( typeof( LegendItemsGump ) );
                    from.SendGump( new LegendItemsGump( from, m_Book, m_Category, prev ) );
                }
                else if ( info.ButtonID == 2 )
                {
                    int next = m_Page + 1;
                    if ( next >= totalPages ) next = 0;
                    from.CloseGump( typeof( LegendItemsGump ) );
                    from.SendGump( new LegendItemsGump( from, m_Book, m_Category, next ) );
                }
                else if ( info.ButtonID >= 1000 )
                {
                    int idx = info.ButtonID - 1000;
                    if ( idx >= 0 && idx < m_Entries.Count )
                    {
                        LegendEntry entry = (LegendEntry)m_Entries[idx];
                        from.CloseGump( typeof( LegendItemsGump ) );
                        from.SendGump( new LegendConfirmGump( from, m_Book, entry, m_Category, m_Page ) );
                    }
                }
            }
        }

        public class LegendConfirmGump : Gump
        {
            private LegendsBook    m_Book;
            private LegendEntry    m_Entry;
            private LegendCategory m_ReturnCategory;
            private int            m_ReturnPage;

            public LegendConfirmGump( Mobile from, LegendsBook book, LegendEntry entry,
                                      LegendCategory returnCat, int returnPage )
                : base( 50, 50 )
            {
                m_Book           = book;
                m_Entry          = entry;
                m_ReturnCategory = returnCat;
                m_ReturnPage     = returnPage;

                string color = "#81db9f";
                bool eligible = LegendsBook.CanChoose( from );

                Closable  = true;
                Disposable = true;
                Dragable  = true;
                Resizable = false;

                AddBackground( 0, 0, 440, 200, 9270 );

                AddHtml( 20, 15, 400, 20,
                    "<BODY><BASEFONT Color=" + color + ">Invocar um artefato lendário:</BASEFONT></BODY>",
                    false, false );
                AddHtml( 20, 38, 400, 20,
                    "<BODY><BASEFONT Color=" + color + ">" + entry.DisplayName + "</BASEFONT></BODY>",
                    false, false );

                if ( eligible )
                {
                    AddHtml( 20, 68, 400, 20,
                        "<BODY><BASEFONT Color=" + color + ">Isso custará 10.000 de ouro e reiniciará sua Fama e Karma.</BASEFONT></BODY>",
                        false, false );
                    AddButton( 80,  155, 4005, 4007, 1, GumpButtonType.Reply, 0 );
                    AddHtml( 115, 157, 60, 20,
                        "<BODY><BASEFONT Color=" + color + ">Confirmar</BASEFONT></BODY>",
                        false, false );
                }
                else
                {
                    AddHtml( 20, 68, 400, 40,
                        "<BODY><BASEFONT Color=" + color + ">Você não é lendário o suficiente. Você precisa de 15.000 de Fama, 15.000 de Karma e 10.000 de Ouro.</BASEFONT></BODY>",
                        false, false );
                }

                AddButton( 240, 155, 4017, 4019, 0, GumpButtonType.Reply, 0 );
                AddHtml( 275, 157, 60, 20,
                    "<BODY><BASEFONT Color=" + color + ">Voltar</BASEFONT></BODY>",
                    false, false );
            }

            public override void OnResponse( NetState state, RelayInfo info )
            {
                Mobile from = state.Mobile;
                from.SendSound( 0x55 );

                if ( info.ButtonID == 1 )
                {
                    if ( !LegendsBook.CanChoose( from ) )
                    {
                        from.SendMessage( "Você não é lendário o bastante para invocar o artefato." );
                        from.CloseGump( typeof( LegendConfirmGump ) );
                        from.SendGump( new LegendCategoryGump( from, m_Book ) );
                        return;
                    }

                    Container pack = from.Backpack;
                    if ( !pack.ConsumeTotal( typeof( Gold ), 10000 ) )
                    {
                        from.SendMessage( "Você não tem ouro suficiente para o tributo." );
                        from.CloseGump( typeof( LegendConfirmGump ) );
                        from.SendGump( new LegendCategoryGump( from, m_Book ) );
                        return;
                    }

                    string sName = LegendsBook.FixDisplayName( m_Entry.DisplayName );
                    string sArty = LegendsBook.ArtyItemName( sName, from );

                    Type itemType = ScriptCompiler.FindTypeByName( m_Entry.TypeName );
                    if ( itemType != null )
                    {
                        from.Fame  = 0;
                        from.Karma = 0;
                        Item reward = (Item)Activator.CreateInstance( itemType );
                        reward.Name = sArty;
                        from.AddToBackpack( reward );
                        LoggingFunctions.LogCreatedArtifact( from, sArty );
                        from.SendMessage( "Os deuses criaram um artefato lendário chamado " + sArty + "." );
                        from.FixedParticles( 0x3709, 10, 30, 5052, 0x480, 0, EffectLayer.LeftFoot );
                        from.PlaySound( 0x208 );
                    }

                    from.CloseGump( typeof( LegendConfirmGump ) );
                }
                else
                {
                    from.CloseGump( typeof( LegendConfirmGump ) );
                    from.SendGump( new LegendItemsGump( from, m_Book, m_ReturnCategory, m_ReturnPage ) );
                }
            }
        }
    }
}