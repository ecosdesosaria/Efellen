using System;	
using Server;	
using System.Collections;	
using Server.Misc;	
using Server.Items;	
using Server.Network;	
using Server.Commands;	
using Server.Commands.Generic;	
using Server.Mobiles;	
using Server.Accounting;	

namespace Server.Misc
{
    class TavernPatrons
    {
		public static void RemoveSomeGear( Mobile m, bool helm )
		{
			m.CoinPurse = 1234567890;
			if ( helm )
				m.DataStoreInt2 = 1;

			RemoveSomeStuff( m );
		}

		public static void RemoveSomeStuff( Mobile m )
		{
			bool helm = false;
			if ( m.DataStoreInt2 == 1 )
				helm = true;

			if ( m.FindItemOnLayer( Layer.OneHanded ) != null ) { m.FindItemOnLayer( Layer.OneHanded ).Delete(); }
			if ( m.FindItemOnLayer( Layer.TwoHanded ) != null ) { m.FindItemOnLayer( Layer.TwoHanded ).Delete(); }
			if ( m.FindItemOnLayer( Layer.FirstValid ) != null && m.FindItemOnLayer( Layer.FirstValid ) is BaseShield ) { m.FindItemOnLayer( Layer.FirstValid ).Delete(); }
			if ( m.FindItemOnLayer( Layer.FirstValid ) != null && m.FindItemOnLayer( Layer.FirstValid ) is BaseWeapon ) { m.FindItemOnLayer( Layer.FirstValid ).Delete(); }
			if ( m.FindItemOnLayer( Layer.Helm ) != null && helm ) { if ( m.FindItemOnLayer( Layer.Helm ) is BaseArmor ){ m.FindItemOnLayer( Layer.Helm ).Delete(); } }
		}

		/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		public static string GetRareLocation( Mobile speaker, bool toPlayer, bool MixTogether )
		{
			string what = "";	
			string where = "";	
			string say = QuestCharacters.RandomWords() + " os matou, tenho certeza.";

			int rare = Utility.RandomMinMax( 1, 11 );	

			if ( rare == 1 )
			{
				what = "Exodus";	
				foreach ( Mobile mob in World.Mobiles.Values )
				if ( mob is Exodus )
				{
					where = Server.Misc.Worlds.GetRegionName( mob.Map, mob.Location );	
				}
			}
			else if ( rare == 2 )
			{
				what = "Jormungandr";	
				foreach ( Mobile mob in World.Mobiles.Values )
				if ( mob is Jormungandr )
				{
					where = Server.Misc.Worlds.GetRegionName( mob.Map, mob.Location );	

					if ( where == "the Bottle World of Kuldar" ){         where = "as águas do Mar de Kuldar"; }
					else if ( where == "the Land of Ambrosia" ){         where = "as águas dos Lagos de Ambrosia"; }
					else if ( where == "the Island of Umber Veil" ){     where = "as águas do Mar de Umber"; }
					else if ( where == "the Land of Lodoria" ){         where = "as águas do Oceano de Lodoria"; }
					else if ( where == "the Underworld" ){                 where = "as águas do Lago Carthax"; }
					else if ( where == "the Serpent Island" ){             where = "as águas dos Mares da Serpente"; }
					else if ( where == "the Isles of Dread" ){             where = "as águas do Mar do Pavor"; }
					else if ( where == "the Savaged Empire" ){             where = "as águas dos Mares Selvagens"; }
					else if ( where == "the Land of Sosaria" ){         where = "as águas do Oceano de Sosaria"; }
				}
			}
			else
			{
				foreach ( Item target in World.Items.Values )
				if ( target is FlamesBase || target is BaneBase || target is PaganBase || target is RunesBase )
				{
					if ( target is FlamesBase )
					{
						if ( rare == 2 ){ what = "o Livro da Verdade";                 FlamesBase targ2 = (FlamesBase)target; if ( targ2.ItemType == 1){ where = Server.Misc.Worlds.GetRegionName( target.Map, target.Location ); } }
						else if ( rare == 3 ){ what = "o Sino da Coragem";         FlamesBase targ3 = (FlamesBase)target; if ( targ3.ItemType == 2){ where = Server.Misc.Worlds.GetRegionName( target.Map, target.Location ); } }
						else if ( rare == 4 ){ what = "a Vela do Amor";         FlamesBase targ4 = (FlamesBase)target; if ( targ4.ItemType == 3){ where = Server.Misc.Worlds.GetRegionName( target.Map, target.Location ); } }
					}
					else if ( target is BaneBase )
					{
						if ( rare == 5 ){ what = "a Balança da Ética";         BaneBase targ5 = (BaneBase)target; if ( targ5.ItemType == 1){ where = Server.Misc.Worlds.GetRegionName( target.Map, target.Location ); } }
						else if ( rare == 6 ){ what = "o Orbe da Lógica";             BaneBase targ6 = (BaneBase)target; if ( targ6.ItemType == 2){ where = Server.Misc.Worlds.GetRegionName( target.Map, target.Location ); } }
						else if ( rare == 7 ){ what = "a Lanterna da Disciplina";     BaneBase targ7 = (BaneBase)target; if ( targ7.ItemType == 3){ where = Server.Misc.Worlds.GetRegionName( target.Map, target.Location ); } }
					}
					else if ( target is PaganBase )
					{
						if ( rare == 8 ){ what = "o Soprar do Ar";                 PaganBase targ8 = (PaganBase)target; if ( targ8.ItemType == 1){ where = Server.Misc.Worlds.GetRegionName( target.Map, target.Location ); } }
						else if ( rare == 9 ){ what = "a Língua da Chama";         PaganBase targ9 = (PaganBase)target; if ( targ9.ItemType == 2){ where = Server.Misc.Worlds.GetRegionName( target.Map, target.Location ); } }
						else if ( rare == 10 ){ what = "o Coração da Terra";         PaganBase targ10 = (PaganBase)target; if ( targ10.ItemType == 3){ where = Server.Misc.Worlds.GetRegionName( target.Map, target.Location ); } }
						else if ( rare == 11 ){ what = "a Lágrima dos Mares";         PaganBase targ11 = (PaganBase)target; if ( targ11.ItemType == 4){ where = Server.Misc.Worlds.GetRegionName( target.Map, target.Location ); } }
					}
					else if ( target is RunesBase )
					{
						what = "o Baú da Virtude";                                 where = Server.Misc.Worlds.GetRegionName( target.Map, target.Location );    
					}
				}
			}

			if ( rare != 2 && where != "" && Utility.RandomBool() ) // CITIZENS LIE HALF THE TIME
			{
				if ( Utility.RandomBool() ){ where = RandomThings.MadeUpDungeon(); }
				else { where = QuestCharacters.SomePlace( null ); }
			}

			if ( where != "" )
			{
				if ( MixTogether )
				{
					say = "";	
					switch( Utility.RandomMinMax( 0, 2 ) )
					{
						case 0: say = "onde se pode encontrar " + what + " em " + where + ""; break;	
						case 1: say = "onde alguém precisaria ir para " + where + " se quiser encontrar " + what + ""; break;	
						case 2: say = "que alguém provavelmente pode encontrar " + what + " se procurar em " + where + ""; break;	
					}
				}
				else if ( toPlayer )
				{
					say = "";	
					switch( Utility.RandomMinMax( 0, 2 ) )
					{
						case 0: say = "Eu descobri onde se pode encontrar " + what + ". Seria preciso ir para " + where + "."; break;	
						case 1: say = "Alguém precisaria ir para " + where + " se quiser encontrar " + what + "."; break;	
						case 2: say = "O " + RandomThings.GetRandomJob() + " em " + RandomThings.GetRandomCity() + " me disse que alguém provavelmente pode encontrar " + what + " se procurar em " + where + "."; break;	
					}
				}
				else if ( speaker is SherryTheMouse )
				{
					say = "";	
					switch( Utility.RandomMinMax( 0, 2 ) )
					{
						case 0: say = "Lord British me contava histórias sobre " + what + ", e como estava em " + where + "."; break;	
						case 1: say = "Alguém no castel foi para " + where + " e viu " + what + "."; break;	
						case 2: say = "Ouvi " + QuestCharacters.RandomWords() + " dizer ao Lord British que " + what + " supostamente estava em " + where + "."; break;	
					}
				}
				else
				{
					say = "";	
					switch( Utility.RandomMinMax( 0, 2 ) )
					{
						case 0: say = "Finalmente descobri onde podemos encontrar " + what + ". Precisamos ir para " + where + "."; break;	
						case 1: say = "Precisamos ir para " + where + " se quisermos encontrar " + what + "."; break;	
						case 2: say = "O " + RandomThings.GetRandomJob() + " em " + RandomThings.GetRandomCity() + " me disse que provavelmente podemos encontrar " + what + " se procurarmos em " + where + "."; break;	
					}
				}
			}

			return say;	
		}

		/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		public static string GetEvilTitle()
		{
			string sTitle = "";	
			string myTitle = "";	

			int otitle = Utility.RandomMinMax( 1, 33 );	
			if (otitle == 1){sTitle = "do Sombrio";}
			else if (otitle == 2){sTitle = "do Vil";}
			else if (otitle == 3){sTitle = "do Sepulcro";}
			else if (otitle == 4){sTitle = "dos Mortos";}
			else if (otitle == 5){sTitle = "do Cemitério";}
			else if (otitle == 6){sTitle = "da Torre Negra";}
			else if (otitle == 7){sTitle = "dos Fogos Abaixo";}
			else if (otitle == 8){sTitle = "dos Pântanos";}
			else if (otitle == 9){sTitle = "do Horrendo";}
			else if (otitle == 10){sTitle = "do Imundo";}
			else if (otitle == 11){sTitle = "do Sombrio";}
			else if (otitle == 12){sTitle = "da Noite";}
			else if (otitle == 13){sTitle = "do Funesto";}
			else if (otitle == 14){sTitle = "do Maléfico";}
			else if (otitle == 15){sTitle = "do Irascível";}
			else if (otitle == 16){sTitle = "da Tumba";}
			else if (otitle == 17){sTitle = "das Catacumbas";}
			else if (otitle == 18){sTitle = "das Criptas";}
			else if (otitle == 19){sTitle = "das Terras Mortas";}
			else if (otitle == 20){sTitle = "da Necrópole";}
			else if (otitle == 21){sTitle = "da Tumba do Vampiro";}
			else if (otitle == 22){sTitle = "dos Ermos Assombrados";}
			else if (otitle == 23){sTitle = "dos Olhos Sinistros";}
			else if (otitle == 24){sTitle = "do Pântano Fétido";}
			else if (otitle == 25){sTitle = "da Cidade Destruída";}
			else if (otitle == 26){sTitle = "do Brejo Assombrado";}
			else if (otitle == 27){sTitle = "da Mansão Sombria";}
			else if (otitle == 28){sTitle = "das Colinas Uivantes";}
			else if (otitle == 29){sTitle = "dos Ermos Infernais";}
			else if (otitle == 30){sTitle = "da Aparência Ameaçadora";}
			else if (otitle == 31){sTitle = "das Terras Selvagens";}
			else if (otitle == 32){sTitle = "das Florestas Malignas";}
			else {sTitle = "dos Olhos Odiosos";}

			string sColor = "Perverso";	
			switch( Utility.RandomMinMax( 0, 9 ) )
			{
				case 0: sColor = "Perverso"; break;	
				case 1: sColor = "Vil"; break;	
				case 2: sColor = "Malevolente"; break;	
				case 3: sColor = "Odioso"; break;	
				case 4: sColor = "Sangrento"; break;	
				case 5: sColor = "Nefário"; break;	
				case 6: sColor = "Abominável"; break;	
				case 7: sColor = "Maligno"; break;	
				case 8: sColor = "Perverso"; break;	
				case 9: sColor = "Cruel"; break;	
			}

			switch ( Utility.RandomMinMax( 0, 46 ) )
			{
				case 0: myTitle = "dos Ermos"; break;	
				case 1: myTitle = "da Sepultura"; break;	
				case 2: myTitle = "das Profundezas"; break;	
				case 3: myTitle = "da Capa " + sColor; break;	
				case 4: myTitle = "da Veste " + sColor; break;	
				case 5: myTitle = "da Ordem " + sColor; break;	
				case 6: myTitle = "do Capuz " + sColor; break;	
				case 7: myTitle = "da Sociedade " + sColor; break;	
				case 8: myTitle = "da Máscara " + sColor; break;	
				case 9: myTitle = sTitle; break;	
				case 10: myTitle = sTitle; break;	
				case 11: myTitle = sTitle; break;	
				case 12: myTitle = sTitle; break;	
				case 13: myTitle = sTitle; break;	
				case 14: myTitle = sTitle; break;	
				case 15: myTitle = "do Lich " + sColor; break;	
				case 16: myTitle = "do Fantasma " + sColor; break;	
				case 17: myTitle = "do Demônio " + sColor; break;	
				case 18: myTitle = "do Castelo " + sColor; break;	
				case 19: myTitle = "da Caveira " + sColor; break;	
				case 20: myTitle = "da Tumba " + sColor; break;	
				case 21: myTitle = "da Casa " + sColor; break;	
				case 22: myTitle = "o " + sColor; break;	
				case 23: myTitle = "o Necromante"; break;	
				case 24: myTitle = "o Bruxo"; break;	
				case 25: myTitle = "a Bruxa"; break;	
				case 26: myTitle = "o Agente Funerário"; break;	
				case 27: myTitle = "o Torturador"; break;	
				case 28: myTitle = "o Senhor do Pavor"; break;	
				case 29: myTitle = "o Cavaleiro da Morte"; break;	
				case 30: myTitle = "o Ladrão"; break;	
				case 31: myTitle = "o Assassino"; break;	
				case 32: myTitle = "o Salteador"; break;	
				case 33: myTitle = "o Diabolista"; break;	
				case 34: myTitle = "o Selvagem"; break;	
				case 35: myTitle = "o Imundo"; break;	
				case 36: myTitle = "o Medonho"; break;	
				case 37: myTitle = "o Assombrado"; break;	
				case 38: myTitle = "o Frenético"; break;	
				case 39: myTitle = "o Repulsivo"; break;	
				case 40: myTitle = "o Irado"; break;	
				case 41: myTitle = "da Carapuça " + sColor; break;	
				case 42: myTitle = "do Olho " + sColor; break;	
				case 43: myTitle = "do Chapéu " + sColor; break;	
				case 44: myTitle = "da Luva " + sColor; break;	
				case 45: myTitle = "do Véu " + sColor; break;	
				case 46: myTitle = "do Sudário " + sColor; break;
			}
			return myTitle;	
		}

