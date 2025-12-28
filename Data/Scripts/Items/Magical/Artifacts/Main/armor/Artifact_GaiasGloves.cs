using System;
using Server;

namespace Server.Items
{
	public class Artifact_GaiasGloves : GiftLeatherGloves
	{
		public override int InitMinHits{ get{ return 80; } }
		public override int InitMaxHits{ get{ return 160; } }

		[Constructable]
		public Artifact_GaiasGloves()
		{
			Name = "Gaia's Gloves";
			Hue = 669;
			SkillBonuses.SetValues( 0, SkillName.Veterinary, 10);
			SkillBonuses.SetValues( 1, SkillName.Druidism, 10);
			ArmorAttributes.MageArmor = 1;
			Attributes.CastSpeed = 1;
			Attributes.LowerManaCost = 10;
			Attributes.LowerRegCost = 20;
			Attributes.BonusMana = 10;
			ArtifactLevel = 2;
			Server.Misc.Arty.ArtySetup( this, "" );
		}

		public Artifact_GaiasGloves( Serial serial ) : base( serial )
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