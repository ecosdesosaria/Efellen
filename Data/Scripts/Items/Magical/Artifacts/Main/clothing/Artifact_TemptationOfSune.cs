using System;
using System.ComponentModel;
using Server;

namespace Server.Items
{
	public class Artifact_TemptationOfSune : GiftFancyDress
	{
		[Constructable]
		public Artifact_TemptationOfSune()
		{
			Name = "Temptation of Sune";
			Hue = 1259;
			SkillBonuses.SetValues( 0, SkillName.Discordance, 15);
			SkillBonuses.SetValues( 1, SkillName.Provocation, 15);
			SkillBonuses.SetValues( 2, SkillName.Musicianship, 10);
			Attributes.CastSpeed = 2;
			Attributes.CastRecovery = 2;
			Attributes.Luck = 50;
			ArtifactLevel = 2;
			Server.Misc.Arty.ArtySetup( this, "" );
		}

		public Artifact_TemptationOfSune( Serial serial ) : base( serial )
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
			ArtifactLevel = 2;
			int version = reader.ReadInt();
		}
	}
}