		public static string GetTitle()
		{
			string sTitle = "";	
			string myTitle = "";	

			int otitle = Utility.RandomMinMax( 1, 33 );	
			if (otitle == 1){sTitle = "do Norte";}
			else if (otitle == 2){sTitle = "do Sul";}
			else if (otitle == 3){sTitle = "do Leste";}
			else if (otitle == 4){sTitle = "do Oeste";}
			else if (otitle == 5){sTitle = "da Cidade";}
			else if (otitle == 6){sTitle = "das Colinas";}
			else if (otitle == 7){sTitle = "das Montanhas";}
			else if (otitle == 8){sTitle = "das Planícies";}
			else if (otitle == 9){sTitle = "das Florestas";}
			else if (otitle == 10){sTitle = "da Luz";}
			else if (otitle == 11){sTitle = "das Trevas";}
			else if (otitle == 12){sTitle = "da Noite";}
			else if (otitle == 13){sTitle = "do Mar";}
			else if (otitle == 14){sTitle = "do Deserto";}
			else if (otitle == 15){sTitle = "da Ordem";}
			else if (otitle == 16){sTitle = "da Floresta";}
			else if (otitle == 17){sTitle = "da Neve";}
			else if (otitle == 18){sTitle = "da Costa";}
			else if (otitle == 19){sTitle = "dos Ermos Áridos";}
			else if (otitle == 20){sTitle = "da Testa Proeminente";}
			else if (otitle == 21){sTitle = "da Cidade Ciclópica";}
			else if (otitle == 22){sTitle = "dos Ermos do Pavor";}
			else if (otitle == 23){sTitle = "dos Olhos Sinistros";}
			else if (otitle == 24){sTitle = "do Pântano Fétido";}
			else if (otitle == 25){sTitle = "da Cidade Esquecida";}
			else if (otitle == 26){sTitle = "do Brejo Assombrado";}
			else if (otitle == 27){sTitle = "do Vale Escondido";}
			else if (otitle == 28){sTitle = "das Colinas Uivantes";}
			else if (otitle == 29){sTitle = "dos Picos Escarpados";}
			else if (otitle == 30){sTitle = "da Aparência Ameaçadora";}
			else if (otitle == 31){sTitle = "da Ilha Selvagem";}
			else if (otitle == 32){sTitle = "das Florestas Emaranhadas";}
			else {sTitle = "dos Olhos Vigilantes";}

			string sColor = "Vermelho";	
			switch( Utility.RandomMinMax( 0, 9 ) )
			{
				case 0: sColor = "Preto"; break;	
				case 1: sColor = "Azul"; break;	
				case 2: sColor = "Cinza"; break;	
				case 3: sColor = "Verde"; break;	
				case 4: sColor = "Vermelho"; break;	
				case 5: sColor = "Marrom"; break;	
				case 6: sColor = "Laranja"; break;	
				case 7: sColor = "Amarelo"; break;	
				case 8: sColor = "Roxo"; break;	
				case 9: sColor = "Branco"; break;	
			}

			string gColor = "Dourado";	
			switch( Utility.RandomMinMax( 0, 11 ) )
			{
				case 0: gColor = "Dourado"; break;	
				case 1: gColor = "Prata"; break;	
				case 2: gColor = "Arcano"; break;	
				case 3: gColor = "Ferro"; break;	
				case 4: gColor = "Aço"; break;	
				case 5: gColor = "Esmeralda"; break;	
				case 6: gColor = "Rubi"; break;	
				case 7: gColor = "Bronze"; break;	
				case 8: gColor = "Jade"; break;	
				case 9: gColor = "Safira"; break;	
				case 10: gColor = "Cobre"; break;	
				case 11: gColor = "Real"; break;	
			}

			string kKiller = "Gigantes";	
			switch( Utility.RandomMinMax( 0, 12 ) )
			{
				case 0: kKiller = "Gigantes"; break;	
				case 1: kKiller = "Dragões"; break;	
				case 2: kKiller = "Ogres"; break;	
				case 3: kKiller = "Trolls"; break;	
				case 4: kKiller = "Demônios"; break;	
				case 5: kKiller = "Diabos"; break;	
				case 6: kKiller = "Drows"; break;	
				case 7: kKiller = "Orcs"; break;	
				case 8: kKiller = "Minotauros"; break;	
				case 9: kKiller = "Monstros"; break;	
				case 10: kKiller = "Mortos-vivos"; break;	
				case 11: kKiller = "Serpentes"; break;	
				case 12: kKiller = "Vampiros"; break;	
			}

			string mKiller = "Gigante";	
			switch( Utility.RandomMinMax( 0, 12 ) )
			{
				case 0: mKiller = "Gigante"; break;	
				case 1: mKiller = "Dragão"; break;	
				case 2: mKiller = "Ogre"; break;	
				case 3: mKiller = "Troll"; break;	
				case 4: mKiller = "Demônio"; break;	
				case 5: mKiller = "Diabo"; break;	
				case 6: mKiller = "Drow"; break;	
				case 7: mKiller = "Orc"; break;	
				case 8: mKiller = "Minotauro"; break;	
				case 9: mKiller = "Monstro"; break;	
				case 10: mKiller = "Morto-vivo"; break;	
				case 11: mKiller = "Serpente"; break;	
				case 12: mKiller = "Vampiro"; break;	
			}

			string aKiller = "Abatedor";	
			switch( Utility.RandomMinMax( 0, 4 ) )
			{
				case 0: aKiller = "Abatedor"; break;	
				case 1: aKiller = "Matador"; break;	
				case 2: aKiller = "Açougueiro"; break;	
				case 3: aKiller = "Carrasco"; break;	
				case 4: aKiller = "Caçador"; break;	
			}

			switch ( Utility.RandomMinMax( 0, 107 ) )
			{
				case 0: myTitle = "do Alto"; break;	
				case 1: myTitle = "de Longe"; break;	
				case 2: myTitle = "de Baixo"; break;	
				case 3: myTitle = "da Capa " + sColor; break;	
				case 4: myTitle = "da Veste " + sColor; break;	
				case 5: myTitle = "da Ordem " + sColor; break;	
				case 6: myTitle = "do Escudo " + gColor; break;	
				case 7: myTitle = "da Espada " + gColor; break;	
				case 8: myTitle = "do Elmo " + gColor; break;	
				case 9: myTitle = sTitle; break;	
				case 10: myTitle = sTitle; break;	
				case 11: myTitle = sTitle; break;	
				case 12: myTitle = sTitle; break;	
				case 13: myTitle = sTitle; break;	
				case 14: myTitle = sTitle; break;	
				case 15: myTitle = sTitle; break;	
				case 16: myTitle = sTitle; break;	
				case 17: myTitle = sTitle; break;	
				case 18: myTitle = sTitle; break;	
				case 19: myTitle = sTitle; break;	
				case 20: myTitle = sTitle; break;	
				case 21: myTitle = sTitle; break;	
				case 22: myTitle = "o " + sColor; break;	
				case 23: myTitle = "o Adepto"; break;	
				case 24: myTitle = "o Nômade"; break;	
				case 25: myTitle = "o Antiquário"; break;	
				case 26: myTitle = "o Arcano"; break;	
				case 27: myTitle = "o Arcaico"; break;	
				case 28: myTitle = "o Bárbaro"; break;	
				case 29: myTitle = "o Batráquio"; break;	
				case 30: myTitle = "o Lutador"; break;	
				case 31: myTitle = "o Bilioso"; break;	
				case 32: myTitle = "o Audaz"; break;	
				case 33: myTitle = "o Destemido"; if (Utility.RandomMinMax( 1, 2 ) == 1){myTitle = "o Bravo";} break;	
				case 34: myTitle = "o Selvagem"; if (Utility.RandomMinMax( 1, 2 ) == 1){myTitle = "o Civilizado";} break;	
				case 35: myTitle = "o Colecionador"; break;	
				case 36: myTitle = "o Críptico"; break;	
				case 37: myTitle = "o Curioso"; break;	
				case 38: myTitle = "o Dândi"; break;	
				case 39: myTitle = "o Audacioso"; break;	
				case 40: myTitle = "o Decadente"; break;	
				case 41: myTitle = "o Explorador"; break;	
				case 42: myTitle = "o Distante"; break;	
				case 43: myTitle = "o Místico"; break;	
				case 44: myTitle = "o Exótico"; break;	
				case 45: myTitle = "o Explorador"; break;	
				case 46: myTitle = "o Belo"; break;	
				case 47: myTitle = "o Forte"; if (Utility.RandomMinMax( 1, 2 ) == 1){myTitle = "o Fraco";} break;	
				case 48: myTitle = "o Volúvel"; break;
				case 49:
					int iDice = Utility.RandomMinMax( 1, 10 );	
					if (iDice == 1){myTitle = "o Primeiro";}
					else if (iDice == 2){myTitle = "o Segundo";}
					else if (iDice == 3){myTitle = "o Terceiro";}
					else if (iDice == 4){myTitle = "o Quarto";}
					else if (iDice == 5){myTitle = "o Quinto";}
					else if (iDice == 6){myTitle = "o Sexto";}
					else if (iDice == 7){myTitle = "o Sétimo";}
					else if (iDice == 8){myTitle = "o Oitavo";}
					else if (iDice == 9){myTitle = "o Nono";}
					else {myTitle = "o Décimo";}
					break;	
				case 50: myTitle = "o Imundo"; break;	
				case 51: myTitle = "o Furtivo"; break;	
				case 52: myTitle = "o Apostador"; break;	
				case 53: myTitle = "o Medonho"; break;	
				case 54: myTitle = "o Giboso"; break;	
				case 55: myTitle = "o Grande"; break;	
				case 56: myTitle = "o Grisalho"; break;	
				case 57: myTitle = "o Rude"; break;	
				case 58: myTitle = "o Espiritual"; break;	
				case 59: myTitle = "o Assombrado"; break;	
				case 60: myTitle = "o Calmo"; if (Utility.RandomMinMax( 1, 2 ) == 1){myTitle = "o Frenético";} break;	
				case 61:
					int iDice2 = Utility.RandomMinMax( 1, 4 );	
					if (iDice2 == 1){myTitle = "o Encapuçado";}
					else if (iDice2 == 2){myTitle = "o Encapotado";}
					else if (iDice2 == 3){myTitle = "o Capuzado";}
					else {myTitle = "o Vestido";}
					break;	
				case 62: myTitle = "o Caçador"; break;	
				case 63: myTitle = "o Imponente"; break;	
				case 64: myTitle = "o Irreverente"; break;	
				case 65: myTitle = "o Repulsivo"; break;	
				case 66:
					int iDice3 = Utility.RandomMinMax( 1, 3 );	
					if (iDice3 == 1){myTitle = "o Quieto";}
					else if (iDice3 == 2){myTitle = "o Silencioso";}
					else {myTitle = "o Barulhento";}
					break;	
				case 67: myTitle = "o Encantador"; break;	
				case 68: myTitle = "o Envolto"; break;	
				case 69: myTitle = "o Mascarado"; if (Utility.RandomMinMax( 1, 2 ) == 1){myTitle = "o Velejado";} break;	
				case 70: myTitle = "o Misericordioso"; if (Utility.RandomMinMax( 1, 2 ) == 1){myTitle = "o Impiedoso";} break;	
				case 71: myTitle = "o Mercurial"; break;	
				case 72: myTitle = "o Poderoso"; break;	
				case 73: myTitle = "o Melancólico"; break;	
				case 74: myTitle = "o Mutável"; break;	
				case 75: myTitle = "o Misterioso"; if (Utility.RandomMinMax( 1, 2 ) == 1){myTitle = "o Desconhecido";} break;	
				case 76: myTitle = "o Obscuro"; break;	
				case 77: myTitle = "o Velho"; if (Utility.RandomMinMax( 1, 2 ) == 1){myTitle = "o Jovem";} break;	
				case 78: myTitle = "o Sombrio"; break;	
				case 79: myTitle = "o Peculiar"; break;	
				case 80: myTitle = "o Perceptivo"; break;	
				case 81: myTitle = "o Pio"; break;	
				case 82: myTitle = "o Rápido"; if (Utility.RandomMinMax( 1, 2 ) == 1){myTitle = "o Lento";} break;	
				case 83: myTitle = "o Esfarrapado"; break;	
				case 84: myTitle = "o Pronto"; break;	
				case 85: myTitle = "o Áspero"; break;	
				case 86: myTitle = "o Rugoso"; break;	
				case 87: myTitle = "o Cicatrizado"; break;	
				case 88: myTitle = "o Buscador"; break;	
				case 89: myTitle = "o Sombrio"; break;	
				case 90: myTitle = "o Baixo"; if (Utility.RandomMinMax( 1, 2 ) == 1){myTitle = "o Alto";} break;	
				case 91: myTitle = "o Firme"; break;	
				case 92: myTitle = "o Sobrenatural"; break;	
				case 93: myTitle = "o Inesperado"; break;	
				case 94: myTitle = "o Incompreensível"; break;	
				case 95: myTitle = "o Verboso"; break;	
				case 96: myTitle = "o Vigoroso"; break;	
				case 97: myTitle = "o Viajante"; if (Utility.RandomMinMax( 1, 2 ) == 1){myTitle = "o Andarilho";} break;	
				case 98: myTitle = "o Cauteloso"; break;	
				case 99: myTitle = "o Estranho"; break;	
				case 100: myTitle = "o Firme"; if (Utility.RandomMinMax( 1, 2 ) == 1){myTitle = "o Despreparado";} break;	
				case 101: myTitle = "o Gentil"; if (Utility.RandomMinMax( 1, 2 ) == 1){myTitle = "o Cruel";} break;	
				case 102: myTitle = "o Perdido"; if (Utility.RandomMinMax( 1, 2 ) == 1){myTitle = "o Exilado";} break;	
				case 103: myTitle = "o Descuidado"; if (Utility.RandomMinMax( 1, 2 ) == 1){myTitle = "o Desajeitado";} break;	
				case 104: myTitle = "o Esperançoso"; if (Utility.RandomMinMax( 1, 2 ) == 1){myTitle = "o Confiante";} break;	
				case 105: myTitle = "o Irritado"; if (Utility.RandomMinMax( 1, 2 ) == 1){myTitle = "o Tímido";} break;	
				case 106: myTitle = "o " + aKiller + " de " + kKiller; break;	
				case 107: myTitle = "o " + mKiller + " " + aKiller; break;	
			}
			return myTitle;	
		}

