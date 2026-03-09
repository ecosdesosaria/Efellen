using System;
using Server;
using System.Collections;
using System.Collections.Generic;
using Server.Misc;
using Server.Items;
using Server.Network;
using Server.Commands;
using Server.Commands.Generic;
using Server.Mobiles;
using Server.Accounting;
using Server.Regions;
using System.Text;

namespace Server.Misc
{
    class QuestCharacters
    {
		public static string ParchmentWriter()
		{
			return RandomThings.GetRandomName();
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		public static string QuestGiverKarma( bool evil )
		{
			string sWho = NameList.RandomName( "male" );
			if ( Utility.RandomMinMax( 1, 2 ) == 1 ) 
			{ 
				sWho = NameList.RandomName( "female" );
			}

			string sTitle = TavernPatrons.GetTitle();

			if ( evil == true )
			{
				sTitle = TavernPatrons.GetEvilTitle();
			}

			return sWho + " " + sTitle;
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		public static string QuestGiver()
		{
			string sWho = NameList.RandomName( "male" );
			if ( Utility.RandomMinMax( 1, 2 ) == 1 ) 
			{ 
				sWho = NameList.RandomName( "female" );
			}

			string sTitle = TavernPatrons.GetTitle();

			return sWho + " " + sTitle;
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		public static string RandomWords()
		{
			string word = "";
			switch ( Utility.Random( 34 ) )
			{
				case 0: word = NameList.RandomName( "male" ); break;
				case 1: word = NameList.RandomName( "female" ); break;
				case 2: word = NameList.RandomName( "henchman_male" ); break;
				case 3: word = NameList.RandomName( "henchman_female" ); break;
				case 4: word = NameList.RandomName( "dark_elf_prefix_female" ); word = word + NameList.RandomName( "dark_elf_suffix_female" ); break;
				case 5: word = NameList.RandomName( "dark_elf_prefix_male" ); word = word + NameList.RandomName( "dark_elf_suffix_male" ); break;
				case 6: word = NameList.RandomName( "imp" ); break;
				case 7: word = NameList.RandomName( "druid" ); break;
				case 8: word = NameList.RandomName( "ork" ); break;
				case 9: word = NameList.RandomName( "dragon" ); break;
				case 10: word = NameList.RandomName( "goddess" ); break;
				case 11: word = NameList.RandomName( "demonic" ); break;
				case 12: word = NameList.RandomName( "ork_male" ); break;
				case 13: word = NameList.RandomName( "ork_female" ); break;
				case 14: word = NameList.RandomName( "barb_female" ); break;
				case 15: word = NameList.RandomName( "barb_male" ); break;
				case 16: word = NameList.RandomName( "tokuno male" ); break;
				case 17: word = NameList.RandomName( "tokuno female" ); break;
				case 18: word = NameList.RandomName( "elf_female" ); break;
				case 19: word = NameList.RandomName( "elf_male" ); break;
				case 20: word = NameList.RandomName( "ancient lich" ); break;
				case 21: word = NameList.RandomName( "gargoyle vendor" ); break;
				case 22: word = NameList.RandomName( "gargoyle name" ); break;
				case 23: word = NameList.RandomName( "centaur" ); break;
				case 24: word = NameList.RandomName( "ethereal warrior" ); break;
				case 25: word = NameList.RandomName( "pixie" ); break;
				case 26: word = NameList.RandomName( "savage" ); break;
				case 27: word = NameList.RandomName( "savage rider" ); break;
				case 28: word = NameList.RandomName( "savage shaman" ); break;
				case 29: word = NameList.RandomName( "golem controller" ); break;
				case 30: word = NameList.RandomName( "daemon" ); break;
				case 31: word = NameList.RandomName( "devil" ); break;
				case 32: word = NameList.RandomName( "evil mage" ); break;
				case 33: word = NameList.RandomName( "evil witch" ); break;
			}

			if ( word == "" || word == " " ){ word = NameList.RandomName( "evil witch" ); }

			return word;
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		public static string SomePlace( string FromWho )
		{
			string sPlace = "the Ancient Prison";
			int section = Utility.RandomMinMax( 1, 104 );
			if ( FromWho != "tavern" ){ section = Utility.RandomMinMax( 17, 104 ); }
			switch( section )
			{
				case 1: sPlace = "the cave"; break;
				case 2: sPlace = "the castle"; break;
				case 3: sPlace = "the tower"; break;
				case 4: sPlace = "the ruins"; break;
				case 5: sPlace = "the dungeon"; break;
				case 6: sPlace = "the labyrinth"; break;
				case 7: sPlace = "the tunnels"; break;
				case 8: sPlace = "the maze"; break;
				case 9: sPlace = "the forest"; break;
				case 10: sPlace = "the swamp"; break;
				case 11: sPlace = "the desert"; break;
				case 12: sPlace = "the jungle"; break;
				case 13: sPlace = "the tomb"; break;
				case 14: sPlace = "the crypt"; break;
				case 15: sPlace = "the cemetery"; break;
				case 16: sPlace = "the graveyard"; break;

				case 17: sPlace = "the City of the Dead"; break;
				case 18: sPlace = "the Mausoleum"; break;
				case 19: sPlace = "the Valley of Dark Druids"; break;
				case 20: sPlace = "Vordo's Castle"; break;
				case 21: sPlace = "Vordo's Dungeon"; break;
				case 22: sPlace = "the Crypts of Kuldar"; break;
				case 23: sPlace = "the Kuldara Sewers"; break;
				case 24: sPlace = "the Ancient Pyramid"; break;
				case 25: sPlace = "Dungeon Exodus"; break;
				case 26: sPlace = "the Cave of Banished Mages"; break;
				case 27: sPlace = "Library of Bal Tsareth"; break;
				case 28: sPlace = "Dardin's Pit"; break;
				case 29: sPlace = "Dungeon Doom"; break;
				case 30: sPlace = "the Fires of Hell"; break;
				case 31: sPlace = "the Mines of Morinia"; break;
				case 32: sPlace = "the Perinian Depths"; break;
				case 33: sPlace = "the Dungeon of Time Awaits"; break;
				case 34: sPlace = "the Pirate Cave"; break;
				case 35: sPlace = "the Dragon's Maw"; break;
				case 36: sPlace = "the Cave of the Zuluu"; break;
				case 37: sPlace = "the Ratmen Lair"; break;
				case 38: sPlace = "the Caverns of Poseidon"; break;
				case 39: sPlace = "the Tower of Brass"; break;
				case 40: sPlace = "the Forgotten Halls"; break;

				case 41: sPlace = "the Vault of the Black Knight"; break;
				case 42: sPlace = "the Undersea Pass"; break;
				case 43: sPlace = "the Castle of Dracula"; break;
				case 44: sPlace = "the Crypts of Dracula"; break;
				case 45: sPlace = "the Lodoria Catacombs"; break;
				case 46: sPlace = "Dungeon Covetous"; break;
				case 47: sPlace = "Dungeon Deceit"; break;
				case 48: sPlace = "Dungeon Despise"; break;
				case 49: sPlace = "Dungeon Destard"; break;
				case 50: sPlace = "the City of Embers"; break;
				case 51: sPlace = "Dungeon Hythloth"; break;
				case 52: sPlace = "the Frozen Hells"; break;
				case 53: sPlace = "the Ice Fiend Lair"; break;
				case 54: sPlace = "the Halls of Undermountain"; break;
				case 55: sPlace = "Dungeon Shame"; break;
				case 56: sPlace = "Terathan Keep"; break;
				case 57: sPlace = "the Volcanic Cave"; break;
				case 58: sPlace = "Dungeon Wrong"; break;
				case 59: sPlace = "Stonegate Castle"; break;
				case 60: sPlace = "the Ancient Elven Mine"; break;

				case 61: sPlace = "the Dungeon of the Mad Archmage"; break;
				case 62: sPlace = "the Dungeon of the Lich King"; break;
				case 63: sPlace = "the Halls of Ogrimar"; break;
				case 64: sPlace = "the Ratmen Mines"; break;
				case 65: sPlace = "Dungeon Rock"; break;
				case 66: sPlace = "the Storm Giant Lair"; break;
				case 67: sPlace = "the Corrupt Pass"; break;
				case 68: sPlace = "the Tombs"; break;
				case 69: sPlace = "the Undersea Castle"; break;
				case 70: sPlace = "the Azure Castle"; break;
				case 71: sPlace = "the Tomb of Kazibal"; break;
				case 72: sPlace = "the Catacombs of Azerok"; break;

				case 73: sPlace = "the Ancient Prison"; break;
				case 74: sPlace = "the Cave of Fire"; break;
				case 75: sPlace = "the Cave of Souls"; break;
				case 76: sPlace = "Dungeon Ankh"; break;
				case 77: sPlace = "Dungeon Bane"; break;
				case 78: sPlace = "Dungeon Hate"; break;
				case 79: sPlace = "Dungeon Scorn"; break;
				case 80: sPlace = "Dungeon Torment"; break;
				case 81: sPlace = "Dungeon Vile"; break;
				case 82: sPlace = "Dungeon Wicked"; break;
				case 83: sPlace = "Dungeon Wrath"; break;
				case 84: sPlace = "the Flooded Temple"; break;
				case 85: sPlace = "the Gargoyle Crypts"; break;
				case 86: sPlace = "the Serpent Sanctum"; break;
				case 87: sPlace = "the Tomb of the Fallen Wizard"; break;

				case 88: sPlace = "the Blood Temple"; break;
				case 89: sPlace = "the Ice Queen Fortress"; break;
				case 90: sPlace = "the Scurvy Reef"; break;
				case 91: sPlace = "the Glacial Scar"; break;
				case 92: sPlace = "the Temple of Osirus"; break;
				case 93: sPlace = "the Sanctum of Saltmarsh"; break;

				case 94: sPlace = "Morgaelin's Inferno"; break;
				case 95: sPlace = "the Zealan Tombs"; break;
				case 96: sPlace = "Argentrock Castle"; break;
				case 97: sPlace = "the Daemon's Crag"; break;
				case 98: sPlace = "the Stygian Abyss"; break;
				case 99: sPlace = "the Hall of the Mountain King"; break;
				case 100: sPlace = "the Depths of Carthax Lake"; break;
				case 101: sPlace = "the Ancient Sky Ship"; break;
				case 102: sPlace = "the Hive of the Eye Tyrant"; break;
				case 103: sPlace = "Cave of the Myconid"; break;
				case 104: sPlace = "the Sunless Citadel"; break;
			}

			return sPlace;
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		// string sWord3 = QuestItems( true );

		public static string EpicQuestStory( string QuestItem, string alignment )
		{
			string[] vWord1 = new string[] {"Preciso que você", "Quero que você", "Necessito que você", "Preciso de alguém para", "Quero alguém para", "Necessito de alguém para", "Necessitamos que você", "Precisamos de alguém para", "Queremos alguém para", "Necessitamos de alguém para"};
				string sWord1 = vWord1[Utility.RandomMinMax( 0, (vWord1.Length-1) )];

			string[] vWord2 = new string[] {"vá encontrar", "procure por", "busque", "me traga", "me encontre", "pegue", "encontre", "procure", "descubra", "localize", "desenterre", "procure por"};
				string sWord2 = vWord2[Utility.RandomMinMax( 0, (vWord2.Length-1) )];

				string sWord3 = QuestItem;

				string sWord4 = RandomThings.GetRandomName();
					if ( Utility.RandomMinMax( 1, 4 ) == 1 ){ sWord4 = RandomThings.GetRandomSociety(); }

				string sWord5 = RandomThings.GetRandomJobTitle(0);

			string sWord6 = "have it destroyed";
				int nWord6 = Utility.RandomMinMax( 1, 16 );
				switch( nWord6 )
				{
					case 1: sWord6 = "destruí-lo"; 			break;
					case 2: sWord6 = "entregar para " + sWord4; 		break;
					case 3: sWord6 = "entregar para " + sWord5; 		break;
					case 4: sWord6 = "devolver para " + sWord4; 		break;
					case 5: sWord6 = "devolver para " + sWord5;		break;
					case 6: sWord6 = "escondê-lo"; 					break;
					case 7: sWord6 = "destruí-lo"; 					break;
					case 8: sWord6 = "mantê-lo seguro"; 				break;
					case 9: sWord6 = "guardá-lo de " + sWord4;		break;
					case 10: sWord6 = "escondê-lo de " + sWord4; 	break;
					case 11: sWord6 = "guardá-lo de " + sWord5; 	break;
					case 12: sWord6 = "escondê-lo de " + sWord5; 	break;
					case 13: sWord6 = "usá-lo"; 					break;
					case 14: sWord6 = "fazer uso dele"; 			break;
					case 15: sWord6 = "cumprir a profecia"; 		break;
					case 16: sWord6 = "impedir a profecia"; 			break;
				}

			string[] vWord7 = new string[] {"encontrar", "pegar", "descobrir", "localizar", "desenterrar", "reivindicar", "roubar", "adquirir", "destruir", "esconder", "levar"};
				string sWord7 = vWord7[Utility.RandomMinMax( 0, (vWord7.Length-1) )] + " ";

			string[] vWord8 = new string[] {"eliminar", "matar", "devastar", "esmagar", "destruir"};
				string sWord8 = vWord8[Utility.RandomMinMax( 0, (vWord8.Length-1) )] + " ";

			string[] vWord9 = new string[] {"cavaleiros", "sacerdotes", "aldeões", "heróis", "campeões", "paladinos", "curandeiros", "deuses", "luz", "crianças", "aventureiros", "defensores", "guardas"};
				string sWord9 = vWord9[Utility.RandomMinMax( 0, (vWord9.Length-1) )] + " ";

			string[] vWord10 = new string[] {"nossos inimigos", "nossos adversários", "os " + sWord9 + "", "meus inimigos", "meus adversários", "aqueles que me opõem"};
				string sWord10 = vWord10[Utility.RandomMinMax( 0, (vWord10.Length-1) )] + " ";

			string sWord11 = "para que eu possa " + sWord6;
				if ( alignment == "evil" && Utility.RandomMinMax( 1, 3 ) == 1 ){ sWord6 = "usá-lo para " + sWord8 + " " + sWord10; }
				int nWord11 = Utility.RandomMinMax( 1, 20 );
				switch( nWord11 )
				{
					case 1: sWord11 = "para que eu possa " + sWord6;									break;
					case 2: sWord11 = "para que eu consiga " + sWord6;									break;
					case 3: sWord11 = "para que possamos " + sWord6;								break;
					case 4: sWord11 = "para que consigamos " + sWord6;								break;
					case 5: sWord11 = "para que eu pudesse " + sWord6;								break;
					case 6: sWord11 = "para que pudéssemos " + sWord6;								break;
					case 7: sWord11 = "para que eu talvez possa " + sWord6;								break;
					case 8: sWord11 = "para que talvez possamos " + sWord6;								break;
					case 9: sWord11 = "para mim para que eu possa " + sWord6;							break;
					case 10: sWord11 = "para nós para que possamos " + sWord6;						break;
					case 11: sWord11 = "para mim para que eu consiga " + sWord6;							break;
					case 12: sWord11 = "para nós para que consigamos " + sWord6;						break;
					case 13: sWord11 = "para mim para que eu pudesse " + sWord6;						break;
					case 14: sWord11 = "para nós para que pudéssemos " + sWord6;						break;
					case 15: sWord11 = "antes que " + sWord4 + " " + sWord7 + " isso";			break;
					case 16: sWord11 = "antes que " + sWord5 + " " + sWord7 + " isso";		break;
					case 17: sWord11 = "antes que " + sWord4 + " " + sWord7 + " isso";			break;
					case 18: sWord11 = "antes que " + sWord5 + " " + sWord7 + " isso";		break;
					case 19: sWord11 = "antes que " + sWord4 + " " + sWord7 + " isso";			break;
					case 20: sWord11 = "antes que " + sWord5 + " " + sWord7 + " isso";		break;
				}

			string Quest = sWord1 + " " + sWord2 + " " + sWord3 + " " + sWord11;

			string MoreSentence = "";

			if ( Utility.RandomMinMax( 1, 3 ) == 1 && nWord11 < 15 ) // ADD MORE TO THE SENTENCE
			{
				string sWord12 = "antes"; if ( Utility.RandomMinMax( 1, 3 ) == 1 ){ sWord12 = "depois"; }

				if ( Utility.RandomMinMax( 1, 2 ) == 1 ) // EVENT
				{
					string sWord13 = "começa";
						int nWord13 = Utility.RandomMinMax( 1, 3 );
						if ( alignment == "evil" ){ nWord13 = Utility.RandomMinMax( 4, 6 ); }
						switch( nWord13 )
						{
							case 1: sWord13 = "começa";				break;
							case 2: sWord13 = "inicia";				break;
							case 3: sWord13 = "não pode ser impedido";	break;
							case 4: sWord13 = "pode ser impedido";		break;
							case 5: sWord13 = "é impedido";			break;
							case 6: sWord13 = "termina";				break;
						}

					string sWord14 = "próxima estação";
						int nWord14 = Utility.RandomMinMax( 1, 19 );
						switch( nWord14 )
						{
							case 1: sWord14 = "próxima estação";															break;
							case 2: sWord14 = "próxima fase";																break;
							case 3: sWord14 = "próximo eclipse";															break;
							case 4: sWord14 = "constelação do " + RandomThings.GetRandomThing(0) + " aparece";	break;
							case 5: sWord14 = "estrelas se alinham no " + RandomThings.GetRandomThing(0);					break;
							case 6: sWord14 = "Lua " + Server.Misc.RandomThings.GetRandomColorName(0);								break;
							case 7: sWord14 = "guerra " + sWord13;															break;
							case 8: sWord14 = "fome " + sWord13;														break;
							case 9: sWord14 = "catástrofe " + sWord13;													break;
							case 10: sWord14 = "cataclismo " + sWord13;													break;
							case 11: sWord14 = "apocalipse " + sWord13;													break;
							case 12: sWord14 = "invasão " + sWord13;													break;
							case 13: sWord14 = "tempestade " + sWord13;														break;
							case 14: sWord14 = "seca " + sWord13;													break;
							case 15: sWord14 = "inundação " + sWord13;														break;
							case 16: sWord14 = "doença " + sWord13;													break;
							case 17: sWord14 = "enfermidade " + sWord13;													break;
							case 18: sWord14 = "ritual " + sWord13;														break;
							case 19: sWord14 = "escuridão " + sWord13;													break;
						}

					MoreSentence = " " + sWord12 + " the " + sWord14;
				}
				else
				{
					string sWord15 = "rei";
						int nWord15 = Utility.RandomMinMax( 1, 13 );
						if ( alignment == "evil" ){ nWord15 = Utility.RandomMinMax( 14, 27 ); }
						switch( nWord15 )
						{
							case 1: sWord15 = "dragão";				break;
							case 2: sWord15 = "demônio";				break;
							case 3: sWord15 = "diabo";				break;
							case 4: sWord15 = "príncipe";				break;
							case 5: sWord15 = "rei";				break;
							case 6: sWord15 = "princesa";			break;
							case 7: sWord15 = "lich";				break;
							case 8: sWord15 = "mago";				break;
							case 9: sWord15 = "serpente";			break;
							case 10: sWord15 = "lobo";				break;
							case 11: sWord15 = "necromante";		break;
							case 12: sWord15 = "escuridão";			break;
							case 13: sWord15 = "gigante";				break;
							case 14: sWord15 = "sacerdote";			break;
							case 15: sWord15 = "curandeiro";			break;
							case 16: sWord15 = "cavaleiro";			break;
							case 17: sWord15 = "paladino";			break;
							case 18: sWord15 = "rei";				break;
							case 19: sWord15 = "príncipe";			break;
							case 20: sWord15 = "princesa";			break;
							case 21: sWord15 = "anjo";				break;
							case 22: sWord15 = "deus";				break;
							case 23: sWord15 = "deusa";			break;
							case 24: sWord15 = "luz";				break;
							case 25: sWord15 = "criança";				break;
							case 26: sWord15 = "menino";				break;
							case 27: sWord15 = "menina";				break;
						}

					string sWord16 = "rei";
						int nWord16 = Utility.RandomMinMax( 1, 8 );
						if ( alignment == "evil" ){ nWord16 = Utility.RandomMinMax( 4, 20 ); }
						switch( nWord16 )
						{
							case 1: sWord16 = "morre";				break;
							case 2: sWord16 = "é morto";			break;
							case 3: sWord16 = "nos destrói";		break;
							case 4: sWord16 = "ascende";				break;
							case 5: sWord16 = "nasce";			break;
							case 6: sWord16 = "desperta";			break;
							case 7: sWord16 = "retorna";			break;
							case 8: sWord16 = "é invocado";		break;
							case 9: sWord16 = "me destrói";		break;
							case 10: sWord16 = "nos destrói";		break;
							case 11: sWord16 = "nos mata";			break;
							case 12: sWord16 = "me mata";			break;
							case 13: sWord16 = "me derrota";		break;
							case 14: sWord16 = "nos derrota";		break;
							case 15: sWord16 = "nos detém";			break;
							case 16: sWord16 = "me detém";			break;
							case 17: sWord16 = "arruina meus planos";	break;
							case 18: sWord16 = "frustra meus planos";	break;
							case 19: sWord16 = "arruina nossos planos";	break;
							case 20: sWord16 = "frustra nossos planos";	break;
						}

					MoreSentence = " " + sWord12 + " the " + sWord15 + " " + sWord16;
				}
			}

			Quest = Quest + MoreSentence + ".";

			return Quest;
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		public static string ArtyItems( bool addQuotes )
		{
			string qte = "";
				if ( addQuotes ){ qte = "'"; }

			string arty = "";	
			switch( Utility.RandomMinMax( 1, 306 ) )
			{
				case 1: arty = "Abysmal Gloves"; break;
				case 2: arty = "Achille's Shield"; break;
				case 3: arty = "Achille's Spear"; break;
				case 4: arty = "Acidic Robe"; break;
				case 5: arty = "Aegis of Grace"; break;
				case 6: arty = "Aegis"; break;
				case 7: arty = "Ailric's Longbow"; break;
				case 8: arty = "Alchemist's Bauble"; break;
				case 9: arty = "Ancient Samurai Helm"; break;
				case 10: arty = "Arcane Arms"; break;
				case 11: arty = "Arcane Cap"; break;
				case 12: arty = "Arcane Gloves"; break;
				case 13: arty = "Arcane Gorget"; break;
				case 14: arty = "Arcane Leggings"; break;
				case 15: arty = "Arcane Shield"; break;
				case 16: arty = "Arcane Tunic"; break;
				case 17: arty = "Arcanic Robe"; break;
				case 18: arty = "Arctic Death Dealer"; break;
				case 19: arty = "Armor of Fortune"; break;
				case 20: arty = "Armor of Insight"; break;
				case 21: arty = "Armor of Nobility"; break;
				case 22: arty = "Arms of Aegis"; break;
				case 23: arty = "Arms of Fortune"; break;
				case 24: arty = "Arms of Insight"; break;
				case 25: arty = "Arms of Nobility"; break;
				case 26: arty = "Arms of the Fallen King"; break;
				case 27: arty = "Arms of the Harrower"; break;
				case 28: arty = "Arms Of Toxicity"; break;
				case 29: arty = "Axe of the Heavens"; break;
				case 30: arty = "Axe of the Minotaur"; break;
				case 31: arty = "Beggar's Robe"; break;
				case 32: arty = "Belt of Hercules"; break;
				case 33: arty = "Berserker's Maul"; break;
				case 34: arty = "Blade Dance"; break;
				case 35: arty = "Blade of Insanity"; break;
				case 36: arty = "Blade of the Cimmerian"; break;
				case 37: arty = "Blade of the Righteous"; break;
				case 38: arty = "Blade of the Shadows"; break;
				case 39: arty = "Blaze of Death"; break;
				case 40: arty = "Blight Gripped Longbow"; break;
				case 41: arty = "Bloodwood Spirit"; break;
				case 42: arty = "Bone Crusher"; break;
				case 43: arty = "Bonesmasher"; break;
				case 44: arty = "Book Of Knowledge"; break;
				case 45: arty = "Boomstick"; break;
				case 46: arty = "Boots of Hermes"; break;
				case 47: arty = "Bow of the Juka King"; break;
				case 48: arty = "Bow of the Phoenix"; break;
				case 49: arty = "Bracelet of Health"; break;
				case 50: arty = "Bracelet of the Elements"; break;
				case 51: arty = "Bracelet of the Vile"; break;
				case 52: arty = "Bramble Coat"; break;
				case 53: arty = "Brave Knight of Sosaria"; break;
				case 54: arty = "Breath of the Dead"; break;
				case 55: arty = "Burglar's Bandana"; break;
				case 56: arty = "Calm"; break;
				case 57: arty = "Candle of Cold Light"; break;
				case 58: arty = "Candle of Energized Light"; break;
				case 59: arty = "Candle of Fire Light"; break;
				case 60: arty = "Candle of Ghostly Light"; break;
				case 61: arty = "Candle of Poisonous Light"; break;
				case 62: arty = "Candle of Wizardly Light"; break;
				case 63: arty = "Cap of Fortune"; break;
				case 64: arty = "Cap of the Fallen King"; break;
				case 65: arty = "Captain John's Hat"; break;
				case 66: arty = "Captain Quacklebush's Cutlass"; break;
				case 67: arty = "Cavorting Club"; break;
				case 68: arty = "Circlet Of The Sorceress"; break;
				case 69: arty = "Cloak of the Rogue"; break;
				case 70: arty = "Coif of Bane"; break;
				case 71: arty = "Coif of Fire"; break;
				case 72: arty = "Cold Blood"; break;
				case 73: arty = "Cold Forged Blade"; break;
				case 74: arty = "Crimson Cincture"; break;
				case 75: arty = "Crown of Tal'Keesh"; break;
				case 76: arty = "Dagger of Venom"; break;
				case 77: arty = "Dark Guardian's Chest"; break;
				case 78: arty = "Dark Neck"; break;
				case 79: arty = "Detective Boots of the Royal Guard"; break;
				case 80: arty = "Divine Arms"; break;
				case 81: arty = "Divine Countenance"; break;
				case 82: arty = "Divine Gloves"; break;
				case 83: arty = "Divine Gorget"; break;
				case 84: arty = "Divine Leggings"; break;
				case 85: arty = "Divine Tunic"; break;
				case 86: arty = "Djinni's Ring"; break;
				case 87: arty = "Dread Pirate Hat"; break;
				case 88: arty = "Dryad Bow"; break;
				case 89: arty = "Dupre’s Shield"; break;
				case 90: arty = "Dupre's Collar"; break;
				case 91: arty = "Earrings of Health"; break;
				case 92: arty = "Earrings of the Elements"; break;
				case 93: arty = "Earrings of the Magician"; break;
				case 94: arty = "Earrings of the Vile"; break;
				case 95: arty = "Embroidered Oak Leaf Cloak"; break;
				case 96: arty = "Enchanted Pirate Rapier"; break;
				case 97: arty = "Essence of Battle"; break;
				case 98: arty = "Evil Mage Gloves"; break;
				case 99: arty = "Excalibur"; break;
				case 100: arty = "Fang of Ractus"; break;
				case 101: arty = "Fey Leggings"; break;
				case 102: arty = "Flesh Ripper"; break;
				case 103: arty = "Fortified Arms"; break;
				case 104: arty = "Fortunate Blades"; break;
				case 105: arty = "Frostbringer"; break;
				case 106: arty = "Fur Cape Of The Sorceress"; break;
				case 107: arty = "Gauntlets of Anger"; break;
				case 108: arty = "Gauntlets of Nobility"; break;
				case 109: arty = "Geishas Obi"; break;
				case 110: arty = "Giant Blackjack"; break;
				case 111: arty = "Gladiator's Collar"; break;
				case 112: arty = "Gloves of Aegis"; break;
				case 113: arty = "Gloves Of Corruption"; break;
				case 114: arty = "Gloves of Dexterity"; break;
				case 115: arty = "Gloves of Fortune"; break;
				case 116: arty = "Gloves of Insight"; break;
				case 117: arty = "Gloves Of Regeneration"; break;
				case 118: arty = "Gloves of the Fallen King"; break;
				case 119: arty = "Gloves of the Harrower"; break;
				case 120: arty = "Gloves of the Pugilist"; break;
				case 121: arty = "Good Samaritan Robe"; break;
				case 122: arty = "Gorget of Aegis"; break;
				case 123: arty = "Gorget of Fortune"; break;
				case 124: arty = "Gorget of Insight"; break;
				case 125: arty = "Grim Reaper's Lantern"; break;
				case 126: arty = "Grim Reaper's Mask"; break;
				case 127: arty = "Grim Reaper's Robe"; break;
				case 128: arty = "Grim Reaper's Scythe"; break;
				case 129: arty = "Guardsman Halberd"; break;
				case 130: arty = "Gwenno's Harp"; break;
				case 131: arty = "Hammer of Thor"; break;
				case 132: arty = "Hat of the Magi"; break;
				case 133: arty = "Heart of the Lion"; break;
				case 134: arty = "Hell Forged Arms"; break;
				case 135: arty = "Helm of Aegis"; break;
				case 136: arty = "Helm of Brilliance"; break;
				case 137: arty = "Helm of Insight"; break;
				case 138: arty = "Helm of Swiftness"; break;
				case 139: arty = "Helm of the Cimmerian"; break;
				case 140: arty = "Holy Knight's Arm Plates"; break;
				case 141: arty = "Holy Knight's Breastplate"; break;
				case 142: arty = "Holy Knight's Gloves"; break;
				case 143: arty = "Holy Knight's Gorget"; break;
				case 144: arty = "Holy Knight's Legging"; break;
				case 145: arty = "Holy Knight's Plate Helm"; break;
				case 146: arty = "Holy Lance"; break;
				case 147: arty = "Holy Sword"; break;
				case 148: arty = "Hunter's Arms"; break;
				case 149: arty = "Hunter's Gloves"; break;
				case 150: arty = "Hunter's Gorget"; break;
				case 151: arty = "Hunter's Headdress"; break;
				case 152: arty = "Hunter's Leggings"; break;
				case 153: arty = "Hunter's Tunic"; break;
				case 154: arty = "Inquisitor's Arms"; break;
				case 155: arty = "Inquisitor's Gorget"; break;
				case 156: arty = "Inquisitor's Helm"; break;
				case 157: arty = "Inquisitor's Leggings"; break;
				case 158: arty = "Inquisitor's Resolution"; break;
				case 159: arty = "Inquisitor's Tunic"; break;
				case 160: arty = "Iolo's Lute"; break;
				case 161: arty = "Ironwood Crown"; break;
				case 162: arty = "Jackal's Arms"; break;
				case 163: arty = "Jackal's Collar"; break;
				case 164: arty = "Jackal's Gloves"; break;
				case 165: arty = "Jackal's Helm"; break;
				case 166: arty = "Jackal's Leggings"; break;
				case 167: arty = "Jackal's Tunic"; break;
				case 168: arty = "Jade Scimitar"; break;
				case 169: arty = "Jester Hat of Chuckles"; break;
				case 170: arty = "Jin-Baori Of Good Fortune"; break;
				case 171: arty = "Kami-Naris Indestructable Axe"; break;
				case 172: arty = "Kodiak Bear Mask"; break;
				case 173: arty = "Legacy of the Dread Lord"; break;
				case 174: arty = "Legging of Fortune"; break;
				case 175: arty = "Legging of Insight"; break;
				case 176: arty = "Leggings of Aegis"; break;
				case 177: arty = "Leggings of Bane"; break;
				case 178: arty = "Leggings Of Deceit"; break;
				case 179: arty = "Leggings Of Enlightenment"; break;
				case 180: arty = "Leggings of Fire"; break;
				case 181: arty = "Leggings of the Fallen King"; break;
				case 182: arty = "Leggings of the Harrower"; break;
				case 183: arty = "Legs of Nobility"; break;
				case 184: arty = "Loin Cloth of the Cimmerian"; break;
				case 185: arty = "Lucky Necklace"; break;
				case 186: arty = "Luminous Rune Blade"; break;
				case 187: arty = "Magician's Mempo"; break;
				case 188: arty = "Mask of Death"; break;
				case 189: arty = "Maul of the Titans"; break;
				case 190: arty = "Melisande's Corroded Hatchet"; break;
				case 191: arty = "Merlin's Mystical Hat"; break;
				case 192: arty = "Merlin's Mystical Robe"; break;
				case 193: arty = "Merlin's Mystical Staff"; break;
				case 194: arty = "Midnight Bracers"; break;
				case 195: arty = "Midnight Gloves"; break;
				case 196: arty = "Midnight Helm"; break;
				case 197: arty = "Midnight Leggings"; break;
				case 198: arty = "Midnight Tunic"; break;
				case 199: arty = "Necromancer Shroud"; break;
				case 200: arty = "Night Reaper"; break;
				case 201: arty = "Night's Kiss"; break;
				case 202: arty = "Nosferatu's Robe"; break;
				case 203: arty = "Nox Ranger's Heavy Crossbow"; break;
				case 204: arty = "Oblivion Needle"; break;
				case 205: arty = "Orc Chieftain Helm"; break;
				case 206: arty = "Orcish Visage"; break;
				case 207: arty = "Ornament of the Magician"; break;
				case 208: arty = "Ornate Crown of the Harrower"; break;
				case 209: arty = "Ossian Grimoire"; break;
				case 210: arty = "Overseer Sundered Blade"; break;
				case 211: arty = "Pacify"; break;
				case 212: arty = "Pads of the Cu Sidhe"; break;
				case 213: arty = "Pendant of the Magi"; break;
				case 214: arty = "Pestilence"; break;
				case 215: arty = "Phantom Staff"; break;
				case 216: arty = "Pixie Swatter"; break;
				case 217: arty = "Polar Bear Boots"; break;
				case 218: arty = "Polar Bear Cape"; break;
				case 219: arty = "Quell"; break;
				case 220: arty = "Quiver of Blight"; break;
				case 221: arty = "Quiver of Fire"; break;
				case 222: arty = "Quiver of Ice"; break;
				case 223: arty = "Quiver of Infinity"; break;
				case 224: arty = "Quiver of Lightning"; break;
				case 225: arty = "Quiver of Rage"; break;
				case 226: arty = "Quiver of the Elements"; break;
				case 227: arty = "Raed's Glory"; break;
				case 228: arty = "Ramus' Necromantic Scalpel"; break;
				case 229: arty = "Resillient Bracer"; break;
				case 230: arty = "Retort"; break;
				case 231: arty = "Righteous Anger"; break;
				case 232: arty = "Ring of Health"; break;
				case 233: arty = "Ring of Protection"; break;
				case 234: arty = "Ring of the Elements"; break;
				case 235: arty = "Ring of the Magician"; break;
				case 236: arty = "Ring of the Vile"; break;
				case 237: arty = "Robe of Sosaria"; break;
				case 238: arty = "Robe Of Teleportation"; break;
				case 239: arty = "Robe of the Eclipse"; break;
				case 240: arty = "Robe of the Equinox"; break;
				case 241: arty = "Robe Of Treason"; break;
				case 242: arty = "Robin Hood's Bow"; break;
				case 243: arty = "Robin Hood's Feathered Hat"; break;
				case 244: arty = "Rod Of Resurrection"; break;
				case 245: arty = "Royal Guard Sash"; break;
				case 246: arty = "Royal Guard Survival Knife"; break;
				case 247: arty = "Royal Guardian's Gorget"; break;
				case 248: arty = "Royal Guard's Chest Plate"; break;
				case 249: arty = "Royal Leggings of Embers"; break;
				case 250: arty = "Rune Carving Knife"; break;
				case 251: arty = "Scepter Of The False Goddess"; break;
				case 252: arty = "Serpent's Fang"; break;
				case 253: arty = "Shadow Dancer Arms"; break;
				case 254: arty = "Shadow Dancer Cap"; break;
				case 255: arty = "Shadow Dancer Gloves"; break;
				case 256: arty = "Shadow Dancer Gorget"; break;
				case 257: arty = "Shadow Dancer Leggings"; break;
				case 258: arty = "Shadow Dancer Tunic"; break;
				case 259: arty = "Shamino’s Crossbow"; break;
				case 260: arty = "Shard Thrasher"; break;
				case 261: arty = "Shield of Invulnerability"; break;
				case 262: arty = "Shimmering Talisman"; break;
				case 263: arty = "Shroud of Deceit"; break;
				case 264: arty = "Shroud of Shadows"; break;
				case 265: arty = "Silvani's Feywood Bow"; break;
				case 266: arty = "Slayer of Dragons"; break;
				case 267: arty = "Song Woven Mantle"; break;
				case 268: arty = "Soul Seeker"; break;
				case 269: arty = "Spell Woven Britches"; break;
				case 270: arty = "Spirit of the Polar Bear"; break;
				case 271: arty = "Spirit of the Totem"; break;
				case 272: arty = "Sprinter's Sandals"; break;
				case 273: arty = "Staff of Power"; break;
				case 274: arty = "Staff of the Magi"; break;
				case 275: arty = "Staff of the Serpent"; break;
				case 276: arty = "Stitcher's Mittens"; break;
				case 277: arty = "Stormbringer"; break;
				case 278: arty = "Subdue"; break;
				case 279: arty = "Swift Strike"; break;
				case 280: arty = "Sword of Shattered Hopes"; break;
				case 281: arty = "Sword of Sinbad"; break;
				case 282: arty = "Talon Bite"; break;
				case 283: arty = "Taskmaster"; break;
				case 284: arty = "Titan's Hammer"; break;
				case 285: arty = "Torch of Trap Burning"; break;
				case 286: arty = "Totem Arms"; break;
				case 287: arty = "Totem Gloves"; break;
				case 288: arty = "Totem Gorget"; break;
				case 289: arty = "Totem Leggings"; break;
				case 290: arty = "Totem of the Void"; break;
				case 291: arty = "Totem Tunic"; break;
				case 292: arty = "Tunic of Aegis"; break;
				case 293: arty = "Tunic of Bane"; break;
				case 294: arty = "Tunic of Fire"; break;
				case 295: arty = "Tunic of the Fallen King"; break;
				case 296: arty = "Tunic of the Harrower"; break;
				case 297: arty = "Vampire Killer"; break;
				case 298: arty = "Vampiric Daisho"; break;
				case 299: arty = "Violet Courage"; break;
				case 300: arty = "Voice of the Fallen King"; break;
				case 301: arty = "Wildfire Bow"; break;
				case 302: arty = "Windsong"; break;
				case 303: arty = "Wizard's Pants"; break;
				case 304: arty = "Wrath of the Dryad"; break;
				case 305: arty = "Yashimoto's Hatsuburi"; break;
				case 306: arty = "Zyronic Claw"; break;
			}

			string adj = "poderoso";
			switch( Utility.RandomMinMax( 0, 5 ) )
			{
				case 1: adj = "encantado";		break;
				case 2: adj = "místico";		break;
				case 3: adj = "lendário";		break;
				case 4: adj = "mágico";		break;
				case 5: adj = "fabuloso";			break;
			}

			arty = "o " + adj + " " + qte + arty + qte + "";

			return arty;
		}

		public static string QuestItems( bool addQuotes )
		{
			string qte = "";
				if ( addQuotes ){ qte = "'"; }

			string OwnerName = OwnerName = RandomThings.GetRandomName();

			if ( OwnerName.EndsWith( "s" ) )
			{
				OwnerName = OwnerName + "'";
			}
			else
			{
				OwnerName = OwnerName + "'s";
			}

			string[] xItem = new string[] {"Amuleto", "Armadura", "Machado", "Bolsa", "Cinto", "Lâmina", "Ossos", "Livro", "Botas", "Garrafa", "Arco", "Bracelete", "Vela", "Capa", "Cálice", "Manto", "Clava", "Códice", "Besta", "Coroa", "Bola de Cristal", "Cimitarra", "Adaga", "Tambor", "Poeira", "Brincos", "Elixir", "Flauta", "Gema", "Luvas", "Cálice", "Alabarda", "Chapéu", "Elmo", "Chifre", "Chave", "Faca", "Kryss", "Lanterna", "Léxico", "Alaúde", "Maça", "Espelho", "Colar", "Pergaminho", "Retrato", "Poção", "Bolsa", "Anel", "Túnica", "Cajado", "Corda", "Bainha", "Cetro", "Cimitarra", "Pergaminho", "Grilhões", "Escudo", "Crânio", "Grimório", "Cajado", "Pedra", "Espada", "Tábua", "Tomo", "Tridente", "Varinha", "Martelo de Guerra"};
				string name = xItem[Utility.RandomMinMax( 0, (xItem.Length-1) )];

			string[] xAdj = new string[] {"Exótico", "Misterioso", "Encantado", "Maravilhoso", "Incrível", "Assombroso", "Místico", "Espantoso", "Mágico", "Divino", "Excelente", "Magnífico", "Fenomenal", "Fantástico", "Inacreditável", "Extraordinário", "Fabuloso", "Prodigioso", "Glorioso", "Perdido", "Lendário", "Mítico", "Desaparecido", "Ancestral", "Ornamentado", "Supremo", "Raro", "Maravilhoso", "Sagrado", "Todo-Poderoso", "Supremo", "Poderoso", "Indizível", "Desconhecido", "Esquecido", "Amaldiçoado", "Brilhante", "Sombrio", "Maligno", "Sagrado", "Vil", "Etéreo", "Demoníaco", "Ardente", "Angelical", "Flamejante", "Congelado", "Glacial", "Enegrecido", "Lunar", "Solar", "Luminoso", "Elétrico", "Mortal", "Enfeitiçado", "Profano", "Abençoado", "Infernal", "Condenado", "Fatídico"};
				string sAdjective = xAdj[Utility.RandomMinMax( 0, (xAdj.Length-1) )];

			string eAdjective = "Might";
			switch( Utility.RandomMinMax( 0, 116 ) )
			{
				case 0: eAdjective = "a Luz"; 			break;
				case 1: eAdjective = "a Escuridão"; 			break;
				case 2: eAdjective = "os Espíritos"; 		break;
				case 3: eAdjective = "os Mortos"; 			break;
				case 4: eAdjective = "a Maldição"; 			break;
				case 5: eAdjective = "Hades"; 				break;
				case 6: eAdjective = "Fogo"; 				break;
				case 7: eAdjective = "Gelo";		 			break;
				case 8: eAdjective = "o Vazio"; 			break;
				case 9: eAdjective = "Veneno"; 				break;
				case 10: eAdjective = "os Planos";			break;
				case 11: eAdjective = "o Demônio";			break;
				case 12: eAdjective = "o Anjo";			break;
				case 13: eAdjective = "o Diabo";			break;
				case 14: eAdjective = "Morte";				break;
				case 15: eAdjective = "Vida";				break;
				case 16: eAdjective = "Ilusões";			break;
				case 17: eAdjective = "o Outro Mundo";	break;
				case 18: eAdjective = "Energia Negativa";	break;
				case 19: eAdjective = "Realidade";			break;
				case 20: eAdjective = "o Céu";			break;
				case 21: eAdjective = "a Lua";			break;
				case 22: eAdjective = "o Sol";			break;
				case 23: eAdjective = "as Estrelas";			break;
				case 24: eAdjective = "a Terra";			break;
				case 25: eAdjective = "a Masmorra";		break;
				case 26: eAdjective = "a Tumba";			break;
				case 27: eAdjective = "o Fantasma";			break;
				case 28: eAdjective = "Mal Supremo"; 		break;
				case 29: eAdjective = "Mal Puro"; 			break;
				case 30: eAdjective = "Poder Demoníaco"; 		break;
				case 31: eAdjective = "Luz Sagrada"; 		break;
				case 32: eAdjective = "o Amaldiçoado"; 		break;
				case 33: eAdjective = "o Condenado"; 		break;
				case 34: eAdjective = "o Vil"; 			break;
				case 35: eAdjective = "Maldade"; 				break;
				case 36: eAdjective = "Escuridão"; 			break;
				case 37: eAdjective = "Pureza"; 			break;
				case 38: eAdjective = "Força"; 				break;
				case 39: eAdjective = "Poder"; 				break;
				case 40: eAdjective = "Grandeza"; 			break;
				case 41: eAdjective = "Magia"; 				break;
				case 42: eAdjective = "Supremacia"; 			break;
				case 43: eAdjective = "o Todo-Poderoso"; 		break;
				case 44: eAdjective = "o Sagrado"; 		break;
				case 45: eAdjective = "Magnificência"; 		break;
				case 46: eAdjective = "Excelência"; 		break;
				case 47: eAdjective = "Glória"; 				break;
				case 48: eAdjective = "Mistério"; 			break;
				case 49: eAdjective = "o Divino"; 		break;
				case 50: eAdjective = "o Esquecido"; 		break;
				case 51: eAdjective = "Lenda"; 			break;
				case 52: eAdjective = "o Perdido"; 			break;
				case 53: eAdjective = "os Anciãos"; 		break;
				case 54: eAdjective = "Maravilha"; 			break;
				case 55: eAdjective = "o Poderoso"; 		break;
				case 56: eAdjective = "Prodígio"; 			break;
				case 57: eAdjective = "Nobreza"; 			break;
				case 58: eAdjective = "Misticismo"; 			break;
				case 59: eAdjective = "Encantamento"; 		break;
				case 60: eAdjective = "o Templário";		break;
				case 61: eAdjective = "o Ladrão";			break;
				case 62: eAdjective = "o Ilusionista";	break;
				case 63: eAdjective = "a Princesa";		break;
				case 64: eAdjective = "o Invocador";		break;
				case 65: eAdjective = "a Sacerdotisa";		break;
				case 66: eAdjective = "o Conjurador";		break;
				case 67: eAdjective = "o Bandido";			break;
				case 68: eAdjective = "a Baronesa";		break;
				case 69: eAdjective = "o Mago";			break;
				case 70: eAdjective = "o Clérigo";			break;
				case 71: eAdjective = "o Monge";			break;
				case 72: eAdjective = "o Menestrel";		break;
				case 73: eAdjective = "o Defensor";		break;
				case 74: eAdjective = "o Cavaleiro";		break;
				case 75: eAdjective = "o Mágico";		break;
				case 76: eAdjective = "a Bruxa";			break;
				case 77: eAdjective = "o Guerreiro";		break;
				case 78: eAdjective = "o Buscador";			break;
				case 79: eAdjective = "o Matador";			break;
				case 80: eAdjective = "o Patrulheiro";			break;
				case 81: eAdjective = "o Bárbaro";		break;
				case 82: eAdjective = "o Explorador";		break;
				case 83: eAdjective = "o Herege";		break;
				case 84: eAdjective = "o Gladiador";		break;
				case 85: eAdjective = "o Sábio";			break;
				case 86: eAdjective = "o Ladino";			break;
				case 87: eAdjective = "o Paladino";		break;
				case 88: eAdjective = "o Bardo";			break;
				case 89: eAdjective = "o Oráculo";		break;
				case 90: eAdjective = "a Dama";			break;
				case 91: eAdjective = "o Fora da Lei";			break;
				case 92: eAdjective = "o Profeta";		break;
				case 93: eAdjective = "o Mercenário";		break;
				case 94: eAdjective = "o Aventureiro";		break;
				case 95: eAdjective = "a Encantadora";	break;
				case 96: eAdjective = "a Rainha";			break;
				case 97: eAdjective = "o Batedor";			break;
				case 98: eAdjective = "o Místico";			break;
				case 99: eAdjective = "o Mago";			break;
				case 100: eAdjective = "o Viajante";		break;
				case 101: eAdjective = "o Invocador";		break;
				case 102: eAdjective = "o Guerreiro";		break;
				case 103: eAdjective = "a Feiticeira";	break;
				case 104: eAdjective = "o Vidente";			break;
				case 105: eAdjective = "o Caçador";		break;
				case 106: eAdjective = "o Cavaleiro";		break;
				case 107: eAdjective = "o Necromante";	break;
				case 108: eAdjective = "o Xamã";		break;
				case 109: eAdjective = "o Príncipe";		break;
				case 110: eAdjective = "o Sacerdote";		break;
				case 111: eAdjective = "o Barão";			break;
				case 112: eAdjective = "o Bruxo";		break;
				case 113: eAdjective = "o Lorde";			break;
				case 114: eAdjective = "o Encantador";		break;
				case 115: eAdjective = "o Rei";			break;
				case 116: eAdjective = "o Feiticeiro";		break;
			}

			int FirstLast = 0;
			if ( Utility.RandomMinMax( 0, 1 ) == 1 ){ FirstLast = 1; }

			if ( FirstLast == 0 ) // FIRST COMES ADJECTIVE
			{
				switch( Utility.RandomMinMax( 0, 5 ) )
				{
					case 0: name = "o " + qte + sAdjective + " " + name + " de " + ContainerFunctions.GetOwner( "property" ) + qte + ""; 	break;
					case 1: name = "o " + qte + name + " de " + ContainerFunctions.GetOwner( "property" ) + qte + ""; 					break;
					case 2: name = "o " + qte + sAdjective + " " + name + qte + ""; 														break;
					case 3: name = "o " + qte + sAdjective + " " + name + " de " + ContainerFunctions.GetOwner( "property" ) + qte + ""; 	break;
					case 4: name = "o " + qte + name + " de " + ContainerFunctions.GetOwner( "property" ) + qte + ""; 					break;
					case 5: name = "o " + qte + sAdjective + " " + name + qte + ""; 														break;
				}
			}
			else // FIRST COMES OWNER
			{
				switch( Utility.RandomMinMax( 0, 3 ) )
				{
					case 0: name = "" + qte + OwnerName + " " + name + " de " + eAdjective + qte + ""; 										break;
					case 1: name = "o " + qte + name + " de " + eAdjective + qte + ""; 													break;
					case 2: name = "" + qte + OwnerName + " " + name + qte + ""; 															break;
					case 3: name = "" + qte + OwnerName + " " + sAdjective + " " + name + qte + ""; 										break;
				}
			}

			name = name.Replace(" o o ", " o ");

			return name;
		}
	}
}
