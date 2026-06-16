using System;

namespace Server.Items
{
	public class BaseWoodBoard : Item
	{
		public override string DefaultDescription{ get{ return "This wood is used by carpenters, to create furniture and wooden armor. Bowyers can use these to create bows and arrows as well."; } }

		public override Catalogs DefaultCatalog{ get{ return Catalogs.Crafting; } }

		[Constructable]
		public BaseWoodBoard() : this( 1 )
		{
		}

		[Constructable]
		public BaseWoodBoard( int amount ) : this( CraftResource.RegularWood, amount )
		{
		}

		public BaseWoodBoard( Serial serial ) : base( serial )
		{
		}

		[Constructable]
		public BaseWoodBoard( CraftResource resource ) : this( resource, 1 )
		{
		}

		[Constructable]
		public BaseWoodBoard( CraftResource resource, int amount ) : base( 0x1BD7 )
		{
			Stackable = true;
			Amount = amount;
			Weight = 0.1;
			m_Resource = resource;
			Hue = CraftResources.GetHue( resource );
			Name = CraftResources.GetTradeItemFullName( this, resource, false, false, null );
			Built = true;
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 4 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();

			if ( version < 4 )
				m_Resource = (CraftResource)reader.ReadInt();

			Hue = CraftResources.GetHue( m_Resource );
			Name = CraftResources.GetTradeItemFullName( this, m_Resource, false, false, null );
			Built = true;
		}
	}
	public class Board : BaseWoodBoard
	{
		[Constructable]
		public Board() : this( 1 )
		{
		}

		[Constructable]
		public Board( int amount ) : base( CraftResource.RegularWood, amount )
		{
		}

		public Board( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int)0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
	public class AshBoard : BaseWoodBoard
	{
		[Constructable]
		public AshBoard() : this( 1 )
		{
		}

		[Constructable]
		public AshBoard( int amount ) : base( CraftResource.AshTree, amount )
		{
		}

		public AshBoard( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int)0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
	public class CherryBoard : BaseWoodBoard
	{
		[Constructable]
		public CherryBoard() : this( 1 )
		{
		}

		[Constructable]
		public CherryBoard( int amount ) : base( CraftResource.CherryTree, amount )
		{
		}

		public CherryBoard( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int)0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
	public class EbonyBoard : BaseWoodBoard
	{
		[Constructable]
		public EbonyBoard() : this( 1 )
		{
		}

		[Constructable]
		public EbonyBoard( int amount ) : base( CraftResource.EbonyTree, amount )
		{
		}

		public EbonyBoard( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int)0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
	public class GoldenOakBoard : BaseWoodBoard
	{
		[Constructable]
		public GoldenOakBoard() : this( 1 )
		{
		}

		[Constructable]
		public GoldenOakBoard( int amount ) : base( CraftResource.GoldenOakTree, amount )
		{
		}

		public GoldenOakBoard( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int)0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
	public class HickoryBoard : BaseWoodBoard
	{
		[Constructable]
		public HickoryBoard() : this( 1 )
		{
		}

		[Constructable]
		public HickoryBoard( int amount ) : base( CraftResource.HickoryTree, amount )
		{
		}

		public HickoryBoard( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int)0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
	public class MahoganyBoard : BaseWoodBoard
	{
		[Constructable]
		public MahoganyBoard() : this( 1 )
		{
		}

		[Constructable]
		public MahoganyBoard( int amount ) : base( CraftResource.MahoganyTree, amount )
		{
		}

		public MahoganyBoard( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int)0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
	public class OakBoard : BaseWoodBoard
	{
		[Constructable]
		public OakBoard() : this( 1 )
		{
		}

		[Constructable]
		public OakBoard( int amount ) : base( CraftResource.OakTree, amount )
		{
		}

		public OakBoard( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int)0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
	public class PineBoard : BaseWoodBoard
	{
		[Constructable]
		public PineBoard() : this( 1 )
		{
		}

		[Constructable]
		public PineBoard( int amount ) : base( CraftResource.PineTree, amount )
		{
		}

		public PineBoard( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int)0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
	public class RosewoodBoard : BaseWoodBoard
	{
		[Constructable]
		public RosewoodBoard() : this( 1 )
		{
		}

		[Constructable]
		public RosewoodBoard( int amount ) : base( CraftResource.RosewoodTree, amount )
		{
		}

		public RosewoodBoard( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int)0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
	public class WalnutBoard : BaseWoodBoard
	{
		[Constructable]
		public WalnutBoard() : this( 1 )
		{
		}

		[Constructable]
		public WalnutBoard( int amount ) : base( CraftResource.WalnutTree, amount )
		{
		}

		public WalnutBoard( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int)0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
	public class DriftwoodBoard : BaseWoodBoard
	{
		[Constructable]
		public DriftwoodBoard() : this( 1 )
		{
		}

		[Constructable]
		public DriftwoodBoard( int amount ) : base( CraftResource.DriftwoodTree, amount )
		{
		}

		public DriftwoodBoard( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int)0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}

	public class GhostBoard : BaseWoodBoard
	{
		[Constructable]
		public GhostBoard() : this( 1 )
		{
		}

		[Constructable]
		public GhostBoard( int amount ) : base( CraftResource.GhostTree, amount )
		{
		}

		public GhostBoard( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int)0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
	public class PetrifiedBoard : BaseWoodBoard
	{
		[Constructable]
		public PetrifiedBoard() : this( 1 )
		{
		}

		[Constructable]
		public PetrifiedBoard( int amount ) : base( CraftResource.PetrifiedTree, amount )
		{
		}

		public PetrifiedBoard( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int)0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
	public class ElvenBoard : BaseWoodBoard
	{
		[Constructable]
		public ElvenBoard() : this( 1 )
		{
		}

		[Constructable]
		public ElvenBoard( int amount ) : base( CraftResource.ElvenTree, amount )
		{
		}

		public ElvenBoard( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int)0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
}