using System;
using Server;
using Server.Mobiles;

namespace Server.Items
{
    public class Artifact_ShacklesOfBhaal : GiftTalismanLeather
	{
		[Constructable]
		public Artifact_ShacklesOfBhaal()
		{
			Name = "Shackles Of Bhaal";
			ItemID = 0x4D10;
			Hue = 1200;
			Resistances.Fire = 10;
			SkillBonuses.SetValues(0, SkillName.Tactics, 15);
			SkillBonuses.SetValues(1, SkillName.Anatomy, 15);
			Attributes.WeaponDamage = 20;
			Attributes.WeaponSpeed = 10;
			ArtifactLevel = 2;
			Server.Misc.Arty.ArtySetup( this, "" );
		}
		
		public Artifact_ShacklesOfBhaal( Serial serial ) :  base( serial )
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