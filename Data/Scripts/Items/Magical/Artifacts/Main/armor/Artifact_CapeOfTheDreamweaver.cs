using System;
using Server;

namespace Server.Items
{
	public class Artifact_CapeOfTheDreamweaver : GiftFurCape
	{
		[Constructable]
		public Artifact_CapeOfTheDreamweaver()
		{
			Name = "Cape of the Dreamweaver";
			Hue = 0x96;
			SkillBonuses.SetValues( 0, SkillName.Psychology, 10);
			SkillBonuses.SetValues( 1, SkillName.Meditation, 10);
			SkillBonuses.SetValues( 2, SkillName.Provocation, 15);
			Resistances.Energy = 50;
			Attributes.SpellDamage = 20;
			Attributes.LowerManaCost = 10;
			Attributes.LowerRegCost = 20;
			ArtifactLevel = 2;
			Server.Misc.Arty.ArtySetup( this, "" );
		}

		public Artifact_CapeOfTheDreamweaver( Serial serial ) : base( serial )
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