using System;
using Server;

namespace Server.Items
{
    public class MarksOfTheScourge : Item
    {
        [Constructable]
        public MarksOfTheScourge() : this(1)
        {
        }

        [Constructable]
        public MarksOfTheScourge(int amount) : base(0xFF5)
        {
            Stackable = true;
            Weight = 0.01;
            Hue = 0x25;
            Amount = amount;
            Name = "Mark of the Scourge";
        }

        public override string DefaultDescription{ get{ return "A Mark of the Scourge represents your commitment to vileness. It can be aqquired by characters of negative karma while defeating their enemies, specially those that do great deeds in the name of all that is good. The Scourge of the realm can offer many boons for those that would speak of rewards with them in exchange for these marks."; } }

        public MarksOfTheScourge(Serial serial) : base(serial)
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
