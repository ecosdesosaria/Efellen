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
using Server.Custom.DailyBosses.System;
using Server.Custom.BossSystems;

namespace Server.Mobiles
{
	[CorpseName( "Spore Mother's Corpse" )]
	public class SporeMother : BaseCreature
	{
		private static readonly Type[] SummonTypes = new Type[] 
		{ 
			typeof(WhippingVine), 
			typeof(Fungal), 
			typeof(FungalMage), 
			typeof(UmberHulk),
			typeof(WeedElemental)
		};

		private static readonly string[] SummonWarcries = new string[]
		{
			"*Releases spores that animate vines!*",
			"*Causes mushrooms to grow with a psychic surge!*",
			"*A psychic surge brings forth creatures from the underdark!*",
			"*Weeds rise and form into new monstrosities!*"
		};

		private static readonly List<Type> BossDrops = new List<Type>
    	{
    	    typeof(Artifact_MyconidChestplate),
    	    typeof(Artifact_MyconidHelmet),
    	    typeof(Artifact_MyconidGloves),
    	    typeof(Artifact_MyconidLeggings),
			typeof(Artifact_MyconidArms),
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
			Title = "The Living Infestation";
			Body = 341;
			NameHue = 0x22;
			Hue = 0x497;

			SetStr( 496, 585 );
			SetDex( 155, 185 );
			SetInt( 286, 375 );

			SetHits( 3000 );
			SetDamage( 13, 24 );

			SetDamageType( ResistanceType.Poison, 100 );
			SetResistance( ResistanceType.Physical, 40 );
			SetResistance( ResistanceType.Fire, 45 );
			SetResistance( ResistanceType.Cold, 40 );
			SetResistance( ResistanceType.Poison, 70 );
			SetResistance( ResistanceType.Energy, 50 );

			SetSkill( SkillName.Magery, 82.5, 125.0 );
			SetSkill( SkillName.Psychology, 52.5, 85.0 );
			SetSkill( SkillName.Meditation, 82.5, 95.0 );
			SetSkill( SkillName.MagicResist, 75.5, 125.0 );
			SetSkill( SkillName.Tactics, 81.0, 95.0 );
			SetSkill( SkillName.FistFighting, 101.0, 115.0 );

			Fame = 15000;
			Karma = -15000;

			VirtualArmor = 30;

			PackItem( Loot.RandomArty() );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.UltraRich, 2 );
		}

		public override int TreasureMapLevel{ get{ return 3; } }
		public override bool CanRummageCorpses{ get{ return false; } }
		public override bool ReacquireOnMovement{ get{ return !Controlled; } }
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
				m_NextSpecialAttack = DateTime.UtcNow + TimeSpan.FromSeconds( 24 - (m_Rage * 2) );
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
					BossSpecialAttack.PerformSlam(
                       boss: this,
                       warcry: "*Releases a burst of crippling poison!*",
                       hue: 267,
                       rage: m_Rage,
                       range: 6,
					   physicalDmg:0,
                       poisonDmg: 100
                   );
                   break;
				}

				case 2: // entangling vines + bleed
				{
					BossSpecialAttack.PerformEntangle(
    				    boss: this,
    				    warcry: "*calls forth piercing vines*",
    				    hue: 0x4F6,
    				    rage: m_Rage,
    				    range: 6,
    				    bleedLevel: 5  // 10-15 damage per tick
    				);
    				break;
				}
				case 3: // cross poison
				{
					BossSpecialAttack.PerformCrossExplosion(
				       boss: this,
				       target: target,
				       warcry: "*Spores burst and explode!*",
				       hue: 0x4F6,
				       rage: m_Rage,
					   physicalDmg:0,
				       poisonDmg: 100
				   );
				   break;
				}
			}
		}


		public override void CheckReflect( Mobile caster, ref bool reflect )
		{
			int chance = m_Rage * 7;
			reflect = ( Utility.Random(100) < chance );
		}

		

		private int GetMaxSummons()
		{
			switch( m_Rage )
			{
				case 0: return 6;
				case 1: return 4;
				case 2: return 3;
				case 3: return 2;
				default: return 6;
			}
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
				0x4F6,// effect hue
				GetMaxSummons(),//summon limit
				50// cooldown
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
				0x4F6,// effect hue
				GetMaxSummons(),//summon limit
				50// cooldown
			);
		}

		public override bool OnBeforeDeath()
		{
			if ( m_Rage == 0 )
			{
				PublicOverheadMessage( MessageType.Regular, 0x21, false, "*releases a psychic shriek!*" );
				this.Hits = this.HitsMax;
				this.FixedParticles( 0x376A, 9, 32, 5030, EffectLayer.Waist );
				this.PlaySound( 0x202 );
				
				SetStr( Str + 25 );
				SetDamage( 22, 29 );
				
				m_Rage = 1;
				return false;
			}
			else if ( m_Rage == 1 )
			{
				PublicOverheadMessage( MessageType.Regular, 0x21, false, "*releases a crushing psychic scream!*" );
				this.Hits = this.HitsMax;
				this.FixedParticles( 0x376A, 9, 32, 5030, EffectLayer.Waist );
				this.PlaySound( 0x202 );
				
				SetStr( Str + 50 );
				SetDex( Dex + 15 );
				SetDamage( 27, 34 );
				VirtualArmor += 5;
				m_Rage = 2;
				return false;
			}
			else if ( m_Rage == 2 )
			{
				PublicOverheadMessage( MessageType.Regular, 0x21, false, "*releases an agonizing psychic scream!*" );
				this.Hits = this.HitsMax;
				this.FixedParticles( 0x376A, 9, 32, 5030, EffectLayer.Waist );
				this.PlaySound( 0x202 );
				
				SetStr( Str + 100 );
				SetDex( Dex + 35 );
				SetDamage( 32, 39 );
				VirtualArmor += 5;
				m_Rage = 3;
				return false;
			}
			else
			{
				Effects.SendLocationParticles( EffectItem.Create( this.Location, this.Map, EffectItem.DefaultDuration ), 0x3728, 10, 10, 2023 );
				this.PlaySound( 0x1FE );
				PublicOverheadMessage( MessageType.Regular, 0x21, false, "*withers into nothingness...*" );
				Mobile killer = this.LastKiller;
				if (killer != null && killer.Player && killer.Karma > 0)
            	{
            	    int marks = Utility.RandomMinMax(31, 71);
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

			int amt = Utility.RandomMinMax( 1, 2 );
			for ( int i = 0; i < amt; i++ )
			{
				c.DropItem( new EtherealPowerScroll() );
			}
			// gold explosion
		    RichesSystem.SpawnRiches( m_LastTarget, 2 );
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
			if (m_Summons == null)
					m_Summons = new List<BaseCreature>();
		}
	}
}