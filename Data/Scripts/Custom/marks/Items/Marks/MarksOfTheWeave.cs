using System;
using Server;

namespace Server.Items
{
    public class MarksOfTheWeave : Item
    {
        [Constructable]
        public MarksOfTheWeave() : this(1)
        {
        }
        
        public override string DefaultDescription{ get{ return "Uma Marca da Trama representa sua perícia como conjurador. Pode ser adquirida por pesquisadores e magos ao derrotarem conjuradores pelas terras e realizarem pesquisas em tomos antigos encontrados em masmorras. O mestre da guilda dos magos pode oferecer muitas bugigangas para aqueles que falarem sobre recompensas com eles."; } }

        [Constructable]
        public MarksOfTheWeave(int amount) : base(0x2ff8)
        {
            Stackable = true;
            Weight = 0.01;
            Hue = 0x0213;
            Amount = amount;
            Name = "Marca da Trama";
        }

        public MarksOfTheWeave(Serial serial) : base(serial)
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
