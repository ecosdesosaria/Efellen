using System;
using Server;
using Server.Misc;
using Server.Items;
using Server.CustomSpells;

namespace Server.Mobiles 
{ 
	[CorpseName( "a mage corpse" )] 
	public class EvilMageApprentice : BaseSpellCaster 
	{ 
		public override int BreathPhysicalDamage{ get{ return 0; } }
		public override int BreathFireDamage{ get{ if ( YellHue < 2 ){ return 100; } else { return 0; } } }
		public override int BreathColdDamage{ get{ if ( YellHue == 3 ){ return 100; } else { return 0; } } }
		public override int BreathPoisonDamage{ get{ if ( YellHue == 2 ){ return 100; } else { return 0; } } }
		public override int BreathEnergyDamage{ get{ return 0; } }
		public override int BreathEffectHue{ get{ if ( YellHue == 1 ){ return 0x488; } else if ( YellHue == 2 ){ return 0xB92; } else if ( YellHue == 3 ){ return 0x5B5; } else { return 0x4FD; } } }
		public override int BreathEffectSound{ get{ return 0x238; } }
		public override int BreathEffectItemID{ get{ return 0x1005; } } // EXPLOSION POTION
		public override bool HasBreath{ get{ return true; } }
		public override double BreathEffectDelay{ get{ return 0.1; } }
		public override int GetBreathForm()
		{
		    return 3;
		}
		public override double BreathDamageScalar{ get{ return 0.4; } }

		[Constructable] 
		public EvilMageApprentice() : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 ) 
		{ 
			if ( this.Female = Utility.RandomBool() ) 
			{ 
				Body = 0x191; 
				Name = NameList.RandomName( "evil witch" );
				switch ( Utility.RandomMinMax( 0, 5 ) )
				{
					case 0: Title = "the apprentice wizard"; break;
					case 1: Title = "the apprentice sorcereress"; break;
					case 2: Title = "the apprentice mage"; break;
					case 3: Title = "the apprentice conjurer"; break;
					case 4: Title = "the apprentice magician"; break;
					case 5: Title = "the apprentice witch"; break;
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
					case 0: Title = "the apprentice wizard"; break;
					case 1: Title = "the apprentice sorcerer"; break;
					case 2: Title = "the apprentice mage"; break;
					case 3: Title = "the apprentice conjurer"; break;
					case 4: Title = "the apprentice magician"; break;
					case 5: Title = "the apprentice warlock"; break;
				}
				Utility.AssignRandomHair( this );
				int HairColor = Utility.RandomHairHue();
				FacialHairItemID = Utility.RandomList( 0, 8254, 8255, 8256, 8257, 8267, 8268, 8269 );
				HairHue = HairColor;
				FacialHairHue = HairColor;
			}

			Hue = Utility.RandomSkinColor();
			EmoteHue = 1;

			Server.Misc.IntelligentAction.DressUpWizards( this, false );

			SetStr( 65 );
			SetDex( 75 );
			SetInt( 90 );

			SetHits( 33, 43 );

			SetDamage( 3, 7 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 10 );
			SetResistance( ResistanceType.Fire, 5, 10 );
			SetResistance( ResistanceType.Poison, 5, 10 );
			SetResistance( ResistanceType.Energy, 5, 10 );

			SetSkill( SkillName.Psychology, 50.0 );
			SetSkill( SkillName.Magery, 55.1, 75.0 );
			SetSkill( SkillName.MagicResist, 67.5 );
			SetSkill( SkillName.Tactics, 47.5 );
			SetSkill( SkillName.FistFighting, 40.0 );
			SetSkill( SkillName.Bludgeoning, 40.0 );

			Fame = 2000;
			Karma = -2000;

			VirtualArmor = 10;
			PackReg( Utility.RandomMinMax( 1, 5 ) );
			PackReg( Utility.RandomMinMax( 1, 5 ) );
			PackReg( Utility.RandomMinMax( 1, 5 ) );

			if ( 0.3 > Utility.RandomDouble() )
				PackItem( new ArcaneGem() );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.LowScrolls );
			AddLoot( LootPack.MedScrolls );
			AddLoot( LootPack.MedPotions );
		}

		public override bool ClickTitle{ get{ return false; } }
		public override bool ShowFameTitle{ get{ return false; } }
		public override bool CanRummageCorpses{ get{ return true; } }
		public override bool AlwaysAttackable{ get{ return true; } }
		public override int Meat{ get{ return 1; } }
		public override int TreasureMapLevel{ get{ return Core.AOS ? 1 : 0; } }
		public override int Skeletal{ get{ return Utility.Random(3); } }
		public override SkeletalType SkeletalType{ get{ return SkeletalType.Brittle; } }

		public override void OnAfterSpawn()
		{
			Server.Misc.IntelligentAction.BeforeMyBirth( this );
			this.MobileMagics(Utility.Random(1,2), SpellType.Wizard | SpellType.Sorcerer, 0);
			base.OnAfterSpawn();
		}

		public override bool OnBeforeDeath()
		{
			Server.Misc.IntelligentAction.BeforeMyDeath( this );
			return base.OnBeforeDeath();
		}

		public EvilMageApprentice( Serial serial ) : base( serial )
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
				this.MobileMagics(Utility.Random(1,2), SpellType.Wizard | SpellType.Sorcerer, 0);
			}
		}
	}
}