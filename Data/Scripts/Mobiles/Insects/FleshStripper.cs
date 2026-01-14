using System;
using System.Collections;
using Server.Items;
using Server.Targeting;
using Server.Custom;
using Server.Network;

namespace Server.Mobiles
{
	[CorpseName( "a Fleshstripper's corpse" )]
	public class FleshStripper : BaseCreature
	{
		private DateTime m_NextSpitAllowed;

		[Constructable]
		public FleshStripper() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a Flesh Stripper";
			Body = 0x15C;
			BaseSoundID = 397;
			Hue = 0x09d3;
			SetStr( 123, 165 );
			SetDex( 96, 135 );
			SetInt( 16, 30 );

			SetHits( 150, 163 );
			SetMana( 0 );

			SetDamage( 9, 16 );

			SetDamageType( ResistanceType.Physical, 60 );
			SetDamageType( ResistanceType.Poison, 40 );

			SetResistance( ResistanceType.Physical, 35 );
			SetResistance( ResistanceType.Fire, 25 );
			SetResistance( ResistanceType.Cold, 15 );
			SetResistance( ResistanceType.Poison, 100 );
			SetResistance( ResistanceType.Energy, 25 );

			SetSkill( SkillName.Poisoning, 80.1, 100.0 );
			SetSkill( SkillName.MagicResist, 30.1, 35.0 );
			SetSkill( SkillName.Tactics, 60.3, 75.0 );
			SetSkill( SkillName.FistFighting, 50.3, 65.0 );

			Fame = 2500;
			Karma = -2500;

			VirtualArmor = 38;

			Tamable = true;
			ControlSlots = 2;
			MinTameSkill = 87.1;

			Item Venom = new VenomSack();
				Venom.Name = "deadly venom sack";
				AddItem( Venom );

			m_NextSpitAllowed = DateTime.UtcNow;
		}

		public override void OnThink()
		{
			base.OnThink();

			if ( Combatant != null && DateTime.UtcNow >= m_NextSpitAllowed && Hits < HitsMax )
			{
				Mobile target = Combatant as Mobile;
				
				if ( target != null && target.Alive && InRange( target, 5 ) && InLOS( target ) )
				{
					DoSpitAttack( target );
				}
			}
		}

		private void DoSpitAttack( Mobile target )
		{
			m_NextSpitAllowed = DateTime.UtcNow + TimeSpan.FromSeconds( 60 );

			Direction = GetDirectionTo( target );
			Animate( 4, 5, 1, true, false, 0 );

			MovingEffect( target, 0x36D4, 7, 0, false, true, 0x3F, 0 );

			Timer.DelayCall( TimeSpan.FromSeconds( 0.5 ), delegate()
			{
				if ( target != null && !target.Deleted )
				{
					int damage = Utility.RandomMinMax( 20, 35 );
					AOS.Damage( target, this, damage, 0, 0, 0, 100, 0 );

					if ( Utility.RandomDouble() < 0.25 )
						target.ApplyPoison( this, Poison.Greater );
					else
						target.ApplyPoison( this, Poison.Regular );

					Point3D loc = target.Location;
					Map map = target.Map;

					if ( map != null )
					{
						new CausticVenomTile( loc, map );
					}
				}
			});
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Meager );
			AddLoot( LootPack.Meager );
		}

		public override int Meat{ get{ return 1; } }
		public override FoodType FavoriteFood{ get{ return FoodType.Meat; } }
		public override PackInstinct PackInstinct{ get{ return PackInstinct.Arachnid; } }
		public override Poison PoisonImmune{ get{ return Poison.Deadly; } }
		public override Poison HitPoison{ get{ return Poison.Greater; } }

		public FleshStripper( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 );
			writer.Write( m_NextSpitAllowed );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
			m_NextSpitAllowed = reader.ReadDateTime();
		}
	}

	public class CausticVenomTile : Item
	{
		private Timer m_Timer;
		private DateTime m_End;

		public CausticVenomTile( Point3D loc, Map map ) : base( 0x122A )
		{
			Movable = false;
			MoveToWorld( loc, map );
			Hue = 0x3F;
			Name = "caustic venom";

			m_End = DateTime.UtcNow + TimeSpan.FromSeconds( 12 );
			m_Timer = Timer.DelayCall( TimeSpan.FromSeconds( 2 ), TimeSpan.FromSeconds( 2 ), OnTick );
		}

		private void OnTick()
		{
			if ( DateTime.UtcNow >= m_End )
			{
				Delete();
				return;
			}

			if ( Map == null || Map == Map.Internal )
				return;

			IPooledEnumerable eable = GetMobilesInRange( 0 );

			foreach ( Mobile m in eable )
			{
				if ( m != null && m.Alive && m.Player )
				{
					int damage = Utility.RandomMinMax( 10, 18 );
					AOS.Damage( m, damage, 0, 0, 0, 100, 0 );
				}
			}

			eable.Free();
		}

		public override void Delete()
		{
			if ( m_Timer != null )
			{
				m_Timer.Stop();
				m_Timer = null;
			}

			base.Delete();
		}

		public CausticVenomTile( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 );
			writer.Write( m_End );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
			m_End = reader.ReadDateTime();

			TimeSpan remaining = m_End - DateTime.UtcNow;
			
			if ( remaining > TimeSpan.Zero )
				m_Timer = Timer.DelayCall( TimeSpan.FromSeconds( 2 ), TimeSpan.FromSeconds( 2 ), OnTick );
			else
				Delete();
		}
	}
}