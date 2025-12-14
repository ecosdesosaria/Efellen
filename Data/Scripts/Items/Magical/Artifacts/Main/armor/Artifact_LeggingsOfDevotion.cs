using System;
using Server.Network;
using Server.Items;

namespace Server.Items
{
	public class Artifact_LeggingsOfDevotion : GiftChainLegs
	{
		public override int InitMinHits{ get{ return 80; } }
		public override int InitMaxHits{ get{ return 160; } }

		public override int BasePhysicalResistance{ get{ return 12; } }
		public override int BaseFireResistance{ get{ return 8; } }
		public override int BaseColdResistance{ get{ return 7; } }
		public override int BasePoisonResistance{ get{ return 4; } }
		public override int BaseEnergyResistance{ get{ return 19; } }

		[Constructable]
		public Artifact_LeggingsOfDevotion()
		{
			Hue = 0xB51;
			ItemID = 0x13BE;
			Name = "Leggings of Devotion";
			Hue = 0x9C2;
			SkillBonuses.SetValues( 0, SkillName.Healing, 10);
			SkillBonuses.SetValues( 1, SkillName.Spiritualism, 10);
			ArmorAttributes.MageArmor = 1;
            Attributes.BonusHits = 10;
            Attributes.WeaponSpeed = 5;
			Attributes.DefendChance = 20;
			ArtifactLevel = 2;
			Server.Misc.Arty.ArtySetup( this, "" );
		}

		public Artifact_LeggingsOfDevotion( Serial serial ) : base( serial )
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