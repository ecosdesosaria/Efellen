using System;
using System.Collections;
using Server;
using Server.Gumps;
using Server.Items;
using Server.Misc;
using Server.Network;

namespace Server.Items
{
    public enum ArtifactCategory
    {
        OneHandedWeapons = 0,
        TwoHandedWeapons = 1,
        RangedWeapons = 2,
        JewelryTrinkets = 3,
        ArmorShields = 4,
        Clothing = 5
    }

    public class ArtifactEntry
    {
        public string TypeName;
        public string DisplayName;
        public ArtifactCategory Category;

        public ArtifactEntry(string typeName, string displayName, ArtifactCategory category)
        {
            TypeName = typeName;
            DisplayName = displayName;
            Category = category;
        }
    }

    // =========================================================================
    public class SearchBook : Item
    {
        public static readonly ArtifactEntry[] AllArtifacts = new ArtifactEntry[]
        {
            // ----- One-Handed Weapons -----
            new ArtifactEntry( "Artifact_BladeDance",                 "Blade Dance",                       ArtifactCategory.OneHandedWeapons ),
            new ArtifactEntry( "Artifact_BladeOfInsanity",            "Blade of Insanity",                 ArtifactCategory.OneHandedWeapons ),
            new ArtifactEntry( "Artifact_ConansSword",                "Blade of the Cimmerian",            ArtifactCategory.OneHandedWeapons ),
            new ArtifactEntry( "Artifact_BladeOfTheWilds",            "Blade of the Wilds",                ArtifactCategory.OneHandedWeapons ),
            new ArtifactEntry( "Artifact_BlazeOfDeath",               "Blaze of Death",                    ArtifactCategory.OneHandedWeapons ),
            new ArtifactEntry( "Artifact_CaptainQuacklebushsCutlass", "Captain Quacklebush's Cutlass",     ArtifactCategory.OneHandedWeapons ),
            new ArtifactEntry( "Artifact_ColdBlood",                  "Cold Blood",                        ArtifactCategory.OneHandedWeapons ),
            new ArtifactEntry( "Artifact_ColdForgedBlade",            "Cold Forged Blade",                 ArtifactCategory.OneHandedWeapons ),
            new ArtifactEntry( "Artifact_DaggerOfVenom",              "Dagger of Venom",                   ArtifactCategory.OneHandedWeapons ),
            new ArtifactEntry( "Artifact_FangOfRactus",               "Fang of Ractus",                    ArtifactCategory.OneHandedWeapons ),
            new ArtifactEntry( "Artifact_FleshRipper",                "Flesh Ripper",                      ArtifactCategory.OneHandedWeapons ),
            new ArtifactEntry( "Artifact_Fury",                       "Fury",                              ArtifactCategory.OneHandedWeapons ),
            new ArtifactEntry( "Artifact_GiantBlackjack",             "Giant Blackjack",                   ArtifactCategory.OneHandedWeapons ),
            new ArtifactEntry( "Artifact_JadeScimitar",               "Jade Scimitar",                     ArtifactCategory.OneHandedWeapons ),
            new ArtifactEntry( "Artifact_LuminousRuneBlade",          "Luminous Rune Blade",               ArtifactCategory.OneHandedWeapons ),
            new ArtifactEntry( "Artifact_MelisandesCorrodedHatchet",  "Melisande's Corroded Hatchet",      ArtifactCategory.OneHandedWeapons ),
            new ArtifactEntry( "Artifact_OblivionsNeedle",            "Oblivion Needle",                   ArtifactCategory.OneHandedWeapons ),
            new ArtifactEntry( "Artifact_Pacify",                     "Pacify",                            ArtifactCategory.OneHandedWeapons ),
            new ArtifactEntry( "Artifact_PixieSwatter",               "Pixie Swatter",                     ArtifactCategory.OneHandedWeapons ),
            new ArtifactEntry( "Artifact_RaedsGlory",                 "Raed's Glory",                      ArtifactCategory.OneHandedWeapons ),
            new ArtifactEntry( "Artifact_Retort",                     "Retort",                            ArtifactCategory.OneHandedWeapons ),
            new ArtifactEntry( "Artifact_RuneCarvingKnife",           "Rune Carving Knife",                ArtifactCategory.OneHandedWeapons ),
            new ArtifactEntry( "Artifact_ShardThrasher",              "Shard Thrasher",                    ArtifactCategory.OneHandedWeapons ),
            new ArtifactEntry( "Artifact_SoulSeeker",                 "Soul Seeker",                       ArtifactCategory.OneHandedWeapons ),
            new ArtifactEntry( "Artifact_GlassSword",                 "Sword of Shattered Hopes",          ArtifactCategory.OneHandedWeapons ),
            new ArtifactEntry( "Artifact_TalonBite",                  "Talon Bite",                        ArtifactCategory.OneHandedWeapons ),
            new ArtifactEntry( "Artifact_VampireKiller",              "Vampire Killer",                    ArtifactCategory.OneHandedWeapons ),
            new ArtifactEntry( "Artifact_ZyronicClaw",                "Zyronic Claw",                      ArtifactCategory.OneHandedWeapons ),
            new ArtifactEntry( "Artifact_UgmarLastWord",              "Ugmar's Last Word",                 ArtifactCategory.OneHandedWeapons ),
            new ArtifactEntry( "Artifact_ChainBreaker",               "Chain Breaker",                     ArtifactCategory.OneHandedWeapons ),
            new ArtifactEntry( "Artifact_SenseisWalkingStick",        "Sensei's Walking Stick",            ArtifactCategory.OneHandedWeapons ),
            new ArtifactEntry( "Artifact_MinersPickaxe",              "Miner's Pickaxe",                   ArtifactCategory.OneHandedWeapons ),
            new ArtifactEntry( "Artifact_BreathOfTheDead",            "Breath of the Dead",                ArtifactCategory.OneHandedWeapons ),
            new ArtifactEntry( "Artifact_AngeroftheGods",             "Anger of the Gods",                 ArtifactCategory.OneHandedWeapons ),
            new ArtifactEntry( "Artifact_RodOfResurrection",          "Rod Of Resurrection",               ArtifactCategory.OneHandedWeapons ),
            new ArtifactEntry( "Artifact_Stormbringer",               "Stormbringer",                      ArtifactCategory.OneHandedWeapons ),
            new ArtifactEntry( "Artifact_ScepterOfBlasting",          "Scepter of Blasting",               ArtifactCategory.OneHandedWeapons ),
            new ArtifactEntry( "Artifact_WhistleofthePiper",          "Whistle of the Pied Piper",         ArtifactCategory.OneHandedWeapons ),
            new ArtifactEntry( "Artifact_WintersGrip",                "Arctic Death Dealer",               ArtifactCategory.OneHandedWeapons ),
            new ArtifactEntry( "Artifact_EnchantedTitanLegBone",      "Enchanted Pirate Rapier",           ArtifactCategory.OneHandedWeapons ),
            new ArtifactEntry( "Artifact_MelodyOfTriumph",            "Enchanted Pirate Rapier",                 ArtifactCategory.OneHandedWeapons ),
         
		    // ----- Two-Handed Weapons -----
			new ArtifactEntry( "Artifact_Calm",                       "Calm",                              ArtifactCategory.TwoHandedWeapons ),
            new ArtifactEntry( "Artifact_Annihilation",               "Annihilation",                      ArtifactCategory.TwoHandedWeapons ),
            new ArtifactEntry( "Artifact_SpellBreaker",               "Spellbreaker",                      ArtifactCategory.TwoHandedWeapons ),
            new ArtifactEntry( "Artifact_SpiritBreaker",              "Spiritbreaker",                     ArtifactCategory.TwoHandedWeapons ),
            new ArtifactEntry( "Artifact_EndOfHope",                  "End of Hope",                       ArtifactCategory.TwoHandedWeapons ),
            new ArtifactEntry( "Artifact_AchillesSpear",              "Achille's Spear",                   ArtifactCategory.TwoHandedWeapons ),
            new ArtifactEntry( "Artifact_AxeOfTheHeavens",            "Axe of the Heavens",                ArtifactCategory.TwoHandedWeapons ),
            new ArtifactEntry( "Artifact_AxeoftheMinotaur",           "Axe of the Minotaur",               ArtifactCategory.TwoHandedWeapons ),
            new ArtifactEntry( "Artifact_TheBeserkersMaul",           "Berserker's Maul",                  ArtifactCategory.TwoHandedWeapons ),
            new ArtifactEntry( "Artifact_BoneCrusher",                "Bone Crusher",                      ArtifactCategory.TwoHandedWeapons ),
            new ArtifactEntry( "Artifact_Boomstick",                  "Boomstick",                         ArtifactCategory.TwoHandedWeapons ),
            new ArtifactEntry( "Artifact_Excalibur",                  "Excalibur",                         ArtifactCategory.TwoHandedWeapons ),
            new ArtifactEntry( "Artifact_FortunateBlades",            "Fortunate Blades",                  ArtifactCategory.TwoHandedWeapons ),
            new ArtifactEntry( "Artifact_VampiricDaisho",             "Vampiric Daisho",                   ArtifactCategory.TwoHandedWeapons ),
            new ArtifactEntry( "Artifact_DarkLordsPitchfork",         "Dark Lord's PitchFork",             ArtifactCategory.TwoHandedWeapons ),
            new ArtifactEntry( "Artifact_GrimReapersScythe",          "Grim Reaper's Scythe",              ArtifactCategory.TwoHandedWeapons ),
            new ArtifactEntry( "Artifact_HammerofThor",               "Hammer of Thor",                    ArtifactCategory.TwoHandedWeapons ),
            new ArtifactEntry( "Artifact_HolyLance",                  "Holy Lance",                        ArtifactCategory.TwoHandedWeapons ),
            new ArtifactEntry( "Artifact_KamiNarisIndestructableDoubleAxe", "Kami-Naris Indestructable Axe", ArtifactCategory.TwoHandedWeapons ),
            new ArtifactEntry( "Artifact_MaulOfTheTitans",            "Maul of the Titans",                ArtifactCategory.TwoHandedWeapons ),
            new ArtifactEntry( "Artifact_TheDragonSlayer",            "Slayer of Dragons",                 ArtifactCategory.TwoHandedWeapons ),
            new ArtifactEntry( "Artifact_StaffOfPower",               "Staff of Power",                    ArtifactCategory.TwoHandedWeapons ),
            new ArtifactEntry( "Artifact_StaffOfTheMagi",             "Staff of the Magi",                 ArtifactCategory.TwoHandedWeapons ),
            new ArtifactEntry( "Artifact_StaffoftheWoodlands",        "Staff of the Woodlands",            ArtifactCategory.TwoHandedWeapons ),
            new ArtifactEntry( "Artifact_StaffOfTheWyrmSpeaker",      "Merlin's Mystical Staff",           ArtifactCategory.TwoHandedWeapons ),
            new ArtifactEntry( "Artifact_TitansHammer",               "Titan's Hammer",                    ArtifactCategory.TwoHandedWeapons ),
            new ArtifactEntry( "Artifact_WrathOfTheDryad",            "Wrath of the Dryad",                ArtifactCategory.TwoHandedWeapons ),
            new ArtifactEntry( "Artifact_StaffOfBlasting",            "Staff of Blasting",                 ArtifactCategory.TwoHandedWeapons ),
            new ArtifactEntry( "Artifact_SerpentCoil",                "Serpent's Coil",                    ArtifactCategory.TwoHandedWeapons ),
            new ArtifactEntry( "Artifact_MemoryOfFrost",              "Memory of Frost",                   ArtifactCategory.TwoHandedWeapons ),
            new ArtifactEntry( "Artifact_TyrantOfTheReefs",           "Tyrant of the Reefs",               ArtifactCategory.TwoHandedWeapons ),
            new ArtifactEntry( "Artifact_MagiciansIllusion",          "Magician's Illusion",               ArtifactCategory.TwoHandedWeapons ),
            new ArtifactEntry( "Artifact_EdgeOfDawn",                 "Edge of Dawn",                      ArtifactCategory.TwoHandedWeapons ),
            new ArtifactEntry( "Artifact_EdgeOfDusk",                 "Edge of Dusk",                      ArtifactCategory.TwoHandedWeapons ),
            new ArtifactEntry( "Artifact_JestersVow",                 "Jester's Vow",                      ArtifactCategory.TwoHandedWeapons ),
            new ArtifactEntry( "Artifact_AncestralKakitaDaisho",      "Ancestral Kakita Daisho",           ArtifactCategory.TwoHandedWeapons ),

            // ----- Ranged Weapons & quivers -----
			new ArtifactEntry( "Artifact_WidowsWhistle",              "Widow's Whistle",                   ArtifactCategory.RangedWeapons ),
            new ArtifactEntry( "Artifact_TheNightReaper",             "Night Reaper",                      ArtifactCategory.RangedWeapons ),
            new ArtifactEntry( "Artifact_Windsong",                   "Windsong",                          ArtifactCategory.RangedWeapons ),
            new ArtifactEntry( "Artifact_Frostbringer",               "Frostbringer",                      ArtifactCategory.RangedWeapons ),
            new ArtifactEntry( "Artifact_WildfireBow",                "Wildfire Bow",                      ArtifactCategory.RangedWeapons ),
            new ArtifactEntry( "Artifact_BowOfTheTribalKing",         "Bow of the Juka King",              ArtifactCategory.RangedWeapons ),
            new ArtifactEntry( "Artifact_BowofthePhoenix",            "Bow of the Phoenix",                ArtifactCategory.RangedWeapons ),
            new ArtifactEntry( "Artifact_BowOfTheProwler",            "Bow of the Prowler",                ArtifactCategory.RangedWeapons ),
            new ArtifactEntry( "Artifact_TheDryadBow",                "Dryad Bow",                         ArtifactCategory.RangedWeapons ),
            new ArtifactEntry( "Artifact_LongShot",                   "Long Shot",                         ArtifactCategory.RangedWeapons ),
            new ArtifactEntry( "Artifact_NoxBow",                     "Nox Ranger's Light Crossbow",       ArtifactCategory.RangedWeapons ),
            new ArtifactEntry( "Artifact_NoxRangersHeavyCrossbow",    "Nox Ranger's Heavy Crossbow",       ArtifactCategory.RangedWeapons ),
            new ArtifactEntry( "Artifact_ReachOfTheDepths",           "Reach of the Depths",               ArtifactCategory.RangedWeapons ),
            new ArtifactEntry( "QuiverOfBlight",                      "Quiver of Blight",                  ArtifactCategory.RangedWeapons ),
            new ArtifactEntry( "QuiverOfFire",                        "Quiver of Fire",                    ArtifactCategory.RangedWeapons ),
            new ArtifactEntry( "QuiverOfIce",                         "Quiver of Ice",                     ArtifactCategory.RangedWeapons ),
            new ArtifactEntry( "QuiverOfInfinity",                    "Quiver of Infinity",                ArtifactCategory.RangedWeapons ),
            new ArtifactEntry( "QuiverOfLightning",                   "Quiver of Lightning",               ArtifactCategory.RangedWeapons ),
            new ArtifactEntry( "QuiverOfRage",                        "Quiver of Rage",                    ArtifactCategory.RangedWeapons ),
            new ArtifactEntry( "QuiverOfElements",                    "Quiver of the Elements",            ArtifactCategory.RangedWeapons ),
            new ArtifactEntry( "Artifact_Pestilence",                 "Pestilence",                        ArtifactCategory.RangedWeapons ),
            // ----- Jewelry & Trinkets -----
			new ArtifactEntry( "Artifact_TotemOfVoid",                "Totem of the Void",                 ArtifactCategory.JewelryTrinkets ),
            new ArtifactEntry( "Artifact_EssenceOfBattle",            "Essence of Battle",                 ArtifactCategory.JewelryTrinkets ),
            new ArtifactEntry( "HornOfKingTriton",                    "Horn of King Triton",               ArtifactCategory.JewelryTrinkets ),
            new ArtifactEntry( "IolosLute",                           "Iolo's Lute",                       ArtifactCategory.JewelryTrinkets ),
            new ArtifactEntry( "GwennosHarp",                         "Gwenno's Harp",                     ArtifactCategory.JewelryTrinkets ),
            new ArtifactEntry( "Artifact_AlchemistsBauble",           "Alchemist's Bauble",                ArtifactCategory.JewelryTrinkets ),
            new ArtifactEntry( "Artifact_BraceletOfHealth",           "Bracelet of Health",                ArtifactCategory.JewelryTrinkets ),
            new ArtifactEntry( "Artifact_BraceletOfTheElements",      "Bracelet of the Elements",          ArtifactCategory.JewelryTrinkets ),
            new ArtifactEntry( "Artifact_BraceletOfTheVile",          "Bracelet of the Vile",              ArtifactCategory.JewelryTrinkets ),
            new ArtifactEntry( "Artifact_CrimsonCincture",            "Crimson Cincture",                  ArtifactCategory.JewelryTrinkets ),
            new ArtifactEntry( "Artifact_DjinnisRing",                "Djinni's Ring",                     ArtifactCategory.JewelryTrinkets ),
            new ArtifactEntry( "Artifact_EarringsOfHealth",           "Earrings of Health",                ArtifactCategory.JewelryTrinkets ),
            new ArtifactEntry( "Artifact_EarringsOfTheElements",      "Earrings of the Elements",          ArtifactCategory.JewelryTrinkets ),
            new ArtifactEntry( "Artifact_EarringsOfTheMagician",      "Earrings of the Magician",          ArtifactCategory.JewelryTrinkets ),
            new ArtifactEntry( "Artifact_EarringsOfTheVile",          "Earrings of the Vile",              ArtifactCategory.JewelryTrinkets ),
            new ArtifactEntry( "Artifact_LuckyEarrings",              "Lucky Earrings",                    ArtifactCategory.JewelryTrinkets ),
            new ArtifactEntry( "Artifact_LuckyNecklace",              "Lucky Necklace",                    ArtifactCategory.JewelryTrinkets ),
            new ArtifactEntry( "Artifact_MagesBand",                  "Mage's Band",                       ArtifactCategory.JewelryTrinkets ),
            new ArtifactEntry( "Artifact_NoxNightlight",              "Nox Nightlight",                    ArtifactCategory.JewelryTrinkets ),
            new ArtifactEntry( "Artifact_OrnamentOfTheMagician",      "Ornament of the Magician",          ArtifactCategory.JewelryTrinkets ),
            new ArtifactEntry( "Artifact_PendantOfTheMagi",           "Pendant of the Magi",               ArtifactCategory.JewelryTrinkets ),
            new ArtifactEntry( "Artifact_PowerSurge",                 "Lantern of Power",                  ArtifactCategory.JewelryTrinkets ),
            new ArtifactEntry( "Artifact_ResilientBracer",            "Resillient Bracer",                 ArtifactCategory.JewelryTrinkets ),
            new ArtifactEntry( "Artifact_RingOfHealth",               "Ring of Health",                    ArtifactCategory.JewelryTrinkets ),
            new ArtifactEntry( "Artifact_RingOfProtection",           "Ring of Protection",                ArtifactCategory.JewelryTrinkets ),
            new ArtifactEntry( "Artifact_RingOfTheElements",          "Ring of the Elements",              ArtifactCategory.JewelryTrinkets ),
            new ArtifactEntry( "Artifact_RingOfTheMagician",          "Ring of the Magician",              ArtifactCategory.JewelryTrinkets ),
            new ArtifactEntry( "Artifact_RingOfTheVile",              "Ring of the Vile",                  ArtifactCategory.JewelryTrinkets ),
            new ArtifactEntry( "Artifact_ShimmeringTalisman",         "Shimmering Talisman",               ArtifactCategory.JewelryTrinkets ),
            new ArtifactEntry( "Artifact_SpiritOfTheTotem",           "Spirit of the Totem",               ArtifactCategory.JewelryTrinkets ),
            new ArtifactEntry( "Artifact_TalismanOfTheAlbatroz",      "Talisman of the Albatroz",          ArtifactCategory.JewelryTrinkets ),
            new ArtifactEntry( "Artifact_TorchOfTrapFinding",         "Torch of Trap Burning",             ArtifactCategory.JewelryTrinkets ),
            new ArtifactEntry( "Artifact_WarriorsClasp",              "Warrior's Clasp",                   ArtifactCategory.JewelryTrinkets ),
            new ArtifactEntry( "Artifact_EternalFlame",               "Eternal Flame",                     ArtifactCategory.JewelryTrinkets ),
            new ArtifactEntry( "Artifact_GrimReapersLantern",         "Grim Reaper's Lantern",             ArtifactCategory.JewelryTrinkets ),
            new ArtifactEntry( "Artifact_EarringsOfAllurement",       "Earrings of Allurement",            ArtifactCategory.JewelryTrinkets ),
            new ArtifactEntry( "Artifact_RingOfAllurement",           "Ring of Allurement",                ArtifactCategory.JewelryTrinkets ),
            new ArtifactEntry( "Artifact_NecklaceOfAllurement",       "Necklace of Allurement",            ArtifactCategory.JewelryTrinkets ),
            new ArtifactEntry( "Artifact_BeltOfHaste",                "Belt of Haste",                     ArtifactCategory.JewelryTrinkets ),
            new ArtifactEntry( "Artifact_BeltofGiantsStrength",       "Belt of Giant's Strength",          ArtifactCategory.JewelryTrinkets ),
            new ArtifactEntry( "Artifact_BeltofHercules",             "Belt of Hercules",                  ArtifactCategory.JewelryTrinkets ),
            new ArtifactEntry( "Artifact_RememberanceOfHereafter",    "Rememberance of Hereafter",         ArtifactCategory.JewelryTrinkets ),
            new ArtifactEntry( "Artifact_ShacklesOfBhaal",            "Shackles of Bhaal",                 ArtifactCategory.JewelryTrinkets ),
            new ArtifactEntry( "Arty_LithosTome",                     "Tome of the Mountain King",         ArtifactCategory.JewelryTrinkets ),
            new ArtifactEntry( "Arty_HydrosLexicon",                  "Lexicon of the Lurker",             ArtifactCategory.JewelryTrinkets ),
            new ArtifactEntry( "Arty_StratosManual",                  "Manual of the Mystic Voice",        ArtifactCategory.JewelryTrinkets ),
            new ArtifactEntry( "Arty_OssianGrimoire",                 "Ossian Grimoire",                   ArtifactCategory.JewelryTrinkets ),
            new ArtifactEntry( "Arty_PyrosGrimoire",                  "Grimoire of the Daemon King",       ArtifactCategory.JewelryTrinkets ),
            new ArtifactEntry( "Arty_BookOfKnowledge",                "Book Of Knowledge",                 ArtifactCategory.JewelryTrinkets ),
            new ArtifactEntry( "Artifact_CandleCold",                 "Candle of Cold Light",              ArtifactCategory.JewelryTrinkets ),
            new ArtifactEntry( "Artifact_CandleEnergy",               "Candle of Energized Light",         ArtifactCategory.JewelryTrinkets ),
            new ArtifactEntry( "Artifact_CandleFire",                 "Candle of Fire Light",              ArtifactCategory.JewelryTrinkets ),
            new ArtifactEntry( "Artifact_CandleNecromancer",          "Candle of Ghostly Light",           ArtifactCategory.JewelryTrinkets ),
            new ArtifactEntry( "Artifact_CandlePoison",               "Candle of Poisonous Light",         ArtifactCategory.JewelryTrinkets ),
            new ArtifactEntry( "Artifact_CandleWizard",               "Candle of Wizardly Light",          ArtifactCategory.JewelryTrinkets ),
            new ArtifactEntry( "Artifact_AuraOfShadows",              "Aura Of Shadows",                   ArtifactCategory.JewelryTrinkets ),
            new ArtifactEntry( "Artifact_BloodwoodSpirit",            "Bloodwood Spirit",                  ArtifactCategory.JewelryTrinkets ),
            new ArtifactEntry( "Artifact_ArcticBeacon",               "Winter Beacon",                     ArtifactCategory.JewelryTrinkets ),
            new ArtifactEntry( "Artifact_ThievesTorment",             "Thieves' Torment",                  ArtifactCategory.JewelryTrinkets ),
            new ArtifactEntry("Artifact_AmuletOfFanaedar",            "Amulet of Fanaedar",                ArtifactCategory.JewelryTrinkets ),
            // ----- Armor & Shields -----
			new ArtifactEntry( "Artifact_DupresCollar",               "Dupre's Collar",                    ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_GladiatorsCollar",           "Gladiator's Collar",                ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_MidnightBracers",            "Midnight Bracers",                  ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_SongWovenMantle",            "Song Woven Mantle",                 ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_Indecency",                  "Indecency",                         ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_InquisitorsResolution",      "Inquisitor's Resolution",           ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_VioletCourage",              "Violet Courage",                    ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_GuantletsOfOgreStrength",    "Gauntlets of Anger",                ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_AbysmalGloves",              "Abysmal Gloves",                    ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_AchillesShield",             "Achille's Shield",                  ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_Aegis",                      "Aegis",                             ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_AegisOfGrace",               "Aegis of Grace",                    ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_ArcaneArms",                 "Arcane Arms",                       ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_ArcaneCap",                  "Arcane Cap",                        ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_ArcaneGloves",               "Arcane Gloves",                     ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_ArcaneGorget",               "Arcane Gorget",                     ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_ArcaneLeggings",             "Arcane Leggings",                   ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_ArcaneShield",               "Arcane Shield",                     ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_ArcaneTunic",                "Arcane Tunic",                      ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_ArmorOfFortune",             "Armor of Fortune",                  ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_ArmorOfInsight",             "Armor of Insight",                  ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_ArmorOfNobility",            "Armor of Nobility",                 ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_ArmsOfAegis",                "Arms of Aegis",                     ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_ArmsOfFortune",              "Arms of Fortune",                   ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_ArmsOfInsight",              "Arms of Insight",                   ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_ArmsOfNobility",             "Arms of Nobility",                  ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_ArmsOfTheFallenKing",        "Arms of the Fallen King",           ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_ArmsOfTheHarrower",          "Arms of the Harrower",              ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_ArmsOfToxicity",             "Arms Of Toxicity",                  ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_CapOfFortune",               "Cap of Fortune",                    ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_CapOfTheFallenKing",         "Cap of the Fallen King",            ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_CoifOfBane",                 "Coif of Bane",                      ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_CoifOfFire",                 "Coif of Fire",                      ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_CrownOfTalKeesh",            "Crown of Tal'Keesh",                ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_CrownOfBrillance",           "Crown of Brillance",                ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_DarkGuardiansChest",         "Dark Guardian's Chest",             ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_DarkNeck",                   "Dark Neck",                         ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_DivineArms",                 "Divine Arms",                       ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_DivineGloves",               "Divine Gloves",                     ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_DivineGorget",               "Divine Gorget",                     ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_DivineLeggings",             "Divine Leggings",                   ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_DivineTunic",                "Divine Tunic",                      ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_DupresShield",               "Dupre's Shield",                    ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_EvilMageGloves",             "Evil Mage Gloves",                  ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_FeyLeggings",                "Fey Leggings",                      ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_Fortifiedarms",              "Fortified Arms",                    ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_MarbleShield",               "Gargoyle Shield",                   ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_GauntletsOfNobility",        "Gauntlets of Nobility",             ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_GlovesOfAegis",              "Gloves of Aegis",                   ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_GlovesOfCorruption",         "Gloves Of Corruption",              ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_GlovesOfDexterity",          "Gloves of Dexterity",               ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_GlovesOfFortune",            "Gloves of Fortune",                 ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_GlovesOfInsight",            "Gloves of Insight",                 ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_GlovesOfRegeneration",       "Gloves Of Regeneration",            ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_GlovesOfTheFallenKing",      "Gloves of the Fallen King",         ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_GlovesOfTheHarrower",        "Gloves of the Harrower",            ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_GlovesOfThePugilist",        "Gloves of the Pugilist",            ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_GorgetOfAegis",              "Gorget of Aegis",                   ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_GorgetOfFortune",            "Gorget of Fortune",                 ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_GorgetOfInsight",            "Gorget of Insight",                 ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_HeartOfTheLion",             "Heart of the Lion",                 ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_HellForgedArms",             "Hell Forged Arms",                  ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_HelmOfAegis",                "Helm of Aegis",                     ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_HelmOfBrilliance",           "Helm of Brilliance",                ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_HelmOfInsight",              "Helm of Insight",                   ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_HelmOfSwiftness",            "Helm of Swiftness",                 ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_ConansHelm",                 "Helm of the Cimmerian",             ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_HolyKnightsArmPlates",       "Holy Knight's Arm Plates",          ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_HolyKnightsBreastplate",     "Holy Knight's Breastplate",         ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_HolyKnightsGloves",          "Holy Knight's Gloves",              ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_HolyKnightsGorget",          "Holy Knight's Gorget",              ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_HolyKnightsLegging",         "Holy Knight's Legging",             ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_HolyKnightsPlateHelm",       "Holy Knight's Plate Helm",          ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_HuntersArms",                "Hunter's Arms",                     ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_HuntersGloves",              "Hunter's Gloves",                   ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_HuntersGorget",              "Hunter's Gorget",                   ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_HuntersHeaddress",           "Hunter's Headdress",                ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_HuntersLeggings",            "Hunter's Leggings",                 ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_HuntersTunic",               "Hunter's Tunic",                    ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_InquisitorsArms",            "Inquisitor's Arms",                 ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_InquisitorsGorget",          "Inquisitor's Gorget",               ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_InquisitorsHelm",            "Inquisitor's Helm",                 ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_InquisitorsLeggings",        "Inquisitor's Leggings",             ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_InquisitorsTunic",           "Inquisitor's Tunic",                ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_IronwoodCrown",              "Ironwood Crown",                    ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_JackalsArms",                "Jackal's Arms",                     ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_JackalsCollar",              "Jackal's Collar",                   ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_JackalsGloves",              "Jackal's Gloves",                   ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_JackalsHelm",                "Jackal's Helm",                     ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_JackalsLeggings",            "Jackal's Leggings",                 ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_JackalsTunic",               "Jackal's Tunic",                    ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_KodiakBearMask",             "Kodiak Bear Mask",                  ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_LegsOfFortune",              "Legging of Fortune",                ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_LegsOfInsight",              "Legging of Insight",                ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_LeggingsOfAegis",            "Leggings of Aegis",                 ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_LeggingsOfBane",             "Leggings of Bane",                  ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_LeggingsOfDeceit",           "Leggings Of Deceit",                ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_LeggingsOfEnlightenment",    "Leggings Of Enlightenment",         ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_LeggingsOfFire",             "Leggings of Fire",                  ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_LegsOfTheFallenKing",        "Leggings of the Fallen King",       ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_LegsOfTheHarrower",          "Leggings of the Harrower",          ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_LegsOfNobility",             "Legs of Nobility",                  ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_DeathsMask",                 "Mask of Death",                     ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_MidnightGloves",             "Midnight Gloves",                   ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_MidnightHelm",               "Midnight Helm",                     ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_MidnightLegs",               "Midnight Leggings",                 ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_MidnightTunic",              "Midnight Tunic",                    ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_NatureVengeanceMask",        "Mask of Natural Vengeance",         ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_NatureVengeanceArms",        "Arms of Natural Vengeance",         ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_NatureVengeanceGloves",      "Gloves of Natural Vengeance",       ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_NatureVengeanceLeggings",    "Leggings of Natural Vengeance",     ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_NatureVengeanceGorget",      "Nature's Vengeance Gorget",         ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_OrcChieftainHelm",           "Orc Chieftain Helm",                ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_OrcishVisage",               "Orcish Visage",                     ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_OrnateCrownOfTheHarrower",   "Ornate Crown of the Harrower",      ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_ProwleroftheWildsLegging",   "Leggings of the Prowler",           ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_ProwleroftheWildsHelmet",    "Mask of the Prowler",               ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_ProwleroftheWildsGloves",    "Gloves of the Prowler",             ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_ProwleroftheWildsTunic",     "Tunic of the Prowler",              ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_ProwleroftheWildsArms",      "Arms of the Prowler",               ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_ProtectoroftheWildsChestplate", "Chestplate of the Wilds",        ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_ProtectoroftheWildsLeggings","Leggings of the Wilds",             ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_ProtectoroftheWildsGloves",  "Gloves of the Wilds",               ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_ProtectoroftheWildsArms",    "Arms of the Wilds",                 ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_ProtectoroftheWildsHelmet",  "Helmet of the Wilds",               ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_ProtectoroftheWildsGorget",  "Gorget of the Protector of the Wilds", ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_NatureMasterGorget",         "Nature Master's Gorget",            ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_RoyalGuardsGorget",          "Royal Guardian's Gorget",           ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_RoyalGuardsChestplate",      "Royal Guard's Chest Plate",         ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_LeggingsOfEmbers",           "Royal Leggings of Embers",          ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_SamuraiHelm",                "Ancient Samurai Helm",              ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_ShadowDancerArms",           "Shadow Dancer Arms",                ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_ShadowDancerCap",            "Shadow Dancer Cap",                 ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_ShadowDancerGloves",         "Shadow Dancer Gloves",              ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_ShadowDancerGorget",         "Shadow Dancer Gorget",              ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_ShadowDancerLeggings",       "Shadow Dancer Leggings",            ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_ShadowDancerTunic",          "Shadow Dancer Tunic",               ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_ShroudOfDeciet",             "Shroud of Deceit",                  ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_ShieldOfInvulnerability",    "Shield of Invulnerability",         ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_ShieldOfAmaunator",          "Shield of Amaunator",               ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_StormKingsShield",           "Storm King's Shield",               ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_StitchersMittens",           "Stitcher's Mittens",                ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_TotemArms",                  "Totem Arms",                        ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_TotemGloves",                "Totem Gloves",                      ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_TotemGorget",                "Totem Gorget",                      ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_TotemLeggings",              "Totem Leggings",                    ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_TotemTunic",                 "Totem Tunic",                       ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_TunicOfAegis",               "Tunic of Aegis",                    ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_TunicOfBane",                "Tunic of Bane",                     ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_TunicOfFire",                "Tunic of Fire",                     ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_TunicOfTheFallenKing",       "Tunic of the Fallen King",          ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_TunicOfTheHarrower",         "Tunic of the Harrower",             ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_VoiceOfTheFallenKing",       "Voice of the Fallen King",          ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_YashimotosHatsuburi",        "Yashimoto's Hatsuburi",             ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_GlovesOfThePiper",           "Gloves of the Pied Piper",          ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_SpellWovenBritches",         "Spell Woven Britches",              ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_AngelicEmbrace",             "Angelic Embrace",                   ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_BrambleCoat",                "Bramble Coat",                      ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_NatureVengeanceCoat",        "Coat of Natural Vengeance",         ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_WizardsPants",               "Wizard's Pants",                    ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_BayushisHonestSmile",        "Bayushi's Honest Smile",            ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_AkodoPrideHiroSode",         "Akodo's Pride Hiro Sode",           ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_AkodoPridePlateKabuto",      "Akodo's Pride Plate Kabuto",        ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_AkodoPrideSuneate",          "Akodo's Pride Suneate",             ArtifactCategory.ArmorShields ),
            new ArtifactEntry( "Artifact_AkodoPridePlateDo",          "Akodo's Pride Plate Do",            ArtifactCategory.ArmorShields ),
            // ----- Clothing -----
			new ArtifactEntry( "Artifact_EmbroideredOakLeafCloak",    "Embroidered Oak Leaf Cloak",        ArtifactCategory.Clothing ),
            new ArtifactEntry( "Artifact_CircletOfTheSorceress",      "Circlet Of The Sorceress",          ArtifactCategory.Clothing ),
            new ArtifactEntry( "Artifact_DetectiveBoots",             "Detective Boots of the Royal Guard", ArtifactCategory.Clothing ),
            new ArtifactEntry( "Artifact_DivineCountenance",          "Divine Countenance",                ArtifactCategory.Clothing ),
            new ArtifactEntry( "Artifact_HatOfTheMagi",               "Hat of the Magi",                   ArtifactCategory.Clothing ),
            new ArtifactEntry( "Artifact_MagiciansMempo",             "Magician's Mempo",                  ArtifactCategory.Clothing ),
            new ArtifactEntry( "Artifact_PadsOfTheCuSidhe",           "Pads of the Cu Sidhe",              ArtifactCategory.Clothing ),
            new ArtifactEntry( "Artifact_PolarBearBoots",             "Polar Bear Boots",                  ArtifactCategory.Clothing ),
            new ArtifactEntry( "Artifact_GeishasObi",                 "Geishas Obi",                       ArtifactCategory.Clothing ),
            new ArtifactEntry( "Artifact_JesterHatofChuckles",        "Jester Hat of Chuckles",            ArtifactCategory.Clothing ),
            new ArtifactEntry( "Artifact_AcidProofRobe",              "Acidic Robe",                       ArtifactCategory.Clothing ),
            new ArtifactEntry( "Artifact_ArcanicRobe",                "Arcanic Robe",                      ArtifactCategory.Clothing ),
            new ArtifactEntry( "Artifact_BeggarsRobe",                "Beggar's Robe",                     ArtifactCategory.Clothing ),
            new ArtifactEntry( "Artifact_BootsofHermes",              "Boots of Hermes",                   ArtifactCategory.Clothing ),
            new ArtifactEntry( "Artifact_BootsofPyros",               "Boots of the Daemon King",          ArtifactCategory.Clothing ),
            new ArtifactEntry( "Artifact_BootsofHydros",              "Boots of the Lurker",               ArtifactCategory.Clothing ),
            new ArtifactEntry( "Artifact_BootsofLithos",              "Boots of the Mountain King",        ArtifactCategory.Clothing ),
            new ArtifactEntry( "Artifact_BootsofStratos",             "Boots of the Mystic Voice",         ArtifactCategory.Clothing ),
            new ArtifactEntry( "Artifact_BootsOfThePiper",            "Boots of the Pied Piper",           ArtifactCategory.Clothing ),
            new ArtifactEntry( "Artifact_BurglarsBandana",            "Burglar's Bandana",                 ArtifactCategory.Clothing ),
            new ArtifactEntry( "Artifact_CaptainJohnsHat",            "Captain John's Hat",                ArtifactCategory.Clothing ),
            new ArtifactEntry( "Artifact_CloakOfTheRogue",            "Cloak of the Rogue",                ArtifactCategory.Clothing ),
            new ArtifactEntry( "Artifact_CoatOfTheDreadPirate",       "Coat of the Dread Pirate",          ArtifactCategory.Clothing ),
            new ArtifactEntry( "Artifact_ConansLoinCloth",            "Loin Cloth of the Cimmerian",       ArtifactCategory.Clothing ),
            new ArtifactEntry( "Artifact_DreadPirateHat",             "Dread Pirate Hat",                  ArtifactCategory.Clothing ),
            new ArtifactEntry( "Artifact_FurCapeOfTheSorceress",      "Fur Cape Of The Sorceress",         ArtifactCategory.Clothing ),
            new ArtifactEntry( "Artifact_GrimReapersMask",            "Grim Reaper's Mask",                ArtifactCategory.Clothing ),
            new ArtifactEntry( "Artifact_GrimReapersRobe",            "Grim Reaper's Robe",                ArtifactCategory.Clothing ),
            new ArtifactEntry( "Artifact_SamaritanRobe",              "Good Samaritan Robe",               ArtifactCategory.Clothing ),
            new ArtifactEntry( "Artifact_HoodedShroudOfShadows",      "Hooded Shroud of Shadows",          ArtifactCategory.Clothing ),
            new ArtifactEntry( "Artifact_JinBaoriOfGoodFortune",      "Jin-Baori Of Good Fortune",         ArtifactCategory.Clothing ),
            new ArtifactEntry( "Artifact_LieutenantOfTheBritannianRoyalGuard", "Royal Guard Sash",         ArtifactCategory.Clothing ),
            new ArtifactEntry( "Artifact_MantleofPyros",              "Mantle of the Daemon King",         ArtifactCategory.Clothing ),
            new ArtifactEntry( "Artifact_MantleofHydros",             "Mantle of the Lurker",              ArtifactCategory.Clothing ),
            new ArtifactEntry( "Artifact_MantleofLithos",             "Mantle of the Mountain King",       ArtifactCategory.Clothing ),
            new ArtifactEntry( "Artifact_MantleofStratos",            "Mantle of the Mystic Voice",        ArtifactCategory.Clothing ),
            new ArtifactEntry( "Artifact_GandalfsHat",                "Merlin's Mystical Hat",             ArtifactCategory.Clothing ),
            new ArtifactEntry( "Artifact_GandalfsRobe",               "Merlin's Mystical Robe",            ArtifactCategory.Clothing ),
            new ArtifactEntry( "Artifact_ANecromancerShroud",         "Necromancer Shroud",                ArtifactCategory.Clothing ),
            new ArtifactEntry( "Artifact_VampiresRobe",               "Nosferatu's Robe",                  ArtifactCategory.Clothing ),
            new ArtifactEntry( "Artifact_PiedPiperFeatheredHat",      "Pied Piper's Feathered Hat",        ArtifactCategory.Clothing ),
            new ArtifactEntry( "Artifact_PolarBearCape",              "Polar Bear Cape",                   ArtifactCategory.Clothing ),
            new ArtifactEntry( "Artifact_PolarBearMask",              "Spirit of the Polar Bear",          ArtifactCategory.Clothing ),
            new ArtifactEntry( "Artifact_TheRobeOfBritanniaAri",      "Robe of Sosaria",                   ArtifactCategory.Clothing ),
            new ArtifactEntry( "Artifact_RobeOfTeleportation",        "Robe Of Teleportation",             ArtifactCategory.Clothing ),
            new ArtifactEntry( "Artifact_RobeofPyros",                "Robe of the Daemon King",           ArtifactCategory.Clothing ),
            new ArtifactEntry( "Artifact_RobeOfTheEclipse",           "Robe of the Eclipse",               ArtifactCategory.Clothing ),
            new ArtifactEntry( "Artifact_RobeOfTheEquinox",           "Robe of the Equinox",               ArtifactCategory.Clothing ),
            new ArtifactEntry( "Artifact_RobeofHydros",               "Robe of the Lurker",                ArtifactCategory.Clothing ),
            new ArtifactEntry( "Artifact_RobeofLithos",               "Robe of the Mountain King",         ArtifactCategory.Clothing ),
            new ArtifactEntry( "Artifact_RobeofStratos",              "Robe of the Mystic Voice",          ArtifactCategory.Clothing ),
            new ArtifactEntry( "Artifact_RobeOfTreason",              "Robe Of Treason",                   ArtifactCategory.Clothing ),
            new ArtifactEntry( "Artifact_RobeOfWilds",                "Robe of the Wilds",                 ArtifactCategory.Clothing ),
            new ArtifactEntry( "Artifact_RobeOfWildLegion",           "Robe of the Wild Legion",           ArtifactCategory.Clothing ),
            new ArtifactEntry( "Artifact_RobinHoodsFeatheredHat",     "Robin Hood's Feathered Hat",        ArtifactCategory.Clothing ),
            new ArtifactEntry( "Artifact_ShirtOfThePiper",            "Shirt of the Pied Piper",           ArtifactCategory.Clothing ),
            new ArtifactEntry( "Artifact_SilksOfAllurement",          "Silks of Allurement",               ArtifactCategory.Clothing ),
            new ArtifactEntry( "Artifact_TemptationOfSune",           "Temptation of Sune",                ArtifactCategory.Clothing ),
            new ArtifactEntry( "Artifact_TrousersOfThePiper",         "Trousers of the Pied Piper",        ArtifactCategory.Clothing ),
            new ArtifactEntry( "Artifact_BootsOfFanaedar",            "Boots of Fanaedar",                 ArtifactCategory.Clothing )
        };

