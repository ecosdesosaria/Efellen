using System;
using Server; 
using System.Collections;
using Server.ContextMenus;
using System.Collections.Generic;
using Server.Misc;
using Server.Network;
using Server.Items;
using Server.Gumps;
using Server.Mobiles;
using Server.Commands;
using System.Globalization;
using Server.Regions;
using Server.Multis;

namespace Server.Items
{
	public class BagOfTricks : Item
	{
		public override bool DisplayWeight { get { return false; } }

		public int PrankPoints;

		[CommandProperty(AccessLevel.Owner)]
		public int Prank_Points { get { return PrankPoints; } set { PrankPoints = value; InvalidateProperties(); } }

		[Constructable]
		public BagOfTricks() : base( 0x1E3F )
		{
			Weight = 1.0;
			Name = "bag of tricks";
			Hue = Utility.RandomColor(0);
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( !IsChildOf( from.Backpack ) ) 
			{
				from.SendMessage( "This must be in your backpack to use." );
				return;
			}
			else
			{
				from.CloseGump( typeof( BagOfTricksGump ) );
				from.SendGump( new BagOfTricksGump( from ) );
				from.PlaySound( 0x48 );
			}
		}

        public override void AddNameProperties(ObjectPropertyList list)
		{
            base.AddNameProperties(list);
            if ( PrankPoints > 0 ){ list.Add( 1070722, "" + PrankPoints + " Prank Points"); }
        } 

		public static int GetPranks( Mobile m )
		{
			int pranks = 0;

			foreach( Item i in m.Backpack.FindItemsByType( typeof( BagOfTricks ), true ) )
			{
				BagOfTricks tricks = (BagOfTricks)i;
				pranks = pranks + tricks.PrankPoints;
			}

			return pranks;
		}

		public static void UsePranks( Mobile m, int pranks )
		{
			ArrayList tricks = new ArrayList();
			foreach( Item item in m.Backpack.FindItemsByType( typeof( BagOfTricks ), true ) )
			{
				tricks.Add( item );
			}
			for ( int i = 0; i < tricks.Count; ++i )
			{
				BagOfTricks bag = (BagOfTricks)tricks[ i ];

				if ( pranks > 0 )
				{
					if ( bag.PrankPoints >= pranks ){ bag.PrankPoints = bag.PrankPoints - pranks; pranks = 0; bag.InvalidateProperties(); }
					else if ( pranks > bag.PrankPoints ){ pranks = pranks - bag.PrankPoints; bag.PrankPoints = 0; bag.InvalidateProperties(); }
				}
			}
		}

