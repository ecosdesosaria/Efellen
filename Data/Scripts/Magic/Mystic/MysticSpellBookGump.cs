using System; 
using System.Collections; 
using Server; 
using Server.Items; 
using Server.Misc; 
using Server.Network; 
using Server.Spells; 
using Server.Spells.Mystic; 
using Server.Prompts; 

namespace Server.Gumps 
{ 
	public class MysticSpellbookGump : Gump 
	{
		private MysticSpellbook m_Book; 
		private int m_Page; 
		private Map m_Map;
		private int m_X;
		private int m_Y;

		public bool HasSpell(Mobile from, int spellID)
		{
			if ( m_Book.RootParentEntity == from )
				return (m_Book.HasSpell(spellID));
			else
				return false;
		}

		public MysticSpellbookGump( Mobile from, MysticSpellbook book, int page ) : base( 100, 100 ) 
		{
			m_Book = book;
			m_Page = page;

			string color = "#d6c382";

			bool showScrollBar = true;

            this.Closable=true;
			this.Disposable=true;
			this.Dragable=true;
			this.Resizable=false;

			AddPage(0);

			int PriorPage = page - 1;
				if ( PriorPage < 1 ){ PriorPage = 12; }
			int NextPage = page + 1;

			AddImage(0, 0, 7005);
			AddImage(0, 0, 7006);
			AddImage(0, 0, 7024, 2736);
			AddButton(72, 45, 4014, 4014, PriorPage, GumpButtonType.Reply, 0);
			AddButton(590, 48, 4005, 4005, NextPage, GumpButtonType.Reply, 0);
			AddImage(83, 110, 7044);
			AddImage(380, 110, 7044);

			string abil_name = "";
			int abil_icon = 0;
			string abil_text = "";
			string abil_info = "<br><br>Para aprender os segredos desta habilidade, você precisa encontrar o seguinte local e abrir este livro lá para alcançar seu ki para iluminação:<br><br>";
			string abil_skil = "";
			string abil_mana = "";
			string abil_tith = "";
			int abil_spid = (page+248);

			if ( page == 2 ){ 		abil_name = "Projeção Astral";	abil_icon = 0x500E;	abil_skil = "80"; abil_mana = "50"; abil_tith = "300"; 
				m_Map = book.WritMap01;
				m_X = (int)( ( book.WritX101 + book.WritX201 ) / 2 );
				m_Y = (int)( ( book.WritY101 + book.WritY201 ) / 2 );
				abil_info += "Local: " + book.WritPlace01 + "<br><br>";
				abil_info += "Mundo: " + book.WritWorld01 + "<br><br>";
				abil_info += "Coordenadas: " + book.WritCoord01 + "<br>";
				abil_text = "Entre no plano astral onde sua alma é imune a danos. Enquanto você está neste estado, pode viajar livremente, mas sua interação com o mundo é mínima. Quanto melhor sua habilidade, mais tempo dura. Monges usam esta habilidade para viajar com segurança por áreas perigosas."; }
			else if ( page == 3 ){ 	abil_name = "Viagem Astral";		abil_icon = 0x410;	abil_skil = "50"; abil_mana = "40"; abil_tith = "35"; 
				m_Map = book.WritMap02;
				m_X = (int)( ( book.WritX102 + book.WritX202 ) / 2 );
				m_Y = (int)( ( book.WritY102 + book.WritY202 ) / 2 );
				abil_info += "Local: " + book.WritPlace02 + "<br><br>";
				abil_info += "Mundo: " + book.WritWorld02 + "<br><br>";
				abil_info += "Coordenadas: " + book.WritCoord02 + "<br>";
				abil_text = "Viaje através do plano astral para outro local com o uso de uma runa de recall mágica. A runa deve ser marcada por outros meios mágicos antes que você possa viajar para esse local. Se você deseja viajar usando um livro de runas, defina a localização padrão do seu livro de runas e então você pode mirar no livro enquanto usa esta habilidade."; }
			else if ( page == 4 ){ 	abil_name = "Criar Manto";			abil_icon = 0x15;	abil_skil = "25"; abil_mana = "20"; abil_tith = "150"; 
				m_Map = book.WritMap03;
				m_X = (int)( ( book.WritX103 + book.WritX203 ) / 2 );
				m_Y = (int)( ( book.WritY103 + book.WritY203 ) / 2 );
				abil_info += "Local: " + book.WritPlace03 + "<br><br>";
				abil_info += "Mundo: " + book.WritWorld03 + "<br><br>";
				abil_info += "Coordenadas: " + book.WritCoord03 + "<br>";
				abil_text = "Cria um manto que você precisará para usar as outras habilidades neste tomo. O manto terá poder baseado em sua habilidade geral como monge, e ninguém mais pode usar o manto. Você só pode ter um manto desses por vez, então criar um novo manto fará com que quaisquer outros que você possua voltem ao plano astral. Após a criação, clique uma vez no manto e selecione a opção 'Encantar' para gastar os pontos nos atributos que você deseja que o manto tenha."; }
			else if ( page == 5 ){ 	abil_name = "Toque Suave";			abil_icon = 0x971;	abil_skil = "30"; abil_mana = "25"; abil_tith = "15"; 
				m_Map = book.WritMap04;
				m_X = (int)( ( book.WritX104 + book.WritX204 ) / 2 );
				m_Y = (int)( ( book.WritY104 + book.WritY204 ) / 2 );
				abil_info += "Local: " + book.WritPlace04 + "<br><br>";
				abil_info += "Mundo: " + book.WritWorld04 + "<br><br>";
				abil_info += "Coordenadas: " + book.WritCoord04 + "<br>";
				abil_text = "Execute um toque suave, curando danos sofridos. Quanto maior sua habilidade, mais dano você curará com seu toque."; }
			else if ( page == 6 ){ 	abil_name = "Salto";					abil_icon = 0x4B2;	abil_skil = "35"; abil_mana = "20"; abil_tith = "10"; 
				m_Map = book.WritMap05;
				m_X = (int)( ( book.WritX105 + book.WritX205 ) / 2 );
				m_Y = (int)( ( book.WritY105 + book.WritY205 ) / 2 );
				abil_info += "Local: " + book.WritPlace05 + "<br><br>";
				abil_info += "Mundo: " + book.WritWorld05 + "<br><br>";
				abil_info += "Coordenadas: " + book.WritCoord05 + "<br>";
				abil_text = "Permite que você salte uma longa distância. Esta é uma ação rápida e pode permitir que um monge salte em direção a um oponente, salte para segurança ou salte sobre alguns obstáculos como rios e riachos."; }
			else if ( page == 7 ){ 	abil_name = "Explosão Psíquica";		abil_icon = 0x5DC2;	abil_skil = "30"; abil_mana = "35"; abil_tith = "15"; 
				m_Map = book.WritMap06;
				m_X = (int)( ( book.WritX106 + book.WritX206 ) / 2 );
				m_Y = (int)( ( book.WritY106 + book.WritY206 ) / 2 );
				abil_info += "Local: " + book.WritPlace06 + "<br><br>";
				abil_info += "Mundo: " + book.WritWorld06 + "<br><br>";
				abil_info += "Coordenadas: " + book.WritCoord06 + "<br>";
				abil_text = "Invoca seu Ki para realizar um ataque mental que causa uma quantidade de dano energético baseado em seus valores de luta de punhos e inteligência. Resistências Elementais podem reduzir o dano causado por este ataque."; }
			else if ( page == 8 ){ 	abil_name = "Muralha Psíquica";			abil_icon = 0x1A;	abil_skil = "60"; abil_mana = "45"; abil_tith = "500"; 
				m_Map = book.WritMap07;
				m_X = (int)( ( book.WritX107 + book.WritX207 ) / 2 );
				m_Y = (int)( ( book.WritY107 + book.WritY207 ) / 2 );
				abil_info += "Local: " + book.WritPlace07 + "<br><br>";
				abil_info += "Mundo: " + book.WritWorld07 + "<br><br>";
				abil_info += "Coordenadas: " + book.WritCoord07 + "<br>";
				abil_text = "Sua pura força de vontade cria uma barreira ao seu redor, desviando ataques mágicos. Isso não funciona contra magias estranhas como necromancia. Feitiços afetados frequentemente ricocheteiam de volta para o conjurador."; }
			else if ( page == 9 ){ 	abil_name = "Pureza Corporal";		abil_icon = 0x96D;	abil_skil = "40"; abil_mana = "35"; abil_tith = "25"; 
				m_Map = book.WritMap08;
				m_X = (int)( ( book.WritX108 + book.WritX208 ) / 2 );
				m_Y = (int)( ( book.WritY108 + book.WritY208 ) / 2 );
				abil_info += "Local: " + book.WritPlace08 + "<br><br>";
				abil_info += "Mundo: " + book.WritWorld08 + "<br><br>";
				abil_info += "Coordenadas: " + book.WritCoord08 + "<br>";
				abil_text = "Você pode limpar seu corpo de venenos com esta habilidade devido à sua disciplina física e, como tal, não pode ser usada para ajudar qualquer outra pessoa."; }
			else if ( page == 10 ){	abil_name = "Palma Trêmula";		abil_icon = 0x5001;	abil_skil = "20"; abil_mana = "20"; abil_tith = "20"; 
				m_Map = book.WritMap09;
				m_X = (int)( ( book.WritX109 + book.WritX209 ) / 2 );
				m_Y = (int)( ( book.WritY109 + book.WritY209 ) / 2 );
				abil_info += "Local: " + book.WritPlace09 + "<br><br>";
				abil_info += "Mundo: " + book.WritWorld09 + "<br><br>";
				abil_info += "Coordenadas: " + book.WritCoord09 + "<br>";
				abil_text = "Você deve estar usando algum tipo de luva de pugilista para esta habilidade funcionar. Ela aprimora temporariamente o tipo de dano que as luvas causam. O tipo de dano infligido ao atingir um alvo será convertido para o tipo de resistência mais fraco do alvo. A duração do efeito é afetada por sua habilidade em luta de punhos."; }
			else if ( page == 11 ){	abil_name = "Corredor do Vento";			abil_icon = 0x19;	abil_skil = "70"; abil_mana = "50"; abil_tith = "250"; 
				m_Map = book.WritMap10;
				m_X = (int)( ( book.WritX110 + book.WritX210 ) / 2 );
				m_Y = (int)( ( book.WritY110 + book.WritY210 ) / 2 );
				abil_info += "Local: " + book.WritPlace10 + "<br><br>";
				abil_info += "Mundo: " + book.WritWorld10 + "<br><br>";
				abil_info += "Coordenadas: " + book.WritCoord10 + "<br>";
				abil_text = "Esta habilidade permite que o monge corra tão rápido quanto uma montaria. Esta habilidade deve ser evitada se você já tiver uma montaria que está cavalgando, ou talvez tenha botas mágicas que permitem correr nesta velocidade. Usar esta habilidade em tais condições pode causar velocidades de viagem incomuns, então seja cauteloso.";
					if ( MySettings.S_NoMountsInCertainRegions )
					{
						abil_text = abil_text + " Esteja ciente ao explorar a terra, que existem algumas áreas onde você não pode usar esta habilidade. Estas são áreas como masmorras, cavernas e algumas áreas internas. Se você entrar em tal área, esta habilidade será prejudicada.";
					}
				}

			abil_info += "<br>Certifique-se de trazer um pergaminho em branco com você, para que possa escrever o que aprendeu. Você pode então colocar seus escritos dentro deste livro.<br>";
			if ( page == 1 )
			{
				AddHtml( 110, 47, 177, 20, @"<BODY><BASEFONT Color=" + color + "><CENTER>HABILIDADES DE MONGE</CENTER></BASEFONT></BODY>", (bool)false, (bool)false);
				AddHtml( 404, 47, 177, 20, @"<BODY><BASEFONT Color=" + color + "><CENTER>HABILIDADES DE MONGE</CENTER></BASEFONT></BODY>", (bool)false, (bool)false);

				int SpellsInBook = 10;
				int SafetyCatch = 0;
				int SpellsListed = 249;
				string SpellName = "";

				int nHTMLx = 130;
				int nHTMLy = 105;

				int nBUTTONx = 75;
				int nBUTTONy = 93;

				int iBUTTON = 1;

				while ( SpellsInBook > 0 )
				{
					SpellsListed++;
					SafetyCatch++;

					if ( this.HasSpell( from, SpellsListed ) )
					{
						SpellsInBook--;

						if ( SpellsListed == 250 ){ SpellName = "Astral Projection"; iBUTTON = 0x500E; }
						else if ( SpellsListed == 251 ){ SpellName = "Astral Travel"; iBUTTON = 0x410; }
						else if ( SpellsListed == 252 ){ SpellName = "Create Robe"; iBUTTON = 0x15; }
						else if ( SpellsListed == 253 ){ SpellName = "Gentle Touch"; iBUTTON = 0x971; }
						else if ( SpellsListed == 254 ){ SpellName = "Leap"; iBUTTON = 0x4B2; }
						else if ( SpellsListed == 255 ){ SpellName = "Psionic Blast"; iBUTTON = 0x5DC2; }
						else if ( SpellsListed == 256 ){ SpellName = "Psychic Wall"; iBUTTON = 0x1A; }
						else if ( SpellsListed == 257 ){ SpellName = "Purity of Body"; iBUTTON = 0x96D; }
						else if ( SpellsListed == 258 ){ SpellName = "Quivering Palm"; iBUTTON = 0x5001; }
						else if ( SpellsListed == 259 ){ SpellName = "Wind Runner"; iBUTTON = 0x19; }

						AddButton(nBUTTONx, nBUTTONy, iBUTTON, iBUTTON, SpellsListed, GumpButtonType.Reply, 0);
						AddImage(nBUTTONx, nBUTTONy, iBUTTON, 2422);
						AddHtml( nHTMLx, nHTMLy, 177, 20, @"<BODY><BASEFONT Color=" + color + ">" + SpellName + "</BASEFONT></BODY>", (bool)false, (bool)false);

						nHTMLy = nHTMLy + 65;
						if ( SpellsInBook == 5 ){ nHTMLx = 432; nHTMLy = 105; }

						nBUTTONy = nBUTTONy + 65;
						if ( SpellsInBook == 5 ){ nBUTTONx = 375; nBUTTONy = 93; }
					}

					if ( SafetyCatch > 10 ){ SpellsInBook = 0; }
				}
			}
			else if ( page > 1 && page < 12 )
			{
				AddHtml( 110, 47, 177, 20, @"<BODY><BASEFONT Color=" + color + "><CENTER>HABILIDADES DE MONGE</CENTER></BASEFONT></BODY>", (bool)false, (bool)false);
				AddHtml( 404, 47, 177, 20, @"<BODY><BASEFONT Color=" + color + "><CENTER>HABILIDADES DE MONGE</CENTER></BASEFONT></BODY>", (bool)false, (bool)false);

				string know = "<BODY><BASEFONT Color=#f58a8a>Não Aprendida</BASEFONT></BODY>"; if ( this.HasSpell( from, abil_spid ) ){ know = "<BODY><BASEFONT Color=#8af599>Aprendida</BASEFONT></BODY>"; }

				string ismonk = "<BODY><BASEFONT Color=#f58a8a>Você não é um Monge!</BASEFONT></BODY>";
				if ( Server.Misc.GetPlayerInfo.isMonk( from ) )
					ismonk = "<BODY><BASEFONT Color=#8af599>Você está no caminho...</BASEFONT></BODY>";

				AddHtml( 130, 105, 200, 20, @"<BODY><BASEFONT Color=" + color + ">" + abil_name + "</BASEFONT></BODY>", (bool)false, (bool)false);
				if ( this.HasSpell( from, abil_spid) ){ abil_info = ""; showScrollBar = false; AddButton(78, 94, abil_icon, abil_icon, abil_spid, GumpButtonType.Reply, 0); }
				AddImage(78, 94, abil_icon, 2422);

				AddHtml( 75, 370, 253, 20, @"" + ismonk + "", (bool)false, (bool)false);

				AddHtml( 75, 336, 253, 20, @"" + know + "", (bool)false, (bool)false);

				AddHtml( 130, 160, 88, 20, @"<BODY><BASEFONT Color=" + color + ">Habilidade:</BASEFONT></BODY>", (bool)false, (bool)false);
				AddHtml( 225, 160, 88, 20, @"<BODY><BASEFONT Color=" + color + ">" + abil_skil + "</BASEFONT></BODY>", (bool)false, (bool)false);
				AddHtml( 130, 210, 88, 20, @"<BODY><BASEFONT Color=" + color + ">Mana:</BASEFONT></BODY>", (bool)false, (bool)false);
				AddHtml( 225, 210, 88, 20, @"<BODY><BASEFONT Color=" + color + ">" + abil_mana + "</BASEFONT></BODY>", (bool)false, (bool)false);
				AddHtml( 130, 260, 88, 20, @"<BODY><BASEFONT Color=" + color + ">Dízimo:</BASEFONT></BODY>", (bool)false, (bool)false);
				AddHtml( 225, 260, 88, 20, @"<BODY><BASEFONT Color=" + color + ">" + abil_tith + "</BASEFONT></BODY>", (bool)false, (bool)false);

				AddHtml( 370, 82, 247, 309, @"<BODY><BASEFONT Color=" + color + ">" + abil_text + abil_info + "</BASEFONT></BODY>", (bool)false, (bool)showScrollBar);

				if ( !this.HasSpell( from, abil_spid ) && Sextants.HasSextant( from ) )
					AddButton(305, 52, 10461, 10461, 800, GumpButtonType.Reply, 0);
			}
			else if ( page == 12 )
			{
				AddHtml( 110, 47, 177, 20, @"<BODY><BASEFONT Color=" + color + "><CENTER>HABILIDADES DE MONGE</CENTER></BASEFONT></BODY>", (bool)false, (bool)false);
				AddHtml( 404, 47, 177, 20, @"<BODY><BASEFONT Color=" + color + "><CENTER>MOCHILA DE MONGE</CENTER></BASEFONT></BODY>", (bool)false, (bool)false);

				AddHtml( 78, 83, 247, 309, @"<BODY><BASEFONT Color=" + color + ">Monjes são uma ordem daqueles que aperfeiçoam seu corpo e espírito. Para se tornar um monge, deve-se se tornar um grão-mestre natural tanto em foco quanto em meditação. Monjes não podem usar nenhuma arma nem usar qualquer tipo de armadura, a menos que a armadura seja leve ou suficiente para permitir o canalizar de feitiços. Suas habilidades inatas vêm de suas habilidades em luta de punhos, para que possam fazer uso de luvas de pugilista. Para realizar qualquer uma das habilidades de monge, deve-se seguir estas regras. Um monge também não é considerado como tal a menos que use um manto místico de monge que eles próprios criam usando a habilidade de monge associada. Junto com isso, os monges não precisam vestir este manto se forem criar tal manto. Essa é a única exceção.<br><br>Quando você adquiriu este tomo, provavelmente olhou as páginas para ver as várias habilidades que um monge pode aprender. Para aprender os segredos dessas habilidades, você precisa viajar para os vários locais e abrir este livro lá para alcançar seu ki para iluminação. Certifique-se de trazer um pergaminho em branco com você, para que você possa escrever o que aprendeu. Você pode então colocar seus escritos dentro deste livro e usar as habilidades se sua habilidade e mana permitirem. Sempre que alguém toca nestes tomos, ele é vinculado ao seu ki individual, a menos que já esteja vinculado a outro. Isso significa que você será o único capaz de abrir o livro, pois ele pertence a você. Seus escritos também compartilham essa qualidade, então quando você aprende sobre novas habilidades, os pergaminhos pertencem a você. Qualquer outra pessoa que tocar nesses pergaminhos fará com que o papel se desfaça em pó.<br><br>Como afirmado anteriormente, os monges podem criar seus próprios mantos e isso é algo que todo monge deve buscar fazer rapidamente. Sem usar este manto, um monge não pode realizar as habilidades que aprendeu. O nível de habilidade de um monge determinará o poder do manto criado. Quando você criar o manto, ele aparecerá em sua mochila e terá vários pontos que você pode gastar para melhorá-lo. Isso permite que você adapte o manto para se adequar ao seu estilo. Para começar, clique uma vez no manto e selecione 'Status'. Um menu aparecerá onde você pode escolher quais atributos deseja que o manto tenha. Tenha cuidado, pois você não pode alterar um atributo depois de selecioná-lo. Os pontos que você pode gastar são iguais ao poder do manto. Apenas um de seus mantos pode existir no mundo por vez, então se você criar outro, quaisquer mantos anteriores desaparecerão para o plano astral.<br><br>Monjes buscam contribuir para causas além das suas, então alguns monges buscam ajudar os menos afortunados, enquanto monges mais vis buscam ajudar causas que diminuem o bem da terra. Como tal, eles devem dizimar ouro para usar suas habilidades. Você pode dizimar ouro em qualquer santuário que encontrar clicando uma vez no santuário e escolhendo a opção apropriada. As habilidades exigem quantidades variadas de pontos de dízimo para usar. Este tomo mostrará quantos pontos você tem disponível, e esta informação também pode ser vista pressionando o botão 'Info' no paper doll do seu personagem.<br><br>Para demonstrar seu título de 'Monge', você deve definir seu título de habilidade para 'Luta de Punhos'. Contanto que você siga as regras da vida monástica, seu título permanecerá como tal. Se você tiver uma habilidade de aprendiz em artes mágicas ou necromânticas, mas viver a vida de um monge, então seu título seria o de 'Místico'. Monges aventureiros podem aprender habilidades além daquelas que os monges devem conhecer, apenas certifique-se de que quaisquer outras habilidades não prejudiquem a vida de um monge (por exemplo, não aprenda esgrima, pois espadas são inúteis para monges). Não há outros requisitos comportamentais para ser um monge. Alguns são bons e alguns são maus. Depende de você o caminho que tomar.<br><br>Você pode ter barras de ferramentas para usar essas habilidades rapidamente, e embora você possa gerenciar isso no menu 'Ajuda', abaixo estão os comandos que você pode digitar para usar essas barras de ferramentas:<br><br>Abra o primeiro editor de barra de habilidades:<br><br>[monkspell1<br><br>Abra o segundo editor de barra de habilidades:<br><br>[monkspell2<br><br>Abra a primeira barra de habilidades:<br><br>[monktool1<br><br>Abra a segunda barra de habilidades:<br><br>[monktool2<br><br>Feche a primeira barra de habilidades:<br><br>[monkclose1<br><br>Feche a segunda barra de habilidades:<br><br>[monkclose2<br><br><br><br>Abaixo estão alguns comandos que você pode digitar para usar essas habilidades e podem ajudar na criação de macros:<br><br>[AstralProjection<br><br>[AstralTravel<br><br>[CreateRobe<br><br>[GentleTouch<br><br>[Leap<br><br>[PsionicBlast<br><br>[PsychicWall<br><br>[PurityOfBody<br><br>[QuiveringPalm<br><br>[WindRunner<br><br></BASEFONT></BODY>", (bool)false, (bool)true);
				AddHtml( 370, 83, 247, 309, @"<BODY><BASEFONT Color=" + color + ">Quando você atingir o nível de grão-mestre monge ou místico, pode viajar para o " + book.PackShrine + " em Ambrosia e usar seu ki para invocar uma mochila de monge do plano astral. Você precisará de uma pérola para fazer isso. Quando você entrar no santuário, abra este livro e se for digno, a mochila aparecerá. No entanto, tenha cuidado, pois você só pode ter uma mochila por vez e quaisquer outras que você possa ter assim desaparecerão de volta para o plano astral junto com quaisquer itens dentro dela. Essas mochilas permitem que um monge carregue 100 itens diferentes virtualmente sem peso para qualquer coisa colocada dentro da mochila. Você será o único capaz de abrir esta mochila em particular, e se você perder seu caminho de grão-mestre monge ou místico, não poderá abrir a mochila. Você não pode armazenar seu manto de monge ou seu tomo nesta bolsa.</BASEFONT></BODY>", (bool)false, (bool)true);
			}
		}

