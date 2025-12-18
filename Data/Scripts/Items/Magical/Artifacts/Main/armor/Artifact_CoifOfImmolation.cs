using System;
using Server;

namespace Server.Items
{
	public class Artifact_CoifOfImmolation : GiftChainCoif
	{
		public override int InitMinHits{ get{ return 80; } }
		public override int InitMaxHits{ get{ return 160; } }

		public override int BasePhysicalResistance{ get{ return 8; } }
		public override int BaseFireResistance{ get{ return 12; } }
		public override int BaseColdResistance{ get{ return 5; } }
		public override int BasePoisonResistance{ get{ return 2; } }
		public override int BaseEnergyResistance{ get{ return 4; } }

		[Constructable]
		public Artifact_CoifOfImmolation()
		{
			Name = "Coif of Immolation";
			ItemID = 0x13BB;
            Hue = 348;
			SkillBonuses.SetValues( 0, SkillName.Alchemy, 10);
			SkillBonuses.SetValues( 1, SkillName.MagicResist, 10);
			ArmorAttributes.MageArmor = 1;
			Attributes.NightSight = 1;
			Attributes.EnhancePotions = 15;
			Attributes.BonusMana = 10;
			Attributes.BonusStam = 10;
			Attributes.Luck = 50;
			ArtifactLevel = 2;
			Server.Misc.Arty.ArtySetup( this, "" );
		}

		public Artifact_CoifOfImmolation( Serial serial ) : base( serial )
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