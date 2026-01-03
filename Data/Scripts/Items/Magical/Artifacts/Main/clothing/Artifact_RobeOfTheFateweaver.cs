
using System;
using Server.Network;
using Server.Items;
using Server.Targeting;

namespace Server.Items
{
	public class Artifact_RobeOfTheFateweaver : GiftRobe
	{
		[Constructable]
		public Artifact_RobeOfTheFateweaver()
		{
            ItemID = 0x2FC6;
			Name = "Robe Of the Fateweaver";
			Hue = 2498;
			SkillBonuses.SetValues( 0, SkillName.Poisoning, 20);
			SkillBonuses.SetValues( 1, SkillName.Fencing, 10);
			Attributes.BonusInt = 5;
			Attributes.BonusDex = 5;
			Attributes.SpellDamage = 20;
			Attributes.BonusMana = 10;
			Attributes.BonusStam = 10;
			ArtifactLevel = 2;
			Server.Misc.Arty.ArtySetup( this, "" );
		}

		public Artifact_RobeOfTheFateweaver( Serial serial ) : base( serial )
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
