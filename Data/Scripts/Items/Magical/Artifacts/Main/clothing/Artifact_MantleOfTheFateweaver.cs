using System;
using Server;

namespace Server.Items
{
	public class Artifact_MantleOfTheFateweaver : GiftWizardsHat
	{
		[Constructable]
		public Artifact_MantleOfTheFateweaver()
		{
			ItemID = 0x5C14;
			Name = "Hood of the Fateweaver";
			Hue = 2498;
			SkillBonuses.SetValues( 0, SkillName.Poisoning, 10);
			SkillBonuses.SetValues( 1, SkillName.Fencing, 10);
			Attributes.CastRecovery = 1;
			Attributes.CastSpeed = 1;
			Attributes.LowerManaCost = 10;
			Attributes.LowerRegCost = 10;
			Attributes.RegenStam = 5;
			Attributes.RegenMana = 5;
			Attributes.SpellDamage = 25;
           Server.Misc.Arty.ArtySetup( this, "" );
		}

		public Artifact_MantleOfTheFateweaver( Serial serial ) : base( serial )
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