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
	[CorpseName( "Irael's corpse" )]
	public class IraelLolthsMorbidReverie : BaseCreature
	{
        private DateTime m_NextSpecialAttack;
		private List<MagmaTile> m_MagmaTiles = new List<MagmaTile>();
		private static Hashtable m_BurningSkinTable = new Hashtable();
		private const int MAGMA_DURATION = 12;
		
		[Constructable]
		public IraelLolthsMorbidReverie() : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.4, 0.8 )
		{
			Name = "Irael";
			Title = "Devaneio Mórbido de Lolth";
            Body = 0x9e;
			Hue = 0x0845;
			BaseSoundID = 466;

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

		private static void ApplyBurningSkin(Mobile m)
		{
		    if (m == null || m.Deleted)
		        return;

		    BurningSkinDebuff debuff = (BurningSkinDebuff)m_BurningSkinTable[m];

		    if (debuff != null)
		    {
		        debuff.Refresh();
		    }
		    else
		    {
		        debuff = new BurningSkinDebuff(m);
		        m_BurningSkinTable[m] = debuff;
		        debuff.Start();
		    }

		    m.SendMessage(33, "*your skin burns!*");
		}

		public override void OnGaveMeleeAttack( Mobile defender )
		{
			base.OnGaveMeleeAttack( defender );

			if ( 0.1 >= Utility.RandomDouble() )
				ApplyBurningSkin(defender);
		}

		public override void OnGotMeleeAttack( Mobile attacker )
		{
			base.OnGotMeleeAttack( attacker );

			if ( 0.1 >= Utility.RandomDouble() )
				ApplyBurningSkin(attacker);
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
					BossSpecialAttack.PerformTargettedAoE( this, target, 3, "Ardei pela senhora das Teias Demoníacas!", 0x0845, 20, 20, 20, 20, 20 );
					break;
				case 2:
					PerformMagmaEruption();
                	break;
			}
		}

		private void PerformMagmaEruption()
		{
			if ( Map == null )
				return;

			PublicOverheadMessage( MessageType.Regular, 0x21, false, "O chão treme com o Devaneio de Lolth!" );
			PlaySound( 0x307 );

			int tileCount = Utility.RandomMinMax( 12, 22 );
			List<Point3D> validLocations = new List<Point3D>();

			for ( int attempts = 0; attempts < tileCount * 3 && validLocations.Count < tileCount; attempts++ )
			{
				int range = Utility.RandomMinMax( 4, 16 );
				int xOffset = Utility.RandomMinMax( -range, range );
				int yOffset = Utility.RandomMinMax( -range, range );

				Point3D loc = new Point3D( X + xOffset, Y + yOffset, Z );

				if ( Map.CanFit( loc, 16, false, false ) && !IsLocationOccupied( loc ) )
				{
					validLocations.Add( loc );
				}
			}

			foreach ( Point3D loc in validLocations )
			{
				Timer.DelayCall( TimeSpan.FromSeconds( Utility.RandomDouble() * 0.5 ), delegate()
				{
					if ( Deleted || !Alive )
						return;

					Effects.SendLocationEffect( loc, Map, 0x36B0, 20, 10, 0x0845, 0 );
					Effects.PlaySound( loc, Map, 0x11D );

					Timer.DelayCall( TimeSpan.FromSeconds( 0.5 ), delegate()
					{
						MagmaTile magma = new MagmaTile( this, loc, Map );
						m_MagmaTiles.Add( magma );

						Timer.DelayCall( TimeSpan.FromSeconds( MAGMA_DURATION ), delegate()
						{
							if ( magma != null )
							{
								m_MagmaTiles.Remove( magma );
								magma.Delete();
							}
						});
					});
				});
			}
		}

		private bool IsLocationOccupied( Point3D loc )
		{
			IPooledEnumerable eable = Map.GetMobilesInRange( loc, 0 );
			bool occupied = false;

			foreach ( Mobile m in eable )
			{
				occupied = true;
				break;
			}

			eable.Free();
			return occupied;
		}

		public override void OnDelete()
		{
		    CleanupMagmaTiles();
		    base.OnDelete();
		}

		private void CleanupMagmaTiles()
		{
			for (int i = m_MagmaTiles.Count - 1; i >= 0; i--)
			{
				MagmaTile tile = m_MagmaTiles[i];

				if (tile != null && !tile.Deleted)
					tile.Delete();
			}
			m_MagmaTiles.Clear();
		}

		public override void OnDeath( Container c )
		{
			base.OnDeath( c );

			Mobile killer = this.LastKiller;
			if ( killer != null )
			{

				if ( killer is BaseCreature )
					killer = ((BaseCreature)killer).GetMaster();
                
				BossLootSystem.BossEnchant(this, c, 350, 100, 3, "DrowBard");
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
			this.MobileMagics(6, SpellType.Bard, 0x0845);
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.FilthyRich, 3 );
		}

		public override bool CanRummageCorpses{ get{ return false; } }
		public override Poison PoisonImmune{ get{ return Poison.Greater; } }

		public override int Feathers{ get{ return 200; } }
		public override int Skeletal{ get{ return Utility.Random(12); } }
		public override SkeletalType SkeletalType{ get{ return SkeletalType.Mystical; } }

		public override int Cloths{ get{ return 12; } }
		public override ClothType ClothType{ get{ return ClothType.Divine; } }
		public IraelLolthsMorbidReverie( Serial serial ) : base( serial )
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
			this.MobileMagics(6, SpellType.Bard, 0x0845);
		}

		private class BurningSkinDebuff : Timer
		{
		    private Mobile m_Mobile;
		    private DateTime m_End;
		    private ResistanceMod m_Mod;

		    public BurningSkinDebuff(Mobile m)
		        : base(TimeSpan.Zero, TimeSpan.FromSeconds(1.0))
		    {
		        m_Mobile = m;
		        m_End = DateTime.UtcNow + TimeSpan.FromSeconds(12);

		        m_Mod = new ResistanceMod(ResistanceType.Fire, -25);
		        Priority = TimerPriority.TwoFiftyMS;

		        if (m_Mobile != null)
		            m_Mobile.AddResistanceMod(m_Mod);
		    }

		    public void Refresh()
		    {
		        m_End = DateTime.UtcNow + TimeSpan.FromSeconds(12);
		    }

		    protected override void OnTick()
		    {
		        if (m_Mobile == null || m_Mobile.Deleted || !m_Mobile.Alive)
		        {
		            Remove();
		            Stop();
		            return;
		        }

		        if (DateTime.UtcNow >= m_End)
		        {
		            Remove();
		            Stop();
		        }
		    }

		    private void Remove()
		    {
		        if (m_Mobile != null)
		            m_Mobile.RemoveResistanceMod(m_Mod);

		        m_BurningSkinTable.Remove(m_Mobile);
		    }
		}
	}
}