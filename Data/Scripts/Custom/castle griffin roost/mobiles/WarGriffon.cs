using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "a griffon corpse" )]
	public class WarGriffon : BaseMount
	{
		public override WeaponAbility GetWeaponAbility()
		{
			return WeaponAbility.BleedAttack;
		}

		[Constructable]
		public WarGriffon() : this( "a trained griffon" )
		{
		}

		[Constructable]
		public WarGriffon( string name ) : base( name, 0x31F, 0x3EBE, AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4 )
		{
			BaseSoundID = 0x2EE;
			Hue = 0x0672;

			SetStr( 296, 320 );
			SetDex( 286, 310 );
			SetInt( 151, 175 );

			SetHits( 458, 572 );

			SetDamage( 29, 35 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 45, 60 );
			SetResistance( ResistanceType.Fire, 30, 40 );
			SetResistance( ResistanceType.Cold, 30, 60 );
			SetResistance( ResistanceType.Poison, 30, 40 );
			SetResistance( ResistanceType.Energy, 30, 50 );

			SetSkill( SkillName.MagicResist, 80.1, 95.0 );
			SetSkill( SkillName.Tactics, 90.1, 110.0 );
			SetSkill( SkillName.FistFighting, 90.1, 112.0 );

			Fame = 9500;
			Karma = 8500;

			VirtualArmor = 52;
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Rich, 2 );
			AddLoot( LootPack.Rich, 2 );
		}

		public override int Meat{ get{ return 12; } }
		public override MeatType MeatType{ get{ return MeatType.Bird; } }
		public override int Feathers{ get{ return 50; } }
		public override int Skeletal{ get{ return Utility.Random(4); } }
		public override SkeletalType SkeletalType{ get{ return SkeletalType.Mystical; } }

		public WarGriffon( Serial serial ) : base( serial )
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