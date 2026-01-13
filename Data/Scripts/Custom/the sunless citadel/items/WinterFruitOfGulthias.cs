using System;
using Server;
using Server.Network;

namespace Server.Items
{
	public class WinterFruitOfGulthias : Item
	{
		[Constructable]
		public WinterFruitOfGulthias() : base( 0x9D0 )
		{
			Name = "Winter Fruit of Gulthias";
			Hue = 1150; 
			Weight = 1.0;
			Stackable = true;
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );
			list.Add( "Looks appetizing" );
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( !IsChildOf( from.Backpack ) )
			{
				from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
				return;
			}

			from.ApplyPoison( from, Poison.Deadly );
			
			from.SendMessage( "You eat the Winter Fruit of Gulthias and immediately feel violently ill!" );
			from.PlaySound( 0x246 );
			from.FixedEffect( 0x374A, 10, 16, 0x3F, 0 );
			
			Delete();
		}

		public WinterFruitOfGulthias( Serial serial ) : base( serial )
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