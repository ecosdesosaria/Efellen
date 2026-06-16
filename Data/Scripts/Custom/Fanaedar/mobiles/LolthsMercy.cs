using System;
using Server.Items;
using Server.Targeting;
using System.Collections;
using Server.Network;

namespace Server.Mobiles
{
	[CorpseName( "a drider corpse" )]
	public class LolthsMercy : BaseCreature
	{
		private static Hashtable m_Table = new Hashtable();

		private class MercyEffect
		{
			public ResistanceMod PhysMod;
			public ResistanceMod PoisonMod;
			public MercyTimer Timer;

			public MercyEffect( ResistanceMod phys, ResistanceMod poison, MercyTimer timer )
			{
				PhysMod = phys;
				PoisonMod = poison;
				Timer = timer;
			}
		}

		[Constructable]
		public LolthsMercy() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "Lolth's Mercy";
			Body = 693;
			BaseSoundID = 0x24D;
			Hue = 0x08e3;

			SetStr( 586, 610 );
			SetDex( 296, 320 );
			SetInt( 126, 145 );

			SetHits( 518, 632 );

			SetDamage( 22, 27 );

			SetDamageType( ResistanceType.Physical, 20 );
			SetDamageType( ResistanceType.Poison, 80 );

			SetResistance( ResistanceType.Physical, 65 );
			SetResistance( ResistanceType.Fire, 45 );
			SetResistance( ResistanceType.Cold, 45 );
			SetResistance( ResistanceType.Poison, 90 );
			SetResistance( ResistanceType.Energy, 40 );

			SetSkill( SkillName.MagicResist, 105.0 );
			SetSkill( SkillName.Tactics, 115.0 );
			SetSkill( SkillName.FistFighting, 115.0, 125.0 );

			Fame = 15500;
			Karma = -15500;

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
				ApplyMercy( defender );
		}

		private class MercyTimer : Timer
		{
			private Mobile m_Mobile;
			private DateTime m_End;

			public MercyTimer( Mobile m, DateTime end ) : base( TimeSpan.Zero, TimeSpan.FromSeconds( 4.0 ) )
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

				int loss = (int)( m_Mobile.Hits * 0.035 );

				if ( loss < 1 )
					loss = 1;

				if ( m_Mobile.Hits >= loss )
					m_Mobile.Hits -= loss;
				else
					m_Mobile.Hits = 0;
			}
		}

		public static bool HasMercy( Mobile m )
		{
			return m_Table.ContainsKey( m );
		}

		public static void ApplyMercy( Mobile m )
		{
			if ( m == null || HasMercy( m ) )
				return;

			m.SendMessage( 38, "A vileza de Lolth penetra fundo em sua alma!" );

			ResistanceMod physMod = new ResistanceMod( ResistanceType.Physical, -14 );
			ResistanceMod poisonMod = new ResistanceMod( ResistanceType.Poison, -10 );

			m.AddResistanceMod( physMod );
			m.AddResistanceMod( poisonMod );

			DateTime end = DateTime.Now + TimeSpan.FromSeconds( 32.0 );

			MercyTimer timer = new MercyTimer( m, end );

			MercyEffect effect = new MercyEffect( physMod, poisonMod, timer );

			m_Table[m] = effect;

			timer.Start();
		}

		public static void StopEffect( Mobile m )
		{
			if ( m == null )
				return;

			MercyEffect effect = m_Table[m] as MercyEffect;

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

		public LolthsMercy( Serial serial ) : base( serial )
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