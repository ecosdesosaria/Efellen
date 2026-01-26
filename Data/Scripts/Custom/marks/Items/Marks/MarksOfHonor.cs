using System;
using Server;

namespace Server.Items
{
    public class MarksOfHonor : Item
    {
        [Constructable]
        public MarksOfHonor() : this(1)
        {
        }

        [Constructable]
        public MarksOfHonor(int amount) : base(0xFF5)
        {
            Stackable = true;
            Weight = 0.01;
            Hue = 0x35;
            Amount = amount;
            Name = "Mark of Honor";
        }

                public override string DefaultDescription{ get{ return "A Mark of Honor represents your commitment to all that is good. It can be aqquired by  while defeating their enemies, specially those that do terrible deeds in the name of all that is evil. The Defender of the realm can offer many boons for those that would speak of rewards with them in exchange for these marks."; } }

        public MarksOfHonor(Serial serial) : base(serial)
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
