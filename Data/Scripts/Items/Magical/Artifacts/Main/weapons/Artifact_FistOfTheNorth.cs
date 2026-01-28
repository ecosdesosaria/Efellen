using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Engines.PartySystem;
using Server.EffectsUtil;

namespace Server.Items
{
	public class Artifact_FistOfTheNorth : GiftExecutionersAxe
	{
		[Constructable]
		public Artifact_FistOfTheNorth()
		{
			Name = "Fist of the North";
			Hue = 1164;
			Slayer = SlayerName.FlameDousing;
			Attributes.RegenStam = 10;
			Attributes.RegenHits = 10;
            Attributes.WeaponDamage = 10;
			WeaponAttributes.HitLeechHits = 20;
			WeaponAttributes.HitLeechStam = 20;
			WeaponAttributes.HitLowerDefend = 20;
			MinDamage = MinDamage + 3;
			MaxDamage = MaxDamage + 3;
			ArtifactLevel = 2;
			Server.Misc.Arty.ArtySetup( this, "" );
		}

		public Artifact_FistOfTheNorth(Serial serial) : base(serial)
		{
		}

		 public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 1 ); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadEncodedInt();
			ArtifactLevel = 2;
		}
	}
}