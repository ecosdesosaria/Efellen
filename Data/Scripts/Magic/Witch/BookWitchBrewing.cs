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
	public class BookWitchBrewing : Item
	{
		[Constructable]
		public BookWitchBrewing( ) : base( 0x5689 )
		{
			Weight = 1.0;
			Name = "The Witch's Brew";
			Hue = 0x9A2;
		}

		public class BookGump : Gump
		{
			public BookGump( Mobile from, int page ): base( 100, 100 )
			{
				string color = "#d89191";
				from.SendSound( 0x55 );

				this.Closable=true;
				this.Disposable=true;
				this.Dragable=true;
				this.Resizable=false;

				AddPage(0);

				AddImage(0, 0, 7005, 2845);
				AddImage(0, 0, 7006);
				AddImage(0, 0, 7024, 2736);
				AddImage(87, 117, 7053);
				AddImage(382, 117, 7053);

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
				else if ( page == 7 ){ potion = 11; }
				else if ( page == 8 ){ potion = 12; }
				else if ( page == 9 ){ potion = 14; }
				else if ( page == 10 ){ potion = 16; }

				// --------------------------------------------------------------------------------

				if ( page == 1 )
				{
					AddHtml( 107, 46, 186, 20, @"<BODY><BASEFONT Color=" + color + "><CENTER>THE WITCH'S BREW</CENTER></BASEFONT></BODY>", (bool)false, (bool)false);
					AddHtml( 398, 48, 186, 20, @"<BODY><BASEFONT Color=" + color + "><CENTER>THE WITCH'S BREW</CENTER></BASEFONT></BODY>", (bool)false, (bool)false);

					AddHtml( 78, 75, 248, 318, @"<BODY><BASEFONT Color=" + color + ">O preparo de bruxaria é a arte de pegar reagentes mórbidos e criar poções que necromantes podem usar em suas magias sombrias. Você usaria sua habilidade de forense para criar as poções e sua habilidade de necromancia para usá-las. Este livro explica as várias misturas que você pode fazer, bem como informações adicionais para gerenciar essas poções efetivamente. Ao contrário de outras poções, estas exigem jarros, pois o líquido precisa de um vidro mais grosso para armazenar, já que é ácido o suficiente para dissolver vidro de garrafa e até a madeira de um barril.</BASEFONT></BODY>", (bool)false, (bool)false);

					AddHtml( 372, 75, 248, 318, @"<BODY><BASEFONT Color=" + color + ">Você precisará de um pequeno caldeirão para preparar essas poções. Você também pode obter uma bolsa de cinto para guardar os ingredientes, caldeirões, jarros, poções e este livro para facilitar o transporte. Clique uma vez nesta bolsa para organizá-la e facilitar o uso das misturas.</BASEFONT></BODY>", (bool)false, (bool)false);
				}
				else
				{
					AddHtml( 107, 46, 186, 20, @"<BODY><BASEFONT Color=" + color + "><CENTER>" + potionInfo( potion, 1 ) + "</CENTER></BASEFONT></BODY>", (bool)false, (bool)false);

					AddHtml( 73, 72, 187, 20, @"<BODY><BASEFONT Color=" + color + ">Forensics:</BASEFONT></BODY>", (bool)false, (bool)false);
					AddHtml( 267, 72, 47, 20, @"<BODY><BASEFONT Color=" + color + ">" + potionInfo( potion, 4 ) + "</BASEFONT></BODY>", (bool)false, (bool)false);

					AddHtml( 73, 98, 187, 20, @"<BODY><BASEFONT Color=" + color + ">Necromancy:</BASEFONT></BODY>", (bool)false, (bool)false);
					AddHtml( 267, 98, 47, 20, @"<BODY><BASEFONT Color=" + color + ">" + potionInfo( potion, 5 ) + "</BASEFONT></BODY>", (bool)false, (bool)false);

					AddImage(77, 128, Int32.Parse( potionInfo( potion, 2 ) ) );
					AddHtml( 133, 139, 187, 20, @"<BODY><BASEFONT Color=" + color + ">Ingredients</BASEFONT></BODY>", (bool)false, (bool)false);

					AddHtml( 73, 180, 246, 20, @"<BODY><BASEFONT Color=" + color + ">" + potionInfo( potion, 6 ) + "</BASEFONT></BODY>", (bool)false, (bool)false);
					AddHtml( 73, 206, 246, 20, @"<BODY><BASEFONT Color=" + color + ">" + potionInfo( potion, 7 ) + "</BASEFONT></BODY>", (bool)false, (bool)false);
					AddHtml( 73, 232, 246, 20, @"<BODY><BASEFONT Color=" + color + ">" + potionInfo( potion, 8 ) + "</BASEFONT></BODY>", (bool)false, (bool)false);

					AddHtml( 73, 258, 245, 133, @"<BODY><BASEFONT Color=" + color + ">" + potionInfo( potion, 3 ) + "</BASEFONT></BODY>", (bool)false, (bool)false);

					///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

					potion++;

					AddHtml( 398, 48, 186, 20, @"<BODY><BASEFONT Color=" + color + "><CENTER>" + potionInfo( potion, 1 ) + "</CENTER></BASEFONT></BODY>", (bool)false, (bool)false);

					AddHtml( 366, 72, 187, 20, @"<BODY><BASEFONT Color=" + color + ">Forensics:</BASEFONT></BODY>", (bool)false, (bool)false);
					AddHtml( 560, 72, 47, 20, @"<BODY><BASEFONT Color=" + color + ">" + potionInfo( potion, 4 ) + "</BASEFONT></BODY>", (bool)false, (bool)false);

					AddHtml( 366, 98, 187, 20, @"<BODY><BASEFONT Color=" + color + ">Necromancy:</BASEFONT></BODY>", (bool)false, (bool)false);
					AddHtml( 560, 98, 47, 20, @"<BODY><BASEFONT Color=" + color + ">" + potionInfo( potion, 5 ) + "</BASEFONT></BODY>", (bool)false, (bool)false);

					AddImage(366, 128, Int32.Parse( potionInfo( potion, 2 ) ) );
					AddHtml( 422, 139, 187, 20, @"<BODY><BASEFONT Color=" + color + ">Ingredients</BASEFONT></BODY>", (bool)false, (bool)false);

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

				txt = "Isto é criado a partir de uma poção de bruxa: " + potionInfo( potion, 3 ) + " Para usá-lo, é preciso ter " + potionInfo( potion, 4 ) + " em Forense e " + potionInfo( potion, 5 ) + " em Necromancia.";

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

				if ( page == 2 ){ txtName = "Eyes of the Dead Mixture"; txtIcon = "11460"; txtInfo = "Dá a visão dos mortos-vivos, permitindo enxergar no escuro."; txtSklA = "10"; txtSklB = "5"; txtIngA = "Mummy Wrap"; txtIngB = "Eye of Toad"; txtIngC = ""; }
				else if ( page == 3 ){ txtName = "Tomb Raiding Concoction"; txtIcon = "11468"; txtInfo = "Invoca os espíritos para destrancar algo para você."; txtSklA = "15"; txtSklB = "10"; txtIngA = "Maggot"; txtIngB = "Beetle Shell"; txtIngC = ""; }
				else if ( page == 4 ){ txtName = "Disease Draught"; txtIcon = "11458"; txtInfo = "Faz com que alguém sofra de uma doença venenosa."; txtSklA = "20"; txtSklB = "15"; txtIngA = "Violet Fungus"; txtIngB = "Nox Crystal"; txtIngC = ""; }
				else if ( page == 5 ){ txtName = "Phantasm Elixir"; txtIcon = "11465"; txtInfo = "Invoca um espírito para desativar uma armadilha."; txtSklA = "25"; txtSklB = "20"; txtIngA = "Dried Toad"; txtIngB = "Gargoyle Ear"; txtIngC = ""; }
				else if ( page == 6 ){ txtName = "Retched Air Elixir"; txtIcon = "11466"; txtInfo = "Cria uma explosão de gás nocivo."; txtSklA = "30"; txtSklB = "25"; txtIngA = "Black Sand"; txtIngB = "Grave Dust"; txtIngC = ""; }
				else if ( page == 7 ){ txtName = "Lich Leech Mixture"; txtIcon = "11464"; txtInfo = "Absorve mana do alvo, dando a você em troca."; txtSklA = "35"; txtSklB = "30"; txtIngA = "Dried Toad"; txtIngB = "Red Lotus"; txtIngC = ""; }
				else if ( page == 8 ){ txtName = "Wall of Spike Draught"; txtIcon = "11470"; txtInfo = "Cria uma parede protetora de espinhos."; txtSklA = "40"; txtSklB = "35"; txtIngA = "Bitter Root"; txtIngB = "Pig Iron"; txtIngC = ""; }
				else if ( page == 9 ){ txtName = "Disease Curing Concoction"; txtIcon = "11459"; txtInfo = "Cura alguém de doenças venenosas."; txtSklA = "45"; txtSklB = "40"; txtIngA = "Wolfsbane"; txtIngB = "Swamp Berries"; txtIngC = ""; }
				else if ( page == 10 ){ txtName = "Blood Pact Elixir"; txtIcon = "11456"; txtInfo = "Tira um pouco da sua vida e a concede a outro."; txtSklA = "50"; txtSklB = "45"; txtIngA = "Blood Rose"; txtIngB = "Daemon Blood"; txtIngC = ""; }
				else if ( page == 11 ){ txtName = "Spectre Shadow Elixir"; txtIcon = "11467"; txtInfo = "Transforma o corpo em uma forma fantasmagórica invisível que não pode ser vista."; txtSklA = "55"; txtSklB = "50"; txtIngA = "Violet Fungus"; txtIngB = "Silver Widow"; txtIngC = ""; }
				else if ( page == 12 ){ txtName = "Ghost Phase Concoction"; txtIcon = "11461"; txtInfo = "Transforma seu corpo em matéria fantasmagórica que reaparece em um local próximo."; txtSklA = "60"; txtSklB = "55"; txtIngA = "Bitter Root"; txtIngB = "Moon Crystal"; txtIngC = ""; }
				else if ( page == 13 ){ txtName = "Demonic Fire Ooze"; txtIcon = "11457"; txtInfo = "Inflama uma runa marcada com poder para transportá-lo para aquele local."; txtSklA = "65"; txtSklB = "60"; txtIngA = "Maggot"; txtIngB = "Black Pearl"; txtIngC = ""; }
				else if ( page == 14 ){ txtName = "Ghostly Images Draught"; txtIcon = "11462"; txtInfo = "Cria uma imagem ilusória sua, distraindo seus inimigos."; txtSklA = "70"; txtSklB = "65"; txtIngA = "Mummy Wrap"; txtIngB = "Bloodmoss"; txtIngC = ""; }
				else if ( page == 15 ){ txtName = "Hellish Branding Ooze"; txtIcon = "11463"; txtInfo = "Marca um local de runa com símbolos do mal, para que você possa usar magia de recall nela para retornar àquele local."; txtSklA = "75"; txtSklB = "70"; txtIngA = "Werewolf Claw"; txtIngB = "Brimstone"; txtIngC = ""; }
				else if ( page == 16 ){ txtName = "Black Gate Draught"; txtIcon = "11455"; txtInfo = "Usa uma runa mágica para criar um portal negro horripilante. Aqueles que entrarem aparecerão no local rúnico."; txtSklA = "80"; txtSklB = "75"; txtIngA = "Black Sand"; txtIngB = "Wolfsbane"; txtIngC = "Pixie Skull"; }
				else if ( page == 17 ){ txtName = "Vampire Blood Draught"; txtIcon = "11469"; txtInfo = "Despeja sangue de vampiro em sua mochila, que o ressuscitará alguns momentos após perder a vida. Você também pode ressuscitar outros diretamente."; txtSklA = "85"; txtSklB = "80"; txtIngA = "Werewolf Claw"; txtIngB = "Bat Wing"; txtIngC = "Blood Rose"; }

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

		public BookWitchBrewing(Serial serial) : base(serial)
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