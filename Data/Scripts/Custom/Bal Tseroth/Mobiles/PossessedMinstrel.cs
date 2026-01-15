using System;
using Server;
using System.Collections;
using Server.Items;
using Server.ContextMenus;
using Server.Misc;
using Server.Network;
using System.Collections.Generic;
using Server.Custom.BalTsareth;

namespace Server.Mobiles
{
	public class PossessedMinstrel : BaseCreature
	{
		private DateTime m_NextAbilityTime;
		private bool m_IsChanneling;
		private Timer m_ChannelTimer;
		private Mobile m_ChannelTarget;
		private int m_ChannelAbility; // 0 = Sound Burst, 1 = Shatter

		[Constructable]
		public PossessedMinstrel() : base( AIType.AI_Archer, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			switch ( Utility.RandomMinMax( 0, 4 ) )
			{
				case 0: Title = "the  Possessed bard"; break;
				case 1: Title = "the Possessed Minstrel"; break;
				case 2: Title = "the  Possessed troubadour"; break;
				case 3: Title = "the  Possessed musician"; break;
				case 4: Title = "the  Possessed balladeer"; break;
			}

			Hue = Utility.RandomSkinColor();

			if ( this.Female = Utility.RandomBool() )
			{
				Body = 0x191;
				Name = NameList.RandomName( "female" );
				AddItem( new Skirt( Utility.RandomColor(0) ) );
				Utility.AssignRandomHair( this );
				HairHue = Utility.RandomHairHue();
			}
			else
			{
				Body = 0x190;
				Name = NameList.RandomName( "male" );
				AddItem( new ShortPants( Utility.RandomColor(0) ) );
				Utility.AssignRandomHair( this );
				int HairColor = Utility.RandomHairHue();
				FacialHairItemID = Utility.RandomList( 0, 8254, 8255, 8256, 8257, 8267, 8268, 8269 );
				HairHue = HairColor;
				FacialHairHue = HairColor;
			}

			SetStr( 86, 100 );
			SetDex( 81, 95 );
			SetInt( 61, 75 );

			SetDamage( 10, 23 );

			SetSkill( SkillName.Marksmanship, 80.1, 90.0 );
			SetSkill( SkillName.FistFighting, 66.0, 97.5 );
			SetSkill( SkillName.MagicResist, 25.0, 47.5 );
			SetSkill( SkillName.Tactics, 65.0, 87.5 );
			SetSkill( SkillName.Musicianship, 65.0, 87.5 );

			Fame = 2000;
			Karma = -2000;

			AddItem( new Boots( ( 0x0213 ) ) );
			AddItem( new FancyShirt( 0x0213 ) );
			
			switch ( Utility.Random( 4 ))
			{
				case 0: AddItem( new FeatheredHat( 0x0213 ) ); break;
				case 1: AddItem( new FloppyHat( 0x0213 ) ); break;
				case 2: AddItem( new StrawHat( 0x0213 ) ); break;
				case 3: AddItem( new SkullCap( 0x0213 ) ); break;
			}

			switch ( Utility.Random( 2 ))
			{
				case 0: AddItem( new Crossbow(0x0213) ); PackItem( new Bolt( Utility.RandomMinMax( 5, 15 ) ) ); break;
				case 1: AddItem( new Bow(0x0213) ); PackItem( new Arrow( Utility.RandomMinMax( 5, 15 ) ) ); break;
			}

			switch ( Utility.Random( 6 ))
			{
				case 0: PackItem( new BambooFlute() );	SpeechHue = 0x504;	break;
				case 1: PackItem( new Drums() );		SpeechHue = 0x38;	break;
				case 2: PackItem( new Tambourine() );	SpeechHue = 0x52;	break;
				case 3: PackItem( new LapHarp() );		SpeechHue = 0x45;	break;
				case 4: PackItem( new Lute() );			SpeechHue = 0x4C;	break;
				case 5: PackItem( new Trumpet() );		SpeechHue = 0x3CE;	break;
			}

			Server.Misc.IntelligentAction.GiveAdventureGear( this );

			Item[] items = new Item[] { this.FindItemOnLayer( Layer.InnerTorso ), this.FindItemOnLayer( Layer.OuterTorso ), 
				this.FindItemOnLayer( Layer.Pants ), this.FindItemOnLayer( Layer.Arms ), this.FindItemOnLayer( Layer.Gloves ), 
				this.FindItemOnLayer( Layer.Helm ), this.FindItemOnLayer( Layer.Neck ), this.FindItemOnLayer( Layer.Waist ), 
				this.FindItemOnLayer( Layer.OneHanded ), this.FindItemOnLayer( Layer.TwoHanded ), this.FindItemOnLayer( Layer.FirstValid ) };

			foreach ( Item item in items )
			{
				if ( item != null )
					item.Hue = 0x0213;
			}

			m_NextAbilityTime = DateTime.UtcNow;
			m_IsChanneling = false;
			if ( 0.5 > Utility.RandomDouble() )
				PackItem( new EerieIdol() );
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
			PublicOverheadMessage( MessageType.Emote, 0x21, false, "*The spell fizzles*" );

			if ( m_ChannelTimer != null )
			{
				m_ChannelTimer.Stop();
				m_ChannelTimer = null;
			}

			m_ChannelTarget = null;
		}

