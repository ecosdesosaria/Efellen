using System;
using Server;
using Server.Items;
using Server.Custom.DailyBosses.System;

namespace Server.Mobiles
{
	[CorpseName( "an ophidian corpse" )]
	public class OphidianMatriarch : BaseCreature
	{
		private DateTime m_NextSpecialAttack = DateTime.MinValue;
		[Constructable]
		public OphidianMatriarch() : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "an ophidian matriarch";
			Body = 87;
			BaseSoundID = 644;

			SetStr( 416, 505 );
			SetDex( 96, 115 );
			SetInt( 366, 455 );

			SetHits( 250, 303 );

			SetDamage( 11, 13 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 65 );
			SetResistance( ResistanceType.Fire, 50 );
			SetResistance( ResistanceType.Cold, 45 );
			SetResistance( ResistanceType.Poison, 60 );
			SetResistance( ResistanceType.Energy, 35, 45 );

			SetSkill( SkillName.Psychology, 90.1, 100.0 );
			SetSkill( SkillName.Magery, 110.0 );
			SetSkill( SkillName.Meditation, 35.0 );
			SetSkill( SkillName.MagicResist, 90.1, 100.0 );
			SetSkill( SkillName.Tactics, 50.1, 70.0 );
			SetSkill( SkillName.FistFighting, 60.1, 80.0 );

			Fame = 16000;
			Karma = -16000;

			VirtualArmor = 50;
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Rich );
			AddLoot( LootPack.Average, 2 );
			AddLoot( LootPack.MedScrolls, 2 );
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
						"I shall annihilate you!",
						267,  // hue
						0,     // physical
						0,   // fire
						0,     // cold
						100,     // poison
						0      // energy
					);
					break;
				}
				case 2: // energy nova
				{
					BossSpecialAttack.PerformCrossExplosion(
					    boss: this,
					    target: target,
					    warcry: "My brood shall be free of you!",
					    hue: 267,
					    rage: 2,
					    coldDmg: 0,
					    fireDmg: 0,
					    energyDmg: 0,
					    poisonDmg: 100,
					    physicalDmg: 0
					);
                	break;
			    }
				case 3: // energy nova
				{
					BossSpecialAttack.PerformSlam(
                	    boss: this,
                	    warcry: "You will wither and decay!",
                	    hue: 267,
                	    rage: 2,
                	    range: 6,
                	    physicalDmg: 0,
						poisonDmg: 100
                	);
                	break;
			    }
			}
		}

		public override Poison PoisonImmune{ get{ return Poison.Greater; } }
		public override int TreasureMapLevel{ get{ return 4; } }
		public override int Meat{ get{ return 1; } }
		public override int Hides{ get{ return 5; } }
		public override HideType HideType{ get{ return HideType.Barbed; } }
		public override int Skin{ get{ return Utility.Random(4); } }
		public override SkinType SkinType{ get{ return SkinType.Snake; } }

		public OphidianMatriarch( Serial serial ) : base( serial )
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

			if ( BaseSoundID == 274 )
				BaseSoundID = 838;
		}
	}
}