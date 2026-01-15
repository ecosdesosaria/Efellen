using System;
using Server;
using Server.Misc;
using Server.Items;
using System.Collections.Generic;
using Server.Custom.BalTsareth;

namespace Server.Mobiles 
{ 
	[CorpseName( "a possesed mage's corpse" )] 
	public class PossessedMage : BaseCreature 
	{ 
		private DateTime m_NextSpellTime;
		private bool m_IsChanneling;
		private Timer m_ChannelTimer;
		private Mobile m_ChannelTarget;
		private int m_ChannelSpell; // 0 = Arcane Armor, 1 = Fireball, 2 = Magic Missile, 3 = Web
		private DateTime m_ArcaneArmorEnds;
		private bool m_HasArcaneArmor;
		private int m_OriginalPhysicalResist;

		[Constructable] 
		public PossessedMage() : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 ) 
		{ 
			if ( this.Female = Utility.RandomBool() ) 
			{ 
				Body = 0x191; 
				Name = NameList.RandomName( "evil witch" );
				switch ( Utility.RandomMinMax( 0, 5 ) )
				{
					case 0: Title = "the possessed wizard"; break;
					case 1: Title = "the possessed sorcereress"; break;
					case 2: Title = "the possessed mage"; break;
					case 3: Title = "the possessed conjurer"; break;
					case 4: Title = "the possessed magician"; break;
					case 5: Title = "the possessed witch"; break;
				}
				Utility.AssignRandomHair( this );
				HairHue = Utility.RandomHairHue();
			} 
			else 
			{ 
				Body = 0x190; 
				Name = NameList.RandomName( "evil mage" );
				switch ( Utility.RandomMinMax( 0, 5 ) )
				{
					case 0: Title = "the wizard"; break;
					case 1: Title = "the sorcerer"; break;
					case 2: Title = "the mage"; break;
					case 3: Title = "the conjurer"; break;
					case 4: Title = "the magician"; break;
					case 5: Title = "the warlock"; break;
				}
				Utility.AssignRandomHair( this );
				int HairColor = Utility.RandomHairHue();
				FacialHairItemID = Utility.RandomList( 0, 8254, 8255, 8256, 8257, 8267, 8268, 8269 );
				HairHue = HairColor;
				FacialHairHue = HairColor;
			}

			Hue = Utility.RandomSkinColor();
			EmoteHue = 1;

			Server.Misc.IntelligentAction.DressUpWizards( this, false );
			Item[] items = new Item[] { this.FindItemOnLayer( Layer.InnerTorso ), this.FindItemOnLayer( Layer.OuterTorso ), 
				this.FindItemOnLayer( Layer.Pants ), this.FindItemOnLayer( Layer.Arms ), this.FindItemOnLayer( Layer.Gloves ), 
				this.FindItemOnLayer( Layer.Helm ), this.FindItemOnLayer( Layer.Neck ), this.FindItemOnLayer( Layer.Waist ), 
				this.FindItemOnLayer( Layer.OneHanded ), this.FindItemOnLayer( Layer.TwoHanded ), this.FindItemOnLayer( Layer.FirstValid ) };

			foreach ( Item item in items )
			{
				if ( item != null )
					item.Hue = 0x0213;
			}

			SetStr( 81, 105 );
			SetDex( 91, 115 );
			SetInt( 96, 120 );

			SetHits( 49, 63 );

			SetDamage( 5, 10 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 15, 20 );
			SetResistance( ResistanceType.Fire, 5, 10 );
			SetResistance( ResistanceType.Poison, 5, 10 );
			SetResistance( ResistanceType.Energy, 5, 10 );

			SetSkill( SkillName.Psychology, 75.1, 100.0 );
			SetSkill( SkillName.Magery, 75.1, 100.0 );
			SetSkill( SkillName.MagicResist, 75.0, 97.5 );
			SetSkill( SkillName.Tactics, 65.0, 87.5 );
			SetSkill( SkillName.FistFighting, 20.2, 60.0 );
			SetSkill( SkillName.Bludgeoning, 20.3, 60.0 );

			Fame = 6000;
			Karma = -6000;

			VirtualArmor = 30;
			PackReg( Utility.RandomMinMax( 2, 10 ) );
			PackReg( Utility.RandomMinMax( 2, 10 ) );
			PackReg( Utility.RandomMinMax( 2, 10 ) );

			if ( 0.7 > Utility.RandomDouble() )
				PackItem( new ArcaneGem() );

			if ( 0.7 > Utility.RandomDouble() )
				PackItem( new EerieIdol() );

			m_NextSpellTime = DateTime.UtcNow;
			m_IsChanneling = false;
			m_HasArcaneArmor = false;
		}

