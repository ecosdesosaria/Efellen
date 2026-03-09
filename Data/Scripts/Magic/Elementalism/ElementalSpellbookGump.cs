using System; 
using System.Collections; 
using Server; 
using Server.Items; 
using Server.Misc; 
using Server.Network; 
using Server.Spells; 
using Server.Spells.Elementalism; 
using Server.Prompts; 

namespace Server.Gumps 
{ 
	public class ElementalSpellbookGump : Gump 
	{
		private ElementalSpellbook m_Book; 

		public bool HasSpell(Mobile from, int spellID)
		{
			if ( m_Book.RootParentEntity == from )
				return (m_Book.HasSpell(spellID));
			else
				return false;
		}

		public ElementalSpellbookGump( Mobile from, ElementalSpellbook book, int page ) : base( 100, 100 ) 
		{
			m_Book = book; 
			m_Book.EllyPage = page;

            this.Closable=true;
			this.Disposable=true;
			this.Dragable=true;
			this.Resizable=false;

			int hue = 2884;
			string fnt = ElementalSpell.FontColor( m_Book.ItemID );

			if ( m_Book.ItemID == 0x6713 ) // EARTH
			{
				hue = 2884;
			}
			else if ( m_Book.ItemID == 0x6715 ) // WATER
			{
				hue = 2876;
			}
			else if ( m_Book.ItemID == 0x6717 ) // AIR
			{
				hue = 2807;
			}
			else if ( m_Book.ItemID == 0x6719 ) // FIRE
			{
				hue = 2464;
			}

			AddPage(0);
			AddImage(0, 0, 11138, hue);
			AddImage(0, 0, 11152);
			AddImage(0, 0, 11147);

			int PriorPage = page - 1;
				if ( PriorPage < 1 ){ PriorPage = 37; }
			int NextPage = page + 1;
				if ( NextPage > 37 ){ NextPage = 1; }

			AddButton(24, 8, 11159, 11159, PriorPage, GumpButtonType.Reply, 0); 	// PAGE LEFT
			AddButton(295, 8, 11160, 11160, NextPage, GumpButtonType.Reply, 0); 	// PAGE RIGHT

			AddButton(40, 209, 2095, 2095, (500+page), GumpButtonType.Reply, 0);	// HELP

			if ( page == 1 ) // MAIN PAGE
			{
				AddHtml( 85, 12, 89, 20, @"<BODY><BASEFONT Color=" + fnt + "><BIG>Elemental</BIG></BASEFONT></BODY>", (bool)false, (bool)false);
				AddHtml( 207, 12, 89, 20, @"<BODY><BASEFONT Color=" + fnt + "><BIG>Spellbook</BIG></BASEFONT></BODY>", (bool)false, (bool)false);
				AddHtml( 34, 35, 133, 160, @"<BODY><BASEFONT Color=" + fnt + ">Feitiços elementais conhecidos podem ser lançados ao selecionar o botão ao lado deles. Um feitiço conhecido também terá um marcador em no qual você pode clicar para lançar.</BASEFONT></BODY>", (bool)false, (bool)false);
				AddHtml( 188, 35, 133, 160, @"<BODY><BASEFONT Color=" + fnt + ">Cada feitiço tem o poder necessário para conjurar, que é a quantidade de mana e vigor que o conjurador precisa. O marcador abaixo pode ser selecionado para visualizar as informações.</BASEFONT></BODY>", (bool)false, (bool)false);
			}
			else if ( page > 1 && page < 6 ) // SPELL LISTS
			{
				int id = 299;
				string seca = "I";
				string secb = "II";

				if ( page == 2 ){ 		id = 299; 	seca = "I";		secb = "II";	}
				else if ( page == 3 ){ 	id = 307; 	seca = "III";	secb = "IV";	}
				else if ( page == 4 ){ 	id = 315; 	seca = "V";		secb = "VI";	}
				else if ( page == 5 ){ 	id = 323; 	seca = "VII";	secb = "VIII";	}

				AddHtml( 59, 14, 100, 20, @"<BODY><BASEFONT Color=" + fnt + "><BIG>Sphere " + seca + "</BIG></BASEFONT></BODY>", (bool)false, (bool)false);
				AddHtml( 207, 14, 100, 20, @"<BODY><BASEFONT Color=" + fnt + "><BIG>Sphere " + secb + "</BIG></BASEFONT></BODY>", (bool)false, (bool)false);

				id++;
				if ( HasSpell(from, id) ){ AddButton(35, 55, 2224, 2224, id, GumpButtonType.Reply, 0); }
				AddHtml( 60, 50, 102, 20, @"<BODY><BASEFONT Color=" + fnt + "><BIG>" + ElementalSpell.CommonInfo( id, 1 ) + "</BIG></BASEFONT></BODY>", (bool)false, (bool)false);
				AddButton(155, 55, 2104, 2104, (id-300+6), GumpButtonType.Reply, 0);

				id++;
				if ( HasSpell(from, id) ){ AddButton(35, 95, 2224, 2224, id, GumpButtonType.Reply, 0); }
				AddHtml( 60, 90, 102, 20, @"<BODY><BASEFONT Color=" + fnt + "><BIG>" + ElementalSpell.CommonInfo( id, 1 ) + "</BIG></BASEFONT></BODY>", (bool)false, (bool)false);
				AddButton(155, 95, 2104, 2104, (id-300+6), GumpButtonType.Reply, 0);

				id++;
				if ( HasSpell(from, id) ){ AddButton(35, 135, 2224, 2224, id, GumpButtonType.Reply, 0); }
				AddHtml( 60, 130, 102, 20, @"<BODY><BASEFONT Color=" + fnt + "><BIG>" + ElementalSpell.CommonInfo( id, 1 ) + "</BIG></BASEFONT></BODY>", (bool)false, (bool)false);
				AddButton(155, 135, 2104, 2104, (id-300+6), GumpButtonType.Reply, 0);

				id++;
				if ( HasSpell(from, id) ){ AddButton(35, 175, 2224, 2224, id, GumpButtonType.Reply, 0); }
				AddHtml( 60, 170, 102, 20, @"<BODY><BASEFONT Color=" + fnt + "><BIG>" + ElementalSpell.CommonInfo( id, 1 ) + "</BIG></BASEFONT></BODY>", (bool)false, (bool)false);
				AddButton(155, 175, 2104, 2104, (id-300+6), GumpButtonType.Reply, 0);

				id++;
				if ( HasSpell(from, id) ){ AddButton(190, 55, 2224, 2224, id, GumpButtonType.Reply, 0); }
				AddHtml( 215, 50, 102, 20, @"<BODY><BASEFONT Color=" + fnt + "><BIG>" + ElementalSpell.CommonInfo( id, 1 ) + "</BIG></BASEFONT></BODY>", (bool)false, (bool)false);
				AddButton(310, 55, 2104, 2104, (id-300+6), GumpButtonType.Reply, 0);

				id++;
				if ( HasSpell(from, id) ){ AddButton(190, 95, 2224, 2224, id, GumpButtonType.Reply, 0); }
				AddHtml( 215, 90, 102, 20, @"<BODY><BASEFONT Color=" + fnt + "><BIG>" + ElementalSpell.CommonInfo( id, 1 ) + "</BIG></BASEFONT></BODY>", (bool)false, (bool)false);
				AddButton(310, 95, 2104, 2104, (id-300+6), GumpButtonType.Reply, 0);

				id++;
				if ( HasSpell(from, id) ){ AddButton(190, 135, 2224, 2224, id, GumpButtonType.Reply, 0); }
				AddHtml( 215, 130, 102, 20, @"<BODY><BASEFONT Color=" + fnt + "><BIG>" + ElementalSpell.CommonInfo( id, 1 ) + "</BIG></BASEFONT></BODY>", (bool)false, (bool)false);
				AddButton(310, 135, 2104, 2104, (id-300+6), GumpButtonType.Reply, 0);

				id++;
				if ( HasSpell(from, id) ){ AddButton(190, 175, 2224, 2224, id, GumpButtonType.Reply, 0); }
				AddHtml( 215, 170, 102, 20, @"<BODY><BASEFONT Color=" + fnt + "><BIG>" + ElementalSpell.CommonInfo( id, 1 ) + "</BIG></BASEFONT></BODY>", (bool)false, (bool)false);
				AddButton(310, 175, 2104, 2104, (id-300+6), GumpButtonType.Reply, 0);
			}
			else // SPELL DESCRIPTIONS
			{
				int spell = 294 + page;
				string sphere = "I";
				int circle = 0;
				int skill = 0;
				int sect = 2;

				if ( spell >= 300 && spell <= 303 ){ 		sphere = "I"; 		circle = 1;		skill = 0;		sect = 2;	}
				else if ( spell >= 304 && spell <= 307 ){ 	sphere = "II"; 		circle = 2;		skill = 0;		sect = 2;	}
				else if ( spell >= 308 && spell <= 311 ){ 	sphere = "III"; 	circle = 3;		skill = 9;		sect = 3;	}
				else if ( spell >= 312 && spell <= 315 ){ 	sphere = "IV"; 		circle = 4;		skill = 23;		sect = 3;	}
				else if ( spell >= 316 && spell <= 319 ){ 	sphere = "V"; 		circle = 5;		skill = 38;		sect = 4;	}
				else if ( spell >= 320 && spell <= 323 ){ 	sphere = "VI"; 		circle = 6;		skill = 52;		sect = 4;	}
				else if ( spell >= 324 && spell <= 327 ){ 	sphere = "VII"; 	circle = 7;		skill = 66;		sect = 5;	}
				else if ( spell >= 328 && spell <= 331 ){ 	sphere = "VIII"; 	circle = 8;		skill = 80;		sect = 5;	}

				string power = (ElementalSpell.GetPower( circle-1 )).ToString();
				
				int spellIcon = ElementalSpell.SpellIcon( book.ItemID, spell );
				if ( HasSpell(from, spell) ){ AddButton(74, 86, spellIcon, spellIcon, spell, GumpButtonType.Reply, 0); }
				else AddImage( 74, 86, spellIcon );

				var spellName = ElementalSpell.CommonInfo( spell, 1 );

				AddHtml( 34, 13, 133, 20, @"<BODY><BASEFONT Color=" + fnt + "><BIG><CENTER>Elemental</CENTER></BIG></BASEFONT></BODY>", (bool)false, (bool)false);
				AddHtml( 34, 29, 133, 20, @"<BODY><BASEFONT Color=" + fnt + "><BIG><CENTER>" + spellName + "</CENTER></BIG></BASEFONT></BODY>", (bool)false, (bool)false);
				AddHtml( 26, 130, 144, 20, @"<BODY><BASEFONT Color=" + fnt + "><CENTER>[E" + spellName + "</CENTER></BASEFONT></BODY>", (bool)false, (bool)false);
				AddHtml( 34, 166, 100, 20, @"<BODY><BASEFONT Color=" + fnt + "><BIG>Power:</BIG></BASEFONT></BODY>", (bool)false, (bool)false);
				AddHtml( 139, 166, 38, 20, @"<BODY><BASEFONT Color=" + fnt + "><BIG>" + power + "</BIG></BASEFONT></BODY>", (bool)false, (bool)false);

				AddHtml( 34, 184, 100, 20, @"<BODY><BASEFONT Color=" + fnt + "><BIG>Elementalism:</BIG></BASEFONT></BODY>", (bool)false, (bool)false);
				AddHtml( 139, 184, 38, 20, @"<BODY><BASEFONT Color=" + fnt + "><BIG>" + skill + "</BIG></BASEFONT></BODY>", (bool)false, (bool)false);

				AddHtml( 190, 14, 100, 20, @"<BODY><BASEFONT Color=" + fnt + "><BIG>Sphere " + sphere + "</BIG></BASEFONT></BODY>", (bool)false, (bool)false);
				AddHtml( 189, 38, 134, 159, @"<BODY><BASEFONT Color=" + fnt + ">" + ElementalSpell.DescriptionInfo( spell, m_Book.ItemID ) + "</BASEFONT></BODY>", (bool)false, (bool)false);

				AddButton(35, 35, 2104, 2104, sect, GumpButtonType.Reply, 0);
			}
		}

