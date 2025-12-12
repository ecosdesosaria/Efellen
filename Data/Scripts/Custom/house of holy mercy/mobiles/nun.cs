using System;
using System.Collections;
using Server.Misc;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "a Nun corpse" )]
	public class nun : BaseCreature
	{
        private DateTime m_NextHeal = DateTime.MinValue;

		[Constructable]
		public nun() : base( AIType.AI_Melee, FightMode.Evil, 10, 1, 0.2, 0.4 )
		{
			Title = "the Nun";
			NameHue = 0x92E;
            Body = 401; 
			Name = NameList.RandomName( "female" );
			Utility.AssignRandomHair( this );
			HairHue = Utility.RandomHairHue();
            Hue = Utility.RandomSkinHue(); 
			SetStr( 90, 99 );
			SetDex( 80, 100 );
			SetInt( 86, 110 );

			SetHits( 67, 87 );

			SetDamage( 5, 15 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 30, 35 );
			SetResistance( ResistanceType.Fire, 10, 20 );
			SetResistance( ResistanceType.Cold, 15, 25 );
			SetResistance( ResistanceType.Poison, 5, 10 );
			SetResistance( ResistanceType.Energy, 5, 10 );

			SetSkill( SkillName.MagicResist, 40.1, 55.0 );
			SetSkill( SkillName.FistFighting, 70.1, 75.0 );
			SetSkill( SkillName.Tactics, 45.1, 70.0 );
            SetSkill( SkillName.Healing, 55.1, 70.0 );
            SetSkill( SkillName.Spiritualism, 70.1, 75.0 );
            SetSkill( SkillName.Anatomy, 45.1, 70.0 );
            
            AddItem( new NunRobe( ) );
			AddItem( new LightCitizen( true ) );
            PackItem( new Bandage( Utility.RandomMinMax( 8, 45 ) ) );
            Fame = 2500;
			Karma = 2500;

			VirtualArmor = 34;
		}

		public override bool ClickTitle{ get{ return false; } }
		public override bool ShowFameTitle{ get{ return false; } }
		public override bool AlwaysAttackable{ get{ return true; } }
		public override int TreasureMapLevel{ get{ return 4; } }
		public override bool AlwaysMurderer { get { return false; } }

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Meager, 2 );
		}

		public override bool CanRummageCorpses{ get{ return true; } }
		public override int Meat{ get{ return 1; } }

		private static readonly string[] AttackLines = new string[]
		{
		    "We have no wealth for you to take!",
		    "Have mercy!",
		    "Has our order wronged you, {0}?",
		    "Thou shall not harm my sisters, {0}!",
		    "{0} is here, escape if you can!",
            "May the light forgive you, {0}!",
            "Your cruelty shall be your undoing, {0}!",
            "Why do you bring violence to our sanctuary?",
            "Sisters, pray for strength!",
            "{0}, the heavens weep for you!",
            "Leave this holy place at once!",
            "The Gods will judge you for this!",
            "Your heart is clouded, {0}!",
            "Repent before it is too late!",
            "We stand united against your wickedness!",
            "I shall pray for your soul!",
            "Turn back, {0}! Turn back from this darkness!",
            "Our order has survived worse than you!",
            "You defile sacred ground with your presence!",
            "The spirits shield us from your evil!",
            "Mercy will find you even if you are undeserving!",
            "This is a house of healing! Cease at once!",
            "We will outlast you!",
            "The light protects us from your shadow, {0}!",
            "Your sins grow heavier with every strike!"
		};

        public override bool OnBeforeDeath()
        {
            Mobile killer = this.LastKiller;

            if (killer != null && killer.Player && killer.Karma < 0)
            {
                int marks = Utility.RandomMinMax(1, 4);
                Server.Custom.DefenderOfTheRealm.MarkLootHelper.AwardMarks(killer, 0, marks);
            }
            return base.OnBeforeDeath();
        }


		public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);
			if(Utility.RandomDouble() < 0.25)
            {
				int i = Utility.Random(AttackLines.Length);
			    Say(string.Format(AttackLines[i], defender.Name));                
            }
        }

		public override bool IsEnemy( Mobile m )
	    {
			if (m == null || m.Deleted)
	        	return false;
			
			if ( !IntelligentAction.GetMyEnemies( m, this, true ) )
				return false;
			
			if (m is nun || m is Cook || m is Herbalist || m is Healer || m is Painter)
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
			if (m is nun || m is Cook || m is Herbalist || m is Healer || m is Painter)
				return;

		    base.AggressiveAction(m, true);
		}

		public override bool CanBeHarmful(Mobile m, bool message, bool ignoreOurBlessedness)
		{
		   	if (m is nun || m is Cook || m is Herbalist || m is Healer || m is Painter)
			    return false;

		    return base.CanBeHarmful(m, message, ignoreOurBlessedness);
		}

		public override bool CanBeBeneficial(Mobile m, bool message, bool allowDead)
		{
		    if (m is nun || m is Cook || m is Herbalist || m is Healer || m is Painter)
		        return true;

		    return base.CanBeBeneficial(m, message, allowDead);
		}

		public override void OnDamage(int amount, Mobile from, bool willKill)
		{
		    base.OnDamage(amount, from, willKill);
			
			if (from.Player && from.Kills < 5 && !from.Criminal) 
				from.Criminal = true;
            
            if (DateTime.UtcNow >= m_NextHeal)
            {
                foreach (Mobile m in this.GetMobilesInRange(6))
                {
                    nun sister = m as nun;
                    if (sister != null && sister != this && sister.Alive && sister.Hits < sister.HitsMax)
                    {
                        int heal = Utility.RandomMinMax(6, 15);
                        sister.Hits += heal;

                        this.Say("Hold on, sister! I am here!");
                        this.FixedEffect(0x376A, 9, 32);
                        sister.FixedEffect(0x376A, 9, 32);

                        m_NextHeal = DateTime.UtcNow + TimeSpan.FromMinutes(1);
                        break; 
                    }
                }
            }
		}

		public nun( Serial serial ) : base( serial )
		{
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