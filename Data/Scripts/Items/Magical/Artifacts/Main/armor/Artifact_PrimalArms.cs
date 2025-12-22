using System;
using Server;

namespace Server.Items
{
	public class Artifact_PrimalArms : GiftBoneArms
	{
		public override int InitMinHits{ get{ return 80; } }
		public override int InitMaxHits{ get{ return 160; } }

		[Constructable]
		public Artifact_PrimalArms()
		{
			Name = "Primal Arms";
			Hue = 0x995;
			SkillBonuses.SetValues( 0, SkillName.Tactics, 10);
			SkillBonuses.SetValues( 1, SkillName.Tracking, 10);
			SkillBonuses.SetValues( 2, SkillName.FistFighting, 15);
			Attributes.BonusDex = 10;
			Attributes.BonusStam = 10;
			Attributes.RegenStam = 10;
			Attributes.WeaponSpeed = 10;
			Attributes.WeaponDamage = 10;
			ArtifactLevel = 2;
			Server.Misc.Arty.ArtySetup( this, "" );
		}

		public Artifact_PrimalArms( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 1 );
		}
		
		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize( reader );
			ArtifactLevel = 2;
			int version = reader.ReadInt();
		}
	}
}