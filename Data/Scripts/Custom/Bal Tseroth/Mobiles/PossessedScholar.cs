using System;
using Server;
using Server.Misc;
using Server.Items;
using System.Collections.Generic;
using Server.Custom.BalTsareth;

namespace Server.Mobiles 
{ 
	[CorpseName( "a possesed Scholar's corpse" )] 
	public class PossessedScholar : BaseCreature 
	{ 
		private DateTime m_NextSpellTime;
		private bool m_IsChanneling;
		private Timer m_ChannelTimer;
		private Mobile m_ChannelTarget;
		private int m_ChannelSpell;
		private DateTime m_ArcaneArmorEnds;
		private bool m_HasArcaneArmor;
		private int m_OriginalPhysicalResist;
		private Dictionary<Point3D, Timer> m_AcidFogTimers;

		[Constructable] 
		public PossessedScholar() : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 ) 
		{ 
			Title = "the possessed scholar";
			if ( this.Female = Utility.RandomBool() ) 
			{ 
				Body = 0x191; 
				Name = NameList.RandomName( "evil witch" );
				Utility.AssignRandomHair( this );
				HairHue = Utility.RandomHairHue();
			} 
			else 
			{ 
				Body = 0x190; 
				Name = NameList.RandomName( "evil mage" );
				Utility.AssignRandomHair( this );
				int HairColor = Utility.RandomHairHue();
				FacialHairItemID = Utility.RandomList( 0, 8254, 8255, 8256, 8257, 8267, 8268, 8269 );
				HairHue = HairColor;
				FacialHairHue = HairColor;
			}

			Hue = Utility.RandomSkinColor();
			EmoteHue = 1;

			Server.Misc.IntelligentAction.DressUpWizards( this, false );
			Item[] items = new Item[] { this.FindItemOnLayer( Layer.InnerTorso ), this.FindItemOnLayer( Layer.OuterTorso ), 
				this.FindItemOnLayer( Layer.Pants ), this.FindItemOnLayer( Layer.Arms ), this.FindItemOnLayer( Layer.Gloves ), 
				this.FindItemOnLayer( Layer.Helm ), this.FindItemOnLayer( Layer.Neck ), this.FindItemOnLayer( Layer.Waist ), 
				this.FindItemOnLayer( Layer.OneHanded ), this.FindItemOnLayer( Layer.TwoHanded ), this.FindItemOnLayer( Layer.FirstValid ) };

			foreach ( Item item in items )
			{
				if ( item != null )
					item.Hue = 0x0213;
			}

			SetStr( 151, 175 );
			SetDex( 261, 285 );
			SetInt( 196, 220 );

			SetHits( 149, 163 );

			SetDamage( 8, 14 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 35, 40 );
			SetResistance( ResistanceType.Fire, 30, 40 );
			SetResistance( ResistanceType.Cold, 30, 40 );
			SetResistance( ResistanceType.Poison, 30, 40 );
			SetResistance( ResistanceType.Energy, 30, 40 );

			SetSkill( SkillName.Psychology, 80.2, 100.0 );
			SetSkill( SkillName.Magery, 95.1, 100.0 );
			SetSkill( SkillName.Meditation, 27.5, 50.0 );
			SetSkill( SkillName.MagicResist, 77.5, 100.0 );
			SetSkill( SkillName.Tactics, 65.0, 87.5 );
			SetSkill( SkillName.FistFighting, 20.3, 80.0 );
			SetSkill( SkillName.Bludgeoning, 20.3, 80.0 );

			Fame = 12500;
			Karma = -12500;

			VirtualArmor = 40;
			PackReg( Utility.RandomMinMax( 4, 12 ) );
			PackReg( Utility.RandomMinMax( 4, 12 ) );
			PackReg( Utility.RandomMinMax( 4, 12 ) );

			if ( 0.9 > Utility.RandomDouble() )
				PackItem( new ArcaneGem() );
			if ( 0.8 > Utility.RandomDouble() )
				PackItem( new EerieIdol() );

			m_NextSpellTime = DateTime.UtcNow;
			m_IsChanneling = false;
			m_HasArcaneArmor = false;
			m_AcidFogTimers = new Dictionary<Point3D, Timer>();
			
		}

