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
using Server.Regions;
using Server.Commands;

namespace Server.Mobiles
{
    public class TownHerald : BasePerson
	{
		private DateTime m_NextTalk;
		public DateTime NextTalk{ get{ return m_NextTalk; } set{ m_NextTalk = value; } }

		public override bool ClickTitle{ get{ return false; } }

		[Constructable]
		public TownHerald() : base( )
		{
			NameHue = -1;

			InitStats( 100, 100, 25 );

			Title = "o pregoeiro da cidade";
			Hue = Utility.RandomSkinColor();

			AddItem( new FancyShirt( Utility.RandomBlueHue() ) );

			Item skirt;

			switch ( Utility.Random( 2 ) )
			{
				case 0: skirt = new Skirt(); break;
				default: case 1: skirt = new Kilt(); break;
			}

			skirt.Hue = Utility.RandomGreenHue();

			AddItem( skirt );

			AddItem( new FeatheredHat( Utility.RandomGreenHue() ) );

			Item boots;

			switch ( Utility.Random( 2 ) )
			{
				case 0: boots = new Boots(); break;
				default: case 1: boots = new ThighBoots(); break;
			}

			AddItem( boots );
			AddItem( new LightCitizen( true ) );

			Utility.AssignRandomHair( this );
		}

		public override void OnMovement( Mobile m, Point3D oldLocation )
		{
			Region reg = Region.Find( this.Location, this.Map );
			if ( DateTime.Now >= m_NextTalk && InRange( m, 10 ) && m is PlayerMobile )
			{
				if ( LoggingFunctions.LoggingEvents() == true )
				{
					if ( Utility.RandomMinMax(1,4) == 1 )
					{
						randomShout( this );
					}
					else
					{
						string sEvents = LoggingFunctions.LogShout();
						Say( sEvents );
					}
				}
				else
				{
					randomShout( this );
				}
				m_NextTalk = (DateTime.Now + TimeSpan.FromSeconds( Utility.RandomMinMax( 15, 30 ) ));
			}
		}

		public static string randomShout( Mobile m )
		{
			string shout = "";

			string greet = "Ouça, ouça! ";
				switch ( Utility.RandomMinMax( 0, 3 ) )
				{
					case 1: greet = "Todos ouçam! "; 			    break;
					case 2: greet = "Todos saudem e ouçam minhas palavras! "; break;
					case 3: greet = "Sua atenção, por favor! "; 		break;
				};
				if ( m == null ){ greet = ""; }

			string name = NameList.RandomName( "female" );
				if ( Utility.RandomBool() ){ name = NameList.RandomName( "male" ); }
				name = name + " " + TavernPatrons.GetTitle();

			string seen = "foi visto em"; 
			switch ( Utility.RandomMinMax( 0, 6 ) )
			{
				case 1: seen = "foi encontrado em"; 		break;
				case 2: seen = "foi avistado em"; 		break;
				case 3: seen = "foi encontrado perto de"; 	break;
				case 4: seen = "foi avistado perto de"; 	break;
				case 5: seen = "foi encontrado saindo de"; 	break;
				case 6: seen = "foi avistado saindo de"; 	break;
			}

			string city = RandomThings.GetRandomCity();	
				if ( Utility.RandomBool() ){ city = RandomThings.MadeUpCity(); }

			string dungeon = QuestCharacters.SomePlace( "tavern" );	
				if ( Utility.RandomBool() ){ dungeon = RandomThings.MadeUpDungeon(); }

			string place = dungeon;
				if ( Utility.RandomBool() ){ place = city; }

			string slain = "destruiu";
			switch( Utility.RandomMinMax( 0, 3 ) )
			{
				case 1: slain = "derrotou";		break;
				case 2: slain = "matou";		break;
				case 3: slain = "subjugou";	    break;
			}

			string died = "foi morto";
			switch( Utility.RandomMinMax( 0, 3 ) )
			{
				case 1: died = "foi abatido";	break;
				case 2: died = "foi superado";	break;
				case 3: died = "foi derrotado";	break;
			}

			string death = "foi morto";
			switch( Utility.RandomMinMax( 0, 3 ) )
			{
				case 1: death = "pereceu";			        break;
				case 2: death = "encontrou seu fim";	    break;
				case 3: death = "foi abatido";		        break;
			}

			string crime = "assassinato";
			switch( Utility.RandomMinMax( 0, 6 ) )
			{
				case 1: crime = "bruxaria";	break;
				case 2: crime = "furto";		break;
				case 3: crime = "heresia";		break;
				case 4: crime = "blasfêmia";	break;
				case 5: crime = "traição";		break;
				case 6: crime = "roubo";		break;
			}

			string item = "destruiu";	
			switch( Utility.RandomMinMax( 0, 4 ) )
			{
				case 1: item = "perdeu"; break;	
				case 2: item = "encontrou"; break;	
				case 3: item = "descobriu"; break;	
				case 4: item = "roubou"; break;	
			}

			switch ( Utility.RandomMinMax( 0, 5 ) )
			{
				case 0: shout = "" + greet + "" + name + " " + seen + " " + place + ""; break;
				case 1: shout = "" + greet + "" + name + " " + slain + " " + RandomThings.GetRandomMonsters() + ""; break;
				case 2: shout = "" + greet + "" + name + " " + died + " por " + RandomThings.GetRandomMonsters() + ""; break;
				case 3: shout = "" + greet + "" + name + " " + death + " em " + dungeon + ""; break;
				case 4: shout = "" + greet + "" + name + " é procurado por " + crime + " em " + city + ""; break;
				case 5: shout = "" + greet + "" + name + " " + item + " " + QuestCharacters.QuestItems( false ) + ""; 
					if ( Utility.RandomBool() ){ shout = "" + greet + "" + name + " " + item + " " + QuestCharacters.ArtyItems( false ) + ""; }
					break;
			}

			if ( m != null ){ shout = shout + "!"; m.Say( shout ); }

			return shout;
		}

        public TownHerald(Serial serial) : base(serial)
		{
		}

		public override void GetContextMenuEntries( Mobile from, List<ContextMenuEntry> list ) 
		{ 
			base.GetContextMenuEntries( from, list ); 
			list.Add( new TownHeraldEntry( from, this ) ); 
		} 

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 ); 
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
		
		public class TownHeraldEntry : ContextMenuEntry
		{
			private Mobile m_Mobile;
			private Mobile m_Giver;
			
			public TownHeraldEntry( Mobile from, Mobile giver ) : base( 6146, 3 )
			{
				m_Mobile = from;
				m_Giver = giver;
			}

			public override void OnClick()
			{
			    if( !( m_Mobile is PlayerMobile ) )
				return;
				
				PlayerMobile mobile = (PlayerMobile) m_Mobile;
				{
					if ( LoggingFunctions.LoggingEvents() == true )
					{
						if ( ! mobile.HasGump( typeof( LoggingGumpCrier ) ) )
						{
							mobile.SendGump(new LoggingGumpCrier( mobile, 1 ));
						}
					}
					else
					{
						m_Giver.Say("Good day to you, " + m_Mobile.Name + ".");
					}
				}
            }
        }
	}  
}