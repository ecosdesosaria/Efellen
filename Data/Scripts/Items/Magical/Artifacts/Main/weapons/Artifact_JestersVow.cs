using System;
using Server;

namespace Server.Items
{
	public class Artifact_JestersVow : GiftSpear
	{
		public override int InitMinHits { get { return 80; } }
		public override int InitMaxHits { get { return 160; } }

		private DateTime m_NextSpecialAttack;

		[Constructable]
		public Artifact_JestersVow()
		{
			Hue = 0x0845;
			Name = "Jester's Vow";
			ItemID = 0xF62;
			SkillBonuses.SetValues(0, SkillName.Stealing, 15);
			SkillBonuses.SetValues(1, SkillName.Hiding, 15);
			WeaponAttributes.HitLeechStam = 10;
			Attributes.AttackChance = 10;
			Attributes.WeaponDamage = 15;
			ArtifactLevel = 2;
			Server.Misc.Arty.ArtySetup(this, "A ganância o libertará");
			m_NextSpecialAttack = DateTime.MinValue;
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

			if (DateTime.Now < m_NextSpecialAttack)
				return;

			double stealing = attacker.Skills[SkillName.Stealing].Value;

			double chance = 5.0 + (stealing / 25.0);

			if (Utility.RandomDouble() > (chance / 100.0))
				return;

			if (stealing < Utility.RandomMinMax(1, 250))
				return;

			Container pack = defender.Backpack;

			if (pack == null)
				return;

			Gold gold = null;

			for (int i = 0; i < pack.Items.Count; i++)
			{
				Gold g = pack.Items[i] as Gold;

				if (g != null && !g.Deleted && g.Amount > 0)
				{
					gold = g;
					break;
				}
			}

			if (gold == null)
				return;

			gold.MoveToWorld(defender.Location, defender.Map);

			attacker.SendMessage(0x845, "O Juramento Sanguinário derrama as riquezas de seu inimigo no chão!");
            if(defender.Player)
            {	
                defender.SendMessage(0x22, "Seu ouro se derrama de sua mochila!");				
            }


			defender.FixedParticles(0x376A, 9, 32, 5030, 0x845, 0, EffectLayer.Waist);
			defender.PlaySound(0x2E6);

			m_NextSpecialAttack = DateTime.Now + TimeSpan.FromMinutes(1.0);
		}


		public Artifact_JestersVow(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int)1); // version
			writer.Write(m_NextSpecialAttack);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			ArtifactLevel = 2;
			int version = reader.ReadInt();
			switch (version)
			{
				case 1:
					{
						m_NextSpecialAttack = reader.ReadDateTime();
						break;
					}
				case 0:
					{
						m_NextSpecialAttack = DateTime.MinValue;
						break;
					}
			}
		}
	}
}