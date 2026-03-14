using System;
using Server;
using Server.Items;
using System.Text;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Commands;
using Server.Misc;

namespace Server.Items
{
	public class BookDruidBrewing : Item
	{
		[Constructable]
		public BookDruidBrewing( ) : base( 0x5688 )
		{
			Weight = 1.0;
			Name = "Druidic Herbalism";
			Hue = 0x85D;
		}

		public class BookGump : Gump
		{
			public BookGump( Mobile from, int page ): base( 100, 100 )
			{
				string color = "#80d080";
				from.SendSound( 0x55 );

				this.Closable=true;
				this.Disposable=true;
				this.Dragable=true;
				this.Resizable=false;

				AddPage(0);

				AddImage(0, 0, 7005, 2936);
				AddImage(0, 0, 7006);
				AddImage(0, 0, 7024, 2736);
				AddImage(77, 98, 7054);
				AddImage(368, 98, 7054);

				int prev = page - 1;
					if ( prev < 1 ){ prev = 99; }
				int next = page + 1;

				AddButton(72, 45, 4014, 4014, prev, GumpButtonType.Reply, 0);
				AddButton(590, 48, 4005, 4005, next, GumpButtonType.Reply, 0);

				int potion = 0;

				if ( page == 2 ){ potion = 2; }
				else if ( page == 3 ){ potion = 4; }
				else if ( page == 4 ){ potion = 6; }
				else if ( page == 5 ){ potion = 8; }
				else if ( page == 6 ){ potion = 10; }
				else if ( page == 7 ){ potion = 12; }
				else if ( page == 8 ){ potion = 14; }
				else if ( page == 9 ){ potion = 16; }
				else if ( page == 10 ){ potion = 18; }

				// --------------------------------------------------------------------------------

				if ( page == 1 )
				{
					AddHtml( 107, 46, 186, 20, @"<BODY><BASEFONT Color=" + color + "><CENTER>HERBALISMO DRUIDICO</CENTER></BASEFONT></BODY>", (bool)false, (bool)false);
					AddHtml( 398, 48, 186, 20, @"<BODY><BASEFONT Color=" + color + "><CENTER>HERBALISMO DRUIDICO</CENTER></BASEFONT></BODY>", (bool)false, (bool)false);

					AddHtml( 78, 75, 248, 318, @"<BODY><BASEFONT Color=" + color + ">Herbalismo Druídico é a arte de tomar reagentes naturais e criar misturas que druidas podem usar. Você usaria sua habilidade de druidismo para criar e usar as poções, mas alguma veterinária é necessária para ajudar a criá-las e torná-las mais eficazes em alguns casos. Este livro explica as várias poções que você pode fazer, bem como informações adicionais para gerenciar essas misturas efetivamente. Diferente de outras poções, estas requerem frascos pois o líquido precisa de um vidro mais grosso para armazenar, pois é ácido o suficiente para dissolver vidro de garrafa e até mesmo a madeira de um barril.</BASEFONT></BODY>", (bool)false, (bool)false);

					AddHtml( 372, 75, 248, 318, @"<BODY><BASEFONT Color=" + color + ">Você precisará de um pequeno caldeirão para preparar estas poções. Você também pode obter uma bolsa de cinto para armazenar os ingredientes, caldeirões, frascos, poções e este livro para facilitar o transporte. Clique uma vez nesta bolsa para organizá-la para um uso mais fácil das poções.</BASEFONT></BODY>", (bool)false, (bool)false);
				}
				else
				{
					AddHtml( 107, 46, 186, 20, @"<BODY><BASEFONT Color=" + color + "><CENTER>" + potionInfo( potion, 1 ) + "</CENTER></BASEFONT></BODY>", (bool)false, (bool)false);

					AddHtml( 73, 72, 187, 20, @"<BODY><BASEFONT Color=" + color + ">Druidismo:</BASEFONT></BODY>", (bool)false, (bool)false);
					AddHtml( 267, 72, 47, 20, @"<BODY><BASEFONT Color=" + color + ">" + potionInfo( potion, 4 ) + "</BASEFONT></BODY>", (bool)false, (bool)false);

					AddHtml( 73, 98, 187, 20, @"<BODY><BASEFONT Color=" + color + ">Veterinária:</BASEFONT></BODY>", (bool)false, (bool)false);
					AddHtml( 267, 98, 47, 20, @"<BODY><BASEFONT Color=" + color + ">" + potionInfo( potion, 5 ) + "</BASEFONT></BODY>", (bool)false, (bool)false);

					AddImage(77, 128, Int32.Parse( potionInfo( potion, 2 ) ) );
					AddHtml( 133, 139, 187, 20, @"<BODY><BASEFONT Color=" + color + ">Ingredientes</BASEFONT></BODY>", (bool)false, (bool)false);

					AddHtml( 73, 180, 246, 20, @"<BODY><BASEFONT Color=" + color + ">" + potionInfo( potion, 6 ) + "</BASEFONT></BODY>", (bool)false, (bool)false);
					AddHtml( 73, 206, 246, 20, @"<BODY><BASEFONT Color=" + color + ">" + potionInfo( potion, 7 ) + "</BASEFONT></BODY>", (bool)false, (bool)false);
					AddHtml( 73, 232, 246, 20, @"<BODY><BASEFONT Color=" + color + ">" + potionInfo( potion, 8 ) + "</BASEFONT></BODY>", (bool)false, (bool)false);

					AddHtml( 73, 258, 245, 133, @"<BODY><BASEFONT Color=" + color + ">" + potionInfo( potion, 3 ) + "</BASEFONT></BODY>", (bool)false, (bool)false);

					///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

					potion++;

					AddHtml( 398, 48, 186, 20, @"<BODY><BASEFONT Color=" + color + "><CENTER>" + potionInfo( potion, 1 ) + "</CENTER></BASEFONT></BODY>", (bool)false, (bool)false);

					AddHtml( 366, 72, 187, 20, @"<BODY><BASEFONT Color=" + color + ">Druidismo:</BASEFONT></BODY>", (bool)false, (bool)false);
					AddHtml( 560, 72, 47, 20, @"<BODY><BASEFONT Color=" + color + ">" + potionInfo( potion, 4 ) + "</BASEFONT></BODY>", (bool)false, (bool)false);

					AddHtml( 366, 98, 187, 20, @"<BODY><BASEFONT Color=" + color + ">Veterinária:</BASEFONT></BODY>", (bool)false, (bool)false);
					AddHtml( 560, 98, 47, 20, @"<BODY><BASEFONT Color=" + color + ">" + potionInfo( potion, 5 ) + "</BASEFONT></BODY>", (bool)false, (bool)false);

					AddImage(366, 128, Int32.Parse( potionInfo( potion, 2 ) ) );
					AddHtml( 422, 139, 187, 20, @"<BODY><BASEFONT Color=" + color + ">Ingredientes</BASEFONT></BODY>", (bool)false, (bool)false);

					AddHtml( 366, 180, 246, 20, @"<BODY><BASEFONT Color=" + color + ">" + potionInfo( potion, 6 ) + "</BASEFONT></BODY>", (bool)false, (bool)false);
					AddHtml( 366, 206, 246, 20, @"<BODY><BASEFONT Color=" + color + ">" + potionInfo( potion, 7 ) + "</BASEFONT></BODY>", (bool)false, (bool)false);
					AddHtml( 366, 232, 246, 20, @"<BODY><BASEFONT Color=" + color + ">" + potionInfo( potion, 8 ) + "</BASEFONT></BODY>", (bool)false, (bool)false);

					AddHtml( 366, 258, 245, 133, @"<BODY><BASEFONT Color=" + color + ">" + potionInfo( potion, 3 ) + "</BASEFONT></BODY>", (bool)false, (bool)false);
				}
			}

