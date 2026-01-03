using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Regions;
using Server.Misc;

namespace Server.Scripts.Custom
{
    public enum CitizenClass
    {
        Wizard = 1,
        Fighter = 2,
        Rogue = 3
    }

    [CorpseName("an adventurer corpse")]
    public class AdventurerTeam : BaseCreature
    {
        #region Dialogue Data (Static / Flavor Text)
        private static readonly string[] FriendlyChat = new string[]
		{
			"The wyrms in the deep caves grow bolder each day...",
			"I barely escaped from a pack of dire wolves yesterday.",
			"These lands are cursed, I tell you. Evil stirs in the shadows.",
			"Lets explore that dungeon, they said. It will be fun, they said.",
			"I saw a dragon's shadow pass overhead last night.",
			"Careful - traps abound in these ancient ruins.",
			"The trollbears have been raiding caravans again.",
			"They say a forgotten tomb lies somewhere around here.",
			"I seek a legendary blade, lost to time.",
			"Ancient treasures await those brave enough to claim them.",
			"A merchant spoke of ruins filled with gold and jewels nearby.",
			"The old wizard's tower supposedly holds great power.",
			"I heard whispers of a hidden vault in the mountains.",
			"My companions fell to an ambush three days past.",
			"I seek fellow brave souls to delve into the darkness.",
			"Traveling alone in these parts is a death sentence.",
			"Lost my entire party to a demon in the lower depths.",
			"We could use another sword arm for what lies ahead.",
			"Running low on supplies... need to restock soon.",
			"Any healers nearby? My wounds still ache.",
			"I'd pay good coin for quality healing potions.",
			"These old bandages won't hold much longer.",
			"Need better armor before venturing deeper.",
			"Red-cloaked murderers were spotted near the crossroads!",
			"They said orcs have been raiding farms in the north.",
			"Beware the dark knights, for they know no mercy.",
			"A band of reavers camps no more than a mile north of here.",
			"Stay out of the southern woods after dark.",
			"The old legends speak of power sealed in these ruins.",
			"Strange lights dance in the graveyard at midnight.",
			"I've seen things down there that defy explanation.",
			"The ancients left more than just their bones behind.",
			"Dark rituals are being performed in the lower levels.",
            "The air grows colder the deeper you go — that's never a good sign.",
            "I marked a safe path through the rubble, but it won't last forever.",
            "Something down there hunts by sound… tread lightly.",
            "We sealed a passage behind us — whatever was inside wasn't happy.",
            "We found a wall covered in runes, still glowing faintly.",
            "I found claw marks where no beast should fit.",
            "Those corridors twist back on themselves!",
            "The dead don't stay dead in here.",
            "I heard chanting echoing through the halls, but found no one.",
            "A collapsed tunnel nearly buried us alive.",
            "There's fresh blood on the stone… and it isn't mine.",
            "We lost our torchbearer to the darkness ahead.",
            "Whatever guards the inner sanctum is still alive.",
            "I swear the walls were watching us.",
            "Some doors are better left unopened.",
            "The floor gave way beneath us — watch your footing.",
            "I've never seen magic linger like that before.",
            "The silence down there is the worse of it all.",
            "We turned back when the torches began to fail.",
            "Light is your best ally in those depths."
		};

		// Evil adventurers - for murderers and dark warriors
		private static readonly string[] EvilChat = new string[]
		{
			"Your coin or your life, fool.",
			"The vultures will feast tonight...",
			"This is OUR territory. Pay the toll or bleed.",
			"The weak exist only to serve the strong.",
			"I smell fear... and gold.",
			"Five corpses before noon. Good hunting today.",
			"Their screams still echo in my ears",
			"Left a trail of bodies from here to the coast.",
			"The river runs red with their blood.",
			"I've lost count of how many I've killed this month.",
			"These ruins belong to us now. Leave or join the dead.",
			"Turn back while you still draw breath.",
			"Only the strong survive here. You don't look strong.",
			"Trespassers end up feeding the crows.",
			"This place is ours. Find your own grave to rob.",
			"Need someone killed? I know people...",
			"For the right price, anyone can disappear.",
			"We don't ask questions. We just collect heads.",
			"Gold talks. Mercy doesn't.",
			"Honor is for the dead and the foolish.",
			"In the end, only power matters.",
			"The darkness welcomes all who embrace it.",
			"Your laws have no power here!",
			"Morality is a luxury we can't afford.",
            "They will never found her body...",
            "Hey, a sack of coins just walked in...",
            "I like to see the light go out.",
            "Can't afford to be weak around these parts. Kill them.",
            "Blood sharpens my blade.",
            "Leave now, or you will be another corpse in my path!",
            "These cowards will finally meet their end!.",
            "The dungeon feeds us well.",
            "Care for a little sport? You run, we hunt.",
            "We will use your bones to mark the path."
		};

