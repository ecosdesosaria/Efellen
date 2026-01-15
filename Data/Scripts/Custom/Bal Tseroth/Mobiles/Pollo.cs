using System;
using Server.ContextMenus;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Misc;
using Server.Gumps; 
using Server.Network; 
using Server.Targeting;

namespace Server.Custom.BalTsareth
{
    public class Pollo : BaseCreature
    {
        public override string TalkGumpTitle{ get{ return "The Ruins of Bal Tsareth"; } }
		public override string TalkGumpSubject{ get{ return "Pollo"; } }
        private DateTime m_NextSpeechTime;
        [Constructable]
        public Pollo() : base(AIType.AI_Thief, FightMode.None, 10, 1, 0.4, 1.6)
        {
            InitStats( 125, 55, 65 ); 
			Name = "Pollo";
			Title = "The Expedition Leader";
            HairHue = Utility.RandomHairHue(); 
			Body = 0x190;
            SpeechHue = Utility.RandomTalkHue();
			Hue = Utility.RandomSkinHue(); 
			Utility.AssignRandomHair( this );
			AddItem( new Boots( Utility.RandomBirdHue() ) );
            AddItem( new Cloak( Utility.RandomBirdHue() ) );
            AddItem( new ScholarRobe(Utility.RandomBirdHue()));
        }

        public override void OnMovement( Mobile m, Point3D oldLocation )
        {
            if ( InRange( m, 6 ) && !InRange( oldLocation, 2 ) )
            {
                if ( m is PlayerMobile && !m.Hidden ) 
                {
                    if ( DateTime.UtcNow >= m_NextSpeechTime )
                    {
                        switch (Utility.Random(11))
                        {
                            case 0: Say("Kamina shall pay dearly for this treachery!"); break;
                            case 1: Say("These vile brigands have undone all my labors!"); break;
                            case 2: Say("Why doth the King tarry, when an army is sorely needed?"); break;
                            case 3: Say("Years of study, laid waste by a brigand’s whim!"); break;
                            case 4: Say("Step forth, brave adventurer! Thy aid is most required!"); break;
                            case 5: Say("Rich reward awaiteth any who dare brave these cursed depths!"); break;
                            case 6: Say("Madness and folly do reign in these halls!"); break;
                            case 7: Say("The restless shades of the fallen shall ever haunt this place!"); break;
                            case 8: Say("Beware thee! Dire perils lurk beyond these stones!"); break;
                            case 9: Say("Truly, I should have chosen the life of a humble clerk!"); break;
                            case 10: Say("So much life is lost… and for what grim purpose?"); break;
                        }
                        m_NextSpeechTime = DateTime.UtcNow + TimeSpan.FromSeconds(30);
                    }
                }
            }
        }

        public override bool OnDragDrop(Mobile from, Item dropped)
        {
            EerieIdol idol = dropped as EerieIdol;

            if (idol == null)
                return base.OnDragDrop(from, dropped);

            int used = ConsumeIdols(idol, from);

            if (used <= 0)
                return false;

            if (used >= 100)
                Say("Hold, hold! Thou bringest far too many! I must reckon them in proper measure, lest my wits be utterly undone!");

            Bag bag = new Bag();
            bag.Hue = 0x0213;
            bag.Name = "Expedition Supplies";

            from.AddToBackpack(bag);

            bag.DropItem(new Gold(used * 30));

            int enchant = used * 4 < 50 ? 50 : used * 4;
            if (enchant > 400)
                enchant = 400;

            GenerateEnchantedItem(from, enchant, bag);

            if (used < 5)
            {
                bag.DropItem(Loot.RandomScroll(1));
                bag.DropItem(Loot.RandomPotion(4, false));
            }
            else if (used < 10)
            {
                bag.DropItem(Loot.RandomGem());
                bag.DropItem(Loot.RandomPotion(4, false));
                bag.DropItem(Loot.RandomScroll(4));
            }
            else if (used < 20)
            {
                bag.DropItem(Loot.RandomScroll(3));
                bag.DropItem(Loot.RandomGem());
                bag.DropItem(Loot.RandomPotion(4, false));
            }
            else if (used < 30)
            {
                bag.DropItem(Loot.RandomScroll(4));
                bag.DropItem(Loot.RandomGem());
            }
            else if (used < 40)
            {
                bag.DropItem(Loot.RandomScroll(5));
                bag.DropItem(Loot.RandomScroll(4));
                bag.DropItem(Loot.RandomPotion(8, false));
            }
            else if (used < 50)
            {
                bag.DropItem(Loot.RandomScroll(6));
                bag.DropItem(Loot.RandomScroll(5));
                bag.DropItem(Loot.RandomScroll(4));
                bag.DropItem(Loot.RandomPotion(8, false));
                bag.DropItem(Loot.RandomPotion(8, false));
            }
            else if (used < 75)
            {
                bag.DropItem(Loot.RandomScroll(8));
                bag.DropItem(Loot.RandomScroll(6));
                bag.DropItem(Loot.RandomScroll(6));
                bag.DropItem(Loot.RandomPotion(12, false));
                bag.DropItem(Loot.RandomPotion(12, false));
            }
            else
            {
                bag.DropItem(Loot.RandomScroll(8));
                bag.DropItem(Loot.RandomScroll(8));
                bag.DropItem(Loot.RandomScroll(6));
                bag.DropItem(Loot.RandomPotion(12, false));
                bag.DropItem(Loot.RandomPotion(12, false));
            }

            new ThankTimer(from).Start();

            return true;
        }


        
        public static void GenerateEnchantedItem(Mobile from, int enchantLevel, Container rewardBag)
        {
            BodySash sash = new BodySash();

            Item enchanted = LootPackEntry.Enchant(from, enchantLevel, sash);

            if (enchanted != null)
            {
                enchanted.Hue = 0x0213;
                enchanted.Name = "Bal Tsareth's Sash";
                rewardBag.DropItem(enchanted);
            }
        }


        private static int ConsumeIdols(EerieIdol idol, Mobile from)
        {
            if (idol == null || idol.Deleted || from == null)
                return 0;

            int total = idol.Amount;
            int used = total > 100 ? 100 : total;
            int remaining = total - used;

            idol.Delete();

            if (remaining > 0)
            {
                EerieIdol remainder = new EerieIdol(remaining);
                from.AddToBackpack(remainder);
            }
            return used;
        }



        private class ThankTimer : Timer
        {
            private Mobile m_From;

            public ThankTimer(Mobile from) : base(TimeSpan.FromSeconds(1.5))
            {
                m_From = from;
            }

            protected override void OnTick()
            {
                if (m_From != null && !m_From.Deleted)
                    m_From.SendMessage(0x59, "Thank thee kindly for thy noble aid, brave soul.");

                Stop();
            }
        }




        public override bool HandlesOnSpeech( Mobile from ) 
		{ 
			return true; 
		} 

        public Pollo(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}