		public override void OnResponse( NetState state, RelayInfo info ) 
		{
			Mobile from = state.Mobile; 
			from.SendSound( 0x55 );

			if ( info.ButtonID < 200 && info.ButtonID > 0 )
			{
				int page = info.ButtonID;
				if ( page < 1 ){ page = 37; }
				if ( page > 37 ){ page = 1; }
				from.SendGump( new ElementalSpellbookGump( from, m_Book, page ) );
			}
			else if ( info.ButtonID >= 500 )
			{
				from.SendGump( new ElementalSpellbookGump( from, m_Book, (info.ButtonID-500) ) );
				from.SendGump( new ElementalSpellHelp( from, m_Book, 1 ) );
			}
			else if ( info.ButtonID > 200 && HasSpell(from, info.ButtonID) )
			{
				if ( info.ButtonID == 300 ){ new Elemental_Armor_Spell( from, null ).Cast(); }
				else if ( info.ButtonID == 301 ){ new Elemental_Bolt_Spell( from, null ).Cast(); }
				else if ( info.ButtonID == 302 ){ new Elemental_Mend_Spell( from, null ).Cast(); }
				else if ( info.ButtonID == 303 ){ new Elemental_Sanctuary_Spell( from, null ).Cast(); }
				else if ( info.ButtonID == 304 ){ new Elemental_Pain_Spell( from, null ).Cast(); }
				else if ( info.ButtonID == 305 ){ new Elemental_Protection_Spell( from, null ).Cast(); }
				else if ( info.ButtonID == 306 ){ new Elemental_Purge_Spell( from, null ).Cast(); }
				else if ( info.ButtonID == 307 ){ new Elemental_Steed_Spell( from, null ).Cast(); }
				else if ( info.ButtonID == 308 ){ new Elemental_Call_Spell( from, null ).Cast(); }
				else if ( info.ButtonID == 309 ){ new Elemental_Force_Spell( from, null ).Cast(); }
				else if ( info.ButtonID == 310 ){ new Elemental_Wall_Spell( from, null ).Cast(); }
				else if ( info.ButtonID == 311 ){ new Elemental_Warp_Spell( from, null ).Cast(); }
				else if ( info.ButtonID == 312 ){ new Elemental_Field_Spell( from, null ).Cast(); }
				else if ( info.ButtonID == 313 ){ new Elemental_Restoration_Spell( from, null ).Cast(); }
				else if ( info.ButtonID == 314 ){ new Elemental_Strike_Spell( from, null ).Cast(); }
				else if ( info.ButtonID == 315 ){ new Elemental_Void_Spell( from, null ).Cast(); }
				else if ( info.ButtonID == 316 ){ new Elemental_Blast_Spell( from, null ).Cast(); }
				else if ( info.ButtonID == 317 ){ new Elemental_Echo_Spell( from, null ).Cast(); }
				else if ( info.ButtonID == 318 ){ new Elemental_Fiend_Spell( from, null ).Cast(); }
				else if ( info.ButtonID == 319 ){ new Elemental_Hold_Spell( from, null ).Cast(); }
				else if ( info.ButtonID == 320 ){ new Elemental_Barrage_Spell( from, null ).Cast(); }
				else if ( info.ButtonID == 321 ){ new Elemental_Rune_Spell( from, null ).Cast(); }
				else if ( info.ButtonID == 322 ){ new Elemental_Storm_Spell( from, null ).Cast(); }
				else if ( info.ButtonID == 323 ){ new Elemental_Summon_Spell( from, null ).Cast(); }
				else if ( info.ButtonID == 324 ){ new Elemental_Devastation_Spell( from, null ).Cast(); }
				else if ( info.ButtonID == 325 ){ new Elemental_Fall_Spell( from, null ).Cast(); }
				else if ( info.ButtonID == 326 ){ new Elemental_Gate_Spell( from, null ).Cast(); }
				else if ( info.ButtonID == 327 ){ new Elemental_Havoc_Spell( from, null ).Cast(); }
				else if ( info.ButtonID == 328 ){ new Elemental_Apocalypse_Spell( from, null ).Cast(); }
				else if ( info.ButtonID == 329 ){ new Elemental_Lord_Spell( from, null ).Cast(); }
				else if ( info.ButtonID == 330 ){ new Elemental_Soul_Spell( from, null ).Cast(); }
				else if ( info.ButtonID == 331 ){ new Elemental_Spirit_Spell( from, null ).Cast(); }

				from.SendGump( new ElementalSpellbookGump( from, m_Book, m_Book.EllyPage ) );
			}
		}
	}

