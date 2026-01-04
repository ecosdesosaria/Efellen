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
using Server.EffectsUtil;
using Server.Custom;
using Server.Custom.DailyBosses.System;

namespace Server.Mobiles
{
	[CorpseName( "Butcher's Corpse" )]
	public class UndeadButcher : BaseCreature
	{
		private int m_Rage = 0;
		private Mobile m_LastTarget;
		private DateTime m_NextSpecialAttack = DateTime.MinValue;

		private static readonly List<Type> BossDrops = new List<Type>
    	{
    	    typeof(Artifact_ButchersSaw),
    	    typeof(Artifact_ButchersGrinder),
    	    typeof(Artifact_ButchersCleaver),
    	    typeof(Artifact_ButchersCaress),
			typeof(ButchersViolence)
    	};
        
		[Constructable]
		public UndeadButcher () : base( AIType.AI_Melee, FightMode.Closest, 20, 1, 0.4, 0.8 )
		{
			Title = " the Hungry";
			NameHue = 0x92E;
            Body = 999;
            Hue = 0x845;
			BaseSoundID = 684;
			Name = "Butcher";
			SetStr( 366, 455 );
			SetDex( 125 );
			SetInt( 106, 135 );

			SetHits( 1755 );
			SetDamage( 19, 29 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 60 );
			SetResistance( ResistanceType.Fire, 45 );
			SetResistance( ResistanceType.Cold, 45 );
			SetResistance( ResistanceType.Poison, 70 );
			SetResistance( ResistanceType.Energy, 35 );

			SetSkill( SkillName.Anatomy, 95.5, 125.0 );
			SetSkill( SkillName.MagicResist, 95.5, 125.0 );
			SetSkill( SkillName.Tactics, 101.0, 125.0 );
			SetSkill( SkillName.FistFighting, 111.0, 121.0 );

			Fame = 13000;
			Karma = 15000;

			VirtualArmor = 20;
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.UltraRich, 2 );
		}


		public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);
        }


		public override int TreasureMapLevel{ get{ return 2; } }
		public override bool CanRummageCorpses{ get{ return false; } }
		public override bool ReacquireOnMovement{ get{ return !Controlled; } }
		public override bool BleedImmune{ get{ return true; } }
		public override bool BardImmune { get { return true; } }
		public override bool Unprovokable { get { return true; } }
		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }
		public override Poison HitPoison{ get{ return Poison.Greater; } }

		public override void OnDamage( int amount, Mobile from, bool willKill )
		{
			m_LastTarget = from;
			Server.Misc.IntelligentAction.LeapToAttacker( this, from );
			
			if ( m_Rage >= 1 && DateTime.UtcNow >= m_NextSpecialAttack )
			{
				PerformRageAttack( from );
				m_NextSpecialAttack = DateTime.UtcNow + TimeSpan.FromSeconds( 40 - (m_Rage * 2) );
			}

			base.OnDamage( amount, from, willKill );
		}

		private void PerformRageAttack(Mobile target)
        {
            if (target == null || target.Deleted || !target.Alive)
                return;

            switch (m_Rage)
            {
                case 0:
                     BossSpecialAttack.PerformRampage(
                       boss: this,
                       warcry: "FRESH MEAT!",
                       hue: 0x845,
                       rage: m_Rage,
                       stunDuration: 4.0
                   );
                   break;

                case 1:
                   BossSpecialAttack.PerformFear(
				      boss: this,
				      warcry: "FRESH MEAT!",
				      range: 6,
				      rage: m_Rage,
				      terror: 70  // Knightship 70+ saves from fear
				  );
				  break;

                case 2:
                    BossSpecialAttack.PerformDegenAura(
		                this,
		                "*spreads its innards around!*",
		                8,          // radius
		                m_Rage,     // rage level
		                12,         // duration - 12 + rage*2 seconds, damage happens every 2 seconds 
		                20,         // intensity - 20 + rage damage per tick
		                "health",   // target attribute
		                0x845       // hue
		            );
                    break;
            }
        }

		public override void CheckReflect( Mobile caster, ref bool reflect )
		{
			int chance = m_Rage * 7;
			reflect = ( Utility.Random(100) < chance );
		}

		public override bool OnBeforeDeath()
		{
			if ( m_Rage == 0 )
			{
				PublicOverheadMessage( MessageType.Regular, 0x21, false, "*RWAAAARRRGHH*" );
				this.Hits = this.HitsMax;
				this.FixedParticles( 0x376A, 9, 32, 5030, EffectLayer.Waist );
				this.PlaySound( 0x202 );
				
				SetStr( Str + 50 );
				SetDex( Dex + 25 );
				SetDamage( 13, 19 );
				m_Rage = 1;
				return false;
			}
			else if ( m_Rage == 1 )
			{
				PublicOverheadMessage( MessageType.Regular, 0x21, false, "*RWAAAAAAAAARRRGHH*!" );
				this.Hits = this.HitsMax;
				this.FixedParticles( 0x376A, 9, 32, 5030, EffectLayer.Waist );
				this.PlaySound( 0x202 );
				
				SetStr( Str + 80 );
				SetDex( Dex + 35 );
				SetDamage( 21, 29 );
				VirtualArmor += 5;
				m_Rage = 2;
				return false;
			}
			else if ( m_Rage == 2 )
			{
				Effects.SendLocationParticles( EffectItem.Create( this.Location, this.Map, EffectItem.DefaultDuration ), 0x3728, 10, 10, 2023 );
				this.PlaySound( 0x1FE );
				PublicOverheadMessage( MessageType.Regular, 0x21, false, "I go...Home...." );
                Mobile killer = this.LastKiller;
				if (killer != null && killer.Player && killer.Karma < 0)
            	{
            	    int marks = Utility.RandomMinMax(51, 74);
            	    Server.Custom.DefenderOfTheRealm.MarkLootHelper.AwardMarks(killer, 1, marks);
            	}
			}
			
			return base.OnBeforeDeath();
		}

        public override void OnDelete()
        {
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
		    RichesSystem.SpawnRiches( m_LastTarget, 1 );
		}

		public override void OnAfterSpawn()
		{
			base.OnAfterSpawn();
		}

		public UndeadButcher( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 1 ); // version

			writer.Write( m_Rage );
			writer.Write( m_NextSpecialAttack );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();

			if ( version >= 1 )
			{
				m_Rage = reader.ReadInt();
				m_NextSpecialAttack = reader.ReadDateTime();
			}
		}
	}
}