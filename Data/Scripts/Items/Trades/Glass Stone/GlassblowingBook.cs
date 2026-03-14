using System;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
	public class GlassblowingBook : Item
	{
		public override string DefaultName
		{
			get { return "Crafting Glass With Glassblowing"; }
		}

		[Constructable]
		public GlassblowingBook() : base( 0xFF4 )
		{
			Weight = 1.0;
		}

		public GlassblowingBook( Serial serial ) : base( serial )
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

		public override void OnDoubleClick( Mobile from )
		{
			PlayerMobile pm = from as PlayerMobile;

			if ( !IsChildOf( from.Backpack ) )
			{
				from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
			}
			else if ( pm == null || from.Skills[SkillName.Alchemy].Base < 100.0 )
			{
				pm.SendMessage( "Apenas um Alquimista Grão-Mestre pode aprender com este livro." );
			}
			else if ( pm.Glassblowing )
			{
				pm.SendMessage( "Você já aprendeu esta informação." );
			}
			else
			{
				pm.Glassblowing = true;
				pm.SendMessage( "Você aprendeu a fazer itens de vidro. Você precisará encontrar mineradores para minerar areia para você fazer esses itens." );
				Delete();
			}
		}
	}
}