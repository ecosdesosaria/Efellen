using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public static class TotemDropHelper
    {
        public static void TryDropTotem(
            Mobile killer,
            BaseCreature creature,
            string formId,
            double requiredDruidism,
            double chance
        )
        {
            if (killer == null || creature == null)
                return;

            PlayerMobile pm = killer as PlayerMobile;

            if (pm == null)
                return;

            if (!pm.Alive)
                return;
            
            HeartOfTheWilds heart = pm.FindItemOnLayer(Layer.Neck) as HeartOfTheWilds;
            if (heart != null && heart.IsFormUnlocked(formId))
                return;


            if (pm.Skills[SkillName.Druidism].Value < requiredDruidism)
                return;
       
            double surplus = pm.Skills[SkillName.Druidism].Value - requiredDruidism;
            chance += surplus * 0.002;
            chance = Math.Min(chance, 0.35);


            if (Utility.RandomDouble() > chance)
                return;

            TotemOfTheWilds totem = new TotemOfTheWilds(formId);

            if (pm.Backpack != null)
                pm.Backpack.DropItem(totem);
            else
                totem.MoveToWorld(pm.Location, pm.Map);

            pm.SendMessage("You recover a primal totem from the fallen creature.");
        }
    }
}
