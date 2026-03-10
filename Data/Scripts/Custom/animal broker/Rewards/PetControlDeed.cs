//Amherst Script
using System; 
using Server.Network; 
using Server.Prompts; 
using Server.Items; 
using Server.Mobiles;
using Server.Targeting; 
using Server.Misc;

namespace Server.Items 
{ 
   	public class PetControlTarget : Target 
   	{ 
      	private PetControlDeed m_Deed; 

      	public PetControlTarget( PetControlDeed deed ) : base( 1, false, TargetFlags.None ) 
      	{ 
         	m_Deed = deed; 
      	} 

      	protected override void OnTarget( Mobile from, object target ) 
      	{ 
         	if ( target is BaseCreature ) 
			{ 
				BaseCreature k = ( BaseCreature ) target; 

				if ( !k.Tamable )
				{ 
					from.SendMessage( "Esse animal não é domável..." );
				} 
				else if ( from.Skills[SkillName.Taming].Base  < k.MinTameSkill )
				{ 
					from.SendMessage( "Você não tem chance de controlar este animal..." );
				} 
				else if ( k.ControlMaster != null ) 
				{ 
					from.SendMessage( "Isso já está domado!" ); 
				}
					
				else  
				 
					{ 

					k.Controlled = true;
					k.SetControlMaster( from );
					k.ControlOrder = OrderType.Follow; 
					from.SendMessage( "Você doma o animal instantaneamente!" );
					m_Deed.Delete(); // Delete the deed 
					} 

			} 
         else 
         { 
            from.SendMessage( "Esse não é um alvo válido." );
         } 
      } 
   } 

   	public class PetControlDeed : Item
   	{ 
      		[Constructable] 
      		public PetControlDeed() : base( 0x14F0 ) 
      		{ 
			Weight = 1.0; 
			Name = "a pet control deed";
			Hue = 598;
      		} 

      	public PetControlDeed( Serial serial ) : base( serial ) 
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
 
         	//LootType = LootType.Blessed; 

         	int version = reader.ReadInt(); 
      	} 

      	public override bool DisplayLootType{ get{ return false; } } 

      	public override void OnDoubleClick( Mobile from ) 
      	{ 
		if ( !IsChildOf( from.Backpack ) ) // Make sure its in their pack 
		{ 
             		from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it. 
         	} 
         	else 
         	{ 
            		from.SendMessage( "Escolha o animal que você deseja controlar." );  
            		from.Target = new PetControlTarget( this ); 
          	} 
      	}    
} 
}