using System;
using Server;

namespace Server.Items
{
	public class Artifact_LegsOfThePainSlave : GiftStuddedLegs
	{
		public override int InitMinHits{ get{ return 80; } }
		public override int InitMaxHits{ get{ return 160; } }

		public override int BasePhysicalResistance{ get{ return 4; } }
		public override int BaseFireResistance{ get{ return 9; } }
		public override int BaseColdResistance{ get{ return 6; } }
		public override int BasePoisonResistance{ get{ return 7; } }
		public override int BaseEnergyResistance{ get{ return 5; } }

		[Constructable]
		public Artifact_LegsOfThePainSlave()
		{
			Name = "Legging of the Pain Slave";
			Hue = 0xb73;
			SkillBonuses.SetValues( 0, SkillName.Bludgeoning, 10);
			SkillBonuses.SetValues( 1, SkillName.Anatomy, 10);
			ArmorAttributes.MageArmor = 1;
			Attributes.Luck = 100;
			Attributes.DefendChance = 10;
			Attributes.LowerManaCost = 10;
			ArtifactLevel = 2;
			Server.Misc.Arty.ArtySetup( this, "" );
		}

		public Artifact_LegsOfThePainSlave( Serial serial ) : base( serial )
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