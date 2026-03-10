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
            "Oh! Isso realmente ajudou. Obrigado!",
            "Deus te abençoe! Consigo a dor sumindo.",
            "Você tem um toque suave, amigo.",
            "Ahh… isso é muito melhor!",
            "Você é muito bom nisso!",
            "Acho que vou ver meus netos crescerem afinal...",
            "As vozes estão mais calmas agora, obrigado!",
            "As freiras disseram que alguém como você viria se eu rezasse!",
            "Obrigado, curandeiro! Estou em dívida.",
            "Ooh… mãos quentes. Que bom!",
            "É aí, é aí mesmo...",
            "O ar não cheira mais a cobre!",
            "Quer dizer que aquilo deveria estar dentro todo esse tempo?",
            "Vou dar o seu nome ao meu primogênito.",
            "Eu disse que não estava sendo dramático."
        };

        public static readonly string[] FailLines = new string[]
        {
            "AU!!",
            "Céus misericordiosos, PARE! Isso dói!",
            "FREIRAS!! Este aqui está tentando me matar!",
            "Isso me deu uma nova perspectiva sobre os males da vida.",
            "Acho que meu rim deveria ficar dentro do corpo",
            "Ei!! Tem certeza de que é treinado para isso?",
            "Não não não—ataduras vão *em volta* dos ferimentos, não *dentro* deles!",
            "*grita de dor*",
            "AI! Eu disse CURAR, não QUEIMAR!",
            "Era pra eu estar sentindo o gosto de todas essas cores?",
            "UGH!! Meu baço! Acho que era meu baço!",
            "Se quer ajudar, talvez pare de me esfaquear!",
            "Enfermeiro! Este aqui precisa de supervisão!",
            "Acho que é o que mereço pelo que paguei",
            "Mereço este castigo pelos meus muitos pecados"
        };

        public bool HandleBandage(Mobile from, Bandage band)
        {
            if (from == null || band == null || band.Deleted)
            return true;

            if (from.Karma < 0)
            {
                from.SendMessage("O paciente está com medo de você. Ele recusa sua ajuda.");
                return true;
            }

            if (from.Skills[SkillName.Healing].Value < 30.0)
            {
                from.SendMessage("Sua skill de cura é muito baixa para tratar este paciente.");
                return true;
            }

            DateTime last;
            if (m_LastHealAttempt[from] != null)
                last = (DateTime)m_LastHealAttempt[from];
            else
                last = DateTime.MinValue;

            if ((DateTime.UtcNow - last) < TimeSpan.FromHours(1))
            {
                from.SendMessage("Você já tratou este paciente recentemente.");
                return true;
            }

            m_LastHealAttempt[from] = DateTime.UtcNow;

            band.Consume();

            if ( from.CheckSkill( SkillName.Healing, 30, 125.0 ) )
            {
                int amount = Utility.Random(5,25);
                from.Karma += amount;
                Say(SuccessLines[Utility.Random(SuccessLines.Length)]);
                from.SendMessage("Sua tentativa de cura é bem-sucedida. O paciente se sente melhor.");
                from.AddToBackpack( new MarksOfDevotion( amount ) );
				from.SendMessage( "Você adquiriu" + " " + amount + " " + "Marcas de Devocão!" );
            }
            else
            {
                Say(FailLines[Utility.Random(FailLines.Length)]);
                SpillBlood();
                from.SendMessage("Sua tentativa de cura falhou.");
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
