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
	public class Artifact_DemonwebTyrant : GiftClaymore
	{
		private DateTime m_NextArtifactAttackAllowed;

		public override int InitMinHits{ get{ return 80; } }
		public override int InitMaxHits{ get{ return 160; } }

		[Constructable]
		public Artifact_DemonwebTyrant()
		{
			Name = "Tyrant of the Demonweb";
			Hue = 0x922;
			WeaponAttributes.HitPoisonArea = 50;
			Attributes.SpellChanneling = 1;
			Attributes.BonusStr = 15;
			Attributes.AttackChance = 15;
			ArtifactLevel = 2;
			MinDamage = MinDamage + 12;
			MaxDamage = MaxDamage + 12;
			Server.Misc.Arty.ArtySetup( this, "Carrega o beijo malicioso de Lolth" );
			m_NextArtifactAttackAllowed = DateTime.MinValue;
		}

		public override void GetDamageTypes( Mobile wielder, out int phys, out int fire, out int cold, out int pois, out int nrgy, out int chaos, out int direct )
		{
			phys = 0;
			cold = 0;
			fire = 0;
			nrgy = 0;
			pois = 100;
			chaos = 0;
			direct = 0;
		}

		public override bool OnEquip( Mobile from )
		{
			if ( from.Karma > -15000 )
			{
				from.SendMessage( 0x922, "A corrupção de Lolth te abate!" );

				from.Hits = 1;
				from.Mana = 1;
				from.Stam = 1;

				from.FixedParticles( 0x3709, 10, 30, 5052, 0x922, 0, EffectLayer.Waist );
				from.BoltEffect( 0 );
			}

			return base.OnEquip( from );
		}

		public override void OnHit( Mobile attacker, Mobile defender, double damageBonus )
		{
			base.OnHit( attacker, defender, damageBonus );

			if (attacker == null || defender == null || attacker.Deleted || defender.Deleted || !attacker.Alive || !defender.Alive || attacker.Map == null || defender.Map == null)
				return;

			if ( attacker.Karma > -15000 )
			{
				attacker.SendMessage( 0x922, "O beijo de Lolth queima sua alma!" );

				attacker.Hits -= GetLossAmount( attacker.HitsMax, 5 );
				attacker.Mana -= GetLossAmount( attacker.ManaMax, 5 );
				attacker.Stam -= GetLossAmount( attacker.StamMax, 5 );

				if ( attacker.Hits < 1 )
					attacker.Hits = 1;

				if ( attacker.Mana < 1 )
					attacker.Mana = 1;

				if ( attacker.Stam < 1 )
					attacker.Stam = 1;

				attacker.FixedParticles( 0x3709, 10, 30, 5052, 0x922, 0, EffectLayer.Waist );
				attacker.BoltEffect( 0 );

				return;
			}

			if ( DateTime.UtcNow < m_NextArtifactAttackAllowed )
				return;

			if ( !attacker.CanBeHarmful( defender, false ) )
				return;

			double chance = GetProcChance( attacker );

			if ( Utility.RandomDouble() > chance )
				return;

			m_NextArtifactAttackAllowed = DateTime.UtcNow + TimeSpan.FromSeconds( 60.0 );

			int hitLoss = GetPositiveKarmaLoss( defender.Karma );
			int stamLoss = GetNegativeKarmaLoss( defender.Karma );

			bool fired = false;

			if ( defender.Karma >= 0 && hitLoss > 0 )
			{
				int amount = GetLossAmount( defender.HitsMax, hitLoss );

				defender.Hits -= amount;

				if ( defender.Hits < 1 )
					defender.Hits = 1;

				fired = true;
			}

			if ( defender.Karma < 0 && stamLoss > 0 )
			{
				int amount = GetLossAmount( defender.StamMax, stamLoss );

				defender.Stam -= amount;

				if ( defender.Stam < 0 )
					defender.Stam = 0;

				fired = true;
			}

			if ( fired )
			{
				attacker.SendMessage( 0x922, "O toque de Lolth incapacita seu inimigo!" );

				defender.FixedParticles( 0x3709, 10, 30, 5052, 0x922, 0, EffectLayer.Waist );
				defender.BoltEffect( 0 );
			}
		}

		private double GetProcChance( Mobile from )
		{
			int cappedLuck = from.Luck;

			if ( cappedLuck > 2000 )
				cappedLuck = 2000;

			double luckChance = cappedLuck * 0.0000125; // 0.025 / 2000

			double swords = from.Skills[SkillName.Swords].Base;

			if ( swords > 125.0 )
				swords = 125.0;

			double swordsChance = swords * 0.0002; // 0.025 / 125

			return luckChance + swordsChance;
		}

		private int GetPositiveKarmaLoss( int karma )
		{
			if ( karma >= 15000 )
				return 10;

			if ( karma >= 12500 )
				return 8;

			if ( karma >= 10000 )
				return 6;

			if ( karma >= 5000 )
				return 4;

			if ( karma >= 1000 )
				return 2;

			if ( karma >= 0 )
				return 1;

			return 0;
		}

		private int GetNegativeKarmaLoss( int karma )
		{
			if ( karma <= -15000 )
				return 10;

			if ( karma <= -12500 )
				return 9;

			if ( karma <= -10000 )
				return 7;

			if ( karma <= -5000 )
				return 5;

			if ( karma <= -1000 )
				return 4;

			if ( karma < 0 )
				return 2;

			return 0;
		}

		private int GetLossAmount( int max, int percent )
		{
			int amount = max * percent / 100;

			if ( amount < 1 )
				amount = 1;

			return amount;
		}

		public Artifact_DemonwebTyrant( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 1 );
			writer.Write( m_NextArtifactAttackAllowed );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			ArtifactLevel = 2;

			int version = reader.ReadEncodedInt();

			if ( version >= 1 )
				m_NextArtifactAttackAllowed = reader.ReadDateTime();
			else
				m_NextArtifactAttackAllowed = DateTime.MinValue;
		}
	}
}