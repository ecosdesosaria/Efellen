using System;
using Server;
using Server.Items;
using System.Collections;
using Server.Gumps;
using Server.Network;

namespace Server.Items
{
	public class SlimeItem : LockableContainer
	{
		[Constructable]
		public SlimeItem() : base( 0x9A8 )
		{
			Name = "slime covered item";
			Locked = true;
			LockLevel = 1000;
			MaxLockLevel = 1000;
			RequiredSkill = 1000;
			Weight = 0.1;
			//Hue = 0xB85;
		}

        public override void AddNameProperties(ObjectPropertyList list)
		{
            base.AddNameProperties(list);
			list.Add( 1070722, "Coberto de Limo Espesso");
			list.Add( 1049644, "Dê a um servo de estalagem ou taverna para limpar"); // PARENTHESIS
        }

		public override void OnDoubleClick( Mobile from )
		{
			from.SendMessage( "Este item está coberto de limo e não pode ser usado." );
		}

		public SlimeItem( Serial serial ) : base( serial )
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