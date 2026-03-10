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
	public class LearnLeatherBook : Item
	{
		public override Catalogs DefaultCatalog{ get{ return Catalogs.Book; } }

		[Constructable]
		public LearnLeatherBook( ) : base( 0x1C11 )
		{
			ItemID = RandomThings.GetRandomBookItemID();
			Hue = Utility.RandomColor(0);
			Weight = 1.0;
			Name = "Leather & Bone Crafts";
		}

		public class LearnLeatherGump : Gump
		{
			private Item m_Book;
			private int m_Page;
			private Mobile m_Mobile;

			public LearnLeatherGump( Mobile from, Item book, int page ): base( 50, 50 )
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
					CraftResource res = CraftResource.RegularLeather;

					int modX = 289;
					int modY = 36;

					while ( amt > 0 )
					{
						amt--; itm++;

						AddItem( x, y, 4199, CraftResources.GetHue( res ) );
						AddHtml( x+44, y, 200, 20, @"<BODY><BASEFONT Color=" + color + ">" + CraftResources.GetName( res ) + "</BASEFONT></BODY>", (bool)false, (bool)false);

						y += modY;

						if ( itm == 9 ){ y = 75; x += modX; }

						res = (CraftResource)( (int)res + 1 );
					}
				}
				else if ( m_Page == 3 )
				{
					int amt = 18;
					int itm = 0;

					int x = 75;
					int y = 75;
					CraftResource res = CraftResource.BrittleSkeletal;

					int modX = 289;
					int modY = 36;

					while ( amt > 0 )
					{
						amt--; itm++;

						AddItem( x, y, 8899, CraftResources.GetHue( res ) );
						AddHtml( x+44, y, 200, 20, @"<BODY><BASEFONT Color=" + color + ">" + CraftResources.GetName( res ) + "</BASEFONT></BODY>", (bool)false, (bool)false);

						y += modY;

						if ( itm == 9 ){ y = 75; x += modX; }

						res = (CraftResource)( (int)res + 1 );
					}
				}
				else
				{
					AddItem(84, 82, 26362, 0xB61);
					AddItem(72, 131, 4216);
					AddItem(371, 86, 26139, 0xB61);
					AddItem(73, 178, 4199);
					AddItem(75, 232, 3999);
					AddItem(371, 138, 8899);

					string leather = "Existem vários tipos de couro que você pode adquirir ao esfolar criaturas por toda a terra. Alguns couros estão listados na próxima página, que um alfaiate pode usar. Peles podem ser obtidas ao esfolar certas criaturas clicando duas vezes em uma arma cortante e depois selecionando um cadáver. Essas peles podem então ser cortadas com tesouras e transformadas em couro. Depois, ferramentas de curtimento podem ser usadas para criar várias armaduras e bolsas.";
					string bone = "Existem vários tipos de ossos que você pode adquirir ao desossar criaturas por toda a terra. Alguns ossos estão listados na página seguinte, que um agente funerário pode usar. Ossos podem ser obtidos ao desossar certas criaturas clicando duas vezes em uma arma cortante e depois selecionando um cadáver. Esses ossos podem então ser usados por um kit de agente funerário para criar vários tipos de armaduras.";
					AddHtml( 122, 80, 200, 300, @"<BODY><BASEFONT Color=" + color + ">" + leather + "</BASEFONT></BODY>", (bool)false, (bool)false);
					AddHtml( 415, 80, 200, 300, @"<BODY><BASEFONT Color=" + color + ">" + bone + "</BASEFONT></BODY>", (bool)false, (bool)false);
				}
			}

			public override void OnResponse( NetState state, RelayInfo info ) 
			{
				if ( info.ButtonID > 0 )
				{
					m_Page = info.ButtonID;
					if ( m_Page >= 900 )
						m_Page = 3;
					else if ( m_Page > 3 )
						m_Page = 1;
					else
						m_Page = info.ButtonID;

					m_Mobile.SendGump( new LearnLeatherGump( m_Mobile, m_Book, m_Page ) );
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
				e.CloseGump( typeof( LearnLeatherGump ) );
				e.SendGump( new LearnLeatherGump( e, this, 1 ) );
				Server.Gumps.MyLibrary.readBook ( this, e );
			}
		}

		public LearnLeatherBook(Serial serial) : base(serial)
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
				Name = "Leather & Bone Crafts";
			}
		}
	}
}