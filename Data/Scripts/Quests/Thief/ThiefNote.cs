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
using Server.Accounting;

namespace Server.Items
{
	public class ThiefNote : Item
	{
		public Mobile NoteOwner;
		[CommandProperty( AccessLevel.GameMaster )]
		public Mobile Note_Owner { get{ return NoteOwner; } set{ NoteOwner = value; } }

		public string NoteItemCategory;
		[CommandProperty(AccessLevel.Owner)]
		public string Note_ItemCategory { get { return NoteItemCategory; } set { NoteItemCategory = value; InvalidateProperties(); } }

		public string NoteItem;
		[CommandProperty(AccessLevel.Owner)]
		public string Note_Item { get { return NoteItem; } set { NoteItem = value; InvalidateProperties(); } }

		public int NoteItemGot;
		[CommandProperty(AccessLevel.Owner)]
		public int Note_ItemGot { get { return NoteItemGot; } set { NoteItemGot = value; InvalidateProperties(); } }

		public string NoteItemArea;
		[CommandProperty(AccessLevel.Owner)]
		public string Note_ItemArea { get { return NoteItemArea; } set { NoteItemArea = value; InvalidateProperties(); } }

		public string NoteItemPerson;
		[CommandProperty(AccessLevel.Owner)]
		public string Note_ItemPerson { get { return NoteItemPerson; } set { NoteItemPerson = value; InvalidateProperties(); } }

		public int NoteDeliverType;
		[CommandProperty(AccessLevel.Owner)]
		public int Note_DeliverType { get { return NoteDeliverType; } set { NoteDeliverType = value; InvalidateProperties(); } }

		public string NoteDeliverTo;
		[CommandProperty(AccessLevel.Owner)]
		public string Note_DeliverTo { get { return NoteDeliverTo; } set { NoteDeliverTo = value; InvalidateProperties(); } }

		public int NoteReward;
		[CommandProperty(AccessLevel.Owner)]
		public int Note_Reward { get { return NoteReward; } set { NoteReward = value; InvalidateProperties(); } }

		public string NoteStory;
		[CommandProperty(AccessLevel.Owner)]
		public string Note_Story { get { return NoteStory; } set { NoteStory = value; InvalidateProperties(); } }

		[Constructable]
		public ThiefNote() : base( 0x2DD )
		{
			Name = "secret note";
			Weight = 1.0;
		}

        public override void AddNameProperties(ObjectPropertyList list)
		{
            base.AddNameProperties(list);
			if ( NoteOwner != null ){ list.Add( 1049644, "Coded for " + NoteOwner.Name + "" ); }
			if ( NoteItemGot > 0 ){ list.Add( 1070722, "Stolen " + NoteItem ); }
        }

		public override void OnDoubleClick( Mobile from )
		{
			if ( ThiefAllowed( from ) != null )
			{
				from.SendMessage( "Você precisa descansar do último trabalho, então leia esta nota em cerca de " + ThiefAllowed( from ) + " minutos." );
			}
			else if ( !IsChildOf( from.Backpack ) )
			{
				from.SendLocalizedMessage( 1060640 ); // O item deve estar em sua mochila para ser usado.
			}
			else if ( NoteOwner != from )
			{
				from.SendMessage( "Esta nota está escrita em um código que você não entende, então você a joga fora!" );
				bool remove = true;
				foreach ( Account a in Accounts.GetAccounts() )
				{
					if (a == null)
						break;

					int index = 0;

					for (int i = 0; i < a.Length; ++i)
					{
						Mobile m = a[i];

						if (m == null)
							continue;

						if ( m == NoteOwner )
						{
							m.AddToBackpack( this );
							remove = false;
						}

						++index;
					}
				}
				if ( remove )
				{
					this.Delete();
				}
			}
			else
			{
				from.SendSound( 0x249 );
				from.CloseGump( typeof( NoteGump ) );
				from.SendGump( new NoteGump( this ) );
			}
		}

		public static void ThiefTimeAllowed( Mobile m )
		{
			DateTime TimeFinished = DateTime.Now;
			string sFinished = Convert.ToString(TimeFinished);
			PlayerSettings.SetQuestInfo( m, "ThiefQuest", sFinished );
		}

