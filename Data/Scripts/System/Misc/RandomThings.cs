using System;
using Server;
using System.Collections;
using Server.Misc;
using Server.Network;
using Server.Commands;
using Server.Commands.Generic;
using Server.Items;
using Server.Regions;

namespace Server.Misc
{
    class RandomThings
    {
		public static string GetOddityAdjective()
		{
			string sAdjective = "um(a) estranho(a)";

			switch( Utility.RandomMinMax( 0, 6 ) )
			{
				case 0: sAdjective = "um(a) estranho(a)"; break;
				case 1: sAdjective = "um(a) incomum"; break;
				case 2: sAdjective = "um(a) bizarro(a)"; break;
				case 3: sAdjective = "um(a) curioso(a)"; break;
				case 4: sAdjective = "um(a) peculiar"; break;
				case 5: sAdjective = "um(a) estranho(a)"; break;
				case 6: sAdjective = "um(a) esquisito(a)"; break;
			}
			return sAdjective;
		}

		///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		public static string GetRandomAuthor()
		{
			string sWhoName = RandomThings.GetRandomBoyName();
			string sWhoTitle = RandomThings.GetBoyGirlJob( 0 );
			string sWhoRoyalty = RandomThings.GetRandomBoyNoble();

			if ( Utility.RandomMinMax( 1, 3 ) == 1 ) // FEMALE
			{
				sWhoName = RandomThings.GetRandomGirlName();
				sWhoTitle = RandomThings.GetBoyGirlJob( 1 );
				sWhoRoyalty = RandomThings.GetRandomGirlNoble();
			}

			if ( Utility.RandomMinMax( 1, 4 ) == 1 )
				return sWhoName + " the " + sWhoRoyalty;

			return sWhoName + " the " + sWhoTitle;
		}

		public static int GetRandomBookItemID()
		{
			return Utility.RandomList( 0x65CC, 0x65CD, 0x5688, 0x5689, 0x4FDD, 0x4FDE, 0x4FDF, 0x4FE0, 0x4FF6, 0x4FF7, 0xAA8, 0xB3B, 0xE3B, 0x0E3B, 0xFEF, 0xFF0, 0xFF1, 0xFF2, 0x27BC, 0x2B6F, 0x2254, 0x2259, 0x225A, 0x225B, 0x22C5, 0x36A2, 0x36A3, 0x27BB, 0x27BD, 0x2D50, 0x2D9D, 0x42B7, 0x42B8, 0x1C11, 0x2253, 0x2254, 0x42BF, 0x2205, 0x220F, 0x2219, 0x2223, 0x222D, 0x225C, 0x225D, 0x225E, 0x225F, 0x2253, 0x2254, 0x3B51, 0x3B52, 0x3B53, 0x3B54, 0x3B55, 0x3B56, 0x3B57, 0x3B58, 0x3B59, 0x3B5A );
		}

		///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		public static int GetRandomMetallicColor()
		{
			return Utility.RandomList( 0x436, 0x445, 0x435, 0x433, 0x43A, 0x424, 0x44C, 0x44B, 0x43F, 0x440, 0x449, 0x432, 0x43E, 0x44D, 0x437, 0x8D5, 0x950, 0x4A2, 0x8E2, 0xB0C, 0xB3B, 0xB5E, 0x869, 0x982, 0x5CE, 0x56A, 0x7CB, 0x7CA, 0x856, 0x99D );
		}

		///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		public static string GetRandomBelongsTo( string style )
		{
			string who = RandomThings.GetRandomName();
				if ( style == "orient" ){ who = RandomThings.GetRandomOrientalName(); }

			if ( who.EndsWith( "s" ) )
			{
				who = who + "'";
			}
			else
			{
				who = who + "'s";
			}

			return who;
		}

		///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		public static string MadeUpCity()
		{
			string sPlace = "Vila";
			string sPerson = NameList.RandomName( "elf_female" );

			switch( Utility.RandomMinMax( 0, 4 ) )
			{
				case 0: sPlace = "Vila"; break;
				case 1: sPlace = "Cidade"; break;
				case 2: sPlace = "Vila"; break;
				case 3: sPlace = "Fortaleza"; break;
				case 4: sPlace = "Aldeia"; break;
			}

			switch( Utility.RandomMinMax( 0, 16 ) )
			{
				case 1: sPerson = NameList.RandomName( "vampire" ); break;
				case 2: sPerson = NameList.RandomName( "drakkul" ); break;
				case 3: sPerson = NameList.RandomName( "greek" ); break;
				case 4: sPerson = NameList.RandomName( "urk" ); break;
				case 5: sPerson = NameList.RandomName( "giant" ); break;
				case 6: sPerson = NameList.RandomName( "imp" ); break;
				case 7: sPerson = NameList.RandomName( "dragon" ); break;
				case 8: sPerson = NameList.RandomName( "goddess" ); break;
				case 9: sPerson = NameList.RandomName( "demonic" ); break;
				case 10: sPerson = NameList.RandomName( "ancient lich" ); break;
				case 11: sPerson = NameList.RandomName( "gargoyle name" ); break;
				case 12: sPerson = NameList.RandomName( "centaur" ); break;
				case 13: sPerson = NameList.RandomName( "devil" ); break;
				case 14: sPerson = NameList.RandomName( "evil mage" ); break;
				case 15: sPerson = NameList.RandomName( "evil witch" ); break;
				case 16: sPerson = NameList.RandomName( "elf_male" ); break;
			}

			return " " + sPlace + " de " + sPerson;
		}

		///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		public static string MadeUpDungeon()
		{
			string sPlace = "Masmorra";
			string sAdjective = "Maligna";
			string sBeing = "Lich";
			string sAdj = "Louco";

			switch( Utility.RandomMinMax( 0, 18 ) )
			{
				case 0: sPlace = "Masmorra"; if ( Utility.RandomMinMax( 0, 1 ) == 1 ){ sPlace = "Masmorras"; } break;
				case 1: sPlace = "Caverna"; if ( Utility.RandomMinMax( 0, 1 ) == 1 ){ sPlace = "Cavernas"; } break;
				case 2: sPlace = "Tumba"; if ( Utility.RandomMinMax( 0, 1 ) == 1 ){ sPlace = "Tumbas"; } break;
				case 3: sPlace = "Labirinto"; break;
				case 4: sPlace = "Salão"; if ( Utility.RandomMinMax( 0, 1 ) == 1 ){ sPlace = "Salões"; } break;
				case 5: sPlace = "Cripta"; if ( Utility.RandomMinMax( 0, 1 ) == 1 ){ sPlace = "Criptas"; } break;
				case 6: sPlace = "Torre"; break;
				case 7: sPlace = "Castelo"; break;
				case 8: sPlace = "Ruínas"; break;
				case 9: sPlace = "Montanha"; if ( Utility.RandomMinMax( 0, 1 ) == 1 ){ sPlace = "Montanhas"; } break;
				case 10: sPlace = "Mausoléu"; if ( Utility.RandomMinMax( 0, 1 ) == 1 ){ sPlace = "Catacumbas"; } break;
				case 11: sPlace = "Túnel"; if ( Utility.RandomMinMax( 0, 1 ) == 1 ){ sPlace = "Túneis"; } break;
				case 12: sPlace = "Labirinto"; break;
				case 13: sPlace = "Poço"; if ( Utility.RandomMinMax( 0, 1 ) == 1 ){ sPlace = "Poços"; } break;
				case 14: sPlace = "Cofre"; if ( Utility.RandomMinMax( 0, 1 ) == 1 ){ sPlace = "Cofres"; } break;
				case 15: sPlace = "Caverna"; if ( Utility.RandomMinMax( 0, 1 ) == 1 ){ sPlace = "Cavernas"; } break;
				case 16: sPlace = "Fortaleza"; break;
				case 17: sPlace = "Bastião"; break;
				case 18: sPlace = "Abismo"; break;
			}

			switch( Utility.RandomMinMax( 0, 19 ) )
			{
				case 1: sBeing = "Rei"; break;
				case 2: sBeing = "Rainha"; break;
				case 3: sBeing = "Fantasma"; break;
				case 4: sBeing = "Vampiro"; break;
				case 5: sBeing = "Senhor da Guerra"; break;
				case 6: sBeing = "Sacerdote"; break;
				case 7: sBeing = "Mago"; break;
				case 8: sBeing = "Feiticeira"; break;
				case 9: sBeing = "Deus"; break;
				case 10: sBeing = "Deusa"; break;
				case 11: sBeing = "Diabo"; break;
				case 12: sBeing = "Demônio"; break;
				case 13: sBeing = "Dragão"; break;
				case 14: sBeing = "Cavaleiro"; break;
				case 15: sBeing = "Tirano"; break;
				case 16: sBeing = Server.Misc.RandomThings.GetRandomJobTitle(0); break;
				case 17: sBeing = Server.Misc.RandomThings.GetRandomThing(0); break;
				case 18: sBeing = Server.Misc.RandomThings.GetRandomJobTitle(0); break;
				case 19: sBeing = Server.Misc.RandomThings.GetRandomThing(0); break;
			}

			switch( Utility.RandomMinMax( 0, 31 ) )
			{
				case 1: sAdj = Server.Misc.RandomThings.GetRandomColorName(0); break;
				case 2: sAdj = "Odiado"; break;
				case 3: sAdj = "Temido"; break;
				case 4: sAdj = "Amaldiçoado"; break;
				case 5: sAdj = "Desprezado"; break;
				case 6: sAdj = "Desdenhado"; break;
				case 7: sAdj = "Perdido"; break;
				case 8: sAdj = "Insano"; break;
				case 9: sAdj = "Demente"; break;
				case 10: sAdj = "Enlouquecido"; break;
				case 11: sAdj = "Arruinado"; break;
				case 12: sAdj = "Corrupto"; break;
				case 13: sAdj = "Irado"; break;
				case 14: sAdj = "Perverso"; break;
				case 15: sAdj = "Repulsivo"; break;
				case 16: sAdj = "Funesto"; break;
				case 17: sAdj = "Cruel"; break;
				case 18: sAdj = "Atroz"; break;
				case 19: sAdj = "Bárbaro"; break;
				case 20: sAdj = "Brutal"; break;
				case 21: sAdj = "Impiedoso"; break;
				case 22: sAdj = "Desalmado"; break;
				case 23: sAdj = "Cruel"; break;
				case 24: sAdj = "Sádico"; break;
				case 25: sAdj = "Tirânico"; break;
				case 26: sAdj = "Cruel"; break;
				case 27: sAdj = "Sanguinário"; break;
				case 28: sAdj = "Ferocíssimo"; break;
				case 29: sAdj = "Feroz"; break;
				case 30: sAdj = "Malevolente"; break;
				case 31: sAdj = "Detestado"; break;
			}

			switch( Utility.RandomMinMax( 1, 116 ) )
			{
				case 1: sAdjective = "o Corrupto";     break;
				case 2: sAdjective = "Destruição";     break;
				case 3: sAdjective = "o Odiado";       break;
				case 4: sAdjective = "o Abominável";   break;
				case 5: sAdjective = "o Malevolente";  break;
				case 6: sAdjective = "o Malicioso";    break;
				case 7: sAdjective = "o Nefário";      break;
				case 8: sAdjective = "o Perverso";     break;
				case 9: sAdjective = "o Cruel";        break;
				case 10: sAdjective = "o Vil";         break;
				case 11: sAdjective = "Vilania";       break;
				case 12: sAdjective = "o Imundo";      break;
				case 13: sAdjective = "Danação";       break;
				case 14: sAdjective = "Terror";        break;
				case 15: sAdjective = "o Amaldiçoado"; break;
				case 16: sAdjective = "Perdição";      break;
				case 17: sAdjective = "Pavor";         break;
				case 18: sAdjective = "Repulsão";      break;
				case 19: sAdjective = "Rancor";        break;
				case 20: sAdjective = "Ira";           break;
				case 21: sAdjective = "Morte";         break;
				case 22: sAdjective = "o Sinistro";    break;
				case 23: sAdjective = "Aflição";       break;
				case 24: sAdjective = "Tormento";      break;
				case 25: sAdjective = "Definhar";      break;
				case 26: sAdjective = "Decadência";    break;
				case 27: sAdjective = "Maldições";     break;
				case 28: sAdjective = "o Condenado";   break;
				case 29: sAdjective = "Horror";        break;
				case 30: sAdjective = "o Atormentado"; break;
				case 31: sAdjective = "o Fadado";      break;
				case 32: sAdjective = "o Indizível";   break;
				case 33: sAdjective = "Ódio";          break;
				case 34: sAdjective = "Miséria";       break;
				case 35: sAdjective = "o Corrompido";  break;
				case 36: sAdjective = "Corrupção";     break;
				case 37: sAdjective = "Fúria";         break;
				case 38: sAdjective = "o Temido";      break;
				case 39: sAdjective = "Trevas";        break;
				case 40: sAdjective = "Sombras";       break;
				case 41: sAdjective = "o Louco";       break;
				case 42: sAdjective = "o Insano";      break;
				case 43: sAdjective = "os Nove Infernos"; break;
				case 44: sAdjective = "Cthulhu";       break;
				case 45: sAdjective = "Inferno";       break;
				case 46: sAdjective = "Hades";         break;
				case 47: sAdjective = "Satanás";       break;
				case 48: sAdjective = "os Espíritos";  break;
				case 49: sAdjective = "o Assombrado";  break;
				case 50: sAdjective = "o Morto-vivo";  break;
				case 51: sAdjective = "a Múmia";       break;
				case 52: sAdjective = "o Vampiro";     break;
				case 53: sAdjective = "Sangue";        break;
				case 54: sAdjective = "o Culto";       break;
				case 55: sAdjective = "o Perdido";     break;
				case 56: sAdjective = "Almas Perdidas"; break;
				case 57: sAdjective = "o " + sAdj + " " + sBeing; break;
				case 58: sAdjective = "Ouro";          break;
				case 59: sAdjective = "Prata";         break;
				case 60: sAdjective = "o Necromante";  break;
				case 61: sAdjective = "a Bruxa";       break;
				case 62: sAdjective = "o Bruxo";       break;
				case 63: sAdjective = "o " + sAdj + " " + sBeing; break;
				case 64: sAdjective = "o " + sAdj + " " + sBeing; break;
				case 65: sAdjective = "o Vilão";       break;
				case 66: sAdjective = "Latão";         break;
				case 67: sAdjective = "Bronze";        break;
				case 68: sAdjective = "o Fantasma";    break;
				case 69: sAdjective = "o Cavaleiro da Morte"; break;
				case 70: sAdjective = "o Lich";        break;
				case 71: sAdjective = "o Ocultista";   break;
				case 72: sAdjective = "o Cultista";    break;
				case 73: sAdjective = "o Diabolista";  break;
				case 74: sAdjective = "a Bruxa";       break;
				case 75: sAdjective = "o Açougueiro";  break;
				case 76: sAdjective = "o Abatedor";    break;
				case 77: sAdjective = "o Carrasco";    break;
				case 78: sAdjective = "o Demônio";     break;
				case 79: sAdjective = "o Espectro";    break;
				case 80: sAdjective = "a Sombra";      break;
				case 81: sAdjective = "o Espectro";    break;
				case 82: sAdjective = "o Diabo";       break;
				case 83: sAdjective = "a Sombra";      break;
				case 84: sAdjective = "o Espectro";    break;
				case 85: sAdjective = "o Vampiro";     break;
				case 86: sAdjective = "a Banshee";     break;
				case 87: sAdjective = "o Sombrio";     break;
				case 88: sAdjective = "o Negro";       break;
				case 89: sAdjective = "o Agente Funerário"; break;
				case 90: sAdjective = "o Embalsamador"; break;
				case 91: sAdjective = "Ferro";         break;
				case 92: sAdjective = "o Demônio";     break;
				case 93: sAdjective = "o Demônio";     break;
				case 94: sAdjective = "o " + sAdj + " " + sBeing; break;
				case 95: sAdjective = "o Odioso";      break;
				case 96: sAdjective = "o " + sAdj + " " + sBeing; break;
				case 97: sAdjective = "o Horrendo";    break;
				case 98: sAdjective = "o " + sAdj + " " + sBeing; break;
				case 99: sAdjective = "o " + sAdj + " " + sBeing; break;
				case 100: sAdjective = "o " + sAdj + " " + sBeing; break;
				case 101: sAdjective = "o Esquecido";  break;
				case 102: sAdjective = "os Antigos";   break;
				case 103: sAdjective = "o Imundo";     break;
				case 104: sAdjective = "o Funesto";    break;
				case 105: sAdjective = "o Depravado";  break;
				case 106: sAdjective = "o Repulsivo";  break;
				case 107: sAdjective = "o Irascível";  break;
				case 108: sAdjective = "o Lamentoso";  break;
				case 109: sAdjective = "o Sombrio";    break;
				case 110: sAdjective = "o Sombrio";    break;
				case 111: sAdjective = "o Sem Vida";   break;
				case 112: sAdjective = "o Falecido";   break;
				case 113: sAdjective = "o Sem Sangue"; break;
				case 114: sAdjective = "o Mortificado"; break;
				case 115: sAdjective = "o Partido";    break;
				case 116: sAdjective = "o Morto";      break;
			}

			return " " + sPlace + " d" + sAdjective;
		}

