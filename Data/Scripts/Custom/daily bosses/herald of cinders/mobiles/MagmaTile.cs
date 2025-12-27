using System;
using Server;
using Server.Mobiles;
using Server.Network;
using System.Collections;
using Server.Items;

namespace Server.Mobiles
{
	public class MagmaTile : Item
	{
		private Mobile m_Caster;
		private Timer m_DamageTimer;
		private Timer m_EffectTimer;
		private Hashtable m_DamagedThisTick = new Hashtable();

		public MagmaTile( Mobile caster, Point3D loc, Map map ) : base( 0x1 )
		{
			Movable = false;
			Visible = false;
			m_Caster = caster;

			MoveToWorld( loc, map );

			// Damage timer - ticks every second
			m_DamageTimer = Timer.DelayCall( TimeSpan.FromSeconds( 1.0 ), TimeSpan.FromSeconds( 1.0 ), new TimerCallback( DamageTick ) );

			// Visual effect timer - creates continuous visual effects
			m_EffectTimer = Timer.DelayCall( TimeSpan.Zero, TimeSpan.FromSeconds( 0.5 ), new TimerCallback( EffectTick ) );
		}

		private void DamageTick()
		{
			if ( Deleted || m_Caster == null || m_Caster.Deleted )
			{
				Delete();
				return;
			}

			m_DamagedThisTick.Clear();

			IPooledEnumerable eable = GetMobilesInRange( 0 );
			foreach ( Mobile m in eable )
			{
				if ( m == null || m.Deleted || !m.Alive )
					continue;

				// Check immunity
				if ( HeraldOfCinders.IsImmuneToMagma( m ) )
					continue;

				if ( m_Caster.CanBeHarmful( m ) )
				{
					m_Caster.DoHarmful( m );

					int damage = Utility.RandomMinMax( 20, 30 );
					AOS.Damage( m, m_Caster, damage, 0, 100, 0, 0, 0 );

					m.FixedParticles( 0x3709, 10, 30, 5052, EffectLayer.Waist );
					m.PlaySound( 0x208 );

					m_DamagedThisTick[m] = true;
				}
			}
			eable.Free();
		}

		private void EffectTick()
		{
			if ( Deleted )
				return;
				Effects.SendLocationParticles(
                            EffectItem.Create(Location, Map, EffectItem.DefaultDuration), 
                            0x3709, 20, 10, 0x208,0
                        );

			// Occasional smoke
			if ( Utility.RandomBool() )
			{
				Effects.SendLocationEffect( Location, Map, 0x3728, 10, 10, 2023, 0 );
			}
		}

		public override bool OnMoveOver( Mobile m )
		{
			if ( m == null || m.Deleted || !m.Alive )
				return true;

			// Check immunity
			if ( HeraldOfCinders.IsImmuneToMagma( m ) )
				return true;

			// Only damage if not already damaged this tick
			if ( !m_DamagedThisTick.Contains( m ) && m_Caster != null && !m_Caster.Deleted )
			{
				if ( m_Caster.CanBeHarmful( m ) )
				{
					m_Caster.DoHarmful( m );

					int damage = Utility.RandomMinMax( 15, 25 );
					AOS.Damage( m, m_Caster, damage, 0, 100, 0, 0, 0 );

					m.FixedParticles( 0x3709, 10, 30, 5052, EffectLayer.Waist );
					m.PlaySound( 0x208 );

					m_DamagedThisTick[m] = true;
				}
			}

			return true;
		}

		public override void Delete()
		{
			if ( m_DamageTimer != null )
			{
				m_DamageTimer.Stop();
				m_DamageTimer = null;
			}

			if ( m_EffectTimer != null )
			{
				m_EffectTimer.Stop();
				m_EffectTimer = null;
			}

			base.Delete();
		}

		public MagmaTile( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 ); // version

			writer.Write( m_Caster );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();

			m_Caster = reader.ReadMobile();

			Delete(); // Delete on server restart
		}
	}
}