using Server;
using System;
using Server.Spells;
using System.Text;
using System.Collections;
using Server.Network;
using Server.Targeting;
using Server.Mobiles;
using System.Globalization;

namespace Server.Items
{
	public class SpellItemInfo
	{
		private MagicSpell m_MagicSpell;
		private int m_SpellID;
		private Type m_ScrollType;
		private string m_SpellName;
		private string m_Description;
		private string m_Circle;
		private int m_Icon;
		private string m_Runes;

		public MagicSpell MageSpell{ get{ return m_MagicSpell; } }
		public int SpellID{ get{ return m_SpellID; } }
		public Type ScrollType{ get{ return m_ScrollType; } }
		public string SpellName{ get{ return m_SpellName; } }
		public string Description{ get{ return m_Description; } }
		public string Circle{ get{ return m_Circle; } }
		public int Icon{ get{ return m_Icon; } }
		public string Runes{ get{ return m_Runes; } }

		public SpellItemInfo( MagicSpell magic, int id, Type scrollType, string name, string desc, string circle, int icon, string runes )
		{
			m_MagicSpell = magic;
			m_SpellID = id;
			m_ScrollType = scrollType;
			m_SpellName = name;
			m_Description = desc;
			m_Circle = circle;
			m_Icon = icon;
			m_Runes = runes;
		}
	}

