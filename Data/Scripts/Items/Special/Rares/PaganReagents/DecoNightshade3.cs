using System;

namespace Server.Items
{
	public class DecoNightshade3 : Item
	{

		[Constructable]
		public DecoNightshade3() : base( 0x18E6 )
		{
			Movable = true;
			Stackable = false;
		}

		public DecoNightshade3( Serial serial ) : base( serial )
		{
		}

		public override bool OnDragLift( Mobile from )
		{
			from.SendMessage( "Isto não pode ser usado em alquimia, mas é raro e colecionável." );
			return base.OnDragLift( from );
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