		/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		public static string CommonTalk( string sWords, string city, string dungeon, Mobile from, string adventurer, bool useAll )
		{
			string misc = "";

			string relics = QuestCharacters.QuestItems( false );
				if ( Utility.RandomBool() ){ relics = QuestCharacters.ArtyItems( false ); }

			int max = 198; if ( !useAll ){ max = max + 40; }
			switch( Utility.RandomMinMax( 0, max ) )
			{
				case 0: sWords = "um santuário branco brilhante em Sosaria que leva à lua"; break;	
				case 1: sWords = "um castelo de magos do mal governado por um arquimago ainda mais vil"; break;	
				case 2: sWords = "uma caverna no pântano de Lodor que é lar de humanoides escamosos"; break;	
				case 3: sWords = "uma caverna de pixies e um druida louco em Savaged Empire"; break;	
				case 4: sWords = "uma caverna a oeste de Lodoria, que está cheia de serpentes, ettins e trolls"; break;	
				case 5: sWords = "uma cripta sob a Cave of Souls"; break;	
				case 6: sWords = "uma masmorra profunda dos mortos nas terras frias de Lodor"; break;	
				case 7: sWords = "uma semideusa do sangue, e ela retornou"; break;	
				case 8: sWords = "um senhor demoníaco corrompendo o núcleo da Dungeon Ankh"; break;	
				case 9: sWords = "um covil de gárgulas antigas nos pântanos de Savaged Empire"; break;	
				case 10: sWords = "um covil de ursos das cavernas no fundo de Dardin's Pit"; break;	
				case 11: sWords = "um covil de dragões ao norte de Village of Whisper"; break;	
				case 12: sWords = "um covil de harpias na ilha em Lodor"; break;	
				case 13: sWords = "uma Dungeon de Doom na região sudoeste de Sosaria"; break;	
				case 14: sWords = "uma masmorra de feiticeiros orcs conspirando contra nós"; break;	
				case 15: sWords = "um palácio congelado onde habita a rainha do gelo"; break;	
				case 16: sWords = "um fantasma assombrando aquelas ruínas de Savaged Empire"; break;	
				case 17: sWords = "uma lula gigantesca vivendo no fundo do Flooded Temple"; break;	
				case 18: sWords = "um grande número de segredos que podem ser aprendidos na Biblioteca de Bal Tsareth"; break;	
				case 19: sWords = "um grupo de adoradores do demônio na Dungeon Vile"; break;	
				case 20: sWords = "um grupo de ophidians adorando na Serpent Sanctum"; break;	
				case 21: sWords = "uma horda de demônios na Dungeon Torment"; break;	
				case 22: sWords = "um segredo horrível dentro das montanhas de Umber Veil"; break;	
				case 23: sWords = "um covil de vampiros que existe ao norte do santuário branco"; break;	
				case 24: sWords = "um grande grupo de construtores navais em Lodor"; break;	
				case 25: sWords = "um espelho mágico na Dungeon Fire"; break;	
				case 26: sWords = "um labirinto de sebes mágico criado séculos atrás"; break;	
				case 27: sWords = "um portal mágico no fundo da tumba do Faraó em Sosaria"; break;	
				case 28: sWords = "um portal mágico em Savaged Empire"; break;	
				case 29: sWords = "um selo mágico, impedindo que o lich king escape"; break;	
				case 30: sWords = "uma matilha de minotauros guardando aquele antigo labirinto de sebes"; break;	
				case 31: sWords = "uma tumba de Faraós no deserto de Sosaria"; break;	
				case 32: sWords = "um poço de líquido vil no fundo da Dungeon Wicked"; break;	
				case 33: sWords = "um lich poderoso vagando dentro de uma torre em Sosaria, com um espelho mágico"; break;	
				case 34: sWords = "um forte orc primitivo perto do antigo cemitério em Savaged Empire"; break;	
				case 35: sWords = "uma raça de homens-serpente na Dungeon Scorn"; break;	
				case 36: sWords = "uma entrada secreta no antigo cemitério de Savaged Empire"; break;	
				case 37: sWords = "uma passagem secreta no castelo de Umber Veil"; break;	
				case 38: sWords = "um storm giant em um castelo no mar de Savaged Empire"; break;	
				case 39: sWords = "uma passagem sinuosa em Savaged Empire com druidas mortos-vivos à solta"; break;	
				case 40: sWords = "um vale de ciclopes em Savaged Empire"; break;	
				case 41: sWords = "uma passagem subterrânea que conecta as ilhas norte e central de Lodoria"; break;	
				case 42: sWords = "uma mina abandonada ao norte de Grey em Sosaria"; break;	
				case 43: sWords = "um altar em Savaged Empire onde são feitos sacrifícios ao rei dragão"; break;	
				case 44: sWords = "um antigo culto de sangue nas Isles of Dread"; break;	
				case 45: sWords = "uma cripta antiga onde os gárgulas enterravam seus mortos"; break;	
				case 46: sWords = "uma cidade antiga de dark elves nas profundezas de Lodor"; break;	
				case 47: sWords = "um mal antigo sob o labirinto de sebes místico"; break;	
				case 48: sWords = "um covil antigo nas cavernas de Lodor, onde habitam magos e elementais"; break;	
				case 49: sWords = "um lich antigo que tem uma fortaleza insular em Savaged Empire"; break;	
				case 50: sWords = "uma prisão antiga escondida nas areias do deserto da Serpent Island"; break;
				case 51: 
					string land = "Lodor";
					string where = "norte";
					string wyrm = "um dragão antigo";
					switch ( Utility.Random( 8 ) )
					{
						case 0: where = "norte"; break;
						case 1: where = "sul"; break;
						case 2: where = "leste"; break;
						case 3: where = "oeste"; break;
						case 4: where = "nordeste"; break;
						case 5: where = "noroeste"; break;
						case 6: where = "sudeste"; break;
						case 7: where = "sudoeste"; break;
					}
					switch ( Utility.Random( 9 ) )
					{
						case 0: land = "Lodor"; break;
						case 1: land = "Sosaria"; break;
						case 2: land = "Ambrosia"; break;
						case 3: land = "Umber Veil"; break;
						case 4: land = "Kuldar"; break;
						case 5: land = "a Ilha da Serpente"; break;
						case 6: land = "o Savage Empire"; break;
						case 7: land = "o Underworld"; break;
						case 8: land = "as Isles of the Dread"; break;
					}
					switch ( Utility.Random( 12 ) )
					{
						case 0: wyrm = "um dragão antigo"; break;
						case 1: wyrm = "um wyvern antigo"; break;
						case 2: wyrm = "um dragão das sombras"; break;
						case 3: wyrm = "um dragão vulcânico"; break;
						case 4: wyrm = "um dragão ancião"; break;
						case 5: wyrm = "um dragão abismal"; break;
						case 6: wyrm = "um dragão primitivo"; break;
						case 7: wyrm = "um dragão vampírico"; break;
						case 8: wyrm = "um dragão rúnico"; break;
						case 9: wyrm = "um dragão real"; break;
						case 10: wyrm = "um dragão estígio"; break;
						case 11: wyrm = "um dragão noturno"; break;
					}
					sWords = wyrm + " voando pela área de " + where + " em " + land + ""; 
					break;
				case 52: sWords = "um ancient wyrm dormindo abaixo da Dungeon Hate"; break;	
				case 53: sWords = "um passe élfico que leva a grandes artesãos"; break;	
				case 54: sWords = "uma infestação de ratos e cobras na Dungeon Wrath"; break;	
				case 55: sWords = "uma ilha em Savaged Empire com drakes de escamas azuis"; break;	
				case 56: sWords = "um edifício arruinado antigo em Sosaria, com tesouro no porão"; break;	
				case 57: sWords = "uma profecia ork que fala de seu deus retornando para governar"; break;	
				case 58: sWords = "um farol em Savaged Empire com um segredo sob ele"; break;	
				case 59: sWords = "criptas antigas nas profundezas de Savaged Empire"; break;	
				case 60: sWords = "uma caverna em Lodoria que apenas rangers ou exploradores poderiam atravessar"; break;	
				case 61: sWords = "bandidos dentro de uma fortaleza no norte de Sosaria"; break;	
				case 62: sWords = "Castelo de Exodus em ruínas desde que o estranho o destruiu"; break;	
				case 63: sWords = "catacumbas sob a cidade de Lodoria"; break;	
				case 64: sWords = "caldeirões cheios de poções naquelas masmorras"; break;	
				case 65: sWords = "drakkul invocando demônios nas cavernas de gelo de Lodor"; break;	
				case 66: sWords = "demônios sendo liberados sob as areias do deserto de Lodor"; break;	
				case 67: sWords = "uma masmorra chamada Deceit que é lar de um lich muito poderoso"; break;	
				case 68: sWords = "humanos maus em um templo antigo nas montanhas de Lodor"; break;	
				case 69: sWords = "besouros de fogo aninhando na Cave of Fire"; break;	
				case 70: sWords = "muitos elementais diferentes guardando a Tumba do Feiticeiro Caido"; break;	
				case 71: sWords = "homens de gelo que a rainha do gelo invoca"; break;	
				case 72: sWords = "minas em Savaged Empire controladas por homens-rato"; break;	
				case 73: sWords = "minas que os bárbaros escavam, na parte norte das Isles of Dread"; break;	
				case 74: sWords = "criaturas amaldiçoadas poderosas vagando pela Serpent Island"; break;	
				case 75: sWords = "pergaminhos do poder, mas eles só poderiam ser usados em santuários em Ambrosia"; break;	
				case 76: sWords = "pequenos assentamentos de tribos primitivas nas Isles of Dread"; break;	
				case 77: sWords = "algumas das criaturas mais venenosas na Dungeon Bane"; break;	
				case 78: sWords = "algumas ruínas antigas em Sosaria, onde homens-rato agora vivem sob elas"; break;	
				case 79: sWords = "uma Cidade de Névoas que supostamente foi engolida pelo mar séculos atrás"; break;	
				case 80: sWords = "elfos negros invocando demônios na dungeon Destard"; break;	
				case 81: sWords = "pedras místicas que os elfos têm que podem colorir qualquer coisa"; break;	
				case 82: sWords = "um cemitério em Lodoria com um segredo escondido"; break;	
				case 83: sWords = "um pântano em Sosaria com um templo antigo onde um lich aguarda a profecia"; break;	
				case 84: sWords = "criaturas aranhas vis em um castelo nas selvas de Lodor"; break;	
				case 85: sWords = "uma relíquia antiga enterrada em uma sepultura em Umber Veil"; break;	
				case 86: sWords = "um livro de feitiços poderoso em uma casa de mago arruinada"; break;	
				case 87: sWords = "um dragão amigável vivendo sob as ilhas de gelo de Sosaria"; break;	
				case 88: sWords = "uma casa de lenhador abandonada em Sosaria, com algo sob as tábuas do chão"; break;	
				case 89: sWords = "bandidos mantendo um prisioneiro real na parte norte de Sosaria"; break;	
				case 90: sWords = "uma torre em Sosaria onde um lich guarda um cajado poderoso"; break;	
				case 91: sWords = "um crânio de Mondain que está nas profundezas do Castelo de Exodus"; break;	
				case 92: sWords = "este faroleiro em Sosaria vendendo artefatos poderosos encontrados na costa"; break;	
				case 93: sWords = "um lich no pântano de Sosaria carregando um artefato maravilhoso"; break;	
				case 94: sWords = "baús cheios de tesouro naquelas poças mágicas"; break;	
				case 95: sWords = "um poderoso troll lord no fundo do Fosso de Dardin"; break;	
				case 96: sWords = "um rei demônio habitando na dungeon doom que concede desejos"; break;	
				case 97: sWords = "um par de botas místicas que permitem andar sobre lava"; break;	
				case 98: sWords = "minério realmente bom nas Minas de Morinia"; break;	
				case 99: sWords = "este time lord que está enviando pessoas para o passado ou futuro"; break;	
				case 100: sWords = "uma passagem secreta na tumba abaixo do cemitério de Lodoria"; break;	
				case 101: sWords = "uma parede quebrada na tumba da família British"; break;	
				case 102: sWords = "um grupo de ogros e ettins que têm queimado terras agrícolas ao sul da Cidade de Moon"; break;	
				case 103: sWords = "uma sepultura sendo escavada na Vila de Grey"; break;	
				case 104: sWords = "um dragão vulcânico no sul de Lodor"; break;	
				case 105: sWords = "um vampiro mestre em uma ilha em Lodor"; break;	
				case 106: sWords = "apenas necromantes e death knights vivendo naquela ilha morta em Lodor"; break;	
				case 107: sWords = "uma cidade chamada Skara Brae que não foi realmente destruída por um mago"; break;	
				case 108: sWords = "um mago chamado Mangar que construiu uma torre em algum lugar de Sosaria"; break;	
				case 109: sWords = "algum estranho que pôs fim a Exodus"; break;	
				case 110: sWords = "alguém escapando de Skara Brae"; break;	
				case 111: sWords = "um cofre do Black Knight que é grande demais para explorar"; break;	
				case 112: sWords = "o Undermountain podendo ser alcançado através das cavernas dos homens lagarto"; break;	
				case 113: sWords = "alguém que tocou uma bola de cristal na torre de Mangar e desapareceu"; break;	
				case 114: sWords = "uma prateleira de carvalho vazia que na verdade é uma porta para a Guilda dos Ladrões"; break;	
				case 115: sWords = "uma Guilda de Magia Negra escondida por aqui"; break;	
				case 116: sWords = "o Black Knight tendo uma cidade inteira presa em uma garrafa"; break;	
				case 117: sWords = "um mago chamado Vordo que conseguiu fazer uma ilha inteira desaparecer"; break;	
				case 118: sWords = "uma raça perdida de Zuluu que podia cavalgar os lendários dragyns"; break;	
				case 119: sWords = "os dragyns, que eram outrora descendentes de wyrms"; break;	
				case 120: sWords = "criaturas semelhantes a dragões com escamas de gemas"; break;	
				case 121: sWords = "uma ilha aparecendo pelas mãos de Poseidon"; break;	
				case 122: sWords = "um ladrão escapando da cela no castelo de Lord British"; break;	
				case 123: sWords = "alguns salões esquecidos abaixo do castelo de Lord British"; break;	
				case 124: sWords = "alguns cultistas trazendo Kazibal de volta dos mortos"; break;	
				case 125: sWords = "um mal antigo habitando abaixo do Castelo de British"; break;	
				case 126: sWords = "um necromante surgindo do fogo eterno em Sosaria"; break;	
				case 127: sWords = "alguém enterrado com grande tesouro no cemitério em " + city; break;	
				case 128: sWords = "um demilich habitando abaixo de " + city; break;	
				case 129: sWords = "algum " + RandomThings.GetRandomJob() + " vendendo artefatos em " + city; break;	
				case 130: sWords = "alguém que matou o " + RandomThings.GetRandomJob() + " em " + city; break;	
				case 131: sWords = "um clã de orcs que lentamente mutou ao longo dos séculos"; break;	
				case 132: sWords = "marinheiros explorando um recife nas Isles of Dread"; break;	
				case 133: sWords = "alguns necromantes praticando magia negra nas profundezas do castelo"; break;	
				case 134: sWords = "uma torre de latão aparecendo em Umber Veil"; break;	
				case 135: sWords = "uma tribo orc que descobriu as minas de prata perdidas"; break;	
				case 136: sWords = "um castelo abandonado de Stonegate, porque todos dentro foram mortos"; break;	
				case 137: sWords = "alguns Shadowlords que tomaram o castelo de Stonegate"; break;	
				case 138: sWords = "um warlord ciclope procurando prata para forjar armas para seu exército"; break;	
				case 139: sWords = "um cavaleiro do mal que tem o crânio de Mondain"; break;	
				case 140: sWords = "um mago vil que tem a gema da imortalidade"; break;	
				case 141: sWords = "um livro antigo de magia enterrado em " + dungeon; break;	
				case 142: sWords = "um mago que navega pelas Isles of Dread, vendendo feitiços raros"; break;	
				case 143: sWords = "um ferreiro em " + city + " que faz armas de mithril"; break;	
				case 144: sWords = "Zorn vivendo em " + dungeon; break;	
				case 145: sWords = "uma espada negra repousando em " + dungeon; break;	
				case 146: sWords = "algum " + adventurer + " que foi morto pelo olho de um ciclope"; break;	
				case 147: sWords = "algum " + adventurer + " que mandou um tinker em " + city + " fazer um golem com um núcleo sombrio"; break;	
				case 148: sWords = "titans que lançam raios do céu"; break;	
				case 149: sWords = "algum " + adventurer + " que foi morto por grues elementais"; break;	
				case 150: sWords = "um ancient wyrm guardando o caminho para o Vale Escondido"; break;	
				case 151: sWords = "um mago louco atuando como um sumo sacerdote de Kazibal"; break;	
				case 152: sWords = "uma mansão insular onde dizem que Azerok ainda vive"; break;	
				case 153: sWords = "uma caverna escondida abaixo do Farol Esquecido"; break;	
				case 154: sWords = GetRareLocation( from, false, true ); if ( from is HouseVisitor ){ sWords = "um comerciante de artefatos em " + city + ""; } break;	
				case 155: sWords = "um rato tagarela no castelo que gosta de queijo"; break;	
				case 156: sWords = "uma moonstone que pode invocar um moongate de quase qualquer lugar"; break;	
				case 157: sWords = "um grupo de mineiros dizendo que Morinia é uma das melhores minas para minério"; break;	
				case 158: sWords = "alguns cristais estando nas minas de Morinia"; break;	
				case 159: sWords = "um mineiro lendário que desenterrou minério anão"; break;	
				case 160: sWords = "um lenhador lendário que cortou madeira élfica"; break;	
				case 161: sWords = "algum " + RandomThings.GetRandomJob() + " resolvendo o mistério do Portal da Caveira"; break;	
				case 162: sWords = "algum " + RandomThings.GetRandomJob() + " resolvendo o mistério dos Pilares da Serpente"; break;
				case 163: 
					misc = "tumba";	
					switch( Utility.RandomMinMax( 0, 4 ) )
					{
						case 1: misc = "cripta"; break;	
						case 2: misc = "tesouro"; break;	
						case 3: misc = "artefato"; break;	
						case 4: misc = "restos"; break;	
					}
					sWords = "um " + misc + " de " + RandomThings.GetRandomName() + " em " + dungeon + ""; break;	
				case 164:
					misc = "mapa";	
					switch( Utility.RandomMinMax( 0, 4 ) )
					{
						case 1: misc = "tabuleta"; break;	
						case 2: misc = "pergaminho"; break;	
						case 3: misc = "livro"; break;	
						case 4: misc = "pista"; break;	
					}
					sWords = "um " + misc + " que leva a " + dungeon + ""; break;	
				case 165:
					misc = "mapa";	
					switch( Utility.RandomMinMax( 0, 4 ) )
					{
						case 1: misc = "tabuleta"; break;	
						case 2: misc = "pergaminho"; break;	
						case 3: misc = "livro"; break;	
						case 4: misc = "pista"; break;	
					}
					string misc2 = "ouro";	
					switch( Utility.RandomMinMax( 0, 5 ) )
					{
						case 1: misc2 = "tesouro"; break;
						case 2: misc2 = "gemas"; break;
						case 3: misc2 = "joias"; break;
						case 4: misc2 = "riquezas"; break;
						case 5: misc2 = "cristais"; break;
					}
					sWords = "um " + misc + " que leva ao " + misc2 + " de " + RandomThings.GetRandomName() + ""; break;	
				case 166: 
					misc = " artefato";	
					switch( Utility.RandomMinMax( 0, 4 ) )
					{
						case 1: misc = "Artefato"; break;	
						case 2: misc = "item mágico"; break;	
						case 3: misc = " artefato antigo"; break;	
						case 4: misc = " relíquia antiga"; break;	
					}
					sWords = "um" + misc + " chamado " + relics + " perdido em " + dungeon + ""; break;	
				case 167: 
					misc = "destruída";	
					switch( Utility.RandomMinMax( 0, 3 ) )
					{
						case 1: misc = "arruinada"; break;	
						case 2: misc = "devastada"; break;	
						case 3: misc = "perdida"; break;	
					}
					sWords = "lendas de " + RandomThings.MadeUpCity() + " sendo " + misc + " durante " + RandomThings.GetRandomDisaster() + ""; break;	
				case 168: 
					misc = "se juntou a";	
					switch( Utility.RandomMinMax( 0, 4 ) )
					{
						case 1: misc = "deixou"; break;	
						case 2: misc = "traiu"; break;	
						case 3: misc = "destruiu"; break;	
						case 4: misc = "iniciou"; break;	
					}
					sWords = "um " + RandomThings.GetBoyGirlJob( Utility.RandomMinMax( 0, 1 ) ) + " que " + misc + " " + RandomThings.GetRandomSociety() + ""; break;
				case 169: 
					misc = "roubado";	
					switch( Utility.RandomMinMax( 0, 5 ) )
					{
						case 1: misc = "morto"; break;	
						case 2: misc = "perdido"; break;	
						case 3: misc = "abatido"; break;	
						case 4: misc = "preso"; break;	
						case 5: misc = "sequestrado"; break;	
					}
					sWords = "um " + RandomThings.GetBoyGirlJob( Utility.RandomMinMax( 0, 1 ) ) + " que foi " + misc + " a caminho de " + RandomThings.MadeUpCity() + ""; break;
				case 170: 
					misc = "hydra";	
					switch( Utility.RandomMinMax( 1, 6 ) )
					{
						case 1: misc = "dragão"; break;	
						case 2: misc = "drake"; break;	
						case 3: misc = "wyrm"; break;	
					}
					sWords = "um dente de " + misc + " sendo jogado no chão para invocar um esqueleto"; break;
				case 171: 
					misc = "pescador";	
					switch( Utility.RandomMinMax( 1, 6 ) )
					{
						case 1: misc = "construtor de navios"; break;	
						case 2: misc = "pirata"; break;	
						case 3: misc = "marinheiro"; break;	
					}
					sWords = "um " + RandomThings.GetBoyGirlJob( Utility.RandomMinMax( 0, 1 ) ) + " vendendo um dente de megaldon para um " + misc + " em " + RandomThings.MadeUpCity() + " por " + (Utility.RandomMinMax( 5, 20 )*100) + " de ouro"; break;
				case 172: 
					misc = "morreu";	
					switch( Utility.RandomMinMax( 0, 4 ) )
					{
						case 1: misc = "desapareceu"; break;	
						case 2: misc = "pereceu"; break;	
						case 3: misc = "foi morto"; break;	
						case 4: misc = "foi perdido"; break;	
					}
					sWords = "um " + RandomThings.GetBoyGirlJob( Utility.RandomMinMax( 0, 1 ) ) + " que " + misc + " em " + dungeon + ""; break;
				case 173: sWords = "Alguns micônidas têm vagado para longe do subterrâneo ultimamente"; break;
				case 174: sWords = "Estranhos são os sonhos que tenho tido ultimamente, o sacerdote disse que eu deveria considerar meus pecados, e ainda assim me pergunto se não há mais algo em jogo aqui"; break;
				case 175: sWords = "Disseram que um Urso-Troll chamado Dente-Negro tem atacado caravanas que passam pelas florestas perto de Montor"; break;
				case 176: 
					misc = "mãe";	
						switch( Utility.RandomMinMax( 0, 4 ) )
						{
							case 1: misc = "papagaio"; break;	
							case 2: misc = "pai"; break;	
							case 3: misc = "filha"; break;	
							case 4: misc = "esposa"; break;	
						}
					sWords = "A Madre Superiora no convento da Santa Misericórdia perto de Grey está cuidando do(a) meu(minha) " + misc + "."; break;
				case 177: sWords = "O Açougueiro é real. Eu o vi, no fundo do castelo em Ravendark."; break;
				case 178: sWords = "Os orcs têm um novo chefe de guerra. Presa de Fogo tem incendiado fazendas perto de Moon."; break;
				case 179: sWords = "Cogumelos estranhos que parecem andar foram avistados na costa leste de Sosaria. O que será que está causando isso."; break;
				case 180: sWords = "O Arquidruida Fiorin abandonou a civilização há muito tempo e agora guarda o Bosque Uivante na costa oeste de Sosaria."; break;
				case 181: sWords = "Eu vi o pacto negro à meia-noite, dançando em torno de um bode vil nas ilhas do pavor!"; break;
				case 182: sWords = "O Rei Esqueleto despertou, eu digo! Sua tumba na Pirâmide Antiga ressoa com seus ossos dourados!"; break;
				case 183: sWords = "Estranhos têm sido meus sonhos ultimamente. Um sábio que consultei em Lodoria me disse que eu estava sendo influenciado por alguma força vil."; break;
				case 184: sWords = "Os Tiranos dos Olhos podem disparar raios terríveis de cada um de seus olhos."; break;
				case 185: sWords = "Eu vi em meu sono. Amarelo e monstruoso, todo olhos e dentes. Disse que seu nome é O Tecelão de Sonhos, e que habita em Lodoria."; break;
				case 186: sWords = "Um caçador me disse que o Velho Caolho arrasou o acampamento deles nos desertos do Império Selvagem e matou muitos antes de seguir em frente."; break;
				case 187: sWords = "Ouvi canções arrepiantes vindas de Ravendark. Alguns dizem que o Príncipe das Trevas está de volta."; break;
				case 188: sWords = "Eu vi o anjo que guarda a fortaleza dos Cavaleiros do Céu. Nunca mais verei algo tão belo."; break;
				case 189: sWords = "Cultistas estranhos foram vistos em Lodoria, falando sobre um grande dragão chamado Ashardalom."; break;
				case 190: sWords = "Dizem que os fogos de Destard estão queimando mais forte do que nunca."; break;
				case 191: sWords = "Eu vi os druidas da Ordem Uivante se transformarem em lobos!"; break;
				case 192: sWords = "Disseram que a Filha do Fogo atrai aventureiros para as entranhas dos Fogos do Inferno, e eles nunca mais são vistos."; break;
				case 193: sWords = "Hrimah reivindicou a coroa congelada e se proclamou o Punho do Norte na Cicatriz Glacial."; break;				
				case 194: sWords = "O elfo disse que a temida casa Fanae havia ascendido no favor de sua deusa maligna."; break;
				case 195: sWords = "Aquele elfo estranho falou de alguma deusa maligna exigindo sacrifício nos cantos escuros do mundo."; break;
				case 196: sWords = "Após milênios, a legião abrasadora foi vista novamente em Sosaria, nas profundezas dos fogos do inferno"; break;
				case 197: sWords = "Uma gangue de bandidos liderada por um Ettin chamado Dardin tem aterrorizado a costa norte"; break;
				case 198: sWords = "Um grupo de cavaleiros caídos construiu uma fortaleza vermelha como sangue na região central de Sosaria, eles são liderados por um homem vil chamado Caelan."; break;
			}
			return sWords;	
		}

