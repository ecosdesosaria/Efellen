using System;
using Server;
using System.Collections;
using System.Collections.Generic;
using Server.Items;
using Server.ContextMenus;
using Server.Misc;
using Server.Network;
using Server.Mobiles;
using Server.Commands;
using Server.Commands.Generic;
using Server.Spells.Necromancy;
using Server.Spells;
using Server.EffectsUtil;

namespace Server.Mobiles
{
	[CorpseName( "Mother Superior's Corpse" )]
	public class MotherSuperior : BaseCreature
	{
		private int m_Rage = 0;
		private Mobile m_LastTarget;
		private DateTime m_NextSpecialAttack = DateTime.MinValue;
        
		[Constructable]
		public MotherSuperior () : base( AIType.AI_Mage, FightMode.Evil, 20, 1, 0.4, 0.8 )
		{
			Title = " the Mother Superior";
			NameHue = 0x92E;
            Body = 401; 
			Name = NameList.RandomName( "female" );
			Utility.AssignRandomHair( this );
			HairHue = Utility.RandomHairHue();
            Hue = Utility.RandomSkinHue(); 

			SetStr( 296, 385 );
			SetDex( 95, 125 );
			SetInt( 186, 225 );

			SetHits( 5555 );
			SetDamage( 14, 24 );

			SetDamageType( ResistanceType.Physical, 70 );
			SetResistance( ResistanceType.Fire, 45 );
			SetResistance( ResistanceType.Cold, 45 );
			SetResistance( ResistanceType.Poison, 70 );
			SetResistance( ResistanceType.Energy, 55 );

            SetSkill( SkillName.Magery, 82.5, 125.0 );
            SetSkill( SkillName.Psychology, 52.5, 85.0 );
			SetSkill( SkillName.Meditation, 82.5, 95.0 );
			SetSkill( SkillName.MagicResist, 75.5, 125.0 );
			SetSkill( SkillName.Tactics, 81.0, 95.0 );
			SetSkill( SkillName.FistFighting, 101.0, 115.0 );

			Fame = 13000;
			Karma = 15000;

			VirtualArmor = 25;
            AddItem( new NunRobe( ) );
			AddItem( new LightCitizen( true ) );

		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.UltraRich, 2 );
		}

		private static readonly string[] AttackLines = new string[]
		{
		    "We have no wealth for you to take!",
		    "Thou shall not harm my sisters, {0}!",
		    "{0} is here, escape if you can!",
            "Why do you bring violence to our sanctuary?",
            "Sisters, pray for strength!",
            "{0}, the heavens weep for you!",
            "Protect the patients!",
            "Your heart is clouded, {0}!",
            "Repent before it is too late!",
            "I shall pray for your soul!",
            "Turn back, {0}! Turn back from this darkness!",
            "You defile sacred ground!",
            "This is a house of healing! Cease at once!",
            "We shall outlast you!"
        };