		///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		public static string RandomEvilTitle()
		{
			string sSubs = "Governante";
			string sAdjective = "Maligno";
			string sAdj = "Louco";

			switch( Utility.RandomMinMax( 0, 18 ) )
			{
				case 0: sSubs = "Governante";       break;
				case 1: sSubs = "Senhor da Guerra";     break;
				case 2: sSubs = "Senhor";           break;
				case 3: sSubs = "Supervisor";       break;
				case 4: sSubs = "Servo";            break;
				case 5: sSubs = "Habitante";        break;
				case 6: sSubs = "Escravo";          break;
				case 7: sSubs = "Olho";             break;
				case 8: sSubs = "Mão";              break;
				case 9: sSubs = "Coração";          break;
				case 10: sSubs = "Capanga";         break;
				case 11: sSubs = "Mestre";          break;
				case 12: sSubs = "Conquistador";    break;
				case 13: sSubs = "Líder";           break;
				case 14: sSubs = "Arauto";          break;
				case 15: sSubs = "Presságio";       break;
				case 16: sSubs = "Portador";        break;
				case 17: sSubs = "Sinal";           break;
				case 18: sSubs = "Discípulo";       break;
			}

			switch( Utility.RandomMinMax( 0, 31 ) )
			{
				case 1: sAdj = Server.Misc.RandomThings.GetRandomColorName(0); break;
				case 2: sAdj = "Odiado"; break;
				case 3: sAdj = "Temido"; break;
				case 4: sAdj = "Amaldiçoado"; break;
				case 5: sAdj = "Desprezado"; break;
				case 6: sAdj = "Desdenhado"; break;
				case 7: sAdj = "Perdido"; break;
				case 8: sAdj = "Insano"; break;
				case 9: sAdj = "Demente"; break;
				case 10: sAdj = "Enlouquecido"; break;
				case 11: sAdj = "Arruinado"; break;
				case 12: sAdj = "Corrupto"; break;
				case 13: sAdj = "Irado"; break;
				case 14: sAdj = "Perverso"; break;
				case 15: sAdj = "Repulsivo"; break;
				case 16: sAdj = "Funesto"; break;
				case 17: sAdj = "Cruel"; break;
				case 18: sAdj = "Atroz"; break;
				case 19: sAdj = "Bárbaro"; break;
				case 20: sAdj = "Brutal"; break;
				case 21: sAdj = "Impiedoso"; break;
				case 22: sAdj = "Desalmado"; break;
				case 23: sAdj = "Cruel"; break;
				case 24: sAdj = "Sádico"; break;
				case 25: sAdj = "Tirânico"; break;
				case 26: sAdj = "Cruel"; break;
				case 27: sAdj = "Sanguinário"; break;
				case 28: sAdj = "Ferocíssimo"; break;
				case 29: sAdj = "Feroz"; break;
				case 30: sAdj = "Malevolente"; break;
				case 31: sAdj = "Detestado"; break;
			}

			switch( Utility.RandomMinMax( 1, 108 ) )
			{
				case 1: sAdjective = "o Corrupto";     break;
				case 2: sAdjective = "Destruição";     break;
				case 3: sAdjective = "o Odiado";       break;
				case 4: sAdjective = "o Abominável";   break;
				case 5: sAdjective = "o Malevolente";  break;
				case 6: sAdjective = "o Malicioso";    break;
				case 7: sAdjective = "o Nefário";      break;
				case 8: sAdjective = "o Perverso";     break;
				case 9: sAdjective = "o Cruel";        break;
				case 10: sAdjective = "o Vil";         break;
				case 11: sAdjective = "Vilania";       break;
				case 12: sAdjective = "o Imundo";      break;
				case 13: sAdjective = "Danação";       break;
				case 14: sAdjective = "Terror";        break;
				case 15: sAdjective = "o Amaldiçoado"; break;
				case 16: sAdjective = "Perdição";      break;
				case 17: sAdjective = "Pavor";         break;
				case 18: sAdjective = "Repulsão";      break;
				case 19: sAdjective = "Rancor";        break;
				case 20: sAdjective = "Ira";           break;
				case 21: sAdjective = "Morte";         break;
				case 22: sAdjective = "o Sinistro";    break;
				case 23: sAdjective = "Aflição";       break;
				case 24: sAdjective = "Tormento";      break;
				case 25: sAdjective = "Definhar";      break;
				case 26: sAdjective = "Decadência";    break;
				case 27: sAdjective = "Maldições";     break;
				case 28: sAdjective = "o Condenado";   break;
				case 29: sAdjective = "Horror";        break;
				case 30: sAdjective = "o Atormentado"; break;
				case 31: sAdjective = "o Fadado";      break;
				case 32: sAdjective = "o Indizível";   break;
				case 33: sAdjective = "Ódio";          break;
				case 34: sAdjective = "Miséria";       break;
				case 35: sAdjective = "o Corrompido";  break;
				case 36: sAdjective = "Corrupção";     break;
				case 37: sAdjective = "Fúria";         break;
				case 38: sAdjective = "o Temido";      break;
				case 39: sAdjective = "Trevas";        break;
				case 40: sAdjective = "Sombras";       break;
				case 41: sAdjective = "o Louco";       break;
				case 42: sAdjective = "o Insano";      break;
				case 43: sAdjective = "os Nove Infernos"; break;
				case 44: sAdjective = "Cthulhu";       break;
				case 45: sAdjective = "Inferno";       break;
				case 46: sAdjective = "Hades";         break;
				case 47: sAdjective = "Satanás";       break;
				case 48: sAdjective = "os Espíritos";  break;
				case 49: sAdjective = "o Assombrado";  break;
				case 50: sAdjective = "o Morto-vivo";  break;
				case 51: sAdjective = "a Múmia";       break;
				case 52: sAdjective = "o Vampiro";     break;
				case 53: sAdjective = "Sangue";        break;
				case 54: sAdjective = "o Culto";       break;
				case 55: sAdjective = "o Perdido";     break;
				case 56: sAdjective = "Almas Perdidas"; break;
				case 57: sAdjective = "o Morto";       break;
				case 58: sAdjective = "Ouro";          break;
				case 59: sAdjective = "Prata";         break;
				case 60: sAdjective = "o Necromante";  break;
				case 61: sAdjective = "a Bruxa";       break;
				case 62: sAdjective = "o Bruxo";       break;
				case 63: sAdjective = "o Mortificado"; break;
				case 64: sAdjective = "o Partido";     break;
				case 65: sAdjective = "o Vilão";       break;
				case 66: sAdjective = "Latão";         break;
				case 67: sAdjective = "Bronze";        break;
				case 68: sAdjective = "o Fantasma";    break;
				case 69: sAdjective = "o Cavaleiro da Morte"; break;
				case 70: sAdjective = "o Lich";        break;
				case 71: sAdjective = "o Ocultista";   break;
				case 72: sAdjective = "o Cultista";    break;
				case 73: sAdjective = "o Diabolista";  break;
				case 74: sAdjective = "a Bruxa";       break;
				case 75: sAdjective = "o Açougueiro";  break;
				case 76: sAdjective = "o Abatedor";    break;
				case 77: sAdjective = "o Carrasco";    break;
				case 78: sAdjective = "o Demônio";     break;
				case 79: sAdjective = "o Espectro";    break;
				case 80: sAdjective = "a Sombra";      break;
				case 81: sAdjective = "o Espectro";    break;
				case 82: sAdjective = "o Diabo";       break;
				case 83: sAdjective = "a Sombra";      break;
				case 84: sAdjective = "o Espectro";    break;
				case 85: sAdjective = "o Vampiro";     break;
				case 86: sAdjective = "a Banshee";     break;
				case 87: sAdjective = "o Sombrio";     break;
				case 88: sAdjective = "o Negro";       break;
				case 89: sAdjective = "o Agente Funerário"; break;
				case 90: sAdjective = "o Embalsamador"; break;
				case 91: sAdjective = "Ferro";         break;
				case 92: sAdjective = "o Demônio";     break;
				case 93: sAdjective = "o Demônio";     break;
				case 94: sAdjective = "o Sem Sangue";  break;
				case 95: sAdjective = "o Odioso";      break;
				case 96: sAdjective = "o Falecido";    break;
				case 97: sAdjective = "o Horrendo";    break;
				case 98: sAdjective = "o Sombrio";     break;
				case 99: sAdjective = "o Sombrio";     break;
				case 100: sAdjective = "o Sem Vida";   break;
				case 101: sAdjective = "o Esquecido";  break;
				case 102: sAdjective = "os Antigos";   break;
				case 103: sAdjective = "o Imundo";     break;
				case 104: sAdjective = "o Funesto";    break;
				case 105: sAdjective = "o Depravado";  break;
				case 106: sAdjective = "o Repulsivo";  break;
				case 107: sAdjective = "o Irascível";  break;
				case 108: sAdjective = "o Lamentoso";  break;
			}

			return " " + sAdj + " " + sSubs + " d" + sAdjective;
		}

		///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		public static string GetRandomDisaster()
		{
			string sEvent = "Cataclismo";
			string sAdj = "Grande";

			switch( Utility.RandomMinMax( 0, 13 ) )
			{
				case 0: sEvent = "Cataclismo"; break;
				case 1: sEvent = "Inundação"; break;
				case 2: sEvent = "Desastre"; break;
				case 3: sEvent = "Praga"; break;
				case 4: sEvent = "Catástrofe"; break;
				case 5: sEvent = "Holocausto"; break;
				case 6: sEvent = "Tragédia"; break;
				case 7: sEvent = "Guerra"; break;
				case 8: sEvent = "Praga"; break;
				case 9: sEvent = "Batalha"; break;
				case 10: sEvent = "Flagelo"; break;
				case 11: sEvent = "Pestilência"; break;
				case 12: sEvent = "Invasão"; break;
				case 13: sEvent = "Terremoto"; break;
			}

			switch( Utility.RandomMinMax( 0, 13 ) )
			{
				case 0: sAdj = "Grande"; break;
				case 1: sAdj = "Terrível"; break;
				case 2: sAdj = "Maligno"; break;
				case 3: sAdj = "Vil"; break;
				case 4: sAdj = "Maior"; break;
				case 5: sAdj = "Imenso"; break;
				case 6: sAdj = "Antigo"; break;
				case 7: sAdj = "Destrutivo"; break;
				case 8: sAdj = "Histórico"; break;
				case 9: sAdj = "Famoso"; break;
				case 10: sAdj = "Formidável"; break;
				case 11: sAdj = "Esquecido"; break;
				case 12: sAdj = "Misterioso"; break;
				case 13: sAdj = "Desconhecido"; break;
			}

			return "o " + sAdj + " " + sEvent;
		}

		///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		public static string GetRandomNoble()
		{
			string noble = "Rei";

			switch( Utility.RandomMinMax( 0, 29 ) )
			{
				case 0: noble = "Imperador"; break;
				case 1: noble = "Imperatriz"; break;
				case 2: noble = "Rei"; break;
				case 3: noble = "Rainha"; break;
				case 4: noble = "Príncipe"; break;
				case 5: noble = "Princesa"; break;
				case 6: noble = "Duque"; break;
				case 7: noble = "Duquesa"; break;
				case 8: noble = "Marquês"; break;
				case 9: noble = "Marquesa"; break;
				case 10: noble = "Conde"; break;
				case 11: noble = "Conde"; break;
				case 12: noble = "Condessa"; break;
				case 13: noble = "Visconde"; break;
				case 14: noble = "Viscondessa"; break;
				case 15: noble = "Barão"; break;
				case 16: noble = "Baronesa"; break;
				case 17: noble = "Barão"; break;
				case 18: noble = "Baronesa"; break;
				case 19: noble = "Cavaleiro"; break;
				case 20: noble = "Marquês"; break;
				case 21: noble = "Marquesa"; break;
				case 22: noble = "Cavaleiro"; break;
				case 23: noble = "Czar"; break;
				case 24: noble = "Monarca"; break;
				case 25: noble = "Arcebispo"; break;
				case 26: noble = "Dama"; break;
				case 27: noble = "Senhor"; break;
				case 28: noble = "Chanceler"; break;
				case 29: noble = "Dama"; break;
			}
			return noble;
		}

		public static string GetRandomGirlNoble()
		{
			string noble = "Rainha";

			switch( Utility.RandomMinMax( 0, 12 ) )
			{
				case 1: noble = "Imperatriz"; break;
				case 2: noble = "Princesa"; break;
				case 3: noble = "Duquesa"; break;
				case 4: noble = "Marquesa"; break;
				case 5: noble = "Condessa"; break;
				case 6: noble = "Viscondessa"; break;
				case 7: noble = "Baronesa"; break;
				case 8: noble = "Nobre"; break;
				case 9: noble = "Cavaleira"; break;
				case 10: noble = "Marquesa"; break;
				case 11: noble = "Lady"; break;
				case 12: noble = "Dama"; break;
			}
			return noble;
		}

		public static string GetRandomBoyNoble()
		{
			string noble = "Rei";

			switch( Utility.RandomMinMax( 0, 17 ) )
			{
				case 1: noble = "Imperador"; break;
				case 2: noble = "Príncipe"; break;
				case 3: noble = "Duque"; break;
				case 4: noble = "Marquês"; break;
				case 5: noble = "Conde"; break;
				case 6: noble = "Nobre"; break;
				case 7: noble = "Visconde"; break;
				case 8: noble = "Barão"; break;
				case 9: noble = "Senhor"; break;
				case 10: noble = "Cavaleiro"; break;
				case 11: noble = "Marquês"; break;
				case 12: noble = "Cavaleiro"; break;
				case 13: noble = "Czar"; break;
				case 14: noble = "Monarca"; break;
				case 15: noble = "Arcebispo"; break;
				case 16: noble = "Lord"; break;
				case 17: noble = "Chanceler"; break;
			}
			return noble;
		}

		///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		public static string GetRandomTimeFrame()
		{
			string time = "10 anos";

			switch( Utility.RandomMinMax( 0, 5 ) )
			{
				case 0: time = ( Utility.RandomMinMax( 1, 90 ) * 10 ) + " anos"; break;
				case 1: time = ( Utility.RandomMinMax( 1, 90 ) * 10 ) + ".000 anos"; break;
				case 2: time = Utility.RandomMinMax( 1, 9 ) + ".000 anos"; break;
				case 3: time = ( Utility.RandomMinMax( 1, 90 ) * 10 ) + " séculos"; break;
				case 4: time = Utility.RandomMinMax( 1, 9 ) + ".000 séculos"; break;
				case 5: time = Utility.RandomMinMax( 2, 9 ) + " séculos"; break;
			}
			return time;
		}


		///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		public static string GetRandomWeapon()
		{
			string item = "adaga de assassino";

			switch( Utility.RandomMinMax( 1, 50 ) )
			{
				case 1: item = "adaga de assassino"; break;
				case 2: item = "espada de assassino"; break;
				case 3: item = "machado"; break;
				case 4: item = "machado bárbaro"; break;
				case 5: item = "alabarda"; break;
				case 6: item = "machado de batalha"; break;
				case 7: item = "maça de batalha"; break;
				case 8: item = "cajado laminado"; break;
				case 9: item = "foice"; break;
				case 10: item = "espada larga"; break;
				case 11: item = "faca de açougueiro"; break;
				case 12: item = "cutelo"; break;
				case 13: item = "porrete"; break;
				case 14: item = "lâmina crescente"; break;
				case 15: item = "cutelo"; break;
				case 16: item = "adaga"; break;
				case 17: item = "machado duplo"; break;
				case 18: item = "cajado de lâmina dupla"; break;
				case 19: item = "machado de carrasco"; break;
				case 20: item = "falção"; break;
				case 21: item = "alabarda"; break;
				case 22: item = "martelo picareta"; break;
				case 23: item = "machadinha"; break;
				case 24: item = "katana"; break;
				case 25: item = "kryss"; break;
				case 26: item = "machado de batalha grande"; break;
				case 27: item = "espada longa"; break;
				case 28: item = "maça"; break;
				case 29: item = "machado"; break;
				case 30: item = "malho"; break;
				case 31: item = "picareta"; break;
				case 32: item = "pique"; break;
				case 33: item = "bastão"; break;
				case 34: item = "espada real"; break;
				case 35: item = "cetro"; break;
				case 36: item = "cimitarra"; break;
				case 37: item = "foice"; break;
				case 38: item = "rapieira"; break;
				case 39: item = "faca de esfolar"; break;
				case 40: item = "lança"; break;
				case 41: item = "tridente"; break;
				case 42: item = "machado de duas mãos"; break;
				case 43: item = "espada bárbara"; break;
				case 44: item = "machado de guerra"; break;
				case 45: item = "lâminas de guerra"; break;
				case 46: item = "cutelo de guerra"; break;
				case 47: item = "adaga de guerra"; break;
				case 48: item = "forquilha de guerra"; break;
				case 49: item = "martelo de guerra"; break;
				case 50: item = "maça de guerra"; break;
			}

			return item;
		}

