using System; 
using Server;
using Server.Items;
using Server.Custom.DailyBosses.System;
using Server.CustomSpells;

namespace Server.Mobiles 
{ 
	[CorpseName( "an archmage corpse" )] 
	public class Archmage : BaseSpellCaster 
	{ 
		private DateTime m_NextSpecialAttack = DateTime.MinValue;
		
		[Constructable] 
		public Archmage() : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 ) 
		{
			Body = 0x190; 
			Name = NameList.RandomName( "evil mage" );
			Title = "the mad archmage";
			EmoteHue = 11;

			Hue = Utility.RandomSkinColor();
			Utility.AssignRandomHair( this );
			int HairColor = Utility.RandomHairHue();
			FacialHairItemID = Utility.RandomList( 0, 8254, 8255, 8256, 8257, 8267, 8268, 8269 );
			HairHue = HairColor;
			FacialHairHue = HairColor;

			Robe robe = new Robe( );
				robe.Hue = 0xA2A;
				robe.LootType = LootType.Blessed;
				AddItem( robe );

			WizardsHat hat = new WizardsHat( );
				hat.Hue = 0xA2A;
				hat.LootType = LootType.Blessed;
				AddItem( hat );

			Item boots = new ThighBoots( );
				boots.Hue = 0x497;
				boots.LootType = LootType.Blessed;
				AddItem( boots );

			if ( Utility.RandomMinMax( 1, 4 ) > 1 )
			{
				QuarterStaff staff = new QuarterStaff();
				staff.Name = "staff";
				staff.ItemID = Utility.RandomList( 0xDF0, 0x13F8, 0xE89, 0x2D25, 0x26BC, 0x26C6, 0xDF2, 0xDF3, 0xDF4, 0xDF5 );
				if ( staff.ItemID == 0x26BC || staff.ItemID == 0x26C6 ){ staff.Name = "scepter"; }
				if ( staff.ItemID == 0xDF2 || staff.ItemID == 0xDF3 || staff.ItemID == 0xDF4 || staff.ItemID == 0xDF5 ){ staff.Name = "magic wand"; }
				staff.LootType = LootType.Blessed;
				staff.Attributes.SpellChanneling = 1;
				AddItem( staff );
			}

			SetStr( 986, 1185 );
			SetDex( 177, 255 );
			SetInt( 196, 220 );

			SetHits( 592, 711 );

			SetDamage( 22, 29 );

			SetDamageType( ResistanceType.Physical, 50 );
			SetDamageType( ResistanceType.Fire, 25 );
			SetDamageType( ResistanceType.Energy, 25 );

			SetResistance( ResistanceType.Physical, 65, 80 );
			SetResistance( ResistanceType.Fire, 60, 80 );
			SetResistance( ResistanceType.Cold, 50, 60 );
			SetResistance( ResistanceType.Poison, 100 );
			SetResistance( ResistanceType.Energy, 40, 50 );

			SetSkill( SkillName.Anatomy, 25.1, 50.0 );
			SetSkill( SkillName.Psychology, 1110.0 );
			SetSkill( SkillName.Magery, 111.0 );
			SetSkill( SkillName.Meditation, 80.0 );
			SetSkill( SkillName.MagicResist, 110.5, 150.0 );
			SetSkill( SkillName.Tactics, 90.1, 100.0 );
			SetSkill( SkillName.FistFighting, 90.1, 100.0 );

			Fame = 24000;
			Karma = -24000;

			VirtualArmor = 90;

			PackReg( 30, 275 );
		}

		public override void OnDamage( int amount, Mobile from, bool willKill )
		{
			if ( DateTime.UtcNow >= m_NextSpecialAttack )
			{
				PerformRageAttack( from );
				m_NextSpecialAttack = DateTime.UtcNow + TimeSpan.FromSeconds( 45 );
			}
			
			base.OnDamage( amount, from, willKill );
		}

		private void PerformRageAttack( Mobile target )
		{
			if ( target == null || target.Deleted || !target.Alive )
				return;

			int attackChoice = Utility.RandomMinMax( 1, 3 );
            Map map = this.Map;

			switch ( attackChoice  )
			{
				case 1: // energy burst
				{
					BossSpecialAttack.PerformTargettedAoE(
						this,
						target,
						1,
						"I shall unravel you!",
						0xA2A,  // hue
						20,     // physical
						20,   // fire
						20,     // cold
						20,     // poison
						20      // energy
					);
					break;
				}
				case 2: // energy nova
				{
					BossSpecialAttack.PerformCrossExplosion(
					    boss: this,
					    target: target,
					    warcry: "Taste my power!",
					    hue: 0xA2A,
					    rage: 2,
					    coldDmg: 20,
					    fireDmg: 20,
					    energyDmg: 20,
					    poisonDmg: 20,
					    physicalDmg: 20
					);
                	break;
			    }
				case 3: // energy nova
				{
					BossSpecialAttack.PerformSlam(
                	    boss: this,
                	    warcry: "The Weave is mine to control",
                	    hue: 0xA2A,
                	    rage: 2,
                	    range: 6,
                	    physicalDmg: 0,
						energyDmg: 100
                	);
                	break;
			    }
			}
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.FilthyRich, 2 );
			AddLoot( LootPack.Rich );
			AddLoot( LootPack.MedScrolls, 2 );
			AddLoot( LootPack.MedPotions );
		}

		public override bool ClickTitle{ get{ return false; } }
		public override bool ShowFameTitle{ get{ return false; } }
		public override bool AlwaysAttackable{ get{ return true; } }
		public override int Meat{ get{ return 1; } }
		public override bool Unprovokable{ get{ return true; } }
		public override Poison PoisonImmune{ get{ return Poison.Deadly; } }
		public override int TreasureMapLevel{ get{ return 5; } }
		public override int Skeletal{ get{ return Utility.Random(3); } }
		public override SkeletalType SkeletalType{ get{ return SkeletalType.Brittle; } }

		public void AddArcane( Item item )
		{
			if ( item is IArcaneEquip )
			{
				IArcaneEquip eq = (IArcaneEquip)item;
				eq.CurArcaneCharges = eq.MaxArcaneCharges = 20;
			}

			item.Hue = ArcaneGem.DefaultArcaneHue;
			item.LootType = LootType.Newbied;

			AddItem( item );
		}

		public override void OnAfterSpawn()
		{
			Server.Misc.IntelligentAction.BeforeMyBirth( this );
			this.MobileMagics(Utility.Random(5,8), SpellType.Wizard | SpellType.Sorcerer, 0);
			base.OnAfterSpawn();
		}

		public override void OnGotMeleeAttack( Mobile attacker )
		{
			base.OnGotMeleeAttack( attacker );
			Server.Misc.IntelligentAction.DoSpecialAbility( this, attacker );
			Server.Misc.IntelligentAction.CryOut( this );
		}

		public override void OnGaveMeleeAttack( Mobile defender )
		{
			base.OnGaveMeleeAttack( defender );
			Server.Misc.IntelligentAction.DoSpecialAbility( this, defender );
		}

		public override bool OnBeforeDeath()
		{
			Server.Misc.IntelligentAction.BeforeMyDeath( this );
			return base.OnBeforeDeath();
		}

		public Archmage( Serial serial ) : base( serial ) 
		{ 
		} 

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 2 );
			writer.Write( m_NextSpecialAttack );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
			if ( version >= 1 )
			{
				m_NextSpecialAttack = reader.ReadDateTime();
			}
			if ( version >= 2 )
			{
				this.MobileMagics(Utility.Random(5,8), SpellType.Wizard | SpellType.Sorcerer, 0);
			}
		}
	} 
}