        public static ArrayList GetEntriesForCategory(ArtifactCategory cat)
        {
            ArrayList result = new ArrayList();
            for (int i = 0; i < AllArtifacts.Length; i++)
            {
                if (AllArtifacts[i].Category == cat)
                    result.Add(AllArtifacts[i]);
            }
            return result;
        }

        public static string CategoryLabel(ArtifactCategory cat)
        {
            switch (cat)
            {
                case ArtifactCategory.OneHandedWeapons: return "One-Handed Weapons";
                case ArtifactCategory.TwoHandedWeapons: return "Two-Handed Weapons";
                case ArtifactCategory.RangedWeapons: return "Ranged Weapons & Quivers";
                case ArtifactCategory.JewelryTrinkets: return "Jewelry & Trinkets";
                case ArtifactCategory.ArmorShields: return "Armor & Shields";
                case ArtifactCategory.Clothing: return "Clothing";
                default: return "Unknown";
            }
        }

        public Mobile owner;
        public int LegendLore;

        [CommandProperty(AccessLevel.GameMaster)]
        public Mobile Owner { get { return owner; } set { owner = value; } }

        [CommandProperty(AccessLevel.Owner)]
        public int Legend_Lore { get { return LegendLore; } set { LegendLore = value; InvalidateProperties(); } }