		// Combat yells - used during active battles
		private static readonly string[] CombatYell = new string[]
		{
			"Surround them!",
			"Cut off their escape!",
			"Focus on the spell-caster!",
			"Shield wall, hold formation!",
			"Flank them from the left!",
			"Watch for ambushes!",
			"Cover the rear!",
			"Break their line!",
			"Press the attack!",
			"Fall back and regroup!",
			"Healer down! Protect them!",
			"They're flanking us!",
			"Ambush! Weapons ready!",
			"Trap! Watch your step!",
			"Reinforcements incoming!",
			"We're surrounded!",
			"Hold the line!",
			"For glory!",
			"Stand and fight!",
			"No retreat!",
			"We end this now!",
			"Fight or die!",
			"To the last breath!",
			"Show no mercy!",
			"Give them steel!",
            "On me!",
            "Push forward!",
            "Drive them back!",
            "Lock shields!",
            "Hold this ground!",
            "Form up!",
            "Take them down!",
            "Bring them down!",
            "Finish them!",
            "Don't let them escape!",
            "Pin them here!",
            "Keep the pressure!",
            "Eyes open!",
            "Spread out!",
            "Close ranks!",
            "Strike now!",
            "Break them!",
            "Stand fast!",
            "Rally on me!",
            "Cut them down!",
            "Force them back!",
            "Hold steady!",
            "All at once!",
            "Crush them!",
            "End them!"
		};
        // Post-combat celebration lines
		private static readonly string[] VictoryLines = new string[]
		{
			"That was close! Everyone alright?",
			"Good fight! Check the body for coin.",
			"We make a good team!",
			"Another one bites the dust.",
			"I need to catch my breath...",
			"Did anyone get hurt badly?",
			"That beast was tougher than expected.",
			"Victory! But stay alert.",
			"Well fought, friends!",
			"*wipes blood from weapon*",
			"Excellent teamwork!",
			"They didn't stand a chance!",
            "Area clear… for now.",
            "That's the last of them.",
            "Count heads, make sure no one's missing.",
            "Catch your breath, but keep watch.",
            "Not bad — we're still alive.",
            "Clean your blades, this place isn't safe.",
            "Let's tend wounds before moving on.",
            "Good work everyone.",
            "Check corners — ambush isn't off the table.",
            "That could've gone worse.",
            "Another step deeper.",
            "Stay sharp, more ahead.",
            "Quick rest, then we move.",
            "Solid fighting.",
            "We earned this moment.",
            "Holy shit we survived!",
            "That'll keep the monsters quiet for a bit.",
            "Take what we need and go.",
            "Quiet now. Listen.",
            "Onward — carefully."

		};
        private static readonly string[] RetreatLines = new string[]
		{
			"Fall back! I'm badly wounded!",
			"I can't take much more!",
			"Retreating! Cover me!",
			"Too many of them!",
			"*stumbles backward*",
			"I need to heal!",
			"Not dying here today!",
			"Getting out of here!",
			"Tactical withdrawal!",
            "Pull back, now!",
            "Break contact!",
            "Cover the retreat!",
            "We're overmatched!",
            "Back, back!",
            "Fall back to the corner!",
            "I can't hold this!",
            "Disengage!",
            "Regroup outside!",
            "Move, move!",
            "Lets heroically get the fuck out!",
            "Hold them off while we withdraw!",
            "This fight's lost!",
            "Save yourselves!",
            "Form on me and pull back!",
            "Retreat to cover!",
            "Withdraw while we can!",
            "Don't let them chase us!",
            "Out, now!",
            "Everyone fall back!"

		};
		private static readonly string[] PotionLines = new string[]
		{
			"*drinks a healing potion*",
			"That should help!",
			"*gulps potion hastily*",
			"Much better!",
			"*uncorks flask*",
			"Good thing I brought these!",
			"*downs potion*",
			"Ah, that's better!",
            "*swallows the potion*",
            "Feeling it work already.",
            "*wipes mouth*",
            "That burns going down.",
            "Just what I needed.",
            "*shakes empty vial*",
            "Still tastes awful.",
            "That'll keep me going.",
            "*breaks the seal*",
            "Tastes like piss but gets the job done",
            "Never leave home without these.",
            "*exhales in relief*",
            "That steadied me.",
            "*stows the empty bottle*",
            "Better than bleeding out.",
            "That'll do.",
            "*grimaces as the magic takes hold*",
            "Not wasting a drop.",
            "Glad I saved this.",
            "Let's keep moving."

		};
        private static readonly string[] BandageLines = new string[]
		{
			"*applies bandages*",
			"*binds wounds*",
			"Just need to stop the bleeding...",
			"*wraps injuries*",
			"These bandages will hold.",
			"*treats wounds*",
            "*tightens the bandage*",
            "Hold still…",
            "*secures the wrap*",
            "This should slow the bleeding.",
            "*pulls bandage taut*",
            "Not pretty, but it'll do.",
            "*ties it off*",
            "That'll have to hold.",
            "Pressure helps.",
            "*checks the wound*",
            "Keep watch while I finish this.",
            "*wraps it tighter*",
            "Could be worse.",
            "*wipes blood away*",
            "That should stop it.",
            "*adjusts the dressing*",
            "Give me a moment.",
            "Still hurts, but it's better.",
            "*tests the limb*",
            "Good enough to fight."

		};
        private static readonly string[] HealSpellLines = new string[]
		{
			"*casts healing magic on ally*",
			"In Vas Mani! Be healed!",
			"*channels healing energy*",
			"Let the light mend thy wounds!",
			"*channels restorative spell*",
			"Hold still, I'll heal thee!",
            "*invokes a healing prayer*",
            "By the light, be restored!",
            "*calls upon divine grace*",
            "Let pain fade from thee!",
            "*focuses restorative magic*",
            "Your wounds close now!",
            "*lays hands upon ally*",
            "Strength return to you!",
            "*whispers a holy chant*",
            "Be renewed!",
            "*radiates healing light*",
            "The light answers!",
            "*draws sigils of renewal*",
            "Rise and fight on!",
            "*channels sacred energy*",
            "Let life flow again!",
            "*extends glowing hand*",
            "Be mended!",
            "*completes the healing rite*",
            "Stand fast — you are healed!"

		};

