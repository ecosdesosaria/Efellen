using System;
using System.Collections;
using System.Collections.Generic;
using Server.Items;
using Server.Targeting;
using Server.Misc;
using Server.Custom.DailyBosses.System;

namespace Server.Mobiles
{
	[CorpseName( "Desmona's corpse" )]
	public class Desmona : BaseCreature
	{
        private DateTime m_NextSpecialAttack;
		private DateTime m_NextSpellTime;
		private bool m_IsChanneling;
		private Timer m_ChannelTimer;
		private Mobile m_ChannelTarget;
		private int m_ChannelSpell;
		private Dictionary<Point3D, Timer> m_AcidFogTimers;

		[Constructable]
		public Desmona() : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.4, 0.8 )
		{
			Name = "Desmona";
			Title = "the Enchantress";
            Hue = 0x0213;
			Body = 310;
			BaseSoundID = 0x482;

			SetStr( 396, 470 );
			SetDex( 121, 160 );
			SetInt( 56, 90 );
			SetHits( 358, 422 );
			SetDamage( 16, 25 );
			SetDamageType( ResistanceType.Energy, 100 );
			SetResistance( ResistanceType.Physical, 75 );
			SetResistance( ResistanceType.Fire, 40 );
			SetResistance( ResistanceType.Cold, 40 );
			SetResistance( ResistanceType.Poison, 40 );
			SetResistance( ResistanceType.Energy, 40 );
			SetSkill( SkillName.MagicResist, 70.0 );
			SetSkill( SkillName.Tactics, 80.0 );
			SetSkill( SkillName.FistFighting, 90.0 );
			SetSkill( SkillName.Magery, 101.0 );
			SetSkill( SkillName.Meditation, 101.0 );
            SetSkill( SkillName.Necromancy, 101.0 );

			Fame = 9500;
			Karma = -9500;
			VirtualArmor = 60;

			m_NextSpecialAttack = DateTime.MinValue;
            m_NextSpellTime = DateTime.UtcNow;
			m_IsChanneling = false;
			m_AcidFogTimers = new Dictionary<Point3D, Timer>();
		}

		public override void OnDamage( int amount, Mobile from, bool willKill )
		{
			if ( m_IsChanneling )
				InterruptChannel();

			if ( DateTime.UtcNow >= m_NextSpecialAttack )
			{
				PerformRageAttack( from );
				m_NextSpecialAttack = DateTime.UtcNow + TimeSpan.FromSeconds( 35 );
			}
			
			base.OnDamage( amount, from, willKill );
		}

		private void InterruptChannel()
		{
			if ( !m_IsChanneling )
				return;

			m_IsChanneling = false;
			this.Frozen = false;
            Say( "*O feitiço falha*" );

			if ( m_ChannelTimer != null )
			{
				m_ChannelTimer.Stop();
				m_ChannelTimer = null;
			}

			m_ChannelTarget = null;
		}

		public override void OnThink()
		{
			base.OnThink();

			if ( !m_IsChanneling && Combatant != null && Hits < HitsMax && 
			     DateTime.UtcNow >= m_NextSpellTime && Utility.RandomDouble() < 0.10 )
			{
				m_ChannelSpell = Utility.Random( 7 );
				m_ChannelTarget = Combatant;
				StartChanneling();
			}
		}

		private void StartChanneling()
		{
			m_IsChanneling = true;
			this.Frozen = true;
            Say( "*começa a canalizar um feitiço*" );
			m_ChannelTimer = Timer.DelayCall( TimeSpan.FromSeconds( 3 ), new TimerCallback( ExecuteSpell ) );
		}

		private void ExecuteSpell()
		{
			if ( !m_IsChanneling )
				return;

			m_IsChanneling = false;
			this.Frozen = false;

			switch ( m_ChannelSpell )
			{
				case 0: CastFireball(); break;
				case 1: CastMagicMissile(); break;
				case 2: CastWeb(); break;
				case 3: CastDisintegrate(); break;
				case 4: CastPowerwordFear(); break;
				case 5: CastPowerwordSlow(); break;
				case 6: CastAcidFog(); break;
			}

			m_NextSpellTime = DateTime.UtcNow + TimeSpan.FromMinutes( 1 );
		}

