using System;
using System.Collections;
using Server;
using Server.Network;

namespace Server.Items
{
	public class Artifact_BootsofTheDreamweaver : GiftBoots
	{
		[Constructable]
		public Artifact_BootsofTheDreamweaver()
		{
			ItemID = 0x2FC4;
			Name = "Boots of the Dreamweaver";
			Hue = 0x96;
			SkillBonuses.SetValues( 0, SkillName.Psychology, 10);
			SkillBonuses.SetValues( 1, SkillName.Meditation, 10);
			SkillBonuses.SetValues( 2, SkillName.Provocation, 10);
			Attributes.CastSpeed = 2;
			Attributes.CastRecovery = 3;
			Attributes.RegenMana = 10;
			Attributes.BonusInt = 10;
			Attributes.SpellDamage = 20;
			ArtifactLevel = 2;
			Server.Misc.Arty.ArtySetup( this, "" );
		}

		public Artifact_BootsofTheDreamweaver( Serial serial ) : base( serial )
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