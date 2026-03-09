using System;
using Server;
using Server.Items;
using System.Text;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;

namespace Server.Items
{
	public class LearnStealingBook : Item
	{
		[Constructable]
		public LearnStealingBook( ) : base( 0x02DD )
		{
			Weight = 1.0;
			Name = "The Art of Thievery";
			ItemID = Utility.RandomList( 0x02DD, 0x201A );
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );
			list.Add( "What To Steal For Better Profit" );
		}

		public class LearnStealingGump : Gump
		{
			public LearnStealingGump( Mobile from ): base( 50, 50 )
			{
				string color = "#ddbc4b";

				this.Closable=true;
				this.Disposable=true;
				this.Dragable=true;
				this.Resizable=false;

				AddPage(0);

				AddImage(0, 0, 9547, Server.Misc.PlayerSettings.GetGumpHue( from ));

				AddHtml( 15, 15, 398, 20, @"<BODY><BASEFONT Color=" + color + ">A ARTE DO LADRÃO</BASEFONT></BODY>", (bool)false, (bool)false);

				AddButton(567, 11, 4017, 4017, 0, GumpButtonType.Reply, 0);

				AddHtml( 14, 50, 579, 388, @"<BODY><BASEFONT Color=" + color + ">Para aqueles habilidosos na arte de bisbilhotar e roubar, a busca por artefatos antigos pode ser um empreendimento lucrativo. Procurando em algumas criptas, tumbas e masmorras... você pode encontrar pedestais com caixas e bolsas ornamentadas que podem conter algo de grande valor. Pode ser um item raro, uma bela peça de arte ou uma arma antiga. As bolsas e caixas finamente trabalhadas podem ser mantidas para si mesmo, ou podem ser vendidas a um ladrão na guilda, onde eles pagarão alegremente algum ouro por cada uma. Estas são altamente colecionáveis e eles têm contatos na guilda para revendê-las à realeza, negociantes de arte ou colecionadores. Quando você encontrar esses pedestais, e houver um item sobre ele, clique duas vezes para tentar roubar o item. Se você não for bem treinado em bisbilhotar, pode ativar uma armadilha mortal. Ter uma boa habilidade de remover armadilhas pode evitar os efeitos de tais armadilhas. Uma vez que a armadilha é evitada, então sua habilidade em roubar será testada. Se você conseguir obter o item, olhe inside e reivindique seu prêmio.<br><br>Muitas pessoas na cidade estão procurando por artefatos raros e podem pagar generosamente por eles.<br><br>Há também baús, caixas, bolsas e malas que contêm tesouro nesses lugares. Você pode tentar roubar esses recipientes. Certifique-se de pegar o que quiser deles antes de roubá-los, pois você esvaziará o recipiente em sua fuga. Um ladrão na guilda também pode pagar por esses recipientes vendendo-os, pois eles também são colecionáveis para outros e podem alcançar um bom preço. Se você quiser levar um desses recipientes de masmorra, use sua habilidade de roubo e então direcione o recipiente. Talvez você seja rápido o suficiente.<br><br>Embora você também possa buscar ouro vasculhando os bolsos de mercadores, você também pode roubar ouro de seus cofres. Você pode bisbilhotar os cofres para ver quanto ouro há nele, e então você pode usar sua habilidade de roubo no cofre para tentar pegar o ouro. Isso pode praticar sua habilidade, mas é uma manobra complicada se você for pego. Você pode roubar moedas e similares de outras criaturas ficando next to a elas e atacando-as, onde você pode automaticamente roubar tais itens ao atacar.</BASEFONT></BODY>", (bool)false, (bool)true);

				AddItem(554, 449, 4643);
				AddItem(19, 457, 13042);
				AddItem(554, 447, 3702);
				AddItem(388, 484, 5373);
				AddItem(18, 459, 3712);
				AddItem(370, 461, 7183);
				AddItem(202, 458, 13111);
			}

			public override void OnResponse( NetState state, RelayInfo info ) 
			{
				Mobile from = state.Mobile;
				from.SendSound( 0x249 );
			}
		}

		public override void OnDoubleClick( Mobile e )
		{
			if ( !IsChildOf( e.Backpack ) && this.Weight != -50.0 ) 
			{
				e.SendMessage( "This must be in your backpack to read." );
			}
			else
			{
				e.CloseGump( typeof( LearnStealingGump ) );
				e.SendGump( new LearnStealingGump( e ) );
				e.PlaySound( 0x249 );
				Server.Gumps.MyLibrary.readBook ( this, e );
			}
		}

		public LearnStealingBook(Serial serial) : base(serial)
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