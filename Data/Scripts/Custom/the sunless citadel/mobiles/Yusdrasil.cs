using System;
using Server;
using Server.Items;
using System.Collections;
using Server.Misc;

namespace Server.Mobiles
{
	[CorpseName( "Yusdrasil's corpse" )]
	public class Yusdrasil : BaseCreature
	{
		public override int BreathPhysicalDamage{ get{ return 0; } }
		public override int BreathFireDamage{ get{ if ( YellHue < 2 ){ return 100; } else { return 0; } } }
		public override int BreathColdDamage{ get{ if ( YellHue == 3 ){ return 100; } else { return 0; } } }
		public override int BreathPoisonDamage{ get{ if ( YellHue == 2 ){ return 100; } else { return 0; } } }
		public override int BreathEnergyDamage{ get{ return 0; } }
		public override int BreathEffectHue{ get{ if ( YellHue == 1 ){ return 0x488; } else if ( YellHue == 2 ){ return 0xB92; } else if ( YellHue == 3 ){ return 0x5B5; } else { return 0x4FD; } } }
		public override int BreathEffectSound{ get{ return 0x238; } }
		public override int BreathEffectItemID{ get{ return 0x1005; } }
		public override bool HasBreath{ get{ return true; } }
		public override double BreathEffectDelay{ get{ return 0.1; } }
		public override int GetBreathForm()
		{
		    return 9;
		}
		public override double BreathDamageScalar{ get{ return 0.6; } }

		public override InhumanSpeech SpeechType{ get{ return InhumanSpeech.Ratman; } }

		[Constructable]
		public Yusdrasil() : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "Yusdrasil";
			Body = 253;
			BaseSoundID = 0x543;
			Hue = 0x25;

			SetStr( 61, 85 );
			SetDex( 41, 60 );
			SetInt( 36, 95 );

			SetHits( 79 );

			SetDamage( 3, 8 );

			SetDamageType( ResistanceType.Physical, 0 );
			SetDamageType( ResistanceType.Fire, 50 );
			SetDamageType( ResistanceType.Poison, 50 );

			SetResistance( ResistanceType.Physical, 35 );
			SetResistance( ResistanceType.Fire, 45 );
			SetResistance( ResistanceType.Cold, 30 );
			SetResistance( ResistanceType.Poison, 40 );
			SetResistance( ResistanceType.Energy, 40 );

			SetSkill( SkillName.Psychology, 40.0 );
			SetSkill( SkillName.Magery, 80.0, 100.0 );
			SetSkill( SkillName.MagicResist, 40.0 );
			SetSkill( SkillName.Tactics, 50.0 );
			SetSkill( SkillName.FistFighting, 54.0 );

			Fame = 535;
			Karma = -535;

			VirtualArmor = 20;
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Meager );
			AddLoot( LootPack.LowScrolls );
			AddLoot( LootPack.LowScrolls );
			AddLoot( LootPack.LowPotions );
		}

		public override bool OnBeforeDeath()
		{
			if ( Server.Misc.IntelligentAction.HealThySelf( this ) ){ return false; }
			return base.OnBeforeDeath();
		}

		public override int Meat{ get{ return 1; } }
		public override int Hides{ get{ return 1; } }
		public override HideType HideType{ get{ return HideType.Horned; } }

		public override int GetAttackSound(){ return 0x5FD; }	// A
		public override int GetDeathSound(){ return 0x5FE; }	// D
		public override int GetHurtSound(){ return 0x5FF; }		// H

		public Yusdrasil( Serial serial ) : base( serial )
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