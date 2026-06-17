using Server.Accounting;
using Server.Commands.Generic;
using Server.Commands;
using Server.Guilds;
using Server.Gumps;
using Server.Items;
using Server.Misc;
using Server.Mobiles;
using Server.Network;
using Server.Regions;
using Server;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Text;
using System;

namespace Server.Misc
{
    class LoggingFunctions
    {
		public static bool LoggingEvents()
		{
			return true; // SET TO TRUE TO ENABLE LOG SYSTEM FOR GAME EVENTS AND TOWN CRIERS
		}

		public static void CreateFile(string sPath)
		{
			/// CREATE THE FILE IF IT DOES NOT EXIST ///
			StreamWriter w = null; 
			try
			{
				using (w = File.AppendText( sPath ) ){}
			}
			catch(Exception)
			{
			}
			finally
			{
				if (w != null)
					w.Dispose();
			}
		}

		public static void UpdateFile(string filename, string header)
		{
			int nLine = 0;
			int nTrim = 150;
			string tempfile = Path.GetTempFileName();
			StreamWriter writer = null;
			StreamReader reader = null;
			using (writer = new StreamWriter(tempfile))
			using (reader = new StreamReader(filename))
			{
				writer.WriteLine(header);
				while (!reader.EndOfStream)
				{
					nLine = nLine + 1;
					if ( nLine < nTrim )
					{
						writer.WriteLine(reader.ReadLine());
					}
					else
					{
						reader.ReadLine();
					}
				}
			}

			if (writer != null)
				writer.Dispose();

			if (reader != null)
				reader.Dispose();

			File.Copy(tempfile, filename, true);
			File.Delete(tempfile);
		}

		public static void DeleteFile(string filename)
		{
			try
			{
				File.Delete(filename);
			}
			catch(Exception)
			{
			}
		}

		public static string LogEvent( string sEvent, string sLog )
		{
			if ( LoggingFunctions.LoggingEvents() == true )
			{
				if ( !Directory.Exists( "Saves/Data" ) )
					Directory.CreateDirectory( "Saves/Data" );

				string sPath = "Saves/Data/adventures.txt";

				if ( sLog == "Logging Adventures" ){ sPath = "Saves/Data/adventures.txt"; }
				else if ( sLog == "Logging Quests" ){ sPath = "Saves/Data/quests.txt"; }
				else if ( sLog == "Logging Battles" ){ sPath = "Saves/Data/battles.txt"; }
				else if ( sLog == "Logging Deaths" ){ sPath = "Saves/Data/deaths.txt"; }
				else if ( sLog == "Logging Murderers" ){ sPath = "Saves/Data/murderers.txt"; }
				else if ( sLog == "Logging Journies" ){ sPath = "Saves/Data/journies.txt"; }
				else if ( sLog == "Logging Server" ){ sPath = "Saves/Data/server.txt"; }
				
				CreateFile( sPath );

				/// PREPEND THE FILE WITH THE EVENT ///
				try
				{
					UpdateFile(sPath, sEvent);
				}
				catch(Exception)
				{
				}
			}
			return null;
		}

		/////////////////////////////////////////////////////////////////////////////////////////////////////////////

		public static string LogRead( string sLog, Mobile m )
		{
			if ( !Directory.Exists( "Saves/Data" ) )
				Directory.CreateDirectory( "Saves/Data" );

			string sPath = "Saves/Data/adventures.txt";

			if ( sLog == "Logging Adventures" ){ sPath = "Saves/Data/adventures.txt"; }
			else if ( sLog == "Logging Quests" ){ sPath = "Saves/Data/quests.txt"; }
			else if ( sLog == "Logging Battles" ){ sPath = "Saves/Data/battles.txt"; }
			else if ( sLog == "Logging Deaths" ){ sPath = "Saves/Data/deaths.txt"; }
			else if ( sLog == "Logging Murderers" ){ sPath = "Saves/Data/murderers.txt"; }
			else if ( sLog == "Logging Journies" ){ sPath = "Saves/Data/journies.txt"; }

			string sBreak = "";

			if ( sLog == "Logging Murderers"){ sBreak = "<br>"; }
			string sLogEntries = "";

			CreateFile( sPath );

			string eachLine = "";
			int nLine = 0;
			int nBlank = 1;
			StreamReader reader = null;

			try
			{
				using (reader = new StreamReader( sPath ))
				{
					while (!reader.EndOfStream)
					{
						eachLine = reader.ReadLine();
						string[] eachWord = eachLine.Split('#');
						nLine = 1;
						foreach (string eachWords in eachWord)
						{
							if ( nLine == 1 ){ nLine = 2; sLogEntries = sLogEntries + eachWords + ".<br>" + sBreak; nBlank = 0; }
							else { nLine = 1; sLogEntries = sLogEntries + " - " + eachWords + "<br><br>"; }
						}
					}
				}
			}
			catch(Exception)
			{
				sLogEntries = sLogEntries + "Perdoai, " + m.Name + ". Encontro-me ocupado no momento.";
			}
			finally
			{
				if (reader != null)
					reader.Dispose();
			}

			if ( nBlank == 1 )
			{
				if ( sLog == "Logging Murderers" ){ sLogEntries = sLogEntries + "Com alegria declaro, " + m.Name + ", que ninguém é procurado por assassinato."; }
					else if ( sLog == "Logging Battles" ){ sLogEntries = sLogEntries + "Perdoai, " + m.Name + ", mas não tenho novos contos de bravura para relatar."; }
					else if ( sLog == "Logging Adventures" ){ sLogEntries = sLogEntries + "Perdoai, " + m.Name + ", mas não tenho novos rumores para partilhar."; }
					else if ( sLog == "Logging Quests" ){ sLogEntries = sLogEntries + "Perdoai, " + m.Name + ", mas não tenho novos feitos para narrar."; }
					else if ( sLog == "Logging Deaths" ){ sLogEntries = sLogEntries + "Com alegria afirmo, " + m.Name + ", que todos os cidadãos de Sosaria vivem e estão bem."; }
					else if ( sLog == "Logging Journies" ){ sLogEntries = sLogEntries + "Perdoai, " + m.Name + ", mas não tenho novas sagas de exploração para contar."; }
					else { sLogEntries = sLogEntries + "Perdoai, " + m.Name + ", mas não tenho novas informações sobre tais assuntos."; }
					}

			if ( sLogEntries.Contains(" .") ){ sLogEntries = sLogEntries.Replace(" .", "."); }
			if ( sLogEntries.Contains("..") ){ sLogEntries = sLogEntries.Replace("..", "."); }

			return sLogEntries;
		}

		/////////////////////////////////////////////////////////////////////////////////////////////////////////////

