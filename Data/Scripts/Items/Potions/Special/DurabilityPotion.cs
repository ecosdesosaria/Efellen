using System;
using Server.Network;
using Server;
using Server.Targets;
using Server.Targeting;

namespace Server.Items
{
	public class DurabilityPotion : BasePotion
	{
		public override string DefaultDescription{ get{ return "Quando você derrama estas poções em armaduras, roupas ou armas, a durabilidade máxima do item aumenta em 10. Não tem efeito em itens com durabilidade máxima maior que 50."; } }

		[Constructable]
		public DurabilityPotion() : base( 0x180F, PotionEffect.Durability )
		{
			Hue = 0xB7D;
			Name = "durability potion";
		}

		public DurabilityPotion( Serial serial ) : base( serial )
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
	  
	  	public override void Drink( Mobile m )
      	{ 
         	if ( m.InRange( this.GetWorldLocation(), 1 ) ) 
         	{ 
				m.SendMessage( "No que você gostaria de derramar isto!" );
				m.Target = new DurabilityTarget( this, m );
         	} 
         	else 
         	{ 
            	m.LocalOverheadMessage( MessageType.Regular, 906, 1019045 ); // I can't reach that. 
         	} 
		}

		public static void ConsumeCharge( DurabilityPotion potion, Mobile from )
		{
			potion.Consume();
			from.RevealingAction();
			from.PlaySound( 0x23E );
		}

		private class DurabilityTarget : Target
		{
			private DurabilityPotion m_Potion;
			private Mobile m_From;

			public DurabilityTarget( DurabilityPotion potion, Mobile from ) :  base ( 1, false, TargetFlags.None )
			{
				m_Potion = potion;
				m_From = from;
			}

			protected override void OnTarget( Mobile from, object targeted )
			{
				if ( targeted is BaseArmor )
				{
					BaseArmor repairing = (BaseArmor)targeted;
					if ( !repairing.IsChildOf( from.Backpack ) )
					{
						from.SendMessage( "O item precisa estar em sua mochila para usar essa poção nele!" );
					}
					else if ( repairing.MaxHitPoints >= 50 )
					{
						from.SendMessage( "Este item já é suficientemente durável para ser afetado!" );
					}
					else
					{
						from.SendMessage( "Você adiciona à durabilidade ao item!" );
						repairing.MaxHitPoints += 10;
						Server.Items.DurabilityPotion.ConsumeCharge( m_Potion, m_From );
					}
				}
				else if ( targeted is BaseWeapon )
				{
					BaseWeapon repairing2 = (BaseWeapon)targeted;
					if ( !repairing2.IsChildOf( from.Backpack ) )
					{
						from.SendMessage( "O item precisa estar em sua mochila para usar essa poção nele!" );
					}
					else if ( repairing2.MaxHitPoints >= 50 )
					{
						from.SendMessage( "Este item já é suficientemente durável para ser afetado!" );
					}
					else
					{
						from.SendMessage( "Você adiciona à durabilidade ao item!" );
						repairing2.MaxHitPoints += 10;
						Server.Items.DurabilityPotion.ConsumeCharge( m_Potion, m_From );
					}
				}
				else if (targeted is BaseClothing) 
				{
					BaseClothing repairing = (BaseClothing)targeted;
					if ( !repairing.IsChildOf( from.Backpack ) )
					{
						from.SendMessage( "O item precisa estar em sua mochila para usar essa poção nele!" );
					}
					else if ( repairing.MaxHitPoints >= 50 )
					{
						from.SendMessage( "Este item já é suficientemente durável para ser afetado!" );
					}
					else
					{
						from.SendMessage( "Você adiciona à durabilidade ao item!" );
						repairing.MaxHitPoints += 10;
						Server.Items.DurabilityPotion.ConsumeCharge( m_Potion, m_From );
					}
				}
				else if ( targeted is Item )
				{
					from.SendMessage( "Este item não pode ser alterado!" );
				}
				else
				{
					from.SendMessage( "Você não pode fazer isso!" );
				}
			}
		}
	}
}