		public BagOfTricks( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int)1 ); // version
            writer.Write( PrankPoints );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
            PrankPoints = reader.ReadInt();
		}

        public static void InvokeCommand( string c, Mobile from )
        {
            CommandSystem.Handle(from, String.Format("{0}{1}", CommandSystem.Prefix, c));
        }

		public static void DoPrank( Mobile from, int skill )
		{
			if ( skill == 260 ){ InvokeCommand( "CanOfSnakes", from ); }
			else if ( skill == 261 ){ InvokeCommand( "Clowns", from ); }
			else if ( skill == 262 ){ InvokeCommand( "FlowerPower", from ); }
			else if ( skill == 263 ){ InvokeCommand( "Hilarity", from ); }
			else if ( skill == 264 ){ InvokeCommand( "Insult", from ); }
			else if ( skill == 265 ){ InvokeCommand( "JumpAround", from ); }
			else if ( skill == 266 ){ InvokeCommand( "PoppingBalloon", from ); }
			else if ( skill == 267 ){ InvokeCommand( "RabbitInAHat", from ); }
			else if ( skill == 268 ){ InvokeCommand( "SeltzerBottle", from ); }
			else if ( skill == 269 ){ InvokeCommand( "SurpriseGift", from ); }
		}

		public static string JesterSpeech()
		{
			return "Bobos da corte são os entertainers da terra, fazendo malabarismos e pulando por aí, espalhando histórias humorísticas e tentando fazer outros rirem. Eles imploram por risadas e podem usar psicologia em seu público para serem mais eficazes no show. Bobos da corte também têm um lado travesso, o que os torna aventureiros capazes à sua própria maneira.<br><br>Se você quer se tornar um bobo da corte, precisará aprimorar suas habilidades em mendigar e na psicologia dos outros. Você também precisará conseguir uma bolsa de truques e algumas roupas de bobo para preencher o papel. Sem essas coisas, você não será capaz de realizar as artes cômicas. A maioria dos bobos pertence à Guilda de Ladrões, pois às vezes se envolvem em esconder, furtividade e prestidigitação. Você, no entanto, pode seguir qualquer carreira secundária que desejar.<br><br>Há alguns bobos pela terra que podem vender tais itens, mas o bobo da corte real não. Esses bobos podem ser encontrados em tavernas, navios, ou passando tempo com atores. Como já mencionado, você deve vestir pelo menos uma peça de roupa de bobo. Isso pode ser um chapéu de bobo, um traje de bobo, ou até sapatos de bobo. Uma bolsa de truques terá dez truques individuais que você pode realizar desde que tenha a mana e os pontos de travessura para executá-los. Você não precisa de muita habilidade para realizar esses truques, mas eles serão de pouca utilidade se você não praticar ambas as habilidades mencionadas.<br><br>Um bobo da corte precisa de ouro para comprar travessuras para encher suas bolsas, e só pode enchê-las até 50.000 pontos de travessura antes que esteja cheia. Para adicionar mais ouro à bolsa, você deve estar próximo a um bobo local ou até mesmo ao bobo real. Arraste o ouro para a bolsa para enchê-la. Os pontos diminuirão à medida que você usar seus truques.<br><br>Clique duas vezes na bolsa para abri-la e aprender sobre os truques que você pode fazer. Nesta primeira visualização, você pode selecionar um ícone de habilidade para executar a habilidade, ou pode clicar no botão ao lado de cada ícone para aprender sobre a habilidade com mais detalhes. Há também opções para abrir barras rápidas para essas habilidades, e elas vêm em dois layouts diferentes com dois tamanhos diferentes.<br><br>Para ter um título compatível com sua profissão escolhida, você pode definir seu título de habilidade para 'Mendigo', o que lhe dará o título de 'Bobo da Corte'. Você também pode definir seu título de habilidade para 'Psicologia', o que lhe dará o título de 'Palhaço'.<br><br>Se isso também lhe interessa, bobos da corte podem ter tendas de circo à venda. Estas exigem um lote de terra já colocado, e então você pode colocar a tenda no lote. Estas são diferentes de tendas regulares porque têm duas cores diferentes ao mesmo tempo. Então, se você tem uma tenda de circo, pode tingi-la de uma cor (vermelho, por exemplo). Então você pode clicar uma vez na tenda e selecionar a opção 'Próximo' para alternar para a cor secundária. Então você pode tingir a tenda de uma cor diferente (azul, por exemplo). Quando você colocar a tenda, ela ficará nas cores vermelho e azul. Se você quiser inverter o padrão, corte a tenda com um machado e use a opção 'Próximo' na tenda para trocar as cores. Então você pode construir a tenda em seu lote novamente.<br><br>Luvas de arremesso são a arma preferida dos bobos, mas você pode usar qualquer arma que desejar. No entanto, se você usar luvas de arremesso, terá opções adicionais para coisas para jogar em seus inimigos. Em vez de apenas pedras, dardos ou facas... você poderá jogar tomates ou cartas de baralho. Você só precisaria configurar suas luvas de arremesso para usar esse objeto específico.<br><br>Há alguns comandos que você pode digitar para ativar as habilidades de bobo: <br><br>[CanOfSnakes <br><br>[Clowns <br><br>[FlowerPower <br><br>[Hilarity <br><br>[Insult <br><br>[JumpAround <br><br>[PoppingBalloon <br><br>[RabbitInAHat <br><br>[SeltzerBottle <br><br>[SurpriseGift <br><br>";
		}

		public static string JokeInfo( int ability, string type )
		{
			string str = "";

			if ( ability == 20749 || ability == ( 20749 + 10 ) ){ if ( type == "name" ){ 		str = "Lata de Cobras"; } else if ( type == "points" ){ 	str = "200"; } else if ( type == "mana" ){ 	str = "40"; } else { 
				str = "Abrir uma lata de nozes simplesmente surpreenderá a plateia quando cobras forem liberadas. Essas cobras seguirão seus comandos por um curto período. Quanto melhores suas habilidades de bobo, mais tempo elas permanecerão e mais fortes serão. Seu veneno também será igualmente forte.";
			}}
			else if ( ability == 20751 || ability == ( 20751 + 10 ) ){ if ( type == "name" ){ 	str = "Palhaços"; } else if ( type == "points" ){ 		str = "50"; } else if ( type == "mana" ){ 	str = "25"; } else { 
				str = "Engane sua plateia criando ilusões de si mesmo, o que pode ajudá-lo a escapar sorrateiramente ou distrair alguém de você. Suas ilusões variarão em duração e quantidade, dependendo de suas habilidades de bobo.";
			}}
			else if ( ability == 20748 || ability == ( 20748 + 10 ) ){ if ( type == "name" ){ 	str = "Poder da Flor"; } else if ( type == "points" ){ 	str = "50"; } else if ( type == "mana" ){ 	str = "20"; } else { 
				str = "Pode ser uma flor comum, mas cheirar esta flor simplesmente o cobrirá de lodo ácido. A eficácia do lodo depende de suas habilidades de bobo, e pode ter o potencial de cobrir o chão com respingos irritantes.";
			}}
			else if ( ability == 20750 || ability == ( 20750 + 10 ) ){ if ( type == "name" ){ 	str = "Hilaridade"; } else if ( type == "points" ){ 		str = "40"; } else if ( type == "mana" ){ 	str = "50"; } else { 
				str = "Mantenha sua plateia em gargalhadas! Conte uma piada e veja se os outros congelam de rir. A duração da risada é baseada em suas habilidades de bobo e na dificuldade da plateia. O alcance em que sua piada é ouvida também depende de suas habilidades de bobo.";
			}}
			else if ( ability == 20747 || ability == ( 20747 + 10 ) ){ if ( type == "name" ){ 	str = "Insulto"; } else if ( type == "points" ){ 		str = "60"; } else if ( type == "mana" ){ 	str = "60"; } else { 
				str = "Prepare-se para dar um insulto afiado! Embora suas palavras não os façam escorregar em suas lágrimas, isso os desmoralizará a ponto de sua mana começar a desaparecer enquanto refletem sobre seus sentimentos feridos. A duração do declínio, bem como quanta mana é perdida a cada segundo, depende de suas habilidades de bobo.";
			}}
			else if ( ability == 20754 || ability == ( 20754 + 10 ) ){ if ( type == "name" ){ 	str = "Pular Por Aí"; } else if ( type == "points" ){ 	str = "20"; } else if ( type == "mana" ){ 	str = "20"; } else { 
				str = "Isso permite que você demonstre suas habilidades acrobáticas, pois pode pular e saltar rapidamente, talvez evitando perigos ou chegando a lugares de difícil acesso.";
			}}
			else if ( ability == 20746 || ability == ( 20746 + 10 ) ){ if ( type == "name" ){ 	str = "Balão Estourado"; } else if ( type == "points" ){ str = "100"; } else if ( type == "mana" ){ 	str = "20"; } else { 
				str = "Todos gostam de balões, até decidirem brincar com um dos seus. Esses balões flutuarão em direção aos seus inimigos, onde serão facilmente estourados e causarão uma força explosiva física. A explosão é equivalente às suas habilidades de bobo, e o alcance da explosão também aumentará com aqueles habilidosos como bobos.";
			}}
			else if ( ability == 20753 || ability == ( 20753 + 10 ) ){ if ( type == "name" ){ 	str = "Coelho na Cartola"; } else if ( type == "points" ){str = "150"; } else if ( type == "mana" ){ 	str = "30"; } else { 
				str = "Alakazam! Puxe coelhos de uma cartola para deslumbrar sua plateia, embora acariciá-los possa ser imprudente. Dizem ser os filhotes do coelho assassino de Caerbannog, essas criaturas seguirão seus comandos por um curto período e atacarão ferozmente aqueles que você soltar sobre eles. A força dos coelhos e o tempo que permanecem dependem de suas habilidades de bobo.";
			}}
			else if ( ability == 20755 || ability == ( 20755 + 10 ) ){ if ( type == "name" ){ 	str = "Garrafa de Seltzer"; } else if ( type == "points" ){ str = "50"; } else if ( type == "mana" ){ 	str = "20"; } else { 
				str = "Ofereça uma bebida à sua plateia, e eles provavelmente não pedirão novamente. Isso pulverizará um alvo com água congelante, onde a eficácia depende de suas habilidades de bobo, e pode ter o potencial de cobrir o chão com água gelada.";
			}}
			else if ( ability == 20752 || ability == ( 20752 + 10 ) ){ if ( type == "name" ){ 	str = "Presente Surpresa"; } else if ( type == "points" ){ 	str = "80"; } else if ( type == "mana" ){ 	str = "20"; } else { 
				str = "Surpreenda sua plateia com um presente que nunca esquecerão. Esses presentes são colocados no chão onde inimigos próximos podem ficar intrigados o suficiente para abri-lo. Eles serão surpreendidos por uma bela explosão flamejante. A explosão é equivalente às suas habilidades de bobo, e o alcance da explosão também aumentará com aqueles habilidosos como bobos.";
			}}

			return str;
		}

		public class BagOfTricksGump : Gump
		{
			public BagOfTricksGump( Mobile from ): base( 50, 50 )
			{
				string color = "#b3706f";
				from.SendSound( 0x4A );

				this.Closable=true;
				this.Disposable=true;
				this.Dragable=true;
				this.Resizable=false;

				AddPage(0);

				AddImage(0, 0, 7033, Server.Misc.PlayerSettings.GetGumpHue( from ));
				AddButton(863, 11, 4017, 4017, 0, GumpButtonType.Reply, 0);
				AddHtml( 12, 12, 727, 20, @"<BODY><BASEFONT Color=" + color + ">BAG OF TRICKS</BASEFONT></BODY>", (bool)false, (bool)false);
				AddHtml( 12, 345, 880, 249, @"<BODY><BASEFONT Color=" + color + ">" + JesterSpeech() + "</BASEFONT></BODY>", (bool)false, (bool)false);

				int o = -78;
				int k = 56;
				int x = 59;
				int y = 79;
				int b = 55;
				int i = 0;

				i = 20749;
				AddButton(110+o, k, i, i, 260, GumpButtonType.Reply, 0);
				AddHtml( 165+o, x, 172, 20, @"<BODY><BASEFONT Color=" + color + ">" + JokeInfo( i, "name" ) + "</BASEFONT></BODY>", (bool)false, (bool)false);
				AddButton(165+o, y, 4011, 4011, i, GumpButtonType.Reply, 0);
				k=k+b; x=x+b; y=y+b;

				i = 20751;
				AddButton(110+o, k, i, i, 261, GumpButtonType.Reply, 0);
				AddHtml( 165+o, x, 172, 20, @"<BODY><BASEFONT Color=" + color + ">" + JokeInfo( i, "name" ) + "</BASEFONT></BODY>", (bool)false, (bool)false);
				AddButton(165+o, y, 4011, 4011, i, GumpButtonType.Reply, 0);
				k=k+b; x=x+b; y=y+b;

				i = 20748;
				AddButton(110+o, k, i, i, 262, GumpButtonType.Reply, 0);
				AddHtml( 165+o, x, 172, 20, @"<BODY><BASEFONT Color=" + color + ">" + JokeInfo( i, "name" ) + "</BASEFONT></BODY>", (bool)false, (bool)false);
				AddButton(165+o, y, 4011, 4011, i, GumpButtonType.Reply, 0);
				k=k+b; x=x+b; y=y+b;

				i = 20750;
				AddButton(110+o, k, i, i, 263, GumpButtonType.Reply, 0);
				AddHtml( 165+o, x, 172, 20, @"<BODY><BASEFONT Color=" + color + ">" + JokeInfo( i, "name" ) + "</BASEFONT></BODY>", (bool)false, (bool)false);
				AddButton(165+o, y, 4011, 4011, i, GumpButtonType.Reply, 0);
				k=k+b; x=x+b; y=y+b;

				i = 20747;
				AddButton(110+o, k, i, i, 264, GumpButtonType.Reply, 0);
				AddHtml( 165+o, x, 172, 20, @"<BODY><BASEFONT Color=" + color + ">" + JokeInfo( i, "name" ) + "</BASEFONT></BODY>", (bool)false, (bool)false);
				AddButton(165+o, y, 4011, 4011, i, GumpButtonType.Reply, 0);
				k=56; x=59; y=79;

				i = 20754;
				AddButton(390+o, k, i, i, 265, GumpButtonType.Reply, 0);
				AddHtml( 445+o, x, 172, 20, @"<BODY><BASEFONT Color=" + color + ">" + JokeInfo( i, "name" ) + "</BASEFONT></BODY>", (bool)false, (bool)false);
				AddButton(445+o, y, 4011, 4011, i, GumpButtonType.Reply, 0);
				k=k+b; x=x+b; y=y+b;

				i = 20746;
				AddButton(390+o, k, i, i, 266, GumpButtonType.Reply, 0);
				AddHtml( 445+o, x, 172, 20, @"<BODY><BASEFONT Color=" + color + ">" + JokeInfo( i, "name" ) + "</BASEFONT></BODY>", (bool)false, (bool)false);
				AddButton(445+o, y, 4011, 4011, i, GumpButtonType.Reply, 0);
				k=k+b; x=x+b; y=y+b;

				i = 20753;
				AddButton(390+o, k, i, i, 267, GumpButtonType.Reply, 0);
				AddHtml( 445+o, x, 172, 20, @"<BODY><BASEFONT Color=" + color + ">" + JokeInfo( i, "name" ) + "</BASEFONT></BODY>", (bool)false, (bool)false);
				AddButton(445+o, y, 4011, 4011, i, GumpButtonType.Reply, 0);
				k=k+b; x=x+b; y=y+b;

				i = 20755;
				AddButton(390+o, k, i, i, 268, GumpButtonType.Reply, 0);
				AddHtml( 445+o, x, 172, 20, @"<BODY><BASEFONT Color=" + color + ">" + JokeInfo( i, "name" ) + "</BASEFONT></BODY>", (bool)false, (bool)false);
				AddButton(445+o, y, 4011, 4011, i, GumpButtonType.Reply, 0);
				k=k+b; x=x+b; y=y+b;

				i = 20752;
				AddButton(390+o, k, i, i, 269, GumpButtonType.Reply, 0);
				AddHtml( 445+o, x, 172, 20, @"<BODY><BASEFONT Color=" + color + ">" + JokeInfo( i, "name" ) + "</BASEFONT></BODY>", (bool)false, (bool)false);
				AddButton(445+o, y, 4011, 4011, i, GumpButtonType.Reply, 0);
				k=k+b; x=x+b; y=y+b;

				int v = -261;
				AddButton(665, 330+v, 4005, 4005, 10, GumpButtonType.Reply, 0);
				AddHtml( 705, 330+v, 106, 20, @"<BODY><BASEFONT Color=" + color + ">Large Column</BASEFONT></BODY>", (bool)false, (bool)false);
				AddButton(665, 360+v, 4005, 4005, 20, GumpButtonType.Reply, 0);
				AddHtml( 705, 360+v, 106, 20, @"<BODY><BASEFONT Color=" + color + ">Large Row</BASEFONT></BODY>", (bool)false, (bool)false);
				AddButton(665, 390+v, 4005, 4005, 30, GumpButtonType.Reply, 0);
				AddHtml( 705, 390+v, 106, 20, @"<BODY><BASEFONT Color=" + color + ">Small Column</BASEFONT></BODY>", (bool)false, (bool)false);
				AddButton(665, 420+v, 4005, 4005, 40, GumpButtonType.Reply, 0);
				AddHtml( 705, 420+v, 106, 20, @"<BODY><BASEFONT Color=" + color + ">Small Row</BASEFONT></BODY>", (bool)false, (bool)false);
				AddButton(665, 450+v, 4017, 4017, 50, GumpButtonType.Reply, 0);
				AddHtml( 705, 450+v, 106, 20, @"<BODY><BASEFONT Color=" + color + ">Close All</BASEFONT></BODY>", (bool)false, (bool)false);
				AddButton(665, 480+v, 4020, 4020, 60, GumpButtonType.Reply, 0);
				AddHtml( 705, 480+v, 106, 20, @"<BODY><BASEFONT Color=" + color + ">Cancel</BASEFONT></BODY>", (bool)false, (bool)false);
			}

			public override void OnResponse( NetState state, RelayInfo info ) 
			{
				Mobile from = state.Mobile; 

				if ( info.ButtonID > 0 && info.ButtonID < 60 )
				{
					from.CloseGump( typeof( TricksLargeRow ) );
					from.CloseGump( typeof( TricksLargeColumn ) );
					from.CloseGump( typeof( TricksSmallRow ) );
					from.CloseGump( typeof( TricksSmallColumn ) );
				}

				if ( info.ButtonID > 20000 ){ from.SendGump( new InfoJester( info.ButtonID, from ) ); }
				else if ( info.ButtonID > 200 ){ DoPrank( from, info.ButtonID ); }
				else if ( info.ButtonID == 10 ){ from.SendGump( new TricksLargeColumn( from ) ); }
				else if ( info.ButtonID == 20 ){ from.SendGump( new TricksLargeRow( from ) ); }
				else if ( info.ButtonID == 30 ){ from.SendGump( new TricksSmallColumn( from ) ); }
				else if ( info.ButtonID == 40 ){ from.SendGump( new TricksSmallRow( from ) ); }

				if ( info.ButtonID > 20000 ){ from.SendSound( 0x4A ); }
				else { from.PlaySound( 0x48 ); }
			}
		}

		public class TricksLargeRow : Gump
		{
			public TricksLargeRow( Mobile from ): base( 50, 50 )
			{
				this.Closable=false;
				this.Disposable=true;
				this.Dragable=true;
				this.Resizable=false;

				AddPage(0);
				AddImage(0, 0, 10864);
				AddButton(77, 0, 20749, 20749, 260, GumpButtonType.Reply, 0);
				AddButton(127, 0, 20751, 20751, 261, GumpButtonType.Reply, 0);
				AddButton(177, 0, 20748, 20748, 262, GumpButtonType.Reply, 0);
				AddButton(227, 0, 20750, 20750, 263, GumpButtonType.Reply, 0);
				AddButton(277, 0, 20747, 20747, 264, GumpButtonType.Reply, 0);
				AddButton(327, 0, 20754, 20754, 265, GumpButtonType.Reply, 0);
				AddButton(377, 0, 20746, 20746, 266, GumpButtonType.Reply, 0);
				AddButton(427, 0, 20753, 20753, 267, GumpButtonType.Reply, 0);
				AddButton(477, 0, 20755, 20755, 268, GumpButtonType.Reply, 0);
				AddButton(527, 0, 20752, 20752, 269, GumpButtonType.Reply, 0);
			}

			public override void OnResponse( NetState state, RelayInfo info ) 
			{
				Mobile from = state.Mobile; 
				DoPrank( from, info.ButtonID );
				from.CloseGump( typeof( TricksLargeRow ) );
				if ( Server.Misc.GetPlayerInfo.isJester ( from ) )
				{
					from.SendGump( new TricksLargeRow( from ) );
				}
			}
		}

		public class TricksLargeColumn : Gump
		{
			public TricksLargeColumn( Mobile from ): base( 50, 50 )
			{
				this.Closable=false;
				this.Disposable=true;
				this.Dragable=true;
				this.Resizable=false;

				AddPage(0);
				AddImage(0, 0, 10864);
				AddButton(15, 53, 20749, 20749, 260, GumpButtonType.Reply, 0);
				AddButton(15, 103, 20751, 20751, 261, GumpButtonType.Reply, 0);
				AddButton(15, 153, 20748, 20748, 262, GumpButtonType.Reply, 0);
				AddButton(15, 203, 20750, 20750, 263, GumpButtonType.Reply, 0);
				AddButton(15, 253, 20747, 20747, 264, GumpButtonType.Reply, 0);
				AddButton(15, 303, 20754, 20754, 265, GumpButtonType.Reply, 0);
				AddButton(15, 353, 20746, 20746, 266, GumpButtonType.Reply, 0);
				AddButton(15, 403, 20753, 20753, 267, GumpButtonType.Reply, 0);
				AddButton(15, 453, 20755, 20755, 268, GumpButtonType.Reply, 0);
				AddButton(15, 503, 20752, 20752, 269, GumpButtonType.Reply, 0);
			}

			public override void OnResponse( NetState state, RelayInfo info ) 
			{
				Mobile from = state.Mobile; 
				DoPrank( from, info.ButtonID );
				from.CloseGump( typeof( TricksLargeColumn ) );
				if ( Server.Misc.GetPlayerInfo.isJester ( from ) )
				{
					from.SendGump( new TricksLargeColumn( from ) );
				}
			}
		}

		public class TricksSmallRow : Gump
		{
			public TricksSmallRow( Mobile from ): base( 50, 50 )
			{
				this.Closable=false;
				this.Disposable=true;
				this.Dragable=true;
				this.Resizable=false;

				AddPage(0);
				AddImage(0, 0, 10865);
				AddButton(43, 0, 20759, 20759, 260, GumpButtonType.Reply, 0);
				AddButton(76, 0, 20761, 20761, 261, GumpButtonType.Reply, 0);
				AddButton(109, 0, 20758, 20758, 262, GumpButtonType.Reply, 0);
				AddButton(142, 0, 20760, 20760, 263, GumpButtonType.Reply, 0);
				AddButton(175, 0, 20757, 20757, 264, GumpButtonType.Reply, 0);
				AddButton(208, 0, 20764, 20764, 265, GumpButtonType.Reply, 0);
				AddButton(241, 0, 20756, 20756, 266, GumpButtonType.Reply, 0);
				AddButton(274, 0, 20763, 20763, 267, GumpButtonType.Reply, 0);
				AddButton(307, 0, 20765, 20765, 268, GumpButtonType.Reply, 0);
				AddButton(340, 0, 20762, 20762, 269, GumpButtonType.Reply, 0);
			}

			public override void OnResponse( NetState state, RelayInfo info ) 
			{
				Mobile from = state.Mobile; 
				DoPrank( from, info.ButtonID );
				from.CloseGump( typeof( TricksSmallRow ) );
				if ( Server.Misc.GetPlayerInfo.isJester ( from ) )
				{
					from.SendGump( new TricksSmallRow( from ) );
				}
			}
		}

		public class TricksSmallColumn : Gump
		{
			public TricksSmallColumn( Mobile from ): base( 50, 50 )
			{
				this.Closable=false;
				this.Disposable=true;
				this.Dragable=true;
				this.Resizable=false;

				AddPage(0);
				AddImage(0, 0, 10865);
				AddButton(7, 30, 20759, 20759, 260, GumpButtonType.Reply, 0);
				AddButton(7, 63, 20761, 20761, 261, GumpButtonType.Reply, 0);
				AddButton(7, 96, 20758, 20758, 262, GumpButtonType.Reply, 0);
				AddButton(7, 129, 20760, 20760, 263, GumpButtonType.Reply, 0);
				AddButton(7, 162, 20757, 20757, 264, GumpButtonType.Reply, 0);
				AddButton(7, 195, 20764, 20764, 265, GumpButtonType.Reply, 0);
				AddButton(7, 228, 20756, 20756, 266, GumpButtonType.Reply, 0);
				AddButton(7, 261, 20763, 20763, 267, GumpButtonType.Reply, 0);
				AddButton(7, 294, 20765, 20765, 268, GumpButtonType.Reply, 0);
				AddButton(7, 327, 20762, 20762, 269, GumpButtonType.Reply, 0);
			}

			public override void OnResponse( NetState state, RelayInfo info ) 
			{
				Mobile from = state.Mobile; 
				DoPrank( from, info.ButtonID );
				from.CloseGump( typeof( TricksSmallColumn ) );
				if ( Server.Misc.GetPlayerInfo.isJester ( from ) )
				{
					from.SendGump( new TricksSmallColumn( from ) );
				}
			}
		}

		public override bool OnDragDrop( Mobile from, Item dropped )
		{
			if ( dropped is Gold )
			{
				int fool = 0;

				foreach ( Mobile m in this.GetMobilesInRange( 20 ) )
				{
					if ( m is Jester || m is ChucklesJester )
						++fool;
				}

				if ( fool == 0 )
				{
					from.SendMessage( "You need to be near a local jester to add pranks!" );
				}
				else if ( PrankPoints >= 50000 )
				{
					from.SendMessage( "That bag is already full of pranks." );
				}
				else if ( ( PrankPoints + dropped.Amount ) < 50000 )
				{
					from.SendMessage( "You add some more gold for pranks." );
					PrankPoints = PrankPoints + dropped.Amount;
					from.PlaySound( 0x2E6 );
					dropped.Delete();
				}
				else
				{
					int need = 50000 - PrankPoints;
					from.SendMessage( "You add some more gold for pranks and now the bag is full." );
					PrankPoints = 50000;
					dropped.Amount = dropped.Amount - need;
					from.PlaySound( 0x2E6 );
				}
			}

			InvalidateProperties();
			return false;
		}
	}
}

