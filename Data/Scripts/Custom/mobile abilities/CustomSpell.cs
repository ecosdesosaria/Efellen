using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Mobiles;
using Server.Spells;

namespace Server.CustomSpells
{
    [Flags]
    public enum SpellType
    {
        None = 0,
        Wizard = 1,
        Druid = 2,
        Sorcerer = 4,
        Bard = 8,
        Cleric = 16
    }

    [Flags]
    public enum SpellTag
    {
        None         = 0,
        AoE          = 1 << 0, // area of effect
        SingleTarget = 1 << 1,
        Heal         = 1 << 2,
        CC           = 1 << 3, // crowd control
        Offensive    = 1 << 4,
        Summon       = 1 << 5,
        Buff         = 1 << 6,
        Debuff       = 1 << 7,
        DoT          = 1 << 8, // damage over time
        HoT          = 1 << 9, // heal over time
    }


    public abstract class CustomSpell
    {
        private string m_Name;
        private int m_DefaultHue;
        private Dictionary<SpellType, int> m_Levels;

        public string Name { get { return m_Name; } }
        public int DefaultHue { get { return m_DefaultHue; } }
        
        private SpellTag m_Tags;
        public SpellTag Tags { get { return m_Tags; } }
        public CustomSpell(string name, int defaultHue)
        {
            m_Name = name;
            m_DefaultHue = defaultHue;
            m_Levels = new Dictionary<SpellType, int>();
            m_Tags = SpellTag.None;
        }

        public void AddTag(SpellTag tag)
        {
            m_Tags |= tag;
        }

        public bool HasTag(SpellTag tag)
        {
            return (m_Tags & tag) != 0;
        }

        public void AddLevel(SpellType type, int level)
        {
            m_Levels[type] = level;
        }

        public int GetLevel(SpellType type)
        {
            if (m_Levels.ContainsKey(type))
                return m_Levels[type];
            return 0;
        }

        public bool HasType(SpellType type)
        {
            return m_Levels.ContainsKey(type);
        }

        public List<SpellType> GetTypes()
        {
            List<SpellType> types = new List<SpellType>();
            foreach (KeyValuePair<SpellType, int> kvp in m_Levels)
            {
                types.Add(kvp.Key);
            }
            return types;
        }

        public abstract void Cast(Mobile caster, int hue, int level);
    }

    public class MobileMagic
    {
        private Mobile m_Mobile;
        private int m_MaxLevel;
        private SpellType m_SpellTypes;
        private int m_HueModifier;
        private DateTime m_LastCastTime;
        private bool m_IsChanneling;
        private Timer m_ChannelTimer;
        private CustomSpell m_CurrentSpell;
        private int m_CurrentSpellHue;

        private static List<CustomSpell> m_AllSpells = new List<CustomSpell>();

        public bool IsChanneling { get { return m_IsChanneling; } }

        static MobileMagic()
        {
            RegisterSpells();
        }

        public MobileMagic(Mobile mobile, int maxLevel, SpellType spellTypes, int hueModifier)
        {
            m_Mobile = mobile;
            m_MaxLevel = maxLevel;
            m_SpellTypes = spellTypes;
            m_HueModifier = hueModifier;
            m_LastCastTime = DateTime.MinValue;
            m_IsChanneling = false;
        }

        private double GetCastTime()
        {
            int reduction = m_MaxLevel / 3;
            int castTime = 5 - reduction;
            if (castTime < 1)
                castTime = 1;
            return castTime;
        }

        private int GetInterruptChance()
        {
            int chance = 75 - (m_MaxLevel * 5);
            if (chance < 0)
                chance = 0;
            return chance;
        }

        public bool CanCast()
        {
            if (m_IsChanneling)
                return false;

            if (DateTime.Now < m_LastCastTime + TimeSpan.FromSeconds(30))
                return false;

            if (m_Mobile.Mana < 9)
                return false;

            return true;
        }

        public void TryCastSpell()
        {
            if (!CanCast())
                return;

            CustomSpell spell = SelectSpell();
            if (spell == null)
                return;

            int spellLevel = GetSpellLevelForMobile(spell);
            int manaCost = spellLevel * 9;

            if (m_Mobile.Mana < manaCost)
                return;

            StartChanneling(spell, manaCost);
        }

