using System;
using System.Collections.Generic;
using Server;
using Server.Misc;
using Server.Items;
using Server.Network;
using Server.Spells;

namespace Server.Mobiles 
{ 
	[CorpseName( "Belak's corpse" )] 
	public class BelakTheOutcast : BaseCreature 
	{ 
		private DateTime m_NextAbility;
		private static readonly TimeSpan AbilityCooldown = TimeSpan.FromSeconds(45);

		public override int BreathPhysicalDamage{ get{ return 0; } }
		public override int BreathFireDamage{ get{ if ( YellHue < 2 ){ return 100; } else { return 0; } } }
		public override int BreathColdDamage{ get{ if ( YellHue == 3 ){ return 100; } else { return 0; } } }
		public override int BreathPoisonDamage{ get{ if ( YellHue == 2 ){ return 100; } else { return 0; } } }
		public override int BreathEnergyDamage{ get{ return 0; } }
		public override int BreathEffectHue{ get{ if ( YellHue == 1 ){ return 0x488; } else if ( YellHue == 2 ){ return 0xB92; } else if ( YellHue == 3 ){ return 0x5B5; } else { return 0x4FD; } } }
		public override int BreathEffectSound{ get{ return 0x238; } }
		public override int BreathEffectItemID{ get{ return 0x1005; } } // EXPLOSION POTION
		public override bool HasBreath{ get{ return true; } }
		public override double BreathEffectDelay{ get{ return 0.1; } }
		public override int GetBreathForm()
		{
		    return 3;
		}
		public override double BreathDamageScalar{ get{ return 0.4; } }

		[Constructable] 
		public BelakTheOutcast() : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 ) 
		{
			Body = 0x190; 
			Name = "Belak";
			Title = "The Outcast";
			Utility.AssignRandomHair( this );
			int HairColor = Utility.RandomHairHue();
			FacialHairItemID = Utility.RandomList( 0, 8254, 8255, 8256, 8257, 8267, 8268, 8269 );
			HairHue = HairColor;
			FacialHairHue = HairColor;
			Hue = Utility.RandomSkinColor();
			EmoteHue = 11;
            Server.Misc.IntelligentAction.DressUpWizards( this, false );

			SetStr( 175 );
			SetDex( 125 );
			SetInt( 220 );
			SetHits( 433 );

			SetDamage( 8, 14 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 40 );
			SetResistance( ResistanceType.Fire, 30 );
			SetResistance( ResistanceType.Cold, 40 );
			SetResistance( ResistanceType.Poison, 45 );
			SetResistance( ResistanceType.Energy, 40 );

			SetSkill( SkillName.Psychology, 90.0 );
			SetSkill( SkillName.Magery, 95.1, 100.0 );
			SetSkill( SkillName.Meditation, 55.0 );
			SetSkill( SkillName.MagicResist, 80.0 );
			SetSkill( SkillName.Tactics, 77.5 );
			SetSkill( SkillName.FistFighting, 60.0 );
			SetSkill( SkillName.Swords, 60.0 );
			SetSkill( SkillName.Bludgeoning, 60.0 );
			SetSkill( SkillName.Poisoning, 40.1, 60.0 );

			Fame = 12500;
			Karma = -12500;

			VirtualArmor = 40;
			PackReg( Utility.RandomMinMax( 4, 12 ) );
			PackReg( Utility.RandomMinMax( 4, 12 ) );
			PackReg( Utility.RandomMinMax( 4, 12 ) );
			PackItem(new BoneHarvester( ) );

			m_NextAbility = DateTime.UtcNow;
		}

		public override void OnThink()
		{
			base.OnThink();

			if ( Combatant != null && DateTime.UtcNow >= m_NextAbility && Alive )
			{
				double healthPercent = (double)Hits / (double)HitsMax;

				if ( healthPercent > 0.80 )
					CastEntangle();
				else if ( healthPercent > 0.40 )
					CastSummonNatureAlly();
				else
					CastSpikeGrowth();

				m_NextAbility = DateTime.UtcNow + AbilityCooldown;
			}
		}

