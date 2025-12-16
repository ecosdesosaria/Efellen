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
	public class SkyKnight : BaseCreature
	{
		private DateTime m_NextShieldBash;
		private DateTime m_NextArmorIgnore;
		public static ArrayList ActiveKnights = new ArrayList();

		[Constructable] 
		public SkyKnight() : base(AIType.AI_Melee, FightMode.Evil, 10, 1, 0.2, 0.4 ) 
		{
			Title = "the Sky Knight";
			NameHue = 0x92E;
			SetStr( 388 );
			SetDex( 200 );
			SetInt( 200 );
			SetHits( 800 );
			SetDamage( 20, 30 );
			VirtualArmor = 60;
			Fame = 10000;
			Karma = 10000;
			Team = 777;
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

			AddItem( new LightCitizen( true ) );
			PackItem( new Gold( Utility.RandomMinMax( 105, 385 ) ) );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Rich, 2 );
		}

		public override void OnBeforeSpawn(Point3D location, Map map)
		{
		    base.OnBeforeSpawn(location, map);
		    if (!ActiveKnights.Contains(this))
		        ActiveKnights.Add(this);
		}

		public override bool OnBeforeDeath()
        {
            Mobile killer = this.LastKiller;

            if (killer != null && killer.Player && killer.Karma < 0)
            {
                int marks = Utility.RandomMinMax(8, 17);
                Server.Custom.DefenderOfTheRealm.MarkLootHelper.AwardMarks(killer, 0, marks);
            }

            return base.OnBeforeDeath();
        }

		public override void OnAfterDelete()
		{
		    base.OnAfterDelete();
		    ActiveKnights.Remove(this);
		}

		public override void OnDamage(int amount, Mobile from, bool willKill)
		{
		    base.OnDamage(amount, from, willKill);

		    if (from != null && from.Alive && !from.Deleted && from != this)
        		AlertAllKnights(from);
			
			if (from.Player && from.Kills < 5 && !from.Criminal) 
				from.Criminal = true;
		}

		private void AlertAllKnights(Mobile target)
		{
		    foreach (SkyKnight knight in ActiveKnights)
		    {
		        if (knight == null || knight.Deleted)
		            continue;
		        if (knight.GetDistanceToSqrt(this) <= 9)
		        {
		            knight.Combatant = target;
		            if (target is PlayerMobile && Utility.RandomDouble() < 0.15)
						knight.Say(true, String.Format("To arms! {0} attacks one of our order!", target.Name));
		        }
		    }
		}

		public override Poison PoisonImmune{ get{ return Poison.Greater; } }
		public override bool Unprovokable { get { return true; } }
		public override bool CanRummageCorpses{ get{ return false; } }
		public override bool ClickTitle{ get{ return false; } }
		public override bool ShowFameTitle{ get{ return false; } }
		public override bool AlwaysAttackable{ get{ return true; } }
		public override int TreasureMapLevel{ get{ return 4; } }
		public override bool AlwaysMurderer { get { return false; } }

		public override void OnAfterSpawn()
		{
		    base.OnAfterSpawn();		

		    AddEquipment();
		    ColorKnight();
		    TryMount();
		}		

		private void AddEquipment()
		{
		    AddItem(new RoyalSword { Hue = 0x0672 });
		    AddItem(new RoyalArms { Hue = 0x0672 });
		    AddItem(new RoyalChest { Hue = 0x0672 });
		    AddItem(new RoyalsLegs { Hue = 0x0672 });
		    AddItem(new RoyalGorget { Hue = 0x0672 });
		    AddItem(new RoyalGloves { Hue = 0x0672 });
		    AddItem(new Boots { Hue = 0x0672 });
		    AddItem(new RoyalHelm { Hue = 0x0672 });
		    AddItem(new RoyalShield { Hue = 0x0672 });		
		    AddItem(new Cloak { Hue = 0x0672 });
		}		

		private void ColorKnight()
		{
		    MorphingTime.ColorMyClothes(this, 0x0672, 0);
		    MorphingTime.ColorMyArms(this, 0, 0);
		    Server.Misc.MorphingTime.CheckMorph(this);
		}		

		private void TryMount()
		{
		    if (Utility.RandomBool() && !Server.Misc.Worlds.InBuilding(this))
		    {
		        BaseMount mount = new EvilMount();
		        mount.Body = 0x31F;
		        mount.ItemID = 0x3EBE;	
				if(Utility.RandomDouble() < 0.35)
				{
					mount.Hue = 0x0672;
				}	

		        BaseMount.Ride(mount, this);
		    }
		}


		private static readonly string[] AttackLines = new string[]
		{
		    "Die villian!",
		    "I will bring you justice!",
		    "So, {0}? Your evil ends here!",
		    "We have been told to watch for {0}!",
		    "Fellow knights, {0} is here!",
		    "We have ways of dealing with the likes of {0}!",
		    "Give up! We do not fear {0}!",
		    "So, {0}? I sentence you to death!",
		    "Face the judgment of the Sky Knights!",
		    "For the realm, I strike you down!",
		    "Your wicked path ends beneath our wings!",
		    "Stand firm, griffin! Justice calls us onward!",
		    "You dare raise steel against the skywatch?",
		    "In the name of honor, fall before us!",
		    "The skies themselves deny you mercy, {0}!",
		    "Feel the wrath of a knight sworn to the heavens!",
		    "The griffins sense your corruption, {0}!",
		    "By oath and virtue, I bring light to your darkness!",
			"Your crimes echo across the skies, {0}!",
			"The griffins shall tear the corruption from your flesh!",
			"Justice descends upon you!",
			"The griffins cry out for your downfall!",
			"You stand against the Sky Order—foolish!",
			"Your fate was sealed the moment you appeared, {0}!",
			"The heavens judge you unworthy!",
			"Your darkness falters beneath our wings!",
			"By the Angel's Lyre, fall!",
			"Our vigilance ends you here, {0}!",
			"You cannot outrun us!",
			"The storm answers our call, {0}!",
			"We soar with purpose, and you shall fall before it!",
			"Your wickedness stains the realm no more!",
			"The light of the heavens burns you, {0}!",
			"Your path ends beneath our righteous charge!",
			"The griffins hunger for victory, yield ruffian!",
			"You cannot face the might of the Sky Knights!",
			"The sky rejects your wickedness, {0}!",
			"I ride the wind of justice—prepare yourself!"
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
			
			if (this.Mount != null && DateTime.UtcNow >= m_NextArmorIgnore && Utility.RandomDouble() < 0.15)
			{
			   DoMountedArmorIgnore(defender);
			}

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

		private void DoMountedArmorIgnore(Mobile defender)
		{
		    if (defender == null || defender.Deleted || !defender.Alive)
		        return;

		    m_NextArmorIgnore = DateTime.UtcNow + TimeSpan.FromMinutes(1.5);
		    this.PublicOverheadMessage(MessageType.Regular, 0x22, false,
		        "The griffin gores " + defender.Name + " with its sharp talons!");
		
		    defender.FixedEffect(0x37B9, 10, 16, 0x44E, 0);
		    defender.PlaySound(0x142);

		    int dmg = Utility.RandomMinMax(15, 35);

		    AOS.Damage(
		        defender,
		        this,
		        dmg,
		        true,
		        100,
		        0, 0, 0, 0
		    );
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


		public SkyKnight( Serial serial ) : base( serial ) 
		{ 
		} 

		public override void Serialize( GenericWriter writer ) 
		{ 
			base.Serialize( writer ); 
			writer.Write( (int) 0 ); // version 
		} 

		public override void Deserialize( GenericReader reader ) 
		{ 
			base.Deserialize( reader ); 
			int version = reader.ReadInt(); 
		} 
	} 
}   