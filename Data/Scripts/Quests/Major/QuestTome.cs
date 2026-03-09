using System;
using Server;
using Server.Network;
using Server.Multis;
using Server.Gumps;
using Server.Misc;
using Server.Mobiles;
using Server.Accounting;
using System.Collections.Generic;
using System.Collections;
using Server.Regions; 
using System.Globalization;
using Server.Custom;

namespace Server.Items
{
	public class QuestTome : Item
	{
		public Map DeliverMap;
		public int DeliverX;
		public int DeliverY;

		public Mobile QuestTomeOwner;
		[CommandProperty( AccessLevel.GameMaster )]
		public Mobile QuestTome_Owner { get{ return QuestTomeOwner; } set{ QuestTomeOwner = value; } }

		/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		public string QuestTomeStoryGood;
		[CommandProperty( AccessLevel.GameMaster )]
		public string QuestTome_StoryGood { get{ return QuestTomeStoryGood; } set{ QuestTomeStoryGood = value; } }

		public string QuestTomeLocateGood;
		[CommandProperty( AccessLevel.GameMaster )]
		public string QuestTome_LocateGood { get{ return QuestTomeLocateGood; } set{ QuestTomeLocateGood = value; } }

		public Land QuestTomeWorldGood;
		[CommandProperty( AccessLevel.GameMaster )]
		public Land QuestTome_WorldGood { get{ return QuestTomeWorldGood; } set{ QuestTomeWorldGood = value; } }

		public string QuestTomeNPCGood;
		[CommandProperty( AccessLevel.GameMaster )]
		public string QuestTome_NPCGood { get{ return QuestTomeNPCGood; } set{ QuestTomeNPCGood = value; } }

		public string QuestTomeStoryEvil;
		[CommandProperty( AccessLevel.GameMaster )]
		public string QuestTome_StoryEvil { get{ return QuestTomeStoryEvil; } set{ QuestTomeStoryEvil = value; } }

		public string QuestTomeLocateEvil;
		[CommandProperty( AccessLevel.GameMaster )]
		public string QuestTome_LocateEvil { get{ return QuestTomeLocateEvil; } set{ QuestTomeLocateEvil = value; } }

		public Land QuestTomeWorldEvil;
		[CommandProperty( AccessLevel.GameMaster )]
		public Land QuestTome_WorldEvil { get{ return QuestTomeWorldEvil; } set{ QuestTomeWorldEvil = value; } }

		public string QuestTomeNPCEvil;
		[CommandProperty( AccessLevel.GameMaster )]
		public string QuestTome_NPCEvil { get{ return QuestTomeNPCEvil; } set{ QuestTomeNPCEvil = value; } }

		/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		public string QuestTomeCitizen;
		[CommandProperty( AccessLevel.GameMaster )]
		public string QuestTome_Citizen { get{ return QuestTomeCitizen; } set{ QuestTomeCitizen = value; } }

		public int QuestTomeGoals;
		[CommandProperty(AccessLevel.Owner)]
		public int QuestTome_Goals { get { return QuestTomeGoals; } set { QuestTomeGoals = value; InvalidateProperties(); } }

		public string QuestTomeDungeon;
		[CommandProperty( AccessLevel.GameMaster )]
		public string QuestTome_Dungeon { get{ return QuestTomeDungeon; } set{ QuestTomeDungeon = value; } }

		public Land QuestTomeLand;
		[CommandProperty( AccessLevel.GameMaster )]
		public Land QuestTome_Land { get{ return QuestTomeLand; } set{ QuestTomeLand = value; } }

		public int QuestTomeType;
		[CommandProperty(AccessLevel.Owner)]
		public int QuestTome_Type { get { return QuestTomeType; } set { QuestTomeType = value; InvalidateProperties(); } }

		/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		public string GoalItem1;
		[CommandProperty(AccessLevel.Owner)]
		public string Goal_Item1 { get { return GoalItem1; } set { GoalItem1 = value; InvalidateProperties(); } }

		public string GoalItem2;
		[CommandProperty(AccessLevel.Owner)]
		public string Goal_Item2 { get { return GoalItem2; } set { GoalItem2 = value; InvalidateProperties(); } }

