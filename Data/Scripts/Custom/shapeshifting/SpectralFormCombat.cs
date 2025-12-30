using System;
using System.Collections;
using Server.Items;
using Server.Mobiles;
using Server.Spells;
using Server.Network;
using Server.Misc;
/* 
using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Mobiles;
using Server.Targeting;
using Server.Items;
using Server.Network;
using Server.Spells.Seventh;
using Server.Spells.Fifth;
using Server.Spells.Necromancy;
using Server.Spells;
using Server.Spells.Ninjitsu;
using Server.Misc;
using Server.Systems;

 */

namespace Server.Items
{
	public class SpectralFormCombat
	{
		private static Hashtable m_SpecialCooldowns = new Hashtable();
		private static Hashtable m_BleedTimers = new Hashtable();

		public static void OnHit(Mobile attacker, Mobile defender)
		{
			if (attacker == null || defender == null || !attacker.Alive || !defender.Alive)
				return;
			
			if (!IsMeleeAttack(attacker, defender))
		        return;
			
			if (IsOnSpecialCooldown(attacker))
		        return;

			SpectralFormContext context = HeartOfTheWilds.GetContext(attacker);

			if (context == null)
				return;

			SpectralFormEntry entry = context.Entry;

			double druidism = attacker.Skills[SkillName.Druidism].Value;
			double spiritualism = attacker.Skills[SkillName.Spiritualism].Value;
			double totalSkill = druidism + spiritualism;

			// checks druidism on hit, checks guild membership
			if (attacker.CheckSkill(SkillName.Druidism, 0, 125) && IsInGuild(attacker) && Utility.RandomMinMax(1, 25) < druidism/10)
			{
				int marks = Utility.RandomMinMax(2, 12);
				attacker.AddToBackpack(new MarksOfTheWilds(marks));
				attacker.SendMessage(string.Format("You gained {0} Marks of the Wilds.", marks));
			}

			// Calculate hit chance: (Druidism + Spiritualism) / 10, capped at 25%
			double hitChance = Math.Min((totalSkill / 10.0) / 100.0, 0.25);

			if (entry.BleedOnHit && Utility.RandomDouble() < hitChance)
			{
			    ApplyBleed(attacker, defender, druidism, spiritualism);
			    StartSpecialCooldown(attacker);
			    return;
			}


			if (entry.LightningOnHit && Utility.RandomDouble() < hitChance)
			{
				ApplyLightning(attacker, defender, druidism, spiritualism);
			    StartSpecialCooldown(attacker);
			}

			if (entry.ParalyzeOnHit && Utility.RandomDouble() < hitChance)
			{
				ApplyParalyze(attacker, defender, druidism, spiritualism);
			    StartSpecialCooldown(attacker);
			}

			if (entry.PoisonOnHit && Utility.RandomDouble() < hitChance)
			{
				ApplyPoison(attacker, defender, druidism, spiritualism);
			    StartSpecialCooldown(attacker);
			}

			if (entry.CleaveOnHit && Utility.RandomDouble() < hitChance)
			{
				ApplyCleave(attacker, defender, druidism, spiritualism);
			    StartSpecialCooldown(attacker);
			}
		}

		public static bool IsInGuild( Mobile m )
		{
			return ( m is PlayerMobile && ((PlayerMobile)m).NpcGuild == NpcGuild.DruidsGuild );
		}

		private static bool ApplyCleave(Mobile attacker,Mobile defender,double druidism,double spiritualism)
		{
		    BaseWeapon weapon = attacker.Weapon as BaseWeapon;
		    if (weapon == null)
		        return false;

		    ArrayList targets = new ArrayList();

		    foreach (Mobile m in attacker.GetMobilesInRange(weapon.MaxRange))
		    {
		        if (m == defender)
		            continue;

		        if (m.Combatant != attacker)
		            continue;

		        if (!m.Alive || m.IsDeadBondedPet)
		            continue;

		        targets.Add(m);
		    }

		    if (targets.Count == 0)
		    {
		        attacker.SendMessage("There is no other prey within reach.");
		        return false;
		    }

		    Mobile target = (Mobile)targets[Utility.Random(targets.Count)];

		    double damageBonus =
		        ((druidism + spiritualism) / 2.0) / 100.0;

		    attacker.SendMessage(
		        "You tear through your foe and carry the force of your attack into another!"
		    );

		    target.SendMessage(
		        "A savage cleaving blow rips into you!"
		    );

		    target.FixedParticles(
		        0x37B9, 1, 4, 0x251D, 0, 0, EffectLayer.Waist
		    );

		    attacker.PlaySound(0x510);

		    weapon.OnSwing(attacker, target, damageBonus);

		    return true;
		}

		private static bool IsMeleeAttack(Mobile attacker, Mobile defender)
		{
		    if (!attacker.InRange(defender, 1))
		        return false;

		    Item weapon = attacker.Weapon as Item;

		    if (weapon == null || weapon is Fists)
		        return true;

		    if (weapon is BaseRanged)
		        return false;

		    if (weapon is BaseMeleeWeapon)
		        return true;

		    return false;
		}


		private static void ApplyBleed(Mobile attacker, Mobile defender, double druidism, double spiritualism)
		{
			if (defender.Alive && !defender.IsDeadBondedPet)
			{
				StopBleed(defender);

				// duration: 2 + (druidism + spiritualism) / 25, capped at 7 seconds
				double totalSkill = druidism + spiritualism;
				double duration = Math.Min(2.0 + (totalSkill / 25.0), 7.0);

				defender.SendMessage("You begin to bleed profusely!");
				defender.FixedParticles(0x377A, 1, 40, 9942, 0x26, 0, EffectLayer.Waist);
				defender.PlaySound(0x133);

				BleedTimer timer = new BleedTimer(defender, attacker, duration, druidism, spiritualism);
				m_BleedTimers[defender] = timer;
				timer.Start();
			}
		}

