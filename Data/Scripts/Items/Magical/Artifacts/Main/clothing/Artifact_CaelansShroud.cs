using System;
using Server;

namespace Server.Items
{
	public class Artifact_CaelansShroud : GiftRobe
	{
		[Constructable]
		public Artifact_CaelansShroud()
		{
			ItemID = 8270;
			Name = "Caelan's Shroud";
			Hue = 0x0AA5;
			SkillBonuses.SetValues( 0, SkillName.Focus, 15);
			SkillBonuses.SetValues( 1, SkillName.MagicResist, 15);
			Attributes.DefendChance = 10;
			Resistances.Physical = 15;
			Resistances.Fire = 15;
			Attributes.LowerManaCost = 10;
			ArtifactLevel = 2;
			Server.Misc.Arty.ArtySetup( this, "" );
		}

		public Artifact_CaelansShroud( Serial serial ) : base( serial )
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