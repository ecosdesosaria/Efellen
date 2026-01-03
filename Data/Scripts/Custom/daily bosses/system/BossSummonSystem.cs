using System;
using System.Collections.Generic;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Items;

namespace Server.Custom.BossSystems
{
	public class BossSummonSystem
	{
		private const int SUMMON_RANGE = 8;
		private const int SPAWN_ATTEMPTS = 20;
		private const int SPAWN_RADIUS = 6;
		
		public static bool TrySummonCreature(
			BaseCreature boss,
			Mobile target,
			Type[] creatureList,
			int rage,
			ref DateTime nextSummonTime,
			string[] warcries,
			List<BaseCreature> summonsList,
			int hue,
			int maxSummons,
			int summonCooldown)
		{
			if (boss == null || boss.Deleted || target == null || target.Deleted)
				return false;

			double[] chances = { 0.10, 0.20, 0.33, 0.50 };

			if (rage >= 0 && rage < chances.Length && chances[rage] >= Utility.RandomDouble())
			{
				return SpawnCreature(
					boss,
					target,
					creatureList,
					rage,
					ref nextSummonTime,
					warcries,
					summonsList,
					hue,
					maxSummons,
					summonCooldown);
			}

			return false;
		}

		private static bool SpawnCreature(
			BaseCreature boss,
			Mobile target,
			Type[] creatureList,
			int rage,
			ref DateTime nextSummonTime,
			string[] warcries,
			List<BaseCreature> summonsList,
			int hue,
			int maxSummons,
			int summonCooldown)
		{
			Map map = boss.Map;
			if (map == null || target == null || target.Deleted)
				return false;

			if (DateTime.UtcNow < nextSummonTime)
				return false;

			CleanupDeadSummons(summonsList);

			int currentSummons = summonsList.Count;

			if (currentSummons >= maxSummons)
				return false;

			boss.PlaySound(0x216);

			int newSummons = GetSummonCount(rage);
			string warcry = GetWarcry(rage, warcries);

			if (!string.IsNullOrEmpty(warcry))
				boss.PublicOverheadMessage(MessageType.Regular, 0x21, false, warcry);

			for (int i = 0; i < newSummons; ++i)
			{
				if (summonsList.Count >= maxSummons)
					break;

				BaseCreature monster = CreateMonster(creatureList, rage);
				if (monster == null)
					continue;

				monster.Team = boss.Team;
				Point3D loc = GetSpawnLocation(boss, map);

				monster.IsTempEnemy = true;
				monster.MoveToWorld(loc, map);
				monster.Combatant = target;

				Effects.SendLocationParticles(
					EffectItem.Create(loc, map, EffectItem.DefaultDuration),
					0x3728, 10, 10, hue, 0, 5042, 0
				);
				Effects.PlaySound(loc, map, 0x215);

				RegisterSummon(monster, summonsList);
			}

			nextSummonTime = DateTime.UtcNow + TimeSpan.FromSeconds(summonCooldown - (rage * 3));
			
			return true;
		}

		private static void CleanupDeadSummons(List<BaseCreature> summonsList)
		{
			if (summonsList == null)
				return;

			summonsList.RemoveAll(s => s == null || s.Deleted || !s.Alive);
		}

		private static void RegisterSummon(BaseCreature bc, List<BaseCreature> summonsList)
		{
			if (bc == null)
				return;

			summonsList.Add(bc);

			Timer.DelayCall(TimeSpan.FromMinutes(2), delegate()
			{
				if (bc != null && !bc.Deleted && bc.Alive)
				{
					bc.Delete();
				}
				summonsList.Remove(bc);
			});
		}

		private static BaseCreature CreateMonster(Type[] creatureList, int rage)
		{
			if (creatureList == null || creatureList.Length == 0)
				return null;

			int listLength = creatureList.Length;
			int rand = Utility.Random(100);

			if (listLength == 3)
			{
				switch (rage)
				{
					case 0:
						return CreateCreature(creatureList[0]);
					case 1:
						if (rand < 40)
							return CreateCreature(creatureList[1]);
						else
							return CreateCreature(creatureList[0]);
					case 2:
						if (rand < 40)
							return CreateCreature(creatureList[2]);
						else
							return CreateCreature(creatureList[1]);
					case 3:
						if (rand < 60)
							return CreateCreature(creatureList[2]);
						else
							return CreateCreature(creatureList[1]);
					default:
						return CreateCreature(creatureList[0]);
				}
			}
			else if (listLength == 5)
			{
				switch (rage)
				{
					case 0:
						return CreateCreature(creatureList[0]);
					case 1:
						if (rand < 40)
							return CreateCreature(creatureList[2]);
						else if (rand < 70)
							return CreateCreature(creatureList[1]);
						else
							return CreateCreature(creatureList[0]);
					case 2:
						if (rand < 40)
							return CreateCreature(creatureList[3]);
						else if (rand < 70)
							return CreateCreature(creatureList[2]);
						else
							return CreateCreature(creatureList[1]);
					case 3:
						if (rand < 40)
							return CreateCreature(creatureList[4]);
						else if (rand < 70)
							return CreateCreature(creatureList[3]);
						else
							return CreateCreature(creatureList[2]);
					default:
						return CreateCreature(creatureList[0]);
				}
			}
			else
			{
				return CreateCreature(creatureList[0]);
			}
		}

		private static BaseCreature CreateCreature(Type creatureType)
		{
			if (creatureType == null)
				return null;

			try
			{
				return Activator.CreateInstance(creatureType) as BaseCreature;
			}
			catch
			{
				return null;
			}
		}

		private static Point3D GetSpawnLocation(BaseCreature boss, Map map)
		{
			for (int j = 0; j < SPAWN_ATTEMPTS; ++j)
			{
				int x = boss.X + Utility.Random(SPAWN_RADIUS * 2 + 1) - SPAWN_RADIUS;
				int y = boss.Y + Utility.Random(SPAWN_RADIUS * 2 + 1) - SPAWN_RADIUS;
				int z = map.GetAverageZ(x, y);

				if (map.CanFit(x, y, boss.Z, 16, false, false))
					return new Point3D(x, y, boss.Z);
				else if (map.CanFit(x, y, z, 16, false, false))
					return new Point3D(x, y, z);
			}

			return boss.Location;
		}

		private static int GetSummonCount(int rage)
		{
			switch (rage)
			{
				case 0:
					return Utility.RandomMinMax(4, 8);
				case 1:
					return Utility.RandomMinMax(3, 7);
				case 2:
					return Utility.RandomMinMax(3, 5);
				case 3:
					return Utility.RandomMinMax(2, 3);
				default:
					return 2;
			}
		}

		private static string GetWarcry(int rage, string[] warcries)
		{
			if (warcries == null || warcries.Length == 0)
				return "";

			if (rage >= 0 && rage < warcries.Length)
				return warcries[rage];

			return "";
		}

		public static void CleanupSummons(List<BaseCreature> summonsList)
		{
			if (summonsList == null)
				return;

			for (int i = summonsList.Count - 1; i >= 0; i--)
			{
				BaseCreature bc = summonsList[i];

				if (bc != null && !bc.Deleted)
					bc.Delete();
			}
			summonsList.Clear();
		}
	}
}