		private void CastFireball()
		{
			if ( m_ChannelTarget == null || m_ChannelTarget.Deleted || !m_ChannelTarget.Alive )
				return;

            Say( "*Lança Bola de Fogo*" );

			Point3D targetLoc = m_ChannelTarget.Location;
			Map map = m_ChannelTarget.Map;

			Effects.SendLocationEffect( targetLoc, map, 0x36BD, 20, 10, 0x0213, 0 );
			Effects.PlaySound( targetLoc, map, 0x307 );

			for ( int x = -1; x <= 1; x++ )
			{
				for ( int y = -1; y <= 1; y++ )
				{
					Point3D loc = new Point3D( targetLoc.X + x, targetLoc.Y + y, targetLoc.Z );
					Effects.SendLocationEffect( loc, map, 0x36BD, 20, 10, 0x0213, 0 );
				}
			}

			List<Mobile> targets = new List<Mobile>();
			IPooledEnumerable eable = m_ChannelTarget.GetMobilesInRange( 1 );

			foreach ( Mobile m in eable )
			{
				if ( m != this && m.Alive && CanBeHarmful( m ) )
					targets.Add( m );
			}
			eable.Free();

			foreach ( Mobile m in targets )
			{
				int damage = Utility.RandomMinMax( 35, 45 );
				AOS.Damage( m, this, damage, 0, 100, 0, 0, 0 );
			}
		}

		private void CastMagicMissile()
		{
			if ( m_ChannelTarget == null || m_ChannelTarget.Deleted || !m_ChannelTarget.Alive )
				return;

            Say( "*Lança Mísseis Mágicos*" );

			int projectileCount = Utility.RandomMinMax( 2, 5 );
			
			for ( int i = 0; i < projectileCount; i++ )
			{
				Timer.DelayCall( TimeSpan.FromSeconds( i * 0.3 ), delegate()
				{
					if ( m_ChannelTarget != null && m_ChannelTarget.Alive && !m_ChannelTarget.Deleted )
					{
						Effects.SendMovingEffect( this, m_ChannelTarget, 0x379F, 7, 0, false, false, 0x0213, 0 );
						Effects.PlaySound( this.Location, this.Map, 0x1F5 );

						Timer.DelayCall( TimeSpan.FromSeconds( 0.5 ), delegate()
						{
							if ( m_ChannelTarget != null && m_ChannelTarget.Alive && !m_ChannelTarget.Deleted )
							{
								int damage = Utility.RandomMinMax( 5, 13 );
								AOS.Damage( m_ChannelTarget, this, damage, 0, 0, 0, 0, 100 );
								Effects.SendLocationEffect( m_ChannelTarget.Location, m_ChannelTarget.Map, 0x3709, 10, 30, 0x0213, 0 );
							}
						});
					}
				});
			}
		}

		private void CastWeb()
		{
			if ( m_ChannelTarget == null || m_ChannelTarget.Deleted || !m_ChannelTarget.Alive )
				return;

            Say( "*Lança Teia*" );

			List<Mobile> targets = new List<Mobile>();
			IPooledEnumerable eable = m_ChannelTarget.GetMobilesInRange( 2 );

			foreach ( Mobile m in eable )
			{
				if ( m != this && m.Alive && CanBeHarmful( m ) )
					targets.Add( m );
			}
			eable.Free();

			foreach ( Mobile m in targets )
			{
				double duration = 15.0 - ( m.Skills[SkillName.MagicResist].Value / 10.0 + m.Dex / 10.0 );
				
				if ( duration < 3.0 )
					duration = 3.0;

				m.Paralyze( TimeSpan.FromSeconds( duration ) );
				Effects.SendLocationParticles( EffectItem.Create( m.Location, m.Map, EffectItem.DefaultDuration ), 0x376A, 9, 10, 0x0213 );
				Effects.SendLocationEffect( m.Location, m.Map, 0x3709, 30, 10, 0x0213, 0 );
				Effects.PlaySound( m.Location, m.Map, 0x204 );
				m.SendMessage( "Você está preso em uma teia mágica!" );
			}
		}

		private void CastDisintegrate()
		{
			if ( m_ChannelTarget == null || m_ChannelTarget.Deleted || !m_ChannelTarget.Alive )
				return;

            Say( "*Lança Desintegração*" );
			Effects.SendMovingEffect( this, m_ChannelTarget, 0x379F, 7, 0, false, false, 0x0213, 0 );
			Effects.PlaySound( this.Location, this.Map, 0x1F1 );

			Timer.DelayCall( TimeSpan.FromSeconds( 0.5 ), delegate()
			{
				if ( m_ChannelTarget != null && m_ChannelTarget.Alive && !m_ChannelTarget.Deleted )
				{
					int damage = ( m_ChannelTarget.Hits <= m_ChannelTarget.HitsMax / 2 )
						? Utility.RandomMinMax( 70, 95 )
						: Utility.RandomMinMax( 35, 40 );

					AOS.Damage( m_ChannelTarget, this, damage, 100, 0, 0, 0, 0 );
					Effects.SendLocationEffect( m_ChannelTarget.Location, m_ChannelTarget.Map, 0x3709, 30, 10, 0x0213, 0 );
					Effects.PlaySound( m_ChannelTarget.Location, m_ChannelTarget.Map, 0x307 );
				}
			});
		}

