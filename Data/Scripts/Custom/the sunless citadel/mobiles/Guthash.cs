using System;
using Server.Mobiles;

namespace Server.Mobiles
{
	[CorpseName( "Guthash's corpse" )]
	public class Guthash : BaseCreature
	{
		[Constructable]
		public Guthash() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "Guthash";
			Body = 0xD7;
			BaseSoundID = 0x188;

			SetStr( 144 );
			SetDex( 95 );
			SetInt( 40 );

			SetHits( 126, 169 );
			SetMana( 0 );

			SetDamage( 9, 14 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 15, 20 );
			SetResistance( ResistanceType.Fire, 5, 10 );
			SetResistance( ResistanceType.Poison, 45 );

			SetSkill( SkillName.MagicResist, 50.0 );
			SetSkill( SkillName.Tactics, 74.0 );
			SetSkill( SkillName.FistFighting, 64.0 );

			Fame = 800;
			Karma = -800;

			VirtualArmor = 32;

			Tamable = true;
			ControlSlots = 1;
			MinTameSkill = 59.1;
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Poor );
		}

		public override int Meat{ get{ return 1; } }
		public override int Hides{ get{ return 6; } }
		public override FoodType FavoriteFood{ get{ return FoodType.Fish | FoodType.Meat | FoodType.FruitsAndVegies | FoodType.Eggs; } }

		public Guthash(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int) 0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}
}