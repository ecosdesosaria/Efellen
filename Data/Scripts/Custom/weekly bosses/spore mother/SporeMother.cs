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

namespace Server.Mobiles
{
	[CorpseName( "Spore Mother's Corpse" )]
	public class SporeMother : BaseCreature
	{
		private const int MAX_SUMMONS_RAGE_0 = 4;
		private const int MAX_SUMMONS_RAGE_1 = 3;
		private const int MAX_SUMMONS_RAGE_2 = 2;
		
		private const int SUMMON_RANGE = 22;
		
		private static readonly Type[] SummonTypes = new Type[] 
		{ 
			typeof(WhippingVine), 
			typeof(Fungal), 
			typeof(FungalMage), 
			typeof(UmberHulk)
		};

		private int m_Rage = 0;
		private Mobile m_LastTarget;
		private DateTime m_NextSummonTime = DateTime.MinValue;
		private DateTime m_NextSpecialAttack = DateTime.MinValue;
        private List<BaseCreature> m_Summons = new List<BaseCreature>();

		[Constructable]
		public SporeMother () : base( AIType.AI_Mage, FightMode.Closest, 20, 1, 0.4, 0.8 )
		{
			Name = "Spore Mother";

			Body = 341;
			NameHue = 0x22;
			Hue = 0x497;

			SetStr( 496, 585 );
			SetDex( 155, 185 );
			SetInt( 286, 375 );

			SetHits( 10000 );
			SetDamage( 13, 24 );

			SetDamageType( ResistanceType.Physical, 100 );
			SetResistance( ResistanceType.Fire, 45 );
			SetResistance( ResistanceType.Cold, 40 );
			SetResistance( ResistanceType.Poison, 70 );
			SetResistance( ResistanceType.Energy, 50 );

			SetSkill( SkillName.Meditation, 82.5, 95.0 );
			SetSkill( SkillName.MagicResist, 75.5, 125.0 );
			SetSkill( SkillName.Tactics, 81.0, 95.0 );
			SetSkill( SkillName.FistFighting, 101.0, 115.0 );

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
		public override int BreathFireDamage{ get{ return 0; } }
		public override int BreathColdDamage{ get{ return 0; } }
		public override int BreathPoisonDamage{ get{ return 100; } }
		public override int BreathEnergyDamage{ get{ return 0; } }
		public override int BreathEffectHue{ get{ return 0x481; } }
		public override int BreathEffectSound{ get{ return 0x64F; } }
		public override bool ReacquireOnMovement{ get{ return !Controlled; } }
		public override bool HasBreath{ get{ return true; } }
		public override double BreathEffectDelay{ get{ return 0.1; } }
		public override void BreathDealDamage( Mobile target, int form ){ base.BreathDealDamage( target, 22 ); }
		public override bool BleedImmune{ get{ return true; } }
		public override bool BardImmune { get { return true; } }
		public override bool Unprovokable { get { return true; } }
		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }

		public override int GetAngerSound()
		{
			return 0x451;
		}

		public override int GetIdleSound()
		{
			return 0x452;
		}

		public override int GetAttackSound()
		{
			return 0x453;
		}

		public override int GetHurtSound()
		{
			return 0x454;
		}

		public override int GetDeathSound()
		{
			return 0x455;
		}

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

