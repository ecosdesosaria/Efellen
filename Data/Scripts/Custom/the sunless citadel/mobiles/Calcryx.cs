using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "Calcryx's corpse" )]
	public class Calcryx : BaseCreature
	{
		public override bool HasBreath{ get{ return true; } }
		public override bool ReacquireOnMovement{ get{ return !Controlled; } }
		public override double BreathEffectDelay{ get{ return 0.1; } }
		public override int GetBreathForm()
		{
		    return 17;
		}

		[Constructable]
		public Calcryx () : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "Calcryx";
			Body = 588;
			Hue = 0x3E1;
			BaseSoundID = 660;

			SetStr( 212 );
			SetDex( 95 );
			SetInt( 86 );

			SetHits( 192 );

			SetDamage( 7, 11 );

			SetDamageType( ResistanceType.Physical, 20 );
			SetDamageType( ResistanceType.Cold, 80 );

			SetResistance( ResistanceType.Physical, 45, 50 );
			SetResistance( ResistanceType.Fire, 0 );
			SetResistance( ResistanceType.Cold, 100 );
			SetResistance( ResistanceType.Poison, 20, 30 );
			SetResistance( ResistanceType.Energy, 30, 40 );

			SetSkill( SkillName.MagicResist, 65.1, 70.0 );
			SetSkill( SkillName.Tactics, 65.1, 70.0 );
			SetSkill( SkillName.FistFighting, 65.1, 70.0 );

			Fame = 2500;
			Karma = -2500;

			VirtualArmor = 15;

			Tamable = true;
			ControlSlots = 1;
			MinTameSkill = 30.3;

			AddItem( new LightSource() );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Average );
			AddLoot( LootPack.Average );
		}

		public override int Meat{ get{ return 2; } }
		public override int Hides{ get{ return 4; } }
		public override HideType HideType{ get{ return HideType.Frozen; } }
		public override int Scales{ get{ return 1; } }
		public override ScaleType ScaleType{ get{ return ScaleType.Blue; } }
		public override FoodType FavoriteFood{ get{ return FoodType.Meat | FoodType.Fish; } }

		public override int GetAttackSound(){ return 0x5E8; }	// A
		public override int GetDeathSound(){ return 0x5E9; }	// D
		public override int GetHurtSound(){ return 0x5EA; }		// H

		public Calcryx( Serial serial ) : base( serial )
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
		}
	}
}