		public static string LogArticles( int article, int section )
		{
			if ( !Directory.Exists( "Info" ) )
				Directory.CreateDirectory( "Info" );

			if ( !Directory.Exists( "Info/Articles" ) )
				Directory.CreateDirectory( "Info/Articles" );

			if ( article > 10 ){ article = 0; }
			else if ( article > 0 ){}
			else { article = 0; }

			string text = article.ToString();

			string path = "Info/Articles/" + text + ".txt";

			string part = "";

			string title = "";
			string date = "";
			string message = "";

			CreateFile( path );

			StreamReader reader = null;

			int line = 0;

			try
			{
				using (reader = new StreamReader( path ))
				{
					while (!reader.EndOfStream)
					{
						if ( line == 0 ){ title = reader.ReadLine(); }
						else if ( line == 1 ){ date = reader.ReadLine(); }
						else { message = reader.ReadLine(); }

						line++;
					}
				}
			}
			catch(Exception)
			{
			}
			finally
			{
				if (reader != null)
					reader.Dispose();
			}

			if ( section == 1 ){ part = title; }
			else if ( section == 2 ){ part = date; }
			else if ( section == 3 ){ part = message; }

			if ( part.Contains(" .") ){ part = part.Replace(" .", "."); }

			return part;
		}

		/////////////////////////////////////////////////////////////////////////////////////////////////////////////

		public static int TotalLines(string filePath)
		{
			int i = 0;
			using (StreamReader r = new StreamReader(filePath)){ while (r.ReadLine() != null) { i++; } }
			return i;
		}

		public static string LogShout()
		{
			if ( !Directory.Exists( "Saves/Data" ) )
				Directory.CreateDirectory( "Saves/Data" );

			string sLog = "Logging Adventures";
			switch ( Utility.Random( 6 ))
			{
				case 0: sLog = "Logging Deaths"; break;
				case 1: sLog = "Logging Quests"; break;
				case 2: sLog = "Logging Battles"; break;
				case 3: sLog = "Logging Journies"; break;
				case 4: sLog = "Logging Murderers"; break;
				case 5: sLog = "Logging Adventures"; break;
			};

			string sPath = "Saves/Data/adventures.txt";

			if ( sLog == "Logging Adventures" ){ sPath = "Saves/Data/adventures.txt"; }
			else if ( sLog == "Logging Quests" ){ sPath = "Saves/Data/quests.txt"; }
			else if ( sLog == "Logging Battles" ){ sPath = "Saves/Data/battles.txt"; }
			else if ( sLog == "Logging Deaths" ){ sPath = "Saves/Data/deaths.txt"; }
			else if ( sLog == "Logging Murderers" ){ sPath = "Saves/Data/murderers.txt"; }
			else if ( sLog == "Logging Journies" ){ sPath = "Saves/Data/journies.txt"; }

			CreateFile( sPath );

			int lineCount = 1;
			string sGreet = "Ouça, Ouça!";
				switch ( Utility.Random( 4 ))
				{
					case 0: sGreet = "Ouça, Ouça!"; break;
					case 1: sGreet = "Todos escutem!"; break;
					case 2: sGreet = "Toda honra e escutai minhas palavras!"; break;
					case 3: sGreet = "Vossa atenção, imploro!"; break;
				};

			string myShout = "";
			if ( sLog == "Logging Murderers" ){ myShout = Server.Mobiles.TownHerald.randomShout( null ); }
			else { myShout = Server.Mobiles.TownHerald.randomShout( null ); }

			try
			{
				lineCount = TotalLines( sPath );
			}
			catch(Exception)
			{
			}

			lineCount = Utility.RandomMinMax( 1, lineCount );
			string readLine = "";
			StreamReader reader = null;
			int nWhichLine = 0;
			int nLine = 1;
			try
			{
				using (reader = new StreamReader( sPath ))
				{
					string line;

					while ((line = reader.ReadLine()) != null)
					{
						nWhichLine = nWhichLine + 1;
						if ( nWhichLine == lineCount )
						{
							readLine = line;
							string[] shoutOut = readLine.Split('#');
							foreach (string shoutOuts in shoutOut)
							{
								if ( nLine == 1 ){ nLine = 2; readLine = shoutOuts; }
							}
						}
					}
					if ( readLine != "" ){ myShout = readLine; }
				}
			}
			catch(Exception)
			{
			}
			finally
			{
				if (reader != null)
					reader.Dispose();
			}

			string sVerb1 = "";
			string sVerb2 = "";
			switch ( Utility.Random( 4 ))
			{
				case 0: sVerb1 = "foi visto(a) em";              sVerb2 = "foi visto(a) saindo de";          break;
				case 1: sVerb1 = "foi avistado(a) em";           sVerb2 = "foi avistado(a) saindo de";       break;
				case 2: sVerb1 = "sabe-se que esteve em";        sVerb2 = "foi visto(a) perto de";           break;
				case 3: sVerb1 = "havia rumores de estar em";    sVerb2 = "foi avistado(a) por";             break;
			};

			myShout = sGreet + " " + myShout + "!";
			if ( myShout.Contains(" !") ){ myShout = myShout.Replace(" !", "!"); }
			if ( myShout.Contains(" had entered ") ){ myShout = myShout.Replace(" had entered ", " " + sVerb1 + " "); }
			if ( myShout.Contains(" had left ") ){ myShout = myShout.Replace(" left ", " " + sVerb2 + " "); }

			return myShout;
		}

		/////////////////////////////////////////////////////////////////////////////////////////////////////////////

