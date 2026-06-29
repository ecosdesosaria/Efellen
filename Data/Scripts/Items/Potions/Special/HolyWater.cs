using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Misc;
using Server.Network;

namespace Server.Items
{
	public class HolyWater : Item
	{
		public override string DefaultDescription{ get{ return "Esta é uma bacia de água benta. Você pode coletar um pouco dela se tiver um frasco cristalino. Jogá-la no chão será prejudicial a criaturas sobrenaturais."; } }

		[Constructable]
		public HolyWater( ) : base( 0x1008 )
		{
			Weight = 1.0;
			Movable = false;
			Name = "holy water";
		}

		public HolyWater(Serial serial) : base(serial)
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
}