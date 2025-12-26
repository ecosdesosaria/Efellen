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
using Server.Custom.BeholderSpecials;
using Server.Custom.DailyBosses.System;

namespace Server.Mobiles
{
	[CorpseName( "Dreamweaver's Corpse" )]
	public class Dreamweaver : BaseCreature
	{
		private const int MAX_SUMMONS_RAGE_0 = 12;
		private const int MAX_SUMMONS_RAGE_1 = 7;
		private const int MAX_SUMMONS_RAGE_2 = 4;
		private const int MAX_SUMMONS_RAGE_3 = 3;
		
		private const int SUMMON_RANGE = 12;
		
		private static readonly Type[] SummonTypes = new Type[] 
		{ 
			typeof(Gazer), 
			typeof(ElderGazer), 
			typeof(Beholder) 
		};

		private static readonly List<Type> BossDrops = new List<Type>
    	{
    	    typeof(Artifact_RobeOfTheDreamweaver),
    	    typeof(Artifact_CircletOfTheDreamweaver),
    	    typeof(Artifact_CapeOfTheDreamweaver),
    	    typeof(Artifact_BootsofTheDreamweaver),
			typeof(Artifact_TalonOfNightmares),
            typeof(Artifact_RingOfTheDreamweaver)
    	};

		private int m_Rage = 0;
		private Mobile m_LastTarget;
		private DateTime m_NextSummonTime = DateTime.MinValue;
		private DateTime m_NextSpecialAttack = DateTime.MinValue;
		private List<BaseCreature> m_Summons = new List<BaseCreature>();
        private DateTime m_NextSpecialBeholderAttack;

		[Constructable]
		public Dreamweaver () : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "Dreamweaver";

			Body = 674;
			BaseSoundID = 377;
			NameHue = 0x22;
			Hue = 0x96;
            Title = "The Shepherd of Filth";
			
			SetStr( 796, 885 );
			SetDex( 165, 225 );
			SetInt( 506, 605 );

			SetHits( 11000 );
			SetDamage( 23, 34 );

			SetDamageType( ResistanceType.Energy, 100 );
			SetResistance( ResistanceType.Physical, 50 );
			SetResistance( ResistanceType.Fire, 75 );
			SetResistance( ResistanceType.Cold, 70 );
			SetResistance( ResistanceType.Poison, 70 );
			SetResistance( ResistanceType.Energy, 75 );

			SetSkill( SkillName.Meditation, 102.5, 125.0 );
			SetSkill( SkillName.MagicResist, 125.5, 145.0 );
			SetSkill( SkillName.Tactics, 101.0, 120.0 );
			SetSkill( SkillName.FistFighting, 101.0, 111.0 );
			SetSkill( SkillName.Magery, 101.0, 120.0 );

			Fame = 30000;
			Karma = 30000;

			VirtualArmor = 60;

