using System;
using System.Collections.Generic;

namespace Server.Custom.Ascensions
{
    public class AscensionAbilityEntry
    {
        public string Name { get; set; }
        public int Level { get; set; }
        public bool IsPassive { get; set; }
        public string Command { get; set; }
        public int IconArt { get; set; }
        public int IconHue { get; set; }
        public string Description { get; set; }

        public AscensionAbilityEntry(string name, int level, bool isPassive, string command, int iconArt, int iconHue, string description)
        {
            Name = name;
            Level = level;
            IsPassive = isPassive;
            Command = command;
            IconArt = iconArt;
            IconHue = iconHue;
            Description = description;
        }
    }

    public static class AscensionAbilityCatalog
    {
        public static List<AscensionAbilityEntry> GetActiveAbilities(AscensionType type)
        {
            List<AscensionAbilityEntry> list = new List<AscensionAbilityEntry>();

            switch (type)
            {
                case AscensionType.Berserker:
                    list.Add(new AscensionAbilityEntry("Rage", 1, false, "[BerserkerRage", 0xF47, 0x21,
                        "Entra em fúria sanguinária. Ganha +(15+nível) FOR, imunidade a paralisia, +10% de dano e -10% de chance de defesa.<br>" +
                        "Dura 10 + nível seg. Recarga: 1 min. Ao expirar, consome 50% do vigor.<br>" +
                        "<BASEFONT COLOR=#82E0AA>Nvl 10:</BASEFONT> +nível/2 de resistências, +5% de dano.<br>" +
                        "<BASEFONT COLOR=#82E0AA>Nvl 15:</BASEFONT> Golpes acima de 40% da vida máx são reduzidos para 40%.<br>" +
                        "<BASEFONT COLOR=#82E0AA>Nvl 20:</BASEFONT> +5% adicional de dano."));

                    list.Add(new AscensionAbilityEntry("Leap Slam", 6, false, "[BerserkerLeapSlam", 0x1439, 0,
                        "Salta 3 + nível/4 tiles em direção ao alvo. Ao aterrissar, causa 20-35 + ((FOR/15)+(nível/3)) de dano físico a inimigos adjacentes.<br>" +
                        "Recarga: 9 seg (-1 seg a cada 3 níveis).<br>" +
                        "<BASEFONT COLOR=#82E0AA>Nvl 12:</BASEFONT> Atordoa e paralisa inimigos adjacentes por 3 segundos."));

                    list.Add(new AscensionAbilityEntry("Warcry", 11, false, "[BerserkerWarCry", 0x0FC7, 0x35,
                        "Rugido estarrecedor. Inimigos perdem (25+nível) FOR, (10+nível) DES e INT por (20+nível) seg.<br>" +
                        "Aliados ganham 2 de vigor a cada 2 seg pela mesma duração.<br>" +
                        "<BASEFONT COLOR=#82E0AA>Nvl 16:</BASEFONT> Inimigos perdem 20+2*nível de vigor imediatamente e 2+nível a cada 2 seg."));

                    list.Add(new AscensionAbilityEntry("Tenacity", 18, false, "[BerserkerTenacity", 0x1B74, 0x21,
                        "Cura completamente os pontos de vida de imediato e novamente a cada 10 seg durante 30 seg.<br>" +
                        "Recarga: 2 minutos.<br>" +
                        "<BASEFONT COLOR=#82E0AA>Nvl 20:</BASEFONT> Também regenera e cura completamente todo o vigor a cada pulso."));
                    break;

                case AscensionType.Archmage:
                    list.Add(new AscensionAbilityEntry("Arcane Storm", 1, false, "[ArchmageArcaneStorm", 0xEFA, 0x48D,
                        "Dispara (6 + 1 a cada 3 níveis) projéteis arcanos. Cada míssil causa 10-18 + (INT/25 + nível/3) de dano de energia. Custo: 45 mana.<br>" +
                        "Recarga: 45 - 1/nível seg.<br>" +
                        "<BASEFONT COLOR=#82E0AA>Nvl 10:</BASEFONT> 3%/nível de chance de causar -(8 + nível/4) resist energia.<br>" +
                        "<BASEFONT COLOR=#82E0AA>Nvl 15:</BASEFONT> O alvo perde 34 + nível/2 de vigor.<br>" +
                        "<BASEFONT COLOR=#82E0AA>Nvl 20:</BASEFONT> Matar com a tempestade zera a recarga de Conflux."));

                    list.Add(new AscensionAbilityEntry("Conflux", 6, false, "[ArchmageConflux", 0x1F19, 0x48D,
                        "Amplifica o poder arcano. O dano de feitiço do archmage aumenta em +10% por 10 + 1/nível seg.<br>" +
                        "Recarga: 2 minutos.<br>" +
                        "<BASEFONT COLOR=#82E0AA>Nvl 12:</BASEFONT> O bônus de dano de feitiço é aumentado para +15%."));

                    list.Add(new AscensionAbilityEntry("Mana Singularity", 11, false, "[ArchmageMassSingularity", 0x0E2D, 0x48D,
                        "Cria um vórtice arcano em colapso a até 2+(nível/3) tiles. Explode após 2.5 seg, causando 25-40 + (INT/10 + nível/2) de dano em área.<br>" +
                        "Inimigos perdem 20+nível de mana. Recupera 4 mana por alvo. Recarga: 60 seg.<br>" +
                        "<BASEFONT COLOR=#82E0AA>Nvl 16:</BASEFONT> Paralisa inimigos por 4 seg e drena 20+nível de vigor."));

                    list.Add(new AscensionAbilityEntry("Timestop", 18, false, "[ArchmageTimestop", 0x1810, 0x48D,
                        "Dobra o próprio tecido do tempo. Criaturas hostis a até 3+(nível/5) tiles ficam totalmente paralisadas por (12 + nível/4) seg.<br>" +
                        "Custo: 60 mana. Recarga: 3 minutos.<br>" +
                        "<BASEFONT COLOR=#82E0AA>Nvl 20:</BASEFONT> Ao expirar a paralisia, inimigos perdem 40+nível de mana e vigor."));
                    break;

                case AscensionType.Palemaster:
                    list.Add(new AscensionAbilityEntry("Undying Hordes", 1, false, "[PalemasterUndyingHordes", 0x1AE2, 0x455,
                        "Invoca uma horda implacável de mortos-vivos (esqueletos, múmias, gigantes e dragões conforme seu nível).<br>" +
                        "Duração: 60 + 3/nível seg. Recarga: 3 minutos.<br>" +
                        "<BASEFONT COLOR=#82E0AA>Nvl 17-20:</BASEFONT> Invoca gigantes mortos-vivos, múmias antigas e dragões esqueléticos."));

                    list.Add(new AscensionAbilityEntry("Enervate", 6, false, "[PalemasterEnervate", 0x182D, 0x455,
                        "Drena a vitalidade dos vivos. Causa 40 + (nível/3) de dano de frio e queima de mana em área.<br>" +
                        "Recupera até 50 de vida e mana para o Palemaster e reduz FOR e DES dos alvos em -(5+nível/4)%.<br>" +
                        "Recarga: 1 minuto.<br>" +
                        "<BASEFONT COLOR=#82E0AA>Nvl 12:</BASEFONT> Limite de cura e mana aumentado para 100."));

                    list.Add(new AscensionAbilityEntry("Circle of Death", 11, false, "[PalemasterCircleOfDeath", 0x0E2E, 0x455,
                        "Corrompe a área ao redor do alvo. Criaturas recebem 12-18 + (nível/2) + (INT/20) de dano de frio por segundo por 3 + nível/4 seg.<br>" +
                        "Recarga: 2 minutos.<br>" +
                        "<BASEFONT COLOR=#82E0AA>Nvl 16:</BASEFONT> Inimigos no círculo perdem vigor a cada segundo igual a 50% do dano."));

                    list.Add(new AscensionAbilityEntry("Danse Macabre", 18, false, "[PalemasterDanseMacabre", 0x26BA, 0x455,
                        "Rompe o véu com o mundo dos mortos. A cada segundo por nível/3 seg, invoca gigantes mortos-vivos, múmias ou cura completamente a vida do Palemaster.<br>" +
                        "Recarga: 4 minutos.<br>" +
                        "<BASEFONT COLOR=#82E0AA>Nvl 20:</BASEFONT> Pode invocar Dragões Esqueléticos e debuffar inimigos em -20% FOR."));
                    break;

                case AscensionType.Crusader:
                    list.Add(new AscensionAbilityEntry("Smite", 1, false, "[CrusaderSmite", 0x0F5E, 0x481,
                        "Explosão de chamas sagradas que causa 20-32 + (nível/2 + FOR/15) de dano a criaturas malignas em área.<br>" +
                        "Recarga: 1 minuto.<br>" +
                        "<BASEFONT COLOR=#82E0AA>Nvl 10:</BASEFONT> Paralisa alvos de karma muito negativo por 4 seg.<br>" +
                        "<BASEFONT COLOR=#82E0AA>Nvl 15:</BASEFONT> Cura aliados na área em 20 + nível/2 de vida.<br>" +
                        "<BASEFONT COLOR=#82E0AA>Nvl 20:</BASEFONT> Inimigos sofrem queimadura sagrada contínua por 12 seg."));

                    list.Add(new AscensionAbilityEntry("Charge", 6, false, "[CrusaderCharge", 0x1BC3, 0x481,
                        "Investida veloz contra o local alvo (alcance 3+nível/5). Chance de paralisar inimigos ao redor do impacto.<br>" +
                        "Recarga: 9 seg (-1 seg a cada 3 níveis).<br>" +
                        "<BASEFONT COLOR=#82E0AA>Nvl 12:</BASEFONT> Causa 30-45 + (FOR/15 + nível/3) de dano físico a todos adjacentes."));

                    list.Add(new AscensionAbilityEntry("Aura of Hope", 11, false, "[CrusaderAuraOfHope", 0x099A, 0x481,
                        "Aura celestial protetora. O crusader e aliados restauram 12+nível/2 de vida e 6+nível/2 de mana e vigor a cada 4 seg por 30 seg.<br>" +
                        "Recarga: 3 minutos.<br>" +
                        "<BASEFONT COLOR=#82E0AA>Nvl 16:</BASEFONT> Ao expirar o buff, cura completamente a vida de todos os afetados."));

                    list.Add(new AscensionAbilityEntry("Heavenly Gate", 18, false, "[CrusaderHeavensGate", 0x0004, 0x481,
                        "Abre os portões celestes e invoca um poderoso Archon celestial para guerrear ao seu lado até ser derrotado.<br>" +
                        "Recarga: 5 minutos.<br>" +
                        "<BASEFONT COLOR=#82E0AA>Nvl 20:</BASEFONT> 1%/nível de chance de invocar dois Archons celestiais de uma vez."));
                    break;

                case AscensionType.Assassin:
                    list.Add(new AscensionAbilityEntry("Noxious Cloud", 1, false, "[AssassinNoxiousCloudAbility", 0x0F0A, 0x44,
                        "Nuvem de vapores venenosos que inflige veneno maior em área.<br>" +
                        "Recarga: 1 minuto.<br>" +
                        "<BASEFONT COLOR=#82E0AA>Nvl 10:</BASEFONT> A nuvem inflige Veneno Mortal.<br>" +
                        "<BASEFONT COLOR=#82E0AA>Nvl 15:</BASEFONT> A nuvem inflige Veneno Letal.<br>" +
                        "<BASEFONT COLOR=#82E0AA>Nvl 20:</BASEFONT> Inimigos perdem 20 de resistência a veneno por 30 seg."));

                    list.Add(new AscensionAbilityEntry("Crippling Poison", 6, false, "[AssassinCripplingPoison", 0x0F51, 0x44,
                        "Arremessa um frasco incapacitante, aplicando veneno mortal e impedindo o alvo de andar por nível/2 seg.<br>" +
                        "Recarga: 1 minuto.<br>" +
                        "<BASEFONT COLOR=#82E0AA>Nvl 12:</BASEFONT> O veneno passa a ser Letal e afeta inimigos em 2 tiles de raio."));

                    list.Add(new AscensionAbilityEntry("Toxic Surge", 11, false, "[AssassinToxicSurge", 0x0F7E, 0x44,
                        "Surto assassino. Por 30 seg, causa +10% de dano adicional com armas contra alvos envenenados.<br>" +
                        "Recarga: 2 minutos.<br>" +
                        "<BASEFONT COLOR=#82E0AA>Nvl 16:</BASEFONT> 1%/nível de chance de zerar a recarga de Noxious Cloud."));

                    list.Add(new AscensionAbilityEntry("Cleansing Annihilation", 18, false, "[AssassinCleansingAnnihilation", 0x1AE2, 0x44,
                        "Aplica veneno letal e consome instantaneamente todos os seus tiques de dano de uma vez só.<br>" +
                        "Se o alvo sobreviver, infecta todos em 3 tiles de raio com veneno letal. Recarga: 4 min.<br>" +
                        "<BASEFONT COLOR=#82E0AA>Nvl 20:</BASEFONT> 1%/nível de chance de zerar a recarga de Toxic Surge."));
                    break;

                case AscensionType.Blackguard:
                    list.Add(new AscensionAbilityEntry("Dark Succor", 1, false, "[BlackguardDarkSuccor", 0x140E, 0x497,
                        "Entra em transe profano por 30+1/nível seg, recebendo +15+nível/2 de FOR. Recarga: 90 seg.<br>" +
                        "<BASEFONT COLOR=#82E0AA>Nvl 10:</BASEFONT> Causa +0.75%/nível de dano extra contra alvos de karma positivo.<br>" +
                        "<BASEFONT COLOR=#82E0AA>Nvl 15:</BASEFONT> Matar inimigos recupera 2 + nível/2 de mana.<br>" +
                        "<BASEFONT COLOR=#82E0AA>Nvl 20:</BASEFONT> Matar inimigos cura 6 + nível/2 de vida."));

                    list.Add(new AscensionAbilityEntry("Death's Advance", 6, false, "[BlackguardDeathsAdvance", 0x1BC4, 0x497,
                        "Avanço espectral contra um tile alvo. Ao chegar, repele inimigos adjacentes.<br>" +
                        "Recarga: 9 seg (-1 seg a cada 3 níveis).<br>" +
                        "<BASEFONT COLOR=#82E0AA>Nvl 12:</BASEFONT> Causa 30-45 + (FOR/15 + nível/3) de dano físico no impacto."));

                    list.Add(new AscensionAbilityEntry("Chains of Ice", 11, false, "[BlackguardChainsOfIce", 0x1A03, 0x48D,
                        "Correntes gélidas que congelam inimigos ao redor por 3 + 1/4 níveis seg e causam 30-43 + FOR/15 dano de frio.<br>" +
                        "Recarga: 90 segundos.<br>" +
                        "<BASEFONT COLOR=#82E0AA>Nvl 16:</BASEFONT> As correntes explodem em dano adicional de frio ao final da paralisia."));

                    list.Add(new AscensionAbilityEntry("Frostwyrm's Fury", 18, false, "[BlackguardsFrostwyrmsFury", 0x2235, 0x48D,
                        "Invoca um Dracolich das profundezas do submundo sob sua vontade, lutando até tombar em combate.<br>" +
                        "Recarga: 5 minutos.<br>" +
                        "<BASEFONT COLOR=#82E0AA>Nvl 20:</BASEFONT> 1%/nível de chance de invocar dois Frostwyrms simultâneos."));
                    break;

                case AscensionType.Skald:
                    list.Add(new AscensionAbilityEntry("War Chant", 1, false, "[SkaldWarChant", 0x0EB2, 0x53,
                        "Canção de bravura. Aliados a até 4 tiles recebem +(5+nível) FOR e DES, e +(5+nível) em Táticas e Resistência Mágica por 10+nível seg.<br>" +
                        "Recarga: 90 seg.<br>" +
                        "<BASEFONT COLOR=#82E0AA>Nvl 10:</BASEFONT> +0.5%/nível de dano com armas.<br>" +
                        "<BASEFONT COLOR=#82E0AA>Nvl 15:</BASEFONT> Abater inimigos recupera 5 vida e 3 vigor.<br>" +
                        "<BASEFONT COLOR=#82E0AA>Nvl 20:</BASEFONT> 2%/nível de chance de reativar sem custo nem recarga."));

                    list.Add(new AscensionAbilityEntry("Saga of Valor", 6, false, "[SkaldSagaOfValor", 0x0EB3, 0x53,
                        "O skald e aliados a até 4 tiles ganham +(nível/2)% de chance de acerto por 15+nível seg.<br>" +
                        "Recarga: 90 seg.<br>" +
                        "<BASEFONT COLOR=#82E0AA>Nvl 12:</BASEFONT> Também concede +(nível/2)% de chance de defesa."));

                    list.Add(new AscensionAbilityEntry("Song of Thunder", 11, false, "[SkaldSongOfThunder", 0x0EB4, 0x48D,
                        "Convoca relâmpagos rítmicos. A cada 3 seg, raios atingem inimigos causando 30-45 (+fama/1000) de dano de energia.<br>" +
                        "Recarga: 180 seg.<br>" +
                        "<BASEFONT COLOR=#82E0AA>Nvl 16:</BASEFONT> Dispara um raio adicional a cada pulso."));

                    list.Add(new AscensionAbilityEntry("Dirge of the Fallen", 18, false, "[SkaldDirgeOfTheFallen", 0x1428, 0x53,
                        "Entoa um réquiem épico invocando 3+(nível/4) guerreiros lendários para massacrar seus oponentes por até 90 seg.<br>" +
                        "Recarga: 5 minutos.<br>" +
                        "<BASEFONT COLOR=#82E0AA>Nvl 20:</BASEFONT> 1%/nível de chance de disparar Song of Thunder automaticamente."));
                    break;

                case AscensionType.Reaver:
                    list.Add(new AscensionAbilityEntry("Gorge", 1, false, "[ReaverGorge", 0x0EC2, 0x20,
                        "Maldiz o solo com sangue vicioso. Inimigos na área sofrem -1.25 de resist física por nível por 10+nível seg.<br>" +
                        "Recarga: 3 minutos.<br>" +
                        "<BASEFONT COLOR=#82E0AA>Nvl 10:</BASEFONT> Inimigos na poça ficam aleijados e não podem correr.<br>" +
                        "<BASEFONT COLOR=#82E0AA>Nvl 15:</BASEFONT> -0.66/nível de penalidade em todas as resistências elementais.<br>" +
                        "<BASEFONT COLOR=#82E0AA>Nvl 20:</BASEFONT> Inimigos mortos na poça explodem em dano físico em área."));

                    list.Add(new AscensionAbilityEntry("Exsanguinate", 6, false, "[ReaverExsanguinate", 0x26BA, 0x20,
                        "Ataque em turbilhão que causa 18-26 + FOR/15 de dano e faz todos sangrarem por 26-32 de dano a cada 3 seg por 13+nível seg.<br>" +
                        "Recarga: 1 minuto.<br>" +
                        "<BASEFONT COLOR=#82E0AA>Nvl 12:</BASEFONT> Recupera 9 de vida para cada inimigo atingido."));

                    list.Add(new AscensionAbilityEntry("Bloodstorm", 11, false, "[ReaverBloodstorm", 0x1182, 0x20,
                        "Explosão de sangue violenta que atinge todos a até 3+(nível/5) tiles, causando 20-32 + (nível/2 + FOR/15) de dano.<br>" +
                        "Recarga: 2 minutos.<br>" +
                        "<BASEFONT COLOR=#82E0AA>Nvl 16:</BASEFONT> 0.5%/nível de chance de zerar a recarga de Exsanguinate."));

                    list.Add(new AscensionAbilityEntry("Absolute Tyranny", 18, false, "[ReaverAbsoluteTyranny", 0x153B, 0x20,
                        "Dominação absoluta no campo de batalha, dizimando a resistência e a moral dos adversários adjacentes.<br>" +
                        "Recarga: 4 minutos."));
                    break;

                case AscensionType.Kensai:
                    list.Add(new AscensionAbilityEntry("Battle Meditation", 1, false, "[KensaiBattleMeditation", 0x227B, 0,
                        "Transe supremo de lâmina. Causa +(10+nível/2)% de dano com espadas e +(5+nível/2)% de velocidade de ataque por 20+nível seg.<br>" +
                        "Recarga: 2 minutos.<br>" +
                        "<BASEFONT COLOR=#82E0AA>Nvl 10:</BASEFONT> +(10+nível/2) de chance de defesa.<br>" +
                        "<BASEFONT COLOR=#82E0AA>Nvl 15:</BASEFONT> 1%/nível de chance de zerar a recarga de Culling Strike.<br>" +
                        "<BASEFONT COLOR=#82E0AA>Nvl 20:</BASEFONT> Abater inimigos reduz o tempo de recarga de Tempest."));

                    list.Add(new AscensionAbilityEntry("Kai", 6, false, "[KensaiKai", 0x13FF, 0,
                        "Salto instantâneo contra o alvo (alcance 3+nível/4). Causa 20-35 + ((DES/15) + nível/3) de dano a todos adjacentes.<br>" +
                        "Recarga: 9 seg (-1 seg a cada 3 níveis).<br>" +
                        "<BASEFONT COLOR=#82E0AA>Nvl 12:</BASEFONT> 1%/nível de chance de desferir um Culling Strike em todos ao redor."));

                    list.Add(new AscensionAbilityEntry("Culling Strike", 11, false, "[KensaiCullingStrike", 0x27A2, 0,
                        "Golpe de execução. Por 30 seg, ataques contra inimigos abaixo de 10% de vida têm 2%/nível de chance de causar +80% de dano.<br>" +
                        "Recarga: 3 minutos.<br>" +
                        "<BASEFONT COLOR=#82E0AA>Nvl 16:</BASEFONT> O limiar de execução aumenta para alvos abaixo de 15% de vida."));

                    list.Add(new AscensionAbilityEntry("Tempest", 18, false, "[KensaiTempest", 0x27A4, 0,
                        "Torna-se um furacão de lâminas cortantes, atingindo todos os inimigos a até 6 tiles por 70-85 + DES/15 de dano físico.<br>" +
                        "Recarga: 1 minuto.<br>" +
                        "<BASEFONT COLOR=#82E0AA>Nvl 20:</BASEFONT> Abater um inimigo com Tempest tem chance de reativá-la de graça."));
                    break;

                case AscensionType.Hierophant:
                    list.Add(new AscensionAbilityEntry("Divine Wrath", 1, false, "[HierophantDivineWrath", 0x0E81, 0x482,
                        "Chama divina cai sobre a área alvo, causando 30-48 + (karma/1000) de dano de fogo (+15% contra malignos). Recarga: 1 min.<br>" +
                        "<BASEFONT COLOR=#82E0AA>Nvl 10:</BASEFONT> +25% de dano contra criaturas de karma negativo.<br>" +
                        "<BASEFONT COLOR=#82E0AA>Nvl 15:</BASEFONT> Paralisa por 4 seg e inflige queimadura sagrada contínua.<br>" +
                        "<BASEFONT COLOR=#82E0AA>Nvl 20:</BASEFONT> 20% de chance de conjurar uma segunda vez imediatamente."));

                    list.Add(new AscensionAbilityEntry("Exalted Presence", 6, false, "[HierophantExaltedPresence", 0x0E28, 0x482,
                        "Presença sacra avassaladora. Força inimigos a se afastarem e interrompe seus ataques imediatamente.<br>" +
                        "Recarga: 2 minutos.<br>" +
                        "<BASEFONT COLOR=#82E0AA>Nvl 12:</BASEFONT> Criaturas malignas afetadas ficam paralisadas por 8 segundos."));

                    list.Add(new AscensionAbilityEntry("Consecrated Ground", 11, false, "[HierophantConsecratedGround", 0x0004, 0x482,
                        "Purifica até 4 tiles ao redor. Aliados regeneram 12+nível/3 de vida e 5+nível/3 de mana; malignos recebem dano de fogo constante.<br>" +
                        "Recarga: 3 minutos.<br>" +
                        "<BASEFONT COLOR=#82E0AA>Nvl 16:</BASEFONT> Seus feitiços de cura recuperam +25% de vida adicional."));

                    list.Add(new AscensionAbilityEntry("Divine Power", 18, false, "[HierophantDivinePower", 0x1F03, 0x482,
                        "Canaliza poder avatar. Ganha +20 FOR e DES, +15 Táticas e Espiritualismo, regenera 8 HP/seg e +20% dano de impacto.<br>" +
                        "Dura 30+nível seg. Recarga: 3 minutos.<br>" +
                        "<BASEFONT COLOR=#82E0AA>Nvl 20:</BASEFONT> Abater inimigos malignos tem 20% de chance de zerar Divine Wrath."));
                    break;

                case AscensionType.ArcaneArcher:
                    list.Add(new AscensionAbilityEntry("Imbue Arrow", 1, false, "[ArcaneArcherImbueArrows", 0x2D1E, 0x489,
                        "Encanta flechas com poder mágico por 30+1/nível seg. Flechas têm 2%/nível de chance de atingir a pior resistência do alvo.<br>" +
                        "Recarga: 2 minutos.<br>" +
                        "<BASEFONT COLOR=#82E0AA>Nvl 10:</BASEFONT> Efeitos desencadeados têm chance de disparar duas vezes.<br>" +
                        "<BASEFONT COLOR=#82E0AA>Nvl 15:</BASEFONT> Efeitos desencadeados causam +25% de dano.<br>" +
                        "<BASEFONT COLOR=#82E0AA>Nvl 20:</BASEFONT> Chance de zerar a recarga de Barrage."));

                    list.Add(new AscensionAbilityEntry("Charged Arrows", 6, false, "[ArcaneArcherChargedArrows", 0x0F3F, 0x489,
                        "Dispara uma flecha massiva perfurante (alcance 6 tiles) que causa 45-75 + (INT/15 + DES/15) de dano elemental.<br>" +
                        "Recarga: 1 minuto.<br>" +
                        "<BASEFONT COLOR=#82E0AA>Nvl 12:</BASEFONT> 0.5%/nível de chance de zerar a recarga de Arcane Volley."));

                    list.Add(new AscensionAbilityEntry("Arcane Volley", 11, false, "[ArcaneArcherArcaneVolley", 0x2FB7, 0x489,
                        "Dispara saraivada rápida de flechas arcanas em área (4 tiles), causando 55-75 + inscrição/10 de dano elemental.<br>" +
                        "Recarga: 30 segundos.<br>" +
                        "<BASEFONT COLOR=#82E0AA>Nvl 16:</BASEFONT> 1%/nível de chance de disparar a salva duas vezes consecutivas."));

                    list.Add(new AscensionAbilityEntry("Barrage", 18, false, "[ArcaneArcherBarrage", 0x13B2, 0x489,
                        "Torrente implacável de projéteis mágicos, atacando nível/4 vezes no alvo em um único instante.<br>" +
                        "Recarga: 3 minutos.<br>" +
                        "<BASEFONT COLOR=#82E0AA>Nvl 20:</BASEFONT> 0.25%/nível de chance de ser disparado em dobro."));
                    break;

                default:
                    list.Add(new AscensionAbilityEntry("Poder Ancestral", 1, false, "[Ativar", 0x2259, 0,
                        "Habilidade primária concedida pela ascensão espiritual ao atingir o nível correspondente."));
                    break;
            }

            return list;
        }

