using System;
using System.Collections;
using Server.Network;
using Server.Targeting;
using Server.Prompts;

namespace Server.Items
{
	public class SkeletonsKey : Item
	{
		public override string DefaultDescription
		{
			get
			{
				if ( Technology )
					return "Estes cartões de acesso podem abrir muitas portas ou baús tecnológicos. Use o cartão de acesso e selecione o item trancado para ver se funciona.";

				return "Estas chaves podem abrir muitas portas ou baús. Use a chave e selecione o item trancado para ver se funciona.";
			}
		}

		public override double DefaultWeight
		{
			get { return 0.1; }
		}

		[Constructable]
		public SkeletonsKey() : base( 0x410A )
		{
			Name = "skeleton key";
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
				from.SendMessage( "Em qual baú ou porta trancada você quer usar a chave?" );
				t = new UnlockTarget( this );
				from.Target = t;
			}
		}

		public override void AddNameProperties( ObjectPropertyList list )
		{
			base.AddNameProperties( list );
			list.Add( 1049644, "Abre quase todos os baús ou portas trancadas" );
		}

		private class UnlockTarget : Target
		{
			private SkeletonsKey m_Key;

			public UnlockTarget( SkeletonsKey key ) : base( 1, false, TargetFlags.None )
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
					from.SendMessage( "Esta chave é para destrancar certos baús." );
				}
				else if ( targeted is BaseHouseDoor )  // house door check
				{
					from.SendMessage( "Esta chave é para destrancar certos baús." );
				}
				else if ( targeted is Item && ((Item)targeted).VirtualContainer )
				{
					from.SendMessage( "Esta chave é para destrancar quase qualquer baú." );
				}
				else if ( targeted is BaseDoor )
				{
					if ( Server.Items.DoorType.IsDungeonDoor( (BaseDoor)targeted ) && m_Key.ItemID != 0x3A75 )
					{
						if ( ((BaseDoor)targeted).Locked == false )
							from.SendMessage( "Isso não precisa ser destrancado." );
						else
						{
							((BaseDoor)targeted).Locked = false;
							Server.Items.DoorType.UnlockDoors( (BaseDoor)targeted );
							from.RevealingAction();
							from.PlaySound( 0x241 );
							m_Key.Consume();
						}
					}
					else
						from.SendMessage( "Isso não precisa ser destrancado." );
				}
				else if ( targeted is ILockable )
				{
					ILockable o = (ILockable)targeted;
					LockableContainer cont2 = (LockableContainer)o;

					if ( Multis.BaseHouse.CheckSecured( cont2 ) ) 
						from.SendLocalizedMessage( 503098 ); // You cannot cast this on a secure item.
					else if ( !cont2.Locked )
						from.LocalOverheadMessage( MessageType.Regular, 0x3B2, 503101 ); // That did not need to be unlocked.
					else if ( cont2.LockLevel == 0 )
						from.SendLocalizedMessage( 501666 ); // You can't unlock that!
					else if ( o.Locked && m_Key.ItemID != 0x3A75 )
					{
						if ( o is BaseDoor && !((BaseDoor)o).UseLocks() )  // this seems to check house doors also
						{
							from.SendMessage( "Esta chave é para destrancar certos baús." );
						}
						else if ( ( cont2.RequiredSkill < 51 ) && !( targeted is TreasureMapChest ) && !( targeted is PirateChest ) && !( targeted is ParagonChest ) )
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

							if ( targeted is Item )
							{
								Item item = (Item)targeted;
								from.SendMessage( "A chave abre a fechadura, desgastando a chave para uso futuro." );
							}

							from.RevealingAction();
							from.PlaySound( 0x241 );
							m_Key.Consume();
						}
						else 
						{
							from.SendMessage( "A fechadura parece muito complicada para esta chave." );
						}
					}
					else
					{
						from.SendMessage( "Você não precisa usar esta chave nisso." );
					}
				}
				else
				{
					from.SendMessage( "Esta chave é para destrancar certos baús." );
				}
			}
		}

		public SkeletonsKey( Serial serial ) : base( serial )
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
}
