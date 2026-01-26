using System;
using Server;

namespace Server.Items
{
	public class Artifact_XyrtaxisBlackReach : GiftBlackStaff
	{
		public override int InitMinHits{ get{ return 80; } }
		public override int InitMaxHits{ get{ return 160; } }

		[Constructable]
		public Artifact_XyrtaxisBlackReach()
		{
			Name = "Xyrtaxis' Black Reach";
			ItemID = 0x0DF1;
			Hue = 1316;
			Attributes.SpellChanneling = 1;
			Attributes.SpellDamage = 40;
			Attributes.CastRecovery = 3;
			Attributes.CastSpeed = 3;
			ArtifactLevel = 2;
			Server.Misc.Arty.ArtySetup( this, "Shivers with arcane power" );
		}

		public Artifact_XyrtaxisBlackReach( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}
		
		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize( reader );
			ArtifactLevel = 2;

			int version = reader.ReadInt();
		}
	}
}