using System;
using Server;
using Server.Gumps; 
using Server.Network; 
using Server.Misc; 
using Server.Mobiles;
using Server.Targeting;
using Server.Items;

namespace Server.Items
{

	public class PetDyeTubTarget : Target
	{
		private PetDyeTub m_Tub;

		public PetDyeTubTarget( PetDyeTub tub ) : base( 12, false, TargetFlags.None )
		{
			m_Tub = tub;
		}

		protected override void OnTarget( Mobile from, object targeted )
		{
			if ( targeted is Item )
			{
				from.SendMessage( "Você não pode tingir itens com isso." );
			}
			else if ( targeted is PlayerMobile )
			{
				from.SendMessage( "Você não pode tingir jogadores com isso." );
			}
			else if ( targeted is BaseCreature )
			{
				BaseCreature targ = (BaseCreature)targeted;
				if( from.InRange( m_Tub.GetWorldLocation(), 3 ) ) 
				{
					if ( targ.Controlled == false )
					{
						from.SendMessage( "Este animal não é domado." );
					}
					else if ( targ.IsDeadPet )
					{
						from.SendMessage( "Você não pode tingir um animal morto." );
					}
					else if ( targ.ControlMaster != from )
					{
						from.SendMessage( "Este não é seu animal de estimação." );
					}
					else
					{
						targ.Hue = m_Tub.DyedHue;
						from.PlaySound( 0x23E ); 
					}
				}
		       		else 
		        	{ 
		            		from.SendLocalizedMessage( 500446 ); // That is too far away. 
		        	}
			}
			else
			{
				from.SendMessage( "Você não pode tingir isso." );
			}
		}
	}

	public class PetDyeTub : DyeTub
	{
		private bool m_Redyable;

		[Constructable]
		public PetDyeTub()
		{
			Weight = 0.0;
			Hue = 0;
			Name = "a pet dye tub";
			m_Redyable = true;
		}
		public PetDyeTub( Serial serial ) : base( serial )
		{
		}

		public override void OnDoubleClick( Mobile from )
		{

			if ( !IsChildOf (from.Backpack))
			{
				from.Target = new PetDyeTubTarget( this );
				from.SendMessage( "Qual animal você deseja tingir?" );
			}	
			else if( from.InRange( this.GetWorldLocation(), 3 ) )
			{
				from.Target = new PetDyeTubTarget( this );
				from.SendMessage( "Qual animal você deseja tingir?" );

			}

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
