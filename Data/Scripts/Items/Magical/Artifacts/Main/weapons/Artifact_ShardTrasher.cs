using System;
using Server.Network;
using Server.Items;

namespace Server.Items
{
	public class Artifact_ShardThrasher : GiftDiamondMace
	{
		[Constructable]
		public Artifact_ShardThrasher()
		{
			Hue = 0x4F2;
			Name = "Shard Thrasher";
			ItemID = 0x2D24;

			WeaponAttributes.HitPhysicalArea = 30;
			Attributes.BonusStam = 8;
			Attributes.AttackChance = 10;
			Attributes.WeaponSpeed = 12;
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

		public Artifact_ShardThrasher( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			ArtifactLevel = 2;

			int version = reader.ReadEncodedInt();
		}
	}
}