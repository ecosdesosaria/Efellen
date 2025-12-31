using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Regions;

namespace Server.Mobiles
{
	[CorpseName( "a wolf corpse" )]
	[TypeAlias( "Server.Mobiles.BlackWolf" )]
	public class BlackWolf : BaseMount
	{
		[Constructable]
		public BlackWolf() : this( "a black wolf" )
		{
		}

		[Constructable]
		public BlackWolf( string name ) : base( name, 277, 16017, AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4 )
		{
			Name = "a black wolf";
			BaseSoundID = 0xE5;
			Hue = 0xB3A;

			SetStr( 56, 80 );
			SetDex( 56, 75 );
			SetInt( 31, 55 );

			SetHits( 34, 48 );
			SetMana( 0 );

			SetDamage( 3, 7 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 15, 20 );
			SetResistance( ResistanceType.Fire, 10, 15 );
			SetResistance( ResistanceType.Cold, 20, 25 );
			SetResistance( ResistanceType.Poison, 10, 15 );
			SetResistance( ResistanceType.Energy, 10, 15 );

			SetSkill( SkillName.MagicResist, 20.1, 35.0 );
			SetSkill( SkillName.Tactics, 45.1, 60.0 );
			SetSkill( SkillName.FistFighting, 45.1, 60.0 );

			Fame = 450;
			Karma = 0;

			VirtualArmor = 16;

			Tamable = true;
			ControlSlots = 1;
			MinTameSkill = 65.1;
		}

		public override int Meat{ get{ return 1; } }
		public override int Hides{ get{ return 6; } }
		public override int Cloths{ get{ return 3; } }
		public override ClothType ClothType{ get{ return ClothType.Furry; } }
		public override FoodType FavoriteFood{ get{ return FoodType.Meat; } }
		public override PackInstinct PackInstinct{ get{ return PackInstinct.Canine; } }

		public BlackWolf( Serial serial ) : base( serial )
		{
		}

		private bool IsFriendlyCreature(Mobile m)
		{
			Region reg = Region.Find( this.Location, this.Map );
			return (reg.IsPartOf( "The Howling Grove" ) && (
					m is FiorinTheArchdruid ||
					m is GuardianPanda || 
			       	m is GuardianWolf || 
			       	m is BlackWolf || 
			       	m is DeepWoodSniper || 
			       	m is DruidOfTheHowlingOrder || 
			       	m is WereWolf));
		}

		public override bool IsEnemy( Mobile m )
	    {
			if (m == null || m.Deleted)
	        	return false;
			
			if (IsFriendlyCreature(m))
		    	return false;
			
			if (m is BaseCreature && ((BaseCreature)m).ControlMaster == null )
			{
				this.Location = m.Location;
				this.Combatant = m;
				this.Warmode = true;
			}
			
			return true;
	    }

		public override void OnDeath(Container c)
		{
		    base.OnDeath(c);

		    Mobile killer = this.LastKiller;

		    TotemDropHelper.TryDropTotem(
		        killer,
		        this,
		        "Worg",
		        105.0,
		        0.10
		    );
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

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int) 0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
			Hue = 0xB3A;
		}
	}
}