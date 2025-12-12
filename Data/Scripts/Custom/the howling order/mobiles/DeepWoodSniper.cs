using System;
using Server;
using System.Collections;
using Server.Items;
using Server.ContextMenus;
using Server.Misc;
using Server.Network;
using System.Collections.Generic;

namespace Server.Mobiles
{
	public class DeepWoodSniper : BaseCreature
	{
		private DateTime m_NextPiercingShot;
		private DateTime m_NextPoisonShot;

		[Constructable]
		public DeepWoodSniper() : base( AIType.AI_Archer, FightMode.Evil, 10, 1, 0.2, 0.4 )
		{
			
			Hue = Utility.RandomSkinColor();
            Title = "the Deepwood Sniper";
			Name = NameList.RandomName( "druid" );

			if ( this.Female = Utility.RandomBool() )
			{
				Body = 0x191;
				AddItem( new Skirt( Utility.RandomColor(0) ) );
				Utility.AssignRandomHair( this );
				HairHue = Utility.RandomHairHue();
			}
			else
			{
				Body = 0x190;
				AddItem( new ShortPants( Utility.RandomColor(0) ) );
				Utility.AssignRandomHair( this );
				int HairColor = Utility.RandomHairHue();
				FacialHairItemID = Utility.RandomList( 0, 8254, 8255, 8256, 8257, 8267, 8268, 8269 );
				HairHue = HairColor;
				FacialHairHue = HairColor;
			}

			Hue = Utility.RandomSkinColor();
			NameHue = 0x92E;

			SetStr( 111, 145 );
			SetDex( 111, 145 );
			SetInt( 46, 60 );

			SetHits( 119, 193 );

			SetDamage( 13, 19 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 45 );
			SetResistance( ResistanceType.Fire, 25 );
			SetResistance( ResistanceType.Poison, 30 );
			SetResistance( ResistanceType.Energy, 25 );
			SetResistance( ResistanceType.Cold, 25 );

			SetSkill( SkillName.MagicResist, 75.0, 107.5 );
			SetSkill( SkillName.Tactics, 85.0, 111.5 );
			SetSkill( SkillName.FistFighting, 60.2, 90.0 );
			SetSkill( SkillName.Marksmanship, 80.1, 100.0 );
            SetSkill( SkillName.Hiding, 75.0, 98.0 );
            SetSkill( SkillName.Stealth, 75.0, 100.0 );

			Fame = 6000;
			Karma = 6000;
			
            AddItem( new Bow() ); 
            PackItem( new Arrow( Utility.RandomMinMax( 15, 25 ) ) );
            AddItem( new WolfCap() );
            AddItem(new MarksOfTheWilds(Utility.Random(5)));
            Server.Misc.IntelligentAction.GiveAdventureGear( this );

			m_NextPiercingShot = DateTime.UtcNow;
			m_NextPoisonShot = DateTime.UtcNow;
		}

		public override bool ClickTitle{ get{ return false; } }
		public override bool ShowFameTitle{ get{ return false; } }
		public override bool CanRummageCorpses{ get{ return false; } }
		public override bool AlwaysAttackable{ get{ return true; } }
		public override int Meat{ get{ return 1; } }
		public override int TreasureMapLevel{ get{ return Core.AOS ? 1 : 0; } }
		public override int Skeletal{ get{ return Utility.Random(3); } }
		public override SkeletalType SkeletalType{ get{ return SkeletalType.Brittle; } }
		public override bool AlwaysMurderer { get { return false; } }

		public override void OnAfterSpawn()
		{
		    base.OnAfterSpawn();		
		    AddEquipment();
		}		

