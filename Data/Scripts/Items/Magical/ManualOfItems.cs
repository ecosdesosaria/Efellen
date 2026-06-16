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
    public enum RelicCategory
    {
        OneHandedWeapons = 0,
        TwoHandedWeapons = 1,
        RangedWeapons    = 2,
        JewelryTrinkets  = 3,
        Armor            = 4,
        Clothing         = 5
    }

    public class RelicEntry
    {
        public string TypeName;
        public string DisplayName;
        public RelicCategory Category;

        public RelicEntry( string typeName, string displayName, RelicCategory category )
        {
            TypeName    = typeName;
            DisplayName = displayName;
            Category    = category;
        }
    }

    public class ManualOfItems : Item
    {
        public static readonly RelicEntry[] AllRelics = new RelicEntry[]
        {
            // ----- One-Handed Weapons -----
            new RelicEntry( "GiftAssassinSpike",    "Assassin Dagger",   RelicCategory.OneHandedWeapons ),
            new RelicEntry( "GiftElvenSpellblade",  "Assassin Sword",    RelicCategory.OneHandedWeapons ),
            new RelicEntry( "GiftAxe",              "Axe",               RelicCategory.OneHandedWeapons ),
            new RelicEntry( "GiftBroadsword",       "Broadsword",        RelicCategory.OneHandedWeapons ),
            new RelicEntry( "GiftButcherKnife",     "Butcher Knife",     RelicCategory.OneHandedWeapons ),
            new RelicEntry( "GiftCleaver",          "Cleaver",           RelicCategory.OneHandedWeapons ),
            new RelicEntry( "GiftClub",             "Club",              RelicCategory.OneHandedWeapons ),
            new RelicEntry( "GiftCutlass",          "Cutlass",           RelicCategory.OneHandedWeapons ),
            new RelicEntry( "GiftDagger",           "Dagger",            RelicCategory.OneHandedWeapons ),
            new RelicEntry( "GiftDaisho",           "Daisho",            RelicCategory.OneHandedWeapons ),
            new RelicEntry( "GiftRadiantScimitar",  "Falchion",          RelicCategory.OneHandedWeapons ),
            new RelicEntry( "GiftHammers",          "Hammer",            RelicCategory.OneHandedWeapons ),
            new RelicEntry( "GiftHatchet",          "Hatchet",           RelicCategory.OneHandedWeapons ),
            new RelicEntry( "GiftKama",             "Kama",              RelicCategory.OneHandedWeapons ),
            new RelicEntry( "GiftKatana",           "Katana",            RelicCategory.OneHandedWeapons ),
            new RelicEntry( "GiftKryss",            "Kryss",             RelicCategory.OneHandedWeapons ),
            new RelicEntry( "GiftLargeKnife",       "Large Knife",       RelicCategory.OneHandedWeapons ),
            new RelicEntry( "GiftLongsword",        "Longsword",         RelicCategory.OneHandedWeapons ),
            new RelicEntry( "GiftMace",             "Mace",              RelicCategory.OneHandedWeapons ),
            new RelicEntry( "GiftElvenMachete",     "Machete",           RelicCategory.OneHandedWeapons ),
            new RelicEntry( "GiftPickaxe",          "Pickaxe",           RelicCategory.OneHandedWeapons ),
            new RelicEntry( "GiftPugilistGloves",   "Pugilist Gloves",   RelicCategory.OneHandedWeapons ),
            new RelicEntry( "GiftShortSpear",       "Rapier",            RelicCategory.OneHandedWeapons ),
            new RelicEntry( "GiftRoyalSword",       "Royal Sword",       RelicCategory.OneHandedWeapons ),
            new RelicEntry( "GiftSai",              "Sai",               RelicCategory.OneHandedWeapons ),
            new RelicEntry( "GiftScepter",          "Scepter",           RelicCategory.OneHandedWeapons ),
            new RelicEntry( "GiftSceptre",          "Sceptre",           RelicCategory.OneHandedWeapons ),
            new RelicEntry( "GiftScimitar",         "Scimitar",          RelicCategory.OneHandedWeapons ),
            new RelicEntry( "GiftShepherdsCrook",   "Shepherds Crook",   RelicCategory.OneHandedWeapons ),
            new RelicEntry( "GiftShortSword",       "Short Sword",       RelicCategory.OneHandedWeapons ),
            new RelicEntry( "GiftSkinningKnife",    "Skinning Knife",    RelicCategory.OneHandedWeapons ),
            new RelicEntry( "GiftBoneHarvester",    "Sickle",            RelicCategory.OneHandedWeapons ),
            new RelicEntry( "GiftSpikedClub",       "Spiked Club",       RelicCategory.OneHandedWeapons ),
            new RelicEntry( "GiftThinLongsword",    "Sword",             RelicCategory.OneHandedWeapons ),
            new RelicEntry( "GiftTekagi",           "Tekagi",            RelicCategory.OneHandedWeapons ),
            new RelicEntry( "GiftTessen",           "Tessen",            RelicCategory.OneHandedWeapons ),
            new RelicEntry( "GiftThrowingGloves",   "Throwing Gloves",   RelicCategory.OneHandedWeapons ),
            new RelicEntry( "GiftWarAxe",           "War Axe",           RelicCategory.OneHandedWeapons ),
            new RelicEntry( "GiftRuneBlade",        "War Blades",        RelicCategory.OneHandedWeapons ),
            new RelicEntry( "GiftWarCleaver",       "War Cleaver",       RelicCategory.OneHandedWeapons ),
            new RelicEntry( "GiftLeafblade",        "War Dagger",        RelicCategory.OneHandedWeapons ),
            new RelicEntry( "GiftWarFork",          "War Fork",          RelicCategory.OneHandedWeapons ),
            new RelicEntry( "GiftWarMace",          "War Mace",          RelicCategory.OneHandedWeapons ),
            new RelicEntry( "GiftWhips",            "Whip",              RelicCategory.OneHandedWeapons ),
            new RelicEntry( "GiftWakizashi",        "Wakizashi",         RelicCategory.OneHandedWeapons ),

            // ----- Two-Handed Weapons -----
            new RelicEntry( "GiftOrnateAxe",         "Barbarian Axe",       RelicCategory.TwoHandedWeapons ),
            new RelicEntry( "GiftVikingSword",        "Barbarian Sword",     RelicCategory.TwoHandedWeapons ),
            new RelicEntry( "GiftBardiche",           "Bardiche",            RelicCategory.TwoHandedWeapons ),
            new RelicEntry( "GiftBattleAxe",          "Battle Axe",          RelicCategory.TwoHandedWeapons ),
            new RelicEntry( "GiftDiamondMace",        "Battle Mace",         RelicCategory.TwoHandedWeapons ),
            new RelicEntry( "GiftBladedStaff",        "Bladed Staff",        RelicCategory.TwoHandedWeapons ),
            new RelicEntry( "GiftBokuto",             "Bokuto",              RelicCategory.TwoHandedWeapons ),
            new RelicEntry( "GiftClaymore",           "Claymore",            RelicCategory.TwoHandedWeapons ),
            new RelicEntry( "GiftCrescentBlade",      "Crescent Blade",      RelicCategory.TwoHandedWeapons ),
            new RelicEntry( "GiftDoubleAxe",          "Double Axe",          RelicCategory.TwoHandedWeapons ),
            new RelicEntry( "GiftDoubleBladedStaff",  "Double Bladed Staff", RelicCategory.TwoHandedWeapons ),
            new RelicEntry( "GiftWildStaff",          "Druid Staff",         RelicCategory.TwoHandedWeapons ),
            new RelicEntry( "GiftExecutionersAxe",    "Executioner Axe",     RelicCategory.TwoHandedWeapons ),
            new RelicEntry( "GiftGnarledStaff",       "Gnarled Staff",       RelicCategory.TwoHandedWeapons ),
            new RelicEntry( "GiftHalberd",            "Halberd",             RelicCategory.TwoHandedWeapons ),
            new RelicEntry( "GiftHammerPick",         "Hammer Pick",         RelicCategory.TwoHandedWeapons ),
            new RelicEntry( "GiftHarpoon",            "Harpoon",             RelicCategory.TwoHandedWeapons ),
            new RelicEntry( "GiftLajatang",           "Lajatang",            RelicCategory.TwoHandedWeapons ),
            new RelicEntry( "GiftLance",              "Lance",               RelicCategory.TwoHandedWeapons ),
            new RelicEntry( "GiftLargeBattleAxe",     "Large Battle Axe",    RelicCategory.TwoHandedWeapons ),
            new RelicEntry( "GiftMaul",               "Maul",                RelicCategory.TwoHandedWeapons ),
            new RelicEntry( "GiftNoDachi",            "NoDachi",             RelicCategory.TwoHandedWeapons ),
            new RelicEntry( "GiftNunchaku",           "Nunchaku",            RelicCategory.TwoHandedWeapons ),
            new RelicEntry( "GiftPike",               "Pike",                RelicCategory.TwoHandedWeapons ),
            new RelicEntry( "GiftQuarterStaff",       "Quarter Staff",       RelicCategory.TwoHandedWeapons ),
            new RelicEntry( "GiftScythe",             "Scythe",              RelicCategory.TwoHandedWeapons ),
            new RelicEntry( "GiftSpear",              "Spear",               RelicCategory.TwoHandedWeapons ),
            new RelicEntry( "GiftStave",              "Stave",               RelicCategory.TwoHandedWeapons ),
            new RelicEntry( "GiftTetsubo",            "Tetsubo",             RelicCategory.TwoHandedWeapons ),
            new RelicEntry( "GiftTribalSpear",        "Tribal Spear",        RelicCategory.TwoHandedWeapons ),
            new RelicEntry( "GiftPitchfork",          "Trident",             RelicCategory.TwoHandedWeapons ),
            new RelicEntry( "GiftTwoHandedAxe",       "Two Handed Axe",      RelicCategory.TwoHandedWeapons ),
            new RelicEntry( "GiftWarHammer",          "War Hammer",          RelicCategory.TwoHandedWeapons ),
            new RelicEntry( "GiftBlackStaff",         "Wizard Staff",        RelicCategory.TwoHandedWeapons ),

            // ----- Ranged Weapons -----
            new RelicEntry( "GiftBow",                   "Bow",                RelicCategory.RangedWeapons ),
            new RelicEntry( "GiftCompositeBow",          "Composite Bow",      RelicCategory.RangedWeapons ),
            new RelicEntry( "GiftCrossbow",              "Crossbow",           RelicCategory.RangedWeapons ),
            new RelicEntry( "GiftHeavyCrossbow",         "Heavy Crossbow",     RelicCategory.RangedWeapons ),
            new RelicEntry( "GiftRepeatingCrossbow",     "Repeating Crossbow", RelicCategory.RangedWeapons ),
            new RelicEntry( "GiftElvenCompositeLongbow", "Woodland Longbow",   RelicCategory.RangedWeapons ),
            new RelicEntry( "GiftMagicalShortbow",       "Woodland Shortbow",  RelicCategory.RangedWeapons ),
            new RelicEntry( "GiftYumi",                  "Yumi",               RelicCategory.RangedWeapons ),

            // ----- Jewelry & Trinkets -----
            new RelicEntry( "GiftCandle",             "Candle",               RelicCategory.JewelryTrinkets ),
            new RelicEntry( "GiftGoldBeadNecklace",   "Bead Necklace",        RelicCategory.JewelryTrinkets ),
            new RelicEntry( "GiftGoldBracelet",       "Gold Bracelet",        RelicCategory.JewelryTrinkets ),
            new RelicEntry( "GiftGoldEarrings",       "Gold Earrings",        RelicCategory.JewelryTrinkets ),
            new RelicEntry( "GiftGoldNecklace",       "Gold Amulet",          RelicCategory.JewelryTrinkets ),
            new RelicEntry( "GiftGoldRing",           "Gold Ring",            RelicCategory.JewelryTrinkets ),
            new RelicEntry( "GiftLantern",            "Lantern",              RelicCategory.JewelryTrinkets ),
            new RelicEntry( "GiftNecklace",           "Amulet",               RelicCategory.JewelryTrinkets ),
            new RelicEntry( "GiftSilverBeadNecklace", "Silver Bead Necklace", RelicCategory.JewelryTrinkets ),
            new RelicEntry( "GiftSilverBracelet",     "Silver Bracelet",      RelicCategory.JewelryTrinkets ),
            new RelicEntry( "GiftSilverEarrings",     "Silver Earrings",      RelicCategory.JewelryTrinkets ),
            new RelicEntry( "GiftSilverNecklace",     "Silver Amulet",        RelicCategory.JewelryTrinkets ),
            new RelicEntry( "GiftSilverRing",         "Silver Ring",          RelicCategory.JewelryTrinkets ),
            new RelicEntry( "GiftTalismanLeather",    "Trinket, Talisman",    RelicCategory.JewelryTrinkets ),
            new RelicEntry( "GiftTalismanHoly",       "Trinket, Symbol",      RelicCategory.JewelryTrinkets ),
            new RelicEntry( "GiftTalismanSnake",      "Trinket, Idol",        RelicCategory.JewelryTrinkets ),
            new RelicEntry( "GiftTalismanTotem",      "Trinket, Totem",       RelicCategory.JewelryTrinkets ),
            new RelicEntry( "GiftTorch",              "Torch",                RelicCategory.JewelryTrinkets ),

            // ----- Armor -----
            new RelicEntry( "GiftBascinet",              "Bascinet",                RelicCategory.Armor ),
            new RelicEntry( "GiftBoneArms",              "Bone Arms",               RelicCategory.Armor ),
            new RelicEntry( "GiftBoneChest",             "Bone Chest",              RelicCategory.Armor ),
            new RelicEntry( "GiftBoneGloves",            "Bone Gloves",             RelicCategory.Armor ),
            new RelicEntry( "GiftBoneHelm",              "Bone Helm",               RelicCategory.Armor ),
            new RelicEntry( "GiftBoneLegs",              "Bone Legs",               RelicCategory.Armor ),
            new RelicEntry( "GiftBuckler",               "Buckler",                 RelicCategory.Armor ),
            new RelicEntry( "GiftChainChest",            "Chain Chest",             RelicCategory.Armor ),
            new RelicEntry( "GiftChainCoif",             "Chain Coif",              RelicCategory.Armor ),
            new RelicEntry( "GiftChainHatsuburi",        "Chain Hatsuburi",         RelicCategory.Armor ),
            new RelicEntry( "GiftChainLegs",             "Chain Legs",              RelicCategory.Armor ),
            new RelicEntry( "GiftChaosShield",           "Chaos Shield",            RelicCategory.Armor ),
            new RelicEntry( "GiftCirclet",               "Circlet",                 RelicCategory.Armor ),
            new RelicEntry( "GiftCloseHelm",             "Close Helm",              RelicCategory.Armor ),
            new RelicEntry( "GiftDarkShield",            "Dark Shield",             RelicCategory.Armor ),
            new RelicEntry( "GiftDecorativePlateKabuto", "Decorative Plate Kabuto", RelicCategory.Armor ),
            new RelicEntry( "GiftDreadHelm",             "Dread Helm",              RelicCategory.Armor ),
            new RelicEntry( "GiftElvenShield",           "Elven Shield",            RelicCategory.Armor ),
            new RelicEntry( "GiftFemaleLeatherChest",    "Female Leather Chest",    RelicCategory.Armor ),
            new RelicEntry( "GiftFemalePlateChest",      "Female Plate Chest",      RelicCategory.Armor ),
            new RelicEntry( "GiftFemaleStuddedChest",    "Female Studded Chest",    RelicCategory.Armor ),
            new RelicEntry( "GiftGuardsmanShield",       "Guardsman Shield",        RelicCategory.Armor ),
            new RelicEntry( "GiftHeaterShield",          "Heater Shield",           RelicCategory.Armor ),
            new RelicEntry( "GiftHeavyPlateJingasa",     "Heavy Plate Jingasa",     RelicCategory.Armor ),
            new RelicEntry( "GiftHelmet",                "Helmet",                  RelicCategory.Armor ),
            new RelicEntry( "GiftOrcHelm",               "Horned Helm",             RelicCategory.Armor ),
            new RelicEntry( "GiftJeweledShield",         "Jeweled Shield",          RelicCategory.Armor ),
            new RelicEntry( "GiftBronzeShield",          "Large Shield",            RelicCategory.Armor ),
            new RelicEntry( "GiftLeatherArms",           "Leather Arms",            RelicCategory.Armor ),
            new RelicEntry( "GiftLeatherBustierArms",    "Leather Bustier Arms",    RelicCategory.Armor ),
            new RelicEntry( "GiftLeatherCap",            "Leather Cap",             RelicCategory.Armor ),
            new RelicEntry( "GiftLeatherChest",          "Leather Chest",           RelicCategory.Armor ),
            new RelicEntry( "GiftLeatherCloak",          "Leather Cloak",           RelicCategory.Armor ),
            new RelicEntry( "GiftLeatherDo",             "Leather Do",              RelicCategory.Armor ),
            new RelicEntry( "GiftLeatherGloves",         "Leather Gloves",          RelicCategory.Armor ),
            new RelicEntry( "GiftLeatherGorget",         "Leather Gorget",          RelicCategory.Armor ),
            new RelicEntry( "GiftLeatherHaidate",        "Leather Haidate",         RelicCategory.Armor ),
            new RelicEntry( "GiftLeatherHiroSode",       "Leather HiroSode",        RelicCategory.Armor ),
            new RelicEntry( "GiftLeatherJingasa",        "Leather Jingasa",         RelicCategory.Armor ),
            new RelicEntry( "GiftLeatherLegs",           "Leather Legs",            RelicCategory.Armor ),
            new RelicEntry( "GiftLeatherMempo",          "Leather Mempo",           RelicCategory.Armor ),
            new RelicEntry( "GiftLeatherNinjaHood",      "Leather Ninja Hood",      RelicCategory.Armor ),
            new RelicEntry( "GiftLeatherNinjaJacket",    "Leather Ninja Jacket",    RelicCategory.Armor ),
            new RelicEntry( "GiftLeatherNinjaMitts",     "Leather Ninja Mitts",     RelicCategory.Armor ),
            new RelicEntry( "GiftLeatherNinjaPants",     "Leather Ninja Pants",     RelicCategory.Armor ),
            new RelicEntry( "GiftLeatherRobe",           "Leather Robe",            RelicCategory.Armor ),
            new RelicEntry( "GiftLeatherShorts",         "Leather Shorts",          RelicCategory.Armor ),
            new RelicEntry( "GiftLeatherSkirt",          "Leather Skirt",           RelicCategory.Armor ),
            new RelicEntry( "GiftLeatherSuneate",        "Leather Suneate",         RelicCategory.Armor ),
            new RelicEntry( "GiftLightPlateJingasa",     "Light Plate Jingasa",     RelicCategory.Armor ),
            new RelicEntry( "GiftMetalKiteShield",       "Metal Kite Shield",       RelicCategory.Armor ),
            new RelicEntry( "GiftMetalShield",           "Metal Shield",            RelicCategory.Armor ),
            new RelicEntry( "GiftNorseHelm",             "Norse Helm",              RelicCategory.Armor ),
            new RelicEntry( "GiftOniwabanBoots",         "Oniwaban Boots",          RelicCategory.Armor ),
            new RelicEntry( "GiftOniwabanGloves",        "Oniwaban Gloves",         RelicCategory.Armor ),
            new RelicEntry( "GiftOniwabanHood",          "Oniwaban Hood",           RelicCategory.Armor ),
            new RelicEntry( "GiftOniwabanLeggings",      "Oniwaban Leggings",       RelicCategory.Armor ),
            new RelicEntry( "GiftOniwabanTunic",         "Oniwaban Tunic",          RelicCategory.Armor ),
            new RelicEntry( "GiftOrderShield",           "Order Shield",            RelicCategory.Armor ),
            new RelicEntry( "GiftPlateArms",             "Plate Arms",              RelicCategory.Armor ),
            new RelicEntry( "GiftPlateBattleKabuto",     "Plate Battle Kabuto",     RelicCategory.Armor ),
            new RelicEntry( "GiftPlateChest",            "Plate Chest",             RelicCategory.Armor ),
            new RelicEntry( "GiftPlateDo",               "Plate Do",                RelicCategory.Armor ),
            new RelicEntry( "GiftPlateGloves",           "Plate Gloves",            RelicCategory.Armor ),
            new RelicEntry( "GiftPlateGorget",           "Plate Gorget",            RelicCategory.Armor ),
            new RelicEntry( "GiftPlateHaidate",          "Plate Haidate",           RelicCategory.Armor ),
            new RelicEntry( "GiftPlateHatsuburi",        "Plate Hatsuburi",         RelicCategory.Armor ),
            new RelicEntry( "GiftPlateHelm",             "Plate Helm",              RelicCategory.Armor ),
            new RelicEntry( "GiftPlateHiroSode",         "Plate Hiro Sode",         RelicCategory.Armor ),
            new RelicEntry( "GiftPlateLegs",             "Plate Legs",              RelicCategory.Armor ),
            new RelicEntry( "GiftPlateMempo",            "Plate Mempo",             RelicCategory.Armor ),
            new RelicEntry( "GiftPlateSuneate",          "Plate Suneate",           RelicCategory.Armor ),
            new RelicEntry( "GiftRingmailArms",          "Ringmail Arms",           RelicCategory.Armor ),
            new RelicEntry( "GiftRingmailChest",         "Ringmail Chest",          RelicCategory.Armor ),
            new RelicEntry( "GiftRingmailGloves",        "Ringmail Gloves",         RelicCategory.Armor ),
            new RelicEntry( "GiftRingmailLegs",          "Ringmail Legs",           RelicCategory.Armor ),
            new RelicEntry( "GiftRoyalArms",             "Royal Arms",              RelicCategory.Armor ),
            new RelicEntry( "GiftRoyalBoots",            "Royal Boots",             RelicCategory.Armor ),
            new RelicEntry( "GiftRoyalChest",            "Royal Chest",             RelicCategory.Armor ),
            new RelicEntry( "GiftRoyalGloves",           "Royal Gloves",            RelicCategory.Armor ),
            new RelicEntry( "GiftRoyalGorget",           "Royal Gorget",            RelicCategory.Armor ),
            new RelicEntry( "GiftRoyalHelm",             "Royal Helm",              RelicCategory.Armor ),
            new RelicEntry( "GiftRoyalsLegs",            "Royal Legs",              RelicCategory.Armor ),
            new RelicEntry( "GiftRoyalShield",           "Royal Shield",            RelicCategory.Armor ),
            new RelicEntry( "GiftDragonArms",            "Scalemail Arms",          RelicCategory.Armor ),
            new RelicEntry( "GiftDragonGloves",          "Scalemail Gloves",        RelicCategory.Armor ),
            new RelicEntry( "GiftDragonHelm",            "Scalemail Helm",          RelicCategory.Armor ),
            new RelicEntry( "GiftDragonLegs",            "Scalemail Leggings",      RelicCategory.Armor ),
            new RelicEntry( "GiftScalemailShield",       "Scalemail Shield",        RelicCategory.Armor ),
            new RelicEntry( "GiftDragonChest",           "Scalemail Tunic",         RelicCategory.Armor ),
            new RelicEntry( "GiftShinobiCowl",           "Leather Shinobi Cowl",    RelicCategory.Armor ),
            new RelicEntry( "GiftShinobiHood",           "Leather Shinobi Hood",    RelicCategory.Armor ),
            new RelicEntry( "GiftShinobiMask",           "Leather Shinobi Mask",    RelicCategory.Armor ),
            new RelicEntry( "GiftShinobiRobe",           "Leather Shinobi Robe",    RelicCategory.Armor ),
            new RelicEntry( "GiftSmallPlateJingasa",     "Small Plate Jingasa",     RelicCategory.Armor ),
            new RelicEntry( "GiftStandardPlateKabuto",   "Standard Plate Kabuto",   RelicCategory.Armor ),
            new RelicEntry( "GiftStuddedArms",           "Studded Arms",            RelicCategory.Armor ),
            new RelicEntry( "GiftStuddedBustierArms",    "Studded Bustier Arms",    RelicCategory.Armor ),
            new RelicEntry( "GiftStuddedChest",          "Studded Chest",           RelicCategory.Armor ),
            new RelicEntry( "GiftStuddedDo",             "Studded Do",              RelicCategory.Armor ),
            new RelicEntry( "GiftStuddedGloves",         "Studded Gloves",          RelicCategory.Armor ),
            new RelicEntry( "GiftStuddedGorget",         "Studded Gorget",          RelicCategory.Armor ),
            new RelicEntry( "GiftStuddedHaidate",        "Studded Haidate",         RelicCategory.Armor ),
            new RelicEntry( "GiftStuddedHiroSode",       "Studded Hiro Sode",       RelicCategory.Armor ),
            new RelicEntry( "GiftStuddedLegs",           "Studded Legs",            RelicCategory.Armor ),
            new RelicEntry( "GiftStuddedMempo",          "Studded Mempo",           RelicCategory.Armor ),
            new RelicEntry( "GiftStuddedSuneate",        "Studded Suneate",         RelicCategory.Armor ),
            new RelicEntry( "GiftSunShield",             "Sun Shield",              RelicCategory.Armor ),
            new RelicEntry( "GiftVirtueShield",          "Virtue Shield",           RelicCategory.Armor ),
            new RelicEntry( "GiftWoodenKiteShield",      "Wooden Kite Shield",      RelicCategory.Armor ),
            new RelicEntry( "GiftWoodenPlateArms",       "Wooden Plate Arms",       RelicCategory.Armor ),
            new RelicEntry( "GiftWoodenPlateChest",      "Wooden Plate Chest",      RelicCategory.Armor ),
            new RelicEntry( "GiftWoodenPlateGloves",     "Wooden Plate Gloves",     RelicCategory.Armor ),
            new RelicEntry( "GiftWoodenPlateGorget",     "Wooden Plate Gorget",     RelicCategory.Armor ),
            new RelicEntry( "GiftWoodenPlateHelm",       "Wooden Plate Helm",       RelicCategory.Armor ),
            new RelicEntry( "GiftWoodenPlateLegs",       "Wooden Plate Legs",       RelicCategory.Armor ),
            new RelicEntry( "GiftWoodenShield",          "Wooden Shield",           RelicCategory.Armor ),
            new RelicEntry( "GiftChampionShield",        "Champion Shield",         RelicCategory.Armor ),
            new RelicEntry( "GiftCrestedShield",         "Crested Shield",          RelicCategory.Armor ),

            // ----- Clothing -----
            new RelicEntry( "GiftBandana",          "Bandana",           RelicCategory.Clothing ),
            new RelicEntry( "GiftBearMask",         "Bear Mask",         RelicCategory.Clothing ),
            new RelicEntry( "GiftBelt",             "Belt",              RelicCategory.Clothing ),
            new RelicEntry( "GiftBodySash",         "Body Sash",         RelicCategory.Clothing ),
            new RelicEntry( "GiftBonnet",           "Bonnet",            RelicCategory.Clothing ),
            new RelicEntry( "GiftBoots",            "Boots",             RelicCategory.Clothing ),
            new RelicEntry( "GiftCap",              "Cap",               RelicCategory.Clothing ),
            new RelicEntry( "GiftCloak",            "Cloak",             RelicCategory.Clothing ),
            new RelicEntry( "GiftClothNinjaHood",   "Cloth Ninja Hood",  RelicCategory.Clothing ),
            new RelicEntry( "GiftClothNinjaJacket", "Cloth Ninja Jacket",RelicCategory.Clothing ),
            new RelicEntry( "GiftCowl",             "Cowl",              RelicCategory.Clothing ),
            new RelicEntry( "GiftDeerMask",         "Deer Mask",         RelicCategory.Clothing ),
            new RelicEntry( "GiftDoublet",          "Doublet",           RelicCategory.Clothing ),
            new RelicEntry( "GiftElvenBoots",       "Fancy Boots",       RelicCategory.Clothing ),
            new RelicEntry( "GiftFancyDress",       "Fancy Dress",       RelicCategory.Clothing ),
            new RelicEntry( "GiftFancyShirt",       "Fancy Shirt",       RelicCategory.Clothing ),
            new RelicEntry( "GiftFeatheredHat",     "Feathered Hat",     RelicCategory.Clothing ),
            new RelicEntry( "GiftFemaleKimono",     "Female Kimono",     RelicCategory.Clothing ),
            new RelicEntry( "GiftFloppyHat",        "Floppy Hat",        RelicCategory.Clothing ),
            new RelicEntry( "GiftFormalShirt",      "Formal Shirt",      RelicCategory.Clothing ),
            new RelicEntry( "GiftFullApron",        "Full Apron",        RelicCategory.Clothing ),
            new RelicEntry( "GiftFurBoots",         "Fur Boots",         RelicCategory.Clothing ),
            new RelicEntry( "GiftFurCape",          "Fur Cape",          RelicCategory.Clothing ),
            new RelicEntry( "GiftFurSarong",        "Fur Sarong",        RelicCategory.Clothing ),
            new RelicEntry( "GiftGildedDress",      "Gilded Dress",      RelicCategory.Clothing ),
            new RelicEntry( "GiftHakama",           "Hakama",            RelicCategory.Clothing ),
            new RelicEntry( "GiftHakamaShita",      "Hakama Shita",      RelicCategory.Clothing ),
            new RelicEntry( "GiftHalfApron",        "Half Apron",        RelicCategory.Clothing ),
            new RelicEntry( "GiftHood",             "Hood",              RelicCategory.Clothing ),
            new RelicEntry( "GiftHornedTribalMask", "Horned Tribal Mask",RelicCategory.Clothing ),
            new RelicEntry( "GiftJesterHat",        "Jester Hat",        RelicCategory.Clothing ),
            new RelicEntry( "GiftJesterSuit",       "Jester Suit",       RelicCategory.Clothing ),
            new RelicEntry( "GiftJinBaori",         "Jin Baori",         RelicCategory.Clothing ),
            new RelicEntry( "GiftKamishimo",        "Kamishimo",         RelicCategory.Clothing ),
            new RelicEntry( "GiftKasa",             "Kasa",              RelicCategory.Clothing ),
            new RelicEntry( "GiftKilt",             "Kilt",              RelicCategory.Clothing ),
            new RelicEntry( "GiftLoinCloth",        "Loin Cloth",        RelicCategory.Clothing ),
            new RelicEntry( "GiftLongPants",        "Long Pants",        RelicCategory.Clothing ),
            new RelicEntry( "GiftMaleKimono",       "Male Kimono",       RelicCategory.Clothing ),
            new RelicEntry( "GiftNinjaTabi",        "Ninja Tabi",        RelicCategory.Clothing ),
            new RelicEntry( "GiftObi",              "Obi",               RelicCategory.Clothing ),
            new RelicEntry( "GiftPlainDress",       "Plain Dress",       RelicCategory.Clothing ),
            new RelicEntry( "GiftPirateHat",        "Pirate Hat",        RelicCategory.Clothing ),
            new RelicEntry( "GiftRobe",             "Robe",              RelicCategory.Clothing ),
            new RelicEntry( "GiftRoyalCape",        "Royal Cape",        RelicCategory.Clothing ),
            new RelicEntry( "GiftSamuraiTabi",      "Samurai Tabi",      RelicCategory.Clothing ),
            new RelicEntry( "GiftSandals",          "Sandals",           RelicCategory.Clothing ),
            new RelicEntry( "GiftSash",             "Sash",              RelicCategory.Clothing ),
            new RelicEntry( "GiftShirt",            "Shirt",             RelicCategory.Clothing ),
            new RelicEntry( "GiftShoes",            "Shoes",             RelicCategory.Clothing ),
            new RelicEntry( "GiftShortPants",       "Short Pants",       RelicCategory.Clothing ),
            new RelicEntry( "GiftSkirt",            "Skirt",             RelicCategory.Clothing ),
            new RelicEntry( "GiftSkullCap",         "Skull Cap",         RelicCategory.Clothing ),
            new RelicEntry( "GiftStrawHat",         "Straw Hat",         RelicCategory.Clothing ),
            new RelicEntry( "GiftSurcoat",          "Surcoat",           RelicCategory.Clothing ),
            new RelicEntry( "GiftTallStrawHat",     "Tall Straw Hat",    RelicCategory.Clothing ),
            new RelicEntry( "GiftTattsukeHakama",   "Tattsuke Hakama",   RelicCategory.Clothing ),
            new RelicEntry( "GiftThighBoots",       "Thigh Boots",       RelicCategory.Clothing ),
            new RelicEntry( "GiftTribalMask",       "Tribal Mask",       RelicCategory.Clothing ),
            new RelicEntry( "GiftTricorneHat",      "Tricorne Hat",      RelicCategory.Clothing ),
            new RelicEntry( "GiftTunic",            "Tunic",             RelicCategory.Clothing ),
            new RelicEntry( "GiftWaraji",           "Waraji",            RelicCategory.Clothing ),
            new RelicEntry( "GiftWideBrimHat",      "Wide Brim Hat",     RelicCategory.Clothing ),
            new RelicEntry( "GiftWitchHat",         "Witch Hat",         RelicCategory.Clothing ),
            new RelicEntry( "GiftWizardsHat",       "Wizards Hat",       RelicCategory.Clothing ),
            new RelicEntry( "GiftWolfMask",         "Wolf Mask",         RelicCategory.Clothing ),
            new RelicEntry( "GiftHikingBoots",      "Hiking Boots",      RelicCategory.Clothing ),
        };

        public static ArrayList GetEntriesForCategory( RelicCategory cat )
        {
            ArrayList result = new ArrayList();
            for ( int i = 0; i < AllRelics.Length; i++ )
            {
                if ( AllRelics[i].Category == cat )
                    result.Add( AllRelics[i] );
            }
            return result;
        }

        public static string CategoryLabel( RelicCategory cat )
        {
            switch ( cat )
            {
                case RelicCategory.OneHandedWeapons: return "Armas de Uma Mão";
                case RelicCategory.TwoHandedWeapons: return "Armas de Duas Mãos";
                case RelicCategory.RangedWeapons:    return "Armas de Longo Alcance";
                case RelicCategory.JewelryTrinkets:  return "Joias & Trinkets";
                case RelicCategory.Armor:            return "Armaduras";
                case RelicCategory.Clothing:         return "Vestuário";
                default:                             return "Unknown";
            }
        }

        private static string FixDisplayName( string displayName, string extra )
        {
            string sArty = displayName;
            if ( sArty == "Trinket, Symbol"   ) sArty = "Talisman";
            if ( sArty == "Trinket, Idol"     ) sArty = "Talisman";
            if ( sArty == "Trinket, Totem"    ) sArty = "Talisman";
            if ( sArty == "Trinket, Talisman" ) sArty = "Talisman";
            if ( extra != "" && extra != null ) sArty = sArty + " " + extra;
            return sArty;
        }

        public static ManualOfItems m_Book;

        public int m_Charges;
        [CommandProperty( AccessLevel.GameMaster )]
        public int Charges { get { return m_Charges; } set { m_Charges = value; InvalidateProperties(); } }

        public int m_Skill_1;
        [CommandProperty( AccessLevel.GameMaster )]
        public int mSkill1 { get { return m_Skill_1; } set { m_Skill_1 = value; InvalidateProperties(); } }

        public int m_Skill_2;
        [CommandProperty( AccessLevel.GameMaster )]
        public int mSkill2 { get { return m_Skill_2; } set { m_Skill_2 = value; InvalidateProperties(); } }

        public int m_Skill_3;
        [CommandProperty( AccessLevel.GameMaster )]
        public int mSkill3 { get { return m_Skill_3; } set { m_Skill_3 = value; InvalidateProperties(); } }

        public int m_Skill_4;
        [CommandProperty( AccessLevel.GameMaster )]
        public int mSkill4 { get { return m_Skill_4; } set { m_Skill_4 = value; InvalidateProperties(); } }

        public int m_Skill_5;
        [CommandProperty( AccessLevel.GameMaster )]
        public int mSkill5 { get { return m_Skill_5; } set { m_Skill_5 = value; InvalidateProperties(); } }

        public double m_Value_1;
        [CommandProperty( AccessLevel.GameMaster )]
        public double mValue1 { get { return m_Value_1; } set { m_Value_1 = value; InvalidateProperties(); } }

        public double m_Value_2;
        [CommandProperty( AccessLevel.GameMaster )]
        public double mValue2 { get { return m_Value_2; } set { m_Value_2 = value; InvalidateProperties(); } }

        public double m_Value_3;
        [CommandProperty( AccessLevel.GameMaster )]
        public double mValue3 { get { return m_Value_3; } set { m_Value_3 = value; InvalidateProperties(); } }

        public double m_Value_4;
        [CommandProperty( AccessLevel.GameMaster )]
        public double mValue4 { get { return m_Value_4; } set { m_Value_4 = value; InvalidateProperties(); } }

        public double m_Value_5;
        [CommandProperty( AccessLevel.GameMaster )]
        public double mValue5 { get { return m_Value_5; } set { m_Value_5 = value; InvalidateProperties(); } }

        public int m_Slayer_1;
        [CommandProperty( AccessLevel.GameMaster )]
        public int mSlayer1 { get { return m_Slayer_1; } set { m_Slayer_1 = value; InvalidateProperties(); } }

        public int m_Slayer_2;
        [CommandProperty( AccessLevel.GameMaster )]
        public int mSlayer2 { get { return m_Slayer_2; } set { m_Slayer_2 = value; InvalidateProperties(); } }

        public Mobile m_Owner;
        [CommandProperty( AccessLevel.GameMaster )]
        public Mobile mOwner { get { return m_Owner; } set { m_Owner = value; InvalidateProperties(); } }

        public string m_Extra;
        [CommandProperty( AccessLevel.GameMaster )]
        public string mExtra { get { return m_Extra; } set { m_Extra = value; InvalidateProperties(); } }

        public string m_FromWho;
        [CommandProperty( AccessLevel.GameMaster )]
        public string mFromWho { get { return m_FromWho; } set { m_FromWho = value; InvalidateProperties(); } }

        public string m_HowGiven;
        [CommandProperty( AccessLevel.GameMaster )]
        public string mHowGiven { get { return m_HowGiven; } set { m_HowGiven = value; InvalidateProperties(); } }

        public int m_Points;
        [CommandProperty( AccessLevel.GameMaster )]
        public int mPoints { get { return m_Points; } set { m_Points = value; InvalidateProperties(); } }

        public int m_Hue;
        [CommandProperty( AccessLevel.GameMaster )]
        public int mHue { get { return m_Hue; } set { m_Hue = value; InvalidateProperties(); } }

        // ------------------------------------------------------------------
        [Constructable]
        public ManualOfItems() : base( 0x1C0E )
        {
            Weight = 5.0;
            Hue    = Utility.RandomColor( 0 );
            ItemID = Utility.RandomList( 0x1C0E, 0x1C0F );
            Name   = "Baú de Relíquias Mágicas";

            if ( m_Charges < 1 )
            {
                m_Charges  = 1;
                m_Skill_1  = 0; m_Skill_2  = 0; m_Skill_3  = 0; m_Skill_4  = 0; m_Skill_5  = 0;
                m_Value_1  = 0.0; m_Value_2 = 0.0; m_Value_3 = 0.0; m_Value_4 = 0.0; m_Value_5 = 0.0;
                m_Slayer_1 = 0; m_Slayer_2 = 0;
                m_Owner    = null;
                m_Extra    = null;
                m_FromWho  = "";
                m_HowGiven = "";
                m_Points   = 100;
                m_Hue      = 0;
            }
        }

        public override void GetProperties( ObjectPropertyList list )
        {
            base.GetProperties( list );
            list.Add( 1060741, m_Charges.ToString() );
        }

        public override void AddNameProperties( ObjectPropertyList list )
        {
            base.AddNameProperties( list );
            if ( m_FromWho != "" && m_FromWho != null ) list.Add( 1070722, m_FromWho );
            if ( m_Owner != null ) list.Add( 1049644, "Pertence a " + m_Owner.Name );
        }

        public override void OnDoubleClick( Mobile from )
        {
            bool canOpen = ( m_Owner == null || m_Owner == from );

            if ( canOpen )
            {
                from.SendSound( 0x02D );
                from.CloseGump( typeof( RelicIntroGump ) );
                from.CloseGump( typeof( RelicCategoryGump ) );
                from.CloseGump( typeof( RelicItemsGump ) );
                from.CloseGump( typeof( RelicConfirmGump ) );
                from.SendGump( new RelicIntroGump( from, this ) );
            }
            else
            {
                from.SendMessage( "You cannot seem to get the chest to open. Is it yours?" );
            }
        }

        public static void CreateAndGiveReward( Mobile from, ManualOfItems book, RelicEntry entry )
        {
            string sArty = ManualOfItems.FixDisplayName( entry.DisplayName, book.m_Extra );

            Type itemType = ScriptCompiler.FindTypeByName( entry.TypeName );
            if ( itemType == null ) return;

            Item reward = (Item)Activator.CreateInstance( itemType );

            Mobile owner = ( book.m_Owner != null ) ? book.m_Owner : from;

            if ( reward is BaseGiftAxe )      { ((BaseGiftAxe)reward).m_Owner      = owner; ((BaseGiftAxe)reward).m_Gifter      = book.m_FromWho; ((BaseGiftAxe)reward).m_How      = book.m_HowGiven; ((BaseGiftAxe)reward).m_Points      = book.m_Points; }
            if ( reward is BaseGiftRanged )   { ((BaseGiftRanged)reward).m_Owner   = owner; ((BaseGiftRanged)reward).m_Gifter   = book.m_FromWho; ((BaseGiftRanged)reward).m_How   = book.m_HowGiven; ((BaseGiftRanged)reward).m_Points   = book.m_Points; }
            if ( reward is BaseGiftSpear )    { ((BaseGiftSpear)reward).m_Owner    = owner; ((BaseGiftSpear)reward).m_Gifter    = book.m_FromWho; ((BaseGiftSpear)reward).m_How    = book.m_HowGiven; ((BaseGiftSpear)reward).m_Points    = book.m_Points; }
            if ( reward is BaseGiftClothing ) { ((BaseGiftClothing)reward).m_Owner = owner; ((BaseGiftClothing)reward).m_Gifter = book.m_FromWho; ((BaseGiftClothing)reward).m_How = book.m_HowGiven; ((BaseGiftClothing)reward).m_Points = book.m_Points; }
            if ( reward is BaseGiftJewel )    { ((BaseGiftJewel)reward).m_Owner    = owner; ((BaseGiftJewel)reward).m_Gifter    = book.m_FromWho; ((BaseGiftJewel)reward).m_How    = book.m_HowGiven; ((BaseGiftJewel)reward).m_Points    = book.m_Points; }
            if ( reward is BaseGiftArmor )    { ((BaseGiftArmor)reward).m_Owner    = owner; ((BaseGiftArmor)reward).m_Gifter    = book.m_FromWho; ((BaseGiftArmor)reward).m_How    = book.m_HowGiven; ((BaseGiftArmor)reward).m_Points    = book.m_Points; }
            if ( reward is BaseGiftShield )   { ((BaseGiftShield)reward).m_Owner   = owner; ((BaseGiftShield)reward).m_Gifter   = book.m_FromWho; ((BaseGiftShield)reward).m_How   = book.m_HowGiven; ((BaseGiftShield)reward).m_Points   = book.m_Points; }
            if ( reward is BaseGiftKnife )    { ((BaseGiftKnife)reward).m_Owner    = owner; ((BaseGiftKnife)reward).m_Gifter    = book.m_FromWho; ((BaseGiftKnife)reward).m_How    = book.m_HowGiven; ((BaseGiftKnife)reward).m_Points    = book.m_Points; }
            if ( reward is BaseGiftBashing )  { ((BaseGiftBashing)reward).m_Owner  = owner; ((BaseGiftBashing)reward).m_Gifter  = book.m_FromWho; ((BaseGiftBashing)reward).m_How  = book.m_HowGiven; ((BaseGiftBashing)reward).m_Points  = book.m_Points; }
            if ( reward is BaseGiftWhip )     { ((BaseGiftWhip)reward).m_Owner     = owner; ((BaseGiftWhip)reward).m_Gifter     = book.m_FromWho; ((BaseGiftWhip)reward).m_How     = book.m_HowGiven; ((BaseGiftWhip)reward).m_Points     = book.m_Points; }
            if ( reward is BaseGiftPoleArm )  { ((BaseGiftPoleArm)reward).m_Owner  = owner; ((BaseGiftPoleArm)reward).m_Gifter  = book.m_FromWho; ((BaseGiftPoleArm)reward).m_How  = book.m_HowGiven; ((BaseGiftPoleArm)reward).m_Points  = book.m_Points; }
            if ( reward is BaseGiftStaff )    { ((BaseGiftStaff)reward).m_Owner    = owner; ((BaseGiftStaff)reward).m_Gifter    = book.m_FromWho; ((BaseGiftStaff)reward).m_How    = book.m_HowGiven; ((BaseGiftStaff)reward).m_Points    = book.m_Points; }
            if ( reward is BaseGiftSword )    { ((BaseGiftSword)reward).m_Owner    = owner; ((BaseGiftSword)reward).m_Gifter    = book.m_FromWho; ((BaseGiftSword)reward).m_How    = book.m_HowGiven; ((BaseGiftSword)reward).m_Points    = book.m_Points; }

            reward.Name = sArty;
            reward.Hue  = book.m_Hue;

            GiveItemBonus( reward,
                book.m_Skill_1, book.m_Skill_2, book.m_Skill_3, book.m_Skill_4, book.m_Skill_5,
                book.m_Value_1, book.m_Value_2, book.m_Value_3, book.m_Value_4, book.m_Value_5,
                book.m_Slayer_1, book.m_Slayer_2 );

            from.AddToBackpack( reward );
            from.SendMessage( "Agora você possui o " + sArty + "." );
            from.PlaySound( 0x1FA );

            book.m_Charges--;
            book.InvalidateProperties();

            if ( book.m_Charges < 1 )
                book.Delete();
        }

        public static void GiveItemBonus( Item item, int val1, int val2, int val3, int val4, int val5, double sk1, double sk2, double sk3, double sk4, double sk5, int slay1, int slay2 )
        {
            if ( item is BaseWeapon )
            {
                if ( slay1 > 0 ) ((BaseWeapon)item).Slayer  = ResourceMods.GetSlayer( slay1 );
                if ( slay2 > 0 ) ((BaseWeapon)item).Slayer2 = ResourceMods.GetSlayer( slay2 );
                if ( val1 == 99 )      ((BaseWeapon)item).SkillBonuses.SetValues( 0, ((BaseWeapon)item).Skill, sk1 );
                else if ( val1 > 0 )   ((BaseWeapon)item).SkillBonuses.SetValues( 0, ResourceMods.GetSkill( val1 ), sk1 );
                if ( val2 > 0 )        ((BaseWeapon)item).SkillBonuses.SetValues( 1, ResourceMods.GetSkill( val2 ), sk2 );
                if ( val3 > 0 )        ((BaseWeapon)item).SkillBonuses.SetValues( 2, ResourceMods.GetSkill( val3 ), sk3 );
                if ( val4 > 0 )        ((BaseWeapon)item).SkillBonuses.SetValues( 3, ResourceMods.GetSkill( val4 ), sk4 );
                if ( val5 == 100 )     ((BaseWeapon)item).Attributes.EnhancePotions = (int)sk5;
                else if ( val5 > 0 )   ((BaseWeapon)item).SkillBonuses.SetValues( 4, ResourceMods.GetSkill( val5 ), sk5 );
            }
            else if ( item is BaseArmor )
            {
                if ( val1 == 99 && item is BaseShield ) ((BaseShield)item).SkillBonuses.SetValues( 0, SkillName.Parry, sk1 );
                else if ( val1 > 0 )   ((BaseArmor)item).SkillBonuses.SetValues( 0, ResourceMods.GetSkill( val1 ), sk1 );
                if ( val2 > 0 )        ((BaseArmor)item).SkillBonuses.SetValues( 1, ResourceMods.GetSkill( val2 ), sk2 );
                if ( val3 > 0 )        ((BaseArmor)item).SkillBonuses.SetValues( 2, ResourceMods.GetSkill( val3 ), sk3 );
                if ( val4 > 0 )        ((BaseArmor)item).SkillBonuses.SetValues( 3, ResourceMods.GetSkill( val4 ), sk4 );
                if ( val5 == 100 )     ((BaseArmor)item).Attributes.EnhancePotions = (int)sk5;
                else if ( val5 > 0 )   ((BaseArmor)item).SkillBonuses.SetValues( 4, ResourceMods.GetSkill( val5 ), sk5 );
            }
            else if ( item is BaseClothing )
            {
                if ( val1 > 0 )        ((BaseClothing)item).SkillBonuses.SetValues( 0, ResourceMods.GetSkill( val1 ), sk1 );
                if ( val2 > 0 )        ((BaseClothing)item).SkillBonuses.SetValues( 1, ResourceMods.GetSkill( val2 ), sk2 );
                if ( val3 > 0 )        ((BaseClothing)item).SkillBonuses.SetValues( 2, ResourceMods.GetSkill( val3 ), sk3 );
                if ( val4 > 0 )        ((BaseClothing)item).SkillBonuses.SetValues( 3, ResourceMods.GetSkill( val4 ), sk4 );
                if ( val5 == 100 )     ((BaseClothing)item).Attributes.EnhancePotions = (int)sk5;
                else if ( val5 > 0 )   ((BaseClothing)item).SkillBonuses.SetValues( 4, ResourceMods.GetSkill( val5 ), sk5 );
            }
            else if ( item is BaseTrinket )
            {
                if ( val1 > 0 )        ((BaseTrinket)item).SkillBonuses.SetValues( 0, ResourceMods.GetSkill( val1 ), sk1 );
                if ( val2 > 0 )        ((BaseTrinket)item).SkillBonuses.SetValues( 1, ResourceMods.GetSkill( val2 ), sk2 );
                if ( val3 > 0 )        ((BaseTrinket)item).SkillBonuses.SetValues( 2, ResourceMods.GetSkill( val3 ), sk3 );
                if ( val4 > 0 )        ((BaseTrinket)item).SkillBonuses.SetValues( 3, ResourceMods.GetSkill( val4 ), sk4 );
                if ( val5 == 100 )     ((BaseTrinket)item).Attributes.EnhancePotions = (int)sk5;
                else if ( val5 > 0 )   ((BaseTrinket)item).SkillBonuses.SetValues( 4, ResourceMods.GetSkill( val5 ), sk5 );
            }
        }

        public class RelicIntroGump : Gump
        {
            private ManualOfItems m_Book;

            public RelicIntroGump( Mobile from, ManualOfItems book ) : base( 50, 50 )
            {
                m_Book = book;
                string color = "#cfc990";

                Closable  = true;
                Disposable = true;
                Dragable  = true;
                Resizable = false;

                AddPage( 0 );
                AddImage( 0, 0, 7055, Server.Misc.PlayerSettings.GetGumpHue( from ) );

                AddHtml( 61, 12, 579, 20,
					"<BODY><BASEFONT Color=" + color + "><CENTER>BAÚ DE RELÍQUIAS MÁGICAS</CENTER></BASEFONT></BODY>",
					false, false );

				AddHtml( 13, 52, 681, 364,
					"<BODY><BASEFONT Color=" + color + ">Você obteve um baú com itens poderosos de sua escolha. " +
					"Você pode selecionar quantos itens o baú tiver cargas. Quando as cargas forem usadas, o baú desaparecerá. " +
					"Ao fazer uma seleção, o item aparecerá em sua mochila. Alguns baús fornecem atributos adicionais aos itens, " +
					"como propriedades de extermínio ou melhorias de habilidade. Cada item aparecerá com um número de pontos que você pode gastar para " +
					"aprimorar seu item. Isso permite que você personalize o item para se adequar ao seu estilo. Para começar, clique uma vez nos itens e " +
					"selecione 'Encantar'. Um menu aparecerá onde você pode escolher quais atributos deseja que o item tenha. Tenha cuidado, " +
					"pois você não pode alterar um atributo depois de selecioná-lo.</BASEFONT></BODY>",
					false, false );

                AddButton( 668, 425, 4005, 4005, 1, GumpButtonType.Reply, 0 );
            }

            public override void OnResponse( NetState state, RelayInfo info )
            {
                Mobile from = state.Mobile;
                from.SendSound( 0x4A );

                if ( info.ButtonID == 1 )
                {
                    from.CloseGump( typeof( RelicIntroGump ) );
                    from.SendGump( new RelicCategoryGump( from, m_Book ) );
                }
            }
        }

        public class RelicCategoryGump : Gump
        {
            private ManualOfItems m_Book;

            public RelicCategoryGump( Mobile from, ManualOfItems book ) : base( 50, 50 )
            {
                m_Book = book;
                string color = "#cfc990";

                Closable  = true;
                Disposable = true;
                Dragable  = true;
                Resizable = false;

                AddPage( 0 );
                AddImage( 0, 0, 7055, Server.Misc.PlayerSettings.GetGumpHue( from ) );

                AddHtml( 61, 12, 579, 20,
					"<BODY><BASEFONT Color=" + color + "><CENTER>BAÚ DE RELÍQUIAS MÁGICAS - Escolha uma Categoria</CENTER></BASEFONT></BODY>",
					false, false );

				// Cargas restantes
				AddHtml( 61, 35, 579, 20,
					"<BODY><BASEFONT Color=" + color + "><CENTER>Cargas restantes: " + book.m_Charges.ToString() + "</CENTER></BASEFONT></BODY>",
					false, false );

                int btnX   = 120;
                int labelX = 165;
                int y      = 100;
                int step   = 48;

                AddCategoryRow( btnX, labelX, y, 1, "Armas de Uma Mão", color ); y += step;
				AddCategoryRow( btnX, labelX, y, 2, "Armas de Duas Mãos", color ); y += step;
				AddCategoryRow( btnX, labelX, y, 3, "Armas de Longo Alcance", color ); y += step;
				AddCategoryRow( btnX, labelX, y, 4, "Joias & Trinkets", color ); y += step;
				AddCategoryRow( btnX, labelX, y, 5, "Armaduras", color ); y += step;
				AddCategoryRow( btnX, labelX, y, 6, "Vestuário", color );

                // Close
                AddButton( 668, 9, 4017, 4017, 0, GumpButtonType.Reply, 0 );
            }

            private void AddCategoryRow( int btnX, int labelX, int y, int buttonID, string label, string color )
            {
                AddButton( btnX, y, 4005, 4007, buttonID, GumpButtonType.Reply, 0 );
                AddHtml( labelX, y + 2, 300, 22,
                    "<BODY><BASEFONT Color=" + color + ">" + label + "</BASEFONT></BODY>",
                    false, false );
            }

            public override void OnResponse( NetState state, RelayInfo info )
            {
                Mobile from = state.Mobile;
                from.SendSound( 0x4A );

                RelicCategory cat;
                switch ( info.ButtonID )
                {
                    case 1: cat = RelicCategory.OneHandedWeapons; break;
                    case 2: cat = RelicCategory.TwoHandedWeapons; break;
                    case 3: cat = RelicCategory.RangedWeapons;    break;
                    case 4: cat = RelicCategory.JewelryTrinkets;  break;
                    case 5: cat = RelicCategory.Armor;            break;
                    case 6: cat = RelicCategory.Clothing;         break;
                    default: return; // Close
                }

                from.CloseGump( typeof( RelicCategoryGump ) );
                from.SendGump( new RelicItemsGump( from, m_Book, cat, 0 ) );
            }
        }

        public class RelicItemsGump : Gump
        {
            private ManualOfItems m_Book;
            private RelicCategory m_Category;
            private ArrayList     m_Entries;
            private int           m_Page;

            private const int ItemsPerPage = 16;

            // Button IDs:
            //   0            = back to categories
            //   1            = previous page
            //   2            = next page
            //   1000 + index = select item

            public RelicItemsGump( Mobile from, ManualOfItems book, RelicCategory cat, int page )
                : base( 50, 50 )
            {
                m_Book     = book;
                m_Category = cat;
                m_Entries  = ManualOfItems.GetEntriesForCategory( cat );
                m_Page     = page;

                string color = "#cfc990";

                Closable  = true;
                Disposable = true;
                Dragable  = true;
                Resizable = false;

                AddPage( 0 );
                AddImage( 0, 0, 7055, Server.Misc.PlayerSettings.GetGumpHue( from ) );

                // Close / back to category button (top right)
                AddButton( 668, 9, 4017, 4017, 0, GumpButtonType.Reply, 0 );

                // Title
                AddHtml( 61, 12, 579, 20,
                    "<BODY><BASEFONT Color=" + color + "><CENTER>" + ManualOfItems.CategoryLabel( cat ).ToUpper() + "</CENTER></BASEFONT></BODY>",
                    false, false );

                int totalPages = ( m_Entries.Count + ItemsPerPage - 1 ) / ItemsPerPage;
                if ( totalPages < 1 ) totalPages = 1;

                // Prev / Next
                if ( totalPages > 1 )
                {
                    AddButton( 9,   425, 4014, 4014, 1, GumpButtonType.Reply, 0 );
                    AddButton( 668, 425, 4005, 4005, 2, GumpButtonType.Reply, 0 );
                }

                int firstIndex = page * ItemsPerPage;
                int x, y, z, s;

                // ---- Left column buttons ----
                x = 83; s = 84; z = 34;
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
                        ? ( (RelicEntry)m_Entries[idx] ).DisplayName : "";
                    AddHtml( x + 20, y, 155, 20,
                        "<BODY><BASEFONT Color=" + color + ">" + label + "</BASEFONT></BODY>",
                        false, false );
                    y += z;
                }

                // ---- Right column buttons ----
                x = 375;
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
                        ? ( (RelicEntry)m_Entries[idx] ).DisplayName : "";
                    AddHtml( x + 20, y, 155, 20,
                        "<BODY><BASEFONT Color=" + color + ">" + label + "</BASEFONT></BODY>",
                        false, false );
                    y += z;
                }
            }

            public override void OnResponse( NetState state, RelayInfo info )
            {
                Mobile from = state.Mobile;
                from.SendSound( 0x4A );

                int totalPages = ( m_Entries.Count + ItemsPerPage - 1 ) / ItemsPerPage;
                if ( totalPages < 1 ) totalPages = 1;

                if ( info.ButtonID == 0 )
                {
                    from.CloseGump( typeof( RelicItemsGump ) );
                    from.SendGump( new RelicCategoryGump( from, m_Book ) );
                }
                else if ( info.ButtonID == 1 )
                {
                    int prev = m_Page - 1;
                    if ( prev < 0 ) prev = totalPages - 1;
                    from.CloseGump( typeof( RelicItemsGump ) );
                    from.SendGump( new RelicItemsGump( from, m_Book, m_Category, prev ) );
                }
                else if ( info.ButtonID == 2 )
                {
                    int next = m_Page + 1;
                    if ( next >= totalPages ) next = 0;
                    from.CloseGump( typeof( RelicItemsGump ) );
                    from.SendGump( new RelicItemsGump( from, m_Book, m_Category, next ) );
                }
                else if ( info.ButtonID >= 1000 )
                {
                    int idx = info.ButtonID - 1000;
                    if ( idx >= 0 && idx < m_Entries.Count )
                    {
                        RelicEntry entry = (RelicEntry)m_Entries[idx];
                        from.CloseGump( typeof( RelicItemsGump ) );
                        from.SendGump( new RelicConfirmGump( from, m_Book, entry, m_Category, m_Page ) );
                    }
                }
            }
        }

        public class RelicConfirmGump : Gump
        {
            private ManualOfItems m_Book;
            private RelicEntry    m_Entry;
            private RelicCategory m_ReturnCategory;
            private int           m_ReturnPage;

            public RelicConfirmGump( Mobile from, ManualOfItems book, RelicEntry entry,
                                     RelicCategory returnCat, int returnPage )
                : base( 50, 50 )
            {
                m_Book           = book;
                m_Entry          = entry;
                m_ReturnCategory = returnCat;
                m_ReturnPage     = returnPage;

                string color = "#cfc990";
                string sArty = ManualOfItems.FixDisplayName( entry.DisplayName, book.m_Extra );

                Closable  = true;
                Disposable = true;
                Dragable  = true;
                Resizable = false;

                AddBackground( 0, 0, 460, 190, 9270 );

                AddHtml( 20, 18, 420, 20,
					"<BODY><BASEFONT Color=" + color + ">Você está prestes a reivindicar:</BASEFONT></BODY>",
					false, false );
				AddHtml( 20, 42, 420, 20,
					"<BODY><BASEFONT Color=" + color + ">" + sArty + "</BASEFONT></BODY>",
					false, false );
				AddHtml( 20, 72, 420, 40,
					"<BODY><BASEFONT Color=" + color + ">Isso usará uma carga. Cargas restantes: " + book.m_Charges.ToString() + "</BASEFONT></BODY>",
					false, false );

				AddButton( 60,  148, 4005, 4007, 1, GumpButtonType.Reply, 0 );
				AddHtml( 95, 150, 80, 20,
					"<BODY><BASEFONT Color=" + color + ">Confirmar</BASEFONT></BODY>",
					false, false );

				AddButton( 260, 148, 4017, 4019, 0, GumpButtonType.Reply, 0 );
				AddHtml( 295, 150, 80, 20,
					"<BODY><BASEFONT Color=" + color + ">Voltar</BASEFONT></BODY>",
					false, false );
            }

            public override void OnResponse( NetState state, RelayInfo info )
            {
                Mobile from = state.Mobile;
                from.SendSound( 0x4A );

                if ( info.ButtonID == 1 )
                {
                    if ( m_Book.Deleted )
                    {
                        from.SendMessage( "O baú já foi totalmente usado." );
                        return;
                    }

                    ManualOfItems.CreateAndGiveReward( from, m_Book, m_Entry );

                    if ( !m_Book.Deleted )
                    {
                        from.CloseGump( typeof( RelicConfirmGump ) );
                        from.SendGump( new RelicCategoryGump( from, m_Book ) );
                    }
                }
                else
                {
                    from.CloseGump( typeof( RelicConfirmGump ) );
                    from.SendGump( new RelicItemsGump( from, m_Book, m_ReturnCategory, m_ReturnPage ) );
                }
            }
        }

        public ManualOfItems( Serial serial ) : base( serial ) { }

        public override void Serialize( GenericWriter writer )
        {
            base.Serialize( writer );
            writer.Write( (int)1 );
            writer.Write( m_Charges );
            writer.Write( m_Skill_1 ); writer.Write( m_Skill_2 ); writer.Write( m_Skill_3 );
            writer.Write( m_Skill_4 ); writer.Write( m_Skill_5 );
            writer.Write( m_Value_1 ); writer.Write( m_Value_2 ); writer.Write( m_Value_3 );
            writer.Write( m_Value_4 ); writer.Write( m_Value_5 );
            writer.Write( m_Slayer_1 ); writer.Write( m_Slayer_2 );
            writer.Write( m_Owner );
            writer.Write( m_Extra );
            writer.Write( m_FromWho );
            writer.Write( m_HowGiven );
            writer.Write( m_Points );
            writer.Write( m_Hue );
        }

        public override void Deserialize( GenericReader reader )
        {
            base.Deserialize( reader );
            int version = reader.ReadInt();
            m_Charges  = reader.ReadInt();
            m_Skill_1  = reader.ReadInt(); m_Skill_2  = reader.ReadInt(); m_Skill_3  = reader.ReadInt();
            m_Skill_4  = reader.ReadInt(); m_Skill_5  = reader.ReadInt();
            m_Value_1  = reader.ReadDouble(); m_Value_2 = reader.ReadDouble(); m_Value_3 = reader.ReadDouble();
            m_Value_4  = reader.ReadDouble(); m_Value_5 = reader.ReadDouble();
            m_Slayer_1 = reader.ReadInt(); m_Slayer_2 = reader.ReadInt();
            m_Owner    = reader.ReadMobile();
            m_Extra    = reader.ReadString();
            m_FromWho  = reader.ReadString();
            m_HowGiven = reader.ReadString();
            m_Points   = reader.ReadInt();
            m_Hue      = reader.ReadInt();
            if ( ItemID != 0x1C0E && ItemID != 0x1C0F ) ItemID = Utility.RandomList( 0x1C0E, 0x1C0F );
            if ( Name != null && Name.Contains( "Tome " ) ) Name = Name.Replace( "Tome ", "Chest " );
        }
    }
}