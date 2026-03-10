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

namespace Server.Items
{
	public class CarpetBuild : Item
	{
		[Constructable]
		public CarpetBuild() : base( 0x1A97 )
		{
			Weight = 2.0;
			Hue = 0x95B;
			Name = "The Carpet of Aladdin";

			if ( Weight > 1.0 )
			{
				Weight = 1.0;
				HaveGold = 0;
				HaveCloth = 0;
			}
		}

        public override void AddNameProperties(ObjectPropertyList list)
		{
            base.AddNameProperties(list);

			int needGold = 20000 - HaveGold;
				if ( needGold < 0 ){ needGold = 0; }
			int needCloth = 10000 - HaveCloth;
				if ( needCloth < 0 ){ needCloth = 0; }

			int carpetDone = needGold + needCloth;

			if ( carpetDone > 0 )
			{
				list.Add( 1070722, "Coloque os Itens Necessários Neste Livro" );
				list.Add( 1049644, "Precisa de " + needGold.ToString() + " Moedas de Ouro, " + needCloth.ToString() + " Tecido" );
			}
			else
			{
				list.Add( 1070722, "Leia o Livro para Conjurá-lo" );
			}
        }

		public override void OnDoubleClick( Mobile from )
		{
			int needGold = 20000 - HaveGold;
			int needCloth = 10000 - HaveCloth;

			int carpetDone = needGold + needCloth;

			if ( carpetDone > 0 )
			{
				from.SendMessage( "Você precisa coletar mais itens antes de conseguir conjurá-lo!" );
				from.SendSound( 0x4A );
				from.CloseGump( typeof( RugGump ) );
				from.SendGump( new RugGump( from, HaveGold, HaveCloth ) );
			}
			else
			{
				int builder = 0;

				foreach ( Mobile m in this.GetMobilesInRange( 20 ) )
				{
					if ( m is Mage || m is Witches || m is Necromancer || m is MageGuildmaster || m is NecromancerGuildmaster )
						++builder;
				}

				if ( builder < 1 )
				{
					from.SendMessage( "Você precisa estar perto de um mago para conjurar isso!" );
					from.SendSound( 0x4A );
					from.CloseGump( typeof( RugGump ) );
					from.SendGump( new RugGump( from, HaveGold, HaveCloth ) );
				}
				else
				{
					from.SendMessage( "Você lê o livro e ele se transforma em um tapete mágico." );
					from.PlaySound( 0x243 );
					from.AddToBackpack ( new Multis.MagicCarpetADeed() );
					this.Delete();
				}
			}
		}

		private class RugGump : Gump
		{
			public RugGump( Mobile from, int gold, int cloth ) : base( 50, 50 )
			{
				from.SendSound( 0x4A ); 
				string color = "#9ab9cb";

				this.Closable=true;
				this.Disposable=true;
				this.Dragable=true;
				this.Resizable=false;

				if ( gold > 20000 ){ gold = 20000; }
				if ( cloth > 10000 ){ cloth = 10000; }

				AddPage(0);

				AddImage(0, 0, 7008, Server.Misc.PlayerSettings.GetGumpHue( from ));
				AddButton(400, 11, 4017, 4017, 0, GumpButtonType.Reply, 0);
				AddHtml( 12, 14, 357, 20, @"<BODY><BASEFONT Color=" + color + ">O TAPETE DE ALADIM</BASEFONT></BODY>", (bool)false, (bool)false);
				AddHtml( 13, 43, 415, 165, @"<BODY><BASEFONT Color=" + color + ">Este livro conta a história do tapete mágico de Aladim. Nestas páginas, você aprendeu os segredos para ter seu próprio tapete mágico conjurado. Séculos atrás, lendas contam que Aladim planava sobre os mares em seu tapete mágico. Se você quiser ter um item assim criado para si mesmo, precisará coletar 10.000 de tecido e então ler as palavras mágicas deste livro enquanto estiver near de um mago local, pois certamente precisará da ajuda dele. Por seus serviços, o mago exigirá 20.000 de ouro. Para coletar esses itens, coloque-os neste livro conforme você os adquire. Uma vez que você coletar o tecido e o ouro necessários, encontre um mago local e leia o livro enquanto estiver com ele. O livro então deve se transformar em um tapete mágico.<br><br>Tapetes mágicos funcionam de forma muito semelhante a vessels de navegação, exceto que eles podem ser lançados de qualquer costa e não têm um convés inferior para descanso. Talvez o livro, Crânios e Algemas, o ajude, pois a navegação e manutenção são as mesmas para ambos os vessels de navegação e tapetes mágicos. Diferente de navios com pranchas para embarcar, tapetes mágicos têm tecido estendido do qual você pode embarcar e desembarcar. Você também terá uma chave mágica para proteger seu tapete de visitantes indesejados. Eles não têm um porão como um navio, mas instead uma bolsa mágica na qual você pode armazenar itens. Eles não têm um timoneiro, mas instead uma lâmpada mágica para seguir seus comandos. Se você quiser alterar o design decorativo do seu tapete, simplesmente entregue-o a um alfaiate e ele o mudará para um dos nove designs. Você pode simplesmente usar o tapete para ver a prévia de como o design ficará após ser alterado. Se você nomeou seu tapete antes da alteração, terá que renomeá-lo se tiver dado um nome anteriormente. Se você quiser mudar a cor do seu tapete, pode simplesmente tingi-lo de uma cor diferente.</BASEFONT></BODY>", (bool)false, (bool)true);
				AddHtml( 15, 221, 200, 20, @"<BODY><BASEFONT Color=" + color + ">Ouro: " + gold + "/20000</BASEFONT></BODY>", (bool)false, (bool)false);
				AddHtml( 227, 221, 200, 20, @"<BODY><BASEFONT Color=" + color + ">Tecido: " + cloth + "/10000</BASEFONT></BODY>", (bool)false, (bool)false);
			}

			public override void OnResponse(NetState state, RelayInfo info)
			{
				Mobile from = state.Mobile;
				from.SendSound( 0x4A ); 
			}
		}

		public override bool OnDragDrop( Mobile from, Item dropped )
		{          		
			Container pack = from.Backpack;
			int iAmount = 0;

			int needGold = 20000 - HaveGold;
			int needCloth = 10000 - HaveCloth;

			if ( from != null )
			{
				iAmount = dropped.Amount;

				if ( dropped is Gold && needGold > 0 )
				{
					HaveGold = HaveGold + iAmount;
					from.SendMessage( "You added " + iAmount.ToString() + " gold." );
					dropped.Delete();
					this.InvalidateProperties();
					return true;
				}
				else if ( dropped is BaseFabric && needCloth > 0 )
				{
					HaveCloth = HaveCloth + iAmount;
					from.SendMessage( "You added " + iAmount.ToString() + " cloth." );
					dropped.Delete();
					this.InvalidateProperties();
					return true;
				}
			}

			return false;
		}

		public CarpetBuild( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int)1 ); // version

			writer.Write( HaveGold );
			writer.Write( HaveCloth );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();

			HaveGold = reader.ReadInt();
			HaveCloth = reader.ReadInt();
		}

		public int HaveGold;
		[CommandProperty( AccessLevel.GameMaster )]
		public int g_HaveGold { get{ return HaveGold; } set{ HaveGold = value; } }

		public int HaveCloth;
		[CommandProperty( AccessLevel.GameMaster )]
		public int g_HaveCloth { get{ return HaveCloth; } set{ HaveCloth = value; } }
	}
}