using System;
using Server;

namespace Server.Items
{
	public class Artifact_PrimalClaws : GiftPugilistGloves
	{
		public override int InitMinHits{ get{ return 80; } }
		public override int InitMaxHits{ get{ return 160; } }

		[Constructable]
		public Artifact_PrimalClaws()
		{
			Name = "Primal Claws";
			Weight = 2.0;
			Hue = 0x995;
			SkillBonuses.SetValues( 0, SkillName.Tactics, 15);
			SkillBonuses.SetValues( 1, SkillName.Tracking, 10);
			SkillBonuses.SetValues( 2, SkillName.FistFighting, 15);
			Attributes.BonusDex = 10;
			Attributes.WeaponDamage = 20;
			WeaponAttributes.HitLeechHits = 30;
			MinDamage = 15;
			MaxDamage = 21;
			ArtifactLevel = 2;
			Server.Misc.Arty.ArtySetup( this, "" );
		}

		public Artifact_PrimalClaws( Serial serial ) : base( serial )
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