			switch ( attackChoice  )
			{
				case 1: // Poison blast
				{
					PublicOverheadMessage( MessageType.Regular, 0x21, false, "*Releases a burst of crippling poison!*" );
					PlaySound( 0x64F );
					FixedParticles( 0x376A, 9, 32, 5030, EffectLayer.Waist );
					IPooledEnumerable eable = GetMobilesInRange( 6 );
					foreach ( Mobile m in eable )
					{
						if ( m != this && m.Player && m.Alive && CanBeHarmful( m ) )
						{
							DoHarmful( m );
							int damage = Utility.RandomMinMax( 11, 22 );
							AOS.Damage( m, this, damage, 0, 0, 100, 0, 0 );
							m.PlaySound( 0x1FB );
                            m.ApplyPoison( this, Poison.Deadly );
						}
					}
					SlamVisuals.SlamVisual(this, 6, 0x36B0, 267);
					eable.Free();
					break;
				}

				case 2: // entangling vines + bleed
				{
					PublicOverheadMessage( MessageType.Regular, 0x21, false, "*calls forth piercing vines*" );
					PlaySound( 0x133 );
					FixedParticles( 0x3728, 1, 13, 9912, 0x21, 7, EffectLayer.Head );
					IPooledEnumerable eable = GetMobilesInRange( 6 );
					foreach ( Mobile m in eable )
					{
						if ( m != this && m.Player && m.Alive && CanBeHarmful( m ) && Server.Items.BaseRace.IsBleeder( m ) )
						{
							TransformContext context = TransformationSpellHelper.GetContext( m );
							bool isImmune = ( context != null && ( context.Type == typeof( LichFormSpell ) || context.Type == typeof( WraithFormSpell ) ) );
							if ( m is BaseCreature && ((BaseCreature)m).BleedImmune )
								isImmune = true;
							if ( !isImmune )
							{
								DoHarmful( m );
								m.PlaySound( 0x133 );
								m.FixedParticles( 0x377A, 244, 25, 9950, 31, 0, EffectLayer.Waist );
								if ( m is PlayerMobile )
								{
									m.LocalOverheadMessage( MessageType.Regular, 0x982, false, "You are bleeding profusely!" );
								}
            					BeginBossBleed( m, this, 6 );
							}
						}
                    	m.Paralyze( TimeSpan.FromSeconds( getParalyzeDuration( m ) ) );
					}
					SlamVisuals.SlamVisual(this, 6, 0x36B0, 0x4F6);
					eable.Free();
					break;
				}
			}
		}

		private int getParalyzeDuration(Mobile m)
		{
			int resist = (int)(m.Skills.MagicResist.Value);
			// 2s at 125, 8s at 0 magic resist
			int duration = 8 - (int)(resist * (6.0 / 125.0));
			return duration;
		}

		private static Hashtable m_BossBleedTable = new Hashtable();

		public static void BeginBossBleed( Mobile m, Mobile from, int totalTicks )
		{
			Timer t = (Timer)m_BossBleedTable[m];
			if ( t != null )
				t.Stop();

			t = new BossBleedTimer( from, m, totalTicks );
			m_BossBleedTable[m] = t;
			t.Start();
		}

		public static void DoBossBleed( Mobile m, Mobile from, int level )
		{
			if ( m.Alive && Server.Items.BaseRace.IsBleeder( m ) )
			{
				int damage = Utility.RandomMinMax( level * 2, level * 3 );

				if ( !m.Player )
					damage *= 2;

				m.PlaySound( 0x133 );
				AOS.Damage( m, from, damage, 100, 0, 0, 0, 0 );

				Blood blood = new Blood();
				blood.ItemID = Utility.Random( 0x122A, 5 );
				blood.MoveToWorld( m.Location, m.Map );
				m.FixedParticles( 0x377A, 1, 15, 9502, 67, 7, EffectLayer.Waist );
			}
			else
			{
				EndBossBleed( m, false );
			}
		}

		public static void EndBossBleed( Mobile m, bool message )
		{
			Timer t = (Timer)m_BossBleedTable[m];
			if ( t == null )
				return;

			t.Stop();
			m_BossBleedTable.Remove( m );

			if ( message && m is PlayerMobile )
				m.SendMessage( "The bleeding has stopped." );
		}

		private class BossBleedTimer : Timer
		{
			private Mobile m_From;
			private Mobile m_Mobile;
			private int m_Count;
			private int m_MaxTicks;

