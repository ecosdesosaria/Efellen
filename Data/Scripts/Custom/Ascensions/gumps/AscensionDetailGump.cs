using System;
using System.Collections.Generic;
using System.Text;
using Server;
using Server.Gumps;
using Server.Mobiles;
using Server.Network;
using Server.Items;

namespace Server.Custom.Ascensions
{
    public class AscensionDetailGump : Gump
    {
        private PlayerMobile m_From;
        private AscensionType m_Type;
        private AscensionProgress m_Progress;

        private const int Button_Back = 1;
        private const int Button_Activate = 2;

        public AscensionDetailGump(PlayerMobile from, AscensionType type) : base(60, 30)
        {
            m_From = from;
            m_Type = type;
            m_Progress = from.AscensionProfile.Get(type);

            Closable = true;
            Disposable = true;
            Dragable = true;
            Resizable = false;

            BuildGump();
        }

        private void BuildGump()
        {
            const int gumpWidth = 680;
            const int gumpHeight = 675;

            // ==========================================
            // PÁGINA 0: Moldura, Cabeçalho, Abas e Rodapé
            // ==========================================
            AddPage(0);

            // Fundo principal com relevo e transparência
            AddBackground(0, 0, gumpWidth, gumpHeight, 0x1453);
            AddImageTiled(10, 10, gumpWidth - 20, gumpHeight - 20, 0xA40);
            AddAlphaRegion(10, 10, gumpWidth - 20, gumpHeight - 20);

            // Ícone da classe em destaque
            AscensionVisuals.DrawClassIcon(this, 25, 16, m_Type, 50);

            // Título e Subtítulo da Classe
            string hexColor = AscensionVisuals.GetColorHex(m_Type);
            string displayName = AscensionTypeHelper.GetDisplayName(m_Type);
            string subTitle = AscensionVisuals.GetSubTitle(m_Type);
            bool isActive = m_From.AscensionProfile.ActiveAscension == m_Type;

            AddHtml(85, 16, 310, 26, $"<BIG><BASEFONT COLOR={hexColor}><B>{displayName}</B></BASEFONT></BIG>", false, false);
            AddHtml(85, 40, 310, 20, $"<BASEFONT COLOR=#BDC3C7><I>{subTitle}</I></BASEFONT>", false, false);

            // Nível e Barra de Experiência no Topo Direito
            int required = m_Progress.GetRequiredExperience();
            double pct = required > 0 ? Math.Min(1.0, (double)m_Progress.Experience / required) : 1.0;
            string xpText = required > 0 ? $"{m_Progress.Experience} / {required}" : "MAX";

            AddHtml(410, 16, 240, 20, $"<RIGHT><BASEFONT COLOR=#82E0AA><B>Nível {m_Progress.Level}</B></BASEFONT></RIGHT>", false, false);
            AscensionVisuals.DrawProgressBar(this, 410, 38, 240, 18, pct, xpText);

            // Divisória do Cabeçalho
            AddImageTiled(20, 70, gumpWidth - 40, 2, 0xBB8);

            // Barra de Navegação por Abas (Client-Side Instantâneo)
            AddButton(25, 76, 0xFA5, 0xFA7, 0, GumpButtonType.Page, 1);
            AddHtml(60, 78, 150, 20, "<BASEFONT COLOR=#F4D03F><B>Visão Geral & Custos</B></BASEFONT>", false, false);

            AddButton(235, 76, 0xFA5, 0xFA7, 0, GumpButtonType.Page, 2);
            AddHtml(270, 78, 175, 20, "<BASEFONT COLOR=#5DADE2><B>Habilidades Ativas (4)</B></BASEFONT>", false, false);

            AddButton(455, 76, 0xFA5, 0xFA7, 0, GumpButtonType.Page, 3);
            AddHtml(490, 78, 175, 20, "<BASEFONT COLOR=#82E0AA><B>Habilidades Passivas (4)</B></BASEFONT>", false, false);

            // Divisória abaixo das Abas
            AddImageTiled(20, 104, gumpWidth - 40, 1, 0xBB8);

            // Rodapé Comum: Botões Voltar e Ativar
            int footerY = 620;
            AddImageTiled(20, footerY - 8, gumpWidth - 40, 1, 0xBB8);

            // Botão Voltar
            AddButton(30, footerY + 2, 0x15E3, 0x15E7, Button_Back, GumpButtonType.Reply, 0);
            AddHtml(55, footerY + 4, 80, 20, "<BASEFONT COLOR=#BDC3C7><B>Voltar</B></BASEFONT>", false, false);

            // Status Ativo / Botão de Ativação
            if (isActive)
            {
                AddHtml(480, footerY + 4, 170, 20, "<RIGHT><BASEFONT COLOR=#2ECC71><B>✔ ATUALMENTE ATIVA</B></BASEFONT></RIGHT>", false, false);
            }
            else if (m_Progress.Level > 0)
            {
                AddButton(490, footerY + 2, 0xFA5, 0xFA7, Button_Activate, GumpButtonType.Reply, 0);
                AddHtml(525, footerY + 4, 130, 20, "<BASEFONT COLOR=#F4D03F><B>Ativar Ascensão</B></BASEFONT>", false, false);
            }

            // ========================================================
            // PÁGINA 1: Visão Geral, Requisitos e Custos de Evolução
            // ========================================================
            AddPage(1);

            // Painel de Requisitos e Afinidade (sem scrollbar desnecessária)
            AddBackground(20, 112, gumpWidth - 40, 76, 0x2422);
            AddImageTiled(24, 116, gumpWidth - 48, 68, 0xA40);
            AddAlphaRegion(24, 116, gumpWidth - 48, 68);

            string reqHtml = BuildRequirementsText();
            AddHtml(28, 116, gumpWidth - 56, 68, reqHtml, false, false);

            // Seção Descrição do Arquétipo (Expandida em +2 linhas e com scrollbar de segurança)
            AddHtml(20, 190, 250, 20, "<BASEFONT COLOR=#F4D03F><B>Descrição do Arquétipo</B></BASEFONT>", false, false);
            AddBackground(20, 210, gumpWidth - 40, 210, 0x2422);
            AddImageTiled(24, 214, gumpWidth - 48, 202, 0xA40);
            AddAlphaRegion(24, 214, gumpWidth - 48, 202);

            string description = AscensionDefinitions.GetDescription(m_Type);
            AddHtml(28, 216, gumpWidth - 56, 198, $"<BASEFONT COLOR=#E5E7E9>{description}</BASEFONT>", false, true);

            // Card Atalho para Ver as Habilidades
            AddBackground(20, 426, gumpWidth - 40, 50, 0x2422);
            AddImageTiled(24, 430, gumpWidth - 48, 42, 0xA40);
            AddAlphaRegion(24, 430, gumpWidth - 48, 42);

            AddItem(28, 434, 0x2253, 0x481); // Ícone de livro de feitiços/grimório
            AddHtml(70, 432, 380, 20, "<BASEFONT COLOR=#F4D03F><B>Poderes Ancestrais da Classe</B></BASEFONT>", false, false);
            AddHtml(70, 450, 380, 20, "<BASEFONT COLOR=#BDC3C7><SMALL>Esta ascensão concede 4 habilidades ativas e 4 habilidades passivas exclusivas.</SMALL></BASEFONT>", false, false);

            AddButton(480, 438, 0xFA5, 0xFA7, 0, GumpButtonType.Page, 2);
            AddHtml(515, 440, 140, 20, "<BASEFONT COLOR=#5DADE2><B>Ver Habilidades ➔</B></BASEFONT>", false, false);

            // Painel de Custo / Evolução
            int upgradeY = 484;
            if (m_Progress.CanLevelUp() && m_Progress.Level < AscensionConstants.MaxAscensionLevel)
            {
                int next = m_Progress.GetNextLevel();
                int gold = AscensionCosts.GetGoldCost(next);
                int dust = AscensionCosts.GetDustCost(next);
                int scrolls = AscensionCosts.GetScrollCost(next);

                AddBackground(20, upgradeY, gumpWidth - 40, 95, 0x2422);
                AddImageTiled(24, upgradeY + 4, gumpWidth - 48, 87, 0xA40);
                AddAlphaRegion(24, upgradeY + 4, gumpWidth - 48, 87);

                AddHtml(30, upgradeY + 8, 350, 20, $"<BASEFONT COLOR=#82E0AA><BIG><B>Pronto para Evoluir para o Nível {next}!</B></BIG></BASEFONT>", false, false);

                // Ouro
                AddItem(30, upgradeY + 36, 0xEEF);
                bool hasGold = AscensionUnlocking.HasGold(m_From, gold);
                string goldColor = hasGold ? "#2ECC71" : "#E74C3C";
                AddHtml(65, upgradeY + 38, 120, 20, $"<BASEFONT COLOR={goldColor}><B>{gold} Ouro</B></BASEFONT>", false, false);

                // Poeira Arcana
                AddItem(190, upgradeY + 36, 0x0F8F);
                bool hasDust = AscensionUnlocking.HasArcaneDust(m_From, dust);
                string dustColor = hasDust ? "#2ECC71" : "#E74C3C";
                AddHtml(225, upgradeY + 38, 130, 20, $"<BASEFONT COLOR={dustColor}><B>{dust} Poeira</B></BASEFONT>", false, false);

                // Pergaminhos
                AddItem(365, upgradeY + 36, 0xEF3, AscensionVisuals.GetItemHue(m_Type));
                bool hasScrolls = AscensionUnlocking.HasScrolls(m_From, m_Type, scrolls);
                string scrollColor = hasScrolls ? "#2ECC71" : "#E74C3C";
                AddHtml(400, upgradeY + 38, 130, 20, $"<BASEFONT COLOR={scrollColor}><B>{scrolls} Pergaminho{(scrolls != 1 ? "s" : "")}</B></BASEFONT>", false, false);

                // Botão de Upgrade
                AddButton(535, upgradeY + 34, 0xFA5, 0xFA7, 3000 + (int)m_Type, GumpButtonType.Reply, 0);
                AddHtml(570, upgradeY + 36, 80, 20, "<BASEFONT COLOR=#F4D03F><B>Evoluir</B></BASEFONT>", false, false);
            }
            else if (m_Progress.Level >= AscensionConstants.MaxAscensionLevel)
            {
                AddBackground(20, upgradeY, gumpWidth - 40, 70, 0x2422);
                AddAlphaRegion(24, upgradeY + 4, gumpWidth - 48, 62);
                AddHtml(20, upgradeY + 24, gumpWidth - 40, 22, "<CENTER><BASEFONT COLOR=#F4D03F><BIG><B>★ ASCENSÃO MESTRA (NÍVEL MÁXIMO ATINGIDO) ★</B></BIG></BASEFONT></CENTER>", false, false);
            }
            else
            {
                AddBackground(20, upgradeY, gumpWidth - 40, 70, 0x2422);
                AddAlphaRegion(24, upgradeY + 4, gumpWidth - 48, 62);
                AddHtml(20, upgradeY + 24, gumpWidth - 40, 22, "<CENTER><BASEFONT COLOR=#BDC3C7><I>Ganhe mais experiência em combate para liberar a próxima evolução de nível.</I></BASEFONT></CENTER>", false, false);
            }

            // ========================================================
            // PÁGINA 2: Habilidades Ativas com Cards e Ícones Visuais
            // ========================================================
            AddPage(2);

            List<AscensionAbilityEntry> activeAbilities = AscensionAbilityCatalog.GetActiveAbilities(m_Type);
            RenderAbilityCards(activeAbilities, 114, gumpWidth);

            // ========================================================
            // PÁGINA 3: Habilidades Passivas com Cards e Ícones Visuais
            // ========================================================
            AddPage(3);

            List<AscensionAbilityEntry> passiveAbilities = AscensionAbilityCatalog.GetPassiveAbilities(m_Type);
            RenderAbilityCards(passiveAbilities, 114, gumpWidth);
        }

