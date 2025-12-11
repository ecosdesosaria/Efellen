using System;
using Server;
using System.Collections;
using System.Collections.Generic;
using Server.Targeting;
using Server.Items;
using Server.Network;
using Server.ContextMenus;
using Server.Gumps;
using Server.Misc;
using Server.Mobiles;

namespace Server.Mobiles
{
	[CorpseName( "a griffon corpse" )]
	public class WarGriffon : BaseMount
	{
		public override WeaponAbility GetWeaponAbility()
		{
			return WeaponAbility.BleedAttack;
		}

		[Constructable]
		public WarGriffon() : this( "a trained griffon" )
		{
		}

		[Constructable]
		public WarGriffon( string name ) : base( name, 0x31F, 0x3EBE, AIType.AI_Animal, FightMode.Evil, 10, 1, 0.2, 0.4 )
		{
			BaseSoundID = 0x2EE;
			Hue = 0x0672;

			SetStr( 296, 320 );
			SetDex( 286, 310 );
			SetInt( 151, 175 );

			SetHits( 358, 422 );

			SetDamage( 16, 21 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 45, 60 );
			SetResistance( ResistanceType.Fire, 30, 40 );
			SetResistance( ResistanceType.Cold, 30, 60 );
			SetResistance( ResistanceType.Poison, 30, 40 );
			SetResistance( ResistanceType.Energy, 30, 50 );

			SetSkill( SkillName.MagicResist, 80.1, 95.0 );
			SetSkill( SkillName.Tactics, 90.1, 110.0 );
			SetSkill( SkillName.FistFighting, 90.1, 112.0 );

			Fame = 9500;
			Karma = 8500;
			Team = 777;
			VirtualArmor = 52;
		}

		public override void OnDamage(int amount, Mobile from, bool willKill)
		{
		    base.OnDamage(amount, from, willKill);

		    if (from.Player && from.Kills < 5 && !from.Criminal) 
				from.Criminal = true;
		}

		public override bool IsEnemy( Mobile m )
	    {
			if (m == null || m.Deleted)
	        	return false;
			
			if (m is HeavenlyMarshall || m is SkyKnight || m is GriffonRiding || m is WarGriffon || m is EtherealWarriorGeneral)
		    	return false;

			if ( !IntelligentAction.GetMyEnemies( m, this, true ) )
				return false;

			if ( m.Region != this.Region )
				return false;

			if (m is BaseCreature && ((BaseCreature)m).ControlMaster == null )
			{
				this.Location = m.Location;
				this.Combatant = m;
				this.Warmode = true;
			}
	  		return true;
	    }

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Rich );
		}

		public override int Meat{ get{ return 12; } }
		public override MeatType MeatType{ get{ return MeatType.Bird; } }
		public override int Feathers{ get{ return 50; } }
		public override int Skeletal{ get{ return Utility.Random(4); } }
		public override SkeletalType SkeletalType{ get{ return SkeletalType.Mystical; } }
		public override bool AlwaysMurderer { get { return false; } }

		public override void AggressiveAction(Mobile m, bool criminal)
		{
			if (m is HeavenlyMarshall || m is SkyKnight || m is GriffonRiding || m is WarGriffon || m is EtherealWarriorGeneral)
				return;

		    base.AggressiveAction(m, true);
		}

		public override bool CanBeHarmful(Mobile m, bool message, bool ignoreOurBlessedness)
		{
		    if (m is HeavenlyMarshall || m is SkyKnight || m is GriffonRiding || m is WarGriffon || m is EtherealWarriorGeneral)
		        return false;

		    return base.CanBeHarmful(m, message, ignoreOurBlessedness);
		}

		public override bool CanBeBeneficial(Mobile m, bool message, bool allowDead)
		{
		     if (m is HeavenlyMarshall || m is SkyKnight || m is GriffonRiding || m is WarGriffon || m is EtherealWarriorGeneral)
		        return true;

		    return base.CanBeBeneficial(m, message, allowDead);
		}


		public WarGriffon( Serial serial ) : base( serial )
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