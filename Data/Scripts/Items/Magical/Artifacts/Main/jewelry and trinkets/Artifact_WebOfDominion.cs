using System;
using Server.Mobiles;

namespace Server.Items
{
	public class Artifact_WebOfDominion : GiftGoldNecklace
	{
		private DateTime m_NextSummonTime;
		private Mobile m_Wearer;
		private int m_LastHits;
		private InternalTimer m_Timer;

		[Constructable]
		public Artifact_WebOfDominion()
		{
			Name = "Web Of Dominion";
			Hue = 1316;
			SkillBonuses.SetValues( 0, SkillName.Spiritualism, ( 20.0 ) );
			SkillBonuses.SetValues( 1, SkillName.Poisoning, ( 20.0 ) );
			SkillBonuses.SetValues( 2, SkillName.MagicResist, ( 20.0 ) );
			SkillBonuses.SetValues( 3, SkillName.Magery, ( 20.0 ) );
			Resistances.Poison = 25;
			ArtifactLevel = 2;
			m_NextSummonTime = DateTime.MinValue;
			Server.Misc.Arty.ArtySetup( this, "Lolths eternal cruelty" );
		}

		public override void OnAdded(object parent)
		{
			base.OnAdded(parent);

			if (parent is Mobile)
			{
				m_Wearer = (Mobile)parent;
				m_LastHits = m_Wearer.Hits;
				
				if (m_Timer != null)
					m_Timer.Stop();
				
				m_Timer = new InternalTimer(this);
				m_Timer.Start();
			}
		}

        public override bool OnEquip(Mobile from)
		{
			if (from.Karma >= -11999)
			{
				from.SendMessage("This vile implement judges you unworthy!");
				from.ApplyPoison(from, Poison.Deadly);
				return false;
			}

			return base.OnEquip(from);
		}

		public override void OnRemoved(object parent)
		{
			base.OnRemoved(parent);

			if (m_Timer != null)
			{
				m_Timer.Stop();
				m_Timer = null;
			}

			m_Wearer = null;
		}

		private void CheckForDamage()
		{
			if (m_Wearer == null || m_Wearer.Deleted || !m_Wearer.Alive)
				return;

			if (m_Wearer.Hits < m_LastHits)
			{
				if (DateTime.UtcNow >= m_NextSummonTime)
				{
					if (m_Wearer.Karma < -11999)
					{
						if (Utility.RandomDouble() <= 0.15)
						{
							SummonSpinner();
						}
					}
				}
			}

			m_LastHits = m_Wearer.Hits;
		}

		private void SummonSpinner()
		{
			if (m_Wearer == null || m_Wearer.Map == null)
				return;

			DemonwebSpinner spinner = new DemonwebSpinner();

			Point3D loc = m_Wearer.Location;
			Map map = m_Wearer.Map;

			for (int i = 0; i < 10; i++)
			{
				int x = m_Wearer.X + Utility.RandomMinMax(-2, 2);
				int y = m_Wearer.Y + Utility.RandomMinMax(-2, 2);
				int z = map.GetAverageZ(x, y);

				if (map.CanSpawnMobile(x, y, z))
				{
					loc = new Point3D(x, y, z);
					break;
				}
			}

			spinner.MoveToWorld(loc, map);

			if (m_Wearer.Combatant != null)
			{
				spinner.Combatant = m_Wearer.Combatant;
			}

			Effects.SendLocationParticles(
				EffectItem.Create(spinner.Location, spinner.Map, EffectItem.DefaultDuration),
				0x3728, 10, 10, 2023
			);
			spinner.PlaySound(0x218);

			m_Wearer.SendMessage("Lolth has taken an interest in your suffering.");

            m_Wearer.ApplyPoison(m_Wearer, Poison.Deadly);

			Timer.DelayCall(TimeSpan.FromMinutes(1.0), delegate()
			{
				if (spinner != null && !spinner.Deleted)
				{
					Effects.SendLocationParticles(
						EffectItem.Create(spinner.Location, spinner.Map, EffectItem.DefaultDuration),
						0x3728, 10, 10, 2023
					);
					spinner.PlaySound(0x209);
					spinner.Delete();
				}
			});
			m_NextSummonTime = DateTime.UtcNow + TimeSpan.FromMinutes(4);
		}

		public override void OnDelete()
		{
			if (m_Timer != null)
			{
				m_Timer.Stop();
				m_Timer = null;
			}

			base.OnDelete();
		}

		private class InternalTimer : Timer
		{
			private Artifact_WebOfDominion m_Item;

			public InternalTimer(Artifact_WebOfDominion item) : base(TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(10))
			{
				m_Item = item;
				Priority = TimerPriority.FiftyMS;
			}

			protected override void OnTick()
			{
				if (m_Item == null || m_Item.Deleted || m_Item.m_Wearer == null)
				{
					Stop();
					return;
				}

				m_Item.CheckForDamage();
			}
		}

		public Artifact_WebOfDominion( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 1 ); // version

			writer.Write( m_NextSummonTime );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			ArtifactLevel = 2;
			int version = reader.ReadInt();

			if (version >= 1)
			{
				m_NextSummonTime = reader.ReadDateTime();
			}

			if (Parent is Mobile)
			{
				m_Wearer = (Mobile)Parent;
				m_LastHits = m_Wearer.Hits;
				m_Timer = new InternalTimer(this);
				m_Timer.Start();
			}
		}
	}
}