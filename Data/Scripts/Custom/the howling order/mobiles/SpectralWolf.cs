using System;
using Server;
using Server.Items;
using System.Collections;

namespace Server.Mobiles
{
	[CorpseName( "dissipated energy" )]
	public class SpectralWolf : BaseCreature
	{
		public override bool AlwaysAttackable{ get{ return true; } }
		public override bool DeleteCorpseOnDeath { get { return true; } }
		public override bool AlwaysMurderer { get { return false; } }
		public override double DispelDifficulty { get { return 900.0; } }
		public override double DispelFocus { get { return 900.0; } }
        public override PackInstinct PackInstinct{ get{ return PackInstinct.Canine; } }

		[Constructable]
		public SpectralWolf() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "Spectral Wolf";
			Body = 967;
			Hue = 667;
			BaseSoundID = 0xE5;

			Timer.DelayCall( TimeSpan.FromSeconds( (double)(Utility.RandomMinMax( 90, 120 )) ), new TimerCallback( Delete ) );

			SetStr( 200 );
			SetDex( 200 );
			SetInt( 100 );

			SetHits( ( Core.SE ) ? 180 : 120 );
			SetStam( 250 );
			SetMana( 0 );

			SetDamage( 12, 15 );

			SetDamageType( ResistanceType.Physical, 0 );
			SetDamageType( ResistanceType.Energy, 100 );

			SetResistance( ResistanceType.Physical, 60, 80 );
			SetResistance( ResistanceType.Fire, 30, 50 );
			SetResistance( ResistanceType.Cold, 30, 50 );
			SetResistance( ResistanceType.Poison, 30, 50 );
			SetResistance( ResistanceType.Energy, 90, 100 );

			SetSkill( SkillName.MagicResist, 99.9 );
			SetSkill( SkillName.Tactics, 100.0 );
			SetSkill( SkillName.FistFighting, 120.0 );

			Fame = 0;
			Karma = 0;
			Tamable = false;
			VirtualArmor = 40;

			AddItem( new LightSource() );
		}

		public override bool BleedImmune{ get{ return true; } }
		public override Poison PoisonImmune { get { return Poison.Lethal; } }

		public override int GetAngerSound()
		{
			return 0x15;
		}

		public override int GetAttackSound()
		{
			return 0x28;
		}

		public SpectralWolf( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
			Timer.DelayCall( TimeSpan.FromSeconds( 30.0 ), new TimerCallback( Delete ) );
		}
	}
}