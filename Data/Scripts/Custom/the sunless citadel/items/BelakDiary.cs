using System;
using Server;
using Server.Items;
using System.Text;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;

namespace Server.Items
{
	public class BelakDiary : Item
	{
		[Constructable]
		public BelakDiary( ) : base( 0xE34 )
		{
			Weight = 1.0;
			Hue = 0xB98;
			Name = "a dusty diary";
			ItemID = 0x14EE;
		}

		public class ClueGump : Gump
		{
			public ClueGump( Mobile from ): base( 100, 100 )
			{
				from.PlaySound( 0x249 );
				string sText = "Those simpleminded fools from the order will regret the day in which they expeled me!<br>I was right. I was right and that is all that matters.<br>The Tree of Gulthias speaks to me. Long ago a vampire lord of some kind was staked in it with still living wood, and the stake grew past the creature's heart and into the tree, giving it powers over death itself!<br>I have studied it for long and managed to grow a garden, even here in the darkness, in this collapsed fortress from an age gone by. It's powers are impressive and their fruits wondrous in their magic. The white fruit that blossoms in winter is as vile a poison as I have ever seen, while the crimson fruit of summer can bring life even to those in the brink of death.<BR>But this is just the beginning. These so called 'adventurers' were the first to embrace Gulthias true power - while the tree exists, they will be bound to it and answer to my commands. Over time, I will have an army, and then I shall show to those simpletons of the howling order my true powers!";

				this.Closable=true;
				this.Disposable=true;
				this.Dragable=true;
				this.Resizable=false;

				AddPage(0);

				AddImage(0, 0, 10901, 2786);
				AddImage(0, 0, 10899, 2117);
				AddHtml( 45, 78, 386, 218, @"<BODY><BASEFONT Color=#d9c781>" + sText + "</BASEFONT></BODY>", (bool)false, (bool)true);
			}

			public override void OnResponse( NetState state, RelayInfo info ) 
			{
				Mobile from = state.Mobile; 
				from.PlaySound( 0x249 );
			}
		}

		public override void OnDoubleClick( Mobile m )
		{
			if ( m.InRange( this.GetWorldLocation(), 2 ) )
			{
				m.SendGump( new ClueGump( m ) );
				m.PlaySound( 0x249 );
			}
			else
			{
				m.SendLocalizedMessage( 502138 ); // That is too far away for you to use
			}
		}

		public BelakDiary(Serial serial) : base(serial)
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