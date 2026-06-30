using System;
using Server;

namespace Server.Items
{
	public class Artifact_AkodoPridePlateDo : GiftPlateDo
	{
		public override int InitMinHits{ get{ return 80; } }
		public override int InitMaxHits{ get{ return 160; } }

		public override int BasePhysicalResistance{ get{ return 10; } }
		public override int BaseFireResistance{ get{ return 6; } }
		public override int BaseColdResistance{ get{ return 6; } }
		public override int BasePoisonResistance{ get{ return 3; } }
		public override int BaseEnergyResistance{ get{ return 6; } }

		[Constructable]
		public Artifact_AkodoPridePlateDo()
		{
			Name = "Plate Do do Orgulho de Akodo";
			Hue = 548;
			SkillBonuses.SetValues( 0, SkillName.Tactics, 10 );
			SkillBonuses.SetValues( 1, SkillName.ArmsLore, 10 );
			Attributes.BonusStr = 5;
			Attributes.BonusHits = 5;
			Attributes.AttackChance = 10;
			Attributes.WeaponDamage = 10;
			ArtifactLevel = 2;
			Server.Misc.Arty.ArtySetup( this, "" );
		}

		public Artifact_AkodoPridePlateDo( Serial serial ) : base( serial )
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