		public string GoalItem3;
		[CommandProperty(AccessLevel.Owner)]
		public string Goal_Item3 { get { return GoalItem3; } set { GoalItem3 = value; InvalidateProperties(); } }

		public string GoalItem4;
		[CommandProperty(AccessLevel.Owner)]
		public string Goal_Item4 { get { return GoalItem4; } set { GoalItem4 = value; InvalidateProperties(); } }

		/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		public string VillainCategory;
		[CommandProperty(AccessLevel.Owner)]
		public string Villain_Category { get { return VillainCategory; } set { VillainCategory = value; InvalidateProperties(); } }

		public string VillainType;
		[CommandProperty(AccessLevel.Owner)]
		public string Villain_Type { get { return VillainType; } set { VillainType = value; InvalidateProperties(); } }

		public string VillainName;
		[CommandProperty(AccessLevel.Owner)]
		public string Villain_Name { get { return VillainName; } set { VillainName = value; InvalidateProperties(); } }

		public string VillainTitle;
		[CommandProperty(AccessLevel.Owner)]
		public string Villain_Title { get { return VillainTitle; } set { VillainTitle = value; InvalidateProperties(); } }

		public int VillainBody;
		[CommandProperty(AccessLevel.Owner)]
		public int Villain_Body { get { return VillainBody; } set { VillainBody = value; InvalidateProperties(); } }

		public int VillainHue;
		[CommandProperty(AccessLevel.Owner)]
		public int Villain_Hue { get { return VillainHue; } set { VillainHue = value; InvalidateProperties(); } }

		/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		[Constructable]
		public QuestTome() : base( 0x1A97 )
		{
			Name = "lost journal";
			Weight = 1.0;
		}

        public override void AddNameProperties(ObjectPropertyList list)
		{
            base.AddNameProperties(list);
			if ( QuestTomeOwner != null ){ list.Add( 1049644, "Belongs to " + QuestTomeOwner.Name + "" ); }
        }

		public override void OnDoubleClick( Mobile from )
		{
			if ( !IsChildOf( from.Backpack ) )
			{
				from.SendLocalizedMessage( 1060640 ); // The item must be in your backpack to use it.
			}
			else if ( QuestTomeOwner != from )
			{
				from.SendMessage( "Este livro não pertence aqui e desintegra-se em pó!" );
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

						if ( m == QuestTomeOwner )
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
			else if ( QuestTomeGoals > 2 && from.Region.Name == QuestTomeDungeon && QuestTomeCitizen != "" )
			{
				QuestTomeCitizen = "";
				QuestTomeLand = Land.None;
				QuestTomeType = 0;

				Type mobType = ScriptCompiler.FindTypeByName( VillainType );
				Mobile mob = (Mobile)Activator.CreateInstance( mobType );
				BaseCreature monster = (BaseCreature)mob;

				SummonPrison.SetDifficultyForMonster( monster );

				Map map = from.Map;

				bool validLocation = false;
				Point3D loc = from.Location;

				for ( int j = 0; !validLocation && j < 10; ++j )
				{
					int x = from.X + Utility.Random( 3 ) - 1;
					int y = from.Y + Utility.Random( 3 ) - 1;
					int z = map.GetAverageZ( x, y );

					if ( validLocation = map.CanFit( x, y, from.Z, 16, false, false ) )
						loc = new Point3D( x, y, from.Z );
					else if ( validLocation = map.CanFit( x, y, z, 16, false, false ) )
						loc = new Point3D( x, y, z );
				}

				monster.NameHue = 0x22;
				monster.Hue = VillainHue;
				if ( VillainBody > 0 ){ monster.Body = VillainBody; }
				monster.Title = VillainTitle;
				monster.Name = VillainName;
				monster.MoveToWorld( loc, map );
				monster.Combatant = from;
				monster.Fame = 0;
				monster.Karma = 0;
				Effects.SendLocationParticles( EffectItem.Create( monster.Location, monster.Map, EffectItem.DefaultDuration ), 0x3728, 10, 10, 2023 );
				monster.PlaySound( 0x1FE );
			}
			else
			{
				from.CloseGump( typeof( QuestTomeGump ) );
				from.SendGump( new QuestTomeGump( this, from, 0 ) );
			}
		}

		public QuestTome(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int)2);

