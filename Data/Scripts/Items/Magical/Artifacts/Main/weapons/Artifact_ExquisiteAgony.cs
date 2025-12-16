using System;
using Server.Network;
using Server.Items;
using Server.CustomEffects;

namespace Server.Items
{
	public class Artifact_ExquisiteAgony : GiftWhips
	{
		private DateTime m_NextArtifactAttackAllowed;
		private DateTime m_NextParalyze;
		public override int InitMinHits{ get{ return 80; } }
		public override int InitMaxHits{ get{ return 160; } }

		[Constructable]
		public Artifact_ExquisiteAgony()
		{
			Name = "Exquisite Agony";
			Hue = 0xb73;
			WeaponAttributes.HitLeechStam = 50;
            Attributes.WeaponSpeed = 20;
			Attributes.BonusDex = 10;
			Attributes.AttackChance = 8;
			ArtifactLevel = 2;
			Server.Misc.Arty.ArtySetup( this, "Immobilizes and delivers unending pain" );
			m_NextArtifactAttackAllowed = DateTime.MinValue;
		}

		public override void OnHit(Mobile attacker, Mobile defender, double damageBonus)
        {
            base.OnHit(attacker, defender, damageBonus);

            if (DateTime.Now < m_NextParalyze)
                return;

          
            if (Utility.RandomDouble() < 0.25)
            {
				if (defender != null && defender.Alive && !defender.Paralyzed)
                {
                    defender.Paralyze(TimeSpan.FromSeconds(5));
                    attacker.SendMessage("Your blow immobilizes your foe!");
                    double skill = attacker.Skills[SkillName.Bludgeoning].Value;
					int duration = 7 + (int)(skill / 25.0);
					DotEffect.ApplyDot(defender, duration, attacker,2);
					m_NextParalyze = DateTime.Now + TimeSpan.FromSeconds(45);
                }
            }
        }

		public override void GetDamageTypes( Mobile wielder, out int phys, out int fire, out int cold, out int pois, out int nrgy, out int chaos, out int direct )
		{
			fire = 100;

			pois = cold = phys = nrgy = chaos = direct = 0;
		}

		public Artifact_ExquisiteAgony( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 1 ); // version
			writer.Write(m_NextArtifactAttackAllowed);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadEncodedInt();
			if (version >= 1)
		        m_NextArtifactAttackAllowed = reader.ReadDateTime();
		    else
		        m_NextArtifactAttackAllowed = DateTime.MinValue;
			ArtifactLevel = 2;
		}
	}
}