        private static readonly int FriendlyChatLength = FriendlyChat.Length;
        private static readonly int EvilChatLength = EvilChat.Length;
        private static readonly int CombatYellLength = CombatYell.Length;
        private static readonly int VictoryLinesLength = VictoryLines.Length;
        private static readonly int RetreatLinesLength = RetreatLines.Length;
        private static readonly int PotionLinesLength = PotionLines.Length;
        private static readonly int BandageLinesLength = BandageLines.Length;
        private static readonly int HealSpellLinesLength = HealSpellLines.Length;
        #endregion

        #region Configuration
        private static readonly TimeSpan SpeechThrottle = TimeSpan.FromSeconds(2.0);
        private const int TeamMemberRange = 12;
        private const double HealSelfThreshold = 0.40;
        private const double HealAllyThreshold = 0.50;
        private const double RetreatThreshold = 0.20;
        #endregion

        #region Instance Fields
        private int m_CitizenType;
        private int m_CitizenLevel;
        private bool m_SpawnedBySystem;
        private bool m_IsEvil;
        private bool m_SpawnMounted;
        private int m_TeamId;

        private long m_LastMessageTicks;
        private DateTime m_NextChatTime;
        private DateTime m_NextHealCheck;
        private DateTime m_PendingDeparture;
        
        private bool m_IsLeaving;
        private bool m_IsUsingBandage;
        private bool m_IsRetreating;
        private DateTime m_RetreatResetTime;

        // Skip thinking when idle
        private int m_ThinkSkipCounter;
        private const int ThinkSkipMax = 3;

		private DateTime m_NextThink;

		private class CachedCount
		{
		    public int Count;
		    public DateTime Time;

		    public CachedCount()
		    {
		        Time = DateTime.MinValue;
		    }
		}


		private static Dictionary<PlayerMobile, CachedCount> s_NearbyCountCache = new Dictionary<PlayerMobile, CachedCount>();


        private List<AdventurerTeam> m_MySquad = new List<AdventurerTeam>();
        #endregion

        #region Properties
        [CommandProperty(AccessLevel.Owner)]
        public CitizenClass CitizenClass
        {
            get { return (CitizenClass)m_CitizenType; }
            set { m_CitizenType = (int)value; InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.Owner)]
        public int CitizenLevel
        {
            get { return m_CitizenLevel; }
            set { m_CitizenLevel = value; InvalidateProperties(); }
        }

