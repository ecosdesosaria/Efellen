using System;
using Server;

namespace Server.Items
{
    public class DraconicPortcullis : PortcullisNS
    {
        [Constructable]
        public DraconicPortcullis() : base()
        {
            Movable = false;
            Name = "draconic Portcullis";
        }

        public DraconicPortcullis(int itemID) : base(itemID)
        {
            Movable = false;
            Name = "draconic Portcullis";
        }

        public DraconicPortcullis(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}