using System;
using Server.Network;
using Server.Items;
using Server.Targeting;
using Server.Misc;
using Server.Mobiles;
using Server.Gumps;

namespace Server.Items
{
	public class Artifact_LolthsAbsoluteWill : GiftScepter
	{
		private DateTime m_NextSlayTime;
		private Timer m_PoisonTimer;

		[Constructable]
		public Artifact_LolthsAbsoluteWill()
		{
			Name = "Lolth's Absolute Will";
			Hue = 1316;
			Slayer = SlayerName.Repond;
			Attributes.WeaponDamage = 50;
			WeaponAttributes.HitLowerAttack = 50;
			Attributes.RegenStam = 10;
			ArtifactLevel = 2;
			m_NextSlayTime = DateTime.MinValue;
			Server.Misc.Arty.ArtySetup( this, "The will of Lolth" );
		}

		public override bool OnEquip(Mobile from)
		{
			if (from.Karma >= -11999)
			{
				from.SendMessage("This vile implement judges you unworthy!");
				from.ApplyPoison(from, Poison.Deadly);
				return false;
			}

			return base.OnEquip(from);
		}

		public override void OnHit(Mobile attacker, Mobile defender, double damageDealt)
		{
			base.OnHit(attacker, defender, damageDealt);

			if (attacker == null || defender == null || !defender.Alive)
				return;

			if (DateTime.UtcNow < m_NextSlayTime)
				return;

			double slayChance = 1.0;

			if (attacker.Karma < 0)
			{
				double karmaBonus = Math.Min(2.5, (Math.Abs(attacker.Karma) / 15000.0) * 2.5);
				slayChance += karmaBonus;
			}

			double spiritualismSkill = attacker.Skills[SkillName.Spiritualism].Base;
			double spiritualismBonus = Math.Min(2.5, (spiritualismSkill / 125.0) * 2.5);
			slayChance += spiritualismBonus;

			if (Utility.RandomDouble() * 100.0 < slayChance)
			{
				defender.Kill();

				defender.FixedParticles(0x374A, 10, 30, 5052, EffectLayer.Waist);
				defender.PlaySound(0x1FB);
				Effects.SendLocationParticles(
					EffectItem.Create(defender.Location, defender.Map, EffectItem.DefaultDuration),
					0x3728, 10, 10, 2023
				);

				attacker.SendMessage("Lolth smites your foe!");

				m_NextSlayTime = DateTime.UtcNow + TimeSpan.FromMinutes(2);

				attacker.ApplyPoison(attacker, Poison.Deadly);

				if (m_PoisonTimer != null)
					m_PoisonTimer.Stop();

				m_PoisonTimer = new PoisonMaintenanceTimer(attacker, this);
				m_PoisonTimer.Start();
			}
		}

		private class PoisonMaintenanceTimer : Timer
		{
			private Mobile m_Owner;
			private Artifact_LolthsAbsoluteWill m_Weapon;
			private DateTime m_EndTime;

			public PoisonMaintenanceTimer(Mobile owner, Artifact_LolthsAbsoluteWill weapon) 
				: base(TimeSpan.FromSeconds(12.0), TimeSpan.FromSeconds(12.0))
			{
				m_Owner = owner;
				m_Weapon = weapon;
				m_EndTime = DateTime.UtcNow + TimeSpan.FromMinutes(2);
			}

			protected override void OnTick()
			{
				if (DateTime.UtcNow >= m_EndTime)
				{
					Stop();
					return;
				}

				if (m_Owner == null || m_Owner.Deleted || !m_Owner.Alive)
				{
					Stop();
					return;
				}

				if (m_Owner.Poison == null)
				{
					m_Owner.ApplyPoison(m_Owner, Poison.Lethal);
					m_Owner.SendMessage("Lolth's attention comes at a price.");
				}
			}
		}

		public override void OnDelete()
		{
			if (m_PoisonTimer != null)
			{
				m_PoisonTimer.Stop();
				m_PoisonTimer = null;
			}

			base.OnDelete();
		}

		public Artifact_LolthsAbsoluteWill( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 1 ); // version

			writer.Write( m_NextSlayTime );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			ArtifactLevel = 2;
			int version = reader.ReadInt();

			if (version >= 1)
			{
				m_NextSlayTime = reader.ReadDateTime();
			}
		}
	}
}