		///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		public static string GetRandomArmorWeaponItem()
		{
			string item = "Bascinete";

			switch( Utility.RandomMinMax( 0, 253 ) )
			{
				case 0: item = "Bascinete"; break;
				case 1: item = "Braços de Osso"; break;
				case 2: item = "Peitoral de Osso"; break;
				case 3: item = "Manoplas de Osso"; break;
				case 4: item = "Elmo de Osso"; break;
				case 5: item = "Perneiras de Osso"; break;
				case 6: item = "Broquel"; break;
				case 7: item = "Peitoral de Cota de Malha"; break;
				case 8: item = "Capuz de Cota de Malha"; break;
				case 9: item = "Hatsuburi de Cota de Malha"; break;
				case 10: item = "Perneiras de Cota de Malha"; break;
				case 11: item = "Escudo do Caos"; break;
				case 12: item = "Diadema"; break;
				case 13: item = "Elmo Fechado"; break;
				case 14: item = "Kabuto Decorativo de Placas"; break;
				case 15: item = "Braços de Escamas de Dragão"; break;
				case 16: item = "Manoplas de Escamas de Dragão"; break;
				case 17: item = "Elmo de Escamas de Dragão"; break;
				case 18: item = "Perneiras de Escamas de Dragão"; break;
				case 19: item = "Túnica de Escamas de Dragão"; break;
				case 20: item = "Peitoral de Couro Feminino"; break;
				case 21: item = "Peitoral de Placas Feminino"; break;
				case 22: item = "Peitoral de Couro Reforçado Feminino"; break;
				case 23: item = "Diadema com Gema"; break;
				case 24: item = "Escudo de Aquecedor"; break;
				case 25: item = "Jingasa de Placas Pesado"; break;
				case 26: item = "Elmo"; break;
				case 27: item = "Braços de Couro"; break;
				case 28: item = "Braços de Corselete de Couro"; break;
				case 29: item = "Gorro de Couro"; break;
				case 30: item = "Peitoral de Couro"; break;
				case 31: item = "Do de Couro"; break;
				case 32: item = "Manoplas de Couro"; break;
				case 33: item = "Gorgueira de Couro"; break;
				case 34: item = "Haidate de Couro"; break;
				case 35: item = "HiroSode de Couro"; break;
				case 36: item = "Jingasa de Couro"; break;
				case 37: item = "Perneiras de Couro"; break;
				case 38: item = "Mempo de Couro"; break;
				case 39: item = "Capuz de Ninja de Couro"; break;
				case 40: item = "Jaqueta de Ninja de Couro"; break;
				case 41: item = "Manoplas de Ninja de Couro"; break;
				case 42: item = "Calças de Ninja de Couro"; break;
				case 43: item = "Calções de Couro"; break;
				case 44: item = "Saia de Couro"; break;
				case 45: item = "Suneate de Couro"; break;
				case 46: item = "Jingasa de Placas Leve"; break;
				case 47: item = "Escudo de Pipas de Metal"; break;
				case 48: item = "Escudo de Metal"; break;
				case 49: item = "Elmo Nórdico"; break;
				case 50: item = "Elmo de Chifres"; break;
				case 51: item = "Escudo da Ordem"; break;
				case 52: item = "Braços de Placas"; break;
				case 53: item = "Kabuto de Batalha de Placas"; break;
				case 54: item = "Peitoral de Placas"; break;
				case 55: item = "Do de Placas"; break;
				case 56: item = "Manoplas de Placas"; break;
				case 57: item = "Gorgueira de Placas"; break;
				case 58: item = "Haidate de Placas"; break;
				case 59: item = "Hatsuburi de Placas"; break;
				case 60: item = "Elmo de Placas"; break;
				case 61: item = "Hiro Sode de Placas"; break;
				case 62: item = "Perneiras de Placas"; break;
				case 63: item = "Mempo de Placas"; break;
				case 64: item = "Suneate de Placas"; break;
				case 65: item = "Elmo de Corvo"; break;
				case 66: item = "Braços de Cota de Malha de Anéis"; break;
				case 67: item = "Peitoral de Cota de Malha de Anéis"; break;
				case 68: item = "Manoplas de Cota de Malha de Anéis"; break;
				case 69: item = "Perneiras de Cota de Malha de Anéis"; break;
				case 70: item = "Braços Reais"; break;
				case 71: item = "Botas Reais"; break;
				case 72: item = "Peitoral Real"; break;
				case 73: item = "Diadema Real"; break;
				case 74: item = "Manoplas Reais"; break;
				case 75: item = "Gorgueira Real"; break;
				case 76: item = "Elmo Real"; break;
				case 77: item = "Perneiras Reais"; break;
				case 78: item = "Escudo Real"; break;
				case 79: item = "Jingasa de Placas Pequeno"; break;
				case 80: item = "Kabuto de Placas Padrão"; break;
				case 81: item = "Escudo de Aço"; break;
				case 82: item = "Braços de Couro Reforçado"; break;
				case 83: item = "Braços de Corselete de Couro Reforçado"; break;
				case 84: item = "Peitoral de Couro Reforçado"; break;
				case 85: item = "Do de Couro Reforçado"; break;
				case 86: item = "Manoplas de Couro Reforçado"; break;
				case 87: item = "Gorgueira de Couro Reforçado"; break;
				case 88: item = "Haidate de Couro Reforçado"; break;
				case 89: item = "Hiro Sode de Couro Reforçado"; break;
				case 90: item = "Perneiras de Couro Reforçado"; break;
				case 91: item = "Mempo de Couro Reforçado"; break;
				case 92: item = "Suneate de Couro Reforçado"; break;
				case 93: item = "Elmo de Abutre"; break;
				case 94: item = "Elmo Alado"; break;
				case 95: item = "Escudo de Pipas de Madeira"; break;
				case 96: item = "Braços de Placas de Madeira"; break;
				case 97: item = "Peitoral de Placas de Madeira"; break;
				case 98: item = "Manoplas de Placas de Madeira"; break;
				case 99: item = "Gorgueira de Placas de Madeira"; break;
				case 100: item = "Elmo de Placas de Madeira"; break;
				case 101: item = "Perneiras de Placas de Madeira"; break;
				case 102: item = "Escudo de Madeira"; break;
				case 103: item = "Adaga de Assassino"; break;
				case 104: item = "Espada de Assassino"; break;
				case 105: item = "Machado"; break;
				case 106: item = "Machado Bárbaro"; break;
				case 107: item = "Alabarda"; break;
				case 108: item = "Machado de Batalha"; break;
				case 109: item = "Maça de Batalha"; break;
				case 110: item = "Cajado de Mago"; break;
				case 111: item = "Cajado Laminado"; break;
				case 112: item = "Bokuto"; break;
				case 113: item = "Foice"; break;
				case 114: item = "Arco"; break;
				case 115: item = "Espada Larga"; break;
				case 116: item = "Faca de Açougueiro"; break;
				case 117: item = "Cutelo"; break;
				case 118: item = "Porrete"; break;
				case 119: item = "Arco Composto"; break;
				case 120: item = "Lâmina Crescente"; break;
				case 121: item = "Besta"; break;
				case 122: item = "Cutelo"; break;
				case 123: item = "Adaga"; break;
				case 124: item = "Daisho"; break;
				case 125: item = "Machado Duplo"; break;
				case 126: item = "Cajado de Lâmina Dupla"; break;
				case 127: item = "Cajado de Druida"; break;
				case 128: item = "Machado de Carrasco"; break;
				case 129: item = "Falcão"; break;
				case 130: item = "Cajado Nodoso"; break;
				case 131: item = "Alabarda"; break;
				case 132: item = "Martelo Picareta"; break;
				case 133: item = "Machadinha"; break;
				case 134: item = "Besta Pesada"; break;
				case 135: item = "Kama"; break;
				case 136: item = "Katana"; break;
				case 137: item = "Kryss"; break;
				case 138: item = "Lajatang"; break;
				case 139: item = "Lança"; break;
				case 140: item = "Machado de Batalha Grande"; break;
				case 141: item = "Espada Longa"; break;
				case 142: item = "Maça"; break;
				case 143: item = "Machete"; break;
				case 144: item = "Malho"; break;
				case 145: item = "NoDachi"; break;
				case 146: item = "Nunchaku"; break;
				case 147: item = "Picareta"; break;
				case 148: item = "Pique"; break;
				case 149: item = "Luvas de Pugilista"; break;
				case 150: item = "Bastão"; break;
				case 151: item = "Besta de Repetição"; break;
				case 152: item = "Espada Real"; break;
				case 153: item = "Sai"; break;
				case 154: item = "Cetro"; break;
				case 155: item = "Cimitarra"; break;
				case 156: item = "Foice"; break;
				case 157: item = "Cajado de Pastor"; break;
				case 158: item = "Rapieira"; break;
				case 159: item = "Faca de Esfolar"; break;
				case 160: item = "Lança"; break;
				case 161: item = "Arco Longo da Floresta"; break;
				case 162: item = "Arco Curto da Floresta"; break;
				case 163: item = "Tekagi"; break;
				case 164: item = "Tessen"; break;
				case 165: item = "Tetsubo"; break;
				case 166: item = "Espada"; break;
				case 167: item = "Lança Tribal"; break;
				case 168: item = "Tridente"; break;
				case 169: item = "Machado de Duas Mãos"; break;
				case 170: item = "Espada Bárbara"; break;
				case 171: item = "Wakizashi"; break;
				case 172: item = "Machado de Guerra"; break;
				case 173: item = "Lâminas de Guerra"; break;
				case 174: item = "Cutelo de Guerra"; break;
				case 175: item = "Adaga de Guerra"; break;
				case 176: item = "Forquilha de Guerra"; break;
				case 177: item = "Martelo de Guerra"; break;
				case 178: item = "Maça de Guerra"; break;
				case 179: item = "Yumi"; break;
				case 180: item = "Bandana"; break;
				case 181: item = "Máscara de Urso"; break;
				case 182: item = "Cinto"; break;
				case 183: item = "Faixa Corporal"; break;
				case 184: item = "Boné"; break;
				case 185: item = "Botas"; break;
				case 186: item = "Gorro"; break;
				case 187: item = "Capa"; break;
				case 188: item = "Capuz de Ninja de Pano"; break;
				case 189: item = "Jaqueta de Ninja de Pano"; break;
				case 190: item = "Máscara de Veado"; break;
				case 191: item = "Gibão"; break;
				case 192: item = "Botas Elegantes"; break;
				case 193: item = "Vestido Elegante"; break;
				case 194: item = "Camisa Elegante"; break;
				case 195: item = "Chapéu de Penas"; break;
				case 196: item = "Kimono Feminino"; break;
				case 197: item = "Veste Feminina"; break;
				case 198: item = "Chapéu Mole"; break;
				case 199: item = "Guirlanda de Flores"; break;
				case 200: item = "Camisa Formal"; break;
				case 201: item = "Avental Completo"; break;
				case 202: item = "Vestido Dourado"; break;
				case 203: item = "Hakama"; break;
				case 204: item = "Hakama Shita"; break;
				case 205: item = "Meio Avental"; break;
				case 206: item = "Máscara Tribal com Chifres"; break;
				case 207: item = "Chapéu de Bufão"; break;
				case 208: item = "Traje de Bufão"; break;
				case 209: item = "Jin Baori"; break;
				case 210: item = "Kamishimo"; break;
				case 211: item = "Kasa"; break;
				case 212: item = "Kilt"; break;
				case 213: item = "Tanga"; break;
				case 214: item = "Calças Compridas"; break;
				case 215: item = "Kimono Masculino"; break;
				case 216: item = "Tabi de Ninja"; break;
				case 217: item = "Obi"; break;
				case 218: item = "Vestido Simples"; break;
				case 219: item = "Veste"; break;
				case 220: item = "Capa Real"; break;
				case 221: item = "Tabi de Samurai"; break;
				case 222: item = "Sandálias"; break;
				case 223: item = "Camisa"; break;
				case 224: item = "Sapatos"; break;
				case 225: item = "Calças Curtas"; break;
				case 226: item = "Saia"; break;
				case 227: item = "Gorro de Caveira"; break;
				case 228: item = "Chapéu de Palha"; break;
				case 229: item = "Sobretúnica"; break;
				case 230: item = "Chapéu de Palha Alto"; break;
				case 231: item = "Tattsuke Hakama"; break;
				case 232: item = "Botas de Cano Alto"; break;
				case 233: item = "Máscara Tribal"; break;
				case 234: item = "Chapéu Tricórnio"; break;
				case 235: item = "Túnica"; break;
				case 236: item = "Waraji"; break;
				case 237: item = "Chapéu de Aba Larga"; break;
				case 238: item = "Chapéu de Mago"; break;
				case 239: item = "Vela"; break;
				case 240: item = "Colar de Contas de Ouro"; break;
				case 241: item = "Bracelete de Ouro"; break;
				case 242: item = "Brincos de Ouro"; break;
				case 243: item = "Colar de Ouro"; break;
				case 244: item = "Anel de Ouro"; break;
				case 245: item = "Lanterna"; break;
				case 246: item = "Colar"; break;
				case 247: item = "Colar de Contas de Prata"; break;
				case 248: item = "Bracelete de Prata"; break;
				case 249: item = "Brincos de Prata"; break;
				case 250: item = "Colar de Prata"; break;
				case 251: item = "Anel de Prata"; break;
				case 252: item = "Talismã"; break;
				case 253: item = "Tocha"; break;
			}

			return item;
		}

		///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		public static string GetRandomShipName( string captain, int lower )
		{
			string sNumber = Utility.RandomMinMax( 3, 12 ).ToString();

			string[] vName1 = new string[] {"de Achelous", "de Aegaeon", "de Alpheus", "Irado", "Horrível", "Negro", "Sangrento", "Azul", "Latão", "do Bucaneiro", "de Calypso", "do Capitão", "Coral", "Cruel", "Choroso", "Amaldiçoado", "Condenado", "Sombrio", "de Davy Jones", "Mortífero", "Enganador", "de Delfim", "do Diabo", "Sujo", "Vergonhoso", "Desonrado", "Desonroso", "do Dragão", "Sonhador", "Esmeralda", "de Eurybia", "Maligno", "do Carrasco", "Caído", "Esquecido", "Imundo", "Gentil", "Dourado", "Cinzento", "Ganancioso", "Verde", "de Hades", "Odioso", "Assombrado", "Infernal", "Uivante", "Jade", "do Assassino", "do Patife", "Perdido", "Ameaçador", "Mórbido", "do Assassino", "de Netuno", "de Nereu", "da Noite", "do Oceano", "de Oceano", "do Pirata", "do Saqueador", "Veneno", "de Poseidon", "Orgulhoso", "do Corsário", "de Proteu", "Enfurecido", "Vermelho", "Real", "Rubi", "do Marinheiro", "Safira", "Selvagem", "Gritante", "Buscador", "do Mar", "da Serpente", "Vergonhoso", "Chilreante", "Prata", "da Cobra", "Firme", "Viajante", "de Tritão", "Vil", "Andarilho", "Branco", "Amarelo"};
			string sName1 = vName1[Utility.RandomMinMax( 0, (vName1.Length-1) )];

			if ( captain != "" && captain != null ){ sName1 = captain; }

			string[] vName2 = new string[] {"Âncora", "Ira", "Craca", "Lâmina", "Bucaneiro", "Capitão", "Coral", "Ossos Cruzados", "Crueldade", "Cutelo", "Cortador", "Adaga", "Danação", "Morte", "Demônio", "Diabo", "Desonra", "Perdição", "Sonho", "Carrasco", "Medo", "Vendaval", "Galeão", "Cálice", "Ódio", "Chifre", "Horror", "Furacão", "Insanidade", "Jóia", "Assassino", "Patife", "Faca", "Relâmpago", "Sereia", "Assassino", "Mistério", "Noite", "Pesadelo", "Pérola", "Pirata", "Veneno", "Corsário", "Saqueador", "Sabre", "Vela", "Grito", "Segredo", "Serpente", "Servo", "Tubarão", "Navio", "Caveira", "Escravo", "Tempestade", "Cortesã", "Sol", "Espada", "Trovão", "Tesouro", "Tridente", "Baleia", "Redemoinho", "Cortesã"};
			string sName2 = vName2[Utility.RandomMinMax( 0, (vName2.Length-1) )];

			string sName3 = "";
			switch( Utility.RandomMinMax( 1, 120 ) )
			{
				case 1: sName3 = " da Capa"; break;
				case 2: sName3 = " da Costa"; break;
				case 3: sName3 = " dos Condenados"; break;
				case 4: sName3 = " do Sombrio"; break;
				case 5: sName3 = " do Diabo"; break;
				case 6: sName3 = " do Leste"; break;
				case 7: sName3 = " dos Deuses"; break;
				case 8: sName3 = " do Elmo"; break;
				case 9: sName3 = " das " + sNumber + " Ilhas"; break;
				case 10: sName3 = " das Ilhas"; break;
				case 11: sName3 = " da Luz"; break;
				case 12: sName3 = " da Noite"; break;
				case 13: sName3 = " do Norte"; break;
				case 14: sName3 = " do Oceano"; break;
				case 15: sName3 = " do Recife"; break;
				case 16: sName3 = " dos Justos"; break;
				case 17: sName3 = " do Mar"; break;
				case 18: sName3 = " dos " + sNumber + " Mares"; break;
				case 19: sName3 = " do Escudo"; break;
				case 20: sName3 = " da Costa"; break;
				case 21: sName3 = " do Sul"; break;
				case 22: sName3 = " da Tempestade"; break;
				case 23: sName3 = " da Espada"; break;
				case 24: sName3 = " da Lâmina"; break;
				case 25: sName3 = " dos Trópicos"; break;
				case 26: sName3 = " das Ondas"; break;
				case 27: sName3 = " do Oeste"; break;
				case 28: sName3 = " dos Ventos"; break;
				case 29: sName3 = " dos Doca"; break;
				case 30: sName3 = " do Cais"; break;
				case 31: sName3 = " das " + sNumber + " Lâminas"; break;
				case 32: sName3 = " das " + sNumber + " Espadas"; break;
				case 33: sName3 = " dos " + sNumber + " Deuses"; break;
				case 34: sName3 = " das " + sNumber + " Tempestades"; break;
				case 35: sName3 = " das " + sNumber + " Costas"; break;
				case 36: sName3 = " dos " + sNumber + " Escudos"; break;
				case 37: sName3 = " das " + sNumber + " Bandeiras"; break;
				case 38: sName3 = " das " + sNumber + " Costas"; break;
			}
			if ( lower > 0 ){ return sName2 + " " + sName3 + " " + sName1; }

			return sName2 + " " + sName3 + " " + sName1;
		}

		///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		public static string RandomMagicalItem()
		{
			string sAdjective = "incomum";
			string eAdjective = "poder";

			sAdjective = RandomThings.MagicItemAdj( "start", false, false, 0 );
			eAdjective = RandomThings.MagicItemAdj( "end", false, false, 0 );

			sAdjective = System.Globalization.CultureInfo.InvariantCulture.TextInfo.ToTitleCase(sAdjective);
			eAdjective = System.Globalization.CultureInfo.InvariantCulture.TextInfo.ToTitleCase(eAdjective);

			string name = GetRandomArmorWeaponItem();

			switch( Utility.RandomMinMax( 0, 1 ) )
			{
				case 0: name = name +  " " +  sAdjective + " do(a) " + eAdjective;	break;
				case 1: name = name + " do(a) " + eAdjective;						break;
			}

			return name;
		}

