using System;
using Server;

namespace Server.Items
{
	public class Artifact_PixieSwatter : GiftScepter
	{
		private DateTime m_NextArtifactAttackAllowed;
		public override int InitMinHits{ get{ return 80; } }
		public override int InitMaxHits{ get{ return 160; } }

		[Constructable]
		public Artifact_PixieSwatter()
		{
			Name = "Pixie Swatter";
			Hue = 0x8A;
			WeaponAttributes.HitPoisonArea = 50;
			Attributes.WeaponSpeed = 10;
			WeaponAttributes.ResistEnergyBonus = 20;
			Slayer = SlayerName.Fey;
			ArtifactLevel = 2;
			Server.Misc.Arty.ArtySetup( this, "Immobilizes fey creatures" );
			m_NextArtifactAttackAllowed = DateTime.MinValue;
		}

		public override void OnHit(Mobile attacker, Mobile defender, double damageBonus)
        {
            base.OnHit(attacker, defender, damageBonus);

            if (DateTime.Now < m_NextArtifactAttackAllowed)
                return;

            bool validTarget = false;
            SlayerEntry fey = SlayerGroup.GetEntryByName(SlayerName.Fey);
			if (fey != null && fey.Slays(defender))
                validTarget = true;
			if (!validTarget)
                return;

            if (Utility.RandomDouble() < 0.15)
            {
                if (defender != null && defender.Alive && !defender.Paralyzed)
                {
                    defender.Paralyze(TimeSpan.FromSeconds(5));
                    attacker.SendMessage("Your blow immobilizes your foe!");
                    m_NextArtifactAttackAllowed = DateTime.Now + TimeSpan.FromSeconds(30);
                }
            }
        }

		public override void GetDamageTypes( Mobile wielder, out int phys, out int fire, out int cold, out int pois, out int nrgy, out int chaos, out int direct )
		{
			fire = 100;

			cold = pois = phys = nrgy = chaos = direct = 0;
		}

		public Artifact_PixieSwatter( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.WriteEncodedInt( 1 );
			writer.Write(m_NextArtifactAttackAllowed);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			
			ArtifactLevel = 2;
			
			int version = reader.ReadEncodedInt();
			
			if (version >= 1)
				m_NextArtifactAttackAllowed = reader.ReadDateTime();
			else
				m_NextArtifactAttackAllowed = DateTime.MinValue;
		}
	}
}