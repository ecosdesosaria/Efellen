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

namespace Server.Mobiles
{
	[CorpseName( "Firefang's Corpse" )]
	public class FirefangTheWarchief : BaseCreature
	{
        // firefang has lots of buddies
		private const int MAX_SUMMONS_RAGE_0 = 16;
		private const int MAX_SUMMONS_RAGE_1 = 12;
		private const int MAX_SUMMONS_RAGE_2 = 8;
		
		private const int SUMMON_RANGE = 20;
		
		private static readonly Type[] SummonTypes = new Type[] 
		{ 
			typeof(Orc), 
			typeof(OrcBomber), 
			typeof(OrcBomber), 
			typeof(OrcishLord)
		};

		private static readonly List<Type> BossDrops = new List<Type>
		{
			typeof(Artifact_TunicOfImmolation),
			typeof(Artifact_GauntletsOfImmolation),
			typeof(Artifact_ArmsOfImmolation),
			typeof(Artifact_CoifOfImmolation),
			typeof(Artifact_LeggingsOfImmolation),
		};

		private int m_Rage = 0;
		private Mobile m_LastTarget;
		private DateTime m_NextSummonTime = DateTime.MinValue;
		private DateTime m_NextSpecialAttack = DateTime.MinValue;
		private DateTime m_NextBomb = DateTime.MinValue;
		private int m_Thrown;
		private List<BaseCreature> m_Summons = new List<BaseCreature>();
		private List<ExplosiveTile> m_ExplosiveTiles = new List<ExplosiveTile>();

		public override InhumanSpeech SpeechType{ get{ return InhumanSpeech.Orc; } }

		[Constructable]
		public FirefangTheWarchief () : base( AIType.AI_Mage, FightMode.Closest, 20, 1, 0.4, 0.8 )
		{
			Name = "Firefang";
			Title = "The Warchief";
			Body = 0x1d9;
			NameHue = 0x22;
			Hue = 348;
			BaseSoundID = 0x45A;

			SetStr( 496, 585 );
			SetDex( 155, 235 );
			SetInt( 206, 275 );

			SetHits( 3000 );
			SetDamage( 13, 24 );

			SetDamageType( ResistanceType.Fire, 50 );
			SetDamageType( ResistanceType.Physical, 50 );

			SetResistance( ResistanceType.Physical, 45 );
			SetResistance( ResistanceType.Fire, 65 );
			SetResistance( ResistanceType.Cold, 40 );
			SetResistance( ResistanceType.Poison, 50 );
			SetResistance( ResistanceType.Energy, 50 );

			SetSkill( SkillName.Magery, 82.5, 115.0 );
			SetSkill( SkillName.Psychology, 62.5, 85.0 );
			SetSkill( SkillName.Meditation, 72.5, 85.0 );
			SetSkill( SkillName.MagicResist, 75.5, 125.0 );
			SetSkill( SkillName.Tactics, 81.0, 95.0 );
			SetSkill( SkillName.FistFighting, 111.0, 125.0 );

			Fame = 15000;
			Karma = -15000;

			VirtualArmor = 60;

			PackItem( Loot.RandomArty() );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.UltraRich, 2 );
		}

