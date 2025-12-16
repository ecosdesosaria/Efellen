using System;
using Server;

namespace Server.Items
{
	public class Artifact_RingOfTheDreamweaver : GiftGoldRing
	{
		[Constructable]
		public Artifact_RingOfTheDreamweaver()
		{
			Name = "Ring of the Dreamweaver";
			ItemID = 0x6731;
			Hue = 0x96;
			SkillBonuses.SetValues( 0, SkillName.Psychology, 15);
			SkillBonuses.SetValues( 1, SkillName.Meditation, 15);
			SkillBonuses.SetValues( 2, SkillName.Provocation, 15);
			Attributes.CastRecovery = 2;
			Attributes.CastSpeed = 2;
			Attributes.LowerManaCost = 10;
			Attributes.LowerRegCost = 10;
			Resistances.Energy = 25;
			Attributes.RegenMana = 10;
			ArtifactLevel = 2;
			Server.Misc.Arty.ArtySetup( this, "" );
		}

		public Artifact_RingOfTheDreamweaver( Serial serial ) : base( serial )
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