		///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		public static string GetRandomName()
		{
			string name = NameList.RandomName( "male" );

			switch( Utility.RandomMinMax( 1, 29 ) )
			{
				case 1: name = NameList.RandomName( "vampire" ); break;
				case 2: name = NameList.RandomName( "drakkul" ); break;
				case 3: name = NameList.RandomName( "imp" ); break;
				case 4: name = NameList.RandomName( "druid" ); break;
				case 5: name = NameList.RandomName( "ork" ); break;
				case 6: name = NameList.RandomName( "dragon" ); break;
				case 7: name = NameList.RandomName( "goddess" ); break;
				case 8: name = NameList.RandomName( "demonic" ); break;
				case 9: name = NameList.RandomName( "ork_male" ); break;
				case 10: name = NameList.RandomName( "ork_female" ); break;
				case 11: name = NameList.RandomName( "barb_male" ); break;
				case 12: name = NameList.RandomName( "barb_female" ); break;
				case 13: name = NameList.RandomName( "ancient lich" ); break;
				case 14: name = NameList.RandomName( "demon knight" ); break;
				case 15: name = NameList.RandomName( "shadow knight" ); break;
				case 16: name = NameList.RandomName( "gargoyle vendor" ); break;
				case 17: name = NameList.RandomName( "gargoyle name" ); break;
				case 18: name = NameList.RandomName( "centaur" ); break;
				case 19: name = NameList.RandomName( "pixie" ); break;
				case 20: name = NameList.RandomName( "golem controller" ); break;
				case 21: name = NameList.RandomName( "daemon" ); break;
				case 22: name = NameList.RandomName( "devil" ); break;
				case 23: name = NameList.RandomName( "evil mage" ); break;
				case 24: name = NameList.RandomName( "evil witch" ); break;
				case 25: name = NameList.RandomName( "elf_male" ); break;
				case 26: name = NameList.RandomName( "elf_female" ); break;
				case 27: name = NameList.RandomName( "female" ); break;
				case 28: name = NameList.RandomName( "male" ); break;
				case 29: name = NameList.RandomName( "greek" ); break;
			}

			if ( name == null || name == "" ){ name = NameList.RandomName( "male" ); }

			return name;
		}

		public static string GetRandomGirlName()
		{
			string name = NameList.RandomName( "female" );

			switch( Utility.RandomMinMax( 0, 5 ) )
			{
				case 1: name = NameList.RandomName( "ork_female" ); break;
				case 2: name = NameList.RandomName( "barb_female" ); break;
				case 3: name = NameList.RandomName( "evil witch" ); break;
				case 4: name = NameList.RandomName( "elf_female" ); break;
				case 5: name = NameList.RandomName( "tokuno female" ); break;
			}

			return name;
		}

		public static string GetRandomBoyName()
		{
			string name = NameList.RandomName( "male" );

			switch( Utility.RandomMinMax( 0, 5 ) )
			{
				case 1: name = NameList.RandomName( "ork_male" ); break;
				case 2: name = NameList.RandomName( "barb_male" ); break;
				case 3: name = NameList.RandomName( "evil mage" ); break;
				case 4: name = NameList.RandomName( "elf_male" ); break;
				case 5: name = NameList.RandomName( "tokuno male" ); break;
			}

			return name;
		}

		public static string GetRandomCharacterName()
		{
			string name = NameList.RandomName( "female" );

			switch( Utility.RandomMinMax( 0, 11 ) )
			{
				case 1: name = NameList.RandomName( "ork_female" ); break;
				case 2: name = NameList.RandomName( "barb_female" ); break;
				case 3: name = NameList.RandomName( "evil witch" ); break;
				case 4: name = NameList.RandomName( "elf_female" ); break;
				case 5: name = NameList.RandomName( "tokuno female" ); break;
				case 6: name = NameList.RandomName( "ork_male" ); break;
				case 7: name = NameList.RandomName( "barb_male" ); break;
				case 8: name = NameList.RandomName( "evil mage" ); break;
				case 9: name = NameList.RandomName( "elf_male" ); break;
				case 10: name = NameList.RandomName( "tokuno male" ); break;
				case 11: name = NameList.RandomName( "male" ); break;
			}

			return name;
		}

		///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		public static string GetRandomWizardName()
		{
			string name = NameList.RandomName( "ancient lich" );

			switch( Utility.RandomMinMax( 1, 6 ) )
			{
				case 1: name = NameList.RandomName( "ancient lich" ); break;
				case 2: name = NameList.RandomName( "vampire" ); break;
				case 3: name = NameList.RandomName( "greek" ); break;
				case 4: name = NameList.RandomName( "drakkul" ); break;
				case 5: name = NameList.RandomName( "evil mage" ); break;
				case 6: name = NameList.RandomName( "evil witch" ); break;
			}

			if ( name == null || name == "" ){ name = NameList.RandomName( "male" ); }

			return name;
		}

		///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		public static string GetBoyGirlJob( int gender )
		{
			string girlJob = "Curandeira";
			string boyJob = "Curandeiro";

			switch( Utility.RandomMinMax( 1, 46 ) )
			{
				case 1: girlJob = "Aventureira"; boyJob = "Aventureiro"; break;
				case 2: girlJob = "Bandida"; boyJob = "Bandido"; break;
				case 3: girlJob = "Bárbara"; boyJob = "Bárbaro"; break;
				case 4: girlJob = "Barda"; boyJob = "Bardo"; break;
				case 5: girlJob = "Amazona"; boyJob = "Cavaleiro"; break;
				case 6: girlJob = "Clériga"; boyJob = "Clérigo"; break;
				case 7: girlJob = "Conjuradora"; boyJob = "Conjurador"; break;
				case 8: girlJob = "Defensora"; boyJob = "Defensor"; break;
				case 9: girlJob = "Adivinha"; boyJob = "Adivinho"; break;
				case 10: girlJob = "Druida"; boyJob = "Druida"; break;
				case 11: girlJob = "Encantadora"; boyJob = "Encantador"; break;
				case 12: girlJob = "Exploradora"; boyJob = "Explorador"; break;
				case 13: girlJob = "Lutadora"; boyJob = "Lutador"; break;
				case 14: girlJob = "Gladiadora"; boyJob = "Gladiador"; break;
				case 15: girlJob = "Herege"; boyJob = "Herege"; break;
				case 16: girlJob = "Caçadora"; boyJob = "Caçador"; break;
				case 17: girlJob = "Ilusionista"; boyJob = "Ilusionista"; break;
				case 18: girlJob = "Invocadora"; boyJob = "Invocador"; break;
				case 19: girlJob = "Cavaleira"; boyJob = "Cavaleiro"; break;
				case 20: girlJob = "Maga"; boyJob = "Mago"; break;
				case 21: girlJob = "Mágica"; boyJob = "Mágico"; break;
				case 22: girlJob = "Mercenária"; boyJob = "Mercenário"; break;
				case 23: girlJob = "Menestrela"; boyJob = "Menestrel"; break;
				case 24: girlJob = "Monge"; boyJob = "Monge"; break;
				case 25: girlJob = "Mística"; boyJob = "Místico"; break;
				case 26: girlJob = "Necromante"; boyJob = "Necromante"; break;
				case 27: girlJob = "Fora-da-lei"; boyJob = "Fora-da-lei"; break;
				case 28: girlJob = "Paladina"; boyJob = "Paladino"; break;
				case 29: girlJob = "Sacerdotisa"; boyJob = "Sacerdote"; break;
				case 30: girlJob = "Profetisa"; boyJob = "Profeta"; break;
				case 31: girlJob = "Ranger"; boyJob = "Ranger"; break;
				case 32: girlJob = "Ladra"; boyJob = "Ladrão"; break;
				case 33: girlJob = "Sábia"; boyJob = "Sábio"; break;
				case 34: girlJob = "Batedora"; boyJob = "Batedor"; break;
				case 35: girlJob = "Buscadora"; boyJob = "Buscador"; break;
				case 36: girlJob = "Vidente"; boyJob = "Vidente"; break;
				case 37: girlJob = "Xamã"; boyJob = "Xamã"; break;
				case 38: girlJob = "Abatedora"; boyJob = "Abatedor"; break;
				case 39: girlJob = "Feiticeira"; boyJob = "Feiticeiro"; break;
				case 40: girlJob = "Invocadora"; boyJob = "Invocador"; break;
				case 41: girlJob = "Templária"; boyJob = "Templário"; break;
				case 42: girlJob = "Ladra"; boyJob = "Ladrão"; break;
				case 43: girlJob = "Viajante"; boyJob = "Viajante"; break;
				case 44: girlJob = "Guerreira"; boyJob = "Bruxo"; break;
				case 45: girlJob = "Bruxa"; boyJob = "Guerreiro"; break;
				case 46: girlJob = "Maga"; boyJob = "Mago"; break;
			}

			if ( gender == 1 ){ return girlJob; }

			return boyJob;
		}

		///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		public static string GetRandomKingdomName()
		{
			string name = NameList.RandomName( "vampire" );

			switch( Utility.RandomMinMax( 1, 27 ) )
			{
				case 1: name = NameList.RandomName( "vampire" ); break;
				case 2: name = NameList.RandomName( "drakkul" ); break;
				case 3: name = NameList.RandomName( "imp" ); break;
				case 4: name = NameList.RandomName( "druid" ); break;
				case 5: name = NameList.RandomName( "ork" ); break;
				case 6: name = NameList.RandomName( "dragon" ); break;
				case 7: name = NameList.RandomName( "goddess" ); break;
				case 8: name = NameList.RandomName( "demonic" ); break;
				case 9: name = NameList.RandomName( "ork_male" ); break;
				case 10: name = NameList.RandomName( "ork_female" ); break;
				case 11: name = NameList.RandomName( "barb_male" ); break;
				case 12: name = NameList.RandomName( "barb_female" ); break;
				case 13: name = NameList.RandomName( "ancient lich" ); break;
				case 14: name = NameList.RandomName( "demon knight" ); break;
				case 15: name = NameList.RandomName( "shadow knight" ); break;
				case 16: name = NameList.RandomName( "gargoyle vendor" ); break;
				case 17: name = NameList.RandomName( "gargoyle name" ); break;
				case 18: name = NameList.RandomName( "centaur" ); break;
				case 19: name = NameList.RandomName( "pixie" ); break;
				case 20: name = NameList.RandomName( "golem controller" ); break;
				case 21: name = NameList.RandomName( "lizardman" ); break;
				case 22: name = NameList.RandomName( "devil" ); break;
				case 23: name = NameList.RandomName( "evil mage" ); break;
				case 24: name = NameList.RandomName( "evil witch" ); break;
				case 25: name = NameList.RandomName( "elf_male" ); break;
				case 26: name = NameList.RandomName( "elf_female" ); break;
				case 27: name = NameList.RandomName( "greek" ); break;
			}

			return name;
		}

		public static string GetRandomKingdom()
		{
			string kingdom = "Reino";

			switch( Utility.RandomMinMax( 1, 13 ) )
			{
				case 1: kingdom = "Reino"; break;
				case 2: kingdom = "Dinastia"; break;
				case 3: kingdom = "Império"; break;
				case 4: kingdom = "Domínio"; break;
				case 5: kingdom = "Soberania"; break;
				case 6: kingdom = "Regime"; break;
				case 7: kingdom = "Reinado"; break;
				case 8: kingdom = "Nação"; break;
				case 9: kingdom = "Monarquia"; break;
				case 10: kingdom = "Reino"; break;
				case 11: kingdom = "Território"; break;
				case 12: kingdom = "Terras"; break;
				case 13: kingdom = "Ilhas"; break;
			}

			return kingdom;
		}

		///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		public static string GetRandomOrientalName()
		{
			string name = NameList.RandomName( "tokuno male" );

			switch( Utility.RandomMinMax( 1, 4 ) )
			{
				case 1: name = NameList.RandomName( "tokuno male" ); break;
				case 2: name = NameList.RandomName( "tokuno female" ); break;
				case 3: name = NameList.RandomName( "drakkul" ); break;
				case 4: name = NameList.RandomName( "goddess" ); break;
			}

			if ( name == null || name == "" ){ name = NameList.RandomName( "tokuno male" ); }

			return name;
		}

		///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		public static string GetRandomOrientalNation()
		{
			string name = NameList.RandomName( "dark_elf_prefix_female" );

			switch( Utility.RandomMinMax( 1, 2 ) )
			{
				case 1: name = NameList.RandomName( "dark_elf_prefix_female" ); break;
				case 2: name = NameList.RandomName( "dark_elf_prefix_male" ); break;
			}

			if ( name == null || name == "" ){ name = NameList.RandomName( "dark_elf_prefix_female" ); }

			switch( Utility.RandomMinMax( 1, 2 ) )
			{
				case 1: name = name + "anese"; break;
				case 2: name = name + "ist"; break;
			}

			return name;
		}

		///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		public static string GetRandomBookType( bool caps )
		{
			string book = "livro";

			int tome = Utility.RandomMinMax( 0, 6 );
			if ( caps ){ Utility.RandomMinMax( 7, 13 ); }

			switch ( tome ) 
			{
				case 0 : book = "livro"; break;
				case 1 : book = "léxico"; break;
				case 2 : book = "compêndio"; break;
				case 3 : book = "manual"; break;
				case 4 : book = "fólio"; break;
				case 5 : book = "códex"; break;
				case 6 : book = "tomo"; break;
				case 7 : book = "Livro"; break;
				case 8 : book = "Léxico"; break;
				case 9 : book = "Compêndio"; break;
				case 10 : book = "Manual"; break;
				case 11 : book = "Fólio"; break;
				case 12 : book = "Códex"; break;
				case 13 : book = "Tomo"; break;
			}

			return book;
		}

		///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		public static string GetBookTitle()
		{
			string bookTitle = "o Livro dos Mortos";

			string[] vName1 = new string[] {"Exótico", "Misterioso", "Encantado", "Maravilhoso", "Espantoso", "Surpreendente", "Místico", "Estupendo", "Mágico", "Divino", "Excelente", "Magnífico", "Fenomenal", "Fantástico", "Incrível", "Milagroso", "Extraordinário", "Fabuloso", "Prodigioso", "Glorioso", "Temível", "Horrífico", "Terrible", "Perturbador", "Amedrontador", "Horrível", "Funesto", "Sombrio", "Vil", "Perdido", "Lendário", "Mítico", "Desaparecido", "Condenado", "Infinito", "Eterno", "Exaltado", "Cintilante", "Sádico", "Perturbador", "Espiritual", "Demoníaco", "Santo", "Celestial", "Ancestral", "Ornado", "Supremo", "Abissal", "Enlouquecido", "Élfico", "Orc", "Anão", "Gnômico", "Amaldiçoado", "Silvestre", "Feiticeiro", "Robusto", "Estranho", "Raro", "Prezado", "Maldito", "Maligno", "Ordeiro", "Imundo", "Infernal", "Real", "Mundano", "Blasfemo", "Planar", "Maravilhoso", "Perfeito", "Cruel", "Caótico", "Assombrado", "Viajante", "Profano", "Infernal", "Vil", "Amaldiçoado", "Demoníaco", "Adorado", "Sagrado", "Glorificado", "Sacro", "Bem-aventurado", "Todo-poderoso", "Dominante", "Supremo", "Caído", "Sombrio", "Terrestre", "Poderoso", "Indizível", "Desconhecido", "Esquecido", "Mortífero", "Morto-vivo", "Infinito", "Abissal"};
			string sName1 = vName1[Utility.RandomMinMax( 0, (vName1.Length-1) )];

			string[] vName2 = new string[] {"Conto", "Livro", "Aventuras", "Léxico", "Escritos", "Compêndio", "Mistério", "Manual", "Fólio", "Diário", "Tomo", "História", "Eventos", "História", "Crônicas", "Fábula", "Lenda", "Mito", "Segredos"};
			string sName2 = vName2[Utility.RandomMinMax( 0, (vName2.Length-1) )];

			string[] vName3 = new string[] {"Demônio", "Diabo", "Dragão", "Anão", "Elfo", "Bruxa", "Hobbit", "Diabrete", "Leprechaun", "Vampiro", "Fantasma", "Lich", "Templário", "Ladrão", "Ilusionista", "Princesa", "Invocador", "Sacerdote", "Conjurador", "Bandido", "Sacerdotisa", "Barão", "Mago", "Clérigo", "Monge", "Menestrel", "Defensor", "Cavaleiro", "Mágico", "Bruxa", "Lutador", "Buscador", "Abatedor", "Ranger", "Bárbaro", "Explorador", "Herege", "Gladiador", "Sábio", "Ladrão", "Paladino", "Bardo", "Adivinho", "Senhor", "Fora-da-lei", "Profeta", "Mercenário", "Aventureiro", "Encantador", "Rei", "Batedor", "Místico", "Mago", "Viajante", "Invocador", "Rainha", "Guerreiro", "Feiticeiro", "Vidente", "Caçador", "Cavaleiro", "Príncipe", "Necromante", "Feiticeira", "Xamã"};
			string sName3 = vName3[Utility.RandomMinMax( 0, (vName3.Length-1) )];

			string[] vName4 = new string[] {"Texugo", "Basilisco", "Urso", "Javali", "Búfalo", "Bugbear", "Touro", "Centauro", "Quimera", "Gigante das Nuvens", "Crocodilo", "Ciclope", "Demônio", "Diabo", "Cão", "Dragão", "Drake", "Dríade", "Anão", "Elefante", "Elfo", "Ettin", "Gigante do Fogo", "Peixe", "Sapo", "Gigante do Gelo", "Gárgula", "Gênio", "Gnoll", "Gnomo", "Goblin", "Górgona", "Grifo", "Bruxa", "Hobbit", "Harpia", "Cão do Inferno", "Gigante das Colinas", "Hipogrifo", "Hipopótamo", "Hobbit", "Hobgoblin", "Cavalo", "Hidra", "Diabrete", "Chacal", "Kobold", "Kraken", "Leprechaun", "Leão", "Lagarto", "Mantícora", "Diabrete", "Minotauro", "Mula", "Naga", "Nixie", "Ninfa", "Sapomem", "Ogro", "Orc", "Corujurso", "Pégaso", "Fênix", "Pixie", "Verme Gigante", "Pixie Sombria", "Monstro Podre", "Escorpião", "Serpente", "Ceifador", "Cobra", "Esfinge", "Aranha", "Sprite", "Gigante de Pedra", "Gigante das Tempestades", "Súcubo", "Tigre", "Titã", "Sapo", "Ent", "Neptar", "Troglodita", "Troll", "Tartaruga", "Unicórnio", "Morsa", "Doninha", "Lobisomem", "Baleia", "Wisp", "Lobo", "Carcaju", "Wyrm", "Wyvern", "Zorn", "Yeti", "Templário", "Ladrão", "Ilusionista", "Princesa", "Invocador", "Sacerdote", "Conjurador", "Bandido", "Sacerdotisa", "Barão", "Mago", "Clérigo", "Monge", "Menestrel", "Defensor", "Cavaleiro", "Mágico", "Bruxa", "Lutador", "Buscador", "Abatedor", "Ranger", "Bárbaro", "Explorador", "Herege", "Gladiador", "Sábio", "Ladrão", "Paladino", "Bardo", "Adivinho", "Senhor", "Fora-da-lei", "Profeta", "Mercenário", "Aventureiro", "Encantador", "Rei", "Batedor", "Místico", "Mago", "Viajante", "Invocador", "Rainha", "Guerreiro", "Feiticeiro", "Vidente", "Caçador", "Cavaleiro", "Príncipe", "Necromante", "Feiticeira", "Xamã"};
			string sName4 = vName4[Utility.RandomMinMax( 0, (vName4.Length-1) )];

			string[] vName5 = new string[] {"Castelo", "Caverna", "Mansão", "Casa", "Caverna", "Masmorra", "Floresta", "Deserto", "Torre", "Deserto", "Montanhas", "Pântano", "Colinas", "Noite", "Escuridão", "Nevoeiro", "Bosque", "Névoa", "Luz", "Garrafa", "Céu", "Chão", "Água", "Mar", "Areia", "Árvores", "Nuvens", "Estrelas", "Cristal", "Gema", "Lâmpada", "Jarro", "Correntes", "Fortaleza", "Cidade", "Vila", "Tumba", "Cripta"};
			string sName5 = vName5[Utility.RandomMinMax( 0, (vName5.Length-1) )];

			string sName6 = NameList.RandomName( "author" );

			string[] vName7 = new string[] {"Cálice", "Espada", "Machado", "Adaga", "Armadura", "Cristal", "Gema", "Poço", "Varinha", "Anel", "Amuleto", "Elmo", "Coroa", "Botas", "Cinto", "Veste", "Cálice", "Espelho", "Lança", "Escudo", "Cetro", "Cajado", "Livro", "Poção", "Arco", "Pedra", "Fogo", "Fragmento", "Caixa"};
			string sName7 = vName7[Utility.RandomMinMax( 0, (vName7.Length-1) )];

			string[] vName8 = new string[] {"Busca", "Missão", "Maldição", "Magia", "Mistério", "Poder", "Destruição", "Assassinato", "Desejo", "Natureza", "Lenda", "Mito", "Mentiras", "Localização"};
			string sName8 = vName8[Utility.RandomMinMax( 0, (vName8.Length-1) )];

			switch ( Utility.RandomMinMax( 0, 10 ) ) 
			{
				case 0: bookTitle = "O " + sName1 + " " + sName2 + " do " + sName4; break;
				case 1: bookTitle = "O " + sName2 + " do " + sName1 + " " + sName4; break;
				case 2: bookTitle = "O " + sName4 + " no " + sName5; break;
				case 3: bookTitle = "O " + sName2 + " do " + sName3 + " no " + sName5; break;
				case 4: bookTitle = "O " + sName1 + " " + sName5 + " do " + sName3; break;
				case 5: bookTitle = "A " + sName8 + " do " + sName1 + " " + sName7 + " de " + sName6; break;
				case 6: bookTitle = "A " + sName8 + " do " + sName7 + " de " + sName6; break;
				case 7: bookTitle = "O " + sName7 + " e o " + sName3; break;
				case 8: bookTitle = "O " + sName3 + " e o " + sName7; break;
				case 9: bookTitle = "O " + sName2 + " de " + sName6 + " o " + sName3; break;
				case 10: bookTitle = "O " + sName2 + " de " + sName6 + " o " + sName3; break;
			}

			return bookTitle;
		}