        public bool SpawnedBySystem
        {
            get { return m_SpawnedBySystem; }
            set { m_SpawnedBySystem = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool IsEvil
        {
            get { return m_IsEvil; }
            set { m_IsEvil = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int TeamId
        {
            get { return m_TeamId; }
            set { m_TeamId = value; }
        }

        public override bool CanRummageCorpses { get { return true; } }
        public override bool AlwaysMurderer { get { return m_IsEvil; } }
        #endregion

        #region Constructors
        [Constructable]
        public AdventurerTeam() : this(0, false, false) { }

        [Constructable]
        public AdventurerTeam(int teamId, bool isEvil) : this(teamId, isEvil, false) { }

        [Constructable]
        public AdventurerTeam(int teamId, bool isEvil, bool mounted) 
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            if (teamId != 0)
                this.Team = teamId;

            m_TeamId = teamId; 
            m_IsEvil = isEvil;
            m_SpawnedBySystem = (teamId != 0);
            m_SpawnMounted = mounted;

            FightMode = FightMode.Closest; 
            RangePerception = TeamMemberRange; 

            InitStatsAndAppearance();
            EnforceMountState(mounted);

            DateTime now = DateTime.UtcNow;
            m_NextChatTime = now.AddSeconds(Utility.RandomMinMax(10, 30));
            m_PendingDeparture = now.AddMinutes(Utility.RandomMinMax(20, 40));
            m_LastMessageTicks = now.Ticks;
        }

        public AdventurerTeam(Serial serial) : base(serial) { }
        #endregion

        #region Squad Management (OPTIMIZED - Reactive Cleanup)
        public void AddToSquad(AdventurerTeam member)
        {
            if (member != null && member != this && !m_MySquad.Contains(member))
                m_MySquad.Add(member);
        }

        public void RemoveFromSquad(AdventurerTeam member)
        {
            m_MySquad.Remove(member);
        }

        public override void OnDelete()
        {
            base.OnDelete();
            
            // Notify squad members 
            for (int i = 0; i < m_MySquad.Count; i++)
            {
                if (m_MySquad[i] != null && !m_MySquad[i].Deleted)
                    m_MySquad[i].RemoveFromSquad(this);
            }
            m_MySquad.Clear();
        }
        #endregion

        #region Core Logic
        public override void OnThink()
        {
			
            if (DateTime.UtcNow < m_NextThink)
		       return;
		
	    	m_NextThink = DateTime.UtcNow + TimeSpan.FromSeconds(1.5);
		
		    base.OnThink();
            
			if (Deleted || Map == null || Map == Map.Internal) return;

            // Skip thinking every 3rd cycle when idle
            if (Combatant == null && !m_IsRetreating)
            {
                if (++m_ThinkSkipCounter < ThinkSkipMax)
                    return;
                m_ThinkSkipCounter = 0;
            }

            DateTime now = DateTime.UtcNow;

            if (m_IsRetreating)
            {
                if (now > m_RetreatResetTime)
                    m_IsRetreating = false;
                else
                {
                    Combatant = null;
                    Warmode = false;
                    return;
                }
            }

            if (!m_IsLeaving && m_TeamId != 0 && now > m_PendingDeparture)
            {
                if (Combatant == null && Utility.RandomDouble() < 0.10)
                {
                    m_IsLeaving = true;
                    if (CanSendMessage(now)) Say("Time to move on."); 
                    Timer.DelayCall(TimeSpan.FromSeconds(5.0), ExecuteDeparture);
                }
            }

            if (now > m_NextHealCheck)
            {
                m_NextHealCheck = now + TimeSpan.FromSeconds(6);
                PerformCombatSupport(now);
            }

            if (Combatant == null && now > m_NextChatTime)
            {
                if (CanSendMessage(now))
                {
                    Say(m_IsEvil ? 
                        GetPooledMessage(EvilChat, EvilChatLength) : 
                        GetPooledMessage(FriendlyChat, FriendlyChatLength));
                    m_NextChatTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 45));
                }
            }
			// dictionary clean up
			if (Utility.Random(100) == 0)
			    PruneNearbyCache();
        }

        private void PerformCombatSupport(DateTime now)
        {
            double hpRatio = (double)Hits / HitsMax;
            
            if (hpRatio < HealSelfThreshold)
                TryHealSelf(now);

            if (m_CitizenType == (int)CitizenClass.Wizard && Mana > 10)
            {
                // handled by OnDelete
                for (int i = 0; i < m_MySquad.Count; i++)
                {
                    AdventurerTeam ally = m_MySquad[i];
                    
                    if (ally.Alive && ally.Map == this.Map && ally.InRange(this, 12))
                    {
                        if (ally.Hits < (ally.HitsMax * HealAllyThreshold))
                        {
                            DoMagicHeal(ally);
                            break;
                        }
                    }
                }
            }
        }
        #endregion

        #region Combat Events & Retreat
        public override void OnDamage(int amount, Mobile from, bool willKill)
        {
            base.OnDamage(amount, from, willKill);
            
            if (willKill || Deleted) return;

            if (!m_IsRetreating && m_CitizenType != (int)CitizenClass.Fighter)
            {
                double hpRatio = (double)Hits / HitsMax;
                if (hpRatio < RetreatThreshold && Utility.RandomDouble() < 0.35)
                {
                    m_IsRetreating = true;
                    m_RetreatResetTime = DateTime.UtcNow + TimeSpan.FromSeconds(6.0);
                    
                    if (CanSendMessage(DateTime.UtcNow)) 
                        Say(GetPooledMessage(RetreatLines, RetreatLinesLength));
                    
                    Combatant = null;
                    Warmode = false;
                }
            }
        }

        public override void OnCombatantChange()
        {
            base.OnCombatantChange();
            DateTime now = DateTime.UtcNow;

            if (Combatant != null)
            {
                if (CanSendMessage(now) && Utility.RandomDouble() < 0.3)
                    Say(GetPooledMessage(CombatYell, CombatYellLength));
            }
            else
            {
                if (!m_IsRetreating && CanSendMessage(now) && Utility.RandomDouble() < 0.5)
                    Say(GetPooledMessage(VictoryLines, VictoryLinesLength));
            }
        }

        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            if (m_IsLeaving || Deleted || m == null || !m.Alive || !m.Player) return;

            if (CanSendMessage(DateTime.UtcNow) && CanSee(m) && m.InRange(this, 8))
            {
                Say(m_IsEvil ? 
                    GetPooledMessage(EvilChat, EvilChatLength) : 
                    GetPooledMessage(FriendlyChat, FriendlyChatLength));
            }
        }
        #endregion

        #region Actions (Heal/Support)
        private void TryHealSelf(DateTime now)
        {
            BaseHealPotion potion = Backpack.FindItemByType(typeof(BaseHealPotion)) as BaseHealPotion;
            if (potion != null)
            {
                potion.Drink(this);
                PublicOverheadMessage(MessageType.Emote, 0x3B2, true, 
                    GetPooledMessage(PotionLines, PotionLinesLength));
                return;
            }

            if (m_CitizenType != (int)CitizenClass.Wizard && !m_IsUsingBandage)
            {
                Bandage bandage = Backpack.FindItemByType(typeof(Bandage)) as Bandage;
                if (bandage != null)
                {
                    m_IsUsingBandage = true;
                    bandage.Consume(1);
                    PublicOverheadMessage(MessageType.Emote, 0x3B2, true, 
                        GetPooledMessage(BandageLines, BandageLinesLength));
                    
                    Timer.DelayCall(TimeSpan.FromSeconds(4.0), delegate 
                    { 
                        if (!Deleted && Alive) 
                        {
                            Heal(Utility.RandomMinMax(20, 40)); 
                            PlaySound(0x57);
                            m_IsUsingBandage = false;
                        }
                    });
                }
            }
            else if (m_CitizenType == (int)CitizenClass.Wizard)
            {
                DoMagicHeal(this);
            }
        }

        private void DoMagicHeal(Mobile target)
        {
            if (Mana < 10) return;
            Mana -= 10;
            if (target != this) Direction = GetDirectionTo(target);
            
            Animate(17, 7, 1, true, false, 0);
            PlaySound(0x1F2);
            PublicOverheadMessage(MessageType.Emote, 0x3B2, true, 
                GetPooledMessage(HealSpellLines, HealSpellLinesLength));
            
            Timer.DelayCall(TimeSpan.FromSeconds(1.0), delegate
            {
                if (!Deleted && Alive && target.Alive && target.Map == Map && target.InRange(this, 12))
                {
                    target.Heal(Utility.RandomMinMax(20, 35));
                    target.FixedParticles(0x376A, 9, 32, 5030, EffectLayer.Waist);
                }
            });
        }
        #endregion

