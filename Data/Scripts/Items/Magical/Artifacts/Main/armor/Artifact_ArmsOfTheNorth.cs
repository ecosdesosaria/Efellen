using System;
using Server;

namespace Server.Items
{
	public class Artifact_ArmsOfTheNorth : GiftRoyalArms
	{
		public override int InitMinHits{ get{ return 80; } }
		public override int InitMaxHits{ get{ return 160; } }

		public override int BasePhysicalResistance{ get{ return 10; } }
		public override int BaseFireResistance{ get{ return 5; } }
		public override int BaseColdResistance{ get{ return 16; } }
		public override int BasePoisonResistance{ get{ return 5; } }
		public override int BaseEnergyResistance{ get{ return 12; } }

		[Constructable]
		public Artifact_ArmsOfTheNorth()
		{
			Name = "Arm Plates of the North";
			Hue = 1164;
			SkillBonuses.SetValues( 1, SkillName.Tactics, 10);
			SkillBonuses.SetValues( 2, SkillName.Swords, 10);
			Attributes.DefendChance = 10;
			Attributes.BonusDex = 10;
			Attributes.BonusStam = 10;
			ArtifactLevel = 2;
			Server.Misc.Arty.ArtySetup( this, "" );
		}

		public Artifact_ArmsOfTheNorth( Serial serial ) : base( serial )
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