		///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		public static string GetSongTitle()
		{
			string bookTitle = "a Canção dos Mortos";

			string[] vName1 = new string[] {"Exótica", "Misteriosa", "Encantada", "Maravilhosa", "Espantosa", "Surpreendente", "Mística", "Estupenda", "Mágica", "Divina", "Excelente", "Magnífica", "Fenomenal", "Fantástica", "Incrível", "Milagrosa", "Extraordinária", "Fabulosa", "Prodigiosa", "Gloriosa", "Temível", "Horrífica", "Terrível", "Perturbadora", "Amedrontadora", "Horrível", "Funesta", "Sombria", "Vil", "Perdida", "Lendária", "Mítica", "Desaparecida", "Condenada", "Infinito", "Eterno", "Exaltado", "Cintilante", "Sádica", "Perturbadora", "Espiritual", "Demoníaca", "Santa", "Celestial", "Ancestral", "Ornada", "Suprema", "Abissal", "Enlouquecida", "Élfica", "Orc", "Anã", "Gnômica", "Amaldiçoada", "Silvestre", "Feiticeira", "Robusta", "Estranha", "Rara", "Prezada", "Maldita", "Maligna", "Ordeira", "Imunda", "Infernal", "Real", "Mundana", "Blasfema", "Planar", "Maravilhosa", "Perfeita", "Cruel", "Caótica", "Assombrada", "Viajante", "Profana", "Infernal", "Vil", "Amaldiçoada", "Demoníaca", "Adorada", "Sagrada", "Glorificada", "Sacra", "Bem-aventurada", "Todo-poderosa", "Dominante", "Suprema", "Caída", "Sombria", "Terrestre", "Poderosa", "Indizível", "Desconhecida", "Esquecida", "Mortífera", "Morta-viva", "Infinito", "Abissal"};
			string sName1 = vName1[Utility.RandomMinMax( 0, (vName1.Length-1) )];

			string[] vName2 = new string[] {"Conto", "Lenda", "Aventuras", "Jornada", "Missão", "Mistério", "História", "Eventos", "História", "Crônicas", "Fábula", "Mito", "Segredos"};
			string sName2 = vName2[Utility.RandomMinMax( 0, (vName2.Length-1) )];

			string[] vName3 = new string[] {"Demônio", "Diabo", "Dragão", "Anão", "Elfo", "Bruxa", "Hobbit", "Diabrete", "Leprechaun", "Vampiro", "Fantasma", "Lich", "Templário", "Ladrão", "Ilusionista", "Princesa", "Invocador", "Sacerdote", "Conjurador", "Bandido", "Sacerdotisa", "Barão", "Mago", "Clérigo", "Monge", "Menestrel", "Defensor", "Cavaleiro", "Mágico", "Bruxa", "Lutador", "Buscador", "Abatedor", "Ranger", "Bárbaro", "Explorador", "Herege", "Gladiador", "Sábio", "Ladrão", "Paladino", "Bardo", "Adivinho", "Senhor", "Fora-da-lei", "Profeta", "Mercenário", "Aventureiro", "Encantador", "Rei", "Batedor", "Místico", "Mago", "Viajante", "Invocador", "Rainha", "Guerreiro", "Feiticeiro", "Vidente", "Caçador", "Cavaleiro", "Príncipe", "Necromante", "Feiticeira", "Xamã"};
			string sName3 = vName3[Utility.RandomMinMax( 0, (vName3.Length-1) )];

			string[] vName4 = new string[] {"Texugo", "Basilisco", "Urso", "Javali", "Búfalo", "Bugbear", "Touro", "Centauro", "Quimera", "Gigante das Nuvens", "Crocodilo", "Ciclope", "Demônio", "Diabo", "Cão", "Dragão", "Drake", "Dríade", "Anão", "Elefante", "Elfo", "Ettin", "Gigante do Fogo", "Peixe", "Sapo", "Gigante do Gelo", "Gárgula", "Gênio", "Gnoll", "Gnomo", "Goblin", "Górgona", "Grifo", "Bruxa", "Hobbit", "Harpia", "Cão do Inferno", "Gigante das Colinas", "Hipogrifo", "Hipopótamo", "Hobbit", "Hobgoblin", "Cavalo", "Hidra", "Diabrete", "Chacal", "Kobold", "Kraken", "Leprechaun", "Leão", "Lagarto", "Mantícora", "Diabrete", "Minotauro", "Mula", "Naga", "Nixie", "Ninfa", "Sapomem", "Ogro", "Orc", "Corujurso", "Pégaso", "Fênix", "Pixie", "Verme Gigante", "Pixie Sombria", "Monstro Podre", "Escorpião", "Serpente", "Ceifador", "Cobra", "Esfinge", "Aranha", "Sprite", "Gigante de Pedra", "Gigante das Tempestades", "Súcubo", "Tigre", "Titã", "Sapo", "Ent", "Neptar", "Troglodita", "Troll", "Tartaruga", "Unicórnio", "Morsa", "Doninha", "Lobisomem", "Baleia", "Wisp", "Lobo", "Carcaju", "Wyrm", "Wyvern", "Zorn", "Yeti", "Templário", "Ladrão", "Ilusionista", "Princesa", "Invocador", "Sacerdote", "Conjurador", "Bandido", "Sacerdotisa", "Barão", "Mago", "Clérigo", "Monge", "Menestrel", "Defensor", "Cavaleiro", "Mágico", "Bruxa", "Lutador", "Buscador", "Abatedor", "Ranger", "Bárbaro", "Explorador", "Herege", "Gladiador", "Sábio", "Ladrão", "Paladino", "Bardo", "Adivinho", "Senhor", "Fora-da-lei", "Profeta", "Mercenário", "Aventureiro", "Encantador", "Rei", "Batedor", "Místico", "Mago", "Viajante", "Invocador", "Rainha", "Guerreiro", "Feiticeiro", "Vidente", "Caçador", "Cavaleiro", "Príncipe", "Necromante", "Feiticeira", "Xamã"};
			string sName4 = vName4[Utility.RandomMinMax( 0, (vName4.Length-1) )];

			string[] vName5 = new string[] {"Castelo", "Caverna", "Mansão", "Casa", "Caverna", "Masmorra", "Floresta", "Deserto", "Torre", "Deserto", "Montanhas", "Pântano", "Colinas", "Noite", "Escuridão", "Nevoeiro", "Bosque", "Névoa", "Luz", "Garrafa", "Céu", "Chão", "Água", "Mar", "Areia", "Árvores", "Nuvens", "Estrelas", "Cristal", "Gema", "Lâmpada", "Jarro", "Correntes", "Fortaleza", "Cidade", "Vila", "Tumba", "Cripta"};
			string sName5 = vName5[Utility.RandomMinMax( 0, (vName5.Length-1) )];

			string sName6 = NameList.RandomName( "author" );

			string[] vName7 = new string[] {"Cálice", "Espada", "Machado", "Adaga", "Armadura", "Cristal", "Gema", "Poço", "Varinha", "Anel", "Amuleto", "Elmo", "Coroa", "Botas", "Cinto", "Veste", "Cálice", "Espelho", "Lança", "Escudo", "Cetro", "Cajado", "Livro", "Poção", "Arco", "Pedra", "Fogo", "Fragmento", "Caixa"};
			string sName7 = vName7[Utility.RandomMinMax( 0, (vName7.Length-1) )];

			string[] vName8 = new string[] {"Busca", "Missão", "Maldição", "Magia", "Mistério", "Poder", "Destruição", "Assassinato", "Desejo", "Natureza", "Lenda", "Mito", "Mentiras", "Localização"};
			string sName8 = vName8[Utility.RandomMinMax( 0, (vName8.Length-1) )];

			switch ( Utility.RandomMinMax( 0, 10 ) ) 
			{
				case 0: bookTitle = "a " + sName1 + " " + sName2 + " do " + sName4; break;
				case 1: bookTitle = "a " + sName2 + " do " + sName1 + " " + sName4; break;
				case 2: bookTitle = "o " + sName4 + " no " + sName5; break;
				case 3: bookTitle = "a " + sName2 + " do " + sName3 + " no " + sName5; break;
				case 4: bookTitle = "a " + sName1 + " " + sName5 + " do " + sName3; break;
				case 5: bookTitle = "a " + sName8 + " do " + sName1 + " " + sName7 + " de " + sName6; break;
				case 6: bookTitle = "a " + sName8 + " do " + sName7 + " de " + sName6; break;
				case 7: bookTitle = "o " + sName7 + " e o " + sName3; break;
				case 8: bookTitle = "o " + sName3 + " e o " + sName7; break;
				case 9: bookTitle = "a " + sName2 + " de " + sName6 + " o " + sName3; break;
				case 10: bookTitle = "a " + sName2 + " de " + sName6 + " o " + sName3; break;
			}

			return bookTitle;
		}

		///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		public static string GetRandomScenePainting()
		{
			string sceneType = "Colinas";
			string sceneName = "Gigante";
			string sceneFinal = "as Colinas de Ferro";

			switch( Utility.RandomMinMax( 1, 11 ) )
			{
				case 1: sceneType = "Colina"; break;
				case 2: sceneType = "Floresta"; break;
				case 3: sceneType = "Bosque"; break;
				case 4: sceneType = "Clareira"; break;
				case 5: sceneType = "Campos"; break;
				case 6: sceneType = "Montanha"; break;
				case 7: sceneType = "Ermos"; break;
				case 8: sceneType = "Deserto"; break;
				case 9: sceneType = "Pastagem"; break;
				case 10: sceneType = "Selva"; break;
				case 11: sceneType = "Terra"; break;
			}

			switch( Utility.RandomMinMax( 1, 5 ) )
			{
				case 1: sceneName = GetRandomJobTitle(0);     sceneFinal = "a " + sceneType + " de " + sceneName + "";                                break;
				case 2: sceneName = GetRandomColorName(0);     sceneFinal = "a " + sceneType + " de " + sceneName + " " + GetRandomThing(0);            break;
				case 3: sceneName = GetRandomThing(0);         sceneFinal = "a " + sceneType + " de " + sceneName + "";                                break;
				case 4: sceneName = GetRandomName();         sceneFinal = "a " + sceneType + " de " + sceneName + "";                                    break;
				case 5: sceneName = GetRandomCreature();     sceneFinal = "a " + sceneType + " de " + sceneName + "";
					if ( Utility.RandomMinMax( 1, 3 ) == 1 ){ sceneFinal = "a " + sceneType + " de " + GetRandomColorName(0) + " " + sceneName + ""; }
					break;
			}

			if ( Utility.RandomMinMax( 1, 2 ) == 1 ){ sceneFinal = "de " + sceneName + " " + sceneType + ""; }

			return sceneFinal;
		}

		///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		public static string GetRandomSociety()
		{
			string[] vName1 = new string[] {"Aliança", "Assembleia", "Banda", "Corrente", "Igreja", "Círculo", "Clã", "Coligação", "Facção", "Família", "Irmandade", "Seguidores", "Fraternidade", "Guilda", "Liga", "Legião", "Ordem", "Sociedade", "Soldados", "Sindicato", "União"};
			string sName1 = vName1[Utility.RandomMinMax( 0, (vName1.Length-1) )];

			string[] vName2 = new string[] {"do", "para o", "contra o", "com o", "sob o", "sob o", "sobre o", "acima do"};
			string sName2 = vName2[Utility.RandomMinMax( 0, (vName2.Length-1) )] + " ";

			string[] vName3 = new string[] {"Todo-poderoso", "Espantoso", "Âmbar", "Ancestral", "Angélico", "Surpreendente", "Estupendo", "Azul", "Negro", "Ennegrecido", "Abençoado", "Azul", "Brilhante", "Bronze", "Marrom", "Ardente", "Claro", "Cobre", "Cristal", "Amaldiçoado", "Condenado", "Sombrio", "Mortífero", "Demoníaco", "Diamante", "Divino", "Fadado", "Elétrico", "Esmeralda", "Encantado", "Etéreo", "Maligno", "Excelente", "Exótico", "Extraordinário", "Lendário", "Fabuloso", "Fantástico", "Esquecido", "Congelado", "Glorioso", "Brilhante", "Ouro", "Grande", "Cinza", "Grande", "Verde", "Enfeitiçado", "Alto", "Santo", "Gélido", "Incrível", "Índigo", "Infernal", "Marfim", "Jade", "Lendário", "Perdido", "Lunar", "Mágico", "Magnífico", "Marrom", "Maravilhoso", "Poderoso", "Desaparecido", "Misterioso", "Místico", "Mítico", "Laranja", "Ornado", "Fenomenal", "Platina", "Roxo", "Raro", "Vermelho", "Rubi", "Sagrado", "Safira", "Escarlate", "Recôndito", "Secreto", "Prata", "Solar", "Supremo", "Moreno", "Torcido", "Supremo", "Profano", "Desconhecido", "Indizível", "Veludo", "Vil", "Violeta", "Branco", "Maravilhoso", "Prodigioso", "Amarelo"};
			string sName3 = vName3[Utility.RandomMinMax( 0, (vName3.Length-1) )] + " ";
			if ( Utility.RandomMinMax( 0, 1 ) == 1 ){ sName3 = ""; }
			string sName4 = vName3[Utility.RandomMinMax( 0, (vName3.Length-1) )] + " ";
			if ( Utility.RandomMinMax( 0, 1 ) == 1 && sName3 != "" ){ sName4 = ""; }

			string[] vName4 = new string[] {"Aventureiro", "Amuleto", "Armadura", "Machado", "Bolsa", "Bandido", "Bárbaro", "Bardo", "Barão", "Besta", "Cinto", "Lâmina", "Ossos", "Livro", "Botas", "Garrafa", "Arco", "Bracelete", "Vela", "Capa", "Castelo", "Cavaleiro", "Cálice", "Clérigo", "Capa", "Pano", "Porrete", "Conjurador", "Coroa", "Cutelo", "Adaga", "Defensor", "Adivinho", "Dragão", "Tambor", "Pó", "Elemento", "Encantador", "Explorador", "Olho", "Lutador", "Flauta", "Gema", "Gladiador", "Luva", "Cálice", "Tumba", "Alabarda", "Martelo", "Mão", "Chapéu", "Coração", "Elmo", "Herege", "Chifre", "Caçador", "Ilusionista", "Invocador", "Chave", "Rei", "Reino", "Faca", "Cavaleiro", "Kryss", "Labirinto", "Lanterna", "Luz", "Senhor", "Alaúde", "Maça", "Mago", "Mágico", "Mercenário", "Menestrel", "Espelho", "Monge", "Lua", "Místico", "Prego", "Necromante", "Orbe", "Fora-da-lei", "Paladino", "Poção", "Bolsa", "Sacerdote", "Príncipe", "Profeta", "Ranger", "Enigma", "Anel", "Veste", "Ladrão", "Corda", "Sábio", "Bainha", "Cetro", "Cimitarra", "Batedor", "Pergaminho", "Buscador", "Vidente", "Algemas", "Xamã", "Escudo", "Caveira", "Céu", "Abatedor", "Feiticeiro", "Cajado", "Estrela", "Pedra", "Invocador", "Sol", "Espada", "Templário", "Templo", "Ladrão", "Tumba", "Tomo", "Torre", "Viajante", "Árvore", "Tridente", "Unicórnio", "Varinha", "Bruxo", "Guerreiro", "Vento", "Mago", "Palavra"};
			string sName5 = vName4[Utility.RandomMinMax( 0, (vName4.Length-1) )];

			string nSociety = "a '" + sName3 + sName1 + " " + sName2 + sName4 + sName5 + "'";

			return nSociety;
		}

