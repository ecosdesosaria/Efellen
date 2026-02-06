using System;
using Server;

namespace Server.Items
{
	public class Artifact_RobeOfTesting : GiftRobe
	{
		[Constructable]
		public Artifact_RobeOfTesting()
		{
			ItemID = 0x1F04;
			Name = "Robe of testing";
			Hue = 0x486;;
			Resistances.Physical = 70;
			Resistances.Cold = 70;
			Resistances.Fire = 70;
			Resistances.Poison = 70;
			Resistances.Energy = 70;
			Attributes.CastRecovery = 6;
			Attributes.CastSpeed = 6;
			Attributes.LowerManaCost = 40;
			Attributes.BonusHits = 500;
			Attributes.BonusStam = 500;
			Attributes.NightSight = 1;
            Attributes.DefendChance = 45;
            Attributes.AttackChance = 45;
			Attributes.BonusStr = 150;
			Attributes.BonusDex = 150;
			Attributes.BonusInt = 150;
			Attributes.Luck = 2000;
			ArtifactLevel = 2;
			Server.Misc.Arty.ArtySetup( this, "" );
		}

		public Artifact_RobeOfTesting( Serial serial ) : base( serial )
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