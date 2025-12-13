using System;
using Server;
using Server.Mobiles;
using System.Collections;
using System.Collections.Generic;
using Server.Misc;
using Server.Network;

namespace Server.Custom.Grove
{
	public class SpectralWolfTotem : BaseCreature
	{
		private Timer m_AutoDeleteTimer;
		private Timer m_HowlTimer;
		
		public SpectralWolfTotem()
			: base(AIType.AI_Animal, FightMode.None, 0, 1, 0.2, 0.4)
		{
			Name = "a spectral wolf";
			Body = 967;
			Hue = 667;
			BaseSoundID = 0xE5;
			Blessed = true;
			CantWalk = true;
			
			m_HowlTimer = Timer.DelayCall(TimeSpan.FromSeconds(2.5), new TimerCallback(DoHowl));
			m_AutoDeleteTimer = Timer.DelayCall(TimeSpan.FromSeconds(10.0), new TimerCallback(DoDelete));
		}
		
		private void DoDelete()
		{
			if (!Deleted)
			{
				Delete();
			}
		}
		
		public override void OnDelete()
		{
			if (m_HowlTimer != null)
			{
				m_HowlTimer.Stop();
				m_HowlTimer = null;
			}
			
			if (m_AutoDeleteTimer != null)
			{
				m_AutoDeleteTimer.Stop();
				m_AutoDeleteTimer = null;
			}
			
			base.OnDelete();
		}
		
		private void DoHowl()
		{
			if (Deleted)
				return;
			
			PublicOverheadMessage(MessageType.Regular, 0x21, false, "Awoo!");
			PlaySound(BaseSoundID);
			
			IPooledEnumerable eable = GetClientsInRange(15);
			eable.Free();
		}
		
		public SpectralWolfTotem(Serial serial) : base(serial) { }
		
		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write(0);
		}
		
		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			reader.ReadInt();
			
			Timer.DelayCall(TimeSpan.Zero, new TimerCallback(Delete));
		}
	}
}