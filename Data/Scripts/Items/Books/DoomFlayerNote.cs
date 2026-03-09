using System;
using Server;
using Server.Items;
using System.Text;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;

namespace Server.Items
{
	public class DoomFlayerNote : Item
	{
		[Constructable]
		public DoomFlayerNote( ) : base( 0xE34 )
		{
			Weight = 1.0;
			Hue = 0xB98;
			Name = "a dusty scroll";
			ItemID = 0x14EE;
		}

		public class ClueGump : Gump
		{
			public ClueGump( Mobile from ): base( 100, 100 )
			{
				from.PlaySound( 0x249 );
				string sText = "O demônio abriu o portão negro e desencadeou o caos através de Lodoria. Onde os exércitos anões haviam caído, as forças élficas reuniram toda sua magia e enviaram a fera de volta ao vazio. Enquanto o próprio mundo forneceu as forças naturais para convocar o demônio, agora elas diminuíram para o núcleo do mundo. É aqui que temos procurado por séculos, bem abaixo da cidade de Lodoria. Os drow se juntaram à nossa causa e nos ajudaram a construir nossa cidade nas profundezas, onde podemos continuar procurando em segredo. Agora que minha pesquisa em Doom está completa, retornarei ao cemitério à noite para não ser visto.";

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

		public override void OnDoubleClick( Mobile m )
		{
			if ( m.InRange( this.GetWorldLocation(), 2 ) )
			{
				m.SendGump( new ClueGump( m ) );
				m.PlaySound( 0x249 );
			}
			else
			{
				m.SendLocalizedMessage( 502138 ); // That is too far away for you to use
			}
		}

		public DoomFlayerNote(Serial serial) : base(serial)
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