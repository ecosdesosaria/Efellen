using System;
using Server;
using Server.Network;
using Server.Targeting;

namespace Server.Items
{
	public class SummerFruitOfGulthias : Item
	{
		[Constructable]
		public SummerFruitOfGulthias() : base( 0x9D0 )
		{
			Name = "Summer Fruit of Gulthias";
			Hue = 0;
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

			int healAmount = Utility.RandomMinMax( 70, 120 );
			from.Heal( healAmount );
			
			from.SendMessage( "Você come o Fruto de Verão de Gulthias e se sente revigorado!" );
			from.PlaySound( 0x4F );
			from.FixedEffect( 0x376A, 9, 32 );
			
			Delete();
		}

		public SummerFruitOfGulthias( Serial serial ) : base( serial )
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