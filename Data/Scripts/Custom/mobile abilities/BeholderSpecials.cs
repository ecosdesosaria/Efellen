using System;
using System.Collections.Generic;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Spells;

namespace Server.Custom.BeholderSpecials
{
	public class BeholderSpecials
	{
		private static Dictionary<Mobile, Dictionary<string, DateTime>> m_Cooldowns = new Dictionary<Mobile, Dictionary<string, DateTime>>();

		public static bool AntiMagicEye(Mobile caster, int manaDrain, int cooldown, Mobile target)
		{
			if (caster == null || caster.Deleted || target == null || target.Deleted || !target.Alive)
				return false;

			if (IsOnCooldown(caster, "AntiMagicEye"))
				return false;

			if (!caster.InLOS(target))
				return false;

			int actualDrain = Math.Min(manaDrain, target.Mana);
			target.Mana -= actualDrain;

			target.FixedParticles(0x374A, 10, 15, 5038, 0x9C2, 0, EffectLayer.Head);
            DoRayEffect(caster, target, 0x36D4, 1172, 10);
			target.PlaySound(0x1F8);

			target.SendMessage("Your magical energy is drained by the anti-magic ray!");
			SetCooldown(caster, "AntiMagicEye", cooldown);
        	return true;
		}

		public static bool Disintegration(Mobile caster, int damage, int cooldown, Mobile target)
		{
			if (caster == null || caster.Deleted || target == null || target.Deleted || !target.Alive)
				return false;

			if (IsOnCooldown(caster, "Disintegration"))
				return false;

			if (!caster.InLOS(target))
				return false;

			if (target.Hits >= (target.HitsMax / 2))
			{
				target.SendMessage("The disintegration ray grazes you, but you resist its full power!");
				int reducedDamage = damage / 3;
				AOS.Damage(target, caster, reducedDamage, 0, 0, 0, 0, 100);
			}
			else
			{
				target.SendMessage("The disintegration ray tears through your weakened form!");
				AOS.Damage(target, caster, damage, 0, 0, 0, 0, 100);
			}

			target.FixedParticles(0x3709, 10, 30, 5052, 0x96, 0, EffectLayer.LeftFoot);
            DoRayEffect(caster, target, 0x36D4, 1175, 10);
			target.PlaySound(0x208);

			Effects.SendLocationParticles(target, 0x3728, 8, 20, 0x96, 0, 5042, 0);

			SetCooldown(caster, "Disintegration", cooldown);

			return true;
		}

        public static bool Petrification(Mobile caster, int cooldown, Mobile target)
		{
			if (caster == null || caster.Deleted || target == null || target.Deleted || !target.Alive)
				return false;

			if (IsOnCooldown(caster, "Petrification"))
				return false;

			if (!caster.InLOS(target))
				return false;

			int resist = (int)(target.Skills.MagicResist.Value);
			int duration = 8 - (int)(resist * (6.0 / 125.0));
			duration = Math.Max(2, Math.Min(8, duration));

			target.Paralyze(TimeSpan.FromSeconds(duration));
            DoRayEffect(caster, target, 1195, 1153, 10);
			target.FixedParticles(0x376A, 9, 32, 5030, 0x3F, 0, EffectLayer.Waist);
			target.PlaySound(0x204);

			target.SendMessage("You are petrified by the energy's ray!");
			SetCooldown(caster, "Petrification", cooldown);

			return true;
		}

