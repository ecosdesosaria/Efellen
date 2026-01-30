using System;
using Server;

namespace Server.Items
{
	public class Artifact_StaffOfTheDreadHost : GiftBlackStaff
	{
		public override int InitMinHits{ get{ return 80; } }
		public override int InitMaxHits{ get{ return 160; } }

		[Constructable]
		public Artifact_StaffOfTheDreadHost()
		{
			Name = "Staff of the Dread Host";
			ItemID = 0x2AAC;
			Hue = 1109;
			SkillBonuses.SetValues( 0, SkillName.Necromancy, 15);
			SkillBonuses.SetValues( 1, SkillName.MagicResist, 15);
			MinDamage = MinDamage + 2;
			MaxDamage = MaxDamage + 2;
			Attributes.SpellChanneling = 1;
			Attributes.SpellDamage = 60;
			ArtifactLevel = 2;
			Server.Misc.Arty.ArtySetup( this, "" );
		}

		public override void GetDamageTypes( Mobile wielder, out int phys, out int fire, out int cold, out int pois, out int nrgy, out int chaos, out int direct )
		{
			phys = fire = cold = pois = chaos = direct = 0;
			nrgy = 100;
		}

		public Artifact_StaffOfTheDreadHost( Serial serial ) : base( serial )
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