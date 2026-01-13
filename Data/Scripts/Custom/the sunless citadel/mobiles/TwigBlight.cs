using System;
using Server;
using Server.Items;
using Server.Engines.Plants;
using Server.Regions;
using Server.Mobiles;
using Server.Misc;

namespace Server.Mobiles
{
	[CorpseName( "a fallen Twig" )]
	public class TwigBlight : BaseCreature
	{
		public override WeaponAbility GetWeaponAbility()
		{
			return WeaponAbility.BleedAttack;
		}

		[Constructable]
		public TwigBlight() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a Twig Blight";
			Body = Utility.RandomList( 285, 301 );
			BaseSoundID = 442;
			Hue = 0x497;
			SetStr( 66, 115 );
			SetDex( 65 );
			SetInt( 101, 250 );

			SetHits( 49 );

			SetDamage( 7, 10 );

			SetDamageType( ResistanceType.Physical, 80 );
			SetDamageType( ResistanceType.Poison, 20 );

			SetResistance( ResistanceType.Physical, 35 );
			SetResistance( ResistanceType.Fire, 15 );
			SetResistance( ResistanceType.Cold, 30 );
			SetResistance( ResistanceType.Poison, 60 );
			SetResistance( ResistanceType.Energy, 50 );

			SetSkill( SkillName.MagicResist, 105.0 );
			SetSkill( SkillName.Tactics, 50.0 );
			SetSkill( SkillName.FistFighting, 60.0 );

			Fame = 1500;
			Karma = -1500;

			VirtualArmor = 40;

		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Poor );
		}

		public override Poison PoisonImmune{ get{ return Poison.Greater; } }
		public override bool BleedImmune{ get{ return true; } }

		public TwigBlight( Serial serial ) : base( serial )
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