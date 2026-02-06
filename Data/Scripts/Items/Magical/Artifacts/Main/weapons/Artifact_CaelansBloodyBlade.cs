using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Engines.PartySystem;
using Server.EffectsUtil;
using Server.Guilds;

namespace Server.Items
{
	public class Artifact_CaelansBloodyBlade : GiftRoyalSword
	{

		[Constructable]
		public Artifact_CaelansBloodyBlade()
		{
			Name = "Caelan's Bloody Blade";
			Hue = 0x0AA5;
			Slayer = SlayerName.Repond;
			WeaponAttributes.SelfRepair = 10;
			Attributes.BonusHits = 10;
			Attributes.RegenHits = 10;
			WeaponAttributes.HitLeechHits = 30;
			WeaponAttributes.HitLowerDefend = 30;
			MinDamage = MinDamage + 3;
			MaxDamage = MaxDamage + 3;
			ArtifactLevel = 2;
			Server.Misc.Arty.ArtySetup( this, "Culls the weak" );
		}

		public override void OnHit(Mobile attacker, Mobile defender, double damageBonus)
        {
            if (attacker == null || defender == null)
                return;

            if (defender.Hits > 0 && defender.Hits < (defender.HitsMax / 10))
            {
                int extra = (int)(defender.HitsMax * 0.25);
                if (extra < 1)
                    extra = 1;
				if (extra > 100)
					extra = 100;

                defender.Damage(extra, attacker);

                attacker.FixedParticles(0x3728, 10, 10, 5052, 0, 0, EffectLayer.Head);
                attacker.PlaySound(0x1F1);
            }

            base.OnHit(attacker, defender, damageBonus);
        }

		public Artifact_CaelansBloodyBlade(Serial serial) : base(serial)
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