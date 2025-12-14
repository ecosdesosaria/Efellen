using System;
using Server;

namespace Server.Items
{
	public class Artifact_LeggingsOfTheBlackSabbath : GiftLeatherLegs
	{
		public override int InitMinHits{ get{ return 80; } }
		public override int InitMaxHits{ get{ return 160; } }
		public override int BasePhysicalResistance{ get{ return 11; } }
		public override int BaseFireResistance{ get{ return 6; } }
		public override int BaseColdResistance{ get{ return 8; } }
		public override int BasePoisonResistance{ get{ return 6; } }
		public override int BaseEnergyResistance{ get{ return 8; } }

		[Constructable]
		public Artifact_LeggingsOfTheBlackSabbath()
		{
			Name = "Leggings of the Black Sabbath";
			Hue = 0x497;
			SkillBonuses.SetValues( 0, SkillName.Discordance, 15);
			SkillBonuses.SetValues( 1, SkillName.Necromancy, 15);
			SkillBonuses.SetValues( 2, SkillName.Forensics, 15);
			ArmorAttributes.MageArmor = 1;
			ItemID = 0x13cb;
			Attributes.AttackChance = 10;
			Attributes.CastSpeed = 1;
			Attributes.LowerManaCost = 10;
			Attributes.LowerRegCost = 10;
			Attributes.SpellDamage = 25;
			ArtifactLevel = 2;
			Server.Misc.Arty.ArtySetup( this, "" );
		}

		public Artifact_LeggingsOfTheBlackSabbath( Serial serial ) : base( serial )
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