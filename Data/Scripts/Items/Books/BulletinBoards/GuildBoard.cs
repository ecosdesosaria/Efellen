using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Network;
using Server.Misc;
using Server.Prompts;
using System.Net;
using Server.Accounting;
using Server.Mobiles;
using Server.Commands;
using Server.Regions;
using Server.Spells;
using Server.Gumps;
using Server.Targeting;

namespace Server.Items
{
	[Flipable(0x577B, 0x577C)]
	public class GuildBoard : Item
	{
		[Constructable]
		public GuildBoard( ) : base( 0x577B )
		{
			Weight = 1.0;
			Name = "Local Guilds";
			Hue = 0xB79;
		}

		public override void OnDoubleClick( Mobile e )
		{
			if ( e.InRange( this.GetWorldLocation(), 4 ) )
			{
				e.CloseGump( typeof( GuildBoardGump ) );
				e.SendGump( new GuildBoardGump( e ) );
			}
			else
			{
				e.SendLocalizedMessage( 502138 ); // That is too far away for you to use
			}
		}

		public class GuildBoardGump : Gump
		{
			public GuildBoardGump( Mobile from ): base( 100, 100 )
			{
				from.SendSound( 0x59 );
				string guildMasters = "<br><br>";
				foreach ( Mobile target in World.Mobiles.Values )
				if ( target is BaseGuildmaster )
				{
					guildMasters = guildMasters + target.Name + "<br>" + target.Title + "<br>" + Server.Misc.Worlds.GetRegionName( target.Map, target.Location ) + "<br><br>";
				}

				this.Closable=true;
				this.Disposable=true;
				this.Dragable=true;
				this.Resizable=false;

				AddPage(0);
				AddImage(0, 0, 9541, Server.Misc.PlayerSettings.GetGumpHue( from ));

				PlayerMobile pm = (PlayerMobile)from;
				if ( pm.NpcGuild != NpcGuild.None )
				{
					AddHtml( 55, 402, 285, 20, @"<BODY><BASEFONT Color=#e97f76>Renunciar à Minha Guilda Local</BASEFONT></BODY>", (bool)false, (bool)false);
					AddButton(16, 401, 4005, 4005, 10, GumpButtonType.Reply, 0);
				}

				string warn = "Atenção, cada guilda que você entrar terá uma taxa aumentada para ingressar. Isto é baseado no número de guildas das quais você foi membro anteriormente. Então, quando você entrar em uma guilda por " + MyServerSettings.JoiningFee( from ).ToString() + " de ouro, a próxima guilda que você entrar exigirá " + (MyServerSettings.JoiningFee( from )*2).ToString() + " de ouro. A guilda seguinte será " + (MyServerSettings.JoiningFee( from )*3).ToString() + " de ouro. ";
				if ( !MySettings.S_GuildIncrease )
					warn = "";

				string benefit = "Um dos benefícios de entrar em uma guilda local é receber mais ouro por bens vendidos a outros membros da guilda. Você também receberá";
				if ( !MySettings.S_VendorsBuyStuff )
					benefit = "";

				AddHtml( 11, 12, 562, 20, @"<BODY><BASEFONT Color=#b6d593>GUILDAS LOCAIS</BASEFONT></BODY>", (bool)false, (bool)false);
				AddHtml( 12, 44, 623, 349, @"<BODY><BASEFONT Color=#b6d593>Há muitos grupos na terra que estabeleceram casas de guilda e frequentemente procuram por membros. Essas guildas são separadas das várias guildas de aventureiros que podem ser estabelecidas por conta própria, pois focam em um grupo de pessoas com um conjunto específico de habilidades e ofício. Abaixo está uma lista de casas de guilda procurando por membros.<br><br>- Guilda dos Alquimistas<br>- Guilda dos Arqueiros<br>- Guilda dos Assassinos<br>- Guilda dos Bardos<br>- Guilda dos Ferreiros<br>- Guilda dos Carpinteiros<br>- Guilda dos Cartógrafos<br>- Guilda dos Culinários<br>- Guilda dos Druidas<br>- Guilda dos Elementalistas<br>- Guilda dos Curandeiros<br>- Guilda dos Bibliotecários<br>- Guilda dos Magos<br>- Guilda dos Marinheiros<br>- Guilda dos Mercadores<br>- Guilda dos Mineiros<br>- Guilda dos Necromantes<br>- Guilda dos Rangers<br>- Guilda dos Alfaiates<br>- Guilda dos Ladrões<br>- Guilda dos Funileiros<br>- Guilda dos Guerreiros<br><br>O requisito para entrada em qualquer uma dessas guildas (além de não ser membro de outra guilda local) é " + MyServerSettings.JoiningFee( from ).ToString() + " de ouro pago ao mestre da guilda. Para entrar em uma guilda, encontre o mestre da guilda apropriado e clique uma vez nele para selecionar 'Entrar'. Eles então pedirão uma quantia de ouro se você atender às qualificações. Apenas solte a quantia exata de ouro neles para entrar. Você pode renunciar a uma guilda voltando ao seu mestre da guilda, clicando uma vez nele e selecionando 'Renunciar' (ou você pode usar este quadro para renunciar). Então você poderia entrar em outra guilda. " + warn + "" + benefit + " um anel de associação à guilda que o ajudará com habilidades relacionadas à guilda, que seria seu e somente seu. Se você perder seu anel por qualquer motivo, dê a um mestre da guilda 400 de ouro para substituí-lo. As habilidades auxiliadas pelo anel também são as habilidades que você ganhará mais rapidamente, sendo membro da guilda. Você também poderá comprar itens dos mestres da guilda, pois eles vendem itens extras para membros da guilda.<br><br>Para roubar de outros jogadores, você deve ser membro da Guilda dos Ladrões." + guildMasters + "</BASEFONT></BODY>", (bool)false, (bool)true);
				AddButton(609, 8, 4017, 4017, 0, GumpButtonType.Reply, 0);
			}

			public override void OnResponse( NetState state, RelayInfo info )
			{
				Mobile from = state.Mobile;
				PlayerMobile pm = (PlayerMobile)from;
				from.SendSound( 0x59 );

				if ( info.ButtonID > 0 )
					BaseGuildmaster.ResignGuild( from, null );
			}
		}

		public GuildBoard(Serial serial) : base(serial)
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