        [Constructable]
        public SearchBook(Mobile from, int paid) : base(0x22C5)
        {
            owner = from;
            LegendLore = (paid / 1000) - 4;
            Weight = 1.0;
            Hue = 0x978;
            Name = "Artifact Encyclopedia";
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!IsChildOf(from.Backpack))
            {
                from.SendMessage("This must be in your backpack to read.");
                return;
            }
            if (owner != from)
            {
                from.SendMessage("This is not your book.");
                return;
            }

            from.SendSound(0x55);
            from.CloseGump(typeof(CategoryGump));
            from.CloseGump(typeof(CategoryItemsGump));
            from.CloseGump(typeof(ConfirmGump));
            from.SendGump(new CategoryGump(from, this));
        }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            if (owner != null)
                list.Add(1070722, "Belongs to " + owner.Name);

            list.Add(1049644, "Legend Lore: Level " + LegendLore.ToString());
        }

        public SearchBook(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)1);
            writer.Write((Mobile)owner);
            writer.Write(LegendLore);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            owner = reader.ReadMobile();
            LegendLore = reader.ReadInt();
        }

        public class CategoryGump : Gump
        {
            private SearchBook m_Book;


            public CategoryGump(Mobile from, SearchBook book) : base(100, 100)
            {
                m_Book = book;
                string color = "#d6c382";

                Closable = true;
                Disposable = true;
                Dragable = true;
                Resizable = false;

                AddPage(0);

                AddBackground(0, 0, 340, 320, 9270);

                AddHtml(20, 15, 300, 24,
                    "<BODY><BASEFONT Color=" + color + "><CENTER>ARTIFACT ENCYCLOPEDIA</CENTER></BASEFONT></BODY>",
                    false, false);
                AddHtml(20, 38, 300, 20,
                    "<BODY><BASEFONT Color=" + color + "><CENTER>Choose a Category</CENTER></BASEFONT></BODY>",
                    false, false);

                int btnX = 60;
                int labelX = 100;
                int y = 74;
                int step = 36;

                AddCategoryRow(btnX, labelX, y, 1, "One-Handed Weapons", color); y += step;
                AddCategoryRow(btnX, labelX, y, 2, "Two-Handed Weapons", color); y += step;
                AddCategoryRow(btnX, labelX, y, 3, "Ranged Weapons & Quivers", color); y += step;
                AddCategoryRow(btnX, labelX, y, 4, "Jewelry & Trinkets", color); y += step;
                AddCategoryRow(btnX, labelX, y, 5, "Armor & Shields", color); y += step;
                AddCategoryRow(btnX, labelX, y, 6, "Clothing", color);

                AddButton(148, 282, 4017, 4019, 0, GumpButtonType.Reply, 0);
                AddHtml(183, 284, 60, 20,
                    "<BODY><BASEFONT Color=" + color + ">Close</BASEFONT></BODY>",
                    false, false);
            }

            private void AddCategoryRow(int btnX, int labelX, int y, int buttonID, string label, string color)
            {
                AddButton(btnX, y, 4005, 4007, buttonID, GumpButtonType.Reply, 0);
                AddHtml(labelX, y + 2, 210, 22,
                    "<BODY><BASEFONT Color=" + color + ">" + label + "</BASEFONT></BODY>",
                    false, false);
            }

            public override void OnResponse(NetState state, RelayInfo info)
            {
                Mobile from = state.Mobile;
                from.SendSound(0x55);

                ArtifactCategory cat;
                switch (info.ButtonID)
                {
                    case 1: cat = ArtifactCategory.OneHandedWeapons; break;
                    case 2: cat = ArtifactCategory.TwoHandedWeapons; break;
                    case 3: cat = ArtifactCategory.RangedWeapons; break;
                    case 4: cat = ArtifactCategory.JewelryTrinkets; break;
                    case 5: cat = ArtifactCategory.ArmorShields; break;
                    case 6: cat = ArtifactCategory.Clothing; break;
                    default: return; // Close 
                }

                from.CloseGump(typeof(CategoryGump));
                from.SendGump(new CategoryItemsGump(from, m_Book, cat, 0));
            }
        }

        public class CategoryItemsGump : Gump
        {
            private SearchBook m_Book;
            private ArtifactCategory m_Category;
            private ArrayList m_Entries;
            private int m_Page;

            private const int ItemsPerPage = 16;

            // Button ID encoding
            //   0              = close / back to categories
            //   1              = navigate: previous page
            //   2              = navigate: next page
            //   1000 + index   = select artifact at m_Entries[index]

            public CategoryItemsGump(Mobile from, SearchBook book, ArtifactCategory cat, int page)
                : base(100, 100)
            {
                m_Book = book;
                m_Category = cat;
                m_Entries = SearchBook.GetEntriesForCategory(cat);
                m_Page = page;

                string color = "#d6c382";

                Closable = true;
                Disposable = true;
                Dragable = true;
                Resizable = false;

                AddPage(0);

                AddImage(0, 0, 7005);
                AddImage(0, 0, 7006);
                AddImage(0, 0, 7024, 2736);
                AddButton(590, 48, 4017, 4017, 0, GumpButtonType.Reply, 0);

                AddHtml(77, 49, 259, 20,
                    "<BODY><BASEFONT Color=" + color + "><CENTER>" + SearchBook.CategoryLabel(cat).ToUpper() + "</CENTER></BASEFONT></BODY>",
                    false, false);

                int totalPages = (m_Entries.Count + ItemsPerPage - 1) / ItemsPerPage;
                if (totalPages < 1) totalPages = 1;

                int prevPage = page - 1;
                if (prevPage < 0) prevPage = totalPages - 1;
                int nextPage = page + 1;
                if (nextPage >= totalPages) nextPage = 0;

                if (totalPages > 1)
                {
                    AddButton(75, 374, 4014, 4014, 1, GumpButtonType.Reply, 0);
                    AddButton(590, 375, 4005, 4005, 2, GumpButtonType.Reply, 0);
                }

                AddButton(300, 374, 4017, 4019, 0, GumpButtonType.Reply, 0);
                AddHtml(336, 376, 120, 18,
                    "<BODY><BASEFONT Color=" + color + ">Categories</BASEFONT></BODY>",
                    false, false);

                int firstIndex = page * ItemsPerPage;

                int x = 115;
                int y = 64;
                int z = 34;
                int s = 64;

                y = s;
                y += z;
                for (int slot = 0; slot < 8; slot++)
                {
                    int idx = firstIndex + slot;
                    if (idx < m_Entries.Count)
                    {
                        AddButton(x, y, 2447, 2447, 1000 + idx, GumpButtonType.Reply, 0);
                    }
                    y += z;
                }

                y = s - 3;
                y += z;
                for (int slot = 0; slot < 8; slot++)
                {
                    int idx = firstIndex + slot;
                    string label = (idx < m_Entries.Count)
                        ? ((ArtifactEntry)m_Entries[idx]).DisplayName
                        : "";
                    AddHtml(x + 20, y, 155, 20,
                        "<BODY><BASEFONT Color=" + color + ">" + label + "</BASEFONT></BODY>",
                        false, false);
                    y += z;
                }

                x = 407;
                y = s;
                y += z;
                for (int slot = 8; slot < 16; slot++)
                {
                    int idx = firstIndex + slot;
                    if (idx < m_Entries.Count)
                    {
                        AddButton(x, y, 2447, 2447, 1000 + idx, GumpButtonType.Reply, 0);
                    }
                    y += z;
                }

                y = s - 3;
                y += z;
                for (int slot = 8; slot < 16; slot++)
                {
                    int idx = firstIndex + slot;
                    string label = (idx < m_Entries.Count)
                        ? ((ArtifactEntry)m_Entries[idx]).DisplayName
                        : "";
                    AddHtml(x + 20, y, 155, 20,
                        "<BODY><BASEFONT Color=" + color + ">" + label + "</BASEFONT></BODY>",
                        false, false);
                    y += z;
                }
            }

            public override void OnResponse(NetState state, RelayInfo info)
            {
                Mobile from = state.Mobile;
                from.SendSound(0x55);

                int totalPages = (m_Entries.Count + ItemsPerPage - 1) / ItemsPerPage;
                if (totalPages < 1) totalPages = 1;

                if (info.ButtonID == 0)
                {
                    from.CloseGump(typeof(CategoryItemsGump));
                    from.SendGump(new CategoryGump(from, m_Book));
                }
                else if (info.ButtonID == 1)
                {
                    int prev = m_Page - 1;
                    if (prev < 0) prev = totalPages - 1;
                    from.CloseGump(typeof(CategoryItemsGump));
                    from.SendGump(new CategoryItemsGump(from, m_Book, m_Category, prev));
                }
                else if (info.ButtonID == 2)
                {
                    int next = m_Page + 1;
                    if (next >= totalPages) next = 0;
                    from.CloseGump(typeof(CategoryItemsGump));
                    from.SendGump(new CategoryItemsGump(from, m_Book, m_Category, next));
                }
                else if (info.ButtonID >= 1000)
                {
                    int idx = info.ButtonID - 1000;
                    if (idx >= 0 && idx < m_Entries.Count)
                    {
                        ArtifactEntry entry = (ArtifactEntry)m_Entries[idx];
                        from.CloseGump(typeof(CategoryItemsGump));
                        from.SendGump(new ConfirmGump(from, entry, m_Book, m_Category, m_Page));
                    }
                }
            }
        }

        public class ConfirmGump : Gump
        {
            private SearchBook m_Book;
            private ArtifactEntry m_Entry;
            private ArtifactCategory m_ReturnCategory;
            private int m_ReturnPage;

            public ConfirmGump(Mobile user, ArtifactEntry entry, SearchBook book,
                                ArtifactCategory returnCat, int returnPage)
                : base(50, 50)
            {
                m_Book = book;
                m_Entry = entry;
                m_ReturnCategory = returnCat;
                m_ReturnPage = returnPage;

                Closable = true;
                Disposable = true;
                Dragable = true;
                Resizable = false;

                AddBackground(0, 0, 420, 175, 9270);

                AddLabel(30, 20, 2120, "Are you sure you want to search for:");
                AddLabel(30, 42, 2120, entry.DisplayName);
                AddLabel(30, 72, 2120, "Confirming will consume your Artifact Encyclopedia.");

                // Yes
                AddButton(80, 130, 4005, 4007, 1, GumpButtonType.Reply, 0);
                AddLabel(115, 132, 2120, "Yes");

                // No
                AddButton(220, 130, 4017, 4019, 0, GumpButtonType.Reply, 0);
                AddLabel(255, 132, 2120, "No");
            }

            public override void OnResponse(NetState sender, RelayInfo info)
            {
                Mobile from = sender.Mobile;

                if (info.ButtonID == 1)
                {
                    from.AddToBackpack(new SearchPage(from, m_Book.LegendLore, m_Entry.TypeName, m_Entry.DisplayName));
                    from.SendMessage("You tear the page out of the book.");
                    m_Book.Delete();
                    from.CloseGump(typeof(ConfirmGump));
                }
                else
                {
                    from.CloseGump(typeof(ConfirmGump));
                    from.SendGump(new CategoryItemsGump(from, m_Book, m_ReturnCategory, m_ReturnPage));
                }
            }
        }
    }
}