			writer.Write( DeliverMap );
			writer.Write( DeliverX );
			writer.Write( DeliverY );

			writer.Write( (Mobile)QuestTomeOwner );
			writer.Write( QuestTomeStoryGood );
			writer.Write( QuestTomeLocateGood );
			writer.Write( (int)QuestTomeWorldGood );
			writer.Write( QuestTomeNPCGood );
			writer.Write( QuestTomeStoryEvil );
			writer.Write( QuestTomeLocateEvil );
			writer.Write( (int)QuestTomeWorldEvil );
			writer.Write( QuestTomeNPCEvil );
			writer.Write( QuestTomeCitizen );
			writer.Write( QuestTomeGoals );
			writer.Write( QuestTomeDungeon );
			writer.Write( (int)QuestTomeLand );
			writer.Write( QuestTomeType );
			writer.Write( GoalItem1 );
			writer.Write( GoalItem2 );
			writer.Write( GoalItem3 );
			writer.Write( GoalItem4 );
			writer.Write( VillainCategory );
			writer.Write( VillainType );
			writer.Write( VillainName );
			writer.Write( VillainTitle );
			writer.Write( VillainBody );
			writer.Write( VillainHue );
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();

			switch ( version )
			{
				case 2:
				{
					DeliverMap = reader.ReadMap();
					DeliverX = reader.ReadInt();
					DeliverY = reader.ReadInt();
					break;
				}
			}

			QuestTomeOwner = reader.ReadMobile();
            QuestTomeStoryGood = reader.ReadString();
            QuestTomeLocateGood = reader.ReadString();

			if ( version < 1 )
				QuestTomeWorldGood = Server.Lands.LandRef( reader.ReadString() );
			else
				QuestTomeWorldGood = (Land)(reader.ReadInt());

            QuestTomeNPCGood = reader.ReadString();
            QuestTomeStoryEvil = reader.ReadString();
            QuestTomeLocateEvil = reader.ReadString();

			if ( version < 1 )
				QuestTomeWorldEvil = Server.Lands.LandRef( reader.ReadString() );
			else
				QuestTomeWorldEvil = (Land)(reader.ReadInt());

            QuestTomeNPCEvil = reader.ReadString();
            QuestTomeCitizen = reader.ReadString();
            QuestTomeGoals = reader.ReadInt();
            QuestTomeDungeon = reader.ReadString();

			if ( version < 1 )
				QuestTomeLand = Server.Lands.LandRef( reader.ReadString() );
			else
				QuestTomeLand = (Land)(reader.ReadInt());

            QuestTomeType = reader.ReadInt();
            GoalItem1 = reader.ReadString();
            GoalItem2 = reader.ReadString();
            GoalItem3 = reader.ReadString();
            GoalItem4 = reader.ReadString();
			VillainCategory = reader.ReadString();
			VillainType = reader.ReadString();
			VillainName = reader.ReadString();
			VillainTitle = reader.ReadString();
			VillainBody = reader.ReadInt();
			VillainHue = reader.ReadInt();
		}

		private class QuestTomeGump : Gump
		{
			private QuestTome m_Book;
			private Map m_Map;
			private int m_X;
			private int m_Y;