	public class SpellItems
	{
		private static SpellItemInfo[] m_MagicInfo = new SpellItemInfo[]																														
		{
			new SpellItemInfo( MagicSpell.None, 0, typeof( BlankScroll ), "", "", "", 0, "" ),
			new SpellItemInfo( MagicSpell.Clumsy, 0, typeof( ClumsyScroll ), "clumsy", "Reduz temporariamente a Destreza do Alvo.", "First", 2240, "Uus Jux" ),
			new SpellItemInfo( MagicSpell.CreateFood, 1, typeof( CreateFoodScroll ), "create food", "Cria um item de comida aleatório na mochila do Conjurador.", "First", 2241, "In Mani Ylem" ),
			new SpellItemInfo( MagicSpell.Feeblemind, 2, typeof( FeeblemindScroll ), "feeblemind", "Reduz temporariamente a Inteligência do Alvo.", "First", 2242, "Rel Wis" ),
			new SpellItemInfo( MagicSpell.Heal, 3, typeof( HealScroll ), "heal", "Cura uma pequena quantidade de Pontos de Vida perdidos do Alvo.", "First", 2243, "In Mani" ),
			new SpellItemInfo( MagicSpell.MagicArrow, 4, typeof( MagicArrowScroll ), "magic arrow", "Dispara uma flecha mágica no Alvo, que causa dano de Fogo.", "First", 2244, "In Por Ylem" ),
			new SpellItemInfo( MagicSpell.NightSight, 5, typeof( NightSightScroll ), "night sight", "Permite temporariamente que o Alvo veja na escuridão.", "First", 2245, "In Lor" ),
			new SpellItemInfo( MagicSpell.ReactiveArmor, 6, typeof( ReactiveArmorScroll ), "reactive armor", "Aumenta a Resistência Física do Conjurador enquanto reduz suas Resistências Elementais. A habilidade de Inscrição do Conjurador adiciona um bônus à quantidade de Resistência Física aplicada. Ativo até que o feitiço seja desativado ao ser relançado no mesmo Alvo.", "First", 2246, "Flam Sanct" ),
			new SpellItemInfo( MagicSpell.Weaken, 7, typeof( WeakenScroll ), "weaken", "Reduz temporariamente a Força do Alvo.", "First", 2247, "Des Mani" ),
			new SpellItemInfo( MagicSpell.Agility, 8, typeof( AgilityScroll ), "agility", "Aumenta temporariamente a Destreza do Alvo.", "Second", 2248, "Ex Uus" ),
			new SpellItemInfo( MagicSpell.Cunning, 9, typeof( CunningScroll ), "cunning", "Aumenta temporariamente a Inteligência do Alvo.", "Second", 2249, "Uus Wis" ),
			new SpellItemInfo( MagicSpell.Cure, 10, typeof( CureScroll ), "cure", "Tenta neutralizar venenos que afetam o Alvo.", "Second", 2250, "An Nox" ),
			new SpellItemInfo( MagicSpell.Harm, 11, typeof( HarmScroll ), "harm", "Afasta o Alvo com um efeito gelado, causando dano de Gelo. Quanto mais próximo o Alvo estiver do Conjurador, mais dano é causado.", "Second", 2251, "An Mani" ),
			new SpellItemInfo( MagicSpell.MagicTrap, 12, typeof( MagicTrapScroll ), "magic trap", "Coloca uma proteção mágica explosiva em um recipiente que causa dano de Fogo à próxima pessoa que o abrir. Você também pode mirar no chão e colocar uma armadilha aleatória para os descuidados.", "Second", 2252, "In Jux" ),
			new SpellItemInfo( MagicSpell.RemoveTrap, 13, typeof( MagicUnTrapScroll ), "magic untrap", "Desativa uma armadilha mágica em um recipiente, ou você pode conjurar em si mesmo para invocar um orbe de detecção de armadilhas. O orbe do item permaneceria em sua mochila e o ajudaria a evitar armadilhas ocultas.", "Second", 2253, "An Jux" ),
			new SpellItemInfo( MagicSpell.Protection, 14, typeof( ProtectionScroll ), "protection", "Impede que os feitiços do Alvo sejam interrompidos, mas diminui sua Resistência Física e Resistência Mágica. Ativo até que o feitiço seja desativado ao ser relançado no mesmo Alvo.", "Second", 2254, "Uus Sanct" ),
			new SpellItemInfo( MagicSpell.Strength, 15, typeof( StrengthScroll ), "strength", "Aumenta temporariamente a Força do Alvo.", "Second", 2255, "Uus Mani" ),
			new SpellItemInfo( MagicSpell.Bless, 16, typeof( BlessScroll ), "bless", "Aumenta temporariamente a Força, Destreza e Inteligência do Alvo.", "Third", 2256, "Rel Sanct" ),
			new SpellItemInfo( MagicSpell.Fireball, 17, typeof( FireballScroll ), "fireball", "Dispara uma bola de chamas violentas em um Alvo, causando dano de Fogo.", "Third", 2257, "Vas Flam" ),
			new SpellItemInfo( MagicSpell.MagicLock, 18, typeof( MagicLockScroll ), "magic lock", "Tranca magicamente um recipiente ou porta de masmorra, ou também pode prender a alma de uma criatura em um frasco de ferro.", "Third", 2258, "An Por" ),
			new SpellItemInfo( MagicSpell.Poison, 19, typeof( PoisonScroll ), "poison", "O Alvo é afligido por veneno, de uma força determinada pela Magia e Habilidade de Veneno do Conjurador, e pela distância do Alvo.", "Third", 2259, "In Nox" ),
			new SpellItemInfo( MagicSpell.Telekinesis, 20, typeof( TelekinisisScroll ), "telekinisis", "Permite que o Conjurador Use um item à distância. Você também pode pegar objetos menores à distância e colocá-los em sua mochila.", "Third", 2260, "Ort Por Ylem" ),
			new SpellItemInfo( MagicSpell.Teleport, 21, typeof( TeleportScroll ), "teleport", "O Conjurador é transportado para o Local Alvo.", "Third", 2261, "Rel Por" ),
			new SpellItemInfo( MagicSpell.Unlock, 22, typeof( UnlockScroll ), "unlock", "Destranca uma fechadura mágica ou fechadura normal de baixo nível.", "Third", 2262, "Ex Por" ),
			new SpellItemInfo( MagicSpell.WallOfStone, 23, typeof( WallOfStoneScroll ), "wall of stone", "Cria uma parede temporária de pedra que bloqueia o movimento.", "Third", 2263, "In Sanct Ylem" ),
			new SpellItemInfo( MagicSpell.ArchCure, 24, typeof( ArchCureScroll ), "arch cure", "Neutraliza venenos em todos os personagens dentro de um pequeno raio ao redor do conjurador.", "Fourth", 2264, "Vas An Nox" ),
			new SpellItemInfo( MagicSpell.ArchProtection, 25, typeof( ArchProtectionScroll ), "arch protection", "Aplica o feitiço Proteção a todos os alvos válidos dentro de um pequeno raio ao redor do Local Alvo.", "Fourth", 2265, "Vas Uus Sanct" ),
			new SpellItemInfo( MagicSpell.Curse, 26, typeof( CurseScroll ), "curse", "Diminui a Força, Destreza e Inteligência do Alvo. Quando conjurado durante combate Jogador vs. Jogador, o feitiço também reduz os valores máximos de resistência do alvo.", "Fourth", 2266, "Des Sanct" ),
			new SpellItemInfo( MagicSpell.FireField, 27, typeof( FireFieldScroll ), "fire field", "Invoca uma parede de fogo que causa dano de Fogo a todos que passarem por ela", "Fourth", 2267, "In Flam Grav" ),
			new SpellItemInfo( MagicSpell.GreaterHeal, 28, typeof( GreaterHealScroll ), "greater heal", "Cura uma quantidade média de Pontos de Vida perdidos do alvo.", "Fourth", 2268, "In Vas Mani" ),
			new SpellItemInfo( MagicSpell.Lightning, 29, typeof( LightningScroll ), "lightning", "Ataca o Alvo com um raio, que causa dano de Energia.", "Fourth", 2269, "Por Ort Grav" ),
			new SpellItemInfo( MagicSpell.ManaDrain, 30, typeof( ManaDrainScroll ), "mana drain", "Remove temporariamente uma quantidade de mana do Alvo, baseada numa comparação entre a habilidade de Psicologia do Conjurador e a habilidade de Resistência Mágica do Alvo.", "Fourth", 2270, "Ort Rel" ),
			new SpellItemInfo( MagicSpell.Recall, 31, typeof( RecallScroll ), "recall", "O Conjurador é transportado para a localização marcada na runa Alvo. Se uma chave de navio for alvo, o Conjurador é transportado para o barco que a chave abre.", "Fourth", 2271, "Kal Ort Por" ),
			new SpellItemInfo( MagicSpell.BladeSpirits, 32, typeof( BladeSpiritsScroll ), "blade spirits", "Invoca um pilar giratório de lâminas que seleciona um Alvo para atacar com base em sua força de combate e proximidade. O Espírito de Lâmina desaparece após um tempo determinado. Requer 2 slots de controle de animais.", "Fifth", 2272, "In Jux Hur Ylem" ),
			new SpellItemInfo( MagicSpell.DispelField, 33, typeof( DispelFieldScroll ), "dispel field", "Destrói um tile de um feitiço de Campo alvo.", "Fifth", 2273, "An Grav" ),
			new SpellItemInfo( MagicSpell.Incognito, 34, typeof( IncognitoScroll ), "incognito", "Disfarça o Conjurador com uma aparência e nome gerados aleatoriamente.", "Fifth", 2274, "Kal In Ex" ),
			new SpellItemInfo( MagicSpell.MagicReflect, 35, typeof( MagicReflectScroll ), "magic reflect", "Faz com que os feitiços de magia lançados em você sejam refletidos de volta para quem os lançou. Quanto melhores forem sua magia e psicologia, mais magia você poderá refletir antes que o feitiço acabe. Você precisará de um diamante para fazer o feitiço do item funcionar, junto com os reagentes.", "Fifth", 2275, "In Jux Sanct" ),
			new SpellItemInfo( MagicSpell.MindBlast, 36, typeof( MindBlastScroll ), "mind blast", "Causa dano de Gelo ao Alvo baseado na Magia e Inteligência do Conjurador.", "Fifth", 2276, "Por Corp Wis" ),
			new SpellItemInfo( MagicSpell.Paralyze, 37, typeof( ParalyzeScroll ), "paralyze", "Imobiliza o Alvo por um breve período de tempo. A habilidade de Resistência Mágica do Alvo afeta a Duração da imobilização.", "Fifth", 2277, "An Ex Por" ),
			new SpellItemInfo( MagicSpell.PoisonField, 38, typeof( PoisonFieldScroll ), "poison field", "Conjura uma parede de vapor venenoso que envenena qualquer coisa que passe por ela. A força do Veneno é baseada nas habilidades de Magia e Veneno do Conjurador.", "Fifth", 2278, "In Nox Grav" ),
			new SpellItemInfo( MagicSpell.SummonCreature, 39, typeof( SummonCreatureScroll ), "summon creature", "Invoca uma criatura aleatória como um Animal de Estimação por uma duração limitada. A força da criatura invocada é baseada na habilidade de Magia do Conjurador.", "Fifth", 2279, "Kal Xen" ),
			new SpellItemInfo( MagicSpell.Dispel, 40, typeof( DispelScroll ), "dispel", "Tenta Dispersar uma criatura invocada, fazendo-a desaparecer do mundo. A dificuldade da Dispersão é afetada pela habilidade de Magia do dono da criatura.", "Sixth", 2280, "An Ort" ),
			new SpellItemInfo( MagicSpell.EnergyBolt, 41, typeof( EnergyBoltScroll ), "energy bolt", "Dispara um raio de força mágica no Alvo, causando dano de Energia.", "Sixth", 2281, "Corp Por" ),
			new SpellItemInfo( MagicSpell.Explosion, 42, typeof( ExplosionScroll ), "explosion", "Ataca o Alvo com uma explosão de energia, causando dano de Fogo.", "Sixth", 2282, "Vas Ort Flam" ),
			new SpellItemInfo( MagicSpell.Invisibility, 43, typeof( InvisibilityScroll ), "invisibility", "Faz com que o Alvo se torne invisível temporariamente.", "Sixth", 2283, "An Lor Xen" ),
			new SpellItemInfo( MagicSpell.Mark, 44, typeof( MarkScroll ), "mark", "Marca uma runa na Localização atual do Conjurador. Existem feitiços e habilidades mágicas que podem ser usados na runa para teleportar alguém para a localização com a qual ela está marcada.", "Sixth", 2284, "Kal Por Ylem" ),
			new SpellItemInfo( MagicSpell.MassCurse, 45, typeof( MassCurseScroll ), "mass curse", "Lança o feitiço Maldição em um Alvo e em quaisquer criaturas num raio de dois tiles.", "Sixth", 2285, "Vas Des Sanct" ),
			new SpellItemInfo( MagicSpell.ParalyzeField, 46, typeof( ParalyzeFieldScroll ), "paralyze field", "Conjura um campo de energia paralisante que afeta qualquer criatura que entrar nele com os efeitos do feitiço Paralisia.", "Sixth", 2286, "In Ex Grav" ),
			new SpellItemInfo( MagicSpell.Reveal, 47, typeof( RevealScroll ), "reveal", "Revela a presença de quaisquer criaturas ou jogadores invisíveis ou escondidos dentro de um raio ao redor do tile alvo.", "Sixth", 2287, "Wis Quas" ),
			new SpellItemInfo( MagicSpell.ChainLightning, 48, typeof( ChainLightningScroll ), "chain lightning", "Dano a alvos próximos com uma série de raios que causam dano de Energia.", "Seventh", 2288, "Vas Ort Grav" ),
			new SpellItemInfo( MagicSpell.EnergyField, 49, typeof( EnergyFieldScroll ), "energy field", "Conjura um campo temporário de energia no chão no Local Alvo que bloqueia todo movimento.", "Seventh", 2289, "In Sanct Grav" ),
			new SpellItemInfo( MagicSpell.FlameStrike, 50, typeof( FlamestrikeScroll ), "flamestrike", "Envolve o alvo numa coluna de chama mágica que causa dano de Fogo.", "Seventh", 2290, "Kal Vas Flam" ),
			new SpellItemInfo( MagicSpell.GateTravel, 51, typeof( GateTravelScroll ), "gate travel", "Mirar uma runa marcada com o feitiço Marca abre um portal temporário para a localização marcada da runa. O portal pode ser usado por qualquer um para viajar para aquela localização.", "Seventh", 2291, "Vas Rel Por" ),
			new SpellItemInfo( MagicSpell.ManaVampire, 52, typeof( ManaVampireScroll ), "mana vampire", "Drena mana do Alvo e a transfere para o Conjurador. A quantidade de mana drenada é determinada por uma comparação entre a habilidade de Psicologia do Conjurador e a habilidade de Resistência Mágica do Alvo.", "Seventh", 2292, "Ort Sanct" ),
			new SpellItemInfo( MagicSpell.MassDispel, 53, typeof( MassDispelScroll ), "mass dispel", "Tenta dispersar qualquer criatura invocada dentro de um raio de oito tiles.", "Seventh", 2293, "Vas An Ort" ),
			new SpellItemInfo( MagicSpell.MeteorSwarm, 54, typeof( MeteorSwarmScroll ), "meteor swarm", "Invoca uma nuvem de meteoros flamejantes que atingem todos os alvos dentro de um raio ao redor do Local Alvo. O dano total de Fogo é dividido entre todos os Alvos do feitiço.", "Seventh", 2294, "Flam Kal Des Ylem" ),
			new SpellItemInfo( MagicSpell.Polymorph, 55, typeof( PolymorphScroll ), "polymorph", "Transforma temporariamente o Conjurador numa criatura selecionada de uma lista específica. Enquanto polimorfado, outros jogadores verão o Conjurador como um criminoso.", "Seventh", 2295, "Vas Ylem Rel" ),
			new SpellItemInfo( MagicSpell.Earthquake, 56, typeof( EarthquakeScroll ), "earthquake", "Causa um tremor violento da terra que danifica todas as criaturas e personagens próximos.", "Eighth", 2296, "In Vas Por" ),
			new SpellItemInfo( MagicSpell.EnergyVortex, 57, typeof( EnergyVortexScroll ), "energy vortex", "Invoca uma massa giratória de energia que seleciona um Alvo para atacar com base em sua inteligência e proximidade. O Vórtice de Energia desaparece após um tempo determinado. Requer 2 slots de controle de animais.", "Eighth", 2297, "Vas Corp Por" ),
			new SpellItemInfo( MagicSpell.Resurrection, 58, typeof( ResurrectionScroll ), "resurrection", "Ressuscita outro ou invoca um item mágico para se ressuscitar mais tarde.", "Eighth", 2298, "An Corp" ),
			new SpellItemInfo( MagicSpell.AirElemental, 59, typeof( SummonAirElementalScroll ), "summon air elemental", "Um elemental do ar é invocado para servir o Conjurador. Requer 2 slots de controle de animais.", "Eighth", 2299, "Kal Vas Xen Hur" ),
			new SpellItemInfo( MagicSpell.SummonDaemon, 60, typeof( SummonDaemonScroll ), "summon daemon", "Um demônio é invocado para servir o Conjurador. Resulta em uma grande perda de Karma para o Conjurador. Requer 4 slots de controle de animais.", "Eighth", 2300, "Kal Vas Xen Corp" ),
			new SpellItemInfo( MagicSpell.EarthElemental, 61, typeof( SummonEarthElementalScroll ), "summon earth elemental", "Um elemental da terra é invocado para servir o conjurador. Requer 2 slots de controle de animais", "Eighth", 2301, "Kal Vas Xen Ylem" ),
			new SpellItemInfo( MagicSpell.FireElemental, 62, typeof( SummonFireElementalScroll ), "summon fire elemental", "Um elemental do fogo é invocado para servir o conjurador. Requer 4 slots de controle de animais.", "Eighth", 2302, "Kal Vas Xen Flam" ),
			new SpellItemInfo( MagicSpell.WaterElemental, 63, typeof( SummonWaterElementalScroll ), "summon water elemental", "Um elemental da água é invocado para servir o conjurador. Requer 3 slots de controle de animais.", "Eighth", 2303, "Kal Vas Xen An Flam" ),
			new SpellItemInfo( MagicSpell.SummonSnakes, 700, typeof( BlankScroll ), "", "", "", 0, "" ),
			new SpellItemInfo( MagicSpell.SummonDragon, 701, typeof( BlankScroll ), "", "", "", 0, "" ),
			new SpellItemInfo( MagicSpell.SummonSkeleton, 704, typeof( BlankScroll ), "", "", "", 0, "" ),
			new SpellItemInfo( MagicSpell.Identify, 705, typeof( BlankScroll ), "", "", "", 0, "" ),
			new SpellItemInfo( MagicSpell.CurseWeapon, 103, typeof( CurseWeaponScroll ), "curse weapon", "Imbuí temporariamente uma arma com um efeito de drenagem de vida.", "First", 20483, "An Sanct Grav Corp" ),
			new SpellItemInfo( MagicSpell.BloodOath, 101, typeof( BloodOathScroll ), "blood oath", "Cria temporariamente um pacto sombrio entre o Conjurador e o Alvo. Qualquer dano causado pelo Alvo ao Conjurador é aumentado, mas o Alvo recebe a mesma quantidade de dano.", "First", 20481, "In Jux Mani Xen" ),
			new SpellItemInfo( MagicSpell.CorpseSkin, 102, typeof( CorpseSkinScroll ), "corpse skin", "Transmografa a carne da criatura ou jogador Alvo para se assemelhar à carne de um cadáver podre, tornando-os mais vulneráveis a dano de Fogo e Veneno, mas aumentando sua Resistência a dano Físico e de Gelo.", "First", 20482, "In An Corp Ylem" ),
			new SpellItemInfo( MagicSpell.EvilOmen, 104, typeof( EvilOmenScroll ), "evil omen", "Amaldiçoa o Alvo para que o próximo evento prejudicial que o afete seja ampliado.", "First", 20484, "Por Tym An Sanct" ),
			new SpellItemInfo( MagicSpell.PainSpike, 108, typeof( PainSpikeScroll ), "pain spike", "Causa temporariamente dor física intensa ao Alvo, causando dano Direto. Quando o feitiço acabar, se o Alvo ainda estiver vivo, parte dos Pontos de Vida perdidos através do Pico de Dor são restaurados.", "First", 20488, "In Sanct" ),
			new SpellItemInfo( MagicSpell.WraithForm, 115, typeof( WraithFormScroll ), "wraith form", "Transforma o Conjurador num espectro etéreo, diminuindo algumas Resistências Elementais, enquanto aumenta sua resistência Física. A Forma de Espectro também permite que o conjurador sempre tenha sucesso ao usar o feitiço Lembrança e causa um efeito de Dreno de Mana ao acertar inimigos. O Conjurador permanece nesta forma até relançar o feitiço Forma de Espectro.", "First", 20495, "Rel Xen Uus" ),
			new SpellItemInfo( MagicSpell.MindRot, 107, typeof( MindRotScroll ), "mind rot", "Tenta colocar uma maldição no Alvo que aumenta o custo de mana de quaisquer feitiços que ele lance, por uma duração baseada numa comparação entre a habilidade de Espiritualismo do Conjurador e a habilidade de Resistência Mágica do Alvo.", "Second", 20487, "Wis An Bet" ),
			new SpellItemInfo( MagicSpell.SummonFamiliar, 111, typeof( SummonFamiliarScroll ), "summon familiar", "Permite que o Conjurador invoque um Familiar de uma lista selecionada. Um Familiar seguirá e lutará com seu dono, além de conceder bônus únicos ao Conjurador, dependentes do tipo de Familiar invocado.", "Second", 20491, "Kal Xen Bet" ),
			new SpellItemInfo( MagicSpell.AnimateDead, 100, typeof( AnimateDeadScroll ), "animate dead", "Anima o cadáver Alvo, criando um morto-vivo sem mente e errante. A força do morto-vivo erguido é grandemente modificada pelo poder da criatura original e pelo poder do necromante.", "Third", 20480, "Uus Corp" ),
			new SpellItemInfo( MagicSpell.HorrificBeast, 105, typeof( HorrificBeastScroll ), "horrific beast", "Transforma o Conjurador numa besta demoníaca horripilante, que causa mais dano e recupera pontos de vida mais rápido, mas não pode mais lançar nenhum feitiço, exceto feitiços de Transformação de Necromante. O Conjurador permanece nesta forma até relançar o feitiço Besta Horripilante.", "Third", 20485, "Rel Xen Vas Bet" ),
			new SpellItemInfo( MagicSpell.PoisonStrike, 109, typeof( PoisonStrikeScroll ), "poison strike", "Cria uma explosão de energia venenosa centrada no Alvo. O Alvo principal é afligido com uma grande quantidade de dano de Veneno, e todos os Alvos válidos num raio ao redor do Alvo principal são afligidos com um efeito menor.", "Fourth", 20489, "In Vas Nox" ),
			new SpellItemInfo( MagicSpell.Wither, 114, typeof( WitherScroll ), "wither", "Cria uma geada definhadora ao redor do Conjurador, que causa dano de Gelo a todos os alvos válidos num raio.", "Fifth", 20494, "Kal Vas An Flam" ),
			new SpellItemInfo( MagicSpell.Strangle, 110, typeof( StrangleScroll ), "strangle", "Sufoca temporariamente o suprimento de ar do Alvo com vapores venenosos. O Alvo é afligido com dano de Veneno ao longo do tempo. A quantidade de dano causado a cada golpe é baseada na habilidade de Espiritualismo do Conjurador e no Vigor atual do Alvo.", "Fifth", 20490, "In Bet Nox" ),
			new SpellItemInfo( MagicSpell.LichForm, 106, typeof( LichFormScroll ), "lich form", "Transforma o Conjurador num lich, aumentando sua regeneração de mana e algumas Resistências, enquanto diminui sua Resistência ao Fogo e lentamente drena sua vida. O Conjurador permanece nesta forma até relançar o feitiço Forma de Lich.", "Sixth", 20486, "Rel Xen Corp Ort" ),
			new SpellItemInfo( MagicSpell.Exorcism, 116, typeof( ExorcismScroll ), "exorcism", "Este feitiço pode forçar os mortos-vivos a encontrar a verdadeira morte, ou pode enviar criaturas demoníacas de volta ao inferno. Alguns podem ser muito poderosos para este feitiço, mas muitos não são.", "Seventh", 20496, "Ort Corp Grav" ),
			new SpellItemInfo( MagicSpell.VengefulSpirit, 113, typeof( VengefulSpiritScroll ), "vengeful spirit", "Invoca um Espírito vil que assombra o Alvo até que o Alvo ou o Espírito esteja morto. Espíritos Vingativos têm a habilidade de rastrear seus Alvos onde quer que possam viajar. A força de um Espírito é determinada pelas habilidades de Necromancia e Espiritualismo do Conjurador.", "Seventh", 20493, "Kal Xen Bet Zu" ),
			new SpellItemInfo( MagicSpell.VampiricEmbrace, 112, typeof( VampiricEmbraceScroll ), "vampiric embrace", "Transforma o Conjurador num poderoso Vampiro, que aumenta sua regeneração de Vigor e Mana enquanto diminui sua Resistência ao Fogo. Vampiros também realizam Dreno de Vida ao acertar seus inimigos. O Conjurador permanece nesta forma até relançar o feitiço Abraço Vampírico.", "Eighth", 20492, "Rel Xen An Sanct" )
		};

