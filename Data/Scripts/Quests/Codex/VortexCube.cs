using System;
using Server;
using Server.Network;
using Server.Multis;
using Server.Gumps;
using Server.Mobiles;
using Server.Accounting;

namespace Server.Items
{
	public class VortexCube : Item
	{
		public Mobile CubeOwner;
		[CommandProperty( AccessLevel.GameMaster )]
		public Mobile Cube_Owner { get{ return CubeOwner; } set{ CubeOwner = value; } }

		/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		public int HasConvexLense;
		[CommandProperty(AccessLevel.Owner)]
		public int Has_ConvexLense { get { return HasConvexLense; } set { HasConvexLense = value; InvalidateProperties(); } }

		public int HasConcaveLense;
		[CommandProperty(AccessLevel.Owner)]
		public int Has_ConcaveLense { get { return HasConcaveLense; } set { HasConcaveLense = value; InvalidateProperties(); } }

		/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		public int HasKeyLaw;
		[CommandProperty(AccessLevel.Owner)]
		public int Has_KeyLaw { get { return HasKeyLaw; } set { HasKeyLaw = value; InvalidateProperties(); } }

			public string TextKeyLaw;
			[CommandProperty(AccessLevel.Owner)]
			public string Text_KeyLaw { get { return TextKeyLaw; } set { TextKeyLaw = value; InvalidateProperties(); } }

			public string LocationKeyLaw;
			[CommandProperty(AccessLevel.Owner)]
			public string Location_KeyLaw { get { return LocationKeyLaw; } set { LocationKeyLaw = value; InvalidateProperties(); } }

		public int HasKeyChaos;
		[CommandProperty(AccessLevel.Owner)]
		public int Has_KeyChaos { get { return HasKeyChaos; } set { HasKeyChaos = value; InvalidateProperties(); } }

			public string TextKeyChaos;
			[CommandProperty(AccessLevel.Owner)]
			public string Text_KeyChaos { get { return TextKeyChaos; } set { TextKeyChaos = value; InvalidateProperties(); } }

			public string LocationKeyChaos;
			[CommandProperty(AccessLevel.Owner)]
			public string Location_KeyChaos { get { return LocationKeyChaos; } set { LocationKeyChaos = value; InvalidateProperties(); } }

		public int HasKeyBalance;
		[CommandProperty(AccessLevel.Owner)]
		public int Has_KeyBalance { get { return HasKeyBalance; } set { HasKeyBalance = value; InvalidateProperties(); } }

			public string TextKeyBalance;
			[CommandProperty(AccessLevel.Owner)]
			public string Text_KeyBalance { get { return TextKeyBalance; } set { TextKeyBalance = value; InvalidateProperties(); } }

			public string LocationKeyBalance;
			[CommandProperty(AccessLevel.Owner)]
			public string Location_KeyBalance { get { return LocationKeyBalance; } set { LocationKeyBalance = value; InvalidateProperties(); } }

		/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		public int HasCrystalRed;
		[CommandProperty(AccessLevel.Owner)]
		public int Has_CrystalRed { get { return HasCrystalRed; } set { HasCrystalRed = value; InvalidateProperties(); } }

		public int HasCrystalBlue;
		[CommandProperty(AccessLevel.Owner)]
		public int Has_CrystalBlue { get { return HasCrystalBlue; } set { HasCrystalBlue = value; InvalidateProperties(); } }

		public int HasCrystalGreen;
		[CommandProperty(AccessLevel.Owner)]
		public int Has_CrystalGreen { get { return HasCrystalGreen; } set { HasCrystalGreen = value; InvalidateProperties(); } }

		public int HasCrystalYellow;
		[CommandProperty(AccessLevel.Owner)]
		public int Has_CrystalYellow { get { return HasCrystalYellow; } set { HasCrystalYellow = value; InvalidateProperties(); } }

		public int HasCrystalWhite;
		[CommandProperty(AccessLevel.Owner)]
		public int Has_CrystalWhite { get { return HasCrystalWhite; } set { HasCrystalWhite = value; InvalidateProperties(); } }

		public int HasCrystalPurple;
		[CommandProperty(AccessLevel.Owner)]
		public int Has_CrystalPurple { get { return HasCrystalPurple; } set { HasCrystalPurple = value; InvalidateProperties(); } }

		public string TextCrystal;
		[CommandProperty(AccessLevel.Owner)]
		public string Text_Crystal { get { return TextCrystal; } set { TextCrystal = value; InvalidateProperties(); } }

		public string LocationCrystal;
		[CommandProperty(AccessLevel.Owner)]
		public string Location_Crystal { get { return LocationCrystal; } set { LocationCrystal = value; InvalidateProperties(); } }

