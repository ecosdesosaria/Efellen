using System;
using Server;

namespace Server.Items
{
	public class Artifact_CinderForgedBreastplate : GiftDragonChest
	{
		public override int InitMinHits{ get{ return 80; } }
		public override int InitMaxHits{ get{ return 160; } }
		public override int BasePhysicalResistance{ get{ return 10; } }
		public override int BaseFireResistance{ get{ return 10; } }
		public override int BaseColdResistance{ get{ return 4; } }
		public override int BasePoisonResistance{ get{ return 9; } }
		public override int BaseEnergyResistance{ get{ return 9; } }
		[Constructable]
		public Artifact_CinderForgedBreastplate()
		{
			Name = "Cinder Forged Breastplate";
			Hue = 0x81b;
			SkillBonuses.SetValues( 0, SkillName.MagicResist, 15 );
			SkillBonuses.SetValues( 1, SkillName.Inscribe, 15 );
            SkillBonuses.SetValues( 2, SkillName.Blacksmith, 15 );
			ArmorAttributes.MageArmor = 1;
            Attributes.BonusInt = 10;
			Attributes.SpellDamage = 10;
			Attributes.ReflectPhysical = 25;
			Attributes.BonusMana = 10;
			Attributes.RegenMana = 10;
			Attributes.DefendChance = 10;
			ArtifactLevel = 2;
			Server.Misc.Arty.ArtySetup( this, "" );
		}

		public Artifact_CinderForgedBreastplate( Serial serial ) : base( serial )
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