		private void CastEntangle()
		{
			if ( Combatant == null || !Combatant.Alive )
				return;

			PublicOverheadMessage( MessageType.Regular, 0x3B2, false, "*casts Entangle*" );

			Mobile target = Combatant;
			
			for ( int x = -4; x <= 4; x++ )
			{
				for ( int y = -4; y <= 4; y++ )
				{
					Point3D loc = new Point3D( target.X + x, target.Y + y, target.Z );
					if ( Utility.RandomDouble() < 0.5 )
					{
						Effects.SendLocationEffect( loc, target.Map, 0x3735, 20, 10, 0x47E, 0 ); 
					}
				}
			}

			IPooledEnumerable eable = target.GetMobilesInRange( 4 );
			foreach ( Mobile m in eable )
			{
				if ( m != this && m.Alive && CanBeHarmful( m ) && !m.Player == false || m.Player )
				{
					double magicResist = m.Skills[SkillName.MagicResist].Value;
					double duration = 12.0 - ( magicResist / 100.0 * 6.0 ); 
					
					if ( duration < 6.0 )
						duration = 6.0;

					m.Paralyze( TimeSpan.FromSeconds( duration ) );
					m.FixedEffect( 0x376A, 9, 32 );
					m.PlaySound( 0x204 );
				}
			}
			eable.Free();
		}

		private void CastSummonNatureAlly()
		{
			PublicOverheadMessage( MessageType.Regular, 0x3B2, false, "*casts Summon Nature Ally*" );

			int numToads = Utility.RandomMinMax( 2, 3 );

			for ( int i = 0; i < numToads; i++ )
			{
				Point3D spawnLoc = GetSpawnLocation();
				
				if ( spawnLoc != Point3D.Zero )
				{
					Effects.SendLocationEffect( spawnLoc, Map, 0x3709, 20, 10, 0x47E, 0 );
					Effects.PlaySound( spawnLoc, Map, 0x208 );

					GiantToad toad = new GiantToad();
					toad.MoveToWorld( spawnLoc, Map );
					if ( Combatant != null )
						toad.Combatant = Combatant;
				}
			}
		}

		private Point3D GetSpawnLocation()
		{
			for ( int i = 0; i < 20; i++ )
			{
				int distance = Utility.RandomMinMax( 2, 5 );
				int x = X + Utility.RandomMinMax( -distance, distance );
				int y = Y + Utility.RandomMinMax( -distance, distance );
				int z = Map.GetAverageZ( x, y );

				Point3D p = new Point3D( x, y, z );

				if ( Map.CanSpawnMobile( p ) && InRange( p, 5 ) )
					return p;
			}

			return Point3D.Zero;
		}

		private void CastSpikeGrowth()
		{
			if ( Combatant == null || !Combatant.Alive )
				return;

			PublicOverheadMessage( MessageType.Regular, 0x3B2, false, "*casts Spike Growth*" );

			Mobile target = Combatant;

			for ( int x = -5; x <= 5; x++ )
			{
				for ( int y = -5; y <= 5; y++ )
				{
					Point3D loc = new Point3D( target.X + x, target.Y + y, target.Z );
					if ( Utility.RandomDouble() < 0.25 )
					{
						int effectID = Utility.RandomList( 0x3735, 0x3709, 0x3728 );
						Effects.SendLocationEffect( loc, target.Map, effectID, 20, 10, Utility.RandomList( 0x47E, 0x3F, 0x55 ), 0 );
					}
				}
			}

			Effects.PlaySound( target.Location, target.Map, 0x22F );

			IPooledEnumerable eable = target.GetMobilesInRange( 5 );
			foreach ( Mobile m in eable )
			{
				if ( m != this && m.Alive && CanBeHarmful( m ) && m.Player )
				{
					DoHarmful( m );
					
					int damage = Utility.RandomMinMax( 20, 35 );
					AOS.Damage( m, this, damage, 100, 0, 0, 0, 0 );

					m.FixedEffect( 0x36BD, 20, 10, 0x26, 0 );

					int effect = Utility.Random( 100 );
					
					if ( effect < 30 ) 
					{
						m.ApplyPoison( this, Poison.Greater );
					}
					else if ( effect < 60 )
					{
						ApplyBleed( m );
					}
				}
			}
			eable.Free();
		}

