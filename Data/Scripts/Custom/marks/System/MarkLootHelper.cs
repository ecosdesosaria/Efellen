using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Custom.DefenderOfTheRealm
{
    public static class MarkLootHelper
    {
        public static void CheckForMarks(BaseCreature bc, Container c, Mobile killer)
        {
            if (bc == null || c == null || killer == null)
                return;

            if (bc.Controlled || bc.Summoned || bc.Player)
                return;

            if (bc.Fame < 10000)
                return;
            
            if (Utility.RandomDouble() > 0.25)
                return;

            int baseMin = 1;
            int baseMax = 9;

            int luck = killer.Luck;
            if (luck < 0) luck = 0;
            if (luck > 2000) luck = 2000;

            int bonus = (luck * 2 / 2000);
            int amount = Utility.RandomMinMax(baseMin, baseMax + bonus);

            if (amount <= 0)
                return;

            if ( killer.Karma < 0)
            {
                c.DropItem(new MarksOfTheScourge(amount));
            }
            else if ( killer.Karma >= 0)
            {
                c.DropItem(new MarksOfHonor(amount));
            }
        }

        public static void AwardMarks(Mobile recipient, int type, int amount)
        {
            if (recipient == null || recipient.Deleted)
                return;

            if (amount <= 0)
                return;

            Item marks = null;
            string str = "";

            try
            {
                switch (type)
                {
                    case 0: marks = new MarksOfTheScourge(amount); str = "Scourge"; break;
                    case 1: marks = new MarksOfHonor(amount); str = "Honor"; break;
                    case 2: marks = new MarksOfTheShadowbroker(amount); str = "Shadowbroker"; break;
                    case 3: marks = new MarksOfTheWilds(amount); str = "Wilds"; break;
                    case 4: marks = new MarksOfDevotion(amount); str = "Devotion"; break;
                    case 5: marks = new MarksOfTheWeave(amount); str = "Weave"; break;
                    default:
                        return;
                }

                Container pack = recipient.Backpack;

                if (pack != null && !pack.Deleted)
                {
                    Item existing = null;

                    foreach (Item i in pack.Items)
                    {
                        if (i.GetType() == marks.GetType())
                        {
                            existing = i;
                            break;
                        }
                    }

                    if (existing != null)
                    {
                        existing.Amount += amount;

                        string msg = "Você ganhou " + amount + " marca" + (amount > 1 ? "s" : "") +
                                     " do " + str + ".";
                        recipient.SendMessage(msg);

                        marks.Delete();
                    }
                    else
                    {
                        pack.DropItem(marks);

                        string msg = "Você recebeu " + amount + " marca" +
                                     (amount > 1 ? "s" : "") + " do " + str + ".";
                        recipient.SendMessage(msg);
                    }
                }
                else
                {
                    marks.MoveToWorld(recipient.Location, recipient.Map);
                    recipient.SendMessage("Suas marcas do " + str + " foram colocadas em seus pés.");
                }
            }
            catch (Exception){}
        }
    }

}
