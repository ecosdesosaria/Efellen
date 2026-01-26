using System;

namespace Server.Items
{
	public class Artifact_BookOfVileDarkness : NecromancerSpellbook
	{
		[Constructable]
		public Artifact_BookOfVileDarkness() : base()
		{
			Hue = 1316;
			Name = "Book of Vile Darkness";
			Attributes.RegenMana = 10;
			ArtifactLevel = 1;

			this.Content = (ulong)( (int)(ulong)0x1FFFF );
			int attributeCount = Utility.RandomMinMax(5,10);
			BaseRunicTool.ApplyAttributesTo( (Spellbook)this, attributeCount, 45, 65 );
			Attributes.SpellDamage = 25;
			Attributes.CastSpeed = 1;
			Attributes.CastRecovery = 1;
			SkillBonuses.SetValues( 0, SkillName.Spiritualism, ( 20.0 ) );
			SkillBonuses.SetValues( 1, SkillName.Necromancy, ( 20.0 ) );
			SkillBonuses.SetValues( 2, SkillName.Meditation, ( 20.0 ) );
		}

		public Artifact_BookOfVileDarkness( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.WriteEncodedInt( 0 ); //version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadEncodedInt();
			ArtifactLevel = 1;
		}
	}
}
