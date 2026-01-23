using System;
using Server;

namespace Server.Items
{
	public class Artifact_LolthsCaressingChoker : GiftGoldBeadNecklace
	{
		[Constructable]
		public Artifact_LolthsCaressingChoker()
		{
			Name = "Lolth's Caressing Choker";
			Hue = 2346;
			SkillBonuses.SetValues( 0, SkillName.MagicResist, 10);
			SkillBonuses.SetValues( 1, SkillName.Poisoning, 10);
			Attributes.CastRecovery = 2;
			Attributes.CastSpeed = 2;
			Attributes.WeaponDamage = 20;
			Resistances.Poison = 25;
			Attributes.RegenStam = 5;
			Attributes.RegenMana = 5;
			ArtifactLevel = 2;
			Server.Misc.Arty.ArtySetup( this, "" );
		}

		public Artifact_LolthsCaressingChoker( Serial serial ) : base( serial )
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

			if ( Hue == 0x12B )
				Hue = 0x554;
		}
	}
}