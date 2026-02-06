using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Gumps;
using Server.Prompts;
using Server.Mobiles;
using Server.ContextMenus;
using Server.Items;
using Server.Network;
using Server.Multis;

namespace Server.Items
{
    public class TomeOfPower : Item, ISecurable
    {
        private Dictionary<SkillName, int> m_Entries;
        private string m_BookName;
        private SecureLevel m_Level;

        [CommandProperty(AccessLevel.GameMaster)]
        public string BookName
        {
            get { return m_BookName; }
            set { m_BookName = value; InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public SecureLevel Level
        {
            get { return m_Level; }
            set { m_Level = value; }
        }

        public Dictionary<SkillName, int> Entries
        {
            get { return m_Entries; }
        }

        [Constructable]
        public TomeOfPower()
            : base(0x2259)
        {
            Weight = 1.0;
            Hue = 23;
            Name = "Tome of Power";
            LootType = LootType.Blessed;

            m_Entries = new Dictionary<SkillName, int>();
            m_Level = SecureLevel.CoOwners;
        }

        public override void OnSingleClick(Mobile from)
        {
            base.OnSingleClick(from);

            if (this.Weight > 1.0)
            {
                LabelTo(from, "Single Click to Organize");
            }
        }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            if (this.Weight > 1.0)
            {
                list.Add(1070722, "Single Click to Organize");
            }
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!from.InRange(GetWorldLocation(), 2))
            {
                from.LocalOverheadMessage(Network.MessageType.Regular, 0x3B2, 1019045); // I can't reach that.
            }
            else if (m_Entries.Count == 0)
            {
                from.SendMessage("The tome is empty.");
            }
            else if (from is PlayerMobile)
            {
                from.SendGump(new TomeOfPowerGump((PlayerMobile)from, this));
            }
        }

        public void OrganizeScrolls(Mobile from)
        {
            if (!IsChildOf(from.Backpack))
            {
                from.SendMessage("The tome must be in your backpack to organize scrolls.");
                return;
            }

            Container pack = from.Backpack;
            if (pack == null)
                return;

            List<Item> toDelete = new List<Item>();
            int totalAdded = 0;

            foreach (Item item in pack.Items)
            {
                if (item is EtherealPowerScroll)
                {
                    EtherealPowerScroll scroll = (EtherealPowerScroll)item;
                    SkillName skill = scroll.Skill;

                    if (m_Entries.ContainsKey(skill))
                    {
                        m_Entries[skill]++;
                    }
                    else
                    {
                        m_Entries[skill] = 1;
                    }

                    toDelete.Add(item);
                    totalAdded++;
                }
            }

            foreach (Item item in toDelete)
            {
                item.Delete();
            }

            if (totalAdded > 0)
            {
                from.SendSound(0x42, GetWorldLocation());
                from.SendMessage("You have organized {0} power scroll{1} into the tome.", totalAdded, totalAdded != 1 ? "s" : "");
                InvalidateProperties();

                if (from is PlayerMobile)
                {
                    from.SendGump(new TomeOfPowerGump((PlayerMobile)from, this));
                }
            }
            else
            {
                from.SendMessage("There are no Ethereal Power Scrolls in your backpack to organize.");
            }
        }

        public override bool OnDragDrop(Mobile from, Item dropped)
        {
            if (dropped is EtherealPowerScroll)
            {
                if (!IsChildOf(from.Backpack))
                {
                    from.SendMessage("The tome must be in your backpack to add scrolls to it.");
                    return false;
                }

                EtherealPowerScroll scroll = (EtherealPowerScroll)dropped;
                SkillName skill = scroll.Skill;

                if (m_Entries.ContainsKey(skill))
                {
                    m_Entries[skill]++;
                }
                else
                {
                    m_Entries[skill] = 1;
                }

                InvalidateProperties();
                from.SendSound(0x42, GetWorldLocation());
                from.SendMessage("Power scroll added to tome.");

                if (from is PlayerMobile)
                {
                    from.SendGump(new TomeOfPowerGump((PlayerMobile)from, this));
                }

                dropped.Delete();
                return true;
            }

            from.SendMessage("That is not an Ethereal Power Scroll.");
            return false;
        }

