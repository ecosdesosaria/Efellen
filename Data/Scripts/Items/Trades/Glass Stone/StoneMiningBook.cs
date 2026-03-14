using System;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
	public class StoneMiningBook : Item
	{
		public override string DefaultName
		{
			get { return "Mining For Quality Stone"; }
		}

		[Constructable]
		public StoneMiningBook() : base( 0xFBE )
		{
			Weight = 1.0;
		}

		public StoneMiningBook( Serial serial ) : base( serial )
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
			else if ( pm == null || from.Skills[SkillName.Mining].Base < 100.0 )
			{
				pm.SendMessage( "Apenas um Minerador Grão-Mestre pode aprender com este livro." );
			}
			else if ( pm.StoneMining )
			{
				pm.SendMessage( "Você já aprendeu esta informação." );
			}
			else
			{
				pm.StoneMining = true;
				pm.SendMessage( "Você aprendeu a minerar pedras. Selecione montanhas ao minerar para procurar pedras." );
				Delete();
			}
		}
	}
}