using Server;
using System;
using System.Collections;
using Server.Network;
using Server.Misc;
using Server.Mobiles;
using System.Globalization;

namespace Server.Items
{
	public class QuestTake : Item
	{
		public Map DeliverMap;
		public int DeliverX;
		public int DeliverY;

		[Constructable]
		public QuestTake() : base( 0x1A97 )
		{
			Weight = 1.0;
			Name = "Journal of " + RandomThings.GetRandomName() + " the ";
				if ( Utility.RandomBool() ){ Name = Name + RandomThings.GetBoyGirlJob( 0 ); }
				else { Name = Name + RandomThings.GetBoyGirlJob( 1 ); }

			ItemID = Utility.RandomList( 0x65CC, 0x65CD, 0x1A97, 0x1A98, 0x1AA3, 0x2205, 0x220F, 0x2219, 0x2223, 0x222D, 0x2255, 0x225C, 0x225D, 0x225E, 0x225F, 0x5688, 0x5689 );
				if ( ItemID == 0x1A97 || ItemID == 0x1A98 || ItemID == 0x1AA3 || ItemID == 0x5688 || ItemID == 0x5689 ){ Hue = Utility.RandomColor(0); }
		}

		public override bool OnDragLift( Mobile from )
		{
			OnDoubleClick( from );
			return false;
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( from is PlayerMobile )
			{
				bool HasTome = false;

				ArrayList targets = new ArrayList();
				foreach ( Item item in World.Items.Values )
				{
					if ( item is QuestTome )
					{
						if ( ((QuestTome)item).QuestTomeOwner == from )
						{
							targets.Add( item );
							HasTome = true;
						}
					}
				}
				for ( int i = 0; i < targets.Count; ++i )
				{
					Item item = ( Item )targets[ i ];
					item.ItemID = ItemID;
					item.Name = Name;
					item.Hue = Hue;
					from.AddToBackpack( item );
					from.SendMessage( "Você toma posse do livro!" );
					from.SendSound( 0x3D );
					this.Delete();
				}

				if ( !HasTome )
				{
					SetupBook( from );
					from.SendMessage( "Você toma posse do livro!" );
					from.SendSound( 0x3D );
					LoggingFunctions.LogGeneric( from, "has found a Book of Questing." );
					this.Delete();
				}
			}
		}

