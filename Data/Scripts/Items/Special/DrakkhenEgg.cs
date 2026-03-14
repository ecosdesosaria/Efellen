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

namespace Server.Items
{
	public class DrakkhenEggRed : Item
	{
		[Constructable]
		public DrakkhenEggRed() : base( 0x1444 )
		{
			Weight = 4.0;
			Name = "Drakkhen Crystal";
			Light = LightType.Circle225;
			Hue = 0xB01;
			HaveGold = 0;
		}

		public override void OnDoubleClick( Mobile from )
		{
			from.SendSound( 0x5AA );
			from.CloseGump( typeof( DrakkhenEggGump ) );
			from.SendGump( new DrakkhenEggGump( from, this ) );
		}

		public DrakkhenEggRed( Serial serial ) : base( serial )
		{
		}

		public override bool OnDragDrop( Mobile from, Item dropped )
		{          		
			int iAmount = 0;
			string sEnd = ".";

			if ( from != null )
			{
				if ( dropped is Gold && 50000 > HaveGold )
				{
					from.SendSound( 0x5AA );
					int WhatIsDropped = dropped.Amount;
					int WhatIsNeeded = 50000 - HaveGold;
					int WhatIsExtra = WhatIsDropped - WhatIsNeeded; if ( WhatIsExtra < 1 ){ WhatIsExtra = 0; }
					int WhatIsTaken = WhatIsDropped - WhatIsExtra;

					if ( WhatIsExtra > 0 ){ from.AddToBackpack( new Gold( WhatIsExtra ) ); }
					iAmount = WhatIsTaken;

					if ( iAmount > 1 ){ sEnd = "s."; }

					HaveGold = HaveGold + iAmount;
					from.SendMessage( "Você adicionou " + iAmount.ToString() + " moeda" + sEnd );
					dropped.Delete();
					return true;
				}
			}

			return false;
		}

