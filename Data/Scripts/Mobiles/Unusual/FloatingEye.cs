using System;
using Server;
using Server.Items;
using Server.Custom.BeholderSpecials;

namespace Server.Mobiles
{
	[CorpseName( "a floating eye corpse" )]
	public class FloatingEye : BaseCreature
	{
		private DateTime m_NextSpecialAttack;
		[Constructable]
		public FloatingEye () : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a floating eye";
			Body = 22;
			BaseSoundID = 377;
			Hue = 493;

			CanSwim = true;

			SetStr( 96, 125 );
			SetDex( 86, 105 );
			SetInt( 141, 165 );

			SetHits( 58, 75 );

			SetDamage( 5, 10 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 35, 40 );
			SetResistance( ResistanceType.Fire, 40, 50 );
			SetResistance( ResistanceType.Cold, 20, 30 );
			SetResistance( ResistanceType.Poison, 10, 20 );
			SetResistance( ResistanceType.Energy, 20, 30 );

			SetSkill( SkillName.Psychology, 50.1, 65.0 );
			SetSkill( SkillName.Magery, 50.1, 65.0 );
			SetSkill( SkillName.MagicResist, 60.1, 75.0 );
			SetSkill( SkillName.Tactics, 50.1, 70.0 );
			SetSkill( SkillName.FistFighting, 50.1, 70.0 );

			Fame = 3500;
			Karma = -3500;

			VirtualArmor = 36;
			m_NextSpecialAttack = DateTime.Now;
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Average );
			AddLoot( LootPack.Rich );
		}

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
			}
		}

		public override bool BleedImmune{ get{ return true; } }
		public override int TreasureMapLevel{ get{ return 1; } }
		public override int Meat{ get{ return 1; } }
		public override MeatType MeatType{ get{ return MeatType.Fish; } }
		public override int Skin{ get{ return Utility.Random(2); } }
		public override SkinType SkinType{ get{ return SkinType.Seaweed; } }

		public FloatingEye( Serial serial ) : base( serial )
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