		public static void setSpell( int level, Item item )
		{
			if ( level > 1000 ) // SPECIFIC WAND
			{
				level = level - 1000;
				item.Enchanted = ( MagicSpell )( level );
			}
			else
			{
				if ( level < 1 )
					level = Utility.RandomMinMax(1,8);

				if ( level > 8 )
					level = 8;

				if ( Utility.Random(25) == 0 ) // NECRO WANDS
				{
					int necro = Utility.RandomMinMax( 69, 74 );
					if ( level == 2 )
						necro = Utility.RandomMinMax( 75, 76 );
					else if ( level == 3 )
						necro = Utility.RandomMinMax( 77, 78 );
					else if ( level == 4 )
						necro = 79;
					else if ( level == 5 )
						necro = Utility.RandomMinMax( 80, 81 );
					else if ( level == 6 )
						necro = 82;
					else if ( level == 7 )
						necro = Utility.RandomMinMax( 83, 84 );
					else if ( level == 8 )
						necro = 85;

					item.Enchanted = ( MagicSpell )( necro );

					if ( item is MagicalWand )
					{
						item.ItemID = 0x6729;
						item.Hue = Utility.RandomEvilHue();
					}
				}
				else
					item.Enchanted = ( MagicSpell )( ( ( level * 8 ) - 8 ) + Utility.RandomMinMax( 1, 8 ) );
			}
		}

