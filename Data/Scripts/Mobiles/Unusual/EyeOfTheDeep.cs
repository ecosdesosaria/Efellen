using System;
using Server;
using Server.Items;
using Server.Custom.BeholderSpecials;

namespace Server.Mobiles
{
	[CorpseName( "a deep eye corpse" )]
	public class EyeOfTheDeep : BaseCreature
	{
		private DateTime m_NextSpecialAttack;
		[Constructable]
		public EyeOfTheDeep () : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "an eye of the deep";
			Body = 159;
			BaseSoundID = 377;

			CanSwim = true;

			SetStr( 296, 325 );
			SetDex( 86, 105 );
			SetInt( 291, 385 );

			SetHits( 178, 195 );

			SetDamage( 8, 19 );

			SetDamageType( ResistanceType.Physical, 50 );
			SetDamageType( ResistanceType.Energy, 50 );

			SetResistance( ResistanceType.Physical, 45, 55 );
			SetResistance( ResistanceType.Fire, 60, 70 );
			SetResistance( ResistanceType.Cold, 40, 50 );
			SetResistance( ResistanceType.Poison, 40, 50 );
			SetResistance( ResistanceType.Energy, 40, 50 );

			SetSkill( SkillName.Anatomy, 62.0, 100.0 );
			SetSkill( SkillName.Psychology, 90.1, 100.0 );
			SetSkill( SkillName.Magery, 90.1, 100.0 );
			SetSkill( SkillName.MagicResist, 115.1, 130.0 );
			SetSkill( SkillName.Tactics, 80.1, 100.0 );
			SetSkill( SkillName.FistFighting, 80.1, 100.0 );

			Fame = 12500;
			Karma = -12500;

			VirtualArmor = 50;
			m_NextSpecialAttack = DateTime.Now;
		}

		public override bool BleedImmune{ get{ return true; } }
		public override int TreasureMapLevel{ get{ return Core.AOS ? 4 : 0; } }
		public override int Skin{ get{ return Utility.Random(2); } }
		public override SkinType SkinType{ get{ return SkinType.Seaweed; } }

		public override void OnDamage( int amount, Mobile from, bool willKill )
		{
			base.OnDamage( amount, from, willKill );

			if ( DateTime.Now >= m_NextSpecialAttack && from != null && from.Alive && !willKill )
			{
				if ( Utility.RandomDouble() < 0.50 )
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
				if ( Utility.RandomDouble() < 0.30 )
				{
					TriggerSpecialAttack( from );
					m_NextSpecialAttack = DateTime.Now + TimeSpan.FromSeconds( 30 );
				}
			}
		}

		private void TriggerSpecialAttack( Mobile target )
		{
			int choice = Utility.Random( 5 );

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
			}
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.FilthyRich );
			AddLoot( LootPack.Rich );
		}

		public EyeOfTheDeep( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 1 ); // version
			writer.Write(m_NextSpecialAttack);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadEncodedInt();
			if (version >= 1)
		        m_NextSpecialAttack = reader.ReadDateTime();
		    else
		        m_NextSpecialAttack = DateTime.MinValue;
		}
	}
}