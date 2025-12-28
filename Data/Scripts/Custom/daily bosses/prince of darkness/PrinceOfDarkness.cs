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
	[CorpseName( "Prince of Darkness Corpse" )]
	public class PrinceOfDarkness : BaseCreature
	{	
		private static readonly Type[] SummonTypes = new Type[] 
		{ 
			typeof(MetalHead), 
			typeof(Ozzy_WereWolf), 
			typeof(Demon), 
			typeof(Daemon), 
			typeof(Balron) 
		};

		private static readonly string[] SummonWarcries = new string[]
		{
			"Sabbath bloody sabbath!",
			"All aboard! HahaHAha!",
			"Bark at the moon!",
			"Generals gathered in their masses!"
		};

		private static readonly List<Type> BossDrops = new List<Type>
    	{
    	    typeof(Artifact_ArmsOfTheBlackSabbath),
    	    typeof(Artifact_HelmOfTheBlackSabbath),
    	    typeof(Artifact_LeggingsOfTheBlackSabbath),
    	    typeof(Artifact_GlovesOfTheBlackSabbath),
			typeof(Artifact_CoatOfTheBlackSabbath),
    	};

		private int m_Rage = 0;
		private Mobile m_LastTarget;
		private DateTime m_NextSummonTime = DateTime.MinValue;
		private DateTime m_NextSpecialAttack = DateTime.MinValue;
		private List<BaseCreature> m_Summons = new List<BaseCreature>();

		[Constructable]
		public PrinceOfDarkness () : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "Prince of Darkness";
			Title = "The Lord of This World";
			Body = 0x58;
			BaseSoundID = 838;
			NameHue = 0x22;
			Hue = 0x497;
			
			SetStr( 896, 985 );
			SetDex( 125, 175 );
			SetInt( 586, 675 );

			SetHits( 19000 );
			SetDamage( 23, 34 );

			SetDamageType( ResistanceType.Physical, 100 );
			SetResistance( ResistanceType.Physical, 60 );
			SetResistance( ResistanceType.Fire, 75 );
			SetResistance( ResistanceType.Cold, 70 );
			SetResistance( ResistanceType.Poison, 70 );
			SetResistance( ResistanceType.Energy, 70 );

			SetSkill( SkillName.Meditation, 112.5, 125.0 );
			SetSkill( SkillName.MagicResist, 125.5, 150.0 );
			SetSkill( SkillName.Tactics, 101.0, 125.0 );
			SetSkill( SkillName.FistFighting, 101.0, 125.0 );
			SetSkill( SkillName.Spiritualism, 125.0, 125.0);
			SetSkill( SkillName.Necromancy, 125.0, 125.0);
			SetSkill( SkillName.Magery, 101.0, 120.0 );

			Fame = 35000;
			Karma = -35000;

			VirtualArmor = 60;

			PackItem( Loot.RandomArty() );
			PackItem( Loot.RandomArty() );
			PackItem( Loot.RandomArty() );
			PackItem( Loot.RandomArty() );
			PackItem( Loot.RandomArty() );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.UltraRich, 8 );
		}

		public override int TreasureMapLevel{ get{ return 5; } }
		public override int Hides{ get{ return 38; } }
		public override HideType HideType{ get{ return HideType.Hellish; } }
		public override int Skin{ get{ return 50; } }
		public override SkinType SkinType{ get{ return SkinType.Demon; } }
		public override int Skeletal{ get{ return 50; } }
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
				m_NextSpecialAttack = DateTime.UtcNow + TimeSpan.FromSeconds( 15 - (m_Rage * 2) );
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
				case 1: // Freezing blast
				{
					BossSpecialAttack.PerformSlam(
                    boss: this,
                    warcry: "Feel the blizzard of Ozz!",
                    hue: 0x25,
                    rage: m_Rage,
                    range: 6,
					physicalDmg:0,
                    coldDmg: 100
                );
                break;
				}

				case 2: // Rage 2+: Laceration (bleed+paralyze)
				{
					BossSpecialAttack.PerformEntangle(
    			    boss: this,
    			    warcry: "Bleed for me!",
    			    hue: 0x25,
    			    rage: m_Rage,
    			    range: 6,
    			    bleedLevel: 8  // 16-24 damage per tick
    			);
    			break;
				}

				case 3: // Rage 3: Void blast
				{
					BossSpecialAttack.PerformCrossExplosion(
				    boss: this,
				    target: target,
				    warcry: "The sun, the moon and the stars all bear my seal!",
				    hue: 0x25,
				    rage: m_Rage,
				    coldDmg: 20,
				    fireDmg: 20,
				    energyDmg: 20,
				    poisonDmg: 20,
				    physicalDmg: 20
				);
				break;
				}
			}
		}

		public override void CheckReflect( Mobile caster, ref bool reflect )
		{
			int chance = m_Rage * 22;
			reflect = ( Utility.Random(100) < chance );
		}

		private int GetMaxSummons()
		{
			switch( m_Rage )
			{
				case 0: return 12;
				case 1: return 10;
				case 2: return 8;
				case 3: return 6;
				default: return 12;
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

		public override void OnGotMeleeAttack( Mobile attacker )
		{
			BossSummonSystem.TrySummonCreature(
				this,//boss
				attacker,//target
				SummonTypes,//creature list
				m_Rage,// current rage
				ref m_NextSummonTime,//next available summon
				SummonWarcries,//warcries per rage
				m_Summons,//current active summons
				0x25,// effect hue
				GetMaxSummons(),//summon limit
				40// cooldown
			);
		}

		public override void OnGaveMeleeAttack( Mobile defender )
		{
			BossSummonSystem.TrySummonCreature(
				this,//boss
				defender,//target
				SummonTypes,//creature list
				m_Rage,// current rage
				ref m_NextSummonTime,//next available summon
				SummonWarcries,//warcries per rage
				m_Summons,//current active summons
				0x25,// effect hue
				GetMaxSummons(),//summon limit
				40// cooldown
			);
		}

		public override bool OnBeforeDeath()
		{
			if ( m_Rage == 0 )
			{
				PublicOverheadMessage( MessageType.Regular, 0x21, false, "No more tears!" );
				this.Hits = this.HitsMax;
				this.FixedParticles( 0x376A, 9, 32, 5030, EffectLayer.Waist );
				this.PlaySound( 0x202 );
				
				SetStr( Str + 30 );
				SetDamage( 28, 34 );
				
				m_Rage = 1;
				return false;
			}
			else if ( m_Rage == 1 )
			{
				PublicOverheadMessage( MessageType.Regular, 0x21, false, "No more tears!" );
				this.Hits = this.HitsMax;
				this.FixedParticles( 0x376A, 9, 32, 5030, EffectLayer.Waist );
				this.PlaySound( 0x202 );
				
				SetStr( Str + 60 );
				SetDex( Dex + 25 );
				SetDamage( 33, 39 );
				VirtualArmor += 10;
				
				m_Rage = 2;
				return false;
			}
			else if ( m_Rage == 2 )
			{
				PublicOverheadMessage( MessageType.Regular, 0x21, false, "No more tears!" );
				this.Hits = this.HitsMax;
				this.FixedParticles( 0x376A, 9, 32, 5030, EffectLayer.Waist );
				this.PlaySound( 0x202 );
				
				SetStr( Str + 120 );
				SetDex( Dex + 50 );
				SetDamage( 38, 45 );
				VirtualArmor += 10;
				
				PublicOverheadMessage( MessageType.Regular, 0x21, false, "SHAAAAROOON!" );
				BaseCreature sharon = new Sharon();
				sharon.Team = this.Team;
				Point3D loc = GetSpawnLocation( this.Map );
				sharon.IsTempEnemy = true;
				sharon.MoveToWorld( loc, this.Map );
				if ( Combatant != null )
					sharon.Combatant = Combatant;
				
				m_Rage = 3;
				return false;
			}
			else 
			{
				Effects.SendLocationParticles( EffectItem.Create( this.Location, this.Map, EffectItem.DefaultDuration ), 0x3728, 10, 10, 2023 );
				this.PlaySound( 0x1FE );
				PublicOverheadMessage( MessageType.Regular, 0x21, false, "Mama...I'm coming home..." );
				Mobile killer = this.LastKiller;
				if (killer != null && killer.Player && killer.Karma > 0)
            	{
            	    int marks = Utility.RandomMinMax(231, 347);
            	    Server.Custom.DefenderOfTheRealm.MarkLootHelper.AwardMarks(killer, 1, marks);
            	}
			}
			
			return base.OnBeforeDeath();
		}

		public override void OnDelete()
        {
            BossSummonSystem.CleanupSummons(m_Summons);
            base.OnDelete();
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

		public PrinceOfDarkness( Serial serial ) : base( serial )
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
			// Initialize summons list if null
			if (m_Summons == null)
				m_Summons = new List<BaseCreature>();
		}
	}
}