        #region Setup (Mounts & Appearance)
        private void EnforceMountState(bool shouldBeMounted)
        {
            if (shouldBeMounted)
            {
                if (this.Mount == null)
                    new Horse().Rider = this;
            }
            else
            {
                if (this.Mount != null)
                {
                    IMount mount = this.Mount;
                    mount.Rider = null;
                    if (mount is Mobile) ((Mobile)mount).Delete();
                }
            }
        }

        private void InitStatsAndAppearance()
        {
            Female = Utility.RandomBool();
            Body = Female ? 401 : 400;
            Name = Female ? NameList.RandomName("female") : NameList.RandomName("male");

            if (!Female)
                FacialHairItemID = Utility.RandomList(0, 8254, 8255, 8256, 8257, 8267, 8268, 8269);

            m_CitizenLevel = Utility.RandomMinMax(1, 9);
            Fame = 2500 * m_CitizenLevel;
            Karma = m_IsEvil ? -Fame : Fame;
            VirtualArmor = m_CitizenLevel * 10;
            
            if (m_IsEvil)
            {
                Title = TavernPatrons.GetEvilTitle();
                Hue = Utility.RandomList(0x995, 0x8A4, 0x8B0, 0x8AC);
                FightMode = FightMode.Good;
            }
            else
            {
                Title = TavernPatrons.GetTitle();
                Hue = Utility.RandomSkinHue();
                FightMode = FightMode.Evil;
            }

            Utility.AssignRandomHair(this);
            SpeechHue = Utility.RandomTalkHue();
            HairHue = FacialHairHue = Utility.RandomHairHue();

            int baseSkill = 25 + (m_CitizenLevel * 10);
            int strMin = m_CitizenLevel * 20, strMax = m_CitizenLevel * 30;
            int dexMin = m_CitizenLevel * 20, dexMax = m_CitizenLevel * 30;
            int intMin = m_CitizenLevel * 20, intMax = m_CitizenLevel * 30;
            int hitsMin = m_CitizenLevel * 30, hitsMax = m_CitizenLevel * 40;
            
            int type = Utility.Random(3);
            switch (type)
			{
				case 0: // Wizard
					IntelligentAction.DressUpWizards(this, m_IsEvil);
					m_CitizenType = (int)CitizenClass.Wizard;
					AI = AIType.AI_Mage;
					
					SetSkill(SkillName.Psychology, baseSkill);
					SetSkill(SkillName.Magery, baseSkill);
					SetSkill(SkillName.Meditation, baseSkill);
					SetSkill(SkillName.MagicResist, baseSkill);
					SetSkill(SkillName.FistFighting, baseSkill);
					SetSkill(SkillName.Tactics, baseSkill - 20);
					
					intMax += m_CitizenLevel * 30;
					break;
					
				case 1: // Fighter
					IntelligentAction.DressUpFighters(this, "", m_IsEvil, false, true);
					m_CitizenType = (int)CitizenClass.Fighter;
					AI = AIType.AI_Melee;
					
					SetSkill(SkillName.Fencing, baseSkill);
					SetSkill(SkillName.Bludgeoning, baseSkill);
					SetSkill(SkillName.Swords, baseSkill);
					SetSkill(SkillName.Parry, baseSkill);
					SetSkill(SkillName.MagicResist, baseSkill);
					SetSkill(SkillName.Tactics, baseSkill + 10);
					SetSkill(SkillName.Healing, baseSkill + 10);
					SetSkill(SkillName.Anatomy, baseSkill);
					
					strMax += m_CitizenLevel * 10;
					hitsMax += m_CitizenLevel * 20;
					break;
					
				case 2: // Rogue
					IntelligentAction.DressUpRogues(this, "", m_IsEvil, false, true);
					m_CitizenType = (int)CitizenClass.Rogue;
					AI = AIType.AI_Archer;
					
					SetSkill(SkillName.Marksmanship, baseSkill);
					SetSkill(SkillName.Tactics, baseSkill);
					SetSkill(SkillName.MagicResist, baseSkill);
					SetSkill(SkillName.Healing, baseSkill);
					SetSkill(SkillName.Anatomy, baseSkill - 10);
					
					dexMax += m_CitizenLevel * 10;
					break;
			}

            SetStr(strMin, strMax);
            SetDex(dexMin, dexMax);
            SetInt(intMin, intMax);
            SetHits(hitsMin, hitsMax);
            
            AddWeapon(true);
            AddHealingSupplies();
        }

