using System;
using Server;
using Server.Items;
using Server.CustomSpells;
using Server.Custom;

namespace Server.Mobiles
{
	[CorpseName( "a Scorched Kiln Fiend corpse" )]
	public class ScorchedKilnFiend : BaseCreature
	{
		[Constructable]
		public ScorchedKilnFiend () : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a Scorched Kiln Fiend";
			Body = 0x2fd;
			Hue = 2931;
			BaseSoundID = 357;

			SetStr( 276, 305 );
			SetDex( 46, 65 );
			SetInt( 201, 225 );

			SetHits( 186, 203 );

			SetDamage( 4, 12 );

			SetDamageType( ResistanceType.Physical, 50 );
			SetDamageType( ResistanceType.Fire, 50 );

			SetResistance( ResistanceType.Physical, 45, 60 );
			SetResistance( ResistanceType.Fire, 60, 70 );
			SetResistance( ResistanceType.Cold, 20, 30 );
			SetResistance( ResistanceType.Poison, 20, 30 );
			SetResistance( ResistanceType.Energy, 30, 40 );

			SetSkill( SkillName.Psychology, 70.1, 80.0 );
			SetSkill( SkillName.Magery, 70.1, 80.0 );
			SetSkill( SkillName.MagicResist, 85.1, 95.0 );
			SetSkill( SkillName.Tactics, 70.1, 80.0 );
			SetSkill( SkillName.FistFighting, 60.1, 80.0 );

			Fame = 9000;
			Karma = -9000;
			VirtualArmor = 40;
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Average, 2 );
			AddLoot( LootPack.MedScrolls, 2 );
		}
        
		public override void OnAfterSpawn()
		{
			this.MobileMagics(Utility.Random(3,5), SpellType.Wizard, 2931);
			base.OnAfterSpawn();
		}

        public override void OnDeath(Container c)
		{
		    base.OnDeath(c);
		    BossLootSystem.BossEnchant(this, c, 275, 15, 1, "scorched");
		}

		public override Poison PoisonImmune{ get{ return Poison.Regular; } }
		public override int TreasureMapLevel{ get{ return 2; } }
		public override int Meat{ get{ return 1; } }
		public override int Hides{ get{ return 8; } }
		public override HideType HideType{ get{ return HideType.Hellish; } }
		public override int Skin{ get{ return Utility.Random(2); } }
		public override SkinType SkinType{ get{ return SkinType.Demon; } }
		public override int Skeletal{ get{ return Utility.Random(3); } }
		public override SkeletalType SkeletalType{ get{ return SkeletalType.Devil; } }

		public ScorchedKilnFiend( Serial serial ) : base( serial )
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
			this.MobileMagics(Utility.Random(3,5), SpellType.Wizard, 2931);
		}
	}
}
