using System;
using Server;
using Server.Misc;
using Server.Items;
using Server.CustomSpells;

namespace Server.Mobiles 
{ 
	[CorpseName( "Sharwyn's corpse" )] 
	public class Sharwyn : BaseSpellCaster 
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
		public Sharwyn() : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 ) 
		{ 
			Body = 0x191; 
			Name = "Sharwyn";
			Utility.AssignRandomHair( this );
			int HairColor = Utility.RandomHairHue();
			HairHue = HairColor;
			Hue = Utility.RandomSkinColor();
			EmoteHue = 1;

			Server.Misc.IntelligentAction.DressUpWizards( this, false );

			SetStr( 95 );
			SetDex( 105 );
			SetInt( 120 );

			SetHits( 223 );

			SetDamage( 5, 10 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 15, 20 );
			SetResistance( ResistanceType.Fire, 5, 10 );
			SetResistance( ResistanceType.Poison, 5, 10 );
			SetResistance( ResistanceType.Energy, 5, 10 );

			SetSkill( SkillName.Psychology, 75.1, 100.0 );
			SetSkill( SkillName.Magery, 75.1, 100.0 );
			SetSkill( SkillName.MagicResist, 75.0, 97.5 );
			SetSkill( SkillName.Tactics, 65.0, 87.5 );
			SetSkill( SkillName.FistFighting, 20.2, 60.0 );
			SetSkill( SkillName.Bludgeoning, 20.3, 60.0 );

			Fame = 6000;
			Karma = -6000;
        	VirtualArmor = 30;
			PackReg( Utility.RandomMinMax( 2, 10 ) );
			PackReg( Utility.RandomMinMax( 2, 10 ) );
			PackReg( Utility.RandomMinMax( 2, 10 ) );

			if ( 0.5 > Utility.RandomDouble() )
				PackItem( new ArcaneGem() );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Average );
			AddLoot( LootPack.Average );
			AddLoot( LootPack.MedScrolls );
			AddLoot( LootPack.MedPotions );
		}

        public override void OnDeath( Container c )
		{
			base.OnDeath( c );

			if ( Utility.RandomMinMax( 1, 6 ) == 1 )
			{
				ElvenGlasses shawynGlasses = new WizardsGlasses();
				shawynGlasses.Name = "Shawyn's Glasses";
                shawynGlasses.Hue = 1322;
				shawynGlasses.SkillBonuses.SetValues( 0, SkillName.Magery, 10 );
				shawynGlasses.SkillBonuses.SetValues( 1, SkillName.Inscribe, 10 );
                shawynGlasses.Attributes.LowerManaCost = 15;
				c.DropItem( shawynGlasses );
			}
		}

		public override void OnAfterSpawn()
		{
			this.MobileMagics(2, SpellType.Sorcerer, 0);
			base.OnAfterSpawn();
		}

		public override bool ClickTitle{ get{ return false; } }
		public override bool ShowFameTitle{ get{ return false; } }
		public override bool CanRummageCorpses{ get{ return true; } }
		public override bool AlwaysAttackable{ get{ return true; } }
		public override int Meat{ get{ return 1; } }
		public override int TreasureMapLevel{ get{ return Core.AOS ? 1 : 0; } }
		public override int Skeletal{ get{ return Utility.Random(3); } }
		public override SkeletalType SkeletalType{ get{ return SkeletalType.Brittle; } }

		public Sharwyn( Serial serial ) : base( serial )
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
				this.MobileMagics(2, SpellType.Sorcerer, 0);
			}
		}
	}
}