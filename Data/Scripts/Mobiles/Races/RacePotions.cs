using System;
using Server; 
using System.Collections;
using Server.ContextMenus;
using System.Collections.Generic;
using Server.Misc;
using Server.Network;
using Server.Items;
using Server.Gumps;
using Server.Mobiles;
using Server.Commands;
using System.Globalization;
using Server.Regions;

namespace Server.Items
{
	public class RacePotions : Item
	{
		[Constructable]
		public RacePotions() : base( 0x506C )
		{
			Weight = 1.0;
			Name = "gypsy potion shelf";
			Hue = 0xABE;
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( from.InRange( this.GetWorldLocation(), 4 ) && MyServerSettings.MonstersAllowed() )
			{
				if ( from.RaceSection < 1 ){ from.RaceSection = 1; }
				from.CloseGump( typeof( GypsyTarotGump ) );
				from.CloseGump( typeof( WelcomeGump ) );
				from.CloseGump( typeof( RacePotionsGump ) );
				from.SendGump( new RacePotionsGump( from, 0 ) );
				from.PlaySound( 0x02F );
			}
		}

		public RacePotions( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int)1 ); 
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}

		public class RacePotionsGump : Gump
		{
			private int m_Tavern; 

			public RacePotionsGump( Mobile from, int tavern ): base( 50, 50 )
			{
				m_Tavern = tavern;
				string color = "#c09b88";
				int page = from.RaceSection;

				string species = "";
				if ( tavern > 0 )
				{
					if ( from.FindItemOnLayer( Layer.Special ) != null )
					{
						if ( from.FindItemOnLayer( Layer.Special ) is BaseRace )
						{
							BaseRace info = (BaseRace)(from.FindItemOnLayer( Layer.Special ));
							species = info.SpeciesFamily;
						}
					}
				}

				this.Closable=true;
				this.Disposable=true;
				this.Dragable=true;
				this.Resizable=false;

				AddPage(0);

				int x = 194;
				int y = 337;
				int p = 175;

				int race = 79999+page;
				int btn = 0;

				int next = page+1;
				bool nxt = false;
				if ( page == p ){ next = 1; }
				while ( !nxt )
				{
					if ( next == 1 )
						nxt = true;
					else if ( BaseRace.GetMonsterSize( (79999+next), species, from.RaceID, tavern ) )
						nxt = true;
					else
						next++;

					if ( next == p+1 ){ next = 1; nxt = true; }
				}
				int prev = page-1;
				bool prv = false;
				if ( page == 1 ){ prev = p; }
				while ( !prv )
				{
					if ( prev == 1 )
						prv = true;
					else if ( BaseRace.GetMonsterSize( (79999+prev), species, from.RaceID, tavern) )
						prv = true;
					else
						prev--;
				}

				string titleG = "GYPSY POTION SHELF";

				if ( tavern > 0 )
				{
					AddImage(0, 0, 2612, Server.Misc.PlayerSettings.GetGumpHue( from ));
					titleG = "CREATURE APPEARANCE";
					AddButton(736, 10, 4017, 4017, 0, GumpButtonType.Reply, 0);	// CLOSE
				}
				else
				{
					AddImage(0, 0, 2613, Server.Misc.PlayerSettings.GetGumpHue( from ));
					AddImage(387, 0, 2613, Server.Misc.PlayerSettings.GetGumpHue( from ));
					AddImage(774, 0, 2613, Server.Misc.PlayerSettings.GetGumpHue( from ));
					AddButton(10, 10, 3610, 3610, 9999, GumpButtonType.Reply, 0);	// HELP

					int g = 0;
					int gc = 0;
					int gx = 804;
					int gy = 56;
					int gm = 30;
					int go = MySettings.S_MonsterCharacters;
					int gb = 6000;

					AddHtml( 786, 11, 213, 20, @"<BODY><BASEFONT Color=" + color + ">CATEGORIES</BASEFONT></BODY>", (bool)false, (bool)false);

					gc++; AddButton(gx, gy, 2447, 2447, 123456789, GumpButtonType.Reply, 0); AddHtml( gx+18, gy-4, 98, 20, @"<BODY><BASEFONT Color=" + color + ">Human</BASEFONT></BODY>", (bool)false, (bool)false); gy=gy+gm;

					while ( g < 45 )
					{
						g++;

						if ( gc == 18 || gc == 36 ){ gx=gx+126; gy=56; }

						if ( g == 1 && go >= 1 ){ gc++; AddButton(gx, gy, 2447, 2447, g+gb, GumpButtonType.Reply, 0); AddHtml( gx+18, gy-4, 98, 20, @"<BODY><BASEFONT Color=" + color + ">Aquatic</BASEFONT></BODY>", (bool)false, (bool)false); gy=gy+gm; }
						else if ( g == 2 && go >= 1 ){ gc++; AddButton(gx, gy, 2447, 2447, g+gb, GumpButtonType.Reply, 0); AddHtml( gx+18, gy-4, 98, 20, @"<BODY><BASEFONT Color=" + color + ">Bugbear</BASEFONT></BODY>", (bool)false, (bool)false); gy=gy+gm; }
						else if ( g == 3 && go >= 1 ){ gc++; AddButton(gx, gy, 2447, 2447, g+gb, GumpButtonType.Reply, 0); AddHtml( gx+18, gy-4, 98, 20, @"<BODY><BASEFONT Color=" + color + ">Centaur</BASEFONT></BODY>", (bool)false, (bool)false); gy=gy+gm; }
						else if ( g == 4 && go >= 2 ){ gc++; AddButton(gx, gy, 2447, 2447, g+gb, GumpButtonType.Reply, 0); AddHtml( gx+18, gy-4, 98, 20, @"<BODY><BASEFONT Color=" + color + ">Cyclops</BASEFONT></BODY>", (bool)false, (bool)false); gy=gy+gm; }
						else if ( g == 5 && go >= 2 ){ gc++; AddButton(gx, gy, 2447, 2447, g+gb, GumpButtonType.Reply, 0); AddHtml( gx+18, gy-4, 98, 20, @"<BODY><BASEFONT Color=" + color + ">Daemon</BASEFONT></BODY>", (bool)false, (bool)false); gy=gy+gm; }
						else if ( g == 6 && go >= 2 ){ gc++; AddButton(gx, gy, 2447, 2447, g+gb, GumpButtonType.Reply, 0); AddHtml( gx+18, gy-4, 98, 20, @"<BODY><BASEFONT Color=" + color + ">Dagon</BASEFONT></BODY>", (bool)false, (bool)false); gy=gy+gm; }
						else if ( g == 7 && go >= 1 ){ gc++; AddButton(gx, gy, 2447, 2447, g+gb, GumpButtonType.Reply, 0); AddHtml( gx+18, gy-4, 98, 20, @"<BODY><BASEFONT Color=" + color + ">Demon</BASEFONT></BODY>", (bool)false, (bool)false); gy=gy+gm; }
						else if ( g == 8 && go >= 3 ){ gc++; AddButton(gx, gy, 2447, 2447, g+gb, GumpButtonType.Reply, 0); AddHtml( gx+18, gy-4, 98, 20, @"<BODY><BASEFONT Color=" + color + ">Devil</BASEFONT></BODY>", (bool)false, (bool)false); gy=gy+gm; }
						else if ( g == 9 && go >= 2 ){ gc++; AddButton(gx, gy, 2447, 2447, g+gb, GumpButtonType.Reply, 0); AddHtml( gx+18, gy-4, 98, 20, @"<BODY><BASEFONT Color=" + color + ">Dragon</BASEFONT></BODY>", (bool)false, (bool)false); gy=gy+gm; }
						else if ( g == 10 && go >= 1 ){ gc++; AddButton(gx, gy, 2447, 2447, g+gb, GumpButtonType.Reply, 0); AddHtml( gx+18, gy-4, 98, 20, @"<BODY><BASEFONT Color=" + color + ">Drakkul</BASEFONT></BODY>", (bool)false, (bool)false); gy=gy+gm; }
						else if ( g == 11 && go >= 2 ){ gc++; AddButton(gx, gy, 2447, 2447, g+gb, GumpButtonType.Reply, 0); AddHtml( gx+18, gy-4, 98, 20, @"<BODY><BASEFONT Color=" + color + ">Ettin</BASEFONT></BODY>", (bool)false, (bool)false); gy=gy+gm; }
						else if ( g == 12 && go >= 1 ){ gc++; AddButton(gx, gy, 2447, 2447, g+gb, GumpButtonType.Reply, 0); AddHtml( gx+18, gy-4, 98, 20, @"<BODY><BASEFONT Color=" + color + ">Fey</BASEFONT></BODY>", (bool)false, (bool)false); gy=gy+gm; }
						else if ( g == 13 && go >= 1 ){ gc++; AddButton(gx, gy, 2447, 2447, g+gb, GumpButtonType.Reply, 0); AddHtml( gx+18, gy-4, 98, 20, @"<BODY><BASEFONT Color=" + color + ">Gargoyle</BASEFONT></BODY>", (bool)false, (bool)false); gy=gy+gm; }
						else if ( g == 14 && go >= 3 ){ gc++; AddButton(gx, gy, 2447, 2447, g+gb, GumpButtonType.Reply, 0); AddHtml( gx+18, gy-4, 98, 20, @"<BODY><BASEFONT Color=" + color + ">Giant</BASEFONT></BODY>", (bool)false, (bool)false); gy=gy+gm; }
						else if ( g == 15 && go >= 1 ){ gc++; AddButton(gx, gy, 2447, 2447, g+gb, GumpButtonType.Reply, 0); AddHtml( gx+18, gy-4, 98, 20, @"<BODY><BASEFONT Color=" + color + ">Gnoll</BASEFONT></BODY>", (bool)false, (bool)false); gy=gy+gm; }
						else if ( g == 16 && go >= 1 ){ gc++; AddButton(gx, gy, 2447, 2447, g+gb, GumpButtonType.Reply, 0); AddHtml( gx+18, gy-4, 98, 20, @"<BODY><BASEFONT Color=" + color + ">Goblin</BASEFONT></BODY>", (bool)false, (bool)false); gy=gy+gm; }
						else if ( g == 17 && go >= 1 ){ gc++; AddButton(gx, gy, 2447, 2447, g+gb, GumpButtonType.Reply, 0); AddHtml( gx+18, gy-4, 98, 20, @"<BODY><BASEFONT Color=" + color + ">Golem</BASEFONT></BODY>", (bool)false, (bool)false); gy=gy+gm; }
						else if ( g == 18 && go >= 1 ){ gc++; AddButton(gx, gy, 2447, 2447, g+gb, GumpButtonType.Reply, 0); AddHtml( gx+18, gy-4, 98, 20, @"<BODY><BASEFONT Color=" + color + ">Hobgoblin</BASEFONT></BODY>", (bool)false, (bool)false); gy=gy+gm; }
						else if ( g == 19 && go >= 1 ){ gc++; AddButton(gx, gy, 2447, 2447, g+gb, GumpButtonType.Reply, 0); AddHtml( gx+18, gy-4, 98, 20, @"<BODY><BASEFONT Color=" + color + ">Illithid</BASEFONT></BODY>", (bool)false, (bool)false); gy=gy+gm; }
						else if ( g == 20 && go >= 1 ){ gc++; AddButton(gx, gy, 2447, 2447, g+gb, GumpButtonType.Reply, 0); AddHtml( gx+18, gy-4, 98, 20, @"<BODY><BASEFONT Color=" + color + ">Imp</BASEFONT></BODY>", (bool)false, (bool)false); gy=gy+gm; }
						else if ( g == 21 && go >= 1 ){ gc++; AddButton(gx, gy, 2447, 2447, g+gb, GumpButtonType.Reply, 0); AddHtml( gx+18, gy-4, 98, 20, @"<BODY><BASEFONT Color=" + color + ">Kilrathi</BASEFONT></BODY>", (bool)false, (bool)false); gy=gy+gm; }
						else if ( g == 22 && go >= 1 ){ gc++; AddButton(gx, gy, 2447, 2447, g+gb, GumpButtonType.Reply, 0); AddHtml( gx+18, gy-4, 98, 20, @"<BODY><BASEFONT Color=" + color + ">Kobold</BASEFONT></BODY>", (bool)false, (bool)false); gy=gy+gm; }
						else if ( g == 23 && go >= 1 ){ gc++; AddButton(gx, gy, 2447, 2447, g+gb, GumpButtonType.Reply, 0); AddHtml( gx+18, gy-4, 98, 20, @"<BODY><BASEFONT Color=" + color + ">Minotaur</BASEFONT></BODY>", (bool)false, (bool)false); gy=gy+gm; }
						else if ( g == 24 && go >= 1 ){ gc++; AddButton(gx, gy, 2447, 2447, g+gb, GumpButtonType.Reply, 0); AddHtml( gx+18, gy-4, 98, 20, @"<BODY><BASEFONT Color=" + color + ">Mummy</BASEFONT></BODY>", (bool)false, (bool)false); gy=gy+gm; }
						else if ( g == 25 && go >= 1 ){ gc++; AddButton(gx, gy, 2447, 2447, g+gb, GumpButtonType.Reply, 0); AddHtml( gx+18, gy-4, 98, 20, @"<BODY><BASEFONT Color=" + color + ">Mushroom</BASEFONT></BODY>", (bool)false, (bool)false); gy=gy+gm; }
						else if ( g == 26 && go >= 1 ){ gc++; AddButton(gx, gy, 2447, 2447, g+gb, GumpButtonType.Reply, 0); AddHtml( gx+18, gy-4, 98, 20, @"<BODY><BASEFONT Color=" + color + ">Naga</BASEFONT></BODY>", (bool)false, (bool)false); gy=gy+gm; }
						else if ( g == 27 && go >= 1 ){ gc++; AddButton(gx, gy, 2447, 2447, g+gb, GumpButtonType.Reply, 0); AddHtml( gx+18, gy-4, 98, 20, @"<BODY><BASEFONT Color=" + color + ">Ogre</BASEFONT></BODY>", (bool)false, (bool)false); gy=gy+gm; }
						else if ( g == 28 && go >= 1 ){ gc++; AddButton(gx, gy, 2447, 2447, g+gb, GumpButtonType.Reply, 0); AddHtml( gx+18, gy-4, 98, 20, @"<BODY><BASEFONT Color=" + color + ">Orc</BASEFONT></BODY>", (bool)false, (bool)false); gy=gy+gm; }
						else if ( g == 29 && go >= 2 ){ gc++; AddButton(gx, gy, 2447, 2447, g+gb, GumpButtonType.Reply, 0); AddHtml( gx+18, gy-4, 98, 20, @"<BODY><BASEFONT Color=" + color + ">Owlbear</BASEFONT></BODY>", (bool)false, (bool)false); gy=gy+gm; }
						else if ( g == 30 && go >= 1 ){ gc++; AddButton(gx, gy, 2447, 2447, g+gb, GumpButtonType.Reply, 0); AddHtml( gx+18, gy-4, 98, 20, @"<BODY><BASEFONT Color=" + color + ">Plant</BASEFONT></BODY>", (bool)false, (bool)false); gy=gy+gm; }
						else if ( g == 31 && go >= 1 ){ gc++; AddButton(gx, gy, 2447, 2447, g+gb, GumpButtonType.Reply, 0); AddHtml( gx+18, gy-4, 98, 20, @"<BODY><BASEFONT Color=" + color + ">Reptilian</BASEFONT></BODY>", (bool)false, (bool)false); gy=gy+gm; }
						else if ( g == 32 && go >= 2 ){ gc++; AddButton(gx, gy, 2447, 2447, g+gb, GumpButtonType.Reply, 0); AddHtml( gx+18, gy-4, 98, 20, @"<BODY><BASEFONT Color=" + color + ">Revenant</BASEFONT></BODY>", (bool)false, (bool)false); gy=gy+gm; }
						else if ( g == 33 && go >= 1 ){ gc++; AddButton(gx, gy, 2447, 2447, g+gb, GumpButtonType.Reply, 0); AddHtml( gx+18, gy-4, 98, 20, @"<BODY><BASEFONT Color=" + color + ">Rodent</BASEFONT></BODY>", (bool)false, (bool)false); gy=gy+gm; }
						else if ( g == 34 && go >= 1 ){ gc++; AddButton(gx, gy, 2447, 2447, g+gb, GumpButtonType.Reply, 0); AddHtml( gx+18, gy-4, 98, 20, @"<BODY><BASEFONT Color=" + color + ">Salamander</BASEFONT></BODY>", (bool)false, (bool)false); gy=gy+gm; }
						else if ( g == 35 && go >= 1 ){ gc++; AddButton(gx, gy, 2447, 2447, g+gb, GumpButtonType.Reply, 0); AddHtml( gx+18, gy-4, 98, 20, @"<BODY><BASEFONT Color=" + color + ">Satyr</BASEFONT></BODY>", (bool)false, (bool)false); gy=gy+gm; }
						else if ( g == 36 && go >= 1 ){ gc++; AddButton(gx, gy, 2447, 2447, g+gb, GumpButtonType.Reply, 0); AddHtml( gx+18, gy-4, 98, 20, @"<BODY><BASEFONT Color=" + color + ">Serpent</BASEFONT></BODY>", (bool)false, (bool)false); gy=gy+gm; }
						else if ( g == 37 && go >= 1 ){ gc++; AddButton(gx, gy, 2447, 2447, g+gb, GumpButtonType.Reply, 0); AddHtml( gx+18, gy-4, 98, 20, @"<BODY><BASEFONT Color=" + color + ">Skeleton</BASEFONT></BODY>", (bool)false, (bool)false); gy=gy+gm; }
						else if ( g == 38 && go >= 1 ){ gc++; AddButton(gx, gy, 2447, 2447, g+gb, GumpButtonType.Reply, 0); AddHtml( gx+18, gy-4, 98, 20, @"<BODY><BASEFONT Color=" + color + ">Sphinx</BASEFONT></BODY>", (bool)false, (bool)false); gy=gy+gm; }
						else if ( g == 39 && go >= 1 ){ gc++; AddButton(gx, gy, 2447, 2447, g+gb, GumpButtonType.Reply, 0); AddHtml( gx+18, gy-4, 98, 20, @"<BODY><BASEFONT Color=" + color + ">Succubus</BASEFONT></BODY>", (bool)false, (bool)false); gy=gy+gm; }
						else if ( g == 40 && go >= 2 ){ gc++; AddButton(gx, gy, 2447, 2447, g+gb, GumpButtonType.Reply, 0); AddHtml( gx+18, gy-4, 98, 20, @"<BODY><BASEFONT Color=" + color + ">Titan</BASEFONT></BODY>", (bool)false, (bool)false); gy=gy+gm; }
						else if ( g == 41 && go >= 2 ){ gc++; AddButton(gx, gy, 2447, 2447, g+gb, GumpButtonType.Reply, 0); AddHtml( gx+18, gy-4, 98, 20, @"<BODY><BASEFONT Color=" + color + ">Tree</BASEFONT></BODY>", (bool)false, (bool)false); gy=gy+gm; }
						else if ( g == 42 && go >= 1 ){ gc++; AddButton(gx, gy, 2447, 2447, g+gb, GumpButtonType.Reply, 0); AddHtml( gx+18, gy-4, 98, 20, @"<BODY><BASEFONT Color=" + color + ">Troll</BASEFONT></BODY>", (bool)false, (bool)false); gy=gy+gm; }
						else if ( g == 43 && go >= 1 ){ gc++; AddButton(gx, gy, 2447, 2447, g+gb, GumpButtonType.Reply, 0); AddHtml( gx+18, gy-4, 98, 20, @"<BODY><BASEFONT Color=" + color + ">Vampyre</BASEFONT></BODY>", (bool)false, (bool)false); gy=gy+gm; }
						else if ( g == 44 && go >= 1 ){ gc++; AddButton(gx, gy, 2447, 2447, g+gb, GumpButtonType.Reply, 0); AddHtml( gx+18, gy-4, 98, 20, @"<BODY><BASEFONT Color=" + color + ">Zombi</BASEFONT></BODY>", (bool)false, (bool)false); gy=gy+gm; }
						else if ( g == 45 && go >= 1 ){ gc++; AddButton(gx, gy, 2447, 2447, g+gb, GumpButtonType.Reply, 0); AddHtml( gx+18, gy-4, 98, 20, @"<BODY><BASEFONT Color=" + color + ">Drow</BASEFONT></BODY>", (bool)false, (bool)false); gy=gy+gm; }
					}

					AddButton(1125, 10, 4017, 4017, 0, GumpButtonType.Reply, 0);	// CLOSE
				}

				AddHtml( 128, 11, 249, 20, @"<BODY><BASEFONT Color=" + color + "><RIGHT>" + titleG + "</RIGHT></BASEFONT></BODY>", (bool)false, (bool)false);

				// MONSTER IMAGE BACKDROP
				AddImage(45, 110, 155);
				AddImage(45, 263, 155);
				AddImage(47, 113, 163);
				AddImage(47, 265, 163);

				if ( race > 80000 )
				{
					BaseRace costume = Server.Items.BaseRace.GetCostume( race );
					btn = 80000+costume.SpeciesID;

					// LEFT SIDE ---------------------------------------------------------------------------------------------------------------------------------------------------------

					string alignment = "#e0e2b7";

					if ( costume.SpeciesAlignment == "good" ){ alignment = "#97cb9a"; }
					else if ( costume.SpeciesAlignment == "evil" ){ alignment = "#d17777"; }

					AddHtml( 105, 42, 100, 20, @"<BODY><BASEFONT Color=" + color + ">Species:</BASEFONT></BODY>", (bool)false, (bool)false);
					AddHtml( 105, 77, 100, 20, @"<BODY><BASEFONT Color=" + color + ">Alignment:</BASEFONT></BODY>", (bool)false, (bool)false);
					AddHtml( 234, 42, 157, 20, @"<BODY><BASEFONT Color=" + color + ">" + MorphingTime.CapitalizeWords(costume.SpeciesFamily) + "</BASEFONT></BODY>", (bool)false, (bool)false);
					AddHtml( 234, 77, 157, 20, @"<BODY><BASEFONT Color=" + alignment + ">" + MorphingTime.CapitalizeWords(costume.SpeciesAlignment) + "</BASEFONT></BODY>", (bool)false, (bool)false);

					x = x - (int)( costume.SpeciesWide / 2 );
					y = y - (int)( costume.SpeciesHigh / 2 );
					AddImage(x, y, costume.SpeciesIcon);	// MONSTER IMAGE

					AddButton(12, 573, 4014, 4014, prev, GumpButtonType.Reply, 0);	// PREV
					AddButton(347, 573, 4005, 4005, next, GumpButtonType.Reply, 0);	// NEXT

					// RIGHT SIDE -----------------------------------------------------------------------------------------------------------------------------------------------------

					AddImage(452, 32, 2001);						// PAPERDOLL CONTAINER
					AddImage(459, 51, 50000+costume.SpeciesGump);	// MONSTER PAPERDOLL
					AddImage(453, 51, 50422);						// BACKPACK
					AddHtml( 479, 308, 200, 20, @"<BODY><BASEFONT Color=#000008><CENTER>" + (costume.Name).ToUpper() + "</CENTER></BASEFONT></BODY>", (bool)false, (bool)false);
					AddButton(653, 254, 4023, 4023, btn, GumpButtonType.Reply, 0);	// OK BUTTON
					AddHtml( 403, 372, 356, 217, @"<BODY><BASEFONT Color=" + color + ">" + Server.Items.BaseRace.GetAbilities( btn-80000 ) + "</BASEFONT></BODY>", (bool)false, (bool)false);

					costume.Delete();
				}
				else
				{
					// LEFT SIDE ---------------------------------------------------------------------------------------------------------------------------------------------------------

					string alignment = "#e0e2b7";

					AddHtml( 105, 42, 100, 20, @"<BODY><BASEFONT Color=" + color + ">Espécie:</BASEFONT></BODY>", (bool)false, (bool)false);
					AddHtml( 105, 77, 100, 20, @"<BODY><BASEFONT Color=" + color + ">Alinhamento:</BASEFONT></BODY>", (bool)false, (bool)false);
					AddHtml( 234, 42, 157, 20, @"<BODY><BASEFONT Color=" + color + ">Humano</BASEFONT></BODY>", (bool)false, (bool)false);
					AddHtml( 234, 77, 157, 20, @"<BODY><BASEFONT Color=" + alignment + ">Neutro</BASEFONT></BODY>", (bool)false, (bool)false);

					x = x - 15;
					y = y - 29;
					AddImage(x, y, 2818);	

					AddButton(12, 573, 4014, 4014, prev, GumpButtonType.Reply, 0);	
					AddButton(347, 573, 4005, 4005, next, GumpButtonType.Reply, 0);	

					// LADO DIREITO -----------------------------------------------------------------------------------------------------------------------------------------------------

					AddImage(452, 32, 2001);	// CONTÊINER DO PAPERDOLL
					AddImage(459, 51, 50994);	// PAPERDOLL DO MONSTRO
					AddImage(453, 51, 50422);	// MOCHILA
					AddHtml( 479, 308, 200, 20, @"<BODY><BASEFONT Color=#000008><CENTER>HUMANO</CENTER></BASEFONT></BODY>", (bool)false, (bool)false);
					AddButton(653, 254, 4023, 4023, 1000, GumpButtonType.Reply, 0);	// BOTÃO CONFIRMAR
					AddHtml( 403, 372, 356, 217, @"<BODY><BASEFONT Color=" + color + "><BR>Humanos são a raça comum e mais frequente nestas terras.</BASEFONT></BODY>", (bool)false, (bool)false);
				}
			}

			public override void OnResponse( NetState state, RelayInfo info ) 
			{
				Mobile from = state.Mobile; 

				from.CloseGump( typeof( GypsyTarotGump ) );
				from.CloseGump( typeof( WelcomeGump ) );
				from.CloseGump( typeof( RacePotionsGump ) );

				if ( info.ButtonID == 123456789 )
				{
					from.RaceSection = 0;
					from.SendGump( new RacePotionsGump( from, m_Tavern ) );
					from.SendSound( 0x4A ); 
				}
				else if ( info.ButtonID > 6000 && info.ButtonID < 6100 )
				{
					int quick = info.ButtonID - 6000;
					int move = 0;

					if ( quick == 1 ){ move = 1; }
					else if ( quick == 2 ){ move = 6; }
					else if ( quick == 3 ){ move = 7; }
					else if ( quick == 4 ){ move = 8; }
					else if ( quick == 5 ){ move = 11; }
					else if ( quick == 6 ){ move = 21; }
					else if ( quick == 7 ){ move = 23; }
					else if ( quick == 8 ){ move = 29; }
					else if ( quick == 9 ){ move = 34; }
					else if ( quick == 10 ){ move = 35; }
					else if ( quick == 11 ){ move = 43; }
					else if ( quick == 12 ){ move = 50; }
					else if ( quick == 13 ){ move = 53; }
					else if ( quick == 14 ){ move = 57; }
					else if ( quick == 15 ){ move = 70; }
					else if ( quick == 16 ){ move = 71; }
					else if ( quick == 17 ){ move = 74; }
					else if ( quick == 18 ){ move = 76; }
					else if ( quick == 19 ){ move = 77; }
					else if ( quick == 20 ){ move = 78; }
					else if ( quick == 21 ){ move = 80; }
					else if ( quick == 22 ){ move = 81; }
					else if ( quick == 23 ){ move = 84; }
					else if ( quick == 24 ){ move = 90; }
					else if ( quick == 25 ){ move = 92; }
					else if ( quick == 26 ){ move = 94; }
					else if ( quick == 27 ){ move = 97; }
					else if ( quick == 28 ){ move = 100; }
					else if ( quick == 29 ){ move = 110; }
					else if ( quick == 30 ){ move = 111; }
					else if ( quick == 31 ){ move = 113; }
					else if ( quick == 32 ){ move = 121; }
					else if ( quick == 33 ){ move = 122; }
					else if ( quick == 34 ){ move = 129; }
					else if ( quick == 35 ){ move = 130; }
					else if ( quick == 36 ){ move = 131; }
					else if ( quick == 37 ){ move = 138; }
					else if ( quick == 38 ){ move = 151; if ( MySettings.S_MonsterCharacters > 1 ){ move = 150; } }
					else if ( quick == 39 ){ move = 152; }
					else if ( quick == 40 ){ move = 155; }
					else if ( quick == 41 ){ move = 38; }
					else if ( quick == 42 ){ move = 157; }
					else if ( quick == 43 ){ move = 163; }
					else if ( quick == 44 ){ move = 166; }
					else if ( quick == 45 ){ move = 173; }

					from.RaceSection = move+1;
					from.SendGump( new RacePotionsGump( from, m_Tavern ) );
					from.SendSound( 0x4A ); 
				}
				else if ( info.ButtonID == 9999 )
				{
					from.SendGump( new RacePotionsGump( from, m_Tavern ) );
					from.SendGump( new CreatureHelpGump( from, m_Tavern ) );
					from.SendSound( 0x4A ); 
				}
				else if ( info.ButtonID > 0 && info.ButtonID < 180 )
				{
					
					from.RaceSection = info.ButtonID;
					from.SendGump( new RacePotionsGump( from, m_Tavern ) );
					from.SendSound( 0x4A ); 
				}
				else if ( info.ButtonID == 1000 )
				{
					if ( from.RaceID > 0 )
					{
						BaseRace.BackToHuman( from );
					}
					else
					{
						from.SendMessage("Você já está na forma humana.");
					}
					if ( m_Tavern == 0 ){ from.PlaySound( Utility.RandomList( 0x030, 0x031 ) ); }
					Effects.SendLocationParticles( EffectItem.Create( from.Location, from.Map, EffectItem.DefaultDuration ), 0x3728, 8, 20, 0, 0, 5042, 0 );
				}
				else if ( info.ButtonID > 80000 )
				{
					int race = info.ButtonID - 80000;
					BaseRace.CreateRace( from, race, true );
					if ( m_Tavern == 0 ){ from.PlaySound( Utility.RandomList( 0x030, 0x031 ) ); }
					Effects.SendLocationParticles( EffectItem.Create( from.Location, from.Map, EffectItem.DefaultDuration ), 0x3728, 8, 20, 0, 0, 5042, 0 );
				}
				else
				{
					if ( m_Tavern > 0 )
					{
						from.SendSound( 0x4A ); 
						from.SendGump( new Server.Engines.Help.HelpGump( from, 12 ) );
					}
					else
					{
						from.PlaySound( 0x02F );
					}
				}
			}
		}

		public static string RaceHelp( int origin )
		{
			string txt = "";

			if ( origin < 1 ){ txt = txt + "Esta prateleira contém muitos dos concoções que a cigana preparou, onde bebê-las irá alterar-te fisicamente para outra criatura. Podes usar as setas na base para navegar pela prateleira. Se escolheres iniciar tua jornada como uma criatura, procura nesta prateleira e encontra uma que te agrade. Quando encontrares algo de interesse, bebe a poção pressionando o botão CONFIRMAR próximo à imagem do paperdoll da criatura. Cada poção listada nota a espécie e lista quaisquer benefícios que possas ganhar ao seres essa criatura. Também verás representações dessas criaturas, pois essa será a aparência que terás pelo resto da vida desse personagem. Só poderás mudar tua aparência de criatura para outra criatura da mesma espécie, mas algumas criaturas têm apenas uma opção de aparência disponível.<BR><BR>"; }

			txt = txt + "Porque estas criaturas usam os modelos estáticos do jogo, não podes personalizar suas aparências como podes com humanos. As imagens mostradas são como tu parecerás. No entanto, podes usar todos os mesmos tipos de itens que os humanos usam. Em vez de vê-los representados em teu personagem, eles serão exibidos como ícones na janela do paperdoll do teu personagem. Podes pairar sobre esses ícones para ver a informação do item, assim como selecioná-los para arrastar e soltá-los dentro de teu inventário. Criaturas não podem montar cavalarias, então têm que encontrar outros meios para viajar rapidamente. Isto pode ser de feitiços mágicos, ou apenas um bom par de botas de caminhada.<BR><BR>";

			txt = txt + "Como criatura, podes pairar teu cursor sobre teu paperdoll para ver os atributos adicionais que tens por seres esse tipo de criatura. À medida que tua criatura progride no jogo, esses atributos flutuarão baseados no nível do teu personagem. Teu nível de personagem pode ser visto usando o botão INFO em teu paperdoll, e é uma combinação de três áreas diferentes. A primeira é o total de habilidades adquiridas, onde a segunda é o total de estatísticas ganhas como força ou destreza. A área final é tua fama e karma, que fará teu nível subir ou descer mais frequentemente. Quanto mais perto de zero tua fama e karma estiverem, menor o impacto em teu nível. <BR><BR>";

			txt = txt + "Algumas criaturas não precisam comer ou beber, e estas são normalmente criaturas como os mortos-vivos. Há outras que só precisam beber, e isso é comum em criaturas do tipo vegetal. Vampiros sofrem os efeitos da fome e sede como humanos, mas nunca podem comer e beber comidas comuns. Em vez disso, devem banquetear-se com sangue fresco que satisfaz tanto sede quanto fome simultaneamente. Alguns outros podem precisar comer cérebros frescos para sobreviver. A única maneira de obter sangue fresco ou cérebros frescos é matando pássaros, animais, humanoides ou gigantes.<BR><BR>";

			txt = txt + "Há dois caminhos iniciais comuns de aventura que se pode tomar. Podem simplesmente entrar no mundo da maneira comum, ou podem ser rotulados como criminosos assassinos. Onde personagens regulares têm a habilidade de alcançar até 1.000 pontos de habilidade, aqueles que começam como criminosos assassinos podem alcançar até 1.300 pontos. Cada criatura tem um alinhamento inerente que define o tom de sua perspectiva perante outros no mundo. Criaturas de alinhamento bom não podem iniciar sua jornada como criminosos assassinos. Criaturas neutras podem escolher ser aventureiros regulares ou criminosos assassinos. Aqueles de alinhamento mal também podem escolher qualquer um, mas evitar o caminho do criminoso assassino não te dá as boas-vindas em vilas e cidades civilizadas. Alguém que começa com alinhamento mal precisaria alcançar um título de fama e karma de admirável (2.500 ou mais de cada). Isto então assume que não tens assassinatos registrados ou ações criminosas pendentes. Cair abaixo de admirável faria cidadãos e guardas olharem para ti com temor e tentarem te expulsar. Lugares como as ilhas do porto, a Cidade Subterrânea de Umbra, a Vila de Ravendark, ou mesmo Xardok pouco se importam se és uma criatura maligna ou não. É provavelmente por isso que tais criaturas se reúnem nesses lugares.<BR><BR>";

			txt = txt + "Depois que entrares no mundo como criatura, há uma configuração que podes ajustar mais tarde. Quando acederes à seção AJUDA do paperdoll do teu personagem, podes ver esta opção aparecer na seção CONFIGURAÇÕES. Por padrão, teu personagem criatura por vezes fará sons quando magoar outros ou for magoado. Podes ligar ou desligar esses sons. Também podes aceder a esta informação novamente na seção AJUDA, usando o botão rotulado AJUDA DA CRIATURA. Portanto, toda a informação contida aqui está prontamente acessível mais tarde.<BR><BR>";

			txt = txt + "Há duas outras configurações que podes ajustar mais tarde, mas apenas em tavernas e estalagens. Estas também são acedidas pelo botão AJUDA, e depois pela seção CONFIGURAÇÕES. Uma configuração é o tipo de magia que queres que tua criatura se concentre, que só pode ser acedida em tavernas ou estalagens. Só verás esta opção se a criatura tiver alguma habilidade mágica inerente como magia ou necromancia. Mudar tua MAGIA DA CRIATURA mudará no que a criatura tem proficiência. Significando que se selecionares uma criatura com bónus para magia, alterar esta configuração poderia fazer com que essa criatura tenha instead um bónus para necromancia ou elementarismo. A outra opção é o TIPO DE CRIATURA. Isto permite-te mudar para humano para sempre, ou mudar tua aparência para outro modelo de criatura dentro da mesma espécie. Se houver apenas um modelo de aparência de criatura para tua espécie, a única opção aqui será tornar-te humano. Quando voltares a ser humano, perderás todos os atributos da raça que eras.<BR><BR>";

			txt = txt + "Este mundo tornou-se uma terra aceitante para a maioria das criaturas. Há muito tempo, raças como anões, elfos, bobbits e fuzzies viviam entre humanos. Agora podes tropeçar em cidades e ver um troll a beber na taverna, ou um minotauro a descansar por uma fogueira com seus camaradas humanos. Embora a maioria das criaturas sejam consideradas monstros assassinos para serem mortos à vista, algumas elevaram-se acima do resto e realizaram actos de heroísmo ou simplesmente convenceram outros de que não significam mal. Podes ser um ogro que livra o mundo do mal, ou um homem-lagarto praticando magia negra com feitiços necromânticos. A escolha é tua.<BR><BR>";

			if ( origin < 1 ){ txt = txt + "NOTA: O ambiente de jogo permite opções descobertas para fazer ligeiras mudanças na aparência do teu personagem, sem manobrar para longe dos modelos humanos de vestir e personalização de aparência. Itens como máscaras de orc ou elmos de osso, podem permitir-te superficialmente interpretar um orc ou um cavaleiro esquelético. Há também orbes mágicos de essência que podem mudar tuas cores de pele e cabelo para imitar as dos drow, orcs, homens-lagarto e vampiros. Novamente, estes são superficiais e apenas te dão a aparência de tal raça. Estas criaturas, no entanto, instilam um sentido fantástico de tornar-se algo diferente.<BR><BR>"; }

			return txt;
		}

		public static string RaceEquipment()
		{
			string txt = "Aqui está um exemplo de um personagem bugbear chamado Drub da Garra Selvagem. A esquerda está seu avatar no jogo, e a direita está sua imagem de paperdoll. Nota os vários ícones de equipamento à esquerda e direita da imagem do paperdoll do bugbear. Criaturas podem equipar os mesmos itens que humanos, e gerir o equipamento de maneira similar. Podes selecionar os ícones para arrastar e soltar equipamento, ou pairar sobre os ícones para obter a informação necessária do item.<BR><BR>NOTA: Quando vestes são equipadas em humanos, as túnicas de armadura e mangas/braços são cobertas pela veste. É o mesmo para criaturas, onde um ícone de veste cobrirá os dois ícones do canto inferior esquerdo da imagem do paperdoll do bugbear.";

			return txt;
		}
	}
}