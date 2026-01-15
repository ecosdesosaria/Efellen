
using System;
using Server.Network;
using Server.Items;
using Server.Targeting;

namespace Server.Items
{
	public class Artifact_PrismaticRobeOfBalTsareth : GiftRobe
	{
		[Constructable]
		public Artifact_PrismaticRobeOfBalTsareth()
		{
            ItemID = 0x2652;
			Name = "Prismatic Robe Of Bal Tsareth";
			Hue = 0x0213;
			SkillBonuses.SetValues( 0, SkillName.Psychology, 20);
			SkillBonuses.SetValues( 1, SkillName.Inscribe, 10);
            Resistances.Physical = 10;
            Resistances.Cold = 10;
            Resistances.Fire = 10;
            Resistances.Energy = 10;
            Resistances.Poison = 10;
			Attributes.BonusInt = 10;
			Attributes.SpellDamage = 20;
			ArtifactLevel = 2;
			Server.Misc.Arty.ArtySetup( this, "" );
		}

		public Artifact_PrismaticRobeOfBalTsareth( Serial serial ) : base( serial )
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
