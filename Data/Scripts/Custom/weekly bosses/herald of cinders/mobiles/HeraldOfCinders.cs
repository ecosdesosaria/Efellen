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
using Server.Spells;
using Server.EffectsUtil;
using Server.Custom;

namespace Server.Mobiles
{
	[CorpseName( "Herald of Fire's Corpse" )]
	public class HeraldOfCinders : BaseCreature
	{
		private const int MAX_SUMMONS_RAGE_0 = 16;
		private const int MAX_SUMMONS_RAGE_1 = 14;
		private const int MAX_SUMMONS_RAGE_2 = 12;
		private const int MAX_SUMMONS_RAGE_3 = 8;
		
		private const int SUMMON_RANGE = 12;
		private const int FLAME_CHARGE_DISTANCE = 8;
		private const int MAGMA_DURATION = 12;
		
		private static readonly Type[] SummonTypes = new Type[] 
		{ 
			typeof(BabyDragon), 
			typeof(YoungDragon), 
			typeof(AncientDrake), 
			typeof(Wyrm), 
			typeof(Drake) 
		};

		private static readonly Type[] ImmuneToMagmaTypes = new Type[]
		{
			typeof(BabyDragon),
			typeof(YoungDragon),
			typeof(AncientDrake),
			typeof(Wyrm),
			typeof(Drake),
			typeof(HeraldOfCinders)
		};

		private static readonly List<Type> BossDrops = new List<Type>
    	{
    	    typeof(Artifact_CinderForgedArms),
    	    typeof(Artifact_CinderForgedGloves),
    	    typeof(Artifact_CinderForgedHelm),
    	    typeof(Artifact_CinderForgedLeggings),
			typeof(Artifact_CinderForgedBreastplate),
    	};

		private int m_Rage = 0;
		private Mobile m_LastTarget;
		private DateTime m_NextSummonTime = DateTime.MinValue;
		private DateTime m_NextSpecialAttack = DateTime.MinValue;
		private List<BaseCreature> m_Summons = new List<BaseCreature>();
		private List<MagmaTile> m_MagmaTiles = new List<MagmaTile>();

		[Constructable]
		public HeraldOfCinders () : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "The Herald of Cinders";
			Body = 713;
			BaseSoundID = 362;
			NameHue = 0x22;
			Hue = 1111;
			
			SetStr( 896, 985 );
			SetDex( 125, 175 );
			SetInt( 586, 675 );

			SetHits( 19000 );
			SetDamage( 23, 34 );

			SetDamageType( ResistanceType.Physical, 100 );
		
			SetResistance( ResistanceType.Physical, 60 );
			SetResistance( ResistanceType.Fire, 80 );
			SetResistance( ResistanceType.Cold, 40 );
			SetResistance( ResistanceType.Poison, 70 );
			SetResistance( ResistanceType.Energy, 70 );

			SetSkill( SkillName.Meditation, 112.5, 125.0 );
			SetSkill( SkillName.MagicResist, 125.5, 150.0 );
			SetSkill( SkillName.Tactics, 101.0, 125.0 );
			SetSkill( SkillName.FistFighting, 101.0, 125.0 );
			SetSkill( SkillName.Magery, 120.0, 125.0);

			Fame = 35000;
			Karma = -35000;
			VirtualArmor = 60;

			if ( Backpack == null )
				AddItem( new Backpack() );

