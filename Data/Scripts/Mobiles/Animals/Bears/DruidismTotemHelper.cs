using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public static class DruidismTotemHelper
    {
        private static Hashtable m_Lockouts = new Hashtable();

        public static void TryGainTotemFromStudy(
            Mobile from,
            BaseCreature creature,
            string formId,
            double requiredDruidism,
            double baseChance
        )
        {
            if (from == null || creature == null)
                return;

            PlayerMobile pm = from as PlayerMobile;
            if (pm == null || !pm.Alive)
                return;

            HeartOfTheWilds heart = pm.FindItemOnLayer(Layer.Neck) as HeartOfTheWilds;
            if (heart == null)
            {
                pm.SendMessage("You must be wearing the Heart of the Wilds.");
                return;
            }

            if (pm.Skills[SkillName.Druidism].Value < requiredDruidism)
            {
                pm.SendMessage("You lack the druidic insight to understand this creature.");
                return;
            }
            // Already unlocked
            if (heart.IsFormUnlocked(formId))
            {
                pm.SendMessage("You already understand this form.");
                return;
            }
            // Lockout check
            if (IsLockedOut(pm, formId))
            {
                pm.SendMessage("You need more time to contemplate this creature's essence.");
                return;
            }

            double chance = baseChance;

            double surplus = pm.Skills[SkillName.Druidism].Value - requiredDruidism;
            if (surplus > 0)
                chance += surplus * 0.001;

            chance = Math.Min(chance, 0.20); // hard cap 20%

            if (Utility.RandomDouble() <= chance)
            {
                GrantTotem(pm, formId);
                pm.SendMessage("Through careful study, you attune yourself to this creature's spirit.");
            }
            else
            {
                StartLockout(pm, formId);
                pm.SendMessage("You fail to grasp the creature's true nature.");
            }
        }

       private static void GrantTotem(PlayerMobile pm, string formId)
        {
            TotemOfTheWilds totem = new TotemOfTheWilds(formId);

            if (pm.Backpack != null)
                pm.Backpack.DropItem(totem);
            else
                totem.MoveToWorld(pm.Location, pm.Map);

            pm.FixedParticles(0x373A, 10, 15, 5018, EffectLayer.Waist);
            pm.PlaySound(0x1F7);
        }

        private static bool IsLockedOut(PlayerMobile pm, string formId)
        {
            Hashtable table = m_Lockouts[pm] as Hashtable;
            if (table == null)
                return false;

            object o = table[formId];
            if (!(o is DateTime))
                return false;

            return DateTime.UtcNow < (DateTime)o;
        }

        private static void StartLockout(PlayerMobile pm, string formId)
        {
            Hashtable table = m_Lockouts[pm] as Hashtable;

            if (table == null)
            {
                table = new Hashtable();
                m_Lockouts[pm] = table;
            }

            table[formId] = DateTime.UtcNow + TimeSpan.FromMinutes(5.0);
        }
    }
}