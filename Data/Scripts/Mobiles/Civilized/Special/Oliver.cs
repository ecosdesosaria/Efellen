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
            
            // Store random hue
            int randomHue = GetRandomHue();
            
            // Create bag and apply hue
            Bag rewardBag = new Bag();
            rewardBag.Hue = randomHue;
            
            // Create robe, apply hue and enchantment, add to bag
            Item reward = Loot.RandomRobe(false);
            reward.Hue = randomHue;
            reward = Server.LootPackEntry.Enchant(from, transPower, reward);
            rewardBag.DropItem(reward);
            
            AddPotionsToBag(rewardBag, transPower);
            
            // Give bag to player
            from.AddToBackpack(rewardBag);
            
            int karmaAmount = Utility.RandomMinMax(playerLevel, playerLevel * 2);
            from.Karma += karmaAmount;
            PublicOverheadMessage(Network.MessageType.Regular, 0x3B2, false, 
                "Thank you so much! I know someone that will really appreciate this! Here, have this for your trouble, I think it suits you!");
            
            m_LastPotionHandin[from] = DateTime.UtcNow;
            return true;
        }
        
        private void AddPotionsToBag(Bag bag, int transPower)
        {
            if (transPower >= 1 && transPower <= 100)
            {
                bag.DropItem(new RepairPotion());
                
                AddPotions(bag, typeof(ManaPotion), Utility.RandomMinMax(1, 2));
                
                AddPotions(bag, typeof(RefreshPotion), Utility.RandomMinMax(1, 2));
                
                if (Utility.RandomBool())
                    AddPotions(bag, typeof(GreaterCurePotion), Utility.RandomMinMax(1, 2));
                else
                    AddPotions(bag, typeof(GreaterHealPotion), Utility.RandomMinMax(1, 2));
            }
            else if (transPower >= 101 && transPower <= 200)
            {
                switch (Utility.Random(4))
                {
                    case 0: AddPotions(bag, typeof(PotionOfMight), Utility.RandomMinMax(1, 3)); break;
                    case 1: AddPotions(bag, typeof(PotionOfDexterity), Utility.RandomMinMax(1, 3)); break;
                    case 2: AddPotions(bag, typeof(PotionOfWisdom), Utility.RandomMinMax(1, 3)); break;
                    case 3: AddPotions(bag, typeof(SuperPotion), Utility.RandomMinMax(1, 3)); break;
                }
                
                AddPotions(bag, typeof(ManaPotion), Utility.RandomMinMax(2, 3));
                
                AddPotions(bag, typeof(RefreshPotion), Utility.RandomMinMax(2, 3));
                
                AddPotions(bag, typeof(RepairPotion), 2);
                
                if (Utility.RandomBool())
                    AddPotions(bag, typeof(GreaterCurePotion), 3);
                else
                    AddPotions(bag, typeof(GreaterHealPotion), 3);
            }
            else if (transPower >= 201 && transPower <= 300)
            {
                switch (Utility.Random(4))
                {
                    case 0: AddPotions(bag, typeof(PotionOfMight), Utility.RandomMinMax(2, 4)); break;
                    case 1: AddPotions(bag, typeof(PotionOfDexterity), Utility.RandomMinMax(2, 4)); break;
                    case 2: AddPotions(bag, typeof(PotionOfWisdom), Utility.RandomMinMax(2, 4)); break;
                    case 3: AddPotions(bag, typeof(SuperPotion), Utility.RandomMinMax(1, 5)); break;
                }
                
                AddPotions(bag, typeof(ManaPotion), Utility.RandomMinMax(3, 4));
                
                AddPotions(bag, typeof(RefreshPotion), Utility.RandomMinMax(3, 4));
                
                AddPotions(bag, typeof(RepairPotion), 3);
                
                if (Utility.RandomBool())
                    AddPotions(bag, typeof(GreaterCurePotion), 4);
                else
                    AddPotions(bag, typeof(GreaterHealPotion), 4);
            }
            else if (transPower >= 301 && transPower <= 450)
            {
                switch (Utility.Random(4))
                {
                    case 0: AddPotions(bag, typeof(PotionOfMight), Utility.RandomMinMax(2, 4)); break;
                    case 1: AddPotions(bag, typeof(PotionOfDexterity), Utility.RandomMinMax(2, 4)); break;
                    case 2: AddPotions(bag, typeof(PotionOfWisdom), Utility.RandomMinMax(2, 4)); break;
                    case 3: AddPotions(bag, typeof(SuperPotion), Utility.RandomMinMax(2, 5)); break;
                }
                
                AddPotions(bag, typeof(RepairPotion), 4);
                
                bag.DropItem(new InvulnerabilityPotion());
                
                AddPotions(bag, typeof(TotalRefreshPotion), Utility.RandomMinMax(3, 4));
                
                AddPotions(bag, typeof(GreaterManaPotion), Utility.RandomMinMax(3, 4));
                
                if (Utility.RandomBool())
                    AddPotions(bag, typeof(GreaterCurePotion), 5);
                else
                    AddPotions(bag, typeof(GreaterHealPotion), 5);
            }
            else if (transPower >= 451)
            {
                switch (Utility.Random(3))
                {
                    case 0: AddPotions(bag, typeof(PotionOfMight), Utility.RandomMinMax(3, 5)); break;
                    case 1: AddPotions(bag, typeof(PotionOfDexterity), Utility.RandomMinMax(3, 5)); break;
                    case 2: AddPotions(bag, typeof(PotionOfWisdom), Utility.RandomMinMax(3, 5)); break;
                }
                
                AddPotions(bag, typeof(SuperPotion), Utility.RandomMinMax(5, 10));
                
                AddPotions(bag, typeof(RepairPotion), 5);
                
                AddPotions(bag, typeof(InvulnerabilityPotion), 2);
                
                AddPotions(bag, typeof(GreaterManaPotion), Utility.RandomMinMax(4, 5));
                
                AddPotions(bag, typeof(TotalRefreshPotion), Utility.RandomMinMax(4, 5));
            }
        }
        
        private void AddPotions(Bag bag, Type potionType, int count)
        {
            Item potion = (Item)Activator.CreateInstance(potionType);
            potion.Amount = count;
            bag.DropItem(potion);
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