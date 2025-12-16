using System;
using Server.Network;
using Server.Items;
using Server.Targeting;

namespace Server.Items
{
	public class Artifact_CircletOfTheDreamweaver : GiftLeatherCap
	{
		public override int InitMinHits{ get{ return 80; } }
		public override int InitMaxHits{ get{ return 160; } }
      
      [Constructable]
		public Artifact_CircletOfTheDreamweaver()
		{
			ItemID = 0x672E;
			Resource = CraftResource.None;
			Name = "Circlet Of The Dreamweaver";
			Hue = 0x96;
			SkillBonuses.SetValues( 0, SkillName.Psychology, 10);
			SkillBonuses.SetValues( 1, SkillName.Meditation, 15);
			SkillBonuses.SetValues( 2, SkillName.Provocation, 15);
			ArmorAttributes.MageArmor = 1;
			Attributes.BonusMana = 10;
			Attributes.LowerManaCost = 10;
			Attributes.LowerRegCost = 10;
			Attributes.BonusInt = 5;
			Attributes.SpellDamage = 25;
			FireBonus = 8;
			PoisonBonus = 4;
			ColdBonus = 7;
			EnergyBonus = 8;
			ArtifactLevel = 2;
			Server.Misc.Arty.ArtySetup( this, "" );
		}

		public Artifact_CircletOfTheDreamweaver( Serial serial ) : base( serial )
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
