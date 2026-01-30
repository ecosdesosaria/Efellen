using System;
using Server.Network;
using Server.Items;
using Server.Targeting;
using Server.CustomEffects;

namespace Server.Items
{
	public class Artifact_GaiasScimitar : GiftScimitar
	{
		private DateTime m_NextArtifactAttackAllowed;
		public override int InitMinHits{ get{ return 80; } }
		public override int InitMaxHits{ get{ return 160; } }

		[Constructable]
		public Artifact_GaiasScimitar()
		{
			Name = "Gaia's Glaive";
			Hue = 669;
			Attributes.SpellChanneling = 1;
			WeaponAttributes.HitLowerDefend = 20;
			WeaponAttributes.HitLowerAttack = 20;
			MinDamage = MinDamage + 2;
			MaxDamage = MaxDamage + 2;
			Attributes.WeaponSpeed = 30;
			MinDamage = 15;
			MaxDamage = 20;
			ArtifactLevel = 2;
			Server.Misc.Arty.ArtySetup( this, "Bursts with power" );
			m_NextArtifactAttackAllowed = DateTime.MinValue;
		}

		public override void OnHit(Mobile attacker, Mobile defender, double damageBonus)
		{
			if (Utility.RandomDouble() < 0.15)
		    {
		        damageBonus += 0.35;
		        attacker.SendMessage("Your strike pierces through your enemy!");
		        attacker.PlaySound(0x20F);
		    }

			base.OnHit(attacker, defender, damageBonus);

			if (attacker == null || defender == null || defender.Deleted)
				return;

			if (DateTime.UtcNow < m_NextArtifactAttackAllowed)
				return;

			if (Utility.RandomDouble() > 0.25)
				return;

			double skill = attacker.Skills[SkillName.Druidism].Value;
			int duration = 4 + (int)(skill / 25.0);

			if (duration < 4) duration = 4;
			if (duration > 9) duration = 9;


			DotEffect.ApplyDot(defender, duration, attacker, Utility.RandomMinMax(2, 5));

			attacker.SendMessage(33, "The Gaia's Scimitar bursts with power!");
			attacker.PlaySound(0x208);

			m_NextArtifactAttackAllowed = DateTime.UtcNow + TimeSpan.FromMinutes(2);
		}

		public Artifact_GaiasScimitar( Serial serial ) : base( serial )
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
