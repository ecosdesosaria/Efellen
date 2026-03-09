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
	public class MelchiorTheSwordsmith : BaseCreature
	{

		[Constructable]
		public MelchiorTheSwordsmith()  : base(AIType.AI_Thief, FightMode.None, 10, 1, 0.4, 1.6)
		{
            InitStats( 125, 55, 65 ); 
			Name = "Melchior, o Espadachim";
			Body = 0x190;
			Hue = 0;
			Blessed = true;
            SpeechHue = Utility.RandomTalkHue();
            Hue = Utility.RandomSkinHue(); 
			CantWalk = false;
            Utility.AssignRandomHair( this );
            FacialHairItemID = Utility.RandomList( 0, 8254, 8255, 8256, 8257, 8267, 8268, 8269 );
            AddItem( new Boots( Utility.RandomBirdHue() ) );
            AddItem( new GildedRobe( Utility.RandomBirdHue() ) );
            AddItem( new Cloak( Utility.RandomBirdHue() ) );
            AddItem( new Bonnet( Utility.RandomBirdHue() ) );
		}


		public override void OnDoubleClick(Mobile from)
		{
			if (!from.InRange(this.Location, 4))
			{
				from.SendMessage("Você está muito longe para falar com Melchior.");
				return;
			}

            if (Server.Misc.PlayerSettings.GetKeys(from, "Masamune"))
	        {
	        	this.PrivateOverheadMessage(MessageType.Regular, 1150, false, "Eu fiz tudo que pude por você, por enquanto. Volte depois de dominar o Masamune.", from.NetState);
	        	return;
	        }

			from.CloseGump(typeof(MelchiorDialogueGump));
			from.SendGump(new MelchiorDialogueGump(from, this));
		}

		public MelchiorTheSwordsmith(Serial serial) : base(serial)
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

	public class MelchiorDialogueGump : Gump
	{
		private Mobile m_Player;
		private MelchiorTheSwordsmith m_Melchior;

		public MelchiorDialogueGump(Mobile from, MelchiorTheSwordsmith melchior) : base(100, 100)
		{
			m_Player = from;
			m_Melchior = melchior;

			double bushido = from.Skills[SkillName.Bushido].Base;
			int karma = from.Karma;
			int tithing = from.TithingPoints;
			bool hasDreamstone = Server.Misc.PlayerSettings.GetKeys(from, "Dreamstone");

			string name = from.Name;
			string text = GetDialogueText(name, bushido, karma, tithing, hasDreamstone);

			AddBackground(0, 0, 420, 280, 9270);
			AddHtml(20, 20, 380, 200, text, true, true);


			if (ShouldOfferQuest(karma, bushido, tithing))
			{
				AddButton(160, 235, 4005, 4007, 1, GumpButtonType.Reply, 0);
				AddHtml(195, 237, 100, 20, "Continue", false, false);
			}
			else
			{
				AddButton(160, 235, 4005, 4007, 0, GumpButtonType.Reply, 0);
				AddHtml(195, 237, 100, 20, "Close", false, false);
			}
		}

		private string GetDialogueText(string name, double bushido, int karma, int tithing, bool hasDreamstone)
        {
            string intro = "Saudações " + name + ", vaguei longe e aprendi muito, e agora me estabeleci neste pequeno pedaço de meu antigo lar.<br><br>" +
                        "Muito aprendi sobre o ofício da guerra, um ofício do qual me cansei. Para que precisamos de mais espadas?<br><br>";

            if (!hasDreamstone)
            {
                intro += "Além do mais, o que vou fazer com o metal bruto desta terra? Não, minhas habilidades não serviriam para nada a menos que eu tivesse o material correspondente.<br><br>"+
                "Eu precisaria de uma pedra do sonho, que foi perdida no tempo.<br><br>";
            }
            else if (hasDreamstone)
                intro += "Você a tem... Você de alguma forma encontrou a Pedra do Sonho... O metal perfeito. Pensei que tudo estivesse perdido numa era passada.<br><br>"+
                "Não. Não fará bem a ninguém.<br><br>";

            if (karma < 0)
            {
                return intro + "Vejo a podridão nas bordas de sua alma, " + name + ". Você carrega dentro de si uma escuridão que transformaria meu ofício em um instrumento de vilania degradante.<br><br>" +
                    "Fora! Não lhe oferecerei nenhum de meus serviços!";
            }

            if (karma >= 0 && bushido < 111.0)
            {
                return intro + "Vejo sua alma nas bordas de sua ambição, " + name + ". Você empunharia uma lâmina com o desejo de fazer o bem, mas que bem há para ser feito que não tenha sido frustrado mil vezes?<br><br>" +
                    "Não — você tem o coração, mas não a habilidade. Minhas lâminas são algo de terror, feitas para cortar as almas dos homens ao meio.<br><br>" +
                    "Não lhe oferecerei meus serviços, receio.";
            }

            if (karma > 0 && bushido >= 111.0 && tithing >= 50000 && karma >= 15000)
            {
                return intro + "Sua alma pesa como uma pena, " + name + ". Muito bem. Acenderei a forja, uma última vez.";
            }

            if (karma > 0 && bushido >= 111.0)
            {
                return intro + "Vejo sua alma nas bordas de sua ambição, " + name + ". Você empunharia uma lâmina com o desejo de fazer o bem, mas que bem há para ser feito que não tenha sido frustrado mil vezes?<br><br>" +
                    "Prove para mim — prove seu desejo implacável de fazer deste mundo um lugar melhor.<br><br>" +
                    "Dê a Durama o que é seu devido e pratique o bem por si só, e faça-o por sua própria vontade.<br><br>" +
                    "Faça isso e acenderei a forja, uma última vez.";
            }

            return intro;
        }

		private bool ShouldOfferQuest(int karma, double bushido, int tithing)
		{
			return (karma >= 15000 && bushido >= 111.0 && tithing >= 50000);
		}

		public override void OnResponse(NetState state, RelayInfo info)
		{
			if (info.ButtonID == 1)
			{
				m_Player.CloseGump(typeof(MelchiorConfirmationGump));
				m_Player.SendGump(new MelchiorConfirmationGump(m_Player, m_Melchior));
			}
		}
	}

	public class MelchiorConfirmationGump : Gump
	{
		private Mobile m_Player;
		private Mobile m_Melchior;

		public MelchiorConfirmationGump(Mobile from, Mobile melchior) : base(100, 100)
		{
			m_Player = from;
			m_Melchior = melchior;

			AddBackground(0, 0, 350, 160, 9270);
			AddHtml(20, 20, 310, 60, "Você deseja entregar sua Pedra do Sonho a Melchior?<br>Você não encontrará outra. Confirmar também consumirá 50.000 pontos de dízimo.", true, true);
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
                	m_Player.SendMessage("Você não tem uma Pedra do Sonho para entregar.");
                	return;
                }

				if (m_Player.TithingPoints < 50000)
				{
					m_Player.SendMessage("Você não tem os 50.000 pontos de dízimo necessários.");
					return;
				}

				m_Player.TithingPoints -= 50000;

				Item reward = new LevelNoDachiMasamune(m_Player.Name);
				m_Player.AddToBackpack(reward);

				m_Player.SendMessage("Melchior pega a Pedra do Sonho e acena solenemente.");
                m_Player.SendMessage("Você recebe a lendária Masamune!");
                m_Melchior.Say(true, "Então está feito. A forja está em silêncio mais uma vez...");
                if ( PlayerSettings.GetKeys( m_Player, "Masamune" ) )
                {
                    m_Player.PrivateOverheadMessage(MessageType.Regular, 1150, false, "Belchior já forjou a Masamune para você!", m_Player.NetState);
                }
                else
                {
                    PlayerSettings.SetKeys( m_Player, "Masamune", true );
                    PlayerSettings.SetKeys( m_Player,"DreamstoneUsed", true);
                    m_Player.SendSound( 0x3D );
                    m_Player.PrivateOverheadMessage(MessageType.Regular, 1150, false, "Você adquiriu a Masamune.", m_Player.NetState);
                }
                }
                else
                {
                    m_Player.SendMessage("Você decide manter sua Pedra do Sonho por enquanto.");
                }
		}
	}
}