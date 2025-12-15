using System;
using Server;

namespace Server.Items
{
	public class Artifact_AnnointedArms : GiftRoyalArms
	{
		public override int InitMinHits{ get{ return 80; } }
		public override int InitMaxHits{ get{ return 160; } }

		public override int BasePhysicalResistance{ get{ return 10; } }
		public override int BaseFireResistance{ get{ return 10; } }
		public override int BaseColdResistance{ get{ return 6; } }
		public override int BasePoisonResistance{ get{ return 10; } }
		public override int BaseEnergyResistance{ get{ return 12; } }

		[Constructable]
		public Artifact_AnnointedArms()
		{
			Name = "Annointed Arm Plates";
			Hue = 0x0672;
			SkillBonuses.SetValues( 0, SkillName.Knightship, 15);
			SkillBonuses.SetValues( 1, SkillName.MagicResist, 15);
			SkillBonuses.SetValues( 2, SkillName.Parry, 15);
			Attributes.DefendChance = 10;
			Attributes.ReflectPhysical = 20;
			Attributes.BonusDex = 10;
			Attributes.BonusHits = 10;
			Attributes.Luck = 60;
			ArtifactLevel = 2;
			Server.Misc.Arty.ArtySetup( this, "" );
		}

		public Artifact_AnnointedArms( Serial serial ) : base( serial )
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