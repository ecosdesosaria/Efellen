using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Gumps;
using Server.Items;
using Server.Network;
using Server.Targeting;
using Server.ContextMenus;
using Server.Misc;
using Server.Mobiles;

namespace Server.Mobiles
{
    public class DruidGuildmaster : BaseGuildmaster
    {
        public override NpcGuild NpcGuild { get { return NpcGuild.DruidsGuild; } }

        public override string TalkGumpTitle { get { return "The Protectors Of The Forest"; } }
        public override string TalkGumpSubject { get { return "Druid"; } }

        [Constructable]
        public DruidGuildmaster() : base("druid")
        {
            SetSkill(SkillName.Herding, 80.0, 100.0);
            SetSkill(SkillName.Camping, 80.0, 100.0);
            SetSkill(SkillName.Cooking, 80.0, 100.0);
            SetSkill(SkillName.Alchemy, 80.0, 100.0);
            SetSkill(SkillName.Druidism, 85.0, 100.0);
            SetSkill(SkillName.Taming, 90.0, 100.0);
            SetSkill(SkillName.Veterinary, 90.0, 100.0);

            AddItem(new LightSource());
        }

        public override void InitSBInfo(Mobile m)
        {
            m_Merchant = m;
            SBInfos.Add(new MyStock());
        }

        public class MyStock : SBInfo
        {
            private List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
            private IShopSellInfo m_SellInfo = new InternalSellInfo();

            public MyStock()
            {
            }

            public override IShopSellInfo SellInfo { get { return m_SellInfo; } }
            public override List<GenericBuyInfo> BuyInfo { get { return m_BuyInfo; } }

            public class InternalBuyInfo : List<GenericBuyInfo>
            {
                public InternalBuyInfo()
                {
                    ItemInformation.GetSellList(m_Merchant, this, ItemSalesInfo.Category.None, ItemSalesInfo.Material.All, ItemSalesInfo.Market.Druid, ItemSalesInfo.World.None, null);
                    ItemInformation.GetSellList(m_Merchant, this, ItemSalesInfo.Category.Book, ItemSalesInfo.Material.All, ItemSalesInfo.Market.Druid, ItemSalesInfo.World.None, null);
                    ItemInformation.GetSellList(m_Merchant, this, ItemSalesInfo.Category.Potion, ItemSalesInfo.Material.All, ItemSalesInfo.Market.Druid, ItemSalesInfo.World.None, null);
                    ItemInformation.GetSellList(m_Merchant, this, ItemSalesInfo.Category.Resource, ItemSalesInfo.Material.All, ItemSalesInfo.Market.Druid, ItemSalesInfo.World.None, null);
                    ItemInformation.GetSellList(m_Merchant, this, ItemSalesInfo.Category.Reagent, ItemSalesInfo.Material.All, ItemSalesInfo.Market.Druid, ItemSalesInfo.World.None, null);
                }
            }

            public class InternalSellInfo : GenericSellInfo
            {
                public InternalSellInfo()
                {
                    ItemInformation.GetBuysList(m_Merchant, this, ItemSalesInfo.Category.None, ItemSalesInfo.Material.All, ItemSalesInfo.Market.Druid, ItemSalesInfo.World.None, null);
                    ItemInformation.GetBuysList(m_Merchant, this, ItemSalesInfo.Category.Book, ItemSalesInfo.Material.All, ItemSalesInfo.Market.Druid, ItemSalesInfo.World.None, null);
                    ItemInformation.GetBuysList(m_Merchant, this, ItemSalesInfo.Category.Potion, ItemSalesInfo.Material.All, ItemSalesInfo.Market.Druid, ItemSalesInfo.World.None, null);
                    ItemInformation.GetBuysList(m_Merchant, this, ItemSalesInfo.Category.Resource, ItemSalesInfo.Material.All, ItemSalesInfo.Market.Druid, ItemSalesInfo.World.None, null);
                    ItemInformation.GetBuysList(m_Merchant, this, ItemSalesInfo.Category.Reagent, ItemSalesInfo.Material.All, ItemSalesInfo.Market.Druid, ItemSalesInfo.World.None, null);
                }
            }
        }

