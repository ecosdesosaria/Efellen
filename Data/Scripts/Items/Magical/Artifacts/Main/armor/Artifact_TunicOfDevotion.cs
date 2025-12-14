using System;
using Server;

namespace Server.Items
{
	public class Artifact_TunicOfDevotion : GiftChainChest
	{
		public override int InitMinHits{ get{ return 80; } }
		public override int InitMaxHits{ get{ return 160; } }

		public override int BasePhysicalResistance{ get{ return 15; } }
		public override int BaseFireResistance{ get{ return 8; } }
		public override int BaseColdResistance{ get{ return 9; } }
		public override int BasePoisonResistance{ get{ return 8; } }
		public override int BaseEnergyResistance{ get{ return 9; } }

		[Constructable]
		public Artifact_TunicOfDevotion()
		{
			Name = "Tunic of Devotion";
			ItemID = 0x13BF;
			Hue = 0x9C2;
			SkillBonuses.SetValues( 0, SkillName.Healing, 10);
			SkillBonuses.SetValues( 1, SkillName.Spiritualism, 10);
			ArmorAttributes.MageArmor = 1;
			ArmorAttributes.SelfRepair = 5;
			Attributes.ReflectPhysical = 25;
			Attributes.BonusHits = 10;
			Attributes.BonusStam = 10;
			Attributes.Luck = 50;
			ArtifactLevel = 2;
			Server.Misc.Arty.ArtySetup( this, "" );
		}

		public Artifact_TunicOfDevotion( Serial serial ) : base( serial )
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