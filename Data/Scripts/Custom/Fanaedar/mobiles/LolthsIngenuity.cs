using System;
using Server.Items;
using Server.Targeting;
using System.Collections;
using Server.Network;

namespace Server.Mobiles
{
	[CorpseName( "a drider corpse" )]
	public class LolthsIngenuity : BaseCreature
	{
		private static Hashtable m_Table = new Hashtable();

		private class IngenuityEffect
		{
			public ResistanceMod EnergyMod;
			public ResistanceMod PoisonMod;
			public IngenuityTimer Timer;

			public IngenuityEffect( ResistanceMod energy, ResistanceMod poison, IngenuityTimer timer )
			{
				EnergyMod = energy;
				PoisonMod = poison;
				Timer = timer;
			}
		}

		[Constructable]
		public LolthsIngenuity() : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "Lolth's Ingenuity";
			Body = 693;
			BaseSoundID = 0x24D;
			Hue = 0x0b32;

			SetStr( 486, 510 );
			SetDex( 396, 420 );
			SetInt( 126, 145 );

			SetHits( 518, 632 );

			SetDamage( 12, 16 );

			SetDamageType( ResistanceType.Physical, 20 );
			SetDamageType( ResistanceType.Poison, 80 );

			SetResistance( ResistanceType.Physical, 55 );
			SetResistance( ResistanceType.Fire, 45 );
			SetResistance( ResistanceType.Cold, 45 );
			SetResistance( ResistanceType.Poison, 95 );
			SetResistance( ResistanceType.Energy, 45 );

			SetSkill( SkillName.Psychology, 95.0 );
			SetSkill( SkillName.Magery, 105.0 );
			SetSkill( SkillName.Meditation, 90.0 );
			SetSkill( SkillName.MagicResist, 110.0 );
			SetSkill( SkillName.Tactics, 115.0 );
			SetSkill( SkillName.FistFighting, 110.0, 120.0 );

			Fame = 16000;
			Karma = -16000;

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
				ApplyIngenuity( defender );
		}

		private class IngenuityTimer : Timer
		{
			private Mobile m_Mobile;
			private DateTime m_End;

			public IngenuityTimer( Mobile m, DateTime end ) : base( TimeSpan.Zero, TimeSpan.FromSeconds( 4.0 ) )
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

				int loss = (int)( m_Mobile.ManaMax * 0.035 );

				if ( loss < 1 )
					loss = 1;

				if ( m_Mobile.Mana >= loss )
					m_Mobile.Mana -= loss;
				else
					m_Mobile.Mana = 0;
			}
		}

		public static bool HasIngenuity( Mobile m )
		{
			return m_Table.ContainsKey( m );
		}

		public static void ApplyIngenuity( Mobile m )
		{
			if ( m == null || HasIngenuity( m ) )
				return;

			m.SendMessage( 38, "O toque de Lolth penetra fundo em sua alma!" );

			ResistanceMod energyMod = new ResistanceMod( ResistanceType.Energy, -12 );
			ResistanceMod poisonMod = new ResistanceMod( ResistanceType.Poison, -12 );

			m.AddResistanceMod( energyMod );
			m.AddResistanceMod( poisonMod );

			DateTime end = DateTime.Now + TimeSpan.FromSeconds( 32.0 );

			IngenuityTimer timer = new IngenuityTimer( m, end );

			IngenuityEffect effect = new IngenuityEffect( energyMod, poisonMod, timer );

			m_Table[m] = effect;

			timer.Start();
		}

		public static void StopEffect( Mobile m )
		{
			if ( m == null )
				return;

			IngenuityEffect effect = m_Table[m] as IngenuityEffect;

			if ( effect == null )
				return;

			if ( effect.Timer != null )
				effect.Timer.Stop();

			if ( effect.EnergyMod != null )
				m.RemoveResistanceMod( effect.EnergyMod );

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

		public LolthsIngenuity( Serial serial ) : base( serial )
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