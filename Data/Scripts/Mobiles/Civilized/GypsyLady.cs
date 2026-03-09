using System;
using System.Collections.Generic;
using Server;
using Server.Targeting;
using Server.Items;
using Server.Network;
using Server.ContextMenus;
using Server.Misc;
using Server.Mobiles;
using System.Collections;
using Server.Gumps;

namespace Server.Mobiles
{
	public class GypsyLady : BasePerson
	{
		public override string TalkGumpTitle{ get{ return "Visions of the Truth"; } }
		public override string TalkGumpSubject{ get{ return "Gypsy"; } }

		private DateTime m_NextTalk;
		public DateTime NextTalk{ get{ return m_NextTalk; } set{ m_NextTalk = value; } }
		public override void OnMovement( Mobile m, Point3D oldLocation )
		{
			if( m is PlayerMobile )
			{
				if ( DateTime.Now >= m_NextTalk && InRange( m, 4 ) && InLOS( m ) )
				{
					switch ( Utility.Random( 45 ))
					{
						case 0: Say("Uma reunião não deve ocorrer ou a quimera de má sorte deve recuar na idade da tentação."); break;
						case 1: Say("Uma lesão acontecerá."); break;
						case 2: Say("Ele não se assimilirá com a juventude orgulhosa."); break;
						case 3: Say("Eles não tecerão perto de um altar."); break;
						case 4: Say("Ela finalmente irá se intrometer."); break;
						case 5: Say("O gambá de diamante não desviará perto de uma fortaleza em um dia ensolarado antes da chegada da beleza."); break;
						case 6: Say("Um término de relacionamento finalmente acontecerá com o malabarista cansado durante a estação de plantio."); break;
						case 7: Say("O hamster astuto vai transgredir com o jovem açafrão em um castelo quando as primeiras flores desabrocharem."); break;
						case 8: Say("Uma traição finalmente acontecerá ou ele balbuciará na vinda da destruição."); break;
						case 9: Say("Ela finalmente vacilará em um mercado antes que seja tarde demais."); break;
						case 10: Say("Uma introdução finalmente acontecerá ou a víbora sedutora não adorará com o druida fanático durante a estação de plantio."); break;
						case 11: Say("A avó de coração partido agirá ou ele não vagueará."); break;
						case 12: Say("Uma derrota não ocorrerá em um castelo durante uma jornada."); break;
						case 13: Say("O comerciante destemido não olhará com o druida violeta."); break;
						case 14: Say("O fugitivo honesto sucumbirá com a lebre faminta antes da vinda da luxúria."); break;
						case 15: Say("Isto julgará no verão."); break;
						case 16: Say("O leão esmeralda nunca terá medo por causa do inverno."); break;
						case 17: Say("Ela finalmente definhará e uma recuperação não acontecerá perto de um poço durante uma tempestade."); break;
						case 18: Say("Uma luta ocorrerá por causa da força de vontade."); break;
						case 19: Say("O mago fanático não crescerá perto de um local sagrado à tarde na vinda da sorte."); break;
						case 20: Say("Ela não ascenderá após o pôr do sol."); break;
						case 21: Say("Isto nunca orará com o zelote tímido."); break;
						case 22: Say("Um encontro deve acontecer com o cozinheiro malicioso."); break;
						case 23: Say("A musa sem remorsos confraternizará na ponte."); break;
						case 24: Say("O berserker rússio deve suspirar ou o escravo sem remorsos não deve discutir com o leão triunfante perto de uma fazenda no equinócio da primavera."); break;
						case 25: Say("Ele esgrimirá com a condessa negra perto de um portal."); break;
						case 26: Say("Uma introdução não acontecerá e o campeão inteligente baixará no equinócio da primavera."); break;
						case 27: Say("O estalajadeiro apressado deve pular."); break;
						case 28: Say("Um acordo deve ocorrer com o clérigo de coração partido."); break;
						case 29: Say("Isto finalmente beneficiará na cidadela na era dos sonhos."); break;
						case 30: Say("Uma dificuldade financeira nunca ocorrerá ou o zumbi desajeitado finalmente chorará à meia-noite."); break;
						case 31: Say("Ela soldará ou ele finalmente esmagará durante a estação de crescimento."); break;
						case 32: Say("O artista ganancioso amarrará e o general de granada não deve compor."); break;
						case 33: Say("O ladrão arrogante não deve obedecer ao ladrão índigo na vinda da alegria."); break;
						case 34: Say("O invocador lavanda esmagará em um tempo de verdade."); break;
						case 35: Say("Uma competição não deve acontecer após a primeira geada."); break;
						case 36: Say("Ela não deve consentir na era da entropia."); break;
						case 37: Say("O pônei iludido não esquecerá em um cemitério em um dia ventoso por causa da alquimia."); break;
						case 38: Say("Uma reversão de fortuna não acontecerá e uma queda não ocorrerá na primavera por causa da luxúria."); break;
						case 39: Say("Ela assegurará na cidadela."); break;
						case 40: Say("O malabarista preguiçoso finalmente perguntará e uma perda não ocorrerá."); break;
						case 41: Say("Uma promessa finalmente ocorrerá e isto não incomodará durante o outono em um tempo de medo."); break;
						case 42: Say("Ele não deve cansar."); break;
						case 43: Say("O burro laranja não vacilará na ponte."); break;
						case 44: Say("A palavra para as profundezas do mago negro é 'bravoka'."); break;
					};
					m_NextTalk = (DateTime.Now + TimeSpan.FromSeconds( 30 ));
				}
			}
		}

