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
	[CorpseName( "Blacktooth's Corpse" )]
	public class Blacktooth : BaseCreature
	{
		private int m_Rage = 0;
		private Mobile m_LastTarget;
		private DateTime m_NextSpecialAttack = DateTime.MinValue;

		private static readonly List<Type> BossDrops = new List<Type>
    	{
    	    typeof(Artifact_AncestorScorn),
    	    typeof(Artifact_AncestorEmbrace),
    	    typeof(Artifact_AncestorGrip),
    	    typeof(Artifact_AncestorWarpath),
			typeof(Artifact_AncestorGarb),
    	};
        
		[Constructable]
		public Blacktooth () : base( AIType.AI_Melee, FightMode.Closest, 20, 1, 0.4, 0.8 )
		{
			Title = " the Vicious";
			NameHue = 0x92E;
            Body = 736;
			BaseSoundID = 0xA3;
			Name = "Blacktooth";
			Hue = 660;
			SetStr( 366, 425 );
			SetDex( 125, 165 );
			SetInt( 106, 235 );

			SetHits( 1455 );
			SetDamage( 19, 29 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 40 );
			SetResistance( ResistanceType.Fire, 45 );
			SetResistance( ResistanceType.Cold, 45 );
			SetResistance( ResistanceType.Poison, 70 );
			SetResistance( ResistanceType.Energy, 55 );

			SetSkill( SkillName.Anatomy, 95.5, 125.0 );
			SetSkill( SkillName.MagicResist, 95.5, 125.0 );
			SetSkill( SkillName.Tactics, 101.0, 125.0 );
			SetSkill( SkillName.FistFighting, 111.0, 121.0 );

			Fame = 13000;
			Karma = 15000;

			VirtualArmor = 25;
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
		public override int GetIdleSound(){ return 0x61D; }
        public override int GetAngerSound(){ return 0x61A; }
        public override int GetHurtSound(){ return 0x61C; }
        public override int GetDeathSound(){ return 0x61B; }
		

		public override void OnDamage( int amount, Mobile from, bool willKill )
		{
			m_LastTarget = from;
			Server.Misc.IntelligentAction.LeapToAttacker( this, from );
			
			if ( m_Rage >= 1 && DateTime.UtcNow >= m_NextSpecialAttack )
			{
				PerformRageAttack( from );
				m_NextSpecialAttack = DateTime.UtcNow + TimeSpan.FromSeconds( 20.6 - (m_Rage * 1.5) );
			}

			base.OnDamage( amount, from, willKill );
		}

		private void PerformRageAttack(Mobile target)
        {
            if (target == null || target.Deleted || !target.Alive)
                return;

            int roll;

            if (m_Rage <= 1)
                roll = Utility.Random(2); // 0–1
            else 
                roll = Utility.Random(4); // 0–3

            switch (roll)
            {
                case 0:
                     BossSpecialAttack.PerformRampage(
                       boss: this,
                       warcry: "*Screeches and charges forward!*",
                       hue: 660,
                       rage: m_Rage,
                       stunDuration: 6.0
                   );
                   break;

                case 1:
                   BossSpecialAttack.PerformFear(
				      boss: this,
				      warcry: "*Unleashes a bestial roar!*",
				      range: 6,
				      rage: m_Rage,
				      terror: 70  // Knightship 70+ saves from fear
				  );
				  break;

                case 2:
                    BossSpecialAttack.SummonHonorGuard(
                        boss: this,
                        target: target,
                        warcry: "Ancestors! Aid me!",
                        amount: 1,
                        creatureType: typeof(SummonDireBear),
                        hue: 660
                    );
                    break;

                case 3:
                    BossSpecialAttack.PerformSlam(
                       boss: this,
                       warcry: "The ground shakes!",
                       hue: 660,
                       rage: m_Rage,
                       range: 6,
                       physicalDmg: 100
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

		public Blacktooth( Serial serial ) : base( serial )
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