		///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		public static string GetRandomJobTitle( int space )
		{
			string[] vTitle = new string[] {"Aventureiro", "Bandido", "Bárbaro", "Bardo", "Barão", "Baronesa", "Cavaleiro", "Clérigo", "Conjurador", "Defensor", "Adivinho", "Druida", "Encantador", "Encantadora", "Explorador", "Lutador", "Gladiador", "Herege", "Caçador", "Ilusionista", "Invocador", "Rei", "Cavaleiro", "Dama", "Senhor", "Mago", "Mágico", "Mercenário", "Menestrel", "Monge", "Místico", "Necromante", "Fora-da-lei", "Paladino", "Sacerdote", "Sacerdotisa", "Príncipe", "Princesa", "Profeta", "Rainha", "Ranger", "Ladrão", "Sábio", "Batedor", "Buscador", "Vidente", "Xamã", "Abatedor", "Feiticeiro", "Feiticeira", "Invocador", "Templário", "Ladrão", "Viajante", "Bruxo", "Guerreiro", "Bruxa", "Mago"};
			string sTitle = "o " + vTitle[Utility.RandomMinMax( 0, (vTitle.Length-1) )];
			if ( space > 0 ){ sTitle = sTitle + " "; }

			return sTitle;
		}

		///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		public static string GetRandomColorName( int space )
		{
			string[] vColor = new string[] {"Âmbar", "Azul", "Preto", "Azul", "Brilhante", "Bronze", "Marrom", "Ardente", "Cobre", "Cristal", "Sombrio", "Diamante", "Esmeralda", "Congelado", "Brilhante", "Ouro", "Cinza", "Verde", "Gélido", "Índigo", "Marfim", "Jade", "Marrom", "Laranja", "Platina", "Roxo", "Vermelho", "Rubi", "Safira", "Escarlate", "Prata", "Veludo", "Violeta", "Branco", "Amarelo"};
			string sColor = vColor[Utility.RandomMinMax( 0, (vColor.Length-1) )];
			if ( space > 0 ){ sColor = sColor + " "; }

			return sColor;
		}

		///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		public static string GetRandomThing( int space )
		{
			string[] vThing = new string[] {"Aventureiro", "Amuleto", "Armadura", "Machado", "Bandido", "Bárbaro", "Bardo", "Barão", "Besta", "Cinto", "Lâmina", "Ossos", "Botas", "Garrafa", "Arco", "Bracelete", "Vela", "Cavaleiro", "Cálice", "Clérigo", "Porrete", "Conjurador", "Coroa", "Cutelo", "Adaga", "Defensor", "Adivinho", "Dragão", "Tambor", "Elemento", "Encantador", "Explorador", "Olho", "Lutador", "Flauta", "Gladiador", "Cálice", "Alabarda", "Martelo", "Mão", "Coração", "Elmo", "Herege", "Chifre", "Caçador", "Ilusionista", "Invocador", "Chave", "Rei", "Faca", "Cavaleiro", "Kryss", "Lanterna", "Senhor", "Alaúde", "Maça", "Mago", "Mágico", "Mercenário", "Menestrel", "Monge", "Místico", "Prego", "Necromante", "Orbe", "Fora-da-lei", "Paladino", "Sacerdote", "Príncipe", "Profeta", "Ranger", "Anel", "Veste", "Ladrão", "Sábio", "Bainha", "Cetro", "Cimitarra", "Batedor", "Buscador", "Vidente", "Algemas", "Xamã", "Escudo", "Caveira", "Abatedor", "Feiticeiro", "Cajado", "Pedra", "Invocador", "Espada", "Templário", "Ladrão", "Torre", "Viajante", "Árvore", "Tridente", "Unicórnio", "Varinha", "Bruxo", "Guerreiro", "Mago"};
			string sThing = vThing[Utility.RandomMinMax( 0, (vThing.Length-1) )];
			if ( space > 0 ){ sThing = sThing + " "; }

			return sThing;
		}

		///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		public static string GetRandomMonsters()
		{
			string[] vThing = new string[] {"um balrog", "um balron", "um bandido", "um bárbaro", "um beholder", "um bugbear", "uma quimera", "um ciclope", "um demônio", "um demônio", "um diabo", "um dracolich", "um dragão", "um dragão tartaruga", "um drake", "um dreadhorn", "um drow", "uma gárgula", "um gazer", "um fantasma", "um ghoul", "um gigante", "um besouro gigante", "um caranguejo gigante", "uma enguia gigante", "um escorpião gigante", "uma serpente gigante", "uma aranha gigante", "uma lula gigante", "um gnoll", "um gnomo", "um goblin", "um golem", "uma górgona", "um grifo", "uma bruxa", "uma harpia", "um hipogrifo", "um hobgoblin", "uma hidra", "um kobold", "um kraken", "um leviatã", "um lich", "um homem-lagarto", "uma mantícora", "um devorador de mentes", "um minotauro", "um morlock", "uma múmia", "uma naga", "um nazghoul", "um fantasma", "um homem-rato", "um ceifador", "um selvagem", "um slime", "uma esfinge", "um sprite", "uma súcubo", "um terathan", "um tritun", "um troll", "um vampiro", "um guerreiro", "um wight", "uma bruxa", "um mago", "um wyrm", "um wyvern", "um xorn", "um yeti", "um zumbi", "um efreeti", "um elemental", "um ettin", "um ifreeti", "um diabrete", "um ogro", "um ofídio", "um orc", "um umber hulk"};
			string sThing = vThing[Utility.RandomMinMax( 0, (vThing.Length-1) )];

			return sThing;
		}

		///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		public static string GetRandomAttackers()
		{
			string[] vThing = new string[] {"balrogs", "balrons", "bandidos", "bárbaros", "bugbears", "demônios", "demônios", "drows", "ettins", "gigantes", "gnolls", "gnomos", "goblins", "hobgoblins", "kobolds", "homens-lagarto", "minotauros", "ogros", "ofídios", "orcs", "homens-rato", "selvagens", "terathans", "trituns", "trolls"};
			string sThing = vThing[Utility.RandomMinMax( 0, (vThing.Length-1) )];

			return sThing;
		}

		///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		public static string GetRandomTroops()
		{
			string[] vThing = new string[] {"exército", "soldados", "tropas", "balrogs", "balrons", "bandidos", "bárbaros", "bugbears", "demônios", "demônios", "drows", "ettins", "gigantes", "gnolls", "gnomos", "goblins", "hobgoblins", "kobolds", "homens-lagarto", "minotauros", "ogros", "ofídios", "orcs", "homens-rato", "selvagens", "terathans", "trituns", "trolls", "magos"};
			string sThing = vThing[Utility.RandomMinMax( 0, (vThing.Length-1) )];

			return sThing;
		}

		///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		public static string GetRandomCoinReward()
		{
			string[] vThing = new string[] {"500", "600", "700", "800", "900", "1,000", "1,100", "1,200", "1,300", "1,400", "1,500", "1,600", "1,700", "1,800", "1,900", "2,000", "2,100", "2,200", "2,300", "2,400", "2,500", "2,600", "2,700", "2,800", "2,900", "3,000", "3,100", "3,200", "3,300", "3,400", "3,500", "3,600", "3,700", "3,800", "3,900", "4,000", "4,100", "4,200", "4,300", "4,400", "4,500", "4,600", "4,700", "4,800", "4,900", "5,000", "5,100", "5,200", "5,300", "5,400", "5,500", "5,600", "5,700", "5,800", "5,900", "6,000", "6,100", "6,200", "6,300", "6,400", "6,500", "6,600", "6,700", "6,800", "6,900", "7,000", "7,100", "7,200", "7,300", "7,400", "7,500", "7,600", "7,700", "7,800", "7,900", "8,000", "8,100", "8,200", "8,300", "8,400", "8,500", "8,600", "8,700", "8,800", "8,900", "9,000", "9,100", "9,200", "9,300", "9,400", "9,500", "9,600", "9,700", "9,800", "9,900", "10,000"};
				string sThing = vThing[Utility.RandomMinMax( 0, (vThing.Length-1) )];

			return sThing;
		}

		///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		public static string GetRandomJob()
		{
			string sJob = "funileiro";
			int section = Utility.RandomMinMax( 1, 23 );
			switch( section )
			{
				case 1: sJob = "ferreiro"; break;
				case 2: sJob = "joalheiro"; break;
				case 3: sJob = "providor"; break;
				case 4: sJob = "banqueiro"; break;
				case 5: sJob = "cunhador"; break;
				case 6: sJob = "garçom"; break;
				case 7: sJob = "guarda"; break;
				case 8: sJob = "sábio"; break;
				case 9: sJob = "mago"; break;
				case 10: sJob = "herbalista"; break;
				case 11: sJob = "alquimista"; break;
				case 12: sJob = "curandeiro"; break;
				case 13: sJob = "mestre de guilda"; break;
				case 14: sJob = "funileiro"; break;
				case 15: sJob = "estalajadeiro"; break;
				case 16: sJob = "barman"; break;
				case 17: sJob = "açougueiro"; break;
				case 18: sJob = "alfaiate"; break;
				case 19: sJob = "tecelão"; break;
				case 20: sJob = "construtor naval"; break;
				case 21: sJob = "escriba"; break;
				case 22: sJob = "fazendeiro"; break;
				case 23: sJob = "mestre de estábulo"; break;
			}

			return sJob;
		}

		///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		public static string GetRandomShop()
		{
			string sType = "loja";
			switch( Utility.RandomMinMax( 0, 3 ) )
			{
				case 1: sType = "estabelecimento"; break;
				case 2: sType = "mercearia"; break;
				case 3: sType = "mercado"; break;
			}
			string sJob = "funileiro";
			switch( Utility.RandomMinMax( 1, 24 ) )
			{
				case 1: sJob = "uma " + sType + " de ferreiro"; break;
				case 2: sJob = "uma " + sType + " de joalheria"; break;
				case 3: sJob = "uma " + sType + " de provisões"; break;
				case 4: sJob = "uma " + sType + " de couro"; break;
				case 5: sJob = "uma " + sType + " de música"; break;
				case 6: sJob = "uma " + sType + " de alquimia"; break;
				case 7: sJob = "uma " + sType + " de poções"; break;
				case 8: sJob = "uma " + sType + " de livros"; break;
				case 9: sJob = "uma " + sType + " de magia"; break;
				case 10: sJob = "uma " + sType + " de ervas"; break;
				case 11: sJob = "uma " + sType + " de jardim"; break;
				case 12: sJob = "uma " + sType + " de animais"; break;
				case 13: sJob = "uma " + sType + " de alfaiate"; break;
				case 14: sJob = "uma estalagem"; break;
				case 15: sJob = "uma taverna"; break;
				case 16: sJob = "um banco"; break;
				case 17: sJob = "uma " + sType + " de carnes"; break;
				case 18: sJob = "uma " + sType + " de construção naval"; break;
				case 19: sJob = "uma " + sType + " de escriba"; break;
				case 20: sJob = "uma guilda"; break;
				case 21: sJob = "um culto"; break;
				case 22: sJob = "uma igreja"; break;
				case 23: sJob = "um estábulo"; break;
				case 24: sJob = "uma biblioteca"; break;
			}

			return sJob;
		}

		///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		public static string GetRandomGemType( string category )
		{
			string sGem = "rubi";

			int section = Utility.RandomMinMax( 1, 18 );
			if ( category == "dragyns" ){ section = Utility.RandomMinMax( 1, 12 ); }

			switch( section )
			{
				case 1: sGem = "rubi"; break;
				case 2: sGem = "jade"; break;
				case 3: sGem = "quartzo"; break;
				case 4: sGem = "safira"; break;
				case 5: sGem = "ônix"; break;
				case 6: sGem = "espinela"; break;
				case 7: sGem = "topázio"; break;
				case 8: sGem = "ametista"; break;
				case 9: sGem = "esmeralda"; break;
				case 10: sGem = "granada"; break;
				case 11: sGem = "prata"; break;
				case 12: sGem = "rubi estrela"; break;
				case 13: sGem = "safira estrela"; break;
				case 14: sGem = "citrino"; break;
				case 15: sGem = "caddelita"; break;
				case 16: sGem = "âmbar"; break;
				case 17: sGem = "diamante"; break;
				case 18: sGem = "turmalina"; break;
			}

			return sGem;
		}

		///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		public static string GetRandomCity()
		{
			string sPlace = "Britain";
			int section = Utility.RandomMinMax( 1, 23 );
			switch( section )
			{
				case 1: sPlace = "Britain"; break;
				case 2: sPlace = "Fawn"; break;
				case 3: sPlace = "Grey"; break;
				case 4: sPlace = "Moon"; break;
				case 5: sPlace = "Yew"; break;
				case 6: sPlace = "Montor"; break;
				case 7: sPlace = "Umbra"; break;
				case 8: sPlace = "Devil Guard"; break;
				case 9: sPlace = "Death Gulch"; break;
				case 10: sPlace = "Renika"; break;
				case 11: sPlace = "Glacial Hills"; break;
				case 12: sPlace = "Springvale"; break;
				case 13: sPlace = "Elidor"; break;
				case 14: sPlace = "Islegem"; break;
				case 15: sPlace = "o Porto de Dusk"; break;
				case 16: sPlace = "o Porto de Starguide"; break;
				case 17: sPlace = "Portshine"; break;
				case 18: sPlace = "Greensky Village"; break;
				case 19: sPlace = "a Cidade de Lodoria"; break;
				case 20: sPlace = "Cimmeran Hold"; break;
				case 21: sPlace = "a Vila de Barako"; break;
				case 22: sPlace = "a Vila de Kurak"; break;
				case 23: sPlace = "Kuldara"; break;
			}

			return sPlace;
		}

		///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		public static string GetRandomCreature()
		{
			string sCreature = "Gigante";
			int section = Utility.RandomMinMax( 0, 120 );
			switch( section )
			{
				case 0: sCreature = "Formiga"; break;
				case 1: sCreature = "Macaco"; break;
				case 2: sCreature = "Babuíno"; break;
				case 3: sCreature = "Texugo"; break;
				case 4: sCreature = "Basilisco"; break;
				case 5: sCreature = "Urso"; break;
				case 6: sCreature = "Castor"; break;
				case 7: sCreature = "Besouro"; break;
				case 8: sCreature = "Beholder"; break;
				case 9: sCreature = "Javali"; break;
				case 10: sCreature = "Duende"; break;
				case 11: sCreature = "Búfalo"; break;
				case 12: sCreature = "Touro"; break;
				case 13: sCreature = "Camelo"; break;
				case 14: sCreature = "Centauro"; break;
				case 15: sCreature = "Centopeia"; break;
				case 16: sCreature = "Quimera"; break;
				case 17: sCreature = "Cockatrice"; break;
				case 18: sCreature = "Crocodilo"; break;
				case 19: sCreature = "Veado"; break;
				case 20: sCreature = "Demônio"; break;
				case 21: sCreature = "Diabo"; break;
				case 22: sCreature = "Dinossauro"; break;
				case 23: sCreature = "Gênio"; break;
				case 24: sCreature = "Cão"; break;
				case 25: sCreature = "Dragão"; break;
				case 26: sCreature = "Dríade"; break;
				case 27: sCreature = "Anão"; break;
				case 28: sCreature = "Águia"; break;
				case 29: sCreature = "Efreet"; break;
				case 30: sCreature = "Elemental"; break;
				case 31: sCreature = "Elefante"; break;
				case 32: sCreature = "Elfo"; break;
				case 33: sCreature = "Ettin"; break;
				case 34: sCreature = "Sapo"; break;
				case 35: sCreature = "Fungo"; break;
				case 36: sCreature = "Gárgula"; break;
				case 37: sCreature = "Espectro"; break;
				case 38: sCreature = "Fantasma"; break;
				case 39: sCreature = "Ghoul"; break;
				case 40: sCreature = "Gigante"; break;
				case 41: sCreature = "Gnoll"; break;
				case 42: sCreature = "Gnomo"; break;
				case 43: sCreature = "Cabra"; break;
				case 44: sCreature = "Goblin"; break;
				case 45: sCreature = "Golem"; break;
				case 46: sCreature = "Górgona"; break;
				case 47: sCreature = "Grifo"; break;
				case 48: sCreature = "Bruxa"; break;
				case 49: sCreature = "Hobbit"; break;
				case 50: sCreature = "Harpia"; break;
				case 51: sCreature = "Cão do Inferno"; break;
				case 52: sCreature = "Hipogrifo"; break;
				case 53: sCreature = "Hipopótamo"; break;
				case 54: sCreature = "Hobgoblin"; break;
				case 55: sCreature = "Cavalo"; break;
				case 56: sCreature = "Hidra"; break;
				case 57: sCreature = "Hiena"; break;
				case 58: sCreature = "Diabrete"; break;
				case 59: sCreature = "Chacal"; break;
				case 60: sCreature = "Jaguar"; break;
				case 61: sCreature = "Ki-rin"; break;
				case 62: sCreature = "Kobold"; break;
				case 63: sCreature = "Leopardo"; break;
				case 64: sCreature = "Leprechaun"; break;
				case 65: sCreature = "Lich"; break;
				case 66: sCreature = "Leão"; break;
				case 67: sCreature = "Lagarto"; break;
				case 68: sCreature = "Homem-lagarto"; break;
				case 69: sCreature = "Lobisomem"; break;
				case 70: sCreature = "Lince"; break;
				case 71: sCreature = "Mamute"; break;
				case 72: sCreature = "Mantícora"; break;
				case 73: sCreature = "Mastodonte"; break;
				case 74: sCreature = "Medusa"; break;
				case 75: sCreature = "Minotauro"; break;
				case 76: sCreature = "Mula"; break;
				case 77: sCreature = "Múmia"; break;
				case 78: sCreature = "Naga"; break;
				case 79: sCreature = "Pesadelo"; break;
				case 80: sCreature = "Ogro"; break;
				case 81: sCreature = "Orc"; break;
				case 82: sCreature = "Coruja"; break;
				case 83: sCreature = "Pégaso"; break;
				case 84: sCreature = "Pixie"; break;
				case 85: sCreature = "Porco-espinho"; break;
				case 86: sCreature = "Carneiro"; break;
				case 87: sCreature = "Rato"; break;
				case 88: sCreature = "Ceifador"; break;
				case 89: sCreature = "Rinoceronte"; break;
				case 90: sCreature = "Roc"; break;
				case 91: sCreature = "Sátiro"; break;
				case 92: sCreature = "Escorpião"; break;
				case 93: sCreature = "Serpente"; break;
				case 94: sCreature = "Sombra"; break;
				case 95: sCreature = "Esqueleto"; break;
				case 96: sCreature = "Gambá"; break;
				case 97: sCreature = "Cobra"; break;
				case 98: sCreature = "Espectro"; break;
				case 99: sCreature = "Esfinge"; break;
				case 100: sCreature = "Aranha"; break;
				case 101: sCreature = "Sprite"; break;
				case 102: sCreature = "Cervo"; break;
				case 103: sCreature = "Tigre"; break;
				case 104: sCreature = "Titã"; break;
				case 105: sCreature = "Sapo"; break;
				case 106: sCreature = "Troglodita"; break;
				case 107: sCreature = "Troll"; break;
				case 108: sCreature = "Unicórnio"; break;
				case 109: sCreature = "Vampiro"; break;
				case 110: sCreature = "Doninha"; break;
				case 111: sCreature = "Wight"; break;
				case 112: sCreature = "Wisp"; break;
				case 113: sCreature = "Lobo"; break;
				case 114: sCreature = "Carcaju"; break;
				case 115: sCreature = "Verme"; break;
				case 116: sCreature = "Espectro"; break;
				case 117: sCreature = "Wyvern"; break;
				case 118: sCreature = "Yeti"; break;
				case 119: sCreature = "Zumbi"; break;
				case 120: sCreature = "Zorn"; break;
			}

			return sCreature;
		}