			public QuestTomeGump( QuestTome book, Mobile from, int page ) : base( 50, 50 )
			{
				m_Book = book;

				from.SendSound( 0x55 );

				m_Map = book.DeliverMap;
				m_X = book.DeliverX;
				m_Y = book.DeliverY;

				this.Closable=true;
				this.Disposable=true;
				this.Dragable=true;
				this.Resizable=false;

				AddPage(0);

				string color = "#c6c67b";
				string story = m_Book.QuestTomeStoryGood;
				string locat = m_Book.QuestTomeLocateGood;
				string world = Server.Lands.LandName( m_Book.QuestTomeWorldGood );
				string names = m_Book.QuestTomeNPCGood;
						
				if ( ((PlayerMobile)from).KarmaLocked ) // THEY ARE ON AN EVIL PATH
				{
					color = "#cfa495";
					story = m_Book.QuestTomeStoryEvil;
					locat = m_Book.QuestTomeLocateEvil;
					world = Server.Lands.LandName( m_Book.QuestTomeWorldEvil );
					names = m_Book.QuestTomeNPCEvil;
				}

				AddImage(0, 0, 7032, Server.Misc.PlayerSettings.GetGumpHue( from ));
				AddHtml( 12, 12, 665, 20, @"<BODY><BASEFONT Color=" + color + ">" + m_Book.Name + "</BASEFONT></BODY>", (bool)false, (bool)false);

				string dead = m_Book.Name; if ( dead.Contains("Journal of ") ){ dead = dead.Replace("Journal of ", ""); }
				if ( story.Contains("DDDDD") ){ story = story.Replace("DDDDD", dead); }

				if ( page > 0 )
				{
					AddButton(864, 9, 4017, 4017, 2, GumpButtonType.Reply, 0);
					AddHtml( 12, 43, 878, 548, @"<BODY><BASEFONT Color=" + color + ">Há muitos momentos em que aventureiros recebem uma grande missão para obter um item mágico derrotando uma criatura poderosa e, assim, usando o item para o bem ou para o mal. Você encontrou o diário de um desses aventureiros. Qual destino os atingiu, você nunca saberá. Eles perderam o diário? Pereceram em sua busca por " + m_Book.GoalItem4 + "?<br><br>Agora você possui o diário e pode prosseguir com esta missão, pois é só sua. A missão tem dois caminhos que você pode seguir. Se seu karma estiver trancado, o objetivo o levará pelo caminho vil de " + m_Book.QuestTomeNPCEvil + ". Caso contrário, sua missão servirá ao bem para " + m_Book.QuestTomeNPCGood + ". Você só pode ter uma única missão de diário por vez. Se você encontrar outro diário e escolher pegá-lo enquanto atualmente tem um diário, receberá um novo diário com a mesma missão inacabada que tinha antes.<br><br>Para derrotar " + m_Book.VillainName + " " + m_Book.VillainTitle + " e reivindicar " + m_Book.GoalItem4 + ", você terá que encontrar 3 itens únicos para ajudá-lo. Você não tem ideia de onde esses itens estão, então terá que falar com cidadãos (nomes laranja) para ver se eles ouviram rumores que podem ajudá-lo. Se um cidadão inicialmente não mencionar nada sobre sua missão, você terá que procurar outro. Quando você finalmente conseguir uma pista, uma melodia tocará e seu diário será atualizado com o rumor que eles lhe deram. Pode ser verdadeiro ou falso. Você não saberá até investigá-lo. Às vezes o item pode estar em um baú ou saco em um pedestal em uma masmorra, ou guardado por uma das criaturas mais poderosas dentro dessa masmorra.<br><br>Uma vez que você coletar as relíquias necessárias, deve então descobrir onde " + m_Book.VillainName + " está. Novamente, conversar com cidadãos pode revelar uma pista. Quando você descobrir onde " + m_Book.VillainName + " está, vá rapidamente para esse local e enfrente-o em batalha. Assim que você entrar na área, encontre um lugar estratégico onde deseja combatê-lo e então abra o diário para convocá-lo para enfrentá-lo. A batalha certamente será dura, então é melhor você estar preparado. Certifique-se de matá-lo para que você possa tomar " + m_Book.GoalItem4 + " dele. Fazê-lo desaparecer por outros meios o privará de seu objetivo, assim como sair da área onde ele está. Se ele conseguir escapar, você terá que procurar rumores novamente para determinar para onde " + m_Book.VillainName + " fugiu.<br><br>Matar " + m_Book.VillainName + " revelará uma abundância de riquezas que ele tomou de outros aventureiros que não conseguiram ser vitoriosos. Sinta-se à vontade para pegar este tesouro para si, pois " + m_Book.VillainName + " " + m_Book.VillainTitle + " não precisará mais dele. Uma vez que você adquiriu " + m_Book.GoalItem4 + ", procure por " + m_Book.QuestTomeNPCGood + " ou " + m_Book.QuestTomeNPCEvil + " e entregue a eles o diário. Sua moral e fama serão afetadas por sua escolha de ética e você será recompensado com um item de sua escolha. Quando você selecionar sua recompensa, o item aparecerá em sua mochila. Cada item aparecerá com um número de pontos que você pode gastar para melhorar seu item. Isso permite que você adapte o item para se adequar ao seu estilo. Para começar, clique uma vez nos itens e selecione 'Enfeitiçar'. Um menu aparecerá onde você pode escolher quais atributos deseja que o item tenha. Tenha cuidado, pois você não pode alterar um atributo depois de selecioná-lo.</BASEFONT></BODY>", (bool)false, (bool)false);
				}
				else
				{
					AddButton(864, 9, 4017, 4017, 0, GumpButtonType.Reply, 0);
					AddButton(792, 9, 3610, 3610, 1, GumpButtonType.Reply, 0);

					if ( Sextants.HasSextant( from ) )
						AddButton(756, 12, 10461, 10461, 3, GumpButtonType.Reply, 0);

					AddHtml( 12, 46, 346, 20, @"<BODY><BASEFONT Color=" + color + ">Quest for " + from.Name + "</BASEFONT></BODY>", (bool)false, (bool)false);

					if ( m_Book.QuestTomeCitizen != "" ){ story = GetRumor( m_Book, false ) + "<br><br>" + story; }

					AddHtml( 12, 82, 878, 358, @"<BODY><BASEFONT Color=" + color + ">" + story + "</BASEFONT></BODY>", (bool)false, (bool)false);

					TextInfo cultInfo = new CultureInfo("en-US", false).TextInfo;

					if ( m_Book.QuestTomeGoals < 4 )
					{
						AddHtml( 55, 461, 346, 20, @"<BODY><BASEFONT Color=" + color + ">" + cultInfo.ToTitleCase( m_Book.GoalItem1 ) + "</BASEFONT></BODY>", (bool)false, (bool)false);
						if ( m_Book.QuestTomeGoals > 0 ){ AddItem(0, 448, 20413); }
						AddHtml( 55, 513, 346, 20, @"<BODY><BASEFONT Color=" + color + ">" + cultInfo.ToTitleCase( m_Book.GoalItem2 ) + "</BASEFONT></BODY>", (bool)false, (bool)false);
						if ( m_Book.QuestTomeGoals > 1 ){ AddItem(0, 500, 20413); }
						AddHtml( 55, 564, 346, 20, @"<BODY><BASEFONT Color=" + color + ">" + cultInfo.ToTitleCase( m_Book.GoalItem3 ) + "</BASEFONT></BODY>", (bool)false, (bool)false);
						if ( m_Book.QuestTomeGoals > 2 ){ AddItem(0, 551, 20413); }
					}
					else
					{
						AddHtml( 55, 513, 346, 20, @"<BODY><BASEFONT Color=" + color + ">" + cultInfo.ToTitleCase( m_Book.GoalItem4 ) + "</BASEFONT></BODY>", (bool)false, (bool)false);
						AddItem(0, 500, 20413);
					}
				}
			}

