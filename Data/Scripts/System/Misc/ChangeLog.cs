using System;
using Server;
using System.Collections;
using Server.Misc;
using Server.Network;
using Server.Commands;
using Server.Commands.Generic;
using Server.Mobiles;
using Server.Accounting;
using Server.Regions;
using Server.Targeting;
using System.Collections.Generic;
using Server.Items;
using Server.Spells.Fifth;
using System.IO;
using System.Xml;

namespace Server.Misc
{
    class ChangeLog
    {
		public static string Version()
		{
			return "Version: Sacrifice (7 October 2025)";
		}

		public static string Versions()
        {
			string versionTEXT = ""

       
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        + "Ashardalon - X of Y of (hopefully) 2026<br>"

        + "<br>"
        + "* New bosses:<br>"
        + "    • The following beings can now be found in Sosaria:<br>"
        + "        • The Spore mother took some of its fellow underdark denizens attracted by powerful psychic energies and set up a base on the Myconid cave.<br>"
        + "        • The Daughter of Fire now has a permanent residence in the fires of hell. Only visit with a group of veteran friends.<br>"
		+ "        • The Prince of Darkness now holds court in Ravendark. RIP Ozzy.<br>"
		+ "        • The Heavenly Marshall is leading the conquest of southern Sosaria at castle griffin roost. Evildoers beware!<br>"
        + "		   • The mother superior waits ready to defend her convent from those that would dare to defile it!<br>"
		+"		   • The Herald of Cinders now protects his brood and his treasure in Destard.<br>"
		+"     • Each one of them has a themed (and powerful) armor set that can only be aqquired by defeating them."
		+ "<br>"
        + "* Crafting:<br>"
		+ "	• All crafting bonuses have been rebalanced from the ground up. Skills are more varied, progression is smoother and resists are more sensible,<br>"
		+ " 	• slayers can now be obtained more reliably from high grade materials, carpenter-made armors have mage armor, and all alien crafting is gone.<br>"
        + "<br>"
		+ "* Artifacts:<br>"
		+"	• All artifact weapons now have special attacks that affect gameplay rather significantly and are somewhat rarer.<br>" 
		+"		• Their bonuses have also been changed to take their effects into account. From now summons to AoE explosions and DoTs, there's a lot to find.<br>"
        +"  • Skill bonuses have been made rarer and more specialized on artifacts, and many redundant artifacts have been retired.<br>"
		+ "<br>"
        +"* Customization"
		+"	• The settings file has been radically debloated. Many of the customization options are no longer there, mainly because the developer is only interested in maintaining and balancing a single game.<br>"
		+ "<br>"
        + "* NPCS:<br>"
		+"	• Added a new npc named Oliver to the south of Britain. They are very interested in potions of Gender Change and can reward you for any that you might bring them.<br>"
		+"	• NPCS have a lot more to say now. About 400 new things to say. Go fight them!<br>"
		+"  • NPCS that offer training can now train skills up to 50 instead of up to 42.<br>"
		+"  • Exodus no longer auto-wrecks any pets that hit him.<br>"
		+ "<br>"
		+ "* Tamers:<br>"
		+"	• Adds marks of the wilds to the game."
		+"	• Marks of the wild can be aqquired by members of the druid's guild when adventuring with their pets. These can be traded for rewards with the druid's guildmaster.<br>"
		+" 		• They can also be aqquired from completing taming contracts from the animal appraiser if the character belongs to the druid's guild and by meditating in the Howling Grove.<br>"
		+"	• Rewards from taming contracts have been rebalanced to be more fitting of the current game's design and to no longer generate very ugly pets.<br>"
		+ "<br>"
		+"* Locations:<br>"
		+"	• Added a new Point of interest called Castle Griffin Roost to the southern coast of Sosaria, east of Montor. It is filled with knights that challenge evil doers to come test their mettle.<br>"
		+"	• Added a new Point of interest called the Myconid caves close to the eastern coast of Sosaria. It's a small cave that irradiates psychic energy.<br>"
		+"  • Added a new point of interest called The howling grove close to the western coast of Sosaria. The druids that guard it commune with powerful wolf spirits.<br>"
		+"  • Added a new Point of interest called The House of Holy Mercy. Its a hospital and a convent where the sick are treated and the hungry are fed. Helping the nuns heal their patients can be a great way of raising your healing skill.<br>"
		+"  • Added a new Arena to Destard. It can be accessed by collecting a key from a Draconic Cultist. Enemy spawns in Destard have also been rebalanced.<BR>"
		+"	• All easy dungeons now have one extra room with more challenging monsters. They also had their spawn pool rebuilt to be more consize.<br>"
		+"	• Mages are now slightly more common in newbie dungeons to help new characters get their spellbooks started (and also to teach them humility).<br>"
		+"	• The ancient pyramid had a slight redesign on level 2 to make it less blocky. It also has some nasty new inhabitants.<br>"
        + "<br>"
        + sepLine()

			+ "";

			return versionTEXT;
		}

		public static string sepLine()
		{
			return "---------------------------------------------------------------------------------<BR><BR>";
		}
	}
}
