using System;
using Server;

namespace Server.Items
{
	public class Artifact_ExaltedArmsOfDevotion : GiftRingmailArms
	{
		public override int InitMinHits{ get{ return 80; } }
		public override int InitMaxHits{ get{ return 160; } }

		public override int BasePhysicalResistance{ get{ return 10; } }
		public override int BaseFireResistance{ get{ return 10; } }
		public override int BaseColdResistance{ get{ return 10; } }
		public override int BasePoisonResistance{ get{ return 10; } }
		public override int BaseEnergyResistance{ get{ return 10; } }

		[Constructable]
		public Artifact_ExaltedArmsOfDevotion()
		{
			Name = "Braçadeiras Exaltadas da Devoção";
			Hue = 0x0672;;
			SkillBonuses.SetValues( 0, SkillName.Healing, 15);
			SkillBonuses.SetValues( 1, SkillName.Spiritualism, 15);
			SkillBonuses.SetValues( 2, SkillName.Knightship, 15);
            ArmorAttributes.MageArmor = 1;
			Attributes.BonusStr = 10;
			Attributes.Luck = 40;
			Attributes.WeaponDamage = 10;
			ArtifactLevel = 2;
			Server.Misc.Arty.ArtySetup( this, "" );
		}

		public Artifact_ExaltedArmsOfDevotion( Serial serial ) : base( serial )
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