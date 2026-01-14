using System;
using Server;
using Server.Items;
using Server.Custom.DailyBosses.System;

namespace Server.Mobiles
{
	[CorpseName( "a skeletal dragon corpse" )]
	public class SkeletalDragon : BaseCreature
	{
		private DateTime m_NextSpecialAttack = DateTime.MinValue;
		public override int BreathPhysicalDamage{ get{ return 20; } }
		public override int BreathFireDamage{ get{ return 20; } }
		public override int BreathColdDamage{ get{ return 20; } }
		public override int BreathPoisonDamage{ get{ return 20; } }
		public override int BreathEnergyDamage{ get{ return 20; } }
		public override int BreathEffectHue{ get{ return 0x9C1; } }
		public override int BreathEffectSound{ get{ return 0x653; } }
		public override int BreathEffectItemID{ get{ return 0x37BC; } }
		public override bool ReacquireOnMovement{ get{ return !Controlled; } }
		public override bool HasBreath{ get{ return true; } }
		public override double BreathEffectDelay{ get{ return 0.1; } }
		public override int GetBreathForm()
		{
		    return 25;
		}

		[Constructable]
		public SkeletalDragon () : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = NameList.RandomName( "dragon" );
			Title = "the skeletal dragon";
			switch ( Utility.RandomMinMax( 0, 2 ) )
			{
				case 0:	Title = "the skeletal dragon";	break;
				case 1:	Title = "the bone dragon";		break;
				case 2:	Title = "the dragon of bones";	break;
			}

			Body = 323;
			BaseSoundID = 0x488;

			SetStr( 898, 1030 );
			SetDex( 68, 200 );
			SetInt( 488, 620 );

			SetHits( 558, 599 );

			SetDamage( 29, 35 );

			SetDamageType( ResistanceType.Physical, 75 );
			SetDamageType( ResistanceType.Fire, 25 );

			SetResistance( ResistanceType.Physical, 75, 80 );
			SetResistance( ResistanceType.Fire, 40, 60 );
			SetResistance( ResistanceType.Cold, 40, 60 );
			SetResistance( ResistanceType.Poison, 70, 80 );
			SetResistance( ResistanceType.Energy, 40, 60 );

			SetSkill( SkillName.Psychology, 80.1, 100.0 );
			SetSkill( SkillName.Magery, 80.1, 100.0 );
			SetSkill( SkillName.MagicResist, 100.3, 130.0 );
			SetSkill( SkillName.Tactics, 97.6, 100.0 );
			SetSkill( SkillName.FistFighting, 97.6, 100.0 );

			Fame = 22500;
			Karma = -22500;

			VirtualArmor = 80;
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.FilthyRich, 4 );
			AddLoot( LootPack.Gems, 8 );
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
					if ( Utility.RandomMinMax( 1, 20 ) == 1 && killer.Skills[SkillName.Necromancy].Base >= 50 )
					{
						c.DropItem( new DracolichSkull() );
					}
				}
			}
		}

		public override void OnDamage( int amount, Mobile from, bool willKill )
		{
			if ( DateTime.UtcNow >= m_NextSpecialAttack )
			{
				PerformRageAttack( from );
				m_NextSpecialAttack = DateTime.UtcNow + TimeSpan.FromSeconds( 30 );
			}
			
			base.OnDamage( amount, from, willKill );
		}
		
		private void PerformRageAttack( Mobile target )
		{
			if ( target == null || target.Deleted || !target.Alive )
				return;

			int attackChoice = Utility.RandomMinMax( 1, 3 );
            Map map = this.Map;

			switch ( attackChoice  )
			{
				case 1:
                {
                    BossSpecialAttack.SummonHonorGuard(
                        boss: this,
                        target: target,
                        warcry: "*Rattles Bones into servitude*",
                        amount: 4,
                        creatureType: typeof(SkeletalKnight),
                        hue: 267
                    );
                    break;
                }
				case 2:
                {
                    BossSpecialAttack.PerformConeBreath(
					    boss: this,
					    target: target,
					    warcry: "*exhales devastating fumes!*",
					    hue: 267,
					    rage: 1,
					    range: 6, 
						physicalDmg:0,
						coldDmg:0,
						poisonDmg:100,
						energyDmg:0,
					    fireDmg: 0
					);
					break;
                }
				case 3:
				{
					BossSpecialAttack.PerformDegenAura(
		                this,
		                "*Channels the powers of undeath!*",
		                6,          // radius
		                2,     		// rage level
		                12,         // duration - 12 + rage*2 seconds, damage happens every 2 seconds 
		                12,         // intensity - 20 + rage damage per tick
		                "health",   // target attribute
		                267         // hue
		            );
                    break;
				}
			}
		}

		public override double BonusPetDamageScalar{ get{ return (Core.SE)? 3.0 : 1.0; } }
		public override bool AutoDispel{ get{ return true; } }
		public override Poison PoisonImmune{ get{ return Poison.Deadly; } }
		public override bool BleedImmune{ get{ return true; } }
		public override int Skeletal{ get{ return Utility.Random(10); } }
		public override SkeletalType SkeletalType{ get{ return SkeletalType.Draco; } }

		public SkeletalDragon( Serial serial ) : base( serial )
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