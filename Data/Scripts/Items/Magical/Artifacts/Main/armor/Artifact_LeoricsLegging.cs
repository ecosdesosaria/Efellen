using System;
using Server;

namespace Server.Items
{
	public class Artifact_LeoricsLegging : GiftRoyalsLegs
	{
		public override int InitMinHits{ get{ return 80; } }
		public override int InitMaxHits{ get{ return 160; } }

		public override int BasePhysicalResistance{ get{ return 11; } }
		public override int BaseFireResistance{ get{ return 8; } }
		public override int BaseColdResistance{ get{ return 9; } }
		public override int BasePoisonResistance{ get{ return 10; } }
		public override int BaseEnergyResistance{ get{ return 12; } }

		[Constructable]
		public Artifact_LeoricsLegging()
		{
			Name = "Leoric's Leggings";
			Hue = 0x09d3;
			SkillBonuses.SetValues( 1, SkillName.ArmsLore, 15);
			SkillBonuses.SetValues( 2, SkillName.Swords, 15);
			Attributes.ReflectPhysical = 20;
			Attributes.BonusHits = 10;
			Attributes.BonusStam = 10;
			Attributes.WeaponSpeed = 20;
			ArtifactLevel = 2;
			Server.Misc.Arty.ArtySetup( this, "" );
		}

		public Artifact_LeoricsLegging( Serial serial ) : base( serial )
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