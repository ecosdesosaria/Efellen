using System;
using Server;
using Server.Network;
using Server.Multis;
using Server.Gumps;
using Server.Mobiles;
using Server.Accounting;

namespace Server.Items
{
	public class ObeliskTip : Item
	{
		public Mobile ObeliskOwner;
		[CommandProperty( AccessLevel.GameMaster )]
		public Mobile Obelisk_Owner { get{ return ObeliskOwner; } set{ ObeliskOwner = value; } }

		/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		public int HasAir;
		[CommandProperty(AccessLevel.Owner)]
		public int Has_Air { get { return HasAir; } set { HasAir = value; InvalidateProperties(); } }

		public int WonAir;
		[CommandProperty(AccessLevel.Owner)]
		public int Won_Air { get { return WonAir; } set { WonAir = value; InvalidateProperties(); } }

		/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		public int HasFire;
		[CommandProperty(AccessLevel.Owner)]
		public int Has_Fire { get { return HasFire; } set { HasFire = value; InvalidateProperties(); } }

		public int WonFire;
		[CommandProperty(AccessLevel.Owner)]
		public int Won_Fire { get { return WonFire; } set { WonFire = value; InvalidateProperties(); } }

		/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		public int HasEarth;
		[CommandProperty(AccessLevel.Owner)]
		public int Has_Earth { get { return HasEarth; } set { HasEarth = value; InvalidateProperties(); } }

		public int WonEarth;
		[CommandProperty(AccessLevel.Owner)]
		public int Won_Earth { get { return WonEarth; } set { WonEarth = value; InvalidateProperties(); } }

		/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		public int HasWater;
		[CommandProperty(AccessLevel.Owner)]
		public int Has_Water { get { return HasWater; } set { HasWater = value; InvalidateProperties(); } }

		public int WonWater;
		[CommandProperty(AccessLevel.Owner)]
		public int Won_Water { get { return WonWater; } set { WonWater = value; InvalidateProperties(); } }

		/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		[Constructable]
		public ObeliskTip() : base( 0x185F )
		{
			Name = "obelisk tip";
			Weight = 1.0;
			Light = LightType.Circle150;
		}

        public override void AddNameProperties(ObjectPropertyList list)
		{
            base.AddNameProperties(list);
			if ( ObeliskOwner != null ){ list.Add( 1049644, "Belongs to " + ObeliskOwner.Name + "" ); }
        }

		public override void OnDoubleClick( Mobile from )
		{
			if ( !IsChildOf( from.Backpack ) )
			{
				from.SendLocalizedMessage( 1060640 ); // The item must be in your backpack to use it.
			}
			else if ( ObeliskOwner != from )
			{
				from.SendMessage( "This blackrock does not belong to you so it vanishes!" );
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

						if ( m == ObeliskOwner )
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
				from.CloseGump( typeof( ObeliskGump ) );
				from.SendGump( new ObeliskGump( this, from ) );
			}
		}

		public ObeliskTip(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int) 0);

			writer.Write( (Mobile)ObeliskOwner);

            writer.Write( HasAir );
            writer.Write( WonAir );
            writer.Write( HasFire );
            writer.Write( WonFire );
            writer.Write( HasEarth );
            writer.Write( WonEarth );
            writer.Write( HasWater );
            writer.Write( WonWater );
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();

			ObeliskOwner = reader.ReadMobile();

