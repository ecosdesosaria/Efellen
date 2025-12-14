using System;
using Server;

namespace Server.Items
{
	public class Artifact_ArmsOfTheBlackSabbath : GiftLeatherArms
	{
		public override int InitMinHits{ get{ return 80; } }
		public override int InitMaxHits{ get{ return 160; } }

		public override int BasePhysicalResistance{ get{ return 8; } }
		public override int BaseFireResistance{ get{ return 11; } }
		public override int BaseColdResistance{ get{ return 7; } }
		public override int BasePoisonResistance{ get{ return 7; } }
		public override int BaseEnergyResistance{ get{ return 4; } }

		[Constructable]
		public Artifact_ArmsOfTheBlackSabbath()
		{
			Name = "Arms of the Black Sabbath";
			ItemID = 0x13cd;
			Hue = 0x497;
			SkillBonuses.SetValues( 0, SkillName.Discordance, 15);
			SkillBonuses.SetValues( 1, SkillName.Necromancy, 15);
			SkillBonuses.SetValues( 2, SkillName.Forensics, 15);
			ArmorAttributes.MageArmor = 1;
			Attributes.DefendChance = 10;
			Attributes.LowerManaCost = 10;
			Attributes.LowerRegCost = 10;
			Attributes.SpellDamage = 25;
			Attributes.Luck = 75;
			ArmorAttributes.SelfRepair = 2;
			ArtifactLevel = 2;
			Server.Misc.Arty.ArtySetup( this, "" );
		}

		public Artifact_ArmsOfTheBlackSabbath( Serial serial ) : base( serial )
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