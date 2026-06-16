using System;
using Server;

namespace Server.Items
{
	public class Artifact_ExaltedTunicOfDevotion : GiftChainChest
	{
		public override int InitMinHits{ get{ return 80; } }
		public override int InitMaxHits{ get{ return 160; } }

		public override int BasePhysicalResistance{ get{ return 10; } }
		public override int BaseFireResistance{ get{ return 10; } }
		public override int BaseColdResistance{ get{ return 10; } }
		public override int BasePoisonResistance{ get{ return 10; } }
		public override int BaseEnergyResistance{ get{ return 10; } }

		[Constructable]
		public Artifact_ExaltedTunicOfDevotion()
		{
			Name = "Túnica Exaltada de Devoção";
			ItemID = 0x13BF;
			Hue = 0x0672;
			SkillBonuses.SetValues( 0, SkillName.Healing, 15);
			SkillBonuses.SetValues( 1, SkillName.Spiritualism, 15);
			SkillBonuses.SetValues( 2, SkillName.Knightship, 15);
			ArmorAttributes.MageArmor = 1;
			ArmorAttributes.SelfRepair = 5;
			Attributes.ReflectPhysical = 25;
			Attributes.BonusHits = 10;
			Attributes.BonusStam = 10;
			Attributes.Luck = 50;
			ArtifactLevel = 2;
			Server.Misc.Arty.ArtySetup( this, "" );
		}

		public Artifact_ExaltedTunicOfDevotion( Serial serial ) : base( serial )
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
				if ( Hue == 0x54E )
					Hue = 0x54F;

				if ( Attributes.NightSight == 0 )
					Attributes.NightSight = 1;

				PhysicalBonus = 0;
				FireBonus = 0;
			}
		}
	}
}