		public static void StopBleed(Mobile defender)
		{
			if (m_BleedTimers.Contains(defender))
			{
				BleedTimer timer = m_BleedTimers[defender] as BleedTimer;
				if (timer != null && timer.Running)
					timer.Stop();
				m_BleedTimers.Remove(defender);
			}
		}

		public static void ClearAllEffects(Mobile m)
		{
			ArrayList toRemove = new ArrayList();
			foreach (DictionaryEntry entry in m_BleedTimers)
			{
				BleedTimer timer = entry.Value as BleedTimer;
				if (timer != null && timer.Attacker == m)
				{
					toRemove.Add(entry.Key);
				}
			}
			foreach (Mobile key in toRemove)
			{
				StopBleed(key);
			}
		}

		private static bool IsOnSpecialCooldown(Mobile m)
		{
		    object o = m_SpecialCooldowns[m];

		    if (o is DateTime)
		        return DateTime.UtcNow < (DateTime)o;

		    return false;
		}

		private static void StartSpecialCooldown(Mobile m)
		{
		    m_SpecialCooldowns[m] = DateTime.UtcNow + TimeSpan.FromMinutes(1.0);
		}

		public static void ClearCooldown(Mobile m)
		{
		    if (m_SpecialCooldowns.Contains(m))
		        m_SpecialCooldowns.Remove(m);
		}



		private static void ApplyLightning(Mobile attacker, Mobile defender, double druidism, double spiritualism)
		{
			if (defender.Alive && !defender.IsDeadBondedPet)
			{
				defender.FixedParticles(0x3818, 1, 11, 0x13A8, 0, 0, EffectLayer.Waist);
				defender.PlaySound(0x29);

				// Scale damage with skills: base 15-25 + (druidism + spiritualism) / 10
				double totalSkill = druidism + spiritualism;
				int baseDamage = Utility.RandomMinMax(15, 25);
				int skillBonus = (int)(totalSkill / 10.0);
				int damage = baseDamage + skillBonus;

				AOS.Damage(defender, attacker, damage, 0, 0, 0, 100, 0);

				defender.SendMessage("You are struck by spectral lightning!");
			}
		}

		private static void ApplyParalyze(Mobile attacker, Mobile defender, double druidism, double spiritualism)
		{
			if (defender.Alive && !defender.IsDeadBondedPet && !defender.Paralyzed)
			{
				// Calculate duration: 2 + (druidism + spiritualism) / 25, capped at 7 seconds
				double totalSkill = druidism + spiritualism;
				double duration = Math.Min(2.0 + (totalSkill / 25.0), 7.0);

				defender.Paralyze(TimeSpan.FromSeconds(duration));
				defender.FixedParticles(0x376A, 9, 32, 0x13AF, EffectLayer.Waist);
				defender.PlaySound(0x204);
				defender.SendMessage("The spectral force paralyzes you!");
			}
		}

		private static void ApplyPoison(Mobile attacker, Mobile defender, double druidism, double spiritualism)
		{
			if (defender.Alive && !defender.IsDeadBondedPet)
			{
				double poisoning = attacker.Skills[SkillName.Poisoning].Value;
				int level = 0;
				// first form requries 80 poisoning to no point in checking for lower poisons
				if (poisoning >= 105.0)
					level = 4; // Deadly
				else
					level = 3; // Greater
				
				Poison poison = Poison.GetPoison(level);

				if (poison != null)
				{
					defender.ApplyPoison(attacker, poison);
					defender.FixedParticles(0x374A, 10, 15, 5021, EffectLayer.Waist);
					defender.PlaySound(0x205);
				}
			}
		}

		private class BleedTimer : Timer
		{
			private Mobile m_Defender;
			private Mobile m_Attacker;
			private double m_Duration;
			private double m_Druidism;
			private double m_Spiritualism;
			private int m_Ticks;

			public Mobile Attacker { get { return m_Attacker; } }

			public BleedTimer(Mobile defender, Mobile attacker, double duration, double druidism, double spiritualism) 
				: base(TimeSpan.FromSeconds(2.0), TimeSpan.FromSeconds(2.0))
			{
				m_Defender = defender;
				m_Attacker = attacker;
				m_Duration = duration;
				m_Druidism = druidism;
				m_Spiritualism = spiritualism;
				m_Ticks = 0;
				Priority = TimerPriority.TwoFiftyMS;
			}

			protected override void OnTick()
			{
				if (!m_Defender.Alive || m_Defender.Deleted || m_Ticks * 2 >= (int)m_Duration)
				{
					SpectralFormCombat.StopBleed(m_Defender);
					Stop();
					return;
				}

				// Scale damage with skills: base 5-10 + (druidism + spiritualism) / 20
				double totalSkill = m_Druidism + m_Spiritualism;
				int baseDamage = Utility.RandomMinMax(5, 10);
				int skillBonus = (int)(totalSkill / 20.0);
				int damage = baseDamage + skillBonus;

				AOS.Damage(m_Defender, m_Attacker, damage, 100, 0, 0, 0, 0);
				m_Defender.FixedParticles(0x377A, 1, 40, 9942, 0x26, 0, EffectLayer.Waist);

				m_Ticks++;
			}
		}
	}
}