		public void SetupBook( Mobile from )
		{
			QuestTome tome = new QuestTome();

			tome.QuestTomeOwner = from;
			tome.ItemID = ItemID;
			tome.Hue = Hue;
			tome.Name = Name;

			NPCGood( from, tome );
			NPCEvil( from, tome );

			tome.QuestTomeCitizen = "";
			tome.QuestTomeGoals = 0;
			tome.QuestTomeDungeon = "";
			tome.QuestTomeLand = Land.None;
			tome.QuestTomeType = 0;

			switch ( Utility.RandomMinMax( 0, 3 ) )
			{
				case 0:	tome.GoalItem1 = Server.Misc.QuestCharacters.QuestItems( false );	break;
				case 1:	tome.GoalItem1 = Server.Misc.QuestCharacters.QuestItems( false );	break;
				case 2:	tome.GoalItem1 = RandomHerb();										break;
				case 3:	tome.GoalItem1 = RandomMagic();										break;
			}
			switch ( Utility.RandomMinMax( 0, 3 ) )
			{
				case 0:	tome.GoalItem2 = Server.Misc.QuestCharacters.QuestItems( false );	break;
				case 1:	tome.GoalItem2 = Server.Misc.QuestCharacters.QuestItems( false );	break;
				case 2:	tome.GoalItem2 = RandomHerb();										break;
				case 3:	tome.GoalItem2 = RandomMagic();										break;
			}
			switch ( Utility.RandomMinMax( 0, 3 ) )
			{
				case 0:	tome.GoalItem3 = Server.Misc.QuestCharacters.QuestItems( false );	break;
				case 1:	tome.GoalItem3 = Server.Misc.QuestCharacters.QuestItems( false );	break;
				case 2:	tome.GoalItem3 = RandomHerb();										break;
				case 3:	tome.GoalItem3 = RandomMagic();										break;
			}

			string heard = "ouviu";
			switch ( Utility.RandomMinMax( 0, 3 ) )
			{
				case 1:	heard = "contou";	break;
				case 2:	heard = "soube";	break;
				case 3:	heard = "compartilhou";	break;
			}

			string legend = "lendas";
			switch ( Utility.RandomMinMax( 0, 3 ) )
			{
				case 1:	legend = "fábulas";	break;
				case 2:	legend = "mitos";	break;
				case 3:	legend = "conhecimento";	break;
			}

			string hush = "sussurrou";
			switch ( Utility.RandomMinMax( 0, 3 ) )
			{
				case 1:	hush = "contou";		break;
				case 2:	hush = "cantou";		break;
				case 3:	hush = "falou";		break;
			}

			string inn = "tavernas";
			switch ( Utility.RandomMinMax( 0, 4 ) )
			{
				case 1:	inn = "acampamentos";	break;
				case 2:	inn = "cidades";		break;
				case 3:	inn = "vilas";		break;
				case 4:	inn = "estalagens";	break;
			}

			string takes = "apoderou-se";
			switch ( Utility.RandomMinMax( 0, 4 ) )
			{
				case 1:	takes = "roubou";		break;
				case 2:	takes = "tomou";		break;
				case 3:	takes = "manteve";		break;
				case 4:	takes = "guardou";		break;
			}

			tome.GoalItem4 = Server.Misc.QuestCharacters.QuestItems( false );

			MakeVillain( tome );

			tome.QuestTomeStoryGood = "Você encontrou o diário de DDDDD, onde receberam uma missão de " + tome.QuestTomeNPCGood + " para encontrar " + tome.GoalItem4 + " que é conhecido por estar " + takes + " por " + tome.VillainName + " " + tome.VillainTitle + ". " + tome.VillainName + " é " + tome.VillainCategory + " que tem sido " + heard + " em " + legend + " e " + hush + " sobre em " + inn + ". O objetivo de DDDDD era encontrar " + tome.GoalItem1 + ", " + tome.GoalItem2 + ", & " + tome.GoalItem3 + " para ajudá-los a derrotar " + tome.VillainName + " e então trazer " + tome.GoalItem4 + " de volta para " + tome.QuestTomeNPCGood + " antes que " + tome.QuestTomeNPCEvil + " possa usá-lo para seus planos nefastos.<br><br>Esta agora é sua missão e você terá que falar com outros para encontrar pistas sobre a localização das relíquias necessárias, bem como onde " + tome.VillainName + " habita. Uma vez que você derrote " + tome.VillainName + " e reclame " + tome.GoalItem4 + ", você pode dar este diário para " + tome.QuestTomeNPCGood + " em " + tome.QuestTomeWorldGood + " nas seguintes coordenadas:<br><br>" + tome.QuestTomeLocateGood + "";

			tome.QuestTomeStoryEvil = "Você encontrou o diário de DDDDD, onde receberam uma missão de " + tome.QuestTomeNPCEvil + " para encontrar " + tome.GoalItem4 + " que é conhecido por estar " + takes + " por " + tome.VillainName + " " + tome.VillainTitle + ". " + tome.VillainName + " é " + tome.VillainCategory + " que tem sido " + heard + " em " + legend + " e " + hush + " sobre em " + inn + ". O objetivo de DDDDD era encontrar " + tome.GoalItem1 + ", " + tome.GoalItem2 + ", & " + tome.GoalItem3 + " para ajudá-los a derrotar " + tome.VillainName + " e então trazer " + tome.GoalItem4 + " de volta para " + tome.QuestTomeNPCEvil + " antes que " + tome.QuestTomeNPCGood + " possa usá-lo para seus empreendimentos justos.<br><br>Esta agora é sua missão e você terá que falar com outros para encontrar pistas sobre a localização das relíquias necessárias, bem como onde " + tome.VillainName + " habita. Uma vez que você derrote " + tome.VillainName + " e reclame " + tome.GoalItem4 + ", você pode dar este diário para " + tome.QuestTomeNPCEvil + " em " + tome.QuestTomeWorldEvil + " nas seguintes coordenadas:<br><br>" + tome.QuestTomeLocateEvil + "";

			if ( Utility.RandomBool() ) // INVERTER A HISTÓRIA
			{
				tome.QuestTomeStoryGood = "Você encontrou o diário de DDDDD, onde receberam uma missão de " + tome.QuestTomeNPCEvil + " para encontrar " + tome.GoalItem4 + " que é conhecido por estar " + takes + " por " + tome.VillainName + " " + tome.VillainTitle + ". " + tome.VillainName + " é " + tome.VillainCategory + " que tem sido " + heard + " em " + legend + " e " + hush + " sobre em " + inn + ". O objetivo de DDDDD era encontrar " + tome.GoalItem1 + ", " + tome.GoalItem2 + ", & " + tome.GoalItem3 + " para ajudá-los a derrotar " + tome.VillainName + " e então trazer " + tome.GoalItem4 + " de volta para " + tome.QuestTomeNPCEvil + " antes que " + tome.QuestTomeNPCGood + " possa usá-lo para seus empreendimentos justos.<br><br>Agora é sua missão impedir que " + tome.QuestTomeNPCEvil + " obtenha " + tome.GoalItem4 + ". Para encontrá-lo para " + tome.QuestTomeNPCGood + ", você terá que falar com outros para encontrar pistas sobre a localização das relíquias necessárias, bem como onde " + tome.VillainName + " habita. Uma vez que você derrote " + tome.VillainName + " e reclame " + tome.GoalItem4 + ", você pode dar este diário para " + tome.QuestTomeNPCGood + " em " + tome.QuestTomeWorldGood + " nas seguintes coordenadas:<br><br>" + tome.QuestTomeLocateGood + "";

				tome.QuestTomeStoryEvil = "Você encontrou o diário de DDDDD, onde receberam uma missão de " + tome.QuestTomeNPCGood + " para encontrar " + tome.GoalItem4 + " que é conhecido por estar " + takes + " por " + tome.VillainName + " " + tome.VillainTitle + ". " + tome.VillainName + " é " + tome.VillainCategory + " que tem sido " + heard + " em " + legend + " e " + hush + " sobre em " + inn + ". O objetivo de DDDDD era encontrar " + tome.GoalItem1 + ", " + tome.GoalItem2 + ", & " + tome.GoalItem3 + " para ajudá-los a derrotar " + tome.VillainName + " e então trazer " + tome.GoalItem4 + " de volta para " + tome.QuestTomeNPCGood + " antes que " + tome.QuestTomeNPCEvil + " possa usá-lo para seus planos nefastos.<br><br>Agora é sua missão impedir que " + tome.QuestTomeNPCGood + " obtenha " + tome.GoalItem4 + ". Para encontrá-lo para " + tome.QuestTomeNPCEvil + ", você terá que falar com outros para encontrar pistas sobre a localização das relíquias necessárias, bem como onde " + tome.VillainName + " habita. Uma vez que você derrote " + tome.VillainName + " e reclame " + tome.GoalItem4 + ", você pode dar este diário para " + tome.QuestTomeNPCEvil + " em " + tome.QuestTomeWorldEvil + " nas seguintes coordenadas:<br><br>" + tome.QuestTomeLocateEvil + "";
			}

			from.AddToBackpack( tome );
		}

