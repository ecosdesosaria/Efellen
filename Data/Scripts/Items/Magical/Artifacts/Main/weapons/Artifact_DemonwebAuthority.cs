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
	public class Artifact_DemonwebAuthority : GiftElvenCompositeLongbow
	{
		private static Hashtable m_Table = new Hashtable();

		private DateTime m_NextArtifactAttackAllowed;

		public override int InitMinHits { get { return 80; } }
		public override int InitMaxHits { get { return 160; } }

		[Constructable]
		public Artifact_DemonwebAuthority()
		{
			Name = "Authority of the Demonweb";
			Hue = 0x922;
			WeaponAttributes.HitPoisonArea = 50;
			WeaponAttributes.HitDispel = 50;
			Attributes.SpellChanneling = 1;
			Attributes.BonusInt = 15;
			ArtifactLevel = 2;
			MinDamage = MinDamage + 12;
			MaxDamage = MaxDamage + 12;
			Server.Misc.Arty.ArtySetup(this, "Carrega o beijo dominador de Lolth");
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

		private static Hashtable m_VenomTable = new Hashtable();

		private static readonly int[] CloudGraphics = new int[]
		{
	0x3729, 0x372B, 0x372C, 0x372D,
	0x372E, 0x372F, 0x3730, 0x3731,
	0x3733, 0x3734
		};

		private class VenomData
		{
			public int Stacks;
			public ResistanceMod Mod;
			public VenomExpireTimer Timer;

			public VenomData(int stacks, ResistanceMod mod, VenomExpireTimer timer)
			{
				Stacks = stacks;
				Mod = mod;
				Timer = timer;
			}
		}

		private class VenomExpireTimer : Timer
		{
			private Mobile m_Target;

			public VenomExpireTimer(Mobile target) : base(TimeSpan.FromSeconds(15.0))
			{
				m_Target = target;
				Priority = TimerPriority.OneSecond;
			}

			protected override void OnTick()
			{
				ClearVenom(m_Target);
			}
		}

		public override void OnHit(Mobile attacker, Mobile defender, double damageBonus)
		{
			base.OnHit(attacker, defender, damageBonus);

			if (attacker == null || defender == null || attacker.Map == null || defender.Map == null || defender.Deleted || attacker.Deleted || !defender.Alive || !attacker.Alive)
				return;

			if (attacker.Karma > -15000)
				return;

			if (Utility.RandomDouble() > 0.25)
				return;

			ApplyVenomStack(attacker, defender);
		}

		private void ApplyVenomStack(Mobile attacker, Mobile defender)
		{
			VenomData data = m_VenomTable[defender] as VenomData;

			if (data == null)
			{
				ResistanceMod mod = new ResistanceMod(ResistanceType.Poison, -2);

				defender.AddResistanceMod(mod);

				VenomExpireTimer timer = new VenomExpireTimer(defender);

				data = new VenomData(1, mod, timer);

				m_VenomTable[defender] = data;

				timer.Start();
			}
			else
			{
				if (data.Timer != null)
					data.Timer.Stop();

				if (data.Mod != null)
					defender.RemoveResistanceMod(data.Mod);

				if (data.Stacks < 5)
					data.Stacks++;

				int penalty = data.Stacks * 2;

				data.Mod = new ResistanceMod(ResistanceType.Poison, -penalty);

				defender.AddResistanceMod(data.Mod);

				data.Timer = new VenomExpireTimer(defender);
				data.Timer.Start();
			}

			attacker.SendMessage(0x922, "Lolth afirma sua autoridade!");

			if (data.Stacks >= 5)
				TryPoisonBurst(attacker, defender);
		}

		private void TryPoisonBurst(Mobile attacker, Mobile defender)
		{
            if (attacker == null || defender == null || attacker.Deleted || defender.Deleted || !attacker.Alive || !defender.Alive)
				return;

			if (DateTime.UtcNow < m_NextArtifactAttackAllowed)
				return;

			m_NextArtifactAttackAllowed = DateTime.UtcNow + TimeSpan.FromSeconds(30.0);

			attacker.SendMessage(0x922, "A vontade de Lolth é absoluta!");

			Map map = defender.Map;

			if (map == null)
				return;

			int damagePercent = GetBurstPercent(attacker);
			int cloudLen = CloudGraphics.Length;

			IPooledEnumerable eable = map.GetMobilesInRange(defender.Location, 5);

			try
			{
				foreach (Mobile m in eable)
				{
					if (m == null || m.Deleted || !m.Alive || m == attacker)
						continue;

					if (!attacker.CanBeHarmful(m, false))
						continue;

					attacker.DoHarmful(m);

					int damage = (m.HitsMax * damagePercent) / 100;

					if (damage < 1)
						damage = 1;

					m.Hits = Math.Max(0, m.Hits - damage);

					int gfx = CloudGraphics[Utility.Random(cloudLen)];

					Effects.SendLocationParticles(
						EffectItem.Create(m.Location, m.Map, EffectItem.DefaultDuration),
						gfx,
						9,
						20,
						0x922,
						0,
						0,
						0
					);

					m.FixedParticles(0x374A, 10, 15, 5021, 0x922, 0, EffectLayer.Waist);
				}
			}
			finally
			{
				eable.Free();
			}

			ClearVenom(defender);
		}

		private int GetBurstPercent(Mobile attacker)
		{
			double percent = 5.0;

			double poisoning = attacker.Skills[SkillName.Poisoning].Base;

			if (poisoning > 125.0)
				poisoning = 125.0;

			percent += (poisoning / 125.0) * 7.5;

			return (int)percent;
		}

		private static void ClearVenom(Mobile target)
		{
			if (target == null)
				return;

			VenomData data = m_VenomTable[target] as VenomData;

			if (data == null)
				return;

			if (data.Timer != null)
				data.Timer.Stop();

			if (data.Mod != null)
				target.RemoveResistanceMod(data.Mod);

			m_VenomTable.Remove(target);
		}

		public Artifact_DemonwebAuthority(Serial serial) : base(serial)
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