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
                            case 0: Say("Kamina pagará caro por esta traição!"); break;
                            case 1: Say("Estes vis bandidos desfizeram todo o meu trabalho!"); break;
                            case 2: Say("Por que o Rei demora, quando um exército é tão necessário?"); break;
                            case 3: Say("Anos de estudo, destruídos por um capricho de bandido!"); break;
                            case 4: Say("Apresente-se, bravo aventureiro! Tua ajuda é mais que necessária!"); break;
                            case 5: Say("Rica recompensa aguarda quem ousar enfrentar estas profundezas amaldiçoadas!"); break;
                            case 6: Say("Loucura e tolice reinam nestes salões!"); break;
                            case 7: Say("As sombras inquietas dos caídos assombrarão este lugar para sempre!"); break;
                            case 8: Say("Cuidado! Perigos terríveis espreitam além destas pedras!"); break;
                            case 9: Say("Verdadeiramente, deveria ter escolhido a vida de um humilde escriba!"); break;
                            case 10: Say("Tanta vida é perdida… e para que propósito sombrio?"); break;
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
                Say("Pare, pare! Trazes muitos! Devo contá-los na medida certa, para que meu juízo não se perca por completo!");

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
                    m_From.SendMessage(0x59, "Agradeço-te gentilmente por tua nobre ajuda, brava alma.");

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