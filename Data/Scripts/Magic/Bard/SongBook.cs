using System;
using Server.Network;
using Server.Gumps;
using Server.Spells;
using Server.Misc;
using Server.Mobiles;

namespace Server.Items
{
	[FlipableAttribute( 0x671B, 0x671C )]
	public class SongBook : Spellbook
	{
		public override string DefaultDescription{ get{ return "Este livro é usado por bardos para escrever as canções místicas que encontram. As canções dentro do livro podem ser usadas para produzir vários efeitos mágicos. Estas canções exigem o uso de um instrumento musical. Soltar tais pergaminhos neste livro colocará a canção em suas páginas. Alguns livros têm propriedades aprimoradas, que só são efetivas quando o livro é segurado."; } }

		public override SpellbookType SpellbookType{ get{ return SpellbookType.Song; } }
		public override int BookOffset{ get{ return 351; } }
		public override int BookCount{ get{ return 16; } }

		public BaseInstrument Instrument;

		[Constructable]
		public SongBook() : this( (ulong)0 )
		{
		}

		[Constructable]
		public SongBook( ulong content ) : base( content, 0x671B )
		{
			Name = "canções de bardo";
			Layer = Layer.Trinket;
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( from.InRange( GetWorldLocation(), 1 ) )
			{
				from.CloseGump( typeof( SongBookGump ) );
				from.SendGump( new SongBookGump( from, this, 1 ) );
			}
		}

		public static string SpellDescription( int spell )
		{
			string txt = "Esta é uma canção de bardo: ";
			string skl = "0";

			if ( spell == 351 ){     skl = "55"; txt = "Uma área de efeito que regenera lentamente a saúde do seu grupo."; }
			else if ( spell == 352 ){     skl = "60"; txt = "Uma área de efeito que aumenta a inteligência do seu grupo."; }
			else if ( spell == 353 ){     skl = "50"; txt = "Uma área de efeito que aumenta a resistência à energia do seu grupo."; }
			else if ( spell == 354 ){     skl = "70"; txt = "Diminui a resistência à energia do seu alvo."; }
			else if ( spell == 355 ){     skl = "50"; txt = "Uma área de efeito que aumenta a resistência ao fogo do seu grupo."; }
			else if ( spell == 356 ){     skl = "70"; txt = "Diminui a resistência ao fogo do seu alvo."; }
			else if ( spell == 357 ){     skl = "50"; txt = "Danifica seu alvo com uma explosão de energia sônica."; }
			else if ( spell == 358 ){     skl = "50"; txt = "Uma área de efeito que aumenta a resistência ao frio do seu grupo."; }
			else if ( spell == 359 ){     skl = "70"; txt = "Diminui a resistência ao gelo do seu alvo."; }
			else if ( spell == 360 ){     skl = "50"; txt = "Uma área de efeito que aumenta a resistência física do seu grupo."; }
			else if ( spell == 361 ){     skl = "55"; txt = "Uma área de efeito que regenera lentamente a mana do seu grupo."; }
			else if ( spell == 362 ){     skl = "90"; txt = "Uma área de efeito que dissipa todas as criaturas invocadas ao seu redor."; }
			else if ( spell == 363 ){     skl = "50"; txt = "Uma área de efeito que aumenta a resistência a veneno do seu grupo."; }
			else if ( spell == 364 ){     skl = "70"; txt = "Diminui a resistência a veneno do seu alvo."; }
			else if ( spell == 365 ){     skl = "60"; txt = "Uma área de efeito que aumenta a destreza do seu grupo."; }
			else if ( spell == 366 ){     skl = "60"; txt = "Uma área de efeito que aumenta a força do seu grupo."; }

			if ( skl == "0" )
				return txt;

			return txt + " Requer pelo menos " + skl + " em Música para ser executada.";
		}

		public SongBook( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 ); // version
			writer.Write( (Item)Instrument );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
			switch( version )
			{
				case 0:
				{
					Instrument = reader.ReadItem() as BaseInstrument;
					break;
				}
			}

			if ( ItemID != 0x671B && ItemID != 0x671C )
				ItemID = 0x671B;
		}
	}
}
