using System;
using Server;

namespace Server.Items
{
	public class Artifact_AncestorWarpath : GiftBoots
	{

		[Constructable]
		public Artifact_AncestorWarpath()
		{
			Name = "Ancestor's Warpath";
			Hue = 660;
			SkillBonuses.SetValues( 0, SkillName.Druidism, 10);
			SkillBonuses.SetValues( 1, SkillName.Spiritualism, 10);
			Attributes.DefendChance = 10;
            Attributes.WeaponDamage = 10;
			Attributes.SpellDamage = 10;
			Attributes.BonusMana = 10;
			ArtifactLevel = 2;
			Server.Misc.Arty.ArtySetup( this, "" );
		}

		public Artifact_AncestorWarpath( Serial serial ) : base( serial )
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