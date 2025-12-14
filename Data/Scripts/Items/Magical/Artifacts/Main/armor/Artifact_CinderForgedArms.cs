using System;
using Server;

namespace Server.Items
{
	public class Artifact_CinderForgedArms : GiftDragonArms
	{
		public override int InitMinHits{ get{ return 80; } }
		public override int InitMaxHits{ get{ return 160; } }
		public override int BasePhysicalResistance{ get{ return 9; } }
		public override int BaseFireResistance{ get{ return 10; } }
		public override int BaseColdResistance{ get{ return 4; } }
		public override int BasePoisonResistance{ get{ return 6; } }
		public override int BaseEnergyResistance{ get{ return 8; } }
		[Constructable]
		public Artifact_CinderForgedArms()
		{
			Name = "Cinder Forged Arms";
			Hue = 0x845;
			SkillBonuses.SetValues( 0, SkillName.MagicResist, 15 );
			SkillBonuses.SetValues( 1, SkillName.Inscribe, 15 );
            SkillBonuses.SetValues( 2, SkillName.Blacksmith, 15 );
            ArmorAttributes.MageArmor = 1;
			Attributes.SpellDamage = 25;
			Attributes.AttackChance = 10;
			Attributes.LowerManaCost = 10;
			Attributes.LowerRegCost = 25;
			ArtifactLevel = 2;
			Server.Misc.Arty.ArtySetup( this, "" );
		}

		public Artifact_CinderForgedArms( Serial serial ) : base( serial )
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