        public void WithdrawScrolls(Mobile from, SkillName skill, int amount)
        {
            if (!IsChildOf(from.Backpack))
            {
                from.SendMessage("The tome must be in your backpack.");
                return;
            }

            if (!m_Entries.ContainsKey(skill))
            {
                from.SendMessage("There are no scrolls of that type in the tome.");
                return;
            }

            if (amount <= 0)
            {
                from.SendMessage("Invalid amount.");
                return;
            }

            int available = m_Entries[skill];
            if (amount > available)
            {
                from.SendMessage("You only have {0} scroll{1} of that type.", available, available != 1 ? "s" : "");
                return;
            }

            Container pack = from.Backpack;
            if (pack == null)
                return;

            // Check if backpack can hold the scrolls
            if (pack.TotalItems + amount > pack.MaxItems)
            {
                from.SendMessage("Your backpack cannot hold that many items.");
                return;
            }

            // Create the scrolls
            for (int i = 0; i < amount; i++)
            {
                EtherealPowerScroll scroll = new EtherealPowerScroll(skill);
                pack.DropItem(scroll);
            }

            // Update the entries
            m_Entries[skill] -= amount;
            if (m_Entries[skill] <= 0)
            {
                m_Entries.Remove(skill);
            }

            from.SendSound(0x55, GetWorldLocation());
            from.SendMessage("You have withdrawn {0} power scroll{1} from the tome.", amount, amount != 1 ? "s" : "");
            InvalidateProperties();

            if (from is PlayerMobile)
            {
                from.SendGump(new TomeOfPowerGump((PlayerMobile)from, this));
            }
        }

