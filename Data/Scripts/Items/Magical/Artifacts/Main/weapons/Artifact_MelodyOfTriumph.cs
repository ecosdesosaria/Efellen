using System;
using System.Collections;
using Server;
using Server.Mobiles;

namespace Server.Items
{
	public class MelodyOfTriumphTimer : Timer
	{
		private Mobile m_Mobile;
		private int m_Str;
		private int m_Dex;
		private int m_Int;

		public MelodyOfTriumphTimer( Mobile m, int str, int dex, int intel, TimeSpan duration ) : base( duration )
		{
			m_Mobile = m;
			m_Str = str;
			m_Dex = dex;
			m_Int = intel;
			Priority = TimerPriority.OneSecond;
		}

		protected override void OnTick()
		{
			m_Mobile.RemoveStatMod( "MelodyOfTriumph_Str" );
			m_Mobile.RemoveStatMod( "MelodyOfTriumph_Dex" );
			m_Mobile.RemoveStatMod( "MelodyOfTriumph_Int" );
			m_Mobile.Delta( MobileDelta.Stat );
		}
	}

	public class Artifact_MelodyOfTriumph : GiftShortSpear
	{
		public override int InitMinHits{ get{ return 80; } }
		public override int InitMaxHits{ get{ return 160; } }

		[Constructable]
		public Artifact_MelodyOfTriumph()
		{
			Name = "Melody of Triumph";
			Hue = 0x445;
			SkillBonuses.SetValues( 0, SkillName.Musicianship, 10 );
			SkillBonuses.SetValues( 1, SkillName.Tactics, 10 );
			WeaponAttributes.HitLowerAttack = 30;
			WeaponAttributes.HitLowerDefend = 30;
			Attributes.WeaponDamage = 10;
			ArtifactLevel = 2;
			Server.Misc.Arty.ArtySetup( this, "Canta com a alegria da batalha" );
		}

		public override void OnHit( Mobile attacker, Mobile defender, double damageBonus )
		{
            base.OnHit(attacker, defender, damageBonus);
			if (attacker == null || defender == null || attacker.Map == null || defender.Map == null || defender.Deleted || attacker.Deleted)
		        return;

			if ( Utility.RandomDouble() >= 0.05 )
				return;

			if ( attacker.GetStatMod( "MelodyOfTriumph_Str" ) != null )
				return;

			bool passed = false;
			int roll = Utility.Random( 3 );

			if ( roll == 0 )
				passed = attacker.CheckSkill( SkillName.Peacemaking, 0.0, 125.0 );
			else if ( roll == 1 )
				passed = attacker.CheckSkill( SkillName.Provocation, 0.0, 125.0 );
			else
				passed = attacker.CheckSkill( SkillName.Discordance, 0.0, 125.0 );

			if ( !passed )
				return;

			double peace  = attacker.Skills[SkillName.Peacemaking].Value;
			double prov   = attacker.Skills[SkillName.Provocation].Value;
			double disc   = attacker.Skills[SkillName.Discordance].Value;
			double music  = attacker.Skills[SkillName.Musicianship].Value;

			int bonus    = (int)( ( peace + prov + disc ) / 18.0 );
			double secs  = music / 2.0;

			if ( bonus < 1 )
				bonus = 1;

			attacker.AddStatMod( new StatMod( StatType.Str, "MelodyOfTriumph_Str", bonus, TimeSpan.Zero ) );
			attacker.AddStatMod( new StatMod( StatType.Dex, "MelodyOfTriumph_Dex", bonus, TimeSpan.Zero ) );
			attacker.AddStatMod( new StatMod( StatType.Int, "MelodyOfTriumph_Int", bonus, TimeSpan.Zero ) );

			MelodyOfTriumphTimer t = new MelodyOfTriumphTimer( attacker, bonus, bonus, bonus, TimeSpan.FromSeconds( secs ) );
			t.Start();

			attacker.SendMessage( "A Melodia do Triunfo canta de alegria!" );
            
		}

		public Artifact_MelodyOfTriumph( Serial serial ) : base( serial )
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
			ArtifactLevel = 2;
			int version = reader.ReadInt();
		}
	}
}