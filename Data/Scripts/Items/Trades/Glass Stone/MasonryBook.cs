using System;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
	public class MasonryBook : Item
	{
		public override string DefaultName
		{
			get { return "Making Valuables With Stonecrafting"; }
		}

		[Constructable]
		public MasonryBook() : base( 0xFBE )
		{
			Weight = 1.0;
		}

		public MasonryBook( Serial serial ) : base( serial )
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
			else if ( pm == null || from.Skills[SkillName.Carpentry].Base < 100.0 )
			{
				pm.SendMessage( "Apenas um Carpinteiro Grão-Mestre pode aprender com este livro." );
			}
			else if ( pm.Masonry )
			{
				pm.SendMessage( "Você já aprendeu esta informação." );
			}
			else
			{
				pm.Masonry = true;
				pm.SendMessage( "Você aprendeu a fazer itens de pedra. Você precisará encontrar mineradores para minerar pedras para você fazer esses itens." );
				Delete();
			}
		}
	}
}