		public static void Cast( Spell spell, Mobile caster )
		{
			bool m = caster.CantWalk;
			caster.CantWalk = false;
			spell.Cast();
			caster.CantWalk = m;
		}

		public static void ChangeMagicSpell( MagicSpell spell, Item item, bool chargeable )
		{
			if ( spell == MagicSpell.None )
			{
				item.InfoData = null;
				item.InfoText2 = null;
				item.EnchantUsesMax = 0;
				item.EnchantUses = 0;
			}
			else
			{
				int level = SpellItems.GetLevel( (int)spell );
				item.EnchantUsesMax = 90 - ( level * 10 );
				item.EnchantUses = item.EnchantUsesMax;

				if ( !chargeable )
					item.EnchantUsesMax = 0;

				item.InfoData = "Isto pode lançar o feitiço " + SpellItems.GetName( item.Enchanted ) + ". " + SpellItems.GetData( item.Enchanted ) + " Deve estar equipado para lançar feitiços, onde mana geralmente é necessária. Quando as cargas se esgotarem, a magia desaparecerá. Para lançar o feitiço encantado, clique uma vez no item e selecione 'Magia'.";
			}
		}

		public static void CastEnchantment( Mobile from, Item item )
		{
			int uses = 1;
				if ( item.EnchantMod > 0 )
					uses = item.EnchantMod;

			if ( item.Parent != from )
				from.SendMessage("Isso deve estar equipado para usar.");
			else if ( item.EnchantUses >= uses )
				SpellItems.Cast( SpellRegistry.NewSpell( GetRegNum( item.Enchanted ), from, item ), from );
			else
				from.SendLocalizedMessage( 1019073 ); // This item is out of charges.
		}