			PackItem( Loot.RandomArty() );
			PackItem( Loot.RandomArty() );
			PackItem( Loot.RandomArty() );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.UltraRich, 8 );
		}

		public override int TreasureMapLevel{ get{ return 5; } }
		public override bool CanRummageCorpses{ get{ return false; } }
		public override bool ReacquireOnMovement{ get{ return !Controlled; } }
		public override bool BleedImmune{ get{ return true; } }
		public override bool BardImmune { get { return true; } }
		public override bool Unprovokable { get { return true; } }
		public override Poison PoisonImmune{ get{ return Poison.Greater; } }

		public override void Damage( int amount, Mobile from )
		{
			if ( m_Rage < 3 && this.Hits - amount <= 0 )
			{
				IncreaseRage(m_Rage);
			}
			else
			{
				base.Damage( amount, from );
			}
		}

	
		public override void OnDamage( int amount, Mobile from, bool willKill )
		{
			m_LastTarget = from;
			Server.Misc.IntelligentAction.LeapToAttacker( this, from );
			
			if ( m_Rage >= 1 && DateTime.UtcNow >= m_NextSpecialAttack )
			{
				PerformRageAttack( from );
				m_NextSpecialAttack = DateTime.UtcNow + TimeSpan.FromSeconds( 30 - (m_Rage * 2) );
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
				case 1: // flame charge 
					{
						PerformFlameCharge( target );
						break;
					}

				case 2: // fire blast
					{
						PublicOverheadMessage( MessageType.Regular, 0x21, false, "I shall consume you!" );
						PlaySound( 0x64F );
						FixedParticles( 0x376A, 9, 32, 5030, EffectLayer.Waist );

						IPooledEnumerable eable = GetMobilesInRange( 6 );
						foreach ( Mobile m in eable )
						{
							if ( m != this && m.Player && m.Alive && CanBeHarmful( m ) )
							{
								DoHarmful( m );

								int damage = Utility.RandomMinMax( 35, 49 );
								AOS.Damage( m, this, damage, 0, 100, 0, 0, 0 );
								m.PlaySound( 0x1FB );
							}
						}
						SlamVisuals.SlamVisual(this, 6, 0x36B0, 1161);
						eable.Free();
						break;
					}

				case 3: // magma eruption
					{
						PerformMagmaEruption();
						break;
					}
			}
		}

		private void PerformFlameCharge( Mobile target )
		{
			if ( target == null || this.Map == null )
				return;

			PublicOverheadMessage( MessageType.Regular, 0x21, false, "The dragon prepares to lunge forward!" );
			PlaySound( 0x15F );

			Timer.DelayCall( TimeSpan.FromSeconds( 2.0 ), delegate()
			{
				if ( Deleted || !Alive || target.Deleted )
					return;

				Direction chargeDir = GetDirectionTo( target );
				Point3D startLoc = Location;
				Point3D endLoc = startLoc;
				List<Mobile> hitMobiles = new List<Mobile>();

				for ( int i = 1; i <= FLAME_CHARGE_DISTANCE; i++ )
				{
					int offsetX = 0, offsetY = 0;
					GetOffset( chargeDir, out offsetX, out offsetY );

					Point3D checkLoc = new Point3D( startLoc.X + (offsetX * i), startLoc.Y + (offsetY * i), startLoc.Z );

					if ( !Map.CanFit( checkLoc, 16, false, false ) )
						break;

					endLoc = checkLoc;

					IPooledEnumerable eable = Map.GetMobilesInRange( checkLoc, 0 );
					foreach ( Mobile m in eable )
					{
						if ( m != this && m.Alive && CanBeHarmful( m ) && !hitMobiles.Contains( m ) )
						{
							hitMobiles.Add( m );
						}
					}
					eable.Free();

					Effects.SendLocationEffect( checkLoc, Map, 0x3728, 10, 10, 2023, 0 );
				}

				Point3D oldLoc = Location;
				Location = endLoc;
				ProcessDelta();

				PlaySound( 0x665 );
				FixedParticles( 0x3709, 10, 30, 5052, EffectLayer.Waist );

				foreach ( Mobile m in hitMobiles )
				{
					if ( m.Deleted || !m.Alive )
						continue;

					DoHarmful( m );

					int damage = Utility.RandomMinMax( 35, 49 );
					int physDamage = damage / 2;
					int fireDamage = damage - physDamage;

					AOS.Damage( m, this, damage, physDamage, fireDamage, 0, 0, 0 );

					m.PlaySound( 0x1FB );
					m.FixedParticles( 0x3709, 10, 30, 5052, EffectLayer.Waist );

					Direction knockDir = GetDirectionTo( m );
					int knockX = 0, knockY = 0;
					GetOffset( knockDir, out knockX, out knockY );

					Point3D knockLoc = new Point3D( m.X + knockX, m.Y + knockY, m.Z );

					if ( Map.CanFit( knockLoc, 16, false, false ) )
					{
						m.Location = knockLoc;
						m.ProcessDelta();
					}
				}
			});
		}

		private void GetOffset( Direction d, out int xOffset, out int yOffset )
		{
			xOffset = 0;
			yOffset = 0;

			switch ( d & Direction.Mask )
			{
				case Direction.North: yOffset = -1; break;
				case Direction.South: yOffset = 1; break;
				case Direction.West: xOffset = -1; break;
				case Direction.East: xOffset = 1; break;
				case Direction.Right: xOffset = 1; yOffset = -1; break;
				case Direction.Left: xOffset = -1; yOffset = 1; break;
				case Direction.Down: xOffset = 1; yOffset = 1; break;
				case Direction.Up: xOffset = -1; yOffset = -1; break;
			}
		}

		private void PerformMagmaEruption()
		{
			if ( Map == null )
				return;

			PublicOverheadMessage( MessageType.Regular, 0x21, false, "The ground trembles with fury!" );
			PlaySound( 0x307 );

			int tileCount = Utility.RandomMinMax( 8, 14 );
			List<Point3D> validLocations = new List<Point3D>();

			for ( int attempts = 0; attempts < tileCount * 3 && validLocations.Count < tileCount; attempts++ )
			{
				int range = Utility.RandomMinMax( 3, 10 );
				int xOffset = Utility.RandomMinMax( -range, range );
				int yOffset = Utility.RandomMinMax( -range, range );

				Point3D loc = new Point3D( X + xOffset, Y + yOffset, Z );

				if ( Map.CanFit( loc, 16, false, false ) && !IsLocationOccupied( loc ) )
				{
					validLocations.Add( loc );
				}
			}

			foreach ( Point3D loc in validLocations )
			{
				Timer.DelayCall( TimeSpan.FromSeconds( Utility.RandomDouble() * 0.5 ), delegate()
				{
					if ( Deleted || !Alive )
						return;

					Effects.SendLocationEffect( loc, Map, 0x36B0, 20, 10, 1161, 0 );
					Effects.PlaySound( loc, Map, 0x11D );

					Timer.DelayCall( TimeSpan.FromSeconds( 0.5 ), delegate()
					{
						MagmaTile magma = new MagmaTile( this, loc, Map );
						m_MagmaTiles.Add( magma );

						Timer.DelayCall( TimeSpan.FromSeconds( MAGMA_DURATION ), delegate()
						{
							if ( magma != null )
							{
								m_MagmaTiles.Remove( magma );
								magma.Delete();
							}
						});
					});
				});
			}
		}

		private bool IsLocationOccupied( Point3D loc )
		{
			IPooledEnumerable eable = Map.GetMobilesInRange( loc, 0 );
			bool occupied = false;

			foreach ( Mobile m in eable )
			{
				occupied = true;
				break;
			}

			eable.Free();
			return occupied;
		}

		public static bool IsImmuneToMagma( Mobile m )
		{
			if ( m == null )
				return false;

			Type mType = m.GetType();
			foreach ( Type immuneType in ImmuneToMagmaTypes )
			{
				if ( mType == immuneType )
					return true;
			}

			return false;
		}
		
		public override void CheckReflect( Mobile caster, ref bool reflect )
		{
			int chance = m_Rage * 22;
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
				case 3: return MAX_SUMMONS_RAGE_3;
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
					newSummons = Utility.RandomMinMax( 4, 8 ); 
					song = "Feed the wyrmlings!"; 
					break;
				case 1: 
					newSummons = Utility.RandomMinMax( 4, 8 ); 
					song = "Come, children, it's time to feast!"; 
					break;
				case 2: 
					newSummons = Utility.RandomMinMax( 3, 6 ); 
					song = "Thou shall not leave my lair unscathed! Come forth, my spawn!"; 
					break;
				case 3: 
					newSummons = Utility.RandomMinMax( 2, 4 );
					song = "Spawn of Ashardalon, Awaken!"; 
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
			}

			m_NextSummonTime = DateTime.UtcNow + TimeSpan.FromSeconds( 6.0 - (m_Rage * 0.5) );
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
					return new BabyDragon();
				case 1:
					if ( rand < 35 )
						return new YoungDragon();
					else
						return new BabyDragon();
				case 2:
					if ( rand < 20 )
						return new Wyrm();
					else if ( rand < 45 )
						return new Drake();
					else
						return new YoungDragon();
				case 3:
					if ( rand < 20 )
						return new AncientDrake();
					else if ( rand < 35 )
						return new Wyrm();
					else
						return new Drake();
				default:
					return new BabyDragon();
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

			double[] chances = { 0.10, 0.20, 0.33, 0.50 };

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

		public void IncreaseRage(int rage)
		{
			if ( rage == 0 )
			{
				PublicOverheadMessage( MessageType.Regular, 0x21, false, "You shall burn!" );
				this.Hits = this.HitsMax;
				this.FixedParticles( 0x376A, 9, 32, 5030, EffectLayer.Waist );
				this.PlaySound( 0x202 );
				SetStr( Str + 40 );
				SetDamage( 28, 34 );
				m_Rage = 1;
				return;
			}
			else if ( rage == 1 )
			{
				PublicOverheadMessage( MessageType.Regular, 0x21, false, "Fire eternal shall consume you!" );
				this.Hits = this.HitsMax;
				this.FixedParticles( 0x376A, 9, 32, 5030, EffectLayer.Waist );
				this.PlaySound( 0x202 );
				SetStr( Str + 80 );
				SetDex( Dex + 15 );
				SetDamage( 33, 44 );
				VirtualArmor += 10;
				m_Rage = 2;
				return;
			}
			else if ( rage == 2 )
			{
				PublicOverheadMessage( MessageType.Regular, 0x21, false, "Ashardalon, heed my call!" );
				this.Hits = this.HitsMax;
				this.FixedParticles( 0x376A, 9, 32, 5030, EffectLayer.Waist );
				this.PlaySound( 0x202 );
				SetStr( Str + 125 );
				SetDex( Dex + 50 );
				SetDamage( 40, 55 );
				VirtualArmor += 15;             
				m_Rage = 3;
				return;
			}
			else
			{
				Effects.SendLocationParticles( EffectItem.Create( this.Location, this.Map, EffectItem.DefaultDuration ), 0x3728, 10, 10, 2023 );
				this.PlaySound( 0x1FE );
				PublicOverheadMessage( MessageType.Regular, 0x21, false, "Forgive me...My lord..." );
				Mobile killer = this.LastKiller;
				if ( killer != null && killer.Player && killer.Karma > 0 )
				{
					try
					{
						int marks = Utility.RandomMinMax( 31, 47 );
						Server.Custom.DefenderOfTheRealm.MarkLootHelper.AwardMarks( killer, 1, marks );
					}
					catch { }
				}
			}
		}
		
		public override void OnDeath( Container c )
		{
			//sanity
			if ( m_Rage < 3 )
				return;
				
			base.OnDeath( c );

			BossLootSystem.AwardBossSpecial(this,BossDrops, 15);

			if ( Utility.RandomDouble() < 0.15 )
			{
				c.DropItem( new EternalPowerScroll() );
			}

			int amt = Utility.RandomMinMax( 3, 9 );
			for ( int i = 0; i < amt; i++ )
			{
				c.DropItem( new EtherealPowerScroll() );
			}
			// gold explosion
			RichesSystem.SpawnRiches( m_LastTarget, 5 );
		}

		public override void OnDelete()
		{
			CleanupSummons();
			CleanupMagmaTiles();
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

		private void CleanupMagmaTiles()
		{
			for (int i = m_MagmaTiles.Count - 1; i >= 0; i--)
			{
				MagmaTile tile = m_MagmaTiles[i];

				if (tile != null && !tile.Deleted)
					tile.Delete();
			}
			m_MagmaTiles.Clear();
		}

		public override void OnAfterSpawn()
		{
			base.OnAfterSpawn();
			LeechImmune = true;
		}

		public HeraldOfCinders( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 1 ); // version

			writer.Write( m_Rage );
			writer.Write( m_NextSummonTime );
			writer.Write( m_NextSpecialAttack );
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
			}

			LeechImmune = true;
		}
	}
}