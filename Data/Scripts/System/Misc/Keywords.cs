using System;
using Server;
using Server.Items;
using Server.Guilds;
using Server.Mobiles;
using Server.Gumps;

namespace Server.Misc
{
	public class Keywords
	{
		public static void Initialize()
		{
			// Register our speech handler
			EventSink.Speech += new SpeechEventHandler( EventSink_Speech );
		}

		public static void ConsiderSins( Mobile from )
		{
			from.SendMessage( 0x3B2, "Assassinatos de curto prazo: {0} | Assassinatos de longo prazo: {1}", from.ShortTermMurders, from.Kills );

			if ( from is PlayerMobile )
			{
				PlayerMobile pm = (PlayerMobile)from;
				TimeSpan remaining = pm.KillDecayRemaining;

				if ( pm.Kills > 0 && remaining > TimeSpan.Zero )
				{
					int hours = (int)remaining.TotalHours;
					int minutes = remaining.Minutes;

					if ( hours > 0 )
						from.SendMessage( 0x3B2, "Tempo restante para o próximo perdão: {0} hora(s) e {1} minuto(s) de jogo.", hours, minutes );
					else
						from.SendMessage( 0x3B2, "Tempo restante para o próximo perdão: {0} minuto(s) de jogo.", minutes );
				}
				else if ( pm.Kills > 0 )
				{
					from.SendMessage( 0x3B2, "Seu próximo perdão ocorrerá no próximo salvamento do mundo." );
				}
			}
		}

		public static void EventSink_Speech( SpeechEventArgs args )
		{
			Mobile from = args.Mobile;

			if ( args.Speech != null && ( args.Speech.IndexOf( "devo considerar meus pecados", StringComparison.OrdinalIgnoreCase ) >= 0 || args.Speech.IndexOf( "i must consider my sins", StringComparison.OrdinalIgnoreCase ) >= 0 ) )
			{
				ConsiderSins( from );
				return;
			}

			int[] keywords = args.Keywords;

			for ( int i = 0; i < keywords.Length; ++i )
			{
				switch ( keywords[i] )
				{
					case 0x002A: // *i resign from my guild*
					{
						if ( from.Guild != null )
							((Guild)from.Guild).RemoveMember( from );

						break;
					}
					case 0x0032: // *i must consider my sins*
					{
						ConsiderSins( from );
						break;
					}
					case 0x0035: // i renounce my young player status*
					{
						if ( from is PlayerMobile && ((PlayerMobile)from).Young && !from.HasGump( typeof( RenounceYoungGump ) ) )
						{
							from.SendGump( new RenounceYoungGump() );
						}

						break;
					}
				}
			}
		}
	}
}