using System;
using Server.Items;
using Server.Targeting;
using System.Collections;
using Server.Custom;

namespace Server.Mobiles
{
	[CorpseName( "a spider corpse" )]
	public class LolthsChosen : BaseCreature
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
		public LolthsChosen() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "Lolth's Chosen";
			Body = Utility.RandomList( 173, 460 );
			BaseSoundID = 0x388;
            Hue = 1316;
			SetStr( 476, 520 );
			SetDex( 436, 525 );
			SetInt( 336, 360 );

			SetHits( 446, 860 );
			SetMana( 0 );

			SetDamage( 26, 32 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 80 );
			SetResistance( ResistanceType.Poison, 100 );
			SetResistance( ResistanceType.Fire, 40 );
			SetResistance( ResistanceType.Cold, 40 );
			SetResistance( ResistanceType.Energy, 40 );

			SetSkill( SkillName.Poisoning, 125.0 );
			SetSkill( SkillName.MagicResist, 90.0 );
			SetSkill( SkillName.Tactics, 100.0 );
			SetSkill( SkillName.FistFighting, 100.0 );

			Fame = 16000;
			Karma = -16000;

			VirtualArmor = 50;

			PackItem( new SpidersSilk( 100 ) );

			Item Venom = new VenomSack();
				Venom.Name = "lethal venom sack";
				AddItem( Venom );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Rich, 3 );
		}
		
		public override void OnDeath(Container c)
		{
		    base.OnDeath(c);

		    Mobile killer = this.LastKiller;

			if (Utility.RandomDouble() < 0.05)
    		{
    		    c.DropItem(new EssenceOfLolthsHatred());
    		}
		

		    TotemDropHelper.TryDropTotem(
		        killer,
		        this,
		        "Monstrous Spider",
		        120.0,
		        0.45
		    );
		}

		public override PackInstinct PackInstinct{ get{ return PackInstinct.Arachnid; } }
		public override Poison PoisonImmune{ get{ return Poison.Deadly; } }
		public override Poison HitPoison{ get{ return Poison.Lethal; } }

		public override int GetAttackSound(){ return 0x601; }	// A
		public override int GetDeathSound(){ return 0x602; }	// D
		public override int GetHurtSound(){ return 0x603; }		// H

		public LolthsChosen( Serial serial ) : base( serial )
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