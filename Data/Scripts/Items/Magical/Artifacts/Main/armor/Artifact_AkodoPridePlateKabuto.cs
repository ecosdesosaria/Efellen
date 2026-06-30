using System;
using Server;

namespace Server.Items
{
	public class Artifact_AkodoPridePlateKabuto : GiftDecorativePlateKabuto
	{
		public override int InitMinHits{ get{ return 80; } }
		public override int InitMaxHits{ get{ return 160; } }

		public override int BasePhysicalResistance{ get{ return 7; } }
		public override int BaseFireResistance{ get{ return 7; } }
		public override int BaseColdResistance{ get{ return 5; } }
		public override int BasePoisonResistance{ get{ return 6; } }
		public override int BaseEnergyResistance{ get{ return 6; } }

		[Constructable]
		public Artifact_AkodoPridePlateKabuto()
		{
			Name = "Plate Kabuto do Orgulho de Akodo";
			Hue = 548;
			SkillBonuses.SetValues( 0, SkillName.ArmsLore, 10 );
			SkillBonuses.SetValues( 1, SkillName.Bushido, 10 );
			Attributes.BonusStr = 5;
			Attributes.BonusHits = 5;
			Attributes.DefendChance = 10;
			Attributes.AttackChance = 5;
			ArtifactLevel = 2;
			Server.Misc.Arty.ArtySetup( this, "" );
		}

		public Artifact_AkodoPridePlateKabuto( Serial serial ) : base( serial )
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