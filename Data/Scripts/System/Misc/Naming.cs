using Server.Gumps;
using Server.Items;
using Server.Misc;
using Server.Mobiles;
using Server.Network;
using Server;
using System.Collections;
using System.IO;
using System.Text;
using System.Threading;
using System;
using Server.Commands;
using System.Collections.Generic;
using System.Xml;

namespace Server
{
	public class NameList
	{
		private string m_Type;
		private string[] m_List;

		public string Type{ get{ return m_Type; } }
		public string[] List{ get{ return m_List; } }

		public bool ContainsName( string name )
		{
			for ( int i = 0; i < m_List.Length; i++ )
				if ( name == m_List[i] )
					return true;

			return false;
		}

		public NameList( string type, XmlElement xml )
		{
			m_Type = type;
			m_List = xml.InnerText.Split( ',' );

			for ( int i = 0; i < m_List.Length; ++i )
				m_List[i] = Utility.Intern( m_List[i].Trim() );
		}

		public string GetRandomName()
		{
			if ( m_List.Length > 0 )
				return m_List[Utility.Random( m_List.Length )];

			return "";
		}

		public static NameList GetNameList( string type )
		{
			NameList n = null;
			m_Table.TryGetValue( type, out n );
			return n;
		}

		public static string RandomName( string type )
		{
			NameList list = GetNameList( type );

			if ( list != null )
				return list.GetRandomName();

			return "";
		}

		private static Dictionary<string, NameList> m_Table;

		static NameList()
		{
			m_Table = new Dictionary<string, NameList>( StringComparer.OrdinalIgnoreCase );

			string filePath = Path.Combine( Core.BaseDirectory, "Data/System/XML/names.xml" );

			if ( !File.Exists( filePath ) )
				return;

			try
			{
				Load( filePath );
			}
			catch ( Exception e )
			{
				Console.WriteLine( "Warning: Exception caught loading name lists:" );
				Console.WriteLine( e );
			}
		}

		private static void Load( string filePath )
		{
			XmlDocument doc = new XmlDocument();
			doc.Load( filePath );

			XmlElement root = doc["names"];

			foreach ( XmlElement element in root.GetElementsByTagName( "namelist" ) )
			{
				string type = element.GetAttribute( "type" );

				if ( String.IsNullOrEmpty( type ) )
					continue;

				try
				{
					NameList list = new NameList( type, element );

					m_Table[type] = list;
				}
				catch
				{
				}
			}
		}
	}
}

namespace Server.Misc
{
	public class NameVerification
	{
		public static readonly char[] SpaceDashPeriodQuote = new char[]
			{
				' ', '-', '.', '\''
			};

	//Unique Naming System//

		public static readonly char[] SpaceOnly = new char[]
			{
				' '
			};

	//Unique Naming System//

		public static readonly char[] Empty = new char[0];

		public static void Initialize()
		{
			CommandSystem.Register( "ValidateName", AccessLevel.Administrator, new CommandEventHandler( ValidateName_OnCommand ) );
		}

		[Usage( "ValidateName" )]
		[Description( "Checks the result of NameValidation on the specified name." )]
		public static void ValidateName_OnCommand( CommandEventArgs e )
		{
			if ( Validate( e.ArgString, 2, 16, true, false, true, 1, SpaceDashPeriodQuote ) )
				e.Mobile.SendMessage( 0x59, "That name is considered valid." );
			else
				e.Mobile.SendMessage( 0x22, "That name is considered invalid." );
		}

		public static bool Validate( string name, int minLength, int maxLength, bool allowLetters, bool allowDigits, bool noExceptionsAtStart, int maxExceptions, char[] exceptions )
		{
			return Validate( name, minLength, maxLength, allowLetters, allowDigits, noExceptionsAtStart, maxExceptions, exceptions, m_Disallowed, m_StartDisallowed );
		}

