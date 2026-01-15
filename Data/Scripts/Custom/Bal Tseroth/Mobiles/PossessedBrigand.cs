using System;
using System.Collections;
using Server.Items;
using Server.ContextMenus;
using Server.Misc;
using Server.Network;
using Server.Regions;
using Server.Custom.BalTsareth;

namespace Server.Mobiles
{
	public class PossessedBrigand : BaseCreature
	{
		[Constructable]
		public PossessedBrigand() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			SpeechHue = Utility.RandomTalkHue();
			Hue = Utility.RandomSkinColor();

			if ( this.Female = Utility.RandomBool() )
			{
				Body = 0x191;
				Name = NameList.RandomName( "female" );
				Utility.AssignRandomHair( this );
				HairHue = Utility.RandomHairHue();
			}
			else
			{
				Body = 0x190;
				Name = NameList.RandomName( "male" );
				Utility.AssignRandomHair( this );
				int HairColor = Utility.RandomHairHue();
				FacialHairItemID = Utility.RandomList( 0, 8254, 8255, 8256, 8257, 8267, 8268, 8269 );
				HairHue = HairColor;
				FacialHairHue = HairColor;
			}

			SetStr( 86, 100 );
			SetDex( 81, 95 );
			SetInt( 61, 75 );

			SetDamage( 10, 23 );

			SetSkill( SkillName.Fencing, 66.0, 97.5 );
			SetSkill( SkillName.Bludgeoning, 65.0, 87.5 );
			SetSkill( SkillName.MagicResist, 25.0, 47.5 );
			SetSkill( SkillName.Swords, 65.0, 87.5 );
			SetSkill( SkillName.Tactics, 65.0, 87.5 );
			SetSkill( SkillName.FistFighting, 15.0, 37.5 );

			Fame = 1000;
			Karma = -1000;
			if ( 0.3 > Utility.RandomDouble() )
				PackItem( new EerieIdol() );
		}

		public override bool ClickTitle{ get{ return false; } }
		public override bool ShowFameTitle{ get{ return false; } }
		public override bool CanRummageCorpses{ get{ return true; } }
		public override bool AlwaysAttackable{ get{ return true; } }
		public override int Meat{ get{ return 1; } }
		public override int Skeletal{ get{ return Utility.Random(3); } }
		public override SkeletalType SkeletalType{ get{ return SkeletalType.Brittle; } }

		public override void OnAfterSpawn()
		{
			Server.Misc.IntelligentAction.DressUpRogues( this, "", true, false, false );
			
			Item[] items = new Item[] { this.FindItemOnLayer( Layer.InnerTorso ), this.FindItemOnLayer( Layer.OuterTorso ), 
				this.FindItemOnLayer( Layer.Pants ), this.FindItemOnLayer( Layer.Arms ), this.FindItemOnLayer( Layer.Gloves ), 
				this.FindItemOnLayer( Layer.Helm ), this.FindItemOnLayer( Layer.Neck ), this.FindItemOnLayer( Layer.Waist ), 
				this.FindItemOnLayer( Layer.OneHanded ), this.FindItemOnLayer( Layer.TwoHanded ), this.FindItemOnLayer( Layer.FirstValid ) };

			foreach ( Item item in items )
			{
				if ( item != null )
					item.Hue = 0x0213;
			}
			
			base.OnAfterSpawn();

			
		}

		public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);
			if(Utility.RandomDouble() < 0.15)
            {
				Say(Server.Custom.BalTsareth.BalTsarethSpeech.GetAttackLine(defender));              
            }
        }

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Average );
		}

		public PossessedBrigand( Serial serial ) : base( serial )
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