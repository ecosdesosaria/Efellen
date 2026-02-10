using System;
using Server;
using Server.Network;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Items
{
	public class Artifact_ScepterOfBlasting : GiftStave
	{
		public override int InitMinHits{ get{ return 80; } }
		public override int InitMaxHits{ get{ return 160; } }
		
		private DateTime m_NextArtifactAttackAllowed;

		[Constructable]
		public Artifact_ScepterOfBlasting()
		{
			Name = "Scepter of Blasting";
			Hue = 0x0213;
			ItemID = 0x63A0;
			Attributes.SpellChanneling = 1;
			Attributes.AttackChance = 10;
			Attributes.SpellDamage = 20;
			MinDamage = MinDamage + 1;
			MaxDamage = MaxDamage + 1;
			ArtifactLevel = 2;
			Server.Misc.Arty.ArtySetup( this, "Bursts with energy" );
			m_NextArtifactAttackAllowed = DateTime.MinValue;
		}
		
		public override void OnHit(Mobile attacker, Mobile defender, double damageBonus)
		{
			base.OnHit(attacker, defender, damageBonus);
			
			if (attacker == null || defender == null || !defender.Alive)
				return;

			if (DateTime.UtcNow < m_NextArtifactAttackAllowed)
		    	return;

			double focus = attacker.Skills[SkillName.Focus].Value;
			double psychology = attacker.Skills[SkillName.Psychology].Value;
			
			double avgSkill = (focus + psychology) / 2.0;
			double chance = (avgSkill / 125.0) * 0.12;
			
		

			if (Utility.RandomDouble() > chance)
				return;

			double seconds = 120.0 - (avgSkill * (90.0 / 125.0));
		    m_NextArtifactAttackAllowed = DateTime.UtcNow + TimeSpan.FromSeconds(seconds);
			
			int missiles = GetMissileCount(psychology);
			
			FireEnergyMissiles(attacker, defender, missiles);
		}
		
		private int GetMissileCount(double psychology)
		{
			if (psychology >= 125.0)
				return 6;
			else if (psychology >= 115.0)
				return 5;
			else if (psychology >= 105.0)
				return 4;
			else if (psychology >= 95.0)
				return 3;
			else if (psychology >= 85.0)
				return 2;
			else
				return 1;
		}
		
		private void FireEnergyMissiles(Mobile caster, Mobile target, int missiles)
		{
			if (caster == null || target == null)
				return;
			
			int intelligence = caster.Int;
			
			for (int i = 0; i < missiles; i++)
			{
				int currentMissile = i;
				Timer.DelayCall(TimeSpan.FromSeconds(currentMissile * 0.3), delegate()
				{
					if (target == null || !target.Alive || target.Deleted)
						return;
					
					Effects.SendMovingEffect(
						caster,
						target,
						0x379F,
						7,
						0,
						false,
						false,
						0x0213,
						0
					);
					
					Effects.PlaySound(caster.Location, caster.Map, 0x1F5);
					
					Timer.DelayCall(TimeSpan.FromSeconds(0.5), delegate()
					{
						if (target == null || !target.Alive || target.Deleted)
							return;
						
						int damage = Utility.RandomMinMax(4, 9 + (intelligence / 15)) ;
						
						AOS.Damage(target, caster, damage, 0, 0, 0, 0, 100);
						
						Effects.SendLocationEffect(
							target.Location,
							target.Map,
							0x3709,
							10,
							30,
							0x0213,
							0
						);
						target.FixedParticles(0x36BD, 6, 10, 5044, 0x0213, 0, EffectLayer.Waist);
					});
				});
			}
		}
		
		public Artifact_ScepterOfBlasting( Serial serial ) : base( serial )
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