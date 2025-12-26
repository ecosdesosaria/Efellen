using System;
using Server;

namespace Server.Items
{
	public class Artifact_LeoricsBreastplate : GiftRoyalChest
	{
		public override int InitMinHits{ get{ return 80; } }
		public override int InitMaxHits{ get{ return 160; } }
		public override int BasePhysicalResistance{ get{ return 12; } }
		public override int BaseFireResistance{ get{ return 10; } }
		public override int BaseColdResistance{ get{ return 9; } }
		public override int BasePoisonResistance{ get{ return 10; } }
		public override int BaseEnergyResistance{ get{ return 12; } }
		[Constructable]
		public Artifact_LeoricsBreastplate()
		{
			Name = "Leoric's Breastplate";
			Hue = 0x09d3;
			SkillBonuses.SetValues( 1, SkillName.ArmsLore, 10);
			SkillBonuses.SetValues( 2, SkillName.Swords, 10);
			Attributes.DefendChance = 10;
			Attributes.ReflectPhysical = 25;
			Attributes.BonusHits = 10;
			Attributes.BonusStr = 10;
			ArtifactLevel = 2;
			Server.Misc.Arty.ArtySetup( this, "" );
		}

		public Artifact_LeoricsBreastplate( Serial serial ) : base( serial )
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