using System;
using Server;
using Server.Items;
using System.Collections;
using Server.Gumps;
using Server.Network;

namespace Server.Items
{
	public class WeededItem : LockableContainer
	{
		[Constructable]
		public WeededItem() : base( 0x9A8 )
		{
			Name = "weed covered item";
			Locked = true;
			LockLevel = 1000;
			MaxLockLevel = 1000;
			RequiredSkill = 1000;
			Weight = 0.1;
			Hue = 0xB87;
		}

        public override void AddNameProperties(ObjectPropertyList list)
		{
            base.AddNameProperties(list);
			list.Add( 1070722, "Envolto em Ervas Daninhas Grossas");
			list.Add( 1049644, "Dê a um alquimista ou herbalista para remover"); // PARENTHESIS
        }

		public override void OnDoubleClick( Mobile from )
		{
			from.SendMessage( "Este item está envolto em ervas daninhas e não pode ser usado." );
		}

		public WeededItem( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
}