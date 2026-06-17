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

namespace Server.Mobiles
{
	public class Xardok : BasePerson
	{
		public override bool InitialInnocent{ get{ return true; } }

		public override string TalkGumpTitle{ get{ return "The Guild Of Assassins"; } }
		public override string TalkGumpSubject{ get{ return "Xardok"; } }

		[Constructable]
		public Xardok() : base()
		{
			SpeechHue = Utility.RandomTalkHue();
			NameHue = 1154;

			Body = 400; 
			Name = "Xardok";
			Title = "o Barão";
			AI = AIType.AI_Citizen;
			FightMode = FightMode.None;

			AddItem( new Boots() );
			Item cloth1 = new Robe();
				cloth1.Hue = 0x6E8;
				AddItem( cloth1 );
			Item cloth2 = new FloppyHat();
				cloth2.Hue = 0x6E8;
				AddItem( cloth2 );

			SetStr( 200 );
			SetDex( 200 );
			SetInt( 200 );

			SetDamage( 15, 20 );
			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 35, 45 );
			SetResistance( ResistanceType.Fire, 25, 30 );
			SetResistance( ResistanceType.Cold, 25, 30 );
			SetResistance( ResistanceType.Poison, 10, 20 );
			SetResistance( ResistanceType.Energy, 10, 20 );

			SetSkill( SkillName.FistFighting, 100 );
			Karma = 10000;
			VirtualArmor = 100;

			Hue = 0x83EA;
			FacialHairItemID = 0x204C; // BEARD
			FacialHairHue = 0x455;
			HairItemID = 0x203C; // LONG HAIR
			HairHue = 0x455;
		}

		public override bool OnBeforeDeath()
		{
			Say("In Vas Mani");
			this.Hits = this.HitsMax;
			this.FixedParticles( 0x376A, 9, 32, 5030, EffectLayer.Waist );
			this.PlaySound( 0x202 );
			return false;
		}

		public override bool IsEnemy( Mobile m )
		{
			return false;
		}

		public override void GetContextMenuEntries( Mobile from, List<ContextMenuEntry> list ) 
		{ 
			base.GetContextMenuEntries( from, list ); 
			list.Add( new XardokEntry( from, this ) );
			list.Add( new XardokComplete( from, this ) ); 
		} 

		public class XardokEntry : ContextMenuEntry
		{
			private Mobile m_Mobile;
			private Mobile m_Giver;
			
			public XardokEntry( Mobile from, Mobile giver ) : base( 6120, 12 )
			{
				m_Mobile = from;
				m_Giver = giver;
			}

			public override void OnClick()
			{
			    if( !( m_Mobile is PlayerMobile ) )
				return;
				
				PlayerMobile mobile = (PlayerMobile) m_Mobile;

				string myQuest = PlayerSettings.GetQuestInfo( m_Mobile, "AssassinQuest" );

				int nAllowedForAnotherQuest = AssassinFunctions.QuestTimeNew( m_Mobile );
				int nServerQuestTimeAllowed = MyServerSettings.GetTimeBetweenQuests();
				int nWhenForAnotherQuest = nServerQuestTimeAllowed - nAllowedForAnotherQuest;
				string sAllowedForAnotherQuest = nWhenForAnotherQuest.ToString();

				if ( PlayerSettings.GetQuestState( m_Mobile, "AssassinQuest" ) )
				{
					m_Giver.Say("Você já tem suas ordens. Retorne a mim quando terminar a tarefa.");
				}
				else if ( mobile.NpcGuild != NpcGuild.AssassinsGuild )
				{
					m_Giver.Say("Hmmm... você não parece do tipo com quem eu queira discutir assuntos.");
				}
				else if ( m_Mobile.Karma > -1250 ) 
				{
					m_Giver.Say("Hmmm... talvez me mostre primeiro que você poderia lidar com tais tarefas.");
				}
				else if ( nWhenForAnotherQuest > 0 )
				{
					m_Giver.Say("Não tenho nada para você no momento. Verifique novamente em " + sAllowedForAnotherQuest + " minutos.");
				}
				else
				{
					int nFame = m_Mobile.Fame * 2;
						nFame = Utility.RandomMinMax( 0, nFame )+2000;

					if (Utility.RandomMinMax( 1, 100 ) > 30)
					{
						AssassinFunctions.FindTarget( m_Mobile, nFame );
					}
					else
					{
						AssassinFunctions.FindInnocentTarget( m_Mobile );
					}

					string TellQuest = AssassinFunctions.QuestStatus( m_Mobile ) + ".";
					m_Giver.Say( TellQuest );
				}
            }
        }

		public class XardokComplete : ContextMenuEntry
		{
			private Mobile m_Mobile;
			private Mobile m_Giver;
			
			public XardokComplete( Mobile from, Mobile giver ) : base( 548, 12 )
			{
				m_Mobile = from;
				m_Giver = giver;
			}

			public override void OnClick()
			{
			    if( !( m_Mobile is PlayerMobile ) )
				return;

				string myQuest = PlayerSettings.GetQuestInfo( m_Mobile, "AssassinQuest" );

				int nSucceed = AssassinFunctions.DidAssassin( m_Mobile );

				if ( nSucceed > 0 )
				{
					AssassinFunctions.PayAssassin( m_Mobile, m_Giver );
				}
				else if ( myQuest.Length > 0 )
				{
					if ( ! m_Mobile.HasGump( typeof( SpeechGump ) ) )
					{
						Server.Misc.IntelligentAction.SayHey( m_Mobile );
						m_Mobile.SendGump(new SpeechGump( m_Mobile, "Failure Is Frowned Upon", SpeechFunctions.SpeechText( m_Giver, m_Mobile, "XardokFail" ) ));
					}
				}
				else
				{
					m_Giver.Say("Concluído? O quê? Não sei do que você está falando.");
				}
            }
        }

		public Xardok( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
}