		/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		[Constructable]
		public VortexCube() : base( 0x05D5 )
		{
			Name = "vortex cube";
			Weight = 1.0;
			Light = LightType.Circle150;
		}

        public override void AddNameProperties(ObjectPropertyList list)
		{
            base.AddNameProperties(list);
			if ( CubeOwner != null ){ list.Add( 1049644, "Belongs to " + CubeOwner.Name + "" ); }
        }

		public override void OnDoubleClick( Mobile from )
		{
			if ( !IsChildOf( from.Backpack ) )
			{
				from.SendLocalizedMessage( 1060640 ); // The item must be in your backpack to use it.
			}
			else if ( CubeOwner != from )
			{
				from.SendMessage( "This Codex does not belong to you so it vanishes!" );
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

						if ( m == CubeOwner )
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
				from.CloseGump( typeof( VortexGump ) );
				from.SendGump( new VortexGump( this, from ) );
			}
		}

		public VortexCube(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int) 0);

			writer.Write( (Mobile)CubeOwner);

            writer.Write( HasConvexLense );
            writer.Write( HasConcaveLense );

            writer.Write( HasKeyLaw );
            writer.Write( TextKeyLaw );
            writer.Write( LocationKeyLaw );
            writer.Write( HasKeyChaos );
            writer.Write( TextKeyChaos );
            writer.Write( LocationKeyChaos );
            writer.Write( HasKeyBalance );
            writer.Write( TextKeyBalance );
            writer.Write( LocationKeyBalance );

            writer.Write( HasCrystalRed );
            writer.Write( HasCrystalBlue );
            writer.Write( HasCrystalGreen );
            writer.Write( HasCrystalYellow );
            writer.Write( HasCrystalWhite );
            writer.Write( HasCrystalPurple );

            writer.Write( TextCrystal );
            writer.Write( LocationCrystal );
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();

			CubeOwner = reader.ReadMobile();

			HasConvexLense = reader.ReadInt();
			HasConcaveLense = reader.ReadInt();

			HasKeyLaw = reader.ReadInt();
			TextKeyLaw = reader.ReadString();
			LocationKeyLaw = reader.ReadString();
			HasKeyChaos = reader.ReadInt();
			TextKeyChaos = reader.ReadString();
			LocationKeyChaos = reader.ReadString();
			HasKeyBalance = reader.ReadInt();
			TextKeyBalance = reader.ReadString();
			LocationKeyBalance = reader.ReadString();

			HasCrystalRed = reader.ReadInt();
			HasCrystalBlue = reader.ReadInt();
			HasCrystalGreen = reader.ReadInt();
			HasCrystalYellow = reader.ReadInt();
			HasCrystalWhite = reader.ReadInt();
			HasCrystalPurple = reader.ReadInt();