namespace Server.Gumps
{
    public class InfoJester : Gump
    {
        public InfoJester( int i, Mobile from ) : base( 50, 50 )
        {
			string color = "#b3706f";
			from.SendSound( 0x4A );

            this.Closable=true;
			this.Disposable=true;
			this.Dragable=true;
			this.Resizable=false;

			int tile1 = Utility.RandomList( 151, 152 );
			int tile2 = Utility.RandomList( 153, 154 );
			if ( Utility.RandomBool() )
			{
				tile1 = Utility.RandomList( 153, 154 );
				tile2 = Utility.RandomList( 151, 152 );
			}

			AddPage(0);

			AddImage(0, 0, 7040, Server.Misc.PlayerSettings.GetGumpHue( from ));
			AddButton(279, 10, 4017, 4017, 0, GumpButtonType.Reply, 0);
			AddImage(10, 9, i+10);
			AddHtml( 44, 12, 226, 20, @"<BODY><BASEFONT Color=" + color + ">" + Server.Items.BagOfTricks.JokeInfo( i, "name" ) + "</BASEFONT></BODY>", (bool)false, (bool)false);
			AddHtml( 12, 46, 294, 214, @"<BODY><BASEFONT Color=" + color + ">" + Server.Items.BagOfTricks.JokeInfo( i, "detail" ) + "</BASEFONT></BODY>", (bool)false, (bool)false);
			AddHtml( 12, 274, 181, 20, @"<BODY><BASEFONT Color=" + color + ">Prank Points: " + Server.Items.BagOfTricks.JokeInfo( i, "points" ) + "</BASEFONT></BODY>", (bool)false, (bool)false);
			AddHtml( 205, 274, 100, 20, @"<BODY><BASEFONT Color=" + color + "><RIGHT>Mana: " + Server.Items.BagOfTricks.JokeInfo( i, "mana" ) + "</RIGHT></BASEFONT></BODY>", (bool)false, (bool)false);
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;
			from.SendSound( 0x4A );
			from.CloseGump( typeof( Server.Items.BagOfTricks.BagOfTricksGump ) );
			from.CloseGump( typeof( InfoJester ) );
			from.SendGump( new Server.Items.BagOfTricks.BagOfTricksGump( from ) );
        }
    }
}