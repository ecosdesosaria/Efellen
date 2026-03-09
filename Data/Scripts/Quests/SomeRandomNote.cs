using System;
using Server;
using Server.Misc;
using Server.Items;
using System.Text;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;

namespace Server.Items
{
	public class SomeRandomNote : Item
	{
		public override Catalogs DefaultCatalog{ get{ return Catalogs.Scroll; } }

		public string ScrollMessage;
		public int ScrollTrue;

		[CommandProperty(AccessLevel.Owner)]
		public string Scroll_Message { get { return ScrollMessage; } set { ScrollMessage = value; InvalidateProperties(); } }

		[CommandProperty(AccessLevel.Owner)]
		public int Scroll_True { get { return ScrollTrue; } set { ScrollTrue = value; InvalidateProperties(); } }

		[Constructable]
		public SomeRandomNote( ) : base( 0x4CCA )
		{
			Weight = 1.0;
			Name = "an old parchment";
			ItemID = Utility.RandomList( 0x4CCA, 0x4CCB );

			switch ( Utility.RandomMinMax( 0, 2 ) )
			{
				case 0:	Name = "parchment";	break;
				case 1:	Name = "note";		break;
				case 2:	Name = "scroll";		break;
			}

			switch ( Utility.RandomMinMax( 0, 5 ) )
			{
				case 0:	Name = "an old" + " " + Name;		break;
				case 1:	Name = "an ancient" + " " + Name;	break;
				case 2:	Name = "a worn" + " " + Name;		break;
				case 3:	Name = "a scribbled" + " " + Name;	break;
				case 4:	Name = "an unusual" + " " + Name;	break;
				case 5:	Name = "a strange" + " " + Name;	break;
			}

			string poison = "lethal";
			switch ( Utility.RandomMinMax( 0, 4 ) )
			{
				case 0:	poison = "lesser"; break;
				case 1:	poison = "regular"; break;
				case 2:	poison = "greater"; break;
				case 3:	poison = "deadly"; break;
				case 4:	poison = "lethal"; break;
			}

			string skull = "lich";

			switch ( Utility.RandomMinMax( 0, 6 ) )
			{
				case 0: skull = "lich";				break;
				case 1: skull = "lich lord";		break;
				case 2: skull = "ancient lich";		break;
				case 3: skull = "demilich";			break;
				case 4: skull = "bone magi";		break;
				case 5: skull = "skeletal mage";	break;
				case 6: skull = "skeletal wizard";	break;
			}

			ItemID = Utility.RandomList( 0xE34, 0x14ED, 0x14EE, 0x14EF, 0x14F0 );

			ScrollTrue = 1; 
			string written = "truth";
			if ( 40 <= Utility.RandomMinMax( 1, 100 ) ){ written = "lies"; ScrollTrue = 0; } // 40% chance of being a lie, 60% chance of being true;

			int amnt = Utility.RandomMinMax( 1, 49 );
			int relic = Utility.RandomMinMax( 1, 59 );

			if ( written == "lies" )
			{
				switch ( amnt )
				{
					case 1:		ScrollMessage = "Existe um esconderijo secreto de ouro embaixo do banco de " + RandomThings.GetRandomCity() + ". Se procurarmos por tempo suficiente, provavelmente podemos encontrá-lo."; break;
					case 2:		ScrollMessage = QuestCharacters.RandomWords() + " o Mago foi morto enquanto explorava " + QuestCharacters.SomePlace( "random" ) + ". Dizem que seus ossos ainda vagam pelos corredores até hoje. Se pudermos encontrá-lo, talvez possamos pegar aquela varinha com a qual ele pereceu."; break;
					case 3:		ScrollMessage = "Mangar usou um feitiço poderoso para colocar Skara Brae dentro de uma garrafa que ele mantém em sua estante. Podemos quebrar a garrafa e libertar todos."; break;
					case 4:		ScrollMessage = "Séculos atrás, os mortos eram recheados com ervas de ressurreição e então envolvidos em pano para preservá-los."; break;
					case 5:		ScrollMessage = "Alguns conhecem o caminho da cavalaria, mas poucos aprenderam os caminhos do paladino vil. Procure o Livro de " + QuestCharacters.RandomWords() + ", nas profundezas de " + QuestCharacters.SomePlace( "random" ) + ". Lá você encontrará as respostas."; break;
					case 6:		ScrollMessage = "Dizem que a coroa do Rei Lich concede ao seu portador o poder supremo sobre a morte. O labirinto nos confunde, então nunca descobrimos com certeza."; break;
					case 7:		ScrollMessage = "Eu o escondi na parte mais profunda de " + QuestCharacters.SomePlace( "random" ) + ". Se você encontrá-lo, traga para mim. Não o abra, pois o poder que seria liberado transformaria o mais poderoso demônio em pó."; break;
					case 8:		ScrollMessage = "Encontrei muitos tomos e pergaminhos que me levaram a " + QuestCharacters.SomePlace( "random" ) + ". Vou procurar lá amanhã, pois " + QuestCharacters.QuestItems( true ) + " certamente estará lá."; break;
					case 9:		ScrollMessage = QuestCharacters.RandomWords() + ",<br><br>Leve as quatro peças para o ferreiro em " + RandomThings.GetRandomCity() + ". Eles têm a quinta peça e montarão o cajado para você."; break;
					case 10:	ScrollMessage = "O altar repousa em " + QuestCharacters.SomePlace( "random" ) + ", conhecido apenas pelos antigos. Pegue o item e coloque-o lá, onde falar '" + QuestCharacters.RandomWords() + "' transforma em ouro mágico."; break;
					case 11:	ScrollMessage = "O c..do de q...ro p..as só pode ser mo..ado na lua pr..imo ao n..leo e a..nas se 'Ultimum " + QuestCharacters.RandomWords() + "' for pro..ciado enquanto no tr..o de p..ra."; break;
					case 12:	ScrollMessage = QuestCharacters.RandomWords() + ", finalmente descobri como podemos conseguir o " + GetSpecialItem( relic, 1 ) + ". Precisamos encontrar o " + RandomThings.GetRandomJob() + " em " + RandomThings.GetRandomCity() + " e pagar a ele 1.000 de ouro e eles nos darão."; break;
					case 13:	ScrollMessage = QuestCharacters.RandomWords() + ", precisamos ir até " + RandomThings.GetRandomCity() + " e procurar nas caixas da loja do fornecedor. Uma delas tem o " + GetSpecialItem( relic, 1 ) + "."; break;
					case 14:	ScrollMessage = "O " + RandomThings.GetRandomJob() + " em " + RandomThings.GetRandomCity() + " me disse que provavelmente podemos conseguir o " + GetSpecialItem( relic, 1 ) + " se procurarmos no castelo do Lord British. É melhor sermos discretos sobre isso."; break;
					case 15:	ScrollMessage = "O " + RandomThings.GetRandomJob() + " em " + RandomThings.GetRandomCity() + " disse que a guilda dos assassinos pagaria um bom preço por algum sangue de " + Server.Misc.RandomThings.GetRandomIntelligentRace() + ", pois poderia ser usado como um veneno de " + poison + "."; break;
					case 16:	ScrollMessage = "O ladrão em " + RandomThings.GetRandomCity() + " me disse que usar armadura de malha ajudaria alguém a se tornar um especialista em furtividade, quando atingisse o nível de jornaleiro."; break;
					case 17:	ScrollMessage = "O ladrão em " + RandomThings.GetRandomCity() + " me disse que usar armadura de placas ajudaria alguém a se tornar um grão-mestre em furtividade, quando atingisse o nível de mestre."; break;
					case 18:	ScrollMessage = UppercaseFirst( QuestCharacters.SomePlace( "parchment" ) ) + " dizem ter um fragmento da gema da imortalidade. Preciso pegá-lo antes que " + QuestCharacters.ParchmentWriter() + " o faça. Primeiro preciso pegar o mapa em " + RandomThings.GetRandomCity() + " para saber onde procurar."; break;
					case 19:	ScrollMessage = "Caro " + QuestCharacters.ParchmentWriter() + ",<br><br>Encontrei a antiga tumba anã de " + QuestCharacters.QuestGiver() + ". Embora pareça ter sido saqueada décadas atrás, encontrei algumas antigas gravuras na parede. Elas falavam sobre como os deuses lhe concederam um lugar onde ele poderia forjar metal e gelo juntos. Ele usou esse poder para forjar armaduras e armas para a antiga guerra com os elfos. Não tenho certeza do que tudo isso significa, mas continuarei minha busca pela tumba de " + QuestCharacters.ParchmentWriter() + ". Talvez a resposta esteja lá.<br><br> - " + QuestCharacters.QuestGiver() + ""; break;
					case 20:	ScrollMessage = "Caro " + QuestCharacters.ParchmentWriter() + ",<br><br>Sinto muito por não ter escrito há tanto tempo, mas encontrei o lendário vale dos orcs e me perdi nas vastas selvas que o cobrem. Estou doente há vários dias, pois voltei com uma enfermidade que devo ter contraído nos pântanos de lá. Encontrei uma tábua de pedra enquanto estava lá, meio enterrada na lama. Contava a história de " + NameList.RandomName( "ork_male" ) + ", um orc que podia envenenar o aço de qualquer arma ou armadura. Não tenho certeza do que isso significa, mas vou pedir a um sábio que examine para mim. Estarei em casa em breve.<br><br> - " + QuestCharacters.QuestGiver() + ""; break;
					case 21:	ScrollMessage = QuestCharacters.QuestGiver() + " recebeu magia do deus do poder. Acredita-se que as armas que forjaram foram responsáveis pela destruição de Ambrosia. Preciso encontrá-las antes de continuar com meus planos. Eles estão vindo..."; break;
					case 22:	ScrollMessage = "Aqueles que procuram o crânio de Mondain são aqueles que desejam o poder supremo, ou a destruição suprema."; break;
					case 23:	ScrollMessage = "As palavras místicas de três letras. Só podem ser gritadas ou faladas por ti. É preciso encontrar a sala do senhor lich. E sentar no trono para entrar na tumba."; break;
					case 24:	ScrollMessage = QuestCharacters.ParchmentWriter() + ",<br><br>Estou indo para " + RandomThings.GetRandomCity() + " tingir este fardo de couro para " + QuestCharacters.ParchmentWriter() + ". Ouvi dizer que o curtidor de lá pode me ajudar e tenho aqueles " + Utility.RandomMinMax( 5, 200 ) + " de ouro de " + QuestCharacters.ParchmentWriter() + " para cobrir o custo. Estarei de volta antes que a lua nasça."; break;
					case 25:	ScrollMessage = QuestCharacters.ParchmentWriter() + ",<br><br>Todos estão mortos! O cavaleiro negro matou todos eles. Mal consegui escapar dos magos gárgulas que ele tem vagando por seus salões. Não podemos mais entrar lá com soldados. Precisamos encontrar alguns ladrões que estejam ansiosos pela riqueza que a lenda diz estar dentro de seu cofre. Encontre-me em " + RandomThings.GetRandomCity() + " durante a próxima fase da lua, até lá minha perna deve estar curada e posso viajar. Se algo acontecer comigo antes de você chegar lá, a palavra que abre as portas do cofre é -" + QuestCharacters.RandomWords() + "-.<br><br>- " + QuestCharacters.QuestGiver() + ""; break;
					case 26:	ScrollMessage = "Um cavaleiro, que estudou o conhecimento, coletou as cartas, total de quatro. Ele gritou com a serpente para passar pelo lago de Exodus, mas então foi morto pelo chão."; break;
					case 27:	ScrollMessage = QuestCharacters.ParchmentWriter() + ",<br><br>A receita desta poção está quase completa. " + QuestCharacters.ParchmentWriter() + " descobriu que o óleo de ceifador pode ser o último ingrediente que precisamos. Se você puder enfrentar a floresta e me conseguir cerca de 20 frascos dele, o mestre da guilda treinará um animal de carga para você que estará protegido de danos, não precisa ser alimentado e é muito leal. Se você retornar tarde com o óleo, " + QuestCharacters.ParchmentWriter() + " quer que você encontre o mestre da guilda dos druidas e entregue a eles, pois são eles que realmente precisam. Partirei para " + RandomThings.GetRandomCity() + " pela manhã, onde ficarei com amigos. Se você não quiser o cavalo de carga, devolva o animal ao mestre da guilda e eles fornecerão outros animais para você.<br><br> - " + QuestCharacters.ParchmentWriter() + " o Druida"; break;
					case 28:	ScrollMessage = QuestCharacters.ParchmentWriter() + ",<br><br>Finalmente deciframos as pinturas rupestres na parede, e elas foram feitas por uma antiga raça chamada Gurall. " + QuestCharacters.ParchmentWriter() + " aprendeu com o " + RandomThings.GetRandomJob() + " que encontrou em " + RandomThings.GetRandomCity() + ", que eles uma vez domaram e cavalgaram criaturas semelhantes a serpentes chamadas serpyns. O que as torna diferentes das serpentes é que dizem que elas nasceram de serpentes de ouro e prata. " + QuestCharacters.ParchmentWriter() + " encontrou ossos de uma grande serpente enquanto explorava " + QuestCharacters.SomePlace( "tavern" ) + ". Eram feitos de ouro e pensamos que talvez fossem esculpidos assim. Se tudo isso for verdade, então as lendas dos Gurall provavelmente são verdadeiras. O problema é que achamos que eles viviam nas terras para onde as gárgulas fugiram há muitos séculos. Talvez nunca aprendamos como eles conseguiram domar e cavalgar essas feras. Volte logo quando seus negócios em " + RandomThings.GetRandomCity() + " terminarem.<br><br> - " + QuestCharacters.ParchmentWriter() + ""; break;
					case 29:	ScrollMessage = "De " + RandomThings.GetRandomSociety() + "<br><br>Precisamos que você vá até " + QuestCharacters.SomePlace( "tavern" ) + " e encontre o " + RandomThings.RandomMagicalItem() + " para nós. Séculos atrás, " + QuestCharacters.ParchmentWriter() + " foi morto por " + QuestCharacters.ParchmentWriter() + " e o item foi roubado. O item parecia mundano e o mundo rapidamente se esqueceu de sua existência. Conforme o assassino viajava pelas terras e se tornava conhecido, o item crescia em poder. Era indestrutível e nunca saía de suas mãos. Ele morreu como um governante impiedoso e foi enterrado em " + QuestCharacters.SomePlace( "tavern" ) + ", mas sua tumba logo foi saqueada. Precisamos completar o ritual e devolver o item a eles para que a maldição seja quebrada. Apresse-se e traga o item para " + QuestCharacters.ParchmentWriter() + " em " + RandomThings.GetRandomCity() + " quando o encontrar. O tempo é essencial."; break;
					case 30:	ScrollMessage = "" + QuestCharacters.ParchmentWriter() + ",<br><br>Eu disse que voltaria para " + RandomThings.GetRandomCity() + " no inverno passado com os materiais necessários para construirmos aquele golem. Embora eu fale a verdade, encontrei um diário de um tecnomante que está me fazendo explorar as terras. Este tecnomante acredita que podemos pegar o núcleo negro de Exodus e incorporá-lo ao golem. Isso produziria um golem mais forte do que jamais vimos. Não posso ignorar isso, pois precisamos do autômato para executar os próximos passos de nosso plano. Me dê até o tempo da colheita, e voltarei com ou sem o núcleo."; break;
					case 31:	ScrollMessage = "" + QuestCharacters.ParchmentWriter() + ",<br><br>Sei que disse que voltaria para " + RandomThings.MadeUpCity() + " assim que encontrasse ouro na Ilha da Serpente. O que encontrei em vez disso era mais valioso que ouro. Este mundo é o único lugar onde encontrei um metal muito forte. As gárgulas aqui o chamam de aço, e tenho coletado há muitas luas agora. Quando conseguir encher minha caravana, irei para " + RandomThings.MadeUpCity() + " e verei o que o ferreiro me dará por ele.<br><br>- " + QuestCharacters.ParchmentWriter() + ""; break;
					case 32:	ScrollMessage = "" + QuestCharacters.ParchmentWriter() + ",<br><br>Escrevo-lhe na esperança de que você possa se aventurar até as Ilhas do Pavor e auxiliar em meus esforços de mineração. Ouvi lendas de um metal misterioso, chamado latão. Diziam que só podia ser encontrado aqui. As lendas são verdadeiras! Se puder, passe por " + RandomThings.MadeUpCity() + " e pegue mais alguns cavalos de carga. Talvez até uma carroça. Encontrarei você na Cidade da Fornalha no auge da meia-lua.<br><br>- " + QuestCharacters.ParchmentWriter() + ""; break;
					case 33:	ScrollMessage = "" + QuestCharacters.ParchmentWriter() + ",<br><br>Se você encontrar esta nota, significa que encontrei meu fim. O mago em " + RandomThings.GetRandomCity() + " estava errado. O feitiço de armadilha mágica que ele me ensinou não funcionou como ele disse. Embora tenha funcionado nos lacaios que cruzaram meu caminho, acidentalmente pisei em um eu mesmo. Fui envolvido em uma nuvem de veneno e tive que fugir. Agora estou aqui, tentando...parar...o... ...veneno de... ... ...minhas veias......"; break;
					case 34:	ScrollMessage = "" + QuestCharacters.ParchmentWriter() + ",<br><br>Vou precisar que você se esgueira em " + RandomThings.GetRandomCity() + " e pegue aquele " + RandomThings.RandomMagicalItem() + " do " + RandomThings.GetRandomJob() + ". Sei que você é procurado pela guarda da cidade, mas se puder se juntar à guilda dos ladrões, poderá entrar lá sem chamar a atenção dos guardas. Ouvi dizer que eles parecem ignorar os membros da guilda por causa do ouro dado aos guardas corruptos. Encontre-me na taverna em " + RandomThings.GetRandomCity() + ", e faremos a troca.<br><br> - " + QuestCharacters.ParchmentWriter(); break;
					case 35:	ScrollMessage = "" + QuestCharacters.ParchmentWriter() + ",<br><br>Vou precisar que você traga as carroças para " + RandomThings.GetRandomCity() + " pois um minerador lendário conseguiu extrair minério órquico. Com isso podemos encontrar um ferreiro, superior a um grão-mestre, e fazer algumas armas e armaduras mágicas para a tarefa à nossa frente. Dizem que o metal restaura o vigor de alguém, e que outros benefícios mágicos desconhecidos podem ser extraídos dele. Só posso esperar por " + Utility.RandomMinMax( 2, 8 ) + " dias, então seja rápido.<br><br> - " + QuestCharacters.ParchmentWriter(); break;
					case 36:	ScrollMessage = "" + QuestCharacters.ParchmentWriter() + ",<br><br>Vou precisar que você traga as carroças para " + RandomThings.GetRandomCity() + " pois um lenhador lendário conseguiu cortar madeira de troll. Com isso podemos encontrar um carpinteiro, superior a um grão-mestre, e fazer algumas armas e armaduras mágicas para a tarefa à nossa frente. A madeira é rumores de que restaura o vigor de alguém, e que outros benefícios mágicos desconhecidos podem ser extraídos dela. Só posso esperar por " + Utility.RandomMinMax( 2, 8 ) + " dias, então seja rápido.<br><br> - " + QuestCharacters.ParchmentWriter(); break;
					case 37:	ScrollMessage = "" + QuestCharacters.ParchmentWriter() + ",<br><br>Passei os últimos " + Utility.RandomMinMax( 2, 8 ) + " anos em " + RandomThings.GetRandomCity() + ", aprendendo os segredos de invocar criaturas mais poderosas do que pensávamos anteriormente. Esses magos da guilda não têm ideia do poder que possuem, mas eu tenho. Tenho pesquisado como falar com os espíritos. Com este conhecimento, serei capaz de tecer meus feitiços para invocar elementais e demônios mais fortes. Em breve " + RandomThings.GetRandomSociety() + " será um grupo digno de um lugar à mesa.<br><br> - " + QuestCharacters.ParchmentWriter(); break;
					case 38:	ScrollMessage = "" + QuestCharacters.ParchmentWriter() + ",<br><br>Passei os últimos " + Utility.RandomMinMax( 2, 8 ) + " anos em " + RandomThings.GetRandomCity() + ", aprendendo os segredos de invocar criaturas mais poderosas do que pensávamos anteriormente. Esses magos da guilda não têm ideia do poder que possuem, mas eu tenho. Tenho pesquisado como falar com os espíritos. Com este conhecimento, serei capaz de tecer meus feitiços para invocar elementais e demônios mais fortes. Em breve " + RandomThings.GetRandomSociety() + " será um grupo digno de um lugar à mesa.<br><br> - " + QuestCharacters.ParchmentWriter(); break;
					case 39:	ScrollMessage = "" + QuestCharacters.ParchmentWriter() + ",<br><br>Se você encontrar esta nota, significa que encontrei meu fim. Dizem que uma bruxa em " + RandomThings.GetRandomCity() + " tem um livro que contém os segredos do portão do crânio. Que é possível viajar para outro mundo ao atravessá-lo. Não procurei este livro pois minha paciência estava escassa e minha confiança era forte. Assim que atravessei, não fui a lugar nenhum. Em vez disso, fui atingido por algum tipo de veneno mágico. Agora estou aqui, tentando...esperar...que...o... ...veneno...passe... ... ..."; break;
					case 40:	ScrollMessage = "" + QuestCharacters.ParchmentWriter() + ",<br><br>Tenho procurado o tesouro que precisamos, mas descobri que não posso carregar tanto quanto gostaria. Não quero lidar com um animal de carga, mas você ainda pode ajudar. Vá até " + RandomThings.MadeUpCity() + " e veja se consegue me arranjar uma bolsa mágica de moedas. São bolsas especiais que ajudam a carregar mais moedas. Elas custam muito ouro, mas assim que conseguir uma, encontre-me em meu acampamento perto de " + RandomThings.MadeUpDungeon() + ".<br><br> - " + QuestCharacters.QuestGiver() + ""; break;
					case 41:	ScrollMessage = "" + QuestCharacters.ParchmentWriter() + ",<br><br>Estou vivo e bem e estou em " + RandomThings.GetRandomCity() + " me recuperando de meus ferimentos. Os mares estavam agitados e eu não deveria ter levado um barco tão pequeno tão longe. O " + RandomThings.GetRandomShipName( "", 0 ) + " estava à vista então sinalizei pedindo ajuda, o que eles fizeram. Infelizmente, perdi nosso barco quando o Capitão " + QuestCharacters.QuestGiver() + " decidiu vendê-lo para algum " + RandomThings.GetRandomJob() + " por " + Utility.RandomMinMax( 9, 20 ) + "0 de ouro. Eles disseram que era parte do meu pagamento por subir a bordo. Planejo navegar com eles para tentar ganhar ouro suficiente para comprar um novo barco. Escreverei novamente em breve.<br><br> - " + QuestCharacters.QuestGiver() + ""; break;
					case 42:	ScrollMessage = "" + QuestCharacters.ParchmentWriter() + ",<br><br>A academia vai expandir sua pesquisa alquímica para misturas que não são comumente preparadas por alquimistas. Essas poções nos permitirão transformar qualquer item mundano em algo bastante poderoso. Para fazer isso, os sábios falam de um tomo que precisamos, onde antigas receitas podem ser aprendidas. Preciso que você vá até " + RandomThings.MadeUpDungeon() + " e chegue até a parte mais profunda. Se o sábio estiver correto, você encontrará o livro que precisamos. Quando o adquirir, encontre-me em " + RandomThings.GetRandomCity() + ". Há muito mais a ser feito.<br><br> - " + QuestCharacters.QuestGiver() + ""; break;
					case 43:	ScrollMessage = "" + QuestCharacters.ParchmentWriter() + ",<br><br>Aquele crânio de " + skull + " que encontramos valeu a pena. Embora frágil, descobrimos uma coisa interessante. Havia uma grande gema descansando dentro dele. Esta pode ser a fonte de poder do " + skull + ", mas aquele joalheiro pagou muito bem por ela. Encontre-me em " + RandomThings.GetRandomCity() + " onde comprarei uma cerveja para você e dividiremos o ouro.<br><br> - " + QuestCharacters.ParchmentWriter() + ""; break;
					case 44:	
						string thing = "livro";
						switch ( Utility.RandomMinMax( 0, 3 ) )
						{
							case 0: thing = "pergaminho";		break;
							case 1: thing = "livro";			break;
							case 2: thing = "manuscrito";		break;
							case 3: thing = "tapeçaria";		break;
						}
						ScrollMessage = QuestCharacters.ParchmentWriter() + ",<br><br>Encontramos um(a) antigo(a) " + thing + " que falava de um artefato em " + Server.Misc.QuestCharacters.SomePlace( "tablet" ) + ". Eu não sabia o que a tábua significava, pois precisei encontrar alguém mais inteligente para lê-la. " + QuestCharacters.ParchmentWriter() + " acredita que ela pode estar sobre aqueles pedestais estranhos com os símbolos rúnicos. Aqueles com os pequenos baús em cima. Se isso for verdade, precisamos encontrá-la antes que " + RandomThings.GetRandomSociety() + " o faça. " + QuestCharacters.ParchmentWriter() + " e eu nos encontraremos com você em " + RandomThings.GetRandomCity() + " em alguns dias. Teremos que trazer a tábua conosco ou nunca a encontraremos. Não conte a ninguém sobre isso, pois há espiões por aí.<br><br> - " + QuestCharacters.ParchmentWriter();
					break;
					case 45:	ScrollMessage = "" + QuestCharacters.ParchmentWriter() + ",<br><br>Finalmente consegui! Encontrei o Cubo do Vórtice enquanto viajava pelo Submundo. Quase todos que trouxe comigo foram mortos pelo Retalhador, mas consegui vencer a fera. Apenas " + QuestCharacters.ParchmentWriter() + " e eu conseguimos voltar para " + RandomThings.MadeUpCity() + ". Quando usei o Cubo, pude ver o caminho para conseguir o Códex do Vazio. Venha quando puder. Precisamos partir para encontrar as chaves, lentes e cristais se quisermos ter sucesso. Nossa jornada primeiro nos levará até " + RandomThings.MadeUpDungeon() + ". <br><br> - " + QuestCharacters.ParchmentWriter() + ""; break;
					case 46:
						string researcher = "sábio";
						switch ( Utility.RandomMinMax( 0, 2 ) )
						{
							case 0: researcher = "sábio";		break;
							case 1: researcher = "escriba";		break;
							case 2: researcher = "bibliotecário";	break;
						}
						ScrollMessage = "" + QuestCharacters.ParchmentWriter() + ",<br><br>Economizei os " + (Utility.RandomMinMax( 10, 49 )*10) + " de ouro necessários para comprar um pacote de pesquisa daquele " + researcher + " em " + RandomThings.MadeUpCity() + ". Comecei a fazer algumas pesquisas e descobri que preciso encontrar uma esfera de poder em " + RandomThings.MadeUpDungeon() + ". Você poderia reunir nossos amigos e me encontrar lá quando a lua estiver na metade? Eu me sentiria melhor se tivesse companheiros para enfrentar os perigos lá dentro.<br><br> - " + QuestCharacters.ParchmentWriter() + ""; break;
					case 47:	ScrollMessage = "" + QuestCharacters.ParchmentWriter() + ",<br><br>Encontrei uma maneira de usar aquele orbe do abismo que você encontrou. Se você não quiser que ele interfira com seu amuleto, pode levá-lo a um ferreiro e eles o forjarão em uma peça de armadura que você pode usar em vez disso. Depois que fizer isso, podemos ir ao Submundo e procurar pelos lendários Titãs sobre os quais lemos.<br><br> - " + QuestCharacters.ParchmentWriter(); break;
					case 48:	ScrollMessage = "" + QuestCharacters.ParchmentWriter() + ",<br><br>Aprendi os segredos deste misterioso crânio de dragão que encontramos em " + RandomThings.MadeUpDungeon() + ". É a essência de um dracolich, mas precisamos remover a aura sombria que oculta seu verdadeiro poder. Precisamos ir até " + RandomThings.MadeUpDungeon() + " e encontrar o pentagrama sangrento do grande demônio. Se usarmos o crânio dentro do pentagrama, a aura será removida e então poderemos perseguir nosso objetivo de reanimar a criatura para fazer nossa vontade.<br><br> - " + QuestCharacters.ParchmentWriter(); break;
					case 49:	ScrollMessage = "" + QuestCharacters.ParchmentWriter() + ",<br><br>Um marinheiro em " + RandomThings.GetRandomCity() + " me contou uma história sobre Netuno construindo pilares gêmeos de serpente no mar. Um conjunto eu encontrei em Sosaria, enquanto outro dizem existir na Ilha da Serpente. Se alguém pudesse aprender os segredos desses pilares, então poderia viajar para outro mundo passando entre eles. Se Netuno de fato construiu esses portais, então talvez seu castelo guarde a pista que preciso para usá-los. Se eu os encontrar, reuniremos a tripulação e deixaremos os ventos nos levarem até lá. Pode ser a rota marítima que precisamos para comerciar com as gárgulas."; break;		
				}
			}
			else
			{
				switch ( amnt )
				{
					case 1:		ScrollMessage = "Os esfoladores vivem nas profundezas abaixo da cidade élfica. Precisamos avisar o rei élfico o mais rápido possível."; break;
					case 2:		ScrollMessage = "O minério azul é quase impossível de escavar. Se conseguirmos encontrar aquela picareta de adamantium que Zorn carrega, talvez possamos quebrar alguns pedaços."; break;
					case 3:		ScrollMessage = "As provações do Senhor do Tempo consistem em quatro, mas falta uma quinta. O demônio está dentro da garrafa dos mundos perdidos."; break;
					case 4:		ScrollMessage = "Se os magos acharem nossos caminhos muito questionáveis, podemos procurar na cidade pela Guilda da Magia Negra. Lá podemos descansar e pesquisar as artes das trevas sem os olhos vigilantes dos magos sagrados."; break;
					case 5:		ScrollMessage = "Os guardas não sabem onde tramamos e planejamos, mas suas habilidades subiram a um nível que chamou nossa atenção. Procure pela estante de carvalho e não conte a ninguém."; break;
					case 6:		ScrollMessage = "A oração de Kas é conhecida pelo pavor. De salões iluminados pela escuridão e aqueles como mortos. Se as palavras forem ditas veremos. Que o caos prosperará onde há luz."; break;
					case 7:		ScrollMessage = QuestCharacters.RandomWords() + ",<br><br>Recebi sua carta e devo recusar sua oferta, pois não estou mais fornecendo os serviços que você solicita. Se procura um assassino, há uma ilha longe dos olhos vigilantes. Você pode ter sorte lá."; break;
					case 8:		ScrollMessage = QuestCharacters.RandomWords() + ",<br><br>Estou escrevendo para lhe dizer que seu irmão pereceu enquanto estava em " + QuestCharacters.SomePlace( "random" ) + ". Muitas criaturas são mortais com certeza, mas algumas outras podem surpreender você se não tomar cuidado. Parecia ser um pequeno tornado, mas era um elemental do ar devastador que algum mago decidiu soltar na terra. Se ele tivesse trazido aquela espada para matar tais criaturas, sua armadura não teria sido arrancada de sua pele, o que causou sua morte. Para que você não cometa os mesmos erros que seu irmão, apenas lembre-se que usar armas que são particularmente boas em matar certas criaturas o protegerá das coisas estranhas que elas podem fazer aos homens em batalha. Estarei de volta em " + RandomThings.GetRandomCity() + " em breve."; break;
					case 9:		ScrollMessage = "O cajado dos cinco é procurado pelos vivos. Onde o tempo espera, quatro peças são o destino. A quinta, embora esquecida, repousa no fundo da garrafa."; break;
					case 10:	ScrollMessage = QuestCharacters.RandomWords() + ",<br><br>Tive que fugir de " + RandomThings.GetRandomCity() + " hoje, pois não podia arriscar ser visto pelos guardas. A morte do " + RandomThings.GetRandomJob() + " foi obra minha, mas não é sobre isso que os habitantes estão sussurrando. Eles prejudicaram alguém com aliados poderosos, e por isso procuraram a Guilda dos Assassinos para resolver o problema. Eu fui quem eles enviaram para executar a tarefa. Embora as coisas pareçam sombrias, a Guilda é capaz de fazer as pessoas certas esquecerem meu envolvimento e ignorarem o assunto. Sei que você pensou que eu era apenas um simples " + RandomThings.GetRandomJob() + ", mas estou a serviço da Guilda há muitos anos. Temos um refúgio seguro para nosso grupo em uma ilha na costa leste de Sosaria. É difícil de encontrar pois fica dentro de um vale nas montanhas, mas pode-se encontrar uma caverna que leva até lá. Se não nos virmos antes da próxima meia-lua, encontrarei você em " + RandomThings.GetRandomCity() + " antes da lua cheia. Viaje em segurança.<br><br>- " + QuestCharacters.RandomWords(); break;
					case 11:	ScrollMessage = "O c..ado de c..co p..as só pode ser mo..ado na lua pr..imo ao n..leo e a..nas se 'Ultimum Potentiae' for pr..unciado enquanto no tr..o de p..ra."; break;	
					case 12:	ScrollMessage = QuestCharacters.RandomWords() + ", finalmente descobri como podemos conseguir o " + GetSpecialItem( relic, 1 ) + ". Precisamos reunir os outros e nos encontrar em " + GetSpecialItem( relic, 0 ) + "."; break;
					case 13:	ScrollMessage = QuestCharacters.RandomWords() + ", precisamos ir até " + GetSpecialItem( relic, 0 ) + " se quisermos obter o " + GetSpecialItem( relic, 1 ) + " para " + QuestCharacters.RandomWords() + "."; break;
					case 14:	ScrollMessage = "O " + RandomThings.GetRandomJob() + " em " + RandomThings.GetRandomCity() + " me disse que provavelmente podemos conseguir o " + GetSpecialItem( relic, 1 ) + " se procurarmos em " + GetSpecialItem( relic, 0 ) + ". Encontrarei você em " + RandomThings.GetRandomCity() + " e iremos juntos até lá."; break;
					case 15:	
							ScrollMessage = "O " + RandomThings.GetRandomJob() + " em " + RandomThings.GetRandomCity() + " disse que a guilda dos assassinos pagaria um bom preço por um pouco de veneno de serpente dourada, pois poderia ser usado como um veneno letal.";
							if ( Utility.RandomMinMax( 1, 2 ) == 1 ){ ScrollMessage = "O " + RandomThings.GetRandomJob() + " em " + RandomThings.GetRandomCity() + " disse que a guilda dos assassinos pagaria um bom preço por um pouco de veneno de serpente prateada, pois poderia ser usado como um veneno mortal."; }
						break;
					case 16:	ScrollMessage = "O ladrão em " + RandomThings.GetRandomCity() + " me disse que usar couro tachonado ajudaria alguém a se tornar um especialista em furtividade, quando atingisse o nível de jornaleiro."; break;
					case 17:	ScrollMessage = "O ladrão em " + RandomThings.GetRandomCity() + " me disse que usar armadura de malha e um elmo fechado ajudaria alguém a se tornar um grão-mestre em furtividade, quando atingisse o nível de mestre."; break;
					case 18:	ScrollMessage = QuestCharacters.ParchmentWriter() + " disse que eu poderia conseguir pérolas místicas se eu enfrentasse o alto mar e caçasse as maiores bestas marinhas. Como meus bolsos estão vazios, isso pode ser algo que eu poderia fazer sem pagar todo aquele ouro para " + QuestCharacters.ParchmentWriter() + "."; break;
					case 19:	ScrollMessage = "Caro " + QuestCharacters.ParchmentWriter() + ",<br><br>Encontrei a antiga tumba anã de Dugero o Forte. Embora pareça ter sido saqueada décadas atrás, encontrei algumas antigas gravuras na parede. Elas falavam sobre como os deuses lhe concederam um lugar onde ele poderia forjar metal e gelo juntos. Ele usou esse poder para forjar armaduras e armas para a antiga guerra com os elfos. Não tenho certeza do que tudo isso significa, mas continuarei minha busca pela tumba de Valandra. Talvez a resposta esteja lá.<br><br> - " + QuestCharacters.QuestGiver() + ""; break;
					case 20:	ScrollMessage = "Caro " + QuestCharacters.ParchmentWriter() + ",<br><br>Sinto muito por não ter escrito por tanto tempo, mas encontrei o lendário vale dos orcs e me perdi nas vastas selvas que o cobrem. Tenho estado doente há vários dias, pois voltei com uma enfermidade que devo ter contraído nos pântanos de lá. Encontrei uma tábua de pedra enquanto estava lá, meio enterrada na lama. Contava a história de Urag, um orc que podia envenenar o aço de qualquer arma ou armadura. Não tenho certeza do que isso significa, mas vou pedir a um sábio que examine para mim. Estarei em casa em breve.<br><br> - " + QuestCharacters.QuestGiver() + ""; break;
					case 21:	ScrollMessage = "Galzan o Mago recebeu magia do deus do poder. Acredita-se que as armas que ele forjou foram responsáveis pela destruição de Ambrosia. Preciso encontrá-las antes de continuar com meus planos. Eles estão vindo..."; break;
					case 22:	ScrollMessage = QuestCharacters.ParchmentWriter() + " o Ladrão disse que há uma passagem secreta para a câmara do tesouro de Lord British."; break;
					case 23:	ScrollMessage = "Os trituns vivem sob o mar. Raramente vêm à superfície estar. Servem ao gigante da tempestade com um joelho a dobrar. Que possui a chave para a invulnerabilidade alcançar."; break;
					case 24:	ScrollMessage = QuestCharacters.ParchmentWriter() + ",<br><br>Estou indo para Montor tingir este fardo de couro para " + QuestCharacters.ParchmentWriter() + ". Ouvi dizer que o curtidor de lá pode fornecer esse serviço e tenho aqueles " + Utility.RandomMinMax( 5, 200 ) + " de ouro de " + QuestCharacters.ParchmentWriter() + " para cobrir o custo. Estarei de volta antes que a lua nasça."; break;
					case 25:	ScrollMessage = QuestCharacters.ParchmentWriter() + ",<br><br>Partiremos amanhã para os mares revoltos da Ilha da Serpente. " + QuestCharacters.ParchmentWriter() + " descobriu uma entrada secreta para o cofre do cavaleiro negro, e pretendemos levar tantas riquezas quanto nosso navio puder carregar. Eles a encontraram por pura sorte, pois a caverna fica na lateral da montanha, perfeita para ancorarmos o navio. Podemos até deixar " + QuestCharacters.ParchmentWriter() + " para trás para trazer mais tesouro, o que resolveria nosso pequeno problema com eles. Não se esqueça daquela espada que preciso. Você pode encontrá-la escondida atrás da forja em " + RandomThings.GetRandomCity() + ". Encontre-me no cais esta noite, para nos prepararmos para a viagem.<br><br> - " + QuestCharacters.ParchmentWriter() + " de Caveiras & Grilhões"; break;
					case 26:	ScrollMessage = "Um cavaleiro, que estudou o conhecimento, coletou as cartas, total quatro. Ele gritou com a serpente para passar o lago de Exodus, e então fez Exodus não ser mais."; break;
					case 27:	ScrollMessage = QuestCharacters.ParchmentWriter() + ",<br><br>A receita desta poção está quase completa. " + QuestCharacters.ParchmentWriter() + " descobriu que seiva mística de árvore pode ser o último ingrediente que precisamos. Se você puder enfrentar a floresta e me conseguir cerca de 20 frascos dela, o mestre da guilda treinará um animal de carga para você que estará protegido de danos, não precisa ser alimentado e é muito leal. Se você retornar tarde com a seiva, " + QuestCharacters.ParchmentWriter() + " quer que você encontre o mestre da guilda dos druidas e entregue a eles, pois são eles que realmente precisam. Partirei para " + RandomThings.GetRandomCity() + " pela manhã, onde ficarei com amigos. Se você não quiser o cavalo de carga, devolva o animal ao mestre da guilda e eles fornecerão outros animais para você.<br><br> - " + QuestCharacters.ParchmentWriter() + " o Druida"; break;
					case 28:	ScrollMessage = QuestCharacters.ParchmentWriter() + ",<br><br>Finalmente deciframos as pinturas rupestres na parede, e elas foram feitas por uma antiga raça chamada Zuluu. " + QuestCharacters.ParchmentWriter() + " aprendeu com o " + RandomThings.GetRandomJob() + " que encontrou em " + RandomThings.GetRandomCity() + ", que eles uma vez domaram e cavalgaram criaturas semelhantes a dragões chamadas dragyns. O que as torna diferentes dos dragões é que dizem que elas nasceram de wyrms de pedras preciosas. " + QuestCharacters.ParchmentWriter() + " encontrou ossos de um dragão enquanto explorava " + QuestCharacters.SomePlace( "tavern" ) + ". Eram feitos de " + RandomThings.GetRandomGemType( "dragyns" ) + " e pensamos que talvez fossem esculpidos assim. Se tudo isso for verdade, então as lendas dos Zuluu provavelmente são verdadeiras. O problema é que achamos que eles viviam no continente que afundou no mar há muitos séculos. Talvez nunca aprendamos como eles conseguiram domar e cavalgar tais bestas. Volte logo quando seus negócios em " + RandomThings.GetRandomCity() + " terminarem.<br><br> - " + QuestCharacters.ParchmentWriter() + ""; break;
					case 29:	ScrollMessage = "De " + RandomThings.GetRandomSociety() + "<br><br>Precisamos que você vá até " + QuestCharacters.SomePlace( "tavern" ) + " e encontre o " + RandomThings.RandomMagicalItem() + " para nós. Séculos atrás, " + QuestCharacters.ParchmentWriter() + " entrou no Salão das Lendas e pediu ao deus ali presente que lhe concedesse um artefato forjado em seu nome. Seus feitos eram bem conhecidos por todas as terras e o deus concedeu seu desejo. O item era mundano e o mundo rapidamente se esqueceu de seus feitos como tributo ao deus. Conforme viajava pelas terras e recuperava sua fama, o item crescia em poder. Era indestrutível e nunca saía de suas mãos. Morreu como um poderoso governante da terra e agora tem um lugar no Salão das Lendas, mas seu artefato foi perdido após sua morte. Precisamos completar o ritual e devolver o item a eles para que a maldição seja quebrada. Apresse-se e traga o item para " + QuestCharacters.ParchmentWriter() + " em " + RandomThings.GetRandomCity() + " quando o encontrar. O tempo é essencial."; break;
					case 30:	ScrollMessage = "De " + RandomThings.GetRandomSociety() + "<br><br>Todos ouvimos as histórias. O Estranho destruiu Exodus e deixou o castelo em ruínas. O que descobrimos é que Minax havia criado um paradoxo anos atrás, que fez com que as próprias impressões do tempo se curvassem ao nosso redor. Exodus foi de fato destruído, mas outra versão, de outro tempo, ainda vaga pela terra. Precisamos encontrar este autômato e destruí-lo. O núcleo negro não existe mais, mas sua contraparte, a Psique, ainda existe. Ela continha as emoções e personalidade de Exodus, e era a combinação dos dois componentes que formava a totalidade da mente da cria infernal. Devemos ir até " + QuestCharacters.SomePlace( "tavern" ) + ", onde foram vistos pela última vez. Compre os suprimentos necessários e nos encontre amanhã em " + RandomThings.GetRandomCity() + ". Partiremos de lá."; break;
					case 31:	ScrollMessage = "" + QuestCharacters.ParchmentWriter() + ",<br><br>Sei que disse que voltaria para " + RandomThings.GetRandomCity() + " assim que encontrasse ouro no Império Selvagem. O que encontrei em vez disso era mais valioso que ouro. Este mundo é o único lugar onde encontrei um metal muito forte. Os orcs aqui o chamam de aço, e tenho coletado há muitas luas agora. Quando conseguir encher minha caravana, irei para " + RandomThings.GetRandomCity() + " e verei o que o ferreiro me dará por ele.<br><br>- " + QuestCharacters.ParchmentWriter() + ""; break;
					case 32:	ScrollMessage = "" + QuestCharacters.ParchmentWriter() + ",<br><br>Escrevo-lhe na esperança de que você possa se aventurar até o Véu Umbroso e auxiliar em meus esforços de mineração. Ouvi lendas de um metal misterioso, chamado latão. Diziam que só podia ser encontrado aqui. As lendas são verdadeiras! Se puder, passe por " + RandomThings.GetRandomCity() + " e pegue mais alguns cavalos de carga. Talvez até uma carroça. Encontrarei você em Renika no auge da meia-lua.<br><br>- " + QuestCharacters.ParchmentWriter() + ""; break;
					case 33:	ScrollMessage = "" + QuestCharacters.ParchmentWriter() + ",<br><br>Se você encontrar esta nota, significa que encontrei meu fim. O mago em " + RandomThings.GetRandomCity() + " estava errado. O feitiço de armadilha mágica que ele me ensinou não foi tão eficaz contra o demônio quanto ele disse que seria. Embora tenha funcionado nos lacaios que cruzaram meu caminho, o demônio foi capaz de usar psicologia em mim, o que permitiu que ele visse o símbolo rúnico que coloquei a seus pés. Mal escapei com vida. Agora estou aqui, tentando...parar...o... ...sangramento do... ... ...meu ferimento......"; break;
					case 34:	ScrollMessage = "" + QuestCharacters.ParchmentWriter() + ",<br><br>Vou precisar que você se esgueira em " + RandomThings.GetRandomCity() + " e pegue aquele " + RandomThings.RandomMagicalItem() + " do " + RandomThings.GetRandomJob() + ". Sei que você é procurado pela guarda da cidade, mas se conseguir um kit de disfarce da guilda dos ladrões, poderá entrar lá sem chamar a atenção dos guardas. Kits de disfarce são difíceis de usar, mas aqueles que alcançaram apenas um talento bruto de aprendiz em habilidades como psicologia, ninjutsu, furtividade, ocultação ou bisbilhotagem devem conseguir aplicá-lo efetivamente. Fique avisado que eles podem durar apenas algumas horas antes que o disfarce se desgaste, então faça seu trabalho e saia. Eles não são perfeitos, pois os comerciantes costumam desconfiar daqueles usando disfarces. Eles simplesmente não conseguem confiar em alguém que parece um pouco incomum. Além disso, alguns atos criminosos farão com que vejam seu verdadeiro eu, então mantenha-se discreto o máximo possível. Encontre-me na taverna em " + RandomThings.GetRandomCity() + ", e faremos a troca.<br><br> - " + QuestCharacters.ParchmentWriter(); break;
					case 35:	ScrollMessage = "" + QuestCharacters.ParchmentWriter() + ",<br><br>Vou precisar que você traga as carroças para " + RandomThings.GetRandomCity() + " pois um minerador lendário conseguiu extrair minério anão. Com isso podemos encontrar um ferreiro, superior a um grão-mestre, e fazer algumas armas e armaduras mágicas para a tarefa à nossa frente. Dizem que o metal cura os ferimentos de um guerreiro, e que outros benefícios mágicos desconhecidos podem ser extraídos dele. Só posso esperar por " + Utility.RandomMinMax( 2, 8 ) + " dias, então seja rápido.<br><br> - " + QuestCharacters.ParchmentWriter(); break;
					case 36:	ScrollMessage = "" + QuestCharacters.ParchmentWriter() + ",<br><br>Vou precisar que você traga as carroças para " + RandomThings.GetRandomCity() + " pois um lenhador lendário conseguiu cortar madeira élfica. Com isso podemos encontrar um carpinteiro, superior a um grão-mestre, e fazer algumas armas e armaduras mágicas para a tarefa à nossa frente. Dizem que a madeira cura os ferimentos de um guerreiro, e que outros benefícios mágicos desconhecidos podem ser extraídos dela. Só posso esperar por " + Utility.RandomMinMax( 2, 8 ) + " dias, então seja rápido.<br><br> - " + QuestCharacters.ParchmentWriter(); break;
					case 37:	ScrollMessage = "" + QuestCharacters.ParchmentWriter() + ",<br><br>Passei os últimos " + Utility.RandomMinMax( 2, 8 ) + " anos em " + RandomThings.GetRandomCity() + ", aprendendo os segredos de invocar criaturas mais poderosas do que pensávamos anteriormente. Esses magos da guilda não têm ideia do poder que possuem, mas eu tenho. Tenho pesquisado como usar psicologia em outras criaturas. Com esta sabedoria, serei capaz de tecer meus feitiços para invocar elementais e demônios mais fortes. Em breve " + RandomThings.GetRandomSociety() + " será um grupo digno de um lugar à mesa.<br><br> - " + QuestCharacters.ParchmentWriter(); break;
					case 38:	ScrollMessage = "" + QuestCharacters.ParchmentWriter() + ",<br><br>Passei os últimos " + Utility.RandomMinMax( 2, 8 ) + " anos em " + RandomThings.GetRandomCity() + ", aprendendo os segredos de invocar criaturas mais poderosas do que pensávamos anteriormente. Esses magos da guilda não têm ideia do poder que possuem, mas eu tenho. Tenho pesquisado como usar psicologia em outras criaturas. Com esta sabedoria, serei capaz de tecer meus feitiços para invocar elementais e demônios mais fortes. Em breve " + RandomThings.GetRandomSociety() + " será um grupo digno de um lugar à mesa.<br><br> - " + QuestCharacters.ParchmentWriter(); break;
					case 39:	ScrollMessage = "" + QuestCharacters.ParchmentWriter() + ",<br><br>Se você encontrar esta nota, significa que encontrei meu fim. Dizem que uma bruxa em Ravendark tem um livro que contém os segredos do portão do crânio. Que é possível viajar para outro mundo ao atravessá-lo. Não procurei este livro pois minha paciência estava escassa e minha confiança era forte. Assim que atravessei, não fui a lugar nenhum. Em vez disso, fui atingido por algum tipo de veneno mágico. Agora estou aqui, tentando...esperar...que...o... ...veneno...passe... ... ..."; break;
					case 40:	ScrollMessage = "" + QuestCharacters.ParchmentWriter() + ",<br><br>Tenho procurado os reagentes que precisamos para criar aquelas poções para nosso amigo, mas descobri que não posso carregar tanto quanto gostaria. Não quero lidar com um animal de carga, mas você ainda pode ajudar. Vá até " + RandomThings.GetRandomCity() + " e veja se consegue me arranjar uma mochila de alquimia. São bolsas especiais que ajudam a carregar mais itens de criação alquímica. Elas custam muito ouro, mas assim que conseguir uma, encontre-me em meu acampamento perto de " + RandomThings.MadeUpDungeon() + ".<br><br> - " + QuestCharacters.QuestGiver() + ""; break;
					case 41:	ScrollMessage = "" + QuestCharacters.ParchmentWriter() + ",<br><br>Estou vivo e bem e estou em " + RandomThings.GetRandomCity() + " me recuperando de meus ferimentos. Os mares estavam agitados e eu não deveria ter levado um barco tão pequeno tão longe. O " + RandomThings.GetRandomShipName( "", 0 ) + " estava à vista então sinalizei pedindo ajuda, o que eles fizeram. Infelizmente, perdi nosso barco quando o Capitão " + QuestCharacters.QuestGiver() + " decidiu usar um machado e cortá-lo em tábuas para usar. Eles disseram que era parte do meu pagamento por subir a bordo. Disseram que um bom carpinteiro pode conseguir muitas tábuas de um barco, mas um marinheiro consegue a melhor madeira dele. Planejo navegar com eles para tentar ganhar ouro suficiente para comprar um novo barco. Escreverei novamente em breve.<br><br> - " + QuestCharacters.QuestGiver() + ""; break;
					case 42:	
						switch ( Utility.Random( 2 ) )
						{
							case 0: ScrollMessage = "" + QuestCharacters.ParchmentWriter() + ",<br><br>A academia me informou que há uma pesquisa alquímica sobre misturas que não são comumente preparadas por alquimistas. Essas misturas nos permitem criar poções que, quando derramadas, liberarão um líquido que pode ajudar a vencer quaisquer inimigos que possamos encontrar em nossa jornada. Para fazer isso, precisamos de um livro chamado Misturas Alquímicas. Preciso que você vá até " + RandomThings.GetRandomCity() + " e veja se consegue adquirir um desses livros. Traga-o para meu laboratório quando o encontrar e começaremos o trabalho.<br><br> - " + QuestCharacters.QuestGiver() + ""; break;
							case 1: ScrollMessage = "" + QuestCharacters.ParchmentWriter() + ",<br><br>A academia me informou que há uma pesquisa alquímica sobre elixires que não são comumente preparados por alquimistas. Esses elixires aprimoram nossas habilidades e nos ajudarão muito em nossa jornada. Para fazer isso, precisamos de um livro chamado Elixires Alquímicos. Preciso que você vá até " + RandomThings.GetRandomCity() + " e veja se consegue adquirir um desses livros. Traga-o para meu laboratório quando o encontrar e começaremos o trabalho.<br><br> - " + QuestCharacters.QuestGiver() + ""; break;
						}
					break;
					case 43:	ScrollMessage = "" + QuestCharacters.ParchmentWriter() + ",<br><br>Aquele crânio de " + skull + " que você me trouxe era realmente interessante. Embora frágil, ele tinha uma propriedade interessante. A magia do conjurador morto estava presa dentro dele. Embora um mago da luz destruísse tais coisas, eu mesmo acho bastante útil para repor minha preciosa mana quando realizo meus rituais. Se você encontrar mais destes, eu pagaria generosamente.<br><br> - " + QuestCharacters.ParchmentWriter() + " o Necromante"; break;
					case 44:	ScrollMessage = QuestCharacters.ParchmentWriter() + ",<br><br>Encontramos uma antiga tábua que falava de um artefato em " + Server.Misc.QuestCharacters.SomePlace( "tablet" ) + ". Eu não sabia o que a tábua significava, pois precisei encontrar alguém mais inteligente para lê-la. " + QuestCharacters.ParchmentWriter() + " acredita que ele pode estar sobre aqueles pedestais estranhos com os símbolos rúnicos. Aqueles com os pequenos baús em cima. Se isso for verdade, precisamos encontrá-lo antes que " + RandomThings.GetRandomSociety() + " o faça. " + QuestCharacters.ParchmentWriter() + " e eu nos encontraremos com você em " + RandomThings.GetRandomCity() + " em alguns dias. Teremos que trazer a tábua conosco ou nunca o encontraremos. Não conte a ninguém sobre isso, pois há espiões por aí.<br><br> - " + QuestCharacters.ParchmentWriter(); break;
					case 45:	ScrollMessage = "" + QuestCharacters.ParchmentWriter() + ",<br><br>Finalmente consegui! Encontrei o Códex da Sabedoria Suprema enquanto viajava pela Ilha da Serpente. Quase todos que trouxe comigo foram mortos pela criatura amaldiçoada, mas consegui vencer a fera. Apenas " + QuestCharacters.ParchmentWriter() + " e eu conseguimos voltar para " + RandomThings.MadeUpCity() + ". Quando usei o Códex, pude ver o caminho para conseguir a Gema da Imortalidade. Venha quando puder. Precisamos partir para encontrar os orbes, pergaminhos e chaves se quisermos ter sucesso. Nossa jornada primeiro nos levará até " + RandomThings.MadeUpDungeon() + ". <br><br> - " + QuestCharacters.ParchmentWriter() + ""; break;
					case 46:
						string researcher = "sábio";
						switch ( Utility.RandomMinMax( 0, 2 ) )
						{
							case 0: researcher = "sábio";		break;
							case 1: researcher = "escriba";		break;
							case 2: researcher = "bibliotecário";	break;
						}
						ScrollMessage = "" + QuestCharacters.ParchmentWriter() + ",<br><br>Economizei os 500 de ouro necessários para comprar um pacote de pesquisa daquele " + researcher + " em " + RandomThings.GetRandomCity() + ". Comecei a fazer algumas pesquisas e descobri que preciso encontrar um cubo de poder em " + RandomThings.MadeUpDungeon() + ". Você poderia reunir nossos amigos e me encontrar lá quando a lua estiver na metade? Eu me sentiria melhor se tivesse companheiros para enfrentar os perigos lá dentro.<br><br> - " + QuestCharacters.ParchmentWriter() + ""; break;
					case 47:	ScrollMessage = "" + QuestCharacters.ParchmentWriter() + ",<br><br>Encontrei uma maneira de usar este orbe do abismo que você encontrou. Se você não quiser que ele interfira com seu amuleto, pode levá-lo a um funileiro e eles o modificarão em uma joia que você pode usar em vez disso. Depois que fizer isso, podemos ir ao Submundo e procurar pelos lendários Titãs sobre os quais lemos.<br><br> - " + QuestCharacters.ParchmentWriter(); break;
					case 48:	ScrollMessage = "" + QuestCharacters.ParchmentWriter() + ",<br><br>Aprendi os segredos deste misterioso crânio de dragão que encontramos em " + RandomThings.MadeUpDungeon() + ". É a essência de um dracolich, mas precisamos remover a aura sombria que oculta seu verdadeiro poder. Precisamos ir até a Masmorra Hythloth e encontrar o pentagrama sangrento do grande demônio. Se usarmos o crânio dentro do pentagrama, a aura será removida e então poderemos perseguir nosso objetivo de reanimar a criatura para fazer nossa vontade.<br><br> - " + QuestCharacters.ParchmentWriter(); break;
					case 49:	ScrollMessage = "" + QuestCharacters.ParchmentWriter() + ",<br><br>Um marinheiro em " + RandomThings.GetRandomCity() + " me contou uma história sobre Poseidon construindo pilares gêmeos de serpente no mar. Um conjunto eu encontrei em Sosaria, enquanto outro dizem existir em Lodoria. Se alguém pudesse aprender os segredos desses pilares, então poderia viajar para outro mundo passando entre eles. Se Poseidon de fato construiu esses portais, então talvez suas cavernas guardem a pista que preciso para usá-los. Se eu os encontrar, reuniremos a tripulação e deixaremos os ventos nos levarem até lá. Pode ser a rota marítima que precisamos para comerciar com os elfos."; break;		}
			}
		}

