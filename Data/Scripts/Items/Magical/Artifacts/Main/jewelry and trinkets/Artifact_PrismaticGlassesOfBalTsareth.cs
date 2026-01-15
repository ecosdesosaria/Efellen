using System;
using Server.Network;
using Server.Items;
using Server.Targeting;

namespace Server.Items
{
	public class Artifact_PrismaticGlassesOfBalTsareth : ElvenGlasses
	{
		public override int InitMinHits{ get{ return 80; } }
		public override int InitMaxHits{ get{ return 160; } }
      
      [Constructable]
		public Artifact_PrismaticGlassesOfBalTsareth()
		{
			ItemID = 0x672E;
			Name = "Glasses of Bal Tsareth";
			Hue = 0x0213;
			SkillBonuses.SetValues( 0, SkillName.Psychology, 15);
			SkillBonuses.SetValues( 1, SkillName.Inscribe, 15);
			Attributes.BonusMana = 10;
			Attributes.LowerManaCost = 10;
			Attributes.LowerRegCost = 10;
			Attributes.BonusInt = 10;
			Attributes.SpellDamage = 20;
			PhysicalBonus = 10;
			FireBonus = 10;
			PoisonBonus = 10;
			ColdBonus = 10;
			EnergyBonus = 10;
			ArtifactLevel = 2;
			Server.Misc.Arty.ArtySetup( this, "" );
		}

		public Artifact_PrismaticGlassesOfBalTsareth( Serial serial ) : base( serial )
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
			ItemID = 0x672E;
		}
	}
}
