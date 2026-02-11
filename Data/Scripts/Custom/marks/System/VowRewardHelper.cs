using System;
using Server;
using Server.Items;
using Server.Mobiles;

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

        public static void GenerateRewards(Mobile from, int rewardWorth, Container rewardBag, VowType type)
        {
            if (rewardWorth < 5)
            {
                GenerateEnchantedItem(from, 75, rewardBag);
                rewardBag.DropItem(Loot.RandomScroll(1));
                rewardBag.DropItem(Loot.RandomPotion(4, false));
            }
            else if (rewardWorth < 10)
            {
                GenerateEnchantedItem(from, 150, rewardBag);
                rewardBag.DropItem(Loot.RandomGem());
                rewardBag.DropItem(Loot.RandomPotion(4, false));
                rewardBag.DropItem(Loot.RandomPotion(4, false));
                rewardBag.DropItem(Loot.RandomScroll(3));
            }
            else if (rewardWorth < 15)
            {
                GenerateEnchantedItem(from, 200, rewardBag);
                rewardBag.DropItem(Loot.RandomScroll(4));
                rewardBag.DropItem(Loot.RandomGem());
                rewardBag.DropItem(Loot.RandomGem());
                if (Utility.RandomDouble() < 0.10)
                {
                    rewardBag.DropItem(ScrollofTranscendence.CreateRandom(5, 10));
                }
            }
            else if (rewardWorth < 20)
            {
                GenerateEnchantedItem(from, 250, rewardBag);
                rewardBag.DropItem(Loot.RandomScroll(5));
                rewardBag.DropItem(Loot.RandomPotion(8, false));
                rewardBag.DropItem(Loot.RandomPotion(8, false));
                if (Utility.RandomDouble() < 0.20)
                {
                    rewardBag.DropItem(ScrollofTranscendence.CreateRandom(5, 15));
                }
            }
            else if (rewardWorth < 25)
            {
                GenerateEnchantedItem(from, 300, rewardBag);
                rewardBag.DropItem(Loot.RandomScroll(6));
                rewardBag.DropItem(Loot.RandomPotion(8, false));
                if (Utility.RandomDouble() < 0.20)
                {
                    rewardBag.DropItem(new EtherealPowerScroll());
                }
                if (Utility.RandomDouble() < 0.40)
                {
                    rewardBag.DropItem(ScrollofTranscendence.CreateRandom(5, 15));
                }
            }
            else if (rewardWorth < 30)
            {
                GenerateEnchantedItem(from, 350, rewardBag);
                rewardBag.DropItem(Loot.RandomScroll(8));
                rewardBag.DropItem(Loot.RandomPotion(12, false));
                rewardBag.DropItem(Loot.RandomPotion(12, false));
                if (Utility.RandomDouble() < 0.40)
                {
                    rewardBag.DropItem(new EtherealPowerScroll());
                }
                if (Utility.RandomDouble() < 0.60)
                {
                    rewardBag.DropItem(ScrollofTranscendence.CreateRandom(5, 15));
                }
            }
            else if (rewardWorth < 35)
            {
                GenerateEnchantedItem(from, 400, rewardBag);
                if (Utility.RandomDouble() < 0.05)
                {
                    rewardBag.DropItem(Loot.RandomSArty(Server.LootPackEntry.playOrient(from), from));
                }
                if (Utility.RandomDouble() < 0.60)
                {
                    rewardBag.DropItem(new EtherealPowerScroll());
                }
                if (Utility.RandomDouble() < 0.80)
                {
                    rewardBag.DropItem(ScrollofTranscendence.CreateRandom(5, 15));
                }
            }
            else if (rewardWorth < 40)
            {
                GenerateEnchantedItem(from, 450, rewardBag);
                rewardBag.DropItem(Loot.RandomRare(Utility.RandomMinMax(6, 12), from));
                if (Utility.RandomDouble() < 0.15)
                {
                    rewardBag.DropItem(Loot.RandomSArty(Server.LootPackEntry.playOrient(from), from));
                }
                if (Utility.RandomDouble() < 0.80)
                {
                    rewardBag.DropItem(new EtherealPowerScroll());
                }
                if (Utility.RandomDouble() < 0.20)
                {
                    rewardBag.DropItem(new EtherealPowerScroll());
                }
                rewardBag.DropItem(ScrollofTranscendence.CreateRandom(5, 15));
                if (Utility.RandomDouble() < 0.20)
                {
                    rewardBag.DropItem(ScrollofTranscendence.CreateRandom(5, 25));
                }
            }
            else if (rewardWorth < 45)
            {
                rewardBag.DropItem(Loot.RandomRelic(from));
                GenerateEnchantedItem(from, 500, rewardBag);
                if (Utility.RandomDouble() < 0.35)
                {
                    rewardBag.DropItem(Loot.RandomSArty(Server.LootPackEntry.playOrient(from), from));
                }
                rewardBag.DropItem(new EtherealPowerScroll());
                if (Utility.RandomDouble() < 0.40)
                {
                    rewardBag.DropItem(new EtherealPowerScroll());
                }
                if (Utility.RandomDouble() < 0.40)
                {
                    rewardBag.DropItem(ScrollofTranscendence.CreateRandom(5, 25));
                    rewardBag.DropItem(ScrollofTranscendence.CreateRandom(5, 25));
                }
            }
            else if (rewardWorth < 50)
            {
                rewardBag.DropItem(Loot.RandomSArty(Server.LootPackEntry.playOrient(from), from));
                if (Utility.RandomDouble() < 0.40)
                {
                    rewardBag.DropItem(Loot.RandomRelic(from));
                }
                if (Utility.RandomDouble() < 0.40)
                {
                    rewardBag.DropItem(Loot.RandomRare(Utility.RandomMinMax(6, 12), from));
                }
                rewardBag.DropItem(new EtherealPowerScroll());
                rewardBag.DropItem(new EtherealPowerScroll());
                if (Utility.RandomDouble() < 0.60)
                {
                    rewardBag.DropItem(ScrollofTranscendence.CreateRandom(5, 25));
                    rewardBag.DropItem(ScrollofTranscendence.CreateRandom(5, 25));
                }
                if (Utility.RandomDouble() < 0.40)
                {
                    rewardBag.DropItem(new EtherealPowerScroll());
                }
                if(Utility.RandomDouble() < 0.05 )
                {
                    rewardBag.DropItem(new EternalPowerScroll());
                }
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
                rewardBag.DropItem(ScrollofTranscendence.CreateRandom(5, 25));
                rewardBag.DropItem(ScrollofTranscendence.CreateRandom(5, 25));
                GenerateEnchantedItem(from, 500, rewardBag);
                GenerateEnchantedItem(from, 500, rewardBag);
                GenerateEnchantedItem(from, 500, rewardBag);
                if(Utility.RandomDouble() < 0.25 )
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
