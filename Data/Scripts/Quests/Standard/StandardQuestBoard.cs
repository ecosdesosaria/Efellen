using System;
using System.Collections;
using Server.ContextMenus;
using System.Collections.Generic;
using Server.Misc;
using Server.Network;
using Server;
using Server.Items;
using Server.Gumps;
using Server.Mobiles;
using Server.Commands;

namespace Server.Items
{
	[Flipable(0x577C, 0x577B)]
	public class StandardQuestBoard : Item
	{
		[Constructable]
		public StandardQuestBoard() : base(0x577B)
		{
			Weight = 1.0;
			Name = "Seeking Brave Adventurers";
			Hue = 0xB26;
		}

		public override void GetContextMenuEntries( Mobile from, List<ContextMenuEntry> list ) 
		{ 
			base.GetContextMenuEntries( from, list ); 
			list.Add( new SpeechGumpEntry( from ) );
			list.Add( new StandardQuestEntry( from ) );
			list.Add( new StandardQuestComplete( from ) ); 
		}

		public override void OnDoubleClick( Mobile e )
		{
			if ( e.InRange( this.GetWorldLocation(), 4 ) )
			{
				e.CloseGump( typeof( BoardGump ) );
				e.SendGump( new BoardGump( e, "PROCURANDO AVENTUREIROS CORAJOSOS", "Os moradores estão procurando por aventureiros corajosos, " + e.Name + ". Aos aventureiros são dados contratos nos quais devem procurar e matar, ou itens que devem procurar e recuperar. Cada missão deve ser concluída para obter outra. Se você falhar em uma missão, os moradores não concederão outra, a menos que sejam dadas reparações. Quanto mais famoso um aventureiro, maior a chance de obter uma recompensa de alto preço ou um item valioso para encontrar. Claro, quanto mais ouro para uma recompensa, geralmente significa quão difícil a missão pode ser.<br><br><br><br>Para obter uma missão, deve-se simplesmente perguntar a este quadro de avisos se algum morador deseja 'contratá-lo'. Essas missões não o enviam para uma terra onde você nunca esteve, mas podem enviá-lo para qualquer masmorra em terras que você já viajou. Se você não sabe a localização de um lugar específico, é melhor começar sua exploração dessas áreas. Quaisquer outros detalhes da missão podem ser lidos no registro de missões (digitando '[quests'). Quando tal missão for concluída, retorne a qualquer um desses quadros de avisos e selecione que você está 'pronto'. Você será recompensado com algum ouro e fama. Você ganhará algum karma, a menos que seu karma esteja trancado. Nesse caso, você perderá karma em vez disso.", "#e9e9e9", false ) );
			}
			else
			{
				e.SendLocalizedMessage( 502138 ); // That is too far away for you to use
			}
		}

		public class SpeechGumpEntry : ContextMenuEntry
		{
			private Mobile m_Mobile;
			
			public SpeechGumpEntry( Mobile from ) : base( 1024, 3 )
			{
				m_Mobile = from;
			}

			public override void OnClick()
			{
			    if( !( m_Mobile is PlayerMobile ) )
				return;
				
				m_Mobile.CloseGump( typeof( BoardGump ) );
				m_Mobile.SendGump( new BoardGump( m_Mobile, "PROCURANDO AVENTUREIROS CORAJOSOS", "Os moradores estão procurando por aventureiros corajosos, " + m_Mobile.Name + ". Aos aventureiros são dados contratos nos quais devem procurar e matar, ou itens que devem procurar e recuperar. Cada missão deve ser concluída para obter outra. Se você falhar em uma missão, os moradores não concederão outra, a menos que sejam dadas reparações. Quanto mais famoso um aventureiro, maior a chance de obter uma recompensa de alto preço ou um item valioso para encontrar. Claro, quanto mais ouro para uma recompensa, geralmente significa quão difícil a missão pode ser.<br><br>Para obter uma missão, deve-se simplesmente perguntar a este quadro de avisos se algum morador deseja 'contratá-lo'. Essas missões não o enviam para uma terra onde você nunca esteve, mas podem enviá-lo para qualquer masmorra em terras que você já viajou. Se você não sabe a localização de um lugar específico, é melhor começar sua exploração dessas áreas. Quaisquer outros detalhes da missão podem ser lidos no registro de missões (digitando '[quests'). Quando tal missão for concluída, retorne a qualquer um desses quadros de avisos e selecione que você está 'pronto'. Você será recompensado com algum ouro e fama. Você ganhará algum karma, a menos que seu karma esteja trancado. Nesse caso, você perderá karma em vez disso.", "#e9e9e9", false ) );
            }
        }

