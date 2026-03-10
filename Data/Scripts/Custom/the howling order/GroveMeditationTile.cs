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
			"A besta encara sua alma e você encontra parentesco com ela.",
			"Um uivo fraco ecoa em sua mente enquanto o espírito do lobo reconhece sua presença.",
			"O lobo espectral circula você uma vez, e você sente uma conexão primal se agitando.",
			"Sua batida cardíaca sincroniza com a respiração do lobo, ainda que por um momento.",
			"Os olhos do espírito encontram os seus, e você pressente o início da compreensão."
		};

		private static readonly string[] m_TwoStar = new string[]
		{
			"O lobo lambe a pata avidamente, encarando você e rosnando, e você sente sua fome.",
			"O espírito corre em sua direção brincalhão, e você entende a alegria da caçada.",
			"Você sente as memórias do lobo de corridas noturnas através de florestas antigas inundarem sua mente.",
			"O rabo da criatura abana ao reconhecer uma alma afim, e você compartilha sua empolgação.",
			"Um rosnado reverbera em seu peito, não ameaçador, mas acolhedor, enquanto predador aceita predador."
		};

		private static readonly string[] m_ThreeStar = new string[]
		{
			"O espírito acena com a cabeça em compreensão, e você sente como se ambos fossem um.",
			"Sua consciência se funde com a do lobo por um instante, e vocês correm juntos por planícies etéreas.",
			"A fronteira entre humano e besta se dissolve, e sabedoria antiga flui entre vocês livremente.",
			"O espírito do lobo inclina a cabeça para você como igual, e você conhece os segredos da natureza.",
			"O tempo para enquanto suas almas se entrelaçam, caçador e caçado, predador e alcateia, unificados em propósito."
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
				from.SendMessage(string.Format("Você deve aguardar {0} antes de se comunicar com os espíritos novamente.", FormatTimeSpan(remaining)));
				return;
			}
			
			from.SendMessage("Você medita sobre a natureza do lobo e do homem.");
			
			int successes = CountSuccesses(from);
			
			if (successes > 0)
			{
				if (from.Skills[SkillName.Druidism].Value > 75.0 && from.Skills[SkillName.Spiritualism].Value > 75.0)
				{
					HeartOfTheWilds existingHeart = from.FindItemOnLayer(Layer.Neck) as HeartOfTheWilds;
    		        if (existingHeart == null)
					{
	    				from.SendMessage("O bosque responde à sua presença, e você encontra novo poder dentro de si");
						from.SendMessage("Você adquiriu o Coração das Selvas");
						HeartOfTheWilds heart = new HeartOfTheWilds(from);
	                    from.Backpack.DropItem(heart);
					}
				}
				int totalMarks = CalculateMarks(successes);
				Server.Custom.DefenderOfTheRealm.MarkLootHelper.AwardMarks(from, 3, totalMarks);
				
				string communionMessage = GetCommunionMessage(successes);
				from.PublicOverheadMessage(Network.MessageType.Regular, 0x44, false, communionMessage);
				
				SpawnSpectralWolf(from);
			}
			else
			{
				from.SendMessage("Você falha em se comunicar com os espíritos do bosque.");
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
				return string.Format("{0:0} hora{1} e {2:0} minuto{3}", 
					ts.Hours, ts.Hours != 1 ? "s" : "", 
					ts.Minutes, ts.Minutes != 1 ? "s" : "");
			else if (ts.TotalMinutes >= 1)
				return string.Format("{0:0} minuto{1}", ts.Minutes, ts.Minutes != 1 ? "s" : "");
			else
				return string.Format("{0:0} segundo{1}", ts.Seconds, ts.Seconds != 1 ? "s" : "");
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