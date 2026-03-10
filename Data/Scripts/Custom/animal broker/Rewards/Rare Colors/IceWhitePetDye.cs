using Server.Targeting; 
using System; 
using Server; 
using Server.Gumps; 
using Server.Network; 
using Server.Menus; 
using Server.Menus.Questions; 
using Server.Mobiles; 
using System.Collections; 

namespace Server.Items 
{ 
   	public class IceWhitePetDye : Item 
   	{ 
    
      		[Constructable] 
      		public IceWhitePetDye() : base( 0xE2B ) 
      		{ 
         		Weight = 1.0;  
         		Movable = true;
			Hue = 1153;
         		Name="pet dye (Ice White)"; 
          	} 

      		public IceWhitePetDye( Serial serial ) : base( serial ) 
      		{ 
          
      
      		} 
      		public override void OnDoubleClick( Mobile from ) 
     	 	{ 

			if ( !IsChildOf( from.Backpack ) )
			{
				from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
			}
			else if( from.InRange( this.GetWorldLocation(), 1 ) ) 
		        {
				from.SendMessage( "O que você deseja tingir?" );
           			from.Target = new IceWhiteDyeTarget( this );
		        } 
		        else 
		        { 
		            from.SendLocalizedMessage( 500446 ); // That is too far away. 
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


  		private class IceWhiteDyeTarget : Target 
      		{ 
         		private Mobile m_Owner; 
      
         		private IceWhitePetDye m_Powder; 

         		public IceWhiteDyeTarget( IceWhitePetDye charge ) : base ( 10, false, TargetFlags.None ) 
         		{ 
            			m_Powder=charge; 
         		} 
          
         		protected override void OnTarget( Mobile from, object target ) 
         		{ 

					if ( target == from ) 
						from.SendMessage( "Isso só pode ser usado em animais de estimação." );

					else if ( target is PlayerMobile )
						from.SendMessage( "Você não pode tingir eles." );

					else if ( target is Item )
						from.SendMessage( "Você não pode tingir isso." );

          			else if ( target is BaseCreature ) 
          			{ 
          				BaseCreature c = (BaseCreature)target;	
					if ( c.BodyValue == 400 || c.BodyValue == 401 && c.Controlled == false )
					{
						from.SendMessage( "Você não pode tingir eles." );
					}
					else if ( c.ControlMaster != from && c.Controlled == false )
					{
						from.SendMessage( "Esse não é seu animal de estimação." );
					}
					else if ( c.Controlled == true && c.ControlMaster == from)
					{
						c.Hue = 1153;
						from.SendMessage( 53, "Seu animal agora foi tingido." );
						from.PlaySound( 0x23E );
						m_Powder.Delete();
					}
  
            			}
         		} 
      		} 
   	} 
} 
