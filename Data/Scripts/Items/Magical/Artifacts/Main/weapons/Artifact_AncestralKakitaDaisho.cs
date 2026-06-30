using System;
using Server.Network;
using Server.Items;
using Server.Targeting;

namespace Server.Items
{
	public class Artifact_AncestralKakitaDaisho : GiftDaisho
	{
		public override int InitMinHits { get { return 80; } }
		public override int InitMaxHits { get { return 160; } }

		private DateTime m_NextSpecialAttack;
		private BuffTimer m_BuffTimer;

		private static readonly TimeSpan EffectDuration = TimeSpan.FromSeconds(30.0);
		private static readonly TimeSpan Cooldown = TimeSpan.FromMinutes(1.0);

		[Constructable]
		public Artifact_AncestralKakitaDaisho()
		{
			Name = "Daisho Ancestral de Kakita";
			Hue = 493;
			WeaponAttributes.HitHarm = 20;
			Attributes.WeaponSpeed = 40;
			Attributes.SpellChanneling = 1;
			MinDamage = MinDamage + 2;
			MaxDamage = MaxDamage + 2;
			ArtifactLevel = 2;
			Server.Misc.Arty.ArtySetup(this, "A perfeição de um clã transformada em aço frio.");
			m_NextSpecialAttack = DateTime.MinValue;
		}

		public override bool OnEquip(Mobile from)
		{
			if (from.Skills[SkillName.Bushido].Value < 101)
			{
				from.SendMessage("A lâmina ancestral dos Kakita não tolera os indignos.");
				return false;
			}

			return base.OnEquip(from);
		}

		public override void GetDamageTypes(Mobile wielder, out int phys, out int fire, out int cold, out int pois, out int nrgy, out int chaos, out int direct)
		{
			cold = phys = 50;

			pois = fire = nrgy = chaos = direct = 0;
		}

		public override void OnHit(Mobile attacker, Mobile defender, double damageBonus)
		{
			base.OnHit(attacker, defender, damageBonus);

			if (attacker == null || defender == null)
				return;

			if (attacker.Deleted || defender.Deleted)
				return;

			if (attacker.Map == null || defender.Map == null)
				return;

			if (DateTime.UtcNow < m_NextSpecialAttack)
				return;

			double chance = 2.5 + (attacker.Skills[SkillName.Bushido].Value / 25.0);

			if (Utility.RandomDouble() * 100.0 >= chance)
				return;

			m_NextSpecialAttack = DateTime.UtcNow + Cooldown;

			attacker.SendMessage("Os ancestrais Kakita observam orgulhosos sobre você.");

			attacker.FixedParticles(0x373A, 10, 15, 5018, 493, 0, EffectLayer.Waist);
			attacker.PlaySound(0x1F5);

			attacker.AddStatMod(new StatMod(StatType.Str, "KakitaStr", 25, EffectDuration));
			attacker.AddStatMod(new StatMod(StatType.Dex, "KakitaDex", 25, EffectDuration));

			attacker.AddSkillMod(new DefaultSkillMod(SkillName.ArmsLore, true, 15.0));
			attacker.AddSkillMod(new DefaultSkillMod(SkillName.Tactics, true, 15.0));

			if (m_BuffTimer != null)
				m_BuffTimer.Stop();

			m_BuffTimer = new BuffTimer(attacker);
			m_BuffTimer.Start();
		}
		private class BuffTimer : Timer
		{
			private Mobile m_Owner;
			private DateTime m_End;

			private SkillMod m_ArmsLore;
			private SkillMod m_Tactics;

			public BuffTimer(Mobile owner)
				: base(TimeSpan.Zero, TimeSpan.FromSeconds(2.0))
			{
				m_Owner = owner;
				m_End = DateTime.UtcNow + EffectDuration;

				m_ArmsLore = new DefaultSkillMod(SkillName.ArmsLore, true, 15.0);
				m_Tactics = new DefaultSkillMod(SkillName.Tactics, true, 15.0);

				owner.AddSkillMod(m_ArmsLore);
				owner.AddSkillMod(m_Tactics);
			}

			protected override void OnTick()
			{
				if (m_Owner == null || m_Owner.Deleted)
				{
					Cleanup();
					return;
				}

				if (DateTime.UtcNow >= m_End)
				{
					Cleanup();

					m_Owner.SendMessage("A bênção dos ancestrais Kakita desaparece.");

					return;
				}

				m_Owner.Stam += 2;

				if (m_Owner.Stam > m_Owner.StamMax)
					m_Owner.Stam = m_Owner.StamMax;

				m_Owner.Mana += 2;

				if (m_Owner.Mana > m_Owner.ManaMax)
					m_Owner.Mana = m_Owner.ManaMax;

				m_Owner.FixedParticles(
					0x373A,
					1,
					15,
					9502,
					493,
					0,
					EffectLayer.Waist);

				m_Owner.PlaySound(0x1F5);
			}

			private void Cleanup()
			{
				Stop();

				if (m_Owner != null && !m_Owner.Deleted)
				{
					m_Owner.RemoveStatMod("KakitaStr");
					m_Owner.RemoveStatMod("KakitaDex");

					if (m_ArmsLore != null)
						m_Owner.RemoveSkillMod(m_ArmsLore);

					if (m_Tactics != null)
						m_Owner.RemoveSkillMod(m_Tactics);
				}
			}
		}
		public override void OnDelete()
		{
			if (m_BuffTimer != null)
			{
				m_BuffTimer.Stop();
				m_BuffTimer = null;
			}

			base.OnDelete();
		}
		public Artifact_AncestralKakitaDaisho(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int)1);
			writer.Write(m_NextSpecialAttack);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			ArtifactLevel = 2;
			int version = reader.ReadInt();

			if (version >= 1)
				m_NextSpecialAttack = reader.ReadDateTime();
			else
				m_NextSpecialAttack = DateTime.MinValue;
		}
	}
}