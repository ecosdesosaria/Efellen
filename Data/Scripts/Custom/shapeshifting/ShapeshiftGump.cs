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
				m_Caster.SendMessage("You transform into a {0}!", entry.Name);
			}
			else
			{
				m_Caster.SendMessage("You fail to shapeshift.");
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
				"<CENTER><B>Wild Shape</B></CENTER><BR><BR>" +
				"The Heart of the Wilds allows druids to transform into various animal forms. Each form grants unique bonuses and abilities.<BR><BR>" +
				"<B>Mechanics:</B><BR>" +
				"• Every form requires you to meet the Druidism and Spiritualism skill requirements listed beside it<BR>" +
				"• Some forms require additional skills at the same level that they require Druidism and Spiritualism.<BR>" +
				"• Cannot be used while mounted or wearing metal armor (unless it has Mage Armor).<BR>" +
				"• Necromancers cannot wildshape.<BR>" +
				"• Transformations end on death or unequipping the amulet.<BR>" +
				"• While under wild shape, you can only cast elementalist spells.<BR>"+
				"• Forms offer various different abilities that scaled up with your Druidism and Spiritualism<BR>" +
				"<B>Unlocking new forms:</B><BR>" +
				"• You can learn new forms by aqquiring Totems of the wilds. These can be found by studying creatures, fighting alongside tamed pets and slaying creatures that have a wild shape form<BR><BR>" +
				"• The following forms can be learned:<BR>"+
				"• Anaconda (learned from various giant serpents, requires poisoning 80)<BR>"+
				"• Dire Bear (learned from cave bears and elder bears requires 85 druidism)<BR>"+
				"• Stalker (learned only from stalkers, requires 85 hiding and druidism)<BR>"+
				"• Scorpion (learned from scorpions and deadly scorpion, requires poisoning 90 and druidism)<BR>"+
				"• Gorakong (learned only from gorillas Gorakongs)<BR>"+
				"• Worg (learned from wolves and Worgs requires 105 druidism)<BR>"+
				"• Griffon (learned from Griffons and Trained Griffons, requires 110 druidism)<BR>"+
				"• Stegosaurus (learned from Stegosaurus) requires 115 druidism<BR>"+
				"• Monstrous spider (learned from monstrous spiders, requires 120 poisoning and druidism)<BR>"+
				"• Tyranasaur (learned from Tyranasaurus requires 125 druidism)<BR>"+
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