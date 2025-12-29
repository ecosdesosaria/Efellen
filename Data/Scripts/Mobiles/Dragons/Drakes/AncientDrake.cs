using System;
using Server;
using Server.Items;
using Server.Custom.DailyBosses.System;

namespace Server.Mobiles
{
	[CorpseName( "a drake corpse" )]
	public class AncientDrake : BaseCreature
	{
		public override bool ReacquireOnMovement{ get{ return !Controlled; } }
		private DateTime m_NextSpecialAttack = DateTime.MinValue;

		public override WeaponAbility GetWeaponAbility()
		{
			return WeaponAbility.BleedAttack;
		}

		[Constructable]
		public AncientDrake () : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "an ancient drake";
			Body = 338;
			BaseSoundID = 362;
			Resource = CraftResource.RedScales;

			SetStr( 601, 630 );
			SetDex( 233, 252 );
			SetInt( 301, 340 );

			SetHits( 541, 558 );

			SetDamage( 18, 26 );

			SetDamageType( ResistanceType.Physical, 80 );
			SetDamageType( ResistanceType.Fire, 20 );

			SetResistance( ResistanceType.Physical, 65, 70 );
			SetResistance( ResistanceType.Fire, 70, 80 );
			SetResistance( ResistanceType.Cold, 50, 60 );
			SetResistance( ResistanceType.Poison, 30, 40 );
			SetResistance( ResistanceType.Energy, 40, 50 );

			SetSkill( SkillName.MagicResist, 85.1, 100.0 );
			SetSkill( SkillName.Tactics, 85.1, 110.0 );
			SetSkill( SkillName.FistFighting, 85.1, 100.0 );

			Fame = 9500;
			Karma = -9500;

			VirtualArmor = 50;

			Tamable = true;
			ControlSlots = 2;
			MinTameSkill = 94.3;

			PackReg( 9 );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Rich );
			AddLoot( LootPack.FilthyRich );
			AddLoot( LootPack.MedScrolls, 2 );
		}

		public override int TreasureMapLevel{ get{ return 4; } }
		public override int Meat{ get{ return 10; } }
		public override int Hides{ get{ return 20; } }
		public override HideType HideType{ get{ return HideType.Draconic; } }
		public override int Scales{ get{ return 3; } }
		public override ScaleType ScaleType{ get{ return ResourceScales(); } }
		public override FoodType FavoriteFood{ get{ return FoodType.Meat | FoodType.Fish; } }

		public AncientDrake( Serial serial ) : base( serial )
		{
		}

		public override void OnDamage( int amount, Mobile from, bool willKill )
		{
			if ( DateTime.UtcNow >= m_NextSpecialAttack && Utility.RandomDouble() < 0.25 )
			{
				PerformRageAttack( from );
				m_NextSpecialAttack = DateTime.UtcNow + TimeSpan.FromSeconds( 30 );
			}
			
			base.OnDamage( amount, from, willKill );
		}

		private void PerformRageAttack( Mobile target )
		{
			if ( target == null || target.Deleted || !target.Alive )
				return;

			Map map = this.Map;

			BossSpecialAttack.PerformConeBreath(
			    boss: this,
			    target: target,
			    warcry: "*exhales devastating fire!*",
			    hue: 1160,
			    rage: 1,
			    range: 3, 
				physicalDmg:0,
			    fireDmg: 100
			);
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 1 );
			writer.Write( m_NextSpecialAttack );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
			if ( version >= 1 )
			{
				m_NextSpecialAttack = reader.ReadDateTime();
			}
		}
	}
}