	public class ElementalSpellHelp : Gump 
	{
		private ElementalSpellbook m_Book; 

		public ElementalSpellHelp( Mobile from, ElementalSpellbook book, int page ) : base( 300, 200 ) 
		{
			m_Book = book; 

			from.SendSound( 0x55 );

            this.Closable=true;
			this.Disposable=true;
			this.Dragable=true;
			this.Resizable=false;

			AddPage(0);

			int img = 11143;
			int pic = 11148;
			string cat = "EARTH";
			string fnt = ElementalSpell.FontColor( m_Book.ItemID );

			if ( m_Book.ItemID == 0x6713 ) // EARTH
			{
				img = 11143;
				pic = 11148;
				cat = "EARTH";

			}
			else if ( m_Book.ItemID == 0x6715 ) // WATER
			{
				img = 11144;
				pic = 11149;
				cat = "WATER";
			}
			else if ( m_Book.ItemID == 0x6717 ) // AIR
			{
				img = 11145;
				pic = 11150;
				cat = "AIR";
			}
			else if ( m_Book.ItemID == 0x6719 ) // FIRE
			{
				img = 11146;
				pic = 11151;
				cat = "FIRE";
			}

			int btn = 4005;
			int pge = 2;
				if ( page == 2 ){ btn = 4014; pge = 1; }


			AddImage(0, 0, 7041, Server.Misc.PlayerSettings.GetGumpHue( from ));
			AddButton(608, 9, 4017, 4017, 0, GumpButtonType.Reply, 0);
			AddButton(567, 10, btn, btn, pge, GumpButtonType.Reply, 0);
			AddHtml( 12, 12, 530, 20, @"<BODY><BASEFONT Color=" + fnt + ">" + cat + " ELEMENTAL MAGIC</BASEFONT></BODY>", (bool)false, (bool)false);

			if ( page == 2 )
			{
				AddImage(14, 326, pic);
				AddHtml( 181, 118, 451, 360, @"<BODY><BASEFONT Color=" + fnt + ">[EArmor<BR>    Cast Elemental Armor<BR><BR>[EBolt<BR>    Cast Elemental Bolt<BR><BR>[EMend<BR>    Cast Elemental Mend<BR><BR>[ESanctuary<BR>    Cast Elemental Sanctuary<BR><BR>[EPain<BR>    Cast Elemental Pain<BR><BR>[EProtection<BR>    Cast Elemental Protection<BR><BR>[EPurge<BR>    Cast Elemental Purge<BR><BR>[ESteed<BR>    Cast Elemental Steed<BR><BR>[ECall<BR>    Cast Elemental Call<BR><BR>[EForce<BR>    Cast Elemental Force<BR><BR>[EWall<BR>    Cast Elemental Wall<BR><BR>[EWarp<BR>    Cast Elemental Warp<BR><BR>[EField<BR>    Cast Elemental Field<BR><BR>[ERestoration<BR>    Cast Elemental Restoration<BR><BR>[EStrike<BR>    Cast Elemental Strike<BR><BR>[EVoid<BR>    Cast Elemental Void<BR><BR>[EBlast<BR>    Cast Elemental Blast<BR><BR>[EEcho<BR>    Cast Elemental Echo<BR><BR>[EFiend<BR>    Cast Elemental Fiend<BR><BR>[EHold<BR>    Cast Elemental Hold<BR><BR>[EBarrage<BR>    Cast Elemental Barrage<BR><BR>[ERune<BR>    Cast Elemental Rune<BR><BR>[EStorm<BR>    Cast Elemental Storm<BR><BR>[ESummon<BR>    Cast Elemental Summon<BR><BR>[EDevastation<BR>    Cast Elemental Devastation<BR><BR>[EFall<BR>    Cast Elemental Fall<BR><BR>[EGate<BR>    Cast Elemental Gate<BR><BR>[EHavoc<BR>    Cast Elemental Havoc<BR><BR>[EApocalypse<BR>    Cast Elemental Apocalypse<BR><BR>[ELord<BR>    Cast Elemental Lord<BR><BR>[ESoul<BR>    Cast Elemental Soul<BR><BR>[ESpirit<BR>    Cast Elemental Spirit<BR><BR><BR><BR></BASEFONT></BODY>", (bool)false, (bool)true);
				AddHtml( 12, 44, 623, 60, @"<BODY><BASEFONT Color=" + fnt + ">Abaixo estão os [comandos que você pode digitar para lançar rapidamente um feitiço específico, ou configurar uma tecla de atalho para emitir este comando e lançar o feitiço.</BASEFONT></BODY>", (bool)false, (bool)false);
			}
			else
			{
				AddImage(14, 365, img);
				string lowreg = "Se você conseguir encontrar itens mágicos que tenham uma qualidade de reagente reduzido, então o vigor necessário para os feitiços será reduzido proporcionalmente por esse valor. ";
					if ( MyServerSettings.LowerReg() < 1 )
						lowreg = "";

				AddHtml( 118, 47, 516, 433, @"<BODY><BASEFONT Color=" + fnt + ">Diferente de outras formas de magia, o elementarismo depende tanto do intelecto quanto da condição física do conjurador. Não são necessários reagentes, mas lançar esses feitiços requer um uso igual de mana e vigor, referido como 'poder'. " + lowreg + "Não há habilidades suplementares necessárias para elementalistas, como magos com sua psicologia ou necromantes com seu espiritualismo. Há uma guilda na qual você pode se juntar no Liceu, que está escondido nas montanhas da Guarda do Diabo, com um feitiço ilusório. Para chegar lá, você só precisa lançar o feitiço Santuário. Este feitiço não pode ser usado em todos os lugares, mas conforme você aprende mais sobre elementarismo, poderá lançá-lo em lugares como masmorras para chegar rapidamente à segurança.<BR><BR>Se você visitar o Liceu, há quatro santuários para os elementos da Terra, Ar, Água e Fogo. Cada elementalista só pode se concentrar em um elemento por vez, e pode mudar seu foco quando quiser. Para fazer isso, basta subir no santuário de sua escolha. Se quiser mudar suas roupas para combinar com o elemento focado, simplesmente diga a palavra 'culara'. Mudar o foco não afeta sua biblioteca de feitiços. Isso significa que se você tem 10 feitiços em seu Livro de Feitiços do Elemental da Terra, e muda seu foco para Magia do Elemental da Água, então seu Livro de Feitiços do Elemental da Água alterado terá os mesmos 10 tipos de feitiços.<BR><BR>A única maneira de escrever feitiços elementais em pergaminhos é escrevendo as palavras mágicas com sangue de demônio, e lançar feitiços pode ser um processo tedioso, mas não precisa ser. Há um segredo que poucos elementalistas conhecem. Você pode ter até 2 barras de feitiços personalizadas que pode usar para lançar. Essas barras de feitiços mantêm seus feitiços favoritos à mão e permitem lançá-los mais rapidamente. Para aprender os comandos para acessar esses segredos, você pode encontrá-los usando o botão 'Ajuda' no paperdoll.<BR><BR>Dizem que esta magia foi forjada pelos Titãs dos Elementos, e muitos elementalistas tentam encontrar o caminho para o Submundo para procurá-los. O que eles desejam são os livros de feitiços dos Titãs, pois são muito poderosos. A maioria dos magos e elementalistas têm seus lançamentos de feitiços frequentemente falhando no Submundo. Elementalistas acreditam que equipar um dos livros de feitiços dos Titãs lhes permitiria explorar o reino sombrio sem esse obstáculo. Se verdade ou falsidade, só se pode tentar.<BR><BR>Um aviso sobre praticar outras formas de magia: magia e necromancia não podem ser concedidas ao mesmo elementalista. Conhecer essas outras formas de magia fará com que qualquer um dos feitiços falhe ao tentar lançá-los. Isso inclui ter itens que aprimorem esses tipos de habilidades. Um conjurador aspirante deve escolher um caminho e seguir em direção a esse objetivo. Você pode buscar o elementarismo, ou aprender sobre magia e necromancia. O conhecimento elementarismo até afeta as forças de varinhas mágicas ou magia rúnica. A busca pela pesquisa de magia também é algo que elementalistas não podem alcançar. Se você perceber que está perdendo o interesse em uma dessas escolas de magia e deseja rapidamente seguir a outra, procure a Caverna do Feiticeiro em Sosaria ou a Caverna do Conjurador em Lodoria. Eles têm cristais descobertos séculos atrás, que podem ajudá-lo a esquecer o que aprendeu em uma área de magia para poder aprender outra.<BR><BR>Por fim, a magia elemental é muito sensível às forças que cercam o conjurador. Quanto mais armadura você estiver usando, maior a probabilidade de seus feitiços falharem. Você deve evitar usar armadura ou encontrar armadura adequada para conjuradores, que talvez tenham qualidade de armadura de mago. Escudos com forças de canalização de feitiços também ajudam. Armadura de metal é a mais intrusiva nesse aspecto, enquanto armadura de madeira é menos prejudicial. Armadura de couro é a menos impactante, mas encontrar roupas de qualidade é o curso desejado.</BASEFONT></BODY>", (bool)false, (bool)true);
			}
		}

		public override void OnResponse( NetState state, RelayInfo info ) 
		{
			Mobile from = state.Mobile; 
			from.SendSound( 0x55 );

			if ( info.ButtonID > 0 )
			{
				int page = info.ButtonID;
				if ( page < 1 ){ page = 37; }
				if ( page > 37 ){ page = 1; }
				from.SendGump( new ElementalSpellHelp( from, m_Book, page ) );
			}
		}
	}
}
