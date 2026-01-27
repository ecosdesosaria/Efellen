using System;
using Server;
using Server.Misc;
using Server.Items;
using Server.CustomSpells;
using Server.Custom;

namespace Server.Mobiles 
{ 
	[CorpseName( "a drow mage corpse" )] 
	public class DrowMageLord : BaseSpellCaster 
	{ 
	
		[Constructable] 
		public DrowMageLord() : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 ) 
		{
			string sGrand = "grand";

			switch ( Utility.RandomMinMax( 0, 5 ) )
			{
				case 0: sGrand = "grand"; break;
				case 1: sGrand = "great"; break;
				case 2: sGrand = "master"; break;
				case 3: sGrand = "powerful"; break;
				case 4: sGrand = "supreme"; break;
				case 5: sGrand = "almighty"; break;
			}
			Body = 605; 
			Name = NameList.RandomName( "dark_elf_prefix_male" )  +" "+  NameList.RandomName( "dark_elf_suffix_male" );
			switch ( Utility.RandomMinMax( 0, 5 ) )
			{
				case 0: Title = "the " + sGrand + " Drow Wizard"; break;
				case 1: Title = "the " + sGrand + " Drow Sorcerer"; break;
				case 2: Title = "the " + sGrand + " Drow Mage"; break;
				case 3: Title = "the " + sGrand + " Drow Conjurer"; break;
				case 4: Title = "the " + sGrand + " Drow Magician"; break;
				case 5: Title = "the " + sGrand + " Drow Warlock"; break;
			}
			Utility.AssignRandomHair( this );
			HairHue = 1150;
			Hue = 1316;
			EmoteHue = 11;

			Server.Misc.IntelligentAction.DressUpWizards( this, false );

			SetStr( 181, 275 );
			SetDex( 261, 285 );
			SetInt( 296, 320 );

			SetHits( 449, 563 );

			SetDamage( 12, 18 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 35, 40 );
			SetResistance( ResistanceType.Fire, 30, 40 );
			SetResistance( ResistanceType.Cold, 30, 40 );
			SetResistance( ResistanceType.Poison, 30, 40 );
			SetResistance( ResistanceType.Energy, 30, 40 );

			SetSkill( SkillName.Psychology, 110.0 );
			SetSkill( SkillName.Magery, 115.0 );
			SetSkill( SkillName.Meditation, 70.0 );
			SetSkill( SkillName.MagicResist, 125.0 );
			SetSkill( SkillName.Tactics, 97.5 );
			SetSkill( SkillName.FistFighting, 70.0 );
			SetSkill( SkillName.Bludgeoning, 90.0 );

			Fame = 16500;
			Karma = -16500;

			VirtualArmor = 55;
			PackReg( Utility.RandomMinMax( 6, 15 ) );
			PackReg( Utility.RandomMinMax( 6, 15 ) );
			PackItem( new ArcaneGem() );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Rich, 1 );
			AddLoot( LootPack.HighScrolls, 1 );
			AddLoot( LootPack.MedScrolls, 2 );
		}

		public override bool ClickTitle{ get{ return false; } }
		public override bool ShowFameTitle{ get{ return false; } }
		public override bool CanRummageCorpses{ get{ return true; } }
		public override bool AlwaysAttackable{ get{ return true; } }
		public override int Meat{ get{ return 1; } }
		public override int TreasureMapLevel{ get{ return 4; } }
		public override int Skeletal{ get{ return Utility.Random(8); } }
		public override SkeletalType SkeletalType{ get{ return SkeletalType.Drow; } }

		public override void OnAfterSpawn()
		{
			Server.Misc.IntelligentAction.BeforeMyBirth( this );
			this.MobileMagics(Utility.Random(5,7), SpellType.Wizard | SpellType.Sorcerer, 0);
			base.OnAfterSpawn();
		}

		public override void OnDeath(Container c)
		{
		    base.OnDeath(c);
			if (Utility.RandomDouble() < 0.08)
    		{
    		    c.DropItem(new EssenceOfLolthsHatred());
    		}
		
		    BossLootSystem.BossEnchant(this, c, 500, 15, 1, "DrowMage");
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

		public DrowMageLord( Serial serial ) : base( serial ) 
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
				this.MobileMagics(Utility.Random(5,7), SpellType.Wizard | SpellType.Sorcerer, 0);
			} 
		} 
	} 
}