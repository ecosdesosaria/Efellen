using System;
using Server;

namespace Server.Items
{
    public class Artifact_ButchersCaress : GiftExecutionersAxe
    {
        private DateTime m_NextParalyze;
        
        public override int InitMinHits{ get{ return 80; } }
        public override int InitMaxHits{ get{ return 160; } }

        [Constructable]
        public Artifact_ButchersCaress()
        {
            Name = "Butcher's Caress";
            Hue = 0x845;
            SkillBonuses.SetValues(0, SkillName.Anatomy, 20);
            ItemID = 0xF45;
            Slayer = SlayerName.Repond;
            WeaponAttributes.HitLeechMana = 30;
            Attributes.AttackChance = 10;
            MinDamage = MinDamage + 1;
			MaxDamage = MaxDamage + 1;
            ArtifactLevel = 2;
            Server.Misc.Arty.ArtySetup( this, "Chops limbs" );
            m_NextParalyze = DateTime.MinValue;
        }

        public override void OnHit(Mobile attacker, Mobile defender, double damageBonus)
        {
            base.OnHit(attacker, defender, damageBonus);

            if (defender == null || !defender.Alive || defender.Paralyzed)
                return;

            if (DateTime.UtcNow < m_NextParalyze)
                return;

            if (Utility.RandomDouble() >= 0.15)
                return;

            if (!IsSlayerEffective(defender))
                return;

            defender.Paralyze(TimeSpan.FromSeconds(9));
            
            if (attacker != null && !attacker.Deleted)
                attacker.SendMessage("Your vicious blow chops a limb out of your opponent!");
            
            m_NextParalyze = DateTime.UtcNow + TimeSpan.FromSeconds(60);
        }

        private static SlayerEntry s_RepondEntry = null;

        private bool IsSlayerEffective(Mobile m)
        {
            if (Slayer != SlayerName.Repond)
                return false;

            if (s_RepondEntry == null)
                s_RepondEntry = SlayerGroup.GetEntryByName(SlayerName.Repond);

            return s_RepondEntry != null && s_RepondEntry.Slays(m);
        }

        public override void GetDamageTypes( Mobile wielder, out int phys, out int fire, out int cold, out int pois, out int nrgy, out int chaos, out int direct )
        {
            phys = 100;
            fire = cold = pois = nrgy = chaos = direct = 0;
        }

        public Artifact_ButchersCaress( Serial serial ) : base( serial )
        {
        }

        public override void Serialize( GenericWriter writer )
        {
            base.Serialize( writer );
            writer.Write( (int) 1 ); // version

            writer.Write( m_NextParalyze );
        }
        
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize( reader );
            
            ArtifactLevel = 2;
            
            int version = reader.ReadInt();

            switch (version)
            {
                case 1:
                {
                    m_NextParalyze = reader.ReadDateTime();
                    break;
                }
                case 0:
                {
                    m_NextParalyze = DateTime.MinValue;
                    break;
                }
            }

            if (Slayer == SlayerName.None)
                Slayer = SlayerName.Repond;
        }
    }
}