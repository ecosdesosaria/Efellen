using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a spider corpse" )]
	public class LolthsChampion : BaseCreature
	{
		public override int BreathPhysicalDamage{ get{ return 50; } }
		public override int BreathFireDamage{ get{ return 0; } }
		public override int BreathColdDamage{ get{ return 0; } }
		public override int BreathPoisonDamage{ get{ return 50; } }
		public override int BreathEnergyDamage{ get{ return 0; } }
		public override int BreathEffectHue{ get{ return 0; } }
		public override int BreathEffectSound{ get{ return 0x62A; } }
		public override int BreathEffectItemID{ get{ return 0x10D4; } }
		public override bool HasBreath{ get{ return true; } }
		public override double BreathEffectDelay{ get{ return 0.1; } }
		public override int GetBreathForm()
		{
		    return 6;
		}

		[Constructable]
		public LolthsChampion () : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "Lolth's Champion";
			Body = 140;
			Hue = 1316;
			BaseSoundID = 1170;

			SetStr( 320 );
			SetDex( 195 );
			SetInt( 310 );

			SetHits( 432 );

			SetDamage( 9, 23 );

			SetDamageType( ResistanceType.Physical, 20 );
			SetDamageType( ResistanceType.Poison, 80 );

			SetResistance( ResistanceType.Physical, 50 );
			SetResistance( ResistanceType.Fire, 30 );
			SetResistance( ResistanceType.Cold, 40 );
			SetResistance( ResistanceType.Poison, 95, 100 );
			SetResistance( ResistanceType.Energy, 40 );

			SetSkill( SkillName.Psychology, 101.0 );
			SetSkill( SkillName.Magery, 101.0 );
			SetSkill( SkillName.Meditation, 80.0 );
			SetSkill( SkillName.MagicResist, 120.0 );
			SetSkill( SkillName.Tactics, 90.0 );
			SetSkill( SkillName.FistFighting, 95.0 );

			Fame = 5000;
			Karma = -5000;

			VirtualArmor = 36;

			PackItem( new SpidersSilk( 8 ) );

			Item Venom = new VenomSack();
				Venom.Name = "lethal venom sack";
				AddItem( Venom );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Rich, 2 );
		}

		public override Poison PoisonImmune{ get{ return Poison.Deadly; } }
		public override Poison HitPoison{ get{ return Poison.Lethal; } }
		public override int TreasureMapLevel{ get{ return 3; } }

		public override int Cloths{ get{ return 12; } }
		public override ClothType ClothType{ get{ return ClothType.Silk; } }

		public LolthsChampion( Serial serial ) : base( serial )
		{
		}

		public override void OnDeath(Container c)
		{
		    base.OnDeath(c);
			if (Utility.RandomDouble() < 0.04)
    		{
    		    c.DropItem(new EssenceOfLolthsHatred());
    		}
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

			if ( BaseSoundID == 263 )
				BaseSoundID = 1170;
		}
	}
}