		public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);
			if(Utility.RandomDouble() < 0.25)
            {
				int i = Utility.Random(AttackLines.Length);
			    Say(string.Format(AttackLines[i], defender.Name));                
            }
        }


		public override int TreasureMapLevel{ get{ return 2; } }
		public override bool CanRummageCorpses{ get{ return false; } }
		public override bool ReacquireOnMovement{ get{ return !Controlled; } }
		public override bool BleedImmune{ get{ return true; } }
		public override bool BardImmune { get { return true; } }
		public override bool Unprovokable { get { return true; } }
		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }
		public override bool AlwaysAttackable{ get{ return true; } }
		public override bool AlwaysMurderer { get { return false; } }

		public override bool IsEnemy( Mobile m )
	    {
			if (m == null || m.Deleted)
	        	return false;
			
			if (m is nun )
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
			if (m is nun )
				return;

		    base.AggressiveAction(m, true);
		}

		public override bool CanBeHarmful(Mobile m, bool message, bool ignoreOurBlessedness)
		{
		    if (m is nun )
		        return false;

		    return base.CanBeHarmful(m, message, ignoreOurBlessedness);
		}

		public override bool CanBeBeneficial(Mobile m, bool message, bool allowDead)
		{
		     if (m is nun )
		        return true;

		    return base.CanBeBeneficial(m, message, allowDead);
		}

		public override void OnDamage( int amount, Mobile from, bool willKill )
		{
			m_LastTarget = from;
			Server.Misc.IntelligentAction.LeapToAttacker( this, from );
			
			if ( m_Rage >= 1 && DateTime.UtcNow >= m_NextSpecialAttack )
			{
				PerformRageAttack( from );
				m_NextSpecialAttack = DateTime.UtcNow + TimeSpan.FromSeconds( 20.6 - (m_Rage * 1.5) );
			}

			if (from.Player && from.Kills < 5 && !from.Criminal) 
				from.Criminal = true;	
			
			base.OnDamage( amount, from, willKill );
		}

		private void PerformRageAttack( Mobile target )
		{
			if ( target == null || target.Deleted || !target.Alive )
				return;

			int availableAttacks = m_Rage;
			int attackChoice = Utility.RandomMinMax( 1, availableAttacks );

			switch ( attackChoice  )
			{
				case 1: // holy smite
				{
					
					if (target == null || target.Deleted || !target.Alive)
	                	return;

                	if (!CanBeHarmful(target))
	                	return;

                	PublicOverheadMessage(MessageType.Regular, 0x21, false, "By divine will, be judged!");
	                PlaySound(0x29);
	                FixedParticles(0x37C4, 10, 30, 5052, EffectLayer.Head);
                    DoHarmful(target);
                    int min = 10 + (m_Rage * 5);
	                int max = 15 + (m_Rage * 10);
	                int damage = Utility.RandomMinMax(min, max);
	                target.BoltEffect(0);
	                target.PlaySound(0x1FB);
	                AOS.Damage(target, this, damage, 0, 0, 0, 0, 100);
                    break;
				}

				case 2:  // cleansing burst = a nova of fire damage
				{
					PublicOverheadMessage( MessageType.Regular, 0x21, false, "Heavens protect us!" );
					PlaySound( 0x64F );
					FixedParticles( 0x376A, 9, 32, 5030, EffectLayer.Waist );
					IPooledEnumerable eable = GetMobilesInRange( 6 );
					foreach ( Mobile m in eable )
					{
						if ( m != this && m.Player && m.Alive && CanBeHarmful( m ) )
						{
							DoHarmful( m );
							int damage = Utility.RandomMinMax( 11, 22 );
							AOS.Damage( m, this, damage, 0, 100, 0, 0, 0 );
							SlamVisuals.SlamVisual(this, 6, 0x36B0, 0xb73);
							m.PlaySound( 0x1FB );
                    	}
					}
					eable.Free();
					break;
				}
			}
		}

		public override void CheckReflect( Mobile caster, ref bool reflect )
		{
			int chance = m_Rage * 7;
			reflect = ( Utility.Random(100) < chance );
		}

		public override bool OnBeforeDeath()
		{
			if ( m_Rage == 0 )
			{
				PublicOverheadMessage( MessageType.Regular, 0x21, false, "Please stop this madness!" );
				this.Hits = this.HitsMax / 2;
				this.FixedParticles( 0x376A, 9, 32, 5030, EffectLayer.Waist );
				this.PlaySound( 0x202 );
				
				SetStr( Str + 50 );
				SetDex( Dex + 25 );
				SetDamage( 13, 19 );
				m_Rage = 1;
				return false;
			}
			else if ( m_Rage == 1 )
			{
				PublicOverheadMessage( MessageType.Regular, 0x21, false, "You forced my hand!" );
				this.Hits = this.HitsMax / 4;
				this.FixedParticles( 0x376A, 9, 32, 5030, EffectLayer.Waist );
				this.PlaySound( 0x202 );
				
				SetStr( Str + 80 );
				SetDex( Dex + 35 );
				SetDamage( 21, 29 );
				VirtualArmor += 5;
				m_Rage = 2;
				return false;
			}
			else if ( m_Rage == 2 )
			{
				Effects.SendLocationParticles( EffectItem.Create( this.Location, this.Map, EffectItem.DefaultDuration ), 0x3728, 10, 10, 2023 );
				this.PlaySound( 0x1FE );
				PublicOverheadMessage( MessageType.Regular, 0x21, false, "The gods...will forgive you." );
                Mobile killer = this.LastKiller;
				if (killer != null && killer.Player && killer.Karma < 0)
            	{
            	    int marks = Utility.RandomMinMax(11, 24);
            	    Server.Custom.DefenderOfTheRealm.MarkLootHelper.AwardMarks(killer, 0, marks);
            	}
			}
			
			return base.OnBeforeDeath();
		}

        public override void OnDelete()
        {
            base.OnDelete();
        }       

		public override void OnDeath( Container c )
		{
			base.OnDeath( c );

			int amt = Utility.RandomMinMax( 1, 2 );
			for ( int i = 0; i < amt; i++ )
			{
				c.DropItem( new EtherealPowerScroll() );
			}
			TitanRiches( m_LastTarget );
		}

		public static void TitanRiches( Mobile m )
		{
			if ( m == null || m.Map == null )
				return;

			Map map = m.Map;

			for ( int x = -12; x <= 12; ++x )
			{
				for ( int y = -12; y <= 12; ++y )
				{
					double dist = Math.Sqrt( x * x + y * y );

					if ( dist <= 12 )
						new GoodiesTimer( map, m.X + x, m.Y + y ).Start();
				}
			}
		}

		public class GoodiesTimer : Timer
		{
			private Map m_Map;
			private int m_X, m_Y;

			public GoodiesTimer( Map map, int x, int y ) : base( TimeSpan.FromSeconds( Utility.RandomDouble() * 5.0 ) )
			{
				m_Map = map;
				m_X = x;
				m_Y = y;
			}

			protected override void OnTick()
			{
				int z = m_Map.GetAverageZ( m_X, m_Y );
				bool canFit = m_Map.CanFit( m_X, m_Y, z, 6, false, false );

				for ( int i = -3; !canFit && i <= 3; ++i )
				{
					canFit = m_Map.CanFit( m_X, m_Y, z + i, 6, false, false );

					if ( canFit )
						z += i;
				}

				if ( !canFit )
					return;

				Item g = null;

				int r1 = (int)( Utility.RandomMinMax( 10, 20 ) * ( MyServerSettings.GetGoldCutRate() * .01 ) );
				int r2 = (int)( Utility.RandomMinMax( 25, 50 ) * ( MyServerSettings.GetGoldCutRate() * .01 ) );
				int r3 = (int)( Utility.RandomMinMax( 50, 100 ) * ( MyServerSettings.GetGoldCutRate() * .01 ) );
				int r4 = (int)( Utility.RandomMinMax( 100, 150 ) * ( MyServerSettings.GetGoldCutRate() * .01 ) );
				int r5 = (int)( Utility.RandomMinMax( 150, 200 ) * ( MyServerSettings.GetGoldCutRate() * .01 ) );

				switch ( Utility.Random( 21 ) )
				{
					case 0: g = new Crystals( r1 ); break;
					case 1: g = new DDGemstones( r2 ); break;
					case 2: g = new DDJewels( r2 ); break;
					case 3: g = new DDGoldNuggets( r3 ); break;
					case 4: g = new Gold( r3 ); break;
					case 5: g = new Gold( r3 ); break;
					case 6: g = new Gold( r3 ); break;
					case 7: g = new DDSilver( r4 ); break;
					case 8: g = new DDSilver( r4 ); break;
					case 9: g = new DDSilver( r4 ); break;
					case 10: g = new DDSilver( r4 ); break;
					case 11: g = new DDSilver( r4 ); break;
					case 12: g = new DDSilver( r4 ); break;
					case 13: g = new DDCopper( r5 ); break;
					case 14: g = new DDCopper( r5 ); break;
					case 15: g = new DDCopper( r5 ); break;
					case 16: g = new DDCopper( r5 ); break;
					case 17: g = new DDCopper( r5 ); break;
					case 18: g = new DDCopper( r5 ); break;
					case 19: g = new DDCopper( r5 ); break;
					case 20: g = new DDCopper( r5 ); break;
				}

				if ( g != null )
				{
					g.MoveToWorld( new Point3D( m_X, m_Y, z ), m_Map );

					if ( 0.5 >= Utility.RandomDouble() )
					{
						switch ( Utility.Random( 3 ) )
						{
							case 0: // Fire column
								Effects.SendLocationParticles( EffectItem.Create( g.Location, g.Map, EffectItem.DefaultDuration ), 0x3709, 10, 30, 5052 );
								Effects.PlaySound( g, g.Map, 0x208 );
								break;
							case 1: // Explosion
								Effects.SendLocationParticles( EffectItem.Create( g.Location, g.Map, EffectItem.DefaultDuration ), 0x36BD, 20, 10, 5044 );
								Effects.PlaySound( g, g.Map, 0x307 );
								break;
							case 2: // Ball of fire
								Effects.SendLocationParticles( EffectItem.Create( g.Location, g.Map, EffectItem.DefaultDuration ), 0x36FE, 10, 10, 5052 );
								break;
						}
					}
				}
			}
		}

		public override void OnAfterSpawn()
		{
			base.OnAfterSpawn();
		}

		public MotherSuperior( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 1 ); // version

			writer.Write( m_Rage );
			writer.Write( m_NextSpecialAttack );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();

			if ( version >= 1 )
			{
				m_Rage = reader.ReadInt();
				m_NextSpecialAttack = reader.ReadDateTime();
			}
		}
	}
}