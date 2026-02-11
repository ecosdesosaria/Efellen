using System;
using Server;

namespace Server.Items
{
	public class Artifact_ProtectoroftheWildsGorget : GiftWoodenPlateGorget
	{
		public override int InitMinHits{ get{ return 80; } }
		public override int InitMaxHits{ get{ return 160; } }

		public override int BasePhysicalResistance{ get{ return 15; } }
		public override int BaseFireResistance{ get{ return 3; } }
		public override int BaseColdResistance{ get{ return 7; } }
		public override int BasePoisonResistance{ get{ return 9; } }
		public override int BaseEnergyResistance{ get{ return 9; } }

		[Constructable]
		public Artifact_ProtectoroftheWildsGorget()
		{
			Name = "Gorget of the wilds";
			Hue = 0x21F;
			ArtifactLevel = 2;
			SkillBonuses.SetValues( 0, SkillName.Tactics, 15);
			SkillBonuses.SetValues( 1, SkillName.Tracking, 15);
			Attributes.DefendChance = 10;
			Attributes.WeaponSpeed = 10;
			Attributes.WeaponDamage = 10;
			Server.Misc.Arty.ArtySetup( this, "" );
		}

		public Artifact_ProtectoroftheWildsGorget( Serial serial ) : base( serial )
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