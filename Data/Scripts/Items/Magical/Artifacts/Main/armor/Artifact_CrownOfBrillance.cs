using System;
using Server;

namespace Server.Items
{
	public class Artifact_CrownOfBrillance : GiftHornedTribalMask
	{
		public override int BaseEnergyResistance{ get{ return 25; } }

		[Constructable]
		public Artifact_CrownOfBrillance()
		{
			Hue = 0x0213;
			Name = "Crown of Brillance";
			Attributes.BonusInt = 30;
			ArtifactLevel = 2;
			Server.Misc.Arty.ArtySetup( this, "" );
		}

		public Artifact_CrownOfBrillance( Serial serial ) : base( serial )
		{
		}
		
		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 );
		}
		
		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize( reader );
			ArtifactLevel = 2;

			int version = reader.ReadInt();
		}
	}
}