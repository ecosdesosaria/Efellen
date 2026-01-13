using System;
using System.Collections;
using Server.Items;
using Server.ContextMenus;
using Server.Misc;
using Server.Network;

namespace Server.Mobiles
{
	public class SirBradford : BaseCreature
	{
		public override bool ClickTitle{ get{ return false; } }

		[Constructable]
		public SirBradford() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			SpeechHue = Utility.RandomTalkHue();
			Name = "Sir Bradford";
			Hue = Utility.RandomSkinColor();

			Body = 0x190;
			Utility.AssignRandomHair( this );
			FacialHairItemID = Utility.RandomList( 0, 8254, 8255, 8256, 8257, 8267, 8268, 8269 );
			HairHue = Utility.RandomHairHue();
			FacialHairHue = HairHue;

			SetStr( 125 );
			SetDex( 85 );
			SetInt( 65 );
            SetHits( 325 );
			SetDamage( 9, 16 );

			SetSkill( SkillName.Fencing, 66.0, 97.5 );
			SetSkill( SkillName.Bludgeoning, 65.0, 87.5 );
			SetSkill( SkillName.MagicResist, 25.0, 47.5 );
			SetSkill( SkillName.Swords, 107.5 );
			SetSkill( SkillName.Tactics, 107.5 );
			SetSkill( SkillName.FistFighting, 15.0, 37.5 );
            Fame = 2400;
			Karma = -2400;

			AddItem( new Shirt( Utility.RandomColor(0) ));
			AddItem(new RoyalSword() );
		    AddItem(new RoyalArms() );
		    AddItem(new RoyalChest() );
		    AddItem(new RoyalsLegs() );
		    AddItem(new RoyalGorget() );
		    AddItem(new RoyalGloves() );
		    AddItem(new Boots() );
		    AddItem(new RoyalHelm() );
		    AddItem(new RoyalShield() );		
		    AddItem(new Cloak(Utility.RandomColor(0)) );
			
		}

		public override void OnDeath( Container c )
		{
			base.OnDeath( c );

			if ( Utility.RandomMinMax( 1, 6 ) == 1 )
			{
				BaseWeapon shardsplitter = new RoyalSword();
				shardsplitter.Name = "Shardsplitter";
				shardsplitter.SkillBonuses.SetValues( 0, SkillName.ArmsLore, 10 );
				shardsplitter.SkillBonuses.SetValues( 1, SkillName.Tactics, 10 );
				shardsplitter.Attributes.WeaponDamage = 30;
				shardsplitter.Attributes.AttackChance = 10;
				shardsplitter.WeaponAttributes.HitLowerAttack = 30;
				shardsplitter.AccuracyLevel = WeaponAccuracyLevel.Supremely;
				shardsplitter.MinDamage = shardsplitter.MinDamage + 3;
				shardsplitter.MaxDamage = shardsplitter.MaxDamage + 5;
				c.DropItem( shardsplitter );
			}
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Average );
			AddLoot( LootPack.Meager );
		}

		public override bool AlwaysAttackable{ get{ return true; } }
		public override int Skeletal{ get{ return Utility.Random(3); } }
		public override SkeletalType SkeletalType{ get{ return SkeletalType.Brittle; } }

		public SirBradford( Serial serial ) : base( serial )
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