using System;
using Server;

namespace Server.Items
{
	public class Artifact_CoatOfTheBlackSabbath : GiftRobe
	{
		[Constructable]
		public Artifact_CoatOfTheBlackSabbath()
		{
			ItemID = 0x567E;
			Name = "Coat of the Black Sabbath";
			Hue = 0x497;
			SkillBonuses.SetValues( 0, SkillName.Discordance, 15);
			SkillBonuses.SetValues( 1, SkillName.Necromancy, 15);
			SkillBonuses.SetValues( 2, SkillName.Forensics, 15);
			Resistances.Fire = 15;
			Resistances.Physical = 15;
			Attributes.BonusDex = 10;
			Attributes.WeaponDamage = 10;
			Attributes.ReflectPhysical = 25;
			ArtifactLevel = 2;
			Server.Misc.Arty.ArtySetup( this, "" );
		}

		public Artifact_CoatOfTheBlackSabbath( Serial serial ) : base( serial )
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