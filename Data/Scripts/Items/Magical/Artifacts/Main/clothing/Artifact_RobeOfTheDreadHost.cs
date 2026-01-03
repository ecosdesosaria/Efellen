using System;
using Server;

namespace Server.Items
{
	public class Artifact_RobeOfTheDreadHost : GiftRobe
	{
		[Constructable]
		public Artifact_RobeOfTheDreadHost()
		{
			ItemID = 8270;
			Name = "Robe of the Dread Host";
			Hue = 1109;
			SkillBonuses.SetValues( 0, SkillName.Necromancy, 15);
			SkillBonuses.SetValues( 1, SkillName.MagicResist, 15);
			Attributes.SpellDamage = 25;
			Resistances.Physical = 10;
			Attributes.CastRecovery = 1;
			Attributes.CastSpeed = 1;
			Attributes.LowerManaCost = 10;
			ArtifactLevel = 2;
			Server.Misc.Arty.ArtySetup( this, "" );
		}

		public Artifact_RobeOfTheDreadHost( Serial serial ) : base( serial )
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