		public override bool AutoDispel{ get{ return !Controlled; } }
		public override int TreasureMapLevel{ get{ return 3; } }
		public override bool CanRummageCorpses{ get{ return false; } }
		public override int BreathPhysicalDamage{ get{ return 0; } }
		public override int BreathFireDamage{ get{ return 100; } }
		public override int BreathColdDamage{ get{ return 0; } }
		public override int BreathPoisonDamage{ get{ return 0; } }
		public override int BreathEnergyDamage{ get{ return 0; } }
		public override int BreathEffectHue{ get{ return 348; } }
		public override int BreathEffectSound{ get{ return 0x64F; } }
		public override bool ReacquireOnMovement{ get{ return !Controlled; } }
		public override bool HasBreath{ get{ return true; } }
		public override double BreathEffectDelay{ get{ return 0.1; } }
		public override void BreathDealDamage( Mobile target, int form ){ base.BreathDealDamage( target, 22 ); }
		public override bool BleedImmune{ get{ return true; } }
		public override bool BardImmune { get { return true; } }
		public override bool Unprovokable { get { return true; } }
		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }

		public override void OnDamage( int amount, Mobile from, bool willKill )
		{
			m_LastTarget = from;
			Server.Misc.IntelligentAction.LeapToAttacker( this, from );
			
			if ( m_Rage >= 1 && DateTime.UtcNow >= m_NextSpecialAttack )
			{
				PerformRageAttack( from );
				m_NextSpecialAttack = DateTime.UtcNow + TimeSpan.FromSeconds( 12.6 - (m_Rage * 1.5) );
			}
			
			base.OnDamage( amount, from, willKill );
		}

		private void PerformRageAttack( Mobile target )
		{
			if ( target == null || target.Deleted || !target.Alive )
				return;

			int availableAttacks = m_Rage;
			int attackChoice = Utility.RandomMinMax( 1, availableAttacks );

			switch ( attackChoice )
			{
				case 1:
				{
					PerformBigBombAttack( target );
					break;
				}

				case 2:
				{
					PerformLightTheFuses();
					break;
				}
			}
		}

		private void PerformBigBombAttack( Mobile target )
		{
			if ( target == null || target.Deleted || target.Map == null )
				return;

			int radius = 4 + m_Rage;
			int minDamage = 20;
			int maxDamage = 35;

			PublicOverheadMessage( MessageType.Regular, 0x21, false, "*BOOM TIME!*" );
			
			this.MovingParticles( target, 0x1C19, 1, 0, false, true, 348, 0, 9502, 6014, 0x11D, EffectLayer.Waist, 0 );
			
			Timer.DelayCall( TimeSpan.FromSeconds( 1.0 ), delegate()
			{
				if ( target == null || target.Deleted || target.Map == null )
					return;

				Effects.SendLocationParticles( EffectItem.Create( target.Location, target.Map, EffectItem.DefaultDuration ), 0x36BD, 20, 10, 348, 0, 5044, 0 );
				Effects.PlaySound( target.Location, target.Map, 0x307 );

				IPooledEnumerable eable = target.GetMobilesInRange( radius );
				foreach ( Mobile m in eable )
				{
					if ( m == null || m == this || !m.Alive )
						continue;

					if ( m is BaseCreature )
					{
						BaseCreature bc = m as BaseCreature;
						if ( bc != null && bc.Team == this.Team )
							continue;
					}

					int damage = Utility.RandomMinMax( minDamage, maxDamage );
					AOS.Damage( m, this, damage, 0, 100, 0, 0, 0 );
					
					SetOnFire( m );
				}
				eable.Free();

				LightTilesOnFire( target.Location, target.Map, radius );
			});
		}

		private void PerformLightTheFuses()
		{
			if ( this.Map == null )
				return;

			int tilesToMark = Utility.RandomMinMax( 8, 12 ) + (m_Rage * 3);
			int radius = 10;
			int marked = 0;

			PublicOverheadMessage( MessageType.Regular, 0x21, false, "*LIGHT DA FUSES BOYS!*" );
			PlaySound( 0x233 );

			List<Point3D> validLocations = new List<Point3D>();

			for ( int x = -radius; x <= radius; x++ )
			{
				for ( int y = -radius; y <= radius; y++ )
				{
					Point3D loc = new Point3D( this.X + x, this.Y + y, this.Z );
					
					if ( !this.Map.CanSpawnMobile( loc.X, loc.Y, loc.Z ) )
						continue;

					bool occupied = false;
					IPooledEnumerable eable = this.Map.GetMobilesInRange( loc, 0 );
					foreach ( Mobile m in eable )
					{
						occupied = true;
						break;
					}
					eable.Free();

					if ( !occupied )
						validLocations.Add( loc );
				}
			}

			while ( marked < tilesToMark && validLocations.Count > 0 )
			{
				int index = Utility.Random( validLocations.Count );
				Point3D loc = validLocations[index];
				validLocations.RemoveAt( index );

				CreateExplosiveTile( loc );
				marked++;
			}
		}

		private void CreateExplosiveTile( Point3D location )
		{
			if ( this.Map == null )
				return;

			ExplosiveTile tile = new ExplosiveTile( this, location, this.Map );
			m_ExplosiveTiles.Add( tile );
		}

		private void LightTilesOnFire( Point3D center, Map map, int radius )
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

					Effects.SendLocationParticles( EffectItem.Create( loc, map, TimeSpan.FromSeconds( 2.0 ) ), 0x3709, 10, 30, 348, 0, 5052, 0 );
				}
			}
		}

		private void SetOnFire( Mobile m )
		{
			if ( m == null || m.Deleted )
				return;

			m.FixedParticles( 0x3709, 10, 30, 5052, 348, 0, EffectLayer.Waist );
		}

		public override void CheckReflect( Mobile caster, ref bool reflect )
		{
			int chance = m_Rage * 7;
			reflect = ( Utility.Random(100) < chance );
		}

		private int CountSummons()
		{
			int count = 0;
			IPooledEnumerable eable = GetMobilesInRange( SUMMON_RANGE );
			
			foreach ( Mobile m in eable )
			{
				Type mobileType = m.GetType();
				foreach ( Type summonType in SummonTypes )
				{
					if ( mobileType == summonType )
					{
						count++;
						break;
					}
				}
			}
			eable.Free();
			return count;
		}

		private int GetMaxSummons()
		{
			switch( m_Rage )
			{
				case 0: return MAX_SUMMONS_RAGE_0;
				case 1: return MAX_SUMMONS_RAGE_1;
				case 2: return MAX_SUMMONS_RAGE_2;
				default: return 8;
			}
		}

		private void SpawnCreature( Mobile target )
		{
			Map map = this.Map;
			if ( map == null || target == null || target.Deleted )
				return;

			if ( DateTime.UtcNow < m_NextSummonTime )
				return;

			int currentSummons = CountSummons();
			int maxSummons = GetMaxSummons();

			if ( currentSummons >= maxSummons )
				return;

			PlaySound( 0x216 );

			int newSummons;
			string song;
			
			switch( m_Rage )
			{
				case 0: 
					newSummons = Utility.RandomMinMax( 3, 4 ); 
					song = "Me MATES WILL CUT YOU!"; 
					break;
				case 1: 
					newSummons = Utility.RandomMinMax( 2, 3 ); 
					song = "WE WILL EAT YOU RAW!"; 
					break;
				case 2: 
					newSummons = Utility.RandomMinMax( 1, 2 ); 
					song = "*KILL IT KILL IT FASTA!*"; 
					break;
				default:
					newSummons = 2;
					song = "";
					break;
			}
			PublicOverheadMessage( MessageType.Regular, 0x21, false, song );
		
			for ( int i = 0; i < newSummons; ++i )
			{
				BaseCreature monster = CreateMonster();
				if ( monster == null )
					continue;

				monster.Team = this.Team;
				Point3D loc = GetSpawnLocation( map );

				monster.IsTempEnemy = true;
				monster.MoveToWorld( loc, map );
				monster.Combatant = target;
				RegisterSummon(monster);
			}

			m_NextSummonTime = DateTime.UtcNow + TimeSpan.FromSeconds( 18.0 - (m_Rage * 0.5) );
		}

		public void RegisterSummon(BaseCreature bc)
		{
			if (bc == null)
				return;

			m_Summons.Add(bc);

			Timer.DelayCall(TimeSpan.FromMinutes(1), delegate()
			{
				if (bc != null && !bc.Deleted && bc.Alive)
					bc.Delete();
			});
		}

		private BaseCreature CreateMonster()
		{
			int rand = Utility.Random( 100 );

			switch ( m_Rage )
			{
				case 0:
					return new Orc();
				case 1:
					return new OrcBomber();	
				case 2:
					if ( rand < 60 )
						return new OrcBomber();
					else
						return new OrcishLord();
				default:
					return new OrcBomber();
			}
		}

		private Point3D GetSpawnLocation( Map map )
		{
			for ( int j = 0; j < 20; ++j )
			{
				int x = X + Utility.Random( 13 ) - 6;
				int y = Y + Utility.Random( 13 ) - 6;
				int z = map.GetAverageZ( x, y );

				if ( map.CanFit( x, y, this.Z, 16, false, false ) )
					return new Point3D( x, y, Z );
				else if ( map.CanFit( x, y, z, 16, false, false ) )
					return new Point3D( x, y, z );
			}

			return this.Location;
		}

		private void TrySummonCreature( Mobile target )
		{
			if ( target == null || target.Deleted )
				return;

			double[] chances = { 0.20, 0.30, 0.55 };

			if ( m_Rage >= 0 && m_Rage < chances.Length && chances[m_Rage] >= Utility.RandomDouble() )
				SpawnCreature( target );
		}

		public override void OnGotMeleeAttack( Mobile attacker )
		{
			TrySummonCreature( attacker );
			base.OnGotMeleeAttack( attacker );
		}

		public override void OnGaveMeleeAttack( Mobile defender )
		{
			TrySummonCreature( defender );
			base.OnGaveMeleeAttack( defender );
		}

		public override bool OnBeforeDeath()
		{
			if ( m_Rage == 0 )
			{
				PublicOverheadMessage( MessageType.Regular, 0x21, false, "ME NO HURT!" );
				this.Hits = this.HitsMax;
				this.FixedParticles( 0x376A, 9, 32, 5030, EffectLayer.Waist );
				this.PlaySound( 0x202 );
				
				SetStr( Str + 100 );
				SetDex( Dex + 25 );
				SetDamage( 22, 29 );
				
				m_Rage = 1;
				return false;
			}
			else if ( m_Rage == 1 )
			{
				PublicOverheadMessage( MessageType.Regular, 0x21, false, "ME WILL CHEW UR BONES*" );
				this.Hits = this.HitsMax;
				this.FixedParticles( 0x376A, 9, 32, 5030, EffectLayer.Waist );
				this.PlaySound( 0x202 );
				
				SetStr( Str + 150 );
				SetDex( Dex + 35 );
				SetDamage( 29, 44 );
				VirtualArmor += 5;
				m_Rage = 2;
				return false;
			}
			else if ( m_Rage == 2 )
			{
				Effects.SendLocationParticles( EffectItem.Create( this.Location, this.Map, EffectItem.DefaultDuration ), 0x3728, 10, 10, 2023 );
				this.PlaySound( 0x1FE );
				PublicOverheadMessage( MessageType.Regular, 0x21, false, "AM...DONE*" );
				Mobile killer = this.LastKiller;
				if (killer != null && killer.Player && killer.Karma > 0)
				{
					int marks = Utility.RandomMinMax(11, 31);
					Server.Custom.DefenderOfTheRealm.MarkLootHelper.AwardMarks(killer, 1, marks);
				}
			}
			
			return base.OnBeforeDeath();
		}

		public override void OnDelete()
		{
			CleanupSummons();
			CleanupExplosiveTiles();
			base.OnDelete();
		}

		private void CleanupSummons()
		{
			for (int i = m_Summons.Count - 1; i >= 0; i--)
			{
				BaseCreature bc = m_Summons[i];

				if (bc != null && !bc.Deleted)
					bc.Delete();
			}
			m_Summons.Clear();
		}

		private void CleanupExplosiveTiles()
		{
			for (int i = m_ExplosiveTiles.Count - 1; i >= 0; i--)
			{
				ExplosiveTile tile = m_ExplosiveTiles[i];
				if (tile != null)
					tile.Stop();
			}
			m_ExplosiveTiles.Clear();
		}

		public void RemoveExplosiveTile( ExplosiveTile tile )
		{
			m_ExplosiveTiles.Remove( tile );
		}

		public override void OnDeath( Container c )
		{
			base.OnDeath( c );

			BossLootSystem.AwardBossSpecial(this, BossDrops, 15);

			int amt = Utility.RandomMinMax( 1, 2 );
			for ( int i = 0; i < amt; i++ )
			{
				c.DropItem( new EtherealPowerScroll() );
			}
			
			RichesSystem.SpawnRiches( m_LastTarget, 2 );
		}

		public override void OnAfterSpawn()
		{
			base.OnAfterSpawn();
			LeechImmune = true;
		}

		public FirefangTheWarchief( Serial serial ) : base( serial )
		{
		}

		public override void OnActionCombat()
		{
			Mobile combatant = Combatant;

			if ( combatant == null || combatant.Deleted || combatant.Map != Map || !InRange( combatant, 12 ) || !CanBeHarmful( combatant ) || !InLOS( combatant ) )
				return;

			if ( DateTime.UtcNow >= m_NextBomb )
			{
				ThrowBomb( combatant );

				m_Thrown++;

				if ( 0.85 >= Utility.RandomDouble() && (m_Thrown % 2) == 1 )
					m_NextBomb = DateTime.UtcNow + TimeSpan.FromSeconds( 3.0 );
				else
					m_NextBomb = DateTime.UtcNow + TimeSpan.FromSeconds( 5.0 + (10.0 * Utility.RandomDouble()) );
			}
		}

		public void ThrowBomb( Mobile m )
		{
			DoHarmful( m );

			this.MovingParticles( m, 0x1C19, 1, 0, false, true, 0, 0, 9502, 6014, 0x11D, EffectLayer.Waist, 0 );

			new BombTimer( m, this, m_Rage ).Start();
		}

		private class BombTimer : Timer
		{
			private Mobile m_Mobile;
			private Mobile m_From;
			private int m_Rage;

			public BombTimer( Mobile m, Mobile from, int rage ) : base( TimeSpan.FromSeconds( 1.0 ) )
			{
				m_Mobile = m;
				m_From = from;
				m_Rage = rage;
				Priority = TimerPriority.TwoFiftyMS;
			}

			protected override void OnTick()
			{
				if ( m_Mobile == null || m_Mobile.Deleted )
					return;

				m_Mobile.PlaySound( 0x11D );
				
				int minDmg = 10 + (m_Rage * 2);
				int maxDmg = 20 + (m_Rage * 2);
				
				AOS.Damage( m_Mobile, m_From, Utility.RandomMinMax( minDmg, maxDmg ), 0, 100, 0, 0, 0 );
			}
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 1 ); // version

			writer.Write( m_Rage );
			writer.Write( m_NextSummonTime );
			writer.Write( m_NextSpecialAttack );
			writer.Write( m_NextBomb );
			writer.Write( m_Thrown );
			
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();

			if ( version >= 1 )
			{
				m_Rage = reader.ReadInt();
				m_NextSummonTime = reader.ReadDateTime();
				m_NextSpecialAttack = reader.ReadDateTime();
				m_NextBomb = reader.ReadDateTime();
				m_Thrown = reader.ReadInt();
			}

			LeechImmune = true;
			
			m_Summons = new List<BaseCreature>();
			m_ExplosiveTiles = new List<ExplosiveTile>();
		}
	}

	public class ExplosiveTile
	{
		private FirefangTheWarchief m_Boss;
		private Point3D m_Location;
		private Map m_Map;
		private Item m_Visual;
		private Timer m_Timer;

		public ExplosiveTile( FirefangTheWarchief boss, Point3D location, Map map )
		{
			m_Boss = boss;
			m_Location = location;
			m_Map = map;

			m_Visual = new InternalItem();
			m_Visual.MoveToWorld( location, map );

			// Show warning effect
			Effects.SendLocationParticles(
                EffectItem.Create( location, map, TimeSpan.FromSeconds( 0.5 ) ),
                0x3728, 
                5,
                10,
                0,
                0,
                0,
                0
            );

			Effects.PlaySound( location, map, 0x44D );

			double delay = Utility.RandomMinMax( 30, 90 ) / 10.0; // 3-9 seconds
			m_Timer = Timer.DelayCall( TimeSpan.FromSeconds( delay ), new TimerCallback( Explode ) );
		}

		public void Stop()
		{
			if ( m_Timer != null && m_Timer.Running )
				m_Timer.Stop();

			if ( m_Visual != null && !m_Visual.Deleted )
				m_Visual.Delete();
		}

		private void Explode()
		{
			if ( m_Map == null || m_Boss == null || m_Boss.Deleted )
			{
				Stop();
				return;
			}

            

			Effects.SendLocationParticles(
                EffectItem.Create( m_Location, m_Map, EffectItem.DefaultDuration ),
                0x3709,
                10,
                30,
                0,     
                0,
                5052,
                0
            );          

            // Smoke after explosion
            Effects.SendLocationParticles(
                EffectItem.Create( m_Location, m_Map, TimeSpan.FromSeconds( 1.5 ) ),
                0x3728,
                10,
                15,
                0,    
                0,
                0,
                0
            );
			Effects.PlaySound( m_Location, m_Map, 0x307 );

			// Damage anyone standing on this tile
			IPooledEnumerable eable = m_Map.GetMobilesInRange( m_Location, 0 );
			foreach ( Mobile m in eable )
			{
				if ( m == null || m == m_Boss || !m.Alive )
					continue;

				if ( m is BaseCreature )
				{
					BaseCreature bc = m as BaseCreature;
					if ( bc != null && bc.Team == m_Boss.Team )
						continue;
				}
                // heavy fire damage on anyone standing on the tile when it blows up
				AOS.Damage( m, m_Boss, 180, 0, 100, 0, 0, 0 );
				m.FixedParticles( 0x3709, 10, 30, 5052, 348, 0, EffectLayer.Waist );
			}
			eable.Free();

			if ( m_Visual != null && !m_Visual.Deleted )
				m_Visual.Delete();

			if ( m_Boss != null && !m_Boss.Deleted )
				m_Boss.RemoveExplosiveTile( this );
		}

		private class InternalItem : Item
		{
			public InternalItem() : base( 0x1B1F ) // Fire runes
			{
				Movable = false;
				Hue = 1260;
				Name = "Explosive Runes";
			}

			public InternalItem( Serial serial ) : base( serial )
			{
			}

			public override void Serialize( GenericWriter writer )
			{
				base.Serialize( writer );
				writer.Write( (int) 0 );
			}

			public override void Deserialize( GenericReader reader )
			{
				base.Deserialize( reader );
				int version = reader.ReadInt();
				
				Timer.DelayCall( TimeSpan.Zero, new TimerCallback( Delete ) );
			}
		}
	}
}