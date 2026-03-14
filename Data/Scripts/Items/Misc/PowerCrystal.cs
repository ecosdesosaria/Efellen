using Server;
using System;
using System.Collections;
using Server.Network;
using Server.Targeting;
using Server.Prompts;
using Server.Misc;

namespace Server.Items
{
	public class PowerCrystal : Item
	{
		public override string DefaultName
		{
			get { return "power crystal"; }
		}

		[Constructable]
		public PowerCrystal() : base( 0x1F1C )
		{
			Weight = 1.0;
		}

		public PowerCrystal( Serial serial ) : base( serial )
		{
		}

		public override void OnDoubleClick( Mobile from )
		{
			Target t;

			if ( !IsChildOf( from.Backpack ) )
			{
				from.SendLocalizedMessage( 1060640 ); // The item must be in your backpack to use it.
			}
			else
			{
				from.SendMessage( "No que você quer usar o cristal de poder?" );
				t = new PowerTarget( this );
				from.Target = t;
			}
		}

		private class PowerTarget : Target
		{
			private PowerCrystal m_Crystal;

			public PowerTarget( PowerCrystal crystal ) : base( 1, false, TargetFlags.None )
			{
				m_Crystal = crystal;
			}

			protected override void OnTarget( Mobile from, object targeted )
			{
				Item iCrystal = targeted as Item;

				if ( iCrystal is GolemPorterItem )
				{
					GolemPorterItem xCrystal = (GolemPorterItem)iCrystal;

					int myCharges = xCrystal.m_Charges;

					if ( !iCrystal.IsChildOf( from.Backpack ) )
					{
						from.SendMessage( "Você só pode usar este cristal em itens do seu inventário." );
					}
					else if ( myCharges < 100 )
					{
						int UpMe = 5;
						if ( xCrystal.PorterType > 0 ){ UpMe = 1; }

						xCrystal.m_Charges = xCrystal.m_Charges + UpMe;

						if ( xCrystal.m_Charges > 100 ){ xCrystal.m_Charges = 100; }

						from.SendMessage( "Você carrega seu golem com o cristal de poder." );
						from.RevealingAction();
						from.PlaySound( 0x652 );

						xCrystal.InvalidateProperties();

						m_Crystal.Delete();
					}
					else
					{
						from.SendMessage( "Esse golem já está totalmente carregado." );
					}
				}
				else
				{
					from.SendMessage( "Você não acha que isso vai realmente fazer alguma coisa." );
				}
			}
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}