		[Constructable]
		public GypsyLady() : base( )
		{
			Hue = Utility.RandomSkinColor();
			NameHue = -1;

			Body = 0x191;
			Female = true;
			Name = NameList.RandomName( "female" );
			Title = "a cigana";

			AddItem( new Kilt( Utility.RandomDyedHue() ) );
			AddItem( new Shirt( Utility.RandomDyedHue() ) );
			AddItem( new ThighBoots() );
			AddItem( new SkullCap( Utility.RandomDyedHue() ) );

			SetSkill( SkillName.Cooking, 65, 88 );
			SetSkill( SkillName.Snooping, 65, 88 );
			SetSkill( SkillName.Stealing, 65, 88 );
			SetSkill( SkillName.Spiritualism, 65, 88 );
			SetSkill( SkillName.FistFighting, 100 );

			SetStr( 100 );
			SetDex( 100 );
			SetInt( 100 );

			SetDamage( 15, 20 );
			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 35, 45 );
			SetResistance( ResistanceType.Fire, 25, 30 );
			SetResistance( ResistanceType.Cold, 25, 30 );
			SetResistance( ResistanceType.Poison, 10, 20 );
			SetResistance( ResistanceType.Energy, 10, 20 );

			VirtualArmor = 30;

			Utility.AssignRandomHair( this );
			HairHue = Utility.RandomHairHue();
			FacialHairItemID = 0;
		}

		private class TruthEntry : ContextMenuEntry
		{
			private GypsyLady m_GypsyLady;
			private Mobile m_From;

			public TruthEntry( GypsyLady GypsyLady, Mobile from ) : base( 2058, 12 )
			{
				m_GypsyLady = GypsyLady;
				m_From = from;
			}

			public override void OnClick()
			{
				m_GypsyLady.FindTruth( m_From );
			}
		}

        public void FindTruth( Mobile from )
        {
            if ( Deleted || !from.Alive )
                return;

			SayTo(from, "Então você quer que eu revele a verdade de um pergaminho para você?");

            from.Target = new RevealTarget(this);
        }

        private class RevealTarget : Target
        {
            private GypsyLady m_GypsyLady;

            public RevealTarget( GypsyLady mage ) : base(12, false, TargetFlags.None)
            {
                m_GypsyLady = mage;
            }

