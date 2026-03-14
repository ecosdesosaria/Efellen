using System;

namespace Server.Items
{
	public class DecoDragonsBlood : Item
	{
		[Constructable]
		public DecoDragonsBlood() : base( 0x4077 )
		{
			Movable = true;
			Stackable = false;
		}

		public DecoDragonsBlood( Serial serial ) : base( serial )
		{
		}

		public override bool OnDragLift( Mobile from )
		{
			from.SendMessage( "Este reagente pagão não pode ser usado em alquimia, mas é raro e colecionável." );
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
