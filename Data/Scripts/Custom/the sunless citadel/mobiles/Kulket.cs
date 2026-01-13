using System;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "Kulket's corpse" )]
	public class Kulket : BaseCreature
	{
		private Timer m_Timer;

		[Constructable]
		public Kulket() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "Kulket";
			Body = 80;
			Hue = Utility.RandomList( 0, 0xB79, 0xB19, 0xB0D, 0xACE, 0xACF, 0xAB0 );

			BaseSoundID = 0x26B;
			SetStr( 120 );
			SetDex( 45 );
			SetInt( 11, 20 );

			SetHits( 160 );
			SetMana( 0 );

			SetDamage( 8, 19 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 45 );
			SetResistance( ResistanceType.Fire, 30 );
			SetResistance( ResistanceType.Energy, 30 );

			SetSkill( SkillName.MagicResist, 50.0 );
			SetSkill( SkillName.Tactics, 60.0 );
			SetSkill( SkillName.FistFighting, 60.0 );

			Fame = 1250;
			Karma = -1250;

			VirtualArmor = 34;
			m_Timer = new Kulket.TeleportTimer( this, 0x1CC );
			m_Timer.Start();
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Poor );
		}

		public override int Hides{ get{ return 12; } }
		public override FoodType FavoriteFood{ get{ return FoodType.Fish | FoodType.Meat; } }

		public Kulket(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int) 1);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
			m_Timer = new Kulket.TeleportTimer( this, 0x1CC );
			m_Timer.Start();
		}

		public class TeleportTimer : Timer
		{
			private Mobile m_Owner;
			private int m_Sound;

			private static int[] m_Offsets = new int[]
			{
				-1, -1,
				-1,  0,
				-1,  1,
				0, -1,
				0,  1,
				1, -1,
				1,  0,
				1,  1
			};

			public TeleportTimer( Mobile owner, int sound ) : base( TimeSpan.FromSeconds( 5.0 ), TimeSpan.FromSeconds( 5.0 ) )
			{
				Priority = TimerPriority.TwoFiftyMS;

				m_Owner = owner;
				m_Sound = sound;
			}

			protected override void OnTick()
			{
				if ( m_Owner.Deleted )
				{
					Stop();
					return;
				}

				Map map = m_Owner.Map;

				if ( map == null )
					return;

				if ( 0.25 < Utility.RandomDouble() )
					return;

				if ( m_Owner.Combatant == null )
					return;

				Mobile toTeleport = null;

				foreach ( Mobile m in m_Owner.GetMobilesInRange( 10 ) )
				{
					if ( !((BaseCreature)m_Owner).Controlled && m != m_Owner && m.Player && m_Owner.InLOS( m ) && m_Owner.CanBeHarmful( m ) && m_Owner.CanSee( m ) && m.AccessLevel == AccessLevel.Player && ( (BaseCreature)m_Owner ).IsEnemy( m ) )
					{
						toTeleport = m;
						break;
					}
				}

				if ( toTeleport != null )
				{
					int offset = Utility.Random( 8 ) * 2;

					Point3D to = m_Owner.Location;

					for ( int i = 0; i < m_Offsets.Length; i += 2 )
					{
						int x = m_Owner.X + m_Offsets[(offset + i) % m_Offsets.Length];
						int y = m_Owner.Y + m_Offsets[(offset + i + 1) % m_Offsets.Length];

						if ( map.CanSpawnMobile( x, y, m_Owner.Z ) )
						{
							to = new Point3D( x, y, m_Owner.Z );
							break;
						}
						else
						{
							int z = map.GetAverageZ( x, y );

							if ( map.CanSpawnMobile( x, y, z ) )
							{
								to = new Point3D( x, y, z );
								break;
							}
						}
					}

					Mobile m = toTeleport;

					Point3D from = m.Location;

					m.Location = to;

					Server.Spells.SpellHelper.Turn( m_Owner, toTeleport );
					Server.Spells.SpellHelper.Turn( toTeleport, m_Owner );

					m.ProcessDelta();

					m.PlaySound( m_Sound );

					m_Owner.Combatant = toTeleport;

					BaseCreature.TeleportPets( m, m.Location, m.Map, false );
				}
			}
		}
	}
}