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
using Server.Regions;
using Server.Custom;

namespace Server.Mobiles 
{ 
	public class BloodstoneKeepKnight : BaseCreature
	{
		private DateTime m_NextShieldBash;
		private DateTime m_NextRageAllowed;
		private DateTime m_RageEnds;
		private bool m_IsRaging;
		private Timer m_RageTimer;
		private Timer m_RegenerationTimer;

		[Constructable] 
		public BloodstoneKeepKnight() : base(AIType.AI_Melee, FightMode.Aggressor, 10, 1, 0.2, 0.4 ) 
		{
			Title = "the Death Knight";
			SetStr( 388 );
			SetDex( 200 );
			SetInt( 200 );
			SetHits( 800 );
			SetDamage( 18, 24 );
			VirtualArmor = 50;
			Fame = 10000;
			Karma = -10000;
			Team = 666;
			if ( Female = Utility.RandomBool() ) 
			{ 
				Body = 401; 
				Name = NameList.RandomName( "female" );
			}
			else 
			{ 
				Body = 400; 			
				Name = NameList.RandomName( "male" ); 
				FacialHairItemID = Utility.RandomList( 0, 0, 8254, 8255, 8256, 8257, 8267, 8268, 8269 );
			}
			Utility.AssignRandomHair( this );
			HairHue = Utility.RandomHairHue();
			FacialHairHue = HairHue;
			SetResistance(ResistanceType.Physical, 50, 65);
            SetResistance(ResistanceType.Fire, 40, 55);
            SetResistance(ResistanceType.Cold, 25, 35);
            SetResistance(ResistanceType.Poison, 25, 35);
            SetResistance(ResistanceType.Energy, 50, 60);

			SetSkill( SkillName.Anatomy, 100.0 );
			SetSkill( SkillName.MagicResist, 100.0 );
			SetSkill( SkillName.Swords, 100.0 );
			SetSkill( SkillName.Tactics, 100.0 );
			SetSkill( SkillName.Parry, 100.0 );

			PackItem( new Gold( Utility.RandomMinMax( 105, 385 ) ) );
            m_NextRageAllowed = DateTime.UtcNow;
			m_IsRaging = false;
			InitializeEquipment();
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Rich, 2 );
		}

        public override void OnThink()
		{
			base.OnThink();

			if ( !m_IsRaging && Hits < HitsMax && DateTime.UtcNow >= m_NextRageAllowed && Combatant != null )
			{
				StartRage();
			}
		}
        private void StartRage()
		{
			m_IsRaging = true;
			m_RageEnds = DateTime.UtcNow + TimeSpan.FromSeconds( 12 );

			RawStr += 70;
			RawInt -= 20;

			PublicOverheadMessage( MessageType.Emote, 0x21, false, "*Flies into a mad rage*" );

			Effects.SendLocationParticles( EffectItem.Create( this.Location, this.Map, EffectItem.DefaultDuration ), 0x3709, 10, 30, 0x0213 );

			m_RegenerationTimer = Timer.DelayCall( TimeSpan.FromSeconds( 3 ), TimeSpan.FromSeconds( 3 ), new TimerCallback( RegenerateHealth ) );

			m_RageTimer = Timer.DelayCall( TimeSpan.FromSeconds( 12 ), new TimerCallback( EndRage ) );
		}

		private void RegenerateHealth()
		{
			if ( m_IsRaging && Alive )
			{
				Hits += 32;
			}
		}

		private void EndRage()
		{
			if ( !m_IsRaging )
				return;

			m_IsRaging = false;

			RawStr -= 70;
			RawInt += 20;

			Stam = Stam / 2;

			if ( m_RegenerationTimer != null )
			{
				m_RegenerationTimer.Stop();
				m_RegenerationTimer = null;
			}

			m_NextRageAllowed = DateTime.UtcNow + TimeSpan.FromMinutes( 2 );
		}

		public override void OnDeath(Container c)
		{
		    base.OnDeath(c);
		    BossLootSystem.BossEnchant(this, c, 300, 15, 1, "deathknight");
		}

		public override void OnDelete()
		{
			if ( m_RageTimer != null )
			{
				m_RageTimer.Stop();
				m_RageTimer = null;
			}

			if ( m_RegenerationTimer != null )
			{
				m_RegenerationTimer.Stop();
				m_RegenerationTimer = null;
			}

			base.OnDelete();
		}

		public override bool OnBeforeDeath()
        {
            Mobile killer = this.LastKiller;

            if (killer != null && killer.Player && killer.Karma > 0)
            {
                int marks = Utility.RandomMinMax(18, 27);
                Server.Custom.DefenderOfTheRealm.MarkLootHelper.AwardMarks(killer, 1, marks);
            }

            return base.OnBeforeDeath();
        }

		public override void OnDamage(int amount, Mobile from, bool willKill)
		{
		    base.OnDamage(amount, from, willKill);

		}

		public override Poison PoisonImmune{ get{ return Poison.Greater; } }
		public override bool Unprovokable { get { return true; } }
		public override bool CanRummageCorpses{ get{ return false; } }
		public override bool ClickTitle{ get{ return false; } }
		public override bool ShowFameTitle{ get{ return false; } }
		public override bool AlwaysAttackable{ get{ return true; } }
		public override int TreasureMapLevel{ get{ return 4; } }
	
		public override void OnAfterSpawn()
		{
		    base.OnAfterSpawn();		
			InitializeEquipment();
		}	