		public override void OnDamage( int amount, Mobile from, bool willKill )
		{
			base.OnDamage( amount, from, willKill );

			if ( m_IsChanneling )
			{
				InterruptChannel();
			}
		}

		private void InterruptChannel()
		{
			if ( !m_IsChanneling )
				return;

			m_IsChanneling = false;
			this.Frozen = false;
			Say("*The spell fizzles*");

			if ( m_ChannelTimer != null )
			{
				m_ChannelTimer.Stop();
				m_ChannelTimer = null;
			}

			m_ChannelTarget = null;
		}

		public override void OnThink()
		{
			base.OnThink();

			if ( m_HasArcaneArmor && DateTime.UtcNow >= m_ArcaneArmorEnds )
			{
				RemoveArcaneArmor();
			}

			if ( !m_IsChanneling && Combatant != null && Hits < HitsMax && 
			     DateTime.UtcNow >= m_NextSpellTime && Utility.RandomDouble() < 0.10 )
			{
				m_ChannelSpell = Utility.Random( 4 );
				m_ChannelTarget = Combatant;
				
				StartChanneling();
			}
		}

		private void StartChanneling()
		{
			m_IsChanneling = true;
			this.Frozen = true;
			Say("*starts channeling a spell*");

			m_ChannelTimer = Timer.DelayCall( TimeSpan.FromSeconds( 3 ), new TimerCallback( ExecuteSpell ) );
		}

		private void ExecuteSpell()
		{
			if ( !m_IsChanneling )
				return;

			m_IsChanneling = false;
			this.Frozen = false;

			switch ( m_ChannelSpell )
			{
				case 0: CastArcaneArmor(); break;
				case 1: CastFireball(); break;
				case 2: CastMagicMissile(); break;
				case 3: CastWeb(); break;
			}

			m_NextSpellTime = DateTime.UtcNow + TimeSpan.FromMinutes( 1 );
		}

		private void CastArcaneArmor()
		{
			Say("*Casts Arcane Armor*");

			Effects.SendLocationParticles( EffectItem.Create( this.Location, this.Map, EffectItem.DefaultDuration ), 0x376A, 9, 32, 0x0213 );
			Effects.PlaySound( this.Location, this.Map, 0x1F2 );

			m_OriginalPhysicalResist = this.GetResistance( ResistanceType.Physical );
			this.SetResistance( ResistanceType.Physical, 60 );
			
			m_HasArcaneArmor = true;
			m_ArcaneArmorEnds = DateTime.UtcNow + TimeSpan.FromSeconds( 60 );
		}

		private void RemoveArcaneArmor()
		{
			if ( !m_HasArcaneArmor )
				return;

			m_HasArcaneArmor = false;
			this.SetResistance( ResistanceType.Physical, m_OriginalPhysicalResist );
		}

		private void CastFireball()
		{
			if ( m_ChannelTarget == null || m_ChannelTarget.Deleted || !m_ChannelTarget.Alive )
				return;
			Say("*Casts Fireball*");

			Point3D targetLoc = m_ChannelTarget.Location;
			Map map = m_ChannelTarget.Map;

			Effects.SendLocationEffect( targetLoc, map, 0x36BD, 20, 10, 0x0213, 0 );
			Effects.PlaySound( targetLoc, map, 0x307 );

			for ( int x = -1; x <= 1; x++ )
			{
				for ( int y = -1; y <= 1; y++ )
				{
					Point3D loc = new Point3D( targetLoc.X + x, targetLoc.Y + y, targetLoc.Z );
					Effects.SendLocationEffect( loc, map, 0x36BD, 20, 10, 0x0213, 0 );
				}
			}

			IPooledEnumerable eable = m_ChannelTarget.GetMobilesInRange( 1 );
			List<Mobile> targets = new List<Mobile>();

			foreach ( Mobile m in eable )
			{
				if ( m != this && m.Alive && CanBeHarmful( m ) )
					targets.Add( m );
			}
			eable.Free();

			foreach ( Mobile m in targets )
			{
				int damage = Utility.RandomMinMax( 35, 45 );
				AOS.Damage( m, this, damage, 0, 100, 0, 0, 0 );
			}
		}

