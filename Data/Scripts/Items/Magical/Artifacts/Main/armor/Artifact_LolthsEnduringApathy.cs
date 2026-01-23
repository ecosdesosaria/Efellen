using System;
using Server;

namespace Server.Items
{
	public class Artifact_LolthsEnduringApathy : GiftDarkShield
	{
		public override int BasePhysicalResistance{ get{ return 0; } }
		public override int BaseFireResistance{ get{ return 0; } }
		public override int BaseColdResistance{ get{ return 0; } }
		public override int BasePoisonResistance{ get{ return 15; } }
		public override int BaseEnergyResistance{ get{ return 0; } }

		public override int InitMinHits{ get{ return 200; } }
		public override int InitMaxHits{ get{ return 200; } }

		[Constructable]
		public Artifact_LolthsEnduringApathy()
		{
			Name = "Lolth's Enduring Apathy";
			Hue = 2346;
			SkillBonuses.SetValues( 0, SkillName.MagicResist, 10);
			SkillBonuses.SetValues( 1, SkillName.Poisoning, 10);
			Attributes.SpellChanneling = 1;
			Attributes.DefendChance = 10;
			Attributes.BonusInt = 10;
			Attributes.SpellDamage = 20;
			ArtifactLevel = 2;
			Server.Misc.Arty.ArtySetup( this, "" );
		}

		public Artifact_LolthsEnduringApathy( Serial serial ) : base( serial )
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