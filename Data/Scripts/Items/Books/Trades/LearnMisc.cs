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
	public class LearnMiscBook : Item
	{
		public override Catalogs DefaultCatalog{ get{ return Catalogs.Book; } }

		[Constructable]
		public LearnMiscBook( ) : base( 0x1C11 )
		{
			ItemID = RandomThings.GetRandomBookItemID();
			Hue = Utility.RandomColor(0);
			Weight = 1.0;
			Name = "Skinning & Carving";
		}

		public class LearnMiscGump : Gump
		{
			private Item m_Book;
			private Mobile m_Mobile;

			public LearnMiscGump( Mobile from, Item book ): base( 50, 50 )
			{
				m_Book = book;
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

				AddHtml( 106, 44, 215, 20, @"<BODY><BASEFONT Color=" + color + ">" + m_Book.Name + "</BASEFONT></BODY>", (bool)false, (bool)false);

				AddItem(76, 192, 7123);
				AddItem(74, 116, 2545);
				AddItem(359, 347, 7137, 0x512);
				AddItem(76, 320, 5981, 0x981);
				AddItem(70, 279, 9908, 0x806);
				AddItem(358, 140, 4199, 0xB80);
				AddItem(73, 153, 2489);
				AddItem(356, 84, 4216, 0x69E);
				AddItem(83, 86, 3921);
				AddItem(354, 304, 7144);
				AddItem(362, 197, 8536, 0x424);
				AddItem(82, 239, 8899, 0x43F);
				AddItem(351, 243, 6585, 0x5CE);
				AddItem(68, 365, 3576);

				string p1 = "Use um item cortante, como uma adaga ou faca, em um cadáver clicando duas vezes no item e selecionando o cadáver. Se houver algo para ser retirado dele, aparecerá em sua mochila. Você pode obter itens como carne, penas, ossos, escamas, tecido, lã, peles, couros, rochas, pedras, metal ou madeira. Quanto melhor sua habilidade de forense, mais você pode retirar de um cadáver. Quaisquer cadáveres que possam ser retirados indicarão isso quando você passar o cursor sobre eles.";

				string p2 = "Animais são a melhor fonte de carne, enquanto penas vêm de criaturas parecidas com pássaros. Ossos vêm de muitas criaturas diferentes, e escamas vêm de répteis. Tecido é raro de encontrar, mas fantasmas frequentemente o possuem. Lã pode vir de ovelhas e couros podem vir de qualquer criatura de pele grossa. Rochas e metais são frequentemente encontrados em golems e elementais de pedra, enquanto madeira é frequentemente encontrada em ents e ceifadores.";
				
				AddHtml( 122, 80, 200, 300, @"<BODY><BASEFONT Color=" + color + ">" + p1 + "</BASEFONT></BODY>", (bool)false, (bool)false);
				AddHtml( 415, 80, 200, 300, @"<BODY><BASEFONT Color=" + color + ">" + p2 + "</BASEFONT></BODY>", (bool)false, (bool)false);
			}

			public override void OnResponse( NetState state, RelayInfo info ) 
			{
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
				e.CloseGump( typeof( LearnMiscGump ) );
				e.SendGump( new LearnMiscGump( e, this ) );
				Server.Gumps.MyLibrary.readBook ( this, e );
			}
		}

		public LearnMiscBook(Serial serial) : base(serial)
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
				Name = "Skinning & Carving";
			}
		}
	}
}