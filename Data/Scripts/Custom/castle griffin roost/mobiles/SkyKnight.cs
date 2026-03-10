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
			SetDamage( 18, 24 );
			VirtualArmor = 50;
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
		    MorphingTime.ColorMyArms(this, 0x0672, 0);
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
			"Morra vilão!",
			"Eu trarei justiça a você!",
			"Então, {0}? Seu mal termina aqui!",
			"Disseram-nos para vigiar {0}!",
			"Cavaleiros, {0} está aqui!",
			"Temos maneiras de lidar com a laia de {0}!",
			"Renda-se! Não tememos {0}!",
			"Então, {0}? Eu o sentencio à morte!",
			"Enfrente o julgamento dos Cavaleiros do Céu!",
			"Pelo reino, eu o derrubo!",
			"Seu caminho perverso termina sob nossas asas!",
			"Mantenha-se firme, grifo! A justiça nos chama adiante!",
			"Ousa erguer aço contra a guarda dos céus?",
			"Em nome da honra, caia diante de nós!",
			"Os próprios céus negam misericórdia a você, {0}!",
			"Sinta a ira de um cavaleiro jurado aos céus!",
			"Os grifos sentem sua corrupção, {0}!",
			"Por juramento e virtude, trago luz à sua escuridão!",
			"Seus crimes ecoam pelos céus, {0}!",
			"Os grifos arrancarão a corrupção de sua carne!",
			"A justiça desce sobre você!",
			"Os grifos clamam por sua queda!",
			"Você se opõe à Ordem do Céu — insensato!",
			"Seu destino foi selado no momento em que apareceu, {0}!",
			"Os céus o julgam indigno!",
			"Sua escuridão vacila sob nossas asas!",
			"Pela Lira do Anjo, caia!",
			"Nossa vigilância acaba com você aqui, {0}!",
			"Você não pode fugir de nós!",
			"A tempestade atende nosso chamado, {0}!",
			"Voamos com propósito, e você cairá diante dele!",
			"Sua maldade não mancha mais o reino!",
			"A luz dos céus queima você, {0}!",
			"Seu caminho termina sob nossa investida justa!",
			"Os grifos anseiam pela vitória, renda-se malfeitor!",
			"Você não pode enfrentar o poder dos Cavaleiros do Céu!",
			"O céu rejeita sua maldade, {0}!",
			"Cavalgo o vento da justiça — prepare-se!"
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
		        "* O cavaleiro golpeia " + defender.Name + " com seu escudo! *"
		    );

		    defender.SendMessage("Você é atingido por uma pancada esmagadora de escudo!");

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

		public override void OnDeath(Container c)
		{
		    base.OnDeath(c);
		    BossLootSystem.BossEnchant(this, c, 300, 15, 1, "skyknight");
		}

		private void DoMountedArmorIgnore(Mobile defender)
		{
		    if (defender == null || defender.Deleted || !defender.Alive)
		        return;

		    m_NextArmorIgnore = DateTime.UtcNow + TimeSpan.FromMinutes(1.5);
		    this.PublicOverheadMessage(MessageType.Regular, 0x22, false,
		        "O grifo dilacera " + defender.Name + " com suas garras afiadas!");
		
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
		private bool IsFriendlyCreature(Mobile m)
		{
			return 	m is HeavenlyMarshall || 
					m is SkyKnight || 
					m is Angel || 
					m is Archangel ||
					m is WarGriffon || 
					m is EtherealWarriorGeneral;
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