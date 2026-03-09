using System;
using Server;
using Server.Items;
using System.Text;
using Server.Mobiles;
using Server.Gumps;
using System.Collections;
using Server.Network;
using Server.Spells;

namespace Server.Items
{
	public class WeaponAbilityBook : Item
	{
		[Constructable]
		public WeaponAbilityBook( ) : base( 0x2254 )
		{
			Weight = 1.0;
			Name = "Weapon Abilities";
		}

		public class AbilityBookGump : Gump
		{
			public AbilityBookGump( Mobile from ) : base( 100, 100 )
			{
				from.SendSound( 0x55 );

				Closable=true;
				Disposable=true;
				Dragable=true;
				Resizable=false;

				AddPage( 0 ); 
				AddImage( 80, 79, 11010, 0 );

				AddPage(1);
				AddButton( 401, 87, 0x89E, 0x89E, 18, GumpButtonType.Page, 2 );
				AddButton( 129, 87, 0x89D, 0x89D, 19, GumpButtonType.Page, 63 );
				AddHtml( 146, 111, 112, 75, @"<p align='center'>O Livro Completo da Maestria em Armas", (bool)false, (bool)false);
				AddImage(166, 193, 21248);
				AddHtml( 291, 108, 139, 163, @"Guerreiros têm a capacidade de usar sua Mana para executar manobras devastadoras com suas armas que podem ter uma variedade de efeitos colaterais incomuns. Cada arma terá uma", (bool)false, (bool)false);

				AddPage(2);
				AddButton( 401, 87, 0x89E, 0x89E, 18, GumpButtonType.Page, 3 );
				AddButton( 129, 87, 0x89D, 0x89D, 19, GumpButtonType.Page, 1 );
				AddHtml( 135, 108, 139, 163, @"combinação única de golpes especiais. Guerreiros que atingiram o nível 70 de habilidade em sua arma poderão executar o golpe especial primário da arma. Podem existir cinco", (bool)false, (bool)false);
				AddHtml( 291, 108, 139, 163, @"habilidades especiais totais para armas, alcançadas nos níveis 80, 90, 100 e 110 da habilidade com a arma. O nível de habilidade necessário pode até ser alcançado através do uso de itens que aumentam", (bool)false, (bool)false);

				AddPage(3);
				AddButton( 401, 87, 0x89E, 0x89E, 18, GumpButtonType.Page, 4 );
				AddButton( 129, 87, 0x89D, 0x89D, 19, GumpButtonType.Page, 2 );
				AddHtml( 135, 108, 139, 163, @"habilidades, como anéis, braceletes, botas, vestes, capas, cintos e brincos. Em todos os casos, outra habilidade é necessária para executar essas manobras. Esta é sempre tática, embora anatomia", (bool)false, (bool)false);
				AddHtml( 291, 108, 139, 163, @"possa às vezes ajudar. Sempre que você equipar uma arma, verá uma display de botões para selecionar e iniciar esses golpes especiais se sua habilidade permitir. Para ativar ou desativar um", (bool)false, (bool)false);

				AddPage(4);
				AddButton( 401, 87, 0x89E, 0x89E, 18, GumpButtonType.Page, 5 );
				AddButton( 129, 87, 0x89D, 0x89D, 19, GumpButtonType.Page, 3 );
				AddHtml( 135, 108, 139, 163, @"golpe especial, selecione o ícone e a fita ficará vermelha. Na primeira oportunidade, o golpe especial é executado e a fita retorna ao estado cinza. O Custo de Mana de cada golpe", (bool)false, (bool)false);
				AddHtml( 291, 108, 139, 163, @"especial pode ser reduzido se as habilidades do guerreiro forem altas o suficiente. Some os pontos de habilidade para Espadas, Pancadas, Esgrima, Atirar, Aparar, Luta de Punhos, Lenhador, Furtividade, Envenenamento, Bushido e", (bool)false, (bool)false);

				AddPage(5);
				AddButton( 401, 87, 0x89E, 0x89E, 18, GumpButtonType.Page, 6 );
				AddButton( 129, 87, 0x89D, 0x89D, 19, GumpButtonType.Page, 4 );
				AddHtml( 135, 108, 139, 163, @"Ninjitsu. Se o total estiver entre 200 e 299, subtraia 5 do Custo de Mana. Se o total for 300 ou mais, subtraia 10 do Custo de Mana. Alguns itens têm uma propriedade chamada 'menor", (bool)false, (bool)false);
				AddHtml( 291, 108, 139, 163, @"custo de mana'. Esses itens também reduzem o Custo de Mana desses Golpes Especiais. Se um golpe especial for tentado dentro de 3 segundos após outro golpe especial, o custo de mana desse golpe", (bool)false, (bool)false);

				AddPage(6);
				AddButton( 401, 87, 0x89E, 0x89E, 18, GumpButtonType.Page, 7 );
				AddButton( 129, 87, 0x89D, 0x89D, 19, GumpButtonType.Page, 5 );
				AddHtml( 135, 108, 139, 163, @"será dobrado. A barra de golpes especiais pode ter os nomes dos golpes especiais à direita dos ícones, se você assim desejar. Se quiser ativar ou desativar este recurso, simplesmente digite o", (bool)false, (bool)false);
				AddHtml( 291, 108, 139, 163, @"comando '[abilitynames' sem as aspas.", (bool)false, (bool)false);

				int counter = 0;
				int pages = 55;
				int nPage = 6;
				int myIcon = 0;
				string myAttack = "";
				int myMana = 0;
				string sMana = "";
				string myDescribe1 = "";
				string myDescribe2 = "";

				while ( counter < pages )
				{
					counter++;
					nPage++;
					myDescribe2 = "";

					switch( counter )
					{
						case 1:  myIcon = 0x1;		myAttack = "Achilles Strike";			myMana = 20; myDescribe1 = "Um golpe da arma irá ferir gravemente o tendão de Aquiles do alvo."; break;
						case 2:  myIcon = 0x2;		myAttack = "Armor Ignore";				myMana = 20; myDescribe1 = "Ignora as Resistências do Alvo mas causa dano ligeiramente inferior ao potencial máximo da arma."; break;
						case 3:  myIcon = 0x3;		myAttack = "Armor Pierce";				myMana = 20; myDescribe1 = "Golpeia teu inimigo com força perfurante de armadura e infligindo maior dano."; break;
						case 4:  myIcon = 0x4;		myAttack = "Bladeweave";				myMana = 10; myDescribe1 = "O guerreiro torna-se um com sua arma, permitindo que ela guie sua mão."; myDescribe2 = "Os efeitos deste ataque são imprevisíveis, mas eficazes (10+? Mana)."; break;
						case 5:  myIcon = 0x5;		myAttack = "Bleed Attack";				myMana = 30; myDescribe1 = "Faz o alvo sangrar profusamente, causando Dano Direto várias vezes ao longo"; myDescribe2 = "dos próximos segundos. A quantidade de Dano causado diminui a cada vez."; break;
						case 6:  myIcon = 0x6;		myAttack = "Block";						myMana = 20; myDescribe1 = "Aumenta tuas defesas por um curto período."; break;
						case 7:  myIcon = 0x7;		myAttack = "Concussion Blow";			myMana = 20; myDescribe1 = "Causa Dano Direto ao Alvo baseado na diferença entre seus atuais"; myDescribe2 = "Pontos de Vida e Mana. Quanto maior a diferença, mais Dano eles recebem."; break;
						case 8:  myIcon = 0x8;		myAttack = "Consecrated Strike";		myMana = 20; myDescribe1 = "Faz a arma causar a melhor quantidade de dano possível."; break;
						case 9:  myIcon = 0xA;		myAttack = "Crushing Blow";				myMana = 25; myDescribe1 = "Causa uma quantidade substancial extra de dano diretamente ao Alvo."; break;
						case 10: myIcon = 0xB;		myAttack = "Death Blow";				myMana = 50; myDescribe1 = "Permite-te desferir um golpe mortal."; break;
						case 11: myIcon = 0x11;		myAttack = "Defense Mastery";			myMana = 20; myDescribe1 = "Aumenta tua resistência física por um curto período enquanto reduz tua habilidade de infligir dano."; break;
						case 12: myIcon = 0x12;		myAttack = "Devastating Blow";			myMana = 30; myDescribe1 = "Permite-te desferir um golpe quase mortal."; break;
						case 13: myIcon = 0x13;		myAttack = "Disarm";					myMana = 20; myDescribe1 = "Desarma o Alvo e impede-o de rearmar qualquer arma por uma curta duração."; break;
						case 14: myIcon = 0x14;		myAttack = "Dismount";					myMana = 20; myDescribe1 = "Desaloja o Alvo de sua Montaria e causa uma quantidade moderada de Dano direto a eles."; break;
						case 15: myIcon = 0x15;		myAttack = "Disrobe";					myMana = 15; myDescribe1 = "Força teu alvo a perder sua roupa exterior."; break;
						case 16: myIcon = 0x16;		myAttack = "Double Shot";				myMana = 30; myDescribe1 = "Envia duas flechas voando em direção ao teu oponente."; break;
						case 17: myIcon = 0x17;		myAttack = "Double Strike";				myMana = 30; myDescribe1 = "O próximo Alvo que o usuário golpear será atingido pela arma duas vezes."; break;
						case 18: myIcon = 0x18;		myAttack = "Double Whirlwind Attack";	myMana = 25; myDescribe1 = "Atinge todos os inimigos no alcance, com dano bónus extra se houver muitos alvos ao teu redor."; break;
						case 19: myIcon = 0x19;		myAttack = "Drain Dexterity";			myMana = 25; myDescribe1 = "Drena a destreza do alvo quando ele é golpeado."; break;
						case 20: myIcon = 0x1A;		myAttack = "Drain Intellect";			myMana = 25; myDescribe1 = "Drena a inteligência do alvo quando ele é golpeado."; break;
						case 21: myIcon = 0x1B;		myAttack = "Drain Mana";				myMana = 25; myDescribe1 = "Drena a mana do alvo quando ele é golpeado."; break;
						case 22: myIcon = 0x1C;		myAttack = "Drain Stamina";				myMana = 25; myDescribe1 = "Drena a stamina do alvo quando ele é golpeado."; break;
						case 23: myIcon = 0x2B;		myAttack = "Drain Strength";			myMana = 25; myDescribe1 = "Drena a força do alvo quando ele é golpeado."; break;
						case 24: myIcon = 0x2C;		myAttack = "Dual Wield";				myMana = 30; myDescribe1 = "Ataca mais rápido enquanto balanças com ambas as armas."; break;
						case 25: myIcon = 0x2D;		myAttack = "Earth Strike";				myMana = 20; myDescribe1 = "Faz a arma causar uma quantidade extra de dano físico."; break;
						case 26: myIcon = 0x2E;		myAttack = "Elemental Strike";			myMana = 20; myDescribe1 = "Faz a arma causar uma quantidade extra de dano entre fogo, frio, energia e veneno."; break;
						case 27: myIcon = 0x30;		myAttack = "Feint";						myMana = 25; myDescribe1 = "Ganha uma vantagem defensiva sobre teu oponente principal por um curto período."; break;
						case 28: myIcon = 0x3E9;	myAttack = "Fire Strike";				myMana = 20; myDescribe1 = "Faz a arma causar uma quantidade extra de dano de fogo."; break;
						case 29: myIcon = 0x3EA;	myAttack = "Fists of Fury";				myMana = 20; myDescribe1 = "Ataca com ambos os punhos com muito mais eficácia."; break;
						case 30: myIcon = 0x3EB;	myAttack = "Force Arrow";				myMana = 20; myDescribe1 = "O arqueiro concentra sua vontade numa flecha de força pura, atordoando seu inimigo."; myDescribe2 = "Inimigos atordoados ficam temporariamente mais fáceis de acertar, e às vezes esquecem quem estão atacando."; break;
						case 31: myIcon = 0x3E8;	myAttack = "Force of Nature";			myMana = 40; myDescribe1 = "Infunde o atacante com a Fúria da Natureza. Este poder faz com que vinhas frondosas irrompam"; myDescribe2 = "debaixo da pele do atacante, causando dano físico e de veneno a eles."; break;
						case 32: myIcon = 0x3EC;	myAttack = "Freeze Strike";				myMana = 20; myDescribe1 = "Faz a arma causar uma quantidade extra de dano de frio."; break;
						case 33: myIcon = 0x3ED;	myAttack = "Frenzied Whirlwind";		myMana = 20; myDescribe1 = "Um ataque rápido a todos os inimigos no alcance de tua arma que causa dano ao longo do tempo."; break;
						case 34: myIcon = 0x3EE;	myAttack = "Greater Magic Protection";	myMana = 30; myDescribe1 = "Permite-te absorver uma grande quantidade de energia mágica."; break;
						case 35: myIcon = 0x3EF;	myAttack = "Greater Melee Protection";	myMana = 30; myDescribe1 = "Permite-te absorver uma grande quantidade de dano físico."; break;
						case 36: myIcon = 0x3F0;	myAttack = "Infectious Strike";			myMana = 15; myDescribe1 = "Tenta aplicar o veneno de uma arma envenenada ao Alvo. Quanto maior a habilidade de Envenenamento do"; myDescribe2 = "usuário, maior a chance de a força do Veneno aplicado ser aumentada em um nível."; break;
						case 37: myIcon = 0x3F1;	myAttack = "Lightning Arrow";			myMana = 20; myDescribe1 = "Uma flecha carregada que arremessa relâmpagos nos aliados do seu alvo."; break;
						case 38: myIcon = 0x3F2;	myAttack = "Lightning Strike";			myMana = 20; myDescribe1 = "Faz a arma causar uma quantidade extra de dano de energia."; break;
						case 39: myIcon = 0x3F3;	myAttack = "Magic Protection";			myMana = 25; myDescribe1 = "Permite-te absorver uma boa quantidade de energia mágica."; break;
						case 40: myIcon = 0x3F4;	myAttack = "Melee Protection";			myMana = 25; myDescribe1 = "Permite-te absorver uma boa quantidade de dano físico."; break;
						case 41: myIcon = 0x3F5;	myAttack = "Mortal Strike";				myMana = 25; myDescribe1 = "Impede o Alvo de ser curado por qualquer meio por alguns Segundos."; myDescribe2 = "Este efeito não impede um Alvo de regenerar pontos de vida."; break;
						case 42: myIcon = 0x3F6;	myAttack = "Moving Shot";				myMana = 15; myDescribe1 = "Permite ao arqueiro disparar uma flecha ou virote enquanto se move."; myDescribe2 = "Normalmente um Arqueiro deve estar Parado para disparar em um Alvo."; break;
						case 43: myIcon = 0x3F7;	myAttack = "Nerve Strike";				myMana = 20; myDescribe1 = "Causa dano e paralisa teu oponente por um curto período."; break;
						case 44: myIcon = 0x3F8;	myAttack = "Paralyzing Blow";			myMana = 20; myDescribe1 = "Paralisa o alvo por alguns segundos."; break;
						case 45: myIcon = 0x3F9;	myAttack = "Psychic Attack";			myMana = 30; myDescribe1 = "Encanta temporariamente a arma do atacante com energia psíquica mortal,"; myDescribe2 = "permitindo danificar a mente do defensor e sua habilidade de infligir dano com magia."; break;
						case 46: myIcon = 0x3FA;	myAttack = "Riding Attack";				myMana = 20; myDescribe1 = "Dá aos teus ataques a cavalo um efeito muito mais mortal."; break;
						case 47: myIcon = 0x3FB;	myAttack = "Riding Swipe";				myMana = 30; myDescribe1 = "Se estás a pé, desmonta teu oponente e danifica o cavaleiro etéreo ou a montaria viva"; myDescribe2 = "(que deve ser curada antes de ser montada novamente)."; break;
						case 48: myIcon = 0x3FC;	myAttack = "Serpent Arrow";				myMana = 30; myDescribe1 = "Dispara uma serpente no alvo, envenenando-o além do dano normal com um acerto bem-sucedido."; myDescribe2 = "O arqueiro deve ser habilidoso em envenenamento e ágil de mãos para obter sucesso."; break;
						case 49: myIcon = 0x3FD;	myAttack = "Shadow Infectious Strike";	myMana = 25; myDescribe1 = "Tenta aplicar o veneno de uma arma envenenada ao Alvo quando furtivo e escondido. Quanto maior a"; myDescribe2 = "habilidade de Envenenamento do usuário, maior a chance de a força do Veneno aplicado ser aumentada em um nível."; break;
						case 50: myIcon = 0x3FE;	myAttack = "Shadow Strike";				myMana = 20; myDescribe1 = "Este ataque causa Dano extra moderado ao Alvo e esconde imediatamente o"; myDescribe2 = "usuário. Para tentar um Golpe das Sombras deves ter uma alta quantidade da habilidade Furtividade."; break;
						case 51: myIcon = 0x3FF;	myAttack = "Spin Attack";				myMana = 20; myDescribe1 = "Faz com que alguém gire muito rapidamente, acertando múltiplas vezes com sua arma."; break;
						case 52: myIcon = 0x400;	myAttack = "Stunning Strike";			myMana = 20; myDescribe1 = "Um golpe com uma arma deixará seriamente atordoado."; break;
						case 53: myIcon = 0x401;	myAttack = "Talon Strike";				myMana = 30; myDescribe1 = "Ataca com dano aumentado com dano adicional ao longo do tempo."; break;
						case 54: myIcon = 0x402;	myAttack = "Toxic Strike";				myMana = 20; myDescribe1 = "Faz a arma causar uma quantidade extra de dano de veneno."; break;
						case 55: myIcon = 0x403;	myAttack = "Whirlwind Attack";			myMana = 15; myDescribe1 = "Ataca todos os Alvos válidos dentro de um raio de um tile do atacante."; break;
					}
					AddPage( nPage ); 

					AddButton( 401, 87, 0x89E, 0x89E, 18, GumpButtonType.Page, (nPage+1) );
					AddButton( 129, 87, 0x89D, 0x89D, 19, GumpButtonType.Page, (nPage-1) ); 

					int mana = CalculateMana( from, myMana );

					sMana = mana.ToString();

					AddImage(296, 95, 0x5DD0);
					AddImage(296, 95, myIcon);
					AddHtml( 137, 113, 134, 44, @"" + myAttack + "", (bool)false, (bool)false);
					AddHtml( 347, 115, 80, 19, @"Mana: " + sMana + "", (bool)false, (bool)false);
					AddHtml( 137, 160, 131, 99, @"" + myDescribe1 + "", (bool)false, (bool)false);
					AddHtml( 297, 145, 131, 121, @"" + myDescribe2 + "", (bool)false, (bool)false);
				}

				AddPage(62);
				AddButton( 401, 87, 0x89E, 0x89E, 18, GumpButtonType.Page, 63 );
				AddButton( 129, 87, 0x89D, 0x89D, 19, GumpButtonType.Page, 61 );
				AddHtml( 135, 108, 139, 163, @"Há outras maneiras de usar estes comandos: '[sad' abrirá a barra de ferramentas, caso a feches tu mesmo. Claro que ela reabrirá se", (bool)false, (bool)false);
				AddHtml( 291, 108, 139, 163, @"simplesmente desequipares e reequipares a arma novamente. Se desejas criar macros para iniciar estas habilidades, precisarás apenas usar os comandos '[set1', '[set2',", (bool)false, (bool)false);

				AddPage(63);
				AddButton( 401, 87, 0x89E, 0x89E, 18, GumpButtonType.Page, 1 );
				AddButton( 129, 87, 0x89D, 0x89D, 19, GumpButtonType.Page, 62 );
				AddHtml( 135, 108, 139, 163, @"'[set3', '[set4', ou '[set5'... dependendo de qual habilidade desejas usar. Este livro é apenas para referência, e não precisas carregá-lo para usar estas", (bool)false, (bool)false);
				AddHtml( 291, 108, 139, 163, @"habilidades especiais.", (bool)false, (bool)false);
			}
		}

