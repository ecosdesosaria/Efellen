using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Engines.PartySystem;
using Server.Guilds;
using Server.EffectsUtil;

namespace Server.Items
{
	public class Artifact_HolySword : GiftLongsword
	{
		private DateTime m_NextArtifactAttackAllowed;

		[Constructable]
		public Artifact_HolySword()
		{
			Name = "Carsomyr";
			Hue = 0x482;
			ItemID = 0xF61;
			Slayer = SlayerName.Silver;
			Attributes.WeaponDamage = 50;
			WeaponAttributes.SelfRepair = 10;
			WeaponAttributes.UseBestSkill = 1;
			WeaponAttributes.HitDispel = 50;
			Attributes.RegenStam = 10;
			ArtifactLevel = 2;
			Server.Misc.Arty.ArtySetup( this, "Smites Evil" );
			m_NextArtifactAttackAllowed = DateTime.MinValue;
		}

		public override void OnHit(Mobile attacker, Mobile defender, double damageBonus)
		{
			base.OnHit(attacker, defender, damageBonus);

			if (attacker == null || defender == null)
				return;

			if (attacker.Skills[SkillName.Knightship].Value <= 75.0 || attacker.Karma <= 7777)
				return;

			if (DateTime.UtcNow < m_NextArtifactAttackAllowed)
				return;

			double skill = attacker.Skills[SkillName.Knightship].Value;
			double chance = 0.05 + (skill / 125.0) * 0.20;

			if (Utility.RandomDouble() > chance)
				return;

			double seconds = 120.0 - (skill * (90.0 / 125.0));
			m_NextArtifactAttackAllowed = DateTime.UtcNow + TimeSpan.FromSeconds(seconds);

			int minDmg = attacker.Karma / 777;
			int maxDmg = attacker.Karma / 555;

			if (minDmg < 0) minDmg = 0;
			if (maxDmg < minDmg) maxDmg = minDmg;

			Party attackerParty = Party.Get(attacker);
			BaseGuild attackerGuild = attacker.Guild;

			IPooledEnumerable eable = defender.GetMobilesInRange(8);

			foreach (Mobile mob in eable)
			{
				if (mob == null || mob == attacker || mob == defender)
					continue;

				if (mob.Karma > 0)
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

				int bonus = 0;
				if (mob.Karma < 0)
				{
					int scaled = 1 + ((-mob.Karma) * 24 / 15000);
					if (scaled < 1) scaled = 0;
					if (scaled > 25) scaled = 25;
					bonus = scaled;
				}

				int dmg = Utility.RandomMinMax(minDmg, maxDmg) + bonus;

				if (dmg > 0)
				{
					AOS.Damage(mob, attacker, dmg, 0, 100, 0, 0, 0);
					mob.PlaySound(0x208);
				}
			}

			eable.Free();

			attacker.SendMessage("Your Holy Sword unleashes a burst of divine fire!");
			SlamVisuals.SlamVisual(attacker, 8, 0x36B0, 1153);
		}

		public override bool OnEquip(Mobile from)
		{
			if (from.Karma < 0)
			{
				from.SendMessage("This holy blade burns your hands and refuses to be wielded by you!");
				return false;
			}

			return base.OnEquip(from);
		}

		public Artifact_HolySword(Serial serial) : base(serial)
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