using System;
using Server;

namespace Server.Items
{
	public class Artifact_CoifOfDevotion : GiftChainCoif
	{
		public override int InitMinHits{ get{ return 80; } }
		public override int InitMaxHits{ get{ return 160; } }

		public override int BasePhysicalResistance{ get{ return 11; } }
		public override int BaseFireResistance{ get{ return 7; } }
		public override int BaseColdResistance{ get{ return 7; } }
		public override int BasePoisonResistance{ get{ return 2; } }
		public override int BaseEnergyResistance{ get{ return 4; } }

		[Constructable]
		public Artifact_CoifOfDevotion()
		{
			Name = "Coif of Devotion";
			ItemID = 0x13BB;
            Hue = 0x9C2;
			SkillBonuses.SetValues( 0, SkillName.Healing, 10);
			SkillBonuses.SetValues( 1, SkillName.Spiritualism, 10);
			ArmorAttributes.MageArmor = 1;
			Attributes.NightSight = 1;
			Attributes.ReflectPhysical = 20;
			Attributes.BonusMana = 10;
			Attributes.BonusStam = 10;
			Attributes.Luck = 50;
			ArtifactLevel = 2;
			Server.Misc.Arty.ArtySetup( this, "" );
		}

		public Artifact_CoifOfDevotion( Serial serial ) : base( serial )
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