using System;
using System.Collections;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    public class Oliver : BaseCreature
    {
        private Hashtable m_LastPotionHandin;
        
        public override string TalkGumpTitle { get { return "Oliver the doc"; } }
        public override string TalkGumpSubject { get { return "Oliver"; } }
        
        [Constructable]
        public Oliver() : base(AIType.AI_Thief, FightMode.None, 1, 1, 0.2, 0.4)
        {
            m_LastPotionHandin = new Hashtable();
            Title = "the Doctor";
            NameHue = 0x92E;
            Name = "Oliver";
            InitStats(50, 50, 25);
            Hue = Utility.RandomSkinHue();
            
            if (Female = Utility.RandomBool())
            {
                Body = 401;
                AddItem(new Server.Items.LeatherBoots());
                AddItem(new FancyDress(GetRandomHue()));
            }
            else
            {
                Body = 400;
                FacialHairItemID = Utility.RandomList(0, 0, 8254, 8255, 8256, 8257, 8267, 8268, 8269);
                AddItem(new Server.Items.Shoes());
                AddItem(new Server.Items.LongPants());
                AddItem(new Server.Items.PirateCoat(GetRandomHue()));
            }
            
            Utility.AssignRandomHair(this);
            HairHue = Utility.RandomHairHue();
            FacialHairHue = HairHue;
        }
        
        public override bool OnDragDrop(Mobile from, Item dropped)
        {
            if (from == null || dropped == null)
                return false;
            
            if (dropped is GenderPotion)
            {
                return HandleGenderPotionDrop(from, dropped);
            }
            
            return base.OnDragDrop(from, dropped);
        }
        
        private bool HandleGenderPotionDrop(Mobile from, Item potion)
        {
            if (m_LastPotionHandin[from] != null)
            {
                DateTime lastHandin = (DateTime)m_LastPotionHandin[from];
                DateTime nextAllowed = lastHandin + TimeSpan.FromHours(1.0);
                
                if (DateTime.UtcNow < nextAllowed)
                {
                    TimeSpan remaining = nextAllowed - DateTime.UtcNow;
                    PublicOverheadMessage(Network.MessageType.Regular, 0x3B2, false, 
                        "I'm sorry, but I'm still waiting for the pickup of the last one and my shelves are full! Can you hold on to it for a bit longer?");
                    from.SendMessage(string.Format("You must wait {0} before Oliver can accept another potion.", FormatTimeSpan(remaining)));
                    return false;
                }
            }
            
            potion.Delete();
            int playerLevel = Server.Misc.GetPlayerInfo.GetPlayerLevel(from);
            int transPower = Utility.RandomMinMax(playerLevel * 2, playerLevel * 5);
            Item reward = Loot.RandomRobe(false);
            reward.Hue = GetRandomHue();
            reward = Server.LootPackEntry.Enchant(from, transPower, reward);
            from.AddToBackpack(reward);
            int karmaAmount = Utility.RandomMinMax(playerLevel, playerLevel * 2);
            from.Karma += karmaAmount;
            PublicOverheadMessage(Network.MessageType.Regular, 0x3B2, false, 
                "Thank you so much! I know someone that will really appreciate this! Here, have this for your trouble, I think it suits you!");
            
            m_LastPotionHandin[from] = DateTime.UtcNow;
            return true;
        }

        public virtual int GetRandomHue()
		{
			switch ( Utility.Random( 7 ) )
			{
				default:
				case 0: return Utility.RandomBlueHue();
				case 1: return Utility.RandomGreenHue();
				case 2: return Utility.RandomRedHue();
				case 3: return Utility.RandomYellowHue();
				case 4: return Utility.RandomNeutralHue();
                case 5: return Utility.RandomPinkHue();
                case 6: return Utility.RandomRedHue();
			}
		}
        
        private string FormatTimeSpan(TimeSpan ts)
        {
            if (ts.TotalHours >= 1)
                return string.Format("{0:0} hour{1} and {2:0} minute{3}",
                    ts.Hours, ts.Hours != 1 ? "s" : "",
                    ts.Minutes, ts.Minutes != 1 ? "s" : "");
            else if (ts.TotalMinutes >= 1)
                return string.Format("{0:0} minute{1}", ts.Minutes, ts.Minutes != 1 ? "s" : "");
            else
                return string.Format("{0:0} second{1}", ts.Seconds, ts.Seconds != 1 ? "s" : "");
        }
        
        public Oliver(Serial serial) : base(serial)
        {
        }
        
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
            
            writer.Write(m_LastPotionHandin.Count);
            foreach (DictionaryEntry e in m_LastPotionHandin)
            {
                writer.Write((Mobile)e.Key);
                writer.Write((DateTime)e.Value);
            }
        }
        
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            
            m_LastPotionHandin = new Hashtable();
            int count = reader.ReadInt();
            for (int i = 0; i < count; i++)
            {
                Mobile m = reader.ReadMobile();
                DateTime t = reader.ReadDateTime();
                if (m != null)
                    m_LastPotionHandin[m] = t;
            }
        }
    }
}