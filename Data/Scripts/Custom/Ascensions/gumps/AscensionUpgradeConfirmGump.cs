using System;
using Server;
using Server.Gumps;
using Server.Mobiles;
using Server.Network;
using Server.Custom.Ascensions;

namespace Server.Custom.Ascensions
{
    public class AscensionUpgradeConfirmGump : Gump
    {
        private PlayerMobile m_Player;
        private AscensionType m_Type;

        public AscensionUpgradeConfirmGump(PlayerMobile pm, AscensionType type)
            : base(150, 150)
        {
            m_Player = pm;
            m_Type = type;

            Closable = true;
            Disposable = true;
            Dragable = true;
            Resizable = false;

            const int width = 440;
            const int height = 260;

            AddPage(0);

            // Moldura principal
            AddBackground(0, 0, width, height, 0x1453);
            AddImageTiled(10, 10, width - 20, height - 20, 0xA40);
            AddAlphaRegion(10, 10, width - 20, height - 20);

            // Ícone da classe
            AscensionVisuals.DrawClassIcon(this, 25, 20, type, 46);

            AscensionProgress prog = pm.AscensionProfile.Get(type);
            bool isUnlock = !prog.Unlocked || prog.Level == 0;
            int nextLevel = prog.Level + 1;

            string hexColor = AscensionVisuals.GetColorHex(type);
            string displayName = AscensionTypeHelper.GetDisplayName(type);

            string titleText = isUnlock ? "Desbloqueio de Ascensão" : "Evolução de Nível";
            AddHtml(80, 20, 330, 22, $"<BIG><BASEFONT COLOR={hexColor}><B>{titleText}</B></BASEFONT></BIG>", false, false);

            string promptText = isUnlock
                ? $"Deseja desbloquear a classe <B>{displayName}</B>?"
                : $"Deseja evoluir <B>{displayName}</B> para o <B>Nível {nextLevel}</B>?";
            AddHtml(80, 42, 330, 20, $"<BASEFONT COLOR=#BDC3C7>{promptText}</BASEFONT>", false, false);

            // Divisória
            AddImageTiled(20, 74, width - 40, 2, 0xBB8);

            // Custos
            int gold, dust, scrolls;
            if (isUnlock)
            {
                AscensionUnlocking.GetUnlockCost(pm, out gold, out dust, out scrolls);
            }
            else
            {
                gold = AscensionCosts.GetGoldCost(nextLevel);
                dust = AscensionCosts.GetDustCost(nextLevel);
                scrolls = AscensionCosts.GetScrollCost(nextLevel);
            }

            // Exibição dos 3 recursos em cards estilizados
            int resY = 90;

            // Ouro
            AddBackground(25, resY, 120, 80, 0x2422);
            AddImageTiled(28, resY + 3, 114, 74, 0xA40);
            AddAlphaRegion(28, resY + 3, 114, 74);
            AddItem(68, resY + 12, 0xEEF);
            bool hasGold = AscensionUnlocking.HasGold(pm, gold);
            string goldColor = hasGold ? "#2ECC71" : "#E74C3C";
            AddHtml(25, resY + 52, 120, 20, $"<CENTER><BASEFONT COLOR={goldColor}><B>{gold} Ouro</B></BASEFONT></CENTER>", false, false);

            // Poeira Arcana
            AddBackground(160, resY, 120, 80, 0x2422);
            AddImageTiled(163, resY + 3, 114, 74, 0xA40);
            AddAlphaRegion(163, resY + 3, 114, 74);
            AddItem(203, resY + 12, 0x0F8F);
            bool hasDust = AscensionUnlocking.HasArcaneDust(pm, dust);
            string dustColor = hasDust ? "#2ECC71" : "#E74C3C";
            AddHtml(160, resY + 52, 120, 20, $"<CENTER><BASEFONT COLOR={dustColor}><B>{dust} Poeira</B></BASEFONT></CENTER>", false, false);

            // Pergaminhos
            AddBackground(295, resY, 120, 80, 0x2422);
            AddImageTiled(298, resY + 3, 114, 74, 0xA40);
            AddAlphaRegion(298, resY + 3, 114, 74);
            AddItem(338, resY + 12, 0xEF3, AscensionVisuals.GetItemHue(type));
            bool hasScrolls = AscensionUnlocking.HasScrolls(pm, type, scrolls);
            string scrollColor = hasScrolls ? "#2ECC71" : "#E74C3C";
            AddHtml(295, resY + 52, 120, 20, $"<CENTER><BASEFONT COLOR={scrollColor}><B>{scrolls} Pergaminho{(scrolls != 1 ? "s" : "")}</B></BASEFONT></CENTER>", false, false);

            // Divisória inferior
            AddImageTiled(20, 185, width - 40, 1, 0xBB8);

            // Botões de Confirmação e Cancelamento
            int btnY = 202;

            // Confirmar
            AddButton(70, btnY, 0xFA5, 0xFA7, 1, GumpButtonType.Reply, 0);
            AddHtml(105, btnY + 2, 90, 20, "<BASEFONT COLOR=#2ECC71><B>Confirmar</B></BASEFONT>", false, false);

            // Cancelar
            AddButton(250, btnY, 0xFB1, 0xFB2, 0, GumpButtonType.Reply, 0);
            AddHtml(285, btnY + 2, 90, 20, "<BASEFONT COLOR=#E74C3C><B>Cancelar</B></BASEFONT>", false, false);
        }

        public override void OnResponse(Server.Network.NetState sender, RelayInfo info)
        {
            if (info.ButtonID == 1)
            {
                if (m_Player.AscensionProfile.IsUnlocked(m_Type))
                {
                    AscensionUpgradeSystem.TryUpgrade(m_Player, m_Type);
                }
                else
                {
                    AscensionUnlocking.TryUnlock(m_Player, m_Type);
                }
            }

            m_Player.SendGump(new AscensionDetailGump(m_Player, m_Type));
        }
    }
}