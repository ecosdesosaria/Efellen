using System;
using Server;
using Server.Misc;
using Server.Items;
using Server.CustomSpells;
using Server.Custom;

namespace Server.Mobiles 
{ 
	[CorpseName( "a Poisoner's corpse" )] 
	public class DrowPoisoner : BaseSpellCaster 
	{ 
		[Constructable] 
		public DrowPoisoner() : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 ) 
		{ 
			
			Body = 605; 
			Name = NameList.RandomName( "dark_elf_prefix_male" ) +" "+ NameList.RandomName( "dark_elf_suffix_male" );
			Title = "the Drow Poisoner";
			Utility.AssignRandomHair( this );
			HairHue = 1150;
			Hue = 1316;
			EmoteHue = 1;

			Server.Misc.IntelligentAction.DressUpWizards( this, false );

			SetStr( 165 );
			SetDex( 155 );
			SetInt( 194 );

			SetHits( 309, 363 );

			SetDamage( 9, 14 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 25 );
			SetResistance( ResistanceType.Fire, 30 );
			SetResistance( ResistanceType.Poison, 50 );
			SetResistance( ResistanceType.Energy, 35 );
			SetResistance( ResistanceType.Cold, 30 );

			SetSkill( SkillName.Psychology, 100.0 );
			SetSkill( SkillName.Magery, 105.0 );
			SetSkill( SkillName.Meditation, 50.0 );
			SetSkill( SkillName.MagicResist, 115.5 );
			SetSkill( SkillName.Tactics, 67.5 );
			SetSkill( SkillName.Poisoning, 107.5 );
			SetSkill( SkillName.FistFighting, 50.0 );
			SetSkill( SkillName.Bludgeoning, 70.0 );

			Fame = 9000;
			Karma = -9000;

			VirtualArmor = 30;
		
		}

		public override void OnDeath(Container c)
		{
		    base.OnDeath(c);
		    BossLootSystem.BossEnchant(this, c, 450, 10, 1, "DrowMage");
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Rich, 2 );
			AddLoot( LootPack.MedPotions, 2 );
		}

		public override bool ClickTitle{ get{ return false; } }
		public override bool ShowFameTitle{ get{ return false; } }
		public override bool CanRummageCorpses{ get{ return true; } }
		public override bool AlwaysAttackable{ get{ return true; } }
		public override int Meat{ get{ return 1; } }
		public override int TreasureMapLevel{ get{ return 3; } }
		public override int Skeletal{ get{ return Utility.Random(3); } }
		public override SkeletalType SkeletalType{ get{ return SkeletalType.Drow; } }

		public override void OnAfterSpawn()
		{
			Server.Misc.IntelligentAction.BeforeMyBirth( this );
			this.MobileMagics(Utility.Random(3,6), SpellType.Druid, 0);
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

		public DrowPoisoner( Serial serial ) : base( serial )
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
			this.MobileMagics(Utility.Random(3,6), SpellType.Wizard, 0);
		}
	}
}