		public class ClueGump : Gump
		{
			public ClueGump( Mobile from, Item parchment ): base( 100, 100 )
			{
				SomeRandomNote scroll = (SomeRandomNote)parchment;
				string sText = scroll.ScrollMessage;
				from.PlaySound( 0x249 );

				this.Closable=true;
				this.Disposable=true;
				this.Dragable=true;
				this.Resizable=false;

				AddPage(0);

				AddImage(0, 0, 10901, 2786);
				AddImage(0, 0, 10899, 2117);
				AddHtml( 45, 78, 386, 218, @"<BODY><BASEFONT Color=#d9c781>" + sText + "</BASEFONT></BODY>", (bool)false, (bool)true);
			}

			public override void OnResponse( NetState state, RelayInfo info ) 
			{
				Mobile from = state.Mobile; 
				from.PlaySound( 0x249 );
			}
		}

		static string UppercaseFirst(string s)
		{
			if (string.IsNullOrEmpty(s))
			{
				return string.Empty;
			}
			return char.ToUpper(s[0]) + s.Substring(1);
		}

		public static string GetSpecialItem( int relic, int part )
		{
			string Part1 = "";
			string Part2 = "";

			switch ( relic )
			{
				case 1: Part1 = "Stonegate Castle"; Part2 = "heart of ash"; break;
				case 2: Part1 = "the Vault of the Black Knight"; Part2 = "mystical wax"; break;
				case 3: Part1 = "the Crypts of Dracula"; Part2 = "vampire teeth"; break;
				case 4: Part1 = "the Lodoria Catacombs"; Part2 = "face of the ancient king"; break;
				case 5: Part1 = "Dungeon Deceit"; Part2 = "wand of Talosh"; break;
				case 6: Part1 = "Dungeon Despise"; Part2 = "head of Urg"; break;
				case 7: Part1 = "Dungeon Destard"; Part2 = "flame of Dramulox"; break;
				case 8: Part1 = "the City of Embers"; Part2 = "crown of Vorgol"; break;
				case 9: Part1 = "Dungeon Hythloth"; Part2 = "claw of Saramon"; break;
				case 10: Part1 = "the Ice Fiend Lair"; Part2 = "horn of the frozen hells"; break;
				case 11: Part1 = "Dungeon Shame"; Part2 = "elemental salt"; break;
				case 12: Part1 = "Terathan Keep"; Part2 = "eye of plagues"; break;
				case 13: Part1 = "the Halls of Undermountain"; Part2 = "hair of the earth"; break;
				case 14: Part1 = "the Volcanic Cave"; Part2 = "skull of Turlox"; break;
				case 15: Part1 = "the Mausoleum"; Part2 = "tattered robe of Mezlo"; break;
				case 16: Part1 = "the Tower of Brass"; Part2 = "blood of the forest"; break;
				case 17: Part1 = "Vordo's Dungeon"; Part2 = "cinders of life"; break;
				case 18: Part1 = "the Dragon's Maw"; Part2 = "crystal scales"; break;
				case 19: Part1 = "the Ancient Pyramid"; Part2 = "chest of suffering"; break;
				case 20: Part1 = "Dungeon Exodus"; Part2 = "whip from below"; break;
				case 21: Part1 = "the Caverns of Poseidon"; Part2 = "scale of the sea"; break;
				case 22: Part1 = "Library of Bal Tsareth"; Part2 = "braclet of war"; break;
				case 23: Part1 = "Dardin's Pit"; Part2 = "stump of the ancients"; break;
				case 24: Part1 = "Dungeon Doom"; Part2 = "dark blood"; break;
				case 25: Part1 = "the Fires of Hell"; Part2 = "firescale tooth"; break;
				case 26: Part1 = "the Mines of Morinia"; Part2 = "ichor of Xthizx"; break;
				case 27: Part1 = "the Perinian Depths"; Part2 = "heart of a vampire queen"; break;
				case 28: Part1 = "the Dungeon of Time Awaits"; Part2 = "hourglass of ages"; break;
				case 29: Part1 = "the Ancient Prison"; Part2 = "shackles of Saramak"; break;
				case 30: Part1 = "the Cave of Fire"; Part2 = "mouth of embers"; break;
				case 31: Part1 = "the Cave of Souls"; Part2 = "cowl of shadegloom"; break;
				case 32: Part1 = "Dungeon Ankh"; Part2 = "wedding dress of virtue"; break;
				case 33: Part1 = "Dungeon Bane"; Part2 = "lilly pad of the bog"; break;
				case 34: Part1 = "Dungeon Hate"; Part2 = "immortal bones"; break;
				case 35: Part1 = "Dungeon Scorn"; Part2 = "staff of scorn"; break;
				case 36: Part1 = "Dungeon Torment"; Part2 = "mind of allurement"; break;
				case 37: Part1 = "Dungeon Vile"; Part2 = "mask of the ghost"; break;
				case 38: Part1 = "Dungeon Wicked"; Part2 = "dead venom flies"; break;
				case 39: Part1 = "Dungeon Wrath"; Part2 = "branch of the reaper"; break;
				case 40: Part1 = "the Flooded Temple"; Part2 = "ink of the deep"; break;
				case 41: Part1 = "the Gargoyle Crypts"; Part2 = "amulet of the stygian abyss"; break;
				case 42: Part1 = "the Serpent Sanctum"; Part2 = "skin of the guardian"; break;
				case 43: Part1 = "the Tomb of the Fallen Wizard"; Part2 = "orb of the fallen wizard"; break;
				case 44: Part1 = "the Blood Temple"; Part2 = "bleeding crystal"; break;
				case 45: Part1 = "the Dungeon of the Mad Archmage"; Part2 = "jade idol of Nesfatiti"; break;
				case 46: Part1 = "the Tombs"; Part2 = "scroll of Abraxus"; break;
				case 47: Part1 = "the Dungeon of the Lich King"; Part2 = "sphere of the dark circle"; break;
				case 48: Part1 = "the Forgotten Halls"; Part2 = "urn of Ulmarek's ashes"; break;
				case 49: Part1 = "the Ice Queen Fortress"; Part2 = "crystal of everfrost"; break;
				case 50: Part1 = "Dungeon Rock"; Part2 = "stone of the night gargoyle"; break;
				case 51: Part1 = "the Scurvy Reef"; Part2 = "pearl of Neptune"; break;
				case 52: Part1 = "the Undersea Castle"; Part2 = "Black Beard's brandy"; break;
				case 53: Part1 = "the Tomb of Kazibal"; Part2 = "lamp of the desert"; break;
				case 54: Part1 = "the Azure Castle"; Part2 = "azure dust"; break;
				case 55: Part1 = "the Catacombs of Azerok"; Part2 = "skull of Azerok"; break;
				case 56: Part1 = "Dungeon Covetous"; Part2 = "egg of the harpy hen"; break;
				case 57: Part1 = "the Glacial Scar"; Part2 = "bone of the frost giant"; break;
				case 58: Part1 = "the Temple of Osirus"; Part2 = "mind of silver"; break;
				case 59: Part1 = "the Sanctum of Saltmarsh"; Part2 = "scale of Scarthis"; break;
				case 60: Part1 = "the Hive of the Eye Tyrant"; Part2 = "Lidless Eye";break;
			}

			if ( part > 0 ){ return Part2; }
			return Part1;
		}

		public override void OnDoubleClick( Mobile e )
		{
			if ( !IsChildOf( e.Backpack ) ) 
			{
				e.SendMessage( "Isso precisa estar na sua bolsa para ler." );
			}
			else
			{
				e.CloseGump( typeof( ClueGump ) );
				e.SendGump( new ClueGump( e, this ) );
			}
		}

		public SomeRandomNote(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int) 0);
            writer.Write( ScrollMessage );
            writer.Write( ScrollTrue );
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
			ScrollMessage = reader.ReadString();
			ScrollTrue = reader.ReadInt();
		}
	}
}
