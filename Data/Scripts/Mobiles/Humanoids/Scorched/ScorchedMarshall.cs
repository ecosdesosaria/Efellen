using System;
using System.Collections;
using Server.Items;
using Server.Targeting;
using Server.Custom;

namespace Server.Mobiles
{
	[CorpseName( "a Scorched Marshall's corpse" )]
	public class ScorchedMarshall : BaseCreature
	{
		[Constructable]
		public ScorchedMarshall() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a Scorched Marshall";
			Body = 0x41;
			Hue = 2931;

			SetStr( 236, 320 );
			SetDex( 116, 145 );
			SetInt( 36, 60 );

			SetHits( 185, 240 );

			SetDamage( 10, 21 );

			SetDamageType( ResistanceType.Physical, 40 );
			SetDamageType( ResistanceType.Fire, 60 );

			SetResistance( ResistanceType.Physical, 45, 65 );
			SetResistance( ResistanceType.Fire, 60, 70 );
			SetResistance( ResistanceType.Cold, 20, 30 );
			SetResistance( ResistanceType.Poison, 30, 40 );
			SetResistance( ResistanceType.Energy, 40, 50 );

			SetSkill( SkillName.MagicResist, 95.1, 101.0 );
			SetSkill( SkillName.Tactics, 95.1, 104.1 );
			SetSkill( SkillName.FistFighting, 91.1, 108.1 );

			Fame = 6000;
			Karma = -6000;

			VirtualArmor = 50;
			
			switch ( Utility.Random( 6 ) )
			{
				case 0: PackItem( new PlateArms() ); break;
				case 1: PackItem( new PlateChest() ); break;
				case 2: PackItem( new PlateGloves() ); break;
				case 3: PackItem( new PlateGorget() ); break;
				case 4: PackItem( new PlateLegs() ); break;
				case 5: PackItem( new PlateHelm() ); break;
			}

			PackItem( new Longsword() );
			PackItem( new DarkShield() );
		}

        public override void OnDeath(Container c)
		{
		    base.OnDeath(c);
		    BossLootSystem.BossEnchant(this, c, 250, 15, 1, "scorched");
		}

        public override bool CanRummageCorpses{ get{ return false; } }

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Rich );
		}

		public ScorchedMarshall( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
}