using System;
using Server;

namespace Server.Items
{
	public class Artifact_CrownOfTheNorth : GiftBandana
	{
		public override int InitMinHits{ get{ return 80; } }
		public override int InitMaxHits{ get{ return 160; } }
		public override int BaseColdResistance{ get{ return 20; } }

		public override bool CanFortify{ get{ return false; } }

		[Constructable]
		public Artifact_CrownOfTheNorth()
		{
			Name = "Crown of The North";
			Hue = 1164;
			SkillBonuses.SetValues( 1, SkillName.Tactics, 10);
			SkillBonuses.SetValues( 2, SkillName.Swords, 10);
			Attributes.BonusStr = 10;
			Attributes.RegenHits = 10;
			Attributes.WeaponDamage = 20;
			ArtifactLevel = 2;
			Server.Misc.Arty.ArtySetup( this, "" );
		}

		public Artifact_CrownOfTheNorth( Serial serial ) : base( serial )
		{
		}
		
		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 );
		}
		
		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize( reader );
			ArtifactLevel = 2;
			int version = reader.ReadInt();
		}
	}
}