		public static bool Fear(Mobile caster, int cooldown, Mobile target)
		{
			if (caster == null || caster.Deleted || target == null || target.Deleted || !target.Alive)
				return false;

			if (IsOnCooldown(caster, "Fear"))
				return false;

			if (!caster.InLOS(target))
				return false;

			if (target.Skills.Knightship.Value > 70.0)
			{
				target.SendMessage("Your knightly courage protects you from fear!");
				SetCooldown(caster, "Fear", cooldown);
				return false;
			}

			int resist = (int)(target.Skills.MagicResist.Value);
			int distance = 8 - (int)(resist * (5.0 / 125.0));
			distance = Math.Max(3, Math.Min(8, distance));

			Direction d = caster.GetDirectionTo(target);
			Point3D newLocation = target.Location;
			bool moved = false;

			for (int i = 0; i < distance; i++)
			{
				int x = newLocation.X;
				int y = newLocation.Y;

				Movement.Movement.Offset(d, ref x, ref y);
				Point3D testLoc = new Point3D(x, y, newLocation.Z);

				if (target.Map.CanSpawnMobile(testLoc))
				{
					newLocation = testLoc;
					moved = true;
				}
				else
				{
					break;
				}
			}

			if (moved && newLocation != target.Location)
			{
                DoRayEffect(caster, target, 0x36D4, 1153, 10);
				Effects.SendLocationParticles(target, 0x3728, 10, 10, 0x1F4, 0, 5029, 0);

				target.MoveToWorld(newLocation, target.Map);
				target.PlaySound(0x204);
				target.FixedParticles(0x376A, 9, 32, 5030, 0x21, 0, EffectLayer.Waist);
				target.SendMessage("You flee in terror from the unnatural gaze!");
			}
			else
			{
				target.SendMessage("The fear ray washes over you, but you cannot move!");
			}

			SetCooldown(caster, "Fear", cooldown);
			return true;
		}

        public static bool TelekineticRay(Mobile caster, int range, int cooldown)
        {
        	if (caster == null || caster.Deleted)
        		return false;

        	if (IsOnCooldown(caster, "TelekineticRay"))
        		return false;

        	List<Mobile> targets = new List<Mobile>();
			
			foreach (Mobile m in caster.GetMobilesInRange(range))
        	{
        		if (m == null || m.Deleted || !m.Alive || m == caster)
        			continue;
        		if (m.Combatant != caster && caster.Combatant != m)
        			continue;
        		if (!caster.InLOS(m))
        			continue;
				targets.Add(m);
			}

			bool affectedAny = false;
			foreach (Mobile target in targets)
			{
        		int resist = (int)target.Skills.MagicResist.Value;
        		int distance = 5 - (int)(resist * (2.0 / 125.0));
        		distance = Math.Max(3, Math.Min(5, distance));

        		Direction d = caster.GetDirectionTo(target);
        		Point3D newLoc = target.Location;
        		bool moved = false;

        		for (int i = 0; i < distance; i++)
        		{
        			int x = newLoc.X;
        			int y = newLoc.Y;

        			Movement.Movement.Offset(d, ref x, ref y);
        			Point3D testLoc = new Point3D(x, y, newLoc.Z);

        			if (target.Map.CanSpawnMobile(testLoc))
        			{
        				newLoc = testLoc;
        				moved = true;
        			}
        			else
        			{
        				break;
        			}
        		}

        		if (moved && newLoc != target.Location)
        		{
                    target.MoveToWorld(newLoc, target.Map);
        			target.PlaySound(0x204);
        			target.FixedParticles(0x3728, 10, 10, 0x1F4, 0, 5029, 0);
                    DoRayEffect(caster, target, 0x36D4, 1153, 10);
        			target.SendMessage("A telekinetic force hurls you!");
        			affectedAny = true;
        		}
        	}

        	if (affectedAny)
        	{
        		caster.PlaySound(0x1F8);
        		SetCooldown(caster, "TelekineticRay", cooldown);
        	}

        	return affectedAny;
        }

        public static bool DeathRay(Mobile caster, Mobile target, int damagePerTick, int durationSeconds, int cooldown)
        {
        	if (caster == null || caster.Deleted || target == null || target.Deleted || !target.Alive)
        		return false;

        	if (IsOnCooldown(caster, "DeathRay"))
        		return false;

        	if (!caster.InLOS(target))
        		return false;

        	target.SendMessage("A deadly ray of necrotic energy begins to consume you!");
        	target.PlaySound(0x208);
        	target.FixedParticles(0x3709, 10, 30, 5052, 0x96, 0, EffectLayer.Head);
            DoRayEffect(caster, target, 0x36D4, 1175, 10);

        	DeathRayTimer timer = new DeathRayTimer(caster, target, damagePerTick, durationSeconds);
        	timer.Start();
        
        	SetCooldown(caster, "DeathRay", cooldown);
        	return true;
        }

