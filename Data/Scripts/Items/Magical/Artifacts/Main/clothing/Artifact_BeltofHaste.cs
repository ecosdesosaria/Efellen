using System;
using Server;

namespace Server.Items
{
	public class Artifact_BeltofHaste : GiftBelt
	{
		[Constructable]
		public Artifact_BeltofHaste()
		{
			Hue = 0x6D1;
			ItemID = 0x2790;
			Name = "Belt of Haste";
			Attributes.BonusDex = 30;
			ArtifactLevel = 2;
			Server.Misc.Arty.ArtySetup( this, "" );
		}

		public Artifact_BeltofHaste( Serial serial ) : base( serial )
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