		///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		public static string GetRandomIntelligentRace()
		{
			string sLanguage = "balron";
			int section = Utility.RandomMinMax( 0, 28 );
			switch( section )
			{
				case 0: sLanguage = "balron"; break;
				case 1: sLanguage = "pixie"; break;
				case 2: sLanguage = "centauro"; break;
				case 3: sLanguage = "demoníaco"; break;
				case 4: sLanguage = "dragão"; break;
				case 5: sLanguage = "anão"; break;
				case 6: sLanguage = "élfico"; break;
				case 7: sLanguage = "fey"; break;
				case 8: sLanguage = "gárgula"; break;
				case 9: sLanguage = "ciclope"; break;
				case 10: sLanguage = "gnoll"; break;
				case 11: sLanguage = "goblin"; break;
				case 12: sLanguage = "gremlin"; break;
				case 13: sLanguage = "druídico"; break;
				case 14: sLanguage = "tritun"; break;
				case 15: sLanguage = "minotauro"; break;
				case 16: sLanguage = "naga"; break;
				case 17: sLanguage = "ogro"; break;
				case 18: sLanguage = "orc"; break;
				case 19: sLanguage = "esfinge"; break;
				case 20: sLanguage = "treekin"; break;
				case 21: sLanguage = "troll"; break;
				case 22: sLanguage = "morto-vivo"; break;
				case 23: sLanguage = "vampiro"; break;
				case 24: sLanguage = "elfo negro"; break;
				case 25: sLanguage = "mágico"; break;
				case 26: sLanguage = "humano"; break;
				case 27: sLanguage = "simbólico"; break;
				case 28: sLanguage = "rúnico"; break;
			}

			return sLanguage;
		}

		///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		public static string MagicItemAdj( string placed, bool oriental, bool evil, int itemid )
		{
			string sAdjective = "mágico(a)";

			int pick = Utility.RandomMinMax( 0, 37 );

			if ( placed == "end" ){ pick = Utility.RandomMinMax( 38, 116 ); }

			if ( placed == "end" && itemid == 0x2C9E ) // SAFETY CATCH FOR DEMON/DRAGON SKULL TRINKETS
			{
				pick = Utility.RandomMinMax( 0, 5 );

				switch( pick )
				{
					case 0: sAdjective = "o Demônio";        break;
					case 1: sAdjective = "o Dragão";        break;
					case 2: sAdjective = "o Demônio";        break;
					case 3: sAdjective = "o Diabo";        break;
					case 4: sAdjective = "o Wyrm";        break;
					case 5: sAdjective = "o Draconiano";        break;
				}
			}
			else
			{
				if ( oriental )
				{
					switch( pick )
					{
						case 0: sAdjective = "exótico(a)";             break;
						case 1: sAdjective = "misterioso(a)";         break;
						case 2: sAdjective = "encantado(a)";         break;
						case 3: sAdjective = "maravilhoso(a)";         break;
						case 4: sAdjective = "espantoso(a)";         break;
						case 5: sAdjective = "surpreendente";         break;
						case 6: sAdjective = "místico(a)";             break;
						case 7: sAdjective = "estupendo(a)";         break;
						case 8: sAdjective = "mágico(a)";             break;
						case 9: sAdjective = "divino(a)";             break;
						case 10: sAdjective = "excelente";             break;
						case 11: sAdjective = "magnífico(a)";         break;
						case 12: sAdjective = "fenomenal";             break;
						case 13: sAdjective = "fantástico(a)";         break;
						case 14: sAdjective = "incrível";             break;
						case 15: sAdjective = "extraordinário(a)";     break;
						case 16: sAdjective = "fabuloso(a)";         break;
						case 17: sAdjective = "prodigioso(a)";         break;
						case 18: sAdjective = "glorioso(a)";         break;
						case 19: sAdjective = "perdido(a)";             break;
						case 20: sAdjective = "lendário(a)";         break;
						case 21: sAdjective = "lendário(a)";         break;
						case 22: sAdjective = "mítico(a)";             break;
						case 23: sAdjective = "ancestral";             break;
						case 24: sAdjective = "ornado(a)";             break;
						case 25: sAdjective = "supremo(a)";             break;
						case 26: sAdjective = "raro(a)";             break;
						case 27: sAdjective = "maravilhoso(a)";         break;
						case 28: sAdjective = "sagrado(a)";             break;
						case 29: sAdjective = "todo-poderoso(a)";     break;
						case 30: sAdjective = "supremo(a)";             break;
						case 31: sAdjective = "poderoso(a)";         break;
						case 32: sAdjective = "indizível";             break;
						case 33: sAdjective = "esquecido(a)";         break;
						case 34: sAdjective = "grande";                 break;
						case 35: sAdjective = "grão";                 break;
						case 36: sAdjective = "mágico(a)";             break;
						case 37: sAdjective = "incomum";             break;
						case 38: sAdjective = "poder";                 break;
						case 39: sAdjective = "poder";                 break;
						case 40: sAdjective = "grandiosidade";         break;
						case 41: sAdjective = "mágico(a)";             break;
						case 42: sAdjective = "supremacia";             break;
						case 43: sAdjective = "o todo-poderoso";     break;
						case 44: sAdjective = "o sagrado";             break;
						case 45: sAdjective = "magnificência";         break;
						case 46: sAdjective = "excelência";             break;
						case 47: sAdjective = "glória";                 break;
						case 48: sAdjective = "mistério";             break;
						case 49: sAdjective = "o divino";             break;
						case 50: sAdjective = "o esquecido";         break;
						case 51: sAdjective = "lenda";                 break;
						case 52: sAdjective = "o perdido";             break;
						case 53: sAdjective = "os antigos";             break;
						case 54: sAdjective = "maravilha";             break;
						case 55: sAdjective = "o poderoso";             break;
						case 56: sAdjective = "proeza";                 break;
						case 57: sAdjective = "nobreza";             break;
						case 58: sAdjective = "misticismo";             break;
						case 59: sAdjective = "encantamento";         break;
						case 60: sAdjective = "o Karateka";        break;
						case 61: sAdjective = "o Ronin";        break;
						case 62: sAdjective = "o Samurai";        break;
						case 63: sAdjective = "o Ninja";        break;
						case 64: sAdjective = "o Yakuza";        break;
						case 65: sAdjective = "o Wu Jen";        break;
						case 66: sAdjective = "o Kensai";        break;
						case 67: sAdjective = "o Shukenja";        break;
						case 68: sAdjective = "o Fangshi";        break;
						case 69: sAdjective = "o Waidan";        break;
						case 70: sAdjective = "o Neidan";        break;
						case 71: sAdjective = "o Monge";        break;
						case 72: sAdjective = "o Kyudo";        break;
						case 73: sAdjective = "o Yuki Ota";        break;
						case 74: sAdjective = "o Sakushi";        break;
						case 75: sAdjective = "o Youxia";        break;
						case 76: sAdjective = "o Kyudoka";        break;
						case 77: sAdjective = "o Ashigaru";        break;
						case 78: sAdjective = "o Artista Marcial";    break;
						case 79: sAdjective = "o Abatedor";        break;
						case 80: sAdjective = "o Wako";        break;
						case 81: sAdjective = "o Bárbaro";        break;
						case 82: sAdjective = "o Explorador";        break;
						case 83: sAdjective = "o Herege";        break;
						case 84: sAdjective = "o Sumo";        break;
						case 85: sAdjective = "o Iaijutsu";        break;
						case 86: sAdjective = "o Imperador";        break;
						case 87: sAdjective = "da Dinastia " + Server.Misc.RandomThings.GetRandomColorName(0);        break;
						case 88: sAdjective = "o Zhuhou";        break;
						case 89: sAdjective = "o Qing";        break;
						case 90: sAdjective = "a Imperatriz";        break;
						case 91: sAdjective = "o Daifu";        break;
						case 92: sAdjective = "o Shi";        break;
						case 93: sAdjective = "o Shumin";        break;
						case 94: sAdjective = "o Heika";        break;
						case 95: sAdjective = "o Denka";        break;
						case 96: sAdjective = "o Hidenka";        break;
						case 97: sAdjective = "o Kakka";        break;
						case 98: sAdjective = "o Daitoryo";        break;
						case 99: sAdjective = "o Renshi";        break;
						case 100: sAdjective = "o Kyoshi";        break;
						case 101: sAdjective = "o Hanshi";        break;
						case 102: sAdjective = "o Meijin";        break;
						case 103: sAdjective = "o Oyakata";        break;
						case 104: sAdjective = "o Shihan";        break;
						case 105: sAdjective = "o Shidoin";        break;
						case 106: sAdjective = "o Shisho";        break;
						case 107: sAdjective = "o Zeki";        break;
						case 108: sAdjective = "o Xamã";        break;
						case 109: sAdjective = "o Shodan";        break;
						case 110: sAdjective = "o Nidan";        break;
						case 111: sAdjective = "o Yodan";        break;
						case 112: sAdjective = "o Godan";        break;
						case 113: sAdjective = "o Rokudan";        break;
						case 114: sAdjective = "o Shichidan";        break;
						case 115: sAdjective = "o Hachidan";        break;
						case 116: sAdjective = "o Judan";        break;
					}
				}
				else if ( evil )
				{
					switch( pick )
					{
						case 0: sAdjective = "maligno(a)";             break;
						case 1: sAdjective = "corrupto(a)";         break;
						case 2: sAdjective = "destrutivo(a)";         break;
						case 3: sAdjective = "odioso(a)";             break;
						case 4: sAdjective = "abominável";             break;
						case 5: sAdjective = "malevolente";         break;
						case 6: sAdjective = "malicioso(a)";         break;
						case 7: sAdjective = "nefário(a)";             break;
						case 8: sAdjective = "perverso(a)";         break;
						case 9: sAdjective = "cruel";                 break;
						case 10: sAdjective = "vil";                 break;
						case 11: sAdjective = "vil";                 break;
						case 12: sAdjective = "imundo(a)";             break;
						case 13: sAdjective = "danoso(a)";             break;
						case 14: sAdjective = "desastroso(a)";         break;
						case 15: sAdjective = "nocivo(a)";             break;
						case 16: sAdjective = "repulsivo(a)";         break;
						case 17: sAdjective = "maléfico(a)";         break;
						case 18: sAdjective = "repulsivo(a)";         break;
						case 19: sAdjective = "rancoroso(a)";         break;
						case 20: sAdjective = "irado(a)";             break;
						case 21: sAdjective = "mortífero(a)";         break;
						case 22: sAdjective = "sinistro(a)";         break;
						case 23: sAdjective = "lamentoso(a)";         break;
						case 24: sAdjective = "fatal";                 break;
						case 25: sAdjective = "definhante";             break;
						case 26: sAdjective = "decadente";             break;
						case 27: sAdjective = "amaldiçoado(a)";         break;
						case 28: sAdjective = "condenatório(a)";     break;
						case 29: sAdjective = "horrífico(a)";         break;
						case 30: sAdjective = "atormentado(a)";     break;
						case 31: sAdjective = "fadado(a)";             break;
						case 32: sAdjective = "indizível";             break;
						case 33: sAdjective = "odiado(a)";             break;
						case 34: sAdjective = "miserável";             break;
						case 35: sAdjective = "infame";             break;
						case 36: sAdjective = "corrompido(a)";         break;
						case 37: sAdjective = "enfurecido(a)";         break;
						case 38: sAdjective = "morte";                 break;
						case 39: sAdjective = "vilania";             break;
						case 40: sAdjective = "trevas";             break;
						case 41: sAdjective = "ódio";                 break;
						case 42: sAdjective = "maligno(a)";             break;
						case 43: sAdjective = "os Nove Infernos";     break;
						case 44: sAdjective = "Cthulhu";             break;
						case 45: sAdjective = "Inferno";             break;
						case 46: sAdjective = "Hades";                 break;
						case 47: sAdjective = "Satanás";             break;
						case 48: sAdjective = "espíritos";             break;
						case 49: sAdjective = "o assombrado";         break;
						case 50: sAdjective = "o morto-vivo";         break;
						case 51: sAdjective = "a múmia";             break;
						case 52: sAdjective = "o enterrado";         break;
						case 53: sAdjective = "o poltergeist";        break;
						case 54: sAdjective = "o culto";             break;
						case 55: sAdjective = "a tumba";             break;
						case 56: sAdjective = "sangue";             break;
						case 57: sAdjective = "o Fantasma " + Server.Misc.RandomThings.GetRandomColorName(0);         break;
						case 58: sAdjective = "a tumba";             break;
						case 59: sAdjective = "a cripta";             break;
						case 60: sAdjective = "o Necromante";        break;
						case 61: sAdjective = "a Bruxa";        break;
						case 62: sAdjective = "o Bruxo";        break;
						case 63: sAdjective = "o Vil";        break;
						case 64: sAdjective = "o Odiado";        break;
						case 65: sAdjective = "o Vilão";        break;
						case 66: sAdjective = "o Assassino";        break;
						case 67: sAdjective = "o Matador";        break;
						case 68: sAdjective = "o Fantasma";        break;
						case 69: sAdjective = "o Cavaleiro da Morte";    break;
						case 70: sAdjective = "o Lich";        break;
						case 71: sAdjective = "o Ocultista";        break;
						case 72: sAdjective = "o Cultista";        break;
						case 73: sAdjective = "o Diabolista";        break;
						case 74: sAdjective = "a Bruxa";        break;
						case 75: sAdjective = "o Açougueiro";        break;
						case 76: sAdjective = "o Abatedor";        break;
						case 77: sAdjective = "o Carrasco";        break;
						case 78: sAdjective = "o Demônio";        break;
						case 79: sAdjective = "o Espectro";        break;
						case 80: sAdjective = "a Sombra";        break;
						case 81: sAdjective = "o Espectro";        break;
						case 82: sAdjective = "o Diabo";        break;
						case 83: sAdjective = "a Sombra";        break;
						case 84: sAdjective = "o Espectro";        break;
						case 85: sAdjective = "o Vampiro";        break;
						case 86: sAdjective = "a Banshee";        break;
						case 87: sAdjective = "o Sombrio";        break;
						case 88: sAdjective = "o Negro";        break;
						case 89: sAdjective = "o Agente Funerário";        break;
						case 90: sAdjective = "o Embalsamador";        break;
						case 91: sAdjective = "a Tumba";        break;
						case 92: sAdjective = "o Demônio";        break;
						case 93: sAdjective = "o Demônio";        break;
						case 94: sAdjective = "o Corrupto";        break;
						case 95: sAdjective = "o Odioso";        break;
						case 96: sAdjective = "o Abominável";        break;
						case 97: sAdjective = "o Horrendo";        break;
						case 98: sAdjective = "o Malevolente";        break;
						case 99: sAdjective = "o Malicioso";        break;
						case 100: sAdjective = "o Nefário";        break;
						case 101: sAdjective = "o Cruel";        break;
						case 102: sAdjective = "o Perverso";        break;
						case 103: sAdjective = "o Imundo";        break;
						case 104: sAdjective = "o Funesto";        break;
						case 105: sAdjective = "o Depravado";        break;
						case 106: sAdjective = "o Repulsivo";        break;
						case 107: sAdjective = "o Irascível";        break;
						case 108: sAdjective = "o Lamentoso";        break;
						case 109: sAdjective = "o Sombrio";        break;
						case 110: sAdjective = "o Sombrio";        break;
						case 111: sAdjective = "o Sem Vida";        break;
						case 112: sAdjective = "o Falecido";        break;
						case 113: sAdjective = "o Sem Sangue";        break;
						case 114: sAdjective = "o Mortificado";        break;
						case 115: sAdjective = "o Partido";        break;
						case 116: sAdjective = "o Morto";        break;
					}
				}
				else
				{
					switch( pick )
					{
						case 0: sAdjective = "exótico(a)";             break;
						case 1: sAdjective = "misterioso(a)";         break;
						case 2: sAdjective = "encantado(a)";         break;
						case 3: sAdjective = "maravilhoso(a)";         break;
						case 4: sAdjective = "espantoso(a)";         break;
						case 5: sAdjective = "surpreendente";         break;
						case 6: sAdjective = "místico(a)";             break;
						case 7: sAdjective = "estupendo(a)";         break;
						case 8: sAdjective = "mágico(a)";             break;
						case 9: sAdjective = "divino(a)";             break;
						case 10: sAdjective = "excelente";             break;
						case 11: sAdjective = "magnífico(a)";         break;
						case 12: sAdjective = "fenomenal";             break;
						case 13: sAdjective = "fantástico(a)";         break;
						case 14: sAdjective = "incrível";             break;
						case 15: sAdjective = "extraordinário(a)";     break;
						case 16: sAdjective = "fabuloso(a)";         break;
						case 17: sAdjective = "prodigioso(a)";         break;
						case 18: sAdjective = "glorioso(a)";         break;
						case 19: sAdjective = "perdido(a)";             break;
						case 20: sAdjective = "lendário(a)";         break;
						case 21: sAdjective = "lendário(a)";         break;
						case 22: sAdjective = "mítico(a)";             break;
						case 23: sAdjective = "ancestral";             break;
						case 24: sAdjective = "ornado(a)";             break;
						case 25: sAdjective = "supremo(a)";             break;
						case 26: sAdjective = "raro(a)";             break;
						case 27: sAdjective = "maravilhoso(a)";         break;
						case 28: sAdjective = "sagrado(a)";             break;
						case 29: sAdjective = "todo-poderoso(a)";     break;
						case 30: sAdjective = "supremo(a)";             break;
						case 31: sAdjective = "poderoso(a)";         break;
						case 32: sAdjective = "indizível";             break;
						case 33: sAdjective = "esquecido(a)";         break;
						case 34: sAdjective = "grande";                 break;
						case 35: sAdjective = "grão";                 break;
						case 36: sAdjective = "mágico(a)";             break;
						case 37: sAdjective = "incomum";             break;
						case 38: sAdjective = "poder";                 break;
						case 39: sAdjective = "poder";                 break;
						case 40: sAdjective = "grandiosidade";         break;
						case 41: sAdjective = "mágico(a)";             break;
						case 42: sAdjective = "supremacia";             break;
						case 43: sAdjective = "o todo-poderoso";     break;
						case 44: sAdjective = "o sagrado";             break;
						case 45: sAdjective = "magnificência";         break;
						case 46: sAdjective = "excelência";             break;
						case 47: sAdjective = "glória";                 break;
						case 48: sAdjective = "mistério";             break;
						case 49: sAdjective = "o divino";             break;
						case 50: sAdjective = "o esquecido";         break;
						case 51: sAdjective = "lenda";                 break;
						case 52: sAdjective = "o perdido";             break;
						case 53: sAdjective = "os antigos";             break;
						case 54: sAdjective = "maravilha";             break;
						case 55: sAdjective = "o poderoso";             break;
						case 56: sAdjective = "proeza";                 break;
						case 57: sAdjective = "nobreza";             break;
						case 58: sAdjective = "misticismo";             break;
						case 59: sAdjective = "encantamento";         break;
						case 60: sAdjective = "o Templário";        break;
						case 61: sAdjective = "o Ladrão";        break;
						case 62: sAdjective = "o Ilusionista";    break;
						case 63: sAdjective = "a Princesa";        break;
						case 64: sAdjective = "o Invocador";        break;
						case 65: sAdjective = "a Sacerdotisa";        break;
						case 66: sAdjective = "o Conjurador";        break;
						case 67: sAdjective = "o Bandido";        break;
						case 68: sAdjective = "a Baronesa";        break;
						case 69: sAdjective = "o Mago";        break;
						case 70: sAdjective = "o Clérigo";        break;
						case 71: sAdjective = "o Monge";        break;
						case 72: sAdjective = "o Menestrel";        break;
						case 73: sAdjective = "o Defensor";        break;
						case 74: sAdjective = "o Cavaleiro";        break;
						case 75: sAdjective = "o Mágico";        break;
						case 76: sAdjective = "a Bruxa";        break;
						case 77: sAdjective = "o Lutador";        break;
						case 78: sAdjective = "o Buscador";        break;
						case 79: sAdjective = "o Abatedor";        break;
						case 80: sAdjective = "o Ranger";        break;
						case 81: sAdjective = "o Bárbaro";        break;
						case 82: sAdjective = "o Explorador";        break;
						case 83: sAdjective = "o Herege";        break;
						case 84: sAdjective = "o Gladiador";        break;
						case 85: sAdjective = "o Sábio";        break;
						case 86: sAdjective = "o Ladrão";        break;
						case 87: sAdjective = "o Paladino";        break;
						case 88: sAdjective = "o Bardo";        break;
						case 89: sAdjective = "o Adivinho";        break;
						case 90: sAdjective = "a Dama";        break;
						case 91: sAdjective = "o Fora-da-lei";        break;
						case 92: sAdjective = "o Profeta";        break;
						case 93: sAdjective = "o Mercenário";        break;
						case 94: sAdjective = "o Aventureiro";        break;
						case 95: sAdjective = "a Encantadora";    break;
						case 96: sAdjective = "a Rainha";        break;
						case 97: sAdjective = "o Batedor";        break;
						case 98: sAdjective = "o Místico";        break;
						case 99: sAdjective = "o Mago";        break;
						case 100: sAdjective = "o Viajante";        break;
						case 101: sAdjective = "o Invocador";        break;
						case 102: sAdjective = "o Guerreiro";        break;
						case 103: sAdjective = "a Feiticeira";    break;
						case 104: sAdjective = "o Vidente";        break;
						case 105: sAdjective = "o Caçador";        break;
						case 106: sAdjective = "o Cavaleiro";        break;
						case 107: sAdjective = "o Necromante";    break;
						case 108: sAdjective = "o Xamã";        break;
						case 109: sAdjective = "o Príncipe";        break;
						case 110: sAdjective = "o Sacerdote";        break;
						case 111: sAdjective = "o Barão";        break;
						case 112: sAdjective = "o Bruxo";        break;
						case 113: sAdjective = "o Senhor";        break;
						case 114: sAdjective = "o Encantador";    break;
						case 115: sAdjective = "o Rei";        break;
						case 116: sAdjective = "o Feiticeiro";        break;
					}
				}
			}

			return sAdjective;
		}

