using System;
using System.Collections;
using Server.Items;
using Server.Misc;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "um cadáver em decomposição" )]
	public class SeaZombie : BaseCreature
	{
		[Constructable]
		public SeaZombie() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Body = 304;
			CanSwim = true;

			switch( Utility.RandomMinMax( 0, 9 ) )
			{
				case 0: Name = "um zumbi"; break;
				case 1: Name = "um morto-vivo ambulante"; break;
				case 2: Name = "um cadaver"; break;
				case 3: Name = "um cadaver podre"; break;
				case 4: Name = "um cadaver morto-vivo"; break;
				case 5: Name = "um zumbi em decomposicao"; break;
				case 6: Name = "um zumbi"; break;
				case 7: Name = "um zumbi em decomposicao"; break;
				case 8: Name = "um cadaver em decomposicao"; break;
				case 9: Name = "um cadaver ambulante"; break;
			}

			int[] list = new int[]
				{
					0x1CF0, 0x1CEF, 0x1CEE, 0x1CED, 0x1CE9, 0x1DA0, 0x1DAE, // pieces
					0x1CEC, 0x1CE5, 0x1CE2, 0x1CDD, 0x1AE4, 0x1DA1, 0x1DA2, 0x1DA4, 0x1DAF, 0x1DB0, 0x1DB1, 0x1DB2, // limbs
					0x1CE8, 0x1CE0, 0x1D9F, 0x1DAD // torsos
				};

			PackItem( new BodyPart( Utility.RandomList( list ) ) );

			BaseSoundID = 471;

			SetStr( 46, 70 );
			SetDex( 31, 50 );
			SetInt( 26, 40 );

			SetHits( 28, 42 );

			SetDamage( 3, 7 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 15, 20 );
			SetResistance( ResistanceType.Cold, 20, 30 );
			SetResistance( ResistanceType.Poison, 5, 10 );

			SetSkill( SkillName.MagicResist, 15.1, 40.0 );
			SetSkill( SkillName.Tactics, 35.1, 50.0 );
			SetSkill( SkillName.FistFighting, 35.1, 50.0 );

			Fame = 600;
			Karma = -600;

			VirtualArmor = 18;
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Meager );
		}

		public override bool BleedImmune{ get{ return true; } }
		public override Poison PoisonImmune{ get{ return Poison.Regular; } }
		public override bool CanRummageCorpses{ get{ return true; } }
		public override bool ShowFameTitle{ get{ return false; } }
		public override bool AlwaysAttackable{ get{ return true; } }
		public override int Hides{ get{ return 3; } }
		public override HideType HideType{ get{ return HideType.Necrotic; } }

		public SeaZombie( Serial serial ) : base( serial )
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