		private void CastMagicMissile()
		{
			if ( m_ChannelTarget == null || m_ChannelTarget.Deleted || !m_ChannelTarget.Alive )
				return;

			Say("*Casts Magic Missile*");

			int projectileCount = Utility.RandomMinMax( 2, 4 );
			
			for ( int i = 0; i < projectileCount; i++ )
			{
				Timer.DelayCall( TimeSpan.FromSeconds( i * 0.3 ), delegate()
				{
					if ( m_ChannelTarget != null && m_ChannelTarget.Alive && !m_ChannelTarget.Deleted )
					{
						Effects.SendMovingEffect( this, m_ChannelTarget, 0x379F, 7, 0, false, false, 0x0213, 0 );
						Effects.PlaySound( this.Location, this.Map, 0x1F5 );

						Timer.DelayCall( TimeSpan.FromSeconds( 0.5 ), delegate()
						{
							if ( m_ChannelTarget != null && m_ChannelTarget.Alive && !m_ChannelTarget.Deleted )
							{
								int damage = Utility.RandomMinMax( 5, 13 );
								AOS.Damage( m_ChannelTarget, this, damage, 0, 0, 0, 0, 100 );
								
								Effects.SendLocationEffect( m_ChannelTarget.Location, m_ChannelTarget.Map, 0x3709, 10, 30, 0x0213, 0 );
							}
						});
					}
				});
			}
		}

		private void CastWeb()
		{
			if ( m_ChannelTarget == null || m_ChannelTarget.Deleted || !m_ChannelTarget.Alive )
				return;

			Say("*Casts Web*");

			IPooledEnumerable eable = m_ChannelTarget.GetMobilesInRange( 2 );
			List<Mobile> targets = new List<Mobile>();

			foreach ( Mobile m in eable )
			{
				if ( m != this && m.Alive && CanBeHarmful( m ) )
					targets.Add( m );
			}
			eable.Free();

			foreach ( Mobile m in targets )
			{
				// paralyze duration: 15 - (magic resist / 10 + dex / 10), min 3 seconds
				double magicResist = m.Skills[SkillName.MagicResist].Value;
				int dex = m.Dex;
				double duration = 15.0 - (magicResist / 10.0 + dex / 10.0);
				
				if ( duration < 3.0 )
					duration = 3.0;

				m.Paralyze( TimeSpan.FromSeconds( duration ) );

				Effects.SendLocationParticles( EffectItem.Create( m.Location, m.Map, EffectItem.DefaultDuration ), 0x376A, 9, 10, 0x0213 );
				Effects.SendLocationEffect( m.Location, m.Map, 0x3709, 30, 10, 0x0213, 0 );
				Effects.PlaySound( m.Location, m.Map, 0x204 );

				m.SendMessage( "You are trapped in a magical web!" );
			}
		}

		public override void OnDelete()
		{
			if ( m_ChannelTimer != null )
			{
				m_ChannelTimer.Stop();
				m_ChannelTimer = null;
			}

			if ( m_HasArcaneArmor )
			{
				RemoveArcaneArmor();
			}

			base.OnDelete();
		}

		public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);
			if(Utility.RandomDouble() < 0.15)
            {
				Say(Server.Custom.BalTsareth.BalTsarethSpeech.GetAttackLine(defender));              
            }
        }

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Average );
			AddLoot( LootPack.LowScrolls );
			AddLoot( LootPack.MedScrolls );
			AddLoot( LootPack.MedPotions );
		}

		public override bool ClickTitle{ get{ return false; } }
		public override bool ShowFameTitle{ get{ return false; } }
		public override bool CanRummageCorpses{ get{ return true; } }
		public override bool AlwaysAttackable{ get{ return true; } }
		public override int Meat{ get{ return 1; } }
		public override int TreasureMapLevel{ get{ return Core.AOS ? 1 : 0; } }
		public override int Skeletal{ get{ return Utility.Random(3); } }
		public override SkeletalType SkeletalType{ get{ return SkeletalType.Brittle; } }

		public override void OnAfterSpawn()
		{
			Server.Misc.IntelligentAction.BeforeMyBirth( this );
			base.OnAfterSpawn();
		}

		public PossessedMage( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 1 ); // version

			writer.Write( m_NextSpellTime );
			writer.Write( m_IsChanneling );
			writer.Write( m_HasArcaneArmor );
			writer.Write( m_ArcaneArmorEnds );
			writer.Write( m_OriginalPhysicalResist );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();

			if ( version >= 1 )
			{
				m_NextSpellTime = reader.ReadDateTime();
				m_IsChanneling = reader.ReadBool();
				m_HasArcaneArmor = reader.ReadBool();
				m_ArcaneArmorEnds = reader.ReadDateTime();
				m_OriginalPhysicalResist = reader.ReadInt();

				if ( m_IsChanneling )
				{
					m_IsChanneling = false;
				}

				if ( m_HasArcaneArmor && DateTime.UtcNow >= m_ArcaneArmorEnds )
				{
					RemoveArcaneArmor();
				}
			}
		}
	}
}