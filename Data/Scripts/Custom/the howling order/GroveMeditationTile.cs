using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.SkillHandlers;

namespace Server.Custom.Grove
{
	public class GroveMeditationTile : Item
	{
		private static Hashtable m_Cooldowns = new Hashtable();
		
		private static readonly string[] m_OneStar = new string[]
		{
			"The beast stares into your soul and you find kinship with it.",
			"A faint howl echoes in your mind as the wolf spirit acknowledges your presence.",
			"The spectral wolf circles you once, and you feel a primal connection stirring.",
			"Your heartbeat synchronizes with the wolf's breathing, if only for a moment.",
			"The spirit's eyes meet yours, and you sense the beginning of understanding."
		};
		
		private static readonly string[] m_TwoStar = new string[]
		{
			"The wolf eagerly licks its paw, staring at you and snarling, and you feel its hunger.",
			"The spirit bounds toward you playfully, and you understand the joy of the hunt.",
			"You feel the wolf's memories of moonlit runs through ancient forests flood your mind.",
			"The creature's tail wags as it recognizes a kindred soul, and you share in its excitement.",
			"A growl reverberates through your chest, not threatening but welcoming, as predator accepts predator."
		};
		
		private static readonly string[] m_ThreeStar = new string[]
		{
			"The spirit nods towards you in understanding, and you feel as if you both were one.",
			"Your consciousness merges with the wolf's for a heartbeat, and you run together across ethereal plains.",
			"The boundary between human and beast dissolves, and ancient wisdom flows between you freely.",
			"The wolf spirit bows its head to you as an equal, and you know the secrets of the wild.",
			"Time stands still as your souls intertwine, hunter and hunted, predator and pack, unified in purpose."
		};
		
		[Constructable]
		public GroveMeditationTile() : base(0x6519)
		{
			Name = "Altar";
			Movable = false;
			Visible = false;
		}
		
		public void TryMeditate(Mobile from)
		{
			if (from == null || !from.Alive)
				return;
			
			TimeSpan remaining;
			if (IsOnCooldown(from, out remaining))
			{
				from.SendMessage(string.Format("You must wait {0} before communing with the spirits again.", FormatTimeSpan(remaining)));
				return;
			}
			
			from.SendMessage("You meditate on the nature of wolf and man.");
			
			int successes = CountSuccesses(from);
			
			if (successes > 0)
			{
				int totalMarks = CalculateMarks(successes);
				Server.Custom.DefenderOfTheRealm.MarkLootHelper.AwardMarks(from, 3, totalMarks);
				
				string communionMessage = GetCommunionMessage(successes);
				from.PublicOverheadMessage(Network.MessageType.Regular, 0x44, false, communionMessage);
				
				SpawnSpectralWolf(from);
			}
			else
			{
				from.SendMessage("You fail to commune with the spirits of the grove.");
			}
			
			SetCooldown(from, TimeSpan.FromHours(1.0));
		}
		
		private int CountSuccesses(Mobile from)
		{
			int successes = 0;
			
			if (from.CheckSkill(SkillName.Druidism, 0, 125.0))
				successes++;
			if (from.CheckSkill(SkillName.Meditation, 0, 125.0))
				successes++;
			if (from.CheckSkill(SkillName.Spiritualism, 0, 125.0))
				successes++;
			
			return successes;
		}
		
		private int CalculateMarks(int successes)
		{
			int totalMarks = 0;
			for (int i = 0; i < successes; i++)
				totalMarks += Utility.RandomMinMax(4, 11);
			
			return totalMarks;
		}
		
		private string GetCommunionMessage(int successes)
		{
			string[] messages;
			
			if (successes >= 3)
				messages = m_ThreeStar;
			else if (successes >= 2)
				messages = m_TwoStar;
			else
				messages = m_OneStar;
			
			return messages[Utility.Random(messages.Length)];
		}
		
		private bool IsOnCooldown(Mobile from, out TimeSpan remaining)
		{
			remaining = TimeSpan.Zero;
			
			if (m_Cooldowns[from] != null)
			{
				DateTime next = (DateTime)m_Cooldowns[from];
				if (DateTime.UtcNow < next)
				{
					remaining = next - DateTime.UtcNow;
					return true;
				}
			}
			
			return false;
		}
		
		private void SetCooldown(Mobile from, TimeSpan duration)
		{
			m_Cooldowns[from] = DateTime.UtcNow + duration;
		}
		
		private string FormatTimeSpan(TimeSpan ts)
		{
			if (ts.TotalHours >= 1)
				return string.Format("{0:0} hour{1} and {2:0} minute{3}", 
					ts.Hours, ts.Hours != 1 ? "s" : "", 
					ts.Minutes, ts.Minutes != 1 ? "s" : "");
			else if (ts.TotalMinutes >= 1)
				return string.Format("{0:0} minute{1}", ts.Minutes, ts.Minutes != 1 ? "s" : "");
			else
				return string.Format("{0:0} second{1}", ts.Seconds, ts.Seconds != 1 ? "s" : "");
		}
		
		private void SpawnSpectralWolf(Mobile from)
		{
			Map map = Map;
			if (map == null)
				return;
			
			Point3D loc = new Point3D(4097, 3775, 24);
			
			foreach (Mobile m in map.GetMobilesInRange(loc, 1))
			{
				if (m is SpectralWolfTotem)
					return;
			}
			
			SpectralWolfTotem wolf = new SpectralWolfTotem();
			wolf.MoveToWorld(loc, map);
		}
		
		public GroveMeditationTile(Serial serial) : base(serial) { }
		
		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write(0); // version
		}
		
		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			reader.ReadInt(); // version
		}
	}
}