		public static string LogSpeak()
		{
			if ( !Directory.Exists( "Saves/Data" ) )
				Directory.CreateDirectory( "Saves/Data" );

			string sLog = "Logging Murderers";
			switch ( Utility.Random( 6 ))
			{
				case 0: sLog = "Logging Deaths"; break;
				case 1: sLog = "Logging Battles"; break;
				case 2: sLog = "Logging Journies"; break;
				case 3: sLog = "Logging Battles"; break;
				case 4: sLog = "Logging Journies"; break;
			};

			string sPath = "Saves/Data/murderers.txt";

			if ( sLog == "Logging Battles" ){ sPath = "Saves/Data/battles.txt"; }
			else if ( sLog == "Logging Deaths" ){ sPath = "Saves/Data/deaths.txt"; }
			else if ( sLog == "Logging Journies" ){ sPath = "Saves/Data/journies.txt"; }

			CreateFile( sPath );

			int lineCount = 1;

			string mySpeaking = "as coisas estando calmas por toda a terra";

			try
			{
				lineCount = TotalLines( sPath );
			}
			catch(Exception)
			{
			}

			lineCount = Utility.RandomMinMax( 1, lineCount );
			string readLine = "";
			StreamReader reader = null;
			int nWhichLine = 0;
			int nLine = 1;
			try
			{
				using (reader = new StreamReader( sPath ))
				{
					string line;

					while ((line = reader.ReadLine()) != null)
					{
						nWhichLine = nWhichLine + 1;
						if ( nWhichLine == lineCount )
						{
							readLine = line;
							string[] shoutOut = readLine.Split('#');
							foreach (string shoutOuts in shoutOut)
							{
								if ( nLine == 1 ){ nLine = 2; readLine = shoutOuts; }
							}
						}
					}
					if ( readLine != "" ){ mySpeaking = readLine; }
				}
			}
			catch(Exception)
			{
			}
			finally
			{
				if (reader != null)
					reader.Dispose();
			}

			string sVerb1 = "";
			string sVerb2 = "";
			string sVerb3 = "";
			switch ( Utility.Random( 4 ))
			{
				case 0: sVerb1 = "sendo visto(a) em";       sVerb2 = "sendo visto(a) saindo de";    sVerb3 = "matando";     break;
				case 1: sVerb1 = "sendo avistado(a) em";    sVerb2 = "sendo avistado(a) saindo de"; sVerb3 = "abatendo";    break;
				case 2: sVerb1 = "sendo visto(a) em";       sVerb2 = "sendo visto(a) perto de";     sVerb3 = "derrotando";  break;
				case 3: sVerb1 = "sendo avistado(a) em";    sVerb2 = "sendo avistado(a) por";       sVerb3 = "abatendo";    break;
			};

			if ( mySpeaking.Contains(" had been ") ){ mySpeaking = mySpeaking.Replace(" had been ", " sendo "); }
			if ( mySpeaking.Contains(" had slain ") ){ mySpeaking = mySpeaking.Replace(" had slain ", " " + sVerb3 + " "); }
			if ( mySpeaking.Contains(" had killed ") ){ mySpeaking = mySpeaking.Replace(" had killed ", " matando acidentalmente "); }
			if ( mySpeaking.Contains(" made a fatal mistake ") ){ mySpeaking = mySpeaking.Replace(" made a fatal mistake ", " cometendo um erro fatal "); }
			if ( mySpeaking.Contains(" entered ") ){ mySpeaking = mySpeaking.Replace(" entered ", " " + sVerb1 + " "); }
			if ( mySpeaking.Contains(" left ") ){ mySpeaking = mySpeaking.Replace(" left ", " " + sVerb2 + " "); }
						
			return mySpeaking;
		}

		public static string LogSpeakQuest()
		{
			if ( !Directory.Exists( "Saves/Data" ) )
				Directory.CreateDirectory( "Saves/Data" );

			string sPath = "Saves/Data/quests.txt";

			CreateFile( sPath );

			int lineCount = 1;

			string mySpeaking = "Os aventureiros parecem estar todos sentados em tavernas";

			try
			{
				lineCount = TotalLines( sPath );
			}
			catch(Exception)
			{
			}

			lineCount = Utility.RandomMinMax( 1, lineCount );
			string readLine = "";
			StreamReader reader = null;
			int nWhichLine = 0;
			int nLine = 1;
			try
			{
				using (reader = new StreamReader( sPath ))
				{
					string line;

					while ((line = reader.ReadLine()) != null)
					{
						nWhichLine = nWhichLine + 1;
						if ( nWhichLine == lineCount )
						{
							readLine = line;
							string[] shoutOut = readLine.Split('#');
							foreach (string shoutOuts in shoutOut)
							{
								if ( nLine == 1 ){ nLine = 2; readLine = shoutOuts; }
							}
						}
					}
					if ( readLine != "" ){ mySpeaking = readLine; }
				}
			}
			catch(Exception)
			{
			}
			finally
			{
				if (reader != null)
					reader.Dispose();
			}
						
			return mySpeaking;
		}

		/////////////////////////////////////////////////////////////////////////////////////////////////////////////

		public static string LogRegions( Mobile m, string sRegion, string sDirection )
		{
			if ( m is PlayerMobile )
			{
				int nDifficulty = Server.Difficult.GetDifficulty( m.Location, m.Map );
				string sDifficulty = "";

				if ( nDifficulty == -1 ){ sDifficulty = " (Easy)"; }
				else if ( nDifficulty == 0 ){ sDifficulty = " (Normal)"; }
				else if ( nDifficulty == 1 ){ sDifficulty = " (Difficult)"; }
				else if ( nDifficulty == 2 ){ sDifficulty = " (Challenging)"; }
				else if ( nDifficulty == 3 ){ sDifficulty = " (Hard)"; }
				else if ( nDifficulty == 4 ){ sDifficulty = " (Deadly)"; }
				else if ( nDifficulty > 4 ){ sDifficulty = " (Epic)"; }

				if ( sDirection == "enter" ){ m.SendMessage("Você entrou em " + sRegion + sDifficulty + "."); }
				else { m.SendMessage("Você saiu de " + sRegion + "."); }
			}

			if ( ( m is PlayerMobile ) && ( m.AccessLevel < AccessLevel.GameMaster ) )
			{
				if ( !m.Alive && m.QuestArrow == null ){ GhostHelper.OnGhostWalking( m ); }
				string sDateString = GetPlayerInfo.GetTodaysDate();
				string sTitle = "the " + GetPlayerInfo.GetSkillTitle( m );
				if ( m.Title != null ){ sTitle = m.Title; }

				PlayerMobile pm = (PlayerMobile)m;
				if (pm.PublicInfo == true)
				{
					string sEvent;

					if ( sDirection == "enter" ){ sEvent = m.Name + " " + sTitle + " entered " + sRegion + "#" + sDateString; LoggingFunctions.LogEvent( sEvent, "Logging Journies" ); }
					// else { sEvent = m.Name + " " + sTitle + " left " + sRegion + "#" + sDateString; LoggingFunctions.LogEvent( sEvent, "Logging Journies" ); }
				}
			}
			return null;
		}

		/////////////////////////////////////////////////////////////////////////////////////////////////////////////

