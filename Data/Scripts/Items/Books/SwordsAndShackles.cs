using System;
using Server;
using System.Collections;
using System.Collections.Generic;
using Server.Misc;
using Server.Network;
using Server.Gumps;
using Server.Mobiles;

namespace Server.Items
{
	public class SwordsAndShackles : Item
	{
		[Constructable]
		public SwordsAndShackles() : base( 0x529D )
		{
			Weight = 1.0;
			Hue = 0x944;
			ItemID = Utility.RandomList( 0x529D, 0x529E );
			Name = "Skulls and Shackles";
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( from.InRange( this.GetWorldLocation(), 4 ) || this.Weight == -50.0 )
			{
				from.CloseGump( typeof( SwordsAndShacklesGump ) );
				from.SendGump( new SwordsAndShacklesGump( from, 1 ) );
				Server.Gumps.MyLibrary.readBook ( this, from );
			}
		}

		public SwordsAndShackles( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int)1 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}

		public class SwordsAndShacklesGump : Gump
		{
			private int m_Page;

			public SwordsAndShacklesGump( Mobile from, int page ): base( 50, 50 )
			{
				from.SendSound( 0x55 );
				string color = "#76b4d4";
				m_Page = page;

				this.Closable=true;
				this.Disposable=true;
				this.Dragable=true;
				this.Resizable=false;

				int max = 11; // MAX PAGES
				int page1 = m_Page-1; if ( page1 < 1 ){ page1 = max; }
				int page2 = m_Page+1; if ( page2 > max ){ page2 = 1; }

				AddImage(0, 0, 7010, 2878);
				AddImage(0, 0, 7011);
				AddImage(0, 0, 7025, 2736);
				AddButton(110, 67, 4014, 4014, page1, GumpButtonType.Reply, 0);
				AddButton(906, 70, 4005, 4005, page2, GumpButtonType.Reply, 0);

				AddHtml( 596, 72, 299, 20, @"<BODY><BASEFONT Color=" + color + "><CENTER>CRÂNIOS E ALGEMAS</CENTER></BASEFONT></BODY>", (bool)false, (bool)false);

				if ( m_Page == 1 )
				{
					AddHtml( 151, 72, 299, 20, @"<BODY><BASEFONT Color=" + color + "><CENTER>CONCEITOS BÁSICOS DA PESCA</CENTER></BASEFONT></BODY>", (bool)false, (bool)false);
					AddHtml( 116, 107, 393, 486, @"<BODY><BASEFONT Color=" + color + ">A pesca é a arte paciente de atrair peixes para sua isca na busca por alimentar-se. Em sua forma básica, você adquiriria uma vara de pesca e iria para a costa para ver o que consegue pegar. Varas de pesca podem ser fabricadas por carpinteiros ou compradas de pescadores. Uma vara de pesca só pode ser usada um número determinado de vezes e eventualmente quebrará com o uso excessivo. Simplesmente segure a vara de pesca, use-a e selecione um local nearby na água para pescar. Durante esta prática inicial, você pegará alguns peixes ou puxará roupas encharcadas ou armaduras enferrujadas. Pode ser mórbido, mas você pode até puxar os restos de uma vítima de afogamento ou uma bolsa que alguém pode ter deixado cair na água anos atrás.<br><br>Você não pode praticar na costa para sempre se planeja se tornar ainda mais proficiente. Depois de atingir o nível de aprendiz (50) em navegação, você terá que adquirir um navio e velejar para longe da segurança da costa. Aqui você pegará mais tipos de coisas, mas também arriscará pescar serpentes marinhas. Se você enfrentar tal besta e reivindicar vitória sobre ela, poderá encontrar itens especiais. Se você avistar um navio naufragado abaixo da superfície das ondas, poderá puxar algum tesouro decorativo dos destroços abaixo.<br><br>É provável que você encontre alguns peixes exóticos incomuns no mar. Embora você seja livre para cortá-los e cozinhá-los, eles geralmente são mais valiosos devido à sua raridade. Se quiser ganhar algum ouro por esses tipos de peixe, simplesmente encontre um cais que tenha um barril de peixes e coloque o peixe dentro dele. Você será premiado com uma quantia em ouro que pode ser aumentada com base em sua habilidade de navegação.<br><br>Peixes mágicos são frequentemente capturados por pescadores especialistas, e você pode simplesmente comê-los crus e ter um aumento temporário de força, destreza ou inteligência. Algas incomuns podem ficar presas em sua linha e não devem ser simplesmente jogadas fora. Examine essas plantas porque elas comumente têm propriedades alquímicas que podem ser de grande utilidade quando longe da terra. Você precisará de frascos vazios e então poderá usar a alga para tentar espremer os fluidos delas para criar poções. Como mencionado anteriormente, você provavelmente pode puxar armaduras e armas enferrujadas. Embora inúteis para aventureiros, esses itens enferrujados podem ser derretidos e reutilizados por trabalhadores de ferro e ferreiros na cidade. Simplesmente leve os itens para suas lojas e coloque-os em barris de ferro-velho para então adquirir o ouro. O pagamento por tais itens é uma moeda de ouro por pedra de peso do item enferrujado.<br><br>Aqueles que escolhem ser conhecidos por esta profissão são conhecidos como 'marinheiros' ou 'piratas', e depende do seu karma qual deles você é. Aqueles de origens bárbaras terão títulos de 'Atlante' pois essa é sua herança marítima. Grão-mestres desta habilidade recebem o título adicional de 'capitão'.</BASEFONT></BODY>", (bool)false, (bool)true);
					AddImage(594, 180, 10887);
				}
				else if ( m_Page == 2 )
				{
					AddHtml( 151, 72, 299, 20, @"<BODY><BASEFONT Color=" + color + "><CENTER>ARPÕES DE ARREMESSO</CENTER></BASEFONT></BODY>", (bool)false, (bool)false);
					AddHtml( 116, 107, 393, 486, @"<BODY><BASEFONT Color=" + color + ">Arpões são às vezes a arma de escolha para marinheiros dos mares. Usando a habilidade de pontaria, pode-se lançar uma arma semelhante a uma lança em direção ao seu inimigo. Embora comerciantes marítimos vendam tais armas, ferreiros são capazes de construí-las. Para usar um arpão, deve-se ser capaz de arremessá-lo e puxá-lo de volta para si para poder arremessá-lo novamente. Para conseguir isso, seria necessário um bom suprimento de corda de arpão. Este estilo de corda é barato e também comumente vendido por comerciantes marítimos. Alfaiates também são capazes de tecer tal corda. Sempre que você arremessa um arpão, essas cordas geralmente são gastas, então certifique-se de trazer um bom suprimento com você em sua jornada se usar tal arma. Quanto melhor sua habilidade de navegação, mais eficaz você pode ser com tais armas.</BASEFONT></BODY>", (bool)false, (bool)false);
					AddImage(594, 180, 10887);
				}
				else if ( m_Page == 3 )
				{
					AddHtml( 151, 72, 299, 20, @"<BODY><BASEFONT Color=" + color + "><CENTER>SEU PRÓPRIO NAVIO</CENTER></BASEFONT></BODY>", (bool)false, (bool)false);
					AddHtml( 116, 107, 393, 486, @"<BODY><BASEFONT Color=" + color + ">Quando você ganhou ouro suficiente com seus vários esforços, pode ser capaz de possuir seu próprio navio. Construtores navais frequentemente vendem navios, onde quanto maior o navio, maior o porão para armazenar sua carga. Para lançar um navio, ou colocá-lo em doca seca, você mais comumente precisa estar em um cais para fazê-lo. Há exceções a isso, pois há uma pequena quantidade de ilhas que permitem lançar e atracar seu navio. Capitães do mar não sofrem com esta limitação, pois podem lançar e atracar seus navios em qualquer costa. Comerciantes marítimos às vezes vendem lanternas de atracação, que são lanternas especialmente brilhantes para navios atracarem. Estas só podem ser usadas de sua casa e permitem que você construa uma casa à beira-mar para atracar e lançar seu navio se você não atingiu esse nível de habilidade de navegação. Quando você lança seu navio, recebe uma chave para sua mochila e seu banco. Você pode usá-la para proteger seu barco ou lançar magia de recall de teletransporte nele para retornar ao seu navio de longe. Se você não estiver near de seu navio e não tiver tal magia, pode dar sua chave a um construtor naval e eles o transportarão para seu vessel. Isso custará 1.000 de ouro, é claro.<br><br>Para pilotar seu navio, simplesmente esteja a bordo e clique duas vezes no timoneiro. Um mecanismo de direção aparecerá e você poderá então navegar pelos mares. Este mecanismo também permite que você renomeie seu vessel e levante ou solte a âncora. Você deve estar a bordo ao usar este mecanismo. O centro do mecanismo é transparente para que você possa posicioná-lo sobre seu mapa de radar, se desejar. O mecanismo permite que você dimensione-o para corresponder aos dois estilos de mapas de radar para sobreposição.<br><br>Mestres na habilidade de navegação terão um recurso adicional em seus navios que outros marinheiros não têm, um convés inferior. Esta é uma área pública abaixo de seu navio que tem confortos como uma taverna, fornecedor, banco e curandeiro. Contanto que você não esteja em combate em seu navio, pode descer ao convés inferior e relaxar. Se seu navio for comandado a velejar e você descer ao convés inferior, seu navio continuará navegando até ser parado por um obstáculo. Se seus camaradas do mundo real estiverem no convés inferior e você atracar seu navio, seus camaradas aparecerão na terra onde você atracou seu navio.</BASEFONT></BODY>", (bool)false, (bool)true);
					AddImage(638, 180, 10890);
				}
				else if ( m_Page == 4 )
				{
					AddHtml( 151, 72, 299, 20, @"<BODY><BASEFONT Color=" + color + "><CENTER>NAVEGANDO SEU NAVIO</CENTER></BASEFONT></BODY>", (bool)false, (bool)false);
					AddImage(279, 305, 10921);
					AddHtml( 126, 102, 801, 203, @"<BODY><BASEFONT Color=" + color + ">Depois de lançar seu navio no mar, é hora de subir a bordo e velejar. Enquanto estiver a bordo, clique duas vezes no timoneiro e uma janela aparecerá com um volante de direção. Os botões dourados no próprio volante moverão seu vessel naquela direção. O botão vermelho no centro parará seu navio. Há botões adicionais e suas funções estão rotuladas à esquerda. Essas funções soltam ou levantam sua âncora, nomeiam seu vessel, viram à esquerda ou direita, dão meia-volta ou definem seu barco para se mover um passo de cada vez (use o mesmo botão para desativar o passo único para mover normalmente). Você também pode usar mapas para traçar um curso a seguir. Você só pode fazer isso em mapas desenhados por outros personagens e não pode fazer isso nos mapas mundiais realmente grandes. Abra o mapa e escolha a opção de traçar curso no topo. Escolha os vários pontos de caminho no mapa, garantindo que nenhuma massa de terra atrapalhe. Quando terminar, clique no topo do mapa para indicar que terminou de traçar o curso. Você também pode limpar o curso traçado com a opção na parte inferior. Se estiver satisfeito com o curso traçado, entregue o mapa ao timoneiro, onde ele verificará se você realmente tem um mapa. Diga ao timoneiro para seguir este curso simplesmente dizendo 'start' enquanto estiver em seu navio. O timoneiro então seguirá o curso que você traçou.</BASEFONT></BODY>", (bool)false, (bool)false);
				}
				else if ( m_Page == 5 )
				{
					AddHtml( 151, 72, 299, 20, @"<BODY><BASEFONT Color=" + color + "><CENTER>REDES DE PESCA</CENTER></BASEFONT></BODY>", (bool)false, (bool)false);
					AddHtml( 116, 107, 393, 486, @"<BODY><BASEFONT Color=" + color + ">Redes de pesca são itens raros que um pescador usaria para ver o que pode trazer das profundezas. Estas podem ser comumente encontradas matando monstros marinhos ou obtendo tesouro oceânico e têm quatro níveis diferentes de dificuldade para usá-las:<br><br>-Rede de Pesca<br>-Rede de Pesca Resistente<br>-Rede de Pesca Antiga<br>-Rede de Pesca de Netuno<br><br>Essas redes não podem ser usadas near de nenhuma costa. Quando usada, a rede afunda lentamente no mar com algumas bolhas, outra onda de bolhas anuncia a chegada de criaturas proporcionais aparecendo around de seu navio, então esteja pronto para lutar. Essas redes são usadas pelos mais corajosos marinheiros que procuram fazer um nome para si mesmos no alto mar.</BASEFONT></BODY>", (bool)false, (bool)false);
					AddImage(594, 180, 10887);
				}
				else if ( m_Page == 6 )
				{
					AddHtml( 151, 72, 299, 20, @"<BODY><BASEFONT Color=" + color + "><CENTER>MENSAGENS EM UMA GARRAFA</CENTER></BASEFONT></BODY>", (bool)false, (bool)false);
					AddHtml( 116, 107, 393, 486, @"<BODY><BASEFONT Color=" + color + ">Ocasionalmente, um marinheiro sortudo descobrirá uma mensagem em uma garrafa. Olhar a nota inside revelará uma mensagem de um marinheiro a bordo de um navio que está afundando. As notas parecerão antigas, com certeza, então as chances de que o navio sobreviveu ao seu destino são pequenas. Se você deseja tentar pescar os destroços, precisa pegar um sextante e uma vara de pesca, embarcar em seu navio e navegar para as coordenadas na nota. Uma vez que você chegue ao local, certifique-se de ter a mensagem engarrafada em sua mochila e então você pode começar a pescar nas águas. O objetivo da maioria dos marinheiros é trazer o baú de saque que o navio continha antes de repousar nas profundezas turvas abaixo. Enquanto tenta trazer este baú, você trará outras partes dos destroços, como pinturas e ossos dos marinheiros que encontraram seu destino naquele navio. Tenha em mente que esses naufrágios não são como os que você pode ver visualmente no fundo do oceano. Você pode pescar nesses locais, no entanto, o baú de riquezas que continha foi perdido para sempre há muito tempo.</BASEFONT></BODY>", (bool)false, (bool)false);
					AddImage(594, 180, 10887);
				}
				else if ( m_Page == 7 )
				{
					AddHtml( 151, 72, 299, 20, @"<BODY><BASEFONT Color=" + color + "><CENTER>BARCOS ABANDONADOS</CENTER></BASEFONT></BODY>", (bool)false, (bool)false);
					AddHtml( 116, 107, 393, 486, @"<BODY><BASEFONT Color=" + color + ">Há aqueles que saem pela manhã para pegar alguns peixes para suas famílias comerem. Eles têm seus pequenos barcos e se afastam da costa para ter uma chance melhor de pegar peixes maiores. Há alguns, no entanto, que nunca retornam. Quer tenham caído de seus barcos e se afogado, ou suas linhas de âncora quebraram e seus barcos à deriva, esses barcos agora abandonados flutuam nos mares para que outros talvez os encontrem. Estes often contêm bens e tesouros menores, então sinta-se à vontade para revistá-los se você navegar por um. A maioria dos marinheiros often traz um machado com eles para que possam cortar esses navios para salvar alguma madeira utilizável. Quanto mais habilidoso o marinheiro, melhor a qualidade da madeira que pode ser salva. A quantidade de madeira é greatly afetada pela habilidade de carpintaria do marinheiro.</BASEFONT></BODY>", (bool)false, (bool)false);
					AddItem(675, 397, 8857);
					AddItem(682, 203, 8862);
				}
				else if ( m_Page == 8 )
				{
					AddHtml( 151, 72, 299, 20, @"<BODY><BASEFONT Color=" + color + "><CENTER>BARCOS PEQUENOS</CENTER></BASEFONT></BODY>", (bool)false, (bool)false);
					AddHtml( 116, 107, 393, 486, @"<BODY><BASEFONT Color=" + color + ">Às vezes há barcos pequenos no mar e eles podem ser facilmente identificados pois não têm velas ou timoneiro. Eles terão, no entanto, um único membro da tripulação a bordo. Eles podem ser um simples cidadão pescando pelo dia, ou talvez transportando carga para outro lugar além do horizonte. Eles também podem ser um pirata patife que ainda não fez um nome para ter uma tripulação e um galeão à sua disposição. Como você lida com esses barcos depende de sua moralidade. Os marinheiros inocentes often ignoram os outros, a menos que sejam assassinos, criminosos ou simplesmente tenham um karma desagradável. Os bucaneiros e afins, no entanto, simplesmente atacarão você à vista. No entanto, como você lida com esses marinheiros é com você. Você pode atacá-los de longe ou pode embarcar em seu navio se tiver um gancho de abordagem em sua mochila. Ganchos de abordagem são vendidos por comerciantes marítimos e são usados para embarcar nesses barcos menores ou enormes galeões. Para embarcar em um navio, simplesmente use o gancho de abordagem e direcione-se ao membro da tripulação no barco. Se você matar o membro da tripulação, seu barco afundará com apenas uma pequena seção saindo de beneath das ondas. Felizmente, esta será a carga do barco, então você pode revistar seus pertences para ver o que deseja tomar para si. Como barcos abandonados, você também pode cortar esses cascos para madeira.</BASEFONT></BODY>", (bool)false, (bool)false);
					AddItem(638, 254, 20889);
					AddItem(692, 307, 6045);
					AddImage(809, 463, 10889);
					AddItem(650, 352, 6055);
					AddItem(711, 299, 6053);
					AddItem(671, 329, 6045);
				}
				else if ( m_Page == 9 )
				{
					AddHtml( 151, 72, 299, 20, @"<BODY><BASEFONT Color=" + color + "><CENTER>GALEÕES</CENTER></BASEFONT></BODY>", (bool)false, (bool)false);
					AddHtml( 116, 107, 393, 486, @"<BODY><BASEFONT Color=" + color + ">Galeões são navios muito grandes que muito poucos poderiam adquirir. Você nunca estaria em posição de possuir tal vessel, mas há aqueles por aí que os têm e você os verá no alto mar. Como os barcos pequenos, você pode ter os inocentes ou os vilões nesses navios. Piratas tendem a matar a tripulação dos marinheiros inocentes e tomar toda a sua riqueza. Quaisquer que sejam suas motivações, você pode escolher atacar qualquer galeão que encontrar. Um capitão de galeão pode ter uma tripulação de 9-12 membros, e todos eles atirarão ou lançarão algo em inimigos nearby. Alguns lançam pedras, adagas, arpões ou pedregulhos. Outros podem ser mais inclinados magicamente e desencadear rajadas de fogo, frio ou energia. Capitães de galeão often deixam sua tripulação lutar, participando apenas um pouco para alguns quando inimigos não embarcaram em seus vessels.<br><br>Embora você possa atirar na tripulação você mesmo, pode não querer ou ter meios para fazê-lo. Como barcos menores, para embarcar em um galeão você precisará de um gancho de abordagem. Simplesmente use-o e direcione-se a qualquer membro da tripulação para lançar-se a bordo de seu navio. Se você precisar retornar ao seu navio, pode usar a prancha de seu navio para fazê-lo. Esteja avisado, que o capitão também tem um gancho de abordagem que ele tende a usar. Se você acha que pode atacar o capitão da segurança de seu convés, estaria enganado. Eles provavelmente usarão seu gancho de abordagem para agarrá-lo e puxá-lo em direção ao seu navio para ter mais facilidade em matá-lo.<br><br>Diferente de barcos menores, capitães de galeão podem ser bastante fortes ou às vezes oponentes muito poderosos. Eles não conseguiram um galeão e uma tripulação sem se tornar assim. Se você planeja atacar tal vessel, seria sábio julgar a luta conforme ela progride. Quanto mais poderoso o capitão, no entanto, melhor o saque dentro da carga do navio. Muitos pensam em piratas e marinheiros como meramente homens ou elfos. O alto mar está cheio de outras criaturas que buscam saque nas ondas. Você pode enfrentar um ogro poderoso e sua tripulação de criaturas hediondas. Um demônio do inferno poderia ter um vessel com uma tripulação demoníaca procurando por almas. Se você é um pirata, pode encontrar uma tripulação de milícia de soldados navegantes, procurando por pessoas como você para trazer à justiça. Seja qual for a tripulação, eles apoiarão seus capitães com suas vidas. Quaisquer membros restantes da tripulação curarão seus capitães, então você deve despachar a tripulação antes de enfrentar o capitão.<br><br>Se você conseguir afundar um galeão, verá sua carga aparecendo na superfície da água. Como os barcos menores, você pode vasculhar essa carga para quaisquer bens e riquezas que desejar. Se você está atrás de algumas boas tábuas de convés, certifique-se de cortar a parte restante do navio. Certifique-se de examinar cuidadosamente o saque de um pirata, pois pode haver uma recompensa por suas cabeças e o pergaminho de procurado pode estar na carga. Você pode ler o pergaminho para ver qual é a recompensa por aquele pirata em particular. Entregar este pergaminho de recompensa a um guarda da cidade ou vila concederá a você a recompensa.<br><br>Por fim, nem todos os inimigos no alto mar são piratas. Alguns são cultistas que adoram as muitas divindades da água vis da mitologia e lenda. Eles buscam apenas livrar os mares daqueles que trespassam no domínio de seu deus. Como piratas, eles são often procurados por justiça ou temidos por marinheiros. Eles tomam o que querem, desde que atenda a seus planos sinistros, e têm pouca simpatia por aqueles de quem tiram.</BASEFONT></BODY>", (bool)false, (bool)true);
					AddImage(598, 249, 10886);
				}
				else if ( m_Page == 10 )
				{
					AddHtml( 151, 72, 299, 20, @"<BODY><BASEFONT Color=" + color + "><CENTER>CARGA</CENTER></BASEFONT></BODY>", (bool)false, (bool)false);
					AddHtml( 116, 107, 393, 486, @"<BODY><BASEFONT Color=" + color + ">A maioria dos navios lida com carga. Quer estejam transportando-a de assentamento para assentamento, ou a tenham em seus porões como saque, a carga é uma grande moeda no alto mar. Quando você afunda um vessel, seja um barco ou galeão, sua carga pode conter alguns tipos de carga. Pode ser caixotes, baús, toneis ou barris. Você pode espiar o conteúdo da carga e determinar o que é e como melhor usá-la. Se você reivindicar carga de piratas ou vilões, a carga indicará que foi apreendida e que você ganhará karma se escolher dar as mercadorias ao comerciante listado que as desejaria. Se você tomar carga dos inocentes, então indicará que foi saqueada e que você perderá karma se escolher trocá-la com comerciantes. Fama pode ser ganha não importa a fonte da carga.<br><br>A carga tem um valor e é determinado por alguns fatores. O primeiro é um valor aleatório da própria carga. Você então leva em conta quanta habilidade você tem como marinheiro (navegação) e quão eficaz você é como comerciante (mercantil). Se você é daqueles que mendiga, e sua postura está definida para mendicância, você pode ganhar algum ouro extra pela carga. Ser membro da Guilda dos Marinheiros também fornecerá um bônus ao valor da carga. O último fator é se você está em um porto ou não. Desfazer-se da carga em um porto renderá mais ouro, mas você é livre para se livrar dela em qualquer lugar onde possa encontrar o comerciante apropriado.<br><br>Quase toda a carga, no entanto, pode simplesmente ser reivindicada por você mesmo. Cada carga que você reivindicar terá a opção apropriada para você fazê-lo. A informação na carga indicará a você quanto de algo está armazenado dentro do recipiente de carga. Se você escolher ficar com ela, obterá o conteúdo assim como o recipiente que a continha. Então, se você quiser um barril de 100 limões, obterá o barril e os 100 limões. Qual carga você deseja manter é com você.</BASEFONT></BODY>", (bool)false, (bool)true);
					AddImage(594, 180, 10887);
				}
				else if ( m_Page == 11 )
				{
					AddHtml( 151, 72, 299, 20, @"<BODY><BASEFONT Color=" + color + "><CENTER>PORTOS MARÍTIMOS</CENTER></BASEFONT></BODY>", (bool)false, (bool)false);
					AddHtml( 116, 107, 393, 486, @"<BODY><BASEFONT Color=" + color + ">Há portos marítimos nas muitas terras que são lares para marinheiros e piratas que têm pouco interesse em retornar ao continente. <br><br>Sosaria; Porto da Rocha da Âncora<br>Lodoria: Porto do Recife da Kraken<br>Ilha da Serpente: Porto das Velas da Serpente<br>Ilhas do Pavor: Porto das Sombras<br>Império Selvagem: Porto dos Mares Selvagens<br><br>Estes portos são território neutro para marinheiros e piratas, então as leis são muito rigorosas para que nenhum mal venha àqueles que visitam. Alguns dos cais around destes portos são maiores que outros, onde o Porto das Sombras está escondido near do Farol Esquecido. Rocha da Âncora e Recife da Kraken são construídos em cima de grandes áreas cavernosas, onde alguns marinheiros escolhem construir suas casas. Alguns não se incomodam em construir casas nessas cavernas, mas instead mineram nepturita ou madeira de árvores mortas dentro. A área de assentamento, no topo desta formação rochosa, é onde os marinheiros vêm descansar e negociar por bens e serviços. Esta parte particular do porto é uma área pública, onde aqueles que visitam qualquer vila portuária estarão dentro da mesma área portuária para melhor interagir uns com os outros. Portos são um local comum onde marinheiros ou piratas se desfazem de sua carga, pois podem obter mais ouro por tais transações.<br><br>A área do cais é onde você pode deixar seu navio se escolher embarcar novamente mais tarde. Dependendo do porto que você visita, o cais pode ser muito grande ou bastante pequeno. Alguns têm acesso a outras áreas públicas como o banco, taverna ou guildas. Qualquer criatura perseguindo seu navio often mergulhará abaixo da superfície ao se aproximar dos cais, é por isso que muitos marinheiros buscam esses refúgios quando estão fracos demais para lutar.</BASEFONT></BODY>", (bool)false, (bool)true);
					AddImage(596, 140, 10891);
				}
			}

			public override void OnResponse( NetState state, RelayInfo info ) 
			{
				Mobile from = state.Mobile; 
				if ( info.ButtonID > 0 || info.ButtonID < 0 )
				{
					from.SendGump( new SwordsAndShacklesGump( from, info.ButtonID ) );
				}
				else
					from.PlaySound( 0x55 );
			}
		}
	}
}