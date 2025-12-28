using System;
using Server.Items;
using Server.Engines.Plants;
using Server.Custom.DailyBosses.System;

namespace Server.Mobiles
{
	[CorpseName( "a kuthulu corpse" )]
	public class Kuthulu : BaseCreature
	{
		private Timer m_Timer;
		private DateTime m_NextSpecialAttack = DateTime.MinValue;
		[Constructable]
		public Kuthulu() : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			
			if ( Utility.RandomBool() )
			{
				Name = "a kuthulu";
				Body = 352;
				BaseSoundID = 357;

				SetStr( 1150 );
				SetDex( 275 );
				SetInt( 750 );

				SetHits( 1450 );

				SetDamage( 26, 30 );

				SetDamageType( ResistanceType.Physical, 60 );
				SetDamageType( ResistanceType.Cold, 20 );
				SetDamageType( ResistanceType.Energy, 20 );

				SetResistance( ResistanceType.Physical, 55 );
				SetResistance( ResistanceType.Fire, 55 );
				SetResistance( ResistanceType.Cold, 85 );
				SetResistance( ResistanceType.Poison, 60 );
				SetResistance( ResistanceType.Energy, 70 );

				SetSkill( SkillName.Psychology, 120.0 );
				SetSkill( SkillName.Magery, 125.0 );
				SetSkill( SkillName.Meditation, 120.0 );
				SetSkill( SkillName.MagicResist, 145.0 );
				SetSkill( SkillName.Tactics, 105.0 );
				SetSkill( SkillName.FistFighting, 105.0 );

				Fame = 9500;
				Karma = -9500;

				VirtualArmor = 34;
			}
			else
			{
				Name = "an azathoth";
				Body = 222;
				BaseSoundID = 357;

				SetStr( 801, 950 );
				SetDex( 126, 175 );
				SetInt( 201, 250 );

				SetHits( 650 );

				SetDamage( 22, 26 );

				SetDamageType( ResistanceType.Physical, 60 );
				SetDamageType( ResistanceType.Cold, 20 );
				SetDamageType( ResistanceType.Energy, 20 );

				SetResistance( ResistanceType.Physical, 45, 55 );
				SetResistance( ResistanceType.Fire, 25, 35 );
				SetResistance( ResistanceType.Cold, 15, 25 );
				SetResistance( ResistanceType.Poison, 60, 70 );
				SetResistance( ResistanceType.Energy, 40, 50 );

				SetSkill( SkillName.Psychology, 90.1, 100.0 );
				SetSkill( SkillName.Magery, 90.1, 100.0 );
				SetSkill( SkillName.Meditation, 90.1, 100.0 );
				SetSkill( SkillName.MagicResist, 90.1, 105.0 );
				SetSkill( SkillName.Tactics, 75.1, 85.0 );
				SetSkill( SkillName.FistFighting, 80.1, 100.0 );

				Fame = 9500;
				Karma = -9500;

				VirtualArmor = 44;

			}

			PackReg( 24, 45 );

			m_Timer = new GiantToad.TeleportTimer( this, 0x1FE );
			m_Timer.Start();
		}

		public override void OnDamage( int amount, Mobile from, bool willKill )
		{
			if ( DateTime.UtcNow >= m_NextSpecialAttack )
			{
				PerformRageAttack( from );
				m_NextSpecialAttack = DateTime.UtcNow + TimeSpan.FromSeconds( 45 );
			}
			
			base.OnDamage( amount, from, willKill );
		}

		private void PerformRageAttack( Mobile target )
		{
			if ( target == null || target.Deleted || !target.Alive )
				return;

			int attackChoice = Utility.RandomMinMax( 1, 3 );
            Map map = this.Map;

			switch ( attackChoice  )
			{
				case 1: // energy burst
				{
					BossSpecialAttack.PerformTargettedAoE(
						this,
						target,
						1,
						"*Screeches Insanely*",
						1571,  // hue
						0,     // physical
						0,   // fire
						100,     // cold
						0,     // poison
						0      // energy
					);
					break;
				}
				case 2: // energy nova
				{
					BossSpecialAttack.PerformCrossExplosion(
					    boss: this,
					    target: target,
					    warcry:"*Screeches Insanely*",
					    hue: 1571,
					    rage: 2,
					    coldDmg: 100,
					    fireDmg: 0,
					    energyDmg:0,
					    poisonDmg: 0,
					    physicalDmg: 0
					);
                	break;
			    }
				case 3: // energy nova
				{
					BossSpecialAttack.PerformSlam(
                	    boss: this,
                	    warcry:"*Screeches Insanely*",
                	    hue: 1571,
                	    rage: 2,
                	    range: 6,
                	    physicalDmg: 0,
						coldDmg: 100
                	);
                	break;
			    }
			}
		}

		public override void GenerateLoot()
		{
			if ( Body == 222 ){ AddLoot( LootPack.FilthyRich, 2 ); } else { AddLoot( LootPack.Rich, 2 ); } 
		}

		public override Poison PoisonImmune
		{
			get
			{
				if ( Body == 222 ){ return Poison.Lethal; } return Poison.Greater;
			}
		}

		public override int Meat{ get{ return 3; } }

		public Kuthulu( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 1 );
			writer.Write( m_NextSpecialAttack );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
			if ( version >= 1 )
			{
				m_NextSpecialAttack = reader.ReadDateTime();
			}
			m_Timer = new GiantToad.TeleportTimer( this, 0x1FE );
			m_Timer.Start();
		}		
	}
}