	public static string LogBattles( Mobile m, Mobile mob )
		{
		    if ( m == null || mob == null )
		        return null;

		    if ( !(m is PlayerMobile) )
		        return null;

		    string sDateString = GetPlayerInfo.GetTodaysDate();

		    string sTitle = "the " + GetPlayerInfo.GetSkillTitle( m );
		    if ( m.Title != null ){ sTitle = m.Title; }

		    PlayerMobile pm = (PlayerMobile)m;

		    if ( mob.Name == null )
		        return null;

		    string sKiller = mob.Name;
		    string[] eachWord = sKiller.Split('[');
		    int nLine = 1;
		    foreach (string eachWords in eachWord)
		    {
		        if ( nLine == 1 ){ nLine = 2; sKiller = eachWords; }
		    }
		    sKiller = sKiller.TrimEnd();

		    if ( mob is BaseCreature && ( mob.Fame > -2000 && mob.Fame < 2000 ) )
		    {
		        // NOT WORTH RECORDING OTHERWISE YOU GET A BATTLE LOG FULL OF GOAT OR RABBIT SLAYINGS...OR BASICALLY EASY MONSTERS
		        return null;
		    }
		    else if ( pm.PublicInfo == true )
		    {
		        string Killed = sKiller;
		        // Check for null BEFORE checking for empty string
		        if ( mob.Title != null && mob.Title != "" ){ Killed = Killed + " " + mob.Title; }

		        if ( m.Name == null )
		            return null;

		        string sEvent = m.Name + " " + sTitle + " abateu " + Killed + "#" + sDateString;
		        LoggingFunctions.LogEvent( sEvent, "Logging Battles" );
		    }
		    else
		    {
		        string privateEnemy = "um oponente";
				switch ( Utility.Random( 6 ) )
				{
					case 0: privateEnemy = "um oponente"; break;
					case 1: privateEnemy = "um inimigo"; break;
					case 2: privateEnemy = "um outro"; break;
					case 3: privateEnemy = "um adversário"; break;
					case 4: privateEnemy = "um inimigo"; break;
					case 5: privateEnemy = "um rival"; break;
				}

		        if ( m.Name == null )
		            return null;

		        string sEvent = m.Name + " " + sTitle + " abateu " + privateEnemy + "#" + sDateString;
		        LoggingFunctions.LogEvent( sEvent, "Logging Battles" );
		    }

		    return null;
		}

		/////////////////////////////////////////////////////////////////////////////////////////////////////////////

		public static string LogTraps( Mobile m, string sTrap )
		{
			string sDateString = GetPlayerInfo.GetTodaysDate();
			string sTitle = "the " + GetPlayerInfo.GetSkillTitle( m );
			if ( m.Title != null ){ sTitle = m.Title; }

			string sTrip = "ativou";
			switch( Utility.Random( 7 ) )
			{
				case 0: sTrip = "ativou";	break;
				case 1: sTrip = "acionou";	break;
				case 2: sTrip = "entrou em";	break;
				case 3: sTrip = "tropeçou em";	break;
				case 4: sTrip = "foi atingido(a) por";	break;
				case 5: sTrip = "foi afetado(a) por";	break;
				case 6: sTrip = "colidiu com";	break;
			}

			PlayerMobile pm = (PlayerMobile)m;
			if (pm.PublicInfo == true)
			{
				string sEvent = m.Name + " " + sTitle + " " + sTrip + " " + sTrap + "#" + sDateString;
				LoggingFunctions.LogEvent( sEvent, "Logging Adventures" );
			}

			return null;
		}

		/////////////////////////////////////////////////////////////////////////////////////////////////////////////

		public static string LogVoid( Mobile m, string sTrap )
		{
			string sDateString = GetPlayerInfo.GetTodaysDate();
			string sTitle = "the " + GetPlayerInfo.GetSkillTitle( m );
			if ( m.Title != null ){ sTitle = m.Title; }

			PlayerMobile pm = (PlayerMobile)m;
			if (pm.PublicInfo == true)
			{
				string sEvent = m.Name + " " + sTitle + " " + sTrap + ", teleportando-os para longe#" + sDateString;
				LoggingFunctions.LogEvent( sEvent, "Logging Adventures" );
			}

			return null;
		}

		/////////////////////////////////////////////////////////////////////////////////////////////////////////////

		public static string LogPrison( Mobile m, string sJail )
		{
			string sDateString = GetPlayerInfo.GetTodaysDate();
			string sTitle = "the " + GetPlayerInfo.GetSkillTitle( m );
			if ( m.Title != null ){ sTitle = m.Title; }

			PlayerMobile pm = (PlayerMobile)m;
			if (pm.PublicInfo == true)
			{
				string sEvent = m.Name + " " + sTitle + " foi enviado(a) para " + sJail + "#" + sDateString;
				LoggingFunctions.LogEvent( sEvent, "Logging Journies" );
			}

			return null;
		}

		/////////////////////////////////////////////////////////////////////////////////////////////////////////////

		public static string LogKillTile( Mobile m, string sTrap )
		{
			string sDateString = GetPlayerInfo.GetTodaysDate();
			string sTitle = "the " + GetPlayerInfo.GetSkillTitle( m );
			if ( m.Title != null ){ sTitle = m.Title; }

			PlayerMobile pm = (PlayerMobile)m;
			if (pm.PublicInfo == true)
			{
				string sEvent = m.Name + " " + sTitle + " cometeu um erro fatal por causa de " + sTrap + "#" + sDateString;
				LoggingFunctions.LogEvent( sEvent, "Logging Journies" );
			}

			return null;
		}

		/////////////////////////////////////////////////////////////////////////////////////////////////////////////

		public static string LogLoot( Mobile m, string sBox, string sType )
		{
			string sDateString = GetPlayerInfo.GetTodaysDate();
			string sTitle = "the " + GetPlayerInfo.GetSkillTitle( m );
			if ( m.Title != null ){ sTitle = m.Title; }

			string sLoot = "havia vasculhado um(a)";
			switch( Utility.Random( 7 ) )
			{
				case 0: sLoot = "havia vasculhado um(a)";	break;
				case 1: sLoot = "havia encontrado um(a)";	break;
				case 2: sLoot = "havia descoberto um(a)";	break;
				case 3: sLoot = "havia examinado um(a)";	break;
				case 4: sLoot = "havia se deparado com um(a)";	break;
				case 5: sLoot = "havia escavado um(a)";	break;
				case 6: sLoot = "havia aberto um(a)";	break;
			}
			if ( sType == "boat" )
			{
				switch( Utility.Random( 5 ) )
				{
					case 0: sLoot = "havia vasculhado um(a)";	break;
					case 1: sLoot = "havia encontrado um(a)";	break;
					case 2: sLoot = "havia descoberto um(a)";	break;
					case 3: sLoot = "havia examinado um(a)";	break;
					case 4: sLoot = "havia navegado em um(a)";	break;
				}
				if ( sBox.Contains("Abandoned") || sBox.Contains("Adrift") ){ sLoot = sLoot + "n"; }
			}
			else if ( sType == "corpse" )
			{
				switch( Utility.Random( 5 ) )
				{
					case 0: sLoot = "havia vasculhado um(a)";	break;
					case 1: sLoot = "havia encontrado um(a)";	break;
					case 2: sLoot = "havia descoberto um(a)";	break;
					case 3: sLoot = "havia examinado um(a)";	break;
					case 4: sLoot = "havia mexido em um(a)";	break;
				}
				if ( sBox.Contains("Abandoned") || sBox.Contains("Adrift") ){ sLoot = sLoot + "n"; }
			}

			PlayerMobile pm = (PlayerMobile)m;
			if (pm.PublicInfo == true)
			{
				string sEvent = m.Name + " " + sTitle + " " + sLoot + " " + sBox + "#" + sDateString;
				LoggingFunctions.LogEvent( sEvent, "Logging Adventures" );
			}

			return null;
		}

