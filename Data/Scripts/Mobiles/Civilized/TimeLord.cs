using System;
using System.Collections;
using Server.Items;
using Server.ContextMenus;
using Server.Misc;
using Server.Network;

namespace Server.Mobiles
{
	public class TimeLord : BasePerson
	{
		private DateTime m_NextTalk;
		public DateTime NextTalk{ get{ return m_NextTalk; } set{ m_NextTalk = value; } }

		public override void OnMovement( Mobile m, Point3D oldLocation )
		{
			if( m is PlayerMobile )
			{
				if ( DateTime.Now >= m_NextTalk && InRange( m, 4 ) && InLOS( m ) )
				{
					switch ( Utility.Random( 9 ))
					{
						case 0: Say("O Estranho salvou Sosaria de Exodus."); break;
						case 1: Say("O Castelo de Exodus está em ruínas, ninguém sabe que mal habita abaixo."); break;
						case 2: Say("O legado de Mondain está extinto para sempre."); break;
						case 3: Say("A linha do tempo foi restaurada, após a ira de Minax."); break;
						case 4: Say("Um dia o Estranho retornará a Sosaria."); break;
						case 5: Say("Embora alguns falem de virtude, são as serpentes da ordem que mantêm o equilíbrio."); break;
						case 6: Say("A ordem era Amor, Sol, Lua e Morte."); break;
						case 7: Say("Talvez um dia o Estranho alcance o Avatar."); break;
						case 8: Say("Os fios do tempo mostram que o Guardião está chegando."); break;
					};

					m_NextTalk = (DateTime.Now + TimeSpan.FromSeconds( 30 ));
				}
			}
		}

		[Constructable]
		public TimeLord() : base( )
		{
			SpeechHue = Utility.RandomTalkHue();
			NameHue = -1;
			Body = 0x190;
			Hue = 0x430;
			Name = "the Time Lord";
			Blessed = true;

			AddItem( new Sandals() );
			AddItem( new ClothCowl() );
			AddItem( new SorcererRobe() );

			SetStr( 3000, 3000 );
			SetDex( 3000, 3000 );
			SetInt( 3000, 3000 );

			SetHits( 6000,6000 );
			SetDamage( 500, 900 );

			VirtualArmor = 3000;

			SetDamageType( ResistanceType.Physical, 40 );
			SetDamageType( ResistanceType.Cold, 60 );
			SetDamageType( ResistanceType.Energy, 60 );

			SetResistance( ResistanceType.Physical, 65, 75 );
			SetResistance( ResistanceType.Fire, 35, 40 );
			SetResistance( ResistanceType.Cold, 60, 70 );
			SetResistance( ResistanceType.Poison, 60, 70 );
			SetResistance( ResistanceType.Energy, 35, 40 );

			SetSkill( SkillName.Psychology, 130.1, 140.0 );
			SetSkill( SkillName.Magery, 130.1, 140.0 );
			SetSkill( SkillName.Meditation, 110.1, 111.0 );
			SetSkill( SkillName.Poisoning, 110.1, 111.0 );
			SetSkill( SkillName.MagicResist, 185.2, 210.0 );
			SetSkill( SkillName.Tactics, 100.1, 110.0 );
			SetSkill( SkillName.FistFighting, 85.1, 110.0 );
			SetSkill( SkillName.Bludgeoning, 85.1, 110.0 );
		}

		public override bool BardImmune{ get{ return true; } }
		public override Poison PoisonImmune{ get{ return Poison.Deadly; } }
		public override bool Unprovokable { get { return true; } }
		public override bool Uncalmable{ get{ return true; } }

		public TimeLord( Serial serial ) : base( serial )
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