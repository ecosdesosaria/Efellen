using System;
using Server;
using Server.Misc;
using Server.Items;
using System.Collections;
using System.Collections.Generic;
using Server.Network;
using Server.Mobiles;
using Server.Custom.DailyBosses.System;

namespace Server.Mobiles 
{ 
	[CorpseName( "Dracula's corpse" )] 
	public class Dracula : BaseCreature 
	{ 
		private bool m_TrueForm;
		private DateTime m_NextSpecialAttack = DateTime.MinValue;

		public override WeaponAbility GetWeaponAbility()
		{
			return WeaponAbility.BleedAttack;
		}

		[Constructable] 
		public Dracula() : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 ) 
		{ 
			Name = "Dracula";
			Title = "the master vampire";
			Body = 311;
			BaseSoundID = 0x47D;

			SetStr( 1096, 1185 );
			SetDex( 126, 215 );
			SetInt( 686, 775 );

			SetHits( 550, 750 );

			SetDamage( 29, 35 );

			SetDamageType( ResistanceType.Physical, 75 );
			SetDamageType( ResistanceType.Fire, 25 );

			SetResistance( ResistanceType.Physical, 65, 75 );
			SetResistance( ResistanceType.Fire, 80, 90 );
			SetResistance( ResistanceType.Cold, 70, 80 );
			SetResistance( ResistanceType.Poison, 60, 70 );
			SetResistance( ResistanceType.Energy, 60, 70 );

			SetSkill( SkillName.Psychology, 100.0 );
			SetSkill( SkillName.Magery, 100.0 );
			SetSkill( SkillName.Meditation, 75.0 );
			SetSkill( SkillName.MagicResist, 125.5, 150.0 );
			SetSkill( SkillName.Tactics, 97.6, 110.0 );
			SetSkill( SkillName.FistFighting, 107.6, 120.0 );

			Fame = 22500;
			Karma = -22500;

			VirtualArmor = 70;

			if ( 1 == Utility.RandomMinMax( 0, 2 ) )
			{
				LootChest MyChest = new LootChest( Server.Misc.IntelligentAction.FameBasedLevel( this ) );
				MyChest.Name = "Dracula's Chest";
				MyChest.Hue = 0x485;
				PackItem( MyChest );
			}

			if ( 1 == Utility.RandomMinMax( 0, 3 ) )
			{
				PackItem( new DraculaSword() );
			}
			else
			{
				RoyalSword sword = new RoyalSword();
				sword.Hue = 0x497;
				PackItem( sword );
			}
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Rich );
			AddLoot( LootPack.UltraRich, 2 );
			AddLoot( LootPack.HighScrolls, 2 );
		}

		public override bool ClickTitle{ get{ return false; } }
		public override bool ShowFameTitle{ get{ return false; } }
		public override bool CanRummageCorpses{ get{ return true; } }
		public override bool AlwaysAttackable{ get{ return true; } }
		public override Poison PoisonImmune{ get{ return Poison.Deadly; } }
		public override int TreasureMapLevel{ get{ return Core.AOS ? 4 : 0; } }
		public override bool Unprovokable{ get{ return Core.SE; } }
		public override int Hides{ get{ return 3; } }
		public override HideType HideType{ get{ return HideType.Necrotic; } }
		public override int Skeletal{ get{ return Utility.Random(10); } }
		public override SkeletalType SkeletalType{ get{ return SkeletalType.Vampire; } }

		public void DrainLife()
		{
			ArrayList list = new ArrayList();

			foreach ( Mobile m in this.GetMobilesInRange( 2 ) )
			{
				if ( m == this || !CanBeHarmful( m ) )
					continue;

				if ( m is BaseCreature && (((BaseCreature)m).Controlled || ((BaseCreature)m).Summoned || ((BaseCreature)m).Team != this.Team) )
					list.Add( m );
				else if ( m.Player )
					list.Add( m );
			}

			foreach ( Mobile m in list )
			{
				if ( !Server.Items.HiddenTrap.IAmAWeaponSlayer( m, this ) )
				{
					DoHarmful( m );

					m.PlaySound( 0x133 );
					m.FixedParticles( 0x377A, 244, 25, 9950, 31, 0, EffectLayer.Waist );

					m.SendMessage( "You feel the blood draining from you!" );

					int toDrain = Utility.RandomMinMax( 15, 30 );

					Hits += toDrain;
					m.Damage( toDrain, this );
				}
			}
		}

		public override void OnAfterSpawn()
		{
			Server.Misc.IntelligentAction.BeforeMyBirth( this );
			base.OnAfterSpawn();
		}

		public override void OnGotMeleeAttack( Mobile attacker )
		{
			base.OnGotMeleeAttack( attacker );
			Server.Misc.IntelligentAction.DoSpecialAbility( this, attacker );
		}

		public override void OnGaveMeleeAttack( Mobile defender )
		{
			base.OnGaveMeleeAttack( defender );
			Server.Misc.IntelligentAction.DoSpecialAbility( this, defender );

			if ( 0.1 >= Utility.RandomDouble() )
				DrainLife();
		}

		public override bool OnBeforeDeath()
		{
			if ( m_TrueForm )
			{
				Server.Misc.IntelligentAction.BeforeMyDeath( this );
				Server.Misc.IntelligentAction.DropItem( this, this.LastKiller );

				this.Body = 13;
				this.BaseSoundID = 655;
				this.Hue = 0xB85;

				return base.OnBeforeDeath();
			}
			else
			{
				Morph();
				return false;
			}
		}

		public void Morph()
		{
			if (m_TrueForm)
			return;

			m_TrueForm = true;

			Body = 191;
			Hue = 0x497;
			BaseSoundID = 372;

			SetHits( 350, 400 );

			Say("Arrrrrgh!"); 
		}
		public override void OnDamage( int amount, Mobile from, bool willKill )
		{
			if ( DateTime.UtcNow >= m_NextSpecialAttack )
			{
				PerformRageAttack( from );
				m_NextSpecialAttack = DateTime.UtcNow + TimeSpan.FromSeconds( 60 );
			}
			
			base.OnDamage( amount, from, willKill );
		}

		private void PerformRageAttack( Mobile target )
		{
			if ( target == null || target.Deleted || !target.Alive )
				return;

			int attackChoice = Utility.RandomMinMax( 1, 2 );
            Map map = this.Map;

			switch ( attackChoice  )
			{
				case 1: // summon blood elemental
				{
					BossSpecialAttack.SummonHonorGuard(
                        boss: this,
                        target: target,
                        warcry: "The homeland beckons!",
                        amount: 2,
                        creatureType: typeof(BloodElemental),
                        hue: 0x25
                    );
                    break;
				}
				case 2: // blood whip
				{
					if (map == null)
                        return;
                    int range = 7;
                    this.PlaySound(0x208);
                    PublicOverheadMessage(Network.MessageType.Emote, 0x22, false, "*lashes a bloody whip!*");
                    Point3D start = this.Location;
                    int dx = 0, dy = 0;
					int minDamage = 30;
					int maxDamage = 40;
                    GetDirectionVector(this.Direction, out dx, out dy);
                    for (int i = 1; i <= range; i++)
                    {
                        Point3D p = new Point3D(start.X + dx * i, start.Y + dy * i, start.Z);
                        Effects.SendLocationEffect(p, map, 0x36D4, 20, 10, 0x25, 0);
                        foreach (Mobile m in map.GetMobilesInRange(p, 0))
                        {
                            if (m != null && m != this && !m.Deleted && CanBeHarmful(m))
                            {
                                DoHarmful(m);
                                m.Damage(Utility.RandomMinMax(minDamage, maxDamage), this);
                                m.PlaySound(0x15E);
                            }
                        }
                    }
                break;
			    }
			}
		}

		private void GetDirectionVector(Direction d, out int dx, out int dy)
        {
            dx = 0;
            dy = 0;

            switch (d)
            {
                case Direction.North:
                    dy = -1;
                    break;
                case Direction.Right:
                    dx = 1;
                    dy = -1;
                    break;
                case Direction.East:
                    dx = 1;
                    break;
                case Direction.Down:
                    dx = 1;
                    dy = 1;
                    break;
                case Direction.South:
                    dy = 1;
                    break;
                case Direction.Left:
                    dx = -1;
                    dy = 1;
                    break;
                case Direction.West:
                    dx = -1;
                    break;
                case Direction.Up:
                    dx = -1;
                    dy = -1;
                    break;
            }
        }

		public override void OnDeath( Container c )
		{
			base.OnDeath( c );

			Mobile killer = this.LastKiller;
			if ( killer != null )
			{
				if ( killer is BaseCreature )
					killer = ((BaseCreature)killer).GetMaster();

				if ( killer is PlayerMobile )
				{
					if ( GetPlayerInfo.LuckyKiller( killer.Luck ) && Utility.RandomMinMax( 1, 5 ) == 1 && !Server.Misc.PlayerSettings.GetSpecialsKilled( killer, "Dracula" ) )
					{
						Server.Misc.PlayerSettings.SetSpecialsKilled( killer, "Dracula", true );
						ManualOfItems book = new ManualOfItems();
							book.Hue = 0x497;
							book.Name = "Chest of Dracula's Relics";
							book.m_Charges = 1;
							book.m_Skill_1 = 99;
							book.m_Skill_2 = 0;
							book.m_Skill_3 = 0;
							book.m_Skill_4 = 0;
							book.m_Skill_5 = 0;
							book.m_Value_1 = 15.0;
							book.m_Value_2 = 0.0;
							book.m_Value_3 = 0.0;
							book.m_Value_4 = 0.0;
							book.m_Value_5 = 0.0;
							book.m_Slayer_1 = 24;
							book.m_Slayer_2 = 0;
							book.m_Owner = killer;
							book.m_Extra = "of the Vampire";
							book.m_FromWho = "from Dracula";
							book.m_HowGiven = "Acquired by";
							book.m_Points = 300;
							book.m_Hue = 0x497;
							killer.AddToBackpack( book );
							killer.PrivateOverheadMessage(MessageType.Regular, 1153, false, "You found a book and put it in your pack.", killer.NetState);
					}

					if ( GetPlayerInfo.LuckyKiller( killer.Luck ) && Server.Misc.IntelligentAction.FameBasedEvent( this ) )
					{
						LootChest MyChest = new LootChest( Server.Misc.IntelligentAction.FameBasedLevel( this ) );
						Server.Misc.ContainerFunctions.MakeTomb( MyChest, this, 0 );
						c.DropItem( MyChest );
					}
				}
			}
		}

		public Dracula( Serial serial ) : base( serial ) 
		{ 
		} 

		public override void Serialize( GenericWriter writer ) 
		{ 
			base.Serialize( writer ); 
			writer.Write( (int) 1 ); 
			writer.Write( m_TrueForm );	
			writer.Write( m_NextSpecialAttack );
		} 

		public override void Deserialize( GenericReader reader ) 
		{ 
			base.Deserialize( reader ); 
			int version = reader.ReadInt(); 
			switch ( version )
			{
				case 0:
				case 1:
				{
					m_TrueForm = reader.ReadBool();
					break;
				}
			}
			if ( version >= 1 )
			{
				m_NextSpecialAttack = reader.ReadDateTime();
			}
		} 
	} 
}