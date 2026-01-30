using System;
using System.Collections;
using Server.Items;
using Server.Targeting;
using Server.Misc;
using Server.Custom.DailyBosses.System;
using Server.CustomSpells;
using Server.Custom;

namespace Server.Mobiles
{
	[CorpseName( "A fallen angel's corpse" )]
	public class FlameOfTheFall : BaseCreature
	{
        private DateTime m_NextSpecialAttack = DateTime.MinValue;
		private static Hashtable m_BurningSkinTable = new Hashtable();

		[Constructable]
		public FlameOfTheFall() : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.4, 0.8 )
		{
			Name = "Flame of the Fall";
			Body = 0x9e;
			BaseSoundID = 466;

			SetStr( 396, 470 );
			SetDex( 121, 160 );
			SetInt( 56, 90 );

			SetHits( 1558, 1722 );

			SetDamage( 16, 22 );

			SetDamageType( ResistanceType.Physical, 40 );
			SetDamageType( ResistanceType.Fire, 60 );

			SetResistance( ResistanceType.Physical, 45, 55 );
			SetResistance( ResistanceType.Fire, 60, 80 );
			SetResistance( ResistanceType.Cold, 10, 20 );
			SetResistance( ResistanceType.Poison, 20, 60 );
			SetResistance( ResistanceType.Energy, 20, 30 );
			SetSkill( SkillName.Psychology, 90.1, 110.0 );
			SetSkill( SkillName.Magery, 90.1, 110.0 );
			SetSkill( SkillName.MagicResist, 101.1, 105.0 );
			SetSkill( SkillName.Tactics, 90.0 );
			SetSkill( SkillName.FistFighting, 95.0 );

			Fame = 16500;
			Karma = -16500;

			VirtualArmor = 60;
		}

		public override void OnAfterSpawn()
		{
			this.MobileMagics(Utility.Random(4,7), SpellType.Cleric, 2931);
			base.OnAfterSpawn();
		}
		public override void OnGaveMeleeAttack(Mobile defender)
		{
		    base.OnGaveMeleeAttack(defender);

		    if (defender == null || defender.Deleted || !defender.Alive)
		        return;

		    if (Utility.Random(100) < 35)
		        ApplyBurningSkin(defender);
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
                        EtherealHiryu mount = new EtherealHiryu();
                        mount.Hue = 0x09D3;
                        c.DropItem( mount );
                    }
				}
				BossLootSystem.BossEnchant(this, c, 300, 100, 2, "scorched");
			}
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Rich, 2 );
			AddLoot( LootPack.Gems, 2 );
			AddLoot( LootPack.MedPotions, 2 );
		}

		public override int Hides{ get{ return 3; } }
		public override int Feathers{ get{ return 100; } }
		public override int Skeletal{ get{ return Utility.Random(8); } }
		public override SkeletalType SkeletalType{ get{ return SkeletalType.Mystical; } }

		public override int Cloths{ get{ return 8; } }
		public override ClothType ClothType{ get{ return ClothType.Divine; } }

        public override void OnDamage( int amount, Mobile from, bool willKill )
		{
			if ( DateTime.UtcNow >= m_NextSpecialAttack )
			{
				PerformRageAttack( from );
				m_NextSpecialAttack = DateTime.UtcNow + TimeSpan.FromSeconds( 25 );
			}
			
			base.OnDamage( amount, from, willKill );
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

		private void PerformRageAttack( Mobile target )
		{
			if ( target == null || target.Deleted || !target.Alive )
				return;

			int attackChoice = Utility.RandomMinMax( 1, 4 );
            Map map = this.Map;

			switch ( attackChoice  )
			{
				case 1: // fire burst
				{
					BossSpecialAttack.PerformTargettedAoE(
						this,
						target,
						1,
						"Rejoice, for the flame accepts all!",
						2931,  // hue
						0,     // physical
						100,   // fire
						0,     // cold
						0,     // poison
						0      // energy
					);
					break;
				}
				case 2: // X explosion
				{
					BossSpecialAttack.PerformCrossExplosion(
					    boss: this,
					    target: target,
					    warcry: "Embrace thy immolation!",
					    hue: 2931,
					    rage: 1,
					    coldDmg: 0,
					    fireDmg: 100,
					    energyDmg: 0,
					    poisonDmg: 0,
					    physicalDmg: 0
					);
                	break;
			    }
				case 3: // nova
				{
					BossSpecialAttack.PerformSlam(
                	    boss: this,
                	    warcry: "Fire cleanses all!",
                	    hue: 2931,
                	    rage: 2,
                	    range: 6,
                	    physicalDmg: 0,
						fireDmg: 100
                	);
                	break;
			    }
                case 4:
                {
                    BossSpecialAttack.SummonHonorGuard(
                        boss: this,
                        target: target,
                        warcry: "Join the burning chorus!",
                        amount: 3,
                        creatureType: typeof(ScorchedAngel),
                        hue: 2931
                    );
                    break;
                }
			}
		}

		public FlameOfTheFall( Serial serial ) : base( serial )
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
			this.MobileMagics(Utility.Random(4,7), SpellType.Cleric, 2931);
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

		        m_Mod = new ResistanceMod(ResistanceType.Fire, -18);
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