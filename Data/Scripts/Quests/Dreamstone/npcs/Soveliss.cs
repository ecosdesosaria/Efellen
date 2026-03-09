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
	public class Soveliss : BaseCreature
	{

		[Constructable]
		public Soveliss()  : base(AIType.AI_Thief, FightMode.None, 10, 1, 0.4, 1.6)
		{
            InitStats( 125, 55, 65 ); 
			Name = "Soveliss, a ranger";
			Body = 0x190;
			Hue = 1420;
			Blessed = true;
            SpeechHue = 1420;
            CantWalk = false;
            Utility.AssignRandomHair( this );
            AddItem( new RangerArms() );
            AddItem( new RangerLegs() );
            AddItem( new Cloak( Utility.RandomBirdHue() ) );
            AddItem( new RangerChest() );
            AddItem( new RangerGloves());
            AddItem( new Boots( Utility.RandomBirdHue() ) );
            AddItem( new FancyHood( Utility.RandomBirdHue( ) ) );
            AddItem( new Daisho());
		}


		public override void OnDoubleClick(Mobile from)
		{
			if (!from.InRange(this.Location, 4))
			{
				from.SendMessage("Você está muito longe para falar com Soveliss.");
				return;
			}

            if (Server.Misc.PlayerSettings.GetKeys(from, "Soveliss"))
	        {
	        	this.PrivateOverheadMessage(MessageType.Regular, 1150, false, "Eu fiz tudo que pude por você, por enquanto. Volte assim que terminar sua caçada.", from.NetState);
	        	return;
	        }

			from.CloseGump(typeof(SovelissDialogueGump));
			from.SendGump(new SovelissDialogueGump(from, this));
		}

		public Soveliss(Serial serial) : base(serial)
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

	public class SovelissDialogueGump : Gump
	{
		private Mobile m_Player;
		private Soveliss m_Soveliss;

		public SovelissDialogueGump(Mobile from, Soveliss soveliss) : base(100, 100)
		{
			m_Player = from;
			m_Soveliss = soveliss;

			double tracking = from.Skills[SkillName.Tracking].Base;
            int rawAttr = (from.RawStr + from.RawDex)/2;
			int fame = from.Fame;
			bool hasDreamstone = Server.Misc.PlayerSettings.GetKeys(from, "Dreamstone");

			string name = from.Name;
			string text = GetDialogueText(name, tracking, fame, rawAttr, hasDreamstone);

			AddBackground(0, 0, 420, 280, 9270);
			AddHtml(20, 20, 380, 200, text, true, true);


			if (ShouldOfferQuest(fame, tracking, rawAttr))
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

		private string GetDialogueText(string name, double tracking, int fame, double rawAttr, bool hasDreamstone)
        {
            string intro = "Mae govannen! " + name + ". Então você se junta a mim na minha caçada.<br><br>" +
                        "Por muito tempo persegui esta terra, protegendo-a das muitas ameaças que a cercam.<br><br>"+
                        "Você também é um ranger? Tenho uma tarefa que preciso completar antes de poder retornar em segurança para casa, mellon.<br><br>";

            if (!hasDreamstone)
            {
                intro += "Se você me ajudasse a encontrar uma Pedra do Sonho, eu poderia conceder a você a arma que me foi confiada para adquiri-la, se você for digno dela.<br><br>";
            }
            else if (hasDreamstone)
                intro += "Você a tem... Em qual terra você encontrou a fera? Por muito tempo vaguei...<br><br>";

            if (fame <= 0)
            {
                return intro + "Nay! Você não está pronto para isto! É um símbolo do meu povo e requer alguém que a honre apropriadamente!<br><br>";
            }

            if (fame >= 0 && (tracking < 111.0 || rawAttr < 110))
            {
                return intro + "Você parece confiável, mas não acho que esteja pronto para uma arma como esta, " + name + ".<br>" +
                    "Aprenda os caminhos da caça, torne-se um com a natureza, e considerarei você digno das armas do meu povo.<br><br>" +
                    "Não lhe oferecerei meus serviços hoje, receio.<br><br>";
            }

            if (tracking >= 111.0 && rawAttr >= 125 && fame >= 15000)
            {
                return intro + "Você fará a arma se orgulhar, " + name + ". Muito bem. Concederei a você este presente.<br><br>";
            }

            if (fame > 0 && (tracking >= 111.0 || rawAttr >= 110))
            {
                return intro + "Conheço seus feitos, " + name + ".<br><br>" +
                    "Conheço seus feitos, mas não tanto quanto gostaria.<br><br>" +
                    "Cace as bestas que assombram esta terra, faça isso e lhe darei um presente digno de minha gente das florestas!<br><br>";
            }
            return intro;
        }

		private bool ShouldOfferQuest(int fame, double tracking, double rawAttr)
		{
			return (fame >= 15000 && tracking >= 111.0 && rawAttr >= 110);
		}

		public override void OnResponse(NetState state, RelayInfo info)
		{
			if (info.ButtonID == 1)
			{
				m_Player.CloseGump(typeof(SovelissConfirmationGump));
				m_Player.SendGump(new SovelissConfirmationGump(m_Player, m_Soveliss));
			}
		}
	}

	public class SovelissConfirmationGump : Gump
	{
		private Mobile m_Player;
		private Mobile m_Soveliss;

		public SovelissConfirmationGump(Mobile from, Mobile soveliss) : base(100, 100)
		{
			m_Player = from;
			m_Soveliss = soveliss;

			AddBackground(0, 0, 350, 160, 9270);
            AddHtml(20, 20, 310, 60, "Você deseja entregar sua Pedra do Sonho a Soveliss?<br>Você não encontrará outra.", true, true);
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

                Item reward = new LevelElvenCompositeLongbowDragonBane(m_Player.Name);
                m_Player.AddToBackpack(reward);

                m_Player.SendMessage("Soveliss pega a Pedra do Sonho e acena com entusiasmo.");
                m_Player.SendMessage("Você recebe o lendário Flagelo do Dragão!");
                if ( PlayerSettings.GetKeys( m_Player, "Soveliss" ) )
                {
                    m_Player.PrivateOverheadMessage(MessageType.Regular, 1150, false, "Soveliss já lhe presenteou com uma arma!", m_Player.NetState);
                }
                else
                {
                    PlayerSettings.SetKeys( m_Player, "Soveliss", true );
                    PlayerSettings.SetKeys( m_Player,"DreamstoneUsed", true);
                    m_Player.SendSound( 0x3D );
                    m_Player.PrivateOverheadMessage(MessageType.Regular, 1150, false, "Você adquiriu o Flagelo do Dragão.", m_Player.NetState);
                }
            }
            else
            {
                m_Player.SendMessage("Você decide manter sua Pedra do Sonho por enquanto.");
            }
		}
	}
}