using System;
using Server;

namespace Server.Items
{
	public class Artifact_RobeofMyrkul : GiftRobe
	{
		[Constructable]
		public Artifact_RobeofMyrkul()
		{
			ItemID = 0x288;
			Name = "Robe of Myrkul";
			Hue = 0x0455;
			Resistances.Poison = 20;
			Attributes.CastRecovery = 1;
			Attributes.CastSpeed = 1;
			Attributes.LowerManaCost = 10;
			Attributes.LowerRegCost = 10;
			SkillBonuses.SetValues(0, SkillName.Necromancy, 15);
			SkillBonuses.SetValues(1, SkillName.Forensics, 15);
			ArtifactLevel = 2;
			Server.Misc.Arty.ArtySetup( this, "Vestes Vis de Myrkul" );
		}

		public override bool OnEquip(Mobile from)
		{
			if (from.Karma >= 0)
			{
				from.SendMessage("As Vis Vestes de Myrkul não serão usadas por alguém como você");
				return false;
			}

			return base.OnEquip(from);
		}

		public Artifact_RobeofMyrkul( Serial serial ) : base( serial )
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