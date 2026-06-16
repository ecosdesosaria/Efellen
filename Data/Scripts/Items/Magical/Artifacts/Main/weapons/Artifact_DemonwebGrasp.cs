using System;
using System.Collections;
using Server.Network;
using Server.Items;
using Server.Targeting;
using Server.Mobiles;
using Server.Engines.PartySystem;
using Server.Guilds;
using Server.EffectsUtil;

namespace Server.Items
{
	public class Artifact_DemonwebGrasp : GiftMaul
	{
		private static Hashtable m_Table = new Hashtable();

		private DateTime m_NextArtifactAttackAllowed;

		public override int InitMinHits { get { return 80; } }
		public override int InitMaxHits { get { return 160; } }

		private class PoisonCurseTimer : Timer
		{
			private Mobile m_Target;
			private ResistanceMod m_Mod;

			public PoisonCurseTimer(Mobile target, ResistanceMod mod) : base(TimeSpan.FromSeconds(30.0))
			{
				m_Target = target;
				m_Mod = mod;
			}

			protected override void OnTick()
			{
				if (m_Target != null && !m_Target.Deleted && m_Mod != null)
					m_Target.RemoveResistanceMod(m_Mod);

				if (m_Target != null)
					m_Table.Remove(m_Target);
			}
		}

		[Constructable]
		public Artifact_DemonwebGrasp()
		{
			Name = "Grasp of the Demonweb";
			Hue = 0x922;
			WeaponAttributes.HitPoisonArea = 50;
			Attributes.SpellChanneling = 1;
			Attributes.BonusStr = 30;
			ArtifactLevel = 2;
			MinDamage = MinDamage + 12;
			MaxDamage = MaxDamage + 12;
			Server.Misc.Arty.ArtySetup(this, "Carrega o beijo devastador de Lolth");
			m_NextArtifactAttackAllowed = DateTime.MinValue;
		}

		public override void GetDamageTypes(Mobile wielder, out int phys, out int fire, out int cold, out int pois, out int nrgy, out int chaos, out int direct)
		{
			phys = 0;
			cold = 0;
			fire = 0;
			nrgy = 0;
			pois = 100;
			chaos = 0;
			direct = 0;
		}

		public override bool OnEquip(Mobile from)
		{
			if (from.Karma > -15000)
			{
				from.SendMessage(0x922, "A corrupção de Lolth te abate!");

				from.Hits = 1;
				from.Mana = 1;
				from.Stam = 1;

				from.FixedParticles(0x3709, 10, 30, 5052, 0x922, 0, EffectLayer.Waist);
				from.BoltEffect(0);
			}

			return base.OnEquip(from);
		}

		private int GetLossAmount( int max, int percent )
		{
			int amount = (int)( (double)max * (double)percent / 100.0 );

			if ( amount < 1 )
				amount = 1;

			return amount;
		}

		public override void OnHit(Mobile attacker, Mobile defender, double damageBonus)
		{
			base.OnHit(attacker, defender, damageBonus);

			if (attacker == null || defender == null || attacker.Deleted || defender.Deleted || !attacker.Alive || !defender.Alive || attacker.Map == null || defender.Map == null)
				return;

			if ( attacker.Karma > -15000 )
			{
				attacker.SendMessage( 0x922, "O beijo de Lolth queima sua alma!" );

				attacker.Hits -= GetLossAmount( attacker.HitsMax, 5 );
				attacker.Mana -= GetLossAmount( attacker.ManaMax, 5 );
				attacker.Stam -= GetLossAmount( attacker.StamMax, 5 );

				if ( attacker.Hits < 1 )
					attacker.Hits = 1;

				if ( attacker.Mana < 1 )
					attacker.Mana = 1;

				if ( attacker.Stam < 1 )
					attacker.Stam = 1;

				attacker.FixedParticles( 0x3709, 10, 30, 5052, 0x922, 0, EffectLayer.Waist );
				attacker.BoltEffect( 0 );

				return;
			}

			if (DateTime.UtcNow < m_NextArtifactAttackAllowed)
				return;

			if (m_Table.ContainsKey(defender))
				return;

			if (Utility.RandomDouble() > 0.05)
				return;


			m_NextArtifactAttackAllowed = DateTime.UtcNow + TimeSpan.FromSeconds(60.0);

			int penalty = GetResistancePenalty(attacker);

			if (penalty < 1)
				return;

			ResistanceMod mod = new ResistanceMod(ResistanceType.Poison, -penalty);

			defender.AddResistanceMod(mod);

			PoisonCurseTimer timer = new PoisonCurseTimer(defender, mod);

			m_Table[defender] = timer;

			timer.Start();

			attacker.SendMessage(0x922, "Lolth devasta seu inimigo com sua vileza!");

			defender.FixedParticles(0x374A, 10, 15, 5038, 0x922, 0, EffectLayer.Waist);
			defender.BoltEffect(0);
		}

		private int GetResistancePenalty(Mobile attacker)
		{
			double total = 0.0;

			int luck = attacker.Luck;

			if (luck > 2000)
				luck = 2000;

			total += luck * 0.00375; // 7.5 / 2000

			double skill = attacker.Skills[SkillName.Bludgeoning].Base;

			if (skill > 125.0)
				skill = 125.0;

			total += skill * 0.06; // 7.5 / 125

			int penalty = (int)total;

			if (penalty < 1)
				penalty = 1;

			return penalty;
		}

		public Artifact_DemonwebGrasp(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.WriteEncodedInt(1);
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