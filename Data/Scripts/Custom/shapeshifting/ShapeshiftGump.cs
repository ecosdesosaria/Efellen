using System;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Gumps
{
	public class ShapeshiftGump : Gump
	{
		private Mobile m_Caster;
		private HeartOfTheWilds m_Amulet;

		public ShapeshiftGump(Mobile caster, HeartOfTheWilds amulet) : base(50, 50)
		{
			m_Caster = caster;
			m_Amulet = amulet;

			int formCount = SpectralFormEntry.Entries.Length;
			int gumpHeight = 100 + (formCount * 58);

			AddPage(0);

			AddBackground(0, 0, 620, gumpHeight, 0x1453);
			AddImageTiled(10, 10, 600, 20, 0xA40);
			AddImageTiled(10, 40, 600, gumpHeight - 80, 0xA40);
			AddImageTiled(10, gumpHeight - 30, 600, 20, 0xA40);
			AddAlphaRegion(10, 10, 600, gumpHeight - 20);

			AddHtml(14, 12, 500, 20, "<CENTER><BASEFONT COLOR=#FFFFFF>Wild Shape</CENTER></BASEFONT>", false, false);

			AddButton(550, 12, 0x15E1, 0x15E5, 99999, GumpButtonType.Reply, 0);
			AddHtml(520, 12, 30, 20, "<BASEFONT COLOR=#FFFFFF>Help</BASEFONT>", false, false);

			AddButton(10, gumpHeight - 30, 0xFB1, 0xFB2, 0, GumpButtonType.Reply, 0);
			AddHtml(45, gumpHeight - 28, 550, 20, "<BASEFONT COLOR=#FFFFFF>CANCEL</BASEFONT>", false, false);

			double druidism = caster.Skills[SkillName.Druidism].Value;
			double spiritualism = caster.Skills[SkillName.Spiritualism].Value;

			int yPos = 50;

			for (int i = 0; i < formCount; i++)
			{
				SpectralFormEntry entry = SpectralFormEntry.Entries[i];
				if (amulet == null || !amulet.IsFormUnlocked(entry.Id))
				{
				    continue;
				}

			
				bool hasMainSkill = (druidism >= entry.RequiredSkill && spiritualism >= entry.RequiredSkill);
				bool hasThirdSkill = (entry.ThirdSkill == SkillName.Alchemy || caster.Skills[entry.ThirdSkill].Value >= entry.ThirdSkillRequired);
				bool hasMana = (caster.Mana >= entry.ManaCost);
				bool hasSkill = hasMainSkill && hasThirdSkill;

				string textColor = hasSkill && hasMana ? "#90EE90" : "#CD853F";

				AddBackground(20, yPos, 580, 55, 0x2422);
				AddImageTiled(25, yPos + 5, 570, 45, 0xA40);
				AddAlphaRegion(25, yPos + 5, 570, 45);

				AddHtml(35, yPos + 8, 180, 20, String.Format("<BASEFONT COLOR={0}><B>{1}</B></BASEFONT>", textColor, entry.Name), false, false);

				string specialText = GetSpecialAbilitiesText(entry, druidism, spiritualism);
				if (specialText.Length > 0)
					AddHtml(35, yPos + 28, 420, 20, String.Format("<BASEFONT COLOR={0}>{1}</BASEFONT>", textColor, specialText), false, false);

				string reqText = GetRequirementsText(entry);
				AddHtml(370, yPos + 8, 150, 20, String.Format("<BASEFONT COLOR={0}><I>{1}</I></BASEFONT>", textColor, reqText), false, false);

				// Button
				if (hasSkill && hasMana)
				{
					AddButton(530, yPos + 15, 0xFA5, 0xFA7, i + 1, GumpButtonType.Reply, 0);
				}
				else
				{
					AddImage(535, yPos + 20, 0x5689);
				}

				yPos += 58;
			}
		}

		private string GetRequirementsText(SpectralFormEntry entry)
		{
			if (entry.ThirdSkill != SkillName.Alchemy)
			{
				return String.Format("Req: {0} / Mana: {1}", entry.ThirdSkillRequired, entry.ManaCost);
			}
			else
			{
				return String.Format("Req: {0} / Mana: {1}", entry.RequiredSkill, entry.ManaCost);
			}
		}

		private string GetSpecialAbilitiesText(SpectralFormEntry entry, double druidism, double spiritualism)
		{
			System.Collections.Generic.List<string> abilities = new System.Collections.Generic.List<string>();

			if (entry.SpeedBoost)
				abilities.Add("Fast");
			
			if (entry.BleedOnHit)
				abilities.Add("Bleeding strike");
			
			if (entry.HealthRegen)
				abilities.Add("Regenerates Health");
			
			if (entry.StaminaRegen)
				abilities.Add("Regenerates Stamina");
			
			if (entry.ManaRegen)
				abilities.Add("Regenerates Mana");
			
			if (entry.LightningOnHit)
				abilities.Add("Lightning strike");
			
			if (entry.ParalyzeOnHit)
				abilities.Add("Paralyzing strike");

			if (entry.PoisonOnHit)
				abilities.Add("Poisoning strike");

			return String.Join(", ", abilities.ToArray());
		}

		public override void OnResponse(NetState sender, RelayInfo info)
		{
			int buttonID = info.ButtonID;

			if (buttonID == 0)
				return;

			// Help button
			if (buttonID == 99999)
			{
				m_Caster.CloseGump(typeof(ShapeshiftGump));
				m_Caster.SendGump(new WildShapeHelpGump(m_Caster, m_Amulet));
				return;
			}

			int entryID = buttonID - 1;

			if (entryID < 0 || entryID >= SpectralFormEntry.Entries.Length)
				return;

			SpectralFormEntry entry = SpectralFormEntry.Entries[entryID];

			if (HeartOfTheWilds.Shapeshift(m_Caster, entry, m_Amulet))
			{
				m_Caster.SendMessage("Você se transforma em {0}!", entry.Name);
			}
			else
			{
				m_Caster.SendMessage("Você falha ao transmorfar.");
			}
		}
	}

	public class WildShapeHelpGump : Gump
	{
		private Mobile m_Caster;
		private HeartOfTheWilds m_Amulet;

		public WildShapeHelpGump(Mobile caster, HeartOfTheWilds amulet) : base(50, 50)
		{
			m_Caster = caster;
			m_Amulet = amulet;

			AddPage(0);

			AddBackground(0, 0, 500, 450, 0x1453);
			AddImageTiled(10, 10, 480, 20, 0xA40);
			AddImageTiled(10, 40, 480, 370, 0xA40);
			AddImageTiled(10, 420, 480, 20, 0xA40);
			AddAlphaRegion(10, 10, 480, 430);

			AddHtml(14, 12, 480, 20, "<CENTER><BASEFONT COLOR=#FFFFFF>Wild Shape - Help</CENTER></BASEFONT>", false, false);

			string helpText = "<BASEFONT COLOR=#FFFFFF>" +
				"<CENTER><B>Forma Selvagem</B></CENTER><BR><BR>" +
				"O Coração das Selvas permite que druidas se transformem em várias formas animais. Cada forma concede bônus e habilidades únicas.<BR><BR>" +
				"<B>Mecânicas:</B><BR>" +
				"• Cada forma exige que você atenda aos requisitos de habilidade de Druidismo e Espiritualismo listados ao lado dela<BR>" +
				"• Algumas formas exigem habilidades adicionais no mesmo nível que exigem Druidismo e Espiritualismo.<BR>" +
				"• Não pode ser usado enquanto montado ou usando armadura de metal (a menos que tenha Armadura de Mago).<BR>" +
				"• Necromantes não podem usar Forma Selvagem.<BR>" +
				"• As transformações terminam ao morrer ou ao remover o amuleto.<BR>" +
				"• Enquanto na forma selvagem, você só pode lançar feitiços de elementalismo.<BR>"+
				"• As formas oferecem várias habilidades diferentes que são ampliadas com seu Druidismo e Espiritualismo<BR>" +
				"<B>Desbloqueando novas formas:</B><BR>" +
				"• Você pode aprender novas formas adquirindo Totens das selvas. Estes podem ser encontrados estudando criaturas, lutando ao lado de animais domados e abatendo criaturas que possuem uma forma selvagem<BR><BR>" +
				"• As seguintes formas podem ser aprendidas:<BR>"+
				"• Anaconda (aprendida com várias serpentes gigantes, requer envenenamento 80)<BR>"+
				"• Urso-Dire (aprendido com ursos das cavernas e ursos anciões, requer 85 de druidismo)<BR>"+
				"• Stalker (aprendido apenas com stalkers, requer 85 de furtividade e druidismo)<BR>"+
				"• Escorpião (aprendido com escorpiões e escorpiões mortais, requer envenenamento 90 e druidismo)<BR>"+
				"• Gorakong (aprendido apenas com gorilas e gorakongs)<BR>"+
				"• Worg (aprendido com lobos e worgs, requer 105 de druidismo)<BR>"+
				"• Grifo (aprendido com grifos e grifos treinados, requer 110 de druidismo)<BR>"+
				"• Estegossauro (aprendido com estegossauros) requer 115 de druidismo<BR>"+
				"• Aranha monstruosa (aprendida com aranhas monstruosas, requer 120 de envenenamento e druidismo)<BR>"+
				"• Tiranossauro (aprendido com tiranossauros, requer 125 de druidismo)<BR>"+
				"</BASEFONT>";

			AddHtml(20, 45, 460, 360, helpText, false, true);

			AddButton(10, 420, 0xFB1, 0xFB2, 1, GumpButtonType.Reply, 0);
			AddHtml(45, 422, 400, 20, "<BASEFONT COLOR=#FFFFFF>BACK TO WILD SHAPE</BASEFONT>", false, false);
		}

		public override void OnResponse(NetState sender, RelayInfo info)
		{
			if (info.ButtonID == 1)
			{
				m_Caster.CloseGump(typeof(WildShapeHelpGump));
				m_Caster.SendGump(new ShapeshiftGump(m_Caster, m_Amulet));
			}
		}
	}
}