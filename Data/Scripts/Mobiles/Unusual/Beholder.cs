using System;
using Server;
using Server.Items;
using Server.Misc;
using Server.Custom.BeholderSpecials;

namespace Server.Mobiles
{
	[CorpseName( "a beholder corpse" )]
	public class Beholder : BaseCreature
	{
		private DateTime m_NextSpecialAttack;
		public override int BreathPhysicalDamage{ get{ return 0; } }
		public override int BreathFireDamage{ get{ return BeholderEye( this.VirtualArmor, 1 ); } }
		public override int BreathColdDamage{ get{ return BeholderEye( this.VirtualArmor, 2 ); } }
		public override int BreathPoisonDamage{ get{ return BeholderEye( this.VirtualArmor, 3 ); } }
		public override int BreathEnergyDamage{ get{ return BeholderEye( this.VirtualArmor, 4 ); } }
		public override int BreathEffectHue{ get{ return BeholderEye( this.VirtualArmor, 5 ); } }
		public override bool ReacquireOnMovement{ get{ return !Controlled; } }
		public override bool HasBreath{ get{ return true; } }
		public override int BreathEffectSound{ get{ return BeholderEye( this.VirtualArmor, 7 ); } }
		public override int BreathEffectItemID{ get{ return BeholderEye( this.VirtualArmor, 6 ); } }

		public static int BeholderEye ( int cyclops, int value )
		{
			if ( value == 1 && cyclops == 48 ){ return 100; } // RETURN THE FIRE DAMAGE
			if ( value == 2 && cyclops == 49 ){ return 100; } // RETURN THE COLD DAMAGE
			if ( value == 3 && cyclops == 50 ){ return 100; } // RETURN THE POISON DAMAGE
			if ( value == 4 && cyclops == 51 ){ return 100; } // RETURN THE ENERGY DAMAGE

			if ( value == 5 && cyclops == 48 ){ return 0; } // RETURN THE FIRE HUE
			if ( value == 5 && cyclops == 49 ){ return 0x481; } // RETURN THE COLD HUE
			if ( value == 5 && cyclops == 50 ){ return 0x3F; } // RETURN THE POISON HUE
			if ( value == 5 && cyclops == 51 ){ return 0x9C2; } // RETURN THE ENERGY HUE

			if ( value == 6 && cyclops == 48 ){ return 0x36D4; } // RETURN THE FIRE ID
			if ( value == 6 && cyclops == 49 ){ return 0x36D4; } // RETURN THE COLD ID
			if ( value == 6 && cyclops == 50 ){ return 0x36D4; } // RETURN THE POISON ID
			if ( value == 6 && cyclops == 51 ){ return 0x3818; } // RETURN THE ENERGY ID

			if ( value == 7 && cyclops == 48 ){ return 0x227; } // RETURN THE FIRE SOUND
			if ( value == 7 && cyclops == 49 ){ return 0x64F; } // RETURN THE COLD SOUND
			if ( value == 7 && cyclops == 50 ){ return 0x658; } // RETURN THE POISON SOUND
			if ( value == 7 && cyclops == 51 ){ return 0x665; } // RETURN THE ENERGY SOUND

			return 0;
		}

		public override void BreathDealDamage( Mobile target, int form )
		{
			if ( this.VirtualArmor == 48 ){ form = 17; } // RETURN THE FIRE DAMAGE
			if ( this.VirtualArmor == 49 ){ form = 19; } // RETURN THE COLD DAMAGE
			if ( this.VirtualArmor == 50 ){ form = 18; } // RETURN THE POISON DAMAGE
			if ( this.VirtualArmor == 51 ){ form = 20; } // RETURN THE ENERGY DAMAGE

			base.BreathDealDamage( target, form );

			this.VirtualArmor = Utility.RandomMinMax( 48, 51 ); // THIS IS USED TO RANDOMIZE ATTACK TYPES
		}

