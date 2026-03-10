using System;
using Server;
using Server.Network;
using Server.Mobiles;
using Server.Items;
using System.Collections.Generic;
using Server.Misc;
using System.Collections;
using Server.Targeting;
using Server.Gumps;

namespace Server.Items
{
    public class MonsterManual : Item
	{
        [Constructable]
        public MonsterManual() : base( 0x301E )
		{
            Name = "Monster Manual";
			Weight = 1.0;
        }

        public override void AddNameProperties(ObjectPropertyList list)
		{
            base.AddNameProperties(list);
            list.Add( 1049644, "Dungeons & Dragons");
        }

        public override void OnDoubleClick( Mobile from )
		{
			from.SendMessage( "Qual criatura você quer pesquisar?" );
			Target t = new BookTarget( this );
			from.Target = t;
			from.SendSound( 0x55 );
        }

		private class BookTarget : Target
		{
			private MonsterManual m_Book;

			public BookTarget( MonsterManual researched ) : base( 12, true, TargetFlags.None )
			{
				m_Book = researched;
			}

			protected override void OnTarget( Mobile from, object targeted )
			{
				if ( targeted is PlayerMobile )
				{
					from.SendMessage( "Você provavelmente precisaria do Manual do Jogador para isso." );
				}
				else if ( targeted is HenchmanMonster || targeted is HenchmanWizard || targeted is HenchmanFighter || targeted is HenchmanArcher )
				{
					from.SendMessage( "Estes ajudantes não iriam querer o escrutínio." );
				}
				else if (	targeted is BaseVendor || targeted is BasePerson || targeted is Citizens || targeted is PackBeast || 
							targeted is FrankenPorter || targeted is FrankenFighter || targeted is HenchmanFamiliar || targeted is AerialServant || 
							targeted is GolemPorter || targeted is Robot || targeted is GolemFighter || targeted is HenchmanArcher || 
							targeted is HenchmanMonster || targeted is HenchmanFighter || targeted is HenchmanWizard )
				{
					from.SendMessage( "Eles não parecem estar neste livro." );
				}
				else if ( targeted is Mobile )
				{
					Mobile m = (Mobile)targeted;

					if ( Server.Items.PlayersHandbook.IsPeople( m ) )
					{
						from.SendMessage( "Você provavelmente precisaria do Manual do Jogador para isso." );
					}
					else if ( m is BaseCreature )
					{
						BaseCreature c = (BaseCreature)m;
						from.CloseGump( typeof( Server.SkillHandlers.DruidismGump ) );
						from.SendGump( new Server.SkillHandlers.DruidismGump( from, c, 1 ) );
						from.SendSound( 0x55 );
					}
					else
					{
						from.SendMessage( "Isso não parece estar neste livro." );
					}
				}
				else
				{
					from.SendMessage( "Isso não parece estar neste livro." );
				}
			}
		}

        public MonsterManual( Serial serial ) : base( serial )
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