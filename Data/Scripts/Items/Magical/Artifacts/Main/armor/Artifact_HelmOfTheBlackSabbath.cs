using System;
using Server;

namespace Server.Items
{
	public class Artifact_HelmOfTheBlackSabbath : GiftNorseHelm
	{
		public override int InitMinHits{ get{ return 80; } }
		public override int InitMaxHits{ get{ return 160; } }
		
		public override int BasePhysicalResistance{ get{ return 11; } }
		public override int BaseFireResistance{ get{ return 6; } }
		public override int BaseColdResistance{ get{ return 8; } }
		public override int BasePoisonResistance{ get{ return 6; } }
		public override int BaseEnergyResistance{ get{ return 8; } }

		[Constructable]
		public Artifact_HelmOfTheBlackSabbath() : base()
		{
			Name = "Helm of the Black Sabbath";
			Hue = 0x497;
			SkillBonuses.SetValues( 0, SkillName.Discordance, 15);
			SkillBonuses.SetValues( 1, SkillName.Necromancy, 15);
			SkillBonuses.SetValues( 2, SkillName.Forensics, 15);
			ArmorAttributes.MageArmor = 1;
			Attributes.BonusInt = 10;
			Attributes.WeaponSpeed = 25;
			Attributes.RegenStam = 5;
			Attributes.BonusStam = 5;
			ArtifactLevel = 2;
			Server.Misc.Arty.ArtySetup( this, "" );
		}

		public Artifact_HelmOfTheBlackSabbath( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.WriteEncodedInt( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			ArtifactLevel = 2;
			int version = reader.ReadEncodedInt();
		}
	}
}
