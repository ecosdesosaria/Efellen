using System;
using System.Collections;
using Server.Network;
using Server.Targeting;
using Server.Prompts;

namespace Server.Items
{
	public class EvilSkull : Item
	{
		public override string DefaultDescription{ get{ return "Estes crânios são encontradas nos restos mortais finais de magos esqueletais. Se você usá-lo, ele se desfará em pó que você inalará, restaurando sua mana. Fazer isso, no entanto, será um ato bastante vil e seu karma será afetado."; } }

		public override Catalogs DefaultCatalog{ get{ return Catalogs.Potion; } }

		[Constructable]
		public EvilSkull() : base( 0x1AE0 )
		{
			ItemID = Utility.RandomList( 0x1AE0, 0x1AE1, 0x1AE2, 0x1AE3 );
			Name = "evil skull";
			Weight = 1.0;
			Built = true;
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( !IsChildOf( from.Backpack ) ) 
			{
				from.SendMessage( "Isto deve estar em sua mochila para usar." );
				return;
			}
			else
			{
				if ( from.Mana < from.ManaMax )
				{
					from.SendMessage( "O crânio se desfaz em pó, restaurando magicamente sua mana." );
					from.Mana = from.ManaMax;
					from.PlaySound( 0x1FA );
					Misc.Titles.AwardKarma( from, -100, true );
				}
				else
				{
					from.SendMessage( "O crânio se desfaz em pó." );
				}

				this.Delete();
			}
		}

		public EvilSkull(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int) 0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
			Built = true;
		}
	}
}