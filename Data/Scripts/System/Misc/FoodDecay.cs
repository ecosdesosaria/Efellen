using System;
using Server;
using Server.Network;
using Server.Mobiles;
using Server.Regions;

namespace Server.Misc
{
	public class FoodDecayTimer : Timer
	{
		public static void Initialize()
		{
			new FoodDecayTimer().Start();
		}

		public FoodDecayTimer() : base( TimeSpan.FromMinutes( MyServerSettings.FoodCheck() ), TimeSpan.FromMinutes( MyServerSettings.FoodCheck() ) )
		{
			Priority = TimerPriority.OneMinute;
		}

		protected override void OnTick()
		{
			FoodDecay();			
		}

		public static void FoodDecay()
		{
			foreach ( NetState state in NetState.Instances )
			{
				HungerDecay( state.Mobile );
				ThirstDecay( state.Mobile );
			}
		}

		public static void HungerDecay( Mobile m )
		{
			if ( m != null  )
			{
				if ( m is PlayerMobile )
				{
					Server.Items.BaseRace.SyncRace( m, true );

					BuffInfo.CleanupIcons( m, false );

					bool InsideInn = false;

					if ( MySettings.S_Belly )
					{
						if ( m.Region is PublicRegion )
							InsideInn = true;
						else if ( m.Region is PrisonArea )
							InsideInn = true;
						else if ( m.Region is SafeRegion )
							InsideInn = true;
						else if ( m.Region is StartRegion )
							InsideInn = true;
						else if ( m.Region is HouseRegion )
							InsideInn = true;
					}

					if ( m.Skills[SkillName.Camping].Value >= Utility.RandomMinMax( 1, 200 ) ){}
					else if ( InsideInn ){}
					else if ( Server.Items.BaseRace.NoFood( m.RaceID ) ){ m.Hunger = 20; }
					else if ( Server.Items.BaseRace.NoFoodOrDrink( m.RaceID ) ){ m.Thirst = 20; m.Hunger = 20; }
					else
					{
						if ( m.Hunger >= 1 )
						{
							m.Hunger -= 1;
							// added to give hunger value a real meaning.
							if ( m.Hunger < 5 ){ m.SendMessage( "Você está extremamente faminto." ); m.LocalOverheadMessage(MessageType.Emote, 0x916, true, "Estou extremamente faminto."); }
							else if ( m.Hunger < 10 ){ m.SendMessage( "Você está ficando muito faminto." ); m.LocalOverheadMessage(MessageType.Emote, 0x916, true, "Estou ficando muito faminto."); }
						}	
						else
						{
							if ( m.Hits > 5 )
								m.Hits -= 5;
							if ( m.Mana > 2 )
								m.Mana -= 2;

							m.SendMessage( "Você está morrendo de fome!" );
							m.LocalOverheadMessage(MessageType.Emote, 0x916, true, "Estou morrendo de fome!");
						}
					}
				}
				else if ( m is BaseCreature )
				{
					BaseCreature bc = (BaseCreature)m;

					if ( bc.Controlled && m.Hunger >= 1 )
					{
						m.Hunger -= 1;
					}
				}
			}
		}

		public static void ThirstDecay( Mobile m )
		{
			if ( m != null )
			{
				if ( m is PlayerMobile )
				{
					Server.Items.BaseRace.SyncRace( m, true );

					bool InsideInn = false;

					if ( MySettings.S_Belly )
					{
						if ( m.Region is PublicRegion )
							InsideInn = true;
						else if ( m.Region is PrisonArea )
							InsideInn = true;
						else if ( m.Region is SafeRegion )
							InsideInn = true;
						else if ( m.Region is StartRegion )
							InsideInn = true;
						else if ( m.Region is HouseRegion )
							InsideInn = true;
					}

					if ( m.Skills[SkillName.Camping].Value >= Utility.RandomMinMax( 1, 200 ) ){}
					else if ( InsideInn ){}
					else if ( Server.Items.BaseRace.NoFoodOrDrink( m.RaceID ) ){ m.Thirst = 20; m.Hunger = 20; }
					else if ( Server.Items.BaseRace.BrainEater( m.RaceID ) ){ m.Thirst = 20; }
					else
					{
						if ( m.Thirst >= 1 )
						{
							m.Thirst -= 1;
							if ( m.Thirst < 5 ){ m.SendMessage( "Você está extremamente sedento." ); m.LocalOverheadMessage(MessageType.Emote, 0x916, true, "Estou extremamente sedento."); }
							else if ( m.Thirst < 10 ){ m.SendMessage( "Você está ficando com sede." ); m.LocalOverheadMessage(MessageType.Emote, 0x916, true, "Estou ficando com sede."); }
						}
						else
						{
							if ( m.Stam > 5 )
								m.Stam -= 5;
							if ( m.Mana > 2 )
								m.Mana -= 2;

							m.SendMessage( "Você está exausto de sede" );
							m.LocalOverheadMessage(MessageType.Emote, 0x916, true, "Estou exausto de sede!");
						}
					}
				}
				else if ( m is BaseCreature )
				{
					BaseCreature bc = (BaseCreature)m;

					if ( bc.Controlled && m.Thirst >= 1 )
					{
						m.Thirst -= 1;
					}
				}
			}
		}
	}
}
