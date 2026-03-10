using System;
using Server;
using Server.Items;
using Server.Misc;
using Server.Mobiles;
using Server.Targeting;
using Server.Network;
using Server.Custom.DefenderOfTheRealm.Knight;
using Server.Custom.DefenderOfTheRealm.VowGump;

namespace Server.Custom.DefenderOfTheRealm.Vow
{
    public class VowOfHonor : Item
    {
        private string m_OwnerName;
        private int m_Level;
        private int m_Required;
        private int m_Current;
        private int m_Reward;

        public string OwnerName { get { return m_OwnerName; } set { m_OwnerName = value; InvalidateProperties(); } }
        public int Level { get { return m_Level; } set { m_Level = value; InvalidateProperties(); } }
        public int Required { get { return m_Required; } set { m_Required = value; InvalidateProperties(); } }
        public int Current { get { return m_Current; } set { m_Current = value; InvalidateProperties(); } }
        public int Reward { get { return m_Reward; } set { m_Reward = value; InvalidateProperties(); } }

        [Constructable]
        public VowOfHonor(Mobile from) : base(5360)
        {
            Hue = 0x35;
            LootType = LootType.Blessed;
            Name = from.Name+"'s Vow of Honor";
            m_OwnerName = from.Name;
            m_Level = IntelligentAction.GetCreatureLevel(from);
            m_Required = VowRewardHelper.GetRequiredAmount(m_Level);
            m_Current = 0;
            m_Reward = 0;
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from == null || from.Deleted)
                return;

            if (from.Backpack == null || !IsChildOf(from.Backpack))
            {
                from.SendMessage("O Juramento deve estar em sua mochila para ser usado.");
                return;
            }

            from.SendGump(new VowOfHonorGump(from, this));
        }

        public override string DefaultDescription{ get{ return "Um Juramento representa o compromisso de abater inimigos temíveis que habitam em masmorras. Quando concluído, leve-o até quem o concedeu a você. Diga 'rewards' a essa pessoa para ver quais presentes podem ser concedidos a você ao completá-los."; } }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);

            list.Add("Pertence a {0}", m_OwnerName);
            list.Add("Um juramento de abater {0} inimigos mortais", m_Required);
            list.Add("Progresso: {0}/{1}", m_Current, m_Required);
            list.Add("Recompensa até agora: {0} Marcas", m_Reward);
        }

        public void AddTrophy(Mobile from)
        {
            if (m_Current >= m_Required)
                return;

            int luck = from.Luck;
            if (luck > 2000) luck = 2000;

            int marks = 2 + (luck * (8) / 2000);//2-10 marks added based on player's luck
            m_Current++;
            m_Reward += Utility.RandomMinMax((int)(marks * 0.6), (int)(marks * 1.2)) < 1 ? 1 : Utility.RandomMinMax((int)(marks * 0.6), (int)(marks * 1.2));
            InvalidateProperties();
            from.SendMessage("Você adiciona um troféu ao seu Juramento de Honra.");
            if (m_Current >= m_Required)
            {
                from.SendMessage("Seu juramento de honra está completo!");
            }
        }

        public VowOfHonor(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
            writer.Write(m_OwnerName);
            writer.Write(m_Level);
            writer.Write(m_Required);
            writer.Write(m_Current);
            writer.Write(m_Reward);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_OwnerName = reader.ReadString();
            m_Level = reader.ReadInt();
            m_Required = reader.ReadInt();
            m_Current = reader.ReadInt();
            m_Reward = reader.ReadInt();
        }
        public override bool OnDroppedToMobile(Mobile from, Mobile target)
        {
            if (target == null || !(target is DefenderOfRealm))
            {
                from.SendMessage("Você só pode completar seu Juramento com um Defensor do Reino.");
                return false;
            }
            if (from.Name != m_OwnerName)
            {
                from.SendMessage("Este juramento não pertence a você!");
                return false;
            }
            if (m_Current < m_Required)
            {
                from.SendMessage("Seu juramento não está completo. Você precisa de {0} mais troféus.", m_Required - m_Current);
                return false;
            }
            if (from.Backpack == null || from.Backpack.Items.Count >= from.Backpack.MaxItems)
            {
                from.SendMessage("Você não tem espaço suficiente em seu inventário para receber as recompensas.");
                return false;
            }
            Bag rewardBag = new Bag();
            rewardBag.Name = "Deeds of Valor";
            rewardBag.Hue = 0x35;
            VowRewardHelper.GenerateRewards(from, m_Reward, rewardBag,VowType.Honor);
            rewardBag.DropItem(new MarksOfHonor(m_Reward));
            from.AddToBackpack(rewardBag);
            from.SendMessage("Você completou seu Juramento de Honra e recebeu uma bolsa contendo {0} Marcas de Honra e recompensas adicionais!", m_Reward);
            Effects.PlaySound(from.Location, from.Map, 0x243);
            Misc.Titles.AwardKarma( from, 400, true );
            this.Delete();
            return true;
        }
    }

    public class VowTrophyTarget : Target
    {
        private VowOfHonor m_Vow;
        private Mobile m_From;

        public VowTrophyTarget(Mobile from, VowOfHonor vow) : base(1, false, TargetFlags.None)
        {
            m_From = from;
            m_Vow = vow;
        }

        protected override void OnTarget(Mobile from, object targeted)
        {
            if (m_Vow == null || m_Vow.Deleted)
                return;
            Item item = targeted as Item;
            if (item == null)
            {
                from.SendMessage("Esse não é um troféu válido. Apenas itens adquiridos de inimigos temíveis em masmorras profundas podem ser adicionados a ele.");
                return;
            }
            if (from.Name != m_Vow.OwnerName)
            {
                from.SendMessage("Este juramento não pertence a você!");
                return;
            }
            if (!(item is SummonItems))
            {
                from.SendMessage("Esse item não pode ser adicionado ao seu Juramento. Apenas itens adquiridos de inimigos temíveis em masmorras profundas podem ser adicionados a ele.");
                return;
            }
            SummonItems summonItem = (SummonItems)item;
            if (summonItem.Owner == null)
            {
                from.SendMessage("Este troféu não tem dono e não pode ser adicionado ao seu juramento.");
                return;
            }
            if (summonItem.Owner.Name != m_Vow.OwnerName)
            {
                from.SendMessage("Este troféu não foi capturado por você!");
                return;
            }
            item.Delete();
            m_Vow.AddTrophy(from);
        }
    }
}
