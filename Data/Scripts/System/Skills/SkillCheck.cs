using System;
using Server;
using Server.Misc;
using Server.Mobiles;

namespace Server.Misc
{
	/// <summary>
	/// Handles skill checks, skill gain calculations, and stat gain mechanics for players and creatures.
	/// </summary>
	public class SkillCheck
	{
		#region Configuration Constants

		private const bool ANTI_MACRO_ENABLED = false;
		private const int LOCATION_GRID_SIZE = 5;
		
		// Public constants used by other systems
		public static TimeSpan AntiMacroExpire = TimeSpan.FromMinutes(5.0);
		public const int Allowance = 3;

		#endregion

		#region Anti-Macro Skill Configuration

		/// <summary>
		/// Defines which skills use anti-macro protection.
		/// </summary>
		private static readonly bool[] SkillUsesAntiMacro = new bool[]
		{
			false, // Alchemy = 0
			true,  // Anatomy = 1
			true,  // Druidism = 2
			true,  // ItemID = 3
			true,  // ArmsLore = 4
			false, // Parry = 5
			true,  // Begging = 6
			false, // Blacksmith = 7
			false, // Bowcrafting = 8
			true,  // Peacemaking = 9
			true,  // Camping = 10
			false, // Carpentry = 11
			false, // Cartography = 12
			false, // Cooking = 13
			true,  // Searching = 14
			true,  // Discordance = 15
			true,  // EvalInt = 16
			true,  // Healing = 17
			false, // Seafaring = 18
			true,  // Forensics = 19
			true,  // Herding = 20
			true,  // Hiding = 21
			true,  // Provocation = 22
			false, // Inscribe = 23
			true,  // Lockpicking = 24
			true,  // Magery = 25
			true,  // MagicResist = 26
			false, // Tactics = 27
			true,  // Snooping = 28
			true,  // Musicianship = 29
			true,  // Poisoning = 30
			false, // Marksmanship = 31
			true,  // Spiritualism = 32
			true,  // Stealing = 33
			false, // Tailoring = 34
			true,  // Taming = 35
			true,  // Tasting = 36
			false, // Tinkering = 37
			true,  // Tracking = 38
			true,  // Veterinary = 39
			false, // Swords = 40
			false, // Bludgeoning = 41
			false, // Fencing = 42
			false, // FistFighting = 43
			true,  // Lumberjacking = 44
			true,  // Mining = 45
			true,  // Meditation = 46
			true,  // Stealth = 47
			true,  // RemoveTrap = 48
			true,  // Necromancy = 49
			false, // Focus = 50
			true,  // Knightship = 51
			true,  // Bushido = 52
			true,  // Ninjitsu = 53
			true   // Elementalism = 54
		};

		#endregion

		#region Initialization

		public static void Initialize()
		{
			Mobile.SkillCheckLocationHandler = new SkillCheckLocationHandler(Mobile_SkillCheckLocation);
			Mobile.SkillCheckDirectLocationHandler = new SkillCheckDirectLocationHandler(Mobile_SkillCheckDirectLocation);
			Mobile.SkillCheckTargetHandler = new SkillCheckTargetHandler(Mobile_SkillCheckTarget);
			Mobile.SkillCheckDirectTargetHandler = new SkillCheckDirectTargetHandler(Mobile_SkillCheckDirectTarget);
		}

		#endregion

		#region Location-Based Skill Checks

		public static bool Mobile_SkillCheckLocation(Mobile from, SkillName skillName, double minSkill, double maxSkill)
		{
			Skill skill = from.Skills[skillName];
			if (skill == null)
				return false;

			double value = skill.Value;

			if (value < minSkill)
				return false; // Too difficult
			
			if (value >= maxSkill)
				return true; // No challenge

			double chance = CalculateSuccessChance(value, minSkill, maxSkill);
			Point2D gridLocation = GetGridLocation(from.Location);
			
			return CheckSkill(from, skill, gridLocation, chance);
		}

