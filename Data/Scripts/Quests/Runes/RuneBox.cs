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

namespace Server.Items
{
	public class RuneBox : Item
	{
		public Mobile RuneBoxOwner;
		[CommandProperty( AccessLevel.GameMaster )]
		public Mobile RuneBox_Owner { get{ return RuneBoxOwner; } set{ RuneBoxOwner = value; } }

		public int HasCompassion;
		[CommandProperty(AccessLevel.Owner)]
		public int Has_Compassion { get { return HasCompassion; } set { HasCompassion = value; InvalidateProperties(); } }

		public int HasHonesty;
		[CommandProperty(AccessLevel.Owner)]
		public int Has_Honesty { get { return HasHonesty; } set { HasHonesty = value; InvalidateProperties(); } }

		public int HasHonor;
		[CommandProperty(AccessLevel.Owner)]
		public int Has_Honor { get { return HasHonor; } set { HasHonor = value; InvalidateProperties(); } }

		public int HasHumility;
		[CommandProperty(AccessLevel.Owner)]
		public int Has_Humility { get { return HasHumility; } set { HasHumility = value; InvalidateProperties(); } }

		public int HasJustice;
		[CommandProperty(AccessLevel.Owner)]
		public int Has_Justice { get { return HasJustice; } set { HasJustice = value; InvalidateProperties(); } }

		public int HasSacrifice;
		[CommandProperty(AccessLevel.Owner)]
		public int Has_Sacrifice { get { return HasSacrifice; } set { HasSacrifice = value; InvalidateProperties(); } }

		public int HasSpirituality;
		[CommandProperty(AccessLevel.Owner)]
		public int Has_Spirituality { get { return HasSpirituality; } set { HasSpirituality = value; InvalidateProperties(); } }

		public int HasValor;
		[CommandProperty(AccessLevel.Owner)]
		public int Has_Valor { get { return HasValor; } set { HasValor = value; InvalidateProperties(); } }

		[Constructable]
		public RuneBox() : base( 0x52FB )
		{
			Name = "chest of virtue";
			Weight = 5.0;
			ItemID = Utility.RandomList( 0x52FB, 0x52FD );
			Light = LightType.Circle225;
		}

        public override void AddNameProperties(ObjectPropertyList list)
		{
            base.AddNameProperties(list);
			if ( RuneBoxOwner != null ){ list.Add( 1049644, "Belongs to " + RuneBoxOwner.Name + "" ); }
        }

