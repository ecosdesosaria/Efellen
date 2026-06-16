using System;
using Server;
using System.Collections;
using System.Collections.Generic;
using Server.Items;
using Server.Targeting;
using Server.Misc;
using System.Net;
using Server.CustomSpells;
using Server.Custom;
using Server.EffectsUtil;
using Server.Network;
using Server.Mobiles;
using Server.Spells;
using Server.Custom.DailyBosses.System;
using Server.Custom.BossSystems;
using Server.Custom.Ascensions;

namespace Server.Mobiles
{
	[CorpseName( "Nathyrra's corpse" )]
	public class NathyrraLolthsErodingLove : BaseCreature
	{
        private DateTime m_NextSpecialAttack;

		private static Hashtable m_Table = new Hashtable();

		private class PenitenceEffect
		{
			public ResistanceMod PoisonMod;
			public PenitenceTimer Timer;

			public PenitenceEffect( ResistanceMod poison, PenitenceTimer timer )
			{
				PoisonMod = poison;
				Timer = timer;
			}
		}


		[Constructable]
		public NathyrraLolthsErodingLove() : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.4, 0.8 )
		{
			Name = "Nathyrra";
			Title = "O Amor Erosivo de Lolth";
            Body = 224;
			Hue = 0x0967;
			BaseSoundID = 278;

			SetStr( 596, 670 );
			SetDex( 221, 360 );
			SetInt( 356, 490 );
			SetHits( 7358, 7422 );
			SetDamage( 16, 25 );
			SetDamageType( ResistanceType.Energy, 100 );
			SetResistance( ResistanceType.Physical, 75 );
			SetResistance( ResistanceType.Fire, 40 );
			SetResistance( ResistanceType.Cold, 40 );
			SetResistance( ResistanceType.Poison, 40 );
			SetResistance( ResistanceType.Energy, 40 );
		
			SetSkill( SkillName.MagicResist, 110.0 );
			SetSkill( SkillName.Tactics, 100.0 );
			SetSkill( SkillName.FistFighting, 100.0 );
			SetSkill( SkillName.Magery, 111.0 );
			SetSkill( SkillName.Meditation, 121.0 );
            SetSkill( SkillName.Necromancy, 111.0 );

			Fame = 19500;
			Karma = -19500;
			VirtualArmor = 60;

			m_NextSpecialAttack = DateTime.MinValue;
        }

		
		public override void OnDamage( int amount, Mobile from, bool willKill )
		{
		
			if ( DateTime.UtcNow >= m_NextSpecialAttack )
			{
				PerformRageAttack( from );
				m_NextSpecialAttack = DateTime.UtcNow + TimeSpan.FromSeconds( 20 );
			}
			
			base.OnDamage( amount, from, willKill );
		}
		private void PerformRageAttack( Mobile target )
		{
			if ( target == null || target.Deleted || !target.Alive )
				return;

			int attackChoice = Utility.RandomMinMax( 1, 2 );

			switch ( attackChoice )
			{
				case 1:
					BossSpecialAttack.PerformTargettedAoE( this, target, 3, "A paixão de Lolth vos libertará!", 0x9C4, 20, 20, 20, 20, 20 );
					break;
				case 2:
					BossSpecialAttack.SummonHonorGuard(
                        boss: this,
                        target: target,
                        warcry: "Vinde a mim as criancinhas!",
                        amount: 4,
                        creatureType: typeof(LolthsJealousy),
                        hue: 0x09d3
                    );
                    break;
			}
		}

		public override void OnDeath( Container c )
		{
			base.OnDeath( c );

			Mobile killer = this.LastKiller;
			if ( killer != null )
			{

				if ( killer is BaseCreature )
					killer = ((BaseCreature)killer).GetMaster();
                
				BossLootSystem.BossEnchant(this, c, 350, 100, 3, "DrowMage");
				for ( int i = 0; i < 2; i++ )
				{
					c.DropItem( Loot.RandomArty() );
					c.DropItem( new EtherealPowerScroll() );
					c.DropItem( AscensionScrollFactory.CreateRandom());
				}

				if ( killer is PlayerMobile && Utility.RandomDouble() <= 0.10 )
				{
					c.DropItem( new OrbOfTheDemonwebPits() );
				}
			}
		}

		public override void OnAfterSpawn()
		{
			base.OnAfterSpawn();
			this.MobileMagics(9, SpellType.Wizard, 0x9C4);
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.FilthyRich, 3 );
		}

		public override bool CanRummageCorpses{ get{ return false; } }
		public override Poison PoisonImmune{ get{ return Poison.Deadly; } }
		public override Poison HitPoison{ get{ return Poison.Deadly; } }
		public override int Skeletal{ get{ return Utility.Random(9); } }
		public override SkeletalType SkeletalType{ get{ return SkeletalType.Drow; } }

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

				int loss = (int)( m_Mobile.StamMax * 0.095 );

				if ( loss < 1 )
					loss = 1;

				if ( m_Mobile.Mana >= loss )
					m_Mobile.Mana -= loss;
				else
					m_Mobile.Stam = 0;
				
				if ( m_Mobile.Hits >= loss )
					m_Mobile.Hits -= loss;
				else
					m_Mobile.Hits = 1;
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

			ResistanceMod poisonMod = new ResistanceMod( ResistanceType.Poison, -25 );

			m.AddResistanceMod( poisonMod );

			DateTime end = DateTime.Now + TimeSpan.FromSeconds( 32.0 );

			PenitenceTimer timer = new PenitenceTimer( m, end );

			PenitenceEffect effect = new PenitenceEffect( poisonMod, timer );

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

	
			if ( effect.PoisonMod != null )
				m.RemoveResistanceMod( effect.PoisonMod );

			m_Table.Remove( m );

			m.SendMessage( 68, "A influência de Lolth sobre sua alma desaparece." );
		}

		public NathyrraLolthsErodingLove( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 1 );
            writer.Write( m_NextSpecialAttack );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();

            if ( version >= 1 )
			{
				m_NextSpecialAttack = reader.ReadDateTime();
			}
			this.MobileMagics(9, SpellType.Wizard, 0x9C4);
		}
	}
}