		public static string MagicItemName( Item item, Mobile m, Region from )
		{
			bool isOriental = false;
			bool isEvil = false;

			string RegionName = "";
			string xName = ContainerFunctions.GetOwner( "property" );

			if ( from is DungeonRegion && Utility.RandomBool() ){ RegionName = from.Name; }
			else
			{
				switch( Utility.RandomMinMax( 0, 3 ) )
				{
					case 0: RegionName = Server.Misc.RandomThings.MadeUpDungeon(); 												break;
					case 1: RegionName = Server.Misc.RandomThings.MadeUpCity(); 												break;
					case 2: RegionName = Server.Misc.RandomThings.GetRandomScenePainting(); 									break;
					case 3: RegionName = "the " + RandomThings.GetRandomKingdomName() + " " + RandomThings.GetRandomKingdom();	break;
				}
			}

			string OwnerName = RandomThings.GetRandomName();
			if ( ( item.ItemID >= 0x2776 && item.ItemID <= 0x27FA ) || Server.Misc.GetPlayerInfo.OrientalPlay( m ) == true )
			{
				isOriental = true;
				OwnerName = RandomThings.GetRandomOrientalName();
				xName = OwnerName + " " + MagicItemAdj( "end", true, false, item.ItemID );
			}
			else if ( Server.Misc.GetPlayerInfo.EvilPlay( m ) )
			{
				isEvil = true;
				xName = OwnerName + " " + MagicItemAdj( "end", false, true, item.ItemID );
			}

			if ( OwnerName.EndsWith( "s" ) )
			{
				OwnerName = OwnerName + "'";
			}
			else
			{
				OwnerName = OwnerName + "'s";
			}
			if ( Utility.RandomBool() ){ OwnerName = NameList.RandomName( "cultures" ); }

			string sAdjective = "unusual";
			string eAdjective = "might";

			sAdjective = MagicItemAdj( "start", isOriental, isEvil, item.ItemID );
			eAdjective = MagicItemAdj( "end", isOriental, isEvil, item.ItemID );

			string name = "item";

			if ( item.Name != null && item.Name != "" ){ name = item.Name; }
			if ( name == "item" ){ item.SyncName(); name = item.Name; }

			if ( isEvil && item is WizardWand && Utility.RandomMinMax( 1, 4 ) != 1 )
			{
				item.ItemID = Utility.RandomList( 0x269D, 0x269E );
			}

			if ( item is BambooFlute ){ name = "flauta"; }
			else if ( item is Drums ){ name = "tambor"; }
			else if ( item is Harp ){ name = "harpa"; }
			else if ( item is LapHarp ){ name = "harpa"; }
			else if ( item is Lute ){ name = "alaúde"; if ( Utility.RandomMinMax( 1, 2 ) == 1 ){ name = "bandolim"; } }
			else if ( item is Tambourine ){ name = "pandeiro"; }
			else if ( item is TambourineTassel ){ name = "pandeiro"; }
			else if ( item is Trumpet ){ name = "trombeta"; }

			int FirstLast = Utility.RandomMinMax( 0, 1 );

			int HaveNewName = 0;

			if ( SkipMagicName( item ) )
			{
				// DO NOT CHANGE THE NAME OF
			}
			else if ( FirstLast == 0 ) // FIRST COMES ADJECTIVE
			{
				int val = 15; if ( !item.Movable ){ val = 7; }
				switch( Utility.RandomMinMax( 0, val ) )
				{
					case 0: name = sAdjective + " " + name + " of " + xName; 										HaveNewName = 1; break;
					case 1: name = name + " of " + xName; 															HaveNewName = 1; break;
					case 2: name = sAdjective + " " + name; 														HaveNewName = 1; break;
					case 3: name = sAdjective + " " + name + " of " + xName; 										HaveNewName = 1; break;
					case 4: name = name + " of " + xName; 															HaveNewName = 1; break;
					case 5: name = sAdjective + " " + name; 														HaveNewName = 1; break;
					case 6: if ( RegionName != "" ){ name = sAdjective + " " + name + " from " + RegionName;	}	HaveNewName = 1; break;
					case 7: if ( RegionName != "" ){ name = name + " from " + RegionName;	}						break;
				}
			}
			else // FIRST COMES OWNER
			{
				int val = 11; if ( !item.Movable ){ val = 5; }
				switch( Utility.RandomMinMax( 0, val ) )
				{
					case 0: name = OwnerName + " " + name + " of " + eAdjective; 									HaveNewName = 1; break;
					case 1: name = name + " of " + eAdjective; 														HaveNewName = 1; break;
					case 2: name = OwnerName + " " + name; 															HaveNewName = 1; break;
					case 3: name = OwnerName + " " + sAdjective + " " + name; 										HaveNewName = 1; break;
					case 4: if ( RegionName != "" ){ name = sAdjective + " " + name + " from " + RegionName;	}	HaveNewName = 1; break;
					case 5: if ( RegionName != "" ){ name = name + " from " + RegionName;	}						break;
				}
			}

			if ( Server.Misc.GetPlayerInfo.EvilPlay( m ) == true && HaveNewName > 0 )
			{
				item.Hue = Utility.RandomEvilHue();
			}

			if ( name.Contains("The The") ){ name = name.Replace("The The", "The"); }
			if ( name.Contains("the the") ){ name = name.Replace("the the", "the"); }
			if ( name.Contains("The the") ){ name = name.Replace("The the", "the"); }
			if ( name.Contains("the The") ){ name = name.Replace("the The", "the"); }

			return name;
		}

		public static string MagicWandOwner()
		{
			string OwnerName = RandomThings.GetRandomName();

			if ( OwnerName.EndsWith( "s" ) )
			{
				OwnerName = OwnerName + "'";
			}
			else
			{
				OwnerName = OwnerName + "'s";
			}
			if ( Utility.RandomBool() ){ OwnerName = NameList.RandomName( "cultures" ); }

			return OwnerName;
		}

		public static bool SkipMagicName( Item item )
		{
			if ( item.ArtifactLevel > 0 )
				return true;

			if ( item.NotModAble && !(item is BaseTrinket) )
				return true;

			return false;
		}

		public static void SpecialName( Item item, Mobile m, Region reg )
		{
			bool isOriental = GetPlayerInfo.OrientalPlay( m );
			bool isEvil = GetPlayerInfo.EvilPlay( m );

			bool colorModHue = false;

			if ( SkipMagicName( item ) )
				return;

			if ( !( item is BaseArmor || item is BaseWeapon || item is BaseInstrument || item is BaseClothing || item is BaseTrinket || item is BaseQuiver || item is Spellbook ) )
				return;

			string RegionName = null;
			string xName = ContainerFunctions.GetOwner( "property" );

			if ( reg is DungeonRegion && Utility.RandomBool() )
				RegionName = reg.Name;
			else
			{
				switch( Utility.Random( 4 ) )
				{
					case 0: RegionName = RandomThings.MadeUpDungeon(); 															break;
					case 1: RegionName = RandomThings.MadeUpCity(); 															break;
					case 2: RegionName = RandomThings.GetRandomScenePainting(); 												break;
					case 3: RegionName = "the " + RandomThings.GetRandomKingdomName() + " " + RandomThings.GetRandomKingdom();	break;
				}
			}

			string OwnerName = RandomThings.GetRandomName();

			if ( isOriental )
			{
				OwnerName = RandomThings.GetRandomOrientalName();
				xName = OwnerName + " " + MagicItemAdj( "end", true, false, item.ItemID );
			}
			else if ( isEvil )
				xName = OwnerName + " " + MagicItemAdj( "end", false, true, item.ItemID );

			if ( OwnerName.EndsWith( "s" ) )
				OwnerName = OwnerName + "'";
			else
				OwnerName = OwnerName + "'s";

			if ( Utility.RandomBool() )
				OwnerName = NameList.RandomName( "cultures" );

			string sAdjective = "unusual";
			string eAdjective = "might";

			sAdjective = MagicItemAdj( "start", isOriental, isEvil, item.ItemID );
			eAdjective = MagicItemAdj( "end", isOriental, isEvil, item.ItemID );

			string name = item.Name;
				if ( item is MagicalWand )
					name = "magic wand";

			string subname = null;

			if ( isEvil && item is WizardWand && Utility.Random( 4 ) > 0 )
				item.ItemID = Utility.RandomList( 0x269D, 0x269E );

			bool HasNewName = false;

			if ( Utility.RandomBool() ) // FIRST COMES ADJECTIVE
			{
				int val = 15; if ( !item.Movable ){ val = 7; }

				int num = Utility.RandomMinMax( 0, val );
					if ( RegionName != null && ( val == 6 || val == 7 ) )
						val = 5;

				switch( num )
				{
					case 0: name = sAdjective + " " + name + " of";		subname = xName; 			HasNewName = true; colorModHue = true; 	break;
					case 1: name = name + " of";						subname = xName; 			HasNewName = true; 						break;
					case 2: name = sAdjective + " " + name; 										HasNewName = true; colorModHue = true; 	break;
					case 3: name = sAdjective + " " + name + " of";		subname = xName; 			HasNewName = true; colorModHue = true; 	break;
					case 4: name = name + " of";						subname = xName; 			HasNewName = true; 						break;
					case 5: name = sAdjective + " " + name; 										HasNewName = true; colorModHue = true; 	break;
					case 6: name = sAdjective + " " + name + " from";	subname = RegionName;		HasNewName = true; colorModHue = true; 	break;
					case 7: name = name + " from";						subname = RegionName;		HasNewName = true; 						break;
				}
			}
			else // FIRST COMES OWNER
			{
				int val = 11; if ( !item.Movable ){ val = 5; }

				int num = Utility.RandomMinMax( 0, val );
					if ( RegionName != null && ( val == 4 || val == 5 ) )
						val = 3;

				switch( Utility.RandomMinMax( 0, val ) )
				{
					case 0: name = OwnerName + " " + name + " of";				subname = eAdjective; 		HasNewName = true; colorModHue = true; 	break;
					case 1: name = name + " of";								subname = eAdjective; 		HasNewName = true; colorModHue = true; 	break;
					case 2: name = OwnerName + " " + name; 													HasNewName = true; 						break;
					case 3: name = OwnerName + " " + sAdjective + " " + name;								HasNewName = true; colorModHue = true; 	break;
					case 4: name = sAdjective + " " + name + " from";			subname = RegionName;		HasNewName = true; colorModHue = true; 	break;
					case 5: name = name + " from";								subname = RegionName;		HasNewName = true; 						break;
				}
			}

			if ( HasNewName )
			{
				string textHue = "0080FF";

				if ( isEvil && colorModHue )
				{
					item.Hue = Utility.RandomEvilHue();
					textHue = "DE1D1D";
				}
				else if ( colorModHue && item is MagicalWand && Utility.RandomBool() )
					item.Hue = GetRandomMetallicColor();

				if ( name.Contains("The The") ){ name = name.Replace("The The", "The"); }
				if ( name.Contains("the the") ){ name = name.Replace("the the", "the"); }
				if ( name.Contains("The the") ){ name = name.Replace("The the", "the"); }
				if ( name.Contains("the The") ){ name = name.Replace("the The", "the"); }

				item.ColorText1 = name;
				item.ColorHue1 = textHue;

				if ( subname != null )
				{
					if ( subname.Contains("The The") ){ subname = subname.Replace("The The", "The"); }
					if ( subname.Contains("the the") ){ subname = subname.Replace("the the", "the"); }
					if ( subname.Contains("The the") ){ subname = subname.Replace("The the", "the"); }
					if ( subname.Contains("the The") ){ subname = subname.Replace("the The", "the"); }
					item.ColorText2 = subname;
					item.ColorHue2 = textHue;
				}
			}
		}
	}
}