		public override void OnDamage( int amount, Mobile from, bool willKill )
		{
			base.OnDamage( amount, from, willKill );

			if ( m_IsChanneling )
			{
				InterruptChannel();
			}
		}

		private void InterruptChannel()
		{
			if ( !m_IsChanneling )
				return;

			m_IsChanneling = false;
			this.Frozen = false;
            Say("*The spell fizzles*");

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

			if ( m_HasArcaneArmor && DateTime.UtcNow >= m_ArcaneArmorEnds )
			{
				RemoveArcaneArmor();
			}

			if ( !m_IsChanneling && Combatant != null && Hits < HitsMax && 
			     DateTime.UtcNow >= m_NextSpellTime && Utility.RandomDouble() < 0.10 )
			{
				m_ChannelSpell = Utility.Random( 8 );
				m_ChannelTarget = Combatant;
				
				StartChanneling();
			}
		}

		private void StartChanneling()
		{
			m_IsChanneling = true;
			this.Frozen = true;
            Say("*starts channeling a spell*");

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
				case 0: CastArcaneArmor(); break;
				case 1: CastFireball(); break;
				case 2: CastMagicMissile(); break;
				case 3: CastWeb(); break;
				case 4: CastDisintegrate(); break;
				case 5: CastPowerwordFear(); break;
				case 6: CastPowerwordSlow(); break;
				case 7: CastAcidFog(); break;
			}

