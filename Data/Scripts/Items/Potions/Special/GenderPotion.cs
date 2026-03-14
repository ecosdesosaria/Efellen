using System;
using Server;
using Server.Mobiles;

namespace Server.Items
{
    public class GenderPotion : Item
	{
		public override string DefaultDescription{ get{ return "Beber esta poção transformará um humano de homem para mulher, ou de mulher para homem."; } }

		public override Catalogs DefaultCatalog{ get{ return Catalogs.Potion; } }

        [Constructable]
        public GenderPotion() : base( 0x1FDC )
		{
            Name = "potion of gender change";
			Hue = 0xB46;
			Built = true;
        }

        public override void OnDoubleClick( Mobile from )
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
			else if ( from.Body == 0x191 )
			{
				int HairColor = from.HairHue;
				from.Body = 0x190;
				from.BodyValue = 0x190;
				from.Female = false;
				Utility.AssignRandomHair( from );
				from.FacialHairItemID = Utility.RandomList( 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 8254, 8255, 8256, 8257, 8267, 8268, 8269 );
				from.HairHue = HairColor;
				from.FacialHairHue = HairColor;
				from.RecordsHair( true );
				from.SendMessage("Seu corpo se transforma no de um homem.");
			}
			else if ( from.Body == 0x190 )
			{
				int HairColor = from.HairHue;
				from.Body = 0x191;
				from.BodyValue = 0x191;
				from.Female = true;
				Utility.AssignRandomHair( from );
				from.FacialHairItemID = 0;
				from.HairHue = HairColor;
				from.FacialHairHue = HairColor;
				from.RecordsHair( true );
				from.SendMessage("Seu corpo se transforma no de uma mulher.");
			}
			else
			{
				from.SendMessage("Beber a poção parece não fazer nada.");
			}
			from.PlaySound( Utility.RandomList( 0x30, 0x2D6 ) );
			this.Delete();
			from.AddToBackpack( new Bottle() );
        }

        public GenderPotion( Serial serial ) : base( serial )
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