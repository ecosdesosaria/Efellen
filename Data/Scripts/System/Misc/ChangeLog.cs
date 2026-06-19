using System;
using Server;
using System.Collections;
using Server.Misc;
using Server.Network;
using Server.Commands;
using Server.Commands.Generic;
using Server.Mobiles;
using Server.Accounting;
using Server.Regions;
using Server.Targeting;
using System.Collections.Generic;
using Server.Items;
using Server.Spells.Fifth;
using System.IO;
using System.Xml;

namespace Server.Misc
{
    class ChangeLog
    {
		public static string Version()
		{
			return "Version: Lolth's Gift (6th of June 2026)";
		}

		public static string Versions()
        {
			string versionTEXT = ""

       
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        +"Wrath of Ashardalom - Xº de Y de Z<BR>"
		+" <BR>"
		+"  * Progressão, balanceamento de jogo e Sistemas:<BR>"
		+"• Sistema de Marca do Caçador:"
		+"  • Adicionado o comando [HuntersMark, que permite que rastreadores habilidosos ganhem um bônus de dano contra sua presa baseado em seu nível de habilidade.<BR>"
		+"  • Quando a Marca do Caçador está ativa em um alvo, o rastreador pode aumentar sua habilidade de rastreamento ao acertá-lo. A Marca do Caçador tem 1 minuto de recarga entre usos, e também pode ser ativada pelo uso regular da habilidade de rastreamento.<BR>"
		+"• Vários feitiços de classes desbloqueáveis (místico, cavaleiro da morte, sacerdote sagrado, shinobi) foram fortalecidos e rebalanceados.<BR>"
		+"• Livros de feitiços de extermínio e armas de longo alcance agora adicionam 70% de dano base (antes 100%).<br>"
		+"• Adicionados ataques especiais faltantes ao Khumash Gor.<br>"
		+" <BR>"
		+"  * Itens e criação:<BR>"
		+"• Adicionados vários novos artefatos como recompensa de chefes, drop global e recompensas de missões de sábio.<BR>"
		+"• Carpinteiros agora podem criar caixas de treino de arrombamento.<br>"
		+" <BR>"
		+"  * Qualidade de Vida:<BR>"
		+"• Livros de feitiços para magos, necromantes e elementalistas, e livros de canções agora têm uma entrada de menu de contexto 'configurar' que coletará automaticamente pergaminhos faltantes do inventário do jogador e os adicionará ao livro.<BR>"
		+" <BR>"

		+ sepLine()

		+"Lolth's Gift - 6 de Junho de 2026<BR>"
		+" <BR>"
		+"  * Novos Chefes foram adicionados a várias regiões do jogo:<BR>"
		+"• Cada chefe pode dropar itens temáticos e poderosos, pergaminhos de habilidade e uma variedade de tesouros.<BR>"
		+"• Os chefes são divididos em níveis que representam sua dificuldade geral e as recompensas por derrotá-los.<BR>"
		+"  • Nível 1 (Chefes amigáveis para solo, para aventureiros estabelecidos):<BR>"
		+"• A Madre Superiora defende seu convento a leste da cidade de Grey.<BR>"
		+"• Blacktooth, o Urso-Troll, ataca caravanas do sul de Sosaria nas florestas ao redor de Montor.<BR>"
		+"• O Açougueiro foi trazido para o serviço dos magos loucos de Ravendark.<BR>"
		+"  • Nível 2 (Chefes Solo/Pequeno Grupo):<BR>"
		+"• Firefang, o Chefe de Guerra, está incendiando o interior próximo à cidade de Moon.<BR>"			
		+"• Mãe dos Esporos comanda forças psíquicas nas Cavernas dos Micônidas, no leste do continente de Sosaria.<BR>"
		+"• Fiorin, o Arquidruida, lidera sua alcateia na defesa do Bosque Uivante.<BR>"
		+"• Black Phillip reúne seu pacto nas Ilhas do Pavor.<BR>"
		+"  • Nível 3 (Chefes desafiadores projetados para grupos ou aventureiros muito poderosos): <BR>"
		+"• Caelan, o Cavaleiro do Pavor, medita em seu castelo enquanto o sangue escorre de suas vítimas.<BR>"
		+"• Hrimah, Punho do Norte, governa a Cicatriz Glacial das profundezas da fortaleza.<BR>"
		+"• A Filha do Fogo reivindica residência nos fogos do Inferno.<BR>"
		+"• O Rei Esqueleto despertou na Pirâmide Antiga, e sua corte atendeu seu chamado.<BR>"
		+"  • Nível 4 (Chefes poderosos para grupos): <BR>"
		+"• O fantasma de Bal Tsareth assombra seu santuário mais uma vez, após uma expedição perturbar seu descanso.<BR>"
		+"• O Tecelão de Sonhos ameaça Lodoria de seu covil, no fundo de uma caverna cujas próprias paredes foram levadas à loucura.<BR>"
		+"• Velho Caolho vagueia pelos ermos do Império Selvagem, inconteste entre os predadores daquela terra abandonada.<BR>"
		+"• Tecelão do Destino serve sua rainha nas cavernas cintilantes de Fanaedar.<BR>"
		+"• Xyrtaxis, Decano das Artes Negras, ensina na Academia Arcana de Fanaedar.<BR>"
		+"• Annath, Sudário dos Sem-Luz, prega os mistérios de Lolth em Fanaedar.<BR>"
		+"• Waervaerendor e Voaraghamanthar, os gêmeos dragões negros, agora lideram o culto no templo de Osirus.<BR>"
		+"  • Nível 5 (para grupos muito otimizados): <BR>"
		+"• Teel Fanae governa a cidade da rainha aranha com punho de ferro.<br> "
		+"• O Príncipe das Trevas governa em Ravendark (RIP Ozzy).<BR>"
		+"• O Marechal Celestial protege o sul de Sosaria do castelo Griffin's Roost.<BR>"
		+"• O Arauto das Brasas vigia sua prole em Destard.<BR>"
		+"  • Nível 6+ (para grupos de masoquistas:)<BR>"
		+"• A Rainha dos Poços da Teia Demoníaca está solitária e adoraria um pouco de companhia.<BR>"
		+" <BR>"
		+"  * Reformulação da Criação:<BR>"
		+"• Os bônus de criação de diferentes materiais foram completamente reconstruídos para uma progressão mais simplificada e menos inchaço de propriedades.<BR>"
		+"• As resistências de Armaduras Excepcionais foram significativamente reduzidas.<BR>"
		+"• Armaduras feitas por carpinteiros agora sempre têm a propriedade Armadura de Mago.<BR>"
		+"• Removidos todos os materiais de criação alienígenas.<BR>"
		+"• Roupas criadas herdam o tom do tecido usado.<BR>"
		+" <BR>"
		+"  * Artefatos:<BR>"
		+"• Todas as armas de artefato agora apresentam ataques especiais impactantes (invocações, explosões em área, DoTs).<BR>" 
		+"• Os bônus dos artefatos foram ajustados para complementar os efeitos especiais.<BR>"
		+"• Os bônus de habilidade em artefatos são mais raros e especializados.<BR>"
		+"• Muitos artefatos redundantes foram removidos.<BR>"
		+"• Todas as bases de armas agora devem ter um ou dois artefatos.<BR>"
		+"• Adicionados novos artefatos como drops de chefes, recompensas de marcas e drops globais.<BR>"
		+"• A interface dos livros de busca de missões de sábio e artefatos lendários de titã foi reformulada para causar menos sofrimento.<BR>"
		+" <BR>"
		+"  * Personalização:<BR>"
		+"• O arquivo de configurações foi significativamente simplificado, para reduzir as chances do desenvolvedor ter um derrame precoce.<BR>"
		+" <BR>"
		+"  * NPCs:<BR>"
		+"• Oliver (ao sul de Britain) troca recompensas por poções de mudança de gênero.<BR>"
		+"• Adicionadas cerca de 400 novas falas de diálogo para NPCs.<BR>"
		+"• NPCs de treino agora ensinam habilidades até 50 (antes 32).<BR>"
		+"• Exodus não deleta mais automaticamente os animais de estimação que atacam.<BR>"
		+"• Beholders agora são mais raros, mais fortes, com ataques perigosos de olhos.<BR>"
		+"• Muitos inimigos únicos ganharam ataques especiais dinâmicos.<BR>"
		+"• Ladrões inimigos agora roubam apenas ouro (em vez de itens aleatórios de suas bolsas).<BR>"
		+"• Conjuradores inimigos reformulados com listas de feitiços baseadas em classes (cerca de 100 novos feitiços de D&D 3.5 foram importados para o jogo):<BR>"
		+"  • Druidas invocam animais, Magos lançam feitiços arcanos, Clérigos golpeiam e curam, Bardos enfraquecem e assim por diante.<BR>"
		+"• A maioria dos conjuradores não se cura mais completamente ao serem derrotados.<BR>"
		+" <BR>"
		+"  * Progressão e Sistemas:<BR>"
		+"• Ascensões foram adicionadas ao jogo. Este sistema adiciona muitas novas classes e habilidades e uma nova forma de progressão para personagens poderosos.<br>"
		+"  • Ascensões sobem de nível conforme você derrota inimigos enquanto estão ativas. Conforme um personagem sobe de nível em uma ascensão, ele ganhará até 4 habilidades ativas e 4 passivas. Ascensões podem chegar ao nível 20.<br>"
		+"  • Um personagem pode ter múltiplas ascensões, mas apenas uma pode estar ativa por vez.<br>"
		+"  • use [Ascension para ver uma lista das opções atualmente disponíveis, seus requisitos e benefícios.<br>"
		+"  • as seguintes ascensões estão atualmente implementadas: Arcane Archer, Archmage, Assassin, Berserker, Blackguard, Crusader, Hierophant, Kensai, Palemaster, Reaver, Skald.<br>"
		+"• Tomo do Poder e Tomo da Ascensão foram adicionados aos vendedores de marcas (2.000 marcas cada por armazenamento ilimitado de seu tipo de pergaminho).<BR>"
		+"• Um Sistema de Encantamento de Armas de fim de jogo foi adicionado a Fanaedar: Requer que você troque 20 Essências do Ódio de Lolth na piscina sacrificial para aprimorar armas de artefato, a um alto custo para si mesmo.<BR>"
		+"• Orbes dos Poços da Teia Demoníaca podem ser encontrados na masmorra de mesmo nome. Eles podem adicionar 25 pontos de encantamento a armaduras e roupas de artefato regulares e lendárias.<BR>"
		+"• Marcas da Trama: Moeda de recompensa da Guilda dos Magos por derrotar conjuradores e pesquisar tomos.<BR>"
		+"• Marcas de Devoção: Moeda de recompensa da Guilda dos Curandeiros por abater mortos-vivos e curar na Casa da Santa Misericórdia.<BR>"
		+"• Marcas das Selvas: Moeda de recompensa da Guilda dos Druidas por se aventurar com animais de estimação, contratos de doma e meditação no Bosque Uivante.<BR>"
		+"• Expansão do Arquétipo de Druida:<BR>"
		+"  • Druidas de alta habilidade ganham imunidade a veneno.<BR>"
		+"  • Sistema de Forma Selvagem: Medite no Bosque Uivante para ter a chance de adquirir um 'Coração das Selvas', que é um talismã usado para transmorfar.<BR>"
		+"    • Desbloqueie formas através de estudo, combate ou companhia de animais de estimação.<BR>"
		+"    • Personagens transmorfados não podem usar magia não-elemental ou usar armaduras de metal.<BR>"
		+"    • Cada forma tem ataques especiais únicos e requisitos de habilidade (Espiritualismo/Druidismo, às vezes uma terceira habilidade).<BR>"
		+"• Feitiços de área (Devastação Elemental, Apocalipse, Queda, Cadeia de Relâmpagos, Enxame de Meteoros) não atingem mais membros do grupo; a escalabilidade de dano em múltiplos alvos foi corrigida.<BR>"
		+"• Todos os anéis de guilda agora concedem 30 pontos de habilidade; alguns bônus foram ajustados para consistência temática.<BR>"
		+"• O bônus de dano de Ninjitsu agora se aplica apenas a armas de esgrima<BR>."
		+"• Armadilhas que distorcem karma redefinem o karma para zero (em vez de invertê-lo).<BR>"
		+"• Mudanças no sistema de veneno:<BR>"
		+"  • Cargas máximas de veneno em armas: 25.<BR>"
		+"  • A habilidade de Envenenamento concede até 25% de chance de preservar cargas ao acertar.<BR>"
		+"  • Usuários de veneno altamente habilidosos agora podem infligir uma penalidade na resistência a veneno dos alvos.<BR>"
		+"• O tempo de logout foi reduzido para 30 segundos (antes 5 minutos).<BR>"
		+"• As recompensas de contratos de doma foram rebalanceadas e não geram mais animais de estimação extremamente feios.<BR>"
		+"• Vendedores de marcas agora têm o menu de contexto 'Recompensas' para jogadores elegíveis.<BR>"
		+"• Adicionado o comando [SkillDrop, que permite que os jogadores reduzam suas habilidades sob demanda.<BR>"
		+ "<BR>"
		+"  * Localizações:<BR>"
		+"• Bloodstone Keep (Sosaria) substitui um dos Acampamentos Orcs em Sosaria como uma fortaleza inimiga de alto nível.<BR>"
		+"• O Fosso de Dardin (Sosaria) foi expandido com novas salas, pool de spawns reconstruído e mini-chefe.<BR>"		
		+"• Fanaedar (Submundo): Cidade Drow de fim de jogo massiva com pool de loot único e 4 chefes de grupo.<BR>"
		+"• Poços da Teia Demoníaca (Submundo): Lar da própria rainha aranha, projetado para os mais corajosos dos corajosos.<BR>"
		+"• Cidadela sem Sol (Sosaria): Local de nível inicial para personagens de masmorras pós-iniciantes, inspirado no módulo de D&D 3E, com um mini-chefe.<BR>"
		+"• Colmeia do Tirano dos Olhos (Lodoria): Masmorra de dificuldade escondida em uma das cavernas em Lodoria.<BR>"
		+"• Castelo Griffin's Roost (Sul de Sosaria): Cavaleiros da lei desafiam aventureiros malignos nesta fortaleza.<BR>"
		+"• Cavernas dos Micônidas (Leste de Sosaria): Pequena caverna tomada por cogumelos.<BR>"
		+"• Bosque Uivante (Oeste de Sosaria): Santuário druida com espíritos de lobo.<BR>"
		+"• Casa da Santa Misericórdia (Sosaria): Hospital/convento para treino de cura e cuidado de pacientes.<BR>"
		+"• Destard: Tem uma nova arena com um chefe poderoso para lutar, seus spawns foram rebalanceados e sua dificuldade aumentada para Difícil.<BR>"
		+"• Todas as masmorras fáceis ganharam uma sala extra com inimigos mais fortes e agora têm pools de spawn condensados.<BR>"
		+"• Aprendizes de Mago foram adicionados a masmorras de iniciantes para ajudar com livros de feitiços e ensinar combate de conjurador.<BR>"
		+"• Fogos do Inferno: Dobrou de tamanho com novos habitantes, drops especiais de armas flamejantes e dois mini-chefes.<BR>"
		+"• Pirâmide Antiga: Redesign interna e externa importante, novos habitantes e mini-chefe e muitos novos desafios.<BR>"
		+"• Biblioteca de Bal Tsareth (antiga 'Pistas'): Pool de mobs, encontros e lore reconstruídos com nova missão do líder da expedição.<BR>"
		+" <BR>"
		+ sepLine()

			+ "";

			return versionTEXT;
		}

		public static string sepLine()
		{
			return "---------------------------------------------------------------------------------<BR><BR>";
		}
	}
}