		private void ApplyBleed( Mobile m )
		{
			new BleedTimer( m, this ).Start();
		}

		private class BleedTimer : Timer
		{
			private Mobile m_Mobile;
			private Mobile m_From;
			private int m_Ticks;

			public BleedTimer( Mobile m, Mobile from ) : base( TimeSpan.FromSeconds( 2.0 ), TimeSpan.FromSeconds( 2.0 ) )
			{
				m_Mobile = m;
				m_From = from;
				m_Ticks = 0;
			}

			protected override void OnTick()
			{
				if ( !m_Mobile.Alive || m_Ticks >= 5 )
				{
					Stop();
					return;
				}

				int damage = Utility.RandomMinMax( 5, 10 );
				AOS.Damage( m_Mobile, m_From, damage, 100, 0, 0, 0, 0 );
				
				m_Mobile.FixedParticles( 0x377A, 1, 15, 9502, 97, 3, EffectLayer.Waist );
				
				m_Ticks++;
			}
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Average );
            AddLoot( LootPack.Average );
			AddLoot( LootPack.Meager );
			AddLoot( LootPack.MedScrolls, 2 );
			AddLoot( LootPack.MedPotions );
		}

		public override void OnDeath( Container c )
		{
			base.OnDeath( c );

			Say( "I was so...close" );

			Mobile killer = this.LastKiller;
			if ( killer != null )
			{
				if ( killer is BaseCreature )
					killer = ((BaseCreature)killer).GetMaster();

				if ( killer is PlayerMobile )
				{
					if ( GetPlayerInfo.LuckyKiller( killer.Luck ) && Utility.RandomMinMax( 1, 3 ) == 1 )
					{
						BaseWeapon sickle = new BoneHarvester();
						sickle.Name = "Masterwork Sickle";
						sickle.SkillBonuses.SetValues( 0, SkillName.Druidism, 10 );
						sickle.SkillBonuses.SetValues( 1, SkillName.Poisoning, 10 );
						sickle.WeaponAttributes.ResistPoisonBonus = 15;
						sickle.Attributes.WeaponDamage = 20;
						sickle.Attributes.AttackChance = 10;
						sickle.AccuracyLevel = WeaponAccuracyLevel.Supremely;
						sickle.MinDamage = sickle.MinDamage + 3;
						sickle.MaxDamage = sickle.MaxDamage + 5;
						sickle.AosElementDamages.Poison = 50;
						sickle.AosElementDamages.Physical = 50;
						c.DropItem( sickle );
					}
				}
			}
		}

		public override bool ClickTitle{ get{ return false; } }
		public override bool ShowFameTitle{ get{ return false; } }
		public override bool CanRummageCorpses{ get{ return false; } }
		public override bool AlwaysAttackable{ get{ return true; } }
		public override int Meat{ get{ return 1; } }
		public override int TreasureMapLevel{ get{ return Core.AOS ? 2 : 0; } }
		public override int Skeletal{ get{ return Utility.Random(3); } }
		public override SkeletalType SkeletalType{ get{ return SkeletalType.Brittle; } }

		public BelakTheOutcast( Serial serial ) : base( serial ) 
		{ 
		} 

		public override void Serialize( GenericWriter writer ) 
		{ 
			base.Serialize( writer ); 
			writer.Write( (int) 0 );
			writer.Write( m_NextAbility );
		} 

		public override void Deserialize( GenericReader reader ) 
		{ 
			base.Deserialize( reader ); 
			int version = reader.ReadInt();
			m_NextAbility = reader.ReadDateTime();
		} 
	} 
}