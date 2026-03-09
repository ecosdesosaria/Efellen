using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;
using Server.Misc;

namespace Server.Items
{
    public class WandOfDisenchanting : Item
    {
        private int m_Charges;

        [Constructable]
        public WandOfDisenchanting() : base(0xDF5)
        {
            Name = "Wand of disenchanting";
            Hue = 1350;
            Weight = 1.0;
            m_Charges = 75;
        }

        public WandOfDisenchanting(Serial serial) : base(serial)
        {
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Charges
        {
            get { return m_Charges; }
            set { m_Charges = value; InvalidateProperties(); }
        }

        public override string DefaultDescription{ get{ return "Esta varinha é usada para desvendar itens mágicos e tirar sua essência. Ele pode ter como alvo um único item, ou uma bolsa ou contêiner cheio de itens, fazendo isso irá destruí-los e recompensá-lo com Pó Arcano que artesãos habilidosos da guilda podem usar para aprimorar itens, e que também é de grande valor para magos poderosos. Artefatos e itens não identificados nunca podem ser desencantados."; } }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);
            list.Add("cargas restantes: {0}", m_Charges);
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!IsChildOf(from.Backpack))
            {
                from.SendMessage("A varinha deve estar em seu inventário para usá-la.");
                return;
            }

            if (m_Charges <= 0)
            {
                from.SendMessage("A varinha não tem cargas restantes.");
                return;
            }

            from.SendMessage("Selecione o item ou contêiner que deseja desencantar.");
            from.Target = new DisenchantTarget(this);
        }

        private class DisenchantTarget : Target
        {
            private WandOfDisenchanting m_Wand;

            public DisenchantTarget(WandOfDisenchanting wand) : base(10, false, TargetFlags.None)
            {
                m_Wand = wand;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
              Server.Misc.DisenchantingSystem.HandleTarget(from, targeted as Item, m_Wand);
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
            writer.Write(m_Charges);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt();
            m_Charges = reader.ReadInt();
        }
    }
}