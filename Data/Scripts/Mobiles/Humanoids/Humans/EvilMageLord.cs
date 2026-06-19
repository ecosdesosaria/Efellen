using System;
using Server;
using Server.Misc;
using Server.Items;
using Server.CustomSpells;

namespace Server.Mobiles 
{ 
	[CorpseName( "o cadáver de um mago" )]
	public class EvilMageLord : BaseSpellCaster 
	{ 
	
		[Constructable] 
		public EvilMageLord() : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 ) 
		{
			string sGrand = "grande";

			switch ( Utility.RandomMinMax( 0, 5 ) )
			{
				case 0: sGrand = "grande"; break;
				case 1: sGrand = "magnífico"; break;
				case 2: sGrand = "mestre"; break;
				case 3: sGrand = "poderoso"; break;
				case 4: sGrand = "supremo"; break;
				case 5: sGrand = "todo-poderoso"; break;
			}

			if ( this.Female = Utility.RandomBool() ) 
			{ 
				Body = 0x191; 
				Name = NameList.RandomName( "evil witch" );
				switch ( Utility.RandomMinMax( 0, 5 ) )
				{
					case 0: Title = "o " + sGrand + " maga"; break;
					case 1: Title = "a " + sGrand + " feiticeira"; break;
					case 2: Title = "a " + sGrand + " maga"; break;
					case 3: Title = "a " + sGrand + " conjuradora"; break;
					case 4: Title = "a " + sGrand + " ilusionista"; break;
					case 5: Title = "a " + sGrand + " bruxa"; break;
				}
				Utility.AssignRandomHair( this );
				HairHue = Utility.RandomHairHue();
			} 
			else 
			{ 
				Body = 0x190; 
				Name = NameList.RandomName( "evil mage" );
				switch ( Utility.RandomMinMax( 0, 5 ) )
				{
					case 0: Title = "o " + sGrand + " mago"; break;
					case 1: Title = "o " + sGrand + " feiticeiro"; break;
					case 2: Title = "o " + sGrand + " mago"; break;
					case 3: Title = "o " + sGrand + " conjurador"; break;
					case 4: Title = "o " + sGrand + " ilusionista"; break;
					case 5: Title = "o " + sGrand + " bruxo"; break;
				}
				Utility.AssignRandomHair( this );
				int HairColor = Utility.RandomHairHue();
				FacialHairItemID = Utility.RandomList( 0, 8254, 8255, 8256, 8257, 8267, 8268, 8269 );
				HairHue = HairColor;
				FacialHairHue = HairColor;
			}

			Hue = Utility.RandomSkinColor();
			EmoteHue = 11;

			Server.Misc.IntelligentAction.DressUpWizards( this, false );

			SetStr( 151, 175 );
			SetDex( 261, 285 );
			SetInt( 196, 220 );

			SetHits( 149, 163 );

			SetDamage( 8, 14 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 35, 40 );
			SetResistance( ResistanceType.Fire, 30, 40 );
			SetResistance( ResistanceType.Cold, 30, 40 );
			SetResistance( ResistanceType.Poison, 30, 40 );
			SetResistance( ResistanceType.Energy, 30, 40 );

			SetSkill( SkillName.Psychology, 80.2, 100.0 );
			SetSkill( SkillName.Magery, 95.1, 100.0 );
			SetSkill( SkillName.Meditation, 27.5, 50.0 );
			SetSkill( SkillName.MagicResist, 77.5, 100.0 );
			SetSkill( SkillName.Tactics, 65.0, 87.5 );
			SetSkill( SkillName.FistFighting, 20.3, 80.0 );
			SetSkill( SkillName.Bludgeoning, 20.3, 80.0 );

			Fame = 12500;
			Karma = -12500;

			VirtualArmor = 40;
			PackReg( Utility.RandomMinMax( 4, 12 ) );
			PackReg( Utility.RandomMinMax( 4, 12 ) );
			PackReg( Utility.RandomMinMax( 4, 12 ) );

			if ( 0.6 > Utility.RandomDouble() )
				PackItem( new LesserWandOfDisenchanting() );

			if ( 0.9 > Utility.RandomDouble() )
				PackItem( new ArcaneGem() );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Average );
			AddLoot( LootPack.Meager );
			AddLoot( LootPack.MedScrolls, 2 );
			AddLoot( LootPack.MedPotions );
		}

		public override bool ClickTitle{ get{ return false; } }
		public override bool ShowFameTitle{ get{ return false; } }
		public override bool CanRummageCorpses{ get{ return true; } }
		public override bool AlwaysAttackable{ get{ return true; } }
		public override int Meat{ get{ return 1; } }
		public override int TreasureMapLevel{ get{ return Core.AOS ? 2 : 0; } }
		public override int Skeletal{ get{ return Utility.Random(3); } }
		public override SkeletalType SkeletalType{ get{ return SkeletalType.Brittle; } }

		public override void OnAfterSpawn()
		{
			Server.Misc.IntelligentAction.BeforeMyBirth( this );
			this.MobileMagics(Utility.Random(4,6), SpellType.Wizard | SpellType.Sorcerer, 0);
			base.OnAfterSpawn();
		}

		public override void OnGotMeleeAttack( Mobile attacker )
		{
			base.OnGotMeleeAttack( attacker );
			Server.Misc.IntelligentAction.DoSpecialAbility( this, attacker );
			Server.Misc.IntelligentAction.CryOut( this );
		}

		public override void OnGaveMeleeAttack( Mobile defender )
		{
			base.OnGaveMeleeAttack( defender );
			Server.Misc.IntelligentAction.DoSpecialAbility( this, defender );
		}

		public override bool OnBeforeDeath()
		{
			Server.Misc.IntelligentAction.BeforeMyDeath( this );
			return base.OnBeforeDeath();
		}

		public void AddArcane( Item item )
		{
			if ( item is IArcaneEquip )
			{
				IArcaneEquip eq = (IArcaneEquip)item;
				eq.CurArcaneCharges = eq.MaxArcaneCharges = 20;
			}

			item.Hue = ArcaneGem.DefaultArcaneHue;
			item.LootType = LootType.Newbied;

			AddItem( item );
		}

		public EvilMageLord( Serial serial ) : base( serial ) 
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
				this.MobileMagics(Utility.Random(4,6), SpellType.Wizard | SpellType.Sorcerer, 0);
			} 
		} 
	} 
}