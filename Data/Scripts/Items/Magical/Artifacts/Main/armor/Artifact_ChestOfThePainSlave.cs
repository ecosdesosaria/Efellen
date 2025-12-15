using System;
using Server;

namespace Server.Items
{
    public class Artifact_ChestOfThePainSlave : GiftStuddedChest
	{
		public override int InitMinHits{ get{ return 80; } }
		public override int InitMaxHits{ get{ return 160; } }

        public override int BasePhysicalResistance{ get{ return 3; } }
        public override int BaseColdResistance{ get{ return 12; } }
        public override int BaseFireResistance{ get{ return 12; } }
        public override int BaseEnergyResistance{ get{ return 13; } }
        public override int BasePoisonResistance{ get{ return 18; } }

        [Constructable]
        public Artifact_ChestOfThePainSlave()
        {
            Name = "Chest of the Pain Slave";
            Hue = 0xb73;
			SkillBonuses.SetValues( 0, SkillName.Bludgeoning, 10);
			SkillBonuses.SetValues( 1, SkillName.Anatomy, 10);
			ArmorAttributes.MageArmor = 1;
            Attributes.BonusStr = 5;
            Attributes.BonusInt = 5;
            Attributes.BonusDex = 5;
            Attributes.Luck = 100;
			ArtifactLevel = 2;
			Server.Misc.Arty.ArtySetup( this, "" );
		}

        public Artifact_ChestOfThePainSlave(Serial serial) : base( serial )
        {
        }

        public override void Serialize( GenericWriter writer )
        {
            base.Serialize( writer );
            writer.Write( (int) 0 );
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize( reader );
			ArtifactLevel = 2;
            int version = reader.ReadInt();
        }
    }
}
