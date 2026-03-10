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
	public class LearnTailorBook : Item
	{
		public override Catalogs DefaultCatalog{ get{ return Catalogs.Book; } }

		[Constructable]
		public LearnTailorBook( ) : base( 0x1C11 )
		{
			ItemID = RandomThings.GetRandomBookItemID();
			Hue = Utility.RandomColor(0);
			Weight = 1.0;
			Name = "Tailoring the Cloth";
		}

		public class LearnTailorGump : Gump
		{
			private Item m_Book;
			private int m_Page;
			private Mobile m_Mobile;

			public LearnTailorGump( Mobile from, Item book, int page ): base( 50, 50 )
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
					int amt = 12;
					int itm = 0;

					int x = 75;
					int y = 75;
					CraftResource res = CraftResource.Fabric;

					int modX = 289;
					int modY = 36;

					while ( amt > 0 )
					{
						amt--; itm++;

						AddItem( x, y, 5987, CraftResources.GetHue( res ) );
						AddHtml( x+44, y, 200, 20, @"<BODY><BASEFONT Color=" + color + ">" + CraftResources.GetName( res ) + "</BASEFONT></BODY>", (bool)false, (bool)false);

						y += modY;

						if ( itm == 9 ){ y = 75; x += modX; }

						res = (CraftResource)( (int)res + 1 );
					}
				}
				else
				{
					AddItem(368, 137, 6812);
					AddItem(378, 167, 21562);
					AddItem(84, 82, 19585, 0xB61);
					AddItem(368, 106, 3577);
					AddItem(76, 122, 5987);
					AddItem(370, 73, 3576);
					AddItem(76, 164, 3999);
					AddItem(354, 292, 4191);
					AddItem(360, 198, 4117);
					AddItem(367, 270, 4192);

					string tailoring = "Alfaiataria é a habilidade de pegar tecido e fazer roupas. Usando um kit de costura, você pode usar tecido e transformá-lo em itens como vestes, calças ou chapéus. Quanto melhor o tecido, melhor a roupa que você pode criar. Os tipos de tecido que se pode encontrar podem ser vistos na próxima página. Você também pode usar tesouras em roupas existentes e, se sua habilidade for alta o suficiente, elas serão transformadas em tecido utilizável. Você pode tosquiar ovelhas com uma arma cortante para obter lã clicando duas vezes na arma e depois selecionando a ovelha.";
					string cloth = "Você também pode encontrar jardins que cultivam algodão e linho. Você pode colhê-los usando-os ou passando por cima deles. As plantas serão coletadas em sua mochila. Após colhidas, você pode usá-las em uma roda de fiar para fazer fio. Quando tiver o fio, você pode usá-lo em um tear para fazer tecido, usando o fio e depois selecionando o tear. Você também pode cortar o tecido em ataduras se precisar delas.";
					
					AddHtml( 122, 80, 200, 310, @"<BODY><BASEFONT Color=" + color + ">" + tailoring + "</BASEFONT></BODY>", (bool)false, (bool)false);
					AddHtml( 415, 80, 200, 310, @"<BODY><BASEFONT Color=" + color + ">" + cloth + "</BASEFONT></BODY>", (bool)false, (bool)false);
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

					m_Mobile.SendGump( new LearnTailorGump( m_Mobile, m_Book, m_Page ) );
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
				e.CloseGump( typeof( LearnTailorGump ) );
				e.SendGump( new LearnTailorGump( e, this, 1 ) );
				Server.Gumps.MyLibrary.readBook ( this, e );
			}
		}

		public LearnTailorBook(Serial serial) : base(serial)
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
				Name = "Tailoring the Cloth";
			}
		}
	}
}