		private void CastPowerwordFear()
		{
            Say( "*Lança Palavra de Poder: Medo*" );

			List<Mobile> targets = new List<Mobile>();
			IPooledEnumerable eable = this.GetMobilesInRange( 4 );

			foreach ( Mobile m in eable )
			{
				if ( m != this && m.Alive && CanBeHarmful( m ) )
					targets.Add( m );
			}
			eable.Free();

			foreach ( Mobile m in targets )
			{
				if ( m.Skills[SkillName.Knightship].Value >= 95.0 )
				{
					m.SendMessage( "Sua coragem te protege contra o medo!" );
					continue;
				}

				Point3D newLoc = FindValidTeleportLocation( m, Utility.RandomMinMax( 4, 6 ) );

				if ( newLoc != Point3D.Zero )
				{
					Effects.SendLocationEffect( m.Location, m.Map, 0x3709, 30, 10, 0x0213, 0 );
					Effects.PlaySound( m.Location, m.Map, 0x482 );
					m.MoveToWorld( newLoc, m.Map );
					Effects.SendLocationEffect( newLoc, m.Map, 0x3709, 30, 10, 0x0213, 0 );

					double duration = 17.0 - ( m.Skills[SkillName.MagicResist].Value / 12.0 );
					
					if ( duration > 0 )
						m.Paralyze( TimeSpan.FromSeconds( duration ) );

					m.SendMessage( "Você é tomado por um medo avassalador!" );
				}
			}

			Effects.PlaySound( this.Location, this.Map, 0x5C3 );
		}

		private Point3D FindValidTeleportLocation( Mobile target, int distance )
		{
			for ( int attempt = 0; attempt < 20; attempt++ )
			{
				int xOffset = Utility.RandomMinMax( -distance, distance );
				int yOffset = Utility.RandomMinMax( -distance, distance );
				Point3D newLoc = new Point3D( target.X + xOffset, target.Y + yOffset, target.Z );

				if ( target.Map.CanSpawnMobile( newLoc ) )
					return newLoc;
			}

			return Point3D.Zero;
		}

		private void CastPowerwordSlow()
		{
            Say( "*Lança Palavra de Poder: Lentidão*" );

			List<Mobile> targets = new List<Mobile>();
			IPooledEnumerable eable = this.GetMobilesInRange( 4 );

			foreach ( Mobile m in eable )
			{
				if ( m != this && m.Alive && CanBeHarmful( m ) )
					targets.Add( m );
			}
			eable.Free();

			foreach ( Mobile m in targets )
			{
				m.Stam = 1;
				Effects.SendLocationEffect( m.Location, m.Map, 0x3709, 30, 10, 0x0213, 0 );
				Effects.PlaySound( m.Location, m.Map, 0x204 );
				m.SendMessage( "Você se sente incrivelmente lento!" );
			}

			Effects.PlaySound( this.Location, this.Map, 0x5C3 );
		}

		private void CastAcidFog()
		{
			if ( m_ChannelTarget == null || m_ChannelTarget.Deleted || !m_ChannelTarget.Alive )
				return;

            Say( "*Lança Névoa Ácida*" );

			Point3D centerLoc = m_ChannelTarget.Location;
			Map map = m_ChannelTarget.Map;
			List<Point3D> fogLocations = new List<Point3D>();

			for ( int x = -3; x <= 3; x++ )
			{
				for ( int y = -3; y <= 3; y++ )
				{
					if ( Math.Sqrt( x * x + y * y ) <= 3.0 )
					{
						Point3D loc = new Point3D( centerLoc.X + x, centerLoc.Y + y, centerLoc.Z );
						fogLocations.Add( loc );
						Effects.SendLocationEffect( loc, map, 0x3709, 30, 10, 0x0213, 0 );
					}
				}
			}

			Effects.PlaySound( centerLoc, map, 0x208 );

			foreach ( Point3D loc in fogLocations )
			{
				if ( m_AcidFogTimers.ContainsKey( loc ) )
				{
					m_AcidFogTimers[loc].Stop();
					m_AcidFogTimers[loc] = new AcidFogTimer( this, loc, map );
				}
				else
				{
					m_AcidFogTimers.Add( loc, new AcidFogTimer( this, loc, map ) );
				}

				m_AcidFogTimers[loc].Start();
			}
		}

