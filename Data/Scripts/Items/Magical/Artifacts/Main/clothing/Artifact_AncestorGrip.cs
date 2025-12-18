using System;
using Server;

namespace Server.Items
{
	public class Artifact_AncestorGrip : GiftBelt
	{
		[Constructable]
		public Artifact_AncestorGrip()
		{
			ItemID = 0x2790;
			Name = "Ancestor's Grip";
			Hue = 660;
			SkillBonuses.SetValues( 0, SkillName.Druidism, 10);
			SkillBonuses.SetValues( 1, SkillName.Spiritualism, 10);
			Attributes.BonusStr = 10;
			Attributes.BonusInt = 10;
			Attributes.WeaponDamage = 10;
			ArtifactLevel = 2;
			Server.Misc.Arty.ArtySetup( this, "" );
		}

		public Artifact_AncestorGrip( Serial serial ) : base( serial )
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