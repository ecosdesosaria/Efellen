using System;
using Server;

namespace Server.Items
{
	public class Artifact_EarringsOfAllurement : GiftGoldEarrings
	{
		[Constructable]
		public Artifact_EarringsOfAllurement()
		{
			Name = "Earrings of Allurement";
			Hue = 0x668;
			SkillBonuses.SetValues( 0, SkillName.Mercantile, 15 );
			SkillBonuses.SetValues( 1, SkillName.Peacemaking, 15 );
			Attributes.Luck = 100;
			ArtifactLevel = 2;
			Server.Misc.Arty.ArtySetup( this, "" );
		}

		public Artifact_EarringsOfAllurement( Serial serial ) : base( serial )
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

			ItemID = 0x672F;
		}
	}
}