
using System;
using Server.Network;
using Server.Items;
using Server.Targeting;

namespace Server.Items
{
	public class Artifact_RobeOfTheDreamweaver : GiftRobe
	{
		[Constructable]
		public Artifact_RobeOfTheDreamweaver()
		{
            ItemID = 0x302;
			Name = "Robe Of the Dreamweaver";
			Hue = 0x96;
			SkillBonuses.SetValues( 0, SkillName.Psychology, 15);
			SkillBonuses.SetValues( 1, SkillName.Meditation, 10);
			SkillBonuses.SetValues( 2, SkillName.Provocation, 10);
			Attributes.BonusInt = 10;
			Attributes.SpellDamage = 20;
			Attributes.BonusMana = 10;
		 	Attributes.CastSpeed = 2;
			Attributes.CastRecovery = 2;
			ArtifactLevel = 2;
			Server.Misc.Arty.ArtySetup( this, "" );
		}

		public Artifact_RobeOfTheDreamweaver( Serial serial ) : base( serial )
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
