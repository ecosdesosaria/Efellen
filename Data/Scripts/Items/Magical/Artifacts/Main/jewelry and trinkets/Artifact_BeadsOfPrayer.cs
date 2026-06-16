using System;
using Server;

namespace Server.Items
{
	public class Artifact_BeadsOfPrayer : GiftGoldBeadNecklace
	{
		[Constructable]
		public Artifact_BeadsOfPrayer()
		{
			Name = "Colar das Preces";
			Hue = 0x0672;
			SkillBonuses.SetValues( 0, SkillName.Healing, 15);
			SkillBonuses.SetValues( 1, SkillName.Spiritualism, 15);
			SkillBonuses.SetValues( 2, SkillName.Knightship, 15);
			Attributes.DefendChance = 10;
			Resistances.Poison = 20;
			Attributes.BonusInt = 8;
			Attributes.RegenMana = 5;
			Attributes.BonusMana = 5;
			ArtifactLevel = 2;
			Server.Misc.Arty.ArtySetup( this, "" );
		}

		public Artifact_BeadsOfPrayer( Serial serial ) : base( serial )
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