		public override void OnDoubleClick( Mobile from )
		{
            int runes = HasCompassion + HasHonesty + HasHonor + HasHumility + HasJustice + HasSacrifice + HasSpirituality + HasValor;
			bool inVirtues = ( from.Map == Map.Sosaria && from.X >= 2587 && from.Y >= 3846 && from.X <= 2604 && from.Y <= 3863 );
			bool inCorrupt = ( from.Map == Map.Sosaria && from.X >= 2858 && from.Y >= 3463 && from.X <= 2875 && from.Y <= 3478 );

			if ( !IsChildOf( from.Backpack ) )
			{
				from.SendLocalizedMessage( 1060640 ); // The item must be in your backpack to use it.
			}
			else if ( RuneBoxOwner != from )
			{
				from.SendMessage( "This chest does not belong to you so it vanishes!" );
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

						if ( m == RuneBoxOwner )
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
			else if ( ( inVirtues || inCorrupt ) && runes > 7 )
			{
				string side = "good";
				int morality = 0;
				int color = 0;
				int tint = 0;

				string virtue1 = "Compassion";
				string virtue2 = "Honesty";
				string virtue3 = "Honor";
				string virtue4 = "Humility";
				string virtue5 = "Justice";
				string virtue6 = "Sacrifice";
				string virtue7 = "Spirituality";
				string virtue8 = "Valor";

				VirtueStoneChest box = new VirtueStoneChest();

				if ( inVirtues )
				{
					from.Fame = 15000;
					from.Karma = 15000;
					from.LocalOverheadMessage(MessageType.Emote, 1150, true, "You have cleansed the Runes in the Chamber of Virtue.");
					from.FixedParticles( 0x3709, 10, 30, 5052, 0x480, 0, EffectLayer.LeftFoot );
					from.PlaySound( 0x208 );
					PlayerSettings.SetKeys( from, "Virtue", true );
					from.Kills = 0;
					from.Criminal = false;
					if ( ((PlayerMobile)from).Fugitive == 1 )
					{
						((PlayerMobile)from).Fugitive = 0;
						from.Profile = "";
						((PlayerMobile)from).BardsTaleQuest = "0#0#0#0#0#0#0#0#0#0#0#0#0#0#0#";
					}
					from.SendMessage( "You have gained a really large amount of fame and karma." );
				}
				else
				{
					from.Fame = 15000;
					from.Karma = -15000;
					side = "evil";
					morality = 1;
					color = 0xB20;
					tint = 0x8B3;
					from.LocalOverheadMessage(MessageType.Emote, 1150, true, "You have corrupted the Runes of Virtue.");
					Effects.SendLocationEffect( from.Location, from.Map, 0x2A4E, 30, 10, 0xB00, 0 );
					from.PlaySound( 0x029 );
					PlayerSettings.SetKeys( from, "Corrupt", true );
					box.Name = "chest of corruption";
					box.Hue = color;
					from.SendMessage( "You have gain a really large amount of fame and lost a really large amount of karma." );

					virtue1 = "Cruelty";
					virtue2 = "Deceit";
					virtue3 = "Scorn";
					virtue4 = "Arrogance";
					virtue5 = "Oppression";
					virtue6 = "Neglect";
					virtue7 = "Sacrilege";
					virtue8 = "Fear";
				}

				QuestSouvenir.GiveReward( from, "Rune of " + virtue1, color, 0x5319 );
				QuestSouvenir.GiveReward( from, "Rune of " + virtue2, color, 0x530F );
				QuestSouvenir.GiveReward( from, "Rune of " + virtue3, color, 0x531B );
				QuestSouvenir.GiveReward( from, "Rune of " + virtue4, color, 0x5313 );
				QuestSouvenir.GiveReward( from, "Rune of " + virtue5, color, 0x5311 );
				QuestSouvenir.GiveReward( from, "Rune of " + virtue6, color, 0x5315 );
				QuestSouvenir.GiveReward( from, "Rune of " + virtue7, color, 0x530D );
				QuestSouvenir.GiveReward( from, "Rune of " + virtue8, color, 0x5317 );

				List<Item> belongings = new List<Item>();
				foreach( Item i in from.Backpack.Items )
				{
					if ( i is QuestSouvenir && (i.Name).Contains("Rune of") ){ belongings.Add(i); }
				}
				foreach ( Item stuff in belongings )
				{
					box.DropItem( stuff );
				}

				DDRelicPainting tapestry1 = new DDRelicPainting();	tapestry1.Name = "Tapestry of " + virtue1;	tapestry1.ItemID = 0x49A8;	tapestry1.CoinPrice = Utility.RandomMinMax( 10, 20 ) * 50;		tapestry1.Hue = tint;	box.DropItem( tapestry1 );
				DDRelicPainting tapestry2 = new DDRelicPainting();	tapestry2.Name = "Tapestry of " + virtue2;	tapestry2.ItemID = 0x49A2;	tapestry2.CoinPrice = Utility.RandomMinMax( 10, 20 ) * 50;		tapestry2.Hue = tint;	box.DropItem( tapestry2 );
				DDRelicPainting tapestry3 = new DDRelicPainting();	tapestry3.Name = "Tapestry of " + virtue3;	tapestry3.ItemID = 0x49B2;	tapestry3.CoinPrice = Utility.RandomMinMax( 10, 20 ) * 50;		tapestry3.Hue = tint;	box.DropItem( tapestry3 );
				DDRelicPainting tapestry4 = new DDRelicPainting();	tapestry4.Name = "Tapestry of " + virtue4;	tapestry4.ItemID = 0x49A3;	tapestry4.CoinPrice = Utility.RandomMinMax( 10, 20 ) * 50;		tapestry4.Hue = tint;	box.DropItem( tapestry4 );
				DDRelicPainting tapestry5 = new DDRelicPainting();	tapestry5.Name = "Tapestry of " + virtue5;	tapestry5.ItemID = 0x49A7;	tapestry5.CoinPrice = Utility.RandomMinMax( 10, 20 ) * 50;		tapestry5.Hue = tint;	box.DropItem( tapestry5 );
				DDRelicPainting tapestry6 = new DDRelicPainting();	tapestry6.Name = "Tapestry of " + virtue6;	tapestry6.ItemID = 0x49A0;	tapestry6.CoinPrice = Utility.RandomMinMax( 10, 20 ) * 50;		tapestry6.Hue = tint;	box.DropItem( tapestry6 );
				DDRelicPainting tapestry7 = new DDRelicPainting();	tapestry7.Name = "Tapestry of " + virtue7;	tapestry7.ItemID = 0x49A1;	tapestry7.CoinPrice = Utility.RandomMinMax( 10, 20 ) * 50;		tapestry7.Hue = tint;	box.DropItem( tapestry7 );
				DDRelicPainting tapestry8 = new DDRelicPainting();	tapestry8.Name = "Tapestry of " + virtue8;	tapestry8.ItemID = 0x49B3;	tapestry8.CoinPrice = Utility.RandomMinMax( 10, 20 ) * 50;		tapestry8.Hue = tint;	box.DropItem( tapestry8 );

				RuneOfVirtue reward = new RuneOfVirtue();
				reward.ItemOwner = from;
				reward.ItemSide = morality;
				RuneOfVirtue.RuneLook( reward );
				box.DropItem( reward );

				Item crystals = new Crystals( Utility.RandomMinMax( 1000, 2000 ) );				box.DropItem( crystals );
				Item jewels = new DDJewels( Utility.RandomMinMax( 2000, 4000 ) );				box.DropItem( jewels );
				Item gold = new Gold( Utility.RandomMinMax( 4000, 6000 ) );						box.DropItem( gold );
				Item silver = new DDSilver( Utility.RandomMinMax( 6000, 8000 ) );				box.DropItem( silver );
				Item copper = new DDCopper( Utility.RandomMinMax( 8000, 10000 ) );				box.DropItem( copper );
				Item gemstones = new DDGemstones( Utility.RandomMinMax( 1000, 2000 ) );			box.DropItem( gemstones );
				Item goldnuggets = new DDGoldNuggets( Utility.RandomMinMax( 2000, 30000 ) );	box.DropItem( goldnuggets );

				from.AddToBackpack( box );
				LoggingFunctions.LogRuneOfVirtue( from, side );

				this.Delete();
			}
			else
			{
				from.CloseGump( typeof( RuneBoxGump ) );
				from.SendGump( new RuneBoxGump( this, from ) );
			}
		}

		public RuneBox(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int) 0);