		public string RandomHerb()
		{
			string herb = "bottle of dragon tears";

			Item bottle = new DDRelicReagent();
			herb = bottle.Name;
			bottle.Delete();

			switch ( Utility.RandomMinMax( 0, 8 ) )
			{
				case 0:	herb = "mystical " + herb;		break;
				case 1:	herb = "magical " + herb;		break;
				case 2:	herb = "enchanted " + herb;		break;
				case 3:	herb = "cursed " + herb;		break;
				case 4:	herb = "ancient " + herb;		break;
				case 5:	herb = "tainted " + herb;		break;
				case 6:	herb = "charmed " + herb;		break;
				case 7:	herb = "tainted " + herb;		break;
				case 8:	herb = "ensorcelled " + herb;	break;
			}

			TextInfo cultInfo = new CultureInfo("en-US", false).TextInfo;
			herb = "" + cultInfo.ToTitleCase(herb) + "";

			return herb;
		}

		public string RandomMagic()
		{
			string spell = "Merlin's Scroll of Acidic Storm";

			Item scroll = new DDRelicScrolls();
			spell = scroll.Name;
			scroll.Delete();

			string inner = "Spell of ";

			switch ( Utility.RandomMinMax( 0, 5 ) )
			{
				case 0:	inner = "Spell of ";		break;
				case 1:	inner = "Scroll of ";		break;
				case 2:	inner = "Parchment of ";	break;
				case 3:	inner = "Wand of ";			break;
				case 4:	inner = "Staff of ";		break;
				case 5:	inner = "Book of ";			break;
			}

			if ( spell.Contains("Spell of ") ){ spell = spell.Replace("Spell of ", inner); }

			return spell;
		}

