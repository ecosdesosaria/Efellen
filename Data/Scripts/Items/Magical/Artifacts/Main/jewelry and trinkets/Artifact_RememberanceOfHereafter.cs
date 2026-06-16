using System;
using Server;

namespace Server.Items
{
	public class Artifact_RememberanceOfHereafter : GiftTalismanLeather
	{
		[Constructable]
		public Artifact_RememberanceOfHereafter()
		{
			Name = "Rememberance of Hereafter";
			ItemID = 0x2C95;
			Hue = 2310;
			SkillBonuses.SetValues( 0, SkillName.Forensics, 30.0 );
			SkillBonuses.SetValues( 1, SkillName.Spiritualism, 20.0 );
			Attributes.LowerRegCost = 20;
			ArtifactLevel = 2;
			Server.Misc.Arty.ArtySetup( this, "" );
		}

		public Artifact_RememberanceOfHereafter( Serial serial ) :  base( serial )
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