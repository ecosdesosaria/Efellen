using System;
using Server;
using Server.Misc;
using Server.Gumps;
using Server.Network;
using Server.Commands;
using Server.Items;
using System.Text;
using Server.Mobiles;
using System.Collections;
using Server.Commands.Generic;
using System.Globalization;

namespace Server.Gumps
{
	public class EpicGump : Gump
	{
        public EpicGump( Mobile talker, Mobile listener, bool allowed, string alignment ) : base( 25, 25 )
        {
			string sTitle = "";
			string sText = "";

			string myName = talker.Name;
			string yourName = listener.Name;
			
			string thisItem = CultureInfo.CurrentCulture.TextInfo.ToTitleCase( Server.Mobiles.EpicCharacter.GetSpecialItemRequirement( listener ) );

			string sInfo = "<br><br>Estes itens podem ser personalizados para se adequar ao seu estilo de aventura. Quando você obtiver um desses itens de tributo, clique uma vez no item e selecione a opção 'Encantar'. Um menu aparecerá que permitirá que você gaste os pontos fornecidos nos atributos que escolher. Tenha cuidado, pois você não pode alterar um atributo depois de selecioná-lo. Uma vez que os pontos tenham sido usados, o item permanecerá como está.";

			string sBare = "<br><br>" + myName + " lhe oferecerá um item de tributo se você recuperar um item raro...<br><br>" + thisItem + "<br><br>...e tiver alcançado uma fama de pelo menos 7.000 pontos. Se você aceitar seu tributo, sua fama diminuirá em 7.000 pontos e você terá que reconstruí-la novamente. Se você alcançou essa quantidade, clique uma vez em " + myName + " e selecione Tributo para escolher o tipo de item que deseja. " + myName + " também precisará de pelo menos 5.000 de ouro para construir o item para você.";

			if ( alignment == "good" )
			{
				sInfo = "<br><br>Estes itens podem ser personalizados para se adequar ao seu estilo de aventura. Quando você obtiver um desses itens de tributo, clique uma vez no item e selecione a opção 'Encantar'. Um menu aparecerá que permitirá que você gaste os pontos fornecidos nos atributos que escolher. Tenha cuidado, pois você não pode alterar um atributo depois de selecioná-lo. Uma vez que os pontos tenham sido usados, o item permanecerá como está.";

				sBare = "<br><br>" + myName + " lhe oferecerá um item de tributo se você recuperar um item raro...<br><br>" + thisItem + "<br><br>...e tiver alcançado uma fama de pelo menos 4.000 pontos e um karma de pelo menos 4.000 pontos. Se você aceitar seu tributo, sua fama e karma diminuirão em 4.000 pontos e você terá que reconstruí-los novamente. Se você alcançou essas quantidades, clique uma vez em " + myName + " e selecione Tributo para escolher o tipo de item que deseja. " + myName + " também precisará de pelo menos 5.000 de ouro para construir o item para você.";
			}
			else if ( alignment == "evil" )
			{
				sInfo = "<br><br>Estes itens podem ser personalizados para se adequar ao seu estilo de aventura. Quando você obtiver um desses itens de tributo, clique uma vez no item e selecione a opção 'Encantar'. Um menu aparecerá que permitirá que você gaste os pontos fornecidos nos atributos que escolher. Tenha cuidado, pois você não pode alterar um atributo depois de selecioná-lo. Uma vez que os pontos tenham sido usados, o item permanecerá como está.";

				sBare = "<br><br>" + myName + " lhe oferecerá um item de tributo se você recuperar um item raro...<br><br>" + thisItem + "<br><br>...e tiver alcançado uma fama de pelo menos 4.000 pontos e um karma de pelo menos -4.000 pontos ou menor. Se você aceitar seu tributo, sua fama diminuirá em 4.000 pontos e seu karma aumentará em 4.000 pontos. Você terá que reconstruí-los novamente. Se você alcançou essas quantidades, clique uma vez em " + myName + " e selecione Tributo para escolher o tipo de item que deseja. " + myName + " também precisará de pelo menos 5.000 de ouro para construir o item para você.";
			}

			if ( myName == "Lord Draxinusom" )
			{
				sTitle = "A Raça Gargula";
				sText = "Saudações, " + yourName + " e bem-vindo à nossa grande cidade. Suas façanhas são conhecidas por toda a terra e gostaríamos de prestar tributo a você. Temos armas que poderiam ajudar na luta contra os ofidianos ou elementos da terra, mas muitos de nossos itens ajudam aqueles que trabalham com minérios e metais." + sInfo + sBare;

				if ( allowed == false )
				{
					sText = "Saudações, " + yourName + " e bem-vindo à nossa grande cidade. Eu lhe ofereceria tributo, mas você é bastante desconhecido por toda a terra. Talvez se você aventurasse mais, suas histórias começariam a ser contadas por outros." + sBare;
				}
			}
			else if ( myName == "the Great Earth Serpent" )
			{
				sTitle = "Buscando o Equilíbrio";
				sText = "Saudações, " + yourName + ". Suas façanhas me mostraram que você poderia talvez fornecer equilíbrio à terra, e assim eu gostaria de prestar tributo a você. Eu poderia invocar armas que o ajudariam com os ofidianos que tentam destruir o equilíbrio, ou auxiliá-lo em lidar com meus irmãos da ordem e do caos." + sInfo + sBare;

				if ( allowed == false )
				{
					sText = "Saudações, " + yourName + ". Eu lhe ofereceria tributo para ajudar com a ordem do equilíbrio, mas você é bastante desconhecido por toda a terra. Talvez você precise provar a si mesmo e me mostrar que pode de fato fornecer equilíbrio à terra." + sBare;
				}
			}
			else if ( myName == "Morphius" )
			{
				sTitle = "A Mão Necrótica";
				sText = "Ora, ora, ora. Vejo que " + yourName + " veio buscar poder no reino dos mortos. Você tem estado bastante ocupado, auxiliando meus planos como um fantoche digno. Suponho que um tributo aos seus esforços seja apropriado. Posso oferecer itens que ajudariam aqueles que exploram a morte e molestam os túmulos de outros." + sInfo + sBare;

				if ( allowed == false )
				{
					sText = "Ora, ora, ora. Vejo que " + yourName + " veio buscar poder no reino dos mortos. Mas, infelizmente, você falhou em promover meus esforços contra os vivos. Siga em frente e retorne quando tiver atendido à minha vontade." + sBare;
				}
			}
			else if ( myName == "Mondain" )
			{
				sTitle = "Os Fragmentos do Tempo";
				sText = "Saudações, " + yourName + ". Vejo que você conseguiu me encontrar aqui. Eu sei o que você está pensando. O Estranho me matou anos atrás. Embora essa possa ser a história contada nas sombras das tavernas, está longe de ser verdade. A gema da imortalidade me salvou do ataque do Estranho, pois a gema estilhaçada liberou um poder que me trouxe de volta à vida pouco tempo depois. Meus esforços foram promovidos por algumas das façanhas que você realizou. Por isso, eu gostaria de dar a você um tributo para ajudá-lo ainda mais em minhas buscas. Esses itens acrescentam grande poder até mesmo aos magos mais novatos." + sInfo + sBare;

				if ( allowed == false )
				{
					sText = "Saudações, " + yourName + ". Vejo que você conseguiu me encontrar aqui. Eu sei o que você está pensando. O Estranho me matou anos atrás. Embora isso possa ser verdade, não é quem eles mataram, mas quando. Eles podem um dia me matar novamente, mas as ondulações do tempo me ajudaram a escapar para este lugar. Se você puder provar que é digno de meu tributo, então vá em frente e traga o caos à terra. Se não, talvez você não deva retornar." + sBare;
				}
			}
			else if ( myName == "Tyball" )
			{
				sTitle = "O Demônio Desatado";
				sText = "Salve, " + yourName + ". Não é frequentemente que encontro alguém que possa enfrentar os salões desta masmorra. Atualmente, estou praticando a arte das reações alquímicas para o controle de demônios. Até agora, tenho tido sucesso, como você vê com meu pequeno amigo ali. Eu acabo saindo de vez em quando, onde tenho que adquirir itens para preparar essas misturas. Embora alguns tentem me impedir, você fez muitas coisas para me ajudar a evitar esses inconvenientes. Por isso, posso oferecer a você um item que o ajudaria a preparar poções tão boas quanto eu." + sInfo + sBare;

				if ( allowed == false )
				{
					sText = "Salve, " + yourName + ". Não é frequentemente que encontro alguém que possa enfrentar os salões desta masmorra. Atualmente, estou praticando a arte das reações alquímicas para o controle de demônios. Até agora, tenho tido sucesso, como você vê com meu pequeno amigo ali. Eu acabo saindo de vez em quando, onde tenho que adquirir itens para preparar essas misturas. Muitos tentam me impedir, e você não fez nada para reduzir esses inconvenientes que frequentemente enfrento. Talvez da próxima vez que nos encontrarmos, você terá histórias daqueles que antes se opunham a mim." + sBare;
				}
			}
			else if ( myName == "Arcadion" )
			{
				sTitle = "Os Fogos do Purgatório";
				sText = "Ora, ora, ora. Se não é a alma de " + yourName + ", talvez se rendendo às minhas correntes? Não importa. Você é mais útil para mim lá fora, pois você entregou muitas almas ao meu covil. Quero que você continue seus esforços, mas sinto que você precisa de um item que ajude em seus caminhos miseráveis." + sInfo + sBare;

				if ( allowed == false )
				{
					sText = "Ora, ora, ora. Se não é a alma de " + yourName + ", talvez se rendendo às minhas correntes? Não importa. Você pode ser mais útil para mim lá fora, pois pode entregar muitas almas ao meu covil. Espalhe o caos por toda a terra e retorne a mim. Há assuntos que podemos discutir mais adiante." + sBare;
				}
			}
			else if ( myName == "Samhayne" )
			{
				sTitle = "A Glória de Poseidon";
				sText = "Saudações, " + yourName + ". Ouvi muito sobre você e suas façanhas por muitas terras. Se você tem interesse em navegar pelos altos mares, estou disposto a oferecer itens que o ajudarão a atravessar as ondas e enfrentar as criaturas das profundezas." + sInfo + sBare;

				if ( allowed == false )
				{
					sText = "Saudações, " + yourName + ". Você planeja navegar pelos altos mares? Se sim, há muita glória a ser ganha. Retorne a mim se tiver histórias de bravura e coragem. Eu gostaria de oferecer algo a tal pessoa." + sBare;
				}
			}
			else if ( myName == "Seggallion" )
			{
				sTitle = "A Vida de um Pirata";
				sText = "Salve, " + yourName + ". Ouvi muitas histórias de suas viagens. Você pega o que quer e não deixa ninguém ficar em seu caminho. Eu gosto disso. Se você tem interesse em navegar pelos altos mares, estou disposto a oferecer itens que o ajudarão a atravessar as ondas e enfrentar as criaturas das profundezas. Há muitos navios por aí que poderiam ser facilmente aliviados de sua carga." + sInfo + sBare;

				if ( allowed == false )
				{
					sText = "Salve, " + yourName + ". Ouvi muito pouco sobre suas viagens. Você deveria sair por aí e pegar o que quer e não deixar ninguém ficar em seu caminho. Talvez se você tiver o que é preciso para extrair as riquezas dos outros, então poderíamos conversar um pouco mais. Eu posso ser capaz de ajudá-lo." + sBare;
				}
			}
			else if ( myName == "Minax" )
			{
				sTitle = "A Mãe do Mal";
				sText = "Parece que " + yourName + " encontrou meu pequeno covil envolto no tempo. Você pode ter ouvido que o Estranho me derrotou anos atrás. Embora eles tenham frustrado meus planos, eles inadvertidamente criaram um paradoxo temporal e permitiram que outra versão de mim continuasse existindo. Isso me traz à sua chegada. Parece que você fez algumas coisas por toda a terra que atrapalharam meus adversários. Por isso, eu ofereço a você algo por suas ações. Esses itens acrescentam grande poder até mesmo aos feiticeiros mais novatos." + sInfo + sBare;

				if ( allowed == false )
				{
					sText = "Parece que " + yourName + " encontrou meu pequeno covil envolto no tempo. Você pode ter ouvido que o Estranho me derrotou anos atrás. Embora eles tenham frustrado meus planos, minha progênie Exodus me trouxe de volta a Sosaria para promover meus esforços. Isso me traz a questionar sua chegada. Se você veio aqui para buscar tributo, é melhor procurar em outro lugar, pois você não fez nada para fazer meus inimigos caírem." + sBare;
				}
			}
			else if ( myName == "Nystal" )
			{
				sTitle = "O Olho Atento";
				sText = "Saudações, " + yourName + " e bem-vindo ao castelo de Lord British. Perdoe meu quarto, mas não tive tempo de limpar, pois tenho estado muito ocupado com meus estudos. Estou sempre procurando ajudar outros a explorar as propriedades da magia, e pelo que ouvi, você é do tipo confiável. Você gostaria talvez de um tributo por suas boas ações? Tenho itens que ajudariam um mago em sua prática de magia." + sInfo + sBare;

				if ( allowed == false )
				{
					sText = "Saudações, " + yourName + " e bem-vindo ao castelo de Lord British. Perdoe meu quarto, mas não tive tempo de limpar, pois tenho estado muito ocupado com meus estudos. Estou sempre procurando ajudar outros a explorar as propriedades da magia, mas não tenho certeza se você é do tipo confiável. Talvez volte mais tarde quando tiver ajudado mais seus concidadãos." + sBare;
				}
			}
			else if ( myName == "Lord British" )
			{
				sTitle = "Cavaleiros de Sosaria";
				sText = "Saudações, " + yourName + ". Suas ações recentes foram notadas por minha corte e eu gostaria de oferecer a você um item que mostraria minha gratidão pessoal. Esses itens abraçam as qualidades da minha cavalaria e sinto que você pode um dia ser um deles." + sInfo + sBare;

				if ( allowed == false )
				{
					sText = "Saudações, " + yourName + ". Devo admitir que não ouvi muito sobre você nestas terras. Talvez se você se aventurar pelo mundo e derrotar o mal, então meus cavaleiros possam notar. Retorne a mim se seu valor superar sua ganância e eu o recompensarei grandemente." + sBare;
				}
			}
			else if ( myName == "Lord Blackthorne" )
			{
				sTitle = "Preço a Pagar";
				sText = "Ora, se não é " + yourName + ". Estou preso nesta garrafa há anos e há muitos que eu gostaria de agradecer pessoalmente, mas não posso fazer muito dentro desta prisão de vidro. Tenho muitos agentes procurando itens para mim, e tenho alguns inimigos que precisam ser eliminados. Você parece ter resolvido algumas dessas questões sem nem mesmo saber. Eu gostaria de dar a você algo que um assassino apreciaria, se seu ofício seguir nessa direção." + sInfo + sBare;

				if ( allowed == false )
				{
					sText = "Ora, se não é " + yourName + ". Estou preso nesta garrafa há anos e há muitos que eu gostaria de agradecer pessoalmente, mas não posso fazer muito dentro desta prisão de vidro. Tenho muitos agentes procurando itens para mim, e tenho alguns inimigos que precisam ser eliminados. Para aqueles que promovem minha causa, eu costumo notar e recompensar adequadamente." + sBare;
				}
			}
			else if ( myName == "Geoffrey" )
			{
				sTitle = "A Arte da Guerra";
				sText = "Saudações, " + yourName + ". Moro no castelo há muitos anos e servi Lord British por ainda mais tempo. A palavra na taverna é que você fez algumas ações realmente boas em suas viagens. Isso é bom de ouvir, pois precisamos de mais campeões no reino. Eu gostaria de oferecer algo por seu serviço. Algo que poderia torná-lo um lutador melhor." + sInfo + sBare;

				if ( allowed == false )
				{
					sText = "Saudações, " + yourName + ". Moro no castelo há muitos anos e servi Lord British por ainda mais tempo. A palavra na taverna é que você ficou na sua e falhou em escolher um lado contra o bem ou o mal. Se seu coração algum dia o levar a fazer o que é certo para o reino, volte e me visite. Posso ter algo para compartilhar com você." + sBare;
				}
			}
			else if ( myName == "Shimazu" )
			{
				sTitle = "O Caminho do Shogun";
				sText = "Salve, " + yourName + ". Você encontrou meu dojo. Isso não me surpreende, pois ouvi sobre suas viagens. Se você busca se tornar mestre do ninjitsu, ou simplesmente um dos samurais, então eu poderia talvez ajudá-lo com alguns itens especiais. O que você diz?" + sInfo + sBare;

				if ( allowed == false )
				{
					sText = "Salve, " + yourName + ". Você encontrou meu dojo, embora eu não saiba como. Este dojo é para aqueles que enfrentaram o perigo e viveram para contar a história, não para aqueles que ficam por aí bebendo hidromel o dia todo. Deixe este lugar, antes que o fedor da covardia atraia outros." + sBare;
				}
			}
			else if ( myName == "Gorn" )
			{
				sTitle = "Força e Aço";
				sText = "Salve, " + yourName + ". Governo estas ilhas há muitos anos, mas nunca ouvi tantas histórias de bravura e coragem quanto as que ouvi sobre você. Se você é um verdadeiro bárbaro, posso fazer algo que o ajudaria a viver da terra. Então você poderia viajar ainda mais, espalhando suas histórias de vitória para outros." + sInfo + sBare;

				if ( allowed == false )
				{
					sText = "Salve, " + yourName + ". Governo estas ilhas há muitos anos, mas nunca ouvi falar de você. Se você quer ser um verdadeiro bárbaro, então precisa viajar para as regiões mais severas do mundo e enfrentar os inimigos mais poderosos. Talvez da próxima vez que eu vê-lo, você será tal alma." + sBare;
				}
			}
			else if ( myName == "Jaana" )
			{
				sTitle = "A Mão Curandeira";
				sText = "Salve, " + yourName + ". Sou uma das muitas curandeiras de Sosaria e decidi morar aqui, pois é central na terra. Não precisei curá-lo, o que é surpreendente, pois ouvi sobre as muitas façanhas que você fez na terra. Se você precisa estar melhor equipado para curar a si mesmo, talvez eu possa ajudar." + sInfo + sBare;

				if ( allowed == false )
				{
					sText = "Salve, " + yourName + ". Sou uma das muitas curandeiras de Sosaria e decidi morar aqui, pois é central na terra. Não precisei curá-lo, mas isso provavelmente é porque você não arrisca sua vida por uma causa. Se você assumir uma verdadeira vida de aventura, visite novamente se precisar estar melhor equipado para curar a si mesmo." + sBare;
				}
			}
			else if ( myName == "Dupre" )
			{
				sTitle = "Cavaleiros Contra o Mal";
				sText = "Saudações, " + yourName + ". Eu sou Dupre e tenho viajado por muitas terras, derrotando o mal em todas as suas formas. Ouvi sobre sua valentia em batalha e sua pureza de espírito. Você deveria talvez abraçar a vida da cavalaria e se juntar à batalha contra os mortos-vivos. Se você tem o objetivo de trazer luz às trevas, basta dizer e outro campeão se juntará às nossas fileiras." + sInfo + sBare;

				if ( allowed == false )
				{
					sText = "Saudações, " + yourName + ". Eu sou Dupre e tenho viajado por muitas terras, derrotando o mal em todas as suas formas. Você deveria talvez abraçar a vida da cavalaria e se juntar à batalha contra os mortos-vivos. No entanto, vou manter meus ouvidos atentos sobre você. Se você puder me mostrar seu valor contra os mortos perversos do mundo, ficarei feliz em chamá-lo de amigo." + sBare;
				}
			}
			else if ( myName == "Gwenno" )
			{
				sTitle = "Música para Meus Ouvidos";
				sText = "Bom encontro, " + yourName + ". Eu aprecio minha música e histórias de aventura, e cantei muitas canções sobre você. Se você talvez busca ajudar outros com o dom da música, eu poderia talvez ajudá-lo." + sInfo + sBare;

				if ( allowed == false )
				{
					sText = "Bom encontro, " + yourName + ". Eu aprecio minha música e histórias de aventura, mas infelizmente não tenho canções sobre sua vida. Talvez você deva retornar quando tiver histórias de glória e eu poderia então cantar sobre sua valentia para outros." + sBare;
				}
			}
			else if ( myName == "Iolo" )
			{
				sTitle = "O Verdadeiro Arqueiro";
				sText = "Bom encontro, " + yourName + ". Sou um bardo à noite, mas um bom arqueiro durante o dia. Faço muitos arcos para Lord British, pois ele aprecia a qualidade, mas é tudo que pareço ter tempo para fazer em arcoaria. Ele me contou muitas histórias sobre suas conquistas de valor e justiça. Estou disposto a ajudá-lo, pois posso dar a você muitos itens que ajudariam muito um arqueiro." + sInfo + sBare;

				if ( allowed == false )
				{
					sText = "Bom encontro, " + yourName + ". Sou um bardo à noite, mas um bom arqueiro durante o dia. Faço muitos arcos para Lord British, pois ele aprecia a qualidade, mas é tudo que pareço ter tempo para fazer em arcoaria. Se você algum dia fizer um nome para si mesmo, volte e me visite. Posso ser capaz de torná-lo um arqueiro melhor." + sBare;
				}
			}
			else if ( myName == "Shamino" )
			{
				sTitle = "As Florestas Maravilhosas";
				sText = "Bom encontro, " + yourName + ". Sou um simples lenhador, mas ouvi muitas histórias de valentia sobre você. Gostaria de poder ajudar em suas buscas, mas tudo que posso oferecer são itens que o ajudariam no ofício da carpintaria. Não me entenda mal, eles podem fazer muito mais do que isso. São apenas itens que o ajudariam a viajar pelas florestas com relativa segurança, com certeza. Se isso lhe interessa, me avise." + sInfo + sBare;

				if ( allowed == false )
				{
					sText = "Bom encontro, " + yourName + ". Sou um simples lenhador e tenho vivido em Montor por muitos anos. Geralmente gosto de ajudar prospectivos lenhadores, mas geralmente ajudo aqueles que foram experientes nos caminhos do valor e da bondade. Bem, cuide-se então." + sBare;
				}
			}
			else if ( myName == "Stefano" )
			{
				sTitle = "O Que É Seu É Meu";
				sText = "Ora, ora, ora. Você deve ser " + yourName + ". Enquanto outros bebem seu hidromel, eu os ouço contar histórias de algumas coisas bastante duras que você fez no reino. Devo dizer que estou bastante impressionado. Se você pudesse abrir mão de algum ouro, eu poderia fazer um item para você que o ajudaria a adquirir coisas mais facilmente do que simplesmente comprá-las. Você está interessado?" + sInfo + sBare;

				if ( allowed == false )
				{
					sText = "Então você diz que se chama " + yourName + "? Nunca ouvi falar de você. Talvez vá lá fora e roube algo que faça toda a terra sussurrar histórias do roubo. Se você puder fazer isso, então talvez possamos conversar. Caso contrário, suma daqui!" + sBare;
				}
			}
			else if ( myName == "Katrina" )
			{
				sTitle = "Amigos da Floresta";
				sText = "Olá, " + yourName + ". Tenho domesticado animais aqui por muitos anos, mas não alcancei a glória que seu nome trouxe. Se você está disposto a aprender os caminhos da domesticação, eu poderia talvez fazer algo para ajudá-lo com esse objetivo. Pode ser uma longa jornada, mas pode valer a pena mais tarde se você não quiser viajar sozinho." + sInfo + sBare;

				if ( allowed == false )
				{
					sText = "Olá, " + yourName + ". Tenho domesticado animais aqui por muitos anos, mas não ouvi falar de você. Volte e me visite se algum dia tiver histórias para contar sobre grandes batalhas ou males derrotados. Adoraria ouvi-las." + sBare;
				}
			}
			else if ( myName == "the Guardian" )
			{
				sTitle = "O Portão Negro";
				sText = "Então você deve ser " + yourName + ". Tenho observado sua jornada por bastante tempo. Estou preso aqui em Sosaria, procurando uma maneira de chegar ao mundo de Pagan. Dupre e Lord British têm sido um espinho em meu lado, mas tenho muitos discípulos ansiosos para procurar o que preciso ou matar quem quero eliminar. Você fez muito para garantir meu sucesso nesse assunto, pois estou cada vez mais perto de fazer meu portão negro me levar para onde busco. Deixe-me melhorar seus esforços, pois posso conjurar muitos itens que poderiam ser de assistência para você." + sInfo + sBare;

				if ( allowed == false )
				{
					sText = "Então você diz que se chama " + yourName + "? Não ouvi falar de você, mas estou preso aqui em Sosaria procurando uma maneira de chegar ao mundo de Pagan. Dupre e Lord British têm sido um espinho em meu lado, mas tenho muitos discípulos ansiosos para procurar o que preciso ou matar quem quero eliminar. Talvez você possa se aventurar e ajudar aqueles que me adoram, ou simplesmente criar caos para manter Dupre e Lord British ocupados. Retorne a mim quando tiver me mostrado que merece minha atenção." + sBare;
				}
			}
			else if ( myName == "Garamon" )
			{
				sTitle = "Um Irmão Não Mais";
				sText = "Saudações, " + yourName + ". Estou muito feliz em vê-lo, pois ouvi muitas histórias de suas façanhas. Quer você saiba ou não, seus esforços atrasaram consideravelmente meu irmão Tyball. Sua tentativa de escravizar demônios só fez com que mais aparecessem na terra e tenho tentado criar elixires para ajudar outros na luta contra tal mal. Eu gostaria de oferecer a você tributo, pois tenho muitos itens que o ajudariam a preparar poções como eu faço." + sInfo + sBare;

				if ( allowed == false )
				{
					sText = "Saudações, " + yourName + ". Estou feliz em conhecê-lo, pois sinto que você poderia talvez me ajudar contra as práticas insanas que meu irmão Tyball realiza. Sua tentativa de escravizar demônios só fez com que mais aparecessem na terra e tenho tentado criar elixires para ajudar outros na luta contra tal mal. Há outros itens e agentes que ele busca para promover sua causa. Retorne a mim quando tiver alcançado glória por toda a terra. Sinto que poderia dar algo para você." + sBare;
				}
			}
			else if ( myName == "Mors Gotha" )
			{
				sTitle = "Morte aos Justos";
				sText = "Bom encontro, " + yourName + ". A luz deste mundo eventualmente será derrotada devido às suas ações recentes. Pessoas como você são bastante raras. Raras o suficiente para que você não devesse causar estragos com o que tem. Que tal eu dar a você algo para ajudá-lo... a espalhar a palavra da morte? Infelizmente, só tenho itens que realmente poderiam ajudar um cavaleiro da morte, mas cavaleiros da morte são realmente raros." + sInfo + sBare;

				if ( allowed == false )
				{
					sText = "O que você quer, " + yourName + "? Não precisamos de gente como você vagando por Umbra! Talvez você deva ir para a superfície e nos mostrar que é digno da escuridão!" + sBare;
				}
			}
			else if ( myName == "Lethe" )
			{
				sTitle = "A Sepultura Rasa";
				sText = "Ora, ora, ora. Vejo que " + yourName + " veio buscar conhecimento nos locais de descanso dos mortos. Você tem estado bastante ocupado, auxiliando meus planos como um servo digno. Suponho que um tributo aos seus esforços seja apropriado. Posso oferecer itens que ajudariam aqueles que exploram a morte e molestam os túmulos de outros." + sInfo + sBare;

				if ( allowed == false )
				{
					sText = "Ora, ora, ora. Vejo que " + yourName + " veio buscar conhecimento nos locais de descanso dos mortos. Mas, infelizmente, você falhou em promover meus esforços contra os vivos. Siga em frente e retorne quando tiver eliminado aqueles que se opõem a mim." + sBare;
				}
			}

            this.Closable=true;
			this.Disposable=true;
			this.Dragable=true;
			this.Resizable=false;

			AddPage(0);

			string color = "#d5a496";

			AddImage(0, 2, 9543, Server.Misc.PlayerSettings.GetGumpHue( listener ));
			AddHtml( 12, 15, 341, 20, @"<BODY><BASEFONT Color=" + color + ">" + sTitle + "</BASEFONT></BODY>", (bool)false, (bool)false);
			AddHtml( 12, 50, 380, 253, @"<BODY><BASEFONT Color=" + color + ">" + sText + "</BASEFONT></BODY>", (bool)false, (bool)true);
			AddButton(367, 12, 4017, 4017, 0, GumpButtonType.Reply, 0);
        }

		public override void OnResponse( NetState sender, RelayInfo info )
		{
			Mobile from = sender.Mobile;
			from.SendSound( 0x4A ); 
		}
    }
}