		public static int ThiefTimeNew( Mobile m )
		{
			int ThiefTime = 10000;
			string sTime = PlayerSettings.GetQuestInfo( m, "ThiefQuest" );

			if ( sTime.Length > 0 && !( PlayerSettings.GetQuestState( m, "ThiefQuest" ) ) )
			{
				DateTime TimeThen = Convert.ToDateTime(sTime);
				DateTime TimeNow = DateTime.Now;
				long ticksThen = TimeThen.Ticks;
				long ticksNow = TimeNow.Ticks;
				int minsThen = (int)TimeSpan.FromTicks(ticksThen).TotalMinutes;
				int minsNow = (int)TimeSpan.FromTicks(ticksNow).TotalMinutes;
				ThiefTime = minsNow - minsThen;
			}
			return ThiefTime;
		}

		public static string ThiefAllowed( Mobile from )
		{
			int nAllowedForAnotherQuest = ThiefTimeNew( from );
			int nServerQuestTimeAllowed = MyServerSettings.GetTimeBetweenQuests();
			int nWhenForAnotherQuest = nServerQuestTimeAllowed - nAllowedForAnotherQuest;
			string sAllowedForAnotherQuest = nWhenForAnotherQuest.ToString();

			if ( nWhenForAnotherQuest > 0 ){ return sAllowedForAnotherQuest; }

			return null;
		}

		public ThiefNote(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int) 0);

			writer.Write( (Mobile)NoteOwner);
			writer.Write( NoteItemCategory );
			writer.Write( NoteItem );
			writer.Write( NoteItemGot );
			writer.Write( NoteItemArea );
			writer.Write( NoteItemPerson );
			writer.Write( NoteDeliverType );
			writer.Write( NoteDeliverTo );
			writer.Write( NoteReward );
			writer.Write( NoteStory );
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();

