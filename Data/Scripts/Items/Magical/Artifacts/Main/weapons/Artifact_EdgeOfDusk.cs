using System;
using Server.Network;
using Server.Items;

namespace Server.Items
{
	public class Artifact_EdgeOfDusk : GiftNoDachi
	{
		public override int InitMinHits { get { return 80; } }
		public override int InitMaxHits { get { return 160; } }

		private DateTime m_NextSpecialAttack;

		[Constructable]
		public Artifact_EdgeOfDusk()
		{
			Name = "Beira do Crepúsculo";
			Hue = 0x0455;
			Attributes.SpellChanneling = 1;
			WeaponAttributes.HitLeechStam = 30;
			WeaponAttributes.HitLeechMana = 30;
			Slayer = SlayerName.Repond;
			MinDamage = MinDamage + 2;
			MaxDamage = MaxDamage + 2;
			ArtifactLevel = 2;
			Server.Misc.Arty.ArtySetup(this, "O dever do crepúsculo é envolver todos na ausência.");
			m_NextSpecialAttack = DateTime.MinValue;
		}

		public override void GetDamageTypes(
	Mobile wielder,
	out int phys,
	out int fire,
	out int cold,
	out int pois,
	out int nrgy,
	out int chaos,
	out int direct)
		{
			phys = 50;
			cold = 50;
			fire = 0;
			pois = 0;
			nrgy = 0;
			chaos = 0;
			direct = 0;
		}

		public override bool OnEquip(Mobile from)
		{
			if (from.Fame < 10000)
			{
				from.SendMessage("A Beira do Crepúsculo te julga indigno.");
				return false;
			}

			return base.OnEquip(from);
		}

		public override void OnHit(Mobile attacker, Mobile defender, double damageBonus)
		{
			base.OnHit(attacker, defender, damageBonus);

			if (attacker == null || defender == null || attacker.Map == null || defender.Map == null || defender.Deleted || attacker.Deleted)
				return;

			if (attacker.Karma >= 0)
				return;

			if (attacker.Skills[SkillName.Bushido].Base < 111.0)
				return;

			if (DateTime.Now < m_NextSpecialAttack)
				return;

			if (Utility.RandomDouble() > 0.20)
				return;

			m_NextSpecialAttack = DateTime.Now + TimeSpan.FromSeconds(2.5);

			attacker.SendMessage(0x455, "Beira do Crepúsculo traz a noite!");

			DoSpecialAttack(attacker);
		}

		private void DoSpecialAttack(Mobile attacker)
		{
			Map map = attacker.Map;

			if (map == null)
				return;

			int damage = 75 + (int)(attacker.Skills[SkillName.Bushido].Value / 2.0);

			int xOffset = 0;
			int yOffset = 0;

			switch (attacker.Direction & Direction.Mask)
			{
				case Direction.North:
					yOffset = -1;
					break;
				case Direction.Right:
					xOffset = 1;
					yOffset = -1;
					break;
				case Direction.East:
					xOffset = 1;
					break;
				case Direction.Down:
					xOffset = 1;
					yOffset = 1;
					break;
				case Direction.South:
					yOffset = 1;
					break;
				case Direction.Left:
					xOffset = -1;
					yOffset = 1;
					break;
				case Direction.West:
					xOffset = -1;
					break;
				case Direction.Up:
					xOffset = -1;
					yOffset = -1;
					break;
			}

			for (int i = 1; i <= 5; i++)
			{
				Point3D p = new Point3D(
					attacker.X + (xOffset * i),
					attacker.Y + (yOffset * i),
					map.GetAverageZ(
						attacker.X + (xOffset * i),
						attacker.Y + (yOffset * i))
				);

				if (!map.CanSpawnMobile(p))
					break;

				new AttackTimer(
					attacker,
					p,
					damage,
					TimeSpan.FromMilliseconds((i - 1) * 250)).Start();
			}
		}

		private class AttackTimer : Timer
		{
			private Mobile m_Attacker;
			private Point3D m_Point;
			private int m_Damage;

			public AttackTimer(
				Mobile attacker,
				Point3D point,
				int damage,
				TimeSpan delay)
				: base(delay)
			{
				m_Attacker = attacker;
				m_Point = point;
				m_Damage = damage;
			}

			protected override void OnTick()
			{
				if (m_Attacker == null || m_Attacker.Deleted)
					return;

				Map map = m_Attacker.Map;

				if (map == null)
					return;

				Effects.SendLocationParticles(
					EffectItem.Create(
						m_Point,
						map,
						EffectItem.DefaultDuration),
					0x3709,
					10,
					30,
					0x0455,
					0,
					5052,
					0);

				Effects.PlaySound(m_Point, map, 0x208);

				IPooledEnumerable eable = map.GetMobilesInRange(m_Point, 0);

				try
				{
					foreach (Mobile m in eable)
					{
						if (m == null)
							continue;

						if (m == m_Attacker)
							continue;

						if (!m.Alive)
							continue;

						if (!m_Attacker.CanBeHarmful(m))
							continue;

						m_Attacker.DoHarmful(m);

						AOS.Damage(
							m,
							m_Attacker,
							m_Damage,
							50,
							0,
							50,
							0,
							0);
					}
				}
				finally
				{
					eable.Free();
				}
			}
		}


		public Artifact_EdgeOfDusk(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.WriteEncodedInt(1);
			writer.Write(m_NextSpecialAttack);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			ArtifactLevel = 2;

			int version = reader.ReadEncodedInt();

			if (version >= 1)
				m_NextSpecialAttack = reader.ReadDateTime();
			else
				m_NextSpecialAttack = DateTime.MinValue;
		}
	}
}