		public void NPCEvil( Mobile m, QuestTome book )
        {
			int c = 0;
			bool picked = false;

			while ( !picked )
			{
				ArrayList npcs = new ArrayList();
				foreach ( Mobile msg in World.Mobiles.Values )
				if ( msg is EpicCharacter && msg.Name != "the Great Earth Serpent" )
				{
					if ( ((EpicCharacter)msg).MyAlignment == "evil" )
					{
						npcs.Add( msg ); c++;
					}
				}

				int o = Utility.RandomMinMax( 0, c );

				for ( int i = 0; i < npcs.Count; ++i )
				{
					EpicCharacter dude = ( EpicCharacter )npcs[ i ];

					if ( i == o )
					{
						Point3D WhoLoc = new Point3D(dude.MyX, dude.MyY, 0);
						Map WhoMap = dude.MyWorld;

						DeliverMap = dude.MyWorld;
						DeliverX = WhoLoc.X;
						DeliverY = WhoLoc.Y;

						book.DeliverMap = dude.MyWorld;
						book.DeliverX = WhoLoc.X;
						book.DeliverY = WhoLoc.Y;

						string my_location = "";

						int xLong = 0, yLat = 0;
						int xMins = 0, yMins = 0;
						bool xEast = false, ySouth = false;

						if ( Sextant.Format( WhoLoc, WhoMap, ref xLong, ref yLat, ref xMins, ref yMins, ref xEast, ref ySouth ) )
						{
							my_location = String.Format( "{0}° {1}'{2}, {3}° {4}'{5}", yLat, yMins, ySouth ? "S" : "N", xLong, xMins, xEast ? "E" : "W" );
						}

						book.QuestTomeWorldEvil = Server.Lands.GetLand( WhoMap, WhoLoc, dude.MyX, dude.MyY );
						book.QuestTomeNPCEvil = dude.Name + " " + dude.Title;
						book.QuestTomeLocateEvil = my_location;
						picked = true;
					}
				}
			}
        }

		public void NPCGood( Mobile m, QuestTome book )
        {
			int c = 0;
			bool picked = false;

			while ( !picked )
			{
				ArrayList npcs = new ArrayList();
				foreach ( Mobile msg in World.Mobiles.Values )
				if ( msg is EpicCharacter && msg.Name != "the Great Earth Serpent" )
				{
					if ( ((EpicCharacter)msg).MyAlignment == "good" )
					{
						npcs.Add( msg ); c++;
					}
				}

				int o = Utility.RandomMinMax( 0, c );

				for ( int i = 0; i < npcs.Count; ++i )
				{
					EpicCharacter dude = ( EpicCharacter )npcs[ i ];

					if ( i == o )
					{
						Point3D WhoLoc = new Point3D(dude.MyX, dude.MyY, 0);
						Map WhoMap = dude.MyWorld;

						DeliverMap = dude.MyWorld;
						DeliverX = WhoLoc.X;
						DeliverY = WhoLoc.Y;

						book.DeliverMap = dude.MyWorld;
						book.DeliverX = WhoLoc.X;
						book.DeliverY = WhoLoc.Y;

						string my_location = "";

						int xLong = 0, yLat = 0;
						int xMins = 0, yMins = 0;
						bool xEast = false, ySouth = false;

						if ( Sextant.Format( WhoLoc, WhoMap, ref xLong, ref yLat, ref xMins, ref yMins, ref xEast, ref ySouth ) )
						{
							my_location = String.Format( "{0}° {1}'{2}, {3}° {4}'{5}", yLat, yMins, ySouth ? "S" : "N", xLong, xMins, xEast ? "E" : "W" );
						}

						book.QuestTomeWorldGood = Server.Lands.GetLand( WhoMap, WhoLoc, dude.MyX, dude.MyY );
						book.QuestTomeNPCGood = dude.Name + " " + dude.Title;
						book.QuestTomeLocateGood = my_location;
						picked = true;
					}
				}
			}
        }