        private CustomSpell SelectSpell()
        {
            List<CustomSpell> availableSpells = new List<CustomSpell>();
            int highestLevel = 0;

            foreach (CustomSpell spell in m_AllSpells)
            {
                foreach (SpellType type in spell.GetTypes())
                {
                    if ((m_SpellTypes & type) != 0)
                    {
                        int level = spell.GetLevel(type);
                        if (level <= m_MaxLevel)
                        {
                            int manaCost = level * 9;
                            if (m_Mobile.Mana >= manaCost)
                            {
                                availableSpells.Add(spell);
                                if (level > highestLevel)
                                    highestLevel = level;
                                break;
                            }
                        }
                    }
                }
            }

            if (availableSpells.Count == 0)
                return null;

            if (Utility.RandomDouble() < 0.70)
            {
                List<CustomSpell> highestSpells = new List<CustomSpell>();
                foreach (CustomSpell spell in availableSpells)
                {
                    if (GetSpellLevelForMobile(spell) == highestLevel)
                        highestSpells.Add(spell);
                }
                
                if (highestSpells.Count > 0)
                    return highestSpells[Utility.Random(highestSpells.Count)];
            }

            return availableSpells[Utility.Random(availableSpells.Count)];
        }

        private int GetSpellLevelForMobile(CustomSpell spell)
        {
            int maxLevel = 0;
            foreach (SpellType type in spell.GetTypes())
            {
                if ((m_SpellTypes & type) != 0)
                {
                    int level = spell.GetLevel(type);
                    if (level > maxLevel && level <= m_MaxLevel)
                        maxLevel = level;
                }
            }
            return maxLevel;
        }

        private void StartChanneling(CustomSpell spell, int manaCost)
        {
            m_IsChanneling = true;
            m_CurrentSpell = spell;
            m_CurrentSpellHue = m_HueModifier != 0 ? m_HueModifier : spell.DefaultHue;
            
            m_Mobile.Frozen = true;
            m_Mobile.PublicOverheadMessage(Server.Network.MessageType.Emote, 0x3B2, false, "*starts channeling a spell*");

            double castTime = GetCastTime();
            m_ChannelTimer = Timer.DelayCall(TimeSpan.FromSeconds(castTime), 
                new TimerStateCallback(FinishCasting), manaCost);
        }

        public void OnDamaged()
        {
            if (!m_IsChanneling)
                return;

            int interruptChance = GetInterruptChance();
            if (Utility.Random(100) < interruptChance)
            {
                InterruptSpell();
            }
        }

        private void InterruptSpell()
        {
            if (m_ChannelTimer != null)
            {
                m_ChannelTimer.Stop();
                m_ChannelTimer = null;
            }

            m_IsChanneling = false;
            m_Mobile.Frozen = false;
            m_Mobile.PublicOverheadMessage(Server.Network.MessageType.Emote, 0x3B2, false, "*the spell fizzles*");
            m_LastCastTime = DateTime.UtcNow + TimeSpan.FromSeconds(10);
            m_CurrentSpell = null;
        }

        private void FinishCasting(object state)
        {
            int manaCost = (int)state;
            
            m_IsChanneling = false;
            m_Mobile.Frozen = false;
            m_Mobile.Mana -= manaCost;
            
            if (m_CurrentSpell != null)
            {
                string message = string.Format("*casts {0}*", m_CurrentSpell.Name);
                m_Mobile.PublicOverheadMessage(Server.Network.MessageType.Emote, 0x3B2, false, message);
                
                int spellLevel = GetSpellLevelForMobile(m_CurrentSpell);
                m_CurrentSpell.Cast(m_Mobile, m_CurrentSpellHue, spellLevel);
            }

            m_LastCastTime = DateTime.Now;
            m_CurrentSpell = null;
            m_ChannelTimer = null;
        }

