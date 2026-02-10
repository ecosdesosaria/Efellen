using System;
using Server.Network;
using Server.Items;
using Server.Targeting;
using Server.Mobiles;
using Server.Engines.PartySystem;
using Server.Guilds;
using Server.EffectsUtil;

namespace Server.Items
{
	public class Artifact_ChainBreaker : GiftTetsubo
	{
		private DateTime m_NextArtifactAttackAllowed;
		
		public override int InitMinHits{ get{ return 80; } }
		public override int InitMaxHits{ get{ return 160; } }

		[Constructable]
		public Artifact_ChainBreaker()
		{
			Name = "Chain Breaker";
			Hue = 0x81b;
			WeaponAttributes.HitLowerDefend = 25;
			WeaponAttributes.HitLowerAttack = 25;
			Attributes.AttackChance = 10;
			ArtifactLevel = 2;
			MinDamage = MinDamage + 2;
			MaxDamage = MaxDamage + 2;
			Server.Misc.Arty.ArtySetup( this, "Knocks foes back" );
			m_NextArtifactAttackAllowed = DateTime.MinValue;
		}

		public override void OnHit(Mobile attacker, Mobile defender, double damageBonus)
		{
		    base.OnHit(attacker, defender, damageBonus);
		    if (attacker == null || defender == null)
		        return;
		    if (attacker.Skills[SkillName.Bludgeoning].Value <= 105.0 || attacker.Str <= 91)
		        return;
		    if (DateTime.UtcNow < m_NextArtifactAttackAllowed)
		        return;
		    double skill = attacker.Skills[SkillName.Bludgeoning].Value;
		    double chance = 0.05 + (skill / 125.0) * 0.20;
		    if (Utility.RandomDouble() > chance)
		        return;
		    double seconds = 120.0 - (skill * (90.0 / 125.0));
		    m_NextArtifactAttackAllowed = DateTime.UtcNow + TimeSpan.FromSeconds(seconds);

		    Point3D attackerLoc = attacker.Location;
		    Point3D defenderLoc = defender.Location;

		    int xDiff = defenderLoc.X - attackerLoc.X;
		    int yDiff = defenderLoc.Y - attackerLoc.Y;

		    double distance = Math.Sqrt(xDiff * xDiff + yDiff * yDiff);
		    if (distance == 0)
		        distance = 1;

		    double xDir = xDiff / distance;
		    double yDir = yDiff / distance;

		    int knockbackDistance = Utility.RandomMinMax(3, 6);

		    Point3D newLocation = Point3D.Zero;
		    bool foundValidLocation = false;

		    for (int i = knockbackDistance; i >= 1; i--)
		    {
		        int newX = defenderLoc.X + (int)Math.Round(xDir * i);
		        int newY = defenderLoc.Y + (int)Math.Round(yDir * i);
		        int newZ = defender.Map.GetAverageZ(newX, newY);

		        newLocation = new Point3D(newX, newY, newZ);

		        if (defender.Map.CanSpawnMobile(newLocation))
		        {
		            foundValidLocation = true;
		            break;
		        }
		    }

		    if (foundValidLocation)
		    {
		        defender.MoveToWorld(newLocation, defender.Map);
		        defender.ProcessDelta();

		        Effects.SendLocationParticles(
		            EffectItem.Create(defenderLoc, defender.Map, EffectItem.DefaultDuration),
		            0x3728, 10, 10, 2023
		        );
		    }
		}

		public Artifact_ChainBreaker( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.WriteEncodedInt( 1 );
			writer.Write(m_NextArtifactAttackAllowed);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			
			ArtifactLevel = 2;
			
			int version = reader.ReadEncodedInt();
			
			if (version >= 1)
				m_NextArtifactAttackAllowed = reader.ReadDateTime();
			else
				m_NextArtifactAttackAllowed = DateTime.MinValue;
		}
	}
}