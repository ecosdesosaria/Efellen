using System;
using Server;

namespace Server.Items
{
	public class Artifact_TunicOfImmolation : GiftChainChest
	{
		public override int InitMinHits{ get{ return 80; } }
		public override int InitMaxHits{ get{ return 160; } }

		public override int BasePhysicalResistance{ get{ return 5; } }
		public override int BaseFireResistance{ get{ return 18; } }
		public override int BaseColdResistance{ get{ return 9; } }
		public override int BasePoisonResistance{ get{ return 8; } }
		public override int BaseEnergyResistance{ get{ return 9; } }

		[Constructable]
		public Artifact_TunicOfImmolation()
		{
			Name = "Tunic of Immolation";
			ItemID = 0x13BF;
			Hue = 348;
			SkillBonuses.SetValues( 0, SkillName.Alchemy, 10);
			SkillBonuses.SetValues( 1, SkillName.MagicResist, 10);
			ArmorAttributes.MageArmor = 1;
			ArmorAttributes.SelfRepair = 5;
			Attributes.EnhancePotions = 25;
			Attributes.SpellDamage = 10;
			Attributes.BonusMana = 10;
			Attributes.Luck = 50;
			ArtifactLevel = 2;
			Server.Misc.Arty.ArtySetup( this, "" );
		}

		public Artifact_TunicOfImmolation( Serial serial ) : base( serial )
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