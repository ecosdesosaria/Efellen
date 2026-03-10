using System;
using Server;
using Server.Items;
using Server.ContextMenus;
using System.Collections;
using Server.Prompts;
using Server.Mobiles;

namespace Server.Items
{
	public class WoodWellAddon : BaseAddon, IWaterSource
	{
		public override string AddonName{ get{ return "well"; } }

		public int Quantity
		{
			get{ return 500; }
			set{}
		}
		
		public override BaseAddonDeed Deed
		{
			get
			{
				return new WoodWellDeed();
			}
		}

		[Constructable]
		public WoodWellAddon() : this( false )
		{
		}
		
		[Constructable]
		public WoodWellAddon( bool sandstone )
		{
			AddComponent( new WoodWellPiece(this, 8564 ), 0, 1, 20 );
			AddComponent( new WoodWellPiece(this, 7840 ), 0, 1, 8 );
			AddComponent( new WoodWellPiece(this, 4090 ), 0, 1, 13 );
			AddComponent( new WoodWellPiece(this, 3264 ), 0, 1, 0 );
			AddComponent( new WoodWellPiece(this, 9 ), -1, -1, 0 );
			AddComponent( new WoodWellPiece(this, 3264 ), 1, 1, 0 );
			AddComponent( new WoodWellPiece(this, 3244 ), 1, 1, 0 );
			AddComponent( new WoodWellPiece(this, 8561 ), 1, 1, 20 );
			AddComponent( new WoodWellPiece(this, 9 ), 0, -1, 0 );
			AddComponent( new WoodWellPiece(this, 22 ), 0, -1, 0 );
			AddComponent( new WoodWellPiece(this, 3207 ), -1, 1, 0 );
			AddComponent( new WoodWellPiece(this, 3223 ), -1, 1, 0 );
			AddComponent( new WoodWellPiece(this, 3248 ), 1, 0, 8 );
			AddComponent( new WoodWellPiece(this, 3246 ), 1, 0, 3 );
			AddComponent( new WoodWellPiece(this, 4973 ), 1, 0, 6 );
			AddComponent( new WoodWellPiece(this, 4963 ), 1, 0, 0 );
			AddComponent( new WoodWellPiece(this, 8561 ), 1, 0, 20 );
			AddComponent( new WoodWellPiece(this, 8564 ), 0, 0, 20 );
			AddComponent( new WoodWellPiece(this, 9 ), 0, 0, 0 );
			AddComponent( new WoodWellPiece(this, 6039 ), 0, 0, 1 );
			AddComponent( new WoodWellPiece(this, 20 ), 0, 0, 0 );
			AddComponent( new WoodWellPiece(this, 9 ), -1, 0, 0 );
			AddComponent( new WoodWellPiece(this, 21 ), -1, 0, 0 );
			AddComponent( new WoodWellPiece(this, 3203 ), -1, 0, 0 );
			AddComponent( new WoodWellPiece(this, 3223 ), 1, -1, 0 );
		}
		
		public WoodWellAddon( Serial serial ) : base( serial )
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

	// component
	
	public class WoodWellPiece : AddonComponent
	{
		private WoodWellAddon m_woodwell;
		
		[CommandProperty( AccessLevel.GameMaster )]
		public WoodWellAddon woodwell
		{
			get{ return m_woodwell; }
			set{}
		}

		public WoodWellPiece( WoodWellAddon woodwell, int itemid ) : base( itemid )
		{
			m_woodwell = woodwell;
		}
		
		public override void OnDoubleClick( Mobile from )
		{
			if ( from.InRange( m_woodwell.GetWorldLocation(), 4 ) )
			{
				if ( from.Thirst >= 20 )
				{
					from.SendMessage( "Você não está com sede alguma." );
				}
				else
				{
					string msg = null;
					
					if ( m_woodwell == null )
					{
						from.SendMessage( "Debug: Parent was null" );
						return;
					}
					
					switch( Utility.RandomMinMax( 1, 5 ) )
					{
						case 1:  
							msg = "Você bebe até se saciar da água fresca do poço. Os sons tranquilos da água respingando são suavemente musicais.";
							break;
						case 2:  
							msg = "A água revigorante do poço refresca você e acalma sua mente. Você bebe até se saciar.";	
							break;
						case 3:  
							msg = "Você bebe profundamente da água limpa do poço. Os reflexos cintilantes na superfície agitam seus pensamentos.";
							break;
						case 4:  
							msg = "Enquanto você bebe da água, um aroma tentador lembra memórias há muito esquecidas.";
							break;
						case 5:  
							msg = "Você bebe do poço puro e sonhos tranquilos de deleite silvestre passam por sua mente.";
							break;
					}
						
					from.SendMessage( msg );
					
					from.Thirst = 20;
				}
			}
			else
			{
				from.SendMessage( "Aproxime-se." );
			}
		}
		
	
		public WoodWellPiece( Serial serial ) : base( serial )
		{
		}
		
		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			
			writer.Write( (int) 0 ); // version
			
			writer.Write( m_woodwell );
			
		}
		
		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			
			int version = reader.ReadInt();
			
			switch( version )
			{
				case 0: {
					m_woodwell = reader.ReadItem() as WoodWellAddon;
					break;
				}
			}
		}
	}

	public class WoodWellDeed : BaseAddonDeed
	{
		public override BaseAddon Addon
		{
			get
			{
				return new WoodWellAddon();
			}
		}

		[Constructable]
		public WoodWellDeed()
		{
			ItemID = 0xF3A;
			Hue = 0xB97;
			Name = "Well Digging Spade";
		}

		public override void AddNameProperties( ObjectPropertyList list )
		{
			base.AddNameProperties( list );
			list.Add( 1070722, "Digs A Well On Your Land" );
			list.Add( 1049644, "Wood Well" );
		}

		public WoodWellDeed( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( 0 ); // Version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
}