            protected override void OnTarget( Mobile from, object targeted )
            {
				Container pack = from.Backpack;

				if ( targeted is ScrollClue )
				{
					ScrollClue scroll = (ScrollClue)targeted;

					int nCost = scroll.ScrollLevel * 100;

					if ( BaseVendor.BeggingPose(from) > 0 ) // LET US SEE IF THEY ARE BEGGING
					{
						nCost = nCost - (int)( ( from.Skills[SkillName.Begging].Value * 0.005 ) * nCost );
					}

					int toConsume = nCost;

					if ( scroll.ScrollIntelligence > 0 )
					{
						m_GypsyLady.SayTo(from, "Esse pergaminho ainda não foi decifrado.");
					}
					else if (pack.ConsumeTotal(typeof(Gold), toConsume))
					{
						string WillSay = "";

						switch ( Utility.RandomMinMax( 0, 3 ) ) 
						{
							case 0: WillSay = "Os espíritos me dizem que este pergaminho está"; break;
							case 1: WillSay = "Minha mente está me mostrando que este pergaminho está"; break;
							case 2: WillSay = "As vozes todas dizem que este pergaminho está"; break;
							case 3: WillSay = "Eu posso ver além que este pergaminho está"; break;
						}

						if ( scroll.ScrollTrue == 1 )
						{
							m_GypsyLady.SayTo(from, WillSay + " escrito com a verdade.");
						}
						else
						{
							m_GypsyLady.SayTo(from, WillSay + " escrito falsamente.");
						}

						from.SendMessage(String.Format("Você paga {0} de ouro.", toConsume));
					}
					else
					{
						m_GypsyLady.SayTo(from, "Eu exijo {0} de ouro para minhas visões.", toConsume);
					}
				}
				///////////////////////////////////////////////////////////////////////////////////
				else if ( targeted is SearchPage )
				{
					SearchPage scroll = (SearchPage)targeted;

					int nCost = ( 100 - scroll.LegendPercent ) * 50;

					if ( BaseVendor.BeggingPose(from) > 0 ) // LET US SEE IF THEY ARE BEGGING
					{
						nCost = nCost - (int)( ( from.Skills[SkillName.Begging].Value * 0.005 ) * nCost );
					}

					int toConsume = nCost;

					if (pack.ConsumeTotal(typeof(Gold), toConsume))
					{
						string WillSay = "";

						switch ( Utility.RandomMinMax( 0, 3 ) ) 
						{
							case 0: WillSay = "Os espíritos me dizem que esta lenda "; break;
							case 1: WillSay = "Minha mente está me mostrando que esta lenda "; break;
							case 2: WillSay = "As vozes todas dizem que esta lenda "; break;
							case 3: WillSay = "Eu posso ver além que esta lenda "; break;
						}

						if ( scroll.LegendReal == 1 )
						{
							m_GypsyLady.SayTo(from, WillSay + " realmente aconteceu.");
						}
						else
						{
							m_GypsyLady.SayTo(from, WillSay + " nunca aconteceu.");
						}

						from.SendMessage(String.Format("Você paga {0} de ouro.", toConsume));
					}
					else
					{
						m_GypsyLady.SayTo(from, "Eu exijo {0} de ouro para minhas visões.", toConsume);
					}
				}
				///////////////////////////////////////////////////////////////////////////////////
				else if ( targeted is DynamicBook )
				{
					DynamicBook scroll = (DynamicBook)targeted;

					int nCost = (scroll.BookPower + 1) * 50;

					if ( BaseVendor.BeggingPose(from) > 0 ) // LET US SEE IF THEY ARE BEGGING
					{
						nCost = nCost - (int)( ( from.Skills[SkillName.Begging].Value * 0.005 ) * nCost );
					}

					int toConsume = nCost;

					if (pack.ConsumeTotal(typeof(Gold), toConsume))
					{
						string WillSay = "";

						switch ( Utility.RandomMinMax( 0, 3 ) ) 
						{
							case 0: WillSay = "Os espíritos me dizem que este livro "; break;
							case 1: WillSay = "Minha mente está me mostrando que este livro "; break;
							case 2: WillSay = "As vozes todas dizem que este livro "; break;
							case 3: WillSay = "Eu posso ver além que este livro "; break;
						}

						if ( scroll.BookTrue > 0 )
						{
							m_GypsyLady.SayTo(from, WillSay + " contém a verdade.");
						}
						else
						{
							m_GypsyLady.SayTo(from, WillSay + " contém falsidades.");
						}

						from.SendMessage(String.Format("Você paga {0} de ouro.", toConsume));
					}
					else
					{
						m_GypsyLady.SayTo(from, "Eu exijo {0} de ouro para minhas visões.", toConsume);
					}
				}
				///////////////////////////////////////////////////////////////////////////////////
				else if ( targeted is SomeRandomNote )
				{
					SomeRandomNote scroll = (SomeRandomNote)targeted;

					int nCost = 100;

					if ( BaseVendor.BeggingPose(from) > 0 ) // LET US SEE IF THEY ARE BEGGING
					{
						nCost = nCost - (int)( ( from.Skills[SkillName.Begging].Value * 0.005 ) * nCost );
					}

					int toConsume = nCost;

					if (pack.ConsumeTotal(typeof(Gold), toConsume))
					{
						string WillSay = "";

						switch ( Utility.RandomMinMax( 0, 3 ) ) 
						{
							case 0: WillSay = "Os espíritos me dizem que este pergaminho está"; break;
							case 1: WillSay = "Minha mente está me mostrando que este pergaminho está"; break;
							case 2: WillSay = "As vozes todas dizem que este pergaminho está"; break;
							case 3: WillSay = "Eu posso ver além que este pergaminho está"; break;
						}

						if ( scroll.ScrollTrue == 1 )
						{
							m_GypsyLady.SayTo(from, WillSay + " escrito com a verdade.");
						}
						else
						{
							m_GypsyLady.SayTo(from, WillSay + " escrito falsamente.");
						}

						from.SendMessage(String.Format("Você paga {0} de ouro.", toConsume));
					}
					else
					{
						m_GypsyLady.SayTo(from, "Eu exijo {0} de ouro para minhas visões.", toConsume);
					}
				}
				///////////////////////////////////////////////////////////////////////////////////
				else
				{
					m_GypsyLady.SayTo(from, "Isso não é um livro ou pergaminho.");
				}
            }
        }

		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		public override void AddCustomContextEntries( Mobile from, List<ContextMenuEntry> list )
		{
			if ( from.Alive )
			{
				list.Add( new TruthEntry( this, from ) );
			}

			base.AddCustomContextEntries( from, list );
		}

		public GypsyLady( Serial serial ) : base( serial )
		{
		}

		public override bool CanTeach { get { return true; } }

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