        private void RenderAbilityCards(List<AscensionAbilityEntry> abilities, int startY, int gumpWidth)
        {
            int cardY = startY;
            const int cardHeight = 118;

            for (int i = 0; i < abilities.Count; i++)
            {
                if (cardY + cardHeight > 610)
                    break;

                AscensionAbilityEntry entry = abilities[i];
                bool isUnlocked = m_Progress.Level >= entry.Level;

                // Fundo do card
                AddBackground(20, cardY, gumpWidth - 40, cardHeight, 0x2422);
                AddImageTiled(24, cardY + 4, gumpWidth - 48, cardHeight - 8, 0xA40);
                AddAlphaRegion(24, cardY + 4, gumpWidth - 48, cardHeight - 8);

                // Destaque luminoso superior se estiver desbloqueada pelo personagem
                if (isUnlocked)
                {
                    AddImageTiled(20, cardY, gumpWidth - 40, 2, 0x805);
                }

                // Slot de ícone da habilidade
                AddBackground(26, cardY + 8, 48, 48, 0x2422);
                AddImageTiled(28, cardY + 10, 44, 44, 0xA40);
                AddAlphaRegion(28, cardY + 10, 44, 44);

                // Arte do Item representativo
                AddItem(32, cardY + 14, entry.IconArt, entry.IconHue);

                // Linha de Cabeçalho da Habilidade: Nome, Nível, Comando e Status
                string statusText = isUnlocked
                    ? "<BASEFONT COLOR=#2ECC71><B>✔ Desbloqueada</B></BASEFONT>"
                    : $"<BASEFONT COLOR=#E74C3C><B>🔒 Requer Nível {entry.Level}</B></BASEFONT>";

                string commandBadge = !string.IsNullOrEmpty(entry.Command)
                    ? $" <BASEFONT COLOR=#5DADE2><SMALL>[{entry.Command}]</SMALL></BASEFONT>"
                    : " <BASEFONT COLOR=#A569BD><SMALL>[Passiva]</SMALL></BASEFONT>";

                AddHtml(82, cardY + 8, 380, 20,
                    $"<BIG><BASEFONT COLOR=#F4D03F><B>{entry.Name}</B></BASEFONT></BIG> <BASEFONT COLOR=#82E0AA>[Nível {entry.Level}]</BASEFONT>{commandBadge}",
                    false, false);

                AddHtml(gumpWidth - 220, cardY + 8, 190, 20,
                    $"<RIGHT>{statusText}</RIGHT>",
                    false, false);

                // Divisória sutil interna
                AddImageTiled(82, cardY + 30, gumpWidth - 110, 1, 0xBB8);

                // Descrição formatada da habilidade
                AddHtml(82, cardY + 34, gumpWidth - 110, cardHeight - 42,
                    $"<BASEFONT COLOR=#E5E7E9>{entry.Description}</BASEFONT>",
                    false, true);

                cardY += cardHeight + 6;
            }
        }