        public override void InitOutfit()
        {
            base.InitOutfit();

            AddItem(new DeerCap());
            switch (Utility.RandomMinMax(1, 5))
            {
                case 1: AddItem(new Server.Items.GnarledStaff()); break;
                case 2: AddItem(new Server.Items.BlackStaff()); break;
                case 3: AddItem(new Server.Items.WildStaff()); break;
                case 4: AddItem(new Server.Items.QuarterStaff()); break;
                case 5: AddItem(new Server.Items.ShepherdsCrook()); break;
            }
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private class FixEntry : ContextMenuEntry
        {
            private DruidGuildmaster m_Druid;
            private Mobile m_From;

            public FixEntry(DruidGuildmaster DruidGuildmaster, Mobile from) : base(6120, 12)
            {
                m_Druid = DruidGuildmaster;
                m_From = from;
                Enabled = m_Druid.CheckVendorAccess(from);
            }

            public override void OnClick()
            {
                m_Druid.BeginServices(m_From);
            }
        }

        private class RewardsEntry : ContextMenuEntry
        {
            private DruidGuildmaster m_DruidGuildmaster;
            private Mobile m_From;

            public RewardsEntry(DruidGuildmaster DruidGuildmaster, Mobile from) : base(6093, 3)
            {
                m_DruidGuildmaster = DruidGuildmaster;
                m_From = from;
                Enabled = m_DruidGuildmaster.CheckVendorAccess(from);
            }

            public override void OnClick()
            {
                m_DruidGuildmaster.MaybeShowDruidicRewardsGump(m_From as PlayerMobile);
            }
        }

        public override void AddCustomContextEntries(Mobile from, List<ContextMenuEntry> list)
        {
            if (CheckChattingAccess(from))
            {
                list.Add(new FixEntry(this, from));
                list.Add(new RewardsEntry(this, from));
            }

            base.AddCustomContextEntries(from, list);
        }

        public void BeginServices(Mobile from)
        {
            if (Deleted || !from.Alive)
                return;

            int nCost = 1000;

            if (BeggingPose(from) > 0) // LET US SEE IF THEY ARE BEGGING
            {
                nCost = nCost - (int)((from.Skills[SkillName.Begging].Value * 0.005) * nCost); if (nCost < 1) { nCost = 1; }
                SayTo(from, "Já que você está implorando, você ainda quer que eu cuide do seu animal de carga por até 5 jornadas, custará apenas " + nCost.ToString() + " de ouro?");
            }
            else { SayTo(from, "Se você quiser que eu cuide do seu animal de carga por até 5 jornadas, custará " + nCost.ToString() + " de ouro."); }

            from.Target = new RepairTarget(this);
        }

        private class RepairTarget : Target
        {
            private DruidGuildmaster m_Druid;

            public RepairTarget(DruidGuildmaster druid) : base(12, false, TargetFlags.None)
            {
                m_Druid = druid;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is PackBeastItem && from.Backpack != null)
                {
                    PackBeastItem ball = targeted as PackBeastItem;
                    Container pack = from.Backpack;

                    int toConsume = 0;

                    if (ball.Charges < 50)
                    {
                        toConsume = 1000;

                        if (BeggingPose(from) > 0) // LET US SEE IF THEY ARE BEGGING
                        {
                            toConsume = toConsume - (int)((from.Skills[SkillName.Begging].Value * 0.005) * toConsume);
                        }
                    }
                    else
                    {
                        m_Druid.SayTo(from, "Seu animal de carga já foi cuidado o suficiente.");
                    }

                    if (toConsume == 0)
                        return;

                    if (pack.ConsumeTotal(typeof(Gold), toConsume))
                    {
                        if (BeggingPose(from) > 0) { Titles.AwardKarma(from, -BeggingKarma(from), true); } // DO ANY KARMA LOSS
                        m_Druid.SayTo(from, "Seu animal de carga está devidamente cuidado.");
			from.SendMessage(String.Format("Você paga {0} de ouro.", toConsume));
                        Effects.PlaySound(from.Location, from.Map, 0x5C1);
                        ball.Charges = ball.Charges + 5;
                    }
                    else
                    {
			m_Druid.SayTo(from, "Custaria {0} de ouro para ter esse animal de carga cuidado.", toConsume);
			from.SendMessage("Você não tem ouro suficiente.");
                    }
                }
                else
                {
			m_Druid.SayTo(from, "Isso não precisa dos meus serviços.");
                }
            }
        }