			HasAir = reader.ReadInt();
			WonAir = reader.ReadInt();
			HasFire = reader.ReadInt();
			WonFire = reader.ReadInt();
			HasEarth = reader.ReadInt();
			WonEarth = reader.ReadInt();
			HasWater = reader.ReadInt();
			WonWater = reader.ReadInt();
		}

		private class ObeliskGump : Gump
		{
			private ObeliskTip m_Tip;

			public ObeliskGump( ObeliskTip tip, Mobile from ) : base( 50, 50 )
			{
				m_Tip = tip;
				from.SendSound( 0x5AA );
				string color = "#d0d0d0";

				this.Closable=true;
				this.Disposable=true;
				this.Dragable=true;
				this.Resizable=false;

				AddPage(0);

				AddImage(0, 0, 7031, Server.Misc.PlayerSettings.GetGumpHue( from ));
				AddButton(864, 9, 4017, 4017, 0, GumpButtonType.Reply, 0);
				AddHtml( 12, 11, 665, 20, @"<BODY><BASEFONT Color=" + color + ">O TITÃ DO ÉTER</BASEFONT></BODY>", (bool)false, (bool)false);
				AddHtml( 245, 87, 632, 354, @"<BODY><BASEFONT Color=" + color + ">Há aqueles que buscam se tornar o Titã do Éter, mas para isso, é preciso derrotar os quatro Titãs do Submundo. Lithos, Pyros, Hydros e Stratos contêm um poder elemental que pode ser infundido dentro da pedra negra e empoderar outro. Esses Titãs não podem simplesmente ser mortos por meios normais, pois seu oponente deve possuir um fragmento específico de pedra negra para vencê-los. Esses fragmentos de pedra negra estão espalhados por toda a terra e você terá que procurar por todos os cantos para encontrá-los. Se você aceitar esta grande missão, certifique-se de carregar a Ponta do Obelisco com você o tempo todo. Se você tiver o fragmento apropriado de pedra negra, pode decidir enfrentar o Titã. Se o Titã for morto, a pedra negra absorverá seu poder. Uma vez que todo o poder dos Titãs tenha sido absorvido nos quatro fragmentos de pedra negra, leve a Ponta do Obelisco à Fortaleza de Obsidiana e aproxime-se do Portão de Pedra Negra para se tornar o Titã do Éter. Os Titãs do Éter podem se tornar grão-mestres em cinco habilidades adicionais, e suas habilidades podem totalizar 300 em vez de 250.</BASEFONT></BODY>", (bool)false, (bool)false);

				AddItem(42, 95, 13042);
				AddItem(42, 165, 13042);
				AddItem(42, 235, 13042);
				AddItem(42, 305, 13042);
				AddItem(42, 375, 13042);

				AddHtml( 106, 102, 150, 20, @"<BODY><BASEFONT Color=" + color + ">Ponta do Obelisco</BASEFONT></BODY>", (bool)false, (bool)false);
				AddHtml( 106, 125, 150, 20, @"<BODY><BASEFONT Color=" + color + ">Khumash-Gor</BASEFONT></BODY>", (bool)false, (bool)false);
				AddItem(48, 95, 6239);

				AddHtml( 106, 172, 150, 20, @"<BODY><BASEFONT Color=" + color + ">Sopro do Ar</BASEFONT></BODY>", (bool)false, (bool)false);
				AddHtml( 106, 195, 150, 20, @"<BODY><BASEFONT Color=" + color + ">Stratos</BASEFONT></BODY>", (bool)false, (bool)false);
				if ( tip.HasAir > 0 ){ AddItem(48, 170, 6240); }

				AddHtml( 106, 242, 150, 20, @"<BODY><BASEFONT Color=" + color + ">Língua de Chama</BASEFONT></BODY>", (bool)false, (bool)false);
				AddHtml( 106, 265, 150, 20, @"<BODY><BASEFONT Color=" + color + ">Pyros</BASEFONT></BODY>", (bool)false, (bool)false);
				if ( tip.HasFire > 0 ){ AddItem(47, 238, 6241); }

				AddHtml( 106, 312, 150, 20, @"<BODY><BASEFONT Color=" + color + ">Coração da Terra</BASEFONT></BODY>", (bool)false, (bool)false);
				AddHtml( 106, 335, 150, 20, @"<BODY><BASEFONT Color=" + color + ">Lithos</BASEFONT></BODY>", (bool)false, (bool)false);
				if ( tip.HasEarth > 0 ){ AddItem(48, 311, 6242); }

				AddHtml( 106, 381, 150, 20, @"<BODY><BASEFONT Color=" + color + ">Lágrima dos Mares</BASEFONT></BODY>", (bool)false, (bool)false);
				AddHtml( 106, 405, 150, 20, @"<BODY><BASEFONT Color=" + color + ">Hydros</BASEFONT></BODY>", (bool)false, (bool)false);
				if ( tip.HasWater > 0 ){ AddItem(48, 376, 6243); }

				///////////////////////////////////////////////////////////////////////////////////

				int rocks = tip.HasAir + tip.HasFire + tip.HasEarth + tip.HasWater + 1;
				int titan = tip.WonAir + tip.WonFire + tip.WonEarth + tip.WonWater;

				string stones = "1 dos Fragmentos de Pedra Negra Encontrado!";
				string titans = "Nenhum Titã Foi Derrotado Ainda!";

				if ( rocks > 4 ){ stones = "Todos os Fragmentos de Pedra Negra Encontrados!"; }
				else if ( rocks > 1 ){ stones = rocks + " Fragmentos de Pedra Negra Encontrados!"; }

				if ( titan > 0 )
				{
					titans = "";
					if ( titan > 3 ){ titans = "Todos os Titãs Foram Derrotados!"; }
					else
					{
						if ( tip.WonAir > 0 ){ titans = titans + "Stratos foi derrotado! "; }
						if ( tip.WonFire > 0 ){ titans = titans + "Pyros foi destruído! "; }
						if ( tip.WonEarth > 0 ){ titans = titans + "Lithos foi morto! "; }
						if ( tip.WonWater > 0 ){ titans = titans + "Hydros foi vencido! "; }
					}
				}

				AddHtml( 40, 489, 878, 20, @"<BODY><BASEFONT Color=" + color + ">" + stones + "</BASEFONT></BODY>", (bool)false, (bool)false);
				AddHtml( 40, 545, 878, 20, @"<BODY><BASEFONT Color=" + color + ">" + titans + "</BASEFONT></BODY>", (bool)false, (bool)false);
			}

			public override void OnResponse( NetState sender, RelayInfo info )
			{
				Mobile from = sender.Mobile;
				from.SendSound( 0x5AA ); 
			}
		}
	}
}