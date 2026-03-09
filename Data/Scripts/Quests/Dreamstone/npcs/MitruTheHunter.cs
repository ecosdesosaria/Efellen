using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections;
using System.Collections.Generic;
using Server.Misc;

namespace Server.Mobiles
{
	public class MitruTheHunter : BaseCreature
	{

		[Constructable]
		public MitruTheHunter()  : base(AIType.AI_Thief, FightMode.None, 10, 1, 0.4, 1.6)
		{
            InitStats( 125, 55, 65 ); 
			Name = "Mitru, o Caçador";
			Body = 0x190;
			Hue = 1040;
			Blessed = true;
            SpeechHue = 1040;
            CantWalk = false;
            Utility.AssignRandomHair( this );
            AddItem( new DoubleBladedStaff());
            AddItem( new RangerArms() );
            AddItem( new RangerLegs() );
            AddItem( new Cloak( Utility.RandomBirdHue() ) );
            AddItem( new RangerChest() );
            AddItem( new RangerGloves() );
            AddItem( new Boots( Utility.RandomBirdHue() ) );
            AddItem( new FancyHood( Utility.RandomBirdHue( ) ) );
		}


		public override void OnDoubleClick(Mobile from)
		{
			if (!from.InRange(this.Location, 4))
			{
				from.SendMessage("Você está muito longe para falar com Mitru.");
				return;
			}

            if (Server.Misc.PlayerSettings.GetKeys(from, "Mitru"))
	        {
	        	this.PrivateOverheadMessage(MessageType.Regular, 1150, false, "Fiz tudo que pude por você, por enquanto. Volte quando você e a lâmina cantarem como um só.", from.NetState);
	        	return;
	        }

			from.CloseGump(typeof(MitruDialogueGump));
			from.SendGump(new MitruDialogueGump(from, this));
		}

		public MitruTheHunter(Serial serial) : base(serial)
		{
		}

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

	public class MitruDialogueGump : Gump
	{
		private Mobile m_Player;
		private MitruTheHunter m_Mitru;

		public MitruDialogueGump(Mobile from, MitruTheHunter mitru) : base(100, 100)
		{
			m_Player = from;
			m_Mitru = mitru;

			double tactics = from.Skills[SkillName.Tactics].Base;
            int rawDex = from.RawDex;
			int fame = from.Fame;
			bool hasDreamstone = Server.Misc.PlayerSettings.GetKeys(from, "Dreamstone");

			string name = from.Name;
			string text = GetDialogueText(name, tactics, fame, rawDex, hasDreamstone);

			AddBackground(0, 0, 420, 280, 9270);
			AddHtml(20, 20, 380, 200, text, true, true);


			if (ShouldOfferQuest(fame, tactics, rawDex))
			{
				AddButton(160, 235, 4005, 4007, 1, GumpButtonType.Reply, 0);
				AddHtml(195, 237, 100, 20, "Continuar", false, false);
			}
			else
			{
				AddButton(160, 235, 4005, 4007, 0, GumpButtonType.Reply, 0);
				AddHtml(195, 237, 100, 20, "Fechar", false, false);
			}
		}

