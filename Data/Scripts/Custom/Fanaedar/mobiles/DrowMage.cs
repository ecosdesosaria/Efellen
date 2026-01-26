using System;
using Server;
using Server.Misc;
using Server.Items;
using Server.CustomSpells;

namespace Server.Mobiles 
{ 
	[CorpseName( "a mage corpse" )] 
	public class DrowMage : BaseSpellCaster 
	{ 
		[Constructable] 
		public DrowMage() : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 ) 
		{ 
			
			Body = 605; 
			Name = NameList.RandomName( "dark_elf_prefix_male" )  +" "+  NameList.RandomName( "dark_elf_suffix_male" );
			switch ( Utility.RandomMinMax( 0, 5 ) )
			{
				case 0: Title = "the Drow Wizard"; break;
				case 1: Title = "the Drow Sorcerer"; break;
				case 2: Title = "the Drow Mage"; break;
				case 3: Title = "the Drow Conjurer"; break;
				case 4: Title = "the Drow Magician"; break;
				case 5: Title = "the Drow Warlock"; break;
			}
			Utility.AssignRandomHair( this );
			HairHue = 1150;
			Hue = 1316;
			EmoteHue = 1;

			Server.Misc.IntelligentAction.DressUpWizards( this, false );

			SetStr( 121, 155 );
			SetDex( 131, 165 );
			SetInt( 196, 220 );

			SetHits( 249, 303 );

			SetDamage( 9, 14 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 25 );
			SetResistance( ResistanceType.Fire, 30 );
			SetResistance( ResistanceType.Poison, 30 );
			SetResistance( ResistanceType.Energy, 40 );
			SetResistance( ResistanceType.Cold, 30, 40 );

			SetSkill( SkillName.Psychology, 105.0 );
			SetSkill( SkillName.Magery, 110.0 );
			SetSkill( SkillName.Meditation, 50.0 );
			SetSkill( SkillName.MagicResist, 115.5 );
			SetSkill( SkillName.Tactics, 87.5 );
			SetSkill( SkillName.FistFighting, 50.0 );
			SetSkill( SkillName.Bludgeoning, 70.0 );

			Fame = 9000;
			Karma = -9000;

			VirtualArmor = 30;
			PackReg( Utility.RandomMinMax( 5, 15 ) );

			if ( 0.8 > Utility.RandomDouble() )
				PackItem( new ArcaneGem() );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Rich, 2 );
			AddLoot( LootPack.MedScrolls, 2 );
			AddLoot( LootPack.MedPotions );
		}

		public override bool ClickTitle{ get{ return false; } }
		public override bool ShowFameTitle{ get{ return false; } }
		public override bool CanRummageCorpses{ get{ return true; } }
		public override bool AlwaysAttackable{ get{ return true; } }
		public override int Meat{ get{ return 1; } }
		public override int TreasureMapLevel{ get{ return Core.AOS ? 1 : 0; } }
		public override int Skeletal{ get{ return Utility.Random(3); } }
		public override SkeletalType SkeletalType{ get{ return SkeletalType.Drow; } }

		public override void OnAfterSpawn()
		{
			Server.Misc.IntelligentAction.BeforeMyBirth( this );
			this.MobileMagics(Utility.Random(3,6), SpellType.Wizard | SpellType.Sorcerer, 0);
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

		public DrowMage( Serial serial ) : base( serial )
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
			this.MobileMagics(Utility.Random(3,6), SpellType.Wizard | SpellType.Sorcerer, 0);
		}
	}
}