using System;
using Server;
using Server.Items;
using Server.Custom.BeholderSpecials;

namespace Server.Mobiles
{
	[CorpseName( "an elder gazer corpse" )]
	public class ElderGazer : BaseCreature
	{
		private DateTime m_NextSpecialAttack;
		[Constructable]
		public ElderGazer () : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "an elder gazer";
			Body = 457;
			BaseSoundID = 377;

			SetStr( 336, 375 );
			SetDex( 126, 155 );
			SetInt( 331, 425 );

			SetHits( 278, 345 );

			SetDamage( 13, 23 );

			SetDamageType( ResistanceType.Physical, 50 );
			SetDamageType( ResistanceType.Energy, 50 );

			SetResistance( ResistanceType.Physical, 45, 45 );
			SetResistance( ResistanceType.Fire, 60, 65 );
			SetResistance( ResistanceType.Cold, 40, 50 );
			SetResistance( ResistanceType.Poison, 40, 50 );
			SetResistance( ResistanceType.Energy, 40, 65 );

			SetSkill( SkillName.Anatomy, 62.0, 100.0 );
			SetSkill( SkillName.Psychology, 95.1, 105.0 );
			SetSkill( SkillName.Magery, 100.1, 110.0 );
			SetSkill( SkillName.MagicResist, 115.1, 135.0 );
			SetSkill( SkillName.Tactics, 90.1, 100.0 );
			SetSkill( SkillName.FistFighting, 90.1, 100.0 );

			Fame = 12500;
			Karma = -12500;

			VirtualArmor = 50;
			m_NextSpecialAttack = DateTime.Now;
		}

		public override void OnDamage( int amount, Mobile from, bool willKill )
		{
			base.OnDamage( amount, from, willKill );

			if ( DateTime.Now >= m_NextSpecialAttack && from != null && from.Alive && !willKill )
			{
				if ( Utility.RandomDouble() < 0.40 )
				{
					TriggerSpecialAttack( from );
					m_NextSpecialAttack = DateTime.Now + TimeSpan.FromSeconds( 30 );
				}
			}
		}

		public override void OnGaveMeleeAttack( Mobile from)
		{
			base.OnGaveMeleeAttack(from);

			if ( DateTime.Now >= m_NextSpecialAttack && from != null && from.Alive )
			{
				if ( Utility.RandomDouble() < 0.20 )
				{
					TriggerSpecialAttack( from );
					m_NextSpecialAttack = DateTime.Now + TimeSpan.FromSeconds( 30 );
				}
			}
		}

		private void TriggerSpecialAttack( Mobile target )
		{
			int choice = Utility.Random( 4 );

			switch ( choice )
			{
				case 0:
				{
					if ( BeholderSpecials.AntiMagicEye( this, 40, 45, target ) )
					{
						this.Say( "*Focuses its anti-magic eye on {0}*", target.Name );
					}
					break;
				}
				case 1:
				{
					if ( BeholderSpecials.Fear( this, 90, target ) )
					{
						this.Say( "*Strikes fear into {0}*", target.Name );
					}
					break;
				}
				case 2:
				{
					if ( BeholderSpecials.Petrification( this, 30, target ) )
					{
						this.Say( "*Petrifies {0} with its gaze*", target.Name );
					}
					break;
				}
				case 3:
				{
					if ( BeholderSpecials.TelekineticRay( this, 4, 40 ) )
					{
						this.Say( "*A wave of Telekinetic energy oozes from an eyestalk!*", target.Name );
					}
					break;
				}
				case 4:
				{
					if ( BeholderSpecials.DeathRay( this, target, 14, 7, 90 ) )
					{
						this.Say( "*Fires a necrotic ray at {0}*", target.Name );
					}
					break;
				}
			}
		}

		public override int TreasureMapLevel{ get{ return Core.AOS ? 4 : 0; } }

		public override void GenerateLoot()
		{
			AddLoot( LootPack.FilthyRich );
		}

		public ElderGazer( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
}