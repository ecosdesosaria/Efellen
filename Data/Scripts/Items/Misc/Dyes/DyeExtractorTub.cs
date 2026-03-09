using System;
using Server;
using Server.Targeting;
using Server.Network;

namespace Server.Items
{
    public class DyeExtractorTub : DyeTub
    {
        private int m_ExtractedHue;

        [CommandProperty(AccessLevel.GameMaster)]
        public int ExtractedHue
        {
            get { return m_ExtractedHue; }
            set 
            { 
                m_ExtractedHue = value; 
                Hue = value;
                DyedHue = value;
                InvalidateProperties();
            }
        }

        [Constructable]
        public DyeExtractorTub()
        {
            m_ExtractedHue = 0;
            Hue = 0;
            Name = "Dye Extractor Tub";
            Redyable = false;
        }

        public DyeExtractorTub(Serial serial) : base(serial)
        {
        }
        public override string DefaultDescription{ get{ return "Esse item pode consumir uma peça de roupa para extrair a cor dela, que pode então ser transferida para outro recipiente de tintura."; } }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);

            if (m_ExtractedHue > 0)
            {
                list.Add(1060658, "Cor extraída\t{0}", m_ExtractedHue.ToString());
            }
            else
            {
                list.Add(1070722, "Vazio - Pronto para Extrair");
            }
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from.InRange(this.GetWorldLocation(), 2))
            {
                if (m_ExtractedHue > 0)
                {
                    from.SendMessage("Selecione outro recipiente de tintura para transferir a cor extraída.");
                    from.Target = new TransferTarget(this);
                }
                else
                {
                    from.SendMessage("Selecione um item para extrair a cor. O item será destruído.");
                    from.Target = new ExtractTarget(this);
                }
            }
            else
            {
                from.SendLocalizedMessage(500446); 
            }
        }

        private class ExtractTarget : Target
        {
            private DyeExtractorTub m_Tub;

            public ExtractTarget(DyeExtractorTub tub) : base(1, false, TargetFlags.None)
            {
                m_Tub = tub;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (m_Tub == null || m_Tub.Deleted)
                    return;

                if (m_Tub.ExtractedHue > 0)
                {
                    from.SendMessage("Este recipiente já contém uma cor extraída. Transfira-a primeiro.");
                    return;
                }

                Item item = targeted as Item;

                if (item == null)
                {
                    from.SendMessage("Você só pode extrair cor de um item.");
                    return;
                }

                if (!item.IsChildOf(from.Backpack))
                {
                    from.SendLocalizedMessage(502437); // Must be in backpack
                    return;
                }

                if (item.Hue == 0)
                {
                    from.SendMessage("Este item não tem cor para extrair.");
                    return;
                }

                if (item is DyeTub || item is DyeExtractorTub)
                {
                    from.SendMessage("Você não pode extrair cor de um recipiente de tintura.");
                    return;
                }

                // Extract color and destroy item
                int extractedHue = item.Hue;
                item.Delete();

                m_Tub.ExtractedHue = extractedHue;

                from.SendMessage("Cor extraída com sucesso! O item foi consumido.");
                from.SendMessage("Agora selecione outro recipiente de tintura para aplicar esta cor.");
            }
        }

        private class TransferTarget : Target
        {
            private DyeExtractorTub m_SourceTub;

            public TransferTarget(DyeExtractorTub tub) : base(1, false, TargetFlags.None)
            {
                m_SourceTub = tub;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (m_SourceTub == null || m_SourceTub.Deleted)
                    return;

                if (m_SourceTub.ExtractedHue == 0)
                {
                    from.SendMessage("Este recipiente não contém nenhuma cor extraída.");
                    return;
                }

                DyeTub targetTub = targeted as DyeTub;

                if (targetTub == null)
                {
                    from.SendMessage("Você deve selecionar um recipiente de tintura.");
                    return;
                }

                if (!targetTub.IsChildOf(from.Backpack))
                {
                    from.SendLocalizedMessage(502437); 
                    return;
                }

                if (targetTub is DyeExtractorTub)
                {
                    from.SendMessage("Você não pode transferir cor para outro recipiente de extração.");
                    return;
                }

                int colorToTransfer = m_SourceTub.ExtractedHue;

                targetTub.Hue = colorToTransfer;
                targetTub.DyedHue = colorToTransfer;

                m_SourceTub.ExtractedHue = 0;

                from.SendMessage("Cor transferida com sucesso!");
                from.PlaySound(0x23E);
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)1); // version
            writer.Write(m_ExtractedHue);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            if (version >= 1)
                m_ExtractedHue = reader.ReadInt();
            else
                m_ExtractedHue = 0;
        }
    }
}