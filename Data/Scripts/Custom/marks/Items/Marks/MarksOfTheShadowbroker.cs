using System;
using Server;

namespace Server.Items
{
    public class MarksOfTheShadowbroker : Item
    {
        [Constructable]
        public MarksOfTheShadowbroker() : this(1)
        {
        }
        
        public override string DefaultDescription{ get{ return "Uma Marca do Subornador das Sombras representa sua perícia como ladrão. Pode ser adquirida por ladrões enquanto eles se aventuram e surrupiam os bolsos de suas vítimas. O mestre da guilda dos ladrões pode oferecer muitas bugigangas para aqueles que falarem sobre recompensas com eles."; } }

        [Constructable]
        public MarksOfTheShadowbroker(int amount) : base(0x2ff8)
        {
            Stackable = true;
            Weight = 0.01;
            Hue = 0x455;
            Amount = amount;
            Name = "Marca do Subornador das Sombras";
        }

        public MarksOfTheShadowbroker(Serial serial) : base(serial)
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
