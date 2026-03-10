using System;
using Server;
using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using System.Collections;

namespace Server.Gumps
{
	public class LevelUpAcceptGump : Gump
	{
        private LevelUpScroll m_Scroll;
		private Mobile m_From;

		public LevelUpAcceptGump( LevelUpScroll scroll, Mobile from ) : base( 0, 0 )
		{
			m_Scroll = scroll;
			m_From = from;

            string PaymentMsg = null;
            if (LevelItems.RewardBlacksmith && LevelItems.BlacksmithRewardAmt > 0)
                PaymentMsg = "<BR><BR>Se você aceitar e o processo for bem-sucedido, você receberá uma compensação adicional de " + LevelItems.BlacksmithRewardAmt + ".";

			Closable=false;
			Disposable=false;
			Dragable=true;
			Resizable=false;
			AddPage(0);

            AddBackground(25, 22, 318, 268, 9390);
            AddLabel(52, 27, 0, @"Level Increase Request");
            AddLabel(52, 60, 0, @"Requested By:");
            AddLabel(52, 81, 0, @"Level Amount:");
            AddHtml(49, 109, 271, 116, @"<CENTER><U>Solicitação de Aumento de Nível Máximo</U><BR><BR>Alguém solicitou seus serviços especializados para aumentar os níveis máximos de um item de nível."+PaymentMsg+"<BR><BR>Você aceita a oferta?", (bool)false, (bool)true);
            AddButton(50, 235, 4023, 4024, 1, GumpButtonType.Reply, 0);
            AddButton(83, 235, 4017, 4018, 2, GumpButtonType.Reply, 0);
            if (m_From != null)
                AddLabel(155, 60, 390, m_From.Name.ToString());
            if (m_Scroll != null)
                AddLabel(155, 81, 390, m_Scroll.Value.ToString());
		}

		public override void OnResponse( NetState state, RelayInfo info )
		{
			Mobile smith = state.Mobile;

			if ( smith == null )
				return;

			//Accept
			if ( info.ButtonID == 1 )
			{
                if ( m_From != null && m_Scroll != null)
                {
					if ( m_From != null )
					{
						m_Scroll.BlacksmithValidated = true;
                        m_From.CloseGump(typeof(AwaitingSmithApprovalGump));
                        m_From.SendMessage("Eles validaram seu pergaminho. Selecione um item de nível para aumentar o nível máximo ou ESC para aplicar em outro momento.");
                        m_From.Target = new LevelUpScroll.LevelItemTarget(m_Scroll); // Call our target
					}

					if ( smith != null ) //Accepted... send message to smith and pay them bonus reward
					{
						smith.SendMessage("Obrigado pelos seus serviços!");
                        if (smith != m_From && LevelItems.RewardBlacksmith && LevelItems.BlacksmithRewardAmt > 0)
                        {
                            smith.AddToBackpack(new BankCheck(LevelItems.BlacksmithRewardAmt));
                            smith.SendMessage("Um pagamento bônus foi adicionado à sua mochila.");
                        }
					}
				}
				else
				{
					if ( m_From != null && smith != null )
					{
						m_From.SendMessage( "Houve um problema ao validar este pergaminho." );
						smith.SendMessage( "Houve um problema ao validar este pergaminho." );
					}
				}
			}

			//Decline
			if ( info.ButtonID == 2 )
			{
				smith.SendMessage( "Você recusou a oferta." );

				if ( m_From != null )
                    m_From.CloseGump(typeof(AwaitingSmithApprovalGump));
					m_From.SendMessage( "Eles recusaram sua oferta." );
			}
		}
	}
}