		/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		public static string Adventurer()
		{
			string sAdventurer = "bandido";	
			switch( Utility.RandomMinMax( 0, 56 ) )
			{
				case 0: sAdventurer = "aventureiro"; break;	
				case 1: sAdventurer = "bandido"; break;	
				case 2: sAdventurer = "bárbaro"; break;	
				case 3: sAdventurer = "bardo"; break;	
				case 4: sAdventurer = "barão"; break;	
				case 5: sAdventurer = "baronesa"; break;	
				case 6: sAdventurer = "cavaleiro"; break;	
				case 7: sAdventurer = "clérigo"; break;	
				case 8: sAdventurer = "conjurador"; break;	
				case 9: sAdventurer = "defensor"; break;	
				case 10: sAdventurer = "adivinho"; break;	
				case 11: sAdventurer = "encantador"; break;	
				case 12: sAdventurer = "encantadora"; break;	
				case 13: sAdventurer = "explorador"; break;	
				case 14: sAdventurer = "guerreiro"; break;	
				case 15: sAdventurer = "gladiador"; break;	
				case 16: sAdventurer = "herege"; break;	
				case 17: sAdventurer = "caçador"; break;	
				case 18: sAdventurer = "ilusionista"; break;	
				case 19: sAdventurer = "invocador"; break;	
				case 20: sAdventurer = "rei"; break;	
				case 21: sAdventurer = "cavaleiro"; break;	
				case 22: sAdventurer = "dama"; break;	
				case 23: sAdventurer = "lord"; break;	
				case 24: sAdventurer = "mago"; break;	
				case 25: sAdventurer = "magician"; break;	
				case 26: sAdventurer = "mercenário"; break;	
				case 27: sAdventurer = "menestrel"; break;	
				case 28: sAdventurer = "monge"; break;	
				case 29: sAdventurer = "místico"; break;	
				case 30: sAdventurer = "necromante"; break;	
				case 31: sAdventurer = "fora da lei"; break;	
				case 32: sAdventurer = "paladino"; break;	
				case 33: sAdventurer = "sacerdote"; break;	
				case 34: sAdventurer = "sacerdotisa"; break;	
				case 35: sAdventurer = "príncipe"; break;	
				case 36: sAdventurer = "princesa"; break;	
				case 37: sAdventurer = "profeta"; break;	
				case 38: sAdventurer = "rainha"; break;	
				case 39: sAdventurer = "ranger"; break;	
				case 40: sAdventurer = "ladino"; break;	
				case 41: sAdventurer = "sábio"; break;	
				case 42: sAdventurer = "batedor"; break;	
				case 43: sAdventurer = "buscador"; break;	
				case 44: sAdventurer = "vidente"; break;	
				case 45: sAdventurer = "xamã"; break;	
				case 46: sAdventurer = "exterminador"; break;	
				case 47: sAdventurer = "feiticeiro"; break;	
				case 48: sAdventurer = "feiticeira"; break;	
				case 49: sAdventurer = "invocador"; break;	
				case 50: sAdventurer = "templário"; break;	
				case 51: sAdventurer = "ladrão"; break;	
				case 52: sAdventurer = "viajante"; break;	
				case 53: sAdventurer = "bruxo"; break;	
				case 54: sAdventurer = "guerreiro"; break;	
				case 55: sAdventurer = "bruxa"; break;	
				case 56: sAdventurer = "mago"; break;	
			}
			return sAdventurer;	
		}

