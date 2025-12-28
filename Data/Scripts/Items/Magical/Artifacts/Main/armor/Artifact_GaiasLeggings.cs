using System;
using Server;

namespace Server.Items
{
	public class Artifact_GaiasLeggings : GiftLeatherLegs
	{
		public override int InitMinHits{ get{ return 80; } }
		public override int InitMaxHits{ get{ return 160; } }

		[Constructable]
		public Artifact_GaiasLeggings()
		{
			Name = "Gaia's Leggings";
			Hue = 669;
			SkillBonuses.SetValues( 0, SkillName.Veterinary, 10);
			SkillBonuses.SetValues( 1, SkillName.Druidism, 10);
			ArmorAttributes.MageArmor = 1;
			Attributes.DefendChance = 10;
			Attributes.LowerManaCost = 10;
			Attributes.LowerRegCost = 10;
			Attributes.SpellDamage = 10;
			ArtifactLevel = 2;
			Server.Misc.Arty.ArtySetup( this, "" );
		}

		public Artifact_GaiasLeggings( Serial serial ) : base( serial )
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