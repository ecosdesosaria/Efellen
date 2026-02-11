using System;
using Server;

namespace Server.Items
{
	public class Artifact_NatureVengeanceGorget : GiftLeatherGorget
	{
		public override int InitMinHits{ get{ return 80; } }
		public override int InitMaxHits{ get{ return 160; } }

		public override int BasePhysicalResistance{ get{ return 6; } }
		public override int BaseFireResistance{ get{ return 4; } }
		public override int BaseColdResistance{ get{ return 5; } }
		public override int BasePoisonResistance{ get{ return 6; } }
		public override int BaseEnergyResistance{ get{ return 4; } }

		[Constructable]
		public Artifact_NatureVengeanceGorget()
		{
			Name = "Gorget of Natural Vengeance";
			Hue = 0x592;
            SkillBonuses.SetValues( 0, SkillName.Elementalism, 10);
			SkillBonuses.SetValues( 1, SkillName.Taming, 10);
			SkillBonuses.SetValues( 2, SkillName.Druidism, 10);
			Attributes.SpellDamage = 20;
			ArmorAttributes.MageArmor = 1;
			Attributes.LowerManaCost = 20;
			ArtifactLevel = 2;
			Server.Misc.Arty.ArtySetup( this, "" );
		}

		public Artifact_NatureVengeanceGorget( Serial serial ) : base( serial )
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