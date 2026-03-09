using System;
using Server;
using Server.Commands;
using Server.Items;
using Server.Network;
using Server.Targeting;
using CPA = Server.CommandPropertyAttribute;

namespace Server.Scripts.Commands
{
	public class MyHunger
	{
		public static void Initialize()
		{
			CommandSystem.Register ( "mhgr", AccessLevel.Player, new CommandEventHandler ( MyHunger_OnCommand ) );
			CommandSystem.Register ( "myhunger", AccessLevel.Player, new CommandEventHandler ( MyHunger_OnCommand ) );
		}
		public static void MyHunger_OnCommand( CommandEventArgs e )
		{
			int h = e.Mobile.Hunger; // Variable to hold the hunger value of the player
			// these values are taken from Food.cs and relate directly to the message
			// you get when you eat.
			if (h <= 0 )
				e.Mobile.SendMessage( "Você está morrendo de fome." );
			else if ( h <= 5 )
				e.Mobile.SendMessage( "Você está extremamente faminto." );
			else if ( h <= 10 )
				e.Mobile.SendMessage( "Você está muito faminto." );
			else if ( h <= 15 )
				e.Mobile.SendMessage( "Você está um pouco faminto." );
			else if ( h <= 19 )
				e.Mobile.SendMessage( "Você não está realmente faminto." );
			else if ( h > 19 )
				e.Mobile.SendMessage( "Você está bastante satisfeito." );
			else
				e.Mobile.SendMessage( "Erro: Por favor, reporte este erro: fome não encontrada." );

			int t = e.Mobile.Thirst; // Variable to hold the thirst value of the player
			// read the comments above to see where these values came from
			if ( t <= 0 )
				e.Mobile.SendMessage( "Você está exausto de sede." );
			else if ( t <= 5 )
				e.Mobile.SendMessage( "Você está extremamente sedento." );
			else if ( t <= 10 )
				e.Mobile.SendMessage( "Você está muito sedento." );
			else if ( t <= 15 )
				e.Mobile.SendMessage( "Você está um pouco sedento." );
			else if ( t <= 19 )
				e.Mobile.SendMessage( "Você não está realmente sedento." );
			else if ( t > 19 )
				e.Mobile.SendMessage( "Você não está com sede." );
			else
				e.Mobile.SendMessage( "Erro: Por favor, reporte este erro: sede não encontrada." );
		}
	}
}