		public void MakeVillain( QuestTome book )
        {
			book.VillainHue = 0;
			book.VillainBody = 0;
			bool color = false;

			int enemy = Utility.RandomMinMax( 1, 10 );

			if ( enemy == 1 )
			{
				switch ( Utility.RandomMinMax( 0, 3 ) )
				{
					case 0:	book.VillainType = "ArchFiend";			book.VillainName = NameList.RandomName( "daemon" );		book.VillainBody = Utility.RandomList( 9, 320 );	color = true;	book.VillainCategory = "a daemon";		break;
					case 1:	book.VillainType = "ArchFiend";			book.VillainName = NameList.RandomName( "demonic" );	book.VillainBody = Utility.RandomList( 191, 427 );	color = true;
						switch ( Utility.RandomMinMax( 0, 2 ) )
						{
							case 0:	book.VillainCategory = "a balron"; break;
							case 1:	book.VillainCategory = "a balor";  break;
							case 2:	book.VillainCategory = "a balrog"; break;
						}
					break;
					case 2:	book.VillainType = "ArchFiend";			book.VillainName = NameList.RandomName( "devil" );		book.VillainBody = Utility.RandomList( 765, 804, 436, 88, 138 );	book.VillainCategory = "a devil";		break;
					case 3:	book.VillainType = "Succubus";			book.VillainName = NameList.RandomName( "goddess" );	book.VillainBody = 174;	color = true;
						switch ( Utility.RandomMinMax( 0, 2 ) )
						{
							case 0:	book.VillainCategory = "a succubus"; break;
							case 1:	book.VillainCategory = "a demoness"; break;
							case 2:	book.VillainCategory = "a daemoness"; break;
						}
					break;
				}
			}
			else if ( enemy == 2 )
			{
				book.VillainCategory = "a giant";
				book.VillainName = NameList.RandomName( "giant" );

				switch ( Utility.RandomMinMax( 0, 18 ) )
				{
					case 0:		book.VillainType = "AbyssGiant";		break;
					case 1:		book.VillainType = "CloudGiant";		break;
					case 2:		book.VillainType = "FireGiant";			break;
					case 3:		book.VillainType = "ForestGiant";		break;
					case 4:		book.VillainType = "FrostGiant";		break;
					case 5:		book.VillainType = "HillGiant";			break;
					case 6:		book.VillainType = "HillGiantShaman";	break;
					case 7:		book.VillainType = "IceGiant";			break;
					case 8:		book.VillainType = "JungleGiant";		break;
					case 9:		book.VillainType = "LavaGiant";			break;
					case 10:	book.VillainType = "MountainGiant";		break;
					case 11:	book.VillainType = "SandGiant";			break;
					case 12:	book.VillainType = "StarGiant";			break;
					case 13:	book.VillainType = "StoneGiant";		break;
					case 14:	book.VillainType = "StormGiant";		break;
					case 15:	book.VillainType = "AncientCyclops";	book.VillainName = NameList.RandomName( "greek" );		book.VillainCategory = "a cyclops";	break;
					case 16:	book.VillainType = "AncientEttin";		book.VillainCategory = "an ettin";	break;
					case 17:	book.VillainType = "OgreLord";			book.VillainCategory = "an ogre";	break;
					case 18:	book.VillainType = "Giant";				break;
				}
			}
			else if ( enemy == 3 )
			{
				book.VillainCategory = "a dragon";
				book.VillainName = NameList.RandomName( "dragon" );
				switch ( Utility.RandomMinMax( 0, 24 ) )
				{
					case 0:		book.VillainType = "Dragon";					book.VillainBody = Utility.RandomList( 12, 59 );	color = true;	break;
					case 1:		book.VillainType = "AncientWyrm";				break;
					case 2:		book.VillainType = "ShadowWyrm";				break;
					case 3:		book.VillainType = "VolcanicDragon";			break;
					case 4:		book.VillainType = "VoidDragon";				break;
					case 5:		book.VillainType = "AshDragon";					break;
					case 6:		book.VillainType = "CrystalDragon";				break;
					case 7:		book.VillainType = "ElderDragon";				break;
					case 8:		book.VillainType = "PrimevalAmberDragon";		break;
					case 9:		book.VillainType = "VolcanicDragon";			break;
					case 10:	book.VillainType = "PrimevalBlackDragon";		break;
					case 11:	book.VillainType = "PrimevalDragon";			break;
					case 12:	book.VillainType = "PrimevalFireDragon";		break;
					case 13:	book.VillainType = "PrimevalGreenDragon";		break;
					case 14:	book.VillainType = "PrimevalNightDragon";		break;
					case 15:	book.VillainType = "PrimevalRedDragon";			break;
					case 16:	book.VillainType = "PrimevalRoyalDragon";		break;
					case 17:	book.VillainType = "PrimevalRunicDragon";		break;
					case 18:	book.VillainType = "PrimevalSilverDragon";		break;
					case 19:	book.VillainType = "PrimevalStygianDragon";		break;
					case 20:	book.VillainType = "PrimevalVolcanicDragon";	break;
					case 21:	book.VillainType = "VampiricDragon";			break;
					case 22:	book.VillainType = "PrimevalAbysmalDragon";		break;
					case 23:	book.VillainType = "AncientDrake";				book.VillainCategory = "a drake";	break;
					case 24:	book.VillainType = "AncientWyvern";				book.VillainCategory = "a wyvern";	break;
				}
			}
			else if ( enemy == 4 )
			{
				book.VillainCategory = "a beholder";
				Mobile m = new Beholder(); 
				book.VillainType = "Beholder";
				book.VillainName = m.Name;
				book.VillainHue = m.Hue;
				m.Delete();
			}
			else if ( enemy == 5 )
			{
				if ( Utility.RandomBool() )
				{
					book.VillainCategory = "a gargoyle";
					book.VillainType = "StygianGargoyleLord";
					book.VillainName = NameList.RandomName( "gargoyle name" );
				}
				else
				{
					book.VillainCategory = "a sphinx";
					book.VillainType = "AncientSphinx";
					book.VillainName = NameList.RandomName( "drakkul" );
				}
			}
			else if ( enemy == 6 )
			{
				if ( Utility.RandomBool() )
				{
					book.VillainCategory = "a reptilian humanoid";
					Mobile m = new Sleestax(); 
					book.VillainType = "Sleestax";
					book.VillainName = m.Name;
					book.VillainHue = m.Hue;
					m.Delete();
				}
				else
				{
					book.VillainCategory = "a serpentoid";
					book.VillainType = "OphidianKnight";
					book.VillainName = NameList.RandomName( "lizardman" );
					book.VillainBody = 306;
				}
			}
			else if ( enemy == 7 )
			{
				if ( Utility.RandomBool() )
				{
					book.VillainCategory = "an arachnid";
					book.VillainType = "AbyssCrawler";
					book.VillainName = NameList.RandomName( "goblin" );
					book.VillainBody = 173;
				}
				else
				{
					book.VillainCategory = "an insectoid";
					book.VillainType = "AntaurKing";
					book.VillainName = NameList.RandomName( "goblin" );
					book.VillainBody = 784;
				}
			}
			else if ( enemy == 8 )
			{
				if ( Utility.RandomBool() )
				{
					book.VillainCategory = "a reaper";
					Mobile m = new EvilEnt(); 
					book.VillainType = "EvilEnt";
					book.VillainName = m.Name;
					book.VillainHue = m.Hue;
					m.Delete();
				}
				else
				{
					book.VillainCategory = "an elemental";
					book.VillainType = "CrystalGoliath";
					book.VillainName = NameList.RandomName( "urk" );
					book.VillainBody = 753;
				}
			}
			else if ( enemy == 9 )
			{
				switch ( Utility.RandomMinMax( 0, 4 ) )
				{
					case 0:		book.VillainType = "Dracolich";					book.VillainName = NameList.RandomName( "dragon" );			book.VillainBody = Utility.RandomList( 104, 323 );		book.VillainCategory = "a dracolich";		break;
					case 1:		book.VillainType = "AncientLich";				book.VillainName = NameList.RandomName( "ancient lich" );	book.VillainCategory = "a lich";			break;
					case 2:		book.VillainType = "AncientFleshGolem";			book.VillainName = NameList.RandomName( "greek" );			book.VillainBody = 999;									book.VillainCategory = "a flesh golem";		break;
					case 3:		book.VillainType = "GrundulVarg";				book.VillainName = NameList.RandomName( "ancient lich" );	book.VillainBody = Utility.RandomList( 768, 65, 107 );	book.VillainCategory = "a dread lord";		break;
					case 4:		book.VillainType = "GrundulVarg";				book.VillainName = NameList.RandomName( "greek" );			book.VillainBody = Utility.RandomList( 768, 65, 107 );	book.VillainCategory = "a death knight";	break;
				}
			}
			else
			{
				switch ( Utility.RandomMinMax( 1, 3 ) )
				{
					case 1:		book.VillainType = "Cerberus";					book.VillainName = NameList.RandomName( "greek" );			book.VillainCategory = "a cerberus";		break;
					case 2:		book.VillainType = "Styguana";					book.VillainName = NameList.RandomName( "lizardman" );		book.VillainCategory = "a styguana";		break;
					case 3:		book.VillainType = "HellBeast";					book.VillainName = NameList.RandomName( "imp" );			book.VillainCategory = "a hell beast";		break;
				}
			}

			book.VillainHue = 0; if ( color ){ book.VillainHue = Utility.RandomColor( 0 ); }

			book.VillainTitle = RandomThings.RandomEvilTitle();
		}