		/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		public static void GetChatter( Mobile patron )
		{
			string relics = QuestCharacters.QuestItems( false );
				if ( Utility.RandomBool() ){ relics = QuestCharacters.ArtyItems( false ); }

			string cVal = "";	
			string cDun = "";	
			string act = "";
			string misc = "";
			string misc2 = "";

			string sSourceName = RandomThings.GetRandomBoyName();
			string sSourceJob = RandomThings.GetBoyGirlJob(0);
			if ( Utility.RandomBool() )
			{
				sSourceName = RandomThings.GetRandomGirlName();
				sSourceJob = RandomThings.GetBoyGirlJob(1);
			}

			string sSource = "Eu ouvi sobre";	
			switch( Utility.RandomMinMax( 1, 13 ) )
			{
				case 1: sSource = "Há rumores sobre"; break;
				case 2: sSource = "Estão falando sobre"; break;
				case 3: sSource = "Existem rumores sobre"; break;
				case 4: sSource = "Me contaram sobre"; break;
				case 5: sSource = "Ouvi alguém falando sobre"; break;
				case 6: sSource = "Há uma história sobre"; break;
				case 7: sSource = sSourceName + " me contou sobre"; break;
				case 8: sSource = sSourceName + " o " + sSourceJob + " me contou sobre"; break;
				case 9: sSource = "Algum " + sSourceJob + " me contou sobre"; break;
				case 10: sSource = sSourceName + " o " + sSourceJob + " ouviu sobre"; break;
				case 11: sSource = "Algum " + sSourceJob + " ouviu sobre"; break;
				case 12: sSource = sSourceName + " o " + sSourceJob + " descobriu sobre"; break;
				case 13: sSource = "Algum " + sSourceJob + " descobriu sobre"; break;
			}

			string sThey = "Samson";	
			if ( Utility.RandomMinMax( 1, 2 ) == 1 ){ sThey = NameList.RandomName( "female" ); } else { sThey = NameList.RandomName( "male" ); }

			string city = RandomThings.GetRandomCity();	
				if ( Utility.RandomMinMax( 1, 3 ) == 1 ){ city = RandomThings.MadeUpCity(); }

			string dungeon = QuestCharacters.SomePlace( "tavern" );	
				if ( Utility.RandomMinMax( 1, 3 ) == 1 ){ dungeon = RandomThings.MadeUpDungeon(); }

			string sAdventurer = Adventurer();	

			string sMoney = "ouro";	
			switch( Utility.RandomMinMax( 0, 6 ) )
			{
				case 0: sMoney = "prata"; break;	
				case 1: sMoney = "cobre"; break;	
				case 2: sMoney = "joias"; break;	
				case 3: sMoney = "cristais"; break;	
			}

			string sDebt = "daquele jogo de cartas";	
			switch( Utility.RandomMinMax( 0, 19 ) )
			{
				case 0: sDebt = "daquela aposta"; break;	
				case 1: sDebt = "por aquele artefato"; break;	
				case 2: sDebt = "daquele jogo de cartas"; break;	
				case 3: sDebt = "daquele jogo de dardos"; break;	
				case 4: sDebt = "por aquele cavalo"; break;	
				case 5: sDebt = "por aquela poção"; break;	
				case 6: sDebt = "por aquela arma"; break;	
				case 7: sDebt = "por aquela armadura"; break;	
				case 8: sDebt = "por libertá-lo"; break;	
				case 9: sDebt = "por encontrar aquele item"; break;	
				case 10: sDebt = "por resolver aquele enigma"; break;	
				case 11: sDebt = "por desenterrar aquele tesouro"; break;	
				case 12: sDebt = "por aquela gema"; break;	
				case 13: sDebt = "por aquela varinha"; break;	
				case 14: sDebt = "por aquele cajado"; break;	
				case 15: sDebt = "por consertar aquela coisa"; break;	
				case 16: sDebt = "por matar aquele monstro"; break;	
				case 17: sDebt = "por roubar aquela coisa"; break;	
				case 18: sDebt = "por escondê-los na minha casa"; break;	
				case 19: sDebt = "por aquele mapa"; break;	
			}

			int relic = Utility.RandomMinMax( 1, 59 );	

			int CommonTalkingCount = 58;
			string sSpeech = "Nós devemos esperar por " + sThey + ".";
			switch( Utility.RandomMinMax( 1, CommonTalkingCount ) )
			{
				case 1: sSpeech = "Nós devemos esperar por " + sThey + "."; break;	
				case 2: sSpeech = sThey + " mora em algum lugar perto de " + city + "."; break;	
				case 3: sSpeech = "Nós vamos encontrar " + sThey + " amanhã."; break;	
				case 4: sSpeech = "Precisamos encontrar um banco e dividir este saque que temos."; break;	
				case 5: sSpeech = sThey + " ainda me deve " + Utility.RandomMinMax( 5, 200 ) + " de " + sMoney + " " + sDebt + "."; break;	
				case 6:
					cVal = "dormindo";	
					switch( Utility.RandomMinMax( 0, 8 ) )
					{
						case 1: cVal = "bebendo"; break;	
						case 2: cVal = "comendo"; break;	
						case 3: cVal = "distraídos"; break;	
						case 4: cVal = "procurando"; break;	
						case 5: cVal = "perdidos"; break;	
						case 6: cVal = "ausentes"; break;	
						case 7: cVal = "explorando"; break;	
						case 8: cVal = "bêbados"; break;	
					}
					sSpeech = "Acho que " + sThey + " roubou enquanto estávamos " + cVal + "."; break;	
				case 7: sSpeech = sThey + " trará isso aqui quando encontrar."; break;	
				case 8:
					cVal = "Você sabe";	
					switch( Utility.RandomMinMax( 0, 9 ) )
					{
						case 1: cVal = "Onde você conheceu"; break;	
						case 2: cVal = "Onde você viu"; break;	
						case 3: cVal = "Quando você conheceu"; break;	
						case 4: cVal = "Quando você viu"; break;	
						case 5: cVal = "Quando você teve notícias de"; break;	
						case 6: cVal = "Quando você matou"; break;	
						case 7: cVal = "Onde você matou"; break;	
						case 8: cVal = "Quando eu vou conhecer"; break;	
						case 9: cVal = "Quando nós vamos conhecer"; break;	
					}
					sSpeech = cVal + " " + sThey + "?"; break;	
				case 9: sSpeech = sThey + " vendeu " + relics + " por " + Utility.RandomMinMax( 5, 200 ) + " de " + sMoney + "."; break;	
				case 10: sSpeech = "Eu paguei a " + sThey + " " + Utility.RandomMinMax( 5, 200 ) + " de " + sMoney + " por " + relics + "."; break;	
				case 11:
					cVal = "destruiu";	
					switch( Utility.RandomMinMax( 0, 6 ) )
					{
						case 1: cVal = "vendeu"; break;	
						case 2: cVal = "perdeu"; break;	
						case 3: cVal = "encontrou"; break;	
						case 4: cVal = "descobriu"; break;	
						case 5: cVal = "trocou"; break;	
						case 6: cVal = "roubou"; break;	
					}
					sSpeech = sThey + " " + cVal + " " + relics + "."; break;	
				case 12:
					cVal = "roubou";	
					switch( Utility.RandomMinMax( 0, 6 ) )
					{
						case 1: cVal = "assassinou"; break;	
						case 2: cVal = "traiu"; break;	
						case 3: cVal = "capturou"; break;	
						case 4: cVal = "enganou"; break;	
						case 5: cVal = "matou"; break;	
						case 6: cVal = "extorquiu"; break;	
					}
					sSpeech = sThey + " " + cVal + " eles, eu sei disso."; break;	
				case 13:
					cVal = "comprou isso de";	
					switch( Utility.RandomMinMax( 0, 8 ) )
					{
						case 1: cVal = "roubou isso de"; break;	
						case 2: cVal = "vendeu isso para"; break;	
						case 3: cVal = "se encontrou com"; break;	
						case 4: cVal = "sequestrou"; break;	
						case 5: cVal = "assaltou"; break;	
						case 6: cVal = "trabalha para"; break;	
						case 7: cVal = "mora com"; break;	
						case 8: cVal = "deve " + Utility.RandomMinMax( 5, 200 ) + " de ouro para"; break;	
					}
					sSpeech = sThey + " " + cVal + " um " + RandomThings.GetRandomJob() + " em " + city + "."; break;	
				case 14:
					act = "assaltou";	
					switch( Utility.RandomMinMax( 0, 9 ) )
					{
						case 1: act = "assassinou"; break;	
						case 2: act = "traiu"; break;	
						case 3: act = "capturou"; break;	
						case 4: act = "conheceu"; break;	
						case 5: act = "matou"; break;	
						case 6: act = "deixou"; break;	
						case 7: act = "seguiu"; break;	
						case 8: act = "serviu"; break;	
						case 9: act = "prendeu"; break;	
					}
					cVal = NameList.RandomName( "female" );	
					if ( Utility.RandomBool() ){ cVal = NameList.RandomName( "male" ); }
					string scene = city;	
					if ( Utility.RandomBool() ){ scene = dungeon; }
					sSpeech = sThey + " " + act + " " + cVal + " em " + scene + "."; break;	
				case 15:
					cVal = "executado";	
					switch( Utility.RandomMinMax( 0, 8 ) )
					{
						case 1: cVal = "preso"; break;	
						case 2: cVal = "preso"; break;	
						case 3: cVal = "capturado"; break;	
						case 4: cVal = "banido"; break;	
						case 5: cVal = "recompensado"; break;	
						case 6: cVal = "celebrado"; break;	
						case 7: cVal = "promovido"; break;	
						case 8: cVal = "libertado"; break;	
					}
					sSpeech = sThey + " foi " + cVal + " por matar aquele " + RandomThings.GetRandomJob() + " em " + city + "."; break;	
				case 16: sSpeech = "Ouvi dizer que " + sThey + " se tornou um " + RandomThings.GetRandomJob() + " em " + city + "."; break;	
				case 17: sSpeech = "Preciso ver o " + RandomThings.GetRandomJob() + " antes de continuarmos viajando."; break;	
				case 18: sSpeech = sThey + " se aposentou e se tornou um " + RandomThings.GetRandomJob() + " em " + city + "."; break;	
				case 19: sSpeech = sThey + " foi preso por roubar do " + RandomThings.GetRandomJob() + " em " + city + "."; break;	
				case 20:
					string item20 = Server.Items.SomeRandomNote.GetSpecialItem( relic, 1 );		if ( patron is HouseVisitor ){ item20 = relics; }
					string place20 = Server.Items.SomeRandomNote.GetSpecialItem( relic, 0 );
						if ( Utility.RandomBool() ) // CITIZENS LIE HALF THE TIME
						{
							if ( Utility.RandomBool() ){ place20 = RandomThings.MadeUpDungeon(); }
							else { place20 = QuestCharacters.SomePlace( null ); }
						}
						if ( patron is HouseVisitor ){ place20 = dungeon; }
					sSpeech = "Finalmente descobri como podemos obter o " + item20 + ". Precisamos reunir os outros e nos encontrar em " + place20 + "."; break;	
				case 21:
					string item21 = Server.Items.SomeRandomNote.GetSpecialItem( relic, 1 );		if ( patron is HouseVisitor ){ item21 = relics; }
					string place21 = Server.Items.SomeRandomNote.GetSpecialItem( relic, 0 );
						if ( Utility.RandomBool() ) // CITIZENS LIE HALF THE TIME
						{
							if ( Utility.RandomBool() ){ place21 = RandomThings.MadeUpDungeon(); }
							else { place21 = QuestCharacters.SomePlace( null ); }
						}
						if ( patron is HouseVisitor ){ place21 = dungeon; }
					sSpeech = "Precisamos ir para " + place21 + " se quisermos obter o " + item21 + " para " + QuestCharacters.RandomWords() + "."; break;	
				case 22:
					string item22 = Server.Items.SomeRandomNote.GetSpecialItem( relic, 1 );		if ( patron is HouseVisitor ){ item22 = relics; }
					string place22 = Server.Items.SomeRandomNote.GetSpecialItem( relic, 0 );
						if ( Utility.RandomBool() ) // CITIZENS LIE HALF THE TIME
						{
							if ( Utility.RandomBool() ){ place22 = RandomThings.MadeUpDungeon(); }
							else { place22 = QuestCharacters.SomePlace( null ); }
						}
						if ( patron is HouseVisitor ){ place22 = dungeon; }
					sSpeech = "O " + RandomThings.GetRandomJob() + " em " + city + " me disse que provavelmente podemos obter o " + item22 + " se procurarmos em " + place22 + "."; break;
				case 23: sSpeech = GetRareLocation( patron, false, false ); if ( patron is HouseVisitor ){ sSpeech = "Precisamos ir para " + city + " se quisermos encontrar o " + relics + "."; } break;	
				case 24: sSpeech = sThey + " tem vendido partes do corpo para a guilda de magia negra."; break;	
				case 25: sSpeech = sThey + " vendeu aquele crânio de monstro para os necromantes por " + Utility.RandomMinMax( 50, 200 ) + " de ouro."; break;	
				case 26: sSpeech = "Vamos procurar pelo " + Server.Misc.RandomThings.GetRandomColorName( 0 ) + " " + RandomThings.GetRandomThing( 0 ) + " amanhã."; break;	
				case 27: sSpeech = "O " + RandomThings.GetRandomJob() + " em " + RandomThings.MadeUpCity() + " está procurando ajuda com " + RandomThings.GetRandomMonsters() + "."; break;	
				case 28: sSpeech = RandomThings.GetRandomShipName( "", 0 ) + " afundou na costa do " + RandomThings.GetRandomKingdom() + " " + RandomThings.GetRandomKingdomName() + "."; break;	
				case 29:
					cVal = RandomThings.MadeUpDungeon();	
					switch( Utility.RandomMinMax( 0, 1 ) )
					{
						case 1: cVal = RandomThings.MadeUpCity(); break;	
					}
					sSpeech = "Encontrei um mapa que leva a " + cVal + "."; break;	
				case 30:
					cVal = "atacar";	
					switch( Utility.RandomMinMax( 0, 5 ) )
					{
						case 1: cVal = "destruir"; break;	
						case 2: cVal = "invadir"; break;	
						case 3: cVal = "guerrear com"; break;	
						case 4: cVal = "ser derrotado por"; break;	
						case 5: cVal = "ser atacado por"; break;	
					}
					sSpeech = "Os " + RandomThings.GetRandomTroops() + " vão " + cVal + " o " + RandomThings.GetRandomKingdomName() + " " + RandomThings.GetRandomKingdom() + "."; break;	
				case 31:
					cVal = "torre";	
					switch( Utility.RandomMinMax( 0, 5 ) )
					{
						case 1: cVal = "castelo"; break;	
						case 2: cVal = "mansão"; break;	
						case 3: cVal = "fortaleza"; break;	
						case 4: cVal = "casa"; break;	
						case 5: cVal = "cabana"; break;	
					}
					sSpeech = "Deveríamos construir aquela " + cVal + " no " + RandomThings.GetRandomKingdomName() + " " + RandomThings.GetRandomKingdom() + "."; break;	
				case 32:
					cVal = RandomThings.MadeUpDungeon();	
					switch( Utility.RandomMinMax( 0, 1 ) )
					{
						case 1: cVal = RandomThings.MadeUpCity(); break;	
					}
					sSpeech = "Precisamos chegar a " + cVal + " antes de " + sThey + "."; break;	
				case 33: sSpeech = "O " + RandomThings.GetRandomJob() + " em " + RandomThings.MadeUpCity() + " tem " + relics + " à venda."; break;	
				case 34: sSpeech = "O " + RandomThings.GetRandomNoble() + " está oferecendo ouro para livrar o " + RandomThings.GetRandomKingdomName() + " " + RandomThings.GetRandomKingdom() + " de " + RandomThings.GetRandomAttackers() + "."; break;	
				case 35:
					cVal = RandomThings.MadeUpDungeon();	
					switch( Utility.RandomMinMax( 0, 1 ) )
					{
						case 1: cVal = QuestCharacters.SomePlace( "" ); break;	
					}
					sSpeech = "Acho que conseguimos o maior tesouro de " + cVal + "."; break;	
				case 36:
					cVal = "assaltou";	
					switch( Utility.RandomMinMax( 0, 9 ) )
					{
						case 1: cVal = "assassinou"; break;	
						case 2: cVal = "conheceu"; break;	
						case 3: cVal = "espionou"; break;	
						case 4: cVal = "traiu"; break;	
						case 5: cVal = "prestou juramento a"; break;	
						case 6: cVal = "serve"; break;	
						case 7: cVal = "foi preso por"; break;	
						case 8: cVal = "foi morto por"; break;	
						case 9: cVal = "matou"; break;	
					}
					sSpeech = sThey + " " + cVal + " o " + RandomThings.GetRandomNoble() + " em " + RandomThings.MadeUpCity() + "."; break;	
				case 37: sSpeech = "Algum " + RandomThings.GetRandomNoble() + " nos pagará " + RandomThings.GetRandomCoinReward() + " de ouro se encontrarmos " + relics + " para ele."; break;
				case 38: sSpeech = "Há uma recompensa de " + RandomThings.GetRandomCoinReward() + " de ouro por " + sThey + " o " + RandomThings.GetBoyGirlJob( Utility.RandomMinMax( 0, 1 ) ) + "."; break;	
				case 39: sSpeech = "O " + RandomThings.GetBoyGirlJob( Utility.RandomMinMax( 0, 1 ) ) + " disse que para um grande tesouro precisamos ir para " + RandomThings.MadeUpDungeon() + "."; break;	
				case 40:
					cVal = "escondeu";	
					switch( Utility.RandomMinMax( 0, 6 ) )
					{
						case 1: cVal = "perdeu"; break;	
						case 2: cVal = "deixou"; break;	
						case 3: cVal = "escondeu"; break;	
						case 4: cVal = "encontrou"; break;	
						case 5: cVal = "descobriu"; break;	
						case 6: cVal = "criou"; break;	
					}
					sSpeech = sThey + " " + cVal + " " + relics + " nas profundezas de " + RandomThings.MadeUpDungeon() + "."; break;	
				case 41:
					cVal = RandomThings.MadeUpDungeon();	
					string portal = "espelho";	
					if ( Utility.RandomBool() ){ cVal = QuestCharacters.SomePlace( "" ); }
					if ( Utility.RandomBool() ){ portal = "portal"; }
					sSpeech = sThey + " encontrou um " + portal + " mágico que levou a " + cVal + "."; break;	
				case 42:
					cVal = "todas as suas moedas viraram chumbo";	
					switch( Utility.RandomMinMax( 0, 13 ) )
					{
						case 1: cVal = "todas as suas míseras moedas viraram ouro"; break;	
						case 2: cVal = "eles ficaram muito mais fortes"; break;	
						case 3: cVal = "eles ficaram muito mais ágeis"; break;	
						case 4: cVal = "eles ficaram mais inteligentes"; break;	
						case 5: cVal = "eles ficaram muito mais fracos"; break;	
						case 6: cVal = "eles ficaram muito menos ágeis"; break;	
						case 7: cVal = "eles perderam a mente"; break;	
						case 8: cVal = "elementais da água jorraram"; break;	
						case 9: cVal = "eles viram uma grande caixa de tesouro dentro dele"; break;	
						case 10: cVal = "eles morreram envenenados"; break;	
						case 11: cVal = "eles foram magicamente curados"; break;	
						case 12: cVal = "eles foram curados do veneno"; break;	
						case 13: cVal = "seu " + Server.Items.SomeRandomNote.GetSpecialItem( relic, 1 ) + " desapareceu"; break;	
					}
					cDun = RandomThings.MadeUpDungeon();	
					if ( Utility.RandomBool() ){ cDun = QuestCharacters.SomePlace( "" ); }
					sSpeech = sThey + " bebeu de um poço estranho em " + cDun + " e " + cVal + "."; break;	
				case 43:
					cVal = "uma armadilha de buraco";	
					switch( Utility.RandomMinMax( 0, 8 ) )
					{
						case 1: cVal = "uma armadilha de espinhos"; break;	
						case 2: cVal = "uma armadilha de fogo"; break;	
						case 3: cVal = "uma armadilha explosiva"; break;	
						case 4: cVal = "uma armadilha de gás venenoso"; break;	
						case 5: cVal = "um cogumelo explosivo"; break;	
						case 6: cVal = "uma armadilha de lâmina de serra"; break;	
						case 7: cVal = "uma armadilha de rosto de pedra flamejante"; break;	
						case 8: cVal = "uma armadilha mágica"; break;	
					}
					cDun = RandomThings.MadeUpDungeon();	
					if ( Utility.RandomBool() ){ cDun = QuestCharacters.SomePlace( "" ); }
					sSpeech = sThey + " morreu em " + cDun + " por causa de " + cVal + "."; break;	
				case 44:
					cVal = "caiu para";	
					switch( Utility.RandomMinMax( 0, 8 ) )
					{
						case 1: cVal = "foi atacado por"; break;	
						case 2: cVal = "foi invadido por"; break;	
						case 3: cVal = "foi destruído por"; break;	
						case 4: cVal = "foi derrotado por"; break;	
						case 5: cVal = "se rendeu a"; break;	
						case 6: cVal = "venceu contra"; break;	
						case 7: cVal = "derrotou"; break;	
						case 8: cVal = "matou o exército de"; break;	
					}
					sSpeech = "O " + RandomThings.GetRandomKingdomName() + " " + RandomThings.GetRandomKingdom() + " " + cVal + " os " + RandomThings.GetRandomTroops() + "."; break;	
				case 45:
					cVal = "morto";	
					switch( Utility.RandomMinMax( 0, 5 ) )
					{
						case 1: cVal = "abatido"; break;	
						case 2: cVal = "derrotado"; break;	
						case 3: cVal = "quase morto"; break;	
						case 4: cVal = "quase abatido"; break;	
						case 5: cVal = "quase derrotado"; break;	
					}
					sSpeech = sThey + " foi " + cVal + " por " + RandomThings.GetRandomMonsters() + " em " + RandomThings.MadeUpDungeon() + "."; break;
				case 46:
					string dIrc = "Deixe-me contar a você";
						if ( Utility.RandomBool() ){ dIrc = "Conte-me"; }

					cVal = "conto";	
					switch( Utility.RandomMinMax( 0, 4 ) )
					{
						case 1: cVal = "história"; break;	
						case 2: cVal = "fábula"; break;	
						case 3: cVal = "lenda"; break;	
						case 4: cVal = "mito"; break;	
					}

					sSpeech = dIrc + " o " + cVal + " de " + relics + ".";

					switch( Utility.RandomMinMax( 0, 5 ) )
					{
						case 1: sSpeech = dIrc + " o " + cVal + " do " + RandomThings.GetRandomKingdomName() + " " + RandomThings.GetRandomKingdom() + "."; break;	
						case 2: sSpeech = dIrc + " o " + cVal + " de " + RandomThings.MadeUpDungeon() + "."; break;	
						case 3: sSpeech = dIrc + " o " + cVal + " do " + RandomThings.GetRandomJobTitle(0) + " e do " + RandomThings.GetRandomThing(0) + "."; break;	
						case 4: sSpeech = dIrc + " o " + cVal + " do " + RandomThings.GetRandomColorName(0) + " " + RandomThings.GetRandomThing(0) + "."; break;	
						case 5: sSpeech = dIrc + " o " + cVal + " do " + RandomThings.GetRandomJobTitle(0) + " e da " + RandomThings.GetRandomCreature() + "."; break;	
					}
					break;
				case 47:
					cVal = "procurando por";	
					switch( Utility.RandomMinMax( 0, 3 ) )
					{
						case 1: cVal = "procurando por"; break;	
						case 2: cVal = "tentando encontrar"; break;	
						case 3: cVal = "tentando localizar"; break;	
					}

					string goal = "o Codex da Sabedoria Suprema";	
					switch( Utility.RandomMinMax( 0, 25 ) )
					{
						case 1: goal = "o Núcleo Sombrio de Exodus";	 	break;	
						case 2: goal = QuestCharacters.QuestItems( false );	break;	
						case 3: goal = "o Cajado de Cinco Partes";	break;	
						case 4: goal = "Mangar, o Sombrio";	break;	
						case 5: goal = "as Runas da Virtude";	break;	
						case 6: goal = "o Livro da Verdade";	break;	
						case 7: goal = "o Sino da Coragem";	break;	
						case 8: goal = "a Vela do Amor";	break;	
						case 9: goal = "a Balança da Ethicalidade";	break;	
						case 10: goal = "o Orbe da Lógica";	break;	
						case 11: goal = "a Lanterna da Disciplina";	break;	
						case 12: goal = "o Sopro do Ar";	break;	
						case 13: goal = "a Língua da Chama";	break;	
						case 14: goal = "o Coração da Terra";	break;	
						case 15: goal = "a Lágrima dos Mares";	break;	
						case 16: goal = "a Estátua de Gygax";	break;	
						case 17: goal = "a Caveira do Barão Almric";	break;	
						case 18: goal = "o Fragmento da Covardia";	break;	
						case 19: goal = "o Fragmento da Falsidade";	break;	
						case 20: goal = "o Fragmento do Ódio";	break;	
						case 21: goal = "a Gema da Imortalidade";	break;	
						case 22: goal = "o Manual dos Golems";	break;	
						case 23: goal = "o Diário de Frankenstein";	break;	
						case 24: goal = "o Cubo de Vortex";	break;	
						case 25: goal = QuestCharacters.QuestItems( false );	break;	
					}

					string fate = "morreu";	
					switch( Utility.RandomMinMax( 0, 6 ) )
					{
						case 1: fate = "desapareceu";	 			break;	
						case 2: fate = "está";	 				break;	
						case 3: fate = "quase morreu";	 			break;	
						case 4: fate = "nunca retornou enquanto";	 	break;	
						case 5: fate = "desapareceu";	 				break;	
						case 6: fate = "pereceu";	 				break;	
					}

					sSpeech = sThey + " " + fate + " " + cVal + " " + goal + "."; break;

					break;
				case 48: 
					misc = "matar";	
					switch( Utility.RandomMinMax( 0, 8 ) )
					{
						case 1: misc = "encontrar"; break;
						case 2: misc = "abater"; break;
						case 3: misc = "assassinar"; break;
						case 4: misc = "resgatar"; break;
						case 5: misc = "sequestrar"; break;
						case 6: misc = "libertar"; break;
						case 7: misc = "ajudar"; break;
						case 8: misc = "capturar"; break;
					}
					string prize = "prêmio";	
					switch( Utility.RandomMinMax( 0, 7 ) )
					{
						case 1: prize = "taxa"; break;
						case 2: prize = "recompensa"; break;
						case 3: prize = "tributo"; break;
						case 4: prize = "saco"; break;
						case 5: prize = "baú"; break;
						case 6: prize = "cofre"; break;
						case 7: prize = "pilha"; break;
					}

					if ( Utility.RandomBool() ){ sSpeech = "" + sSource + " um " + prize + " de " + RandomThings.GetRandomCoinReward() + " de ouro se nós " + misc + " " + RandomThings.GetRandomGirlName() + " a " + RandomThings.GetBoyGirlJob(1) + "."; }
					else { sSpeech = "" + sSource + " um " + prize + " de " + RandomThings.GetRandomCoinReward() + " de ouro se nós " + misc + " " + RandomThings.GetRandomBoyName() + " o " + RandomThings.GetBoyGirlJob(0) + "."; }
				break;
				case 49:
					misc = "uma guerra";	
					switch( Utility.RandomMinMax( 0, 8 ) )
					{
						case 1: misc = "uma batalha"; break;
						case 2: misc = "uma aliança"; break;
						case 3: misc = "um pacto"; break;
						case 4: misc = "um acordo comercial"; break;
						case 5: misc = "um torneio"; break;
						case 6: misc = "um impasse"; break;
						case 7: misc = "um bloqueio"; break;
						case 8: misc = "uma disputa"; break;
					}

					if ( Utility.RandomBool() ){ sSpeech = "" + sSource + " " + misc + " entre " + RandomThings.MadeUpCity() + " e " + RandomThings.MadeUpCity() + "."; }
					else { sSpeech = "" + sSource + " " + misc + " entre o " + RandomThings.GetRandomKingdomName() + " " + RandomThings.GetRandomKingdom() + " e o " + RandomThings.GetRandomKingdomName() + " " + RandomThings.GetRandomKingdom() + "."; }
				break;
				case 50: 
					misc = "";	if ( Utility.RandomBool() ){ misc = RandomThings.GetRandomGirlNoble() + " "; }
					string mis2 = "";	if ( Utility.RandomBool() ){ mis2 = RandomThings.GetRandomBoyNoble() + " "; }

					switch( Utility.RandomMinMax( 1, 8 ) )
					{
						case 1: sSpeech = "" + sSource + " " + misc + RandomThings.GetRandomGirlName() + " se casando com " + mis2 + RandomThings.GetRandomBoyName() + " em " + RandomThings.MadeUpCity() + "." ; break;
						case 2: sSpeech = "" + sSource + " " + mis2 + RandomThings.GetRandomBoyName() + " se casando com " + misc + RandomThings.GetRandomGirlName() + " em " + RandomThings.MadeUpCity() + "." ; break;
						case 3: sSpeech = "" + sSource + " " + "a " + RandomThings.GetRandomGirlNoble() + " de " + RandomThings.MadeUpCity()  + " se casando com o " + RandomThings.GetRandomBoyNoble() + " de " + RandomThings.MadeUpCity() + "." ; break;
						case 4: sSpeech = "" + sSource + " " + "o " + RandomThings.GetRandomBoyNoble() + " de " + RandomThings.MadeUpCity() + " se casando com a " + RandomThings.GetRandomGirlNoble() + " de " + RandomThings.MadeUpCity() + "." ; break;
						case 5: sSpeech = "" + sSource + " " + RandomThings.GetRandomGirlName() + " se casando com o " + RandomThings.GetRandomBoyNoble() + " de " + RandomThings.MadeUpCity() + "." ; break;
						case 6: sSpeech = "" + sSource + " " + RandomThings.GetRandomGirlName() + " se casando com o " + RandomThings.GetRandomBoyNoble() + " do " + RandomThings.GetRandomKingdomName() + " " + RandomThings.GetRandomKingdom() + "." ; break;
						case 7: sSpeech = "" + sSource + " " + RandomThings.GetRandomBoyName() + " se casando com a " + RandomThings.GetRandomGirlNoble() + " de " + RandomThings.MadeUpCity() + "." ; break;
						case 8: sSpeech = "" + sSource + " " + RandomThings.GetRandomBoyName() + " se casando com a " + RandomThings.GetRandomGirlNoble() + " do " + RandomThings.GetRandomKingdomName() + " " + RandomThings.GetRandomKingdom() + "." ; break;
					}
				break;
				case 51:
					misc = "guerra";	
					switch( Utility.RandomMinMax( 0, 12 ) )
					{
						case 1: misc = "batalha"; break;
						case 2: misc = "destruição"; break;
						case 3: misc = "praga"; break;
						case 4: misc = "maldição"; break;
						case 5: misc = "taberna"; break;
						case 6: misc = "vilania"; break;
						case 7: misc = "impostos"; break;
						case 8: misc = "problemas"; break;
						case 9: misc = "estalagem"; break;
						case 10: misc = "problemas"; break;
						case 11: misc = RandomThings.GetRandomGirlNoble(); break;
						case 12: misc = RandomThings.GetRandomBoyNoble(); break;
					}

					switch( Utility.RandomMinMax( 1, 2 ) )
					{
						case 1: sSpeech = sSource + " a " + misc + " em " + RandomThings.MadeUpCity() + "."; break;
						case 2: sSpeech = sSource + " a " + misc + " no " + RandomThings.GetRandomKingdomName() + " " + RandomThings.GetRandomKingdom() + "."; break;
					}
				break;
				case 52:
					misc = "";
						if ( Utility.RandomBool() ){ misc = " o " + RandomThings.GetBoyGirlJob(0) + ""; }
					string mis3 = "";
						if ( Utility.RandomBool() ){ mis3 = " a " + RandomThings.GetBoyGirlJob(1) + ""; }

					switch( Utility.RandomMinMax( 1, 4 ) )
					{
						case 1: sSpeech = sSource + " " + RandomThings.GetRandomBoyName() + misc + " se tornando o " + RandomThings.GetRandomBoyNoble() + " de " + RandomThings.MadeUpCity() + "."; break;
						case 2: sSpeech = sSource + " " + RandomThings.GetRandomGirlName() + mis3 + " se tornando a " + RandomThings.GetRandomGirlNoble() + " de " + RandomThings.MadeUpCity() + "."; break;
						case 3: sSpeech = sSource + " " + RandomThings.GetRandomBoyName() + misc + " se tornando o " + RandomThings.GetRandomBoyNoble() + " do " + RandomThings.GetRandomKingdomName() + " " + RandomThings.GetRandomKingdom()+ "."; break;
						case 4: sSpeech = sSource + " " + RandomThings.GetRandomGirlName() + mis3 + " se tornando a " + RandomThings.GetRandomGirlNoble() + " do " + RandomThings.GetRandomKingdomName() + " " + RandomThings.GetRandomKingdom() + "."; break;
					}
				break;
				case 53:
					misc = "destruída";	
					switch( Utility.RandomMinMax( 0, 8 ) )
					{
						case 1: misc = "capturada"; break;
						case 2: misc = "invadida"; break;
						case 3: misc = "resgatada"; break;
						case 4: misc = "libertada"; break;
						case 5: misc = "arruinada"; break;
						case 6: misc = "tomada"; break;
						case 7: misc = "cercada"; break;
						case 8: misc = "estabelecida"; break;
					}
					string mis4 = "exército";	
					switch( Utility.RandomMinMax( 0, 7 ) )
					{
						case 1: mis4 = "tropas"; break;
						case 2: mis4 = "soldados"; break;
						case 3: mis4 = "cavaleiros"; break;
						case 4: mis4 = "frota"; break;
						case 5: mis4 = RandomThings.GetRandomGirlNoble(); break;
						case 6: mis4 = RandomThings.GetRandomBoyNoble(); break;
						case 7: mis4 = "forças"; break;
					}

					switch( Utility.RandomMinMax( 1, 2 ) )
					{
						case 1: sSpeech = sSource + " " + RandomThings.MadeUpCity() + " sendo " + misc + " pelo " + mis4 + " do " + RandomThings.GetRandomKingdomName() + " " + RandomThings.GetRandomKingdom() + "."; break;
						case 2: sSpeech = sSource + " " + RandomThings.MadeUpCity() + " sendo " + misc + " pelo " + mis4 + " de " + RandomThings.MadeUpCity() + "."; break;
					}
				break;
				case 54:
					misc = "";
						if ( Utility.RandomBool() ){ misc = " o " + RandomThings.GetBoyGirlJob(0) + ""; }
					string mis5 = "";
						if ( Utility.RandomBool() ){ mis5 = " a " + RandomThings.GetBoyGirlJob(1) + ""; }
					string misc3 = "se escondendo";	
					switch( Utility.RandomMinMax( 0, 9 ) )
					{
						case 1: misc3 = "desaparecido"; break;
						case 2: misc3 = "vivendo"; break;
						case 3: misc3 = "descansando"; break;
						case 4: misc3 = "mantendo-se discreto"; break;
						case 5: misc3 = "aprisionado"; break;
						case 6: misc3 = "trancado"; break;
						case 7: misc3 = "aposentado"; break;
						case 8: misc3 = "estabelecendo"; break;
						case 9: misc3 = "iniciando " + RandomThings.GetRandomShop(); break;
					}

					string gbv346 = RandomThings.GetRandomCity(); if ( Utility.RandomBool() ){ gbv346 = RandomThings.MadeUpCity(); }

					switch( Utility.RandomMinMax( 1, 4 ) )
					{
						case 1: sSpeech = sSource + " " + RandomThings.GetRandomBoyName() + misc + " estando " + misc3 + " em " + gbv346 + "."; break;
						case 2: sSpeech = sSource + " " + RandomThings.GetRandomGirlName() + mis5 + " estando " + misc3 + " em " + gbv346 + "."; break;
						case 3: sSpeech = sSource + " " + RandomThings.GetRandomBoyName() + misc + " estando " + misc3 + " no " + RandomThings.GetRandomKingdomName() + " " + RandomThings.GetRandomKingdom()+ "."; break;
						case 4: sSpeech = sSource + " " + RandomThings.GetRandomGirlName() + mis5 + " estando " + misc3 + " no " + RandomThings.GetRandomKingdomName() + " " + RandomThings.GetRandomKingdom() + "."; break;
					}
				break;
				case 55:
					string titleA = "";
						if ( Utility.RandomBool() ){ titleA = " o " + RandomThings.GetBoyGirlJob(0) + ""; }
					string titleB = "";
						if ( Utility.RandomBool() ){ titleB = " a " + RandomThings.GetBoyGirlJob(1) + ""; }
					misc = "procurado";	
					switch( Utility.RandomMinMax( 0, 8 ) )
					{
						case 1: misc = "em julgamento"; break;
						case 2: misc = "na cadeia"; break;
						case 3: misc = "na prisão"; break;
						case 4: misc = "condenado à morte"; break;
						case 5: misc = "procurado"; break;
						case 6: misc = "acorrentado"; break;
						case 7: misc = "sentenciado"; break;
						case 8: misc = "colocado na donzela de ferro"; break;
					}
					string crime = "assassinato";	
					switch( Utility.RandomMinMax( 0, 7 ) )
					{
						case 1: crime = "roubo"; break;
						case 2: crime = "jogo ilegal"; break;
						case 3: crime = "bruxaria"; break;
						case 4: crime = "escravidão"; break;
						case 5: crime = "tentativa de assassinato"; break;
						case 6: crime = "devassidão"; break;
						case 7: crime = "embriaguez"; break;
					}

					string bjj311 = RandomThings.GetRandomCity(); if ( Utility.RandomBool() ){ bjj311 = RandomThings.MadeUpCity(); }

					switch( Utility.RandomMinMax( 1, 2 ) )
					{
						case 1: sSpeech = sSource + " " + RandomThings.GetRandomBoyName() + titleA + " estando " + misc + " por " + crime + " em " + bjj311 + "."; break;
						case 2: sSpeech = sSource + " " + RandomThings.GetRandomGirlName() + titleB + " estando " + misc + " por " + crime + " em " + bjj311 + "."; break;
					}
				break;
				case 56:
					string titleC = "";
						if ( Utility.RandomBool() ){ titleC = " o " + RandomThings.GetBoyGirlJob(0) + ""; }
					string titleD = "";
						if ( Utility.RandomBool() ){ titleD = " a " + RandomThings.GetBoyGirlJob(1) + ""; }

					string town = RandomThings.MadeUpDungeon();
					switch( Utility.RandomMinMax( 0, 3 ) )
					{
						case 1: town = QuestCharacters.SomePlace( null ); break;
						case 2: town = RandomThings.MadeUpCity(); break;
						case 3: town = RandomThings.GetRandomCity(); break;
					}
					misc = "escondendo";	
					switch( Utility.RandomMinMax( 0, 7 ) )
					{
						case 1: misc = "enterrando"; break;
						case 2: misc = "trazendo"; break;
						case 3: misc = "perdendo"; break;
						case 4: misc = "encontrando"; break;
						case 5: misc = "procurando por"; break;
						case 6: misc = "entregando"; break;
						case 7: misc = "deixando"; break;
					}
					misc2 = "escondido";	
					switch( Utility.RandomMinMax( 0, 3 ) )
					{
						case 1: misc2 = "enterrado"; break;
						case 2: misc2 = "perdido"; break;
						case 3: misc2 = "esperando"; break;
					}
					string loot = RandomThings.RandomMagicalItem();
					switch( Utility.RandomMinMax( 1, 12 ) )
					{
						case 1: loot = "tesouro"; break;
						case 2: loot = "ouro"; break;
						case 3: loot = "cristais"; break;
						case 4: loot = "gemas"; break;
						case 5: loot = "joias"; break;
						case 6: loot = "moedas"; break;
					}
					string locale = "perto de";	
					switch( Utility.RandomMinMax( 0, 5 ) )
					{
						case 1: locale = "nos arredores de"; break;
						case 2: locale = "fora de"; break;
						case 3: locale = "dentro de"; break;
						case 4: locale = "em"; break;
						case 5: locale = "próximo a"; break;
					}
					if ( Utility.RandomBool() ){ locale = "em algum lugar " + locale; }

					switch( Utility.RandomMinMax( 1, 4 ) )
					{
						case 1: sSpeech = sSource + " " + RandomThings.GetRandomBoyName() + titleC + " " + misc + " o " + loot + " " + locale + " " + town + "."; break;
						case 2: sSpeech = sSource + " " + RandomThings.GetRandomGirlName() + titleD + " " + misc + " o " + loot + " " + locale + " " + town + "."; break;
						case 3: sSpeech = sSource + " o " + loot + " estando " + misc2 + " " + locale + " " + town + "."; break;
						case 4: sSpeech = sSource + " o " + loot + " estando " + misc2 + " " + locale + " " + town + "."; break;
					}
				break;
				case 57:
					string titleE = "";
						if ( Utility.RandomBool() ){ titleE = " o " + RandomThings.GetBoyGirlJob(0) + ""; }
					string titleF = "";
						if ( Utility.RandomBool() ){ titleF = " a " + RandomThings.GetBoyGirlJob(1) + ""; }

					string tomb = RandomThings.MadeUpDungeon();
					if ( Utility.RandomBool() ){ tomb = QuestCharacters.SomePlace( null ); }

					misc = "matando";	
					switch( Utility.RandomMinMax( 0, 8 ) )
					{
						case 1: misc = "abatendo"; break;
						case 2: misc = "sendo morto por"; break;
						case 3: misc = "sendo abatido por"; break;
						case 4: misc = "fugindo de"; break;
						case 5: misc = "perseguindo"; break;
						case 6: misc = "caçando"; break;
						case 7: misc = "procurando por"; break;
						case 8: misc = "nunca encontrando"; break;
					}

					switch( Utility.RandomMinMax( 1, 2 ) )
					{
						case 1: sSpeech = sSource + " " + RandomThings.GetRandomBoyName() + titleE + " " + misc + " " + RandomThings.GetRandomMonsters() + " em " + tomb + "."; break;
						case 2: sSpeech = sSource + " " + RandomThings.GetRandomGirlName() + titleF + " " + misc + " " + RandomThings.GetRandomMonsters() + " em " + tomb + "."; break;
					}
				break;
				case 58:
					if ( Utility.RandomBool() ){ dungeon = city; }
					misc = "procurar por";	
					switch( Utility.RandomMinMax( 0, 8 ) )
					{
						case 1: misc = "procurar por"; break;
						case 2: misc = "encontrar"; break;
						case 3: misc = "buscar"; break;
						case 4: misc = "tentar encontrar"; break;
						case 5: misc = "emboscar"; break;
						case 6: misc = "surpreender"; break;
						case 7: misc = "tentar emboscar"; break;
						case 8: misc = "tentar capturar"; break;
					}
					sSpeech = "Nós vamos " + misc + " " + sThey + " em " + dungeon + "";
					if ( Utility.RandomBool() ){ sSpeech = sSpeech + " amanhã"; } sSpeech = sSpeech + ".";
				break;
			}

			string sGossip = sSpeech;	

			switch( Utility.RandomMinMax( 1, ( 11 + CommonTalkingCount ) ) )
			{
				case 1: sGossip = "Outra cerveja aqui!"; break;	
				case 2: sGossip = "Mais vinho!"; break;	
				case 3: sGossip = "Posso receber outro caneco aqui?"; break;	
				case 4: sGossip = "O que é preciso para conseguir uma boa bebida neste lugar?"; break;	
				case 5: sGossip = sThey + " disse que este é o melhor lugar para beber."; break;	
				case 6: sGossip = sThey + " mora por aqui em algum lugar."; break;	
				case 7: sGossip = "Levante um caneco para " + sThey + ", pois não nos esqueceremos deles."; break;	
				case 8: sGossip = "Deveríamos comer enquanto estamos aqui."; break;	
				case 9: sGossip = "este é um vinho muito bom."; break;	
				case 10: sGossip = "Nunca tomei uma cerveja assim."; break;	
				case 11: sGossip = "Estou começando a achar que eles adulteram as bebidas."; break;	
			}

			string sTent = sSpeech;	
			switch( Utility.RandomMinMax( 1, ( 5 + CommonTalkingCount ) ) )
			{
				case 1: sTent = sThey + " disse que este é o lugar mais seguro para acampar."; break;	
				case 2: sTent = "Levante um caneco para " + sThey + ", pois não nos esqueceremos deles."; break;	
				case 3: sTent = "Deveríamos comer enquanto descansamos aqui."; break;	
				case 4: sTent = "este é um vinho muito bom que você trouxe."; break;	
				case 5: sTent = "Nunca tomei uma cerveja assim."; break;	
			}

			string sCitizen = sSpeech;	
			switch( Utility.RandomMinMax( 1, ( 2 + CommonTalkingCount ) ) )
			{
				case 1: sCitizen = sThey + " disse que este é o lugar mais seguro para ficar."; break;	
				case 2: sCitizen = sThey + " mora em algum lugar perto de " + city + "."; break;	
			}

			string sHappen = "Um amigo meu morreu"; string sEnd = ".";	
			switch( Utility.RandomMinMax( 0, 35 ) )
			{
				case 0: sHappen = "Um amigo meu se perdeu em"; sEnd = "."; break;	
				case 1: sHappen = "Um amigo meu morreu em"; sEnd = "."; break;	
				case 2: sHappen = "Eu perdi aquela arma em"; sEnd = "."; break;	
				case 3: sHappen = "Você já esteve em"; sEnd = "?"; break;	
				case 4: sHappen = "Você já ouviu falar de"; sEnd = "?"; break;	
				case 5: sHappen = "Quando você foi para"; sEnd = "?"; break;	
				case 6: sHappen = "Como você chegou a"; sEnd = "?"; break;	
				case 7: sHappen = "Por que você foi para"; sEnd = "?"; break;	
				case 8: sHappen = "O que você encontrou em"; sEnd = "?"; break;	
				case 9: sHappen = "Você encontrou isso em"; sEnd = "?"; break;	
				case 10: sHappen = "Eles morreram em"; sEnd = "."; break;	
				case 11: sHappen = "Eu nunca estive em"; sEnd = "."; break;	
				case 12: sHappen = "Aquele artefato veio de"; sEnd = "."; break;	
				case 13: sHappen = "Eles se perderam em"; sEnd = "."; break;	
				case 14: sHappen = "Eles desapareceram em"; sEnd = "."; break;	
				case 15: sHappen = "Eu quase não consegui sair de"; sEnd = "."; break;	
				case 16: sHappen = "Eles não conseguiram sair de"; sEnd = "."; break;	
				case 17: sHappen = "Eu perdi aquele item mágico em"; sEnd = "."; break;	
				case 18: sHappen = "Você perdeu isso em"; sEnd = "?"; break;	
				case 19: sHappen = "Nós deveríamos procurar em"; sEnd = "."; break;	
				case 20: sHappen = "Nós deveríamos explorar"; sEnd = "."; break;	
				case 21: sHappen = "Esta noite nós vamos para"; sEnd = "."; break;	
				case 22: sHappen = sThey + " se perdeu em"; sEnd = "."; break;	
				case 23: sHappen = sThey + " morreu em"; sEnd = "."; break;	
				case 24: sHappen = sThey + " perdeu aquela arma em"; sEnd = "."; break;	
				case 25: sHappen = "Quando " + sThey + " foi para"; sEnd = "?"; break;	
				case 26: sHappen = "Como " + sThey + " chegou a"; sEnd = "?"; break;	
				case 27: sHappen = "Por que " + sThey + " foi para"; sEnd = "?"; break;	
				case 28: sHappen = "O que " + sThey + " encontrou em"; sEnd = "?"; break;	
				case 29: sHappen = sThey + " encontrou isso em"; sEnd = "?"; break;	
				case 30: sHappen = sThey + " nunca esteve em"; sEnd = "."; break;	
				case 31: sHappen = sThey + " desapareceu em"; sEnd = "."; break;	
				case 32: sHappen = sThey + " quase não conseguiu sair de"; sEnd = "."; break;	
				case 33: sHappen = sThey + " não conseguiu sair de"; sEnd = "."; break;	
				case 34: sHappen = sThey + " perdeu aquele item mágico em"; sEnd = "."; break;	
				case 35: sHappen = sThey + " perdeu isso em"; sEnd = "?"; break;	
			}

			string sEvent = sHappen + " " + dungeon + sEnd;	

			string sWords = CommonTalk( "", city, dungeon, patron, sAdventurer, false );	

			int LogReader = 0;	
			if ( sWords == "" )
			{
				sWords = Server.Misc.LoggingFunctions.LogSpeak();	
				LogReader = 1;	
				if ( Utility.RandomMinMax( 1, 4 ) == 1 ){ sWords = Server.Misc.LoggingFunctions.LogSpeakQuest(); LogReader = 2; }
			}

			string sJob = sThey;	
			switch( Utility.RandomMinMax( 0, 86 ) )
			{
					case 0: sJob = "Um aventureiro"; break;	
					case 1: sJob = "Um bandido"; break;	
					case 2: sJob = "Um bárbaro"; break;	
					case 3: sJob = "Um bardo"; break;	
					case 4: sJob = "Um barão"; break;	
					case 5: sJob = "Uma baronesa"; break;	
					case 6: sJob = "Um cavaleiro"; break;	
					case 7: sJob = "Um clérigo"; break;	
					case 8: sJob = "Um conjurador"; break;	
					case 9: sJob = "Um defensor"; break;	
					case 10: sJob = "Um adivinho"; break;	
					case 11: sJob = "Um encantador"; break;	
					case 12: sJob = "Uma encantadora"; break;	
					case 13: sJob = "Um explorador"; break;	
					case 14: sJob = "Um guerreiro"; break;	
					case 15: sJob = "Um gladiador"; break;	
					case 16: sJob = "Um herege"; break;	
					case 17: sJob = "Um caçador"; break;	
					case 18: sJob = "Um ilusionista"; break;	
					case 19: sJob = "Um invocador"; break;	
					case 20: sJob = "Um rei"; break;	
					case 21: sJob = "Um cavaleiro"; break;	
					case 22: sJob = "Uma dama"; break;	
					case 23: sJob = "Um lord"; break;	
					case 24: sJob = "Um mago"; break;	
					case 25: sJob = "Um magician"; break;	
					case 26: sJob = "Um mercenário"; break;	
					case 27: sJob = "Um menestrel"; break;	
					case 28: sJob = "Um monge"; break;	
					case 29: sJob = "Um místico"; break;	
					case 30: sJob = "Um necromante"; break;	
					case 31: sJob = "Um fora da lei"; break;	
					case 32: sJob = "Um paladino"; break;	
					case 33: sJob = "Um sacerdote"; break;	
					case 34: sJob = "Uma sacerdotisa"; break;	
					case 35: sJob = "Um príncipe"; break;	
					case 36: sJob = "Uma princesa"; break;	
					case 37: sJob = "Um profeta"; break;	
					case 38: sJob = "Uma rainha"; break;	
					case 39: sJob = "Um ranger"; break;	
					case 40: sJob = "Um ladino"; break;	
					case 41: sJob = "Um sábio"; break;	
					case 42: sJob = "Um batedor"; break;	
					case 43: sJob = "Um buscador"; break;	
					case 44: sJob = "Um vidente"; break;	
					case 45: sJob = "Um xamã"; break;	
					case 46: sJob = "Um exterminador"; break;	
					case 47: sJob = "Um feiticeiro"; break;	
					case 48: sJob = "Uma feiticeira"; break;	
					case 49: sJob = "Um invocador"; break;	
					case 50: sJob = "Um templário"; break;	
					case 51: sJob = "Um ladrão"; break;	
					case 52: sJob = "Um viajante"; break;	
					case 53: sJob = "Um bruxo"; break;	
					case 54: sJob = "Um guerreiro"; break;	
					case 55: sJob = "Uma bruxa"; break;	
					case 56: sJob = "Um mago"; break;	
			}

				string sBuild1 = "Eu encontrei"; string sBuild2 = ".";	

			if ( LogReader == 1 )
			{
				switch( Utility.RandomMinMax( 0, 11 ) )
				{
					case 0: sBuild1 = sJob + " ouviu falar de"; sBuild2 = "."; break;	
					case 1: sBuild1 = sJob + " conta sobre"; sBuild2 = "."; break;	
					case 2: sBuild1 = sJob + " está espalhando rumores sobre"; sBuild2 = "."; break;	
					case 3: sBuild1 = sJob + " conta histórias de"; sBuild2 = "."; break;	
					case 4: sBuild1 = sJob + " mencionou algo sobre"; sBuild2 = "."; break;	
					case 5: sBuild1 = sJob + " ouviu rumores sobre"; sBuild2 = "."; break;	
					case 6: sBuild1 = "Eu encontrei"; sBuild2 = "."; break;	
					case 7: sBuild1 = "Eu ouvi rumores sobre"; sBuild2 = "."; break;	
					case 8: sBuild1 = "Eu ouvi uma história sobre"; sBuild2 = "."; break;	
					case 9: sBuild1 = "Eu ouvi alguém contar sobre"; sBuild2 = "."; break;	
					case 10: sBuild1 = "Você estava dizendo algo sobre"; sBuild2 = "?"; break;	
					case 11: sBuild1 = "Onde eu ouvi sobre"; sBuild2 = "?"; break;	
				}
			}
			else if ( LogReader == 0 )
			{
				switch( Utility.RandomMinMax( 0, 13 ) )
				{
					case 0: sBuild1 = sJob + " encontrou"; sBuild2 = "."; break;	
					case 1: sBuild1 = sJob + " conta sobre"; sBuild2 = "."; break;	
					case 2: sBuild1 = sJob + " está espalhando rumores sobre"; sBuild2 = "."; break;	
					case 3: sBuild1 = sJob + " conta histórias de"; sBuild2 = "."; break;	
					case 4: sBuild1 = sJob + " mencionou que havia"; sBuild2 = "."; break;	
					case 5: sBuild1 = sJob + " ouviu rumores sobre"; sBuild2 = "."; break;	
					case 6: sBuild1 = "Eu encontrei"; sBuild2 = "."; break;	
					case 7: sBuild1 = "Eu ouvi rumores sobre"; sBuild2 = "."; break;	
					case 8: sBuild1 = "Eu ouvi uma história sobre"; sBuild2 = "."; break;	
					case 9: sBuild1 = "Eu ouvi alguém contar sobre"; sBuild2 = "."; break;	
					case 10: sBuild1 = "Você estava dizendo que há"; sBuild2 = "?"; break;	
					case 11: sBuild1 = "Onde eu ouvi que há"; sBuild2 = "?"; break;	
					case 12: sBuild1 = "Você está me dizendo que há"; sBuild2 = "?"; break;	
					case 13: sBuild1 = "Você quer dizer que há"; sBuild2 = "?"; break;	
				}
			}

			string sPhrase = sBuild1 + " " + sWords + sBuild2;	

			if ( LogReader == 2 )
			{
				sPhrase = sWords + ".";	
			}

			Region reg = Region.Find( patron.Location, patron.Map );	

			int iWillSay = Utility.RandomMinMax( 1, 8 );	

			if ( iWillSay < 3 )
			{
				switch( Utility.RandomMinMax( 1, 39 ) )
				{
					case 1: patron.PlaySound( patron.Female ? 778 : 1049 ); patron.Say( "*ah!*" ); break;	
					case 2: patron.PlaySound( patron.Female ? 779 : 1050 ); patron.Say( "Ah ha!" ); break;	
					case 3: patron.PlaySound( patron.Female ? 780 : 1051 ); patron.Say( "*aplaude*" ); break;	
					case 4: patron.PlaySound( patron.Female ? 781 : 1052 ); patron.Say( "*assoa o nariz*" );	break;	
					case 5: patron.PlaySound( patron.Female ? 786 : 1057 ); patron.Say( "*tosse*" ); break;	
					case 6: patron.PlaySound( patron.Female ? 782 : 1053 ); patron.Say( "*arroto*" ); break;	
					case 7: patron.PlaySound( patron.Female ? 784 : 1055 ); patron.Say( "*limpa a garganta*" ); break;	
					case 8: patron.PlaySound( patron.Female ? 785 : 1056 ); patron.Say( "*tosse*" ); break;	
					case 9: patron.PlaySound( patron.Female ? 787 : 1058 ); patron.Say( "*chora*" ); break;	
					case 10: patron.PlaySound( patron.Female ? 792 : 1064 ); patron.Say( "*peida*" ); break;	
					case 11: patron.PlaySound( patron.Female ? 793 : 1065 ); patron.Say( "*suspiro*" ); break;	
					case 12: patron.PlaySound( patron.Female ? 794 : 1066 ); patron.Say( "*risada*" ); break;	
					case 13: patron.PlaySound( patron.Female ? 0x31B : 0x42B ); patron.Say( "*gemido*" ); break;	
					case 14: patron.PlaySound( patron.Female ? 0x338 : 0x44A ); patron.Say( "*rosna*" ); break;	
					case 15: patron.PlaySound( patron.Female ? 797 : 1069 ); patron.Say( "Ei!" ); break;	
					case 16: patron.PlaySound( patron.Female ? 798 : 1070 ); patron.Say( "*soluço*" ); break;	
					case 17: patron.PlaySound( patron.Female ? 799 : 1071 ); patron.Say( "Hã?" ); break;	
					case 18: patron.PlaySound( patron.Female ? 801 : 1073 ); patron.Say( "*ri*" ); break;	
					case 19: patron.PlaySound( patron.Female ? 802 : 1074 ); patron.Say( "Não!" ); break;	
					case 20: patron.PlaySound( patron.Female ? 803 : 1075 ); patron.Say( "Oh!" ); break;	
					case 21: patron.PlaySound( patron.Female ? 811 : 1085 ); patron.Say( "Oooh." ); break;	
					case 22: patron.PlaySound( patron.Female ? 812 : 1086 ); patron.Say( "Ops!" ); break;	
					case 23: patron.PlaySound( patron.Female ? 0x32E : 0x440 ); patron.Say( "Ahhhh!" ); break;	
					case 24: patron.PlaySound( patron.Female ? 815 : 1089 ); patron.Say( "Shhh!" ); break;	
					case 25: patron.PlaySound( patron.Female ? 816 : 1090 ); patron.Say( "*suspiro*" ); break;	
					case 26: patron.PlaySound( patron.Female ? 817 : 1091 ); patron.Say( "Atchim!" ); break;	
					case 27: patron.PlaySound( patron.Female ? 818 : 1092 ); patron.Say( "*fungada*" ); break;	
					case 28: patron.PlaySound( patron.Female ? 819 : 1093 ); patron.Say( "*ronco*" ); break;	
					case 29: patron.PlaySound( patron.Female ? 820 : 1094 ); patron.Say( "*cospe*" ); break;	
					case 30: patron.PlaySound( patron.Female ? 821 : 1095 ); patron.Say( "*assobia*" ); break;	
					case 31: patron.PlaySound( patron.Female ? 783 : 1054 ); patron.Say( "Uhuu!" ); break;	
					case 32: patron.PlaySound( patron.Female ? 822 : 1096 ); patron.Say( "*bocejo*" ); break;	
					case 33: patron.PlaySound( patron.Female ? 823 : 1097 ); patron.Say( "Sim!" ); break;	
					case 34: patron.PlaySound( patron.Female ? 0x31C : 0x42C ); patron.Say( "*grita*" ); break;	
					case 35: patron.PlaySound( Utility.RandomList( 0x30, 0x2D6 ) ); break;	
					case 36: patron.PlaySound( Utility.RandomList( 0x30, 0x2D6 ) ); break;	
					case 37: patron.PlaySound( Utility.RandomList( 0x30, 0x2D6 ) ); break;	
					case 38: patron.PlaySound( Utility.RandomList( 0x30, 0x2D6 ) ); break;	
					case 39: patron.PlaySound( Utility.RandomList( 0x30, 0x2D6 ) ); break;	
				}
			}
				else if ( iWillSay < 5 ){ patron.Say( sPhrase ); }
				else if ( iWillSay < 7 ){ patron.Say( sEvent ); }
				else if ( reg.Name == "the Basement" || reg.Name == "the Dungeon Room" || reg.Name == "the Camping Tent" ) { patron.Say( sTent ); }
				else if ( !( patron is TavernPatronNorth || patron is TavernPatronSouth || patron is TavernPatronEast || patron is TavernPatronWest ) ) { patron.Say( sCitizen ); }
				else { patron.Say( sGossip ); }
		}
	}
}
