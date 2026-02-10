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
        
        public override string DefaultDescription{ get{ return "A Mark of the weave represents your prowess as a spellcaster. It can be aqquired by researchers and wizards as they defeat spellcasters through the lands and perform research in ancient tomes found in dungeons. The guildmaster of the wizards guild can offer many trinkets for those that would speak of rewards with them."; } }

        [Constructable]
        public MarksOfTheWeave(int amount) : base(0x2ff8)
        {
            Stackable = true;
            Weight = 0.01;
            Hue = 0x0213;
            Amount = amount;
            Name = "Mark of the Weave";
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
