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
	[CorpseName( "Heavenly Marshall's Corpse" )]
	public class HeavenlyMarshall : BaseCreature
	{
		private const int MAX_SUMMONS_RAGE_0 = 16;
		private const int MAX_SUMMONS_RAGE_1 = 14;
		private const int MAX_SUMMONS_RAGE_2 = 12;
		private const int MAX_SUMMONS_RAGE_3 = 8;
		
		private const int SUMMON_RANGE = 12;
		
		private static readonly Type[] SummonTypes = new Type[] 
		{ 
			typeof(GriffonRiding), 
			typeof(WarGriffon), 
			typeof(Archangel), 
			typeof(Angel), 
			typeof(EtherealWarriorGeneral) 
		};

		private static readonly List<Type> BossDrops = new List<Type>
    	{
    	    typeof(Artifact_GauntletsOfDevotion),
    	    typeof(Artifact_LeggingsOfDevotion),
    	    typeof(Artifact_TunicOfDevotion),
    	    typeof(Artifact_ArmsOfDevotion),
			typeof(Artifact_CoifOfDevotion),
    	};

		private int m_Rage = 0;
		private Mobile m_LastTarget;
		private DateTime m_NextSummonTime = DateTime.MinValue;
		private DateTime m_NextSpecialAttack = DateTime.MinValue;
        private List<BaseCreature> m_Summons = new List<BaseCreature>();

		[Constructable]
		public HeavenlyMarshall () : base( AIType.AI_Mage, FightMode.Evil, 10, 1, 0.2, 0.4 )
		{
			Name = "Heavenly Marshall";

			Body = 346;
			BaseSoundID = 466;
			NameHue = 0x92E;
			Hue = 0x0672;

			SetStr( 796, 885 );
			SetDex( 125, 175 );
			SetInt( 586, 675 );

			SetHits( 19000 );
			SetDamage( 23, 34 );

			SetDamageType( ResistanceType.Energy, 100 );
			SetResistance( ResistanceType.Physical, 60 );
			SetResistance( ResistanceType.Fire, 70 );
			SetResistance( ResistanceType.Cold, 70 );
			SetResistance( ResistanceType.Poison, 70 );
			SetResistance( ResistanceType.Energy, 75 );

			SetSkill( SkillName.Anatomy, 55.1, 75.0 );
			SetSkill( SkillName.Psychology, 90.1, 125.0 );
			SetSkill( SkillName.Meditation, 112.5, 125.0 );
			SetSkill( SkillName.MagicResist, 125.5, 150.0 );
			SetSkill( SkillName.Tactics, 101.0, 125.0 );
			SetSkill( SkillName.FistFighting, 101.0, 125.0 );
			
			Fame = 35000;
			Karma = -35000;

			VirtualArmor = 60;

			PackItem( Loot.RandomArty() );
			PackItem( Loot.RandomArty() );
			PackItem( Loot.RandomArty() );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.UltraRich, 8 );
		}

		public override int TreasureMapLevel{ get{ return 5; } }
		public override int Skeletal{ get{ return 50; } }
		public override SkeletalType SkeletalType{ get{ return SkeletalType.Mystical; } }
		public override int Cloths{ get{ return Utility.Random(50); } }
		public override ClothType ClothType{ get{ return ClothType.Divine; } }
		public override bool CanRummageCorpses{ get{ return false; } }
		public override bool ReacquireOnMovement{ get{ return !Controlled; } }
		public override bool BleedImmune{ get{ return true; } }
		public override bool BardImmune { get { return true; } }
		public override bool Unprovokable { get { return true; } }
		public override Poison PoisonImmune{ get{ return Poison.Greater; } }
		public override bool AlwaysAttackable{ get{ return true; } }
		public override bool AlwaysMurderer { get { return false; } }

        public override bool IsEnemy( Mobile m )
	    {
			if (m == null || m.Deleted)
	        	return false;
			
			if (m is HeavenlyMarshall || m is SkyKnight || m is GriffonRiding || m is WarGriffon || m is EtherealWarriorGeneral)
		    	return false;
			
			if ( !IntelligentAction.GetMyEnemies( m, this, true ) )
				return false;
			
			if ( m.Region != this.Region )
				return false;
			
			if (m is BaseCreature && ((BaseCreature)m).ControlMaster == null )
			{
				this.Location = m.Location;
				this.Combatant = m;
				this.Warmode = true;
			}
			return true;
	    }

		public override void AggressiveAction(Mobile m, bool criminal)
		{
			if (m is HeavenlyMarshall || m is SkyKnight || m is GriffonRiding || m is WarGriffon || m is EtherealWarriorGeneral)
				return;

		    base.AggressiveAction(m, true);
		}

		public override bool CanBeHarmful(Mobile m, bool message, bool ignoreOurBlessedness)
		{
		    if (m is HeavenlyMarshall || m is SkyKnight || m is GriffonRiding || m is WarGriffon || m is EtherealWarriorGeneral)
		        return false;

		    return base.CanBeHarmful(m, message, ignoreOurBlessedness);
		}

		public override bool CanBeBeneficial(Mobile m, bool message, bool allowDead)
		{
		     if (m is HeavenlyMarshall || m is SkyKnight || m is GriffonRiding || m is WarGriffon || m is EtherealWarriorGeneral)
		        return true;

		    return base.CanBeBeneficial(m, message, allowDead);
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
		
			if (from.Player && from.Kills < 5 && !from.Criminal) 
				from.Criminal = true;		
		
			base.OnDamage( amount, from, willKill );
		}


		private void PerformRageAttack( Mobile target )
		{
			if ( target == null || target.Deleted || !target.Alive )
				return;

			int availableAttacks = m_Rage;
			int attackChoice = Utility.RandomMinMax( 1, availableAttacks );
            Map map = this.Map;

			switch ( attackChoice  )
			{
				case 1: // holy nova
				{
					PublicOverheadMessage( MessageType.Regular, 0x21, false, "Heavens smite thee!" );
					PlaySound( 0x64F );
					FixedParticles( 0x376A, 9, 32, 5030, EffectLayer.Waist );
					IPooledEnumerable eable = GetMobilesInRange( 6 );
					foreach ( Mobile m in eable )
					{
						if ( m != this && m.Player && m.Alive && CanBeHarmful( m ) )
						{
							DoHarmful( m );
							int damage = Utility.RandomMinMax( 35, 49 );
							AOS.Damage( m, this, damage, 0, 0, 0, 0, 100 );
							m.PlaySound( 0x1FB );
						}
					}
					SlamVisuals.SlamVisual(this, 6, 0x36B0, 0x4D5);
					eable.Free();
					break;
				}

				case 2: //holy fire
				{
					if (map == null)
                        return;
                    int range = 7;
                    this.PlaySound(0x208);
                    PublicOverheadMessage(Network.MessageType.Emote, 0x22, false, "Burn in the light!");
                    Point3D start = this.Location;
                    int dx = 0, dy = 0;
                    GetDirectionVector(this.Direction, out dx, out dy);
                    for (int i = 1; i <= range; i++)
                    {
                        Point3D p = new Point3D(start.X + dx * i, start.Y + dy * i, start.Z);
                        Effects.SendLocationEffect(p, map, 0x36D4, 20, 10, 0xb73, 0);
                        foreach (Mobile m in map.GetMobilesInRange(p, 0))
                        {
                            if (m != null && m != this && !m.Deleted && CanBeHarmful(m))
                            {
                                DoHarmful(m);
                                m.Damage(Utility.RandomMinMax(25, 45), this);
                                m.PlaySound(0x15E);
                            }
                        }
                    }
					break;
				}
				
				case 3: // Rage 3: holy blast (Mana drain + damage)
				{
					PublicOverheadMessage( MessageType.Regular, 0x21, false, "Light everlasting shall consume you!" );
					PlaySound( 0x228 );
					FixedParticles( 0x3789, 10, 25, 5032, EffectLayer.Head );
					IPooledEnumerable eable = GetMobilesInRange( 8 );
					foreach ( Mobile m in eable )
					{
						if ( m != this && m.Player && m.Alive && CanBeHarmful( m ) )
						{
							DoHarmful( m );
							int manaDrain = Utility.RandomMinMax( 35, 45 );
							m.Mana -= manaDrain;
							int damage = Utility.RandomMinMax( manaDrain/2, manaDrain*2 );
							AOS.Damage( m, this, damage, 0, 0, 0, 0, 100 );
							m.FixedParticles( 0x374A, 10, 15, 5013, 0x497, 0, EffectLayer.Waist );
							m.PlaySound( 0x1FB );
							this.Mana = Math.Min( this.ManaMax, this.Mana + manaDrain / 3 );
						}
					}
					SlamVisuals.SlamVisual(this, 6, 0x36B0, 0x497);
					eable.Free();
					break;
				}
			}
		}

		private void GetDirectionVector(Direction d, out int dx, out int dy)
        {
            dx = 0;
            dy = 0;

            switch (d)
            {
                case Direction.North:
                    dy = -1;
                    break;
                case Direction.Right:
                    dx = 1;
                    dy = -1;
                    break;
                case Direction.East:
                    dx = 1;
                    break;
                case Direction.Down:
                    dx = 1;
                    dy = 1;
                    break;
                case Direction.South:
                    dy = 1;
                    break;
                case Direction.Left:
                    dx = -1;
                    dy = 1;
                    break;
                case Direction.West:
                    dx = -1;
                    break;
                case Direction.Up:
                    dx = -1;
                    dy = -1;
                    break;
            }
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
					song = "Come forth, comrades!"; 
					break;
				case 1: 
					newSummons = Utility.RandomMinMax( 4, 8 ); 
					song = "Lets end this menace right now!"; 
					break;
				case 2: 
					newSummons = Utility.RandomMinMax( 3, 6 ); 
					song = "We shall stand against injustice!"; 
					break;
				case 3: 
					newSummons = Utility.RandomMinMax( 2, 4 );
					song = "Hosts of heaven, answer my call!"; 
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
					return new GriffonRiding();
				case 1:
					if ( rand < 45 )
						return new GriffonRiding();
					else
						return new WarGriffon();
				case 2:
					if ( rand < 10 )
						return new Angel();
					else if ( rand < 25 )
						return new EtherealWarrior();
					else
						return new WarGriffon();

				case 3:
					if ( rand < 20 )
						return new Archangel();
					else if ( rand < 45 )
						return new Angel();
					else
						return new EtherealWarriorGeneral();

				default:
					return new GriffonRiding();
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
		}

		public override void OnGaveMeleeAttack( Mobile defender )
		{
			TrySummonCreature( defender );
		}

		public override bool OnBeforeDeath()
		{
        	if ( m_Rage == 0 )
			{
				PublicOverheadMessage( MessageType.Regular, 0x21, false, "Justice shall not falther today!" );
				this.Hits = this.HitsMax;
				this.FixedParticles( 0x376A, 9, 32, 5030, EffectLayer.Waist );
				this.PlaySound( 0x202 );
				
				SetStr( Str + 40 );
				SetDamage( 28, 34 );
				
				m_Rage = 1;
				return false;
			}
			else if ( m_Rage == 1 )
			{
				PublicOverheadMessage( MessageType.Regular, 0x21, false, "By the heavens above I command thee to stand down!" );
				this.Hits = this.HitsMax;
				this.FixedParticles( 0x376A, 9, 32, 5030, EffectLayer.Waist );
				this.PlaySound( 0x202 );
				
				SetStr( Str + 80 );
				SetDex( Dex + 15 );
				SetDamage( 33, 44 );
				VirtualArmor += 10;
				
				m_Rage = 2;
				return false;
			}
			else if ( m_Rage == 2 )
			{
				PublicOverheadMessage( MessageType.Regular, 0x21, false, "For the Skywatch!" );
				this.Hits = this.HitsMax;
				this.FixedParticles( 0x376A, 9, 32, 5030, EffectLayer.Waist );
				this.PlaySound( 0x202 );
				
				SetStr( Str + 125 );
				SetDex( Dex + 50 );
				SetDamage( 40, 55 );
				VirtualArmor += 15;	
				m_Rage = 3;
				return false;
			}
			else 
			{
				Effects.SendLocationParticles( EffectItem.Create( this.Location, this.Map, EffectItem.DefaultDuration ), 0x3728, 10, 10, 2023 );
				this.PlaySound( 0x1FE );
				PublicOverheadMessage( MessageType.Regular, 0x21, false, "I return...to the skies..." );
				Mobile killer = this.LastKiller;

            	if (killer != null && killer.Player && killer.Karma < 0)
            	{
            	    int marks = Utility.RandomMinMax(31, 47);
            	    Server.Custom.DefenderOfTheRealm.MarkLootHelper.AwardMarks(killer, 0, marks);
            	}
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

		public override void OnAfterSpawn()
		{
			base.OnAfterSpawn();
			LeechImmune = true;
		}

		public HeavenlyMarshall( Serial serial ) : base( serial )
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