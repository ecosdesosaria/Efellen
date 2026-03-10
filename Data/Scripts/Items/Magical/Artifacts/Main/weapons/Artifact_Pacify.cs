using System;
using Server;

namespace Server.Items
{
	public class Artifact_Pacify : GiftPike
	{
		private DateTime m_NextArtifactAttackAllowed;
		public override int InitMinHits{ get{ return 80; } }
		public override int InitMaxHits{ get{ return 160; } }

		public override bool CanFortify{ get{ return false; } }

		[Constructable]
		public Artifact_Pacify()
		{
			Name = "Pacify";
			Hue = 0x835;

			Attributes.SpellChanneling = 1;
			Attributes.WeaponSpeed = 25;
			WeaponAttributes.HitLeechHits = 50;
			ArtifactLevel = 2;
			Server.Misc.Arty.ArtySetup( this, "Immobilizes foes" );
			m_NextArtifactAttackAllowed = DateTime.MinValue;
		}

		public override void OnHit(Mobile attacker, Mobile defender, double damageBonus)
        {
            base.OnHit(attacker, defender, damageBonus);

            if (DateTime.Now < m_NextArtifactAttackAllowed)
                return;


            if (Utility.RandomDouble() < 0.25)
            {
                if (defender != null && defender.Alive && !defender.Paralyzed)
                {
                    defender.Paralyze(TimeSpan.FromSeconds(9));
                    attacker.SendMessage("Seu golpe imobiliza seu inimigo!");
                    m_NextArtifactAttackAllowed = DateTime.Now + TimeSpan.FromSeconds(90);
                }
            }
        }

		public Artifact_Pacify( Serial serial ) : base( serial )
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
