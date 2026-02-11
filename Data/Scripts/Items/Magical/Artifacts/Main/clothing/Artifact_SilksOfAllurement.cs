using System;
using System.ComponentModel;
using Server;

namespace Server.Items
{
	public class Artifact_SilksOfAllurement : GiftGildedDress
	{
		[Constructable]
		public Artifact_SilksOfAllurement()
		{
			Name = "Silks of Allurement";
			Hue = 0x668;
			SkillBonuses.SetValues( 0, SkillName.Peacemaking, 10);
			SkillBonuses.SetValues( 1, SkillName.Mercantile, 10);
			SkillBonuses.SetValues( 2, SkillName.Musicianship, 10);
			Attributes.BonusDex = 10;
			Attributes.Luck = 50;
			ArtifactLevel = 2;
			Server.Misc.Arty.ArtySetup( this, "" );
		}

		public Artifact_SilksOfAllurement( Serial serial ) : base( serial )
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