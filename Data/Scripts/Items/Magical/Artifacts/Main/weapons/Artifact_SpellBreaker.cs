using System;
using Server;
using Server.CustomEffects;

namespace Server.Items
{
	public class Artifact_SpellBreaker : GiftDoubleAxe
	{
		public override int InitMinHits{ get{ return 80; } }
		public override int InitMaxHits{ get{ return 160; } }

		[Constructable]
		public Artifact_SpellBreaker()
		{
			Name = "Spellbreaker";
			Hue = 2310;
			ItemID = 0xF4B;
			WeaponAttributes.HitDispel = 50;
			Attributes.AttackChance = 10;
			Attributes.WeaponDamage = 10;
			ArtifactLevel = 2;
			Server.Misc.Arty.ArtySetup( this, "Devora magia" );
		}

		public override void OnHit(Mobile attacker, Mobile defender, double damageBonus)
		{
            base.OnHit(attacker, defender, damageBonus);
		    if (attacker == null || defender == null || attacker.Map == null || defender.Map == null || defender.Deleted || attacker.Deleted)
                return;

		    if (Utility.RandomDouble() > 0.15)
		        return;

		    double swordsSkill = attacker.Skills[SkillName.Swords].Value;
		    double drainPercent = 0.05 + (swordsSkill / 125.0) * 0.10;

		    int manaDrain = (int)(defender.Mana * drainPercent);

		    if (manaDrain > 0)
		    {
		        defender.Mana -= manaDrain;
		        defender.SendMessage(0x22, "Suas reservas mágicas foram drenadas!");
		    }
            
		}

		public override void GetDamageTypes( Mobile wielder, out int phys, out int fire, out int cold, out int pois, out int nrgy, out int chaos, out int direct )
		{
			fire = cold = pois = chaos = direct = 0;
			nrgy = phys = 50;
		}

		public Artifact_SpellBreaker( Serial serial ) : base( serial )
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