		public static bool ProcessDrakkhenEgg( Mobile m, Mobile druid, Item dropped )
		{
			DrakkhenEggRed egg = (DrakkhenEggRed)dropped;

			if ( egg.HaveGold < 50000 )
			{
				druid.Say( "Você não tem ouro suficiente para eu realizar este serviço." );
				return false;
			}
			else if ( (m.Followers + 2) > m.FollowersMax )
			{
				druid.Say( "Você tem muitos seguidores com você para quebrar este cristal." );
				return false;
			}

			BaseCreature drakkhen = new DrakkhenRed();
			drakkhen.OnAfterSpawn();
			drakkhen.Controlled = true;
			drakkhen.ControlMaster = m;
			drakkhen.IsBonded = true;
			drakkhen.MoveToWorld( m.Location, m.Map );
			drakkhen.ControlTarget = m;
			drakkhen.Tamable = true;
			drakkhen.ControlOrder = OrderType.Follow;

			LoggingFunctions.LogGenericQuest( m, "quebrou um cristal de drakkhen" );
			m.PrivateOverheadMessage(MessageType.Regular, 1153, false, "Seu drakkhen está livre.", m.NetState);

			m.PlaySound( 0x041 );

			dropped.Delete();

			return true;
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int)1 ); // version
			writer.Write( HaveGold );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
			HaveGold = reader.ReadInt();
		}

		public class DrakkhenEggGump : Gump
		{
			public DrakkhenEggGump( Mobile from, DrakkhenEggRed egg ): base( 50, 50 )
			{
				string color = "#f73d3c";

				this.Closable=true;
				this.Disposable=true;
				this.Dragable=true;
				this.Resizable=false;

				int need = 50000 - egg.HaveGold;
				string cost = " Coloque " + need + " mais ouro no cristal e entregue-o a um druida.";
				if ( egg.HaveGold >= 50000 ){ cost = " Agora você pode entregar isto a um druida pois tem ouro suficiente."; }

				AddPage(0);

				AddImage(0, 0, 7016, Server.Misc.PlayerSettings.GetGumpHue( from ));
				AddHtml( 12, 11, 420, 20, @"<BODY><BASEFONT Color=" + color + ">DRAKKHEN CRYSTAL</BASEFONT></BODY>", (bool)false, (bool)false);
				AddButton(546, 10, 4017, 4017, 0, GumpButtonType.Reply, 0);
				AddHtml( 12, 42, 561, 297, @"<BODY><BASEFONT Color=" + color + "><BR><BR><BR>Você ouviu contos sobre estas gemas. Estes cristais raros vêm das poderosas bestas dragão-kin das quais isto foi encontrado. Dentro dele jaz a versão infantil da criatura, mas apenas os druidas locais sabem como libertá-la com segurança desta gema encapsulada. Se você puder encontrar tal druida e quiser libertar a criatura, então esteja pronto para dar 50000 moedas de ouro em tributo, pois o druida não fará tal coisa por bondade de coração. Quando o drakkhen for libertado, será muito jovem e apenas metade tão poderoso quanto um drake. Você pode montá-lo se quiser, mas leva séculos para que cresçam tão poderosos quanto aquele de quem isto foi tirado, então eles nunca serão tão fortes. No entanto, são bestas raras." + cost + "</BASEFONT></BODY>", (bool)false, (bool)false);
			}

			public override void OnResponse(NetState state, RelayInfo info)
			{
				Mobile from = state.Mobile;
				from.SendSound( 0x5AA ); 
			}
		}

		public int HaveGold;
		[CommandProperty( AccessLevel.GameMaster )]
		public int g_HaveGold { get{ return HaveGold; } set{ HaveGold = value; } }
	}
	public class DrakkhenEggBlack : Item
	{
		[Constructable]
		public DrakkhenEggBlack() : base( 0x1444 )
		{
			Weight = 4.0;
			Name = "Drakkhen Crystal";
			Light = LightType.Circle225;
			Hue = 0x99E;
			HaveGold = 0;
		}

		public override void OnDoubleClick( Mobile from )
		{
			from.SendSound( 0x5AA );
			from.CloseGump( typeof( DrakkhenEggGump ) );
			from.SendGump( new DrakkhenEggGump( from, this ) );
		}

		public DrakkhenEggBlack( Serial serial ) : base( serial )
		{
		}

		public override bool OnDragDrop( Mobile from, Item dropped )
		{          		
			int iAmount = 0;
			string sEnd = ".";

			if ( from != null )
			{
				if ( dropped is Gold && 50000 > HaveGold )
				{
					from.SendSound( 0x5AA );
					int WhatIsDropped = dropped.Amount;
					int WhatIsNeeded = 50000 - HaveGold;
					int WhatIsExtra = WhatIsDropped - WhatIsNeeded; if ( WhatIsExtra < 1 ){ WhatIsExtra = 0; }
					int WhatIsTaken = WhatIsDropped - WhatIsExtra;

					if ( WhatIsExtra > 0 ){ from.AddToBackpack( new Gold( WhatIsExtra ) ); }
					iAmount = WhatIsTaken;

					if ( iAmount > 1 ){ sEnd = "s."; }

					HaveGold = HaveGold + iAmount;
					from.SendMessage( "Você adicionou " + iAmount.ToString() + " moeda" + sEnd );
					dropped.Delete();
					return true;
				}
			}

			return false;
		}

		public static bool ProcessDrakkhenEgg( Mobile m, Mobile druid, Item dropped )
		{
			DrakkhenEggBlack egg = (DrakkhenEggBlack)dropped;

			if ( egg.HaveGold < 50000 )
			{
				druid.Say( "Você não tem ouro suficiente para eu realizar este serviço." );
				return false;
			}
			else if ( (m.Followers + 2) > m.FollowersMax )
			{
				druid.Say( "Você tem muitos seguidores com você para quebrar este cristal." );
				return false;
			}

			BaseCreature drakkhen = new DrakkhenBlack();
			drakkhen.OnAfterSpawn();
			drakkhen.Controlled = true;
			drakkhen.ControlMaster = m;
			drakkhen.IsBonded = true;
			drakkhen.MoveToWorld( m.Location, m.Map );
			drakkhen.ControlTarget = m;
			drakkhen.Tamable = true;
			drakkhen.ControlOrder = OrderType.Follow;

			LoggingFunctions.LogGenericQuest( m, "quebrou um cristal de drakkhen" );
			m.PrivateOverheadMessage(MessageType.Regular, 1153, false, "Seu drakkhen foi libertado.", m.NetState);

			m.PlaySound( 0x041 );

			dropped.Delete();

			return true;
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int)1 ); // version
			writer.Write( HaveGold );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
			HaveGold = reader.ReadInt();
		}

		public class DrakkhenEggGump : Gump
		{
			public DrakkhenEggGump( Mobile from, DrakkhenEggBlack egg ): base( 50, 50 )
			{
				string color = "#f73d3c";

				this.Closable=true;
				this.Disposable=true;
				this.Dragable=true;
				this.Resizable=false;

				int need = 50000 - egg.HaveGold;
				string cost = " Coloque " + need + " mais ouro sobre o cristal e entregue-o a um druida.";
				if ( egg.HaveGold >= 50000 ){ cost = " Agora você pode entregar isto a um druida já que tem ouro suficiente."; }

				AddPage(0);

				AddImage(0, 0, 7016, Server.Misc.PlayerSettings.GetGumpHue( from ));
				AddHtml( 12, 11, 420, 20, @"<BODY><BASEFONT Color=" + color + ">DRAKKHEN CRYSTAL</BASEFONT></BODY>", (bool)false, (bool)false);
				AddButton(546, 10, 4017, 4017, 0, GumpButtonType.Reply, 0);
				AddHtml( 12, 42, 561, 297, @"<BODY><BASEFONT Color=" + color + "><BR><BR><BR>Você ouviu contos sobre estas gemas. Estes cristais raros vêm das poderosas bestas dragão-kin das quais isto foi encontrado. Dentro dele jaz a versão infantil da criatura, mas apenas os druidas locais sabem como libertá-la com segurança desta gema encapsulada. Se você puder encontrar tal druida e quiser libertar a criatura, então esteja pronto para dar 50000 moedas de ouro em tributo, pois o druida não fará tal coisa por bondade de coração. Quando o drakkhen for libertado, será muito jovem e apenas metade tão poderoso quanto um drake. Você pode montá-lo se quiser, mas leva séculos para que cresçam tão poderosos quanto aquele de quem isto foi tirado, então eles nunca serão tão fortes. No entanto, são bestas raras." + cost + "</BASEFONT></BODY>", (bool)false, (bool)false);
			}

			public override void OnResponse(NetState state, RelayInfo info)
			{
				Mobile from = state.Mobile;
				from.SendSound( 0x5AA ); 
			}
		}

		public int HaveGold;
		[CommandProperty( AccessLevel.GameMaster )]
		public int g_HaveGold { get{ return HaveGold; } set{ HaveGold = value; } }
	}
}