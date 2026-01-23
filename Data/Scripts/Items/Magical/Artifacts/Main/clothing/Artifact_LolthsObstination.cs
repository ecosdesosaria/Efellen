using System;
using Server;

namespace Server.Items
{
	public class Artifact_LolthsObstination : GiftThighBoots
	{

		[Constructable]
		public Artifact_LolthsObstination()
		{
			Name = "Lolth's Obstination";
			Hue = 2346;
			SkillBonuses.SetValues( 0, SkillName.MagicResist, 10);
			SkillBonuses.SetValues( 1, SkillName.Poisoning, 10);
			Attributes.Luck = 100;
			Attributes.DefendChance = 5;
            Attributes.RegenStam = 10;

			ArtifactLevel = 2;
			Server.Misc.Arty.ArtySetup( this, "" );
		}

		public Artifact_LolthsObstination( Serial serial ) : base( serial )
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