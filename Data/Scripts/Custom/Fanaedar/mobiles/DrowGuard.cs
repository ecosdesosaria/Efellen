using System;
using System.Collections;
using Server.Items;
using Server.ContextMenus;
using Server.Misc;
using Server.Network;
using Server.Regions;
using Server.Custom;

namespace Server.Mobiles
{
	public class DrowGuard : BaseCreature
	{
		[Constructable]
		public DrowGuard() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			SpeechHue = Utility.RandomTalkHue();
			Title = "the Drow Blackguard";
			Hue = 1316;
			Body = 605;
			Name = NameList.RandomName( "dark_elf_prefix_male" )  +" "+  NameList.RandomName( "dark_elf_suffix_male" );
			Utility.AssignRandomHair( this );
			HairHue = 1150;
			
			SetStr( 286, 420 );
			SetDex( 281, 295 );
			SetInt( 161, 175 );

			SetDamage( 18, 23 );

			SetSkill( SkillName.Fencing, 117.5 );
			SetSkill( SkillName.Bludgeoning, 117.5 );
			SetSkill( SkillName.MagicResist, 145.5 );
			SetSkill( SkillName.Swords, 117.5 );
			SetSkill( SkillName.Tactics, 117.5 );
			SetSkill( SkillName.Parry, 117.5 );
			SetSkill( SkillName.Anatomy, 117.5 );
			SetSkill( SkillName.FistFighting, 117.5 );

			Fame = 16000;
			Karma = -16000;
		}

		public override void OnDeath(Container c)
		{
		    base.OnDeath(c);
		    BossLootSystem.BossEnchant(this, c, 500, 10, 1, "DrowBlackGuard");
			if (Utility.RandomDouble() < 0.05)
    		{
    		    c.DropItem(new EssenceOfLolthsHatred());
    		}
		}

		public override bool ClickTitle{ get{ return false; } }
		public override bool ShowFameTitle{ get{ return false; } }
		public override bool CanRummageCorpses{ get{ return true; } }
		public override bool AlwaysAttackable{ get{ return true; } }
		public override int Meat{ get{ return 1; } }
		public override int Skeletal{ get{ return Utility.Random(8); } }
		public override SkeletalType SkeletalType{ get{ return SkeletalType.Drow; } }

		private void EquipDrowGuardGear()
		{
		    int hue = Utility.RandomDrowHue();
		
		    switch (Utility.Random(3))
		    {
		        case 0:
		            AddItem(new RingmailArms { Hue = hue });
		            AddItem(new RingmailChest { Hue = hue });
		            AddItem(new RingmailGloves { Hue = hue });
		            AddItem(new RingmailSkirt { Hue = hue });
		            AddItem(new RoyalGorget { Hue = hue });
		            break;
		
		        case 1:
		            AddItem(new ScaledChest { Hue = hue });
		            AddItem(new ScaledGloves { Hue = hue });
		            AddItem(new ScaledGorget { Hue = hue });
		            AddItem(new ScaledLegs { Hue = hue });
		            AddItem(new ScaledArms { Hue = hue });
		            break;
		
		        case 2:
		            AddItem(new PlateArms { Hue = hue });
		            AddItem(new PlateChest { Hue = hue });
		            AddItem(new PlateGloves { Hue = hue });
		            AddItem(new PlateGorget { Hue = hue });
		            AddItem(new PlateHelm { Hue = hue });
		            AddItem(new PlateLegs { Hue = hue });
		            break;
		    }
		
		    Item weapon = null;
		
		    switch (Utility.Random(3))
		    {
		        case 0: weapon = new Longsword(); break;
		        case 1: weapon = new Broadsword(); break;
		        case 2: weapon = new ElvenSpellblade(); break;
		    }
		
		    Item shield = CreateRandomGuardShield();
		
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
		        Cloak cloak = new Cloak();
		        cloak.Hue = Utility.RandomDrowHue();
		        AddItem(cloak);
		    }
		
		    LeatherSoftBoots boots = new LeatherSoftBoots();
		    boots.Hue = Utility.RandomDrowHue();
		    AddItem(boots);
		}


		private Item CreateRandomGuardShield()
		{
		    switch (Utility.Random(8))
		    {
		        case 0: return new BronzeShield();
		        case 3: return new HeaterShield();
		        case 4: return new JeweledShield();
		        case 5: return new MetalShield();
		        case 6: return new RoyalShield();
		        default: return new WoodenKiteShield();
		    }
		}
		public override void OnAfterSpawn()
		{
			EquipDrowGuardGear();
			base.OnAfterSpawn();
		}

		public override void OnGotMeleeAttack( Mobile attacker )
		{
			base.OnGotMeleeAttack( attacker );
			Server.Misc.IntelligentAction.CryOut( this );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Rich, 1 );
		}

		public DrowGuard( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}