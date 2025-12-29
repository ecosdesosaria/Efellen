using System;
using System.Collections;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    public class Patient : BaseCreature
    {
        private Hashtable m_LastHealAttempt;

        [Constructable]
        public Patient() : base(AIType.AI_Thief, FightMode.None, 1, 1, 0.2, 0.4)
        {
            m_LastHealAttempt = new Hashtable();
            Title = "the patient";
			NameHue = 0x92E;

            InitStats(50, 50, 25);

            Hue = Utility.RandomSkinHue();


            if ( Female = Utility.RandomBool() ) 
			{ 
				Body = 401; 
				Name = NameList.RandomName( "female" );
                AddItem( new Server.Items.Shoes() );
                AddItem(new PlainDress(Utility.RandomNeutralHue()));
            }
			else 
			{ 
				Body = 400; 			
				Name = NameList.RandomName( "male" ); 
				FacialHairItemID = Utility.RandomList( 0, 0, 8254, 8255, 8256, 8257, 8267, 8268, 8269 );
                AddItem( new Server.Items.Shoes() );
			    AddItem( new Server.Items.Robe(Utility.RandomNeutralHue()) );
			}
			Utility.AssignRandomHair( this );
			HairHue = Utility.RandomHairHue();
			FacialHairHue = HairHue;
        }

        public static readonly string[] SuccessLines = new string[]
        {
            "Oh! That actually helped. Thank you!",
            "Bless you! I can feel the pain fading.",
            "You have a gentle touch, friend.",
            "Ahh… that feels much better!",
            "You’re pretty good at this!",
            "I think I'll see my grandchildren age after all...",
            "The voices are quieter now, thank you!",
            "The nuns said someone like you would come if I prayed!",
            "Thank you, healer! I owe you much.",
            "Ooh… warm hands. Nice!",
            "That's it, that's the spot...",
            "The air no longer smells of copper!",
            "You mean those were supposed to be inside all this time?",
            "I'll name my firstborn after you.",
            "I told them I wasn't being dramatic."
        };

        public static readonly string[] FailLines = new string[]
        {
            "OUCH!!",
            "Merciful heavens, STOP! That hurts!",
            "NUNS!! This one is trying to finish me off!",
            "That gave me a new perspective on the woes of life.",
            "I think that my kidney was supposed to stay inside",
            "Hey!! Are you sure you're trained for this?",
            "No no no—bandages go *around* wounds, not *in* them!",
            "*screams in pain*",
            "OW! I said HEALING, not SEARING!",
            "Was I supposed to be tasting all of these colors?",
            "AUGH!! My spleen! I think that was my spleen!",
            "If you want to help, maybe stop stabbing me!",
            "Nurse! This one needs supervision!",
            "I guess I get what I paid for",
            "I deserve this punishment for my many sins"
        };

        public bool HandleBandage(Mobile from, Bandage band)
        {
            if (from == null || band == null || band.Deleted)
            return true;

            if (from.Karma < 0)
            {
                from.SendMessage("The patient is scared of you. They refuse your help.");
                return true;
            }

            if (from.Skills[SkillName.Healing].Value < 30.0)
            {
                from.SendMessage("Your healing skill is too low to treat this patient.");
                return true;
            }

            DateTime last;
            if (m_LastHealAttempt[from] != null)
                last = (DateTime)m_LastHealAttempt[from];
            else
                last = DateTime.MinValue;

            if ((DateTime.UtcNow - last) < TimeSpan.FromHours(1))
            {
                from.SendMessage("You have already treated this patient recently.");
                return true;
            }

            m_LastHealAttempt[from] = DateTime.UtcNow;

            band.Consume();

            if ( from.CheckSkill( SkillName.Healing, 30, 125.0 ) )
            {
                int amount = Utility.Random(5,25);
                from.Karma += amount;
                Say(SuccessLines[Utility.Random(SuccessLines.Length)]);
                from.SendMessage("Your healing attempt succeeds. The patient feels better.");
                from.AddToBackpack( new MarksOfDevotion( amount ) );
				from.SendMessage( "You aqquired" + " " + amount + " " + "Marks of Devotion!" );
            }
            else
            {
                Say(FailLines[Utility.Random(FailLines.Length)]);
                SpillBlood();
                from.SendMessage("Your healing attempt fails.");
            }
            return true; 
        }

        private void SpillBlood()
        {
            Blood blood = new Blood();
            blood.MoveToWorld(this.Location, this.Map);
        }

        public Patient(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);

            writer.Write(m_LastHealAttempt.Count);
            foreach (DictionaryEntry e in m_LastHealAttempt)
            {
                writer.Write((Mobile)e.Key);
                writer.Write((DateTime)e.Value);
            }
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            m_LastHealAttempt = new Hashtable();
            int count = reader.ReadInt();

            for (int i = 0; i < count; i++)
            {
                Mobile m = reader.ReadMobile();
                DateTime t = reader.ReadDateTime();

                if (m != null)
                    m_LastHealAttempt[m] = t;
            }
        }
    }
}
