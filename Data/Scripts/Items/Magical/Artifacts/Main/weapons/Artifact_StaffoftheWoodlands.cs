using System;
using Server;
using Server.Items;
using Server.Spells.Magical;
using Server.Targeting;

namespace Server.Items
{
	public class Artifact_StaffoftheWoodlands : GiftShepherdsCrook
	{
		public DateTime m_TimeUsed;
		public override int InitMinHits{ get{ return 80; } }
		public override int InitMaxHits{ get{ return 160; } }

		[Constructable]
		public Artifact_StaffoftheWoodlands() : base()
		{
			Name = "Staff of the Woodlands";
			Hue = 0x8A0;
			
			Attributes.SpellChanneling = 1;
			Attributes.RegenStam = 5;
			Attributes.RegenHits = 5;
			Attributes.RegenMana = 5;
			Attributes.DefendChance = 15;
			SkillBonuses.SetValues(0, SkillName.Druidism,  15);
			SkillBonuses.SetValues(2, SkillName.Herding,  15);
			ArtifactLevel = 2;
			Server.Misc.Arty.ArtySetup( this, "Calls forth the spirit of a dire bear" );
			m_TimeUsed = DateTime.MinValue;
		}

		public override void OnDoubleClick( Mobile from )
		{
			DateTime TimeNow = DateTime.Now;
			long ticksThen = m_TimeUsed.Ticks;
			long ticksNow = TimeNow.Ticks;
			int minsThen = (int)TimeSpan.FromTicks(ticksThen).TotalMinutes;
			int minsNow = (int)TimeSpan.FromTicks(ticksNow).TotalMinutes;
			int CanUseMagic = 30 - ( minsNow - minsThen );

			if ( Parent != from )
			{
				from.SendMessage( "Você precisa estar segurando o arco para invocar um urso feroz." );
			}
			else if ( CanUseMagic > 0 )
			{
				TimeSpan t = TimeSpan.FromMinutes( CanUseMagic );
				string wait = string.Format("{0:D1} horas e {1:D2} minutos", 
								t.Hours, 
								t.Minutes);
				from.SendMessage( "Você pode usar a magia novamente em " + wait + "." );
			}
			else
			{
				new SummonDireBearSpell( from, this ).Cast();
				m_TimeUsed = DateTime.Now;
			}
		}

		public override void GetDamageTypes( Mobile wielder, out int phys, out int fire, out int cold, out int pois, out int nrgy, out int chaos, out int direct )
		{
			chaos = fire = cold = pois = nrgy = direct = 0;
			phys = 100;
		}

		public Artifact_StaffoftheWoodlands( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.WriteEncodedInt( 1 );
			writer.Write(m_TimeUsed);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			
			ArtifactLevel = 2;
			
			int version = reader.ReadEncodedInt();
			
			if (version >= 1)
				m_TimeUsed = reader.ReadDateTime();
			else
				m_TimeUsed = DateTime.MinValue;
		}
	}
}
