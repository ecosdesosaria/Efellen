using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Engines.PartySystem;
using Server.EffectsUtil;

namespace Server.Items
{
	public class Artifact_LeoricsSword : GiftClaymore
	{
		private DateTime m_NextArtifactAttackAllowed;

		[Constructable]
		public Artifact_LeoricsSword()
		{
			Name = "Leoric's Regal Blade";
			Hue = 2515;
			Slayer = SlayerName.Repond;
			WeaponAttributes.SelfRepair = 10;
			Attributes.RegenStam = 10;
			Attributes.RegenHits = 10;
			WeaponAttributes.HitLeechHits = 20;
			WeaponAttributes.HitLeechStam = 20;
			WeaponAttributes.HitLowerDefend = 20;
			ArtifactLevel = 2;
			Server.Misc.Arty.ArtySetup( this, "Smites the living" );
    	    m_NextArtifactAttackAllowed = DateTime.MinValue;
		}

		public override void OnHit(Mobile attacker, Mobile defender, double damageBonus)
		{
			base.OnHit(attacker, defender, damageBonus);
			if (attacker == null || defender == null)
				return;
			//only works for people serious about the evil king business
			if (attacker.Skills[SkillName.ArmsLore].Value > 75.0 && attacker.Karma < 7777)
			{
				if (DateTime.UtcNow < m_NextArtifactAttackAllowed)
    	        return;
    	    	double skill = attacker.Skills[SkillName.ArmsLore].Value; 
    	    	double chance = 0.05 + (skill / 125.0) * 0.20; // 5% chant at 0 skill, 25% chance at 125 skill
    	    	if (Utility.RandomDouble() > chance)
    	    	    return;
    	    	double seconds = 120.0 - (skill * (90.0 / 125.0)); // 120secs cooldown at 0 skill, 30 secs cooldown at 125 skill
    	    	m_NextArtifactAttackAllowed = DateTime.UtcNow + TimeSpan.FromSeconds(seconds);
				int minDmg = (-attacker.Karma) / 777; // 19 at -15k
                int maxDmg = (-attacker.Karma) / 555; // 27 at -15k
		    	if (minDmg < 0) minDmg = 0;
		    	if (maxDmg < 0) maxDmg = 0;
		    	if (maxDmg < minDmg) maxDmg = minDmg;
                SlayerEntry silver = SlayerGroup.GetEntryByName(SlayerName.Silver);
                SlayerEntry exorcism = SlayerGroup.GetEntryByName(SlayerName.Exorcism);
    	    	
                foreach (Mobile mob in defender.GetMobilesInRange(8))
    	    	{
    	    	    if (mob == null || mob == attacker || mob == defender)
    	    	        continue;
					// dont smite pets/owned summoned creatures
    	    	    if (mob is BaseCreature)
    	    	    {
    	    	        BaseCreature bc = (BaseCreature)mob;
    	    	        if ((bc.Controlled && bc.ControlMaster == attacker) || (bc.Summoned && bc.SummonMaster == attacker))
    	    	            continue;
    	    	    }
                    // dont smite undead
                    bool validTarget = false;
                    if ((silver != null && silver.Slays(mob)) || exorcism != null && exorcism.Slays(mob))
                        continue;

    	    	    // don't smite party members
    	    	    Party attackerParty = Party.Get(attacker);
    	    	    Party mobParty = Party.Get(mob);
					if (attackerParty != null && mobParty != null && attackerParty == mobParty)
						continue;
    	    	    // dont smite guild mates
    	    	    if (attacker.Guild != null && mob.Guild != null && attacker.Guild == mob.Guild)
    	    	        continue;
    	    	    
					// smites gooder people harder
                    int bonus = 0;
                    if (mob.Karma > 0)
                    {
                        int scaled = 1 + ((mob.Karma) * 24 / 15000);
                        if (scaled < 1) scaled = 0;
                        if (scaled > 25) scaled = 25;
                        bonus = scaled;
                    }
					// smiting!
					int dmg = Utility.RandomMinMax(minDmg, maxDmg) + bonus;
					if (dmg > 0)
					{
						AOS.Damage(mob, attacker, dmg, 100, 0, 0, 0, 0);
						mob.PlaySound(0x208);
       				}
    	    	}
    			attacker.SendMessage("Your regal Sword unleashes an explosion!");
				SlamVisuals.SlamVisual(attacker, 8, 0x36B0, 0x09d3);
			}
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

		public Artifact_LeoricsSword(Serial serial) : base(serial)
		{
		}

		 public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 1 ); // version
			writer.Write(m_NextArtifactAttackAllowed);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadEncodedInt();
			if (version >= 1)
		        m_NextArtifactAttackAllowed = reader.ReadDateTime();
		    else
		        m_NextArtifactAttackAllowed = DateTime.MinValue;
			ArtifactLevel = 2;
		}
	}
}