		public static bool Validate( string name, int minLength, int maxLength, bool allowLetters, bool allowDigits, bool noExceptionsAtStart, int maxExceptions, char[] exceptions, string[] disallowed, string[] startDisallowed )
		{
			if ( name == null || name.Length < minLength || name.Length > maxLength )
				return false;

			int exceptCount = 0;

			name = name.ToLower();

			if ( !allowLetters || !allowDigits || (exceptions.Length > 0 && (noExceptionsAtStart || maxExceptions < int.MaxValue)) )
			{
				for ( int i = 0; i < name.Length; ++i )
				{
					char c = name[i];

					if ( c >= 'a' && c <= 'z' )
					{
						if ( !allowLetters )
							return false;

						exceptCount = 0;
					}
					else if ( c >= '0' && c <= '9' )
					{
						if ( !allowDigits )
							return false;

						exceptCount = 0;
					}
					else
					{
						bool except = false;

						for ( int j = 0; !except && j < exceptions.Length; ++j )
							if ( c == exceptions[j] )
								except = true;

						if ( !except || (i == 0 && noExceptionsAtStart) )
							return false;

						if ( exceptCount++ == maxExceptions )
							return false;
					}
				}
			}

			for ( int i = 0; i < disallowed.Length; ++i )
			{
				int indexOf = name.IndexOf( disallowed[i] );

				if ( indexOf == -1 )
					continue;

				bool badPrefix = ( indexOf == 0 );

				for ( int j = 0; !badPrefix && j < exceptions.Length; ++j )
					badPrefix = ( name[indexOf - 1] == exceptions[j] );

				if ( !badPrefix )
					continue;

				bool badSuffix = ( (indexOf + disallowed[i].Length) >= name.Length );

				for ( int j = 0; !badSuffix && j < exceptions.Length; ++j )
					badSuffix = ( name[indexOf + disallowed[i].Length] == exceptions[j] );

				if ( badSuffix )
					return false;
			}

			for ( int i = 0; i < startDisallowed.Length; ++i )
			{
				if ( name.StartsWith( startDisallowed[i] ) )
					return false;
			}

			return true;
		}

		public static string[] StartDisallowed { get { return m_StartDisallowed; } }
		public static string[] Disallowed { get { return m_Disallowed; } }

		private static string[] m_StartDisallowed = new string[]
			{
				"seer",
				"counselor",
				"gm",
				"admin",
				"lady",
				"lord"
			};

		private static string[] m_Disallowed = new string[]
			{
				"jigaboo",
				"chigaboo",
				"wop",
				"kyke",
				"kike",
				"tit",
				"spic",
				"prick",
				"piss",
				"lezbo",
				"lesbo",
				"felatio",
				"dyke",
				"dildo",
				"chinc",
				"chink",
				"cunnilingus",
				"cum",
				"cocksucker",
				"cock",
				"clitoris",
				"clit",
				"ass",
				"hitler",
				"penis",
				"nigga",
				"nigger",
				"klit",
				"kunt",
				"jiz",
				"jism",
				"jerkoff",
				"jackoff",
				"goddamn",
				"fag",
				"blowjob",
				"bitch",
				"asshole",
				"dick",
				"pussy",
				"snatch",
				"cunt",
				"twat",
				"shit",
				"fuck",
				"tailor",
				"smith",
				"scholar",
				"rogue",
				"novice",
				"neophyte",
				"merchant",
				"medium",
				"master",
				"mage",
				"lb",
				"journeyman",
				"grandmaster",
				"fisherman",
				"expert",
				"chef",
				"carpenter",
				"british",
				"blackthorne",
				"blackthorn",
				"beggar",
				"archer",
				"apprentice",
				"adept",
				"gamemaster",
				"frozen",
				"squelched",
				"invulnerable",
				"osi",
				"origin",

		//Unique Naming System//

				"generic player"

		//Unique Naming System//
			};
	}
}

namespace Server.Items
{
	[Flipable(0xFBD, 0xFBE)]
	public class CensusRecords : Item
	{
		[Constructable]
		public CensusRecords( ) : base( 0xFBD )
		{
			Weight = 1.0;
			Name = "Census Records";
		}

		public override void OnDoubleClick( Mobile e )
		{
			if ( Name == "Census Records" ){ e.SendGump( new CensusGump( e, true ) ); } else { e.SendGump( new CensusGump( e, false ) ); }
		}

		public CensusRecords(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int) 0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}
	}
}

