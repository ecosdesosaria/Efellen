using System;
using Server; 
using Server.Misc;
using System.Diagnostics;
using System.Collections.Generic;
using System.Collections;
using System.Reflection;
using System.Net;
using Server.Network;
using Server.Mobiles;
using Server.Accounting;
using Server.Guilds;
using Server.Items;
using Server.Gumps;
using Server.Commands;

namespace Server.Mobiles
{
	public class SherryTheMouse : BasePerson
	{
		private DateTime m_NextTalk;
		public DateTime NextTalk{ get{ return m_NextTalk; } set{ m_NextTalk = value; } }
		public override void OnMovement( Mobile m, Point3D oldLocation )
		{
			if( m is PlayerMobile )
			{
				if ( DateTime.Now >= m_NextTalk && InRange( m, 4 ) && InLOS( m ) )
				{
					Say("Squeak");
					m_NextTalk = (DateTime.Now + TimeSpan.FromSeconds( 30 ));
				}
			}
		}

		[Constructable]
		public SherryTheMouse() : base( )
		{
			SpeechHue = Utility.RandomTalkHue();
			NameHue = 1276;

			Body = 238;
			BaseSoundID = 0xCC;

			Name = "Sherry";
			Title = "the Mouse";
			Direction = Direction.East;
			CantWalk = true;

			SetStr( 100 );
			SetDex( 100 );
			SetInt( 100 );

			SetDamage( 15, 20 );
			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 35, 45 );
			SetResistance( ResistanceType.Fire, 25, 30 );
			SetResistance( ResistanceType.Cold, 25, 30 );
			SetResistance( ResistanceType.Poison, 10, 20 );
			SetResistance( ResistanceType.Energy, 10, 20 );

			SetSkill( SkillName.FistFighting, 100 );
			Karma = 1000;
			VirtualArmor = 30;
		}

		public override void OnDoubleClick( Mobile from )
		{
			bool CanTalk = true;

			if ( !(this.CanSee( from )) ){ CanTalk = false; }
			if ( !(this.InLOS( from )) ){ CanTalk = false; }

			if ( CanTalk )
			{
				this.PlaySound( 0x0CD );
				from.CloseGump( typeof( SherryGump ) );
				from.SendGump( new SherryGump( from, this ) );
			}
			else
			{
				from.SendMessage( "She is too far away from you." );
			}
		}

		public class SherryGump : Gump
		{
			public Mobile mouse;

			public SherryGump( Mobile from, Mobile rat ): base( 50, 50 )
			{
				mouse = rat;
				this.Closable=true;
				this.Disposable=false;
				this.Dragable=true;
				this.Resizable=false;

				AddPage(0);
				AddImage(20, 16, 1243);
				AddButton(202, 247, 2020, 2020, 1, GumpButtonType.Reply, 0);
				AddHtml( 62, 288, 178, 27, @"<BODY><BASEFONT Color=#111111><BIG><CENTER>Sherry the Mouse</CENTER></BIG></BASEFONT></BODY>", (bool)false, (bool)false);
			}

			public override void OnResponse( NetState state, RelayInfo info )
			{
				Mobile from = state.Mobile; 

				mouse.PlaySound( 0x0CD );

				if ( info.ButtonID > 0 )
				{
					switch ( Utility.RandomMinMax( 0, 8 ) )
					{
						case 0:	mouse.Say("Muitas vezes desejei que o estranho retornasse."); break;
						case 1:	mouse.Say("Devemos trazer os fragmentos para a harmonia, para que ressoem de maneira que combine com o universo original."); break;
						case 2:	mouse.Say("No entanto, às vezes é preciso sacrificar um peão para salvar um rei."); break;
						case 3:	mouse.Say("Subitamente as persianas se abriram e Lord British caiu no chão, com uma mão protegendo os olhos."); break;
						case 4:	mouse.Say("Eu testemunhei tudo do meu pequeno buraco de rato."); break;
						case 5:	mouse.Say("Mas eu sou apenas um rato, e ninguém me ouve."); break;
						case 6:	mouse.Say("Um fragmento de um universo é uma coisa poderosa."); break;
						case 7:	mouse.Say("Ajudai a nobreza que reside no coração humano."); break;
						case 8:	mouse.Say("Até os peões têm vidas e amores em casa, meu senhor."); break;
					}
				}
			}
		}

