using System;
using Server;
using Server.Network;
using Server.Gumps;

namespace Server.Items 
{
	[Flipable( 0x12AB, 0x12AC )]
	public class TarotDeck : Item
	{
		private static string GetFortuneMsg( int MyFortune )
		{
			string phrase = "";

			switch ( MyFortune )
			{
				default:
				case 0: phrase = "o Tolo! Eles devem tomar cuidado e usar a cabeça."; break;
				case 1: phrase = "o Mago! Eles exibem maior controle sobre seu destino."; break;
				case 2: phrase = "a Alta Sacerdotisa! Seu caminho se tornará claro para eles."; break;
				case 3: phrase = "a Imperatriz! A vida está correndo suavemente."; break;
				case 4: phrase = "o Imperador! Eles devem lutar pelo que é seu."; break;
				case 5: phrase = "o Hierofante! Eles devem reconhecer sua falibilidade."; break;
				case 6: phrase = "os Amantes! Eles serão confrontados com uma escolha importante."; break;
				case 7: phrase = "a Carruagem! Eles estão em posição de derrotar seus inimigos. Ataquem agora!"; break;
				case 8: phrase = "a Justiça! Eles receberão o que merecem."; break;
				case 9: phrase = "o Eremita! Eles descobrirão uma grande verdade."; break;
				case 10: phrase = "a Roda da Fortuna! Seu destino está baseado no capricho dos deuses."; break;
				case 11: phrase = "a Força! Eles enfrentarão um grande teste de sua resistência."; break;
				case 12: phrase = "o Enforcado! Eles devem sacrificar para atingir seu objetivo."; break;
				case 13: phrase = "a Morte! Sua vida mudará completamente... em breve."; break;
				case 14: phrase = "a Temperança! Eles devem ser pacientes!"; break;
				case 15: phrase = "o Diabo! Eles não devem tomar o caminho mais fácil, isso pode significar destruição!"; break;
				case 16: phrase = "a Torre! Eles ultrapassaram seus limites."; break;
				case 17: phrase = "a Estrela! O que eles buscam está ao seu alcance."; break;
				case 18: phrase = "a Lua! Eles devem ter cuidado com forças além de seu controle, para que não os controlem!"; break;
				case 19: phrase = "o Sol! Eles trabalharam duro. Agora podem desfrutar os frutos de seu trabalho."; break;
				case 20: phrase = "o Julgamento! Seu sucesso é garantido. Eles devem atacar enquanto o ferro está quente!"; break;
				case 21: phrase = "o Mundo! Eles alcançaram um sucesso completo em seu empreendimento."; break;
			}
			return phrase;
		}

		private int GetFortuneImg( int MyFortune )
		{
			int value = 0;

			switch ( MyFortune )
			{
				default:
				case 0: value = 0x454; break;
				case 1: value = 0x45C; break;
				case 2: value = 0x458; break;
				case 3: value = 0x453; break;
				case 4: value = 0x452; break;
				case 5: value = 0x457; break;
				case 6: value = 0x45B; break;
				case 7: value = 0x44F; break;
				case 8: value = 0x45A; break;
				case 9: value = 0x456; break;
				case 10: value = 0x463; break;
				case 11: value = 0x45F; break;
				case 12: value = 0x455; break;
				case 13: value = 0x450; break;
				case 14: value = 0x461; break;
				case 15: value = 0x451; break;
				case 16: value = 0x462; break;
				case 17: value = 0x45E; break;
				case 18: value = 0x45D; break;
				case 19: value = 0x460; break;
				case 20: value = 0x459; break;
				case 21: value = 0x464; break;
			}
			return value;
		}

        private class TarotGump : Gump
        {
            public TarotGump( int card ) : base(0, 0)
            {
				this.Closable=true;
				this.Disposable=true;
				this.Dragable=true;
				this.Resizable=false;
				AddPage(0);
				AddImage(52, 52, card);
            }
        }

		[Constructable]
		public TarotDeck() : base( 0x12AB )
		{
			Name = "tarot deck";
		}
		