        private static int GetNecroticRayDamage(Mobile target, int baseDamage)
        {
        	if (target == null)
        		return baseDamage;

        	double karma = target.Karma;
        	double karmaFactor = karma <= 0 ? 0.0 : Math.Min(karma / 15000.0, 1.0);
        	double karmaReduction = karmaFactor * 0.25;

        	double resist = target.Skills[SkillName.MagicResist].Value;
        	double resistFactor = Math.Min(resist / 125.0, 1.0);
        	double resistReduction = resistFactor * 0.25;

        	double totalReduction = Math.Min(karmaReduction + resistReduction, 0.5);
        	double finalMultiplier = 1.0 - totalReduction;

        	return Math.Max(1, (int)Math.Round(baseDamage * finalMultiplier));
        }

        private class DeathRayTimer : Timer
        {
        	private Mobile m_Caster;
        	private Mobile m_Target;
        	private int m_Damage;
        	private int m_TicksRemaining;

        	public DeathRayTimer(Mobile caster, Mobile target, int damagePerTick, int durationSeconds)
        		: base(TimeSpan.Zero, TimeSpan.FromSeconds(1.0))
        	{
        		m_Caster = caster;
        		m_Target = target;
        		m_Damage = damagePerTick;
        		m_TicksRemaining = durationSeconds;

        		Priority = TimerPriority.TwoFiftyMS;
        	}

        	protected override void OnTick()
            {
            	if (m_Target == null || m_Target.Deleted || !m_Target.Alive || m_TicksRemaining <= 0)
            	{
            		Stop();
            		return;
            	}

            	int adjustedDamage = GetNecroticRayDamage(m_Target, m_Damage);
            	AOS.Damage(m_Target, m_Caster, adjustedDamage, 0, 0, 0, 0, 100);

            	m_Target.FixedParticles(0x376A, 9, 32, 5030, 0x21, 0, EffectLayer.Waist);
            	m_Target.PlaySound(0x1F8);

            	m_TicksRemaining--;
            }
        }

		public static void DoRayEffect(Mobile from, Mobile to, int effectID, int hue, int speed)
		{
			if (from == null || from.Deleted || to == null || to.Deleted || from.Map == null)
				return;

			Effects.SendMovingEffect(
				from,
				to,
				effectID,
				speed,
				0,
				false,
				false,
				hue,
				0
			);
		}

		private static bool IsOnCooldown(Mobile m, string abilityName)
		{
			if (m == null || m.Deleted)
				return false;

			if (!m_Cooldowns.ContainsKey(m))
				return false;

			if (!m_Cooldowns[m].ContainsKey(abilityName))
				return false;

			return DateTime.UtcNow < m_Cooldowns[m][abilityName];
		}

		private static void SetCooldown(Mobile m, string abilityName, int seconds)
		{
			if (m == null || m.Deleted)
				return;

			if (!m_Cooldowns.ContainsKey(m))
				m_Cooldowns[m] = new Dictionary<string, DateTime>();

			m_Cooldowns[m][abilityName] = DateTime.UtcNow + TimeSpan.FromSeconds(seconds);
		}

		public static void CleanupCooldowns()
		{
			List<Mobile> toRemove = new List<Mobile>();

			foreach (KeyValuePair<Mobile, Dictionary<string, DateTime>> kvp in m_Cooldowns)
			{
				if (kvp.Key == null || kvp.Key.Deleted)
				{
					toRemove.Add(kvp.Key);
					continue;
				}

				List<string> expiredAbilities = new List<string>();
				foreach (KeyValuePair<string, DateTime> ability in kvp.Value)
				{
					if (DateTime.UtcNow > ability.Value.AddMinutes(5))
						expiredAbilities.Add(ability.Key);
				}

				foreach (string ability in expiredAbilities)
					kvp.Value.Remove(ability);

				if (kvp.Value.Count == 0)
					toRemove.Add(kvp.Key);
			}

			foreach (Mobile m in toRemove)
				m_Cooldowns.Remove(m);
		}
	}
}