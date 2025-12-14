using System;
using Server;

namespace Server.Items
{
	public class Artifact_BootsOfThePainSlave : GiftThighBoots
	{

		[Constructable]
		public Artifact_BootsOfThePainSlave()
		{
			Name = "Boots of The Pain Slave";
			Hue = 0xb73;
			SkillBonuses.SetValues( 0, SkillName.Bludgeoning, 10);
			SkillBonuses.SetValues( 1, SkillName.Anatomy, 10);
			Attributes.Luck = 100;
			Attributes.DefendChance = 5;
            Attributes.RegenStam = 5;
            Attributes.CastSpeed = 1;
            Attributes.CastRecovery = 1;

			ArtifactLevel = 2;
			Server.Misc.Arty.ArtySetup( this, "" );
		}

		public Artifact_BootsOfThePainSlave( Serial serial ) : base( serial )
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