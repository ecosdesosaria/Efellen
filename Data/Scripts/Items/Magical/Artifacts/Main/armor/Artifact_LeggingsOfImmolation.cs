using System;
using Server.Network;
using Server.Items;

namespace Server.Items
{
	public class Artifact_LeggingsOfImmolation : GiftChainLegs
	{
		public override int InitMinHits{ get{ return 80; } }
		public override int InitMaxHits{ get{ return 160; } }

		public override int BasePhysicalResistance{ get{ return 12; } }
		public override int BaseFireResistance{ get{ return 18; } }
		public override int BaseColdResistance{ get{ return 7; } }
		public override int BasePoisonResistance{ get{ return 4; } }
		public override int BaseEnergyResistance{ get{ return 9; } }

		[Constructable]
		public Artifact_LeggingsOfImmolation()
		{
			Hue = 0xB51;
			ItemID = 0x13BE;
			Name = "Leggings of Immolation";
			Hue = 348;
			SkillBonuses.SetValues( 0, SkillName.Alchemy, 10);
			SkillBonuses.SetValues( 1, SkillName.MagicResist, 10);
			ArmorAttributes.MageArmor = 1;
            Attributes.BonusHits = 10;
            Attributes.SpellDamage = 10;
			Attributes.EnhancePotions = 20;
			Attributes.DefendChance = 10;
			ArtifactLevel = 2;
			Server.Misc.Arty.ArtySetup( this, "" );
		}

		public Artifact_LeggingsOfImmolation( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			ArtifactLevel = 2;

			int version = reader.ReadEncodedInt();
		}
	}
}