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
	[CorpseName( "Hellhammer's corpse" )]
	public class HellHammer : BaseCreature
	{
        private DateTime m_NextSpecialAttack = DateTime.MinValue;
		
		[Constructable]
		public HellHammer() : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.4, 0.8 )
		{
			Name = "Hell Hammer";
			Body = 0x58;
			Hue = 2931;
			BaseSoundID = 357;

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
			this.MobileMagics(Utility.Random(4,7), SpellType.Wizard, 2931);
			base.OnAfterSpawn();
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
                        EtherealReptalon mount = new EtherealReptalon();
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

		public override int Skeletal{ get{ return Utility.Random(8); } }
		public override SkeletalType SkeletalType{ get{ return SkeletalType.Devil; } }
		public override int Hides{ get{ return 12; } }
		public override HideType HideType{ get{ return HideType.Hellish; } }
		public override int Skin{ get{ return Utility.Random(4); } }
		public override SkinType SkinType{ get{ return SkinType.Demon; } }

        public override void OnDamage( int amount, Mobile from, bool willKill )
		{
			if ( DateTime.UtcNow >= m_NextSpecialAttack )
			{
				PerformRageAttack( from );
				m_NextSpecialAttack = DateTime.UtcNow + TimeSpan.FromSeconds( 25 );
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
				case 1: // fire burst
				{
					BossSpecialAttack.PerformTargettedAoE(
						this,
						target,
						1,
						"The legion shall feast on your ashes!",
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
					BossSpecialAttack.PerformConeBreath(
	    			    boss: this,
	    			    target: target,
	    			    warcry: "*exhales devastating flames!*",
	    			    hue: 2931,
	    			    rage: 3,
	    			    range: 5,
	    				physicalDmg:0,
	    			    fireDmg: 100
	    			);	
                    break;
			    }
				case 3: // nova
				{
					BossSpecialAttack.PerformSlam(
                	    boss: this,
                	    warcry: "Fire never lies!",
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
                    BossSpecialAttack.PerformDelayedExplosion(
				        this,
				        "*conjures flaming runes*",
				        0x497,   // hue
				        12,     // radius
				        3,
				        0,      // physical
				        0,      // fire
				        0,      // cold
				        100,    // poison
				        0       // energy
				    );
                    break;
                }
			}
		}

		public HellHammer( Serial serial ) : base( serial )
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
			this.MobileMagics(Utility.Random(4,7), SpellType.Wizard, 2931);
		}
	}
}