using System;
using Server.Mobiles;

namespace Server.Items
{
	public class Artifact_TemptationOfTheDreadHost : GiftGoldNecklace
	{
		[Constructable]
		public Artifact_TemptationOfTheDreadHost()
		{
			Name = "Temptation of the Dread Host";
			Hue = 1109;
			SkillBonuses.SetValues( 0, SkillName.Necromancy, 15);
			SkillBonuses.SetValues( 1, SkillName.MagicResist, 15);
			Attributes.Luck = 50;
			Attributes.SpellDamage = 25;
			ArtifactLevel = 2;
			Server.Misc.Arty.ArtySetup( this, "" );
		}

		public Artifact_TemptationOfTheDreadHost( Serial serial ) : base( serial )
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
			reader.ReadInt();
		}
	}
}
