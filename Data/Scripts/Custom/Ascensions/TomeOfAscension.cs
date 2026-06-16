using System;
using System.Collections.Generic;
using Server;
using Server.ContextMenus;
using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Custom.Ascensions
{
    public class TomeOfAscension : Item
    {
        private Dictionary<AscensionType, int> m_Entries;

        [CommandProperty(AccessLevel.GameMaster)]
        public Dictionary<AscensionType, int> Entries
        {
            get { return m_Entries; }
        }

        [Constructable]
        public TomeOfAscension()
            : base(0x2259)
        {
            Weight = 1.0;
            Hue = 0x439;
            Name = "Tomo da Ascensão";
            LootType = LootType.Blessed;

            m_Entries = new Dictionary<AscensionType, int>();
        }

        public override void OnSingleClick(Mobile from)
        {
            base.OnSingleClick(from);
        }

        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            base.GetContextMenuEntries(from, list);

            if (from.CheckAlive() && IsChildOf(from.Backpack))
            {
                list.Add(new OrganizeScrollsEntry(from, this));
            }
        }

        private class OrganizeScrollsEntry : ContextMenuEntry
        {
            private Mobile m_From;
            private TomeOfAscension m_Tome;

            public OrganizeScrollsEntry(Mobile from, TomeOfAscension tome)
                : base(0097)
            {
                m_From = from;
                m_Tome = tome;
            }

            public override void OnClick()
            {
                if (m_From.CheckAlive() && m_Tome.IsChildOf(m_From.Backpack))
                {
                    int totalAdded = m_Tome.CollectAscensionScrolls(m_From);

                    if (totalAdded > 0)
                    {
                        m_From.SendSound(0x42, m_Tome.GetWorldLocation());
                        m_From.SendMessage("Você organizou {0} pergaminho{1} de ascensão no tomo.", totalAdded, totalAdded != 1 ? "s" : "");
                        if (m_From is PlayerMobile)
                            m_From.SendGump(new TomeOfAscensionGump((PlayerMobile)m_From, m_Tome));
                    }
                    else
                    {
                        m_From.SendMessage("Não há pergaminhos de ascensão em sua mochila para coletar.");
                    }
                }
            }
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!from.InRange(GetWorldLocation(), 2))
            {
                from.LocalOverheadMessage(MessageType.Regular, 0x3B2, 1019045); // I can't reach that.
                return;
            }

            if (m_Entries.Count == 0)
            {
                from.SendMessage("O tomo está vazio.");
                return;
            }

            if (from is PlayerMobile)
            {
                from.SendGump(new TomeOfAscensionGump((PlayerMobile)from, this));
            }
        }

        public int CollectAscensionScrolls(Mobile from)
        {
            if (from == null || from.Backpack == null)
                return 0;
            List<Item> toDelete = new List<Item>();
            int totalAdded = 0;

            // Collect all ascension scrolls from backpack and nested containers
            List<Item> found = new List<Item>();
            GetAllScrollsInBackpack(from.Backpack, found);

            foreach (Item item in found)
            {
                if (item == null || item.Deleted)
                    continue;

                AscensionScroll scroll = item as AscensionScroll;
                if (scroll == null)
                    continue;

                AscensionType type = scroll.Ascension;

                if (m_Entries.ContainsKey(type))
                    m_Entries[type] += scroll.Amount;
                else
                    m_Entries[type] = scroll.Amount;

                totalAdded += scroll.Amount;
                toDelete.Add(item);
            }

            foreach (Item item in toDelete)
            {
                item.Delete();
            }

            if (totalAdded > 0)
                InvalidateProperties();

            return totalAdded;
        }

        private void GetAllScrollsInBackpack(Container container, List<Item> list)
        {
            if (container == null)
                return;

            foreach (Item item in container.Items)
            {
                if (item == null || item.Deleted)
                    continue;

                if (item is Container)
                    GetAllScrollsInBackpack((Container)item, list);
                else if (item is AscensionScroll)
                    list.Add(item);
            }
        }

        private AscensionScroll FindExistingScrollStack(Container pack, AscensionType type)
        {
            if (pack == null)
                return null;

            for (int i = 0; i < pack.Items.Count; i++)
            {
                AscensionScroll scroll = pack.Items[i] as AscensionScroll;
                if (scroll != null && scroll.Ascension == type)
                    return scroll;
            }

            return null;
        }

        public override bool OnDragDrop(Mobile from, Item dropped)
        {
            AscensionScroll scroll = dropped as AscensionScroll;

            if (scroll == null)
                return false;

            if (!IsChildOf(from.Backpack))
            {
                from.SendMessage("O tomo deve estar em sua mochila para adicionar pergaminhos a ele.");
                return false;
            }

            AscensionType type = scroll.Ascension;
            int amount = scroll.Amount;

            if (m_Entries.ContainsKey(type))
                m_Entries[type] += amount;
            else
                m_Entries[type] = amount;

            scroll.Delete();
            InvalidateProperties();
            from.SendSound(0x42, GetWorldLocation());
            from.SendMessage("Pergaminho de ascensão adicionado ao tomo.");

            if (from is PlayerMobile)
                from.SendGump(new TomeOfAscensionGump((PlayerMobile)from, this));

            return true;
        }

        public void WithdrawScrolls(Mobile from, AscensionType type, int amount)
        {
            if (!IsChildOf(from.Backpack))
            {
                from.SendMessage("O tomo deve estar em sua mochila para sacar pergaminhos.");
                return;
            }

            if (!m_Entries.ContainsKey(type))
            {
                from.SendMessage("Não há pergaminhos desse tipo no tomo.");
                return;
            }

            if (amount <= 0)
            {
                from.SendMessage("Invalid amount.");
                return;
            }

            int available = m_Entries[type];
            if (amount > available)
            {
                from.SendMessage("Você só tem {0} pergaminho{1} desse tipo.", available, available != 1 ? "s" : "");
                return;
            }

            Container pack = from.Backpack;
            if (pack == null)
                return;

            AscensionScroll existing = FindExistingScrollStack(pack, type);

            if (existing != null)
            {
                existing.Amount += amount;
            }
            else
            {
                AscensionScroll scroll = new AscensionScroll(type);
                scroll.Amount = amount;
                pack.DropItem(scroll);
            }

            m_Entries[type] -= amount;
            if (m_Entries[type] <= 0)
                m_Entries.Remove(type);

            InvalidateProperties();
            from.SendSound(0x55, GetWorldLocation());
            from.SendMessage("Você sacou {0} pergaminho{1} de ascensão do tomo.", amount, amount != 1 ? "s" : "");

            if (from is PlayerMobile)
                from.SendGump(new TomeOfAscensionGump((PlayerMobile)from, this));
        }

        public TomeOfAscension(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version

            writer.WriteEncodedInt(m_Entries.Count);
            foreach (KeyValuePair<AscensionType, int> kvp in m_Entries)
            {
                writer.Write((int)kvp.Key);
                writer.Write((int)kvp.Value);
            }
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch (version)
            {
                case 0:
                    {
                        int count = reader.ReadEncodedInt();
                        m_Entries = new Dictionary<AscensionType, int>();

                        for (int i = 0; i < count; ++i)
                        {
                            AscensionType type = (AscensionType)reader.ReadInt();
                            int amount = reader.ReadInt();
                            m_Entries[type] = amount;
                        }

                        break;
                    }
            }
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);

            int totalScrolls = 0;
            foreach (KeyValuePair<AscensionType, int> kvp in m_Entries)
            {
                totalScrolls += kvp.Value;
            }

            list.Add("Pergaminhos de Ascensão: {0}", totalScrolls);
        }
    }

    public class TomeOfAscensionGump : Gump
    {
        private PlayerMobile m_From;
        private TomeOfAscension m_Tome;
        private List<KeyValuePair<AscensionType, int>> m_SortedEntries;

        public TomeOfAscensionGump(PlayerMobile from, TomeOfAscension tome)
            : base(50, 50)
        {
            m_From = from;
            m_Tome = tome;

            m_SortedEntries = new List<KeyValuePair<AscensionType, int>>(tome.Entries);
            m_SortedEntries.Sort((a, b) => a.Key.ToString().CompareTo(b.Key.ToString()));

            BuildGump();
        }

        private void BuildGump()
        {
            int entryCount = m_SortedEntries.Count;
            int height = Math.Max(250, 100 + (entryCount * 30));
            if (height > 520)
                height = 520;

            Closable = true;
            Disposable = true;
            Dragable = true;
            Resizable = false;

            AddPage(0);
            AddBackground(0, 0, 380, height, 9270);
            AddLabel(20, 20, 1152, "Tomo da Ascensão");
            AddLabel(20, 45, 0x480, String.Format("Total Scrolls: {0}", GetTotalScrollCount()));

            int yPos = 80;
            for (int i = 0; i < entryCount; i++)
            {
                KeyValuePair<AscensionType, int> entry = m_SortedEntries[i];

                AddLabel(20, yPos, 0x480, entry.Key.ToString());
                AddLabel(180, yPos, 0x480, String.Format("{0}", entry.Value));
                AddButton(300, yPos - 2, 4005, 4007, i + 1, GumpButtonType.Reply, 0);

                yPos += 30;
            }
        }

        private int GetTotalScrollCount()
        {
            int total = 0;
            foreach (KeyValuePair<AscensionType, int> kvp in m_SortedEntries)
                total += kvp.Value;
            return total;
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            if (info.ButtonID == 0)
                return;

            int index = info.ButtonID - 1;
            if (index < 0 || index >= m_SortedEntries.Count)
                return;

            KeyValuePair<AscensionType, int> entry = m_SortedEntries[index];

            if (entry.Value == 1)
            {
                m_Tome.WithdrawScrolls(m_From, entry.Key, 1);
            }
            else
            {
                m_From.SendGump(new TomeOfAscensionWithdrawGump(m_From, m_Tome, entry.Key, entry.Value));
            }
        }
    }

    public class TomeOfAscensionWithdrawGump : Gump
    {
        private PlayerMobile m_From;
        private TomeOfAscension m_Tome;
        private AscensionType m_Type;
        private int m_Available;

        public TomeOfAscensionWithdrawGump(PlayerMobile from, TomeOfAscension tome, AscensionType type, int available)
            : base(150, 150)
        {
            m_From = from;
            m_Tome = tome;
            m_Type = type;
            m_Available = available;

            Closable = true;
            Disposable = true;
            Dragable = true;
            Resizable = false;

            AddPage(0);
            AddBackground(0, 0, 350, 220, 9270);
            AddLabel(20, 20, 1152, "Sacar Pergaminhos de Ascensão");

            string message = String.Format("Você tem {0} {1} pergaminho{2}.<br>Quantos gostaria de sacar?",
                available, type.ToString(), available != 1 ? "s" : "");

            AddHtml(20, 50, 310, 60, message, true, true);
            AddLabel(20, 120, 0x480, "Quantidade:");
            AddBackground(100, 120, 100, 25, 9350);
            AddTextEntry(105, 120, 90, 20, 0, 0, "");

            AddButton(60, 160, 247, 248, 1, GumpButtonType.Reply, 0); // Withdraw
            AddButton(200, 160, 241, 242, 0, GumpButtonType.Reply, 0); // Cancel
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            if (info.ButtonID != 1)
            {
                m_From.SendGump(new TomeOfAscensionGump(m_From, m_Tome));
                return;
            }

            TextRelay entry = info.GetTextEntry(0);
            if (entry == null || String.IsNullOrEmpty(entry.Text))
            {
                m_From.SendMessage("Quantidade inválida. Por favor, insira um número válido.");
                m_From.SendGump(new TomeOfAscensionGump(m_From, m_Tome));
                return;
            }

            int amount;
            if (!int.TryParse(entry.Text, out amount) || amount <= 0 || amount > m_Available)
            {
                m_From.SendMessage("Quantidade inválida. Por favor, insira um número entre 1 e {0}.", m_Available);
                m_From.SendGump(new TomeOfAscensionGump(m_From, m_Tome));
                return;
            }

            m_Tome.WithdrawScrolls(m_From, m_Type, amount);
        }
    }
}