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
using Server.Custom;
using Server.Custom.BeholderSpecials;

namespace Server.Mobiles
{
	[CorpseName( "Old One Eye's Corpse" )]
	public class OldOneEye : BaseCreature
	{
		private static readonly List<Type> BossDrops = new List<Type>
    	{
    	    typeof(Artifact_PrimalTunic),
            typeof(Artifact_PrimalLegs),
            typeof(Artifact_PrimalArms),
            typeof(Artifact_PrimalCrown),
            typeof(Artifact_PrimalClaws),
    	};

		private int m_Rage = 0;
		private Mobile m_LastTarget;
		private DateTime m_NextSpecialAttack = DateTime.MinValue;
		private DateTime m_NextTailSwipe = DateTime.MinValue;
		private bool m_IsStunned = false;
		private DateTime m_StunEndTime = DateTime.MinValue;
		
		[Constructable]
		public OldOneEye () : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "Old One Eye";

			Body = 665;
			BaseSoundID = 362;
			NameHue = 0x22;
			Hue = 0x995;
            Title = "The Primal Terror";
			
			SetStr( 796, 885 );
			SetDex( 265, 325 );
			SetInt( 106, 205 );

			SetHits( 11000 );
			SetDamage( 24, 34 );

			SetDamageType( ResistanceType.Physical, 100 );
			SetResistance( ResistanceType.Physical, 75 );
			SetResistance( ResistanceType.Fire, 75 );
			SetResistance( ResistanceType.Cold, 40 );
			SetResistance( ResistanceType.Poison, 60 );
			SetResistance( ResistanceType.Energy, 60 );

			SetSkill( SkillName.MagicResist, 145.5, 165.0 );
			SetSkill( SkillName.Tactics, 121.0, 125.0 );
			SetSkill( SkillName.FistFighting, 121.0, 125.0 );
			SetSkill( SkillName.Anatomy, 121.0, 125.0 );

			Fame = 30000;
			Karma = 30000;

			VirtualArmor = 60;

