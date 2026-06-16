using System;
using Server;

namespace Server.Items
{
	public class Artifact_AmuletOfFanaedar : GiftGoldNecklace
	{
		[Constructable]
		public Artifact_AmuletOfFanaedar()
		{
			Name = "Amuleto de Fanaedar";
			Hue = 0x455;
            SkillBonuses.SetValues( 0, SkillName.Stealing, 10);
			SkillBonuses.SetValues( 1, SkillName.Snooping, 10);
			SkillBonuses.SetValues( 2, SkillName.Lockpicking, 10);
			SkillBonuses.SetValues( 3, SkillName.MagicResist, 10);
			Attributes.Luck = 100;
			ArtifactLevel = 2;
			Server.Misc.Arty.ArtySetup( this, "" );
		}

		public Artifact_AmuletOfFanaedar( Serial serial ) : base( serial )
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