
using System;
using Server.Network;
using Server.Items;
using Server.Targeting;

namespace Server.Items
{
	public class Artifact_LolthsDomination : GiftRobe
	{
		[Constructable]
		public Artifact_LolthsDomination()
		{
            ItemID = 0x2FC6;
			Name = "Lolth's Domination";
			Hue = 2346;
			SkillBonuses.SetValues( 0, SkillName.MagicResist, 20);
			SkillBonuses.SetValues( 1, SkillName.Poisoning, 15);
			Attributes.BonusInt = 10;
			Attributes.SpellDamage = 20;
			Attributes.BonusMana = 10;
			Attributes.WeaponSpeed = 15;
			ArtifactLevel = 2;
			Server.Misc.Arty.ArtySetup( this, "" );
		}

		public Artifact_LolthsDomination( Serial serial ) : base( serial )
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
