using System;
using Server;

namespace Server.Items
{
	public class Artifact_AkodoPrideHiroSode : GiftPlateHiroSode
	{
		public override int InitMinHits{ get{ return 80; } }
		public override int InitMaxHits{ get{ return 160; } }

		public override int BasePhysicalResistance{ get{ return 9; } }
		public override int BaseFireResistance{ get{ return 7; } }
		public override int BaseColdResistance{ get{ return 5; } }
		public override int BasePoisonResistance{ get{ return 5; } }
		public override int BaseEnergyResistance{ get{ return 5; } }

		[Constructable]
		public Artifact_AkodoPrideHiroSode()
		{
			Name = "Hiro Sode do Orgulho de Akodo";
			Hue = 548;
			SkillBonuses.SetValues( 0, SkillName.Swords, 10 );
			SkillBonuses.SetValues( 1, SkillName.Bushido, 10 );
			Attributes.BonusStr = 5;
			Attributes.BonusHits = 5;
			Attributes.CastSpeed = 2;
			Attributes.CastRecovery = 2;
			Attributes.WeaponSpeed = 10;
			ArtifactLevel = 2;
			Server.Misc.Arty.ArtySetup( this, "" );
		}

		public Artifact_AkodoPrideHiroSode( Serial serial ) : base( serial )
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