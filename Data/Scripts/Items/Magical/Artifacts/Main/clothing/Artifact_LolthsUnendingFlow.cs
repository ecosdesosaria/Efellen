
using System;
using Server.Network;
using Server.Items;
using Server.Targeting;

namespace Server.Items
{
	public class Artifact_LolthsUnendingFlow : GiftRobe
	{
		[Constructable]
		public Artifact_LolthsUnendingFlow()
		{
            ItemID = 0x2652;
			Name = "Lolth's unending flow";
			Hue = 1316;
			SkillBonuses.SetValues( 0, SkillName.Psychology, 15);
			SkillBonuses.SetValues( 1, SkillName.Magery, 15);
            Resistances.Poison = 50;
			Attributes.BonusInt = 10;
			Attributes.SpellDamage = 20;
			ArtifactLevel = 2;
			Server.Misc.Arty.ArtySetup( this, "" );
		}

		public Artifact_LolthsUnendingFlow( Serial serial ) : base( serial )
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
