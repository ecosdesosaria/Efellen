using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Items
{
    public class TotemOfTheWilds : Item
    {
        private string m_FormId;

        [CommandProperty(AccessLevel.GameMaster)]
        public string FormId
        {
            get { return m_FormId; }
            set { m_FormId = value; }
        }

        public override string DefaultName
        {
            get { return "Totem of the Wilds"; }
        }

        [Constructable]
        public TotemOfTheWilds() : this(null)
        {
        }

        [Constructable]
        public TotemOfTheWilds(string formId) : base(12122)
        {
            Hue = 669;
            Weight = 1.0;
            m_FormId = formId;

            if (!String.IsNullOrEmpty(formId))
                Name = "Totem of the " + formId;
        }

        public override string DefaultDescription{ get{ return "A totem of the wilds contains the spiritual essence of a creature. It can be used to infuse a druid's talisman and teach them to shapeshift into that creature"; } }
        
        public override void OnDoubleClick(Mobile from)
        {
            if (!IsChildOf(from.Backpack))
            {
                from.SendMessage("The totem must be in your backpack to use it.");
                return;
            }

            if (String.IsNullOrEmpty(m_FormId))
            {
                from.SendMessage("This totem holds no spiritual knowledge.");
                return;
            }

            from.SendMessage("Select the Heart of the Wilds to absorb this totem.");
            from.Target = new TotemTarget(this);
        }


        public TotemOfTheWilds(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_FormId);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_FormId = reader.ReadString();

            if (!String.IsNullOrEmpty(m_FormId))
                Name = "Totem of the " + m_FormId;
        }
    }

    public class TotemTarget : Target
{
    private TotemOfTheWilds m_Totem;

    public TotemTarget(TotemOfTheWilds totem) : base(1, false, TargetFlags.None)
    {
        m_Totem = totem;
    }

    protected override void OnTarget(Mobile from, object targeted)
    {
        if (m_Totem == null || m_Totem.Deleted)
            return;

        HeartOfTheWilds heart = targeted as HeartOfTheWilds;

        if (heart == null)
        {
            from.SendMessage("That is not a Heart of the Wilds.");
            return;
        }

        if (!heart.IsChildOf(from))
        {
            from.SendMessage("You must be holding the Heart of the Wilds.");
            return;
        }

        if (heart.IsFormUnlocked(m_Totem.FormId))
        {
            from.SendMessage("You have already mastered this form.");
            return;
        }

        heart.UnlockForm(m_Totem.FormId);

        from.SendMessage("The spirits reveal the form of the {0} to you.", m_Totem.FormId);
        from.FixedParticles(0x373A, 10, 15, 5018, EffectLayer.Waist);
        from.PlaySound(0x1F7);

        m_Totem.Delete();
    }
}

}