		public override void OnActionCombat()
		{
			base.OnActionCombat();

			if ( !m_IsChanneling && Combatant != null && DateTime.UtcNow >= m_NextAbilityTime && Utility.RandomDouble() < 0.50 )
			{
				m_ChannelAbility = Utility.Random( 2 );
				m_ChannelTarget = Combatant;
				
				if ( m_ChannelAbility == 0 )
				{
					StartSoundBurst();
				}
				else
				{
					StartShatter();
				}
			}
		}

		private void StartSoundBurst()
		{
			m_IsChanneling = true;
			PublicOverheadMessage( MessageType.Emote, 0x21, false, "*Channeling Soundburst*" );

			m_ChannelTimer = Timer.DelayCall( TimeSpan.FromSeconds( 2 ), new TimerCallback( ExecuteSoundBurst ) );
		}

		private void ExecuteSoundBurst()
		{
			if ( !m_IsChanneling || m_ChannelTarget == null || m_ChannelTarget.Deleted || !m_ChannelTarget.Alive )
			{
				m_IsChanneling = false;
				return;
			}

			m_IsChanneling = false;

			Point3D targetLoc = m_ChannelTarget.Location;
			Map map = m_ChannelTarget.Map;

			Effects.SendLocationEffect( targetLoc, map, 0x36BD, 20, 10, 0x0213, 0 );

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
				int damage = Utility.RandomMinMax( 15, 35 );
				AOS.Damage( m, this, damage, 0, 0, 0, 0, 100 );
			}

			m_NextAbilityTime = DateTime.UtcNow + TimeSpan.FromSeconds( 90 );
		}

		private void StartShatter()
		{
			m_IsChanneling = true;
			PublicOverheadMessage( MessageType.Emote, 0x21, false, "*Channeling Shatter*" );

			m_ChannelTimer = Timer.DelayCall( TimeSpan.FromSeconds( 2 ), new TimerCallback( ExecuteShatter ) );
		}

		private void ExecuteShatter()
		{
			if ( !m_IsChanneling || m_ChannelTarget == null || m_ChannelTarget.Deleted || !m_ChannelTarget.Alive )
			{
				m_IsChanneling = false;
				return;
			}

			m_IsChanneling = false;

			Effects.SendMovingEffect( this, m_ChannelTarget, 0x379F, 7, 0, false, false, 0x0213, 0 );

			Timer.DelayCall( TimeSpan.FromSeconds( 0.5 ), delegate()
			{
				if ( m_ChannelTarget != null && m_ChannelTarget.Alive )
				{
					int damage = Utility.RandomMinMax( 25, 45 );
					AOS.Damage( m_ChannelTarget, this, damage, 0, 0, 0, 0, 100 );
					
					Effects.SendLocationEffect( m_ChannelTarget.Location, m_ChannelTarget.Map, 0x36BD, 20, 10, 0x0213, 0 );
				}
			});

			m_NextAbilityTime = DateTime.UtcNow + TimeSpan.FromSeconds( 90 );
		}

		public override void OnDelete()
		{
			if ( m_ChannelTimer != null )
			{
				m_ChannelTimer.Stop();
				m_ChannelTimer = null;
			}

			base.OnDelete();
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Average );
			AddLoot( LootPack.Songs );
		}

		public override bool AlwaysAttackable{ get{ return true; } }
		public override bool ClickTitle{ get{ return false; } }
		public override bool ShowFameTitle{ get{ return false; } }
		public override int Skeletal{ get{ return Utility.Random(3); } }
		public override SkeletalType SkeletalType{ get{ return SkeletalType.Brittle; } }

		public override void OnThink()
		{
			if ( DateTime.Now >= NextPickup )
			{
				switch( Utility.RandomMinMax( 0, 3 ) )
				{
					case 0:	Peace( Combatant ); break;
					case 1:	Undress( Combatant ); break;
					case 2:	Suppress( Combatant ); break;
					case 3:	Provoke( Combatant ); break;
				}
			}
			base.OnThink();
		}

		public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);
			if(Utility.RandomDouble() < 0.15)
            {
				Say(Server.Custom.BalTsareth.BalTsarethSpeech.GetAttackLine(defender));              
            }
        }

		public PossessedMinstrel( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 1 ); // version

			writer.Write( m_NextAbilityTime );
			writer.Write( m_IsChanneling );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();

			if ( version >= 1 )
			{
				m_NextAbilityTime = reader.ReadDateTime();
				m_IsChanneling = reader.ReadBool();

				if ( m_IsChanneling )
				{
					m_IsChanneling = false;
				}
			}
		}
	}
}