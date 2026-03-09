using System;
using Server;
using Server.Items;
using System.Text;
using Server.Mobiles;
using Server.Gumps;
using Server.Misc;
using Server.Network;
using System.Collections;
using System.Globalization;

namespace Server.Items
{
	public class DynamicBook : Item
	{
		[Constructable]
		public DynamicBook( ) : base( 0x1C11 )
		{
			Weight = 1.0;

			if ( BookTitle == "" || BookTitle == null )
			{
				ItemID = RandomThings.GetRandomBookItemID();
				Hue = Utility.RandomColor(0);
				SetBookCover( 0, this );
				BookTitle = Server.Misc.RandomThings.GetBookTitle();
				Name = BookTitle;
				BookAuthor = Server.Misc.RandomThings.GetRandomAuthor();
			}
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );
			list.Add( "Written by " + BookAuthor );
		}

		public class DynamicSythGump : Gump
		{
			public DynamicSythGump( Mobile from, DynamicBook book ): base( 100, 100 )
			{
				this.Closable=true;
				this.Disposable=true;
				this.Dragable=true;
				this.Resizable=false;
				this.AddPage(0);

				AddImage(0, 0, 30521);
				AddImage(51, 41, 11428);
				AddImage(52, 438, 11426);
				AddHtml( 275, 45, 445, 20, @"<BODY><BASEFONT Color=#FF0000>" + book.BookTitle + " by " + book.BookAuthor + "</BASEFONT></BODY>", (bool)false, (bool)false);
				AddHtml( 275, 84, 445, 521, @"<BODY><BASEFONT Color=#00FF06>" + book.BookText + "</BASEFONT></BODY>", (bool)false, (bool)true);
			}

			public override void OnResponse( NetState state, RelayInfo info )
			{
				Mobile from = state.Mobile;
				from.SendSound( 0x54D );
			}
		}

		public class DynamicJediGump : Gump
		{
			public DynamicJediGump( Mobile from, DynamicBook book ): base( 100, 100 )
			{
				this.Closable=true;
				this.Disposable=true;
				this.Dragable=true;
				this.Resizable=false;
				this.AddPage(0);

				AddImage(0, 0, 30521);
				AddImage(51, 41, 11435);
				AddImage(52, 438, 11433);
				AddHtml( 275, 45, 445, 20, @"<BODY><BASEFONT Color=#308EB3>" + book.BookTitle + " by " + book.BookAuthor + "</BASEFONT></BODY>", (bool)false, (bool)false);
				AddHtml( 275, 84, 445, 521, @"<BODY><BASEFONT Color=#00FF06>" + book.BookText + "</BASEFONT></BODY>", (bool)false, (bool)true);
			}

			public override void OnResponse( NetState state, RelayInfo info )
			{
				Mobile from = state.Mobile;
				from.SendSound( 0x54D );
			}
		}

		public class DynamicBookGump : Gump
		{
			public DynamicBookGump( Mobile from, DynamicBook book ): base( 100, 100 )
			{
				this.Closable=true;
				this.Disposable=true;
				this.Dragable=true;
				this.Resizable=false;

				string color = "#d6c382";

				this.AddPage(0);

				AddImage(0, 0, 7005, book.Hue-1);
				AddImage(0, 0, 7006);
				AddImage(0, 0, 7024, 2736);
				AddImage(362, 55, 1262, 2736);
				AddImage(408, 94, book.BookCover, 2736);
				AddHtml( 73, 49, 251, 20, @"<BODY><BASEFONT Color=" + color + ">" + book.BookTitle + "</BASEFONT></BODY>", (bool)false, (bool)false);
				AddHtml( 73, 76, 251, 20, @"<BODY><BASEFONT Color=" + color + ">by " + book.BookAuthor + "</BASEFONT></BODY>", (bool)false, (bool)false);
				AddHtml( 73, 105, 251, 290, @"<BODY><BASEFONT Color=" + color + ">" + book.BookText + "</BASEFONT></BODY>", (bool)false, (bool)true);
			}

			public override void OnResponse( NetState state, RelayInfo info )
			{
				Mobile from = state.Mobile;
				from.SendSound( 0x55 );
			}
		}

		public static void SetBookCover( int cover, DynamicBook book )
		{
			if ( cover == 0 ){ cover = Utility.RandomMinMax( 1, 80 ); }

			switch( cover )
			{
				case 1: book.BookCover = 0x4F1; break; // Man Fighting Skeleton
				case 2: book.BookCover = 0x4F2; break; // Dungeon Door
				case 3: book.BookCover = 0x4F3; break; // Castle
				case 4: book.BookCover = 0x4F4; break; // Old Man
				case 5: book.BookCover = 0x4F5; break; // Sword and Shield
				case 6: book.BookCover = 0x4F6; break; // Lion with Sword
				case 7: book.BookCover = 0x4F7; break; // Chalice
				case 8: book.BookCover = 0x4F8; break; // Two Women
				case 9: book.BookCover = 0x4F9; break; // Dragon
				case 10: book.BookCover = 0x4FA; break; // Dragon
				case 11: book.BookCover = 0x4FB; break; // Dragon
				case 12: book.BookCover = 0x4FC; break; // Wizard Hat
				case 13: book.BookCover = 0x4FD; break; // Skeleton Dancing
				case 14: book.BookCover = 0x4FE; break; // Skull Crown
				case 15: book.BookCover = 0x4FF; break; // Devil Pitchfork
				case 16: book.BookCover = 0x500; break; // Sun Symbol
				case 17: book.BookCover = 0x501; break; // Griffon
				case 18: book.BookCover = 0x502; break; // Unicorn
				case 19: book.BookCover = 0x503; break; // Mermaid
				case 20: book.BookCover = 0x504; break; // Merman
				case 21: book.BookCover = 0x505; break; // Crown
				case 22: book.BookCover = 0x506; break; // Demon
				case 23: book.BookCover = 0x507; break; // Hell
				case 24: book.BookCover = 0x514; break; // Arch Devil
				case 25: book.BookCover = 0x515; break; // Grim Reaper
				case 26: book.BookCover = 0x516; break; // Castle
				case 27: book.BookCover = 0x517; break; // Tombstone
				case 28: book.BookCover = 0x518; break; // Dragon Crest
				case 29: book.BookCover = 0x519; break; // Cross
				case 30: book.BookCover = 0x51A; break; // Village
				case 31: book.BookCover = 0x51B; break; // Knight
				case 32: book.BookCover = 0x51C; break; // Alchemy
				case 33: book.BookCover = 0x51D; break; // Symbol Man Magic Dragon
				case 34: book.BookCover = 0x51E; break; // Throne
				case 35: book.BookCover = 0x51F; break; // Ship
				case 36: book.BookCover = 0x520; break; // Ship with Fish
				case 37: book.BookCover = 0x579; break; // Bard
				case 38: book.BookCover = 0x57A; break; // Thief
				case 39: book.BookCover = 0x57B; break; // Witches
				case 40: book.BookCover = 0x57C; break; // Ship
				case 41: book.BookCover = 0x57D; break; // Village Map
				case 42: book.BookCover = 0x57E; break; // World Map
				case 43: book.BookCover = 0x57F; break; // Dungeon Map
				case 44: book.BookCover = 0x580; break; // Devil with 2 Servants
				case 45: book.BookCover = 0x581; break; // Druid
				case 46: book.BookCover = 0x582; break; // Star Magic Symbol
				case 47: book.BookCover = 0x583; break; // Giant
				case 48: book.BookCover = 0x584; break; // Harpy
				case 49: book.BookCover = 0x585; break; // Minotaur
				case 50: book.BookCover = 0x586; break; // Cloud Giant
				case 51: book.BookCover = 0x960; break; // Skeleton Warrior
				case 52: book.BookCover = 0x961; break; // Lich
				case 53: book.BookCover = 0x962; break; // Mind Flayer
				case 54: book.BookCover = 0x963; break; // Lizard
				case 55: book.BookCover = 0x521; break; // Mondain
				case 56: book.BookCover = 0x522; break; // Minax
				case 57: book.BookCover = 0x523; break; // Serpent Pillar
				case 58: book.BookCover = 0x524; break; // Gem of Immortality
				case 59: book.BookCover = 0x525; break; // Wizard Den
				case 60: book.BookCover = 0x526; break; // Guard
				case 61: book.BookCover = 0x527; break; // Shadowlords
				case 62: book.BookCover = 0x528; break; // Gargoyle
				case 63: book.BookCover = 0x529; break; // Moongate
				case 64: book.BookCover = 0x52A; break; // Elf
				case 65: book.BookCover = 0x52B; break; // Shipwreck
				case 66: book.BookCover = 0x52C; break; // Black Demon
				case 67: book.BookCover = 0x52D; break; // Exodus
				case 68: book.BookCover = 0x52E; break; // Sea Serpent
				case 69: book.BookCover = 0x530; break; // Hydra
				case 70: book.BookCover = 0x531; break; // Beholder
				case 71: book.BookCover = 0x532; break; // Flying Castle
				case 72: book.BookCover = 0x533; break; // Serpent
				case 73: book.BookCover = 0x534; break; // Ogre
				case 74: book.BookCover = 0x535; break; // Skeleton Graveyard
				case 75: book.BookCover = 0x536; break; // Shrine
				case 76: book.BookCover = 0x537; break; // Volcano
				case 77: book.BookCover = 0x538; break; // Castle
				case 78: book.BookCover = 0x539; break; // Dark Knight
				case 79: book.BookCover = 0x53A; break; // Skull Ring
				case 80: book.BookCover = 0x53B; break; // Serpents of Balance
			}
		}

		public override void OnDoubleClick( Mobile e )
		{
			if ( this.Weight == -50.0 || ( e.InRange( this.GetWorldLocation(), 5 ) && e.CanSee( this ) && e.InLOS( this ) ) )
			{
				if ( ItemID == 0x4CDF )
				{
					e.CloseGump( typeof( DynamicBookGump ) );
					e.CloseGump( typeof( DynamicSythGump ) );
					e.CloseGump( typeof( DynamicJediGump ) );
					e.SendGump( new DynamicSythGump( e, this ) );
					e.SendSound( 0x54D );
				}
				else if ( ItemID == 0x543C )
				{
					e.CloseGump( typeof( DynamicBookGump ) );
					e.CloseGump( typeof( DynamicSythGump ) );
					e.CloseGump( typeof( DynamicJediGump ) );
					e.SendGump( new DynamicJediGump( e, this ) );
					e.SendSound( 0x54D );
				}
				else
				{
					e.CloseGump( typeof( DynamicSythGump ) );
					e.CloseGump( typeof( DynamicBookGump ) );
					e.CloseGump( typeof( DynamicJediGump ) );
					e.SendGump( new DynamicBookGump( e, this ) );
					e.SendSound( 0x55 );
				}
				Server.Gumps.MyLibrary.readBook ( this, e );
			}
			else
			{
				e.SendMessage( "That is too far away to read." );
			}
		}

