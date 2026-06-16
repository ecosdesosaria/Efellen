using System;
using Server;

namespace Server.Items
{
	public class Artifact_BoneCrusher : GiftWarMace
	{
		public override int InitMinHits{ get{ return 80; } }
		public override int InitMaxHits{ get{ return 160; } }

		[Constructable]
		public Artifact_BoneCrusher()
		{
			Name = "Bone Crusher";
			ItemID = 0x1406;
			Hue = 0x60C;
			WeaponAttributes.HitLowerDefend = 50;
			Attributes.BonusStr = 10;
			Attributes.WeaponDamage = 10;
			ArtifactLevel = 2;
			Server.Misc.Arty.ArtySetup( this, "Powerful criticals" );
		}

		public override void OnHit(Mobile attacker, Mobile defender, double damageBonus)
		{
			base.OnHit(attacker, defender, damageBonus);
			if (attacker == null || defender == null || attacker.Map == null || defender.Map == null || defender.Deleted || attacker.Deleted)
		        return;

		    if (Utility.RandomDouble() < 0.15)
		    {
		        damageBonus += 0.35;
		        attacker.SendMessage("Seu golpe perfura seu inimigo!");
		        attacker.PlaySound(0x20F);
		    }
		    
		}

		public Artifact_BoneCrusher( Serial serial ) : base( serial )
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