using System;
using Server.Network;
using Server.Items;
using Server.Targeting;
using Server.Mobiles;
using Server.Engines.PartySystem;
using Server.EffectsUtil;
using Server.Guilds;

namespace Server.Items
{
	public class Artifact_SpiritBreaker : GiftMaul
	{
		public override int InitMinHits{ get{ return 80; } }
		public override int InitMaxHits{ get{ return 160; } }

      [Constructable]
		public Artifact_SpiritBreaker()
		{
			Name = "Spirit Breaker";
			Hue = 2310;
			WeaponAttributes.HitLowerDefend = 50;
			Attributes.AttackChance = 10;
			Attributes.WeaponDamage = 10;
			ArtifactLevel = 2;
			Server.Misc.Arty.ArtySetup( this, "Devora vontade" );
		}

		public override void OnHit(Mobile attacker, Mobile defender, double damageBonus)
		{
            base.OnHit(attacker, defender, damageBonus);
		    if (attacker == null || defender == null || attacker.Map == null || defender.Map == null || defender.Deleted || attacker.Deleted)
                return;

		    if (Utility.RandomDouble() > 0.15)
		        return;

		    double weaponSkill = attacker.Skills[SkillName.Bludgeoning].Value;
		    double drainPercent = 0.05 + (weaponSkill / 125.0) * 0.10;

		    int stamDrain = (int)(defender.Mana * drainPercent);

		    if (stamDrain > 0)
		    {
		        defender.Stam -= stamDrain;
		        defender.SendMessage(0x22, "Sua stamina foi drenada!");
		    }
            
		}

		public override void GetDamageTypes( Mobile wielder, out int phys, out int fire, out int cold, out int pois, out int nrgy, out int chaos, out int direct )
		{
			fire = cold = pois = chaos = direct = 0;
			nrgy = phys = 50;
		}

		
		public Artifact_SpiritBreaker( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.WriteEncodedInt( 1 );
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			
			ArtifactLevel = 2;
			
			int version = reader.ReadEncodedInt();
			
		}
	}
}