using System;
using Server.Network;
using Server.Items;
using Server.Targeting;

namespace Server.Items
{
	public class Artifact_BayushisHonestSmile : GiftPlateMempo
	{
		public override int InitMinHits{ get{ return 80; } }
		public override int InitMaxHits{ get{ return 160; } }

		public override int BaseColdResistance{ get{ return 6; } } 
		public override int BaseEnergyResistance{ get{ return 7; } } 
		public override int BasePhysicalResistance{ get{ return 7; } } 
		public override int BasePoisonResistance{ get{ return 15; } } 
		public override int BaseFireResistance{ get{ return 6; } } 
      
		[Constructable]
		public Artifact_BayushisHonestSmile()
		{
			Name = "Sorriso Honesto de Bayushi";
			Hue = 1151;
			SkillBonuses.SetValues( 0, SkillName.Poisoning, 10 );
			SkillBonuses.SetValues( 1, SkillName.Bushido, 10 );
			ArmorAttributes.MageArmor = 1;
			Attributes.BonusInt = 3;
			Attributes.BonusMana = 10;
			Attributes.LowerManaCost = 10;
			Attributes.LowerRegCost = 10;
			Attributes.WeaponSpeed = 5;
			ArtifactLevel = 2;
			Server.Misc.Arty.ArtySetup( this, "" );
		}

		public Artifact_BayushisHonestSmile( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			ArtifactLevel = 2;

			int version = reader.ReadInt();
		}
	}
}