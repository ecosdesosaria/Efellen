using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Engines.PartySystem;
using Server.EffectsUtil;
using Server.Guilds;

namespace Server.Items
{
    public class Artifact_ShadowBlade : GiftLongsword
    {
        private DateTime m_NextArtifactAttackAllowed;

        [Constructable]
        public Artifact_ShadowBlade()
        {
            Name = "Blade of the Shadows";
            ItemID = 0xF61;
            Hue = 1899;
            Attributes.AttackChance = 10;
            Attributes.SpellChanneling = 1;
            Attributes.SpellDamage = 20;
            WeaponAttributes.HitHarm = 40;
            WeaponAttributes.HitLeechMana = 40;
            ArtifactLevel = 2;
            Server.Misc.Arty.ArtySetup(this, "Reaps the Light");
            m_NextArtifactAttackAllowed = DateTime.MinValue;
        }

        public override void OnHit(Mobile attacker, Mobile defender, double damageBonus)
		{
			base.OnHit(attacker, defender, damageBonus);

			if (attacker == null || defender == null)
				return;

			if (attacker.Skills[SkillName.Knightship].Value <= 75.0 || attacker.Karma >= -7777)
				return;

			if (DateTime.UtcNow < m_NextArtifactAttackAllowed)
				return;

			double skill = attacker.Skills[SkillName.Knightship].Value;
			double chance = 0.05 + (skill / 125.0) * 0.20;

			if (Utility.RandomDouble() > chance)
				return;

			double seconds = 120.0 - (skill * (90.0 / 125.0));
			m_NextArtifactAttackAllowed = DateTime.UtcNow + TimeSpan.FromSeconds(seconds);

			int minDmg = (-attacker.Karma) / 777; // 19 at -15k
            int maxDmg = (-attacker.Karma) / 555; // 27 at -15k

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

				int bonus = 0;
				if (mob.Karma > 0)
				{
					int scaled = 1 + ((mob.Karma) * 24 / 15000);
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

			attacker.SendMessage("Your Shadow Blade erupts with vile darkness!");
			SlamVisuals.SlamVisual(attacker, 8, 0x36B0, 1153);
		}

		public override bool OnEquip(Mobile from)
        {
            if (from.Karma > 0)
            {
                from.SendMessage("This vile blade burns your hands and refuses to be wielded by you!");
                return false;
            }

            return base.OnEquip(from);
        }

        public Artifact_ShadowBlade(Serial serial) : base(serial)
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
