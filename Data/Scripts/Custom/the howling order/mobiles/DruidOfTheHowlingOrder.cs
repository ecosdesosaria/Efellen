using System;
using Server;
using Server.Misc;
using Server.Items;
using Server.Regions;
using Server.CustomSpells;

namespace Server.Mobiles 
{ 
	[CorpseName( "a Druid's corpse" )] 
	public class DruidOfTheHowlingOrder : BaseSpellCaster 
	{ 
		private DateTime m_NextSummon;
        private DateTime m_NextWolfCall;

		[Constructable] 
		public DruidOfTheHowlingOrder() : base( AIType.AI_Mage, FightMode.Evil, 10, 1, 0.2, 0.4 ) 
		{ 
			if ( this.Female = Utility.RandomBool() ) 
			{ 
				Body = 0x191; 
				Utility.AssignRandomHair( this );
				HairHue = Utility.RandomHairHue();
            } 
			else 
			{ 
				Body = 0x190; 
				Utility.AssignRandomHair( this );
				int HairColor = Utility.RandomHairHue();
				FacialHairItemID = Utility.RandomList( 0, 8254, 8255, 8256, 8257, 8267, 8268, 8269 );
				HairHue = HairColor;
				FacialHairHue = HairColor;
			}

			Name = NameList.RandomName( "druid" );
			Hue = Utility.RandomSkinColor();
			NameHue = 0x92E;
            Title = "the Druid of the Howling Order";

			SetStr( 81, 145 );
			SetDex( 91, 145 );
			SetInt( 96, 120 );

			SetHits( 89, 163 );

			SetDamage( 9, 17 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 35 );
			SetResistance( ResistanceType.Fire, 25 );
			SetResistance( ResistanceType.Poison, 35 );
			SetResistance( ResistanceType.Energy, 25 );
			SetResistance( ResistanceType.Cold, 25 );

			SetSkill( SkillName.Psychology, 75.1, 100.0 );
			SetSkill( SkillName.Magery, 75.1, 100.0 );
			SetSkill( SkillName.MagicResist, 75.0, 107.5 );
			SetSkill( SkillName.Tactics, 65.0, 87.5 );
			SetSkill( SkillName.FistFighting, 60.2, 90.0 );
			SetSkill( SkillName.Bludgeoning, 60.3, 90.0 );

			Fame = 6000;
			Karma = 6000;

			VirtualArmor = 30;
            AddItem( new WolfCap() );
            AddItem(new MarksOfTheWilds(Utility.Random(9)));

            m_NextWolfCall = DateTime.UtcNow;
			m_NextSummon = DateTime.UtcNow;
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Rich );
            AddLoot( LootPack.Average );
		}

		public override bool ClickTitle{ get{ return false; } }
		public override bool ShowFameTitle{ get{ return false; } }
		public override bool CanRummageCorpses{ get{ return true; } }
		public override bool AlwaysAttackable{ get{ return true; } }
		public override int Meat{ get{ return 1; } }
		public override int TreasureMapLevel{ get{ return Core.AOS ? 1 : 0; } }
		public override int Skeletal{ get{ return Utility.Random(3); } }
		public override SkeletalType SkeletalType{ get{ return SkeletalType.Brittle; } }
		public override bool AlwaysMurderer { get { return false; } }

		public override void OnAfterSpawn()
		{
		    base.OnAfterSpawn();		
			this.MobileMagics(Utility.Random(2,5), SpellType.Druid, 0);
		    AddEquipment();
		}		

