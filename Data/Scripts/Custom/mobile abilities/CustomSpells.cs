using System;
using Server;
using Server.Mobiles;
using Server.Targeting;
using Server.Spells;
using Server.Network;
using Server.Misc;
using System.Collections.Generic;
using Server.Items;

/* 

This list shows the lowers level in which a spell becomes available

Level 1

Burngle
Magic Missile
Sleepning Hands
Cause Fear
Cure Light Wounds
Enta
Summon Nature’s Ally I
Summon Spore Field
Vigor, Lesser

Level 2

Aid
Bear’s Endurance
Body of the Sun
Bull’s Strength
Cat’s Grace
Cloud of Knives
Dark Bolt
Flaming Sphere
Hold Person
Melf’s Acid Arrow
Owl’s Wisdom
Scorching Ray
Summon Nature’s Ally II
Web

Level 3

Acidball
Aura of Cold, Lesser
Bestow Curse
Call Lightning
Contagion
Cure Moderate Wounds
Deafening Blast
Dirge of Discord
Dissonant Chord
Fireball
Good Hope
Harmonic Chorus
Lightningbolt
Mass Vigor, Lesser
Prayer
Protection from Energy: Acid
Protection from Energy: Cold
Protection from Energy: Electricity
Protection from Energy: Fire
Spike Growth
Summon Nature’s Ally III
Vigor

Level 4

Flamestrike
Ice Storm
Orb of Acid
Orb of Cold
Orb of Electricity
Orb of Fire
Orb of Force
Shout
Stoneskin
Summon Nature’s Ally IV
Valiant Spirit

Level 5

Call Lightning Storm
Cure Serious Wounds
Slay Living Spell
Summon Nature’s Ally V
Vigor, Greater
Wail of Doom

Level 6

Acid Fog
Bear Endurance, Mass
Bestow Curse, Greater
Bull Strength, Mass
Cacophonic Shield
Cat’s Grace, Mass
Chain Lightning
Cometfall
Disintegrate
Greater Shout
Heal
Mass Vigor
Owl’s Wisdom, Mass
Summon Nature’s Ally VI

Level 7

Aura of Cold, Greater
Firestorm
Finger of Death Spell
Hold Person, Mass
Plague
Sunbeam
Summon Nature’s Ally VII

Level 8

Black Fire
Enervation
Horrid Wilting
Polar Ray
Powerword: Fatigue
Powerword: Fear
Sunburst
Summon Nature’s Ally VIII
Valiant Spirit, mass

Level 9

Iceberg
Meteor Swarm
Powerword: Kill
Storm of Vengeance
Summon Nature’s Ally IX
 */
namespace Server.CustomSpells
{
    public enum EnergyProtectionType
    {
        Cold,
        Fire,
        Energy,
        Poison
    }

    public static class BardSpellHelpers
    {
        public static bool IsFriendly(Mobile caster, Mobile target)
        {
            if (target == caster)
                return true;

            BaseCreature bc = caster as BaseCreature;
            if (bc != null && bc.Controlled && bc.ControlMaster == target)
                return true;

            return !caster.CanBeHarmful(target);
        }


        public static void ApplyStatBuff(
            Mobile m,
            int str, int dex, int intel,
            TimeSpan duration,
            string namePrefix)
        {
            m.AddStatMod(new StatMod(StatType.Str, namePrefix + "_Str", str, duration));
            m.AddStatMod(new StatMod(StatType.Dex, namePrefix + "_Dex", dex, duration));
            m.AddStatMod(new StatMod(StatType.Int, namePrefix + "_Int", intel, duration));
        }   

        public static void ApplySkillBuff(
            Mobile m,
            SkillName skill,
            double value,
            string name)
        {
            m.AddSkillMod(new DefaultSkillMod(skill, true, value));
        }
    }


    public static class NatureSpellHelper
    {
        public static void ApplyVigor(
            Mobile m,
            int healPerTick,
            int durationSeconds,
            int level)
        {
            int ticks = durationSeconds;

            for (int i = 0; i < ticks; i++)
            {
                Timer.DelayCall(TimeSpan.FromSeconds(i + 1), delegate
                {
                    if (m == null || m.Deleted || !m.Alive)
                        return;

                    m.Heal(healPerTick);

                    m.FixedParticles(0x376A, 9, 32, 5005, 0x3F, 0, EffectLayer.Waist);
                    m.PlaySound(0x202);
                });
            }
        }

        public static void SpawnSporeTile(Point3D loc, Map map, Mobile caster, int level, TimeSpan duration)
        {
            InternalSporeTile tile = new InternalSporeTile(caster, level, duration);
            tile.MoveToWorld(loc, map);

            Effects.SendLocationEffect(loc, map, 0x375A, 20, 10, 0x48, 0);
        }

        private class InternalSporeTile : Item
        {
            private Mobile m_Caster;
            private int m_Level;

            public InternalSporeTile(Mobile caster, int level, TimeSpan duration)
                : base(0x12B7) // mushroom-ish static
            {
                m_Caster = caster;
                m_Level = level;
                Movable = false;
                Hue = 0x48;

                Timer.DelayCall(duration, Delete);

                StartTick();
            }

            private void StartTick()
            {
                Timer.DelayCall(TimeSpan.Zero, TimeSpan.FromSeconds(2), Tick);
            }

            private void Tick()
            {
                if (Deleted)
                    return;

                foreach (Mobile m in GetMobilesInRange(0))
                {
                    if (!m.Alive)
                        continue;

                    m.ApplyPoison(m_Caster, Poison.Regular);

                    int loss = 3 + m_Level;
                    m.Mana = Math.Max(0, m.Mana - loss);
                    m.Stam = Math.Max(0, m.Stam - loss);

                    m.FixedParticles(0x374A, 10, 15, 5032, 0x48, 0, EffectLayer.Waist);
                }
            }

            public InternalSporeTile(Serial serial) : base(serial) { }

            public override void Serialize(GenericWriter writer)
            {
                base.Serialize(writer);
                writer.Write(0);
            }

            public override void Deserialize(GenericReader reader)
            {
                base.Deserialize(reader);
                reader.ReadInt();
            }
        }
    }

    public static class DeathSpellHelper
    {
        public static bool IsUndead(Mobile target)
        {
            SlayerEntry silver = SlayerGroup.GetEntryByName(SlayerName.Silver);
            SlayerEntry exorcism = SlayerGroup.GetEntryByName(SlayerName.Exorcism);

            if (target is BaseCreature)
            {
                if (silver != null && silver.Slays(target))
                    return true;

                if (exorcism != null && exorcism.Slays(target))
                    return true;
            }

            return false;
        }

        public static void TryKill(
            Mobile caster,
            Mobile target,
            int roll,
            int minDmg,
            int maxDmg,
            int level)
        {
            double resist = target.Skills[SkillName.MagicResist].Value;

            if (roll > resist)
            {
                target.Kill();
                target.FixedEffect(0x3709, 10, 30);
                target.PlaySound(0x211);
                target.SendMessage("Your life is ripped away!");
            }
            else
            {
                int dmg = Utility.RandomMinMax(minDmg, maxDmg) + level;
                AOS.Damage(target, caster, dmg, 100, 0, 0, 0, 0);
                target.PlaySound(0x1F2);
                target.SendMessage("You resist death, but suffer terribly!");
            }
        }
    }


    public static class MassBuffHelper
    {
        public static void ApplyStatBuff(
            Mobile caster,
            int range,
            int amount,
            TimeSpan duration,
            Action<Mobile, int> apply,
            Action<Mobile, int> remove,
            int effect,
            int hue,
            string message)
        {
            IPooledEnumerable eable = caster.GetMobilesInRange(range);

            foreach (Mobile m in eable)
            {
                if (!m.Alive || !caster.CanBeBeneficial(m))
                    continue;

                apply(m, amount);

                m.FixedEffect(effect, 10, 20, hue, 0);
                m.SendMessage(hue, message);

                Timer.DelayCall(duration, delegate
                {
                    if (m != null && !m.Deleted)
                        remove(m, amount);
                });
            }

            eable.Free();
        }
    }

    public static class FieldSpellHelper
    {
        public static void SpawnField(
            Mobile caster,
            Point3D center,
            Map map,
            int count,
            int radius,
            Func<Item> factory)
        {
            for (int i = 0; i < count; i++)
            {
                int x = Utility.RandomMinMax(-radius, radius);
                int y = Utility.RandomMinMax(-radius, radius);

                Item item = factory();
                item.MoveToWorld(new Point3D(center.X + x, center.Y + y, center.Z), map);
            }
        }
    }

    public abstract class SummonNatureAllySpell : CustomSpell
    {
        protected Type CreatureType;
        protected int SpellLevel;

        public SummonNatureAllySpell(
            string name,
            int icon,
            int level,
            Type creatureType)
            : base(name, icon)
        {
            SpellLevel = level;
            CreatureType = creatureType;
            AddLevel(SpellType.Druid, level);
            AddTag(SpellTag.Summon);
        }

        public override void Cast(Mobile caster, int hue, int level)
        {
            SpellHelpers.SummonNatureAlly(caster, CreatureType, SpellLevel, hue);
        }
    }


    public static class SpellHelpers
    {
        public static void SummonNatureAlly(
            Mobile caster,
            Type creatureType,
            int level,
            int hue)
        {
            Map map = caster.Map;
            if (map == null)
                return;     

            Point3D spawnLoc = Point3D.Zero;        

            // Find closest empty tile
            for (int r = 1; r <= 2; r++)
            {
                for (int x = -r; x <= r; x++)
                for (int y = -r; y <= r; y++)
                {
                    Point3D p = new Point3D(caster.X + x, caster.Y + y, caster.Z);      

                    if (map.CanSpawnMobile(p))
                    {
                        spawnLoc = p;
                        goto FOUND;
                    }
                }
            }       

            return;     

        FOUND:      

            Effects.SendLocationEffect(spawnLoc, map, 0x1A5E, 16, 10, hue, 0);
            Timer.DelayCall(TimeSpan.FromSeconds(0.8), delegate
            {
                Effects.SendLocationEffect(spawnLoc, map, 0x1A5E, 16, 10, hue, 0);
            });     

            BaseCreature bc = null;     

            try
            {
                bc = (BaseCreature)Activator.CreateInstance(creatureType);
            }
            catch
            {
                return;
            }       

            bc.MoveToWorld(spawnLoc, map);
            bc.Controlled = true;
            bc.ControlMaster = caster;
            bc.ControlOrder = OrderType.Guard;
            bc.FightMode = FightMode.Closest;
            bc.Summoned = true;
            bc.SummonMaster = caster;

            bc.IsBonded = false;
            bc.CantWalk = false;

            TimeSpan duration = TimeSpan.FromSeconds(20 + level * 2);
            Timer.DelayCall(duration, delegate
            {
                if (bc != null && !bc.Deleted)
                {
                    Effects.SendLocationEffect(bc.Location, bc.Map, 0x3728, 13, 20, hue, 0);
                    bc.Delete();
                }
            });
        }

        public static void DamageHostilesAtPoint(
            Mobile caster,
            Point3D center,
            Map map,
            int range,
            Func<int> damageRoll,
            int phys, int fire, int cold, int poison, int energy,
            int effectID,
            int hue)
        {
            Effects.SendLocationEffect(center, map, effectID, 20, 10, hue, 0);

            IPooledEnumerable eable = map.GetMobilesInRange(center, range);
            foreach (Mobile m in eable)
            {
                if (m.Alive && caster.CanBeHarmful(m))
                {
                    caster.DoHarmful(m);
                    int dmg = damageRoll();
                    AOS.Damage(m, caster, dmg, phys, fire, cold, poison, energy);
                    m.PlaySound(0x64E);
                }
            }
            eable.Free();
        }

        public static void DoTimedStorm(
            Mobile caster,
            Point3D center,
            Map map,
            int radius,
            int ticks,
            int secondsBetweenTicks,
            Func<int> damageRoll,
            int phys, int fire, int cold, int poison, int energy,
            int effectID,
            int hue,
            string message)
        {
            for (int i = 0; i < ticks; i++)
            {
                Timer.DelayCall(TimeSpan.FromSeconds(i * secondsBetweenTicks), delegate
                {
                    Effects.SendLocationEffect(center, map, effectID, 30, 10, hue, 0);

                    IPooledEnumerable eable = map.GetMobilesInRange(center, radius);
                    foreach (Mobile m in eable)
                    {
                        if (m.Alive && caster.CanBeHarmful(m))
                        {
                            caster.DoHarmful(m);
                            int dmg = damageRoll();
                            AOS.Damage(m, caster, dmg, phys, fire, cold, poison, energy);
                            m.PlaySound(0x64E);
                            m.SendMessage(message);
                        }
                    }
                    eable.Free();
                });
            }
        }

        public static void ApplyRepeatedStatDrain(
            Mobile target,
            StatType stat,
            int minLoss,
            int maxLoss,
            int ticks,
            int tickDelaySeconds,
            int restoreAfterSeconds)
        {
        for (int i = 0; i < ticks; i++)
        {
            Timer.DelayCall(TimeSpan.FromSeconds(i * tickDelaySeconds), delegate
            {
                if (target == null || target.Deleted)
                    return; 

                int loss = Utility.RandomMinMax(minLoss, maxLoss);  

                if (stat == StatType.Str)
                    target.RawStr -= loss;
                else if (stat == StatType.Dex)
                    target.RawDex -= loss;
                else if (stat == StatType.Int)
                    target.RawInt -= loss;  

                Timer.DelayCall(TimeSpan.FromSeconds(restoreAfterSeconds), delegate
                {
                    if (target != null && !target.Deleted)
                    {
                        if (stat == StatType.Str)
                            target.RawStr += loss;
                        else if (stat == StatType.Dex)
                            target.RawDex += loss;
                        else if (stat == StatType.Int)
                            target.RawInt += loss;
                    }
                });
            });
        }
    }

    public static void ForEachHostileInRange(
        Mobile center,
        Mobile caster,
        int range,
        Action<Mobile> action)
    {
        IPooledEnumerable eable = center.GetMobilesInRange(range);

        foreach (Mobile m in eable)
        {
            if (m.Alive && caster.CanBeHarmful(m))
                action(m);
        }
        eable.Free();
    }

    public static void DoConeDamage(
        Mobile caster,
        int range,
        int damageMin,
        int damageMax,
        int level,
        int phys, int fire, int cold, int poison, int energy,
        int effectID,
        int hue,
        string message)
    {
        Direction d = caster.Direction & Direction.Mask;
    
        for (int i = 1; i <= range; i++)
        {
            Point3D loc = GetPointInDirection(caster, d, i);
    
            Effects.SendLocationEffect(
                loc,
                caster.Map,
                effectID,
                20,
                10,
                hue,
                0
            );
    
            int damage = Utility.RandomMinMax(damageMin, damageMax) + level;
    
            DamageAtLocation(
                caster,
                loc,
                damage,
                phys, fire, cold, poison, energy
            );
        }
    
    }


        public static Point3D GetPointInDirection(Mobile m, Direction d, int distance)
        {
            switch (d & Direction.Mask)
            {
                case Direction.North: return new Point3D(m.X, m.Y - distance, m.Z);
                case Direction.South: return new Point3D(m.X, m.Y + distance, m.Z);
                case Direction.West: return new Point3D(m.X - distance, m.Y, m.Z);
                case Direction.East: return new Point3D(m.X + distance, m.Y, m.Z);
                default: return m.Location;
            }
        }

        public static void DamageAtLocation(Mobile caster, Point3D loc, int damage, int phys, int fire, int cold, int poison, int energy)
        {
            IPooledEnumerable eable = caster.Map.GetMobilesInRange(loc, 0);
            foreach (Mobile m in eable)
            {
                if (m != caster && m.Alive && caster.CanBeHarmful(m))
                {
                    caster.DoHarmful(m);
                    AOS.Damage(m, caster, damage, phys, fire, cold, poison, energy);
                    m.PlaySound(0x208);
                }
            }
            eable.Free();
        }

