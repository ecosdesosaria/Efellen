using System;
using Server;

namespace Server.Items
{
    public class MarksOfTheWilds : Item
    {
        [Constructable]
        public MarksOfTheWilds() : this(1)
        {
        }

        public override string DefaultDescription{ get{ return "A Mark of the wilds represents your prowess as a druid. It can be aqquired by tamers as they adventure with their followers. The guildmaster of the druid's guild can offer many trinkets for those that would speak of rewards with them."; } }

        [Constructable]
        public MarksOfTheWilds(int amount) : base(0x2ff8)
        {
            Stackable = true;
            Weight = 0.1;
            Hue = 669;
            Amount = amount;
            Name = "Mark of the Wilds";
        }

        public MarksOfTheWilds(Serial serial) : base(serial)
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