		public static void DropChest( Mobile m )
		{
			if ( m.Fame == 0 && m.Karma == 0 && m.Title != "" && ((BaseCreature)m).Home.X == 0 && ((BaseCreature)m).Home.Y == 0 )
			{
				bool BookExists = false;
				QuestTome book = null;

				foreach ( Item item in World.Items.Values )
				{
					if ( item is QuestTome )
					{
						if ( ((QuestTome)item).VillainName == m.Name && ((QuestTome)item).VillainTitle == m.Title )
						{
							BookExists = true;
							book = ((QuestTome)item);
						}
					}
				}

				if ( BookExists )
				{
					MajorItemOnCorpse majorChest = new MajorItemOnCorpse();
					majorChest.Name = "Chest of " + m.Name;
					majorChest.VillainName = m.Name;
					majorChest.VillainTitle = m.Title;
					majorChest.MoveToWorld( m.Location, m.Map );
					Server.Misc.IntelligentAction.BurnAway( m );
					book.QuestTomeCitizen = "";
					book.QuestTomeDungeon = "";
					book.QuestTomeLand = Land.None;
					book.QuestTomeType = 0;
					if ( m.Corpse != null ){ m.Corpse.Delete(); }
				}
			}
		}

		public QuestTake( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( ( int)1 ); // version
			writer.Write( DeliverMap );
			writer.Write( DeliverX );
			writer.Write( DeliverY );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();

			switch ( version )
			{
				case 1:
				{
					DeliverMap = reader.ReadMap();
					DeliverX = reader.ReadInt();
					DeliverY = reader.ReadInt();
					break;
				}
			}
		}
	}

