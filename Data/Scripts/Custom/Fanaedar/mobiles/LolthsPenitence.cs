using System;
using Server.Items;
using Server.Targeting;
using System.Collections;
using Server.Network;

namespace Server.Mobiles
{
	[CorpseName( "a drider corpse" )]
	public class LolthsPenitence : BaseCreature
	{
		private static Hashtable m_Table = new Hashtable();

		private class PenitenceEffect
		{
			public ResistanceMod PhysMod;
			public ResistanceMod PoisonMod;
			public PenitenceTimer Timer;

			public PenitenceEffect( ResistanceMod phys, ResistanceMod poison, PenitenceTimer timer )
			{
				PhysMod = phys;
				PoisonMod = poison;
				Timer = timer;
			}
		}

		[Constructable]
		public LolthsPenitence() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "Lolth's Penitence";
			Body = 693;
			BaseSoundID = 0x24D;
			Hue = 0x0497;

			SetStr( 386, 410 );
			SetDex( 396, 420 );
			SetInt( 226, 245 );

			SetHits( 518, 632 );

			SetDamage( 15, 22 );

			SetDamageType( ResistanceType.Physical, 20 );
			SetDamageType( ResistanceType.Poison, 80 );

			SetResistance( ResistanceType.Physical, 45 );
			SetResistance( ResistanceType.Fire, 50 );
			SetResistance( ResistanceType.Cold, 45 );
			SetResistance( ResistanceType.Poison, 95 );
			SetResistance( ResistanceType.Energy, 50 );

			SetSkill( SkillName.MagicResist, 110.0 );
			SetSkill( SkillName.Tactics, 115.0 );
			SetSkill( SkillName.FistFighting, 110.0, 120.0 );

			Fame = 15000;
			Karma = -15000;

			VirtualArmor = 42;

			PackItem( new SpidersSilk( 8 ) );

			Item Venom = new VenomSack();
			Venom.Name = "lethal venom sack";
			AddItem( Venom );
		}

		public override void OnDeath( Container c )
		{
			base.OnDeath( c );
		
			if ( Utility.RandomDouble() <= 0.01 )
				c.DropItem( new OrbOfTheDemonwebPits() );
		}

		public override void OnGaveMeleeAttack( Mobile defender )
		{
			base.OnGaveMeleeAttack( defender );

			if ( defender == null || !defender.Player || !defender.Alive )
				return;

			if ( Utility.RandomDouble() <= 0.35 )
				ApplyPenitence( defender );
		}

		private class PenitenceTimer : Timer
		{
			private Mobile m_Mobile;
			private DateTime m_End;

			public PenitenceTimer( Mobile m, DateTime end ) : base( TimeSpan.Zero, TimeSpan.FromSeconds( 4.0 ) )
			{
				m_Mobile = m;
				m_End = end;
				Priority = TimerPriority.TwoFiftyMS;
			}

			protected override void OnTick()
			{
				if ( m_Mobile == null || m_Mobile.Deleted || !m_Mobile.Alive )
				{
					StopEffect( m_Mobile );
					return;
				}

				if ( DateTime.Now >= m_End )
				{
					StopEffect( m_Mobile );
					return;
				}

				int loss = (int)( m_Mobile.StamMax * 0.045 );

				if ( loss < 1 )
					loss = 1;

				if ( m_Mobile.Stam >= loss )
					m_Mobile.Stam -= loss;
				else
					m_Mobile.Stam = 0;
			}
		}

		public static bool HasPenitence( Mobile m )
		{
			return m_Table.ContainsKey( m );
		}

		public static void ApplyPenitence( Mobile m )
		{
			if ( m == null || HasPenitence( m ) )
				return;

			m.SendMessage( 38, "Os tentáculos de Lolth penetram fundo em sua alma!" );

			ResistanceMod physMod = new ResistanceMod( ResistanceType.Physical, -8 );
			ResistanceMod poisonMod = new ResistanceMod( ResistanceType.Poison, -16 );

			m.AddResistanceMod( physMod );
			m.AddResistanceMod( poisonMod );

			DateTime end = DateTime.Now + TimeSpan.FromSeconds( 32.0 );

			PenitenceTimer timer = new PenitenceTimer( m, end );

			PenitenceEffect effect = new PenitenceEffect( physMod, poisonMod, timer );

			m_Table[m] = effect;

			timer.Start();
		}

		public static void StopEffect( Mobile m )
		{
			if ( m == null )
				return;

			PenitenceEffect effect = m_Table[m] as PenitenceEffect;

			if ( effect == null )
				return;

			if ( effect.Timer != null )
				effect.Timer.Stop();

			if ( effect.PhysMod != null )
				m.RemoveResistanceMod( effect.PhysMod );

			if ( effect.PoisonMod != null )
				m.RemoveResistanceMod( effect.PoisonMod );

			m_Table.Remove( m );

			m.SendMessage( 68, "A influência de Lolth sobre sua alma desaparece." );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Rich, 3 );
		}

		public override Poison PoisonImmune{ get{ return Poison.Deadly; } }
		public override Poison HitPoison{ get{ return Poison.Deadly; } }
		public override int Skeletal{ get{ return Utility.Random( 2 ); } }
		public override SkeletalType SkeletalType{ get{ return SkeletalType.Drow; } }

		public LolthsPenitence( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int)0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
}