using System;
using System.Collections;
using Server.Items;
using Server.Targeting;
using Server.Custom.DailyBosses.System;

namespace Server.Mobiles
{
	[CorpseName( "an antaur corpse" )] // TODO: Corpse name?
	public class AntaurKing : BaseCreature
	{
		private DateTime m_NextSpecialAttack = DateTime.MinValue;
		[Constructable]
		public AntaurKing() : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "Xthizx";
			Title = "the antaur king";
			Body = 784;
			BaseSoundID = 959;

			SetStr( 796, 825 );
			SetDex( 86, 105 );
			SetInt( 436, 475 );

			SetHits( 478, 495 );

			SetDamage( 16, 22 );

			SetDamageType( ResistanceType.Physical, 50 );
			SetDamageType( ResistanceType.Poison, 50 );

			SetResistance( ResistanceType.Physical, 75 );
			SetResistance( ResistanceType.Fire, 60, 70 );
			SetResistance( ResistanceType.Cold, 35 );
			SetResistance( ResistanceType.Poison, 50 );
			SetResistance( ResistanceType.Energy, 35 );

			SetSkill( SkillName.Psychology, 50.0 );
			SetSkill( SkillName.Magery, 70.0 );
			SetSkill( SkillName.MagicResist, 99.1, 100.0 );
			SetSkill( SkillName.Tactics, 97.6, 100.0 );
			SetSkill( SkillName.FistFighting, 90.1, 92.5 );

			Fame = 15000;
			Karma = -15000;

			VirtualArmor = 60;

			Item Venom = new VenomSack();
				Venom.Name = "deadly venom sack";
				AddItem( Venom );
		}

		public override void OnDamage( int amount, Mobile from, bool willKill )
		{
			if ( DateTime.UtcNow >= m_NextSpecialAttack )
			{
				PerformRageAttack( from );
				m_NextSpecialAttack = DateTime.UtcNow + TimeSpan.FromSeconds( 45 );
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
					BossSpecialAttack.PerformTargettedAoE(
						this,
						target,
						1,
						"*Screeches violently*",
						267,  // hue
						0,     // physical
						0,   // fire
						0,     // cold
						100,     // poison
						0      // energy
					);
					break;
				}
				case 2: 
				{
					BossSpecialAttack.PerformCrossExplosion(
					    boss: this,
					    target: target,
                	    warcry: "*Screeches violently*",
					    hue: 267,
					    rage: 2,
					    coldDmg: 0,
					    fireDmg: 0,
					    energyDmg: 0,
					    poisonDmg: 100,
					    physicalDmg: 0
					);
                	break;
			    }
				case 3: 
				{
					BossSpecialAttack.PerformSlam(
                	    boss: this,
                	    warcry: "*Screeches violently*",
                	    hue: 267,
                	    rage: 2,
                	    range: 6,
                	    physicalDmg: 0,
						poisonDmg: 100
                	);
                	break;
			    }
			}
		}

		public override void OnGotMeleeAttack( Mobile attacker )
		{
			base.OnGotMeleeAttack( attacker );

			if ( Utility.RandomMinMax( 1, 4 ) == 1 )
			{
				int goo = 0;

				foreach ( Item splash in this.GetItemsInRange( 10 ) ){ if ( splash is MonsterSplatter && splash.Name == "acidic ichor" ){ goo++; } }

				if ( goo == 0 )
				{
					MonsterSplatter.AddSplatter( this.X, this.Y, this.Z, this.Map, this.Location, this, "acidic ichor", 1167, 0 );
				}
			}
		}

		public override void OnDeath( Container c )
		{
			base.OnDeath( c );

			if ( Utility.RandomMinMax( 1, 4 ) == 1 )
			{
				Item acid = new BottleOfAcid();
				acid.Name = "jar of acidic ichor";
				acid.ItemID = 0x1007;
				acid.Hue = 0xB96;
				c.DropItem( acid );
			}
		}

		public override bool OnBeforeDeath()
		{
			this.BaseSoundID = 278;
			this.PlaySound( 0x580 );
			
			Effects.SendLocationEffect( this.Location, this.Map, 0x23B2, 16, 10, 1166, 0 );

			int goo = 0;

			foreach ( Item splash in this.GetItemsInRange( 10 ) ){ if ( splash is MonsterSplatter && splash.Name == "acidic ichor" ){ goo++; } }

			if ( goo == 0 )
			{
				MonsterSplatter.AddSplatter( this.X, this.Y, this.Z, this.Map, this.Location, this, "acidic ichor", 1167, 0 );
			}

			return base.OnBeforeDeath();
		}

		public override int GetAngerSound()
		{
			return 0x621;
		}

		public override int GetIdleSound()
		{
			return 0x259;
		}

		public override int GetAttackSound()
		{
			return 0x61E;
		}

		public override int GetHurtSound()
		{
			return 0x620;
		}

		public override int GetDeathSound()
		{
			return 0x61F;
		}

		public override int TreasureMapLevel{ get{ return 4; } }

		public override void GenerateLoot()
		{
			AddLoot( LootPack.FilthyRich, 2 );
			AddLoot( LootPack.Gems, 4);
		}

		public AntaurKing( Serial serial ) : base( serial )
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

			if ( BaseSoundID == 274 )
				BaseSoundID = 838;
		}
	}
}