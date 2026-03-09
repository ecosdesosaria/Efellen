using System;
using System.Collections;
using Server.ContextMenus;
using System.Collections.Generic;
using Server.Misc;
using Server.Network;
using Server;
using Server.Items;
using Server.Gumps;
using Server.Mobiles;
using Server.Commands;
using System.Net;
using System.Diagnostics;
using Server.Accounting;

namespace Server.Mobiles
{
    public class ShardGreeter : BasePerson
	{
		public override bool IsInvulnerable{ get{ return true; } }

		[Constructable]
		public ShardGreeter() : base( )
		{
			Direction = Direction.South;
			CantWalk = true;
			Female = true;

			SpeechHue = Utility.RandomTalkHue();
			Hue = 1009;
			NameHue = 0x92E;
			Body = 0x191;
			Name = NameList.RandomName( "female" );
			Title = "a cigana";

			FancyDress dress = new FancyDress(0xAFE);
			dress.ItemID = 0x1F00;
			AddItem( dress );
			AddItem( new Sandals() );

			Utility.AssignRandomHair( this );
			HairHue = 0x92E;
			HairItemID = 8252;
			FacialHairItemID = 0;
		}

		public ShardGreeter(Serial serial) : base(serial)
		{
		}

		public override void GetContextMenuEntries( Mobile from, List<ContextMenuEntry> list ) 
		{ 
			base.GetContextMenuEntries( from, list ); 
			list.Add( new ShardGreeterEntry( from, this ) ); 
		} 

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 ); 
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}

		public class ShardGreeterEntry : ContextMenuEntry
		{
			private Mobile m_Mobile;
			private Mobile m_Giver;

			public ShardGreeterEntry( Mobile from, Mobile giver ) : base( 6146, 3 )
			{
				m_Mobile = from;
				m_Giver = giver;
			}

			public override void OnClick()
			{
				if( !( m_Mobile is PlayerMobile ) )
					return;

				PlayerMobile mobile = (PlayerMobile) m_Mobile;

				if ( ( m_Mobile.X == 3567 && m_Mobile.Y == 3404 ) || m_Mobile.RaceID > 0 )
				{
					m_Giver.PlaySound( 778 );
					mobile.CloseGump( typeof( GypsyTarotGump ) );
					mobile.CloseGump( typeof( WelcomeGump ) );
					mobile.CloseGump( typeof( RacePotions.RacePotionsGump ) );
					mobile.SendGump(new GypsyTarotGump( m_Mobile, 0 ) );
				}
				else
				{
					m_Giver.Say( "Por favor, " + m_Mobile.Name + ". Sente-se e vamos começar." );
				}
			}
		}
	}
}

namespace Server.Gumps
{
	public class MonsterGump : Gump
    {
        public MonsterGump( Mobile from ) : base( 50, 50 )
        {
			string color = "#efc99b";
			from.SendSound( 0x4A );

            this.Closable=true;
			this.Disposable=true;
			this.Dragable=true;
			this.Resizable=false;

			AddPage(0);

			AddImage(0, 0, 7034, Server.Misc.PlayerSettings.GetGumpHue( from ));
			AddHtml( 12, 12, 425, 20, @"<BODY><BASEFONT Color=" + color + ">" + Server.Items.BaseRace.StartName( from.RaceID ) + " - " + from.Name + "</BASEFONT></BODY>", (bool)false, (bool)false);
			AddHtml( 12, 42, 509, 351, @"<BODY><BASEFONT Color=" + color + ">" + from.Profile + "</BASEFONT></BODY>", (bool)false, (bool)false);
			AddButton(496, 9, 4017, 4017, 0, GumpButtonType.Reply, 0);
        }

		public override void OnResponse( NetState state, RelayInfo info )
		{
			Mobile from = state.Mobile;
			from.SendSound( 0x4A );
		}
    }

	public class WelcomeGump : Gump
    {
        public WelcomeGump( Mobile from ) : base( 400, 50 )
        {
            this.Closable=true;
			this.Disposable=true;
			this.Dragable=true;
			this.Resizable=false;

			AddPage(0);

			AddImage(0, 0, 2610, Server.Misc.PlayerSettings.GetGumpHue( from ));

			int header = 11474;
			if ( MySettings.S_ServerName == "Ecos de Sosaria" ){ header = 11377; }
			AddImage(13, 12, header, 2126);

			AddHtml( 13, 58, 482, 312, @"<BODY><BASEFONT Color=#94C541>Para você, o dia foi normal, como qualquer outro. No entanto, quando o sol da tarde finalmente desapareceu atrás da paisagem, você se recolheu à cama, onde o sono foi inquieto e os sonhos mais vívidos. Você não se lembra dos detalhes do sonho, mas recorda-se de ter sido puxado deste mundo através de um portal giratório. Ao acordar, você se encontrou aqui, nesta floresta. Suas roupas de dormir desapareceram e agora você veste trajes medievais, empunhando uma lanterna.<BR><BR>Através da escuridão da noite, você vê uma fogueira à frente. Uma tenda colorida está ao lado, com o brilho acolhedor de lanternas ao redor. O som do riacho próximo proporciona tranquilidade, e você pode ver um urso pardo dormindo profundamente ao lado do calor do fogo. Se você pudesse se livrar das preocupações da sua vida atual, sente que este seria o lugar para recomeçar. Você decide ver quem está acampando aqui e talvez descobrir onde você está.</BASEFONT></BODY>", (bool)false, (bool)false);
			
			AddButton(468, 10, 4017, 4017, 0, GumpButtonType.Reply, 0);
        }

		public override void OnResponse( NetState state, RelayInfo info )
		{
			Mobile from = state.Mobile;
			from.SendSound( 0x4A );
		}
    }

	public class GypsyTarotGump : Gump
	{
		public bool visitLodor( Mobile from )
		{
			bool visited = false;

			Account a = from.Account as Account;

			if ( a == null )
				return false;

			int index = 0;

			for ( int i = 0; i < a.Length; ++i )
			{
				Mobile m = a[i];

				if ( m != null )
				{
					if ( PlayerSettings.GetDiscovered( m, "the Land of Lodoria" ) ){ visited = true; }
				}
				++index;
			}
			return visited;
		}

