using System;
using System.Collections;
using Server.Items;
using Server.Targeting;
using Server.Misc;
using Server.Custom.DailyBosses.System;

namespace Server.Mobiles
{
	[CorpseName( "Vraax's corpse" )]
	public class Vraax : BaseCreature
	{
        private DateTime m_NextSpecialAttack = DateTime.MinValue;
		public override int BreathPhysicalDamage{ get{ return 0; } }
		public override int BreathFireDamage{ get{ return 0; } }
		public override int BreathColdDamage{ get{ return 0; } }
		public override int BreathPoisonDamage{ get{ return 100; } }
		public override int BreathEnergyDamage{ get{ return 0; } }
		public override int BreathEffectHue{ get{ return 0; } }
		public override int BreathEffectSound{ get{ return 0x5D2; } }
		public override int BreathEffectItemID{ get{ return 0x239F; } }
		public override bool HasBreath{ get{ return true; } }
		public override double BreathEffectDelay{ get{ return 0.1; } }
		public override int GetBreathForm()
		{
		    return 37;
		}
		public override Poison HitPoison{ get{ return Poison.Regular;}}

		public override WeaponAbility GetWeaponAbility()
		{
			return WeaponAbility.MortalStrike;
		}

		[Constructable]
		public Vraax() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.4, 0.8 )
		{
			Name = "Vraax";
			Title = "the Pale Embalmer";
            Hue = 0x09d3;
			Body = 601;
			BaseSoundID = 471;

			SetStr( 396, 470 );
			SetDex( 121, 160 );
			SetInt( 56, 90 );

			SetHits( 1358, 1422 );

			SetDamage( 16, 25 );

			SetDamageType( ResistanceType.Physical, 40 );
			SetDamageType( ResistanceType.Cold, 60 );

			SetResistance( ResistanceType.Physical, 45, 55 );
			SetResistance( ResistanceType.Fire, 10, 20 );
			SetResistance( ResistanceType.Cold, 50, 60 );
			SetResistance( ResistanceType.Poison, 20, 60 );
			SetResistance( ResistanceType.Energy, 20, 30 );
        	SetSkill( SkillName.Poisoning, 80.1, 100.0 );
			SetSkill( SkillName.MagicResist, 70.0 );
			SetSkill( SkillName.Tactics, 80.0 );
			SetSkill( SkillName.FistFighting, 90.0 );

			Fame = 9500;
			Karma = -9500;

			VirtualArmor = 60;

			PackItem( new Garlic( 10 ) );
			PackItem( new Bandage( 20 ) );
			PackItem( new MummyWrap( Utility.RandomMinMax(4,10) ) );

			int[] list = new int[]
				{
					0x1CF0, 0x1CEF, 0x1CEE, 0x1CED, 0x1CE9, 0x1DA0, 0x1DAE, // pieces
					0x1CEC, 0x1CE5, 0x1CE2, 0x1CDD, 0x1AE4, 0x1DA1, 0x1DA2, 0x1DA4, 0x1DAF, 0x1DB0, 0x1DB1, 0x1DB2, // limbs
					0x1CE8, 0x1CE0, 0x1D9F, 0x1DAD // torsos
				};

			PackItem( new BodyPart( Utility.RandomList( list ) ) );
			PackItem( new BodyPart( Utility.RandomList( list ) ) );
			PackItem( new BodyPart( Utility.RandomList( list ) ) );
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
                        EtherealBeetle mount = new EtherealBeetle();
                        mount.Hue = 0x09D3;
                        c.DropItem( mount );
                    }
                    if ( Utility.RandomMinMax( 1, 5 ) == 1 )
					{
						CanopicJar jar = new CanopicJar();
						c.DropItem( jar );
					}
					if ( GetPlayerInfo.LuckyKiller( killer.Luck ) && Server.Misc.IntelligentAction.FameBasedEvent( this ) )
					{
						LootChest MyChest = new LootChest( Server.Misc.IntelligentAction.FameBasedLevel( this ) );
						Server.Misc.ContainerFunctions.MakeTomb( MyChest, this, 1 );
						c.DropItem( MyChest );
					}
				}
			}
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Rich );
			AddLoot( LootPack.Gems );
			AddLoot( LootPack.MedPotions );
			AddLoot( LootPack.Rich );
			AddLoot( LootPack.Gems );
			AddLoot( LootPack.MedPotions );
		}

		public override bool BleedImmune{ get{ return true; } }
		public override Poison PoisonImmune{ get{ return Poison.Deadly; } }
		public override int Hides{ get{ return 3; } }
		public override HideType HideType{ get{ return HideType.Necrotic; } }
		public override int Cloths{ get{ return 10; } }
		public override ClothType ClothType{ get{ return ClothType.Haunted; } }

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
				case 1: // poison burst
				{
					BossSpecialAttack.PerformTargettedAoE(
						this,
						target,
						1,
						"Rejoice, for thy shall join our empire, forever!",
						267,  // hue
						0,     // physical
						0,   // fire
						0,     // cold
						100,     // poison
						0      // energy
					);
					break;
				}
				case 2: // X explosion
				{
					BossSpecialAttack.PerformCrossExplosion(
					    boss: this,
					    target: target,
					    warcry: "Thy organs shall be honored amongst our dead!",
					    hue: 267,
					    rage: 1,
					    coldDmg: 0,
					    fireDmg: 0,
					    energyDmg: 0,
					    poisonDmg: 50,
					    physicalDmg: 50
					);
                	break;
			    }
				case 3: // nova
				{
					BossSpecialAttack.PerformSlam(
                	    boss: this,
                	    warcry: "We are everlasting!",
                	    hue: 267,
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
                        warcry: "Awake, servants of the great Leoric!",
                        amount: 3,
                        creatureType: typeof(Mummy),
                        hue: 267
                    );
                    break;
                }
			}
		}

		public Vraax( Serial serial ) : base( serial )
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
		}
	}
}