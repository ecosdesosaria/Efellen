using System;
using Server;

namespace Server.Items
{
	public class Artifact_CloakOfTheFateweaver : GiftFurCape
	{
		[Constructable]
		public Artifact_CloakOfTheFateweaver()
		{
			Name = "Cloak of the Fateweaver";
			Hue = 2498;
			SkillBonuses.SetValues( 0, SkillName.Poisoning, 10);
			SkillBonuses.SetValues( 1, SkillName.Fencing, 10);
			Resistances.Poison = 40;
			Attributes.SpellDamage = 20;
			Attributes.LowerManaCost = 10;
			Attributes.LowerRegCost = 10;
			Attributes.WeaponSpeed = 10;
			ArtifactLevel = 2;
			Server.Misc.Arty.ArtySetup( this, "" );
		}

		public Artifact_CloakOfTheFateweaver( Serial serial ) : base( serial )
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