namespace Server.Gumps
{
    public class CensusGump : Gump
    {
        public CensusGump(Mobile from, bool legal) : base(50, 50)
        {
			from.SendSound( 0x4A ); 
			string color = "#b7765d";

            this.Closable=true;
			this.Disposable=true;
			this.Dragable=true;
			this.Resizable=false;

			string text = "Estes são os registos censitários das muitas terras, e os sábios compilaram uma lista de nomes de seus cidadãos. Teu nome também está nesta lista. Se desejas mudar teu nome, podes fazê-lo dentro deste livro.";

			if ( !legal ){ text = "Estes são os registos censitários falsificados das muitas terras, e a guilda de ladrões compilou uma lista de nomes de seus cidadãos. Teu nome também está nesta lista. Se desejas mudar teu nome, podes fazê-lo dentro deste livro."; } 

			AddPage(0);

			AddImage(0, 0, 9547, Server.Misc.PlayerSettings.GetGumpHue( from ));
			AddHtml( 15, 15, 577, 261, @"<BODY><BASEFONT Color=" + color + ">" + text + " Portanto, se tens uma ideia para um novo nome apropriado para fantasia, e estás disposto a gastar 2.000 peças de ouro, então apaga o texto abaixo e reescreve-o. Um novo nome não pode ter mais de 16 caracteres.</BASEFONT></BODY>", (bool)false, (bool)false);

			AddHtml( 15, 301, 577, 152, @"<BODY><BASEFONT Color=#E5FF00>AVISO: Se decides mudar teu nome, prossegue e fá-lo. Depois de o fazeres, fecha teu cliente do jogo. Então, navega até a pasta do perfil da tua conta no diretório `Game\Data\Profiles`. Nessa pasta, verás um diretório que corresponde ao nome anterior do teu personagem. Renomeia essa pasta para o nome exato para o qual mudaste teu personagem. Depois podes iniciar o cliente novamente e continuar a jogar. Fazer este processo garantirá que todas as tuas configurações do cliente serão aplicadas ao novo nome do personagem, sem criar um novo perfil e redefinir tudo para as configurações padrão.</BASEFONT></BODY>", (bool)false, (bool)false);

			AddTextEntry(49, 496, 200, 20, 0x481, 1, @"Escreve aqui...", 16);
			AddButton(13, 495, 4023, 4023, 1, GumpButtonType.Reply, 0);
			AddButton(563, 493, 4020, 4020, 0, GumpButtonType.Reply, 0);
        }

        private string GetString(RelayInfo info, int id)
        {
            TextRelay t = info.GetTextEntry(id);
            return (t == null ? null : t.Text.Trim());
        }

        public override void OnResponse( NetState sender, RelayInfo info )
        {
            Mobile from = sender.Mobile;
			from.SendSound( 0x4A ); 

            if (from == null)
            {
                return;
            }

			Container pack = from.Backpack;

            string name = GetString(info, 1);
            if (name != null)
            {
                name = name.Trim();
            }

			if ( name == "Type here..." || info.ButtonID == 0 )
			{
			}
            else if (name != "" && info.ButtonID == 1)
            {
                if (!NameVerification.Validate(name, 2, 16, true, false, true, 1, NameVerification.SpaceOnly))
                {
                    from.SendMessage(0X22, "That name is unacceptable or already taken.");
                    return;
                }
                else if ( CharacterCreation.CheckDupe(from, name) && pack.ConsumeTotal(typeof(Gold), 2000) )
                {
                    from.SendMessage(0X22, "Your name is now {0}.", name);
                    from.Name = name;
                    from.CantWalk = false;
                    return;
                }
                else if ( CharacterCreation.CheckDupe(from, name) && !(pack.ConsumeTotal(typeof(Gold), 2000)) )
                {
                    from.SendMessage(0X22, "You do not have enough gold!");
                    return;
                }
                else
                {
                    from.SendMessage(0X22, "That name is unacceptable or already taken.");
                    return;
                }
            }
            else
            {
                from.SendMessage(0X22, "You must enter a name.");
            }
        }
    }
}

namespace Server.Gumps
{
    public class CustomTitleGump : Gump
    {
        public CustomTitleGump(Mobile from) : base(50, 50)
        {
            this.Closable=true;
			this.Disposable=true;
			this.Dragable=true;
			this.Resizable=false;

			AddPage(0);

			from.SendSound( 0x4A ); 
			string color = "#b7765d";

            this.Closable=true;
			this.Disposable=true;
			this.Dragable=true;
			this.Resizable=false;

			string text = "O mundo te conhecerá pelo título de tua melhor habilidade, ou se especificares uma habilidade pela qual desejas ser conhecido em teu título. No entanto, podes escolher um título único de tua própria autoria. Aqui podes criar um título, ou se já definiste um título personalizado, podes removê-lo ou alterá-lo.";

			AddPage(0);

			AddImage(0, 0, 9577, Server.Misc.PlayerSettings.GetGumpHue( from ));
			AddHtml( 12, 12, 280, 210, @"<BODY><BASEFONT Color=" + color + ">" + text + " Portanto, se tens uma ideia para um título de personagem único, então apaga o texto abaixo e reescreve-o. Um novo título não pode ter mais de 25 caracteres.</BASEFONT></BODY>", (bool)false, (bool)false);
			AddTextEntry(48, 237, 200, 20, 0x481, 1, @"Escreve aqui...", 25);
			AddButton(12, 236, 4023, 4023, 1, GumpButtonType.Reply, 0);
			AddButton(267, 236, 4020, 4020, 0, GumpButtonType.Reply, 0);
        }

