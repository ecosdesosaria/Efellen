using System;
using Server;
using Server.Items;
using Server.Gumps;
using Server.Regions;
using Server.Misc;

namespace Server.Mobiles 
{ 
	[CorpseName( "a Seraph's corpse" )] 
	public class EtherealWarriorGeneral : BaseCreature 
	{ 
		public override bool InitialInnocent{ get{ return true; } }

		[Constructable] 
		public EtherealWarriorGeneral() : base( AIType.AI_Mage, FightMode.Evil, 10, 1, 0.2, 0.4 ) 
		{ 
			Name = "Seraph";
			NameHue = 0x0672;
			Hue = 0x0672;
			Body = 0x9e;
			BaseSoundID = 466;

			SetStr( 586, 785 );
			SetDex( 177, 255 );
			SetInt( 351, 450 );

			SetHits( 352, 471 );

			SetDamage( 13, 19 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 80, 90 );
			SetResistance( ResistanceType.Fire, 40, 50 );
			SetResistance( ResistanceType.Cold, 40, 50 );
			SetResistance( ResistanceType.Poison, 40, 50 );
			SetResistance( ResistanceType.Energy, 40, 50 );

			SetSkill( SkillName.Anatomy, 79.1, 97.0 );
			SetSkill( SkillName.Psychology, 90.1, 100.0 );
			SetSkill( SkillName.Magery, 99.1, 100.0 );
			SetSkill( SkillName.Meditation, 90.1, 100.0 );
			SetSkill( SkillName.MagicResist, 90.1, 100.0 );
			SetSkill( SkillName.Tactics, 90.1, 100.0 );
			SetSkill( SkillName.FistFighting, 97.6, 100.0 );

			Fame = 7000;
			Karma = 7000;

			VirtualArmor = 70;
		}

		public override int TreasureMapLevel{ get{ return Core.AOS ? 5 : 0; } }

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Rich, 3 );
			AddLoot( LootPack.Gems );
		}

		private DateTime m_NextResurrect;
		private static TimeSpan ResurrectDelay = TimeSpan.FromSeconds(2.0);

		public override int Feathers{ get{ return 100; } }

		public override int GetAngerSound()
		{
			return 0x2F8;
		}

		public override int GetIdleSound()
		{
			return 0x2F8;
		}

		public override int GetAttackSound()
		{
			return Utility.Random( 0x2F5, 2 );
		}

		public override int GetHurtSound()
		{
			return 0x2F9;
		}

		public override int GetDeathSound()
		{
			return 0x2F7;
		}

		public override void OnGaveMeleeAttack( Mobile defender )
		{
			base.OnGaveMeleeAttack( defender );

			defender.Damage( Utility.Random( 10, 10 ), this );
			defender.Stam -= Utility.Random( 10, 10 );
			defender.Mana -= Utility.Random( 10, 10 );
		}

		public override void OnGotMeleeAttack( Mobile attacker )
		{
			base.OnGotMeleeAttack( attacker );

			attacker.Damage( Utility.Random( 10, 10 ), this );
			attacker.Stam -= Utility.Random( 10, 10 );
			attacker.Mana -= Utility.Random( 10, 10 );
		}

		public EtherealWarriorGeneral( Serial serial ) : base( serial ) 
		{ 
		}
		
		private bool IsFriendlyCreature(Mobile m)
		{
			Region reg = Region.Find( this.Location, this.Map );
			return (reg.IsPartOf( "Castle Griffin Roost" ) && (
					m is HeavenlyMarshall || 
					m is Angel || 
					m is Archangel ||
					m is SkyKnight || 
					m is WarGriffon || 
					m is EtherealWarriorGeneral));
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