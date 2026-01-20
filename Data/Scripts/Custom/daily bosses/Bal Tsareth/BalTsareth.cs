using System;
using Server;
using System.Collections;
using System.Collections.Generic;
using Server.Items;
using Server.ContextMenus;
using Server.Misc;
using Server.Network;
using Server.Mobiles;
using Server.Commands;
using Server.Commands.Generic;
using Server.Spells.Necromancy;
using Server.Spells;
using Server.EffectsUtil;
using Server.Custom;
using Server.Custom.DailyBosses.System;
using Server.Custom.BossSystems;

namespace Server.Mobiles
{
	[CorpseName( "Kamina's Corpse" )]
	public class BalTsareth : BaseCreature
	{		
		private static readonly Type[] SummonTypes = new Type[] 
		{ 
			typeof(Imp), 
			typeof(AnyElemental), 
			typeof(AnyGemElemental) 
		};

		private static readonly string[] SummonWarcries = new string[]
		{
			"The weave answers to me!",
			"My magic has outlasted civilizations, taste it!",
			"This is MY HOME! MINE!",
			"I've forgotten magic that your civilization is yet to learn!"
		};

		private static readonly List<Type> BossDrops = new List<Type>
    	{
    	    typeof(Artifact_PrismaticRobeOfBalTsareth),
    	    typeof(Artifact_PrismaticGlassesOfBalTsareth),
    	    typeof(Artifact_PrismaticCapeOfBalTsareth),
    	    typeof(Artifact_PrismaticBootsOfBalTsareth),
            typeof(Artifact_PrismaticRingOfBalTsareth)
    	};

		private int m_Rage;
		private Mobile m_LastTarget;
		private DateTime m_NextSummonTime;
		private DateTime m_NextSpecialAttack;
		private List<BaseCreature> m_Summons;
        private DateTime m_NextSpellTime;
		private bool m_IsChanneling;
		private Timer m_ChannelTimer;
		private Mobile m_ChannelTarget;
		private int m_ChannelSpell;
		private DateTime m_StoneSkinEnds;
		private bool m_HasStoneSkin;
		private int m_OriginalPhysicalResist;
		private Dictionary<Point3D, Timer> m_AcidFogTimers;

		[Constructable]
		public BalTsareth () : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "Bal Tsareth";
            Body = 401; 
			Utility.AssignRandomHair( this );
			HairHue = Utility.RandomHairHue();
            Hue = Utility.RandomSkinHue(); 
			NameHue = 0x22;
			Title = "The Ancient Lorekeeper";
			
			SetStr( 596, 785 );
			SetDex( 165, 225 );
			SetInt( 556, 655 );
			SetHits( 10000 );
			SetDamage( 19, 23 );
			SetDamageType( ResistanceType.Physical, 20 );
			SetDamageType( ResistanceType.Poison, 20 );
			SetDamageType( ResistanceType.Cold, 20 );
			SetDamageType( ResistanceType.Fire, 20 );
			SetDamageType( ResistanceType.Energy, 20 );
			SetResistance( ResistanceType.Physical, 45 );
			SetResistance( ResistanceType.Fire, 70 );
			SetResistance( ResistanceType.Cold, 70 );
			SetResistance( ResistanceType.Poison, 70 );
			SetResistance( ResistanceType.Energy, 70 );
			SetSkill( SkillName.Meditation, 102.5, 125.0 );
			SetSkill( SkillName.MagicResist, 125.5, 145.0 );
			SetSkill( SkillName.Tactics, 101.0, 110.0 );
			SetSkill( SkillName.FistFighting, 91.0 );
			SetSkill( SkillName.Bludgeoning, 101.0, 111.0 );
			SetSkill( SkillName.Magery, 101.0, 110.0 );
			SetSkill( SkillName.Psychology, 101.0, 110.0 );

			Fame = 30000;
			Karma = -30000;
			VirtualArmor = 50;

			PackItem( new EerieIdol(Utility.Random(12,26)) );
            AddItem( new ScholarRobe{ Hue = 0x0213 } );
            AddItem( new Sandals{ Hue = 0x0213 } );
            AddItem( new BlackStaff{ Hue = 0x0213 } );