		private void AddEquipment()
		{
		    AddItem(new WildStaff { Hue = 669 });
		    AddItem(new LeatherArms { Hue = 669 });
		    AddItem(new LeatherChest { Hue = 669 });
		    AddItem(new LeatherLegs { Hue = 669 });
		    AddItem(new LeatherGorget { Hue = 669 });
		    AddItem(new LeatherGloves { Hue = 669 });
		    AddItem(new Boots { Hue = 669 });
		    AddItem(new SageRobe { Hue = 669 });		
		    AddItem(new Cloak { Hue = 669 });
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

		private static readonly string[] SummonLines = new string[]
		{
			"Spirits of the wild, heed my call!",
			"The pack answers!",
			"Rise, guardian of the grove!",
			"The wolves come to our aid!",
			"Nature's fury takes form!",
			"From the spirit realm, I summon thee!",
			"The howling begins!"
		};

        private static readonly string[] CallWolfLines = new string[]
		{
			"Brothers of the wilds, come forth!",
			"Pack, aid me!",
			"we shall enact vengeance with tooth and claw!",
			"Come to my aid, companions!",
			"The howling begins!"
		};

        
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
			       	m is KeeperOfTheGrove));
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

		public override void OnGotMeleeAttack( Mobile attacker )
		{
			base.OnGotMeleeAttack( attacker );

			// 5% chance to summon spectral wolf when hit
			if (DateTime.UtcNow >= m_NextSummon && Utility.RandomDouble() < 0.05)
			{
				SpawnSpectralWolf(attacker);
			}
            // 15% chance to summon nearby wolves when hit
			if (DateTime.UtcNow >= m_NextWolfCall && Utility.RandomDouble() < 0.15)
			{
				CallNearbyWolves(attacker);
			}
		}

		public override void OnGaveMeleeAttack( Mobile defender )
		{
            if(Utility.RandomDouble() < 0.25)
            {
				int i = Utility.Random(AttackLines.Length);
			    Say(string.Format(AttackLines[i], defender.Name));                
            }

			// 15% chance to summon spectral wolf when attacking
			if (DateTime.UtcNow >= m_NextSummon && Utility.RandomDouble() < 0.15)
			{
				SpawnSpectralWolf(defender);
			}

			base.OnGaveMeleeAttack( defender );
		}

        public void CallNearbyWolves(Mobile attacker)
        {
            if (attacker == null || attacker.Deleted)
                return;

            Map map = this.Map;
            if (map == null || map == Map.Internal)
                return;

            bool callSuccess = false;

            foreach (Mobile m in this.GetMobilesInRange(9))
            {
                if (m == null || m.Deleted || m == this || !m.Alive)
                    continue;

                if (m is BlackWolf)
                {
                    BlackWolf wolf = (BlackWolf)m;

                    if (wolf.Combatant != attacker)
                    {
                        wolf.Combatant = attacker;
                        wolf.Warmode = true;
                        try { wolf.PlaySound(wolf.GetAttackSound()); } catch { }
                        try { wolf.Say("Awooo!"); } catch { }
                        callSuccess = true;
                    }
                }
            }

            if (callSuccess && Utility.RandomDouble() < 0.25)
            {
                int i = Utility.Random(CallWolfLines.Length);
                Say(string.Format(CallWolfLines[i], attacker.Name));
            }

            m_NextWolfCall = DateTime.UtcNow + TimeSpan.FromMinutes(3);
        }



		public void SpawnSpectralWolf( Mobile target )
		{
			Map map = this.Map;
			if ( map == null || target == null || target.Deleted )
				return;

			int monsters = 0;
			foreach ( Mobile m in this.GetMobilesInRange( 10 ) )
			{
				if ( m is SpectralWolf)
					++monsters;
			}

			if ( monsters < 1 )
			{
				int i = Utility.Random(SummonLines.Length);
				Say(SummonLines[i]);

				PlaySound( 0x216 );
				
				BaseCreature monster = new SpectralWolf();
				monster.Team = this.Team;
				
				bool validLocation = false;
				Point3D loc = this.Location;
				
				for ( int j = 0; !validLocation && j < 10; ++j )
				{
					int x = X + Utility.Random( 3 ) - 1;
					int y = Y + Utility.Random( 3 ) - 1;
					int z = map.GetAverageZ( x, y );
					
					if ( validLocation = map.CanFit( x, y, this.Z, 16, false, false ) )
						loc = new Point3D( x, y, Z );
					else if ( validLocation = map.CanFit( x, y, z, 16, false, false ) )
						loc = new Point3D( x, y, z );
				}
				
				monster.MoveToWorld( loc, map );
				monster.Combatant = target;

				m_NextSummon = DateTime.UtcNow + TimeSpan.FromMinutes(1);
			}
		}

		public override bool OnBeforeDeath()
        {
            Mobile killer = this.LastKiller;

            if (killer != null && killer.Player && killer.Karma < 0)
            {
                int marks = Utility.RandomMinMax(5, 9);
                Server.Custom.DefenderOfTheRealm.MarkLootHelper.AwardMarks(killer, 0, marks);
            }

            return base.OnBeforeDeath();
        }

		public override void OnDamage(int amount, Mobile from, bool willKill)
		{
		    base.OnDamage(amount, from, willKill);

			if (from.Player && from.Kills < 5 && !from.Criminal) 
				from.Criminal = true;
		}

		public DruidOfTheHowlingOrder( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 1 );

			writer.Write( m_NextSummon );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
			if(version >= 1)
			{
				this.MobileMagics(Utility.Random(2,5), SpellType.Druid, 0);
			}
			m_NextSummon = reader.ReadDateTime();
		}
	}
}