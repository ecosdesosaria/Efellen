using System;
using Server;
using Server.Items;
using Server.Misc;
using Server.CustomSpells;
using Server.Custom;
using Server.Targeting;
using System.Collections;


namespace Server.Mobiles
{
	[CorpseName( "a Scorched Angel corpse" )]
	public class ScorchedAngel : BaseCreature
	{
		private static Hashtable m_BurningSkinTable = new Hashtable();

		[Constructable]
		public ScorchedAngel () : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a Scorched Angel";
			Body = 0x9e;
			Hue = 2931;
			BaseSoundID = 466;

			SetStr( 476, 505 );
			SetDex( 76, 95 );
			SetInt( 301, 325 );

			SetHits( 286, 303 );

			SetDamage( 7, 14 );

			SetDamageType( ResistanceType.Physical, 50 );
			SetDamageType( ResistanceType.Fire, 50 );

			SetResistance( ResistanceType.Physical, 45, 60 );
			SetResistance( ResistanceType.Fire, 60, 80 );
			SetResistance( ResistanceType.Cold, 20, 30 );
			SetResistance( ResistanceType.Poison, 20, 30 );
			SetResistance( ResistanceType.Energy, 30, 40 );

			SetSkill( SkillName.Psychology, 70.1, 80.0 );
			SetSkill( SkillName.Magery, 70.1, 80.0 );
			SetSkill( SkillName.MagicResist, 85.1, 95.0 );
			SetSkill( SkillName.Tactics, 70.1, 80.0 );
			SetSkill( SkillName.FistFighting, 60.1, 80.0 );

			Fame = 15000;
			Karma = -15000;

			VirtualArmor = 58;
		}

		public override void OnGaveMeleeAttack(Mobile defender)
		{
		    base.OnGaveMeleeAttack(defender);

		    if (defender == null || defender.Deleted || !defender.Alive)
		        return;

		    if (Utility.Random(100) < 15)
		        ApplyBurningSkin(defender);
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



        public override void OnAfterSpawn()
		{
			this.MobileMagics(Utility.Random(3,6), SpellType.Cleric, 2931);
			base.OnAfterSpawn();
		}

        public override void OnDeath(Container c)
		{
		    base.OnDeath(c);
		    BossLootSystem.BossEnchant(this, c, 300, 15, 1, "scorched");
		}

		
		public override void GenerateLoot()
		{
			AddLoot( LootPack.Rich, 2 );
			AddLoot( LootPack.Average, 2 );
		}

		public override bool CanRummageCorpses{ get{ return false; } }
		public override Poison PoisonImmune{ get{ return Poison.Greater; } }
		public override int Feathers{ get{ return 100; } }
		public override int Skeletal{ get{ return Utility.Random(5); } }
		public override SkeletalType SkeletalType{ get{ return SkeletalType.Mystical; } }

		public override int Cloths{ get{ return 4; } }
		public override ClothType ClothType{ get{ return ClothType.Divine; } }

		public ScorchedAngel( Serial serial ) : base( serial )
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
            this.MobileMagics(Utility.Random(3,6), SpellType.Cleric, 2931);
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

		        m_Mod = new ResistanceMod(ResistanceType.Fire, -12);
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