			writer.Write( (Mobile)RuneBoxOwner);

            writer.Write( HasCompassion );
            writer.Write( HasHonesty );
            writer.Write( HasHonor );
            writer.Write( HasHumility );
            writer.Write( HasJustice );
            writer.Write( HasSacrifice );
            writer.Write( HasSpirituality );
            writer.Write( HasValor );
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();

			RuneBoxOwner = reader.ReadMobile();

			HasCompassion = reader.ReadInt();
			HasHonesty = reader.ReadInt();
			HasHonor = reader.ReadInt();
			HasHumility = reader.ReadInt();
			HasJustice = reader.ReadInt();
			HasSacrifice = reader.ReadInt();
			HasSpirituality = reader.ReadInt();
			HasValor = reader.ReadInt();
		}

		private class RuneBoxGump : Gump
		{
			private RuneBox m_Box;
			private Map m_Map;
			private int m_X;
			private int m_Y;

			public RuneBoxGump( RuneBox box, Mobile from ) : base( 50, 50 )
			{
				from.SendSound( 0x5AA ); 

				m_Box = box;

				this.Closable=true;
				this.Disposable=true;
				this.Dragable=true;
				this.Resizable=false;

				int theme = 7035;
				int chamber = 10894;
				string room = "Chamber of Virtue";
				string map = "In the Land of Sosaria";
				string coor = "148° 37'N, 113° 58'E";
				string hue = "#7dc0c0";
				m_Map = Map.Sosaria;
				m_X = 4181;
				m_Y = 272;

				if ( ((PlayerMobile)from).KarmaLocked ) // THEY ARE ON AN EVIL PATH
				{
					theme = 7036;
					chamber = 10895;
					room = "Chamber of Corruption";
					map = "In the Underworld";
					coor = "17° 6'N, 120° 40'W";
					hue = "#e0ab9f";
					m_Map = Map.Underworld;
					m_X = 260;
					m_Y = 723;
				}

				AddPage(0);

				AddImage(0, 0, theme, Server.Misc.PlayerSettings.GetGumpHue( from ));
				AddButton(860, 9, 4017, 4017, 0, GumpButtonType.Reply, 0);
				AddHtml( 12, 12, 627, 20, @"<BODY><BASEFONT Color=" + hue + ">RUNES OF VIRTUE</BASEFONT></BODY>", (bool)false, (bool)false);
				AddHtml( 676, 281, 201, 169, @"<BODY><BASEFONT Color=" + hue + ">" + room + "<BR><BR>" + map + "<BR><BR>" + coor + "</BASEFONT></BODY>", (bool)false, (bool)false);
				AddImage(654, 46, chamber);
				AddHtml( 12, 48, 627, 409, @"<BODY><BASEFONT Color=" + hue + ">Após a derrota de Exodus e o fim da Era das Trevas, Lord British criou 8 Runas da Virtude para representar misticamente uma nova visão de vida, pela qual as pessoas poderiam se esforçar. Como a maior parte do mal externo havia sido derrotado, Lord British queria que as pessoas começassem a erradicar o mal que habita dentro de si mesmas. Para que esta nova filosofia funcionasse, Lord British pegou as Runas e as espalhou por toda parte. Ele desafia qualquer um a alcançar a iluminação em todas as Virtudes, encontrando as Runas e purificando-as na Câmara da Virtude. Aqueles que conseguissem seriam um símbolo do puro bem e seriam famosos por toda a terra. Há, no entanto, aqueles que buscam corromper as Virtudes. Eles também procuram pelas Runas e alcançariam seus próprios fins nefastos levando-as para a Câmara da Corrupção no Submundo.<br><br>Você pode escolher um desses caminhos. Com o Baú da Virtude, você pode procurar nas muitas masmorras por essas Runas e colocar as Runas dentro do baú. Quando você encontrar outro Baú da Virtude, você pode abrir o baú onde uma criatura emergirá que deve ser derrotada. Lord British colocou sentinelas mágicas para proteger as Runas e você deve derrotá-las em batalha para reivindicar a Runa. Você pode olhar dentro do seu baú a qualquer momento para ver quais Runas você possui. Quando você tiver coletado todas as 8 Runas, você pode então levar seu baú para a Câmara de sua escolha e abri-lo, mas o baú só mostrará a localização da Câmara que melhor reflete sua moralidade. Se seu karma estiver bloqueado, então você verá a localização da Câmara da Corrupção. Caso contrário, você verá a localização da Câmara da Virtude.<br><br>Aqueles que conseguirem adquirirão um Baú da Virtude para usar como contêiner. Dentro dele, encontrarão as 8 Runas como lembranças, estandartes que representam as virtudes e uma riqueza considerável. Você também receberá uma runa que nunca poderá ser perdida e ganhará poder enquanto a usa em suas aventuras. Conforme ela ganha níveis de poder, você pode clicar uma vez na runa e gastar pontos para moldar a runa à sua vontade. Você também pode clicar duas vezes na runa para mudar seu símbolo de virtude. Se você tiver uma Runa da Virtude, só poderá equipá-la se seu karma for positivo. Se você tiver uma Runa da Corrupção, só poderá equipá-la se seu karma for negativo.<br><br>Se você corromper as Runas, perderá uma quantidade muito grande de karma e adquirirá os mesmos tipos de itens listados acima, mas suas Virtudes serão o oposto do que Lord British havia estabelecido. Se você purificar as Runas, então ganhará uma quantidade muito grande de karma e será absolvido de todos os crimes. Qualquer curso lhe concederá uma quantidade muito grande de fama por toda a terra.</BASEFONT></BODY>", (bool)false, (bool)true);

				if ( Sextants.HasSextant( from ) )
					AddButton(851, 339, 10461, 10461, 1, GumpButtonType.Reply, 0);

				// RUNES
				if ( m_Box.HasCompassion > 0 ){ AddItem(220, 525, 21272); }
				if ( m_Box.HasHonesty > 0 ){ AddItem(280, 525, 21262); }
				if ( m_Box.HasHonor > 0 ){ AddItem(340, 525, 21274); }
				if ( m_Box.HasHumility > 0 ){ AddItem(400, 525, 21266); }
				if ( m_Box.HasJustice > 0 ){ AddItem(460, 525, 21264); }
				if ( m_Box.HasSacrifice > 0 ){ AddItem(520, 525, 21268); }
				if ( m_Box.HasSpirituality > 0 ){ AddItem(580, 525, 21260); }
				if ( m_Box.HasValor > 0 ){ AddItem(640, 525, 21270); }
			}

			public override void OnResponse(NetState state, RelayInfo info)
			{
				Mobile from = state.Mobile; 

				if ( info.ButtonID > 0 )
				{
					from.CloseGump( typeof( Sextants.MapGump ) );
					from.SendGump( new Sextants.MapGump( from, m_Map, m_X, m_Y, null ) );
				}
				else
				{
					from.PlaySound( 0x5AA );
					from.CloseGump( typeof( Sextants.MapGump ) );
				}
			}
		}
	}
}