		public static bool Mobile_SkillCheckDirectLocation(Mobile from, SkillName skillName, double chance)
		{
			Skill skill = from.Skills[skillName];
			if (skill == null)
				return false;

			if (chance < 0.0)
				return false; // Too difficult
			
			if (chance >= 1.0)
				return true; // No challenge

			Point2D gridLocation = GetGridLocation(from.Location);
			return CheckSkill(from, skill, gridLocation, chance);
		}

		#endregion

		#region Target-Based Skill Checks

		public static bool Mobile_SkillCheckTarget(Mobile from, SkillName skillName, object target, double minSkill, double maxSkill)
		{
			Skill skill = from.Skills[skillName];
			if (skill == null)
				return false;

			double value = skill.Value;

			if (value < minSkill)
				return false; // Too difficult
			
			if (value >= maxSkill)
				return true; // No challenge

			double chance = CalculateSuccessChance(value, minSkill, maxSkill);
			return CheckSkill(from, skill, target, chance);
		}

		public static bool Mobile_SkillCheckDirectTarget(Mobile from, SkillName skillName, object target, double chance)
		{
			Skill skill = from.Skills[skillName];
			if (skill == null)
				return false;

			if (chance < 0.0)
				return false; // Too difficult
			
			if (chance >= 1.0)
				return true; // No challenge

			return CheckSkill(from, skill, target, chance);
		}

		#endregion

		#region Core Skill Check Logic

		public static bool CheckSkill(Mobile from, Skill skill, object checkObject, double successChance)
		{
			if (from.Skills.Cap == 0)
				return false;

			bool success = (successChance >= Utility.RandomDouble());
			double gainChance = CalculateGainChance(from, skill, successChance, success);

			DisplayGainChanceIfEnabled(from, skill, gainChance);

			if (ShouldAttemptGain(from, skill, checkObject, gainChance))
			{
				AttemptSkillGain(from, skill);
			}

			return success;
		}

		private static bool ShouldAttemptGain(Mobile from, Skill skill, object checkObject, double gainChance)
		{
			if (!from.Alive)
				return false;

			bool lowSkillAutoGain = skill.Base < 10.0;
			bool normalGain = gainChance >= Utility.RandomDouble() && AllowGainByAntiMacro(from, skill, checkObject);

			return lowSkillAutoGain || normalGain;
		}

		private static void AttemptSkillGain(Mobile from, Skill skill)
		{
			// Seafaring skill requires being on a boat after 50 skill
			if (IsSeafaringRestricted(from, skill))
			{
				from.SendMessage("You would get better at seafaring if you fished from a boat.");
				return;
			}

			GainSkill(from, skill);
		}

		private static bool IsSeafaringRestricted(Mobile from, Skill skill)
		{
			return !Worlds.IsOnBoat(from) 
				&& skill.SkillName == SkillName.Seafaring 
				&& from.Skills[SkillName.Seafaring].Base >= 50;
		}

		#endregion

		#region Gain Chance Calculation

		private static double CalculateGainChance(Mobile from, Skill skill, double successChance, bool wasSuccessful)
		{
			double gainerModifier = GetGuildGainerModifier(from, skill.SkillName);
			gainerModifier -= MyServerSettings.SkillGain();

			double gainChance = CalculateBaseGainChance(from, skill, successChance, wasSuccessful, gainerModifier);
			gainChance = ApplyHungerThirstModifier(from, gainChance);
			gainChance *= skill.Info.GainFactor;

			// Minimum gain chance
			if (gainChance < 0.01)
				gainChance = 0.01;

			// Controlled creatures gain twice as fast
			if (IsControlledCreature(from))
				gainChance *= 2;

			return gainChance;
		}

		private static double CalculateBaseGainChance(Mobile from, Skill skill, double successChance, bool wasSuccessful, double gainerModifier)
		{
			double capRatio = (from.Skills.Cap - from.Skills.Total) / from.Skills.Cap;
			double skillCapRatio = (skill.Cap - skill.Base) / skill.Cap;
			double gainChance = (capRatio + skillCapRatio) / gainerModifier;

			// Adjust for success/failure
			double successAdjustment = wasSuccessful ? 0.5 : (Core.AOS ? 0.0 : 0.2);
			gainChance += (1.0 - successChance) * successAdjustment;
			gainChance /= gainerModifier;

			return gainChance;
		}

