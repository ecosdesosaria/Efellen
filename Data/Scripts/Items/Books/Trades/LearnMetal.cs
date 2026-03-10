using System;
using Server;
using Server.Items;
using System.Text;
using Server.Mobiles;
using Server.Gumps;
using Server.Misc;
using Server.Network;

namespace Server.Items
{
	public class LearnMetalBook : Item
	{
		public override Catalogs DefaultCatalog{ get{ return Catalogs.Book; } }

		[Constructable]
		public LearnMetalBook( ) : base( 0x1C11 )
		{
			ItemID = RandomThings.GetRandomBookItemID();
			Hue = Utility.RandomColor(0);
			Weight = 1.0;
			Name = "Metal Smithing & Tinkering";
		}

		public class LearnMetalGump : Gump
		{
			private Item m_Book;
			private int m_Page;
			private Mobile m_Mobile;

			public LearnMetalGump( Mobile from, Item book, int page ): base( 50, 50 )
			{
				m_Book = book;
				m_Page = page;
				m_Mobile = from;

				string color = "#CEAA87";
				m_Mobile.SendSound( 0x55 );

				Closable = true;
				Disposable = true;
				Dragable = true;
				Resizable = false;

				AddPage(0);

				AddImage(0, 0, 7005, book.Hue);
				AddImage(0, 0, 7006);
				AddImage(0, 0, 7024, 2789);

				int prevPage = page - 1; if ( prevPage < 1 ){ prevPage = 900; }
				int nextPage = page + 1;

				AddHtml( 106, 44, 215, 20, @"<BODY><BASEFONT Color=" + color + ">" + m_Book.Name + "</BASEFONT></BODY>", (bool)false, (bool)false);

				AddButton(71, 41, 4014, 4014, prevPage, GumpButtonType.Reply, 0);
				AddButton(596, 41, 4005, 4005, nextPage, GumpButtonType.Reply, 0);

				if ( m_Page == 2 )
				{
					int amt = 16;
					int itm = 0;

					int x = 75;
					int y = 75;
					CraftResource res = CraftResource.Iron;

					int modX = 289;
					int modY = 36;

					while ( amt > 0 )
					{
						amt--; itm++;

						AddItem( x, y, 7153, CraftResources.GetHue( res ) );
						AddHtml( x+44, y, 200, 20, @"<BODY><BASEFONT Color=" + color + ">" + CraftResources.GetName( res ) + "</BASEFONT></BODY>", (bool)false, (bool)false);

						y += modY;

						if ( itm == 9 ){ y = 75; x += modX; }

						res = (CraftResource)( (int)res + 1 );
					}
				}
				else
				{
					AddItem(358, 157, 6585);
					AddItem(363, 299, 4017);
					AddItem(75, 82, 4020);
					AddItem(360, 334, 4015);
					AddItem(348, 72, 3897);
					AddItem(79, 256, 26376);
					AddItem(365, 259, 7153);
					AddItem(367, 111, 3717);

					string smithing = "Ferraria é a arte de pegar lingotes de metal e criar armaduras e armas. Você precisará de um martelo de ferreiro e materiais. Quanto melhor o metal, melhor o item.";
					string tinkering = "Funilaria permite criar itens intrincados como ferramentas. Você também pode fazer outros itens como joias. Metais melhores fazem ferramentas e joias melhores. Joias podem ser aprimoradas com gemas.";
					string mining = "Para obter lingotes de metal, você precisa encontrar minério. O minério pode ser extraído em cavernas ou ao longo de encostas de montanhas. Usar o minério em uma forja o fundirá em lingotes. Quanto melhor sua habilidade de mineração, melhor minério você pode encontrar e fundir.";
					string crafting = "Além das ferramentas necessárias para criar, você pode precisar estar perto de uma bigorna e forja para fabricar itens. Alguns exemplos de metais que você pode encontrar estão listados na próxima página.";
					AddHtml( 122, 80, 200, 130, @"<BODY><BASEFONT Color=" + color + ">" + smithing + "</BASEFONT></BODY>", (bool)false, (bool)false);
					AddHtml( 122, 255, 200, 130, @"<BODY><BASEFONT Color=" + color + ">" + tinkering + "</BASEFONT></BODY>", (bool)false, (bool)false);
					AddHtml( 415, 80, 200, 130, @"<BODY><BASEFONT Color=" + color + ">" + mining + "</BASEFONT></BODY>", (bool)false, (bool)false);
					AddHtml( 415, 255, 200, 130, @"<BODY><BASEFONT Color=" + color + ">" + crafting + "</BASEFONT></BODY>", (bool)false, (bool)false);
				}
			}

			public override void OnResponse( NetState state, RelayInfo info ) 
			{
				if ( info.ButtonID > 0 )
				{
					m_Page = info.ButtonID;
					if ( m_Page >= 900 )
						m_Page = 2;
					else if ( m_Page > 2 )
						m_Page = 1;
					else
						m_Page = info.ButtonID;

					m_Mobile.SendGump( new LearnMetalGump( m_Mobile, m_Book, m_Page ) );
				}
				else
					m_Mobile.SendSound( 0x55 );
			}
		}

		public override void OnDoubleClick( Mobile e )
		{
			if ( !IsChildOf( e.Backpack ) && this.Weight != -50.0 ) 
			{
				e.SendMessage( "Isto precisa estar em sua mochila para ser lido." );
			}
			else
			{
				e.CloseGump( typeof( LearnMetalGump ) );
				e.SendGump( new LearnMetalGump( e, this, 1 ) );
				Server.Gumps.MyLibrary.readBook ( this, e );
			}
		}

		public LearnMetalBook(Serial serial) : base(serial)
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

			if ( ItemID == 0x02DD || ItemID == 0x201A )
			{
				ItemID = RandomThings.GetRandomBookItemID();
				Hue = Utility.RandomColor(0);
				Name = "Metal Smithing & Tinkering";
			}
		}
	}
}