		public static SpellItemInfo GetInfo( MagicSpell magicspell )
		{
			SpellItemInfo[] list = m_MagicInfo;

			int index = GetIndex( magicspell );

			if ( index >= 0 && index < list.Length )
				return list[index];

			return null;
		}

		public static int GetIndex( MagicSpell magicspell )
		{
			if ( magicspell == MagicSpell.None )
				return 0;

			return (int)(magicspell);
		}

		public static int GetRegNum( MagicSpell magicspell )
		{
			SpellItemInfo info = GetInfo( magicspell );

			if ( info == null || magicspell == MagicSpell.None )
				return -1;

			return info.SpellID;
		}

		public static string GetCircle( MagicSpell magicspell )
		{
			SpellItemInfo info = GetInfo( magicspell );

			if ( info == null )
				return null;

			if ( info.Circle == "" )
				return null;

			return info.Circle + " Circle";
		}

		public static string GetRunes( MagicSpell magicspell )
		{
			SpellItemInfo info = GetInfo( magicspell );

			if ( info == null )
				return null;

			if ( info.Runes == "" )
				return null;

			return info.Runes;
		}

		public static int GetIcon( MagicSpell magicspell )
		{
			SpellItemInfo info = GetInfo( magicspell );

			if ( info == null )
				return 0;

			if ( info.Icon < 1 )
				return 0;

			return info.Icon;
		}