		public class StandardQuestEntry : ContextMenuEntry
		{
			private Mobile m_Mobile;
			
			public StandardQuestEntry( Mobile from ) : base( 6120, 12 )
			{
				m_Mobile = from;
			}

			public override void OnClick()
			{
			    if( !( m_Mobile is PlayerMobile ) )
				return;

				string myQuest = PlayerSettings.GetQuestInfo( m_Mobile, "StandardQuest" );

				int nAllowedForAnotherQuest = StandardQuestFunctions.QuestTimeNew( m_Mobile );
				int nServerQuestTimeAllowed = MyServerSettings.GetTimeBetweenQuests();
				int nWhenForAnotherQuest = nServerQuestTimeAllowed - nAllowedForAnotherQuest;
				string sAllowedForAnotherQuest = nWhenForAnotherQuest.ToString();

				if ( PlayerSettings.GetQuestState( m_Mobile, "StandardQuest" ) )
				{
					m_Mobile.PrivateOverheadMessage(MessageType.Regular, 1150, false, "Você já está em uma missão. Volte aqui quando terminar.", m_Mobile.NetState);
				}
				else if ( nWhenForAnotherQuest > 0 )
				{
					m_Mobile.PrivateOverheadMessage(MessageType.Regular, 1150, false, "Não há missões no momento. Volte em " + sAllowedForAnotherQuest + " minutos.", m_Mobile.NetState);
				}
				else
				{
					int nFame = m_Mobile.Fame * 2;
						nFame = Utility.RandomMinMax( 0, nFame )+2000;

					StandardQuestFunctions.FindTarget( m_Mobile, nFame );

					string TellQuest = StandardQuestFunctions.QuestStatus( m_Mobile ) + ".";
					m_Mobile.PrivateOverheadMessage(MessageType.Regular, 1150, false, TellQuest, m_Mobile.NetState);
				}
            }
        }

		public class StandardQuestComplete : ContextMenuEntry
		{
			private Mobile m_Mobile;
			
			public StandardQuestComplete( Mobile from ) : base( 548, 12 )
			{
				m_Mobile = from;
			}

			public override void OnClick()
			{
			    if( !( m_Mobile is PlayerMobile ) )
				return;

				string myQuest = PlayerSettings.GetQuestInfo( m_Mobile, "StandardQuest" );

				int nSucceed = StandardQuestFunctions.DidQuest( m_Mobile );

				if ( nSucceed > 0 )
				{
					StandardQuestFunctions.PayAdventurer( m_Mobile );
				}
				else if ( myQuest.Length > 0 )
				{
					m_Mobile.CloseGump( typeof( BoardGump ) );
					m_Mobile.SendGump( new BoardGump( m_Mobile, "SUA REPUTAÇÃO ESTÁ EM JOGO", "Você está atualmente em uma missão que não deve ser muito difícil para alguém tão resistente quanto você. Se você acha que esta missão está além da sua coragem, pode nunca mais ser convidado a fazer outra, a menos que sejam pagas reparações. Se você deseja se livrar desta missão, então deve pagar a recompensa oferecida para restaurar sua reputação com os moradores. Portanto, qualquer que fosse a recompensa, você deve colocar esse total em qualquer um desses quadros de avisos... se desejar abandonar esta missão, isto é.", "#e9e9e9", false ) );
				}
				else
				{
					m_Mobile.PrivateOverheadMessage(MessageType.Regular, 1150, false, "Você não está atualmente em uma missão.", m_Mobile.NetState);
				}
            }
        }

		public override bool OnDragDrop( Mobile from, Item dropped )
		{
			if ( dropped is Gold )
			{
				int nPenalty = StandardQuestFunctions.QuestFailure( from );

				if ( dropped.Amount >= nPenalty )
				{
					PlayerSettings.ClearQuestInfo( from, "StandardQuest" );
					StandardQuestFunctions.QuestTimeAllowed( from );
					from.PrivateOverheadMessage(MessageType.Regular, 1153, false, "Alguém eventualmente cuidará disso.", from.NetState);
					dropped.Delete();
				}
				else
				{
					from.AddToBackpack ( dropped );
				}
			}
			else
			{
				from.AddToBackpack ( dropped );
			}
			return true;
		}

		public StandardQuestBoard(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int) 0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}
	}
}