        public static void CreateAreaEffect(Point3D center, Map map, int range, double chance, int effectID, int speed, int duration, int hue)
        {
            for (int x = -range; x <= range; x++)
            {
                for (int y = -range; y <= range; y++)
                {
                    if (Utility.RandomDouble() < chance)
                    {
                        Point3D loc = new Point3D(center.X + x, center.Y + y, center.Z);
                        Effects.SendLocationEffect(loc, map, effectID, speed, duration, hue, 0);
                    }
                }
            }
        }

        public static void CreateExplosion(Point3D center, Map map, int effectID, int hue)
        {
            Effects.SendLocationEffect(center, map, effectID, 20, 10, hue, 0);
            Effects.PlaySound(center, map, 0x307);

            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    Point3D loc = new Point3D(center.X + x, center.Y + y, center.Z);
                    Effects.SendLocationEffect(loc, map, effectID, 20, 10, hue, 0);
                }
            }
        }

        public static List<Mobile> GetMobilesInRange(Mobile center, Mobile caster, int range)
        {
            List<Mobile> list = new List<Mobile>();
            IPooledEnumerable eable = center.GetMobilesInRange(range);
            foreach (Mobile m in eable)
            {
                if (m != caster && m.Alive && caster.CanBeHarmful(m))
                    list.Add(m);
            }
            eable.Free();
            return list;
        }

        public static Mobile FindNearbyEnemy(Mobile caster, int range)
        {
            IPooledEnumerable eable = caster.GetMobilesInRange(range);
            Mobile target = null;
            foreach (Mobile m in eable)
            {
                if (m != caster && m.Alive && caster.CanBeHarmful(m))
                {
                    target = m;
                    break;
                }
            }
            eable.Free();
            return target;
        }
    }

    // ===== LEVEL 1 =====
    public class BurningHandsSpell : CustomSpell
    {
        public BurningHandsSpell() : base("Burning Hands", 0x3709)
        {
            AddLevel(SpellType.Wizard, 1);
            AddLevel(SpellType.Sorcerer, 1);
            AddTag(SpellTag.AoE);
            AddTag(SpellTag.Offensive);
        }

        public override void Cast(Mobile caster, int hue, int level)
        {
            caster.FixedParticles(0x3709, 10, 15, 5052, hue != 0 ? hue : 1160, 0, EffectLayer.RightHand);
            caster.FixedParticles(0x3709, 10, 15, 5052, hue != 0 ? hue : 1160, 0, EffectLayer.LeftHand);
            
            SpellHelpers.DoConeDamage(
                caster,
                3,
                15, 20,
                level,
                0, 100, 0, 0, 0, 
                0x3709,
                hue != 0 ? hue : 1160,
                "You are burned by scorching flames!"
            );
        }
    }

    public class CauseFearSpell : CustomSpell
    {
        public CauseFearSpell() : base("Cause Fear", 0x3709)
        {
            AddLevel(SpellType.Wizard, 1);
            AddLevel(SpellType.Sorcerer, 1);
            AddLevel(SpellType.Bard, 1);
            AddLevel(SpellType.Cleric, 1);
            AddTag(SpellTag.SingleTarget);
            AddTag(SpellTag.CC);
        }

        public override void Cast(Mobile caster, int hue, int level)
        {
            Mobile target = caster.Combatant as Mobile;
            if (target == null || !target.Alive || !caster.CanBeHarmful(target))
                return;

            if (target.Skills[SkillName.Knightship].Value > 50 + level * 2)
            {
                target.SendMessage("Your bravery shields you from fear!");
                target.FixedEffect(0x375A, 10, 20, hue != 0 ? hue : 0x47E, 0);
                target.PlaySound(0x1F7);
                return;
            }

            caster.DoHarmful(target);

            target.FixedParticles(0x374A, 10, 15, 5013, hue != 0 ? hue : 0x455, 0, EffectLayer.Waist);
            target.FixedParticles(0x376A, 9, 20, 5044, hue != 0 ? hue : 0x455, 0, EffectLayer.Head);
            target.PlaySound(0x1F8);

            Direction dir = target.GetDirectionTo(caster);
            int distance = level + 1;
            
            for (int i = 0; i < distance; i++)
            {
                target.Move(dir);
                
                if (i % 2 == 0)
                {
                    Effects.SendLocationEffect(target.Location, target.Map, 0x3728, 10, 10, hue != 0 ? hue : 0x455, 0);
                }
            }

            target.FixedParticles(0x376A, 9, 32, 5008, hue != 0 ? hue : 0x455, 0, EffectLayer.Waist);
            target.SendMessage("You flee in terror!");
        }
    }

    public class LesserVigorSpell : CustomSpell
    {
        public LesserVigorSpell() : base("Vigor, Lesser", 0x376A)
        {
            AddLevel(SpellType.Cleric, 1);
            AddLevel(SpellType.Druid, 1);
            AddTag(SpellTag.Heal);
            AddTag(SpellTag.SingleTarget);
            AddTag(SpellTag.HoT);
        }

        public override void Cast(Mobile caster, int hue, int level)
        {
            int duration = 6 + (level / 2);
            
            caster.FixedParticles(0x376A, 9, 32, 5030, hue != 0 ? hue : 0x47D, 0, EffectLayer.Waist);
            caster.PlaySound(0x1F7);
            caster.SendMessage("You feel vitality flowing through your body.");
            
            NatureSpellHelper.ApplyVigor(caster, 3, duration, level);
        }
    }


    public class CureLightWoundsSpell : CustomSpell
    {
        public CureLightWoundsSpell() : base("Cure Light Wounds", 0x376A)
        {
            AddLevel(SpellType.Cleric, 1);
            AddLevel(SpellType.Druid, 1);
            AddLevel(SpellType.Bard, 1);
            AddTag(SpellTag.Heal);
            AddTag(SpellTag.SingleTarget);
        }

        public override void Cast(Mobile caster, int hue, int level)
        {
            int healAmount = Utility.RandomMinMax(4, 12) * level;
            caster.Heal(healAmount);
            
            caster.FixedParticles(0x376A, 9, 32, 5030, hue != 0 ? hue : 0x47D, 0, EffectLayer.Waist);
            caster.FixedParticles(0x375A, 10, 15, 5018, hue != 0 ? hue : 0x47D, 0, EffectLayer.Head);
            caster.PlaySound(0x202);
            
            caster.SendMessage("You feel healing energy flow through you.");
        }
    }

    public class EntangleSpell : CustomSpell
    {
        public EntangleSpell() : base("Entangle", 0x3728)
        {
            AddLevel(SpellType.Druid, 1);
            AddTag(SpellTag.CC);
            AddTag(SpellTag.AoE);
        }

        public override void Cast(Mobile caster, int hue, int level)
        {
            Mobile target = caster.Combatant as Mobile;
            if (target == null || !target.Alive)
                return;

            SpellHelpers.CreateAreaEffect(
                target.Location, 
                target.Map, 
                4, 
                0.5, 
                0x3735, 
                20, 
                10, 
                hue != 0 ? hue : 0x47E
            );

            Effects.PlaySound(target.Location, target.Map, 0x5C6);

            SpellHelpers.ForEachHostileInRange(target, caster, 4, delegate(Mobile m)
            {
                caster.DoHarmful(m);
                
                double duration = Math.Max(3.0, (3.0 + level) - (m.Skills[SkillName.MagicResist].Value / 100.0 * 6.0));
                m.Paralyze(TimeSpan.FromSeconds(duration));
                
                m.FixedEffect(0x376A, 9, 32);
                m.FixedParticles(0x3735, 10, 20, 5052, hue != 0 ? hue : 0x47E, 0, EffectLayer.Waist);
                m.PlaySound(0x204);
                m.SendMessage("Magical vines entangle you!");
            });
        }
    }

    public class MagicMissileSpell : CustomSpell
    {
        public MagicMissileSpell() : base("Magic Missile", 0x36E4)
        {
            AddLevel(SpellType.Wizard, 1);
            AddLevel(SpellType.Sorcerer, 1);
            AddTag(SpellTag.Offensive);
            AddTag(SpellTag.SingleTarget);
        }

        public override void Cast(Mobile caster, int hue, int level)
        {
            Mobile target = caster.Combatant as Mobile;
            if (target == null || !caster.CanBeHarmful(target))
                return;

            caster.DoHarmful(target);
            
            int missiles = (level >= 9) ? 5 : (level >= 7) ? 4 : (level >= 5) ? 3 : (level >= 3) ? 2 : 1;

            for (int i = 0; i < missiles; i++)
            {
                Timer.DelayCall(TimeSpan.FromSeconds(i * 0.3), delegate ()
                {
                    if (target == null || !target.Alive || target.Deleted)
                        return;

                    Effects.SendMovingEffect(
                        caster, 
                        target, 
                        0x379F, 
                        7, 
                        0, 
                        false, 
                        false, 
                        hue != 0 ? hue : 0x0213, 
                        0
                    );
                    Effects.PlaySound(caster.Location, caster.Map, 0x1F5);

                    Timer.DelayCall(TimeSpan.FromSeconds(0.5), delegate ()
                    {
                        if (target == null || !target.Alive || target.Deleted)
                            return;

                        int damage = Utility.RandomMinMax(3, 7) + level;
                        AOS.Damage(target, caster, damage, 0, 0, 0, 0, 100);
                        
                        Effects.SendLocationEffect(
                            target.Location, 
                            target.Map, 
                            0x3709, 
                            10, 
                            30, 
                            hue != 0 ? hue : 0x0213, 
                            0
                        );
                        target.FixedParticles(0x36BD, 6, 10, 5044, hue != 0 ? hue : 0x0213, 0, EffectLayer.Waist);
                    });
                });
            }
        }
    }

    public class SleepSpell : CustomSpell
    {
        public SleepSpell() : base("Sleep", 0x376A)
        {
            AddLevel(SpellType.Wizard, 1);
            AddLevel(SpellType.Sorcerer, 1);
            AddLevel(SpellType.Bard, 1);
            AddTag(SpellTag.CC);
            AddTag(SpellTag.AoE);
        }

        public override void Cast(Mobile caster, int hue, int level)
        {
            Mobile target = caster.Combatant as Mobile;
            if (target == null || !target.Alive)
                return;

            double baseDuration = Math.Min(9.0, 3.0 + level);
            
            target.FixedEffect(0x375A, 10, 20, hue != 0 ? hue : 0x47E, 0);
            target.FixedParticles(0x373A, 10, 15, 5036, hue != 0 ? hue : 0x47E, 0, EffectLayer.Head);
            target.PlaySound(0x1F2);

            SpellHelpers.ForEachHostileInRange(target, caster, 1, delegate(Mobile m)
            {
                caster.DoHarmful(m);
                
                double duration = Math.Max(1.0, baseDuration - (m.Skills[SkillName.MagicResist].Value / 100.0 * baseDuration));
                m.Paralyze(TimeSpan.FromSeconds(duration));
                
                m.FixedEffect(0x376A, 9, 32, hue != 0 ? hue : 0x47E, 0);
                m.FixedParticles(0x375A, 9, 20, 5044, hue != 0 ? hue : 0x47E, 0, EffectLayer.Head);
                m.PlaySound(0x204);
                m.SendMessage("You fall into a magical slumber!");
            });
        }
    }

    public class SporeFieldSpell : CustomSpell
    {
        public SporeFieldSpell() : base("Spore Field", 0x375A)
        {
            AddLevel(SpellType.Druid, 1);
            AddTag(SpellTag.CC);
            AddTag(SpellTag.Debuff);
            AddTag(SpellTag.AoE);
        }

        public override void Cast(Mobile caster, int hue, int level)
        {
            Mobile target = caster.Combatant as Mobile;
            if (target == null)
                target = caster;

            int range = 1;
            TimeSpan duration = TimeSpan.FromSeconds(8 + level);

            Effects.SendLocationEffect(target.Location, target.Map, 0x3728, 20, 10, 0x48, 0);
            caster.PlaySound(0x222);

            for (int x = -range; x <= range; x++)
            {
                for (int y = -range; y <= range; y++)
                {
                    Point3D loc = new Point3D(
                        target.X + x,
                        target.Y + y,
                        target.Z
                    );

                    NatureSpellHelper.SpawnSporeTile(loc, target.Map, caster, level, duration);
                }
            }
        }
    }


    public class SummonNatureAllyISpell : SummonNatureAllySpell
    {
        public SummonNatureAllyISpell()
            : base("Summon Nature's Ally I", 0x2156, 1, typeof(GiantRat))
        {
        }
    }


    // ===== LEVEL 2 =====

    public class AidSpell : CustomSpell
    {
        public AidSpell() : base("Aid", 0x376A)
        {
            AddLevel(SpellType.Cleric, 2);
            AddTag(SpellTag.Buff);
            AddTag(SpellTag.Heal);
        }

        public override void Cast(Mobile caster, int hue, int level)
        {
            int strBonus = 13 + level;
            int healAmount = Utility.RandomMinMax(12, 22) + level;

            caster.AddStatMod(new StatMod(StatType.Str, "AidStr", strBonus, TimeSpan.FromSeconds(30)));
            caster.Hits += healAmount;

            caster.FixedParticles(0x376A, 10, 32, 5030, hue != 0 ? hue : 0x47D, 0, EffectLayer.Waist);
            caster.FixedParticles(0x375A, 9, 20, 5030, hue != 0 ? hue : 0x47D, 0, EffectLayer.Head);
            caster.FixedParticles(0x373A, 10, 15, 5018, hue != 0 ? hue : 0x47D, 0, EffectLayer.RightHand);
            caster.PlaySound(0x1F2);
        }
    }

    public class BearsEnduranceSpell : CustomSpell
    {
        public BearsEnduranceSpell() : base("Bear's Endurance", 0x480)
        {
            AddLevel(SpellType.Wizard, 2);
            AddLevel(SpellType.Sorcerer, 2);
            AddLevel(SpellType.Druid, 2);
            AddLevel(SpellType.Cleric, 2);
            AddTag(SpellTag.Buff);
            AddTag(SpellTag.SingleTarget);
        }

        public override void Cast(Mobile caster, int hue, int level)
        {
            int bonusHP = 35 + level;
            int strBonus = Math.Max(1, bonusHP / 2);

            caster.AddStatMod(new StatMod(StatType.Str, "BearsEndurance", strBonus, TimeSpan.FromSeconds(60)));
            caster.Hits += bonusHP;

            caster.FixedEffect(0x376A, 10, 20, hue != 0 ? hue : 0x21, 0);
            caster.FixedParticles(0x373A, 10, 15, 5018, hue != 0 ? hue : 0x21, 0, EffectLayer.Waist);
            caster.FixedParticles(0x375A, 9, 20, 5044, hue != 0 ? hue : 0x21, 0, EffectLayer.Head);
            caster.PlaySound(0x1EA);
            
        }
    }



    public class BodyOfTheSunSpell : CustomSpell
    {
        public BodyOfTheSunSpell() : base("Body of the Sun", 0x3709)
        {
            AddLevel(SpellType.Druid, 2);
            AddLevel(SpellType.Wizard, 2);
            AddLevel(SpellType.Sorcerer, 2);
            AddTag(SpellTag.DoT);
            AddTag(SpellTag.AoE);
            AddTag(SpellTag.Offensive);
        }

        public override void Cast(Mobile caster, int hue, int level)
        {
            int duration = level * 2;

            caster.FixedEffect(0x3709, 10, 30, hue != 0 ? hue : 0x501, 0);
            caster.FixedParticles(0x376A, 9, 32, 5008, hue != 0 ? hue : 0x501, 0, EffectLayer.Waist);
            caster.PlaySound(0x208);
            caster.SendMessage("Your body radiates scorching heat!");

            new BodyOfTheSunTimer(caster, level, duration, hue).Start();
        }

        private class BodyOfTheSunTimer : Timer
        {
            private Mobile m_Caster;
            private int m_Level;
            private int m_Ticks;
            private int m_Hue;

            public BodyOfTheSunTimer(Mobile caster, int level, int duration, int hue)
                : base(TimeSpan.Zero, TimeSpan.FromSeconds(2))
            {
                m_Caster = caster;
                m_Level = level;
                m_Ticks = duration / 2;
                m_Hue = hue;
            }

            protected override void OnTick()
            {
                if (m_Caster == null || m_Caster.Deleted || !m_Caster.Alive || m_Ticks-- <= 0)
                {
                    Stop();
                    return;
                }

                m_Caster.FixedParticles(0x3709, 5, 10, 5052, m_Hue != 0 ? m_Hue : 0x501, 0, EffectLayer.Waist);

                SpellHelpers.ForEachHostileInRange(m_Caster, m_Caster, 1, delegate(Mobile m)
                {
                    m_Caster.DoHarmful(m);
                    
                    int dmg = Utility.RandomMinMax(8, 13) + m_Level;
                    AOS.Damage(m, m_Caster, dmg, 0, 100, 0, 0, 0);

                    m.FixedEffect(0x3709, 10, 20, m_Hue != 0 ? m_Hue : 0x501, 0);
                    m.PlaySound(0x208);
                });
            }
        }
    }


    public class BullsStrengthSpell : CustomSpell
    {
        public BullsStrengthSpell() : base("Bull's Strength", 0x375A)
        {
            AddLevel(SpellType.Wizard, 2);
            AddLevel(SpellType.Sorcerer, 2);
            AddLevel(SpellType.Cleric, 2);
            AddLevel(SpellType.Druid, 2);
            AddTag(SpellTag.SingleTarget);
            AddTag(SpellTag.Buff);
        }

        public override void Cast(Mobile caster, int hue, int level)
        {
            int bonus = 15 + level;
            caster.RawStr += bonus;

            Timer.DelayCall(TimeSpan.FromSeconds(30), delegate
            {
                if (caster != null && !caster.Deleted)
                    caster.RawStr -= bonus;
            });

            caster.FixedEffect(0x375A, 10, 20, hue != 0 ? hue : 0x21, 0);
            caster.FixedParticles(0x373A, 10, 15, 5018, hue != 0 ? hue : 0x21, 0, EffectLayer.Waist);
            caster.FixedParticles(0x376A, 9, 20, 5044, hue != 0 ? hue : 0x21, 0, EffectLayer.LeftHand);
            caster.FixedParticles(0x376A, 9, 20, 5044, hue != 0 ? hue : 0x21, 0, EffectLayer.RightHand);
            caster.PlaySound(0x1E9);
            
            caster.SendMessage("You feel the strength of a raging bull!");
        }
    }

    public class CatsGraceSpell : CustomSpell
    {
        public CatsGraceSpell() : base("Cat's Grace", 0x375A)
        {
            AddLevel(SpellType.Wizard, 2);
            AddLevel(SpellType.Sorcerer, 2);
            AddLevel(SpellType.Bard, 2);
            AddLevel(SpellType.Druid, 2);
            AddTag(SpellTag.SingleTarget);
            AddTag(SpellTag.Buff);
        }

        public override void Cast(Mobile caster, int hue, int level)
        {
            int bonus = 15 + level;
            caster.RawDex += bonus;

            Timer.DelayCall(TimeSpan.FromSeconds(30), delegate
            {
                if (caster != null && !caster.Deleted)
                    caster.RawDex -= bonus;
            });

            caster.FixedEffect(0x375A, 10, 20, hue != 0 ? hue : 0x47E, 0);
            caster.FixedParticles(0x376A, 9, 32, 5008, hue != 0 ? hue : 0x47E, 0, EffectLayer.Waist);
            caster.FixedParticles(0x373A, 10, 15, 5036, hue != 0 ? hue : 0x47E, 0, EffectLayer.Head);
            caster.PlaySound(0x1E9);
            
            caster.SendMessage("You move with feline grace and agility!");
        }
    }

    public class CloudOfKnivesSpell : CustomSpell
    {
        public CloudOfKnivesSpell() : base("Cloud of Knives", 0x36CB)
        {
            AddLevel(SpellType.Cleric, 2);
            AddLevel(SpellType.Wizard, 2);
            AddLevel(SpellType.Sorcerer, 2);
            AddTag(SpellTag.DoT);
            AddTag(SpellTag.Offensive);
        }

        public override void Cast(Mobile caster, int hue, int level)
        {
            Mobile target = caster.Combatant as Mobile;
            if (target == null)
                return;

            Point3D loc = target.Location;
            Map map = target.Map;

            int duration = 6 + (level / 3) * 2;

            // Enhanced visuals: Spinning blade cloud
            Effects.SendLocationEffect(loc, map, 0x36CB, 20, 10, hue != 0 ? hue : 0x481, 0);
            Effects.PlaySound(loc, map, 0x23B);

            new CloudOfKnivesTimer(caster, loc, map, level, duration, hue).Start();
        }

        private class CloudOfKnivesTimer : Timer
        {
            private Mobile m_Caster;
            private Point3D m_Loc;
            private Map m_Map;
            private int m_Level;
            private int m_Ticks;
            private int m_Hue;

            public CloudOfKnivesTimer(Mobile caster, Point3D loc, Map map, int level, int duration, int hue)
                : base(TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(2))
            {
                m_Caster = caster;
                m_Loc = loc;
                m_Map = map;
                m_Level = level;
                m_Ticks = duration / 2;
                m_Hue = hue;
            }

            protected override void OnTick()
            {
                if (m_Caster == null || m_Caster.Deleted || m_Ticks-- <= 0)
                {
                    Stop();
                    return;
                }
                Effects.SendLocationEffect(m_Loc, m_Map, 0x36CB, 10, 10, m_Hue != 0 ? m_Hue : 0x481, 0);
                Effects.PlaySound(m_Loc, m_Map, 0x23B);

                IPooledEnumerable eable = m_Map.GetMobilesInRange(m_Loc, 0);
                foreach (Mobile m in eable)
                {
                    if (m.Alive && m_Caster.CanBeHarmful(m))
                    {
                        m_Caster.DoHarmful(m);
                        
                        int dmg = Utility.RandomMinMax(8, 13) + m_Level;
                        AOS.Damage(m, m_Caster, dmg, 100, 0, 0, 0, 0);
                        
                        m.SendMessage("Whirling blades slice into you!");
                    }
                }
                eable.Free();
            }
        }
    }


     public class DarkBoltSpell : CustomSpell
    {
        public DarkBoltSpell() : base("Dark Bolt", 0x36BD)
        {
            AddLevel(SpellType.Cleric, 2);
            AddLevel(SpellType.Wizard, 2);
            AddLevel(SpellType.Sorcerer, 2);
            AddTag(SpellTag.SingleTarget);
            AddTag(SpellTag.Offensive);
            AddTag(SpellTag.CC);
        }

        public override void Cast(Mobile caster, int hue, int level)
        {
            Mobile target = caster.Combatant as Mobile;
            if (target == null || !caster.CanBeHarmful(target))
                return;

            caster.DoHarmful(target);

            caster.MovingEffect(target, 0x36BD, 7, 0, false, false, hue != 0 ? hue : 0x497, 0);
            caster.FixedParticles(0x374A, 10, 15, 5013, hue != 0 ? hue : 0x497, 0, EffectLayer.RightHand);

            Timer.DelayCall(TimeSpan.FromSeconds(0.5), delegate()
            {
                if (target == null || target.Deleted || !target.Alive)
                    return;

                int dmg = Utility.RandomMinMax(17, 22) + level;
                AOS.Damage(target, caster, dmg, 100, 0, 0, 0, 0);

                target.FixedParticles(0x374A, 10, 15, 5013, hue != 0 ? hue : 0x497, 0, EffectLayer.Waist);
                target.FixedParticles(0x36BD, 6, 10, 5044, hue != 0 ? hue : 0x497, 0, EffectLayer.Head);
                target.PlaySound(0x1F2);

                double resist = target.Skills[SkillName.MagicResist].Value;
                int roll = Utility.RandomMinMax(6, 26) + level;

                if (resist < roll)
                {
                    target.Paralyze(TimeSpan.FromSeconds(4));
                    target.SendMessage("Dark energy freezes you in place!");
                }
                else
                {
                    target.SendMessage("You resist the paralyzing energy!");
                }
            });
        }
    }

    public class FlamingSphereSpell : CustomSpell
    {
        public FlamingSphereSpell() : base("Flaming Sphere", 0x36BD)
        {
            AddLevel(SpellType.Druid, 2);
            AddLevel(SpellType.Wizard, 2);
            AddLevel(SpellType.Sorcerer, 2);
            AddTag(SpellTag.Offensive);
            AddTag(SpellTag.AoE);
            AddTag(SpellTag.DoT);
        }

        public override void Cast(Mobile caster, int hue, int level)
        {
            Mobile target = caster.Combatant as Mobile;
            if (target == null)
                return;

            Point3D loc = target.Location;
            Map map = target.Map;

            Effects.SendLocationEffect(loc, map, 0x36BD, 20, 10, hue != 0 ? hue : 0x489, 0);
            Effects.PlaySound(loc, map, 0x208);

            int duration = 6 + (level / 3) * 2;
            new FlamingSphereTimer(caster, loc, map, level, duration, hue).Start();
        }

        private class FlamingSphereTimer : Timer
        {
            private Mobile m_Caster;
            private Point3D m_Loc;
            private Map m_Map;
            private int m_Level;
            private int m_Ticks;
            private int m_Hue;

            public FlamingSphereTimer(Mobile caster, Point3D loc, Map map, int level, int duration, int hue)
                : base(TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(2))
            {
                m_Caster = caster;
                m_Loc = loc;
                m_Map = map;
                m_Level = level;
                m_Ticks = duration / 2;
                m_Hue = hue;
            }

            protected override void OnTick()
            {
                if (m_Caster == null || m_Caster.Deleted || m_Ticks-- <= 0)
                {
                    Stop();
                    return;
                }

                Effects.SendLocationEffect(m_Loc, m_Map, 0x36BD, 20, 10, m_Hue != 0 ? m_Hue : 0x489, 0);
                Effects.PlaySound(m_Loc, m_Map, 0x208);

                IPooledEnumerable eable = m_Map.GetMobilesInRange(m_Loc, 1);
                foreach (Mobile m in eable)
                {
                    if (m.Alive && m_Caster.CanBeHarmful(m))
                    {
                        m_Caster.DoHarmful(m);
                        
                        int dmg = Utility.RandomMinMax(12, 21) + m_Level;
                        AOS.Damage(m, m_Caster, dmg, 0, 100, 0, 0, 0);
                        
                        m.FixedParticles(0x3709, 10, 15, 5052, m_Hue != 0 ? m_Hue : 0x489, 0, EffectLayer.Waist);
                        m.SendMessage("The flaming sphere burns you!");
                    }
                }
                eable.Free();
            }
        }
    }



    public class HoldPersonSpell : CustomSpell
    {
        public HoldPersonSpell() : base("Hold Person", 0x376A)
        {
            AddLevel(SpellType.Cleric, 2);
            AddLevel(SpellType.Wizard, 3);
            AddLevel(SpellType.Sorcerer, 3);
            AddTag(SpellTag.SingleTarget);
            AddTag(SpellTag.CC);
        }

        public override void Cast(Mobile caster, int hue, int level)
        {
            Mobile target = caster.Combatant as Mobile;
            if (target == null || !(target is PlayerMobile) || !caster.CanBeHarmful(target))
                return;

            caster.DoHarmful(target);
            
            double duration = Math.Max(1.0, (6.0 + level) - (target.Skills[SkillName.MagicResist].Value / 100.0 * 6.0));
            target.Paralyze(TimeSpan.FromSeconds(duration));

            target.FixedParticles(0x376A, 9, 32, 5030, hue != 0 ? hue : 0x0, 0, EffectLayer.Waist);
            target.FixedParticles(0x3779, 10, 15, 5009, hue != 0 ? hue : 0x0, 0, EffectLayer.Head);
            target.FixedParticles(0x375A, 10, 20, 5044, hue != 0 ? hue : 0x0, 0, EffectLayer.LeftHand);
            target.FixedParticles(0x375A, 10, 20, 5044, hue != 0 ? hue : 0x0, 0, EffectLayer.RightHand);
            target.PlaySound(0x204);
            target.SendMessage("Magical bonds hold you completely still!");
        }
    }

    public class MelfsAcidArrowSpell : CustomSpell
    {
        public MelfsAcidArrowSpell() : base("Melf's Acid Arrow", 0x36D4)
        {
            AddLevel(SpellType.Wizard, 2);
            AddLevel(SpellType.Sorcerer, 2);
            AddTag(SpellTag.SingleTarget);
            AddTag(SpellTag.Offensive);
            AddTag(SpellTag.DoT);
        }

        public override void Cast(Mobile caster, int hue, int level)
        {
            Mobile target = caster.Combatant as Mobile;
            if (target == null || !caster.CanBeHarmful(target))
                return;

            caster.DoHarmful(target);

            Effects.SendMovingEffect(caster, target, 0x36D4, 10, 0, false, false, hue > 0 ? hue : 0x48E, 0);
            Effects.PlaySound(caster.Location, caster.Map, 0x5D3);

            Timer.DelayCall(TimeSpan.FromSeconds(0.5), delegate()
            {
                if (target == null || target.Deleted || !target.Alive)
                    return;

                int initialDamage = Utility.RandomMinMax(20, 25) + level;
                AOS.Damage(target, caster, initialDamage, 0, 0, 0, 100, 0);

                target.FixedParticles(0x36BD, 10, 20, 5044, 0x48E, 0, EffectLayer.Waist);
                target.SendMessage("Acid sears your flesh!");

                Timer.DelayCall(TimeSpan.FromSeconds(3), TimeSpan.FromSeconds(3), level,
                    new TimerStateCallback(DoTick), new object[] { caster, target });
            });
        }

        private void DoTick(object state)
        {
            object[] data = (object[])state;
            Mobile caster = (Mobile)data[0];
            Mobile target = (Mobile)data[1];

            if (target == null || target.Deleted || !target.Alive)
                return;

            int dotDamage = Utility.RandomMinMax(12, 17);
            AOS.Damage(target, caster, dotDamage, 0, 0, 0, 100, 0);
            
            target.FixedEffect(0x36BD, 10, 20, 0x48E, 0);
            target.SendMessage("The acid continues to burn!");
        }
    }

    public class OwlsWisdomSpell : CustomSpell
    {
        public OwlsWisdomSpell() : base("Owl's Wisdom", 0x375A)
        {
            AddLevel(SpellType.Wizard, 2);
            AddLevel(SpellType.Sorcerer, 2);
            AddLevel(SpellType.Cleric, 2);
            AddLevel(SpellType.Druid, 2);
            AddTag(SpellTag.SingleTarget);
            AddTag(SpellTag.Buff);
        }

        public override void Cast(Mobile caster, int hue, int level)
        {
            int bonus = 15 + level;
            caster.RawInt += bonus;

            Timer.DelayCall(TimeSpan.FromSeconds(30), delegate
            {
                if (caster != null && !caster.Deleted)
                    caster.RawInt -= bonus;
            });

            caster.FixedEffect(0x375A, 10, 20, hue != 0 ? hue : 0x480, 0);
            caster.FixedParticles(0x373A, 10, 15, 5018, hue != 0 ? hue : 0x480, 0, EffectLayer.Waist);
            caster.FixedParticles(0x376A, 9, 32, 5008, hue != 0 ? hue : 0x480, 0, EffectLayer.Head);
            caster.PlaySound(0x1E9);
            
            caster.SendMessage("Ancient wisdom fills your mind!");
        }
    }

    public class ScorchingRaySpell : CustomSpell
    {
        public ScorchingRaySpell() : base("Scorching Ray", 0x36D4)
        {
            AddLevel(SpellType.Wizard, 2);
            AddLevel(SpellType.Sorcerer, 2);
            AddTag(SpellTag.SingleTarget);
            AddTag(SpellTag.Offensive);
        }

        public override void Cast(Mobile caster, int hue, int level)
        {
            Mobile target = caster.Combatant as Mobile;
            if (target == null || !caster.CanBeHarmful(target))
                return;

            caster.DoHarmful(target);
            
            int rays = Math.Min(3, 1 + ((level - 1) / 4));

            for (int i = 0; i < rays; i++)
            {
                Timer.DelayCall(TimeSpan.FromSeconds(i * 0.2), delegate()
                {
                    if (target == null || target.Deleted || !target.Alive)
                        return;

                    caster.MovingParticles(target, 0x36D4, 7, 0, false, true, hue != 0 ? hue : 1160, 0, 9502, 1, 0, (EffectLayer)255, 0x100);
                    
                    int damage = Utility.RandomMinMax(10, 18) + level;
                    AOS.Damage(target, caster, damage, 0, 100, 0, 0, 0);
                    
                    target.FixedParticles(0x3709, 10, 15, 5052, hue != 0 ? hue : 1160, 0, EffectLayer.Waist);
                });
            }
            
            target.PlaySound(0x208);
        }
    }

    public class SummonNatureAllyIISpell : SummonNatureAllySpell
    {
        public SummonNatureAllyIISpell()
            : base("Summon Nature's Ally II", 0x2156, 2, typeof(BlackBear))
        {
        }
    }


    public class WebSpell : CustomSpell
    {
        public WebSpell() : base("Web", 0x376a)
        {
            AddLevel(SpellType.Wizard, 2);
            AddLevel(SpellType.Sorcerer, 2);
            AddTag(SpellTag.AoE);
            AddTag(SpellTag.CC);
        }

        public override void Cast(Mobile caster, int hue, int level)
        {
            Mobile target = caster.Combatant as Mobile;
            if (target == null)
                return;

            List<Mobile> targets = SpellHelpers.GetMobilesInRange(target, caster, 2);

            foreach (Mobile m in targets)
            {
                double duration = Math.Max(3.0, (8 + level) - (m.Dex / 10.0));
                m.Paralyze(TimeSpan.FromSeconds(duration));
                Effects.SendLocationEffect(m.Location, m.Map, 0x23AF, 30, 10, hue != 0 ? hue : 0x0, 0);
                m.FixedParticles(0x376A, 9, 32, 5008, EffectLayer.Waist);
                Effects.PlaySound(m.Location, m.Map, 0x5D2);
                m.SendMessage("You are trapped in a magical web!");
            }
        }
    }
    // ===== LEVEL 3 =====
    public class AcidballSpell : CustomSpell
    {
        public AcidballSpell() : base("Acidball", 0x36D4)
        {
            AddLevel(SpellType.Wizard, 3);
            AddLevel(SpellType.Sorcerer, 3);
            AddTag(SpellTag.AoE);
            AddTag(SpellTag.Offensive);
        }

        public override void Cast(Mobile caster, int hue, int level)
        {
            Mobile target = caster.Combatant as Mobile;
            if (target == null || target.Deleted || !target.Alive)
                return;

            SpellHelpers.CreateExplosion(target.Location, target.Map, 0x36BD, hue != 0 ? hue : 0x4F6);
            List<Mobile> targets = SpellHelpers.GetMobilesInRange(target, caster, 1);
            foreach (Mobile m in targets)
                AOS.Damage(m, caster, Utility.RandomMinMax(20, 25) + level, 0, 0, 0, 100, 0);
        }
    }

    public abstract class AuraOfColdSpell : CustomSpell
    {
        protected int Min;
        protected int Max;

        protected AuraOfColdSpell(string name, int hue, int min, int max) : base(name, hue)
        {
            Min = min;
            Max = max;

            AddTag(SpellTag.DoT);
            AddTag(SpellTag.AoE);
        }

        public override void Cast(Mobile caster, int hue, int level)
        {
            int ticks = (level * 2) / 2;

            caster.FixedParticles(0x374A, 10, 30, 5032, hue, 3, EffectLayer.Waist);

            Timer.DelayCall(TimeSpan.Zero, TimeSpan.FromSeconds(2), ticks, () =>
            {
                if (caster.Deleted)
                    return;

                foreach (Mobile m in caster.GetMobilesInRange(1))
                {
                    if (m != caster && caster.CanBeHarmful(m))
                        continue;

                    int dmg = Utility.RandomMinMax(Min, Max) + level;

                    SpellHelper.Damage(
                        TimeSpan.Zero,
                        m,
                        caster,
                        dmg,
                        0, 0, 0, 100, 0
                    );
                }
            });
        }
    }

    public class AuraOfColdLesserSpell : AuraOfColdSpell
    {
        public AuraOfColdLesserSpell()
            : base("Aura of Cold, Lesser", 0x480, 16, 21)
        {
            AddLevel(SpellType.Cleric, 3);
            AddLevel(SpellType.Druid, 3);
        }
    }



    public class BestowCurseSpell : CustomSpell
    {
        public BestowCurseSpell() : base("Bestow Curse", 0x455)
        {
            AddLevel(SpellType.Bard, 3);
            AddLevel(SpellType.Cleric, 3);
            AddLevel(SpellType.Wizard, 4);
            AddLevel(SpellType.Sorcerer, 4);

            AddTag(SpellTag.Debuff);
            AddTag(SpellTag.SingleTarget);
        }

        public override void Cast(Mobile caster, int hue, int level)
        {
            Mobile target = caster.Combatant as Mobile;
            if (target == null || !caster.CanBeHarmful(target))
                return;

            caster.DoHarmful(target);

            int strLoss = (int)(target.RawStr * (0.12 + level * 0.01));
            TimeSpan dur = TimeSpan.FromSeconds(30 + level);

            target.AddStatMod(new StatMod(StatType.Str, "BestowCurse", -strLoss, dur));

            target.FixedEffect(0x3728, 10, 20);
            target.PlaySound(0x1F8);
        }
    }


    public class CallLightningSpell : CustomSpell
    {
        public CallLightningSpell() : base("Call Lightning", 0x379F)
        {
            AddLevel(SpellType.Druid, 3);
            AddTag(SpellTag.AoE);
            AddTag(SpellTag.Offensive);
            AddTag(SpellTag.DoT);
        }

        public override void Cast(Mobile caster, int hue, int level)
        {
            DateTime end = DateTime.UtcNow + TimeSpan.FromSeconds(6 + level);

            Timer.DelayCall(TimeSpan.Zero, TimeSpan.FromSeconds(3), delegate
            {
                if (caster.Deleted || !caster.Alive || DateTime.UtcNow > end)
                    return;

                Mobile target = SpellHelpers.FindNearbyEnemy(caster, 3);
                if (target != null)
                {
                    caster.DoHarmful(target);
                    target.BoltEffect(hue != 0 ? hue : 0);
                    AOS.Damage(target, caster, Utility.RandomMinMax(16, 21) + level, 0, 0, 0, 0, 100);
                    target.PlaySound(0x29);
                }
            });
        }
    }

    public class CureModerateWoundsSpell : CustomSpell
    {
        public CureModerateWoundsSpell() : base("Cure Moderate Wounds", 0x376A)
        {
            AddLevel(SpellType.Cleric, 3);
            AddLevel(SpellType.Druid, 3);
            AddLevel(SpellType.Bard, 4);
            AddTag(SpellTag.Heal);
            AddTag(SpellTag.SingleTarget);
        }

        public override void Cast(Mobile caster, int hue, int level)
        {
            caster.Heal(Utility.RandomMinMax(3, 7) * level);
            caster.FixedParticles(0x376A, 9, 32, 5030, hue != 0 ? hue : 0x47D, 0, EffectLayer.Waist);
            caster.FixedParticles(0x375A, 10, 15, 5018, hue != 0 ? hue : 0x47D, 0, EffectLayer.Head);
            caster.PlaySound(0x202);
        }
    }

    public class ContagionSpell : CustomSpell
    {
        public ContagionSpell() : base("Contagion", 0x36BD)
        {
            AddLevel(SpellType.Cleric, 3);
            AddLevel(SpellType.Druid, 3);
            AddLevel(SpellType.Wizard, 4);
            AddLevel(SpellType.Sorcerer, 4);
            AddTag(SpellTag.AoE);
            AddTag(SpellTag.Offensive);
            AddTag(SpellTag.Debuff);
        }

        public override void Cast(Mobile caster, int hue, int level)
        {
            Mobile target = caster.Combatant as Mobile;
            if (target == null || !caster.CanBeHarmful(target))
                return;

            caster.DoHarmful(target);

            target.ApplyPoison(caster, Poison.Greater);

            int stamLoss = 12 + level * 3;
            target.Stam = Math.Max(0, target.Stam - stamLoss);

            int strLoss = 6 + level;
            target.AddStatMod(new StatMod(StatType.Str, "ContagionStr", -strLoss, TimeSpan.FromMinutes(2)));

            target.FixedEffect(0x36BD, 10, 20);
            target.PlaySound(0x205);
        }
    }


    public class DeafeningBlastSpell : CustomSpell
    {
        public DeafeningBlastSpell() : base("Deafening Blast", 0x3728)
        {
            AddLevel(SpellType.Bard, 3);
            AddTag(SpellTag.AoE);
            AddTag(SpellTag.Offensive);
            AddTag(SpellTag.Debuff);
        }

        public override void Cast(Mobile caster, int hue, int level)
        {
            Mobile target = caster.Combatant as Mobile;
            if (target == null || !target.Alive)
                return;

            int damage = Utility.RandomMinMax(20, 25) + level;
            target.FixedEffect(0x37C4, 10, 30, hue != 0 ? hue : 0x0, 0);

            IPooledEnumerable eable = target.GetMobilesInRange(1);
            foreach (Mobile m in eable)
            {
                if (m != caster && m.Alive && caster.CanBeHarmful(m))
                {
                    caster.DoHarmful(m);
                    AOS.Damage(m, caster, damage, 50, 0, 0, 0, 50);
                    m.Stam = Math.Max(0, m.Stam - damage);
                    m.FixedParticles(0x376A, 9, 32, 5008, EffectLayer.Waist);
                    m.PlaySound(0x2F3);
                }
            }
            eable.Free();
        }
    }

    public class DirgeOfDiscordSpell : CustomSpell
    {
        public DirgeOfDiscordSpell() : base("Dirge of Discord", 0x455)
        {
            AddLevel(SpellType.Bard, 3);
            AddTag(SpellTag.AoE);
            AddTag(SpellTag.Debuff);
        }

        public override void Cast(Mobile caster, int hue, int level)
        {
            Mobile target = caster.Combatant as Mobile;
            if (target == null)
                return;

            int dexLoss = 20 + level * 2;
            int strLoss = (int)(25 + level * 1.5);
            TimeSpan duration = TimeSpan.FromSeconds(30 + level);

            IPooledEnumerable eable = target.GetMobilesInRange(2);

            foreach (Mobile m in eable)
            {
                if (!m.Alive || !caster.CanBeHarmful(m))
                    continue;

                m.AddStatMod(new StatMod(StatType.Dex, "DirgeDex", -dexLoss, duration));
                m.AddStatMod(new StatMod(StatType.Str, "DirgeStr", -strLoss, duration));

                m.FixedEffect(0x376A, 10, 25);
                m.PlaySound(0x204);
            }
            eable.Free();
        }
    }

    public class DissonantChordSpell : CustomSpell
    {
        public DissonantChordSpell() : base("Dissonant Chord", 0x481)
        {
            AddLevel(SpellType.Bard, 3);
            AddTag(SpellTag.AoE);
            AddTag(SpellTag.Offensive);
        }

        public override void Cast(Mobile caster, int hue, int level)
        {
            IPooledEnumerable eable = caster.GetMobilesInRange(2);

            foreach (Mobile m in eable)
            {
                if (!m.Alive || !caster.CanBeHarmful(m))
                    continue;

                int damage = Utility.RandomMinMax(20, 25) + level;
                AOS.Damage(m, caster, damage, 0, 0, 0, 100, 0);

                m.FixedEffect(0x36BD, 10, 25);
                m.PlaySound(0x1F2);
            }

            eable.Free();
        }
    }



    public class FireballSpell : CustomSpell
    {
        public FireballSpell() : base("Fireball", 0x36D4)
        {
            AddLevel(SpellType.Wizard, 3);
            AddLevel(SpellType.Sorcerer, 3);
            AddTag(SpellTag.AoE);
            AddTag(SpellTag.Offensive);
        }

        public override void Cast(Mobile caster, int hue, int level)
        {
            Mobile target = caster.Combatant as Mobile;
            if (target == null || target.Deleted || !target.Alive)
                return;

            SpellHelpers.CreateExplosion(target.Location, target.Map, 0x36BD, hue != 0 ? hue : 1160);
            List<Mobile> targets = SpellHelpers.GetMobilesInRange(target, caster, 1);
            foreach (Mobile m in targets)
                AOS.Damage(m, caster, Utility.RandomMinMax(16, 25)+level, 0, 100, 0, 0, 0);
        }
    }

    public class GoodHopeSpell : CustomSpell
    {
        public GoodHopeSpell() : base("Good Hope", 0x480)
        {
            AddLevel(SpellType.Bard, 3);
            AddTag(SpellTag.AoE);
            AddTag(SpellTag.Buff);
        }

        public override void Cast(Mobile caster, int hue, int level)
        {
            int statBonus = 15 + level;
            int skillBonus = 10 + level;
            TimeSpan duration = TimeSpan.FromSeconds(30 + level * 2);

            IPooledEnumerable eable = caster.GetMobilesInRange(3);

            foreach (Mobile m in eable)
            {
                if (!m.Alive || !BardSpellHelpers.IsFriendly(caster, m))
                    continue;

                BardSpellHelpers.ApplyStatBuff(m, statBonus, statBonus, statBonus, duration, "GoodHope");

                m.AddSkillMod(new DefaultSkillMod(SkillName.Tactics, true, skillBonus));
                m.AddSkillMod(new DefaultSkillMod(SkillName.Parry, true, skillBonus));

                m.FixedParticles(0x373A, 10, 15, 5012, hue, 0, EffectLayer.Waist);
                m.PlaySound(0x1ED);
            }

            eable.Free();
        }
    }

    public class HarmonicChorusSpell : CustomSpell
    {
        public HarmonicChorusSpell() : base("Harmonic Chorus", 0x480)
        {
            AddLevel(SpellType.Bard, 3);
            AddTag(SpellTag.AoE);
            AddTag(SpellTag.Buff);
        }

        public override void Cast(Mobile caster, int hue, int level)
        {
            int skillBonus = 10 + level;
            TimeSpan duration = TimeSpan.FromSeconds(30 + level * 2);

            IPooledEnumerable eable = caster.GetMobilesInRange(2);

            foreach (Mobile m in eable)
            {
                if (!m.Alive || !BardSpellHelpers.IsFriendly(caster, m))
                    continue;

                m.AddStatMod(new StatMod(StatType.Int, "ChorusInt", 25, duration));

                m.AddSkillMod(new DefaultSkillMod(SkillName.Meditation, true, skillBonus));
                m.AddSkillMod(new DefaultSkillMod(SkillName.Focus, true, skillBonus));

                m.FixedParticles(0x373A, 10, 15, 5012, hue, 0, EffectLayer.Waist);
            }
            eable.Free();
        }
    }



    public class LightningBoltSpell : CustomSpell
    {
        public LightningBoltSpell() : base("Lightning Bolt", 0x29)
        {
            AddLevel(SpellType.Wizard, 3);
            AddLevel(SpellType.Sorcerer, 3);
            AddTag(SpellTag.AoE);
            AddTag(SpellTag.Offensive);
        }   

        public override void Cast(Mobile caster, int hue, int level)
        {
            Mobile target = caster.Combatant as Mobile;
            if (target == null)
                return; 

            Direction d = caster.GetDirectionTo(target);
            int dx = 0;
            int dy = 0; 

            switch (d & Direction.Mask)
            {
                case Direction.North: dy = -1; break;
                case Direction.South: dy = 1; break;
                case Direction.West: dx = -1; break;
                case Direction.East: dx = 1; break;
                case Direction.Up: dx = -1; dy = -1; break;
                case Direction.Down: dx = 1; dy = 1; break;
                case Direction.Left: dx = -1; dy = 1; break;
                case Direction.Right: dx = 1; dy = -1; break;
            }   

            Point3D p = caster.Location;
            Map map = caster.Map;   

            for (int i = 0; i < 6; i++)
            {
                p = new Point3D(p.X + dx, p.Y + dy, p.Z);   

                Effects.SendLocationEffect(p, map, 0x29, 10, 10, hue != 0 ? hue : 0x480, 0);    

                IPooledEnumerable eable = map.GetMobilesInRange(p, 0);
                foreach (Mobile m in eable)
                {
                    if (!m.Alive || !caster.CanBeHarmful(m))
                        continue;   

                    caster.DoHarmful(m);    

                    int dmg = Utility.RandomMinMax(20, 25) + level;
                    AOS.Damage(m, caster, dmg, 0, 0, 0, 100, 0);
                }
                eable.Free();
            }   

            caster.PlaySound(0x29);
        }
    }



    public class MassLesserVigorSpell : CustomSpell
    {
        public MassLesserVigorSpell() : base("Mass Vigor, Lesser", 0x376A)
        {
            AddLevel(SpellType.Cleric, 3);
            AddLevel(SpellType.Druid, 3);
            AddTag(SpellTag.AoE);
            AddTag(SpellTag.Heal);
            AddTag(SpellTag.HoT);
        }

        public override void Cast(Mobile caster, int hue, int level)
        {
            int duration = 6 + (level / 2);

            foreach (Mobile m in caster.GetMobilesInRange(2))
            {
                if (m.Alive && caster.CanBeBeneficial(m))
                {
                    NatureSpellHelper.ApplyVigor(m, 3, duration, level);
                }
            }

            caster.PlaySound(0x202);
        }
    }


    public class PrayerSpell : CustomSpell
    {
        public PrayerSpell() : base("Prayer", 0x376A)
        {
            AddLevel(SpellType.Cleric, 3);
            AddTag(SpellTag.AoE);
            AddTag(SpellTag.Buff);
            AddTag(SpellTag.Debuff);
        }

        public override void Cast(Mobile caster, int hue, int level)
        {
            int effect = 25 + level;
            TimeSpan duration = TimeSpan.FromSeconds(20 + level);
            caster.FixedParticles(0x375A, 10, 30, 5052, hue != 0 ? hue : 0x47D, 0, EffectLayer.Waist);

            IPooledEnumerable eable = caster.GetMobilesInRange(3);
            foreach (Mobile m in eable)
            {
                if (!m.Alive)
                    continue;

                if (m == caster || !caster.CanBeHarmful(m))
                {
                    m.AddStatMod(new StatMod(StatType.Str, "PrayerStr", effect, duration));
                    m.AddStatMod(new StatMod(StatType.Dex, "PrayerDex", effect, duration));
                    m.FixedEffect(0x376A, 10, 32, hue != 0 ? hue : 0x47D, 0);
                }
                else
                {
                    m.AddStatMod(new StatMod(StatType.Str, "PrayerStrDebuff", -effect, duration));
                    m.AddStatMod(new StatMod(StatType.Dex, "PrayerDexDebuff", -effect, duration));
                    m.FixedEffect(0x374A, 10, 32, hue != 0 ? hue : 0x455, 0);
                }
            }
            eable.Free();
            caster.PlaySound(0x1F2);
        }
    }

    public abstract class ProtectionFromEnergySpell : CustomSpell
    {
        private ResistanceType m_ResistType;
        private EnergyProtectionType m_Type;

        protected ProtectionFromEnergySpell(
            string name,
            int hue,
            ResistanceType resistType,
            EnergyProtectionType type
        ) : base(name, hue)
        {
            m_ResistType = resistType;
            m_Type = type;

            AddTag(SpellTag.Buff);
            AddTag(SpellTag.SingleTarget);

            AddLevel(SpellType.Cleric, 3);
            AddLevel(SpellType.Druid, 3);
            AddLevel(SpellType.Wizard, 3);
            AddLevel(SpellType.Sorcerer, 3);
        }

        public override void Cast(Mobile caster, int hue, int level)
        {
            int amount = 12 + level;

            if (GetResist(caster) >= 70)
            {
                return;
            }

            if (GetResist(caster) + amount > 70)
                amount = 70 - GetResist(caster);

            TimeSpan duration = TimeSpan.FromSeconds(40 + level * 2);

            ResistanceMod mod = new ResistanceMod(m_ResistType, amount);

            caster.AddResistanceMod(mod);

            caster.FixedParticles(0x373A, 10, 15, 5012, hue, 3, EffectLayer.Waist);
            caster.PlaySound(0x1EA);

            new ExpireTimer(caster, mod, duration, m_Type).Start();
        }

        private int GetResist(Mobile m)
        {
            switch (m_ResistType)
            {
                case ResistanceType.Cold: return m.ColdResistance;
                case ResistanceType.Fire: return m.FireResistance;
                case ResistanceType.Energy: return m.EnergyResistance;
                case ResistanceType.Poison: return m.PoisonResistance;
            }
            return 0;
        }

        private class ExpireTimer : Timer
        {
            private Mobile m_Mobile;
            private ResistanceMod m_Mod;
            private EnergyProtectionType m_Type;

            public ExpireTimer(Mobile m, ResistanceMod mod, TimeSpan delay, EnergyProtectionType type)
                : base(delay)
            {
                m_Mobile = m;
                m_Mod = mod;
                m_Type = type;
            }

            protected override void OnTick()
            {
                if (m_Mobile == null)
                    return;

                m_Mobile.RemoveResistanceMod(m_Mod);

                switch (m_Type)
                {
                    case EnergyProtectionType.Cold:
                        m_Mobile.SendMessage("Your protection from cold fades.");
                        break;
                    case EnergyProtectionType.Fire:
                        m_Mobile.SendMessage("Your protection from fire fades.");
                        break;
                    case EnergyProtectionType.Energy:
                        m_Mobile.SendMessage("Your protection from electricity fades.");
                        break;
                    case EnergyProtectionType.Poison:
                        m_Mobile.SendMessage("Your protection from acid fades.");
                        break;
                }

                Stop();
            }
        }
    }

    public class ProtectionFromColdSpell : ProtectionFromEnergySpell
    {
        public ProtectionFromColdSpell()
            : base("Protection from Energy: Cold", 0x480, ResistanceType.Cold, EnergyProtectionType.Cold)
        {
        }
    }

    public class ProtectionFromFireSpell : ProtectionFromEnergySpell
    {
        public ProtectionFromFireSpell()
            : base("Protection from Energy: Fire", 0x489, ResistanceType.Fire, EnergyProtectionType.Fire)
        {
        }
    }

    public class ProtectionFromElectricitySpell : ProtectionFromEnergySpell
    {
        public ProtectionFromElectricitySpell()
            : base("Protection from Energy: Electricity", 0x4A0, ResistanceType.Energy, EnergyProtectionType.Energy)
        {
        }
    }
    public class ProtectionFromAcidSpell : ProtectionFromEnergySpell
    {
        public ProtectionFromAcidSpell()
            : base("Protection from Energy: Acid", 0x455, ResistanceType.Poison, EnergyProtectionType.Poison)
        {
        }
    }



    
    public class SpikeGrowthSpell : CustomSpell
    {
        public SpikeGrowthSpell() : base("Spike Growth", 0x36BD)
        {
            AddLevel(SpellType.Druid, 3);
            AddTag(SpellTag.AoE);
            AddTag(SpellTag.Offensive);
            AddTag(SpellTag.Debuff);
        }   

        public override void Cast(Mobile caster, int hue, int level)
        {
            Mobile target = caster.Combatant as Mobile;
            if (target == null)
                return; 

            Point3D center = target.Location;
            Map map = target.Map;   

            Effects.SendLocationEffect(center, map, 0x36BD, 20, 10, 0x59B, 0);  

            int duration = level + 1;   

            for (int i = 0; i < duration; i++)
            {
                Timer.DelayCall(TimeSpan.FromSeconds(i * 2), delegate
                {
                    IPooledEnumerable eable = caster.GetMobilesInRange(2);

                    foreach (Mobile m in eable)
                    {
                        if (!m.Alive || !caster.CanBeHarmful(m))
                            continue;   

                        int dmg = Utility.RandomMinMax(20, 25) + level;  

                        AOS.Damage(m, caster, dmg, 100, 0, 0, 0, 0);
                        m.Stam = Math.Max(0, m.Stam - dmg); 
                        m.FixedEffect(0x36BD, 10, 10);
                    }   
                    eable.Free();
                });
            }
        }
    }

    public class SummonNatureAllyIIISpell : SummonNatureAllySpell
    {
        public SummonNatureAllyIIISpell()
            : base("Summon Nature's Ally III", 0x2156, 3, typeof(WolfDire))
        {
        }
    }
    public class VigorSpell : CustomSpell
    {
        public VigorSpell() : base("Vigor", 0x376A)
        {
            AddLevel(SpellType.Cleric, 3);
            AddLevel(SpellType.Druid, 3);
            AddTag(SpellTag.Heal);
            AddTag(SpellTag.HoT);
        }

        public override void Cast(Mobile caster, int hue, int level)
        {
            int duration = 9 + (level / 2);
            NatureSpellHelper.ApplyVigor(caster, 6, duration, level);
        }
    }


    // ===== LEVEL 4 =====
    public class FlameStrikeSpell : CustomSpell
    {
        public FlameStrikeSpell() : base("Flamestrike", 0x3709)
        {
            AddLevel(SpellType.Wizard, 4);
            AddLevel(SpellType.Sorcerer, 4);
            AddTag(SpellTag.SingleTarget);
            AddTag(SpellTag.Offensive);
        }

        public override void Cast(Mobile caster, int hue, int level)
        {
            Mobile target = caster.Combatant as Mobile;
            if (target == null || !caster.CanBeHarmful(target))
                return;

            caster.DoHarmful(target);

            SpellHelpers.CreateExplosion(
                target.Location,
                target.Map,
                0x3709,
                hue != 0 ? hue : 0x489
            );

            int damage = Utility.RandomMinMax(30, 35) + level;

            AOS.Damage(target, caster, damage, 0, 100, 0, 0, 0);

            target.SendMessage("You are hit by a pillar of flame!");
        }
    }

    public class IceStormSpell : CustomSpell
    {
        public IceStormSpell() : base("Ice Storm", 0x480)
        {
            AddLevel(SpellType.Druid, 4);
            AddLevel(SpellType.Wizard, 4);
            AddLevel(SpellType.Sorcerer, 4);

            AddTag(SpellTag.AoE);
            AddTag(SpellTag.Offensive);
        }

        public override void Cast(Mobile caster, int hue, int level)
        {
            Mobile target = caster.Combatant as Mobile;
            if (target == null || !caster.CanBeHarmful(target))
                return;

            Point3D loc = target.Location;

            IPooledEnumerable eable = caster.Map.GetMobilesInRange(loc, 3);

            foreach (Mobile m in eable)
            {
                if (m == caster)
                    continue;

                if (!caster.CanBeHarmful(m))
                    continue;

                int dmg = Utility.RandomMinMax(24, 29) + level;

                SpellHelper.Damage(
                    TimeSpan.Zero,
                    m,
                    caster,
                    dmg,
                    0, 0, 0, 100, 0
                );

                m.FixedParticles(0x374A, 10, 15, 5032, hue, 3, EffectLayer.Waist);
            }

            eable.Free();
        }

    }


    public abstract class BaseOrbSpell : CustomSpell
    {
        protected abstract int Phys { get; }
        protected abstract int Fire { get; }
        protected abstract int Cold { get; }
        protected abstract int Poison { get; }
        protected abstract int Energy { get; }
        protected abstract int Hue { get; }
        protected abstract string HitMessage { get; }

        public BaseOrbSpell(string name) : base(name, 0x36D4) { }

        public override void Cast(Mobile caster, int hue, int level)
        {
            Mobile target = caster.Combatant as Mobile;
            if (target == null || !caster.CanBeHarmful(target))
                return;

            caster.DoHarmful(target);

            caster.MovingParticles(
                target,
                0x36D4,
                7,
                0,
                false,
                true,
                Hue,
                0,
                9502,
                1,
                0,
                (EffectLayer)255,
                0
            );

            int damage = Utility.RandomMinMax(30, 35) + level;

            AOS.Damage(
                target,
                caster,
                damage,
                Phys, Fire, Cold, Poison, Energy
            );

            target.SendMessage(HitMessage);
        }
    }

    public class OrbOfAcidSpell : BaseOrbSpell
    {
        public OrbOfAcidSpell() : base("Orb of Acid")
        {
            AddLevel(SpellType.Wizard, 4);
            AddLevel(SpellType.Sorcerer, 4);
            AddTag(SpellTag.SingleTarget);
            AddTag(SpellTag.Offensive);
        }

        protected override int Phys { get { return 0; } }
        protected override int Fire { get { return 0; } }
        protected override int Cold { get { return 0; } }
        protected override int Poison { get { return 100; } }
        protected override int Energy { get { return 0; } }
        protected override int Hue { get { return 0x3F; } }
        protected override string HitMessage { get { return "Acid burns your flesh!"; } }
    }

    public class OrbOfColdSpell : BaseOrbSpell
    {
        public OrbOfColdSpell() : base("Orb of Cold")
        {
            AddLevel(SpellType.Wizard, 4);
            AddLevel(SpellType.Sorcerer, 4);
            AddTag(SpellTag.SingleTarget);
            AddTag(SpellTag.Offensive);
        }
    
        protected override int Phys { get { return 0; } }
        protected override int Fire { get { return 0; } }
        protected override int Cold { get { return 100; } }
        protected override int Poison { get { return 0; } }
        protected override int Energy { get { return 0; } }
        protected override int Hue { get { return 0x3F; } }
        protected override string HitMessage { get { return "Cold scars your flesh!"; } }
    }

    public class OrbOfElectricitySpell : BaseOrbSpell
    {
        public OrbOfElectricitySpell() : base("Orb of Electricity")
        {
            AddLevel(SpellType.Wizard, 4);
            AddLevel(SpellType.Sorcerer, 4);
            AddTag(SpellTag.SingleTarget);
            AddTag(SpellTag.Offensive);
        }
    
        protected override int Phys { get { return 0; } }
        protected override int Fire { get { return 0; } }
        protected override int Cold { get { return 0; } }
        protected override int Poison { get { return 0; } }
        protected override int Energy { get { return 100; } }
        protected override int Hue { get { return 0x3F; } }
        protected override string HitMessage { get { return "Electricity runs through your flesh!"; } }
    }

    public class OrbOfFireSpell : BaseOrbSpell
    {
        public OrbOfFireSpell() : base("Orb of Fire")
        {
            AddLevel(SpellType.Wizard, 4);
            AddLevel(SpellType.Sorcerer, 4);
            AddTag(SpellTag.SingleTarget);
            AddTag(SpellTag.Offensive);
        }
    
        protected override int Phys { get { return 0; } }
        protected override int Fire { get { return 100; } }
        protected override int Cold { get { return 0; } }
        protected override int Poison { get { return 0; } }
        protected override int Energy { get { return 0; } }
        protected override int Hue { get { return 0x3F; } }
        protected override string HitMessage { get { return "Fire gnaws at your flesh!"; } }
    }

    public class OrbOfForceSpell : BaseOrbSpell
    {
        public OrbOfForceSpell() : base("Orb of Force")
        {
            AddLevel(SpellType.Wizard, 4);
            AddLevel(SpellType.Sorcerer, 4);
            AddTag(SpellTag.SingleTarget);
            AddTag(SpellTag.Offensive);
        }
    
        protected override int Phys { get { return 100; } }
        protected override int Fire { get { return 0; } }
        protected override int Cold { get { return 0; } }
        protected override int Poison { get { return 0; } }
        protected override int Energy { get { return 0; } }
        protected override int Hue { get { return 0x3F; } }
        protected override string HitMessage { get { return "A powerful impact hits you!"; } }
    }
    
    public class ShoutSpell : CustomSpell
    {
        public ShoutSpell() : base("Shout", 0x375A)
        {
            AddLevel(SpellType.Wizard, 4);
            AddLevel(SpellType.Sorcerer, 4);
            AddLevel(SpellType.Bard, 4);
            AddTag(SpellTag.AoE);
            AddTag(SpellTag.Offensive);
        }

        public override void Cast(Mobile caster, int hue, int level)
        {
            SpellHelpers.DoConeDamage(
                caster,
                6,
                24, 29,
                level,
                30, 0, 0, 0, 70,
                0x375A,
                hue != 0 ? hue : 0x480,
                "You hear a thunderous shout!"
            );

            caster.PlaySound(0x2A1);
        }
    }

    public class StoneskinSpell : CustomSpell
    {
        public StoneskinSpell() : base("Stoneskin", 0x375A)
        {
            AddLevel(SpellType.Wizard, 4);
            AddLevel(SpellType.Sorcerer, 4);
            AddLevel(SpellType.Druid, 5);
            AddTag(SpellTag.SingleTarget);
            AddTag(SpellTag.Buff);
        }

        public override void Cast(Mobile caster, int hue, int level)
        {
            int bonus = 25 * level;

            caster.Hits += bonus;

            caster.FixedEffect(0x375A, 10, 30);
            caster.PlaySound(0x1E9);

        }
    }
    public class SummonNatureAllyIVSpell : SummonNatureAllySpell
    {
        public SummonNatureAllyIVSpell()
            : base("Summon Nature's Ally IV", 0x2156, 4, typeof(FireSalamander))
        {
        }
    }

    public class ValiantSpiritSpell : CustomSpell
    {
        public ValiantSpiritSpell() : base("Valiant Spirit", 0x480)
        {
            AddLevel(SpellType.Bard, 4);
            AddLevel(SpellType.Cleric, 4);
            AddTag(SpellTag.SingleTarget);
            AddTag(SpellTag.Buff);
            AddTag(SpellTag.Heal);
        }

        public override void Cast(Mobile caster, int hue, int level)
        {
            int heal = Utility.RandomMinMax(20, 32) + level;
            int str = 5 + level;
            int resist = Math.Min(70, 6 + level);

            caster.Hits += heal;
            caster.AddStatMod(new StatMod(StatType.Str, "ValiantStr", str, TimeSpan.FromSeconds(60 + level * 10)));

            TimeSpan dur = TimeSpan.FromSeconds(60 + level * 10);
            caster.AddResistanceMod(new ResistanceMod(ResistanceType.Fire, resist));
            caster.AddResistanceMod(new ResistanceMod(ResistanceType.Cold, resist));
            caster.AddResistanceMod(new ResistanceMod(ResistanceType.Energy, resist));
            caster.AddResistanceMod(new ResistanceMod(ResistanceType.Poison, resist));
            caster.AddResistanceMod(new ResistanceMod(ResistanceType.Physical, resist));

            caster.FixedParticles(0x375A, 10, 15, 5012, hue, 0, EffectLayer.Waist);
            caster.PlaySound(0x1F7);
        }
    }


    // ===== LEVEL 5 =====
    public class CallLightningStormSpell : CustomSpell
    {
        public CallLightningStormSpell() : base("Call Lightning Storm", 0x379F)
        {
            AddLevel(SpellType.Druid, 5);
            AddTag(SpellTag.AoE);
            AddTag(SpellTag.Offensive);
            AddTag(SpellTag.DoT);
        }

        public override void Cast(Mobile caster, int hue, int level)
        {
            TimeSpan duration = TimeSpan.FromSeconds(12 + level);
            DateTime end = DateTime.UtcNow + duration;

            caster.PublicOverheadMessage(
                Server.Network.MessageType.Emote,
                0x3B2,
                false,
                "*dark clouds gather overhead*"
            );

            Timer.DelayCall(
                TimeSpan.Zero,
                TimeSpan.FromSeconds(3),
                delegate
                {
                    if (caster.Deleted || !caster.Alive || DateTime.UtcNow > end)
                        return;

                    Mobile target = SpellHelpers.FindNearbyEnemy(caster, 6);

                    if (target == null)
                        return;

                    caster.DoHarmful(target);

                    target.BoltEffect(hue != 0 ? hue : 0);
                    target.PlaySound(0x29);

                    int dmg = Utility.RandomMinMax(24, 29) + level;
                    AOS.Damage(target, caster, dmg, 0, 0, 0, 0, 100);

                    target.SendMessage("A bolt of lightning crashes down on you!");
                }
            );
        }
    }

    public class CureSeriousWoundsSpell : CustomSpell
    {
        public CureSeriousWoundsSpell() : base("Cure Serious Wounds", 0x376A)
        {
            AddLevel(SpellType.Cleric, 5);
            AddLevel(SpellType.Druid, 5);
            AddLevel(SpellType.Bard, 6);
            AddTag(SpellTag.SingleTarget);
            AddTag(SpellTag.Heal);
        }

        public override void Cast(Mobile caster, int hue, int level)
        {
            int heal = Utility.RandomMinMax(12, 22) + level;

            caster.Heal(heal);

            caster.FixedParticles(0x376A, 9, 32, 5030, hue, 0, EffectLayer.Waist);
            caster.PlaySound(0x202);

        }
    }

    public class SlayLivingSpell : CustomSpell
    {
        public SlayLivingSpell() : base("Slay Living", 0x22C5)
        {
            AddLevel(SpellType.Cleric, 5);
            AddTag(SpellTag.SingleTarget);
            AddTag(SpellTag.Offensive);
        }

        public override void Cast(Mobile caster, int hue, int level)
        {
            Mobile target = caster.Combatant as Mobile;
            if (target == null || !target.Alive || !caster.CanBeHarmful(target))
                return;

            SlayerEntry silver = SlayerGroup.GetEntryByName(SlayerName.Silver);
            SlayerEntry exorcism = SlayerGroup.GetEntryByName(SlayerName.Exorcism);

            if (target is BaseCreature &&
                ((silver != null && silver.Slays(target)) ||
                 (exorcism != null && exorcism.Slays(target))))
            {
                return;
            }

            caster.DoHarmful(target);

            target.FixedEffect(0x3709, 10, 30);
            target.PlaySound(0x211);

            double resist = target.Skills[SkillName.MagicResist].Value;
            int roll = Utility.Random(40 + level * 2);

            if (roll > resist)
            {
                target.SendMessage("Your life force is ripped from your body!");
                target.Kill();
            }
            else
            {
                int damage = Utility.RandomMinMax(17, 22) + level;
                AOS.Damage(target, caster, damage, 100, 0, 0, 0, 0);

                target.SendMessage("You resist death but suffer terrible pain!");
                target.PlaySound(0x1F2);
            }
        }
    }
    public class SummonNatureAllyVSpell : SummonNatureAllySpell
    {
        public SummonNatureAllyVSpell()
            : base("Summon Nature's Ally V", 0x2156, 5, typeof(AnyElemental))
        {
        }
    }
    public class GreaterVigorSpell : CustomSpell
    {
        public GreaterVigorSpell() : base("Vigor, Greater", 0x376A)
        {
            AddLevel(SpellType.Cleric, 5);
            AddLevel(SpellType.Druid, 5);
            AddTag(SpellTag.SingleTarget);
            AddTag(SpellTag.Heal);
        }
    
        public override void Cast(Mobile caster, int hue, int level)
        {
            int duration = 12 + (level / 2);
            NatureSpellHelper.ApplyVigor(caster, 12, duration, level);
        }
    }

    public class WailOfDoomSpell : CustomSpell
    {
        public WailOfDoomSpell() : base("Wail of Doom", 0x482)
        {
            AddLevel(SpellType.Bard, 5);
            AddTag(SpellTag.AoE);
            AddTag(SpellTag.Offensive);
            AddTag(SpellTag.CC);
        }

        public override void Cast(Mobile caster, int hue, int level)
        {
            int range = 4;
            int angle = 60; 
            Direction dir = caster.Direction;

            foreach (Mobile m in caster.GetMobilesInRange(range))
            {
                if (!m.Alive || !caster.CanBeHarmful(m))
                    continue;

                if (!InCone(caster, m, dir, angle))
                    continue;

                caster.DoHarmful(m);

                int damage = Utility.RandomMinMax(35, 40) + level;
                AOS.Damage(m, caster, damage, 0, 0, 0, 100, 0);

                int save = Utility.RandomMinMax(40, 52) + level * 2;
                if (m.Int < save)
                {
                    m.Paralyze(TimeSpan.FromSeconds(6 + level));
                }

                m.FixedEffect(0x36BD, 10, 25);
                m.PlaySound(0x204);
            }
        }

        private bool InCone(Mobile caster, Mobile target, Direction dir, int angle)
        {
            double dx = target.X - caster.X;
            double dy = target.Y - caster.Y;
            double dist = Math.Sqrt(dx * dx + dy * dy);

            if (dist == 0)
                return true;

            double targetAngle = Math.Atan2(dy, dx) * 180 / Math.PI;
            double facingAngle = GetFacingAngle(dir);

            double diff = Math.Abs(NormalizeAngle(targetAngle - facingAngle));
            return diff <= angle / 2;
        }

        private double GetFacingAngle(Direction dir)
        {
            switch (dir & Direction.Mask)
            {
                case Direction.North: return -90;
                case Direction.Right: return 0;
                case Direction.East: return 0;
                case Direction.Down: return 90;
                case Direction.South: return 90;
                case Direction.Left: return 180;
                case Direction.West: return 180;
                case Direction.Up: return -90;
            }

            return 0;
        }

        private double NormalizeAngle(double angle)
        {
            while (angle < -180) angle += 360;
            while (angle > 180) angle -= 360;
            return angle;
        }
    }



    // ===== LEVEL 6 =====
    public class AcidFogSpell : CustomSpell
    {
        public AcidFogSpell() : base("Acid Fog", 0x36D4)
        {
            AddLevel(SpellType.Wizard, 6);
            AddLevel(SpellType.Sorcerer, 6);
            AddTag(SpellTag.AoE);
            AddTag(SpellTag.Offensive);
            AddTag(SpellTag.DoT);
        }

        public override void Cast(Mobile caster, int hue, int level)
        {
            Mobile target = caster.Combatant as Mobile;
            if (target == null)
            {
                return;
            }

            caster.PlaySound(0x231);
    
            FieldSpellHelper.SpawnField(
                caster,
                target.Location,
                target.Map,
                6,
                1,
                delegate { return new AcidFogTile(caster, level); }
            );
        }
    }

    public class AcidFogTile : Item
    {
        private Mobile m_Caster;
        private int m_Level;

        public AcidFogTile(Mobile caster, int level) : base(0x398C)
        {
            Movable = false;
            Hue = 0x48E;

            m_Caster = caster;
            m_Level = level;

            Timer.DelayCall(TimeSpan.Zero, TimeSpan.FromSeconds(1.0), Tick);
            Timer.DelayCall(TimeSpan.FromSeconds(2 + level), Delete);
        }

        private void Tick()
        {
            if (Deleted)
                return;

            IPooledEnumerable eable = GetMobilesInRange(0);

            foreach (Mobile m in eable)
            {
                if (!m.Alive || !m_Caster.CanBeHarmful(m))
                    continue;

                m_Caster.DoHarmful(m);

                int dmg = Utility.RandomMinMax(28, 33) + m_Level;
                AOS.Damage(m, m_Caster, dmg, 0, 0, 0, 100, 0);

                m.FixedEffect(0x36BD, 10, 20);
                m.SendMessage("The acid burns your flesh!");
            }
            eable.Free();
        }

        public AcidFogTile(Serial serial) : base(serial) { }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt();
            Delete();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }
    }

    public class MassBearsEnduranceSpell : CustomSpell
    {
        public MassBearsEnduranceSpell() : base("Mass Bear's Endurance", 0x480)
        {
            AddLevel(SpellType.Wizard, 6);
            AddLevel(SpellType.Sorcerer, 6);
            AddLevel(SpellType.Druid, 6);
            AddLevel(SpellType.Cleric, 6);

            AddTag(SpellTag.Buff);
            AddTag(SpellTag.AoE);
        }

        public override void Cast(Mobile caster, int hue, int level)
        {
            IPooledEnumerable eable = caster.GetMobilesInRange(6);
            foreach (Mobile m in eable)
            {
                if (!m.Alive || !caster.CanBeBeneficial(m))
                    continue;

                caster.DoBeneficial(m);

                int bonusHP = 45 + level * 2;
                int strBonus = bonusHP / 2;
                if (strBonus < 1)
                    strBonus = 1;

                m.AddStatMod(new StatMod(StatType.Str, "MassBearsEndurance", strBonus, TimeSpan.FromSeconds(60)));
                m.Hits += bonusHP;

                m.FixedEffect(0x376A, 10, 20);
                m.PlaySound(0x1EA);
            }
            eable.Free();
        }
    }

    public class GreaterBestowCurseSpell : CustomSpell
    {
        public GreaterBestowCurseSpell() : base("Bestow Curse, Greater", 0x455)
        {
            AddLevel(SpellType.Bard, 6);
            AddLevel(SpellType.Cleric, 7);
            AddLevel(SpellType.Wizard, 8);
            AddLevel(SpellType.Sorcerer, 8);
            AddTag(SpellTag.Debuff);
            AddTag(SpellTag.SingleTarget);
        }

        public override void Cast(Mobile caster, int hue, int level)
        {
            Mobile target = caster.Combatant as Mobile;
            if (target == null || !caster.CanBeHarmful(target))
                return;

            caster.DoHarmful(target);

            int strLoss = (int)(target.RawStr * (0.24 + level * 0.02));
            TimeSpan dur = TimeSpan.FromSeconds(30 + level);

            target.AddStatMod(new StatMod(StatType.Str, "GreaterBestowCurse", -strLoss, dur));

            target.FixedEffect(0x3728, 10, 20);
            target.PlaySound(0x1F8);
        }
    }


    public class BullsStrengthMassSpell : CustomSpell
    {
        public BullsStrengthMassSpell() : base("Bull's Strength, Mass", 0x375A)
        {
            AddLevel(SpellType.Cleric, 6);
            AddLevel(SpellType.Druid, 6);
            AddLevel(SpellType.Wizard, 6);
            AddLevel(SpellType.Sorcerer, 6);
            AddTag(SpellTag.AoE);
            AddTag(SpellTag.Buff);
        }

        public override void Cast(Mobile caster, int hue, int level)
        {
            int bonus = 30 + level * 2;

            caster.PlaySound(0x1E9);

            MassBuffHelper.ApplyStatBuff(
                caster,
                6,
                bonus,
                TimeSpan.FromMinutes(1),
                delegate(Mobile m, int amt) { m.RawStr += amt; },
                delegate(Mobile m, int amt) { m.RawStr -= amt; },
                0x375A,
                hue,
                "Your muscles surge with power!"
            );
        }
    }

    public class CacophonicShieldSpell : CustomSpell
    {
        public CacophonicShieldSpell() : base("Cacophonic Shield", 0x481)
        {
            AddLevel(SpellType.Bard, 6);
            AddLevel(SpellType.Wizard, 7);
            AddLevel(SpellType.Sorcerer, 7);
            AddTag(SpellTag.AoE);
            AddTag(SpellTag.Buff);
            AddTag(SpellTag.DoT);
        }

        public override void Cast(Mobile caster, int hue, int level)
        {
            int resist = 6 + level;
            TimeSpan dur = TimeSpan.FromSeconds(21 + level);

            caster.AddResistanceMod(new ResistanceMod(ResistanceType.Energy, resist));

            int ticks = (int)(dur.TotalSeconds / 2);

            Timer.DelayCall(
                TimeSpan.Zero,
                TimeSpan.FromSeconds(2),
                ticks,
                delegate
                {
                    IPooledEnumerable eable = caster.GetMobilesInRange(1);

                    foreach (Mobile m in eable)
                    {
                        if (!m.Alive || !caster.CanBeHarmful(m))
                            continue;

                        int dmg = Utility.RandomMinMax(28, 33);
                        AOS.Damage(m, caster, dmg, 0, 0, 0, 100, 0);
                    }

                    eable.Free();
                }
            );


            caster.FixedParticles(0x375A, 10, 15, 5012, hue, 0, EffectLayer.Waist);
        }
    }


    public class CatsGraceMassSpell : CustomSpell
    {
        public CatsGraceMassSpell() : base("Cat's Grace, Mass", 0x375A)
        {
            AddLevel(SpellType.Bard, 6);
            AddLevel(SpellType.Druid, 6);
            AddLevel(SpellType.Wizard, 6);
            AddLevel(SpellType.Sorcerer, 6);
            AddTag(SpellTag.AoE);
            AddTag(SpellTag.Buff);
        }

        public override void Cast(Mobile caster, int hue, int level)
        {
            int bonus = 30 + level * 2;

            caster.PlaySound(0x1E9);
        
            MassBuffHelper.ApplyStatBuff(
                caster,
                6,
                bonus,
                TimeSpan.FromMinutes(1),
                delegate(Mobile m, int amt) { m.RawDex += amt; },
                delegate(Mobile m, int amt) { m.RawDex -= amt; },
                0x375A,
                hue,
                "Your movements become swift and precise!"
            );
        }
    }

    public class ChainLightningSpell : CustomSpell
    {
        public ChainLightningSpell() : base("Chain Lightning", 0x379F)
        {
            AddLevel(SpellType.Wizard, 6);
            AddLevel(SpellType.Sorcerer, 6);
            AddTag(SpellTag.AoE);
            AddTag(SpellTag.Offensive);
        }

        public override void Cast(Mobile caster, int hue, int level)
        {
            caster.PlaySound(0x29);
        
            IPooledEnumerable eable = caster.GetMobilesInRange(3);

            foreach (Mobile m in eable)
            {
                if (m == caster || !m.Alive || !caster.CanBeHarmful(m))
                    continue;

                caster.DoHarmful(m);

                m.BoltEffect(hue);
                int dmg = Utility.RandomMinMax(32, 37) + level;

                AOS.Damage(m, caster, dmg, 0, 0, 0, 0, 100);
                m.SendMessage("Electricity surges through your body!");
            }
            eable.Free();
        }
    }

    public class CometfallSpell : CustomSpell
    {
        public CometfallSpell() : base("Cometfall", 0x489)
        {
            AddLevel(SpellType.Cleric, 6);
            AddLevel(SpellType.Druid, 6);

            AddTag(SpellTag.SingleTarget);
            AddTag(SpellTag.Offensive);
            AddTag(SpellTag.CC);
        }

        public override void Cast(Mobile caster, int hue, int level)
        {
            Mobile target = caster.Combatant as Mobile;
            if (target == null || !caster.CanBeHarmful(target))
                return;

            int dmg = Utility.RandomMinMax(30, 35) + level;

            SpellHelper.Damage(
                TimeSpan.Zero,
                target,
                caster,
                dmg,
                50, 0, 50, 0, 0
            );

            target.FixedParticles(0x36BD, 20, 10, 5044, hue, 3, EffectLayer.Head);
            target.PlaySound(0x160);

            if (target.Dex < Utility.RandomMinMax(20, 60) + level)
            {
                DoKnockback(target, caster, 3);
                target.Paralyze(TimeSpan.FromSeconds(4));
            }
        }

        private void DoKnockback(Mobile target, Mobile caster, int tiles)
        {
            Direction d = caster.GetDirectionTo(target);
            Point3D loc = target.Location;
            Map map = target.Map;

            for (int i = 0; i < tiles; i++)
            {
                Point3D next = GetOffset(loc, d);

                if (map == null || !map.CanFit(next, 16, false, false))
                    break;

                loc = next;
            }

            target.MoveToWorld(loc, map);
        }

        private Point3D GetOffset(Point3D p, Direction d)
        {
            switch (d)
            {
                case Direction.North: return new Point3D(p.X, p.Y - 1, p.Z);
                case Direction.South: return new Point3D(p.X, p.Y + 1, p.Z);
                case Direction.West:  return new Point3D(p.X - 1, p.Y, p.Z);
                case Direction.East:  return new Point3D(p.X + 1, p.Y, p.Z);
                case Direction.Up:    return new Point3D(p.X - 1, p.Y - 1, p.Z);
                case Direction.Down:  return new Point3D(p.X + 1, p.Y + 1, p.Z);
                case Direction.Left:  return new Point3D(p.X - 1, p.Y + 1, p.Z);
                case Direction.Right: return new Point3D(p.X + 1, p.Y - 1, p.Z);
            }

            return p;
        }
    }



    public class DisintegrateSpell : CustomSpell
    {
        public DisintegrateSpell() : base("Disintegrate", 0x36D4)
        {
            AddLevel(SpellType.Wizard, 6);
            AddLevel(SpellType.Sorcerer, 6);
            AddTag(SpellTag.SingleTarget);
            AddTag(SpellTag.Offensive);
        }

        public override void Cast(Mobile caster, int hue, int level)
        {
            Mobile target = caster.Combatant as Mobile;
            if (target == null || !target.Alive || !caster.CanBeHarmful(target))
            {
                return;
            }

            caster.DoHarmful(target);
        
            Effects.SendMovingEffect(
                caster, target, 0x36D4, 10, 0, false, false, hue != 0 ? hue : 0x47E, 0
            );

            bool weakened = target.Hits < (target.HitsMax / 2);

            int dmg = weakened
                ? Utility.RandomMinMax(70, 80) + level * 2
                : Utility.RandomMinMax(35, 40) + level * 2;

            AOS.Damage(target, caster, dmg, 100, 0, 0, 0, 0);

            target.FixedEffect(0x3709, 10, 30);
            target.PlaySound(0x211);
        }
    }

    public class GreaterShoutSpell : ShoutSpell
    {
        public GreaterShoutSpell() : base()
        {
            AddLevel(SpellType.Wizard, 8);
            AddLevel(SpellType.Sorcerer, 8);
            AddLevel(SpellType.Bard, 6);
            AddTag(SpellTag.AoE);
            AddTag(SpellTag.Offensive);
        }
    
        public override void Cast(Mobile caster, int hue, int level)
        {
            int range = 9;
            int min = 32;
            int max = 37;
           SpellHelpers.DoConeDamage(caster, range, min, max, level,50,0,0,0,50,0x375A,hue != 0 ? hue : 0x480, "A powerful scream pierces your ears!");
        }
    }

    public class MassVigorSpell : CustomSpell
    {
        public MassVigorSpell() : base("Mass Vigor", 0x376A)
        {
            AddLevel(SpellType.Cleric, 6);
            AddLevel(SpellType.Druid, 6);
            AddTag(SpellTag.AoE);
            AddTag(SpellTag.Heal);
            AddTag(SpellTag.HoT);
        }

        public override void Cast(Mobile caster, int hue, int level)
        {
            int duration = 9 + (level / 2);

            foreach (Mobile m in caster.GetMobilesInRange(2))
            {
                if (m.Alive && caster.CanBeBeneficial(m))
                {
                    NatureSpellHelper.ApplyVigor(m, 6, duration, level);
                }
            }

            caster.PlaySound(0x202);
        }
    }

        public class OwlsWisdomMassSpell : CustomSpell
        {
            public OwlsWisdomMassSpell() : base("Owl's Wisdom, Mass", 0x375A)
            {
                AddLevel(SpellType.Cleric, 6);
                AddLevel(SpellType.Druid, 6);
                AddLevel(SpellType.Wizard, 6);
                AddLevel(SpellType.Sorcerer, 6);
                AddTag(SpellTag.AoE);
                AddTag(SpellTag.Buff);
            }

            public override void Cast(Mobile caster, int hue, int level)
            {
                int bonus = 30 + level * 2;

                caster.PlaySound(0x1E9);

                MassBuffHelper.ApplyStatBuff(
                    caster,
                    6,
                    bonus,
                    TimeSpan.FromMinutes(1),
                    delegate(Mobile m, int amt) { m.RawInt += amt; },
                    delegate(Mobile m, int amt) { m.RawInt -= amt; },
                    0x375A,
                    hue,
                    "Your mind becomes clearer!"
                );
            }
        }


    public class HealSpell : CustomSpell
    {
        public HealSpell() : base("Heal", 0x376A)
        {
            AddLevel(SpellType.Cleric, 6);
            AddLevel(SpellType.Druid, 7);
            AddTag(SpellTag.SingleTarget);
            AddTag(SpellTag.Heal);
        }

        public override void Cast(Mobile caster, int hue, int level)
        {
            int heal = level * 12;

            caster.Heal(heal);
        
            caster.FixedParticles(0x376A, 9, 32, 5030, hue != 0 ? hue : 0x376A, 0, EffectLayer.Waist);
            caster.PlaySound(0x202);
        }
    }
    public class SummonNatureAllyVISpell : SummonNatureAllySpell
    {
        public SummonNatureAllyVISpell()
            : base("Summon Nature's Ally VI", 0x2156, 6, typeof(AnyGemElemental))
        {
        }
    }

    // ===== LEVEL 7 =====
    public class AuraOfColdGreaterSpell : AuraOfColdSpell
    {
        public AuraOfColdGreaterSpell()
            : base("Aura of Cold, Greater", 0x480, 32, 37)
        {
            AddLevel(SpellType.Cleric, 7);
            AddLevel(SpellType.Druid, 7);
        }
    }

    public class HoldPersonMassSpell : CustomSpell
    {
        public HoldPersonMassSpell() : base("Hold Person, Mass", 0x376A)
        {
            AddLevel(SpellType.Wizard, 7);
            AddLevel(SpellType.Sorcerer, 7);
            AddTag(SpellTag.AoE);
            AddTag(SpellTag.CC);
        }

        public override void Cast(Mobile caster, int hue, int level)
        {
            Mobile center = caster.Combatant as Mobile;
            if (center == null)
            {
                return;
            }

            caster.PlaySound(0x204);
            IPooledEnumerable eable = center.GetMobilesInRange(4);

            foreach (Mobile m in eable)
            {
                if (!m.Alive || !caster.CanBeHarmful(m))
                    continue;

                double resist = m.Skills[SkillName.MagicResist].Value;
                int duration = (int)(6 + level - resist / 10);

                if (duration > 0)
                {
                    m.Paralyze(TimeSpan.FromSeconds(duration));
                    m.SendMessage("You are frozen in place!");
                }
                else
                {
                    m.SendMessage("You resist the paralyzing magic!");
                }

                m.FixedEffect(0x376A, 10, 30, hue, 0);
                caster.DoHarmful(m);
            }

            eable.Free();
        }
    }

    public class FingerOfDeathSpell : CustomSpell
    {
        public FingerOfDeathSpell() : base("Finger of Death", 0x22C5)
        {
            AddLevel(SpellType.Druid, 8);
            AddLevel(SpellType.Sorcerer, 7);
            AddLevel(SpellType.Wizard, 7);
            AddTag(SpellTag.SingleTarget);
            AddTag(SpellTag.Offensive);
        }

        public override void Cast(Mobile caster, int hue, int level)
        {
            Mobile target = caster.Combatant as Mobile;
            if (target == null || !target.Alive || !caster.CanBeHarmful(target))
            {
                return;
            }

            if (DeathSpellHelper.IsUndead(target))
            {
                return;
            }

            caster.DoHarmful(target);
    
            int roll = Utility.Random(50 + level * 2);

            DeathSpellHelper.TryKill(
                caster,
                target,
                roll,
                32,
                37,
                level
            );
        }
    }

    public class FirestormSpell : CustomSpell
    {
        public FirestormSpell() : base("Firestorm", 0x489)
        {
            AddLevel(SpellType.Cleric, 8);
            AddLevel(SpellType.Druid, 7);

            AddTag(SpellTag.Offensive);
            AddTag(SpellTag.AoE);
            AddTag(SpellTag.DoT);
        }

        public override void Cast(Mobile caster, int hue, int level)
        {
            for (int i = 0; i < level; i++)
            {
                Direction dir = (Direction)Utility.Random(8);
                Point3D loc = caster.Location;

                for (int j = 0; j < 2; j++)
                {
                    loc = GetOffset(loc, dir);

                    FirestormField field = new FirestormField(loc, caster, level);
                    field.MoveToWorld(loc, caster.Map);
                }
            }
        }

        private Point3D GetOffset(Point3D p, Direction d)
        {
            switch (d)
            {
                case Direction.North: return new Point3D(p.X, p.Y - 1, p.Z);
                case Direction.South: return new Point3D(p.X, p.Y + 1, p.Z);
                case Direction.West:  return new Point3D(p.X - 1, p.Y, p.Z);
                case Direction.East:  return new Point3D(p.X + 1, p.Y, p.Z);
                case Direction.Up:    return new Point3D(p.X - 1, p.Y - 1, p.Z);
                case Direction.Down:  return new Point3D(p.X + 1, p.Y + 1, p.Z);
                case Direction.Left:  return new Point3D(p.X - 1, p.Y + 1, p.Z);
                case Direction.Right: return new Point3D(p.X + 1, p.Y - 1, p.Z);
            }

            return p;
        }

    }

    public class FirestormField : Item
    {
        private Timer m_Timer;
        private Mobile m_Caster;
        private int m_Level;

        public FirestormField(Point3D loc, Mobile caster, int level) : base(0x398C)
        {
            Movable = false;
            m_Caster = caster;
            m_Level = level;

            MoveToWorld(loc, caster.Map);

            m_Timer = Timer.DelayCall(
                TimeSpan.Zero,
                TimeSpan.FromSeconds(2),
                new TimerCallback(Tick)
            );

            Timer.DelayCall(TimeSpan.FromSeconds(12 + level), Delete);
        }

        public FirestormField(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version

            writer.Write(m_Caster);
            writer.Write(m_Level);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            m_Caster = reader.ReadMobile();
            m_Level = reader.ReadInt();

            Delete();
        }

        private void Tick()
        {
            if (Deleted || m_Caster == null)
                return;

            IPooledEnumerable eable = GetMobilesInRange(0);

            foreach (Mobile m in eable)
            {
                if (!m.Alive || !m_Caster.CanBeHarmful(m))
                    continue;

                int dmg = Utility.RandomMinMax(32, 37) + m_Level;

                SpellHelper.Damage(
                    TimeSpan.Zero,
                    m,
                    m_Caster,
                    dmg,
                    0, 100, 0, 0, 0
                );
            }

            eable.Free();
        }

        public override void OnDelete()
        {
            if (m_Timer != null)
                m_Timer.Stop();

            base.OnDelete();
        }
    }




    public class PlagueSpell : CustomSpell
    {
        public PlagueSpell() : base("Plague", 0x36BD)
        {
            AddLevel(SpellType.Cleric, 7);
            AddLevel(SpellType.Druid, 7);
            AddLevel(SpellType.Wizard, 8);
            AddLevel(SpellType.Sorcerer, 8);
            AddTag(SpellTag.AoE);
            AddTag(SpellTag.Offensive);
            AddTag(SpellTag.DoT);
            AddTag(SpellTag.Debuff);
        }

        public override void Cast(Mobile caster, int hue, int level)
        {
            Mobile center = caster.Combatant as Mobile;
            if (center == null)
            {
                return;
            }

            caster.PlaySound(0x205);
            IPooledEnumerable eable = center.GetMobilesInRange(4);

            foreach (Mobile m in eable)
            {
                if (!m.Alive || !caster.CanBeHarmful(m))
                    continue;

                m.ApplyPoison(caster, Poison.Deadly);

                int debuff = 12 + level * 3;
                m.AddStatMod(new StatMod(StatType.Str, "PlagueStr", -debuff, TimeSpan.FromSeconds(20 + level)));
                m.Stam = Math.Max(0, m.Stam - (40 + level * 3));

                m.FixedEffect(0x36BD, 10, 20);
                m.SendMessage("Your body weakens as disease ravages you!");

                caster.DoHarmful(m);
            }
            eable.Free();
        }
    }

    public class SummonNatureAllyVIISpell : SummonNatureAllySpell
    {
        public SummonNatureAllyVIISpell()
            : base("Summon Nature's Ally VII", 0x2156, 7, typeof(Tyranasaur))
        {
        }
    }


    public class SunbeamSpell : CustomSpell
    {
        public SunbeamSpell() : base("Sunbeam", 0x3709)
        {
            AddLevel(SpellType.Druid, 7);
            AddTag(SpellTag.SingleTarget);
            AddTag(SpellTag.Offensive);
        }

        public override void Cast(Mobile caster, int hue, int level)
        {
            Mobile target = caster.Combatant as Mobile;
            if (target == null)
            {
                return;
            }

            int beams = Math.Max(1, level / 3);

            caster.PlaySound(0x29);
            
            for (int i = 0; i < beams; i++)
            {
                if (!target.Alive || !caster.CanBeHarmful(target))
                    break;

                caster.DoHarmful(target);

                target.BoltEffect(hue != 0 ? hue : 0x480);

                int dmg = Utility.RandomMinMax(45, 50)+level;
                AOS.Damage(target, caster, dmg, 0, 100, 0, 0, 0);

                double resist = target.Skills[SkillName.MagicResist].Value;
                int para = (int)(2 + level - resist / 10);

                if (para > 0)
                    target.Paralyze(TimeSpan.FromSeconds(para));
            }
        }
    }
    // ===== LEVEL 8 =====
    public class BlackfireSpell : CustomSpell
    {
        public BlackfireSpell() : base("Blackfire", 0x36D4)
        {
            AddLevel(SpellType.Wizard, 8);
            AddLevel(SpellType.Sorcerer, 8);
            AddTag(SpellTag.SingleTarget);
            AddTag(SpellTag.Offensive);
            AddTag(SpellTag.Debuff);
        }

        public override void Cast(Mobile caster, int hue, int level)
        {
            Mobile target = caster.Combatant as Mobile;
            if (target == null || !caster.CanBeHarmful(target))
            {
                return;
            }

            caster.DoHarmful(target);
            target.SendMessage("Black flames burn into your soul!");

            caster.MovingParticles(target, 0x36D4, 7, 0, false, true,
                hue != 0 ? hue : 0x455, 0, 9502, 1, 0, (EffectLayer)255, 0);

            int dmg = Utility.RandomMinMax(50,55)+level;

            AOS.Damage(target, caster, dmg, 0, 0, 100, 0, 0);

            SpellHelpers.ApplyRepeatedStatDrain(
                target,
                StatType.Str,
                5,
                8,
                6,
                2,
                22 + level
            );

            target.PlaySound(0x208);
        }
    }

    public class EnervationSpell : CustomSpell
    {
        public EnervationSpell() : base("Enervation", 0x374A)
        {
            AddLevel(SpellType.Wizard, 8);
            AddLevel(SpellType.Sorcerer, 8);
            AddLevel(SpellType.Cleric, 8);
            AddTag(SpellTag.AoE);
            AddTag(SpellTag.Offensive);
            AddTag(SpellTag.Debuff);
        }

        public override void Cast(Mobile caster, int hue, int level)
        {
            Mobile target = caster.Combatant as Mobile;
            if (target == null)
                return;


            SpellHelpers.ForEachHostileInRange(target, caster, 1, m =>
            {
                caster.DoHarmful(m);

                int debuff = 32 + level * 2;
                int loss = 40 + level * 3;
                TimeSpan dur = TimeSpan.FromSeconds(42 + level * 2);

                m.AddStatMod(new StatMod(StatType.Str, "EnervationStr", -debuff, dur));
                m.Stam = Math.Max(0, m.Stam - loss);
                m.Mana = Math.Max(0, m.Mana - loss);

                m.FixedEffect(0x374A, 10, 20);
                m.PlaySound(0x1FB);
                m.SendMessage("Your strength and energy are drained!");
            });
        }
    }

    public class HorridWiltingSpell : CustomSpell
    {
        public HorridWiltingSpell() : base("Horrid Wilting", 0x3709)
        {
            AddLevel(SpellType.Wizard, 8);
            AddLevel(SpellType.Sorcerer, 8);
            AddTag(SpellTag.AoE);
            AddTag(SpellTag.Offensive);
        }

        public override void Cast(Mobile caster, int hue, int level)
        {
            Mobile target = caster.Combatant as Mobile;
            if (target == null)
                return;


            SpellHelpers.ForEachHostileInRange(target, caster, 3, m =>
            {
                int dmg = Utility.RandomMinMax(36, 39) + level;
                AOS.Damage(m, caster, dmg, 0, 50, 0, 50, 0);

                m.FixedEffect(0x3709, 10, 30);
                m.PlaySound(0x1FB);
                m.SendMessage("Your body shrivels from magical drought!");
            });
        }
    }

    public class SummonNatureAllyVIIISpell : SummonNatureAllySpell
    {
        public SummonNatureAllyVIIISpell()
            : base("Summon Nature's Ally VIII", 0x2156, 8, typeof(YoungRoc))
        {
        }
    }


    public class SunburstSpell : CustomSpell
    {
        public SunburstSpell() : base("Sunburst", 0x3709)
        {
            AddLevel(SpellType.Druid, 8);
            AddLevel(SpellType.Wizard, 8);
            AddLevel(SpellType.Sorcerer, 8);
            AddTag(SpellTag.AoE);
            AddTag(SpellTag.Offensive);
            AddTag(SpellTag.CC);
        }

        public override void Cast(Mobile caster, int hue, int level)
        {
            caster.PlaySound(0x29);

            SpellHelpers.ForEachHostileInRange(caster, caster, 4, m =>
            {
                caster.DoHarmful(m);

                int dmg = Utility.RandomMinMax(40, 45)+level;
                AOS.Damage(m, caster, dmg, 0, 100, 0, 0, 0);

                double resist = m.Skills[SkillName.MagicResist].Value;
                int para = (int)(8 + level - resist / 10);

                if (para > 0)
                    m.Paralyze(TimeSpan.FromSeconds(para));

                m.FixedEffect(0x3709, 10, 30);
                m.SendMessage("You are blinded by searing light!");
            });
        }
    }

    public class PolarRaySpell : CustomSpell
    {
        public PolarRaySpell() : base("Polar Ray", 0x36D4)
        {
            AddLevel(SpellType.Wizard, 8);
            AddLevel(SpellType.Sorcerer, 8);
            AddTag(SpellTag.SingleTarget);
            AddTag(SpellTag.Offensive);
            AddTag(SpellTag.CC);
        }   

        public override void Cast(Mobile caster, int hue, int level)
        {
            Mobile target = caster.Combatant as Mobile;
            if (target == null || !caster.CanBeHarmful(target))
                return; 

            caster.DoHarmful(target);
            target.SendMessage("You are struck by absolute cold!"); 

            caster.MovingParticles(target, 0x36D4, 7, 0, false, true,
                hue != 0 ? hue : 0x480, 0, 9502, 1, 0, (EffectLayer)255, 0);    

            int dmg = Utility.RandomMinMax(50,55)+level;   

            AOS.Damage(target, caster, dmg, 0, 0, 100, 0, 0);   

            double resist = target.Skills[SkillName.MagicResist].Value;
            int para = (int)(12 + level - resist / 10); 

            if (para > 0)
                target.Paralyze(TimeSpan.FromSeconds(para));    

            target.PlaySound(0x64F);
        }
    }

   public class PowerWordFearSpell : CustomSpell
    {
        public PowerWordFearSpell() : base("Powerword: Fear", 0x22C5)
        {
            AddLevel(SpellType.Wizard, 8);
            AddLevel(SpellType.Sorcerer, 8);
            AddLevel(SpellType.Cleric, 8);
            AddTag(SpellTag.AoE);
            AddTag(SpellTag.Offensive);
            AddTag(SpellTag.CC);
        }

        public override void Cast(Mobile caster, int hue, int level)
        {
            caster.PlaySound(0x204);

            SpellHelpers.ForEachHostileInRange(caster, caster, 5, m =>
            {
                caster.DoHarmful(m);

                double resist = m.Skills[SkillName.MagicResist].Value;
                double duration = Math.Max(2, (10 + level) - resist / 15.0);

                m.Paralyze(TimeSpan.FromSeconds(duration));

                int dist = Utility.RandomMinMax(5, 8);
                Point3D flee = SpellHelpers.GetPointInDirection(
                    m,
                    m.GetDirectionTo(caster),
                    -dist);

                m.MoveToWorld(flee, m.Map);

                m.FixedEffect(0x376A, 9, 32);
                m.SendMessage("You flee in absolute terror!");
            });
        }
    }
    
   public class PowerWordFatigueSpell : CustomSpell
    {
        public PowerWordFatigueSpell() : base("Powerword: Fatigue", 0x22C5)
        {
            AddLevel(SpellType.Wizard, 8);
            AddLevel(SpellType.Sorcerer, 8);
            AddLevel(SpellType.Cleric, 8);
            AddTag(SpellTag.AoE);
            AddTag(SpellTag.Offensive);
            AddTag(SpellTag.Debuff);
        }
    
        public override void Cast(Mobile caster, int hue, int level)
        {
            caster.PlaySound(0x1F8);
    
            SpellHelpers.ForEachHostileInRange(caster, caster, 5, m =>
            {
                caster.DoHarmful(m);
    
                double resist = m.Skills[SkillName.MagicResist].Value;
                double remaining = resist / 20.0;
                if (remaining > 5)
                    remaining = 5;
    
                int newStam = (int)(m.StamMax * (remaining / 100.0));
                m.Stam = Math.Max(0, newStam);
    
                m.FixedEffect(0x3709, 10, 30);
                m.SendMessage("Your limbs feel unbearably heavy!");
            });
        }
    }

    public class MassValiantSpiritSpell : CustomSpell
    {
        public MassValiantSpiritSpell() : base("Valiant Spirit, Mass", 0x480)
        {
            AddLevel(SpellType.Cleric, 8);
            AddTag(SpellTag.AoE);
            AddTag(SpellTag.Heal);
            AddTag(SpellTag.Buff);
        }

        public override void Cast(Mobile caster, int hue, int level)
        {
            IPooledEnumerable eable = caster.GetMobilesInRange(3);

            foreach (Mobile m in eable)
            {
                if (!m.Alive || !BardSpellHelpers.IsFriendly(caster, m))
                    continue;

                int heal = Utility.RandomMinMax(40, 52) + level;
                int str = 10 + level;
                int resist = Math.Min(70, 11 + level);

                m.Hits += heal;
                m.AddStatMod(new StatMod(StatType.Str, "ValiantMassStr", str, TimeSpan.FromSeconds(60 + level * 10)));

                m.AddResistanceMod(new ResistanceMod(ResistanceType.Fire, resist));
                m.AddResistanceMod(new ResistanceMod(ResistanceType.Cold, resist));
                m.AddResistanceMod(new ResistanceMod(ResistanceType.Energy, resist));
                m.AddResistanceMod(new ResistanceMod(ResistanceType.Poison, resist));
                m.AddResistanceMod(new ResistanceMod(ResistanceType.Physical, resist));

                m.FixedParticles(0x375A, 10, 15, 5012, hue, 0, EffectLayer.Waist);
            }

            eable.Free();
        }
    }

    // ===== LEVEL 9 =====
    public class IcebergSpell : CustomSpell
    {
        public IcebergSpell() : base("Iceberg", 0x36BD)
        {
            AddLevel(SpellType.Wizard, 9);
            AddLevel(SpellType.Sorcerer, 9);
            AddTag(SpellTag.AoE);
            AddTag(SpellTag.Offensive);
            AddTag(SpellTag.Debuff);
        }

        public override void Cast(Mobile caster, int hue, int level)
        {
            Mobile target = caster.Combatant as Mobile;
            if (target == null)
                return;

            caster.PlaySound(0x64E);

            SpellHelpers.ForEachHostileInRange(target, caster, 3, m =>
            {
                Effects.SendLocationEffect(m.Location, m.Map, 0x36BD, 20, 10, 0x480, 0);

                int damage = Utility.RandomMinMax(44, 49)+level;
                int stamLoss = damage / 3;

                AOS.Damage(m, caster, damage, 0, 0, 100, 0, 0);
                m.Stam = Math.Max(0, m.Stam - stamLoss);

                m.SendMessage("You are crushed beneath falling ice!");
            });
        }
    }

    public class MeteorSwarmSpell : CustomSpell
    {
        public MeteorSwarmSpell() : base("Meteor Swarm", 0x36CB)
        {
            AddLevel(SpellType.Wizard, 9);
            AddLevel(SpellType.Sorcerer, 9);
            AddTag(SpellTag.AoE);
            AddTag(SpellTag.Offensive);
        }

        public override void Cast(Mobile caster, int hue, int level)
        {
            Mobile target = caster.Combatant as Mobile;
            if (target == null)
                return;

            caster.PlaySound(0x307);

            for (int i = 0; i < 4; i++)
            {
                int ox = Utility.RandomMinMax(-3, 3);
                int oy = Utility.RandomMinMax(-3, 3);

                Point3D center = new Point3D(target.X + ox, target.Y + oy, target.Z);

                for (int x = -1; x <= 0; x++)
                for (int y = -1; y <= 0; y++)
                {
                    Point3D loc = new Point3D(center.X + x, center.Y + y, center.Z);
                    Effects.SendLocationEffect(loc, target.Map, 0x36CB, 16, 10, hue != 0 ? hue : 0x501, 0);
                }

                SpellHelpers.DamageHostilesAtPoint(
                    caster,
                    center,
                    target.Map,
                    1,
                    () => Utility.RandomMinMax(44, 49)+level,
                    0, 100, 0, 0, 0,
                    0x36CB,
                    hue != 0 ? hue : 0x501
                );
            }
        }
    }

    public class PowerWordKillSpell : CustomSpell
    {
        public PowerWordKillSpell() : base("Powerword: Kill", 0x22C5)
        {
            AddLevel(SpellType.Wizard, 9);
            AddLevel(SpellType.Sorcerer, 9);
            AddTag(SpellTag.SingleTarget);
            AddTag(SpellTag.Offensive);
            AddTag(SpellTag.Debuff);
        }

        public override void Cast(Mobile caster, int hue, int level)
        {
            Mobile target = caster.Combatant as Mobile;
            if (target == null || !caster.CanBeHarmful(target))
                return;

            caster.DoHarmful(target);

            if (target.Hits < 101)
            {
                target.Kill();
                target.FixedEffect(0x3709, 10, 30);
                target.PlaySound(0x211);
            }
            else
            {
                target.SendMessage("The word of death fails to claim you!");
                target.PlaySound(0x1F2);
            }
        }
    }

    public class StormOfVengeanceSpell : CustomSpell
    {
        public StormOfVengeanceSpell() : base("Storm of Vengeance", 0x36BD)
        {
            AddLevel(SpellType.Druid, 9);
            AddLevel(SpellType.Cleric, 9);
        }

        public override void Cast(Mobile caster, int hue, int level)
        {
            Mobile target = caster.Combatant as Mobile;
            if (target == null)
                return;

            Point3D center = target.Location;
            Map map = target.Map;

            SpellHelpers.DoTimedStorm(
                caster,
                center,
                map,
                7,
                5,
                3,
                () => Utility.RandomMinMax(44, 49)+level,
                25, 25, 25, 25, 0,
                0x36BD,
                hue != 0 ? hue : 0x480,
                "The storm tears into you!"
            );
        }
    }

    public class SummonNatureAllyIXSpell : SummonNatureAllySpell
    {
        public SummonNatureAllyIXSpell()
            : base("Summon Nature's Ally IX", 0x2156, 9, typeof(Roc))
        {
        }
    }

}