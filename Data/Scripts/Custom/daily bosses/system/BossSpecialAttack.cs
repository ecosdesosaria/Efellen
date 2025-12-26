using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.EffectsUtil;
using Server.Spells.Necromancy;
using Server.Spells;
using Server.Items;

namespace Server.Custom.DailyBosses.System
{
    public static class BossSpecialAttack
    {
        private static ArrayList m_BossBleeds = new ArrayList();
        private const double TELEGRAPH_DELAY = 2;
        #region slam
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
        #endregion
        #region rampage
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
        #endregion
        #region summon honor guard
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
        #endregion
        #region fear
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

                m.Paralyze(TimeSpan.FromSeconds(GetParalyzeDuration(m,rage)));
                m.SendMessage("You are frozen in terror!");
                m.FixedParticles(0x376A, 9, 32, 5030, EffectLayer.Head);
            }
            eable.Free();
        }
        #endregion
        #region parahax + bleed
        /// <summary>
        /// Performs an AoE entangling attack that paralyzes and causes bleed damage over time
        /// </summary>
        /// <param name="boss">The boss performing the attack</param>
        /// <param name="warcry">Message displayed overhead when attack executes</param>
        /// <param name="hue">Color hue for visual effects</param>
        /// <param name="rage">Boss rage level (affects paralyze duration)</param>
        /// <param name="range">Attack radius</param>
        /// <param name="bleedLevel">Bleed damage multiplier (damage per tick = level * 2-3)</param>
        public static void PerformEntangle(
            BaseCreature boss,
            string warcry,
            int hue,
            int rage,
            int range,
            int bleedLevel
        )
        {
            if (boss == null || boss.Deleted || !boss.Alive)
                return;

            // Telegraph
            boss.PublicOverheadMessage(MessageType.Regular, hue, false, "*gathering energy*");
            boss.PlaySound(0x1F5);
            boss.FixedParticles(0x375A, 10, 15, 5037, hue, 0, EffectLayer.Waist);

            // Store location and map for the delayed attack
            Point3D bossLocation = boss.Location;
            Map bossMap = boss.Map;

            Timer.DelayCall(TimeSpan.FromSeconds(2.0), delegate()
            {
                if (boss.Deleted || !boss.Alive || bossMap == null)
                    return;

                if (!string.IsNullOrEmpty(warcry))
                {
                    boss.PublicOverheadMessage(MessageType.Regular, hue, false, warcry);
                }

                boss.PlaySound(0x133);
                boss.FixedParticles(0x3728, 1, 13, 9912, hue, 7, EffectLayer.Head);

                Effects.SendLocationEffect(bossLocation, bossMap, 0x36B0, 30, 10, hue, 0);
                SlamVisuals.SlamVisual(boss, range, 0x36B0, hue);

                IPooledEnumerable eable = bossMap.GetMobilesInRange(bossLocation, range);

                foreach (Mobile m in eable)
                {
                    if (m == boss || !m.Player || !m.Alive || !boss.CanBeHarmful(m))
                        continue;

                    boss.DoHarmful(m);

                    int paralyzeDuration = GetParalyzeDuration(m, rage);
                    m.Paralyze(TimeSpan.FromSeconds(paralyzeDuration));
                    m.SendMessage("You are paralyzed by the attack!");

                    if (!Server.Items.BaseRace.IsBleeder(m))
                        continue;

                    if (IsBleedImmune(m))
                        continue;

                    m.PlaySound(0x133);
                    m.FixedParticles(0x377A, 244, 25, 9950, 31, 0, EffectLayer.Waist);

                    if (m is PlayerMobile)
                    {
                        m.LocalOverheadMessage(MessageType.Regular, 0x982, false, "You are bleeding profusely!");
                    }

                    BeginBossBleed(m, boss, bleedLevel);
                }

                eable.Free();
            });
        }
        #endregion
        #region cross explosion
        /// <summary>
        /// Performs a targeted cross-pattern explosion that damages everything it touches
        /// </summary>
        /// <param name="boss">The boss performing the attack</param>
        /// <param name="target">Primary target for the cross pattern</param>
        /// <param name="warcry">Message displayed overhead</param>
        /// <param name="hue">Color hue for visual effects</param>
        /// <param name="rage">Boss rage level (affects damage and range)</param>
        /// <param name="physicalDmg">Physical damage percentage (0-100)</param>
        /// <param name="fireDmg">Fire damage percentage (0-100)</param>
        /// <param name="coldDmg">Cold damage percentage (0-100)</param>
        /// <param name="poisonDmg">Poison damage percentage (0-100)</param>
        /// <param name="energyDmg">Energy damage percentage (0-100)</param>
        public static void PerformCrossExplosion(
            BaseCreature boss,
            Mobile target,
            string warcry,
            int hue,
            int rage,
            int physicalDmg = 100,
            int fireDmg = 0,
            int coldDmg = 0,
            int poisonDmg = 0,
            int energyDmg = 0
        )
        {
            if (boss == null || boss.Deleted || !boss.Alive)
                return;

            if (target == null || target.Deleted || !target.Alive)
                return;

            int totalDamage = physicalDmg + fireDmg + coldDmg + poisonDmg + energyDmg;
            if (totalDamage != 100)
            {
                Console.WriteLine("Warning: Damage percentages for " + boss.Name + " cross explosion total " + totalDamage + "%, expected 100%");
            }

            if (!string.IsNullOrEmpty(warcry))
            {
                boss.PublicOverheadMessage(MessageType.Regular, hue, false, warcry);
            }

            target.FixedParticles(0x3709, 10, 30, 5052, hue, 0, EffectLayer.Waist);
            Effects.PlaySound(target.Location, target.Map, 0x208);

            // Store target location for delayed explosion
            Point3D targetLocation = target.Location;
            Map targetMap = target.Map;

            // Double the normal telegraph delay (3 seconds)
            Timer.DelayCall(TimeSpan.FromSeconds(TELEGRAPH_DELAY * 2), delegate()
            {
                if (boss.Deleted || !boss.Alive || targetMap == null)
                    return;

                int minDamage = 30 + (rage * 2);
                int maxDamage = 36 + (rage * 3);

                Point3D[] points = CrossPoints(target.Location, rage*2);

                List<Mobile> damagedMobiles = new List<Mobile>();

                foreach (Point3D point in points)
                {
                    Effects.SendLocationEffect(point, targetMap, Utility.RandomList(0x33E5, 0x33F5), 85, 10, hue, 0);

                    IPooledEnumerable eable = targetMap.GetMobilesInRange(point, 0);
                    foreach (Mobile m in eable)
                    {
                        if (damagedMobiles.Contains(m) || m == boss || !m.Alive || !m.Player)
                            continue;

                        if (!boss.CanBeHarmful(m))
                            continue;

                        boss.DoHarmful(m);
                        
                        int damage = Utility.RandomMinMax(minDamage, maxDamage);
                        AOS.Damage(m, boss, damage, physicalDmg, fireDmg, coldDmg, poisonDmg, energyDmg);

                        m.FixedParticles(0x36B0, 1, 10, 5013, hue, 0, EffectLayer.Waist);
                        damagedMobiles.Add(m);
                    }
                    eable.Free();
                }
                Effects.PlaySound(targetLocation, targetMap, 0x15F);
            });
        }
        #endregion
        #region targetted AoE explosion
        /// <summary>
        /// Performs a targeted AoE explosion that marks tiles
        /// </summary>
        /// <param name="boss">The boss performing the attack</param>
        /// <param name="target">Primary target for the attack</param>
        /// <param name="warcry">Message displayed overhead</param>
        /// <param name="hue">Color hue for visual effects</param>
        /// <param name="rage">Boss rage level (affects damage and radius)</param>
        /// <param name="physicalDmg">Physical damage percentage (0-100)</param>
        /// <param name="fireDmg">Fire damage percentage (0-100)</param>
        /// <param name="coldDmg">Cold damage percentage (0-100)</param>
        /// <param name="poisonDmg">Poison damage percentage (0-100)</param>
        /// <param name="energyDmg">Energy damage percentage (0-100)</param>
        public static void PerformTargettedAoE(
            BaseCreature boss,
        	Mobile target,
        	int rage,
        	string warcry,
        	int hue,
        	int physicalDmg,
        	int fireDmg,
        	int coldDmg,
        	int poisonDmg,
        	int energyDmg
        )
        {
        	if ( target == null || target.Deleted || target.Map == null )
        		return;

        	int radius = 4 + rage;

        	int minDamage = (int)(20 + (rage * 1.5));
        	int maxDamage = 30 + (rage * 3);

        	// Telegraph / warning
        	if (!string.IsNullOrEmpty(warcry))
            {
                boss.PublicOverheadMessage(MessageType.Regular, hue, false, warcry);
            }

        	// Charge / targeting effect
        	boss.MovingParticles(
        		target,
        		0x1C19,
        		1,
        		0,
        		false,
        		true,
        		hue,
        		0,
        		9502,
        		6014,
        		0x11D,
        		EffectLayer.Waist,
        		0
        	);

        	Timer.DelayCall( TimeSpan.FromSeconds( TELEGRAPH_DELAY ), delegate()
        	{
        		if ( target == null || target.Deleted || target.Map == null )
        			return;

        		Map map = target.Map;

        		Effects.SendLocationParticles(
        			EffectItem.Create( target.Location, map, EffectItem.DefaultDuration ),
        			0x36BD,
        			20,
        			10,
        			hue,
        			0,
        			5044,
        			0
        		);

        		Effects.PlaySound( target.Location, map, 0x307 );

        		IPooledEnumerable eable = target.GetMobilesInRange( radius );
        		foreach ( Mobile m in eable )
        		{
        			if ( m == null || m.Deleted || !m.Alive || m == boss )
        				continue;

        			if ( m is BaseCreature )
        			{
        				BaseCreature bc = (BaseCreature)m;
        				if ( bc.Team == boss.Team )
        					continue;
        			}

        			int damage = Utility.RandomMinMax( minDamage, maxDamage );

        			AOS.Damage(
        				m,
        				boss,
        				damage,
        				physicalDmg,
        				fireDmg,
        				coldDmg,
        				poisonDmg,
        				energyDmg
        			);

        			SetOnFire( m, hue );
        		}
        		eable.Free();
        		LightTilesOnFire( target.Location, map, radius, hue );
        	});
        }
        #endregion

        #region helpers
        /// <summary>
        /// Cross shaped AoE attack
        /// </summary>
        /// <param name="center">origin point</param>
        /// <param name="radius">how far the cross goes</param>
        public static Point3D[] CrossPoints(Point3D center, int radius)
        {
            List<Point3D> points = new List<Point3D>();
            points.Add(center);

            for (int i = 1; i <= radius; i++)
            {
                points.Add(new Point3D(center.X + i, center.Y, center.Z)); // East
                points.Add(new Point3D(center.X - i, center.Y, center.Z)); // West
                points.Add(new Point3D(center.X, center.Y + i, center.Z)); // South
                points.Add(new Point3D(center.X, center.Y - i, center.Z)); // North
            }
            return points.ToArray();
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
        private static int GetParalyzeDuration(Mobile m, int rage)
		{
			int resist = (int)(m.Skills.MagicResist.Value);
			// base 2s at 125, caps 8s at 0 magic resist, adds rage afterwards
			int duration = (8 - (int)(resist * (6.0 / 125.0))) + rage;
            if (duration < 2 )
                duration = 2;
			return duration;
		}

        private class BossBleedEntry
        {
            public Mobile Mobile;
            public Timer Timer;

            public BossBleedEntry(Mobile m, Timer t)
            {
                Mobile = m;
                Timer = t;
            }
        }

        private static BossBleedEntry FindBleedEntry(Mobile m)
        {
            for (int i = 0; i < m_BossBleeds.Count; i++)
            {
                BossBleedEntry entry = (BossBleedEntry)m_BossBleeds[i];

                if (entry.Mobile == m)
                    return entry;
            }        
            return null;
        }
        private static bool IsBleedImmune(Mobile m)
        {
            TransformContext context = TransformationSpellHelper.GetContext(m);
            if (context != null && (context.Type == typeof(LichFormSpell) || context.Type == typeof(WraithFormSpell)))
                return true;

            if (m is BaseCreature && ((BaseCreature)m).BleedImmune)
                return true;

            return false;
        }

        public static void BeginBossBleed(Mobile m, Mobile from, int totalTicks)
        {
            BossBleedEntry entry = FindBleedEntry(m);

            if (entry != null)
            {
                if (entry.Timer != null)
                    entry.Timer.Stop();

                m_BossBleeds.Remove(entry);
            }

            Timer t = new BossBleedTimer(from, m, totalTicks);
            m_BossBleeds.Add(new BossBleedEntry(m, t));
            t.Start();
        }


        public static void DoBossBleed(Mobile m, Mobile from, int level)
        {
            if (!m.Alive || !Server.Items.BaseRace.IsBleeder(m) || m == null || m.Deleted )
            {
                EndBossBleed(m, false);
                return;
            }

            int damage = Utility.RandomMinMax(level * 2, level * 3);
            
            if (!m.Player)
                damage *= 2;

            m.PlaySound(0x133);
            AOS.Damage(m, from, damage, 100, 0, 0, 0, 0);

            Blood blood = new Blood();
            blood.ItemID = Utility.Random(0x122A, 5);
            blood.MoveToWorld(m.Location, m.Map);
            m.FixedParticles(0x377A, 1, 15, 9502, 67, 7, EffectLayer.Waist);
        }

        public static void EndBossBleed(Mobile m, bool message)
        {
            BossBleedEntry entry = FindBleedEntry(m);

            if (entry == null)
                return;

            if (entry.Timer != null)
                entry.Timer.Stop();

            m_BossBleeds.Remove(entry);

            if (message && m is PlayerMobile)
                m.SendMessage("The bleeding has stopped.");
        }


        private class BossBleedTimer : Timer
        {
            private Mobile m_From;
            private Mobile m_Mobile;
            private int m_Count;
            private int m_MaxTicks;

            public BossBleedTimer(Mobile from, Mobile m, int maxTicks) 
                : base(TimeSpan.FromSeconds(2.0), TimeSpan.FromSeconds(2.0))
            {
                m_From = from;
                m_Mobile = m;
                m_MaxTicks = maxTicks;
                Priority = TimerPriority.TwoFiftyMS;
            }

            protected override void OnTick()
            {
                DoBossBleed(m_Mobile, m_From, m_MaxTicks - m_Count);
                
                if (++m_Count == m_MaxTicks)
                    EndBossBleed(m_Mobile, true);
            }
        }

        private static void LightTilesOnFire( Point3D center, Map map, int radius, int hue )
        {
        	if ( map == null )
        		return;

        	for ( int x = -radius; x <= radius; x++ )
        	{
        		for ( int y = -radius; y <= radius; y++ )
        		{
        			Point3D loc = new Point3D( center.X + x, center.Y + y, center.Z );

        			int dx = center.X - loc.X;
        			int dy = center.Y - loc.Y;
        			double distance = Math.Sqrt( (dx * dx) + (dy * dy) );

        			if ( distance > radius )
        				continue;

        			Effects.SendLocationParticles(
        				EffectItem.Create( loc, map, TimeSpan.FromSeconds( 2.0 ) ),
        				0x3709,
        				10,
        				30,
        				hue,
        				0,
        				5052,
        				0
        			);
        		}
        	}
        }

        private static void SetOnFire( Mobile m, int hue )
        {
        	if ( m == null || m.Deleted )
        		return;

        	m.FixedParticles(
        		0x3709,
        		10,
        		30,
        		5052,
        		hue,
        		0,
        		EffectLayer.Waist
        	);
        }
        #endregion
    }
}