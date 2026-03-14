using System;
using Server;
using System.Collections;
using System.Collections.Generic;
using Server.Misc;
using Server.Network;

namespace Server.Items
{
    public class DwarvenForge : Item
	{
        [Constructable]
        public DwarvenForge() : base( 0x544A )
		{
            Name = "dwarven forge";
			Weight = 100.0;
			Light = LightType.Empty;
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( IsChildOf( from.Backpack ) )
				from.SendMessage( "Você só pode acender isto se estiver seguro em uma casa." );
			else if ( !from.InRange( GetWorldLocation(), 2 ) )
				from.SendMessage( "Você precisará se aproximar para acender isso." );
			else if ( Movable )
				from.SendMessage( "Você só pode acender isso se estiver seguro em uma casa." );
			else
			{
				if ( ItemID == 0x544A ){ 	ItemID = 0x544B; Light = LightType.Circle225; 	from.SendSound( 0x208 ); }
				else { 						ItemID = 0x544A; Light = LightType.Empty; 		from.SendSound( 0x208 ); }
			}
		}

		public override bool OnDragLift( Mobile from )
		{
			if ( ItemID != 0x544A ){ 		ItemID = 0x544A; Light = LightType.Empty; 		from.SendSound( 0x208 ); }

			return true;
		}

        public DwarvenForge( Serial serial ) : base( serial )
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