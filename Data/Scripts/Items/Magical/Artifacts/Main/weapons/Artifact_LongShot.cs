using System;
using Server;

namespace Server.Items
{
    public class Artifact_LongShot : GiftCompositeBow
    {
        public override int InitMinHits { get { return 80; } }
        public override int InitMaxHits { get { return 160; } }

        private DateTime m_NextHeadshot;
        private bool m_WasHiddenBeforeShot;

        [Constructable]
        public Artifact_LongShot()
        {
            Name = "Long Shot";
            Hue = 1195;
            ItemID = 0x13B2;
            Attributes.AttackChance = 30;
            Attributes.SpellChanneling = 1;
            Velocity = 30;
            ArtifactLevel = 2;
            m_NextHeadshot = DateTime.MinValue;
            m_WasHiddenBeforeShot = false;
            Server.Misc.Arty.ArtySetup(this, "Headhunter");
        }

        public override void OnHit(Mobile attacker, Mobile defender, double damageBonus)
        {
            base.OnHit(attacker, defender, damageBonus);

            if (!m_WasHiddenBeforeShot)
                return;

            if (attacker == null || defender == null || !defender.Alive)
                return;

            if (DateTime.UtcNow < m_NextHeadshot)
                return;

            double ms = attacker.Skills[SkillName.Marksmanship].Value;
            double hd = attacker.Skills[SkillName.Hiding].Value;
            double chance = 15.0 + ((ms / 25.0) + (hd / 25.0));

            if (Utility.RandomDouble() > (chance / 100.0))
                return;

            double cooldownSeconds = (10 * 60) - (hd * 5.0);
            if (cooldownSeconds < 10) cooldownSeconds = 10;

            m_NextHeadshot = DateTime.UtcNow + TimeSpan.FromSeconds(cooldownSeconds);

            double tactics = attacker.Skills[SkillName.Tactics].Value;
            double tracking = attacker.Skills[SkillName.Tracking].Value;
            
            int dmg = Utility.RandomMinMax(this.MinDamage, this.MaxDamage);
            dmg += (int)(attacker.Str / 10.0);
            dmg = (int)(dmg * (0.5 + (tactics * 0.005)));
            dmg = (int)(dmg * (1.0 + (tracking * 0.005)));
            dmg = (int)(dmg * (1.0 + damageBonus));
            
            if (dmg < 1) dmg = 1;

            int extra = (int)(dmg * 0.50);
            if (extra < 1) extra = 1;

            AOS.Damage(defender, attacker, extra, 100, 0, 0, 0, 0);
            attacker.SendMessage(1161, "Tiro certeiro com " + extra + " de dano extra");
            defender.FixedParticles(0x36BD, 10, 10, 5044, 0, 0, EffectLayer.Head);
            defender.PlaySound(0x22F);
        }

        public override TimeSpan OnSwing(Mobile attacker, Mobile defender)
        {
            m_WasHiddenBeforeShot = attacker.Hidden;
            return base.OnSwing(attacker, defender);
        }

        public override void GetDamageTypes(Mobile wielder, out int phys, out int fire, out int cold, out int pois, out int nrgy, out int chaos, out int direct)
        {
            phys = 100;
            cold = 0;
            fire = 0;
            nrgy = 0;
            pois = 0;
            chaos = 0;
            direct = 0;
        }

        public Artifact_LongShot(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)1);

            writer.Write(m_NextHeadshot);
            writer.Write(m_WasHiddenBeforeShot);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            
            ArtifactLevel = 2;
            
            int version = reader.ReadInt();

            switch (version)
            {
                case 1:
                {
                    m_NextHeadshot = reader.ReadDateTime();
                    m_WasHiddenBeforeShot = reader.ReadBool();
                    break;
                }
                case 0:
                {
                    m_NextHeadshot = DateTime.MinValue;
                    m_WasHiddenBeforeShot = false;
                    break;
                }
            }
        }
    }
}
