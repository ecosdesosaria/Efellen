using System;
using Server.ContextMenus;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Misc;
using Server.Gumps;
using Server.Network;
using Server.Targeting;
using Server.Custom.DefenderOfTheRealm.Vow;

namespace Server.Custom.DefenderOfTheRealm.Knight
{
    public class DefenderOfRealm : BaseCreature
    {
        private DateTime m_NextSpeechTime;
        [Constructable]
        public DefenderOfRealm() : base(AIType.AI_Thief, FightMode.None, 10, 1, 0.4, 1.6)
        {
            InitStats(125, 55, 65);
            Name = this.Female ? NameList.RandomName("female") : NameList.RandomName("male");
            Title = "Defensor do Reino";
            HairHue = Utility.RandomHairHue();
            Body = this.Female ? 0x191 : 0x190;
            SpeechHue = Utility.RandomTalkHue();
            Hue = Utility.RandomSkinHue();
            Utility.AssignRandomHair(this);
            if ((!this.Female))
            {
                FacialHairItemID = Utility.RandomList(0, 8254, 8255, 8256, 8257, 8267, 8268, 8269);
            }
            AddItem(new Boots(Utility.RandomBirdHue()));
            AddItem(new Cloak(Utility.RandomBirdHue()));
            AddItem(new Artifact_DefenderOfTheRealmArms());
            AddItem(new Artifact_DefenderOfTheRealmChestpiece());
            AddItem(new Artifact_DefenderOfTheRealmGloves());
            AddItem(new Artifact_DefenderOfTheRealmGorget());
            AddItem(new Artifact_DefenderOfTheRealmHelmet());
            AddItem(new Artifact_DefenderOfTheRealmLeggings());
        }

        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            if (InRange(m, 6) && !InRange(oldLocation, 2))
            {
                if (m is PlayerMobile && !m.Hidden)
                {
                    if (DateTime.UtcNow >= m_NextSpeechTime)
                    {
                        switch (Utility.Random(11))
                        {
                            case 0: Say("Os Defensores do Reino precisam de reforços!"); break;
                            case 1: Say("Abatei muitas bestas vis e tornai nossa terra mais segura!"); break;
                            case 2: Say("Por decreto do rei, livraremos esta terra do mal!"); break;
                            case 3: Say("Erguei-vos, poderosos guerreiros do reino! Nossos entes queridos contam com vossa coragem!"); break;
                            case 4: Say("Fortalecei vosso coração, pois a escuridão inquieta ronda estas terras!"); break;
                            case 5: Say("Provai vosso valor em nome de nosso rei!"); break;
                            case 6: Say("A honra é sua própria recompensa para os dignos!"); break;
                            case 7: Say("Erguei vossa lâmina em nome da virtude!"); break;
                            case 8: Say("Cuidado! Muitos perigos vos esperam!"); break;
                            case 9: Say("As hordas vis serão decapitadas pelo abate de seus generais!"); break;
                            case 10: Say("Muitos perdemos em nossa luta contra a escuridão, mas não daremos trégua a ela!"); break;
                        }
                        m_NextSpeechTime = DateTime.UtcNow + TimeSpan.FromSeconds(10);
                    }
                }
            }
        }

        public override bool HandlesOnSpeech(Mobile from)
        {
            return true;
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            Mobile from = e.Mobile;

            if (from == null || !(from is PlayerMobile))
                return;

            if (e.Mobile.InRange(this, 4))
            {
                if (e.Speech.IndexOf("reward") >= 0)
                {
                    MaybeShowDefenderOfTheRealmRewardsGump(from as PlayerMobile);
                }
                else
                {
                    base.OnSpeech(e);
                }
            }
        }

