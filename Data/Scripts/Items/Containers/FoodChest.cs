using System;
using Server;
using Server.Mobiles;

namespace Server.Items
{
	[Flipable(0xE42, 0xE43)]
    public class FoodChest : Item
	{
		private DateTime m_NextFill;
		public DateTime NextFill{ get{ return m_NextFill; } set{ m_NextFill = value; } }

        [Constructable]
        public FoodChest() : base(0xE42)
		{
            Name = "Food Chest";
        }

        public override void OnDoubleClick(Mobile from)
		{
			if ( from.InRange( this.GetWorldLocation(), 4 ) && DateTime.Now >= m_NextFill )
			{
				m_NextFill = (DateTime.Now + TimeSpan.FromSeconds( 60 ));

				switch( Utility.RandomMinMax( 0, 4 ) )
				{
					case 0: Item jerky = new FoodBeefJerky(); jerky.Amount = Utility.RandomMinMax(2,6); from.AddToBackpack( jerky ); from.SendMessage( "Você pega um pouco de carne seca." ); break;
					case 1: Item bread = new BakedBread(); bread.Amount = Utility.RandomMinMax(2,6); from.AddToBackpack( bread ); from.SendMessage( "Você pega um pouco de pão." ); break;
					case 2: Item toad = new FoodToadStool(); toad.Amount = Utility.RandomMinMax(2,6); from.AddToBackpack( toad ); from.SendMessage( "Você pega alguns cogumelos comestíveis." ); break;
					case 3: Item berry = new FoodImpBerry(); berry.Amount = Utility.RandomMinMax(2,6); from.AddToBackpack( berry ); from.SendMessage( "Você pega algumas frutinhas de diablito." ); break;
					case 4: Item FoodPotato = new FoodPotato(); FoodPotato.Amount = Utility.RandomMinMax(2,6);  from.AddToBackpack( FoodPotato ); from.SendMessage( "Você pega algumas batatas." ); break;
				}
			}
			else if ( from.InRange( this.GetWorldLocation(), 4 ) )
			{
				from.SendMessage( "Você deve esperar um minuto para ver se há comida aqui." );
			}
			else
			{
				from.SendLocalizedMessage( 502138 ); // That is too far away for you to use
			}
        }

        public FoodChest( Serial serial ) : base( serial )
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