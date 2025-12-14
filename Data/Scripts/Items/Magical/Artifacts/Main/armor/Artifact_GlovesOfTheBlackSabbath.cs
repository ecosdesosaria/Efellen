using System;
using Server;

namespace Server.Items
{
	public class Artifact_GlovesOfTheBlackSabbath : GiftLeatherGloves
	{
		public override int InitMinHits{ get{ return 80; } }
		public override int InitMaxHits{ get{ return 160; } }
		public override int BasePhysicalResistance{ get{ return 8; } }
		public override int BaseFireResistance{ get{ return 9; } }
		public override int BaseColdResistance{ get{ return 8; } }
		public override int BasePoisonResistance{ get{ return 6; } }
		public override int BaseEnergyResistance{ get{ return 7; } }

		[Constructable]
		public Artifact_GlovesOfTheBlackSabbath()
		{
			Hue = 1172;
			ItemID = 0x13C6;
			Name = "Gloves of the Black Sabbath";
			Hue = 0x497;
			SkillBonuses.SetValues( 0, SkillName.Discordance, 15);
			SkillBonuses.SetValues( 1, SkillName.Necromancy, 15);
			SkillBonuses.SetValues( 2, SkillName.Forensics, 15);
			ArmorAttributes.MageArmor = 1;
			Attributes.BonusStr = 10;
			Attributes.LowerManaCost = 10;
			Attributes.LowerRegCost = 10;
			Attributes.AttackChance = 10;
			Attributes.RegenMana = 5;
			Attributes.RegenHits = 5;
			Attributes.RegenStam = 5;
			ArtifactLevel = 2;
			Server.Misc.Arty.ArtySetup( this, "" );
		}

		public Artifact_GlovesOfTheBlackSabbath( Serial serial ) : base( serial )
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
		}
	}
}