		private string GetDialogueText(string name, double tactics, int fame, double rawDex, bool hasDreamstone)
        {
            string intro = "Salve e bem-vindo " + name + "!.<br><br>" +
                        "Por muito tempo ouvi a canção - Você a ouve? O choque da lâmina e do escudo, o borrifo de sangue, a conquista da glória!<br><br>"+
                        "Sou apenas um devoto apaixonado da doce música que o conflito desta terra produz";

            if (!hasDreamstone)
            {
                intro += "Você tem interesse em se juntar à grande canção?<br><br>"+
                "Prove para mim, então. Adquira a fabulosa Pedra do Sonho da temível criatura que a guarda, e essa batalha certamente será digna de uma canção!<br><br>";
            }
            else if (hasDreamstone)
                intro += "Você a tem... Você matou a fera? Não importa como o fez, o que importa é que um se levantou contra muitos e conquistou o mais ousado dos inimigos!<br><br>";

            if (fame <= 0)
            {
                return intro + "Que coisa vergonhosa seria travar esta canção, não seria? Cresça sua lenda, enfrente perigos cada vez"+ 
                " maiores e volte! Não vou entreter aqueles não dignos da grande canção!<br><br>";
            }

            if (fame >= 0 && (tactics < 111.0 || rawDex < 125))
            {
                return intro + "Você parece valente, mas não acho que mereça uma arma como esta, " + name + "." +
                    "Fortaleça-se - Aprenda os detalhes da guerra, o comércio de sangue, e torne-se ágil matando muitas bestas vis, e considerarei você digno de uma arma lendária!<br><br>" +
                    "Não lhe oferecerei meus serviços hoje, receio.<br><br>";
            }

            if (tactics >= 111.0 && rawDex >= 125 && fame >= 15000)
            {
                return intro + "Você fará a arma se orgulhar, " + name + ". Muito bem. Concederei a você este presente.<br><br>";
            }

            if (fame > 0 && (tactics >= 111.0 || rawDex >= 125))
            {
                return intro + "Eu soube sobre você, " + name + ".<br><br>" +
                    "Mas não tanto quanto gostaria.<br><br>" +
                    "Esculpa uma lenda para si mesmo nos ossos desta terra! Faça sua lenda crescer, e lhe darei uma arma lendária!<br><br>";
            }
            return intro;
        }

		private bool ShouldOfferQuest(int fame, double tactics, double rawDex)
		{
			return (fame >= 15000 && tactics >= 111.0 && rawDex >= 125);
		}

		public override void OnResponse(NetState state, RelayInfo info)
		{
			if (info.ButtonID == 1)
			{
				m_Player.CloseGump(typeof(MitruConfirmationGump));
				m_Player.SendGump(new MitruConfirmationGump(m_Player, m_Mitru));
			}
		}
	}

	public class MitruConfirmationGump : Gump
	{
		private Mobile m_Player;
		private Mobile m_Mitru;

		public MitruConfirmationGump(Mobile from, Mobile mitru) : base(100, 100)
		{
			m_Player = from;
			m_Mitru = mitru;

			AddBackground(0, 0, 350, 160, 9270);
            AddHtml(20, 20, 310, 60, "Você deseja entregar sua Pedra do Sonho a Mitru?<br>Você não encontrará outra.", true, true);
            AddButton(60, 110, 4005, 4007, 1, GumpButtonType.Reply, 0);
            AddHtml(95, 112, 60, 20, "Sim", false, false);
            AddButton(190, 110, 4005, 4007, 0, GumpButtonType.Reply, 0);
            AddHtml(225, 112, 60, 20, "Não", false, false);
		}

		public override void OnResponse(NetState state, RelayInfo info)
		{
			if (m_Player == null || m_Player.Deleted)
				return;

			if (info.ButtonID == 1)
			{
				bool hasUsableDreamstone = Server.Misc.PlayerSettings.GetKeys(m_Player, "Dreamstone") 
	                    && !Server.Misc.PlayerSettings.GetKeys(m_Player, "DreamstoneUsed");

                if(!hasUsableDreamstone)
                {
                    m_Player.SendMessage("Você não tem uma Pedra do Sonho para dar.");
                    return;
                }

                Item reward = new LevelDoubleBladedStaffMoonDancer(m_Player.Name);
                m_Player.AddToBackpack(reward);

                m_Player.SendMessage("Mitru pega a Pedra do Sonho e acena com entusiasmo.");
                m_Player.SendMessage("Você recebe a lendária Dançarina da Lua!");
                if ( PlayerSettings.GetKeys( m_Player, "Mitru" ) )
                {
                    m_Player.PrivateOverheadMessage(MessageType.Regular, 1150, false, "Mitru já lhe presenteou com uma arma!", m_Player.NetState);
                }
                else
                {
                    PlayerSettings.SetKeys( m_Player, "Mitru", true );
                    PlayerSettings.SetKeys( m_Player,"DreamstoneUsed", true);
                    m_Player.SendSound( 0x3D );
                    m_Player.PrivateOverheadMessage(MessageType.Regular, 1150, false, "Você adquiriu a Dançarina da Lua.", m_Player.NetState);
                }
            }
            else
            {
                m_Player.SendMessage("Você decide manter sua Pedra do Sonho por enquanto.");
            }
		}
	}
}