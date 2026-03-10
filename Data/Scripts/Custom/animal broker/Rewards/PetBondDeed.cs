using System;
using Server.Network;
using Server.Prompts;
using Server.Items;
using System.Collections;
using Server.Gumps;
using Server.Targeting;
using Server.Misc;
using Server.Accounting;
using System.Xml;
using Server.Mobiles; 

namespace Server.Items
{
	public class PetBondDeed : Item
	{
		[Constructable]
		public PetBondDeed() : base( 0x14F0 )
		{
			base.Weight = 0;
			//base.LootType = LootType.Blessed;
			base.Name = "a pet bond deed";
		}		

		public override void OnDoubleClick( Mobile from )
		{
			if ( IsChildOf( from.Backpack ) )
			{
				from.Target = new InternalTarget(from, this);
			}
			else
			{
				from.SendMessage("Isso precisa estar na sua mochila, bobinho.");
			}
		}
		
		public PetBondDeed( Serial serial ) : base( serial )
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
	
		public class InternalTarget : Target
		{
			private Mobile m_From;
			private PetBondDeed m_Deed;
			
			public InternalTarget( Mobile from, PetBondDeed deed ) :  base ( 3, false, TargetFlags.None )
			{
				m_Deed = deed;
				m_From = from;
				from.SendMessage("Com qual animal você quer se vincular?");
		
				
			}
			
			protected override void OnTarget( Mobile from, object targeted )
			{
				
				if (m_Deed.IsChildOf( m_From.Backpack ) )
				{					
					if ( targeted is Mobile )
					{
						if ( targeted is BaseCreature )
						{
							BaseCreature creature = (BaseCreature)targeted;
							if( !creature.Tamable ){
								from.SendMessage("Este animal não é domável!");
							}
							else if(  !creature.Controlled || creature.ControlMaster != from ){
								from.SendMessage("Não é seu animal de estimação!");
							}
							else if( creature.IsDeadPet ){
								from.SendMessage("Este animal está morto... ");
							}
							else if ( creature.Summoned ){
								from.SendMessage("Este animal é invocado");
							}
							else if ( creature.Body.IsHuman ){
								from.SendMessage("Você quer se vincular a um humanoide?? Hmm... tente num quarto.");
							}
							else{	
								
								if( creature.IsBonded == true ){
									from.SendMessage("Tentando vincular o animal....");
								}
								else{
									
									if( from.Skills[SkillName.Taming].Base  < creature.MinTameSkill ){
										from.SendMessage("Sua habilidade é muito baixa para controlar este animal!");
									}
									else if( from.Skills[SkillName.Druidism].Base  < creature.MinTameSkill ){
											from.SendMessage("Sua habilidade é muito baixa para controlar este animal!");
										}
									else{
										try{
											creature.IsBonded = true;
											from.SendMessage("{0} agora está vinculado a você!",creature.Name);
											m_Deed.Delete();
										}
										catch{
											from.SendMessage("Houve um problema ao vincular este animal..");
										}
											
									}
								}
							}							
						}
						else{
							from.SendMessage("Você pode bondar somente animais");
						}
					}
					else{
							from.SendMessage("Você pode bondar somente animais");
						}
				}
				else{
					from.SendMessage("Isso precisa estar na sua mochila, bobinho.");
				}			
		}
	}
}