		public DynamicBook(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int) 0);
			writer.Write( BookCover );
			writer.Write( BookTitle );
			writer.Write( BookAuthor );
			writer.Write( BookText );
			writer.Write( BookRegion );
			writer.Write( BookMap );
			writer.Write( BookWorld );
			writer.Write( BookItem );
			writer.Write( BookTrue );
			writer.Write( BookPower );
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
			BookCover = reader.ReadInt();
			BookTitle = reader.ReadString();
			BookAuthor = reader.ReadString();
			BookText = reader.ReadString();
			BookRegion = reader.ReadString();
			BookMap = reader.ReadMap();
			BookWorld = reader.ReadString();
			BookItem = reader.ReadString();
			BookTrue = reader.ReadInt();
			BookPower = reader.ReadInt();
		}

		public int BookCover;
		[CommandProperty(AccessLevel.Owner)]
		public int Book_Cover { get { return BookCover; } set { BookCover = value; InvalidateProperties(); } }

		public string BookTitle;
		[CommandProperty(AccessLevel.Owner)]
		public string Book_Title { get { return BookTitle; } set { BookTitle = value; InvalidateProperties(); } }

		public string BookAuthor;
		[CommandProperty(AccessLevel.Owner)]
		public string Book_Author { get { return BookAuthor; } set { BookAuthor = value; InvalidateProperties(); } }

		public string BookText;
		[CommandProperty(AccessLevel.Owner)]
		public string Book_Text { get { return BookText; } set { BookText = value; InvalidateProperties(); } }

		public string BookRegion;
		[CommandProperty(AccessLevel.Owner)]
		public string Book_Region { get { return BookRegion; } set { BookRegion = value; InvalidateProperties(); } }

		public Map BookMap;
		[CommandProperty(AccessLevel.Owner)]
		public Map Book_Map { get { return BookMap; } set { BookMap = value; InvalidateProperties(); } }

		public string BookWorld;
		[CommandProperty(AccessLevel.Owner)]
		public string Book_World { get { return BookWorld; } set { BookWorld = value; InvalidateProperties(); } }

		public string BookItem;
		[CommandProperty(AccessLevel.Owner)]
		public string Book_Item { get { return BookItem; } set { BookItem = value; InvalidateProperties(); } }

		public int BookTrue;
		[CommandProperty(AccessLevel.Owner)]
		public int Book_True { get { return BookTrue; } set { BookTrue = value; InvalidateProperties(); } }

		public int BookPower;
		[CommandProperty(AccessLevel.Owner)]
		public int Book_Power { get { return BookPower; } set { BookPower = value; InvalidateProperties(); } }

		public static string BasicHelp()
		{
			string text = "CONCEITOS BÁSICOS DO JOGO<BR><BR>Jogar no " + MySettings.S_ServerName + " é bastante simples, pois a interface é bastante intuitiva. Embora o jogo tenha mais de 20 anos, alguma explicação é necessária. Após fazer login e estar no mundo do jogo, você verá um livro aberto com habilidades. Clique com o botão direito neste livro para fechá-lo, pois você não precisará dele para este jogo. Quase qualquer janela pode ser fechada com um clique direito. Seu personagem está sempre no centro da tela de jogo. Para viajar, simplesmente mova o mouse sobre a exibição do mundo do jogo... então clique e segure o botão direito. O cursor do mouse sempre apontará para longe do seu personagem, que se moverá na direção indicada (por exemplo, se você deseja andar para cima na tela, mantenha o cursor acima do seu personagem). Você continuará indo nessa direção até encontrar um obstáculo ou soltar o botão do mouse. Quanto mais longe o cursor estiver do seu personagem, mais rápido o personagem se moverá. Clicar duas vezes com o botão direito fará com que seu personagem se mova para o ponto exato onde o cursor estava... a menos que desabilitado nas opções.<br><br>PAPERDOLL: O paperdoll do seu personagem estará aberto quando você iniciar. Se não estiver, pressionar Alt P o abrirá para você. Abaixo explicarei o que ele faz. O lado esquerdo mostra caixas para o slot mostrando o que está na sua cabeça, depois o slot mostrando o que está nas suas orelhas, depois o slot mostrando o que está no seu pescoço, depois o slot mostrando o que está no seu dedo, por último o slot mostrando o que está no seu pulso. A parte inferior mostrará seu nome e seu título. Às vezes é personalizado, enquanto na maioria das vezes é sua habilidade mais praticada, junto com qualquer fama e/ou karma que você ganhou. O lado direito tem vários botões. Pressionar o botão AJUDA traz um menu de ajuda simples. A única coisa que você deve usar aqui é quando seu personagem está fisicamente preso no mundo do jogo e precisa ser teleportado para fora. Ele irá teleportá-lo para uma área segura em algum lugar da terra. O botão OPÇÕES trará suas opções para o jogo (discutido posteriormente). O botão SAIR faz você sair do jogo. Certifique-se de estar em um local seguro. O botão ESTATÍSTICAS trará algumas estatísticas vitais sobre seu personagem (discutido posteriormente). O botão HABILIDADES trará todas as habilidades disponíveis no jogo. Aqui você gerencia sua progressão de habilidades (discutido posteriormente). O botão GUILDA permite que você comece sua própria guilda. Custará dinheiro para começar, mas você pode convidar outros jogadores e compartilhar casas e conversar uns com os outros. O botão PAZ alterna se você está pronto para lutar... ou não. Por último, o botão STATUS trará o status do seu personagem (discutido posteriormente). O centro mostra seu personagem. Aqui você pode arrastar e soltar roupas, armaduras e outros equipamentos usados pelo seu personagem. Clique duas vezes no pergaminho esquerdo para ver há quanto tempo sua conta existe. Clique duas vezes no pergaminho direito para organizar um grupo de outros jogadores. Isso é importante se você planeja compartilhar as recompensas da exploração de masmorras. Clique duas vezes na mochila para abrir sua mochila (discutido posteriormente).<br><br>BARRA DE MENU: Esta barra de menu pode permitir que você tenha acesso a certos itens mais rapidamente. Pode ser desabilitada nas opções. O pequeno triângulo minimizará a barra de menu. O botão MAPA abrirá um mini mapa da área ao seu redor. Pressioná-lo uma segunda vez tornará o mapa um pouco maior (Alt R também faz isso). O botão PAPERDOLL abrirá seu paperdoll. O botão INVENTÁRIO abrirá sua mochila (discutido posteriormente). O botão DIÁRIO abrirá seu diário, que mostra as coisas mais recentes que você viu ou ouviu. O botão CHAT não funciona, pois a opção de chat está desabilitada. Digite o comando [c instead. O botão AJUDA trará o menu de ajuda que já foi discutido. O botão ? traz uma tela de informação muito desatualizada. É melhor não usá-lo.<br><br>MOCHILA: Quando você clica duas vezes na mochila no paperdoll, ela abrirá (Alt I também fará isso). Você só pode carregar uma certa quantidade de peso com base na sua força. Se sua força for extremamente alta, então você está à mercê de quanto a mochila pode realmente aguentar. A imagem à direita mostra como você pode realmente ter recipientes dentro da mochila para ajudar a organizar as coisas melhor. Você pode arrastar e soltar itens entre os recipientes. Às vezes, seus recipientes fecharão quando você viajar entre diferentes mundos. Se você fechar um recipiente que tem outros recipientes abertos dentro dele, esses recipientes também fecharão. OPÇÕES Pressionar o botão de opções abrirá esta janela (assim como pressionar Alt O). Você pode mudar muitas coisas na seção de opções. Você pode controlar o volume da música e dos sons. Você pode alterar as fontes e cores dessas fontes. Você pode configurar macros para criar teclas de atalho para séries de comandos comumente usadas. Você também pode filtrar obscenidades. É também aqui que você pode definir sua busca de caminho, modo de guerra, sistema de mira e opções da barra de menu. Você pode escolher deslocar suas janelas de interface (como recipientes) ao abrir. Preste atenção às opções de macro, pois você pode aprender sobre algumas das teclas de atalho pré-construídas... junto com aprender a dirigir navios quando puder comprar um.<br><br>ESTATÍSTICAS: Há muitos aspectos do seu personagem, e o botão de estatísticas os exibirá para você. Você pode ver o que compõe suas habilidades e se tem algum bônus para regeneração de estatísticas como mana, vigor ou pontos de vida. Você pode ver os valores do seu karma e fama (que também podem aparecer como um título no seu paperdoll). Sua fome e sede serão mostradas para que você possa determinar se precisa comer ou beber (é claro, o jogo lhe dirá quando você estiver morrendo de fome ou sede sem esta estatística). Você pode dizer quão rápido você pode lançar feitiços ou aplicar ataduras. Se você assassinou alguém inocente, você pode ver esse valor aqui. Se você usar pontos de dízimo (pessoas praticando cavalaria), você pode ver esse valor aqui para saber se precisa fazer uma doação de ouro a um santuário mais cedo ou mais tarde.<br><br>HABILIDADES: Pressionar o botão de habilidades abrirá isto (assim como pressionar Alt K). Aqui você pode ver as muitas habilidades disponíveis no jogo para seu personagem se tornar proficiente. Você terá um máximo de 1.000 pontos de habilidade para usar. A habilidade com os pontos azuis à esquerda são habilidades que são ativadas para usar na maioria das vezes (significando que, às vezes, elas também estão trabalhando em segundo plano). Para estas habilidades, você pode clicar e arrastá-las para fora deste pergaminho e isso fará um botão na sua tela que você pode clicar para ativar a habilidade no futuro. À direita de cada valor há uma seta para cima que você pode mudar para uma seta para baixo ou um cadeado. Você pode travar uma habilidade em um certo valor para que ela não aumente ou diminua mais do que isso. Você também pode mudá-la para uma seta para baixo. Isso dirá ao jogo que esta habilidade diminuirá se outra habilidade aumentar (e você tiver usado todos os 1.000 pontos). Você pode ver o exemplo à direita. O número inferior direito de 193,1 indica que este personagem usou tantos pontos de habilidade até agora. Alguns itens mágicos adicionam às suas habilidades e serão refletidos aqui. Se você apenas quiser ver os valores de habilidade (sem a adição que os itens mágicos fornecem), clique na opção 'mostrar real' no canto inferior direito. A opção 'mostrar limites' mostrará o valor máximo que você pode ter uma habilidade. Cada habilidade tem permissão para ir até 100 cada (sem ultrapassar 1.000 no total). Você pode encontrar pergaminhos de poder que permitirão que uma habilidade ultrapasse 100, e esta opção mostrará isso. As habilidades são organizadas por categoria e você pode até clicar no botão 'novo grupo' para fazer um grupo próprio. Então você pode arrastar e soltar habilidades neste 'grupo' para poder selecionar um conjunto particular de habilidades que você pode querer manter sob observação para aquele personagem.<br><br>STATUS: A janela de status mostra a força, destreza, inteligência, pontos de vida, vigor, mana, sorte, peso carregado, seguidores, dano e ouro carregado do seu personagem. Você também pode ver o valor máximo permitido para força, destreza e inteligência (sempre 250). Clicar duas vezes nesta janela a alternará de uma visualização detalhada para uma visualização de barra menor. Você pode configurar sua força, destreza e inteligência para aumentar e diminuir de forma semelhante às habilidades descritas acima... com as setas à esquerda de cada valor. No lado direito estão seus valores de defesa contra físico, fogo, frio, veneno e energia. Todas as criaturas têm esses valores e alguns ataques causam dano em todas ou algumas dessas categorias. Um dia você vai querer que todos esses valores sejam altos (máximo de 70% em cada). O restante dos recursos do jogo pode ser aprendido enquanto estiver no jogo. Como exemplo, você pode aprender mais alguns comandos a partir da mensagem do dia. Você também pode visitar um sábio e comprar um pergaminho que detalhará o que todas as habilidades fazem e como usá-las. Muitos comandos você pode digitar no canto inferior esquerdo da janela de visualização do mundo digitando um símbolo '[' (sem aspas)... junto com o comando. Por exemplo, '[c' trará a janela de chat. '[status' trará a janela de estatísticas. '[motd' trará a mensagem do dia. Na janela da mensagem do dia, pressione o ? no canto superior direito para aprender mais comandos. Caberá a você explorar. Agora vamos entrar em algumas das coisas comuns que você provavelmente fará no jogo.<br><br>CHAT: Este é um meio de se comunicar dentro do mundo do jogo quando você não está na mesma tela que outro jogador. Digite '[c' para começar a usar o sistema de chat. Isso também permite que você envie uma mensagem para outro jogador que eles possam ler mais tarde. Tenha em mente que isso é específico do personagem e não da conta. Se você enviar uma mensagem para um personagem, mas o jogador fizer login com um personagem diferente, ele não verá a mensagem até fazer login com 'aquele personagem'. Este recurso de chat tem muitas opções que os sistemas de chat da Internet têm. Você pode ver quem está online. Você pode estabelecer canais. Você pode até definir alguns níveis de privacidade para ignorar outros ou não ser visto.<br><br>CIDADÃOS: Muitos cidadãos têm um menu de contexto quando você clica uma vez com o botão esquerdo neles. Isso traz uma lista de serviços que eles fornecem para você. Alguns podem estar esmaecidos por serem algo que eles não podem fornecer a 'você' em particular (se eles treinam alfaiataria, por exemplo, e você já é um mestre nisso, ficará esmaecido porque eles não podem te ensinar mais sobre isso). Muitos cidadãos têm uma opção 'contratar'. Certifique-se de explorar o que você pode contratá-los para fazer por você. Pode ser útil mais tarde. Tenha cuidado ao clicar uma vez com o botão esquerdo neles, pois você ainda pode estar no modo de guerra e atacá-los acidentalmente. Se eles sobreviverem ao seu ataque inicial, fuja ou arrisque-se a se tornar um assassino. Assassinatos levam cerca de 8 horas de tempo real (enquanto no jogo) para desaparecer. Levará 40 horas de tempo real (enquanto no jogo) para desaparecer se você continuar a assassinar enquanto é um assassino. ";
			if ( MySettings.S_Bribery >= 1000 )
			{
				text = text + "Outra opção é visitar o Mestre da Guilda de Assassinos, onde você pode contratá-los por " + MySettings.S_Bribery + " de ouro para convencer os guardas a esquecerem um único assassinato que você possa ter cometido. Membros da Guilda de Assassinos pagam apenas metade desse valor, e fugitivos são bem conhecidos demais para serem perdoados por subornos. Se você não tiver ouro suficiente em sua mochila, eles simplesmente o tirarão de seu cofre bancário. ";
			}
			text = text + "Você não será permitido em um assentamento enquanto for um assassino, a menos que talvez se disfarce. Criminosos, por outro lado, perderão seu status de criminoso em alguns minutos.<br><br>BANCOS: Bancos são um local seguro para deixar seus objetos de valor. Você não pode carregar tudo com você e isso funciona bem até que você possa pagar uma casa. Além disso, o mundo é perigoso. Você vai querer guardar equipamento extra no banco caso perca seu conjunto favorito. Os banqueiros são atenciosos e se você simplesmente entrar no prédio e dizer 'banco', eles lhe darão acesso ao seu cofre bancário. Bancos são o único lugar que aceitará sua moeda não dourada e a trocará por ouro. Você encontrará moedas de cobre e prata por aí e precisa convertê-las. Entregá-las a um banqueiro fará isso. Você também pode colocar as moedas em seu cofre bancário e clicar duas vezes nelas. Isso também as converterá. Se você precisar carregar uma grande quantidade de ouro para algum lugar, pode converter fundos para um documento oficial chamado cheque. Para fazer isso, simplesmente diga a palavra 'cheque' e o valor.<br><br>ESTALAGENS E TAVERNAS: Estalagens (placas de vela) e Tabernas (placas de vinho/uva) são os locais seguros para aventureiros descansarem e relaxarem. Você não pode lançar feitiços aqui ou atacar qualquer outra pessoa. Esses lugares são bons para negociar trocas, comprar comida e bebida, ou simplesmente conversar e jogar os jogos que eles oferecem (a taverna oferece jogos... não as estalagens). Você sai instantaneamente quando estiver nesses lugares, caso contrário... levará alguns minutos para seu personagem sair quando estiver fora. Tabernas são um bom lugar para contratar capangas para aventurar também. Às vezes, também há frequentadores de bar em tavernas que contarão sobre lugares para ir e rumores que ouviram.<br><br>PRÁTICA: Alguns assentamentos têm um lugar para você praticar suas habilidades de arma. Equipe uma arma colocando-a na mão do paperdoll e então clique duas vezes em um boneco de treinamento. Você só pode treinar uma habilidade de arma até certo ponto neles, mas isso o ajudará a começar. Para armas de longo alcance, use um alvo de tiro com arco para praticar. Se você conseguir encontrar um ladrão, eles geralmente têm bonecos com os quais você pode praticar furto de bolso.<br><br>COMBATE: Para começar uma luta com alguém (ou algo), pressione o botão de paz no paperdoll para entrar no modo de guerra. Então clique duas vezes em um alvo. Você estará agora em uma luta. Lembre-se de pressionar o botão novamente para voltar ao modo de paz, para não atacar acidentalmente ninguém. A maioria das armas precisa de você cara a cara com seu oponente, enquanto arcos permitem que você fique a uma certa distância. Você também pode atacar com seguidores ou feitiços. Lançar um feitiço nocivo em outro também iniciará uma luta. Tenha em mente, há momentos em que você pode precisar fugir de uma luta. É melhor viver e lutar outro dia. Matar cidadãos ou outros jogadores lhe dará contagens de assassinato. A exceção é quando você mata outro jogador que já é um assassino ou criminoso. Você pode dizer quando os destaca. Se estiverem vermelhos, são assassinos. Se estiverem cinza, são criminosos.<br><br>ARMAS: Existem muitas armas diferentes no jogo. Passar o cursor sobre elas mostrará as estatísticas da arma. Você pode ver o dano que causa e o tipo de dano. Você pode ver com que frequência pode balançar a arma (ou atirar). Haverá um requisito de força para usar a arma, junto com o tipo de habilidade ao usar a arma (esgrima, pontaria, etc). A arma indicará se é uma arma de uma ou duas mãos. Isso é importante se você quiser segurar um escudo, tocha ou lanterna. Armas de duas mãos não permitem isso. Algumas armas terão propriedades mágicas que também serão listadas dessa maneira. Cada arma tem uma durabilidade. Seu item quebrará se ficar muito baixo, então você precisa consertá-lo sozinho (geralmente com habilidades de ferreiro ou arco e flecha) ou encontrar um cidadão que possa contratar para consertá-lo para você.<br><br>ARMADURA: A armadura não cobre apenas as armaduras medievais de metal dos cavaleiros, mas também os itens menores como luvas de couro e chapéus. Se você olhar para a direita, pode ver que o chapéu do mago tem alguns bônus para resistências (veja a seção sobre Status). A armadura tem um requisito de força e durabilidade como as armas. Você pode comprar muitas peças de armadura que cobrem muitas seções do corpo. Áreas como mãos, braços, pernas, pescoço, tórax e cabeça podem ter algum tipo de armadura vestida. Você pode equipá-las como uma arma ou joia. Arraste e solte a peça da armadura no paperdoll. Se não equipar, você não é forte o suficiente ou já tem uma peça de armadura ou roupa naquele local. Quanto mais pesada a armadura, menos provável você poderá meditar ou se esgueirar. A armadura de couro é geralmente boa para fazer tais coisas, onde mesmo mudar para armadura de couro cravejada pode ser muito pesada. Qualquer armadura mostrada como 'armadura de mago' permitirá que você medite ou se esgueire, não importa o peso.<br><br>PORTÕES MÁGICOS: Você pode encontrar um portão mágico que leva a outro lugar. Nada a fazer além de ir embora ou entrar. Eles vêm em diferentes cores de azul, vermelho ou preto. Magos poderosos podem invocar esses portões, enquanto algumas poções necromânticas podem invocar portões negros. Se você aprender para onde esses portões vão, poderá explorar novos mundos ou se locomover pelo mundo atual muito mais rápido. Às vezes, matar uma criatura mística criará um portão, mas esses são muito... muito raros.<br><br>RUNAS MÁGICAS: Runas de Recall são pequenas pedras marrons com um símbolo de ankh dourado nelas, nas quais se pode lançar um Feitiço de Marca. Isso marcará a localização atual da runa naquele local. Pode-se usar magia para então transportar de volta para aquele local. Quando as runas são marcadas, elas mudarão de cor dependendo do mundo para onde vão. Você pode comprar/fazer livros de runas que permitirão que você coloque uma pedra rúnica para uso mais fácil. Você tem permissão para 16 por livro. Soltar pergaminhos do Feitiço de Recall no livro aumentará as cargas para que você possa usar o livro para simplesmente teleportar para esses locais sem saber magia.<br><br>LIVROS DE MAGIA: Existem alguns livros de magia no mundo. O que os magos usam é um simples livro de feitiços, enquanto os necromantes usam um livro de feitiços de necromancia. Ambos os livros precisam ter pergaminhos de feitiços escritos neles, soltando o pergaminho de feitiço apropriado no livro. Você pode clicar duas vezes em um pergaminho e tentar lançar a partir dele, mas ele simplesmente desaparecerá depois. Colocá-lo em um livro permite que você continue usando-o repetidamente. Os outros três livros são para cavalaria, samurai e ninjas. Eles não são como livros de feitiços, pois são simplesmente livros que permitem que você use essas habilidades especiais. Não importa o livro, você pode arrastar e soltar ícones deles para ter uma maneira mais rápida de lançar/usá-los. Os feitiços de mago e necromante exigem componentes de feitiço. Você também pode visitar um sábio ou escriba para maneiras ainda mais fáceis de usar essa magia.<br><br>MORTE: Você pode se encontrar sob uma série de eventos infelizes... e assim deixar a terra dos vivos. Não tema! Você pode retornar de algumas maneiras diferentes. A primeira escolha é ressuscitar com certas penalidades para suas habilidades e atributos. Isso é rápido, mas o preço para as progressões do seu personagem dará um grande passo para trás. Você poderia ter um camarada para ressuscitá-lo, seja por magia ou cura... ou você pode ter sorte o suficiente para ter consumido uma poção de ressurreição antes de sua morte. Por último, você poderia simplesmente levar sua alma e procurar na terra por um curandeiro ou santuário de onde você pode se fazer ressuscitar. Para que isso ocorra, um curandeiro pedirá uma doação por tais serviços... ou um santuário exigirá tributo aos deuses. Você precisará ter algum ouro guardado em seu cofre bancário ou ouro dizimado para aproveitar isso. Você pode ser ressuscitado sem a taxa ou penalidades se seus atributos totais forem 90 ou menos, e seu total de habilidades for 200 ou menos.<br><br>FINALMENTE: Isso deve dar a você informações mais do que suficientes para começar sua aventura neste mundo. Explore e experimente coisas diferentes e você descobrirá muitas opções disponíveis para seu personagem. Um dos primeiros conselhos seria procurar um sábio, pois eles vendem informações valiosas que podem ajudá-lo a aprender mais sobre como as coisas funcionam. Eles têm pergaminhos que contêm informações sobre como ser ferreiro, alfaiate ou encontrar recursos no mundo cavando ou esfolando animais. Você pode aprender como cortar madeira, roubar de masmorras, evitar armadilhas e até sobre os diferentes tipos de reagentes. Descubra como você vai conseguir comida e bebida, pois precisará delas para sua jornada. Boa sorte em suas aventuras!";

			return text;
		}

		public static void SetStaticText( DynamicBook book )
		{
			if ( book is TendrinsJournal ){ book.BookText = "Entrada 1 - Hoje parece ser um bom dia para Skara Brae. Todos os moradores estão se preparando para a celebração do outono. Eu mesmo estou fazendo pães doces para o grande jantar desta noite. Melhor eu pôr mãos à obra.<br><br>Entrada 2 - A celebração não foi bem. Mangar estava lá e estava ameaçando muitos membros do conselho. Aparentemente, há alguma lenda sobre um artefato perdido em ruínas abaixo de Skara Brae, e ele quer permissão para escavar. O conselho é fraco, mas com o apoio de Lord British... Mangar pode fazer muito pouco para forçá-los a atender suas demandas. Ele eventualmente saiu furioso para sua torre. Vou perguntar ao conselho se querem que eu espie sua torre amanhã. Fica no centro de Sosaria, então será uma longa caminhada... mas acho que precisamos saber o que ele está tramando.<br><br>Entrada 3 - Estou indo com a bênção do conselho. Será uma longa jornada, mas devo chegar lá amanhã cedo. Montei acampamento e estou pronto para descansar pela noite.<br><br>Entrada 4 - Estou de volta a Skara Brae, pois viajei a noite toda para chegar aqui. As coisas estão piorando. Mangar não estava em sua torre quando cheguei, mas escalei as paredes e revistei seu estudo. Havia muitos pergaminhos e livros. Pelo que pude ler, ele está planejando algo horrível para nós. Seu plano é transportar magicamente Skara Brae para o Vazio. Se ele conseguir isso, não seremos capazes de obter ajuda externa e Mangar poderá fazer o que quiser com Skara Brae. Discuti isso com o conselho esta noite e eles enviaram um mensageiro para Britain pedindo ajuda.<br><br>Entrada 5 - Mangar chegou a Skara Brae esta manhã e está com o conselho desde que chegou. Fui até o prédio para escutar a conversa. Ele instou o conselho a atender suas demandas para escavar. Se não o fizerem, ele desencadeará uma magia horrível sobre nós e nos enviará para o Vazio. Quando um dos membros do conselho afirmou que os magos de Lord British nos encontrariam e nos trariam de volta do Vazio... foi quando Mangar disse: 'Ninguém em Sosaria saberá que estamos no Vazio'. O que isso significa?<br><br>Entrada 6 - Voltei à torre de Mangar na esperança de pôr fim à sua tirania. Ele não estava lá novamente, e a torre parecia descuidada, para dizer o mínimo. Revirando mais pergaminhos, descobri o que ele quis dizer no outro dia. Seu plano é fazer parecer que Skara Brae está em ruínas. Toda Sosaria pensará que perecemos em algum tipo de desastre. Devo voltar e avisar os outros. <br><br>Entrada 7 - Cheguei tarde demais. Skara Brae se foi, e não há nada além de casas destruídas ao meu redor. Estou sentado no chão do que foi minha casa, sangrando até a morte por uma facada nas costas. Distraído pelo vórtice mágico mais cedo, Mangar se aproximou por trás e me esfaqueou. Ele então teleportou para longe com uma risada sinistra. Parece que ele venceu. Que os deuses ajudem todos aqueles presos em Skara Brae."; }
			else if ( book is BookGuideToAdventure || book is LoreGuidetoAdventure ){ book.BookText = BasicHelp(); }
			else if ( book is BookBottleCity ){ book.BookText = "Começou com apenas alguns. Um experimento concebido pela mente do Cavaleiro Negro. Vordo, um de seus magos mais altos, trabalhou por anos para aperfeiçoar o feitiço que eventualmente engoliu a pequena ilha de Kuldar perto da Ilha da Serpente. Os gargulas temiam o poder do Cavaleiro Negro, pois acreditavam que a ilha havia sido destruída. A verdade era algo muito mais sinistro. Dentro da garrafa mística, a ilha fica flutuando na água que abriga a vida que foi trazida com ela. O Cavaleiro Negro exilou alguns de seus prisioneiros para a garrafa para viverem seus anos restantes. Séculos se passaram e esses prisioneiros cultivaram a terra, construíram uma cidade, tiveram filhos e viveram em prosperidade. Vordo decidiu que queria governar esta terra como o Cavaleiro Negro governa a dele. Ele disse ao Cavaleiro Negro que a garrafa foi destruída em um acidente, algo com que o Cavaleiro Negro se importou muito pouco, pois estava envolvido em outros assuntos de interesse. Vordo deixou seu castelo cair na garrafa, onde ele caiu next to à cidade dentro dela. Ele entrou magicamente na garrafa e invocou um portão lunar para sair um dia, se desejasse. Ele governou com punho de ferro por quase uma década até que os cidadãos se levantaram e puseram fim à sua tirania. Eles selaram seu castelo e trancaram quaisquer horrores que Vordo criou inside. Lendas contam de seu fantasma vagando por esses corredores, onde ele carrega as notas que lhe permitiriam sair da garrafa. Se eu pudesse banir seu espírito para a verdadeira morte, mesmo que por um breve momento, poderia tomar suas notas e usar magia de portão ou recall para escapar deste lugar."; }
			else if ( book is BookofDeadClue ){ book.BookText = "Ele navegou pelo mundo, capturando as muitas almas perdidas que nadavam por perto. O capitão solitário, um necromante, dirigia a embarcação para águas de morte e miséria. Aqueles que vivem buscam o navio enegrecido, em busca do famoso Livro dos Mortos. Com este poder, um necromante treinado pode pegar as partes do corpo dos mortos e criar um demônio ambulante de poder sem mente. Apenas o coração sombrio deve ser obtido para realizar tal feito. A lenda conta que a embarcação sombria frequentemente recua para Ambrosia, onde a única maneira de embarcar nela é pronunciar a palavra mortal de poder, 'necropalyx'. Lembre-se bem desta palavra, pois ela deve ser falada para escapar do navio. Para entrar no porão mortal, você deve encontrar o pentagrama sombrio e falar a palavra. O navio deve estar ancorado nearby."; }
			else if ( book is CBookTombofDurmas ){ book.BookText = "O Rei Durmas IV tinha um alto mago em seu conselho que buscava a magia da imortalidade. Embora encarregado de fazer isso pelo Rei, este mago realmente buscava o poder para si mesmo. O sucesso deste mago não foi conhecido até séculos depois, quando ele ressuscitou de sua sepultura e quis controlar todos os mortos do mundo, matando os vivos em seu rastro. Até que possamos colocar este poderoso lich sob controle, ele permanecerá para sempre enterrado na cripta da família Durmas. Há apenas uma entrada e saída da cripta. Há um altar de pedra construído onde falar a palavra 'xormluz' enviará magicamente aquele que está sobre o altar para aparecer na cripta selada. Falar as palavras no altar do lado oposto trará aqueles de volta da cripta. A pesquisa continua."; }
			else if ( book is CBookElvesandOrks ){ book.BookText = "Conta-se que elfos e orks existem, mas suas terras estão a mundos de distância. Orks, as relações mais civilizadas com orcs, vivem dentro do Império Selvagem. Os elfos, ricos em cultura e raridades, vivem em uma enorme terra de Lodor. A ponte entre o vale e Lodor é dita ser uma caverna gelada. Os elfos só vão lá para visitar as montanhas, onde se diz que os deuses podem fazer itens raros e maravilhosos."; }
			else if ( book is MagestykcClueBook ){ book.BookText = "O Conselho de Magos já teve o suficiente das práticas bárbaras das quais este grupo Magestykc tem participado. A invocação de demônios para nossos reinos não será tolerada. Embora não possam ser todos encontrados, a maioria deles foi banida para uma parte do underworld para viver seus dias restantes. Lá eles podem invocar os senhores do inferno e ser exilados com eles. Seu grande mago escapou, no entanto, e pode muito facilmente fazer um portão mágico entre Sosaria e a prisão do underworld. Temo que este dia chegará e devemos nos preparar para o apocalipse que se aproxima. Enviaremos alguns de nossos melhores magos para ver se este portal foi de fato criado. Eles costumam falar o nome de seu grupo para ativá-lo, então devemos ter pouca dificuldade em encontrá-lo."; }
			else if ( book is FamiliarClue ){ book.BookText = "Passei dias nesta masmorra maldita, procurando por pistas da existência das terras dos gargulas. Eu tinha comida e água suficiente para durar dias, mas não pude carregar tudo. Ouvi de outros magos que os mestres de guild frequentemente procuram por rubis. Aparentemente, eles são usados para alguns tipos de conjuração de feitiços. Eles aceitam doações felizes, mas se um aprendiz oferecer 20 ou mais, geralmente recebem um presente. Não importa se você pratica magia ou magia negra, contanto que tenha alcançado o nível de habilidade de aprendiz em tais campos. Havia algo também semelhante que ouvi de outro conjurador de feitiços de que o mestre de guild da magia negra tem uma predileção por safiras estelares. Não encontrei nenhuma delas. Durante minha última jornada, encontrei 23 rubis em um baú de metal e os dei ao mestre de guild de magos. Ele me deu uma bola de cristal, que invoca um familiar para me servir. Não faz muito em termos de serviços, mas carregará algumas das minhas coisas para mim e me fará companhia. A bola de cristal tinha apenas 5 cargas, mas os mestres de guild de magos podem ser contratados para carregá-la ainda mais. Recebi um demôniozinho como companheiro de viagem, mas eu queria um gato preto. Devolvi a bola de cristal ao mago, onde ele me deu outra para olhar. Simplesmente continuei passando-as de volta para ele até conseguir o gato que procurava. O mago me disse que eu poderia usar cores de tubos de tintura comuns e derramá-las na bola de cristal. Ela reteria a cor e, assim, o familiar compartilharia essa cor. Ele estava certo. Finalmente tinha meu familiar de gato preto, assim como outros magos famosos tiveram. Chamei-o de Moonbeam. Estou descansando agora, no fundo deste lugar. Continuarei minha busca pela manhã. Ouço algo nearby. Devo ver o que é."; }
			else if ( book is LodorBook ){ book.BookText = "Por anos procurei uma maneira de viajar para o mundo de Lodor, o que alguns chamam de terra dos elfos. Embora um mito para muitos, finalmente cheguei a este novo mundo. Parece ser um lugar pacífico com muitas cidades. Encontrei a Cidade de Lodoria, onde os sábios foram capazes de me ensinar como ganhar mais poder no uso da feitiçaria. Estou morrendo com a passagem do tempo e este novo poder me ajudará a finalmente terminar os rituais necessários para me tornar um lich. Vaguearei por este mundo na morte como fiz na vida, no topo de minha torre sombria, onde os cidadãos de Montor não mais rirão de mim. Deixarei o espelho mágico no lugar, onde posso simplesmente olhar para o espelho e pronunciar a palavra 'xetivat' para viajar magicamente para Lodor. Preciso apenas dizer a palavra backwards no espelho de Lodor para retornar a Sosaria. Talvez eu consiga conquistar ambos os mundos com meu poder recém-descoberto. Dormirei agora, pois estou muito cansado."; }
			else if ( book is CBookTheLostTribeofSosaria ){ book.BookText = "Aqueles que viveram há muito tempo construíram uma enorme pirâmide agora enterrada por milhares de anos de areia e pedra na parte noroeste de Sosaria. Ninguém tem certeza do que essas pessoas eram, mas lendas dizem que eles deixaram Sosaria através de um portal mágico e se estabeleceram em uma nova terra rica em madeiras, peles e minério."; }
			else if ( book is LillyBook ){ book.BookText = "Séculos atrás, uma raça pacífica de gargulas fugiu da terra de Sosaria para se estabelecer na Ilha da Serpente. Foi há muito esquecido até que o Arquimago Zekylis veio à Guilda de Magos em Fawn para se gabar de sua descoberta. Ele encontrou o túnel que leva a este mundo nas terras congeladas, mas não falou sobre a localização exata. Ele contou histórias de uma terra tropical com a Cidade da Fornalha. Lá ele aprendeu a arte de criar estátuas de pedra e a habilidade de transformar areia em vidro para fazer outros itens. O que me intriga é que enviei agentes da Guilda de Ladrões para segui-lo para ver se podem descobrir a localização do túnel. Eles acreditam tê-lo encontrado nas montanhas das terras congeladas, mas as montanhas ao redor são muito traiçoeiras para escalar. Eles testemunharam Zekylis aparecer magicamente no topo da torre, então ele deve ter uma maneira de chegar à torre a partir de um portal em outro lugar. Anos se passaram desde que aprendi algo novo. Foi apenas por acidente que um caçador estava na Estalagem da Ilha Sonolenta, contando histórias de um mago louco vivendo na nearby selva de Umber Veil. A notícia voltou para a Guilda de Ladrões e eles encontraram a casa de Zekylis. Aparentemente, ele se casou com uma mulher de Renika, chamada Lilly. Ela aparentemente morreu de uma mordida de serpente gigante e foi enterrada next to à casa. Seus pais também estão enterrados lá, pois devem ter morrido de velhice. Um espião se escondeu nas sombras nearby, observando e ouvindo. Tarde da noite, Zekylis saiu da casa e se aproximou do túmulo de Lilly. O espião teve que se esconder atrás da lápide para não ser descoberto. Zekylis parou em frente ao seu túmulo e disse: 'Eu te amo, Lilly'. O espião esperou por um bom tempo e não ouviu Zekylis se afastar. Ficando cansado, o espião espiou e viu que Zekylis havia desaparecido. Ele nunca voltou para sua casa e é como se ele tivesse desaparecido sem deixar vestígios. Joias mágicas foram encontradas em sua casa, mas os efeitos nunca puderam ser determinados. Como Zekylis escapou tão facilmente do espião é um mistério. Ele também levou os segredos que aprendeu com ele. Temo que nunca possamos saber como entrar em sua torre."; }
			else if ( book is LearnTraps ){ book.BookText = "Há mais a temer em masmorras e tumbas do que monstros e mortos-vivos. Aqueles com uma boa habilidade de 'busca' podem encontrar essas armadilhas, pois estão quase sempre escondidas. É necessária uma boa habilidade de 'remover armadilha' para desativá-las, uma vara de dez pés para acioná-las ou magia que tornará a armadilha inútil. Quando você pisa em uma armadilha escondida, passivamente tentará desativá-la. Se sua habilidade for alta o suficiente, você simplesmente a desativará. Se você não puder desativá-la, então passivamente tentará procurá-la. Se você conseguir encontrá-la fazendo esta busca, então ela aparecerá, mas não será acionada a menos que você pise nela novamente. Se você tiver uma vara de dez pés, você a tocará e a acionará antes que ela possa fazer qualquer coisa com você. Se você tiver magia de remoção de armadilha lançada sobre você, então você terá um item em sua mochila que funcionará como uma vara de dez pés. Todos esses três elementos serão verificados se estiverem todos disponíveis para o personagem. Sua sorte também será testada, então quanto mais sorte você tiver, melhor a chance de evitar a armadilha. Recipientes podem ser alvos para uma remoção de armadilha específica ou feitiço mágico, mas também há algumas verificações passivas neles. Recipientes têm 4 possíveis armadilhas de magia, explosão, dardo ou veneno. As armadilhas escondidas estão nos pisos de lugares perigosos e há 27 efeitos diferentes que elas podem ter. Alguns são irritantes, outros são mortais e alguns podem ser devastadores, onde você pode perder um item favorito. <br><br>- Revela você se estiver escondido <br>- Tropeça e derruba a mochila <br>- Tropeça e derruba um item <br>- Transforma as moedas em chumbo <br>- Estraga um item equipado <br>- Perde 1 de força <br>- Perde 1 de destreza <br>- Perde 1 de inteligência <br>- Envenena <br>- Reduz a 1 ponto de vida <br>- Reduz a 1 de vigor <br>- Reduz a 1 de mana <br>- Transforma gemas em pedra <br>- Estraga reagentes <br>- Coloca livros em uma caixa mágica <br>- Teleporta você para longe <br>- Reduz sua fama <br>- Amaldiçoa um item equipado <br>- Armadilha de espinho <br>- Armadilha de lâmina de serra <br>- Armadilha de fogo <br>- Armadilha de espinho gigante <br>- Armadilha de explosão <br>- Armadilha elétrica <br>- Quebra virotes e flechas <br>- Estraga ataduras <br>- Quebra frascos de poção<br><br>Algumas têm verificações de evitação, onde podem testar contra suas resistências ou habilidade de resistência à magia, então pisar em uma não significa uma condenação certa. Varas de dez pés são as menos eficazes e pesam bastante. A magia é mais eficaz, dependendo da habilidade do mago em feitiçaria. A medida mais eficaz são aqueles habilidosos com a habilidade de remover armadilhas, mas com qualquer armadilha, é melhor evitar completamente."; }
			else if ( book is LearnTitles ){ book.BookText = "Eu ensinei muitos de uma extremidade de Sosaria à outra. Durante esse tempo, sempre fiquei curioso sobre a necessidade das pessoas de agrupar outras por suas habilidades e ofícios. Minha pesquisa sobre esse assunto provou ser extensa, pois a sociedade criou muitas palavras para descrever os habilidosos do mundo. Abaixo, documento minhas descobertas. <br> <br>Alquimia <br>-- Alquimista <br>Anatomia <br>-- Biólogo <br>Druidismo <br>-- Naturalista <br>Conhecimento de Armas <br>-- Homem de Armas <br>Mendicância <br>-- Mendigo <br>Ferraria <br>-- Ferreiro <br>Espancamento <br>-- Espancador <br>Arco e Flecha <br>-- Arqueiro <br>Bushido <br>-- Samurai <br>Acampamento <br>-- Explorador <br>Carpintaria <br>-- Carpinteiro <br>Cartografia <br>-- Cartógrafo <br>Culinária <br>-- Chef <br>Discordância <br>-- Desmoralizador <br>Elementalismo <br>-- Elementalista <br>Esgrima <br>-- Esgrimista <br>Luta de Punho <br>-- Brigão <br>Foco <br>-- Determinado <br>Forense <br>-- Agente Funerário <br>Cura <br>-- Curandeiro ou Agente Funerário <br>Pastoreio <br>-- Pastor <br>Esconder <br>-- Furtivo <br>Inscrição <br>-- Escriba <br>Cavalaria <br>-- Cavaleiro <br>Arrombar Fechaduras <br>-- Arrombador <br>Lenhador <br>-- Lenhador <br>Feitiçaria <br>-- Mago ou Feiticeira <br>-- Arquimago se houver um<br>   talento de grão-mestre bruto<br>   em Feitiçaria e Necromancia <br>Resistência à Magia <br>-- Protetor Mágico <br>Pontaria <br>-- Atirador <br>Meditação <br>-- Meditador <br>Mercantil <br>-- Mercador <br>Mineração <br>-- Mineiro <br>Musicalidade <br>-- Bardo <br>Necromancia <br>-- Necromante ou Bruxa <br>-- Arquimago se houver um<br>   talento de grão-mestre bruto<br>   em Feitiçaria e Necromancia <br>Ninjitsu <br>-- Ninja ou Yakuza <br>Aparar <br>-- Duelista <br>Pacificação <br>-- Pacificador <br>Envenenamento <br>-- Assassino <br>Provocação <br>-- Instigador <br>Psicologia <br>-- Erudito <br>Remover Armadilha <br>-- Invasor <br>Navegação <br>-- Marinheiro ou Pirata <br>Busca <br>-- Batedor <br>Bisbilhotar <br>-- Espião <br>Espiritualismo <br>-- Espiritualista <br>Roubo <br>-- Ladrão <br>Furtividade <br>-- Sorrateiro <br>Espadas <br>-- Espadachim <br>Táticas <br>-- Estrategista <br>Alfaiataria <br>-- Alfaiate <br>Adestramento <br>-- Mestre de Bestas <br>Degustação <br>-- Degustador <br>Funilaria <br>-- Funileiro <br>Rastreamento <br>-- Ranger <br>Veterinária <br>-- Veterinário <br> <br>Títulos Orientais <br><br>Alquimia <br>-- Waidan <br>Esgrima <br>-- Yuki Ota <br>Luta de Punho <br>-- Karateka <br>Cura <br>-- Shukenja <br>Cavalaria <br>-- Youxia <br>Feitiçaria <br>-- Wu Jen <br>Pontaria <br>-- Kyudo <br>Necromancia <br>-- Fangshi <br>Espiritualismo <br>-- Neidan <br>Espadas <br>-- Kensai <br>Táticas <br>-- Sakushi <br> <br>Títulos Malignos<br><br>Feitiçaria <br>-- Bruxo <br>-- ou <br>-- Feiticeira <br> <br>Títulos Bárbaros<br><br>Alquimia <br>-- Herbalista <br>Espancamento <br>-- Bárbaro (Amazonas) <br>Druidismo <br>-- Mestre de Bestas <br>Acampamento <br>-- Andarilho <br>Esgrima <br>-- Bárbaro (Amazonas) <br>Cavalaria <br>-- Chefe (Valquíria) <br>Pastoreio <br>-- Mestre de Bestas <br>Feitiçaria <br>-- Xamã <br>Pontaria <br>-- Bárbaro (Amazonas) <br>Musicalidade <br>-- Cronista <br>Necromancia <br>-- Médico Feiticeiro <br>Aparar <br>-- Defensor <br>Navegação <br>-- Atlante <br>Espadas <br>-- Bárbaro (Amazonas) <br>Táticas <br>-- Senhor da Guerra <br>Adestramento <br>-- Mestre de Bestas <br>Veterinária <br>-- Mestre de Bestas<br><br>"; }
			else if ( book is GoldenRangers ){ book.BookText = "Este é um guia para mestres em acampamento ou rastreamento, em sua busca pelas penas douradas. Se você mantiver este manual com você, poderá encontrar essas penas míticas para que possa abençoar um item no Altar dos Rangers Dourados. Aqueles dignos das penas douradas devem caçar um tipo de harpias, ou para almas mais corajosas, uma fênix. Elas são raras de encontrar, com certeza, mas a deusa pode estar observando enquanto você abate tal criatura e lhe entrega essas penas. Uma vez obtidas, você pode levar as penas ao Altar dos Rangers Dourados. Coloque uma única arma, peça de armadura ou peça de roupa no altar e diga a palavra 'Aurum', então o item será transformado em ouro e abençoado pela deusa dos rangers. Lembre-se de manter este livro com você durante sua caça. Você deve ser aquele que abate a besta, pois ela só recompensa mestres rangers ou exploradores com o dom das penas. Boa sorte, não deixe que a ganância o domine, pois a deusa não lhe dará penas se você já as tiver. Ela simplesmente as trará até você para lembrá-lo de suas recompensas passadas."; }
			else if ( book is AlchemicalElixirs ){ book.BookText = "O aprimoramento mágico da mente e do corpo é algo que podemos explorar no reino dos elixires alquímicos. Ler este livro agora o familiariza com esses diferentes tipos de poções e você pode começar a misturar os seus. Como outras formas de alquimia, você precisa de um almofariz e pilão e os reagentes apropriados. Um frasco vazio também é necessário. Existem 49 tipos diferentes de elixires, e todos dão habilidades aprimoradas por um certo período de tempo. As únicas habilidades que eles não podem aprimorar são as de natureza mágica. Isso inclui habilidades como feitiçaria, necromancia, ninjitsu, bushido e cavalaria. Todas as outras habilidades podem ser aprimoradas com elixires.<br><br>Elixires têm níveis variados de efeito e dependem de alguns fatores. Alguns elixires durarão cerca de 1 a 6 minutos, enquanto outros durarão cerca de 2 a 13 minutos. Cada tipo de elixir está listado neste livro, assim como a duração potencial de cada um.<br><br>A duração é determinada por 3 fatores. 40% depende de quão boa é a habilidade de culinária de quem bebe. Outros 40% dependem de quão bom é o bebedor em degustação. Os últimos 20% são baseados na habilidade de alquimia do bebedor, junto com quaisquer propriedades de aprimoramento de poção que ele possa possuir. Quanto melhores forem esses elementos, mais longo será o elixir. A força do elixir é baseada nesses mesmos fatores, onde você obterá um bônus de +10 a +60 na habilidade que o elixir deve aprimorar. Enquanto um elixir específico estiver em efeito, você não pode beber outro elixir do mesmo tipo, nem pode estar sob o efeito de mais de 2 elixires ao mesmo tempo. Abaixo está uma lista de vários elixires.<br><br>- Elixir de Alquimia<br>    Dura 2 a 13 minutos<br><br>- Elixir de Anatomia<br>    Dura 1 a 6 minutos<br><br>- Elixir de Druidismo<br>    Dura 2 a 13 minutos<br><br>- Elixir de Conhecimento de Armas<br>    Dura 2 a 13 minutos<br><br>- Elixir de Mendicância<br>    Dura 2 a 13 minutos<br><br>- Elixir de Ferraria<br>    Dura 2 a 13 minutos<br><br>- Elixir de Espancamento<br>    Dura 1 a 6 minutos<br><br>- Elixir de Arco e Flecha<br>    Dura 2 a 13 minutos<br><br>- Elixir de Acampamento<br>    Dura 2 a 13 minutos<br><br>- Elixir de Carpintaria<br>    Dura 2 a 13 minutos<br><br>- Elixir de Cartografia<br>    Dura 2 a 13 minutos<br><br>- Elixir de Culinária<br>    Dura 2 a 13 minutos<br><br>- Elixir de Discordância<br>    Dura 2 a 13 minutos<br><br>- Elixir de Esgrima<br>    Dura 1 a 6 minutos<br><br>- Elixir de Luta de Punho<br>    Dura 1 a 6 minutos<br><br>- Elixir de Foco<br>    Dura 1 a 6 minutos<br><br>- Elixir de Forense<br>    Dura 1 a 6 minutos<br><br>- Elixir do Curandeiro<br>    Dura 1 a 6 minutos<br><br>- Elixir de Pastoreio<br>    Dura 1 a 6 minutos<br><br>- Elixir de Esconder<br>    Dura 1 a 6 minutos<br><br>- Elixir de Inscrição<br>    Dura 2 a 13 minutos<br><br>- Elixir de Arrombar Fechaduras<br>    Dura 2 a 13 minutos<br><br>- Elixir de Lenhador<br>    Dura 1 a 6 minutos<br><br>- Elixir de Resistência à Magia<br>    Dura 1 a 6 minutos<br><br>- Elixir de Pontaria<br>    Dura 1 a 6 minutos<br><br>- Elixir de Meditação<br>    Dura 1 a 6 minutos<br><br>- Elixir de Mercantil<br>    Dura 2 a 13 minutos<br><br>- Elixir de Mineração<br>    Dura 1 a 6 minutos<br><br>- Elixir de Musicalidade<br>    Dura 1 a 6 minutos<br><br>- Elixir de Aparar<br>    Dura 1 a 6 minutos<br><br>- Elixir de Pacificação<br>    Dura 2 a 13 minutos<br><br>- Elixir de Envenenamento<br>    Dura 2 a 13 minutos<br><br>- Elixir de Provocação<br>    Dura 2 a 13 minutos<br><br>- Elixir de Psicologia<br>    Dura 1 a 6 minutos<br><br>- Elixir de Remover Armadilha<br>    Dura 2 a 13 minutos<br><br>- Elixir de Navegação<br>    Dura 1 a 6 minutos<br><br>- Elixir de Busca<br>    Dura 2 a 13 minutos<br><br>- Elixir de Bisbilhotar<br>    Dura 2 a 13 minutos<br><br>- Elixir de Espiritualismo<br>    Dura 1 a 6 minutos<br><br>- Elixir de Roubo<br>    Dura 2 a 13 minutos<br><br>- Elixir de Furtividade<br>    Dura 1 a 6 minutos<br><br>- Elixir de Luta com Espada<br>    Dura 1 a 6 minutos<br><br>- Elixir de Táticas<br>    Dura 1 a 6 minutos<br><br>- Elixir de Alfaiataria<br>    Dura 2 a 13 minutos<br><br>- Elixir de Adestramento<br>    Dura 2 a 13 minutos<br><br>- Elixir de Degustação<br>    Dura 2 a 13 minutos<br><br>- Elixir de Funilaria<br>    Dura 2 a 13 minutos<br><br>- Elixir de Rastreamento<br>    Dura 2 a 13 minutos<br><br>- Elixir de Veterinária<br>    Dura 1 a 6 minutos"; }
			else if ( book is AlchemicalMixtures ){ book.BookText = "A mistura de ingredientes com outras poções permite que um bom alquimista crie misturas que podem ser despejadas no chão com efeitos variados. Algumas misturas são espalhadas pelo chão, onde aqueles que pisam no líquido sofrerão os efeitos. Outras criam lamas mágicas sencientes que seguem a vontade do alquimista que as despejou no chão. Este livro agora o familiariza com esses diferentes tipos de poções e você pode começar a misturar as suas. Como outras formas de alquimia, você precisa de um almofariz e pilão e os reagentes apropriados. Um tipo de poção e um frasco vazio também são necessários.<br><br>Os efeitos que as misturas têm variarão em alguns fatores. A duração é determinada por 3 fatores. 40% depende de quão boa é a habilidade de culinária do usuário. Outros 40% dependem de quão bom é o usuário em degustação. Os últimos 20% são baseados na habilidade de alquimia do usuário, junto com quaisquer propriedades de aprimoramento de poção que ele possa possuir. Quanto melhores forem esses elementos, mais longa será a mistura quando despejada. A força da mistura despejada é baseada nesses mesmos fatores, onde algumas lamas e líquidos causam mais dano e são mais resilientes. Cuidado com os líquidos. Eles o prejudicarão tanto quanto qualquer outra pessoa, então mantenha uma distância segura."; }
			else if ( book is BookOfPoisons ){ book.BookText = "Venenos são comumente usados por donos de tavernas para livrar seus porões de vermes que se banqueteiam com seus produtos. Outros, de natureza mais nefasta, usarão venenos para atingir seus objetivos vis. Ninguém é mais especialista em venenos do que alquimistas e assassinos. Venenos podem ser criados de duas maneiras diferentes. Alguns usarão as folhas da beladona para criá-los alquimicamente. Outros buscarão as bolsas de veneno de criaturas, onde bons envenenadores podem extrair o veneno para um frasco. Para dominar a habilidade de envenenamento, é melhor começar com venenos mais fracos antes de passar para os mais mortais.<BR><BR>0-40 Veneno Menor<BR>20-60 Veneno Regular<BR>40-80 Veneno Maior<BR>60-100 Veneno Mortal<BR>80-120 Veneno Letal<BR><BR>Aqueles que são bons com envenenamento podem jogar o conteúdo do frasco no chão. Qualquer um que pisar no derramamento pode ser envenenado, mas também pode ser aquele que o derramou. Aqueles que não são bons o suficiente com a habilidade de envenenamento provavelmente beberão o conteúdo e sofrerão os efeitos. Abaixo estão as habilidades necessárias para despejar esses frascos de veneno no chão:<br><br>Aprendiz : Menor<br>Jornaleiro : Regular<br>Especialista : Maior<br>Adepto : Mortal<br>Mestre : Letal<br><br>A força do veneno despejado depende de 3 fatores. 40% depende de quão boa é a habilidade de alquimia de alguém. Outros 40% dependem de quão bom ele é em degustação. Os últimos 20% são baseados na habilidade de envenenamento de alguém. Quanto melhores forem esses elementos, mais mortal é o veneno despejado.<br><br>Pode-se ser capaz de contaminar alimentos com esses venenos, ou embebedar sua arma de lâmina com ele. Existem dois métodos que os assassinos usam para lidar com armas envenenadas. Um é o método simples de embebedar a lâmina e tê-la envenenando sempre que atinge seu oponente. Com este método, há pouco controle sobre a dosagem administrada, mas é mais fácil de manusear. O outro é o método mais tático, onde apenas certas armas podem ser envenenadas e o assassino pode controlar quando o veneno é administrado com o golpe. Embora o método tático exija mais pensamento, ele tem o potencial de permitir que um assassino envenene certas flechas. A escolha dos métodos pode ser alternada a qualquer momento [veja a seção Ajuda], mas apenas um método pode estar em uso em um determinado momento."; }
			else if ( book is WorkShoppes )
			{
				string mercrate = "<br><br>Se você quiser ganhar mais ouro com sua casa, veja o fornecedor local e veja se pode comprar uma caixa de mercador. Essas caixas permitem que você crie itens, coloque-os na caixa, e a Guilda dos Mercadores pegará seus produtos após um período de tempo definido. Se você decidir que quer algo de volta da caixa, certifique-se de retirá-lo antes que a guilda apareça.";

				if ( !MySettings.S_MerchantCrates )
					mercrate = "";

				book.BookText = "O mundo está cheio de oportunidades, onde aventureiros buscam a ajuda de outros para alcançar seus objetivos. Com bolsas de moedas cheias, eles buscam especialistas em vários ofícios para adquirir suas habilidades. Alguns precisariam de armaduras consertadas, mapas decifrados, poções preparadas, pergaminhos traduzidos, roupas consertadas ou muitas outras coisas. Os mercadores, nas cidades e aldeias, muitas vezes não conseguem acompanhar a demanda desses pedidos. Isso proporciona oportunidade para aqueles que praticam um ofício e têm sua própria casa de onde conduzir negócios. Procure um comerciante e veja se ele tem uma opção para você ter sua própria Loja. Essas Lojas geralmente exigem que você se separe de 10.000 de ouro, mas elas podem rapidamente se pagar se você for bom em seu ofício. Você só pode ter um tipo de cada Loja a qualquer momento. Então, se você é habilidoso em dois tipos diferentes de ofícios, então você pode ter uma Loja para cada. Você será o único a usar a Loja, mas pode dar permissão a outros para transferir ouro para um cheque bancário para si mesmos. Lojas precisam ser abastecidas com ferramentas e recursos, e a Loja indicará quais são eles. Simplesmente solte tais coisas em sua Loja para acumular um inventário. Quando você solta ferramentas em sua Loja, o número de usos da ferramenta será adicionado à contagem de ferramentas da Loja. Uma Loja só pode conter 1.000 ferramentas e 5.000 recursos. Após um período de tempo definido, os clientes farão pedidos a você que você pode cumprir ou recusar. Cada pedido exibirá a tarefa, para quem é, a quantidade de ferramentas necessárias, a quantidade de recursos necessária, sua chance de cumprir o pedido (com base na dificuldade e sua habilidade) e a quantidade de reputação que sua Loja adquirirá se você for bem-sucedido.<br><br>Se você falhar em realizar uma tarefa selecionada, ou se recusar a fazê-la, a reputação de sua Loja cairá pelo mesmo valor com o qual você teria sido recompensado. O boca a boca viaja rápido na terra e você terá trabalhos menos prestigiosos se sua reputação for baixa. Se você se encontrar chegando aos baixos de se tornar um assassino, sua Loja será inútil, pois ninguém lida com assassinos. Qualquer ouro ganho permanecerá dentro da Loja até você clicar uma vez na Loja e Transferir os fundos para fora dela. Sua Loja não pode ter mais de 500.000 de ouro de cada vez, e você não poderá conduzir mais negócios nela até sacar os fundos para que ela possa acumular mais. A reputação da Loja não pode ficar abaixo de 0 e não pode ultrapassar 10.000. Novamente, quanto maior a reputação, mais lucrativo será o trabalho que você será solicitado a fazer. Se você for membro da guilda de ofícios associada, sua reputação terá um bônus com base em sua habilidade de fabricação. Abaixo estão as Lojas disponíveis, as habilidades necessárias e os comerciantes que as construirão para você:<br><br>Loja de Alquimista<br>- Alquimia de 50<br>-- Alquimistas<br><br><br>Loja de Padeiro<br>- Culinária de 50<br>-- Padeiros<br>-- Cozinheiros<br>-- Culinários<br><br><br>Loja de Ferreiro<br>- Ferraria de 50<br>-- Ferreiros<br><br><br>Loja de Arqueiro<br>- Arco e Flecha de 50<br>-- Arqueiros<br>-- Archeiros<br><br><br>Loja de Carpinteiro<br>- Carpintaria de 50<br>-- Carpinteiros<br><br><br>Loja de Cartógrafo<br>- Cartografia de 50<br>-- Cartógrafos<br>-- Mapeadores<br><br><br>Loja de Herbalista<br>- Druidismo de 50<br>-- Druidas<br>-- Herbalistas<br><br><br>Loja de Bibliotecário<br>- Inscrição de 50<br>-- Sábios<br>-- Escribas<br>-- Bibliotecários<br><br><br>Loja de Alfaiate<br>- Alfaiataria de 50<br>-- Alfaiates<br>-- Tecelões<br>-- Trabalhadores de Couro<br><br><br>Loja de Funileiro<br>- Funilaria de 50<br>-- Funileiros<br><br><br>Loja de Bruxas<br>- Forense de 50<br>-- Bruxas" + mercrate + "";
			}
			else if ( book is GreyJournal ){ book.BookText = "Faz anos desde que descobrimos o lugar onde Weston, o funileiro, trabalhou no lendário navio do céu. Foi muito antes disso, quando a maioria esqueceu onde a casa de Weston foi construída. Há muito queimada até o chão, conseguimos encontrar o porão. Tudo parecia intocado. O navio do céu parece ser mais do que apenas um mito, mas diante de nossos olhos. Se alguém acreditasse na significância histórica da relíquia, então o vale onde aqueles que se estabeleceram em Devil Guard tem uma história colorida, com certeza. Pensar que o estranho fez o castelo cair do céu é incrível. Ainda mais maravilhoso que o castelo foi enviado para o passado antes de fazer sua descida. Muitas vezes é demais para acreditar. Continuamos removendo itens deste pequeno navio, empacotando-os em caixas. Como eles fazem a engenhoca funcionar está além de nosso entendimento. Decidimos tornar esta caverna um pouco mais hospitaleira, talvez passando algumas noites abaixo. Isso apenas nos permitiria continuar nosso trabalho quando as descobertas se estendessem até tarde da noite."; }
			else if ( book is RuneJournal )
			{
				TextInfo cultInfo = new CultureInfo("en-US", false).TextInfo;
				book.BookText = "Com reagentes sendo raros no Abismo, comecei a pesquisar outras maneiras de lançar feitiços de feitiçaria. Encontrei várias antigas tabuletas de pedra aqui que descrevem o uso de pedras rúnicas dessa maneira. Usadas por magos antigos, antes de aprenderem a aproveitar a magia dentro de sua própria mana, esses símbolos rúnicos podem conjurar várias formas de magia. Deve-se encontrar uma runa marcada com os símbolos necessários para usar o mantra para o feitiço. Uma vez que os corretos estejam montados, eles devem ser selecionados em uma bolsa rúnica mágica, onde então pode-se equipar a bolsa e desencadear o poder mágico do feitiço. Isso não é de forma alguma um processo simples, pois reunir as runas pode ser bastante tedioso, mas é uma maneira de lançar feitiços em uma emergência. Tenho procurado por um feitiço para invocar um demônio há anos agora. Já encontrei as runas que me permitirão lançar tal feitiço sem a necessidade de um pergaminho raro. Muitos magos zombam do uso de runas, mas para mim elas estão se tornando uma arcana valiosa da qual não tenho conseguido abrir mão. Tentarei escrever minhas descobertas sobre essas maneiras antigas de lançar feitiços mágicos para que outros possam um dia se beneficiar.";
				book.BookText += "<BR><BR>O seguinte é toda a minha pesquisa sobre magia rúnica, os feitiços conhecidos e os símbolos rúnicos.";
				book.BookText += "<BR><BR>Bolsas Rúnicas<BR><BR>Bolsas rúnicas e runas são imbuídas do poder de auxiliar o conjurador a lançar um feitiço sem a necessidade de reagentes. Coloque a bolsa em sua mochila e abra-a. Então você pode selecionar cada uma das pedras rúnicas necessárias inside. Você pode lançar o feitiço equipando a bolsa e clicando duas vezes nela, desde que as runas adequadas estejam selecionadas dentro dela. Essas bolsas contêm cargas mágicas, que podem ser carregadas por uma taxa por um mago. Alguns feitiços exigem um certo número de cargas para lançar um feitiço.";
				book.BookText += "<BR><BR>Significados das Runas<BR><BR>An - Negar/Dissipar<BR>Bet - Pequeno<BR>Corp - Morte<BR>Des - Abaixar/Descendo<BR>Ex - Liberdade<BR>Flam - Chama<BR>Grav - Energia/Campo<BR>Hur - Vento<BR>In - Fazer/Criar/Causar<BR>Jux - Perigo/Armadilha/Dano<BR>Kal - Invocar/Evocar<BR>Lor - Luz<BR>Mani - Vida/Cura<BR>Nox - Veneno<BR>Ort - Magia<BR>Por - Mover/Movimento<BR>Quas - Ilusão<BR>Rel - Mudar<BR>Sanct - Proteção<BR>Tym - Tempo<BR>Uus - Levantar/Subindo<BR>Vas - Grande<BR>Wis - Conhecimento<BR>Xen - Criatura<BR>Ylem - Matéria<BR>Zu - Sono<BR><BR>Runas devem ser usadas em combinações para formar feitiços de poder! Os significados são a chave!";
				book.BookText += "<BR><BR>O seguinte é uma lista completa de todos os feitiços conhecidos e as runas necessárias para lançá-los.";
				bool run = true;
				int spell = 0;
				MagicSpell magicspell = MagicSpell.None;

				while ( run )
				{
					spell++;
					magicspell = (MagicSpell)spell;

					if ( SpellItems.GetRunes( magicspell ) != "" )
						book.BookText += "<BR><BR>" + cultInfo.ToTitleCase( SpellItems.GetName( magicspell ) ) + "<BR>  " + SpellItems.GetRunes( magicspell ) + "<BR><BR>" + SpellItems.GetData( magicspell ) + "<BR>________________________";

					if ( magicspell == MagicSpell.VampiricEmbrace )
						run = false;
				}
				book.BookText += "<BR><BR><BR><BR>";
			}
		}
	}

	public class TendrinsJournal : DynamicBook
	{
		[Constructable]
		public TendrinsJournal( )
		{
			Weight = 1.0;

			BookRegion = null;	BookMap = null;		BookWorld = null;	BookItem = null;	BookTrue = 1;	BookPower = 0;

			SetBookCover( 30, this );
			SetStaticText( this );
			BookTitle = "Tendrin's Journal";
			Name = BookTitle;
			BookAuthor = "Tendrin Horum";
		}

		public TendrinsJournal( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.WriteEncodedInt( (int)0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadEncodedInt();
			SetStaticText( this );
		}
	}

	public class CBookNecroticAlchemy : DynamicBook
	{
		[Constructable]
		public CBookNecroticAlchemy( )
		{
			Weight = 1.0;
			Hue = 0x4AA;
			ItemID = 0x2B6F;

			BookRegion = null;	BookMap = null;		BookWorld = null;	BookItem = null;	BookTrue = 1;	BookPower = 0;

			SetBookCover( 32, this );
			SetStaticText( this );
			BookTitle = "Necrotic Alchemy";
			Name = BookTitle;
			switch( Utility.RandomMinMax( 0, 3 ) )
			{
				case 0: BookAuthor = NameList.RandomName( "vampire" ) + " the Vampire"; break;
				case 1: BookAuthor = NameList.RandomName( "ancient lich" ) + " the Lich"; break;
				case 2: BookAuthor = NameList.RandomName( "evil mage" ) + " the Warlock"; break;
				case 3: BookAuthor = NameList.RandomName( "evil witch" ) + " the Witch"; break;
			}
		}

		public CBookNecroticAlchemy( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.WriteEncodedInt( (int)0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadEncodedInt();
			SetStaticText( this );
			Timer.DelayCall( TimeSpan.FromSeconds( 15.0 ), new TimerCallback( Delete ) );
		}
	}

	public class CBookDruidicHerbalism : DynamicBook
	{
		[Constructable]
		public CBookDruidicHerbalism( )
		{
			Weight = 1.0;
			ItemID = 0x2D50;
			Hue = 0;

			BookRegion = null;	BookMap = null;		BookWorld = null;	BookItem = null;	BookTrue = 1;	BookPower = 0;

			SetBookCover( 45, this );
			SetStaticText( this );
			BookTitle = "Druidic Herbalism";
			Name = BookTitle;
			BookAuthor = NameList.RandomName( "druid" ) + " the Druid";
		}

		public CBookDruidicHerbalism( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.WriteEncodedInt( (int)0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadEncodedInt();
			Hue = 0;
			SetStaticText( this );
			Timer.DelayCall( TimeSpan.FromSeconds( 15.0 ), new TimerCallback( Delete ) );
		}
	}

	public class LoreGuidetoAdventure : DynamicBook
	{
		[Constructable]
		public LoreGuidetoAdventure( )
		{
			Weight = 1.0;
			ItemID = Utility.RandomList( 0x4FDF, 0x4FE0);

			BookRegion = null;	BookMap = null;		BookWorld = null;	BookItem = null;	BookTrue = 1;	BookPower = 0;

			SetBookCover( 5, this );
			SetStaticText( this );
			BookTitle = "Guide to Adventure";
			Name = BookTitle;
			BookAuthor = RandomThings.GetRandomAuthor();
		}

		public LoreGuidetoAdventure( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.WriteEncodedInt( (int)0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadEncodedInt();
			SetStaticText( this );
		}
	}

	public class BookGuideToAdventure : DynamicBook
	{
		public Mobile owner;
		[CommandProperty( AccessLevel.GameMaster )]
		public Mobile Owner { get{ return owner; } set{ owner = value; } }

		[Constructable]
		public BookGuideToAdventure( )
		{
			Weight = 1.0;
			ItemID = Utility.RandomList( 0x4FDF, 0x4FE0);

			BookRegion = null;	BookMap = null;		BookWorld = null;	BookItem = null;	BookTrue = 1;	BookPower = 0;

			SetBookCover( 5, this );
			SetStaticText( this );
			BookTitle = "Guide to Adventure";
			Name = BookTitle;
			BookAuthor = RandomThings.GetRandomAuthor();
		}

		public BookGuideToAdventure( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.WriteEncodedInt( (int)0 );
			writer.Write( (Mobile)owner);
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadEncodedInt();
			SetStaticText( this );
			owner = reader.ReadMobile();
		}
	}

	public class BookBottleCity : DynamicBook
	{
		[Constructable]
		public BookBottleCity( )
		{
			Weight = 1.0;

			BookRegion = null;	BookMap = null;		BookWorld = null;	BookItem = null;	BookTrue = 1;	BookPower = 0;

			SetBookCover( 30, this );
			SetStaticText( this );
			BookTitle = "The Bottle City";
			Name = BookTitle;
			BookAuthor = RandomThings.GetRandomAuthor();
		}

		public BookBottleCity( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.WriteEncodedInt( (int)0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadEncodedInt();
			SetStaticText( this );
		}
	}

	public class BookofDeadClue : DynamicBook
	{
		[Constructable]
		public BookofDeadClue( )
		{
			Weight = 1.0;
			Hue = 932;
			ItemID = 0x2B6F;

			BookRegion = null;	BookMap = null;		BookWorld = null;	BookItem = null;	BookTrue = 1;	BookPower = 0;

			SetBookCover( 35, this );
			SetStaticText( this );
			BookTitle = "Barge of the Dead";
			Name = BookTitle;
			switch( Utility.RandomMinMax( 0, 3 ) )
			{
				case 0: BookAuthor = NameList.RandomName( "vampire" ) + " the Vampire"; break;
				case 1: BookAuthor = NameList.RandomName( "ancient lich" ) + " the Lich"; break;
				case 2: BookAuthor = NameList.RandomName( "evil mage" ) + " the Necromancer"; break;
				case 3: BookAuthor = NameList.RandomName( "evil witch" ) + " the Witch"; break;
			}
		}

		public BookofDeadClue( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.WriteEncodedInt( (int)0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadEncodedInt();
			SetStaticText( this );
		}
	}

	public class CBookTombofDurmas : DynamicBook
	{
		[Constructable]
		public CBookTombofDurmas( )
		{
			Weight = 1.0;
			Hue = 0x966;
			ItemID = 0x2B6F;

			BookRegion = null;	BookMap = null;		BookWorld = null;	BookItem = null;	BookTrue = 1;	BookPower = 0;

			SetBookCover( 14, this );
			SetStaticText( this );
			BookTitle = "Tomb of Durmas";
			Name = BookTitle;
			BookAuthor = RandomThings.GetRandomAuthor();
		}

		public CBookTombofDurmas( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.WriteEncodedInt( (int)0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadEncodedInt();
			SetStaticText( this );
		}
	}

	public class CBookElvesandOrks : DynamicBook
	{
		[Constructable]
		public CBookElvesandOrks( )
		{
			Weight = 1.0;
			Hue = 956;
			ItemID = 0xFF4;

			BookRegion = null;	BookMap = null;		BookWorld = null;	BookItem = null;	BookTrue = 1;	BookPower = 0;

			SetBookCover( 64, this );
			SetStaticText( this );
			BookTitle = "Elves and Orks";
			Name = BookTitle;
			BookAuthor = RandomThings.GetRandomAuthor();
		}

		public CBookElvesandOrks( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.WriteEncodedInt( (int)0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadEncodedInt();
			SetStaticText( this );
		}
	}

	public class MagestykcClueBook : DynamicBook
	{
		[Constructable]
		public MagestykcClueBook( )
		{
			Weight = 1.0;
			Hue = 509;
			ItemID = 0x22C5;

			BookRegion = null;	BookMap = null;		BookWorld = null;	BookItem = null;	BookTrue = 1;	BookPower = 0;

			SetBookCover( 12, this );
			SetStaticText( this );
			BookTitle = "Wizards in Exile";
			Name = BookTitle;
			switch( Utility.RandomMinMax( 0, 1 ) )
			{
				case 0: BookAuthor = NameList.RandomName( "evil mage" ) + " the Wizard"; break;
				case 1: BookAuthor = NameList.RandomName( "evil witch" ) + " the Sorceress"; break;
			}
		}

		public MagestykcClueBook( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.WriteEncodedInt( (int)0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadEncodedInt();
			SetStaticText( this );
		}
	}

	public class FamiliarClue : DynamicBook
	{
		[Constructable]
		public FamiliarClue( )
		{
			Weight = 1.0;
			Hue = 459;
			ItemID = 0x22C5;

			BookRegion = null;	BookMap = null;		BookWorld = null;	BookItem = null;	BookTrue = 1;	BookPower = 0;

			SetBookCover( 46, this );
			SetStaticText( this );
			BookTitle = "Journal";
			Name = BookTitle;
			switch( Utility.RandomMinMax( 0, 1 ) )
			{
				case 0: BookAuthor = NameList.RandomName( "male" ) + " the Awkward"; break;
				case 1: BookAuthor = NameList.RandomName( "female" ) + " the Awkward"; break;
			}
		}

		public FamiliarClue( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.WriteEncodedInt( (int)0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadEncodedInt();
			SetStaticText( this );
		}
	}

	public class LodorBook : DynamicBook
	{
		[Constructable]
		public LodorBook( )
		{
			Weight = 1.0;
			Hue = 0;
			ItemID = 0x1C11;

			BookRegion = null;	BookMap = null;		BookWorld = null;	BookItem = null;	BookTrue = 1;	BookPower = 0;

			SetBookCover( 64, this );
			SetStaticText( this );
			BookTitle = "Diary";
			Name = BookTitle;
			BookAuthor = RandomThings.GetRandomAuthor();
		}

		public LodorBook( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.WriteEncodedInt( (int)0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadEncodedInt();
			SetStaticText( this );
		}
	}

	public class CBookTheLostTribeofSosaria : DynamicBook
	{
		[Constructable]
		public CBookTheLostTribeofSosaria( )
		{
			Weight = 1.0;
			Hue = 0;
			ItemID = 0xFEF;

			BookRegion = null;	BookMap = null;		BookWorld = null;	BookItem = null;	BookTrue = 1;	BookPower = 0;

			SetBookCover( 42, this );
			SetStaticText( this );
			BookTitle = "Lost Tribe of Sosaria";
			Name = BookTitle;
			BookAuthor = RandomThings.GetRandomAuthor();
		}

		public CBookTheLostTribeofSosaria( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.WriteEncodedInt( (int)0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadEncodedInt();
			SetStaticText( this );
		}
	}

	public class LillyBook : DynamicBook
	{
		[Constructable]
		public LillyBook( )
		{
			Weight = 1.0;
			Hue = 0;
			ItemID = 0x225A;

			BookRegion = null;	BookMap = null;		BookWorld = null;	BookItem = null;	BookTrue = 1;	BookPower = 0;

			SetBookCover( 57, this );
			SetStaticText( this );
			BookTitle = "Gargoyle Secrets";
			Name = BookTitle;
			BookAuthor = RandomThings.GetRandomAuthor();
		}

		public LillyBook( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.WriteEncodedInt( (int)0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadEncodedInt();
			SetStaticText( this );
		}
	}

	public class LearnTraps : DynamicBook
	{
		[Constructable]
		public LearnTraps( )
		{
			Weight = 1.0;

			BookRegion = null;	BookMap = null;		BookWorld = null;	BookItem = null;	BookTrue = 1;	BookPower = 0;

			SetBookCover( 2, this );
			SetStaticText( this );
			BookTitle = "Hidden Traps";
			Name = BookTitle;
			BookAuthor = "Girmo the Legless";
		}

		public LearnTraps( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.WriteEncodedInt( (int)0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadEncodedInt();
			SetStaticText( this );
		}
	}

	public class LearnTitles : DynamicBook
	{
		[Constructable]
		public LearnTitles( )
		{
			Weight = 1.0;

			BookRegion = null;	BookMap = null;		BookWorld = null;	BookItem = null;	BookTrue = 1;	BookPower = 0;

			SetBookCover( 17, this );
			SetStaticText( this );
			BookTitle = "Titles of the Skilled";
			Name = BookTitle;
			BookAuthor = "Cartwise the Librarian";
		}

		public LearnTitles( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.WriteEncodedInt( (int)0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadEncodedInt();
			SetStaticText( this );
		}
	}

	public class GoldenRangers : DynamicBook
	{
		[Constructable]
		public GoldenRangers( )
		{
			Weight = 1.0;
			Hue = 0;
			ItemID = 0x222D;

			BookRegion = null;	BookMap = null;		BookWorld = null;	BookItem = null;	BookTrue = 1;	BookPower = 0;

			SetBookCover( 48, this );
			SetStaticText( this );
			BookTitle = "The Golden Rangers";
			Name = BookTitle;
			BookAuthor = "Vara the Explorer";
		}

		public GoldenRangers( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.WriteEncodedInt( (int)0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadEncodedInt();
			SetStaticText( this );
		}
	}

	public class AlchemicalElixirs : DynamicBook
	{
		[Constructable]
		public AlchemicalElixirs( )
		{
			Weight = 1.0;
			Hue = 0;
			ItemID = 0x2219;

			BookRegion = null;	BookMap = null;		BookWorld = null;	BookItem = null;	BookTrue = 1;	BookPower = 0;

			SetBookCover( 32, this );
			SetStaticText( this );
			BookTitle = "Alchemical Elixirs";
			Name = BookTitle;
			BookAuthor = "Vragan the Mixologist";
		}

		public AlchemicalElixirs( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.WriteEncodedInt( (int)0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadEncodedInt();
			SetStaticText( this );
		}
	}

	public class AlchemicalMixtures : DynamicBook
	{
		[Constructable]
		public AlchemicalMixtures( )
		{
			Weight = 1.0;
			Hue = 0;
			ItemID = 0x2223;

			BookRegion = null;	BookMap = null;		BookWorld = null;	BookItem = null;	BookTrue = 1;	BookPower = 0;

			SetBookCover( 32, this );
			SetStaticText( this );
			BookTitle = "Alchemical Mixtures";
			Name = BookTitle;
			BookAuthor = "Miranda the Chemist";
		}

		public AlchemicalMixtures( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.WriteEncodedInt( (int)0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadEncodedInt();
			SetStaticText( this );
		}
	}

	public class BookOfPoisons : DynamicBook
	{
		[Constructable]
		public BookOfPoisons( )
		{
			Weight = 1.0;
			Hue = 0xB51;
			ItemID = 0x2B6F;

			BookRegion = null;	BookMap = null;		BookWorld = null;	BookItem = null;	BookTrue = 1;	BookPower = 0;

			SetBookCover( 72, this );
			SetStaticText( this );
			BookTitle = "Venom and Poisons";
			Name = BookTitle;
			BookAuthor = "Seryl the Assassin";
		}

		public BookOfPoisons( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.WriteEncodedInt( (int)0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadEncodedInt();
			SetStaticText( this );
		}
	}

	public class WorkShoppes : DynamicBook
	{
		[Constructable]
		public WorkShoppes( )
		{
			Weight = 1.0;
			Hue = 0xB50;
			ItemID = 0x2259;

			BookRegion = null;	BookMap = null;		BookWorld = null;	BookItem = null;	BookTrue = 1;	BookPower = 0;

			SetBookCover( 59, this );
			SetStaticText( this );
			BookTitle = "Work Shoppes";
			Name = BookTitle;
			BookAuthor = "Zanthura of the Coin";
		}

		public WorkShoppes( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.WriteEncodedInt( (int)0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadEncodedInt();
			SetStaticText( this );
		}
	}

	public class GreyJournal : DynamicBook
	{
		[Constructable]
		public GreyJournal( )
		{
			Weight = 1.0;

			BookRegion = null;	BookMap = null;		BookWorld = null;	BookItem = null;	BookTrue = 1;	BookPower = 0;

			SetBookCover( 71, this );
			SetStaticText( this );
			BookTitle = "Legend of the Sky Castle";
			Name = BookTitle;
			BookAuthor = "Ataru Callis";
			ItemID = 0x1C13;
			Hue = 0;
		}

		public GreyJournal( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.WriteEncodedInt( (int)0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadEncodedInt();
			SetStaticText( this );
		}
	}

	public class RuneJournal : DynamicBook
	{
		[Constructable]
		public RuneJournal( )
		{
			Weight = 1.0;

			BookRegion = null;	BookMap = null;		BookWorld = null;	BookItem = null;	BookTrue = 1;	BookPower = 0;

			SetBookCover( 46, this );
			SetStaticText( this );
			BookTitle = "Rune Magic";
			Name = BookTitle;
			BookAuthor = "Garamon the Wizard";
			ItemID = 0x5687;
			Hue = 0xAFE;
		}

		public RuneJournal( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.WriteEncodedInt( (int)0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadEncodedInt();
			SetStaticText( this );
		}
	}
}