		private static double GetGuildGainerModifier(Mobile from, SkillName skillName)
		{
			if (!(from is PlayerMobile) || !IsGuildSkill(from, skillName))
				return 2.0;

			// Guild members get a random bonus between 1.0 and 1.5
			int random = Utility.RandomMinMax(0, 5);
			switch (random)
			{
				case 0: return 1.5;
				case 1: return 1.4;
				case 2: return 1.3;
				case 3: return 1.2;
				case 4: return 1.1;
				default: return 1.0;
			}
		}

		private static double ApplyHungerThirstModifier(Mobile from, double gainChance)
		{
			if (!(from is PlayerMobile))
				return gainChance;

			int hungerThirstTotal = from.Hunger + from.Thirst;

			if (hungerThirstTotal <= 8)
				return gainChance / 1.50;
			else if (hungerThirstTotal <= 16)
				return gainChance / 1.25;
			else if (hungerThirstTotal <= 24)
				return gainChance * 1.10;
			else if (hungerThirstTotal <= 32)
				return gainChance * 1.25;
			else if (hungerThirstTotal <= 40)
				return gainChance * 1.50;
			
			return gainChance;
		}

		private static void DisplayGainChanceIfEnabled(Mobile from, Skill skill, double gainChance)
		{
			if (!from.Player || !SkillGainSettings.ShowChance(from))
				return;

			if (skill.Lock != SkillLock.Up || skill.Value >= skill.Cap)
				return;

			double displayChance = gainChance * 100.0;
			if (displayChance > 100.0)
				displayChance = 100.0;

			from.SendMessage("Skill gain chance for {0}: {1:0.00}%", skill.SkillName, displayChance);
		}

		#endregion

		#region Guild Skill Checks

		public static bool IsGuildSkill(Mobile from, SkillName skillName)
		{
			if (!(from is PlayerMobile))
				return false;

			PlayerMobile pm = (PlayerMobile)from;

			if (pm.NpcGuild == NpcGuild.MagesGuild)
				return IsSkillInList(skillName, SkillName.Psychology, SkillName.Magery, SkillName.Meditation);
			else if (pm.NpcGuild == NpcGuild.ElementalGuild)
				return IsSkillInList(skillName, SkillName.Focus, SkillName.Elementalism, SkillName.Meditation);
			else if (pm.NpcGuild == NpcGuild.WarriorsGuild)
				return skillName == SkillName.Tactics;
			else if (pm.NpcGuild == NpcGuild.ThievesGuild)
				return IsSkillInList(skillName, SkillName.Lockpicking, SkillName.Snooping, SkillName.Stealing);
			else if (pm.NpcGuild == NpcGuild.RangersGuild)
				return IsSkillInList(skillName, SkillName.Camping, SkillName.Tracking);
			else if (pm.NpcGuild == NpcGuild.HealersGuild)
				return IsSkillInList(skillName, SkillName.Anatomy, SkillName.Healing, SkillName.Spiritualism);
			else if (pm.NpcGuild == NpcGuild.MinersGuild)
				return skillName == SkillName.Mining;
			else if (pm.NpcGuild == NpcGuild.MerchantsGuild)
				return IsSkillInList(skillName, SkillName.Mercantile, SkillName.ArmsLore, SkillName.Tasting);
			else if (pm.NpcGuild == NpcGuild.TinkersGuild)
				return skillName == SkillName.Tinkering;
			else if (pm.NpcGuild == NpcGuild.TailorsGuild)
				return skillName == SkillName.Tailoring;
			else if (pm.NpcGuild == NpcGuild.FishermensGuild)
				return skillName == SkillName.Seafaring;
			else if (pm.NpcGuild == NpcGuild.BardsGuild)
				return skillName == SkillName.Musicianship;
			else if (pm.NpcGuild == NpcGuild.BlacksmithsGuild)
				return IsSkillInList(skillName, SkillName.Blacksmith, SkillName.ArmsLore);
			else if (pm.NpcGuild == NpcGuild.NecromancersGuild)
				return IsSkillInList(skillName, SkillName.Forensics, SkillName.Necromancy, SkillName.Spiritualism);
			else if (pm.NpcGuild == NpcGuild.AlchemistsGuild)
				return IsSkillInList(skillName, SkillName.Alchemy, SkillName.Cooking, SkillName.Tasting);
			else if (pm.NpcGuild == NpcGuild.DruidsGuild)
				return IsSkillInList(skillName, SkillName.Druidism, SkillName.Taming, SkillName.Veterinary);
			else if (pm.NpcGuild == NpcGuild.ArchersGuild)
				return IsSkillInList(skillName, SkillName.Marksmanship, SkillName.Bowcraft);
			else if (pm.NpcGuild == NpcGuild.CarpentersGuild)
				return IsSkillInList(skillName, SkillName.Carpentry, SkillName.Lumberjacking);
			else if (pm.NpcGuild == NpcGuild.CartographersGuild)
				return skillName == SkillName.Cartography;
			else if (pm.NpcGuild == NpcGuild.LibrariansGuild)
				return IsSkillInList(skillName, SkillName.Mercantile, SkillName.Inscribe);
			else if (pm.NpcGuild == NpcGuild.CulinariansGuild)
				return IsSkillInList(skillName, SkillName.Cooking, SkillName.Tasting);
			else if (pm.NpcGuild == NpcGuild.AssassinsGuild)
				return IsSkillInList(skillName, SkillName.Hiding, SkillName.Poisoning, SkillName.Stealth);

			return false;
		}

