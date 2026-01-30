using System;
using Server.Network;
using Server.Items;
using Server.Targeting;
using Server.Mobiles;
using Server.Engines.PartySystem;
using Server.EffectsUtil;
using Server.Guilds;

namespace Server.Items
{

	public class Artifact_BladeOfInsanity : GiftKatana
	{
		private DateTime m_NextArtifactAttackAllowed;
		public override int InitMinHits{ get{ return 80; } }
		public override int InitMaxHits{ get{ return 160; } }

		[Constructable]
		public Artifact_BladeOfInsanity()
		{
			Name = "Blade of Insanity";
			Hue = 0x76D;
			ItemID = 0x13FF;
			WeaponAttributes.HitLeechStam = 50;
			Attributes.WeaponSpeed = 25;
			ArtifactLevel = 2;
			Server.Misc.Arty.ArtySetup( this, "Herald of disease" );
		}

		public override void OnHit(Mobile attacker, Mobile defender, double damageBonus)
		{
		    base.OnHit(attacker, defender, damageBonus);

		    if (attacker == null || defender == null)
		        return;

		    if (attacker.Skills[SkillName.Poisoning].Value > 105.0  && attacker.Dex > 101)
		        return;

		    if (DateTime.UtcNow < m_NextArtifactAttackAllowed)
		        return;

		    double skill = attacker.Skills[SkillName.Poisoning].Value;
		    double chance = 0.05 + (skill / 125.0) * 0.20;

		    if (Utility.RandomDouble() > chance)
		        return;

		    double seconds = 120.0 - (skill * (90.0 / 125.0));
		    m_NextArtifactAttackAllowed = DateTime.UtcNow + TimeSpan.FromSeconds(seconds);

		    int minDmg = attacker.Dex / 12;
		    int maxDmg = attacker.Dex / 5;

		    if (minDmg < 0) minDmg = 0;
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

		            int distance = (int)attacker.GetDistanceToSqrt(mob);

		            int bonus;
		            if (distance <= 1)
		                bonus = 7;
		            else if (distance <= 3)
		                bonus = 5;
		            else if (distance <= 5)
		                bonus = 1;
		            else
		                continue;

		            int dmg = Utility.RandomMinMax(minDmg + bonus, maxDmg + bonus);

		            if (dmg > 0)
		            {
		                AOS.Damage(mob, attacker, dmg, 0, 0, 0, 100, 0);
						mob.PlaySound(0x208);
		            }
		        }
		    }
		    finally
		    {
		        eable.Free();
		    }
			attacker.SendMessage("Your blade of insanity unleashes poisonous fire!");
			SlamVisuals.SlamVisual(attacker, 5, 0x36B0, 63);
		}
		public Artifact_BladeOfInsanity( Serial serial ) : base( serial )
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