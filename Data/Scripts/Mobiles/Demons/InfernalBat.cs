using System;
using System.Collections;
using Server.Items;
using Server.Targeting;
using Server.Regions;
using Server.Mobiles;
using Server.Misc;

namespace Server.Mobiles
{
	[CorpseName( "am Infernal Bats corpse" )]
	public class InfernalBat : BaseCreature
	{
		[Constructable]
		public InfernalBat() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a Infernal Bat";
			Body = 317;
			BaseSoundID = 0x270;
			Hue = 0xB01;

			SetStr( 191, 310 );
			SetDex( 291, 315 );
			SetInt( 26, 50 );

			SetHits( 255,366 );

			SetDamage( 14, 19 );

			SetDamageType( ResistanceType.Physical, 50 );
			SetDamageType( ResistanceType.Poison, 50 );

			SetResistance( ResistanceType.Physical, 45 );
			SetResistance( ResistanceType.Fire, 35 );
			SetResistance( ResistanceType.Cold, 35 );
			SetResistance( ResistanceType.Poison, 60, 70 );
			SetResistance( ResistanceType.Energy, 40 );

			SetSkill( SkillName.MagicResist, 70.1, 95.0 );
			SetSkill( SkillName.Tactics, 100.0 );
			SetSkill( SkillName.FistFighting, 105.0 );

			Fame = 4000;
			Karma = -4000;

			VirtualArmor = 24;

		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Rich, 1 );
		}


		public override int GetIdleSound()
		{
			return 0x29B;
		}

		public InfernalBat( Serial serial ) : base( serial )
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