		/////////////////////////////////////////////////////////////////////////////////////////////////////////////

		public static string LogSlayingLord( Mobile m, string creature )
		{
			if ( m != null )
			{
				if ( m is BaseCreature )
					m = ((BaseCreature)m).GetMaster();

				if ( m is PlayerMobile )
				{
					string sDateString = GetPlayerInfo.GetTodaysDate();
					string sTitle = "the " + GetPlayerInfo.GetSkillTitle( m );
					if ( m.Title != null ){ sTitle = m.Title; }

					string verb = "destruiu";
					switch( Utility.Random( 4 ) )
					{
						case 0: verb = "derrotou";		break;
						case 1: verb = "abateu";		break;
						case 2: verb = "destruiu";	break;
						case 3: verb = "aniquilou";	break;
					}

					PlayerMobile pm = (PlayerMobile)m;
					if (pm.PublicInfo == true)
					{
						string sEvent = m.Name + " " + sTitle + " " + verb + " " + creature + "#" + sDateString;
						LoggingFunctions.LogEvent( sEvent, "Logging Quests" );
					}
				}
			}

			return null;
		}
		// --------------------------------------------------------------------------------------------
		public static string LogCreatedArtifact( Mobile m, string sArty )
		{
			string sDateString = GetPlayerInfo.GetTodaysDate();

			PlayerMobile pm = (PlayerMobile)m;
			if (pm.PublicInfo == true)
			{
				string sEvent = "Os deuses criaram um artefato lendário chamado " + sArty + "#" + sDateString;
				LoggingFunctions.LogEvent( sEvent, "Logging Quests" );
			}

			return null;
		}
		// --------------------------------------------------------------------------------------------
		public static string LogRuneOfVirtue( Mobile m, string side )
		{
			string sDateString = GetPlayerInfo.GetTodaysDate();
			string sTitle = "the " + GetPlayerInfo.GetSkillTitle( m );
			if ( m.Title != null ){ sTitle = m.Title; }

			string sText = "purificou as Runas da Câmara da Virtude.";
			if ( side == "evil" ){ sText = "corrompeu as Runas da Virtude."; }

			PlayerMobile pm = (PlayerMobile)m;
			if (pm.PublicInfo == true)
			{
				string sEvent = m.Name + " " + sTitle + " " + sText + "#" + sDateString;
				LoggingFunctions.LogEvent( sEvent, "Logging Quests" );
			}

			return null;
		}
		// --------------------------------------------------------------------------------------------
		public static string LogGenericQuest( Mobile m, string sText )
		{
			string sDateString = GetPlayerInfo.GetTodaysDate();
			string sTitle = "the " + GetPlayerInfo.GetSkillTitle( m );
			if ( m.Title != null ){ sTitle = m.Title; }

			PlayerMobile pm = (PlayerMobile)m;
			if (pm.PublicInfo == true)
			{
				string sEvent = m.Name + " " + sTitle + " " + sText + "#" + sDateString;
				LoggingFunctions.LogEvent( sEvent, "Logging Quests" );
			}

			return null;
		}
		// --------------------------------------------------------------------------------------------
		public static string LogFoundItemQuest( Mobile m, string sBox )
		{
			string sDateString = GetPlayerInfo.GetTodaysDate();
			string sTitle = "the " + GetPlayerInfo.GetSkillTitle( m );
			if ( m.Title != null ){ sTitle = m.Title; }

			string sLoot = "descobriu o(a)";
			switch( Utility.Random( 4 ) )
			{
				case 0: sLoot = "encontrou o(a)";		break;
				case 1: sLoot = "recuperou o(a)";	break;
				case 2: sLoot = "desenterrou o(a)";	break;
				case 3: sLoot = "descobriu o(a)";	break;
			}

			PlayerMobile pm = (PlayerMobile)m;
			if (pm.PublicInfo == true)
			{
				string sEvent = m.Name + " " + sTitle + " " + sLoot + " " + sBox + "#" + sDateString;
				LoggingFunctions.LogEvent( sEvent, "Logging Quests" );
			}

			return null;
		}
		// --------------------------------------------------------------------------------------------
		public static string LogScammedBySage( Mobile m, string sBox )
		{
			string sDateString = GetPlayerInfo.GetTodaysDate();
			string sTitle = "the " + GetPlayerInfo.GetSkillTitle( m );
			if ( m.Title != null ){ sTitle = m.Title; }

			string sLoot = "registrou uma queixa contra a ordem dos sábios devido a informações enganosas sobre o(a)";

			PlayerMobile pm = (PlayerMobile)m;
			if (pm.PublicInfo == true)
			{
				string sEvent = m.Name + " " + sTitle + " " + sLoot + " !"+"#" + sDateString;
				LoggingFunctions.LogEvent( sEvent, "Logging Quests" );
			}

			return null;
		}
		// --------------------------------------------------------------------------------------------
		public static string LogQuestItem( Mobile m, string sBox )
		{
			string sDateString = GetPlayerInfo.GetTodaysDate();
			string sTitle = "the " + GetPlayerInfo.GetSkillTitle( m );
			if ( m.Title != null ){ sTitle = m.Title; }

			string sLoot = "descobriu";
			switch( Utility.Random( 4 ) )
			{
				case 0: sLoot = "encontrou";		break;
				case 1: sLoot = "recuperou";	break;
				case 2: sLoot = "desenterrou";	break;
				case 3: sLoot = "descobriu";	break;
			}

			PlayerMobile pm = (PlayerMobile)m;
			if (pm.PublicInfo == true)
			{
				string sEvent = m.Name + " " + sTitle + " " + sLoot + " " + sBox + "#" + sDateString;
				LoggingFunctions.LogEvent( sEvent, "Logging Quests" );
			}

			return null;
		}
		// --------------------------------------------------------------------------------------------
		public static string LogQuestBody( Mobile m, string sBox )
		{
			string sDateString = GetPlayerInfo.GetTodaysDate();
			string sTitle = "the " + GetPlayerInfo.GetSkillTitle( m );
			if ( m.Title != null ){ sTitle = m.Title; }

			string sLoot = "encontrou";
			switch( Utility.Random( 4 ) )
			{
				case 0: sLoot = "encontrou";		break;
				case 1: sLoot = "recuperou";	break;
				case 2: sLoot = "desenterrou";	break;
				case 3: sLoot = "escavou";		break;
			}

			string sBone = "os ossos";
			switch( Utility.Random( 4 ) )
			{
				case 0: sBone = "os ossos";		break;
				case 1: sBone = "o corpo";			break;
				case 2: sBone = "os restos";		break;
				case 3: sBone = "o cadáver";		break;
			}

			PlayerMobile pm = (PlayerMobile)m;
			if (pm.PublicInfo == true)
			{
				string sEvent = m.Name + " " + sTitle + " " + sLoot + " " + sBone + " of " + sBox + "#" + sDateString;
				LoggingFunctions.LogEvent( sEvent, "Logging Quests" );
			}

			return null;
		}
		// --------------------------------------------------------------------------------------------
		public static string LogQuestChest( Mobile m, string sBox )
		{
			string sDateString = GetPlayerInfo.GetTodaysDate();
			string sTitle = "the " + GetPlayerInfo.GetSkillTitle( m );
			if ( m.Title != null ){ sTitle = m.Title; }

			string sLoot = "encontrou";
			switch( Utility.Random( 4 ) )
			{
				case 0: sLoot = "encontrou";		break;
				case 1: sLoot = "recuperou";	break;
				case 2: sLoot = "desenterrou";	break;
				case 3: sLoot = "escavou";		break;
			}

			string sChest = "o baú escondido";
			switch( Utility.Random( 4 ) )
			{
				case 0: sChest = "o escondido";		break;
				case 1: sChest = "o perdido";		break;
				case 2: sChest = "o desaparecido";		break;
				case 3: sChest = "o secreto";		break;
			}

			PlayerMobile pm = (PlayerMobile)m;
			if (pm.PublicInfo == true)
			{
				string sEvent = m.Name + " " + sTitle + " " + sLoot + " " + sChest + " baú de " + sBox + "#" + sDateString;
				LoggingFunctions.LogEvent( sEvent, "Logging Quests" );
			}

			return null;
		}
		// --------------------------------------------------------------------------------------------
		public static string LogQuestMap( Mobile m, int sLevel, string chest )
		{
			string sDateString = GetPlayerInfo.GetTodaysDate();
			string sTitle = "the " + GetPlayerInfo.GetSkillTitle( m );
			if ( m.Title != null ){ sTitle = m.Title; }

			string sLoot = "encontrou";
			switch( Utility.Random( 4 ) )
			{
				case 0: sLoot = "encontrou";		break;
				case 1: sLoot = "recuperou";	break;
				case 2: sLoot = "desenterrou";	break;
				case 3: sLoot = "escavou";		break;
			}

			PlayerMobile pm = (PlayerMobile)m;
			if (pm.PublicInfo == true)
			{
				string sEvent = m.Name + " " + sTitle + " " + sLoot + " " + chest + "#" + sDateString;
				LoggingFunctions.LogEvent( sEvent, "Logging Quests" );
			}

			return null;
		}
		// --------------------------------------------------------------------------------------------
		public static string LogQuestSea( Mobile m, int sLevel, string sShip )
		{
			string sDateString = GetPlayerInfo.GetTodaysDate();
			string sTitle = "the " + GetPlayerInfo.GetSkillTitle( m );
			if ( m.Title != null ){ sTitle = m.Title; }

			string sLoot = "pescou";
			switch( Utility.Random( 4 ) )
			{
				case 0: sLoot = "trouxe à superfície";		break;
				case 1: sLoot = "resgatou";		break;
				case 2: sLoot = "trouxe para cima";	break;
				case 3: sLoot = "pescou";	break;
			}

			string sChest = "um grande baú submerso";
			switch( sLevel )
			{
				case 0: sChest = "um baú submerso modesto";		break;
				case 1: sChest = "um baú submerso simples";		break;
				case 2: sChest = "um baú submerso bom";			break;
				case 3: sChest = "um baú submerso ótimo";		break;
				case 4: sChest = "um baú submerso excelente";	break;
				case 5: sChest = "um baú submerso soberbo";		break;
			}

			PlayerMobile pm = (PlayerMobile)m;
			if (pm.PublicInfo == true)
			{
				string sEvent = m.Name + " " + sTitle + " " + sLoot + " " + sChest + " de " + sShip + "#" + sDateString;
				LoggingFunctions.LogEvent( sEvent, "Logging Quests" );
			}

			return null;
		}
		// --------------------------------------------------------------------------------------------
		public static string LogQuestKill( Mobile m, string sBox, Mobile t )
		{
			string sDateString = GetPlayerInfo.GetTodaysDate();
			string sTitle = "the " + GetPlayerInfo.GetSkillTitle( m );
			if ( m.Title != null ){ sTitle = m.Title; }

			string sLoot = "";
			string sWho = "";
			
			if ( sBox == "bounty" )
			{
				sWho = "";
				switch( Utility.Random( 4 ) )
				{
					case 0: sLoot = "cumpriu uma recompensa por";	break;
					case 1: sLoot = "reivindicou uma recompensa por";		break;
					case 2: sLoot = "cumpriu uma sentença por";		break;
					case 3: sLoot = "completou uma recompensa por";	break;
				}
			}
			if ( sBox == "sea" )
			{
				sWho = " em alto-mar";
				switch( Utility.Random( 4 ) )
				{
					case 0: sLoot = "cumpriu uma recompensa por";	break;
					case 1: sLoot = "reivindicou uma recompensa por";		break;
					case 2: sLoot = "cumpriu uma sentença por";		break;
					case 3: sLoot = "completou uma recompensa por";	break;
				}
			}
			if ( sBox == "assassin" )
			{
				sWho = " para a guilda";
				switch( Utility.Random( 4 ) )
				{
					case 0: sLoot = "assassinou";		break;
					case 1: sLoot = "eliminou";		break;
					case 2: sLoot = "resolveu a situação com";		break;
					case 3: sLoot = "aniquilou";		break;
				}
			}
			
			sLoot = sLoot + " " + t.Name + " " + t.Title;

			PlayerMobile pm = (PlayerMobile)m;
			if (pm.PublicInfo == true)
			{
				string sEvent = m.Name + " " + sTitle + " " + sLoot + sWho + "#" + sDateString;
				LoggingFunctions.LogEvent( sEvent, "Logging Quests" );
			}

			return null;
		}