		public void AddWeapon(bool initial)
		{
			BaseWeapon hand = FindItemOnLayer(Layer.OneHanded) as BaseWeapon;
			BaseWeapon twohand = FindItemOnLayer(Layer.TwoHanded) as BaseWeapon;

			if (!initial && (hand != null || twohand != null))
				return;

			if (m_CitizenType == (int)CitizenClass.Fighter)
			{
				if (hand != null || twohand != null)
					return;
				
				switch (Utility.Random(3))
				{
					case 0: AddItem(new Longsword()); break;
					case 1: AddItem(new BattleAxe()); break;
					case 2: AddItem(new Mace()); break;
				}
				return;
			}

			if (initial)
			{
				if (hand != null) hand.Delete();
				if (twohand != null) twohand.Delete();
			}

			if (Utility.RandomBool() && (m_CitizenType == (int)CitizenClass.Wizard || m_CitizenType == (int)CitizenClass.Rogue))
			{
				Item glove = new Item(0x13c6);
				glove.Name = "Throwing Gloves";
				AddItem(glove);
				
				Item ammo = new Item(0xF0E);
				ammo.Name = "Throwing Ammunition";
				PackItem(ammo);
				return;
			}

			if (m_CitizenType == (int)CitizenClass.Wizard)
			{
				if (Utility.RandomBool())
					AddItem(new GnarledStaff());
				else
					AddItem(new QuarterStaff());
				return;
			}

			if (m_CitizenType == (int)CitizenClass.Rogue)
			{
				switch (Utility.Random(8))
				{
					case 0: AddItem(new Bow()); PackItem(new Arrow(Utility.RandomMinMax(20, 40))); break;
					case 1: AddItem(new Crossbow()); PackItem(new Bolt(Utility.RandomMinMax(20, 40))); break;
					case 2: AddItem(new HeavyCrossbow()); PackItem(new Bolt(Utility.RandomMinMax(20, 40))); break;
					case 3: AddItem(new RepeatingCrossbow()); PackItem(new Bolt(Utility.RandomMinMax(20, 40))); break;
					case 4: AddItem(new CompositeBow()); PackItem(new Arrow(Utility.RandomMinMax(20, 40))); break;
					case 5: AddItem(new Bow()); PackItem(new Arrow(Utility.RandomMinMax(20, 40))); break;
					case 6: AddItem(new Crossbow()); PackItem(new Bolt(Utility.RandomMinMax(20, 40))); break;
					case 7: AddItem(new Crossbow()); PackItem(new Bolt(Utility.RandomMinMax(20, 40))); break;
				}
				return;
			}
		}

		private void AddHealingSupplies()
		{
			int potionCount = Utility.RandomMinMax(3, 5);
			int bandageCount = Utility.RandomMinMax(20, 40);
			
			switch (m_CitizenType)
			{
				case (int)CitizenClass.Wizard:
					if (m_CitizenLevel >= 7)
					{
						for (int i = 0; i < Math.Max(2, potionCount - 2); i++)
                        {
							PackItem(new GreaterHealPotion());
                        }
                        if(Utility.RandomBool())
                        {
                            int roll;
                            roll = Utility.Random(4);
                            switch (roll)
                            {
                                case 0:
                                   PackItem(new GreaterManaPotion());
                                    break;
                                case 1:
                                    PackItem(new ArcaneGem());
                                    break;
                                case 2:
                                    PackItem(new PotionOfWisdom());
                                    break;
                                case 3:
                                    PackItem(new ManaPotion());
                                    break;
                            }
                        }
					}
					else if (m_CitizenLevel >= 4)
					{
						for (int i = 0; i < Math.Max(2, potionCount - 2); i++)
                        {
                            PackItem(new HealPotion());
                        }
                        if(Utility.RandomBool())
                        {
                            int roll;
                            roll = Utility.Random(3);
                            switch (roll)
                            {
                                case 0:
                                   PackItem(new GreaterManaPotion());
                                    break;
                                case 1:
                                    PackItem(new ArcaneGem());
                                    break;
                                case 2:
                                    PackItem(new ManaPotion());
                                    break;
                            }
                        }
					}
					else
					{
						for (int i = 0; i < Math.Max(2, potionCount - 2); i++)
							PackItem(new LesserHealPotion());
					}
					break;
					
				case (int)CitizenClass.Fighter:
					PackItem(new Bandage(bandageCount + 10));
					
					if (m_CitizenLevel >= 7)
					{
						for (int i = 0; i < potionCount; i++)
                        {
							PackItem(new GreaterHealPotion());
                        }
                        if(Utility.RandomBool())
                        {
                            int roll;
                            roll = Utility.Random(4);
                            switch (roll)
                            {
                                case 0:
                                   PackItem(new GreaterCurePotion());
                                    break;
                                case 1:
                                    PackItem(new GreaterHealPotion());
                                    break;
                                case 2:
                                    PackItem(new PotionOfMight());
                                    break;
                                case 3:
                                    PackItem(new CurePotion());
                                    break;
                            }
                        }
					}
					else if (m_CitizenLevel >= 4)
					{
						for (int i = 0; i < potionCount; i++)
                        {
							PackItem(new HealPotion());
                        }
                        if(Utility.RandomBool())
                        {
                            int roll;
                            roll = Utility.Random(3);
                            switch (roll)
                            {
                                case 0:
                                   PackItem(new GreaterCurePotion());
                                    break;
                                case 1:
                                    PackItem(new GreaterHealPotion());
                                    break;
                                case 2:
                                    PackItem(new CurePotion());
                                    break;
                            }
                        }

					}
					else
					{
						for (int i = 0; i < potionCount; i++)
							PackItem(new LesserHealPotion());
					}
					break;
					
				case (int)CitizenClass.Rogue:
					PackItem(new Bandage(bandageCount));
					
					if (m_CitizenLevel >= 7)
					{
						for (int i = 0; i < potionCount; i++)
                        {
							PackItem(new GreaterHealPotion());
                        }
                        if(Utility.RandomBool())
                        {
                            int roll;
                            roll = Utility.Random(4);
                            switch (roll)
                            {
                                case 0:
                                   PackItem(new GreaterCurePotion());
                                    break;
                                case 1:
                                    PackItem(new MasterSkeletonsKey());
                                    break;
                                case 2:
                                    PackItem(new PotionOfDexterity());
                                    break;
                                case 3:
                                    PackItem(new CurePotion());
                                    break;
                            }
                        }

					}
					else if (m_CitizenLevel >= 4)
					{
						for (int i = 0; i < potionCount; i++)
                        {
							PackItem(new HealPotion());
                        }
                        if(Utility.RandomBool())
                        {
                            int roll;
                            roll = Utility.Random(3);
                            switch (roll)
                            {
                                case 0:
                                   PackItem(new GreaterCurePotion());
                                    break;
                                case 1:
                                    PackItem(new MasterSkeletonsKey());
                                    break;
                                case 2:
                                    PackItem(new CurePotion());
                                    break;
                            }
                        }

					}
					else
					{
						for (int i = 0; i < potionCount; i++)
							PackItem(new LesserHealPotion());
					}
					break;
			}
		}

