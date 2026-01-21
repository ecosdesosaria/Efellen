using System;
using Server;
using Server.Items;
using Server.CustomSpells;

namespace Server.Mobiles
{
	[CorpseName( "a drakkul corpse" )]
	public class DrakkulMage : BaseSpellCaster
	{
		public override bool HasBreath{ get{ return true; } }
		public override bool ReacquireOnMovement{ get{ return !Controlled; } }
		public override double BreathEffectDelay{ get{ return 0.1; } }
		public override int GetBreathForm()
		{
		    return 17;
		}

		[Constructable]
		public DrakkulMage() : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = NameList.RandomName( "drakkul" );
			Title = "the drakkul shaman";
			Body = 669;
			BaseSoundID = 357;
			Resource = CraftResource.BlackScales;

			SetStr( 246, 275 );
			SetDex( 76, 95 );
			SetInt( 301, 325 );

			SetHits( 286, 303 );

			SetDamage( 9, 18 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 50, 55 );
			SetResistance( ResistanceType.Fire, 45, 55 );
			SetResistance( ResistanceType.Cold, 25, 30 );
			SetResistance( ResistanceType.Poison, 45, 55 );

			SetSkill( SkillName.Psychology, 70.1, 80.0 );
			SetSkill( SkillName.Magery, 70.1, 80.0 );
			SetSkill( SkillName.MagicResist, 85.1, 95.0 );
			SetSkill( SkillName.Tactics, 70.1, 80.0 );
			SetSkill( SkillName.FistFighting, 60.1, 80.0 );

			Fame = 7500;
			Karma = -7500;

			VirtualArmor = 46;
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Rich );
			AddLoot( LootPack.Gems, Utility.RandomMinMax( 1, 4 ) );
		}
		public override int Meat{ get{ return 2; } }
		public override int Hides{ get{ return 2; } }
		public override int Scales{ get{ return 2; } }
		public override HideType HideType{ get{ return HideType.Draconic; } }
		public override ScaleType ScaleType{ get{ return ResourceScales(); } }
		public override int Skin{ get{ return Utility.Random(2); } }
		public override SkinType SkinType{ get{ return SkinType.Dragon; } }
		public override int Skeletal{ get{ return Utility.Random(2); } }
		public override SkeletalType SkeletalType{ get{ return SkeletalType.Draco; } }

		public DrakkulMage( Serial serial ) : base( serial )
		{
		}

		public override void OnAfterSpawn()
		{
			this.MobileMagics(Utility.Random(2,6), SpellType.Sorcerer | SpellType.Wizard, 0);
			base.OnAfterSpawn();
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 1 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
			if ( version >= 1 )
			{
				this.MobileMagics(Utility.Random(2,6), SpellType.Sorcerer | SpellType.Wizard, 0);
			}
		}
	}
}