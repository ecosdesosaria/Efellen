using System;
using Server.Items;
using Server.Targeting;
using System.Collections;
using Server.Custom;

namespace Server.Mobiles
{
	[CorpseName( "a spider corpse" )]
	public class DemonwebSpinner : BaseCreature
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
		public DemonwebSpinner() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "Demonweb Spinner";
			Body = Utility.RandomList( 173, 460 );
			BaseSoundID = 0x388;
            Hue = 0x0672;
			SetStr( 576, 620 );
			SetDex( 636 );
			SetInt( 336 );

			SetHits( 860 );
			SetMana( 0 );

			SetDamage( 18, 24 );

			SetDamageType( ResistanceType.Poison, 100 );

			SetResistance( ResistanceType.Physical, 80 );
			SetResistance( ResistanceType.Poison, 100 );
			SetResistance( ResistanceType.Fire, 40 );
			SetResistance( ResistanceType.Cold, 40 );
			SetResistance( ResistanceType.Energy, 40 );

			SetSkill( SkillName.Poisoning, 125.0 );
			SetSkill( SkillName.MagicResist, 90.0 );
			SetSkill( SkillName.Tactics, 100.0 );
			SetSkill( SkillName.FistFighting, 100.0 );

			Fame = 21000;
			Karma = -21000;

			VirtualArmor = 50;

			Item Venom = new VenomSack();
				Venom.Name = "lethal venom sack";
				AddItem( Venom );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Rich, 4 );
		}
		
		public override void OnDeath(Container c)
		{
		    base.OnDeath(c);

			if (Utility.RandomDouble() < 0.07)
    		{
    		    c.DropItem(new EssenceOfLolthsHatred());
    		}

		    Mobile killer = this.LastKiller;

		    TotemDropHelper.TryDropTotem(
		        killer,
		        this,
		        "Monstrous Spider",
		        120.0,
		        0.75
		    );
		}

		public override PackInstinct PackInstinct{ get{ return PackInstinct.Arachnid; } }
		public override Poison PoisonImmune{ get{ return Poison.Deadly; } }
		public override Poison HitPoison{ get{ return Poison.Deadly; } }

		public override int GetAttackSound(){ return 0x601; }	// A
		public override int GetDeathSound(){ return 0x602; }	// D
		public override int GetHurtSound(){ return 0x603; }		// H

		public DemonwebSpinner( Serial serial ) : base( serial )
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