using System;
using Server;
using Server.Items;
using System.Collections;
using Server.Misc;
using Server.CustomSpells;

namespace Server.Mobiles
{
	[CorpseName( "a kobold corpse" )]
	public class KoboldMage : BaseSpellCaster
	{
		public override InhumanSpeech SpeechType{ get{ return InhumanSpeech.Ratman; } }

		[Constructable]
		public KoboldMage() : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a kobold shaman";
			Body = 253;
			BaseSoundID = 0x543;

			SetStr( 61, 75 );
			SetDex( 41, 50 );
			SetInt( 36, 55 );

			SetHits( 36, 48 );

			SetDamage( 3, 8 );

			SetDamageType( ResistanceType.Physical, 0 );
			SetDamageType( ResistanceType.Fire, 50 );
			SetDamageType( ResistanceType.Poison, 50 );

			SetResistance( ResistanceType.Physical, 25, 35 );
			SetResistance( ResistanceType.Fire, 40, 50 );
			SetResistance( ResistanceType.Cold, 20, 30 );
			SetResistance( ResistanceType.Poison, 30, 40 );
			SetResistance( ResistanceType.Energy, 30, 40 );

			SetSkill( SkillName.Psychology, 20.1, 30.0 );
			SetSkill( SkillName.Magery, 60.1, 100.0 );
			SetSkill( SkillName.MagicResist, 30.1, 50.0 );
			SetSkill( SkillName.Tactics, 42.1, 50.0 );
			SetSkill( SkillName.FistFighting, 40.1, 44.0 );

			Fame = 35;
			Karma = -35;

			VirtualArmor = 10;
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Meager );
			AddLoot( LootPack.LowScrolls );
			AddLoot( LootPack.LowPotions );
		}

		public override bool OnBeforeDeath()
		{
			if ( Server.Misc.IntelligentAction.HealThySelf( this ) ){ return false; }
			return base.OnBeforeDeath();
		}

		public override int Meat{ get{ return 1; } }
		public override int Hides{ get{ return 1; } }
		public override HideType HideType{ get{ return HideType.Horned; } }

		public override int GetAttackSound(){ return 0x5FD; }	// A
		public override int GetDeathSound(){ return 0x5FE; }	// D
		public override int GetHurtSound(){ return 0x5FF; }		// H

		public override void OnAfterSpawn()
		{
			this.MobileMagics(1, SpellType.Wizard | SpellType.Sorcerer, 0);
			base.OnAfterSpawn();
		}
		public KoboldMage( Serial serial ) : base( serial )
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
			this.MobileMagics(1, SpellType.Wizard | SpellType.Sorcerer, 0);
		}
	}
}