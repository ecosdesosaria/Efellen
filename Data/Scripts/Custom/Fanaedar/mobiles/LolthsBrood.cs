using System;
using Server.Items;
using Server.Targeting;
using System.Collections;

namespace Server.Mobiles
{
	[CorpseName( "a spider corpse" )]
	public class LolthsBrood : BaseCreature
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
		public LolthsBrood() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "Lolth's Brood";
			Body = Utility.RandomList( 28, 140, 964 );
			BaseSoundID = 0x388;
			Hue = 1316;
			
			SetStr( 126, 150 );
			SetDex( 96, 145 );
			SetInt( 36, 60 );

			SetHits( 146, 260 );
			SetMana( 0 );

			SetDamage( 9, 18 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 40 );
			SetResistance( ResistanceType.Fire, 20 );
			SetResistance( ResistanceType.Cold, 30 );
			SetResistance( ResistanceType.Poison, 75, 80 );
			SetResistance( ResistanceType.Energy, 30 );

			SetSkill( SkillName.Poisoning, 90.1, 100.0 );
			SetSkill( SkillName.MagicResist, 50.0 );
			SetSkill( SkillName.Tactics, 60.0 );
			SetSkill( SkillName.FistFighting, 75.0 );

			Fame = 7600;
			Karma = -7600;

			VirtualArmor = 16;

			Tamable = true;
			ControlSlots = 1;
			MinTameSkill = 99.1;

			PackItem( new SpidersSilk( 5 ) );

			Item Venom = new VenomSack();
				Venom.Name = "venom sack";
				AddItem( Venom );
		}

		public override void OnDeath(Container c)
		{
		    base.OnDeath(c);
			if (Utility.RandomDouble() < 0.04)
    		{
    		    c.DropItem(new EssenceOfLolthsHatred());
    		}
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Rich );
		}

		public override FoodType FavoriteFood{ get{ return FoodType.Meat; } }
		public override PackInstinct PackInstinct{ get{ return PackInstinct.Arachnid; } }
		public override Poison PoisonImmune{ get{ return Poison.Regular; } }
		public override Poison HitPoison{ get{ return Poison.Regular; } }

		public override int Cloths{ get{ return 5; } }
		public override ClothType ClothType{ get{ return ClothType.Silk; } }

		public LolthsBrood( Serial serial ) : base( serial )
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