			PackItem( Loot.RandomArty() );
			PackItem( Loot.RandomArty() );
			PackItem( Loot.RandomArty() );
			PackItem( Loot.RandomArty() );
            m_NextSpecialBeholderAttack = DateTime.Now;
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.UltraRich, 6 );
		}

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
			Server.Misc.IntelligentAction.LeapToAttacker( this, from );
			
			if ( m_Rage >= 1 && DateTime.UtcNow >= m_NextSpecialAttack )
			{
				PerformRageAttack( from );
				m_NextSpecialAttack = DateTime.UtcNow + TimeSpan.FromSeconds( 30 - (m_Rage * 2) );
			}

            if ( DateTime.Now >= m_NextSpecialBeholderAttack && from != null && from.Alive && !willKill )
			{
				if ( Utility.RandomDouble() < 0.50 )
				{
					TriggerEyestalkAttack( from );
					m_NextSpecialBeholderAttack = DateTime.Now + TimeSpan.FromSeconds( 25 );
				}
			}
			
			base.OnDamage( amount, from, willKill );
		}

        private int getParalyzeDuration(Mobile m)
		{
			int resist = (int)(m.Skills.MagicResist.Value);
			// 2s at 125, 8s at 0 magic resist
			int duration = 8 - (int)(resist * (6.0 / 125.0));
			return duration;
		}

		private void PerformRageAttack( Mobile target )
		{
			if ( target == null || target.Deleted || !target.Alive )
				return;

			int availableAttacks = m_Rage;
			int attackChoice = Utility.RandomMinMax( 1, availableAttacks );

			switch ( attackChoice  )
			{
				case 1: // energy blast
				{
					BossSpecialAttack.PerformSlam(
                       boss: this,
                       warcry: "*Stares fiercely in all directions*",
                       hue: 5030,
                       rage: m_Rage,
                       range: 6,
                       energyDmg: 100
                   );
                   break;
				}
				case 2: // Rage 2: psychic blast (stamina drain + damage)
				{
					PublicOverheadMessage( MessageType.Regular, 0x21, false, "*Quivers with power*" );
					PlaySound( 0x228 );
					FixedParticles( 0x3789, 10, 25, 5032, EffectLayer.Head );
					IPooledEnumerable eable = GetMobilesInRange( 8 );
					foreach ( Mobile m in eable )
					{
						if ( m != this && m.Player && m.Alive && CanBeHarmful( m ) )
						{
							DoHarmful( m );
							int staminaDrain = Utility.RandomMinMax( 45, 65 );
							m.Stam -= staminaDrain;
							int damage = Utility.RandomMinMax( staminaDrain/2, staminaDrain*2 );
							AOS.Damage( m, this, damage, 0, 0, 0, 0, 100 );
							m.FixedParticles( 0x374A, 10, 15, 5013, 0x81b, 0, EffectLayer.Waist );
							m.PlaySound( 0x1FB );
							this.Stam = Math.Min( this.StamMax, this.Stam + staminaDrain / 3 );
							m.Paralyze( TimeSpan.FromSeconds( getParalyzeDuration( m ) + Utility.RandomMinMax(1,3 ) ) );
						}
					}
					SlamVisuals.SlamVisual(this, 6, 0x36B0, 0x25);
					eable.Free();
					break;
				}
                case 3: // telekinetic crush - move away + damage + paralyze
				{
                    if (this == null || this.Deleted)
        		        return;
        	        bool affectedAny = false;
        	        PublicOverheadMessage( MessageType.Regular, 0x21, false, "*Quivers with rage*" );

                    foreach (Mobile victim in this.GetMobilesInRange(9))
        	        {
        	        	if (victim == null || victim.Deleted || !victim.Alive || victim == this)
        	        		continue;
        	        	if (victim.Combatant != this && this.Combatant != victim)
        	        		continue;
        	        	if (!this.InLOS(victim))
        	        		continue;
        	        	int resist = (int)victim.Skills.MagicResist.Value;
        	        	int distance = 10 - (int)(resist * (2.0 / 125.0));
        	        	if (distance < 5)
        	        		distance = 5;
        	        	if (distance > 10)
        	        		distance = 10;
        	        	Direction d = this.GetDirectionTo(victim);
        	        	Point3D newLoc = victim.Location;
        	        	bool moved = false;
        	        	for (int i = 0; i < distance; i++)
        	        	{
        	        		int x = newLoc.X;
        	        		int y = newLoc.Y;
        	        		Movement.Movement.Offset(d, ref x, ref y);
        	        		Point3D testLoc = new Point3D(x, y, newLoc.Z);
        	        		if (victim.Map.CanSpawnMobile(testLoc))
        	        		{
        	        			newLoc = testLoc;
        	        			moved = true;
        	        		}
        	        		else
        	        		{
        	        			break;
        	        		}
        	        	}
        	        	if (moved && newLoc != victim.Location)
        	        	{
                            victim.MoveToWorld(newLoc, victim.Map);
        	        		victim.PlaySound(0x204);
                            DoHarmful( victim );
							int damage = Utility.RandomMinMax( 21, 32 );
							AOS.Damage( victim, this, damage, 0, 0, 0, 0, 100 );
							victim.PlaySound( 0x1FB );
							victim.Paralyze( TimeSpan.FromSeconds( (getParalyzeDuration( victim )*1.5) ) );
        	        		victim.FixedParticles(0x3728, 10, 10, 0x1F4, 0, 5029, 0);
                            BeholderSpecials.DoRayEffect(
                            	this,
                            	victim,
                            	0x36D4,
                            	1153,
                            	10
                            );
        	        		victim.SendMessage("A telekinetic force crushes you!");
        	        	}
        	        }
        	       break;
				}
			}
		}


		public override void CheckReflect( Mobile caster, ref bool reflect )
		{
			int chance = m_Rage * 16;
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
					song = "*Focuses its alien eyes*"; 
					break;
				case 1: 
					newSummons = Utility.RandomMinMax( 4, 8 ); 
					song = "*Intently focuses its alien eyes*"; 
					break;
				case 2: 
					newSummons = Utility.RandomMinMax( 3, 6 ); 
					song = "*Diabolically focuses its alien eyes*"; 
					break;
				case 3: 
					newSummons = Utility.RandomMinMax( 2, 4 );
					song = "*Stares maniacally into the void*"; 
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
					return new Gazer();
				case 1:
					if ( rand < 35 )
						return new ElderGazer();
					else
						return new Gazer();
				case 2:
					if ( rand < 35 )
						return new Beholder();
					else
						return new ElderGazer();
				case 3:
					return new Beholder();
				default:
					return new Gazer();
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

            if ( DateTime.Now >= m_NextSpecialAttack && defender != null && defender.Alive )
			{
				if ( Utility.RandomDouble() < 0.30 )
				{
					TriggerEyestalkAttack( defender );
					m_NextSpecialBeholderAttack = DateTime.Now + TimeSpan.FromSeconds( 30 );
				}
			}
		}

		public override bool OnBeforeDeath()
		{
			if ( m_Rage == 0 )
			{
				PublicOverheadMessage( MessageType.Regular, 0x21, false, "*Stares with boredom*" );
				this.Hits = this.HitsMax;
				this.FixedParticles( 0x376A, 9, 32, 5030, EffectLayer.Waist );
				this.PlaySound( 0x202 );
				SetStr( Str + 30 );
				SetDamage( 25, 30 );
				
				m_Rage = 1;
				return false;
			}
			else if ( m_Rage == 1 )
			{
				PublicOverheadMessage( MessageType.Regular, 0x21, false, "*Annoyedly pouts*" );
				this.Hits = this.HitsMax;
				this.FixedParticles( 0x376A, 9, 32, 5030, EffectLayer.Waist );
				this.PlaySound( 0x202 );
				
				SetStr( Str + 60 );
				SetDex( Dex + 10 );
				SetDamage( 29, 39 );
				VirtualArmor += 10;
				m_Rage = 2;
				return false;
			}
			else if ( m_Rage == 2 )
			{
				PublicOverheadMessage( MessageType.Regular, 0x21, false, "*Bites its tongue and spews black blood*" );
				this.Hits = this.HitsMax;
				this.FixedParticles( 0x376A, 9, 32, 5030, EffectLayer.Waist );
				this.PlaySound( 0x202 );
				
				SetStr( Str + 90 );
				SetDex( Dex + 40 );
				SetDamage( 36, 49 );
				VirtualArmor += 15;
				m_Rage = 3;
				return false;
			}
			else 
			{
				Effects.SendLocationParticles( EffectItem.Create( this.Location, this.Map, EffectItem.DefaultDuration ), 0x3728, 10, 10, 2023 );
				this.PlaySound( 0x1FE );
				PublicOverheadMessage( MessageType.Regular, 0x21, false, "*Stares in disbelief*" );
				Mobile killer = this.LastKiller;
				if (killer != null && killer.Player && killer.Karma > 0)
            	{
            	    int marks = Utility.RandomMinMax(21, 47);
            	    Server.Custom.DefenderOfTheRealm.MarkLootHelper.AwardMarks(killer, 1, marks);
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
			RichesSystem.SpawnRiches( m_LastTarget, 4 );
		}

        private void TriggerEyestalkAttack( Mobile target )
		{
			int choice = Utility.Random( 6 );

			switch ( choice )
			{
				case 0:
				{
					if ( BeholderSpecials.AntiMagicEye( this, 80, 45, target ) )
					{
						this.Say( "*Focuses its anti-magic eye on {0}*", target.Name );
					}
					break;
				}
				case 1:
				{
					if ( BeholderSpecials.Disintegration( this, 100, 90, target ) )
					{
						this.Say( "*Fires a disintegration ray at {0}*", target.Name );
					}
					break;
				}
				case 2:
				{
					if ( BeholderSpecials.Petrification( this, 30, target ) )
					{
						this.Say( "*Petrifies {0} with its gaze*", target.Name );
					}
					break;
				}
				case 3:
				{
					if ( BeholderSpecials.Fear( this, 60, target ) )
					{
						this.Say( "*Strikes fear into {0}*", target.Name );
					}
					break;
				}
				case 4:
				{
					if ( BeholderSpecials.TelekineticRay( this, 9, 40 ) )
					{
						this.Say( "*A wave of Telekinetic energy oozes from an eyestalk!*", target.Name );
					}
					break;
				}
				case 5:
				{
					if ( BeholderSpecials.DeathRay( this, target, 31, 10, 90 ) )
					{
						this.Say( "*Fires a necrotic ray at {0}*", target.Name );
					}
					break;
				}
			}
		}

		public override void OnAfterSpawn()
		{
			base.OnAfterSpawn();
			LeechImmune = true;
		}

         public override int GetDeathSound()
        {
            return 0x56F;
        }
 
        public override int GetAttackSound()
        {
            return 0x570;
        }
 
        public override int GetIdleSound()
        {
            return 0x571;
        }
 
        public override int GetAngerSound()
        {
            return 0x572;
        }
 
        public override int GetHurtSound()
        {
            return 0x573;
        }

		public Dreamweaver( Serial serial ) : base( serial )
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