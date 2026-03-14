using System;
using System.Collections;
using Server.Network;
using Server.Targeting;
using Server.Prompts;

namespace Server.Items
{
	public class BottleOfAcid : Item
	{
		public override Catalogs DefaultCatalog{ get{ return Catalogs.Potion; } }

		public override string DefaultDescription{ get{ return "Estas garrafas de ácido podem não só corroer quase qualquer recipiente trancado, mas também destruir quaisquer armadilhas neles."; } }

		public override int Hue{ get { return 1167; } }

		public override double DefaultWeight
		{
			get { return 1.0; }
		}

		[Constructable]
		public BottleOfAcid() : base( 0x180F )
		{
			Name = "bottle of acid";
			Stackable = true;
			Built = true;
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
				from.SendMessage( "Em qual baú você quer usar o ácido?" );
				t = new UnlockTarget( this );
				from.Target = t;
			}
		}

		private class UnlockTarget : Target
		{
			private BottleOfAcid m_Key;

			public UnlockTarget( BottleOfAcid key ) : base( 1, false, TargetFlags.None )
			{
				m_Key = key;
				CheckLOS = true;
			}

			protected override void OnTarget( Mobile from, object targeted )
			{
				if ( !m_Key.IsChildOf( from.Backpack ) )
				{
					from.SendLocalizedMessage( 1060640 ); // The item must be in your backpack to use it.
				}
				else if ( targeted == m_Key )
				{
					from.SendMessage( "Este ácido é para dissolver fechaduras e armadilhas na maioria dos baús." );
				}
				else if ( targeted is BaseHouseDoor )  // house door check
				{
					from.SendMessage( "Este ácido é para dissolver fechaduras e armadilhas na maioria dos baús." );
				}
				else if ( targeted is Item && ((Item)targeted).VirtualContainer )
				{
					from.SendMessage( "Esta chave serve para destrancar quase qualquer recipiente." );
				}
				else if ( targeted is BaseDoor )
				{
					if ( Server.Items.DoorType.IsDungeonDoor( (BaseDoor)targeted ) )
					{
						if ( ((BaseDoor)targeted).Locked == false )
							from.SendMessage( "Isso não precisa ser destrancado." );

						else
						{
							((BaseDoor)targeted).Locked = false;
							Server.Items.DoorType.UnlockDoors( (BaseDoor)targeted );
							from.RevealingAction();
							from.PlaySound( 0x231 );
							if ( m_Key.ItemID == 0x1007 ){ from.AddToBackpack( new Jar() ); } else { from.AddToBackpack( new Bottle() ); }
							m_Key.Consume();
						}
					}
					else
						from.SendMessage( "Isso não precisa ser destrancado." );
				}
				else if ( targeted is Head )
				{
					if ( ((Item)targeted).ItemID == 7584 || ((Item)targeted).ItemID == 7393 )
					{
						from.RevealingAction();
						from.PlaySound( 0x231 );
						if ( m_Key.ItemID == 0x1007 ){ from.AddToBackpack( new Jar() ); } else { from.AddToBackpack( new Bottle() ); }
						m_Key.Consume();
						((Item)targeted).ItemID = 0x1AE0;
						if ( (((Item)targeted).Name).Contains(" head ") ){ (((Item)targeted).Name) = (((Item)targeted).Name).Replace(" head ", " skull "); }
						from.SendMessage( "O ácido derrete a pele, deixando apenas um crânio." );
					}
					else
					{
						from.SendMessage( "Alguém já usou ácido para derreter a pele." );
					}
				}
				else if ( targeted is ILockable )
				{
					ILockable o = (ILockable)targeted;
					LockableContainer cont2 = (LockableContainer)o;
					TrapableContainer cont3 = (TrapableContainer)o;

					if ( ( o.Locked ) || ( cont3.TrapType != TrapType.None ) )
					{
						if ( o is BaseDoor && !((BaseDoor)o).UseLocks() )  // this seems to check house doors also
						{
							from.SendMessage( "Este ácido serve para dissolver fechaduras e armadilhas na maioria dos baús." );
						}
						else if ( targeted is TreasureMapChest )
						{
							from.SendMessage( "O ácido parece não ter feito nada ao mecanismo interno." );
							m_Key.Consume();
						}
						else if ( 100 >= cont2.RequiredSkill )
						{
							o.Locked = false;

							if ( o is LockableContainer )
							{
								LockableContainer cont = (LockableContainer)o;
								if ( cont.LockLevel == -255 )
								{
									cont.LockLevel = cont.RequiredSkill - 10;
									if ( cont.LockLevel == 0 )
										cont.LockLevel = -1;
								}

								cont.Picker = from;  // sets "lockpicker" to the user.
							}

							if ( o is TrapableContainer )
							{
								TrapableContainer cont = (TrapableContainer)o;

								if ( cont.TrapType != TrapType.None )
									cont.TrapType = TrapType.None;
							}

							if ( targeted is Item )
							{
								Item item = (Item)targeted;
								from.SendMessage( "O ácido parece ter corroído o mecanismo interno." );
							}

							from.RevealingAction();
							from.PlaySound( 0x231 );
							if ( m_Key.ItemID == 0x1007 ){ from.AddToBackpack( new Jar() ); } else { from.AddToBackpack( new Bottle() ); }
							m_Key.Consume();
						}
						else if ( ( cont3.TrapType != TrapType.None ) && ( cont3.TrapLevel > 0 ) ) 
						{
							if ( o is TrapableContainer )
							{
								TrapableContainer cont = (TrapableContainer)o;

								if ( cont.TrapType != TrapType.None )
									cont.TrapType = TrapType.None;
							}

							if ( targeted is Item )
							{
								Item item = (Item)targeted;
								from.SendMessage( "O ácido parece ter corroído o mecanismo interno." );
							}

							from.RevealingAction();
							from.PlaySound( 0x231 );
							if ( m_Key.ItemID == 0x1007 ){ from.AddToBackpack( new Jar() ); } else { from.AddToBackpack( new Bottle() ); }
							m_Key.Consume();
						}
						else 
						{
							from.SendMessage( "O ácido parece não ter feito nada ao mecanismo interno." );
							m_Key.Consume();
						}
					}
					else
					{
						from.SendMessage( "Você não precisa usar ácido nisso." );
					}
				}
				else
				{
					from.SendMessage( "Este ácido serve para dissolver fechaduras e armadilhas na maioria dos baús." );
				}
			}
		}

		public BottleOfAcid( Serial serial ) : base( serial )
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
			Built = true;
		}
	}
}