        private string BuildRequirementsText()
        {
            AscensionDefinition def = AscensionRegistry.Get(m_Type);
            if (def == null)
                return "<BASEFONT COLOR=#A0A0A0>Nenhum requisito especial catalogado.</BASEFONT>";

            int targetLevel = m_Progress.Level > 0 ? m_Progress.Level : 1;
            int reqValue = def.GetRequiredSkillValue(targetLevel);

            StringBuilder sb = new StringBuilder();
            sb.Append("<BASEFONT COLOR=#FFFFFF><B>Status de Requisitos (Nível ").Append(targetLevel).Append("):</B><br>");

            // Habilidades Requeridas
            if (def.RequiredSkills != null && def.RequiredSkills.Length > 0)
            {
                sb.Append("<BASEFONT COLOR=#BDC3C7>Requer (").Append(reqValue).Append("): </BASEFONT>");
                for (int i = 0; i < def.RequiredSkills.Length; i++)
                {
                    SkillName sk = def.RequiredSkills[i];
                    double current = m_From.Skills[sk].Base;
                    bool ok = current >= reqValue;
                    string color = ok ? "#2ECC71" : "#E74C3C";
                    sb.Append("<BASEFONT COLOR=").Append(color).Append(">");
                    sb.Append(sk.ToString()).Append(" [").Append((int)current).Append("]");
                    sb.Append(ok ? " ✔" : " ✘");
                    sb.Append("</BASEFONT>");
                    if (i < def.RequiredSkills.Length - 1)
                        sb.Append(", ");
                }
                sb.Append("<br>");
            }

            // Habilidades Proibidas
            if (def.ForbiddenSkills != null && def.ForbiddenSkills.Length > 0)
            {
                sb.Append("<BASEFONT COLOR=#BDC3C7>Proibidas: </BASEFONT>");
                bool hasAnyViolation = false;
                for (int i = 0; i < def.ForbiddenSkills.Length; i++)
                {
                    SkillName sk = def.ForbiddenSkills[i];
                    double current = m_From.Skills[sk].Base;
                    if (current > 0)
                    {
                        hasAnyViolation = true;
                        sb.Append("<BASEFONT COLOR=#E74C3C><B>").Append(sk.ToString()).Append(" [").Append((int)current).Append("] ✘</B></BASEFONT> ");
                    }
                }

                if (!hasAnyViolation)
                {
                    sb.Append("<BASEFONT COLOR=#2ECC71>Nenhuma habilidade proibida aprendida ✔</BASEFONT>");
                }
            }

            return sb.ToString();
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            switch (info.ButtonID)
            {
                case Button_Back:
                    m_From.SendGump(new AscensionSelectionGump(m_From));
                    return;

                case Button_Activate:
                    m_From.ActivateAscension(m_Type);
                    Refresh();
                    return;
            }

            if (info.ButtonID >= 3000)
            {
                AscensionType type = (AscensionType)(info.ButtonID - 3000);
                m_From.SendGump(new AscensionUpgradeConfirmGump(m_From, type));
            }
        }

        private void Refresh()
        {
            m_From.SendGump(new AscensionDetailGump(m_From, m_Type));
        }
    }
}
