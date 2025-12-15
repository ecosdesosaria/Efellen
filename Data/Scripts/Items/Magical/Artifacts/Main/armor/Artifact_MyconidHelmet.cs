using System;
using Server;

namespace Server.Items
{
	public class Artifact_MyconidHelmet : GiftWoodenPlateHelm
	{
		public override int InitMinHits{ get{ return 80; } }
		public override int InitMaxHits{ get{ return 160; } }

		public override int BasePhysicalResistance{ get{ return 10; } }
		public override int BaseFireResistance{ get{ return 3; } }
		public override int BaseColdResistance{ get{ return 5; } }
		public override int BasePoisonResistance{ get{ return 7; } }
		public override int BaseEnergyResistance{ get{ return 7; } }

		[Constructable]
		public Artifact_MyconidHelmet()
		{
			Name = "Helmet of the Myconid";
			Hue = 0x497;
			ArtifactLevel = 2;
			SkillBonuses.SetValues( 0, SkillName.Poisoning, 10);
			SkillBonuses.SetValues( 1, SkillName.Alchemy, 10);
			Attributes.DefendChance = 5;
			Attributes.WeaponDamage = 10;
			Attributes.BonusInt = 10;
			Attributes.SpellDamage = 10;
			Attributes.NightSight = 1;
			ArmorAttributes.MageArmor = 1;
			Server.Misc.Arty.ArtySetup( this, "" );
		}

		public Artifact_MyconidHelmet( Serial serial ) : base( serial )
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