        public static List<AscensionAbilityEntry> GetPassiveAbilities(AscensionType type)
        {
            List<AscensionAbilityEntry> list = new List<AscensionAbilityEntry>();

            switch (type)
            {
                case AscensionType.Berserker:
                    list.Add(new AscensionAbilityEntry("Cleave", 2, true, null, 0x143E, 0,
                        "Golpes de duas mãos têm 4% + 1%/nível de chance de desferir um segundo ataque em um inimigo adjacente.<br>" +
                        "<BASEFONT COLOR=#82E0AA>Nvl 5 & 13:</BASEFONT> Chance de desferir um segundo e terceiro golpe adicional."));

                    list.Add(new AscensionAbilityEntry("Uncanny Dodge", 8, true, null, 0x1515, 0,
                        "Reflexos sobre-humanos. 25 + 1%/nível de chance de ignorar completamente os efeitos de armadilhas.<br>" +
                        "<BASEFONT COLOR=#82E0AA>Nvl 17:</BASEFONT> A chance aumenta para 45 + 1%/nível."));

                    list.Add(new AscensionAbilityEntry("Pummeling Strikes", 14, true, null, 0x1414, 0,
                        "Golpes de arma de duas mãos têm (nível/4)% de chance de ignorar totalmente a armadura física do alvo.<br>" +
                        "<BASEFONT COLOR=#82E0AA>Nvl 19:</BASEFONT> A chance aumenta para (nível/2)%."));

                    list.Add(new AscensionAbilityEntry("Undying Wrath", 20, true, null, 0x1AE2, 0x21,
                        "Se receber um golpe que reduziria sua vida a zero ou menos, reduz para 1 ponto de vida e zera o tempo de recarga de Warcry.<br>" +
                        "Pode ocorrer uma vez por minuto."));
                    break;

                case AscensionType.Archmage:
                    list.Add(new AscensionAbilityEntry("Mana Vault", 2, true, null, 0x09A8, 0x48D,
                        "Quando tentarem drenar sua mana, há 2%/nível de chance de recuperá-la instantaneamente.<br>" +
                        "<BASEFONT COLOR=#82E0AA>Nvl 5:</BASEFONT> Inimigos recebem 15-25 + nível/2 de dano de energia ao tentar drenar.<br>" +
                        "<BASEFONT COLOR=#82E0AA>Nvl 13:</BASEFONT> Inimigos ficam paralisados por 2 segundos."));

                    list.Add(new AscensionAbilityEntry("Weave Reflection", 8, true, null, 0x1008, 0x48D,
                        "Recebe 2.5%/nível de chance passiva de refletir feitiços de volta ao conjurador.<br>" +
                        "<BASEFONT COLOR=#82E0AA>Nvl 17:</BASEFONT> Refletir feitiços tem 1%/nível de chance de zerar a recarga de Conflux."));

                    list.Add(new AscensionAbilityEntry("Arcane Tempest", 14, true, null, 0x1F56, 0x48D,
                        "Lançar feitiços em alvos hostis tem (0.25 * nível)% de chance de desencadear uma Mana Singularity gratuita sem recarga.<br>" +
                        "<BASEFONT COLOR=#82E0AA>Nvl 19:</BASEFONT> Chance de zerar a recarga de Arcane Storm."));

                    list.Add(new AscensionAbilityEntry("Weave Unraveling", 20, true, null, 0x1ECD, 0x48D,
                        "Feitiços nocivos têm 0.25%/nível de chance de criar 6-12 fendas na Trama ao redor do alvo por 12-22 seg.<br>" +
                        "Inimigos sobre as distorções sofrem 14-22 + (INT/15) de dano de energia por segundo."));
                    break;

                case AscensionType.Palemaster:
                    list.Add(new AscensionAbilityEntry("Deathless Vigor", 2, true, null, 0x1CF0, 0x455,
                        "Lançar feitiços de necromancia ofensivos tem 1.5%/nível de chance de curar 2 + (nível/2) de vida.<br>" +
                        "<BASEFONT COLOR=#82E0AA>Nvl 5:</BASEFONT> Chance de ganhar +10% de dano de feitiço por 15 segundos.<br>" +
                        "<BASEFONT COLOR=#82E0AA>Nvl 13:</BASEFONT> Bônus aumenta para +15% de dano por 25 segundos."));

                    list.Add(new AscensionAbilityEntry("Undead Graft", 8, true, null, 0x1B10, 0x455,
                        "Ao usar armadura de osso completa, feitiços de necromancia têm chance de curar 5% da vida de todos os mortos-vivos aliados.<br>" +
                        "<BASEFONT COLOR=#82E0AA>Nvl 17:</BASEFONT> Concede regeneração contínua de 2% da vida máx por segundo."));

                    list.Add(new AscensionAbilityEntry("Creeping Cold", 14, true, null, 0x221D, 0x455,
                        "Ao lançar Wither, há 0.25%/nível de chance de ativá-lo uma segunda vez adicional.<br>" +
                        "<BASEFONT COLOR=#82E0AA>Nvl 19:</BASEFONT> Chance de disparar Enervate automaticamente sem gastar mana ou recarga."));

                    list.Add(new AscensionAbilityEntry("Herald of Hereafter", 20, true, null, 0x3735, 0x455,
                        "Feitiços de necromancia têm 0.25%/nível de chance de gerar névoa fétida de distorção ao redor do alvo.<br>" +
                        "Causa 14-22 + (INT/15) de dano de veneno por segundo aos que ficarem na área."));
                    break;

                case AscensionType.Crusader:
                    list.Add(new AscensionAbilityEntry("Divine Grace", 2, true, null, 0x0F80, 0x481,
                        "Pureza de espírito. 4%/nível de chance de se curar automaticamente de venenos menores, regulares e maiores.<br>" +
                        "<BASEFONT COLOR=#82E0AA>Nvl 5:</BASEFONT> Cura venenos mortais.<br>" +
                        "<BASEFONT COLOR=#82E0AA>Nvl 13:</BASEFONT> Cura venenos letais."));

                    list.Add(new AscensionAbilityEntry("Holy Fervor", 8, true, null, 0x1F2B, 0x481,
                        "Matar um demônio tem 1.5%/nível de chance de causar uma detonação sagrada em área.<br>" +
                        "<BASEFONT COLOR=#82E0AA>Nvl 17:</BASEFONT> Ativar o fervor zera imediatamente a recarga de Smite."));

                    list.Add(new AscensionAbilityEntry("Inquisitorial Strikes", 14, true, null, 0x1439, 0x481,
                        "Golpes de combate corpo a corpo causam +(nível/2)% de dano adicional contra demônios.<br>" +
                        "<BASEFONT COLOR=#82E0AA>Nvl 19:</BASEFONT> Causa +(nível/4)% de dano extra contra todas as criaturas malignas."));

                    list.Add(new AscensionAbilityEntry("Divine Judgement", 20, true, null, 0x1579, 0x481,
                        "Ao matar uma criatura maligna poderosa, há 2.5% de chance de conjurar um Luminar celestial para lutar ao seu lado por 1 minuto."));
                    break;

                case AscensionType.Assassin:
                    list.Add(new AscensionAbilityEntry("Virulent Strikes", 2, true, null, 0x27AC, 0x44,
                        "Ao envenenar com um golpe, tem 1%/nível de chance de resolver imediatamente 1 tique de dano de veneno.<br>" +
                        "<BASEFONT COLOR=#82E0AA>Nvl 5 & 13:</BASEFONT> Chance de resolver tiques de veneno instantâneos a cada ataque subsequente."));

                    list.Add(new AscensionAbilityEntry("Dangerous Habits", 8, true, null, 0x1544, 0x44,
                        "Aumenta o dano de todos os tiques de veneno infligidos em +9%.<br>" +
                        "<BASEFONT COLOR=#82E0AA>Nvl 17:</BASEFONT> O bônus de dano de tique aumenta para +18%."));

                    list.Add(new AscensionAbilityEntry("Deadly Strikes", 14, true, null, 0x1401, 0x44,
                        "Causa +9% de dano com ataques contra alvos envenenados.<br>" +
                        "<BASEFONT COLOR=#82E0AA>Nvl 19:</BASEFONT> Seus ataques ignoram 25% da resistência a veneno do inimigo."));

                    list.Add(new AscensionAbilityEntry("Terminal", 20, true, null, 0x1848, 0x44,
                        "Ao matar uma criatura envenenada, 0.25%/nível de chance de espalhar veneno letal para todos os inimigos a até 2 tiles de raio."));
                    break;

                case AscensionType.Blackguard:
                    list.Add(new AscensionAbilityEntry("Frozen Heart", 2, true, null, 0x221D, 0x48D,
                        "Aumenta o dano de frio de todos os seus golpes de arma em +6%.<br>" +
                        "<BASEFONT COLOR=#82E0AA>Nvl 5:</BASEFONT> Aumenta para +12%.<br>" +
                        "<BASEFONT COLOR=#82E0AA>Nvl 13:</BASEFONT> Aumenta para +18%."));

                    list.Add(new AscensionAbilityEntry("Morbidity", 8, true, null, 0x141B, 0x497,
                        "Ao matar uma criatura de karma positivo, tem 0.25%/nível de chance de aterrorizar inimigos próximos, fazendo-os fugir em pânico.<br>" +
                        "<BASEFONT COLOR=#82E0AA>Nvl 17:</BASEFONT> O tempo de fuga do pavor aumenta para 6 segundos."));

                    list.Add(new AscensionAbilityEntry("Merciless Strikes", 14, true, null, 0x1441, 0x497,
                        "Golpes de arma têm 1%/nível de chance de mirar na menor resistência defensiva do alvo.<br>" +
                        "<BASEFONT COLOR=#82E0AA>Nvl 19:</BASEFONT> Chance de paralisar o alvo por 3 segundos."));

                    list.Add(new AscensionAbilityEntry("Soul Reaper", 20, true, null, 0x1EA7, 0x497,
                        "Ao matar um inimigo hostil, 0.25%/nível de chance de drenar nível + FOR/25 de vida de todas as criaturas a até 2 tiles."));
                    break;

                case AscensionType.Skald:
                    list.Add(new AscensionAbilityEntry("Battlefield Rhythm", 2, true, null, 0x0F61, 0x53,
                        "Ganha +3% de dano de arma contra criaturas em discórdia, provocadas ou pacificadas.<br>" +
                        "<BASEFONT COLOR=#82E0AA>Nvl 5:</BASEFONT> Bônus aumenta para +9%.<br>" +
                        "<BASEFONT COLOR=#82E0AA>Nvl 13:</BASEFONT> Bônus aumenta para +18%."));

                    list.Add(new AscensionAbilityEntry("Cutting Words", 8, true, null, 0x0EBE, 0x53,
                        "Suas tentativas de discórdia suprimem +4% adicional dos atributos e perícias do alvo.<br>" +
                        "<BASEFONT COLOR=#82E0AA>Nvl 17:</BASEFONT> Suprime +8% adicional."));

                    list.Add(new AscensionAbilityEntry("Resonance", 14, true, null, 0x1C12, 0x53,
                        "Sua proeza musical é considerada 15% mais potente para todos os feitiços de canção.<br>" +
                        "<BASEFONT COLOR=#82E0AA>Nvl 19:</BASEFONT> Aumenta para 25% mais potente."));

                    list.Add(new AscensionAbilityEntry("Saga of Steel", 20, true, null, 0x0F62, 0x53,
                        "Ao matar um inimigo com uma arma, tem 0.25%/nível de chance de ativar instantaneamente o feitiço Foe Requiem no inimigo mais próximo."));
                    break;

                case AscensionType.Reaver:
                    list.Add(new AscensionAbilityEntry("Blood Scent", 2, true, null, 0x0F7E, 0x20,
                        "Faro voraz de sangue. Seus ataques contra criaturas que estão sangrando causam dano aumentado."));

                    list.Add(new AscensionAbilityEntry("Hemorrhage", 8, true, null, 0x0E21, 0x20,
                        "Ferimentos graves. O dano periódico de sangramento aplicado a seus alvos se torna mais persistente e cruel."));

                    list.Add(new AscensionAbilityEntry("Mutilate", 14, true, null, 0x0EC4, 0x20,
                        "Golpes mutiladores desferidos contra criaturas feridas têm chance de ignorar parte da armadura e reduzir seu vigor."));

                    list.Add(new AscensionAbilityEntry("Deep Cuts", 20, true, null, 0x099A, 0x20,
                        "Ao matar um inimigo hostil, 0.25%/nível de chance de aplicar sangramento pesado e aleijar todas as criaturas adjacentes por até 21 seg."));
                    break;

                case AscensionType.Kensai:
                    list.Add(new AscensionAbilityEntry("Practiced Perfection", 2, true, null, 0x13FE, 0,
                        "Maestria do corte. Causa +6% de dano aumentado com todas as espadas.<br>" +
                        "<BASEFONT COLOR=#82E0AA>Nvl 5:</BASEFONT> +12% de dano.<br>" +
                        "<BASEFONT COLOR=#82E0AA>Nvl 13:</BASEFONT> +18% de dano."));

                    list.Add(new AscensionAbilityEntry("Singular Focus", 8, true, null, 0x27A8, 0,
                        "Ao matar um oponente com vida cheia usando uma espada, ganha +9% de dano extra com espadas por 30 seg.<br>" +
                        "<BASEFONT COLOR=#82E0AA>Nvl 17:</BASEFONT> O bônus aumenta para +18%."));

                    list.Add(new AscensionAbilityEntry("Iaijutsu", 14, true, null, 0x27AB, 0,
                        "Golpes com espadas têm 1%/nível de chance de atingir a menor resistência do alvo.<br>" +
                        "<BASEFONT COLOR=#82E0AA>Nvl 19:</BASEFONT> 0.25%/nível de chance de desferir um segundo corte imediato."));

                    list.Add(new AscensionAbilityEntry("Final Cut", 20, true, null, 0x27A2, 0,
                        "Ao matar um oponente com vida cheia, tem 0.25%/nível de chance de desencadear uma Tempest sem gastar vigor."));
                    break;

                case AscensionType.Hierophant:
                    list.Add(new AscensionAbilityEntry("Blessed Might", 2, true, null, 0x1439, 0x482,
                        "Bênção sagrada. Causa +5% mais dano com armas de impacto.<br>" +
                        "<BASEFONT COLOR=#82E0AA>Nvl 5:</BASEFONT> +10% de dano.<br>" +
                        "<BASEFONT COLOR=#82E0AA>Nvl 13:</BASEFONT> +15% de dano."));

                    list.Add(new AscensionAbilityEntry("Divine Resilience", 8, true, null, 0x1B76, 0x482,
                        "Proteção divina inabalável. Recebe 4% menos dano de todas as fontes de ataque.<br>" +
                        "<BASEFONT COLOR=#82E0AA>Nvl 17:</BASEFONT> Redução aumentada para 8% de dano de todas as fontes."));

                    list.Add(new AscensionAbilityEntry("Death Ward", 14, true, null, 0x1AE2, 0x482,
                        "Proteção contra a morte. Ao sofrer dano letal, tem 1%/nível de chance de ignorar o dano fatal.<br>" +
                        "<BASEFONT COLOR=#82E0AA>Nvl 19:</BASEFONT> Ao ser salvo pela proteção, cura 33% dos seus pontos de vida."));

                    list.Add(new AscensionAbilityEntry("Divine Absolution", 20, true, null, 0x0E28, 0x482,
                        "Ao sofrer dano de criatura maligna, 0.25%/nível de chance de ativar Consecrated Ground de graça sem gastar mana."));
                    break;

                case AscensionType.ArcaneArcher:
                    list.Add(new AscensionAbilityEntry("Arcane Precision", 2, true, null, 0x0F3F, 0x489,
                        "Miras encantadas. Causa +6% de dano aumentado com qualquer ataque à distância.<br>" +
                        "<BASEFONT COLOR=#82E0AA>Nvl 5:</BASEFONT> +12% de dano.<br>" +
                        "<BASEFONT COLOR=#82E0AA>Nvl 13:</BASEFONT> +18% de dano."));

                    list.Add(new AscensionAbilityEntry("Mystical Ricochet", 8, true, null, 0x2D1E, 0x489,
                        "Flechas saltadoras. 0.25%/nível de chance de o projétil ricochetear e acertar o inimigo mais próximo.<br>" +
                        "<BASEFONT COLOR=#82E0AA>Nvl 17:</BASEFONT> Pode ricochetear em até dois inimigos adicionais."));

                    list.Add(new AscensionAbilityEntry("Arcane Feedback", 14, true, null, 0x1F52, 0x489,
                        "Ao abater um oponente à distância, recupera Inscrição/25 de mana.<br>" +
                        "<BASEFONT COLOR=#82E0AA>Nvl 19:</BASEFONT> Também recupera Foco/25 de vigor ou vida."));

                    list.Add(new AscensionAbilityEntry("Arcane Momentum", 20, true, null, 0x2FB7, 0x489,
                        "Ao abater um oponente com ataque à distância, ganha Inscrição/8 de absorção de feitiços por 30 segundos."));
                    break;

                default:
                    list.Add(new AscensionAbilityEntry("Bênção Espiritual", 2, true, null, 0x0EFA, 0,
                        "Concede foco espiritual e bônus passivos graduais ao longo da evolução da ascensão."));
                    break;
            }

            return list;
        }
    }
}
