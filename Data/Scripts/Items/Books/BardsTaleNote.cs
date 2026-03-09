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
	public class BardsTaleNote : Item
	{
		public string ScrollMessage;

		[CommandProperty(AccessLevel.Owner)]
		public string Scroll_Message { get { return ScrollMessage; } set { ScrollMessage = value; InvalidateProperties(); } }

		[Constructable]
		public BardsTaleNote( ) : base( 0xE34 )
		{
			Weight = 1.0;
			Hue = 0xB98;
			Name = "um pergaminho antigo";

			switch ( Utility.RandomMinMax( 0, 2 ) )
			{
				case 0: Name = "pergaminho"; break;
				case 1: Name = "nota"; break;
				case 2: Name = "rolo"; break;
			}

			switch ( Utility.RandomMinMax( 0, 5 ) )
			{
				case 0: Name = "um antigo " + Name; break;
				case 1: Name = "um desgastado " + Name; break;
				case 2: Name = "um gasto " + Name; break;
				case 3: Name = "um rabiscado " + Name; break;
				case 4: Name = "um incomum " + Name; break;
				case 5: Name = "um estranho " + Name; break;
			}

			ItemID = Utility.RandomList( 0xE34, 0x14ED, 0x14EE, 0x14EF, 0x14F0 );

			int amnt = Utility.RandomMinMax( 1, 18 );

			switch ( amnt )
			{
				case 1: ScrollMessage = "O mago chamado Kylearan está realmente por trás do destino de Skara Brae."; break;
				case 2: ScrollMessage = "A espada de cristal pode cortar através do vazio."; break;
				case 3: ScrollMessage = "A estátua de cristal é indestrutível."; break;
				case 4: ScrollMessage = "Há uma maneira de escapar do vazio a partir dos esgotos."; break;
				case 5: ScrollMessage = "Há uma passagem secreta na adega abaixo do Bardo Escarlate."; break;
				case 6: ScrollMessage = "Alguém poderia entrar nas catacumbas se soubesse o nome do deus louco."; break;
				case 7: ScrollMessage = "Há mais de uma maneira de entrar na torre de Mangar."; break;
				case 8: ScrollMessage = "Há uma passagem secreta no quarto do caçador de veados."; break;
				case 9: ScrollMessage = "Há uma passagem atrás do trono de Harkyn."; break;
				case 10: ScrollMessage = "O dragão cinza guarda a chave para escapar."; break;
				case 11: ScrollMessage = "Alguns acreditam que um acordo foi feito entre Kylearan e Mangar"; break;
				case 12: ScrollMessage = "Há uma caverna onde Garth obtém seu minério."; break;
				case 13: ScrollMessage = "Houve um deus louco que deixou ruínas de Skara Brae em Sosaria."; break;
				case 14: ScrollMessage = "Há três formas de prata necessárias para entrar no quarto de Mangar."; break;
				case 15: ScrollMessage = "Alguns viram um mago que entrava em suas celas de masmorra e desaparecia."; break;
				case 16: ScrollMessage = "Há uma estátua do deus louco no topo da torre de Harkyn."; break;
				case 17: ScrollMessage = "Alguns acreditam que a chave da torre de Mangar foi vista na torre de Kylearan."; break;
				case 18: ScrollMessage = "Há muito tempo, uma estátua de cristal foi esculpida com uma caixa de jade inside."; break;
			}
		}

		public class ClueGump : Gump
		{
			public ClueGump( Mobile from, Item parchment ): base( 100, 100 )
			{
				BardsTaleNote scroll = (BardsTaleNote)parchment;
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

		public override void OnDoubleClick( Mobile e )
		{
			if ( !IsChildOf( e.Backpack ) ) 
			{
				e.SendMessage( "This must be in your backpack to read." );
			}
			else
			{
				e.CloseGump( typeof( ClueGump ) );
				e.SendGump( new ClueGump( e, this ) );
				e.PlaySound( 0x249 );
			}
		}

		public BardsTaleNote(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int) 0);
            writer.Write( ScrollMessage );
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
			ScrollMessage = reader.ReadString();
		}
	}
}