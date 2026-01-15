using System;
using Server;
using Server.Items;
using System.Text;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;

namespace Server.Items
{
	public class DesmonaDiary : Item
	{
		[Constructable]
		public DesmonaDiary( ) : base( 0xE34 )
		{
			Weight = 1.0;
			Hue = 0x0213;
			Name = "a dusty scroll";
			ItemID = 0x14EE;
		}

		public class ClueGump : Gump
		{
			public ClueGump( Mobile from ): base( 100, 100 )
			{
				from.PlaySound( 0x249 );
                string sText = "...Tsareth mine own, my dread and darling heart—what suffering yet remaineth ere I may be with thee again?<br>For long ages have I tended our sanctum and its grim tomes, and yet I cannot undo the wicked decree that sundered us, when the Order cast us forth for our love of magic and of each other—fearing the greatness we were so near to grasping.<br>Then came that creeping calamity which gnawed upon the Weave itself, a blighted tide men now dare not name. In that hour thou didst not abandon me, but with deliberate hand and loving will thou didst bind my soul to undeath, that I might be spared its wasting breath. This dark benediction was thy gift to me, my truest mercy.<br>Yet even thee the plague did not wholly spare. I felt the moment thy light was torn from the world, and since that day I have stood vigil within our halls, guarding our library and our home as both tomb and temple, lest any profane hand despoil what was once thine.<br>Will thou be mine open wound, love?<br>To tend ’til the end of time,<br>to dress when thy absence maketh me suffer,<br>soothe when thy memory burneth me raw,<br>touch when this gnawing hunger turneth my loathing upon myself?<br>";


				this.Closable=true;
				this.Disposable=true;
				this.Dragable=true;
				this.Resizable=false;

				AddPage(0);

				AddImage(0, 0, 10901, 2786);
				AddImage(0, 0, 10899, 2117);
				AddHtml( 45, 78, 386, 218, @"<BODY><BASEFONT Color=#d9c781>" + sText + "</BASEFONT></BODY>", (bool)false, (bool)true);
			}

			public override void OnResponse( NetState state, RelayInfo info ) 
			{
				Mobile from = state.Mobile; 
				from.PlaySound( 0x249 );
			}
		}

		public override void OnDoubleClick( Mobile m )
		{
			if ( m.InRange( this.GetWorldLocation(), 2 ) )
			{
				m.SendGump( new ClueGump( m ) );
				m.PlaySound( 0x249 );
			}
			else
			{
				m.SendLocalizedMessage( 502138 ); // That is too far away for you to use
			}
		}

		public DesmonaDiary(Serial serial) : base(serial)
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