		private static bool IsSkillInList(SkillName skill, params SkillName[] skillList)
		{
			for (int i = 0; i < skillList.Length; i++)
			{
				if (skill == skillList[i])
					return true;
			}
			return false;
		}

		#endregion

		#region Anti-Macro Check

		private static bool AllowGainByAntiMacro(Mobile from, Skill skill, object obj)
		{
			if (!ANTI_MACRO_ENABLED)
				return true;

			if (!(from is PlayerMobile))
				return true;

			if (!SkillUsesAntiMacro[skill.Info.SkillID])
				return true;

			PlayerMobile playerMobile = (PlayerMobile)from;
			return playerMobile.AntiMacroCheck(skill, obj);
		}

		#endregion

		#region Skill Gain

		public static void GainSkill(Mobile from, Skill skill)
		{
			if (ShouldPreventGain(from, skill))
				return;

			if (skill.Base >= skill.Cap || skill.Lock != SkillLock.Up)
				return;

			int gainAmount = CalculateGainAmount(skill);
			bool gainApplied = TryApplySkillGain(from, skill, gainAmount);

			if (gainApplied)
			{
				RefreshSkillUI(from, skill);
				AttemptStatGain(from, skill);
			}
		}

		private static bool ShouldPreventGain(Mobile from, Skill skill)
		{
			if (from.Region.IsPartOf(typeof(Regions.Jail)))
				return true;

			if (from is BaseCreature)
			{
				BaseCreature creature = (BaseCreature)from;
				if (creature.IsDeadPet)
					return true;
			}

			// Focus skill doesn't gain on creatures
			if (skill.SkillName == SkillName.Focus && from is BaseCreature)
				return true;

			return false;
		}

		private static int CalculateGainAmount(Skill skill)
		{
			// Low skills gain faster (1-4 points)
			if (skill.Base <= 10.0)
				return Utility.Random(4) + 1;

			return 1;
		}

		private static bool TryApplySkillGain(Mobile from, Skill skill, int gainAmount)
		{
			Skills skills = from.Skills;

			// For players, try to lower a skill if at cap
			if (from.Player && (skills.Total / skills.Cap) >= Utility.RandomDouble())
			{
				TryLowerLockedDownSkill(skills, skill, gainAmount);
			}

			// Apply Scroll of Alacrity bonus for players
			if (from is PlayerMobile)
			{
				gainAmount = ApplyAlacritySrollBonus((PlayerMobile)from, skill, gainAmount);
			}

			// Apply gain if under cap or not a player
			if (!from.Player || (skills.Total + gainAmount) <= skills.Cap)
			{
				skill.BaseFixedPoint += gainAmount;
				return true;
			}

			return false;
		}

