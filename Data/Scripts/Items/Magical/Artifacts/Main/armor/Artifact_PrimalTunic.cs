using System;
using Server;

namespace Server.Items
{
	public class Artifact_PrimalTunic : GiftBoneChest
	{
		public override int InitMinHits{ get{ return 80; } }
		public override int InitMaxHits{ get{ return 160; } }

		public override int BasePoisonResistance{ get{ return 15; } }
	public override int BasePhysicalResistance{ get{ return 15; } }

		[Constructable]
		public Artifact_PrimalTunic()
		{
			Name = "Primal Tunic";
			Hue = 0x995;
			SkillBonuses.SetValues( 0, SkillName.Tactics, 10);
			SkillBonuses.SetValues( 1, SkillName.Tracking, 10);
			SkillBonuses.SetValues( 2, SkillName.FistFighting, 15);
			Attributes.RegenHits = 10;
			Attributes.RegenStam = 10;
			Attributes.WeaponDamage = 30;
			Attributes.BonusStr = 10;
			ArtifactLevel = 2;
			Server.Misc.Arty.ArtySetup( this, "" );
		}

		public Artifact_PrimalTunic( Serial serial ) : base( serial )
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