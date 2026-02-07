using System;
using Server;

namespace Server.Items
{
	public class Artifact_CaelansVisage : GiftDreadHelm
	{
		public override int InitMinHits{ get{ return 80; } }
		public override int InitMaxHits{ get{ return 160; } }
		public override int BasePoisonResistance{ get{ return 12; } }
        public override int BaseEnergyResistance{ get{ return 11; } }

		[Constructable]
		public Artifact_CaelansVisage()
		{
			Name = "Caelan's Visage";
			Hue = 0x0AA5;
			SkillBonuses.SetValues( 0, SkillName.Focus, 15);
			SkillBonuses.SetValues( 1, SkillName.MagicResist, 15);
			Attributes.BonusStr = 10;
			Attributes.WeaponDamage = 30;
			ArtifactLevel = 2;
			Server.Misc.Arty.ArtySetup( this, "" );
		}

		public Artifact_CaelansVisage( Serial serial ) : base( serial )
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