		private static void TryLowerLockedDownSkill(Skills skills, Skill currentSkill, int amount)
		{
			for (int i = 0; i < skills.Length; i++)
			{
				Skill skillToLower = skills[i];

				if (skillToLower != currentSkill 
					&& skillToLower.Lock == SkillLock.Down 
					&& skillToLower.BaseFixedPoint >= amount)
				{
					skillToLower.BaseFixedPoint -= amount;
					break;
				}
			}
		}

		private static int ApplyAlacritySrollBonus(PlayerMobile pm, Skill skill, int baseGain)
		{
			bool hasActiveAlacrity = skill.SkillName == pm.AcceleratedSkill 
				&& pm.AcceleratedStart > DateTime.Now;

			if (hasActiveAlacrity)
				return baseGain * Utility.RandomMinMax(2, 5);

			return baseGain;
		}

		private static void RefreshSkillUI(Mobile from, Skill skill)
		{
			// Focus and Meditation only refresh occasionally to reduce overhead
			bool isPassiveSkill = skill.SkillName == SkillName.Focus 
				|| skill.SkillName == SkillName.Meditation;

			if (isPassiveSkill)
			{
				if (Utility.RandomMinMax(1, 10) == 1)
					Server.Gumps.SkillListingGump.RefreshSkillList(from);
			}
			else
			{
				Server.Gumps.SkillListingGump.RefreshSkillList(from);
			}
		}

		#endregion

		#region Stat Gain

		public enum Stat { Str, Dex, Int }

		private static readonly TimeSpan StatGainDelay = MyServerSettings.StatGainDelay();
		private static readonly TimeSpan PetStatGainDelay = MyServerSettings.PetStatGainDelay();

		private static void AttemptStatGain(Mobile from, Skill skill)
		{
			if (skill.Lock != SkillLock.Up)
				return;

			SkillInfo info = skill.Info;
			double statGainDivisor = MyServerSettings.StatGain();

			if (from.StrLock == StatLockType.Up && (info.StrGain / statGainDivisor) > Utility.RandomDouble())
				GainStat(from, Stat.Str);
			else if (from.DexLock == StatLockType.Up && (info.DexGain / statGainDivisor) > Utility.RandomDouble())
				GainStat(from, Stat.Dex);
			else if (from.IntLock == StatLockType.Up && (info.IntGain / statGainDivisor) > Utility.RandomDouble())
				GainStat(from, Stat.Int);
		}

		public static void GainStat(Mobile from, Stat stat)
		{
			if (!CanAttemptStatGain(from, stat))
				return;

			UpdateLastStatGainTime(from, stat);

			bool shouldAtrophy = (from.RawStatTotal / (double)from.StatCap) >= Utility.RandomDouble();
			IncreaseStat(from, stat, shouldAtrophy);
		}

		private static bool CanAttemptStatGain(Mobile from, Stat stat)
		{
			TimeSpan delay = IsControlledCreature(from) ? PetStatGainDelay : StatGainDelay;
			DateTime lastGain = GetLastStatGainTime(from, stat);

			return (lastGain + delay) < DateTime.Now;
		}

		private static DateTime GetLastStatGainTime(Mobile from, Stat stat)
		{
			switch (stat)
			{
				case Stat.Str: return from.LastStrGain;
				case Stat.Dex: return from.LastDexGain;
				case Stat.Int: return from.LastIntGain;
				default: return DateTime.MinValue;
			}
		}

		private static void UpdateLastStatGainTime(Mobile from, Stat stat)
		{
			switch (stat)
			{
				case Stat.Str:
					from.LastStrGain = DateTime.Now;
					break;
				case Stat.Dex:
					from.LastDexGain = DateTime.Now;
					break;
				case Stat.Int:
					from.LastIntGain = DateTime.Now;
					break;
			}
		}

