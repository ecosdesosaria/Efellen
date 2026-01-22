using System;
using System.Collections;
using Server.Items;
using Server.ContextMenus;
using Server.Misc;
using Server.Network;
using Server.Regions;

namespace Server.Mobiles
{
	public class DrowGuard : BaseCreature
	{
		[Constructable]
		public DrowGuard() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			SpeechHue = Utility.RandomTalkHue();
			Title = "the Drow Guard";
			Hue = 1316;
			Body = 0x190;
			Name = NameList.RandomName( "elf_male" );
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

		public override bool ClickTitle{ get{ return false; } }
		public override bool ShowFameTitle{ get{ return false; } }
		public override bool CanRummageCorpses{ get{ return true; } }
		public override bool AlwaysAttackable{ get{ return true; } }
		public override int Meat{ get{ return 1; } }
		public override int Skeletal{ get{ return Utility.Random(8); } }
		public override SkeletalType SkeletalType{ get{ return SkeletalType.Drow; } }

		public override void OnAfterSpawn()
		{
			Server.Misc.IntelligentAction.DressUpRogues( this, "", true, false, false );
			base.OnAfterSpawn();
		}

		public override void OnGotMeleeAttack( Mobile attacker )
		{
			base.OnGotMeleeAttack( attacker );
			Server.Misc.IntelligentAction.CryOut( this );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Rich );
			AddLoot( LootPack.Rich );
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