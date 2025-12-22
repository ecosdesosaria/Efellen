using System;
using Server;

namespace Server.Items
{
	public class Artifact_PrimalCrown : GiftBoneHelm
	{
		public override int InitMinHits{ get{ return 80; } }
		public override int InitMaxHits{ get{ return 160; } }
		public override int BasePoisonResistance{ get{ return 12; } }
        public override int BaseEnergyResistance{ get{ return 11; } }

		[Constructable]
		public Artifact_PrimalCrown()
		{
			Name = "Primal Crown";
			Hue = 0x995;
			SkillBonuses.SetValues( 0, SkillName.Tactics, 15);
			SkillBonuses.SetValues( 1, SkillName.Tracking, 10);
			SkillBonuses.SetValues( 2, SkillName.FistFighting, 15);
			Attributes.BonusStr = 10;
			Attributes.BonusDex = 10;
			Attributes.WeaponDamage = 25;
			ArtifactLevel = 2;
			Server.Misc.Arty.ArtySetup( this, "" );
		}

		public Artifact_PrimalCrown( Serial serial ) : base( serial )
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

			if ( version < 1 )
			{
				if ( Hue == 0x55A )
					Hue = 0x4F6;

				PoisonBonus = 0;
			}
		}
	}
}