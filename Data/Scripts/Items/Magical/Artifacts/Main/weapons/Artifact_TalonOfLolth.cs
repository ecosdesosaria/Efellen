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
	public class Artifact_TalonOfLolth : GiftKama
	{
		private DateTime m_NextArtifactAttackAllowed;

		[Constructable]
		public Artifact_TalonOfLolth()
		{
			Name = "Talon Of Lolth";
			Hue = 2498;
			SkillBonuses.SetValues( 0, SkillName.Poisoning, 10);
			SkillBonuses.SetValues( 1, SkillName.Fencing, 10);
			WeaponAttributes.HitLeechStam = 30;
            Attributes.SpellChanneling = 1;
			WeaponAttributes.SelfRepair = 10;
			WeaponAttributes.HitLeechMana = 30;
			MinDamage = MinDamage + 4;
			MaxDamage = MaxDamage + 4;
            Attributes.BonusDex = 10;
			ArtifactLevel = 2;
			Server.Misc.Arty.ArtySetup( this, "Spreads Virulence" );
            m_NextArtifactAttackAllowed = DateTime.UtcNow;
		}

		public override void OnHit(Mobile attacker, Mobile defender, double damageBonus)
		{
		    base.OnHit(attacker, defender, damageBonus);

		    if (attacker == null || defender == null)
		        return;

		    if (attacker.Skills[SkillName.Poisoning].Value > 75.0 && attacker.Karma >= -7777)
		        return;

		    if (DateTime.UtcNow < m_NextArtifactAttackAllowed)
		        return;

		    double skill = attacker.Skills[SkillName.Poisoning].Value;
		    double chance = 0.05 + (skill / 125.0) * 0.20;

		    if (Utility.RandomDouble() > chance)
		        return;

		    double seconds = 115.0 - (skill * (90.0 / 125.0));
		    m_NextArtifactAttackAllowed = DateTime.UtcNow + TimeSpan.FromSeconds(seconds);

			int minDmg = (-attacker.Karma) / 777; 
            int maxDmg = (-attacker.Karma) / 555;
		    if (minDmg < 0) minDmg = 0;
		    if (maxDmg < 0) maxDmg = 0;
		    if (maxDmg < minDmg) maxDmg = minDmg;
    

		    Party attackerParty = Party.Get(attacker);
		    Guild attackerGuild = attacker.Guild as Guild;

		    IPooledEnumerable eable = defender.GetMobilesInRange(5);

		    try
		    {
		        foreach (Mobile mob in eable)
		        {
		            if (mob == null || mob == attacker || mob == defender)
		                continue;

		            if (mob is BaseCreature)
		            {
		                BaseCreature bc = (BaseCreature)mob;
		                if ((bc.Controlled && bc.ControlMaster == attacker) || 
		                    (bc.Summoned && bc.SummonMaster == attacker))
		                    continue;
		            }

        
		            if (attackerParty != null && Party.Get(mob) == attackerParty)
		                continue;

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
						AOS.Damage(mob, attacker, dmg, 0, 0, 0, 100, 0);
       				}
		        }
		    }
		    finally
		    {
		        eable.Free();
		    }
			attacker.SendMessage("Sua arma pesadelo libera uma explosão de vapores venenosos fantasmagóricos!");
			SlamVisuals.SlamVisual(attacker, 5, 0x36B0, 0x4F6);
		}

		public override bool OnEquip(Mobile from)
        {
            if (from.Karma > 0)
            {
                from.SendMessage("Esta arma vil se recusa a ser empunhada por você!");
                return false;
            }

            return base.OnEquip(from);
        }

		public Artifact_TalonOfLolth(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.WriteEncodedInt(1); // version
            
            writer.Write(m_NextArtifactAttackAllowed.Ticks);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadEncodedInt();
            
            if (version >= 1)
            {
                long ticks = reader.ReadLong();
                m_NextArtifactAttackAllowed = new DateTime(ticks);
            }
            else
            {
                m_NextArtifactAttackAllowed = DateTime.UtcNow;
            }
            
            ArtifactLevel = 2;
        }
	}
}