        #endregion

        #region Helpers

		private static int ComputeNearbyCount(PlayerMobile pm)
		{
		    if (pm == null || pm.Deleted || pm.Map == null)
		        return 0;

		    int count = 0;

		    foreach (Mobile m in pm.GetMobilesInRange(8))
		    {
		        if (m != null && !m.Deleted && m is BaseCreature)
		            count++;
		    }

		    return count;
		}

		private static void PruneNearbyCache()
		{
		    List<PlayerMobile> remove = null;
		    DateTime now = DateTime.UtcNow;

		    foreach (KeyValuePair<PlayerMobile, CachedCount> kv in s_NearbyCountCache)
		    {
		        if (kv.Key == null || kv.Key.Deleted || now - kv.Value.Time > TimeSpan.FromMinutes(5))
		        {
		            if (remove == null)
		                remove = new List<PlayerMobile>();

		            remove.Add(kv.Key);
		        }
		    }

		    if (remove != null)
		        for (int i = 0; i < remove.Count; i++)
		            s_NearbyCountCache.Remove(remove[i]);
		}


        private bool CanSendMessage(DateTime now)
        {
            long nowTicks = now.Ticks;
            if ((nowTicks - m_LastMessageTicks) < SpeechThrottle.Ticks) 
                return false;
            m_LastMessageTicks = nowTicks;
            return true;
        }

        private string GetPooledMessage(string[] source, int cachedLength)
        {
            if (source == null || cachedLength == 0) return "";
            return source[Utility.Random(cachedLength)];
        }

        private void ExecuteDeparture()
        {
            if (m_TeamId != 0) AutoTeamMaintainer.RecycleTeamId(m_TeamId);
            Delete();
        }

        public override void GenerateLoot()
        {
            if (m_CitizenLevel >= 7) AddLoot(LootPack.Rich);
            else if (m_CitizenLevel >= 5) AddLoot(LootPack.Average);
            else AddLoot(LootPack.Meager);
			
            if (Utility.Random(25) == 0)
            {
                Type rareType = Loot.AdventurerRareItemTypes[Utility.Random(Loot.AdventurerRareItemTypes.Length)];
                Item rare = Activator.CreateInstance(rareType) as Item;
                if (rare != null) PackItem(rare);
            }

            if (m_CitizenType == (int)CitizenClass.Wizard)
                AddLoot(LootPack.MedScrolls, (m_CitizenLevel / 3) + 1);
        }

        public override void Serialize(GenericWriter writer) 
        { 
            base.Serialize(writer); 
            writer.Write((int)0); 
        }
        
        public override void Deserialize(GenericReader reader) 
        { 
            base.Deserialize(reader); 
            int v = reader.ReadInt(); 
            Timer.DelayCall(TimeSpan.Zero, Delete); 
        }
        #endregion
    }
    // ========================================================================
    // AUTO TEAM MAINTAINER 
    // ========================================================================
    public static class AutoTeamMaintainer
    {
        private static bool s_Enabled = true;
        private static Timer s_MaintenanceTimer;
        private static readonly Queue<int> s_RecycledIds = new Queue<int>();
        private static int s_NextTeamId = 1;
        private static readonly object s_IdLock = new object();

        //Track last spawn attempt per player
        private static readonly Dictionary<Mobile, DateTime> s_LastSpawnAttempt = new Dictionary<Mobile, DateTime>();
        private static readonly TimeSpan SpawnCooldown = TimeSpan.FromMinutes(4);
        
        // Cache nearby counts
        private static readonly Dictionary<Mobile, CachedCount> s_NearbyCountCache = new Dictionary<Mobile, CachedCount>();
		private static readonly TimeSpan CountCacheDuration = TimeSpan.FromSeconds(5);


        private class CachedCount
        {
            public int Count;
            public DateTime Expires;
			 public DateTime Time;

        }

        public static void Initialize()
        {
            if (s_MaintenanceTimer != null) s_MaintenanceTimer.Stop();
            s_MaintenanceTimer = Timer.DelayCall(TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(5), MaintainTeams);
        }

        public static int GetNewTeamId()
        {
            lock (s_IdLock) 
                return (s_RecycledIds.Count > 0) ? s_RecycledIds.Dequeue() : s_NextTeamId++;
        }

        public static void RecycleTeamId(int id)
        {
            lock (s_IdLock) 
                s_RecycledIds.Enqueue(id);
        }

