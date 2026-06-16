using System;
using Server;
using Server.Items;
using Server.Targeting;

namespace Server.Items
{
	public class OrbOfTheDemonwebPits : Item
	{
		private static int[] m_Hues = new int[]
		{
			0x0455,
			0x047E,
			0x0497,
			0x07C7,
			0x0845,
			0x08E3,
			0x0967,
			0x09C4,
			0x0B32
		};

		[Constructable]
		public OrbOfTheDemonwebPits() : base( 0x2C84 )
		{
			Name = "Orb of the Demonweb Pits";
			Weight = 1.0;
			Hue = m_Hues[Utility.Random( m_Hues.Length )];
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( !IsChildOf( from.Backpack ) )
			{
				from.SendLocalizedMessage( 1042001 );
				return;
			}

			from.Target = new OrbTarget( this );
			from.SendMessage( "Selecione um item para corromper." );
		}

		private class OrbTarget : Target
		{
			private OrbOfTheDemonwebPits m_Orb;

			public OrbTarget( OrbOfTheDemonwebPits orb ) : base( 1, false, TargetFlags.None )
			{
				m_Orb = orb;
			}

			protected override void OnTarget( Mobile from, object targeted )
			{
				if ( m_Orb == null || m_Orb.Deleted )
					return;

				Item item = targeted as Item;

				if ( item == null )
				{
					from.SendMessage( "Apenas artefatos poderosos podem suportar o toque de Lolth." );
					return;
				}

				bool valid =
					item is BaseGiftArmor ||
					item is BaseLevelArmor ||
					item is BaseLevelClothing ||
					item is BaseGiftClothing;

				if ( !valid )
				{
					from.SendMessage( "Apenas artefatos poderosos podem suportar o toque de Lolth." );
					return;
				}

				if (item is BaseGiftArmor)
                    ((BaseGiftArmor)item).Points += 25;
				if (item is BaseGiftClothing)
                    ((BaseGiftClothing)item).Points += 25;
				if (item is BaseLevelClothing)
                    ((BaseLevelClothing)item).Points += 25;
				if (item is BaseLevelArmor)
                    ((BaseLevelArmor)item).Points += 25;

				item.Hue = m_Orb.Hue;

				if ( from.Karma > 0 )
				{
					if ( from.Karma >= 5000 )
						from.Karma -= 5000;
					else
						from.Karma = 0;
				}

				from.SendMessage( "A influência de Lolth corrompe e fortalece o item." );

				m_Orb.Delete();
			}
		}

		public OrbOfTheDemonwebPits( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int)0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
}