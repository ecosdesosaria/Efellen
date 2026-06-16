using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Misc;
using Server.Network;

namespace Server.Mobiles
{
	[CorpseName( "an elemental corpse" )]
	public class LolthsJealousy : BaseCreature
	{
		public override double DispelDifficulty{ get{ return 140.5; } }
		public override double DispelFocus{ get{ return 30.0; } }

		public override int BreathPhysicalDamage{ get{ return 25; } }
		public override int BreathFireDamage{ get{ return 0; } }
		public override int BreathColdDamage{ get{ return 0; } }
		public override int BreathPoisonDamage{ get{ return 75; } }
		public override int BreathEnergyDamage{ get{ return 0; } }
		public override int BreathEffectHue{ get{ return 0x48F; } }
		public override int BreathEffectSound{ get{ return 0x012; } }
		public override int BreathEffectItemID{ get{ return 0x1A85; } }
		public override bool ReacquireOnMovement{ get{ return !Controlled; } }
		public override bool HasBreath{ get{ return true; } }
		public override double BreathEffectDelay{ get{ return 0.1; } }
		public override int GetBreathForm()
		{
		    return 36;
		}

		[Constructable]
		public LolthsJealousy () : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "Lolth's Jealousy";
			Body = 224;
			Hue = 0x07c7;
			BaseSoundID = 278;

			SetStr( 526, 555 );
			SetDex( 226, 245 );
			SetInt( 371, 395 );
			SetHits( 396, 513 );
			SetDamage( 15, 21 );
			SetDamageType( ResistanceType.Physical, 50 );
			SetDamageType( ResistanceType.Poison, 50 );
			SetResistance( ResistanceType.Physical, 55, 65 );
			SetResistance( ResistanceType.Poison, 90 );
			SetResistance( ResistanceType.Fire, 30, 40 );
			SetResistance( ResistanceType.Cold, 20, 30 );
			SetResistance( ResistanceType.Energy, 40, 50 );
			SetSkill( SkillName.Anatomy, 60.3, 90.0 );
			SetSkill( SkillName.Psychology, 105.0 );
			SetSkill( SkillName.Magery, 105.0 );
			SetSkill( SkillName.MagicResist, 85.0 );
			SetSkill( SkillName.Tactics, 105.0 );
			SetSkill( SkillName.FistFighting, 95.0 );
			Fame = 17000;
			Karma = -17000;
			VirtualArmor = 50;

			PackItem( new BottleOfAcid() );
			PackItem( new BottleOfAcid() );
			PackItem( new BottleOfAcid() );
		}

		public override void OnDeath( Container c )
		{
			base.OnDeath( c );
		
			if ( Utility.RandomDouble() <= 0.01 )
				c.DropItem( new OrbOfTheDemonwebPits() );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Rich,3 );
		}

		public override bool BleedImmune{ get{ return true; } }
		public override Poison HitPoison{ get{ return Poison.Lethal; } }
		public override double HitPoisonChance{ get{ return 0.6; } }
		public override int TreasureMapLevel{ get{ return Core.AOS ? 2 : 3; } }

		public override void OnGotMeleeAttack( Mobile attacker )
		{
			base.OnGotMeleeAttack( attacker );

			if ( Utility.RandomMinMax( 1, 3 ) == 1 )
			{
				int goo = 0;

				foreach ( Item splash in this.GetItemsInRange( 10 ) ){ if ( splash is MonsterSplatter && splash.Name == "acidic slime" ){ goo++; } }

				if ( goo == 0 )
				{
					MonsterSplatter.AddSplatter( this.X, this.Y, this.Z, this.Map, this.Location, this, "acidic slime", 1167, 0 );
				}
			}
		}

		public LolthsJealousy( Serial serial ) : base( serial )
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