		public bool visitSavage( Mobile from )
		{
			bool visited = false;

			Account a = from.Account as Account;

			if ( a == null )
				return false;

			int index = 0;

			for ( int i = 0; i < a.Length; ++i )
			{
				Mobile m = a[i];

				if ( m != null )
				{
					if ( PlayerSettings.GetDiscovered( m, "the Savaged Empire" ) ){ visited = true; }
				}
				++index;
			}
			return visited;
		}

		public int pageShow( Mobile from, int page, bool forward )
		{
			bool isDrow = false;
			if ( from.RaceID > 0 && from.FindItemOnLayer(Layer.Special) is BaseRace )
			{
				BaseRace race = (BaseRace)from.FindItemOnLayer(Layer.Special);
				isDrow = (race.SpeciesFamily == "drow");
			}

			if ( from.RaceID > 0 )
			{
				if ( isDrow )
				{
					if ( forward )
					{
						page++;
						if ( page > 6 ) page = 5;
					}
					else
					{
						page--;
						if ( page < 5 ) page = 6;
					}
					return page;
				}
				else
				{
					if ( forward )
					{
						page++;

						if ( Server.Items.BaseRace.IsGood( from ) && page == 2 ){ page++; }
						if ( !visitLodor( from ) && page == 3 ){ page++; }
						if ( ( Server.Items.BaseRace.IsGood( from ) || !visitLodor( from ) ) && page == 4 ){ page++; }
						
						if ( page > 4 ){ page = 20; }
					}
					else
					{
						page--;

						if ( ( Server.Items.BaseRace.IsGood( from ) || !visitLodor( from ) ) && page == 4 ){ page--; }
						if ( !visitLodor( from ) && page == 3 ){ page--; }
						if ( Server.Items.BaseRace.IsGood( from ) && page == 2 ){ page--; }
						
						if ( page < 1 ){ page = 20; }
					}
					return page;
				}
			}
			else
			{
				if ( forward )
				{
					page++;

					if ( !visitLodor( from ) && page == 10 ){ page++; }
					if ( !visitLodor( from ) && page == 11 ){ page++; }
					if ( !visitSavage( from ) && page == 12 ){ page++; }
					//if ( !MySettings.S_AllowAlienChoice && page == 13 && from.RaceID == 0 ){ page++; }
					if ( page > 12 ){ page = 20; }
				}
				else
				{
					page--;

					//if ( !MySettings.S_AllowAlienChoice && page == 13 && from.RaceID == 0 ){ page--; }
					if ( !visitSavage( from ) && page == 12 ){ page--; }
					if ( !visitLodor( from ) && page == 11 ){ page--; }
					if ( !visitLodor( from ) && page == 10 ){ page--; }
					if ( page < 1 ){ page = 20; }
				}
				return page;
			}
		}

		public static string GypsySpeech( Mobile from )
		{
			string monst = "";
			string races = "Por fim, este reino ";
			if ( MyServerSettings.MonstersAllowed() )
			{
				monst = " Há uma prateleira ali com poções interessantes que você pode querer. Então, se quiser uma, beba-a agora e retorne aqui para sua leitura de tarô.";
				races = "Você pode não ser realmente de descendência humana. Você pode ser na verdade um ogro, troll ou sátiro. Há muitas criaturas que você pode realmente ser. Se quiser explorar essas ideias, olhe na minha prateleira de poções atrás de mim. Lá você encontrará várias poções de alteração, que podem mudar sua vida. Se escolher uma dessas criaturas para ser, considere mudar seu nome para representar melhor a criatura que escolheu interpretar. Isso me leva às minhas palavras finais de conselho. Este reino ";
			}

			return "Saudações, " + from.Name + "... você está prestes a entrar em uma das terras do " + MySettings.S_ServerName + ". Não muito tempo atrás, o Estranho chegou a Sosaria e frustrou os planos malignos de Exodus. O Castelo de Exodus está em ruínas e Sosaria está mais uma vez tentando reconstruir em paz. No entanto, muitos monstros vis ainda percorrem a terra, mas aventureiros corajosos têm procurado bravamente nos livrar desses horrores. Para começar sua jornada, simplesmente escolha seu destino do meu baralho de tarô (comece pressionando o botão no canto superior direito). Depois de examinar o baralho (pressionando os botões de seta), você pode tirar uma carta de sua escolha (pressionando o botão OK no canto superior direito)." + monst + "<br><br>Agora deixe-me contar algumas coisas do mundo para o qual o destino o trouxe. Viajar pelas terras pode ser perigoso, pois outros aventureiros podem decidir matá-lo por seu ouro ou propriedade. As tavernas, estalagens e bancos estão seguros contra tais ameaças, mas também há muitos guardas nos assentamentos para manter a paz. Eles são conhecidos por lidar rapidamente com assassinos e criminosos. Há muitos comerciantes em todos os assentamentos. Eles não são capazes de vender ou comprar tudo com o que normalmente lidam, pois suas escolhas do que compram e vendem mudam de dia para dia.<br><br>Há segredos a serem aprendidos e itens mágicos a serem encontrados nas muitas masmorras. Cada assentamento em Sosaria é relativamente seguro nas terras ao redor, então caçar por comida ou peles deve ser relativamente seguro. Não posso dizer o mesmo de outras terras. Há também uma masmorra menor perto de cada assentamento de Sosaria, se você deseja começar a percorrer os perigos abaixo antes de estar totalmente preparado. Esteja avisado que as criaturas vis não são tudo o que você deve enfrentar. Há muitas armadilhas mortais nas salas e corredores desses lugares que podem matá-lo mais rápido do que o monstro do qual você pode estar fugindo.<br><br>Prepare-se para seguir em frente e fazer de sua vida o que você quiser. Torne-se o melhor artesão da terra, um proprietário rico de terras e castelos, o guerreiro mais poderoso, ou até mesmo o mago mais poderoso. A escolha é sua.<br><br>Este mundo pode ser percorrido sozinho ou com amigos, onde se pode ter grandes aventuras. Como já disse, o curso de vida que você escolher é o que você quiser fazer. Você pode ser um guerreiro poderoso ou um mago influente. Você pode simplesmente abrir uma loja de poções perto de uma grande cidade. Você pode ser um mestre das feras ou um bardo místico. Este é um mundo onde grande riqueza e artefatos podem ser obtidos das muitas masmorras por toda a terra. Você pode ser morto por uma criatura, morrer de fome, perder-se no escuro, ou tropeçar em uma armadilha mortal. Você pode encontrar relíquias poderosas e ouro suficiente para construir seu próprio castelo.<br><br>" + races + "é melhor aproveitado se você tiver um nome que seja compatível com um mundo de fantasia rico. Você tem uma última chance de mudar seu nome, se precisar, simplesmente usando meu diário na mesa atrás de mim. Você não pode ter um nome que alguém já tenha, então ele deve ser único. Se quiser mudar seu nome, vá até a mesa onde guardo meu diário. Depois que seu nome for alterado, retorne aqui para sua leitura de tarô.";
		}

