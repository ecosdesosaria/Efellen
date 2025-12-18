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

        public static void AwardBossSpecial(BaseCreature boss, int chance, params Type[] dropList)
        {
            AwardBossSpecial(boss, new List<Type>(dropList), chance);
        }
    }
}