			public override void OnResponse( NetState state, RelayInfo info ) 
			{
				Mobile from = state.Mobile; 
				from.CloseGump( typeof( Sextants.MapGump ) );

				if ( info.ButtonID == 1 ){ from.SendGump( new QuestTomeGump( m_Book, from, 1 ) ); }
				else if ( info.ButtonID == 2 ){ from.SendGump( new QuestTomeGump( m_Book, from, 0 ) ); }
				else if ( info.ButtonID == 3 )
				{
					from.SendGump( new QuestTomeGump( m_Book, from, 0 ) );
					from.SendGump( new Sextants.MapGump( from, m_Map, m_X, m_Y, null ) );
				}

				from.SendSound( 0x55 );
			}
		}

		public static string TellRumor( Mobile player, Mobile citizen )
		{
			string rumor = "";

			if ( citizen.Fame == 0 && player.Backpack.FindItemByType( typeof ( QuestTome ) ) != null )
			{
				QuestTome book = ( QuestTome )( player.Backpack.FindItemByType( typeof ( QuestTome ) ) );

				if ( book.QuestTomeOwner == player )
				{
					if ( Utility.RandomMinMax( 1, 10 ) > 1 ){ citizen.Fame = 1; }

					if ( citizen.Fame == 0 && book.QuestTomeCitizen == "" && book.QuestTomeGoals < 4 )
					{
						citizen.Fame = 1;
						SetRumor( citizen, book );
						rumor = GetRumor( book, true );
					}
				}
			}

			return rumor;
		}