			m_NextSpellTime = DateTime.UtcNow + TimeSpan.FromMinutes( 1 );
		}

		private void CastArcaneArmor()
		{
            Say("*Casts Arcane Armor*");

			Effects.SendLocationParticles( EffectItem.Create( this.Location, this.Map, EffectItem.DefaultDuration ), 0x376A, 9, 32, 0x0213 );
			Effects.PlaySound( this.Location, this.Map, 0x1F2 );

			m_OriginalPhysicalResist = this.GetResistance( ResistanceType.Physical );
			this.SetResistance( ResistanceType.Physical, 60 );
			
			m_HasArcaneArmor = true;
			m_ArcaneArmorEnds = DateTime.UtcNow + TimeSpan.FromSeconds( 60 );
		}

		private void RemoveArcaneArmor()
		{
			if ( !m_HasArcaneArmor )
				return;

			m_HasArcaneArmor = false;
			this.SetResistance( ResistanceType.Physical, m_OriginalPhysicalResist );
		}

		private void CastFireball()
		{
			if ( m_ChannelTarget == null || m_ChannelTarget.Deleted || !m_ChannelTarget.Alive )
				return;

            Say("*Casts Fireball*");

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

			IPooledEnumerable eable = m_ChannelTarget.GetMobilesInRange( 1 );
			List<Mobile> targets = new List<Mobile>();

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

            Say("Casts magic Missle");

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

            Say("*Casts Web*");

			IPooledEnumerable eable = m_ChannelTarget.GetMobilesInRange( 2 );
			List<Mobile> targets = new List<Mobile>();

			foreach ( Mobile m in eable )
			{
				if ( m != this && m.Alive && CanBeHarmful( m ) )
					targets.Add( m );
			}
			eable.Free();

			foreach ( Mobile m in targets )
			{
				double magicResist = m.Skills[SkillName.MagicResist].Value;
				int dex = m.Dex;
				double duration = 15.0 - (magicResist / 10.0 + dex / 10.0);
				
				if ( duration < 3.0 )
					duration = 3.0;

				m.Paralyze( TimeSpan.FromSeconds( duration ) );

				Effects.SendLocationParticles( EffectItem.Create( m.Location, m.Map, EffectItem.DefaultDuration ), 0x376A, 9, 10, 0x0213 );
				Effects.SendLocationEffect( m.Location, m.Map, 0x3709, 30, 10, 0x0213, 0 );
				Effects.PlaySound( m.Location, m.Map, 0x204 );

				m.SendMessage( "You are trapped in a magical web!" );
			}
		}

		private void CastDisintegrate()
		{
			if ( m_ChannelTarget == null || m_ChannelTarget.Deleted || !m_ChannelTarget.Alive )
				return;
            Say("*Casts Disintegrate*");

			Effects.SendMovingEffect( this, m_ChannelTarget, 0x379F, 7, 0, false, false, 0x0213, 0 );
			Effects.PlaySound( this.Location, this.Map, 0x1F1 );

			Timer.DelayCall( TimeSpan.FromSeconds( 0.5 ), delegate()
			{
				if ( m_ChannelTarget != null && m_ChannelTarget.Alive && !m_ChannelTarget.Deleted )
				{
					int damage;
					if ( m_ChannelTarget.Hits <= (m_ChannelTarget.HitsMax / 2) )
					{
						damage = Utility.RandomMinMax( 70, 95 );
					}
					else
					{
						damage = Utility.RandomMinMax( 35, 40 );
					}

					AOS.Damage( m_ChannelTarget, this, damage, 100, 0, 0, 0, 0 );
					Effects.SendLocationEffect( m_ChannelTarget.Location, m_ChannelTarget.Map, 0x3709, 30, 10, 0x0213, 0 );
					Effects.PlaySound( m_ChannelTarget.Location, m_ChannelTarget.Map, 0x307 );
				}
			});
		}

		private void CastPowerwordFear()
		{
            Say("*Casts Powerword: Fear*");

			IPooledEnumerable eable = this.GetMobilesInRange( 4 );
			List<Mobile> targets = new List<Mobile>();

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
					m.SendMessage( "Your valor steels you against fear!" );
					continue;
				}

				int distance = Utility.RandomMinMax( 4, 6 );
				Point3D newLoc = Point3D.Zero;
				bool foundLocation = false;

				for ( int attempt = 0; attempt < 20; attempt++ )
				{
					int xOffset = Utility.RandomMinMax( -distance, distance );
					int yOffset = Utility.RandomMinMax( -distance, distance );
					
					newLoc = new Point3D( m.X + xOffset, m.Y + yOffset, m.Z );

					if ( m.Map.CanSpawnMobile( newLoc ) )
					{
						foundLocation = true;
						break;
					}
				}

				if ( foundLocation )
				{
					Effects.SendLocationEffect( m.Location, m.Map, 0x3709, 30, 10, 0x0213, 0 );
					Effects.PlaySound( m.Location, m.Map, 0x482 );

					m.MoveToWorld( newLoc, m.Map );

					Effects.SendLocationEffect( newLoc, m.Map, 0x3709, 30, 10, 0x0213, 0 );

					double magicResist = m.Skills[SkillName.MagicResist].Value;
					double duration = 17.0 - (magicResist / 12.0);
					
					if ( duration < 0 )
						duration = 0;

					if ( duration > 0 )
						m.Paralyze( TimeSpan.FromSeconds( duration ) );

					m.SendMessage( "You are gripped by overwhelming fear!" );
				}
			}

			Effects.PlaySound( this.Location, this.Map, 0x5C3 );
		}

		private void CastPowerwordSlow()
		{
            Say("*Casts Powerword: Slow*");

			IPooledEnumerable eable = this.GetMobilesInRange( 4 );
			List<Mobile> targets = new List<Mobile>();

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

				m.SendMessage( "You feel incredibly sluggish!" );
			}

			Effects.PlaySound( this.Location, this.Map, 0x5C3 );
		}

		private void CastAcidFog()
		{
			if ( m_ChannelTarget == null || m_ChannelTarget.Deleted || !m_ChannelTarget.Alive )
				return;

            Say("*Casts Acid Fog*");

			Point3D centerLoc = m_ChannelTarget.Location;
			Map map = m_ChannelTarget.Map;

			List<Point3D> fogLocations = new List<Point3D>();

			for ( int x = -3; x <= 3; x++ )
			{
				for ( int y = -3; y <= 3; y++ )
				{
					if ( Math.Sqrt(x * x + y * y) <= 3.0 )
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
				Timer fogTimer = new AcidFogTimer( this, loc, map );
				fogTimer.Start();

				if ( m_AcidFogTimers.ContainsKey( loc ) )
				{
					m_AcidFogTimers[loc].Stop();
					m_AcidFogTimers[loc] = fogTimer;
				}
				else
				{
					m_AcidFogTimers.Add( loc, fogTimer );
				}
			}
		}

		private class AcidFogTimer : Timer
		{
			private PossessedScholar m_Owner;
			private Point3D m_Location;
			private Map m_Map;
			private int m_Ticks;
			private const int MaxTicks = 6; // 12 seconds / 2 seconds per tick

			public AcidFogTimer( PossessedScholar owner, Point3D location, Map map ) 
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

		public override void OnDelete()
		{
			if ( m_ChannelTimer != null )
			{
				m_ChannelTimer.Stop();
				m_ChannelTimer = null;
			}

			if ( m_HasArcaneArmor )
			{
				RemoveArcaneArmor();
			}

			// Clean up acid fog timers
			foreach ( Timer timer in m_AcidFogTimers.Values )
			{
				if ( timer != null )
					timer.Stop();
			}
			m_AcidFogTimers.Clear();

			base.OnDelete();
		}

		public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);
			if(Utility.RandomDouble() < 0.15)
            {
				Say(Server.Custom.BalTsareth.BalTsarethSpeech.GetAttackLine(defender));              
            }
        }

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Average );
			AddLoot( LootPack.Meager );
			AddLoot( LootPack.MedScrolls, 2 );
			AddLoot( LootPack.MedScrolls, 2 );
			AddLoot( LootPack.MedPotions );
		}

		public override bool ClickTitle{ get{ return false; } }
		public override bool ShowFameTitle{ get{ return false; } }
		public override bool CanRummageCorpses{ get{ return true; } }
		public override bool AlwaysAttackable{ get{ return true; } }
		public override int Meat{ get{ return 1; } }
		public override int TreasureMapLevel{ get{ return Core.AOS ? 1 : 0; } }
		public override int Skeletal{ get{ return Utility.Random(3); } }
		public override SkeletalType SkeletalType{ get{ return SkeletalType.Brittle; } }

		public override void OnAfterSpawn()
		{
			Server.Misc.IntelligentAction.BeforeMyBirth( this );
			base.OnAfterSpawn();
		}

		public PossessedScholar( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 1 );

			writer.Write( m_NextSpellTime );
			writer.Write( m_IsChanneling );
			writer.Write( m_HasArcaneArmor );
			writer.Write( m_ArcaneArmorEnds );
			writer.Write( m_OriginalPhysicalResist );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();

			m_AcidFogTimers = new Dictionary<Point3D, Timer>();

			if ( version >= 1 )
			{
				m_NextSpellTime = reader.ReadDateTime();
				m_IsChanneling = reader.ReadBool();
				m_HasArcaneArmor = reader.ReadBool();
				m_ArcaneArmorEnds = reader.ReadDateTime();
				m_OriginalPhysicalResist = reader.ReadInt();

				if ( m_IsChanneling )
				{
					m_IsChanneling = false;
				}

				if ( m_HasArcaneArmor && DateTime.UtcNow >= m_ArcaneArmorEnds )
				{
					RemoveArcaneArmor();
				}
			}
		}
	}
}