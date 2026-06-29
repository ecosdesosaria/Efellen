using System;
using Server;

namespace Server.Items
{
	public class Artifact_RosaryOfMyrkul : GiftGoldBeadNecklace
	{
		[Constructable]
		public Artifact_RosaryOfMyrkul()
		{
			Name = "Rosário de Myrkul";
			Hue = 0x0455;
			SkillBonuses.SetValues( 0, SkillName.Necromancy, 15);
			SkillBonuses.SetValues( 1, SkillName.Spiritualism, 15);
			SkillBonuses.SetValues( 2, SkillName.Forensics, 15);
			Resistances.Poison = 10;
			Resistances.Physical = 10;
			Attributes.BonusInt = 5;
			Attributes.RegenMana = 5;
			Attributes.BonusMana = 5;
			ArtifactLevel = 2;
			Server.Misc.Arty.ArtySetup( this, "" );
		}

		public Artifact_RosaryOfMyrkul( Serial serial ) : base( serial )
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

			if ( Hue == 0x12B )
				Hue = 0x554;
		}
	}
}