        private string GetString(RelayInfo info, int id)
        {
            TextRelay t = info.GetTextEntry(id);
            return (t == null ? null : t.Text.Trim());
        }

        public override void OnResponse( NetState sender, RelayInfo info )
        {
            Mobile from = sender.Mobile;
			from.SendSound( 0x4A ); 
            if ( from == null )
            {
                return;
            }

            string name = GetString(info, 1);
            if (name != null)
            {
                name = name.Trim();
            }

			if ( name == "Type here..." )
			{
			}
			else if ( name.Contains("Legendary ") || name.Contains("Elder ") || name.Contains("Grandmaster ") || name.Contains("Master ") || name.Contains("Adept ") || name.Contains("Expert ") || name.Contains("Journeyman ") || name.Contains("Apprentice ") || name.Contains("Novice ") || name.Contains("Neophyte ") )
            {
				from.SendMessage(0X22, "The words you used are not allowed.", name);
				return;
            }
			else if ( name.Contains("legendary ") || name.Contains("elder ") || name.Contains("grandmaster ") || name.Contains("master ") || name.Contains("adept ") || name.Contains("expert ") || name.Contains("journeyman ") || name.Contains("apprentice ") || name.Contains("novice ") || name.Contains("neophyte ") )
            {
				from.SendMessage(0X22, "The words you used are not allowed.", name);
				return;
            }
			else if ( name.Contains("Titan ") || name.Contains("titan ") )
            {
				from.SendMessage(0X22, "The words you used are not allowed.", name);
				return;
            }
            else if ( name != "" && name != "clear" )
            {
				from.SendMessage(0X22, "Your title is now {0}.", name);
				from.Title = name;
				return;
            }
            else
            {
				from.Title = null;
                from.SendMessage(0X22, "Your title has been removed.");
            }
			from.CloseGump( typeof( Server.Engines.Help.HelpGump ) );
			from.SendGump( new Server.Engines.Help.HelpGump( from, 12 ) );
        }
    }
}

namespace Server.Gumps
{
    public class NameAlterGump : Gump
    {
        public NameAlterGump(Mobile from) : base(50, 50)
        {
			from.SendSound( 0x4A ); 
            this.Closable=true;
			this.Disposable=true;
			this.Dragable=true;
			this.Resizable=false;

			AddPage(0);

			from.SendSound( 0x4A ); 
			string color = "#b7765d";

            this.Closable=true;
			this.Disposable=true;
			this.Dragable=true;
			this.Resizable=false;

			AddPage(0);

			AddImage(0, 0, 9577, Server.Misc.PlayerSettings.GetGumpHue( from ));
			AddHtml( 12, 12, 280, 210, @"<BODY><BASEFONT Color=" + color + ">Um mundo de fantasia é melhor servido com um nome único de fantasia para teu personagem. Se estás a reconsiderar um nome diferente, agora é a hora de inserir o nome pelo qual desejas ser conhecido no diário da cigana. Todos os nomes devem ser únicos entre outros aventureiros, portanto escolhe sabiamente. Se sentes que teu nome é apropriado, então fecha este livro. Caso contrário, remove o texto abaixo e insere um novo nome para ti mesmo que não tenha mais de 16 caracteres.</BASEFONT></BODY>", (bool)false, (bool)false);
			AddTextEntry(48, 237, 200, 20, 0x481, 1, @"Escreve aqui...", 16);
			AddButton(12, 236, 4023, 4023, 1, GumpButtonType.Reply, 0);
			AddButton(267, 236, 4020, 4020, 0, GumpButtonType.Reply, 0);
        }

