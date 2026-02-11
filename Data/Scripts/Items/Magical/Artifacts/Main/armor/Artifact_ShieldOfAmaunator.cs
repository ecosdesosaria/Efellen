using System;
using Server;

namespace Server.Items
{
	public class Artifact_ShieldOfAmaunator : GiftSunShield
	{
		public override int BasePhysicalResistance{ get{ return 5; } }
		public override int BaseFireResistance{ get{ return 15; } }
		public override int BaseColdResistance{ get{ return 0; } }
		public override int BasePoisonResistance{ get{ return 0; } }
		public override int BaseEnergyResistance{ get{ return 0; } }

		public override int InitMinHits{ get{ return 200; } }
		public override int InitMaxHits{ get{ return 200; } }

		[Constructable]
		public Artifact_ShieldOfAmaunator()
		{
			Hue = 2781;
			Name = "Shield of Amaunator";
			SkillBonuses.SetValues( 0, SkillName.Parry, 10);
			SkillBonuses.SetValues( 1, SkillName.Spiritualism, 10);
			Attributes.SpellChanneling = 1;
			Attributes.ReflectPhysical = 25;
			Attributes.DefendChance = 15;
			ArtifactLevel = 2;
			Server.Misc.Arty.ArtySetup( this, "" );
		}

		public Artifact_ShieldOfAmaunator( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			ArtifactLevel = 2;
			int version = reader.ReadInt();
		}
	}
}