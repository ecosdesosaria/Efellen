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

        + "Lolth's Gift - X of Y of (hopefully) 2026<br>"
        +" <br>"
        +" * The following new bosses have been added to the game:<br>"
		+"     • Tier 1 bosses (meant to be soloable by a competent adventurer):<br>"
        +" 		   • The mother superior waits ready to defend her convent from those that would dare to defile it.<br>"
		+"         • Blacktooth the trollbear now raids caravans across the southern forests of Sosaria.<br>"
		+"		   • The vicious Butcher has been called upon by the mad mages of Ravendark.<br>"
        +"     • Tier 2 bosses (meant to be faced by a very powerful adventurer, or a group of competent ones):<br>"
		+" 		   • Firefang the Warchief is setting farms ablaze near the city of Moon.<br>"			
        +"         • The Spore mother took some of its fellow underdark denizens attracted by powerful psychic energies and set up a base on the Myconid cave.<br>"
		+"		   • Fiorin the Archdruid tends to his pack in the howling grove.<br>"
		+ "        • Black Phillip now gathers his covenant at a remote location in the Dread Island.<br>"
		+"     • Tier 3 bosses (meant to be faced by a group of skilled adventures): <br>"
		+"         • Hrimah, the fist of the north has claimed the giant's crown and now rules over the Glacial Scar (with a new giant model!).<br>"
		+"         • The Daughter of Fire now has a permanent residence in the fires of hell.<br>"
		+"         • The Skeleton King has awoken from its long slumber in the depths of the ancient pyramid.<br> "
		+"     • Tier 4 bosses (meant to be faced by powerful groups of very competent adventurers): <br>"
		+"          • Bal Tsareth's ghost returned from death to expel the invaders from her sanctum!<br>"
		+"          • The Dreamweaver now threathens all of Lodoria from the depths of its lair.<br>"
		+"          • The Old One Eye now stalks for prey across the wastes of the Savage Empire.<br>"
		+"          • The Fateweaver now guards the entrance to the vile drow city of Fanaedar in the Underworld.<br>"
		+"     • Tier 5 bosses (meant to be faced by large groups of extremely optimized adventurers): <br>"
		+"         • The Prince of Darkness now holds court in Ravendark. RIP Ozzy.<br>"
		+"         • The Heavenly Marshall is leading the conquest of southern Sosaria at castle griffin roost. Evildoers beware!<br>"
		+" 		   • The Herald of Cinders now protects his brood and his treasure in Destard.<br>"
		+"     • Each one of them has a themed (and powerful) set of items that can only be aqquired by defeating them.<BR>"
		+" <br>"
        +" * Crafting:<br>"
		+"	• All crafting bonuses have been rebalanced from the ground up. Skills are more varied, progression is smoother and resists are more sensible,<br>"
		+" 	• slayers can now be obtained more reliably from high grade materials, carpenter-made armors have mage armor, and all alien crafting is gone.<br>"
		+"  • Crafting resistance bonuses for armor have been toned down dramatically.<br>"
		+"  • Crafted clothing now gets the hue of the fabric used to produce them.<br>"
        +" <br>"
		+" * Artifacts:<br>"
		+"	• All artifact weapons now have special attacks that affect gameplay rather significantly and are somewhat rarer.<br>" 
		+"		• Their bonuses have also been changed to take their effects into account. From now summons to AoE explosions and DoTs, there's a lot to find.<br>"
        +"  • Skill bonuses have been made rarer and more specialized on artifacts, and many redundant artifacts have been retired.<br>"
		+" <br>"
        +" * Customization<br>"
		+"	• The settings file has been radically debloated. Many of the customization options are no longer there, mainly because the developer is only interested in maintaining and balancing a single game.<br>"
		+" <br>"
        +" * NPCS:<br>"
		+"	• Added a new npc named Oliver to the south of Britain. They are very interested in potions of Gender Change and can reward you for any that you might bring them.<br>"
		+"	• NPCS have a lot more to say now. About 400 new things to say. Go fight them!<br>"
		+"  • NPCS that offer training can now train skills up to 50.<br>"
		+"  • Exodus no longer auto-deletes pets that hit him.<br>"
		+"  • Beholder-type enemies are now much rarer and much stronger. Watch out for their eyestalks!"
		+"  • Various unique and powerful enemies now have access to interesting special attacks that will make the battles much more dynamic.<br>"
		+"  • Rogue-type enemies are now only insterested in your gold. The random junk you have in your bags is safe.<br>"
		+"  • Dramatically reworked how enemy spellcasting works. Most mage-type enemies now have a spell list based on a character class which gives them access to a wide variety of different new tools to make you "+
		"miserable. Druid enemies will be more prone to summoning animals, mages will zap you, clerics will smite you, bards will yell at you, and so on. There are about 100 new spells directly taken from d&d 3.5"+
		" ready to be used by your foes.<br>"
		+"  • Most spellcasting enemies no longer have a chance to heal to full upon being defeated.<br>"
		+" <br>"
		+" * Progression,  archetypes & system changes:<br>"
		+"  • Marks of Devotion:<BR>"
		+"  	• Marks of Devotion can be aqquired by members of the healer's guild as they face fearsome undead and heal the sick at the House of Holy Mercy.<BR>"
		+"  	• Marks of Devotion can be traded for rewards with the Healer's Guildmaster by saying 'reward' near them.<BR>"
		+"  • Druid archetype expansion:"
		+"  	• Highly skilled Druids now have venom immunity.<BR> "
		+"  	• Added wildshapes into the game.<br>"
		+"    		• Shapeshifting is a new archetype for druids that require both druidism and spiritualism. Upon meditating at the Howling grove, a character can receive a new talisman item called 'Heart of the Wilds', which will immediately allow the character to shapeshift into a wolf.<BR>"
		+"    		• Multiple different forms can be unlocked by studying them, fighting them, or fighting alongside pets of those forms.<BR>"
		+"    		• A shapeshifted character cannot use non-elementalist magic and cannot wear metal armor.<BR>"
		+"    		• Different forms have different special attacks and effects.<BR>"
		+"    		• Different forms require different levels of both Spiritualism and Druidism, and occasionally a third skill (like poisoning for turning into a giant spider).<BR>"
		+"		• Adds marks of the wilds to the game.<BR>"
		+"			• Marks of the wild can be aqquired by members of the druid's guild when adventuring with their pets. These can be traded for rewards with the druid's guildmaster.<br>"
		+" 			• They can also be aqquired from completing taming contracts from the animal appraiser if the character belongs to the druid's guild and by meditating in the Howling Grove.<br>"
		+"  • AoE spells (elemental devastation, apocalypse, fall, chain lightning, meteor swarm) will no longer hit party members. They also had their damage scaling when hitting multiple targets corrected.<BR>"
		+"  • All guild rings now offer 30 skill points. Some guild bonuses have been changed to offer more thematic consistency.<BR>"
		+"  • Poison:<BR>"
		+"    • The maximum number of poison charges a weapon can hold is now 25.<BR>"
		+"    • There's now a chance (capping at 25%) that a poison charge will not be consumed based on the user's poisoning skill with every hit.<BR>"
		+"  • A character now fully logs out after 30 seconds, down from 5 minutes.<BR>"
		+"	• Rewards from taming contracts have been rebalanced to be more fitting of the current game's design and to no longer generate very ugly pets.<br>"
		+ "<br>"
		+"* Locations:<br>"
		+"  • Added a new dungeon called The Sunless Citadel in Sosaria. It's meant to be challenging to new adventurers fresh out of the newbie dungeons, and was built in homage to the old ttrpg module of the same name, from the d&d third edition days. It has a miniboss at the end, designed for a party of four absolute newbies or by one mildly competent adventurer.<br>"
		+"  • Added a new dungeon called Hive of the Eye Tyrant to Lodoria. Its a very difficult dungeon filled with powerful enemies.<BR>"
		+"	• Added a new Point of interest called Castle Griffin Roost to the southern coast of Sosaria, east of Montor. It is filled with knights that challenge evil doers to come test their mettle.<br>"
		+"	• Added a new Point of interest called the Myconid caves close to the eastern coast of Sosaria. It's a small cave that irradiates psychic energy.<br>"
		+"  • Added a new point of interest called The howling grove close to the western coast of Sosaria. The druids that guard it commune with powerful wolf spirits.<br>"
		+"  • Added a new Point of interest called The House of Holy Mercy. Its a hospital and a convent where the sick are treated and the hungry are fed. Helping the nuns heal their patients can be a great way of raising your healing skill.<br>"
		+"  • Added a new Arena to Destard. It can be accessed by collecting a key from a Draconic Cultist. Enemy spawns in Destard have also been rebalanced, and Destard has been promoved from 'challenging' to 'hard'.<BR>"
		+"	• All easy dungeons now have one extra room with slightly more challenging monsters. They also had their spawn pool rebuilt to be more consize.<br>"
		+"	• Mage apprentices can now be found in most newbie dungeons to help new characters get their spellbooks started and to teach them how to fight casters.<br>"
		+"	• The ancient pyramid had a major makeover and is more pyramid shaped now. It also has many nasty new inhabitants and encounters.<br>"
        +"  • The Library of Bal Tsareth (formerly known as 'Clues') had its mobpool, encounters and lore rebuilt. There's a new quest related to it, accessible by talking to the expedition leader at its entrance.<br>"
		+" <br>"
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