		public static int GetCircleNumber( MagicSpell magicspell )
		{
			SpellItemInfo info = GetInfo( magicspell );

			if ( info == null )
				return 0;

			if ( info.Circle == "" )
				return 0;

			if ( info.Circle == "First" )
				return 3;
			else if ( info.Circle == "Second" )
				return 6;
			else if ( info.Circle == "Third" )
				return 9;
			else if ( info.Circle == "Fourth" )
				return 12;
			else if ( info.Circle == "Fifth" )
				return 15;
			else if ( info.Circle == "Sixth" )
				return 18;
			else if ( info.Circle == "Seventh" )
				return 21;
			else if ( info.Circle == "Eighth" )
				return 24;

			return 0;
		}

		public static string GetData( MagicSpell magicspell )
		{
			SpellItemInfo info = GetInfo( magicspell );

			return ( info == null ? null : info.Description );
		}

		public static string GetName( MagicSpell magicspell )
		{
			SpellItemInfo info = GetInfo( magicspell );

			return ( info == null ? null : info.SpellName );
		}

		public static string GetNameUpper( MagicSpell magicspell )
		{
			SpellItemInfo info = GetInfo( magicspell );
			TextInfo cultInfo = new CultureInfo("en-US", false).TextInfo;

			return ( info == null ? null : cultInfo.ToTitleCase( info.SpellName ) );
		}

