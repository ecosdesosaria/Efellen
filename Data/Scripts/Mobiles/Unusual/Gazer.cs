using System;
using Server;
using Server.Items;
using Server.Custom.BeholderSpecials;

namespace Server.Mobiles
{
	[CorpseName( "a Gauth corpse" )]
	public class Gazer : BaseCreature
	{
		private DateTime m_NextSpecialAttack;
		[Constructable]
		public Gazer () : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a Gauth";
			Body = 22;
			BaseSoundID = 377;

			SetStr( 96, 125 );
			SetDex( 86, 105 );
			SetInt( 141, 165 );

			SetHits( 128, 185 );

			SetDamage( 9, 14 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 35, 40 );
			SetResistance( ResistanceType.Fire, 40, 40 );
			SetResistance( ResistanceType.Cold, 20, 30 );
			SetResistance( ResistanceType.Poison, 10, 20 );
			SetResistance( ResistanceType.Energy, 20, 40 );

			SetSkill( SkillName.Psychology, 50.1, 65.0 );
			SetSkill( SkillName.Magery, 50.1, 75.0 );
			SetSkill( SkillName.MagicResist, 60.1, 85.0 );
			SetSkill( SkillName.Tactics, 50.1, 80.0 );
			SetSkill( SkillName.FistFighting, 50.1, 70.0 );

			Fame = 5500;
			Karma = -5500;

			VirtualArmor = 36;

			PackItem( new Nightshade( 4 ) );
			m_NextSpecialAttack = DateTime.Now;
		}

		public override void OnDamage( int amount, Mobile from, bool willKill )
		{
			base.OnDamage( amount, from, willKill );

			if ( DateTime.Now >= m_NextSpecialAttack && from != null && from.Alive && !willKill )
			{
				if ( Utility.RandomDouble() < 0.30 )
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
				if ( Utility.RandomDouble() < 0.10 )
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
					if ( BeholderSpecials.AntiMagicEye( this, 60, 45, target ) )
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
					if ( BeholderSpecials.TelekineticRay( this, 6, 40 ) )
					{
						this.Say( "*A wave of Telekinetic energy oozes from an eyestalk!*", target.Name );
					}
					break;
				}
			}
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Average );
			AddLoot( LootPack.LowPotions );
		}

		public override int TreasureMapLevel{ get{ return 1; } }
		public override int Meat{ get{ return 1; } }

		public Gazer( Serial serial ) : base( serial )
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