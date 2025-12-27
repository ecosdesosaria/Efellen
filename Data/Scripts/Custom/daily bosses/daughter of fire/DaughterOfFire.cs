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

namespace Server.Mobiles
{
	[CorpseName( "Daughter of Fire's Corpse" )]
	public class DaughterOfFire : BaseCreature
	{
		private const int MAX_SUMMONS_RAGE_0 = 6;
		private const int MAX_SUMMONS_RAGE_1 = 4;
		private const int MAX_SUMMONS_RAGE_2 = 2;
		private const int MAX_SUMMONS_RAGE_3 = 2;
		
		private const int SUMMON_RANGE = 10;
		
		private static readonly Type[] SummonTypes = new Type[] 
		{ 
			typeof(Penitent), 
			typeof(FireGargoyle), 
			typeof(Succubus), 
			typeof(FireElemental), 
			typeof(Efreet) 
		};
		
		private static readonly List<Type> BossDrops = new List<Type>
    	{
    	    typeof(Artifact_GlovesOfThePainSlave),
    	    typeof(Artifact_LegsOfThePainSlave),
    	    typeof(Artifact_ArmsOfThePainSlave),
    	    typeof(Artifact_BootsOfThePainSlave),
			typeof(Artifact_ChestOfThePainSlave),
			typeof(Artifact_ExquisiteAgony)
    	};

		private int m_Rage = 0;
		private Mobile m_LastTarget;
		private DateTime m_NextSummonTime = DateTime.MinValue;
		private DateTime m_NextSpecialAttack = DateTime.MinValue;
		private List<BaseCreature> m_Summons = new List<BaseCreature>();

		[Constructable]
		public DaughterOfFire () : base( AIType.AI_Mage, FightMode.Closest, 20, 1, 0.4, 0.8 )
		{
			Name = "Daughter of Fire";
            Title = "The Emissary of Pain";
			Body = 149;
			BaseSoundID = 0x4B0;
			NameHue = 0x22;
			Hue = 0xb73;

			SetStr( 696, 685 );
			SetDex( 185, 205 );
			SetInt( 486, 475 );

			SetHits( 7000 );
			SetDamage( 23, 28 );

			SetDamageType( ResistanceType.Fire, 100 );
			SetResistance( ResistanceType.Physical, 50 );
			SetResistance( ResistanceType.Fire, 75 );
			SetResistance( ResistanceType.Cold, 70 );
			SetResistance( ResistanceType.Poison, 70 );
			SetResistance( ResistanceType.Energy, 70 );

			SetSkill( SkillName.Magery, 100.0 );
			SetSkill( SkillName.Meditation, 115.0 );
			SetSkill( SkillName.MagicResist, 115.5, 135.0 );
			SetSkill( SkillName.Tactics, 111.0 );
			SetSkill( SkillName.FistFighting, 111.0 );
			SetSkill( SkillName.Spiritualism, 116.0);
			SetSkill( SkillName.Necromancy, 116.0);

			Fame = 25000;
			Karma = -25000;

			VirtualArmor = 50;

			PackItem( Loot.RandomArty() );
			PackItem( Loot.RandomArty() );
			PackItem( Loot.RandomArty() );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.UltraRich, 4 );
		}

		public override bool AutoDispel{ get{ return !Controlled; } }
		public override int TreasureMapLevel{ get{ return 4; } }
		public override int Hides{ get{ return 38; } }
		public override HideType HideType{ get{ return HideType.Hellish; } }
		public override int Skin{ get{ return Utility.Random(9); } }
		public override SkinType SkinType{ get{ return SkinType.Lava; } }
		public override int Skeletal{ get{ return Utility.Random(9); } }
		public override SkeletalType SkeletalType{ get{ return SkeletalType.Devil; } }
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
            Map map = this.Map;