		/////////////////////////////////////////////////////////////////////////////////////////////////////////////

		public static string LogGeneric( Mobile m, string sText )
		{
			string sDateString = GetPlayerInfo.GetTodaysDate();
			string sTitle = "the " + GetPlayerInfo.GetSkillTitle( m );
			if ( m.Title != null ){ sTitle = m.Title; }

			PlayerMobile pm = (PlayerMobile)m;
			if (pm.PublicInfo == true)
			{
				string sEvent = m.Name + " " + sTitle + " " + sText + "#" + sDateString;
				LoggingFunctions.LogEvent( sEvent, "Logging Quests" );
			}

			return null;
		}

		/////////////////////////////////////////////////////////////////////////////////////////////////////////////

		public static string LogStandard( Mobile m, string sText )
		{
			string sDateString = GetPlayerInfo.GetTodaysDate();
			string sTitle = "the " + GetPlayerInfo.GetSkillTitle( m );
			if ( m.Title != null ){ sTitle = m.Title; }

			PlayerMobile pm = (PlayerMobile)m;
			if (pm.PublicInfo == true)
			{
				string sEvent = m.Name + " " + sTitle + " " + sText + "#" + sDateString;
				LoggingFunctions.LogEvent( sEvent, "Logging Adventures" );
			}

			return null;
		}
		
