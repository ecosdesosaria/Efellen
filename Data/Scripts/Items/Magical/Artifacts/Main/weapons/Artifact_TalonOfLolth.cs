using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Engines.PartySystem;
using Server.EffectsUtil;

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
			//only works for people serious about the vile poisoner thing
			if (attacker.Skills[SkillName.Poisoning].Value > 75.0 && attacker.Karma < - 7777)
			{
				if (DateTime.UtcNow < m_NextArtifactAttackAllowed)
    	        return;
    	    	double skill = attacker.Skills[SkillName.Poisoning].Value; 
    	    	double chance = 0.05 + (skill / 125.0) * 0.20; // 5% chant at 0 skill, 25% chance at 125 skill
    	    	if (Utility.RandomDouble() > chance)
    	    	    return;
    	    	double seconds = 120.0 - (skill * (90.0 / 125.0)); // 120secs cooldown at 0 skill, 30 secs cooldown at 125 skill
    	    	m_NextArtifactAttackAllowed = DateTime.UtcNow + TimeSpan.FromSeconds(seconds);
				int minDmg = attacker.Int / 9; // 16 base min damage at 150 int
		    	int maxDmg = attacker.Int / 5; // 30 base max damage at 150 int
		    	if (minDmg < 0) minDmg = 0;
		    	if (maxDmg < 0) maxDmg = 0;
		    	if (maxDmg < minDmg) maxDmg = minDmg;
    	    	foreach (Mobile mob in defender.GetMobilesInRange(8))
    	    	{
    	    	    if (mob == null || mob == attacker || mob == defender)
    	    	        continue;
					// dont zap pets/owned summoned creatures
    	    	    if (mob is BaseCreature)
    	    	    {
    	    	        BaseCreature bc = (BaseCreature)mob;
    	    	        if ((bc.Controlled && bc.ControlMaster == attacker) || (bc.Summoned && bc.SummonMaster == attacker))
    	    	            continue;
    	    	    }
    	    	    // don't zap party members
    	    	    Party attackerParty = Party.Get(attacker);
    	    	    Party mobParty = Party.Get(mob);
					if (attackerParty != null && mobParty != null && attackerParty == mobParty)
						continue;
    	    	    // dont zap guild mates
    	    	    if (attacker.Guild != null && mob.Guild != null && attacker.Guild == mob.Guild)
    	    	        continue;
    	    	    // Only smite good things
    	    	    if (mob.Karma < 0)
    	    	        continue;
					// adds damage based on how goood the target is
					// zaps gooder people harder
                    int bonus = 0;
                    if (mob.Karma > 0)
                    {
                        int scaled = 1 + ((mob.Karma) * 24 / 15000);
                        if (scaled < 1) scaled = 0;
                        if (scaled > 25) scaled = 25;
                        bonus = scaled;
                    }
					// zap!
                    int dmg = Utility.RandomMinMax(minDmg, maxDmg) + bonus;

                    if (dmg > 0)
                    {
                        AOS.Damage(mob, attacker, dmg, 0, 0, 0, 100, 0);//poison damage
                        mob.PlaySound(0x1FE);
                     }
    	    	}
    			attacker.SendMessage("Your Nightmarish weapon unleashes a burst of ghostly poison fumes!");
				SlamVisuals.SlamVisual(attacker, 8, 0x36B0, 0x4F6);
			}
    	}

		public override bool OnEquip(Mobile from)
        {
            if (from.Karma > 0)
            {
                from.SendMessage("This vile weapon refuses to be wielded by you!");
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