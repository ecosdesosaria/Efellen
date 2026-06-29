using System;
using Server;
using Server.Mobiles;

namespace Server.Items
{
	public class Artifact_SymbolOfMyrkul : GiftTalismanLeather
	{
		[Constructable]
		public Artifact_SymbolOfMyrkul()
		{
			Name = "Símbolo Profano de Myrkul";
			ItemID = 0x2C95;
			Hue = 0x0455;
			SkillBonuses.SetValues( 0, SkillName.Forensics, 20.0 );
			SkillBonuses.SetValues( 1, SkillName.Necromancy, 20.0 );
			Attributes.LowerManaCost = 10;
			Attributes.BonusMana = 10;
			Attributes.BonusInt = 10;
			ArtifactLevel = 2;
			Server.Misc.Arty.ArtySetup( this, "" );
		}

		public Artifact_SymbolOfMyrkul( Serial serial ) :  base( serial )
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