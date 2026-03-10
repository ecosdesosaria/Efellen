using System; 
using Server.Network; 
using Server.Prompts; 
using Server.Items; 
using Server.Mobiles;
using Server.Targeting;
using Server.Gumps; 

namespace Server.Items 
{ 
   public class TrainTarget : Target 
   { 
      private PetTrainer m_Deed; 

      public TrainTarget( PetTrainer deed ) : base( 1, false, TargetFlags.None ) 
      { 
         m_Deed = deed; 
      } 

      protected override void OnTarget( Mobile from, object target ) 
      { 
         if ( target is BaseCreature ) 
         { 
            BaseCreature t = ( BaseCreature ) target; 

            if ( t.IsDeadPet == true )
            { 
               from.SendMessage( "Esse ser precisa estar vivo!!!" );
            } 
            else if ( t.ControlMaster != from ) 
            { 
               from.SendMessage( "Esse não é seu animal de estimação!" ); 
            } 
		  else if ( t.IsBonded == false ) 
           				 { 
               				from.SendMessage( "A criatura precisa estar vinculada a você!!!" );
           				 } 
								else if ( t.SkillsTotal >= t.SkillsCap ) 
           						{ 
               						from.SendMessage( "A criatura está no nível máximo de habilidade" );
           						} 
					else if ( from.Skills[SkillName.Taming].Base < 100.0 ) 
           					 	{


						switch ( Utility.Random( 19 ) )

									{
				 						case 0: t.Skills[SkillName.FistFighting].Base += 0.5;
											from.SendMessage( "Seu animal fica mais forte!!!" );
											break;
										case 1: t.Skills[SkillName.Psychology].Base += 0.5;
											from.SendMessage( "Seu animal fica mais forte!!!" );
											break;
										case 2: t.Skills[SkillName.Magery].Base += 0.5;
											from.SendMessage( "Seu animal fica mais forte!!!" );
											break;
										case 3: t.Skills[SkillName.Meditation].Base += 0.5;
											from.SendMessage( "Seu animal fica mais forte!!!" );
											break;
										case 4: t.Skills[SkillName.MagicResist].Base += 0.5;
											from.SendMessage( "Seu animal fica mais forte!!!" );
											break;
										case 5: t.Skills[SkillName.Tactics].Base += 0.5;
											from.SendMessage( "Seu animal fica mais forte!!!" );
											break;
										case 6: t.Skills[SkillName.Poisoning].Base += 0.5;
											from.SendMessage( "Seu animal fica mais forte!!!" );
											break;
										case 7: t.Skills[SkillName.Anatomy].Base += 0.5;
											from.SendMessage( "Seu animal fica mais forte!!!" );
											break;
										case 8: t.Skills[SkillName.FistFighting].Base -= 1;
											from.SendMessage( "Você irrita a fera" );
											t.PlaySound( t.GetAngerSound() );
											t.Direction = t.GetDirectionTo( from );
											break;
										case 9: t.Skills[SkillName.Psychology].Base -= 1;
											from.SendMessage( "Você irrita a fera" );
											t.PlaySound( t.GetAngerSound() );
											t.Direction = t.GetDirectionTo( from );
											break;
										case 10: t.Skills[SkillName.Magery].Base -= 1;
											from.SendMessage( "Você irrita a fera" );
											t.PlaySound( t.GetAngerSound() );
											t.Direction = t.GetDirectionTo( from );
											break;
										case 11: t.Skills[SkillName.Meditation].Base -= 1;
											from.SendMessage( "Você irrita a fera" );
											t.PlaySound( t.GetAngerSound() );
											t.Direction = t.GetDirectionTo( from );
											break;
										case 12: t.Skills[SkillName.MagicResist].Base -= 1;
											from.SendMessage( "Você irrita a fera" );
											t.PlaySound( t.GetAngerSound() );
											t.Direction = t.GetDirectionTo( from );
											break;
										case 13: t.Skills[SkillName.Tactics].Base -= 1;
											from.SendMessage( "Você irrita a fera" );
											t.PlaySound( t.GetAngerSound() );
											t.Direction = t.GetDirectionTo( from );
											break;
										case 14: t.Skills[SkillName.Poisoning].Base -= 1;
											from.SendMessage( "Você irrita a fera" );
											t.PlaySound( t.GetAngerSound() );
											t.Direction = t.GetDirectionTo( from );
											break;
										case 15: t.Skills[SkillName.Anatomy].Base -= 1;
											from.SendMessage( "Você irrita a fera" );
											t.PlaySound( t.GetAngerSound() );
											t.Direction = t.GetDirectionTo( from );
											break;
										case 16: from.SendMessage("Seu Mascote Apenas Olha para Você Timidamente...");
											break;
										case 17: from.SendMessage("Seu Mascote Apenas Olha para Você Timidamente...");
											break;
										case 18: from.SendMessage("Seu Mascote Apenas Olha para Você Timidamente...");
											break;										
										
									}
						
						

						switch ( Utility.Random( 10 ) )
							{
				 				case 0:	t.Combatant = from;
								from.SendMessage( "Você irrita muito a fera!!!" );
								break;
							}
						
           				 } 
           							 else              
               								{ 

							switch ( Utility.Random( 19 ) )

									{
				 						case 0: t.Skills[SkillName.FistFighting].Base += 1;
											from.SendMessage( "Seu animal fica mais forte!!!" );
											break;
										case 1: t.Skills[SkillName.Psychology].Base += 1;
											from.SendMessage( "Seu animal fica mais forte!!!" );
											break;
										case 2: t.Skills[SkillName.Magery].Base += 1;
											from.SendMessage( "Seu animal fica mais forte!!!" );
											break;
										case 3: t.Skills[SkillName.Meditation].Base += 1;
											from.SendMessage( "Seu animal fica mais forte!!!" );
											break;
										case 4: t.Skills[SkillName.MagicResist].Base += 1;
											from.SendMessage( "Seu animal fica mais forte!!!" );
											break;
										case 5: t.Skills[SkillName.Tactics].Base += 1;
											from.SendMessage( "Seu animal fica mais forte!!!" );
											break;
										case 6: t.Skills[SkillName.Poisoning].Base += 1;
											from.SendMessage( "Seu animal fica mais forte!!!" );
											break;
										case 7: t.Skills[SkillName.Anatomy].Base += 1;
											from.SendMessage( "Seu animal fica mais forte!!!" );
											break;
										case 8: t.Skills[SkillName.FistFighting].Base -= 0.5;
											from.SendMessage( "Você irrita a fera" );
											t.PlaySound( t.GetAngerSound() );
											t.Direction = t.GetDirectionTo( from );
											break;
										case 9: t.Skills[SkillName.Psychology].Base -= 0.5;
											from.SendMessage( "Você irrita a fera" );
											t.PlaySound( t.GetAngerSound() );
											t.Direction = t.GetDirectionTo( from );
											break;
										case 10: t.Skills[SkillName.Magery].Base -= 0.5;
											from.SendMessage( "Você irrita a fera" );
											t.PlaySound( t.GetAngerSound() );
											t.Direction = t.GetDirectionTo( from );
											break;
										case 11: t.Skills[SkillName.Meditation].Base -= 0.5;
											from.SendMessage( "Você irrita a fera" );
											t.PlaySound( t.GetAngerSound() );
											t.Direction = t.GetDirectionTo( from );
											break;
										case 12: t.Skills[SkillName.MagicResist].Base -= 0.5;
											from.SendMessage( "Você irrita a fera" );
											t.PlaySound( t.GetAngerSound() );
											t.Direction = t.GetDirectionTo( from );
											break;
										case 13: t.Skills[SkillName.Tactics].Base -= 0.5;
											from.SendMessage( "Você irrita a fera" );
											t.PlaySound( t.GetAngerSound() );
											t.Direction = t.GetDirectionTo( from );
											break;
										case 14: t.Skills[SkillName.Poisoning].Base -= 0.5;
											from.SendMessage( "Você irrita a fera" );
											t.PlaySound( t.GetAngerSound() );
											t.Direction = t.GetDirectionTo( from );
											break;
										case 15: t.Skills[SkillName.Anatomy].Base -= 0.5;
											from.SendMessage( "Você irrita a fera" );
											t.PlaySound( t.GetAngerSound() );
											t.Direction = t.GetDirectionTo( from );
											break;
										case 16: from.SendMessage("Seu Mascote Apenas Olha para Você Timidamente...");
											break;
										case 17: from.SendMessage("Seu Mascote Apenas Olha para Você Timidamente...");
											break;
										case 18: from.SendMessage("Seu Mascote Apenas Olha para Você Timidamente...");
											break;									
										
									}						

							switch ( Utility.Random( 20 ) )

									{
				 						case 0:	t.Combatant = from;
										from.SendMessage( "Você irrita muito a fera!!!" );
										break;
									}
								
              								 } 
            
         } 
         else 
         { 
            from.SendMessage( "Esse não é um alvo válido." ); 
         } 
      } 
   } 

   public class PetTrainer : Item // Create the item class which is derived from the base item class 
   { 
      [Constructable] 
      public PetTrainer() : base( 0x166E ) 
      { 
         Name = "A Pet Trainer"; 
         //LootType = LootType.Blessed;
      } 

      public PetTrainer( Serial serial ) : base( serial ) 
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

      public override void OnDoubleClick( Mobile from ) // Override double click of the deed to call our target 
      { 
         if ( !IsChildOf( from.Backpack ) ) // Make sure its in their pack 
         { 
             from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it. 
         } 
		 if (Utility.RandomDouble() <= 0.1)
		 {
			this.Delete();
			from.SendMessage( "Infelizmente, você quebrou a ferramenta..." );
		 }
         else 
         { 
            from.SendMessage( "Escolha o animal que deseja treinar!!" );  
            from.Target = new TrainTarget( this ); // Call our target 
          } 
      }    
   } 
}