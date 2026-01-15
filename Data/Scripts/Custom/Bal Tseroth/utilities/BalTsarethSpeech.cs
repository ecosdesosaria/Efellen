using Server;
using System.Collections;
using System;

namespace Server.Custom.BalTsareth
{
    public static class BalTsarethSpeech
    {
        private static readonly string[] Lines = new string[]
        {
            "Bal Tsareth sees thee… and he hungers!",
			"Run {0},  please run! I cannot stop this!",
			"The arcane wind rises, I feel it in my bones!",
			"The library itself screams for thy end, {0}!",
			"Thou fightest the will of an immortal mage!",
			"Gods help me… her voice will not leave my skull…",
			"My hands are not mine! They strike of their own wicked will!",
			"I see her eyes when I blink… burning… watching…",
			"Please, {0}, strike me down before I become naught but her puppet!",
			"I feel my thoughts being peeled away, one by one…",
			"Make it stop… the magic claws inside my head!",
			"I remember who I was… I need… I need to…",
			"He is inside me… whispering… laughing…",
			"My will is crumbling like old parchment…",
			"I beg thee, {0}, end this torment!",
			"I can feel myself slipping away… do not let him take what remaineth of me!",
			"My thoughts unravel like rotted scrolls… I cannot hold them!",
			"He is erasing me… word by cursed word!",
			"Please, {0}, I do not wish to be a puppet!",
			"I hear him rewriting my mind… oh Gods, I hear the quill!",
			"My memories burn as though set to flame!",
			"I am becoming her spell… nothing more!",
			"Strike me, {0}! Better death than bondage!",
			"I am trapped behind my own eyes!",
			"My body moveth, yet my soul doth scream!",
			"I feel him crawling through my thoughts like vermin!",
			"My name… I cannot remember my own name!",
			"He is carving her sigils into my mind!",
			"I beg thee, free me from this living tomb!",
			"I can feel my will turning to ash!",
			"He puppeteers my flesh! Make it stop, {0}, make it stop!",
			"My screams are trapped within my skull!",
			"Do not let me become only her shadow!",
			"I am fading… I am fading…",
			"Help me, {0}, before I am lost forever!",
			"I am but a page in Bal Tsareth’s grand tome!",
			"Thy fate is already writ, {0}!",
			"Her will is my will — thou canst not sever it!",
			"Struggle if thou must, the ending is unchanged!",
			"There is no fear here, {0}, only servitude!",
			"Bal Tsareth hath unmade my weakness!",
			"My flesh is ink, and her magic the quill!",
			"Thou art naught but a footnote in her design!",
			"I am perfected by her dominion!",
			"Rejoice, {0}, for thou shalt be recorded in her chronicles!",
			"My mind is silent, and in that silence I hear only him!",
			"I serve gladly, freed from doubt and frailty!",
			"Thy death shall adorn her library!",
			"Every breath I take is her command!",
			"I have been rewritten!",
			"The master’s will is the only truth!",
			"I welcome oblivion, if it be by her hand!",
			"Thou shalt be bound within her endless archive!",
			"Be still, and accept thy place in her story!"
        };

        public static string GetAttackLine(Mobile defender)
        {
            if (defender == null)
                return Lines[Utility.Random(Lines.Length)];

            string line = Lines[Utility.Random(Lines.Length)];

            if (line.IndexOf("{0}") >= 0)
                return String.Format(line, defender.Name);

            return line;
        }
    }
}
