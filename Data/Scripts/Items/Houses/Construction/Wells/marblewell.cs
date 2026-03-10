using System;
using Server;
using Server.Items;
using Server.ContextMenus;
using System.Collections;
using Server.Prompts;
using Server.Mobiles;

namespace Server.Items
{
	public class MarbleWellAddon : BaseAddon, IWaterSource
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
				return new MarbleWellDeed();
			}
		}

		[Constructable]
		public MarbleWellAddon() : this( false )
		{
		}
		
		[Constructable]
		public MarbleWellAddon( bool sandstone )
		{
			AddComponent( new MarbleWellPiece(this, 6012 ), -1, 1, 1 );
			AddComponent( new MarbleWellPiece(this, 3248 ), -1, 1, 1 );
			AddComponent( new MarbleWellPiece(this, 272 ), -1, 0, 1 );
			AddComponent( new MarbleWellPiece(this, 269 ), -1, 0, 4 );
			AddComponent( new MarbleWellPiece(this, 3208 ), -1, 0, 1 );
			AddComponent( new MarbleWellPiece(this, 269 ), -1, -1, 4 );
			AddComponent( new MarbleWellPiece(this, 271 ), 0, -1, 1 );
			AddComponent( new MarbleWellPiece(this, 269 ), 0, -1, 4 );
			AddComponent( new MarbleWellPiece(this, 4090 ), 0, -1, 0 );
			AddComponent( new MarbleWellPiece(this, 3250 ), 1, -1, 1 );
			AddComponent( new MarbleWellPiece(this, 3241 ), 1, -1, 1 );
			AddComponent( new MarbleWellPiece(this, 10420 ), 0, 1, 16 );
			AddComponent( new MarbleWellPiece(this, 3245 ), 0, 1, 1 );
			AddComponent( new MarbleWellPiece(this, 10419 ), 1, 1, 16 );
			AddComponent( new MarbleWellPiece(this, 4972 ), 1, 1, 1 );
			AddComponent( new MarbleWellPiece(this, 3246 ), 1, 1, 1 );
			AddComponent( new MarbleWellPiece(this, 10419 ), 1, 0, 16 );
			AddComponent( new MarbleWellPiece(this, 4963 ), 1, 0, 1 );
			AddComponent( new MarbleWellPiece(this, 4090 ), 1, 0, 8 );
			AddComponent( new MarbleWellPiece(this, 7840 ), 1, 0, 3 );
			AddComponent( new MarbleWellPiece(this, 6814 ), 1, 0, 1 );
			AddComponent( new MarbleWellPiece(this, 270 ), 0, 0, 1 );
			AddComponent( new MarbleWellPiece(this, 269 ), 0, 0, 4 );
			AddComponent( new MarbleWellPiece(this, 6039 ), 0, 0, 1 );
			AddComponent( new MarbleWellPiece(this, 10420 ), 0, 0, 16 );
			AddComponent( new MarbleWellPiece(this, 3247 ), 2, 0, 1 );

		}
		
		public MarbleWellAddon( Serial serial ) : base( serial )
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
	
	public class MarbleWellPiece : AddonComponent
	{
		private MarbleWellAddon m_marblewell;
		
		[CommandProperty( AccessLevel.GameMaster )]
		public MarbleWellAddon marblewell
		{
			get{ return m_marblewell; }
			set{}
		}

		public MarbleWellPiece( MarbleWellAddon marblewell, int itemid ) : base( itemid )
		{
			m_marblewell = marblewell;
		}
		
		public override void OnDoubleClick( Mobile from )
		{
			if ( from.InRange( m_marblewell.GetWorldLocation(), 4 ) )
			{
				if ( from.Thirst >= 20 )
				{
					from.SendMessage( "Você não está com sede alguma." );
				}
				else
				{
					string msg = null;
					
					if ( m_marblewell == null )
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
		
	
		public MarbleWellPiece( Serial serial ) : base( serial )
		{
		}
		
		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			
			writer.Write( (int) 0 ); // version
			
			writer.Write( m_marblewell );
			
		}
		
		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			
			int version = reader.ReadInt();
			
			switch( version )
			{
				case 0: {
					m_marblewell = reader.ReadItem() as MarbleWellAddon;
					break;
				}
			}
		}
	}

	public class MarbleWellDeed : BaseAddonDeed
	{
		public override BaseAddon Addon
		{
			get
			{
				return new MarbleWellAddon();
			}
		}

		[Constructable]
		public MarbleWellDeed()
		{
			ItemID = 0xF3A;
			Hue = 0xB97;
			Name = "Well Digging Spade";
		}

		public override void AddNameProperties( ObjectPropertyList list )
		{
			base.AddNameProperties( list );
			list.Add( 1070722, "Digs A Well On Your Land" );
			list.Add( 1049644, "Marble Well" );
		}

		public MarbleWellDeed( Serial serial ) : base( serial )
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