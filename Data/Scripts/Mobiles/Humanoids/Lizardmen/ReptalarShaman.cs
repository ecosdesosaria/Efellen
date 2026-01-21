using System;
using Server;
using Server.Misc;
using Server.Items;
using Server.CustomSpells;

namespace Server.Mobiles
{
	[CorpseName( "a reptalar corpse" )]
	public class ReptalarShaman : BaseSpellCaster
	{
		public override InhumanSpeech SpeechType { get { return InhumanSpeech.Lizardman; } }

		[Constructable]
		public ReptalarShaman () : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = NameList.RandomName( "lizardman" );
			Title = "the reptalar shaman";
			Body = 324;
			BaseSoundID = 417;
			Resource = CraftResource.BlueScales;

			SetStr( 136, 165 );
			SetDex( 56, 75 );
			SetInt( 131, 155 );

			SetHits( 82, 99 );

			SetDamage( 7, 17 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 25, 35 );
			SetResistance( ResistanceType.Fire, 30, 40 );
			SetResistance( ResistanceType.Cold, 20, 30 );
			SetResistance( ResistanceType.Poison, 30, 40 );
			SetResistance( ResistanceType.Energy, 30, 40 );

			SetSkill( SkillName.Psychology, 80.1, 92.5 );
			SetSkill( SkillName.Magery, 80.1, 92.5 );
			SetSkill( SkillName.MagicResist, 80.1, 95.0 );
			SetSkill( SkillName.Tactics, 70.1, 95.0 );
			SetSkill( SkillName.FistFighting, 60.1, 80.0 );

			Fame = 4000;
			Karma = -4000;

			VirtualArmor = 50;

			PackReg( 16 );
		}

		public override void OnAfterSpawn()
		{
			this.MobileMagics(Utility.Random(1,3), SpellType.Cleric, 0);
			base.OnAfterSpawn();
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Average );
			AddLoot( LootPack.LowScrolls );
			AddLoot( LootPack.LowPotions );
		}

		public override bool OnBeforeDeath()
		{
			if ( Server.Misc.IntelligentAction.HealThySelf( this ) ){ return false; }
			return base.OnBeforeDeath();
		}

		public override bool CanRummageCorpses{ get{ return true; } }
		public override int Meat{ get{ return 1; } }
		public override int Hides{ get{ return 12; } }
		public override HideType HideType{ get{ return HideType.Horned; } }
		public override int Scales{ get{ return 1; } }
		public override ScaleType ScaleType{ get{ return ResourceScales(); } }
		public override Poison PoisonImmune{ get{ return Poison.Regular; } }
		public override Poison HitPoison{ get{ return Poison.Lesser; } }
		public override int Skeletal{ get{ return Utility.Random(3); } }
		public override SkeletalType SkeletalType{ get{ return SkeletalType.Reptile; } }

		public ReptalarShaman( Serial serial ) : base( serial )
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
			this.MobileMagics(Utility.Random(1,3), SpellType.Cleric, 0);
		}
	}
}
