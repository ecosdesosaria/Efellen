using System;
using Server;

namespace Server.Items
{
	public class Artifact_GlovesOfThePugilist : GiftPugilistGloves
	{
		public override int InitMinHits{ get{ return 80; } }
		public override int InitMaxHits{ get{ return 160; } }

		[Constructable]
		public Artifact_GlovesOfThePugilist()
		{
			Name = "gloves of the pugilist";
			Weight = 2.0;
			Hue = 0x6D1;
			SkillBonuses.SetValues( 0, SkillName.FistFighting, 15.0 );
			Attributes.BonusDex = 10;
			Attributes.WeaponDamage = 20;
			WeaponAttributes.ResistPhysicalBonus = 20;
			WeaponAttributes.ResistColdBonus = 5;
			WeaponAttributes.ResistEnergyBonus = 5;
			WeaponAttributes.ResistFireBonus = 5;
			WeaponAttributes.ResistPoisonBonus = 5;
			MinDamage = MinDamage + 2;
			MaxDamage = MaxDamage + 2;
			ArtifactLevel = 2;
			Server.Misc.Arty.ArtySetup( this, "" );
		}

		public Artifact_GlovesOfThePugilist( Serial serial ) : base( serial )
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
		}
	}
}