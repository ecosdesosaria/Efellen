using System;
using Server;
using Server.Misc;
using Server.Gumps;
using Server.Network;
using Server.Commands;
using Server.Items;
using System.Text;
using Server.Mobiles;
using System.Collections;
using Server.Commands.Generic;

namespace Server.Gumps
{
	public class SummonTutorial : Gump
	{
        public SummonTutorial( Mobile from, SummonPrison item ) : base( 50, 50 )
        {
			string color = "#90c5d0";
			string regs = "#b8d090";
			from.SendSound( 0x5C9 ); 

			string sEnding = "Se alguém tocar mais de uma dessas prisões mágicas, todas, exceto uma, desaparecerão no vazio.";
				if ( item.owner != null ){ sEnding = "Se " + item.owner.Name + " tocar outra prisão mágica como esta, esta prisão selada desaparecerá no vazio."; }
			string sPrisoner = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(item.Prisoner.ToLower());

			string sText = "Você encontrou um orbe raro que contém o espírito de " + sPrisoner + ". Magicamente selado aqui por " + item.Jailor + ", você não tem ideia de quanto tempo ele(a) esteve trancado(a). Para libertar " + sPrisoner + " desta prisão mágica, você precisará encontrar alguns itens especiais. Uma vez que os itens tenham sido encontrados, esta prisão de cristal precisará ser levada para " + item.Dungeon + ", onde ele(a) foi enfeitiçado(a) no orbe. Se for libertado(a), certamente buscará desencadear fúria sobre todos que estiverem em sua frente, mas o que ele(a) guardava antes de seu encarceramento pode valer o risco.<br><br><br><br>Abaixo você pode ver quais itens precisa para destravar a cela. Quando você obtiver todos os itens necessários, aventure-se no local do encarceramento e use o orbe lá. Esteja pronto para a batalha nesse caso, pois você pode não saber o que realmente enfrenta. Ele(a) esteve trancado(a) por anos, ou talvez séculos, então a loucura certamente o(a) dominou até agora. Uma vez libertado(a), ele(a) permanecerá por uma hora antes de deixar a área e ir para outro lugar, para sempre. Seja rápido com o ataque iminente se esta luta é verdadeiramente o curso desejado que você deseja tomar. " + sEnding;
            this.Closable=true;
			this.Disposable=true;
			this.Dragable=true;
			this.Resizable=false;

			AddPage(0);

			AddImage(0, 0, 5595, Server.Misc.PlayerSettings.GetGumpHue( from ));
			AddButton(767, 10, 4017, 4017, 0, GumpButtonType.Reply, 0);
			AddHtml( 12, 12, 667, 20, @"<BODY><BASEFONT Color=" + color + ">PRISÕES DE FEITIÇARIA</BASEFONT></BODY>", (bool)false, (bool)false);
			AddHtml( 12, 45, 783, 353, @"<BODY><BASEFONT Color=" + color + ">" + sText + "</BASEFONT></BODY>", (bool)false, (bool)false);

			sText = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(item.Prisoner.ToLower());

			sText = sText + "<br><br>Para libertá-lo(a), você precisa:";

			sText = sText + "<br><br>" + System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(item.KeyA.ToLower());
			sText = sText + "<br>" + System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(item.KeyB.ToLower());
			sText = sText + "<br>" + System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(item.KeyC.ToLower());

			sText = sText + "<br>" + item.ReagentQtyA.ToString() + " " + System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(item.ReagentNameA.ToLower());
			sText = sText + "<br>" + item.ReagentQtyB.ToString() + " " + System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(item.ReagentNameB.ToLower());

			sText = sText + "<br><br>Então leve para " + item.Dungeon;

			AddHtml( 12, 368, 783, 266, @"<BODY><BASEFONT Color=" + regs + ">" + sText + "</BASEFONT></BODY>", (bool)false, (bool)false);
        }

        public override void OnResponse(NetState state, RelayInfo info)
        {
			Mobile from = state.Mobile;
			from.SendSound( 0x5C9 ); 
        }
    }
}