        public override bool OnDragDrop(Mobile from, Item dropped)
        {
            if (dropped is MysticalTreeSap)
            {
                int TreeSap = dropped.Amount;
                string sMessage = "";

                if (TreeSap > 19)
                {
			sMessage = "Ahhh... isso é generoso de sua parte. Aqui... tome isto como um símbolo da gratidão da guilda.";
                    PackBeastItem ball = new PackBeastItem();
                    ball.PorterOwner = from.Serial;
                    from.AddToBackpack(ball);
                }
                else
                {
			sMessage = "Obrigado por estes. A seiva de árvore mística é algo que frequentemente procuramos.";
                }

                this.PrivateOverheadMessage(MessageType.Regular, 1153, false, sMessage, from.NetState);
                dropped.Delete();
            }
            else if (dropped is PackBeastItem)
            {
                string sMessage = "";

                PackBeastItem ball = (PackBeastItem)dropped;

                if (ball.PorterType == 291) { ball.ItemID = 0x2127; ball.PorterType = 292; ball.Hue = 0; sMessage = "You may like a pack llama instead."; }
                else if (ball.PorterType == 292) { ball.ItemID = 0x20DB; ball.PorterType = 23; ball.Hue = 0; sMessage = "You may like a pack brown bear instead."; }
                else if (ball.PorterType == 23) { ball.ItemID = 0x20CF; ball.PorterType = 177; ball.Hue = 0; sMessage = "You may like a pack black bear instead."; }
                else if (ball.PorterType == 177) { ball.ItemID = 0x20E1; ball.PorterType = 179; ball.Hue = 0; sMessage = "You may like a pack polar bear instead."; }
                else if (ball.PorterType == 179) { ball.ItemID = 0x2126; ball.PorterType = 291; ball.Hue = 0; sMessage = "You may like a pack horse instead."; }

                sMessage = "Você talvez gostaria de um animal de carga diferente? " + sMessage;
                from.AddToBackpack(ball);

                this.PrivateOverheadMessage(MessageType.Regular, 1153, false, sMessage, from.NetState);
            }
            return base.OnDragDrop(from, dropped);
        }

        public virtual bool CheckResurrect(Mobile m)
        {
            return true;
        }

        private DateTime m_NextResurrect;
        private static TimeSpan ResurrectDelay = TimeSpan.FromSeconds(2.0);
        public override bool HandlesOnSpeech(Mobile from)
        {
            if (from is PlayerMobile && from.InRange(this, 4))
                return true;

            return base.HandlesOnSpeech(from);
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            Mobile from = e.Mobile;

            if (from == null || !(from is PlayerMobile))
            {
                base.OnSpeech(e);
                return;
            }


            if (!from.InRange(this, 4))
            {
                base.OnSpeech(e);
                return;
            }

            if (e.Speech.ToLower().IndexOf("reward") >= 0)
            {
                MaybeShowDruidicRewardsGump(from as PlayerMobile);
                e.Handled = true;
                return;
            }

            base.OnSpeech(e);
        }

        public void MaybeShowDruidicRewardsGump(PlayerMobile from)
        {
            PlayerMobile druid = from as PlayerMobile;

            if (druid.NpcGuild == NpcGuild.DruidsGuild)
            {
                from.SendGump(new Server.Custom.DefenderOfTheRealm.RewardGump(druid, 4, 0));
                Say("These are the gifts I can bestow thee, " + (from.Female ? "sister." : "brother."));
            }
            else
            {
                Say("I only trust those that are in good standing with our order, friend.");
            }
        }

        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            if (!m.Frozen && DateTime.Now >= m_NextResurrect && InRange(m, 4) && !InRange(oldLocation, 4) && InLOS(m))
            {
                if (m.IsDeadBondedPet)
                {
                    m_NextResurrect = DateTime.Now + ResurrectDelay;

                    if (m.Map == null || !m.Map.CanFit(m.Location, 16, false, false))
                    {
                        Say("I sense a spirt of an animal...somewhere.");
                    }
                    else
                    {
                        BaseCreature bc = m as BaseCreature;

                        bc.PlaySound(0x214);
                        bc.FixedEffect(0x376A, 10, 16);

                        bc.ResurrectPet();

                        Say("Levante-se, meu amigo. Gostaria de poder salvar todos os animais infelizes.");
                    }
                }
            }
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public DruidGuildmaster(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
        }
    }
}