			TextCrystal = reader.ReadString();
			LocationCrystal = reader.ReadString();
		}

		private class VortexGump : Gump
		{
			private VortexCube m_Cube;

			public VortexGump( VortexCube cube, Mobile from ) : base( 50, 50 )
			{
				m_Cube = cube;

				string color = "#6cb89a";
				from.SendSound( 0x5AA );

				this.Closable=true;
				this.Disposable=true;
				this.Dragable=true;
				this.Resizable=false;

				AddPage(0);

				AddImage(0, 0, 7029, Server.Misc.PlayerSettings.GetGumpHue( from ));
				AddButton(961, 9, 4017, 4017, 0, GumpButtonType.Reply, 0);
				AddHtml( 11, 11, 807, 20, @"<BODY><BASEFONT Color=" + color + ">CODEX DA SABEDORIA SUPREMA</BASEFONT></BODY>", (bool)false, (bool)false);
				AddHtml( 12, 42, 976, 177, @"<BODY><BASEFONT Color=" + color + ">Aqueles que empunham o Codex da Sabedoria Suprema podem usar o conhecimento contido nele para se tornarem mais inteligentes (+25) e um grão-mestre em duas habilidades de sua escolha (+100 em 2 habilidades escolhidas). O Codex está dentro do Vazio Etéreo e só pode ser retirado de dentro da Câmara do Codex. Para isso, você deve obter as 3 Chaves do Infinito para entrar na câmara. Para ver dentro do Vazio, onde o Codex está, você precisará das Lentes Côncava e Convexa. Finalmente, este Cubo tem o poder de retirar coisas do Vazio. Para isso, você precisará encontrar os 6 cristais do vazio para energizar o cubo. Se você conseguir encontrar todos esses itens, poderá entrar na Câmara do Codex e se aproximar do Vazio. O Codex será então seu para fazer o que quiser, mas será de uso exclusivo seu. Certifique-se de levar este cubo com você ao realizar esta missão.</BASEFONT></BODY>", (bool)false, (bool)false);

				/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

				int k = -49;
				int g = -25;

				// PEDESTAIS
				AddItem(85+k, 246+g, 4643);
				AddItem(85+k, 346+g, 4643);
				AddItem(85+k, 446+g, 4643);

				AddHtml( 140+k, 252+g, 181, 20, @"<BODY><BASEFONT Color=" + color + ">O Cubo de Vortex</BASEFONT></BODY>", (bool)false, (bool)false);
				AddHtml( 140+k, 279+g, 1016, 20, @"<BODY><BASEFONT Color=" + color + ">Encontrado!</BASEFONT></BODY>", (bool)false, (bool)false);
				AddItem(80+k, 245+g, 1493);

				AddHtml( 140+k, 355+g, 181, 20, @"<BODY><BASEFONT Color=" + color + ">A Lente Côncava</BASEFONT></BODY>", (bool)false, (bool)false);
				if ( m_Cube.HasConcaveLense > 0 )
				{
					AddHtml( 140+k, 382+g, 1016, 20, @"<BODY><BASEFONT Color=" + color + ">Encontrada!</BASEFONT></BODY>", (bool)false, (bool)false);
					AddItem(80+k, 343+g, 1517);
				}
				else
				{
					AddHtml( 140+k, 382+g, 1016, 20, @"<BODY><BASEFONT Color=" + color + ">Naxatilor " + GargoyleLocation( "Naxatilor" ) + ".</BASEFONT></BODY>", (bool)false, (bool)false);
				}

				AddHtml( 140+k, 458+g, 181, 20, @"<BODY><BASEFONT Color=" + color + ">A Lente Convexa</BASEFONT></BODY>", (bool)false, (bool)false);
				if ( m_Cube.HasConvexLense > 0 )
				{
					AddHtml( 140+k, 485+g, 1016, 20, @"<BODY><BASEFONT Color=" + color + ">Encontrada!</BASEFONT></BODY>", (bool)false, (bool)false);
					AddItem(80+k, 443+g, 1518);
				}
				else
				{
					AddHtml( 140+k, 485+g, 1016, 20, @"<BODY><BASEFONT Color=" + color + ">Lor-wis-lem " + GargoyleLocation( "Lor-wis-lem" ) + ".</BASEFONT></BODY>", (bool)false, (bool)false);
				}

				/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

				// PEDESTAL
				AddItem(84+k, 552+g, 13042);

				if ( m_Cube.HasKeyLaw > 0 )
				{
					AddItem(89+k, 551+g, 13519); // CHAVE DA LEI
					AddHtml( 140+k, 538+g, 1016, 20, @"<BODY><BASEFONT Color=" + color + ">A Chave da Lei foi encontrada!</BASEFONT></BODY>", (bool)false, (bool)false);
				}
				else
				{
					AddHtml( 140+k, 538+g, 1016, 20, @"<BODY><BASEFONT Color=" + color + ">A Chave da Lei " + m_Cube.TextKeyLaw + " " + m_Cube.LocationKeyLaw + ".</BASEFONT></BODY>", (bool)false, (bool)false);
				}

				if ( m_Cube.HasKeyBalance > 0 )
				{
					AddItem(98+k, 542+g, 13516); // CHAVE DO EQUILÍBRIO
					AddHtml( 140+k, 568+g, 1016, 20, @"<BODY><BASEFONT Color=" + color + ">A Chave do Equilíbrio foi encontrada!</BASEFONT></BODY>", (bool)false, (bool)false);
				}
				else
				{
					AddHtml( 140+k, 568+g, 1016, 20, @"<BODY><BASEFONT Color=" + color + ">A Chave do Equilíbrio " + m_Cube.TextKeyBalance + " " + m_Cube.LocationKeyBalance + ".</BASEFONT></BODY>", (bool)false, (bool)false);
				}

				if ( m_Cube.HasKeyChaos > 0 )
				{
					AddItem(109+k, 550+g, 13520); // CHAVE DO CAOS
					AddHtml( 140+k, 598+g, 1016, 20, @"<BODY><BASEFONT Color=" + color + ">A Chave do Caos foi encontrada!</BASEFONT></BODY>", (bool)false, (bool)false);
				}
				else
				{
					AddHtml( 140+k, 598+g, 1016, 20, @"<BODY><BASEFONT Color=" + color + ">A Chave do Caos " + m_Cube.TextKeyChaos + " " + m_Cube.LocationKeyChaos + ".</BASEFONT></BODY>", (bool)false, (bool)false);
				}

				/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

				AddHtml( 99+k, 656+g, 1016, 20, @"<BODY><BASEFONT Color=" + color + ">Os Cristais do Vazio estão espalhados por toda a terra. O Cubo de Vortex pode guiá-lo em direção às masmorras onde eles podem estar.</BASEFONT></BODY>", (bool)false, (bool)false);

				int d = -104;
				int v = -66;

				if ( ( m_Cube.HasCrystalRed + m_Cube.HasCrystalBlue + m_Cube.HasCrystalGreen + m_Cube.HasCrystalYellow + m_Cube.HasCrystalWhite + m_Cube.HasCrystalPurple ) > 5 )
				{
					AddHtml( 116+v, 715, 976, 20, @"<BODY><BASEFONT Color=" + color + ">Todos os Cristais do Vazio foram encontrados!</BASEFONT></BODY>", (bool)false, (bool)false);
				}
				else if ( m_Cube.HasCrystalRed == 0 )
				{
					AddHtml( 116+v, 715, 976, 20, @"<BODY><BASEFONT Color=" + color + ">O Cristal do Vazio Vermelho " + m_Cube.TextCrystal + " " + m_Cube.LocationCrystal + ".</BASEFONT></BODY>", (bool)false, (bool)false);
				}
				else if ( m_Cube.HasCrystalBlue == 0 )
				{
					AddHtml( 116+v, 715, 976, 20, @"<BODY><BASEFONT Color=" + color + ">O Cristal do Vazio Azul " + m_Cube.TextCrystal + " " + m_Cube.LocationCrystal + ".</BASEFONT></BODY>", (bool)false, (bool)false);
				}
				else if ( m_Cube.HasCrystalGreen == 0 )
				{
					AddHtml( 116+v, 715, 976, 20, @"<BODY><BASEFONT Color=" + color + ">O Cristal do Vazio Verde " + m_Cube.TextCrystal + " " + m_Cube.LocationCrystal + ".</BASEFONT></BODY>", (bool)false, (bool)false);
				}
				else if ( m_Cube.HasCrystalYellow == 0 )
				{
					AddHtml( 116+v, 715, 976, 20, @"<BODY><BASEFONT Color=" + color + ">O Cristal do Vazio Amarelo " + m_Cube.TextCrystal + " " + m_Cube.LocationCrystal + ".</BASEFONT></BODY>", (bool)false, (bool)false);
				}
				else if ( m_Cube.HasCrystalWhite == 0 )
				{
					AddHtml( 116+v, 715, 976, 20, @"<BODY><BASEFONT Color=" + color + ">O Cristal do Vazio Branco " + m_Cube.TextCrystal + " " + m_Cube.LocationCrystal + ".</BASEFONT></BODY>", (bool)false, (bool)false);
				}
				else if ( m_Cube.HasCrystalPurple == 0 )
				{
					AddHtml( 116+v, 715, 976, 20, @"<BODY><BASEFONT Color=" + color + ">O Cristal do Vazio Roxo " + m_Cube.TextCrystal + " " + m_Cube.LocationCrystal + ".</BASEFONT></BODY>", (bool)false, (bool)false);
				}

				// PEDESTALS
				AddItem(382+d, 788, 4643);
				AddItem(462+d, 788, 4643);
				AddItem(542+d, 788, 4643);
				AddItem(622+d, 788, 4643);
				AddItem(702+d, 788, 4643);
				AddItem(782+d, 788, 4643);

				if ( m_Cube.HasCrystalRed > 0 ){ AddItem(379+d, 782, 6284); }
				if ( m_Cube.HasCrystalBlue > 0 ){ AddItem(459+d, 782, 6286); }
				if ( m_Cube.HasCrystalGreen > 0 ){ AddItem(539+d, 782, 6288); }
				if ( m_Cube.HasCrystalYellow > 0 ){ AddItem(619+d, 782, 6290); }
				if ( m_Cube.HasCrystalWhite > 0 ){ AddItem(699+d, 782, 6496); }
				if ( m_Cube.HasCrystalPurple > 0 ){ AddItem(779+d, 782, 6498); }
			}

			public override void OnResponse( NetState sender, RelayInfo info )
			{
				Mobile from = sender.Mobile;
				from.SendSound( 0x5AA ); 
			}
		}

		public static string GargoyleLocation( string gargoyle )
		{
			string where = "the gargoyle's whereabouts are currently unknown";

			foreach ( Mobile mob in World.Mobiles.Values )
			if ( mob is CodexGargoyleA && gargoyle == "Naxatilor" )
			{
				where = "the gargoyle is said to be within " + Server.Misc.Worlds.GetRegionName( mob.Map, mob.Location );
			}
			else if ( mob is CodexGargoyleB && gargoyle == "Lor-wis-lem" )
			{
				where = "the gargoyle is said to be within " + Server.Misc.Worlds.GetRegionName( mob.Map, mob.Location );
			}

			return where;
		}
	}
}