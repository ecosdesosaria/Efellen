using System;
using Server;

namespace Server.Items
{
	public class Artifact_EmbraceOfTheDreadHost : GiftCloak
	{
		public override int InitMinHits{ get{ return 150; } }
		public override int InitMaxHits{ get{ return 150; } }

		public override bool CanFortify{ get{ return false; } }

		[Constructable]
		public Artifact_EmbraceOfTheDreadHost() : base( 0x2684 )
		{
			Name = "Embrace of the Dread Host";
			Hue = 1109;
			SkillBonuses.SetValues( 0, SkillName.Necromancy, 15);
			SkillBonuses.SetValues( 1, SkillName.MagicResist, 15);
			Attributes.SpellDamage = 25;
			Attributes.CastRecovery = 1;
			Attributes.CastSpeed = 1;
			Attributes.LowerManaCost = 15;
			ArtifactLevel = 2;
			Server.Misc.Arty.ArtySetup( this, "" );
		}

		public Artifact_EmbraceOfTheDreadHost( Serial serial ) : base( serial )
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