		/////////////////////////////////////////////////////////////////////////////////////////////////////////////

		public static string LogAccess( Mobile m, string sAccess )
		{
			PlayerMobile pm = (PlayerMobile)m;
			string sDateString = GetPlayerInfo.GetTodaysDate();
			string sTitle = "the " + GetPlayerInfo.GetSkillTitle( m );
			if ( m.Title != null ){ sTitle = m.Title; }

            if ( m.AccessLevel < AccessLevel.GameMaster )
            {
				m.ResetInn();
				string sEvent;
				if ( sAccess == "login" )
				{
					sEvent = m.Name + " " + sTitle + " entrou no reino#" + sDateString;
					World.Broadcast(0x35, true, "{0} {1} entrou no reino", m.Name, sTitle);
				}
				else
				{
					sEvent = m.Name + " " + sTitle + " saiu do reino#" + sDateString;
					World.Broadcast(0x35, true, "{0} {1} saiu do reino", m.Name, sTitle);
				}

				LoggingFunctions.LogEvent( sEvent, "Logging Adventures" );
            }

			return null;
		}

		/////////////////////////////////////////////////////////////////////////////////////////////////////////////

		public static string LogDeaths( Mobile m, Mobile mob )
		{
			if ( m is PlayerMobile && mob != null )
			{
				PlayerMobile pm = (PlayerMobile)m;
				string sDateString = GetPlayerInfo.GetTodaysDate();
				string sTitle = "the " + GetPlayerInfo.GetSkillTitle( m );
				if ( m.Title != null ){ sTitle = m.Title; }

				string sKiller = mob.Name;
				string[] eachWord = sKiller.Split('[');
				int nLine = 1;
				foreach (string eachWords in eachWord)
				{
					if ( nLine == 1 ){ nLine = 2; sKiller = eachWords; }
				}
				sKiller = sKiller.TrimEnd();

				///////// PLAYER DIED SO DO SINGLE FILES //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
				if ( m.AccessLevel < AccessLevel.GameMaster )
				{
					string sEvent = "";

					if ( pm.PublicInfo == true )
					{
						if ( ( mob == m ) && ( mob != null ) )
						{
							sEvent = m.Name + " " + sTitle + " se matou#" + sDateString;
						}
						else if ( ( mob != null ) && ( mob is PlayerMobile ) )
						{
							string kTitle = " o " + GetPlayerInfo.GetSkillTitle( mob );
							if ( mob.Title != null ){ kTitle = " " + mob.Title; }
							sEvent = m.Name + " " + sTitle + " foi morto(a) por " + sKiller + kTitle + "#" + sDateString;
						}
						else if ( mob != null )
						{
							string kTitle = "";
							if ( mob.Title != null ){ kTitle = " " + mob.Title; }
							sEvent = m.Name + " " + sTitle + " foi morto(a) por " + sKiller + kTitle + "#" + sDateString;
						}
						else
						{
							sEvent = m.Name + " " + sTitle + " foi morto(a)#" + sDateString;
						}
					}
					else
					{
						string privateEnemy = "um oponente";
						switch ( Utility.Random( 6 ) )
						{
							case 0: privateEnemy = "um oponente"; break;
							case 1: privateEnemy = "um inimigo"; break;
							case 2: privateEnemy = "um outro"; break;
							case 3: privateEnemy = "um adversário"; break;
							case 4: privateEnemy = "um inimigo"; break;
							case 5: privateEnemy = "um rival"; break;
						}

						if ( ( mob == m ) && ( mob != null ) )
						{
							sEvent = m.Name + " " + sTitle + " se matou#" + sDateString;
						}
						else if ( ( mob != null ) && ( mob is PlayerMobile ) )
						{
							string kTitle = " o " + GetPlayerInfo.GetSkillTitle( mob );
							if ( mob.Title != null ){ kTitle = mob.Title; }
							sEvent = m.Name + " " + sTitle + " foi morto(a) por " + sKiller + " " + kTitle + "#" + sDateString;
						}
						else if ( mob != null )
						{
							sEvent = m.Name + " " + sTitle + " foi morto(a) por " + privateEnemy + "#" + sDateString;
						}
						else
						{
							sEvent = m.Name + " " + sTitle + " foi morto(a)#" + sDateString;
						}
					}
					LoggingFunctions.LogEvent( sEvent, "Logging Deaths" );
				}
			}
			return null;
		}

		/////////////////////////////////////////////////////////////////////////////////////////////////////////////

		public static string LogKillers( Mobile m, int nKills )
		{
			string sEvent = "";
			string sDateString = GetPlayerInfo.GetTodaysDate();
			string sTitle = "the " + GetPlayerInfo.GetSkillTitle( m );
			if ( m.Title != null ){ sTitle = m.Title; }

			if ( m.Kills > 1){ sEvent = m.Name + " " + sTitle + " é procurado pelo assassinato de " + m.Kills + " pessoas."; }
			else if ( m.Kills > 0){ sEvent = m.Name + " " + sTitle + " é procurado por assassinato."; }

			LoggingFunctions.LogEvent( sEvent, "Logging Murderers" );

			return null;
		}

		/////////////////////////////////////////////////////////////////////////////////////////////////////////////

		public static string LogClear( string sLog )
		{
			string sPath = "Saves/Data/adventures.txt";
 
			if ( sLog == "Logging Adventures" ){ sPath = "Saves/Data/adventures.txt"; }
			else if ( sLog == "Logging Battles" ){ sPath = "Saves/Data/battles.txt"; }
			else if ( sLog == "Logging Deaths" ){ sPath = "Saves/Data/deaths.txt"; }
			else if ( sLog == "Logging Murderers" ){ sPath = "Saves/Data/murderers.txt"; }
			else if ( sLog == "Logging Journies" ){ sPath = "Saves/Data/journies.txt"; }

			DeleteFile( sPath );

			return null;
		}
	}
}

namespace Server.Misc
{
	public class StatusPage : Timer
	{
		public static bool Enabled = true;

		public static void Initialize()
		{
			if ( Enabled )
				new StatusPage().Start();
		}

		public StatusPage() : base( TimeSpan.FromSeconds( 5.0 ), TimeSpan.FromSeconds( 60.0 ) )
		{
			Priority = TimerPriority.FiveSeconds;
		}

		private static string Encode( string input )
		{
			StringBuilder sb = new StringBuilder( input );

			sb.Replace( "&", "&amp;" );
			sb.Replace( "<", "&lt;" );
			sb.Replace( ">", "&gt;" );
			sb.Replace( "\"", "&quot;" );
			sb.Replace( "'", "&apos;" );

			return sb.ToString();
		}

