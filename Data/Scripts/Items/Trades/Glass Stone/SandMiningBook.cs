using System;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
	public class SandMiningBook : Item
	{
		public override string DefaultName
		{
			get { return "Find Glass-Quality Sand"; }
		}

		[Constructable]
		public SandMiningBook() : base( 0xFF4 )
		{
			Weight = 1.0;
		}

		public SandMiningBook( Serial serial ) : base( serial )
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
			else if ( pm.SandMining )
			{
				pm.SendMessage( "Você já aprendeu esta informação." );
			}
			else
			{
				pm.SandMining = true;
				pm.SendMessage( "Você aprendeu como minerar areia de qualidade. Selecione áreas de areia ao minerar para procurar areia de qualidade." );
				Delete();
			}
		}
	}
}