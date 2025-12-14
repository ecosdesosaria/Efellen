using System;
using Server;

namespace Server.Items
{
	public class Artifact_GauntletsOfDevotion : GiftRingmailGloves
	{
		public override int InitMinHits{ get{ return 80; } }
		public override int InitMaxHits{ get{ return 160; } }

		public override int BasePhysicalResistance{ get{ return 18; } }
		public override int BasePoisonResistance{ get{ return 20; } }

		[Constructable]
		public Artifact_GauntletsOfDevotion()
		{
			Name = "Gauntlets of Devotion";
			Hue = 0x9C2;;
			SkillBonuses.SetValues( 0, SkillName.Healing, 10);
			SkillBonuses.SetValues( 1, SkillName.Spiritualism, 10);
			ArmorAttributes.MageArmor = 1;
			Attributes.EnhancePotions = 25;
			Attributes.Luck = 100;
			ArtifactLevel = 2;
			Server.Misc.Arty.ArtySetup( this, "" );
		}

		public Artifact_GauntletsOfDevotion( Serial serial ) : base( serial )
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