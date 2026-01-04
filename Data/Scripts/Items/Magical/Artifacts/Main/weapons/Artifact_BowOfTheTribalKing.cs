using System;
using Server;

namespace Server.Items
{
    public class Artifact_BowOfTheTribalKing : GiftBow
    {
        private DateTime m_NextArtifactBuff;
        private bool m_BuffActive;
        
        public override int InitMinHits{ get{ return 80; } }
        public override int InitMaxHits{ get{ return 160; } }

        [Constructable]
        public Artifact_BowOfTheTribalKing()
        {
            Name = "Bow of the Tribal King";
            Hue = 0x460;
            ItemID = 0x13B2;
            Slayer = SlayerName.ReptilianDeath;
            Attributes.AttackChance = 15;
            Attributes.WeaponDamage = 25;
            ArtifactLevel = 2;
            Server.Misc.Arty.ArtySetup( this, "Bane of reptile kind" );
            m_NextArtifactBuff = DateTime.MinValue;
            m_BuffActive = false;
        }

        public override void OnHit(Mobile attacker, Mobile defender, double damage)
        {
            base.OnHit(attacker, defender, damage);

            if (attacker == null || defender == null)
                return;

            if (defender.Alive || defender.Hits > 0)
                return;

            if (DateTime.UtcNow < m_NextArtifactBuff)
                return;

            if (m_BuffActive)
                return;

            if (Utility.RandomDouble() > 0.15)
                return;

            if (!IsSlayerEffective(defender))
                return;

            ApplyArtifactBuff(attacker);
            attacker.SendMessage(33, "The fallen reptile empowers you!");
            attacker.PlaySound(0x1E9);
            m_NextArtifactBuff = DateTime.UtcNow + TimeSpan.FromMinutes(5.0);
        }

        private static SlayerEntry s_ReptilianDeathEntry = null;

        private bool IsSlayerEffective(Mobile m)
        {
            if (Slayer != SlayerName.ReptilianDeath)
                return false;

            if (s_ReptilianDeathEntry == null)
                s_ReptilianDeathEntry = SlayerGroup.GetEntryByName(SlayerName.ReptilianDeath);

            return s_ReptilianDeathEntry != null && s_ReptilianDeathEntry.Slays(m);
        }

        private void ApplyArtifactBuff(Mobile m)
        {
            m_BuffActive = true;

            m.AddStatMod(new StatMod(StatType.Str, "ReptileSlayerStr", 10, TimeSpan.FromMinutes(3)));
            m.AddStatMod(new StatMod(StatType.Dex, "ReptileSlayerDex", 10, TimeSpan.FromMinutes(3)));

            new ArtifactBuffEndTimer(this, m).Start();
        }

        private class ArtifactBuffEndTimer : Timer
        {
            private Artifact_BowOfTheTribalKing m_Item;
            private Mobile m_Mobile;

            public ArtifactBuffEndTimer(Artifact_BowOfTheTribalKing item, Mobile mob)
                : base(TimeSpan.FromMinutes(2.0))
            {
                m_Item = item;
                m_Mobile = mob;
                Priority = TimerPriority.TwoFiftyMS;
            }

            protected override void OnTick()
            {
                if (m_Mobile != null && !m_Mobile.Deleted)
                {
                    m_Mobile.RemoveStatMod("ReptileSlayerStr");
                    m_Mobile.RemoveStatMod("ReptileSlayerDex");
                }

                if (m_Item != null && !m_Item.Deleted)
                    m_Item.m_BuffActive = false;
            }
        }

        public Artifact_BowOfTheTribalKing( Serial serial ) : base( serial )
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)1);

            writer.Write(m_NextArtifactBuff);
            writer.Write(m_BuffActive);
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
                    m_NextArtifactBuff = reader.ReadDateTime();
                    m_BuffActive = reader.ReadBool();
                    break;
                }
                case 0:
                {
                    m_NextArtifactBuff = DateTime.MinValue;
                    m_BuffActive = false;
                    break;
                }
            }

            if (Slayer == SlayerName.None)
                Slayer = SlayerName.ReptilianDeath;
        }
    }
}