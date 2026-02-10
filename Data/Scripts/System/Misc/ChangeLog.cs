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

        + "Lolth's Gift - X of Y of (hopefully) 2026<BR>"
        +" <BR>"
        +" * The following new bosses have been added to the game:<BR>"
		+"     • Tier 1 bosses (meant to be soloable by a competent adventurer):<BR>"
        +" 		   • The mother superior waits ready to defend her convent from those that would dare to defile it.<BR>"
		+"         • Blacktooth the trollbear now raids caravans across the southern forests of Sosaria.<BR>"
		+"		   • The vicious Butcher has been called upon by the mad mages of Ravendark.<BR>"
        +"     • Tier 2 bosses (meant to be faced by a very powerful adventurer, or a group of competent ones):<BR>"
		+" 		   • Firefang the Warchief is setting farms ablaze near the city of Moon.<BR>"			
        +"         • The Spore mother took some of its fellow underdark denizens attracted by powerful psychic energies and set up a base on the Myconid cave.<BR>"
		+"		   • Fiorin the Archdruid tends to his pack in the howling grove.<BR>"
		+ "        • Black Phillip now gathers his covenant at a remote location in the Dread Island.<BR>"
		+"     • Tier 3 bosses (meant to be faced by a group of skilled adventures): <BR>"
		+"         • Caelan the Dread Knight broods in his keep amongst his brethren <BR>"
		+"         • Hrimah, the fist of the north has claimed the giant's crown and now rules over the Glacial Scar (with a new giant model!).<BR>"
		+"         • The Daughter of Fire now has a permanent residence in the fires of hell.<BR>"
		+"         • The Skeleton King has awoken from its long slumber in the depths of the ancient pyramid.<BR> "
		+"     • Tier 4 bosses (meant to be faced by powerful groups of very competent adventurers): <BR>"
		+"          • Bal Tsareth's ghost returned from death to expel the invaders from her sanctum!<BR>"
		+"          • The Dreamweaver now threathens all of Lodoria from the depths of its lair.<BR>"
		+"          • The Old One Eye now stalks for prey across the wastes of the Savage Empire.<BR>"
		+"          • The Fateweaver now keeps dominion of the glittering caves of Fanaedar.<BR>"
		+"          • Xyrtaxis, the Dean of the Black Arts, can be found in the arcane academy in Fanaedar.<BR>"
		+"			• Annath, the shroud of the lightless, can be found preaching the mysteries of Lolth in Fanaedar.<BR>"
		+"     • Tier 5 bosses (meant to be faced by large groups of extremely optimized adventurers): <BR>"
		+"         • The Prince of Darkness now holds court in Ravendark. RIP Ozzy.<BR>"
		+"         • The Heavenly Marshall is leading the conquest of southern Sosaria at castle griffin roost. Evildoers beware!<BR>"
		+" 		   • The Herald of Cinders now protects his brood and his treasure in Destard.<BR>"
		+"     • Each one of them has a themed (and powerful) set of items that can only be aqquired by defeating them.<BR>"
		+" <BR>"
        +" * Crafting:<BR>"
		+"	• All crafting bonuses have been rebalanced from the ground up. Skills are more varied, progression is smoother and resists are more sensible,<BR>"
		+" 	• slayers can now be obtained more reliably from high grade materials, carpenter-made armors have mage armor, and all alien crafting is gone.<BR>"
		+"  • Crafting resistance bonuses for armor have been toned down dramatically.<BR>"
		+"  • Crafted clothing now gets the hue of the fabric used to produce them.<BR>"
        +" <BR>"
		+" * Artifacts:<BR>"
		+"	• All artifact weapons now have special attacks that affect gameplay rather significantly and are somewhat rarer.<BR>" 
		+"		• Their bonuses have also been changed to take their effects into account. From now summons to AoE explosions and DoTs, there's a lot to find.<BR>"
        +"  • Skill bonuses have been made rarer and more specialized on artifacts, and many redundant artifacts have been retired.<BR>"
		+"  • Many new artifacts were added either as boss-exclusive drops, mark rewards or to the global drop pool.<BR>"
		+" <BR>"
        +" * Customization<BR>"
		+"	• The settings file has been radically debloated. Many of the customization options are no longer there, mainly because the developer is only interested in maintaining and balancing a single game.<BR>"
		+" <BR>"
        +" * NPCS:<BR>"
		+"	• Added a new npc named Oliver to the south of Britain. They are very interested in potions of Gender Change and can reward you for any that you might bring them.<BR>"
		+"	• NPCS have a lot more to say now. About 400 new things to say. Go fight them!<BR>"
		+"  • NPCS that offer training can now train skills up to 50.<BR>"
		+"  • Exodus no longer auto-deletes pets that hit him.<BR>"
		+"  • Beholder-type enemies are now much rarer and much stronger. Watch out for their eyestalks!<BR>"
		+"  • Various unique and powerful enemies now have access to interesting special attacks that will make the battles much more dynamic.<BR>"
		+"  • Rogue-type enemies are now only insterested in your gold. The random junk you have in your bags is safe.<BR>"
		+"  • Dramatically reworked how enemy spellcasting works. Most mage-type enemies now have a spell list based on a character class which gives them access to a wide variety of different new tools to make you "+
		"miserable. Druid enemies will be more prone to summoning animals, mages will zap you, clerics will smite you, bards will yell at you, and so on. There are about 100 new spells directly taken from d&d 3.5"+
		" ready to be used by your foes.<BR>"
		+"  • Most spellcasting enemies no longer have a chance to heal to full upon being defeated.<BR>"
		+" <BR>"
		+" * Progression,  archetypes & system changes:<BR>"
		+"  • A new item called Tome of Power has been added to all mark vendors. It can store an unlimited amount of Ethereal Power Scrolls and costs 2000 marks.<BR>"
		+"  • Added a new endgame weapon enchanting system to Fanaedar. Drow enemies and bosses will drop an item called Essense of Lolth's Hatred, and 20 of those can be taken to a sacrificial pit in the drow city to enhance an artifact weapon with increased damage and enchantment points.<BR>"
		+"  • Marks of The Weave:<BR>"
		+"  	• Marks of the Weave can be aqquired by members of the mages guild as they face enemy spellcasters and research dusty tomes in forgotten dungeons.<BR>"
		+"  	• Marks of the Weave can be traded for rewards with the Mage's Guildmaster by saying 'reward' near them, or using their context menu entry.<BR>"
		+"  • Marks of Devotion:<BR>"
		+"  	• Marks of Devotion can be aqquired by members of the healer's guild as they face fearsome undead and heal the sick at the House of Holy Mercy.<BR>"
		+"  	• Marks of Devotion can be traded for rewards with the Healer's Guildmaster by saying 'reward' near them, or using their context menu entry.<BR>"
		+"  • Druid archetype expansion:<BR>"
		+"  	• Highly skilled Druids now have venom immunity.<BR>"
		+"  	• Added wildshapes into the game.<BR>"
		+"    		• Shapeshifting is a new archetype for druids that require both druidism and spiritualism. Upon meditating at the Howling grove, a character can receive a new talisman item called 'Heart of the Wilds', which will immediately allow the character to shapeshift into a wolf.<BR>"
		+"    		• Multiple different forms can be unlocked by studying them, fighting them, or fighting alongside pets of those forms.<BR>"
		+"    		• A shapeshifted character cannot use non-elementalist magic and cannot wear metal armor.<BR>"
		+"    		• Different forms have different special attacks and effects.<BR>"
		+"    		• Different forms require different levels of both Spiritualism and Druidism, and occasionally a third skill (like poisoning for turning into a giant spider).<BR>"
		+"		• Adds marks of the wilds to the game.<BR>"
		+"			• Marks of the wild can be aqquired by members of the druid's guild when adventuring with their pets. These can be traded for rewards with the druid's guildmaster.<BR>"
		+" 			• They can also be aqquired from completing taming contracts from the animal appraiser if the character belongs to the druid's guild and by meditating in the Howling Grove.<BR>"
		+"  • AoE spells (elemental devastation, apocalypse, fall, chain lightning, meteor swarm) will no longer hit party members. They also had their damage scaling when hitting multiple targets corrected.<BR>"
		+"  • All guild rings now offer 30 skill points. Some guild bonuses have been changed to offer more thematic consistency.<BR>"
		+"  • Ninjitsu damage bonus now only applies to piercing weapons.<BR>"
		+"  • Karma-warping traps now set a character's karma to zero instead of completely inverting it.<BR>"
		+"  • Poisons:<BR>"
		+"    • The maximum number of poison charges a weapon can hold is now 25.<BR>"
		+"    • There's now a chance (capping at 25%) that a poison charge will not be consumed based on the user's poisoning skill with every hit.<BR>"
		+"  • A character now fully logs out after 30 seconds, down from 5 minutes.<BR>"
		+"	• Rewards from taming contracts have been rebalanced to be more fitting of the current game's design and to no longer generate very ugly pets.<BR>"
		+"  • Mark vendors now offer a 'rewards' context menu entry for players that meet their requirements.<BR>"
		+ "<BR>"
		+"* Locations:<BR>"
		+"  • A new point of interest called Bloodstone Keep has replaced one of the Orc Camps in Sosaria. Many powerful enemies await in there.<BR>"
		+"  • The dungeon of Dardin's pit had its spawnpool rebuilt and now has a few more rooms. It also has a new miniboss.<BR>"		
		+"  • Added a giant new dungeon called Fanaedar, the city of the drow. Its meant to be an extremely challenging dungeon even for veteran adventurers. It can be found in the underworld, guarded by an ungodly amount of nasty spiders. This dungeon has its own loot pool and powerful items to be found, as well as 4 group bosses.<BR>"
		+"  • Added a new dungeon called The Sunless Citadel in Sosaria. It's meant to be challenging to new adventurers fresh out of the newbie dungeons, and was built in homage to the old ttrpg module of the same name, from the d&d third edition days. It has a miniboss at the end, designed for a party of four absolute newbies or by one mildly competent adventurer.<BR>"
		+"  • Added a new dungeon called Hive of the Eye Tyrant to Lodoria. Its a very difficult dungeon filled with powerful enemies.<BR>"
		+"	• Added a new Point of interest called Castle Griffin Roost to the southern coast of Sosaria, east of Montor. It is filled with knights that challenge evil doers to come test their mettle.<BR>"
		+"	• Added a new Point of interest called the Myconid caves close to the eastern coast of Sosaria. It's a small cave that irradiates psychic energy.<BR>"
		+"  • Added a new point of interest called The howling grove close to the western coast of Sosaria. The druids that guard it commune with powerful wolf spirits.<BR>"
		+"  • Added a new Point of interest called The House of Holy Mercy. Its a hospital and a convent where the sick are treated and the hungry are fed. Helping the nuns heal their patients can be a great way of raising your healing skill.<BR>"
		+"  • Added a new Arena to Destard. It can be accessed by collecting a key from a Draconic Cultist. Enemy spawns in Destard have also been rebalanced, and Destard has been promoved from 'challenging' to 'hard'.<BR>"
		+"	• All easy dungeons now have one extra room with slightly more challenging monsters. They also had their spawn pool rebuilt to be more consize.<BR>"
		+"	• Mage apprentices can now be found in most newbie dungeons to help new characters get their spellbooks started and to teach them how to fight casters.<BR>"
		+"	• The fires of hell roughly doubled in size, has many new inhabitants and said inhabitants have a chance of dropping special flaming weapons. It also has two minibosses.<BR>"
		+"	• The ancient pyramid had a major makeover and is more pyramid shaped now. It also has many nasty new inhabitants and a miniboss.<BR>"
        +"  • The Library of Bal Tsareth (formerly known as 'Clues') had its mobpool, encounters and lore rebuilt. There's a new quest related to it, accessible by talking to the expedition leader at its entrance.<BR>"
		+" <BR>"
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