			switch ( attackChoice  )
			{
				case 1: // Flaming blast
				{
					BossSpecialAttack.PerformSlam(
                       boss: this,
                       warcry: "BURN!",
                       hue: 0xb73,
                       rage: m_Rage,
                       range: 6,
                       fireDmg: 100
                   );
                   break;
				}
				case 2: // rage 2: summon fire vortex
				{
					BossSpecialAttack.SummonHonorGuard(
                        boss: this,
                        target: target,
                        warcry: "Hellfire shall consume you!",
                        amount: 2,
                        creatureType: typeof(EvilScorchingVortex),
                        hue: 0xb73
                    );
                    break;
				}
				case 3: // Rage 3: fire cone
				{
					if (map == null)
                        return;
                    int range = 7;
                    this.PlaySound(0x208);
                    PublicOverheadMessage(Network.MessageType.Emote, 0x22, false, "*lashes a blazing whip of fire!*");
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
			int chance = m_Rage * 11;
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
					song = "Fire walks with me!"; 
					break;
				case 1: 
					newSummons = Utility.RandomMinMax( 4, 8 ); 
					song = "Soon you shall be ash and memory!"; 
					break;
				case 2: 
					newSummons = Utility.RandomMinMax( 3, 6 ); 
					song = "We will turn your hopes and dreams into cinder!"; 
					break;
				case 3: 
					newSummons = Utility.RandomMinMax( 2, 4 );
					song = "Hell is unleashed!"; 
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

			m_NextSummonTime = DateTime.UtcNow + TimeSpan.FromSeconds( 24.0 - (m_Rage * 0.5) );
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
					return new Penitent();
					
				case 1:
					if ( rand < 55 )
						return new FireGargoyle();
					else
						return new Penitent();
				case 2:
					if ( rand < 55 )
						return new FireElemental();
					else
						return new Efreet();
				case 3:
					if ( rand < 20 )
						return new Succubus();
					else if ( rand < 45 )
						return new FireElemental();
					else
						return new Efreet();
				default:
					return new Penitent();
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
				PublicOverheadMessage( MessageType.Regular, 0x21, false, "We are just getting started, honey!" );
				this.Hits = this.HitsMax;
				this.FixedParticles( 0x376A, 9, 32, 5030, EffectLayer.Waist );
				this.PlaySound( 0x202 );
				
				SetStr( Str + 100 );
				SetDex( Dex + 25 );
				SetDamage( 28, 33 );
				
				m_Rage = 1;
				return false;
			}
			else if ( m_Rage == 1 )
			{
				PublicOverheadMessage( MessageType.Regular, 0x21, false, "Feeling the heat yet?" );
				this.Hits = this.HitsMax;
				this.FixedParticles( 0x376A, 9, 32, 5030, EffectLayer.Waist );
				this.PlaySound( 0x202 );
				
				SetStr( Str + 150 );
				SetDex( Dex + 35 );
				SetDamage( 33, 38 );
				VirtualArmor += 10;
				
				m_Rage = 2;
				return false;
			}
			else if ( m_Rage == 2 )
			{
				PublicOverheadMessage( MessageType.Regular, 0x21, false, "You are starting to burn me down!" );
				this.Hits = this.HitsMax;
				this.FixedParticles( 0x376A, 9, 32, 5030, EffectLayer.Waist );
				this.PlaySound( 0x202 );
				
				SetStr( Str + 200 );
				SetDex( Dex + 50 );
				SetDamage( 38, 43 );
				VirtualArmor += 15;
				
				PublicOverheadMessage( MessageType.Regular, 0x21, false, "Fool! I'm fire everlasting" );
				BaseCreature balron = new Balron();
				balron.Team = this.Team;
				Point3D loc = GetSpawnLocation( this.Map );
				balron.IsTempEnemy = true;
				balron.MoveToWorld( loc, this.Map );
				if ( Combatant != null )
					balron.Combatant = Combatant;
				
				m_Rage = 3;
				return false;
			}
			else 
			{
				Effects.SendLocationParticles( EffectItem.Create( this.Location, this.Map, EffectItem.DefaultDuration ), 0x3728, 10, 10, 2023 );
				this.PlaySound( 0x1FE );
				PublicOverheadMessage( MessageType.Regular, 0x21, false, "You win...this time..." );
				Mobile killer = this.LastKiller;
				if (killer != null && killer.Player && killer.Karma > 0)
            	{
            	    int marks = Utility.RandomMinMax(81, 137);
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

			int amt = Utility.RandomMinMax( 3, 6 );
			for ( int i = 0; i < amt; i++ )
			{
				c.DropItem( new EtherealPowerScroll() );
			}

			// gold explosion
		    RichesSystem.SpawnRiches( m_LastTarget, 3 );
		}

		public override void OnAfterSpawn()
		{
			base.OnAfterSpawn();
			LeechImmune = true;
		}

		public DaughterOfFire( Serial serial ) : base( serial )
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