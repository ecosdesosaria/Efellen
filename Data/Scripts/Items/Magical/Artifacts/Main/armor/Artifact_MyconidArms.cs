using System;
using Server;

namespace Server.Items
{
	public class Artifact_MyconidArms : GiftWoodenPlateArms
	{
		public override int InitMinHits{ get{ return 80; } }
		public override int InitMaxHits{ get{ return 160; } }

		public override int BasePhysicalResistance{ get{ return 8; } }
		public override int BaseFireResistance{ get{ return 2; } }
		public override int BaseColdResistance{ get{ return 9; } }
		public override int BasePoisonResistance{ get{ return 14; } }
		public override int BaseEnergyResistance{ get{ return 8; } }

		[Constructable]
		public Artifact_MyconidArms()
		{
			Name = "Myconid Arms";
			Hue = 0x497;
			ArtifactLevel = 2;
			SkillBonuses.SetValues( 0, SkillName.Poisoning, 10);
			SkillBonuses.SetValues( 1, SkillName.Alchemy, 10);
			Attributes.DefendChance = 5;
			Attributes.CastRecovery = 2;
			Attributes.CastRecovery = 2;
			Attributes.SpellDamage = 15;
			Attributes.BonusStam = 5;
			Attributes.RegenMana = 5;
            ArmorAttributes.MageArmor = 1;
			Server.Misc.Arty.ArtySetup( this, "" );
		}

		public Artifact_MyconidArms( Serial serial ) : base( serial )
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