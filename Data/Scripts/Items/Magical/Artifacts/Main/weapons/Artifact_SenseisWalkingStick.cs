using System;
using Server.Network;
using Server.Items;

namespace Server.Items
{
	public class Artifact_SenseisWalkingStick : GiftBokuto
	{
		public override int InitMinHits{ get{ return 80; } }
		public override int InitMaxHits{ get{ return 160; } }

		[Constructable]
		public Artifact_SenseisWalkingStick()
		{
			Name = "Sensei's Walking Stick";
			Hue = 0x2A;
			SkillBonuses.SetValues( 0, SkillName.Bushido, 15 );
			SkillBonuses.SetValues( 1, SkillName.Parry, 15 );
			WeaponAttributes.HitLowerAttack = 30;
			WeaponAttributes.HitLowerDefend = 30;
			MinDamage = MinDamage + 2;
			MaxDamage = MaxDamage + 2;
			ArtifactLevel = 2;
			Server.Misc.Arty.ArtySetup( this, "Powerful criticals" );
		}

		public override void OnHit(Mobile attacker, Mobile defender, double damageBonus)
		{
		    if (Utility.RandomDouble() < 0.15)
		    {
		        damageBonus += 0.35;
		        attacker.SendMessage("Seu golpe perfura seu inimigo!");
		        attacker.PlaySound(0x20F);
		    }
		    base.OnHit(attacker, defender, damageBonus);
		}

		public override void GetDamageTypes( Mobile wielder, out int phys, out int fire, out int cold, out int pois, out int nrgy, out int chaos, out int direct )
		{
			phys = 100;

			pois = fire = cold = nrgy = chaos = direct = 0;
		}

		public Artifact_SenseisWalkingStick( Serial serial ) : base( serial )
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