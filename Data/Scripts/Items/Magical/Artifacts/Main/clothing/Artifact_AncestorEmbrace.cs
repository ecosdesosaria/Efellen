using System;
using Server;

namespace Server.Items
{
	public class Artifact_AncestorEmbrace : GiftFurCape
	{
		[Constructable]
		public Artifact_AncestorEmbrace()
		{
			Name = "Ancestor's Embrace";
			Hue = 660;
			SkillBonuses.SetValues( 0, SkillName.Druidism, 10);
			SkillBonuses.SetValues( 1, SkillName.Spiritualism, 10);
			Resistances.Physical = 50;
			Attributes.DefendChance = 15;
			ArtifactLevel = 2;
			Server.Misc.Arty.ArtySetup( this, "" );
		}

		public Artifact_AncestorEmbrace( Serial serial ) : base( serial )
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