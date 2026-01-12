using System;
using Server;
using System.Collections;
using Server.Items;
using Server.Targeting;
using Server.Misc;
namespace Server.Mobiles
{
	[CorpseName( "a Remorhaz corpse" )]
	public class Remorhaz : BaseCreature
	{
		public override WeaponAbility GetWeaponAbility()
		{
			return WeaponAbility.Dismount;
		}
		public override int BreathPhysicalDamage{ get{ return 0; } }
		public override int BreathFireDamage{ get{ return 0; } }
		public override int BreathColdDamage{ get{ return 100; } }
		public override int BreathPoisonDamage{ get{ return 0; } }
		public override int BreathEnergyDamage{ get{ return 0; } }
		public override int BreathEffectHue{ get{ return 0x481; } }
		public override int BreathEffectSound{ get{ return 0x64F; } }
		public override bool ReacquireOnMovement{ get{ return !Controlled; } }
		public override bool HasBreath{ get{ return true; } }
		public override int GetBreathForm()
		{
		    return 12;
		}
		[Constructable]
		public Remorhaz() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a Remorhaz";
			Body = 0xC5;
			SetStr( 796, 825 );
			SetDex( 126, 155 );
			SetInt( 136, 175 );
			SetHits( 478, 495 );
			SetDamage( 14, 26 );
			SetDamageType( ResistanceType.Physical, 40 );
			SetDamageType( ResistanceType.Cold, 30 );
			SetDamageType( ResistanceType.Fire, 30 );
			SetResistance( ResistanceType.Physical, 25, 35 );
			SetResistance( ResistanceType.Cold, 80, 90 );
			SetResistance( ResistanceType.Fire, 65, 85 );
			SetResistance( ResistanceType.Poison, 20, 30 );
			SetResistance( ResistanceType.Energy, 20, 30 );
			SetSkill( SkillName.MagicResist, 80.2, 110.0 );
			SetSkill( SkillName.Tactics, 60.1, 80.0 );
			SetSkill( SkillName.FistFighting, 40.1, 50.0 );
			Fame = 15000;
			Karma = -15000;
			VirtualArmor = 40;
			AddItem( new LighterSource() );
		}
		
		public override void OnGaveMeleeAttack( Mobile defender )
		{
			base.OnGaveMeleeAttack( defender );
			
			if ( defender != null && defender.Alive && Utility.RandomDouble() < 0.15 ) 
			{
				defender.SendMessage( "You have been set on fire!" );
				defender.FixedParticles( 0x3709, 10, 30, 5052, EffectLayer.LeftFoot );
				defender.PlaySound( 0x208 );
				
				BeginBurn( defender );
			}
		}
		
		private static Hashtable m_BurnTimers = new Hashtable();
		
		private void BeginBurn( Mobile m )
		{
			Timer t = (Timer)m_BurnTimers[m];
			
			if ( t != null )
			{
				t.Stop();
				m_BurnTimers.Remove( m );
			}
			
			t = new BurnTimer( m, this );
			m_BurnTimers[m] = t;
			t.Start();
		}
		
		private class BurnTimer : Timer
		{
			private Mobile m_Mobile;
			private Mobile m_From;
			private int m_Count;
			private const int m_MaxCount = 4;
			
			public BurnTimer( Mobile m, Mobile from ) : base( TimeSpan.FromSeconds( 2.0 ), TimeSpan.FromSeconds( 2.0 ) )
			{
				m_Mobile = m;
				m_From = from;
				m_Count = 0;
				Priority = TimerPriority.TwoFiftyMS;
			}
			
			protected override void OnTick()
			{
				if ( m_Mobile == null || !m_Mobile.Alive || m_Mobile.Deleted )
				{
					Stop();
					m_BurnTimers.Remove( m_Mobile );
					return;
				}
				
				m_Count++;
				
				int damage = Utility.RandomMinMax( 15, 35 );
				
				AOS.Damage( m_Mobile, m_From, damage, 0, 100, 0, 0, 0 );
				m_Mobile.FixedParticles( 0x3709, 10, 30, 5052, EffectLayer.LeftFoot );
				
				if ( m_Count >= m_MaxCount )
				{
					Stop();
					m_BurnTimers.Remove( m_Mobile );
				}
			}
		}
		
		public override void GenerateLoot()
		{
			AddLoot( LootPack.FilthyRich );
			AddLoot( LootPack.Average );
			AddLoot( LootPack.Average );
		}
		public override int Meat{ get{ return 4; } }
		public override int TreasureMapLevel{ get{ return 3; } }
		public override int Hides{ get{ return 18; } }
		public override HideType HideType{ get{ return HideType.Frozen; } }
		public override int Skeletal{ get{ return Utility.Random(8); } }
		public override SkeletalType SkeletalType{ get{ return SkeletalType.Shark; } }
		public Remorhaz( Serial serial ) : base( serial )
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