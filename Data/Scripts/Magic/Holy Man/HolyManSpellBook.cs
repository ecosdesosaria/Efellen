using System;
using Server.Network;
using Server.Gumps;
using Server.Spells;

namespace Server.Items
{
	[FlipableAttribute( 0x672B, 0x672C )]
	public class HolyManSpellbook : Spellbook
	{
		public override string DefaultDescription{ get{ return "Este livro sagrado pode conter magia usada por sacerdotes. Preencher suas páginas só pode ser alcançado encontrando os locais de descanso de pessoas virtuosas de muito tempo atrás."; } }

		public Mobile owner;

		[CommandProperty( AccessLevel.GameMaster )]
		public Mobile Owner { get{ return owner; } set{ owner = value; } }

		public override SpellbookType SpellbookType{ get{ return SpellbookType.HolyMan; } }
		public override int BookOffset{ get{ return 770; } }
		public override int BookCount{ get{ return 15; } }

		[Constructable]
		public HolyManSpellbook() : this( 0, null )
		{
		}

		[Constructable]
		public HolyManSpellbook( ulong content, Mobile gifted ) : base( content, 0x672B )
		{
			Layer = Layer.Invalid;

			owner = gifted;

			string sGood = "Santo";
			switch ( Utility.RandomMinMax( 0, 7 ) ) 
			{
				case 0: sGood = "Santo";		break;
				case 1: sGood = "Divino";		break;
				case 2: sGood = "Espiritual";	break;
				case 3: sGood = "Fiel";		break;
				case 4: sGood = "Angélico";		break;
				case 5: sGood = "Virtuoso";		break;
				case 6: sGood = "Abençoado";	break;
				case 7: sGood = "Devoto";		break;
			}

			switch ( Utility.RandomMinMax( 1, 2 ) ) 
			{
				case 1: this.Name = "Livro de Preces " + sGood;	break;
				case 2: this.Name = "Tomo de Preces " + sGood;	break;
			}
		}

		public override void OnDoubleClick( Mobile from )
		{
			Container pack = from.Backpack;

			if ( owner != from )
			{
				from.SendMessage( "Estas páginas parecem rabiscos para você." );
			}
			else if ( Parent == from || ( pack != null && Parent == pack ) )
			{
				from.SendSound( 0x55 );
				from.CloseGump( typeof( HolyManSpellbookGump ) );
				from.SendGump( new HolyManSpellbookGump( from, this, 1 ) );
			}
			else from.SendLocalizedMessage(500207); // The spellbook must be in your backpack (and not in a container within) to open.
		}

        public override void AddNameProperties(ObjectPropertyList list)
		{
            base.AddNameProperties(list);
			if ( owner != null ){ list.Add( 1070722, "For " + owner.Name + "" ); }
        }

		public HolyManSpellbook( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 ); // version
			writer.Write( (Mobile)owner);
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
			owner = reader.ReadMobile();

			if ( ItemID != 0x672B && ItemID != 0x672C )
				ItemID = 0x672B;
		}
	}
}
