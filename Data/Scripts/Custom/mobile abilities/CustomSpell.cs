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

        private int CountHostilesInRange(int range)
        {
            int count = 0;
            IPooledEnumerable eable = m_Mobile.GetMobilesInRange(range);
            foreach (Mobile m in eable)
            {
                if (SpellHelpers.isValidHostileTarget(m_Mobile, m))
                    count++;
            }
            eable.Free();
            return count;
        }

        private int CountFriendliesInRange(int range)
        {
            int count = 0;
            IPooledEnumerable eable = m_Mobile.GetMobilesInRange(range);
            foreach (Mobile m in eable)
            {
                if (m != m_Mobile && !SpellHelpers.isValidHostileTarget(m_Mobile, m))
                    count++;
            }
            eable.Free();
            return count;
        }

        private Mobile GetPrimaryHostileTarget(int range)
        {
             if (m_Mobile == null || m_Mobile.Deleted || m_Mobile.Map == null)
                return null;
        
            IPooledEnumerable eable = m_Mobile.GetMobilesInRange(range);
            Mobile best = null;

            foreach (Mobile m in eable)
            {
                if (SpellHelpers.isValidHostileTarget(m_Mobile, m))
                {
                    best = m;
                    break;
                }
            }

            eable.Free();
            return best;
        }


        private double HealthPercent
        {
            get { return (double)m_Mobile.Hits / m_Mobile.HitsMax; }
        }

        private List<CustomSpell> FilterByTag(List<CustomSpell> spells, SpellTag tag)
        {
            List<CustomSpell> list = new List<CustomSpell>();
            foreach (var s in spells)
                if (s.HasTag(tag))
                    list.Add(s);
            return list;
        }

        private List<CustomSpell> FilterByTags(List<CustomSpell> spells, SpellTag tag1, SpellTag tag2)
        {
            List<CustomSpell> list = new List<CustomSpell>();
            foreach (var s in spells)
                if (s.HasTag(tag1) && s.HasTag(tag2))
                    list.Add(s);
            return list;
        }

        private CustomSpell PickRandom(List<CustomSpell> spells)
        {
            if (spells.Count == 0)
                return null;
            return spells[Utility.Random(spells.Count)];
        }



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
            if (m_Mobile == null || m_Mobile.Deleted || m_Mobile.Map == null)
                return;

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
            if (m_Mobile == null || m_Mobile.Deleted || m_Mobile.Map == null)
                return null;        

            List<CustomSpell> available = new List<CustomSpell>();
            int highestLevel = 0;       

            foreach (CustomSpell spell in m_AllSpells)
            {
                foreach (SpellType type in spell.GetTypes())
                {
                    if ((m_SpellTypes & type) == 0)
                        continue;       

                    int level = spell.GetLevel(type);
                    if (level > m_MaxLevel)
                        continue;       

                    int manaCost = level * 9;
                    if (m_Mobile.Mana < manaCost)
                        continue;       

                    available.Add(spell);
                    if (level > highestLevel)
                        highestLevel = level;
                    break;
                }
            }       

            if (available.Count == 0)
                return null;        

            int hostiles = CountHostilesInRange(6);
            int friendlies = CountFriendliesInRange(6);
            double hp = HealthPercent;
            Mobile primary = GetPrimaryHostileTarget(6);
            double enemyHp = 1.0;       

            if (primary != null && primary.HitsMax > 0)
                enemyHp = (double)primary.Hits / primary.HitsMax;       

            if (hp <= 0.40 && Utility.RandomDouble() < 0.80)
            {
                var heals = FilterByTag(available, SpellTag.Heal);
                var pick = PickRandom(heals);
                if (pick != null)
                    return pick;
            }       

            if (hostiles > 1 && Utility.RandomDouble() < 0.70)
            {
                var aoeOff = FilterByTags(available, SpellTag.AoE, SpellTag.Offensive);
                var pick = PickRandom(aoeOff);
                if (pick != null)
                    return pick;
            }

            if (hostiles > 1 && hostiles > friendlies && Utility.RandomDouble() < 0.60)
            {
                var aoeCC = FilterByTags(available, SpellTag.AoE, SpellTag.CC);
                var pick = PickRandom(aoeCC);
                if (pick != null)
                    return pick;
            }       

            if (hostiles > friendlies && Utility.RandomDouble() < 0.60)
            {
                var summons = FilterByTag(available, SpellTag.Summon);
                var pick = PickRandom(summons);
                if (pick != null)
                    return pick;
            }       

            if (enemyHp >= 0.70 && Utility.RandomDouble() < 0.60)
            {
                var cc = FilterByTags(available, SpellTag.CC, SpellTag.SingleTarget);
                var pick = PickRandom(cc);
                if (pick != null)
                    return pick;
            }       

            if (enemyHp < 0.70 && Utility.RandomDouble() < 0.50)
            {
                var dots = FilterByTag(available, SpellTag.DoT);
                var pick = PickRandom(dots);
                if (pick != null)
                    return pick;
            }       

            if (hp <= 0.50 && Utility.RandomDouble() < 0.50)
            {
                var hots = FilterByTag(available, SpellTag.HoT);
                var pick = PickRandom(hots);
                if (pick != null)
                    return pick;
            }       

            if (friendlies == 0 && Utility.RandomDouble() < 0.50)
            {
                var buffs = FilterByTag(available, SpellTag.Buff);
                var pick = PickRandom(buffs);
                if (pick != null)
                    return pick;
            }       

            if (friendlies > 2 && Utility.RandomDouble() < 0.60)
            {
                var buffs = FilterByTags(available, SpellTag.Buff, SpellTag.AoE);
                var pick = PickRandom(buffs);
                if (pick != null)
                    return pick;
            }       

            if (Utility.RandomDouble() < 0.60)
            {
                var single = FilterByTags(available, SpellTag.Offensive, SpellTag.SingleTarget);
                var pick = PickRandom(single);
                if (pick != null)
            return pick;
    }

    List<CustomSpell> highest = new List<CustomSpell>();
    foreach (var s in available)
        if (GetSpellLevelForMobile(s) == highestLevel)
            highest.Add(s);

    if (highest.Count > 0)
        return highest[Utility.Random(highest.Count)];

    return available[Utility.Random(available.Count)];
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

            if(m_Mobile.Map == null || m_Mobile == null || m_Mobile.Deleted || !m_Mobile.Alive)
                return;
        
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
            RegisterSpell(new AcidballSpell());
            RegisterSpell(new AcidFogSpell());
            RegisterSpell(new AidSpell());
            RegisterSpell(new AuraOfColdGreaterSpell());
            RegisterSpell(new AuraOfColdLesserSpell());
            RegisterSpell(new BearsEnduranceSpell());
            RegisterSpell(new BestowCurseSpell());
            RegisterSpell(new BlackfireSpell());
            RegisterSpell(new BodyOfTheSunSpell());
            RegisterSpell(new BullsStrengthMassSpell());
            RegisterSpell(new BullsStrengthSpell());
            RegisterSpell(new BurningHandsSpell());
            RegisterSpell(new CacophonicShieldSpell());
            RegisterSpell(new CallLightningSpell());
            RegisterSpell(new CallLightningStormSpell());
            RegisterSpell(new CatsGraceMassSpell());
            RegisterSpell(new ChainLightningSpell());
            RegisterSpell(new CloudOfKnivesSpell());
            RegisterSpell(new CometfallSpell());
            RegisterSpell(new ContagionSpell());
            RegisterSpell(new CureLightWoundsSpell());
            RegisterSpell(new CureModerateWoundsSpell());
            RegisterSpell(new CureSeriousWoundsSpell());
            RegisterSpell(new DarkBoltSpell());
            RegisterSpell(new DeafeningBlastSpell());
            RegisterSpell(new DirgeOfDiscordSpell ());
            RegisterSpell(new DisintegrateSpell());
            RegisterSpell(new DissonantChordSpell());
            RegisterSpell(new EnervationSpell());
            RegisterSpell(new EntangleSpell());
            RegisterSpell(new FingerOfDeathSpell());
            RegisterSpell(new FireballSpell());
            RegisterSpell(new FirestormSpell());
            RegisterSpell(new FlameStrikeSpell());
            RegisterSpell(new FlamingSphereSpell());
            RegisterSpell(new GreaterBestowCurseSpell());
            RegisterSpell(new GreaterShoutSpell());
            RegisterSpell(new GreaterVigorSpell());
            RegisterSpell(new GoodHopeSpell());
            RegisterSpell(new HarmonicChorusSpell());
            RegisterSpell(new HealSpell());
            RegisterSpell(new HoldPersonMassSpell());
            RegisterSpell(new HoldPersonSpell());
            RegisterSpell(new HorridWiltingSpell());
            RegisterSpell(new IcebergSpell());
            RegisterSpell(new IceStormSpell());
            RegisterSpell(new LesserVigorSpell());
            RegisterSpell(new LightningBoltSpell());
            RegisterSpell(new MagicMissileSpell());
            RegisterSpell(new MassBearsEnduranceSpell());
            RegisterSpell(new MassLesserVigorSpell());
            RegisterSpell(new MassValiantSpiritSpell ());
            RegisterSpell(new MassVigorSpell());
            RegisterSpell(new MeteorSwarmSpell());
            RegisterSpell(new MelfsAcidArrowSpell());
            RegisterSpell(new OrbOfAcidSpell());
            RegisterSpell(new OrbOfColdSpell());
            RegisterSpell(new OrbOfElectricitySpell());
            RegisterSpell(new OrbOfFireSpell());
            RegisterSpell(new OrbOfForceSpell());
            RegisterSpell(new OwlsWisdomMassSpell());
            RegisterSpell(new OwlsWisdomSpell());
            RegisterSpell(new PlagueSpell());
            RegisterSpell(new PolarRaySpell());
            RegisterSpell(new PowerWordFatigueSpell());
            RegisterSpell(new PowerWordFearSpell());
            RegisterSpell(new PowerWordKillSpell());
            RegisterSpell(new PrayerSpell());
            RegisterSpell(new ProtectionFromAcidSpell());
            RegisterSpell(new ProtectionFromColdSpell());
            RegisterSpell(new ProtectionFromElectricitySpell());
            RegisterSpell(new ProtectionFromFireSpell());
            RegisterSpell(new ScorchingRaySpell());
            RegisterSpell(new ShoutSpell());
            RegisterSpell(new SlayLivingSpell());
            RegisterSpell(new SleepSpell());
            RegisterSpell(new SpikeGrowthSpell());
            RegisterSpell(new SporeFieldSpell());
            RegisterSpell(new StoneskinSpell());
            RegisterSpell(new StormOfVengeanceSpell());
            RegisterSpell(new SunbeamSpell());
            RegisterSpell(new SunburstSpell());
            RegisterSpell(new ValiantSpiritSpell ());
            RegisterSpell(new WailOfDoomSpell());
            RegisterSpell(new WebSpell());
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