using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class KnightsMinneScroll : SpellScroll
	{
		public override string DefaultDescription{ get{ return SongBook.SpellDescription( 360 ); } }

		[Constructable]
		public KnightsMinneScroll() : this( 1 )
		{
		}

		[Constructable]
		public KnightsMinneScroll( int amount ) : base( 360, 0x1F31, amount )
		{
			Name = "knight's minne sheet music";
			Hue = 0x96;
			Stackable = true;
        }

		public KnightsMinneScroll( Serial serial ) : base( serial )
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