        public TomeOfPower(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version

            writer.Write((int)m_Level);
            writer.Write(m_BookName);

            writer.WriteEncodedInt(m_Entries.Count);
            foreach (KeyValuePair<SkillName, int> kvp in m_Entries)
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
                        m_Level = (SecureLevel)reader.ReadInt();
                        m_BookName = reader.ReadString();

                        int count = reader.ReadEncodedInt();
                        m_Entries = new Dictionary<SkillName, int>();

                        for (int i = 0; i < count; ++i)
                        {
                            SkillName skill = (SkillName)reader.ReadInt();
                            int amount = reader.ReadInt();
                            m_Entries[skill] = amount;
                        }

                        break;
                    }
            }
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);

            int totalScrolls = 0;
            foreach (KeyValuePair<SkillName, int> kvp in m_Entries)
            {
                totalScrolls += kvp.Value;
            }

            list.Add(1062344, totalScrolls.ToString()); // Deeds in book: ~1_val~

            if (m_BookName != null && m_BookName.Length > 0)
            {
                list.Add(1062481, m_BookName); // Book Name: ~1_val~
            }
        }

        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            base.GetContextMenuEntries(from, list);

            if (from.CheckAlive() && IsChildOf(from.Backpack))
            {
                list.Add(new OrganizeScrollsEntry(from, this));
                list.Add(new NameBookEntry(from, this));
            }

            SetSecureLevelEntry.AddTo(from, this, list);
        }

        private class OrganizeScrollsEntry : ContextMenuEntry
        {
            private Mobile m_From;
            private TomeOfPower m_Tome;

            public OrganizeScrollsEntry(Mobile from, TomeOfPower tome)
                : base(0097)
            {
                m_From = from;
                m_Tome = tome;
            }

            public override void OnClick()
            {
                if (m_From.CheckAlive() && m_Tome.IsChildOf(m_From.Backpack))
                {
                    m_Tome.OrganizeScrolls(m_From);
                }
            }
        }

        private class NameBookEntry : ContextMenuEntry
        {
            private Mobile m_From;
            private TomeOfPower m_Tome;

            public NameBookEntry(Mobile from, TomeOfPower tome)
                : base(6216)
            {
                m_From = from;
                m_Tome = tome;
            }

            public override void OnClick()
            {
                if (m_From.CheckAlive() && m_Tome.IsChildOf(m_From.Backpack))
                {
                    m_From.Prompt = new NameBookPrompt(m_Tome);
                    m_From.SendMessage("Type in the new name of the tome:");
                }
            }
        }

        private class NameBookPrompt : Prompt
        {
            private TomeOfPower m_Tome;

            public NameBookPrompt(TomeOfPower tome)
            {
                m_Tome = tome;
            }

            public override void OnResponse(Mobile from, string text)
            {
                if (text.Length > 40)
                    text = text.Substring(0, 40);

                if (from.CheckAlive() && m_Tome.IsChildOf(from.Backpack))
                {
                    m_Tome.BookName = Utility.FixHtml(text.Trim());
                    from.SendMessage("The tome's name has been changed.");
                }
            }

            public override void OnCancel(Mobile from)
            {
            }
        }
    }

    public class TomeOfPowerGump : Gump
    {
        private PlayerMobile m_From;
        private TomeOfPower m_Tome;
        private List<KeyValuePair<SkillName, int>> m_SortedEntries;

        public TomeOfPowerGump(PlayerMobile from, TomeOfPower tome)
            : base(50, 50)
        {
            m_From = from;
            m_Tome = tome;

            // Sort entries by skill name
            m_SortedEntries = new List<KeyValuePair<SkillName, int>>(tome.Entries);
            m_SortedEntries.Sort(delegate (KeyValuePair<SkillName, int> a, KeyValuePair<SkillName, int> b)
            {
                return a.Key.ToString().CompareTo(b.Key.ToString());
            });

            BuildGump();
        }

        private void BuildGump()
        {
            int entryCount = m_SortedEntries.Count;
            int height = Math.Max(300, 100 + (entryCount * 25) + 50);
            if (height > 600)
                height = 600;

            int entriesPerPage = (height - 150) / 25;
            int totalPages = (int)Math.Ceiling((double)entryCount / entriesPerPage);

            Closable = true;
            Disposable = true;
            Dragable = true;
            Resizable = false;

            AddPage(0);
            AddBackground(0, 0, 400, height, 9270);
            AddLabel(20, 20, 1152, "Tome of Power");
            AddLabel(20, 45, 0x480, String.Format("Total Scrolls: {0}", GetTotalScrollCount()));

            int yPos = 80;
            int page = 1;
            int currentPageEntries = 0;

            for (int i = 0; i < entryCount; i++)
            {
                if (currentPageEntries == 0)
                {
                    AddPage(page);

                    if (page > 1)
                    {
                        AddButton(20, height - 40, 4014, 4016, 0, GumpButtonType.Page, page - 1);
                        AddLabel(55, height - 40, 0x480, "Previous");
                    }

                    if (page < totalPages)
                    {
                        AddButton(250, height - 40, 4005, 4007, 0, GumpButtonType.Page, page + 1);
                        AddLabel(285, height - 40, 0x480, "Next");
                    }

                    yPos = 80;
                }

                KeyValuePair<SkillName, int> entry = m_SortedEntries[i];

                AddLabel(30, yPos, 0x480, entry.Key.ToString());
                AddLabel(250, yPos, 0x480, String.Format("Amount: {0}", entry.Value));
                AddButton(330, yPos, 4005, 4007, i + 1, GumpButtonType.Reply, 0);

                yPos += 25;
                currentPageEntries++;

                if (currentPageEntries >= entriesPerPage && i < entryCount - 1)
                {
                    currentPageEntries = 0;
                    page++;
                }
            }
        }

        private int GetTotalScrollCount()
        {
            int total = 0;
            foreach (KeyValuePair<SkillName, int> kvp in m_SortedEntries)
            {
                total += kvp.Value;
            }
            return total;
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            if (info.ButtonID == 0)
                return;

            int index = info.ButtonID - 1;

            if (index >= 0 && index < m_SortedEntries.Count)
            {
                KeyValuePair<SkillName, int> entry = m_SortedEntries[index];

                if (entry.Value == 1)
                {
                    // Only one scroll, withdraw it directly
                    m_Tome.WithdrawScrolls(m_From, entry.Key, 1);
                }
                else
                {
                    // Multiple scrolls, prompt for amount
                    m_From.SendGump(new TomeWithdrawGump(m_From, m_Tome, entry.Key, entry.Value));
                }
            }
        }
    }

    public class TomeWithdrawGump : Gump
    {
        private PlayerMobile m_From;
        private TomeOfPower m_Tome;
        private SkillName m_Skill;
        private int m_Available;

        public TomeWithdrawGump(PlayerMobile from, TomeOfPower tome, SkillName skill, int available)
            : base(150, 150)
        {
            m_From = from;
            m_Tome = tome;
            m_Skill = skill;
            m_Available = available;

            Closable = true;
            Disposable = true;
            Dragable = true;
            Resizable = false;

            AddPage(0);
            AddBackground(0, 0, 350, 200, 9270);
            AddLabel(20, 20, 1152, "Withdraw Scrolls");

            string message = String.Format("You have {0} {1} scroll{2}.<br>How many would you like to withdraw?",
                available, skill.ToString(), available != 1 ? "s" : "");

            AddHtml(20, 50, 310, 60, message, true, true);

            AddLabel(20, 120, 0x480, "Amount:");
            AddBackground(100, 120, 100, 25, 9350);
            AddTextEntry(105, 120, 90, 20, 0, 0, "");

            AddButton(60, 160, 247, 248, 1, GumpButtonType.Reply, 0); // Withdraw
            AddButton(200, 160, 241, 242, 0, GumpButtonType.Reply, 0); // Cancel
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            if (info.ButtonID == 1)
            {
                TextRelay entry = info.GetTextEntry(0);
                if (entry != null && entry.Text.Length > 0)
                {
                    int amount;
                    if (int.TryParse(entry.Text, out amount))
                    {
                        if (amount > 0 && amount <= m_Available)
                        {
                            m_Tome.WithdrawScrolls(m_From, m_Skill, amount);
                        }
                        else
                        {
                            m_From.SendMessage("Invalid amount. Please enter a number between 1 and {0}.", m_Available);
                            m_From.SendGump(new TomeOfPowerGump(m_From, m_Tome));
                        }
                    }
                    else
                    {
                        m_From.SendMessage("Invalid amount. Please enter a valid number.");
                        m_From.SendGump(new TomeOfPowerGump(m_From, m_Tome));
                    }
                }
                else
                {
                    m_From.SendGump(new TomeOfPowerGump(m_From, m_Tome));
                }
            }
            else
            {
                m_From.SendGump(new TomeOfPowerGump(m_From, m_Tome));
            }
        }
    }
}