		public override void OnResponse( NetState state, RelayInfo info ) 
		{
			Mobile from = state.Mobile; 

			if ( info.ButtonID == 800 )
			{
				from.PlaySound( 0x249 );
				from.SendGump( new MysticSpellbookGump( from, m_Book, m_Page ) );
				from.CloseGump( typeof( Sextants.MapGump ) );
				from.SendGump( new Sextants.MapGump( from, m_Map, m_X, m_Y, null ) );
			}
			else if ( info.ButtonID < 200 && info.ButtonID > 0 )
			{
				from.SendSound( 0x55 );
				int page = info.ButtonID;
				if ( page < 1 ){ page = 12; }
				if ( page > 12 ){ page = 1; }
				from.SendGump( new MysticSpellbookGump( from, m_Book, page ) );
				from.CloseGump( typeof( Sextants.MapGump ) );
			}
			else if ( info.ButtonID > 200 && HasSpell(from, info.ButtonID) )
			{
				if ( info.ButtonID == 250 ){ new AstralProjection( from, null ).Cast(); }
				else if ( info.ButtonID == 251 ){ new AstralTravel( from, null ).Cast(); }
				else if ( info.ButtonID == 252 ){ new CreateRobe( from, null ).Cast(); }
				else if ( info.ButtonID == 253 ){ new GentleTouch( from, null ).Cast(); }
				else if ( info.ButtonID == 254 ){ new Leap( from, null ).Cast(); }
				else if ( info.ButtonID == 255 ){ new PsionicBlast( from, null ).Cast(); }
				else if ( info.ButtonID == 256 ){ new PsychicWall( from, null ).Cast(); }
				else if ( info.ButtonID == 257 ){ new PurityOfBody( from, null ).Cast(); }
				else if ( info.ButtonID == 258 ){ new QuiveringPalm( from, null ).Cast(); }
				else if ( info.ButtonID == 259 ){ new WindRunner( from, null ).Cast(); }

				from.SendGump( new MysticSpellbookGump( from, m_Book, 1 ) );
				from.CloseGump( typeof( Sextants.MapGump ) );
			}
			else
			{
				from.SendSound( 0x55 );
				from.CloseGump( typeof( Sextants.MapGump ) );
			}
		}
	}
}