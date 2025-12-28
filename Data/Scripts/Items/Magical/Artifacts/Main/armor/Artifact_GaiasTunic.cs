using System;
using Server;

namespace Server.Items
{
	public class Artifact_GaiasTunic : GiftLeatherChest
	{
		public override int InitMinHits{ get{ return 80; } }
		public override int InitMaxHits{ get{ return 160; } }

		[Constructable]
		public Artifact_GaiasTunic()
		{
			Name = "Gaia's Tunic";
			Hue = 669;
			SkillBonuses.SetValues( 0, SkillName.Veterinary, 10);
			SkillBonuses.SetValues( 1, SkillName.Druidism, 10);
			ArmorAttributes.MageArmor = 1;
			Attributes.DefendChance = 15;
			Attributes.CastSpeed = 1;
			Attributes.CastRecovery = 1;
			Attributes.LowerManaCost = 5;
			Attributes.LowerRegCost = 5;
			Attributes.SpellDamage = 5;
			ArtifactLevel = 2;
			Server.Misc.Arty.ArtySetup( this, "" );
		}

		public Artifact_GaiasTunic( Serial serial ) : base( serial )
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