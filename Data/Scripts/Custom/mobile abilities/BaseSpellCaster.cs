using System;
using Server;
using Server.Mobiles;
using Server.CustomSpells;

namespace Server.Mobiles
{
    public class BaseSpellCaster : BaseCreature
    {
        public BaseSpellCaster(AIType ai, FightMode mode, int iRangePerceive, int iRangeFight, double dActiveSpeed, double dPassiveSpeed)
            : base(ai, mode, iRangePerceive, iRangeFight, dActiveSpeed, dPassiveSpeed)
        {
        }

       public override void OnThink()
        {
            base.OnThink();
            if (Combatant == null || Combatant.Deleted || !Combatant.Alive)
                return;
            MobileMagic magic = this.GetMobileMagic();
            if (magic != null && Utility.RandomDouble() > 0.30)
                magic.TryCastSpell();
        }

        
        public override void OnDamage(int amount, Mobile from, bool willKill)
        {
            base.OnDamage(amount, from, willKill);
            MobileMagic magic = this.GetMobileMagic();
            if (magic != null && magic.IsChanneling)
            {
                magic.OnDamaged();
            }
        }
        
        public override void OnDelete()
        {
            this.RemoveMobileMagic();
            base.OnDelete();
        }

        public BaseSpellCaster(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}