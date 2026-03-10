using System;
using Server;
using System.Collections.Generic;
using Server.Spells.Chivalry;

namespace Server.Items
{
    public class Artifact_Fury : GiftKatana
    {
        private Dictionary<Mobile, DateTime> m_Cooldowns;
        
        public override int InitMinHits{ get{ return 80; } }
        public override int InitMaxHits{ get{ return 160; } }

        [Constructable]
        public Artifact_Fury()
        {
            Name = "Fury";
            ItemID = 0x13FF;
            WeaponAttributes.HitFireball = 25;
            WeaponAttributes.HitLightning = 25;
            WeaponAttributes.HitHarm = 25;
            Attributes.ReflectPhysical = 25;
            Hue = 1357;
            ArtifactLevel = 2;
            Server.Misc.Arty.ArtySetup( this, "Brings forth Divine Fury" );
            m_Cooldowns = new Dictionary<Mobile, DateTime>();
        }

        private bool CanCast(Mobile m)
        {
            if (m == null || m.Deleted)
                return false;

            DateTime next;
            if (!m_Cooldowns.TryGetValue(m, out next))
                return true;

            if (DateTime.UtcNow >= next)
            {
                m_Cooldowns.Remove(m);
                return true;
            }

            return false;
        }

        private void StartCooldown(Mobile m)
        {
            if (m == null || m.Deleted)
                return;

            m_Cooldowns[m] = DateTime.UtcNow + TimeSpan.FromMinutes(1.0);
        }

        public override void OnHit(Mobile attacker, Mobile defender, double damageBonus)
        {
            base.OnHit(attacker, defender, damageBonus);

            if (attacker == null || defender == null)
                return;

            if (Utility.RandomDouble() >= 0.15)
                return;

            if (!CanCast(attacker))
                return;

            new DivineFurySpell(attacker, this).Cast();
            StartCooldown(attacker);
            attacker.SendMessage("Fury empowers you!");
        }

        public override void OnDoubleClick( Mobile from )
        {
            if (Parent != from)
            {
                from.SendMessage("Você precisa estar segurando Fúria para invocar seu poder.");
                return;
            }

            if (!CanCast(from))
            {
                from.SendMessage("Fúria ainda está recarregando.");
                return;
            }

            new DivineFurySpell(from, this).Cast();
            StartCooldown(from);
        }

        public override void GetDamageTypes( Mobile wielder, out int phys, out int fire, out int cold, out int pois, out int nrgy, out int chaos, out int direct )
        {
            phys = 40;
            cold = 15;
            fire = 15;
            nrgy = 15;
            pois = 15;
            chaos = 0;
            direct = 0;
        }

        public Artifact_Fury( Serial serial ) : base( serial )
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)2);

            writer.Write(m_Cooldowns.Count);
            foreach (KeyValuePair<Mobile, DateTime> kvp in m_Cooldowns)
            {
                writer.Write(kvp.Key);
                writer.Write(kvp.Value);
            }
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            
            ArtifactLevel = 2;
            
            int version = reader.ReadInt();

            switch (version)
            {
                case 2:
                {
                    m_Cooldowns = new Dictionary<Mobile, DateTime>();
                    int count = reader.ReadInt();
                    for (int i = 0; i < count; i++)
                    {
                        Mobile m = reader.ReadMobile();
                        DateTime dt = reader.ReadDateTime();
                        if (m != null && !m.Deleted)
                            m_Cooldowns[m] = dt;
                    }
                    break;
                }
                case 1:
                case 0:
                {
                    m_Cooldowns = new Dictionary<Mobile, DateTime>();
                    break;
                }
            }
        }
    }
}