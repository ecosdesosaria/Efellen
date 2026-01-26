using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Engines.Craft;

namespace Server.Items
{
	public class Artifact_GrimoireOfTheDemonweb : Spellbook
	{
		[Constructable]
		public Artifact_GrimoireOfTheDemonweb() : base()
		{
			Name = "Grimoire of the Demonweb";
			Hue = 1316;
			ArtifactLevel = 1;

			switch ( Utility.Random( 6 ) ) 
			{
				case 0: this.Content = 0xFFFFFFFFFFF;		break;
				case 1: this.Content = 0xFFFFFFFFFFFF;		break;
				case 2: this.Content = 0xFFFFFFFFFFFFF;		break;
				case 3: this.Content = 0xFFFFFFFFFFFFFF;	break;
				case 4: this.Content = 0xFFFFFFFFFFFFFFF;	break;
				case 5: this.Content = 0xFFFFFFFFFFFFFFFF;	break;
			}

			int attributeCount = Utility.RandomMinMax(5,10);
			BaseRunicTool.ApplyAttributesTo( (Spellbook)this, attributeCount, 45, 55 );
			Attributes.SpellDamage = 50;
			Attributes.CastSpeed = 1;
			Attributes.CastRecovery = 1;
			SkillBonuses.SetValues( 0, SkillName.Magery, ( 20.0 ) );
			SkillBonuses.SetValues( 1, SkillName.Psychology, ( 20.0 ) );
			SkillBonuses.SetValues( 2, SkillName.Inscribe, ( 20.0 ) );
		}

		public Artifact_GrimoireOfTheDemonweb( Serial serial ) : base( serial )
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
			int version = reader.ReadInt();
			ArtifactLevel = 1;
		}
		
	}
}