		public static string GetRumor( QuestTome book, bool talk )
		{
			int goal = book.QuestTomeType;
			string locate = "held by a powerful creature";
			if ( goal == 2 ){ locate = "lost somewhere"; }
			if ( book.QuestTomeGoals == 3 ){ locate = "found"; goal = 3; }

			string world = Server.Lands.LandName( book.QuestTomeLand );
			string dungeon = book.QuestTomeDungeon;
			string from = book.QuestTomeCitizen;
			string item = book.GoalItem1;
				if ( book.QuestTomeGoals == 1 ){ item = book.GoalItem2; }
				else if ( book.QuestTomeGoals == 2 ){ item = book.GoalItem3; }
				else if ( book.QuestTomeGoals == 3 ){ item = book.VillainName + " " + book.VillainTitle; }

			if ( talk )
			{
				string who = "Eu ouvi";
				switch ( Utility.RandomMinMax( 0, 5 ) )
				{
					case 0:	who = "Eu ouvi";																								break;
					case 1:	who = "Eu soube";																								break;
					case 2:	who = "Eu descobri";																							break;
					case 3:	who = "O " + RandomThings.GetRandomJob() + " em " + RandomThings.GetRandomCity() + " me contou";				break;
					case 4:	who = "Eu ouvi alguns " + RandomThings.GetRandomJob() + " dizerem";												break;
					case 5:	who = "Meu amigo me contou";																						break;
				}
				return who + " que " + item + " pode estar " + locate + " dentro de " + dungeon + " em " + world + ".";
			}

			if ( world != "" ){ return "" + from + " lhe contou que " + item + " pode estar " + locate + " dentro de " + dungeon + " em " + world + "."; }

			return "";
		}

		public static void SetRumor( Mobile m, QuestTome book )
		{
			book.QuestTomeType = Utility.RandomMinMax( 1, 2 );

			if ( book.QuestTomeGoals > 2 ){ book.QuestTomeType = 3; }

			Land searchLocation = Land.Sosaria;

			switch ( Utility.RandomMinMax( 0, 10 ) )
			{
				case 0:		searchLocation = Land.Sosaria;		break;
				case 1:		searchLocation = Land.Lodoria;		break;
				case 2:		searchLocation = Land.Serpent;		break;
				case 3:		searchLocation = Land.Sosaria;		break;
				case 4:		searchLocation = Land.Lodoria;		break;
				case 5:		searchLocation = Land.Serpent;		break;
				case 6:		searchLocation = Land.UmberVeil;	break;
				case 7:		searchLocation = Land.Ambrosia;		break;
				case 8:		searchLocation = Land.IslesDread;	break;
				case 9:		searchLocation = Land.Savaged;		break;
				case 10:	searchLocation = Land.Kuldar;		break;
			}

			string dungeon = "the Dungeon of Doom";

			int aCount = 0;

			ArrayList targets = new ArrayList();

			if ( book.QuestTomeType == 1 )
			{
				foreach ( Mobile target in World.Mobiles.Values )
				if ( target.Region is DungeonRegion && target.Fame >= 18000 && !( target is Exodus || target is CodexGargoyleA || target is CodexGargoyleB ) )
				{
					if ( target.Land == searchLocation )
					{
						targets.Add( target );
						aCount++;
					}
				}
			}
			else
			{
				foreach ( Item target in World.Items.Values )
				if ( target is SearchBase || target is StealBase )
				{
					if ( target.Land == searchLocation )
					{
						targets.Add( target );
						aCount++;
					}
				}
			}

			aCount = Utility.RandomMinMax( 1, aCount );

			int xCount = 0;
			for ( int i = 0; i < targets.Count; ++i )
			{
				xCount++;

				if ( xCount == aCount )
				{
					if ( book.QuestTomeType == 1 )
					{
						Mobile finding = ( Mobile )targets[ i ];
						dungeon = Server.Misc.Worlds.GetRegionName( finding.Map, finding.Location );
					}
					else
					{
						Item finding = ( Item )targets[ i ];
						dungeon = Server.Misc.Worlds.GetRegionName( finding.Map, finding.Location );
					}
				}
			}

			book.QuestTomeLand = searchLocation;
			book.QuestTomeDungeon = dungeon;
			book.QuestTomeCitizen = "" + m.Name + " " + m.Title + "";
		}

