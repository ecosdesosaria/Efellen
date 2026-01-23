using System;
using Server;
using Server.Misc;
using Server.Items;
using Server.CustomSpells;
using System.Net;

namespace Server.Mobiles 
{ 
	[CorpseName( "a drow priestess' corpse" )] 
	public class DrowPriestess : BaseSpellCaster 
	{ 
	
		[Constructable] 
		public DrowPriestess() : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 ) 
		{
			Body = 606; 
			Name = NameList.RandomName( "dark_elf_prefix_female" ) + NameList.RandomName( "dark_elf_suffix_female" );
			Title = "the Drow Priestess";
			Utility.AssignRandomHair( this );
			HairHue = 1150;
			Hue = 1316;
			EmoteHue = 11;

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

		private void EquipDrowPriestessGear()
		{
		    int hue = Utility.RandomDrowHue();
		
		    switch (Utility.Random(4))
		    {
		        case 0:
		            AddItem(new FemaleLeatherChest { Hue = hue });
		            AddItem(new LeatherBustierArms { Hue = hue });
		            AddItem(new LeatherGloves { Hue = hue });
		            AddItem(new LeatherLegs { Hue = hue });
		            AddItem(new LeatherGorget { Hue = hue });
		            break;
		
		        case 1:
		            AddItem(new RingmailArms { Hue = hue });
		            AddItem(new RingmailChest { Hue = hue });
		            AddItem(new RingmailGloves { Hue = hue });
		            AddItem(new RingmailSkirt { Hue = hue });
		            AddItem(new RoyalGorget { Hue = hue });
		            break;
		
		        case 2:
		            AddItem(new ScaledChest { Hue = hue });
		            AddItem(new ScaledGloves { Hue = hue });
		            AddItem(new ScaledGorget { Hue = hue });
		            AddItem(new ScaledLegs { Hue = hue });
		            AddItem(new ScaledArms { Hue = hue });
		            break;
		
		        case 3:
		            AddItem(new FemaleStuddedChest { Hue = hue });
		            AddItem(new StuddedBustierArms { Hue = hue });
		            AddItem(new StuddedGloves { Hue = hue });
		            AddItem(new StuddedGorget { Hue = hue });
		            AddItem(new StuddedSkirt { Hue = hue });
		            break;
		    }
		
		    Item weapon = null;
		    Item shield = null;
		
		    switch (Utility.Random(4))
		    {
		        case 0:
		            weapon = new Whips();
		            break;
		
		        case 1:
		            weapon = new DiamondMace();
		            shield = CreateRandomShield();
		            break;
		
		        case 2:
		            weapon = new WarMace();
		            shield = CreateRandomShield();
		            break;
		
		        case 3:
		            weapon = new SpikedClub();
		            shield = CreateRandomShield();
		            break;
		    }
		
		    if (weapon != null)
		    {
		        weapon.Hue = hue;
		        AddItem(weapon);
		    }
		
		    if (shield != null)
		    {
		        shield.Hue = hue;
		        AddItem(shield);
		    }
		
		    if (Utility.RandomDouble() < 0.25)
		    {
		        Item cloak = null;
		
		        switch (Utility.Random(4))
		        {
		            case 0: cloak = new Cloak(); break;
		            case 2: cloak = new RoyalSkirt(); break;
		            case 3: cloak = new RoyalLongSkirt(); break;
		        }
		
        		if (cloak != null)
        		{
        		    cloak.Hue = Utility.RandomDrowHue();
        		    AddItem(cloak);
        		}
	    	}

		    if (Utility.RandomDouble() < 0.55)
		    {
		        Item robe = null;

		        switch (Utility.Random(3))
		        {
		            case 0: robe = new AssassinRobe(); break;
		            case 1: robe = new ChaosRobe(); break;
		            case 2: robe = new SpiderRobe(); break;
		        }

		        if (robe != null)
		        {
		            robe.Hue = Utility.RandomDrowHue();
		            AddItem(robe);
		        }
		    }

		    Item boots = new LeatherThighBoots();
		    boots.Hue = Utility.RandomDrowHue();
		    AddItem(boots);
		}

		private Item CreateRandomShield()
		{
		    switch (Utility.Random(8))
		    {
		        case 0: return new BronzeShield();
		        case 1: return new ChaosShield();
		        case 2: return new DarkShield();
		        case 3: return new HeaterShield();
		        case 4: return new JeweledShield();
		        case 5: return new MetalShield();
		        case 6: return new RoyalShield();
		        default: return new WoodenKiteShield();
		    }
		}


		public override void OnAfterSpawn()
		{
			this.MobileMagics(Utility.Random(5,7), SpellType.Cleric, 0);
    		EquipDrowPriestessGear();

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