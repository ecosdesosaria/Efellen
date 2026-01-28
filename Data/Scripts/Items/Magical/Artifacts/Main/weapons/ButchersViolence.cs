using System;
using Server.Network;
using Server.Items;

namespace Server.Items
{
	public class ButchersViolence : GiftWarCleaver
	{
		public override int InitMinHits{ get{ return 80; } }
		public override int InitMaxHits{ get{ return 160; } }

		[Constructable]
		public ButchersViolence()
		{
			Name = "Butcher's Violence";
			ItemID = 0x2D23;
			Hue = 0x845;
            SkillBonuses.SetValues(0, SkillName.Anatomy, 20);
			Attributes.WeaponSpeed = 20;
			WeaponAttributes.HitLeechHits = 50;
			Attributes.WeaponDamage = 10;
			MinDamage = MinDamage + 1;
			MaxDamage = MaxDamage + 1;
			ArtifactLevel = 2;
			Server.Misc.Arty.ArtySetup( this, "Vicious criticals" );
		}

		public override void OnHit(Mobile attacker, Mobile defender, double damageBonus)
		{
		    if (Utility.RandomDouble() < 0.10)
		    {
		        damageBonus += 0.35;
		        attacker.SendMessage("Your strike pierces through your enemy!");
		        attacker.PlaySound(0x20F);
		    }
		    base.OnHit(attacker, defender, damageBonus);
		}

		public ButchersViolence( Serial serial ) : base( serial )
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