		public GypsyTarotGump( Mobile from, int page ): base( 50, 50 )
		{
            this.Closable=true;
			this.Disposable=true;
			this.Dragable=true;
			this.Resizable=false;

			AddPage(0);
			AddImage(0, 0, 2611, Server.Misc.PlayerSettings.GetGumpHue( from ));
			AddImage(10, 8, 1102);

			if ( page > 0 && page < 50 )
			{
				int prev = pageShow( from, page, false );
				int next = pageShow( from, page, true );

				AddImage(640, 8, cardGraphic( page, from ));

				AddItem(269, 349, 4775);
				AddItem(586, 349, 4776);
				AddButton(317, 375, 4014, 4014, prev, GumpButtonType.Reply, 0);
				AddButton(552, 375, 4005, 4005, next, GumpButtonType.Reply, 0);

				AddHtml( 269, 12, 240, 20, @"<BODY><BASEFONT Color=#DEC6DE>" + cardText( page, 1, from ) + "</BASEFONT></BODY>", (bool)false, (bool)false);
				AddHtml( 271, 47, 356, 297, @"<BODY><BASEFONT Color=#DEC6DE>" + cardText( page, 2, from ) + "<BR><BR>" + cardText( page, 3, from ) + "</BASEFONT></BODY>", (bool)false, scrollBar( page, from ));

				AddItem(566, 12, 4777);
				AddItem(580, 26, 4779);
				AddButton(599, 11, 4023, 4023, page+100, GumpButtonType.Reply, 0);
			}
			else
			{
				int header = 11473;
				if ( MySettings.S_ServerName == "Secrets of Sosaria" ){ header = 11376; }

				AddImage(271, 13, header, 2813);

				AddHtml( 278, 73, 604, 320, @"<BODY><BASEFONT Color=#DEC6DE>" + GypsySpeech( from ) + "</BASEFONT></BODY>", (bool)false, (bool)true);
				
				int nextPage = 1;
				if ( from.RaceID > 0 && from.FindItemOnLayer(Layer.Special) is BaseRace )
				{
					BaseRace race = (BaseRace)from.FindItemOnLayer(Layer.Special);
					if ( race.SpeciesFamily == "drow" )
					{
						nextPage = 5;
					}
				}
				
				AddButton(819, 14, 4011, 4011, nextPage, GumpButtonType.Reply, 0);
				AddItem(851, 11, 4773);
			}
		}

		public int cardGraphic( int page, Mobile from )
		{
			int val = 0;

			if ( from.RaceID > 0 )
			{
				bool isDrow = false;
				if ( from.FindItemOnLayer(Layer.Special) is BaseRace )
				{
					BaseRace race = (BaseRace)from.FindItemOnLayer(Layer.Special);
					isDrow = (race.SpeciesFamily == "drow");
				}

				if ( isDrow )
				{
					switch ( page )
					{
						case 5: val = 1112; break;
						case 6: val = 1109; break;
					}
				}
				else
				{
					switch ( page )
					{
						case 1: val = 1340; break;
						case 2: val = 1341; break;
						case 3: val = 1342; break;
						case 4: val = 1343; break;
					}
				}
			}
			else 
			{
				switch ( page )
				{
					case 1: val = 1106; break;
					case 2: val = 1105; break;
					case 3: val = 1110; break;
					case 4: val = 1122; break;
					case 5: val = 1116; break;
					case 6: val = 1108; break;
					case 7: val = 1104; break;
					case 8: val = 1120; break;
					case 9: val = 1109; break;
					case 10: val = 1111; break;
					case 11: val = 1112; break;
					case 12: val = 1119; break;
					case 13: val = 1118; break;
				}
			}
			return val;
		}

		public bool scrollBar( int page, Mobile from )
		{
			bool scroll = false;

			if ( from.RaceID > 0 )
			{
				bool isDrow = false;
				if ( from.FindItemOnLayer(Layer.Special) is BaseRace )
				{
					BaseRace race = (BaseRace)from.FindItemOnLayer(Layer.Special);
					isDrow = (race.SpeciesFamily == "drow");
				}

				if ( isDrow )
				{
					scroll = true;
				}
				else
				{
					switch ( page )
					{
						case 1: scroll = false; break;
						case 2: scroll = true; break;
						case 3: scroll = false; break;
						case 4: scroll = true; break;
					}
					if ( Server.Items.BaseRace.GetUndead( from.RaceID ) )
						scroll = true;
				}
			}
			else 
			{
				switch ( page )
				{
					case 1: scroll = false; break;
					case 2: scroll = false; break;
					case 3: scroll = false; break;
					case 4: scroll = false; break;
					case 5: scroll = false; break;
					case 6: scroll = false; break;
					case 7: scroll = false; break;
					case 8: scroll = false; break;
					case 9: scroll = true; break;
					case 10: scroll = true; break;
					case 11: scroll = true; break;
					case 12: scroll = true; break;
					case 13: scroll = true; break;
				}
			}
			return scroll;
		}

