using System;
using Server;

namespace Server.Items
{
    public class MarksOfDevotion : Item
    {
        [Constructable]
        public MarksOfDevotion() : this(1)
        {
        }
        
        public override string DefaultDescription{ get{ return "A Mark of devotion represents your prowess as a cleric. It can be aqquired by healers and holy adventurers as they slay vile undead and spread healing through the lands. The guildmaster of the healers guild can offer many trinkets for those that would speak of rewards with them."; } }

        [Constructable]
        public MarksOfDevotion(int amount) : base(0x2ff8)
        {
            Stackable = true;
            Weight = 0.1;
            Hue = 0x9C2;
            Amount = amount;
            Name = "Mark of Devotion";
        }

        public MarksOfDevotion(Serial serial) : base(serial)
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
