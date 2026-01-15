using System;
using Server;
using System.Collections; 
using Server.Items; 
using Server.ContextMenus; 
using Server.Misc; 
using Server.Network;
using Server.Mobiles;
using Server.Custom.BalTsareth;

namespace Server.Mobiles 
{
	public class PossessedBerserker : BaseCreature 
	{
		private DateTime m_NextRageAllowed;
		private DateTime m_RageEnds;
		private bool m_IsRaging;
		private Timer m_RageTimer;
		private Timer m_RegenerationTimer;

		[Constructable] 
		public PossessedBerserker() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 ) 
		{
			SpeechHue = Utility.RandomTalkHue();
			Hue = Utility.RandomSkinColor();
			if ( this.Female = Utility.RandomBool() ) 
			{
				this.Body = 0x191; 
				this.Name = NameList.RandomName( "female" ); 
				Utility.AssignRandomHair( this );
				HairHue = Utility.RandomHairHue();
			} 
			else 
			{ 
				this.Body = 0x190; 
				this.Name = NameList.RandomName( "male" ); 
				Utility.AssignRandomHair( this );
				FacialHairItemID = Utility.RandomList( 0, 8254, 8255, 8256, 8257, 8267, 8268, 8269 );
				HairHue = Utility.RandomHairHue();
				FacialHairHue = HairHue;
			}
			
			SetStr( Utility.RandomMinMax( 150, 170 ) );
			SetDex( Utility.RandomMinMax( 70, 90 ) );
			SetInt( Utility.RandomMinMax( 40, 60 ) );
			SetHits( RawStr );
			SetDamage( 8, 18 );
			SetDamageType( ResistanceType.Physical, 100 );
			SetResistance( ResistanceType.Physical, 10 );
			SetResistance( ResistanceType.Fire, 0 );
			SetResistance( ResistanceType.Cold, 0 );
			SetResistance( ResistanceType.Poison, 0 );
			SetResistance( ResistanceType.Energy, 0 );
			SetSkill( SkillName.Searching, 20.0 );
			SetSkill( SkillName.Anatomy, 50.0 );
			SetSkill( SkillName.MagicResist, 10.0 );
			SetSkill( SkillName.Bludgeoning, 60.0 );
			SetSkill( SkillName.Fencing, 60.0 );
			SetSkill( SkillName.FistFighting, 60.0 );
			SetSkill( SkillName.Swords, 60.0 );
			SetSkill( SkillName.Tactics, 60.0 );
			
			Fame = 100;
			Karma = -100;
			VirtualArmor = 0;

			m_NextRageAllowed = DateTime.UtcNow;
			m_IsRaging = false;
           
            if ( 0.5 > Utility.RandomDouble() )
				PackItem( new EerieIdol() );
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

			RawStr += 30;
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
				Hits += 12;
			}
		}

		private void EndRage()
		{
			if ( !m_IsRaging )
				return;

			m_IsRaging = false;

			RawStr -= 30;
			RawInt += 20;

			Stam = Stam / 2;

			if ( m_RegenerationTimer != null )
			{
				m_RegenerationTimer.Stop();
				m_RegenerationTimer = null;
			}

			m_NextRageAllowed = DateTime.UtcNow + TimeSpan.FromMinutes( 2 );
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

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Average );
			AddLoot( LootPack.Average );
			AddLoot( LootPack.Meager );
		}

		public override bool ClickTitle{ get{ return false; } }
		public override bool ShowFameTitle{ get{ return false; } }
		public override bool CanRummageCorpses{ get{ return true; } }
		public override bool AlwaysAttackable{ get{ return true; } }
		public override int Meat{ get{ return 1; } }
		public override int Skeletal{ get{ return Utility.Random(3); } }
		public override SkeletalType SkeletalType{ get{ return SkeletalType.Brittle; } }

		public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);
			if(Utility.RandomDouble() < 0.15)
            {
				Say(Server.Custom.BalTsareth.BalTsarethSpeech.GetAttackLine(defender));              
            }
        }

		public override void OnAfterSpawn()
		{
			Server.Misc.IntelligentAction.ChooseFighter( this, "" );
			
			Item[] items = new Item[] { this.FindItemOnLayer( Layer.InnerTorso ), this.FindItemOnLayer( Layer.OuterTorso ), 
				this.FindItemOnLayer( Layer.Pants ), this.FindItemOnLayer( Layer.Arms ), this.FindItemOnLayer( Layer.Gloves ), 
				this.FindItemOnLayer( Layer.Helm ), this.FindItemOnLayer( Layer.Neck ), this.FindItemOnLayer( Layer.Waist ), 
				this.FindItemOnLayer( Layer.OneHanded ), this.FindItemOnLayer( Layer.TwoHanded ), this.FindItemOnLayer( Layer.FirstValid ) };

			foreach ( Item item in items )
			{
				if ( item != null )
					item.Hue = 0x0213;
			}
			
			base.OnAfterSpawn();
		}

		public PossessedBerserker( Serial serial ) : base( serial ) 
		{ 
		} 

		public override void Serialize( GenericWriter writer ) 
		{ 
			base.Serialize( writer ); 
			writer.Write( (int) 1 ); // version 

			writer.Write( m_NextRageAllowed );
			writer.Write( m_IsRaging );
			writer.Write( m_RageEnds );
		}

		public override void Deserialize( GenericReader reader ) 
		{ 
			base.Deserialize( reader ); 
			int version = reader.ReadInt(); 

			if ( version >= 1 )
			{
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
}