		public static Type GetScroll( MagicSpell magicspell )
		{
			SpellItemInfo info = GetInfo( magicspell );

			return ( info == null ? null : info.ScrollType );
		}

		public static MagicSpell GetID( Type itemtype )
		{
			SpellItemInfo[] list = m_MagicInfo;
			int entries = list.Length;
			int val = 0;

			while ( entries > 0 )
			{
				if ( list[val].ScrollType == itemtype )
					entries = 0;
				else
					val++;

				entries--;
			}

			return (MagicSpell)val;
		}

		public static int GetWand( string name )
		{
			SpellItemInfo[] list = m_MagicInfo;
			int entries = list.Length;
			int val = 0;

			while ( entries > 0 )
			{
				if ( list[val].SpellName == name )
					entries = 0;
				else
					val++;

				entries--;
			}

			return 1000+val;
		}

		public static int GetLevel( int level )
		{
			if ( level == 69 )
				level = 1;
			else if ( level == 70 )
				level = 1;
			else if ( level == 71 )
				level = 1;
			else if ( level == 72 )
				level = 1;
			else if ( level == 73 )
				level = 1;
			else if ( level == 74 )
				level = 1;
			else if ( level == 75 )
				level = 2;
			else if ( level == 76 )
				level = 2;
			else if ( level == 77 )
				level = 3;
			else if ( level == 78 )
				level = 3;
			else if ( level == 79 )
				level = 4;
			else if ( level == 80 )
				level = 5;
			else if ( level == 81 )
				level = 5;
			else if ( level == 82 )
				level = 6;
			else if ( level == 83 )
				level = 7;
			else if ( level == 84 )
				level = 7;
			else if ( level == 85 )
				level = 8;
			else if ( level >= 57 )
				level = 8;
			else if ( level >= 49 )
				level = 7;
			else if ( level >= 41 )
				level = 6;
			else if ( level >= 33 )
				level = 5;
			else if ( level >= 25 )
				level = 4;
			else if ( level >= 17 )
				level = 3;
			else if ( level >= 9 )
				level = 2;
			else
				level = 1;

			return level;
		}
	}
}