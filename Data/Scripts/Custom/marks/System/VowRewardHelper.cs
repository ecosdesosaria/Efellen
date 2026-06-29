using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Custom.Ascensions;

namespace Server.Custom.DefenderOfTheRealm.Vow
{
    public enum VowType
    {
        Honor,
        Scourge,
        Shadowbroker,
        Wilds
    }

    public static class VowRewardHelper
    {
        public static int GetRequiredAmount(int level)
        {
            Random rand = new Random();

            if (level <= 15)
                return rand.Next(4, 7);
            if (level <= 25)
                return rand.Next(5, 8);
            if (level <= 45)
                return rand.Next(6, 9);
            if (level <= 76)
                return rand.Next(7, 10);
            if (level <= 99)
                return rand.Next(8, 11);
            if (level <= 105)
                return rand.Next(9, 13);
            if (level <= 111)
                return rand.Next(10, 15);
            if (level <= 120)
                return rand.Next(11, 20);
            if (level <= 124)
                return rand.Next(15, 29);
            return rand.Next(20, 36);
        }

        private static void TryAddAscendancy(Container bag, double chance)
        {
            if (Utility.RandomDouble() < chance)
                bag.DropItem(new AscensionScroll());
        }

        private static void TryAddEthereal(Container bag, double chance)
        {
            if (Utility.RandomDouble() < chance)
                bag.DropItem(new EtherealPowerScroll());
        }

        private static void TryAddArtifact(Mobile from, Container bag, double chance)
        {
            if (Utility.RandomDouble() < chance)
                bag.DropItem(Loot.RandomSArty(Server.LootPackEntry.playOrient(from), from));
        }

        private static void TryAddRelic(Mobile from, Container bag, double chance)
        {
            if (Utility.RandomDouble() < chance)
                bag.DropItem(Loot.RandomRelic(from));
        }

        private static void TryAddRare(Mobile from, Container bag, double chance)
        {
            if (Utility.RandomDouble() < chance)
                bag.DropItem(Loot.RandomRare(Utility.RandomMinMax(6, 12), from));
        }