        public void MaybeShowDefenderOfTheRealmRewardsGump(PlayerMobile from)
        {
            if (from.Karma > 0)
            {
                from.SendGump(new Server.Custom.DefenderOfTheRealm.RewardGump(from, 1, 0));
                Say("Estas são as recompensas que posso te oferecer.");
            }
            else
            {
                Say("Não oferecerei meus serviços a servos do mal! Redima-te!");
            }
        }

        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            base.GetContextMenuEntries(from, list);
            if (from is PlayerMobile)
            {
                list.Add(new GiveVowEntry(from, this));
                list.Add(new RewardsEntry(from, this));
            }
        }

        private class RewardsEntry : ContextMenuEntry
        {
            private DefenderOfRealm m_Npc;
            private Mobile m_From;

            public RewardsEntry(Mobile from, DefenderOfRealm npc) : base(6093, 3)
            {
                m_From = from;
                m_Npc = npc;
            }

            public override void OnClick()
            {
                m_Npc.MaybeShowDefenderOfTheRealmRewardsGump(m_From as PlayerMobile);
            }
        }

        private class GiveVowEntry : ContextMenuEntry
        {
            private Mobile m_From;
            private DefenderOfRealm m_Npc;
            private static TimeSpan Delay = TimeSpan.FromHours(6);
            private static Dictionary<PlayerMobile, DateTime> LastUsers = new Dictionary<PlayerMobile, DateTime>();

            public GiveVowEntry(Mobile from, DefenderOfRealm npc) : base(6146)
            {
                m_From = from;
                m_Npc = npc;
            }

            public override void OnClick()
            {
                if (!(m_From is PlayerMobile))
                    return;

                if (m_From == null || m_From.Deleted || m_Npc == null || m_Npc.Deleted)
                    return;

                PlayerMobile mobile = (PlayerMobile)m_From;
                DateTime lastUse;

                if (!mobile.CheckAlive())
                {
                    mobile.SendMessage("Você precisa estar vivo para receber um Juramento de Honra.");
                    return;
                }
                else if (mobile.Backpack == null)
                {
                    mobile.SendMessage("Você não tem uma mochila para receber o Juramento de Honra.");
                    return;
                }
                else if (LastUsers.TryGetValue(mobile, out lastUse))
                {
                    TimeSpan cooldown = Delay - (DateTime.UtcNow - lastUse);
                    if (cooldown > TimeSpan.Zero)
                    {
                        m_Npc.Say(String.Format("Terei outro juramento para você em {0} hora{1} e {2} minuto{3}.",
                          cooldown.Hours, cooldown.Hours == 1 ? "" : "s",
                          cooldown.Minutes, cooldown.Minutes == 1 ? "" : "s"));
                        return;
                    }
                }
                else if (mobile.Karma < 0)
                {
                    m_Npc.Say("Tu te desviaste do caminho da virtude! Não confiarei em ti para seres honrado até que te redimas!");
                    return;
                }
                if (CanGetVow(mobile))
                {
                    LastUsers[mobile] = DateTime.UtcNow;
                    VowOfHonor vow = new VowOfHonor(mobile);
                    m_From.Backpack.DropItem(vow);

                    if (vow.Parent == mobile.Backpack)
                    {
                        mobile.SendGump(new SpeechGump(mobile, "Defender of the Realm", SpeechFunctions.SpeechText(m_Npc, mobile, "Defender of the Realm")));
                        mobile.SendMessage("Você recebe um Juramento de Honra.");
                    }
                    else
                    {
                        vow.Delete();
                        mobile.SendMessage("Você não tem espaço suficiente em seu inventário para receber um Juramento de Honra.");
                    }
                }
            }
            private bool CanGetVow(PlayerMobile asker)
            {
                if (!LastUsers.ContainsKey(asker))
                {
                    LastUsers.Add(asker, DateTime.UtcNow);
                    return true;
                }
                else
                {
                    if (DateTime.UtcNow - LastUsers[asker] < Delay)
                    {
                        return false;
                    }
                    else
                    {
                        LastUsers[asker] = DateTime.UtcNow;
                        return true;
                    }
                }
            }
        }

        public DefenderOfRealm(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
