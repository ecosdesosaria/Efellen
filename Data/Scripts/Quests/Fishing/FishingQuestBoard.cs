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
	public class FishingQuestBoard : Item
	{
		[Constructable]
		public FishingQuestBoard() : base(0x577B)
		{
			Weight = 1.0;
			Name = "Seeking Brave Sailors";
			Hue = 0x8AB;
		}

		public override void GetContextMenuEntries( Mobile from, List<ContextMenuEntry> list ) 
		{ 
			base.GetContextMenuEntries( from, list ); 
			list.Add( new SpeechGumpEntry( from ) );
			list.Add( new FishingQuestEntry( from ) );
			list.Add( new FishingQuestComplete( from ) ); 
		}

		public override void OnDoubleClick( Mobile e )
		{
			if ( e.InRange( this.GetWorldLocation(), 4 ) )
			{
				e.CloseGump( typeof( BoardGump ) );
				e.SendGump( new BoardGump( e, "PROCURAM-SE MARINHEIROS CORAJOSOS", "O povo da cidade está procurando por marinheiros corajosos dos altos mares, " + e.Name +". Aos marinheiros são oferecidas recompensas por piratas ou criaturas marinhas, ou itens que devem procurar e recuperar. Cada missão deve ser concluída para receber outra. Se você falhar em uma missão, o povo não concederá outra a menos que sejam dadas reparações. Quanto mais famoso for o marinheiro, maior a chance de conseguir uma recompensa valiosa ou um item de alto valor para encontrar.<br><br><br><br>Para obter uma missão, basta perguntar a este mural se algum morador deseja 'contratá-lo'. Estas missões não o enviam para mares onde você nunca esteve. Quaisquer detalhes da missão podem ser lidos no registro de missões (digitando '[quests'). Quando tal missão for concluída, retorne a qualquer um desses murais e selecione que está 'concluído'. Você será recompensado com algum ouro e fama. Ganhará algum karma, a menos que seu karma esteja bloqueado. Nesse caso, você perderá karma.", "#9dc2e8", false ) );
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
				m_Mobile.SendGump( new BoardGump( m_Mobile, "PROCURAM-SE MARINHEIROS CORAJOSOS", "O povo da cidade está procurando por marinheiros corajosos dos altos mares, " + m_Mobile.Name +". Aos marinheiros são oferecidas recompensas por piratas ou criaturas marinhas, ou itens que devem procurar e recuperar. Cada missão deve ser concluída para receber outra. Se você falhar em uma missão, o povo não concederá outra a menos que sejam dadas reparações. Quanto mais famoso for o marinheiro, maior a chance de conseguir uma recompensa valiosa ou um item de alto valor para encontrar.<br><br><br><br>Para obter uma missão, basta perguntar a este mural se algum morador deseja 'contratá-lo'. Estas missões não o enviam para mares onde você nunca esteve. Quaisquer detalhes da missão podem ser lidos no registro de missões (digitando '[quests'). Quando tal missão for concluída, retorne a qualquer um desses murais e selecione que está 'concluído'. Você será recompensado com algum ouro e fama. Ganhará algum karma, a menos que seu karma esteja bloqueado. Nesse caso, você perderá karma.", "#9dc2e8", false ) );
            }
        }

		public class FishingQuestEntry : ContextMenuEntry
		{
			private Mobile m_Mobile;
			
			public FishingQuestEntry( Mobile from ) : base( 6120, 12 )
			{
				m_Mobile = from;
			}

			public override void OnClick()
			{
			    if( !( m_Mobile is PlayerMobile ) )
				return;

				string myQuest = PlayerSettings.GetQuestInfo( m_Mobile, "FishingQuest" );

				int nAllowedForAnotherQuest = FishingQuestFunctions.QuestTimeNew( m_Mobile );
				int nServerQuestTimeAllowed = MyServerSettings.GetTimeBetweenQuests();
				int nWhenForAnotherQuest = nServerQuestTimeAllowed - nAllowedForAnotherQuest;
				string sAllowedForAnotherQuest = nWhenForAnotherQuest.ToString();

				if ( PlayerSettings.GetQuestState( m_Mobile, "FishingQuest" ) )
				{
					m_Mobile.PrivateOverheadMessage(MessageType.Regular, 1150, false, "Você já está em uma missão. Retorne aqui quando terminar.", m_Mobile.NetState);
				}
				else if ( nWhenForAnotherQuest > 0 )
				{
					m_Mobile.PrivateOverheadMessage(MessageType.Regular, 1150, false, "Não há missões no momento. Volte em " + sAllowedForAnotherQuest + " minutos.", m_Mobile.NetState);
				}
				else
				{
					int nFame = m_Mobile.Fame * 2;
						nFame = Utility.RandomMinMax( 0, nFame )+2000;

					FishingQuestFunctions.FindTarget( m_Mobile, nFame );

					string TellQuest = FishingQuestFunctions.QuestStatus( m_Mobile ) + ".";
					m_Mobile.PrivateOverheadMessage(MessageType.Regular, 1150, false, TellQuest, m_Mobile.NetState);
				}
            }
        }

		public class FishingQuestComplete : ContextMenuEntry
		{
			private Mobile m_Mobile;
			
			public FishingQuestComplete( Mobile from ) : base( 548, 12 )
			{
				m_Mobile = from;
			}

			public override void OnClick()
			{
			    if( !( m_Mobile is PlayerMobile ) )
				return;

				string myQuest = PlayerSettings.GetQuestInfo( m_Mobile, "FishingQuest" );

				int nSucceed = FishingQuestFunctions.DidQuest( m_Mobile );

				if ( nSucceed > 0 )
				{
					FishingQuestFunctions.PayAdventurer( m_Mobile );
				}
				else if ( myQuest.Length > 0 )
				{
					m_Mobile.CloseGump( typeof( BoardGump ) );
					m_Mobile.SendGump( new BoardGump( m_Mobile, "SUA REPUTAÇÃO ESTÁ EM JOGO", "Você está atualmente em uma missão que não deve ser muito difícil para um marinheiro tão experiente quanto você. Se acha que esta missão está além da sua coragem, pode nunca mais ser convidado a fazer outra, a menos que sejam pagas reparações. Se deseja se livrar desta missão, deve pagar a recompensa oferecida para restaurar sua reputação com o povo da cidade. Portanto, qualquer que fosse a recompensa, você deve colocar esse total em qualquer um destes murais... se desejar abandonar esta missão, é claro.", "#9dc2e8", false ) );
				}
				else
				{
					m_Mobile.PrivateOverheadMessage(MessageType.Regular, 1150, false, "Você não está em uma missão no momento.", m_Mobile.NetState);
				}
            }
        }

		public override bool OnDragDrop( Mobile from, Item dropped )
		{
			if ( dropped is Gold )
			{
				int nPenalty = FishingQuestFunctions.QuestFailure( from );

				if ( dropped.Amount >= nPenalty )
				{
					PlayerSettings.ClearQuestInfo( from, "FishingQuest" );
					FishingQuestFunctions.QuestTimeAllowed( from );
					from.PrivateOverheadMessage(MessageType.Regular, 1153, false, "Alguém acabará cuidando disso eventualmente.", from.NetState);
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

		public FishingQuestBoard(Serial serial) : base(serial)
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
