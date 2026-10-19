using System;
using Server;
using Server.Gumps;

namespace Server.Custom.Ascensions
{
    public static class AscensionVisuals
    {
        public static int GetItemArt(AscensionType type)
        {
            switch (type)
            {
                case AscensionType.Berserker:    return 0xF47;  // Battle Axe
                case AscensionType.Archmage:     return 0xEFA;  // Spellbook
                case AscensionType.Battlemage:   return 0x0F5E; // Broadsword
                case AscensionType.Archdruid:    return 0x13F8; // Gnarled Staff
                case AscensionType.Palemaster:   return 0x1AE2; // Skull
                case AscensionType.Crusader:     return 0x1BC3; // Order Shield
                case AscensionType.Kensai:       return 0x13FF; // Katana
                case AscensionType.Skald:        return 0x0EB2; // Harp
                case AscensionType.Assassin:     return 0x0F51; // Dagger
                case AscensionType.Reaver:       return 0x26BA; // Scythe
                case AscensionType.ArcaneArcher: return 0x2D1E; // Elven Composite Bow
                case AscensionType.Hierophant:   return 0x0004; // Ankh
                case AscensionType.Blackguard:   return 0x1BC4; // Chaos Shield
                default:                         return 0x2259; // Tome
            }
        }

        public static int GetItemHue(AscensionType type)
        {
            switch (type)
            {
                case AscensionType.Berserker:    return 0x21;  // Blood Crimson
                case AscensionType.Archmage:     return 0x48D; // Arcane Blue
                case AscensionType.Battlemage:   return 0x488; // Cyan Fire
                case AscensionType.Archdruid:    return 0x59;  // Forest Green
                case AscensionType.Palemaster:   return 0x455; // Pale Bone/Necrotic
                case AscensionType.Crusader:     return 0x481; // Holy Gold
                case AscensionType.Kensai:       return 0x0;   // Pure Steel
                case AscensionType.Skald:        return 0x53;  // Bronze/Amber
                case AscensionType.Assassin:     return 0x44;  // Poison Emerald
                case AscensionType.Reaver:       return 0x20;  // Dark Crimson
                case AscensionType.ArcaneArcher: return 0x489; // Violet
                case AscensionType.Hierophant:   return 0x482; // Divine Gold/Light
                case AscensionType.Blackguard:   return 0x497; // Abyssal Purple/Black
                default:                         return 0x0;
            }
        }

        public static string GetColorHex(AscensionType type)
        {
            switch (type)
            {
                case AscensionType.Berserker:    return "#E74C3C"; // Red
                case AscensionType.Archmage:     return "#5DADE2"; // Light Blue
                case AscensionType.Battlemage:   return "#48C9B0"; // Teal
                case AscensionType.Archdruid:    return "#52BE80"; // Moss Green
                case AscensionType.Palemaster:   return "#A2D9CE"; // Pale Cyan/Green
                case AscensionType.Crusader:     return "#F4D03F"; // Gold
                case AscensionType.Kensai:       return "#EBEDEF"; // Silver/White
                case AscensionType.Skald:        return "#F39C12"; // Amber
                case AscensionType.Assassin:     return "#2ECC71"; // Poison Green
                case AscensionType.Reaver:       return "#C0392B"; // Dark Red
                case AscensionType.ArcaneArcher: return "#BB8FCE"; // Violet
                case AscensionType.Hierophant:   return "#F7DC6F"; // Warm Gold
                case AscensionType.Blackguard:   return "#AF7AC5"; // Dark Violet
                default:                         return "#FFFFFF";
            }
        }

        public static string GetSubTitle(AscensionType type)
        {
            switch (type)
            {
                case AscensionType.Berserker:    return "Fúria Selvagem & Armas de Duas Mãos";
                case AscensionType.Archmage:     return "Mestre Supremo das Artes Arcanas";
                case AscensionType.Battlemage:   return "Guerreiro Arcano Híbrido";
                case AscensionType.Archdruid:    return "Comunhão com a Natureza & Feras";
                case AscensionType.Palemaster:   return "Senhor dos Mortos & Maldições";
                case AscensionType.Crusader:     return "Guerreiro Sagrado da Virtude";
                case AscensionType.Kensai:       return "Mestre da Lâmina & Bushido";
                case AscensionType.Skald:        return "Bardo Guerreiro dos Cânticos";
                case AscensionType.Assassin:     return "Mestre das Sombras & Venenos";
                case AscensionType.Reaver:       return "Combatente Sangrento Incapacitante";
                case AscensionType.ArcaneArcher: return "Atirador de Projéteis Mágicos";
                case AscensionType.Hierophant:   return "Canalizador do Poder Divino";
                case AscensionType.Blackguard:   return "Campeão das Trevas & Corrupção";
                default:                         return "Caminho da Ascensão";
            }
        }

        public static void DrawClassIcon(Gump gump, int x, int y, AscensionType type, int slotSize = 46)
        {
            // Moldura externa escura
            gump.AddBackground(x, y, slotSize, slotSize, 0x2422);
            gump.AddImageTiled(x + 2, y + 2, slotSize - 4, slotSize - 4, 0xA40);
            gump.AddAlphaRegion(x + 2, y + 2, slotSize - 4, slotSize - 4);

            int itemID = GetItemArt(type);
            int itemHue = GetItemHue(type);

            // Item centralizado na moldura
            gump.AddItem(x + 4, y + 5, itemID, itemHue);
        }

        public static void DrawProgressBar(Gump gump, int x, int y, int width, int height, double percentage, string label = null)
        {
            // Moldura de fundo da barra
            gump.AddBackground(x, y, width, height, 0x2422);
            gump.AddImageTiled(x + 2, y + 2, width - 4, height - 4, 0xA40);
            gump.AddAlphaRegion(x + 2, y + 2, width - 4, height - 4);

            double clamped = Math.Max(0.0, Math.Min(1.0, percentage));
            int fillWidth = (int)((width - 4) * clamped);

            if (fillWidth > 0)
            {
                // Barra de preenchimento (usando 0x805/0x806 ou padrão tiled UO)
                gump.AddImageTiled(x + 2, y + 2, fillWidth, height - 4, 0x805);
            }

            if (!string.IsNullOrEmpty(label))
            {
                gump.AddHtml(x, y + (height - 18) / 2, width, 18,
                    $"<CENTER><BASEFONT COLOR=#FFFFFF><SMALL><B>{label}</B></SMALL></BASEFONT></CENTER>",
                    false, false);
            }
        }
    }
}
