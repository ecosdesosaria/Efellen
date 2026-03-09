using System;
using Server;
using Server.Misc;
using Server.Gumps;
using Server.Network;

namespace Server.Gumps 
{
    public class PrisonGump : Gump
    {
        public PrisonGump ( Mobile from ) : base ( 50, 50 )
        {
			from.SendSound( 0x4A ); 
			string color = "#e98650";

			this.Closable=true;
			this.Disposable=false;
			this.Dragable=true;
			this.Resizable=false;

			AddPage(0);

			AddImage(0, 0, 7021, Server.Misc.PlayerSettings.GetGumpHue( from ));
			AddHtml( 13, 13, 415, 20, @"<BODY><BASEFONT Color=" + color + ">ENVIADO PARA A PRISÃO</BASEFONT></BODY>", (bool)false, (bool)false);
			AddButton(466, 10, 4017, 4017, 0, GumpButtonType.Reply, 0);
			AddHtml( 16, 46, 475, 246, @"<BODY><BASEFONT Color=" + color + ">Por suas ações você foi enviado para a prisão! Embora os guardas pretendessem que você apodrecesse para sempre em sua cela, eles foram descuidados. Não só esqueceram de trancar sua cela, mas também o deixaram sozinho por um breve tempo. Você decidiu usar esta oportunidade para escapar, mas a saída está trancada. Você reúne seus pertences do baú onde os guardas os colocaram, apenas para descobrir que confiscaram algumas de suas coisas. Você certamente nunca as verá novamente. Você ouviu rumores de outros escapando desta prisão através de um túnel que escavaram de uma das celas. Talvez você possa fazer o mesmo.</BASEFONT></BODY>", (bool)false, (bool)false);
        }

        public override void OnResponse(NetState state, RelayInfo info)
        {
			Mobile from = state.Mobile;
			from.SendSound( 0x4A ); 
        }
    }
}