using System;
using System.Collections;
using Server.Misc;
using Server.Items;
using Server.Targeting;
using Server.CustomSpells;

namespace Server.Mobiles
{
	[CorpseName( "a glowing ratman corpse" )]
	public class RatmanMage : BaseSpellCaster
	{
		public override InhumanSpeech SpeechType{ get{ return InhumanSpeech.Ratman; } }

		[Constructable]
		public RatmanMage() : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = NameList.RandomName( "ratman" );
			Title = "the ratman shaman";
			Body = Utility.RandomList( 73, 871 );
			BaseSoundID = 437;

			if ( Body == 871 )
				Title = "the ratling mage";

			SetStr( 146, 180 );
			SetDex( 101, 130 );
			SetInt( 186, 210 );

			SetHits( 88, 108 );

			SetDamage( 7, 14 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 40, 45 );
			SetResistance( ResistanceType.Fire, 10, 20 );
			SetResistance( ResistanceType.Cold, 10, 20 );
			SetResistance( ResistanceType.Poison, 10, 20 );
			SetResistance( ResistanceType.Energy, 10, 20 );

			SetSkill( SkillName.Psychology, 70.1, 80.0 );
			SetSkill( SkillName.Magery, 70.1, 80.0 );
			SetSkill( SkillName.MagicResist, 65.1, 90.0 );
			SetSkill( SkillName.Tactics, 50.1, 75.0 );
			SetSkill( SkillName.FistFighting, 50.1, 75.0 );

			Fame = 7500;
			Karma = -7500;

			VirtualArmor = 44;

			PackReg( 6 );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Rich );
			AddLoot( LootPack.LowScrolls );
		}

		public override void OnAfterSpawn()
		{
			this.MobileMagics(Utility.Random(1,3), SpellType.Wizard | SpellType.Bard, 0);
			base.OnAfterSpawn();
		}

		public override bool CanRummageCorpses{ get{ return true; } }
		public override int Meat{ get{ return 1; } }
		public override int Hides{ get{ return 8; } }

		public RatmanMage( Serial serial ) : base( serial )
		{
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
			if(version>=1)
			{
				this.MobileMagics(Utility.Random(1,3), SpellType.Wizard | SpellType.Bard, 0);
			}

		}
	}
}
