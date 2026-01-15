using System;
using Server;

namespace Server.Items
{
    public class EerieIdol : Item
    {
        [Constructable]
        public EerieIdol() : this(1)
        {
        }

        [Constructable]
        public EerieIdol(int amount) : base(0x4688)
        {
            Stackable = true;
            Weight = 0.01;
            Hue = 0x0213;
            Amount = amount;
            Name = "Eerie Idol";
        }

        public override string DefaultDescription{ get{ return "This ancient idol shimmers with strange and twisted energy. Pollo might be looking for these at the entrance of the excavation."; } }

        public EerieIdol(Serial serial) : base(serial)
        {
        }

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
