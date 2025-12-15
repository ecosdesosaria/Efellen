using System;
using Server;

namespace Server.Items
{
	public class Artifact_MyconidLeggings : GiftWoodenPlateLegs
	{
		public override int InitMinHits{ get{ return 80; } }
		public override int InitMaxHits{ get{ return 160; } }

		public override int BasePhysicalResistance{ get{ return 10; } }
		public override int BaseFireResistance{ get{ return 3; } }
		public override int BaseColdResistance{ get{ return 7; } }
		public override int BasePoisonResistance{ get{ return 13; } }
		public override int BaseEnergyResistance{ get{ return 11; } }

		[Constructable]
		public Artifact_MyconidLeggings()
		{
			Name = "Myconid Leggings";
			Hue = 0x497;
			ArtifactLevel = 2;
			SkillBonuses.SetValues( 0, SkillName.Poisoning, 10);
			SkillBonuses.SetValues( 1, SkillName.Alchemy, 10);
			Attributes.DefendChance = 5;
			Attributes.WeaponDamage = 10;
			Attributes.BonusDex = 10;
			Attributes.RegenHits = 5;
			Attributes.RegenStam = 5;
            ArmorAttributes.MageArmor = 1;
			Server.Misc.Arty.ArtySetup( this, "" );
		}

		public Artifact_MyconidLeggings( Serial serial ) : base( serial )
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