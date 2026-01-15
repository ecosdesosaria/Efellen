using System;
using Server;

namespace Server.Items
{
	public class Artifact_PrismaticRingOfBalTsareth : GiftGoldRing
	{
		[Constructable]
		public Artifact_PrismaticRingOfBalTsareth()
		{
			Name = "Prismatic Ring of Bal Tsareth";
			ItemID = 0x6731;
			Hue = 0x0213;
			SkillBonuses.SetValues( 0, SkillName.Psychology, 10);
			SkillBonuses.SetValues( 1, SkillName.Inscribe, 10);
            Resistances.Physical = 10;
            Resistances.Cold = 10;
            Resistances.Fire = 10;
            Resistances.Energy = 10;
            Resistances.Poison = 10;
			Attributes.CastRecovery = 2;
			Attributes.CastSpeed = 2;
			Attributes.LowerManaCost = 10;
			Attributes.LowerRegCost = 10;
			
			Attributes.RegenMana = 10;
			ArtifactLevel = 2;
			Server.Misc.Arty.ArtySetup( this, "" );
		}

		public Artifact_PrismaticRingOfBalTsareth( Serial serial ) : base( serial )
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