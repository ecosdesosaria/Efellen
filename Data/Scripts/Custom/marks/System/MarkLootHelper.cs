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

            if (bc.Fame < 3000)
                return;

            if (Utility.RandomDouble() > 0.05)
                return;

            int fameMod = bc.Fame / 750;
            int baseMin = 1;
            int baseMax = bc.Fame/750 > 25 ? 25 : bc.Fame/750;

            int amount = Utility.RandomMinMax(baseMin, baseMax);

            if (killer.Karma < 0)
            {
                c.DropItem(new MarksOfTheScourge(amount));
            }
            else if (killer.Karma >= 0)
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
                    case 0: marks = new MarksOfTheScourge(amount); str = "Flagelo"; break;
                    case 1: marks = new MarksOfHonor(amount); str = "Honra"; break;
                    case 2: marks = new MarksOfTheShadowbroker(amount); str = "Sombra"; break;
                    case 3: marks = new MarksOfTheWilds(amount); str = "Selva"; break;
                    case 4: marks = new MarksOfDevotion(amount); str = "Devoção"; break;
                    case 5: marks = new MarksOfTheWeave(amount); str = "Trama"; break;
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
                                 " de " + str + ".";
                        recipient.SendMessage(msg);

                        marks.Delete();
                    }
                    else
                    {
                        pack.DropItem(marks);

                        string msg = "Você recebeu " + amount + " marca" +
                                     (amount > 1 ? "s" : "") + " de " + str + ".";
                        recipient.SendMessage(msg);
                    }
                }
                else
                {
                    marks.MoveToWorld(recipient.Location, recipient.Map);
                    recipient.SendMessage("Suas marcas de " + str + " foram colocadas a seus pés.");
                }
            }
            catch (Exception) { }
        }
    }

}