			PackItem( Loot.RandomArty() );
			PackItem( Loot.RandomArty() );
			PackItem( Loot.RandomArty() );
			PackItem( Loot.RandomArty() );
    	}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.UltraRich, 6 );
		}

		public override int TreasureMapLevel{ get{ return 4; } }
		public override bool CanRummageCorpses{ get{ return false; } }
		public override bool ReacquireOnMovement{ get{ return !Controlled; } }
		public override bool BleedImmune{ get{ return true; } }
		public override bool BardImmune { get { return true; } }
		public override bool Unprovokable { get { return true; } }
		public override Poison PoisonImmune{ get{ return Poison.Greater; } }

		public override void OnDamage( int amount, Mobile from, bool willKill )
		{
			m_LastTarget = from;
			Server.Misc.IntelligentAction.LeapToAttacker( this, from );
			
			if ( !m_IsStunned && m_Rage >= 1 && DateTime.UtcNow >= m_NextSpecialAttack )
			{
				PerformRageAttack( from );
				m_NextSpecialAttack = DateTime.UtcNow + TimeSpan.FromSeconds( 30 - (m_Rage * 2) );
			}
			base.OnDamage( amount, from, willKill );
		}

        private int getParalyzeDuration(Mobile m)
		{
			int resist = (int)(m.Skills.MagicResist.Value);
			// 2s at 125, 8s at 0 magic resist
			int duration = 8 - (int)(resist * (6.0 / 125.0));
			return duration;
		}

		private void PerformRageAttack( Mobile target )
		{
			if ( target == null || target.Deleted || !target.Alive )
				return;

			int availableAttacks = m_Rage;
			int attackChoice = Utility.RandomMinMax( 1, availableAttacks );

			switch ( attackChoice  )
			{
				case 1: // terrifying roar
				{
					TerrorizingScream();
					break;
				}
				case 2: // ground stomp (knockback + stagger)
				{
					GroundStomp();
		            break;
				}
                case 3: // rampage - multi charge
				{
                    PerformRampage();
		            break;
				}
			}
		}

		public void TerrorizingScream() 
		{
			PublicOverheadMessage(
                MessageType.Regular,
                0x21,
                false,
                "*The Old One Eye unleashes a soul-freezing roar!*"
            );

            PlaySound(0x64D);

            IPooledEnumerable eable = GetMobilesInRange(6);
            foreach (Mobile m in eable)
            {
                if (m == this || !m.Alive || !m.Player || !CanBeHarmful(m))
                    continue;

                if (m.Combatant != this || m.Skills.Knightship.Value > 90.0)
                    continue;

                DoHarmful(m);

                double resist = m.Skills[SkillName.MagicResist].Value;
                int duration = 8 - (int)(resist * (6.0 / 125.0));

                if (duration < 2)
                    duration = 2;

                m.Paralyze(TimeSpan.FromSeconds(duration));
                m.SendMessage("You are frozen in terror!");
                m.FixedParticles(0x376A, 9, 32, 5030, EffectLayer.Head);
            }
            eable.Free();
		}

		public void GroundStomp()
		{
			 PublicOverheadMessage(
                MessageType.Regular,
                0x21,
                false,
                "*The ground quakes!*"
            );

            PlaySound(0x64F);

            IPooledEnumerable eable = GetMobilesInRange(8);
            foreach (Mobile m in eable)
            {
                if (m != this && m.Player && m.Alive && CanBeHarmful(m))
                {
                    DoHarmful(m);

                    int damage = Utility.RandomMinMax(33, 44);
                    AOS.Damage(m, this, damage, 100, 0, 0, 0, 0);

                    // Knockback effect
                    Direction d = Utility.GetDirection(this, m);
                    Point3D targetLoc = m.Location;
                    
                    for (int i = 0; i < 3; i++)
                    {
                        Point3D newLoc = GetPointInDirection(targetLoc, d, 1);
                        if (m.Map.CanSpawnMobile(newLoc))
                            targetLoc = newLoc;
                        else
                            break;
                    }
                    
                    m.MoveToWorld(targetLoc, m.Map);
                    m.Paralyze(TimeSpan.FromSeconds(2));
                    m.SendMessage("You are knocked back by the tremendous force!");

                    SlamVisuals.SlamVisual(
                        this,
                        6,
                        0x36B0,
                        0x995
                    );
                }
            }
            eable.Free();
		}

		public void PerformRampage()
		{
			PublicOverheadMessage(
                MessageType.Regular,
                0x21,
                false,
                "*The Old One Eye charges wildly in all directions!*"
            );

			PlaySound(0x654);

			List<Point3D> rampagePath = new List<Point3D>();
			Point3D currentLoc = this.Location;
			
			int chargeCount = Utility.RandomMinMax(4, 6);
			
			for (int charge = 0; charge < chargeCount; charge++)
			{
				Direction chargeDir = (Direction)Utility.Random(8);
				int chargeDist = Utility.RandomMinMax(6, 8);
				
				for (int step = 0; step < chargeDist; step++)
				{
					Point3D nextLoc = GetPointInDirection(currentLoc, chargeDir, 1);
					
					if (this.Map.CanSpawnMobile(nextLoc))
					{
						rampagePath.Add(nextLoc);
						currentLoc = nextLoc;
					}
					else
					{
						break;
					}
				}
			}
			List<Mobile> damagedMobiles = new List<Mobile>();
			
			foreach (Point3D loc in rampagePath)
			{
				this.MoveToWorld(loc, this.Map);
				
				// Visual effect at each location
				Effects.SendLocationEffect(loc, this.Map, 0x3728, 15, 10, 0x995, 0);
				
				IPooledEnumerable eable = this.Map.GetMobilesInRange(loc, 1);
				foreach (Mobile m in eable)
				{
					if (m != this && m.Player && m.Alive && CanBeHarmful(m) && !damagedMobiles.Contains(m))
					{
						DoHarmful(m);
						int damage = Utility.RandomMinMax(43, 54);
						AOS.Damage(m, this, damage, 100, 0, 0, 0, 0);
						m.SendMessage("You are trampled by the rampaging beast!");
						damagedMobiles.Add(m);
					}
				}
				eable.Free();
			}
			// self Stun after rampage
			m_IsStunned = true;
			m_StunEndTime = DateTime.UtcNow + TimeSpan.FromSeconds(4);
			this.Frozen = true;
			
			Timer.DelayCall(TimeSpan.FromSeconds(4), delegate()
			{
				m_IsStunned = false;
				this.Frozen = false;
				PublicOverheadMessage(MessageType.Regular, 0x21, false, "*The beast recovers from its rampage*");
			});
		}

		// Helper method to get a point in a specific direction
		private Point3D GetPointInDirection(Point3D from, Direction d, int distance)
		{
			int x = from.X;
			int y = from.Y;
			
			switch (d & Direction.Mask)
			{
				case Direction.North:
					y -= distance;
					break;
				case Direction.South:
					y += distance;
					break;
				case Direction.West:
					x -= distance;
					break;
				case Direction.East:
					x += distance;
					break;
				case Direction.Right:
					x += distance;
					y -= distance;
					break;
				case Direction.Left:
					x -= distance;
					y += distance;
					break;
				case Direction.Down:
					x += distance;
					y += distance;
					break;
				case Direction.Up:
					x -= distance;
					y -= distance;
					break;
			}
			
			return new Point3D(x, y, from.Z);
		}

		public override void CheckReflect( Mobile caster, ref bool reflect )
		{
			int chance = m_Rage * 16;
			reflect = ( Utility.Random(100) < chance );
		}

		public override void OnGotMeleeAttack( Mobile attacker )
		{
			base.OnGotMeleeAttack(attacker);
			
			if (Utility.Random(100) < 15 && DateTime.UtcNow >= m_NextTailSwipe)
			{
				PerformTailSwipe();
				m_NextTailSwipe = DateTime.UtcNow + TimeSpan.FromSeconds(20);
			}
		}

		private void PerformTailSwipe()
		{
			PublicOverheadMessage(
                MessageType.Regular,
                0x21,
                false,
                "*Swings its massive tail!*"
            );

			PlaySound(0x64E);

			IPooledEnumerable eable = GetMobilesInRange(1);
			foreach (Mobile m in eable)
			{
				if (m != this && m.Player && m.Alive && CanBeHarmful(m))
				{
					DoHarmful(m);
					int damage = Utility.RandomMinMax(29, 40);
					AOS.Damage(m, this, damage, 100, 0, 0, 0, 0);
					m.SendMessage("You are struck by a devastating tail swipe!");
					m.FixedParticles(0x36BD, 20, 10, 5044, EffectLayer.Head);
				}
			}
			eable.Free();
		}

		public override bool OnBeforeDeath()
		{
			if ( m_Rage == 0 )
			{
				PublicOverheadMessage( MessageType.Regular, 0x21, false, "*Screeches defiantly*" );
				this.Hits = this.HitsMax;
				this.FixedParticles( 0x376A, 9, 32, 5030, EffectLayer.Waist );
				this.PlaySound( 0x63F );
				SetStr( Str + 30 );
				SetDamage( 29, 39 );
				
				m_Rage = 1;
				return false;
			}
			else if ( m_Rage == 1 )
			{
				PublicOverheadMessage( MessageType.Regular, 0x21, false, "*Screeches in Pain*" );
				this.Hits = this.HitsMax;
				this.FixedParticles( 0x376A, 9, 32, 5030, EffectLayer.Waist );
				this.PlaySound( 0x63F );
				
				SetStr( Str + 60 );
				SetDex( Dex + 10 );
				SetDamage( 34, 44 );
				VirtualArmor += 10;
				m_Rage = 2;
				return false;
			}
			else if ( m_Rage == 2 )
			{
				PublicOverheadMessage( MessageType.Regular, 0x21, false, "*Roars terrifyingly*" );
				this.Hits = this.HitsMax;
				this.FixedParticles( 0x376A, 9, 32, 5030, EffectLayer.Waist );
				this.PlaySound( 0x63F );
				
				SetStr( Str + 90 );
				SetDex( Dex + 40 );
				SetDamage( 39, 51 );
				VirtualArmor += 15;
				m_Rage = 3;
				return false;
			}
			else 
			{
				Effects.SendLocationParticles( EffectItem.Create( this.Location, this.Map, EffectItem.DefaultDuration ), 0x3728, 10, 10, 2023 );
				this.PlaySound( 0x63F );
				PublicOverheadMessage( MessageType.Regular, 0x21, false, "*Screeches one last time*" );
				Mobile killer = this.LastKiller;
				if (killer != null && killer.Player && killer.Karma > 0)
            	{
            	    int marks = Utility.RandomMinMax(21, 47);
            	    Server.Custom.DefenderOfTheRealm.MarkLootHelper.AwardMarks(killer, 1, marks);
            	}
			}
			return base.OnBeforeDeath();
		}
		public override void OnDeath( Container c )
		{
			base.OnDeath( c );

			BossLootSystem.AwardBossSpecial(this,BossDrops, 15);

			if ( Utility.RandomDouble() < 0.15 )
			{
				c.DropItem( new EternalPowerScroll() );
			}

			int amt = Utility.RandomMinMax( 3, 9 );
			for ( int i = 0; i < amt; i++ )
			{
				c.DropItem( new EtherealPowerScroll() );
			}
			// gold explosion
			RichesSystem.SpawnRiches( m_LastTarget, 4 );
		}

		public override void OnAfterSpawn()
		{
			base.OnAfterSpawn();
			LeechImmune = true;
		}


		public OldOneEye( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 1 ); // version

			writer.Write( m_Rage );
			writer.Write( m_NextSpecialAttack );
			writer.Write( m_NextTailSwipe );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();

			if ( version >= 1 )
			{
				m_Rage = reader.ReadInt();
				m_NextSpecialAttack = reader.ReadDateTime();
				m_NextTailSwipe = reader.ReadDateTime();
			}

			LeechImmune = true;
		}
	}
}