			public override void OnResponse( NetState state, RelayInfo info )
			{
				Mobile from = state.Mobile;
				int page = info.ButtonID;
					if ( page == 99 ){ page = 9; }
					else if ( page > 9 ){ page = 1; }

				if ( info.ButtonID > 0 )
				{
					from.SendGump( new BookGump( from, page ) );
				}
				else
					from.SendSound( 0x55 );
			}

			public static string potionDesc( int potion )
			{
				string txt = "";

				txt = "Isto é criado a partir do herbalismo druídico: " + potionInfo( potion, 3 ) + " Para usá-lo, deve-se ter " + potionInfo( potion, 4 ) + " em Druidismo e " + potionInfo( potion, 5 ) + " em Veterinária.";

				return txt;
			}

			public static string potionInfo( int page, int val )
			{
				string txtName = "";
				string txtIcon = "";
				string txtInfo = "";
				string txtSklA = "";
				string txtSklB = "";
				string txtIngA = "";
				string txtIngB = "";
				string txtIngC = "";

				if ( page == 2 ){ txtName = "Pedra em um Frasco"; txtIcon = "11446"; txtInfo = "Despeja uma pedra mágica que atrai todos os animais próximos para ela."; txtSklA = "10"; txtSklB = "5"; txtIngA = "Cristal da Lua"; txtIngB = "Viúva Prateada"; txtIngC = ""; }
				else if ( page == 3 ){ txtName = "Mistura de Passagem da Natureza"; txtIcon = "11449"; txtInfo = "Transforma alguém em pétalas de flor e as carrega no vento para uma localização de runa mágica."; txtSklA = "15"; txtSklB = "10"; txtIngA = "Sal Marinho"; txtIngB = "Ovo de Fada"; txtIngC = ""; }
				else if ( page == 4 ){ txtName = "Líquido do Escudo da Terra"; txtIcon = "11450"; txtInfo = "Faz uma parede de folhagem crescer, bloqueando o caminho de outros."; txtSklA = "20"; txtSklB = "15"; txtIngA = "Ginseng"; txtIngB = "Pérola Negra"; txtIngC = ""; }
				else if ( page == 5 ){ txtName = "Óleo de Proteção da Floresta"; txtIcon = "11454"; txtInfo = "Aumenta sua proteção tornando sua pele como a casca de uma árvore antiga."; txtSklA = "25"; txtSklB = "20"; txtIngA = "Alho"; txtIngB = "Bagas do Pântano"; txtIngC = ""; }
				else if ( page == 6 ){ txtName = "Poção da Ascensão da Pedra"; txtIcon = "11451"; txtInfo = "Faz pedras emergirem do chão, prendendo seus inimigos."; txtSklA = "30"; txtSklB = "25"; txtIngA = "Casco de Besouro"; txtIngB = "Sal Marinho"; txtIngC = ""; }
				else if ( page == 7 ){ txtName = "Mistura de Raízes Agarrantes"; txtIcon = "11443"; txtInfo = "Libera raízes do chão para emaranhar um inimigo."; txtSklA = "35"; txtSklB = "30"; txtIngA = "Raiz de Mandrágora"; txtIngB = "Ginseng"; txtIngC = ""; }
				else if ( page == 8 ){ txtName = "Óleo de Marcação Druídica"; txtIcon = "11439"; txtInfo = "Marca uma runa mágica com sua localização, que você pode usar magias de recall para transportar posteriormente."; txtSklA = "40"; txtSklB = "35"; txtIngA = "Pérola Negra"; txtIngB = "Olho de Sapo"; txtIngC = ""; }
				else if ( page == 9 ){ txtName = "Elixir de Cura Herbácea"; txtIcon = "11444"; txtInfo = "Cura o alvo de todas as enfermidades."; txtSklA = "45"; txtSklB = "40"; txtIngA = "Lótus Vermelho"; txtIngB = "Alho"; txtIngC = ""; }
				else if ( page == 10 ){ txtName = "Óleo de Camuflagem da Floresta"; txtIcon = "11442"; txtInfo = "Permite que alguém se misture perfeitamente com a floresta, fazendo os inimigos perderem sua visão."; txtSklA = "50"; txtSklB = "45"; txtIngA = "Viúva Prateada"; txtIngB = "Meimendro"; txtIngC = ""; }
				else if ( page == 11 ){ txtName = "Frasco de Vaga-lumes"; txtIcon = "11445"; txtInfo = "Libera vaga-lumes para distrair um inimigo da batalha."; txtSklA = "55"; txtSklB = "50"; txtIngA = "Seda de Aranha"; txtIngB = "Asas de Borboleta"; txtIngC = ""; }
				else if ( page == 12 ){ txtName = "Crescimento de Portal de Cogumelos"; txtIcon = "11448"; txtInfo = "Usando uma runa mágica, este líquido faz cogumelos mágicos crescerem um portal para a localização rúnica."; txtSklA = "60"; txtSklB = "55"; txtIngA = "Musgo Sangrento"; txtIngB = "Olho de Sapo"; txtIngC = ""; }
				else if ( page == 13 ){ txtName = "Frasco de Insetos"; txtIcon = "11441"; txtInfo = "Libera um enxame de insetos do frasco que mordem e picam inimigos próximos."; txtSklA = "65"; txtSklB = "60"; txtIngA = "Asas de Borboleta"; txtIngB = "Casco de Besouro"; txtIngC = ""; }
				else if ( page == 14 ){ txtName = "Fada em um Frasco"; txtIcon = "11440"; txtInfo = "Libera uma fada do frasco para ajudar o aventureiro em sua jornada."; txtSklA = "70"; txtSklB = "65"; txtIngA = "Ovo de Fada"; txtIngB = "Cristal da Lua"; txtIngC = ""; }
				else if ( page == 15 ){ txtName = "Fertilizante de Treant"; txtIcon = "11452"; txtInfo = "Faz uma árvore viva crescer e atacar inimigos próximos."; txtSklA = "75"; txtSklB = "70"; txtIngA = "Bagas do Pântano"; txtIngB = "Raiz de Mandrágora"; txtIngC = ""; }
				else if ( page == 16 ){ txtName = "Fluido Vulcânico"; txtIcon = "11453"; txtInfo = "Faz lava derretida irromper do chão, atingindo todos os inimigos próximos."; txtSklA = "80"; txtSklB = "75"; txtIngA = "Enxofre"; txtIngB = "Cinza Sulfúrea"; txtIngC = ""; }
				else if ( page == 17 ){ txtName = "Frasco de Lama Mágica"; txtIcon = "11447"; txtInfo = "Despeja lama mística em sua mochila, que irá ressuscitá-lo alguns momentos após perder sua vida. Você também pode ressuscitar outros diretamente."; txtSklA = "85"; txtSklB = "80"; txtIngA = "Meimendro"; txtIngB = "Lótus Vermelho"; txtIngC = ""; }
				if ( val == 1 )
					return txtName;
				else if ( val == 2 )
					return txtIcon;
				else if ( val == 3 )
					return txtInfo;
				else if ( val == 4 )
					return txtSklA;
				else if ( val == 5 )
					return txtSklB;
				else if ( val == 6 )
					return txtIngA;
				else if ( val == 7 )
					return txtIngB;

				return txtIngC;
			}
		}

		public override void OnDoubleClick( Mobile e )
		{
			if ( !IsChildOf( e.Backpack ) ) 
			{
				e.SendMessage( "Isto precisa estar em sua mochila para ser lido." );
			}
			else
			{
				e.CloseGump( typeof( BookGump ) );
				e.SendGump( new BookGump( e, 1 ) );
			}
		}

		public BookDruidBrewing(Serial serial) : base(serial)
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