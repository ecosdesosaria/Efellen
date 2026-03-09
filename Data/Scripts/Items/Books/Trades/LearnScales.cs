using System;
using Server;
using Server.Items;
using System.Text;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Misc;

namespace Server.Items
{
	public class LearnScalesBook : Item
	{
		public override Catalogs DefaultCatalog{ get{ return Catalogs.Book; } }

		[Constructable]
		public LearnScalesBook( ) : base( 0x1C11 )
		{
			ItemID = RandomThings.GetRandomBookItemID();
			Hue = Utility.RandomColor(0);
			Weight = 1.0;
			Name = "Reptile Scale Crafts";
		}

		public class LearnScalesGump : Gump
		{
			private Item m_Book;
			private int m_Page;
			private Mobile m_Mobile;

			public LearnScalesGump( Mobile from, Item book, int page ): base( 50, 50 )
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
					int amt = 13;
					int itm = 0;

					int x = 75;
					int y = 75;
					CraftResource res = CraftResource.RedScales;

					int modX = 289;
					int modY = 36;

					while ( amt > 0 )
					{
						amt--; itm++;

						AddItem( x, y, 9908, CraftResources.GetHue( res ) );
						AddHtml( x+44, y, 200, 20, @"<BODY><BASEFONT Color=" + color + ">" + CraftResources.GetName( res ) + " Scales</BASEFONT></BODY>", (bool)false, (bool)false);

						y += modY;

						if ( itm == 9 ){ y = 75; x += modX; }

						res = (CraftResource)( (int)res + 1 );
					}
				}
				else
				{
					AddItem(75, 85, 26372, 0x99D);
					AddItem(361, 83, 4017);
					AddItem(73, 169, 9908, 0x99D);
					AddItem(82, 139, 3922);
					AddItem(364, 144, 4016);

					string craft = "Ferreiros podem usar as escamas endurecidas de répteis para fazer vários tipos de armaduras e escudos. Essas escamas podem variar em cor e propriedades que melhoram para os itens que você pode criar com elas. Devido à natureza endurecida dessas escamas, seria necessário uma bigorna e uma forja para aquecê-las e martelá-las na forma necessária.";
					string scales = "Use um item cortante, como uma adaga ou faca, em um cadáver clicando duas vezes no item e depois selecionando o cadáver. Se houver escamas de réptil para serem retiradas, elas aparecerão na mochila. Diferentes tipos de escamas podem ser encontrados em muitas criaturas como lagartos, dragões e dinossauros. Você pode usar essas escamas para fazer diferentes tipos de armaduras e escudos usando ferramentas de escamação. Alguns dos tipos de escamas que você pode encontrar estão listados na próxima página.";
					AddHtml( 122, 80, 200, 300, @"<BODY><BASEFONT Color=" + color + ">" + craft + "</BASEFONT></BODY>", (bool)false, (bool)false);
					AddHtml( 415, 80, 200, 300, @"<BODY><BASEFONT Color=" + color + ">" + scales + "</BASEFONT></BODY>", (bool)false, (bool)false);
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

					m_Mobile.SendGump( new LearnScalesGump( m_Mobile, m_Book, m_Page ) );
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
				e.CloseGump( typeof( LearnScalesGump ) );
				e.SendGump( new LearnScalesGump( e, this, 1 ) );
				Server.Gumps.MyLibrary.readBook ( this, e );
			}
		}

		public LearnScalesBook(Serial serial) : base(serial)
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
				Name = "Reptile Scale Crafts";
			}
		}
	}
}