		protected override void OnTick()
		{
			if ( !Directory.Exists( "Saves/Data" ) )
				Directory.CreateDirectory( "Saves/Data" );

			LoggingFunctions.CreateFile( "Saves/Data/online.txt" );

			using ( StreamWriter op = new StreamWriter( "Saves/Data/online.txt" ) )
			{
				foreach ( NetState state in NetState.Instances )
				{
					Mobile m = state.Mobile;

					if ( m != null && ( m.AccessLevel < AccessLevel.GameMaster ) )
					{
						op.Write( Encode( m.Name ) );
						op.Write( " the " );
						op.Write( GetPlayerInfo.GetSkillTitle( m ) );
						op.Write( "\n" );
					}
				}
			}

			if ( LoggingFunctions.LoggingEvents() == true )
			{
				LoggingFunctions.LogClear( "Logging Murderers" );

				// GET ALL OF THE MURDERERS ///////////////////////////////
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

						if ( ( m.Kills > 0 ) && (m.AccessLevel < AccessLevel.GameMaster) )
						{
							LoggingFunctions.LogKillers( m, m.Kills );
						}

						++index;
					}
				}
			}
		}
	}
}

namespace Server.Gumps
{
	public class LoggingGumpCrier : Gump
	{
        public LoggingGumpCrier( Mobile from, int page ) : base( 50, 50 )
        {
			from.SendSound( 0x4A ); 
			string color = "#aecdf6";
			string sEvents = "";
			bool scroll = false;

            this.Closable=true;
			this.Disposable=true;
			this.Dragable=true;
			this.Resizable=false;

			AddPage(0);

			AddImage(0, 0, 7018, Server.Misc.PlayerSettings.GetGumpHue( from ));

			AddHtml( 12, 12, 835, 20, @"<BODY><BASEFONT Color=" + color + ">AS NOTÍCIAS DO PREGOEIRO</BASEFONT></BODY>", (bool)false, (bool)false);

			AddButton(879, 10, 4017, 4017, 0, GumpButtonType.Reply, 0);

			int btn1 = 3609;
			int btn2 = 3609;
			int btn3 = 3609;
			int btn4 = 3609;
			int btn5 = 3609;
			int btn6 = 3609;

			if ( page == 2 )
			{
				sEvents = "Feitos no Reino<br><br>" + LoggingFunctions.LogRead( "Logging Quests", from ); scroll = true; btn1 = 4011;
			}
			else if ( page == 3 )
			{
				sEvents = "Explorações no Reino<br><br>" + LoggingFunctions.LogRead( "Logging Journies", from ); scroll = true; btn2 = 4011;
			}
			else if ( page == 4 )
			{
				sEvents = "Vitórias no Reino<br><br>" + LoggingFunctions.LogRead( "Logging Battles", from ); scroll = true; btn3 = 4011;
			}
			else if ( page == 5 )
			{
				sEvents = "Mortes Recentes no Reino<br><br>" + LoggingFunctions.LogRead( "Logging Deaths", from ); scroll = true; btn4 = 4011;
			}
			else if ( page == 6 )
			{
				sEvents = "Assassinos no Reino<br><br>" + LoggingFunctions.LogRead( "Logging Murderers", from ); scroll = true; btn5 = 4011;
			}
			else if ( page == 7 )
			{
				sEvents = "Fofocas no Reino<br><br>" + LoggingFunctions.LogRead( "Logging Adventures", from ); scroll = true; btn6 = 4011;
			}

			AddButton(12, 48, btn1, btn1, 1, GumpButtonType.Reply, 0);
			AddHtml( 52, 50, 185, 20, @"<BODY><BASEFONT Color=" + color + ">Feitos no Reino</BASEFONT></BODY>", (bool)false, (bool)false);

			AddButton(344, 49, btn2, btn2, 2, GumpButtonType.Reply, 0);
			AddHtml( 384, 51, 185, 20, @"<BODY><BASEFONT Color=" + color + ">Explorações no Reino</BASEFONT></BODY>", (bool)false, (bool)false);

			AddButton(676, 50, btn3, btn3, 3, GumpButtonType.Reply, 0);
			AddHtml( 716, 52, 185, 20, @"<BODY><BASEFONT Color=" + color + ">Vitórias em Batalha</BASEFONT></BODY>", (bool)false, (bool)false);

			AddButton(12, 77, btn6, btn6, 6, GumpButtonType.Reply, 0);
			AddHtml( 52, 79, 185, 20, @"<BODY><BASEFONT Color=" + color + ">Fofocas no Reino</BASEFONT></BODY>", (bool)false, (bool)false);

			AddButton(344, 78, btn4, btn4, 4, GumpButtonType.Reply, 0);
			AddHtml( 384, 80, 185, 20, @"<BODY><BASEFONT Color=" + color + ">Mortes Recentes</BASEFONT></BODY>", (bool)false, (bool)false);

			AddButton(676, 79, btn5, btn5, 5, GumpButtonType.Reply, 0);
			AddHtml( 716, 81, 185, 20, @"<BODY><BASEFONT Color=" + color + ">Assassinos Procurados</BASEFONT></BODY>", (bool)false, (bool)false);

			AddHtml( 12, 111, 888, 491, @"<BODY><BASEFONT Color=" + color + ">" + sEvents + "</BASEFONT></BODY>", (bool)false, (bool)scroll);
        }

		public override void OnResponse( NetState sender, RelayInfo info )
		{
			Mobile from = sender.Mobile;
			from.SendSound( 0x4A ); 

			switch ( info.ButtonID )
			{
				case 1:
				{
					from.CloseGump( typeof( LoggingGumpCrier ) );
					from.SendGump( new LoggingGumpCrier( from, 2 ) );
					break;
				}
				case 2:
				{
					from.CloseGump( typeof( LoggingGumpCrier ) );
					from.SendGump( new LoggingGumpCrier( from, 3 ) );
					break;
				}
				case 3:
				{
					from.CloseGump( typeof( LoggingGumpCrier ) );
					from.SendGump( new LoggingGumpCrier( from, 4 ) );
					break;
				}
				case 4:
				{
					from.CloseGump( typeof( LoggingGumpCrier ) );
					from.SendGump( new LoggingGumpCrier( from, 5 ) );
					break;
				}
				case 5:
				{
					from.CloseGump( typeof( LoggingGumpCrier ) );
					from.SendGump( new LoggingGumpCrier( from, 6 ) );
					break;
				}
				case 6:
				{
					from.CloseGump( typeof( LoggingGumpCrier ) );
					from.SendGump( new LoggingGumpCrier( from, 7 ) );
					break;
				}
			}
		}
    }
}
