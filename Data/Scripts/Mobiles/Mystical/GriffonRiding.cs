using System;
using System.Collections;
using Server.Items;
using Server.Targeting;
using Server.Custom;
using Server.Regions;
using Server.Misc;

namespace Server.Mobiles
{
	[CorpseName( "a griffon corpse" )]
	public class GriffonRiding : BaseMount
	{
		public override WeaponAbility GetWeaponAbility()
		{
			return WeaponAbility.BleedAttack;
		}

		[Constructable]
		public GriffonRiding() : this( "a griffon" )
		{
		}

		[Constructable]
		public GriffonRiding( string name ) : base( name, 0x31F, 0x3EBE, AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4 )
		{
			BaseSoundID = 0x2EE;

			SetStr( 196, 220 );
			SetDex( 186, 210 );
			SetInt( 151, 175 );

			SetHits( 158, 172 );

			SetDamage( 9, 15 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 25, 30 );
			SetResistance( ResistanceType.Fire, 10, 20 );
			SetResistance( ResistanceType.Cold, 10, 30 );
			SetResistance( ResistanceType.Poison, 20, 30 );
			SetResistance( ResistanceType.Energy, 10, 20 );

			SetSkill( SkillName.MagicResist, 50.1, 65.0 );
			SetSkill( SkillName.Tactics, 70.1, 100.0 );
			SetSkill( SkillName.FistFighting, 60.1, 90.0 );

			Fame = 3500;
			Karma = 3500;

			VirtualArmor = 32;

			Tamable = true;
			ControlSlots = 1;
			MinTameSkill = 59.1;
		}

		public override void OnDeath(Container c)
		{
		    base.OnDeath(c);

		    Mobile killer = this.LastKiller;

		    TotemDropHelper.TryDropTotem(
		        killer,
		        this,
		        "Griffon",
		        110.0,
		        0.15
		    );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Meager, 2 );
		}

		public override int Meat{ get{ return 12; } }
		public override MeatType MeatType{ get{ return MeatType.Bird; } }
		public override int Feathers{ get{ return 50; } }
		public override int Skeletal{ get{ return Utility.Random(2); } }
		public override SkeletalType SkeletalType{ get{ return SkeletalType.Mystical; } }

		public GriffonRiding( Serial serial ) : base( serial )
		{
		}

		private bool IsFriendlyCreature(Mobile m)
		{
			return 	m is HeavenlyMarshall || 
					m is SkyKnight || 
					m is Angel || 
					m is Archangel ||
					m is GriffonRiding || 
					m is WarGriffon || 
					m is EtherealWarriorGeneral;
		}

		public override bool IsEnemy( Mobile m )
	    {
			if (m == null || m.Deleted)
	        	return false;
			
			if (IsFriendlyCreature(m))
		    	return false;
			
			if (m.Player && m.Karma >= 0 && m.Combatant != this)
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

		public override void AggressiveAction(Mobile m, bool criminal)
		{
		    if (IsFriendlyCreature(m))
				return;

		    base.AggressiveAction(m, criminal);
		}

		public override bool CanBeHarmful(Mobile m, bool message, bool ignoreOurBlessedness)
		{
		    if (IsFriendlyCreature(m))
		        return false;

		    return base.CanBeHarmful(m, message, ignoreOurBlessedness);
		}

		public override bool CanBeBeneficial(Mobile m, bool message, bool allowDead)
		{
		    if (IsFriendlyCreature(m))
		        return true;

		    return base.CanBeBeneficial(m, message, allowDead);
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