        public static void GenerateRewards(Mobile from, int rewardWorth, Container rewardBag, VowType type)
        {
            if (rewardWorth < 25)
            {
                GenerateEnchantedItem(from, 75, rewardBag);
                rewardBag.DropItem(Loot.RandomScroll(1));
                rewardBag.DropItem(Loot.RandomPotion(4, false));
            }
            else if (rewardWorth < 50)
            {
                GenerateEnchantedItem(from, 150, rewardBag);
                rewardBag.DropItem(Loot.RandomGem());
                rewardBag.DropItem(Loot.RandomPotion(4, false));
                rewardBag.DropItem(Loot.RandomPotion(4, false));
                rewardBag.DropItem(Loot.RandomScroll(3));
            }
            else if (rewardWorth < 75)
            {
                GenerateEnchantedItem(from, 200, rewardBag);
                rewardBag.DropItem(Loot.RandomScroll(4));
                rewardBag.DropItem(Loot.RandomGem());
                rewardBag.DropItem(Loot.RandomGem());
                TryAddAscendancy(rewardBag, 0.20);  
            }
            else if (rewardWorth < 100)
            {
                GenerateEnchantedItem(from, 250, rewardBag);
                rewardBag.DropItem(Loot.RandomScroll(5));
                rewardBag.DropItem(Loot.RandomPotion(8, false));
                rewardBag.DropItem(Loot.RandomPotion(8, false));
                TryAddAscendancy(rewardBag, 0.35);
                TryAddAscendancy(rewardBag, 0.15); 
            }
            else if (rewardWorth < 125)
            {
                GenerateEnchantedItem(from, 300, rewardBag);
                rewardBag.DropItem(Loot.RandomScroll(6));
                rewardBag.DropItem(Loot.RandomPotion(8, false));
                TryAddEthereal(rewardBag, 0.20);
                TryAddAscendancy(rewardBag, 0.40);            
                TryAddAscendancy(rewardBag, 0.20);
            }
            else if (rewardWorth < 150)
            {
                GenerateEnchantedItem(from, 350, rewardBag);
                rewardBag.DropItem(Loot.RandomScroll(8));
                rewardBag.DropItem(Loot.RandomPotion(12, false));
                rewardBag.DropItem(Loot.RandomPotion(12, false));
                TryAddEthereal(rewardBag, 0.40);
                TryAddAscendancy(rewardBag, 0.65);
                TryAddAscendancy(rewardBag, 0.45); 
                TryAddAscendancy(rewardBag, 0.15);
            }
            else if (rewardWorth < 175)
            {
                GenerateEnchantedItem(from, 400, rewardBag);
                TryAddArtifact(from, rewardBag, 0.10);
                TryAddEthereal(rewardBag, 0.60);
                TryAddAscendancy(rewardBag, 0.75);
                TryAddAscendancy(rewardBag, 0.65); 
                TryAddAscendancy(rewardBag, 0.35);
            }
            else if (rewardWorth < 200)
            {
                TryAddArtifact(from, rewardBag, 0.20);
                GenerateEnchantedItem(from, 450, rewardBag);
                rewardBag.DropItem(Loot.RandomRare(Utility.RandomMinMax(6, 12), from));
                TryAddEthereal(rewardBag, 0.80);
                TryAddEthereal(rewardBag, 0.20);
                TryAddAscendancy(rewardBag, 0.85);
                TryAddAscendancy(rewardBag, 0.65); 
                TryAddAscendancy(rewardBag, 0.35);
            }
            else if (rewardWorth < 220)
            {
                rewardBag.DropItem(Loot.RandomRelic(from));
                GenerateEnchantedItem(from, 500, rewardBag);
                TryAddArtifact(from, rewardBag, 0.35);
                rewardBag.DropItem(new EtherealPowerScroll());
                rewardBag.DropItem(new AscensionScroll());
                TryAddEthereal(rewardBag, 0.40);
                TryAddAscendancy(rewardBag, 0.85); 
                TryAddAscendancy(rewardBag, 0.55);
                TryAddAscendancy(rewardBag, 0.35);
            }
            else if (rewardWorth < 250)
            {
                rewardBag.DropItem(Loot.RandomSArty(Server.LootPackEntry.playOrient(from), from));
                TryAddRelic(from, rewardBag, 0.40);
                TryAddRare(from, rewardBag, 0.40);
                rewardBag.DropItem(new EtherealPowerScroll());
                rewardBag.DropItem(new EtherealPowerScroll());
                rewardBag.DropItem(new AscensionScroll());
                rewardBag.DropItem(new AscensionScroll());
                TryAddEthereal(rewardBag, 0.60);
                TryAddAscendancy(rewardBag, 0.75);
                TryAddAscendancy(rewardBag, 0.35);
                GenerateEnchantedItem(from, 500, rewardBag);
                GenerateEnchantedItem(from, 500, rewardBag);
            }
            else
            {
                rewardBag.DropItem(Loot.RandomSArty(Server.LootPackEntry.playOrient(from), from));
                rewardBag.DropItem(Loot.RandomRelic(from));
                rewardBag.DropItem(Loot.RandomRare(Utility.RandomMinMax(6, 12), from));
                rewardBag.DropItem(new EtherealPowerScroll());
                rewardBag.DropItem(new EtherealPowerScroll());
                rewardBag.DropItem(new EtherealPowerScroll());
                rewardBag.DropItem(new AscensionScroll());
                rewardBag.DropItem(new AscensionScroll());
                rewardBag.DropItem(new AscensionScroll());
                rewardBag.DropItem(new AscensionScroll());
                GenerateEnchantedItem(from, 500, rewardBag);
                GenerateEnchantedItem(from, 500, rewardBag);
                GenerateEnchantedItem(from, 500, rewardBag);
                if (Utility.RandomDouble() < 0.25)
                {
                    rewardBag.DropItem(new EternalPowerScroll());
                }
            }
        }
        public static void GenerateEnchantedItem(Mobile from, int enchantLevel, Container rewardBag)
        {
            Item item = Loot.RandomMagicalItem(Server.LootPackEntry.playOrient(from));
            if (item != null)
            {
                item = LootPackEntry.Enchant(from, enchantLevel, item);
                rewardBag.DropItem(item);
            }
        }
    }
}
