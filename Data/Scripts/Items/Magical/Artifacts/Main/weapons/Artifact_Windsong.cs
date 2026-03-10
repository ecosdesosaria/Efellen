using System;
using Server;
using Server.Spells;
using Server.Spells.Eighth;
using Server.Targeting;

namespace Server.Items
{
	public class Artifact_Windsong : GiftMagicalShortbow
	{
		public DateTime TimeUsed;
		public override int InitMinHits{ get{ return 80; } }
		public override int InitMaxHits{ get{ return 160; } }

		[Constructable]
		public Artifact_Windsong() : base()
		{
			Hue = 0xAC;
			Name = "Windsong";
			ItemID = 0x2D2B;
			Attributes.WeaponDamage = 25;
			SkillBonuses.SetValues( 0, SkillName.Marksmanship, 10 );
			Velocity = 25;
			ArtifactLevel = 2;
			Server.Misc.Arty.ArtySetup( this, "Calls forth ruinous winds" );
			TimeUsed = DateTime.MinValue;
		}

		public override void OnDoubleClick( Mobile from )
		{
			DateTime TimeNow = DateTime.Now;
			long ticksThen = TimeUsed.Ticks;
			long ticksNow = TimeNow.Ticks;
			int minsThen = (int)TimeSpan.FromTicks(ticksThen).TotalMinutes;
			int minsNow = (int)TimeSpan.FromTicks(ticksNow).TotalMinutes;
			int CanUseMagic = 60 - ( minsNow - minsThen );

			if ( Parent != from )
			{
				from.SendMessage( "Você precisa estar segurando o arco para invocar um elemental." );
			}
			else if ( CanUseMagic > 0 )
			{
				TimeSpan t = TimeSpan.FromMinutes( CanUseMagic );
				string wait = string.Format("{0:D1} horas e {1:D2} minutos", 
								t.Hours, 
								t.Minutes);
				from.SendMessage( "Você pode usar a magia novamente em " + wait + "." );
			}
			else
			{
				new AirElementalSpell( from, this ).Cast();
				TimeUsed = DateTime.Now;
			}
		}

		public Artifact_Windsong( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.WriteEncodedInt( 1 );
			writer.Write(TimeUsed);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			
			ArtifactLevel = 2;
			
			int version = reader.ReadEncodedInt();
			
			if (version >= 1)
				TimeUsed = reader.ReadDateTime();
			else
				TimeUsed = DateTime.MinValue;
		}
	}
}