		private void InitializeEquipment()
		{
		    if (FindItemOnLayer(Layer.OneHanded) != null)
		        return;
		
		    AddEquipment();
		    ColorKnight();
		}	

		private void AddEquipment()
		{
		    AddItem(new RoyalSword { Hue = 0x0AA5 });
		    AddItem(new RoyalArms { Hue = 0x0AA5 });
		    AddItem(new RoyalChest { Hue = 0x0AA5 });
		    AddItem(new RoyalsLegs { Hue = 0x0AA5 });
		    AddItem(new RoyalGorget { Hue = 0x0AA5 });
		    AddItem(new RoyalGloves { Hue = 0x0AA5 });
		    AddItem(new Boots { Hue = 0x0AA5 });
		    AddItem(new DreadHelm { Hue = 0x0AA5 });
		    AddItem(new RoyalShield { Hue = 0x0AA5 });		
		    AddItem(new Cloak { Hue = 0x0AA5 });
		}		

		private void ColorKnight()
		{
		    MorphingTime.ColorMyClothes(this, 0x0AA5, 0);
		    MorphingTime.ColorMyArms(this, 0x0AA5, 0);
		    Server.Misc.MorphingTime.CheckMorph(this);
		}		

		
		private static readonly string[] AttackLines = new string[]
		{
		    "So, {0}? You dare tread unto our home?",
		    "Your blood will feed these stones!",
		    "By broken oath and burning hate, I command you to yield!",
		    "You stand before the true knights of the realm!",
		    "The realm sent you? Then the realm will mourn!",
		    "Bloodstone Keep is not a place of mercy, {0}!",
		    "You face those who have forsaken grace!",
		    "Your death will echo through these cursed halls!",
		    "The kingdom will fear our vengeance!",
		    "Steel yourself, for our blades know only hate!",
		    "You smell of the realm, {0}. That stench ends now!",
		    "Honor humiliated us, hatred craddled us!",
		    "The king’s laws have no place here!",
		    "You trespass where hope was slaughtered!",
		    "Bloodstone drinks deeply of the unworthy!",
		    "We will guard this keep to the last of us!",
		    "Your courage amuses us, {0}!",
		    "You will feed the stones beneath our boots!",
		    "Mercy was burned from us long ago!",
		    "We are the last oath the realm broke!",
		    "Kneel, {0}, and die knowing why!",
		    "The keep demands your life!",
		    "Your death serves a higher vengeance!",
		    "You face knights who chose damnation!",
		    "The kingdom abandoned us—now we repay the debt!",
		    "Your screams will honor Bloodstone!",
		    "Steel and spite guide my hand!",
		    "You challenge warriors forged in hatred!",
		    "The dead remember your kind, {0}!",
		    "Bloodstone Keep stands—*you* will not!",
		    "Our blades carry centuries of wrath!",
		    "Your life is forfeit by our creed!",
		    "We endure, we hate, we kill!",
		    "You bring the realm’s lies to our gates!",
		    "Even death recoils from what we are!",
		    "Your end strengthens our resolve!",
		    "By blood and vengeance, fall!",
		    "You should have turned back, {0}!",
		    "The keep is eternal—your life is not!"
		};


		public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);
			if(Utility.RandomDouble() < 0.25)
            {
				int i = Utility.Random(AttackLines.Length);
			    Say(string.Format(AttackLines[i], defender.Name));                
            }

           	if (DateTime.UtcNow >= m_NextShieldBash && Utility.RandomDouble() < 0.15)
        		DoShieldBash(defender);
        }

		private void DoShieldBash(Mobile defender)
		{
		    if (defender == null || defender.Deleted || !defender.Alive)
		        return;

		    m_NextShieldBash = DateTime.UtcNow + TimeSpan.FromMinutes(1.5);

		    PublicOverheadMessage(
		        MessageType.Regular,
		        0x3B2,
		        false,
		        "* The knight slams its shield into " + defender.Name + "! *"
		    );

		    defender.SendMessage("You are struck by a crushing shield bash!");

		    double duration = 6.0;

		    double tactics = defender.Skills[SkillName.Tactics].Value;

		    duration -= (tactics / 30.0);

			int bonusDamage = (int)((this.Str / 25.0) + (this.Skills[SkillName.Tactics].Value / 5.0) +Utility.RandomMinMax(1, 14)
    		);

		    defender.Damage(bonusDamage, this);

		    if (duration < 1.0)
		        duration = 1.0;

		    defender.Paralyze(TimeSpan.FromSeconds(duration));

		    defender.FixedEffect(0x376A, 10, 16, 0x481, 0);
		}

		
		public BloodstoneKeepKnight( Serial serial ) : base( serial ) 
		{ 
		} 

		public override void Serialize( GenericWriter writer ) 
		{ 
			base.Serialize( writer ); 
			writer.Write( (int) 0 ); // version 
            writer.Write( m_NextRageAllowed );
			writer.Write( m_IsRaging );
			writer.Write( m_RageEnds );
		} 

		public override void Deserialize( GenericReader reader ) 
		{ 
			base.Deserialize( reader ); 
			int version = reader.ReadInt(); 
            m_NextRageAllowed = reader.ReadDateTime();
			m_IsRaging = reader.ReadBool();
			m_RageEnds = reader.ReadDateTime();
			if ( m_IsRaging )
			{
				EndRage();
			}
		} 
	} 
}   