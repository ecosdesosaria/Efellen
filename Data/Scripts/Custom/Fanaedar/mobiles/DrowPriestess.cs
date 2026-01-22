using System;
using Server;
using Server.Misc;
using Server.Items;
using Server.CustomSpells;

namespace Server.Mobiles 
{ 
	[CorpseName( "a drow priestess' corpse" )] 
	public class DrowPriestess : BaseSpellCaster 
	{ 
	
		[Constructable] 
		public DrowPriestess() : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 ) 
		{
			Body = 0x191; 
			Name = NameList.RandomName( "elf_female" );
			Title = "the Drow Priestess";
			Utility.AssignRandomHair( this );
			HairHue = 1150;
			Hue = 1316;
			EmoteHue = 11;

			Server.Misc.IntelligentAction.DressUpWizards( this, false );

			SetStr( 221, 325 );
			SetDex( 261, 285 );
			SetInt( 276, 300 );

			SetHits( 449, 563 );

			SetDamage( 15, 21 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 35, 60 );
			SetResistance( ResistanceType.Fire, 30, 40 );
			SetResistance( ResistanceType.Cold, 30, 40 );
			SetResistance( ResistanceType.Poison, 30, 40 );
			SetResistance( ResistanceType.Energy, 30, 40 );

			SetSkill( SkillName.Psychology, 100.0 );
			SetSkill( SkillName.Magery, 115.0 );
			SetSkill( SkillName.Meditation, 70.0 );
			SetSkill( SkillName.MagicResist, 125.0 );
			SetSkill( SkillName.Tactics, 107.5 );
			SetSkill( SkillName.FistFighting, 76.0 );
			SetSkill( SkillName.Bludgeoning, 99.0 );

			Fame = 16500;
			Karma = -16500;

			VirtualArmor = 55;
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Rich );
			AddLoot( LootPack.HighScrolls );
			AddLoot( LootPack.MedScrolls, 2 );
			AddLoot( LootPack.HighPotions );
		}

		public override bool ClickTitle{ get{ return false; } }
		public override bool ShowFameTitle{ get{ return false; } }
		public override bool CanRummageCorpses{ get{ return true; } }
		public override bool AlwaysAttackable{ get{ return true; } }
		public override int Meat{ get{ return 1; } }
		public override int TreasureMapLevel{ get{ return Core.AOS ? 2 : 0; } }
		public override int Skeletal{ get{ return Utility.Random(8); } }
		public override SkeletalType SkeletalType{ get{ return SkeletalType.Drow; } }

		public override void OnAfterSpawn()
		{
			Server.Misc.IntelligentAction.BeforeMyBirth( this );
			this.MobileMagics(Utility.Random(5,7), SpellType.Cleric, 0);
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

		public DrowPriestess( Serial serial ) : base( serial ) 
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
				this.MobileMagics(Utility.Random(5,7), SpellType.Cleric, 0);
			} 
		} 
	} 
}