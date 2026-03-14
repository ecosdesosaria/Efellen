using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class MagesBalladScroll : SpellScroll
	{
		public override string DefaultDescription{ get{ return SongBook.SpellDescription( 361 ); } }

		[Constructable]
		public MagesBalladScroll() : this( 1 )
		{
		}

		[Constructable]
		public MagesBalladScroll( int amount ) : base( 361, 0x1F30, amount )
		{
			Name = "mage's ballad sheet music";
			Hue = 0x96;
			Stackable = true;
        }

		public MagesBalladScroll( Serial serial ) : base( serial )
		{
		}

		public override void OnDoubleClick( Mobile from )
		{
			from.SendMessage( "A partitura deve estar em seu livro de música." );
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