using System;
using Server;

namespace Server.Items
{
	public class Artifact_RingOfAllurement : GiftGoldRing
	{
		[Constructable]
		public Artifact_RingOfAllurement()
		{
			Name = "Ring of Allurement";
			Hue = 0x668;
			SkillBonuses.SetValues( 0, SkillName.Mercantile, 15 );
			SkillBonuses.SetValues( 1, SkillName.Peacemaking, 15 );
			Attributes.Luck = 100;
			ArtifactLevel = 2;
			Server.Misc.Arty.ArtySetup( this, "" );
		}

		public Artifact_RingOfAllurement( Serial serial ) : base( serial )
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