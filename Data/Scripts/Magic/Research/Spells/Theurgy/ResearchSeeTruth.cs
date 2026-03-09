using System;
using Server.Targeting;
using Server.Network;
using Server.Items;

namespace Server.Spells.Research
{
	public class ResearchSeeTruth : ResearchSpell
	{
		public override int spellIndex { get { return 7; } }
		public int CirclePower = 1;
		public static int spellID = 7;
		public override TimeSpan CastDelayBase { get { return TimeSpan.FromSeconds( 1.0 ); } }
		public override double RequiredSkill{ get{ return (double)(Int32.Parse( Server.Misc.Research.SpellInformation( spellIndex, 8 ))); } }
		public override int RequiredMana{ get{ return Int32.Parse( Server.Misc.Research.SpellInformation( spellIndex, 7 )); } }

		private static SpellInfo m_Info = new SpellInfo(
				Server.Misc.Research.SpellInformation( spellID, 2 ),
				Server.Misc.Research.CapsCast( Server.Misc.Research.SpellInformation( spellID, 4 ) ),
				215,
				9001,
				Reagent.GraveDust,Reagent.SeaSalt,Reagent.FairyEgg
			);

		public ResearchSeeTruth( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
		{
		}

		public override void OnCast()
		{
			if ( CheckSequence() )
			{
				Caster.Target = new InternalTarget( this, spellID, Scroll, alwaysConsume );
			}

			FinishSequence();
		}

		private class InternalTarget : Target
		{
			private ResearchSeeTruth m_Owner;
			private int m_SpellIndex;
			private Item m_fromBook;
			private bool m_alwaysConsume;

			public InternalTarget( ResearchSeeTruth owner, int spellIndex, Item fromBook, bool alwaysConsume ) : base( Core.ML ? 10 : 12, false, TargetFlags.None )
			{
				m_Owner = owner;
				m_fromBook = fromBook;
				m_SpellIndex = spellIndex;
				m_alwaysConsume = alwaysConsume;
			}

			protected override void OnTarget( Mobile from, object targeted )
			{
				bool consume = false;

				if ( targeted is ScrollClue )
				{
					ScrollClue scroll = (ScrollClue)targeted;
					consume = true;
					from.PlaySound( 0xF9 );

					if ( scroll.ScrollIntelligence > 0 )
					{
						from.SendMessage("Esse pergaminho ainda não foi decifrado.");
					}
					else
					{
						string WillSay = "";

						switch ( Utility.RandomMinMax( 0, 3 ) ) 
						{
							case 0: WillSay = "Os espíritos lhe dizem que este pergaminho está"; break;
							case 1: WillSay = "Sua mente está mostrando que este pergaminho está"; break;
							case 2: WillSay = "As vozes todas falam que este pergaminho está"; break;
							case 3: WillSay = "Você pode ver além que este pergaminho está"; break;
						}

						if ( scroll.ScrollTrue == 1 )
						{
							from.SendMessage(WillSay + " escrito com a verdade.");
						}
						else
						{
							from.SendMessage(WillSay + " escrito falsamente.");
						}
					}
				}
				///////////////////////////////////////////////////////////////////////////////////
				else if ( targeted is SearchPage )
				{
					SearchPage scroll = (SearchPage)targeted;
					consume = true;
					from.PlaySound( 0xF9 );

					string WillSay = "";

					switch ( Utility.RandomMinMax( 0, 3 ) ) 
					{
						case 0: WillSay = "Os espíritos lhe dizem que esta lenda "; break;
						case 1: WillSay = "Sua mente está mostrando que esta lenda "; break;
						case 2: WillSay = "As vozes todas falam que esta lenda "; break;
						case 3: WillSay = "Você pode ver além que esta lenda "; break;
					}

					if ( scroll.LegendReal == 1 )
					{
						from.SendMessage(WillSay + " realmente aconteceu.");
					}
					else
					{
						from.SendMessage(WillSay + " nunca aconteceu.");
					}
				}
				///////////////////////////////////////////////////////////////////////////////////
				else if ( targeted is DynamicBook )
				{
					DynamicBook scroll = (DynamicBook)targeted;
					consume = true;
					from.PlaySound( 0xF9 );

					string WillSay = "";

					switch ( Utility.RandomMinMax( 0, 3 ) ) 
					{
						case 0: WillSay = "Os espíritos lhe dizem que este livro "; break;
						case 1: WillSay = "Sua mente está mostrando que este livro "; break;
						case 2: WillSay = "As vozes todas falam que este livro "; break;
						case 3: WillSay = "Você pode ver além que este livro "; break;
					}

					if ( scroll.BookTrue > 0 )
					{
						from.SendMessage(WillSay + " contém a verdade.");
					}
					else
					{
						from.SendMessage(WillSay + " contém falsidades.");
					}
				}
				///////////////////////////////////////////////////////////////////////////////////
				else if ( targeted is SomeRandomNote )
				{
					SomeRandomNote scroll = (SomeRandomNote)targeted;
					consume = true;
					from.PlaySound( 0xF9 );

					string WillSay = "";

					switch ( Utility.RandomMinMax( 0, 3 ) ) 
					{
						case 0: WillSay = "Os espíritos lhe dizem que este pergaminho está"; break;
						case 1: WillSay = "Sua mente está mostrando que este pergaminho está"; break;
						case 2: WillSay = "As vozes todas falam que este pergaminho está"; break;
						case 3: WillSay = "Você pode ver além que este pergaminho está"; break;
					}

					if ( scroll.ScrollTrue == 1 )
					{
						from.SendMessage(WillSay + " escrito com a verdade.");
					}
					else
					{
						from.SendMessage(WillSay + " escrito falsamente.");
					}
				}
				
				///////////////////////////////////////////////////////////////////////////////////
				else
				{
					from.SendMessage("Isso não é um livro ou pergaminho.");
				}

				if ( consume ){ Server.Misc.Research.ConsumeScroll( from, true, m_SpellIndex, m_alwaysConsume, m_fromBook ); }
				m_Owner.FinishSequence();
			}

			protected override void OnTargetFinish( Mobile from )
			{
				m_Owner.FinishSequence();
			}
		}
	}
}