		public string cardText( int page, int section, Mobile from )
		{
			string card = "";
			string town = "";
			string text = "";
			string lodor = "A maioria dos aventureiros nasce na Terra de Sosaria, apenas ouvindo contos e lendas de outras terras distantes. Uma dessas terras é o mundo élfico de Lodoria. Este mundo é um pouco maior que Sosaria e as masmorras são um pouco mais difíceis. O que Lodoria tem são locais familiares que aventureiros veteranos lembram com carinho. Masmorras como Shame, Destard e Wrong podem ser encontradas por toda parte. Há muitas vilas e cidades, e todas são habitadas pelo bom povo élfico. O povo élfico muito mais vil, os drow, procura destruir aqueles que abraçam a luz e tenta suprimir seu governo sob a superfície do mundo. Se você deseja começar sua jornada em Lodoria, então você será um humano que cresceu nesta terra estranha sem laços com aqueles de Sosaria.";

			string fate = "Se você escolher este destino, ";
			switch ( Utility.RandomMinMax(0,8) )
			{
				case 1: fate = "Se você escolher esta carta, "; break;
				case 2: fate = "Se você pegar esta carta, "; break;
				case 3: fate = "Se esta for a carta que você quer, "; break;
				case 4: fate = "Se esta carta for sua, "; break;
				case 5: fate = "Se este destino é para você, "; break;
				case 6: fate = "Se você tirar esta carta, "; break;
				case 7: fate = "Se você escolher este caminho, "; break;
				case 8: fate = "Se você pegar esta estrada, "; break;
			}

			string begin = "você começará sua jornada";
			switch ( Utility.RandomMinMax(0,8) )
			{
				case 1: begin = "você começará sua vida"; break;
				case 2: begin = "você entrará no mundo"; break;
				case 3: begin = "você será um cidadão"; break;
				case 4: begin = "você terá uma nova vida"; break;
				case 5: begin = "você pode começar uma nova vida"; break;
				case 6: begin = "você pode ter um novo lar"; break;
				case 7: begin = "você pode começar sua jornada"; break;
				case 8: begin = "você pode começar uma nova vida"; break;
			}

			fate = fate + begin;

			if ( from.RaceID > 0 )
			{
				bool isDrow = false;
				if ( from.FindItemOnLayer(Layer.Special) is BaseRace )
				{
					BaseRace race = (BaseRace)from.FindItemOnLayer(Layer.Special);
					isDrow = (race.SpeciesFamily == "drow");
				}

				string undead = "";
				if ( Server.Items.BaseRace.GetUndead( from.RaceID ) ){ undead = " Embora você não se lembre de sua vida passada, você se sente diferente dos outros mortos-vivos. Você parece ter retido sua alma, o que certamente será notado por outros mortos-vivos. Isso significa que eles provavelmente o atacarão como fazem com os vivos."; }

				if ( Server.Items.BaseRace.BloodDrinker( from.RaceID ) ){ undead = undead + " Ter uma alma, no entanto, significa que você pode caminhar com segurança pela terra durante a luz do dia."; }

				if ( isDrow )
				{
					switch ( page )
					{
						case 5: 
							card = "A MORTE"; 
							town = "A Subcidade de Umbra";
							text = fate + " no fundo da Subcidade de Umbra, um refúgio para aqueles que praticam as artes necróticas. No fundo das montanhas, a sudeste de Britain, os salões e cavernas sombrios têm uma sensação assustadora, mas os necromantes providenciam para si mesmos lojas para fornecer itens necessários. A caverna fora da cidade é uma das mais altas já vistas. Alguns dizem que é alta o suficiente para até construir um castelo longe da luz do sol. A tumba de um cavaleiro da morte também foi construída nas proximidades, e as Fogueiras do Inferno estão a apenas uma caminhada de distância." + undead; 
							break;
							
						case 6: 
							card = "O ENFORCADO"; 
							town = "As Masmorras de Britain";
							text = "Você pode escolher um destino neste mundo que tem uma vida mais desafiadora, onde você é um fugitivo da justiça. Se você escolher este caminho, você será capaz de se tornar grão-mestre em " + MyServerSettings.SkillGypsy( "fugitive" ) + " habilidades diferentes em vez das " + MyServerSettings.SkillGypsy( "default" ) + " normalmente realizadas. Isso se deve a você confiar em si mesmo para sobreviver. Os tributos para ressurreição custarão o dobro, talvez forçando-o a ressuscitar com penalidades. Você não terá permissão para entrar em nenhuma área civilizada a menos que talvez encontre uma maneira de se disfarçar. As exceções são algumas áreas públicas como estalagens, tavernas e bancos. Os guardas o atacarão à vista, os comerciantes tentarão expulsá-lo e você não poderá se juntar a nenhuma guilda local, exceto as guildas de Assassinos, Ladrões e Magia Negra. A razão para isso é que você é procurado por assassinato. Você pode ter realmente cometido o ato, ou pode ter simplesmente sido incriminado. O assassinato foi contra uma figura muito poderosa, então muitas terras nunca perdoarão o feito. Seja verdade ou falsidade, cabe a você contar. Faça com sua vida o que quiser. Você pode viver uma vida de buscas criminosas, ou pode destruir o mal que espreita nos lugares mais sombrios da terra. Se você deseja escolher tal vida, você estará por conta própria, e deve primeiro escapar de sua cela de prisão. De lá, é melhor você ir para Stonewall a noroeste, mas você pode ir para onde quiser. Tudo que você precisa pode ser encontrado pelo mundo." + undead;
							break;
					}
				}
				else
				{
					town = Server.Items.BaseRace.StartName( from.RaceID );
					
					switch ( page )
					{
						case 1: card = "O DIA"; text = fate + " " + Server.Items.BaseRace.StartSentence( town ) + " de Sosaria." + undead + " Este mundo sofreu três eras das trevas, onde um estranho veio de uma terra distante para trazer luz a cada um desses eventos. Depois que Mondain, Minax e Exodus foram frustrados em seus planos malignos, Sosaria atingiu um nível de paz e prosperidade. Embora a maioria queira levar uma vida humilde como simples aldeões, há alguns que buscam explorar as antigas masmorras, tumbas, ruínas e criptas do mundo. Este caminho o levará a se juntar aos caminhos do homem civilizado, mas fazê-lo certamente fará com que seus parentes o baniquem de sua presença. Isso importa pouco para você, pois você prefere buscar fama e fortuna neste mundo livre dos males mais poderosos que já viu."; break;

						case 2: card = "A NOITE"; text = fate + " " + Server.Items.BaseRace.StartSentence( town ) + " de Sosaria." + undead + " Este destino em Sosaria tem uma vida mais desafiadora, onde você talvez tenha deixado outros da sua espécie, mas decidiu abraçar seus caminhos monstruosos e buscar poder para si mesmo. Você será capaz de se tornar grão-mestre em " + MyServerSettings.SkillGypsy( "fugitive" ) + " habilidades diferentes em vez das " + MyServerSettings.SkillGypsy( "default" ) + " normalmente realizadas. Os tributos para ressurreição custarão o dobro, talvez forçando-o a ressuscitar com penalidades. Você não terá permissão para entrar em nenhuma área civilizada, a menos que talvez encontre uma maneira de se disfarçar. As exceções são algumas áreas públicas como estalagens, tavernas e bancos. Os guardas o atacarão à vista, os comerciantes tentarão expulsá-lo e você não poderá se juntar a nenhuma guilda local, exceto as guildas de Assassinos, Ladrões e Magia Negra. A razão para isso é que você é visto como uma besta assassina. Tudo que você precisa pode ser encontrado pelo mundo, no entanto, então você pode seguir em sua jornada."; break;

						case 3: card = "A LUZ"; text = fate + " " + Server.Items.BaseRace.StartSentence( town ) + " de Lodoria." + undead + " Este mundo já foi governado por anões, mas agora suas cidades estão em ruínas e os elfos surgiram como a principal raça civilizada da terra. Empurrando os drow de volta para seus covis profundos no subsolo, muitos buscam explorar este mundo. Embora a maioria queira levar uma vida humilde como simples aldeões, há alguns que buscam explorar as antigas masmorras, tumbas, ruínas e criptas do mundo. Este caminho o levará a se juntar aos caminhos da civilização dentro da terra dos elfos. Onde você pode buscar glória e riquezas além de seus sonhos mais selvagens."; break;

						case 4: card = "A ESCURIDÃO"; text = fate + " " + Server.Items.BaseRace.StartSentence( town ) + " de Lodoria." + undead + " Este destino em Lodoria tem uma vida mais desafiadora, onde você talvez tenha deixado outros da sua espécie, mas decidiu abraçar seus caminhos monstruosos e buscar poder para si mesmo. Você será capaz de se tornar grão-mestre em " + MyServerSettings.SkillGypsy( "fugitive" ) + " habilidades diferentes em vez das " + MyServerSettings.SkillGypsy( "default" ) + " normalmente realizadas. Os tributos para ressurreição custarão o dobro, talvez forçando-o a ressuscitar com penalidades. Você não terá permissão para entrar em nenhuma área civilizada, a menos que talvez encontre uma maneira de se disfarçar. As exceções são algumas áreas públicas como estalagens, tavernas e bancos. Os guardas o atacarão à vista, os comerciantes tentarão expulsá-lo e você não poderá se juntar a nenhuma guilda local, exceto as guildas de Assassinos, Ladrões e Magia Negra. A razão para isso é que você é visto como uma besta assassina. Tudo que você precisa pode ser encontrado pelo mundo, no entanto, então você pode seguir em sua jornada."; break;
					}
				}
			}
			else
			{
				switch ( page )
				{
					case 1: card = "O IMPERADOR"; town = "A Cidade de Britain"; text = fate + " na capital de Sosaria e o lar de Lord British. O magnífico castelo de Lord British está situado na parte norte da cidade, com vista para a Baía de Britain. Este edifício alto é a maior estrutura arquitetônica da nova era. Os súditos leais prestam homenagem a Sua Majestade e renovam a fidelidade sempre que estão nas proximidades de seu castelo. Rumores em tavernas falam de um segredo sombrio abaixo do castelo, tão sombrio que nem mesmo os cidadãos podem vê-lo. Há fazendas por toda parte, bem como cemitérios para os cidadãos e outro para a Família Real Britânica. Alguns foram vistos entrando no túmulo britânico, tarde da noite."; break;

					case 2: card = "O DIABO"; town = "A Cidade de Devil Guard"; text = fate + " em uma cidade totalmente enclausurada pelas Grandes Montanhas durante a Terceira Era das Trevas, e só era acessível pelo portão mágico. Após a destruição de Exodus, um túnel cavernoso rasgou a montanha, fornecendo uma rota alternativa. Lendas antigas contam sobre um castelo que caiu do céu, colidindo com as montanhas e criando o vale no qual Devil Guard foi eventualmente construída. Contam-se histórias de que a cidade foi criada e habitada por aqueles do castelo do céu, e eles a nomearam porque estavam protegendo os outros dos demônios há muito tempo."; break;

					case 3: card = "O EREMITA"; town = "A Vila de Grey"; text = fate + " nesta vila onde os habitantes, durante a Terceira Era das Trevas, deram várias pistas ao Estranho que derrotou Exodus. Até se rumoreava que eles vendiam navios que podiam voar para as estrelas, mas ninguém que resta sabe como criar tais coisas. Lendas dizem que o Estranho voou para o céu e alterou o tempo e a realidade, fazendo com que um castelo caísse para trás no tempo e colidisse com a terra da antiga Sosaria. Agora a vila é muitas vezes o lar daqueles que apreciam a solidão. Não há montanhas para minerar, mas alguns cavaram sob o solo da floresta para obter minério. Rumores dizem que o cemitério tem um segredo que necromantes sussurram em vozes baixas."; break;

					case 4: card = "A TORRE"; town = "A Cidade de Montor"; text = fate + " em uma vasta cidade, onde a coragem é especialmente valorizada, tendo todas as lojas necessárias para todos. Os habitantes de Montor sabiam muito sobre as místicas Quatro Cartas que o Estranho precisava para derrotar Exodus, bem como contos dos santuários perdidos de Ambrosia. Montor é a cidade mais visitada e também a maior em Sosaria devido ao comércio de navios. Há uma pequena mina a leste, bem como uma torre a nordeste. Diz-se que esta torre é o lar de um lich vil com um espelho mágico que atravessa dimensões, mas esses rumores são frequentemente contados com um caneco de hidromel."; break;

					case 5: card = "O MAGO"; town = "A Cidade de Moon"; text = fate + " na cidade onde, durante a Terceira Era das Trevas, era uma cidade cheia de magos. Eles eram, no entanto, do tipo corrupto e desonesto. Erstam também morava na cidade, conduzindo seus experimentos para a imortalidade. Quando Lord British expulsou os magos corruptos da cidade após a destruição de Exodus, Erstam e os outros decidiram ir para a Ilha da Serpente, onde ninguém poderia controlá-los. Agora um lugar tranquilo, muitos vêm para cá para cultivar e navegar pela costa para os mercados de peixe. É uma cidade popular por não ser muito grande, mas consegue fornecer muitos mercados para visitar. Aventureiros frequentemente chegam do deserto próximo, se gabando de riquezas obtidas da Pirâmide Antiga."; break;

					case 6: card = "O TOLO"; town = "A Cidade de Mountain Crest"; text = fate + " em algumas pequenas ilhas em Sosaria, que tem uma paisagem invernal rigorosa que outros acreditam ser tola de habitar. Junto com esta cidade, também há assentamentos a oeste e leste. Existem várias cavernas e masmorras dentro das montanhas, e uma torre incomum construída por um mago há muito tempo. Este lugar é uma das áreas mais difíceis de se viver, mas uma região nevada pode ser seu destino se você a escolher."; break;

					case 7: card = "A MORTE"; town = "A Subcidade de Umbra"; text = fate + " em um lugar que muitas pessoas não conhecem, pois foi construído como um refúgio para aqueles que praticam as artes necróticas. Sendo o lar dos Drows, você viverá como um escravo dos drows caso escolha esse caminho. No fundo das montanhas, a sudeste de Britain, os salões e cavernas sombrios têm uma sensação assustadora, mas os drows providenciam para si mesmos lojas para fornecer itens necessários. A caverna fora da cidade é uma das mais altas já vistas. Alguns dizem que é alta o suficiente para até construir um castelo longe da luz do sol. A tumba de um cavaleiro da morte também foi construída nas proximidades, e as Fogueiras do Inferno estão a apenas uma caminhada de distância."; break;

					case 8: card = "O SOL"; town = "A Vila de Yew"; text = fate + " em um vale de floresta densa, a oeste de Britain e leste de Moon, onde o sol cultiva as maiores árvores de Sosaria. Yew é um dos principais comércios de madeira da terra. Durante a Terceira Era das Trevas, o Estranho visitou Yew e aprendeu os segredos da Grande Serpente da Terra. Isso permitiu que o Estranho libertasse a serpente que estava bloqueando seu navio de alcançar o Castelo de Exodus na Ilha do Fogo. Alguns dizem que libertar a serpente causou um desequilíbrio no cosmos, mas isso podem ser magos bêbados contando histórias. Você pode minerar em uma caverna próxima, mas os mineiros descobriram algo no lado sul da cordilheira que eles não ousam entrar."; break;

					case 9: card = "O ENFORCADO"; town = "As Masmorras de Britain"; text = "Você pode escolher um destino neste mundo que tem uma vida mais desafiadora, onde você é um fugitivo da justiça. Se você escolher este caminho, você será capaz de se tornar grão-mestre em " + MyServerSettings.SkillGypsy( "fugitive" ) + " habilidades diferentes em vez das " + MyServerSettings.SkillGypsy( "default" ) + " normalmente realizadas. Isso se deve a você confiar em si mesmo para sobreviver. Os tributos para ressurreição custarão o dobro, talvez forçando-o a ressuscitar com penalidades. Você não terá permissão para entrar em nenhuma área civilizada, a menos que talvez encontre uma maneira de se disfarçar. As exceções são algumas áreas públicas como estalagens, tavernas e bancos. Os guardas o atacarão à vista, os comerciantes tentarão expulsá-lo e você não poderá se juntar a nenhuma guilda local, exceto as guildas de Assassinos, Ladrões e Magia Negra. A razão para isso é que você é procurado por assassinato. Você pode ter realmente cometido o ato, ou pode ter simplesmente sido incriminado. O assassinato foi contra uma figura muito poderosa, então muitas terras nunca perdoarão o feito. Seja verdade ou falsidade, cabe a você contar. Faça com sua vida o que quiser. Você pode viver uma vida de buscas criminosas, ou pode destruir o mal que espreita nos lugares mais sombrios da terra. Se você deseja escolher tal vida, você estará por conta própria, e deve primeiro escapar de sua cela de prisão. De lá, é melhor você ir para Stonewall a noroeste, mas você pode ir para onde quiser. Tudo que você precisa pode ser encontrado pelo mundo."; break;

					case 10: card = "O HIEROFANTE"; town = "A Cidade de Lodoria"; text = lodor + " A cidade é a capital de Lodor, e tem todos os comerciantes que você pode precisar. O Castelo do Conhecimento fica na alta montanha no lado oeste, onde estudiosos aprendem os caminhos do mundo. Tem uma mina ao norte e um cemitério no vale sul. O continente é grande e aventureiros contam histórias de masmorras como Shame, Despise e uma caverna de homens-lagarto. Outro pequeno assentamento fica a noroeste. Você escolhe este destino?"; break;

					case 11: card = "A ALTA SACERDOTISA"; town = "A Cidade de Elidor"; text = lodor + " A cidade está localizada no segundo maior continente, diverso com um sul coberto por floresta e um norte invernal. A Alta Sacerdotisa de Elidor construiu o famoso Salão das Ilusões, onde muitos de seus súditos praticam magia prismática. Existem outros assentamentos como Springvale a leste e Glacial Hills ao norte. Aventureiros bêbados frequentemente falam de riquezas de Wrong, Deceit e dos Infernos Congelados. Você deseja tirar esta carta?"; break;

					case 12: card = "A FORÇA"; town = "O Império Selvagem"; text = "Você pode escolher um modo de vida bárbaro para começar sua jornada, e não é para os fracos, mas para aqueles dotados de força. Se você escolher este caminho, você será capaz de se tornar grão-mestre em " + MyServerSettings.SkillGypsy( "savage" ) + " habilidades diferentes em vez das " + MyServerSettings.SkillGypsy( "default" ) + " normalmente realizadas. Isso se deve a você confiar em si mesmo para sobreviver em uma terra indomada. Sua aventura começará como um bárbaro no Império Selvagem, que é uma das terras mais difíceis dos reinos. Está cheio de muitos animais perigosos e dinossauros colossais. Não há lugares seguros para caçar comida, o que também significa que praticar suas habilidades de combate é igualmente perigoso. Você, no entanto, começará com uma armadura de couro que o ajudará a sobreviver aos perigos longe dos assentamentos. Você também começará com um talismã que o auxiliará em acampamento e culinária, para que você possa viver melhor da terra. Ouro adicional, comida e ataduras serão fornecidos, bem como uma adaga de aço e uma barraca de acampamento durável. Qualquer masmorra que você ousar entrar será mais mortal do que aquelas em Sosaria, então pense bem antes de decidir por este caminho. Sua jornada então começará na Vila de Kurak, onde os arredores têm muitas coisas para caçar, mas também muitos perigos dos quais você pode precisar fugir. Há uma caverna ao norte onde você pode minerar minérios preciosos também."; break;

					//case 13: card = "A ESTRELA"; town = "O Local do Acidente da Nave"; text = "Tudo o que o médico sabia sobre você como paciente está registrado em seu prontuário médico. Você estava perto da morte, mas colocá-lo na câmara de estase pareceu ter realizado o processo de cura. Suas varreduras mostraram um trauma craniano incrível, então você acordará de seu coma sem memórias do que ou quem você era (você começa sem habilidades). Com a estação espacial caindo em Sosaria, devido ao Estranho drenando as reservas de combustível, o médico decidiu colocar sua câmara de estase em sua última nave médica. Eles a configuraram no piloto automático e esperaram pelo melhor. Ela pousou com segurança em Sosaria onde você poderia continuar sua vida neste mundo primitivo. Você pode ter uma vantagem por ser de uma raça de seres mais avançada, então você tem a capacidade de lembrar e aprender mais coisas (pode se tornar grão-mestre em " + MyServerSettings.SkillGypsy( "alien" ) + " habilidades diferentes).<br><br>No entanto, devido ao seu conhecimento avançado de lógica e ciência, algumas coisas aprendidas sobre Sosaria são que eles têm elementos que você não pode entender completamente. Ressurreição mágica e o conceito de divindades são coisas que você não pode compreender (custa 3 vezes mais ouro para ressuscitar em um santuário ou curandeiro). O choque do sistema de qualquer tal ressurreição certamente cobraria seu preço (pagar tributo total ainda causa uma perda de 10% na fama e karma, e uma perda de 5% nas habilidades e atributos) o que poderia ser devastador (não pagar tributo algum causaria uma perda de 20% na fama e karma, e uma perda de 10% nas habilidades e atributos).<br><br>Embora você seja capaz de aprender algumas das habilidades que são classificadas como mágicas ou divinas, você certamente justificará com ciência. Por causa de sua falta de superstição, ao contrário dos habitantes deste mundo, você não acredita no conceito de sorte (você nunca se beneficiará da sorte). Você não tem nenhuma das moedas de Sosaria para negociar (você começa sem ouro), e porque você sente que é mais avançado, você provavelmente não se dará bem com os mestres de guildas dos ofícios rudes que praticam (a associação à guilda custa 4 vezes mais que o normal).<br><br>Se você escolher este destino, então você aparecerá em sua nave acidentada onde sua aventura começa. Você pode usar o terminal de computador próximo para alterar seus tons de pele e cabelo se quiser uma aparência ligeiramente diferente da humana, devido à sua herança alienígena.<br><br>Quando você acordar, você não terá memória de quem você era. Você se encontrará perto da nave que caiu no topo da montanha. O sistema de computador instruiu você sobre como configurar uma fonte de energia do combustível restante, e pareceu que uma criatura alienígena se agarrou à nave e morreu no acidente. Você tem usado isso como uma fonte de comida e sobreviveu alguns dias com isso. Agora seus suprimentos estão acabando, seu cantil está vazio e tudo que você tem é uma faca. Você terá que se aventurar se planeja sobreviver."; break;
				}
			}

			if ( section == 1 ){ return card; }
			else if ( section == 2 ){ return town; }
			return text;
		}

