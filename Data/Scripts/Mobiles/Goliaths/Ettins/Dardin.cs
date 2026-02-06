using System;
using Server;
using System.Collections;
using Server.Items;
using Server.Targeting;
using Server.Misc;
using Server.Custom.DailyBosses.System;

namespace Server.Mobiles
{
	[CorpseName( "an ettin corpse" )]
	public class Dardin : BaseCreature
	{
		private DateTime m_NextSpecialAttack = DateTime.MinValue;

		[Constructable]
		public Dardin() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "Dardin";
			Title = "the Lord of the Pit";
			Body = 732;
			BaseSoundID = 0x59D;
			Hue = 0x07DA;

			SetStr( 836, 985 );
			SetDex( 256, 285 );
			SetInt( 381, 405 );

			SetHits( 1622, 1651 );

			SetDamage( 18, 24 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 45, 55 );
			SetResistance( ResistanceType.Fire, 40, 50 );
			SetResistance( ResistanceType.Cold, 35, 45 );
			SetResistance( ResistanceType.Poison, 40, 50 );
			SetResistance( ResistanceType.Energy, 40, 50 );

			SetSkill( SkillName.MagicResist, 90.2, 120.0 );
			SetSkill( SkillName.Tactics, 100.1, 110.0 );
			SetSkill( SkillName.FistFighting, 100.1, 111.1 );

			Fame = 22500;
			Karma = -22500;

			VirtualArmor = 60;
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.FilthyRich, 2 );
			AddLoot( LootPack.Average );
			AddLoot( LootPack.MedScrolls );
		}

		public override void OnDamage( int amount, Mobile from, bool willKill )
		{
			if ( DateTime.UtcNow >= m_NextSpecialAttack )
			{
				PerformRageAttack( from );
				m_NextSpecialAttack = DateTime.UtcNow + TimeSpan.FromSeconds( 35 );
			}
			
			base.OnDamage( amount, from, willKill );
		}

		private void PerformRageAttack( Mobile target )
		{
			if ( target == null || target.Deleted || !target.Alive )
				return;

			int attackChoice = Utility.RandomMinMax( 1, 4 );
            Map map = this.Map;

			switch ( attackChoice  )
			{
				case 1: // phys burst
				{
					BossSpecialAttack.PerformTargettedAoE(
						this,
						target,
						1,
						"*Throws huge boulders*",
						0x07DA,  // hue
						100,     // physical
						0,   // fire
						0,     // cold
						0,     // poison
						0      // energy
					);
					break;
				}
				case 2: // pull mobiles close
				{
					BossSpecialAttack.PerformPull(
						this,
						"Get over here!",
						0x07DA,
						2,
						false
					);
                	break;
			    }
				case 3: // nova
				{
					BossSpecialAttack.PerformSlam(
                	    boss: this,
                	    warcry: "*Stomps the ground furiously*",
                	    hue: 0x07DA,
                	    rage: 2,
                	    range: 6,
                	    physicalDmg: 0,
						poisonDmg: 100
                	);
                	break;
			    }
                case 4:
                {
                    BossSpecialAttack.SummonHonorGuard(
                        boss: this,
                        target: target,
                        warcry: "Come, brethren! Slay this interloper!",
                        amount: 2,
                        creatureType: typeof(OgreLord),
                        hue: 0x07DA
                    );
                    break;
                }
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

				if ( killer is PlayerMobile )
				{
					if ( Utility.RandomMinMax( 1, 10 ) == 1 )
                    {
                        EtherealRidgeback mount = new EtherealRidgeback();
                        mount.Hue = 0x09D3;
                        c.DropItem( mount );
                    }
				}
			}
			if ( GetPlayerInfo.LuckyKiller( killer.Luck ) && Utility.RandomMinMax( 1, 5 ) == 1 )
			{
				BaseWeapon club = new Club();
				club.Name = "Dardin's Toothpick";
				club.Hue = 0xB78;
				club.SkillBonuses.SetValues( 0, SkillName.Bludgeoning, 10 );
				club.SkillBonuses.SetValues( 1, SkillName.Tactics, 10 );
				club.WeaponAttributes.ResistPhysicalBonus = 15;
				club.Attributes.WeaponDamage = 25;
				club.Attributes.AttackChance = 10;
				club.MinDamage = club.MinDamage + 3;
				club.MaxDamage = club.MaxDamage + 6;
				c.DropItem( club );
			}
		}

		public override bool CanRummageCorpses{ get{ return false; } }
		public override Poison PoisonImmune{ get{ return Poison.Greater; } }
		public override int TreasureMapLevel{ get{ return 5; } }
		public override int Meat{ get{ return 12; } }
		public override int Hides{ get{ return 34; } }
		public override HideType HideType{ get{ return HideType.Goliath; } }
		public override int Skeletal{ get{ return Utility.Random(12); } }
		public override SkeletalType SkeletalType{ get{ return SkeletalType.Colossal; } }

		public Dardin( Serial serial ) : base( serial )
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