		public override bool OnDragDrop( Mobile from, Item dropped )
		{
			if ( dropped is CheeseWheel || dropped is CheeseWedge || dropped is CheeseSlice )
			{
				this.PlaySound( 0x0CD );

				string sMessage = "Squeak";

				int relic = Utility.RandomMinMax( 1, 59 );

				int chance = dropped.Amount;
					if ( chance > 75 ){ chance = 75; }

				int pick = Utility.RandomMinMax( 0, 8 );
					if ( chance >= Utility.RandomMinMax( 1, 100 ) ){ pick = 9; }

				switch ( pick )
				{
					case 0:	sMessage = "Ouvi dizer que o " + Server.Items.SomeRandomNote.GetSpecialItem( relic, 1 ) + " pode ser obtido em " + Server.Items.SomeRandomNote.GetSpecialItem( relic, 0 ) + "."; break;
					case 1:	sMessage = "Nystal disse algo sobre o " + Server.Items.SomeRandomNote.GetSpecialItem( relic, 1 ) + " e " + Server.Items.SomeRandomNote.GetSpecialItem( relic, 0 ) + "."; break;
					case 2:	sMessage = "Alguém disse a Lord British que " + Server.Items.SomeRandomNote.GetSpecialItem( relic, 0 ) + " é onde você procuraria pelo " + Server.Items.SomeRandomNote.GetSpecialItem( relic, 1 ) + "."; break;
					case 3:	sMessage = "Lord British me contava histórias de cavaleiros indo para " + Server.Items.SomeRandomNote.GetSpecialItem( relic, 0 ) + " e trazendo de volta o " + Server.Items.SomeRandomNote.GetSpecialItem( relic, 1 ) + "."; break;
					case 4:	sMessage = QuestCharacters.RandomWords() + " estava na cozinha sussurrando sobre o " + Server.Items.SomeRandomNote.GetSpecialItem( relic, 1 ) + " e " + Server.Items.SomeRandomNote.GetSpecialItem( relic, 0 ) + "."; break;
					case 5:	sMessage = "Vi uma nota do " + RandomThings.GetRandomJob() + ", e ela mencionava o " + Server.Items.SomeRandomNote.GetSpecialItem( relic, 1 ) + " e " + Server.Items.SomeRandomNote.GetSpecialItem( relic, 0 ) + "."; break;
					case 6:	sMessage = "Lord British se encontrou com " + QuestCharacters.RandomWords() + " e lhes disse para trazerem de volta o " + Server.Items.SomeRandomNote.GetSpecialItem( relic, 1 ) + " de " + Server.Items.SomeRandomNote.GetSpecialItem( relic, 0 ) + "."; break;
					case 7:	sMessage = "Ouvi dizer que o " + Server.Items.SomeRandomNote.GetSpecialItem( relic, 1 ) + " pode ser encontrado em " + Server.Items.SomeRandomNote.GetSpecialItem( relic, 0 ) + "."; break;
					case 8:	sMessage = "Alguém de " + RandomThings.GetRandomCity() + " morreu em " + Server.Items.SomeRandomNote.GetSpecialItem( relic, 0 ) + " procurando pelo " + Server.Items.SomeRandomNote.GetSpecialItem( relic, 1 ) + "."; break;
					case 9:	sMessage = Server.Misc.TavernPatrons.GetRareLocation( this, false, false );		break;
				}
				this.PrivateOverheadMessage(MessageType.Regular, 1153, false, sMessage, from.NetState);
				dropped.Delete();
				return true;
			}

			return base.OnDragDrop( from, dropped );
		}

		public SherryTheMouse( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
}