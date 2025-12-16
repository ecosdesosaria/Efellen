using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Engines.PartySystem;
using Server.EffectsUtil;

namespace Server.Items
{
	public class Artifact_TalonOfNightmares : GiftWarFork
	{
		private DateTime m_NextAoE;

		[Constructable]
		public Artifact_TalonOfNightmares()
		{
			Name = "Talon Of Nightmares";
			Hue = 0x96;
			ItemID = 0x1405;
			Slayer = SlayerName.WizardSlayer;
			WeaponAttributes.HitLowerDefend = 30;
            Attributes.SpellChanneling = 1;
			Attributes.WeaponDamage = 30;
			WeaponAttributes.SelfRepair = 10;
			WeaponAttributes.HitLeechMana = 30;
            Attributes.BonusInt = 10;
			ArtifactLevel = 2;
			Server.Misc.Arty.ArtySetup( this, "Spreads Insanity" );
		}

		public override void OnHit(Mobile attacker, Mobile defender, double damageBonus)
		{
			base.OnHit(attacker, defender, damageBonus);
			if (attacker == null || defender == null)
				return;
			//only works for people serious about the mad genius business
			if (attacker.Skills[SkillName.Psychology].Value > 75.0 && attacker.Int > 75)
			{
				if (DateTime.UtcNow < m_NextAoE)
    	        return;
    	    	double skill = attacker.Skills[SkillName.Psychology].Value; 
    	    	double chance = 0.05 + (skill / 125.0) * 0.20; // 5% chant at 0 skill, 25% chance at 125 skill
    	    	if (Utility.RandomDouble() > chance)
    	    	    return;
    	    	double seconds = 120.0 - (skill * (90.0 / 125.0)); // 120secs cooldown at 0 skill, 30 secs cooldown at 125 skill
    	    	m_NextAoE = DateTime.UtcNow + TimeSpan.FromSeconds(seconds);
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
    	    	    // Only smite intellingent things
    	    	    if (mob.Karma > 0 && mob.Int >= 30)
    	    	        continue;
					// adds damage based on how smart the target is
					// adds up to 18 damage when hitting something with 300+ int
					int bonus = 0;
					if (mob.Int >= 300)
						bonus = 18;
					if (mob.Int >= 30 && mob.Int < 300)
					{
					    int scaled = (int)(mob.Int * 0.06);
						if (scaled < 1) scaled = 0;
					    if (scaled > 18) scaled = 18;
					    bonus = scaled;
					}
					// zapping!
					int dmg = Utility.RandomMinMax(minDmg, maxDmg) + bonus;
					if (dmg > 0)
					{
						AOS.Damage(mob, attacker, dmg, 0, 0, 0, 0, 100);
						mob.PlaySound(0x208);
       				}
    	    	}
    			attacker.SendMessage("Your Nightmarish weapon unleashes a burst of psychic energy!");
				SlamVisuals.SlamVisual(attacker, 8, 0x36B0, 0x81b);
			}
    	}

		public override bool OnEquip(Mobile from)
        {
            if (from.Int < 75)
            {
                from.SendMessage("This strange implement refuses to be wielded by you!");
                return false;
            }

            return base.OnEquip(from);
        }

		public Artifact_TalonOfNightmares(Serial serial) : base(serial)
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.WriteEncodedInt( (int) 0 ); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize( reader );
			ArtifactLevel = 2;
			int version = reader.ReadEncodedInt();
		}
	}
}