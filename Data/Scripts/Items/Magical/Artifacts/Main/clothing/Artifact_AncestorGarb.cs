
using System;
using Server.Network;
using Server.Items;
using Server.Targeting;

namespace Server.Items
{
	public class Artifact_AncestorGarb : GiftRobe
	{
		[Constructable]
		public Artifact_AncestorGarb()
		{
			Name = "Ancestor's Garb";
			Hue = 660;
			SkillBonuses.SetValues( 0, SkillName.Druidism, 15);
			SkillBonuses.SetValues( 1, SkillName.Spiritualism, 15);
			Attributes.RegenHits = 10;
			Attributes.RegenMana = 5;
			Attributes.RegenStam = 5;
			Attributes.SpellDamage = 10;
			Attributes.WeaponDamage = 10;

			ArtifactLevel = 2;
			Server.Misc.Arty.ArtySetup( this, "" );
		}

		public Artifact_AncestorGarb( Serial serial ) : base( serial )
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