		[Constructable]
		public Beholder () : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = NameList.RandomName( "drakkul" );
			Title = "the beholder";
			Body = 674;
			BaseSoundID = 377;

			SetStr( 336, 365 );
			SetDex( 86, 105 );
			SetInt( 321, 425 );

			SetHits( 208, 295 );

			SetDamage( 15, 25 );

			SetDamageType( ResistanceType.Physical, 50 );
			SetDamageType( ResistanceType.Energy, 50 );

			SetResistance( ResistanceType.Physical, 45, 55 );
			SetResistance( ResistanceType.Fire, 60, 70 );
			SetResistance( ResistanceType.Cold, 40, 50 );
			SetResistance( ResistanceType.Poison, 40, 50 );
			SetResistance( ResistanceType.Energy, 75 );

			SetSkill( SkillName.Anatomy, 82.0, 105.0 );
			SetSkill( SkillName.Psychology, 100.1, 110.0 );
			SetSkill( SkillName.Magery, 100.1, 115.0 );
			SetSkill( SkillName.MagicResist, 125.0, 140.0 );
			SetSkill( SkillName.Tactics, 100.1, 110.0 );
			SetSkill( SkillName.FistFighting, 100.1, 110.0 );

			Fame = 15500;
			Karma = -15500;

			VirtualArmor = Utility.RandomMinMax( 48, 51 );
			m_NextSpecialAttack = DateTime.Now;
		}

		public override void OnDamage( int amount, Mobile from, bool willKill )
		{
			base.OnDamage( amount, from, willKill );

			if ( DateTime.Now >= m_NextSpecialAttack && from != null && from.Alive && !willKill )
			{
				if ( Utility.RandomDouble() < 0.50 )
				{
					TriggerSpecialAttack( from );
					m_NextSpecialAttack = DateTime.Now + TimeSpan.FromSeconds( 5 );
				}
			}
		}

		public override void OnGaveMeleeAttack( Mobile from)
		{
			base.OnGaveMeleeAttack(from);

			if ( DateTime.Now >= m_NextSpecialAttack && from != null && from.Alive )
			{
				if ( Utility.RandomDouble() < 0.30 )
				{
					TriggerSpecialAttack( from );
					m_NextSpecialAttack = DateTime.Now + TimeSpan.FromSeconds( 5 );
				}
			}
		}

		private void TriggerSpecialAttack( Mobile target )
		{
			int choice = Utility.Random( 6 );

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
					if ( BeholderSpecials.Disintegration( this, 90, 90, target ) )
					{
						this.Say( "*Fires a disintegration ray at {0}*", target.Name );
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
					if ( BeholderSpecials.Fear( this, 60, target ) )
					{
						this.Say( "*Strikes fear into {0}*", target.Name );
					}
					break;
				}
				case 4:
				{
					if ( BeholderSpecials.TelekineticRay( this, 6, 40 ) )
					{
						this.Say( "*A wave of Telekinetic energy oozes from an eyestalk!*", target.Name );
					}
					break;
				}
				case 5:
				{
					if ( BeholderSpecials.DeathRay( this, target, 21, 9, 90 ) )
					{
						this.Say( "*Fires a necrotic ray at {0}*", target.Name );
					}
					break;
				}
			}
		}

		public override int TreasureMapLevel{ get{ return Core.AOS ? 4 : 0; } }

		public override int Cloths{ get{ return 10; } }
		public override ClothType ClothType{ get{ return ClothType.Mysterious; } }

        public override int GetDeathSound()
        {
            return 0x56F;
        }
 
        public override int GetAttackSound()
        {
            return 0x570;
        }
 
        public override int GetIdleSound()
        {
            return 0x571;
        }
 
        public override int GetAngerSound()
        {
            return 0x572;
        }
 
        public override int GetHurtSound()
        {
            return 0x573;
        }

		public override void GenerateLoot()
		{
			AddLoot( LootPack.FilthyRich );
		}

		public Beholder( Serial serial ) : base( serial )
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