		public static void ResetStatGain(Mobile from, int extra)
		{
			if (extra >= 2)
				return;

			DateTime resetTime = DateTime.Now.AddMinutes(-60.01);
			from.LastStrGain = resetTime;
			from.LastDexGain = resetTime;
			from.LastIntGain = resetTime;
		}

		#endregion

		#region Stat Increase/Decrease Logic

		public static void IncreaseStat(Mobile from, Stat stat, bool atrophy)
		{
			atrophy = atrophy || (from.RawStatTotal >= from.StatCap);

			switch (stat)
			{
				case Stat.Str:
					HandleStrengthIncrease(from, atrophy);
					break;
				case Stat.Dex:
					HandleDexterityIncrease(from, atrophy);
					break;
				case Stat.Int:
					HandleIntelligenceIncrease(from, atrophy);
					break;
			}
		}

		private static void HandleStrengthIncrease(Mobile from, bool atrophy)
		{
			if (atrophy)
			{
				if (CanLower(from, Stat.Dex) && (from.RawDex < from.RawInt || !CanLower(from, Stat.Int)))
					--from.RawDex;
				else if (CanLower(from, Stat.Int))
					--from.RawInt;
			}

			if (CanRaise(from, Stat.Str))
				++from.RawStr;
		}

		private static void HandleDexterityIncrease(Mobile from, bool atrophy)
		{
			if (atrophy)
			{
				if (CanLower(from, Stat.Str) && (from.RawStr < from.RawInt || !CanLower(from, Stat.Int)))
					--from.RawStr;
				else if (CanLower(from, Stat.Int))
					--from.RawInt;
			}

			if (CanRaise(from, Stat.Dex))
				++from.RawDex;
		}

		private static void HandleIntelligenceIncrease(Mobile from, bool atrophy)
		{
			if (atrophy)
			{
				if (CanLower(from, Stat.Str) && (from.RawStr < from.RawDex || !CanLower(from, Stat.Dex)))
					--from.RawStr;
				else if (CanLower(from, Stat.Dex))
					--from.RawDex;
			}

			if (CanRaise(from, Stat.Int))
				++from.RawInt;
		}

		public static bool CanLower(Mobile from, Stat stat)
		{
			switch (stat)
			{
				case Stat.Str: 
					return (from.StrLock == StatLockType.Down && from.RawStr > 10);
				case Stat.Dex: 
					return (from.DexLock == StatLockType.Down && from.RawDex > 10);
				case Stat.Int: 
					return (from.IntLock == StatLockType.Down && from.RawInt > 10);
			}

			return false;
		}

		public static bool CanRaise(Mobile from, Stat stat)
		{
			if (!(from is BaseCreature && ((BaseCreature)from).Controlled))
			{
				if (from.RawStatTotal >= from.StatCap)
					return false;
			}

			int maxStatValue = from.StatCap > 250 ? 175 : 150;

			switch (stat)
			{
				case Stat.Str: 
					return (from.StrLock == StatLockType.Up && from.RawStr < maxStatValue);
				case Stat.Dex: 
					return (from.DexLock == StatLockType.Up && from.RawDex < maxStatValue);
				case Stat.Int: 
					return (from.IntLock == StatLockType.Up && from.RawInt < maxStatValue);
			}

			return false;
		}

		#endregion

		#region Helper Methods

		private static double CalculateSuccessChance(double skillValue, double minSkill, double maxSkill)
		{
			return (skillValue - minSkill) / (maxSkill - minSkill);
		}

		private static Point2D GetGridLocation(Point3D location)
		{
			return new Point2D(location.X / LOCATION_GRID_SIZE, location.Y / LOCATION_GRID_SIZE);
		}

		private static bool IsControlledCreature(Mobile from)
		{
			if (!(from is BaseCreature))
				return false;

			BaseCreature creature = (BaseCreature)from;
			return creature.Controlled;
		}

		#endregion
	}
}