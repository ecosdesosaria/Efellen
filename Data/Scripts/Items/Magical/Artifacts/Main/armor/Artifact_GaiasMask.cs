using System;
using Server;

namespace Server.Items
{
	public class Artifact_GaiasMask : GiftWolfMask
	{
		public override int InitMinHits{ get{ return 80; } }
		public override int InitMaxHits{ get{ return 160; } }

		public override int BasePhysicalResistance{ get{ return 8; } }
		public override int BaseFireResistance{ get{ return 6; } }
		public override int BaseColdResistance{ get{ return 6; } }
		public override int BasePoisonResistance{ get{ return 7; } }
		public override int BaseEnergyResistance{ get{ return 7; } }

		[Constructable]
		public Artifact_GaiasMask()
		{
			Name = "Mask of Gaia";
			ArtifactLevel = 2;
			Hue = 669;
			SkillBonuses.SetValues( 0, SkillName.Veterinary, 15);
			SkillBonuses.SetValues( 1, SkillName.Druidism, 15);
			Attributes.AttackChance = 10;
			Attributes.WeaponSpeed = 10;
			Attributes.WeaponDamage = 10;
			Attributes.BonusStr = 5;
			Server.Misc.Arty.ArtySetup( this, "" );
		}

		public Artifact_GaiasMask( Serial serial ) : base( serial )
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