		private class AcidFogTimer : Timer
		{
			private Desmona m_Owner;
			private Point3D m_Location;
			private Map m_Map;
			private int m_Ticks;
			private const int MaxTicks = 6;

			public AcidFogTimer( Desmona owner, Point3D location, Map map ) 
				: base( TimeSpan.FromSeconds( 2.0 ), TimeSpan.FromSeconds( 2.0 ) )
			{
				m_Owner = owner;
				m_Location = location;
				m_Map = map;
				m_Ticks = 0;
			}

			protected override void OnTick()
			{
				m_Ticks++;

				if ( m_Ticks > MaxTicks || m_Owner == null || m_Owner.Deleted || !m_Owner.Alive )
				{
					Stop();
					if ( m_Owner != null && m_Owner.m_AcidFogTimers.ContainsKey( m_Location ) )
						m_Owner.m_AcidFogTimers.Remove( m_Location );
					return;
				}

				Effects.SendLocationEffect( m_Location, m_Map, 0x3709, 30, 10, 0x0213, 0 );

				IPooledEnumerable eable = m_Map.GetMobilesInRange( m_Location, 0 );
				foreach ( Mobile m in eable )
				{
					if ( m != m_Owner && m.Alive && m_Owner.CanBeHarmful( m ) )
					{
						int damage = Utility.RandomMinMax( 15, 30 );
						AOS.Damage( m, m_Owner, damage, 0, 0, 0, 100, 0 );
					}
				}
				eable.Free();
			}
		}

		private void PerformRageAttack( Mobile target )
		{
			if ( target == null || target.Deleted || !target.Alive )
				return;

			int attackChoice = Utility.RandomMinMax( 1, 3 );

			switch ( attackChoice )
			{
				case 1:
					BossSpecialAttack.PerformTargettedAoE( this, target, 1, "You will not touch our books!", 0x0213, 20, 20, 20, 20, 20 );
					break;
				case 2:
					BossSpecialAttack.PerformCrossExplosion( this, target, "Leave our home at once!", 0x0213, 1, 20, 20, 20, 20, 20 );
                	break;
				case 3:
					BossSpecialAttack.PerformSlam( this, "Bal shall not be disturbed!", 0x0213, 2, 6, 0, 0, 0, 100, 0 );
                	break;
			}
		}

		public override void OnDelete()
		{
			if ( m_ChannelTimer != null )
			{
				m_ChannelTimer.Stop();
				m_ChannelTimer = null;
			}

			foreach ( Timer timer in m_AcidFogTimers.Values )
			{
				if ( timer != null )
					timer.Stop();
			}
			m_AcidFogTimers.Clear();

			base.OnDelete();
		}

		public override void OnDeath( Container c )
		{
			base.OnDeath( c );

			Mobile killer = this.LastKiller;
			if ( killer != null )
			{
				if ( killer is BaseCreature )
					killer = ((BaseCreature)killer).GetMaster();
                
				if ( killer is PlayerMobile )
				{
					if ( Utility.RandomMinMax( 1, 10 ) == 1 )
                    {
                        EtherealUnicorn mount = new EtherealUnicorn();
                        mount.Hue = 0x0213;
                        c.DropItem( mount );
                    }
					c.DropItem( new EerieIdol( Utility.Random( 7, 14 ) ) );
				}
			}
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Rich, 2 );
			AddLoot( LootPack.Gems, 2 );
			AddLoot( LootPack.MedPotions, 2 );
		}

		public override bool BleedImmune{ get{ return true; } }
		public override Poison PoisonImmune{ get{ return Poison.Greater; } }
		public override int Cloths{ get{ return 14; } }
		public override ClothType ClothType{ get{ return ClothType.Haunted; } }

		public Desmona( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 1 );
            writer.Write( m_NextSpecialAttack );
			writer.Write( m_NextSpellTime );
			writer.Write( m_IsChanneling );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();

			m_AcidFogTimers = new Dictionary<Point3D, Timer>();

            if ( version >= 1 )
			{
				m_NextSpecialAttack = reader.ReadDateTime();
				m_NextSpellTime = reader.ReadDateTime();
				m_IsChanneling = reader.ReadBool();

				if ( m_IsChanneling )
					m_IsChanneling = false;
			}
		}
	}
}