		private void AddEquipment()
		{
		    AddItem(new StuddedArms { Hue = 669 });
		    AddItem(new StuddedChest { Hue = 669 });
		    AddItem(new StuddedLegs { Hue = 669 });
		    AddItem(new StuddedGorget { Hue = 669 });
		    AddItem(new StuddedGloves { Hue = 669 });
		    AddItem(new Boots { Hue = 669 });
		    AddItem(new Cloak { Hue = 669 });
		}		

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Average, 2 );
            AddLoot( LootPack.Average, 2 );
		}
		

		private static readonly string[] AttackLines = new string[]
		{
		    "The wild reclaims you!!",
		    "Nature doesn't forgive!",
            "So, {0}? The wild calls for your blood!",
            "The grove has marked you, {0}!",
            "Trespasser! The spirits howl for {0}!",
            "We have your scent, {0}!",
            "You cannot escape the forest's wrath, {0}!",
            "So, {0}? you shall feed our grove!",
            "We are untamable!",
            "Your corruption ends here, defiler!",
            "The ancient trees remember your kind's sins!",
            "Hear them, intruder? The wolves cry out!",
            "You dare bring steel into our sacred grove?",
            "The Howling Order suffers no despoilers!",
            "Nature doesn't forget, {0}!",
            "We shall hunt you down!",
            "The spirits smell your civilization's rot, {0}!",
            "By sage and fang, we cleanse this place of you!",
            "Your footsteps wound the earth, {0}!",
            "The wolves feast on those who defile the grove!",
            "The forest's judgment falls upon you!",
            "The pack circles you, {0}! There is no escape!",
            "You stand in the heart of the wild!",
            "Your presence poisons the very soil, {0}!",
            "The true world rejects your taint!",
            "Your civilized ways can't protect you here!",
            "By the Old Oak's roots, fall!",
            "Our vigil shall be your end, {0}!",
            "You cannot outgrow the forest!",
            "The storm answers nature's fury, {0}!",
            "We are the shield of the wild, and you shall break upon it!",
            "Your kind has taken enough from this land!",
            "The spirits converge upon you, {0}!",
            "Your path ends beneath our feet!",
            "The wolves hunger! Yield, defiler!",
            "You cannot stand against the primal force!",
            "The grove remembers every wound, {0}!",
            "I am the voice of the wilderness, and it howls for vengeance!"
		};
        
        public override bool IsEnemy( Mobile m )
	    {
			if (m == null || m.Deleted)
	        	return false;
			
			if (m is DeepWoodSniper || m is DruidOfTheHowlingOrder)
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
		    if (m is DeepWoodSniper || m is DruidOfTheHowlingOrder)
				return;

		    base.AggressiveAction(m, true);
		}

		public override bool CanBeHarmful(Mobile m, bool message, bool ignoreOurBlessedness)
		{
		    if (m is DeepWoodSniper || m is DruidOfTheHowlingOrder)
		        return false;

		    return base.CanBeHarmful(m, message, ignoreOurBlessedness);
		}

		public override bool CanBeBeneficial(Mobile m, bool message, bool allowDead)
		{
		    if (m is DeepWoodSniper || m is DruidOfTheHowlingOrder)
		        return true;

		    return base.CanBeBeneficial(m, message, allowDead);
		}

		public override void OnGotMeleeAttack( Mobile attacker )
		{
			base.OnGotMeleeAttack( attacker );
            Server.Misc.IntelligentAction.HideFromOthers( this );
		}

		public override void OnGaveMeleeAttack( Mobile defender )
		{
            if(Utility.RandomDouble() < 0.25)
            {
				int i = Utility.Random(AttackLines.Length);
			    Say(string.Format(AttackLines[i], defender.Name));                
            }
			base.OnGaveMeleeAttack( defender );
		}

		public override void OnDamage(int amount, Mobile from, bool willKill)
		{
			// piercing retribution shot
            if (DateTime.UtcNow >= m_NextPiercingShot && Utility.RandomDouble() < 0.15)
			{
				Say("Nature's wrath guides my arrows!");
				int bonusDamage = (int)(amount * 0.55);
				if (bonusDamage > 0 && from != null && !from.Deleted)
				{
					from.Damage(bonusDamage, this);
				}
				PlaySound(0x20F);
				m_NextPiercingShot = DateTime.UtcNow + TimeSpan.FromSeconds(30);
			}
			// Poison Shot
			if (DateTime.UtcNow >= m_NextPoisonShot && Utility.RandomDouble() < 0.15 && from != null && !from.Deleted)
			{
                Say("I got you now!");
				from.ApplyPoison(this, Poison.Greater);
				m_NextPoisonShot = DateTime.UtcNow + TimeSpan.FromSeconds(45);
			}

		    base.OnDamage(amount, from, willKill);

			if (from.Player && from.Kills < 5 && !from.Criminal) 
				from.Criminal = true;
		}

		public override bool OnBeforeDeath()
		{
			return base.OnBeforeDeath();
		}

		public DeepWoodSniper( Serial serial ) : base( serial )
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