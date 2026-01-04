using System;
using Server.Network;
using Server.Items;
using Server.Targeting;
using Server.Mobiles;
using Server.Engines.PartySystem;
using Server.Guilds;
using Server.EffectsUtil;

namespace Server.Items
{
	public class Artifact_CinderForgedMaul : GiftWarMace
	{
		private DateTime m_NextArtifactAttackAllowed;
		
		public override int InitMinHits{ get{ return 80; } }
		public override int InitMaxHits{ get{ return 160; } }

		[Constructable]
		public Artifact_CinderForgedMaul()
		{
			Name = "Cinder Forged Maul";
			ItemID = 0x2682;
			Hue = 0x81b;
			WeaponAttributes.HitFireball = 40;
			WeaponAttributes.HitFireArea = 40;
			Attributes.SpellChanneling = 1;
			Attributes.BonusStr = 10;
			Attributes.AttackChance = 10;
			ArtifactLevel = 2;
			Server.Misc.Arty.ArtySetup( this, "Sets the ground ablaze" );
			m_NextArtifactAttackAllowed = DateTime.MinValue;
		}

		public override void OnHit(Mobile attacker, Mobile defender, double damageBonus)
		{
			base.OnHit(attacker, defender, damageBonus);

			if (attacker == null || defender == null)
				return;

			if (attacker.Skills[SkillName.Bludgeoning].Value <= 105.0 || attacker.Str <= 111)
				return;

			if (DateTime.UtcNow < m_NextArtifactAttackAllowed)
				return;

			double skill = attacker.Skills[SkillName.Bludgeoning].Value;
			double chance = 0.05 + (skill / 125.0) * 0.20;

			if (Utility.RandomDouble() > chance)
				return;

			double seconds = 120.0 - (skill * (90.0 / 125.0));
			m_NextArtifactAttackAllowed = DateTime.UtcNow + TimeSpan.FromSeconds(seconds);

			int minDmg = attacker.Str / 12;
			int maxDmg = attacker.Str / 5;
			
			if (minDmg < 0) minDmg = 0;
			if (maxDmg < minDmg) maxDmg = minDmg;

			Party attackerParty = Party.Get(attacker);
			BaseGuild attackerGuild = attacker.Guild;

			IPooledEnumerable eable = defender.GetMobilesInRange(8);
			
			foreach (Mobile mob in eable)
			{
				if (mob == null || mob == attacker || mob == defender)
					continue;

				if (mob is BaseCreature)
				{
					BaseCreature bc = (BaseCreature)mob;
					if ((bc.Controlled && bc.ControlMaster == attacker) || (bc.Summoned && bc.SummonMaster == attacker))
						continue;
				}

				if (attackerParty != null)
				{
					Party mobParty = Party.Get(mob);
					if (mobParty != null && attackerParty == mobParty)
						continue;
				}

				if (attackerGuild != null && mob.Guild != null && attackerGuild == mob.Guild)
					continue;

				int distance = (int)(attacker.GetDistanceToSqrt(mob));
				int bonus;

				if (distance <= 1)
					bonus = 5;
				else if (distance <= 3)
					bonus = 4;
				else if (distance <= 5)
					bonus = 3;
				else if (distance <= 7)
					bonus = 1;
				else
					bonus = 0;

				int dmg = Utility.RandomMinMax(minDmg + bonus, maxDmg + bonus);
				
				if (dmg > 0)
				{
					AOS.Damage(mob, attacker, dmg, 0, 100, 0, 0, 0);
					mob.PlaySound(0x208);
				}
			}
			
			eable.Free();

			attacker.SendMessage("Your Maul sets the ground ablaze!");
			SlamVisuals.SlamVisual(attacker, 8, 0x36B0, 1160);
		}

		public Artifact_CinderForgedMaul( Serial serial ) : base( serial )
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