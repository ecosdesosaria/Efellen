using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Server;
using Server.Commands;
using Server.ContextMenus;
using Server.Gumps;
using Server.Items;
using Server.Misc;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
	public class EpicCharacter : BasePerson
	{
		public override bool InitialInnocent{ get{ return true; } }
		public static Mobile m_Mobile;
		public static Mobile m_Giver;
		public static bool m_Pay;

		public string MyAlignment;
		public string MyItemText;
		public int MyItemHue;
		public int MyItemPower;
		public Map MyWorld;
		public int MyX;
		public int MyY;

		[CommandProperty(AccessLevel.Owner)]
		public string My_Alignment { get { return MyAlignment; } set { MyAlignment = value; InvalidateProperties(); } }

		[CommandProperty(AccessLevel.Owner)]
		public string My_ItemText { get { return MyItemText; } set { MyItemText = value; InvalidateProperties(); } }

		[CommandProperty(AccessLevel.Owner)]
		public int My_ItemHue { get { return MyItemHue; } set { MyItemHue = value; InvalidateProperties(); } }

		[CommandProperty(AccessLevel.Owner)]
		public int My_ItemPower { get { return MyItemPower; } set { MyItemPower = value; InvalidateProperties(); } }

		[CommandProperty(AccessLevel.Owner)]
		public Map My_World { get { return MyWorld; } set { MyWorld = value; InvalidateProperties(); } }

		[CommandProperty(AccessLevel.Owner)]
		public int My_X { get { return MyX; } set { MyX = value; InvalidateProperties(); } }

		[CommandProperty(AccessLevel.Owner)]
		public int My_Y { get { return MyY; } set { MyY = value; InvalidateProperties(); } }

		[Constructable]
		public EpicCharacter() : base( )
		{
			SpeechHue = Utility.RandomTalkHue();
			NameHue = 0xB0C;
			CantWalk = true;
			Name = "a stranger";
			Body = 400;
			MyItemText = "";
			MyItemHue = 0;
			MyItemPower = 200;
			MyAlignment = "neutral";
			AI = AIType.AI_Citizen;
			FightMode = FightMode.None;

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
			VirtualArmor = 100;
		}

		public override void GetContextMenuEntries( Mobile from, List<ContextMenuEntry> list ) 
		{ 
			base.GetContextMenuEntries( from, list );
			if ( !from.Blessed )
			{
				list.Add( new SpeechGumpEntry( from, this ) );
				list.Add( new GiftGumpEntry( from, this, true ) );
			}
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

		public override void OnMovement( Mobile m, Point3D oldLocation )
		{
			if ( !m.Frozen && m is PlayerMobile && InRange( m, 6 ) && this.Name == "Lord British" && m.Karma >= 0 && this.CanSee( m ) && this.InLOS( m ) )
			{
				if ( m.Hits < m.HitsMax && m is PlayerMobile )
				{
					OfferHeal( (PlayerMobile) m );
				}
			}
		}

		public virtual void OfferHeal( PlayerMobile m )
		{
			if ( m.CheckYoungHealTime() )
			{
				Say( "Você parece ter alguns ferimentos." );
				m.PlaySound( 0x1F2 );
				m.FixedEffect( 0x376A, 9, 32 );
				m.Hits = m.HitsMax;
			}
			else
			{
				Say( "Desculpe, mas estou cansado e não posso curá-lo agora." );
			}
		}

		public static void SetSpecialItemRequirement( Mobile m )
		{
			string epicName = ((PlayerMobile)m).EpicQuestName;
			int epicNumber = ((PlayerMobile)m).EpicQuestNumber;

			if ( epicName == "NEW" || epicName == "" || epicName == null )
			{
				int choice = Utility.RandomMinMax( 1, 59 );

				string KeepTrack = "_" + epicNumber.ToString() + "_";

				while ( Server.Items.SummonPrison.UsedNumberCheck( KeepTrack, choice ) == true ){ choice = Utility.RandomMinMax( 1, 59 ); }
					((PlayerMobile)m).EpicQuestName = Server.Items.SummonPrison.GetItemNeeded( choice, 3 );
					((PlayerMobile)m).EpicQuestNumber = choice;
			}
		}

		public static string GetSpecialItemRequirement( Mobile m )
		{
			return ((PlayerMobile)m).EpicQuestName;
		}

		public static void ClearSpecialItemRequirement( Mobile m )
		{
			string rare = ((PlayerMobile)m).EpicQuestName;

			ArrayList targets = new ArrayList();
			foreach ( Item item in World.Items.Values )
			if ( item is SummonItems && item.Name == rare )
			{
				if ( ((SummonItems)item).owner == m )
					targets.Add( item );
			}
			for ( int i = 0; i < targets.Count; ++i )
			{
				Item item = ( Item )targets[ i ];
				item.Delete();
			}
			((PlayerMobile)m).EpicQuestName = "NEW";
		}

		public static bool HaveSpecialItemRequirement( Mobile m )
		{
			string item = ((PlayerMobile)m).EpicQuestName;

			bool HasItem = false;

			if ( m != null && m.Backpack != null )
			{
				List<Item> list = new List<Item>();
				(m.Backpack).RecurseItems( list );
				foreach ( Item i in list )
				{
					if ( i is SummonItems && i.Name == item && ((SummonItems)i).Owner == m )
						HasItem = true;
				}
			}

			return HasItem;
		}

		public class SpeechGumpEntry : ContextMenuEntry
		{
			public SpeechGumpEntry( Mobile from, Mobile giver ) : base( 6146, 3 )
			{
				m_Mobile = from;
				m_Giver = giver;
			}

			public override void OnClick()
			{
			    if( !( m_Mobile is PlayerMobile ) )
				return;
				
				bool passTest = false;

				SetSpecialItemRequirement( m_Mobile );

				EpicCharacter ec = (EpicCharacter)m_Giver;
                if      ( ec.MyAlignment == "good"    && m_Mobile.Fame >= 4000 && m_Mobile.Karma >= 4000  ) passTest = true;
                else if ( ec.MyAlignment == "evil"    && m_Mobile.Fame >= 4000 && m_Mobile.Karma <= -4000 ) passTest = true;
                else if ( ec.MyAlignment == "neutral" && m_Mobile.Fame >= 7000                            ) passTest = true;

				PlayerMobile mobile = (PlayerMobile) m_Mobile;

				if ( ! mobile.HasGump( typeof( EpicGump ) ) )
				{
					mobile.SendGump( new EpicGump( m_Giver, m_Mobile, passTest, ec.MyAlignment ) );
				}
            }
        }

		public class GiftGumpEntry : ContextMenuEntry
		{
			public GiftGumpEntry( Mobile from, Mobile giver, bool pay ) : base( 6163, 3 )
			{
				m_Mobile = from;
				m_Giver = giver;
				m_Pay = pay;
			}

			public override void OnClick()
			{
			    if( !( m_Mobile is PlayerMobile ) )
				return;

				bool passTest = false;

				SetSpecialItemRequirement( m_Mobile );

				string merit = "bravery";

				EpicCharacter ec = (EpicCharacter)m_Giver;
                if      ( ec.MyAlignment == "good"    && m_Pay && m_Mobile.Fame >= 4000 && m_Mobile.Karma >= 4000  ) { passTest = true; merit = "valor";    }
                else if ( ec.MyAlignment == "evil"    && m_Pay && m_Mobile.Fame >= 4000 && m_Mobile.Karma <= -4000 ) { passTest = true; merit = "tenacity"; }
                else if ( ec.MyAlignment == "neutral" && m_Pay && m_Mobile.Fame >= 7000                            ) { passTest = true; }


				if ( m_Mobile.TotalGold < 5000 && m_Pay )
				{
					m_Mobile.SendMessage( m_Giver.Name + " precisa de pelo menos 5.000 moedas de ouro para construir o item para você.");
				}
				else if ( !(HaveSpecialItemRequirement( m_Mobile )) && m_Pay )
				{
					m_Mobile.SendMessage( m_Giver.Name + " precisará de um símbolo do seu " + merit + " (" + GetSpecialItemRequirement( m_Mobile ) + ").");
				}
				else if ( passTest == true || !m_Pay )
				{
					PlayerMobile mobile = (PlayerMobile) m_Mobile;
					{
						if ( ! mobile.HasGump( typeof( EpicCategoryGump ) ) )
						{
							mobile.SendGump( new EpicCategoryGump( m_Mobile, m_Giver, m_Pay ) );
						}
					}
				}
				else
				{
					m_Mobile.SendMessage( "Suas ações não lhe concedem um presente de tributo.");
				}
            }
        }
		public class EpicCategoryGump : Gump
        {
            private Mobile m_From;
            private Mobile m_NPC;
            private bool   m_Pay;

            public EpicCategoryGump( Mobile from, Mobile npc, bool pay ) : base( 100, 100 )
            {
                m_From = from;
                m_NPC  = npc;
                m_Pay  = pay;

                string color = "#cfc990";

                Closable  = true;
                Disposable = true;
                Dragable  = true;
                Resizable = false;

                AddPage( 0 );
                AddImage( 0, 0, 7055, Server.Misc.PlayerSettings.GetGumpHue( from ) );
                AddButton( 668, 9, 4017, 4017, 0, GumpButtonType.Reply, 0 );

                AddHtml( 61, 12, 579, 20,
                    "<BODY><BASEFONT Color=" + color + "><CENTER>PRESENTES DE TRIBUTO</CENTER></BASEFONT></BODY>",
                    false, false );

                int btnX   = 120;
                int labelX = 165;
                int y      = 100;
                int step   = 48;

                AddCatRow( btnX, labelX, y, 1, "Armas de Uma Mão", color ); y += step;
                AddCatRow( btnX, labelX, y, 2, "Armas de Duas Mãos", color ); y += step;
                AddCatRow( btnX, labelX, y, 3, "Armas de Longo Alcance",     color ); y += step;
                AddCatRow( btnX, labelX, y, 4, "Joias & Trinkets", color ); y += step;
                AddCatRow( btnX, labelX, y, 5, "Armaduras",              color ); y += step;
                AddCatRow( btnX, labelX, y, 6, "Vestuário",           color );
            }
			private void AddCatRow( int btnX, int labelX, int y, int id, string label, string color )
            {
                AddButton( btnX, y, 4005, 4007, id, GumpButtonType.Reply, 0 );
                AddHtml( labelX, y + 2, 300, 22,
                    "<BODY><BASEFONT Color=" + color + ">" + label + "</BASEFONT></BODY>",
                    false, false );
            }

            public override void OnResponse( NetState state, RelayInfo info )
            {
                Mobile from = state.Mobile;
                from.SendSound( 0x55 );

                Server.Items.RelicCategory cat;
                switch ( info.ButtonID )
                {
                    case 1: cat = Server.Items.RelicCategory.OneHandedWeapons; break;
                    case 2: cat = Server.Items.RelicCategory.TwoHandedWeapons; break;
                    case 3: cat = Server.Items.RelicCategory.RangedWeapons;    break;
                    case 4: cat = Server.Items.RelicCategory.JewelryTrinkets;  break;
                    case 5: cat = Server.Items.RelicCategory.Armor;            break;
                    case 6: cat = Server.Items.RelicCategory.Clothing;         break;
                    default: return;
                }

                from.CloseGump( typeof( EpicCategoryGump ) );
                from.SendGump( new EpicItemsGump( from, m_NPC, m_Pay, cat, 0 ) );
            }
        }
		public class EpicItemsGump : Gump
        {
            private Mobile                        m_From;
            private Mobile                        m_NPC;
            private bool                          m_Pay;
            private Server.Items.RelicCategory    m_Category;
            private ArrayList                     m_Entries;
            private int                           m_Page;

            private const int ItemsPerPage = 16;

            // Button IDs:
            //   0            = back to categories
            //   1            = previous page
            //   2            = next page
            //   1000 + index = select item

            public EpicItemsGump( Mobile from, Mobile npc, bool pay,
                                  Server.Items.RelicCategory cat, int page )
                : base( 100, 100 )
            {
                m_From     = from;
                m_NPC      = npc;
                m_Pay      = pay;
                m_Category = cat;
                m_Entries  = Server.Items.ManualOfItems.GetEntriesForCategory( cat );
                m_Page     = page;

                string color = "#cfc990";

                Closable  = true;
                Disposable = true;
                Dragable  = true;
                Resizable = false;

                AddPage( 0 );
                AddImage( 0, 0, 7055, Server.Misc.PlayerSettings.GetGumpHue( from ) );
                AddButton( 668, 9, 4017, 4017, 0, GumpButtonType.Reply, 0 );

                AddHtml( 61, 12, 579, 20,
                    "<BODY><BASEFONT Color=" + color + "><CENTER>" +
                    Server.Items.ManualOfItems.CategoryLabel( cat ).ToUpper() +
                    "</CENTER></BASEFONT></BODY>", false, false );

                int totalPages = ( m_Entries.Count + ItemsPerPage - 1 ) / ItemsPerPage;
                if ( totalPages < 1 ) totalPages = 1;

                if ( totalPages > 1 )
                {
                    AddButton( 9,   425, 4014, 4014, 1, GumpButtonType.Reply, 0 );
                    AddButton( 668, 425, 4005, 4005, 2, GumpButtonType.Reply, 0 );
                }

                int firstIndex = page * ItemsPerPage;
                int x, y, z, s;

                // Left column buttons
                x = 83; s = 84; z = 34;
                y = s + z;
                for ( int slot = 0; slot < 8; slot++ )
                {
                    int idx = firstIndex + slot;
                    if ( idx < m_Entries.Count )
                        AddButton( x, y, 2447, 2447, 1000 + idx, GumpButtonType.Reply, 0 );
                    y += z;
                }

                // Left column labels
                y = s - 3 + z;
                for ( int slot = 0; slot < 8; slot++ )
                {
                    int    idx   = firstIndex + slot;
                    string label = ( idx < m_Entries.Count )
                        ? ( (Server.Items.RelicEntry)m_Entries[idx] ).DisplayName : "";
                    AddHtml( x + 20, y, 155, 20,
                        "<BODY><BASEFONT Color=" + color + ">" + label + "</BASEFONT></BODY>",
                        false, false );
                    y += z;
                }

                // Right column buttons
                x = 375;
                y = s + z;
                for ( int slot = 8; slot < 16; slot++ )
                {
                    int idx = firstIndex + slot;
                    if ( idx < m_Entries.Count )
                        AddButton( x, y, 2447, 2447, 1000 + idx, GumpButtonType.Reply, 0 );
                    y += z;
                }

                // Right column labels
                y = s - 3 + z;
                for ( int slot = 8; slot < 16; slot++ )
                {
                    int    idx   = firstIndex + slot;
                    string label = ( idx < m_Entries.Count )
                        ? ( (Server.Items.RelicEntry)m_Entries[idx] ).DisplayName : "";
                    AddHtml( x + 20, y, 155, 20,
                        "<BODY><BASEFONT Color=" + color + ">" + label + "</BASEFONT></BODY>",
                        false, false );
                    y += z;
                }
            }

            public override void OnResponse( NetState state, RelayInfo info )
            {
                Mobile from = state.Mobile;
                from.SendSound( 0x55 );

                int totalPages = ( m_Entries.Count + ItemsPerPage - 1 ) / ItemsPerPage;
                if ( totalPages < 1 ) totalPages = 1;

                if ( info.ButtonID == 0 )
                {
                    from.CloseGump( typeof( EpicItemsGump ) );
                    from.SendGump( new EpicCategoryGump( from, m_NPC, m_Pay ) );
                }
                else if ( info.ButtonID == 1 )
                {
                    int prev = m_Page - 1;
                    if ( prev < 0 ) prev = totalPages - 1;
                    from.CloseGump( typeof( EpicItemsGump ) );
                    from.SendGump( new EpicItemsGump( from, m_NPC, m_Pay, m_Category, prev ) );
                }
                else if ( info.ButtonID == 2 )
                {
                    int next = m_Page + 1;
                    if ( next >= totalPages ) next = 0;
                    from.CloseGump( typeof( EpicItemsGump ) );
                    from.SendGump( new EpicItemsGump( from, m_NPC, m_Pay, m_Category, next ) );
                }
                else if ( info.ButtonID >= 1000 )
                {
                    int idx = info.ButtonID - 1000;
                    if ( idx >= 0 && idx < m_Entries.Count )
                    {
                        Server.Items.RelicEntry entry = (Server.Items.RelicEntry)m_Entries[idx];
                        from.CloseGump( typeof( EpicItemsGump ) );
                        from.SendGump( new EpicConfirmGump( from, m_NPC, m_Pay, entry, m_Category, m_Page ) );
                    }
                }
            }
        }
		public class EpicConfirmGump : Gump
        {
            private Mobile                     m_From;
            private Mobile                     m_NPC;
            private bool                       m_Pay;
            private Server.Items.RelicEntry    m_Entry;
            private Server.Items.RelicCategory m_ReturnCat;
            private int                        m_ReturnPage;

            public EpicConfirmGump( Mobile from, Mobile npc, bool pay,
                                    Server.Items.RelicEntry entry,
                                    Server.Items.RelicCategory returnCat, int returnPage )
                : base( 50, 50 )
            {
                m_From       = from;
                m_NPC        = npc;
                m_Pay        = pay;
                m_Entry      = entry;
                m_ReturnCat  = returnCat;
                m_ReturnPage = returnPage;

                string color    = "#cfc990";
                EpicCharacter ec = (EpicCharacter)npc;
                string sArty    = entry.DisplayName;
                if ( sArty == "Trinket, Symbol" ) sArty = "Talisman";
                if ( sArty == "Trinket, Idol"   ) sArty = "Talisman";
                if ( sArty == "Trinket, Totem"  ) sArty = "Talisman";
                sArty = sArty + " " + ec.MyItemText;

                Closable  = true;
                Disposable = true;
                Dragable  = true;
                Resizable = false;

                AddBackground( 0, 0, 460, 190, 9270 );

                AddHtml( 20, 18, 420, 20,
                    "<BODY><BASEFONT Color=" + color + ">You are about to claim:</BASEFONT></BODY>",
                    false, false );
                AddHtml( 20, 42, 420, 20,
                    "<BODY><BASEFONT Color=" + color + ">" + sArty + "</BASEFONT></BODY>",
                    false, false );

                if ( pay )
                {
                    AddHtml( 20, 72, 420, 20,
                        "<BODY><BASEFONT Color=" + color + ">This will cost 5,000 gold and tribute from your Fame/Karma.</BASEFONT></BODY>",
                        false, false );
                }

                // Confirm
                AddButton( 60,  148, 4005, 4007, 1, GumpButtonType.Reply, 0 );
                AddHtml( 95, 150, 80, 20,
                    "<BODY><BASEFONT Color=" + color + ">Confirm</BASEFONT></BODY>",
                    false, false );

                // Back
                AddButton( 260, 148, 4017, 4019, 0, GumpButtonType.Reply, 0 );
                AddHtml( 295, 150, 80, 20,
                    "<BODY><BASEFONT Color=" + color + ">Back</BASEFONT></BODY>",
                    false, false );
            }

            public override void OnResponse( NetState state, RelayInfo info )
            {
                Mobile        from    = state.Mobile;
                EpicCharacter tribute = (EpicCharacter)m_NPC;

                from.SendSound( 0x55 );

                if ( info.ButtonID == 0 )
                {
                    // Back to item list
                    from.CloseGump( typeof( EpicConfirmGump ) );
                    from.SendGump( new EpicItemsGump( from, m_NPC, m_Pay, m_ReturnCat, m_ReturnPage ) );
                    return;
                }

                // ---- Confirm ----
                string merit = "bravery";
                if      ( tribute.MyAlignment == "good" )    merit = "valor";
                else if ( tribute.MyAlignment == "evil" )    merit = "tenacity";

                bool passTest = false;
                Container pack = from.Backpack;

                if ( m_Pay )
                {
                    if ( from.TotalGold < 5000 )
                    {
                        from.SendMessage( m_NPC.Name + " needs at least 5,000 gold to construct the item for you." );
                        from.CloseGump( typeof( EpicConfirmGump ) );
                        from.SendGump( new EpicCategoryGump( from, m_NPC, m_Pay ) );
                        return;
                    }

                    if ( !HaveSpecialItemRequirement( from ) )
                    {
                        from.SendMessage( m_NPC.Name + " will need a symbol of your " + merit + " (" + GetSpecialItemRequirement( from ) + ")." );
                        from.CloseGump( typeof( EpicConfirmGump ) );
                        from.SendGump( new EpicCategoryGump( from, m_NPC, m_Pay ) );
                        return;
                    }

                    if ( tribute.MyAlignment == "good" && from.Fame >= 4000 && from.Karma >= 4000 )
                    {
                        from.Fame   -= 4000;
                        from.Karma  -= 4000;
                        passTest     = true;
                    }
                    else if ( tribute.MyAlignment == "evil" && from.Fame >= 4000 && from.Karma <= -4000 )
                    {
                        from.Fame   -= 4000;
                        from.Karma  += 4000;
                        passTest     = true;
                    }
                    else if ( tribute.MyAlignment == "neutral" && from.Fame >= 7000 )
                    {
                        from.Fame   -= 7000;
                        passTest     = true;
                    }

                    if ( !passTest )
                    {
                        from.SendMessage( "Your deeds do not grant you a gift of tribute." );
                        from.CloseGump( typeof( EpicConfirmGump ) );
                        return;
                    }

                    if ( !pack.ConsumeTotal( typeof( Gold ), 5000 ) )
                    {
                        from.SendMessage( m_NPC.Name + " needs at least 5,000 gold to construct the item for you." );
                        from.CloseGump( typeof( EpicConfirmGump ) );
                        return;
                    }

                    ClearSpecialItemRequirement( from );
                }

                string sArty = m_Entry.DisplayName;
                if ( sArty == "Trinket, Symbol" ) sArty = "Talisman";
                if ( sArty == "Trinket, Idol"   ) sArty = "Talisman";
                if ( sArty == "Trinket, Totem"  ) sArty = "Talisman";
                sArty = sArty + " " + tribute.MyItemText;

                Type itemType = ScriptCompiler.FindTypeByName( m_Entry.TypeName );
                if ( itemType == null )
                {
                    from.SendMessage( "An error occurred finding that item type." );
                    return;
                }

                Item reward = (Item)Activator.CreateInstance( itemType );
                int  points = tribute.MyItemPower;
                string gifter = "From " + m_NPC.Name + " " + m_NPC.Title;

                if ( reward is BaseGiftAxe )      { ((BaseGiftAxe)reward).m_Owner      = from; ((BaseGiftAxe)reward).m_Gifter      = gifter; ((BaseGiftAxe)reward).m_How      = "Tribute To"; ((BaseGiftAxe)reward).m_Points      = points; }
                if ( reward is BaseGiftRanged )   { ((BaseGiftRanged)reward).m_Owner   = from; ((BaseGiftRanged)reward).m_Gifter   = gifter; ((BaseGiftRanged)reward).m_How   = "Tribute To"; ((BaseGiftRanged)reward).m_Points   = points; }
                if ( reward is BaseGiftSpear )    { ((BaseGiftSpear)reward).m_Owner    = from; ((BaseGiftSpear)reward).m_Gifter    = gifter; ((BaseGiftSpear)reward).m_How    = "Tribute To"; ((BaseGiftSpear)reward).m_Points    = points; }
                if ( reward is BaseGiftClothing ) { ((BaseGiftClothing)reward).m_Owner = from; ((BaseGiftClothing)reward).m_Gifter = gifter; ((BaseGiftClothing)reward).m_How = "Tribute To"; ((BaseGiftClothing)reward).m_Points = points; }
                if ( reward is BaseGiftJewel )    { ((BaseGiftJewel)reward).m_Owner    = from; ((BaseGiftJewel)reward).m_Gifter    = gifter; ((BaseGiftJewel)reward).m_How    = "Tribute To"; ((BaseGiftJewel)reward).m_Points    = points; }
                if ( reward is BaseGiftArmor )    { ((BaseGiftArmor)reward).m_Owner    = from; ((BaseGiftArmor)reward).m_Gifter    = gifter; ((BaseGiftArmor)reward).m_How    = "Tribute To"; ((BaseGiftArmor)reward).m_Points    = points; }
                if ( reward is BaseGiftShield )   { ((BaseGiftShield)reward).m_Owner   = from; ((BaseGiftShield)reward).m_Gifter   = gifter; ((BaseGiftShield)reward).m_How   = "Tribute To"; ((BaseGiftShield)reward).m_Points   = points; }
                if ( reward is BaseGiftKnife )    { ((BaseGiftKnife)reward).m_Owner    = from; ((BaseGiftKnife)reward).m_Gifter    = gifter; ((BaseGiftKnife)reward).m_How    = "Tribute To"; ((BaseGiftKnife)reward).m_Points    = points; }
                if ( reward is BaseGiftBashing )  { ((BaseGiftBashing)reward).m_Owner  = from; ((BaseGiftBashing)reward).m_Gifter  = gifter; ((BaseGiftBashing)reward).m_How  = "Tribute To"; ((BaseGiftBashing)reward).m_Points  = points; }
                if ( reward is BaseGiftWhip )     { ((BaseGiftWhip)reward).m_Owner     = from; ((BaseGiftWhip)reward).m_Gifter     = gifter; ((BaseGiftWhip)reward).m_How     = "Tribute To"; ((BaseGiftWhip)reward).m_Points     = points; }
                if ( reward is BaseGiftPoleArm )  { ((BaseGiftPoleArm)reward).m_Owner  = from; ((BaseGiftPoleArm)reward).m_Gifter  = gifter; ((BaseGiftPoleArm)reward).m_How  = "Tribute To"; ((BaseGiftPoleArm)reward).m_Points  = points; }
                if ( reward is BaseGiftStaff )    { ((BaseGiftStaff)reward).m_Owner    = from; ((BaseGiftStaff)reward).m_Gifter    = gifter; ((BaseGiftStaff)reward).m_How    = "Tribute To"; ((BaseGiftStaff)reward).m_Points    = points; }
                if ( reward is BaseGiftSword )    { ((BaseGiftSword)reward).m_Owner    = from; ((BaseGiftSword)reward).m_Gifter    = gifter; ((BaseGiftSword)reward).m_How    = "Tribute To"; ((BaseGiftSword)reward).m_Points    = points; }

                reward.Name = sArty;
                reward.Hue  = tribute.MyItemHue;

                AddToItem( reward, m_NPC );

                from.AddToBackpack( reward );

                string sEntry   = "has received the " + sArty + " from " + m_NPC.Name + " " + m_NPC.Title;
                string sMessage = "You have received the " + sArty + " from " + m_NPC.Name + ".";

                LoggingFunctions.LogGenericQuest( from, sEntry );
                from.SendMessage( sMessage );
                from.PlaySound( 0x3D );

                from.CloseGump( typeof( EpicConfirmGump ) );
            }
        }
		public static void AddToItem( Item item, Mobile from )
        {
            if      ( from.Name == "Lord Draxinusom"         ) GiveGiftBonus( item,  6,  8, 34,  0,   0, 8.0, 8.0, 8.0, 0.0,  0.0, 23, 15 );
            else if ( from.Name == "the Great Earth Serpent" ) GiveGiftBonus( item, 99, 32,  0,  0,   0,10.0,10.0, 0.0, 0.0,  0.0,  8, 15 );
            else if ( from.Name == "Morphius"                ) GiveGiftBonus( item, 36, 33, 44,  0,   0, 8.0, 8.0, 8.0, 0.0,  0.0, 35,  0 );
            else if ( from.Name == "Mondain"                 ) GiveGiftBonus( item, 17, 31, 33,  0,   0, 8.0, 8.0, 8.0, 0.0,  0.0,  0,  0 );
            else if ( from.Name == "Tyball"                  ) GiveGiftBonus( item,  1, 14, 50,  0, 100, 8.0, 8.0, 8.0, 0.0, 15.0,  0,  0 );
            else if ( from.Name == "Arcadion"                ) GiveGiftBonus( item, 99,  0,  0,  0,   0,10.0, 0.0, 0.0, 0.0,  0.0,  5,  0 );
            else if ( from.Name == "Samhayne"                ) GiveGiftBonus( item, 99, 19, 12,  0,   0,10.0,10.0,10.0, 0.0,  0.0, 34,  0 );
            else if ( from.Name == "Seggallion"              ) GiveGiftBonus( item, 99, 19, 12,  0,   0,10.0,10.0,10.0, 0.0,  0.0, 34,  0 );
            else if ( from.Name == "Minax"                   ) GiveGiftBonus( item, 17, 31, 33,  0,   0, 8.0, 8.0, 8.0, 0.0,  0.0,  0,  0 );
            else if ( from.Name == "Nystal"                  ) GiveGiftBonus( item, 17, 31, 33,  0,   0, 8.0, 8.0, 8.0, 0.0,  0.0,  0,  0 );
            else if ( from.Name == "Lord British"            ) GiveGiftBonus( item, 99, 48, 38, 21,   0, 5.0, 5.0, 5.0, 5.0,  0.0, 11, 13 );
            else if ( from.Name == "Lord Blackthorne"        ) GiveGiftBonus( item, 99, 40, 25, 46,   0, 5.0, 5.0, 5.0, 5.0,  0.0,  5,  0 );
            else if ( from.Name == "Geoffrey"                ) GiveGiftBonus( item, 99, 48, 38, 21,   0, 5.0, 5.0, 5.0, 5.0,  0.0,  6, 31 );
            else if ( from.Name == "Shimazu"                 ) GiveGiftBonus( item, 99,  9, 37, 38,  48, 5.0, 5.0, 5.0, 5.0,  5.0,  0,  0 );
            else if ( from.Name == "Gorn"                    ) GiveGiftBonus( item, 99, 10, 23, 48,  52, 5.0, 5.0, 5.0, 5.0,  5.0, 27,  0 );
            else if ( from.Name == "Jaana"                   ) GiveGiftBonus( item,  2, 23, 53,  0,   0, 8.0, 8.0, 8.0, 0.0,  0.0,  1,  0 );
            else if ( from.Name == "Dupre"                   ) GiveGiftBonus( item, 99, 48, 38, 13,   0, 5.0, 5.0, 5.0, 5.0,  0.0,  1,  0 );
            else if ( from.Name == "Gwenno"                  ) GiveGiftBonus( item, 16, 35, 39, 41,   0, 5.0, 5.0, 5.0, 5.0,  0.0,  0,  0 );
            else if ( from.Name == "Iolo"                    ) GiveGiftBonus( item,100, 48, 21, 29,   0, 5.0, 5.0, 5.0, 5.0,  0.0,  0,  0 );
            else if ( from.Name == "Shamino"                 ) GiveGiftBonus( item, 99, 11, 29, 48,  52, 5.0, 5.0, 5.0, 5.0,  5.0, 33, 30 );
            else if ( from.Name == "Stefano"                 ) GiveGiftBonus( item, 15, 25, 42, 43,  45, 5.0, 5.0, 5.0, 5.0,  5.0,  0,  0 );
            else if ( from.Name == "Katrina"                 ) GiveGiftBonus( item,  3,  4, 24, 53,   0, 5.0, 5.0, 5.0, 5.0,  0.0,  0,  0 );
            else if ( from.Name == "the Guardian"            ) GiveGiftBonus( item, 99, 32,  0,  0,   0,10.0,10.0, 0.0, 0.0,  0.0,  0,  0 );
            else if ( from.Name == "Garamon"                 ) GiveGiftBonus( item,  1, 14, 50,  0, 100, 8.0, 8.0, 8.0, 0.0, 15.0,  0,  0 );
            else if ( from.Name == "Mors Gotha"              ) GiveGiftBonus( item, 99, 48, 38, 13,   0, 5.0, 5.0, 5.0, 5.0,  0.0,  5,  0 );
            else if ( from.Name == "Lethe"                   ) GiveGiftBonus( item, 22,  1, 36,  0,   0, 8.0, 8.0, 8.0, 0.0,  0.0,  0,  0 );
        }

        public static void GiveGiftBonus( Item item, int val1, int val2, int val3, int val4, int val5,
                                          double sk1, double sk2, double sk3, double sk4, double sk5,
                                          int slay1, int slay2 )
        {
            if ( item is BaseWeapon )
            {
                if ( slay1 > 0 ) ((BaseWeapon)item).Slayer  = ResourceMods.GetSlayer( slay1 );
                if ( slay2 > 0 ) ((BaseWeapon)item).Slayer2 = ResourceMods.GetSlayer( slay2 );
                if      ( val1 == 99 )                         ((BaseWeapon)item).SkillBonuses.SetValues( 0, ((BaseWeapon)item).Skill, sk1 );
                else if ( val1 == 100 && item is BaseRanged )  ((BaseWeapon)item).SkillBonuses.SetValues( 0, ResourceMods.GetSkill( 5 ), sk1 );
                else if ( val1 > 0 )                           ((BaseWeapon)item).SkillBonuses.SetValues( 0, ResourceMods.GetSkill( val1 ), sk1 );
                if ( val2 > 0 )  ((BaseWeapon)item).SkillBonuses.SetValues( 1, ResourceMods.GetSkill( val2 ), sk2 );
                if ( val3 > 0 )  ((BaseWeapon)item).SkillBonuses.SetValues( 2, ResourceMods.GetSkill( val3 ), sk3 );
                if ( val4 > 0 )  ((BaseWeapon)item).SkillBonuses.SetValues( 3, ResourceMods.GetSkill( val4 ), sk4 );
                if ( val5 == 100 ) ((BaseWeapon)item).Attributes.EnhancePotions = (int)sk5;
                else if ( val5 > 0 ) ((BaseWeapon)item).SkillBonuses.SetValues( 4, ResourceMods.GetSkill( val5 ), sk5 );
            }
            else if ( item is BaseArmor )
            {
                if      ( val1 == 99  ) { }
                else if ( val1 == 100 ) ((BaseArmor)item).SkillBonuses.SetValues( 0, ResourceMods.GetSkill( 5 ), sk1 );
                else if ( val1 > 0    ) ((BaseArmor)item).SkillBonuses.SetValues( 0, ResourceMods.GetSkill( val1 ), sk1 );
                if ( val2 > 0 )  ((BaseArmor)item).SkillBonuses.SetValues( 1, ResourceMods.GetSkill( val2 ), sk2 );
                if ( val3 > 0 )  ((BaseArmor)item).SkillBonuses.SetValues( 2, ResourceMods.GetSkill( val3 ), sk3 );
                if ( val4 > 0 )  ((BaseArmor)item).SkillBonuses.SetValues( 3, ResourceMods.GetSkill( val4 ), sk4 );
                if ( val5 == 100 ) ((BaseArmor)item).Attributes.EnhancePotions = (int)sk5;
                else if ( val5 > 0 ) ((BaseArmor)item).SkillBonuses.SetValues( 4, ResourceMods.GetSkill( val5 ), sk5 );
            }
            else if ( item is BaseClothing )
            {
                if      ( val1 == 99  ) { }
                else if ( val1 == 100 ) ((BaseClothing)item).SkillBonuses.SetValues( 0, ResourceMods.GetSkill( 5 ), sk1 );
                else if ( val1 > 0    ) ((BaseClothing)item).SkillBonuses.SetValues( 0, ResourceMods.GetSkill( val1 ), sk1 );
                if ( val2 > 0 )  ((BaseClothing)item).SkillBonuses.SetValues( 1, ResourceMods.GetSkill( val2 ), sk2 );
                if ( val3 > 0 )  ((BaseClothing)item).SkillBonuses.SetValues( 2, ResourceMods.GetSkill( val3 ), sk3 );
                if ( val4 > 0 )  ((BaseClothing)item).SkillBonuses.SetValues( 3, ResourceMods.GetSkill( val4 ), sk4 );
                if ( val5 == 100 ) ((BaseClothing)item).Attributes.EnhancePotions = (int)sk5;
                else if ( val5 > 0 ) ((BaseClothing)item).SkillBonuses.SetValues( 4, ResourceMods.GetSkill( val5 ), sk5 );
            }
            else if ( item is BaseTrinket )
            {
                if      ( val1 == 99  ) { }
                else if ( val1 == 100 ) ((BaseTrinket)item).SkillBonuses.SetValues( 0, ResourceMods.GetSkill( 5 ), sk1 );
                else if ( val1 > 0    ) ((BaseTrinket)item).SkillBonuses.SetValues( 0, ResourceMods.GetSkill( val1 ), sk1 );
                if ( val2 > 0 )  ((BaseTrinket)item).SkillBonuses.SetValues( 1, ResourceMods.GetSkill( val2 ), sk2 );
                if ( val3 > 0 )  ((BaseTrinket)item).SkillBonuses.SetValues( 2, ResourceMods.GetSkill( val3 ), sk3 );
                if ( val4 > 0 )  ((BaseTrinket)item).SkillBonuses.SetValues( 3, ResourceMods.GetSkill( val4 ), sk4 );
                if ( val5 == 100 ) ((BaseTrinket)item).Attributes.EnhancePotions = (int)sk5;
                else if ( val5 > 0 ) ((BaseTrinket)item).SkillBonuses.SetValues( 4, ResourceMods.GetSkill( val5 ), sk5 );
            }
        }
		public override bool OnDragDrop( Mobile from, Item dropped )
        {
            if (    ( dropped is Artifact_DupresCollar && this.Name == "Dupre" ) ||
                    ( dropped is Artifact_DupresShield && this.Name == "Dupre" ) ||
                    ( dropped is GwennosHarp           && this.Name == "Gwenno" ) ||
                    ( dropped is IolosLute             && this.Name == "Iolo"   ) )
            {
                this.Say( "Thank you, " + from.Name + "! I lost that this years ago." );
                from.SendSound( 0x5B4 );
                dropped.Delete();
                int gold = Utility.RandomMinMax( 5, 10 ) * 1000;
                from.AddToBackpack( new BankCheck( gold ) );
                from.SendMessage( this.Name + " gave you a check for " + gold + " gold!" );
                return true;
            }
            else if ( dropped is CourierMail && !from.Blessed )
            {
                CourierMail scroll   = (CourierMail)dropped;
                string      fullName = this.Name + " " + this.Title;

                if ( scroll.owner == from && scroll.MsgComplete > 0 && scroll.ForWho == fullName )
                {
                    string success = "has found the " + scroll.SearchItem + " for " + fullName;
                    LoggingFunctions.LogGenericQuest( from, success );

                    int karmaFame = ( scroll.MsgReward * 100 ) - 100;
                    if ( karmaFame < 100 ) karmaFame = 100;

                    Titles.AwardFame( from, karmaFame, true );

                    if      ( scroll.ForAlignment == "evil"  ) Titles.AwardKarma( from, -karmaFame, true );
                    else if ( scroll.ForAlignment == "good"  ) Titles.AwardKarma( from,  karmaFame, true );

                    int goldReward = scroll.MsgReward * 1000;
                    if ( scroll.ForAlignment == "neutral" ) goldReward = scroll.MsgReward * 1500;

                    from.AddToBackpack( new Gold( goldReward ) );

                    string sMessage = "";
                    if ( scroll.ForAlignment == "good" )
                    {
                        switch ( Utility.RandomMinMax( 0, 5 ) )
                        {
                            case 0: sMessage = "Thank you for bringing this to me.";           break;
                            case 1: sMessage = "I knew you could do it.";                      break;
                            case 2: sMessage = "This is a great help to us all.";              break;
                            case 3: sMessage = "Good work! I am glad to see you arrive well."; break;
                            case 4: sMessage = "Your valor will be remembered.";               break;
                            case 5: sMessage = "You have done what most others could not.";    break;
                        }
                    }
                    else if ( scroll.ForAlignment == "evil" )
                    {
                        switch ( Utility.RandomMinMax( 0, 5 ) )
                        {
                            case 0: sMessage = "It is good that you did not fail me.";          break;
                            case 1: sMessage = "I trust you eliminated any troubles for this?"; break;
                            case 2: sMessage = "Ahhh...another step closer to my plan.";        break;
                            case 3: sMessage = "You may prove to be useful yet.";               break;
                            case 4: sMessage = "You took long enough.";                         break;
                            case 5: sMessage = "I was about to send someone to deal with you."; break;
                        }
                    }
                    else
                    {
                        switch ( Utility.RandomMinMax( 0, 5 ) )
                        {
                            case 0: sMessage = "Hmmm...I see you found it.";          break;
                            case 1: sMessage = "I trust you had little difficulty?";  break;
                            case 2: sMessage = "Good! I thought for sure you were lost."; break;
                            case 3: sMessage = "I guess my trust was well placed.";   break;
                            case 4: sMessage = "I thought you perished in the attempt."; break;
                            case 5: sMessage = "I wasn't sure it really existed.";    break;
                        }
                    }

                    from.SendSound( 0x3D );
                    from.SendMessage( goldReward.ToString() + " gold has been added to your pack." );
                    this.PrivateOverheadMessage( MessageType.Regular, 1153, false, sMessage, from.NetState );
                    dropped.Delete();
                    return true;
                }
            }
            else if ( dropped is QuestTome && !from.Blessed )
            {
                QuestTome book     = (QuestTome)dropped;
                string    fullName = this.Name + " " + this.Title;

                if ( book.QuestTomeOwner == from &&
                     ( book.QuestTomeNPCGood == fullName || book.QuestTomeNPCEvil == fullName ) )
                {
                    string sMessage = "";
                    if ( book.QuestTomeGoals > 3 )
                    {
                        string success = "has found " + book.GoalItem4 + " for " + fullName;
                        LoggingFunctions.LogGenericQuest( from, success );

                        Titles.AwardFame( from, 1000, true );
                        from.SendSound( 0x3D );

                        if      ( this.MyAlignment == "evil" ) Titles.AwardKarma( from, -1000, true );
                        else if ( this.MyAlignment == "good" ) Titles.AwardKarma( from,  1000, true );

                        if      ( this.MyAlignment == "good" ) sMessage = "Ahhh...you found it and perhaps saved us all! Choose your reward.";
                        else if ( this.MyAlignment == "evil" ) sMessage = "Good! Everything is going to plan. Choose your reward.";

                        if ( !from.HasGump( typeof( EpicCategoryGump ) ) )
                            from.SendGump( new EpicCategoryGump( from, this, false ) );

                        dropped.Delete();
                    }
                    else
                    {
                        if      ( this.MyAlignment == "good" ) sMessage = "Return to me when you find it.";
                        else if ( this.MyAlignment == "evil" ) sMessage = "Do not fail me in this task.";
                    }

                    this.PrivateOverheadMessage( MessageType.Regular, 1153, false, sMessage, from.NetState );
                    return true;
                }
            }

            return base.OnDragDrop( from, dropped );
        }

		public override void OnAfterSpawn()
		{
			if ( this.X == 798 && this.Y == 1095 && this.Map == Map.SerpentIsland )
			{
				this.Body = Utility.RandomList(427,191); 
				this.Hue = 0;
				this.Name = "Lord Draxinusom";
				this.Title = "the Gargoyle King";
				this.MyAlignment = "neutral";
				this.Direction = Direction.East;
				this.MyItemText = "of the Gargoyles";
				this.MyItemHue = 0x846;
				this.MyWorld = this.Map;
				this.MyX = 882;
				this.MyY = 1134;
			}
			else if ( this.X == 2425 && this.Y == 627 && this.Map == Map.SerpentIsland )
			{
				this.Body = 21;
				this.Hue = 0x83F;
				this.Name = "the Great Earth Serpent";
				this.MyAlignment = "neutral";
				this.Direction = Direction.East;
				this.MyItemText = "of Balance";
				this.MyItemHue = 0x978;
				this.MyWorld = this.Map;
				this.MyX = 1453;
				this.MyY = 835;
			}
			else if ( this.X == 126 && this.Y == 2942 && this.Map == Map.SavagedEmpire )
			{
				this.Body = 24;
				this.Hue = 0x83B;
				this.Name = "Morphius";
				this.Title = "the Vile Lich";
				this.MyAlignment = "evil";
				this.Direction = Direction.South;
				this.MyItemText = "of the Necrotic";
				this.MyItemHue = 0xB9A;
				this.MyWorld = this.Map;
				this.MyX = 223;
				this.MyY = 1361;
			}
			else if ( this.X == 63 && this.Y == 2893 && this.Map == Map.SavagedEmpire )
			{
				this.Body = 400; 
				this.Hue = 0x83EA;
				this.FacialHairItemID = 0x204D;
				this.HairItemID = 0;
				this.FacialHairHue = 0x497;
				this.HairHue = 0x497;

				AddItem( new Boots() );
				Item cloth1 = new Robe();
					cloth1.Hue = 0x497;
					this.AddItem( cloth1 );

				this.Name = "Mondain";
				this.Title = "the Wizard";
				this.MyAlignment = "evil";
				this.Direction = Direction.South;
				this.MyItemText = "of Mondain";
				this.MyItemHue = 0x497;
				this.MyWorld = this.Map;
				this.MyX = 1128;
				this.MyY = 22;
			}
			else if ( this.X == 6067 && this.Y == 344 && this.Map == Map.Lodor )
			{
				this.Body = 400; 
				this.Hue = 0x83EA;
				this.FacialHairItemID = 0x2041;
				this.HairItemID = 0x203C;
				this.FacialHairHue = 0x497;
				this.HairHue = 0x497;

				AddItem( new Boots() );
				Item cloth1 = new Robe();
					cloth1.Hue = 0x845;
					this.AddItem( cloth1 );

				Item cloth2 = new WizardsHat();
					cloth2.Hue = 0x845;
					this.AddItem( cloth2 );

				this.Name = "Tyball";
				this.Title = "the Demonologist";
				this.MyAlignment = "evil";
				this.Direction = Direction.South;
				this.MyItemText = "of Demonic Souls";
				this.MyItemHue = 0x54D;
				this.MyWorld = this.Map;
				this.MyX = 1637;
				this.MyY = 2804;
			}
			else if ( this.X == 5415 && this.Y == 1160 && this.Map == Map.Lodor )
			{
				this.Body = 9; 
				this.Hue = 0x845;
				this.Name = "Arcadion";
				this.Title = "the Daemon";
				this.MyAlignment = "evil";
				this.Direction = Direction.South;
				this.MyItemText = "of Purgatory";
				this.MyItemHue = 0x550;
				this.MyWorld = this.Map;
				this.MyX = 3196;
				this.MyY = 3318;
			}
			else if ( this.X == 2142 && this.Y == 2754 && this.Map == Map.Lodor )
			{
				this.Body = 400; 
				this.Hue = 0x83EA;
				this.FacialHairItemID = 0;
				this.HairItemID = 0x203C;
				this.FacialHairHue = 0x8A5;
				this.HairHue = 0x8A5;

				AddItem( new Boots() );
				Item cloth1 = new FancyShirt();
					cloth1.Hue = Utility.RandomBlueHue();
					this.AddItem( cloth1 );

				Item cloth2 = new LongPants();
					cloth2.Hue = Utility.RandomNeutralHue();
					this.AddItem( cloth2 );

				this.Name = "Samhayne";
				this.Title = "the Master Sailor";
				this.MyAlignment = "good";
				this.Direction = Direction.East;
				this.MyItemText = "of Poseidon";
				this.MyItemHue = 0x542;
				this.MyWorld = this.Map;
				this.MyX = this.X;
				this.MyY = this.Y;
			}
			else if ( this.X == 5035 && this.Y == 3830 && this.Map == Map.Sosaria )
			{
				this.Body = 400; 
				this.Hue = 0x83EA;
				this.FacialHairItemID = 0x2041;
				this.HairItemID = 0x203B;
				this.FacialHairHue = 0x908;
				this.HairHue = 0x908;

				AddItem( new Boots() );
				Item cloth1 = new FancyShirt();
					cloth1.Hue = Utility.RandomYellowHue();
					this.AddItem( cloth1 );

				Item cloth2 = new LongPants();
					cloth2.Hue = Utility.RandomNeutralHue();
					this.AddItem( cloth2 );

				Item cloth3 = new TricorneHat();
					cloth3.Hue = Utility.RandomNeutralHue();
					this.AddItem( cloth3 );

				this.Name = "Seggallion";
				this.Title = "the Pirate Lord";
				this.MyAlignment = "evil";
				this.Direction = Direction.East;
				this.MyItemText = "of the Buccaneer";
				this.MyItemHue = 0x549;
				this.MyWorld = this.Map;
				this.MyX = 1878;
				this.MyY = 2215;
			}
			else if ( this.X == 4755 && this.Y == 3978 && this.Map == Map.Sosaria )
			{
				this.Body = 401; 
				this.Hue = 0x83EA;
				this.FacialHairItemID = 0;
				this.HairItemID = 0x203C;
				this.FacialHairHue = 0x497;
				this.HairHue = 0x497;

				AddItem( new Boots() );
				Item cloth1 = new FancyDress();
					cloth1.Hue = 0x497;
					this.AddItem( cloth1 );

				this.Name = "Minax";
				this.Title = "the Enchantress";
				this.MyAlignment = "evil";
				this.Direction = Direction.East;
				this.MyItemText = "of Minax";
				this.MyItemHue = 0x497;
				this.MyWorld = this.Map;
				this.MyX = 3832;
				this.MyY = 1494;
			}
			else if ( this.X == 3011 && this.Y == 951 && this.Map == Map.Sosaria )
			{
				this.Body = 400; 
				this.Hue = 0x83EA;
				this.FacialHairItemID = 0x204B;
				this.HairItemID = 0x203C;
				this.FacialHairHue = 0x370;
				this.HairHue = 0x370;

				AddItem( new Boots() );
				Item cloth1 = new Robe();
					cloth1.Hue = 0x907;
					this.AddItem( cloth1 );

				Item cloth2 = new WizardsHat();
					cloth2.Hue = 0x907;
					this.AddItem( cloth2 );

				this.Name = "Nystal";
				this.Title = "the Royal Wizard";
				this.MyAlignment = "good";
				this.Direction = Direction.East;
				this.MyItemText = "of Wizardry";
				this.MyItemHue = 0x48B;
				this.MyWorld = this.Map;
				this.MyX = this.X;
				this.MyY = this.Y;
			}
			else if ( this.X == 2990 && this.Y == 902 && this.Map == Map.Sosaria )
			{
				this.Body = 400; 
				this.Hue = 0x83EA;
				this.FacialHairItemID = 0x204B;
				this.HairItemID = 0x203C;
				this.FacialHairHue = 0x906;
				this.HairHue = 0x906;

				AddItem( new Boots() );

				Item cloth1 = new Robe();
					cloth1.Hue = 0xA47;
					this.AddItem( cloth1 );

				Item cloth2 = new JewelryCirclet();
					cloth2.Name = "royal crown";
					cloth2.Hue = 0x8A5;
					this.AddItem( cloth2 );

				Item cloth3 = new FancyShirt();
					cloth3.Hue = 0xA20;
					this.AddItem( cloth3 );

				this.Name = "Lord British";
				this.Title = "the King of Britain";
				this.MyAlignment = "good";
				this.Direction = Direction.South;
				this.MyItemText = "of Sosaria";
				this.MyItemHue = 0x430;
				this.MyWorld = this.Map;
				this.MyX = this.X;
				this.MyY = this.Y;
			}
			else if ( this.X == 6732 && this.Y == 1663 && this.Map == Map.Sosaria )
			{
				this.Body = 400; 

				AddItem( new Server.Items.Boots() );
				AddItem( new LordBlackthorneSuit()); 

				this.Name = "Lord Blackthorne";
				this.Title = "the Ruler of Kuldar";
				this.MyAlignment = "evil";
				this.Direction = Direction.East;
				this.MyItemText = "of Blackthorne";
				this.MyItemHue = 0x966;
				this.MyWorld = this.Map;
				this.MyX = this.X;
				this.MyY = this.Y;
			}
			else if ( this.X == 3025 && this.Y == 962 && this.Map == Map.Sosaria )
			{
				this.Body = 400; 
				this.Hue = 0x83EA;
				this.FacialHairItemID = 0x2041;
				this.HairItemID = 0;
				this.FacialHairHue = 0x45C;
				this.HairHue = 0x45C;

				AddItem( new Boots() );

				Item cloth1 = new Cloak();
					cloth1.Hue = Utility.RandomBlueHue();
					this.AddItem( cloth1 );

				Item cloth2 = new ChainChest();
					cloth2.Hue = 0x430;
					this.AddItem( cloth2 );

				Item cloth3 = new RingmailArms();
					cloth3.Hue = 0x430;
					this.AddItem( cloth3 );

				Item cloth4 = new ChainLegs();
					cloth4.Hue = 0x430;
					this.AddItem( cloth4 );

				Item cloth5 = new RingmailGloves();
					cloth5.Hue = 0x430;
					this.AddItem( cloth5 );

				Item cloth6 = new ChainCoif();
					cloth6.Hue = 0x430;
					this.AddItem( cloth6 );

				Item cloth7 = new OrderShield();
					cloth7.Hue = 0x430;
					this.AddItem( cloth7 );

				this.AddItem( new Longsword() );

				this.Name = "Geoffrey";
				this.Title = "the Knight";
				this.MyAlignment = "good";
				this.Direction = Direction.West;
				this.MyItemText = "of the Warrior";
				this.MyItemHue = 0;
				this.MyWorld = this.Map;
				this.MyX = this.X;
				this.MyY = this.Y;
			}
			else if ( this.X == 5615 && this.Y == 2888 && this.Map == Map.Sosaria )
			{
				this.Body = 400; 
				this.Hue = 0x83EA;
				this.FacialHairItemID = 0x2041;
				this.HairItemID = 0;
				this.FacialHairHue = 0x455;
				this.HairHue = 0x455;

				int ronin = 0x972;

				SamuraiTabi cloth1 = new SamuraiTabi( );
					cloth1.Hue = ronin;
					this.AddItem( cloth1 );

				LeatherHiroSode cloth2 = new LeatherHiroSode( );
					cloth2.Hue = ronin;
					this.AddItem( cloth2 );

				LeatherDo cloth3 = new LeatherDo( );
					cloth3.Hue = ronin;
					this.AddItem( cloth3 );

				Item glove = new LeatherGloves();
					glove.Hue = ronin;
					this.AddItem( glove );

				switch ( Utility.Random( 4 ) )
				{
					case 0: LightPlateJingasa cloth4 = new LightPlateJingasa( );	cloth4.Hue = ronin;	this.AddItem( cloth4 ); break;
					case 1: ChainHatsuburi cloth5 = new ChainHatsuburi( );	cloth5.Hue = ronin;	this.AddItem( cloth5 ); break;
					case 2: DecorativePlateKabuto cloth6 = new DecorativePlateKabuto( );	cloth6.Hue = ronin;	this.AddItem( cloth6 ); break;
					case 3: LeatherJingasa cloth7 = new LeatherJingasa( );	cloth7.Hue = ronin;	this.AddItem( cloth7 ); break;
				}

				switch ( Utility.Random( 3 ) )
				{
					case 0: StuddedHaidate cloth8 = new StuddedHaidate( );	cloth8.Hue = ronin;	this.AddItem( cloth8 ); break;
					case 1: LeatherSuneate cloth9 = new LeatherSuneate( );	cloth9.Hue = ronin;	this.AddItem( cloth9 ); break;
					case 2: PlateSuneate cloth0 = new PlateSuneate( );	cloth0.Hue = ronin;	this.AddItem( cloth0 ); break;
				}

				this.Name = "Shimazu";
				this.Title = "the Shogun Samurai";
				this.MyAlignment = "neutral";
				this.Direction = Direction.East;
				this.MyItemText = "of the Shogun";
				this.MyItemHue = 0;
				this.MyWorld = this.Map;
				this.MyX = 1328;
				this.MyY = 3589;
			}
			else if ( this.X == 313 && this.Y == 1040 && this.Map == Map.IslesDread )
			{
				this.Body = 0x190;
            	this.Hue = Utility.RandomSkinColor();

				this.FacialHairItemID = 0x204C; // BEARD
				this.HairItemID = 0x203C; // LONG HAIR
				this.FacialHairHue = 0x455;
				this.HairHue = 0x455;

				AddItem( new StuddedChest() );
				AddItem( new StuddedLegs() );
				AddItem( new Boots() );
				Item cloth2 = new JewelryCirclet();
					cloth2.Name = "circlet";
					cloth2.Hue = 0;
					this.AddItem( cloth2 );
				AddItem( new Cloak( Utility.RandomYellowHue() ) );
				AddItem( new StuddedGloves() );

				this.Name = "Gorn";
            	this.Title = "the King of Cimmeran";
				this.MyAlignment = "neutral";
				this.Direction = Direction.East;
				this.MyItemText = "of the Barbarian";
				this.MyItemHue = 0x972;
				this.MyWorld = this.Map;
				this.MyX = this.X;
				this.MyY = this.Y;
			}
			else if ( this.X == 2462 && this.Y == 865 && this.Map == Map.Sosaria )
			{
				this.Body = 401; 
				this.Hue = 0x83EA;
				this.FacialHairItemID = 0;
				this.HairItemID = 0x203C;
				this.FacialHairHue = 0x8A5;
				this.HairHue = 0x8A5;

				AddItem( new Boots() );
				Item cloth1 = new Robe();
					cloth1.Hue = 0x907;
					this.AddItem( cloth1 );

				this.Name = "Jaana";
				this.Title = "the Herb Healer";
				this.MyAlignment = "good";
				this.Direction = Direction.South;
				this.MyItemText = "of the Cleric";
				this.MyItemHue = 0x47E;
				this.MyWorld = this.Map;
				this.MyX = this.X;
				this.MyY = this.Y;
			}
			else if ( this.X == 1395 && this.Y == 3778 && this.Map == Map.Sosaria )
			{
				this.Name = "Dupre";
				this.MyAlignment = "good";
				this.Direction = Direction.South;
				this.Body = 400;

				AddItem( new Server.Items.Boots() );
				AddItem( new DupreSuit());

				this.Title = "the Paladin";
				this.MyItemText = "of the Paladin";
				this.MyItemHue = 0x430;
				this.MyWorld = this.Map;
				this.MyX = this.X;
				this.MyY = this.Y;
			}
			else if ( this.X == 2119 && this.Y == 247 && this.Map == Map.Sosaria )
			{
				this.Name = "Gwenno";
				this.MyAlignment = "good";
				this.Direction = Direction.South;

				this.Body = 401; 
				this.Hue = Utility.RandomSkinColor();
				this.HairItemID = 0x203C;
				this.HairHue = 0x45C;

				AddItem( new Boots() );
				Item cloth1 = new FancyDress();
					cloth1.Hue = 0x96F;
					this.AddItem( cloth1 );

				this.Title = "the Bard";
				this.MyItemText = "of the Minstrel";
				this.MyItemHue = 0;
				this.MyWorld = this.Map;
				this.MyX = this.X;
				this.MyY = this.Y;
			}
			else if ( this.X == 937 && this.Y == 2081 && this.Map == Map.Sosaria )
			{
				this.Name = "Iolo";
				this.MyAlignment = "good";
				this.Direction = Direction.South;

				this.Body = 0x190;
            	this.Hue = Utility.RandomSkinColor();

				this.FacialHairItemID = 0x204C;
				this.HairItemID = 0x2048;
				this.FacialHairHue = 0x430;
				this.HairHue = 0x430;

				AddItem( new FancyShirt( Utility.RandomYellowHue() ) );
				AddItem( new LongPants( Utility.RandomYellowHue() ) );
				AddItem( new Boots() );
				AddItem( new Crossbow() );
				AddItem( new Cloak( Utility.RandomYellowHue() ) );
				AddItem( new LeatherGloves() );

            	this.Title = "the Bowman";
				this.MyItemText = "of the Archer";
				this.MyItemHue = 0;
				this.MyWorld = this.Map;
				this.MyX = this.X;
				this.MyY = this.Y;
			}
			else if ( this.X == 3263 && this.Y == 2582 && this.Map == Map.Sosaria )
			{
				this.Name = "Shamino";
				this.MyAlignment = "good";
				this.Direction = Direction.East;

				this.Body = 0x190;
            	this.Hue = Utility.RandomSkinColor();

				this.FacialHairItemID = 0x2041;
				this.HairItemID = 0x203B;
				this.FacialHairHue = 0x8A5;
				this.HairHue = 0x8A5;

				AddItem( new FancyShirt( Utility.RandomYellowHue() ) );
				AddItem( new LongPants( Utility.RandomYellowHue() ) );
				AddItem( new Boots() );
				AddItem( new Hatchet() );
				AddItem( new Cloak( Utility.RandomRedHue() ) );
				AddItem( new LeatherGloves() );

            	this.Title = "the Woodsman";
				this.MyItemText = "of the Woodlands";
				this.MyItemHue = 0x840;
				this.MyWorld = this.Map;
				this.MyX = this.X;
				this.MyY = this.Y;
			}
			else if ( this.X == 3441 && this.Y == 3190 && this.Map == Map.Sosaria )
			{
				this.Name = "Stefano";
				this.MyAlignment = "neutral";
				this.Direction = Direction.West;

				this.Body = 0x190;
            	this.Hue = Utility.RandomSkinColor();

				this.FacialHairItemID = 0;
				this.HairItemID = 0x203C;
				this.FacialHairHue = 0x840;
				this.HairHue = 0x840;

				AddItem( new FancyShirt( Utility.RandomBlueHue() ) );
				AddItem( new LongPants( Utility.RandomBlueHue() ) );
				AddItem( new Boots() );
				AddItem( new Cloak( Utility.RandomBlueHue() ) );
				AddItem( new LeatherGloves() );

            	this.Title = "the Sneak";
				this.MyItemText = "of the Thief";
				this.MyItemHue = 0x83A;
				this.MyWorld = this.Map;
				this.MyX = 3317;
				this.MyY = 2064;
			}
			else if ( this.X == 1604 && this.Y == 1604 && this.Map == Map.Sosaria )
			{
				this.Body = 401; 
				this.Hue = 0x83EA;
				this.FacialHairItemID = 0;
				this.HairItemID = 0x2049;
				this.FacialHairHue = 0x5E3;
				this.HairHue = 0x5E3;

				AddItem( new Sandals() );
				Item cloth1 = new Skirt();
					cloth1.Hue = 0x907;
					this.AddItem( cloth1 );
				Item cloth2 = new FancyShirt();
					cloth2.Hue = 0x907;
					this.AddItem( cloth2 );

				this.Name = "Katrina";
				this.Title = "the Shepherd";
				this.MyAlignment = "good";
				this.Direction = Direction.East;
				this.MyItemText = "of the Beastmaster";
				this.MyItemHue = 0x840;
				this.MyWorld = this.Map;
				this.MyX = this.X;
				this.MyY = this.Y;
			}
			else if ( this.X == 4993 && this.Y == 3997 && this.Map == Map.Sosaria )
			{
				this.Body = 485; 
				this.Hue = 1461;
				this.Name = "the Guardian";
				this.MyAlignment = "evil";
				this.Direction = Direction.East;
				this.MyItemText = "of Pagan";
				this.MyItemHue = 1461;
				this.MyItemPower = 250;
				this.MyWorld = this.Map;
				this.MyX = 877;
				this.MyY = 2654;
			}
			else if ( this.X == 5033 && this.Y == 3750 && this.Map == Map.Sosaria )
			{
				this.Body = 400; 
				this.Hue = 0x83EA;
				this.FacialHairItemID = 0x204C;
				this.HairItemID = 0x203C;
				this.FacialHairHue = 0x370;
				this.HairHue = 0x370;

				AddItem( new Boots() );
				Item cloth1 = new Robe();
					cloth1.Hue = Utility.RandomBlueHue();
					this.AddItem( cloth1 );

				Item cloth2 = new WizardsHat();
					cloth2.Hue = Utility.RandomBlueHue();
					this.AddItem( cloth2 );

				this.Name = "Garamon";
				this.Title = "the Wizard";
				this.MyAlignment = "good";
				this.Direction = Direction.South;
				this.MyItemText = "of the Alchemist";
				this.MyItemHue = 0x6DF;
				this.MyWorld = this.Map;
				this.MyX = 6003;
				this.MyY = 3679;
			}
			else if ( this.X == 2648 && this.Y == 3306 && this.Map == Map.Sosaria )
			{
				this.Body = 401; 
				this.Hue = 0x83EA;
				this.FacialHairItemID = 0;
				this.HairItemID = 0x203D;
				this.FacialHairHue = 0x497;
				this.HairHue = 0x497;

				AddItem( new Boots() );

				Item cloth1 = new Cloak();
					cloth1.Hue = 0x497;
					this.AddItem( cloth1 );

				Item cloth2 = new ChainChest();
					cloth2.Hue = 0x963;
					this.AddItem( cloth2 );

				Item cloth3 = new RingmailArms();
					cloth3.Hue = 0x963;
					this.AddItem( cloth3 );

				Item cloth4 = new ChainLegs();
					cloth4.Hue = 0x963;
					this.AddItem( cloth4 );

				Item cloth5 = new RingmailGloves();
					cloth5.Hue = 0x963;
					this.AddItem( cloth5 );

				Item cloth6 = new NorseHelm();
					cloth6.Hue = 0x963;
					this.AddItem( cloth6 );

				Item cloth7 = new ChaosShield();
					cloth7.Hue = 0x497;
					this.AddItem( cloth7 );

				this.Name = "Mors Gotha";
				this.Title = "the Death Knight";
				this.MyAlignment = "evil";
				this.Direction = Direction.South;
				this.MyItemText = "of Death";
				this.MyItemHue = 0x963;
				this.MyWorld = this.Map;
				this.MyX = 3370;
				this.MyY = 1552;
			}
			else if ( this.X == 4136 && this.Y == 3424 && this.Map == Map.Sosaria )
			{
				this.Body = 24;
				this.Hue = 0x83B;
				this.Name = "Lethe";
				this.Title = "the Dreaded Lich";
				this.MyAlignment = "evil";
				this.Direction = Direction.South;
				this.MyItemText = "of the Undertaker";
				this.MyItemHue = 0x837;
				this.MyItemPower = 250;
				this.MyWorld = this.Map;
				this.MyX = 1528;
				this.MyY = 3599;
			}
		}

		public EpicCharacter( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 ); // version
            writer.Write( MyAlignment );
		    writer.Write( MyItemText );
		    writer.Write( MyItemHue );
		    writer.Write( MyItemPower );
		    writer.Write( MyWorld );
		    writer.Write( MyX );
		    writer.Write( MyY );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
            MyAlignment = reader.ReadString();
			MyItemText = reader.ReadString();
			MyItemHue = reader.ReadInt();
		    MyItemPower = reader.ReadInt();
			MyWorld = reader.ReadMap();
			MyX = reader.ReadInt();
			MyY = reader.ReadInt();
		}
	}
}

		
