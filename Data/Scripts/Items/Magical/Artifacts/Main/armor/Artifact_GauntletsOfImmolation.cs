using System;
using Server;

namespace Server.Items
{
	public class Artifact_GauntletsOfImmolation : GiftRingmailGloves
	{
		public override int InitMinHits{ get{ return 80; } }
		public override int InitMaxHits{ get{ return 160; } }

		public override int BasePhysicalResistance{ get{ return 8; } }
        public override int BaseFireResistance{ get{ return 18; } }
		public override int BasePoisonResistance{ get{ return 6; } }

		[Constructable]
		public Artifact_GauntletsOfImmolation()
		{
			Name = "Gauntlets of Immolation";
			Hue = 348;
			SkillBonuses.SetValues( 0, SkillName.Alchemy, 10);
			SkillBonuses.SetValues( 1, SkillName.MagicResist, 10);
			ArmorAttributes.MageArmor = 1;
			ArmorAttributes.SelfRepair = 10;
			Attributes.EnhancePotions = 15;
			Attributes.Luck = 50;
            Attributes.SpellDamage = 20;
			ArtifactLevel = 2;
			Server.Misc.Arty.ArtySetup( this, "" );
		}

		public Artifact_GauntletsOfImmolation( Serial serial ) : base( serial )
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