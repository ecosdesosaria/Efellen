using System;
using System.Collections.Generic;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.EffectsUtil;

namespace Server.Custom.DailyBosses.System
{
    public static class BossSpecialAttack
    {
        private const double TELEGRAPH_DELAY = 1.5;
        /// <summary>
        /// Performs a telegraphed AoE slam attack centered on the boss
        /// </summary>
        /// <param name="boss">The boss performing the attack</param>
        /// <param name="warcry">Message displayed overhead</param>
        /// <param name="hue">Color hue for visual effects</param>
        /// <param name="rage">Boss rage level (affects damage)</param>
        /// <param name="range">Attack radius (default: 6)</param>
        /// <param name="physicalDmg">Physical damage percentage (0-100)</param>
        /// <param name="fireDmg">Fire damage percentage (0-100)</param>
        /// <param name="coldDmg">Cold damage percentage (0-100)</param>
        /// <param name="poisonDmg">Poison damage percentage (0-100)</param>
        /// <param name="energyDmg">Energy damage percentage (0-100)</param>
        public static void PerformSlam(
            BaseCreature boss,
            string warcry,
            int hue,
            int rage,
            int range = 6,
            int physicalDmg = 100,
            int fireDmg = 0,
            int coldDmg = 0,
            int poisonDmg = 0,
            int energyDmg = 0
        )
        {
            if (boss == null || boss.Deleted || !boss.Alive)
                return;

            // Validate damage percentages -- must equal to 100
            int totalDamage = physicalDmg + fireDmg + coldDmg + poisonDmg + energyDmg;
            if (totalDamage != 100)
            {
                Console.WriteLine("Warning: Damage percentages for " + boss.Name + " slam total " + totalDamage + "%, expected 100%");
            }

            if (!string.IsNullOrEmpty(warcry))
            {
                boss.PublicOverheadMessage(MessageType.Regular, hue, false, warcry);
            }

            // Telegraph phase
            boss.FixedParticles(0x3709, 10, 30, 5052, hue, 0, EffectLayer.Waist);
            boss.PlaySound(0x208);

            // Store boss location and map (in case boss moves/dies during delay)
            Point3D bossLocation = boss.Location;
            Map bossMap = boss.Map;

            // Execute attack after telegraph delay
            Timer.DelayCall(TimeSpan.FromSeconds(TELEGRAPH_DELAY), delegate()
            {
                if (boss.Deleted || !boss.Alive || bossMap == null)
                    return;

                boss.PlaySound(0x64F);
                Effects.SendLocationEffect(bossLocation, bossMap, 0x36B0, 30, 10, hue, 0);

                int minDamage = 20 + (int)(rage * 1.5);
                int maxDamage = 30 + (rage * 3);

                IPooledEnumerable eable = bossMap.GetMobilesInRange(bossLocation, range);
                
                foreach (Mobile m in eable)
                {
                    if (m == null || m == boss || !m.Player || !m.Alive)
                        continue;

                    if (!boss.CanBeHarmful(m))
                        continue;

                    boss.DoHarmful(m);
                    
                    int damage = Utility.RandomMinMax(minDamage, maxDamage);
                    AOS.Damage(m, boss, damage, physicalDmg, fireDmg, coldDmg, poisonDmg, energyDmg);

                    m.FixedParticles(0x36B0, 1, 10, 5013, hue, 0, EffectLayer.Waist);
                }
                eable.Free();
                SlamVisuals.SlamVisual(boss, range, 0x36B0, hue);
            });
        }
        /// <summary>
        /// Performs a telegraphed directional charge attack
        /// </summary>
        /// <param name="boss">The boss performing the attack</param>
        /// <param name="warcry">Message displayed overhead</param>
        /// <param name="hue">Color hue for visual effects</param>
        /// <param name="rage">Boss rage level (affects damage and charge count)</param>
        /// <param name="stunDuration">Duration boss is stunned after charge (seconds)</param>
        public static void PerformRampage(
            BaseCreature boss,
            string warcry,
            int hue,
            int rage,
            double stunDuration = 4.0
        )
        {
            if (boss == null || boss.Deleted || !boss.Alive)
                return;

            if (!string.IsNullOrEmpty(warcry))
            {
                boss.PublicOverheadMessage(MessageType.Regular, hue, false, warcry);
            }

            boss.Frozen = true;

            boss.FixedParticles(0x375A, 10, 30, 5052, hue, 0, EffectLayer.CenterFeet);
            boss.FixedParticles(0x376A, 9, 32, 5030, hue, 0, EffectLayer.Waist);
            boss.PlaySound(0x15E);

            Effects.SendLocationEffect(boss.Location, boss.Map, 0x3728, 30, 10, hue, 0);

            Timer.DelayCall(TimeSpan.FromSeconds(TELEGRAPH_DELAY), delegate()
            {
                if (boss.Deleted || !boss.Alive)
                {
                    boss.Frozen = false;
                    return;
                }

                boss.Frozen = false;
                boss.PlaySound(0x51D);

                int chargeCount = Utility.RandomMinMax(3, 5) + rage;
                int minDamage = 20 + (int)(rage * 1.5);
                int maxDamage = 30 + (rage * 3);

                List<Point3D> path = BuildRampagePath(boss, chargeCount);
                List<Mobile> damagedMobiles = new List<Mobile>();

                foreach (Point3D loc in path)
                {
                    boss.MoveToWorld(loc, boss.Map);

                    Effects.SendLocationEffect(loc, boss.Map, 0x3728, 15, 10, hue, 0);
                    Effects.SendLocationEffect(loc, boss.Map, 0x36B0, 10, 10, hue, 0);

                    IPooledEnumerable eable = boss.Map.GetMobilesInRange(loc, 1);
                    foreach (Mobile m in eable)
                    {
                        if (m != boss && m.Player && m.Alive && boss.CanBeHarmful(m) && !damagedMobiles.Contains(m))
                        {
                            boss.DoHarmful(m);
                            int damage = Utility.RandomMinMax(minDamage, maxDamage);
                            AOS.Damage(m, boss, damage, 100, 0, 0, 0, 0);
                            m.FixedParticles(0x36B0, 1, 10, 5013, hue, 0, EffectLayer.Waist);
                            damagedMobiles.Add(m);
                        }
                    }
                    eable.Free();
                }

                // Post-rampage stun
                boss.Frozen = true;
                Timer.DelayCall(TimeSpan.FromSeconds(stunDuration), delegate()
                {
                    if (boss.Deleted)
                        return;

                    boss.Frozen = false;
                    boss.PublicOverheadMessage(MessageType.Regular, hue, false, "*recovers balance*");
                    boss.FixedParticles(0x375A, 10, 15, 5013, hue, 0, EffectLayer.Waist);
                });
            });
        }
        /// <summary>
        /// Builds a random rampage path for the boss
        /// </summary>
        private static List<Point3D> BuildRampagePath(BaseCreature boss, int chargeCount)
        {
            List<Point3D> path = new List<Point3D>();
            Point3D currentLoc = boss.Location;

            for (int charge = 0; charge < chargeCount; charge++)
            {
                Direction chargeDir = (Direction)Utility.Random(8);
                int chargeDist = Utility.RandomMinMax(6, 8);

                for (int step = 0; step < chargeDist; step++)
                {
                    Point3D nextLoc = GetPointInDirection(currentLoc, chargeDir, 1);

                    if (boss.Map.CanSpawnMobile(nextLoc))
                    {
                        path.Add(nextLoc);
                        currentLoc = nextLoc;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return path;
        }
        /// <summary>
        /// Gets a point in a specific direction from a location
        /// </summary>
        private static Point3D GetPointInDirection(Point3D from, Direction d, int distance)
        {
            int x = from.X;
            int y = from.Y;

            switch (d & Direction.Mask)
            {
                case Direction.North: y -= distance; break;
                case Direction.South: y += distance; break;
                case Direction.West: x -= distance; break;
                case Direction.East: x += distance; break;
                case Direction.Right: x += distance; y -= distance; break;
                case Direction.Left: x -= distance; y += distance; break;
                case Direction.Down: x += distance; y += distance; break;
                case Direction.Up: x -= distance; y -= distance; break;
            }

            return new Point3D(x, y, from.Z);
        }
        /// <summary>
        /// Summons elite guard creatures with telegraphed spawn locations
        /// </summary>
        /// <param name="boss">The boss performing the summon</param>
        /// <param name="target">Target for summoned creatures to attack</param>
        /// <param name="warcry">Message displayed overhead</param>
        /// <param name="amount">Number of creatures to summon</param>
        /// <param name="creatureType">Type of creature to summon (must inherit from BaseCreature)</param>
        /// <param name="hue">Color hue for visual effects</param>
        public static void SummonHonorGuard(
            BaseCreature boss,
            Mobile target,
            string warcry,
            int amount,
            Type creatureType,
            int hue
        )
        {
            if (boss == null || boss.Deleted || !boss.Alive)
                return;

            if (creatureType == null || !creatureType.IsSubclassOf(typeof(BaseCreature)))
            {
                Console.WriteLine("Warning: Invalid creature type passed to SummonHonorGuard");
                return;
            }

            Map map = boss.Map;
            if (map == null)
                return;

            if (!string.IsNullOrEmpty(warcry))
            {
                boss.PublicOverheadMessage(MessageType.Regular, hue, false, warcry);
            }

            boss.PlaySound(0x133);
            boss.FixedParticles(0x3728, 1, 13, 9912, hue, 7, EffectLayer.Head);

            List<Point3D> spawnLocations = new List<Point3D>();
            for (int i = 0; i < amount; i++)
            {
                Point3D loc = GetSpawnLocation(boss, map);
                spawnLocations.Add(loc);

                // Warning visual at each spawn location
                Effects.SendLocationEffect(loc, map, 0x3709, 30, 10, hue, 0);
                Effects.PlaySound(loc, map, 0x20F);
            }

            Timer.DelayCall(TimeSpan.FromSeconds(TELEGRAPH_DELAY), delegate()
            {
                if (boss.Deleted || !boss.Alive || map == null)
                    return;

                foreach (Point3D loc in spawnLocations)
                {
                    Effects.SendLocationEffect(loc, map, 0x3728, 20, 10, hue, 0);
                    Effects.PlaySound(loc, map, 0x307);

                    BaseCreature monster = null;
                    try
                    {
                        monster = (BaseCreature)Activator.CreateInstance(creatureType);
                    }
                    catch
                    {
                        Console.WriteLine("Error: Failed to create instance of " + creatureType.Name);
                        continue;
                    }

                    if (monster == null)
                        continue;

                    monster.Team = boss.Team;
                    monster.IsTempEnemy = true;
                    monster.MoveToWorld(loc, map);
                    
                    if (target != null && target.Alive)
                        monster.Combatant = target;
                }
            });
        }
        /// <summary>
        /// Gets a valid spawn location near the boss
        /// </summary>
        private static Point3D GetSpawnLocation(BaseCreature boss, Map map)
        {
            for (int j = 0; j < 20; ++j)
            {
                int x = boss.X + Utility.Random(13) - 6;
                int y = boss.Y + Utility.Random(13) - 6;
                int z = map.GetAverageZ(x, y);

                if (map.CanFit(x, y, boss.Z, 16, false, false))
                    return new Point3D(x, y, boss.Z);
                else if (map.CanFit(x, y, z, 16, false, false))
                    return new Point3D(x, y, z);
            }

            return boss.Location;
        }
        /// <summary>
        /// Performs an AoE fear effect that paralyzes nearby players
        /// </summary>
        /// <param name="boss">The boss performing the fear</param>
        /// <param name="warcry">Message displayed overhead</param>
        /// <param name="range">Fear effect radius</param>
        /// <param name="rage">Boss rage level (increases duration)</param>
        /// <param name="terror">Knightship skill level required to be immune (1-125)</param>
        public static void PerformFear(
            BaseCreature boss,
            string warcry,
            int range,
            int rage,
            int terror
        )
        {
            if (boss == null || boss.Deleted || !boss.Alive)
                return;

            if (!string.IsNullOrEmpty(warcry))
            {
                boss.PublicOverheadMessage(MessageType.Regular, 0x21, false, warcry);
            }

            boss.PlaySound(0x64D);

            IPooledEnumerable eable = boss.GetMobilesInRange(range);
            
            foreach (Mobile m in eable)
            {
                if (m == boss || !m.Alive || !m.Player || !boss.CanBeHarmful(m))
                    continue;

                if (m.Combatant != boss)
                    continue;

                if (m.Skills.Knightship.Value >= terror)
                {
                    m.SendMessage("Your bravery protects you from fear!");
                    m.FixedParticles(0x375A, 10, 15, 5037, EffectLayer.Waist);
                    continue;
                }

                boss.DoHarmful(m);

                double resist = m.Skills[SkillName.MagicResist].Value;
                int duration = (8 - (int)(resist * (6.0 / 125.0))) + rage;
                
                if (duration < 2)
                    duration = 2;

                m.Paralyze(TimeSpan.FromSeconds(duration));
                m.SendMessage("You are frozen in terror!");
                m.FixedParticles(0x376A, 9, 32, 5030, EffectLayer.Head);
            }
            eable.Free();
        }
    }
}