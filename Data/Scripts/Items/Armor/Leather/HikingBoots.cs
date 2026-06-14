using System;
using System.Collections;
using Server;
using Server.Network;

namespace Server.Items
{
	public class HikingBoots : LeatherBoots
	{
		[Constructable]
		public HikingBoots()
		{
			Name = "hiking boots";
			ItemID = 0x2FC4;
			CoinPrice = 5;
		}

		public override bool OnEquip( Mobile from )
		{
			Console.WriteLine("=== HikingBoots OnEquip ===");
			Console.WriteLine("RaceID: " + from.RaceID);
			Console.WriteLine("Is Drow? " + (from.RaceID == 605 || from.RaceID == 606));
			
			if ( from.RaceID > 0 && from.RaceID != 605 && from.RaceID != 606 )
			{
				Console.WriteLine("Path: Monster (non-Drow) - giving mount speed");
				if ( MySettings.S_NoMountsInCertainRegions && Server.Mobiles.AnimalTrainer.IsNoMountRegion( from, Region.Find( from.Location, from.Map ) ) )
				{
					Weight = 5.0;
					from.Send(SpeedControl.Disable);
				}
				else
				{
					Weight = 3.0;
					from.Send(SpeedControl.MountSpeed);
				}
			}
			else
			{
				Console.WriteLine("Path: Human or Drow - NO speed control");
				Weight = 3.0;
				// Don't send any SpeedControl
			}
			
			return base.OnEquip(from);
		}

		public override void OnRemoved( object parent )
		{
			Console.WriteLine("=== HikingBoots OnRemoved ===");
			if ( parent is Mobile )
			{
				Mobile from = (Mobile)parent;
				Console.WriteLine("RaceID: " + from.RaceID);
				
				if ( from.RaceID > 0 && from.RaceID != 605 && from.RaceID != 606 )
				{
					Console.WriteLine("Resetting speed for monster");
					from.Send(SpeedControl.Disable);
				}
				else
				{
					Console.WriteLine("Human or Drow - no reset needed");
				}
			}
			base.OnRemoved(parent);
		}

		public HikingBoots( Serial serial ) : base( serial )
		{
		}
		
		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 );
		}
		
		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();

			if ( !MyServerSettings.MonstersAllowed() )
				this.Delete();
		}
	}
}