			NoteOwner = reader.ReadMobile();
			NoteItemCategory = reader.ReadString();
			NoteItem = reader.ReadString();
			NoteItemGot = reader.ReadInt();
			NoteItemArea = reader.ReadString();
			NoteItemPerson = reader.ReadString();
			NoteDeliverType = reader.ReadInt();
			NoteDeliverTo = reader.ReadString();
			NoteReward = reader.ReadInt();
			NoteStory = reader.ReadString();
		}

		public class NoteGump : Gump
		{
			private ThiefNote m_Note;

			public NoteGump( ThiefNote note ) : base( 100, 100 )
			{
				m_Note = note;

				this.Closable=true;
				this.Disposable=true;
				this.Dragable=true;
				this.Resizable=false;

				AddPage(0);

				string describe = "<br><br>Mantenha esta nota com você o tempo todo se for realizar este trabalho. Se você for roubar algo de uma masmorra, precisa encontrar o pedestal com a bolsa ou caixa que um ladrão normalmente tentaria roubar. Use a bolsa ou caixa no pedestal para ver se você rouba o item com sucesso. Se você precisar furtar de um comerciante da cidade, encontre o cofre dele e use sua habilidade de roubo para ver se consegue pegar o item. Fique avisado, você pode ser marcado como criminoso e os guardas certamente acabarão com você se for pego. Se conseguir escapar com vida e com o item procurado, então leve esta nota para o local especificado nestas instruções. Se você perder esta nota secreta, encontre o mestre da guilda dos ladrões e eles lhe darão uma cópia da mensagem.";

				AddImage(0, 0, 10901, 2801);
				AddImage(0, 0, 10899, 2378);
				AddHtml( 45, 78, 386, 218, @"<BODY><BASEFONT Color=#da6363>" + m_Note.NoteStory + describe + "</BASEFONT></BODY>", (bool)false, (bool)true);
			}
		}

		public static Item GetMyCurrentJob( Mobile m )
		{
			if ( m is PlayerMobile )
			{
				foreach ( Item item in World.Items.Values )
				if ( item is ThiefNote )
				{
					if ( ((ThiefNote)item).NoteOwner == m ){ return item; }
				}

			}
			return null;
		}

		public static void SetupNote( ThiefNote note, Mobile m )
		{
			note.Hue = Utility.RandomList( 0x95E, 0x95D, 0x95B, 0x952, 0x957, 0x94D, 0x944, 0x945, 0x940, 0x93D, 0xB79 );
			note.ItemID = Utility.RandomList( 0x2DD, 0x201A );

			note.NoteOwner = m;

			note.NoteItemCategory = "";
			note.NoteItem = QuestCharacters.QuestItems( true );
			note.NoteItemGot = 0;
			note.NoteItemPerson = ContainerFunctions.GetOwner( "Pilfer" );
			note.NoteDeliverType = Utility.RandomMinMax( 1, 2 );

			if ( Utility.RandomBool() ) // STEAL FROM TOWN
			{
				int rewardMod = 1;
				Land searchLocation = Land.Sosaria;
				switch ( Utility.RandomMinMax( 0, 13 ) )
				{
					case 0:		searchLocation = Land.Sosaria;			break;
					case 1:		searchLocation = Land.Sosaria;			break;
					case 2:		searchLocation = Land.Sosaria;			break;
					case 3:		searchLocation = Land.Lodoria;			rewardMod = 2;	if ( !( PlayerSettings.GetDiscovered( m, "the Land of Lodoria" ) ) ){ searchLocation = Land.Sosaria; } break;
					case 4:		searchLocation = Land.Lodoria;			rewardMod = 2;	if ( !( PlayerSettings.GetDiscovered( m, "the Land of Lodoria" ) ) ){ searchLocation = Land.Sosaria; } break;
					case 5:		searchLocation = Land.Lodoria;			rewardMod = 2;	if ( !( PlayerSettings.GetDiscovered( m, "the Land of Lodoria" ) ) ){ searchLocation = Land.Sosaria; } break;
					case 6:		searchLocation = Land.Serpent;			rewardMod = 3;	if ( !( PlayerSettings.GetDiscovered( m, "the Serpent Island" ) ) ){ searchLocation = Land.Sosaria; } break;
					case 7:		searchLocation = Land.Serpent;			rewardMod = 3;	if ( !( PlayerSettings.GetDiscovered( m, "the Serpent Island" ) ) ){ searchLocation = Land.Sosaria; } break;
					case 8:		searchLocation = Land.Serpent;			rewardMod = 3;	if ( !( PlayerSettings.GetDiscovered( m, "the Serpent Island" ) ) ){ searchLocation = Land.Sosaria; } break;
					case 9:		searchLocation = Land.IslesDread;		rewardMod = 4;	if ( !( PlayerSettings.GetDiscovered( m, "the Isles of Dread" ) ) ){ searchLocation = Land.Sosaria; } break;
					case 10:	searchLocation = Land.Savaged;			rewardMod = 5;	if ( !( PlayerSettings.GetDiscovered( m, "the Savaged Empire" ) ) ){ searchLocation = Land.Sosaria; } break;
					case 11:	searchLocation = Land.Savaged;			rewardMod = 5;	if ( !( PlayerSettings.GetDiscovered( m, "the Savaged Empire" ) ) ){ searchLocation = Land.Sosaria; } break;
					case 12:	searchLocation = Land.UmberVeil;		rewardMod = 2;	if ( !( PlayerSettings.GetDiscovered( m, "the Island of Umber Veil" ) ) ){ searchLocation = Land.Sosaria; } break;
					case 13:	searchLocation = Land.Kuldar;			rewardMod = 4;	if ( !( PlayerSettings.GetDiscovered( m, "the Bottle World of Kuldar" ) ) ){ searchLocation = Land.Sosaria; } break;
				}

				if ( !( PlayerSettings.GetDiscovered( m, "the Land of Sosaria" ) ) && searchLocation == Land.Sosaria )
				{
					if ( ((PlayerMobile)m).SkillStart == 11000 ){ searchLocation = Land.Savaged; }
					else { searchLocation = Land.Lodoria; }
				}

				if ( searchLocation == Land.Sosaria ){ rewardMod = 1; }

				int aCount = 0;
				ArrayList targets = new ArrayList();
				foreach ( Item target in World.Items.Values )
				if ( target is Coffer && target.Land == searchLocation )
				{
					targets.Add( target ); aCount++;
				}

				aCount = Utility.RandomMinMax( 1, aCount );

				int xCount = 0;
				for ( int i = 0; i < targets.Count; ++i )
				{
					xCount++;

					if ( xCount == aCount )
					{
						Item finding = ( Item )targets[ i ];
						Coffer coffer = (Coffer)finding;
						note.NoteItemArea = coffer.CofferTown;
						note.NoteItemCategory = coffer.CofferType;
						note.NoteReward = ( rewardMod * 500 ) + ( Utility.RandomMinMax( 0, 10 ) * 50 );
						note.NoteReward = (int)( (MyServerSettings.QuestRewardModifier() * 0.01) * note.NoteReward ) + note.NoteReward;
					}
				}
			}
			else // STEAL FROM DUNGEON
			{
				Land searchLocation = Land.Sosaria;
				switch ( Utility.RandomMinMax( 0, 15 ) )
				{
					case 0:		searchLocation = Land.Sosaria;			break;
					case 1:		searchLocation = Land.Sosaria;			break;
					case 2:		searchLocation = Land.Sosaria;			break;
					case 3:		searchLocation = Land.Lodoria;			if ( !( PlayerSettings.GetDiscovered( m, "the Land of Lodoria" ) ) ){ searchLocation = Land.Sosaria; } break;
					case 4:		searchLocation = Land.Lodoria;			if ( !( PlayerSettings.GetDiscovered( m, "the Land of Lodoria" ) ) ){ searchLocation = Land.Sosaria; } break;
					case 5:		searchLocation = Land.Lodoria;			if ( !( PlayerSettings.GetDiscovered( m, "the Land of Lodoria" ) ) ){ searchLocation = Land.Sosaria; } break;
					case 6:		searchLocation = Land.Serpent;			if ( !( PlayerSettings.GetDiscovered( m, "the Serpent Island" ) ) ){ searchLocation = Land.Sosaria; } break;
					case 7:		searchLocation = Land.Serpent;			if ( !( PlayerSettings.GetDiscovered( m, "the Serpent Island" ) ) ){ searchLocation = Land.Sosaria; } break;
					case 8:		searchLocation = Land.Serpent;			if ( !( PlayerSettings.GetDiscovered( m, "the Serpent Island" ) ) ){ searchLocation = Land.Sosaria; } break;
					case 9:		searchLocation = Land.IslesDread;		if ( !( PlayerSettings.GetDiscovered( m, "the Isles of Dread" ) ) ){ searchLocation = Land.Sosaria; } break;
					case 10:	searchLocation = Land.Savaged;			if ( !( PlayerSettings.GetDiscovered( m, "the Savaged Empire" ) ) ){ searchLocation = Land.Sosaria; } break;
					case 11:	searchLocation = Land.Savaged;			if ( !( PlayerSettings.GetDiscovered( m, "the Savaged Empire" ) ) ){ searchLocation = Land.Sosaria; } break;
					case 12:	searchLocation = Land.UmberVeil;		if ( !( PlayerSettings.GetDiscovered( m, "the Island of Umber Veil" ) ) ){ searchLocation = Land.Sosaria; } break;
					case 13:	searchLocation = Land.Kuldar;			if ( !( PlayerSettings.GetDiscovered( m, "the Bottle World of Kuldar" ) ) ){ searchLocation = Land.Sosaria; } break;
					case 14:	searchLocation = Land.Underworld;		if ( !( PlayerSettings.GetDiscovered( m, "the Underworld" ) ) ){ searchLocation = Land.Underworld; } break;
					case 15:	searchLocation = Land.Ambrosia;			if ( !( PlayerSettings.GetDiscovered( m, "the Land of Ambrosia" ) ) ){ searchLocation = Land.Sosaria; } break;
				}

				if ( !( PlayerSettings.GetDiscovered( m, "the Land of Sosaria" ) ) && searchLocation == Land.Sosaria )
				{
					if ( ((PlayerMobile)m).SkillStart == 11000 ){ searchLocation = Land.Savaged; }
					else { searchLocation = Land.Lodoria; }
				}

				int aCount = 0;
				ArrayList targets = new ArrayList();
				foreach ( Item target in World.Items.Values )
				if ( target is StealBase && target.Land == searchLocation )
				{
					targets.Add( target ); aCount++;
				}

				aCount = Utility.RandomMinMax( 1, aCount );

				int xCount = 0;
				for ( int i = 0; i < targets.Count; ++i )
				{
					xCount++;

					if ( xCount == aCount )
					{
						Item finding = ( Item )targets[ i ];
						note.NoteItemArea = Server.Misc.Worlds.GetRegionName( finding.Map, finding.Location );
						int difficult = Server.Difficult.GetDifficulty( finding.Location, finding.Map ) + 2;
							if ( difficult < 2 ){ difficult = 2; }
						note.NoteReward = ( difficult * 500 ) + ( Utility.RandomMinMax( 0, 10 ) * 50 );
					}
				}
			}

			Land dropLocation = Land.Sosaria;
			switch ( Utility.RandomMinMax( 0, 13 ) )
			{
				case 0:		dropLocation = Land.Sosaria;		break;
				case 1:		dropLocation = Land.Sosaria;		break;
				case 2:		dropLocation = Land.Sosaria;		break;
				case 3:		dropLocation = Land.Lodoria;		if ( !( PlayerSettings.GetDiscovered( m, "the Land of Lodoria" ) ) ){ dropLocation = Land.Sosaria; } break;
				case 4:		dropLocation = Land.Lodoria;		if ( !( PlayerSettings.GetDiscovered( m, "the Land of Lodoria" ) ) ){ dropLocation = Land.Sosaria; } break;
				case 5:		dropLocation = Land.Lodoria;		if ( !( PlayerSettings.GetDiscovered( m, "the Land of Lodoria" ) ) ){ dropLocation = Land.Sosaria; } break;
				case 6:		dropLocation = Land.Serpent;		if ( !( PlayerSettings.GetDiscovered( m, "the Serpent Island" ) ) ){ dropLocation = Land.Sosaria; } break;
				case 7:		dropLocation = Land.Serpent;		if ( !( PlayerSettings.GetDiscovered( m, "the Serpent Island" ) ) ){ dropLocation = Land.Sosaria; } break;
				case 8:		dropLocation = Land.Serpent;		if ( !( PlayerSettings.GetDiscovered( m, "the Serpent Island" ) ) ){ dropLocation = Land.Sosaria; } break;
				case 9:		dropLocation = Land.IslesDread;		if ( !( PlayerSettings.GetDiscovered( m, "the Isles of Dread" ) ) ){ dropLocation = Land.Sosaria; } break;
				case 10:	dropLocation = Land.Savaged;		if ( !( PlayerSettings.GetDiscovered( m, "the Savaged Empire" ) ) ){ dropLocation = Land.Sosaria; } break;
				case 11:	dropLocation = Land.Savaged;		if ( !( PlayerSettings.GetDiscovered( m, "the Savaged Empire" ) ) ){ dropLocation = Land.Sosaria; } break;
				case 12:	dropLocation = Land.UmberVeil;		if ( !( PlayerSettings.GetDiscovered( m, "the Island of Umber Veil" ) ) ){ dropLocation = Land.Sosaria; } break;
				case 13:	dropLocation = Land.Kuldar;			if ( !( PlayerSettings.GetDiscovered( m, "the Bottle World of Kuldar" ) ) ){ dropLocation = Land.Sosaria; } break;
			}

			if ( !( PlayerSettings.GetDiscovered( m, "the Land of Sosaria" ) ) && dropLocation == Land.Sosaria )
			{
				if ( ((PlayerMobile)m).SkillStart == 11000 ){ dropLocation = Land.Savaged; }
				else { dropLocation = Land.Lodoria; }
			}

			int dCount = 0;
			ArrayList drops = new ArrayList();
			foreach ( Item target in World.Items.Values )
			if ( ( (note.NoteDeliverType == 1 && target is HollowStump) || (note.NoteDeliverType == 2 && target is HayCrate) ) && target.Land == dropLocation )
			{
				drops.Add( target ); dCount++;
			}

			dCount = Utility.RandomMinMax( 1, dCount );

			int sCount = 0;
			for ( int i = 0; i < drops.Count; ++i )
			{
				sCount++;

				if ( sCount == dCount )
				{
					Item finding = ( Item )drops[ i ];

					if ( finding is HayCrate )
					{
						HayCrate hay = (HayCrate)finding;
						note.NoteDeliverTo = hay.HayTown;
					}
					else if ( finding is HollowStump )
					{
						HollowStump stump = (HollowStump)finding;
						note.NoteDeliverTo = stump.StumpTown;
					}
				}
			}

			string action = "recuperar";
				switch( Utility.RandomMinMax( 0, 4 ) )
				{
					case 0: action = "recuperar"; break;
					case 1: action = "roubar"; break;
					case 2: action = "adquirir"; break;
					case 3: action = "encontrar"; break;
					case 4: action = "pegar"; break;
				}

			string drop = "deixar";
				switch( Utility.RandomMinMax( 0, 4 ) )
				{
					case 0: drop = "deixar"; break;
					case 1: drop = "colocar"; break;
					case 2: drop = "depositar"; break;
					case 3: drop = "pôr"; break;
					case 4: drop = "largar"; break;
				}

			string container = "caixa de feno em";
			if ( note.NoteDeliverType == 1 ){ container = "tronco oco perto de"; }

			string location = note.NoteItemArea;
				if ( note.NoteItemCategory != "" && note.NoteItemCategory != null ){ location = "o " + note.NoteItemCategory + " em " + note.NoteItemArea; }

			note.NoteStory = note.NoteItemPerson + " quer que você " + action + " " + note.NoteItem + " de " + location + ".";
			note.NoteStory = note.NoteStory + " Quando você tiver o item, " + drop + " ele no " + container + " " + note.NoteDeliverTo + ".";
			note.NoteStory = note.NoteStory + " Lá você também encontrará seu pagamento de " + note.NoteReward + " moedas de ouro e instruções para seu próximo trabalho.";

			note.InvalidateProperties();
		}
	}
}