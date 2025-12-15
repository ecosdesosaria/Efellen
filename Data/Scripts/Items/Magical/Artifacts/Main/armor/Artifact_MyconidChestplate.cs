using System;
using Server;

namespace Server.Items
{
	public class Artifact_MyconidChestplate : GiftWoodenPlateChest
	{
		public override int InitMinHits{ get{ return 80; } }
		public override int InitMaxHits{ get{ return 160; } }

		public override int BasePhysicalResistance{ get{ return 9; } }
		public override int BaseFireResistance{ get{ return 3; } }
		public override int BaseColdResistance{ get{ return 7; } }
		public override int BasePoisonResistance{ get{ return 15; } }
		public override int BaseEnergyResistance{ get{ return 9; } }

		[Constructable]
		public Artifact_MyconidChestplate()
		{
			Name = "Chestplate of the Myconid";
			Hue = 0x497;
			ArtifactLevel = 2;
			SkillBonuses.SetValues( 0, SkillName.Poisoning, 15);
			SkillBonuses.SetValues( 1, SkillName.Alchemy, 15);
			ArmorAttributes.MageArmor = 1;
			Attributes.DefendChance = 8;
			Attributes.WeaponDamage = 5;
			Attributes.SpellDamage = 5;
			Attributes.BonusStr = 5;
			Attributes.BonusInt = 5;
			Server.Misc.Arty.ArtySetup( this, "" );
		}

		public Artifact_MyconidChestplate( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 );
		}
		
		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize( reader );
			ArtifactLevel = 2;

			int version = reader.ReadInt();
		}
	}
}