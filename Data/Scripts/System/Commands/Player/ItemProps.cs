using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Commands;

namespace Server.Gumps
{
    public class ItemPropsGump : Gump
    {
		public int m_Origin;

        public ItemPropsGump( Mobile from, int origin ) : base( 50, 50 )
        {
			m_Origin = origin;
			string color = "#ddbc4b";
			from.SendSound( 0x4A ); 

			this.Closable=true;
			this.Disposable=true;
			this.Dragable=true;
			this.Resizable=false;

			AddPage(0);

			AddImage(0, 0, 9546, Server.Misc.PlayerSettings.GetGumpHue( from ));
			AddHtml( 14, 14, 400, 20, @"<BODY><BASEFONT Color=" + color + ">PROPRIEDADES DO ITEM</BASEFONT></BODY>", (bool)false, (bool)false);
			AddButton(867, 10, 4017, 4017, 0, GumpButtonType.Reply, 0);

			string lowmana = "<BR><BR>Custo de Mana Reduzido - Reduz a quantidade de mana necessária para lançar um feitiço ou usar um movimento especial.";
				if ( MyServerSettings.LowerMana() < 1 )
					lowmana = "";

			string lowreg = "<BR><BR>Custo de Reagente Reduzido - Reduz a quantidade de reagentes necessários para lançar feitiços como magia e necromancia. 100% elimina a necessidade de carregar reagentes. Pontos de dízimo, embora não usados, são necessários para lançar feitiços de Cavalaria. Elementalismo reduz a quantidade de perda de vigor para lançar feitiços.<BR><BR>Requisitos Reduzidos - Reduz quaisquer requisitos de atributo que o item tenha em uma porcentagem. Se um item tiver 100% de Requisitos Reduzidos, não terá requisitos de atributo.";
				if ( MyServerSettings.LowerReg() < 1 )
					lowreg = "";

			AddHtml( 17, 49, 875, 726, @"<BODY><BASEFONT Color=" + color + ">Muitos itens de equipamento que você encontrar podem ter atributos mágicos ou propriedades especiais. Abaixo estão as breves descrições das várias características que esses itens podem ter:<BR><BR>Aumento de Dano - Aumenta o dano base que você inflige com sua arma.<BR><BR>Modificador de Dano - Aumenta o dano final causado pelo arco com o qual é usado.<BR><BR>Aumento de Chance de Defesa - Aumenta sua chance de esquivar de golpes.<BR><BR>Densidade - A densidade de um item representa a força do material do qual o item é criado. Tem categorias de fraca, regular, ótima, superior e suprema. Quanto melhor a densidade, menor a chance de ser reduzida em durabilidade. Se houver uma chance de o item se beneficiar do auto-reparo, a quantidade reparada pode ser maior. A capacidade de aprimorar itens, e não quebrá-los, beneficia-se de uma boa densidade de material. Quando você tropeça em uma armadilha, que pode ter um efeito devastador em um item, ter uma boa densidade pode evitar tais efeitos. Efeitos similares, causados por inimigos, serão igualmente evitados com uma boa densidade de material.<BR><BR>Bônus de Destreza - Aumenta seu Atributo de Destreza pelo número de pontos no item.<BR><BR>Bônus de Durabilidade - Bônus de durabilidade são aplicados a um objeto uma vez. Um objeto mais durável leva mais tempo para se desgastar e quebrar.<BR><BR>Aprimorar Poções - Aumenta os efeitos das poções quando são usadas. Poções de veneno e visão noturna são excluídas.<BR><BR>Recuperação de Conjuração Rápida - Reduz o tempo de espera entre a conjuração de feitiços.<BR><BR>Conjuração Rápida - Diminui o tempo necessário para conjurar feitiços em 0,25 segundos por ponto.<BR><BR>Dano de Área no Acerto - Pode ser do tipo físico, fogo, frio, veneno ou energia. Fornece uma porcentagem de chance em cada acerto de causar dano de área adicional com base na metade do dano da arma infligido ao alvo primário. O dano de área não é infligido ao alvo original, mas é infligido a alvos atacáveis dentro de um raio de 5 tiles do alvo original.<BR><BR>Aumento de Chance de Acerto - Aumenta sua chance de acertar seus oponentes.<BR><BR>Dispersão no Acerto - Tem uma porcentagem de chance em cada acerto, baseada na habilidade de Táticas do usuário, de lançar o feitiço de magia dispersar em qualquer criatura convocada.<BR><BR>Bola de Fogo no Acerto - Tem uma porcentagem de chance em cada acerto de lançar o feitiço de magia bola de fogo no alvo.<BR><BR>Dano no Acerto - Tem uma porcentagem de chance em cada acerto de lançar o feitiço de magia dano no alvo.<BR><BR>Roubo de Vida no Acerto - Em cada acerto bem-sucedido, converte uma porcentagem do dano infligido pelo ataque em pontos de vida para o usuário.<BR><BR>Relâmpago no Acerto - Tem uma porcentagem de chance em cada acerto de lançar o feitiço de magia relâmpago no alvo.<BR><BR>Redução de Ataque no Acerto - Tem uma porcentagem de chance em cada acerto de reduzir a chance de acerto do alvo.<BR><BR>Redução de Defesa no Acerto - Tem uma porcentagem de chance em cada acerto de reduzir as capacidades defensivas do alvo.<BR><BR>Flecha Mágica no Acerto - Tem uma porcentagem de chance em cada acerto de lançar o feitiço de magia flecha mágica no alvo.<BR><BR>Dreno de Mana no Acerto - Reduz a mana do alvo em uma porcentagem do dano causado pelo ataque que desencadeia o efeito.<BR><BR>Roubo de Mana no Acerto - Em cada acerto bem-sucedido, converte uma porcentagem do dano infligido pelo ataque em pontos de mana para o usuário.<BR><BR>Aumento de Pontos de Vida - Aumenta seus pontos de vida máximos pelo número de pontos no item.<BR><BR>Regeneração de Pontos de Vida - Aumenta a taxa na qual você recupera pontos de vida.<BR><BR>Roubo de Vigor no Acerto - Tem uma porcentagem de chance em cada acerto de converter 100% do dano infligido no alvo em vigor para o usuário.<BR><BR>Bônus de Inteligência - Aumenta seu Atributo de Inteligência pelo número de pontos no item.<BR><BR>Custo de Munição Reduzido - Reduz o número de flechas/virotes usados em uma porcentagem." + lowmana + lowreg + "<BR><BR>Sorte - Aumenta a sorte do personagem, o que auxilia em eventos como encontrar tesouros melhores ou evitar armadilhas.<BR><BR>Armadura de Mago - Neutraliza impedimentos à meditação ativa e passiva de tipos de armadura que normalmente a bloqueariam. Também neutraliza impedimento à habilidade de furtividade.<BR><BR>Arma de Mago - Permite que a habilidade de magia substitua a habilidade de combate normal da arma. Movimentos especiais não podem ser usados via esta substituição. A habilidade de magia é reduzida enquanto uma arma de mago está equipada.<BR><BR>Aumento de Mana - Aumenta sua mana máxima pelo número de pontos no item.<BR><BR>Regeneração de Mana - Aumenta a taxa na qual você recupera mana, sujeito a retornos decrescentes.<BR><BR>Visão Noturna - Ajuda você a ver no escuro, mas também ajuda a encontrar tesouros escondidos em masmorras. Quanto mais itens de visão noturna você tiver equipado, maior a chance de encontrar tais tesouros escondidos.<BR><BR>Refletir Dano Físico - Refletir Dano Físico refletirá uma porcentagem de qualquer dano físico cinético que for infligido em você de volta para quem o infligiu.<BR><BR>Resistência - Os tipos de resistência são: físico/fogo/frio/veneno/energia. A resistência permite que você resista a uma porcentagem de todo dano descrito.<BR><BR>Auto-reparo - Tem uma chance de recuperar uma durabilidade, quando atingido durante o combate. Quanto melhor a densidade do item, mais isso reparará.<BR><BR>Bônus de Habilidade - Aumenta seus pontos de habilidade em uma habilidade específica.<BR><BR>Aniquilador - Armas e livros de feitiços causarão dano aumentado contra todas as criaturas dentro de um certo grupo, enquanto instrumentos musicais serão mais eficazes.<BR><BR>Canalização de Feitiço - Permite a conjuração de feitiços enquanto uma arma ou escudo está equipado.<BR><BR>Aumento de Dano de Feitiço - Aumenta a quantidade de dano que os feitiços infligem.<BR><BR>Aumento de Vigor - Aumenta seu vigor máximo pelo número de pontos no item.<BR><BR>Regeneração de Vigor - Aumenta a taxa na qual você recupera vigor.<BR><BR>Bônus de Força - Aumenta seu Atributo de Força pelo número de pontos no item.<BR><BR>Aumento de Velocidade de Ataque - Aumenta a velocidade base na qual você balança sua arma.<BR><BR>Usar Melhor Habilidade de Arma - Substitui a habilidade de arma treinada do personagem pela normalmente exigida para o tipo de arma, mas apenas para armas corpo a corpo. Arco e flecha e luta de punhos não estão incluídos.<BR><BR>Redução de Peso - Reduz o peso da munição contida em uma aljava.<BR><BR></BASEFONT></BODY>", (bool)false, (bool)true);
        }

		public override void OnResponse( NetState sender, RelayInfo info )
		{
			Mobile from = sender.Mobile;
			from.SendSound( 0x4A ); 
			if ( m_Origin > 0 ){ from.SendGump( new Server.Engines.Help.HelpGump( from, 1 ) ); }
		}
    }
}