        private static void MaintainTeams()
        {
            if (!s_Enabled) return;
            
            List<Mobile> eligiblePlayers = new List<Mobile>();
            DateTime now = DateTime.UtcNow;
            
            foreach (NetState state in NetState.Instances)
            {
                Mobile m = state.Mobile;
                if (m != null && m.Player && m.Alive && m.Map != null && m.Map != Map.Internal)
                {
					DateTime lastAttempt;
                    if (s_LastSpawnAttempt.TryGetValue(m, out lastAttempt))
                    {
                        if (now - lastAttempt < SpawnCooldown)
                            continue;
                    }
                    
                    eligiblePlayers.Add(m);
                }
            }
            
            List<Mobile> toRemove = new List<Mobile>();
            foreach (var kvp in s_LastSpawnAttempt)
            {
                if (kvp.Key.Deleted || kvp.Key.NetState == null)
                    toRemove.Add(kvp.Key);
            }
            foreach (var m in toRemove)
            {
                s_LastSpawnAttempt.Remove(m);
                s_NearbyCountCache.Remove(m);
            }
            
            // max 5 spawns attempts regardless of how many online players
            int maxProcessPerCycle = Math.Min(5, eligiblePlayers.Count);
            for (int i = 0; i < maxProcessPerCycle; i++)
            {
                Mobile pm = eligiblePlayers[Utility.Random(eligiblePlayers.Count)];
                s_LastSpawnAttempt[pm] = now;
                
                if (Utility.RandomDouble() < 0.1)
                    TrySpawnTeamForPlayer(pm);
            }
        }

        private static void TrySpawnTeamForPlayer(Mobile pm)
		{
		    DateTime now = DateTime.UtcNow;
		
		    CachedCount cached;
		    int nearbyCount;
		
		    if (!s_NearbyCountCache.TryGetValue(pm, out cached))
		    {
		        cached = new CachedCount();
		        s_NearbyCountCache[pm] = cached;
		        cached.Time = DateTime.MinValue;
		    }
		
		    if (now - cached.Time < CountCacheDuration)
		    {
		        nearbyCount = cached.Count;
		    }
		    else
		    {
		        nearbyCount = ComputeNearbyCount(pm);
		        cached.Count = nearbyCount;
		        cached.Time = now;
		    }
		
		    if (nearbyCount > 5)
		        return;
		
		    // reset cache after spawn
		    s_NearbyCountCache.Remove(pm);
		
		    Point3D spawnLoc = FindSpawnLocation(pm);
		    if (spawnLoc == Point3D.Zero)
		        return;
		
		    int teamId = GetNewTeamId();
		    bool isEvil = Utility.RandomBool();
		    bool mounted = Utility.RandomDouble() < 0.4;
		    int size = Utility.RandomMinMax(3, 6);
		
		    List<AdventurerTeam> newSquadMembers = new List<AdventurerTeam>();
		
		    for (int i = 0; i < size; i++)
		    {
		        AdventurerTeam npc = new AdventurerTeam(teamId, isEvil, mounted);
		        npc.MoveToWorld(spawnLoc, pm.Map);
		        newSquadMembers.Add(npc);
		    }
		
		    for (int i = 0; i < newSquadMembers.Count; i++)
		    {
		        AdventurerTeam member = newSquadMembers[i];
		        for (int j = 0; j < newSquadMembers.Count; j++)
		        {
		            if (member != newSquadMembers[j])
		                member.AddToSquad(newSquadMembers[j]);
		        }
		    }
		}


		private static int ComputeNearbyCount(Mobile pm)
		{
		    if (pm == null || pm.Deleted || pm.Map == null)
		        return 0;

		    int count = 0;

		    foreach (Mobile m in pm.GetMobilesInRange(12))
		    {
		        if (m != null && !m.Deleted && m is AdventurerTeam)
		            count++;
		    }

		    return count;
		}

        private static Point3D FindSpawnLocation(Mobile nearPlayer)
        {
            Map map = nearPlayer.Map;
            
            // Try closest distances first
            int[] distances = { 24, 28, 32, 35 };
            
            for (int d = 0; d < distances.Length; d++)
            {
                int dist = distances[d];
                
                for (int a = 0; a < 2; a++)
                {
                    double ang = Utility.RandomDouble() * Math.PI * 2;
                    int x = nearPlayer.X + (int)(Math.Cos(ang) * dist);
                    int y = nearPlayer.Y + (int)(Math.Sin(ang) * dist);
                    Point3D p = new Point3D(x, y, map.GetAverageZ(x, y));

                    if (IsInForbiddenRegion(p, map)) continue;
                    if (map.CanSpawnMobile(p)) return p;
                }
            }
            return Point3D.Zero;
        }

        private static bool IsInForbiddenRegion(Point3D loc, Map map)
        {
            Region reg = Region.Find(loc, map);
            if (reg == null) return false;

            if (reg is WantedRegion || reg is SavageRegion || reg is VillageRegion ||
                reg is UnderHouseRegion || reg is UmbraRegion || reg is TownRegion ||
                reg is StartRegion || reg is SkyHomeDwelling || reg is SafeRegion ||
                reg is ProtectedRegion || reg is PublicRegion || reg is PirateRegion ||
                reg is BardTownRegion || reg is DawnRegion || reg is DungeonHomeRegion ||
                reg is GargoyleRegion || reg is GuardedRegion || reg is HouseRegion ||
                reg is LunaRegion || reg is MazeRegion || reg is MoonCore)
            {
                return true;
            }
            return false;
        }
    }
}