		public TarotDeck( Serial serial ) : base( serial ) 
		{ 
		}
	
		public override void Serialize( GenericWriter writer ) 
		{
			base.Serialize( writer );
			writer.Write( (int) 0 );
		}
	
		public override void Deserialize(GenericReader reader) 
		{ 
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	
		public override void OnDoubleClick( Mobile from ) 
		{
			from.CloseGump( typeof( TarotGump ) );
			int MyFortune = Utility.Random( 22 );

			switch ( ((Item)this).ItemID )
			{
				case 0x12AB: // Closed north
					if ( Utility.Random( 2 ) == 0 )
						((Item)this).ItemID = 0x12A5;
					else
						((Item)this).ItemID = 0x12A8;
					break;
				case 0x12AC: // Closed east
					if ( Utility.Random( 2 ) == 0 )
						((Item)this).ItemID = 0x12A6;
					else
						((Item)this).ItemID = 0x12A7;
					break;
				case 0x12A5:
					from.PublicOverheadMessage(MessageType.Regular, 0, false, string.Format("{0} draws " + GetFortuneMsg( MyFortune ) + "", from.Name));
					from.SendGump( new TarotGump( GetFortuneImg( MyFortune ) ) );
					break;
				case 0x12A6:
					from.PublicOverheadMessage(MessageType.Regular, 0, false, string.Format("{0} draws " + GetFortuneMsg( MyFortune ) + "", from.Name));
					from.SendGump( new TarotGump( GetFortuneImg( MyFortune ) ) );
					break;
				case 0x12A8:
					from.PublicOverheadMessage(MessageType.Regular, 0, false, string.Format("{0} draws " + GetFortuneMsg( MyFortune ) + "", from.Name));
					from.SendGump( new TarotGump( GetFortuneImg( MyFortune ) ) );
					break;
				case 0x12A7:
					from.PublicOverheadMessage(MessageType.Regular, 0, false, string.Format("{0} draws " + GetFortuneMsg( MyFortune ) + "", from.Name));
					from.SendGump( new TarotGump( GetFortuneImg( MyFortune ) ) );
					break;
			}
		}
		
		public override void OnAdded(object target)
		{
			switch ( ((Item)this).ItemID )
			{
				case 0x12A5: ((Item)this).ItemID = 0x12AB; break; // Open north
				case 0x12A6: ((Item)this).ItemID = 0x12AC; break; // Open east
				case 0x12A8: ((Item)this).ItemID = 0x12AB; break; // Open north
				case 0x12A7: ((Item)this).ItemID = 0x12AC; break; // Open east
			}
		}
	}
	
	[Flipable( 0x12AB, 0x12AC )]
	public class DecoTarotDeck : Item
	{
		[Constructable]
		public DecoTarotDeck() : base( 0x12AB )
		{
			Name = "tarot deck";
		}
		
		public DecoTarotDeck( Serial serial ) : base( serial ) 
		{ 
		}
	
		public override void Serialize( GenericWriter writer ) 
		{
			base.Serialize( writer );
			writer.Write( (int) 0 );
		}
	
		public override void Deserialize(GenericReader reader) 
		{ 
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	
		public override void OnDoubleClick( Mobile from ) 
		{
			switch ( ((Item)this).ItemID )
			{
				case 0x12AB: // Closed north
					if ( Utility.Random( 2 ) == 0 )
						((Item)this).ItemID = 0x12A5;
					else
						((Item)this).ItemID = 0x12A8;
					break;
				case 0x12AC: // Closed east
					if ( Utility.Random( 2 ) == 0 )
						((Item)this).ItemID = 0x12A6;
					else
						((Item)this).ItemID = 0x12A7;
					break;
				case 0x12A5: ((Item)this).ItemID = 0x12AB; break; // Open north
				case 0x12A6: ((Item)this).ItemID = 0x12AC; break; // Open east
				case 0x12A8: ((Item)this).ItemID = 0x12AB; break; // Open north
				case 0x12A7: ((Item)this).ItemID = 0x12AC; break; // Open east
			}
		}
	}
}
