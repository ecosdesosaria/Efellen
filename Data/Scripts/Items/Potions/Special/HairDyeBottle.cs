using System;
using Server;
using Server.Mobiles;

namespace Server.Items
{
    public class HairDyeBottle : Item
	{
		public override string DefaultDescription{ get{ return "Estas misturas precisam ser tingidas de alguma cor, antes de usá-las em seu cabelo. Se você não tingir o conteúdo, e em vez disso deixá-lo em uma cor neutra, então a cor do seu cabelo retornará ao que era anteriormente."; } }

		public override Catalogs DefaultCatalog{ get{ return Catalogs.Potion; } }

        [Constructable]
        public HairDyeBottle() : base(0xE0F)
		{
            Name = "hair dye mixture";
			Hue = 0;
			Built = true;
        }

        public override void OnDoubleClick(Mobile from)
		{
			if ( from.RaceID > 0 )
			{
				from.SendMessage( "Você não acha isso realmente útil." );
				return;
			}
			else if ( !IsChildOf( from.Backpack ) ) 
			{
				from.SendMessage( "Isto deve estar em sua mochila para usar." );
				return;
			}
			else if ( this.Hue == 0 )
			{
				from.HairHue = from.RecordHairColor;
				from.FacialHairHue = from.RecordBeardColor;
				from.SendMessage("Você usa a tinta neutra para colorir seu cabelo de volta à cor normal.");
				from.PlaySound( 0x5A4 );
			}
			else
			{
				from.HairHue = this.Hue;
				from.FacialHairHue = this.Hue;
				from.SendMessage("Você tinge seu cabelo de uma nova cor.");
				from.PlaySound( 0x5A4 );
			}
			this.Delete();
        }

        public HairDyeBottle( Serial serial ) : base( serial )
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
			Built = true;
	    }
    }
}