	public class MajorItemOnCorpse : Item
	{
		public string VillainName;
		[CommandProperty( AccessLevel.GameMaster )]
		public string Villain_Name { get{ return VillainName; } set{ VillainName = value; } }

		public string VillainTitle;
		[CommandProperty( AccessLevel.GameMaster )]
		public string Villain_Title { get{ return VillainTitle; } set{ VillainTitle = value; } }

		[Constructable]
		public MajorItemOnCorpse() : base( 0x0E40 )
		{
			ItemRemovalTimer thisTimer = new ItemRemovalTimer( this ); 
			thisTimer.Start();
			ItemID = Utility.RandomList( 0x0E40, 0x0E41 );
			ResourceMods.SetRandomResource( false, false, this, CraftResource.Iron, false, null );
			Hue = CraftResources.GetHue(Resource);
			Name = "chest";
		}

		public override bool OnDragLift( Mobile from )
		{
			OnDoubleClick( from );
			return false;
		}

		public MajorItemOnCorpse( Serial serial ) : base( serial )
		{
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( QuestTome.FoundItem( from, 1, this ) )
			{
				this.Delete();
			}
			else if ( from.InRange( this.GetWorldLocation(), 5 ) )
			{
				from.SendMessage( "O baú parece estar vazio." );
			}
			else
			{
				from.SendLocalizedMessage( 502138 ); // That is too far away for you to use
			}
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
			this.Delete(); // none when the world starts 
		}

		public class ItemRemovalTimer : Timer 
		{ 
			private Item i_item; 
			public ItemRemovalTimer( Item item ) : base( TimeSpan.FromMinutes( 20.0 ) ) 
			{ 
				Priority = TimerPriority.OneSecond; 
				i_item = item; 
			} 

			protected override void OnTick() 
			{ 
				if (( i_item != null ) && ( !i_item.Deleted ))
				{
					i_item.Delete();
				}
			} 
		} 
	}
}
