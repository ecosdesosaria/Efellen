using System;
using Server;

namespace Server.Items
{
	public class Artifact_NecklaceOfTheFateweaver : GiftGoldNecklace
	{
		[Constructable]
		public Artifact_NecklaceOfTheFateweaver()
		{
			Name = "Necklace of the Fateweaver";
			Hue = 0x96;
			SkillBonuses.SetValues( 0, SkillName.Poisoning, 25);
			SkillBonuses.SetValues( 1, SkillName.Fencing, 20);
			Attributes.CastRecovery = 2;
			Attributes.CastSpeed = 2;
			Attributes.WeaponDamage = 20;
			Resistances.Poison = 25;
			Attributes.RegenStam = 5;
			Attributes.RegenMana = 5;
			ArtifactLevel = 2;
			Server.Misc.Arty.ArtySetup( this, "" );
		}

		public Artifact_NecklaceOfTheFateweaver( Serial serial ) : base( serial )
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

			if ( Hue == 0x12B )
				Hue = 0x554;
		}
	}
}