            m_NextSummonTime = DateTime.MinValue;
			m_NextSpecialAttack = DateTime.MinValue;
			m_Summons = new List<BaseCreature>();
            m_NextSpellTime = DateTime.UtcNow;
			m_IsChanneling = false;
			m_HasStoneSkin = false;
			m_AcidFogTimers = new Dictionary<Point3D, Timer>();
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.UltraRich, 6 );
		}

        public override bool AlwaysAttackable{ get{ return true; } }
		public override bool AlwaysMurderer { get { return true; } }
		public override int TreasureMapLevel{ get{ return 4; } }
		public override bool CanRummageCorpses{ get{ return false; } }
		public override bool ReacquireOnMovement{ get{ return !Controlled; } }
		public override bool BleedImmune{ get{ return true; } }
		public override bool BardImmune { get { return true; } }
		public override bool Unprovokable { get { return true; } }
		public override Poison PoisonImmune{ get{ return Poison.Greater; } }

		public override void OnDamage( int amount, Mobile from, bool willKill )
		{
			m_LastTarget = from;

            if ( m_IsChanneling )
				InterruptChannel();
			
			if ( m_Rage >= 1 && DateTime.UtcNow >= m_NextSpecialAttack )
			{
				PerformRageAttack( from );
				m_NextSpecialAttack = DateTime.UtcNow + TimeSpan.FromSeconds( 20 - (m_Rage * 2) );
			}

            if ( Utility.RandomDouble() < 0.25 )
                TryWeaveStep();

			base.OnDamage( amount, from, willKill );
		}

        private void InterruptChannel()
		{
			if ( !m_IsChanneling )
				return;

			m_IsChanneling = false;
			this.Frozen = false;
            Say( "*The spell fizzles*" );

			if ( m_ChannelTimer != null )
			{
				m_ChannelTimer.Stop();
				m_ChannelTimer = null;
			}

			m_ChannelTarget = null;
            m_NextSpellTime = DateTime.UtcNow + TimeSpan.FromSeconds( 15 );
		}

        public override void OnThink()
		{
			base.OnThink();

			if ( m_HasStoneSkin && DateTime.UtcNow >= m_StoneSkinEnds )
				RemoveStoneSkin();

			if ( !m_IsChanneling && Combatant != null && Hits < HitsMax && 
			     DateTime.UtcNow >= m_NextSpellTime && Utility.RandomDouble() < 0.20 )
			{
				m_ChannelSpell = Utility.Random( 10 );
				m_ChannelTarget = Combatant;
				StartChanneling();
			}
		}

        private void StartChanneling()
		{
			m_IsChanneling = true;
			this.Frozen = true;
            Say( "*starts channeling a powerful spell*" );
			m_ChannelTimer = Timer.DelayCall( TimeSpan.FromSeconds( 2 ), new TimerCallback( ExecuteSpell ) );
		}

        private void ExecuteSpell()
		{
			if ( !m_IsChanneling )
				return;

			m_IsChanneling = false;
			this.Frozen = false;

			switch ( m_ChannelSpell )
			{
				case 0: CastStoneSkin(); break;
				case 1: CastFireball(); break;
				case 2: CastMagicMissile(); break;
				case 3: CastWeb(); break;
				case 4: CastDisintegrate(); break;
				case 5: CastPowerwordFear(); break;
				case 6: CastPowerwordSlow(); break;
				case 7: CastAcidFog(); break;
				case 8: CastFingerOfDeath(); break;
				case 9: CastSummonCreature(); break;
			}

			m_NextSpellTime = DateTime.UtcNow + TimeSpan.FromSeconds( 30 );
		}

		private void CastStoneSkin()
		{
            Say( "*Casts Stone Skin*" );
			Effects.SendLocationParticles( EffectItem.Create( this.Location, this.Map, EffectItem.DefaultDuration ), 0x376A, 9, 32, 0x0213 );
			Effects.PlaySound( this.Location, this.Map, 0x1F2 );
			m_OriginalPhysicalResist = this.GetResistance( ResistanceType.Physical );
			this.SetResistance( ResistanceType.Physical, 80 );
			m_HasStoneSkin = true;
			m_StoneSkinEnds = DateTime.UtcNow + TimeSpan.FromSeconds( 60 );
		}

		private void RemoveStoneSkin()
		{
			if ( !m_HasStoneSkin )
				return;

			m_HasStoneSkin = false;
			this.SetResistance( ResistanceType.Physical, m_OriginalPhysicalResist );
		}

		private void CastFireball()
		{
			if ( m_ChannelTarget == null || m_ChannelTarget.Deleted || !m_ChannelTarget.Alive )
				return;

            Say( "*Casts Fireball*" );

			Point3D targetLoc = m_ChannelTarget.Location;
			Map map = m_ChannelTarget.Map;

			Effects.SendLocationEffect( targetLoc, map, 0x36BD, 20, 10, 0x0213, 0 );
			Effects.PlaySound( targetLoc, map, 0x307 );

			for ( int x = -1; x <= 1; x++ )
			{
				for ( int y = -1; y <= 1; y++ )
				{
					Point3D loc = new Point3D( targetLoc.X + x, targetLoc.Y + y, targetLoc.Z );
					Effects.SendLocationEffect( loc, map, 0x36BD, 20, 10, 0x0213, 0 );
				}
			}

			List<Mobile> targets = new List<Mobile>();
			IPooledEnumerable eable = m_ChannelTarget.GetMobilesInRange( 1 );

			foreach ( Mobile m in eable )
			{
				if ( m != this && m.Alive && CanBeHarmful( m ) )
					targets.Add( m );
			}
			eable.Free();

			foreach ( Mobile m in targets )
			{
				int damage = Utility.RandomMinMax( 55, 95 );
				AOS.Damage( m, this, damage, 0, 100, 0, 0, 0 );
			}
		}

		private void CastMagicMissile()
		{
			if ( m_ChannelTarget == null || m_ChannelTarget.Deleted || !m_ChannelTarget.Alive )
				return;

            Say( "Casts Magic Missle Storm" );

			int projectileCount = Utility.RandomMinMax( 4, 8 );
			
			for ( int i = 0; i < projectileCount; i++ )
			{
				Timer.DelayCall( TimeSpan.FromSeconds( i * 0.3 ), delegate()
				{
					if ( m_ChannelTarget != null && m_ChannelTarget.Alive && !m_ChannelTarget.Deleted )
					{
						Effects.SendMovingEffect( this, m_ChannelTarget, 0x379F, 7, 0, false, false, 0x0213, 0 );
						Effects.PlaySound( this.Location, this.Map, 0x1F5 );

						Timer.DelayCall( TimeSpan.FromSeconds( 0.5 ), delegate()
						{
							if ( m_ChannelTarget != null && m_ChannelTarget.Alive && !m_ChannelTarget.Deleted )
							{
								int damage = Utility.RandomMinMax( 5, 13 );
								AOS.Damage( m_ChannelTarget, this, damage, 0, 0, 0, 0, 100 );
								Effects.SendLocationEffect( m_ChannelTarget.Location, m_ChannelTarget.Map, 0x3709, 10, 30, 0x0213, 0 );
							}
						});
					}
				});
			}
		}

		private void CastWeb()
		{
			if ( m_ChannelTarget == null || m_ChannelTarget.Deleted || !m_ChannelTarget.Alive )
				return;

            Say( "*Casts Web*" );

			List<Mobile> targets = new List<Mobile>();
			IPooledEnumerable eable = m_ChannelTarget.GetMobilesInRange( 2 );

			foreach ( Mobile m in eable )
			{
				if ( m != this && m.Alive && CanBeHarmful( m ) )
					targets.Add( m );
			}
			eable.Free();

			foreach ( Mobile m in targets )
			{
				double magicResist = m.Skills[SkillName.MagicResist].Value;
				double duration = 18.0 - (magicResist / 12.0 + m.Dex / 15.0);
				
				if ( duration < 5.0 )
					duration = 5.0;

				m.Paralyze( TimeSpan.FromSeconds( duration ) );
				Effects.SendLocationParticles( EffectItem.Create( m.Location, m.Map, EffectItem.DefaultDuration ), 0x376A, 9, 10, 0x0213 );
				Effects.SendLocationEffect( m.Location, m.Map, 0x3709, 30, 10, 0x0213, 0 );
				Effects.PlaySound( m.Location, m.Map, 0x204 );
				m.SendMessage( "You are trapped in a magical web!" );
			}
		}

		private void CastDisintegrate()
		{
			if ( m_ChannelTarget == null || m_ChannelTarget.Deleted || !m_ChannelTarget.Alive )
				return;

            Say( "*Casts Disintegrate*" );
			Effects.SendMovingEffect( this, m_ChannelTarget, 0x379F, 7, 0, false, false, 0x0213, 0 );
			Effects.PlaySound( this.Location, this.Map, 0x1F1 );

			Timer.DelayCall( TimeSpan.FromSeconds( 0.5 ), delegate()
			{
				if ( m_ChannelTarget != null && m_ChannelTarget.Alive && !m_ChannelTarget.Deleted )
				{
					int damage = ( m_ChannelTarget.Hits <= m_ChannelTarget.HitsMax / 2 ) 
						? Utility.RandomMinMax( 90, 135 ) 
						: Utility.RandomMinMax( 45, 60 );

					AOS.Damage( m_ChannelTarget, this, damage, 100, 0, 0, 0, 0 );
					Effects.SendLocationEffect( m_ChannelTarget.Location, m_ChannelTarget.Map, 0x3709, 30, 10, 0x0213, 0 );
					Effects.PlaySound( m_ChannelTarget.Location, m_ChannelTarget.Map, 0x307 );
				}
			});
		}

		private void CastPowerwordFear()
		{
            Say( "*Casts Powerword: Fear*" );

			List<Mobile> targets = new List<Mobile>();
			IPooledEnumerable eable = this.GetMobilesInRange( 5 );

			foreach ( Mobile m in eable )
			{
				if ( m != this && m.Alive && CanBeHarmful( m ) )
					targets.Add( m );
			}
			eable.Free();

			foreach ( Mobile m in targets )
			{
				if ( m.Skills[SkillName.Knightship].Value >= 115.0 )
				{
					m.SendMessage( "Your valor steels you against fear!" );
					continue;
				}

				Point3D newLoc = FindValidTeleportLocation( m, Utility.RandomMinMax( 4, 6 ) );

				if ( newLoc != Point3D.Zero )
				{
					Effects.SendLocationEffect( m.Location, m.Map, 0x3709, 30, 10, 0x0213, 0 );
					Effects.PlaySound( m.Location, m.Map, 0x482 );
					m.MoveToWorld( newLoc, m.Map );
					Effects.SendLocationEffect( newLoc, m.Map, 0x3709, 30, 10, 0x0213, 0 );

					double duration = 17.0 - ( m.Skills[SkillName.MagicResist].Value / 15.0 );
					
					if ( duration > 0 )
						m.Paralyze( TimeSpan.FromSeconds( duration ) );

					m.SendMessage( "You are gripped by overwhelming fear!" );
				}
			}

			Effects.PlaySound( this.Location, this.Map, 0x5C3 );
		}

		private Point3D FindValidTeleportLocation( Mobile target, int distance )
		{
			for ( int attempt = 0; attempt < 20; attempt++ )
			{
				int xOffset = Utility.RandomMinMax( -distance, distance );
				int yOffset = Utility.RandomMinMax( -distance, distance );
				Point3D newLoc = new Point3D( target.X + xOffset, target.Y + yOffset, target.Z );

				if ( target.Map.CanSpawnMobile( newLoc ) )
					return newLoc;
			}

			return Point3D.Zero;
		}

		private void CastPowerwordSlow()
		{
            Say( "*Casts Powerword: Slow*" );

			List<Mobile> targets = new List<Mobile>();
			IPooledEnumerable eable = this.GetMobilesInRange( 5 );

			foreach ( Mobile m in eable )
			{
				if ( m != this && m.Alive && CanBeHarmful( m ) )
					targets.Add( m );
			}
			eable.Free();

			foreach ( Mobile m in targets )
			{
				m.Stam = 1;
				Effects.SendLocationEffect( m.Location, m.Map, 0x3709, 30, 10, 0x0213, 0 );
				Effects.PlaySound( m.Location, m.Map, 0x204 );
				m.SendMessage( "You feel incredibly sluggish!" );
			}

			Effects.PlaySound( this.Location, this.Map, 0x5C3 );
		}

		private void CastAcidFog()
		{
			if ( m_ChannelTarget == null || m_ChannelTarget.Deleted || !m_ChannelTarget.Alive )
				return;

            Say( "*Casts Acid Fog*" );

			Point3D centerLoc = m_ChannelTarget.Location;
			Map map = m_ChannelTarget.Map;
			List<Point3D> fogLocations = new List<Point3D>();

			for ( int x = -3; x <= 3; x++ )
			{
				for ( int y = -3; y <= 3; y++ )
				{
					if ( Math.Sqrt( x * x + y * y ) <= 3.0 )
					{
						Point3D loc = new Point3D( centerLoc.X + x, centerLoc.Y + y, centerLoc.Z );
						fogLocations.Add( loc );
						Effects.SendLocationEffect( loc, map, 0x3709, 30, 10, 0x0213, 0 );
					}
				}
			}

			Effects.PlaySound( centerLoc, map, 0x208 );

			foreach ( Point3D loc in fogLocations )
			{
				if ( m_AcidFogTimers.ContainsKey( loc ) )
				{
					m_AcidFogTimers[loc].Stop();
					m_AcidFogTimers[loc] = new AcidFogTimer( this, loc, map );
				}
				else
				{
					m_AcidFogTimers.Add( loc, new AcidFogTimer( this, loc, map ) );
				}

				m_AcidFogTimers[loc].Start();
			}
		}

		private void CastFingerOfDeath()
		{
			if ( m_ChannelTarget == null || m_ChannelTarget.Deleted || !m_ChannelTarget.Alive )
				return;

			Say( "*Casts Finger of Death*" );
			Effects.SendMovingEffect( this, m_ChannelTarget, 0x36E4, 7, 0, false, false, 0x0213, 0 );
			Effects.PlaySound( this.Location, this.Map, 0x1FB );

			Timer.DelayCall( TimeSpan.FromSeconds( 0.5 ), delegate()
			{
				if ( m_ChannelTarget == null || m_ChannelTarget.Deleted || !m_ChannelTarget.Alive )
					return;

				Effects.SendLocationEffect( m_ChannelTarget.Location, m_ChannelTarget.Map, 0x3709, 30, 10, 0x0213, 0 );
				Effects.PlaySound( m_ChannelTarget.Location, m_ChannelTarget.Map, 0x1FE );

				double magicResist = m_ChannelTarget.Skills[SkillName.MagicResist].Value;
				int maxHits = m_ChannelTarget.HitsMax;
				int currentHits = m_ChannelTarget.Hits;
				int newHits;

				if ( magicResist >= 110.0 )
				{
					newHits = maxHits / 2;
					if ( currentHits < newHits )
						newHits = Math.Min( currentHits, maxHits / 10 );
				}
				else if ( magicResist >= 90.0 )
				{
					newHits = maxHits / 3;
					if ( currentHits < newHits )
						newHits = Math.Min( currentHits, maxHits / 15 );
				}
				else if ( magicResist >= 75.0 )
				{
					newHits = maxHits / 4;
					if ( currentHits < newHits )
						newHits = Math.Min( currentHits, maxHits / 25 );
				}
				else
				{
					newHits = maxHits / 5;
					if ( currentHits < newHits )
						newHits = 1;
				}

				m_ChannelTarget.Hits = newHits;
				m_ChannelTarget.SendMessage( "You feel your life force being drained away!" );
			});
		}

		private void CastSummonCreature()
		{
			Type summonType;

			switch ( Utility.Random( 3 ) )
			{
				case 0:
					summonType = typeof( IceGiant );
					break;
				case 1:
					summonType = typeof( StormGiant );
					break;
				default:
					summonType = typeof( LavaGiant );
					break;
			}

			BossSpecialAttack.SummonHonorGuard( this, m_ChannelTarget, "*Casts Summon Creature*", 1, summonType, 0x0213 );
		}

		private class AcidFogTimer : Timer
		{
			private BalTsareth m_Owner;
			private Point3D m_Location;
			private Map m_Map;
			private int m_Ticks;
			private const int MaxTicks = 9;

			public AcidFogTimer( BalTsareth owner, Point3D location, Map map ) 
				: base( TimeSpan.FromSeconds( 2.0 ), TimeSpan.FromSeconds( 2.0 ) )
			{
				m_Owner = owner;
				m_Location = location;
				m_Map = map;
				m_Ticks = 0;
			}

			protected override void OnTick()
			{
				m_Ticks++;

				if ( m_Ticks > MaxTicks || m_Owner == null || m_Owner.Deleted || !m_Owner.Alive )
				{
					Stop();
					if ( m_Owner != null && m_Owner.m_AcidFogTimers.ContainsKey( m_Location ) )
						m_Owner.m_AcidFogTimers.Remove( m_Location );
					return;
				}

				Effects.SendLocationEffect( m_Location, m_Map, 0x3709, 30, 10, 0x0213, 0 );

				IPooledEnumerable eable = m_Map.GetMobilesInRange( m_Location, 0 );
				foreach ( Mobile m in eable )
				{
					if ( m != m_Owner && m.Alive && m_Owner.CanBeHarmful( m ) )
					{
						int damage = Utility.RandomMinMax( 20, 40 );
						AOS.Damage( m, m_Owner, damage, 0, 0, 0, 100, 0 );
					}
				}
				eable.Free();
			}
		}

        private void TryWeaveStep()
        {
            Map map = Map;

            if ( map == null )
                return;

            for ( int i = 0; i < 10; i++ )
            {
                int x = X + Utility.RandomMinMax( -6, 6 );
                int y = Y + Utility.RandomMinMax( -6, 6 );
                int z = map.GetAverageZ( x, y );
                Point3D p = new Point3D( x, y, z );

                if ( map.CanSpawnMobile( p ) )
                {
                    Location = p;
                    PublicOverheadMessage( MessageType.Emote, 0x3B2, false, "*Steps into the weave*" );
                    Effects.SendLocationEffect( p, map, 0x3728, 13, 10, 0, 0 );
                    Effects.PlaySound( p, map, 0x1FE );
                    break;
                }
            }
        }

		private void PerformRageAttack( Mobile target )
		{
			if ( target == null || target.Deleted || !target.Alive )
				return;

			int attackChoice = Utility.RandomMinMax( 1, m_Rage );

			switch ( attackChoice )
			{
				case 1:
					BossSpecialAttack.PerformSlam( this, "MINE! ALL OF THESE SECRETS ARE MINE!", 0x0213, m_Rage, 6, 20, 20, 20, 20, 20 );
					break;
				case 2:
					BossSpecialAttack.PerformDegenAura( this, "You dare to attack me? ME? IN MY HOME?", 8, m_Rage, 16, 29, "mana", 0x0213 );
					break;
				case 3:
					Type summonType;
					switch ( Utility.Random( 3 ) )
					{
						case 0:
							summonType = typeof( IceGiant );
							break;
						case 1:
							summonType = typeof( StormGiant );
							break;
						default:
							summonType = typeof( LavaGiant );
							break;
					}

					BossSpecialAttack.SummonHonorGuard( this, target, "The elements are mine! I unravelled their secrets before your kingdom was born!", m_Rage, summonType, 0x0213 );
					break;
			}
		}

		public override void CheckReflect( Mobile caster, ref bool reflect )
		{
			reflect = ( Utility.Random( 100 ) < m_Rage * 16 );
		}

		private int GetMaxSummons()
		{
			switch ( m_Rage )
			{
				case 1: return 4;
				case 2: return 3;
				case 3: return 2;
				default: return 6;
			}
		}

		public override void OnGotMeleeAttack( Mobile attacker )
		{
			BossSummonSystem.TrySummonCreature(
				this,
				attacker,
				SummonTypes,
				m_Rage,
				ref m_NextSummonTime,
				SummonWarcries,
				m_Summons,
				0x0213,
				GetMaxSummons(),
				60
			);
		}

		public override bool OnBeforeDeath()
		{
			if ( m_Rage == 0 )
			{
				PublicOverheadMessage( MessageType.Regular, 0x21, false, "You will forever guard my tomb!" );
				this.Hits = this.HitsMax;
				this.FixedParticles( 0x376A, 9, 32, 5030, EffectLayer.Waist );
				this.PlaySound( 0x202 );
				SetStr( Str + 30 );
				SetDamage( 25, 30 );
				VirtualArmor += 5;
				m_Rage = 1;
				return false;
			}
			else if ( m_Rage == 1 )
			{
				PublicOverheadMessage( MessageType.Regular, 0x21, false, "Surrender your mind to me!" );
				this.Hits = this.HitsMax;
				this.FixedParticles( 0x376A, 9, 32, 5030, EffectLayer.Waist );
				this.PlaySound( 0x202 );
				SetStr( Str + 60 );
				SetDex( Dex + 20 );
				SetDamage( 30, 35 );
				VirtualArmor += 10;
				m_Rage = 2;
				return false;
			}
			else if ( m_Rage == 2 )
			{
				PublicOverheadMessage( MessageType.Regular, 0x21, false, "I WILL DESTROY YOU!" );
				this.Hits = this.HitsMax;
				this.FixedParticles( 0x376A, 9, 32, 5030, EffectLayer.Waist );
				this.PlaySound( 0x202 );
				SetStr( Str + 90 );
				SetDex( Dex + 40 );
				SetDamage( 35, 40 );
				VirtualArmor += 15;
				m_Rage = 3;
				return false;
			}
			else 
			{
				Effects.SendLocationParticles( EffectItem.Create( this.Location, this.Map, EffectItem.DefaultDuration ), 0x3728, 10, 10, 2023 );
				this.PlaySound( 0x1FE );
				PublicOverheadMessage( MessageType.Regular, 0x21, false, "I shall rise again...in another thousand years..." );
				Mobile killer = this.LastKiller;
				if ( killer != null && killer.Player && killer.Karma > 0 )
            	{
            	    int marks = Utility.RandomMinMax( 21, 47 );
            	    Server.Custom.DefenderOfTheRealm.MarkLootHelper.AwardMarks( killer, 1, marks );
            	}
			}
			
			return base.OnBeforeDeath();
		}

		public override void OnDelete()
        {
            if ( m_ChannelTimer != null )
			{
				m_ChannelTimer.Stop();
				m_ChannelTimer = null;
			}

			if ( m_HasStoneSkin )
				RemoveStoneSkin();

			foreach ( Timer timer in m_AcidFogTimers.Values )
			{
				if ( timer != null )
					timer.Stop();
			}
			m_AcidFogTimers.Clear();

            BossSummonSystem.CleanupSummons( m_Summons );
            base.OnDelete();
        }

		public override void OnDeath( Container c )
		{
			base.OnDeath( c );

			BossLootSystem.AwardBossSpecial( this, BossDrops, 15 );
            c.DropItem( Loot.RandomArty() );
            c.DropItem( Loot.RandomArty() );
            c.DropItem( Loot.RandomArty() );
            c.DropItem( Loot.RandomArty() );

			if ( Utility.RandomDouble() < 0.15 )
				c.DropItem( new EternalPowerScroll() );

			int amt = Utility.RandomMinMax( 3, 9 );
			for ( int i = 0; i < amt; i++ )
				c.DropItem( new EtherealPowerScroll() );

			RichesSystem.SpawnRiches( m_LastTarget, 4 );
		}

		public override void OnAfterSpawn()
		{
			base.OnAfterSpawn();
			LeechImmune = true;
		}

		public BalTsareth( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 1 );
			writer.Write( m_Rage );
			writer.Write( m_NextSummonTime );
			writer.Write( m_NextSpecialAttack );
            writer.Write( m_NextSpellTime );
			writer.Write( m_IsChanneling );
			writer.Write( m_HasStoneSkin );
			writer.Write( m_StoneSkinEnds );
			writer.Write( m_OriginalPhysicalResist );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
            m_AcidFogTimers = new Dictionary<Point3D, Timer>();

			if ( version >= 1 )
			{
				m_Rage = reader.ReadInt();
				m_NextSummonTime = reader.ReadDateTime();
				m_NextSpecialAttack = reader.ReadDateTime();
                m_NextSpellTime = reader.ReadDateTime();
				m_IsChanneling = reader.ReadBool();
				m_HasStoneSkin = reader.ReadBool();
				m_StoneSkinEnds = reader.ReadDateTime();
				m_OriginalPhysicalResist = reader.ReadInt();

				if ( m_IsChanneling )
					m_IsChanneling = false;

				if ( m_HasStoneSkin && DateTime.UtcNow >= m_StoneSkinEnds )
					RemoveStoneSkin();
			}

			LeechImmune = true;

			if ( m_Summons == null )
				m_Summons = new List<BaseCreature>();
		}
	}
}