		public void EnterLand( int page, Mobile m )
		{
			Point3D loc = new Point3D(2999, 1030, 0);
			Map map = Map.Sosaria;

			if ( m.RaceID > 0 )
			{
				string start = Server.Items.BaseRace.StartArea( m.RaceID );
				string world = "the Land of Sosaria";

				bool isDrow = false;
				if ( m.FindItemOnLayer(Layer.Special) is BaseRace )
				{
					BaseRace race = (BaseRace)m.FindItemOnLayer(Layer.Special);
					isDrow = (race.SpeciesFamily == "drow");
				}

				if ( isDrow && (page == 5 || page == 6) )
				{
					if ( page == 5 )
					{
						loc = new Point3D(2666, 3325, 0);
						map = Map.Sosaria;
						start = "umbra";
					}
					else if ( page == 6 )
					{
						loc = new Point3D(4104, 3232, 0);
						map = Map.Sosaria;
						start = "cave";
						
						PlayerSettings.SetBardsTaleQuest( m, "BardsTaleWin", true );
						MyServerSettings.SkillBegin( "fugitive", (PlayerMobile)m );
						m.Kills = 1;
						((PlayerMobile)m).Fugitive = 1;
					}
				}
				else
				{

					if ( start == "cave" ){ 		loc = new Point3D(497, 4066, 0); }
					else if ( start == "ice" ){ 	loc = new Point3D(625, 3224, 0); }
					else if ( start == "pits" ){ 	loc = new Point3D(180, 4075, 0); }
					else if ( start == "sand" ){ 	loc = new Point3D(91, 3244, 0); }
					else if ( start == "sea" ){ 	loc = new Point3D(27, 4077, 0); }
					else if ( start == "sky" ){ 	loc = new Point3D(289, 3222, 20); }
					else if ( start == "swamp" ){ 	loc = new Point3D(92, 3978, 0); }
					else if ( start == "tomb" ){ 	loc = new Point3D(362, 3966, 0); }
					else if ( start == "water" ){ 	loc = new Point3D(27, 4077, 0); }
					else if ( start == "woods" ){ 	loc = new Point3D(357, 4057, 0); }
				}
				List<Item> belongings = new List<Item>();
				foreach( Item i in m.Backpack.Items )
				{
					belongings.Add(i);
				}
				foreach ( Item stuff in belongings )
				{
					stuff.Delete();
				}
				//Server.Items.BaseRace.RemoveMyClothes( m );

				m.AddToBackpack( new Gold( MyServerSettings.StartingGold() ) );

				switch ( Utility.RandomMinMax( 1, 2 ) )
				{
					case 1: m.AddToBackpack( new Dagger() ); break;
					case 2: m.AddToBackpack( new LargeKnife() ); break;
				}

				if ( Server.Items.BaseRace.NoFoodOrDrink( m.RaceID ) )
				{
					// NO NEED TO CREATE FOOD OR DRINK
				}
				else if ( Server.Items.BaseRace.NoFood( m.RaceID ) )
				{
					m.AddToBackpack( new Pitcher( BeverageType.Water ) );
				}
				else if ( Server.Items.BaseRace.BloodDrinker( m.RaceID ) )
				{
					Item blood = new BloodyDrink();
					blood.Amount = 10;
					m.AddToBackpack( blood );
				}
				else if ( Server.Items.BaseRace.BrainEater( m.RaceID ) )
				{
					Item blood = new FreshBrain();
					blood.Amount = 10;
					m.AddToBackpack( blood );
				}
				else
				{
					Container bag = new Bag();
					int food = 10;
					while ( food > 0 )
					{
						food--;
						bag.DropItem( Loot.RandomFoods( true, true ) );
					}
					m.AddToBackpack( bag );
					m.AddToBackpack( new Pitcher( BeverageType.Water ) );
				}

				if ( !Server.Items.BaseRace.NightEyes( m.RaceID ) )
				{
					int light = 2;
					while ( light > 0 )
					{
						light--;
						switch ( Utility.RandomMinMax( 1, 3 ) )
						{
							case 1: m.AddToBackpack( new Torch() ); break;
							case 2: m.AddToBackpack( new Lantern() ); break;
							case 3: m.AddToBackpack( new Candle() ); break;
						}
					}
				}

				if ( page == 1 )
				{
					PlayerSettings.SetDiscovered( m, "the Land of Sosaria", true );
				}
				else if ( page == 2 )
				{
					PlayerSettings.SetDiscovered( m, "the Land of Sosaria", true );
					PlayerSettings.SetBardsTaleQuest( m, "BardsTaleWin", true );
					MyServerSettings.SkillBegin( "fugitive", (PlayerMobile)m );
					m.Kills = 1;
					((PlayerMobile)m).Fugitive = 1;
				}
				else if ( page == 3 )
				{
					PlayerSettings.SetDiscovered( m, "the Land of Lodoria", true );
					world = "the Land of Lodoria";
				}
				else if ( page == 4 )
				{
					PlayerSettings.SetDiscovered( m, "the Land of Lodoria", true );
					PlayerSettings.SetBardsTaleQuest( m, "BardsTaleWin", true );
					MyServerSettings.SkillBegin( "fugitive", (PlayerMobile)m );
					m.Kills = 1;
					((PlayerMobile)m).Fugitive = 1;
					world = "the Land of Lodoria";
				}
				else if ( page == 5 ) 
				{
					PlayerSettings.SetDiscovered( m, "the Land of Sosaria", true );
					world = "the Land of Sosaria";
				}
				else if ( page == 6 )
				{
					PlayerSettings.SetDiscovered( m, "the Land of Sosaria", true );
					PlayerSettings.SetBardsTaleQuest( m, "BardsTaleWin", true );
					MyServerSettings.SkillBegin( "fugitive", (PlayerMobile)m );
					m.Kills = 1;
					((PlayerMobile)m).Fugitive = 1;
					world = "the Land of Sosaria";
				}
				m.Profile = Server.Items.BaseRace.BeginStory( m, world );

				if ( world == "the Land of Sosaria" )
					m.RaceHomeLand = 1;

				else if ( world == "the Land of Lodoria" )
					m.RaceHomeLand = 2;
			}
			else
			{
				switch ( page )
				{
					case 1: loc = new Point3D(2999, 1030, 0); map = Map.Sosaria; break;
					case 2: loc = new Point3D(1617, 1502, 2); map = Map.Sosaria; break;
					case 3: loc = new Point3D(851, 2062, 1); map = Map.Sosaria; break;
					case 4: loc = new Point3D(3220, 2606, 1); map = Map.Sosaria; break;
					case 5: loc = new Point3D(806, 710, 5); map = Map.Sosaria; break;
					case 6: loc = new Point3D(4546, 1267, 2); map = Map.Sosaria; break;
					case 7: MorphingTime.ColorOnlyClothes( m, 0, 1 );
							loc = new Point3D(2666, 3325, 0); map = Map.Sosaria; break;
					case 8: loc = new Point3D(2460, 893, 7); map = Map.Sosaria; break;
					case 9: loc = new Point3D(4104, 3232, 0); map = Map.Sosaria; break;
					case 10: loc = new Point3D(2111, 2187, 0); map = Map.Lodor; break;
					case 11: loc = new Point3D(2930, 1327, 0); map = Map.Lodor; break;
					case 12: loc = new Point3D(251, 1949, -28); map = Map.SavagedEmpire; break;
					case 13: loc = new Point3D(4109, 3775, 2); map = Map.Sosaria; break;
				}

				if ( page == 10 || page == 11 )
					PlayerSettings.SetDiscovered( m, "the Land of Lodoria", true );
				else if ( page == 12 )
					PlayerSettings.SetDiscovered( m, "the Savaged Empire", true );
				else
					PlayerSettings.SetDiscovered( m, "the Land of Sosaria", true );
			}

			m.MoveToWorld( loc, map );
			Effects.SendLocationParticles( EffectItem.Create( m.Location, m.Map, EffectItem.DefaultDuration ), 0x376A, 9, 32, 0, 0, 5024, 0 );
			m.SendSound( 0x65C );
			m.SendMessage( "A carta desaparece da sua mão enquanto você aparece magicamente em outro lugar." );
		}

		public override void OnResponse( NetState state, RelayInfo info )
		{
			Mobile from = state.Mobile;

			from.CloseGump( typeof( GypsyTarotGump ) );
			from.CloseGump( typeof( WelcomeGump ) );
			from.CloseGump( typeof( RacePotions.RacePotionsGump ) );

			if ( info.ButtonID == 99 )
			{
				from.SendGump( new GypsyTarotGump( from, 1 ) );
				from.SendSound( 0x5BB );
			}
			else if ( info.ButtonID >= 100 )
			{
				int go = info.ButtonID - 100;
				EnterLand( go, from );
			}
			else if ( info.ButtonID > 0 )
			{
				int page = info.ButtonID;
					if ( page == 20 ){ page = 0; }
				from.SendGump( new GypsyTarotGump( from, page ) );
				from.SendSound( 0x5B9 );
			}
		}
	}
}