        private static void RegisterSpells()
        {
            RegisterSpell( new MagicMissileSpell());
            RegisterSpell( new CureLightWoundsSpell());
            RegisterSpell( new CureModerateWoundsSpell());
            RegisterSpell( new CureSeriousWoundsSpell());
            RegisterSpell( new EntangleSpell());
            RegisterSpell( new ScorchingRaySpell());
            RegisterSpell( new HoldPersonSpell());
            RegisterSpell( new FireballSpell());
            RegisterSpell( new FlameStrikeSpell());
            RegisterSpell( new ChainLightningSpell());
            RegisterSpell( new HealSpell());
            RegisterSpell( new MeteorSwarmSpell());
            RegisterSpell( new WebSpell());
            RegisterSpell( new AcidballSpell());
            RegisterSpell( new SleepSpell());
            RegisterSpell( new BurningHandsSpell());
            RegisterSpell( new CallLightningSpell());
            RegisterSpell( new CallLightningStormSpell());
            RegisterSpell( new CauseFearSpell());
            RegisterSpell( new ShoutSpell());
            RegisterSpell( new GreaterShoutSpell());
            RegisterSpell( new SlayLivingSpell());
            RegisterSpell( new OrbOfAcidSpell());
            RegisterSpell( new OrbOfColdSpell());
            RegisterSpell( new OrbOfFireSpell());
            RegisterSpell( new OrbOfForceSpell());
            RegisterSpell( new OrbOfElectricitySpell());
            RegisterSpell( new DeafeningBlastSpell());
            RegisterSpell( new DisintegrateSpell());
            RegisterSpell( new PowerWordFearSpell());
            RegisterSpell( new PowerWordFatigueSpell());
            RegisterSpell( new FingerOfDeathSpell());
            RegisterSpell( new AcidFogSpell());
            RegisterSpell( new AidSpell());
            RegisterSpell( new MelfsAcidArrowSpell());
            RegisterSpell( new PowerWordKillSpell());
            RegisterSpell( new PrayerSpell());
            RegisterSpell( new IcebergSpell());
            RegisterSpell( new PlagueSpell());
            RegisterSpell( new EnervationSpell());
            RegisterSpell( new HorridWiltingSpell());
            RegisterSpell( new SunburstSpell());
            RegisterSpell( new SunbeamSpell());
            RegisterSpell( new StormOfVengeanceSpell());
            RegisterSpell( new StoneskinSpell());
            RegisterSpell( new SpikeGrowthSpell());
            RegisterSpell( new PolarRaySpell());
            RegisterSpell( new BlackfireSpell());
            RegisterSpell( new HoldPersonMassSpell());
            RegisterSpell( new BullsStrengthMassSpell());
            RegisterSpell( new CatsGraceMassSpell());
            RegisterSpell( new BullsStrengthSpell());
            RegisterSpell(new SporeFieldSpell());
            RegisterSpell(new LesserVigorSpell());
            RegisterSpell(new VigorSpell());
            RegisterSpell(new GreaterVigorSpell());
            RegisterSpell(new MassLesserVigorSpell());
            RegisterSpell(new MassVigorSpell());
            RegisterSpell(new LightningBoltSpell ());
            RegisterSpell(new ContagionSpell());
            RegisterSpell(new CloudOfKnivesSpell());
            RegisterSpell(new DarkBoltSpell());
            RegisterSpell(new BodyOfTheSunSpell());
            RegisterSpell(new BearsEnduranceSpell());
            RegisterSpell(new MassBearsEnduranceSpell());
            RegisterSpell(new BestowCurseSpell());
            RegisterSpell(new FlamingSphereSpell());

            RegisterSummonNatureAllySpells();

        }

        public static void RegisterSpell(CustomSpell spell)
        {
            m_AllSpells.Add(spell);
        }

        private static void RegisterSummonNatureAllySpells()
        {
            RegisterSpell(new SummonNatureAllyISpell());
            RegisterSpell(new SummonNatureAllyIISpell());
            RegisterSpell(new SummonNatureAllyIIISpell());
            RegisterSpell(new SummonNatureAllyIVSpell());
            RegisterSpell(new SummonNatureAllyVSpell());
            RegisterSpell(new SummonNatureAllyVISpell());
            RegisterSpell(new SummonNatureAllyVIISpell());
            RegisterSpell(new SummonNatureAllyVIIISpell());
            RegisterSpell(new SummonNatureAllyIXSpell());
        }       
    }

    public static class MobileExtensions
    {
        private static Dictionary<Mobile, MobileMagic> m_MobileMagics = new Dictionary<Mobile, MobileMagic>();

        public static void MobileMagics(this Mobile mobile, int level, SpellType spellTypes, int hue)
        {
            MobileMagic magic = new MobileMagic(mobile, level, spellTypes, hue);
            if (!m_MobileMagics.ContainsKey(mobile))
                m_MobileMagics[mobile] = magic;
        }

        public static MobileMagic GetMobileMagic(this Mobile mobile)
        {
            if (m_MobileMagics.ContainsKey(mobile))
                return m_MobileMagics[mobile];
            return null;
        }

        public static void RemoveMobileMagic(this Mobile mobile)
        {
            if (m_MobileMagics.ContainsKey(mobile))
                m_MobileMagics.Remove(mobile);
        }
    }
}