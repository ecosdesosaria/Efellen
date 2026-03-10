using System;
using Server;
using Server.Spells.Sixth;
using Server.Targeting;

namespace Server.Items
{
	public class Artifact_CloakOfTheRogue : GiftCloak
	{
        public DateTime TimeUsed;

		[Constructable]
		public Artifact_CloakOfTheRogue()
		{
			Name = "Cloak of the Rogue";
			Hue = 0x967;
			SkillBonuses.SetValues( 0, SkillName.Stealing, 25 );
			SkillBonuses.SetValues( 1, SkillName.Snooping, 25 );
			SkillBonuses.SetValues( 2, SkillName.RemoveTrap, 50 );
			ArtifactLevel = 2;
			Server.Misc.Arty.ArtySetup( this, "Casts Invisibility" );
		}

		public override void OnDoubleClick( Mobile from )
		{
            DateTime TimeNow = DateTime.Now;
			long ticksThen = TimeUsed.Ticks;
			long ticksNow = TimeNow.Ticks;
			int minsThen = (int)TimeSpan.FromTicks(ticksThen).TotalMinutes;
			int minsNow = (int)TimeSpan.FromTicks(ticksNow).TotalMinutes;
			int CanUseMagic = 15 - ( minsNow - minsThen );

			if ( Parent != from )
			{
				from.SendMessage( "Você precisa estar usando a capa para usar seu poder." );
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
				new InvisibilitySpell( from, this ).Cast();
				TimeUsed = DateTime.Now;
			}
			return;
		}

		public Artifact_CloakOfTheRogue( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 1 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			ArtifactLevel = 2;
			int version = reader.ReadInt();
		}
	}
}