			public BossBleedTimer( Mobile from, Mobile m, int maxTicks ) : base( TimeSpan.FromSeconds( 2.0 ), TimeSpan.FromSeconds( 2.0 ) )
			{
				m_From = from;
				m_Mobile = m;
				m_MaxTicks = maxTicks;
				Priority = TimerPriority.TwoFiftyMS;
			}

			protected override void OnTick()
			{
				DoBossBleed( m_Mobile, m_From, m_MaxTicks - m_Count );

				if ( ++m_Count == m_MaxTicks )
					EndBossBleed( m_Mobile, true );
			}
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
					song = "*releases spores that animate vines!*"; 
					break;
				case 1: 
					newSummons = Utility.RandomMinMax( 2, 3 ); 
					song = "*causes mushrooms to grow with a psychic surge!*"; 
					break;
				case 2: 
					newSummons = Utility.RandomMinMax( 1, 2 ); 
					song = "*a psychic surge brings forth creatures from the underdark!**"; 
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
					return new WhippingVine();
				case 1:
					return new Fungal();	
				case 2:
					if ( rand < 60 )
						return new FungalMage();
					else
						return new UmberHulk();
				default:
					return new Fungal();
			}
		}

		private Point3D GetSpawnLocation( Map map )
		{
			for ( int j = 0; j < 10; ++j )
			{
				int x = X + Utility.Random( 3 ) - 1;
				int y = Y + Utility.Random( 3 ) - 1;
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
			if ( Utility.RandomMinMax( 1, 2 ) == 1 )
			{
				int goo = 0;

				foreach ( Item splash in this.GetItemsInRange( 10 ) ){ if ( splash is MonsterSplatter && splash.Name == "fungal slime" ){ goo++; } }

				if ( goo == 0 )
				{
					MonsterSplatter.AddSplatter( this.X, this.Y, this.Z, this.Map, this.Location, this, "fungal slime", 0x4FC, 0 );
				}
			}
		}

		public override void OnGaveMeleeAttack( Mobile defender )
		{
			TrySummonCreature( defender );
		}

		public override bool OnBeforeDeath()
		{
			if ( m_Rage == 0 )
			{
				PublicOverheadMessage( MessageType.Regular, 0x21, false, "*releases a psychic shriek!*" );
				this.Hits = this.HitsMax / 2;
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
				PublicOverheadMessage( MessageType.Regular, 0x21, false, "*releases a crushing psychic scream!*" );
				this.Hits = this.HitsMax / 4;
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
				PublicOverheadMessage( MessageType.Regular, 0x21, false, "*withers into nothingness...*" );
			}
			
			return base.OnBeforeDeath();
		}

        public override void OnDelete()
        {
            CleanupSummons();
            base.OnDelete();
        }

        private void CleanupSummons()
        {
            for (int i = 0; i < m_Summons.Count; i++)
            {
                BaseCreature bc = m_Summons[i];

                if (bc != null && !bc.Deleted)
                    bc.Delete();
            }
            m_Summons.Clear();
        }

		public override void OnDeath( Container c )
		{
			base.OnDeath( c );

			if ( Utility.RandomDouble() < 0.03 )
			{
				c.DropItem( new EternalPowerScroll() );
			}

			int amt = Utility.RandomMinMax( 1, 3 );
			for ( int i = 0; i < amt; i++ )
			{
				c.DropItem( new EtherealPowerScroll() );
			}
			TitanRiches( m_LastTarget );
		}

		public static void TitanRiches( Mobile m )
		{
			if ( m == null || m.Map == null )
				return;

			Map map = m.Map;

			for ( int x = -12; x <= 12; ++x )
			{
				for ( int y = -12; y <= 12; ++y )
				{
					double dist = Math.Sqrt( x * x + y * y );

					if ( dist <= 12 )
						new GoodiesTimer( map, m.X + x, m.Y + y ).Start();
				}
			}
		}

		public class GoodiesTimer : Timer
		{
			private Map m_Map;
			private int m_X, m_Y;

			public GoodiesTimer( Map map, int x, int y ) : base( TimeSpan.FromSeconds( Utility.RandomDouble() * 5.0 ) )
			{
				m_Map = map;
				m_X = x;
				m_Y = y;
			}

			protected override void OnTick()
			{
				int z = m_Map.GetAverageZ( m_X, m_Y );
				bool canFit = m_Map.CanFit( m_X, m_Y, z, 6, false, false );

				for ( int i = -3; !canFit && i <= 3; ++i )
				{
					canFit = m_Map.CanFit( m_X, m_Y, z + i, 6, false, false );

					if ( canFit )
						z += i;
				}

				if ( !canFit )
					return;

				Item g = null;

				int r1 = (int)( Utility.RandomMinMax( 20, 40 ) * ( MyServerSettings.GetGoldCutRate() * .01 ) );
				int r2 = (int)( Utility.RandomMinMax( 50, 100 ) * ( MyServerSettings.GetGoldCutRate() * .01 ) );
				int r3 = (int)( Utility.RandomMinMax( 100, 200 ) * ( MyServerSettings.GetGoldCutRate() * .01 ) );
				int r4 = (int)( Utility.RandomMinMax( 200, 300 ) * ( MyServerSettings.GetGoldCutRate() * .01 ) );
				int r5 = (int)( Utility.RandomMinMax( 300, 400 ) * ( MyServerSettings.GetGoldCutRate() * .01 ) );

				switch ( Utility.Random( 21 ) )
				{
					case 0: g = new Crystals( r1 ); break;
					case 1: g = new DDGemstones( r2 ); break;
					case 2: g = new DDJewels( r2 ); break;
					case 3: g = new DDGoldNuggets( r3 ); break;
					case 4: g = new Gold( r3 ); break;
					case 5: g = new Gold( r3 ); break;
					case 6: g = new Gold( r3 ); break;
					case 7: g = new DDSilver( r4 ); break;
					case 8: g = new DDSilver( r4 ); break;
					case 9: g = new DDSilver( r4 ); break;
					case 10: g = new DDSilver( r4 ); break;
					case 11: g = new DDSilver( r4 ); break;
					case 12: g = new DDSilver( r4 ); break;
					case 13: g = new DDCopper( r5 ); break;
					case 14: g = new DDCopper( r5 ); break;
					case 15: g = new DDCopper( r5 ); break;
					case 16: g = new DDCopper( r5 ); break;
					case 17: g = new DDCopper( r5 ); break;
					case 18: g = new DDCopper( r5 ); break;
					case 19: g = new DDCopper( r5 ); break;
					case 20: g = new DDCopper( r5 ); break;
				}

				if ( g != null )
				{
					g.MoveToWorld( new Point3D( m_X, m_Y, z ), m_Map );

					if ( 0.5 >= Utility.RandomDouble() )
					{
						switch ( Utility.Random( 3 ) )
						{
							case 0: // Fire column
								Effects.SendLocationParticles( EffectItem.Create( g.Location, g.Map, EffectItem.DefaultDuration ), 0x3709, 10, 30, 5052 );
								Effects.PlaySound( g, g.Map, 0x208 );
								break;
							case 1: // Explosion
								Effects.SendLocationParticles( EffectItem.Create( g.Location, g.Map, EffectItem.DefaultDuration ), 0x36BD, 20, 10, 5044 );
								Effects.PlaySound( g, g.Map, 0x307 );
								break;
							case 2: // Ball of fire
								Effects.SendLocationParticles( EffectItem.Create( g.Location, g.Map, EffectItem.DefaultDuration ), 0x36FE, 10, 10, 5052 );
								break;
						}
					}
				}
			}
		}

		public override void OnAfterSpawn()
		{
			base.OnAfterSpawn();
			LeechImmune = true;
		}

		public SporeMother( Serial serial ) : base( serial )
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