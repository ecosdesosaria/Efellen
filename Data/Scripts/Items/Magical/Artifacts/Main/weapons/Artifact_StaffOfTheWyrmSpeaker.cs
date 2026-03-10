using System;
using Server;
using Server.Spells.Magical;
using Server.Targeting;

namespace Server.Items
{
	public class Artifact_StaffOfTheWyrmSpeaker : GiftQuarterStaff
	{
		public DateTime TimeUsed;

		[CommandProperty(AccessLevel.Owner)]
		public DateTime Time_Used { get { return TimeUsed; } set { TimeUsed = value; InvalidateProperties(); } }

		public override int InitMinHits{ get{ return 80; } }
		public override int InitMaxHits{ get{ return 160; } }

		[Constructable]
		public Artifact_StaffOfTheWyrmSpeaker()
		{
			Hue = 0xB85;
			Name = "Staff of the Wyrmspeaker";
			SkillBonuses.SetValues( 1, SkillName.Magery, 15 );
			SkillBonuses.SetValues( 2, SkillName.MagicResist, 15 );
			Attributes.RegenMana = 10;
			Attributes.BonusInt = 10;
			Attributes.SpellChanneling = 1;
			ArtifactLevel = 2;
			Server.Misc.Arty.ArtySetup( this, "Calls forth Dragons" );
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
				from.SendMessage( "Você precisa estar segurando o cajado para invocar dragões." );
			}
			else if ( CanUseMagic > 0 )
			{
				TimeSpan t = TimeSpan.FromMinutes( CanUseMagic );
				string wait = string.Format("{0:D1} horas e {1:D2} minutos", 
								t.Hours, 
								t.Minutes);
				from.SendMessage( "Você pode usar a magia em " + wait + "." );
			}
			else
			{
				new SummonDragonSpell( from, this ).Cast();
				TimeUsed = DateTime.Now;
			}
		}

		public Artifact_StaffOfTheWyrmSpeaker( Serial serial ) : base( serial )
		{
		}
		
		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 1 );
            writer.Write( TimeUsed );
		}
		
		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize( reader );
			ArtifactLevel = 2;
			int version = reader.ReadInt();
			if (version >= 1)
				TimeUsed = reader.ReadDateTime();
			else
				TimeUsed = DateTime.MinValue;
		}
	}
}