		public static bool FoundItem( Mobile player, int type, MajorItemOnCorpse chest )
		{
			Item item = player.Backpack.FindItemByType( typeof ( QuestTome ) );
			QuestTome book = (QuestTome)item;

			if ( type == book.QuestTomeType && book.QuestTomeDungeon == Server.Misc.Worlds.GetRegionName( player.Map, player.Location ) && book.QuestTomeOwner == player && book.QuestTomeGoals < 3 )
			{
				if ( Utility.RandomMinMax( 1, 3 ) != 1 )
				{
					string relic = book.GoalItem1;
						if ( book.QuestTomeGoals == 1 ){ relic = book.GoalItem2; }
						else if ( book.QuestTomeGoals == 2 ){ relic = book.GoalItem3; }

					player.LocalOverheadMessage(MessageType.Emote, 1150, true, "Você encontrou " + relic + ".");
					player.SendSound( 0x5B4 );
					book.QuestTomeCitizen = "";
					book.QuestTomeDungeon = "";
					book.QuestTomeLand = Land.None;
					book.QuestTomeType = 0;
					book.QuestTomeGoals++;

					return true;
				}
				else
				{
					player.LocalOverheadMessage(MessageType.Emote, 1150, true, book.QuestTomeCitizen + " estava enganado ou mentiu.");
					player.SendSound( 0x5B3 );
					book.QuestTomeCitizen = "";
					book.QuestTomeDungeon = "";
					book.QuestTomeLand = Land.None;
					book.QuestTomeType = 0;

					return false;
				}
			}
			else if ( chest != null && book.VillainName == chest.VillainName && book.VillainTitle == chest.VillainTitle && book.QuestTomeOwner == player && book.QuestTomeGoals >= 3 )
			{
				RichesSystem.SpawnRiches( player, 5 );
				player.LocalOverheadMessage(MessageType.Emote, 1150, true, "Você encontrou " + book.GoalItem4 + ".");
				book.QuestTomeGoals++;
				return true;
			}
			return false;
		}

		public static void BossEscaped( Mobile from, string region )
		{
			if ( from.Backpack.FindItemByType( typeof ( QuestTome ) ) != null )
			{
				Item item = from.Backpack.FindItemByType( typeof ( QuestTome ) );
				QuestTome book = (QuestTome)item;

				if ( book.QuestTomeGoals > 2 && book.QuestTomeDungeon == region && book.QuestTomeOwner == from )
				{
					ArrayList targets = new ArrayList();
					foreach ( Mobile creature in World.Mobiles.Values )
					{
						if ( creature.Name == book.VillainName && creature.Title == book.VillainTitle )
						{
							targets.Add( creature );
						}
					}
					for ( int i = 0; i < targets.Count; ++i )
					{
						Mobile creature = ( Mobile )targets[ i ];

						Effects.SendLocationParticles( EffectItem.Create( creature.Location, creature.Map, EffectItem.DefaultDuration ), 0x3728, 10, 10, 2023 );
						creature.PlaySound( 0x1FE );

						creature.Delete();
					}
				}
			}
		}
	}
}
