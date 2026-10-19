using System;
using System.Collections.Generic;
using Server;
using Server.Gumps;
using Server.Mobiles;

namespace Server.Custom.Ascensions
{
    public class AscensionQuickbarGump : Gump
    {
        private PlayerMobile _pm;

        private const int GumpWidth = 260;
        private const int RowHeight = 32;

        public AscensionQuickbarGump(PlayerMobile pm) : base(20, 200)
        {
            _pm = pm;

            Closable = true;
            Disposable = true;
            Dragable = true;
            Resizable = false;

            AscensionType activeType = pm.ActiveAscension;
            AscensionProgress prog = pm.AscensionProfile.Get(activeType);

            List<AscensionAbility> usable = GetUsableAbilities(pm);
            int rowsCount = Math.Max(1, usable.Count);

            int headerHeight = 72;
            int totalHeight = headerHeight + (rowsCount * RowHeight) + 12;

            AddPage(0);

            // Moldura da barra de ação
            AddBackground(0, 0, GumpWidth, totalHeight, 0x1453);
            AddImageTiled(6, 6, GumpWidth - 12, totalHeight - 12, 0xA40);
            AddAlphaRegion(6, 6, GumpWidth - 12, totalHeight - 12);

            // Ícone da classe ativa e cabeçalho
            AscensionVisuals.DrawClassIcon(this, 10, 10, activeType, 36);

            string hexColor = AscensionVisuals.GetColorHex(activeType);
            string displayName = AscensionTypeHelper.GetDisplayName(activeType);
            AddHtml(52, 10, 195, 20, $"<BASEFONT COLOR={hexColor}><B>{displayName}</B></BASEFONT> <BASEFONT COLOR=#82E0AA>[Nvl {prog.Level}]</BASEFONT>", false, false);

            // Mini barra de progresso de experiência
            int required = prog.GetRequiredExperience();
            double pct = required > 0 ? Math.Min(1.0, (double)prog.Experience / required) : 1.0;
            string xpText = required > 0 ? $"{prog.Experience}/{required}" : "MAX";

            AscensionVisuals.DrawProgressBar(this, 52, 28, 196, 14, pct, xpText);

            // Divisória sutil
            AddImageTiled(10, 52, GumpWidth - 20, 1, 0xBB8);

            // Linhas de Habilidades
            int y = 58;
            int buttonID = 1;

            if (usable.Count == 0)
            {
                AddHtml(12, y + 4, GumpWidth - 24, 20, "<CENTER><BASEFONT COLOR=#8E999E><SMALL><I>Nenhuma habilidade ativa</I></SMALL></BASEFONT></CENTER>", false, false);
            }
            else
            {
                foreach (AscensionAbility ability in usable)
                {
                    // Fundo da linha de habilidade
                    AddBackground(10, y, GumpWidth - 20, 28, 0x2422);
                    AddImageTiled(12, y + 2, GumpWidth - 24, 24, 0xA40);
                    AddAlphaRegion(12, y + 2, GumpWidth - 24, 24);

                    // Botão de ativação
                    AddButton(14, y + 4, 0xFA5, 0xFA7, buttonID, GumpButtonType.Reply, 0);

                    // Nome da Habilidade
                    AddHtml(44, y + 4, GumpWidth - 54, 20, $"<BASEFONT COLOR=#E5E7E9><B>{ability.DisplayName}</B></BASEFONT>", false, false);

                    y += RowHeight;
                    buttonID++;
                }
            }
        }

        public override void OnResponse(Server.Network.NetState sender, RelayInfo info)
        {
            if (_pm == null || _pm.Deleted)
                return;

            List<AscensionAbility> usable = GetUsableAbilities(_pm);

            int index = info.ButtonID - 1;

            if (index >= 0 && index < usable.Count)
            {
                usable[index].Execute(_pm);

                _pm.CloseGump(typeof(AscensionQuickbarGump));
                _pm.SendGump(new AscensionQuickbarGump(_pm));
            }
        }

        private static List<AscensionAbility> GetUsableAbilities(PlayerMobile pm)
        {
            List<AscensionAbility> abilities = AscensionAbilityRegistry.GetAbilities(pm.ActiveAscension);
            List<AscensionAbility> usable = new List<AscensionAbility>();

            foreach (AscensionAbility ability in abilities)
            {
                if (ability.IsPassive)
                    continue;

                if (!ability.CanUse(pm))
                    continue;

                usable.Add(ability);
            }

            return usable;
        }
    }
}