        private string GetString(RelayInfo info, int id)
        {
            TextRelay t = info.GetTextEntry(id);
            return (t == null ? null : t.Text.Trim());
        }

        public override void OnResponse( NetState sender, RelayInfo info )
        {
            Mobile from = sender.Mobile;
			from.SendSound( 0x4A ); 
            if (from == null)
            {
                return;
            }

            string name = GetString(info, 1);
            if (name != null)
            {
                name = name.Trim();
            }
            else
            {
                from.SendMessage(0X22, "Podes inserir um nome.");
                from.SendGump(new NameAlterGump(from));
            }

			if ( name == "Escreve aqui..." || info.ButtonID == 0 )
			{
			}
            else if (name != "" && info.ButtonID == 1)
            {
                if (!NameVerification.Validate(name, 2, 16, true, false, true, 1, NameVerification.SpaceOnly))
				{
					from.SendMessage(0X22, "Esse nome é inaceitável ou já está em uso.");
					return;
				}
				else if ( CharacterCreation.CheckDupe(from, name) )
				{
					from.SendMessage(0X22, "Teu nome agora é {0}.", name);
					from.Name = name;
					from.CantWalk = false;
					return;
				}
				else if ( CharacterCreation.CheckDupe(from, name) )
				{
					from.SendMessage(0X22, "Esse nome é inaceitável ou já está em uso.");
					return;
				}
            }
            else
            {
                from.SendMessage(0X22, "Deves inserir um nome.");
            }
        }
    }
}

namespace Server.Gumps
{
    public class NameChangeGump : Gump
    {
        public NameChangeGump(Mobile from) : base(50, 50)
        {
            Closable = false;
            Disposable = false;
            Dragable = true;
            Resizable = false;

            AddPage(0);

			from.SendSound( 0x4A ); 
			string color = "#b7765d";

            this.Closable=true;
			this.Disposable=true;
			this.Dragable=true;
			this.Resizable=false;

			AddPage(0);

			AddImage(0, 0, 9577, Server.Misc.PlayerSettings.GetGumpHue( from ));
			AddHtml( 12, 12, 280, 210, @"<BODY><BASEFONT Color=" + color + ">O nome que escolheste está atualmente em uso e já não está disponível. Deves escolher um nome diferente antes de poderes continuar. Portanto, apaga o texto abaixo e insere um novo nome apropriado para fantasia.</BASEFONT></BODY>", (bool)false, (bool)false);
			AddTextEntry(48, 237, 200, 20, 0x481, 1, @"Type here...", 16);
			AddButton(12, 236, 4023, 4023, 1, GumpButtonType.Reply, 0);
			AddButton(267, 236, 4020, 4020, 0, GumpButtonType.Reply, 0);
        }

        private string GetString(RelayInfo info, int id)
        {
            TextRelay t = info.GetTextEntry(id);
            return (t == null ? null : t.Text.Trim());
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;
            if (from == null)
            {
                return;
            }

            string name = GetString(info, 1);
            if (name != null)
            {
                name = name.Trim();
            }
           else
			{
				from.SendMessage(0X22, "Deves inserir um nome.");
				from.SendGump(new NameChangeGump(from));
			}

			if ( name == "Escreve aqui..." || info.ButtonID == 0 )
			{
			}
			else if (name != "" && info.ButtonID == 1)
			{
				if (!NameVerification.Validate(name, 2, 16, true, false, true, 1, NameVerification.SpaceOnly))
				{
					from.SendMessage(0X22, "Esse nome é inaceitável ou já está em uso.");
					from.SendGump(new NameChangeGump(from));
					return;
				}
				if (CharacterCreation.CheckDupe(from, name))
				{
					from.SendMessage(0X22, "Teu nome agora é {0}.", name);
					from.Name = name;
					from.CantWalk = false;
					return;
				}
			}
            else
            {
                from.SendMessage(0X22, "Deves inserir um nome.");
            }

            from.SendGump(new NameChangeGump(from));
        }
    }
}

namespace Server.Items
{
	[Flipable(0x14EF, 0x14F0)]
	public class ChangeName : Item
	{
		[Constructable]
		public ChangeName( ) : base( 0x14EF )
		{
			Weight = 1.0;
			Name = "Name Change Contract";
		}

		public override void OnDoubleClick( Mobile e )
		{
			e.SendGump( new NameAlterGump( e ) );
			e.SendSound( 0x55 );
		}

		public ChangeName(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int) 0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}
	}
}