		public static int CalculateMana( Mobile from, int Power )
		{
			int mana = Power;

			double skillTotal = GetSkill( from, SkillName.Swords ) + GetSkill( from, SkillName.Bludgeoning )
				+ GetSkill( from, SkillName.Fencing ) + GetSkill( from, SkillName.Marksmanship ) + GetSkill( from, SkillName.Parry )
				+ GetSkill( from, SkillName.Lumberjacking ) + GetSkill( from, SkillName.Stealth )
				+ GetSkill( from, SkillName.Poisoning ) + GetSkill( from, SkillName.Bushido ) + GetSkill( from, SkillName.Ninjitsu );

			if ( skillTotal >= 300.0 )
				mana -= 10;
			else if ( skillTotal >= 200.0 )
				mana -= 5;

			double scalar = 1.0;
			if ( !Server.Spells.Necromancy.MindRotSpell.GetMindRotScalar( from, ref scalar ) )
				scalar = 1.0;

			// Lower Mana Cost = 40%
			int lmc = Math.Min( AosAttributes.GetValue( from, AosAttribute.LowerManaCost ), 40 );

			scalar -= (double)lmc / 100;
			mana = (int)(mana * scalar);

			return mana;
		}

		public static double GetSkill( Mobile from, SkillName skillName )
		{
			Skill skill = from.Skills[skillName];

			if ( skill == null )
				return 0.0;

			return skill.Value;
		}

		public override void OnDoubleClick( Mobile e )
		{
			e.SendGump( new AbilityBookGump( e ) );
		}

		public WeaponAbilityBook(Serial serial) : base(serial)
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